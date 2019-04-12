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
                    lblFirstName.Text = rqst.FirstName;
                    lblLastName.Text = rqst.LastName;
                    lblDegree.Text = rqst.Degree.Equals("Other") ? "Other - " + rqst.DegreeOther : rqst.Degree;
                    lblEmail.Text = rqst.Email;
                    lblPhone.Text = rqst.Phone;
                    lblDept.Text = rqst.Department;
                    lblInvestStatus.Text = rqst.InvestStatus.Equals("Other") ? "Other - " + rqst.InvestStatusOther : rqst.InvestStatus;
                    lblJuniorPI.Text = rqst.IsJuniorPI.Equals(true) ? "Yes" : "No";
                    lblMentor.Text = rqst.HasMentor.Equals(true) ? "Yes - " + rqst.MentorFirstName + " " + rqst.MentorLastName + " (" + rqst.MentorEmail + ")" : "No";
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
               

                    string biostatName = cr.BioStat_cr.FirstOrDefault(f=>f.Id == rqst.BiostatId).Name;
                    lblBiostat.Text = biostatName;
     

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

                //if (myRqst != null)
                //{
                //    var studyAreas = cr.ProjectField_cr.Where(s => s.IsStudyArea == true);

                //    //db.HiPrograms
                //    ///Where(s => s.isStudyArea);

                //    //StringBuilder sb = new StringBuilder();
                //    //String[] studyArray = myRqst.StudyAreaBitSum.StudyArea.Split(';');

                //    //foreach (string studyCode in studyArray)
                //    //{
                //    //    int studyId = 0;
                //    //    Int32.TryParse(studyCode, out studyId);

                //    //    if (studyId > 0)
                //    //    {
                //    //        var studyArea = studyAreas.FirstOrDefault(d => d.Id == studyId);

                //    //        if (studyArea != null)
                //    //        {
                //    //            sb.AppendFormat("{0}; ", studyArea.Name);
                //    //        }
                //    //    }
                //    //}

                //    //myRqst.StudyArea = sb.ToString();

                //    //sb.Clear();

                //    //var serviceTypes = db.ServiceTypes;
                //    //String[] serviceArray = myRqst.ServiceType.Split(';');
                //    //foreach (string serviceCode in serviceArray)
                //    //{
                //    //    int serviceId = 0;
                //    //    Int32.TryParse(serviceCode, out serviceId);

                //    //    if (serviceId > 0)
                //    //    {
                //    //        var serviceType = serviceTypes.FirstOrDefault(d => d.Id == serviceId);

                //    //        if (serviceType != null)
                //    //        {
                //    //            sb.AppendFormat("{0}; ", serviceType.Name);
                //    //        }
                //    //    }
                //    //}
                //    //myRqst.ServiceType = sb.ToString();

                //}
            }

            return myRqst;
        }
    }
}