using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.Util
{
    public class ExploreFile
    {
        System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
        private string fileName;//文件名
        private string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "techtemp\\"); // "c:\\windows\\temp\\techtemp\\";//目录路径
        private string fileExtension;//文件后缀

        /// <summary>
        /// 文件名
        /// </summary>
        virtual public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 目录路径
        /// </summary>
        virtual public string Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// 文件后缀
        /// </summary>
        virtual public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }

        //通过调用windows的程序来浏览文件
        public void BrowseFile()
        {
            switch (fileExtension)
            {
                case ("pdf"):
                    System.Diagnostics.Process.Start("AcroRd32.exe", path + fileName);
                    break;
                case ("html"):
                    System.Diagnostics.Process.Start("iexplore.exe", path + fileName);
                    break;
                case ("htm"):
                    System.Diagnostics.Process.Start("iexplore.exe", path + fileName);
                    break;
                case ("txt"):
                    System.Diagnostics.Process.Start("notepad.exe", path + fileName);
                    break;
                case ("doc"):
                    System.Diagnostics.Process.Start("winword.exe", path + fileName);
                    break;
                case ("xls"):
                    System.Diagnostics.Process.Start("excel.exe", path + fileName);
                    break;
                default:
                    //System.Diagnostics.Process.Start("mspaint.exe", path + fileName);
                    System.Diagnostics.Process.Start("iexplore.exe", path + fileName);
                    break;
            }
        }

        //判断是否支持浏览，文件类型支持,html,pdf、html、txt、word、excel
        public bool ifBrowse()
        {
            return true;
            //if ("pdf".Equals(fileExtension) || "html".Equals(fileExtension) || "htm".Equals(fileExtension) || "txt".Equals(fileExtension) || "doc".Equals(fileExtension) || "xls".Equals(fileExtension))
            //{
            //    return true;
            //}
            //return false;
        }

        //从客户端读取技术管理系统服务器端文件到本地
        public void CreateEnergyFileFromKnowledge(string energyTempletName)
        {
            try
            {
                this.FileName = energyTempletName;
                this.CreateDirectory();
                //byte[] data = med.CubeService.ReadEnergTempletByKnowledge(energyTempletName);
                //this.CreateFile(data);
            }
            catch (Exception e)
            {
                MessageBox.Show("创建文件出错：" + e.Message);
            }
        }


        //根据字节流，生成文件
        public void CreateFile(byte[] data)
        {
            try
            {
                FileInfo fi = new FileInfo(path + fileName);

                if (fileName.IndexOf(@"\") > 0)
                {
                    string subdir = fileName.Substring(0, fileName.IndexOf(@"\"));
                    if (!Directory.Exists(path + subdir))
                    {
                        Directory.CreateDirectory(path + subdir);
                    }
                }
                FileStream stream = fi.Create();
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            catch (Exception e)
            {
                throw new Exception("生成文件出错：" + e.Message);
            }
        }

        public byte[] GetBytes(string fileName)
        {
            byte[] result;
            try
            {
                result = File.ReadAllBytes(fileName);
            }
            catch
            {
                throw;
            }
            return result;
        }

        //创建目录
        public void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    System.IO.DirectoryInfo dirt = new System.IO.DirectoryInfo(path);
                    dirt.Create();
                }
            }
            catch (Exception e)
            {
                throw new Exception("生成目录出错：" + e.Message);
            }
        }

        //删除文件
        public void DeleteFile()
        {
            try
            {
                FileInfo fi = new FileInfo(path + fileName);
                fi.Delete();
            }
            catch (Exception e)
            {
                throw new Exception("删除文件出错：" + e.Message);
            }
        }

        //删除目录
        public void DeleteDirectory()
        {
            try
            {
                System.IO.DirectoryInfo dirt = new System.IO.DirectoryInfo(path);
                dirt.Delete();
            }
            catch (Exception e)
            {
                throw new Exception("删除目录出错：" + e.Message);
            }
        }


        //形成流程的方法
        public void ExploreFileMain(byte[] data)
        {
            try
            {
                this.CreateDirectory();
                this.CreateFile(data);
                this.BrowseFile();
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("系统找不到指定的文件") >= 0)
                {
                    throw new Exception("您的系统未安装打开(." + fileExtension + ")后缀的软件！");
                }
                else
                {
                    throw new Exception("浏览文件出错：" + e.Message);
                }
            }
        }

        /// <summary>
        /// 判断指定文件是否存在系统服务器的目录下
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IfExistFileInServer(string fileName)
        {
            bool ifExist = false;
            try
            {
                ifExist = StaticMethod.IfExistFileInServer(fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show("判断指定文件是否存在系统服务器的目录下出错：" + e.Message);
            }
            return ifExist;
        }

        /// <summary>
        /// //从客户端读取技术管理系统服务器端文件到本地
        /// </summary>
        /// <param name="templetName"></param>
        public void CreateTempleteFileFromServer(string templetName)
        {
            try
            {
                this.FileName = templetName;
                this.CreateDirectory();
                byte[] data = StaticMethod.ReadTempletByServer(templetName);
                this.CreateFile(data);
            }
            catch (Exception e)
            {
                MessageBox.Show("创建文件出错：" + e.Message);
            }
        }

        //从客户端读取服务器端文件到本地
        public void CreateFileFromServer(string filePath)
        {
            try
            {
                this.FileName = filePath;
                path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "techtemp"); //"c:\\windows\\temp\\techtemp\\ReportResult\\";
                this.CreateDirectory();
                //byte[] data = med.CubeService.ReadFileFromServer(filePath);
                //path = "c:\\windows\\temp\\techtemp\\";
                //this.CreateFile(data);
            }
            catch (Exception e)
            {
                MessageBox.Show("创建文件出错：" + e.Message);
            }
        }

        public static OleDbConnection OleConn = null;
        /// <summary>
        /// 打开Excel连接
        /// </summary>
        /// <returns></returns>
        public static DataSet OpenExcel(string SExcelFileName)
        {
            //打开excel2003
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + SExcelFileName + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1;'";
            //打开excel2007
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + SExcelFileName + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
            OleConn = OleConn = new System.Data.OleDb.OleDbConnection(strConn);
            if (OleConn.State != ConnectionState.Open)
                OleConn.Open();
            string sql = "SELECT * FROM  [Sheet1$]";//可是更改Sheet名称，比如sheet2，等等   

            OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
            DataSet OleDsExcle = new DataSet();
            OleDaExcel.Fill(OleDsExcle);
            //DataTable dt = new DataTable();
            //dt = OleDsExcle.Tables[0];//将结果保存在临时表中
            return OleDsExcle;
        }

        public static DataSet ReadDataFromExcel(string SExcelFileName)
        {
            Excel.ApplicationClass my = new Excel.ApplicationClass();

            try
            {
                DataSet dataSet = new DataSet();

                object objMissing = System.Reflection.Missing.Value;

                //打开工作簿   
                Excel.Workbook mybook = my.Workbooks.Open(SExcelFileName, objMissing, objMissing, objMissing,
                                                        objMissing, objMissing, objMissing,
                                                        objMissing, objMissing, objMissing,
                                                        objMissing, objMissing, objMissing, objMissing, objMissing);

                Excel.Worksheet mySheet = mybook.Sheets[1] as Excel.Worksheet;
                mySheet.Activate();
                dataSet = ReadExcelSheet(mySheet);
                mybook.Close(null, SExcelFileName, null);
                my.Quit();
                return dataSet;
            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                my.Quit();
            }
        }

        private static DataSet ReadExcelSheet(Excel.Worksheet mySheet)
        {
            try
            {
                long lngMaxCol = mySheet.UsedRange.Columns.Count;
                long lngMaxRow = mySheet.UsedRange.Rows.Count;
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(mySheet.Name.ToString());
                //for (int j = 1; j < mySheet.Columns.Count; j++)
                //{
                //    if ((mySheet.Cells[1, j] as Excel.Range).Text == "") break;
                //    lngMaxCol++;
                //}

                //for (int i = 1; i < mySheet.Rows.Count; i++)
                //{
                //    string aa = mySheet.Cells[i, 1].ToString();
                //    if ((mySheet.Cells[i, 1] as Excel.Range).Text == "") break;
                //    lngMaxRow++;
                //}
                //获得表头
                for (int j = 1; j < lngMaxCol + 1; j++)
                {
                    dataSet.Tables[0].Columns.Add((mySheet.Cells[1, j] as Excel.Range).Text.ToString());
                }
                //添加行
                for (int i = 2; i < lngMaxRow + 1; i++)
                {
                    DataRow dataRow = dataSet.Tables[0].NewRow();
                    for (int j = 1; j < lngMaxCol + 1; j++)
                    {
                        dataRow[j - 1] = (mySheet.Cells[i, j] as Excel.Range).Text;
                    }
                    dataSet.Tables[0].Rows.Add(dataRow);
                }
                return dataSet;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }


        /// <summary>
        /// 关闭Excel连接
        /// </summary>
        /// <returns></returns>
        public static void CloseExcel()
        {
            OleConn.Dispose();
        }


    }
}
