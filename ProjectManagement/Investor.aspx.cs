using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace ProjectManagement
{
    public partial class Investor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ClearEmpForm();
                PopulateList();
            }
        }

        //protected void btnsave_Click(object sender, EventArgs e)
        //{
        //    ProjectTrackerContainer db = new ProjectTrackerContainer();

        //    InvestStatus investStatus = new InvestStatus();

        //    investStatus.StatusValue = txtInvestStatusValue.Text;

        //    db.InvestStatus.Add(investStatus);

        //    db.SaveChanges();
        //    ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "<script>alert('Record Added Successfully')</script>");

        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            PopulateList();
        }   
           
               
        private void PopulateList()
        {
            using (ProjectTrackerContainer db = new ProjectTrackerContainer())
            {
                //var query = from d in db.Invests
                //            orderby d.Id
                //            select d;

                var query = db.Invests
                            .Include(d => d.InvestStatu)
                            .Where(a => a.LastName.Contains(txtSrchInvestorName.Text) || a.FirstName.Contains(txtSrchInvestorName.Text))
                            .OrderBy(d => d.Id);

                var investorList = query.ToList();

                GridView1.DataSource = investorList;
                GridView1.DataBind();

                var statusQuery = db.InvestStatus
                                    .OrderBy(d => d.Id);

                //ddlInvestStatus.DataSource = statusQuery.ToList();
                //ddlInvestStatus.DataValueField = "Id";
                //ddlInvestStatus.DataTextField = "StatusValue";

                //ddlInvestStatus.DataBind();

              
                //IQueryable<InvestStatus> statusQuery = from investStatus in db.InvestStatus
                //                                       //where investStatus.StatusValue == txtSrchStatusName.Text
                //                                        select investStatus;

                //List<InvestStatus> statusList = new List<InvestStatus>();

                //foreach (var investS in statusQuery)
                //{
                //    statusList.Add(investS);
                //}

                //ddlInvestStatus.DataSource = statusList;
                //ddlInvestStatus.DataValueField = "Id";
                //ddlInvestStatus.DataTextField = "StatusValue";

                //ddlInvestStatus.DataBind();

                //ddlInvestStatus.Items.Insert(0, new ListItem("--Add New--", "0"));

                //GridView1.DataSource = statusList;
                //GridView1.DataBind();

            }
            

        }

        private void ClearEmpForm()
        {
            //txtInvestStatusId.Text = "";
            //txtInvestStatusValue.Text = "";

            //btnSave.CommandArgument = "0";
            //btnSave.Text = "SAVE";
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "InsertNew")
            {
                GridViewRow row = GridView1.FooterRow;
                TextBox txtFirstName = row.FindControl("txtFirstNameNew") as TextBox;
                TextBox txtLastName = row.FindControl("txtLastNameNew") as TextBox;
                TextBox txtEmail = row.FindControl("txtEmailNew") as TextBox;
                TextBox txtPhone = row.FindControl("txtPhoneNew") as TextBox;
                DropDownList ddlStatus = row.FindControl("ddlStatusNew") as DropDownList;
                if (txtFirstName != null && txtLastName != null && txtEmail != null && ddlStatus != null)
                {
                    using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                    {
                        Invest invest = new Invest();
                        invest.FirstName = txtFirstName.Text;
                        invest.LastName = txtLastName.Text;
                        invest.Email = txtEmail.Text;
                        invest.Phone = txtPhone.Text;
                        invest.InvestStatusId = Convert.ToInt32(ddlStatus.SelectedValue);

                        context.Invests.Add(invest);

                        WriteInvest(context, invest);

                        BindGrid();
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
            GridViewRow row = GridView1.Rows[e.RowIndex];
            Label lblId = row.FindControl("lblId") as Label;
            TextBox txtFirstName = row.FindControl("txtFirstName") as TextBox;
            TextBox txtLastName = row.FindControl("txtLastName") as TextBox;
            TextBox txtEmail = row.FindControl("txtEmail") as TextBox;
            TextBox txtPhone = row.FindControl("txtPhone") as TextBox;
            DropDownList ddlStatus = row.FindControl("ddlStatus") as DropDownList;
            if (txtFirstName != null && txtLastName != null && txtEmail != null && ddlStatus != null)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    //int investorId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
                    int investorId = Convert.ToInt32(lblId.Text);
                    Invest invest = context.Invests.First(x => x.Id == investorId);
                    invest.FirstName = txtFirstName.Text;
                    invest.LastName = txtLastName.Text;
                    invest.Email = txtEmail.Text;
                    invest.Phone = txtPhone.Text;
                    invest.InvestStatusId = Convert.ToInt32(ddlStatus.SelectedValue);

                    WriteInvest(context, invest);
                    
                    GridView1.EditIndex = -1;
                    BindGrid();
                }
            }
        }

        private void WriteInvest(ProjectTrackerContainer context, Invest invest)
        {
            var result = context.Entry(invest).GetValidationResult();
            if (!result.IsValid)
            {
                lblMsg.Text = result.ValidationErrors.First().ErrorMessage;
            }
            else
            {
                context.SaveChanges();
                lblMsg.Text = "Saved successfully.";
                //ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "<script>alert('Investor Added Successfully!')</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DropDownList ddl = null;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ddl = e.Row.FindControl("ddlStatusNew") as DropDownList;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ddl = e.Row.FindControl("ddlStatus") as DropDownList;
            }
            if (ddl != null)
            {
                using (ProjectTrackerContainer context = new ProjectTrackerContainer())
                {
                    ddl.DataSource = context.InvestStatus.ToList();
                    ddl.DataTextField = "StatusValue";
                    ddl.DataValueField = "Id";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem(""));
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ddl.SelectedValue = ((Invest)(e.Row.DataItem)).InvestStatu.Id.ToString();
                }
            }
        }   

        private void BindGrid()
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                if (context.Invests.Count() > 0)
                {
                    var query = context.Invests
                           .Include(d => d.InvestStatu)
                           .Where(a => a.LastName.Contains(txtSrchInvestorName.Text) || a.FirstName.Contains(txtSrchInvestorName.Text))
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
    }
}