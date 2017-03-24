using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VDatePeriodDefine : TBasicDataView
    {
        private MFinanceMultData mOperate = null;

        public VDatePeriodDefine()
        {
            InitializeComponent();
            
            InitEvents();

            InitData();
        }

        private void InitData()
        {
            mOperate = new MFinanceMultData();

            var years = new List<KeyValuePair<int, string>>();
            for (var y = DateTime.Now.Year + 10; y >= 2010; y--)
            {
                years.Add(new KeyValuePair<int, string>(y, string.Concat(y, "年")));
            }

            cmbYear.DisplayMember = "Value";
            cmbYear.ValueMember = "Key";
            cmbYear.DataSource = years;
            cmbYear.SelectedValue = DateTime.Now.Year;
        }

        private  void InitEvents()
        {
            dgMaster.BorderStyle = BorderStyle.FixedSingle;
            dgMaster.AutoGenerateColumns = false;
            dgMaster.RowPostPaint += dgMaster_RowPostPaint;

            cmbYear.SelectedIndexChanged += periodData_Changed;
            chkShowInValid.CheckedChanged += chkShowInValid_CheckedChanged;

            btnSave.Click += btnSave_Click;
            btnCreate.Click += periodData_Changed;
        }

        private int GetWeekOffsetDay(DayOfWeek wk)
        {
            var offset = 0;
            switch (wk)
            {
                case DayOfWeek.Monday:
                    offset= 0;
                    break;
                case DayOfWeek.Tuesday:
                    offset = 1;
                    break;
                case DayOfWeek.Wednesday:
                    offset = 2;
                    break;
                case DayOfWeek.Thursday:
                    offset = 3;
                    break;
                case DayOfWeek.Friday:
                    offset = 4;
                    break;
                case DayOfWeek.Saturday:
                    offset = 5;
                    break;
                case DayOfWeek.Sunday:
                    offset = 6;
                    break;
            }

            return offset;
        }

        private DatePeriodDefine NewDatePeriodDefine()
        {
            var nw = new DatePeriodDefine();
            nw.CreateDate = DateTime.Now;
            nw.CreatePerson = ConstObject.TheLogin.ThePerson;
            nw.CreatePersonName = nw.CreatePerson.Name;
            
            nw.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            nw.OperOrgInfoName = nw.OperOrgInfo.Name;
            nw.OpgSysCode = nw.OperOrgInfo.SysCode;

            return nw;
        }

        private IList CreatePeriodItem(IList pList, int year)
        {
            var periodList = pList.OfType<DatePeriodDefine>().ToList();
            var yearPeriod = periodList.Find(p => p.PeriodCode.Equals(year.ToString()));
            if (yearPeriod == null)
            {
                yearPeriod = NewDatePeriodDefine();
                yearPeriod.PeriodCode = year.ToString();
                yearPeriod.PeriodName = string.Format("{0}年", year);
                yearPeriod.BeginDate = new DateTime(year, 1, 1);
                yearPeriod.EndDate = yearPeriod.BeginDate.AddYears(1).AddDays(-1);
                yearPeriod.PeriodType = PeriodType.Year;

                periodList.Add(yearPeriod);
            }

            for (var i = 1; i <= 4; i++)
            {
                #region 季度

                var quart = string.Concat(year, i);
                DatePeriodDefine qPeriod = null;
                qPeriod = periodList.Find(p => p.PeriodCode.Equals(quart));
                if (chkQuarter.Checked)
                {
                    if (qPeriod == null)
                    {
                        qPeriod = NewDatePeriodDefine();
                        qPeriod.PeriodCode = quart;
                        qPeriod.PeriodName = string.Format("{0}年{1}季度", year, i);
                        qPeriod.BeginDate = new DateTime(year, (i - 1)*3 + 1, 1);
                        qPeriod.EndDate = qPeriod.BeginDate.AddMonths(3).AddDays(-1);
                        qPeriod.PeriodType = PeriodType.Quarter;
                        qPeriod.ParentPeriod = yearPeriod;

                        periodList.Add(qPeriod);
                    }
                    else if (qPeriod.PeriodType == PeriodType.InValid)
                    {
                        qPeriod.PeriodType = PeriodType.Quarter;
                    }
                }
                else if (qPeriod != null)
                {
                    qPeriod.PeriodType = PeriodType.InValid;
                }

                #endregion

                for (int j = 1; j <= 3; j++)
                {
                    #region 月

                    var mon = string.Concat(quart, ((i - 1)*3 + j).ToString().PadLeft(2, '0'));
                    var bgDate = new DateTime(year, (i - 1)*3 + j, 1);
                    var endDate = bgDate.AddMonths(1).AddDays(-1);
                    DatePeriodDefine monPeriod = null;
                    monPeriod = periodList.Find(p => p.PeriodCode.Equals(mon));
                    if (chkMonth.Checked)
                    {
                        if (monPeriod == null)
                        {
                            monPeriod = NewDatePeriodDefine();
                            monPeriod.PeriodCode = mon;
                            monPeriod.PeriodName = string.Format("{0}年{1}月", year, (i - 1) * 3 + j);
                            monPeriod.BeginDate = bgDate;
                            monPeriod.EndDate = endDate;
                            monPeriod.PeriodType = PeriodType.Month;
                            monPeriod.ParentPeriod = qPeriod ?? yearPeriod;
                            periodList.Add(monPeriod);
                        }
                        else if (monPeriod.PeriodType == PeriodType.InValid)
                        {
                            monPeriod.PeriodType = PeriodType.Month;
                        }
                    }
                    else if (monPeriod != null)
                    {
                        monPeriod.PeriodType = PeriodType.InValid;
                    }

                    #endregion

                    var offsetDay = GetWeekOffsetDay(bgDate.DayOfWeek);
                    var totalWeek = (endDate.Day + offsetDay)/7;
                    if ((endDate.Day + offsetDay) % 7>0)
                    {
                        totalWeek++;
                    }
                    for (int k = 1; k <= totalWeek; k++)
                    {
                        #region 周

                        var wk = string.Concat(mon, k);
                        var wkPeriod = periodList.Find(p => p.PeriodCode.Equals(wk));
                        if(chkWeek.Checked)
                        {
                            if (wkPeriod == null)
                            {
                                wkPeriod = NewDatePeriodDefine();
                                wkPeriod.PeriodCode = wk;
                                wkPeriod.PeriodName = string.Format("{0}年{1}季度{2}月第{3}周", year, i, (i - 1) * 3 + j, k);
                                if (k == 1)
                                {
                                    wkPeriod.BeginDate = bgDate;
                                    wkPeriod.EndDate = wkPeriod.BeginDate.AddDays(7 - offsetDay - 1);
                                }
                                else if (k == totalWeek)
                                {
                                    wkPeriod.BeginDate = bgDate.AddDays(7*(k - 1) - offsetDay);
                                    wkPeriod.EndDate =
                                        wkPeriod.BeginDate.AddDays(bgDate.AddMonths(1).AddDays(-1).Day -
                                                                   wkPeriod.BeginDate.Day);
                                }
                                else
                                {
                                    wkPeriod.BeginDate = bgDate.AddDays(7*(k - 1) - offsetDay);
                                    wkPeriod.EndDate = wkPeriod.BeginDate.AddDays(6);
                                }
                                wkPeriod.PeriodType = PeriodType.Week;
                                if (monPeriod!=null)
                                {
                                    wkPeriod.ParentPeriod = monPeriod;
                                }
                                else if (qPeriod != null)
                                {
                                    wkPeriod.ParentPeriod = qPeriod;
                                }
                                else
                                {
                                    wkPeriod.ParentPeriod = yearPeriod;
                                }
                                periodList.Add(wkPeriod);
                            }
                            else if (wkPeriod.PeriodType == PeriodType.InValid)
                            {
                                wkPeriod.PeriodType = PeriodType.Week;
                            }
                        }
                        else if (wkPeriod != null)
                        {
                            wkPeriod.PeriodType = PeriodType.InValid;
                        }

                        #endregion
                    }
                }
            }

            return periodList.OrderBy(p => p.PeriodCode).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var list = dgMaster.DataSource as List<DatePeriodDefine>;
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有数据可供保存");
                return;
            }

            try
            {
                mOperate.FinanceMultDataSrv.SaveOrUpdateList(list);

                MessageBox.Show("保存成功");

                dgMaster.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存时间期间定义失败：{0}", ex.Message));
            }
        }

        private void periodData_Changed(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在加载数据，请稍候...");

                var year = (int)cmbYear.SelectedValue;
                IList periodList = mOperate.FinanceMultDataSrv.GetDatePeriodDefineByYear(year, false);
                if (periodList == null)
                {
                    periodList = new List<DatePeriodDefine>();
                    chkMonth.Checked = true;
                }

                dgMaster.DataSource = CreatePeriodItem(periodList, year);

                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();

                MessageBox.Show("获取时间期间定义信息失败：" + ex.Message);
            }
        }

        private void dgMaster_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgMaster.Rows[e.RowIndex].Cells[colRowNo.Name].Value = e.RowIndex + 1;

                var dt = dgMaster.Rows[e.RowIndex].DataBoundItem as DatePeriodDefine;
                if (dt != null && dt.PeriodType == PeriodType.InValid)
                {
                    var rowStyle = new DataGridViewCellStyle();
                    rowStyle.ForeColor = Color.Silver;

                    dgMaster.Rows[e.RowIndex].DefaultCellStyle = rowStyle;

                    dgMaster.Rows[e.RowIndex].Cells[colRemark.Name].Value = "未启用";
                }
                else if(dt != null && string.IsNullOrEmpty(dt.Id))
                {
                    dgMaster.Rows[e.RowIndex].Cells[colRemark.Name].Value = "未保存";
                }
                else
                {
                    dgMaster.Rows[e.RowIndex].Cells[colRemark.Name].Value = string.Empty;
                }
            }
        }

        private void chkShowInValid_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgMaster.Rows.Count; i++)
            {
                var dt = dgMaster.Rows[i].DataBoundItem as DatePeriodDefine;
                if (dt != null && dt.PeriodType == PeriodType.InValid)
                {
                    dgMaster.Rows[i].Visible = !chkShowInValid.Checked;
                }
            }
        }
    }
}
