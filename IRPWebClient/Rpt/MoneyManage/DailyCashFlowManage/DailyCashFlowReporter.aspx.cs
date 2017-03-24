using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

public partial class Rpt_MoneyManage_DailyCashFlowManage_DailyCashFlowReporter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Export();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        btnExport.Visible = false;
        Export();
    }

    protected void Export()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("Row_Number");
        //dt.Columns.Add("Name");
        //dt.Columns.Add("Age");
        //dt.Columns.Add("Code");
        //dt.Columns.Add("Number");
        //dt.Columns.Add("EnglishName");
        //for (int i = 1; i < 6; i++)
        //{
        //    var dr = dt.NewRow();
        //    dr["Row_Number"] = i;
        //    dr["Name"] = "孙小双" + i;
        //    dr["Age"] = 28 + i;
        //    dr["Code"] = "sunxsh" + i;
        //    dr["Number"] = "2071714117" + i;
        //    dr["EnglishName"] = "Dancy" + i;
        //    dt.Rows.Add(dr);
        //}

        //HSSFWorkbook wk = new HSSFWorkbook();
        //ISheet tb = wk.CreateSheet("Student");
        //var titleRow = tb.CreateRow(1);
        //for (int i = 0; i < dt.Columns.Count; i++)
        //{
        //    ICell cell = titleRow.CreateCell(i);
        //    cell.SetCellValue(dt.Columns[i].ColumnName);
        //    titleRow.Cells.Add(cell);
        //}
        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    var rowData = tb.CreateRow(i + 2);
        //    for (int j = 0; j < dt.Columns.Count; j++)
        //    {
        //        ICell cell = rowData.CreateCell(j);
        //        cell.SetCellValue(dt.Rows[i][j] + "");
        //        rowData.Cells.Add(cell);
        //    }
        //}
        //if (!Directory.Exists(Server.MapPath("temp")))
        //{
        //    Directory.CreateDirectory(Server.MapPath("temp"));
        //}
        string fileName = "DailyCashFlowReporter" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        //string filePath = Server.MapPath("temp");
        //using (FileStream fs = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
        //{
        //    wk.Write(fs);
        //}

        Response.ContentType = "application/vnd.ms-excel";
        Response.AppendHeader("Content-Disposition ", "attachment;filename=" + fileName);
        
        //Response.WriteFile(filePath);
    }

}
