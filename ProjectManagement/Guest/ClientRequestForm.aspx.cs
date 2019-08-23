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
    ///  2019JUL23 - Jason Delos Reyes  -  Added "Study Population" to list of errors as it previously was not part of the checks.
    ///  2019AUG23 - Jason Delos Reyes  -  Added new verbiage to instructions.
    ///                                 -  Added new "confirmation" pop-up modal in order to give users the option to review their 
    ///                                    choices before saving it into the database for review.
    ///                                 -  Added dates of consultation for individuals wanting to present their date/time availabilities
    ///                                    to save time in consultation scheduling.
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
        /// • Checks if validation form fields are valid.
        ///     - If it is not valid, 
        ///       it creates a pop-up message telling which validation has failed.
        ///     - Otherwise, it sends to affirmation screen to verify that the submitted information is correct.
        ///       From here, the user can choose to either go back and edit their choices, or submit to the 
        ///       Biostatistics Core.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {

            string validateForm = ValidateForm();

            if (validateForm.Equals(string.Empty))
            {

                using (ClientRequestTracker cr = new ClientRequestTracker())
                {
                    // Fill out fields before
                    txtFirstName_review.Value = txtFirstName.Value; //lblFirstName.Text = txtFirstName.Value;
                    txtLastName_review.Value = txtLastName.Value; //lblLastName.Text = txtLastName.Value;
                    txtDegree_review.Value = ddlDegree.SelectedItem.Text;//lblDegree.Text = ddlDegree.SelectedItem.Text;
                    if (!txtDegreeOther.Value.Equals(string.Empty))
                    {
                        txtDegree_review.Value = txtDegree_review.Value + " - " + txtDegreeOther.Value;
                    }//if (!txtDegreeOther.Value.Equals(string.Empty)) lblDegree.Text = lblDegree.Text + " - " + txtDegreeOther.Value;
                    txtEmail_review.Value = txtEmail.Value;//lblEmail.Text = txtEmail.Value;
                    txtPhone_review.Value = txtPhone.Value;//lblPhone.Text = txtPhone.Value;
                    txtDept_review.Value = txtDept.Value;//lblDept.Text = txtDept.Value;
                    txtPIStatus_review.Value = ddlPIStatus.SelectedItem.Text;//lblInvestStatus.Text = ddlPIStatus.SelectedItem.Text;
                    chkJuniorPIYes_review.Checked = chkJuniorPIYes.Checked ? true : false;//lblJuniorPI.Text = chkJuniorPIYes.Checked ? "Yes" : "No";
                    chkJuniorPINo_review.Checked = chkJuniorPINo.Checked ? true : false;
                    chkMentorYes_review.Checked = chkMentorYes.Checked ? true : false;//lblMentor.Text = chkMentorYes.Checked ? "Yes" : "No";
                    chkMentorNo_review.Checked = chkMentorNo.Checked ? true : false;
                    if (chkMentorYes.Checked)
                    {
                        divMentor_review.Visible = true;
                        txtMentorFirstName_review.Value = txtMentorFirstName.Value;
                        txtMentorLastName_review.Value = txtMentorLastName.Value;
                        txtMentorEmail_review.Value = txtMentorEmail.Value;
                    }
                    else
                    {
                        divMentor_review.Visible = false;
                    }
                    txtProjectTitle_review.Value = txtProjectTitle.Value;//lblProjectTitle.Text = txtProjectTitle.Value;
                    txtProjectSummary_review.Value = txtProjectSummary.Value;
                    //if (txtProjectSummary.Value.Equals(string.Empty))
                    //{
                    //    lblProjectSummary.Text = "-----";
                    //}
                    //else
                    //{
                    //    lblProjectSummary.Text = txtProjectSummary.Value;
                    //}

                    /// (Study Area)
                    
                    // Populates 'Study Area' checkbox grid.
                    var qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyArea == true).ToList();

                    rptStudyArea_review.DataSource = qProjectField;
                    rptStudyArea_review.DataBind();

                    // Bind checkboxes
                    int studyAreaBitSum = 0;
                    Int32.TryParse(txtStudyAreaBitSum.Value, out studyAreaBitSum);

                    BindTable(rptStudyArea_review, studyAreaBitSum);

                    // Other
                    if (!txtStudyAreaOther.Value.Equals(String.Empty))
                    {
                        divStudyAreaOther_review.Visible = true;
                        txtStudyAreaOther_review.Value = txtStudyAreaOther.Value;
                    } else
                    {
                        divStudyAreaOther_review.Visible = false;
                    }


                    // (Study Area)
                    /*Dictionary<int, string> studyAreas = cr.ProjectField_cr.Where(f => f.IsStudyArea == true)
                                                                           .Select(d => new { d.BitValue, d.Name })
                                                                           .ToDictionary(g => g.BitValue, g => g.Name);

                    foreach (var sa in studyAreas)
                    {
                        
                        int bitValue = sa.Key;
                        int match = studyAreaBitSum & bitValue;
                        if (match == sa.Key)
                        {
                            
                            // check this current checkbox


                            //if (lblStudyArea.Text.Equals(string.Empty) && sa.Value.Equals("Other"))
                            //{
                            //    lblStudyArea.Text = sa.Value + " - " + txtStudyAreaOther.Value;
                            //} else if (!lblStudyArea.Text.Equals(string.Empty) && sa.Value.Equals("Other"))
                            //{
                            //    lblStudyArea.Text = lblStudyArea.Text + ", " + sa.Value + " - " + txtStudyAreaOther.Value;
                            //} else if (lblStudyArea.Text.Equals(string.Empty))
                            //{
                            //    lblStudyArea.Text = sa.Value;
                            //} else
                            //{
                            //    lblStudyArea.Text = lblStudyArea.Text + ", " + sa.Value;
                            //}
                        }
                    }*/

                    /// (Health Data)

                    // Populates 'Health Data' checkbox grid.
                    qProjectField = cr.ProjectField_cr.Where(f => f.IsHealthData == true).ToList();

                    rptHealthData_review.DataSource = qProjectField;
                    rptHealthData_review.DataBind();

                    // Bind checkboxes
                    int healthDataBitSum = 0;
                    Int32.TryParse(txtHealthDataBitSum.Value, out healthDataBitSum);

                    BindTable(rptHealthData_review, healthDataBitSum);

                    // Other
                    if (!txtHealthDataOther.Value.Equals(String.Empty))
                    {
                        divHealthDataOther_review.Visible = true;
                        txtHealthDataOther_review.Value = txtHealthDataOther.Value;
                    }
                    else
                    {
                        divHealthDataOther_review.Visible = false;
                    }

                    // (Study Type)

                    // Populates 'StudyType' checkbox grid.
                    qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyType == true).ToList();

                    rptStudyType_review.DataSource = qProjectField;
                    rptStudyType_review.DataBind();

                    // Bind checkboxes
                    int studyTypeBitSum = 0;
                    Int32.TryParse(txtStudyTypeBitSum.Value, out studyTypeBitSum);

                    BindTable(rptStudyType_review, studyTypeBitSum);

                    // Other
                    if (!txtStudyTypeOther.Value.Equals(String.Empty))
                    {
                        divStudyTypeOther_review.Visible = true;
                        txtStudyTypeOther_review.Value = txtStudyTypeOther.Value;
                    }
                    else
                    {
                        divStudyTypeOther_review.Visible = false;
                    }


                    // (Study Population)

                    // Populates 'StudyType' checkbox grid.
                    qProjectField = cr.ProjectField_cr.Where(f => f.IsStudyPopulation == true).ToList();

                    rptStudyPopulation_review.DataSource = qProjectField;
                    rptStudyPopulation_review.DataBind();

                    // Bind checkboxes
                    int studyPopulationBitSum = 0;
                    Int32.TryParse(txtStudyPopulationBitSum.Value, out studyPopulationBitSum);

                    BindTable(rptStudyPopulation_review, studyPopulationBitSum);

                    // Other
                    if (!txtStudyPopulationOther.Value.Equals(String.Empty))
                    {
                        divStudyPopulationOther_review.Visible = true;
                        txtStudyPopulationOther_review.Value = txtStudyPopulationOther.Value;
                    }
                    else
                    {
                        divStudyPopulationOther_review.Visible = false;
                    }

                    if(chkHealthDisparityYes.Checked || chkHealthDisparityNo.Checked || chkHealthDisparityNA.Checked)
                    {
                        divHealthDisparity_review.Visible = true;
                        chkHealthDisparityYes_review.Checked = chkHealthDisparityYes.Checked;
                        chkHealthDisparityNo_review.Checked = chkHealthDisparityNo.Checked;
                        chkHealthDisparityNA_review.Checked = chkHealthDisparityNA.Checked;
                    }
                    else
                    {
                        divHealthDisparity_review.Visible = false;
                    }

                    // (Service)

                    // Populates 'Service' checkbox grid.
                    qProjectField = cr.ProjectField_cr.Where(f => f.IsService == true).ToList();

                    rptService_review.DataSource = qProjectField;
                    rptService_review.DataBind();

                    // Bind checkboxes
                    int serviceBitSum = 0;
                    Int32.TryParse(txtServiceBitSum.Value, out serviceBitSum);

                    BindTable(rptService_review, serviceBitSum);

                    // Other
                    if (!txtServiceOther.Value.Equals(String.Empty))
                    {
                        divServiceOther_review.Visible = true;
                        txtServiceOther_review.Value = txtServiceOther.Value;
                    }
                    else
                    {
                        divServiceOther_review.Visible = false;
                    }


                    //"Is project a funding infrastructure grant pilot study"
                    chkPilotYes_review.Checked = chkPilotYes.Checked;
                    chkPilotNo_review.Checked = chkPilotNo.Checked;


                    // "Is this project for a grant proposal?"
                    chkProposalYes_review.Checked = chkProposalYes.Checked;
                    chkProposalNo_review.Checked = chkProposalNo.Checked;

                    if (chkProposalYes.Checked)
                    {
                        divIsUHPilotGrant_review.Visible = true;
                        chkIsUHPilotGrantYes_review.Checked = chkIsUHPilotGrantYes.Checked;
                        chkIsUHPilotGrantNo_review.Checked = chkIsUHPilotGrantNo.Checked;
                    }
                    else
                    {
                        divIsUHPilotGrant_review.Visible = false;
                    }

                    if (!txtGrantProposalFundingAgency.Value.Equals(String.Empty))
                    {
                        divGrantProposalFundingAgency_review.Visible = true;
                        txtGrantProposalFundingAgency_review.Value = txtGrantProposalFundingAgency.Value;
                    }
                    else
                    {
                        divGrantProposalFundingAgency_review.Visible = false;
                    }


                    // (Funding Source)
                    // Populates 'FundingSource' checkbox grid.
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

                    BindTable2(qFundingSource, rptFunding_review);

                    // Bind checkboxes
                    long fundingSourceBitSum = 0;
                    Int64.TryParse(txtFundingBitSum.Value, out fundingSourceBitSum);

                    BindTable(rptFunding_review, (int)fundingSourceBitSum);

                    // Other
                    if (!txtFundingOther.Value.Equals(String.Empty))
                    {
                        divFundingOther_review.Visible = true;
                        txtFundingOther_review.Value = txtFundingOther.Value;
                    }
                    else
                    {
                        divFundingOther_review.Visible = false;
                    }

                    // Department Funding
                    if (!ddlDepartmentFunding.SelectedItem.Text.Equals(String.Empty))
                    {
                        divDepartmentFunding_review.Visible = true;
                        txtDepartmentFunding_review.Value = ddlDepartmentFunding.SelectedItem.Text;

                        // MOU - if Dept.Funding == "SONDH"
                        if (ddlDepartmentFunding.SelectedItem.Equals("School of Nursing & Dental Hygiene")) 
                        {
                            divDeptFundMou_review.Visible = true;
                            chkDeptFundMouYes_review.Checked = chkDeptFundMouYes.Checked;
                            chkDeptFundMouNo_review.Checked = chkDeptFundMouNo.Checked;
                        }
                        else
                        {
                            divDeptFundMou_review.Visible = false;
                        }

                        // Other - if Dept. Funding == "Other"
                        if (ddlDepartmentFunding.SelectedItem.Equals("Other")) 
                        {
                            divFundingOther_review.Visible = true;
                            txtFundingOther_review.Value = txtFundingOther.Value;
                        }
                        else
                        {
                            divFundingOther_review.Visible = false;
                        }

                    }
                    else
                    {
                        divDepartmentFunding_review.Visible = false;
                        divDeptFundMou_review.Visible = false;
                        divFundingOther_review.Visible = false;
                    }

                    // Important Deadlines (should be empty if blank)
                    txtDueDate_review.Value = txtDueDate.Text.ToString().Equals(String.Empty) ? "N/A" : txtDueDate.Text.ToString();

                    // Biostatistics Core Facility faculty/staff preference
                    txtBiostat_review.Value = !ddlBiostat.SelectedItem.Text.Equals(String.Empty) ? ddlBiostat.SelectedItem.Text : "N/A" ;

                    // Select up to three dates/times you would like to meet for a consultation
                    txtConsult1_review.Value = txtConsult1.Text.ToString().Equals(String.Empty) ? "N/A" : txtConsult1.Text.ToString();
                    txtConsult2_review.Value = txtConsult2.Text.ToString().Equals(String.Empty) ? "N/A" : txtConsult2.Text.ToString();
                    txtConsult3_review.Value = txtConsult3.Text.ToString().Equals(String.Empty) ? "N/A" : txtConsult3.Text.ToString();


                }
                


                // Open confirmation modal
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(@"<script type='text/javascript'>");
                //sb2.Append("$('#MainContent_lblWarning').text('Please review the following error message:');");
                //sb2.Append("$('#textWarning').append('<span>" + validateForm + "</span>');");
                sb2.Append("$('#btnShowReviewModal').click();");
                sb2.Append(@"</script>");

                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "ShowModalScript", sb2.ToString());

                
            }
            else
            {

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

            int studyTypeBitSum = 0;
            Int32.TryParse(txtStudyTypeBitSum.Value, out studyTypeBitSum);
            if (studyTypeBitSum <= 0)
            {
                validateForm.Append("Study type is required. <br />");
            }

            int studyPopulationBitSum = 0;
            Int32.TryParse(txtStudyPopulationBitSum.Value, out studyPopulationBitSum);
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
                validateForm.Append("Type(s) of support needed is required. <br />");
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
                validateForm.Append(">>> UH Infrastructure Grant specified - Response to follow-up question \"What is the grant name?\" is required. <br />");
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
        /// • Checks if reCaptcha is valid. 
        ///    - If it is valid, it saves the request into the database.
        ///    - Otherwise, it creates a pop-up message saying that captcha verification has failed.
        /// • If both reCaptcha validation succeeds, saves the client form into the database for approval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isValid = IsReCaptchaValid();

            if (!isValid)
            {
                string reCaptchaFailed = "Captcha verification failed! Please resubmit form again.<br />";

                StringBuilder sb2 = new StringBuilder();
                sb2.Append(@"<script type='text/javascript'>");
                sb2.Append("$('#MainContent_lblWarning').text('Please review the following error message:');");
                sb2.Append("$('#textWarning').append('<span>" + reCaptchaFailed + "</span>');");
                //sb2.Append("ShowWarningModal();");
                sb2.Append("$('#btnShowWarningModal').click();");
                sb2.Append(@"</script>");

                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "ShowModalScript", sb2.ToString());
            }
            else
            {
                // Save into database here
                int newRequestId = SaveRequest();
                if (newRequestId > 0)
                {
                    //Direct Notification
                    SendEmailNotification(newRequestId);
                    Response.Redirect(String.Format("MahaloRequest"));
                }


            }


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
            DateTime dt, tc1, tc2, tc3;
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
                Archive = false,
                ConsultDate1 = DateTime.TryParse(txtConsult1.Text, out tc1) ? tc1 : (DateTime?)null,
                ConsultDate2 = DateTime.TryParse(txtConsult2.Text, out tc2) ? tc2 : (DateTime?)null,
                ConsultDate3 = DateTime.TryParse(txtConsult3.Text, out tc3) ? tc3 : (DateTime?)null
            };

            try
            {
                using (ClientRequestTracker cr = new ClientRequestTracker())
                {
                    //Make N/A default if no preferrence.
                    if (rqst.BiostatId == 0)
                    {
                        rqst.BiostatId = 99;
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

    }
}