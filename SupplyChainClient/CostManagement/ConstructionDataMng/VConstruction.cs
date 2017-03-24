using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Domain;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng
{
    public partial class VConstruction : TBasicDataView
    {
        private MConstructionData model = new MConstructionData();
        private ConstructionData curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public ConstructionData CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VConstruction()
        {
            InitializeComponent();
            InitData();
            InitEvent();
            ModelToView();
        }

        private void InitData()
        {
            VBasicDataOptr.InitWBSCheckRequir(txtInspectionSpecail, true);
            txtInspectionType.Items.AddRange(new object[] { "工程任务检查", "专业检查" });
        }
        private void InitEvent()
        {
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            Controlfalse();
        }

        void btnSearch_Click(object sender,EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VConstructionSelect frm = new VConstructionSelect();
            frm.ShowDialog();
            ConstructionData list = frm.CurBillMaster;
            IList lists = frm.Result;
            if (list != null)
            {
                foreach (ConstructionData var in lists)
                {
                    txtConstruction.Text = var.Code;
                }
                //txtConstruction.Text = list.Code;
            }
        }

        private void Controlfalse()
        {
            txtInspectionType.Enabled = false;
            txtNum.Enabled = false;
            txtProject.Enabled = false;
            txtInspectionSpecail.Enabled = false;
            txtContent.Enabled = false;
            txtCreateDate.Enabled = false;
            txtCreatePerson.Enabled = false;
            txtProject.Enabled = false;

        }

        private void Control()
        {
            txtInspectionType.Enabled = true;
            txtNum.Enabled = true;
            txtProject.Enabled = true;
            txtInspectionSpecail.Enabled = true;
            txtContent.Enabled = true;
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <returns></returns>
        private bool ModelToView()
        {
            try
            {
                curBillMaster = new ConstructionData();
                ObjectQuery oq = new ObjectQuery();
                IList lists = model.ConstructionDataSrv.GetConstructionData(oq);
                if (lists.Count <= 0 || lists == null)
                {
                    dgDetail.Rows.Clear();
                    return false;
                }
                dgDetail.Rows.Clear();
                foreach (ConstructionData var in lists)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail.Rows[i].Tag = var;
                    this.dgDetail[colInspectionContent.Name, i].Value = var.InspectionContent;//检查内容
                    this.dgDetail[colInspectionSpecail.Name, i].Value = var.Specail;//专业
                    this.dgDetail[colInspectionType.Name, i].Value = var.InspectionType;//检查类型
                    this.dgDetail[colNum.Name, i].Value = var.SerailNum;//序号
                    this.dgDetail[colId.Name, i].Value = var.Id;
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            Control();
            this.curBillMaster = new ConstructionData();
            curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
            curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
            curBillMaster.CreateDate = ConstObject.LoginDate;//制单时间
            curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
            curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
            curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
            curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
            curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
            curBillMaster.DocState = DocumentState.Edit;//状态
            //制单人
            txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
            txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
            //制单日期
            txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
            //归属项目
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                txtProject.Tag = projectInfo;
                txtProject.Text = projectInfo.Name;
                curBillMaster.ProjectId = projectInfo.Id;
                curBillMaster.ProjectName = projectInfo.Name;
            }
        }
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModelService()) return;
                if (dgDetail.CurrentRow.Cells[colId.Name].Value != "")
                {
                    curBillMaster.Id = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colId.Name].Value);
                }
                curBillMaster = model.ConstructionDataSrv.SaveConstructionData(curBillMaster);
                //更新Caption
                this.ViewCaption = "施工专业基础表" + "-" + curBillMaster.Code;
                ModelToView();
                Controlfalse();
                MessageBox.Show("保存成功！");
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
            }

        }
        //保存
        private bool ViewToModelService()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.InspectionContent = ClientUtil.ToString(txtContent.Text);
                curBillMaster.InspectionType = ClientUtil.ToString(txtInspectionType.SelectedItem);
                curBillMaster.SerailNum = ClientUtil.ToInt(txtNum.Text);
                curBillMaster.Specail = ClientUtil.ToString(txtInspectionSpecail.SelectedItem);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }

        }
        /// <summary>
        /// 保存前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (txtNum.Text == "")
            {
                MessageBox.Show("序号不可为空！");
                return false;
            }
            else
            {
                int strNum = ClientUtil.ToInt(txtNum.Text);
                IList lists = model.ConstructionDataSrv.GetConstructionDataBySerailNum(strNum);
                if (lists.Count > 0)
                {
                    MessageBox.Show("序号不能重复！");
                    return false;
                }
            }
            return true;
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null || dr.IsNewRow) return;
            txtContent.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colInspectionContent.Name].Value);
            txtInspectionSpecail.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colInspectionSpecail.Name].Value);
            txtInspectionType.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colInspectionType.Name].Value);
            txtNum.Text = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colNum.Name].Value);
            Control();
        }
        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    curBillMaster = new ConstructionData();
                    curBillMaster.Id = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colId.Name].Value);
                    curBillMaster = model.ConstructionDataSrv.GetConstructionDateById(curBillMaster.Id);
                    if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                    {
                        if (!model.ConstructionDataSrv.DeleteByDao(curBillMaster)) return;
                        ModelToView();
                        Controlfalse();
                        MessageBox.Show("删除成功！");
                        return;
                    }
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                    return;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(err));
                return;
            }

        }
    }
}
