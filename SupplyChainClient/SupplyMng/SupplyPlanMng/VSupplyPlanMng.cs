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
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng
{
    public partial class VSupplyPlanMng : TMasterDetailView
    {
        private MSupplyPlanMng model = new MSupplyPlanMng();
        private MDemandMasterPlanMng Demandmodle = new MDemandMasterPlanMng();
        private SupplyPlanMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        EnumSupplyType execelType;
        /// <summary>
        /// 
        /// </summary>
        public EnumSupplyType ExecType
        {
            get { return execelType; }
            set { execelType = value; }
        }

        
        public SupplyPlanMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VSupplyPlanMng()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
            InitSpecailType();
        }
        public VSupplyPlanMng(EnumSupplyType execType)
        {
            this.ExecType = execType;
            InitializeComponent();
            InitEvent();
            InitDate();
            InitSpecailType();
        }

        private void InitSpecailType()
        {
            //添加专业分类下拉框
            VBasicDataOptr.InitProfessionCategory(colSpecailType, false);
            dgDetail.ContextMenuStrip = cmsDg;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate;
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
        }

        public void InitDate()
        {
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
            this.btnSetSpecial.Click += new EventHandler(btnSet_Click);
            this.btnSetBZ.Click += new EventHandler(btnSet_Click);
            this.btnSetPart.Click += new EventHandler(btnSet_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
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
          
            Button oBtn=sender as Button ;
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
            else if(string.Equals(oBtn.Name, "btnSetPart"))
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
            else if (string.Equals(oBtn.Name, "btnSetBZ"))
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

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as SupplyPlanDetail);
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
                    curBillMaster = model.SupplyPlanSrv.GetSupplyPlanById(Id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtApplyCode, txtSummoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name,colQuantity.Name,colMoney.Name,colSpecailType.Name};
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

                this.curBillMaster = new SupplyPlanMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
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
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
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
                curBillMaster = model.SupplyPlanSrv.GetSupplyPlanById(curBillMaster.Id);
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
                curBillMaster = model.SupplyPlanSrv.SaveSupplyPlan(curBillMaster, movedDtlList);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "采购计划单";
                log.Code = curBillMaster.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                if (flag)
                {
                    log.OperType = "新增";
                }
                else
                {
                    log.OperType = "修改";
                }
                StaticMethod.InsertLogData(log);
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
                curBillMaster = model.SupplyPlanSrv.GetSupplyPlanById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    foreach (SupplyPlanDetail dtl in curBillMaster.Details)
                    {
                        if (dtl.RefQuantity > 0)
                        {
                            MessageBox.Show("信息被引用，不可删除！");
                            return false;
                        }
                    }
                    if (!model.SupplyPlanSrv.DeleteSupplyPlan(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "采购计划单";
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
                        curBillMaster = model.SupplyPlanSrv.GetSupplyPlanById(curBillMaster.Id);
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
                curBillMaster = model.SupplyPlanSrv.GetSupplyPlanById(curBillMaster.Id);
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
            if (this.cboProfessionCat.SelectedItem != null)
            { }
            else
            {
                MessageBox.Show("专业分类为必选项！");
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
                if (dr.Cells[colNeedQuantity.Name].Value == null)
                {
                    MessageBox.Show("计划数量不能为空！");
                    return false;
                }

                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                DemandMasterPlanDetail forwardDetail = Demandmodle.DemandPlanSrv.GetDemandDetailById(forwardDtlId.ToString());
                if (forwardDetail == null)
                {
                    MessageBox.Show("未找到前续出库单明细,请重新引用。");
                    dgDetail.CurrentCell = dr.Cells[colQuantityTemp.Name];
                    return false;
                }
                else
                {
                    decimal canUseQty = forwardDetail.Quantity - forwardDetail.DemandLeftQuantity;//可利用数量
                    decimal currentQty = decimal.Parse(dr.Cells[colNeedQuantity.Name].Value.ToString());//所需数量
                    object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;//临时数量
                    decimal qtyTemp = 0;
                    if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                    {
                        qtyTemp = decimal.Parse(qtyTempObj.ToString());
                    }

                    if (currentQty - qtyTemp - canUseQty > 0)
                    {
                        MessageBox.Show("计划数量【" + currentQty + "】不可大于需求数量【" + (canUseQty + qtyTemp) + "】。");
                        dgDetail.CurrentCell = dr.Cells[colNeedQuantity.Name];
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
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                curBillMaster.ForwardBillCode = ClientUtil.ToString(this.txtApplyCode.Text);
                curBillMaster.SpecialType = cboProfessionCat.SelectedItem == null ? null : cboProfessionCat.SelectedItem.ToString();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    SupplyPlanDetail curBillDtl = new SupplyPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as SupplyPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    curBillDtl.LeftQuantity = ClientUtil.TransToDecimal(var.Cells[colNeedQuantity.Name].Value);//计划数量
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colQuantity.Name].Value);//需用数量
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);//单价
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//描述
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[colMoney.Name].Value);//金额
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);//物资名称
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格型号
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colStuff.Name].Value);//材质
                    curBillDtl.QualityStandard = ClientUtil.ToString(var.Cells[colQualityStandard.Name].Value);//质量标准
                    if (var.Cells[colUsedPart.Name].Value != null)
                    {
                        curBillDtl.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                        curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                    }
                    curBillDtl.SpecialType = ClientUtil.ToString(var.Cells[colSpecailType.Name].Value);//专业分类
                    curBillDtl.MaterialCategoryName = ClientUtil.ToString(var.Cells[colMaterialType.Name].Value);//材料类别
                    curBillDtl.MaterialCategory = var.Cells[colMaterialType.Name].Tag as MaterialCategory;
                    curBillMaster.ForwardBillId =ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);
                    curBillDtl.ForwardDetailId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);
                    curBillDtl.QuantityTemp = ClientUtil.ToDecimal(var.Cells[colQuantityTemp.Name].Value);
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                    curBillDtl.TechnologyParameter = ClientUtil.ToString(var.Cells[colTelchPara.Name].Value);
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
            if (txtApplyCode.Text == "")
            {
                MessageBox.Show("请选择需求总计划单");
                return;
            }
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    CommonMaterial materialSelector = new CommonMaterial();
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

                        //dgDetail[colBalECRule.Name, i].Value = Enum.GetNames(typeof(EnumMaterialMngBalRule)).First();
                        i++;
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
                {
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    List<TreeNode> list = frm.SelectResult;
                    if (list.Count > 0)
                    {
                        dgDetail[colUsedPart.Name, e.RowIndex].Tag = (list[0] as TreeNode).Tag as GWBSTree;
                        dgDetail[colUsedPart.Name, e.RowIndex].Value = (list[0] as TreeNode).Text;
                        txtCode.Focus();
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
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtRemark.Text = curBillMaster.Descript;
                txtApplyCode.Text = curBillMaster.ForwardBillCode;
                txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                //专业分类
                if (curBillMaster.SpecialType != null)
                {
                    cboProfessionCat.SelectedItem = curBillMaster.SpecialType;
                }
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                txtSumQuantity.Text = ClientUtil.ToString(curBillMaster.SumQuantity.ToString("#,###.####"));
                this.txtSummoney.Text = ClientUtil.ToString(curBillMaster.SumMoney.ToString("#,###.####"));
                txtProject.Text = ClientUtil.ToString(curBillMaster.ProjectName);
                //txtProject.Text = StaticMethod.GetProjectName();
                this.dgDetail.Rows.Clear();
                foreach (SupplyPlanDetail var in curBillMaster.Details)
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
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;//需用数量
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colMoney.Name, i].Value = var.Money;
                    this.dgDetail[colNeedQuantity.Name, i].Value = var.LeftQuantity;//计划数量
                    this.dgDetail[colSpecailType.Name, i].Value = var.SpecialType;//专业分类
                    this.dgDetail[colMaterialType.Name, i].Value = var.MaterialCategoryName;//材料类别
                    this.dgDetail[colQualityStandard.Name, i].Value = var.QualityStandard;//质量标准
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;//前驱单号
                    this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;//临时数量
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;//使用部位
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;//备注
                    this.dgDetail.Rows[i].Tag = var;
                    this.dgDetail[colTelchPara.Name, i].Value = var.TechnologyParameter;
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
            decimal QuantityTemp = 0;
            decimal MoneyTemp = 0;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colNeedQuantity.Name || colName == colPrice.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colNeedQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colNeedQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("计划数量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colNeedQuantity.Name].Value = "";
                        flag = false;
                    }
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("价格为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "";
                        flag = false;
                    }
                }
                if (flag)
                {
                    //根据单价和数量计算金额  
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colNeedQuantity.Name].Value;
                    object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                    decimal sumqty = 0;
                    decimal SMoney = 0;
                    if (ClientUtil.ToString(quantity) != "" && ClientUtil.ToString(price) != "")
                    {
                        decimal desumMoney = ClientUtil.ToDecimal(quantity) * ClientUtil.ToDecimal(price);
                        this.dgDetail[colMoney.Name, e.RowIndex].Value = ClientUtil.ToString(desumMoney);
                        if (txtSummoney.Text.Equals(""))
                        {
                            SMoney = 0;
                        }
                        else
                        {
                            SMoney = ClientUtil.ToDecimal(txtSummoney.Text.ToString());
                        }
                        MoneyTemp = SMoney + desumMoney;
                        txtSummoney.Text = ClientUtil.ToString(MoneyTemp);
                    }
                }
                
            }
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                if (colName == colNeedQuantity.Name)
                {
                    string Quantemp = ClientUtil.ToString(dgDetail.Rows[i].Cells[colNeedQuantity.Name].Value);
                    validity = CommonMethod.VeryValid(Quantemp);

                    if (Quantemp == null || Quantemp == "")
                    {
                        Quantemp = "0";
                        return;
                    }
                    validity = CommonMethod.VeryValid(Quantemp);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        flag = false;
                    }
                    QuantityTemp += ClientUtil.ToDecimal(Quantemp);
                    txtSumQuantity.Text = ClientUtil.ToString(QuantityTemp);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            VDemandMasterPlanSelector vmros = new VDemandMasterPlanSelector(this.ExecType );
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            DemandMasterPlanMaster supplyMaster = list[0] as DemandMasterPlanMaster;
            txtApplyCode.Tag = supplyMaster.Id;
            txtApplyCode.Text = supplyMaster.Code;

            ////处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                DemandMasterPlanDetail dtl = dr.Tag as DemandMasterPlanDetail;
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

            ////显示引用的明细
            this.dgDetail.Rows.Clear();
            foreach (DemandMasterPlanDetail var in supplyMaster.Details)
            {
                if (var.IsSelect == false) continue;
                int i = this.dgDetail.Rows.Add();
                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colStuff.Name, i].Value = var.MaterialStuff;
                this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                this.dgDetail[this.colPrice.Name, i].Value = var.Price;
                this.dgDetail[colDescript.Name, i].Value = var.Descript;
                this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                this.dgDetail[colQuantity.Name, i].Value = var.Quantity;//数量
                this.dgDetail[colTelchPara.Name, i].Value = var.TechnologyParameter;//技术参数
                this.dgDetail[colQuantityTemp.Name, i].Value = var.DemandLeftQuantity;//引用数量
             this.dgDetail[colSpecailType.Name, i].Value = supplyMaster.SpecialType;
            }
           // cboProfessionCat.SelectedValue  = supplyMaster.SpecialType;
            for (int i = 0; i < cboProfessionCat.Items.Count; i++)
            {
                string sName=cboProfessionCat.Items[i] as string ;
                if (string.Equals(sName, supplyMaster.SpecialType))
                {
                    cboProfessionCat.SelectedIndex = i;
                    break;
                }
            }
            {

            }
           // cboProfessionCat_SelectedIndexChanged(cboProfessionCat, null);
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"物资采购计划单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"物资采购计划单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            curBillMaster = model.SupplyPlanSrv.SaveSupplyPlan(curBillMaster, movedDtlList);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"物资采购计划单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("物资采购计划单【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(SupplyPlanMaster billMaster)
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
            flexGrid1.Cell(3, 5).Text = billMaster.MaterialCategoryName;
            flexGrid1.Cell(3, 8).Text = billMaster.Compilation;
            decimal sumQuantity = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                SupplyPlanDetail detail = (SupplyPlanDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Column(1).AutoFit();
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
                flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Column(2).AutoFit();
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i,3).Text = detail.MatStandardUnitName;
                flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Column(3).AutoFit();
                //需用数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 4).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Column(4).AutoFit();
                //采购数量
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.Quantity);
                sumQuantity += ClientUtil.ToDecimal(detail.Quantity.ToString());
                
                flexGrid1.Column(5).AutoFit();
                //进场时间
                //flexGrid1.Cell(detailStartRowNumber + i,6).Text = detail.

                //使用部位
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.UsedPartName);
                flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Column(7).AutoFit();
                //质量标准
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.QualityStandard);
                flexGrid1.Cell(detailStartRowNumber + i, 8).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Column(8).AutoFit();
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 9).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                flexGrid1.Column(9).AutoFit();
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 1).Text = "合计：";
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = "";
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumQuantity);//采购数量
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                }
                //条形码
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
                this.flexGrid1.Cell(1, 7).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
                this.flexGrid1.Cell(1, 7).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 7).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            }
            flexGrid1.Cell(6 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(6 + detailCount, 5).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(6 + detailCount, 8).Text = billMaster.CreatePersonName;

            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

            //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
            //pageSetup.PaperSize = paperSize;

        }
    }
}
