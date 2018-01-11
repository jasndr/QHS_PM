using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace ProjectManagement.Model
{
    public static class PageUtility
    {
        public static void UncheckGrid(GridView gv)
        {
            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    chkRow.Checked = false;
                }
            }
        }

        public static void BindDropDownList(DropDownList ddl, IDictionary<int, string> dataSource, string firstItemText)
        {
            if (ddl != null)
            {
                ddl.DataSource = dataSource;
                ddl.DataValueField = "Key";
                ddl.DataTextField = "Value";
                ddl.DataBind();

                if (firstItemText != null)  //(dataSource.Count > 1)
                {
                    ddl.Items.Insert(0, new ListItem(firstItemText, String.Empty));
                }
            }
        }


        public static List<int> GetSelectedRow_GridView(GridView gv)
        {
            List<int> list = new List<int>();

            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    Label lblId = row.FindControl("lblId") as Label;

                    int id;                   
                    if (chkRow == null || chkRow.Checked == true)
                    {
                        if (Int32.TryParse(lblId.Text, out id))
                        {
                            list.Add(id);
                        }                        
                    }
                }
            }

            return list;
        }

        public static void SelectRow_GridView(GridView gridView, List<int> idList)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    chkRow.Checked = false;
                    Label lblId = row.FindControl("lblId") as Label;

                    if (idList.Contains(Convert.ToInt32(lblId.Text)))
                    {
                        chkRow.Checked = true;
                    }
                }
            }
        }


        internal static void BindEmptyGridView(GridView gridView, Type type)
        {
            object obj = Activator.CreateInstance(type);
            var dataSource = new List<object>();
            dataSource.Add(obj);
        
            // Bind the DataTable which contain a blank row to the GridView
            gridView.DataSource = dataSource;
            gridView.DataBind();
            //int columnsCount = GridView1.Columns.Count;
            gridView.Rows[0].Cells.Clear();// clear all the cells in the row

            //int columnsCount = gridView.Columns.Count;
            //gridView.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
            //gridView.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

            ////You can set the styles here
            //gridView.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //gridView.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            //gridView.Rows[0].Cells[0].Font.Bold = true;
            ////set No Results found to the new added cell
            //gridView.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
        }    
              
        internal static string LoadEditScript(bool isOpen)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            //sb.Append("$('.pdsa-submit-progress').addClass('hidden');");

            if (isOpen)
            {
                sb.Append("$('#editModal').modal('show');");
            }
            else
            {
                //sb.Append("var url = window.location.href;");
                //sb.Append("window.location.href = url.split('?')[0];");
                sb.Append("alert('Record Saved Successfully');");
                sb.Append("if (window.location.href.indexOf('?') > 0) window.location.href = window.location.href.split('?')[0];");
                sb.Append("$('#editModal').modal('hide');");
                
            }
            sb.Append(@"</script>");

            return sb.ToString();
        }

        internal static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        
    }


}