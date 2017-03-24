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

        #region �ļ�����
        /// <summary>
        /// �������ļ����뵽.xls�ļ�
        /// </summary>
        /// <param name="ds"></param>
        public void ExportToExcel(DataSet ds)
        {

            //if (ds.Tables.Count != 0)
            //{
            //    //����.xls�ļ�����·����
            //    string tempFileName = GetTempFileName();
            //    object filename = EXCELPATH + tempFileName + EXCELPOSTFIX;
            //    object Nothing = System.Reflection.Missing.Value;

            //    //����excel�ļ����ļ�����ϵͳʱ�����ɾ�ȷ������
            //    Excel.Application myExcel = new Excel.ApplicationClass();
            //    myExcel.Application.Workbooks.Add(Nothing);

            //    try
            //    {
            //        //��Dataset�е����ݲ���excel�ļ���
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
            //            //����excel�ļ���ָ����Ŀ¼�£��ļ�����ϵͳʱ�����ɾ�ȷ������
            //            myExcel.ActiveWorkbook._SaveAs(filename, Nothing, Nothing, Nothing, Nothing, Nothing, XlSaveAsAccessMode.xlExclusive, Nothing, Nothing, Nothing, Nothing);
            //        }
            //        catch
            //        {
            //            throw new Exception("ϵͳ�Ҳ���ָ��Ŀ¼�µ��ļ���  " + EXCELPATH + tempFileName + EXCELPOSTFIX);
            //            return;
            //        }
            //        //�����ɵ�excel�ļ��ɼ�
            //        myExcel.Visible = true;
            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception("��excel�ļ���д�����ݳ�����  " + e.Message);
            //    }
            //}
            //else
            //{
            //    throw new Exception("No Data");
            //}
        }


        /// <summary>
        /// �����ݵ��뵽.doc�ļ�
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

                //        //����.doc�ļ�����·����
                //        filename = DATAWORDPATH + tempFileName + WORDPOSTFIX;

                //        //����һ��word�ļ����ļ�����ϵͳʱ�����ɾ�ȷ������
                //        Word.Application myWord = new Word.ApplicationClass();
                //        Word._Document myDoc = new Word.DocumentClass();
                //        myDoc = myWord.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //        myDoc.Activate();

                //        //���dataset�еı����뵽word���ļ���

                //        for (int totalTable = 0; totalTable < ds.Tables.Count; totalTable++)
                //        {
                //            myWord.Application.Selection.TypeText(ds.Tables[totalTable].TableName + "������������");
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

                //        //����word�ļ���ָ����Ŀ¼��
                //        try
                //        {
                //            myDoc.SaveAs(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //            myWord.Visible = true;
                //        }
                //        catch
                //        {
                //            System.Windows.Forms.MessageBox.Show("ϵͳ�Ҳ���ָ��Ŀ¼�µ��ļ���  " + DATAWORDPATH + tempFileName + WORDPOSTFIX);
                //            return;
                //        }
                //        //�����ɵ�excel�ļ��ɼ�
                //        myWord.Visible = true;
                //    }
                //    catch (Exception ex)
                //    {
                //        System.Windows.Forms.MessageBox.Show("��word�ļ���д�����ݳ�����  " + ex.Message);
                //    }
            }
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show("No Data");
            //}
        }
        /// <summary>
        /// ��ͼƬ�ļ����뵽.doc�ļ�
        /// </summary>
        /// <param name="bp"></param>
        //public void ExportToWord(Bitmap bp)
        //{
        //string tempFileName = null;
        //string bmpPath = null;
        //object filename = null;
        //object Nothing = null;
        //tempFileName = GetTempFileName();


        ////����.bmp�ļ�����·����
        //bmpPath = IMAGEPATH + tempFileName + IMAGEPOSTFIX;

        ////����.doc�ļ�����·����
        //filename = IMAGEWORDPATH + tempFileName + WORDPOSTFIX;
        //Nothing = System.Reflection.Missing.Value;

        ////����һ��word�ļ����ļ�����ϵͳʱ�����ɾ�ȷ������
        ////Word.Application myWord = new Word.ApplicationClass();
        ////Word._Document myDoc = new Word.DocumentClass();
        ////myDoc = myWord.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

        //try
        //{
        //    //��bitmap���󱣴浽ϵͳ�������ļ�����·����
        //    bp.Save(bmpPath);
        //}
        //catch
        //{
        //    System.Windows.Forms.MessageBox.Show("ϵͳ�Ҳ���ָ��Ŀ¼�µ��ļ���  " + bmpPath);
        //    return;
        //}

        //try
        //{
        //    //��word�ļ��в���ͼƬ
        //    myDoc.InlineShapes.AddPicture(bmpPath, ref Nothing, ref Nothing, ref Nothing);
        //}
        //catch
        //{
        //    System.Windows.Forms.MessageBox.Show("ϵͳ�Ҳ���ָ��Ŀ¼�µ��ļ���  " + bmpPath);
        //    return;
        //}

        //try
        //{
        //    //����word�ļ���ָ����Ŀ¼��
        //    myDoc.SaveAs(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
        //}
        //catch
        //{
        //    System.Windows.Forms.MessageBox.Show("ϵͳ�Ҳ���ָ��Ŀ¼�µ��ļ���  " + IMAGEWORDPATH + tempFileName + WORDPOSTFIX);
        //    return;
        //}

        ////�����ɵ�word�ļ��ɼ�
        //myWord.Visible = true;
        //}


        /// <summary>
        /// �������ļ����뵽.txt�ļ�
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


                //����һ��.txt�ļ����ļ�����ϵͳʱ�����ɾ�ȷ������
                FileInfo file = new FileInfo(TXTPATH + tempFileName + TXTPOSTFIX);

                if (!Directory.Exists(file.DirectoryName))
                    Directory.CreateDirectory(file.DirectoryName);

                StreamWriter textFile = new StreamWriter(file.FullName, true, Encoding.Default);
                //textFile = file.CreateText();

                //��Dataset�е�����д��.txt�ļ���
                for (int totaltable = 0; totaltable < ds.Tables.Count; totaltable++)
                {
                    //ͳ��dataset�е�ǰ��������
                    int row = ds.Tables[totaltable].Rows.Count;

                    //ͳ��dataset�е�ǰ��������
                    int column = ds.Tables[totaltable].Columns.Count;

                    //����ͳ�Ƶ�ǰ����ÿ�м�¼���ַ�������ַ����ĳ���֮��
                    int totalLength = 0;

                    //����ͳ�Ʊ���ĳ��ȣ�dataset�еı�����length+"������������"��length��
                    int titleLength = 0;

                    //ͳ��ÿ�м�¼���ַ�������ַ����ĳ���
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


                    //ͳ�Ƶ�ǰ����ÿ�м�¼���ַ�������ַ����ĳ���֮��
                    //for (int i = 0; i < column; i++)
                    //{
                    //    totalLength = totalLength + columnLength[i] + DATADISTANCE;
                    //}
                    //totalLength = totalLength + 2 * TABDISTANCE - DATADISTANCE;

                    //ͳ�Ʊ���ĳ��ȣ�dataset�еĵ�ǰ������length+"������������"��length��
                    //titleLength = ds.Tables[totaltable].TableName.ToString().Length + "������������".Length * 2;

                    //�ѱ���д��.txt�ļ���
                    //for (int i = 0; i < (int)((totalLength - titleLength) / 2); i++)
                    //{
                    //    textFile.Write(' ');
                    //}
                    //textFile.Write(ds.Tables[totaltable].TableName + "������������");
                    //textFile.WriteLine();
                    //for (int i = 0; i < totalLength; i++)
                    //{
                    //    textFile.Write('*');
                    //}
                    //textFile.WriteLine();
                    //textFile.Write("\t");

                    //��dataset�е�ǰ�����ֶ���д��.txt�ļ���
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

                    //��dataset�е�ǰ��������д��.txt�ļ���
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

                //�رյ�ǰ��StreamWriter��
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
                stateName = "����";
            }
            else if (state == 1)
            {
                stateName = "����";
            }
            else if (state == 2)
            {
                stateName = "����";
            }
            return stateName;
        }
        #endregion

        #region ͨ�÷���
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
        //�õ��ϼ��ȵ����
        public static int GetLastSeasonYear(int kjn, int kjy)
        {
            int year = kjn;
            if (kjy >= 1 && kjy <= 3)
            {
                year = kjn - 1;
            }
            return year;
        }
        //�õ��ϼ��ȵļ�ĩ��
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


        //�õ�һ���ַ������ض��ַ��ĸ���
        public static int GetStrCountInChar(string str, char c)
        {
            if (str == null || str == "")
                return 0;
            string[] temp = str.Split(c);
            return temp.Length;
        }

        /// <summary>
        /// �ַ�����ȡ
        /// ��ָ����ȡ�ַ��Ŀ�ʼ��������������
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

        #region ��ȡflxģ�巽��
        public static bool IfExistFileInServer(string fileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory;
            string disk = currDir.Substring(0, currDir.IndexOf(":") + 1);
            string kn_dir = disk + "\\THDSupplyChain_1\\flxģ��\\";
            //FileInfo fFile = new FileInfo(kn_dir + fileName);
            FileInfo fFile = new FileInfo(currDir + "\\flxģ��\\" + fileName);
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
        //�ļ�����
        public static void ChangeFileName(string oldFileName, string newFileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + indicatorTemplateDir;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.Move(path + oldFileName + ".flx", path + newFileName + ".flx");
        }

        //ɾ���ļ�
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

        //��ϵͳ�������˶�ȡflxģ���ļ���Ŀ¼�µ��ļ�
        public static byte[] ReadTempletByServer(string fileName)
        {
            string currDir = AppDomain.CurrentDomain.BaseDirectory + "flxģ��\\";
            string disk = currDir.Substring(0, currDir.IndexOf(":") + 1);
            //string kn_dir = disk + "\\THDSupplyChain_1\\flxģ��\\";
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

        #region ϵͳ����
        /// <summary>
        /// ��˾ID,����,�����
        /// </summary>
        public static string CompanyOpgGUID = "0_8J_eQfP068UvW6Eu_qkO";
        public static string CompanyOpgName = "�н����ּ������޹�˾�����ܳа���˾";
        public static string CompanyOpgSyscode = "91cf7bf193fb4824adbcfbf5360369b3.0_8J_eQfP068UvW6Eu_qkO.";
        /// <summary>
        /// רҵ����[����]
        /// </summary>
        public static string ConSpecialTJ = "����";
        /// <summary>
        /// רҵ����[��װ]
        /// </summary>
        public static string ConSpecialAZ = "��װ";
        //���ʸ��²�����������Ŀ�ĿĬ��
        public static string ConStockOutSubjectId = "0rm13rzNf2NvLzXcr8RNW1";
        public static string ConStockOutSubjectName = "���Ϸ�����";
        public static string ConStockOutSubjectSyscode = "1t0qn9eqT63AsDYSdfCKDQ.0WZ19quoDDferFRjWt3pMf.0khaNuCEbA2O07cdDFiMzD.0rm13rzNf2NvLzXcr8RNW1.";
        //�Ͼ߿�Ŀ
        public static string ConTrafficSubjectId = "0JfKMahVb8mfnwpLCk0BHS";
        public static string ConTrafficSubjectName = "���������";
        public static string ConTrafficSubjectSyscode = "1t0qn9eqT63AsDYSdfCKDQ.0WZ19quoDDferFRjWt3pMf.0khaNuCEbA2O07cdDFiMzD.0JfKMahVb8mfnwpLCk0BHS.";
        //�Ͼ����ʿ�Ŀ
        public static string ConWasterSubjectId = "3PKNzPcZf6bOKVhXempy0n";
        public static string ConWasterSubjectName = "�����Ͼ����ʴ���";
        public static string ConWasterSubjectSyscode = "1t0qn9eqT63AsDYSdfCKDQ.0WZ19quoDDferFRjWt3pMf.0khaNuCEbA2O07cdDFiMzD.3PKNzPcZf6bOKVhXempy0n.";
        //��װ�Ϸ��Ƿ�����Ŀ
        public static string ConAZZCSubjectId = "2bD8P9jDr6euVU1qnBUl0Q";
        public static string ConAZZCSubjectName = "��װ���ķ�";
        public static string ConAZSBSubjectId = "2zaGMnYkD1yxCgfLtMiEOe";
        public static string ConAZSBSubjectName = "��װ�豸��";


        //�Ͼ�����
        public static string ConTrafficMaterialId = "0z2gjqShb60u$z4idpTTdD";
        public static string ConTrafficMaterialCode = "I14200000";
        public static string ConTrafficMaterialName = "�Ͼ�����";

        //�Ͼ�վ����
        public static string ConStationTrafficMaterialId = "3987pVC3XBwuXQkhYTE_Ww";
        public static string ConStationTrafficMaterialCode = "I14500000";
        public static string ConStationTrafficMaterialName = "�Ͼ�վ����";


        //��Դ
        public static string MaterialResourceSysCode = "1.29BioV9QP5T9tJmw1VKARN.";//������Դsyscode

        //��е��Դ
        public static string MechanicalResourceId = "1OT5rRK4n4KxD1Jb_5sdGI";
        public static string MechanicalResourceCode = "1.1OT5rRK4n4KxD1Jb_5sdGI.";
        public static string MechanicalResourceName = "��е��Դ";

        //�ְ���Դ
        public static string SubcontractResourceId = "0661UjUO1ExB05ppYyFb0O";
        public static string SubcontractResourceCode = "1.0661UjUO1ExB05ppYyFb0O.";
        public static string SubcontractResourceName = "�ְ���Դ";

        public static string ElectResourceMaterial = "R20203002";
        #endregion

        /// <summary>
        /// C# HTTP Request�������ģ��  ��������ͳ�����
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