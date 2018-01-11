using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement
{
    public partial class PubHistory1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }
        }

        private void BindControl()
        {
            string userName = Page.User.Identity.Name;

            if (!userName.Equals(string.Empty))
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var dbQuery = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DateTime>("CurrentDateTime() ");
                    HiddenFieldCurrentDate.Value = dbQuery.AsEnumerable().First().ToShortDateString();
                    DateTime currentDate = Convert.ToDateTime(HiddenFieldCurrentDate.Value);
                    
                    IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                    //ddlBiostat
                    dropDownSource = context.BioStats
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.Name);
                    BindDropDownList(ddlBiostat, dropDownSource, string.Empty);

                    //ddlPublication
                    dropDownSource = context.Publications
                                        .OrderBy(b => b.Id)
                                        .ToDictionary(c => c.Id, c => c.Title);

                    BindDropDownList(ddlPublication, dropDownSource, string.Empty);

                    //var queryHistory = context.PubHistories;

                    //GridViewHistory.DataSource = queryHistory.ToList();
                    //GridViewHistory.DataBind();
                    
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateControl())
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    int pubId = Convert.ToInt32(ddlPublication.SelectedValue);

                    PubHistory newHist = new PubHistory()
                    {
                        PublicationId = pubId,
                        StatusId = PubStatus,
                        PaperDate = Convert.ToDateTime(TextBoxStartDate.Text),
                        Creator = Page.User.Identity.Name,
                        CreateDate = Convert.ToDateTime(HiddenFieldCurrentDate.Value)
                    };

                    context.PubHistories.Add(newHist);

                    context.SaveChanges();

                    Response.Write("<script>alert('Publication History Saved.');</script>");

                    var pubHist = context.PubHistories
                                    .Join(context.Publications, h => h.PublicationId, p => p.Id, (h, p) => new { h })
                                    //.Join(context.PublishStatus, ph => ph.h.StatusId, ps => ps.Id, (ph, ps) => new { ph.h.Id, ps.Name, ph.h.PaperDate,  ph.h.Creator, ph.h.PublicationId })
                                    .Where(i => i.h.PublicationId == pubId)
                                    .ToList();

                    GridViewHistory.DataSource = pubHist;
                    GridViewHistory.DataBind();
                }
            }
            else
            {
                Response.Write("<script>alert('Please check the data entered.');</script>");
            }
        }

        private bool ValidateControl()
        {
            DateTime startDate = Convert.ToDateTime(TextBoxStartDate.Text);
            DateTime endDate = Convert.ToDateTime(TextBoxEndDate.Text);

            if (DateTime.Compare(startDate, endDate) > 0)
                return false;

            int pubId;
            bool result = Int32.TryParse(ddlPublication.SelectedValue, out pubId);

            return result;
        }

        //protected void GridViewHistory_RowCommand(Object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Delete")
        //    {
        //        GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
        //        //int id = Convert.ToInt32(e.CommandSource);
                

        //        Label lblId = gvr.FindControl("lblId") as Label;

        //         if (lblId != null)
        //         {
        //             using (ProjectTrackerContainer context = new ProjectTrackerContainer())
        //             {
        //                 int id = Convert.ToInt32(lblId.Text);
        //                 PubHistory ph = context.PubHistories.First(t => t.Id == id);
        //                 context.PubHistories.Remove(ph);
        //                 context.SaveChanges();

        //                 GridViewHistory.DeleteRow(gvr.RowIndex);
 
        //             }
        //         }
        //    }
        //}

        protected void GridViewHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewHistory.Rows[e.RowIndex];

            Label lblId = row.FindControl("lblId") as Label;

            if (lblId != null)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    int id = Convert.ToInt32(lblId.Text);
                    PubHistory hist = context.PubHistories.First(t => t.Id == id);
                    context.PubHistories.Remove(hist);
                    context.SaveChanges();

                    var pubHist = context.PubHistories
                                    .Join(context.Publications, h => h.PublicationId, p => p.Id, (h, p) => new { h })
                                    //.Join(context.PublishStatus, ph => ph.h.Status, ps => ps.Id, (ph, ps) => new { ph.h.Id, ps.Name, ph.h.StartDate, ph.h.EndDate, ph.h.Creator, ph.h.PublicationId })
                                    .Where(i => i.h.PublicationId == hist.PublicationId)
                                    .ToList();

                    GridViewHistory.DataSource = pubHist;
                    GridViewHistory.DataBind();
                }                

                if (GridViewHistory.Rows.Count > 0)
                {
                    GridViewHistory.SelectRow(GridViewHistory.Rows.Count - 1);
                    GridViewHistory.SelectedRow.Focus();
                }


            }
        }

        //ddlBiostat_Changed
        protected void ddlBiostat_Changed(Object sender, EventArgs e)
        {
            int bioStatId;
            Int32.TryParse(ddlBiostat.SelectedValue, out bioStatId);

            if (bioStatId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                    dropDownSource = context.Publications
                                .Join(context.PublicationBioStats, p=>p.Id, b=>b.Publications_Id, (p, b) => new {p.Id, p.Title, b.BioStats_Id})
                                .Where(i => i.BioStats_Id == bioStatId)
                                .ToDictionary(c => c.Id, c => c.Title);

                    BindDropDownList(ddlPublication, dropDownSource, string.Empty);
                }

            }

        }

        protected void ddlPublication_Changed(Object sender, EventArgs e)
        {
            int pubId;
            Int32.TryParse(ddlPublication.SelectedValue, out pubId);

            if (pubId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var pubHist = context.PubHistories
                                    .Join(context.Publications, h => h.PublicationId, p => p.Id, (h,p) => new {h})
                                    //.Join(context.PublishStatus, ph => ph.h.StatusId, ps => ps.Id, (ph, ps) => new { ph.h.Id, ps.Name, ph.h.StartDate, ph.h.EndDate, ph.h.Creator, ph.h.PublicationId })
                                    .Where(i => i.h.PublicationId == pubId)
                                    .ToList();

                    GridViewHistory.DataSource = pubHist;
                    GridViewHistory.DataBind();
                }
            }
        }

        private void BindDropDownList(DropDownList ddl, IDictionary<int, string> dataSource, string firstItemText)
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = "Key";
            ddl.DataTextField = "Value";
            ddl.DataBind();

            if (firstItemText != null)  //(dataSource.Count > 1)
            {
                ddl.Items.Insert(0, new ListItem(firstItemText, String.Empty));
            }

        }

        protected int PubStatus
        {
            get
            {
                if (optionSubmitted.Checked) return (int)StatusEnum.SubResub;           //"Submitted"
                else if (optionAccepted.Checked) return (int)StatusEnum.Accepted;       //"Accepted"
                else if (optionPublished.Checked) return (int)StatusEnum.Published;     //"Published";
                else return (int)StatusEnum.NotAccepted;                                //"NotAccepted";              
            }
        }

    }
}