using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                divDashboard.Visible = true;
                divIdleProject.Visible = true;
                BindControl();
            }
            else
            {
                divDashboard.Visible = false;
                divIdleProject.Visible = false;
            }

        }

        private void BindControl()
        {
            int projectCnt = 0;
            decimal? timeCnt = 0.0m;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var biostat = db.BioStats.FirstOrDefault(b => b.LogonId == Page.User.Identity.Name);

                if (biostat != null)
                {
                    //projectCnt = db.ProjectBioStats
                    //             .Where(p => p.BioStats_Id == biostat.Id && p.EndDate >= DateTime.Now
                    //                 && (p.Project.ProjectCompletionDate == null || p.Project.ProjectCompletionDate > DateTime.Now))
                    //             .Count();

                    //var timeEntry = db.TimeEntries
                    //                .Join(db.Date1
                    //                    , t => t.DateKey
                    //                    , d => d.DateKey
                    //                    , (t, d) => new
                    //                    {
                    //                        t.Duration
                    //                        ,
                    //                        t.BioStatId
                    //                        ,
                    //                        d.Date
                    //                    }
                    //                )
                    //                .Where(a => a.BioStatId == biostat.Id && a.Date.Year == DateTime.Now.Year && a.Date.Month == DateTime.Now.Month);

                    ////timeCnt = timeEntry == null : 0.0m ? timeEntry.Sum(i => i.Duration);

                    //if (timeEntry.Count() > 0)
                    //{
                    //    timeCnt = timeEntry.Sum(i => i.Duration);
                    //}

                    //bind rptProject 
                    var projectTime = db.vwBiostatTime.Where(v => v.biostatid == biostat.Id).ToList();

                    rptProject.DataSource = projectTime.Where(t => t.monthdiff <= 3 || (bool)t.IsInternal).OrderByDescending(t => t.lastentry);
                    rptProject.DataBind();

                    rptIdleProject.DataSource = projectTime.Where(t => t.monthdiff > 3 && !(bool)t.IsInternal).OrderByDescending(t => t.projectid);
                    rptIdleProject.DataBind();
                }

            }

            lblProjectCnt.Text = projectCnt.ToString();
            hlTimeCnt.Text = timeCnt.ToString();
        }        

        [WebMethod]
        public static string LoadPublication()
        {
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                dropDownSource = context.Publications.ToDictionary(c => c.Id, c => c.Title);
            }

            JavaScriptSerializer jscript = new JavaScriptSerializer();
            return jscript.Serialize(dropDownSource);
        }

        protected void rptProject_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {          
            string command = e.CommandName;
            int id = 0;
            int.TryParse(e.CommandArgument as string, out id);

            if (id > 0)
            {
                switch (command)
                {
                    case "Project":
                        Response.Redirect("~/ProjectForm2?Id=" + id);
                        break;
                    case "PI":
                        Response.Redirect("~/PI?Id=" + id);
                        break;
                    case "TimeEntry":
                        Response.Redirect("~/TimeEntry?Id=" + id);
                        break;
                }
            }
        }
    }
}