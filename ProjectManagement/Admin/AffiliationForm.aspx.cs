using Newtonsoft.Json;
using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    public partial class AffiliationForm : System.Web.UI.Page
    {
        IProjectTrackerRepository _repository = new ProjectTrackerRepository();
     
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.IsInRole("Admin"))
            {
                string affiliationId = Request.QueryString["Id"];

                if (!Page.IsPostBack)
                {
                    BindControl(affiliationId);
                }
            }
        }

        private void BindControl(string affiliationId)
        {
            List<JabsomAffil> affiliations = _repository.GetAffiliations(affiliationId);
            rptAffiliation.DataSource = affiliations;
            rptAffiliation.DataBind();

            var category = affiliations.Select(a => a.Type).Distinct();
            ddlCategory.DataSource = category;
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("", String.Empty));

            ddlCategoryEdit.DataSource = category;
            ddlCategoryEdit.DataBind();
            ddlCategoryEdit.Items.Insert(0, new ListItem("", String.Empty));
        }

        protected void rptAffiliation_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                var arg = string.Format("{0}{1}{2}","{",((Button)e.CommandSource).CommandArgument,"}");

                JabsomAffil affil = JsonConvert.DeserializeObject<JabsomAffil>(arg);

                if (affil != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "ModalScript", PageUtility.LoadEditScript(true), false);

                    lblAffilId.Text = affil.Id.ToString();
                    ddlCategoryEdit.SelectedValue = affil.Type;
                    ddlCategoryEdit.Enabled = false;
                    txtName.Value = affil.Name;
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblAffilId.Text = string.Empty;
            ddlCategoryEdit.SelectedValue = ddlCategory.SelectedValue;
            ddlCategoryEdit.Enabled = true;
            txtName.Value = string.Empty;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "ModalScript", PageUtility.LoadEditScript(true), false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Value.Length < 10 || ddlCategoryEdit.SelectedValue == string.Empty)
            {
                return;
            }
            else
            {
                int affilId = 0;
                Int32.TryParse(lblAffilId.Text, out affilId);
                bool saved = false;

                var affil = _repository.GetAffiliationByName(txtName.Value);
                if (affilId > 0)
                {
                    if (affil == null || affil.Id == affilId)
                    {
                        var affilEdit = new JabsomAffil()
                        {
                            Id = affilId,
                            Name = txtName.Value
                        };

                        saved = _repository.UpdateAffiliation(affilEdit);
                    }
                }
                else
                {
                    var newAffil = new JabsomAffil
                    {
                        Type = ddlCategoryEdit.SelectedValue,
                        Name = txtName.Value
                    };

                    if (affil != null)
                    {
                        Response.Write("<script>alert('Affiliation exists already!');</script>");
                    }
                    else
                    {
                        saved = _repository.AddAffiliation(newAffil) > 0;
                    }
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                   "ModalScript", PageUtility.LoadEditScript(false), false);

                List<JabsomAffil> affiliations = _repository.GetAffiliations("");
                rptAffiliation.DataSource = affiliations;
                rptAffiliation.DataBind();
            }
        }

        private void CloseModal(bool saved)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
               "ModalScript", PageUtility.LoadEditScript(false), false);

            if (saved)
                Response.Write("<script>alert('Affilation is saved.');</script>");

           
        }

        
    }
}