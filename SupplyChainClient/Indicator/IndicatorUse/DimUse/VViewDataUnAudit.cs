using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;

using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewDataUnAudit : TBasicDataView
    {
        private MIndicatorUse model = new MIndicatorUse();
        private string front_kjn;
        private string front_kjy;
        private string kjn;
        private string kjy;
        
        public VViewDataUnAudit()
        {
            InitializeComponent();
        }

        internal void Start()
        {
            GetFrontKjq();
            IintialEvents();
            InitialControls();
        }

        private void GetFrontKjq()
        {
            kjn = ConstObject.LoginDate.Year + "";
            kjy = ConstObject.LoginDate.Month + "";
            if (ConstObject.LoginDate.Month == 1)
            {
                front_kjn = (ConstObject.LoginDate.Year - 1) + "";
                front_kjy = "12";
            }
            else
            {
                front_kjn = ConstObject.LoginDate.Year + "";
                front_kjy = (ConstObject.LoginDate.Month - 1) + "";
                if (int.Parse(front_kjy) < 10)
                {
                    front_kjy = "0" + front_kjy;
                }
            }

        }

        private void InitialControls()
        {
            colSelect.FalseValue = false;
            colSelect.TrueValue = true;

            DateTime dt = DateTime.Now;
            txtDisDateStart.Text = dt.AddMonths(-1).ToShortDateString();
            txtDisDateEnd.Text = dt.ToShortDateString();

            btnSearch.Focus();

            //把分发流水号设为不可见
            txtDisSerial.Visible = false;
            lblDisSerial.Visible = false;
        }

        private void IintialEvents()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnUnAudit.Click += new EventHandler(btnUnAudit_Click);
            chkall.CheckedChanged += new EventHandler(chkall_CheckedChanged);
            btnReset.Click += new EventHandler(btnReset_Click);
        }

        void btnReset_Click(object sender, EventArgs e)
        {
            txtViewName.Text = "";
            txtDisSerial.Text = "";
            DateTime dt = DateTime.Now;
            txtDisDateStart.Text = dt.AddMonths(-1).ToShortDateString();
            txtDisDateEnd.Text = dt.ToShortDateString();

        }

        void chkall_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvViewDis.Rows)
            {
                row.Cells[colSelect.Name].Value = chkall.Checked;
            }
        }

        void btnUnAudit_Click(object sender, EventArgs e)
        {
            IList auditList = new ArrayList();
            IList deleteRow = new ArrayList();

            foreach (DataGridViewRow row in dgvViewDis.Rows)
            { 
                object selected=row.Cells[colSelect.Name].Value;
                if (selected != null && ((bool)selected) == true)
                {
                    deleteRow.Add(row);
                    ViewDistribute vd = row.Tag as ViewDistribute;
                    if (vd == null) continue;

                    vd.StateName = front_kjn + front_kjy;
                    auditList.Add(vd);
                }
            }
            try
            {
                model.ViewSrv.SaveViewDistribute(auditList);
                foreach (DataGridViewRow row in deleteRow)
                {
                    dgvViewDis.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("反审分发视图出错。",ex);
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            //视图名称
            string disViewName = txtViewName.Text;
            if (disViewName != null && !disViewName.Trim().Equals(""))
            {
                oq.AddCriterion(Expression.Like("ViewName", "%" + disViewName + "%"));
            }
            //分发流水号
            string disSerial = txtDisSerial.Text;
            if (disSerial != null && !disSerial.Trim().Equals(""))
            {
                oq.AddCriterion(Expression.Like("DistributeSerial", "%" + disSerial + "%"));
            }
            //分发日期
            DateTime dt = DateTime.Now;
            string disDateStart = txtDisDateStart.Text;
            if (DateTime.TryParse(disDateStart, out dt) == false)
            {
                KnowledgeMessageBox.InforMessage("日期格式不对。");
                txtDisDateStart.Focus();
                return;
            }
            oq.AddCriterion(Expression.Ge("DistributeDate",DateTime.Parse(disDateStart)));
            string disDataEnd = txtDisDateEnd.Text;
            if (DateTime.TryParse(disDataEnd, out dt) == false)
            {
                KnowledgeMessageBox.InforMessage("日期格式不对。");
                txtDisDateEnd.Focus();
                return;
            }
            oq.AddCriterion(Expression.Le("DistributeDate", DateTime.Parse(disDataEnd)));

            //查询己提交的分发视图
            oq.AddCriterion(Expression.Le("StateName", int.Parse(kjn + kjy)));

            oq.AddFetchMode("TheJob", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Author", NHibernate.FetchMode.Eager);

            try
            {
                IList disViewList = model.ViewSrv.GetViewDistributeByQuery(oq);
                if (disViewList == null || disViewList.Count == 0)
                {
                    KnowledgeMessageBox.InforMessage("没有检索到数据。");
                    return;
                }
                LoadDisView(disViewList);
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("检索数据出错。");
            }
        }

        private void LoadDisView(IList disViewList)
        {
            dgvViewDis.Rows.Clear();
            if (disViewList == null || disViewList.Count == 0) return;
            foreach (ViewDistribute vd in disViewList)
            {
                int rowIndex = dgvViewDis.Rows.Add();
                DataGridViewRow row = dgvViewDis.Rows[rowIndex];
                row.Tag = vd;
                row.Cells[colViewName.Name].Value = vd.ViewName;
                if (vd.DistributeDate != null)
                {
                    row.Cells[colDisDate.Name].Value = vd.DistributeDate.ToShortDateString();
                }
                
                row.Cells[colDisSerial.Name].Value = vd.DistributeSerial;
                row.Cells[colState.Name].Value = vd.StateName;
                if (vd.TheJob != null)
                {
                    row.Cells[colJobName.Name].Value = vd.TheJob.Name;
                }
                if (vd.Author != null)
                {
                    row.Cells[colAuthor.Name].Value = vd.Author.Name;
                }
            }
        }

        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            switch (state)
            {
                case MainViewState.Initialize:
                    ToolMenu.LockItem(ToolMenuItem.AddNew);
                    break;
            }
        }
    }
}