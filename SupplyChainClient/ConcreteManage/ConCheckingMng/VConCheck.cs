using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng
{
    public partial class VConCheck : TMasterDetailView
    {
        MConcreteMng model = new MConcreteMng();
        private ConcreteCheckingMaster conCheckMaster;
        private IList lst_ConPouringNoteDtl = new ArrayList();

        public VConCheck()
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
            this.btnCreateCheck.Click += new EventHandler(btnCreateCheck_Click);
            this.txtSupply.ValueChanged += new EventHandler(txtSupply_ValueChanged);
            txtAdjustmentMoney.tbTextChanged += new EventHandler(txtAdjustmentMoney_tbTextChanged);
            dgDelete.Click += new EventHandler(dgDelete_Click);
            //dgDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_RowHeaderMouseClick);
            this.AddCostDetial.Click += new EventHandler(AddCostDetial_Click);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
             
        }

        void dgDetail_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgDetail.PointToClient(Cursor.Position);
               // this.contextMenuStrip1.Show(dgDetail, _Point);
            }
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
                    conCheckMaster.Details.Remove(dr.Tag as ConcreteCheckingDetail);
                }
            }
        }
        void AddCostDetial_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要添加一条费用明细记录吗？", "添加记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                bool IsExistNew = false;
                DataGridViewRow oRowEmpty = null;
                foreach (DataGridViewRow oRow in this.dgDetail.Rows)
                {
                    if (oRow.IsNewRow)
                    {
                        IsExistNew = true;
                        oRowEmpty = oRow;
                    }
                }
                if (!IsExistNew)
                {
                  int iRow= this.dgDetail.Rows.Add();
                  oRowEmpty = this.dgDetail.Rows[iRow];
                }
                IntialRow(oRowEmpty);
                //oRowEmpty.Cells[colBalVolume.Name].Value = "0";
            }
        }
        /// <summary>
        /// 是否是是添加的费用明细的行
        /// </summary>
        /// <param name="oRow"></param>
        /// <returns></returns>
        bool IsAddDetialRow(DataGridViewRow oRow)
        {
            object obj = oRow.Cells[colTicketVolume.Name].Value;
             if (obj == null || string.Equals(obj.ToString(), "0"))
             {
                 return true ;
             }
             else
             {
                 return false;
             }
        }
        void IntialRow(DataGridViewRow oRowEmpty)
        {
            object obj = oRowEmpty.Cells[colMoney.Name].Value;
            if (obj == null)
            {
                oRowEmpty.Cells[colTicketVolume.Name].Value = "0";
                oRowEmpty.Cells[colTicketVolume.Name].ReadOnly = true;

                oRowEmpty.Cells[colDeductionVolume.Name].Value = "0";
                oRowEmpty.Cells[colDeductionVolume.Name].ReadOnly = true;

                oRowEmpty.Cells[this.colLessPumpVolume.Name].Value = "0";
                oRowEmpty.Cells[colLessPumpVolume.Name].ReadOnly = true;

                oRowEmpty.Cells[colBalVolume.Name].Value = "0";
                oRowEmpty.Cells[colBalVolume.Name].ReadOnly = true;

                oRowEmpty.Cells[colPrice.Name].Value = "0";
                oRowEmpty.Cells[colPrice.Name].ReadOnly = true;

                oRowEmpty.Cells[colMoney.Name].Value = "0";

                oRowEmpty.Cells[colMoney.Name].ReadOnly = false; 
           
            }
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (IsAddDetialRow(this.dgDetail.CurrentRow))//判断是否是新添加的费用明细
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))//物资
                {
                    if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
                    {
                       
                        CommonMaterial materialSelector = new CommonMaterial();
                        materialSelector.materialCatCode = "I112";
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

                            theCurrentRow.Cells[colMaterialCode.Name].Tag = theMaterial;
                            theCurrentRow.Cells[colMaterialCode.Name].Value = theMaterial.Code;
                            theCurrentRow.Cells[colMaterialName.Name].Value = theMaterial.Name;
                            theCurrentRow.Cells[colMaterialSpec.Name].Value = theMaterial.Specification;
                         
                            IntialRow(theCurrentRow);
                            break;
                        }
                        
                    }
                    
                }

                else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
                {
                    
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        TreeNode root = frm.SelectResult[0];

                        GWBSTree task = root.Tag as GWBSTree;
                        if (task != null)
                        {
                            theCurrentRow.Cells[colUsedPart.Name].Value = task.Name;
                            theCurrentRow.Cells[colUsedPart.Name].Tag = task;
                            this.txtCode.Focus();
                        }
                        IntialRow(theCurrentRow);
                    }
                    

                }
                else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSubjectName.Name))
                {
                    //双击核算科目
                    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                    frm.ShowDialog();
                    CostAccountSubject cost = frm.SelectAccountSubject;
                    if (cost != null)
                    {
                        if (theCurrentRow.Cells[colMaterialCode.Name].Value != null)
                        {
                            theCurrentRow.Cells[colSubjectName.Name].Tag = cost;
                            theCurrentRow.Cells[colSubjectName.Name].Value = cost.Name;
                        }
                        IntialRow(theCurrentRow);
                    }
                    this.txtCode.Focus();
                    
                }
            }
        }
        void txtAdjustmentMoney_tbTextChanged(object sender, EventArgs e)
        {
            object money = txtAdjustmentMoney.Text;
            bool validity = CommonMethod.VeryValid(money.ToString());
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                this.txtAdjustmentMoney.Text = "";
            }
            else
            {
                if (money == null) money = 0;
                decimal sumMoney = 0;
                //计算总金额
                txtSumMoney.Text = ClientUtil.ToString(ClientUtil.ToDecimal(txtSumMoney.Text) + ClientUtil.ToDecimal(money));
                foreach (DataGridViewRow var in dgDetail.Rows)
                {
                    object money_temp = var.Cells[colMoney.Name].Value;
                    if (money_temp == null) money_temp = 0;
                    sumMoney = sumMoney + ClientUtil.ToDecimal(money_temp);
                }
                if (sumMoney > 0)
                {
                    txtSumMoney.Text = ClientUtil.ToString(sumMoney + ClientUtil.ToDecimal(money));
                }
            }
        }

        void txtSupply_ValueChanged(object sender, EventArgs e)
        {
            ///*查询商品砼对账单的最后一次日期
            //当前对账单的开始日期从此日期开始*/
            //lastEndDate = model.ConcreteMngSrv.GetLastEndDate(txtSupply.Result[0] as SupplierRelationInfo);
            //if (lastEndDate == "")
            //{
            //    this.dtpDateBegin.Enabled = true;
            //    this.dtpDateEnd.Value = DateTime.Now.Date;
            //}
            //else
            //{
            //    this.dtpDateBegin.Enabled = false;
            //    dtpDateBegin.Value = Convert.ToDateTime(lastEndDate).AddDays(1);
            //    this.dtpDateEnd.Value = DateTime.Now.Date;
            //}
            //this.dtpDateBegin.Enabled = false;
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
                conCheckMaster = new ConcreteCheckingMaster();
                conCheckMaster.CreatePerson = ConstObject.LoginPersonInfo;
                conCheckMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                conCheckMaster.CreateDate = ConstObject.LoginDate;
                conCheckMaster.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                conCheckMaster.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                conCheckMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                conCheckMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                conCheckMaster.HandlePerson = ConstObject.LoginPersonInfo;
                conCheckMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                conCheckMaster.DocState = DocumentState.Edit;


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
                    conCheckMaster.ProjectId = projectInfo.Id;
                    conCheckMaster.ProjectName = projectInfo.Name;
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
            //if (txtSupply.Result.Count > 0)
            //{
            //    DateTime LastDate = Convert.ToDateTime(model.ConcreteMngSrv.GetLastEndDate(txtSupply.Result[0] as SupplierRelationInfo));
            //    if (conCheckMaster.EndDate < LastDate)
            //    {
            //        MessageBox.Show("当前对账单的结束日期后存在其他对账单，不能修改");
            //        return false;
            //    }
            //}

            if (conCheckMaster.DocState == DocumentState.Edit || conCheckMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                conCheckMaster = model.ConcreteMngSrv.GetConCheckingMasterById(conCheckMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(conCheckMaster.DocState));
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
                conCheckMaster.DocState = DocumentState.InExecute;
                conCheckMaster = model.ConcreteMngSrv.SaveConcreteCheck(conCheckMaster);

                ////插入日志
                ////MStockIn.InsertLogData(conCheckMaster.Id, "保存", conCheckMaster.Code, conCheckMaster.CreatePerson.Name, "商品砼对账单","");
                txtCode.Text = conCheckMaster.Code;
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                conCheckMaster = model.ConcreteMngSrv.GetConCheckingMasterById(conCheckMaster.Id);
                DateTime LastDate = Convert.ToDateTime(model.ConcreteMngSrv.GetLastEndDate(txtSupply.Result[0] as SupplierRelationInfo));

                if (conCheckMaster.EndDate < LastDate)
                {
                    MessageBox.Show("当前对账单的结束日期后存在其他对账单，不能删除");
                    return false;
                }

                if (conCheckMaster.DocState == DocumentState.Valid || conCheckMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ConcreteMngSrv.DeleteConCheckMaster(conCheckMaster)) return false;
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(conCheckMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
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
                        conCheckMaster = model.ConcreteMngSrv.GetConCheckingMasterById(conCheckMaster.Id);
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
                conCheckMaster = model.ConcreteMngSrv.GetConCheckingMasterById(conCheckMaster.Id);
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
                this.txtCode.Text = conCheckMaster.Code;
                 
                if (conCheckMaster.HandlePerson != null)
                {
                    txtHandlePerson.Tag = conCheckMaster.HandlePerson;
                    txtHandlePerson.Text = conCheckMaster.HandlePersonName;
                }
                if (conCheckMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = conCheckMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(conCheckMaster.TheSupplierRelationInfo);
                    txtSupply.Value = conCheckMaster.SupplierName;
                }
                if (conCheckMaster.CreatePerson != null)
                {
                    txtCreatePerson.Tag = conCheckMaster.CreatePerson;
                    txtCreatePerson.Text = conCheckMaster.CreatePersonName;
                }
                txtRemark.Text = conCheckMaster.Descript;
                txtCreateDate.Text = conCheckMaster.CreateDate.ToShortDateString();
                txtProject.Text = conCheckMaster.ProjectName;
                txtProject.Tag = conCheckMaster.ProjectId;
                txtSumVolume.Text = conCheckMaster.SumVolume.ToString();
                cmoCreateTime.Value = conCheckMaster.CreateDate;
                txtSumMoney.Text = conCheckMaster.SumMoney.ToString();
                txtAdjustmentMoney.Text = ClientUtil.ToString(conCheckMaster.AdjustmentMoney);
                dtpDateEnd.Value = conCheckMaster.EndDate;

                //显示收料单明细
                this.dgDetail.Rows.Clear();

                foreach (ConcreteCheckingDetail var in conCheckMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;

                    this.dgDetail[colTicketVolume.Name, i].Value = var.TicketVolume;
                    this.dgDetail[colDeductionVolume.Name, i].Value = var.DeductionVolume;
                    this.dgDetail[this.colLessPumpVolume.Name, i].Value = var.LessPumpVolume;
                    this.dgDetail[colBalVolume.Name, i].Value = var.BalVolume;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colMoney.Name, i].Value = var.Money;

                    if (var.IsPump == true)
                    {
                        this.dgDetail[colIsPump.Name, i].Value = true;
                    }
                    else
                    {
                        this.dgDetail[colIsPump.Name, i].Value = false;
                    }
                    
                    if (var.SubjectGUID != null)
                    {
                        this.dgDetail[colSubjectName.Name, i].Tag = var.SubjectGUID;
                        this.dgDetail[colSubjectName.Name, i].Value = var.SubjectName;
                    }
                    if (var.UsedPart != null)
                    {
                        this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                        this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    }
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
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
                conCheckMaster.CreateDate = ConstObject.LoginDate;
                if (this.txtSupply.Result.Count > 0)
                {
                    conCheckMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    conCheckMaster.SupplierName = this.txtSupply.Text;
                }
                conCheckMaster.EndDate = this.dtpDateEnd.Value.Date;
                conCheckMaster.CreateDate=this.cmoCreateTime.Value.Date;
                conCheckMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                conCheckMaster.SumVolume = ClientUtil.ToDecimal(this.txtSumVolume.Text);
                conCheckMaster.SumMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                conCheckMaster.AdjustmentMoney = ClientUtil.ToDecimal(txtAdjustmentMoney.Text);

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    ConcreteCheckingDetail detail = new ConcreteCheckingDetail();
                    if (var.Tag != null)
                    {
                        detail = var.Tag as ConcreteCheckingDetail;
                        if (detail.Id == null)
                        {
                            conCheckMaster.Details.Remove(detail);
                        }
                    }
                   

                    //材料
                    detail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    detail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    detail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    detail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    detail.PlanQuantity = ClientUtil.ToDecimal(var.Cells[colPlanQuantity.Name].Value);
                    detail.TicketVolume = ClientUtil.ToDecimal(var.Cells[colTicketVolume.Name].Value);
                    detail.DeductionVolume = ClientUtil.ToDecimal(var.Cells[colDeductionVolume.Name].Value);
                    detail.LessPumpVolume = ClientUtil.ToDecimal(var.Cells[this.colLessPumpVolume.Name].Value);
                    detail.BalVolume = ClientUtil.ToDecimal(var.Cells[colBalVolume.Name].Value);
                    detail.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    detail.Money = ClientUtil.ToDecimal(var.Cells[colMoney.Name].Value);
                    detail.TempData = ClientUtil.ToString(var.Cells[colTempData.Name].Value);
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
                    detail.SubjectGUID = var.Cells[colSubjectName.Name].Tag as CostAccountSubject;
                    detail.SubjectName = ClientUtil.ToString(var.Cells[colSubjectName.Name].Value);
                    detail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);

                    conCheckMaster.AddDetail(detail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }

        private bool ValidView()
        {
            string validMessage = "";
            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("对账明细不能为空");
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

                if (dr.Cells[colTicketVolume.Name].Value == null)
                {
                    MessageBox.Show("小票方量不能为空或小于0");
                    dgDetail.CurrentCell = dr.Cells[colTicketVolume.Name];
                    return false;
                }
                if (dr.Cells[colPrice.Name].Value == null)
                {
                    MessageBox.Show("单价不能为空或者小于0");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }
                //是否是添加费用明细
                if (IsAddDetialRow(dr ))
                {
                    string sMsg = string.Empty;
                    if (dr.Cells[colMoney.Name].Value == null  )
                    {
                       
                        MessageBox.Show("添加明细金额不能为空");
                        dr.Cells[colMoney.Name].Selected = true;

                        return false;
                    }
                    if (dr.Cells[colUsedPart.Name].Tag == null)
                    {
                        MessageBox.Show("请选择使用部位");
                        return false;
                    }
                    if (dr.Cells[colSubjectName.Name].Tag == null)
                    {
                        MessageBox.Show("请选择核算科目");
                        return false;
                    }

                }
            }
            dgDetail.Update();
            return true;
        }

        /// <summary>
        /// 生成对账单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCreateCheck_Click(object sender, EventArgs e)
        {
            lst_ConPouringNoteDtl = new ArrayList();
            this.dgDetail.Rows.Clear();
            IList list_conNotePump = new ArrayList();//泵送浇筑记录
            IList list_conNoteNotPump = new ArrayList();//非泵送浇筑记录
            IList list_conNote = new ArrayList();

            if (this.txtSupply.Text == "" && this.txtSupply.Tag == null)
            {
                MessageBox.Show("请选择供应商！");
                return;
            }
            ObjectQuery oq = new ObjectQuery();
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            oq.AddCriterion(Expression.Lt("PouringDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("ConcreteCheckState", 0));
            if (this.txtSupply.Text != "" && this.txtSupply.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("Master.TheSupplierRelationInfo.Id", (this.txtSupply.Result[0] as SupplierRelationInfo).Id));
            }
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("UsedPart", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("SubjectGUID", NHibernate.FetchMode.Eager);
            lst_ConPouringNoteDtl = model.ConcreteMngSrv.GetPouringNoteDetail(oq);
            if (lst_ConPouringNoteDtl.Count == 0)
            {
                MessageBox.Show("当前时间段内所有浇筑记录都已生成对账单！");
                return;
            }

            foreach (PouringNoteDetail detail in lst_ConPouringNoteDtl)
            {
                if (detail.IsPump == true)
                {
                    //泵送
                    list_conNotePump.Add(detail);
                }
                else
                {
                    //非泵送
                    list_conNoteNotPump.Add(detail);
                }

            }

            //根据规格和使用部位汇总数量
            list_conNotePump = this.ArrangeList(list_conNotePump);
            list_conNoteNotPump = this.ArrangeList(list_conNoteNotPump);
            if (list_conNotePump.Count > 0 || list_conNoteNotPump.Count > 0)
            {

                //汇总全部泵送和非泵送商品砼
                if (list_conNotePump.Count > 0)
                {
                    foreach (PouringNoteDetail detail in list_conNotePump)
                    {
                        list_conNote.Add(detail);
                    }
                }
                if (list_conNoteNotPump.Count > 0)
                {
                    foreach (PouringNoteDetail detail in list_conNoteNotPump)
                    {
                        list_conNote.Add(detail);
                    }
                }
                foreach (PouringNoteDetail detail in list_conNote)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Tag = detail.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = detail.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = detail.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = detail.MaterialSpec;
                    this.dgDetail[colTicketVolume.Name, i].Value = detail.Quantity;
                    this.dgDetail[colBalVolume.Name, i].Value = detail.Quantity;
                    this.dgDetail[this.colPrice.Name, i].Value = detail.Price;
                    this.dgDetail[this.colMoney.Name, i].Value = detail.Money;
                    this.dgDetail[colPlanQuantity.Name, i].Value = detail.PlanQuantity;
                    //浇筑部位
                    if (detail.UsedPart != null)
                    {
                        this.dgDetail[colUsedPart.Name, i].Tag = detail.UsedPart;
                        this.dgDetail[colUsedPart.Name, i].Value = detail.UsedPartName;
                    }
                    //核算科目
                    if (detail.SubjectName != null)
                    {
                        this.dgDetail[colSubjectName.Name, i].Tag = detail.SubjectGUID;
                        this.dgDetail[colSubjectName.Name, i].Value = detail.SubjectName;
                    }
                    this.dgDetail[this.colDescript.Name, i].Value = detail.Descript;
                    this.dgDetail[colTempData.Name, i].Value = detail.TempData;

                    bool IsPump = detail.IsPump;
                    if (IsPump == true)
                    {
                        dgDetail[colIsPump.Name, i].Value = true;
                    }
                    else
                    {
                        dgDetail[colIsPump.Name, i].Value = false;
                    }
                }
                dgDetail.EndEdit();
                dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(4, this.dgDetail.CurrentRow.Index));
            }
        }

        private IList ArrangeList(IList list)
        {
            IList list_temp = new ArrayList();
            foreach (PouringNoteDetail detail in list)
            {
                if (list_temp.Count == 0)
                {
                    detail.TempData = detail.Id + ",";
                    detail.Descript += "[" + detail.PouringDate.ToShortDateString() + ":" + detail.Quantity + "]";
                    list_temp.Add(detail);
                }
                else
                {
                    for (int i = 0; i < list_temp.Count; i++)
                    {

                        PouringNoteDetail thePouringNoteDetail = list_temp[i] as PouringNoteDetail;
                        if (detail.Price == thePouringNoteDetail.Price && detail.MaterialResource.Id == thePouringNoteDetail.MaterialResource.Id && detail.UsedPart.Id == thePouringNoteDetail.UsedPart.Id)
                        {
                            thePouringNoteDetail.Quantity += detail.Quantity;
                            thePouringNoteDetail.Price = detail.Price;
                            thePouringNoteDetail.Money += detail.Quantity * detail.Price;
                            if (thePouringNoteDetail.Descript.Length < 800)
                            {
                                thePouringNoteDetail.Descript += "[" + detail.PouringDate.ToShortDateString() + ":" + detail.Quantity + "]";
                            }
                            thePouringNoteDetail.TempData = thePouringNoteDetail.TempData + detail.Id + ",";

                            break;
                        }
                        else if (i == list_temp.Count - 1)
                        {
                            detail.TempData = detail.Id + ",";
                            if (thePouringNoteDetail.Descript.Length < 800)
                            {
                                detail.Descript += "[" + detail.PouringDate.ToShortDateString() + ":" + detail.Quantity + "]";
                            }
                            list_temp.Add(detail);
                            break;
                        }
                    }
                }
            }
            //if (list_temp != null && list_temp.Count > 0)
            //{
            //    for (int i = list_temp.Count - 1; i >= 0; i--)
            //    {
            //        PouringNoteDetail thePouringNoteDetail = list_temp[i] as PouringNoteDetail;
            //        thePouringNoteDetail.Price = thePouringNoteDetail.Quantity == 0 ? 0 : Math.Round( thePouringNoteDetail.Money / thePouringNoteDetail.Quantity,2);
            //    }
            //}

            return list_temp;
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
                    conCheckMaster = model.ConcreteMngSrv.GetConCheckingMasterById(id);
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
                    this.btnCreateCheck.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    this.btnCreateCheck.Enabled = false;
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

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            //汇总数量
            object quantity = dgDetail.Rows[e.RowIndex].Cells[colTicketVolume.Name].Value;
            decimal sumqty = 0;
            decimal sumMoney = 0;

            //for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            //{
            //    quantity = dgDetail.Rows[i].Cells[colBalVolume.Name].Value;
            //    if (quantity == null) quantity = 0;
            //    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
            //}
            //txtSumVolume.Text = sumqty.ToString();
            if (colName == colMoney.Name)
            {
                if (IsAddDetialRow(dgDetail.Rows[e.RowIndex]))
                {
                    string temp_volume = dgDetail.Rows[e.RowIndex].Cells[  colMoney.Name ].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_volume);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                       decimal  money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colMoney.Name].Value);
                        sumMoney = sumMoney + money;
                    }
                    object adjustMoney = txtAdjustmentMoney.Text;
                    if (adjustMoney == null) adjustMoney = 0;
                    this.txtSumMoney.Text = ClientUtil.ToString(sumMoney + ClientUtil.ToDecimal(adjustMoney));
                }
            }
             if (colName == colTicketVolume.Name || colName == colDeductionVolume.Name || colName == colBalVolume.Name
                 || colName == this.colLessPumpVolume.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colDeductionVolume.Name].Value != null)
                {
                    string temp_volume = dgDetail.Rows[e.RowIndex].Cells[colDeductionVolume.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_volume);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colLessPumpVolume.Name].Value != null)
                {
                    string temp_volume = dgDetail.Rows[e.RowIndex].Cells[colLessPumpVolume.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_volume);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colBalVolume.Name].Value != null)
                {
                    string temp_volume = dgDetail.Rows[e.RowIndex].Cells[colBalVolume.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_volume);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                object ticketVolume = dgDetail.Rows[e.RowIndex].Cells[colTicketVolume.Name].Value;//小票方量
                object deductionVolume = dgDetail.Rows[e.RowIndex].Cells[colDeductionVolume.Name].Value;//其他扣减
                object lessPumpVolume = dgDetail.Rows[e.RowIndex].Cells[this.colLessPumpVolume.Name].Value;//抽磅扣减
                if (ticketVolume == null) ticketVolume = 0;
                if (deductionVolume == null) deductionVolume = 0;
                if (lessPumpVolume == null) lessPumpVolume = 0;

                dgDetail.Rows[e.RowIndex].Cells[colBalVolume.Name].Value =
                    ClientUtil.ToDecimal(ticketVolume) - ClientUtil.ToDecimal(deductionVolume) - ClientUtil.ToDecimal(lessPumpVolume);

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colBalVolume.Name].Value;
                    if (quantity == null) quantity = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                }
                txtSumVolume.Text = sumqty.ToString();
            }
             if (colName == colBalVolume.Name || colName == colPrice.Name || colName == colBalVolume.Name || colName == colDeductionVolume.Name
                 || colName == colLessPumpVolume.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }

                object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                object qty = dgDetail.Rows[e.RowIndex].Cells[colBalVolume.Name].Value;
                if (price == null) price = 0;
                if (qty == null) qty = 0;
                decimal money = ClientUtil.ToDecimal(price) * ClientUtil.ToDecimal(qty);
                this.dgDetail.Rows[e.RowIndex].Cells[colMoney.Name].Value = money;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colMoney.Name].Value);
                    sumMoney = sumMoney + money;
                }
                object adjustMoney = txtAdjustmentMoney.Text;
                if (adjustMoney == null) adjustMoney = 0;
                this.txtSumMoney.Text = ClientUtil.ToString(sumMoney + ClientUtil.ToDecimal(adjustMoney));
            }
        }
    }
}
