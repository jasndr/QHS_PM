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
    /// @File: RmatrixSummary.aspx.cs
    /// @Author: Yang Rui
    /// @Summary: RMATRIX Summary Reports
    /// 
    ///           Summary reports for the list of projects, healthcare data, nonUH, publications, abstracts/presentations, 
    ///           and academic activities conducted by the Quantitative Health Sciences branch of the Department of
    ///           Complementary & Integrative Medicine for reporting to RMATRIX.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018MAY09 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///  2018MAY14 - Jason Delos Reyes  -  Added more comments for easier data structure view and management.
    /// </summary>
    public partial class RmatrixSummary : System.Web.UI.Page
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
        /// Pre-loads dropdown list based on what the type of reports there are for the RMATRIX Summary reports.
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
            if (Int32.TryParse(ddlReport.SelectedValue, out reportId))
            {
                DateTime fromDate = new DateTime(2000,01,01), toDate = new DateTime(2099,01,01);

                if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
                {
                    DataTable dt = new DataTable();
                    if (reportId >= 11 && reportId <= 13) // Projects and NonUH
                    {
                        dt = GetProjectTable(reportId, fromDate, toDate);

                        rptRmatrixSummary.DataSource = dt;
                        rptRmatrixSummary.DataBind();
                    }
                    else if (reportId == 14) // Publications
                    {
                        dt = GetPubTable(reportId, fromDate, toDate);
                        rptRmatrixSummaryPub.DataSource = dt;
                        rptRmatrixSummaryPub.DataBind();
                    }
                    else if (reportId == 15) // AbstractsPresentations
                    {
                        dt = GetPubTable(reportId, fromDate, toDate);
                        rptRmatrixSummaryAbstract.DataSource = dt;
                        rptRmatrixSummaryAbstract.DataBind();
                    }
                    else if (reportId == 16) // Academic
                    {
                        dt = GetAcademicTable(reportId, fromDate, toDate);
                        rptRmatrixSummaryAcademic.DataSource = dt;
                        rptRmatrixSummaryAcademic.DataBind();
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
        /// Populates Data Table for Academic Report for RMATRIX Summary.
        /// </summary>
        /// <param name="reportId">Report ID (16 for "Academic")</param>
        /// <param name="fromDate">Initial time period point.</param>
        /// <param name="toDate">Ending time period point.</param>
        /// <returns></returns>
        private DataTable GetAcademicTable(int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable("tblRmatrixAcademic");
            
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
        /// Generates Publications reports for either Publications (Manuscripts), or Abstracts/Presentations (Abstracts).
        /// </summary>
        /// <param name="reportId">Report type (either 14 for "Publications" [Manuscripts]
        ///                                         or 15 for "Abstracts [Abstracts/Presentations]).</param>
        /// <param name="fromDate">Starting time period.</param>
        /// <param name="toDate">Ending time period.</param>
        /// <returns></returns>
        private DataTable GetPubTable(int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable("tblRmatrixPub");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = "Rpt_Paper"; //Calls "Rpt_Paper" stored procedure in database

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
        /// <param name="reportId">Id of RMATRIX Summary Report type (11 through 13 in SQL 'Report' table).</param>
        /// <param name="fromDate">Starting date parameter specified in text field.</param>
        /// <param name="toDate">Ending date parameter specified in text field.</param>
        /// <returns>Data table selected after SQL query.</returns>
        private DataTable GetProjectTable(int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable("tblRmatrix");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = "Rpt_RMATRIX_Summary";

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
            int reportId;
            DateTime fromDate = new DateTime(2000,01,01), toDate = new DateTime(2099,01,01);
            string fileName= "";

            if (Int32.TryParse(ddlReport.SelectedValue, out reportId) &&
                DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                DataTable dt = new DataTable();

                switch (reportId)
                {
                    case 11: // (all) projects
                        dt = GetProjectTable(reportId, fromDate, toDate);
                        dt.TableName = "Project";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = "Project" + toDate.ToString("yyyyMMdd");
                        break;
                    case 12: // Healthcare Data projects
                        dt = GetProjectTable(reportId, fromDate, toDate);
                        dt.TableName = "HealthData";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = "HealthData" + toDate.ToString("yyyyMMdd");
                        break;
                    case 13: // NonUH projects
                        dt = GetProjectTable(reportId, fromDate, toDate);
                        dt.TableName = "NonUH";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = "NonUH" + toDate.ToString("yyyyMMdd");
                        break;
                    case 14: // Publications
                        dt = GetPubTable(reportId, fromDate, toDate);
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
                        fileName = "Publications" + toDate.ToString("yyyyMMdd");
                        break;
                    case 15: // AbstractsPresentations
                        dt = GetPubTable(reportId, fromDate, toDate);
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
                        fileName = "AbstractsPresentations" + toDate.ToString("yyyyMMdd");
                        break;
                    case 16: // Academic
                        dt = GetAcademicTable(reportId, fromDate, toDate);
                        dt.TableName = "Academic";
                        dt.Columns.Remove("Column1");
                        dt.Columns.Remove("Column2");
                        fileName = "Academic" + toDate.ToString("yyyyMMdd");
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