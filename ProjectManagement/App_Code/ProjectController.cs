using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectManagement.App_Code
{
    public class ProjectController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // GET api/<controller>/5/d/d
        public List<decimal> GetProjectHours(int projectId, string startDate, string endDate)
        {
            List<decimal> projectHours = new List<decimal>();
            DateTime dateStart, dateEnd;

            if (DateTime.TryParse(startDate, out dateStart) && (DateTime.TryParse(endDate, out dateEnd)))
            {
                if (dateEnd > dateStart && projectId > 0)
                {
                    decimal workedPhd = 0.0M, workedMS = 0.0M;
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        ObjectParameter phdHours = new ObjectParameter("PhdHours", typeof(decimal));
                        ObjectParameter msHours = new ObjectParameter("MSHours", typeof(decimal));

                        //phdHours.Value = 0.0M;
                        //msHours.Value = 0.0M;

                        var i = context.P_PROJECT_HOURS(dateStart, dateEnd, projectId, phdHours, msHours);
                        context.SaveChanges();

                        Decimal.TryParse(phdHours.Value.ToString(), out workedPhd);
                        Decimal.TryParse(msHours.Value.ToString(), out workedMS);

                        projectHours.Add(workedPhd);
                        projectHours.Add(workedMS);
                    }

                }
            }
            else
            {
                projectHours.Add(0.0m);
                projectHours.Add(0.0m);
            }

            return projectHours;
        }

        public int GetInvoiceId(string ccAbbrv)
        {
            int lastInvoiceId = 1000;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var invoices = context.Invoice1Set
                                .Join(context.CollabCtr, a => a.CollabCtrId, c => c.Id, (a, c) => new { a.Id, a.InvoiceId, c.NameAbbrv })
                                .Where(m => m.NameAbbrv == ccAbbrv).ToList();

                foreach (var invoice in invoices)
                {
                    string strId = invoice.InvoiceId.Split('-')[2];
                    int id = 0;
                    Int32.TryParse(strId, out id);

                    if (id > lastInvoiceId)
                        lastInvoiceId = id;
                }
            }

            return ++lastInvoiceId;

        }

        public int GetClientAgreementId(string ccAbbrv)
        {
            int lastAgmtId = 0;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var clientAgmts = context.ClientAgmt
                                .Join(context.CollabCtr, a => a.CollabCtrId, c => c.Id, (a, c) => new { a.Id, a.AgmtId, c.NameAbbrv })
                                .Where(m => m.NameAbbrv == ccAbbrv).ToList();

                foreach(var clientAgmt in clientAgmts)
                {
                    string strAgmtId = clientAgmt.AgmtId.Split('-')[2];
                    int tempId = 0;
                    Int32.TryParse(strAgmtId, out tempId);

                    if (tempId > lastAgmtId)
                        lastAgmtId = tempId;
                }
            }

            return ++lastAgmtId;

        }

        //GET api/<controller>/5
        public string GetProjectPIName(int projectId)
        {
            string piName = string.Empty;
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var project = context.Project2.FirstOrDefault(p => p.Id == projectId);

                if (project != null)
                {
                    piName = project.Invests.FirstName + " " + project.Invests.LastName
                        + "|" + Enum.GetName(typeof(ProjectType), project.ProjectType); ;

                }
            }

            return piName;
        }

        public string GetProjectPhase(int projectId)
        {
            ICollection<ProjectPhase> projectPhases = new List<ProjectPhase>();
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var lstPhase = context.ProjectPhase.Where(p => p.ProjectId == projectId).ToList();

                if (lstPhase != null)
                {
                    foreach(var phase in lstPhase)
                    {
                        ProjectPhase p = new ProjectPhase()
                        {
                            Id = phase.Id,
                            Name = phase.Name,
                            ProjectId = phase.ProjectId,
                            Title = phase.Title,
                            MsHrs = phase.MsHrs,
                            PhdHrs = phase.PhdHrs
                        };

                        projectPhases.Add(p);
                    }
                    
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(projectPhases);
        }

        public bool GetSurveyStatus(string surveyId)
        {
            bool status = false;
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var survey = context.SurveyForms.FirstOrDefault(s => s.Id == surveyId);

                if (survey != null)
                {
                    status = survey.Responded;
                }
            }

            return status;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}