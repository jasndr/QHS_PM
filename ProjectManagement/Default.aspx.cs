using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// @File: Default.aspx.cs
/// @Author: Yang Rui
/// @Summary: Home page of Project Tracking System. 
/// 
///           Lists active and inactive projects with information
///           such as Project ID, PI Name, Project Title, and Last Entry Date aimed to help faculty/staff 
///           manage their own existing projects.
///           
/// @Maintenance/Revision History:
///  YYYYDDMMM - NAME/INITIALS      -  REVISION
///  2018MAR19 - Jason Delos Reyes  -  Started adding documentation to make code more easily maintainable. 
/// </summary>
namespace ProjectManagement
{
    public partial class _Default : Page
    {
        /// <summary>
        /// Displays/hides what user sees based on whether or not they have logged into the system.
        /// (1) If user is logged in,
        ///         - display active and inactive projects dashboards.
        /// (2) If user is not logged in,
        ///         - hide active and inactive projects dashboards.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Assigns the project based on inactivity, i.e., if a project's latest time entry has been
        /// over three months, then it will be placed in the "idle projects" list.  Otherwise, if the
        /// latest time entry has been more recent than three months, it will be placesd in the "active
        /// projects" list.
        /// </summary>
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

        /// <summary>
        /// Status: CURRENTLY NOT BEING USED.
        /// 
        /// Summary: Loads list of publications into the screen based on the drop down source.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// In the 'Active Projects' and 'Idle Projects' tables, sets links to the following fields on grid:
        ///             - Id & Project Title >>> ProjectForm2 to review and edit project information.
        ///             - PI Name            >>> PI page and access to PI form for review and edit if needed.
        ///             - TimeEntry          >>> TimeEntry to review hours and enter more time entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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