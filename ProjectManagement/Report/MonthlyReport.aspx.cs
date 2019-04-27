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
    /// @File: MonthlyReport.aspx.cs
    /// @Frontend: MonthlyReport.aspx
    /// @Author: Jason Delos Reyes
    /// @Summary: RMATRIX / Ola Hawaii Monthly Reports.
    /// 
    ///           Report of projects from a specified time period, optionally closing in by specific PI information,
    ///           affiliation, use of HealthData, and/or specific grants.  Derived from "RMATRIX Monthly Reports; extended 
    ///           to include Ola HAWAII.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2019APR26 - Jason Delos Reyes  -  Duplicated file from RmatrixMonthly.aspx to allow for multiple grants, namely
    ///                                    the addition of the Ola HAWAII grant for now.
    /// </summary>
    public partial class MonthlyReport : System.Web.UI.Page
    {
        /// <summary>
        /// Load page on page visit/reload.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnRowCount.Value = "0";

             if (!Page.IsPostBack)
            {
                BindControl();
            }
        }

        /// <summary>
        /// Preloads grant dropdown (selection between Ola HAWAII and RMATRIX)
        /// </summary>
        private void BindControl()
        {
            /// Populates Grant type dropdown (RMATRIX/Ola Hawaii)
            Dictionary<int, string> grantTypeSource = new Dictionary<int, string>();
            grantTypeSource.Add(1, "RMATRIX");
            grantTypeSource.Add(2, "Ola HAWAII");
            PageUtility.BindDropDownList(ddlGrantType, grantTypeSource, null);
        }

        /// <summary>
        /// Obtains RMATRIX summary report from database as long as there is no Request for Resources
        /// form specified.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            divReport.Visible = true;


            if (ddlGrantType.SelectedItem.Text.Equals("RMATRIX"))
            {
                rptMonthly.DataSource = GetRmatrixTable();
                rptMonthly.DataBind();
            }
            else //if (ddlGrantType.SelectedItem.Equals("Ola HAWAII"))
            {
                rptMonthly.DataSource = GetOlaHawaiiTable();
                rptMonthly.DataBind();
            }

            
            Control footerTemplate = rptMonthly.Controls[rptMonthly.Controls.Count - 1].Controls[0];
            Label lblFooter = footerTemplate.FindControl("lblTotal") as Label;
            lblFooter.Text = hdnRowCount.Value;

        }

        /// <summary>
        /// Call ExportTable() function when the "Download" (to Excel) button has been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (chkRmatrix.Checked)
            {
                ExportTable();
            }
            else
            {
                ExportCsv();
            }
        }

        /// <summary>
        /// Obtains the RMATRIX report table from the database with the dates specified.
        /// </summary>
        /// <returns>Table of project to report monthly to RMATRIX (Ms. Grace Matsuura, GDO Data Coordinator).</returns>
        private DataTable GetRmatrixTable()
        {
            DateTime fromDate, toDate;
            DataTable dt = new DataTable("RMATRIX");

            //int year = reportDate.Year;
            //int month = reportDate.Month;
            //var firstDay = new DateTime(year, month, 1);
            //var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                using (ProjectTrackerContainer dbContext = new ProjectTrackerContainer())
                {
                    var query = dbContext.vwProject
                        .Where(p => p.ReportDate >= fromDate
                                    && p.ReportDate <= toDate
                        );

                    if (chkRmatrix.Checked)
                    {
                        query = query.Where(p => (bool) p.IsRmatrix);
                    }
                    else
                    {
                        query = query.Where(p => !(bool) p.IsRmatrix
                                                 && !(bool) p.NotReportRmatrix
                                                 && (int) p.IsInternal == 0
                                                 && p.IsPilot == "N"
                        );
                    }

                    dt = PageUtility.ToDataTable(query.ToList());
                    hdnRowCount.Value = query.Count().ToString();
                }
            }

            return dt;
        }

        /// <summary>
        /// Obtains the Ola HAWAII report table from the database with the dates specified.
        /// </summary>
        /// <returns>Table of project to submit monthly to Ola HAWAII (Jason Delos Reyes or Meliza Roman to import).</returns>
        private DataTable GetOlaHawaiiTable()
        {
            DateTime fromDate, toDate;
            DataTable dt = new DataTable("Ola_HAWAII");


            if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                using (ProjectTrackerContainer dbContext = new ProjectTrackerContainer())
                {
                    var query = dbContext.vwProject
                        .Where(p => p.ReportDate >= fromDate
                                    && p.ReportDate <= toDate
                                    && !(bool)p.IsOlaRequest
                                    && !(bool)p.NotReportOla
                                    && (int)p.IsInternal == 0
                                    && p.IsPilot == "N"
                        );
                 

                    dt = PageUtility.ToDataTable(query.ToList());
                    hdnRowCount.Value = query.Count().ToString();
                }
            }

            return dt;
        }

        /// <summary>
        /// Obtains the list of projects and exports it into excel format
        /// for the purposes of RMATRIX (probably obsolete; refer to "Summary Reports" section instead).
        /// </summary>
        private void ExportTable()
        {
            DataTable dt;

            if (ddlGrantType.SelectedItem.Text.Equals("RMATRIX"))
            {
                dt = GetRmatrixTable();
                dt.TableName = "RMATRIX_Request";
            } else //if (ddlGrantType.SelectedItem.Text.Equals("Ola HAWAII"))
            {
                dt = GetOlaHawaiiTable();
                dt.TableName = "Ola_HAWAII_Request";
            }
            

            dt.Columns.Remove("InvestId");
            dt.Columns.Remove("Summary");
            dt.Columns.Remove("IsPilot");
            dt.Columns.Remove("PilotGrantName");
            dt.Columns.Remove("BiostatId");

            DataRow dr = dt.NewRow();

            dr[1] = "Total";
            dr[2] = dt.Rows.Count;

            dt.Rows.Add(dr);
            
            FileExport fileExport = new FileExport(this.Response);


            if (ddlGrantType.SelectedItem.Text.Equals("RMATRIX"))
            {
                fileExport.ExcelExport(dt, "RMATRIXRequest");
            }
            else //if (ddlGrantType.SelectedItem.Text.Equals("Ola HAWAII"))
            {
                fileExport.ExcelExport(dt, "OlaHAWAIIRequest");
            }
            
        }
        
        /// <summary>
        /// Exports the report in CSV format in which can be imported back
        /// into REDCap for RMATRIX's records (Point person: Ms. Grace Matsuura, GDO Data Coordinator).
        /// </summary>
        private void ExportCsv()
        {
            DateTime fromDate, toDate;

            if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);

                string reportGrant = ddlGrantType.SelectedItem.Text;

                var cmdText = reportGrant == "RMATRIX" ? "Rpt_RMATRIX_Monthly" : "Rpt_Ola_HAWAII_Monthly";

                try
                {
                    DataTable dt = ddlGrantType.SelectedItem.Text == "RMATRIX" ? new DataTable("tblRmatrix") : new DataTable("tblOlaHawaii");
                    using (SqlConnection sqlcon = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand(cmdText, sqlcon))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@StartDate", fromDate);
                            cmd.Parameters.AddWithValue("@EndDate", toDate);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }
                    }

                    FileExport fileExport = new FileExport(this.Response);
                    fileExport.CsvExport(dt, fromDate, download_token.Value, reportGrant);
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
            }
        }
    }
}