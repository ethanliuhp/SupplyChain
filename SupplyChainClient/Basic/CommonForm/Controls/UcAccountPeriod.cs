using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using IRPServiceModel.Services.Common;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonForm.Controls
{
    public partial class UcAccountPeriod : UserControl
    {
        private bool isInit = false;
        private IGWBSTreeSrv commSrv = null;
        private Hashtable periodCache = null;

        public UcAccountPeriod()
        {
            InitializeComponent();

            InitComboBox();
        }

        private IGWBSTreeSrv CommonMethodSrv
        {
            get
            {
                if (commSrv == null)
                {
                    commSrv = StaticMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;
                }

                return commSrv;
            }
        }

        public delegate void AfterSelectedAccountPeriodEvntHandler(UcAccountPeriod sender);

        /// <summary>
        /// 参数：UcAccountPeriod sender
        /// </summary>
        public AfterSelectedAccountPeriodEvntHandler AfterSelectedAccountPeriodEvnt;

        public AccountPeriod SelectedPeriod
        {
            get
            {
                return cmbAccountPeriod.SelectedItem as AccountPeriod;
            }
            set
            {
                cmbAccountPeriod.SelectedItem = value;
            }
        }

        /// <summary>
        /// 设置默认显示期间，形如201603
        /// </summary>
        /// <param name="vl">形如201603</param>
        public void SetDefaultPeriod(string vl)
        {
            if (string.IsNullOrEmpty(vl) || vl.Length != 6)
            {
                return;
            }

            cmbAccountPeriod.Text = vl;

            GetAccountPeriodByYearAndMonth();
        }

        private void InitComboBox()
        {
            cmbAccountPeriod.Text = "请输入年份";
            cmbAccountPeriod.ForeColor = Color.Gray;
            SelectedPeriod = null;

            isInit = true;
        }

        private List<AccountPeriod> GetAccountPeriodByCondition(int year, int mon)
        {
            try
            {
                if (periodCache == null)
                {
                    periodCache = new Hashtable();
                }

                string key = string.Format("{0}{1}", year, mon.ToString().PadLeft(2, '0'));
                if (periodCache.ContainsKey(key))
                {
                    return periodCache[year] as List<AccountPeriod>;
                }

                var sql = string.Format("select p.fiscalmonth,p.fiscalyear,p.enddate from ResFiscalPeriodDet p where p.fiscalyear = {0} ", year);
                if (mon >0 )
                {
                    sql = sql + string.Format(" and p.fiscalmonth = {0}", mon); 
                }
                var ds = CommonMethodSrv.SearchSQL(sql);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }

                var retList = new List<AccountPeriod>();
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var ap = new AccountPeriod();
                    ap.AccountMonth = Convert.ToInt32(ds.Tables[0].Rows[i]["fiscalmonth"]);
                    ap.AccountYear = Convert.ToInt32(ds.Tables[0].Rows[i]["fiscalyear"]);
                    ap.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["enddate"]);

                    retList.Add(ap);
                }
                periodCache.Add(key, retList);

                return retList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("获取会计期间信息失败：{0}", ex.Message));
                return null;
            }
        }

        private void GetAccountPeriodByYear()
        {
            int tmp = 0;
            if (!int.TryParse(cmbAccountPeriod.Text.Trim(), out tmp) || tmp < DateTime.MinValue.Year || tmp > DateTime.MaxValue.Year)
            {
                cmbAccountPeriod.Text = string.Empty;
                errorProvider1.SetError(cmbAccountPeriod, "请输入正确的会计年份再回车确认，如2016");
                return;
            }

            errorProvider1.Clear();
            var ds = GetAccountPeriodByCondition(tmp, 0);
            cmbAccountPeriod.DataSource = ds;
            cmbAccountPeriod.DisplayMember = "DisplayText";

            if (ds == null || ds.Count == 0)
            {
                cmbAccountPeriod.Text = string.Empty;
                cmbAccountPeriod.DroppedDown = false;

                var ms = string.Format("没有找到{0}年的会计期间，请联系系统管理员设置会计期间", tmp);
                errorProvider1.SetError(cmbAccountPeriod, ms);
                MessageBox.Show(ms);
            }
            else
            {
                cmbAccountPeriod.SelectedIndex = 0;
                cmbAccountPeriod.DroppedDown = ds.Count > 1;
            }
        }

        private void GetAccountPeriodByYearAndMonth()
        {
            int tmp = 0;
            if (!int.TryParse(cmbAccountPeriod.Text.Trim().Substring(0, 4), out tmp) || tmp < DateTime.MinValue.Year || tmp > DateTime.MaxValue.Year)
            {
                cmbAccountPeriod.Text = string.Empty;
                errorProvider1.SetError(cmbAccountPeriod, "请输入正确的会计期间再回车确认，如201603");
                return;
            }

            int mTmp = 0;
            if (!int.TryParse(cmbAccountPeriod.Text.Trim().Substring(4, 2), out mTmp) || mTmp < 1 || mTmp > 12)
            {
                cmbAccountPeriod.Text = string.Empty;
                errorProvider1.SetError(cmbAccountPeriod, "请输入正确的会计期间再回车确认，如201603");
                return;
            }

            errorProvider1.Clear();
            var ds = GetAccountPeriodByCondition(tmp, mTmp);
            cmbAccountPeriod.DisplayMember = "DisplayText";
            cmbAccountPeriod.DataSource = ds;

            if (ds == null || ds.Count == 0)
            {
                cmbAccountPeriod.DroppedDown = false;
                MessageBox.Show(string.Format("没有找到{0}年{1}月的会计期间，请联系系统管理员设置会计期间", tmp, mTmp));
            }
            else
            {
                cmbAccountPeriod.DroppedDown = ds.Count > 1;
            }
        }

        private void cmbAccountPeriod_Enter(object sender, EventArgs e)
        {
            if (isInit)
            {
                cmbAccountPeriod.Text = string.Empty;
                cmbAccountPeriod.ForeColor = Color.Black;

                isInit = false;
            }
        }

        private void cmbAccountPeriod_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbAccountPeriod.Text.Trim()) || SelectedPeriod == null)
            {
                InitComboBox();
            }
            else
            {
                cmbAccountPeriod.Text = SelectedPeriod.DisplayText;
            }
        }

        private void cmbAccountPeriod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                cmbAccountPeriod.Text = string.Empty;
                SelectedPeriod = null;
            }          
        }

        private void cmbAccountPeriod_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || SelectedPeriod != null)
            {
                return;
            }

            if (cmbAccountPeriod.Text.Length == 4)
            {
                GetAccountPeriodByYear();
            }
            else if (cmbAccountPeriod.Text.Length == 6)
            {
                GetAccountPeriodByYearAndMonth();
            }
            else
            {
                cmbAccountPeriod.Text = string.Empty;
                errorProvider1.SetError(cmbAccountPeriod, "请输入正确的会计年份再回车确认，如2016");
            }
        }

        private void cmbAccountPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AfterSelectedAccountPeriodEvnt != null)
            {
                AfterSelectedAccountPeriodEvnt(this);
            }
        }
    }

}
