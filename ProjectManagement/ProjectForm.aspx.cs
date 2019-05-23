using Microsoft.AspNet.Identity;
using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;
using System.Reflection;
using System.Web.Services;
using System.Web.Script.Services;
using ProjectManagement.Models;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Text;
using System.Net.Mail;

namespace ProjectManagement
{
    public partial class ProjectForm : System.Web.UI.Page
    {        
        static readonly DateTime _endDate = DateTime.Parse("2099-01-01");
        string _currentDate = "2015-01-01";
        string projectId;
        int selectedProjectId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            projectId = Request.QueryString["Id"];

            if (!Page.IsPostBack)
            {
                BindControl();

                if (projectId != null)
                {
                    BindProject(projectId);
                    ddlProject.SelectedValue = projectId;                   
                }
            }          

            if (!HiddenFieldCurrentDate.Value.Equals(string.Empty))
            {
                _currentDate = HiddenFieldCurrentDate.Value;
            }

            if (!Page.User.IsInRole("Admin"))
            {
                divAdmin.Style["display"] = "none";
            }

            if (!Page.User.IsInRole("Super"))
            {
                btnSurvey.Style["display"] = "none";
            }  

            if (hdnInvoiceRequestNumber.Value.Equals(string.Empty))
            {
                Label lblRequestNumber = ((Label)dvInvoice.FindControl("lblRequestNumber"));

                if (lblRequestNumber != null)
                {
                    hdnInvoiceRequestNumber.Value = lblRequestNumber.Text;
                }                
            }
            if (hdnInvoiceRequestDate.Value.Equals(string.Empty))
            {
                Label lblRequestDate = ((Label)dvInvoice.FindControl("lblRequestDate"));

                if (lblRequestDate != null)
                {
                    hdnInvoiceRequestDate.Value = lblRequestDate.Text;
                }
            }
            if (hdnInvoiceRequestdHoursPhd.Value.Equals(string.Empty))
            {
                Label lblRequestedPhd = ((Label)dvInvoice.FindControl("lblRequestedPhd"));

                if (lblRequestedPhd != null)
                {
                    hdnInvoiceRequestdHoursPhd.Value = lblRequestedPhd.Text;
                }
            }
            if (hdnInvoiceRequestedHoursMS.Value.Equals(string.Empty))
            {
                Label lblRequestedMS = ((Label)dvInvoice.FindControl("lblRequestedMS"));

                if (lblRequestedMS != null)
                {
                    hdnInvoiceRequestedHoursMS.Value = lblRequestedMS.Text;
                }
            }
            if (hdnInvoiceApprovedHoursPhd.Value.Equals(string.Empty))
            {
                Label lblAprovedPhd = ((Label)dvInvoice.FindControl("lblAprovedPhd"));

                if (lblAprovedPhd != null)
                {
                    hdnInvoiceApprovedHoursPhd.Value = lblAprovedPhd.Text;
                }
            }
            if (hdnInvoiceApprovedHoursMS.Value.Equals(string.Empty))
            {
                Label lblAprovedMS = ((Label)dvInvoice.FindControl("lblAprovedMS"));

                if (lblAprovedMS != null)
                {
                    hdnInvoiceApprovedHoursMS.Value = lblAprovedMS.Text;
                }
            }

        }
                

        private void BindControl()
        {
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var dbQuery = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DateTime>("CurrentDateTime() ");
                HiddenFieldCurrentDate.Value = dbQuery.AsEnumerable().First().ToShortDateString();                

                //ddlInvestor
                dropDownSource = context.Invests
                                .OrderBy(d => d.FirstName).ThenBy(l => l.LastName)
                                .Select(x => new { x.Id, Name = x.FirstName + " " + x.LastName })
                                .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlInvestor, dropDownSource, String.Empty);

