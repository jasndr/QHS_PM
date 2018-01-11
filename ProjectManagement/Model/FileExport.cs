using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

namespace ProjectManagement.Model
{
    public class FileExport
    {
        private HttpResponse _repsonse;
        public FileExport(HttpResponse response)
        {
            _repsonse = response;
        }
        public void ExcelExport(DataTable dt, string filename)
        {
            string fileValue = String.Format("attachment;filename={0}.xlsx", filename);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                _repsonse.Clear();
                _repsonse.Buffer = true;
                _repsonse.Charset = "";
                _repsonse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                _repsonse.AddHeader("content-disposition", fileValue);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(_repsonse.OutputStream);
                    _repsonse.Flush();
                    _repsonse.End();
                }
            }
        }

        public void CsvExport(DataTable dt, DateTime dtFrom, string downloadTokenValue)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for CSV file.
                csv += column.ColumnName + ',';
            }

            //Add new line.
            csv += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            string fileName = string.Format("attachment;filename=RMATRIX_Monthly_{0}.csv", dtFrom.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture));

            //Download the CSV file.
            _repsonse.Clear();
            //Response.ClearHeaders();
            //Response.ClearContent();                                   

            _repsonse.Buffer = true;
            _repsonse.AddHeader("content-disposition", fileName);
            _repsonse.Charset = "";
            _repsonse.ContentType = "application/text";
            _repsonse.Output.Write(csv);

            _repsonse.AppendCookie(new HttpCookie("fileDownloadToken", downloadTokenValue)); //downloadTokenValue 

            _repsonse.Flush();
            _repsonse.End();
        }
    }
}