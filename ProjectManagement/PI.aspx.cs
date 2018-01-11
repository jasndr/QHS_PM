using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Reflection;
using ProjectManagement.Model;
using System.Net.Mail;
using System.Text;

namespace ProjectManagement
{
    public partial class PI : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {
            string piId = Request.QueryString["Id"];

            if (!Page.IsPostBack)
            {
                BindControl(piId);
            }

            if (!Page.User.IsInRole("Admin"))
            {
                chkApproved.Enabled = false;
            }
        }

        protected void rptPI_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                int investId = 0;
                int.TryParse(((Button)e.CommandSource).CommandArgument, out investId);

                if (investId > 0)
                {
                    Invest invest;
                    ICollection<JabsomAffil> jabsomAffilList;

                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                        invest = db.Invests.First(i => i.Id == investId);
                        //DetailsView1.DataSource = query;
                        //DetailsView1.DataBind();                   

                        //BindDropDownList(ddlProject, dropDownSource, "Add a new project");

                        jabsomAffilList = invest.JabsomAffils;
                    }

                    lblInvestId.Text = invest.Id.ToString();
                    TextBoxFirstName.Text = invest.FirstName;
                    TextBoxLastName.Text = invest.LastName;
                    TextBoxEmail.Text = invest.Email;
                    TextBoxPhone.Text = invest.Phone;
                    ddlStatus.SelectedValue = invest.InvestStatusId.ToString();
                    TextBoxNonHawaii.Text = invest.NonHawaiiClient;

                    TextBox txtNonUHOther = GridViewNonUH.FooterRow.FindControl("txtNonUHOther") as TextBox;
                    if (invest.NonUHClient != null)
                    {
                        txtNonUHOther.Text = invest.NonUHClient;
                    }
                    else
                    {
                        txtNonUHOther.Text = string.Empty;
                    }

                    TextBox txtDegreeOther = GridViewDegree.FooterRow.FindControl("txtDegreeOther") as TextBox;
                    if (invest.OtherDegree != null)
                    {
                        txtDegreeOther.Text = invest.OtherDegree;
                    }
                    else
                    {
                        txtDegreeOther.Text = string.Empty;
                    }

                    TextBox txtCommunityPartnerOther = GridViewCommunityPartner.FooterRow.FindControl("txtCommunityPartnerOther") as TextBox;
                    if (invest.OtherCommunityPartner != null)
                    {
                        txtCommunityPartnerOther.Text = invest.OtherCommunityPartner;
                    }
                    else
                    {
                        txtCommunityPartnerOther.Text = string.Empty;
                    }

                    chkApproved.Checked = invest.IsApproved;
                    chkPilot.Checked = invest.IsPilot;