                //ddlProject
                dropDownSource = context.Projects
                                .OrderByDescending(d => d.Id)
                                .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title).Substring(0,99) })
                                .ToDictionary(c => c.Id, c => c.FullName);

                PageUtility.BindDropDownList(ddlProject, dropDownSource, "Add a new project");

                //GridViewGrant
                var query = context.GrantAffils
                            .OrderBy(d => d.Id);

                GridViewGrant.DataSource = query.ToList();
                GridViewGrant.DataBind();

                GridViewPilot.DataSource = query.ToList();
                GridViewPilot.DataBind();   

                //ddlBiostat
                DateTime currentDate = DateTime.Parse(HiddenFieldCurrentDate.Value);
                dropDownSource = context.BioStats
                                .Where(b => b.EndDate > currentDate)
                                .OrderBy(b => b.Name)
                                .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlBiostat, dropDownSource, String.Empty);

                GridViewBioStat.DataSource = dropDownSource;
                GridViewBioStat.DataBind();

                var queryServiceType = context.ServiceTypes; //.Where(s => s.Type == "Project");
                GridViewServiceType.DataSource = queryServiceType.ToList();
                GridViewServiceType.DataBind();

                //ddlHiProgram
                dropDownSource = context.HiPrograms
                                .Where(d => d.Id > 0 && d.isStudyArea)
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.Name);

                //PageUtility.BindDropDownList(ddlHiProgram, dropDownSource, null);

                GridViewStudyArea.DataSource = dropDownSource;
                GridViewStudyArea.DataBind();

                //HealthData
                dropDownSource = context.HiPrograms
                               .Where(d => d.Id > 0 && d.isHealthData)
                               .OrderBy(b => b.Id)
                               .ToDictionary(c => c.Id, c => c.Name);

                GridViewHealthData.DataSource = dropDownSource;
                GridViewHealthData.DataBind();

                //ddlEthnicGroup
                dropDownSource = context.EthnicGroups
                                  .OrderBy(e => e.Id)
                                  .ToDictionary(c => c.Id, c => c.GroupName);

                PageUtility.BindDropDownList(ddlEthnicGroup, dropDownSource, null);

                //GridViewStudyType
                dropDownSource = context.StudyTypes
                                .OrderBy(e => e.Id)
                                .ToDictionary(c => c.Id, c => c.Name);

                GridViewStudyType.DataSource = dropDownSource;
                GridViewStudyType.DataBind();

                //GridViewStudyPopulation
                dropDownSource = context.StudyPopulations
                                .OrderBy(e => e.Id)
                                .ToDictionary(c => c.Id, c => c.Name);

                GridViewStudyPop.DataSource = dropDownSource;
                GridViewStudyPop.DataBind();               

            }

            BindGridViewServiceDesc(-1);

            //phase
            dropDownSource = new Dictionary<int, string>();
            for (int p = 0; p < 10; p++)
            {
                string s = string.Format("Phase-{0}", p);
                dropDownSource.Add(p, s);
            }
            PageUtility.BindDropDownList(ddlPhaseHdn, dropDownSource, "--- Select ---");
            BindgvPhase(-1);
        }

        //[WebMethod]
        //[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        //public static List<ProjectPI> GetProjectList(int piId)
        //{
        //    List<ProjectPI> lstProject = new List<ProjectPI>();

        //    if (piId > 0)
        //    {
        //        using (ProjectTrackerContainer context = new ProjectTrackerContainer())
        //        {
        //            List<Project> projects = context.Projects
        //                                   .OrderByDescending(d => d.Id)
        //                                   .Where(p => p.InvestId == piId).ToList();
                    
        //            foreach (var p in projects)
        //            {
        //                lstProject.Add(new ProjectPI()
        //                {
        //                    Id = p.Id,
        //                    Title = p.Title,
        //                    PIFirstName = string.Empty,
        //                    PILastName = string.Empty,
        //                    InitialDate = string.Empty
        //                });
        //            }
        //        }
        //    }

        //    return lstProject;
        //}

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static List<decimal> GetProjectHours(int projectId, string startDate, string endDate)
        {
            List<decimal> projectHours = new List<decimal>();
            DateTime dateStart, dateEnd;

            if (DateTime.TryParse(startDate, out dateStart) && (DateTime.TryParse(endDate, out dateEnd)))
            {
                if (dateEnd > dateStart && projectId > 0)
                {
                    decimal p = 0.0M, m = 0.0M;
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                        ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));

                        //phdHours.Value = 1.0M;
                        //msHours.Value = 1.0M;

                        var i = context.P_PROJECT_HOURS(dateStart, dateEnd, projectId, phdHours, msHours);
                        context.SaveChanges();
                       
                        Decimal.TryParse(phdHours.Value.ToString(), out p);
                        Decimal.TryParse(msHours.Value.ToString(), out m);

                        projectHours.Add(p);
                        projectHours.Add(m);
                    }

                }
            }
            else
            {
                projectHours.Add(0.0m);
                projectHours.Add(0.0m);
            }     

            return projectHours;
        }


        protected void ddlProject_Changed(Object sender, EventArgs e)
        {
            ClearForm();

            string projectId = "0";
            if (ddlProject.SelectedValue.Equals(string.Empty))
            {                
                selectedProjectId = 0;
            }
            else
            {
                projectId = ddlProject.SelectedValue;
                
            }

            BindProject(projectId);

            //lblMsg.Text = selectedProjectId.ToString();
        }

        private void BindProject(string projectId)
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
                DateTime currentDate = DateTime.Parse(_currentDate);

                int id = 0;
                if (Int32.TryParse(projectId, out id))
                {
                    Project project = context.Projects.FirstOrDefault(x => x.Id == id);

                    if (project != null)
                    {
                        //repopulate ddlBiostat and GridViewBioStat                    
                        dropDownSource = context.BioStats
                                        .Where(b => b.EndDate > project.InitialDate)
                                        .OrderBy(b => b.Id)
                                        .ToDictionary(c => c.Id, c => c.Name);

                        PageUtility.BindDropDownList(ddlBiostat, dropDownSource, String.Empty);

                        GridViewBioStat.DataSource = dropDownSource;
                        GridViewBioStat.DataBind();

                        SetControlValue(project, currentDate);
                    }

                    selectedProjectId = id;
                }
                else
                {
                    dropDownSource = context.BioStats
                                       .Where(b => b.EndDate > currentDate)
                                       .OrderBy(b => b.Id)
                                       .ToDictionary(c => c.Id, c => c.Name);

                    PageUtility.BindDropDownList(ddlBiostat, dropDownSource, String.Empty);

                    GridViewBioStat.DataSource = dropDownSource;
                    GridViewBioStat.DataBind();
                }
            }
        }

        private void ClearForm()
        {
            lblProjectId.Text = string.Empty;

            TextBoxTitle.Text = string.Empty;
            TextBoxSummary.Text = string.Empty;
            TextBoxInitialDate.Text = string.Empty;
            TextBoxTargetDate.Text = string.Empty;
            TextBoxAnalysisCompletionDate.Text = string.Empty;
            TextBoxProjectCompletionDate.Text = string.Empty;
            TextBoxComment.Text = string.Empty;

            TextBoxMentorFirst.Text = string.Empty;
            TextBoxMentorLast.Text = string.Empty;
            TextBoxMentorEmail.Text = string.Empty;

            TextBoxRmatrixNum.Text = string.Empty;
            TextBoxRmatrixSubDate.Text = string.Empty;
            TextBoxRmatrixRcvDate.Text = string.Empty;

            chkInternal.Checked = false;
            chkJuniorPI.Checked = false;

            //ddlInvestor.ClearSelection();

            PageUtility.UncheckGrid(GridViewGrant);
            PageUtility.UncheckGrid(GridViewPilot);

            ddlBiostat.ClearSelection();

            PageUtility.UncheckGrid(GridViewBioStat);

            PageUtility.UncheckGrid(GridViewServiceType);

            PageUtility.UncheckGrid(GridViewStudyArea);
            TextBox txtStudyOther = GridViewStudyArea.FooterRow.FindControl("txtStudyOther") as TextBox;
            txtStudyOther.Text = string.Empty;

            PageUtility.UncheckGrid(GridViewHealthData);

            PageUtility.UncheckGrid(GridViewStudyType);
            TextBox txtStudyTypeOther = GridViewStudyType.FooterRow.FindControl("txtStudyTypeOther") as TextBox;
            txtStudyTypeOther.Text = string.Empty;

            PageUtility.UncheckGrid(GridViewStudyPop);
            TextBox txtStudyPopOther = GridViewStudyPop.FooterRow.FindControl("txtStudyPopOther") as TextBox;
            txtStudyPopOther.Text = string.Empty;

            ddlEthnicGroup.ClearSelection();

            TextBox textBoxGrantNew = GridViewGrant.FooterRow.FindControl("txtGrantNameNew") as TextBox;
            textBoxGrantNew.Text = string.Empty;

            chkApproved.Checked = false;

            BindGridViewServiceDesc(-1);
            BindInvoiceDetails(-1, DetailsViewMode.ReadOnly);

            gvPhase.DataSource = null;
            gvPhase.DataBind();
            BindgvPhase(-1);

            BindrptPhaseCompletion(null, false);

            ClearHiddenField();
            ClearViewState();
        }

        private void ClearViewState()
        {
            ViewState["RequestNumber"] = null;
            ViewState["RequestDate"] = null;
            ViewState["RequestedPhd"] = null;
            ViewState["RequestedMs"] = null;
            ViewState["ApproveDate"] = null;
            ViewState["ApprovedPhd"] = null;
            ViewState["ApprovedMs"] = null;
            ViewState["ClientApproveDate"] = null;
            ViewState["InvoiceFromDate"] = null;
            ViewState["InvoiceToDate"] = null;
            ViewState["InvoicedPhd"] = null;
            ViewState["InvoicedMs"] = null;
            ViewState["InvoiceDate"] = null;
            ViewState["InvoiceNumber"] = null;
            ViewState["Comment"] = null;
        }

        private void ClearHiddenField()
        {
            hdnInvoiceRequestNumber.Value = string.Empty;
            hdnInvoiceRequestDate.Value = string.Empty;
            hdnInvoiceRequestdHoursPhd.Value = string.Empty;
            hdnInvoiceRequestedHoursMS.Value = string.Empty;
            hdnInvoiceApprovedHoursPhd.Value = string.Empty;
            hdnInvoiceApprovedHoursMS.Value = string.Empty;
        }

        protected void GridViewGrant_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "InsertNew")
            {
                GridViewRow row = GridViewGrant.FooterRow;

                TextBox txtName = row.FindControl("txtNameNew") as TextBox;

                if (txtName != null)
                {
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        GrantAffil grant = new GrantAffil()
                        {
                            GrantAffilName = txtName.Text
                        };

                        context.GrantAffils.Add(grant);

                        SaveGrant(context, grant);

                        BindControl();
                    }
                }
            }
        }


        private void SaveGrant(ProjectTrackerContainer context, GrantAffil grant)
        {
            var result = context.Entry(grant).GetValidationResult();
            if (!result.IsValid)
            {
                lblMsg.Text = result.ValidationErrors.First().ErrorMessage;
            }
            else
            {
                context.SaveChanges();
                lblMsg.Text = "Saved successfully.";
            }
        }

        protected void GridViewGrant_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewGrant.EditIndex = e.NewEditIndex;
            BindControl();
        }

        protected void GridViewGrant_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewGrant.EditIndex = -1;
            BindControl();
        }

        protected void GridViewGrant_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewGrant.Rows[e.RowIndex];

            Label lblId = row.FindControl("lblId") as Label;
            TextBox txtName = row.FindControl("txtName") as TextBox;

            if (txtName != null)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    int grantId = this.ParseStringToInt(lblId.Text); //Convert.ToInt32(lblId.Text);
                    GrantAffil grant = context.GrantAffils.First(x => x.Id == grantId);
                    grant.GrantAffilName = txtName.Text;

                    SaveGrant(context, grant);

                    GridViewGrant.EditIndex = -1;
                    BindControl();

                }
            }
        }
        protected void GridViewGrant_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridViewGrant_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewGrant.Rows[e.RowIndex];
            Label lblId = row.FindControl("lblId") as Label;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                int grantId = this.ParseStringToInt(lblId.Text); //Convert.ToInt32(lblId.Text);
                GrantAffil grant = context.GrantAffils.First(x => x.Id == grantId);

                context.GrantAffils.Remove(grant);
                SaveGrant(context, grant);

                BindControl();

                lblMsg.Text = "Deleted successfully.";
            }
        }

        string ValidateResult()
        {
            System.Text.StringBuilder validateResult = new System.Text.StringBuilder();

            if (ddlInvestor.SelectedValue.Equals(string.Empty))
            {
                validateResult.Append("PI is required. \\n");
            }

            if (TextBoxTitle.Text.Equals(string.Empty))
            {
                validateResult.Append("Project title is required. \\n");
            }

            DateTime dateValue;
            if (!DateTime.TryParse(TextBoxInitialDate.Text, out dateValue))
            {
                validateResult.Append("Initial date is required. \\n");
            }

            //if (!DateTime.TryParse(TextBoxTargetDate.Text, out dateValue))
            //{
            //    validateResult.Append("Target date is required. \\n");
            //}

            int parseInt;
            if (!Int32.TryParse(ddlBiostat.SelectedValue, out parseInt))
            {
                validateResult.Append("Biostatistician is required. \\n");
            }

            return validateResult.ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int id;
            bool result = Int32.TryParse(ddlInvestor.SelectedValue, out id);

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                //rebind biostat
                DateTime currentDate = DateTime.Parse(HiddenFieldCurrentDate.Value);
                dropDownSource = context.BioStats
                                .Where(b => b.EndDate > currentDate)
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlBiostat, dropDownSource, String.Empty);

                GridViewBioStat.DataSource = dropDownSource;
                GridViewBioStat.DataBind();

                if (result && id > 0)
                {
                    var invest = context.Invests.First(i => i.Id == id);

                    dropDownSource = invest.Projects
                                    .OrderByDescending(d => d.Id)
                                    .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title) })
                                    .ToDictionary(c => c.Id, c => c.FullName);
                }
                else
                {
                    dropDownSource = context.Projects
                                    .OrderByDescending(d => d.Id)
                                    .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title).Substring(0,99) })
                                    .ToDictionary(c => c.Id, c => c.FullName);
                }

                PageUtility.BindDropDownList(ddlProject, dropDownSource, "Add a new project");
            }

            ClearForm();
        }

        //protected void ddlInvestor_Changed(Object sender, EventArgs e)
        //{
        //    if (projectId == null)
        //    {
        //        int id;
        //        bool result = Int32.TryParse(ddlInvestor.SelectedValue, out id);

        //        using (ProjectTrackerContainer context = new ProjectTrackerContainer())
        //        {
        //            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

        //            if (result && id > 0)
        //            {
        //                var invest = context.Invests.First(i => i.Id == id);

        //                dropDownSource = invest.Projects
        //                                .OrderByDescending(d => d.Id)
        //                                .Select(x => new { x.Id, FullName = x.Id + " " + x.Title })
        //                                .ToDictionary(c => c.Id, c => c.FullName);
        //            }
        //            else
        //            {
        //                dropDownSource = context.Projects
        //                                .OrderByDescending(d => d.Id)
        //                                .Select(x => new { x.Id, FullName = x.Id + " " + x.Title })
        //                                .ToDictionary(c => c.Id, c => c.FullName);
        //            }

        //            PageUtility.BindDropDownList(ddlProject, dropDownSource, "Add a new project");
        //        }

        //        ClearForm();
        //    }
        //}

        protected void btnRest_Click(object sender, EventArgs e)
        {
            ddlInvestor.SelectedIndex = 0;
            btnSearch_Click(this, null);
        }       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.User.IsInRole("Admin"))
            {
                int investId;
                Int32.TryParse(ddlInvestor.SelectedValue, out investId);

                TextBox textBoxGrantNew = GridViewGrant.FooterRow.FindControl("txtGrantNameNew") as TextBox;
                string grantOther = textBoxGrantNew.Text;

                if (ValidateResult().Equals(string.Empty) && investId > 0)
                {
                    SaveProject(investId);

                    BindProject(GetProjectId.ToString());
                }
                else
                {
                    Response.Write("<script>alert('" + ValidateResult() + "');</script>");
                }
            }
        }

        private void SaveProject(int investId)
        {
            bool isNewProject;
            int projectId = 0;

            Int32.TryParse(ddlProject.SelectedValue, out projectId);

            Project project;
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                if (projectId == 0) //new project
                {
                    project = new Project();
                    isNewProject = true;
                }
                else
                {
                    project = context.Projects.First(x => x.Id == projectId);
                    isNewProject = false;

                    //allowing biostat add phase
                    UpdatePhase(context, project, gvPhase, Page.User.IsInRole("Admin"));
                    context.SaveChanges();

                    if (!Page.User.IsInRole("Admin") && project.IsApproved == true)
                    {
                        Response.Write("<script>alert('This Project has been approved, please contact with admin group for any changes other than phase.');</script>");
                        return;
                        //Response.Redirect(Request.Url.ToString());
                    }
                }

                SaveProjectDB(context, project, investId, isNewProject);                
            }

            Response.Write("<script>alert('Project has been saved.');</script>");

        }

        private void SaveProjectDB(ProjectTrackerContainer context, Project project, int investId, bool isNewProject)
        {   
            project.InvestId = investId;
            project.Title = TextBoxTitle.Text;
            project.Summary = TextBoxSummary.Text;
            project.InitialDate = Convert.ToDateTime(TextBoxInitialDate.Text);
            project.PilotId = -1;
            project.HiProgramId = -1;  //Convert.ToInt32(ddlHiProgram.SelectedValue);
            project.EthincGroupId = this.ParseStringToInt(ddlEthnicGroup.SelectedValue); //Convert.ToInt32(ddlEthnicGroup.SelectedValue);
            project.IsJabsom = false;
            project.IsInternal = chkInternal.Checked;
            project.IsApproved = chkApproved.Checked;
            project.IsJuniorPI = chkJuniorPI.Checked;

            TextBox textBoxGrantNew = GridViewGrant.FooterRow.FindControl("txtGrantNameNew") as TextBox;
            project.GrantOther = textBoxGrantNew.Text;

            TextBox textBoxStudyOther = GridViewStudyArea.FooterRow.FindControl("txtStudyOther") as TextBox;
            project.StudyOther = textBoxStudyOther.Text;

            project.Comment = TextBoxComment.Text;
            project.Creator = Page.User.Identity.Name;
            project.CreationDate = Convert.ToDateTime(_currentDate);                       

            if (Request.Form["optionsMentors"] != null)
            {
                string hasMentor = Request.Form["optionsMentors"].ToString();
                if (hasMentor == "Yes")
                {
                    project.HasMentor = true;

                    project.MentorFirstName = TextBoxMentorFirst.Text;
                    project.MentorLastName = TextBoxMentorLast.Text;
                    project.MentorEmail = TextBoxMentorEmail.Text;
                }
                else
                {
                    project.HasMentor = false;
                    project.MentorFirstName = string.Empty;
                    project.MentorLastName = string.Empty;
                    project.MentorEmail = string.Empty;
                }
            }

            if (Request.Form["chkRmatrix"] != null && Request.Form["chkRmatrix"] == "on")
            {
                project.IsRmatrix = true;

                int rmatrixNum = 0;
                project.RmatrixNum = Int32.TryParse(TextBoxRmatrixNum.Text, out rmatrixNum) ? rmatrixNum : (Nullable<int>)null;

                DateTime rmatrixDate;
                project.RmatrixSubDate = DateTime.TryParse(TextBoxRmatrixSubDate.Text, out rmatrixDate) ? rmatrixDate : (Nullable<DateTime>)null;
                project.RmatrixRcvDate = DateTime.TryParse(TextBoxRmatrixRcvDate.Text, out rmatrixDate) ? rmatrixDate : (Nullable<DateTime>)null;
            }
            else
            {
                project.IsRmatrix = false;
                project.RmatrixNum = null;
                project.RmatrixSubDate = null;
                project.RmatrixRcvDate = null;
            }

            if (Request.Form["chkNotReportRmatrix"] != null && Request.Form["chkNotReportRmatrix"] == "on")
            {
                project.NotReportRmatrix = true;
            }
            else
            {
                project.NotReportRmatrix = false;
            }

            DateTime parseDate;

            project.DeadLine = DateTime.TryParse(TextBoxTargetDate.Text, out parseDate) ? parseDate : (Nullable<DateTime>)null;

            project.AnslysisCompletionDate = DateTime.TryParse(TextBoxAnalysisCompletionDate.Text, out parseDate) ? parseDate : (Nullable<DateTime>)null;
            
            project.ProjectCompletionDate = DateTime.TryParse(TextBoxProjectCompletionDate.Text, out parseDate) ? parseDate : (Nullable<DateTime>)null;           

            if (isNewProject)
            {
                context.Projects.Add(project);
                SaveProject(context, project);

                var invest = context.Invests.First(i => i.Id == project.InvestId);
                invest.Projects.Add(project);
                context.SaveChanges();                

                //ddlProject.Items.Insert(ddlProject.Items.Count, new ListItem(project.Title, project.Id.ToString()));
                ddlProject.Items.Add(new ListItem(project.Id + " " + project.Title, project.Id.ToString()));
                ddlProject.SelectedValue = project.Id.ToString();

                SendNotificationEmail(project.Id);
            }

            if (project.Id > 0)
            {
                lblProjectId.Text = project.Id.ToString();                               
                
                //Grant
                if (isNewProject)
                {
                    project.ProjectGrants = GetProjectGrantList(project.Id, false);
                }
                else
                {
                    UpdateProjectGrant(context, project, false);
                }

                //pilot grant
                if (Request.Form["optionsPilot"] != null)
                {
                    string isPilot = Request.Form["optionsPilot"].ToString();
                    if (isPilot == "Yes")
                    {
                        project.PilotId = 1;

                        if (isNewProject)
                        {
                            //project.ProjectGrants = GetProjectGrantList(project.Id, true);
                            foreach (ProjectGrant pg in GetProjectGrantList(project.Id, true))
                            {
                                project.ProjectGrants.Add(pg);
                            }
                        }
                        else
                        {
                            UpdateProjectGrant(context, project, true);
                        }
                    }
                    else
                    {
                        project.PilotId = -1;
                        PageUtility.UncheckGrid(GridViewPilot);
                        UpdateProjectGrant(context, project, true);
                    }
                }

                //project or consult
                project.IsProject = true;
                //if (Request.Form["optionsRadios"] != null)
                //{
                //    string selectedServiceType = Request.Form["optionsRadios"].ToString();

                //    if (selectedServiceType == "option1")
                //    {
                //        project.IsProject = false;
                //    }
                //}

                //Biostat
                UpdateProjectBioStats(project);

                //Service Type
                UpdateProjectServiceTypes(project);

                //Study Area and Health Data
                UpdateStudyAreas(project);

                //Service Detail
                project.ProjectServiceDetails = GetProjectServiceDetails(context, project.Id);
                                                   
                //study type
                UpdateStudyList(project, GridViewStudyType, "StudyType");

                //study population
                UpdateStudyList(project, GridViewStudyPop, "StudyPopulation");

                ////phase
                //UpdatePhase(context, project, gvPhase, Page.User.IsInRole("Admin"));
                               
                context.SaveChanges();
            }
        }       

        private void SendNotificationEmail(int projectId)
        {
            string email = System.Configuration.ConfigurationManager.AppSettings["trackingEmail"];

            MailAddress destination = new MailAddress(email);

            string subject = String.Format("A new project is pending approval, id {0}", projectId);

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (url.IndexOf("?Id") > 0)
            {
                url = url.Substring(0, url.IndexOf("?Id"));
            }

            StringBuilder body = new StringBuilder();
            body.AppendFormat("<p>Request GUID {0}<br /><br />", Guid.NewGuid());
            body.AppendFormat("Please approve new project created by {0} at {1}", User.Identity.Name, url);
            body.AppendFormat("?Id={0}</p>", projectId);
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

        private void UpdateProjectServiceTypes(Project project)
        {
            ICollection<ProjectServiceType> prevServiceTypes = project.ProjectServiceTypes
                                                               .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

            List<int> prevServiceTypeList = new List<int>();
            List<int> newServiceTypeList = new List<int>();

            if (prevServiceTypes.Count > 0)
            {
                foreach (ProjectServiceType serviceType in prevServiceTypes)
                {
                    prevServiceTypeList.Add(serviceType.ServiceTypes_Id);
                }
            }

            newServiceTypeList = PageUtility.GetSelectedRow_GridView(GridViewServiceType);

            var newNotPrevServiceTypeList = newServiceTypeList.Except(prevServiceTypeList).ToList();
            var prevNotNewServiceTypeList = prevServiceTypeList.Except(newServiceTypeList).ToList();

            foreach (var expiredId in prevNotNewServiceTypeList)
            {
                project.ProjectServiceTypes.First(b => b.ServiceTypes_Id == expiredId && b.EndDate > DateTime.Parse(_currentDate)).EndDate = DateTime.Parse(_currentDate);
            }
            foreach (var newId in newNotPrevServiceTypeList)
            {
                project.ProjectServiceTypes.Add(new ProjectServiceType()
                {
                    Projects_Id = project.Id,
                    ServiceTypes_Id = newId,
                    StartDate = DateTime.Parse(_currentDate),
                    EndDate = _endDate
                });
            }
        }

        private void UpdateProjectBioStats(Project project)
        {
            ICollection<ProjectBioStat> prevBiostats = project.ProjectBioStats
                                                       .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

            int newLeadId = this.ParseStringToInt(ddlBiostat.SelectedValue); //Convert.ToInt32(ddlBiostat.SelectedValue);
            List<int> newMemberList = PageUtility.GetSelectedRow_GridView(GridViewBioStat);

            int prevLeadId = 0;
            List<int> prevMemberList = new List<int>();

            ProjectBioStat newLead = new ProjectBioStat()
            {
                Projects_Id = project.Id,
                BioStats_Id = newLeadId,
                StartDate = DateTime.Parse(_currentDate),
                EndDate = _endDate,
                isLead = true
            };

            if (prevBiostats.Count > 0)
            {
                foreach (ProjectBioStat biostat in prevBiostats)
                {
                    if (biostat.isLead == true)
                    {
                        prevLeadId = biostat.BioStats_Id;
                    }
                    else
                    {
                        prevMemberList.Add(biostat.BioStats_Id);
                    }
                }
            }

            if (prevLeadId != newLeadId)
            {
                if (prevLeadId > 0)
                {
                    //expire previous lead biostat
                    project.ProjectBioStats.First(b => b.BioStats_Id == prevLeadId).EndDate = DateTime.Parse(_currentDate);
                }

                //add new lead biostat
                project.ProjectBioStats.Add(newLead);
            }

            var newNotPrevList = newMemberList.Except(prevMemberList).ToList();
            var prevNotNewList = prevMemberList.Except(newMemberList).ToList();

            foreach (var expiredId in prevNotNewList)
            {
                project.ProjectBioStats.First(b => b.BioStats_Id == expiredId).EndDate = DateTime.Parse(_currentDate);
            }
            foreach (var newMemberId in newNotPrevList)
            {
                if (newMemberId != newLeadId)
                {
                    project.ProjectBioStats.Add(new ProjectBioStat()
                    {
                        Projects_Id = project.Id,
                        BioStats_Id = newMemberId,
                        StartDate = DateTime.Parse(_currentDate),
                        EndDate = _endDate,
                        isLead = false
                    });
                }
            }
        }

        private ICollection<ProjectServiceDetail> GetProjectServiceDetails(ProjectTrackerContainer dbContext, int projectId)
        {
            ICollection<ProjectServiceDetail> projectServiceDetailList = new Collection<ProjectServiceDetail>();
            foreach (GridViewRow row in GridViewServiceDesc.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lblId = row.FindControl("lblId") as Label;

                    if (lblId != null)
                    {
                        int id = ParseStringToInt(lblId.Text);

                        ProjectServiceDetail psd = dbContext.ProjectServiceDetails.FirstOrDefault(s => s.Id == id);

                        if (psd != null)
                        {
                            //psd.ProjectId = projectId;
                            projectServiceDetailList.Add(psd);
                        }
                    }
                }
            }

            return projectServiceDetailList;
        }       

        //private ICollection<ProjectStudy> GetStudies(int projectId, GridView gv)
        //{
        //    ICollection<ProjectStudy> projectStudyList = new Collection<ProjectStudy>();
        //    foreach (GridViewRow row in gv.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
        //            if (chkRow.Checked)
        //            {
        //                Label lblId = row.FindControl("lblId") as Label;
        //                int studyId = this.ParseStringToInt(lblId.Text); //Convert.ToInt32(lblId.Text);

        //                ProjectStudy study = new ProjectStudy()
        //                {
        //                    ProjectId = projectId,
        //                    StudyId = studyId,
        //                    StartDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
        //                    EndDate = _endDate
        //                };

        //                projectStudyList.Add(study);
        //            }
        //        }
        //    }
            
        //    return projectStudyList;
        //}

        //private ICollection<ProjectServiceType> GetProjectServiceTypes(int projectId)
        //{
        //    ICollection<ProjectServiceType> projectServiceTypeList = new Collection<ProjectServiceType>();

        //    foreach (GridViewRow row in GridViewServiceType.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
        //            if (chkRow.Checked)
        //            {
        //                Label lblId = row.FindControl("lblId") as Label;
        //                int serviceTypeId = this.ParseStringToInt(lblId.Text);    //Convert.ToInt32(lblId.Text);

        //                ProjectServiceType pst = new ProjectServiceType()
        //                {
        //                    Projects_Id = projectId,
        //                    ServiceTypes_Id = serviceTypeId,
        //                    StartDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
        //                    EndDate = _endDate
        //                };

        //                projectServiceTypeList.Add(pst);
        //            }
        //        }
        //    }
            
        //    return projectServiceTypeList;
        //}

        //private ICollection<ProjectBioStat> GetProjectBioStats(int projectId)
        //{
        //    ICollection<ProjectBioStat> projectBiostatList = new Collection<ProjectBioStat>();

        //    int selectedBio;
        //    bool result = Int32.TryParse(ddlBiostat.SelectedValue, out selectedBio);

        //    if (result && selectedBio > 0)
        //    {
        //        ProjectBioStat projectBiostat = new ProjectBioStat()
        //        {
        //            Projects_Id = projectId,
        //            BioStats_Id = selectedBio,
        //            StartDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
        //            EndDate = _endDate,
        //            isLead = true
        //        };
        //        projectBiostatList.Add(projectBiostat);

        //        // add members
        //        foreach (GridViewRow row in GridViewBioStat.Rows)
        //        {
        //            if (row.RowType == DataControlRowType.DataRow)
        //            {
        //                CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
        //                if (chkRow.Checked)
        //                {
        //                    Label lblId = row.FindControl("lblId") as Label;
        //                    int biostatId = this.ParseStringToInt(lblId.Text);    //Convert.ToInt32(lblId.Text);

        //                    if (biostatId != selectedBio)
        //                    {
        //                        ProjectBioStat projectMember = new ProjectBioStat()
        //                        {
        //                            Projects_Id = projectId,
        //                            BioStats_Id = biostatId,
        //                            StartDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
        //                            EndDate = _endDate,
        //                            isLead = false
        //                        };

        //                        projectBiostatList.Add(projectMember);
        //                    }
        //                }
        //            }
        //        }               
        //    }

        //    return projectBiostatList;
        //}

        private void UpdateStudyAreas(Project project)
        {
            ICollection<ProjectStudy> prevStudyAreas = project.ProjectStudies
                                                       .Where(b => b.EndDate > DateTime.Parse(_currentDate)).ToList();

            List<int> prevStudyAreasList = new List<int>();
            List<int> newStudyAreasList = new List<int>();

            foreach (ProjectStudy studyArea in prevStudyAreas)
            {
                prevStudyAreasList.Add(studyArea.StudyId);
            }

            newStudyAreasList = PageUtility.GetSelectedRow_GridView(GridViewStudyArea);
            //newStudyAreasList.AddRange(PageUtility.GetSelectedRow_GridView(GridViewHealthData));
            foreach (int i in PageUtility.GetSelectedRow_GridView(GridViewHealthData))
            {
                newStudyAreasList.Add(i);
            }

            var newNotPrevStudyList = newStudyAreasList.Except(prevStudyAreasList).ToList();
            var prevNotNewStudyList = prevStudyAreasList.Except(newStudyAreasList).ToList();

            foreach (var expiredId in prevNotNewStudyList)
            {
                project.ProjectStudies.First(b => b.StudyId == expiredId && b.EndDate > DateTime.Parse(_currentDate)).EndDate = DateTime.Parse(_currentDate);
            }
            foreach (var newId in newNotPrevStudyList)
            {
                project.ProjectStudies.Add(new ProjectStudy()
                {
                    ProjectId = project.Id,
                    StudyId = newId,
                    StartDate = DateTime.Parse(_currentDate),
                    EndDate = _endDate
                });
            }

            TextBox textBoxStudyOther = GridViewStudyArea.FooterRow.FindControl("txtStudyOther") as TextBox;
            project.StudyOther = textBoxStudyOther.Text;
        }

        private void UpdateProjectGrant(ProjectTrackerContainer context, Project project, bool isPilot)
        {
            var updatedGrants = new HashSet<int>();
            GridView gridView = isPilot == true ? GridViewPilot : GridViewGrant;

            foreach (GridViewRow row in gridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label lblId = row.FindControl("lblId") as Label;
                        int grantId = this.ParseStringToInt(lblId.Text);    //Convert.ToInt32(lblId.Text);

                        updatedGrants.Add(grantId);
                    }
                }
            }

            var projectGrants = new HashSet<int>(project.ProjectGrants.Where(g => g.IsPilot == isPilot).Select(p => p.GrantAffilId));

            foreach (var grant in context.GrantAffils)
            {
                if (updatedGrants.Contains(grant.Id))
                {
                    if (!projectGrants.Contains(grant.Id))
                    {
                        context.ProjectGrants.Add(new ProjectGrant { GrantAffilId = grant.Id, ProjectId = project.Id, StartDate = DateTime.Now, EndDate = _endDate, IsPilot = isPilot });
                    }
                }
                else
                {
                    if (projectGrants.Contains(grant.Id))
                    {
                        ProjectGrant pg = context.ProjectGrants.Find(project.Id, grant.Id, isPilot);
                        context.ProjectGrants.Remove(pg);
                    }

                }
            }
        }

        private ICollection<ProjectGrant> GetProjectGrantList(int projectId, bool isPilot)
        {
            GridView gridView;
   
            if (isPilot)
            {
                gridView = GridViewPilot;
            }
            else
            {
                gridView = GridViewGrant;
            }

            ICollection<ProjectGrant> projectGrantList = new Collection<ProjectGrant>();
            foreach (GridViewRow row in gridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label lblId = row.FindControl("lblId") as Label;
                        int grantId = this.ParseStringToInt(lblId.Text);  //Convert.ToInt32(lblId.Text);

                        ProjectGrant projectGrant = new ProjectGrant()
                        {
                            ProjectId = projectId,
                            GrantAffilId = grantId,
                            StartDate = DateTime.Now,
                            EndDate = _endDate,
                            IsPilot = isPilot
                        };

                        projectGrantList.Add(projectGrant);

                        //SaveProjectGrant(context, pg);
                    }
                }
            }
            return projectGrantList;
        }
    
        private void SaveProject(ProjectTrackerContainer context, Project project)
        {
            var result = context.Entry(project).GetValidationResult();
            if (!result.IsValid)
            {
                lblMsg.Text = result.ValidationErrors.First().ErrorMessage;
            }
            else
            {
                context.SaveChanges();
                lblMsg.Text = "Saved successfully.";
            }
        }

        private void SaveProjectGrant(ProjectTrackerContainer context, ProjectGrant pg)
        {
            var result = context.Entry(pg).GetValidationResult();
            if (!result.IsValid)
            {
                lblMsg.Text = result.ValidationErrors.First().ErrorMessage;
            }
            else
            {
                context.SaveChanges();
                lblMsg.Text = "Saved successfully.";
            }
        }                
        
        private void SetControlValue(Project project, DateTime currentDate)
        {
            lblProjectId.Text = project.Id.ToString();
            TextBoxTitle.Text = project.Title;
            TextBoxSummary.Text = project.Summary;
            TextBoxInitialDate.Text = project.InitialDate.ToShortDateString();

            if (project.DeadLine != null)
            {
                TextBoxTargetDate.Text = Convert.ToDateTime(project.DeadLine).ToShortDateString();
            }

            if (project.AnslysisCompletionDate != null)
            {
                TextBoxAnalysisCompletionDate.Text = Convert.ToDateTime(project.AnslysisCompletionDate).ToShortDateString();
            }

            if (project.ProjectCompletionDate != null)
            {
                TextBoxProjectCompletionDate.Text = Convert.ToDateTime(project.ProjectCompletionDate).ToShortDateString();
            }

            if (project.Comment != null)
            {
                TextBoxComment.Text = project.Comment;
            }            

            System.Text.StringBuilder script = new System.Text.StringBuilder();
            script.Append("<script>");
            
            //mentor
            if (project.HasMentor)
            {
                script.Append("if (!document.getElementById('optionsMentor1').checked){document.getElementById('optionsMentor1').checked=true;} ");

                TextBoxMentorFirst.Text = project.MentorFirstName;
                TextBoxMentorLast.Text = project.MentorLastName;
                TextBoxMentorEmail.Text = project.MentorEmail;
            }

            //internal study
            if (project.IsInternal)
            {
                chkInternal.Checked = true;
            }
            else
            {
                chkInternal.Checked = false;
            }

            ddlInvestor.SelectedValue = project.InvestId.ToString();
 
            List<int> grants = new List<int>();
            foreach (var projectGrant in project.ProjectGrants.Where(p => p.IsPilot == false))
            {
                grants.Add(projectGrant.GrantAffilId);
            }

            if (grants.Count > 0)
            {
                PageUtility.SelectRow_GridView(GridViewGrant, grants);
            }
            else
            {
                PageUtility.UncheckGrid(GridViewGrant);
            }

            TextBox textBoxGrantNew = GridViewGrant.FooterRow.FindControl("txtGrantNameNew") as TextBox;
            if (project.GrantOther != null)
            {
                textBoxGrantNew.Text = project.GrantOther;
            }

            List<int> pilotGrants = new List<int>();
            foreach (var projectGrant in project.ProjectGrants.Where(p => p.IsPilot == true))
            {
                pilotGrants.Add(projectGrant.GrantAffilId);
            }
           
            if  (project.PilotId == 1)    //(pilotGrants.Count > 0)
            {
                script.Append("if (!document.getElementById('optionsPilot1').checked){document.getElementById('optionsPilot1').checked=true;} ");

                PageUtility.SelectRow_GridView(GridViewPilot, pilotGrants);
            }
            else
            {
                PageUtility.UncheckGrid(GridViewPilot);
            }

            BindBioStats(project, currentDate);

            List<int> serviceTypes = new List<int>();
            foreach (var serviceType in project.ProjectServiceTypes)
            {
                if (serviceType.EndDate > currentDate)
                {
                    serviceTypes.Add(serviceType.ServiceTypes_Id);
                }
            }

            if (!project.IsProject)
            {
                script.Append("if (!document.getElementById('optionsRadios1').checked){document.getElementById('optionsRadios1').checked=true;} ");
            }

            if (project.IsRmatrix)
            {
                script.Append("if (!document.getElementById('chkRmatrix').checked){document.getElementById('chkRmatrix').checked=true;} ");

                if (project.RmatrixNum != null)
                {
                    TextBoxRmatrixNum.Text = project.RmatrixNum.ToString();
                }
                
                if (project.RmatrixSubDate != null)
                {
                    TextBoxRmatrixSubDate.Text = Convert.ToDateTime(project.RmatrixSubDate).ToShortDateString();
                }

                if (project.RmatrixRcvDate != null)
                {
                    TextBoxRmatrixRcvDate.Text = Convert.ToDateTime(project.RmatrixRcvDate).ToShortDateString();
                }
            }

            if (project.NotReportRmatrix)
            {
                script.Append("if (!document.getElementById('chkNotReportRmatrix').checked){document.getElementById('chkNotReportRmatrix').checked=true;} ");
            }
            
            script.Append("</script>");
            ClientScript.RegisterStartupScript(GetType(), "Javascript", script.ToString());

            if (serviceTypes.Count > 0)
            {
                PageUtility.SelectRow_GridView(GridViewServiceType, serviceTypes);
            }
            else
            {
                PageUtility.UncheckGrid(GridViewServiceType);
            }

            //ddlHiProgram.SelectedValue = project.HiProgramId.ToString();

            TextBox textBoxStudyOther = GridViewStudyArea.FooterRow.FindControl("txtStudyOther") as TextBox;
            if (project.StudyOther != null)
            {
                textBoxStudyOther.Text = project.StudyOther;
            }

            BindStudyGrid(project, GridViewStudyArea);

            BindStudyGrid(project, GridViewHealthData);           
            
            //load Study Type
            LoadStudyList(project, GridViewStudyType, "StudyType");

            //load Study Population
            LoadStudyList(project, GridViewStudyPop, "StudyPopulation");
            

            //ddlEthnicGroup.SelectedValue = project.EthincGroupId.ToString();

            chkApproved.Checked = project.IsApproved;
            chkJuniorPI.Checked = project.IsJuniorPI;

            //bind service detail
            BindGridViewServiceDesc(project.Id);
            //BindGridViewInvoice(project.Id);
            BindInvoiceDetails(project.Id, DetailsViewMode.ReadOnly);

            BindgvPhase(project.ProjectPhase, true);
            BindrptPhaseCompletion(project.ProjectPhase, true);
        }

        private void BindrptPhaseCompletion(ICollection<ProjectPhase> collection, bool hasRow)
        {
            DataTable dt = CreatePhaseCompletionTable(collection, hasRow);
            rptPhaseCompletion.DataSource = dt;
            rptPhaseCompletion.DataBind();
        }
        

        private void BindgvPhase(ICollection<ProjectPhase> collection, bool hasRow)
        {
            DataTable dt = CreatePhaseTable(collection, true);
            gvPhase.DataSource = dt;
            gvPhase.DataBind();
        }

        private void BindStudyGrid(Project project, GridView gv)
        {
            if (project != null)
            {
                List<int> studyList = new List<int>();
                var studies = project.ProjectStudies;

                switch (gv.ID)
                {
                    case "GridViewStudyArea":
                        studies = studies.Where(s => s.HiProgram.isStudyArea).ToList();
                        break;
                    case "GridViewHealthData":
                        studies = studies.Where(s => s.HiProgram.isHealthData).ToList();
                        break;
                }

                foreach (var study in studies)
                {
                    if (study.EndDate > DateTime.Now)
                    {
                        studyList.Add(study.StudyId);
                    }
                }

                if (studyList.Count > 0)
                {
                    PageUtility.SelectRow_GridView(gv, studyList);
                }
                else
                {
                    PageUtility.UncheckGrid(gv);
                }
            }
        }

        private void LoadStudyList(Project project, GridView gridView, string PropertyName)
        {
            string study = null;
            TextBox txtStudyOther = new TextBox();

            if (PropertyName.Equals("StudyType"))
            {
                study = project.StudyType;
                txtStudyOther = gridView.FooterRow.FindControl("txtStudyTypeOther") as TextBox;
            }
            else if (PropertyName.Equals("StudyPopulation"))
            {
                study = project.StudyPopulation;
                txtStudyOther = gridView.FooterRow.FindControl("txtStudyPopOther") as TextBox;
            }

            if (!String.IsNullOrEmpty(study))
            {
                List<string> studyList = study.Split('|').ToList();

                var lastItem = studyList[studyList.Count - 1];
                int result;
                if (!Int32.TryParse(lastItem, out result))
                {
                    txtStudyOther.Text = lastItem;
                    studyList.RemoveAt(studyList.Count - 1);
                }

                PageUtility.SelectRow_GridView(gridView, studyList.Select(int.Parse).ToList());
            }

        }

        private void UpdateStudyList(Project project, GridView gridView, string PropertyName)
        {
            string prevStudy = null;
            TextBox txtStudyOther = new TextBox();            

            if (PropertyName.Equals("StudyType"))
            {
                prevStudy = project.StudyType;
                txtStudyOther = gridView.FooterRow.FindControl("txtStudyTypeOther") as TextBox;
            }
            else if (PropertyName.Equals("StudyPopulation"))
            {
                prevStudy = project.StudyPopulation;
                txtStudyOther = gridView.FooterRow.FindControl("txtStudyPopOther") as TextBox;
            }

            //List<string> prevStudyList = new List<string>();
            //prevStudyList = prevStudy.Split('|').ToList();

            List<int> newStudyList = PageUtility.GetSelectedRow_GridView(gridView);

            var newStudyStr = string.Join("|", newStudyList);

            if (newStudyList.Contains(4) && !txtStudyOther.Text.Equals(string.Empty))
            {
                newStudyStr = string.Concat(string.Concat(newStudyStr, "|"), txtStudyOther.Text);
            }
            
            if (PropertyName.Equals("StudyType"))
            {
                project.StudyType = newStudyStr;
            }
            else if (PropertyName.Equals("StudyPopulation"))
            {
                project.StudyPopulation = newStudyStr;
            }


            //string prevStudyType = project.StudyType;
            //List<string> prevStudyTypeList = prevStudyType.Split('|').ToList();

            //List<int> newStudyTypeList = new List<int>();

            //newStudyTypeList = PageUtility.GetSelectedRow_GridView(GridViewStudyType);

            //var newStudyTypeStr = string.Join("|", newStudyTypeList);

            //TextBox txtStudyTypeOther = GridViewStudyType.FooterRow.FindControl("txtStudyTypeOther") as TextBox;
            //if (!txtStudyTypeOther.Text.Equals(string.Empty))
            //{
            //    newStudyTypeStr = string.Concat(string.Concat(newStudyTypeStr, "|"), txtStudyTypeOther.Text);
            //}

            //project.StudyType = newStudyTypeStr;
        }

        private void UpdatePhase(ProjectTrackerContainer db, Project project, GridView gvPhase, bool isAdmin)
        {
            List<ProjectPhase> lstPhase = GetPhase(project.Id);

            if (lstPhase.Count > 0)
            {
                var lstPhaseInDB = db.ProjectPhase.Where(p => p.ProjectId == project.Id);

                if (isAdmin)
                {
                    foreach (var phaseDB in lstPhaseInDB)
                    {
                        if (!lstPhase.Any(l => l.Id == phaseDB.Id))
                        {
                            db.ProjectPhase.Remove(phaseDB);
                        }
                    }
                }

                foreach (var phase in lstPhase)
                {
                    var phaseInDB = db.ProjectPhase.FirstOrDefault(i => i.Id == phase.Id);

                    if (phaseInDB != null)
                    {
                        if (isAdmin)
                            db.Entry(phaseInDB).CurrentValues.SetValues(phase);
                    }
                    else
                    {
                        db.ProjectPhase.Add(phase);
                    }
                }               

            }
        }

        private List<ProjectPhase> GetPhase(int projectId)
        {
            List<ProjectPhase> phases = new List<ProjectPhase>();

            foreach (GridViewRow row in gvPhase.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lblId = row.FindControl("lblId") as Label;
                    DropDownList ddlPhase = row.FindControl("ddlPhase") as DropDownList;
                    TextBox txtTitle = row.FindControl("txtTitle") as TextBox;
                    TextBox txtMsHrs = row.FindControl("txtMsHrs") as TextBox;
                    TextBox txtPhdHrs = row.FindControl("txtPhdHrs") as TextBox;
                    TextBox txtStartDate = row.FindControl("txtStartDate") as TextBox;

                    int phaseId = -1;
                    int output = 0;
                    decimal dMsOutput = 0.0m, dPhdOutput = 0.0m;
                    DateTime dt;

                    if (ddlPhase != null && txtTitle != null && txtMsHrs != null && txtPhdHrs != null)
                    {
                        Int32.TryParse(ddlPhase.SelectedValue, out phaseId);

                        if (phaseId >= 0)
                        {
                            ProjectPhase pPhase = new ProjectPhase()
                            {
                                Id = int.TryParse(lblId.Text, out output) ? output : -1,
                                ProjectId = projectId,
                                Name = ddlPhase.SelectedItem.Text,
                                Title = txtTitle.Text,
                                MsHrs = decimal.TryParse(txtMsHrs.Text, out dMsOutput) ? dMsOutput : default(decimal?),
                                PhdHrs = decimal.TryParse(txtPhdHrs.Text, out dPhdOutput) ? dPhdOutput : default(decimal?),
                                Creator = Page.User.Identity.Name,
                                CreateDate = DateTime.Now,
                                StartDate = DateTime.TryParse(txtStartDate.Text, out dt) ? dt : (DateTime?)null
                            };

                            phases.Add(pPhase);
                        }
                    }
                }
            }

            return phases;
        }

        private void BindBioStats(Project project, DateTime currentDate)
        {
            List<int> biostats = new List<int>();
            foreach (var projectBiostat in project.ProjectBioStats)
            {
                if (projectBiostat.EndDate > currentDate)
                {
                    if (projectBiostat.isLead == true)
                    {
                        if (ddlBiostat.Items.FindByValue(projectBiostat.BioStats_Id.ToString()) == null)
                        {
                            ddlBiostat.Items.Add(new ListItem(projectBiostat.BioStat.Name, projectBiostat.BioStats_Id.ToString()));
                        }
                        ddlBiostat.SelectedValue = projectBiostat.BioStats_Id.ToString();
                    }
                    else
                    {
                        biostats.Add(projectBiostat.BioStats_Id);
                    }
                }
            }

            if (biostats.Count > 0)
            {
                PageUtility.SelectRow_GridView(GridViewBioStat, biostats);
            }
            else
            {
                PageUtility.UncheckGrid(GridViewBioStat);
            }
        }

        protected string GetChecked(object isDefault)
        {
            return (bool)isDefault ? "checked='checked'" : string.Empty;
        }

        //new request, add service description to project
        protected void GridViewServiceDesc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "InsertNew")
            {         
                GridViewRow row = GridViewServiceDesc.FooterRow;

                WriteServiceDetail(row, false);
            }

            if (e.CommandName == "CancelNew")
            {
                GridViewServiceDesc.EditIndex = -1;

                BindGridViewServiceDesc(GetProjectId);
            }
        }

        private void WriteServiceDetail(GridViewRow row, bool isUpdate)
        {
            TextBox txtServiceDesc, txtStartDate, txtEndDate;

            if (isUpdate)
            {
                txtServiceDesc = row.FindControl("txtServiceDesc") as TextBox;
                txtStartDate = row.FindControl("txtStartDate") as TextBox;
                txtEndDate = row.FindControl("txtEndDate") as TextBox;
            }
            else
            {
                txtServiceDesc = row.FindControl("txtServiceDescNew") as TextBox;
                txtStartDate = row.FindControl("txtStartDateNew") as TextBox;
                txtEndDate = row.FindControl("txtEndDateNew") as TextBox;
            }

            if (txtServiceDesc != null && txtStartDate != null && txtEndDate != null)
            {
                try
                {
                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                        //date validate
                        DateTime startDate, endDate;
                       
                        if (DateTime.TryParse(txtStartDate.Text, out startDate) && DateTime.TryParse(txtEndDate.Text, out endDate))
                        {
                            ProjectServiceDetail projectServiceDetail;
                            //ServiceDetail serviceDetail;

                            if (isUpdate)
                            {
                                Label lblId = row.FindControl("lblId") as Label;
                                int projectServiceDetailId = this.ParseStringToInt(lblId.Text); //Convert.ToInt32(lblId.Text);

                                projectServiceDetail = db.ProjectServiceDetails.FirstOrDefault(p => p.Id == projectServiceDetailId);
                              
                                if (projectServiceDetail.ServiceDetail != null)
                                {
                                    //projectServiceDetail.ServiceDetail.ServiceDesc = txtServiceDesc.Text;
                                    //projectServiceDetail.ServiceDetail.StartDate = startDate;
                                    //projectServiceDetail.ServiceDetail.EndDate = endDate;
                                    projectServiceDetail.ServiceDetail = txtServiceDesc.Text;
                                    projectServiceDetail.StartDate = startDate;
                                    projectServiceDetail.EndDate = endDate;
                                }

                                db.SaveChanges();
                                GridViewServiceDesc.EditIndex = -1;
                            }
                            else
                            {
                                //serviceDetail = new ServiceDetail()
                                //{
                                //    ServiceDesc = txtServiceDesc.Text,
                                //    StartDate = startDate,
                                //    EndDate = endDate
                                //};

                                //db.ServiceDetails.Add(serviceDetail);
                                //db.SaveChanges();

                                //if (serviceDetail.Id > 0)
                                //{
                                //    Project project = db.Projects.FirstOrDefault(p => p.Id == projectId);

                                //    if (project != null)
                                //    {
                                //        DateTime currentDate = DateTime.Parse(_currentDate);
                                //        ProjectServiceDetail projectServiceDetailNew
                                //            = project.ProjectServiceDetails
                                //              .FirstOrDefault(s => s.ServiceDetail.ServiceDesc == serviceDetail.ServiceDesc && s.ServiceDetail.StartDate == serviceDetail.StartDate);

                                //        if (projectServiceDetailNew == null)
                                //        {
                                //            projectServiceDetailNew = new ProjectServiceDetail()
                                //            {
                                //                ProjectId = projectId,
                                //                ServiceDetailId = serviceDetail.Id,
                                //                StartDate = currentDate,
                                //                EndDate = DateTime.Parse("2099-01-01"),
                                //                Creator = Page.User.Identity.Name
                                //            };

                                //            project.ProjectServiceDetails.Add(projectServiceDetailNew);

                                //            db.SaveChanges();
                                //        }
                                //    }
                                //}

                                projectServiceDetail = new ProjectServiceDetail()
                                {
                                    ProjectId = GetProjectId,
                                    ServiceDetail = txtServiceDesc.Text,
                                    StartDate = startDate,
                                    EndDate = endDate,
                                    Creator = Page.User.Identity.Name
                                };

                                db.ProjectServiceDetails.Add(projectServiceDetail);

                                db.SaveChanges();

                            }

                            BindGridViewServiceDesc(GetProjectId);
                            //BindGridViewInvoice(projectId);
                        }                        
                        else
                        {
                            Response.Write("<script>alert('" + "Please make sure date is in mm/dd/yyyy format!" + "');</script>");
                        }

                    }
                }
                catch (System.Data.Entity.Core.EntityException ex)
                {
                    throw ex;
                }
            }
        }

        protected void GridViewServiceDesc_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewServiceDesc.EditIndex = e.NewEditIndex;
           
            BindGridViewServiceDesc(GetProjectId);
        }  

        protected void GridViewServiceDesc_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridViewServiceDesc.Rows[e.RowIndex];

            WriteServiceDetail(row, true);
        }

        protected void GridViewServiceDesc_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GridViewServiceDesc.Rows[e.RowIndex];

            Label lblId = row.FindControl("lblId") as Label;

            if (lblId != null)
            {
                int projectId, id;
                projectId = GetProjectId;

                Int32.TryParse(lblId.Text, out id);
                if (id > 0)
                {
                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                        Project project = db.Projects.First(x => x.Id == projectId);
                        if (!Page.User.IsInRole("Admin") && project.IsApproved == true)
                        {
                            Response.Write("<script>alert('This Project has been approved, please contact with admin group for any changes.');</script>");
                            return;
                            //Response.Redirect(Request.Url.ToString());
                        }
                        else
                        {
                            ProjectServiceDetail psd = db.ProjectServiceDetails.FirstOrDefault(p => p.Id == id);

                            if (psd != null)
                            {
                                psd.Deleted = true;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                
                BindGridViewServiceDesc(projectId);
            }
        }

        protected void GridViewServiceDesc_RowDataBound(object sender, GridViewRowEventArgs e)
        {           
        }      

        private void BindGridViewServiceDesc(int projectId)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                DateTime currentDate = DateTime.Parse(_currentDate);

                //var query = from p in db.ProjectServiceDetails
                //            join s in db.ServiceDetails on p.ServiceDetailId equals s.Id
                //            where p.ProjectId == projectId && p.EndDate > currentDate
                //            select new { p.Id, s.ServiceDesc, s.StartDate, s.EndDate };
                var query = db.ProjectServiceDetails
                            .Where(p => p.ProjectId == projectId && !p.Deleted)
                            .Select(x => new { x.Id, x.ServiceDetail, x.StartDate, x.EndDate });

                if (query.Count() > 0)
                {
                    GridViewServiceDesc.DataSource = query.ToList();
                    GridViewServiceDesc.DataBind();
                }
                else
                {                
                    PageUtility.BindEmptyGridView(GridViewServiceDesc, typeof(ProjectServiceDetail));
                }
            }

        }

        protected void btnAddInvoice_Click(object sender, EventArgs e)
        {
            dvInvoice.Visible = true;
            dvInvoice.ChangeMode(DetailsViewMode.Insert);

            BindGridViewServiceDesc(GetProjectId);
        }  
        
        protected void dvInvoice_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            int projectId = 0,
                ProjectInvoiceId = 0;

            Int32.TryParse(lblProjectId.Text, out projectId);
            Int32.TryParse(((Label)dvInvoice.FindControl("lblProjectInvoiceID")).Text, out ProjectInvoiceId);

            if (projectId > 0 && ProjectInvoiceId > 0)
            {
                try
                {
                    SaveInvoice(projectId, ProjectInvoiceId, false);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
            }
        }

        protected void dvInvoice_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            int projectId = 0;
            Int32.TryParse(lblProjectId.Text, out projectId);

            if (projectId > 0)
            {
                try
                {
                    SaveInvoice(projectId, 0, true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
            }
        }

        private void SaveInvoice(int projectId, int projectInvoiceId, bool isNewInvoice)
        {
            string txtRequestNumber, txtRequestDate, txtRequestedPhd, txtRequestedMS;
            string txtApprovedPhd, txtApprovedMS, txtInvoicedPhd;
            string txtInvoicedMS, txtInvoiceDate, txtInvoiceNumber;
            string txtInvoiceFromDate, txtInvoiceToDate;
            string txtRequestApproveDate, txtClientApproveDate;
            string txtComment;
 
            if (isNewInvoice)
            {
                txtRequestNumber = ((TextBox)dvInvoice.FindControl("txtRequestNumberNew")).Text;
                txtRequestDate = ((TextBox)dvInvoice.FindControl("txtRequestDateNew")).Text;
                txtRequestedPhd = ((TextBox)dvInvoice.FindControl("txtRequestedPhdNew")).Text;
                txtRequestedMS = ((TextBox)dvInvoice.FindControl("txtRequestedMSNew")).Text;
                txtApprovedPhd = ((TextBox)dvInvoice.FindControl("txtApprovedPhdNew")).Text;
                txtApprovedMS = ((TextBox)dvInvoice.FindControl("txtApprovedMSNew")).Text;
                txtInvoicedPhd = ((TextBox)dvInvoice.FindControl("txtInvoicedPhdNew")).Text;
                txtInvoicedMS = ((TextBox)dvInvoice.FindControl("txtInvoicedMSNew")).Text;
                txtInvoiceDate = ((TextBox)dvInvoice.FindControl("txtInvoiceDateNew")).Text;
                txtInvoiceNumber = ((TextBox)dvInvoice.FindControl("txtInvoiceNumberNew")).Text;
                txtInvoiceFromDate = ((TextBox)dvInvoice.FindControl("txtInvoiceFromDateNew")).Text;
                txtInvoiceToDate = ((TextBox)dvInvoice.FindControl("txtInvoiceToDateNew")).Text;

                txtRequestApproveDate = ((TextBox)dvInvoice.FindControl("txtRequestApproveDateNew")).Text;
                txtClientApproveDate = ((TextBox)dvInvoice.FindControl("txtClientApproveDateNew")).Text;
                txtComment = ((TextBox)dvInvoice.FindControl("txtCommentNew")).Text;
            }
            else
            {
                txtRequestNumber = ((TextBox)dvInvoice.FindControl("txtRequestNumber")).Text;
                txtRequestDate = ((TextBox)dvInvoice.FindControl("txtRequestDate")).Text;
                txtRequestedPhd = ((TextBox)dvInvoice.FindControl("txtRequestedPhd")).Text;
                txtRequestedMS = ((TextBox)dvInvoice.FindControl("txtRequestedMS")).Text;
                txtApprovedPhd = ((TextBox)dvInvoice.FindControl("txtApprovedPhd")).Text;
                txtApprovedMS = ((TextBox)dvInvoice.FindControl("txtApprovedMS")).Text;
                txtInvoicedPhd = ((TextBox)dvInvoice.FindControl("txtInvoicedPhd")).Text;
                txtInvoicedMS = ((TextBox)dvInvoice.FindControl("txtInvoicedMS")).Text;
                txtInvoiceDate = ((TextBox)dvInvoice.FindControl("txtInvoiceDate")).Text;
                txtInvoiceNumber = ((TextBox)dvInvoice.FindControl("txtInvoiceNumber")).Text;
                txtInvoiceFromDate = ((TextBox)dvInvoice.FindControl("txtInvoiceFromDate")).Text;
                txtInvoiceToDate = ((TextBox)dvInvoice.FindControl("txtInvoiceToDate")).Text;

                txtRequestApproveDate = ((TextBox)dvInvoice.FindControl("txtRequestApproveDate")).Text;
                txtClientApproveDate = ((TextBox)dvInvoice.FindControl("txtClientApproveDate")).Text;
                txtComment = ((TextBox)dvInvoice.FindControl("txtComment")).Text;
            }

            decimal requestedPhd = 0.0M,
                    requestedMS = 0.0M,
                    approvedPhd = 0.0M,
                    approvedMS = 0.0M,
                    invoicedPhd = 0.0M,
                    invoicedMS = 0.0M;
            DateTime requestDate, invoiceDate, invoiceFromDate, invoiceToDate, requestApprovedDate, clientApprovedDate;
            bool validInvoice = false;

            decimal.TryParse(txtApprovedPhd, out approvedPhd);
            decimal.TryParse(txtApprovedMS, out approvedMS);
            decimal.TryParse(txtInvoicedPhd, out invoicedPhd);
            decimal.TryParse(txtInvoicedMS, out invoicedMS);
            DateTime.TryParse(txtInvoiceDate, out invoiceDate);

            DateTime.TryParse(txtRequestApproveDate, out requestApprovedDate);
            DateTime.TryParse(txtClientApproveDate, out clientApprovedDate);            

            StringBuilder validateInvoice = new StringBuilder();
            if (txtRequestNumber.Equals(string.Empty))
            {
                validateInvoice.Append("Requested number is not valid. \\n");
            }
            if (!decimal.TryParse(txtRequestedPhd, out requestedPhd))
            {
                validateInvoice.Append("Requested Phd time is not valid. \\n");
            }
            if (!decimal.TryParse(txtRequestedMS, out requestedMS))
            {
                validateInvoice.Append("Requested MS time is not valid. \\n");
            }            
            if (!DateTime.TryParse(txtRequestDate, out requestDate))
            {
                validateInvoice.Append("Request date is not valid. \\n");
            }      

            //if (validateInvoice.ToString().Equals(string.Empty))
            //{
            //    validInvoice = true;
            //}
            validInvoice = true;

            ViewState["RequestNumber"] = txtRequestNumber;
            ViewState["RequestDate"] = txtRequestDate;
            ViewState["RequestedPhd"] = txtRequestedPhd;
            ViewState["RequestedMs"] = txtRequestedMS;
            ViewState["ApproveDate"] = txtRequestApproveDate;
            ViewState["ApprovedPhd"] = txtApprovedPhd;
            ViewState["ApprovedMs"] = txtApprovedMS;
            ViewState["ClientApproveDate"] = txtClientApproveDate;
            ViewState["InvoiceFromDate"] = txtInvoiceFromDate;
            ViewState["InvoiceToDate"] = txtInvoiceToDate;
            ViewState["InvoicedPhd"] = txtInvoicedPhd;
            ViewState["InvoicedMs"] = txtInvoicedMS;
            ViewState["InvoiceDate"] = txtInvoiceDate;
            ViewState["InvoiceNumber"] = txtInvoiceNumber;
            ViewState["Comment"] = txtComment;

            if (validInvoice)
            {
                decimal workedPhd = 0.0M,
                        workedMS = 0.0M;

                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                    ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));

                    //phdHours.Value = 1.0M;
                    //msHours.Value = 1.0M;

                    if (DateTime.TryParse(txtInvoiceFromDate, out invoiceFromDate) &&
                        DateTime.TryParse(txtInvoiceToDate, out invoiceToDate))
                    {
                        var i = context.P_PROJECT_HOURS(invoiceFromDate, invoiceToDate, projectId, phdHours, msHours);
                        context.SaveChanges();

                        Decimal.TryParse(phdHours.Value.ToString(), out workedPhd);
                        Decimal.TryParse(msHours.Value.ToString(), out workedMS);
                    }

                    Invoice invoice;
                    if (isNewInvoice)
                    {
                        invoice = new Invoice();                        
                    }
                    else
                    {
                        invoice = context.Invoices.FirstOrDefault(p => p.Id == projectInvoiceId);
                    }

                    if (invoice != null)
                    {
                        invoice.RequestNumber = txtRequestNumber;                        
                        invoice.RequestedPhd = requestedPhd;
                        invoice.RequestedMS = requestedMS;
                        invoice.ApprovedPhd = approvedPhd;
                        invoice.ApprovedMS = approvedMS;
                        invoice.WorkedPhd = workedPhd;
                        invoice.WorkedMS = workedMS;
                        invoice.InvoicedPhd = invoicedPhd;
                        invoice.InvoicedMS = invoicedMS;
                        //InvoiceDate = invoiceDate,
                        invoice.InvoiceNumber = txtInvoiceNumber;
                        //FromDate = invoiceFromDate,
                        //ToDate = invoiceToDate,
                        //RequestApproveDate = requestApprovedDate,
                        //ClientApproveDate = clientApprovedDate,
                        invoice.Comment = txtComment;
                        invoice.Creator = User.Identity.Name;

                        if (DateTime.TryParse(txtInvoiceDate, out invoiceDate))
                        {
                            invoice.InvoiceDate = invoiceDate;
                        }
                        if (DateTime.TryParse(txtInvoiceFromDate, out invoiceFromDate))
                        {
                            invoice.FromDate = invoiceFromDate;
                        }
                        if (DateTime.TryParse(txtInvoiceToDate, out invoiceToDate))
                        {
                            invoice.ToDate = invoiceToDate;
                        }
                        if (DateTime.TryParse(txtRequestApproveDate, out requestApprovedDate))
                        {
                            invoice.RequestApproveDate = requestApprovedDate;
                        }
                        if (DateTime.TryParse(txtClientApproveDate, out clientApprovedDate))
                        {
                            invoice.ClientApproveDate = clientApprovedDate;
                        }
                        if (DateTime.TryParse(txtRequestDate, out requestDate))
                        {
                            invoice.RequestDate = requestDate;
                        }

                        if (isNewInvoice)
                        {
                            invoice.ProjectId = projectId;
                            invoice.CreateDate = DateTime.Now;
                            context.Invoices.Add(invoice);
                        }

                        context.SaveChanges();
                    } 
                }

                ClearViewState();

                BindInvoiceDetails(projectId, DetailsViewMode.ReadOnly);
            }
            else
            {
                Response.Write("<script>alert('" + validateInvoice.ToString() + "');</script>");
            }
        }

        protected void dvInvoice_ModeChanging(Object sender, DetailsViewModeEventArgs e)
        {
            int projectId = 0;
            Int32.TryParse(lblProjectId.Text, out projectId);

            switch (e.NewMode)
            {
                case DetailsViewMode.Edit:
                    //binddata();
                    dvInvoice.ChangeMode(DetailsViewMode.Edit);
                    //dvInvoice.HeaderText = "Edit Row";
                    //Page_Load(sender,e);
                    dvInvoice.AllowPaging = false;
                    BindInvoiceDetails(projectId, e.NewMode);
                    //e.Cancel = true;
                    //e.CancelingEdit = true;
                    break;
                case DetailsViewMode.ReadOnly:
                    dvInvoice.ChangeMode(DetailsViewMode.ReadOnly);
                    //Page_Load(sender, e);
                    dvInvoice.AllowPaging = true;
                    BindInvoiceDetails(projectId, e.NewMode);
                    break;
                case DetailsViewMode.Insert:
                    dvInvoice.ChangeMode(DetailsViewMode.Insert);
                    //Page_Load(sender, e);
                    dvInvoice.AllowPaging = false;
                    break;
            }

            BindGridViewServiceDesc(projectId);
        }        
     
        protected void dvInvoice_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            int projectId = 0;
            Int32.TryParse(lblProjectId.Text, out projectId);

            dvInvoice.PageIndex = e.NewPageIndex;
            BindInvoiceDetails(projectId, DetailsViewMode.ReadOnly);
        }

        protected void dvInvoice_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
        {
            int projectId = 0,
                ProjectInvoiceId = 0;

            Int32.TryParse(lblProjectId.Text, out projectId);
            Int32.TryParse(((Label)dvInvoice.FindControl("lblProjectInvoiceID")).Text, out ProjectInvoiceId);

            if (projectId > 0 && ProjectInvoiceId > 0)
            {
                try
                {
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        Invoice projectInvoice = context.Invoices.FirstOrDefault(p => p.Id == ProjectInvoiceId);

                        if (projectInvoice != null)
                        {
                            projectInvoice.Deleted = true;
                            projectInvoice.Creator = Page.User.Identity.Name;
                        }

                        context.SaveChanges();
                    }

                    BindInvoiceDetails(projectId, DetailsViewMode.ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
            }
        }

        protected void dvInvoice_PreRender(object sender, System.EventArgs e)
        {
            if ((dvInvoice.FooterRow != null))
            {
                int commandRowIndex = dvInvoice.Rows.Count - 1;
                DetailsViewRow commandRow = dvInvoice.Rows[commandRowIndex];
                DataControlFieldCell cell = (DataControlFieldCell)commandRow.Controls[0];
                foreach (Control ctl in cell.Controls)
                {
                    if (ctl is Button)
                    {
                        var btn = (Button)ctl;
                        if (btn.CommandName == "Delete")
                        {
                            //btn.Attributes["onClick"] = "return confirm('Are you sure you want to delete this record?');"; 
                            //btn.Attributes.Add("onclick", "javascript: return confirm('Are you sure you want to delete this record?');" + btn.ClientID + ".disabled=true;" + this.GetPostBackEventReference(btn));
                            btn.Attributes.Add("onclick", "javascript:if (!confirm('Are you sure you want to delete this record?')) return;");
                        }
                        if (btn.CommandName == "New")
                        {
                            btn.Visible = false;
                        }
                    }
                }
            }

            TextBox txtRequestNumberNew = ((TextBox)dvInvoice.FindControl("txtRequestNumberNew"));
            if (txtRequestNumberNew != null)
            {
                txtRequestNumberNew.Text = (ViewState["RequestNumber"] != null) ? ViewState["RequestNumber"].ToString() : hdnInvoiceRequestNumber.Value;
            }
            TextBox txtRequestDateNew = ((TextBox)dvInvoice.FindControl("txtRequestDateNew"));
            if (txtRequestDateNew != null)
            {
                txtRequestDateNew.Text = (ViewState["RequestDate"] != null) ? ViewState["RequestDate"].ToString() : hdnInvoiceRequestDate.Value;
            }
            TextBox txtRequestedPhdNew = ((TextBox)dvInvoice.FindControl("txtRequestedPhdNew"));
            if (txtRequestedPhdNew != null)
            {
                txtRequestedPhdNew.Text = (ViewState["RequestedPhd"] != null) ? ViewState["RequestedPhd"].ToString() : hdnInvoiceRequestdHoursPhd.Value;
            }
            TextBox txtRequestedMSNew = ((TextBox)dvInvoice.FindControl("txtRequestedMSNew"));
            if (txtRequestedMSNew != null)
            {
                txtRequestedMSNew.Text = (ViewState["RequestedMs"] != null) ? ViewState["RequestedMs"].ToString() : hdnInvoiceRequestedHoursMS.Value;
            }
            TextBox txtApprovedPhdNew = ((TextBox)dvInvoice.FindControl("txtApprovedPhdNew"));
            if (txtApprovedPhdNew != null)
            {
                txtApprovedPhdNew.Text = (ViewState["ApprovedPhd"] != null) ? ViewState["ApprovedPhd"].ToString() : hdnInvoiceApprovedHoursPhd.Value;
            }
            TextBox txtApprovedMSNew = ((TextBox)dvInvoice.FindControl("txtApprovedMSNew"));
            if (txtApprovedMSNew != null)
            {
                txtApprovedMSNew.Text = (ViewState["ApprovedMs"] != null) ? ViewState["ApprovedMs"].ToString() : hdnInvoiceApprovedHoursMS.Value;
            }

            TextBox txtApproveDateNew = ((TextBox)dvInvoice.FindControl("txtApproveDateNew"));
            if (txtApproveDateNew != null)
            {
                txtApproveDateNew.Text = (ViewState["ApproveDate"] != null) ? ViewState["ApproveDate"].ToString() : string.Empty;
            }

            TextBox txtClientApproveDateNew = ((TextBox)dvInvoice.FindControl("txtClientApproveDateNew"));
            if (txtClientApproveDateNew != null)
            {
                txtClientApproveDateNew.Text = (ViewState["ClientApproveDate"] != null) ? ViewState["ClientApproveDate"].ToString() : string.Empty;
            }

            TextBox txtInvoiceFromDateNew = ((TextBox)dvInvoice.FindControl("txtInvoiceFromDateNew"));
            if (txtInvoiceFromDateNew != null)
            {
                txtInvoiceFromDateNew.Text = (ViewState["InvoiceFromDate"] != null) ? ViewState["InvoiceFromDate"].ToString() : string.Empty;
            }

            TextBox txtInvoiceToDateNew = ((TextBox)dvInvoice.FindControl("txtInvoiceToDateNew"));
            if (txtInvoiceToDateNew != null)
            {
                txtInvoiceToDateNew.Text = (ViewState["InvoiceToDate"] != null) ? ViewState["InvoiceToDate"].ToString() : string.Empty;
            }

            TextBox txtInvoicedPhdNew = ((TextBox)dvInvoice.FindControl("txtInvoicedPhdNew"));
            if (txtInvoicedPhdNew != null)
            {
                txtInvoicedPhdNew.Text = (ViewState["InvoicedPhd"] != null) ? ViewState["InvoicedPhd"].ToString() : string.Empty;
            }
                        
            TextBox txtInvoicedMsNew = ((TextBox)dvInvoice.FindControl("txtInvoicedMsNew"));
            if (txtInvoicedMsNew != null)
            {
                txtInvoicedMsNew.Text = (ViewState["InvoicedMs"] != null) ? ViewState["InvoicedMs"].ToString() : string.Empty;
            }

            TextBox txtInvoiceDateNew = ((TextBox)dvInvoice.FindControl("txtInvoiceDateNew"));
            if (txtInvoiceDateNew != null)
            {
                txtInvoiceDateNew.Text = (ViewState["InvoiceDate"] != null) ? ViewState["InvoiceDate"].ToString() : string.Empty;
            }

            TextBox txtInvoiceNumberNew = ((TextBox)dvInvoice.FindControl("txtInvoiceNumberNew"));
            if (txtInvoiceNumberNew != null)
            {
                txtInvoiceNumberNew.Text = (ViewState["InvoiceNumber"] != null) ? ViewState["InvoiceNumber"].ToString() : string.Empty;
            }

            TextBox txtCommentNew = ((TextBox)dvInvoice.FindControl("txtCommentNew"));
            if (txtCommentNew != null)
            {
                txtCommentNew.Text = (ViewState["Comment"] != null) ? ViewState["Comment"].ToString() : string.Empty;
            }

        }


        private void BindInvoiceDetails(int projectId, DetailsViewMode detailsViewMode)
        {
            if (projectId <= 0)
            {
                //dvInvoice.DataSource = null;
                //dvInvoice.DataBind();
                dvInvoice.Visible = false;
                btnAddInvoice.Visible = false;
            }
            else
            {
                btnAddInvoice.Visible = true;

                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    DateTime currentDate = DateTime.Parse(_currentDate);

                    var query = db.Invoices.Where(i => i.ProjectId == projectId && i.Deleted == false);                        

                    if (query.Count() > 0)
                    {
                        dvInvoice.Visible = true;
                        dvInvoice.ChangeMode(detailsViewMode);
                        dvInvoice.DataSource = query.ToList();
                        dvInvoice.DataBind();

                        dvInvoice.AllowPaging = true;
                    }
                    else
                    {
                        //var obj = new List<Invoice>();
                        //obj.Add(new Invoice());

                        //dvInvoice.DataSource = obj;
                        //dvInvoice.DataBind();
                        //dvInvoice.ChangeMode(detailsViewMode);
                        dvInvoice.Visible = false;

                    }

                }
            }

        }

        protected void btnAddGrant_Click(object sender, EventArgs e)
        {
            int projectId = 0;
            Int32.TryParse(lblProjectId.Text, out projectId);

            if (projectId > 0)
            {
                Response.Redirect("~/Tracking/GrantForm?ProjectId=" + projectId);
            }
        }

        protected void btnSurvey_Click(object sender, EventArgs e)
        {
            if (Page.User.IsInRole("Admin"))
            {
                int projectId = 0;
                Int32.TryParse(lblProjectId.Text, out projectId);

                DateTime projectCompletionDate;
                if (projectId > 0 && DateTime.TryParse(TextBoxProjectCompletionDate.Text, out projectCompletionDate))
                {
                    SurveyForm sf = GetSurveyByProjectId(projectId);

                    if (sf.Project2 != null)
                    {
                        if (sf.Responded)
                        {
                            Response.Redirect("~/Guest/PISurveyForm?Id=" + sf.Id);
                        }
                        else
                        {
                            lblSurveyMsg.Text = "Survey has been sent to PI already.";
                            divProjectInfo.Visible = false;
                            btnSendSurvey.Visible = false;
                        }
                    }
                    else
                    {
                        string PIName = ddlInvestor.SelectedItem.Text;
                        lblSurveyMsg.Text = String.Format("Survey invitation will be sent to PI {0}."
                            , PIName + "(" + sf.SendTo + ")");

                        lblProjectTitle.Text = TextBoxTitle.Text;
                        lblBiostats.Text = sf.LeadBiostat;
                        lblProjectPeriod.Text = ((DateTime) sf.ProjectInitialDate).ToShortDateString() + " - " +
                                                ((DateTime) sf.ProjectCompletionDate).ToShortDateString();
                        lblServiceHours.Text = sf.PhdHours + " PhD hours; " + sf.MsHours + " MS hours";

                        divProjectInfo.Visible = true;
                        btnSendSurvey.Visible = true;
                    }
                }
                else
                {
                    lblSurveyMsg.Text = "Please check project is completed.";
                    btnSendSurvey.Visible = false;
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#surveyModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    "ShowModalScript", sb.ToString(), false);

                //Response.Redirect("Guest/PISurveyForm");
            }
        }

        //private SurveyForm GetSurvey(int projectId)
        //{
        //   SurveyForm surveyForm = null;
        //    //string surveyId = string.Empty;

        //   using (ProjectTrackerContainer db = new ProjectTrackerContainer())
        //   {
        //       surveyForm = db.SurveyForms.FirstOrDefault(s => s.ProjectId == projectId);
        //   }

        //   return surveyForm;
        //}

        protected void btnSendSurvey_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            int projectId = 0;
            Int32.TryParse(lblProjectId.Text, out projectId);

            if (projectId > 0)
            {
                //create a survey, return a survey form id
                //string surveyFormId = LoadSurvey(projectId);

                SurveyForm surveyForm = GetSurveyByProjectId(projectId);                

                if (surveyForm != null)
                {
                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                        db.SurveyForms.Add(surveyForm);
                        db.SaveChanges();
                    }

                    SendSurveyEmail(surveyForm);
                    sb.Append("alert('Survey is sent.');");
                    sb.Append("$('#surveyModal').modal('hide');");

                    //if (!String.IsNullOrEmpty(surveyForm.Id))
                    //{
                    //    Response.Redirect("~/Guest/PISurvey?SurveyId=" + surveyForm.Id);
                    //}
                }

            }

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "HideModalScript", sb.ToString(), false);
        }

        private void SendSurveyEmail(SurveyForm surveyForm)
        {
            //send email invitation
            EmailService email = new EmailService();

            string subject = "BQHS Follow-up Survey: " + TextBoxTitle.Text; 

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = manager.FindByName(User.Identity.Name);
            string destination = user.Email;
                   
            //surveyLink = "~/Guest/PISurvey?SurveyId=" + surveyForm.Id,
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            //caution!!! make sure this is for production 
            if (System.Configuration.ConfigurationManager.AppSettings["isProduction"].Equals("Y"))
            {
                url = System.Configuration.ConfigurationManager.AppSettings["internet"] + "/ProjectForm";
                destination = surveyForm.SendTo;
            }
           
            string surveyLink = url.Replace("ProjectForm", "Guest/PISurveyForm?Id=" + surveyForm.Id);

            StringBuilder body = new StringBuilder();
            body.AppendFormat("<p>Dear {0},</p>", ddlInvestor.SelectedItem.Text);
            body.AppendLine("<p></P>");
            body.AppendLine(@"<p>Thank you for working with us. The JABSOM Office of Biostatistics and Quantitative Health Sciences (BQHS) strives for excellence in high quality, efficient, and reliable collaborations and services in the quantitative sciences. To further improve our support, we invite you to complete a brief follow-up survey regarding your project listed below. This survey will take less than 5 minutes of your time. Your feedback is valuable to us.</p>");
            body.AppendLine("<p></P>");
            body.AppendFormat("<p style=\"margin-left:.5in\">Project title: {0}<br />", TextBoxTitle.Text);
            body.AppendFormat("Faculty/Staff: {0}<br />", surveyForm.LeadBiostat);
            body.AppendFormat("Project period: {0} - {1}<br />", ((DateTime)surveyForm.ProjectInitialDate).ToShortDateString(), ((DateTime)surveyForm.ProjectCompletionDate).ToShortDateString());
            body.AppendFormat("Service hours: {0} PhD hours; {1} MS hours</p>", surveyForm.PhdHours, surveyForm.MsHours);
            //body.AppendLine();
            body.AppendLine("<p></P>");
            body.AppendLine("<p>Click on the link below to complete the BQHS follow-up survey:</p>");
            body.AppendLine("<p></P>");
            body.AppendFormat("<p style=\"margin-left:.5in\">{0}</p>", surveyLink);
            body.AppendLine("<p></P>");
            //body.AppendLine();
            body.AppendLine("<i>This survey can only be completed once.</i>");
            body.AppendLine("<p></P>");
            body.AppendLine("<p>Aloha,</P>");
            body.AppendLine("<p></P>");
            body.AppendLine(@"<p>Office of Biostatistics & Quantitative Health Sciences (BQHS)<br />
                                University of Hawaii John A. Burns School of Medicine<br />
                                651 Ilalo Street, Biosciences Building, Suite 211<br />
                                Honolulu, HI 96813<br />
                                Phone: (808) 692-1840<br />
                                Fax: (808) 692-1966<br />
                                E-mail: biostat@hawaii.edu</P>");

            IdentityMessage im = new IdentityMessage()
            {
                Subject = subject,
                Body = body.ToString(),
                Destination = destination               
            };
            
            email.Send(im);
        }

        private SurveyForm GetSurveyByProjectId(int projectId)
        {
            SurveyForm surveyForm = null;
            //string surveyId = string.Empty;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var project = db.Projects.FirstOrDefault(p => p.Id == projectId);

                if (project != null)
                {
                    surveyForm = db.SurveyForms.FirstOrDefault(s => s.ProjectId == projectId);

                    if (surveyForm == null)
                    {
                        decimal p = 0.0M, m = 0.0M;

                        ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                        ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));
                        var i = db.P_PROJECT_HOURS(project.InitialDate, project.ProjectCompletionDate, projectId, phdHours, msHours);
                        db.SaveChanges();

                        Decimal.TryParse(phdHours.Value.ToString(), out p);
                        Decimal.TryParse(msHours.Value.ToString(), out m);

                        //var biostats = project.ProjectBioStats.Where(b => b.EndDate > DateTime.Now).ToList();
                        var biostats = db.BioStats
                                        .Join(db.TimeEntries
                                            , b => b.Id
                                            , t => t.BioStatId
                                            , (b, t) => new { b.Name, t })
                                        .Join(db.Date1
                                            , tt => tt.t.DateKey
                                            , d => d.DateKey
                                            , (tt, d) => new { tt.Name, tt.t.ProjectId, d.Date }
                                        )
                                        .Where(a => a.ProjectId == projectId && a.Date >= project.InitialDate && a.Date <= project.ProjectCompletionDate)
                                        .Select(a => a.Name).Distinct();

                        if (biostats != null)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var biostat in biostats)
                            {
                                sb.Append(biostat);
                                sb.Append("; ");
                            }

                            SurveyForm sf = new SurveyForm()
                            {
                                Id = System.Guid.NewGuid().ToString(),
                                SurveyId = 1,
                                ProjectId = projectId,
                                RequestBy = User.Identity.Name,
                                RequestDate = DateTime.Now,
                                PhdHours = p,
                                MsHours = m,
                                ProjectInitialDate = project.InitialDate,
                                ProjectCompletionDate = project.ProjectCompletionDate,
                                LeadBiostat = sb.ToString().Substring(0, sb.Length - 2),
                                ProjectTitle = project.Title,
                                SendTo = project.Invests.Email,
                                Comment = string.Empty
                            };

                            surveyForm = sf;
                        }
                        
                    }
                }
            }

            return surveyForm;
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

        protected void gvPhase_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvPhase.Rows[e.RowIndex];

            BindgvPhase(e.RowIndex);
        }

        protected void btnAddPhase_Click(object sender, EventArgs e)
        {
            BindgvPhase(-1);
        }

        private void BindgvPhase(int rowIndex)
        {          
            DataTable dt = CreatePhaseTable(null, false);
            DataRow dr;

            foreach (GridViewRow row in gvPhase.Rows)
            {
                if (row.RowIndex != rowIndex)
                {
                    dr = dt.NewRow();

                    Label rowId = row.FindControl("lblId") as Label;
                    DropDownList ddlPhase = row.FindControl("ddlPhase") as DropDownList;
                    TextBox txtStartDate = row.FindControl("txtStartDate") as TextBox;
                    TextBox txtTitle = row.FindControl("txtTitle") as TextBox;
                    TextBox txtMsHrs = row.FindControl("txtMsHrs") as TextBox;
                    TextBox txtPhdHrs = row.FindControl("txtPhdHrs") as TextBox;

                    dr[0] = rowId.Text;
                    dr[1] = ddlPhase.SelectedItem.Text;
                    dr[2] = txtTitle.Text;
                    dr[3] = txtMsHrs.Text;
                    dr[4] = txtPhdHrs.Text;
                    dr[5] = txtStartDate.Text; 

                    dt.Rows.Add(dr);
                }
            }

            if (dt.Rows.Count == 0 || rowIndex < 0)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);                
            }

            gvPhase.DataSource = dt;
            gvPhase.DataBind();
        }

        private DataTable CreatePhaseTable(ICollection<ProjectPhase> phases, bool hasRow)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Title");
            dt.Columns.Add("MsHrs");
            dt.Columns.Add("PhdHrs");
            dt.Columns.Add("StartDate");

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

                    dt.Rows.Add(dr);
                }
            }
            else if (hasRow)
            {
                dt.Rows.Add();
            }

            return dt;
        }

        private DataTable CreatePhaseCompletionTable(ICollection<ProjectPhase> phases, bool hasRow)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Phase");
            dt.Columns.Add("MsHrs");
            dt.Columns.Add("PhdHrs");
            dt.Columns.Add("CompletionDate");

            if (phases != null && phases.Count > 0)
            {
                foreach (var phase in phases)
                {
                    //decimal dMsOut = 0.0m,
                    //        dPhdOut = 0.0m;
                    DataRow dr = dt.NewRow();
                    dr[0] = phase.Name;
                    dr[1] = phase.MsHrs;
                    dr[2] = phase.PhdHrs;
                    dr[3] = phase.CompletionDate != null ? Convert.ToDateTime(phase.CompletionDate).ToShortDateString() : "";

                    dt.Rows.Add(dr);
                }
            }
            else if (hasRow)
            {
                dt.Rows.Add();
            }

            return dt;
        }


        //protected void GridViewInvoice_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    GridViewInvoice.EditIndex = e.NewEditIndex;
        //    int projectId = Convert.ToInt32(lblProjectId.Text);
        //    BindGridViewInvoice(projectId);
        //}

        //protected void GridViewInvoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    GridViewInvoice.EditIndex = -1;
        //    int projectId = Convert.ToInt32(lblProjectId.Text);
        //    BindGridViewInvoice(projectId);
        //}

        //protected void GridViewInvoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    GridViewRow row = GridViewInvoice.Rows[e.RowIndex];

        //    WriteInvoice(row, true);
        //}       

        //protected void GridViewInvoice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    GridViewRow row = GridViewInvoice.Rows[e.RowIndex];

        //    Label lblId = row.FindControl("lblProjectInvoiceID") as Label;

        //    if (lblId != null)
        //    {
        //        using (ProjectTrackerContainer db = new ProjectTrackerContainer())
        //        {
        //            int id = Convert.ToInt32(lblId.Text);

        //            ProjectInvoice pInvoice = db.ProjectInvoices.FirstOrDefault(p => p.Id == id);

        //            if (pInvoice != null)
        //            {
        //                pInvoice.EndDate = DateTime.Now;
        //                pInvoice.Creator = Page.User.Identity.Name;
        //            }

        //            db.SaveChanges();
        //        }

        //        BindGridViewInvoice(Convert.ToInt32(lblProjectId.Text));
        //    }
        //}

        //protected void GridViewInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //}

        //protected void GridViewInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "InsertNew")
        //    {
        //        GridViewRow row = GridViewInvoice.FooterRow;

        //        WriteInvoice(row, false);
        //    }
        //}

        //private void WriteInvoice(GridViewRow row, bool p)
        //{

        //}

        //private void BindGridViewInvoice(int projectId)
        //{
        //    if (projectId == -1)
        //    {
        //        GridViewInvoice.DataSource = null;
        //        GridViewInvoice.DataBind();
        //    }
        //    else
        //    {
        //        using (ProjectTrackerContainer db = new ProjectTrackerContainer())
        //        {
        //            DateTime currentDate = DateTime.Parse(_currentDate);

        //            var query = from p in db.ProjectInvoices
        //                        join s in db.Invoices on p.InvoiceId equals s.Id
        //                        where p.ProjectId == projectId && p.EndDate > currentDate
        //                        select new { p.Id, s.RequestNumber, s.RequestDate, s.RequestedPhd, s.RequestedMS, s.ApprovedPhd, s.ApprovedMS, s.WorkedPhd, s.WorkedMS, s.InvoicedPhd, s.InvoicedMS, s.InvoiceDate, s.InvoiceNumber, s.FromDate, s.ToDate };

        //            if (query.Count() > 0)
        //            {
        //                GridViewInvoice.DataSource = query.ToList();
        //                GridViewInvoice.DataBind();
        //            }
        //            else
        //            {
        //                //GridView1BindEmpty(); 
        //                var obj = new List<Invoice>();
        //                obj.Add(new Invoice());
        //                // Bind the DataTable which contain a blank row to the GridView
        //                GridViewInvoice.DataSource = obj;
        //                GridViewInvoice.DataBind();
        //                //int columnsCount = GridView1.Columns.Count;
        //                GridViewInvoice.Rows[0].Cells.Clear();// clear all the cells in the row                        
        //            }
        //        }
        //    }
        //}

        private int ParseStringToInt(String s)
        {
            int _id;
            Int32.TryParse(s, out _id);
            return _id;
        }

        public int GetProjectId
        {
            get
            {
                int _projectId;
                Int32.TryParse(lblProjectId.Text, out _projectId);
                _projectId = _projectId == 0 ? -1 : _projectId;

                return _projectId;
            }
        }  

    }
}