using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using VirtualMachine.Core;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount
{
    public partial class VSpecialAccount : TMasterDetailView
    {
        private MSpecialAccount model = new MSpecialAccount();
        private SpeCostSettlementMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public SpeCostSettlementMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VSpecialAccount()
        {
            InitializeComponent();
            InitEvent();
            
        }

        private void InitEvent()
        {
            this.btnSearch.Click +=new EventHandler(btnSearch_Click);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            //this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            //this.dgDetail.CellEnter += new DataGridViewCellEventHandler(dgDetail_CellEnter);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Enabled = true)
            {
                VContractExcuteSelector vmros = new VContractExcuteSelector();
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                SubContractProject engineerMaster = list[0] as SubContractProject;
                txtDepart.Text = engineerMaster.BearerOrgName;
                txtDepart.Tag = engineerMaster;
            }
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as SpeCostSettlementDetail);
                }
            }
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Tag != null)
            {
                curBillMaster.Details.Remove(e.Row.Tag as BaseDetail);
            }
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string Id)
        {
            try
            {
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.SpecialCostSrv.GetSpecialAccountById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = ConstObject.LoginPersonInfo;
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
            object[] os = new object[] { txtHandlePerson, txtProject, txtCode, this.dtpDateBegin };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colManageMoney.Name, colAccountMoney.Name };
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
                movedDtlList = new ArrayList();

                this.curBillMaster = new SpeCostSettlementMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                curBillMaster.DocState = DocumentState.Edit;//状态
                
                //制单日期
                dtpDateBegin.Text = ConstObject.LoginDate.ToShortDateString();
                dtpAccountDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
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
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.SpecialCostSrv.GetSpecialAccountById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                if (curBillMaster.DocState == DocumentState.InExecute)
                {
                    MessageBox.Show("已经提交，不能再次提交！");
                    return false;
                }
                    if (!ViewToModel()) return false;
                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;
                    curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;
                    curBillMaster.AuditDate = ConstObject.LoginDate;
                    curBillMaster.AuditYear = ConstObject.LoginDate.Year;
                    curBillMaster.AuditMonth = ConstObject.LoginDate.Month;
                    curBillMaster = model.SpecialCostSrv.SaveSpecialAccount(curBillMaster);
                    //插入日志
                    txtCode.Text = curBillMaster.Code;
                    //更新Caption
                    this.ViewCaption = ViewName + "-" + txtCode.Text;
                    return true;
                
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
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
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.SaveSpecialAccount(curBillMaster);

                ////插入日志
                //txtCode.Text = curBillMaster.Code;
                ////更新Caption
                //LogData log = new LogData();
                //log.BillId = curBillMaster.Id;
                //log.BillType = "废旧物资申请单";
                //log.Code = curBillMaster.Code;
                //log.Descript = "";
                //log.OperPerson = ConstObject.LoginPersonInfo.Name;
                //log.ProjectName = curBillMaster.ProjectName;
                //if (flag)
                //{
                //    log.OperType = "新增";
                //}
                //else
                //{
                //    log.OperType = "修改";
                //}
                //StaticMethod.InsertLogData(log);
                txtCode.Text = curBillMaster.Code;
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                MessageBox.Show("保存成功！");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                 WasteMatApplyDetail ad = new WasteMatApplyDetail();
                 if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                 {
                     if (ad.RefQuantity > 0)
                     {
                         MessageBox.Show("此信息被引用，删除失败！");
                         return false;
                     }
                     else
                     {
                         curBillMaster = model.SpecialCostSrv.GetSpecialAccountById(curBillMaster.Id);
                         if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                         {
                             if (!model.SpecialCostSrv.DeleteByDao(curBillMaster)) return false;
                             LogData log = new LogData();
                             log.BillId = curBillMaster.Id;
                             log.BillType = "专项费用结算单";
                             log.Code = curBillMaster.Code;
                             log.OperType = "删除";
                             log.Descript = "";
                             log.OperPerson = ConstObject.LoginPersonInfo.Name;
                             log.ProjectName = curBillMaster.ProjectName;
                             StaticMethod.InsertLogData(log);
                             ClearView();
                             MessageBox.Show("删除成功！");
                             return true;
                         }
                         MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                         return false;
                     }
                 }
                 string message = "此单状态为【{0}】，不能删除！";
                 message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
                 MessageBox.Show(message);
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
                        curBillMaster = model.SpecialCostSrv.GetSpecialAccountById(curBillMaster.Id);
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
                curBillMaster = model.SpecialCostSrv.GetSpecialAccountById(curBillMaster.Id);
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
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }

            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colAccountMoney.Name].Value == null)
                {
                    MessageBox.Show("结算金额不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colAccountMoney.Name];
                    return false;
                }

                if (dr.Cells[colProject.Name].Value == null)
                {
                    MessageBox.Show("工程任务不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colProject.Name];
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
                curBillMaster.CreateDate = ConstObject.LoginDate;//业务时间
                curBillMaster.HandlePerson = this.txtHandlePerson.Tag as PersonInfo;
                curBillMaster.HandlePersonName = ClientUtil.ToString(this.txtHandlePerson.Text);
                curBillMaster.SubcontractUnitName = txtDepart.Text;
                curBillMaster.SubcontractProjectId = txtDepart.Tag as SubContractProject;
                curBillMaster.SettlementDate = dtpAccountDate.Value;

                string strUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.SpecialCostSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    SpeCostSettlementDetail curBillDtl = new SpeCostSettlementDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as SpeCostSettlementDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.PriceUom = Unit;
                    curBillDtl.PriceUnitName = strUnit;
                    curBillDtl.EngTask = var.Cells[colProject.Name].Tag as GWBSTree;
                    curBillDtl.EngTaskName = ClientUtil.ToString(var.Cells[colProject.Name].Value);
                    curBillDtl.EngTaskSyscode = var.Cells[colAccountMoney.Name].Tag as string;
                    curBillDtl.SettlementMoney = ClientUtil.ToDecimal(var.Cells[colAccountMoney.Name].Value)*10000;
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[colOrderMoney.Name].Value) * 10000;
                    curBillDtl.ManageAcer = ClientUtil.ToDecimal(var.Cells[colManageRace.Name].Value) / 100;
                    curBillDtl.ManageMoney = ClientUtil.ToDecimal(var.Cells[colManageMoney.Name].Value) * 10000;
                    curBillDtl.ElectAcer = ClientUtil.ToDecimal(var.Cells[colElectRace.Name].Value) / 100;
                    curBillDtl.ElectMoney = ClientUtil.ToDecimal(var.Cells[colElectMoney.Name].Value) * 10000;
                    curBillDtl.OtherAccruals = ClientUtil.ToDecimal(var.Cells[colOtherMoney.Name].Value) * 10000;
                    curBillDtl.PayMentFees = ClientUtil.ToDecimal(var.Cells[colPayMentFees.Name].Value) * 10000;
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

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

  
        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProject.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        TreeNode root = frm.SelectResult[0];

                        GWBSTree task = root.Tag as GWBSTree;
                        if (task != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colProject.Name].Value = task.Name;
                            this.dgDetail.CurrentRow.Cells[colProject.Name].Tag = task;
                            this.dgDetail.CurrentRow.Cells[this.colAccountMoney.Name].Tag = task.SysCode;
                            this.txtCode.Focus();
                        }
                    }
                }
                
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                dtpDateBegin.Value = curBillMaster.RealOperationDate;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Value = curBillMaster.HandlePersonName;
                this.txtProject.Text = curBillMaster.ProjectName;
                this.txtProject.Tag = curBillMaster.ProjectId;
                this.dtpAccountDate.Value = curBillMaster.SettlementDate;
                this.txtDepart.Text = curBillMaster.SubcontractUnitName;
                this.txtDepart.Tag = curBillMaster.SubcontractProjectId;
                this.dgDetail.Rows.Clear();
                foreach (SpeCostSettlementDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colAccountMoney.Name,i].Value = var.SettlementMoney/10000;//结算金额
                    this.dgDetail[colAccountMoney.Name, i].Tag = var.EngTaskSyscode;
                    this.dgDetail[colProject.Name,i].Value = var.EngTaskName;//工程任务名称
                    this.dgDetail[colProject.Name, i].Tag = var.EngTask;
                    this.dgDetail[colOrderMoney.Name,i].Value = var.Money/10000;
                    this.dgDetail[colManageMoney.Name,i].Value = var.ManageMoney/10000;
                    this.dgDetail[colManageRace.Name, i].Value = var.ManageAcer * 100;
                    this.dgDetail[colElectRace.Name,i].Value = var.ElectAcer * 100;
                    this.dgDetail[colElectMoney.Name, i].Value = var.ElectMoney / 10000;
                    this.dgDetail[colPayMentFees.Name, i].Value = var.PayMentFees / 10000;
                    this.dgDetail[colOtherMoney.Name, i].Value = var.OtherAccruals / 10000;
                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colOrderMoney.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value = "";
                    }
                }
                dgDetail.Rows[e.RowIndex].Cells[colManageMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colManageRace.Name].Value) / 100);
                dgDetail.Rows[e.RowIndex].Cells[colElectMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colElectRace.Name].Value) / 100));
                dgDetail.Rows[e.RowIndex].Cells[colAccountMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colManageMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colElectMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value));
            }
            if (colName == colManageRace.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colManageRace.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colManageRace.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail.Rows[e.RowIndex].Cells[colManageRace.Name].Value = "";
                    }
                }
                dgDetail.Rows[e.RowIndex].Cells[colManageMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colManageRace.Name].Value) / 100));
                dgDetail.Rows[e.RowIndex].Cells[colAccountMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colManageMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colElectMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value));
            }
            if (colName == colElectRace.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colElectRace.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colElectRace.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail.Rows[e.RowIndex].Cells[colElectRace.Name].Value = "";
                    }
                }
                dgDetail.Rows[e.RowIndex].Cells[colElectMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colElectRace.Name].Value) / 100));
                dgDetail.Rows[e.RowIndex].Cells[colAccountMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colManageMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colElectMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value));
            }
            if (colName == colPayMentFees.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value = "";
                    }
                }
                dgDetail.Rows[e.RowIndex].Cells[colAccountMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colManageMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colElectMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value));
            }
            if (colName == colOtherMoney.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value = "";
                    }
                }
                dgDetail.Rows[e.RowIndex].Cells[colAccountMoney.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOrderMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colManageMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colElectMoney.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colPayMentFees.Name].Value) - ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOtherMoney.Name].Value));
            }
        }

        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"专项费用结算明细.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"专项费用结算明细.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
        //    curBillMaster = model.SaveSpecialAccount(curBillMaster);
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"专项费用结算明细.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("专项费用结算明细【" + curBillMaster.Code + "】", false, false, true);
        //    return true;
        //}

        //private bool LoadTempleteFile(string modelName)
        //{
        //    ExploreFile eFile = new ExploreFile();
        //    string path = eFile.Path;
        //    if (eFile.IfExistFileInServer(modelName))
        //    {
        //        eFile.CreateTempleteFileFromServer(modelName);
        //        //载入格式和数据
        //        flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
        //    }
        //    else
        //    {
        //        MessageBox.Show("未找到模板格式文件【" + modelName + "】");
        //        return false;
        //    }
        //    return true;
        //}

        //private void FillFlex(SpeCostSettlementMaster billMaster)
        //{
        //    int detailStartRowNumber = 5;//5为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;
        //    CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
        //    //主表数据
        //    flexGrid1.Cell(2, 2).Text = billMaster.SubcontractUnitName;
        //    flexGrid1.Cell(2, 2).WrapText = true;
        //    flexGrid1.Cell(2, 6).Text = billMaster.RealOperationDate.ToShortDateString();
        //    flexGrid1.Cell(2, 6).WrapText = true;
        //    flexGrid1.Cell(2, 8).Text = billMaster.SettlementDate.ToShortDateString();
        //    flexGrid1.Cell(2, 8).WrapText = true;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        SpeCostSettlementDetail detail = (SpeCostSettlementDetail)billMaster.Details.ElementAt(i);
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = ClientUtil.ToString(i + 1);
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
        //        //工程任务
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.EngTaskName;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
        //        //合同总收入
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = ClientUtil.ToString(detail.SpeCostMngId.ContractTotalIncome);
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
        //        //管理费率
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.SpeCostMngId.ContractProfit);
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
        //        //计划成本
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.SpeCostMngId.ContractTotalIPay);
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
        //        //累计实际支出
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.SpeCostMngId.RealIncome);
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
        //        //结算金额
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.SettlementMoney);
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.Descript);
        //        flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;

        //        flexGrid1.Row(detailStartRowNumber + i).AutoFit();
        //    }
        //    flexGrid1.Cell(6 + detailCount, 6).Text = billMaster.CreatePersonName;

        //}

        public override bool Preview()
        {
            VSpecialAccountReport vs = new VSpecialAccountReport(curBillMaster);
            vs.ShowDialog();
            return true;
        }
    }
}