                    //bind jabsom affils
                    //if (jabsomAffilList.Count > 0)
                    //{
                    Bind_JabsomAffil(jabsomAffilList);
                    //}

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#editModal').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "ModalScript", sb.ToString(), false);
                }
            }
        }

        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName.Equals("editRecord"))
        //    {
        //        int index = Convert.ToInt32(e.CommandArgument);
        //        int investId = (int)GridView1.DataKeys[index].Value;

        //        //IEnumerable<DataRow> query = from i in dt.AsEnumerable()
        //        //                             where i.Field<String>("Code").Equals(code)
        //        //                             select i;              

        //        //DataTable detailTable = query.CopyToDataTable<DataRow>();

        //        if (investId > 0)
        //        {
        //            Invest invest;
        //            ICollection<JabsomAffil> jabsomAffilList;

        //            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
        //            {
        //                invest = db.Invests.First(i => i.Id == investId);
        //                //DetailsView1.DataSource = query;
        //                //DetailsView1.DataBind();                   

        //                //BindDropDownList(ddlProject, dropDownSource, "Add a new project");

        //                jabsomAffilList = invest.JabsomAffils;
        //            }

        //            lblInvestId.Text = invest.Id.ToString();
        //            TextBoxFirstName.Text = invest.FirstName;
        //            TextBoxLastName.Text = invest.LastName;
        //            TextBoxEmail.Text = invest.Email;
        //            TextBoxPhone.Text = invest.Phone;
        //            ddlStatus.SelectedValue = invest.InvestStatusId.ToString();
        //            TextBoxNonHawaii.Text = invest.NonHawaiiClient;

        //            TextBox txtNonUHOther = GridViewNonUH.FooterRow.FindControl("txtNonUHOther") as TextBox;
        //            if (invest.NonUHClient != null)
        //            {
        //                txtNonUHOther.Text = invest.NonUHClient;
        //            }
        //            else
        //            {
        //                txtNonUHOther.Text = string.Empty;
        //            }

        //            TextBox txtDegreeOther = GridViewDegree.FooterRow.FindControl("txtDegreeOther") as TextBox;
        //            if (invest.OtherDegree != null)
        //            {
        //                txtDegreeOther.Text = invest.OtherDegree;
        //            }
        //            else
        //            {
        //                txtDegreeOther.Text = string.Empty;
        //            }

        //            TextBox txtCommunityPartnerOther = GridViewCommunityPartner.FooterRow.FindControl("txtCommunityPartnerOther") as TextBox;
        //            if (invest.OtherCommunityPartner != null)
        //            {
        //                txtCommunityPartnerOther.Text = invest.OtherCommunityPartner;
        //            }
        //            else
        //            {
        //                txtCommunityPartnerOther.Text = string.Empty;
        //            }

        //            chkApproved.Checked = invest.IsApproved;
        //            chkPilot.Checked = invest.IsPilot;
                   
        //            //bind jabsom affils
        //            //if (jabsomAffilList.Count > 0)
        //            //{
        //                Bind_JabsomAffil(jabsomAffilList);
        //            //}

        //            System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //            sb.Append(@"<script type='text/javascript'>");
        //            sb.Append("$('#editModal').modal('show');");
        //            sb.Append(@"</script>");
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
        //                       "ModalScript", sb.ToString(), false);
        //        }

        //    }
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {          
            ClearEditForm();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

        }

        private void ClearEditForm()
        {
            lblInvestId.Text = string.Empty;
            TextBoxFirstName.Text = string.Empty;
            TextBoxLastName.Text = string.Empty;
            TextBoxEmail.Text = string.Empty;
            TextBoxPhone.Text = string.Empty;
            //TextBoxNonUH.Text = string.Empty;
            TextBoxNonHawaii.Text = string.Empty;

            TextBox txtNonUHOther = GridViewNonUH.FooterRow.FindControl("txtNonUHOther") as TextBox;
            txtNonUHOther.Text = string.Empty;

            TextBox txtDegreeOther = GridViewDegree.FooterRow.FindControl("txtDegreeOther") as TextBox;
            txtDegreeOther.Text = string.Empty;

            TextBox txtCommunityPartnerOther = GridViewCommunityPartner.FooterRow.FindControl("txtCommunityPartnerOther") as TextBox;
            txtCommunityPartnerOther.Text = string.Empty;

            ddlStatus.ClearSelection();
            ddlUHFaculty.ClearSelection();

            PageUtility.UncheckGrid(GridViewDept);
            PageUtility.UncheckGrid(GridViewOffice);

            ddlJabsomOther.ClearSelection();

            PageUtility.UncheckGrid(GridViewUHDept);
            GridViewUHDept.Style["display"] = "none";

            //ddlHospital.ClearSelection();

            //PageUtility.UncheckGrid(GridViewHph);
            //GridViewHph.Style["display"] = "none";

            PageUtility.UncheckGrid(GridViewNonUH);
            PageUtility.UncheckGrid(GridViewDegree);
            PageUtility.UncheckGrid(GridViewCommunityCollege);
            PageUtility.UncheckGrid(GridViewCommunityPartner);

            chkApproved.Checked = false;
            chkPilot.Checked = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vr = ValidateResult();
            if (vr.Equals(string.Empty))
            {
                int id = 0;
                if (!lblInvestId.Text.Equals(string.Empty))
                {
                    id = Convert.ToInt32(lblInvestId.Text);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");

                if (SetPIValue(id))
                {
                    BindGridView();
                    sb.Append("alert('Records Updated Successfully');");
                    sb.Append("$('#editModal').modal('hide');");
                }
                else
                {
                    sb.Append("alert('Please contact admin group for any updates.');");
                }

                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
            }
            else
            {
                Response.Write("<script>alert('" + vr + "');</script>");
            }

        }

        private string ValidateResult()
        {
            System.Text.StringBuilder validateResult = new System.Text.StringBuilder();

            if (TextBoxFirstName.Text.Equals(string.Empty))
            {
                validateResult.Append("First name is required. \\n");
            }

            if (TextBoxLastName.Text.Equals(string.Empty))
            {
                validateResult.Append("Last name is required. \\n");
            }

            //if (TextBoxEmail.Text.Equals(string.Empty))
            //{
            //    validateResult.Append("Email is required. \\n");
            //}

            return validateResult.ToString();
        }

        private void BindGridView()
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var query = db.Invests
                            .Join(db.InvestStatus, i => i.InvestStatusId, s => s.Id, (i, s) => new { i.Id, i.FirstName, i.LastName, i.Email, i.Phone, s.StatusValue })
                            //.Where(a => a.LastName.Contains(txtSrchInvestorName.Text) || a.FirstName.Contains(txtSrchInvestorName.Text) || string.Concat(a.FirstName, " ", a.LastName).Contains(txtSrchInvestorName.Text))
                            .OrderBy(d => d.Id);

                rptPI.DataSource = query.Where(q => q.Id > 0).ToList();
                rptPI.DataBind();
            }
        }

        private bool SetPIValue(int investId)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                Invest invest;                        

                if (investId == 0) // new PI
                {
                    if (!TextBoxFirstName.Text.Equals(string.Empty) && !TextBoxLastName.Text.Equals(string.Empty))
                    {
                        invest = new Invest()
                        {
                            FirstName = TextBoxFirstName.Text.Trim(),
                            LastName = TextBoxLastName.Text.Trim(),
                            Email = TextBoxEmail.Text,
                            Phone = TextBoxPhone.Text,
                            InvestStatusId = Convert.ToInt32(ddlStatus.SelectedValue),
                            IsApproved = chkApproved.Checked,
                            IsPilot = chkPilot.Checked
                        };

                        //check existing PI with same name
                        if (db.Invests.Any(i => i.FirstName == invest.FirstName && i.LastName == invest.LastName))
                        {
                            return false;
                        }

                        if (ddlStatus.SelectedItem.Text == "Non-Hawaii Client" || ddlStatus.SelectedItem.Text == "UH Student")
                        {
                            invest.NonHawaiiClient = TextBoxNonHawaii.Text;
                        }
                        
                        TextBox txtNonUHOther = GridViewNonUH.FooterRow.FindControl("txtNonUHOther") as TextBox;
                        invest.NonUHClient = txtNonUHOther.Text;
                       
                        TextBox txtDegreeOther = GridViewDegree.FooterRow.FindControl("txtDegreeOther") as TextBox;
                        invest.OtherDegree = txtDegreeOther.Text;

                        TextBox txtCommunityPartnerOther = GridViewCommunityPartner.FooterRow.FindControl("txtCommunityPartnerOther") as TextBox;
                        invest.OtherCommunityPartner = txtCommunityPartnerOther.Text;

                        db.Invests.Add(invest);
                        db.SaveChanges();

                        investId = invest.Id;

                        //PageUtility.SendNotificationByEmail("PI", investId, User.Identity.Name);
                        SendNotificationEmail(invest.Id);
                    }
                }
                else
                {
                    invest = db.Invests.First(i => i.Id == investId);

                    if (!Page.User.IsInRole("Admin") && invest.IsApproved)
                    {
                        //Response.Write("<script>alert('This PI has been approved, please contact with admin group for any changes.');</script>");
                        return false;
                    }
                    else
                    {                       
                        if (invest != null && !TextBoxFirstName.Text.Equals(string.Empty) && !TextBoxLastName.Text.Equals(string.Empty))
                        {
                            invest.FirstName = TextBoxFirstName.Text.Trim();
                            invest.LastName = TextBoxLastName.Text.Trim();
                            invest.Email = TextBoxEmail.Text;
                            invest.Phone = TextBoxPhone.Text;
                            invest.InvestStatusId = Convert.ToInt32(ddlStatus.SelectedValue);

                            if (ddlStatus.SelectedItem.Text == "Non-Hawaii Client" || ddlStatus.SelectedItem.Text == "UH Student")
                            {
                                invest.NonHawaiiClient = TextBoxNonHawaii.Text;
                            }

                            TextBox txtNonUHOther = GridViewNonUH.FooterRow.FindControl("txtNonUHOther") as TextBox;
                            invest.NonUHClient = txtNonUHOther.Text;

                            TextBox txtDegreeOther = GridViewDegree.FooterRow.FindControl("txtDegreeOther") as TextBox;
                            invest.OtherDegree = txtDegreeOther.Text;

                            TextBox txtCommunityPartnerOther = GridViewCommunityPartner.FooterRow.FindControl("txtCommunityPartnerOther") as TextBox;
                            invest.OtherCommunityPartner = txtCommunityPartnerOther.Text;

                            invest.IsApproved = chkApproved.Checked;
                            invest.IsPilot = chkPilot.Checked;

                            db.SaveChanges();
                        }
                    }
                }

                #region update PI affiliations
                List<int> newAffliIdList = GetInvestJabsomAffil();

                List<int> prevAffilIdList = new List<int>();

                ICollection<JabsomAffil> jabsomAffils = db.Invests.First(i => i.Id == investId).JabsomAffils;
                foreach (JabsomAffil affil in jabsomAffils)
                {
                    prevAffilIdList.Add(affil.Id);
                }

                var newNotPrevAffilList = newAffliIdList.Except(prevAffilIdList).ToList();
                var prevNotNewAffilList = prevAffilIdList.Except(newAffliIdList).ToList();

                if (prevNotNewAffilList.Count > 0)
                {
                    foreach (var expiredId in prevNotNewAffilList)
                    {
                        //project.ProjectBioStats.First(b => b.BioStats_Id == expiredId).EndDate = DateTime.Parse(_currentDate);
                        var jabsomAffil = jabsomAffils.First(j => j.Id == expiredId);
                        jabsomAffils.Remove(jabsomAffil);
                    }
                }

                if (newNotPrevAffilList.Count > 0)
                {
                    foreach (var newId in newNotPrevAffilList)
                    {
                        var jabsomAffil = db.JabsomAffils.First(j => j.Id == newId);
                        jabsomAffils.Add(jabsomAffil);
                    }
                }

                db.SaveChanges();
                #endregion
            }

            return true;
        }        

        private List<int> GetInvestJabsomAffil()
        {
            ICollection<JabsomAffil> jabsomAffilCollection = new Collection<JabsomAffil>();
            List<int> idList = new List<int>();

            AddSelectedRowToList(GridViewDept, idList);
            AddSelectedRowToList(GridViewOffice, idList);
            AddSelectedRowToList(GridViewNonUH, idList);
            AddSelectedRowToList(GridViewDegree, idList);
            AddSelectedRowToList(GridViewCommunityCollege, idList);
            AddSelectedRowToList(GridViewCommunityPartner, idList);

            if (!ddlJabsomOther.SelectedValue.Equals(string.Empty))
            {
                idList.Add(Convert.ToInt32(ddlJabsomOther.SelectedItem.Value));
                if (ddlJabsomOther.SelectedItem.Text.Contains("UH School"))
                {
                    AddSelectedRowToList(GridViewUHDept, idList);
                }
                //if (ddlJabsomOther.SelectedItem.Text.Contains("Major Hospitals"))
                //{
                //    if (ddlHospital.SelectedIndex > 0)
                //    {
                //        idList.Add(Convert.ToInt32(ddlHospital.SelectedValue));
                //        if (ddlHospital.SelectedItem.Text.Contains("Hawaii Pacific Health Hospitals"))
                //        {
                //            AddSelectedRowToList(GridViewHph, idList);
                //        }
                //    }
                //}
            }

            //if (ddlStatus.SelectedItem.Text == "Non-UH Client")
            //{
            //    AddSelectedRowToList(GridViewNonUH, idList);
            //}

            //ddlUHFaculty
            if (ddlStatus.SelectedItem.Text == "UH Faculty")
            {
                int uhfaculty = 0;
                Int32.TryParse(ddlUHFaculty.SelectedValue, out uhfaculty);
                if (uhfaculty > 0)
                {
                    idList.Add(uhfaculty);
                }
            }

            return idList;      
        }
       
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    BindGridView();
        //}

        private void Bind_JabsomAffil(ICollection<JabsomAffil> jabsomAffilList)
        {
            List<int> idList = new List<int>();

            foreach (JabsomAffil jaf in jabsomAffilList)
            {
                idList.Add(jaf.Id);
            }

            PageUtility.SelectRow_GridView(GridViewDept, idList);

            PageUtility.SelectRow_GridView(GridViewOffice, idList);

            PageUtility.SelectRow_GridView(GridViewNonUH, idList);

            PageUtility.SelectRow_GridView(GridViewDegree, idList);

            PageUtility.SelectRow_GridView(GridViewCommunityCollege, idList);

            PageUtility.SelectRow_GridView(GridViewCommunityPartner, idList);

            int selectedJabsomId = 0;
            //int selectedHospital = 0;

            GridViewUHDept.Style["display"] = "none";
            foreach (ListItem item in ddlJabsomOther.Items)
            {
                int index;
                bool result = Int32.TryParse(item.Value, out index);
                if (result)
                {
                    if (idList.Contains(index))
                    {
                        selectedJabsomId = index;

                        if (item.Text.Contains("UH School"))
                        {
                            GridViewUHDept.Style["display"] = "inherit";
                        }
                    }
                }
            }

            //GridViewHph.Style["display"] = "none";
            //foreach (ListItem item in ddlHospital.Items)
            //{
            //    int index;
            //    bool result = Int32.TryParse(item.Value, out index);
            //    if (result)
            //    {

            //        if (idList.Contains(index))
            //        {
            //            selectedHospital = index;

            //            if (item.Text.Contains("Hawaii Pacific Health"))
            //            {
            //                GridViewHph.Style["display"] = "inherit";
            //            }
            //        }                 
            //    }
            //}

            if (selectedJabsomId > 0)
            {
                ddlJabsomOther.SelectedValue = selectedJabsomId.ToString();
                PageUtility.SelectRow_GridView(GridViewUHDept, idList);
            }
            else
            {
                ddlJabsomOther.ClearSelection();
            }

            //if (selectedHospital > 0)
            //{
            //    ddlHospital.SelectedValue = selectedHospital.ToString();
            //    PageUtility.SelectRow_GridView(GridViewHph, idList);
            //}
            //else
            //{
            //    ddlHospital.ClearSelection();
            //}

            //if (ddlStatus.SelectedItem.Text == "Non-UH Client")
            //{
            //    PageUtility.SelectRow_GridView(GridViewNonUH, idList);
            //}
            //else
            //{
            //    PageUtility.UncheckGrid(GridViewNonUH);
            //}

            //ddlUHFaculty
            ddlUHFaculty.ClearSelection();
            if (ddlStatus.SelectedItem.Text == "UH Faculty")
            {
                foreach (ListItem item in ddlUHFaculty.Items)
                {
                    int index;
                    bool result = Int32.TryParse(item.Value, out index);
                    if (result)
                    {
                        if (idList.Contains(index))
                        {
                            ddlUHFaculty.SelectedValue = index.ToString();
                        }
                    }
                }
            }
        }

        private void AddSelectedRowToList(GridView gv, List<int> idList)
        {
            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label lblId = row.FindControl("lblId") as Label;
                        int deptId = Convert.ToInt32(lblId.Text);

                        idList.Add(deptId);
                    }
                }
            }
        }    
            
        private void BindControl(string piId)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                int id = 0;
                Int32.TryParse(piId, out id);

                if (id > 0)
                {
                    var query1 = db.Invests
                            .Join(db.InvestStatus, i => i.InvestStatusId, s => s.Id,
                                    (i, s) => new { i.Id, i.FirstName, i.LastName, i.Email, i.Phone, s.StatusValue })
                            .Where(d => d.Id == id)
                            .OrderBy(d => d.Id);

                    //GridView1.DataSource = query1.ToList();
                    //GridView1.DataBind();

                    rptPI.DataSource = query1.ToList();
                    rptPI.DataBind();
                }
                else
                {
                    var query = db.Invests
                                .Join(db.InvestStatus, i => i.InvestStatusId, s => s.Id,
                                        (i, s) => new { i.Id, i.FirstName, i.LastName, i.Email, i.Phone, s.StatusValue })
                                .Where(d => d.Id > 0)
                                .OrderBy(d => d.Id);

                    //GridView1.DataSource = query.ToList();
                    //GridView1.DataBind();

                    rptPI.DataSource = query.ToList();
                    rptPI.DataBind();
                }

                var statusQuery = db.InvestStatus
                               .OrderBy(d => d.DisplayOrder);

                ddlStatus.DataSource = statusQuery.ToList();
                ddlStatus.DataValueField = "Id";
                ddlStatus.DataTextField = "StatusValue";

                ddlStatus.DataBind();

                //uhfaculty
                var uhfacultyQuery = db.JabsomAffils
                                    .Where(d => d.Type == "UHFaculty")
                                    .OrderBy(d => d.Id);

                ddlUHFaculty.DataSource = uhfacultyQuery.ToList();
                ddlUHFaculty.DataValueField = "Id";
                ddlUHFaculty.DataTextField = "Name";
                ddlUHFaculty.DataBind();
                ddlUHFaculty.Items.Insert(0, new ListItem(String.Empty, String.Empty));

                //degree
                var queryDegree = db.JabsomAffils
                                  .Where(d => d.Type == "Degree");

                GridViewDegree.DataSource = queryDegree.ToList();
                GridViewDegree.DataBind();

                //department
                var queryDept = db.JabsomAffils
                            .Where(d => d.Type == "Department");

                GridViewDept.DataSource = queryDept.ToList();
                GridViewDept.DataBind();

                var queryOffice = db.JabsomAffils
                                .Where(d => d.Type == "Office");

                GridViewOffice.DataSource = queryOffice.ToList();
                GridViewOffice.DataBind();

                //ddlJabsomOther
                dropDownSource = db.JabsomAffils
                                .Where(d => d.Type == "College")
                                .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlJabsomOther, dropDownSource, string.Empty);

                var queryUHDept = db.JabsomAffils
                                  .Where(d => d.Type == "UHSchool");

                GridViewUHDept.DataSource = queryUHDept.ToList();
                GridViewUHDept.DataBind();

                //GridViewCommunityCollege
                var queryCommunityCollege = db.JabsomAffils
                                            .Where(d => d.Type == "CommunityCollege");

                GridViewCommunityCollege.DataSource = queryCommunityCollege.ToList();
                GridViewCommunityCollege.DataBind();

                //GridViewCommunityPartner
                var queryCommunityPartner = db.JabsomAffils
                                            .Where(d => d.Type == "CommunityPartner");

                GridViewCommunityPartner.DataSource = queryCommunityPartner.ToList();
                GridViewCommunityPartner.DataBind();

                //ddlHospital
                dropDownSource = db.JabsomAffils
                                 .Where(d => d.Type == "MajorHospital")
                                 .ToDictionary(c => c.Id, c => c.Name);

                //PageUtility.BindDropDownList(ddlHospital, dropDownSource, String.Empty);

                ////GridViewHph
                //var queryHph = db.JabsomAffils
                //               .Where(d => d.Type == "HawaiiPacificHealth");

                //GridViewHph.DataSource = queryHph.ToList();
                //GridViewHph.DataBind();

                //GridViewNonUH
                var queryNonUH = db.JabsomAffils
                               .Where(d => d.Type == "MajorHospital");

                GridViewNonUH.DataSource = queryNonUH.ToList();
                GridViewNonUH.DataBind();

                //if (!Page.User.IsInRole("Admin"))
                //{
                //    btnAdd.Visible = false;
                //    btnSave.Visible = false;
                //}

                //var principleI = db.Invests.Where(i => i.Id > 0)
                //                .Select(x => new { FullName = x.FirstName + " " + x.LastName });
                //textAreaPI.Value = Newtonsoft.Json.JsonConvert.SerializeObject(principleI);
            }
        }

        private void SendNotificationEmail(int investId)
        {
            string email = System.Configuration.ConfigurationManager.AppSettings["trackingEmail"];

            MailAddress destination = new MailAddress(email);

            string subject = String.Format("A new PI is pending approval, id {0}", investId);

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (url.IndexOf("?Id") > 0)
            {
                url = url.Substring(0, url.IndexOf("?Id"));
            }

            StringBuilder body = new StringBuilder();
            body.AppendFormat("<p>Request GUID {0}<br /><br />", Guid.NewGuid());
            body.AppendFormat("Please approve new PI created by {0} at {1}", User.Identity.Name, url);
            body.AppendFormat("?Id={0}</p>", investId);
            body.AppendLine();

            IdentityMessage im = new IdentityMessage()
            {
                Subject = subject,
                Destination = destination.Address,
                Body = body.ToString()
            };

            EmailService emailService = new EmailService();
            emailService.Send(im);
        }
        
    }
}