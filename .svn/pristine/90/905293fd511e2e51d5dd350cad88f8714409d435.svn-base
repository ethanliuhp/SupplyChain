using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace ImportIntegration
{
    public class ReadExcel
    {
        public static DataTable ExcelToDataTable(string strExcelFileName, string strSheetName)
        {
            if (strSheetName == String.Empty)
            {
                strSheetName = GetExcelSheetNames(strExcelFileName)[0].ToString();
                if (strSheetName.LastIndexOf("$") > -1)
                    strSheetName = strSheetName.Substring(0, strSheetName.Length - 1);
            }

            bool bHasSheet = false;
            DataTable TempTable = new DataTable();
            string[] sheets = GetExcelSheetNames(strExcelFileName);
            for (int i = 0; i < sheets.Length; i++)
            {
                string sTemp = sheets[i].Substring(0, sheets[i].Length - 1);
                if (string.Compare(sTemp, strSheetName, true) == 0)
                {
                    bHasSheet = true;
                    break;
                }
            }
            if (!bHasSheet)
                return TempTable;

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";

            string strExcel = string.Format("select * from [{0}$]", strSheetName);

            DataSet ds = new DataSet();

            OleDbConnection conn = new OleDbConnection(strConn);

            conn.Open();

            OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
            adapter.Fill(ds, strSheetName);

            conn.Close();

            return ds.Tables[strSheetName];
        }

        private static String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                String connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                objConn = new OleDbConnection(connString);
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                return excelSheets;
            }
            catch
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }
    }
}
