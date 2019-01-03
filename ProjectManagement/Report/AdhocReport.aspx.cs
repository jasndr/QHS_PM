using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ProjectManagement.Report
{
    /// <summary>
    /// @File: AdhocReport.aspx.cs
    /// @Author: Yang Rui
    /// @Summary: General Report
    /// 
    ///           General report for time entry, project, paper, and academic entries on the tracking system.
    ///           Users are able to create a report, in this section, for items entered in the "forms" section
    ///           of the Project Tracking System.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018JUL19 - Jason Delos Reyes  -  Added documentation for readibility. Also added COBRE-Infectious Diseases, OLA Hawaii,
    ///                                    and P30 UHCC to the list of grant choices for the Paper report.
    ///  2018NOV14 - Jason Delos Reyes  -  Edited Rpt_Academic stored procedure to pull the right academic events for the
    ///                                    specified window of dates.
    ///  2019JAN02 - Jason Delos Reyes  -  Added more documentation for readability.
    ///                                 -  Edited "Academic" General Report to display additional details not found in
    ///                                    the reports that Yang created.
    /// </summary>
    public partial class AdhocReport : System.Web.UI.Page
    {
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
        /// Displays the right fields depending on which type of report selected. 
        /// </summary>
        private void LoadStartScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();
            script.Append("<script>");

            if (Request["optionsRadios"].ToString() == "option1")
            {
                script.Append("if (!document.getElementById('optionsRadios1').checked){document.getElementById('optionsRadios1').checked=true;} ");
            }
            else if (Request["optionsRadios"].ToString() == "option2")
            {
                script.Append("if (!document.getElementById('optionsRadios2').checked){document.getElementById('optionsRadios2').checked=true;} ");
            }
            else if (Request["optionsRadios"].ToString() == "option3")
            {
                script.Append("if (!document.getElementById('optionsRadios3').checked){document.getElementById('optionsRadios3').checked=true;} ");
            }
            else
            {
                script.Append("if (!document.getElementById('optionsRadios4').checked){document.getElementById('optionsRadios4').checked=true;} ");
            }

            if (Request["optionsPubs"].ToString() == "abstract")
            {
                script.Append("if (!document.getElementById('optionsPubs2').checked){document.getElementById('optionsPubs2').checked=true;} ");
            }

            script.Append("</script>");
            ClientScript.RegisterStartupScript(GetType(), "Javascript", script.ToString());
        }

        /// <summary>
        /// Populates dropdown fields with corresponding selections.
        /// </summary>
        private void BindControl()
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();                

                //ddlBioStat
                //if (Page.User.IsInRole("Admin"))
                //{
                //    dropDownSource = context.BioStats
                //                    .OrderBy(b => b.Name)
                //                    .ToDictionary(c => c.Id, c => c.Name);

                //    PageUtility.BindDropDownList(ddlBiostat, dropDownSource, "--- Select All ---");
                //}
                //else
                //{
                //    var biostat = context.BioStats.FirstOrDefault(b => b.LogonId == Page.User.Identity.Name);

                //    if (biostat != null)
                //    {
                //        dropDownSource.Add(biostat.Id, biostat.Name);

                //        PageUtility.BindDropDownList(ddlBiostat, dropDownSource, null);
                //    }
                //}
                dropDownSource = context.BioStats
                                    .OrderBy(b => b.Name)
                                    .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlBiostat, dropDownSource, "--- Select All ---");

                dropDownSource = context.BioStats
                    //.Where(b => b.EndDate > currentDate)
                                .OrderBy(b => b.Name)
                                .ToDictionary(c => c.Id, c => c.Name);

                //PageUtility.BindDropDownList(ddlBiostat, dropDownSource, "--- Select All ---");
                PageUtility.BindDropDownList(ddlMember, dropDownSource, "--- Select All ---");

                dropDownSource = context.BiostatReports
                                 .Where(b => b.ReportType == "TimeEntry")
                                 .OrderBy(b => b.Id)
                                 .ToDictionary(c => c.Id, c => c.ReportName);

                PageUtility.BindDropDownList(ddlReport, dropDownSource, "--- Select Report ---");

                int biostatId;
                Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);
                if (biostatId > 0)
                {                
                    dropDownSource = context.ProjectBioStats
                                    .Where(p => p.BioStats_Id == biostatId)
                                    .OrderByDescending(d => d.Project.Id)
                                    .Select(x => new { x.Project.Id, FullName = (x.Project.Id + " " + x.Project.Title).Substring(0, 99) })
                                    .Distinct()
                                    .ToDictionary(c => c.Id, c => c.FullName);
                }
                else
                {
                    dropDownSource = context.Project2
                                    .Where(p => p.Id > 0)
                                    .OrderByDescending(d => d.Id)
                                    .Select(x => new { x.Id, FullName = (x.Id + " " + x.Title).Substring(0, 99) })
                                    .ToDictionary(c => c.Id, c => c.FullName);
                }
                PageUtility.BindDropDownList(ddlProject, dropDownSource, "--- Select All ---");
                                

                dropDownSource = context.PublishStatus
                                .Where(p => p.Id > 0)
                                .OrderBy(d => d.Id)
                                .ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlPaperStatus, dropDownSource, string.Empty);

                dropDownSource.Clear();
                dropDownSource.Add(new KeyValuePair<int, string>(1, "Small"));
                dropDownSource.Add(new KeyValuePair<int, string>(2, "Large"));
                PageUtility.BindDropDownList(ddlServiceType, dropDownSource, "--- Select All ---");

                dropDownSource.Clear();
                for (int i = 1; i <= 10; i++)
                {
                    dropDownSource.Add(new KeyValuePair<int, string>(i, i.ToString()));
                }
                PageUtility.BindDropDownList(ddlThreshold, dropDownSource, "default");

                dropDownSource = context.JabsomAffils
                            .Where(a => a.Type != "Other" && a.Type != "Degree" && a.Type != "Unknown" && a.Name != "Other")
                            .OrderBy(a => a.Name)
                            .ToDictionary(c => c.Id, c => c.Name);
                var affilsJson = dropDownSource.Select(x => new { Id = x.Key, Name = x.Value });
                textAreaAffil.Value = Newtonsoft.Json.JsonConvert.SerializeObject(affilsJson);

                dropDownSource = context.AcademicField.Where(f => f.Category == "AcademicType").OrderBy(f => f.DisplayOrder).ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlAcademicType, dropDownSource, string.Empty);
            }
        }

        /// <summary>
        /// Pulls reports based on selected option when the "Get Report" button is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Request["optionsRadios"].ToString() == "option1") //Time Entry
            {
                GetTimeEntryReport();
            }
            else if (Request["optionsRadios"].ToString() == "option2") //Project
            {
                GetProjectReport();
            }
            else if (Request["optionsRadios"].ToString() == "option3") //Paper
            {
                GetPaperReport();
            }
            else if (Request["optionsRadios"].ToString() == "option4") //Academic
            {
                GetAcademicReport();
            } 
        }

        /// <summary>
        /// NOT BEING USED. (Pulls separate report for RMATRIX Montly Reports; now uses independent report.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRmatrixMonthly_Click(object sender, EventArgs e)
        {
            //create csv file for download
            if (Page.User.IsInRole("Admin"))
            {
                DateTime dtFrom, dtTo;

                if (DateTime.TryParse(TextBoxInitialDate.Text, out dtFrom) && DateTime.TryParse(TextBoxCompleteDate.Text, out dtTo))
                {
                    string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(constr);

                    var cmdText = ddlRmatrix.Value == "Monthly" ? "Rpt_RMATRIX_Monthly" : "Rpt_RMATRIX_Summary";
                                       
                    try 
                    {  
                        DataTable dt = new DataTable("tblRmatrix");
                        using (SqlConnection sqlcon = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(cmdText, sqlcon))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@StartDate", dtFrom);
                                cmd.Parameters.AddWithValue("@EndDate", dtTo);

                                if (ddlRmatrix.Value == "Summary")
                                {
                                    var affiliation = txtPIAffil.Text;
                                    cmd.Parameters.AddWithValue("@Affiliation", affiliation);
                                }

                                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                {
                                    da.Fill(dt);
                                }
                            }
                        }

                        FileExport fileExport = new FileExport(this.Response);
                        if (ddlRmatrix.Value == "Summary")
                        {
                            fileExport.ExcelExport(dt, cmdText);
                        }
                        else
                        {
                            fileExport.CsvExport(dt, dtFrom, download_token.Value);
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

                        GetProjectReport();
                    }
                }
            }
        }
        
        /// <summary>
        /// Presents fields for PAPER (Abstracts/Manuscripts) report type in preparation for getting report.
        /// </summary>
        private void GetPaperReport()
        {	
            int pubType = 1, pubStatus, biostatId;
            DateTime pubFromDate = Convert.ToDateTime("2000-01-01");
            DateTime pubToDate = Convert.ToDateTime("2099-01-01");
            bool hasPMCID = false, noPMCID = false, citation = false;
            string grant, authorAffil; 	        

            Int32.TryParse(ddlPaperStatus.SelectedValue, out pubStatus);
            Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);

            DateTime tempDate;
            if (DateTime.TryParse(TextBoxPaperFromDate.Text, out tempDate))
            {
                pubFromDate = DateTime.Parse(TextBoxPaperFromDate.Text);
            }

            if (DateTime.TryParse(TextBoxPaperToDate.Text, out tempDate))
            {
                pubToDate = DateTime.Parse(TextBoxPaperToDate.Text);
            }

            if (chkPMCID.Checked)
            {
                hasPMCID = true;
            }

            if (chkNoPMCID.Checked)
            {
                noPMCID = true;
            }

            if (chkCitation.Checked)
            {
                citation = true;
            }


            StringBuilder sb = new StringBuilder();
            if (chkRMATRIX.Checked)
            {
                sb.Append("RMATRIX");
                sb.Append(";");
            }
            if (chkG12.Checked)
            {
                sb.Append("G12 BRIDGES");
                sb.Append(";");
            }
            if (chkINBRE.Checked)
            {
                sb.Append("INBRE");
                sb.Append(";");
            }
            if (chkMWest.Checked)
            {
                sb.Append("Mountain West");
                sb.Append(";");
            }
            if (chkCOBREid.Checked)
            {
                sb.Append("COBRE-Infectious Diseases");
                sb.Append(";");
            }
            if (chkOlaHawaii.Checked)
            {
                sb.Append("OLA Hawaii");
                sb.Append(";");
            }
            if (chkP30UHCC.Checked)
            {
                sb.Append("P30 UHCC");
                sb.Append(";");
            }

            grant = sb.ToString();

            authorAffil = txtAuthorAffil.Text;

            string rdlc, rdlPath;

            if (Request["optionsPubs"].ToString() == "paper")
            {
                pubType = 2;
                rdlc = "PaperManuscript.rdlc";
                rdlPath = Server.MapPath("~/Report/" + rdlc);
            }
            else
            {
                rdlc = "PaperAbstract.rdlc";
                rdlPath = Server.MapPath("~/Report/" + rdlc);
            }

            DataTable dt = GetPaper(pubType, pubFromDate, pubToDate, pubStatus, biostatId, grant, hasPMCID, noPMCID, citation, authorAffil);
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = rdlPath;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        }

        /// <summary>
        /// Obtains Paper RDLC report based on given paramaters entered in forms.
        /// </summary>
        /// <param name="pubType">Publication type - Paper or Abstract</param>
        /// <param name="pubFromDate">From Date - Starting date of range</param>
        /// <param name="pubToDate">To Date - Ending date of range</param>
        /// <param name="pubStatus">Publication Status - Submitted, Accepted, Published, or Not Accepted</param>
        /// <param name="biostatId">Biostatistician - QHS faculty or staff member for specific report</param>
        /// <param name="grant">Grants affiliated on paper - RMATRIX/G12 Bridges, INBRE, and/or Mountain West</param>
        /// <param name="hasPMCID">Has PMCID? - 0 for no PMCID, 1 for papers that contain a PMCID</param>
        /// <param name="noPMCID">No PMCID - 1 for papers with no PMCID only, 0 otherwise (all papers w or w/o PMCID)</param>
        /// <param name="citation">Citation Format - Check if results need to be presented in citation format</param>
        /// <param name="authorAffil">Author Affiliation - Search for specific author affiliation based on author affilation recorded in paper entry</param>
        /// <returns></returns>
        private DataTable GetPaper(int pubType, DateTime pubFromDate, DateTime pubToDate, int pubStatus, int biostatId, string grant, bool hasPMCID, bool noPMCID, bool citation, string authorAffil)
        {
            DataTable ResultsTable = new DataTable();

            SqlConnection conn = new SqlConnection(ConfigurationManager
                                 .ConnectionStrings["DefaultConnection"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("Rpt_Paper", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PubType", pubType);
                cmd.Parameters.AddWithValue("@PubFromDate", pubFromDate);
                cmd.Parameters.AddWithValue("@PubToDate", pubToDate);
                cmd.Parameters.AddWithValue("@PubStatus", pubStatus);
                cmd.Parameters.AddWithValue("@BiostatId", biostatId);
                cmd.Parameters.AddWithValue("@Grant", grant);
                cmd.Parameters.AddWithValue("@HasPMCID", hasPMCID);
                cmd.Parameters.AddWithValue("@NoPMCID", noPMCID);
                cmd.Parameters.AddWithValue("@Citation", citation);
                cmd.Parameters.AddWithValue("@AuthorAffil", authorAffil);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ResultsTable);
            }

            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return ResultsTable;
        }              

        /// <summary>
        /// Pulls PROJECT report based on the fields specified.
        /// </summary>
        private void GetProjectReport()
        {
            DateTime initialDate = Convert.ToDateTime("2000-01-01");
            DateTime completeDate = Convert.ToDateTime("2099-01-01");

            string /*leadBiostat="",*/ member="", piAffil="", grant="", pilotGrant="";
            bool onGoing = false, isInternal = false, isPilot = false;
            //int serviceType = 0;

            DateTime tempDate;
            if (DateTime.TryParse(TextBoxInitialDate.Text, out tempDate))
            {
                initialDate = DateTime.Parse(TextBoxInitialDate.Text);
            }
            if (DateTime.TryParse(TextBoxCompleteDate.Text, out tempDate))
            {
                completeDate = DateTime.Parse(TextBoxCompleteDate.Text);
            }

            int leadId, memberId, serviceTypeId, threshold;
            Int32.TryParse(ddlBiostat.SelectedValue, out leadId);
            Int32.TryParse(ddlMember.SelectedValue, out memberId);
            Int32.TryParse(ddlServiceType.SelectedValue, out serviceTypeId);
            Int32.TryParse(ddlThreshold.SelectedValue, out threshold);                      

            //if (leadId > 0)
            //{
            //    leadBiostat = ddlBiostat.SelectedItem.Text;
            //}

            if (memberId > 0)
            {
                member = ddlMember.SelectedItem.Text;
            }

            //if (serviceTypeId > 0)
            //{
            //    serviceType = ddlServiceType.SelectedItem.Text;
            //}

            piAffil = txtPIAffil.Text;
            grant = txtGrant.Text;

            if (chkOngoing.Checked)
            {
                onGoing = true;
            }

            if (chkInternal.Checked)
            {
                isInternal = true;
            }

            if (chkPilotGrant.Checked)
            {
                isPilot = true;
                pilotGrant = txtPilotGrant.Text;
            }

 	        //project details
            string rdlc = "ProjectDetail.rdlc";
            string rdlPath = Server.MapPath("~/Report/" + rdlc);

            DataTable dt = GetProjectDetail(initialDate, completeDate, leadId, member, piAffil, onGoing, isInternal, grant, isPilot, pilotGrant, serviceTypeId, threshold);
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = rdlPath;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
        }

        /// <summary>
        /// Pulls TIME ENTRY reports based on fields specified, differs type of time entry report specified.
        /// </summary>
        private void GetTimeEntryReport()
        {
            DateTime dtFrom, dtTo;
            int selectedReport;
            bool hasInternal = false;

            if (!DateTime.TryParse(TextBoxStartDate.Text, out dtFrom))
            {
                dtFrom = DateTime.Parse("2000-01-01");
            }

            if (!DateTime.TryParse(TextBoxEndDate.Text, out dtTo))
            {
                dtTo = DateTime.Parse("2099-01-01");
            }

            if (!Int32.TryParse(ddlReport.SelectedValue, out selectedReport))
            {
                selectedReport = 1;
            }

            if (chkInternalTimeEntry.Checked)
            {
                hasInternal = true;
            }

            int biostatId, projectId;
                      
            Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);
            Int32.TryParse(ddlProject.SelectedValue, out projectId);

            string rdlc = string.Empty;
                      
            switch (selectedReport)
            {
                case 1:
                    rdlc = "IndividualDailyHours.rdlc";
                    break;
                case 2:
                    rdlc = "IndividualSummaryProject.rdlc";
                    break;
                case 3:
                    rdlc = "ProjectSummary.rdlc";
                    break;
                case 4:
                    rdlc = "StaffSummary.rdlc";
                    break;
                case 5:
                    rdlc = "ProjectByBiostat.rdlc";
                    break;
                case 6:
                    rdlc = "MonthlyTrend.rdlc";
                    break;
                default:
                    break;
            }

            if (dtFrom <= dtTo && rdlc != string.Empty)
            {
                //ReportViewer1.LocalReport.ReportPath = AppDomain.CurrentDomain.BaseDirectory + "../Report/Report1.rdlc";
                string rdlPath = Server.MapPath("~/Report/" + rdlc);

                DataTable dt = GetTimeEntry(dtFrom, dtTo, selectedReport, biostatId, projectId, hasInternal);
                ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.ReportPath = rdlPath;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            }
            
        }

        /// <summary>
        /// Obtains Time Entry Report from stored procedure.
        /// </summary>
        /// <param name="startDate">Specified earliest date possible for pulled time entry hours.</param>
        /// <param name="endDate">Specified latest date possible for pulled time entry hours.</param>
        /// <param name="reportId">Type of time entry report need to be pulled (Individual Daily Hours, etc.)/ </param>
        /// <param name="biostatId">QHS Faculty/Staff, if specified.</param>
        /// <param name="projectId">Project ID, if specified.</param>
        /// <param name="hasInternal">Internal Projects Included (excluded by default).</param>
        /// <returns></returns>
        private DataTable GetTimeEntry(DateTime startDate, DateTime endDate, int reportId, int biostatId, int projectId, bool hasInternal)
        {
            DataTable ResultsTable = new DataTable();

            SqlConnection conn = new SqlConnection(ConfigurationManager
                                 .ConnectionStrings["DefaultConnection"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("Rpt_TimeEntry", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                cmd.Parameters.AddWithValue("@BiostatId", biostatId);
                cmd.Parameters.AddWithValue("@ReportId", reportId);
                cmd.Parameters.AddWithValue("@HasInternal", hasInternal);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ResultsTable);
            }

            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return ResultsTable;
        }


        /// <summary>
        /// Obtains Project report from stored procedure based on specified fields.
        /// </summary>
        /// <param name="initialDate">Earliest possible date of entry.</param>
        /// <param name="completeDate">Latest possible date of entry.</param>
        /// <param name="leadBiostatId">Lead QHS Faculty/Staff, if specified.</param>
        /// <param name="member">Additional Member(s), if specified.</param>
        /// <param name="piAffil">PI Affiliation, if specified.</param>
        /// <param name="onGoing">Determines whether or not to pull ongoing projects.</param>
        /// <param name="isInternal">Pulls internal projects as well if specified.</param>
        /// <param name="grant">NOT BEING USED. (Determines if pilot grants are needed to be pulled).</param>
        /// <param name="isPilot">Pulls pilot grants, if specified.</param>
        /// <param name="pilotGrant">NOT BEING USED. (Pulls name of pilot grant specified).</param>
        /// <param name="serviceTypeId">Scope (Large vs. small).</param>
        /// <param name="threshold">Threshold parameter as specified.</param>
        /// <returns></returns>
        private DataTable GetProjectDetail(DateTime initialDate, DateTime completeDate, int leadBiostatId, string member, string piAffil, bool onGoing, bool isInternal, string grant, bool isPilot, string pilotGrant, int serviceTypeId, int threshold)
        {
            DataTable ResultsTable = new DataTable();

            SqlConnection conn = new SqlConnection(ConfigurationManager
                                 .ConnectionStrings["DefaultConnection"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("Rpt_Project_Detail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InitialDate", initialDate);
                cmd.Parameters.AddWithValue("@CompleteDate", completeDate);
                cmd.Parameters.AddWithValue("@LeadBiostatId", leadBiostatId);
                cmd.Parameters.AddWithValue("@Member", member);
                cmd.Parameters.AddWithValue("@PIAffil", piAffil);
                cmd.Parameters.AddWithValue("@Ongoing", onGoing);
                cmd.Parameters.AddWithValue("@IsInternal", isInternal);
                cmd.Parameters.AddWithValue("@Grant", grant);
                cmd.Parameters.AddWithValue("@IsPilot", isPilot);
                cmd.Parameters.AddWithValue("@PilotGrant", pilotGrant);
                cmd.Parameters.AddWithValue("@ServiceTypeId", serviceTypeId);

                if (threshold > 0)
                {
                    cmd.Parameters.AddWithValue("@Threshold", threshold);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ResultsTable);
            }

            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return ResultsTable;
        }

        /// <summary>
        /// Obtains Academic report with otpional start/end dates and academic type specified.
        /// </summary>
        private void GetAcademicReport()
        {
            DateTime dtFrom, dtTo;
            int academicType = 0, biostatId = 0;

            if (!DateTime.TryParse(txtAcademicFrom.Text, out dtFrom))
            {
                dtFrom = DateTime.Parse("2000-01-01");
            }

            if (!DateTime.TryParse(txtAcademicTo.Text, out dtTo))
            {
                dtTo = DateTime.Parse("2099-01-01");
            }

            Int32.TryParse(ddlAcademicType.SelectedValue, out academicType);
            Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);

            if (dtFrom <= dtTo)
            {
                DataTable dt = GetAcademic(dtFrom, dtTo, academicType, biostatId);
                ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/Academic.rdlc");
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            }
        }

        /// <summary>
        /// Obtains ACADEMIC report from stored procedure.
        /// </summary>
        /// <param name="dtFrom">Earliest possible academic entry.</param>
        /// <param name="dtTo">Latest possible academic entry.</param>
        /// <param name="academicType">Academic Event Type (Seminar, Teaching, etc.).</param>
        /// <param name="biostatId">QHS Faculty/Staff member involved, if specified.</param>
        /// <returns></returns>
        private DataTable GetAcademic(DateTime dtFrom, DateTime dtTo, int academicType, int biostatId)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(ConfigurationManager
                                 .ConnectionStrings["DefaultConnection"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("Rpt_Academic2", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", dtFrom);
                cmd.Parameters.AddWithValue("@ToDate", dtTo);
                cmd.Parameters.AddWithValue("@AcademicTypeId", academicType);
                cmd.Parameters.AddWithValue("@BiostatId", biostatId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            catch (Exception ex)
            {
                //Response.Write(ex.ToString());
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return dt;
        }
    }
}