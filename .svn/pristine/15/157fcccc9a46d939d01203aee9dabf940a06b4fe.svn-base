using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng
{
    public partial class VConBalance : TMasterDetailView
    {
        MConcreteMng model = new MConcreteMng();
        private ConcreteBalanceMaster conBalanceMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        public DateTime dConCheckMasterCreateDate;

        public VConBalance()
        {

            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
        }

        private void InitEvent()
        {
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.btnForward.Click += new EventHandler(btnForward_Click);
            this.dgDelete.Click += new EventHandler(dgDelete_Click);
            this.dgDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_RowHeaderMouseClick);
        }

        void dgDetail_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgDetail.PointToClient(Cursor.Position);
                this.contextMenuStrip1.Show(dgDetail, _Point);
            }
        }
        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    conBalanceMaster.Details.Remove(dr.Tag as BaseDetail);
                    movedDtlList.Add(dr.Tag as ConcreteBalanceDetail);
                }
            }
        }
        /// <summary>
        /// 引用商品砼对账单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnForward_Click(object sender, EventArgs e)
        {
            VConCheckSelector vConSelector = new VConCheckSelector();
            vConSelector.ShowDialog();
            IList list = vConSelector.Result;
            if (list == null || list.Count == 0) return;
            ConcreteCheckingMaster ConCheckMaster = list[0] as ConcreteCheckingMaster;
            txtForwardCode.Tag = ConCheckMaster.Id;
            txtForwardCode.Text = ConCheckMaster.Code;
            txtSupply.Result.Clear();
            if (ConCheckMaster.TheSupplierRelationInfo != null)
            {
                txtSupply.Result.Add(ConCheckMaster.TheSupplierRelationInfo);
                txtSupply.Tag = ConCheckMaster.TheSupplierRelationInfo;
                txtSupply.Text = ConCheckMaster.SupplierName;
            }
            if (ConCheckMaster != null)
            {
                this.dConCheckMasterCreateDate = ConCheckMaster.CreateDate;
            }
            ////处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                ConcreteCheckingDetail dtl = dr.Tag as ConcreteCheckingDetail;
                if (dtl != null)
                {
                    if (conBalanceMaster != null)
                    {
                        conBalanceMaster.Details.Remove(dtl);
                        if (dtl.Id != null)
                        {
                            movedDtlList.Add(dtl);
                        }
                    }
                }
            }

            ////显示引用的明细
            this.dgDetail.Rows.Clear();
            decimal sumQty = 0;
            decimal sumMoney = 0;
            foreach (ConcreteCheckingDetail var in ConCheckMaster.Details)
            {
                if (var.IsSelect == false) continue;
                int i = this.dgDetail.Rows.Add();

                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colPlanQuantity.Name, i].Value = var.PlanQuantity;
                this.dgDetail[colDescript.Name, i].Value = var.Descript;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                this.dgDetail[colCheckVolume.Name, i].Value = var.BalVolume;
                this.dgDetail[colBalVolume.Name, i].Value = var.BalVolume - var.RefQuantity;
                sumQty += var.BalVolume - var.RefQuantity;
                this.dgDetail[this.colPrice.Name, i].Value = var.Price;
                if (IsAddCostDetial(var))
                {
                    this.dgDetail[this.colMoney.Name, i].Value = var.Money;
                    sumMoney += var.Money;
                    InitialAddCostDetialRow(this.dgDetail.Rows[i]);
                }
                else{
                    this.dgDetail[this.colMoney.Name, i].Value = decimal.Round((var.BalVolume - var.RefQuantity) * var.Price, 2);
                    sumMoney += decimal.Round((var.BalVolume - var.RefQuantity) * var.Price, 2);
                }

                this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                this.dgDetail[colSubjectName.Name, i].Tag = var.SubjectGUID;
                this.dgDetail[colSubjectName.Name, i].Value = var.SubjectName;
                if (var.IsPump == true)
                {
                    this.dgDetail[colIsPump.Name, i].Value = true;
                }
                else
                {
                    this.dgDetail[colIsPump.Name, i].Value = false;
                }
            }
            this.txtSumVolume.Text = sumQty + "";
            this.txtSumMoney.Text = sumMoney + "";

        }
        void InitialAddCostDetialRow(DataGridViewRow oRow)
        {
            oRow.Cells[this.colMoney.Name].ReadOnly = false;
            oRow.Cells[this.colBalVolume.Name].ReadOnly = true;
            oRow.Cells[this.colPrice.Name].ReadOnly = true;
            oRow.Cells[this.colPlanQuantity.Name].ReadOnly = true;
        }
        bool IsAddCostDetial(DataGridViewRow oRow)
        {
            object obj = oRow.Cells[colCheckVolume.Name].Value;
            if (obj == null || ClientUtil.ToString(obj) == "" || ClientUtil.ToDecimal(obj) == 0)
            {
                return true;
            }
            return false;
        }
        bool IsAddCostDetial(ConcreteCheckingDetail dtl)
        {
            if (dtl.BalVolume == 0 && dtl.Money != 0)
            {
                ConcreteBalanceDetail oConcreteBalanceDetail = model.ConcreteMngSrv.GetConcreteBalanceDetailByConcreteCheckingDetailID(dtl.Id);
                if (oConcreteBalanceDetail == null)
                {
                    return true;
                }
            }
            return false;
        }
        bool IsAddCostDetial(string ConcreteCheckingDetailID)
        {
            ConcreteCheckingDetail dtl = model.ConcreteMngSrv.GetConCkeckingDetailById(ConcreteCheckingDetailID);
            if (dtl!=null&&dtl.BalVolume == 0 && dtl.Money != 0)
            {
                ConcreteBalanceDetail oConcreteBalanceDetail = model.ConcreteMngSrv.GetConcreteBalanceDetailByConcreteCheckingDetailID(dtl.Id);
                if (oConcreteBalanceDetail == null)
                {

                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                movedDtlList = new ArrayList();
                base.NewView();
                ClearView();
                conBalanceMaster = new ConcreteBalanceMaster();
                conBalanceMaster.CreatePerson = ConstObject.LoginPersonInfo;
                conBalanceMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                conBalanceMaster.CreateDate = ConstObject.LoginDate;
                conBalanceMaster.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                conBalanceMaster.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                conBalanceMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                conBalanceMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                conBalanceMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                conBalanceMaster.HandlePerson = ConstObject.LoginPersonInfo;
                conBalanceMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                conBalanceMaster.DocState = DocumentState.Edit;


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
                    conBalanceMaster.ProjectId = projectInfo.Id;
                    conBalanceMaster.ProjectName = projectInfo.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("新建单据错误：" + ExceptionUtil.ExceptionMessage(ex));
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
            if (conBalanceMaster.DocState == DocumentState.Edit || conBalanceMaster.DocState == DocumentState.Valid)
            {
                movedDtlList = new ArrayList();
                base.ModifyView();
                conBalanceMaster = model.ConcreteMngSrv.GetConcreteBalanceMasterById(conBalanceMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(conBalanceMaster.DocState));
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
                conBalanceMaster = model.ConcreteMngSrv.SaveConcreteBalanceMaster(conBalanceMaster, movedDtlList);

                ////插入日志
                ////MStockIn.InsertLogData(conBalanceMaster.Id, "保存", conBalanceMaster.Code, conBalanceMaster.CreatePerson.Name, "商品砼结算单","");
                txtCode.Text = conBalanceMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;

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
                conBalanceMaster.DocState = DocumentState.InAudit;
                conBalanceMaster = model.ConcreteMngSrv.SaveConcreteBalanceMaster(conBalanceMaster, movedDtlList);
               // model.ConcreteMngSrv.TallyConcreteBalance(conBalanceMaster, conBalanceMaster.ProjectId);

                //conBalanceMaster = model.ConcreteMngSrv.SubmitConcreteBalanceMaster(conBalanceMaster, movedDtlList);
                //插入日志
                //MStockIn.InsertLogData(conBalanceMaster.Id, "提交", conBalanceMaster.Code, conBalanceMaster.CreatePerson.Name, "商品砼结算单","");
                txtCode.Text = conBalanceMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("提交出错。\n"+ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 取消
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
                        conBalanceMaster = model.ConcreteMngSrv.GetConcreteBalanceMasterById(conBalanceMaster.Id);
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                conBalanceMaster = model.ConcreteMngSrv.GetConcreteBalanceMasterById(conBalanceMaster.Id);

                if (conBalanceMaster.DocState == DocumentState.Valid || conBalanceMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ConcreteMngSrv.DeleteConcreteBalance(conBalanceMaster)) return false;
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(conBalanceMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
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
                conBalanceMaster = model.ConcreteMngSrv.GetConcreteBalanceMasterById(conBalanceMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = conBalanceMaster.Code;
                if (conBalanceMaster.ForwardBillCode != null)
                {
                    txtForwardCode.Text = conBalanceMaster.ForwardBillCode;
                    txtForwardCode.Tag = conBalanceMaster.ForwardBillId;
                }
                if (conBalanceMaster.HandlePerson != null)
                {
                    txtHandlePerson.Tag = conBalanceMaster.HandlePerson;
                    txtHandlePerson.Text = conBalanceMaster.HandlePersonName;
                }
                if (conBalanceMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = conBalanceMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(conBalanceMaster.TheSupplierRelationInfo);
                    txtSupply.Text = conBalanceMaster.SupplierName;
                }
                if (conBalanceMaster.CreatePerson != null)
                {
                    txtCreatePerson.Tag = conBalanceMaster.CreatePerson;
                    txtCreatePerson.Text = conBalanceMaster.CreatePersonName;
                }
                txtRemark.Text = conBalanceMaster.Descript;
               // txtCreateDate.Text = conBalanceMaster.CreateDate.ToShortDateString();
                txtProject.Text = conBalanceMaster.ProjectName;
                txtProject.Tag = conBalanceMaster.ProjectId;
                txtSumMoney.Text = conBalanceMaster.SumMoney.ToString();
                txtSumVolume.Text = conBalanceMaster.SumVolumeQuantity.ToString();

                //显示收料单明细
                this.dgDetail.Rows.Clear();

                foreach (ConcreteBalanceDetail var in conBalanceMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colCheckVolume.Name, i].Value = var.CheckingVolume;
                    this.dgDetail[colBalVolume.Name, i].Value = var.BalanceVolume;
                    this.dgDetail[colTempQuantity.Name, i].Value = var.BalanceVolume;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colPlanQuantity.Name, i].Value = var.PlanQuantity;
                    this.dgDetail[colMoney.Name, i].Value = var.Money;
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;

                    if (var.IsPump == true)
                    {
                        this.dgDetail[colIsPump.Name, i].Value = true;
                    }
                    else
                    {
                        this.dgDetail[colIsPump.Name, i].Value = false;
                    }
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    this.dgDetail[colSubjectName.Name, i].Tag = var.SubjectGUID;
                    this.dgDetail[colSubjectName.Name, i].Value = var.SubjectName;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail.Rows[i].Tag = var;

                    if (IsAddCostDetial(this.dgDetail.Rows[i]))
                    {
                       this.InitialAddCostDetialRow ( this.dgDetail.Rows[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                if (this.txtSupply.Result.Count > 0)
                {
                    conBalanceMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    conBalanceMaster.SupplierName = this.txtSupply.Text;
                }
                conBalanceMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                if (txtSumVolume.Text != "")
                    conBalanceMaster.SumVolumeQuantity = ClientUtil.ToDecimal(this.txtSumVolume.Text);
                conBalanceMaster.SumMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                conBalanceMaster.ForwardBillCode = txtForwardCode.Text;
                conBalanceMaster.ForwardBillId = ClientUtil.ToString(txtForwardCode.Tag);
                conBalanceMaster.CreateDate = ClientUtil.ToDateTime(this.txtOperDate.Value.ToShortDateString());
               // conBalanceMaster.SubmitDate = DateTime.Now;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    ConcreteBalanceDetail detail = new ConcreteBalanceDetail();
                    if (var.Tag != null)
                    {
                        detail = var.Tag as ConcreteBalanceDetail;
                        if (detail.Id == null)
                        {
                            conBalanceMaster.Details.Remove(detail);
                        }
                    }
                    //材料
                    detail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    detail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    detail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    detail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    detail.PlanQuantity = ClientUtil.ToDecimal(var.Cells[colPlanQuantity.Name].Value);
                    detail.CheckingVolume = ClientUtil.ToDecimal(var.Cells[colCheckVolume.Name].Value);
                    detail.BalanceVolume = ClientUtil.ToDecimal(var.Cells[colBalVolume.Name].Value);
                    detail.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    detail.Money = ClientUtil.ToDecimal(var.Cells[colMoney.Name].Value);
                    detail.ForwardDetailId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);

                    decimal quantityTemp = StringUtil.StrToDecimal(ClientUtil.ToString(var.Cells[colTempQuantity.Name].Value));

                    detail.TempData = quantityTemp.ToString();
                    bool IsChecked = Convert.ToBoolean(var.Cells[colIsPump.Name].EditedFormattedValue);
                    if (IsChecked == true)
                    {
                        detail.IsPump = true;
                    }
                    else
                    {
                        detail.IsPump = false;
                    }
                    detail.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                    detail.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                    if ((var.Cells[colSubjectName.Name].Tag as CostAccountSubject) != null)
                    {
                        detail.SubjectGUID = var.Cells[colSubjectName.Name].Tag as CostAccountSubject;
                        detail.SubjectName = ClientUtil.ToString(var.Cells[colSubjectName.Name].Value);
                        detail.SubjectSysCode = detail.SubjectGUID.SysCode;
                    }
                    detail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);

                    conBalanceMaster.AddDetail(detail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 数据校验
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            this.txtCode.Focus();
            string validMessage = "";
            bool bAddCostDetial = false;
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("结算单明细不能为空");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            if (txtSupply.Result.Count == 0)
            {
                validMessage += "供应商不能为空！\n";
            }

            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            //对账明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                bAddCostDetial = false;
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                if (ClientUtil.TransToDecimal(dr.Cells[colCheckVolume.Name].Value.ToString()) == 0)
                {
                }
                if (IsAddCostDetial (dr))//是否是添加费用明细
                {
                    // IsAddCostDetial(ClientUtil.ToString(dr.Cells[colForwardDtlId.Name].Value));
                    bAddCostDetial = true;
                    if (dr.Cells[colMoney.Name].Value == null)
                    {
                        MessageBox.Show("添加明细金额不能为空");
                        dr.Cells[colMoney.Name].Selected = true;

                        return false;
                    }
                }
                else
                {
                    if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                    {
                        MessageBox.Show("单价不能为空或者小于0");
                        dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                        return false;

                    }

                    //if (dr.Cells[colBalVolume.Name].Value == null || dr.Cells[colBalVolume.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colBalVolume.Name].Value) <= 0)
                    //{
                    //    MessageBox.Show("结算数量不能为空或者小于0");
                    //    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    //    return false;
                    //}

                    object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                    ConcreteCheckingDetail forwardDetail = model.ConcreteMngSrv.GetConCkeckingDetailById(forwardDtlId.ToString());
                    if (forwardDetail == null)
                    {
                        MessageBox.Show("未找到前续单据明细,请重新引用。");
                        dgDetail.CurrentCell = dr.Cells[colBalVolume.Name];
                        return false;
                    }
                    else
                    {
                        decimal canUseQty = forwardDetail.BalVolume - forwardDetail.RefQuantity;
                        decimal currentQty = decimal.Parse(dr.Cells[colBalVolume.Name].Value.ToString());
                        object qtyTempObj = dr.Cells[colTempQuantity.Name].Value;
                        decimal qtyTemp = 0;
                        if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                        {
                            qtyTemp = decimal.Parse(qtyTempObj.ToString());
                        }

                        if (currentQty - qtyTemp - canUseQty > 0)
                        {
                            MessageBox.Show("提示：输入数量【" + currentQty + "】大于可引用数量【" + (canUseQty + qtyTemp) + "】。");
                            //dgDetail.CurrentCell = dr.Cells[colBalVolume.Name];
                            //return false;
                        }
                    }
                }
            }
            DateTime oOperDate=ClientUtil .ToDateTime (this.txtOperDate .Value .ToShortDateString ());
            DateTime oDate=ClientUtil.ToDateTime(this.dConCheckMasterCreateDate.ToShortDateString ());
            if (oOperDate <oDate )
            {
                MessageBox.Show(string.Format("结算日期[{0}]应该大于对账单的业务日期[{1}]", oOperDate.ToShortDateString(), oDate.ToShortDateString ()));
                return false;
            }
            dgDetail.Update();
            return true;
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgDetail.Rows[e.RowIndex].IsNewRow)
            {
                return ;
            }
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            decimal sumMoney = 0;
            // this.dgDetail[colCheckVolume.Name, i].Value
            if (colName == colMoney.Name)
            {
                if (IsAddCostDetial(dgDetail.Rows[e.RowIndex]))
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colMoney.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                       decimal  money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colMoney.Name].Value);
                        sumMoney = sumMoney + money;
                    }
                    this.txtSumMoney.Text = sumMoney.ToString();
                }
            }
            if (colName == colBalVolume.Name || colName == colPrice.Name)
            {
                decimal dCheckVolume = decimal.Parse(dgDetail.Rows[e.RowIndex].Cells[colCheckVolume.Name].Value.ToString());
                if (dCheckVolume == 0 && IsAddCostDetial(dgDetail.Rows[e.RowIndex]))//手动添加费用明细
                {
                    dgDetail.Rows[e.RowIndex].Cells[colBalVolume.Name].Value = "0";
                    dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "0";
                    return;
                }
                else
                {
                    //汇总数量
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colBalVolume.Name].Value;
                    object ticketQuantity = dgDetail.Rows[e.RowIndex].Cells[colCheckVolume.Name].Value;
                    if (ClientUtil.ToDecimal(quantity) - ClientUtil.ToDecimal(ticketQuantity) > 0)
                    {
                        //MessageBox.Show("提示：结算数量大于对账单的数量！");
                        //return;
                    }
                    decimal sumqty = 0;

                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colBalVolume.Name].Value;
                        if (quantity == null) quantity = 0;
                        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                    }
                    txtSumVolume.Text = sumqty.ToString();

                    if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                    {
                        string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                        validity = CommonMethod.VeryValid(temp_price);
                        if (validity == false)
                            MessageBox.Show("请输入数字！");
                    }

                    object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                    object qty = dgDetail.Rows[e.RowIndex].Cells[colBalVolume.Name].Value;
                    decimal money = ClientUtil.ToDecimal(price) * ClientUtil.ToDecimal(qty);
                    this.dgDetail.Rows[e.RowIndex].Cells[colMoney.Name].Value = money;

                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colMoney.Name].Value);
                        sumMoney = sumMoney + money;
                    }
                    this.txtSumMoney.Text = sumMoney.ToString();
                }
            }
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
                    conBalanceMaster = model.ConcreteMngSrv.GetConcreteBalanceMasterById(id);
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
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            object[] os = new object[] { 10 };

            os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumVolume, txtSumMoney, txtProject };

            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            txtSupply.Result.Clear();
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
            else if (c is CommonSupplier)
            {
                c.Tag = null;
                c.Text = "";
            }
        }

        public override bool Preview()
        {
            VConBalReport vc = new VConBalReport(conBalanceMaster);
            vc.ShowDialog();
            return true;
        }
    }
}
