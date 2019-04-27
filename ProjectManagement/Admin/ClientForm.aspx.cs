using Microsoft.AspNet.Identity;
using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    /// <summary>
    /// @File: ClientForm.aspx.cs
    /// @FrontEnd: ClientForm.aspx
    /// @Author: Yang Rui
    /// @Summary: Admin Review Form for Client Requests.
    /// 
    ///           Page for admin to review Client Request forms and see whether or not they can 
    ///           be added into projects.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2019APR11 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///                                 -  Linked Client Request Form to BQHS Request Database.
    ///  2019APR17 - Jason Delos Reyes  -  Added functionality to convert client request form into
    ///                                    PI and Project entries. Still need to fix notification
    ///                                    emails as they send to the wrong pages.
    ///  2019APR18 - Jason Delos Reyes  -  Fixed notification emails to redirect users to the correct pages.
    ///                                    Need to enhance Client Review forms to include:
    ///                                           √ Unhiding overview,
    ///                                           √ Replacing "Update" button with "Create Project" button,
    ///                                           √ Close client request modal and create alert that project has been created,
    ///                                           √ Make client fields editable while "completed" checkbox unchecked,
    ///                                           √ Make "Completed" checkbox disabled and progmatically create if a project
    ///                                             has been created.  At this point, the "Create Project" button will also be disabled.
    ///                                           • Make fields editable upon review and request is "Created".
    ///                                           • Make PI editable with new information instead of just pulling old information.
    ///                                           
    /// </summary>
    public partial class ClientForm : System.Web.UI.Page
    {
        /// <summary>
        /// Loads the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }
        }

        /// <summary>
        /// Pulls all of the client requests and survey reponses to display on current form.
        /// </summary>
        private void BindControl()
        {
            DataTable clientRqstTable = GetClientRqstAll();

            rptClientRqst.DataSource = clientRqstTable;
            rptClientRqst.DataBind();

            DataTable surveyTable = GetSurveyAll();
            rptSurvey.DataSource = surveyTable;
            rptSurvey.DataBind();
        }

        /// <summary>
        /// Create an instance of a new project in the live database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            using (ClientRequestTracker cr = new ClientRequestTracker())
            {

                int rqstId = 0;
                int.TryParse(lblClientRqstId.Text, out rqstId);

                if (rqstId > 0)
                {

                    ClientRequest2_cr rqst = GetClientRequestById(rqstId);

                    if (rqst != null)
                    {

                        using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                        {
                            ///<> PI <> ///
                            // If there is not an existing PI in the database,
                            // Create create new PI entry and send email to admin.
                            int investId = 0;
                            Invest pi = db.Invests.FirstOrDefault(x => x.FirstName + " " + x.LastName == rqst.FirstName + " " + rqst.LastName);


                            if (pi == null)
                            {
                                Invest invest = CreateInvest(rqst);

                                if (invest != null)
                                {
                                    db.Invests.Add(invest);
                                    db.SaveChanges();

                                    investId = invest.Id;

                                    SendPINotificationEmail(investId);
                                }

                            }
                            else
                            {
                                investId = pi.Id;
                            }

                            ///<> Project <> ///
                            // Push create new PROJECT entry and send email to admin for approval. 


                            Project2 project = CreateProject(rqst);
                            project.PIId = investId;

                            if (project != null)
                            {
                                db.Project2.Add(project);
                                db.SaveChanges();

                                SendProjectNotificationEmail(project.Id);
                            }

                            // Print success message and close window.
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "ModalScript", PageUtility.LoadEditScript(false), false);

                            // Make "Completed" checkbox checked.
                            cr.ClientRequest2_cr.FirstOrDefault(t => t.Id == rqstId).RequestStatus = "Completed";
                            cr.SaveChanges();


                            // Rebind list of Client Requests.
                            DataTable clientRqstTable = GetClientRqstAll();
                            rptClientRqst.DataSource = clientRqstTable;
                            rptClientRqst.DataBind();

                        }

                    }


                }
            }


            //using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            //{
            //    int rqstId = 0;
            //    int.TryParse(lblClientRqstId.Text, out rqstId);

            //    if (rqstId > 0)
            //    {
            //        var rqst = db.ClientRequest.FirstOrDefault(c => c.Id == rqstId);

            //        if (rqst != null)
            //        {
            //            rqst.RequestStatus = chkCompleted.Checked ? "Completed" : "Requested";
            //            db.SaveChanges();
            //        }
            //    }
            //}

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            //           "ModalScript", PageUtility.LoadEditScript(false), false);

            //DataTable clientRqstTable = GetClientRqstAll();

            //rptClientRqst.DataSource = clientRqstTable;
            //rptClientRqst.DataBind();
        }

        /// <summary>
        /// Marks "Client Request Form" as "Completed" or "Requested".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                int rqstId = 0;
                int.TryParse(lblClientRqstId.Text, out rqstId);

                if (rqstId > 0)
                {
                    var rqst = db.ClientRequest.FirstOrDefault(c => c.Id == rqstId);

                    if (rqst != null)
                    {
                        rqst.RequestStatus = chkCompleted.Checked ? "Completed" : "Requested";
                        db.SaveChanges();
                    }
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(false), false);

            DataTable clientRqstTable = GetClientRqstAll();

            rptClientRqst.DataSource = clientRqstTable;
            rptClientRqst.DataBind();
        }

        /// <summary>
        /// Pulls current PI or generates new PI based on what is in the database.
        /// </summary>
        /// <param name="rqst">Provided client request from BQHSRequest Database.</param>
        /// <returns>Created Invest </returns>
        private Invest CreateInvest(ClientRequest2_cr rqst)
        {
            int investId = 0;
            Invest invest;


            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                
                invest = new Invest()
                {

                    FirstName = rqst.FirstName,
                    LastName = rqst.LastName,
                    OtherDegree = rqst.Degree,
                    Email = rqst.Email,
                    Phone = rqst.Phone,
                    InvestStatusId = db.InvestStatus.FirstOrDefault(g => g.StatusValue == rqst.InvestStatus) != null ?
                                     db.InvestStatus.FirstOrDefault(g => g.StatusValue == rqst.InvestStatus).Id : -1,
                    IsApproved = false
                    
                   

                };

                db.Invests.Add(invest);
                db.SaveChanges();

                investId = invest.Id;

                // Update PI affiliations

                //--->Degree
                JabsomAffil investDegree = db.JabsomAffils.FirstOrDefault(g => g.Name == rqst.Degree);
                if (investDegree != null) invest.JabsomAffils.Add(investDegree);

                //-->Organization
                JabsomAffil investOrganization = db.JabsomAffils.FirstOrDefault(g => g.Name == rqst.Department);
                if (investOrganization != null) {
                    invest.JabsomAffils.Add(investOrganization);
                } else
                {
                    invest.NonUHClient = rqst.Department;
                }


                db.SaveChanges();

            }


            return invest;

        }

        /// <summary>
        /// Generates new Project based on what is in the database.
        /// </summary>
        /// <param name="rqst">Provided client request from BQHSRequest Database.</param>
        /// <returns>Created Project</returns>
        private Project2 CreateProject(ClientRequest2_cr rqst)
        {

            int id = 0,
                piId = 0,
                //leadBiostatId = 0,
                //otherMemberBitSum = 0,
                //studyAreaBitSum = 0,
                //healthDataBitSum = 0,
                //studyTypeBitSum = 0,
                //studyPopulationBitSum = 0,
                //serviceBitSum = 0,
                //grantBitSum = 0,
                //grantDepartmentFundingType = 0,
                aknBitSum = 0;
                //aknDepartmentFundingType = 0,
                //rmatrixNum = 0,
                //olaHawaiiNum = 0,
                //uhGrantId = 0;

            long otherMemberBitSum = 0;

            //DateTime dtDeadline, dtRmatrixSubDate, dtOlaHawaiiSubDate, dtCompletionDate;

           
            Project2 project;


            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {

                project = new Project2()
                {
                    Id = id,   // required 
                    PIId = piId, // required
                    Title = rqst.ProjectTitle,
                    Summary = rqst.ProjectSummary,
                    InitialDate = DateTime.Now,
                    DeadLine = rqst.DeadLine,
                    LeadBiostatId = rqst.BiostatId != null ? (int)rqst.BiostatId : -1,// required
                    OtherMemberBitSum = otherMemberBitSum, // required
                    StudyAreaBitSum = rqst.StudyAreaBitSum, // required
                    StudyAreaOther = rqst.StudyAreaOther,
                    HealthDateBitSum = rqst.HealthDateBitSum, // required
                    HealthDataOther = rqst.HealthDataOther,
                    StudyTypeBitSum = rqst.StudyTypeBitSum, // required
                    StudyTypeOther = rqst.StudyTypeOther,
                    StudyPopulationBitSum = rqst.StudyPopulationBitSum, // required
                    StudyPopulationOther = rqst.StudyPopulationOther,
                    IsHealthDisparity = rqst.IsHealthDisparity,
                    ServiceBitSum = rqst.ServiceBitSum, // required
                    ServiceOther = rqst.ServiceOther,
                    GrantBitSum = rqst.GrantBitSum, // required
                    GrantOther = rqst.GrantOther,
                    GrantDepartmentFundingType = rqst.GrantDepartmentFundingType,
                    GrantDepartmentFundingOther = rqst.GrantDepartmentFundingOther,
                    AknBitSum = aknBitSum,
                    IsJuniorPI = rqst.IsJuniorPI,
                    HasMentor = rqst.HasMentor,
                    MentorFirstName = rqst.MentorFirstName,
                    MentorLastName = rqst.MentorLastName,
                    MentorEmail = rqst.MentorEmail,
                    IsPilot = rqst.IsPilot,
                    IsGrantProposal = rqst.IsGrantProposal,
                    IsUHGrant = rqst.IsUHGrant,
                    UHGrantID = db.GrantAffils.FirstOrDefault(g => g.GrantAffilName == rqst.UHGrantName)?.Id,
                    GrantProposalFundingAgency = rqst.GrantProposalFundingAgency,
                    IsInternal = false,
                    IsApproved = false,
                    Creator = User.Identity.Name,
                    CreationDate = DateTime.Now,
                    ProjectType = (byte)1, //biostat
                    CreditTo = (byte)1 // biostat

                };

            }


            return project;

        }


        /// <summary>
        /// Sends a notification email to tracking admin that a new PI has been created.
        /// Only called when a new PI has been saved into the database, i.e., doesn't currently exist in
        /// the database.
        /// </summary>
        /// <param name="investId">PI Id</param>
        private void SendPINotificationEmail(int investId)
        {
            string email = System.Configuration.ConfigurationManager.AppSettings["trackingEmail"];

            MailAddress destination = new MailAddress(email);

            string subject = String.Format("A new PI is pending approval, id {0}", investId);

            Uri uriAddress = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            string url = uriAddress.GetLeftPart(UriPartial.Authority) + "/PI";//HttpContext.Current.Request.Url.AbsoluteUri;
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

        /// <summary>
        /// Sends notification email to QHS Admin when a user enters
        /// a new project into the Project Tracking System.
        /// </summary>
        /// <param name="projectId">Id of new project that was recently entered.</param>
        /// <param name="sendToFiscal">Determines whether or not a paying project is sent to fiscal team.</param>
        private void SendProjectNotificationEmail(int projectId)
        {
            string sendTo = System.Configuration.ConfigurationManager.AppSettings["trackingEmail"];

            /// Sends to fiscal team if a project has been indicated as a paying project.
            //if (sendToFiscal == true) sendTo = sendTo + ","
            //        + System.Configuration.ConfigurationManager.AppSettings["superAdminEmail"];


            string subject = String.Format("A new project is pending approval, id {0}", projectId);

            Uri uriAddress = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            string url = uriAddress.GetLeftPart(UriPartial.Authority) + "/ProjectForm2";//HttpContext.Current.Request.Url.AbsoluteUri;
            if (url.IndexOf("?Id") > 0)
            {
                sendTo = sendTo + ";" /*+ System.Configuration.ConfigurationManager.AppSettings["superAdminEmail"]*/;
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
        /// Obtains all the survey reponses and displays them onto a list.
        /// </summary>
        /// <returns></returns>
        private DataTable GetSurveyAll()
        {
            DataTable dt = new DataTable("surveyTable");

            dt.Columns.Add("ProjectId", System.Type.GetType("System.Int32"));
            dt.Columns.Add("ProjectTitle", System.Type.GetType("System.String"));
            dt.Columns.Add("SendTo", System.Type.GetType("System.String"));
            dt.Columns.Add("Responded", System.Type.GetType("System.String"));
            dt.Columns.Add("RespondDate", System.Type.GetType("System.String"));
            dt.Columns.Add("Id", System.Type.GetType("System.String"));
            dt.Columns.Add("SendDate", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var query = db.SurveyForms
                    .Select(t => new { t.Id, t.ProjectId, t.ProjectTitle, t.SendTo, t.Responded, t.RespondDate, t.RequestDate })
                    .OrderByDescending(t => t.RequestDate);


                foreach (var p in query.ToList())
                {
                    DataRow row = dt.NewRow();

                    row[0] = p.ProjectId;
                    row[1] = p.ProjectTitle;
                    row[2] = p.SendTo;
                    row[3] = p.Responded ? "Yes" : "No";
                    row[4] = p.RespondDate != null ? Convert.ToDateTime(p.RespondDate).ToShortDateString() : "";
                    row[5] = p.Id;
                    row[6] = p.RequestDate != null ? Convert.ToDateTime(p.RequestDate).ToShortDateString() : "";

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        /// <summary>
        /// Obtains all Client Request Forms from the database.
        /// </summary>
        /// <returns></returns>
        private DataTable GetClientRqstAll()
        {
            DataTable dt = new DataTable("clientRqstTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("FirstName", System.Type.GetType("System.String"));
            dt.Columns.Add("LastName", System.Type.GetType("System.String"));
            dt.Columns.Add("ProjectTitle", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateDate", System.Type.GetType("System.String"));
            dt.Columns.Add("Status", System.Type.GetType("System.String"));

            using (ClientRequestTracker cr = new ClientRequestTracker())
            {
                var query = cr.ClientRequest2_cr
                    .Select(t => new { t.Id, t.FirstName, t.LastName, t.ProjectTitle, t.CreationDate, t.RequestStatus })
                    .OrderByDescending(t => t.Id);


                foreach (var p in query.OrderByDescending(p => p.Id).ToList())
                {
                    DataRow row = dt.NewRow();

                    row[0] = p.Id;
                    row[1] = p.FirstName;
                    row[2] = p.LastName;
                    row[3] = p.ProjectTitle;
                    row[4] = p.CreationDate != null ? Convert.ToDateTime(p.CreationDate).ToShortDateString() : "";
                    row[5] = p.RequestStatus;

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        //protected void rptSurvey_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        //{
        //    if (((Button)e.CommandSource).Text.Equals("View"))
        //    {
        //        string strProjectId = ((Button)e.CommandSource).CommandArgument;

        //        int projectId = 0;
        //        Int32.TryParse(strProjectId, out projectId);

        //        string surveyId = "";
        //        if (projectId > 0)
        //        {                    
        //            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
        //            {
        //                var survey = db.SurveyForms.FirstOrDefault(s => s.ProjectId == projectId);

        //                if (survey != null)
        //                {
        //                    surveyId = survey.Id;
        //                }
        //            }
        //        }

        //        if (surveyId != "")
        //        {
        //            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('Guest/PISurveyForm?Id=" + surveyId + "');", true);
        //        }
        //    }
        //}

        /// <summary>
        /// Opens specific Client Request Form entry. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptClientRqst_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                lblClientRqstId.Text = ((Button)e.CommandSource).CommandArgument;

                int rqstId = 0;
                int.TryParse(lblClientRqstId.Text, out rqstId);

                if (rqstId > 0)
                {
                    ClientRequest2_cr rqst = GetClientRequestById(rqstId);

                    if (rqst != null)
                    {
                        BindEditModal(rqst);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "ModalScript", PageUtility.LoadEditScript(true), false);
                    }
                }
            }
        }

        /// <summary>
        /// Binds given request to web form.
        /// </summary>
        /// <param name="rqst">Given request.</param>
        private void BindEditModal(ClientRequest2_cr rqst)
        {
            using (ClientRequestTracker cr = new ClientRequestTracker())
            {

                DateTime dt;

                // Obtain list of study areas
                Dictionary<int, string> studyAreas = cr.ProjectField_cr
                                                       .Where(f => f.IsStudyArea == true)
                                                       .Select(d => new { d.BitValue, d.Name })
                                                       .ToDictionary(g => g.BitValue, g => g.Name);

                Dictionary<int, string> healthData = cr.ProjectField_cr
                                                       .Where(f => f.IsHealthData == true)
                                                       .Select(d => new { d.BitValue, d.Name })
                                                       .ToDictionary(g => g.BitValue, g => g.Name);

                Dictionary<int, string> studyType = cr.ProjectField_cr
                                                       .Where(f => f.IsStudyType == true)
                                                       .Select(d => new { d.BitValue, d.Name })
                                                       .ToDictionary(g => g.BitValue, g => g.Name);

                Dictionary<int, string> studyPopulation = cr.ProjectField_cr
                                                            .Where(f => f.IsStudyPopulation == true)
                                                            .Select(d => new { d.BitValue, d.Name })
                                                            .ToDictionary(g => g.BitValue, g => g.Name);
                Dictionary<int, string> service = cr.ProjectField_cr
                                                    .Where(f => f.IsService == true)
                                                    .Select(d => new { d.BitValue, d.Name })
                                                    .ToDictionary(g => g.BitValue, g => g.Name);

                Dictionary<int, string> fundingSource = cr.ProjectField_cr
                                                   .Where(f => f.IsFundingSource == true)
                                                   .Select(d => new { d.BitValue, d.Name })
                                                   .ToDictionary(g => g.BitValue, g => g.Name);

                Dictionary<int, string> biostats = cr.BioStat_cr
                                                   .Where(f => f.EndDate > DateTime.Today && f.Name != "N/A")
                                                   .Select(d => new { d.Id, d.Name })
                                                   .ToDictionary(g => g.Id, g => g.Name);

                if (rqst != null)
                {
                    txtFirstName.Value = rqst.FirstName; //lblFirstName.Text = rqst.FirstName;
                    txtLastName.Value = rqst.LastName;   //lblLastName.Text = rqst.LastName;
                    
                    /// Populates "Degree" dropdown
                    var dropDownSource = cr.JabsomAffil_cr
                                       .Where(d => d.Type == "Degree")
                                       .OrderBy(d => d.Name)
                                       .ToDictionary(c => c.Id, c => c.Name);
                    PageUtility.BindDropDownList(ddlDegree, dropDownSource, "-- Select Degree --");

                    ddlDegree.SelectedValue = dropDownSource.FirstOrDefault(f => f.Value == rqst.Degree).Key.ToString();

                    txtDegreeOther.Value = rqst.DegreeOther;

                    txtEmail.Value = rqst.Email;
                    txtPhone.Value = rqst.Phone;
                    txtDept.Value = rqst.Department;

                    /// Populates "Organization" typeable dropdown (if available from database)
                    var deptAffil = cr.JabsomAffil_cr
                                      .Where(a => a.Type != "Other" && a.Type != "Degree"
                                                                    && a.Type != "Unknown"
                                                                    && a.Type != "UHFaculty"
                                                                    && a.Name != "Other")
                                      .OrderBy(a => a.Id)
                                      .Select(x => new { Id = x.Id, Name = x.Name });

                    //textAreaDeptAffil.Value = Newtonsoft.Json.JsonConvert.SerializeObject(deptAffil);

                    /// Populates "Investigator Status" dropdown
                    dropDownSource = cr.InvestStatus_cr
                                       .OrderBy(d => d.DisplayOrder)
                                        .ToDictionary(c => c.Id, c => c.StatusValue);

                    PageUtility.BindDropDownList(ddlPIStatus, dropDownSource, "-- Select status --");

                    ddlPIStatus.SelectedValue = dropDownSource.FirstOrDefault(f => f.Value == rqst.InvestStatus).Key.ToString();

                    if (rqst.IsJuniorPI.Equals(true))
                    {
                        chkJuniorPIYes.Checked = true;
                    }
                    else
                    {
                        chkJuniorPINo.Checked = true;
                    }

                    //lblJuniorPI.Text = rqst.IsJuniorPI.Equals(true) ? "Yes" : "No";

                    if (rqst.HasMentor.Equals(true))
                    {
                        chkMentorYes.Checked = true;
                    }
                    else
                    {
                        chkMentorNo.Checked = true;
                    }

                    txtMentorFirstName.Value = rqst.MentorFirstName;
                    txtMentorLastName.Value = rqst.MentorLastName;
                    txtMentorEmail.Value = rqst.MentorEmail;

                    lblProjectTitle.Text = rqst.ProjectTitle;
                    lblProjectSummary.Text = rqst.ProjectSummary;


                    // For each study area, if match study area, then print.
                    foreach (var sa in studyAreas)
                    {
                        int bitValue = sa.Key;

                        int match = (int)rqst.StudyAreaBitSum & bitValue;

                        if (match == sa.Key)
                        {

                            if (lblStudyArea.Text.Equals(string.Empty) && sa.Value.Equals("Other"))
                            {
                                lblStudyArea.Text = sa.Value + " - " + rqst.StudyAreaOther;
                            }
                            else if (!lblStudyArea.Text.Equals(string.Empty) && sa.Value.Equals("Other"))
                            {
                                lblStudyArea.Text = lblStudyArea.Text + ", " + sa.Value + " - " + rqst.StudyAreaOther;
                            }
                            else if (lblStudyArea.Text.Equals(string.Empty))
                            {
                                lblStudyArea.Text = sa.Value;
                            }
                            else
                            {
                                lblStudyArea.Text = lblStudyArea.Text + ", " + sa.Value;
                            }
                        }


                    }

                    // Health Data
                    foreach (var hd in healthData)
                    {
                        int bitValue = hd.Key;

                        int match = (int)rqst.HealthDateBitSum & bitValue;

                        if (match == hd.Key)
                        {

                            if (lblHealthData.Text.Equals(string.Empty) && hd.Value.Equals("Other"))
                            {
                                lblHealthData.Text = hd.Value + " - " + rqst.HealthDataOther;
                            }
                            else if (!lblHealthData.Text.Equals(string.Empty) && hd.Value.Equals("Other"))
                            {
                                lblHealthData.Text = lblHealthData.Text + ", " + hd.Value + " - " + rqst.HealthDataOther;
                            }
                            else if (lblHealthData.Text.Equals(string.Empty))
                            {
                                lblHealthData.Text = hd.Value;
                            }
                            else
                            {
                                lblHealthData.Text = lblHealthData.Text + ", " + hd.Value;
                            }
                        }
                    }


                    // Study Type
                    foreach (var st in studyType)
                    {
                        int bitValue = st.Key;

                        int match = (int)rqst.StudyTypeBitSum & bitValue;

                        if (match == st.Key)
                        {

                            if (lblStudyType.Text.Equals(string.Empty) && st.Value.Equals("Other"))
                            {
                                lblStudyType.Text = st.Value + " - " + rqst.StudyTypeOther;
                            }
                            else if (!lblStudyType.Text.Equals(string.Empty) && st.Value.Equals("Other"))
                            {
                                lblStudyType.Text = lblStudyType.Text + ", " + st.Value + " - " + rqst.StudyTypeOther;
                            }
                            else if (lblStudyType.Text.Equals(string.Empty))
                            {
                                lblStudyType.Text = st.Value;
                            }
                            else
                            {
                                lblStudyType.Text = lblStudyType.Text + ", " + st.Value;
                            }
                        }

                    }

                    // Study Population
                    foreach (var sp in studyPopulation)
                    {
                        int bitValue = sp.Key;

                        int match = (int)rqst.StudyPopulationBitSum & bitValue;

                        if (match == sp.Key)
                        {

                            if (lblStudyPopulation.Text.Equals(string.Empty) && sp.Value.Equals("Other"))
                            {
                                lblStudyPopulation.Text = sp.Value + " - " + rqst.StudyPopulationOther;
                            }
                            else if (!lblStudyPopulation.Text.Equals(string.Empty) && sp.Value.Equals("Other"))
                            {
                                lblStudyPopulation.Text = lblStudyPopulation.Text + ", " + sp.Value + " - " + rqst.StudyPopulationOther;
                            }
                            else if (lblStudyPopulation.Text.Equals(string.Empty))
                            {
                                lblStudyPopulation.Text = sp.Value;
                            }
                            else
                            {
                                lblStudyPopulation.Text = lblStudyPopulation.Text + ", " + sp.Value;
                            }
                        }
                    }


                    // Service
                    foreach (var sv in service)
                    {
                        int bitValue = sv.Key;

                        int match = (int)rqst.ServiceBitSum & bitValue;

                        if (match == sv.Key)
                        {

                            if (lblService.Text.Equals(string.Empty) && sv.Value.Equals("Other"))
                            {
                                lblService.Text = sv.Value + " - " + rqst.ServiceOther;
                            }
                            else if (!lblService.Text.Equals(string.Empty) && sv.Value.Equals("Other"))
                            {
                                lblService.Text = lblService.Text + ", " + sv.Value + " - " + rqst.ServiceOther;
                            }
                            else if (lblService.Text.Equals(string.Empty))
                            {
                                lblService.Text = sv.Value;
                            }
                            else
                            {
                                lblService.Text = lblService.Text + ", " + sv.Value;
                            }
                        }


                    }

                    lblPilot.Text = rqst.IsPilot == true ? "Yes" : "No";

                    lblProposal.Text = rqst.IsGrantProposal == true ? "Yes" : "No";

                    lblUHPilotGrant.Text = rqst.IsUHGrant == true ? "Yes" : "No";

                    lblPilotGrantName.Text = (!rqst.UHGrantName.Equals(string.Empty)
                                                || !rqst.GrantProposalFundingAgency.Equals(string.Empty))
                                                ? (rqst.IsUHGrant == true ? rqst.UHGrantName : rqst.GrantProposalFundingAgency) : "N/A";

                    lblHealthDisparity.Text = rqst.IsHealthDisparity == 1 ? "Yes" : rqst.IsHealthDisparity == 2 ? "No" : "N/A";

                    // Funding Source
                    foreach (var fs in fundingSource)
                    {
                        int bitValue = fs.Key;

                        int match = (int)rqst.GrantBitSum & bitValue;

                        if (match == fs.Key)
                        {

                            if (lblGrant.Text.Equals(string.Empty) && fs.Value.Equals("Other"))
                            {
                                lblGrant.Text = fs.Value + " - " + rqst.GrantOther;
                            }
                            else if (!lblGrant.Text.Equals(string.Empty) && fs.Value.Equals("Other"))
                            {
                                lblGrant.Text = lblGrant.Text + ", " + fs.Value + " - " + rqst.GrantOther;
                            }
                            else if (lblGrant.Text.Equals(string.Empty))
                            {
                                lblGrant.Text = fs.Value;
                            }
                            else
                            {
                                lblGrant.Text = lblGrant.Text + ", " + fs.Value;
                            }
                        }


                    }


                    lblDueDate.Text = DateTime.TryParse(rqst.DeadLine.ToString(), out dt) ? dt.ToShortDateString() : "";


                    string biostatName = cr.BioStat_cr.FirstOrDefault(f => f.Id == rqst.BiostatId).Name;
                    lblBiostat.Text = biostatName;


                    if (rqst.RequestStatus == "Completed")
                    {
                        chkCompleted.Checked = true;
                        btnCreateProject.Enabled = false;
                        btnCreateProject.Text = "-- Project Already Created --";
                        //btnCreateProject.CssClass = "notAllowed";
                    }
                    else
                    {
                        chkCompleted.Checked = false;
                        btnCreateProject.Enabled = true;
                        btnCreateProject.Text = "Create Project";
                        //btnCreateProject.CssClass = "";
                    }
                    

                }

            }


        }

        /// <summary>
        /// Given Client Request form ID, pulls information pertaining to that specific form.
        /// </summary>
        /// <param name="rqstId">Referred to Client Request Form.</param>
        /// <returns>Instance of ClientRequest2 form.</returns>
        private ClientRequest2_cr GetClientRequestById(int rqstId)
        {
            ClientRequest2_cr myRqst;

            using (ClientRequestTracker cr = new ClientRequestTracker())
            {
                myRqst = cr.ClientRequest2_cr.FirstOrDefault(t => t.Id == rqstId);
            }

            return myRqst;
        }
    }
}