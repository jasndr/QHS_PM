using ProjectManagement.Model;
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
    ///           Summary reports for the list of projects for both RMATRIX and OLA Hawaii, cobined into a single form. 
    ///           May need to alter lists of healthcare data and/or nonUH projects, publications, abstracts/presentations, and academic activities,
    ///           for Ola Hawaii requirements.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018MAY14 - Jason Delos Reyes  -  Created 
    /// </summary>
    public partial class SummaryReport : System.Web.UI.Page
    {
        /// <summary>
        /// Loads page based on initial specifications.
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

            }
        }

        /// <summary>
        /// Obtains the proper reports (displays into a table) based on what is selected if a time period has been specified.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            hdnRowCount.Value = "0";
            int reportId;
            int grantId;
            if (Int32.TryParse(ddlReport.SelectedValue, out reportId) && Int32.TryParse(ddlGrantType.SelectedValue, out grantId))
            {
                DateTime fromDate, toDate;

                if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
                {
                    DataTable dt = new DataTable();
                    if (reportId >= 11 && reportId <= 13) // Projects, Healthcare Data, and NonUH
                    {
                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);

                        rptOlaHawaiiSummary.DataSource = dt;
                        rptOlaHawaiiSummary.DataBind();
                    }
                    else if (reportId == 14) // Publications
                    {
                        dt = GetPubTable(grantId, reportId, fromDate, toDate);
                        rptSummaryPub.DataSource = dt;
                        rptSummaryPub.DataBind();
                    }
                    else if (reportId == 15) // AbstractsPresentations
                    {
                        dt = GetPubTable(grantId, reportId, fromDate, toDate);
                        rptSummaryAbstract.DataSource = dt;
                        rptSummaryAbstract.DataBind();
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
                                        : new DataTable ("tblSummaryAcademic");
            
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = "Rpt_Academic2"; // Calls "Rpt_Academic2" stored procedure in database

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
                                        : new DataTable("tblSummaryPub");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = grantId == 2 ? "Rpt_Paper_Ola" : "Rpt_Paper";

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
            DataTable dt = grantId == 2 ? new DataTable("tblOlaHawaii") : new DataTable("tblRmatrix");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = grantId == 2 ? "Rpt_Ola_Hawaii_Summary" : "Rpt_Rmatrix_Summary";

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

            if (Int32.TryParse(ddlGrantType.SelectedValue, out grantId) && Int32.TryParse(ddlReport.SelectedValue, out reportId) &&
                DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                DataTable dt = new DataTable();
                grantName = grantId == 1 ? "Rmatrix"
                          : grantId == 2 ? "OlaHawaii"
                                         : "";

                switch (reportId)
                {
                    case 11: // (all) projects
                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "Project";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = grantName + "Project" + toDate.ToString("yyyyMMdd");
                        break;
                    case 12: // Healthcare Data projects
                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "HealthData";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = grantName + "HealthData" + toDate.ToString("yyyyMMdd");
                        break;
                    case 13: // NonUH projects
                        dt = GetProjectTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "NonUH";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = grantName + "NonUH" + toDate.ToString("yyyyMMdd");
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
                        fileName = grantName + "Publications" + toDate.ToString("yyyyMMdd");
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
                        fileName = grantName + "AbstractsPresentations" + toDate.ToString("yyyyMMdd");
                        break;
                    case 16: // Academic
                        dt = GetAcademicTable(grantId, reportId, fromDate, toDate);
                        dt.TableName = "Academic";
                        dt.Columns.Remove("Column1");
                        dt.Columns.Remove("Column2");
                        fileName = grantName + "Academic" + toDate.ToString("yyyyMMdd");
                        break;
                    default:
                        break;
                }

                FileExport fileExport = new FileExport(this.Response);
                fileExport.ExcelExport(dt, fileName);
            }

        }


    }
}