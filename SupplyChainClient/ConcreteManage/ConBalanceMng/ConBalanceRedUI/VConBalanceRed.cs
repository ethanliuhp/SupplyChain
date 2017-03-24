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
using System.Collections;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.ConBalanceRedUI
{
    public partial class VConBalanceRed : TMasterDetailView
    {
        private MConcreteMng model = new MConcreteMng();
        private ConcreteBalanceRedMaster conBalRedMaster;

        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空

        public VConBalanceRed()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {

        }

        private void InitEvent()
        {
            this.btnForward.Click += new EventHandler(btnForward_Click);
            dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            dgDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_RowHeaderMouseClick);
            dgDelete.Click += new EventHandler(dgDelete_Click);
        }

        void dgDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    conBalRedMaster.Details.Remove(dr.Tag as BaseDetail);
                    movedDtlList.Add(dr.Tag as ConcreteBalanceRedDetail);
                }
            }
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
                conBalRedMaster = new ConcreteBalanceRedMaster();
                conBalRedMaster.CreatePerson = ConstObject.LoginPersonInfo;
                conBalRedMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                conBalRedMaster.CreateDate = ConstObject.LoginDate;
                conBalRedMaster.CreateYear = ConstObject.LoginDate.Year;
                conBalRedMaster.CreateMonth = ConstObject.LoginDate.Month;
                conBalRedMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                conBalRedMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                conBalRedMaster.HandlePerson = ConstObject.LoginPersonInfo;
                conBalRedMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                conBalRedMaster.DocState = DocumentState.Edit;


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
                    conBalRedMaster.ProjectId = projectInfo.Id;
                    conBalRedMaster.ProjectName = projectInfo.Name;
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                conBalRedMaster = model.ConcreteMngSrv.SaveConcreteBalanceRedMaster(conBalRedMaster, movedDtlList);

                ////插入日志
                ////MStockIn.InsertLogData(conBalRedMaster.Id, "保存", conBalRedMaster.Code, conBalRedMaster.CreatePerson.Name, "商品砼结算红单","");
                txtCode.Text = conBalRedMaster.Code;
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
                conBalRedMaster.DocState = DocumentState.InAudit;

                conBalRedMaster = model.ConcreteMngSrv.SaveConcreteBalanceRedMaster(conBalRedMaster, movedDtlList);
                model.ConcreteMngSrv.TallyConcreteBalanceRed(conBalRedMaster,conBalRedMaster.ProjectId);
                //插入日志
                //MStockIn.InsertLogData(conBalRedMaster.Id, "提交", conBalRedMaster.Code, conBalRedMaster.CreatePerson.Name, "商品砼结算红单","");
                txtCode.Text = conBalRedMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
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
                        conBalRedMaster = model.ConcreteMngSrv.GetConcreteBalanceRedMasterById(conBalRedMaster.Id);
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
                conBalRedMaster = model.ConcreteMngSrv.GetConcreteBalanceRedMasterById(conBalRedMaster.Id);

                if (conBalRedMaster.DocState == DocumentState.Valid || conBalRedMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ConcreteMngSrv.DeleteConcreteBalanceRed(conBalRedMaster)) return false;
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(conBalRedMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (conBalRedMaster.DocState == DocumentState.Edit || conBalRedMaster.DocState == DocumentState.Valid)
            {
                movedDtlList = new ArrayList();
                base.ModifyView();
                conBalRedMaster = model.ConcreteMngSrv.GetConcreteBalanceRedMasterById(conBalRedMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(conBalRedMaster.DocState));
            MessageBox.Show(message);
            return false;
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                conBalRedMaster = model.ConcreteMngSrv.GetConcreteBalanceRedMasterById(conBalRedMaster.Id);
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
                this.txtCode.Text = conBalRedMaster.Code;
                if (conBalRedMaster.ForwardBillCode != null)
                {
                    txtForwardCode.Text = conBalRedMaster.ForwardBillCode;
                    txtForwardCode.Tag = conBalRedMaster.ForwardBillId;
                }
                if (conBalRedMaster.HandlePerson != null)
                {
                    txtHandlePerson.Tag = conBalRedMaster.HandlePerson;
                    txtHandlePerson.Text = conBalRedMaster.HandlePersonName;
                }
                if (conBalRedMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = conBalRedMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(conBalRedMaster.TheSupplierRelationInfo);
                    txtSupply.Text = conBalRedMaster.TheSupplierRelationInfo.SupplierInfo.Name;
                }
                if (conBalRedMaster.CreatePerson != null)
                {
                    txtCreatePerson.Tag = conBalRedMaster.CreatePerson;
                    txtCreatePerson.Text = conBalRedMaster.CreatePerson.Name;
                }
                txtRemark.Text = conBalRedMaster.Descript;
                txtCreateDate.Text = conBalRedMaster.CreateDate.ToShortDateString();
                txtProject.Text = conBalRedMaster.ProjectName;
                txtProject.Tag = conBalRedMaster.ProjectId;
                txtSumMoney.Text = Math.Abs(conBalRedMaster.SumMoney).ToString();
                txtSumVolume.Text = Math.Abs(conBalRedMaster.SumVolumeQuantity).ToString();

                //显示收料单明细
                this.dgDetail.Rows.Clear();

                foreach (ConcreteBalanceRedDetail var in conBalRedMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                    this.dgDetail[colQuantity.Name, i].Value = Math.Abs(var.BalanceVolume);
                    this.dgDetail[colQuantityTemp.Name, i].Value = Math.Abs(var.BalanceVolume);
                    this.dgDetail[colConCheckQuantity.Name, i].Value = var.CheckingVolume;

                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colMoney.Name, i].Value = Math.Abs(var.Money);
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;

                    if (var.UsedPart != null)
                        this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;

                    if (var.IsPump == true)
                    {
                        this.dgDetail[colIsPump.Name, i].Value = true;
                    }
                    else
                    {
                        this.dgDetail[colIsPump.Name, i].Value = false;
                    }
                    this.dgDetail.Rows[i].Tag = var;
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
                    conBalRedMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    conBalRedMaster.SupplierName = this.txtSupply.Text;
                }
                conBalRedMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                if (txtSumVolume.Text != "")
                    conBalRedMaster.SumVolumeQuantity = -ClientUtil.ToDecimal(this.txtSumVolume.Text);
                conBalRedMaster.SumMoney = -ClientUtil.ToDecimal(this.txtSumMoney.Text);
                conBalRedMaster.ForwardBillCode = txtForwardCode.Text;
                conBalRedMaster.ForwardBillId = ClientUtil.ToString(txtForwardCode.Tag);

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    ConcreteBalanceRedDetail detail = new ConcreteBalanceRedDetail();
                    if (var.Tag != null)
                    {
                        detail = var.Tag as ConcreteBalanceRedDetail;
                        if (detail.Id == null)
                        {
                            conBalRedMaster.Details.Remove(detail);
                        }
                    }
                    //材料
                    detail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    detail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    detail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    detail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    detail.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    detail.Money = -ClientUtil.ToDecimal(var.Cells[colMoney.Name].Value);
                    detail.ForwardDetailId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);
                    detail.CheckingVolume = ClientUtil.ToDecimal(var.Cells[colConCheckQuantity.Name].Value);

                    decimal quantityTemp = StringUtil.StrToDecimal(ClientUtil.ToString(var.Cells[colQuantity.Name].Value));
                    detail.BalanceVolume = -quantityTemp;

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
                    detail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);

                    conBalRedMaster.AddDetail(detail);
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
        /// 校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {

            string validMessage = "";
            if (this.dgDetail.Rows.Count == 0)
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
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }

                if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                {
                    MessageBox.Show("单价不能为空或者小于0");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }

                if (dr.Cells[colQuantity.Name].Value == null || dr.Cells[colQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colQuantity.Name].Value) <= 0)
                {
                    MessageBox.Show("冲红数量不能为空或者小于0");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }

                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                ConcreteBalanceDetail forwardDetail = model.ConcreteMngSrv.GetConBalDetailById(forwardDtlId.ToString());
                if (forwardDetail == null)
                {
                    MessageBox.Show("未找到前续单据明细,请重新引用。");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }
                else
                {
                    decimal canUseQty = forwardDetail.BalanceVolume - forwardDetail.RefQuantity;
                    decimal currentQty = decimal.Parse(dr.Cells[colQuantity.Name].Value.ToString());
                    object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;
                    decimal qtyTemp = 0;
                    if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                    {
                        qtyTemp = decimal.Parse(qtyTempObj.ToString());
                    }

                    if (currentQty - qtyTemp - canUseQty > 0)
                    {
                        MessageBox.Show("输入数量【" + currentQty + "】大于可引用数量【" + (canUseQty + qtyTemp) + "】。");
                        dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                        return false;
                    }
                }
            }
            dgDetail.Update();
            return true;
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            decimal sumMoney = 0;
            if (colName == colQuantity.Name || colName == colPrice.Name)
            {
                //汇总数量
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
                decimal sumqty = 0;
                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
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
                object qty = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
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

        /// <summary>
        /// 引用前驱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnForward_Click(object sender, EventArgs e)
        {
            VConBalanceSelector vConBalanceSelector = new VConBalanceSelector();
            vConBalanceSelector.ShowDialog();
            IList list = vConBalanceSelector.Result;
            if (list == null || list.Count == 0) return;
            ConcreteBalanceMaster ConBalMaster = list[0] as ConcreteBalanceMaster;
            txtForwardCode.Tag = ConBalMaster.Id;
            txtForwardCode.Text = ConBalMaster.Code;
            txtSupply.Result.Clear();
            if (ConBalMaster.TheSupplierRelationInfo != null)
            {
                txtSupply.Result.Add(ConBalMaster.TheSupplierRelationInfo);
                txtSupply.Tag = ConBalMaster.TheSupplierRelationInfo;
                txtSupply.Text = ConBalMaster.SupplierName;
            }

            ////处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                ConcreteBalanceDetail dtl = dr.Tag as ConcreteBalanceDetail;
                if (dtl != null)
                {
                    if (ConBalMaster != null)
                    {
                        ConBalMaster.Details.Remove(dtl);
                        if (dtl.Id != null)
                        {
                            movedDtlList.Add(dtl);
                        }
                    }
                }
            }

            ////显示引用的明细
            this.dgDetail.Rows.Clear();
            foreach (ConcreteBalanceDetail var in ConBalMaster.Details)
            {
                if (var.IsSelect == false) continue;
                int i = this.dgDetail.Rows.Add();

                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colDescript.Name, i].Value = var.Descript;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                this.dgDetail[colPrice.Name, i].Value = var.Price;
                this.dgDetail[colQuantity.Name, i].Value = (var.BalanceVolume - var.RefQuantity).ToString("############.####");
                this.dgDetail[colCanUseQty.Name, i].Value = (var.BalanceVolume - var.RefQuantity).ToString("############.####");
                this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                this.dgDetail[colConCheckQuantity.Name, i].Value = var.CheckingVolume;
                if (var.IsPump == true)
                {
                    this.dgDetail[colIsPump.Name, i].Value = true;
                }
                else
                {
                    this.dgDetail[colIsPump.Name, i].Value = false;
                }
                dgDetail_CellEndEdit(sender, new DataGridViewCellEventArgs(5, i));
            }
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
                    conBalRedMaster = model.ConcreteMngSrv.GetConcreteBalanceRedMasterByCode(code);
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
    }
}
