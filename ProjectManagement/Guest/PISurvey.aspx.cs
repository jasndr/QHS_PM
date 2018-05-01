using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Guest
{
    /// <summary>
    /// @File: PISurvey.aspx.cs
    /// @Author: Yang Rui
    /// @Summary: Old Survey Form of Project Tracking System.
    /// 
    ///           NOTE: This form is no longer / currently not in use.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018APR16 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    /// </summary>
    public partial class PISurvey : System.Web.UI.Page
    {
        /// <summary>
        /// Prepares the survey form and its questions.  It will also 
        /// prepopulate survey if there is previously entered data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string surveyId = Request.QueryString["SurveyId"];

            if (!Page.IsPostBack)
            {
                LoadSurvey();

                if (surveyId != null)
                {
                    BindSurveyData(surveyId);
                    hdnSurveyFormId.Value = surveyId;
                }
            }
        }

        /// <summary>
        /// Based on the provided survey ID, populates survey form with responses stored in the
        /// database that has already been provided by the investigator being surveyed for feedback.
        /// </summary>
        /// <param name="surveyId">Provided survey ID.</param>
        private void BindSurveyData(string surveyId)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var survey = db.SurveyForms.FirstOrDefault(s => s.Id == surveyId);

                if (survey != null)
                {
                    DateTime dtOutput;
                    //decimal dmOutput;
                    lblBiostat.Text = survey.LeadBiostat;
                    lblPhdHours.Text = survey.PhdHours.ToString();
                    lblMsHours.Text = survey.MsHours.ToString();
                    lblInitialDate.Text = DateTime.TryParse(survey.ProjectInitialDate.ToString(), out dtOutput) ? dtOutput.ToShortDateString() : string.Empty;
                    lblCompletionDate.Text = DateTime.TryParse(survey.ProjectCompletionDate.ToString(), out dtOutput) ? dtOutput.ToShortDateString() : string.Empty;
                    lblProjectTitle.Text = survey.ProjectTitle;
                    txtComments.Text = survey.Comment;

                    if (survey.Responded)
                    {
                        //var surveyFormAnswer = db.SurveyFormAnswers.Where(s => s.FormId == survey.Id).ToList();
                        var surveyAnswers = db.SurveyFormAnswers
                                            .Select(a => new {a.FormId, a.QuestionId, a.OptionId })
                                            .Where(f => f.FormId == survey.Id)
                                            .AsEnumerable()
                                            .Select(a => new KeyValuePair<int, int>(a.QuestionId, a.OptionId))                                            
                                            .ToList();

                        if (surveyAnswers.Count > 0 && dlQuestions.Items.Count > 0)
                        {
                            foreach (DataListItem li in dlQuestions.Items)
                            {
                                RadioButtonList rdList = li.FindControl("dtlAnswers") as RadioButtonList;
                                
                                int questionId = 0;
                                HiddenField hdnQuestionId = (HiddenField)li.FindControl("hdnQuestionId");
                                Int32.TryParse(hdnQuestionId.Value, out questionId);

                                if (questionId > 0)
                                {
                                    var surveyAnswer = surveyAnswers.FirstOrDefault(s => s.Key == questionId);

                                    if (surveyAnswer.Key > 0)
                                    {
                                        foreach (ListItem item in rdList.Items)
                                        {
                                            if (item.Value == surveyAnswer.Value.ToString())
                                            {
                                                item.Selected = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        btnSubmit.Visible = false;
                    }
                }

            }
        }

        /// <summary>
        /// Loads the current survey questions to the PI Survey Form.
        /// </summary>
        private void LoadSurvey()
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var surveyQuestions = db.SurveyQuestions.ToList();

                dlQuestions.DataSource = surveyQuestions;
                dlQuestions.DataBind();
            }
        }

        protected void dlQuestions_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            DataListItem drv = e.Item.DataItem as DataListItem;
            RadioButtonList RadioButtonList1 = (RadioButtonList)e.Item.FindControl("dtlAnswers");
            HiddenField hdnQuestionsid = (HiddenField)e.Item.FindControl("hdnQuestionId");

            int questionId = 0;
            Int32.TryParse(hdnQuestionsid.Value, out questionId);

            if (questionId > 0)
            {
                //DataSet ds = LoadQuestionOptionById(questionId);
                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    RadioButtonList1.DataSource = ds;
                //    RadioButtonList1.DataTextField = "question_option";
                //    RadioButtonList1.DataValueField = "question_id";
                //    RadioButtonList1.DataBind();
                //}

                RadioButtonList1.DataSource = LoadQuestionOptionById(questionId);
                RadioButtonList1.DataTextField = "QuestionOption";
                RadioButtonList1.DataValueField = "Id";
                RadioButtonList1.DataBind();
            }
        }

        private List<SurveyQuestionOption> LoadQuestionOptionById(int questionId)
        {
            List<SurveyQuestionOption> lst = new List<SurveyQuestionOption>();
            
            //using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            //{
            //    var questionOptions = db.SurveyQuestionOptions.Where(s => s.QuestionId == questionId).OrderBy(o => o.OrderId).ToList();

            //    if (questionOptions != null)
            //    {
            //        lst = questionOptions;
            //    }
            //}

            return lst;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (dlQuestions.Items.Count > 0)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    foreach (DataListItem li in dlQuestions.Items)
                    {
                        RadioButtonList rdList = li.FindControl("dtlAnswers") as RadioButtonList;
                        HiddenField hdnQuestionId = (HiddenField)li.FindControl("hdnQuestionId");
                        foreach (ListItem answer in rdList.Items)
                        {
                            bool isSelected = answer.Selected;
                            if (isSelected)
                            {
                                int slval = Convert.ToInt32(answer.Value);
                                SaveSurveyData(db, hdnSurveyFormId.Value, Convert.ToInt32(hdnQuestionId.Value), Convert.ToInt32(slval));
                            }
                        }
                    }
                    
                    var survey = db.SurveyForms.FirstOrDefault(s => s.Id == hdnSurveyFormId.Value);
                    if(survey != null)
                    {
                        survey.Responded = true;
                        survey.RespondDate = DateTime.Now;
                        survey.Comment = txtComments.Text;
                    }

                    db.SaveChanges();
                }
            }

            Response.Redirect("ThankYou");
        }

        private void SaveSurveyData(ProjectTrackerContainer db, string surveyFormId, int questionId, int optionId)
        {
            SurveyFormAnswer sfa = new SurveyFormAnswer()
            {
                FormId = surveyFormId,
                QuestionId = questionId,
                OptionId = optionId
            };

            db.SurveyFormAnswers.Add(sfa);
        }

    }
}