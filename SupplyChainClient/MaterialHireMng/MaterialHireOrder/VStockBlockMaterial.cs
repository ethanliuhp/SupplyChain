using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using System.Collections;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireLedger;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    public partial class VStockBlockMaterial : TMasterDetailView
    {

        CustomFlexGrid flexGrid1;
        private MMaterialHireMng model = new MMaterialHireMng();
        private MatHireStockBlockMaster Master;
        EnumMatHireType MatHireType;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空   
       // CurrentProjectInfo ProjectInfo;
        MatHireOrderMaster OrderMaster;
        public bool IsProject = false;

        public VStockBlockMaterial(EnumMatHireType MatHireType)
        {
            InitializeComponent();
            this.MatHireType = MatHireType;
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            //this.txtMaterialCategory.rootCatCode = "01";
            //this.txtMaterialCategory.rootLevel = "3";
            DateTimePicker dp = new DateTimePicker();
            DataGridViewCalendarColumn dc = new DataGridViewCalendarColumn();
            dp.CustomFormat = "yyyy-MM-dd";
            dp.Format = DateTimePickerFormat.Custom;
            dp.Visible = false;
            dgDetail.Controls.Add(dp);
            dp = new DateTimePicker();
            if (this.MatHireType == EnumMatHireType.钢管)
            {
                this.colLength.Visible = true;
                this.colType.Visible = false;
            }
            else if (this.MatHireType == EnumMatHireType.普通料具)
            {
                this.colLength.Visible = false;
                this.colType.Visible = false;
            }
            else if (this.MatHireType == EnumMatHireType.碗扣)
            {
                this.colLength.Visible = false;
                this.colType.Visible = true;
                //this.colType.Items.AddRange(VMaterialHireCollection.arrWKType);
            }
            this.colBorrowUnit.Visible = this.colUsedPart.Visible = this.colSubject.Visible = IsProject;
            //txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
            txtBalRule.DataSource = (Enum.GetNames(typeof(EnumMaterialHireMngBalRule)));
        }

        private void InitEvent()
        {
            //this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
           // this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
           // this.TenantSelector.TenantSelectorAfterEvent += new UcTenantSelector.TenantSelectorAfterEventHandler(TenantSelectorAfter);
           // this.txtSupply.TextChanged += new EventHandler(txtSupply_TextChanged);
            this.dtpDateBegin.TextChanged+=new EventHandler(dtpDate_TextChanged);
            this.dtpDateEnd.TextChanged += new EventHandler(dtpDate_TextChanged);
            this.btnForward.Click += new EventHandler(btnForward_Click);
        }
        void btnForward_Click(object sender, EventArgs e)
        {
            int i = 0;
            string []arrMaterialIDs;
            MatHireOrderMaster oTemp = null;
            VMaterialHireOrderSelectMaterial oVMaterialHireOrderSelectMaterial = new VMaterialHireOrderSelectMaterial(this.MatHireType);
            oVMaterialHireOrderSelectMaterial.ShowDialog();
            if (oVMaterialHireOrderSelectMaterial.Result != null && oVMaterialHireOrderSelectMaterial.Result.Count > 0)
            {
                oTemp = oVMaterialHireOrderSelectMaterial.Result[0] as MatHireOrderMaster;
                if (OrderMaster!=null && oTemp.Id != OrderMaster.Id)
                {
                    dgDetail.Rows.Clear();
                }
               OrderMaster=oTemp;
                #region 数据填充
                txtSupply.Text = OrderMaster.SupplierName;
                txtSupply.Tag = OrderMaster.TheSupplierRelationInfo;
                txtContract.Tag = OrderMaster;
                txtContract.Text = OrderMaster.Code;
                txtProjectName.Text = OrderMaster.ProjectName;
                txtProjectName.Tag = OrderMaster.ProjectId;
                arrMaterialIDs = dgDetail.Rows.OfType<DataGridViewRow>().Where(a => !a.IsNewRow && a.Cells[colMaterialCode.Name].Tag != null).Select(a => (a.Cells[colMaterialCode.Name].Tag as Material).Id).Distinct().ToArray();
                if (this.MatHireType == EnumMatHireType.普通料具)
                {
                    foreach (MatHireOrderDetail oMatHireOrderDetail in OrderMaster.TempData)
                    {
                        if (oMatHireOrderDetail.IsSelect )
                        {
                            if (arrMaterialIDs != null && !string.IsNullOrEmpty(arrMaterialIDs.FirstOrDefault(a => a == oMatHireOrderDetail.MaterialResource.Id))) continue;
                             i = this.dgDetail.Rows.Add();
                             this.dgDetail[colMaterialCode.Name, i].Value = oMatHireOrderDetail.MaterialCode;
                             this.dgDetail[colMaterialCode.Name, i].Tag = oMatHireOrderDetail.MaterialResource;
                             this.dgDetail[colMaterialSpec.Name, i].Value = oMatHireOrderDetail.MaterialSpec;
                             this.dgDetail[colMaterialName.Name, i].Value = oMatHireOrderDetail.MaterialName;
                            //this.dgDetail[colCategoryQuantity.Name, i].Value = master.LeftQuantity;
                             this.dgDetail[colUnit.Name, i].Value = oMatHireOrderDetail.MatStandardUnitName;
                             this.dgDetail[colUnit.Name, i].Tag = oMatHireOrderDetail.MatStandardUnit;
                             this.dgDetail[colPrice.Name, i].Value = oMatHireOrderDetail.Price;
                            this.dgDetail[this.colLength.Name, i].Value = 1;
                            this.dgDetail[this.colLeftQuanity.Name, i].Value = oMatHireOrderDetail.TempData3;
                        }
                    }
                }
                else if (this.MatHireType == EnumMatHireType.碗扣)
                {
                    decimal dLen = 0;
                    foreach (MatHireOrderDetail oMatHireOrderDetail in OrderMaster.TempData)
                    {
                        if (oMatHireOrderDetail.IsSelect)
                        {
                            if (arrMaterialIDs != null && !string.IsNullOrEmpty(arrMaterialIDs.FirstOrDefault(a => a == oMatHireOrderDetail.MaterialResource.Id))) break;
                            this.dgDetail.Rows.Clear();
                            foreach (string sType in VMaterialHireCollection.arrWKType)
                            {
                                i = this.dgDetail.Rows.Add();
                                this.dgDetail[colMaterialCode.Name, i].Value = oMatHireOrderDetail.MaterialCode;
                                this.dgDetail[colMaterialCode.Name, i].Tag = oMatHireOrderDetail.MaterialResource;
                                this.dgDetail[colMaterialSpec.Name, i].Value = oMatHireOrderDetail.MaterialSpec;
                                this.dgDetail[colMaterialName.Name, i].Value = oMatHireOrderDetail.MaterialName;
                                //this.dgDetail[colCategoryQuantity.Name, i].Value = master.LeftQuantity;
                                this.dgDetail[colUnit.Name, i].Value = oMatHireOrderDetail.MatStandardUnitName;
                                this.dgDetail[colUnit.Name, i].Tag = oMatHireOrderDetail.MatStandardUnit;
                                this.dgDetail[colPrice.Name, i].Value = oMatHireOrderDetail.Price;
                                this.dgDetail[this.colType.Name, i].Value = sType;
                                dLen= GetLength(sType);
                                this.dgDetail[this.colLength.Name, i].Value = dLen;
                                this.dgDetail[this.colLeftQuanity.Name, i].Value =Math.Round( ClientUtil.ToDecimal( oMatHireOrderDetail.TempData3)/dLen,2).ToString("N2");
                            }
                        }
                    }
                }
                else if (this.MatHireType == EnumMatHireType.钢管)
                {
                    decimal dLen = 0;
                    foreach (MatHireOrderDetail oMatHireOrderDetail in OrderMaster.TempData)
                    {
                        if (oMatHireOrderDetail.IsSelect)
                        {
                            if (arrMaterialIDs != null && !string.IsNullOrEmpty(arrMaterialIDs.FirstOrDefault(a => a == oMatHireOrderDetail.MaterialResource.Id))) break;
                            this.dgDetail.Rows.Clear();
                            for (decimal j = 60; j >= 1; j--)
                            {
                                i = this.dgDetail.Rows.Add();
                                this.dgDetail[colMaterialCode.Name, i].Value = oMatHireOrderDetail.MaterialCode;
                                this.dgDetail[colMaterialCode.Name, i].Tag = oMatHireOrderDetail.MaterialResource;
                                this.dgDetail[colMaterialSpec.Name, i].Value = oMatHireOrderDetail.MaterialSpec;
                                this.dgDetail[colMaterialName.Name, i].Value = oMatHireOrderDetail.MaterialName;
                                //this.dgDetail[colCategoryQuantity.Name, i].Value = master.LeftQuantity;
                                this.dgDetail[colUnit.Name, i].Value = oMatHireOrderDetail.MatStandardUnitName;
                                this.dgDetail[colUnit.Name, i].Tag = oMatHireOrderDetail.MatStandardUnit;
                                this.dgDetail[colPrice.Name, i].Value = oMatHireOrderDetail.Price;
                               
                                dLen = j / 10;
                                this.dgDetail[this.colLength.Name, i].Value = dLen;
                                this.dgDetail[this.colLeftQuanity.Name, i].Value = Math.Round(ClientUtil.ToDecimal(oMatHireOrderDetail.TempData3) / dLen, 2).ToString("N2");
                            
                            }
                        }
                    }
                }
                #endregion
            }
        }
        void dtpDate_TextChanged(object sender, EventArgs e)
        {
            this.dtpDateBegin.TextChanged -= new EventHandler(dtpDate_TextChanged);
            this.dtpDateEnd.TextChanged -= new EventHandler(dtpDate_TextChanged);
            if (sender is CustomDateTimePicker)
            {
                CustomDateTimePicker date = sender as CustomDateTimePicker;
                if (date == dtpDateBegin || date == dtpDateEnd)
                {
                    if (date == dtpDateBegin)
                    {
                        if (date.Value > dtpDateEnd.Value)
                        {
                            dtpDateEnd.Value = date.Value;
                        }
                    }
                    else
                    {
                        if (date.Value < this.dtpDateBegin.Value)
                        {
                            dtpDateBegin.Value = date.Value;
                        }
                    }
                    SumMoney();
                }
                 
            }
            this.dtpDateBegin.TextChanged += new EventHandler(dtpDate_TextChanged);
            this.dtpDateEnd.TextChanged += new EventHandler(dtpDate_TextChanged);
        }
        //void txtSupply_TextChanged(object sender, EventArgs e)
        //{
        //    MatHireOrderMaster oOrder = null;
        //    DataGridViewTextBoxColumn oColumn = null;
         
        //    Hashtable htColumn = new Hashtable();
        //    #region 清除

             
        //    #endregion
        //    #region 判断
        //    if (txtSupply.Result == null || txtSupply.Result.Count == 0)
        //    {
        //        //  ShowMessage("请选择料具出租方");
        //        this.txtSupply.Focus();
        //        return;
        //    }
        //    else if (this.TenantSelector.SelectedProject == null)
        //    {
        //        // ShowMessage("请选择租赁方");
        //        this.TenantSelector.Focus();
        //        return;
        //    }
        //    oOrder = GetOrderMaster();
        //    if (oOrder == null)
        //    {
        //        ShowMessage(string.Format("出租方[{0}]与租赁方[{1}]未签订租赁合同,无法发料(检尺)", txtSupply.Text, TenantSelector.SelectedProject.Name));
        //        return;
        //    }

        //    #endregion
        //    #region 添加列
        //    OrderMaster = oOrder;
        //    txtBalRule.Text = OrderMaster.BalRule;
        //    #endregion
        //}

        //void TenantSelectorAfter(UcTenantSelector a)
        //{
        //    this.ProjectInfo = a.SelectedProject;
        //    if (Master != null)
        //    {
        //        Master.ProjectId = ProjectInfo == null ? "" : ProjectInfo.Id;
        //        Master.ProjectName = ProjectInfo == null ? "" : ProjectInfo.Name;
        //    }
        //    if (this.ProjectInfo != null && txtSupply.Result != null && txtSupply.Result.Count > 0)
        //    {
        //        txtSupply_TextChanged(null, null);
        //    }
        //}
        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
             Master.Details.Remove(e.Row.Tag as BaseDetail);
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
                    Master = model.MaterialHireSupplyOrderSrv.GetStockBlockMasterOrderById(id);
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
                    //cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    //cmsDg.Enabled = false;
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

            ////永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtBalRule ,txtSumMoney, txtSupply, txtProjectName, txtContract };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name,colMaterialCode.Name, colLeftQuanity.Name,
                                                colUsedPart.Name,colPrice.Name,colBorrowUnit.Name,colSubject.Name, colUnit.Name,colMoney.Name,colLength.Name,colType.Name};
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            //txtSupply.Result.Clear();
            //this.txtSupply.Tag = null;
            //this.txtSupply.Text = "";
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

        #region 增删改查
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
                this.Master = new MatHireStockBlockMaster();
                Master.CreatePerson = ConstObject.LoginPersonInfo;
                Master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                Master.CreateDate = ConstObject.LoginDate;
                Master.CreateYear = ConstObject.LoginDate.Year;
                Master.CreateMonth = ConstObject.LoginDate.Month;
                Master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                Master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                Master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                Master.HandlePerson = ConstObject.LoginPersonInfo;
                Master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                Master.DocState = DocumentState.Edit;
                Master.MatHireType = this.MatHireType;
                Master.BalState = 0;
           
                //制单日期
                txtCreateDate.Value = ConstObject.LoginDate;
                //责任人
               // txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
               // txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                txtCreatePerson.Text = Master.CreatePersonName;
                txtCreatePerson.Tag = Master.CreatePerson;
                //归属项目
  
                //if (ProjectInfo != null)
                //{
                  
                //    Master.ProjectId = ProjectInfo.Id;
                //    Master.ProjectName = ProjectInfo.Name;
                //}
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
            if (Master.DocState == DocumentState.Edit || Master.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                Master = model.MaterialHireSupplyOrderSrv.GetStockBlockMasterOrderById(Master.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(Master.DocState));
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
                bool IsNew = string.IsNullOrEmpty(Master.Id);
                Master = model.MaterialHireSupplyOrderSrv.SaveStockBlockMasterOrder(Master);
                //插入日志
                //MStockIn.InsertLogData(Master.Id, "保存", Master.Code, Master.CreatePerson.Name, "料具租赁合同","");
                txtCode.Text = Master.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                WriteLog(IsNew);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }
        public override bool SubmitView()
        {
            try
            {
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                Master.DocState = DocumentState.InExecute;
                bool IsNew = (string.IsNullOrEmpty(Master.Id) ? true : false);
              
                    Master = model.MaterialHireSupplyOrderSrv.SaveStockBlockMasterOrder(Master);
         
                txtCode.Text = Master.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                WriteLog(IsNew);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
               Master.DocState = DocumentState.Edit;
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
                Master = model.MaterialHireSupplyOrderSrv.GetStockBlockMasterOrderById(Master.Id);
                if (Master.DocState == DocumentState.Valid || Master.DocState == DocumentState.Edit)
                {
                    if (!model.MaterialHireSupplyOrderSrv.DeleteByDao(Master)) return false;
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(Master.DocState) + "】，不能删除！");
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
                        Master = model.MaterialHireSupplyOrderSrv.GetStockBlockMasterOrderById(Master.Id);
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
                Master = model.MaterialHireSupplyOrderSrv.GetStockBlockMasterOrderById(Master.Id);
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

            //if (model.MaterialHireSupplyOrderSrv.VerifyCurrSupplierOrder((txtSupply.Result[0] as SupplierRelationInfo), curStockBlockMaster) == true)
            //{
            //    MessageBox.Show(txtSupply.Text + "已经签订料具租赁合同。");
            //    return false;
            //}
            string validMessage = "";
            if (this.dgDetail.Rows.Count==0||(this.dgDetail.Rows.Count == 1 && this.dgDetail.Rows[0].IsNewRow))
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            if (this.dtpDateBegin.Value > this.dtpDateEnd.Value)
            {
                ShowMessage("[开始时间]必须小于等于[结束时间]");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            //if (txtContractNo.Text == "")
            //{
            //    validMessage += "租赁合同号不能为空！\n";
            //}
            //if (txtSupply.Result.Count == 0)
            //{
            //    validMessage += "出租方不能为空！\n";
            //}
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

                //if (dr.Cells[colMaterialCode.Name].Tag == null)
                //{
                //    MessageBox.Show("物料不允许为空！");
                //    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                //    return false;
                //}

                //if (dr.Cells[colUnit.Name].Tag == null)
                //{
                //    MessageBox.Show("计量单位不允许为空！");
                //    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                //    return false;
                //}

                //if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                //{
                //    MessageBox.Show("单价不允许为空！");
                //    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                //    return false;
                //}
                //if(dr.Cells[colMaterialCode.Name].Tag==null){
                //    MessageBox.Show("请选择物资");
                //    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                //    return false;
                //}
                //if (this.MatHireType==EnumMatHireType.碗扣 && dr.Cells[colType.Name].Value == null)
                //{
                //    MessageBox.Show("请选择碗口类型");
                //    dgDetail.CurrentCell = dr.Cells[colType.Name];
                //    return false;
                //}
                //if (this.MatHireType == EnumMatHireType.钢管 && dr.Cells[colLength.Name].Value == null)
                //{
                //    MessageBox.Show("请选择钢管长度");
                //    dgDetail.CurrentCell = dr.Cells[colLength.Name];
                //    return false;
                //}

                if (this.MatHireType == EnumMatHireType.普通料具)
                {
                    if (dr.Cells[colQuantity.Name].Value == null || ClientUtil.ToDecimal(dr.Cells[colQuantity.Name].Value) == 0)
                    {
                        ShowMessage("封存数量不能等于0");
                    }
                }
             
            }
            ////校验费用基本设置数据
            //foreach (DataGridViewRow var in dgBasicCostDetail.Rows)
            //{
            //    //最后一行不进行校验
            //    if (var.IsNewRow) break;
            //    int tempCount = 0;
            //    bool IsChecked = false;
            //    IsChecked = Convert.ToBoolean(var.Cells[colEntry.Name].EditedFormattedValue);
            //    if (IsChecked == true)
            //    {
            //        tempCount++;
            //    }
            //    IsChecked = Convert.ToBoolean(var.Cells[colExit.Name].EditedFormattedValue);
            //    if (IsChecked == true)
            //    {
            //        tempCount++;
            //    }
            //    if (tempCount == 0)
            //    {
            //        MessageBox.Show("基本费用设置，退场计算或者退场计算必须选择一个");
            //        dgBasicCostDetail.CurrentCell = var.Cells[colCostType.Name];
            //        return false;
            //    }
            //}
            dgDetail.Update();
            //dgBasicCostDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                Master.BlockStartTime = dtpDateBegin.Value.Date;
                //Master.LastBalTime = Master.BlockStartTime;
                Master.BlockFinishTime = dtpDateEnd.Value.Date;
                Master.RealOperationDate = DateTime.Now;
                //Master.CreatePerson = txtHandlePerson.Tag as PersonInfo;
                //Master.CreatePersonName = txtHandlePerson.Text;
                Master.StockReason = txtStockReason.Text;
                Master.Descript = txtRemark.Text;
                Master.CreateDate = txtCreateDate.Value;
                //Master.HandlePerson = txtHandlePerson.Tag as PersonInfo;
                //Master.HandlePersonName = txtHandlePerson.Text;
                Master.ProjectName = txtProjectName.Text;
                Master.ProjectId =ClientUtil.ToString( txtProjectName.Tag);
                Master.SupplierName = txtSupply.Text;
                Master.TheSupplierRelationInfo = txtSupply.Tag as SupplierRelationInfo;
                Master.Contract = txtContract.Tag as MatHireOrderMaster;
                Master.ContractCode = txtContract.Text;
                Master.BillCode = txtOldCode.Text;

                Master.Theme = txtTheme.Text;
                Master.BalRule = txtBalRule.Text;
             
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;

                    MatHireStockBlockDetail curDetail = new MatHireStockBlockDetail();

                    if (var.Tag != null)
                    {
                        curDetail = var.Tag as MatHireStockBlockDetail;
                        if (curDetail.Id == null)
                        {
                            Master.Details.Remove(curDetail);
                        }
                    }
                    curDetail.Master = Master;
                    if (var.Cells[colMaterialCode.Name].Tag != null)
                    {
                        curDetail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                        curDetail.StockId = (var.Cells[colMaterialCode.Name].Tag as Material).Id;
                    }
                    curDetail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curDetail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    curDetail.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);
                    curDetail.BeforeStockQty = ClientUtil.ToDecimal(var.Cells[colLeftQuanity.Name].Value);
                    curDetail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    curDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    if (IsProject)
                    {
                        //使用部位
                        if (var.Cells[colUsedPart.Name].Tag != null)
                        {
                            curDetail.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                            curDetail.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                            curDetail.UsedPartSysCode = ClientUtil.ToString((var.Cells[colUsedPart.Name].Tag as GWBSTree).SysCode);
                        }
                        if (var.Cells[colSubject.Name].Tag != null)
                        {
                            curDetail.SubjectGUID = var.Cells[colSubject.Name].Tag as CostAccountSubject;
                            curDetail.SubjectName = ClientUtil.ToString(var.Cells[colSubject.Name].Value);
                            curDetail.SubjectSysCode = ClientUtil.ToString((var.Cells[colSubject.Name].Tag as CostAccountSubject).SysCode);
                        }
                        if (var.Cells[this.colBorrowUnit.Name].Tag != null)
                        {
                            curDetail.BorrowUnit = var.Cells[colBorrowUnit.Name].Tag as SupplierRelationInfo;
                            curDetail.BorrowUnitName = ClientUtil.ToString(var.Cells[colBorrowUnit.Name].Value);
                        }
                    }
                    curDetail.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    curDetail.MatStandardUnit = var.Cells[this.colUnit.Name].Tag as StandardUnit;
                     curDetail.MatStandardUnitName = ClientUtil.ToString( var.Cells[this.colUnit.Name].Value);
                    curDetail.MaterialLength =1;
                    if (MatHireType != EnumMatHireType.普通料具)
                    {
                        curDetail.MaterialLength = ClientUtil.ToDecimal(var.Cells[this.colLength.Name].Value);
                        curDetail.MaterialType = ClientUtil.ToString(var.Cells[this.colType.Name].Value);
                    }
                    curDetail.Money = ClientUtil.ToDecimal(var.Cells[this.colMoney.Name].Value);
                    Master.AddDetail(curDetail);
                     
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
                txtCode.Text = Master.Code;
                dtpDateBegin.Value = Master.BlockStartTime;
                dtpDateEnd.Value = Master.BlockFinishTime;
               // txtHandlePerson.Tag = Master.HandlePerson;
               // txtHandlePerson.Text = Master.HandlePersonName;
                txtStockReason.Text = Master.StockReason;
                txtRemark.Text = Master.Descript;

                txtCreatePerson.Tag = Master.CreatePerson;
                txtCreatePerson.Text = Master.CreatePersonName;
                txtCreateDate.Value = Master.CreateDate;
                OrderMaster = Master.Contract;
                txtContract.Text = Master.ContractCode;
                txtContract.Tag = Master.Contract;
                txtProjectName.Text = Master.ProjectName;
                txtProjectName.Tag = Master.ProjectId;
                txtSupply.Text = Master.SupplierName;
                txtSupply.Tag = Master.TheSupplierRelationInfo;
               
                txtTheme.Text = Master.Theme;
                txtBalRule.Text=Master.BalRule;
                txtSumMoney.Text = Master.SumMoney.ToString();
                txtOldCode.Text = Master.BillCode;
                //Master.ProjectName = ProjectInfo.Name;
                //Master.ProjectId = ProjectInfo.Id;
                //Master.SupplierName = txtSupply.Name;
                //Master.TheSupplierRelationInfo = txtSupply.Result[0] as SupplierRelationInfo;

                this.dgDetail.Rows.Clear();
                foreach (MatHireStockBlockDetail var in Master.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colId.Name, i].Value = var.Id;
                    this.dgDetail[colStockId.Name, i].Value = var.StockId;
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[this.colLeftQuanity.Name, i].Value = var.BeforeStockQty;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[this.colType.Name, i].Value = var.MaterialType;
                    this.dgDetail[this.colLength.Name, i].Value = var.MaterialLength;

                    if (IsProject)
                    {
                        //设置使用部位
                        this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                        this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;

                        //设置使用队伍
                        this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.BorrowUnit;
                        this.dgDetail[colBorrowUnit.Name, i].Value = var.BorrowUnitName;
                        //科目
                        this.dgDetail[colSubject.Name, i].Tag = var.SubjectGUID;
                        this.dgDetail[colSubject.Name, i].Value = var.SubjectGUID == null ? "" : var.SubjectGUID.Name;
                    }
                    this.dgDetail[colPrice.Name, i].Value = var.Price;

                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    this.dgDetail[colMoney.Name, i].Value = var.Money;
                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

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
                    //DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
                    //cbo.Items.Clear();

                    //StandardUnit first = null;
                    //foreach (StandardUnit cu in selectedMaterial.GetMaterialAllUnit())
                    //{
                    //    cbo.Items.Add(cu.Name);
                    //}
                    //first = selectedMaterial.BasicUnit;
                    //this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = first;
                    //cbo.Value = first.Name;
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
 
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                if (OrderMaster == null)
                {
                    ShowMessage("请选择合同");
                    btnForward.Focus();
                    return;
                }
                CurrentProjectInfo ProjectInfo = new CurrentProjectInfo() { Id=OrderMaster.ProjectId, Name=OrderMaster.ProjectName};
                VMaterialHireLedgerSelector VMaterialRenLedSelector = new VMaterialHireLedgerSelector(this.txtSupply.Tag as SupplierRelationInfo, ProjectInfo,this.MatHireType );
                VMaterialRenLedSelector.OpenSelector();
                IList list = VMaterialRenLedSelector.Result;
                foreach (MatHireLedgerMaster master in list)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Value = master.MaterialCode;
                    this.dgDetail[colMaterialCode.Name, i].Tag = master.MaterialResource;
                    this.dgDetail[colMaterialSpec.Name, i].Value = master.MaterialSpec;
                    this.dgDetail[colMaterialName.Name, i].Value = master.MaterialName;
                    //this.dgDetail[colCategoryQuantity.Name, i].Value = master.LeftQuantity;
                    this.dgDetail[colUnit.Name, i].Value = master.MatStandardUnitName;
                    this.dgDetail[colUnit.Name, i].Tag = master.MatStandardUnit;
                    this.dgDetail[colPrice.Name, i].Value = master.RentalPrice;
                    this.dgDetail[colSubject.Name, i].Tag = master.SubjectGUID;
                    this.dgDetail[colSubject.Name, i].Value = master.SubjectName;
                    this.dgDetail[colUsedPart.Name, i].Tag = master.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = master.UsedPartName;
                    this.dgDetail[this.colBorrowUnit.Name, i].Tag = master.TheRank;
                    this.dgDetail[colBorrowUnit.Name, i].Value = master.TheRankName;
                    if (this.MatHireType == EnumMatHireType.碗扣)
                    {
                        this.dgDetail[this.colType.Name, i].Value =VMaterialHireCollection. arrWKType[0];
                        this.dgDetail[this.colLength.Name, i].Value = GetLength(this.dgDetail[this.colType.Name, i].Value);
                    }
                }
                
            }
            else  if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colType.Name))
            {
                dgDetail[colLength.Name, e.RowIndex].Value = GetLength(dgDetail[colType.Name, e.RowIndex].Value);
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool IsChecked = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colQuantity.Name || colLength.Name == colName || colType.Name == colName)
            {
                if (colName == colQuantity.Name)
                {
                    if (dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value != null)
                    {
                        string temp_Quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value.ToString();
                        validity = CommonMethod.VeryValid(temp_Quantity);
                        if (validity == false)
                        {
                            MessageBox.Show("请输入数字！");
                        }
                    }
                }
                else if (colLength.Name == colName)
                {
                    if (dgDetail.Rows[e.RowIndex].Cells[colLength.Name].Value != null)
                    {
                        string temp_Quantity = dgDetail.Rows[e.RowIndex].Cells[colLength.Name].Value.ToString();
                        validity = CommonMethod.VeryValid(temp_Quantity);
                        if (validity == false)
                        {
                            MessageBox.Show("请输入数字！");
                        }
                    }
                }
                else if (colType.Name == colName)
                {
                    dgDetail.Rows[e.RowIndex].Cells[colLength.Name].Value = GetLength(dgDetail.Rows[e.RowIndex].Cells[colType.Name].Value);
                }
                SumMoney();
            }
        }
        public void SumMoney()
        {
            decimal dSum = 0,dMoney=0, dQty = 0, dLen = 0, dPrice = 0;

            int iDay = GetDay();
            foreach (DataGridViewRow oRow in this.dgDetail.Rows)
            {
                if (oRow.IsNewRow) continue;
                dQty = ClientUtil.ToDecimal(oRow.Cells[this.colQuantity.Name].Value);
                dLen = this.MatHireType == EnumMatHireType.普通料具 ? 1 : ClientUtil.ToDecimal(oRow.Cells[this.colLength.Name].Value);
                dPrice = ClientUtil.ToDecimal(oRow.Cells[this.colPrice.Name].Value);
               dMoney=dQty * dLen * dPrice * iDay;
               oRow.Cells[colMoney.Name].Value = dMoney;
               dSum += dMoney;

            }
            txtSumMoney.Text = dSum.ToString();
        }
        public int GetDay()
        {
            int iDay = 0;
            TimeSpan dt = TransUtil.ToShortDateTime(this.dtpDateEnd.Value) - TransUtil.ToShortDateTime(this.dtpDateBegin.Value);
            iDay = dt.Days;
            if (txtBalRule.Text == "两头都算")
            {
                iDay += 1;
            }
            else if (txtBalRule.Text == "算尾不算头")
            {
                 
            }
            else if (txtBalRule.Text == "两头都不算")
            {
                iDay -= 1;
            }
            else if (txtBalRule.Text == "算头不算尾")
            {
                //iDay = dt.Days + 1;
            }
            return iDay;
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"料具封存单打印.flx") == false) return false;
            FillFlex(Master);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"料具封存单打印.flx") == false) return false;
            FillFlex(Master);
            flexGrid1.Print();
            Master.PrintTimes = Master.PrintTimes + 1;
            Master = model.MaterialHireSupplyOrderSrv.SaveStockBlockMasterOrder(Master);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"料具封存单打印.flx") == false) return false;
            FillFlex(Master);
            flexGrid1.ExportToExcel("料具封存单【" + Master.Code + "】", false, false, true);
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

        private void FillFlex(MatHireStockBlockMaster StockBlockMaster)
        {
            //int detailStartRowNumber = 6;//6为模板中的行号
            //int detailCount = StockBlockMaster.Details.Count;

            ////插入明细行
            //flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            ////设置单元格的边框，对齐方式
            //FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            //range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //range.Mask = FlexCell.MaskEnum.Digital;
            //CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            ////主表数据

            //flexGrid1.Cell(3, 2).Text = StockBlockMaster.Code;
            //flexGrid1.Cell(3, 4).Text = DateTime.Now.ToShortDateString();
            //flexGrid1.Cell(3, 6).Text = "";
            //flexGrid1.Cell(3, 7).Text = "合同名称：";
            //flexGrid1.Cell(4, 2).Text = StockBlockMaster.OriginalContractNo;
            //flexGrid1.Cell(4, 4).Text = "";
            //flexGrid1.Cell(4, 6).Text = StockBlockMaster.RealOperationDate.ToShortDateString();
            //flexGrid1.Cell(4, 7).Text = "承租单位：";

            ////填写明细数据
            //for (int i = 0; i < detailCount; i++)
            //{
            //    MatHireOrderDetail detail = (MatHireOrderDetail)StockBlockMaster.Details.ElementAt(i);
            //    //物资名称
            //    flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
            //    flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
            //    //规格型号
            //    flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;

            //    //结算规则
            //    flexGrid1.Cell(detailStartRowNumber + i, 3).Text = Enum.GetName(typeof(EnumMaterialHireMngBalRule), detail.RuleState);

            //    //数量
            //    flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

            //    //日租金
            //    flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

            //    //金额
            //    flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Money);

            //    //备注
            //    flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
            //    flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
            //    flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            //}
            //flexGrid1.Cell(8 + detailCount, 2).Text = StockBlockMaster.ProjectName;
            //flexGrid1.Cell(8 + detailCount, 4).Text = StockBlockMaster.CreateDate.ToShortDateString();
            //flexGrid1.Cell(8 + detailCount, 6).Text = StockBlockMaster.CreatePersonName;

        }

        public void ShowMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //public MatHireOrderMaster GetOrderMaster()
        //{
        //    ObjectQuery oQuery = new ObjectQuery();
        //    oQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Tag as SupplierRelationInfo));
        //    oQuery.AddCriterion(Expression.Eq("ProjectId", this.ProjectInfo.Id));
        //    IList list = model.MaterialHireMngSvr.GetMaterialHireOrder(oQuery) as IList;
        //    return list == null || list.Count == 0 ? null : list[0] as MatHireOrderMaster;
        //}
        public decimal GetLength(object oValue)
        {
            decimal dLen = 0;
            string sValue = string.Empty;
            if (oValue != null)
            {
                sValue = ClientUtil.ToString(oValue);
                sValue = sValue.Replace("LG", "").Replace("HG", "");
                dLen = ClientUtil.ToDecimal(sValue);
            }
            return dLen;
        }
        private void WriteLog(bool IsNew)
        {
            LogData log = new LogData();
            log.BillId =  Master.Id;
            log.BillType =this.MatHireType==EnumMatHireType.普通料具? "料具封存":(this.MatHireType==EnumMatHireType.钢管? "料具封存(钢管)":"料具封存(碗扣)");
            log.Code =  Master.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = Master.ProjectName;
            if (IsNew)
            {
                log.OperType = "新增 - " + log.BillType;
            }
            else
            {
                if (Master.DocState == DocumentState.InExecute)
                {
                    log.OperType = "提交 - " + log.BillType;

                }
                else
                {
                    log.OperType = "修改 - " + log.BillType;

                }
            }
            StaticMethod.InsertLogData(log);
        }

    }
}
