using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    public partial class ClientForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }
        }

        private void BindControl()
        {
            DataTable clientRqstTable = GetClientRqstAll();

            rptClientRqst.DataSource = clientRqstTable;
            rptClientRqst.DataBind();

            DataTable surveyTable = GetSurveyAll();
            rptSurvey.DataSource = surveyTable;
            rptSurvey.DataBind();
        }        

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

        private DataTable GetClientRqstAll()
        {
            DataTable dt = new DataTable("clientRqstTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("FirstName", System.Type.GetType("System.String"));
            dt.Columns.Add("LastName", System.Type.GetType("System.String"));
            dt.Columns.Add("ProjectTitle", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateDate", System.Type.GetType("System.String"));
            dt.Columns.Add("Status", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var query = db.ClientRequest
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

        protected void rptClientRqst_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                lblClientRqstId.Text = ((Button)e.CommandSource).CommandArgument;

                int rqstId = 0;
                int.TryParse(lblClientRqstId.Text, out rqstId);

                if (rqstId > 0)
                {
                    ClientRequest rqst = GetClientRequestById(rqstId);

                    if (rqst != null)
                    {
                        BindEditModal(rqst);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "ModalScript", PageUtility.LoadEditScript(true), false);
                    }
                }
            }
        }

        private void BindEditModal(ClientRequest rqst)
        {
            DateTime dt;
            if (rqst != null)
            {
                lblFirstName.Text = rqst.FirstName;
                lblLastName.Text = rqst.LastName;
                lblDegree.Text = rqst.Degree;
                lblEmail.Text = rqst.Email;
                lblPhone.Text = rqst.Phone;
                lblDept.Text = rqst.Department;
                lblInvestStatus.Text = rqst.InvestStatus;
                lblProjectTitle.Text = rqst.ProjectTitle;
                lblProjectSummary.Text = rqst.ProjectSummary;
                lblStudyArea.Text = rqst.StudyArea;
                lblServiceType.Text = rqst.ServiceType;
                lblDueDate.Text = DateTime.TryParse(rqst.DueDate.ToString(), out dt) ? dt.ToShortDateString() : "";
                lblPreferBiostat.Text = rqst.PreferBiostat;

                if (rqst.RequestStatus == "Completed")
                {
                    chkCompleted.Checked = true;
                }
                else
                {
                    chkCompleted.Checked = false;
                }
            }
        }

        private ClientRequest GetClientRequestById(int rqstId)
        {
            ClientRequest myRqst;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                myRqst = db.ClientRequest.FirstOrDefault(t => t.Id == rqstId);

                if (myRqst != null)
                {
                    var studyAreas = db.HiPrograms
                                    .Where(s => s.isStudyArea);

                    StringBuilder sb = new StringBuilder();
                    String[] studyArray = myRqst.StudyArea.Split(';');

                    foreach (string studyCode in studyArray)
                    {
                        int studyId = 0;
                        Int32.TryParse(studyCode, out studyId);

                        if (studyId > 0)
                        {
                            var studyArea = studyAreas.FirstOrDefault(d => d.Id == studyId);

                            if (studyArea != null)
                            {
                                sb.AppendFormat("{0}; ", studyArea.Name);
                            }
                        }
                    }

                    myRqst.StudyArea = sb.ToString();

                    sb.Clear();

                    var serviceTypes = db.ServiceTypes;
                    String[] serviceArray = myRqst.ServiceType.Split(';');
                    foreach (string serviceCode in serviceArray)
                    {
                        int serviceId = 0;
                        Int32.TryParse(serviceCode, out serviceId);

                        if (serviceId > 0)
                        {
                            var serviceType = serviceTypes.FirstOrDefault(d => d.Id == serviceId);

                            if (serviceType != null)
                            {
                                sb.AppendFormat("{0}; ", serviceType.Name);
                            }
                        }
                    }
                    myRqst.ServiceType = sb.ToString();

                }
            }

            return myRqst;
        }
    }
}