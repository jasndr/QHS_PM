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
    public partial class Project : System.Web.UI.Page
    {
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

        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            DataTable dt = GetProjectTable();

            rptProjectSummary.DataSource = dt;
            rptProjectSummary.DataBind();

            hdnRowCount.Value = dt.Rows.Count.ToString();

            Control footerTemplate = rptProjectSummary.Controls[rptProjectSummary.Controls.Count - 1].Controls[0];
            Label lblFooter = footerTemplate.FindControl("lblTotal") as Label;
            lblFooter.Text = hdnRowCount.Value;

            decimal phdHrs = 0.0m, msHrs = 0.0m;
            foreach (DataRow row in dt.Rows)
            {
                phdHrs += Convert.ToDecimal(row["PhdHrs"].ToString());
                msHrs += Convert.ToDecimal(row["MsHrs"].ToString());
            }

            Label lblPhdHrs = footerTemplate.FindControl("lblPhdHrs") as Label;
            lblPhdHrs.Text = phdHrs.ToString();

            Label lblMsHrs = footerTemplate.FindControl("lblMsHrs") as Label;
            lblMsHrs.Text = msHrs.ToString();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = GetProjectTable();
            dt.TableName = "Project";

            DataRow dr = dt.NewRow();

            dr[1] = "Total";
            dr[2] = dt.Rows.Count;

            decimal phdHrs = 0.0m, msHrs = 0.0m;
            foreach (DataRow row in dt.Rows)
            {
                phdHrs += Convert.ToDecimal(row["PhdHrs"].ToString());
                msHrs += Convert.ToDecimal(row["MsHrs"].ToString());
            }

            dr[16] = phdHrs;
            dr[17] = msHrs;

            dt.Rows.Add(dr);

            FileExport fileExport = new FileExport(this.Response);
            fileExport.ExcelExport(dt, "Project");
        }

        private DataTable GetProjectTable()
        {
            DataTable dt = new DataTable("tblProject");

            int piId = 0, affilId = 0, piStatusId = 0;

            if(txtPIName.Value != string.Empty)
                Int32.TryParse(txtPIId.Value, out piId);

            if (txtAffiliation.Value != string.Empty)
                Int32.TryParse(txtAffilId.Value, out affilId);

            Int32.TryParse(ddlPIStatus.SelectedValue, out piStatusId);

            int phdId = 0, msId = 0, healthValue = 0, grantValue = 0, isProject = 1, isBiostat = 1, creditTo = 1;

            Int32.TryParse(ddlPhd.SelectedValue, out phdId);
            Int32.TryParse(ddlMs.SelectedValue, out msId);
            Int32.TryParse(ddlHealthData.SelectedValue, out healthValue);
            Int32.TryParse(ddlGrant.SelectedValue, out grantValue);

            isProject = chkProject.Checked ? 1 : 0;

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

            DateTime fromDate, toDate;
            if (DateTime.TryParse(txtFromDate.Text, out fromDate) && DateTime.TryParse(txtToDate.Text, out toDate))
            {
                dt = GetProjectTable(fromDate, toDate, phdId, msId, isProject, isBiostat, creditTo, piId, piStatusId, affilId, healthValue, grantValue);
            }

            return dt;
        }

        private DataTable GetProjectTable(DateTime fromDate, DateTime toDate, int phdId, int msId, int isProject, int isBiostat, 
            int creditTo, int piId, int piStatusId, int affilId, int healthValue, int grantValue)
        {
            DataTable dt = new DataTable("tblProject");

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            try
            {

                using (SqlConnection sqlcon = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Rpt_Project_Summary", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                        cmd.Parameters.AddWithValue("@ToDate", toDate);
                        cmd.Parameters.AddWithValue("@PhdId", phdId);
                        cmd.Parameters.AddWithValue("@MsId", msId);
                        cmd.Parameters.AddWithValue("@IsProject", isProject);
                        cmd.Parameters.AddWithValue("@IsBiostat", isBiostat);
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