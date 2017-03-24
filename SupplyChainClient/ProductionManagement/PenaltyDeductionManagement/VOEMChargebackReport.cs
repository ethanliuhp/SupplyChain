using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using IRPServiceModel.Services.Common;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using System.Linq;


namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VOEMChargebackReport : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string title1 = "项目代工扣款台账";
        string flexName1 = "项目代工扣款台账.flx";
        string title2 = "项目代工扣款统计表";
        string flexName2 = "项目代工扣款统计表.flx";
        CurrentProjectInfo projectInfo;
        public VOEMChargebackReport()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin1.Value = dtpDateBegin2.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDateEnd1.Value = dtpDateEnd2.Value = DateTime.Now;
            this.fgDetail1.Rows = 1;
            LoadTempleteFile(fgDetail1, flexName1);
            LoadTempleteFile(fgDetail2, flexName2);
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvent()
        {
            btnSearch1.Click += new EventHandler(btnSearch1_Click);
            btnExcel1.Click += new EventHandler(btnExcel1_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSearch2.Click += new EventHandler(btnSearch2_Click);
            btnSearch3.Click += new EventHandler(btnSearch3_Click);
            btnExcel2.Click += new EventHandler(btnExcel2_Click);
        }

        #region 分包结算

        private void btnExcel2_Click(object sender, EventArgs e)
        {
            fgDetail2.ExportToExcel(title2, false, false, true);
        }
        private void btnSearch3_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在生成[" + title2 + "]报告...");
            try
            {
                LoadTempleteFile(fgDetail2, flexName2);
                var startDate = dtpDateBegin2.Value.Date;
                var endDate = dtpDateEnd2.Value.Date;
                var condition = string.Format(" and endtime between to_date('{0} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date('{1} 23:59:59','yyyy-mm-dd hh24:mi:ss') and t1.projectid='{2}'", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), projectInfo.Id);
                if (!string.IsNullOrEmpty(txtPenaltyRank2.Text))
                {
                    condition += " and insteadteam='" + ((SubContractProject)txtPenaltyRank2.Tag).Id + "'";
                }
                string sql = "select t3.endtime,t2.insteadteamname,t2.accountsummoney,t2.labordescript,t1.bearteamname from THD_LABORSPORADICMASTER t1,THD_LABORSPORADICDETAIL t2,thd_subcontractbalancebill t3,thd_subcontractbalancedetail t4 where t3.state=5 and t1.id=t2.parentid and t2.id=t4.frontbillguid and t3.id=t4.parentid and t1.laborstate like '%代工%'" + condition + " order by endtime desc";
                var dt = service.GetData(sql).Tables[0];
                if (dt == null || dt.Rows.Count == 0) return;
                var drs = dt.Select();
                var nameGroup = drs.GroupBy(a => a["insteadteamname"]).ToList();    // 扣款单位分组
                int rowCount = nameGroup.Count * 2 + dt.Rows.Count - 1;             // flexcell需要增加的行

                fgDetail2.AutoRedraw = false;
                fgDetail2.InsertRow(3, rowCount);
                string sTitle = string.Format("{0}至{1} {2}项目代工扣款统计表", startDate.Date.ToString("yyyy-MM-dd"), endDate.Date.ToString("yyyy-MM-dd"),projectInfo.Name);
                fgDetail2.Cell(1, 1).Text = sTitle;

                int curRow = 3;
                int n = 1;
                foreach (var name in nameGroup)
                {
                    var result = drs.Where(a => a["insteadteamname"] + string.Empty == name.Key + string.Empty);
                    var indexStr = new DigitToChnText().Convert(n.ToString(), false);       // 数字转为中文，记录扣款公司的序号
                    n++;
                    fgDetail2.Cell(curRow, 1).Text = indexStr;
                    fgDetail2.Cell(curRow, 3).Text = name.Key.ToString();
                    decimal sumMoney = 0;                   // 统计每个扣款公司的总扣款额
                    int m = 0;                              // 记录扣款项目数
                    foreach (var item in result)
                    {
                        curRow++;
                        fgDetail2.Cell(curRow, 1).Text = (m + 1).ToString();
                        fgDetail2.Cell(curRow, 2).Text = Convert.ToDateTime(item["endtime"]).ToShortDateString();
                        fgDetail2.Cell(curRow, 3).Text = item["insteadteamname"] + "";
                        fgDetail2.Cell(curRow, 3).WrapText = true;
                        fgDetail2.Cell(curRow, 4).Text = item["accountsummoney"] + "";
                        sumMoney += Convert.ToDecimal(item["accountsummoney"]);
                        fgDetail2.Cell(curRow, 5).Text = item["labordescript"] + "";
                        fgDetail2.Cell(curRow, 5).WrapText = true;
                        fgDetail2.Cell(curRow, 6).Text = item["bearteamname"] + "";
                        fgDetail2.Row(curRow).AutoFit();
                        m++;
                    }
                    curRow++;
                    fgDetail2.Range(curRow, 1, curRow, 2).Merge();          // 合并单元格
                    fgDetail2.Cell(curRow, 1).Text = "小计";
                    fgDetail2.Cell(curRow, 4).Text = sumMoney.ToString();   // 合计金额
                    curRow++;
                }

            }
            catch (Exception e1)
            {
                throw new Exception("生成[" + title2 + "]报告异常[" + e1.Message + "]");
            }
            finally
            {
                fgDetail2.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fgDetail2.BackColorBkg = System.Drawing.SystemColors.ButtonFace;

                fgDetail2.AutoRedraw = true;
                fgDetail2.Refresh();
                FlashScreen.Close();
            }
        }
        private void btnSearch2_Click(object sender, EventArgs e)
        {
            SelectTeam(txtPenaltyRank2);
        }

        #endregion

        #region 功能函数

        /// 选择代工队伍
        /// </summary>
        /// <param name="uc"></param>
        private void SelectTeam(CustomEdit uc)
        {
            VContractExcuteSelector dialogView = new VContractExcuteSelector();
            dialogView.ShowDialog();
            IList list = dialogView.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject item = list[0] as SubContractProject;
            uc.Text = item.BearerOrgName;
            uc.Tag = item;
        }
        /// 给控件加载模版文件
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="modelName"></param>
        private void LoadTempleteFile(CustomFlexGrid cfg, string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                cfg.OpenFile(path + "\\" + modelName);
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        #endregion

        #region 代工扣款

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SelectTeam(txtPenaltyRank);
        }
        private void btnSearch1_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在生成[" + title1 + "]报告...");
            try
            {
                LoadTempleteFile(fgDetail1, flexName1);
                var startDate = dtpDateBegin1.Value.Date;
                var endDate = dtpDateEnd1.Value.Date;
                var condition = string.Format(" and startdate between to_date('{0} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date('{1} 23:59:59','yyyy-mm-dd hh24:mi:ss') and projectid='{2}'", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), projectInfo.Id);
                if (!string.IsNullOrEmpty(txtPenaltyRank.Text))
                {
                    condition += " and bearteam='" + ((SubContractProject)txtPenaltyRank.Tag).Id + "'";
                }
                //string sql = "select t1.bearteamname teamname,t2.laborsubjectname subjectname,t2.labordescript taskcontent,t2.reallabornum,t2.QuantityUnitName unit,t2.accountPrice price,t2.accountsummoney summoney,t2.startdate,t1.handlepersonname name,t2.insteadteamname teamnameed,t1.descript from THD_LABORSPORADICMASTER t1,THD_LABORSPORADICDETAIL t2 where t1.state=5 and t1.id=t2.parentid and laborstate like '%代工%'" + condition + " order by startdate desc";
                //
                string sql = @"select t1.bearteamname teamname,t2.laborsubjectname subjectname,t2.labordescript taskcontent,t2.reallabornum,t2.QuantityUnitName unit,t2.accountPrice price,t2.accountsummoney summoney,t2.startdate,t1.handlepersonname name,t2.insteadteamname teamnameed,t1.descript 
                                from THD_LABORSPORADICMASTER t1,THD_LABORSPORADICDETAIL t2 
                                where t1.state=5 
                                        and t1.id=t2.parentid "
                                     +" and nvl(t2.INSTEADTEAM,' ') <>  ' ' " 
                                                + condition
                              + " order by startdate desc";
                var dt = service.GetData(sql).Tables[0];
                fgDetail1.AutoRedraw = false;
                fgDetail1.InsertRow(3, dt.Rows.Count - 1);
                string sTitle = string.Format("{0}至{1} {2}项目代工扣款台账", startDate.Date.ToString("yyyy-MM-dd"), endDate.Date.ToString("yyyy-MM-dd"),projectInfo.Name);
                fgDetail1.Cell(1, 1).Text = sTitle;
                int curRow = 0;
                decimal sumMoney = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    curRow = 3 + i;
                    fgDetail1.Cell(curRow, 1).Text = (i + 1).ToString();
                    fgDetail1.Cell(curRow, 2).Text = dr["teamname"] + "";
                    fgDetail1.Cell(curRow, 3).Text = dr["subjectname"] + "";
                    fgDetail1.Cell(curRow, 4).Text = dr["taskcontent"] + "";
                    fgDetail1.Cell(curRow, 4).WrapText = true;
                    fgDetail1.Cell(curRow, 5).Text = dr["reallabornum"] + "";
                    fgDetail1.Cell(curRow, 6).Text = dr["unit"] + "";
                    fgDetail1.Cell(curRow, 7).Text = dr["price"] + "";
                    var money = Convert.ToDecimal(dr["summoney"]);
                    sumMoney += money;
                    fgDetail1.Cell(curRow, 8).Text = money.ToString();
                    fgDetail1.Cell(curRow, 9).Text = Convert.ToDateTime(dr["startdate"]).ToShortDateString();
                    fgDetail1.Cell(curRow, 10).Text = dr["name"] + "";
                    fgDetail1.Cell(curRow, 11).Text = dr["teamnameed"] + "";
                    fgDetail1.Cell(curRow, 12).Text = dr["descript"] + "";
                    fgDetail1.Cell(curRow, 12).WrapText = true;
                }
                fgDetail1.Cell(curRow + 1, 8).Text = sumMoney.ToString();
                for (int tt = 0; tt < fgDetail1.Cols - 1; tt++)
                {
                    fgDetail1.Column(tt).AutoFit();
                }
            }
            catch (Exception e1)
            {
                throw new Exception("生成[" + title1 + "]报告异常[" + e1.Message + "]");
            }
            finally
            {
                fgDetail1.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fgDetail1.BackColorBkg = System.Drawing.SystemColors.ButtonFace;

                fgDetail1.AutoRedraw = true;
                fgDetail1.Refresh();
                FlashScreen.Close();
            }
        }
        private void btnExcel1_Click(object sender, EventArgs e)
        {
            fgDetail1.ExportToExcel(title1, false, false, true);
        }

        #endregion



        class DigitToChnText
        {
            private readonly char[] chnGenText;
            private readonly char[] chnGenDigit;

            private readonly char[] chnRMBText;
            private readonly char[] chnRMBDigit;
            private readonly char[] chnRMBUnit;

            //
            // 构造函数
            //
            public DigitToChnText()
            {
                // 一般大写中文数字组
                chnGenText = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
                chnGenDigit = new char[] { '十', '百', '千', '万', '亿' };

                // 人民币专用数字组
                chnRMBText = new char[] { '零', '壹', '贰', '叁', '肆', '伍', '陆', '染', '捌', '玖' };
                chnRMBDigit = new char[] { '拾', '佰', '仟', '萬', '億' };
                chnRMBUnit = new char[] { '角', '分' };
            }

            //
            // 主转换函数
            // 参数
            // string strDigit - 待转换数字字符串
            // bool bToRMB - 是否转换成人民币
            // 返回
            // string    - 转换成的大写字符串
            //
            public string Convert(string strDigit, bool bToRMB)
            {
                // 检查输入数字有效性
                if (CheckDigit(ref strDigit, bToRMB))
                {

                    // 定义结果字符串
                    StringBuilder strResult = new StringBuilder();

                    // 提取符号部分
                    ExtractSign(ref strResult, ref strDigit, bToRMB);

                    // 提取并转换整数和小数部分
                    ConvertNumber(ref strResult, ref strDigit, bToRMB);

                    return strResult.ToString();
                }
                else
                {
                    return "数据无效！";
                }
            }

            //
            // 转换数字
            //
            protected void ConvertNumber(ref StringBuilder strResult, ref string strDigit, bool bToRMB)
            {
                int indexOfPoint;
                if (-1 == (indexOfPoint = strDigit.IndexOf('.'))) // 如果没有小数部分
                {
                    strResult.Append(ConvertIntegral(strDigit, bToRMB));

                    if (bToRMB) // 如果转换成人民币
                    {
                        strResult.Append("圆整");
                    }
                }
                else // 有小数部分
                {
                    // 先转换整数部分
                    if (0 == indexOfPoint) // 如果“.”是第一个字符
                    {
                        if (!bToRMB) // 如果转换成一般中文大写
                        {
                            strResult.Append('零');
                        }
                    }
                    else // 如果“.”不是第一个字符
                    {
                        strResult.Append(ConvertIntegral(strDigit.Substring(0, indexOfPoint), bToRMB));
                    }

                    // 再转换小数部分
                    if (strDigit.Length - 1 != indexOfPoint) // 如果“.”不是最后一个字符
                    {
                        if (bToRMB) // 如果转换成人民币
                        {
                            if (0 != indexOfPoint) // 如果“.”不是第一个字符
                            {
                                if (1 == strResult.Length && "零" == strResult.ToString()) // 如果整数部分只是'0'
                                {
                                    strResult.Remove(0, 1); // 去掉“零”
                                }
                                else
                                {
                                    strResult.Append('圆');
                                }
                            }
                        }
                        else
                        {
                            strResult.Append('点');
                        }

                        string strTmp = ConvertFractional(strDigit.Substring(indexOfPoint + 1), bToRMB);

                        if (0 != strTmp.Length) // 小数部分有返回值
                        {
                            if (bToRMB && // 如果转换为人民币
                            0 == strResult.Length && // 且没有整数部分
                            "零" == strTmp.Substring(0, 1)) // 且返回字串的第一个字符是“零”
                            {
                                strResult.Append(strTmp.Substring(1));
                            }
                            else
                            {
                                strResult.Append(strTmp);
                            }
                        }

                        if (bToRMB)
                        {
                            if (0 == strResult.Length) // 如果结果字符串什么也没有
                            {
                                strResult.Append("零圆整");
                            }
                            // 如果结果字符串最后以"圆"结尾
                            else if ("圆" == strResult.ToString().Substring(strResult.Length - 1, 1))
                            {
                                strResult.Append('整');
                            }
                        }
                    }
                    else if (bToRMB) // 如果"."是最后一个字符，且转换成人民币
                    {
                        strResult.Append("圆整");
                    }
                }
            }
            //
            // 检查输入数字有效性
            //
            private bool CheckDigit(ref string strDigit, bool bToRMB)
            {
                bool isValidate = false;

                decimal dec = 0;
                try
                {
                    dec = decimal.Parse(strDigit);
                    isValidate = true;
                }
                catch (FormatException)
                {
                    MessageBox.Show("输入数字的格式不正确！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isValidate = false;
                }

                if (bToRMB) // 如果转换成人民币
                {
                    if (dec >= 10000000000000000m)
                    {
                        MessageBox.Show("输入数字太大，超出范围！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isValidate = false;
                    }
                    else if (dec < 0)
                    {
                        MessageBox.Show("不允许人民币为负值！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isValidate = false;
                    }
                }
                else   // 如果转换成中文大写
                {
                    if (dec <= -10000000000000000m || dec >= 10000000000000000m)
                    {
                        MessageBox.Show("输入数字太大或太小，超出范围！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isValidate = false;
                    }
                    else
                    {
                        isValidate = true;
                    }
                }

                return isValidate;
            }

            //
            // 提取输入字符串的符号
            //
            protected void ExtractSign(ref StringBuilder strResult, ref string strDigit, bool bToRMB)
            {
                // '+'在最前
                if ("+" == strDigit.Substring(0, 1))
                {
                    strDigit = strDigit.Substring(1);
                }
                // '-'在最前
                else if ("-" == strDigit.Substring(0, 1))
                {
                    if (!bToRMB)
                    {
                        strResult.Append('负');
                    }
                    strDigit = strDigit.Substring(1);
                }
                // '+'在最后
                else if ("+" == strDigit.Substring(strDigit.Length - 1, 1))
                {
                    strDigit = strDigit.Substring(0, strDigit.Length - 1);
                }
                // '-'在最后
                else if ("-" == strDigit.Substring(strDigit.Length - 1, 1))
                {
                    if (!bToRMB)
                    {
                        strResult.Append('负');
                    }
                    strDigit = strDigit.Substring(0, strDigit.Length - 1);
                }
            }

            //
            // 转换整数部分
            //
            protected string ConvertIntegral(string strIntegral, bool bToRMB)
            {
                // 去掉数字前面所有的'0'
                // 并把数字分割到字符数组中
                char[] integral = ((long.Parse(strIntegral)).ToString()).ToCharArray();

                // 定义结果字符串
                StringBuilder strInt = new StringBuilder();

                int digit;
                digit = integral.Length - 1;

                // 使用正确的引用
                char[] chnText = bToRMB ? chnRMBText : chnGenText;
                char[] chnDigit = bToRMB ? chnRMBDigit : chnGenDigit;

                // 变成中文数字并添加中文数位
                // 处理最高位到十位的所有数字
                int i;
                for (i = 0; i < integral.Length - 1; i++)
                {
                    // 添加数字
                    strInt.Append(chnText[integral[i] - '0']);

                    // 添加数位
                    if (0 == digit % 4)     // '万' 或 '亿'
                    {
                        if (4 == digit || 12 == digit)
                        {
                            strInt.Append(chnDigit[3]); // '万'
                        }
                        else if (8 == digit)
                        {
                            strInt.Append(chnDigit[4]); // '亿'
                        }
                    }
                    else         // '十'，'百'或'千'
                    {
                        strInt.Append(chnDigit[digit % 4 - 1]);
                    }

                    digit--;
                }

                // 如果个位数不是'0'
                // 或者只有一位数
                // 则添加相应的中文数字
                if ('0' != integral[integral.Length - 1] || 1 == integral.Length)
                {
                    strInt.Append(chnText[integral[i] - '0']);
                }

                // 遍历整个字符串
                i = 0;
                string strTemp; // 临时存储字符串
                int j;    // 查找“零x”结构时用
                bool bDoSomething; // 找到“零万”或“零亿”时为真

                while (i < strInt.Length)
                {
                    j = i;

                    bDoSomething = false;

                    // 查找所有相连的“零x”结构
                    while (j < strInt.Length - 1 && "零" == strInt.ToString().Substring(j, 1))
                    {
                        strTemp = strInt.ToString().Substring(j + 1, 1);

                        // 如果是“零万”或者“零亿”则停止查找
                        if (chnDigit[3].ToString() /* 万或萬 */ == strTemp ||
                         chnDigit[4].ToString() /* 亿或億 */ == strTemp)
                        {
                            bDoSomething = true;
                            break;
                        }

                        j += 2;
                    }

                    if (j != i) // 如果找到非“零万”或者“零亿”的“零x”结构，则全部删除
                    {
                        strInt = strInt.Remove(i, j - i);

                        // 除了在最尾处，或后面不是"零万"或"零亿"的情况下, 
                        // 其他处均补入一个“零”
                        if (i <= strInt.Length - 1 && !bDoSomething)
                        {
                            strInt = strInt.Insert(i, '零');
                            i++;
                        }
                    }

                    if (bDoSomething) // 如果找到"零万"或"零亿"结构
                    {
                        strInt = strInt.Remove(i, 1); // 去掉'零'
                        i++;
                        continue;
                    }

                    // 指针每次可移动2位
                    i += 2;
                }

                // 遇到“亿万”变成“亿零”或"亿"
                strTemp = chnDigit[4].ToString() + chnDigit[3].ToString(); // 定义字符串“亿万”或“億萬”
                int index = strInt.ToString().IndexOf(strTemp);
                if (-1 != index)
                {
                    if (strInt.Length - 2 != index && // 如果"亿万"不在末尾
                     (index + 2 < strInt.Length
                      && "零" != strInt.ToString().Substring(index + 2, 1))) // 并且其后没有"零"
                    {
                        strInt = strInt.Replace(strTemp, chnDigit[4].ToString(), index, 2); // 变“亿万”为“亿零”
                        strInt = strInt.Insert(index + 1, "零");
                    }
                    else // 如果“亿万”在末尾或是其后已经有“零”
                    {
                        strInt = strInt.Replace(strTemp, chnDigit[4].ToString(), index, 2); // 变“亿万”为“亿”
                    }
                }

                if (!bToRMB) // 如果转换为一般中文大写
                {
                    // 开头为“一十”改为“十”
                    if (strInt.Length > 1 && "一十" == strInt.ToString().Substring(0, 2))
                    {
                        strInt = strInt.Remove(0, 1);
                    }
                }

                return strInt.ToString();
            }

            //
            // 转换小数部分
            //
            protected string ConvertFractional(string strFractional, bool bToRMB)
            {
                char[] fractional = strFractional.ToCharArray();

                StringBuilder strFrac = new StringBuilder();

                // 变成中文数字
                int i;
                if (bToRMB) // 如果转换为人民币
                {
                    for (i = 0; i < Math.Min(2, fractional.Length); i++)
                    {
                        strFrac.Append(chnRMBText[fractional[i] - '0']);
                        strFrac.Append(chnRMBUnit[i]);
                    }

                    // 如果最后两位是“零分”则删除
                    if ("零分" == strFrac.ToString().Substring(strFrac.Length - 2, 2))
                    {
                        strFrac.Remove(strFrac.Length - 2, 2);
                    }

                    // 如果以“零角”开头
                    if ("零角" == strFrac.ToString().Substring(0, 2))
                    {
                        // 如果只剩下“零角”
                        if (2 == strFrac.Length)
                        {
                            strFrac.Remove(0, 2);
                        }
                        else // 如果还有“x分”，则删除“角”
                        {
                            strFrac.Remove(1, 1);
                        }
                    }
                }
                else // 如果转换为一般中文大写
                {
                    for (i = 0; i < fractional.Length; i++)
                    {
                        strFrac.Append(chnGenText[fractional[i] - '0']);
                    }
                }

                return strFrac.ToString();
            }
        }

    }
}
