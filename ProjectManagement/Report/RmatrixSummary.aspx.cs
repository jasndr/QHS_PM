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
    public partial class RmatrixSummary : System.Web.UI.Page
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
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                dropDownSource = context.BiostatReports
                                .Where(b => b.ReportType == "RmatrixSummary")
                                .OrderBy(b => b.Id)
                                .ToDictionary(c => c.Id, c => c.ReportName);

                PageUtility.BindDropDownList(ddlReport, dropDownSource, "--- Select Report ---");
            }
        }

        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            hdnRowCount.Value = "0";
            int reportId;
            if (Int32.TryParse(ddlReport.SelectedValue, out reportId))
            {
                DateTime fromDate, toDate;

                if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
                {
                    DataTable dt = new DataTable();
                    if (reportId >= 11 && reportId <= 13)
                    {
                        dt = GetProjectTable(reportId, fromDate, toDate);

                        rptRmatrixSummary.DataSource = dt;
                        rptRmatrixSummary.DataBind();
                    }
                    else if (reportId == 14)
                    {
                        dt = GetPubTable(reportId, fromDate, toDate);
                        rptRmatrixSummaryPub.DataSource = dt;
                        rptRmatrixSummaryPub.DataBind();
                    }
                    else if (reportId == 15)
                    {
                        dt = GetPubTable(reportId, fromDate, toDate);
                        rptRmatrixSummaryAbstract.DataSource = dt;
                        rptRmatrixSummaryAbstract.DataBind();
                    }
                    else if (reportId == 16)
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

        private DataTable GetAcademicTable(int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable("tblRmatrixAcademic");
            
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = "Rpt_Academic";

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

        private DataTable GetPubTable(int reportId, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable("tblRmatrixPub");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            var cmdText = "Rpt_Paper";

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

                        if (reportId == 12)
                        {
                            cmd.Parameters.AddWithValue("@HealthData", 1);
                        }

                        if (reportId == 13)
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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            int reportId;
            DateTime fromDate, toDate;
            string fileName= "";

            if (Int32.TryParse(ddlReport.SelectedValue, out reportId) &&
                DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                DataTable dt = new DataTable();

                switch (reportId)
                {
                    case 11:
                        dt = GetProjectTable(reportId, fromDate, toDate);
                        dt.TableName = "Project";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = "Project" + toDate.ToString("yyyyMMdd");
                        break;
                    case 12:
                        dt = GetProjectTable(reportId, fromDate, toDate);
                        dt.TableName = "HealthData";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = "HealthData" + toDate.ToString("yyyyMMdd");
                        break;
                    case 13:
                        dt = GetProjectTable(reportId, fromDate, toDate);
                        dt.TableName = "NonUH";
                        dt.Columns.Remove("HealthData");
                        dt.Columns.Remove("NonUH");
                        fileName = "NonUH" + toDate.ToString("yyyyMMdd");
                        break;
                    case 14:
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
                    case 15:
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
                    case 16:
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