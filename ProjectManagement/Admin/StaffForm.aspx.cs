using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement.Admin
{
    public partial class StaffForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                if (context.Invests.Count() > 0)
                {
                    var query = context.BioStats
                           .OrderBy(d => d.Id);

                    GridView1.DataSource = query.ToList();
                    GridView1.DataBind();
                }
                else
                {
                    var obj = new List<Invest>();
                    obj.Add(new Invest());
                    // Bind the DataTable which contain a blank row to the GridView
                    GridView1.DataSource = obj;
                    GridView1.DataBind();
                    int columnsCount = GridView1.Columns.Count;
                    GridView1.Rows[0].Cells.Clear();// clear all the cells in the row
                    GridView1.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
                    GridView1.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

                    //You can set the styles here
                    GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    GridView1.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
                    GridView1.Rows[0].Cells[0].Font.Bold = true;
                    //set No Results found to the new added cell
                    GridView1.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Page.User.IsInRole("Super"))
            {
                if (e.CommandName == "InsertNew")
                {
                    GridViewRow row = GridView1.FooterRow;

                    TextBox txtName = row.FindControl("txtNameNew") as TextBox;
                    TextBox txtType = row.FindControl("txtTypeNew") as TextBox;
                    TextBox txtEmail = row.FindControl("txtEmailNew") as TextBox;
                    TextBox txtLogonId = row.FindControl("txtLogonIdNew") as TextBox;
                    TextBox txtEndDate = row.FindControl("txtEndDateNew") as TextBox;

                    if (txtName != null && txtEmail != null && txtLogonId != null)
                    {
                        using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                        {
                            BioStat biostat = new BioStat()
                            {
                                Name = txtName.Text,
                                Type = txtType.Text,
                                Email = txtEmail.Text,
                                LogonId = txtLogonId.Text,
                                EndDate = DateTime.Parse(txtEndDate.Text)
                            };

                            context.BioStats.Add(biostat);

                            WriteInvest(context, biostat);

                            BindGrid();
                        }
                    }
                }
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.User.IsInRole("Super"))
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];

                Label lblId = row.FindControl("lblId") as Label;
                TextBox txtName = row.FindControl("txtName") as TextBox;
                TextBox txtType = row.FindControl("txtType") as TextBox;
                TextBox txtEmail = row.FindControl("txtEmail") as TextBox;
                TextBox txtLogonId = row.FindControl("txtLogonId") as TextBox;
                TextBox txtEndDate = row.FindControl("txtEndDate") as TextBox;

                if (txtName != null && txtLogonId != null && txtEmail != null)
                {
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        int biostatId = Convert.ToInt32(lblId.Text);
                        BioStat biostat = context.BioStats.First(x => x.Id == biostatId);
                        biostat.Name = txtName.Text;
                        biostat.Type = txtType.Text;
                        biostat.Email = txtEmail.Text;
                        biostat.LogonId = txtLogonId.Text;
                        biostat.EndDate = DateTime.Parse(txtEndDate.Text);

                        WriteInvest(context, biostat);

                        GridView1.EditIndex = -1;
                        BindGrid();

                    }
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        private void WriteInvest(ProjectTrackerContainer context, BioStat biostat)
        {
            var result = context.Entry(biostat).GetValidationResult();
            if (!result.IsValid)
            {
                lblMsg.Text = result.ValidationErrors.First().ErrorMessage;
            }
            else
            {
                context.SaveChanges();
                lblMsg.Text = "Saved successfully.";
            }
        }
    }
}