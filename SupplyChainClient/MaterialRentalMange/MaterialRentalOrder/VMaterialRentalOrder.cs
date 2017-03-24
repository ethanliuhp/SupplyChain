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

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder
{
    public partial class VMaterialRentalOrder : TMasterDetailView
    {
        private MMatRentalMng model = new MMatRentalMng();
        private MaterialRentalOrderMaster curBillMaster;

        /// <summary>
        /// 当前单据
        /// </summary>
        public MaterialRentalOrderMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VMaterialRentalOrder()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            txtBalRule.DataSource = (Enum.GetNames(typeof(EnumMaterialMngBalRule)));
            VBasicDataOptr.InitMatCostType(colCostType, true);
            dgDetail.ContextMenuStrip = cmsDg;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            this.dgBasicCostDetail.CellMouseClick += new DataGridViewCellMouseEventHandler(dgBasicCostDetail_CellMouseClick);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }


        void dgBasicCostDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (txtSupply.Text == "")
            {
                MessageBox.Show("请先选择料具出租方！");
                dgBasicCostDetail.Rows.Clear();
                return;
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
                    curBillMaster.Details.Remove(dr.Tag as MaterialRentalOrderDetail);
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
        public void Start(string id)
        {
            try
            {
                if (id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.MatMngSrv.GetMaterialRentalOrderById(id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate1, txtHandlePerson, txtProject };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            txtSupply.Result.Clear();
            this.txtSupply.Tag = null;
            this.txtSupply.Text = "";
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
                this.curBillMaster = new MaterialRentalOrderMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate1.Text = ConstObject.LoginDate.ToShortDateString();
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
                }
                txtContractNo.Focus();
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
                curBillMaster = model.MatMngSrv.GetMaterialRentalOrderById(curBillMaster.Id);
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
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                curBillMaster = model.SaveMaterialRentalOrder(curBillMaster);
                //插入日志
                //MStockIn.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, curBillMaster.CreatePerson.Name, "料具租赁合同","");
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.MatMngSrv.GetMaterialRentalOrderById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.MatMngSrv.DeleteByDao(curBillMaster)) return false;
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                if (e.ToString().IndexOf("完整约束条件") != -1)
                {
                    MessageBox.Show("数据删除错误：[存在后续单据引用,不能删除此合同]");
                }
                else
                {
                    MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                }
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
                        curBillMaster = model.MatMngSrv.GetMaterialRentalOrderById(curBillMaster.Id);
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
                curBillMaster = model.MatMngSrv.GetMaterialRentalOrderById(curBillMaster.Id);
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
            //校验当前料具商是否已签订料具合同
            if (model.MatMngSrv.VerifyCurrSupplierOrder(txtSupply.Result[0] as SupplierRelationInfo, curBillMaster) == true)
            {
                MessageBox.Show(txtSupply.Text + "已经签订料具租赁合同。");
                return false;
            }
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            if (txtContractNo.Text == "")
            {
                validMessage += "租赁合同号不能为空！\n";
            }
            if (txtSupply.Result.Count == 0)
            {
                validMessage += "出租方不能为空！\n";
            }
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

                if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                {
                    MessageBox.Show("单价不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }
            }
            //校验费用基本设置数据
            foreach (DataGridViewRow var in dgBasicCostDetail.Rows)
            {
                //最后一行不进行校验
                if (var.IsNewRow) break;
                int tempCount = 0;
                bool IsChecked = false;
                IsChecked = Convert.ToBoolean(var.Cells[colEntry.Name].EditedFormattedValue);
                if (IsChecked == true)
                {
                    tempCount++;
                }
                IsChecked = Convert.ToBoolean(var.Cells[colExit.Name].EditedFormattedValue);
                if (IsChecked == true)
                {
                    tempCount++;
                }
                if (tempCount == 0)
                {
                    MessageBox.Show("基本费用设置，退场计算或者退场计算必须选择一个");
                    dgBasicCostDetail.CurrentCell = var.Cells[colCostType.Name];
                    return false;
                }
            }
            dgDetail.Update();
            dgBasicCostDetail.Update();
            return true;
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
                }
                curBillMaster.CreateDate  = dtpDateBegin.Value.Date;
                curBillMaster.OriginalContractNo = this.txtContractNo.Text;
                curBillMaster.BalRule = this.txtBalRule.Text;
                curBillMaster.Descript = this.txtRemark.Text;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MaterialRentalOrderDetail curBillDtl = new MaterialRentalOrderDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as MaterialRentalOrderDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);
                    if (ClientUtil.ToString(var.Cells[colSetRule.Name].Value) == "已设置")
                    {
                        curBillDtl.RuleState = 1;
                    }
                    else if (ClientUtil.ToString(var.Cells[colSetRule.Name].Value) == "未设置")
                    {
                        curBillDtl.RuleState = 0;
                    }
                    if (var.Cells[colSetRule.Name].Tag != null)
                    {
                        //先清除已有当前明细的费用明细设置表
                        IList list_temp = new ArrayList();
                        foreach (BasicDtlCostSet detail in curBillDtl.BasicDtlCostSets)
                        {
                            list_temp.Add(detail);
                        }
                        foreach (BasicDtlCostSet detail in list_temp)
                        {
                            curBillDtl.BasicDtlCostSets.Remove(detail);
                        }
                        //然后再增加
                        foreach (BasicDtlCostSet dtlCostSet in var.Cells[colSetRule.Name].Tag as IList)
                        {
                            curBillDtl.AddBasicDtlCostSet(dtlCostSet);
                        }
                    }
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);

                    curBillMaster.AddDetail(curBillDtl);
                }
                //费用基本设置
                bool IsChecked = true;
                foreach (DataGridViewRow var in dgBasicCostDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    BasicCostSet basicCostSet = new BasicCostSet();
                    if (var.Tag != null)
                    {
                        basicCostSet = var.Tag as BasicCostSet;
                        if (basicCostSet.Id == null)
                        {
                            curBillMaster.BasiCostSets.Remove(basicCostSet);
                        }
                    }
                    basicCostSet.MatCostType = ClientUtil.ToString(var.Cells[colCostType.Name].Value);
                    IsChecked = Convert.ToBoolean(var.Cells[colEntry.Name].EditedFormattedValue);

                    if (IsChecked == true)
                    {
                        //进场计算：1
                        basicCostSet.ApproachCalculation = 1;
                    }
                    else
                    {
                        //进场不算：0
                        basicCostSet.ApproachCalculation = 0;
                    }

                    IsChecked = Convert.ToBoolean(var.Cells[colExit.Name].EditedFormattedValue);
                    if (IsChecked == true)
                    {
                        //退场计算：1
                        basicCostSet.ExitCalculation = 1;
                    }
                    else
                    {
                        //退场不算：0
                        basicCostSet.ExitCalculation = 0;
                    }

                    basicCostSet.Descript = ClientUtil.ToString(var.Cells[colRemark.Name].Value);

                    curBillMaster.AddBasicCostSet(basicCostSet);
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
                this.txtCode.Text = curBillMaster.Code;
                dtpDateBegin.Value = curBillMaster.CreateDate ;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtContractNo.Text = curBillMaster.OriginalContractNo;
                txtBalRule.Text = curBillMaster.BalRule;
                if (curBillMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = curBillMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(curBillMaster.TheSupplierRelationInfo);
                    txtSupply.Value = curBillMaster.TheSupplierRelationInfo.SupplierInfo.Name;
                }
                txtRemark.Text = curBillMaster.Descript;
                txtCreateDate1.Text = curBillMaster.CreateDate.ToShortDateString();


                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;

                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreateDate1.Text = curBillMaster.CreateDate.ToShortDateString();
                txtProject.Text = curBillMaster.ProjectName;

                this.dgDetail.Rows.Clear();
                foreach (MaterialRentalOrderDetail var in curBillMaster.Details)
                {
                    IList list_BasicDtlCostSet = new ArrayList();
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnit.Name;

                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    if (var.RuleState == 1)
                    {
                        this.dgDetail[colSetRule.Name, i].Value = "已设置";
                    }
                    else if (var.RuleState == 0)
                    {
                        this.dgDetail[colSetRule.Name, i].Value = "未设置";
                    }
                    foreach (BasicDtlCostSet detial in var.BasicDtlCostSets)
                    {
                        list_BasicDtlCostSet.Add(detial);
                    }
                    this.dgDetail[colSetRule.Name, i].Tag = list_BasicDtlCostSet;

                    this.dgDetail.Rows[i].Tag = var;
                }
                //费用基本设置
                if (dgBasicCostDetail.Rows.Count > 0)
                {
                    this.dgBasicCostDetail.Rows.Clear();
                }
                foreach (BasicCostSet costSet in curBillMaster.BasiCostSets)
                {
                    int i = this.dgBasicCostDetail.Rows.Add();
                    this.dgBasicCostDetail[colCostType.Name, i].Value = costSet.MatCostType;
                    if (costSet.ApproachCalculation == 1)
                    {
                        dgBasicCostDetail[colEntry.Name, i].Value = true;
                    }
                    else if (costSet.ApproachCalculation == 0)
                    {
                        dgBasicCostDetail[colEntry.Name, i].Value = false;
                    }

                    if (costSet.ExitCalculation == 1)
                    {
                        dgBasicCostDetail[colExit.Name, i].Value = true;
                    }
                    else if (costSet.ExitCalculation == 0)
                    {
                        dgBasicCostDetail[colExit.Name, i].Value = false;
                    }

                    dgBasicCostDetail[colRemark.Name, i].Value = costSet.Descript;
                    dgBasicCostDetail.Rows[i].Tag = costSet;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
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

            if (this.txtSupply.Text == "")
            {
                MessageBox.Show("请选择料具出租方！");
                return;
            }
            if (this.dgBasicCostDetail.Rows.Count - 1 <= 0)
            {
                MessageBox.Show("请先定义[费用基本设置]！");
                return;
            }
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                CommonMaterial materialSelector = new CommonMaterial();
                materialSelector.materialCatCode = CommonUtil.TurnMaterialMatCode;
                DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                object tempValue = cell.EditedFormattedValue;
                if (tempValue != null && !tempValue.Equals(""))
                {
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
                    this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                    if (theMaterial.BasicUnit != null)
                        this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;

                    i++;
                }
            }
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSetRule.Name))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                if (theCurrentRow.Cells[colMaterialCode.Name].Tag != null)
                {
                    IList list_CostSet = new ArrayList();
                    foreach (DataGridViewRow var in dgBasicCostDetail.Rows)
                    {
                        bool IsChecked = false;

                        BasicCostSet costSet = new BasicCostSet();
                        costSet.MatCostType = ClientUtil.ToString(var.Cells[colCostType.Name].Value);
                        IsChecked = Convert.ToBoolean(var.Cells[colEntry.Name].EditedFormattedValue);
                        if (IsChecked == true)
                        {
                            costSet.ApproachCalculation = 1;
                        }
                        else
                        {
                            costSet.ApproachCalculation = 0;
                        }
                        IsChecked = Convert.ToBoolean(var.Cells[colExit.Name].EditedFormattedValue);
                        if (IsChecked == true)
                        {
                            costSet.ExitCalculation = 1;
                        }
                        else
                        {
                            costSet.ExitCalculation = 0;
                        }
                        list_CostSet.Add(costSet);
                    }
                    VMatCostDtlSet matCostSet = new VMatCostDtlSet(theCurrentRow.Cells[colMaterialCode.Name].Tag as Material, list_CostSet);
                    matCostSet.OpenCostSet(this.dgDetail.CurrentRow.Cells[colSetRule.Name].Tag as IList);
                    this.dgDetail.CurrentRow.Cells[colSetRule.Name].Tag = matCostSet.Result;
                    if (matCostSet.Result != null && matCostSet.Result.Count > 0)
                    {
                        this.dgDetail.CurrentRow.Cells[colSetRule.Name].Value = "已设置";
                    }
                }
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool IsChecked = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPrice.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"料具租赁合同打印.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"料具租赁合同打印.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            curBillMaster = model.SaveMaterialRentalOrder(curBillMaster);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"料具租赁合同打印.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(MaterialRentalOrderMaster billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //主表数据

            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 4).Text = DateTime.Now.ToShortDateString();
            flexGrid1.Cell(3, 6).Text = "";
            flexGrid1.Cell(3,7).Text = "合同名称：";
            flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
            flexGrid1.Cell(4, 4).Text = "";
            flexGrid1.Cell(4, 6).Text = billMaster.RealOperationDate.ToShortDateString();
            flexGrid1.Cell(4, 7).Text = "承租单位：";

            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;

                //结算规则
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = Enum.GetName(typeof(EnumMaterialMngBalRule), detail.RuleState);

                //数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

                //日租金
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

                //金额
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Money);

                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            flexGrid1.Cell(8 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(8 + detailCount, 4).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(8 + detailCount, 6).Text = billMaster.CreatePersonName;

        }
    }
}
