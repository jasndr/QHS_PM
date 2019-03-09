using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Vml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ProjectManagement.Report
{

    /// <summary>
    /// @File: FilteredReport.aspx.cs
    /// @Frontend: FilteredReport.aspx
    /// @Author: Jason Delos Reyes
    /// @Summary: Project Report
    /// 
    ///           Report (of projects, for now) that is able to be filtered by pre-selected variables (3 variables for now).
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2019JAN23 - Jason Delos Reyes  -  Begin creating report.
    ///  2019JAN29 - Jason Delos Reyes  -  Finalizing first level of filters on the front-end.
    ///  2019JAN30 - Jason Delos Reyes  -  Finalized initial phase of front end for dynamic filtered project report. 
    ///  2019FEB27 - Jason Delos Reyes  -  Created "Rpt_Project_Summary_Filtered" report to align with back-end for 
    ///                                    report.
    ///  2019MAR06 - Jason Delos Reyes  -  Edited "Rpt_Project_Summary_Filtered" report so that the total row produces
    ///                                    more total numbers for reporting purposes.
    /// </summary>
    public partial class FilteredReport : System.Web.UI.Page
    {
        protected string ReportType { get; set; }
        protected string FromDate { get; set; }
        protected string ToDate { get; set; }

        /// <summary>
        /// Prepares initial page load for use.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                using (ProjectTrackerContainer dbContext = new ProjectTrackerContainer())
                {
                    var dropDownSource = new Dictionary<int, string>();

                    var query = dbContext.BioStats.Where(b => b.Id > 0 && b.Id != 99);

                    // Bind QHS Faculty/Staff Dropdown
                    dropDownSource = query
                                     .OrderBy(b => b.Name)
                                     .ToDictionary(b=>b.Id, b=>b.Name);

                    PageUtility.BindDropDownList(ddlBiostat, dropDownSource, " --- Select --- ");

                    // Bind Filters
                    var dropDownSource2 = new Dictionary<int, string>();
                    dropDownSource2.Add(1, "Investigator");
                    dropDownSource2.Add(2, "Project Type");
                    dropDownSource2.Add(3, "Project Title");
                    dropDownSource2.Add(4, "Project Summary");
                    //dropDownSource2.Add(5, "Project Deadline");
                    dropDownSource2.Add(6, "Study Area");
                    dropDownSource2.Add(7, "Health Data");
                    dropDownSource2.Add(8, "Study Type");
                    dropDownSource2.Add(9, "Study Population");
                    dropDownSource2.Add(10, "Service Type");
                    dropDownSource2.Add(11, "Credit To");
                    dropDownSource2.Add(12, "Is PI a junior investigator?");
                    dropDownSource2.Add(13, "Does PI have mentor?");
                    dropDownSource2.Add(14, "Is project internal study?");
                    dropDownSource2.Add(15, "Is project an infrastructure grant study?");
                    dropDownSource2.Add(16, "Is paying project?");
                    dropDownSource2.Add(17, "Funding Source");
                    dropDownSource2.Add(18, "Acknowledgements");
                    dropDownSource2.Add(19, "Request for resources");

                    PageUtility.BindDropDownList(ddlFilter1, dropDownSource2, " -- Filter 1 -- ");
                    PageUtility.BindDropDownList(ddlFilter2, dropDownSource2, " -- Filter 2 -- ");
                    PageUtility.BindDropDownList(ddlFilter3, dropDownSource2, " -- Filter 3 -- ");

                    textfieldFilter1.Visible = false;
                    textfieldFilter2.Visible = false;
                    textfieldFilter3.Visible = false;

                    dropdownFilter1.Visible = false;
                    dropdownFilter2.Visible = false;
                    dropdownFilter3.Visible = false;

                    checkboxesFilter1.Visible = false;
                    checkboxesFilter2.Visible = false;
                    checkboxesFilter3.Visible = false;

                    chkFilter1Checkbox.Items.Clear();
                    chkFilter2Checkbox.Items.Clear();
                    chkFilter3Checkbox.Items.Clear();

                    //dropDownSource = query
                    //                .Where(b => b.Type == "phd")
                    //                .OrderByDescending(b => b.EndDate)
                    //                .ThenBy(b => b.Name)
                    //                .ToDictionary(b => b.Id, b => b.Name );

                    //PageUtility.BindDropDownList(ddlPhd, dropDownSource, " --- Select --- ");

                    //dropDownSource = query
                    //                .Where(b => b.Type == "master")
                    //                .OrderByDescending(b => b.EndDate)
                    //                .ThenBy(b => b.Name)
                    //                .ToDictionary(b => b.Id, b => b.Name);

                    //PageUtility.BindDropDownList(ddlMs, dropDownSource, " --- Select --- ");

                    //dropDownSource = dbContext.InvestStatus
                    //                .OrderBy(d => d.DisplayOrder)
                    //                .ToDictionary(b => b.Id, b => b.StatusValue);

                    //PageUtility.BindDropDownList(ddlPIStatus, dropDownSource, " --- Select --- ");

                    //dropDownSource = dbContext.ProjectField
                    //                .Where(g => g.GroupName == "HealthData")
                    //                .OrderBy(d => d.DisplayOrder)
                    //                .ToDictionary(b => b.BitValue, b => b.Name);

                    //PageUtility.BindDropDownList(ddlHealthData, dropDownSource, " --- Select --- ");

                    //dropDownSource = dbContext.ProjectField
                    //                .Where(g => g.GroupName == "Grant")
                    //                .OrderBy(d => d.DisplayOrder)
                    //                .ToDictionary(b => b.BitValue, b => b.Name);

                    //PageUtility.BindDropDownList(ddlGrant, dropDownSource, " --- Select --- ");

                    //var affil = dbContext.JabsomAffils
                    //            .Where(a => a.Type != "Other" && a.Type != "Degree" && a.Type != "Unknown" && a.Name != "Other")
                    //            .OrderBy(a => a.Id)
                    //            .Select(x => new { Id = x.Id, Name = x.Name });

                    //textAreaAffil.Value = Newtonsoft.Json.JsonConvert.SerializeObject(affil);

                    var projects = dbContext.Project2
                                .Where(p=>p.Id > 0)
                                .OrderByDescending(p => p.Id)
                                .Select(x => new { Id = x.Id, Title = x.Title });

                    textAreaProj.Value = Newtonsoft.Json.JsonConvert.SerializeObject(projects);

                    var piName = dbContext.Invests
                        .Where(i => i.Id > 0)
                        .Select(x => new { Id = x.Id, Name = x.FirstName + " " + x.LastName });

                    textAreaPI.Value = Newtonsoft.Json.JsonConvert.SerializeObject(piName);
                }
            }
            
        }

        /// <summary>
        /// Produces the report in the web form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            divProject.Visible = true;

            DataTable dt = GetProjectTable();

            rptProjectSummary.DataSource = dt;
            rptProjectSummary.DataBind();

            hdnRowCount.Value = dt.Rows.Count.ToString();

            //Control footerTemplate = rptProjectSummary.Controls[rptProjectSummary.Controls.Count - 1].Controls[0];
            //Label lblFooter = footerTemplate.FindControl("lblTotal") as Label;
            //lblFooter.Text = hdnRowCount.Value + " Project(s)";

            //decimal phdHrs = 0.0m, msHrs = 0.0m;
            //foreach (DataRow row in dt.Rows)
            //{
            //    //phdHrs += Convert.ToDecimal(row["PhdHrs"].ToString());
            //    //msHrs += Convert.ToDecimal(row["MsHrs"].ToString());
            //}

            //Label lblPhdHrs = footerTemplate.FindControl("lblPhdHrs") as Label;
            //lblPhdHrs.Text = phdHrs.ToString();

            //Label lblMsHrs = footerTemplate.FindControl("lblMsHrs") as Label;
            //lblMsHrs.Text = msHrs.ToString();
        }

        /// <summary>
        /// Produces the Excel file of the report for download into Excel format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            
            DataTable dt = GetProjectTable();
            dt.TableName = "Project";

            //DataRow dr = dt.NewRow();

            //dr[1] = "Total";
            //dr[2] = dt.Rows.Count + " Project(s)";

            //decimal phdHrs = 0.0m, msHrs = 0.0m;
            //foreach (DataRow row in dt.Rows)
            //{
            //    phdHrs += Convert.ToDecimal(row["PhdHrs"].ToString());
            //    msHrs += Convert.ToDecimal(row["MsHrs"].ToString());
            //}

            //dr[16] = phdHrs;
            //dr[17] = msHrs;

            //dt.Rows.Add(dr);

            string reportHeader = "Project Report - " + ReportType + " - from " + FromDate + " to " + ToDate;
            string fileName = "Project_Report_-_" + ReportType + "_-_from_" + FromDate + "_to_" + ToDate;

            FileExport fileExport = new FileExport(this.Response);
            fileExport.ExcelExport2(dt, fileName, reportHeader);
            //fileExport.ExcelExport(dt, "Project");
        }

        /// <summary>
        /// Obtains the project information for the table.
        /// </summary>
        /// <returns></returns>
        private DataTable GetProjectTable()
        {
            DataTable dt = new DataTable("tblProject");

            

            // Add PI Name to Report Type (Not necessary).

            //if (txtPIName.Value != string.Empty)
            //{
            //    Int32.TryParse(txtPIId.Value, out piId);
            //    ReportType = ReportType + " " + txtPIName.Value;
            //}

            //if (txtAffiliation.Value != string.Empty)
            //{
            //    Int32.TryParse(txtAffilId.Value, out affilId);
            //    ReportType = ReportType + " " + txtAffiliation.Value;
            //}



            //Int32.TryParse(ddlPIStatus.SelectedValue, out piStatusId);

            //int biostatId = 0, healthValue = 0, grantValue = 0, isProject = 1, isBiostat = 1, creditTo = 1;

            //Int32.TryParse(ddlPhd.SelectedValue, out phdId);
            //Int32.TryParse(ddlMs.SelectedValue, out msId);
            //Int32.TryParse(ddlHealthData.SelectedValue, out healthValue);
            //Int32.TryParse(ddlGrant.SelectedValue, out grantValue);

            //if (healthValue > 0) ReportType = ReportType + " " + ddlHealthData.SelectedItem.Text;
            //if (grantValue > 0) ReportType = ReportType + " " + ddlGrant.SelectedItem.Text;

            //isProject = chkProject.Checked ? 1 : 0;

            //--------------------------------------------------------
            //if ((chkBiostat.Checked && chkBioinfo.Checked) || (!chkBiostat.Checked && !chkBioinfo.Checked))
            //    isBiostat = -1;
            //else
            //{
            //    isBiostat = chkBiostat.Checked ? 1 : 2;
            //}


            //if (!chkCreditToBiostat.Checked && !chkCreditToBioinfo.Checked && !chkCreditToBoth.Checked)
            //    creditTo = -1;
            //else
            //{
            //    creditTo = chkCreditToBoth.Checked ? 3 : chkCreditToBioinfo.Checked ? 2 : 1;
            //}
            //--------------------------------------------------------

            //x////////////////////////////////////////////////////////////////////x//
            //x// Obtain value from filters and apply to GetProjectTable() method//x//
            //x////////////////////////////////////////////////////////////////////x//

            int biostatId = 0, projectType = -1, creditTo = -1;

            int isJuniorPI = -1, hasMentor = -1, isInternal = -1, isPilot = -1, isPaid = -1;

            int studyAreaValue = 0, healthValue = 0, studyTypeValue = 0, studyPopValue = 0;
            int serviceValue = 0, fundingValue = 0, aknValue = 0;

            int isRmatrixRequest = -1, isOlaRequest = -1;

            int piId = 0, projectId = 0;

            string projSummary = "";

            // [Biostat]
            Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);


            //[PI ID]
            //if (txtPIName.Value != string.Empty)
            //{
            //    Int32.TryParse(txtPIId.Value, out piId);
            //    ReportType = ReportType + " " + txtPIName.Value;
            //}

            //if (txtAffiliation.Value != string.Empty)
            //{
            //    Int32.TryParse(txtAffilId.Value, out affilId);
            //    ReportType = ReportType + " " + txtAffiliation.Value;
            //}

            // [Filter 1]
            switch (ddlFilter1.SelectedItem.Text)
            {
                case "Investigator": // (1)
                    //filter1 = "AND i.FirstName + ' ' + i.LastName = '" + txtFilter1TextField.Text + "'";
                    //[PI ID]
                    if (txtFilter1TextField.Text != string.Empty)
                    {
                        Int32.TryParse(txtPIId.Value, out piId);
                        ReportType = ReportType + " " + txtFilter1TextField.Text;
                    }
                    break;
                case "Project Type": // (2)
                    projectType = chkFilter1Checkbox.SelectedItem.Text == "Biostat" ? 1 : 2;
                    break;
                case "Project Title": // (3)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out projectId);
                    break;
                case "Project Summary": // (4)
                    //filter1 = "AND p.Summary LIKE '%" + txtFilter1TextField.Text + "%'";
                    projSummary = txtFilter1TextField.Text;
                    break;
                case "Study Area": // (6)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out studyAreaValue);
                    break;
                case "Health Data": // (7)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out healthValue);
                    break;
                case "Study Type": // (8)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out studyTypeValue);
                    break;
                case "Study Population": // (9)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out studyPopValue);
                    break;
                case "Service Type": // (10)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out serviceValue);
                    break;
                case "Credit To": // (11)
                    creditTo = chkFilter1Checkbox.SelectedItem.Text == "Biostat" ? 1 
                             : chkFilter1Checkbox.SelectedItem.Text == "Bioinfo" ? 2 : 3;
                    break;
                case "Is PI a junior investigator?": // (12)
                    isJuniorPI = chkFilter1Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Does PI have mentor?": // (13)
                    hasMentor = chkFilter1Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is project internal study?": // (14)
                    isInternal = chkFilter1Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is project an infrastructure grant study?": // (15)
                    isPilot = chkFilter1Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is paying project?": // (16)
                    isPaid = chkFilter1Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Funding Source?": // (17)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out fundingValue);
                    break;
                case "Acknowledgements": // (18)
                    Int32.TryParse(ddlFilter1DropDown.SelectedValue, out aknValue);
                    break;
                case "Request for resources": // (19)
                    isRmatrixRequest = chkFilter1Checkbox.SelectedItem.Text == "RMATRIX" ? 1 : 0;
                    isOlaRequest = chkFilter1Checkbox.SelectedItem.Text == "Ola HAWAII" ? 1 : 0;
                    break;
                default:
                    break;
            }


            // [Filter 2]
            switch (ddlFilter2.SelectedItem.Text)
            {
                case "Investigator":
                    //[PI ID]
                    if (txtFilter2TextField.Text != string.Empty)
                    {
                        Int32.TryParse(txtPIId.Value, out piId);
                        ReportType = ReportType + " " + txtFilter2TextField.Text;
                    }
                    break;
                case "Project Type": // (2)
                    projectType = chkFilter2Checkbox.SelectedItem.Text == "Biostat" ? 1 : 2;
                    break;
                case "Project Title": // (3)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out projectId);
                    break;
                case "Project Summary": // (4)
                    //Filter2 = "AND p.Summary LIKE '%" + txtFilter2TextField.Text + "%'";
                    projSummary = txtFilter2TextField.Text;
                    break;
                case "Study Area": // (6)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out studyAreaValue);
                    break;
                case "Health Data": // (7)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out healthValue);
                    break;
                case "Study Type": // (8)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out studyTypeValue);
                    break;
                case "Study Population": // (9)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out studyPopValue);
                    break;
                case "Service Type": // (10)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out serviceValue);
                    break;
                case "Credit To": // (11)
                    creditTo = chkFilter2Checkbox.SelectedItem.Text == "Biostat" ? 1
                             : chkFilter2Checkbox.SelectedItem.Text == "Bioinfo" ? 2 : 3;
                    break;
                case "Is PI a junior investigator?": // (12)
                    isJuniorPI = chkFilter2Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Does PI have mentor?": // (13)
                    hasMentor = chkFilter2Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is project internal study?": // (14)
                    isInternal = chkFilter2Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is project an infrastructure grant study?": // (15)
                    isPilot = chkFilter2Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is paying project?": // (16)
                    isPaid = chkFilter2Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Funding Source?": // (17)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out fundingValue);
                    break;
                case "Acknowledgements": // (18)
                    Int32.TryParse(ddlFilter2DropDown.SelectedValue, out aknValue);
                    break;
                case "Request for resources": // (19)
                    isRmatrixRequest = chkFilter2Checkbox.SelectedItem.Text == "RMATRIX" ? 1 : 0;
                    isOlaRequest = chkFilter2Checkbox.SelectedItem.Text == "Ola HAWAII" ? 1 : 0;
                    break;
                default:
                    break;
            }

            // [Filter 3]
            switch (ddlFilter3.SelectedItem.Text)
            {
                case "Investigator": // (1)
                    //[PI ID]
                    if (txtFilter3TextField.Text != string.Empty)
                    {
                        Int32.TryParse(txtPIId.Value, out piId);
                        ReportType = ReportType + " " + txtFilter3TextField.Text;
                    }
                    break;
                case "Project Type": // (2)
                    projectType = chkFilter3Checkbox.SelectedItem.Text == "Biostat" ? 1 : 2;
                    break;
                case "Project Title": // (3)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out projectId);
                    break;
                case "Project Summary": // (4)
                    //Filter3 = "AND p.Summary LIKE '%" + txtFilter3TextField.Text + "%'";
                    projSummary = txtFilter3TextField.Text;
                    break;
                case "Study Area": // (6)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out studyAreaValue);
                    break;
                case "Health Data": // (7)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out healthValue);
                    break;
                case "Study Type": // (8)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out studyTypeValue);
                    break;
                case "Study Population": // (9)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out studyPopValue);
                    break;
                case "Service Type": // (10)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out serviceValue);
                    break;
                case "Credit To": // (11)
                    creditTo = chkFilter3Checkbox.SelectedItem.Text == "Biostat" ? 1
                             : chkFilter3Checkbox.SelectedItem.Text == "Bioinfo" ? 2 : 3;
                    break;
                case "Is PI a junior investigator?": // (12)
                    isJuniorPI = chkFilter3Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Does PI have mentor?": // (13)
                    hasMentor = chkFilter3Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is project internal study?": // (14)
                    isInternal = chkFilter3Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is project an infrastructure grant study?": // (15)
                    isPilot = chkFilter3Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Is paying project?": // (16)
                    isPaid = chkFilter3Checkbox.SelectedItem.Text == "Yes" ? 1 : 0;
                    break;
                case "Funding Source?": // (17)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out fundingValue);
                    break;
                case "Acknowledgements": // (18)
                    Int32.TryParse(ddlFilter3DropDown.SelectedValue, out aknValue);
                    break;
                case "Request for resources": // (19)
                    isRmatrixRequest = chkFilter3Checkbox.SelectedItem.Text == "RMATRIX" ? 1 : 0;
                    isOlaRequest = chkFilter3Checkbox.SelectedItem.Text == "Ola HAWAII" ? 1 : 0;
                    break;
                default:
                    break;
            }


            // [From/To Dates]
            DateTime fromDate = DateTime.TryParse(txtFromDate.Text, out fromDate) ? DateTime.Parse(txtFromDate.Text) 
                                                                                  : new DateTime(2000,01,01);
            DateTime toDate = DateTime.TryParse(txtToDate.Text, out toDate) ? DateTime.Parse(txtToDate.Text) 
                                                                            : new DateTime(2099,01,01);
            
            //if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            //{


            //dt = GetProjectTable(fromDate, toDate, biostatId, isProject, isBiostat, creditTo, piId,
            //                       studyAreaValue, healthValue, studyTypeValue, studyPopValue, serviceValue,
            //                       fundingValue, aknValue);
            dt = GetProjectTable(biostatId, projectType, creditTo, isJuniorPI, hasMentor, isInternal, isPilot,
                                     isPaid, studyAreaValue, healthValue, studyTypeValue, studyPopValue,
                                     serviceValue, fundingValue, aknValue, isRmatrixRequest, isOlaRequest,
                                     piId, projectId, projSummary, fromDate, toDate);


                FromDate = fromDate.ToString("MM/dd/yyyy");
                ToDate = toDate.ToString("MM/dd/yyyy");
            //}
            


            return dt;
        }

        ///dt = GetProjectTable(biostatId, projectType, creditTo, isJuniorPI, hasMentor, isInternal, isPilot,
        //isPaid, studyAreaValue, healthValue, studyTypeValue, studyPopValue,
        //                             serviceValue, fundingValue, aknValue, isRmatrixRequest, isOlaRequest,
        //                             piId, projectId, projSummary, fromDate, toDate);

        /// <summary>
        /// Parses the database for the table information with the given information.
        /// </summary>
        /// <param name="biostatId">Referred QHS Faculty/Staff member.</param>
        /// <param name="projectType">Whether Biostat or Bioinfo project.</param>
        /// <param name="creditTo">Credit to Biostat, Bioinfo, or both.</param>
        /// <param name="isJuniorPI">Is PI a junior investigator?</param>
        /// <param name="hasMentor">Does PI have a mentor?</param>
        /// <param name="isInternal">Internal project (yes/no)?</param>
        /// <param name="isPilot">Is the project an infrastructure grant study?</param>
        /// <param name="isPaid">Is the project a paying project?</param>
        /// <param name="studyAreaValue">Study area selected.</param>
        /// <param name="healthValue">Health Database selected (if applicable).</param>
        /// <param name="studyTypeValue">Study Type selected.</param>
        /// <param name="studyPopValue">Study Population specified.</param>
        /// <param name="serviceValue">Service specified.</param>
        /// <param name="fundingValue">Funding source specified.</param>
        /// <param name="aknValue">Funding acknowledgement specified.</param>
        /// <param name="isRmatrixRequest">If RMATRIX Request for Resources specified.</param>
        /// <param name="isOlaRequest">If Ola HAWAII Request for Resources specified.</param>
        /// <param name="piId">PI (Investigator) ID, if specified.</param>
        /// <param name="projectId">Project ID, if specific project specified.</param>
        /// <param name="projSummary">Project summary containing text.</param>
        /// <param name="fromDate">Starting date of report parameter.</param>
        /// <param name="toDate">Ending date of reprot parameter.</param>
        /// <returns>Table returned from database stored procedure.</returns>
        private DataTable GetProjectTable(int biostatId, int projectType, int creditTo, int isJuniorPI, int hasMentor,
                                          int isInternal, int isPilot, int isPaid, int studyAreaValue, int healthValue,
                                          int studyTypeValue, int studyPopValue, int serviceValue, int fundingValue,
                                          int aknValue, int isRmatrixRequest, int isOlaRequest, int piId, int projectId,
                                          string projSummary, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable("tblProject");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            try
            {

                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Rpt_Project_Summary_Filtered", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BiostatId", biostatId);
                        cmd.Parameters.AddWithValue("@ProjectType", projectType);
                        cmd.Parameters.AddWithValue("@CreditTo", creditTo);
                        cmd.Parameters.AddWithValue("@IsJuniorPI", isJuniorPI);
                        cmd.Parameters.AddWithValue("@HasMentor", hasMentor);
                        cmd.Parameters.AddWithValue("@IsInternal", isInternal);
                        cmd.Parameters.AddWithValue("@IsPilot", isPilot);
                        cmd.Parameters.AddWithValue("@IsPaid", isPaid);
                        cmd.Parameters.AddWithValue("@StudyAreaBitValue", studyAreaValue);
                        cmd.Parameters.AddWithValue("@HealthBitValue", healthValue);
                        cmd.Parameters.AddWithValue("@StudyTypeBitValue", studyTypeValue);
                        cmd.Parameters.AddWithValue("@StudyPopBitValue", studyPopValue);
                        cmd.Parameters.AddWithValue("@ServiceBitValue", serviceValue);
                        cmd.Parameters.AddWithValue("@FundingBitValue", fundingValue);
                        cmd.Parameters.AddWithValue("@AknBitValue", aknValue);
                        cmd.Parameters.AddWithValue("@IsRmatrixRequest", isRmatrixRequest);
                        cmd.Parameters.AddWithValue("@IsOlaRequest", isOlaRequest);
                        cmd.Parameters.AddWithValue("@PIId", piId);
                        cmd.Parameters.AddWithValue("@ProjectId", projectId);
                        cmd.Parameters.AddWithValue("@ProjSummary", projSummary);
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                        cmd.Parameters.AddWithValue("@ToDate", toDate);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }

            return dt;
        }

        /// <summary>
        /// For Filter 1 - Hides/shows filter options depending on which filter to act upon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFilter1_Changed(object sender, EventArgs e)
        {
            string filter1Selection = ddlFilter1.SelectedItem.Text;

            initializeFilter(1, filter1Selection);

        }

        /// <summary>
        /// For Filter 2 - Hides/shows filter options depending on which filter to act upon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFilter2_Changed(object sender, EventArgs e)
        {
            string filter2Selection = ddlFilter2.SelectedItem.Text;

            initializeFilter(2, filter2Selection);
        }

        /// <summary>
        /// For Filter 3 - Hides/shows filter options depending on which filter to act upon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFilter3_Changed(object sender, EventArgs e)
        {
            string filter3Selection = ddlFilter3.SelectedItem.Text;

            initializeFilter(3, filter3Selection);
        }

        /// <summary>
        /// Initializes fields, dropdown values, or checkbox fields for filter,
        /// dependent on which filter selected.
        /// </summary>
        /// <param name="filterNum">Which filter specified (1, 2, or 3).</param>
        /// <param name="filterSelection">Field/question that user chose to filter list with.</param>
        private void initializeFilter(int filterNum, string filterSelection)
        {
          

            using (ProjectTrackerContainer dbContext = new ProjectTrackerContainer())
            {



                var projectType = new List<string>() { "Biostat", "Bioinfo" };
                var creditTo = new List<string>() { "Biostat", "Bioinfo", "Both" };
                var yesNo = new List<string>() { "Yes", "No" };
                var requestForResources = new List<string>() {"RMATRIX", "Ola HAWAII"};


                switch (filterSelection)
                {
                    case "Investigator": // 1
                    case "Project Summary": // 4
                        
                        if (filterNum == 1)
                        {
                            textfieldFilter1.Visible = true;
                            dropdownFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1TextField.Text = filterSelection;
                        }
                        else if (filterNum == 2)
                        {
                            textfieldFilter2.Visible = true;
                            dropdownFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2TextField.Text = filterSelection;
                        }
                        else
                        {
                            textfieldFilter3.Visible = true;
                            dropdownFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3TextField.Text = filterSelection;
                        }

                        break;

                    case "Project Type": // 2


                        if (filterNum == 1)
                        {
                            checkboxesFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            dropdownFilter1.Visible = false;
                            lblFilter1Checkbox.Text = filterSelection;

                            chkFilter1Checkbox.DataSource = projectType;
                            chkFilter1Checkbox.DataBind();
                        }
                        else if (filterNum == 2)
                        {
                            checkboxesFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            dropdownFilter2.Visible = false;
                            lblFilter2Checkbox.Text = filterSelection;

                            chkFilter2Checkbox.DataSource = projectType;
                            chkFilter2Checkbox.DataBind();
                        }
                        else
                        {
                            checkboxesFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            dropdownFilter3.Visible = false;
                            lblFilter3Checkbox.Text = filterSelection;

                            chkFilter3Checkbox.DataSource = projectType;
                            chkFilter3Checkbox.DataBind();
                        }


                        //checkboxesFilter1.Visible = true;
                        //textfieldFilter1.Visible = false;
                        //dropdownFilter1.Visible = false;
                        //lblFilter1Checkbox.Text = filter1Selection;

                        //chkFilter1Checkbox.DataSource = projectType;
                        //chkFilter1Checkbox.DataBind();

                        //var qProjectType = new CheckBoxList(String);
                        //qProjectType.Add("BioStat");
                        //qProjectType.Add("Bioinformatics");
                        //chkFilter1Checkbox.DataSource = qProjectType;
                        //chkFilter1Checkbox.DataBind();

                        break;

                    case "Credit To": // 11
                        //checkboxesFilter1.Visible = true;
                        //textfieldFilter1.Visible = false;
                        //dropdownFilter1.Visible = false;
                        //lblFilter1Checkbox.Text = filter1Selection;

                        //chkFilter1Checkbox.DataSource = creditTo;
                        //chkFilter1Checkbox.DataBind();

                        if (filterNum == 1)
                        {
                            checkboxesFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            dropdownFilter1.Visible = false;
                            lblFilter1Checkbox.Text = filterSelection;

                            chkFilter1Checkbox.DataSource = creditTo;
                            chkFilter1Checkbox.DataBind();

                        } else if (filterNum == 2)
                        {
                            checkboxesFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            dropdownFilter2.Visible = false;
                            lblFilter2Checkbox.Text = filterSelection;

                            chkFilter2Checkbox.DataSource = creditTo;
                            chkFilter2Checkbox.DataBind();
                        } else
                        {
                            checkboxesFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            dropdownFilter3.Visible = false;
                            lblFilter3Checkbox.Text = filterSelection;

                            chkFilter3Checkbox.DataSource = creditTo;
                            chkFilter3Checkbox.DataBind();
                        }

                        break;

                    case "Is PI a junior investigator?": // 12
                    case "Does PI have mentor?": // 13
                    case "Is project internal study?": // 14
                    case "Is project an infrastructure grant study?": // 15
                    case "Is paying project?": // 16

                        if (filterNum == 1)
                        {
                            checkboxesFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            dropdownFilter1.Visible = false;
                            lblFilter1Checkbox.Text = filterSelection;

                            chkFilter1Checkbox.DataSource = yesNo;
                            chkFilter1Checkbox.DataBind();
                        }
                        else if (filterNum == 2)
                        {
                            checkboxesFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            dropdownFilter2.Visible = false;
                            lblFilter2Checkbox.Text = filterSelection;

                            chkFilter2Checkbox.DataSource = yesNo;
                            chkFilter2Checkbox.DataBind();
                        }
                        else
                        {
                            checkboxesFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            dropdownFilter3.Visible = false;
                            lblFilter3Checkbox.Text = filterSelection;

                            chkFilter3Checkbox.DataSource = yesNo;
                            chkFilter3Checkbox.DataBind();
                        }

                        break;


                    case "Project Title": // 3

                        var qProjects =  dbContext
                                            .Project2
                                            .Where(f=>f.Id > 0)
                                            .OrderByDescending(g=>g.Id)
                                            .ToDictionary(f => f.Id, b => (b.Id + " " + b.Title).Substring(0, Math.Min((b.Id + " " + b.Title).Length, 50)));

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;
                          

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qProjects, " --- Select --- ");
                            
                            //ddlFilter1DropDown.Attributes.

                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qProjects, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter3DropDown, qProjects, " --- Select --- ");
                        }

                        break;

                    case "Study Area": //6

                        var qStudyArea = dbContext
                                            .ProjectField
                                            .Where(f => f.IsStudyArea == true)
                                            .ToDictionary(f => f.BitValue, b => b.Name);

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qStudyArea, " --- Select --- ");
                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qStudyArea, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter3DropDown, qStudyArea, " --- Select --- ");
                        }

                        break;

                    case "Health Data": // 7

                        var qHealthData = dbContext
                                            .ProjectField
                                            .Where(f => f.IsHealthData == true && f.Name != "N/A")                                    
                                            .ToDictionary(f => f.BitValue, b => b.Name);

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qHealthData, " --- Select --- ");
                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qHealthData, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter3DropDown, qHealthData, " --- Select --- ");
                        }
                        
                        break;

                    case "Study Type": // 8

                        var qStudyType = dbContext
                                            .ProjectField
                                            .Where(f => f.IsStudyType == true)
                                            .ToDictionary(f=>f.BitValue, b=>b.Name);

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qStudyType, " --- Select --- ");
                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qStudyType, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter3DropDown, qStudyType, " --- Select --- ");
                        }
                        
                        break;

                    case "Study Population": // 9

                        var qStudyPopulation = dbContext
                                                    .ProjectField
                                                    .Where(f => f.IsStudyPopulation == true && f.Name != "N/A")
                                                    .ToDictionary(f=>f.BitValue, b=>b.Name);

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qStudyPopulation, " --- Select --- ");
                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qStudyPopulation, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter3DropDown, qStudyPopulation, " --- Select --- ");
                        }

                        break;

                    case "Service Type": // 10

                        var qServiceType = dbContext
                                            .ProjectField
                                            .Where(f => f.IsService == true)
                                            .ToDictionary(f=>f.BitValue, b=>b.Name);

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qServiceType, " --- Select --- ");
                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qServiceType, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;


                            PageUtility.BindDropDownList(ddlFilter3DropDown, qServiceType, " --- Select --- ");
                        }                      

                        break;

                    case "Funding Source": // 17

                        var qFundingSource = dbContext
                                                .ProjectField
                                                .Where(f => f.IsGrant == true && f.Name != "N/A")
                                                .ToDictionary(f=>f.BitValue, b=>b.Name);

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qFundingSource, " --- Select --- ");
                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qFundingSource, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter3DropDown, qFundingSource, " --- Select --- ");
                        }
                        
                        break;

                    case "Acknowledgements": // 18

                        var qAcknowledgements = dbContext
                                                    .ProjectField
                                                    .Where(f => f.IsAcknowledgment == true)
                                                    .ToDictionary(f=>f.BitValue, b=>b.Name);

                        if (filterNum == 1)
                        {
                            dropdownFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            lblFilter1DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter1DropDown, qAcknowledgements, " --- Select --- ");
                        }
                        else if (filterNum == 2)
                        {
                            dropdownFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            lblFilter2DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter2DropDown, qAcknowledgements, " --- Select --- ");
                        }
                        else
                        {
                            dropdownFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            lblFilter3DropDown.Text = filterSelection;

                            PageUtility.BindDropDownList(ddlFilter3DropDown, qAcknowledgements, " --- Select --- ");
                        }

                        break;

                    case "Request for resources": // 19

                        if (filterNum == 1)
                        {
                            checkboxesFilter1.Visible = true;
                            textfieldFilter1.Visible = false;
                            dropdownFilter1.Visible = false;
                            lblFilter1Checkbox.Text = filterSelection;

                            chkFilter1Checkbox.DataSource = requestForResources;
                            chkFilter1Checkbox.DataBind();
                        }
                        else if (filterNum == 2)
                        {
                            checkboxesFilter2.Visible = true;
                            textfieldFilter2.Visible = false;
                            dropdownFilter2.Visible = false;
                            lblFilter2Checkbox.Text = filterSelection;

                            chkFilter2Checkbox.DataSource = requestForResources;
                            chkFilter2Checkbox.DataBind();
                        }
                        else
                        {
                            checkboxesFilter3.Visible = true;
                            textfieldFilter3.Visible = false;
                            dropdownFilter3.Visible = false;
                            lblFilter3Checkbox.Text = filterSelection;

                            chkFilter3Checkbox.DataSource = requestForResources;
                            chkFilter3Checkbox.DataBind();
                        }

                        break;

                    default:

                        if (filterNum == 1)
                        {
                            textfieldFilter1.Visible = false;
                            dropdownFilter1.Visible = false;
                            checkboxesFilter1.Visible = false;
                            chkFilter1Checkbox.Items.Clear();
                        }
                        else if (filterNum == 2)
                        {
                            textfieldFilter2.Visible = false;
                            dropdownFilter2.Visible = false;
                            checkboxesFilter2.Visible = false;
                            chkFilter2Checkbox.Items.Clear();
                        }
                        else
                        {
                            textfieldFilter3.Visible = false;
                            dropdownFilter3.Visible = false;
                            checkboxesFilter3.Visible = false;
                            chkFilter3Checkbox.Items.Clear();
                        }
                        
                        break;
                }

                // Another switch statement to 
                // add "typeahead" feature.
                switch (filterSelection)
                {

                    case "Investigator":
                        if (filterNum == 1)
                        {
                            //txtFilter1TextField.Attributes.Add("class", "nameahead");
                            //txtFilter1TextField.Attributes["class"] = "nameahead";
                            txtFilter1TextField.CssClass = txtFilter1TextField.CssClass + " nameahead";
                            txtFilter1TextField.Attributes.Add("onchange", "updateId(this)");
                            //--//
                            txtFilter2TextField.CssClass = "form-control";
                            txtFilter3TextField.CssClass = "form-control";
                            txtFilter2TextField.Attributes["onchange"] = "";
                            txtFilter3TextField.Attributes["onchange"] = "";

                        }
                        else if (filterNum == 2)
                        {
                            txtFilter2TextField.CssClass = txtFilter1TextField.CssClass + " nameahead";
                            txtFilter2TextField.Attributes.Add("onchange", "updateId(this)");
                            //-//
                            txtFilter1TextField.CssClass = "form-control";
                            txtFilter3TextField.CssClass = "form-control";
                            txtFilter1TextField.Attributes["onchange"] = "";
                            txtFilter3TextField.Attributes["onchange"] = "";
                        }
                        else if (filterNum == 3)
                        {
                            txtFilter3TextField.CssClass = txtFilter1TextField.CssClass + " nameahead";
                            txtFilter3TextField.Attributes.Add("onchange", "updateId(this)");
                            //-//
                            txtFilter1TextField.CssClass = "form-control";
                            txtFilter2TextField.CssClass = "form-control";
                            txtFilter1TextField.Attributes["onchange"] = "";
                            txtFilter2TextField.Attributes["onchange"] = "";
                        }
                        break;
                    case "Project Title":
                        if (filterNum == 1)
                        {
                            txtFilter1TextField.CssClass = txtFilter1TextField.CssClass + " typeahead";
                            txtFilter1TextField.Attributes.Add("onchange", "updateId(this)");
                            //-//
                            txtFilter2TextField.CssClass = "form-control";
                            txtFilter3TextField.CssClass = "form-control";
                            txtFilter2TextField.Attributes["onchange"] = "";
                            txtFilter3TextField.Attributes["onchange"] = "";
                        }
                        else if (filterNum == 2)
                        {
                            txtFilter2TextField.CssClass = txtFilter1TextField.CssClass + " typeahead";
                            txtFilter2TextField.Attributes.Add("onchange", "updateId(this)");
                            //-//
                            txtFilter1TextField.CssClass = "form-control";
                            txtFilter3TextField.CssClass = "form-control";
                            txtFilter1TextField.Attributes["onchange"] = "";
                            txtFilter3TextField.Attributes["onchange"] = "";
                        }
                        else if (filterNum == 3)
                        {
                            txtFilter3TextField.CssClass = txtFilter1TextField.CssClass + " typeahead";
                            txtFilter3TextField.Attributes.Add("onchange", "updateId(this)");
                            //-//
                            txtFilter1TextField.CssClass = "form-control";
                            txtFilter2TextField.CssClass = "form-control";
                            txtFilter1TextField.Attributes["onchange"] = "";
                            txtFilter2TextField.Attributes["onchange"] = "";
                        }
                        break;
                    default:
                        txtFilter1TextField.CssClass = "form-control";
                        txtFilter2TextField.CssClass = "form-control";
                        txtFilter3TextField.CssClass = "form-control";
                        txtFilter1TextField.Attributes["onchange"] = "";
                        txtFilter2TextField.Attributes["onchange"] = "";
                        txtFilter3TextField.Attributes["onchange"] = "";

                        break;

                }



            }
        }

    }
}