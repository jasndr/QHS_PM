using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Guest
{
    public partial class PISurveyForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string surveyId = Request.QueryString["Id"];

            if (!IsPostBack)
            {
                if (surveyId != null)
                {
                    hdnSurveyFormId.Value = surveyId;
                    BindSurvey(surveyId);
                    btnSubmit.Visible = true;
                }
                else
                {
                    btnSubmit.Visible = false;
                }
            }
        }

        private void BindSurvey(string surveyId)
        {
            SurveyForm survey = null;
            List<KeyValuePair<int, int>> questionAnswers = new List<KeyValuePair<int, int>>();
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var questions = db.SurveyQuestions
                                        .OrderBy(q => q.OrderId)
                                        .Select(s => new { s.Id, s.Question })
                                        .ToList();
                gvQuestion.DataSource = questions;
                gvQuestion.DataBind();

                var surveyDb = db.SurveyForms.FirstOrDefault(s => s.Id == surveyId);
                survey = surveyDb;

                var surveyAnswers = db.SurveyFormAnswers
                                    .Where(s => s.FormId == surveyId)
                                    .Select(s => new { s.QuestionId, s.OptionId });
               
                foreach(var sa in surveyAnswers)
                {
                    questionAnswers.Add(new KeyValuePair<int, int>(sa.QuestionId, sa.OptionId));
                }
            }

            if (survey != null && survey.Responded)
            {
                if (questionAnswers.Count > 0)
                {
                    foreach (GridViewRow row in gvQuestion.Rows)
                    {
                        Label lblId = row.FindControl("lblId") as Label;
                        int questionId = 0;
                        Int32.TryParse(lblId.Text, out questionId);

                        if (questionId > 0)
                        {
                            for (int i = 1; i < 9; i++)
                            {
                                string chkName = "chk" + i.ToString();
                                CheckBox chkBox = (row.FindControl(chkName) as CheckBox);
                                if (questionAnswers.Contains(new KeyValuePair<int, int>(questionId, i)))
                                {
                                    chkBox.Checked = true;
                                    continue;
                                }
                            }
                        }
                    }
                }

                txtComments.Text = survey.Comment;

                StringBuilder script = new StringBuilder();

                if (survey.Recommend != null)
                {
                    script.Append("<script>");

                    if ((bool)survey.Recommend)
                    {
                        script.Append("if (!document.getElementById('option1').checked){document.getElementById('option1').checked=true;} ");
                    }
                    else
                    {
                        script.Append("if (!document.getElementById('option2').checked){document.getElementById('option2').checked=true;} ");
                    }

                    script.Append("</script>");
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", script.ToString());
                }

                //DisableUI();
            }
        }

        //private void DisableUI()
        //{
        //    gvQuestion.Enabled = false;
        //    txtComments.Enabled = false;
        //    btnSubmit.Enabled = false;
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string surveyId = Request.QueryString["Id"];

            if (surveyId != null)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    var survey = db.SurveyForms.FirstOrDefault(s => s.Id == surveyId);

                    if (survey != null)
                    {
                        if (survey.Responded)
                        {
                            LoadEditScript();
                        }
                        else
                        {
                            if (SaveSurvey(survey, db))
                            {
                                //SendNotificationEmail(surveyId);
                                Response.Redirect("ThankYou");
                            }

                        }
                    }
                }                
            }
        }

        private bool SaveSurvey(SurveyForm survey, ProjectTrackerContainer db)
        {
            bool saved = false;

            var questionAnswers = new List<KeyValuePair<int, int>>();
            foreach (GridViewRow row in gvQuestion.Rows)
            {
                Label lblId = row.FindControl("lblId") as Label;
                int questionId = 0;
                Int32.TryParse(lblId.Text, out questionId);

                if (questionId > 0)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        string chkName = "chk" + i.ToString();
                        CheckBox chkBox = (row.FindControl(chkName) as CheckBox);
                        if (chkBox.Checked)
                        {
                            questionAnswers.Add(new KeyValuePair<int, int>(questionId, i));
                            continue;
                        }
                    }
                }
            }

            foreach (var qOption in questionAnswers)
            {
                SurveyFormAnswer fa = new SurveyFormAnswer()
                {
                    FormId = survey.Id,
                    QuestionId = qOption.Key,
                    OptionId = qOption.Value
                };

                db.SurveyFormAnswers.Add(fa);
            }

            string recommend = "No";
            if (Request.Form["options"] != null)
            {
                recommend = Request.Form["options"].ToString();
            }

            survey.Recommend = recommend == "Yes" ? true : false;
            survey.Comment = txtComments.Text.Length > 500 ? txtComments.Text.Substring(0, 500) : txtComments.Text;

            survey.Responded = true;
            survey.RespondDate = DateTime.Now;

            try
            {
                db.SaveChanges();
                saved = true;
            }
            catch (Exception ex)
            {
                throw new Exception( "An Error Has Occurred. " + ex.Message);
            }

            return saved;
        }

        private void SaveSurvey(string surveyId)
        {          
            var questionAnswers = new List<KeyValuePair<int, int>>();
            foreach (GridViewRow row in gvQuestion.Rows)
            {
                Label lblId = row.FindControl("lblId") as Label;
                int questionId = 0;
                Int32.TryParse(lblId.Text, out questionId);

                if (questionId > 0)
                {                   
                    for (int i = 1; i < 9; i++)
                    {
                        string chkName = "chk"+i.ToString();
                        CheckBox chkBox = (row.FindControl(chkName) as CheckBox);
                        if (chkBox.Checked)
                        {
                            questionAnswers.Add(new KeyValuePair<int,int>(questionId, i));
                            continue;
                        }
                    }
                }
            }

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                foreach (var qOption in questionAnswers)
                {
                    SurveyFormAnswer fa = new SurveyFormAnswer()
                    {
                        FormId = surveyId,
                        QuestionId = qOption.Key,
                        OptionId = qOption.Value
                    };

                    db.SurveyFormAnswers.Add(fa);
                }

                var survey = db.SurveyForms.FirstOrDefault(s => s.Id == surveyId);
 
                if (survey != null)
                {
                    string recommend = "No";
                    if (Request.Form["options"] != null)
                    {
                        recommend = Request.Form["options"].ToString();
                    }

                    survey.Recommend = recommend == "Yes" ? true : false;
                    survey.Comment = txtComments.Text.Length > 500 ? txtComments.Text.Substring(0, 500) : txtComments.Text;

                    survey.Responded = true;
                    survey.RespondDate = DateTime.Now;
                }

                db.SaveChanges();
            }
        }

        private void LoadEditScript()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#surveyModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ShowModalScript", sb.ToString(), false);
        }

        private void SendNotificationEmail(string surveyId)
        {
            string email = System.Configuration.ConfigurationManager.AppSettings["superAdminEmail"];

            System.Net.Mail.MailAddress destination = new System.Net.Mail.MailAddress(email);

            string subject = String.Format("A survey is responded, id {0}", surveyId);

            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            //caution!!! make sure this is for production 
            if (System.Configuration.ConfigurationManager.AppSettings["isProduction"].Equals("Y"))
            {
                url = System.Configuration.ConfigurationManager.AppSettings["internet"] + "/ProjectForm2";
            }

            string surveyLink = url.Replace("ProjectForm", "Guest/PISurveyForm?Id=" + surveyId);
                      
            string body = string.Format("Please review survey at {0}", surveyLink);

            IdentityMessage im = new IdentityMessage()
            {
                Subject = subject,
                Destination = destination.Address,
                Body = body
            };

            try
            {
                EmailService emailService = new EmailService();
                emailService.Send(im);
            }
            finally 
            {
                Response.Redirect("ThankYou");
            }

        }
        
    }
}