﻿using Microsoft.AspNet.Identity;
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
    ///  2019MAY03 - Jason Delos Reyes  -  Fixed the issue that didn't allow javascript code to run.
    ///  2019MAY08 - Jason Delos Reyes  -  Fixed the issue that didn't allow the checkboxes for a checkbox group
    ///                                    (e.g., Study Area) to be clicked or hide the auxilary options.
    ///                                 -  Made all fields disabled if the client request already has a
    ///                                    project form created in the project tracking system.
    ///                                 -  Fixed the issue that duplicated the creation of PI's in the system
    ///                                    whenever a Client Request Form was used to create a new project.
    ///                                 -  Added QHS Banner.
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
                            // Links project to existing project in tracking system
                            if (chkCompleted.Checked.Equals(true))
                            {
                                Project2 project = FindProject(rqst);

                                if (project != null)
                                {
                                    Response.Redirect(String.Format("~/ProjectForm2?Id={0}", project.Id));
                                }
                                else
                                {
                                    btnCreateProject.Enabled = false;
                                    btnCreateProject.Text = "-- Project already created; search manually --";
                                }

                            }
                            else
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
                                        //Delete - duplicates creation of PI's, which duplicates PI records.
                                        //db.Invests.Add(invest);
                                        //db.SaveChanges();

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
                if (investOrganization != null)
                {
                    invest.JabsomAffils.Add(investOrganization);
                }
                else
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
                    UHGrantID = /*db.GrantAffils.FirstOrDefault(g => g.GrantAffilName == rqst.UHGrantName)?.Id*/null,
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


                /// Populates "Degree" dropdown
                var dropDownSource = cr.JabsomAffil_cr
                                   .Where(d => d.Type == "Degree")
                                   .OrderBy(d => d.Name)
                                   .ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlDegree, dropDownSource, "-- Select Degree --");

                //ddlDegree.SelectedValue = dropDownSource.FirstOrDefault(f => f.Value == rqst.Degree).Key.ToString();


                /// Populates "Organization" typeable dropdown (if available from database)
                var deptAffil = cr.JabsomAffil_cr
                                  .Where(a => a.Type != "Other" && a.Type != "Degree"
                                                                && a.Type != "Unknown"
                                                                && a.Type != "UHFaculty"
                                                                && a.Name != "Other")
                                  .OrderBy(a => a.Id)
                                  .Select(x => new { Id = x.Id, Name = x.Name });

                textAreaDeptAffil.Value = Newtonsoft.Json.JsonConvert.SerializeObject(deptAffil);

                /// Populates "Investigator Status" dropdown
                dropDownSource = cr.InvestStatus_cr
                                   .OrderBy(d => d.DisplayOrder)
                                    .ToDictionary(c => c.Id, c => c.StatusValue);

                PageUtility.BindDropDownList(ddlPIStatus, dropDownSource, "-- Select status --");

                //ddlPIStatus.SelectedValue = dropDownSource.FirstOrDefault(f => f.Value == rqst.InvestStatus).Key.ToString();


                /// Populates "Study Area" checkbox grid
                var qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyArea == true).ToList();

                rptStudyArea.DataSource = qProjectField;
                rptStudyArea.DataBind();


                /// Populates "Health Data" checkbox grid
                qProjectField = cr.ProjectField_cr.Where(f => f.IsHealthData == true).ToList();
                rptHealthData.DataSource = qProjectField;
                rptHealthData.DataBind();

                /// Populates "Study Type" checkbox grid.
                qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyType == true).ToList();
                rptStudyType.DataSource = qProjectField;
                rptStudyType.DataBind();

                /// Populates "Study Population" checkbox grid.
                qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyPopulation == true).ToList();
                rptStudyPopulation.DataSource = qProjectField;
                rptStudyPopulation.DataBind();

                /// Populates "Service" checkbox grid.
                qProjectField = cr.ProjectField_cr.Where(f => f.IsService == true).ToList();
                rptService.DataSource = qProjectField;
                rptService.DataBind();

                /// Populates "QHS Faculty/Staff preference" dropdown\
                dropDownSource = cr.BioStat_cr
                                   .Where(b => b.EndDate >= DateTime.Now && b.Id > 0
                                                                         && b.Name != "N/A"
                                                                         && b.Name != "Vedbar Khadka"
                                                                         && b.Name != "Youping Deng"
                                                                         && b.Name != "Mark Menor"
                                                                         && b.Name != "Laura Tipton"
                                                                         && b.Name != "JaNay Wyss")
                                   .OrderBy(b => b.Name)
                                   .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlBiostat, dropDownSource, String.Empty);

                /// Populates "Funding Source" checkbox grid
                var qFundingSource = cr.ProjectField_cr.Where(f => f.IsGrant == true
                                                             && f.IsFundingSource == true
                                                             && f.Name != "N/A"
                                                             && f.Name != "No (No funding)"
                                                             && f.Name != "COBRE-Cardiovascular"
                                                             && f.Name != "RMATRIX"
                                                             && f.Name != "Ola Hawaii"
                                                             && f.Name != "P30 UHCC")
                                      .OrderBy(b => b.DisplayOrder)
                                      .ToDictionary(c => c.Id, c => c.Name);

                BindTable2(qFundingSource, rptFunding);


                /// Populates "Is project for a grant proposal ?"
                ///              > "Is this application for a UH Infrastructure Grant pilot?"
                ///                  >  "What is the grant?" dropdown.
                /*dropDownSource = cr.ProjectField_cr
                                   .Where(f => f.IsGrant == true && f.IsFundingSource == true
                                                               && (f.Name == "Ola Hawaii"
                                                                || f.Name == "RMATRIX"
                                                                || f.Name == "INBRE"
                                                                // --> (Not a grant source) || f.Name == "Native and Pacific Islands Health Disparities Research"
                                                                // --> (Bioinformatics) || f.Name == "COBRE-Cardiovascular"
                                                                // --> (Bioinformatics) || f.Name == "COBRE-Infectious Diseases"
                                                                // --> (Bioinformatics) || f.Name == "COBRE-Biogenesis Research"
                                                                // --> (Bioinformatics) || f.Name == "P30 UHCC"
                                                                ))
                                   .OrderBy(b => (b.Name == "Ola Hawaii" ? 1 : b.Id))
                                   .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlUHGrant, dropDownSource, String.Empty);*/

                /// Populates "Funding Source > Department Funding" dropdown.
                dropDownSource = cr.JabsomAffil_cr
                                   .Where(f => f.Name == "Obstetrics, Gynecology, and Women's Health"
                                            || f.Name == "School of Nursing & Dental Hygiene"
                                            || f.Id == 96 /*"Other"*/)
                                   .OrderBy(b => b.Id)
                                   .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlDepartmentFunding, dropDownSource, String.Empty);

                if (rqst != null)
                {
                    SetRequest(rqst);
                }

            }


        }

        /// <summary>
        /// Sets client request form items into client request admin review form.
        /// </summary>
        /// <param name="request">Request originating from client.</param>
        private void SetRequest(ClientRequest2_cr request)
        {

            DateTime dt;

            txtFirstName.Value = request.FirstName; //lblFirstName.Text = request.FirstName;
            txtLastName.Value = request.LastName;   //lblLastName.Text = request.LastName;

            ddlDegree.SelectedItem.Text = request.Degree /*> 0 ? request.Degree : string.Empty*/;
            txtDegreeOther.Value = request.DegreeOther;
            ddlPIStatus.SelectedItem.Text = request.InvestStatus;

            txtEmail.Value = request.Email;
            txtPhone.Value = request.Phone;
            txtDept.Value = request.Department;


            if (request.IsJuniorPI.Equals(true))
            {
                chkJuniorPIYes.Checked = true;
            }
            else
            {
                chkJuniorPINo.Checked = true;
            }

            //lblJuniorPI.Text = request.IsJuniorPI.Equals(true) ? "Yes" : "No";

            if (request.HasMentor.Equals(true))
            {
                chkMentorYes.Checked = true;
            }
            else
            {
                chkMentorNo.Checked = true;
            }

            txtMentorFirstName.Value = request.MentorFirstName;
            txtMentorLastName.Value = request.MentorLastName;
            txtMentorEmail.Value = request.MentorEmail;

            txtProjectTitle.Value = request.ProjectTitle;
            txtProjectSummary.Value = request.ProjectSummary;


            BindTable(rptStudyArea, (int)request.StudyAreaBitSum);
            txtStudyAreaOther.Value = request.StudyAreaOther;

            BindTable(rptHealthData, (int)request.HealthDateBitSum);
            txtHealthDataOther.Value = request.HealthDataOther;

            BindTable(rptStudyType, (int)request.StudyTypeBitSum);
            txtStudyTypeOther.Value = request.StudyTypeOther;

            BindTable(rptStudyPopulation, (int)request.StudyPopulationBitSum);
            txtStudyPopulationOther.Value = request.StudyPopulationOther;

            switch (request.IsHealthDisparity)
            {
                case 1: // Yes
                    chkHealthDisparityYes.Checked = true;
                    chkHealthDisparityNo.Checked = false;
                    chkHealthDisparityNA.Checked = false;
                    break;
                case 2: // No
                    chkHealthDisparityYes.Checked = false;
                    chkHealthDisparityNo.Checked = true;
                    chkHealthDisparityNA.Checked = false;
                    break;
                case 3: // N/A
                    chkHealthDisparityYes.Checked = false;
                    chkHealthDisparityNo.Checked = false;
                    chkHealthDisparityNA.Checked = true;
                    break;
                default:
                    chkHealthDisparityYes.Checked = false;
                    chkHealthDisparityNo.Checked = false;
                    chkHealthDisparityNA.Checked = false;
                    break;
            }

            BindTable(rptService, (int)request.ServiceBitSum);
            txtServiceOther.Value = request.ServiceOther;

            if (request.IsPilot.Equals(true))
            {
                chkPilotYes.Checked = true;
            }
            else
            {
                chkPilotNo.Checked = true;
            }


            if (request.IsGrantProposal.Equals(true))
            {
                chkProposalYes.Checked = true;
            }
            else
            {
                chkProposalNo.Checked = true;
            }

            if (request.IsUHGrant.Equals(true))
            {
                chkIsUHPilotGrantYes.Checked = true;
            }
            else
            {
                chkIsUHPilotGrantNo.Checked = true;
            }

            txtGrantProposalFundingAgency.Value = request.GrantProposalFundingAgency;


            BindTable(rptFunding, (int)request.GrantBitSum);
            txtFundingOther.Value = request.GrantOther;
            ddlDepartmentFunding.SelectedValue = request.GrantDepartmentFundingType > 0 ? request.GrantDepartmentFundingType.ToString()
                                                                                        : string.Empty;
            txtDeptFundOth.Value = request.GrantDepartmentFundingOther;

            txtDueDate.Text = DateTime.TryParse(request.DeadLine.ToString(), out dt) ? dt.ToShortDateString() : "";

            ddlBiostat.SelectedValue = request.BiostatId > 0 ? request.BiostatId.ToString() : string.Empty;



            if (request.RequestStatus == "Completed")
            {
                chkCompleted.Checked = true;
                //btnCreateProject.Enabled = false;
                //btnCreateProject.Text = "-- Project Already Created --";


                Project2 project = FindProject(request);
                

                if (project != null)
                {
                    btnCreateProject.Text = "-- Project already created; Project # " + project.Id.ToString() + " --";
                    btnCreateProject.Enabled = true;
                }
                else
                {
                    btnCreateProject.Enabled = false;
                    btnCreateProject.Text = "-- Project already created; search manually --";
                }


            }
            else
            {
                chkCompleted.Checked = false;
                btnCreateProject.Enabled = true;
                btnCreateProject.Text = "Create Project";
            }
            
        }

        /// <summary>
        /// Finds project in the database with the same information as the one in the request.
        /// Returns the project if found, null otherwise.
        /// </summary>
        /// <param name="request">Client request.</param>
        /// <returns>Instance of project.</returns>
        private Project2 FindProject(ClientRequest2_cr request)
        {
            Project2 project = null;
            Invest invest = null;

            // Find link to existing project.
            // --> If a link can be found, then enable the button and link to that project.
            // --> Otherwise, leave disable button w/message "search manually for project".
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                invest = db.Invests.FirstOrDefault(f => f.FirstName == request.FirstName
                                                     && f.LastName == request.LastName);

                if (invest != null)
                {
                    project = db.Project2.FirstOrDefault(f => f.Title == request.ProjectTitle.ToString()
                                                           && f.PIId == invest.Id);
                }

            }

            return project;
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

        /// <summary>
        /// Checks whether or not the bit value exists in the current bit sum calculation.
        /// </summary>
        /// <param name="bitSum">Total bit sum (e.g., [1026](fake) = Ved[2] & Chelu[1024])</param>
        /// <param name="hdnBitValue">Bit Value of current selection (e.g., 1024 = Chelu</param>
        /// <returns>Returns 1 if the bit value is in the bitsum, 0 if not.</returns>
        private bool CheckBitValue(int bitSum, HiddenField hdnBitValue)
        {
            int bitValue = 0;
            Int32.TryParse(hdnBitValue.Value, out bitValue);

            int c = bitSum & bitValue;

            return c == bitValue;
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