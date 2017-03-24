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
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
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
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng
{
    public partial class VExpensesSettleMng : TMasterDetailView
    {
        private MExpensesSettleMng model = new MExpensesSettleMng();
        private ExpensesSettleMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空

        /// <summary>
        /// 当前单据
        /// </summary>
        public ExpensesSettleMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        CurrentProjectInfo projectInfo;
        public VExpensesSettleMng()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            //this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //右键删除菜单
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtAccountRootNode.Text = task.Name;
                    txtAccountRootNode.Tag = task;
                }
            }
        }


        void btnExcel_Click(object sender, EventArgs e)
        {
            if (btnExcel.Enabled)
            {
                string strName = "管理资源";
                Application.Resource.MaterialResource.Domain.Material theMaterial = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strName));
                IList lst = model.ExpensesSettleSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.Material), oq);
                if (lst != null && lst.Count > 0)
                {
                    theMaterial = lst[0] as Application.Resource.MaterialResource.Domain.Material;
                }

                string strUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oqUnit = new ObjectQuery();
                oqUnit.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.ExpensesSettleSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oqUnit);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }

                GWBSTree theTree = null;
                if (this.txtAccountRootNode.Text != "")
                {
                    theTree = txtAccountRootNode.Tag as GWBSTree;
                }
                else
                {
                    MessageBox.Show("请选择核算节点！");
                    return;
                }


                VImportExpensesSettle oVImportSupplyOrder = new VImportExpensesSettle();
                oVImportSupplyOrder.ShowDialog();
                this.dgDetail.Rows.Clear();
                curBillMaster.Details.Clear();
                Hashtable htExpenses = oVImportSupplyOrder.htResult;
                ExpensesSettleDetail expenseSettleDetail = null;
                string sName = string.Empty;
                decimal sumMoney = 0;
                if (htExpenses != null && htExpenses.Count > 0)
                {
                    foreach (System.Collections.DictionaryEntry obj in htExpenses)
                    {
                        int i = this.dgDetail.Rows.Add();

                        expenseSettleDetail = obj.Key as ExpensesSettleDetail;

                        if (theTree != null)
                        {
                            this.dgDetail[colProjectTask.Name, i].Tag = theTree;
                            this.dgDetail[colProjectTask.Name, i].Value = theTree.Name;
                        }
                        if (theMaterial != null)
                        {
                            this.dgDetail[colMaterialType.Name, i].Tag = theMaterial;
                            this.dgDetail[colMaterialType.Name, i].Value = theMaterial.Name;
                        }
                        this.dgDetail[colAccountSubject.Name, i].Tag = expenseSettleDetail.AccountCostSubject;
                        this.dgDetail[colAccountSubject.Name, i].Value = expenseSettleDetail.AccountCostName;
                        this.dgDetail[colPriceUnit.Name, i].Value = Unit.Name;
                        this.dgDetail[colPriceUnit.Name, i].Tag = Unit;
                        this.dgDetail[colSumMoney.Name, i].Value = expenseSettleDetail.Money;
                        sumMoney += expenseSettleDetail.Money;

                    }
                    this.txtSumMoney.Text = ClientUtil.ToString(sumMoney.ToString("#,###.####"));
                }
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
                    curBillMaster.Details.Remove(dr.Tag as DemandMasterPlanDetail);
                }
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
                    curBillMaster = model.ExpensesSettleSrv.GetExpensesSettleById(Id);
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
                    btnExcel.Enabled = true;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    btnExcel.Enabled = false;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtProject, txtAccountRootNode };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colPriceUnit.Name, colAccountSubject.Name, colMaterialType.Name, colProjectTask.Name };
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

                this.curBillMaster = new ExpensesSettleMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;

                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单年月
                txtCreateMonth.Text = ClientUtil.ToString(ConstObject.LoginDate.Month);
                txtCreateYear.Text = ClientUtil.ToString(ConstObject.LoginDate.Year);
                //归属项目
                projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                //txtContractNo.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ViewToModel())
                return false;

            LogData log = new LogData();
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                if (optrType == 2)
                {
                    log.OperType = "新增提交";
                }
                else
                {
                    log.OperType = "新增保存";
                }

            }
            else
            {
                if (optrType == 2)
                {
                    log.OperType = "修改提交";
                }
                else
                {
                    log.OperType = "修改保存";
                }
            }
            if (optrType == 2)
            {
                curBillMaster.DocState = DocumentState.InAudit;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            }
            curBillMaster = model.ExpensesSettleSrv.SaveExpensesSettle(curBillMaster);
            this.txtCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            log.BillType = "费用结算单维护";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                curBillMaster = model.ExpensesSettleSrv.GetExpensesSettleById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
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
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (SaveOrSubmitBill(1) == false) return false;
                    MessageBox.Show("保存成功！");
                    return true;
                }
                else
                {
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
                    return false;
                }
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
            try
            {
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (SaveOrSubmitBill(2) == false) return false;
                    MessageBox.Show("提交成功！");
                    return true;
                }
                else
                {
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能提交！");
                    return false;
                }
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
                curBillMaster = model.ExpensesSettleSrv.GetExpensesSettleById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ExpensesSettleSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "费用结算单维护";
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
                        curBillMaster = model.ExpensesSettleSrv.GetExpensesSettleById(curBillMaster.Id);
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
                curBillMaster = model.ExpensesSettleSrv.GetExpensesSettleById(curBillMaster.Id);
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
            //dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
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
                if (dr.Cells[colProjectTask.Name].Value != null)
                {
                    if (dr.Cells[colProjectTask.Name].Tag == null)
                    {
                        MessageBox.Show("未查到工程任务！");
                        dgDetail.CurrentCell = dr.Cells[colProjectTask.Name];
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("工程任务不能为空！");
                    return false;
                }
                if (dr.Cells[colAccountSubject.Name].Value != null)
                {
                    if (dr.Cells[colAccountSubject.Name].Tag == null)
                    {
                        MessageBox.Show("未查到用工科目！");
                        dgDetail.CurrentCell = dr.Cells[colAccountSubject.Name];
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("用工科目不能为空！");
                    return false;
                }
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                txtCode.Focus();
                curBillMaster.CreateDate = ClientUtil.ToDateTime(this.dtpDateBegin.Text);
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                curBillMaster.CreateYear = ClientUtil.ToInt(this.txtCreateYear.Text);
                curBillMaster.CreateMonth = ClientUtil.ToInt(this.txtCreateMonth.Text);
                curBillMaster.SumMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    ExpensesSettleDetail curBillDtl = new ExpensesSettleDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as ExpensesSettleDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.AccountCostName = ClientUtil.ToString(var.Cells[colAccountSubject.Name].Value);//核算科目名称
                    curBillDtl.AccountCostSubject = var.Cells[colAccountSubject.Name].Tag as CostAccountSubject;//核算科目
                    curBillDtl.AccountCostSysCode = (var.Cells[colAccountSubject.Name].Tag as CostAccountSubject).SysCode;//核算科目层次码
                    //curBillDtl.CostName = ClientUtil.ToString(var.Cells[colCoseName.Name].Value);//费用名称
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[colSumMoney.Name].Value);//费用金额
                    curBillDtl.PriceUnit = var.Cells[colPriceUnit.Name].Tag as StandardUnit;//价格计量单位
                    curBillDtl.PriceUnitName = ClientUtil.ToString(var.Cells[colPriceUnit.Name].Value);//价格计量单位名称
                    curBillDtl.ProjectTask = var.Cells[colProjectTask.Name].Tag as GWBSTree;//工程任务
                    curBillDtl.ProjectTaskName = ClientUtil.ToString(var.Cells[colProjectTask.Name].Value);//工程任务名称
                    curBillDtl.ProjectTaskSysCode = (var.Cells[colProjectTask.Name].Tag as GWBSTree).SysCode;//工程任务层次码
                    Application.Resource.MaterialResource.Domain.Material theMaterial = var.Cells[colMaterialType.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    //theMaterial = model.ExpensesSettleSrv.Dao.Get(typeof(Application.Resource.MaterialResource.Domain.Material), theMaterial.Id) as Application.Resource.MaterialResource.Domain.Material;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", theMaterial.Id));
                    IList lists = model.ExpensesSettleSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.Material), oq);
                    if (lists != null && lists.Count > 0)
                    {
                        theMaterial = lists[0] as Application.Resource.MaterialResource.Domain.Material;
                    }
                    curBillDtl.MaterialResource = theMaterial;
                    curBillDtl.MaterialName = theMaterial.Name;
                    curBillDtl.MaterialCode = theMaterial.Code;
                    curBillDtl.MaterialSpec = theMaterial.Specification;
                    curBillDtl.MaterialStuff = theMaterial.Stuff;
                    curBillDtl.MaterialSysCode = theMaterial.TheSyscode;
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
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProjectTask.Name))
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
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colProjectTask.Name, i].Value = task.Name;
                            this.dgDetail[colProjectTask.Name, i].Tag = task;
                            i++;
                        }
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colPriceUnit.Name))
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Tag = su;
                        this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value = su.Name;
                        this.txtCode.Focus();
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colAccountSubject.Name))
                {
                    //双击用工科目
                    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                    frm.ShowDialog();
                    CostAccountSubject cost = frm.SelectAccountSubject;
                    if (cost != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colAccountSubject.Name].Tag = cost;
                        this.dgDetail.CurrentRow.Cells[colAccountSubject.Name].Value = cost.Name;
                    }
                    this.txtCode.Focus();
                }

            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                dtpDateBegin.Value = curBillMaster.CreateDate;
                txtRemark.Text = curBillMaster.Descript;
                txtCreateMonth.Text = ClientUtil.ToString(curBillMaster.CreateMonth);
                txtCreateYear.Text = ClientUtil.ToString(curBillMaster.CreateYear);
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtProject.Text = curBillMaster.ProjectName.ToString();
                this.dgDetail.Rows.Clear();
                decimal sumMoney = 0;
                foreach (ExpensesSettleDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colAccountSubject.Name, i].Tag = var.AccountCostSubject;
                    this.dgDetail[colAccountSubject.Name, i].Value = var.AccountCostName;
                    this.dgDetail[colMaterialType.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialType.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colPriceUnit.Name, i].Value = var.PriceUnitName;
                    this.dgDetail[colPriceUnit.Name, i].Tag = var.PriceUnit;
                    this.dgDetail[colProjectTask.Name, i].Value = var.ProjectTaskName;
                    this.dgDetail[colProjectTask.Name, i].Tag = var.ProjectTask;
                    this.dgDetail[colSumMoney.Name, i].Value = var.Money;
                    object quantity = var.Money;
                    if (quantity != null)
                    {
                        sumMoney += decimal.Parse(quantity.ToString());
                    }
                    this.dgDetail.Rows[i].Tag = var;
                }
                this.txtSumMoney.Text = sumMoney.ToString();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("物资需求总计划【" + curBillMaster.Code + "】", false, false, true);
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

        //private void FillFlex(DemandMasterPlanMaster billMaster)
        //{
        //    int detailStartRowNumber = 5;//5为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;

        //    //主表数据
        //    flexGrid1.Cell(3, 2).Text = billMaster.Code;
        //    flexGrid1.Cell(3, 4).Text = billMaster.MaterialCategoryName;
        //    flexGrid1.Cell(3, 6).Text = billMaster.RealOperationDate.ToShortDateString();
        //    flexGrid1.Cell(3, 7).Text = "编制依据：" + billMaster.Compilation;

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    decimal sumQuantity = 0;
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        DemandMasterPlanDetail detail = (DemandMasterPlanDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;

        //        //计量单位
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;

        //        //需用计划
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
        //        sumQuantity += detail.Quantity;

        //        //质量标准
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.QualityStandard);

        //        //生产厂家
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Manufacturer);

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.Descript);
        //        if (i == detailCount - 1)
        //        {
        //            flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
        //        }
        //    }
        //    flexGrid1.Cell(6 + detailCount, 2).Text = billMaster.ProjectName;
        //    flexGrid1.Cell(6 + detailCount, 5).Text = billMaster.CreateDate.ToShortDateString();
        //    flexGrid1.Cell(6 + detailCount, 7).Text = billMaster.CreatePersonName;
        //}
    }
}
