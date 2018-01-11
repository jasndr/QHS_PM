using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ClientRequestForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCapctha();
            }
        }

        private void FillCapctha()
        {
            try
            {
                Random random = new Random();
                string combination = "23456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                StringBuilder captcha = new StringBuilder();
                for (int i = 0; i < 5; i++)
                    captcha.Append(combination[random.Next(combination.Length)]);
                Session["captcha"] = captcha.ToString();
                imgCaptcha.ImageUrl = "GenerateCaptcha?" + DateTime.Now.Ticks.ToString();
            }
            catch
            {

                throw;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            FillCapctha();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static bool IsCaptchaCodeValid(string captchaCode)
        {
            if (HttpContext.Current.Session["captcha"].ToString() != captchaCode)
                return false;
            else
            {
                return true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int newRequestId = SaveRequest();
            if ( newRequestId> 0)
            {
                SendEmailNotification(newRequestId);
                Response.Redirect("MahaloRequest");
            }           
        }

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