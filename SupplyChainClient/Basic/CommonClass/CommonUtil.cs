using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.Util;
using System.Drawing;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using IRPServiceModel.Services.Document;
using ThoughtWorks.QRCode.Codec;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class CommonUtil
    {
        #region �ֳ��豸ͨ�÷���
        //���Ʒ���
        public enum MouseGestureDirection
        {
            Unknown,
            Up,
            Right,
            Down,
            Left
        }
        //��ȡ���Ʒ���
        public static MouseGestureDirection GetDirection(Point start, Point end)
        {
            const double maxAngleError = 30;

            int deltaX = end.X - start.X;
            int deltaY = end.Y - start.Y;

            double length = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            double sin = deltaX / length;
            double cos = deltaY / length;

            double angle = Math.Asin(Math.Abs(sin)) * 180 / Math.PI;

            if ((sin >= 0) && (cos < 0))
                angle = 180 - angle;
            else if ((sin < 0) && (cos < 0))
                angle = angle + 180;
            else if ((sin < 0) && (cos >= 0))
                angle = 360 - angle;

            if ((angle > 360 - maxAngleError) || (angle < 0 + maxAngleError))
                return MouseGestureDirection.Down;
            else if ((angle > 90 - maxAngleError) && (angle < 90 + maxAngleError))
                return MouseGestureDirection.Right;
            else if ((angle > 180 - maxAngleError) && (angle < 180 + maxAngleError))
                return MouseGestureDirection.Up;
            else if ((angle > 270 - maxAngleError) && (angle < 270 + maxAngleError))
                return MouseGestureDirection.Left;
            else return MouseGestureDirection.Unknown;
        }
        /// <summary>
        /// ���ƹ������Ĺ���
        /// </summary>
        /// <param name="moveType">1Ϊ���ϣ�2Ϊ����</param>
        /// <param name="offset">һ�εĹ�������</param>
        public static void ScrollBarUpAndDown(CustomDataGridView dgDetail, int moveType, int offset)
        {
            int count = dgDetail.Rows.Count;
            if (count == 0)
                return;
            if (moveType == 1)
            {
                int currIndex = dgDetail.SelectedRows[0].Index;
                if (currIndex - offset <= 0)
                {
                    dgDetail.FirstDisplayedScrollingRowIndex = 0;
                }
                else
                {
                    dgDetail.FirstDisplayedScrollingRowIndex = currIndex - offset;
                    dgDetail.Rows[currIndex - offset].Selected = true;
                }
            }
            if (moveType == 2)
            {
                int currIndex = dgDetail.SelectedRows[0].Index;
                if (currIndex + offset >= count)
                {
                    dgDetail.FirstDisplayedScrollingRowIndex = count - 1;
                }
                else
                {
                    dgDetail.FirstDisplayedScrollingRowIndex = currIndex + offset;
                    dgDetail.Rows[currIndex + offset].Selected = true;
                }

            }
        }
        #endregion

        #region ��������
        /// <summary>
        /// ����ָ���ж�д״̬
        /// </summary>
        /// <param name="grid">Grid</param>
        /// <param name="colIndexs">�����еļ���</param>
        /// <param name="isLocked">��д״̬</param>
        public static void SetGridColumnLockState(CustomFlexGrid grid, List<int> colIndexs, bool isLocked)
        {
            foreach (var colIndex in colIndexs)
            {
                for (int i = 4; i < grid.Rows; i++)
                {
                    var cell = grid.Cell(i, colIndex);
                    cell.Locked = isLocked;
                    cell.BackColor = isLocked ? SystemColors.ButtonFace : Color.White;
                }
            }
        }

        public static void SetGridRangeLockState(FlexCell.Range range, bool isLocked)
        {
            range.Locked = isLocked;
            range.BackColor = isLocked ? SystemColors.ButtonFace : Color.White;
        }

        //���ñ����ؼ������
        public static void SetFlexGridFace(CustomFlexGrid flexGrid)
        {
            flexGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            flexGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            for (int t = 1; t < flexGrid.Cols; t++)
            {
                flexGrid.Column(t).AutoFit();
            }
        }

        //���ñ����ؼ������
        public static void SetFlexGridPrintFace(CustomFlexGrid flexGrid)
        {
            //����ΪA5ֽ�ųߴ� 2015-08-06
            flexGrid.PageSetup.RightFooter = "��&Pҳ/��&Nҳ ";
            FlexCell.PageSetup pageSetup = flexGrid.PageSetup;
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            if (pd.PrinterSettings.IsValid)
            {
                for (int t = pd.PrinterSettings.PaperSizes.Count - 1; t > 0; t--)
                    //{//ֽ�ſ���Ϊ110mm,����Ϊ210mm
                    //    if (pd.PrinterSettings.PaperSizes[t].PaperName == "ƾֽ֤" ||
                    //        (pd.PrinterSettings.PaperSizes[t].Width == 433 && pd.PrinterSettings.PaperSizes[t].Height == 827))
                    if (pd.PrinterSettings.PaperSizes[t].Kind == System.Drawing.Printing.PaperKind.A5)
                    {
                        flexGrid.PageSetup.PaperSize = pd.PrinterSettings.PaperSizes[t];
                        flexGrid.PageSetup.Landscape = true;
                        flexGrid.PageSetup.BottomMargin = ClientUtil.Tofloat("0.1");
                        flexGrid.PageSetup.TopMargin = ClientUtil.Tofloat("0.4");
                        flexGrid.PageSetup.FooterMargin = ClientUtil.Tofloat("0.5");
                        flexGrid.PageSetup.HeaderMargin = ClientUtil.Tofloat("0");
                        flexGrid.PageSetup.RightMargin = ClientUtil.Tofloat("0.1");
                        flexGrid.PageSetup.LeftMargin = ClientUtil.Tofloat("0.1");
                        flexGrid.PageSetup.RightFooter = "��&Pҳ/��&Nҳ     ";
                        //flexGrid.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
                        //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("ƾֽ֤[�Զ���]", 433, 827);
                        //pageSetup.PaperSize = paperSize;
                        //pageSetup.Landscape = true;
                        pd.Dispose();
                        return;
                    }
            }

        }

        public static void SetFlexGridPrintByA4(CustomFlexGrid flexGrid)
        {
            flexGrid.PageSetup.BottomMargin = ClientUtil.Tofloat("0.2");
            flexGrid.PageSetup.TopMargin = ClientUtil.Tofloat("0.1");
            flexGrid.PageSetup.FooterMargin = ClientUtil.Tofloat("0.7");
            flexGrid.PageSetup.HeaderMargin = ClientUtil.Tofloat("0");
            flexGrid.PageSetup.RightMargin = ClientUtil.Tofloat("0.1");
            flexGrid.PageSetup.LeftMargin = ClientUtil.Tofloat("0.1");
            flexGrid.PageSetup.RightFooter = "��&Pҳ/��&Nҳ ";
        }

        public static void SetFlexGridAutoFit(CustomFlexGrid flexGrid)
        {
            for (int k = 1; k < flexGrid.Rows; k++)
            {
                for (int t = 1; t < flexGrid.Cols; t++)
                {
                    flexGrid.Cell(k, t).WrapText = true;
                }
                flexGrid.Row(k).AutoFit();
            }
        }

        public static void SetFlexGridColumnAutoFit(CustomFlexGrid flexGrid)
        {
            for (int k = 1; k < flexGrid.Cols; k++)
            {
                flexGrid.Column(k).AutoFit();
            }
        }

        public static void SetFlexGridBorder(CustomFlexGrid flexGrid, int startRow)
        {
            var range = flexGrid.Range(startRow, 1, flexGrid.Rows - 1, flexGrid.Cols - 1);
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }

        public static string SetFlexAuditPrint(CustomFlexGrid flexGrid, string id, int faxNumber)
        {
            if (SignFlag == false)//����ǩ������ 
            {
                return "";
            }
            IMonthlyPlanSrv monthlyPlanSrv = StaticMethod.GetService("MonthlyPlanSrv") as IMonthlyPlanSrv;
            IDocumentSrv documentSrv = StaticMethod.GetService("DocumentSrv") as IDocumentSrv;
            DataSet dataSet = monthlyPlanSrv.MonthAudit(id);
            DataTable dataTable = dataSet.Tables[0];
            string signStr = "";
            if (dataTable.Rows.Count > 0)
            {
                int i = faxNumber;
                flexGrid.InsertRow(i, dataTable.Rows.Count);
                flexGrid.Cell(i, 1).Text = "˳��";
                flexGrid.Cell(i, 2).Text = "������ɫ";
                flexGrid.Cell(i, 3).Text = "������Ա";
                flexGrid.Cell(i, 4).Text = "����ǩ��";
                flexGrid.Cell(i, 1).FontSize = 9;
                flexGrid.Cell(i, 2).FontSize = 9;
                flexGrid.Cell(i, 3).FontSize = 9;
                flexGrid.Cell(i, 4).FontSize = 9;
                flexGrid.Cell(i, 4).Alignment = FlexCell.AlignmentEnum.CenterCenter;

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    DataRow row = dataTable.Rows[j];
                    flexGrid.Cell(i + 1 + j, 1).Text = row["Steporder"].ToString();
                    flexGrid.Cell(i + 1 + j, 1).FontSize = 10;
                    flexGrid.Cell(i + 1 + j, 2).Text = row["RoleName"].ToString();
                    flexGrid.Cell(i + 1 + j, 3).Text = row["PerName"].ToString();
                    byte[] bt = documentSrv.GetElecSign(row["PerId"].ToString());
                    Image image = showEPhote(bt);
                    if (image != null)
                    {
                        int ih = image.Height;
                        short[] ic = convertToShort(ih);
                        //flexGrid.Row(i + 1 + j).Height = ic[0];
                        flexGrid.Row(i + 1 + j).Height = 70;
                        flexGrid.Images.Add(image, j.ToString());
                        flexGrid.Cell(i + 1 + j, 4).SetImage(j.ToString());
                        flexGrid.Cell(i + 1 + j, 4).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                        flexGrid.Row(i + 1 + j).AutoFit();

                    }
                    else
                    {
                        if (signStr.Contains(row["PerName"].ToString()) == false)
                        {
                            signStr += "[" + row["PerName"].ToString() + "]";
                        }
                    }
                    flexGrid.Column(1).AutoFit();
                    //flexGrid.Cell(i + 1 + j, 4).Text = "����ǩ��";

                }
                //�����������ֶ��뷽ʽ
                FlexCell.Range rangeAdd = flexGrid.Range(i, 1, i + dataTable.Rows.Count, 3);
                rangeAdd.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            }
            return signStr;
        }

        /// <summary>
        /// ���õ���ǩ��
        /// </summary>
        /// <param name="flexGrid">flexGrid�������</param>
        /// <param name="perId">����ǩ���˵�perId</param>
        /// <param name="rowindex">���õ���ǩ������</param>
        /// <param name="colIndex">���õ���ǩ������</param>
        public static void SetFlexSign(CustomFlexGrid flexGrid, string perId, int rowindex, int colIndex)
        {
           
            Image image = null;
#if DEBUG
            image = Image.FromFile(@"C:\Users\Administrator\Desktop\����ǩ��1.png");
#else 
            IDocumentSrv documentSrv = StaticMethod.GetService("DocumentSrv") as IDocumentSrv;
            byte[] bt = documentSrv.GetElecSign(perId);
            image = showEPhote(bt);
#endif            
            if (image != null)
            {
                int ih = image.Height;
                short[] ic = convertToShort(ih);
                string imgKey = perId + rowindex + colIndex;
                flexGrid.Row(rowindex).Height = 70;
                flexGrid.Images.Add(image, imgKey);
                flexGrid.Cell(rowindex, colIndex).SetImage(imgKey);
                flexGrid.Cell(rowindex, colIndex).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                flexGrid.Row(rowindex).AutoFit();
                //flexGrid.Column(colIndex).AutoFit();
            }
        }

        public static short[] convertToShort(int i)
        {
            short[] a = new short[2];
            a[0] = (short)(i & 0x0000ffff);          //�����͵ĵ�λȡ��,   
            a[1] = (short)(i >> 16);                     //�����͵ĸ�λȡ��.   
            return a;
        }

        public static Image showEPhote(byte[] infbytes)
        {
            //DBSservice.FileSrvSoapClient dbs = new Application.Business.Erp.SupplyChain.Client.DBSservice.FileSrvSoapClient();
            //byte[] signBytes = dbs.GetUserSign(percode);

            Image img = null; ;
            if (infbytes != null)
            {
                img = ReturnPhoto(infbytes);
            }
            return img;
        }

        //��byte[]ת����ͼƬ
        public static System.Drawing.Image ReturnPhoto(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }

        public static void SetFlexGridDetailFormat(FlexCell.Range range)
        {
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Numeric;
            range.FontBold = false;
        }

        public static void SetFlexGridDetailCenter(FlexCell.Range range)
        {
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Numeric;
            range.FontBold = false;
        }

        //���ñ�����Χ�Ŀ�ֵΪ0
        public static void SetFlexGridDefaultValue(CustomFlexGrid flexGrid, int startRow, int startCol, int endRow, int endCol)
        {
            for (int k = startRow; k < endRow; k++)
            {
                for (int t = startCol; t < endCol; t++)
                {
                    if (ClientUtil.ToString(flexGrid.Cell(k, t).Text) == "")
                    {
                        flexGrid.Cell(k, t).Text = "0";
                    }
                }
            }
        }
        #endregion

        #region �ַ�ת��
        /// <summary>
        /// ����������ת�����ַ�����ʽ
        /// </summary>
        /// <param name="digit">С����λ��</param>
        /// <returns></returns>
        public static string NumberToStringFormate(decimal d, int digit)
        {
            string temp = "";
            if (d != 0)
            {
                string formatQuantity = "###############0.";
                for (int t = 0; t < digit; t++)
                {
                    formatQuantity += "#";
                }
                temp = d.ToString(formatQuantity);
            }
            return temp;
        }

        public static string GetChineseNumber(int s)
        {
            char[] chinese = new char[] { '��', 'һ', '��', '��', '��', '��', '��', '��', '��', '��' };
            //����������ת������.
            string temp = "" + s;
            int slen = temp.Length;
            string result = "";

            if (s < 10)
            {
                result = chinese.GetValue(s) + "";
            }
            else if (s == 10)
            {
                result = "ʮ";
            }
            else if (s > 10 && s < 20)
            {
                result = "ʮ" + chinese.GetValue(int.Parse(temp.Substring(1, 1)));
            }
            else if (s >= 20 && s < 100)
            {
                result = chinese.GetValue(int.Parse(temp.Substring(0, 1))) + "ʮ" + chinese.GetValue(int.Parse(temp.Substring(1, 1)));
            }

            if (result.EndsWith("��"))
            {
                return result.Substring(0, result.Length - 1);
            }
            else
            {
                return result;
            }
        }

        public static DataSet ConvertDataSetToString(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Columns.Count == 0)
                return null;
            DataTable strdt = new DataTable();
            IList dires = new ArrayList();
            //int accdire = ds.Tables[0].Columns.IndexOf("accdire");
            int accday = ds.Tables[0].Columns.IndexOf("accday");
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ColumnName.ToUpper().EndsWith("DIRE"))
                {
                    strdt.Columns.Add(ds.Tables[0].Columns[i].ColumnName, typeof(string));
                    dires.Add(i);
                }
                else
                    strdt.Columns.Add(ds.Tables[0].Columns[i].ColumnName, ds.Tables[0].Columns[i].DataType);
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DataRow strdr = strdt.NewRow();
                for (int i = 0; i < strdt.Columns.Count; i++)
                {
                    //strdr[i] = row[i];
                    if (i == accday)
                    {
                        int dayint = int.Parse(row[i].ToString());
                        if (dayint < 1 || dayint > 31)
                            strdr[i] = DBNull.Value;
                        else
                            strdr[i] = row[i];
                    }
                    else if (dires.Contains(i) && ds.Tables[0].Columns[i].DataType == typeof(decimal))
                    {
                        if (row[i].ToString().Equals(""))
                        {
                            strdr[i] = DBNull.Value;
                        }
                        else
                        {
                            int dirint = int.Parse(row[i].ToString());
                            if (dirint == 0)
                            {
                                strdr[i] = "ƽ";
                            }
                            else if (dirint == 1)
                            {
                                strdr[i] = "��";
                            }
                            else
                            {
                                strdr[i] = "��";
                            }
                        }
                    }
                    else if (ds.Tables[0].Columns[i].DataType == typeof(decimal))
                    {
                        if (row[i].ToString().Equals("0") || (row[i].ToString().Equals("")))
                            strdr[i] = DBNull.Value;
                        else
                        {
                            strdr[i] = row[i];
                        }
                    }
                    else
                    {
                        strdr[i] = row[i];
                    }
                    //Type t = ds.Tables[0].Columns[i].DataType;
                }
                strdt.Rows.Add(strdr);
            }
            //int c=strdt.Columns.Count;
            //int r = strdt.Rows.Count;
            DataSet strds = new DataSet();
            strds.Tables.Add(strdt);
            return strds;
        }

        /// <summary>
        /// ת���ַ�����ʽ��������
        /// </summary>
        /// <param name="expression">�������ʽ</param>
        /// <param name="digit">����С����λ��</param>
        /// <returns>���ֵ</returns>
        public static string CalculateExpression(string expression, int digit)
        {
            DataTable dt = new DataTable();
            string value = dt.Compute(expression, "").ToString();
            if (value != null && !"".Equals(value))
            {
                value = Math.Round(double.Parse(value), digit) + "";
            }
            return value;

        }

        private static string GetSingleNumber(int num, bool isSimple)
        {
            if (num < 0 || num >= 10)
            {
                return "X";
            }

            List<string> chineses = null;
            if (isSimple)
            {
                chineses = new List<string>() { "��", "һ", "��", "��", "��", "��", "��", "��", "��", "��" };
            }
            else
            {
                chineses = new List<string>() { "��", "Ҽ", "��", "��", "��", "��", "½", "��", "��", "��" };
            }
            return chineses[num];
        }

        private static string GetNumberUnit(int bit)
        {
            var unit = "";
            switch (bit)
            {
                case 1:
                    unit = "";
                    break;
                case 2:
                    unit = "ʰ";
                    break;
                case 3:
                    unit = "��";
                    break;
                case 4:
                    unit = "Ǫ";
                    break;
                case 5:
                    unit = "��";
                    break;
                case 6:
                    unit = "��";
                    break;
            }
            return unit;
        }

        private static string ConvertToFourBitChinese(string numStr, bool isSimple)
        {
            if (string.IsNullOrEmpty(numStr) || numStr.Length != 4)
            {
                return string.Empty;
            }

            var vls = numStr.ToCharArray();
            StringBuilder res = new StringBuilder();
            var index = 5;
            foreach (var vl in vls)
            {
                index--;
                var tmp = 0;
                int.TryParse(vl.ToString(), out tmp);
                if (tmp == 0 && res.ToString().EndsWith("��"))
                {
                    continue;
                }

                res.Append(GetSingleNumber(tmp, isSimple));
                if (tmp != 0)
                {
                    res.Append(GetNumberUnit(index));
                }
            }

            if (res.ToString().EndsWith("��"))
            {
                return res.ToString(0, res.Length - 1);
            }
            else
            {
                return res.ToString();
            }
        }

        public static string ToChineseNumber(decimal decNum, bool isSimple)
        {
            var minVal = 0m;
            var maxVal = decimal.MaxValue;
            if (decNum < minVal || decNum > maxVal)
            {
                return string.Format("����Խ�磬������{0}-{1}", minVal, maxVal);
            }

            if (decNum == 0)
            {
                return string.Empty;
            }

            var intNumStr = decimal.Truncate(decNum).ToString();
            var surplus = intNumStr.Length % 4;
            intNumStr = intNumStr.PadLeft(intNumStr.Length + (surplus == 0 ? 0 : (4 - surplus)), '0');
            var loopCount = intNumStr.Length / 4;
            var spitVal = new Dictionary<int, string>();
            for (int i = 0; i < loopCount; i++)
            {
                spitVal.Add(loopCount - i, intNumStr.Substring(4 * i, 4));
            }

            StringBuilder sb = new StringBuilder();
            foreach (var spv in spitVal)
            {
                sb.Append(ConvertToFourBitChinese(spv.Value, isSimple));
                if (spv.Key > 1)
                {
                    sb.Append(GetNumberUnit(spv.Key + 3));
                }
            }

            sb.Append("Ԫ");
            var dms = decNum - decimal.Truncate(decNum);
            if (dms != 0)
            {
                var dmsStr = dms.ToString("N2").ToArray();
                for (int i = 2; i < dmsStr.Count(); i++)
                {
                    var tmp = 0;
                    if (int.TryParse(dmsStr[i].ToString(), out tmp) && tmp > 0)
                    {
                        sb.Append(GetSingleNumber(tmp, isSimple));
                        sb.Append(i == 2 ? "��" : "��");
                    }
                }
            }
            else
            {
                sb.Append("��");
            }

            return sb.ToString().Trim('��');
        }

        public static string ToChineseNumber(decimal num)
        {
            return ToChineseNumber(num, false);
        }

        #endregion

        #region ͨ�÷���
        /// <summary>
        /// ��ȡ��һ�����
        /// </summary>
        public static int GetPreviousFiscalYear(int fiscalYear, int fiscalMonth)
        {
            int previousYear = fiscalYear;
            if (fiscalMonth == 1)
            {
                previousYear = fiscalYear - 1;
            }
            else
            {
                previousYear = fiscalYear;
            }
            return fiscalYear;
        }
        /// <summary>
        /// ��ȡ��һ�����
        /// </summary>
        public static int GetPreviousFiscalMonth(int fiscalYear, int fiscalMonth)
        {
            int previousMonth = fiscalMonth;
            if (fiscalMonth == 1)
            {
                fiscalMonth = 12;
            }
            else
            {
                fiscalMonth = fiscalMonth - 1;
            }
            return fiscalMonth;
        }
        /// <summary>
        /// ��ȡ��ǰ����
        /// </summary>
        public static int GetCurrSeasonValue(int fiscalMonth)
        {
            int season = 1;
            if (fiscalMonth == 4 || fiscalMonth == 5 || fiscalMonth == 6)
            {
                season = 2;
            }
            if (fiscalMonth == 7 || fiscalMonth == 8 || fiscalMonth == 9)
            {
                season = 3;
            }
            if (fiscalMonth == 10 || fiscalMonth == 11 || fiscalMonth == 12)
            {
                season = 4;
            }

            return season;
        }
        /// <summary>
        /// ��ȡ���ݴ�ӡ����(000)��λ
        /// </summary>
        public static string GetPrintTimesStr(int printTimes)
        {
            string str = "";
            if (printTimes < 10)
            {
                str = "00" + printTimes;
            }
            else if (printTimes < 100)
            {
                str = "0" + printTimes;
            }
            else
            {
                str = printTimes + "";
            }
            return str;
        }
        #endregion

        #region ϵͳ����
        /// <summary>
        /// �ܲ���Ŀ��֯����
        /// </summary>
        public static string CompanyProjectCode = "0000";
        /// <summary>
        /// �Ͼ߷������
        /// </summary>
        public static string TurnMaterialMatCode = "I142";
        /// <summary>
        /// �Ͼ�վ�Ͼ߷������
        /// </summary>
        public static string TurnStationMaterialMatCode = "I145";
        /// <summary>
        /// ���ʹ�Ӧ��
        /// </summary>
        public static string SupplierCatCode = "01";
        /// <summary>
        /// �Ͼ߹�Ӧ��
        /// </summary>
        public static string SupplierCatCode1 = "02";
        /// <summary>
        /// �豸��Ӧ��
        /// </summary>
        public static string SupplierCatCode2 = "03";
        /// <summary>
        /// ����Ӧ��
        /// </summary>
        public static string SupplierCatCode3 = "04";
        /// <summary>
        /// רҵ��Ӧ��
        /// </summary>
        public static string SupplierCatCode4 = "05";
        /// <summary>
        /// ���յ�λ
        /// </summary>
        public static string SupplierCatCode5 = "06";
        /// <summary>
        /// �ֹ�����code
        /// </summary>
        public static string MaterialGGCode = "I14503";
        /// <summary>
        /// �ֹ�����code�������
        /// </summary>
        public static string MaterialCateGGCode = "I14503";
        /// <summary>
        /// �������code
        /// </summary>
        public static string MaterialWKCode = "I14502";
        /// <summary>
        /// �������code�������
        /// </summary>
        public static string MaterialCateWKCode = "I14502";
        /// <summary>
        /// �ֳֻ�Ƥ��·��
        /// </summary>
        public static string SkinPath = AppDomain.CurrentDomain.BaseDirectory + @"/mobileskin.ssk";
        /// <summary>
        /// ����ǩ������(ȡֵ�� false��true)
        /// </summary>
        public static bool SignFlag = true;
        #endregion

        #region ��ά��
        /// <summary>
        /// �������ӻ�ȡ��ά��
        /// </summary>
        /// <param name="link">����</param>
        /// <returns>���ض�ά��ͼƬ</returns>

        public static Bitmap GenDimensionalCode(string link)
        {

            Bitmap bmp = null;
            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//��ά�����(Byte��AlphaNumeric��Numeric)
                qrCodeEncoder.QRCodeScale = 6;//��ά��ߴ�(VersionΪ0ʱ��1��26x26��ÿ��1���͸߸���25

                qrCodeEncoder.QRCodeVersion = 10;//��ά���ܼ���0-40
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//��ά���������(L��7% M��15% Q��25% H��30%)
                bmp = qrCodeEncoder.Encode(link);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Invalid version !");
            }

            return bmp;
        }
        #endregion
    }
}