using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Resource.FinancialResource.Service;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial;
using VirtualMachine.Patterns.DataDictionary.Domain;
using Application.Resource.FinancialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng.WeightBill;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockInMng.StockInUI
{
    public partial class VStockIn : TMasterDetailView
    {
        private MStockMng model = new MStockMng();
        private StockIn curBillMaster;
        private StockInBalMaster curBillBalMaster;//安装的验收单
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空

        private EnumStockExecType excuteType;
        /// <summary>
        /// 用来区分专业
        /// </summary>
        public EnumStockExecType ExcuteType
        {
            get { return excuteType; }
            set { excuteType = value; }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public StockIn CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VStockIn()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            //专业分类    
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
            dgDetail.ContextMenuStrip = cmsDg;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
            //this.txtMaterialCategory.Enabled = false;
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            btnForward.Click += new EventHandler(btnForward_Click);
            this.btnSetCal.Click += new EventHandler(btnSetCal_Click);
            this.btnSetQT.Click += new EventHandler(btnSetQT_Click);
            txtMaterialCategory.Leave += new EventHandler(txtMaterialCategory_Leave);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);


            //this.dtpDateBegin.ValueChanged += new EventHandler(dtpDateBegin_ValueChanged);
        }

        void btnSetCal_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (ClientUtil.ToString(dgDetail[this.colCalculate.Name, 0].Value) != "")
                {
                    var.Cells[colCalculate.Name].Value = dgDetail[colCalculate.Name, 0].Value;
                }
            }
        }

        void btnSetQT_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (ClientUtil.ToString(dgDetail[colAppearanceQuality.Name, 0].Value) != "")
                {
                    var.Cells[colAppearanceQuality.Name].Value = dgDetail[colAppearanceQuality.Name, 0].Value;
                }
            }
        }

        void cboProfessionCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pc = cboProfessionCat.SelectedItem as string;
            if (pc != null)
            {
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    dr.Cells[colProfessionalCategory.Name].Value = pc;
                }
            }
        }

        void txtMaterialCategory_Leave(object sender, EventArgs e)
        {
            IList result = txtMaterialCategory.Result;
            if (result != null && result.Count > 0)
            {
                MaterialCategory mc = result[0] as MaterialCategory;
                if (mc.Level == 2) return;
                MaterialCategory firstMc = FindFirstCategory(mc);
                result.Clear();
                result.Add(firstMc);
                txtMaterialCategory.Text = firstMc.Name;
            }
        }

        /// <summary>
        /// 查找一级分类
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        private MaterialCategory FindFirstCategory(MaterialCategory mc)
        {
            if (mc.Level == 2) return mc;
            return FindFirstCategory((MaterialCategory)mc.ParentNode);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            InitControls();
        }

        private void ReadOnlyCat(bool enabled)
        {
            txtMaterialCategory.ReadOnly = enabled;
            cboProfessionCat.ReadOnly = enabled;
        }

        /// <summary>
        /// 根据土建、安装 界面不同显示
        /// </summary>
        private void InitControls()
        {
            if (ExcuteType == EnumStockExecType.土建)
            {
                lblDate.Text = "取价日期:";
                colConfirmPrice.Visible = false;
                colConfirmMoney.Visible = false;
                colProfessionalCategory.Visible = false;
                lblSumConfirmMoney.Visible = false;
                txtSumConfirmMoney.Visible = false;

                lblCat.Text = "物资分类";
                txtMaterialCategory.Visible = true;
                cboProfessionCat.Visible = false;
            }
            else if (ExcuteType == EnumStockExecType.安装)
            {
                lblDate.Text = "收料日期:";
                colConfirmPrice.Visible = true;
                colConfirmMoney.Visible = true;
                colProfessionalCategory.Visible = false;
                lblSumConfirmMoney.Visible = true;
                txtSumConfirmMoney.Visible = true;

                lblCat.Text = "专业分类";
                txtMaterialCategory.Visible = false;
                cboProfessionCat.Visible = true;
                this.colCalculate.Visible = false;
            }
        }

        void btnForward_Click(object sender, EventArgs e)
        {
            if (txtSupply.Text == "" || txtSupply.Result.Count == 0)
            {
                MessageBox.Show("请先选择供应商。");
                return;
            }
            SupplierRelationInfo supplierRelationInfo = txtSupply.Result[0] as SupplierRelationInfo;
            //VDailyPlanSelector vdps = new VDailyPlanSelector();
            VDailyPlanSelector vdps = new VDailyPlanSelector(this.excuteType);
            vdps.SupplierRelationInfo = supplierRelationInfo;
            vdps.ShowDialog();
            IList result = vdps.Result;
            if (result == null || result.Count == 0) return;
            DailyPlanMaster forwardMaster = result[0] as DailyPlanMaster;
            SupplyOrderMaster orderMaster = result[1] as SupplyOrderMaster;

            txtForwardCode.Text = forwardMaster.Code;

            curBillMaster.SupplyOrderCode = orderMaster.OldContractNum;
            curBillMaster.AssociatedOrder = orderMaster.Id;

            curBillMaster.ForwardBillId = forwardMaster.Id;
            curBillMaster.ForwardBillCode = forwardMaster.Code;
            curBillMaster.MaterialCategory = forwardMaster.MaterialCategory;
            curBillMaster.MatCatName = forwardMaster.MaterialCategoryName;

            txtMaterialCategory.Result.Clear();
            this.txtMaterialCategory.Value = curBillMaster.MatCatName;
            txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);

            if (this.excuteType == EnumStockExecType.安装)
            {
                //this.txtMaterialCategory.Value = forwardMaster.SpecialType;
                this.cboProfessionCat.Text = forwardMaster.SpecialType;
            }
            //处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                StockInDtl dtl = dr.Tag as StockInDtl;
                if (dtl != null)
                {
                    if (CurBillMaster != null)
                    {
                        CurBillMaster.Details.Remove(dtl);
                        if (dtl.Id != null)
                        {
                            movedDtlList.Add(dtl);
                        }
                    }
                }
            }
            this.dgDetail.Rows.Clear();
            foreach (DailyPlanDetail var in forwardMaster.Details)
            {
                if (!var.IsSelect) continue;
                int i = this.dgDetail.Rows.Add();
                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colMaterialStuff.Name, i].Value = var.MaterialStuff;
                this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;
                this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                this.dgDetail[colRemainQty.Name, i].Value = var.Quantity - var.RefQuantity;
                this.dgDetail[colCanUseQty.Name, i].Value = var.Quantity - var.RefQuantity;
                this.dgDetail[colQuantity.Name, i].Value = "";// var.Quantity - var.RefQuantity;
                this.dgDetail[colPrice.Name, i].Value = var.Price;
                //认价价格
                if (this.excuteType == EnumStockExecType.安装)
                {
                    this.dgDetail[colConfirmPrice.Name, i].Value = var.TempData;
                    if (!string.IsNullOrEmpty(var.TempData))
                    {

                        this.dgDetail[colConfirmMoney.Name, i].Value = (decimal.Parse(var.TempData) * (var.Quantity - var.RefQuantity)).ToString("###############0.##");
                        //this.dgDetail[colConfirmMoney, i].Value = ( decimal .Parse ( var.TempData) * (var.Quantity - var.RefQuantity)).ToString("###############0.##");
                    }
                }

                dgDetail[colMoney.Name, i].Value = (var.Price * (var.Quantity - var.RefQuantity)).ToString("###############0.##");
                if (cboProfessionCat.SelectedItem != null)
                {
                    this.dgDetail[colProfessionalCategory.Name, i].Value = cboProfessionCat.SelectedItem.ToString();
                }
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;

                StockInDtl stockInDtl = new StockInDtl();
                if (var.SupplyOrderDetail != null)
                {
                    stockInDtl.OriginalContractNo = (var.SupplyOrderDetail.Master as SupplyOrderMaster).OldContractNum;
                    stockInDtl.SupplyOrderDetailId = var.SupplyOrderDetail.Id;
                    dgDetail[colOriginalContractNo.Name, i].Value = stockInDtl.OriginalContractNo;
                }
                stockInDtl.UsedPart = var.UsedPart;
                stockInDtl.UsedPartName = var.UsedPartName;
                dgDetail.Rows[i].Tag = stockInDtl;
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
                    curBillMaster.Details.Remove(dr.Tag as StockInDtl);
                    movedDtlList.Add(dr.Tag as StockInDtl);
                }
            }
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            curBillMaster.Details.Remove(e.Row.Tag as BaseDetail);
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string code)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.StockInSrv.GetStockInByCode(code, Enum.GetName(typeof(EnumStockExecType), ExcuteType), StaticMethod.GetProjectInfo().Id);
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
        public void Start(string code, string id)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.StockInSrv.GetStockInById(id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtForwardCode, txtSumConfirmMoney, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialStuff.Name, colMaterialCode.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colMoney.Name, colConfirmMoney.Name, colPrice.Name, colOriginalContractNo.Name };
            dgDetail.SetColumnsReadOnly(lockCols);

            if (curBillMaster != null && curBillMaster.Code != null)
            {
                ReadOnlyCat(true);
            }
            else
            {
                ReadOnlyCat(false);
            }
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
                ReadOnlyCat(false);

                movedDtlList = new ArrayList();

                this.curBillMaster = new StockIn();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                curBillMaster.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                curBillMaster.StockInManner = EnumStockInOutManner.收料入库;
                curBillMaster.Special = Enum.GetName(typeof(EnumStockExecType), ExcuteType);

                //仓库
                curBillMaster.TheStationCategory = StaticMethod.GetStationCategory();

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                // txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
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
            if (curBillMaster.IsTally == 1)
            {
                MessageBox.Show("此单己记账，不能修改！");
                return false;
            }
            base.ModifyView();
            movedDtlList.Clear();
            curBillMaster = model.StockInSrv.GetStockInById(curBillMaster.Id);

            if (this.excuteType == EnumStockExecType.安装)
            {
                curBillBalMaster = model.StockInSrv.GetStockInBalMasterById(curBillMaster.Id);
            }
            ModelToView();
            return true;
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


                if (curBillMaster.Id == null)
                {

                    curBillMaster = model.StockInSrv.SaveStockIn(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "收料入库单", "", curBillMaster.ProjectName);
                    #region
                    //MAppPlatform AppModel = new MAppPlatform();
                    //AppModel.Service.AppCommitBusiness(curBillMaster);
                    //if (!CreateStockInBalMaster()) return false;
                    //if (this.excuteType == EnumStockExecType.安装)
                    //{
                    //    //curBillBalMaster = model.StockInSrv.SaveStockInBalMaster2(curBillBalMaster, movedDtlList);
                    //    ////插入日志
                    //    //StaticMethod.InsertLogData(curBillBalMaster.Id, "新增保存", curBillBalMaster.Code, ConstObject.LoginPersonInfo.Name, "验收结算单", "", curBillBalMaster.ProjectName);
                    //    curBillBalMaster.DocState = DocumentState.InExecute;

                    //    curBillBalMaster = model.StockInSrv.SaveStockInBalMaster2(curBillBalMaster, movedDtlList);
                    //    //插入日志
                    #endregion
                    //安装回写滚动需求计划移到服务中
                    //if (curBillMaster.Special == "安装")
                    //{
                    //    foreach (StockInDtl dtl in curBillMaster.Details)
                    //    {
                    //        BackResourcePlan(dtl);
                    //    }
                    //}

                    //}
                }
                else
                {
                    curBillMaster = model.StockInSrv.SaveStockIn(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "收料入库单", "", curBillMaster.ProjectName);


                    //if (this.excuteType == EnumStockExecType.安装)
                    //{
                    //    curBillBalMaster = model.StockInSrv.SaveStockInBalMaster2(curBillBalMaster, movedDtlList);
                    //    //插入日志
                    //    StaticMethod.InsertLogData(curBillBalMaster.Id, "修改", curBillBalMaster.Code, ConstObject.LoginPersonInfo.Name, "验收结算单", "", curBillBalMaster.ProjectName);
                    //}
                }

                movedDtlList.Clear();

                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                try
                {
                    //记账
                    //if (MessageBox.Show("保存成功，是否记账", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    //{
                    IList billIdList = new ArrayList();
                    billIdList.Add(curBillMaster.Id);

                    IList billCodeList = new ArrayList();
                    billCodeList.Add(curBillMaster.Code);

                    Hashtable hashBillId = new Hashtable();
                    hashBillId.Add("StockIn", billIdList);

                    Hashtable hashBillCode = new Hashtable();
                    hashBillCode.Add("StockIn", billCodeList);
      
                    Hashtable tallyResult = model.TallyStockIn(hashBillId, hashBillCode);
                    if (tallyResult != null)
                    {
                        string errMsg = (string)tallyResult["err"];
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            TallyError();
                            MessageBox.Show(errMsg);
                        }
                        else
                        {
                            curBillMaster.IsTally = 1;
                            MessageBox.Show("记账成功。");
                        }
                    }
                    //}
                }
                catch (Exception ex)
                {
                    TallyError();
                    MessageBox.Show("记账出错。" + ExceptionUtil.ExceptionMessage(ex));
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }
        public void TallyError()
        {
            try
            {

                if (curBillMaster != null && !string.IsNullOrEmpty(curBillMaster.Id) && ExcuteType == EnumStockExecType.安装)
                {
                    model.StockInSrv.DeleteStockInBalMaster(curBillMaster);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除验收单失败:"+ExceptionUtil.ExceptionMessage(ex));
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.StockInSrv.GetStockInById(curBillMaster.Id);
                if (curBillMaster.IsTally == 0)
                {
                    model.StockInSrv.DeleteStockIn(curBillMaster);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "收料入库单", "", curBillMaster.ProjectName);
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单己记账，不能删除！");
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
                        curBillMaster = model.StockInSrv.GetStockInById(curBillMaster.Id);
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
                curBillMaster = model.StockInSrv.GetStockInById(curBillMaster.Id);
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
            if (txtSupply.Result.Count == 0)
            {
                MessageBox.Show("供应商不能为空");
                txtSupply.Focus();
                return false;
            }
            if (ClientUtil.ToString(this.txtCPH.Text) == "")
            {
                MessageBox.Show("车牌号不能为空！");
                txtCPH.Focus();
                return false;
            }
            if (ExcuteType == EnumStockExecType.安装)
            {
                if (cboProfessionCat.SelectedItem == null)
                {
                    MessageBox.Show("请选择专业分类");
                    return false;
                }
            }
            else if (ExcuteType == EnumStockExecType.土建)
            {
                if (txtMaterialCategory.Text == "" || txtMaterialCategory.Result == null || txtMaterialCategory.Result.Count == 0)
                {
                    MessageBox.Show("请选择物资分类。");
                    return false;
                }
            }

            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            //MStockMng model = new MStockMng();
             
            string sMsg = model.StockInSrv.CheckAccounted( ConstObject.TheLogin.TheAccountOrgInfo,ClientUtil.ToDateTime(this.dtpDateBegin.Text), StaticMethod.GetProjectInfo().Id);
           
          
            if (sMsg != string.Empty)
            {
                MessageBox.Show(sMsg);
                return false;
            }
            if (ClientUtil.ToDateTime(this.dtpDateBegin.Text) > ConstObject.TheLogin.TheComponentPeriod.EndDate)
            {
                MessageBox.Show("当前业务日期[" + this.dtpDateBegin.Text + "]已超出当前会计期的结束日期[" + ConstObject.TheLogin.TheComponentPeriod.EndDate.ToShortDateString() + "]！");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            //明细表数据校验
            IList materialList = new ArrayList();//校验业务日期
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                else {
                    Material material = dr.Cells[colMaterialCode.Name].Tag as Material;
                    materialList.Add(material.Id);
                }

                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }

                if (ClientUtil.ToString(dr.Cells[this.colAppearanceQuality.Name].Value) == "")
                {
                    MessageBox.Show("外观质量不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colAppearanceQuality.Name];
                    return false;
                }

                if (ClientUtil.ToString(dr.Cells[this.colCalculate.Name].Value) == "" && this.excuteType != EnumStockExecType.安装)
                {
                    MessageBox.Show("计算式不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colCalculate.Name];
                    return false;
                }

                if (dr.Cells[colQuantity.Name].Value == null || dr.Cells[colQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colQuantity.Name].Value) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                DailyPlanDetail forwardDetail = model.StockInSrv.GetDailyPlanDetailById(forwardDtlId.ToString());
                if (forwardDetail == null)
                {
                    MessageBox.Show("未找到前驱单据明细,请重新引用。");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }
                else
                {
                    decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;
                    decimal currentQty = decimal.Parse(dr.Cells[colQuantity.Name].Value.ToString());
                    object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;
                    decimal qtyTemp = 0;
                    if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                    {
                        qtyTemp = decimal.Parse(qtyTempObj.ToString());
                    }

                    if (currentQty - qtyTemp - canUseQty > 0)
                    {

                        //MessageBox.Show("输入数量【" + currentQty + "】大于可引用数量【" + (canUseQty + qtyTemp) + "】。");
                        //dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                        //return false;
                    }
                }

                if (dr.Cells[colPrice.Name].Value != null && !string.IsNullOrEmpty(dr.Cells[colPrice.Name].Value.ToString()))
                {
                    decimal tempPrice = 0M;
                    if (!decimal.TryParse(dr.Cells[colPrice.Name].Value.ToString(), out tempPrice))
                    {
                        MessageBox.Show("请输入正确的单价！");
                        return false;
                    }
                    if (tempPrice < 0)
                    {
                        MessageBox.Show("请输入正确的单价！");
                        return false;
                    }
                }

                if (dr.Cells[colConfirmPrice.Name].Value != null && !string.IsNullOrEmpty(dr.Cells[colConfirmPrice.Name].Value.ToString()))
                {
                    decimal tempConfirmPrice = 0M;
                    if (!decimal.TryParse(dr.Cells[colConfirmPrice.Name].Value.ToString(), out tempConfirmPrice))
                    {
                        MessageBox.Show("请输入正确的认价单价！");
                        return false;
                    }
                    if (tempConfirmPrice < 0)
                    {
                        MessageBox.Show("请输入正确的认价单价！");
                        return false;
                    }
                }
            }

            if (ExcuteType == EnumStockExecType.土建)
            {
                sMsg = model.StockInSrv.CheckBusinessDate(ConstObject.TheLogin.TheAccountOrgInfo, ClientUtil.ToDateTime(this.dtpDateBegin.Text), materialList, StaticMethod.GetProjectInfo().Id);
                if (sMsg != string.Empty)
                {
                    MessageBox.Show(sMsg);
                    return false;
                }
            }
            dgDetail.Update();
            return true;
        }
        void qantity(DataGridViewRow dr, StockInDtl curBillDtl)
        {
            object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
            DailyPlanDetail forwardDetail = model.StockInSrv.GetDailyPlanDetailById(forwardDtlId.ToString());
            decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;
            decimal currentQty = decimal.Parse(dr.Cells[colQuantity.Name].Value.ToString());
            object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;
            decimal qtyTemp = 0;
            if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
            {
                qtyTemp = decimal.Parse(qtyTempObj.ToString());
            }
            //没有明白这块代码的含义 因为现在在土建里面 有可能入库单数量大于日常需求计划剩余数量 
            //if (currentQty - qtyTemp - canUseQty > 0)
            //{
            //    curBillDtl.BalQuantity = currentQty;
            //    curBillDtl.RefQuantity = 0;
            //}

        }
        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                if (this.txtSupply.Result.Count > 0)
                {
                    curBillMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    curBillMaster.TheSupplierName = txtSupply.Text;
                }
                curBillMaster.CreateDate = ClientUtil.ToDateTime(dtpDateBegin.Value.Date.ToShortDateString());

                DataTable oTable = model.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }

                curBillMaster.Descript = this.txtRemark.Text;
                curBillMaster.ForwardBillCode = txtForwardCode.Text;
                curBillMaster.Cph = ClientUtil.ToString(txtCPH.Text);
                curBillMaster.SumConfirmMoney = ClientUtil.TransToDecimal(txtSumConfirmMoney.Text);
                curBillMaster.SumMoney = ClientUtil.TransToDecimal(txtSumMoney.Text);
                curBillMaster.SumQuantity = ClientUtil.TransToDecimal(txtSumQuantity.Text);
                //curBillMaster.SubmitDate = DateTime.Now;

                //物资分类
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MatCatName = txtMaterialCategory.Text;
                }

                //专业分类
                curBillMaster.ProfessionCategory = cboProfessionCat.SelectedItem == null ? null : cboProfessionCat.SelectedItem.ToString();

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    StockInDtl curBillDtl = new StockInDtl();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as StockInDtl;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    Material material = var.Cells[colMaterialCode.Name].Tag as Material;
                    curBillDtl.MaterialResource = material;
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colMaterialStuff.Name].Value);
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    curBillDtl.AppearanceQuality = ClientUtil.ToString(var.Cells[colAppearanceQuality.Name].Value);
                    curBillDtl.Calculate = ClientUtil.ToString(var.Cells[colCalculate.Name].Value);
                    decimal quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);
                    decimal quantityTemp = ClientUtil.ToDecimal(var.Cells[colQuantityTemp.Name].Value);

                    curBillDtl.Quantity = quantity;
                    curBillDtl.QuantityTemp = quantityTemp;
                    if (var.Cells[colQuantity.Name].Tag != null)
                    {
                        curBillDtl.WeightBillDetail = var.Cells[colQuantity.Name].Tag as WeightBillDetail;
                    }
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[colMoney.Name].Value);
                    curBillDtl.ConfirmPrice = ClientUtil.TransToDecimal(var.Cells[colConfirmPrice.Name].Value);
                    curBillDtl.ConfirmMoney = ClientUtil.TransToDecimal(var.Cells[colConfirmMoney.Name].Value);
                    qantity(var, curBillDtl);
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号

                    if (var.Cells[colProfessionalCategory.Name].Value != null)
                    {
                        curBillDtl.ProfessionalCategory = var.Cells[colProfessionalCategory.Name].Value.ToString();
                    }
                    else
                    {
                        curBillDtl.ProfessionalCategory = null;
                    }
                    //前驱明细Id
                    curBillDtl.ForwardDetailId = var.Cells[colForwardDtlId.Name].Value.ToString();
                    curBillDtl.OriginalContractNo = ClientUtil.ToString(var.Cells[colOriginalContractNo.Name].Value);

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
        /// 物料编码列增加事件监听，支持处理键盘回车查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.PreviewKeyDown -= new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                textBox.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals(colMaterialCode.Name))
                {
                    textBox.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }

            }
        }

        /// <summary>
        /// 键盘回车查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (base.ViewState == MainViewState.Browser || base.ViewState == MainViewState.Initialize) return;

            if (e.KeyValue == 13)
            {
                CommonMaterial materialSelector = new CommonMaterial();

                TextBox textBox = sender as TextBox;
                if (textBox.Text != null && !textBox.Text.Equals(""))
                {
                    materialSelector.OpenSelect(textBox.Text);
                }
                else
                {
                    materialSelector.OpenSelect();
                }
                IList list = materialSelector.Result;

                if (list != null && list.Count > 0)
                {
                    Application.Resource.MaterialResource.Domain.Material selectedMaterial = list[0] as Application.Resource.MaterialResource.Domain.Material;
                    this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = selectedMaterial;
                    this.dgDetail.CurrentCell.Value = selectedMaterial.Code;
                    this.dgDetail.CurrentRow.Cells[colMaterialName.Name].Value = selectedMaterial.Name;
                    this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = selectedMaterial.Specification;

                    //动态分类复合单位                    
                    DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
                    cbo.Items.Clear();

                    StandardUnit first = null;
                    foreach (StandardUnit cu in selectedMaterial.GetMaterialAllUnit())
                    {
                        cbo.Items.Add(cu.Name);
                    }
                    first = selectedMaterial.BasicUnit;
                    this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = first;
                    cbo.Value = first.Name;
                    this.dgDetail.RefreshEdit();
                }
            }
        }

        /// <summary>
        /// 在物料编码列，敲击键盘时，取消原来已经选择的物料，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //this.dgDetail.CurrentCell.Tag = null;
            //this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialName.Name].Value = "";
            //this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialSpec.Name].Value = "";
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgDetail.EndEdit();
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail.Columns[e.ColumnIndex].Name == this.colQuantity.Name)
            {
                DataGridViewRow oRow = dgDetail.Rows[e.RowIndex];
                string sMaterialCode = ClientUtil.ToString(oRow.Cells[this.colMaterialCode.Name].Value);
                SupplierRelationInfo oSupplierRelationInfo = string.IsNullOrEmpty(this.txtSupply.Text) ? null : (this.txtSupply.Result[0] as SupplierRelationInfo);
                string sSupplyCode = oSupplierRelationInfo==null?null:oSupplierRelationInfo.Code;//有问题
                VWeightBillSelecter oVWeightBillSelecter = new VWeightBillSelecter(true, sMaterialCode, sSupplyCode);
                oVWeightBillSelecter.StartPosition = FormStartPosition.CenterParent;
                oVWeightBillSelecter.MaximizeBox = false;
                oVWeightBillSelecter.ShowDialog();
                if (oVWeightBillSelecter.SelectResult!=null && oVWeightBillSelecter.SelectResult.Count > 0)
                {
                    WeightBillDetail oWeightBillDetail = oVWeightBillSelecter.SelectResult[0];
                    oRow.Cells[colQuantity.Name].Tag = oWeightBillDetail;
                    oRow.Cells[colQuantity.Name].Value = oWeightBillDetail.SJSL;
                    dgDetail.EndEdit();
                    this.txtCode.Focus();
                }
            }
            //if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            //{
            //    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            //    CommonMaterial materialSelector = new CommonMaterial();
            //    DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
            //    object tempValue = cell.EditedFormattedValue;
            //    if (tempValue != null && !tempValue.Equals(""))
            //    {
            //        //materialSelector.OpenSelect(tempValue.ToString());
            //        materialSelector.OpenSelect();
            //    }
            //    else
            //    {
            //        materialSelector.OpenSelect();
            //    }

            //    IList list = materialSelector.Result;
            //    foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
            //    {
            //        int i = dgDetail.Rows.Add();
            //        this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
            //        this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
            //        this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
            //        this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
            //        this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
            //        if (theMaterial.BasicUnit != null)
            //            this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
            //        dgDetail[colPrice.Name, i].Value = 0;
            //        dgDetail[colQuantity.Name, i].Value = 0;
            //        dgDetail[colConfirmPrice.Name, i].Value = 0;
            //    }
            //}
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;

                dtpDateBegin.Value = curBillMaster.CreateDate;

                txtHandlePerson.Text = curBillMaster.HandlePersonName;

                txtForwardCode.Text = curBillMaster.ForwardBillCode;

                if (curBillMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = curBillMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(curBillMaster.TheSupplierRelationInfo);
                    txtSupply.Value = curBillMaster.TheSupplierName;
                }
                txtRemark.Text = curBillMaster.Descript;
                // txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtCPH.Text = curBillMaster.Cph;
                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString();
                txtSumMoney.Text = curBillMaster.SumMoney.ToString();
                txtSumConfirmMoney.Text = curBillMaster.SumConfirmMoney.ToString();

                txtProject.Text = curBillMaster.ProjectName;

                ReadOnlyCat(false);

                //专业分类
                if (curBillMaster.ProfessionCategory != null)
                {
                    cboProfessionCat.SelectedItem = curBillMaster.ProfessionCategory;
                }

                //物资分类
                if (curBillMaster.MaterialCategory != null)
                {
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);
                    txtMaterialCategory.Value = curBillMaster.MatCatName;
                }
                if (string.IsNullOrEmpty(curBillMaster.Code))
                {
                    ReadOnlyCat(false);
                }
                else
                {
                    ReadOnlyCat(true);
                }

                this.dgDetail.Rows.Clear();
                foreach (StockInDtl var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colMaterialStuff.Name, i].Value = var.MaterialStuff;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    this.dgDetail[colCalculate.Name, i].Value = var.Calculate;
                    this.dgDetail[colAppearanceQuality.Name, i].Value = var.AppearanceQuality;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    dgDetail[colMoney.Name, i].Value = var.Money;
                    dgDetail[colConfirmPrice.Name, i].Value = var.ConfirmPrice;
                    dgDetail[colConfirmMoney.Name, i].Value = var.ConfirmMoney;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    dgDetail[colProfessionalCategory.Name, i].Value = var.ProfessionalCategory;
                    dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;
                    dgDetail[colOriginalContractNo.Name, i].Value = var.OriginalContractNo;
                    this.dgDetail.Rows[i].Tag = var;

                    dgDetail[colRemainQty.Name, i].Value = model.StockInSrv.GetRemainQty(var.ForwardDetailId);

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

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPrice.Name || colName == colQuantity.Name || colName == colConfirmPrice.Name)
            {
                object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
                object confirmPrice = dgDetail.Rows[e.RowIndex].Cells[colConfirmPrice.Name].Value;

                if (quantity != null)
                {
                    string temp_quantity = quantity.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail[e.ColumnIndex, e.RowIndex].Selected = true;
                        dgDetail.BeginEdit(false);
                        return;
                    }
                }

                if (price != null)
                {
                    string temp_price = price.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail[e.ColumnIndex, e.RowIndex].Selected = true;
                        dgDetail.BeginEdit(false);
                        return;
                    }
                }

                if (confirmPrice != null)
                {
                    validity = CommonMethod.VeryValid(confirmPrice.ToString());
                    if (!validity)
                    {
                        MessageBox.Show("请输入数字");
                        dgDetail[e.ColumnIndex, e.RowIndex].Selected = true;
                        dgDetail.BeginEdit(false);
                        return;
                    }
                }

                //根据单价和数量计算金额                
                decimal sumqty = 0, sumMoney = 0, sumConfirmMoney = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                    if (quantity == null) quantity = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);

                    price = dgDetail.Rows[i].Cells[colPrice.Name].Value;
                    if (price == null) price = 0;
                    decimal money = ClientUtil.TransToDecimal(quantity) * ClientUtil.TransToDecimal(price);
                    dgDetail.Rows[i].Cells[colMoney.Name].Value = money;
                    sumMoney += money;

                    confirmPrice = dgDetail.Rows[i].Cells[colConfirmPrice.Name].Value;
                    if (confirmPrice == null) confirmPrice = 0;
                    decimal comfirmMoney = ClientUtil.TransToDecimal(quantity) * ClientUtil.TransToDecimal(confirmPrice);
                    dgDetail.Rows[i].Cells[colConfirmMoney.Name].Value = comfirmMoney;
                    sumConfirmMoney += comfirmMoney;
                }

                txtSumQuantity.Text = sumqty.ToString();
                txtSumConfirmMoney.Text = sumConfirmMoney.ToString("########.##");
                txtSumMoney.Text = sumMoney.ToString("#########.##");
            }
        }

        #region 打印处理
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"收料单.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else
            {
                if (LoadTempleteFile(@"收料单_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            return true;
        }

        public override bool Print()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"收料单.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.Print();
                //curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
                //curBillMaster = model.StockInSrv.SaveStockIn(curBillMaster, movedDtlList);
            }
            else
            {
                if (LoadTempleteFile(@"收料单_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.Print();
                //curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
                //curBillMaster = model.StockInSrv.SaveStockIn(curBillMaster, movedDtlList);
            }
            return true;
        }

        public override bool Export()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"收料单.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.ExportToExcel("收料单【" + curBillMaster.Code + "】", false, false, true);
            }
            else
            {
                if (LoadTempleteFile(@"收料单_安装.flx.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.ExportToExcel("收料单【" + curBillMaster.Code + "】", false, false, true);
            }
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

        private void FillFlex(StockIn billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(4, 6).Text = billMaster.MatCatName;
            flexGrid1.Cell(3, 6).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 2).Text = billMaster.CreateDate.ToShortDateString();

            decimal sumQuantity = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockInDtl detail = (StockInDtl)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                //数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();
                sumQuantity += ClientUtil.ToDecimal(detail.Quantity);
                //计算式
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.Calculate;
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;

                //外观质量
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.AppearanceQuality;
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                //车牌号
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = billMaster.Cph;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
            this.flexGrid1.Cell(1, 7).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 7).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 7).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            flexGrid1.Cell(7 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(7 + detailCount, 6).Text = billMaster.RealOperationDate.ToShortDateString();
            flexGrid1.Cell(7 + detailCount, 8).Text = billMaster.CreatePersonName;
        }
        private void FillFlex1(StockIn billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            //主表数据

            //他们给的单据从布局和内容上有好多信息需要修改，

            flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(3, 8).Text = billMaster.Code;
            flexGrid1.Cell(3, 6).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 2).Text = billMaster.TheSupplierName;//供货单位
            flexGrid1.Cell(4, 6).Text = billMaster.ProfessionCategory;
            flexGrid1.Cell(4, 8).Text = billMaster.ContractNo;
            flexGrid1.Row(3).AutoFit();
            flexGrid1.Row(4).AutoFit();
            decimal sumMoney = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockInDtl detail = (StockInDtl)billMaster.Details.ElementAt(i);
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = ClientUtil.ToString(i + 1);
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MaterialSpec;
                flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.DiagramNumber;
                flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.MatStandardUnitName;
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.Price);
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.Money);
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                sumMoney += detail.Money;
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 2).Text = ClientUtil.ToString(sumMoney);
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 6).Text = ClientUtil.ToString(Moneybig);
                }
                //条形码
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumMoney));
                this.flexGrid1.Cell(1, 8).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
                this.flexGrid1.Cell(1, 8).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 8).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
        }
        #endregion

        public void BackResourcePlan(StockInDtl dtl)
        {
            if (dtl.ForwardDetailId != null)
            {
                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddCriterion(Expression.Eq("MaterialResource.Id", dtl.MaterialResource.Id));
                if (dtl.DiagramNumber == "")
                {
                    oq1.AddCriterion(Expression.IsNull("DiagramNumber"));
                }
                else
                {
                    oq1.AddCriterion(Expression.Eq("DiagramNumber", dtl.DiagramNumber));
                }

                oq1.AddCriterion(Expression.Like("TheGWBSTaskGUID.Id", dtl.UsedPart.Id));
                IList list = model.StockInSrv.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                if (list == null || list.Count == 0) return;
                foreach (ResourceRequirePlanDetail plan in list)
                {
                    plan.ExecutedQuantity += dtl.Quantity;
                }
                model.StockInSrv.SaveOrUpdateByDao(list);
            }
        }

    }
}
