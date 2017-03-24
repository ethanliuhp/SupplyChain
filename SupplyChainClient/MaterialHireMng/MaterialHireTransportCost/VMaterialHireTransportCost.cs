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
 
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireTransportCost.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireTransportCost
{
    public partial class VMaterialHireTransportCost : TMasterDetailView
    {

 
        private MMaterialHireMng model = new MMaterialHireMng();
        private MatHireTranCostMaster Master;
        //CurrentProjectInfo ProjectInfo;
        MatHireOrderMaster OrderMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空   
        public VMaterialHireTransportCost( )
        {
            InitializeComponent();
 
            InitEvent();
            InitData();
        }

        public void InitData()
        {
           
        }

        private void InitEvent()
        {
            //this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
           // this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
           // this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            this.btnForward.Click += new EventHandler(btnForward_Click);
        }
        void btnForward_Click(object sender, EventArgs e)
        {
       
            VMaterialHireOrderSelector oVMaterialHireOrderSelector = new VMaterialHireOrderSelector(EnumMatHireType.其他);
            oVMaterialHireOrderSelector.ShowDialog();
            if (oVMaterialHireOrderSelector.Result != null && oVMaterialHireOrderSelector.Result.Count > 0)
            {
                OrderMaster = oVMaterialHireOrderSelector.Result[0] as MatHireOrderMaster;
                #region 数据填充
                txtSupply.Text = OrderMaster.SupplierName;
                txtSupply.Tag = OrderMaster.TheSupplierRelationInfo;
                txtContract.Tag = OrderMaster;
                txtContract.Text = OrderMaster.Code;
                txtProjectName.Text = OrderMaster.ProjectName;
                txtProjectName.Tag = OrderMaster.ProjectId;
                #endregion
            }
        }
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
                    Master = model.MaterialHireMngSvr.GetMatHireTranCostMasterById(id);
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
            object[] os = new object[] { txtCode, txtCreatePerson ,txtSumMoney, txtSupply,txtContract,txtProjectName};
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { };
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
                this.Master = new  MatHireTranCostMaster();
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
                Master.BalState = 0;
           
                //制单日期
                //txtCreateDate1.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                
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
                Master = model.MaterialHireMngSvr.GetMatHireTranCostMasterById(Master.Id);
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
                Master = model.MaterialHireMngSvr.SaveMatHireTranCostMaster(Master);
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

                Master = model.MaterialHireMngSvr.SaveMatHireTranCostMaster(Master);
         
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
                Master = model.MaterialHireMngSvr.GetMatHireTranCostMasterById(Master.Id);
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
                        Master = model.MaterialHireMngSvr.GetMatHireTranCostMasterById(Master.Id);
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
                Master = model.MaterialHireMngSvr.GetMatHireTranCostMasterById(Master.Id);
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
            if (string.IsNullOrEmpty(txtContract.Text) || txtContract.Tag == null)
            {
                MessageBox.Show("请选择采购合同");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (dr.Cells[colTranMoney.Name].Value == null || ClientUtil.ToDecimal(dr.Cells[colTranMoney.Name].Value)<=0)
                {
                    ShowMessage("[运输费]必须大于0");
                    return false;
                }
                if (dr.Cells[this.colDispatchMoney.Name].Value == null || ClientUtil.ToDecimal(dr.Cells[colDispatchMoney.Name].Value) <= 0)
                {
                    ShowMessage("[配送费]必须大于0");
                    return false;
                }
                
            }
           
            dgDetail.Update();
            //dgBasicCostDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            MatHireTranCostDetail curDetail=null;
            if (!ValidView()) return false;
            try
            {
                Master.CreateDate = this.dtpCreateDate.Value;
                Master.TransportTime = Master.CreateDate;
               // Master.TransportTime = this.dtpTransportTime.Value;
                Master.ContractCode = txtContract.Text;
                Master.Contract = txtContract.Tag as MatHireOrderMaster;
                Master.SupplierName = txtSupply.Text;
                Master.TheSupplierRelationInfo = txtSupply.Tag as SupplierRelationInfo;
                Master.ProjectId =ClientUtil.ToString( txtProjectName.Tag);
                Master.ProjectName = txtProjectName.Text;
                Master.Descript = txtRemark.Text;
                Master.BillCode = txtOldCode.Text;
                Master.Details.Clear();
                Master.SumMoney = 0;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    if (var.Tag != null)
                    {
                        curDetail = var.Tag as MatHireTranCostDetail;
                        if (curDetail.Id == null)
                        {
                            Master.Details.Remove(curDetail);
                        }
                    }
                    else
                    {
                        curDetail = new MatHireTranCostDetail();
                    }
                    curDetail.Master = Master;
                    Master.AddDetail(curDetail);
                    curDetail.DispatchMoney = ClientUtil.ToDecimal(var.Cells[colDispatchMoney.Name].Value);
                    curDetail.TransportMoney = ClientUtil.ToDecimal(var.Cells[this.colTranMoney.Name].Value);
                    curDetail.Descript = ClientUtil.ToString(var.Cells[this.colDescript.Name].Value);
                    Master.SumMoney += curDetail.DispatchMoney + curDetail.TransportMoney;
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
                dtpCreateDate.Value = Master.CreateDate;
                //dtpTransportTime.Value = Master.TransportTime;
              
                txtRemark.Text = Master.Descript;

                txtCreatePerson.Tag = Master.CreatePerson;
                txtCreatePerson.Text = Master.CreatePersonName;
                txtContract.Text = Master.ContractCode;
                txtContract.Tag = Master.Contract;
                this.txtProjectName.Tag = Master.ProjectId;
                this.txtProjectName.Text = Master.ProjectName;
                txtSupply.Text = Master.SupplierName;
                txtSupply.Tag = Master.TheSupplierRelationInfo;
                txtSumMoney.Text = Master.SumMoney.ToString("N2");
                txtOldCode.Text = Master.BillCode;

                this.dgDetail.Rows.Clear();
                foreach (MatHireTranCostDetail var in Master.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colDispatchMoney.Name, i].Value = var.DispatchMoney;
                    this.dgDetail[colTranMoney.Name, i].Value = var.TransportMoney;
                    this.dgDetail[this.colDescript.Name, i].Value = var.Descript;
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


        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool IsChecked = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == this.colDispatchMoney.Name || this.colTranMoney.Name == colName)
            {
                if (colName == colDispatchMoney.Name)
                {
                    if (dgDetail.Rows[e.RowIndex].Cells[colDispatchMoney.Name].Value != null)
                    {
                        string temp_Quantity = dgDetail.Rows[e.RowIndex].Cells[colDispatchMoney.Name].Value.ToString();
                        validity = CommonMethod.VeryValid(temp_Quantity);
                        if (validity == false)
                        {
                            MessageBox.Show("请输入数字！");
                            dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colDispatchMoney.Name];
                            dgDetail.BeginEdit(true);
                        }
                    }
                }
                else if (colTranMoney.Name == colName)
                {
                    if (dgDetail.Rows[e.RowIndex].Cells[colTranMoney.Name].Value != null)
                    {
                        string temp_Quantity = dgDetail.Rows[e.RowIndex].Cells[colTranMoney.Name].Value.ToString();
                        validity = CommonMethod.VeryValid(temp_Quantity);
                        if (validity == false)
                        {
                            MessageBox.Show("请输入数字！");
                            dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[colTranMoney.Name];
                            dgDetail.BeginEdit(true);
                        }
                    }
                }

                SumMoney();
            }
        }
        public void SumMoney()
        {
            decimal dSumMoney = 0;
            foreach (DataGridViewRow oRow in dgDetail.Rows)
            {
                dSumMoney += ClientUtil.ToDecimal(oRow.Cells[colTranMoney.Name].Value) + ClientUtil.ToDecimal(oRow.Cells[this.colDispatchMoney.Name].Value);
            }
            txtSumMoney.Text = string.Format("{0:N2}", dSumMoney);
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
             
            return true;
        }

        public override bool Print()
        {
            
            return true;
        }

        public override bool Export()
        {
            
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            
            return true;
        }

        private void FillFlex(MatHireStockBlockMaster StockBlockMaster)
        {
            
        }

        public void ShowMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
         
         
        private void WriteLog(bool IsNew)
        {
            LogData log = new LogData();
            log.BillId =  Master.Id;
            log.BillType = "运输单";
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
