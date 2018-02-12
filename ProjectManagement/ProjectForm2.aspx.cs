//using Biostat.BL;
//using Biostat.Model;
using ProjectManagement.Model;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ProjectManagement
{
    public partial class ProjectForm2 : System.Web.UI.Page
    {
        //IBusinessLayer businessLayer = new BusinessLayer();

        protected void Page_Load(object sender, EventArgs e)
        {
            string projectId = Request.QueryString["Id"];

            if (!Page.IsPostBack)
            {
                BindControl();

                int id = 0;
                Int32.TryParse(projectId, out id);

                if (id > 0)
                    BindProject(id);                    

                if (!Page.User.IsInRole("Admin"))
                {
                    tabAdmin.Style["display"] = "none";
                }
            }    
        }        

        /// <summary>
        /// Saves project into database when user clicks "submit" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit1_Click(object sender, EventArgs e)
        {           
            string str = ValidateResult();

            if (str.Equals(string.Empty))
            {
                Project2 project = GetProject();

                string errorMsg = SaveProject(project);

                if (errorMsg.Equals(string.Empty))
                {
                    Response.Write("<script>alert('Project has been saved.');</script>");

                    //BindProject(project.Id);
                }
                else
                {
                    Response.Write("<script>alert('" + errorMsg + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('" + str + "');</script>");
            }
            
        }

        /// <summary>
        /// Either:
        /// (1) Adds new project in database.
        ///      - Sends an email to QHS tracking list to view current project.
        /// (2) Saves existing project in database.
        /// </summary>
        /// <param name="project">Current project in view.</param>
        /// <returns></returns>
        private string SaveProject(Project2 project)
        {
            string errorMsg = "";
            int id = project.Id;

            List<ProjectPhase> newPhases = GetPhase(project.Id);

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                /// If no existing id, adds project to database and sends email to tracking team for review.
                if (0 == id)
                {
                    //using (var dbTrans = db.Database.BeginTransaction())
                    //{
                        try
                        {
                            db.Project2.Add(project);
                            db.SaveChanges();

                            id = project.Id;
                            foreach (var phase in newPhases)
                            {
                                phase.ProjectId = id;
                                db.ProjectPhase.Add(phase);
                            }
                            db.SaveChanges();

                            //dbTrans.Commit();
                            
                            //BindProject(id);
                            ddlProject.Items.Add(new ListItem(project.Id + " " + project.Title, project.Id.ToString()));
                            ddlProject.SelectedValue = project.Id.ToString();

                            //send email
                            SendNotificationEmail(id);
                        }
                        catch (Exception ex)
                        {
                            //dbTrans.Rollback();
                            return ex.InnerException.Message;
                        }
                    //}
                }
                /// If current user is not admin,
                ///     - prevents user from making changes if admin already approved.
                ///     - otherwise, saves current values of database.
                /// If current user is admin
                ///     - creates new phase information.
                else if (id > 0)
                {
                    bool isAdmin = Page.User.IsInRole("Admin");

                    var prevProject = db.Project2.FirstOrDefault(p => p.Id == id);
                    if (prevProject.IsApproved && !isAdmin)
                    {
                        errorMsg = string.Format("Project {0} can not be updated after approval", id);
                    }
                    else
                    {
                        db.Entry(prevProject).CurrentValues.SetValues(project);
                    }
                   
                    isAdmin = isAdmin || Page.User.IsInRole("Biostat");
                    var prevPhases = db.ProjectPhase.Where(p => p.ProjectId == id);
                    if (isAdmin)
                    {
                        foreach (var phaseDB in prevPhases)
                        {
                            if (!newPhases.Any(l => l.Name == phaseDB.Name))
                            {
                                //db.ProjectPhase.Remove(phaseDB);
                                if (phaseDB.TimeEntries.Count == 0)
                                {
                                    phaseDB.Title = string.Empty;
                                    phaseDB.StartDate = null;
                                    phaseDB.CompletionDate = null;
                                    phaseDB.MsHrs = null;
                                    phaseDB.PhdHrs = null;
                                    phaseDB.IsDeleted = true;
                                    phaseDB.Creator = Page.User.Identity.Name;
                                    phaseDB.CreateDate = DateTime.Now;
                                }
                                else
                                {
                                    errorMsg = string.Format("{0} can not be deleted since logged in time entry", phaseDB.Name);
                                    return errorMsg;
                                }
                            }
                        }
                    }

                    foreach (var phase in newPhases)
                    {
                        var phaseInDB = db.ProjectPhase.FirstOrDefault(i => i.ProjectId == phase.ProjectId && i.Name == phase.Name);

                        if (phaseInDB != null)
                        {
                            if (isAdmin)
                            {
                                if (phaseInDB.StartDate != phase.StartDate
                                    || phaseInDB.CompletionDate != phase.CompletionDate                                
                                    || phaseInDB.Title != phase.Title
                                    || phaseInDB.MsHrs != phase.MsHrs
                                    || phaseInDB.PhdHrs != phase.PhdHrs
                                    || phaseInDB.IsDeleted != phase.IsDeleted
                                    )
                                {
                                    phaseInDB.StartDate = phase.StartDate;
                                    phaseInDB.CompletionDate = phase.CompletionDate  ;                              
                                    phaseInDB.Title = phase.Title;
                                    phaseInDB.MsHrs = phase.MsHrs;
                                    phaseInDB.PhdHrs = phase.PhdHrs;
                                    phaseInDB.IsDeleted = phase.IsDeleted;
                                    phaseInDB.Creator = Page.User.Identity.Name;
                                    phaseInDB.CreateDate = DateTime.Now;
                                }
                                    

                            }
                            else if (project.IsApproved)
                            {
                                errorMsg = "Phase can not be changed, please contact tracking team.";
                                return errorMsg;
                            }
                        }
                        else
                        {
                            db.ProjectPhase.Add(phase);
                            errorMsg = string.Empty;
                        }
                    } 
                                  
                    db.SaveChanges();
                }              
                                
            }

            return errorMsg;
        }

        private string ValidateResult()
        {
            System.Text.StringBuilder validateResult = new System.Text.StringBuilder();

            int piId = 0;
            Int32.TryParse(ddlPI.SelectedValue, out piId);
            if (piId <= 0)
            {
                validateResult.Append("PI is required. \\n");
            }

            if (txtTitle.Value.Equals(string.Empty))
            {
                validateResult.Append("Project title is required. \\n");
            }

            int leadBiostatId = 0;
            Int32.TryParse(ddlLeadBiostat.SelectedValue, out leadBiostatId);
            if (leadBiostatId <= 0)
            {
                validateResult.Append("Lead member is required. \\n");
            }

            //DateTime initialDate;
            //if (!DateTime.TryParse(txtInitialDate.Text, out initialDate))
            //{
            //    validateResult.Append("Initial date is required. \\n");
            //}

            int otherMemberBitSum = 0;
            Int32.TryParse(txtOtherMemberBitSum.Value, out otherMemberBitSum);
            if (otherMemberBitSum <= 0)
            {
                validateResult.Append("Other member is required. \\n");
            }

            int studyAreaBitSum = 0;
            Int32.TryParse(txtStudyAreaBitSum.Value, out studyAreaBitSum);
            if (studyAreaBitSum <= 0)
            {
                validateResult.Append("Study area is required. \\n");
            }

            int healthDataBitSum = 0;
            Int32.TryParse(txtHealthDataBitSum.Value, out healthDataBitSum);
            if (healthDataBitSum <= 0)
            {
                validateResult.Append("Health data is required. \\n");
            }

            int studyTypeBitSum = 0;
            Int32.TryParse(txtStudyTypeBitSum.Value, out studyTypeBitSum);
            if (studyTypeBitSum <= 0)
            {
                validateResult.Append("Study type is required. \\n");
            }

            int studyPopulationBitSum = 0;
            Int32.TryParse(txtStudyPopulationBitSum.Value, out studyPopulationBitSum);
            if (studyPopulationBitSum <= 0)
            {
                validateResult.Append("Study population is required. \\n");
            }

            int serviceBitSum = 0;
            Int32.TryParse(txtServiceBitSum.Value, out serviceBitSum);
            if (serviceBitSum <= 0)
            {
                validateResult.Append("Service is required. \\n");
            }

            return validateResult.ToString();
        }

        private bool IsValidProject(Project2 project)
        {
            bool isValid = true;

            return isValid;
        }
        

        #region Bind UI
        /// <summary>
        /// C
        /// </summary>
        private void BindControl()
        {
            var dropDownSource = new Dictionary<int, string>();

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                /// Populates PI dropdown
                dropDownSource = db.Invests
                                .Where(i => i.Id > 0)
                                .OrderBy(d => d.FirstName).ThenBy(l => l.LastName)
                                .Select(x => new { x.Id, Name = x.FirstName + " " + x.LastName })
                                .ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlPI, dropDownSource, String.Empty);

                /// Populates dropdown of projects.
                dropDownSource = db.Project2
                                .OrderByDescending(d => d.Id)
                                .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title).Substring(0, 99) })
                                .ToDictionary(c => c.Id, c => c.FullName);

                PageUtility.BindDropDownList(ddlProject, dropDownSource, "Add new project");

                /// Populates dropdown of projects based on selected PI.
                dropDownSource = db.Project2
                                .OrderByDescending(d => d.Id)
                                .Select(x => new { x.Id, FullName = x.Id + " " + x.Invests.FirstName + " " + x.Invests.LastName })
                                .ToDictionary(c => c.Id, c => c.FullName);

                PageUtility.BindDropDownList(ddlProjectHdn, dropDownSource, "Add new project");

                //dropDownSource = db.ProjectField
                //                .Where(f => f.IsPhase == true)
                //                .ToDictionary(c => c.Id, c => c.Name);

                //PageUtility.BindDropDownList(ddlPhaseHdn, dropDownSource, "--- Select ---");

                /// Populates Lead member dropdown (current, active QHS faculty/staff and not 'N/A' marker)
                var query = db.BioStats
                            .Where(b => b.EndDate >= DateTime.Now);

                dropDownSource = query
                                .Where(b => b.Id > 0 && b.Name != "N/A")
                                .OrderBy(b => b.Name)
                                .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlLeadBiostat, dropDownSource, String.Empty);

                /// Bind 
                dropDownSource = query
                                .Where(b => b.Id > 0)
                                .OrderBy(b => b.Id == 99 ? 2:1)
                                .ToDictionary(c => (int)c.BitValue, c => c.Name);

                BindTable2(dropDownSource, rptBiostat);                

                var qProjectField = db.ProjectField.Where(f => f.IsStudyArea == true).ToList();

                rptStudyArea.DataSource = qProjectField;
                rptStudyArea.DataBind();

                //dtProjectField.Clear();

                qProjectField = db.ProjectField.Where(f => f.IsHealthData == true).ToList();
                rptHealthData.DataSource = qProjectField;
                rptHealthData.DataBind();

                qProjectField = db.ProjectField.Where(f => f.IsStudyType == true).ToList();
                rptStudyType.DataSource = qProjectField;
                rptStudyType.DataBind();

                qProjectField = db.ProjectField.Where(f => f.IsStudyPopulation == true).ToList();
                rptStudyPopulation.DataSource = qProjectField;
                rptStudyPopulation.DataBind();

                qProjectField = db.ProjectField.Where(f => f.IsService == true).ToList();
                rptService.DataSource = qProjectField;
                rptService.DataBind();

                //BindgvPhase(1);
                BindPhaseByProject(-1);

                //var qPhase = db.ProjectPhase.Where(p => p.ProjectId == 1120).ToList();
                //rptPhase.DataSource = qPhase;
                //rptPhase.DataBind();

                dropDownSource = db.ProjectField
                                .Where(f => f.IsGrant == true)
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.BitValue, c => c.Name);

                BindTable2(dropDownSource, rptGrant);

                if (!Page.User.IsInRole("Admin"))
                {
                    chkApproved.Disabled = true;
                }
                else
                    chkApproved.Disabled = false;
            }
            
        }        

        /// <summary>
        /// Finds the project based on the parameter 'projectId' and
        /// populates the ProjectForm2 field based on the found project.
        /// </summary>
        /// <param name="projectId">Id corresponding to referred project.</param>
        private void BindProject(int projectId)
        {
            //Biostat.Model.Project2 project = businessLayer.GetProjectById(projectId);

            //if (project != null)
            //{
            //    SetProject(project);
            //}

            //var dropDownSource = new Dictionary<int, string>();
            if (projectId > 0)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    Project2 project = db.Project2.FirstOrDefault(x => x.Id == projectId);

                    if (project != null)
                    {                     
                        var query = db.BioStats
                                    .Where(b => b.EndDate >= project.InitialDate);

                        var dropDownSource = query
                                            .Where(b => b.Id > 0 && b.Name != "N/A")
                                            .OrderBy(b => b.Name)
                                            .ToDictionary(c => c.Id, c => c.Name);

                        PageUtility.BindDropDownList(ddlLeadBiostat, dropDownSource, String.Empty);

                        dropDownSource = query
                                        .Where(b => b.Id > 0)
                                        .OrderBy(b => b.Id)
                                        .ToDictionary(c => (int)c.BitValue, c => c.Name);

                        BindTable2(dropDownSource, rptBiostat);   

                        SetProject(project);
                    }
                }
            }
            else
            {
                int piId = 0;
                Int32.TryParse(ddlPI.SelectedValue, out piId);
                SetProject(InitProject(piId));
            }

            //calculate completion phase hours
            BindPhaseByProject(projectId);
        }


        /// <summary>
        /// Populates project field based on which project information 
        /// indicated in parameters. Originally called when projectForm2 Project
        /// dropdown has been changed to select indicated project.
        /// </summary>
        /// <param name="project">Project being referred to.</param>
        private void SetProject(Project2 project)
        {
            lnkPI.CommandArgument = project.PIId.ToString();
            lblPI.Text = project.Invests != null ? project.Invests.FirstName + " " + project.Invests.LastName : string.Empty;            
            lblProjectId.Text = project.Id > 0 ? project.Id.ToString() : string.Empty;

            ddlPI.SelectedValue = project.PIId > 0 ? project.PIId.ToString() : "";
            ddlProject.SelectedValue = project.Id > 0 ? project.Id.ToString() : string.Empty;
            txtTitle.Value = project.Title;
            txtSummary.Value = project.Summary;
            txtInitialDate.Text = project.InitialDate.ToShortDateString();
            txtDeadline.Text = project.DeadLine != null ? Convert.ToDateTime(project.DeadLine).ToShortDateString() : string.Empty;

            ddlLeadBiostat.SelectedValue = project.LeadBiostatId > 0 ? project.LeadBiostatId.ToString() : string.Empty;

            //if (project.OtherMemberBitSum > 0)
            //{
            //    BindTable(rptBiostat, (int)project.OtherMemberBitSum);                    
            //}
            BindTable(rptBiostat, (int)project.OtherMemberBitSum); 

            //if (project.StudyAreaBitSum > 0)
            //{
            //    BindTable(rptStudyArea, (int)project.StudyAreaBitSum);
            //    txtStudyAreaOther.Value = project.StudyAreaOther;
            //}
            BindTable(rptStudyArea, (int)project.StudyAreaBitSum);
            txtStudyAreaOther.Value = project.StudyAreaOther;

            //if (project.HealthDateBitSum > 0)
            //{
            //    BindTable(rptHealthData, (int)project.HealthDateBitSum);
            //    txtHealthDataOther.Value = project.HealthDataOther;
            //}
            BindTable(rptHealthData, (int)project.HealthDateBitSum);
            txtHealthDataOther.Value = project.HealthDataOther;

            //if (project.StudyTypeBitSum > 0)
            //{
            //    BindTable(rptStudyType, (int)project.StudyTypeBitSum);
            //    txtStudyTypeOther.Value = project.StudyTypeOther;
            //}
            BindTable(rptStudyType, (int)project.StudyTypeBitSum);
            txtStudyTypeOther.Value = project.StudyTypeOther;

            //if (project.StudyPopulationBitSum > 0)
            //{
            //    BindTable(rptStudyPopulation, (int)project.StudyPopulationBitSum);
            //    txtStudyPopulationOther.Value = project.StudyPopulationOther;
            //}
            BindTable(rptStudyPopulation, (int)project.StudyPopulationBitSum);
            txtStudyPopulationOther.Value = project.StudyPopulationOther;

            //if (project.ServiceBitSum > 0)
            //{
            //    BindTable(rptService, (int)project.ServiceBitSum);
            //    txtServiceOther.Value = project.ServiceOther;
            //}
            BindTable(rptService, (int)project.ServiceBitSum);
            txtServiceOther.Value = project.ServiceOther;

            //if (project.GrantBitSum > 0)
            //{
            //    BindTable(rptGrant, (int)project.GrantBitSum);
            //    txtGrantOther.Value = project.GrantOther;
            //}
            BindTable(rptGrant, (int)project.GrantBitSum);
            txtGrantOther.Value = project.GrantOther;

            //var projectPhase = project.ProjectPhase;
            //BindPhase(projectPhase);
            //BindrptPhaseCompletion(projectPhase);

            //txtRequestRcvdDate.Text = project.RequestRcvdDate != null ? Convert.ToDateTime(project.RequestRcvdDate).ToShortDateString() : string.Empty;

            if (project.IsJuniorPI.HasValue)
            {
                bool isJunior = (bool)project.IsJuniorPI;
                chkJuniorPIYes.Checked = isJunior;
                chkJuniorPINo.Checked = !isJunior;
            }
            else
            {
                chkJuniorPIYes.Checked = false;
                chkJuniorPINo.Checked = false;
            }

            if (project.HasMentor.HasValue)
            {
                bool hasMentor = (bool)project.HasMentor;
                chkMentorYes.Checked = hasMentor;
                chkMentorNo.Checked = !hasMentor;

                txtMentorFirstName.Value = project.MentorFirstName;
                txtMentorLastName.Value = project.MentorLastName;
                txtMentorEmail.Value = project.MentorEmail;
            }
            else
            {
                chkMentorYes.Checked = false;
                chkMentorNo.Checked = false;
            }

            if (project.IsInternal.HasValue)
            {
                bool isInternal = (bool)project.IsInternal;
                chkInternalYes.Checked = isInternal;
                chkInternalNo.Checked = !isInternal;
            }
            else
            {
                chkInternalYes.Checked = false;
                chkInternalNo.Checked = false;
            }

            if (project.IsPilot.HasValue)
            {
                bool isPilot = (bool)project.IsPilot;
                chkPilotYes.Checked = isPilot;
                chkPilotNo.Checked = !isPilot;
            }
            else
            {
                chkPilotYes.Checked = false;
                chkPilotNo.Checked = false;
            }

            if (project.IsPaid.HasValue)
            {
                bool isPaying = (bool)project.IsPaid;
                chkPayingYes.Checked = isPaying;
                chkPayingNo.Checked = !isPaying;

                txtPayProject.Value = project.TypeOfPayment;
            }
            else
            {
                chkPayingYes.Checked = false;
                chkPayingNo.Checked = false;
            }


            if (project.IsRmatrixRequest.HasValue)
            {
                chkIsRmatrix.Checked = (bool)project.IsRmatrixRequest;

                txtRmatrixNum.Value = project.RmatrixNum.ToString();
                txtRmatrixSubDate.Text = project.RmatrixSubDate != null ? Convert.ToDateTime(project.RmatrixSubDate).ToShortDateString() : string.Empty;
            }
            else
            {
                chkIsRmatrix.Checked = false;
            }

            chkRmatrixReport.Checked = project.IsRmatrixReport.HasValue ? (bool)project.IsRmatrixReport : false;

            txtProjectStatus.Value = project.ProjectStatus;
            txtCompletionDate.Text = project.ProjectCompletionDate != null ? Convert.ToDateTime(project.ProjectCompletionDate).ToShortDateString() : string.Empty;

            txtComments.Value = project.Comments;
            chkApproved.Checked = project.IsApproved;

            if (!Page.User.IsInRole("Admin"))
            {
                chkApproved.Disabled = true;
            }
            else
                chkApproved.Disabled = false;

            chkBiostat.Checked = project.ProjectType == (byte) ProjectType.Biostat;
            chkBioinfo.Checked = project.ProjectType == (byte)ProjectType.Bioinfo;

            chkCreditToBiostat.Checked = project.CreditTo == (byte)ProjectType.Biostat;
            chkCreditToBioinfo.Checked = project.CreditTo == (byte)ProjectType.Bioinfo;
            chkCreditToBoth.Checked = project.CreditTo == (byte)ProjectType.Both;
        }

        private void BindPhaseByProject(int projectId)
        {
            DataTable dt = CreatePhaseTable(projectId);
            
            gvPhase.DataSource = dt;
            gvPhase.DataBind();

            //rptPhaseCompletion.DataSource = dt;
            //rptPhaseCompletion.DataBind();
        }

        private void BindPhaseByIndex(int rowIndex)
        {
            DataTable dt = CreatePhaseTable(0);
            DataRow dr;

            int lastPhase = 0;
            foreach (GridViewRow row in gvPhase.Rows)
            {
                if (row.RowIndex != rowIndex)
                {
                    Label lblId = row.FindControl("lblId") as Label;
                    Label lblPhase = row.FindControl("lblPhase") as Label;
                    TextBox txtStartDate = row.FindControl("txtStartDate") as TextBox;
                    TextBox txtCompletionDate = row.FindControl("txtCompletionDate") as TextBox;
                    TextBox txtTitle = row.FindControl("txtTitle") as TextBox;
                    TextBox txtMsHrs = row.FindControl("txtMsHrs") as TextBox;
                    TextBox txtPhdHrs = row.FindControl("txtPhdHrs") as TextBox;
                    Label agmtId = row.FindControl("lblAgmtId") as Label;

                    dr = dt.NewRow();
                    dr["Id"] = lblId.Text;
                    dr["Name"] = lblPhase.Text;
                    dr["Title"] = txtTitle.Text;
                    dr["MsHrs"] = txtMsHrs.Text;
                    dr["PhdHrs"] = txtPhdHrs.Text;
                    dr["StartDate"] = txtStartDate.Text;
                    dr["CompletionDate"] = txtCompletionDate.Text;
                    dr["AgmtId"] = agmtId.Text;

                    dt.Rows.Add(dr);

                    string[] sPhase = lblPhase.Text.Split('-');
                    Int32.TryParse(sPhase[1], out lastPhase);
                }
            }  

            if (rowIndex < 0 || dt.Rows.Count == 0)
            {
                lastPhase += 1;
                dr = dt.NewRow();
                dr[1] = "Phase-" + lastPhase;
                dt.Rows.Add(dr);
            }           

            gvPhase.DataSource = dt;
            gvPhase.DataBind();

            //rptPhaseCompletion.DataSource = dt;
            //rptPhaseCompletion.DataBind();
        }

        protected void gvPhase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {               
                Label lblAgmtId = e.Row.FindControl("lblAgmtId") as Label;
                Label lblPhase = e.Row.FindControl("lblPhase") as Label;

                if (lblPhase != null && lblAgmtId != null)
                {
                    if (lblPhase.Text == "Phase-0" || lblAgmtId.Text != string.Empty)
                    {
                        LinkButton lb = e.Row.FindControl("lnkDelete") as LinkButton;
                        if (lb != null)
                            lb.Visible = false;
                    }
                }
            }
        }

        protected void gvPhase_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvPhase.Rows[e.RowIndex];

            if (row != null)
            {
                BindPhaseByIndex(e.RowIndex);                    
            }            
        }

        protected void btnAddPhase_Click(object sender, EventArgs e)
        {
            BindPhaseByIndex(-1);
        }

        protected void lnkPI_Command(object sender, CommandEventArgs e)
        {
            int id = 0;
            //int.TryParse(e.CommandArgument as string, out id);
            int.TryParse(ddlPI.SelectedValue, out id);

            if (id > 0)
            {
                Response.Redirect("~/PI?Id=" + id);
            }

        }

        /// <summary>
        /// When 'PI' dropdown is changed,
        /// program changes project dropdown choices with the projects the PI is associated with.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPI_Changed(Object sender, EventArgs e)
        {
            BindProject(-1);       
        }

        /// <summary>
        /// When 'Project' dropdown is changed,
        /// program fills out project fields in relation to the project based on the selected project
        /// through BindProject(projectId) > SetProject(project).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProject_Changed(Object sender, EventArgs e)
        {
            //BindProject(-1);

            int projectId = 0;
            projectId = Int32.TryParse(ddlProject.SelectedValue, out projectId) ? projectId : -1;

            BindProject(projectId);

            //lblMsg.Text = selectedProjectId.ToString();
        }

        #endregion

        #region Add Update 

        #endregion

        #region Other
        //private DataTable CreatePhaseTable(ICollection<ProjectPhase> phases, bool hasRow)
        private DataTable CreatePhaseTable(int projectId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");           
            dt.Columns.Add("Title");
            dt.Columns.Add("MsHrs");
            dt.Columns.Add("PhdHrs");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("CompletionDate");
            dt.Columns.Add("AgmtId");

            if (projectId != 0)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var phases = db.ProjectPhase.Where(p => p.ProjectId == projectId && p.IsDeleted == false).ToList();

                    if (phases == null || phases.Count == 0)
                    {
                        phases = new List<ProjectPhase>();
                        var newPhase = new ProjectPhase()
                        {
                            Name = "Phase-0",
                            Title = "Consultation",
                            MsHrs = 1.0m,
                            PhdHrs = 1.0m
                        };

                        phases.Add(newPhase);
                    }

                    foreach (var phase in phases)
                    {
                        var agmt = db.ClientAgmt.FirstOrDefault(a => a.Project2Id == projectId && a.ProjectPhase == phase.Name);

                        DataRow dr = dt.NewRow();
                        dr[0] = phase.Id;
                        dr[1] = phase.Name;
                        dr[2] = phase.Title;
                        dr[3] = phase.MsHrs;
                        dr[4] = phase.PhdHrs;
                        dr[5] = phase.StartDate != null ? Convert.ToDateTime(phase.StartDate).ToShortDateString() : "";
                        dr[6] = phase.CompletionDate != null ? Convert.ToDateTime(phase.CompletionDate).ToShortDateString() : "";
                        dr[7] = agmt != null ? agmt.AgmtId : "";

                        //decimal dMsOut = 0.0m,
                        //        dPhdOut = 0.0m;
                        //if (phase.ProjectId > 0)
                        //{
                        //    //total spent hours                        
                        //    DateTime startDate = new DateTime(2000, 1, 1), endDate = new DateTime(2099, 1, 1);
                        //    //ObjectParameter startDate = new ObjectParameter("StartDate", typeof(DateTime?));
                        //    //ObjectParameter endDate = new ObjectParameter("EndDate", typeof(DateTime?));
                        //    ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                        //    ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));
                        //    var i = db.P_PROJECTPHASE_HOURS(phase.ProjectId, phase.Name, startDate, endDate, phdHours, msHours);
                        //    db.SaveChanges();

                        //    Decimal.TryParse(phdHours.Value.ToString(), out dPhdOut);
                        //    Decimal.TryParse(msHours.Value.ToString(), out dMsOut);
                        //}

                        //dr[8] = dMsOut;
                        //dr[9] = dPhdOut;

                        dt.Rows.Add(dr);
                    }

                }
            }

            return dt;
        }
        /// <summary>
        /// Binds grid of checkboxes to the values of referred table.
        /// </summary>
        /// <param name="rpt">Grid of Checkboxes (e.g., rptBiostat = List of Biostat Members)</param>
        /// <param name="bitSum">Bitsum of referred field to match grid of checkboxes. 
        ///                      (e.g., Bitsum 894224 = Chelu & Ved [pseudoexample])</param>
        private void BindTable(Repeater rpt, int bitSum)
        {
            foreach (RepeaterItem i in rpt.Items)
            {
                CheckBox cb, cb1, cb2;
                HiddenField hdnBitValue, hdnBitValue1, hdnBitValue2;

                cb = (CheckBox)i.FindControl("chkId");
                hdnBitValue = (HiddenField)i.FindControl("BitValue");

                if (cb != null && hdnBitValue != null)
                {
                    cb.Checked = bitSum > 0 ? CheckBitValue(bitSum, hdnBitValue) : false;                    
                }

                if (cb == null)
                {
                    cb1 = (CheckBox)i.FindControl("FirstchkId");
                    cb2 = (CheckBox)i.FindControl("SecondchkId");

                    hdnBitValue1 = (HiddenField)i.FindControl("FirstBitValue");
                    hdnBitValue2 = (HiddenField)i.FindControl("SecondBitValue");

                    if (cb1 != null && hdnBitValue1 != null)
                    {
                        cb1.Checked = bitSum > 0 ? CheckBitValue(bitSum, hdnBitValue1) : false;
                    }

                    if (cb2 != null && hdnBitValue2 != null)
                    {
                        cb2.Checked = bitSum > 0 ? CheckBitValue(bitSum, hdnBitValue2) : false;
                    }
                }
            }
        }

        /// <summary>
        /// Binds grid of checkboxes with choices for selected areas (faculty/staff, etc).
        /// </summary>
        /// <param name="collection">List of choices for given selected area.</param>
        /// <param name="rpt">Grid for selected area.</param>
        private void BindTable2(Dictionary<int, string> collection, Repeater rpt)
        {
            DataTable dt = new DataTable("tblRpt");

            dt.Columns.Add("Id1", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name1", System.Type.GetType("System.String"));
            dt.Columns.Add("BitValue1", System.Type.GetType("System.Int32"));

            dt.Columns.Add("Id2", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Name2", System.Type.GetType("System.String"));
            dt.Columns.Add("BitValue2", System.Type.GetType("System.Int32"));

            var query = collection.ToArray();

            for (int i = 0; i < query.Length; i += 2)
            {
                DataRow dr = dt.NewRow();

                dr[0] = query[i].Key;
                dr[1] = query[i].Value;
                dr[2] = query[i].Key;

                if (i < query.Length - 1)
                {
                    dr[3] = query[i + 1].Key;
                    dr[4] = query[i + 1].Value;
                    dr[5] = query[i + 1].Key;
                }
                else
                {
                    dr[3] = 0;
                    dr[4] = "";
                    dr[5] = 0;
                }

                dt.Rows.Add(dr);
            }

            rpt.DataSource = dt;
            rpt.DataBind();
        }

        private Project2 InitProject(int piId)
        {
            Project2 project = new Project2()
            {
                Id = 0,
                PIId = piId > 0 ? piId : -1,
                Title = "",
                Summary = "",
                InitialDate = DateTime.Now,
                DeadLine = (DateTime?)null,
                LeadBiostatId = -1,
                OtherMemberBitSum = 0,
                StudyAreaBitSum = 0,
                StudyAreaOther = "",
                HealthDateBitSum = 0,
                HealthDataOther = "",
                StudyTypeBitSum = 0,
                StudyTypeOther = "",
                StudyPopulationBitSum = 0,
                StudyPopulationOther = "",
                ServiceBitSum = 0,
                ServiceOther = "",
                GrantBitSum = 0,
                GrantOther = "",
                //RequestRcvdDate = (DateTime?)null,
                IsJuniorPI = (bool?)null,
                HasMentor = (bool?)null,
                MentorFirstName = "",
                MentorLastName = "",
                MentorEmail = "",
                IsInternal = (bool?)null,
                IsPilot = (bool?)null,
                IsPaid = (bool?)null,
                IsRmatrixRequest = (bool?)null,
                IsRmatrixReport = (bool?)null,
                TypeOfPayment = "",
                RmatrixNum = (Int32?)null,
                RmatrixSubDate = (DateTime?)null,
                ProjectCompletionDate = (DateTime?)null,
                ProjectStatus = "",
                Comments = "",
                IsApproved = false,
                Creator = User.Identity.Name,
                CreationDate = DateTime.Now,
                ProjectType = 1,
                CreditTo = 1
            };

            return project;
        }

        private Project2 GetProject()
        {
            int id = 0,
                piId = 0,
                leadBiostatId = 0,
                otherMemberBitSum = 0,
                studyAreaBitSum = 0,
                healthDataBitSum = 0,
                studyTypeBitSum = 0,
                studyPopulationBitSum = 0,
                serviceBitSum = 0,
                grantBitSum = 0,
                rmatrixNum = 0;

            DateTime dtInitialDate, dtDeadline, dtRmatrixSubDate, dtCompletionDate;

            Project2 project = new Project2()
            {
                Id = Int32.TryParse(ddlProject.SelectedValue, out id) ? id : 0,
                PIId = Int32.TryParse(ddlPI.SelectedValue, out piId) ? piId : -1,
                Title = txtTitle.Value,
                Summary = txtSummary.Value,
                InitialDate = DateTime.TryParse(txtInitialDate.Text, out dtInitialDate) ? dtInitialDate : DateTime.Now,
                DeadLine = DateTime.TryParse(txtDeadline.Text, out dtDeadline) ? dtDeadline : (DateTime?)null,
                LeadBiostatId = Int32.TryParse(ddlLeadBiostat.SelectedValue, out leadBiostatId) ? leadBiostatId : -1,
                OtherMemberBitSum = Int32.TryParse(txtOtherMemberBitSum.Value, out otherMemberBitSum) ? otherMemberBitSum : 0,
                StudyAreaBitSum = Int32.TryParse(txtStudyAreaBitSum.Value, out studyAreaBitSum) ? studyAreaBitSum : 0,
                StudyAreaOther = txtStudyAreaOther.Value,
                HealthDateBitSum = Int32.TryParse(txtHealthDataBitSum.Value, out healthDataBitSum) ? healthDataBitSum : 0,
                HealthDataOther = txtHealthDataOther.Value,
                StudyTypeBitSum = Int32.TryParse(txtStudyTypeBitSum.Value, out studyTypeBitSum) ? studyTypeBitSum : 0,
                StudyTypeOther = txtStudyTypeOther.Value,
                StudyPopulationBitSum = Int32.TryParse(txtStudyPopulationBitSum.Value, out studyPopulationBitSum) ? studyPopulationBitSum : 0,
                StudyPopulationOther = txtStudyPopulationOther.Value,
                ServiceBitSum = Int32.TryParse(txtServiceBitSum.Value, out serviceBitSum) ? serviceBitSum : 0,
                ServiceOther = txtServiceOther.Value,
                GrantBitSum = Int32.TryParse(txtGrantBitSum.Value, out grantBitSum) ? grantBitSum : 0,
                GrantOther = txtGrantOther.Value,
                //RequestRcvdDate = DateTime.TryParse(txtRequestRcvdDate.Text, out dtRequestRcvdDate) ? dtRequestRcvdDate : (DateTime?)null,
                IsJuniorPI = chkJuniorPIYes.Checked, //(bool?)null,
                HasMentor = chkMentorYes.Checked,
                MentorFirstName = txtMentorFirstName.Value,
                MentorLastName = txtMentorLastName.Value,
                MentorEmail = txtMentorEmail.Value,
                IsInternal = chkInternalYes.Checked,
                IsPilot = chkPilotYes.Checked,
                IsPaid = chkPayingYes.Checked,
                IsRmatrixRequest = chkIsRmatrix.Checked,
                IsRmatrixReport = chkRmatrixReport.Checked,
                TypeOfPayment = txtPayProject.Value,
                RmatrixNum = Int32.TryParse(txtRmatrixNum.Value, out rmatrixNum) ? rmatrixNum : (Int32?)null,
                RmatrixSubDate = DateTime.TryParse(txtRmatrixSubDate.Text, out dtRmatrixSubDate) ? dtRmatrixSubDate : (DateTime?)null,
                ProjectCompletionDate = DateTime.TryParse(txtCompletionDate.Text, out dtCompletionDate) ? dtCompletionDate : (DateTime?)null,
                ProjectStatus = txtProjectStatus.Value,
                Comments = txtComments.Value,
                IsApproved = chkApproved.Checked,
                Creator = User.Identity.Name,
                CreationDate = DateTime.Now,
                ProjectType = chkBiostat.Checked ? (byte)ProjectType.Biostat : (byte)ProjectType.Bioinfo,
                CreditTo = chkCreditToBiostat.Checked ? (byte)ProjectType.Biostat : chkCreditToBioinfo.Checked ? (byte)ProjectType.Bioinfo : (byte)ProjectType.Both
            };

            return project;
        }

        private List<ProjectPhase> GetPhase(int projectId)
        {
            List<ProjectPhase> phases = new List<ProjectPhase>();

            foreach (GridViewRow row in gvPhase.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lblId = row.FindControl("lblId") as Label;
                    Label lblPhase = row.FindControl("lblPhase") as Label;
                    TextBox txtTitle = row.FindControl("txtTitle") as TextBox;
                    TextBox txtMsHrs = row.FindControl("txtMsHrs") as TextBox;
                    TextBox txtPhdHrs = row.FindControl("txtPhdHrs") as TextBox;
                    TextBox txtStartDate = row.FindControl("txtStartDate") as TextBox;
                    TextBox txtCompletionDate = row.FindControl("txtCompletionDate") as TextBox;
                                        
                    int output = 0;
                    decimal dMsOutput = 0.0m, dPhdOutput = 0.0m;
                    DateTime dtStart, dtEnd;

                    if (lblPhase != null)
                    {
                        ProjectPhase pPhase = new ProjectPhase()
                        {
                            Id = int.TryParse(lblId.Text, out output) ? output : -1,
                            ProjectId = projectId,
                            Name = lblPhase.Text,
                            Title = txtTitle.Text,
                            MsHrs = decimal.TryParse(txtMsHrs.Text, out dMsOutput) ? dMsOutput : default(decimal?),
                            PhdHrs = decimal.TryParse(txtPhdHrs.Text, out dPhdOutput) ? dPhdOutput : default(decimal?),                            
                            StartDate = DateTime.TryParse(txtStartDate.Text, out dtStart) ? dtStart : (DateTime?)null,
                            CompletionDate = DateTime.TryParse(txtCompletionDate.Text, out dtEnd) ? dtEnd : (DateTime?)null,
                            Creator = Page.User.Identity.Name,
                            CreateDate = DateTime.Now
                        };

                        phases.Add(pPhase);
                    }
                }
            }

            return phases;
        }

        protected void btnSurvey_Click(object sender, EventArgs e)
        {
            int projectId = 0;
            Int32.TryParse(ddlProject.SelectedValue, out projectId);

            DateTime projectCompletionDate;
            if (projectId > 0 && DateTime.TryParse(txtCompletionDate.Text, out projectCompletionDate))
            {
                SurveyForm sf = GetSurvey(projectId);

                int requestCount = sf.RequestCount;
                if (sf.Project2 != null && requestCount > 1)
                {
                    if (sf.Responded)
                    {
                        Response.Redirect("~/Guest/PISurveyForm?Id=" + sf.Id);
                    }
                    else if (requestCount > 1)
                    {                      
                        lblSurveyMsg.Text = "Survey has been sent to PI twice already.";
                        divProjectInfo.Visible = false;
                        btnSendSurvey.Visible = false;                        
                    }
                    else
                        UpdateSurvey(sf.Id);
                }
                else
                {
                    string PIName = ddlPI.SelectedItem.Text;
                    lblSurveyMsg.Text = String.Format("Survey invitation will be sent to PI {0}."
                         , PIName + "(" + sf.SendTo + ")");

                    lblProjectTitle.Text = txtTitle.Value;
                    lblBiostats.Text = sf.LeadBiostat;
                    lblProjectPeriod.Text = ((DateTime)sf.ProjectInitialDate).ToShortDateString() + " - " + ((DateTime)sf.ProjectCompletionDate).ToShortDateString();
                    //lblServiceHours.Text = sf.PhdHours + " PhD hours; " + sf.MsHours + " MS hours";

                    divProjectInfo.Visible = true;
                    btnSendSurvey.Visible = true;
                }
            }
            else
            {
                lblSurveyMsg.Text = "Please check project is completed.";
                btnSendSurvey.Visible = false;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#surveyModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ShowModalScript", sb.ToString(), false);

            //Response.Redirect("Guest/PISurveyForm");
        }              

        protected void btnSendSurvey_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            int projectId = 0;
            Int32.TryParse(ddlProject.SelectedValue, out projectId);

            if (projectId > 0)
            {
                //create a survey, return a survey form id
                //string surveyFormId = LoadSurvey(projectId);

                SurveyForm surveyForm = GetSurvey(projectId);

                if (surveyForm != null)
                {
                    using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                    {
                        var sur = db.SurveyForms.FirstOrDefault(s => s.ProjectId == surveyForm.ProjectId);
                        if (sur == null)
                        {
                            db.SurveyForms.Add(surveyForm);
                            db.SaveChanges();
                        }
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

        private SurveyForm GetSurvey(int projectId)
        {
            SurveyForm surveyForm = null;
            //string surveyId = string.Empty;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var project = db.Project2.FirstOrDefault(p => p.Id == projectId);

                if (project != null)
                {
                    surveyForm = db.SurveyForms.FirstOrDefault(s => s.ProjectId == projectId);

                    if (surveyForm == null)
                    {
                        decimal p = 0.0M, m = 0.0M;

                        //ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                        //ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));
                        //var i = db.P_PROJECT_HOURS(project.InitialDate, project.ProjectCompletionDate, projectId, phdHours, msHours);
                        //db.SaveChanges();

                        //Decimal.TryParse(phdHours.Value.ToString(), out p);
                        //Decimal.TryParse(msHours.Value.ToString(), out m);

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
                                Comment = string.Empty,
                                RequestCount = 1
                            };

                            surveyForm = sf;
                        }

                    }
                }
            }

            return surveyForm;
        }

        private void UpdateSurvey(string surveyId)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var survey = db.SurveyForms.FirstOrDefault(s => s.Id == surveyId);

                if (survey != null)
                {
                    survey.RequestCount += 1;
                    db.SaveChanges();
                }
            }
        }

        protected void btnAddGrant_Click(object sender, EventArgs e)
        {
            int projectId = 0;
            Int32.TryParse(ddlProject.SelectedValue, out projectId);

            if (projectId > 0)
            {
                Response.Redirect("~/Tracking/GrantForm?ProjectId=" + projectId);
            }
        }

        //protected void btnRest_Click(object sender, EventArgs e)
        //{
        //    Project2 initProject = InitProject(-1);
        //    SetProject(initProject);

        //    BindPhaseByProject(0);
        //}        

        private bool CheckBitValue(int bitSum, HiddenField hdnBitValue)
        {
            int bitValue = 0;
            Int32.TryParse(hdnBitValue.Value, out bitValue);

            int c = bitSum & bitValue;

            return c == bitValue;
        }

        #endregion

        #region email
        /// <summary>
        /// Sends notification email to QHS Admin when a user enters
        /// a new project into the Project Tracking System.
        /// </summary>
        /// <param name="projectId">Id of new project that was recently entered.</param>
        private void SendNotificationEmail(int projectId)
        {
            string sendTo = System.Configuration.ConfigurationManager.AppSettings["trackingEmail"];                     

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
                Destination = sendTo,
                Body = body.ToString()
            };

            EmailService emailService = new EmailService();

            emailService.Send(im);
        }

        /// <summary>
        /// When "Send Survey to Client" button has been clicked, an email will notify
        /// clients (Principal Investigator) of the project to ask for a client survey
        /// to review the services that were provided.
        /// </summary>
        /// <param name="surveyForm">Survey form that user will need to fill out.</param>
        private void SendSurveyEmail(SurveyForm surveyForm)
        {
            //send email invitation
            EmailService email = new EmailService();

            string subject = "BQHS Follow-up Survey: " + txtTitle.Value;

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = manager.FindByName(User.Identity.Name);
            string destination = user.Email;

            //surveyLink = "~/Guest/PISurvey?SurveyId=" + surveyForm.Id,
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            //caution!!! make sure this is for production 
            if (System.Configuration.ConfigurationManager.AppSettings["isProduction"].Equals("Y"))
            {
                url = System.Configuration.ConfigurationManager.AppSettings["internet"] + "/ProjectForm2";
                destination = surveyForm.SendTo;
            }

            string surveyLink = url.Replace("ProjectForm2", "Guest/PISurveyForm?Id=" + surveyForm.Id);

            StringBuilder body = new StringBuilder();
            body.AppendFormat("<p>Dear {0},</p>", ddlPI.SelectedItem.Text);
            body.AppendLine("<p></P>");
            body.AppendLine(@"<p>Thank you for working with us. The JABSOM Office of Biostatistics and Quantitative Health Sciences (BQHS) strives for excellence in high quality, efficient, and reliable collaborations and services in the quantitative sciences. To further improve our support, we invite you to complete a brief follow-up survey regarding your project listed below. This survey will take less than 5 minutes of your time. Your feedback is valuable to us.</p>");
            body.AppendLine("<p></P>");
            body.AppendFormat("<p style=\"margin-left:.5in\">Project title: {0}<br />", txtTitle.Value);
            body.AppendFormat("Faculty/Staff: {0}<br />", surveyForm.LeadBiostat);
            body.AppendFormat("Project period: {0} - {1}<br />", ((DateTime)surveyForm.ProjectInitialDate).ToShortDateString(), ((DateTime)surveyForm.ProjectCompletionDate).ToShortDateString());
            //body.AppendFormat("Service hours: {0} PhD hours; {1} MS hours</p>", surveyForm.PhdHours, surveyForm.MsHours);
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

        #endregion

    }
}