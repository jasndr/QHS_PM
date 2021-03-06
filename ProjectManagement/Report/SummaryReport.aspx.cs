﻿using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Report
{
    /// <summary>
    /// @File: SummaryReport.aspx.cs
    /// @Author: Jason Delos Reyes
    /// @Summary: RMATRIX / OLA Hawaii Summary Reports
    /// 
    ///           Summary reports for the list of projects for both RMATRIX and OLA Hawaii, combined into a single form. 
    ///           May need to alter lists of healthcare data and/or nonUH projects, publications, abstracts/presentations, and academic activities,
    ///           for Ola Hawaii requirements.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018MAY14 - Jason Delos Reyes  -  Created Summary Report tab as a consolidation of RMATRIX Summary and Ola Hawaii summary
    ///                                    tabs so both reports can be accessed through one view.
    ///  2018MAY16 - Jason Delos Reyes  -  Revised Ola Hawaii paper pull to extract *all* papers instead.  Will not be using 
    ///                                    Rpt_Paper_Ola stored procedure.
    ///  2018JUL20 - Jason Delos Reyes  -  Changed paper summary reports so that only pulls papers with projects that
    ///                                    are affiliated with either RMATRIX and/or Ola Hawaii (can be expanded later with more
    ///                                    grants).  GetPubTable now extracts from Rpt_Paper2 stored procedure.
    ///  2018JUL25 - Jason Delos Reyes  -  Utilized a new Excel function that prints the title on top of the data table sheet
    ///                                    before being able to download the data (originally obtained from the SQL database) 
    ///                                    into an Excel file.
    ///  2018JUL31 - Jason Delos Reyes  -  Started creating Summary Report feature by adding Collaboration Center dropdown option
    ///                                    for report.
    ///  2018AUG01 - Jason Delos Reyes  -  Adjusted Collaboration Center report, both in input parameters and stored procedure, 
    ///                                    so that users can also view the contact for the specific collaboration center.
    ///  2018AUG02 - Jason Delos Reyes  -  Fixed the error that would go make the page revert to showing the "Grant" dropdown 
    ///                                    instead of remaining fixated on the "Collaboration Center" dropdown after the "submit"
    ///                                    button has been clicked. 
    ///                                 -  Also fixed the error of  the report still pulling from Collaboration Center despite
    ///                                    switching back to the Grant section to view more reports.
    ///                                 -  Updated Rpt_Paper2 stored procedure so that papers affiliated with projects connected
    ///                                    with the specified collaboration center could be pulled into the report.
    ///  2018AUG03 - Jason Delos Reyes  -  Made "Collaboration Center" Report section of the Summary Report page to not be accessible 
    ///                                    to non-admin users.
    ///  2018OCT30 - Jason Delos Reyes  -  Fixed the styling report section that allows users to view the results in a scrollable
    ///                                    window, with a fixed header to keep column labels visible while scrolling.
    ///  2018NOV05 - Jason Delos Reyes  -  Fixed bug that prints the headers for the Collboration Center if that one was selected first
    ///                                    but user went back to the Grant section to download that report instead.
    ///  2018NOV14 - Jason Delos Reyes  -  Fixed Rpt_Academic2 procedure to pull the right academic events within the specified range.
    ///  2019MAR18 - Jason Delos Reyes  -  Changed Ola Hawaii stored procedure reference to "Rpt_Ola_Hawaii_Summary2" to facilitate new
    ///                                    total row numbers similarly requested by the inaugural Ola Hawaii TAC report.
    ///                                 -  Fixed summary reports for Ola Hawaii to pull *all* the columns from the report.
    ///  2019MAR20 - Jason Delos Reyes  -  Redirected Publications and Abstracts to Rpt_Paper2b stored procedure to 
    ///                                    add information specifically regarding to Ola HAWAII-specific numbers.
    ///  2019AUG30 - Jason Delos Reyes  -  Integrated "Rpt_Ola_Hawaii_Summary2a" to reorganize Ola-specific fields in summary report.
    /// </summary>
    public partial class SummaryReport : System.Web.UI.Page
    {
        protected string GrantName { get; set; }
        protected string ReportType { get; set; }
        protected string FromDate { get; set; }
        protected string ToDate { get; set; }
        
        /// <summary>
        /// Loads page based on initial specifications.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            // If Page is being loaded for the first time -- populates dropdown options for review.
            if (!Page.IsPostBack)
            {
                BindControl();
            }
            // If Page is just being reloaded again, e.g., click option button -- loads proper form for report requested.
            else
            {
                LoadStartScript();
            }
        }


        /// <summary>
        /// Loads either -Grant- or -Collaboration- dropdown depending on which option was initially selected.
        /// </summary>
        private void LoadStartScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();
            script.Append("<script>");

            if (Request["optionsRadios"].ToString() == "option1")
            {
                script.Append("if (!document.getElementById('optionsRadios1').checked){document.getElementById('optionsRadios1').checked=true;} ");
            }
            else
            {
                script.Append("if (!document.getElementById('optionsRadios2').checked){document.getElementById('optionsRadios2').checked=true;} ");
            }
            

            script.Append("</script>");
            ClientScript.RegisterStartupScript(GetType(), "Javascript", script.ToString());
        }


        /// <summary>
        /// Pre-loads dropdown list based on what the type of reports there are for the Ola Hawaii Summary reports.
        /// Uses Rmatrix Summary drop downs as they are the same for Ola Hawaii reports.
        /// </summary>
        private void BindControl()
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                //11  RmatrixSummary Projects
                //12  RmatrixSummary HealthcareData
                //13  RmatrixSummary NonUH
                //14  RmatrixSummary Publications
                //15  RmatrixSummary AbstractsPresentations
                //16  RmatrixSummary Academic
                dropDownSource = context.BiostatReports
                                .Where(b => b.ReportType == "RmatrixSummary")
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.ReportName);
                PageUtility.BindDropDownList(ddlReport, dropDownSource, "--- Select Report ---");

                /// Populates Grant type dropdown (RMATRIX/Ola Hawaii)
                Dictionary<int, string> grantTypeSource = new Dictionary<int, string>();
                grantTypeSource.Add(1, "RMATRIX");
                grantTypeSource.Add(2, "Ola Hawaii");
                PageUtility.BindDropDownList(ddlGrantType, grantTypeSource, null);

                /// Populates Collaboration Center dropdown (e.g., OBGYN, SONDH, etc.)
                Dictionary<int, string> collabCtrSource = new Dictionary<int, string>();
                collabCtrSource = context.CollabCtr
                                 .Where(d => d.Id > 0)
                                 .OrderBy(h => h.NameAbbrv)
                                 .ToDictionary(x => x.Id, x => x.NameAbbrv + " (" + x.Contact + ")");
                PageUtility.BindDropDownList(ddlCollabCenter, collabCtrSource, "--- Select Collaborative Center ---");


                /// Only Admin are allowed to view collaborative center report option.
                if (!Page.User.IsInRole("Admin"))
                {
                    collabCtrSectionTitle.Visible = false;
                    collabCtrButtonSection.Visible = false;

                }
                else
                {
                    collabCtrSectionTitle.Visible = true;
                    collabCtrButtonSection.Visible = true;

                }

            }
        }

        /// <summary>
        /// Obtains the proper reports (displays into a table) based on what is selected if a time period has been specified.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            hdnRowCount.Value = "0";
            int reportId;
            int grantId;
            string collabCenterText = ddlCollabCenter.SelectedItem.Text;

            if (Request["optionsRadios"].ToString() == "option1")
            {
                grantId = Convert.ToInt32(ddlGrantType.SelectedValue);
            }
            else
            {
                grantId = 3;
            }

            if (Int32.TryParse(ddlReport.SelectedValue, out reportId) /*&& Int32.TryParse(ddlGrantType.SelectedValue, out grantId)*/)
            {
                DateTime fromDate, toDate;

                grantId = (grantId == 1 || grantId == 2) ? grantId : 3;

                if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
                {
                    DataTable dt = new DataTable();
                    if (reportId >= 11 && reportId <= 13) // Projects, Healthcare Data, and NonUH
                    {
                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);

                        if (grantId == 2)
                        {
                            rptSummaryProjectOla.DataSource = dt;
                            rptSummaryProjectOla.DataBind();
                        }
                        else
                        {
                            rptSummaryProject.DataSource = dt;
                            rptSummaryProject.DataBind();
                        }


                    }
                    else if (reportId == 14) // Publications
                    {
                        dt = GetPubTable(grantId, reportId, fromDate, toDate);
                        if (grantId == 2)
                        {
                            rptSummaryPubOla.DataSource = dt;
                            rptSummaryPubOla.DataBind();
                        }
                        else
                        {
                            rptSummaryPub.DataSource = dt;
                            rptSummaryPub.DataBind();
                        }
                    }
                    else if (reportId == 15) // AbstractsPresentations
                    {
                        dt = GetPubTable(grantId, reportId, fromDate, toDate);
                        if (grantId == 2)
                        {
                            rptSummaryAbstractOla.DataSource = dt;
                            rptSummaryAbstractOla.DataBind();
                        }
                        else
                        {
                            rptSummaryAbstract.DataSource = dt;
                            rptSummaryAbstract.DataBind();
                        }
                            
                    }
                    else if (reportId == 16) // Academic
                    {
                        dt = GetAcademicTable(grantId, reportId, fromDate, toDate);
                        rptSummaryAcademic.DataSource = dt;
                        rptSummaryAcademic.DataBind();
                    }

                    hdnRowCount.Value = dt.Rows.Count.ToString();

                    //string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    //SqlConnection con = new SqlConnection(constr);

                    //var cmdText = "Rpt_RMATRIX_Summary";

                    //try
                    //{
                    //    DataTable dt = new DataTable("tblRmatrix");
                    //    using (SqlConnection sqlcon = new SqlConnection(constr))
                    //    {
                    //        using (SqlCommand cmd = new SqlCommand(cmdText, sqlcon))
                    //        {
                    //            cmd.CommandType = CommandType.StoredProcedure;
                    //            cmd.Parameters.AddWithValue("@StartDate", fromDate);
                    //            cmd.Parameters.AddWithValue("@EndDate", toDate);
                    //            cmd.Parameters.AddWithValue("@Affiliation", "");

                    //            if (reportId == 12)
                    //            {
                    //                cmd.Parameters.AddWithValue("@HealthData", 1);
                    //            }

                    //            if (reportId == 13)
                    //            {
                    //                cmd.Parameters.AddWithValue("@NonUH", 1);
                    //            }

                    //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    //            {
                    //                da.Fill(dt);
                    //            }
                    //        }
                    //    }

                    //    rptRmatrixSummary.DataSource = dt;

                    //    rptRmatrixSummary.DataBind();
                    //}
                    //catch (Exception ex)
                    //{
                    //    //Response.Write(ex.ToString());
                    //    throw ex;
                    //}
                    //finally
                    //{
                    //    if (con.State != ConnectionState.Closed)
                    //    {
                    //        con.Close();
                    //    }
                    //}
                }

            }
        }

        /// <summary>
        /// Obtains academic report from SQL database by adding report and from/to dates
        /// into "Rpt_Academic" stored query.
        /// </summary>
        /// <param name="grantId">RMATRIX or Ola Hawaii Grant</param>
        /// <param name="reportId">Report ID (usually just 16-Academic)</param>
        /// <param name="fromDate">Beginning time period</param>
        /// <param name="toDate">Ending time period</param>
        /// <returns>Table with academic report</returns>
        private DataTable GetAcademicTable(int grantId, int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = grantId == 1 ? new DataTable("tblRmatrixAcademic") 
                         : grantId == 2 ? new DataTable("tblOlaHawaiiAcademic") 
                         : grantId == 3 ? new DataTable ("tblCollabCtrAcademic")
                                        : new DataTable ("tblSummaryAcademic");
            
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = "Rpt_Academic2"; // Calls "Rpt_Academic2" stored procedure in database

            GrantName = grantId == 2 ? "Ola_Hawaii" : grantId == 1 ? "RMATRIX" : ddlCollabCenter.SelectedItem.Text;
            switch (reportId)
            {
                case 11:
                    ReportType = "Projects";
                    break;
                case 12:
                    ReportType = "HealthData_Projects";
                    break;
                case 13:
                    ReportType = "Non-UH_Projects";
                    break;
                case 14:
                    ReportType = "Publications";
                    break;
                case 15:
                    ReportType = "Abstracts/Presentations";
                    break;
                case 16:
                    ReportType = "Academic";
                    break;
                default:
                    ReportType = "";
                    break;
            }
            FromDate = fromDate.ToString("MM/dd/yyyy");
            ToDate = toDate.ToString("MM/dd/yyyy");

            try
            {

                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText, sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                        cmd.Parameters.AddWithValue("@ToDate", toDate);
                        cmd.Parameters.AddWithValue("@IsSummary", 1);
                        
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
        /// Generates data table for "publication" type reports, specific to OLA Hawaii-affiliated papers *only*.
        /// </summary>
        /// <param name="grantId">Grant type (1 for RMATRIX, 2 for Ola Hawaii).</param>
        /// <param name="reportId">Report type (either 14 for manuscripts/publications
        ///                                         or 15 for abstracts/presentations.)</param>
        /// <param name="fromDate">Starting time period.</param>
        /// <param name="toDate">Ending time period.</param>
        /// <returns></returns>
        private DataTable GetPubTable(int grantId, int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = grantId == 1 ? new DataTable("tblRmatrixPub")
                         : grantId == 2 ? new DataTable("tblOlaHawaiiPub")
                         : grantId == 3 ? new DataTable("tblCollabCtrPub")
                                        : new DataTable("tblSummaryPub");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText =  "Rpt_Paper2";

            GrantName = grantId == 2 ? "Ola_Hawaii" : grantId == 1 ? "RMATRIX" : ddlCollabCenter.SelectedItem.Text;
            switch (reportId)
            {
                case 11:
                    ReportType = "Projects";
                    break;
                case 12:
                    ReportType = "HealthData_Projects";
                    break;
                case 13:
                    ReportType = "Non-UH_Projects";
                    break;
                case 14:
                    ReportType = "Publications";
                    if (grantId == 2)
                        cmdText = "Rpt_Paper2b";
                    break;
                case 15:
                    ReportType = "Abstracts/Presentations";
                    if (grantId == 2)
                        cmdText = "Rpt_Paper2b";
                    break;
                case 16:
                    ReportType = "Academic";
                    break;
                default:
                    ReportType = "";
                    break;
            }
            FromDate = fromDate.ToString("MM/dd/yyyy");
            ToDate = toDate.ToString("MM/dd/yyyy");

            try
            {

                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText, sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PubFromDate", fromDate);
                        cmd.Parameters.AddWithValue("@PubToDate", toDate);
                        cmd.Parameters.AddWithValue("@IsSummary", 1);

                        // With the given grantId, allows users to only pull papers
                        // with projects that are affiliated with RMATRIX or Ola Hawaii 
                        // (in funding source and/or acknowledgement sections)
                        if (grantId == 1)
                        {
                            cmd.Parameters.AddWithValue("@ProjectGrant", "RMATRIX");
                        }
                        if (grantId == 2)
                        {
                            cmd.Parameters.AddWithValue("@ProjectGrant", ""); //Removed "Ola Hawaii" to fit with new Rpt_Paper2b stored proecure
                        }
                        if (grantId == 3)
                        {
                            cmd.Parameters.AddWithValue("@ProjectGrant", ddlCollabCenter.SelectedItem.Text);
                        }



                        if (reportId == 14)
                        {
                            cmd.Parameters.AddWithValue("@PubType", 2);
                        }

                        if (reportId == 15)
                        {
                            cmd.Parameters.AddWithValue("@PubType", 1);
                        }

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
        /// Obtains PROJECT report table. Referenced by report types
        ///  11 (all projects), 
        ///  12 (Healthcare Data projects), and
        ///  13 (NonUH projects).
        /// </summary>
        /// <param name="grantId">Grant type (1 for RMATRIX, 2 for Ola Hawaii).</param>
        /// <param name="reportId">Id of RMATRIX Summary Report type (11 through 16 in SQL 'Report' table).</param>
        /// <param name="fromDate">Starting date parameter specified in text field.</param>
        /// <param name="toDate">Ending date parameter specified in text field.</param>
        /// <returns>Data table selected after SQL query.</returns>
        private DataTable GetProjectTable(int grantId, int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = grantId == 2 ? new DataTable("tblOlaHawaii") : 
                           grantId == 1 ? new DataTable("tblRmatrix") :
                                          new DataTable("tblCollabCtr");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = grantId == 2 ? "Rpt_Ola_Hawaii_Summary2a" :
                          grantId == 1 ? "Rpt_Rmatrix_Summary" :
                                         "Rpt_CollabCtr_Summary";

            GrantName = grantId == 2 ? "Ola_Hawaii" :
                        grantId == 1 ? "RMATRIX" :
                                        ddlCollabCenter.SelectedItem.Text;
            switch(reportId)
            {
                case 11:
                    ReportType = "Projects";
                    break;
                case 12:
                    ReportType = "HealthData_Projects";
                    break;
                case 13:
                    ReportType = "Non-UH_Projects";
                    break;
                case 14:
                    ReportType = "Publications";
                    break;
                case 15:
                    ReportType = "Abstracts/Presentations";
                    break;
                case 16:
                    ReportType = "Academic";
                    break;
                default:
                    ReportType = "";
                    break;
            }
            FromDate = fromDate.ToString("MM/dd/yyyy");
            ToDate = toDate.ToString("MM/dd/yyyy");

           
            try
            {

                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText, sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", fromDate);
                        cmd.Parameters.AddWithValue("@EndDate", toDate);
                        cmd.Parameters.AddWithValue("@Affiliation", "");

                        if (grantId == 3) // Collaboration Center - Add to report
                        {
                            cmd.Parameters.AddWithValue("@CollabCtr", ddlCollabCenter.SelectedItem.Text);
                        }

                        if (reportId == 12) // Healthcare Data report
                        {
                            cmd.Parameters.AddWithValue("@HealthData", 1);
                        }

                        if (reportId == 13) // NonUH report
                        {
                            cmd.Parameters.AddWithValue("@NonUH", 1);
                        }

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
        /// Obtains report based on start/end date parameters and exports it into an excel file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            int grantId;
            int reportId;
            DateTime fromDate, toDate;
            string fileName = "";
            string grantName = "";
            string titleName = "";

            if (Request["optionsRadios"].ToString() == "option1")
            {
                grantId = Convert.ToInt32(ddlGrantType.SelectedValue);
            }
            else
            {
                grantId = 3;
            }

            string collabCenterText = ddlCollabCenter.SelectedItem.Text;

            //grantId = collabCenterText != "--- Select Collaborative Center ---" ? grantId = 3 : grantId = Convert.ToInt32(ddlGrantType.SelectedValue);

            //GrantName = grantId == 2 ? "Ola_Hawaii" :
            //            grantId == 1 ? "RMATRIX" :
            //                            ddlCollabCenter.SelectedItem.Text;

            if (/*Int32.TryParse(ddlGrantType.SelectedValue, out grantId) &&*/ Int32.TryParse(ddlReport.SelectedValue, out reportId) &&
                DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                DataTable dt = new DataTable();
                grantName = grantId == 1 ? "Rmatrix"
                          : grantId == 2 ? "OlaHawaii"
                                         : collabCenterText;

                switch (reportId)
                {
                    case 11: // (all) projects

                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "Project";

                        if (grantId != 2)
                        {
                            dt.Columns.Remove("HealthData");
                            dt.Columns.Remove("NonUH");
                        }
                        
                        
                        break;
                    case 12: // Healthcare Data projects
                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "HealthData";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        break;
                    case 13: // NonUH projects
                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "NonUH";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        break;
                    case 14: // Publications
                        dt = GetPubTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "Publications";
                        dt.Columns.Remove("conf");
                        dt.Columns.Remove("ConfLoc");
                        dt.Columns.Remove("StartDate");
                        dt.Columns.Remove("EndDate");
                        dt.Columns.Remove("pp");
                        dt.Columns.Remove("GrantAffilName");
                        dt.Columns.Remove("FromDate");
                        dt.Columns.Remove("ToDate");
                        dt.Columns.Remove("CitationFormat");
                        dt.Columns.Remove("BiostatName");
                        //if(grantId != 2)
                        //{
                        //    dt.Columns.Remove("IsOlaFunded");
                        //    dt.Columns.Remove("IsOlaAcknowledged");
                        //    dt.Columns.Remove("IsOlaRequest");
                        //}
                        break;
                    case 15: // AbstractsPresentations
                        dt = GetPubTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "AbstractsPresentations";
                        dt.Columns.Remove("JournalName");
                        dt.Columns.Remove("pp");
                        dt.Columns.Remove("pubYear");
                        dt.Columns.Remove("pubMonth");
                        dt.Columns.Remove("Volume");
                        dt.Columns.Remove("Issue");
                        dt.Columns.Remove("Page");
                        dt.Columns.Remove("DOI");
                        dt.Columns.Remove("PMID");
                        dt.Columns.Remove("PMCID");
                        dt.Columns.Remove("GrantAffilName");
                        dt.Columns.Remove("FromDate");
                        dt.Columns.Remove("ToDate");
                        dt.Columns.Remove("CitationFormat");
                        dt.Columns.Remove("BiostatName");
                        //if (grantId != 2)
                        //{
                        //    dt.Columns.Remove("IsOlaFunded");
                        //    dt.Columns.Remove("IsOlaAcknowledged");
                        //    dt.Columns.Remove("IsOlaRequest");
                        //}
                        break;
                    case 16: // Academic
                        dt = GetAcademicTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "Academic";
                        dt.Columns.Remove("Column1");
                        dt.Columns.Remove("Column2");
                        break;
                    default:
                        break;
                }

                titleName = GrantName + " " + ReportType + " Report from " + FromDate + " to " + ToDate;
                fileName = GrantName + "-" + ReportType + "-" + "Report-From" + "-" + FromDate + "-" + "To" + "-" + ToDate;

                fileName = fileName.Replace(" ", "_");

                FileExport fileExport = new FileExport(this.Response);
                fileExport.ExcelExport2(dt, fileName, titleName);
            }

        }


    }
}