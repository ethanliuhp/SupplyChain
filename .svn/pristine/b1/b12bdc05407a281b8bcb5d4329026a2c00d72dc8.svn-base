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
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI
{
    public partial class VStockMoveIn : TMasterDetailView
    {
        private MStockMng model = new MStockMng();
        private StockMoveIn curBillMaster;
        private EnumStockExecType execType;
        public EnumStockExecType ExecType
        {
            get { return execType; }
            set { execType = value; }
        }
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public StockMoveIn CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VStockMoveIn()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            dgDetail.ContextMenuStrip = cmsDg;

            //专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, true);

            //物资档次
            //VBasicDataOptr.InitMaterialGrade(colMaterialGrade, true);
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            flexGrid1.PrintPage += new FlexCell.Grid.PrintPageEventHandler(btnPrintPage_Click);
            this.btnDaily.Click +=new EventHandler(btnDaily_Click);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);
            this.btnSearch.Click +=new EventHandler(btnSearch_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void btnDaily_Click(object sender, EventArgs e)
        {
            //string stMmaterial = null;
            //if (txtMaterialCategory.Text == "")
            //{
            //    MessageBox.Show("请先选择物资类型！");
            //    return;
            //}
            //else
            //{
            //    //获得物资类型编码
            //    stMmaterial = ClientUtil.ToString(txtMaterialCategory.Value);
            //}
            //decimal sumqty = 0;
            VDailyPlanSelector theVDailyPlanSelector = null;
            if (this.ExecType == EnumStockExecType.安装)
            {
                  theVDailyPlanSelector = new VDailyPlanSelector(EnumSupplyType.安装  );
            }
            else if (this.ExecType == EnumStockExecType.土建)
            {
                  theVDailyPlanSelector = new VDailyPlanSelector(EnumSupplyType.土建);
            }
            else
            {
                  theVDailyPlanSelector = new VDailyPlanSelector();
            }
            theVDailyPlanSelector.selectType = 2;
            theVDailyPlanSelector.ShowDialog();
            IList list = theVDailyPlanSelector.Result;
            if (list.Count > 0)
            {
                DailyPlanMaster dailyMaster = list[0] as DailyPlanMaster;
                txtDaily.Text = dailyMaster.Code;
                txtDaily.Tag = dailyMaster.Id;
                
                if (this.txtMaterialCategory.Visible == true)
                {
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Text = dailyMaster.MaterialCategoryName;
                    txtMaterialCategory.Result.Add(dailyMaster.MaterialCategory);
                }
                foreach (DailyPlanDetail dailyDetail in dailyMaster.Details)
                {
                    if (dailyDetail.IsSelect == true)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialCode.Name, i].Tag = dailyDetail.MaterialResource;
                        this.dgDetail[colMaterialCode.Name, i].Value = dailyDetail.MaterialCode;
                        this.dgDetail[colMaterialName.Name, i].Value = dailyDetail.MaterialName;
                        this.dgDetail[colForword.Name, i].Value = dailyDetail.Id;
                        this.dgDetail[this.colQuantity.Name, i].Value = dailyDetail.Quantity - dailyDetail.RefQuantity;
                        this.dgDetail[colMaterialSpec.Name, i].Value = dailyDetail.MaterialSpec;
                        this.dgDetail[colMaterialStuff.Name, i].Value = dailyDetail.MaterialStuff;
                        this.dgDetail[colUnit.Name, i].Tag = dailyDetail.MatStandardUnit;
                        this.dgDetail[colUnit.Name, i].Value = dailyDetail.MatStandardUnitName;
                        this.dgDetail[this.colDiagramNum.Name, i].Value = dailyDetail.DiagramNumber;
                    }
                }
            }
        }

        void btnSearch_Click(object sender,EventArgs e)
        {
            string strdepart = "";
            if (txtMoveOutProject.Text != "")
            {
                strdepart = ClientUtil.ToString(txtMoveOutProject.Text);
            }
            VDepartSelector vmros = new VDepartSelector(strdepart);
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;      
            //string strName = ClientUtil.ToString(list[0]);
            //DemandMasterPlanMaster supplyMaster = list[0] as DemandMasterPlanMaster;
            //txtMoveOutProject.Tag = supplyMaster.Id;
            CurrentProjectInfo cpi = list[0] as CurrentProjectInfo;
            txtMoveOutProject.Text = cpi.Name;
            //txtMoveOutProject.Tag = cpi.Id;
        }

        void cboProfessionCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            object pc = cboProfessionCat.SelectedItem;
            if (pc != null)
            {
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    if (dr.IsNewRow) continue;
                    dr.Cells[colProfessionalCategory.Name].Value = pc.ToString();
                }
            }
        }

        private void ReadOnlyCat(bool enabled)
        {
            txtMaterialCategory.ReadOnly = enabled;
            cboProfessionCat.ReadOnly = enabled;
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
            if (execType == EnumStockExecType.土建)
            {
                colConfirmPrice.Visible = false;
                colConfirmMoney.Visible = false;
                colProfessionalCategory.Visible = false;
                customLabel9.Visible = false;
                txtSumConfirmMoney.Visible = false;

                lblCat.Text = "物资分类:";
                txtMaterialCategory.Visible = true;
                cboProfessionCat.Visible = false;
            }
            if (execType == EnumStockExecType.安装)
            {
                colConfirmPrice.Visible = false;
                colConfirmMoney.Visible = false;
                customLabel9.Visible = false;
                txtSumConfirmMoney.Visible = false;

                lblCat.Text = "专业分类:";
                txtMaterialCategory.Visible = false;
                cboProfessionCat.Visible = true;
                colProfessionalCategory.Visible = false;
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
                    curBillMaster.Details.Remove(dr.Tag as BaseDetail);
                }
            }
        }

        //void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        //{
        //    curBillMaster.Details.Remove(e.Row.Tag as BaseDetail);
        //}

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
                    curBillMaster = model.StockMoveSrv.GetStockMoveInByCode(code, Enum.GetName(typeof(EnumStockExecType), execType), StaticMethod.GetProjectInfo().Id);
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
        public void Start(string Id, string code)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.StockMoveSrv.GetStockMoveInById(Id);
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
                    this.btnSearch.Enabled = true;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    this.btnSearch.Enabled = false;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtSumConfirmMoney, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialStuff.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colMoney.Name, colConfirmMoney.Name, colProfessionalCategory.Name };
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
            this.txtMaterialCategory.Result.Clear();
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
                movedDtlList = new ArrayList();
                this.curBillMaster = new StockMoveIn();
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

                curBillMaster.StockInManner = EnumStockInOutManner.调拨入库;
                curBillMaster.Special = Enum.GetName(typeof(EnumStockExecType), execType);

                //仓库
                curBillMaster.TheStationCategory = StaticMethod.GetStationCategory();

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                //txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
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
            movedDtlList = new ArrayList();
            if (curBillMaster.IsTally == 1)
            {
                MessageBox.Show("此单己记账，不能修改！");
                return false;
            }
            base.ModifyView();
            curBillMaster = model.StockMoveSrv.GetStockMoveInById(curBillMaster.Id);
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
                    curBillMaster = model.SaveStockMoveIn(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "调拨入库单", "", curBillMaster.ProjectName);
                }
                else
                {
                    curBillMaster = model.SaveStockMoveIn(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "调拨入库单", "", curBillMaster.ProjectName);
                }

                //插入日志
                //StaticMethod.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, curBillMaster.CreatePerson.Name, "收料单","");
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;

                try
                {
                    Tally();
                    if (curBillMaster.Special == "安装")
                    {
                        foreach (StockMoveInDtl dtl in curBillMaster.Details)
                        {
                            BackResourcePlan(dtl);
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("记账出错。"+ex.Message);
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        public void BackResourcePlan(StockMoveInDtl dtl)
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

                //oq1.AddCriterion(Expression.Like("TheGWBSTaskGUID.Id", dtl.UsedPart.Id));
                IList list = model.StockInSrv.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                if (list == null || list.Count == 0) return;
                foreach (ResourceRequirePlanDetail plan in list)
                {
                    plan.ExecutedQuantity += dtl.Quantity;
                }
                model.StockInSrv.SaveOrUpdateByDao(list);
            }
        }

        private void Tally()
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
                        MessageBox.Show(errMsg);
                    } else
                    {
                        curBillMaster.IsTally = 1;
                        MessageBox.Show("记账成功。");
                    }
                }
            //}
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.StockMoveSrv.GetStockMoveInById(curBillMaster.Id);
                if (curBillMaster.IsTally == 0)
                {
                    if (!model.StockMoveSrv.DeleteStockMoveIn(curBillMaster)) return false;
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "调拨入库单", "", curBillMaster.ProjectName);
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
                        curBillMaster = model.StockMoveSrv.GetStockMoveInById(curBillMaster.Id);
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
                curBillMaster = model.StockMoveSrv.GetStockMoveInById(curBillMaster.Id);
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
            if (txtMoveOutProject.Text.Trim() == "")
            {
                MessageBox.Show("调出单位不能为空！");
                txtMoveOutProject.Focus();
                return false;
            }

            if (ExecType == EnumStockExecType.安装)
            {
                if (cboProfessionCat.SelectedItem == null || string.IsNullOrEmpty ( cboProfessionCat.SelectedItem.ToString ()))
                {
                    MessageBox.Show("请选择专业分类");
                    return false;
                }
            }
            else if (ExecType == EnumStockExecType.土建)
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
           // MStockMng model = new MStockMng();
            DateTime oDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(dtpDateBegin.Value).ToShortDateString());
            if (oDate > ConstObject.TheLogin.FiscalModule.EndDate)
            {
                MessageBox.Show("当前的业务日期[" + oDate.ToShortDateString() + "]必须小于本会计期的最后日期[" + ConstObject.TheLogin.FiscalModule.EndDate.ToShortDateString() + "]!");
                return false;
            }
            string sMsg = model.StockInSrv.CheckAccounted(ConstObject.TheLogin.TheAccountOrgInfo, ClientUtil.ToDateTime(this.dtpDateBegin.Text), StaticMethod.GetProjectInfo().Id);
            if (sMsg != string.Empty)
            {
                MessageBox.Show(sMsg);
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            IList materialList = new ArrayList();//校验业务日期
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
                else
                {
                    Material material = dr.Cells[colMaterialCode.Name].Tag as Material;
                    materialList.Add(material.Id);
                }

                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }

                if (dr.Cells[colQuantity.Name].Value == null || dr.Cells[colQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colQuantity.Name].Value) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }
                if (ClientUtil.ToString(dr.Cells[colMaterialGrade.Name].Value) == "")
                {
                    MessageBox.Show("物资成色不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialGrade.Name];
                    return false;
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
            if (ExecType == EnumStockExecType.土建)
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

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.MaterialProvider = chkMaterialProvider.Checked ? 1 : 0;
                curBillMaster.CreateDate  = dtpDateBegin.Value.Date;

                DataTable oTable = model.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }

                curBillMaster.Descript = this.txtRemark.Text;
                curBillMaster.MoveOutProjectName = txtMoveOutProject.Text;
                curBillMaster.MoveOutProjectId = txtMoveOutProject.Tag + "";
                curBillMaster.ForwardBillCode = txtDaily.Text;
                curBillMaster.ForwardBillId = ClientUtil.ToString(txtDaily.Tag);
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
                    StockMoveInDtl curBillDtl = new StockMoveInDtl();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as StockMoveInDtl;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    Application.Resource.MaterialResource.Domain.Material material = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialResource = material;
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colMaterialStuff.Name].Value);
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colQuantity.Name].Value);
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[colMoney.Name].Value);
                    curBillDtl.ConfirmPrice = ClientUtil.TransToDecimal(var.Cells[colConfirmPrice.Name].Value);
                    curBillDtl.ConfirmMoney = ClientUtil.TransToDecimal(var.Cells[colConfirmMoney.Name].Value);
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    curBillDtl.ForwardDetailId = ClientUtil.ToString(var.Cells[colForword.Name].Value);
                    curBillDtl.ProfessionalCategory = curBillMaster.ProfessionCategory;

                    if (var.Cells[colMaterialGrade.Name].Value != null)
                    {
                        curBillDtl.MaterialGrade = var.Cells[colMaterialGrade.Name].Value.ToString();
                    }
                    else
                    {
                        curBillDtl.MaterialGrade = null;
                    }

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
            //if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //if (this.txtMaterialCategory.Result == null || this.txtMaterialCategory.Result.Count == 0)
            //{
            //    MessageBox.Show("请先选择物资分类！");
            //    return;
            //}
            //if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            //{
            //    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            //    CommonMaterial materialSelector = new CommonMaterial();
            //    if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            //    {
            //        materialSelector.materialCatCode = (txtMaterialCategory.Result[0] as MaterialCategory).Code;
            //    }
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
            //        this.dgDetail[colMaterialStuff.Name, i].Value = theMaterial.Stuff;
            //        this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
            //        if (theMaterial.BasicUnit != null)
            //            this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
            //        dgDetail[colPrice.Name, i].Value = 0;
            //        dgDetail[colQuantity.Name, i].Value = 0;
            //        dgDetail[colConfirmPrice.Name, i].Value = 0;

            //        if (cboProfessionCat.SelectedItem != null)
            //        {
            //            dgDetail[colProfessionalCategory.Name, i].Value = cboProfessionCat.SelectedItem.ToString();
            //        }
            //    }
            //}
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                dtpDateBegin.Value = curBillMaster.CreateDate ;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtMoveOutProject.Text = curBillMaster.MoveOutProjectName;
                txtMoveOutProject.Tag = curBillMaster.MoveOutProjectId;

                if (curBillMaster.MaterialProvider == 1)
                {
                    chkMaterialProvider.Checked = true;
                }
                else
                {
                    chkMaterialProvider.Checked = false;
                }

                txtRemark.Text = curBillMaster.Descript;
               // txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();


                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;

                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString();
                txtSumMoney.Text = curBillMaster.SumMoney.ToString();
                txtSumConfirmMoney.Text = curBillMaster.SumConfirmMoney.ToString();
                txtDaily.Text = curBillMaster.ForwardBillCode;
                txtDaily.Tag = curBillMaster.ForwardBillId;
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
                foreach (StockMoveInDtl var in curBillMaster.Details)
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
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    var.TempData = ClientUtil.ToString(var.Quantity);
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    dgDetail[colMoney.Name, i].Value = var.Money;
                    dgDetail[colConfirmPrice.Name, i].Value = var.ConfirmPrice;
                    dgDetail[colConfirmMoney.Name, i].Value = var.ConfirmMoney;
                    dgDetail[colForword.Name, i].Value = var.ForwardDetailId;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    dgDetail[colProfessionalCategory.Name, i].Value = var.ProfessionalCategory;
                    dgDetail[colMaterialGrade.Name, i].Value = var.MaterialGrade;
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
                    decimal money = decimal.Multiply(ClientUtil.TransToDecimal(quantity), ClientUtil.TransToDecimal(price));
                    dgDetail.Rows[i].Cells[colMoney.Name].Value = money.ToString("#######0.##");
                    sumMoney += money;

                    confirmPrice = dgDetail.Rows[i].Cells[colConfirmPrice.Name].Value;
                    if (confirmPrice == null) confirmPrice = 0;
                    decimal comfirmMoney = ClientUtil.TransToDecimal(quantity) * ClientUtil.TransToDecimal(confirmPrice);
                    dgDetail.Rows[i].Cells[colConfirmMoney.Name].Value = comfirmMoney.ToString("#######0.##");
                    sumConfirmMoney += comfirmMoney;
                }

                txtSumQuantity.Text = sumqty.ToString();
                txtSumConfirmMoney.Text = sumConfirmMoney.ToString("####.##");
                txtSumMoney.Text = sumMoney.ToString("####.##");
            }
        }

        private string flexFileName = @"调拨入库单.flx";
        private string flexFileName1 = @"调拨入库单_安装.flx";

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(flexFileName) == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else
            {
                if (LoadTempleteFile(flexFileName1) == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            return true;
        }

        public override bool Print()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(flexFileName) == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.Print();
            }
            else
            {
                if (LoadTempleteFile(flexFileName1) == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.Print();
            }
            return true;
        }
        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                model.StockInSrv.UpdateBillPrintTimes(2, curBillMaster.Id);//回写次数
                //写打印日志
                StaticMethod.InsertLogData(curBillMaster.Id, "打印", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "调拨入库单", "", curBillMaster.ProjectName);
            }
        }
        public override bool Export()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(flexFileName) == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.ExportToExcel("调拨入库单【" + curBillMaster.Code + "】", false, false, true);
            }
            else
            {
                if (LoadTempleteFile(flexFileName1) == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.ExportToExcel("调拨入库单【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(StockMoveIn billMaster)
        {
            int detailStartRowNumber = 6;//5为模板中的行号
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
            flexGrid1.Cell(2, 1).Text = "调 拨 入 库 单";
            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 6).Text  = billMaster.MoveOutProjectName;
            flexGrid1.Cell(4, 2).Text = billMaster.MatCatName;
            flexGrid1.Cell(4, 6).Text = billMaster.CreateDate.ToShortDateString();
            //填写明细数据
            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            for (int i = 0; i < detailCount; i++)
            {
                StockMoveInDtl detail = (StockMoveInDtl)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                //调拨数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
                sumQuantity += detail.Quantity;
                //成色
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.MaterialGrade);
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                //单价
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Price);
                //总值
                if(detail.Quantity.Equals("") || detail.Price.Equals(""))
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 7).Text = "0";
                }
                else
                {
                    decimal Money = detail.Quantity * detail.Price;
                    flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(decimal.Round ( Money,2));
                    sumMoney += Money;
                }

                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 7).Text = ClientUtil.ToString(decimal.Round( sumMoney,2));
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 2, 2).Text = ClientUtil.ToString(Moneybig);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumMoney));
            this.flexGrid1.Cell(1, 7).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 7).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 7).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            flexGrid1.Cell(8 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(8 + detailCount, 6).Text = billMaster.RealOperationDate.ToShortDateString ();
            flexGrid1.Cell(8 + detailCount, 8).Text = billMaster.CreatePersonName;

            this.flexGrid1.Cell(2, 7).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(billMaster.PrintTimes + 1);
            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.RightFooter = " 打印时间:[" + CommonMethod.GetServerDateTime() + " ],打印人:[" + ConstObject.LoginPersonInfo.Name + "]   " + "第&P页/共&N页  ";

        }

        private void FillFlex1(StockMoveIn billMaster)
        {
            int detailStartRowNumber = 6;//5为模板中的行号
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
            flexGrid1.Cell(3, 2).Text = billMaster.MoveOutProjectName;//调出项目
            flexGrid1.Cell(3, 2).WrapText = true;
            flexGrid1.Cell(3, 7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 2).Text = billMaster.ProjectName;//调出项目
            flexGrid1.Cell(4, 7).Text = billMaster.Code;

            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

            //填写明细数据
            decimal sumMoney = 0;
            for (int i = 0; i < detailCount; i++)
            {
                StockMoveInDtl detail = (StockMoveInDtl)billMaster.Details.ElementAt(i);
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
                //flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.Money);
                //总值
                if (detail.Quantity.Equals("") || detail.Price.Equals(""))
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 8).Text = "0";
                }
                else
                {
                    decimal Money = detail.Quantity * detail.Price;
                    flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(decimal.Round(Money, 2));
                    sumMoney += Money;
                }

                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 2).Text = ClientUtil.ToString(decimal.Round(sumMoney, 2));
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 7).Text = ClientUtil.ToString(Moneybig);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }

            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumMoney));
            this.flexGrid1.Cell(1, 8).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 8).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 8).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;

            this.flexGrid1.Cell(2, 8).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(billMaster.PrintTimes + 1);
            pageSetup.RightFooter = " 打印时间:[" + CommonMethod.GetServerDateTime() + " ],打印人:[" + ConstObject.LoginPersonInfo.Name + "]   " + "第&P页/共&N页  ";

            
        }
    }
}
