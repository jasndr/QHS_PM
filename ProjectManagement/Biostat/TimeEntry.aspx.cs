using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.IO;
//using System.Data;
//using ClosedXML.Excel;

namespace ProjectManagement
{
    public partial class TimeEntry1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }
        }

        protected void GridViewTimeEntry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridViewTimeEntry.Rows[index];

                if (row != null)
                {                    
                    lblEditId.Text = ((Label)row.FindControl("lblId")).Text;
                    lblEditProjectId.Text = ((Label)row.FindControl("lblProjectId")).Text;
                    lblEditProject.Text = ((Label)row.FindControl("lblProjectName")).Text;

                    TextBoxEditDate.Text = Convert.ToDateTime(((Label)row.FindControl("lblDate")).Text).ToShortDateString();
                    TextBoxEditTime.Text = ((Label)row.FindControl("lblTime")).Text;
                    
                    string serviceType = ((Label)row.FindControl("lblServiceType")).Text;
                    ddlEditServiceType.ClearSelection();
                    ddlEditServiceType.Items.FindByText(serviceType).Selected = true;

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#editModal').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "ModalScript", sb.ToString(), false);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            if (ValidateEdit())
            {
                UpdateTimeEntry();

                sb.Append("alert('Records Updated Successfully');");
                sb.Append("$('#editModal').modal('hide');");
                
            }
            else
            {
                sb.Append("alert('Entries are not valid.');");
            }

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
        }

        private void UpdateTimeEntry()
        {
            int id;
            if (Int32.TryParse(lblEditId.Text, out id))
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    TimeEntry timeEntry = context.TimeEntries.First(t => t.Id == id);

                    if (timeEntry != null)
                    {
                        timeEntry.ServiceTypeId = Convert.ToInt32(ddlEditServiceType.SelectedValue);
                        timeEntry.DateKey = GetDate(TextBoxEditDate.Text);
                        timeEntry.Duration = Convert.ToDecimal(TextBoxEditTime.Text);

                        context.SaveChanges();
                    }
                }

                BindGridView();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateControl())
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    TimeEntry timeEntry = new TimeEntry()
                    {
                        ProjectId = Convert.ToInt32(ddlProject.SelectedValue),
                        BioStatId = Convert.ToInt32(ddlBioStat.SelectedValue),
                        Duration = Convert.ToDecimal(TextBoxTime.Text),
                        ServiceTypeId = Convert.ToInt32(ddlServiceType.SelectedValue),
                        DateKey = GetDate(TextBoxSubmitDate.Text)
                    };

                    context.TimeEntries.Add(timeEntry);

                    context.SaveChanges();

                    Response.Write("<script>alert('Time Entry Saved.');</script>");
                }

                BindGridView();
            }
        }

        //protected void ExportExcel(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable("GridView_Data");
        //    foreach (TableCell cell in GridViewTimeEntry.HeaderRow.Cells)
        //    {
        //        dt.Columns.Add(cell.Text);
        //    }
        //    foreach (GridViewRow row in GridViewTimeEntry.Rows)
        //    {
        //        dt.Rows.Add();
        //        //for (int i = 0; i < row.Cells.Count; i++)
        //        //{
        //        //    dt.Rows[dt.Rows.Count - 1][i] = ((Label)row.Cells[i].FindControl("lblId")).Text;
        //        //}
        //        dt.Rows[dt.Rows.Count - 1][0] = ((Label)row.Cells[0].FindControl("lblId")).Text;
        //        dt.Rows[dt.Rows.Count - 1][1] = ((Label)row.Cells[1].FindControl("lblProjectName")).Text;
        //        dt.Rows[dt.Rows.Count - 1][2] = ((Label)row.Cells[2].FindControl("lblCategoryName")).Text;
        //        dt.Rows[dt.Rows.Count - 1][3] = ((Label)row.Cells[3].FindControl("lblDate")).Text;
        //        dt.Rows[dt.Rows.Count - 1][4] = ((Label)row.Cells[4].FindControl("lblTime")).Text;

        //    }
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);

        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //    }
        //}

        protected void ddlMonth_Changed(Object sender, EventArgs e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            int bioStatId, monthId;
            Int32.TryParse(ddlBioStat.SelectedValue, out bioStatId);
            Int32.TryParse(ddlMonth.SelectedValue, out monthId);

            if (bioStatId > 0 && monthId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var query = context.TimeEntries
                                .Join(context.Projects
                                    , t => t.ProjectId
                                    , p => p.Id
                                    , (t, p) => new { t, p.Title }
                                    )
                                .Join(context.ServiceTypes
                                    , tt => tt.t.ServiceTypeId
                                    , s => s.Id
                                    , (tt, s) => new { tt, s.Name }
                                    )
                                .Join(context.Date1
                                    , ttt => ttt.tt.t.DateKey
                                    , d => d.DateKey
                                    , (ttt, d) => new
                                    {
                                        ttt.tt.t.Id
                                        ,
                                        ttt.tt.t.ProjectId
                                        ,
                                        ttt.tt.Title
                                        ,
                                        ttt.Name
                                        ,
                                        d.Date
                                        ,
                                        ttt.tt.t.Duration
                                        ,
                                        ttt.tt.t.BioStatId
                                    }
                                )
                                .Where(a => a.BioStatId == bioStatId && a.Date.Month == monthId);

                    GridViewTimeEntry.DataSource = query.ToList();
                    GridViewTimeEntry.DataBind();
                }
            }
        }

        //private void BindGrid(ProjectTrackerContainer context, GridView GridViewTimeEntry)
        //{
        //    int bioStatId = Convert.ToInt32(ddlBioStat.SelectedValue);

        //    var query = context.TimeEntries
        //                .Join(context.Projects
        //                    , t => t.ProjectId
        //                    , p => p.Id
        //                    , (t, p) => new { t, p.Title }
        //                    )
        //                .Join(context.ServiceTypes
        //                    , tt => tt.t.ServiceTypeId
        //                    , s => s.Id
        //                    , (tt, s) => new { tt, s.Name }
        //                    )
        //                .Join(context.Date1
        //                    , ttt => ttt.tt.t.DateKey
        //                    , d => d.DateKey
        //                    , (ttt, d) => new { 
        //                        ttt.tt.t.Id
        //                        ,ttt.tt.t.ProjectId
        //                        , ttt.tt.Title
        //                        , ttt.Name
        //                        , d.Date
        //                        , ttt.tt.t.Duration
        //                        , ttt.tt.t.BioStatId
        //                    }
        //                )
        //                .Where(a => a.BioStatId == bioStatId);                   
              
        //    GridViewTimeEntry.DataSource = query.ToList();
        //    GridViewTimeEntry.DataBind();
        //}

        private int GetDate(string dateEntered)
        {
            int dateKey = -1;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                DateTime _dateEntered = Convert.ToDateTime(dateEntered);

                var entryDate = context.Date1
                                .First(d => d.Date == _dateEntered);

                if (entryDate != null)
                {
                    dateKey = entryDate.DateKey;
                }
            }

            return dateKey;
        }

        private bool ValidateControl()
        {
            decimal timeEntry;
            bool result = Decimal.TryParse(TextBoxTime.Text, out timeEntry);

            return result;
        }

        private bool ValidateEdit()
        {
            decimal timeEntry;
            bool result1 = Decimal.TryParse(TextBoxEditTime.Text, out timeEntry);

            DateTime dateEntry;
            bool result2 = DateTime.TryParse(TextBoxEditDate.Text, out dateEntry);

            int i = ddlEditServiceType.SelectedIndex;

            return result1 && result2 && (i > 0) && (dateEntry.Month.ToString() == ddlMonth.SelectedValue);
        }

        protected void ddlBioStat_Changed(Object sender, EventArgs e)
        {
            int biostatId;
            Int32.TryParse(ddlBioStat.SelectedValue, out biostatId);

            if (biostatId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var biostat = context.BioStats.First(x => x.Id == biostatId);

                    if (biostat != null)
                    {
                        var projects = context.ProjectBioStats
                                        .Where(p => p.BioStats_Id == biostat.Id);

                        if (projects.Count() > 0)
                        {
                            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                            dropDownSource = projects
                                            .OrderByDescending(d => d.Project.Invest.Id)
                                            .Select(x => new { x.Project.Invest.Id, FullName = x.Project.Invest.FirstName + " " + x.Project.Invest.LastName })
                                            .Distinct()
                                            .ToDictionary(c => c.Id, c => c.FullName);
                            BindDropDownList(ddlPI, dropDownSource, string.Empty);

                            dropDownSource = projects
                                            .OrderByDescending(d => d.Project.Id)
                                            .Select(x => new { x.Project.Id, FullName = x.Project.Id + " " + x.Project.Title })
                                            .ToDictionary(c => c.Id, c => c.FullName);

                            BindDropDownList(ddlProject, dropDownSource, string.Empty);

                            //BindGrid(context, GridViewTimeEntry);
                            BindGridView();

                        }

                    }
                }

            }
        }

        protected void ddlPI_Changed(Object sender, EventArgs e)
        {
            int biostatId;
            bool result = Int32.TryParse(ddlBioStat.SelectedValue, out biostatId);

            int investId;
            bool piResult = Int32.TryParse(ddlPI.SelectedValue, out investId);

            if (biostatId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
                    var projects = context.ProjectBioStats
                                   .Where(p => p.BioStats_Id == biostatId);

                    if (investId > 0)
                    {
                        projects = projects
                                    .Where(p => p.Project.InvestId == investId);
                    }

                    dropDownSource = projects
                                        .OrderByDescending(d => d.Project.Id)
                                        .Select(x => new { x.Project.Id, FullName = x.Project.Id + " " + x.Project.Title })
                                        .ToDictionary(c => c.Id, c => c.FullName);

                    if (projects.Count() > 0)
                    {
                        BindDropDownList(ddlProject, dropDownSource, string.Empty);
                    }

                }
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

                    //ddlBioStat
                    if (Page.User.IsInRole("Admin"))
                    {
                        dropDownSource = context.BioStats
                                        .OrderBy(b => b.Id)
                                        .ToDictionary(c => c.Id, c => c.Name);

                        BindDropDownList(ddlBioStat, dropDownSource, string.Empty);
                    }
                    else
                    {
                        var biostat = context.BioStats
                                        .First(b => b.LogonId == userName);

                        dropDownSource.Add(biostat.Id, biostat.Name);

                        BindDropDownList(ddlBioStat, dropDownSource, null);
                    }

                    //ddlProject
                    if (Page.User.IsInRole("Admin"))
                    {
                        dropDownSource = context.Projects
                                    .OrderByDescending(d => d.Id)
                                    .Select(x => new { x.Id, FullName = x.Id + " " + x.Title })
                                    .ToDictionary(c => c.Id, c => c.FullName);
                    }
                    else
                    {
                        int biostatId;
                        Int32.TryParse(ddlBioStat.SelectedValue, out biostatId);

                        if (biostatId > 0)
                        {
                            var projects = context.ProjectBioStats
                                            .Where(p => p.BioStats_Id == biostatId);

                            if (projects.Count() > 0)
                            {
                                dropDownSource = projects
                                                .OrderByDescending(d => d.Project.Invest.Id)
                                                .Select(x => new { x.Project.Invest.Id, FullName = x.Project.Invest.FirstName + " " + x.Project.Invest.LastName })
                                                .Distinct()
                                                .ToDictionary(c => c.Id, c => c.FullName);
                                BindDropDownList(ddlPI, dropDownSource, string.Empty);

                                dropDownSource = projects
                                                .OrderByDescending(d => d.Project.Id)
                                                .Select(x => new { x.Project.Id, FullName = x.Project.Id + " " + x.Project.Title })
                                                .ToDictionary(c => c.Id, c => c.FullName);
                            }
                        }
                    }

                    BindDropDownList(ddlProject, dropDownSource, string.Empty);

                    //ddlServiceType and ddlEditServiceType
                    dropDownSource = context.ServiceTypes
                                    .OrderBy(b => b.Id)
                                    .Select(x => new { x.Id, x.Name })
                                    .ToDictionary(c => c.Id, c => c.Name);

                    BindDropDownList(ddlServiceType, dropDownSource, string.Empty);

                    BindDropDownList(ddlEditServiceType, dropDownSource, string.Empty);


                    TextBoxSubmitDate.Text = HiddenFieldCurrentDate.Value;

                    //ddlMonth
                    int currentYear = DateTime.Now.Year;
                    string currentMonth = DateTime.Now.Month.ToString();

                    dropDownSource = context.Date1.Where(d => d.Year == currentYear)
                                        .Select(x => new { id = (int)x.MonthOfYear, x.Month })
                                        .Distinct()
                                        .OrderBy(i => i.id)
                                        .ToDictionary(c => c.id, c => c.Month);
                    
                    if (!Page.User.IsInRole("Admin"))
                    {
                        dropDownSource = dropDownSource.Select(i => i).Where(m => m.Key.ToString() == currentMonth).ToDictionary(c => c.Key, c => c.Value);    
                    }

                    BindDropDownList(ddlMonth, dropDownSource, null);
                    ddlMonth.SelectedValue = currentMonth;

                    BindGridView();
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

        protected void GridViewTimeEntry_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewTimeEntry.Rows[e.RowIndex];

            Label lblId = row.FindControl("lblId") as Label;

            if (lblId != null)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    int id = Convert.ToInt32(lblId.Text);
                    TimeEntry te = context.TimeEntries.First(t => t.Id == id);
                    context.TimeEntries.Remove(te);
                    context.SaveChanges();
                }

                BindGridView();

                if (GridViewTimeEntry.Rows.Count > 0)
                {
                    GridViewTimeEntry.SelectRow(GridViewTimeEntry.Rows.Count - 1);
                    GridViewTimeEntry.SelectedRow.Focus();
                }

            }
        }

    }
}