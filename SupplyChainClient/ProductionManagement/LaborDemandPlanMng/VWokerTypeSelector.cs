using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng
{
    public partial class VWokerTypeSelector : TBasicDataView
    {
        MLaborDemandPlanMng model = new MLaborDemandPlanMng();
        private LaborDemandPlanDetail curBillDetail;
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public LaborDemandPlanDetail CurBillDetail
        {
            get { return curBillDetail; }
            set { curBillDetail = value; }
        }

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VWokerTypeSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
            InitData();
        }

        public void InitData()
        {
            //给工种添加下拉列表框的信息
            VBasicDataOptr.InitWokerType(colWokerType, false);
        }

        private void InitForm()
        {
            this.Title = "工种信息引用";
        }

        private void InitEvent()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            Load += new EventHandler(VWokerTypeSelector_Load);
        }

        void VWokerTypeSelector_Load(object sender, EventArgs e)
        {
            if (Result != null && result.Count > 0)
            {
                foreach (LaborDemandWorkerType obj in Result)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail.Rows[rowIndex].Tag = obj;
                    dgDetail[colPersonNum.Name, rowIndex].Value = obj.PeopleNum;
                    dgDetail[colWokerType.Name, rowIndex].Value = obj.WorkerType;
                }
            }
            else
            {
                if (CurBillDetail != null && curBillDetail.Id != null)
                {
                    foreach (LaborDemandWorkerType obj in CurBillDetail.WorkerDetails)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        dgDetail.Rows[rowIndex].Tag = obj;
                        dgDetail[colPersonNum.Name, rowIndex].Value = obj.PeopleNum;
                        dgDetail[colWokerType.Name, rowIndex].Value = obj.WorkerType;
                    }
                }
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnOK.Focus();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            //this.result.Clear();
            if (this.dgDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有选择工种信息！");
                return;
            }
            //校验人数不能为空
            foreach (DataGridViewRow dr in this.dgDetail.Rows)
            {
                if (dr.IsNewRow) break;
                if (dr.Cells[colPersonNum.Name].Value == null)
                {
                    MessageBox.Show("人数不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colPersonNum.Name];
                    return;
                }
                else
                {
                    bool validity = true;
                    string temp_quantity = dr.Cells[colPersonNum.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("人数为数字！");
                        dgDetail.CurrentCell = dr.Cells[colPersonNum.Name];
                        return;
                    }
                }
            }
            //工种不能重复
            for (int i = 0; i < dgDetail.Rows.Count -2; i++)
            {
                //int j = dgDetail.Rows.Count -2;
                for (int j = dgDetail.Rows.Count - 3; j > i; j--)
                {
                    if (dgDetail[1, i].Value.Equals(dgDetail[1, j].Value))
                    {
                        MessageBox.Show("工种信息不可重复！");
                        return;
                    }
                }
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                LaborDemandWorkerType dtl = var.Tag as LaborDemandWorkerType;
                if (dtl == null)
                {
                    dtl = new LaborDemandWorkerType();
                }
                dtl.PeopleNum = ClientUtil.ToInt(var.Cells[colPersonNum.Name].Value);
                dtl.WorkerType = ClientUtil.ToString(var.Cells[colWokerType.Name].Value);
                result.Add(dtl);
            }
            this.btnOK.FindForm().Close();
        }

        private void dgDetail_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    DataGridViewRow dr = dgDetail.CurrentRow;
                    if (dr == null || dr.IsNewRow) return;
                    dgDetail.Rows.Remove(dr);
                    if (dr.Tag != null)
                    {
                        curBillDetail.WorkerDetails.Remove(dr.Tag as LaborDemandWorkerType);
                    }
                }
            }
        }
    }
}
