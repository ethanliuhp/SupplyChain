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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng
{
    public partial class VDemandMasterPlanMng : TMasterDetailView
    {
        private MDemandMasterPlanMng model = new MDemandMasterPlanMng();
        private DemandMasterPlanMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空

        private EnumDemandType demandType;
        /// <summary>
        /// 用来区分专业
        /// </summary>
        public EnumDemandType DemandType
        {
            get { return demandType; }
            set
            {
                demandType = value;
                bool Flag = (EnumDemandType.安装 == demandType);
                colTelchPara.Visible = Flag;
            }
        }


        /// <summary>
        /// 当前单据
        /// </summary>
        public DemandMasterPlanMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VDemandMasterPlanMng()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitZYFL();
        }

        private void InitZYFL()
        {
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            //添加专业分类下拉框
            VBasicDataOptr.InitProfessionCategory(colzyfl, false);
            dgDetail.ContextMenuStrip = cmsDg;
        }

        private void InitData()
        {
            //添加编制依据下拉框信息
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
            VBasicDataOptr.InitDemandCompilation(comCompilation, false);
            comCompilation.ContextMenuStrip = cmsDg;

        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //按钮事件
            this.btnSet.Click += new EventHandler(btnSet_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtMaterialCategory.Leave += new EventHandler(txtMaterialCategory_Leave);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);
            btnSetQuality.Click +=new EventHandler(btnSetQuality_Click);
        }

        void btnSetQuality_Click(object sender,EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (dgDetail[colzlbz.Name, 0].Value != "")
                {
                    var.Cells[colzlbz.Name].Value = dgDetail[colzlbz.Name, 0].Value;
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
                    dr.Cells[colzyfl.Name].Value = pc;
                }
            }
        }

        void txtMaterialCategory_Leave(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if (dr.IsNewRow) break;
                dr.Cells[colwzfl.Name].Value = txtMaterialCategory.Text;
                dr.Cells[colzyfl.Name].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
            }
            //}
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

        public void btnSet_Click(object sender, EventArgs e)
        {
            //获得第一行专业分类的信息，将其他行的专业分类信息都改成和第一行相同的信息
            string strSpecialType = null;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                strSpecialType = this.dgDetail.Rows[0].Cells[9].Value.ToString();
                var.Cells[colzyfl.Name].Value = strSpecialType;

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
            if (DemandType == EnumDemandType.土建)
            {
                colSumMoney.Visible = false;
                colPrice.Visible = false;
                customLabel2.Visible = false;
                txtSumMoney.Visible = false;
                colzyfl.Visible = false;
                btnSet.Visible = false;

                lblCat.Text = "物资分类";
                txtMaterialCategory.Visible = true;
                cboProfessionCat.Visible = false;
            }
            if(demandType == EnumDemandType.安装)
            {
                colwzfl.Visible = false;
                lblCat.Text = "专业分类";
                txtMaterialCategory.Visible = false;
                cboProfessionCat.Visible = true;

                this.colzyfl.Visible = false;
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
                    curBillMaster = model.DemandPlanSrv.GetDemandMasterPlanById(Id);
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
                    this.comCompilation.Enabled = true;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.comCompilation.Enabled = false;
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

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colSumMoney.Name,colwzfl.Name ,colzyfl.Name};
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
                this.curBillMaster = new DemandMasterPlanMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                curBillMaster.Special = Enum.GetName(typeof(EnumDemandType), DemandType);
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
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
                curBillMaster = model.DemandPlanSrv.GetDemandMasterPlanById(curBillMaster.Id);
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
                bool IsNew = (string.IsNullOrEmpty(curBillMaster.Id) ? true : false);
                 
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster = model.DemandPlanSrv.SaveDemandMasterPlan(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;

                //#region 短信
                //MAppPlatMng appModel = new MAppPlatMng();
                //appModel.SendMessage(curBillMaster.Id, "DemandMasterPlanMaster");
                //DataSet ds = AppModel.Service.GetSubmitBillPerson(curBillMaster.Id, "DemandMasterPlanMaster");
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    string sMsg = string.Empty;
                //    foreach (DataRow oRow in ds.Tables[0].Rows)
                //    {
                //        sMsg += "PERNAME:" + ClientUtil.ToString(oRow["PERNAME"]).PadLeft(20, ' ') + "  PERCODE:" + ClientUtil.ToString(oRow["PERCODE"]).PadLeft(20, ' ') + "\n";
                //        MessageBox.Show(sMsg);
                //    }
                //}
                //#endregion
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
                curBillMaster = model.DemandPlanSrv.SaveDemandMasterPlan(curBillMaster, movedDtlList);
                //curBillMaster = model.DemandPlanSrv.SaveDemandMasterPlan(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                //if (DemandType == EnumDemandType.土建)
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
            log.BillType = "需求总计划单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            if (IsNew)
            {
                if (DemandType == EnumDemandType.安装)
                {
                    log.OperType = "新增 - 安装";
                }
                if (DemandType == EnumDemandType.土建)
                {
                    log.OperType = "新增 - 土建";
                }
            }
            else
            {
                if (curBillMaster.DocState == DocumentState.InExecute)
                {
                    if (DemandType == EnumDemandType.安装)
                    {
                        log.OperType = "提交 - 安装";
                    }
                    if (DemandType == EnumDemandType.土建)
                    {
                        log.OperType = "提交 - 土建";
                    }
                }
                else
                {
                    if (DemandType == EnumDemandType.安装)
                    {
                        log.OperType = "修改 - 安装";
                    }
                    if (DemandType == EnumDemandType.土建)
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
                curBillMaster = model.DemandPlanSrv.GetDemandMasterPlanById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    foreach (DemandMasterPlanDetail dtl in curBillMaster.Details)
                    {
                        if (dtl.RefQuantity > 0)
                        {
                            MessageBox.Show("信息被引用，不可删除！");
                            return false;
                        }
                    }
                    if (!model.DemandPlanSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "需求总计划单";
                    log.Code = curBillMaster.Code;
                    if (DemandType == EnumDemandType.安装)
                    {
                        log.OperType = "删除 - 安装";
                    }
                    if (DemandType == EnumDemandType.土建)
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
                        curBillMaster = model.DemandPlanSrv.GetDemandMasterPlanById(curBillMaster.Id);
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
                curBillMaster = model.DemandPlanSrv.GetDemandMasterPlanById(curBillMaster.Id);
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
            if (demandType == EnumDemandType.土建)
            {
                if (txtMaterialCategory.Text.Equals("") || txtMaterialCategory.Text.Equals(null))
                {
                    MessageBox.Show("物资分类为必选项！");
                    return false;
                }
            }
            if (demandType == EnumDemandType.安装)
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

                if (dr.Cells[colQuantity.Name].Value == null || dr.Cells[colQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colQuantity.Name].Value) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                if (ClientUtil.ToString(dr.Cells[this.colzlbz.Name].Value) == "")
                {
                    MessageBox.Show("质量标准不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colzlbz.Name];
                    return false;
                }

                if (DemandType == EnumDemandType.安装)
                {
                    if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                    {
                        MessageBox.Show("单价不允许为空！");
                        dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                        return false;
                    }
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
                curBillMaster.Compilation = ClientUtil.ToString(comCompilation.SelectedItem);
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                curBillMaster.CreateDate = dtpDateBegin.Value;
                //curBillMaster.SpecialType = ClientUtil.ToString(this.cboProfessionCat.SelectedItem);
                //物资分类
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MaterialCategoryName = txtMaterialCategory.Text;
                }

                curBillMaster.SpecialType = cboProfessionCat.SelectedItem == null ? null : cboProfessionCat.SelectedItem.ToString();
                curBillMaster.PlanType = Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain.ExecuteDemandPlanTypeEnum.物资计划;
                curBillMaster.Details.Clear();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    DemandMasterPlanDetail curBillDtl = new DemandMasterPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as DemandMasterPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);//物资名称
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格型号
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colStuff.Name].Value);//材质
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);//计量单位
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colQuantity.Name].Value);//数量
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[colSumMoney.Name].Value);//金额
                    curBillDtl.SpecialType = ClientUtil.ToString(var.Cells[colzyfl.Name].Value);//专业分类
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    curBillDtl.MaterialCategory = var.Cells[colzyfl.Name].Tag as MaterialCategory;
                    curBillDtl.MaterialCategoryName = ClientUtil.ToString(var.Cells[colwzfl.Name].Value);//物资分类
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colzlbz.Name].Value);//质量标准
                    curBillDtl.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);//数量
                    curBillDtl.SupplyLeftQuantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);//总需求可用数量
                    curBillDtl.DemandLeftQuantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);//总需求可用数量
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);//单价
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注
                    if (this.DemandType == EnumDemandType.安装)
                    {
                        curBillDtl.TechnologyParameter = ClientUtil.ToString(var.Cells[colTelchPara.Name].Value);
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

            if (this.txtMaterialCategory.Visible == true)
            {
                MaterialCategory cat = this.txtMaterialCategory.Result[0] as MaterialCategory;
                if (cat.Level != 3)
                {
                    MessageBox.Show("请选择一级物资分类！");
                    return;
                }
            }
            
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
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
                        if (theMaterial.BasicUnit != null)
                            this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                        if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0 )
                        {
                            dgDetail[colwzfl.Name,i].Value = txtMaterialCategory.Value;
                            dgDetail[colzyfl.Name, i].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
                        }
                        i++;
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
                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString();
                //this.cboProfessionCat.SelectedText = curBillMaster.SpecialType;
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
                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString();
                txtSumMoney.Text = curBillMaster.SumMoney.ToString();
                txtProject.Text = curBillMaster.ProjectName.ToString();
                comCompilation.SelectedItem = ClientUtil.ToString(curBillMaster.Compilation);
                this.dgDetail.Rows.Clear();
                foreach (DemandMasterPlanDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                    this.dgDetail[colStuff.Name, i].Value = var.MaterialResource.Stuff;
                    //设置该物料的计量单位
                    if (var.MatStandardUnit != null)
                    {
                        this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                        this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnit.Name;
                    }
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colSumMoney.Name, i].Value = var.Money;
                    this.dgDetail[colzyfl.Name, i].Value = var.SpecialType;
                    this.dgDetail[colzlbz.Name, i].Value = var.QualityStandard;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;
                    this.dgDetail[colwzfl.Name, i].Value = curBillMaster.MaterialCategoryName;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[colTelchPara.Name, i].Value = var.TechnologyParameter;
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
                        //dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name];
                        flag = false;
                    }
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("价格为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "";
                            //dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name];
                        flag = false;
                    }
                }
                if (flag)
                {
                    //根据单价和数量计算金额  
                    decimal sumqty = 0;
                    decimal money = 0;
                    decimal summoney = 0;
                    object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                        if (quantity == null) quantity = 0;

                        if (DemandType == EnumDemandType.安装)
                        {
                            if (ClientUtil.ToString(dgDetail.Rows[i].Cells[colQuantity.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[i].Cells[colPrice.Name].Value) != "")
                            {
                                money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colPrice.Name].Value);
                                dgDetail.Rows[i].Cells[colSumMoney.Name].Value = ClientUtil.ToString(money);
                                summoney = summoney + money;
                            }
                        }
                        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                    }
                    txtSumMoney.Text = summoney.ToString();
                    txtSumQuantity.Text = sumqty.ToString();
                }
                
            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
         
        }

        public override bool Print()
        {

            if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            curBillMaster = model.DemandPlanSrv.SaveDemandMasterPlan(curBillMaster);           
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("物资需求总计划【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(DemandMasterPlanMaster billMaster)
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
            flexGrid1.Cell(3, 1).Text = "单据编号：" + billMaster.Code;
            flexGrid1.Cell(3, 5).Text = billMaster.MaterialCategoryName;
            flexGrid1.Cell(3, 6).Text = "编制依据：" + billMaster.Compilation;

            //填写明细数据
            decimal sumQuantity = 0;
            for (int i = 0; i < detailCount; i++)
            {
                DemandMasterPlanDetail detail = (DemandMasterPlanDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                //需用计划
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
                sumQuantity += detail.Quantity;
                //质量标准
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.QualityStandard);
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).WrapText = true;
                }
                //条形码
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
                this.flexGrid1.Cell(1, 5).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
                this.flexGrid1.Cell(1, 5).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 5).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            int maxRow = detailStartRowNumber + detailCount+4;
            CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id, maxRow);
            flexGrid1.Cell(6 + detailCount, 1).Text = "项目名称：" + billMaster.ProjectName;
            flexGrid1.Cell(6 + detailCount, 5).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(6 + detailCount, 6).Text = "制单人："+billMaster.CreatePersonName;
            //flexGrid1.Cell(6 + detailCount, 6).Text = billMaster.CreatePersonName;
        }
    }
}
