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
    public partial class ProjectList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }

            if (!Page.User.Identity.Name.Equals("yrui"))
            {
                divProjectList.Style["display"] = "none";
            }
        }

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
      
        private void BindControl()
        {
            GridViewBindEmpty(GridViewProject); 

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var query = context.Invests
                           .Join(context.InvestStatus, i => i.InvestStatusId, s => s.Id,
                                   (i, s) => new { i.Id, i.FirstName, i.LastName, i.Email, i.Phone, s.StatusValue, i.IsApproved })
                           .Where(d => d.Id > 0 && !d.IsApproved)
                           .OrderBy(d => d.Id);

                GridViewPI.DataSource = query.ToList();
                GridViewPI.DataBind();

                var pending = context.Project2
                            .Join(context.Invests, p => p.Invests.Id, i => i.Id, ((p, i) => new { p.Id, p.Title, p.InitialDate, p.IsApproved, i.FirstName, i.LastName}))
                            .Where(a => a.IsApproved == false && a.Id > 0);

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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProjectForm2?Id=" + -1);
        }

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