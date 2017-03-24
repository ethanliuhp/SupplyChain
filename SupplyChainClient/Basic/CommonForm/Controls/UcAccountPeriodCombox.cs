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
    public partial class UcAccountPeriodCombox : UserControl
    {
        private IGWBSTreeSrv commSrv = null;
        private List<AccountPeriod> allPeriodList = null;

        public UcAccountPeriodCombox()
        {
            InitializeComponent();

            InitEvents();

            GetAllAccountPeriod();
        }

        private AccountPeriod _selectedPeriod;
        public AccountPeriod SelectedPeriod
        {
            get { return _selectedPeriod; }
            set { _selectedPeriod = value; }
        }

        public delegate void AfterSelectedAccountPeriodEvntHandler(object sender);

        /// <summary>
        /// 参数：UcAccountPeriod sender
        /// </summary>
        public AfterSelectedAccountPeriodEvntHandler AfterSelectedAccountPeriodEvnt;

        public void SetDefaultMonth(int mon)
        {
            cmbMonth.SelectedValue = mon;
        }

        private IGWBSTreeSrv CommonMethodSrv
        {
            get
            {
                try
                {
                    if (commSrv == null)
                    {
                        commSrv = StaticMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;
                    }

                    return commSrv;
                }
                catch { return null; }
            }
        }

        private void InitEvents()
        {
            cmbYear.SelectedIndexChanged += cmbYear_SelectedIndexChanged;
            cmbMonth.SelectedIndexChanged += cmbMonth_SelectedIndexChanged;
        }

        private void GetAllAccountPeriod()
        {
            try
            {
                if (CommonMethodSrv == null)
                {
                    return;
                }

                var sql = "select p.fiscalmonth,p.fiscalyear,p.enddate from ResFiscalPeriodDet p";
                var ds = CommonMethodSrv.SearchSQL(sql);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }

                allPeriodList = new List<AccountPeriod>();
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var ap = new AccountPeriod();
                    ap.AccountMonth = Convert.ToInt32(ds.Tables[0].Rows[i]["fiscalmonth"]);
                    ap.AccountYear = Convert.ToInt32(ds.Tables[0].Rows[i]["fiscalyear"]);
                    if (ap.AccountMonth < 12)
                    {
                        ap.EndDate = new DateTime(ap.AccountYear, ap.AccountMonth + 1, 1).AddDays(-1);
                    }
                    else
                    {
                        ap.EndDate = new DateTime(ap.AccountYear + 1, 1, 1).AddDays(-1);
                    }

                    allPeriodList.Add(ap);
                }

                InitComBox();
            }
            catch (Exception ex)
            {
               MessageBox.Show(string.Format("获取会计期间信息失败：{0}", ex.Message));
            }
        }

        private void InitComBox()
        {
            if (allPeriodList == null)
            {
                return;
            }

            var q = from p in allPeriodList
                    group p by p.AccountYear into gps
                    orderby gps.Key descending
                    select new
                    {
                        Year = string.Concat(gps.Key, "年")
                    };

            var yearList = new List<string>();
            q.ToList().ForEach(y => yearList.Add(y.Year));

            cmbYear.DataSource = yearList;
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbYear.Text))
            {
                return;
            }

            var y = Convert.ToInt32(cmbYear.Text.Replace("年", ""));

            cmbMonth.DataSource = null;
            if (allPeriodList == null)
            {
                return;
            }

            var mList = allPeriodList.FindAll(f => f.AccountYear == y);
            cmbMonth.DisplayMember = "DisplayMonth";
            cmbMonth.ValueMember = "AccountMonth";
            cmbMonth.DataSource = mList;
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedPeriod = cmbMonth.SelectedItem as AccountPeriod;

            if (AfterSelectedAccountPeriodEvnt != null)
            {
                AfterSelectedAccountPeriodEvnt(this);
            }
        }
    }
}
