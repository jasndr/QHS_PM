using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    public partial class TaskForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }
        }

        private void BindControl()
        {
            IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                dropDownSource = db.BioStats
                                .Where(b => b.EndDate > DateTime.Now)
                                .OrderBy(b => b.Name)
                                .Distinct()
                                .ToDictionary(c => c.Id, c => c.Name);
                PageUtility.BindDropDownList(ddlBiostat, dropDownSource, "--- Select ---");
                PageUtility.BindDropDownList(ddlAssignTo, dropDownSource, "--- Select ---");
    
                dropDownSource = db.TaskFields
                                .Where(t => t.FieldType == "Status")
                                .ToDictionary(c => c.Id, c => c.FieldName);
                PageUtility.BindDropDownList(ddlStatus, dropDownSource, "--- Select ---");
                PageUtility.BindDropDownList(ddlTaskStatus, dropDownSource, "--- Select ---");
                

                dropDownSource = db.TaskFields
                                .Where(t => t.FieldType == "Priority")
                                .ToDictionary(c => c.Id, c => c.FieldName);
                PageUtility.BindDropDownList(ddlPriority, dropDownSource, "--- Select ---");
                PageUtility.BindDropDownList(ddlTaskPriority, dropDownSource, "--- Select ---"); 
                
            }
        }

        protected void btnSumbit_Click(object sender, EventArgs e)
        {
            BindTaskAll();
        }

        private void BindTaskAll()
        {
            int biostatId = -1;
            int taskStatusId = -1, taskPriorityId = -1;
                        
            Int32.TryParse(ddlBiostat.SelectedValue, out biostatId);
            Int32.TryParse(ddlStatus.SelectedValue, out taskStatusId);
            Int32.TryParse(ddlPriority.SelectedValue, out taskPriorityId);

            DataTable taskTable = GetTaskAll(biostatId, taskStatusId, taskPriorityId );

            rptTask.DataSource = taskTable;
            rptTask.DataBind();
        }

        private DataTable GetTaskAll(int biostatId, int taskStatusId, int taskPriorityId)
        {
            DataTable dt = new DataTable("taskTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Title", System.Type.GetType("System.String"));
            dt.Columns.Add("Notes", System.Type.GetType("System.String"));
            dt.Columns.Add("Status", System.Type.GetType("System.String"));
            dt.Columns.Add("Priority", System.Type.GetType("System.String"));
            dt.Columns.Add("AssignTo", System.Type.GetType("System.String"));
            dt.Columns.Add("DueDate", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {            
                var query = db.PTTasks
                    .Select(t => new { t.Id, t.Title, t.Notes, t.TaskStatus, t.TaskPriority, t.BioStat.Name, t.DueDate});
                              
                if (biostatId > 0)
                {
                    var biostat = db.BioStats.FirstOrDefault(b => b.Id == biostatId);
                    if (biostat != null)
                    {
                        query = query.Where(q => q.Name.Contains(biostat.Name));
                    }
                }

                if (taskStatusId != 0)
                {
                    var taskStatus = db.TaskFields.FirstOrDefault(f => f.Id == taskStatusId);
                    if (taskStatus != null)
                    {
                        query = query.Where(q => q.TaskStatus == taskStatus.FieldName);
                    }
                }

                if (taskPriorityId > 0)
                {
                    var taskPriority = db.TaskFields.FirstOrDefault(t => t.Id == taskPriorityId);
                    if (taskPriority != null)
                    {
                        query = query.Where(q => q.TaskPriority == taskPriority.FieldName);
                    }
                }

                foreach (var p in query.OrderByDescending(p => p.Id).ToList())
                {
                    DataRow row = dt.NewRow();

                    row[0] = p.Id;
                    row[1] = p.Title;
                    row[2] = p.Notes;
                    row[3] = p.TaskStatus;
                    row[4] = p.TaskPriority;
                    row[5] = p.Name;
                    row[6] = p.DueDate != null ? Convert.ToDateTime(p.DueDate).ToShortDateString() : "";

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        private DataTable GetTaskHistory(int taskId)
        {
            DataTable dt = new DataTable("taskTable");

            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("TaskValue", System.Type.GetType("System.String"));
            dt.Columns.Add("Creator", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateDate", System.Type.GetType("System.String"));

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                var query = db.TaskHistories.Where(h => h.TaskId == taskId);                

                foreach (var p in query.OrderByDescending(p => p.Id).ToList())
                {
                    DataRow row = dt.NewRow();

                    row[0] = p.Id;
                    row[1] = p.TaskValue;
                    row[2] = p.Creator;
                    row[3] = p.CreateDate.ToShortDateString();

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int taskId = 0;
            Int32.TryParse(lblTaskId.Text, out taskId);

            PTTask myTask = GetTaskFromUI(taskId);
            taskId = SaveTask(myTask);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(false), false);

            BindTaskAll();
        }

        private int SaveTask(PTTask myTask)
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                if (myTask.Id > 0) //update
                {
                    var prevTask = db.PTTasks.FirstOrDefault(t => t.Id == myTask.Id);

                    if (prevTask != null)
                    {                      
                        //create a history 
                        StringBuilder sb = new StringBuilder();

                        if (prevTask.BiostatId != myTask.BiostatId)
                        {
                            sb.AppendFormat("Assign To: {0}; ", prevTask.BioStat.Name);
                        }

                        if (prevTask.TaskStatus != myTask.TaskStatus)
                        {
                            sb.AppendFormat("Status: {0}; ", prevTask.TaskStatus);
                        }

                        if (prevTask.TaskPriority != myTask.TaskPriority)
                        {
                            sb.AppendFormat("Priority: {0}", prevTask.TaskPriority);
                        }

                        if (prevTask.Notes != myTask.Notes)
                        {
                            sb.AppendFormat("Note: {0}", prevTask.Notes);
                        }

                        if (!sb.ToString().Equals(string.Empty))
                        {
                            TaskHistory taskHistory = new TaskHistory()
                            {
                                TaskId = prevTask.Id,
                                TaskValue = sb.ToString(),
                                Creator = Page.User.Identity.Name,
                                CreateDate = DateTime.Now
                            };

                            db.TaskHistories.Add(taskHistory);

                            BindTaskHistory(myTask.Id);
                        }

                        db.Entry(prevTask).CurrentValues.SetValues(myTask);
                    }
                }
                else //new
                {
                    db.PTTasks.Add(myTask);
                }

                db.SaveChanges();
            }

            return myTask.Id;
        }

        private void BindTaskHistory(int taskId)
        {
            DataTable taskHistoryTable = GetTaskHistory(taskId);

            rptTaskHistory.DataSource = taskHistoryTable;
            rptTaskHistory.DataBind();
        }       

        private PTTask GetTaskFromUI(int taskId)
        {
            DateTime dt;
            int ddlValue;
            PTTask myTask = new PTTask()
            {
                Id = taskId,
                BiostatId = Int32.TryParse(ddlAssignTo.SelectedValue, out ddlValue) ? ddlValue : -1,
                ProjectId = -1,
                TaskStatus = ddlTaskStatus.SelectedItem.Text,
                TaskPriority = ddlTaskPriority.SelectedItem.Text,
                DueDate = DateTime.TryParse(txtDueDate.Text, out dt) ? dt : DateTime.Now,
                Title = txtTaskTitle.Text,
                Notes = txtTaskNote.Text,
                Creator = Page.User.Identity.Name,
                CreateDate = DateTime.Now
            };

            return myTask;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearEditForm();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "ModalScript", PageUtility.LoadEditScript(true), false);
        }

        private void BindTask(PTTask ptTask)
        {
            if (ptTask != null)
            {
                lblTaskId.Text = ptTask.Id > 0 ? ptTask.Id.ToString() : string.Empty;
                txtTaskTitle.Text = ptTask.Title;
                txtTaskNote.Text = ptTask.Notes;
                txtDueDate.Text = ptTask.DueDate != null ? Convert.ToDateTime(ptTask.DueDate).ToShortDateString() : "";

                if (ptTask.BiostatId > 0)
                {
                    ddlAssignTo.SelectedValue = ptTask.BiostatId.ToString();
                }
                else
                {
                    ddlAssignTo.ClearSelection();
                }

                ddlTaskStatus.ClearSelection();
                if (ptTask.TaskStatus != null)
                {
                    var taskStatus = ddlTaskStatus.Items.FindByText(ptTask.TaskStatus);
                    if (taskStatus != null)
                    {
                        taskStatus.Selected = true;
                    }
                }

                ddlTaskPriority.ClearSelection();
                if (ptTask.TaskPriority != null)
                {
                    var taskPriority = ddlTaskPriority.Items.FindByText(ptTask.TaskPriority);
                    if (taskPriority != null)
                    {
                        taskPriority.Selected = true;
                    }
                }
            }
        }               
        
        protected void rptTask_ItemCommand(Object sender, RepeaterCommandEventArgs e)
        {
            if (((Button)e.CommandSource).Text.Equals("Edit"))
            {
                lblTaskId.Text = ((Button)e.CommandSource).CommandArgument;

                int taskId = 0;
                int.TryParse(lblTaskId.Text, out taskId);

                if (taskId > 0)
                {
                    PTTask myTask = GetPTTaskById(taskId);

                    if (myTask != null)
                    {
                        BindEditModal(myTask);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                   "ModalScript", PageUtility.LoadEditScript(true), false);
                    }
                }
            }
        }

        private void BindEditModal(PTTask myTask)
        {
            SetTaskToUI(myTask);

            BindTaskHistory(myTask.Id);
        }

        private void SetTaskToUI(PTTask myTask)
        {
           if (myTask != null)
           {
               txtTaskTitle.Text = myTask.Title;
               txtTaskNote.Text = myTask.Notes;
               txtDueDate.Text = myTask.DueDate != null ? Convert.ToDateTime(myTask.DueDate).ToShortDateString() : "";
               ddlAssignTo.SelectedValue = myTask.BiostatId.ToString();

               ddlTaskStatus.ClearSelection();
               if (myTask.TaskStatus != null)
               {
                   var status = ddlTaskStatus.Items.FindByText(myTask.TaskStatus);
                   if (status != null)
                   {
                       status.Selected = true;
                   }
               }

               ddlTaskPriority.ClearSelection();
               if (myTask.TaskPriority != null)
               {
                   var status = ddlTaskPriority.Items.FindByText(myTask.TaskPriority);
                   if (status != null)
                   {
                       status.Selected = true;
                   }
               }

           }
        }

        private PTTask GetPTTaskById(int taskId)
        {
            PTTask theTask;

            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                theTask = db.PTTasks.FirstOrDefault(t => t.Id == taskId);

                if (theTask != null)
                {
                    theTask.TaskHistories = theTask.TaskHistories.ToList();
                }
            }

            return theTask;
        }
       
        private void ClearEditForm()
        {
            PTTask ptTask = new PTTask();
            ptTask.Id = -1;
            ptTask.ProjectId = -1;
            ptTask.BiostatId = -1;
            //ptTask.DueDate = null;

            BindTask(ptTask);

            BindTaskHistory(0);
        }

        

    }
}