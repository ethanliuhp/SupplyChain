using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using System.IO;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Application.Business.Erp.SupplyChain.Util
{
    public class TransUtil
    {
        private static string IMAGEPOSTFIX = ".bmp";
        private static string WORDPOSTFIX = ".doc";
        private static string EXCELPOSTFIX = ".xls";
        private static string TXTPOSTFIX = ".txt";
        private static int DATADISTANCE = 5;
        private static int TABDISTANCE = 8;

        #region 文件操作
        /// <summary>
        /// 把数据文件导入到.xls文件
        /// </summary>
        /// <param name="ds"></param>
        public void ExportToExcel(DataSet ds)
        {

            //if (ds.Tables.Count != 0)
            //{
            //    //生成.xls文件完整路径名
            //    string tempFileName = GetTempFileName();
            //    object filename = EXCELPATH + tempFileName + EXCELPOSTFIX;
            //    object Nothing = System.Reflection.Missing.Value;

            //    //创建excel文件，文件名用系统时间生成精确到毫秒
            //    Excel.Application myExcel = new Excel.ApplicationClass();
            //    myExcel.Application.Workbooks.Add(Nothing);

            //    try
            //    {
            //        //把Dataset中的数据插入excel文件中
            //        int totalCount = 0;
            //        for (int k = 0; k < ds.Tables.Count; k++)
            //        {
            //            int row = ds.Tables[k].Rows.Count;
            //            int column = ds.Tables[k].Columns.Count;

            //            for (int i = 0; i < column; i++)
            //            {
            //                myExcel.Cells[totalCount + 2, 1 + i] = ds.Tables[k].Columns[i].ColumnName;
            //            }

            //            for (int i = 0; i < row; i++)
            //            {
            //                for (int j = 0; j < column; j++)
            //                {
            //                    myExcel.Cells[totalCount + 3 + i, 1 + j] = "'" + ds.Tables[k].Rows[i][j].ToString();
            //                }
            //            }
            //            totalCount = totalCount + row + 4;
            //        }

            //        try
            //        {
            //            //保存excel文件到指定的目录下，文件名用系统时间生成精确到毫秒
            //            myExcel.ActiveWorkbook._SaveAs(filename, Nothing, Nothing, Nothing, Nothing, Nothing, XlSaveAsAccessMode.xlExclusive, Nothing, Nothing, Nothing, Nothing);
            //        }
            //        catch
            //        {
            //            throw new Exception("系统找不到指定目录下的文件：  " + EXCELPATH + tempFileName + EXCELPOSTFIX);
            //            return;
            //        }
            //        //让生成的excel文件可见
            //        myExcel.Visible = true;
            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception("向excel文件中写入数据出错：  " + e.Message);
            //    }
            //}
            //else
            //{
            //    throw new Exception("No Data");
            //}
        }


        /// <summary>
        /// 把数据导入到.doc文件
        /// </summary>
        /// <param name="ds"></param>
        public void ExportToWord(DataSet ds)
        {
            if (ds.Tables.Count != 0)
            {
                string tempFileName = null;
                object filename = null;

                //    object tableBehavior = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                //    object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitFixed;

                //    object unit = Word.WdUnits.wdStory;
                //    object extend = System.Reflection.Missing.Value;
                //    object breakType = (int)Word.WdBreakType.wdSectionBreakNextPage;

                //    object count = 1;
                //    object character = Word.WdUnits.wdCharacter;

                //    object Nothing = System.Reflection.Missing.Value;

                //    try
                //    {
                //        tempFileName = GetTempFileName();

                //        //生成.doc文件完整路径名
                //        filename = DATAWORDPATH + tempFileName + WORDPOSTFIX;

                //        //创建一个word文件，文件名用系统时间生成精确到毫秒
                //        Word.Application myWord = new Word.ApplicationClass();
                //        Word._Document myDoc = new Word.DocumentClass();
                //        myDoc = myWord.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //        myDoc.Activate();

                //        //向把dataset中的表插入到word的文件中

                //        for (int totalTable = 0; totalTable < ds.Tables.Count; totalTable++)
                //        {
                //            myWord.Application.Selection.TypeText(ds.Tables[totalTable].TableName + "表的数据如下");
                //            myWord.Application.Selection.TypeParagraph();
                //            myWord.Application.Selection.TypeParagraph();
                //            Word.Range para = myWord.Application.Selection.Range;
                //            myDoc.Tables.Add(para, ds.Tables[totalTable].Rows.Count + 1, ds.Tables[totalTable].Columns.Count, ref tableBehavior, ref autoFitBehavior);
                //            for (int column = 0; column < ds.Tables[totalTable].Columns.Count; column++)
                //            {
                //                myDoc.Tables.Item(totalTable + 1).Cell(1, column + 1).Range.InsertBefore(ds.Tables[0].Columns[column].ColumnName.Trim());
                //            }
                //            for (int row = 0; row < ds.Tables[totalTable].Rows.Count; row++)
                //            {
                //                for (int column = 0; column < ds.Tables[totalTable].Columns.Count; column++)
                //                {
                //                    myDoc.Tables.Item(totalTable + 1).Cell(row + 2, column + 1).Range.InsertBefore(ds.Tables[totalTable].Rows[row][column].ToString().Trim());
                //                }
                //            }
                //            myWord.Application.Selection.EndKey(ref unit, ref extend);
                //            myWord.Application.Selection.TypeParagraph();
                //            myWord.Application.Selection.TypeParagraph();
                //            myWord.Application.Selection.InsertBreak(ref breakType);
                //        }
                //        myWord.Application.Selection.TypeBackspace();
                //        myWord.Application.Selection.Delete(ref character, ref count);
                //        myWord.Application.Selection.HomeKey(ref unit, ref extend);

                //        //保存word文件到指定的目录下
                //        try
                //        {
                //            myDoc.SaveAs(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //            myWord.Visible = true;
                //        }
                //        catch
                //        {
                //            System.Windows.Forms.MessageBox.Show("系统找不到指定目录下的文件：  " + DATAWORDPATH + tempFileName + WORDPOSTFIX);
                //            return;
                //        }
                //        //让生成的excel文件可见
                //        myWord.Visible = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        System.Windows.Forms.MessageBox.Show("向word文件中写入数据出错：  " + ex.Message);
                //    }
            }
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show("No Data");
            //}
        }
        /// <summary>
        /// 把图片文件导入到.doc文件
        /// </summary>
        /// <param name="bp"></param>
        //public void ExportToWord(Bitmap bp)
        //{
        //string tempFileName = null;
        //string bmpPath = null;
        //object filename = null;
        //object Nothing = null;
        //tempFileName = GetTempFileName();


        ////生成.bmp文件完整路径名
        //bmpPath = IMAGEPATH + tempFileName + IMAGEPOSTFIX;

        ////生成.doc文件完整路径名
        //filename = IMAGEWORDPATH + tempFileName + WORDPOSTFIX;
        //Nothing = System.Reflection.Missing.Value;

        ////创建一个word文件，文件名用系统时间生成精确到毫秒
        ////Word.Application myWord = new Word.ApplicationClass();
        ////Word._Document myDoc = new Word.DocumentClass();
        ////myDoc = myWord.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

        //try
        //{
        //    //把bitmap对象保存到系统所生成文件完整路径中
        //    bp.Save(bmpPath);
        //}
        //catch
        //{
        //    System.Windows.Forms.MessageBox.Show("系统找不到指定目录下的文件：  " + bmpPath);
        //    return;
        //}

        //try
        //{
        //    //往word文件中插入图片
        //    myDoc.InlineShapes.AddPicture(bmpPath, ref Nothing, ref Nothing, ref Nothing);
        //}
        //catch
        //{
        //    System.Windows.Forms.MessageBox.Show("系统找不到指定目录下的文件：  " + bmpPath);
        //    return;
        //}

        //try
        //{
        //    //保存word文件到指定的目录下
        //    myDoc.SaveAs(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
        //}
        //catch
        //{
        //    System.Windows.Forms.MessageBox.Show("系统找不到指定目录下的文件：  " + IMAGEWORDPATH + tempFileName + WORDPOSTFIX);
        //    return;
        //}

        ////让生成的word文件可见
        //myWord.Visible = true;
        //}


        /// <summary>
        /// 把数据文件导入到.txt文件
        /// </summary>
        /// <param name="ds"></param>
        public static void ExportToTxt(DataSet ds, string txtName)
        {
            string TXTPATH = @"C:\folder\";

            if (ds.Tables.Count != 0)
            {
                string tempFileName = null;
                // tempFileName = GetTempFileName();
                tempFileName = txtName;


                //创建一个.txt文件，文件名用系统时间生成精确到毫秒
                FileInfo file = new FileInfo(TXTPATH + tempFileName + TXTPOSTFIX);

                if (!Directory.Exists(file.DirectoryName))
                    Directory.CreateDirectory(file.DirectoryName);

                StreamWriter textFile = new StreamWriter(file.FullName, true, Encoding.Default);
                //textFile = file.CreateText();

                //把Dataset中的数据写入.txt文件中
                for (int totaltable = 0; totaltable < ds.Tables.Count; totaltable++)
                {
                    //统计dataset中当前表的行数
                    int row = ds.Tables[totaltable].Rows.Count;

                    //统计dataset中当前表的列数
                    int column = ds.Tables[totaltable].Columns.Count;

                    //用于统计当前表中每列记录中字符数最长的字符串的长度之和
                    int totalLength = 0;

                    //用于统计标题的长度（dataset中的表名的length+"表的数据如下"的length）
                    int titleLength = 0;

                    //统计每列记录中字符数最长的字符串的长度
                    //int[] columnLength = new int[column];
                    //for (int i = 0; i < column; i++)
                    //{
                    //    columnLength[i] = ds.Tables[totaltable].Columns[i].ColumnName.ToString().Length;
                    //}
                    //for (int i = 0; i < row; i++)
                    //{
                    //    for (int j = 0; j < column; j++)
                    //    {
                    //        if (ds.Tables[totaltable].Rows[i][j].ToString().Length > columnLength[j])
                    //        {
                    //            columnLength[j] = ds.Tables[totaltable].Rows[i][j].ToString().Length;
                    //        }
                    //    }
                    //}


                    //统计当前表中每列记录中字符数最长的字符串的长度之和
                    //for (int i = 0; i < column; i++)
                    //{
                    //    totalLength = totalLength + columnLength[i] + DATADISTANCE;
                    //}
                    //totalLength = totalLength + 2 * TABDISTANCE - DATADISTANCE;

                    //统计标题的长度（dataset中的当前表名的length+"表的数据如下"的length）
                    //titleLength = ds.Tables[totaltable].TableName.ToString().Length + "表的数据如下".Length * 2;

                    //把标题写入.txt文件中
                    //for (int i = 0; i < (int)((totalLength - titleLength) / 2); i++)
                    //{
                    //    textFile.Write(' ');
                    //}
                    //textFile.Write(ds.Tables[totaltable].TableName + "表的数据如下");
                    //textFile.WriteLine();
                    //for (int i = 0; i < totalLength; i++)
                    //{
                    //    textFile.Write('*');
                    //}
                    //textFile.WriteLine();
                    //textFile.Write("\t");

                    //把dataset中当前表的字段名写入.txt文件中
                    //for (int i = 0; i < column; i++)
                    //{
                    //    textFile.Write(ds.Tables[totaltable].Columns[i].ColumnName.ToString());
                    //    for (int k = 0; k < columnLength[i] - ds.Tables[totaltable].Columns[i].ColumnName.ToString().Length + DATADISTANCE; k++)
                    //    {
                    //        textFile.Write(' ');
                    //    }
                    //}
                    //textFile.WriteLine();
                    //for (int i = 0; i < totalLength; i++)
                    //{
                    //    textFile.Write('-');
                    //}
                    // textFile.WriteLine();
                    //textFile.Write("\t");

                    //把dataset中当前表的数据写入.txt文件中
                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < column; j++)
                        {

                            textFile.Write("\"" + ds.Tables[totaltable].Rows[i][j].ToString().Trim() + "\"");
                            //for (int k = 0; k < columnLength[j] - ds.Tables[totaltable].Rows[i][j].ToString().Length + DATADISTANCE; k++)
                            //{
                            textFile.Write("\t");
                            //}
                        }
                        textFile.WriteLine();
                        //  textFile.Write("\t");
                    }
                    // textFile.WriteLine();
                    //for (int i = 0; i < totalLength; i++)
                    //{
                    //    textFile.Write('-');
                    //}
                    //textFile.WriteLine();
                    //textFile.WriteLine();
                    //textFile.WriteLine();
                }

                //关闭当前的StreamWriter流
                textFile.Close();
            }
            else
            {
                throw new Exception("No Data");
            }
        }

        public string GetTempFileName()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmssfff");
        }

        public static bool AttachLengthCompare(string fileName, decimal compareFileLength)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                long length = fileInfo.Length;
                decimal fileLength = ((decimal)length) / 1024M / 1024M;
                if (fileLength > compareFileLength)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }

            return false;
        }

        public static void SaveAttachToLocal(string fileName, byte[] source)
        {
            try
            {
                File.WriteAllBytes(fileName, source);
            }
            catch
            {
                throw;
            }
        }

        public static byte[] GetBytes(string fileName)
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
        public static string GetStateName(int state)
        {
            string stateName = "";
            if (state == 0)
            {
                stateName = "定义";
            }
            else if (state == 1)
            {
                stateName = "发布";
            }
            else if (state == 2)
            {
                stateName = "作废";
            }
            return stateName;
        }
        #endregion

        #region 通用方法
        //public static DateTime GetSystemTime()
        //{
        //    return DateUtil.GetSystemTime();

        //}

        public static bool isEmpty(object temp)
        {
            if (temp == null || temp.ToString().Trim().Equals(""))
                return true;
            else
                return false;
        }

        public static string ToString(object obj)
        {
            if (isEmpty(obj))
                return "";
            else
                return Convert.ToString(obj);
        }

        public static int ToInt(object obj)
        {
            if (isEmpty(obj))
                return 0;
            else
                return Convert.ToInt32(obj);
        }

        public static long ToLong(object obj)
        {
            if (isEmpty(obj))
                return 0L;
            else
                return Convert.ToInt64(obj);
        }

        public static int GetSeasonBeginMonth(int kjy)
        {
            int month = 1;
            if (kjy >= 1 && kjy <= 3)
            {
                month = 1;
            }
            if (kjy >= 4 && kjy <= 6)
            {
                month = 4;
            }
            if (kjy >= 7 && kjy <= 9)
            {
                month = 7;
            }
            if (kjy >= 10 && kjy <= 12)
            {
                month = 10;
            }
            return month;
        }

        public static int GetSeasonEndMonth(int kjy)
        {
            int month = 1;
            if (kjy >= 1 && kjy <= 3)
            {
                month = 3;
            }
            if (kjy >= 4 && kjy <= 6)
            {
                month = 6;
            }
            if (kjy >= 7 && kjy <= 9)
            {
                month = 9;
            }
            if (kjy >= 10 && kjy <= 12)
            {
                month = 12;
            }
            return month;
        }
        //得到上季度的年份
        public static int GetLastSeasonYear(int kjn, int kjy)
        {
            int year = kjn;
            if (kjy >= 1 && kjy <= 3)
            {
                year = kjn - 1;
            }
            return year;
        }
        //得到上季度的季末月
        public static int GetLastSeasonEndMonth(int kjn, int kjy)
        {
            int month = 1;
            if (kjy >= 1 && kjy <= 3)
            {
                month = 12;
            }
            if (kjy >= 4 && kjy <= 6)
            {
                month = 3;
            }
            if (kjy >= 7 && kjy <= 9)
            {
                month = 6;
            }
            if (kjy >= 10 && kjy <= 12)
            {
                month = 9;
            }
            return month;
        }


        public static int GetLastYear(int kjn, int kjy)
        {
            int lastYear = kjn;
            if (kjy == 1)
            {
                lastYear = kjn - 1;
            }

            return lastYear;
        }

        public static int GetLastMonth(int kjn, int kjy)
        {
            int lastMonth = kjy - 1;
            if (kjy == 1)
            {
                lastMonth = 12;
            }
            return lastMonth;
        }

        public static int GetNextYear(int kjn, int kjy)
        {
            int nextYear = kjn;
            if (kjy == 12)
            {
                nextYear = kjn + 1;
            }

            return nextYear;
        }

        public static int GetNextMonth(int kjn, int kjy)
        {
            int nextMonth = kjy + 1;
            if (kjy == 12)
            {
                nextMonth = 1;
            }
            return nextMonth;
        }

        public static decimal ToDecimal(object temp)
        {
            if (isEmpty(temp))
                return 0;
            else
            {
                try
                {
                    return Convert.ToDecimal(temp.ToString());
                }
                catch { return 0; }
            }
        }

        public static DateTime ToDateTime(object obj)
        {
            if (isEmpty(obj))
                return DateTime.Parse("1900-01-01");
            else
                return Convert.ToDateTime(obj);
        }

        public static DateTime ToShortDateTime(object obj)
        {
            if (isEmpty(obj))
                return DateTime.Parse("1900-01-01");
            else
            {
                return DateTime.Parse(Convert.ToDateTime(obj).ToShortDateString());
            }
        }


        //得到一个字符串中特定字符的个数
        public static int GetStrCountInChar(string str, char c)
        {
            if (str == null || str == "")
                return 0;
            string[] temp = str.Split(c);
            return temp.Length;
        }

        /// <summary>
        /// 字符串截取
        /// 从指定截取字符的开始个数到结束个数
        /// </summary>
        /// <returns></returns>
        public static string GetStrByIndex(string str, char c, int startIndex, int endIndex)
        {
            string temp = "";
            string[] s = str.Split(c);
            int length = s.Length;
            int tt = 0;
            if (length < endIndex)
            {
                tt = length;
            }
            else
            {
                tt = endIndex;
            }

            for (int t = 0; t < tt; t++)
            {
                string st = s[t].ToString();
                if (t >= startIndex && t < endIndex)
                {
                    if (temp == "")
                    {
                        temp = st;
                    }
                    else
                    {
                        temp += c.ToString() + st;
                    }
                }
            }
            return temp;
        }
        #endregion

        #region 读取flx模板方法
        public static bool IfExistFileInServer(string fileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory;
            string disk = currDir.Substring(0, currDir.IndexOf(":") + 1);
            string kn_dir = disk + "\\THDSupplyChain_1\\flx模板\\";
            //FileInfo fFile = new FileInfo(kn_dir + fileName);
            FileInfo fFile = new FileInfo(currDir + "\\flx模板\\" + fileName);
            return fFile.Exists;
        }

        public static bool IfExistFiles(string filePath)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo fFile = new FileInfo(currDir + filePath);

            return fFile.Exists;
        }

        public static void SaveFileToServerByResult(string fileName, byte[] source)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory + "ReportResult\\";
            if (!Directory.Exists(currDir))
            {
                Directory.CreateDirectory(currDir);
            }
            try
            {
                File.WriteAllBytes(currDir + fileName, source);
            }
            catch
            {
                throw;
            }
        }

        private static string indicatorTemplateDir = "IndicatorTemplate\\";
        //文件改名
        public static void ChangeFileName(string oldFileName, string newFileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + indicatorTemplateDir;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.Move(path + oldFileName + ".flx", path + newFileName + ".flx");
        }

        //删除文件
        public static void DeleteFileFromServer(string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + indicatorTemplateDir;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                FileInfo fi = new FileInfo(path + fileName);
                fi.Delete();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void SaveFileToServer(string fileName, byte[] source)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + indicatorTemplateDir;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                File.WriteAllBytes(path + fileName, source);
            }
            catch
            {
                throw;
            }
        }

        //从系统服务器端读取flx模板文件夹目录下的文件
        public static byte[] ReadTempletByServer(string fileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory + "flx模板\\";
            string disk = currDir.Substring(0, currDir.IndexOf(":") + 1);
            //string kn_dir = disk + "\\THDSupplyChain_1\\flx模板\\";
            byte[] result;
            try
            {
                string file = currDir + fileName;
                result = File.ReadAllBytes(file);
            }
            catch
            {
                throw;
            }
            return result;
        }
        #endregion

        #region 系统常量
        /// <summary>
        /// 公司ID,名称,层次码
        /// </summary>
        public static string CompanyOpgGUID = "0_8J_eQfP068UvW6Eu_qkO";
        public static string CompanyOpgName = "中建三局集团有限公司工程总承包公司";
        public static string CompanyOpgSyscode = "91cf7bf193fb4824adbcfbf5360369b3.0_8J_eQfP068UvW6Eu_qkO.";
        /// <summary>
        /// 专业分类[土建]
        /// </summary>
        public static string ConSpecialTJ = "土建";
        /// <summary>
        /// 专业分类[安装]
        /// </summary>
        public static string ConSpecialAZ = "安装";
        //物资隔月补差出库蓝单的科目默认
        public static string ConStockOutSubjectId = "0rm13rzNf2NvLzXcr8RNW1";
        public static string ConStockOutSubjectName = "材料费其他";
        public static string ConStockOutSubjectSyscode = "1t0qn9eqT63AsDYSdfCKDQ.0WZ19quoDDferFRjWt3pMf.0khaNuCEbA2O07cdDFiMzD.0rm13rzNf2NvLzXcr8RNW1.";
        //料具科目
        public static string ConTrafficSubjectId = "0JfKMahVb8mfnwpLCk0BHS";
        public static string ConTrafficSubjectName = "材料运输费";
        public static string ConTrafficSubjectSyscode = "1t0qn9eqT63AsDYSdfCKDQ.0WZ19quoDDferFRjWt3pMf.0khaNuCEbA2O07cdDFiMzD.0JfKMahVb8mfnwpLCk0BHS.";
        //废旧物资科目
        public static string ConWasterSubjectId = "3PKNzPcZf6bOKVhXempy0n";
        public static string ConWasterSubjectName = "其他废旧物资处理";
        public static string ConWasterSubjectSyscode = "1t0qn9eqT63AsDYSdfCKDQ.0WZ19quoDDferFRjWt3pMf.0khaNuCEbA2O07cdDFiMzD.3PKNzPcZf6bOKVhXempy0n.";
        //安装料费是否结算科目
        public static string ConAZZCSubjectId = "2bD8P9jDr6euVU1qnBUl0Q";
        public static string ConAZZCSubjectName = "安装主材费";
        public static string ConAZSBSubjectId = "2zaGMnYkD1yxCgfLtMiEOe";
        public static string ConAZSBSubjectName = "安装设备费";


        //料具物资
        public static string ConTrafficMaterialId = "0z2gjqShb60u$z4idpTTdD";
        public static string ConTrafficMaterialCode = "I14200000";
        public static string ConTrafficMaterialName = "料具租赁";

        //料具站物资
        public static string ConStationTrafficMaterialId = "3987pVC3XBwuXQkhYTE_Ww";
        public static string ConStationTrafficMaterialCode = "I14500000";
        public static string ConStationTrafficMaterialName = "料具站租赁";


        //资源
        public static string MaterialResourceSysCode = "1.29BioV9QP5T9tJmw1VKARN.";//物资资源syscode

        //机械资源
        public static string MechanicalResourceId = "1OT5rRK4n4KxD1Jb_5sdGI";
        public static string MechanicalResourceCode = "1.1OT5rRK4n4KxD1Jb_5sdGI.";
        public static string MechanicalResourceName = "机械资源";

        //分包资源
        public static string SubcontractResourceId = "0661UjUO1ExB05ppYyFb0O";
        public static string SubcontractResourceCode = "1.0661UjUO1ExB05ppYyFb0O.";
        public static string SubcontractResourceName = "分包资源";

        public static string ElectResourceMaterial = "R20203002";
        #endregion

        /// <summary>
        /// C# HTTP Request请求程序模拟  向服务器送出请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string SendRequest(string param)
        {
            try
            {

                string BIMServerJsonApiUrl = ConfigurationSettings.AppSettings["BIMServerJsonApi"];

                //BIMServerJsonApiUrl = "http://www.cscec3b.com:166/Cameral/Cameral.ashx?Depart=";

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(param);
                HttpWebRequest request =
                (HttpWebRequest)HttpWebRequest.Create(BIMServerJsonApiUrl + param);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.Timeout = Int32.MaxValue;// 10 * 3600 * 1000;
                Stream sm = request.GetRequestStream();
                sm.Write(data, 0, data.Length);
                sm.Close();

                HttpWebResponse response =
                (HttpWebResponse)request.GetResponse();

                if (response.StatusCode.ToString() != "OK")
                {
                    throw new Exception(response.StatusDescription.ToString());
                }

                StreamReader myreader = new StreamReader(
                response.GetResponseStream(), Encoding.UTF8);
                string responseText = myreader.ReadToEnd();

                //JObject resultJson = JObject.Parse(responseText);

                return responseText;

            }
            catch (Exception ex)
            {
                //throw ex;
                return "";
            }
        }

        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }
    }
}
