using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement
{
    /// @File: ProjectList.aspx.cs
    /// @FrontEnd: ProjectList.aspx
    /// @Author: Yang Rui
    /// @Summary: List of pending *new* investigators and projects entered into the tracking system that need to be reviewed
    ///           by the QHS Tracking Team before allowing users to begin entering hours for a project currently under review.
    ///           This page is only visible to the "Admin" user group.  An email already alerts users about these pending items,
    ///           though the ProjectList page can be used to keep track of those that have not yet been approved by the tracking
    ///           team already.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018JUN25 - Jason Delos Reyes  -  Added comments/documentation for easier legibility and
    ///                                    easier data structure view and management.
    ///  2019MAY17 - Jason Delos Reyes  -  Reversed the order of Project Review list to list them last-in first-out.                                  
    ///   
    public partial class ProjectList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }

            // "Approved (Reviewed)" Project List.
            // *No longer being used due to a redundancy
            //  of an approval list if you only need a "pending" list.*
            if (!Page.User.Identity.Name.Equals("yrui"))
            {
                divProjectList.Style["display"] = "none";
            }
        }

        /// <summary>
        /// Initialize empty gridview for list of pending investigators and/or projects to review by the tracking team.
        /// </summary>
        /// <param name="gv">Given gridview project.</param>
        private void GridViewBindEmpty(GridView gv)
        {
            DataTable dt = new DataTable("emptyTable");

            dt.Columns.Add("Id", System.Type.GetType("System.String"));
            dt.Columns.Add("Title", System.Type.GetType("System.String"));
            dt.Columns.Add("FirstName", System.Type.GetType("System.String"));
            dt.Columns.Add("LastName", System.Type.GetType("System.String"));
            dt.Columns.Add("InitialDate", System.Type.GetType("System.String"));
            dt.Columns.Add("", System.Type.GetType("System.String"));

            DataRow row = dt.NewRow();
            //row[0] = "";
            //row[1] = "";
            //row[2] = "";
            //row[3] = "";
            //row[4] = "";
            //row[5] = "";

            dt.Rows.Add(row);
            gv.DataSource = dt;
            gv.DataBind();

            gv.Rows[0].Cells.Clear();
        }
      
        /// <summary>
        /// Obtains a list newly-entered investigators and projects and binds them into a separate tables
        /// for review by the QHS Tracking Team.
        /// </summary>
        private void BindControl()
        {
            GridViewBindEmpty(GridViewProject); 

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                // List of pending Investigators to review by QHS Tracking Team.
                var query = context.Invests
                           .Join(context.InvestStatus, i => i.InvestStatusId, s => s.Id,
                                   (i, s) => new { i.Id, i.FirstName, i.LastName, i.Email, i.Phone, s.StatusValue, i.IsApproved })
                           .Where(d => d.Id > 0 && !d.IsApproved)
                           .OrderByDescending(d => d.Id);

                // Bind list of pending Investigators to table.
                GridViewPI.DataSource = query.ToList();
                GridViewPI.DataBind();

                // List of pending Projects to review by QHS Tracking Team.
                var pending = context.Project2
                            .Join(context.Invests, p => p.Invests.Id, i => i.Id, ((p, i) => new { p.Id, p.Title, p.InitialDate, p.IsApproved, i.FirstName, i.LastName}))
                            .Where(a => a.IsApproved == false && a.Id > 0)
                            .OrderByDescending(d=>d.Id);

                // Binds list of pending Projects to table.
                GridViewPending.DataSource = pending.ToList();
                GridViewPending.DataBind();

                //var project = context.Projects
                //            .Join(context.Invests, p => p.InvestId, i => i.Id, ((p, i) => new { p.Id, p.Title, p.InitialDate, p.IsApproved, i.FirstName, i.LastName }))
                //            .Where(a => a.IsApproved == true)
                //            .OrderByDescending(p => p.Id)
                //            .Take(10);

                //GridViewProject.DataSource = project.ToList();
                //GridViewProject.DataBind();

                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                //ddlInvestor
                dropDownSource = context.Invests
                                .OrderBy(d => d.FirstName).ThenBy(l => l.LastName)
                                .Select(x => new { x.Id, Name = x.FirstName + " " + x.LastName })
                                .ToDictionary(c => c.Id, c => c.Name);

                PageUtility.BindDropDownList(ddlInvestor, dropDownSource, "------ Select All ------");

                DataTable clientRqstTable = GetClientRqstAll();

                rptClientRqst.DataSource = clientRqstTable;
                rptClientRqst.DataBind();
            }
        }

        /// <summary>
        /// (Currently NOT being used). Save button to save "Client Request" box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (chkCompleted.Checked)
            {
                using (ProjectTrackerContainer db = new ProjectTrackerContainer())
                {
                    int rqstId = 0;
                    int.TryParse(lblClientRqstId.Text, out rqstId);

                    if (rqstId > 0)
                    {
                        var rqst = db.ClientRequest.FirstOrDefault(c => c.Id == rqstId);
                        rqst.RequestStatus = "Completed";
                        db.SaveChanges();
                    }
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(false), false);

            DataTable clientRqstTable = GetClientRqstAll();

            rptClientRqst.DataSource = clientRqstTable;
            rptClientRqst.DataBind();
        }

        /// <summary>
        /// (Currently NOT being used). Ability to edit "Client Request" form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptClientRqst_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                lblClientRqstId.Text = ((Button)e.CommandSource).CommandArgument;

                int rqstId = 0;
                int.TryParse(lblClientRqstId.Text, out rqstId);

                if (rqstId > 0)
                {
                    ClientRequest rqst = GetClientRequestById(rqstId);

                    if (rqst != null)
                    {
                        BindEditModal(rqst);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "ModalScript", PageUtility.LoadEditScript(true), false);
                    }
                }
            }
        }

        /// <summary>
        /// (Currently NOT being used). Initiates the client request table with the information from the database.
        /// </summary>
        /// <param name="rqst"></param>
        private void BindEditModal(ClientRequest rqst)
        {
            DateTime dt;
            if (rqst != null)
            {
                lblFirstName.Text = rqst.FirstName;
                lblLastName.Text = rqst.LastName;
                lblDegree.Text = rqst.Degree;
                lblEmail.Text = rqst.Email;
                lblPhone.Text = rqst.Phone;
                lblDept.Text = rqst.Department;
                lblInvestStatus.Text = rqst.InvestStatus;
                lblProjectTitle.Text = rqst.ProjectTitle;
                lblProjectSummary.Text = rqst.ProjectSummary;
                lblStudyArea.Text = rqst.StudyArea;
                lblServiceType.Text = rqst.ServiceType;
                lblDueDate.Text = DateTime.TryParse(rqst.DueDate.ToString(), out dt) ? dt.ToShortDateString() : "";
                lblPreferBiostat.Text = rqst.PreferBiostat;

                if (rqst.RequestStatus == "Completed")
                {
                    chkCompleted.Checked = true;
                }
                else
                {
                    chkCompleted.Checked = false;
                }
            }
        }

        /// <summary>
        /// (Currently NOT being used). Obtains "Client Request" entries based on request ID provided.
        /// </summary>
        /// <param name="rqstId">Referred Client Request Id.</param>
        /// <returns></returns>
        private ClientRequest GetClientRequestById(int rqstId)
        {
            ClientRequest myRqst;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                myRqst = db.ClientRequest.FirstOrDefault(t => t.Id == rqstId);          
      
                if (myRqst != null)
                {
                    var studyAreas = db.HiPrograms
                                    .Where(s => s.isStudyArea);

                    StringBuilder sb = new StringBuilder();
                    String[] studyArray = myRqst.StudyArea.Split(';');

                    foreach (string studyCode in studyArray)
                    {
                        int studyId = 0;
                        Int32.TryParse(studyCode, out studyId);

                        if (studyId > 0)
                        {
                            var studyArea = studyAreas.FirstOrDefault(d => d.Id == studyId);

                            if (studyArea != null)
                            {
                                sb.AppendFormat("{0}; ", studyArea.Name);
                            }
                        }
                    }

                    myRqst.StudyArea = sb.ToString();

                    sb.Clear();

                    var serviceTypes = db.ServiceTypes;
                    String[] serviceArray = myRqst.ServiceType.Split(';');
                    foreach (string serviceCode in serviceArray)
                    {
                        int serviceId = 0;
                        Int32.TryParse(serviceCode, out serviceId);

                        if (serviceId > 0)
                        {
                            var serviceType = serviceTypes.FirstOrDefault(d => d.Id == serviceId);

                            if (serviceType != null)
                            {
                                sb.AppendFormat("{0}; ", serviceType.Name);
                            }
                        }
                    }
                    myRqst.ServiceType = sb.ToString();

                }
            }

            return myRqst;
        }

        /// <summary>
        /// (Currently NOT being used). Obtains all "Client Request" entries.
        /// </summary>
        /// <returns></returns>
        private DataTable GetClientRqstAll()
        {
            DataTable dt = new DataTable("clientRqstTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("FirstName", System.Type.GetType("System.String"));
            dt.Columns.Add("LastName", System.Type.GetType("System.String"));
            dt.Columns.Add("ProjectTitle", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateDate", System.Type.GetType("System.String"));
            dt.Columns.Add("Status", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var query = db.ClientRequest
                    .Select(t => new { t.Id, t.FirstName, t.LastName, t.ProjectTitle, t.CreationDate, t.RequestStatus })
                    .OrderByDescending(t => t.Id);
                

                foreach (var p in query.OrderByDescending(p => p.Id).ToList())
                {
                    DataRow row = dt.NewRow();

                    row[0] = p.Id;
                    row[1] = p.FirstName;
                    row[2] = p.LastName;
                    row[3] = p.ProjectTitle;
                    row[4] = p.CreationDate != null ? Convert.ToDateTime(p.CreationDate).ToShortDateString() : "";
                    row[5] = p.RequestStatus;

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static List<ProjectPI> GetProjectList(int piId, int pagenumber)
        {           
            List<ProjectPI> lstProject = new List<ProjectPI>();

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                if (piId > 0)
                {
                    var projectPI = context.Projects
                            .Join(context.Invests, p => p.InvestId, i => i.Id, ((p, i) => new { p.Id, p.Title, p.InitialDate, p.IsApproved, i.FirstName, i.LastName, p.InvestId }))
                            .Where(a => a.IsApproved == true && a.InvestId == piId)
                            .OrderByDescending(p => p.Id).ToList()
                            .Skip(10 * (--pagenumber))
                            .Take(10);

                    foreach (var p in projectPI.ToList())
                    {
                        lstProject.Add(new ProjectPI()
                        {
                            Id = p.Id,
                            Title = p.Title,
                            PIFirstName = p.FirstName,
                            PILastName = p.LastName,
                            InitialDate = p.InitialDate.ToShortDateString()
                        });
                    }
                }
                else
                {
                    var project = context.Projects
                                .Join(context.Invests, p => p.InvestId, i => i.Id, ((p, i) => new { p.Id, p.Title, p.InitialDate, p.IsApproved, i.FirstName, i.LastName }))
                                .Where(a => a.IsApproved == true)
                                .OrderByDescending(p => p.Id).ToList()
                                .Skip(10 * (--pagenumber))
                                .Take(10);

                    foreach (var p in project.ToList())
                    {
                        lstProject.Add(new ProjectPI()
                             {
                                 Id = p.Id,
                                 Title = p.Title,
                                 PIFirstName = p.FirstName,
                                 PILastName = p.LastName,
                                 InitialDate = p.InitialDate.ToShortDateString()
                             });
                    }
                }
            }

            return lstProject;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public static int GetTotalPageCount(int piId)
        {
            int count = 0;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                if (piId > 0)
                {
                    count = context.Projects.Where(p => p.IsApproved == true && p.InvestId == piId).Count();
                }
                else
                    count = context.Projects.Where(p => p.IsApproved == true).Count();
            }

            return Convert.ToInt32(Math.Ceiling((double)count / 10));
        }

        /// <summary>
        /// (Currently NOT being used). Rendering "Approved Projects" view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewProject_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            GridViewRow pagerRow = (GridViewRow)gv.BottomPagerRow;

            if (pagerRow != null && pagerRow.Visible == false)
                pagerRow.Visible = true;
        }

        //protected void GridViewProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridViewProject.PageIndex = e.NewPageIndex;
        //    BindControl();
        //    //LoadGridData();
        //}

        /// <summary>
        /// Provides control on "Edit" button. E.g., redirects users to Project form of selected row
        /// so that the tracking team is able to click the "Reviewed" button after review.
        /// The same functionality is true when needing to review investigators/PI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewProject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int projectId = 0;
                Int32.TryParse(GridViewProject.DataKeys[index].Value.ToString(), out projectId);

                if (projectId > 0)
                {
                    Response.Redirect("~/ProjectForm?Id=" + projectId);
                }
            }
        }

        /// <summary>
        /// (Currently NOT being used). Adds ability to "add" a project to the tracking system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProjectForm2?Id=" + -1);
        }

        /// <summary>
        /// Allows users to tracking team to go into the pending project, review the newly-entered project,
        /// and check off the "Reviewed" checkbox for approval to allow users to enter hours for the specific
        /// project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int projectId = (int)GridViewPending.DataKeys[index].Value;

                if (projectId > 0)
                {
                    Response.Redirect("~/ProjectForm2?Id=" + projectId);
                }

            }
        }

        /// <summary>
        /// Through the "Edit button, allows users (mainly tracking team) to view the list of 
        /// Investigators/PI's and click the "Reviewed" to mark that it has been viewed and
        /// reviewed by the QHS Tracking Team for clean data entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewPI_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("editRecord"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int piId = (int)GridViewPI.DataKeys[index].Value;

                if (piId > 0)
                {
                    Response.Redirect("~/PI?Id=" + piId);
                }

            }

        }
    }
}