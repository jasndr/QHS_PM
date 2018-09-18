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

    /// <summary>
    /// @File: ProjectController.cs
    /// @Author: Yang Rui
    /// @Summary: Controller that manages API calls.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018SEP10 - Jason Delos Reyes  -  Added documentation for easier readibility and maintainability.
    ///  2018SEP11 - Jason Delos Reyes  -  Changed string split in GetClientAgreementId() and GetInvoiceId() functions in order
    ///                                    to account for names with multiple dashes (-).  The previous functionality
    ///                                    only obtained the value after the second dash, though there are many
    ///                                    Collaborative Centers with dashes in their abbreviation name that
    ///                                    contradicted this rule, thus not being able to increment the number 
    ///                                    for a new Client Agreement ID.  This does not, however, account for
    ///                                    numbers with letters in them (e.g., H7, H10-cont, etc.) and will need
    ///                                    to be discussed on how to address these types of agreement IDs.
    ///                                 -  Also changed invoice start number from 1000 to 0.  It seems arbitrary to have a number
    ///                                    in the thousands, unless it was intentional in the first place.
    ///                                
    /// </summary>
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

        /// <summary>
        /// Finds the highest number and returns number higher than the highest number
        /// of the invoice cycle for the given collab center
        /// to be used for creating a new Invoice ID number.
        /// </summary>
        /// <param name="ccAbbrv">Referred Collaborative Center abbreviation.</param>
        /// <returns>Number to be used in new Invoice instance.</returns>
        public int GetInvoiceId(string ccAbbrv)
        {
            int lastInvoiceId = 0/*1000*/;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                /// Obtains list of Invoices under provided Collaborative Center.
                var invoices = context.Invoice1Set
                                .Join(context.CollabCtr, a => a.CollabCtrId, c => c.Id, (a, c) => new { a.Id, a.InvoiceId, c.NameAbbrv })
                                .Where(m => m.NameAbbrv == ccAbbrv).ToList();


                /// Goes through the list of invoice instancess under provided collab center.
                /// Increments lastInvoiceId if it is greater than already stored int there.
                /// Returns the number above lastAgmtId at the end of loop.
                foreach (var invoice in invoices)
                {
                    string strId = invoice.InvoiceId.Split('-').Last()/*[2]*/; // Obtains value after *last* dash in InvoiceId (e.g., 02 from I-LGao-02)
                    int id = 0;
                    Int32.TryParse(strId, out id);

                    if (id > lastInvoiceId)
                        lastInvoiceId = id;
                }
            }

            return ++lastInvoiceId;

        }

        /// <summary>
        /// Finds the highest number and returns number higher than the highest number
        /// of the client agreement cycle for the given collab center
        /// to be used for creating a new ClientAgreement Id number.
        /// </summary>
        /// <param name="ccAbbrv">Referred Collaborative Center abbreviation.</param>
        /// <returns>Number to be used in new Client Agreement.</returns>
        public int GetClientAgreementId(string ccAbbrv)
        {
            int lastAgmtId = 0;

            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                /// Obtains list of Client Agreements under provided Collaborative Center.
                var clientAgmts = context.ClientAgmt
                                .Join(context.CollabCtr, a => a.CollabCtrId, c => c.Id, (a, c) => new { a.Id, a.AgmtId, c.NameAbbrv })
                                .Where(m => m.NameAbbrv == ccAbbrv).ToList();

                /// Goes through the list of client agreements under provided collab center.
                /// Increments lastAgmtId if it is greater than already stored int there.
                /// Returns the number above lastAgmtId at the end of loop.
                foreach(var clientAgmt in clientAgmts)
                {
                    string strAgmtId = clientAgmt.AgmtId.Split('-').Last()/*[2]*/; // Obtains value after *last* dash in AgmtId (e.g., 02 from A-OBGYN-02)
                    int tempId = 0;
                    Int32.TryParse(strAgmtId, out tempId); // Puts strAgmtId -> temp if it's an integer value. 

                    if (tempId > lastAgmtId)
                        lastAgmtId = tempId;
                }
            }

            return ++lastAgmtId; // Increment, then return value.

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