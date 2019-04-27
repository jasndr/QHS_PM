using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

namespace ProjectManagement.Model
{
    /// <summary>
    /// @File: FileExport.cs
    /// @Author: Yang Rui
    /// @Summary: Exports DataTable report obtained from SQL into Excel or CSV file.
    ///           
    /// @Maintenance/Revision History:
    ///  YYYYDDMMM - NAME/INITIALS      -  REVISION
    ///  ------------------------------------------
    ///  2018JUL25 - Jason Delos Reyes  -  Added ExportExcel2() function in order to print out the Report Title (window of dates,
    ///                                    report type, and grant) on top of the excel sheet that is to be downloaded by the user.
    ///                                    Also added initial documentation for readibility.
    ///  2018JUL30 - Jason Delos Reyes  -  Edited ExportExcel2() function so that the project type is printed as the sheet name
    ///                                    instead of the "Insert Table" text that was initially specified.
    ///  2019APR26 - Jason Delos Reyes  -  Added ExportCsv() overload function to accomodate grant distinction, mainly for Ola HAWAII 
    ///                                    Monthly Reports.
    ///                                 -  Added Year to ExportCsv() overload function (pulls from "from date" year).
    /// </summary>
    public class FileExport
    {
        private HttpResponse _repsonse;
        public FileExport(HttpResponse response)
        {
            _repsonse = response;
        }

        /// <summary>
        /// Creates a downloadable Excel spreadsheet of the data table.
        /// </summary>
        /// <param name="dt">Data table with information parsed from SQL stored procedure.</param>
        /// <param name="filename">Name of file specified.</param>
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

        /// <summary>
        /// Creates a downloadable table of the Excel spreadsheet, accepting a 
        /// "titleName" parameter to allow the data report to have the title
        /// information embedded into its corresponding excel sheet.
        /// </summary>
        /// <param name="dt">Data table previouly obtained through SQL stored procedure.</param>
        /// <param name="fileName">File name specifed to be used in naming Excel file.</param>
        /// <param name="titleName">Title name that will be placed on top of the Excel file for reference.</param>
        public void ExcelExport2(DataTable dt, string fileName, string titleName)
        {

            string fileValue = String.Format("attachment;filename={0}.xlsx", fileName);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workbookTitleName = dt.TableName;
                var ws = wb.Worksheets.Add(workbookTitleName);

                //Add from DataTable
                ws.Cell(1, 1).Value = titleName;
                ws.Range(1, 1, 1, 7).Merge().AddToNamed("Titles");
                var tableWithData = ws.Cell(2, 1).InsertTable(dt.AsEnumerable());

                //Prepare the style for the titles
                var titlesStyle = wb.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Font.FontSize = 16;

                //Format all titles in one shot
                wb.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

                ws.Columns().AdjustToContents();


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

        /// <summary>
        /// Primarily used by the RMATRIX Monthly reports, creates a downloadable CSV
        /// file form the data obtained from the SQL Database.
        /// </summary>
        /// <param name="dt">Data table extracted from SQL data.</param>
        /// <param name="dtFrom">Initial Date of range.</param>
        /// <param name="downloadTokenValue">Download token value required for CSV download cookie.</param>
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

        /// <summary>
        /// Used by the RMATRIX / Ola HAWAII Monthly reports, creates a downloadable CSV
        /// file form the data obtained from the SQL Database.
        /// </summary>
        /// <param name="dt">Data table extracted from SQL data.</param>
        /// <param name="dtFrom">Initial Date of range.</param>
        /// <param name="downloadTokenValue">Download token value required for CSV download cookie.</param>
        /// <param name="reportGrant">Distinguishes between type of grant, mainly RMATRIX or Ola HAWAII.</param>
        public void CsvExport(DataTable dt, DateTime dtFrom, string downloadTokenValue, string reportGrant)
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

            string reportGrantName = reportGrant == "RMATRIX" ? reportGrant : "Ola_HAWAII";

            string fileName = string.Format("attachment;filename={0}_Monthly_{1}_{2}.csv", reportGrantName, dtFrom.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture)
                                                                                                         , dtFrom.Year);

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