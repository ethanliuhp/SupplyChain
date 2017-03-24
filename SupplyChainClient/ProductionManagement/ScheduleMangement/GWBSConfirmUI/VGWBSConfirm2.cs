﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;

using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    public partial class VGWBSConfirm2 : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        private GWBSTaskConfirmMaster curBillMaster;
        CurrentProjectInfo projectInfo;
        /// <summary>
        /// 当前单据
        /// </summary>
        public GWBSTaskConfirmMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VGWBSConfirm2()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            dgDetail.ContextMenuStrip = cmsDg;

            //colMaterialFeeSettlementFlag.DataSource = Enum.GetNames(typeof(EnumMaterialFeeSettlementFlag));
            foreach (string mfsf in Enum.GetNames(typeof(EnumMaterialFeeSettlementFlag)))
            {
                colMaterialFeeSettlementFlag.Items.Add(mfsf);
            }
        }

        private void InitEvent()
        {
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);

            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);

            btnSelectGWBSDetail.Click += new EventHandler(btnProductSchedule_Click);
            //btnTaskHandler.Click += new EventHandler(btnTaskHandler_Click);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            //右键复制一条明细
            tsmiCopy.Click += new EventHandler(tsmiCopy_Click);
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSupplier.Name)
            {
                VContractExcuteSelector vces = new VContractExcuteSelector();
                vces.StartPosition = FormStartPosition.CenterScreen;
                vces.ShowDialog();
                if (vces.Result != null && vces.Result.Count > 0)
                {
                    SubContractProject scp = vces.Result[0] as SubContractProject;
                    dgDetail[e.ColumnIndex, e.RowIndex].Tag = scp;
                    dgDetail[e.ColumnIndex, e.RowIndex].Value = scp.BearerOrgName;

                    GWBSTaskConfirm detail = dgDetail.Rows[e.RowIndex].Tag as GWBSTaskConfirm;
                    if (detail == null)
                    {
                        detail = new GWBSTaskConfirm();
                    }

                    dgDetail.Rows[e.RowIndex].Tag = detail;
                    dgDetail.Rows[e.RowIndex].Cells[colTheTeamQuantityConfirmed.Name].Value = model.ProductionManagementSrv.GetTheTeamQuantityConfirmed(projectInfo, detail.GWBSDetail, scp).ToString();
                    //刷新单元格数据
                    dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex + 1];
                    dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        void btnTaskHandler_Click(object sender, EventArgs e)
        {
            //VContractExcuteSelector vces = new VContractExcuteSelector();
            //vces.StartPosition = FormStartPosition.CenterScreen;
            //vces.ShowDialog();
            //if (vces.Result != null && vces.Result.Count > 0)
            //{
            //    SubContractProject scp = vces.Result[0] as SubContractProject;
            //    txtTaskHandler.Tag = scp;
            //    txtTaskHandler.Text = scp.BearerOrgName;
            //}
        }

        void btnProductSchedule_Click(object sender, EventArgs e)
        {
            VSelectGWBSDetail vss = new VSelectGWBSDetail(new MGWBSTree());
            vss.ShowDialog();
            IList list = vss.SelectGWBSDetail;
            if (list == null || list.Count == 0)
                return;


            List<GWBSDetail> listSelTaskDtl = new List<GWBSDetail>();//需要生成确认单的任务明细

            listSelTaskDtl = list.OfType<GWBSDetail>().ToList();

            //根据周进度计划的项目任务下的（未开具工程任务确认单）生产明细生成工程任务确认单

            //ObjectQuery oq = new ObjectQuery();
            //Disjunction dis = new Disjunction();
            //foreach (GWBSDetail dtl in list)
            //{
            //    //dis.Add(Expression.And(Expression.Eq("ProduceConfirmFlag", 1), Expression.Eq("Id", dtl.Id)));
            //    dis.Add(Expression.Eq("Id", dtl.Id));
            //}
            //oq.AddCriterion(dis);

            //listSelTaskDtl = model.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>().ToList();



            //if (listSelTaskDtl.Count > 0)//过滤已生成确认单的任务明细
            //{
            //    oq.Criterions.Clear();
            //    dis = new Disjunction();
            //    foreach (GWBSDetail dtl in listSelTaskDtl)
            //    {
            //        dis.Add(Expression.Eq("GWBSDetail.Id", dtl.Id));
            //    }
            //    oq.AddCriterion(Expression.Not(Expression.Eq("Master.DocState", DocumentState.Invalid)));
            //    IEnumerable<GWBSTaskConfirm> listConfirm = model.ObjectQuery(typeof(GWBSTaskConfirm), oq).OfType<GWBSTaskConfirm>();//有效的确认单


            //    if (listConfirm.Count() > 0)
            //    {
            //        for (int i = listSelTaskDtl.Count - 1; i > -1; i--)
            //        {
            //            GWBSDetail dtl = listSelTaskDtl[i];

            //            var queryDtl = from d in listConfirm
            //                           where d.GWBSDetail.Id == dtl.Id
            //                           select d;

            //            if (queryDtl.Count() > 0)
            //            {
            //                listSelTaskDtl.RemoveAt(i);
            //            }
            //        }
            //    }
            //}

            var queryTaskDtl = from d in listSelTaskDtl
                               orderby d.TheGWBS.Name ascending
                               select d;

            //dgDetail.Rows.Clear();
            foreach (GWBSDetail detail in queryTaskDtl)
            {
                if (HasAdded(detail))
                    continue;

                GWBSTaskConfirm taskConfirmDetail = new GWBSTaskConfirm();
                taskConfirmDetail.GWBSTree = detail.TheGWBS;
                taskConfirmDetail.GWBSTreeName = detail.TheGWBS.Name;
                taskConfirmDetail.GwbsSysCode = detail.TheGWBS.SysCode;

                taskConfirmDetail.GWBSDetail = detail;
                taskConfirmDetail.GWBSDetailName = detail.Name;

                taskConfirmDetail.CostItem = detail.TheCostItem;
                taskConfirmDetail.CostItemName = detail.TheCostItem.Name;

                taskConfirmDetail.ProjectTaskType = detail.ProjectTaskTypeCode;
                taskConfirmDetail.WorkAmountUnitId = detail.WorkAmountUnitGUID;
                taskConfirmDetail.WorkAmountUnitName = detail.WorkAmountUnitName;
                taskConfirmDetail.PlannedQuantity = detail.PlanWorkAmount;

                taskConfirmDetail.QuantityBeforeConfirm = detail.QuantityConfirmed;
                taskConfirmDetail.ProgressBeforeConfirm = detail.ProgressConfirmed;

                int rowIndex = dgDetail.Rows.Add();

                dgDetail[colGWBSTree.Name, rowIndex].Value = detail.TheGWBS.Name;
                dgDetail[colGWBSTree.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.TheGWBS.Name, detail.TheGWBS.SysCode);

                dgDetail[colGWBSDetail.Name, rowIndex].Tag = detail;
                dgDetail[colGWBSDetail.Name, rowIndex].Value = detail.Name;

                dgDetail[colPlanWorkAmount.Name, rowIndex].Value = detail.PlanWorkAmount;

                dgDetail[colProjectType.Name, rowIndex].Value = detail.ProjectTaskTypeCode;
                dgDetail[colUnit.Name, rowIndex].Value = detail.WorkAmountUnitName;

                dgDetail[colActualFinished.Name, rowIndex].Value = detail.QuantityConfirmed;
                dgDetail[colProgress.Name, rowIndex].Value = detail.ProgressConfirmed;
                dgDetail[colMaterialFeeSettlementFlag.Name,rowIndex].Value = Enum.GetName(typeof(EnumMaterialFeeSettlementFlag), EnumMaterialFeeSettlementFlag.不结算);

                dgDetail[colMaterialFeeSettlementFlag.Name, rowIndex].Value = taskConfirmDetail.MaterialFeeSettlementFlag.ToString();

                dgDetail.Rows[rowIndex].Tag = taskConfirmDetail;
            }
        }

        private bool HasAdded(GWBSDetail detail)
        {
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                GWBSDetail rowGWBSDetail = dr.Cells[colGWBSDetail.Name].Tag as GWBSDetail;
                if (rowGWBSDetail == null) return false;
                if (rowGWBSDetail.Id == detail.Id)
                {
                    return true;
                }
            }
            return false;
        }
        //右键删除
        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as GWBSTaskConfirm);
                }
            }
        }
        //右键复制
        void tsmiCopy_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null || dr.IsNewRow)
                return;

            int rowIndex = dr.Index + 1;
            dgDetail.Rows.Insert(rowIndex, 1);
            DataGridViewRow row = dgDetail.Rows[rowIndex];

            GWBSTaskConfirm detail = dgDetail.CurrentRow.Tag as GWBSTaskConfirm;

            GWBSTaskConfirm taskConfirmDetail = new GWBSTaskConfirm();

            taskConfirmDetail.ConfirmDescript = detail.ConfirmDescript;
            taskConfirmDetail.CostItem = detail.CostItem;
            taskConfirmDetail.CostItemName = detail.CostItemName;
            taskConfirmDetail.DailyCheckState = detail.DailyCheckState;
            taskConfirmDetail.Descript = detail.Descript;

            taskConfirmDetail.GWBSDetail = detail.GWBSDetail;
            taskConfirmDetail.GWBSDetailName = detail.GWBSDetailName;
            taskConfirmDetail.GwbsSysCode = detail.GwbsSysCode;
            taskConfirmDetail.GWBSTree = detail.GWBSTree;
            taskConfirmDetail.GWBSTreeName = detail.GWBSTreeName;

            taskConfirmDetail.ProjectTaskType = detail.ProjectTaskType;
            taskConfirmDetail.WorkAmountUnitId = detail.WorkAmountUnitId;
            taskConfirmDetail.WorkAmountUnitName = detail.WorkAmountUnitName;
            taskConfirmDetail.PlannedQuantity = detail.PlannedQuantity;

            for (int j = 0; j < dr.Cells.Count; j++)
            {
                if (dr.Cells[j].OwningColumn.Name != colSupplier.Name)
                {
                    row.Cells[j].Tag = dr.Cells[j].Tag;
                    row.Cells[j].Value = dr.Cells[j].Value;
                }
            }

            dgDetail.Rows[rowIndex].Tag = taskConfirmDetail;

            dgDetail.CurrentCell = row.Cells[1];
        }

        private void IfSubBalance()
        {
            ////model.ProductionManagementSrv.IfSubBalance(var.GWBSDetail.Id);
            //foreach (DataGridViewRow dr in dgDetail.Rows)
            //{
            //    if (dr.IsNewRow) continue;
            //    GWBSDetail _GWBSDetail = dr.Cells[colGWBSDetail.Name].Tag as GWBSDetail;
            //    bool ifBal = model.ProductionManagementSrv.IfSubBalance(_GWBSDetail.Id);
            //    if (ifBal == true)
            //    {
            //        dr.Cells[colMaterialFeeSettlementFlag.Name].ReadOnly = false;
            //        DataGridViewCellStyle cellStyle = dr.Cells[colMaterialFeeSettlementFlag.Name].Style;
            //        cellStyle.BackColor = Color.White;
            //        dr.Cells[colMaterialFeeSettlementFlag.Name].Style = cellStyle;
            //    }
            //    else
            //    {
            //        dr.Cells[colMaterialFeeSettlementFlag.Name].ReadOnly = true;
            //        DataGridViewCellStyle cellStyle = dr.Cells[colMaterialFeeSettlementFlag.Name].Style;
            //        cellStyle.BackColor = Color.FromName("Control");
            //        dr.Cells[colMaterialFeeSettlementFlag.Name].Style = cellStyle;
            //    }
            //}
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string id)
        {
            try
            {
                if (id == "空" || string.IsNullOrEmpty(id))
                    RefreshState(MainViewState.Initialize);
                else
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", id));
                    oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
                    curBillMaster = model.ProductionManagementSrv.GetGWBSTaskConfirmMasterByCode(oq);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    IfSubBalance();
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtProject };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colGWBSTree.Name, colGWBSDetail.Name, colSupplier.Name, colActualFinished.Name, colQuantityConfirmed.Name, colProjectType.Name, colProgress.Name, colUnit.Name, colTheTeamQuantityConfirmed.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();

                this.curBillMaster = new GWBSTaskConfirmMaster();
                curBillMaster.ConfirmHandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.ConfirmHandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                curBillMaster.DocState = DocumentState.Edit;
                //curBillMaster.CreateDate = ConstObject.LoginDate;
                //curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                //curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                //curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                //curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                //curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;    

                //制单人
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();

                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();
                curBillMaster = model.ProductionManagementSrv.GetGWBSTaskConfirmMasterById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能修改！");
            return false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                return SaveOrSubmitView(1);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    return SaveOrSubmitView(2);
                }
                catch (Exception e)
                {
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
            }
            return false;
        }

        /// <summary>
        /// 保存或提交
        /// </summary>
        /// <param name="action">1 保存；2 提交</param>
        /// <returns></returns>
        private bool SaveOrSubmitView(int action)
        {
            if (!ViewToModel())
                return false;
            string logType = "";
            if (string.IsNullOrEmpty(curBillMaster.Id) && action == 1)
            {
                logType = "新增保存";
            }
            else if (string.IsNullOrEmpty(curBillMaster.Id) && action == 2)
            {
                logType = "新增提交";
            }
            else if (!string.IsNullOrEmpty(curBillMaster.Id) && action == 1)
            {
                logType = "修改保存";
            }
            else if (!string.IsNullOrEmpty(curBillMaster.Id) && action == 2)
            {
                logType = "修改提交";
            }
            if (action == 2)
            {
                curBillMaster.DocState = DocumentState.InAudit;
            }
            curBillMaster = model.ProductionManagementSrv.SaveGWBSTaskConfirmMaster2(curBillMaster);
            txtCode.Text = curBillMaster.Code;
            this.ViewCaption = ViewName + "-" + txtCode.Text;
            //插入日志
            StaticMethod.InsertLogData(curBillMaster.Id, logType, curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "工程任务确认单", "", curBillMaster.ProjectName);
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.ProductionManagementSrv.GetGWBSTaskConfirmMasterById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ProductionManagementSrv.DeleteGWBSTaskConfirmMaster2(curBillMaster)) return false;
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "工程任务确认单", "", curBillMaster.ProjectName);
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        curBillMaster = model.ProductionManagementSrv.GetGWBSTaskConfirmMasterById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.ProductionManagementSrv.GetGWBSTaskConfirmMasterById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {

            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            this.dgDetail.Focus();

            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                object sumQuantity = dr.Cells[colSumQuantity.Name].Value;
                if (!StaticMethod.ValidDecimalCell(sumQuantity))
                {
                    MessageBox.Show("请输入正确的【本次累计工程量】");
                    dgDetail.CurrentCell = dr.Cells[colSumQuantity.Name];
                    return false;
                }

                object sumProgress = dr.Cells[colSumCompletedPercent.Name].Value;
                if (!StaticMethod.ValidDecimalCell(sumProgress))
                {
                    MessageBox.Show("请输入正确的【本次累计形象进度】");
                    dgDetail.CurrentCell = dr.Cells[colSumCompletedPercent.Name];
                    return false;
                }

                object taskHandler = dr.Cells[colSupplier.Name].Value;
                if (taskHandler == null || string.IsNullOrEmpty(taskHandler.ToString()))
                {
                    MessageBox.Show("【任务承担者】不能为空，请双击选择。");
                    dgDetail.CurrentCell = dr.Cells[colSupplier.Name];
                    return false;
                }
            }
            dgDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                //curBillMaster.SubContractProject = txtTaskHandler.Tag as SubContractProject;
                //curBillMaster.TaskHandleName = txtTaskHandler.Text;

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow)
                        break;

                    GWBSTaskConfirm curBillDtl = new GWBSTaskConfirm();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as GWBSTaskConfirm;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }

                    GWBSDetail detail = var.Cells[colGWBSDetail.Name].Tag as GWBSDetail;
                    if (curBillDtl.GWBSDetail == null)
                    {
                        curBillDtl.GWBSTree = detail.TheGWBS;
                        curBillDtl.GWBSDetail = detail;
                        curBillDtl.CostItem = detail.TheCostItem;
                        curBillDtl.CostItemName = detail.TheCostItem.Name;

                        curBillDtl.GwbsSysCode = detail.TheGWBS.SysCode;
                        curBillDtl.ProjectTaskType = detail.ProjectTaskTypeCode;
                        curBillDtl.WorkAmountUnitId = detail.WorkAmountUnitGUID;
                        curBillDtl.WorkAmountUnitName = detail.WorkAmountUnitName;
                        curBillDtl.PlannedQuantity = detail.PlanWorkAmount;
                    }


                    curBillDtl.GWBSTreeName = ClientUtil.ToString(var.Cells[colGWBSTree.Name].Value);
                    curBillDtl.GWBSDetailName = ClientUtil.ToString(var.Cells[colGWBSDetail.Name].Value);
                    curBillDtl.QuantityBeforeConfirm = ClientUtil.ToDecimal(var.Cells[colActualFinished.Name].Value);
                    curBillDtl.QuantiyAfterConfirm = ClientUtil.ToDecimal(var.Cells[colSumQuantity.Name].Value);
                    curBillDtl.ActualCompletedQuantity = ClientUtil.ToDecimal(var.Cells[colQuantityConfirmed.Name].Value);

                    curBillDtl.ProjectTaskType = ClientUtil.ToString(var.Cells[colProjectType.Name].Value);
                    curBillDtl.ProgressBeforeConfirm = ClientUtil.ToDecimal(var.Cells[colProgress.Name].Value);
                    curBillDtl.ProgressAfterConfirm = ClientUtil.ToDecimal(var.Cells[colSumCompletedPercent.Name].Value);

                    curBillDtl.TaskHandler = var.Cells[colSupplier.Name].Tag as SubContractProject;
                    curBillDtl.TaskHandlerName = ClientUtil.ToString(var.Cells[colSupplier.Name].Value);

                    curBillDtl.WorkAmountUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDifference.Name].Value);
                    curBillDtl.ConfirmDescript = var.Cells[colConfirmDescript.Name].Value + "";
                    curBillDtl.MaterialFeeSettlementFlag = VirtualMachine.Component.Util.EnumUtil<EnumMaterialFeeSettlementFlag>.FromDescription(var.Cells[colMaterialFeeSettlementFlag.Name].Value.ToString());
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                txtCode.Text = curBillMaster.Code;

                txtCreatePerson.Tag = curBillMaster.ConfirmHandlePerson;
                txtCreatePerson.Text = curBillMaster.ConfirmHandlePersonName;
                txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                txtProject.Text = curBillMaster.ProjectName;


                this.dgDetail.Rows.Clear();
                foreach (GWBSTaskConfirm var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    dgDetail.Rows[i].Cells[colGWBSTree.Name].Value = var.GWBSTreeName;
                    dgDetail.Rows[i].Cells[colGWBSTree.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), var.GWBSTreeName, var.GwbsSysCode);

                    dgDetail.Rows[i].Cells[colGWBSDetail.Name].Tag = var.GWBSDetail;
                    dgDetail.Rows[i].Cells[colGWBSDetail.Name].Value = var.GWBSDetailName;
                    dgDetail.Rows[i].Cells[colPlanWorkAmount.Name].Value = var.PlannedQuantity;
                    dgDetail.Rows[i].Cells[colActualFinished.Name].Value = var.QuantityBeforeConfirm;//
                    dgDetail.Rows[i].Cells[colSumQuantity.Name].Value = var.QuantiyAfterConfirm;
                    dgDetail.Rows[i].Cells[colQuantityConfirmed.Name].Value = var.ActualCompletedQuantity;//本次确认工程量
                    dgDetail.Rows[i].Cells[colProjectType.Name].Value = var.ProjectTaskType;
                    //dgDetail.Rows[i].Cells[colPlannedQuantity.Name].Value = var.PlannedQuantity;

                    dgDetail.Rows[i].Cells[colProgress.Name].Value = var.ProgressBeforeConfirm;
                    dgDetail.Rows[i].Cells[colSumCompletedPercent.Name].Value = var.ProgressAfterConfirm;

                    dgDetail.Rows[i].Cells[colSupplier.Name].Tag = var.TaskHandler;
                    dgDetail.Rows[i].Cells[colSupplier.Name].Value = var.TaskHandlerName;
                    dgDetail.Rows[i].Cells[colTheTeamQuantityConfirmed.Name].Value = model.ProductionManagementSrv.GetTheTeamQuantityConfirmed(projectInfo, var.GWBSDetail, var.TaskHandler).ToString();

                    dgDetail.Rows[i].Cells[colUnit.Name].Value = var.WorkAmountUnitName;

                    dgDetail.Rows[i].Cells[colDifference.Name].Value = var.Descript;
                    dgDetail.Rows[i].Cells[colConfirmDescript.Name].Value = var.ConfirmDescript;
                    dgDetail.Rows[i].Cells[colMaterialFeeSettlementFlag.Name].Value = Enum.GetName(typeof(EnumMaterialFeeSettlementFlag), var.MaterialFeeSettlementFlag);

                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("显示出错了。\n" + ExceptionUtil.ExceptionMessage(e));
            }
            return true;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            object cellValue = null;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSumQuantity.Name)
            {
                cellValue = e.FormattedValue;
                if (cellValue == null || cellValue.ToString().Trim() == "")
                    return;

                if (!StaticMethod.ValidDecimalCell(cellValue))
                {
                    MessageBox.Show("请输入正确的【本次累计工程量】。");
                    e.Cancel = true;
                    return;
                }
                decimal quantityBefore = ClientUtil.ToDecimal(dgDetail[colActualFinished.Name, e.RowIndex].Value);
                if (decimal.Parse(cellValue.ToString()) <= quantityBefore)
                {
                    MessageBox.Show("【本次累计工程量】要大于【累计确认工程量】。");
                    e.Cancel = true;
                }
            }
            else if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSumCompletedPercent.Name)
            {
                cellValue = e.FormattedValue;
                if (cellValue == null || cellValue.ToString().Trim() == "")
                    return;

                if (!StaticMethod.ValidDecimalCell(cellValue))
                {
                    MessageBox.Show("请输入正确的【本次累积形象进度】。");
                    e.Cancel = true;
                    return;
                }
                decimal progressBefore = ClientUtil.ToDecimal(dgDetail[colProgress.Name, e.RowIndex].Value);
                if (decimal.Parse(cellValue.ToString()) <= progressBefore)
                {
                    MessageBox.Show("【本次累积形象进度】要大于【累积形象进度】。");
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            object cellValue = null;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSumQuantity.Name)
            {
                cellValue = dgDetail[e.ColumnIndex, e.RowIndex].Value;
                if (cellValue == null || cellValue.ToString().Trim() == "")
                    return;

                decimal quantityBefore = ClientUtil.ToDecimal(dgDetail[colActualFinished.Name, e.RowIndex].Value);
                dgDetail[colQuantityConfirmed.Name, e.RowIndex].Value = decimal.Parse(cellValue.ToString()) - quantityBefore;
            }
        }

        #region 打印处理
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            //if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
            //FillFlex(curBillMaster);
            //flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }

        public override bool Print()
        {
            //if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
            //FillFlex(curBillMaster);
            //flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            //if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
            //FillFlex(curBillMaster);
            //flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(WeekScheduleMaster billMaster)
        {
            //int detailStartRowNumber = 7;//7为模板中的行号
            //int detailCount = billMaster.Details.Count;

            ////插入明细行
            //flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            ////设置单元格的边框，对齐方式
            //FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            //range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //range.Mask = FlexCell.MaskEnum.Digital;

            ////主表数据

            //flexGrid1.Cell(2, 1).Text = "使用单位：";
            //flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
            //flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
            //flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
            //flexGrid1.Cell(4, 5).Text = "";//合同名称
            //flexGrid1.Cell(4, 7).Text = "";//材料分类
            //flexGrid1.Cell(5, 2).Text = "";//租赁单位
            //flexGrid1.Cell(5, 2).WrapText = true;
            //flexGrid1.Cell(5, 5).Text = "";//承租单位
            //flexGrid1.Row(5).AutoFit();
            //flexGrid1.Cell(5, 7).Text = billMaster.RealOperationDate.ToShortDateString();//签订日期

            //FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            //pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
            //pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

            //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
            //pageSetup.PaperSize = paperSize;

            ////填写明细数据
            //for (int i = 0; i < detailCount; i++)
            //{
            //    MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
            //    //物资名称
            //    flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
            //    flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

            //    //规格型号
            //    flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
            //    flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

            //    //数量
            //    flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

            //    //日租金
            //    flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

            //    //金额
            //    flexGrid1.Cell(detailStartRowNumber + i, 6).Text = "";

            //    //备注
            //    flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
            //    flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            //}
        }
        #endregion
    }
}