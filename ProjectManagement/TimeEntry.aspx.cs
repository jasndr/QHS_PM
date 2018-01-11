﻿using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//using System.IO;
//using System.Data;
//using ClosedXML.Excel;

namespace ProjectManagement
{
    public partial class TimeEntry1 : System.Web.UI.Page
    {
        string _currentDate = DateTime.Now.ToShortDateString();
        static DateTime _leadDate = DateTime.Now.AddDays(-50);
        static int _leadTime = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeEntryLeadTime"]);
        decimal _totalTime = 0M;

        string strProjectId;
        protected void Page_Load(object sender, EventArgs e)
        {
            strProjectId = Request.QueryString["Id"];

            if (!Page.IsPostBack)
            {
                BindControl(strProjectId);
            }
        }
        
        protected void GridViewTimeEntry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTime = (Label)e.Row.FindControl("lblTime");

                decimal timeEntry = Decimal.Parse(lblTime.Text);

                _totalTime += timeEntry;

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalTime = (Label)e.Row.FindControl("lblTotalTime");

                lblTotalTime.Text = _totalTime.ToString();
            }

        }

        protected void GridViewTimeEntry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {
                //int index = Convert.ToInt32(e.CommandArgument);
                //GridViewRow row = GridViewTimeEntry.Rows[index];

                GridViewRow row = ((GridViewRow)(((LinkButton)e.CommandSource).NamingContainer));

                if (row != null)
                {
                    LoadEditTimeEntry(row);
                }
            }
        }

        private void LoadEditTimeEntry(GridViewRow row)
        {
            lblEditId.Text = ((Label)row.FindControl("lblId")).Text;
            lblEditProjectId.Text = ((Label)row.FindControl("lblProjectId")).Text;
            lblEditProject.Text = ((Label)row.FindControl("lblProjectName")).Text;
            lblEditPhase.Text = ((Label)row.FindControl("lblPhase")).Text;

            int projectId = 0;
            if (Int32.TryParse(lblEditProjectId.Text, out projectId))
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var projectPhases = context.ProjectPhase.Where(p => p.ProjectId == projectId).ToList();

                    IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                    dropDownSource = context.ProjectPhase.Where(p => p.ProjectId == projectId && p.IsDeleted == false)
                                    .OrderBy(d => d.Id)
                                    .Select(x => new { x.Id, x.Name })
                                    .Distinct()
                                    .ToDictionary(c => c.Id, c => c.Name);
                    //PageUtility.BindDropDownList(ddlPI, dropDownSource, string.Empty);

                    PageUtility.BindDropDownList(ddlPhaseEdit, dropDownSource, null);
                    ddlPhaseEdit.ClearSelection();
                    ddlPhaseEdit.Items.FindByText(lblEditPhase.Text).Selected = true; 
                }
            }

            TextBoxEditDate.Text = Convert.ToDateTime(((Label)row.FindControl("lblDate")).Text).ToShortDateString();
            TextBoxEditTime.Text = ((Label)row.FindControl("lblTime")).Text;

            TextBoxEditDesc.Text = ((Label)row.FindControl("lblDesc")).Text;

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
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            if (ValidateDate())
            {
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
            }
            else
            {
                sb.Append("alert('History record can not be changed.');");
            }

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
        }

        private void UpdateTimeEntry()
        {
            int id;
            TimeEntry timeEntry;

            if (Int32.TryParse(lblEditId.Text, out id))
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    timeEntry = context.TimeEntries.First(t => t.Id == id);

                    if (timeEntry != null)
                    {
                        timeEntry.ServiceTypeId = Convert.ToInt32(ddlEditServiceType.SelectedValue);
                        timeEntry.DateKey = GetDateKey(TextBoxEditDate.Text);
                        timeEntry.Duration = Convert.ToDecimal(TextBoxEditTime.Text);
                        timeEntry.Creator = Page.User.Identity.Name;
                        timeEntry.CreationDate = Convert.ToDateTime(_currentDate);
                        timeEntry.Description = TextBoxEditDesc.Text;
                        timeEntry.PhaseId = Convert.ToInt32(ddlPhaseEdit.SelectedValue);

                        context.SaveChanges();
                    }
                }

                BindGridView();

                BindrptPhase(timeEntry.ProjectId); 
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {            
            if (ValidateControl().Equals(string.Empty))
            {
                if (ValidateSubmitDate() || Page.User.IsInRole("Admin"))
                {
                    var timeOverspend = false;
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        int phaseId = 0;
                        Int32.TryParse(ddlPhase.SelectedValue, out phaseId);

                        TimeEntry timeEntry = new TimeEntry()
                        {
                            ProjectId = Convert.ToInt32(Request.Form[ddlProject.UniqueID]),
                            BioStatId = Convert.ToInt32(ddlBioStat.SelectedValue),
                            Duration = Convert.ToDecimal(TextBoxTime.Text),
                            ServiceTypeId = Convert.ToInt32(ddlServiceType.SelectedValue),
                            DateKey = GetDateKey(TextBoxSubmitDate.Text),
                            Creator = Page.User.Identity.Name,
                            CreationDate = Convert.ToDateTime(_currentDate),
                            Description = TextBoxDesc.Text,
                            PhaseId = phaseId > 0 ? phaseId : (int?)null
                        };

                        //check if time spent total exceeds phase hour
                        var biostat = context.BioStats.FirstOrDefault(b => b.Id == timeEntry.BioStatId);
                        var phase = ddlPhase.SelectedItem.Text;

                        var ja = JArray.Parse(textAreaPhase.Value);

                        JObject jo = ja.Children<JObject>().FirstOrDefault(o => o["Name"].ToString() == phase);
                        if (jo != null && biostat != null)
                        {
                            var hours = biostat.Type == "phd" ? "PhdHrs" : "MsHrs";
                            var spent = biostat.Type == "phd" ? "PhdSpt" : "MsSpt";

                            decimal dh = 0.0m, ds = 0.0m;
                            if (decimal.TryParse(jo[hours].ToString(), out dh) &&
                                decimal.TryParse(jo[spent].ToString(), out ds))
                            {
                                if (ds + timeEntry.Duration > dh)
                                    timeOverspend = true;
                            }
                        }

                        if (timeOverspend)
                        {
                            Response.Write("<script>alert('Spent hours can not exceed estimated hours for this phase, please create new phase.');</script>");
                        }
                        else
                        {
                            context.TimeEntries.Add(timeEntry);
                            context.SaveChanges();

                            Response.Write("<script>alert('Time Entry Saved.');</script>");

                            BindGridView();

                            int projectId;
                            bool result = Int32.TryParse(ddlProject.SelectedValue, out projectId);
                            BindrptPhase(projectId);

                            ClearForm();
                        }

                        int investId;
                        Int32.TryParse(ddlBiostatChosen.SelectedValue, out investId);

                        if (investId > 0)
                        {
                            var projects = context.Project2
                                           .OrderByDescending(d => d.Id)
                                           .Where(p => p.PIId == investId);
                        }
                    }
                   
                }
                else
                {
                    Response.Write("<script>alert('The date you picked is not valid.');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('" + ValidateControl() + "');</script>");
            }
            
            //Response.Redirect(Request.Url.ToString());
        }

        private void ClearForm()
        {
            //clear projects, servicetype, description and time
            //ddlProject.ClearSelection();
            ddlServiceType.ClearSelection();
            TextBoxDesc.Text = string.Empty;
            TextBoxTime.Text = "0.5";

            //BindrptPhase(-1);
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
            //BindGridView();
        }

        protected void btnMonthly_Click(Object sender, EventArgs e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            int bioStatId, monthId, yearId;
            Int32.TryParse(ddlBioStat.SelectedValue, out bioStatId);
            Int32.TryParse(ddlMonth.SelectedValue, out monthId);
            Int32.TryParse(ddlYear.SelectedValue, out yearId);

            if (bioStatId > 0 && monthId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var query = context.TimeEntries
                                .Join(context.Project2
                                    , t => t.ProjectId
                                    , p => p.Id
                                    , (t, p) => new { t, p.Title, phase = t.ProjectPhase.Name }
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
                                        ttt.tt.phase
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
                                        ,
                                        ttt.tt.t.Description
                                    }
                                )
                                .Where(a => a.BioStatId == bioStatId && a.Date.Year == yearId && a.Date.Month == monthId)
                                .OrderByDescending(t => t.Id);

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


        protected void ddlBioStat_Changed(Object sender, EventArgs e)
        {
            //ddlPI.ClearSelection();
            ddlProject.ClearSelection();

            int biostatId;
            Int32.TryParse(ddlBioStat.SelectedValue, out biostatId);

            if (biostatId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var biostat = context.BioStats.First(x => x.Id == biostatId);

                    if (biostat != null)
                    {
                        //var projects = context.ProjectBioStats
                        //    .Where(p => p.BioStats_Id == biostat.Id
                        //                && (p.Project.ProjectCompletionDate == null || p.Project.ProjectCompletionDate > _leadDate));
                        var projects = context.vwProjectBiostat.Where(v => v.BiostatId == biostat.Id);

                        if (projects.Count() > 0)
                        {
                            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                            dropDownSource = projects
                                            .OrderByDescending(d => d.FirstName)
                                            .Select(x => new { x.InvestId, FullName = x.FirstName + " " + x.LastName })
                                            .Distinct()
                                            .ToDictionary(c => c.InvestId, c => c.FullName);
                            //PageUtility.BindDropDownList(ddlPI, dropDownSource, string.Empty);

                            PageUtility.BindDropDownList(ddlBiostatChosen, dropDownSource, string.Empty);

                            dropDownSource = projects
                                            .OrderByDescending(d => d.ProjectId)
                                            .Select(x => new { x.ProjectId, FullName = (x.ProjectId + " " + x.FirstName + " " + x.LastName + " | " + x.Title).Substring(0, 188) })
                                            .Distinct()
                                            .ToDictionary(c => c.ProjectId, c => c.FullName);

                            PageUtility.BindDropDownList(ddlProject, dropDownSource, string.Empty);

                            //BindGrid(context, GridViewTimeEntry);
                            BindGridView();

                        }

                    }
                }

                BindrptPhase(-1);
            }
        }

        protected void ddlProject_Changed(Object sender, EventArgs e)
        {
            ddlPhase.Items.Clear();

            int projectId;
            bool result = Int32.TryParse(ddlProject.SelectedValue, out projectId);

            BindrptPhase(projectId);

        }

        private void BindrptPhase(int projectId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Desc");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("PhdHrs");
            dt.Columns.Add("MsHrs");
            dt.Columns.Add("PhdSpt");
            dt.Columns.Add("MsSpt");

            if (projectId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var phases = context.ProjectPhase.Where(p => p.ProjectId == projectId).OrderByDescending(p => p.Id).ToList();

                    foreach (var phase in phases)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = phase.Id.ToString();
                        newItem.Text = phase.Name;
                        newItem.Selected = false;

                        ddlPhase.Items.Add(newItem);

                        DataRow dr = dt.NewRow();
                        dr[0] = phase.Name;
                        dr[1] = phase.Title;
                        dr[2] = phase.StartDate != null ? Convert.ToDateTime(phase.StartDate).ToShortDateString() : "";
                        dr[3] = phase.PhdHrs;
                        dr[4] = phase.MsHrs;

                        //total spent hours
                        decimal p = 0.5m, m = 0.5m;
                        DateTime startDate = new DateTime(2000, 1, 1), endDate = new DateTime(2099, 1, 1);
                        //ObjectParameter startDate = new ObjectParameter("StartDate", typeof(DateTime?));
                        //ObjectParameter endDate = new ObjectParameter("EndDate", typeof(DateTime?));
                        ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                        ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));
                        var i = context.P_PROJECTPHASE_HOURS(projectId, phase.Name, startDate, endDate, phdHours, msHours);
                        context.SaveChanges();

                        Decimal.TryParse(phdHours.Value.ToString(), out p);
                        Decimal.TryParse(msHours.Value.ToString(), out m);

                        dr[5] = p;
                        dr[6] = m;

                        dt.Rows.Add(dr);

                    }

                    //int piId = 0;
                    //Int32.TryParse(ddlBiostatChosen.SelectedValue, out piId);

                    //if (piId > 0)
                    //{
                    //    //rebind ddlProject
                    //}
                }
            }

            if (dt.Rows.Count == 0)
            {
                DataRow emptyRow = dt.NewRow();
                dt.Rows.Add(emptyRow);
            }
            else
            {
                textAreaPhase.Value = JsonConvert.SerializeObject(dt); //.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
            }

            rptPhase.DataSource = dt;
            rptPhase.DataBind();
        }
      
        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static int GetPI(int projectId)
        {
            int piId = 0;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var project = context.Project2.Where(p => p.Id == projectId).FirstOrDefault();
                piId = project.PIId;
            }

            return piId;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static List<ProjectPI> GetProjectList(int biostatId, int piId)
        {           
            List<ProjectPI> lstProject = new List<ProjectPI>();

            if (biostatId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {

                    //var projects = context.ProjectBioStats
                    //               .OrderByDescending(d => d.Project.Id)
                    //               .Where(p => p.BioStats_Id == biostatId && (p.Project.ProjectCompletionDate == null || p.Project.ProjectCompletionDate > _leadDate));
                    var projects = context.vwProjectBiostat.Where(v => v.BiostatId == biostatId);

                    if (piId > 0)
                    {
                        projects = projects
                                       .OrderByDescending(d => d.ProjectId)
                                       .Where(p => p.InvestId == piId);                       
                    }

                    foreach (var p in projects.ToList())
                    {
                        lstProject.Add(new ProjectPI()
                        {
                            Id = p.ProjectId,
                            Title = p.Title,
                            PIFirstName = string.Empty,
                            PILastName = string.Empty,
                            InitialDate = string.Empty
                        });
                    }
                }
            }

            return lstProject;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetPIList(int biostatId, string piName)
        {
            List<String> lstPI = new List<String>();          
            
            if (biostatId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    //var projects = context.ProjectBioStats
                    //                        .Where(p => p.BioStats_Id == biostatId && (p.Project.ProjectCompletionDate == null || p.Project.ProjectCompletionDate > _leadDate));
                    var projects = context.vwProjectBiostat.Where(v => v.BiostatId == biostatId);

                    if (projects.Count() > 0)
                    {
                        var query = projects
                                    .Where(a => a.LastName.StartsWith(piName) || a.FirstName.StartsWith(piName))
                                    .Select(x => new { FullName = x.FirstName + " " + x.LastName })
                                    .Distinct();

                        foreach (var pi in query.ToList())
                        {
                            lstPI.Add(pi.FullName);
                        }
                    }
                }
            }

            return lstPI;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static List<ViewInvoice> GetInvoiceData(int projectId)
        {
            List<ViewInvoice> lstInvoice = new List<ViewInvoice>();

            if (projectId > 0)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    var query = context.ViewInvoices
                                .Where(p => p.ProjectId == projectId)
                                .OrderBy(i => i.InvoiceId);

                    lstInvoice = query.ToList();
                }
            }

            return lstInvoice;
        }

        //protected void ddlPI_Changed(Object sender, EventArgs e)
        //{
        //    int biostatId;
        //    bool result = Int32.TryParse(ddlBioStat.SelectedValue, out biostatId);

        //    int investId;
        //    bool piResult = Int32.TryParse(ddlPI.SelectedValue, out investId);

        //    if (biostatId > 0)
        //    {
        //        using (ProjectTrackerContainer context = new ProjectTrackerContainer())
        //        {
        //            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
        //            var projects = context.ProjectBioStats
        //                           .Where(p => p.BioStats_Id == biostatId && p.Project.IsApproved && p.Project.IsProject && (p.Project.ProjectCompletionDate == null || p.Project.ProjectCompletionDate > _leadDate));

        //            if (investId > 0)
        //            {
        //                projects = projects
        //                            .Where(p => p.Project.InvestId == investId);
        //            }

        //            dropDownSource = projects
        //                                .OrderByDescending(d => d.Project.Id)
        //                                .Select(x => new { x.Project.Id, FullName = (x.Project.Id + " " + x.Project.Invest.FirstName + " " + x.Project.Invest.LastName + " | " + x.Project.Title).Substring(0, 108) })
        //                                .ToDictionary(c => c.Id, c => c.FullName);

        //            if (projects.Count() > 0)
        //            {
        //                PageUtility.BindDropDownList(ddlProject, dropDownSource, string.Empty);
        //            }

        //        }
        //    }
        //}        

        private void BindControl(string projectId)
        {
            string userName = Page.User.Identity.Name;
            _currentDate = DateTime.Now.ToShortDateString();

            if (!userName.Equals(string.Empty))
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    //var dbQuery = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DateTime>("CurrentDateTime() ");
                    //HiddenFieldCurrentDate.Value = dbQuery.AsEnumerable().First().ToString();
                    //_currentDate = HiddenFieldCurrentDate.Value;
                    
                    _leadDate = DateTime.Now.AddDays(-50);                    

                    //ddlBioStat
                    var biostat = context.BioStats.FirstOrDefault(b => b.LogonId == userName);
                    if (biostat != null)
                    {
                        if (Page.User.IsInRole("Admin"))
                        {
                            dropDownSource = context.BioStats
                                            .Where(b => b.Id > 0 && b.Id < 90)
                                            .OrderBy(b => b.Id)
                                            .ToDictionary(c => c.Id, c => c.Name);

                            PageUtility.BindDropDownList(ddlBioStat, dropDownSource, string.Empty);
                            ddlBioStat.Items.FindByText(biostat.Name).Selected = true;
                        }
                        else
                        {
                            dropDownSource.Add(biostat.Id, biostat.Name);

                            PageUtility.BindDropDownList(ddlBioStat, dropDownSource, null);
                        }

                        int biostatId;
                        Int32.TryParse(ddlBioStat.SelectedValue, out biostatId);

                        if (biostatId > 0)
                        {
                            //var projects = context.ProjectBioStats
                            //                .Where(p => p.BioStats_Id == biostatId && (p.Project.ProjectCompletionDate == null || p.Project.ProjectCompletionDate > _leadDate));
                            var projects = context.vwProjectBiostat.Where(v => v.BiostatId == biostatId);

                            if (projects.Count() > 0)
                            {
                                dropDownSource = projects
                                                .OrderByDescending(d => d.InvestId)
                                                .Select(x => new { x.InvestId, FullName = x.FirstName + " " + x.LastName })
                                                .Distinct()
                                                .ToDictionary(c => c.InvestId, c => c.FullName);

                                PageUtility.BindDropDownList(ddlBiostatChosen, dropDownSource, "Search PI");

                                
                                int piId = 0;
                                Int32.TryParse(ddlBiostatChosen.SelectedValue, out piId);

                                if (piId > 0)
                                {
                                    projects = projects.Where(p => p.InvestId == piId);
                                }

                                dropDownSource = projects
                                                .OrderByDescending(d => d.ProjectId)
                                                .Select(x => new { x.ProjectId, FullName = (x.ProjectId + " " + x.FirstName + " " + x.LastName + " | " + x.Title).Substring(0, 108) })
                                                .Distinct()
                                                .ToDictionary(c => c.ProjectId, c => c.FullName);                                    

                                PageUtility.BindDropDownList(ddlProject, dropDownSource, string.Empty);

                                ddlProject.SelectedValue = projectId;

                                int _projectId;
                                bool result = Int32.TryParse(ddlProject.SelectedValue, out _projectId);

                                BindrptPhase(_projectId); 
                            }
                        }
                    }

                    //ddlServiceType and ddlEditServiceType
                    dropDownSource = context.ServiceTypes
                                    .OrderBy(b => b.Id)
                                    .Select(x => new { x.Id, x.Name })
                                    .Distinct().ToDictionary(c => c.Id, c => c.Name);

                    PageUtility.BindDropDownList(ddlServiceType, dropDownSource, string.Empty);

                    PageUtility.BindDropDownList(ddlEditServiceType, dropDownSource, string.Empty);


                    TextBoxSubmitDate.Text = Convert.ToDateTime(_currentDate).ToShortDateString();

                    //ddlMonth
                    int currentYear = DateTime.Now.Year;
                    string currentMonth = DateTime.Now.Month.ToString();

                    dropDownSource = context.Date1.Where(d => d.Year > 2014 )
                                       .Select(x => new { id = (int)x.Year, x.Year })
                                       .Distinct()
                                       .OrderBy(i => i.id)
                                       .ToDictionary(c => c.id, c => c.Year.ToString());
                    PageUtility.BindDropDownList(ddlYear, dropDownSource, null);
                    ddlYear.SelectedValue = currentYear.ToString();

                    int yearSelected = currentYear;
                    Int32.TryParse(ddlYear.SelectedValue, out yearSelected);
                    dropDownSource = context.Date1.Where(d => d.Year == yearSelected)
                                        .Select(x => new { id = (int)x.MonthOfYear, x.Month })
                                        .Distinct()
                                        .OrderBy(i => i.id)
                                        .ToDictionary(c => c.id, c => c.Month);

                    //if (!Page.User.IsInRole("Admin"))
                    //{
                    //    dropDownSource = dropDownSource.Select(i => i).Where(m => m.Key.ToString() == currentMonth).ToDictionary(c => c.Key, c => c.Value);
                    //}

                    PageUtility.BindDropDownList(ddlMonth, dropDownSource, null);
                    ddlMonth.SelectedValue = currentMonth;

                   

                    BindGridView();
                }

                //BindGridViewProjectEffort(-1);

                //phase
                dropDownSource = new Dictionary<int, string>();
                for (int p = 0; p < 10; p++)
                {
                    string s = string.Format("Phase-{0}", p);
                    dropDownSource.Add(p, s);
                }
                PageUtility.BindDropDownList(ddlPhaseHdn, dropDownSource, "--- Select ---");
                //BindgvPhase(-1);
                   
            }
        }

        //private void BindgvPhase(int rowIndex)
        //{
        //    DataTable dt = CreatePhaseTable(null, false);
        //    DataRow dr;

        //    if (dt.Rows.Count == 0 || rowIndex < 0)
        //    {
        //        dr = dt.NewRow();
        //        dt.Rows.Add(dr);
        //    }

        //    foreach (GridViewRow row in gvPhase.Rows)
        //    {
        //        if (row.RowIndex != rowIndex)
        //        {
        //            dr = dt.NewRow();

        //            Label rowId = row.FindControl("lblId") as Label;
        //            DropDownList ddlPhase = row.FindControl("ddlPhase") as DropDownList;
        //            TextBox txtStartDate = row.FindControl("txtStartDate") as TextBox;
        //            TextBox txtTitle = row.FindControl("txtTitle") as TextBox;
        //            TextBox txtMsHrs = row.FindControl("txtMsHrs") as TextBox;
        //            TextBox txtPhdHrs = row.FindControl("txtPhdHrs") as TextBox;
        //            DropDownList ddlServiceType = row.FindControl("ddlServiceType") as DropDownList;

        //            dr[0] = rowId.Text;
        //            dr[1] = ddlPhase.SelectedItem.Text;
        //            dr[2] = txtTitle.Text;
        //            dr[3] = txtMsHrs.Text;
        //            dr[4] = txtPhdHrs.Text;
        //            dr[5] = txtStartDate.Text;
        //            dr[6] = ddlServiceType.SelectedValue;

        //            dt.Rows.Add(dr);
        //        }
        //    }          

        //    gvPhase.DataSource = dt;
        //    gvPhase.DataBind();
        //}

        private DataTable CreatePhaseTable(ICollection<ProjectPhase> phases, bool hasRow)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Title");
            dt.Columns.Add("MsHrs");
            dt.Columns.Add("PhdHrs");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("ServiceTypeId");

            if (phases != null && phases.Count > 0)
            {
                foreach (var phase in phases)
                {
                    //decimal dMsOut = 0.0m,
                    //        dPhdOut = 0.0m;
                    DataRow dr = dt.NewRow();
                    dr[0] = phase.Id;
                    dr[1] = phase.Name;
                    dr[2] = phase.Title;
                    dr[3] = phase.MsHrs;
                    dr[4] = phase.PhdHrs;
                    dr[5] = phase.StartDate != null ? Convert.ToDateTime(phase.StartDate).ToShortDateString() : "";
                    //dr[6] = phase.TimeEntries.se

                    dt.Rows.Add(dr);
                }
            }
            else if (hasRow)
            {
                dt.Rows.Add();
            }

            return dt;
        }

        protected void gvPhase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = e.Row.FindControl("ddlPhase") as DropDownList;

                string id = (e.Row.FindControl("lblPhase") as Label).Text;

                if (ddl != null)
                {
                    //ddl.Items.AddRange(ddlBiostat.Items.OfType<ListItem>().ToArray());
                    foreach (ListItem li in ddlPhaseHdn.Items)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = li.Value;
                        newItem.Text = li.Text;
                        newItem.Selected = false;

                        if (li.Text == id)
                        {
                            newItem.Selected = true;
                        }

                        ddl.Items.Add(newItem);
                    }

                }
            }
        }

        //protected void gvPhase_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    GridViewRow row = gvPhase.Rows[e.RowIndex];

        //    BindgvPhase(e.RowIndex);
        //}

        //protected void btnAddPhase_Click(object sender, EventArgs e)
        //{
        //    BindgvPhase(-1);
        //}
               
        //private void BindGridViewProjectEffort(int projectId)
        //{
        //    System.Data.DataTable dummy = new System.Data.DataTable();
        //    dummy.Columns.Add("InvoiceId");
        //    dummy.Columns.Add("ReqNum");
        //    dummy.Columns.Add("ReqDate");
        //    dummy.Columns.Add("FromDate");
        //    dummy.Columns.Add("ToDate");
        //    dummy.Columns.Add("PhdReq");
        //    dummy.Columns.Add("PhdSpt");
        //    dummy.Columns.Add("MsReq");
        //    dummy.Columns.Add("MsSpt");
        //    dummy.Rows.Add();
        //    GridViewProjectEffort.DataSource = dummy;
        //    GridViewProjectEffort.DataBind();

        //    GridViewProjectEffort.Rows[0].Cells.Clear();
        //}   

        
        protected void GridViewTimeEntry_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewTimeEntry.Rows[e.RowIndex];

            Label lblId = row.FindControl("lblId") as Label;

            if (lblId != null)
            {
                bool validDate = ValidateDate();

                if (validDate)
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
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append("alert('Can not change history input.');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CantDeleteScript", sb.ToString(), false);
                    //Response.Write("<script>alert('Can not delete history input.');</script>");
                }

            }
        }

        private int GetDateKey(string dateEntered)
        {
            int dateKey = -1;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                DateTime _dateEntered = Convert.ToDateTime(Convert.ToDateTime(dateEntered).ToShortDateString());

                var entryDate = context.Date1
                                .First(d => d.Date == _dateEntered);

                if (entryDate != null)
                {
                    dateKey = entryDate.DateKey;
                }
            }

            return dateKey;
        }

        private string ValidateControl()
        {
            System.Text.StringBuilder validateResult = new System.Text.StringBuilder();

            //if (ddlBioStat.SelectedValue.Equals(string.Empty))
            //{
            //    validateResult.Append("Biostat is required. \\n");
            //}

            //if (ddlPI.SelectedValue.Equals(string.Empty))
            //{
            //    validateResult.Append("PI is required. \\n");
            //}

            //if (ddlProject.SelectedValue.Equals(string.Empty))
            //{
            //    validateResult.Append("Project is required. \\n");
            //}

            //if (ddlServiceType.SelectedValue.Equals(string.Empty))
            //{
            //    validateResult.Append("Service Type is required. \\n");
            //}

            //DateTime dateValue;
            //if (!DateTime.TryParse(TextBoxSubmitDate.Text, out dateValue))
            //{
            //    validateResult.Append("Submit date is required. \\n");
            //}

            decimal timeEntry;
            if (!Decimal.TryParse(TextBoxTime.Text, out timeEntry))
            {
                validateResult.Append("Time is not decimal. \\n");
            }

            return validateResult.ToString();
        }

        private bool ValidateEdit()
        {
            decimal timeEntry;
            bool result1 = Decimal.TryParse(TextBoxEditTime.Text, out timeEntry);

            DateTime dateEntry;
            bool result2 = DateTime.TryParse(TextBoxEditDate.Text, out dateEntry);

            int i = ddlEditServiceType.SelectedIndex;

            return result1 && result2 && (i > 0) && (timeEntry > Decimal.Zero); //&& (dateEntry.Month.ToString() == ddlMonth.SelectedValue);
        }

        private bool ValidateDate()
        {
            //var firstDay = new DateTime(entryDate.Year, entryDate.Month, 1);
            var firstDay = new DateTime(Convert.ToDateTime(_currentDate).Year, ddlMonth.SelectedIndex+1, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var dateDiff = (int)(Convert.ToDateTime(_currentDate) - lastDay).TotalDays;

            return (dateDiff - _leadTime < 0);
        }

        private bool ValidateSubmitDate()
        {
            DateTime submitDate = Convert.ToDateTime(TextBoxSubmitDate.Text);
            var firstDay = new DateTime(submitDate.Year, submitDate.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var dateDiff = (int)(Convert.ToDateTime(_currentDate) - lastDay).TotalDays;

            return (dateDiff - _leadTime < 0);
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

    }
}