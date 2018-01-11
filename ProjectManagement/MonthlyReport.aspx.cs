using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagement
{
    public partial class MonthlyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindControl();
            }
        }

        protected void ddlMonth_Changed(Object sender, EventArgs e)
        {
        }

        private void BindControl()
        {
            using (ProjectTrackerContainer context = new ProjectTrackerContainer())
            {
                var query = context.Date1.Where(d => d.Year == 2015);

                IDictionary<int, string> dropDownSource = new Dictionary<int, string>();

                dropDownSource = query                                
                                .Select(x => new { id = (int)x.MonthOfYear, x.Month })                               
                                .Distinct()
                                .OrderBy(i => i.id)
                                .ToDictionary(c => c.id, c => c.Month);
                BindDropDownList(ddlMonth, dropDownSource, string.Empty);
            }
        }

        private void BindDropDownList(DropDownList ddl, IDictionary<int, string> dataSource, string firstItemText)
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = "Key";
            ddl.DataTextField = "Value";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(firstItemText, String.Empty));
        }
    }
}