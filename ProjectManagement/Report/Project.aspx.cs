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
    /// @File: Project.aspx.cs
    /// @Frontend: Project.aspx
    /// @Author: Yang Rui
    /// @Summary: Project Report
    /// 
    ///           Report of projects from a specified time period, optionally closing in by specific PI information,
    ///           affiliation, use of HealthData, and/or specific grants.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018OCT26 - Jason Delos Reyes  -  Begin adding documentation to form.
    ///                                 -  Edit file export to display headers.
    ///  2018OCT29 - Jason Delos Reyes  -  Updated stored procedure to add department affiliation in
    ///                                    the Project2 Form in addition to pulling from 
    ///                                    PI affiliation.
    ///  2018OCT30 - Jason Delos Reyes  -  Added style to "project report" view to be able to see the entire report
    ///                                    using a scrollable window instead of the unsightly overlap of tables.
    ///  2019FEB06 - Jason Delos Reyes  -  Changed "Grant" to "Funding Source" in project report.
    ///                                 -  Added "RMATRIX" and "Ola Hawaii" request for resources button to pull 
    ///                                    report of projects with those checkmarks.
    ///  2019MAR20 - Jason Delos Reyes  -  Edited Project form to accomodate change to letter
    ///                                    of support reference in new project form.
    ///  2019MAY10 - Jason Delos Reyes  -  Created "Rpt_Project_Summary2a" to be able to pull projects that have 
    ///                                    "Submitted to RMATRIX" and "Submitted to Ola HAWAII" checked.
    /// </summary>
    public partial class Project : System.Web.UI.Page
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

                    var query = dbContext.BioStats.Where(b => b.Id > 0);

                    dropDownSource = query
                                    .Where(b => b.Type == "phd")
                                    .OrderByDescending(b => b.EndDate)
                                    .ThenBy(b => b.Name)
                                    .ToDictionary(b => b.Id, b => b.Name );

                    PageUtility.BindDropDownList(ddlPhd, dropDownSource, " --- Select --- ");

                    dropDownSource = query
                                    .Where(b => b.Type == "master")
                                    .OrderByDescending(b => b.EndDate)
                                    .ThenBy(b => b.Name)
                                    .ToDictionary(b => b.Id, b => b.Name);

                    PageUtility.BindDropDownList(ddlMs, dropDownSource, " --- Select --- ");

                    dropDownSource = dbContext.InvestStatus
                                    .OrderBy(d => d.DisplayOrder)
                                    .ToDictionary(b => b.Id, b => b.StatusValue);

                    PageUtility.BindDropDownList(ddlPIStatus, dropDownSource, " --- Select --- ");

                    dropDownSource = dbContext.ProjectField
                                    .Where(g => g.GroupName == "HealthData")
                                    .OrderBy(d => d.DisplayOrder)
                                    .ToDictionary(b => b.BitValue, b => b.Name);

                    PageUtility.BindDropDownList(ddlHealthData, dropDownSource, " --- Select --- ");

                    dropDownSource = dbContext.ProjectField
                                    .Where(g => g.GroupName == "Grant")
                                    .OrderBy(d => d.DisplayOrder)
                                    .ToDictionary(b => b.BitValue, b => b.Name);

                    PageUtility.BindDropDownList(ddlGrant, dropDownSource, " --- Select --- ");

                    var affil = dbContext.JabsomAffils
                                .Where(a => a.Type != "Other" && a.Type != "Degree" && a.Type != "Unknown" && a.Name != "Other")
                                .OrderBy(a => a.Id)
                                .Select(x => new { Id = x.Id, Name = x.Name });

                    textAreaAffil.Value = Newtonsoft.Json.JsonConvert.SerializeObject(affil);

                    var piName = dbContext.Invests
                        .Where(i => i.Id > 0)
                        .Select(x => new {Id = x.Id, Name = x.FirstName +" " + x.LastName});

                    textAreaPI.Value = Newtonsoft.Json.JsonConvert.SerializeObject(piName);
                }
            }
            
        }

        /// <summary>
        /// Produces the report in the web form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            divProject.Visible = true;

            DataTable dt = GetProjectTable();

            rptProjectSummary.DataSource = dt;
            rptProjectSummary.DataBind();

            hdnRowCount.Value = dt.Rows.Count.ToString();

            //Control footerTemplate = rptProjectSummary.Controls[rptProjectSummary.Controls.Count - 1].Controls[0];
            //Label lblFooter = footerTemplate.FindControl("lblTotal") as Label;
            //lblFooter.Text = hdnRowCount.Value + " Projects";

            //decimal phdHrs = 0.0m, msHrs = 0.0m;
            //foreach (DataRow row in dt.Rows)
            //{
            //    phdHrs += Convert.ToDecimal(row["PhdHrs"].ToString());
            //    msHrs += Convert.ToDecimal(row["MsHrs"].ToString());
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
            //dr[2] = dt.Rows.Count;

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

            int piId = 0, affilId = 0, piStatusId = 0;

            if (txtPIName.Value != string.Empty)
            {
                Int32.TryParse(txtPIId.Value, out piId);
                ReportType = ReportType + " " + txtPIName.Value;
            }

            if (txtAffiliation.Value != string.Empty)
            {
                Int32.TryParse(txtAffilId.Value, out affilId);
                ReportType = ReportType + " " + txtAffiliation.Value;
            }



            Int32.TryParse(ddlPIStatus.SelectedValue, out piStatusId);

            int phdId = 0, msId = 0, healthValue = 0, grantValue = 0, isProject = 1, isBiostat = 1, creditTo = 1,
                isRmatrixRequest = 0, isOlaRequest = 0, submitRMATRIX = 1, submitOlaHAWAII = 1, letterOfSupport = 0;

            Int32.TryParse(ddlPhd.SelectedValue, out phdId);
            Int32.TryParse(ddlMs.SelectedValue, out msId);
            Int32.TryParse(ddlHealthData.SelectedValue, out healthValue);
            Int32.TryParse(ddlGrant.SelectedValue, out grantValue);

            if (healthValue > 0) ReportType = ReportType + " " + ddlHealthData.SelectedItem.Text;
            if (grantValue > 0) ReportType = ReportType + " " + ddlGrant.SelectedItem.Text;

            isProject = chkProject.Checked ? 1 : 0;

            isRmatrixRequest = chkRmatrixRequest.Checked ? 1 : 0;
            isOlaRequest = chkOlaRequest.Checked ? 1 : 0;

            submitRMATRIX = chkSubmitToRMATRIX.Checked ? 0 : 1;
            submitOlaHAWAII = chkSubmitToOlaHAWAII.Checked ? 0 : 1;

            letterOfSupport = chkLetterOfSupport.Checked ? 1 : 0;

            if ((chkBiostat.Checked && chkBioinfo.Checked) || (!chkBiostat.Checked && !chkBioinfo.Checked))
                isBiostat = -1;
            else
            {
                isBiostat = chkBiostat.Checked ? 1 : 2;
            }


            if (!chkCreditToBiostat.Checked && !chkCreditToBioinfo.Checked && !chkCreditToBoth.Checked)
                creditTo = -1;
            else
            {
                creditTo = chkCreditToBoth.Checked ? 3 : chkCreditToBioinfo.Checked ? 2 : 1;
            }

            DateTime fromDate = DateTime.TryParse(txtFromDate.Text, out fromDate) ? DateTime.Parse(txtFromDate.Text)
                                                                                  : new DateTime(2000, 01, 01);
            DateTime toDate = DateTime.TryParse(txtToDate.Text, out toDate) ? DateTime.Parse(txtToDate.Text)
                                                                            : DateTime.Now;/*DateTime(2099, 01, 01)*/
                                                                                                     

            //DateTime fromDate = new DateTime(2000,01,01), toDate = new DateTime(2099,01,01);
            //if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            //{
            dt = GetProjectTable(fromDate, toDate, phdId, msId, isProject, isBiostat, isRmatrixRequest, isOlaRequest, submitRMATRIX, submitOlaHAWAII, letterOfSupport, creditTo, piId, piStatusId, affilId, healthValue, grantValue);

            FromDate = fromDate.ToString("MM/dd/yyyy");
            ToDate = toDate.ToString("MM/dd/yyyy");
            //}

            return dt;
        }

        /// <summary>
        /// Parses the database for the table information with the given information.
        /// </summary>
        /// <param name="fromDate">Starting date of range.</param>
        /// <param name="toDate">Ending date of range.</param>
        /// <param name="phdId">Biostat ID of Phd faculty.</param>
        /// <param name="msId">Biostat ID of MS staff.</param>
        /// <param name="isProject">Pulls either projects (1) or consultations (0).</param>
        /// <param name="isBiostat">Pulls either Biostatistics or Bioinformatics projects.</param>
        /// <param name="isRmatrixRequest">Pulls projects that have "RMATRIX Request for Resources" checked.</param>
        /// <param name="isOlaRequest">Pulls projects that have "Ola Hawaii Request for Resources" checked.</param>
        /// <param name="submitRMATRIX">Pulls projects that have "Submit to RMATRIX" checked.</param>
        /// <param name="submitOlaHAWAII">Pulls projects that have "Submit to Ola HAWAII" checked.</param>
        /// <param name="letterOfSupport">Pulls projects that have "Letter of Support" checked for Service Type.</param>
        /// <param name="creditTo">Credit to either Biostatistics or Bioinformatics Cores, or both.</param>
        /// <param name="piId">ID of specified PI (Investigator).</param>
        /// <param name="piStatusId">Specified PI status ID.</param>
        /// <param name="affilId">Affiliation of PI.</param>
        /// <param name="healthValue">Health Database specified.</param>
        /// <param name="grantValue">Funding Source specified.</param>
        /// <returns></returns>
        private DataTable GetProjectTable(DateTime fromDate, DateTime toDate, int phdId, int msId, int isProject, int isBiostat, 
            int isRmatrixRequest, int isOlaRequest, int submitRMATRIX, int submitOlaHAWAII, int letterOfSupport, int creditTo, int piId, int piStatusId, int affilId, int healthValue, int grantValue)
        {
            DataTable dt = new DataTable("tblProject");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            try
            {

                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Rpt_Project_Summary2a", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                        cmd.Parameters.AddWithValue("@ToDate", toDate);
                        cmd.Parameters.AddWithValue("@PhdId", phdId);
                        cmd.Parameters.AddWithValue("@MsId", msId);
                        cmd.Parameters.AddWithValue("@IsProject", isProject);
                        cmd.Parameters.AddWithValue("@IsBiostat", isBiostat);
                        cmd.Parameters.AddWithValue("@IsRmatrixRequest", isRmatrixRequest);
                        cmd.Parameters.AddWithValue("@IsOlaRequest", isOlaRequest);
                        cmd.Parameters.AddWithValue("@SubmitToRMATRIX", submitRMATRIX);
                        cmd.Parameters.AddWithValue("@SubmitToOlaHAWAII", submitOlaHAWAII);
                        cmd.Parameters.AddWithValue("@LetterOfSupport", letterOfSupport);
                        cmd.Parameters.AddWithValue("@CreditTo", creditTo);
                        cmd.Parameters.AddWithValue("@PIId", piId);
                        cmd.Parameters.AddWithValue("@PIStatusId", piStatusId);
                        cmd.Parameters.AddWithValue("@AffilId", affilId);
                        cmd.Parameters.AddWithValue("@HealthBitValue", healthValue);
                        cmd.Parameters.AddWithValue("@GrantBitValue", grantValue);

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

    }
}