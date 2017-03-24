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
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
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
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Service;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng
{
    public partial class VDailyPlanMng : TMasterDetailView
    {
        private MDailyPlanMng model = new MDailyPlanMng();
        private DailyPlanMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        private MDemandMasterPlanMng demandmaster = new MDemandMasterPlanMng();
        CurrentProjectInfo projectInfo;

        private EnumDailyType dailyType;
        /// <summary>
        /// 用来区分专业
        /// </summary>
        public EnumDailyType DailyType
        {
            get { return dailyType; }
            set { dailyType = value; }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public DailyPlanMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VDailyPlanMng()
        {
            InitializeComponent();
            InitEvent();
            InitZYFL();
            InitDate();
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

        private void InitZYFL()
        {
            //添加专业分类下拉框
            VBasicDataOptr.InitProfessionCategory(colSpecailType, false);
            dgDetail.ContextMenuStrip = cmsDg;
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
        }


        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //按钮点击事件
            this.btnSetSpecial.Click += new EventHandler(btnSet_Click);
            this.btnSetDW.Click += new EventHandler(btnSet_Click);
            this.btnSetPart.Click += new EventHandler(btnSet_Click);
            this.btnSetZL.Click += new EventHandler(btnSet_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtMaterialCategory.Leave += new EventHandler(txtMaterialCategory_Leave);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);
            this.flexGrid1.PrintPage += new FlexCell.Grid.PrintPageEventHandler(flexGrid1_PrintPage);
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
                dr.Cells[colMaterialCategory.Name].Value = txtMaterialCategory.Text;
                dr.Cells[colMaterialCategory.Name].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
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

        void btnSet_Click(object sender, EventArgs e)
        {
            //获得第一行专业分类的信息，将其他行的专业分类信息都改成和第一行相同的信息
            //string strSpecialType = null;
            //foreach (DataGridViewRow var in this.dgDetail.Rows)
            //{
            //    if (var.IsNewRow) break;
            //    object spType = dgDetail.Rows[0].Cells[colSpecailType.Name].Value;
            //    var.Cells[colSpecailType.Name].Value = spType;
            //}


            Button oBtn = sender as Button;
            string sValue = string.Empty;
            if (string.Equals(oBtn.Name, "btnSetSpecial"))
            {
                if (dgDetail.Rows.Count > 0 && !this.dgDetail.Rows[0].IsNewRow)
                {
                    sValue = (this.dgDetail.Rows[0].Cells[colSpecailType.Name].Value == null ? string.Empty : this.dgDetail.Rows[0].Cells[colSpecailType.Name].Value.ToString());
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        var.Cells[colSpecailType.Name].Value = sValue;
                    }
                }
            }
            else if (string.Equals(oBtn.Name, "btnSetPart"))
            {
                if (dgDetail.Rows.Count > 0 && !this.dgDetail.Rows[0].IsNewRow)
                {
                    sValue = (this.dgDetail.Rows[0].Cells[colUsedPart.Name].Value == null ? string.Empty : this.dgDetail.Rows[0].Cells[colUsedPart.Name].Value.ToString());
                    object oTag = this.dgDetail.Rows[0].Cells[colUsedPart.Name].Tag;
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        var.Cells[colUsedPart.Name].Value = sValue;
                        var.Cells[colUsedPart.Name].Tag = oTag;
                    }
                }
            }
            else if (string.Equals(oBtn.Name, "btnSetDW"))
            {
                if (dgDetail.Rows.Count > 0 && !this.dgDetail.Rows[0].IsNewRow)
                {
                    sValue = (this.dgDetail.Rows[0].Cells[colUsedRank.Name].Value == null ? string.Empty : this.dgDetail.Rows[0].Cells[colUsedRank.Name].Value.ToString());
                    object oTag = this.dgDetail.Rows[0].Cells[colUsedRank.Name].Tag;
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        var.Cells[colUsedRank.Name].Value = sValue;
                        var.Cells[colUsedRank.Name].Tag = oTag;
                    }
                }
            }
            else if (string.Equals(oBtn.Name, "btnSetZL"))
            {//colQualityStandard
                sValue = (this.dgDetail.Rows[0].Cells[colQualityStandard.Name].Value == null ? string.Empty : this.dgDetail.Rows[0].Cells[colQualityStandard.Name].Value.ToString());

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    var.Cells[colQualityStandard.Name].Value = sValue;

                }
            }
            else
            {
            }
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
            if (DailyType == EnumDailyType.土建)
            {
                colPrice.Visible = false;
                colMoney.Visible = false;
                customLabel1.Visible = false;
                txtSumMoney.Visible = false;
                colSpecailType.Visible = false;
                btnSetSpecial.Visible = false;
               // this.groupSame.Visible = false;
                lblCat.Text = "物资分类";
                txtMaterialCategory.Visible = true;
                cboProfessionCat.Visible = false;
                colSpecailType.Visible = false;

                this.btnSetPart.Visible = true;
                this.btnSetZL.Visible = true;
                this.btnSetSpecial.Visible = false;
                this.btnSetDW.Visible = true ;
            }
            if (DailyType == EnumDailyType.安装)
            {
                lblCat.Text = "专业分类";
              //  this.groupSame.Visible = true ;
                txtMaterialCategory.Visible = false;
                cboProfessionCat.Visible = true;
                colMaterialCategory.Visible = false;
                colSpecailType.Visible = true;
                this.btnSetPart.Visible = true;
                this.btnSetZL.Visible = true;
                this.btnSetSpecial.Visible = true ;
                this.btnSetDW.Visible = true ;
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
                    curBillMaster.Details.Remove(dr.Tag as DailyPlanDetail);
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
                    curBillMaster = model.DailyPlanSrv.GetDailyPlanById(Id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtHandlePerson, txtSumQuantity, txtProject, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name,colSendBackQuantity.Name, colUnit.Name, colMoney.Name, colSpecailType.Name, colMaterialCategory.Name, colUsedPart.Name, colUsedRank.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            if (this.txtMaterialCategory.Visible == true)
            {
                this.txtMaterialCategory.Text = "";
                this.txtMaterialCategory.Result = null;
            }
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
                this.dgDetail.Rows[0].Cells[colQualityStandard.Name].Value="国标";
                this.curBillMaster = new DailyPlanMaster();
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

                curBillMaster.Special = Enum.GetName(typeof(EnumDailyType), DailyType);
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.DailyPlanSrv.GetDailyPlanById(curBillMaster.Id);
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
              
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                //curBillMaster = model.DailyPlanSrv.SaveDailyPlan(curBillMaster);
                curBillMaster = model.DailyPlanSrv.SaveDailyPlan(curBillMaster, movedDtlList);
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

        private void WriteLog(bool flag)
        {
            LogData log = new LogData();
            log.BillId = curBillMaster.Id;
            log.BillType = "日常需求计划单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            if (flag)
            {
                if (DailyType == EnumDailyType.安装)
                {
                    log.OperType = "新增 - 安装";
                }
                if (DailyType == EnumDailyType.土建)
                {
                    log.OperType = "新增 - 土建";
                }
            }
            else
            {
                if (curBillMaster.DocState == DocumentState.InExecute)
                {
                    if (DailyType == EnumDailyType.安装)
                    {
                        log.OperType = "提交 - 安装";
                    }
                    if (DailyType == EnumDailyType.土建)
                    {
                        log.OperType = "提交 - 土建";
                    }
                }
                else
                {
                    if (DailyType == EnumDailyType.安装)
                    {
                        log.OperType = "修改 - 安装";
                    }
                    if (DailyType == EnumDailyType.土建)
                    {
                        log.OperType = "修改 - 土建";
                    }
                }
            }
            StaticMethod.InsertLogData(log);
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
                bool IsNew = (string.IsNullOrEmpty(curBillMaster.Id) ? true : false);
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster = model.DailyPlanSrv.SaveDailyPlan(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                WriteLog(IsNew);
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;

                //#region 短信
                //MAppPlatMng appModel = new MAppPlatMng();
                //appModel.SendMessage(curBillMaster.Id, "DailyPlanMaster");
                //BackResourcePlan(curBillMaster);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                curBillMaster.DocState = DocumentState.Edit;
            }
            return false;
        }

        public void BackResourcePlan(DailyPlanMaster master)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            IList listDaily = model.DailyPlanSrv.ObjectQuery(typeof(DailyPlanDetail), oq);

            foreach (DailyPlanDetail dtl in listDaily)
            {
                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddCriterion(Expression.Eq("MaterialResource.Id", dtl.MaterialResource.Id));
                if (dtl.DiagramNumber == null)
                {
                    oq1.AddCriterion(Expression.IsNull("DiagramNumber"));
                }
                else
                {
                    oq1.AddCriterion(Expression.Eq("DiagramNumber", dtl.DiagramNumber));
                }
                
                oq1.AddCriterion(Expression.Eq("TheGWBSTaskGUID.Id", dtl.UsedPart.Id));
                IList list = model.DailyPlanSrv.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                if (list == null || list.Count == 0) return;
                foreach (ResourceRequirePlanDetail plan in list)
                {
                    plan.DailyPlanPublishQuantity = dtl.Quantity;
                    ResourceRequirePlanDetail ddtl = new ResourceRequirePlanDetail();
                    ddtl = plan;
                    ddtl = model.DailyPlanSrv.SaveOrUpdateByDao(ddtl) as ResourceRequirePlanDetail;
                }
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
                curBillMaster = model.DailyPlanSrv.GetDailyPlanById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.DailyPlanSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "日常需求计划单";
                    log.Code = curBillMaster.Code;
                    if (DailyType == EnumDailyType.安装)
                    {
                        log.OperType = "删除 - 安装";
                    }
                    if (DailyType == EnumDailyType.土建)
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
                        curBillMaster = model.DailyPlanSrv.GetDailyPlanById(curBillMaster.Id);
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
                curBillMaster = model.DailyPlanSrv.GetDailyPlanById(curBillMaster.Id);
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
            if (dailyType == EnumDailyType.土建)
            {
                if (txtMaterialCategory.Text.Equals("") || txtMaterialCategory.Text.Equals(null))
                {
                    MessageBox.Show("物资分类为必选项！");
                    return false;
                }
            }
            if (dailyType == EnumDailyType.安装)
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

                if (ClientUtil.ToString(dr.Cells[this.colQualityStandard.Name].Value) == "")
                {
                    MessageBox.Show("质量标准不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colQualityStandard.Name];
                    return false;
                }
                if (ClientUtil.ToString(dr.Cells[this.colUsedPart.Name].Value) == "")
                {
                    MessageBox.Show("使用部位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUsedPart.Name];
                    return false;
                }
                if (ClientUtil.ToString(dr.Cells[this.colUsedRank.Name].Value) == "")
                {
                    MessageBox.Show("使用队伍不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUsedRank.Name];
                    return false;
                }
                //if (ClientUtil.ToDateTime(dr.Cells[this.colApproachTime.Name].Value) < ConstObject.LoginDate)
                //{
                //    MessageBox.Show("进场时间不能早于[" + ConstObject.LoginDate.ToShortDateString() + "]！");
                //    dgDetail.CurrentCell = dr.Cells[colApproachTime.Name];
                //    return false;
                //}
                if (dr.Cells[colQuantity.Name].Value == null || dr.Cells[colQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colQuantity.Name].Value) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }
                if (ClientUtil.ToDecimal(dr.Cells[colQuantity.Name].Value) < ClientUtil.ToDecimal(dr.Cells[colSupplyQuantity.Name].Value))
                {
                    MessageBox.Show("需求数量应大于等于采购数量！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    dgDetail.CurrentCell = dr.Cells[colSupplyQuantity.Name];
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
               // curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateDate = dtpDateBegin.Value ;
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
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                curBillMaster.PlanType = Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain.ExecuteDemandPlanTypeEnum.物资计划;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    DailyPlanDetail curBillDtl = new DailyPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as DailyPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);//物资名称
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格型号
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colStuff.Name].Value);//材质
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);//计量单位
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colQuantity.Name].Value);//数量
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[colMoney.Name].Value);//金额
                    curBillDtl.ApproachDate = Convert.ToDateTime(var.Cells[colApproachTime.Name].Value);//进场时间
                    curBillDtl.SendBackQuantity = ClientUtil.TransToDecimal(var.Cells[colSendBackQuantity.Name].Value);//累计进厂量
                    curBillDtl.UsedRank = var.Cells[colUsedRank.Name].Tag as SupplierRelationInfo;//使用队伍
                    curBillDtl.SupplyQuantity = ClientUtil.ToDecimal(var.Cells[colSupplyQuantity.Name].Value);
                    curBillDtl.UsedRankName = ClientUtil.ToString(var.Cells[colUsedRank.Name].Value);
                    curBillDtl.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                    curBillDtl.ProjectTaskSysCode = curBillDtl.UsedPart.SysCode;
                    curBillDtl.SpecialType = ClientUtil.ToString(var.Cells[colSpecailType.Name].Value);//专业分类
                    curBillDtl.MaterialCategory = var.Cells[colMaterialCategory.Name].Tag as MaterialCategory;
                    curBillDtl.MaterialCategoryName = ClientUtil.ToString(var.Cells[colMaterialCategory.Name].Value);
                    curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                    curBillDtl.MaterialCategory = var.Cells[colMaterialCategory.Name].Tag as MaterialCategory;//物资分类
                    curBillDtl.MaterialCategoryName = ClientUtil.ToString(var.Cells[colMaterialCategory.Name].Value);
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colQualityStandard.Name].Value);//质量标准
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    curBillDtl.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);//数量
                    //curBillDtl.LeftQuantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);//剩余数量
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);//单价
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注
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
                    this.dgDetail.CurrentRow.Cells[colStuff.Name].Value = selectedMaterial.Stuff;

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

            MaterialCategory cat =null;
            if (this.DailyType == EnumDailyType.安装)
            {
                cat = null;
            }
            else
            {
                cat = this.txtMaterialCategory.Result[0] as MaterialCategory;
                if (cat.Level != 3)
                {
                    MessageBox.Show("请选择一级物资分类！");
                    return;
                }
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
                        this.dgDetail[colStuff.Name, i].Value = theMaterial.Stuff;
                        this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                        if (this.DailyType == EnumDailyType.安装)
                        {
                            this.dgDetail[colSpecailType.Name, i].Value = cboProfessionCat.SelectedItem as string;
                        }
                        if (theMaterial.BasicUnit != null)
                            this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                        //进场时间
                        dgDetail[colApproachTime.Name, i].Value = DateTime.Now;
                        if (txtMaterialCategory.Value != "")
                        {
                            this.dgDetail[colMaterialCategory.Name, i].Value = txtMaterialCategory.Value;
                            this.dgDetail[colMaterialCategory.Name, i].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
                        }

                        string condition = "";
                        condition += " and t1.projectid = '" + projectInfo.Id + "'" + "and t2.material = '" + theMaterial.Id + "'";
                        DataSet dataSet = demandmaster.DemandPlanSrv.Stkstockindtl_RealInQuantity(condition);
                        DataTable dataTable = dataSet.Tables[0];
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            this.dgDetail[colSendBackQuantity.Name, i].Value = dataRow["Quantity"];
                        }

                        i++;
                    }
                }

                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedRank.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VCommonSupplierRelationSelect comSelect = new VCommonSupplierRelationSelect();
                    comSelect.OpenSelectView("", CommonUtil.SupplierCatCode3);
                    IList list = comSelect.Result;
                    if (list != null && list.Count > 0)
                    {
                        SupplierRelationInfo relInfo = list[0] as SupplierRelationInfo;
                        this.dgDetail.CurrentRow.Cells[colUsedRank.Name].Tag = relInfo;
                        this.dgDetail.CurrentRow.Cells[colUsedRank.Name].Value = relInfo.SupplierInfo.Name;
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

                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUnit.Name))
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = su;
                        this.dgDetail.CurrentRow.Cells[colUnit.Name].Value = su.Name;
                        this.txtCode.Focus();
                    }

                    //Dimension ds = UCL.Locate("计量单位量纲选择", StandardUnitExcuteType.SelectDimension) as Dimension;
                    //if (ds != null)
                    //{
                    //    this.dgDetail.CurrentRow.Cells[colUnit.Name].Value = ds.Name;
                    //}
                }
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                dtpDateBegin.Value = curBillMaster.CreateDate;
                this.txtCode.Text = curBillMaster.Code;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtRemark.Text = curBillMaster.Descript;
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
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
                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString("#######.####");
                this.txtSumMoney.Text = curBillMaster.SumMoney.ToString("#########.##");
                txtProject.Text = curBillMaster.ProjectName;
                this.dgDetail.Rows.Clear();
                foreach (DailyPlanDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colStuff.Name, i].Value = var.MaterialStuff;
                    //设置该物料的计量单位
                    this.dgDetail[colSupplyQuantity.Name, i].Value = var.SupplyQuantity;//采购计划总量
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;//计量单位
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;//数量
                    this.dgDetail[colPrice.Name, i].Value = var.Price;//单价
                    this.dgDetail[colMoney.Name, i].Value = var.Money;//金额
                    this.dgDetail[colSpecailType.Name, i].Value = var.SpecialType;
                    this.dgDetail[colMaterialCategory.Name, i].Value = var.MaterialCategoryName;//物资分类
                    this.dgDetail[colQualityStandard.Name, i].Value = var.QualityStandard;//质量标准
                    this.dgDetail[colSpecailType.Name, i].Value = var.SpecialType;//专业分类
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colUsedRank.Name, i].Tag = var.UsedRank;
                    var.UsedPart.SysCode = var.ProjectTaskSysCode;
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;//使用部位                
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;//使用部位
                    this.dgDetail[colUsedRank.Name, i].Value = var.UsedRankName;//使用队伍
                    this.dgDetail[colApproachTime.Name, i].Value = var.ApproachDate;//进场时间
                    this.dgDetail[colSendBackQuantity.Name, i].Value = var.SendBackQuantity;//累计进场数量
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;

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
            if (colName == colPrice.Name || colName == colQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("数量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value = "";
                        flag = false;
                        dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Selected = true;
                        dgDetail.BeginEdit(false);
                        
                    }
                 if(flag )
                     CheckQuantity(dgDetail.Rows[e.RowIndex]);
                   
                }
                
                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("价格为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "";
                        dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Selected = true;
                        dgDetail.BeginEdit(false);
                        flag = false;
                    }
                }
                
                if (flag)
                {
                    //根据单价和数量计算金额  
                    object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;

                    decimal sumqty = 0;
                    decimal money = 0;
                    decimal summoney = 0;

                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                        if (quantity == null) quantity = 0;
                        if (DailyType == EnumDailyType.安装)
                        {
                            if (ClientUtil.ToString(dgDetail.Rows[i].Cells[colQuantity.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[i].Cells[colPrice.Name].Value) != "")
                            {
                                money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colPrice.Name].Value);
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
            //if (colName == colQuantity.Name)
            //{
            //    if (dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value != null)
            //    {
            //        string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value.ToString();
            //        validity = CommonMethod.VeryValid(temp_quantity);
            //        if (validity == false)
            //            MessageBox.Show("请输入数字！");
            //    }

            //    object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
            //    decimal sumqty = 0;
            //    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            //    {
            //        quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
            //        if (quantity == null) quantity = 0;
            //        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
            //    }

            //    txtSumQuantity.Text = sumqty.ToString();
            //}
        }
        public bool CheckQuantity( DataGridViewRow oRow)
        {
            bool bFlag = true ;
            string sSpecialType = string.Empty;
            string sSpecial = string.Empty;
            if (oRow.Cells[colQuantity.Name].Value != null)
            {
                string temp_quantity = oRow.Cells[colQuantity.Name].Value.ToString();
                if (!string.IsNullOrEmpty(temp_quantity.Trim()))
                {
                    Application.Resource.MaterialResource.Domain.Material oMaterial = oRow.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    if (oMaterial != null)
                    {
                        sSpecial = Enum.GetName(typeof(EnumDailyType), this.dailyType);
                        if (this.dailyType == EnumDailyType.安装 && this.cboProfessionCat .Visible )
                        {
                            sSpecialType = "" + this.cboProfessionCat.SelectedItem;
                        }

                        decimal MaxQuantity = model.DailyPlanSrv.DailyPlanQuantity(StaticMethod.GetProjectInfo().Id, oMaterial.Id, sSpecial, sSpecialType);
                        if (MaxQuantity > 0 && MaxQuantity < decimal.Parse(temp_quantity))
                        {
                            MessageBox.Show(string.Format("物资【{0}】总计划剩余数量为【{1}】，你输入的日常需求计划数量应该小于总计划剩余数量", oMaterial.Name, MaxQuantity));
                            bFlag = false;
                            //oRow.Cells[colQuantity.Name].Selected = true;
                            //dgDetail.BeginEdit(false);
                        }

                    }
                    
                }
            }
            return bFlag;

             
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"物资日常需求计划.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            if (curBillMaster.Special == "安装")
            {
                if (LoadTempleteFile(@"日常需求计划_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            return true;
        }
        //打印事件
        void flexGrid1_PrintPage(object Sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false)
            {
                string aa = "";
            }
        }

        public override bool Print()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"物资日常需求计划.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.Print();
                curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
                curBillMaster = model.DailyPlanSrv.SaveDailyPlan(curBillMaster);
            }
            else
            {
                if (LoadTempleteFile(@"日常需求计划_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.Print();
                curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
                curBillMaster = model.DailyPlanSrv.SaveDailyPlan(curBillMaster);
            }
            return true;
        }

        public override bool Export()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"物资日常需求计划.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.ExportToExcel("物资日常需求计划_土建【" + curBillMaster.Code + "】", false, false, true);
            }
            else
            {
                if (LoadTempleteFile(@"日常需求计划_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.ExportToExcel("物资日常需求计划_安装【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(DailyPlanMaster billMaster)
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
            flexGrid1.Cell(3, 2).WrapText = true;
            flexGrid1.Cell(3, 8).Text = billMaster.MaterialCategoryName;
            flexGrid1.Cell(3, 8).WrapText = true;
            flexGrid1.Cell(3, 6).Text = billMaster.Compilation;
            flexGrid1.Cell(3, 6).WrapText = true;

            //填写明细数据
            decimal sumQuantity = 0;
            for (int i = 0; i < detailCount; i++)
            {

                DailyPlanDetail detail = (DailyPlanDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
                //使用队伍
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.UsedRankName);
                flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
                //数量
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                sumQuantity += detail.Quantity;
                //质量标准
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.QualityStandard);
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                //使用部位
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.UsedPartName);
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                //进场时间
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.ApproachDate != null ? detail.ApproachDate.Value.ToShortDateString() : "";
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Alignment = FlexCell.AlignmentEnum.CenterCenter ;
                }
                //条形码
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
                this.flexGrid1.Cell(1, 7).Text = billMaster.Code.Substring(billMaster.Code.Length - 11)+"-"+a;
                this.flexGrid1.Cell(1, 7).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 7).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            flexGrid1.Cell(6 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(6 + detailCount, 7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(6 + detailCount, 9).Text = billMaster.CreatePersonName;        

            this.flexGrid1.FrozenBottomRows = 2;
           //审批信息打印
            int maxRow = detailStartRowNumber + detailCount + 3;            
            CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id,maxRow);
        }
        private void FillFlex1(DailyPlanMaster billMaster)
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
            flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(3, 2).WrapText = true;
            flexGrid1.Cell(3, 7).Text = billMaster.Code;
            flexGrid1.Cell(3, 7).WrapText = true;
            flexGrid1.Cell(4, 2).Text = "";
            flexGrid1.Cell(4, 2).WrapText = true;
            flexGrid1.Cell(4, 5).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 5).WrapText = true;
            flexGrid1.Cell(4, 8).Text = billMaster.SpecialType;
            flexGrid1.Cell(4, 8).WrapText = true;
            flexGrid1.Row(3).AutoFit();
            flexGrid1.Row(4).AutoFit();
            //填写明细数据
            decimal sumQuantity = 0;
            for (int i = 0; i < detailCount; i++)
            {
                DailyPlanDetail detail = (DailyPlanDetail)billMaster.Details.ElementAt(i);
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = ClientUtil.ToString(i+1);
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
                sumQuantity += detail.Quantity;
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.QualityStandard;
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.ApproachDate != null ? detail.ApproachDate.Value.ToShortDateString() : "";
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                }
                //条形码
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
                this.flexGrid1.Cell(1, 7).Text = billMaster.Code.Substring(billMaster.Code.Length - 11)+ "-" +a;
                this.flexGrid1.Cell(1, 7).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 7).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            flexGrid1.FrozenBottomRows = 2;
            //审批信息打印
            int maxRow = detailStartRowNumber + detailCount+2;
            CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id, maxRow);
        }
    }
}
