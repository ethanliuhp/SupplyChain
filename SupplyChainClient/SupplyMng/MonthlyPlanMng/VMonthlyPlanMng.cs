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
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng
{
    public partial class VMonthlyPlanMng : TMasterDetailView
    {
        private MMonthlyPlanMng model = new MMonthlyPlanMng();
        private MonthlyPlanMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        private MDemandMasterPlanMng demandmaster = new MDemandMasterPlanMng();
        CurrentProjectInfo projectInfo;

        private EnumMonthlyType monthlyType;
        /// <summary>
        /// 用来区分专业
        /// </summary>
        public EnumMonthlyType MonthlyType
        {
            get { return monthlyType; }
            set { monthlyType = value; }
        }

        public MonthlyPlanMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VMonthlyPlanMng()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
            InitPlanType();
        }

        private void InitPlanType()
        {
            comPlanType.DataSource = (Enum.GetNames(typeof(EnumPlanType)));
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
        }

        public void InitDate()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            DateTimePicker dp = new DateTimePicker();
            DataGridViewCalendarColumn dc = new DataGridViewCalendarColumn();
            dp.CustomFormat = "yyyy-MM-dd";
            dp.Format = DateTimePickerFormat.Custom;
            dp.Visible = false;
            dgDetail.Controls.Add(dp);
            dp = new DateTimePicker();
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            txtMaterialCategory.Leave += new EventHandler(txtMaterialCategory_Leave);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);
        }

        void cboProfessionCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pc = cboProfessionCat.SelectedItem as string;
            if (pc != null)
            {
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    dr.Cells[colSpecailType.Name].Value = pc;
                }
            }
        }

        void txtMaterialCategory_Leave(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if (dr.IsNewRow) break;
                dr.Cells[colMaterialType.Name].Value = txtMaterialCategory.Text;
                dr.Cells[colMaterialType.Name].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
            }
        }

        /// <summary>
        /// 查找一级分类
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        private MaterialCategory FindFirstCategory(MaterialCategory mc)
        {
            //if (mc.Level == 2) return mc;
            return FindFirstCategory((MaterialCategory)mc.ParentNode);
        }


        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            InitControls();
        }
        /// <summary>
        /// 根据土建、安装 界面不同显示
        /// </summary>
        private void InitControls()
        {
            if (MonthlyType == EnumMonthlyType.土建)
            {
                colPrice.Visible = false;
                colMoney.Visible = false;
                customLabel2.Visible = false;
                txtSumMoney.Visible = false;
                lblCat.Text = "物资分类";
                txtMaterialCategory.Visible = true;
                cboProfessionCat.Visible = false;
                colSpecailType.Visible = false;

            }
            if (MonthlyType == EnumMonthlyType.安装)
            {
                lblCat.Text = "专业分类";
                txtMaterialCategory.Visible = false;
                cboProfessionCat.Visible = true;
                colMaterialType.Visible = false;
                colSpecailType.Visible = true;
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
                    curBillMaster.Details.Remove(dr.Tag as MonthlyPlanDetail);
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
                    curBillMaster = model.MonthlyPlanSrv.GetMonthlyPlanById(Id);
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
                    comPlanType.Enabled = true;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    comPlanType.Enabled = false;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtHandlePerson, txtSumQuantity, txtProject, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colRealInQuantity.Name, colMaterialSpec.Name, colUnit.Name, colMoney.Name, colSpecailType.Name, colMaterialType.Name, colUsedPart.Name, colUsedRanks.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            this.txtMaterialCategory.Result.Clear();
            this.txtMaterialCategory.Tag = null;
            this.txtMaterialCategory.Value = "";
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

                this.curBillMaster = new MonthlyPlanMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                curBillMaster.Special = Enum.GetName(typeof(EnumMonthlyType), MonthlyType);
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
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
                curBillMaster = model.MonthlyPlanSrv.GetMonthlyPlanById(curBillMaster.Id);
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
                if (!ViewToModel()) return false;
                curBillMaster.DocState = DocumentState.InExecute;
                bool IsNew = (string.IsNullOrEmpty(curBillMaster.Id) ? true : false);
                curBillMaster = model.MonthlyPlanSrv.SaveMonthlyPlan(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;

                //#region 短信
                //MAppPlatMng appModel = new MAppPlatMng();
                //appModel.SendMessage(curBillMaster.Id, "MonthlyPlanMaster");
                WriteLog(IsNew);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                curBillMaster.DocState = DocumentState.Edit;
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
                curBillMaster = model.SaveMonthlyPlanMng(curBillMaster, movedDtlList);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                WriteLog(flag);

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

        private void WriteLog(bool IsNew)
        {
            LogData log = new LogData();
            log.BillId = curBillMaster.Id;
            log.BillType = "月度需求计划单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            if (IsNew)
            {
                if (MonthlyType == EnumMonthlyType.安装)
                {
                    log.OperType = "新增 - 安装";
                }
                if (MonthlyType == EnumMonthlyType.土建)
                {
                    log.OperType = "新增 - 土建";
                }
            }
            else
            {
                if (curBillMaster.DocState == DocumentState.InExecute)
                {
                    if (MonthlyType == EnumMonthlyType.安装)
                    {
                        log.OperType = "提交- 安装";
                    }
                    if (MonthlyType == EnumMonthlyType.土建)
                    {
                        log.OperType = "提交 - 土建";
                    }
                }
                else
                {
                    if (MonthlyType == EnumMonthlyType.安装)
                    {
                        log.OperType = "修改 - 安装";
                    }
                    if (MonthlyType == EnumMonthlyType.土建)
                    {
                        log.OperType = "修改 - 土建";
                    }
                }
            }
            StaticMethod.InsertLogData(log);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.MonthlyPlanSrv.GetMonthlyPlanById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.MonthlyPlanSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "月度需求计划单";
                    log.Code = curBillMaster.Code;
                    if (MonthlyType == EnumMonthlyType.安装)
                    {
                        log.OperType = "删除 - 安装";
                    }
                    if (MonthlyType == EnumMonthlyType.土建)
                    {
                        log.OperType = "删除 - 土建";
                    }
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
                        curBillMaster = model.MonthlyPlanSrv.GetMonthlyPlanById(curBillMaster.Id);
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
                curBillMaster = model.MonthlyPlanSrv.GetMonthlyPlanById(curBillMaster.Id);
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
            if (monthlyType == EnumMonthlyType.土建)
            {
                if (txtMaterialCategory.Text.Equals("") || txtMaterialCategory.Text.Equals(null))
                {
                    MessageBox.Show("物资分类为必选项！");
                    return false;
                }
            }
            if (monthlyType == EnumMonthlyType.安装)
            {
                if (this.cboProfessionCat.SelectedItem != null)
                { }
                else
                {
                    MessageBox.Show("专业分类为必选项！");
                    return false;
                }
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

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }

                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }

                if (dr.Cells[colPlanQuantity.Name].Value == null || dr.Cells[colPlanQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPlanQuantity.Name].Value) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colPlanQuantity.Name];
                    return false;
                }
                if (ClientUtil.ToString(dr.Cells[this.colQualityStandard.Name].Value) == "")
                {
                    MessageBox.Show("质量标准不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colQualityStandard.Name];
                    return false;
                }
                if (MonthlyType == EnumMonthlyType.安装)
                {
                    if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                    {
                        MessageBox.Show("单价不允许为空！");
                        dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                        return false;
                    }
                }
                if (ClientUtil.ToDecimal(dr.Cells[colNeedQuantity.Name].Value) < ClientUtil.ToDecimal(dr.Cells[colPlanQuantity.Name].Value))
                {
                    MessageBox.Show("需求量应大于等于计划数量！");
                    dgDetail.CurrentCell = dr.Cells[colNeedQuantity.Name];
                    dgDetail.CurrentCell = dr.Cells[colPlanQuantity.Name];
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
                curBillMaster.CreateDate = dtpDateBegin.Value.Date;//业务日期
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                if (txtSumMoney.Visible == true)
                {
                    curBillMaster.SumMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                }

                //物资分类
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MaterialCategoryName = txtMaterialCategory.Text;
                }

                curBillMaster.SpecialType = cboProfessionCat.SelectedItem == null ? null : cboProfessionCat.SelectedItem.ToString();

                curBillMaster.PlanType = Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain.ExecuteDemandPlanTypeEnum.物资计划;
                curBillMaster.MonthePlanType = this.comPlanType.Text;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MonthlyPlanDetail curBillDtl = new MonthlyPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as MonthlyPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colPlanQuantity.Name].Value);//数量
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);//单价
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//描述
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[colMoney.Name].Value);//金额
                    curBillDtl.RealInQuantity = ClientUtil.ToDecimal(var.Cells[colRealInQuantity.Name].Value);//实际进场量
                    curBillDtl.SpecialType = ClientUtil.ToString(var.Cells[colSpecailType.Name].Value);//专业分类
                    curBillDtl.MaterialCategory = var.Cells[colMaterialType.Name].Tag as MaterialCategory;
                    curBillDtl.MaterialCategoryName = ClientUtil.ToString(var.Cells[colMaterialType.Name].Value);
                    curBillDtl.NeedQuantity = ClientUtil.ToDecimal(var.Cells[colNeedQuantity.Name].Value);//需求数量
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);//物资名称
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格型号
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colSuff.Name].Value);//材质
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colQualityStandard.Name].Value);//质量标准
                    curBillDtl.UsedRank = var.Cells[colUsedRanks.Name].Tag as SupplierRelationInfo;//使用队伍
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    curBillDtl.MaterialType = ClientUtil.ToString(var.Cells[colMaterialType.Name].Value);//物资分类
                    curBillDtl.UsedRankName = ClientUtil.ToString(var.Cells[colUsedRanks.Name].Value);
                    curBillDtl.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;//使用部位
                    curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);//使用部位
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

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals("MaterialCode"))
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
                    this.dgDetail.CurrentRow.Cells[colSuff.Name].Value = selectedMaterial.Stuff;

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
            this.dgDetail.CurrentCell.Tag = null;
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialName.Name].Value = "";
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialSpec.Name].Value = "";
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (this.txtMaterialCategory.Visible == true && (this.txtMaterialCategory.Result == null || this.txtMaterialCategory.Result.Count == 0))
            {
                MessageBox.Show("请先选择物资分类！");
                return;
            }
            MaterialCategory cat = null;
            if (this.MonthlyType == EnumMonthlyType.安装)
            {
                cat = null;
            }
            else
            {
                cat = this.txtMaterialCategory.Result[0] as MaterialCategory;
            }
            if (cat != null && cat.Level != 3)
            {
                MessageBox.Show("请选择一级物资分类！");
                return;
            }

            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    CommonMaterial materialSelector = new CommonMaterial();
                    if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                    {
                        materialSelector.materialCatCode = (txtMaterialCategory.Result[0] as MaterialCategory).Code;
                    }
                    DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                    object tempValue = cell.EditedFormattedValue;
                    if (tempValue != null && !tempValue.Equals(""))
                    {
                        //materialSelector.OpenSelect(tempValue.ToString());
                        materialSelector.OpenSelect();
                    }
                    else
                    {
                        materialSelector.OpenSelect();
                    }

                    IList list = materialSelector.Result;
                    foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                        this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                        this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                        this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                        this.dgDetail[colSuff.Name, i].Value = theMaterial.Stuff;
                        this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;

                        if (theMaterial.BasicUnit != null)
                            this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                        if (txtMaterialCategory.Value != "")
                        {
                            dgDetail[colMaterialType.Name, i].Value = txtMaterialCategory.Value;
                            dgDetail[colMaterialType.Name, i].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
                        }
                        ObjectQuery oq = new ObjectQuery();

                        oq.AddCriterion(Expression.Eq("MaterialResource", theMaterial));
                        IList alists = demandmaster.DemandPlanSrv.GetDemandDetailPlan(oq);
                        foreach (DemandMasterPlanDetail dtl in alists)
                        {
                            this.dgDetail[colQualityStandard.Name, i].Value = dtl.QualityStandard;
                        }

                        string condition = "";
                        condition += " and t1.projectid = '" + projectInfo.Id + "'" + "and t2.material = '" + theMaterial.Id + "'";
                        DataSet dataSet = demandmaster.DemandPlanSrv.Stkstockindtl_RealInQuantity(condition);
                        DataTable dataTable = dataSet.Tables[0];
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            this.dgDetail[colRealInQuantity.Name, i].Value = dataRow["Quantity"];
                        }

                        //dgDetail[colBalECRule.Name, i].Value = Enum.GetNames(typeof(EnumMaterialMngBalRule)).First();
                        i++;
                    }

                }

                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedRanks.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VCommonSupplierRelationSelect comSelect = new VCommonSupplierRelationSelect();
                    comSelect.OpenSelectView("", CommonUtil.SupplierCatCode3);
                    IList list = comSelect.Result;
                    if (list != null && list.Count > 0)
                    {
                        SupplierRelationInfo relInfo = list[0] as SupplierRelationInfo;
                        this.dgDetail.CurrentRow.Cells[this.colUsedRanks.Name].Tag = relInfo;
                        this.dgDetail.CurrentRow.Cells[colUsedRanks.Name].Value = relInfo.SupplierInfo.Name;
                    }
                    this.txtCode.Focus();
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
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
                            this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Value = task.Name;
                            this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Tag = task;
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
                dtpDateBegin.Value = curBillMaster.CreateDate;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtRemark.Text = curBillMaster.Descript;

                //专业分类
                if (curBillMaster.SpecialType != null)
                {
                    cboProfessionCat.SelectedItem = curBillMaster.SpecialType;
                }

                //物资分类
                if (curBillMaster.MaterialCategory != null)
                {
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);
                    txtMaterialCategory.Value = curBillMaster.MaterialCategoryName;
                }
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString("#,###.####");
                txtSumMoney.Text = curBillMaster.SumMoney.ToString("#,###.####");
                txtProject.Text = curBillMaster.ProjectName;
                comPlanType.Text = curBillMaster.MonthePlanType;
                //comPlanType.Text = curBillMaster.PlanType.ToString();
                this.dgDetail.Rows.Clear();
                foreach (MonthlyPlanDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    if (var.MaterialResource != null)
                    {
                        this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                        this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                        this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                        this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                        this.dgDetail[colSuff.Name, i].Value = var.MaterialResource.Stuff;
                    }
                    //设置该物料的计量单位
                    if (var.MatStandardUnit != null)
                    {
                        this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                        this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnit.Name;
                    }
                    this.dgDetail[colPlanQuantity.Name, i].Value = var.Quantity;//计划数量
                    this.dgDetail[colPrice.Name, i].Value = var.Price;//单价
                    this.dgDetail[colMoney.Name, i].Value = var.Money;//金额
                    //this.dgDetail[colComeInDate.Name,i].Value = var.
                    this.dgDetail[colMaterialType.Name, i].Value = var.MaterialCategoryName;
                    this.dgDetail[colSpecailType.Name, i].Value = var.SpecialType;
                    this.dgDetail[colRealInQuantity.Name, i].Value = var.RealInQuantity;//实际进场量
                    this.dgDetail[colNeedQuantity.Name, i].Value = var.NeedQuantity;//需求数量
                    this.dgDetail[colMaterialType.Name, i].Value = var.MaterialType;//物资类别
                    this.dgDetail[colQualityStandard.Name, i].Value = var.QualityStandard;//质量标准
                    this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;//临时数量
                    this.dgDetail[colUsedRanks.Name, i].Value = var.UsedRankName;//使用队伍
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colUsedRanks.Name, i].Tag = var.UsedRank;
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;//使用部位
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;//备注
                    this.dgDetail.Rows[i].Tag = var;
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
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPrice.Name || colName == colPlanQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colPlanQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colPlanQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("计划数量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colPlanQuantity.Name].Value = "";
                        flag = false;
                    }
                }
                else
                {
                    return;
                }
                if (colPrice.Visible == true)
                {
                    if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                    {
                        string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                        validity = CommonMethod.VeryValid(temp_price);
                        if (validity == false)
                        {
                            MessageBox.Show("价格为数字！");
                            this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "";
                            flag = false;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                if (flag)
                {
                    //根据单价和数量计算金额  
                    object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colPlanQuantity.Name].Value;

                    decimal sumqty = 0;
                    decimal money = 0;
                    decimal summoney = 0;

                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colPlanQuantity.Name].Value;
                        if (quantity == null) quantity = 0;
                        if (MonthlyType == EnumMonthlyType.安装)
                        {
                            if (ClientUtil.ToString(dgDetail.Rows[i].Cells[colPlanQuantity.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[i].Cells[colPrice.Name].Value) != "")
                            {
                                money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colPlanQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colPrice.Name].Value);
                                dgDetail.Rows[i].Cells[colMoney.Name].Value = ClientUtil.ToString(money);
                                summoney = summoney + money;
                            }
                        }
                        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                    }

                    txtSumQuantity.Text = sumqty.ToString();
                    txtSumMoney.Text = summoney.ToString();
                }

            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"物资月度需求计划.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"物资月度需求计划.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            curBillMaster = model.MonthlyPlanSrv.SaveMonthlyPlan(curBillMaster);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"物资月度需求计划.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("物资月度需求计划【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(MonthlyPlanMaster billMaster)
        {
            int detailStartRowNumber = 5;//5为模板中的行号
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
            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 7).Text = billMaster.MaterialCategoryName;

            //填写明细数据
            decimal sumQuantity = 0;
            decimal sumNeedQuantity = 0;
            for (int i = 0; i < detailCount; i++)
            {
                MonthlyPlanDetail detail = (MonthlyPlanDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = ClientUtil.ToString(detail.MatStandardUnitName);
                flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
                //总计划量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
                sumQuantity += detail.Quantity;
                //需求数量
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.NeedQuantity);
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                sumNeedQuantity += detail.NeedQuantity;
                //质量标准
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.QualityStandard);
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                //使用部位
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.UsedPartName);
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                //detail.UsedPartName; 

                //使用队伍
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.UsedRankName);
                //detail.UsedRankName;
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.Descript);
                //detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumNeedQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                }
                //条形码
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
                this.flexGrid1.Cell(1, 8).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
                this.flexGrid1.Cell(1, 8).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 8).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            
            flexGrid1.Cell(6 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(6 + detailCount, 7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(6 + detailCount, 9).Text = billMaster.CreatePersonName;

            //审批信息打印
            int maxRow = detailStartRowNumber + detailCount + 4;
            CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id, maxRow);
        }
    }
}
