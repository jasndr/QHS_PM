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
    public partial class RmatrixMonthly : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnRowCount.Value = "0";
        }

        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            rptRmatrixMonthly.DataSource = GetRmatrixTable();
            rptRmatrixMonthly.DataBind();

            Control footerTemplate = rptRmatrixMonthly.Controls[rptRmatrixMonthly.Controls.Count - 1].Controls[0];
            Label lblFooter = footerTemplate.FindControl("lblTotal") as Label;
            lblFooter.Text = hdnRowCount.Value;

            //DateTime reportDate;
            //DateTime fromDate, toDate;

            //if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            //{
            //    //int year = reportDate.Year;
            //    //int month = reportDate.Month;
            //    //var firstDay = new DateTime(year, month, 1);
            //    //var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            //    using (ProjectTrackerContainer dbContext = new ProjectTrackerContainer())
            //    {
            //        var query = dbContext.vwProject
            //            .Where(p => p.ReportDate >= fromDate
            //                        && p.ReportDate <= toDate
            //            );

            //        if (chkRmatrix.Checked)
            //        {
            //            query = query.Where(p => (bool) p.IsRmatrix);
            //        }
            //        else
            //        {
            //            query = query.Where(p => !(bool) p.IsRmatrix
            //                        && !(bool) p.NotReportRmatrix
            //                        && (int) p.IsInternal == 0
            //            );
            //        }

            //        DataTable dt = PageUtility.ToDataTable(query.ToList());
                   
            //        rptRmatrixMonthly.DataSource = dt;
            //        rptRmatrixMonthly.DataBind();

            //        hdnRowCount.Value = query.Count().ToString();
                   
            //    }

               
            //}
        }

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

        private void ExportTable()
        {
            DataTable dt = GetRmatrixTable();
            dt.TableName = "RMATRIX_Request";

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
            fileExport.ExcelExport(dt, "RMATRIXRequest");
        }
        
        private void ExportCsv()
        {
            DateTime fromDate, toDate;

            if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);

                var cmdText = "Rpt_RMATRIX_Monthly";

                try
                {
                    DataTable dt = new DataTable("tblRmatrix");
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
                    fileExport.CsvExport(dt, fromDate, download_token.Value);
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