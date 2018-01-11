using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ProjectManagement.App_Code
{
    public class DashboardController : ApiController
    {
        // GET api/<controller>/Id
        public List<decimal> GetProjectHours(int year)
        {
            List<decimal> projectHours = new List<decimal>();

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                string query = string.Format(@"select sum(t.Duration) 
                                                from Date d 
                                                left outer join TimeEntries t
                                                on d.DateKey = t.DateId
                                                where d.Year={0}
                                                and MonthOfYear<=MONTH(GETDATE())
                                                group by d.Month, d.MonthOfYear
                                                order by d.MonthOfYear", year);

                var result = context.Database.SqlQuery<decimal ?>(query);

                foreach (decimal? qr in result)
                {
                    if (qr != null)
                    {
                        projectHours.Add((decimal)qr);
                    }
                    else
                    {
                        projectHours.Add(0.0m);
                    }
                }

            }

            return projectHours;

        }

        public List<int> GetPaperSubmitted(int year)
        {
            List<int> papers = new List<int>();

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                string query = string.Format(@" select count(p.Id)
                                                from Date d 
                                                left outer join publications p
                                                on d.Date = p.SubmitDate
                                                where d.Year={0}
                                                group by d.Month, d.MonthOfYear
                                                order by d.MonthOfYear", year);

                var result = context.Database.SqlQuery<int?>(query);

                foreach (int? qr in result)
                {
                    if (qr != null)
                    {
                        papers.Add((int)qr);
                    }
                    else
                    {
                        papers.Add(0);
                    }
                }

            }

            return papers;

        }

        public List<int> GetProjectCount(int year)
        {
            List<int> projects = new List<int>();

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
//                string query = string.Format(@"with tmp as (
//	                                            select d.Month, d.MonthOfYear, count(p.Id) mcount
//	                                            from Date d 
//	                                            left outer join Projects p
//	                                            on d.Date = p.InitialDate
//	                                            where d.Year={0}
//	                                            group by d.Month, d.MonthOfYear
//                                            )
//                                            SELECT
//	                                            RunningTotal = mCount + COALESCE(
//	                                            (
//		
//			                                            select sum(mCount) 
//			                                            from tmp d 
//			                                            where d.MonthOfYear < o.MonthOfYear			
//			                                            ), 0
//	                                            )
//                                            FROM tmp AS o
//                                            WHERE MonthOfYear<=MONTH(GETDATE())
//                                            ORDER BY MonthOfYear", year);

                string query = string.Format(@"select count(distinct t.ProjectId) 
                                                from Date d 
                                                left outer join TimeEntries t
                                                on d.DateKey = t.DateId
                                                where d.Year={0}
                                                and MonthOfYear<=MONTH(GETDATE())
                                                group by d.Month, d.MonthOfYear
                                                order by d.MonthOfYear
                                                ", year);

                var result = context.Database.SqlQuery<int?>(query);

                foreach (int? qr in result)
                {
                    if (qr != null)
                    {
                        projects.Add((int)qr);
                    }
                    else
                    {
                        projects.Add(0);
                    }
                }

            }

            return projects;

        }

    }
}