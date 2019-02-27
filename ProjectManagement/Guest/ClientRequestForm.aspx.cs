using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    ///                                  - Added reCaptcha to increase security and prevent bots from
    ///                                    successfully submitting a form.
    ///  2019FEB26 - Jason Delos Reyes  -  Enhanced "submit" feature so that Google reCaptcha feature has to
    ///                                    be valid before submission, as well as duplicating front-end field
    ///                                    validations to the back end.  A modal pop-up field has been added so
    ///                                    that form users will simply not ignore the errors on the page.
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
            }
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
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = ConfigurationManager.AppSettings["captchaSecretKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
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
                    Response.Redirect("MahaloRequest");
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
                sb2.Append("$('#textWarning').append('<span>"+validateForm+"</span>');");
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
            if (txtDegree.Value.Equals(string.Empty))
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
            if (txtProjectTitle.Value.Equals(string.Empty))
            {
                validateForm.Append("Project Title is required. <br />");
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

            string subject = String.Format("A new request is created, id {0}", newRequestId);

            string url = Request.Url.Scheme + "://" + Request.Url.Authority +
                            Request.ApplicationPath.TrimEnd('/') + "/Admin/ClientForm";

            if (url.IndexOf("?Id") > 0)
            {
                url = url.Substring(0, url.IndexOf("?Id"));
            }

            StringBuilder body = new StringBuilder();
            body.AppendFormat("<p>Request GUID {0}<br /><br />", Guid.NewGuid());
            body.AppendFormat("Please check new request id {0} at {1}", newRequestId, url);
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
            int requestId = 0;
            DateTime dt;
            ClientRequest rqst = new ClientRequest()
            {
                FirstName = Request.Form["txtFirstName"],
                LastName = Request.Form["txtLastName"],
                Degree = Request.Form["txtDegree"],
                Email = Request.Form["txtEmail"],
                Phone = Request.Form["txtPhone"],
                Department = Request.Form["txtDept"],
                InvestStatus = Request.Form["ddlPIStatus"],
                ProjectTitle = Request.Form["txtProjectTitle"],
                ProjectSummary = Request.Form["txtProjectSummary"],
                DueDate = DateTime.TryParse(txtDueDate.Text, out dt) ? dt : (DateTime?)null,
                PreferBiostat = Request.Form["txtPreferBiostat"],
                StudyArea = string.Join("; ", GetCheckedValue(tblStudyArea)),
                ServiceType = string.Join("; ", GetCheckedValue(tblServiceType)),
                Creator = Request.UserHostAddress.ToString(),
                CreationDate = DateTime.Now,
                RequestStatus = "Created",
                ProjectId = -1
            };

            try
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    db.ClientRequest.Add(rqst);
                    db.SaveChanges();

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