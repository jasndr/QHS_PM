using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagement.Guest
{
    /// <summary>
    /// @File: ClientRequestForm.aspx.cs
    /// @FrontEnd: ClientRequestForm.aspx
    /// @Author: Yang Rui
    /// @Summary: Client Request Form of Project Tracking System.
    /// 
    ///           Form to request BQHS services through an online form.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2019FEB25 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///                                 -  Added reCaptcha to increase security and prevent bots from
    ///                                    successfully submitting a form.
    ///  2019FEB26 - Jason Delos Reyes  -  Enhanced "submit" feature so that Google reCaptcha feature has to
    ///                                    be valid before submission, as well as duplicating front-end field
    ///                                    validations to the back end.  A modal pop-up field has been added so
    ///                                    that form users will simply not ignore the errors on the page.
    ///  2019APR08 - Jason Delos Reyes  -  Nearly completed adding front-end of new Client Request form.
    ///                                    Need to add "required" indicator to stop users from entering
    ///                                    missing forms.
    ///  2019APR11 - Jason Delos Reyes  -  Finished front-end and added linking to database. Created ClientRequestTracker.edmx
    ///                                    file to build the "second database" functionality of the system.
    ///  2019MAY13 - Jason Delos Reyes  -  Updated notification email link to link with the proper Client Request review form.
    ///  2019MAY16 - Jason Delos Reyes  -  Replaced reCAPTCHA from v2 to v3 for increased security.
    /// </summary>
    public partial class ClientRequestForm : System.Web.UI.Page
    {
        /// <summary>
        /// Loads page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //FillCapctha();

                BindControl();

            }
        }

        /// <summary>
        /// Populates dropdowns and checkboxes with existing database values.
        /// </summary>
        private void BindControl()
        {
            var dropDownSource = new Dictionary<int, string>();

            using (ClientRequestTracker cr = new ClientRequestTracker())
            {

                /// Populates "Degree" dropdown
                dropDownSource = cr.JabsomAffil_cr
                                   .Where(d => d.Type == "Degree")
                                   .OrderBy(d => d.Name)
                                   .ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlDegree, dropDownSource, "-- Select Degree --");

                /// Populates "Investigator Status" dropdown
                dropDownSource = cr.InvestStatus_cr
                                   .OrderBy(d => d.DisplayOrder)
                                    .ToDictionary(c => c.Id, c => c.StatusValue);

                PageUtility.BindDropDownList(ddlPIStatus, dropDownSource, "-- Select status --");

                /// Populates "Organization" typeable dropdown (if available from database)
                var deptAffil = cr.JabsomAffil_cr
                                  .Where(a => a.Type != "Other" && a.Type != "Degree"
                                                                && a.Type != "Unknown"
                                                                && a.Type != "UHFaculty"
                                                                && a.Name != "Other")
                                  .OrderBy(a => a.Id)
                                  .Select(x => new { Id = x.Id, Name = x.Name });

                textAreaDeptAffil.Value = Newtonsoft.Json.JsonConvert.SerializeObject(deptAffil);

                /// Populates "Study Area" checkbox grid
                var qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyArea == true).ToList();

                rptStudyArea.DataSource = qProjectField;
                rptStudyArea.DataBind();

                /// Populates "Health Data" checkbox grid
                qProjectField = cr.ProjectField_cr.Where(f => f.IsHealthData == true).ToList();
                rptHealthData.DataSource = qProjectField;
                rptHealthData.DataBind();

                /// Populates "Study Type" checkbox grid
                qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyType == true).ToList();
                rptStudyType.DataSource = qProjectField;
                rptStudyType.DataBind();

                /// Populates "Study Population" checkbox grid
                qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyPopulation == true).ToList();
                rptStudyPopulation.DataSource = qProjectField;
                rptStudyPopulation.DataBind();

                /// Populates "Service" checkbox grid
                qProjectField = cr.ProjectField_cr.Where(f => f.IsService == true && f.Name != "Bioinformatics Analysis").ToList();
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
                                                                         && b.Name != "JaNay Wyss"
                                                                         && b.Name != "Meliza Roman"
                                                                         && b.Name != "Masako Matsunaga"
                                                                         && b.Name != "Munirih Taafaki")
                                   .OrderBy(b => b.Name)
                                   .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlBiostat, dropDownSource, String.Empty);


                //var members = db.BioStats
                //                .Where(b => b.EndDate >= DateTime.Now)
                //                .OrderBy(b=>b.Name)
                //                .Select(x => new { Id = x.Id, Name = x.Name });

                //textAreaMembers.Value = Newtonsoft.Json.JsonConvert.SerializeObject(members);

                /// Populates "Grant Status" dropdown
                //var qGrantStatus = new Dictionary<int, string>();
                //qGrantStatus.Add(1, "New - first time submission");
                //qGrantStatus.Add(2, "New - resubmission");
                //qGrantStatus.Add(3, "Completing renewal - research grant application");
                //qGrantStatus.Add(4, "Completing renewal - resubmission");


                //PageUtility.BindDropDownList(ddlGrantStatus, qGrantStatus, "-- Select status --");

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
                                      .ToDictionary(c => c.BitValue, c => c.Name);

                BindTable2(qFundingSource, rptFunding);


                /// Populates Funding Source > Department Funding dropdown.
                dropDownSource = cr.JabsomAffil_cr
                                   .Where(f => f.Name == "Obstetrics, Gynecology, and Women's Health"
                                           || f.Name == "School of Nursing & Dental Hygiene"
                                           || f.Id == 96 /*Other*/)
                                   .OrderBy(b => b.Id)
                                   .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlDepartmentFunding, dropDownSource, String.Empty);

                /// Populates "Is project for a grant proposal ?"
                ///              > "Is this application for a UH Infrastructure Grant pilot?"
                ///                  >  "What is the grant?" dropdown.
                /* dropDownSource = cr.ProjectField_cr
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

        //private void FillCapctha()
        //{
        //    try
        //    {
        //        Random random = new Random();
        //        string combination = "23456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        //        StringBuilder captcha = new StringBuilder();
        //        for (int i = 0; i < 5; i++)
        //            captcha.Append(combination[random.Next(combination.Length)]);
        //        Session["captcha"] = captcha.ToString();
        //        imgCaptcha.ImageUrl = "GenerateCaptcha?" + DateTime.Now.Ticks.ToString();
        //    }
        //    catch
        //    {

        //        throw;
        //    }
        //}

        //protected void btnRefresh_Click(object sender, EventArgs e)
        //{
        //    //FillCapctha();
        //}

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        //public static bool IsCaptchaCodeValid(string captchaCode)
        //{
        //    if (HttpContext.Current.Session["captcha"].ToString() != captchaCode)
        //        return false;
        //    else
        //    {
        //        return true;
        //    }
        //}

        /// <summary>
        /// Returns a true/false value on whether the reCaptcha request has been valid.
        /// </summary>
        /// <returns>Whether or not captcha request is valid.</returns>
        private bool IsReCaptchaValid()
        {
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify");

            req.ProtocolVersion = HttpVersion.Version10;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            string Fdata = string.Format("secret={0}&response={1}",
                new object[]
                {
                    HttpUtility.UrlEncode(ConfigurationManager.AppSettings["captchaSecretKey"]),
                    HttpUtility.UrlEncode(GcaptchaResponse.Value),
                });

            byte[] resData = Encoding.ASCII.GetBytes(Fdata);

            using (Stream rStream = req.GetRequestStream())
            {
                rStream.Write(resData, 0, resData.Length);
            }

            bool result;
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (TextReader readStream = new StreamReader(wResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        JObject jResponse = JObject.Parse(readStream.ReadToEnd());
                        var isSuccess = jResponse.Value<bool>("success");
                        result = (isSuccess) ? true : false;
                    }
                }
            }
            catch
            {
                result = false;
                return result;
            }

            return result;

        }

        /// <summary>
        /// Checks if reCaptcha is valid. 
        ///    - If it is valid, it saves the request into the database.
        ///    - Otherwise, it creates a pop-up message saying that captcha verification has failed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isValid = IsReCaptchaValid();

            string validateForm = ValidateForm();

            if (isValid && validateForm.Equals(string.Empty))
            {
                int newRequestId = SaveRequest();
                if (newRequestId > 0)
                {
                    SendEmailNotification(newRequestId);
                    Response.Redirect(String.Format("MahaloRequest?Id={0}", newRequestId));
                }
            }
            else
            {

                if (!isValid)
                {
                    validateForm = validateForm + "Captcha verification failed! <br />";
                }

                StringBuilder sb2 = new StringBuilder();
                sb2.Append(@"<script type='text/javascript'>");
                sb2.Append("$('#MainContent_lblWarning').text('Please review the following error message:');");
                sb2.Append("$('#textWarning').append('<span>" + validateForm + "</span>');");
                //sb2.Append("ShowWarningModal();");
                sb2.Append("$('#btnShowWarningModal').click();");
                sb2.Append(@"</script>");

                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "ShowModalScript", sb2.ToString());

                //lblRecaptchaMessage.InnerHtml = "<span style='color: red'>Captcha verification failed!</span>";
            }

        }

        /// <summary>
        /// Checks the following fields to see whether or not they have been filled out on the form,
        /// to be entered onto the database:
        /// - First Name
        /// - Last Name
        /// - Degree
        /// - Email
        /// - Phone Number
        /// - Department/Organization
        /// - Project Title
        /// * All other fields are optional.
        /// </summary>
        /// <returns>Error message to print out if fields are empty.</returns>
        private string ValidateForm()
        {
            System.Text.StringBuilder validateForm = new System.Text.StringBuilder();

            if (txtFirstName.Value.Equals(string.Empty))
            {
                validateForm.Append("First name is required. <br />");
            }
            if (txtLastName.Value.Equals(string.Empty))
            {
                validateForm.Append("Last name is required. <br />");
            }
            if (ddlDegree.SelectedValue.Equals(string.Empty))
            {
                validateForm.Append("Degree is required. <br />");
            }
            if (txtEmail.Value.Equals(string.Empty))
            {
                validateForm.Append("Email is required. <br />");
            }
            if (txtPhone.Value.Equals(string.Empty))
            {
                validateForm.Append("Phone Number is required. <br />");
            }
            if (txtDept.Value.Equals(string.Empty))
            {
                validateForm.Append("Department/Organization is required. <br />");
            }
            if (ddlPIStatus.SelectedValue.Equals(string.Empty))
            {
                validateForm.Append("Investigator status is required. <br />");
            }
            if (!chkJuniorPIYes.Checked && !chkJuniorPINo.Checked)
            {
                validateForm.Append("Response to question \"Is PI a junior investigator?\" is required. <br />");
            }
            if (!chkMentorYes.Checked && !chkMentorNo.Checked)
            {
                validateForm.Append("Response to question \"Does PI have mentor?\" is required. <br />");
            }
            if (chkMentorYes.Checked && (txtMentorFirstName.Value.Equals(string.Empty)
                                      || txtMentorLastName.Value.Equals(string.Empty)
                                      || txtMentorEmail.Value.Equals(string.Empty)))
            {
                validateForm.Append(">>> Mentor specified - please complete mentor information. <br />");
            }
            if (txtProjectTitle.Value.Equals(string.Empty))
            {
                validateForm.Append("Project Title is required. <br />");
            }
            if (txtProjectTitle.Value.Length > 255)
            {
                validateForm.Append("Project title is too long. Limit is 255 characters. <br />");
            }
            if (txtProjectSummary.Value.Length > 4000)
            {
                validateForm.Append("Project summary is too long. Limit is 4000 characters. <br />");
            }

            int studyAreaBitSum = 0;
            Int32.TryParse(txtStudyAreaBitSum.Value, out studyAreaBitSum);
            if (studyAreaBitSum <= 0)
            {
                validateForm.Append("Study area is required. <br />");
            }

            int healthDataBitSum = 0;
            Int32.TryParse(txtHealthDataBitSum.Value, out healthDataBitSum);
            if (healthDataBitSum <= 0)
            {
                validateForm.Append("Health data is required. <br />");
            }

            int studyPopulationBitSum = 0;
            Int32.TryParse(txtStudyTypeBitSum.Value, out studyPopulationBitSum);
            if (studyPopulationBitSum <= 0)
            {
                validateForm.Append("Study population is required. <br />");
            }
            else
            {
                if ((studyPopulationBitSum != 32 && studyPopulationBitSum > 0) && (!chkHealthDisparityYes.Checked && !chkHealthDisparityNo.Checked && !chkHealthDisparityNA.Checked))
                {
                    validateForm.Append("Study population >> Health disparity is required since study population is specified. \\n");
                }
            }

            int serviceBitSum = 0;
            Int32.TryParse(txtServiceBitSum.Value, out serviceBitSum);
            if (serviceBitSum <= 0)
            {
                validateForm.Append("Service is required. <br />");
            }

            if (!chkPilotYes.Checked && !chkPilotNo.Checked)
            {
                validateForm.Append("Response to question \"Is project a funded infrastructure grant pilot study?\" is required. <br />");
            }

            if (!chkProposalYes.Checked && !chkProposalNo.Checked)
            {
                validateForm.Append("Response to question \"Is this project for a grant proposal?\" is required. <br />");
            }

            if (chkProposalYes.Checked && !chkIsUHPilotGrantYes.Checked && !chkIsUHPilotGrantNo.Checked)
            {
                validateForm.Append(">>> Grant Proposal specified - Response to follow-up question \"Is this application to a pilot program of a UH infrastructure grant?\" is required. <br />");
            }

            if ((chkIsUHPilotGrantYes.Checked || chkIsUHPilotGrantNo.Checked) && txtGrantProposalFundingAgency.Value.Equals(string.Empty))
            {
                validateForm.Append(">>> UH Infrastructure Grant specified - Response to follow-up question \"What is the funding agency?\" is required. <br />");
            }

            int fundingBitSum = 0;
            Int32.TryParse(txtFundingBitSum.Value, out fundingBitSum);
            if (fundingBitSum <= 0)
            {
                validateForm.Append("Funding source is required. <br />");
            }

            return validateForm.ToString();
        }

        /// <summary>
        /// Sends email notification to the admin team when a client request form has been submitted
        /// by an external user (i.e., member of the public, collaborators, etc.).
        /// </summary>
        /// <param name="newRequestId">Request Id that has been generated
        ///                            for the specific entry form by saving into the database.</param>
        private void SendEmailNotification(int newRequestId)
        {
            string email = System.Configuration.ConfigurationManager.AppSettings["trackingEmail"];

            MailAddress destination = new MailAddress(email);

            string subject = String.Format("A new client request has been created, id {0}", newRequestId);

            string url = Request.Url.Scheme + "://" + Request.Url.Authority +
                            Request.ApplicationPath.TrimEnd('/') + "/Admin/ClientForm";

            if (url.IndexOf("?ClientRequestId") > 0)
            {
                url = url.Substring(0, url.IndexOf("?ClientRequestId"));
            }

            StringBuilder body = new StringBuilder();
            body.AppendFormat("<p>Request GUID {0}<br /><br />", Guid.NewGuid());
            body.AppendFormat("Please check new client request, id {0} at {1}", newRequestId, url);
            body.AppendFormat("?ClientRequestId={0}</p>", newRequestId);
            body.AppendLine();

            IdentityMessage im = new IdentityMessage()
            {
                Subject = subject,
                Destination = destination.Address,
                Body = body.ToString()
            };

            try
            {
                EmailService emailService = new EmailService();
                emailService.Send(im);
            }
            finally { }
        }

        /// <summary>
        /// Saves the form items into the database.
        /// </summary>
        /// <returns>Request Id generated by the database for this form.</returns>
        private int SaveRequest()
        {
            int requestId = 0, /*uhGrantId = 0,*/ biostatId = 0, grantDepartmentFundingType = 0;
            long studyAreaBitSum = 0, healthDataBitSum = 0, studyTypeBitSum = 0, studyPopulationBitSum = 0,
                 serviceBitSum = 0, grantBitSum = 0;
            DateTime dt;
            ClientRequest2_cr rqst = new ClientRequest2_cr()
            {
                FirstName = Request.Form["txtFirstName"],
                LastName = Request.Form["txtLastName"],
                Degree = ddlDegree.SelectedItem.Text,//Request.Form["ddlDegree"],
                DegreeOther = Request.Form["txtDegreeOther"],
                Email = Request.Form["txtEmail"],
                Phone = Request.Form["txtPhone"],
                Department = Request.Form["txtDept"],
                InvestStatus = ddlPIStatus.SelectedItem.Text,//Request.Form["ddlPIStatus"],
                InvestStatusOther = Request.Form["txtPIStatusOther"],
                IsJuniorPI = chkJuniorPIYes.Checked ? true : false,
                HasMentor = chkMentorYes.Checked ? true : false,
                MentorFirstName = Request.Form["txtMentorFirstName"],
                MentorLastName = Request.Form["txtMentorLastName"],
                MentorEmail = Request.Form["txtMentorEmail"],
                ProjectTitle = Request.Form["txtProjectTitle"],
                ProjectSummary = Request.Form["txtProjectSummary"],
                StudyAreaBitSum = Int64.TryParse(txtStudyAreaBitSum.Value, out studyAreaBitSum) ? studyAreaBitSum : 0,
                StudyAreaOther = Request.Form["txtStudyAreaOther"],
                HealthDateBitSum = Int64.TryParse(txtHealthDataBitSum.Value, out healthDataBitSum) ? healthDataBitSum : 0,
                HealthDataOther = Request.Form["txtHealthDataOther"],
                StudyTypeBitSum = Int64.TryParse(txtStudyTypeBitSum.Value, out studyTypeBitSum) ? studyTypeBitSum : 0,
                StudyTypeOther = Request.Form["txtStudyTypeOther"],
                StudyPopulationBitSum = Int64.TryParse(txtStudyPopulationBitSum.Value, out studyPopulationBitSum) ? studyPopulationBitSum : 0,
                StudyPopulationOther = Request.Form["txtStudyPopulationOther"],
                IsHealthDisparity = chkHealthDisparityYes.Checked ? (byte)1 : chkHealthDisparityNo.Checked ? (byte)2 : chkHealthDisparityNA.Checked ? (byte)3 : (byte)0,
                ServiceBitSum = Int64.TryParse(txtServiceBitSum.Value, out serviceBitSum) ? serviceBitSum : 0,
                IsPilot = chkPilotYes.Checked ? true : false,
                IsGrantProposal = chkProposalYes.Checked ? true : false,
                IsUHGrant = chkIsUHPilotGrantYes.Checked ? true : false,
                UHGrantName = /*ddlUHGrant.SelectedItem.Text*/null,
                GrantProposalFundingAgency = Request.Form["txtGrantProposalFundingAgency"],
                GrantBitSum = Int64.TryParse(txtFundingBitSum.Value, out grantBitSum) ? grantBitSum : 0,
                GrantOther = Request.Form["txtFundingOther"],
                GrantDepartmentFundingType = Int32.TryParse(ddlDepartmentFunding.SelectedValue, out grantDepartmentFundingType) ? grantDepartmentFundingType : 0,
                GrantDepartmentFundingOther = Request.Form["txtDeptFundOth"],
                DeadLine = DateTime.TryParse(txtDueDate.Text, out dt) ? dt : (DateTime?)null,
                BiostatId = Int32.TryParse(ddlBiostat.SelectedItem.Value, out biostatId) ? biostatId : 0,
                Creator = Request.UserHostAddress.ToString(),
                CreationDate = DateTime.Now,
                RequestStatus = "Created",
                Archive = false
            };

            try
            {
                using (ClientRequestTracker cr = new ClientRequestTracker())
                {
                    //Make Dr. John Chen default if no preferrence.
                    if (rqst.BiostatId == 0)
                    {
                        rqst.BiostatId = cr.BioStat_cr.FirstOrDefault(f => f.Name == "John Chen").Id;
                    }

                    cr.ClientRequest2_cr.Add(rqst);
                    cr.SaveChanges();

                    requestId = rqst.Id;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return requestId;
        }

        /// <summary>
        /// Given a table of checkboxes, combines all the choices into a single string to be
        /// recorded into the database.
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        private List<int> GetCheckedValue(HtmlTable tbl)
        {
            List<int> chkList = new List<int>();

            foreach (HtmlTableRow row in tbl.Rows)
            {
                foreach (HtmlTableCell cell in row.Cells)
                {
                    for (int i = 0; i < cell.Controls.Count; i++)
                    {
                        if (cell.Controls[i].GetType() == typeof(HtmlInputCheckBox))
                        {
                            var checkBox = (HtmlInputCheckBox)cell.Controls[i];

                            int output = 0;
                            if (checkBox.Checked && Int32.TryParse(checkBox.Value, out output))
                            {
                                chkList.Add(output);
                            }
                        }
                    }
                }
            }

            return chkList;
        }
    }
}