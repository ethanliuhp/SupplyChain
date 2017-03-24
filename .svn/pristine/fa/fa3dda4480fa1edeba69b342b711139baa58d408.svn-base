using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.ConcreteManage;
using Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;


namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng
{
    public partial class VConPouringNote : TMasterDetailView
    {
        private MConcreteMng model = new MConcreteMng();
        private MSupplyOrderMng orderModel = new MSupplyOrderMng();
        private PouringNoteMaster pouringNoteMaster;
        private SupplyOrderMaster curOrderMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public PouringNoteMaster PouringNoteMaster
        {
            get { return pouringNoteMaster; }
            set { pouringNoteMaster = value; }
        }
        DateTime ServerDate;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空


        public VConPouringNote()
        {
            InitializeComponent();
            this.InitData();
            this.InitEvent();
        }

        private void InitData()
        {
            string serverDate = CommonMethod.GetServerDateTime().ToShortDateString();
            ServerDate = Convert.ToDateTime(serverDate);
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
            this.btnImport.Click += new EventHandler(btnImport_Click);
            this.btnExport.Click += new EventHandler(btnExport_Click);
        }

        private void InitEvent()
        {
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            //this.dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgDetail.RowsAdded += new DataGridViewRowsAddedEventHandler(dgDetail_RowsAdded);
            txtSupply.TextChanged += new EventHandler(txtSupply_TextChanged);
            btnForward.Click += new EventHandler(btnForward_Click);
            this.dgDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_RowHeaderMouseClick);
            this.dgDelete.Click += new EventHandler(dgDelete_Click);
            btnSelectWBS.Click += new EventHandler(btnSelectWBS_Click);
            btnSetSubject.Click += new EventHandler(btnSetSubject_Click);
        }

        void btnImport_Click(object sender, EventArgs e)
        {
            VDepartSelector vmros = new VDepartSelector("0");
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            CurrentProjectInfo cpi = list[0] as CurrentProjectInfo;
            this.txtInportSupplier.Text = cpi.Name;
        }
        void btnExport_Click(object sender, EventArgs e)
        {
            VDepartSelector vmros = new VDepartSelector("0");
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            CurrentProjectInfo cpi = list[0] as CurrentProjectInfo;
            this.txtExportSupplier.Text = cpi.Name;
        }
        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == this.colIsPump.Name)
            {
                this.txtCode.Focus();
            }
        }
        void btnSetSubject_Click(object sender, EventArgs e)
        {
            CostAccountSubject oCost = null;
            foreach (DataGridViewRow oRow in this.dgDetail.Rows)
            {
                if (oRow.IsNewRow) break;
                if (oRow.Cells[this.colSubject.Name].Tag != null)
                {
                    oCost = oRow.Cells[this.colSubject.Name].Tag as CostAccountSubject;
                    break;
                }
            }
            if (oCost != null)
            {
                for(int iRow=0;iRow <this.dgDetail .Rows .Count ;iRow ++)
                {
                    DataGridViewRow oRow = this.dgDetail.Rows[iRow];
                    if (oRow.IsNewRow) break;
                     
                     oRow.Cells[this.colSubject.Name].Tag  =oCost ;
                     oRow.Cells[this.colSubject.Name].Value = oCost.Name;
                    
                }
            }
        }
        void btnSelectWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                txtUsedPart.Tag = (list[0] as TreeNode).Tag as GWBSTree;
                txtUsedPart.Text = (list[0] as TreeNode).Text;
            }
        }

        void dgDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgDetail.Rows[e.RowIndex].Cells[colDate.Name].Value = DateTime.Now.Date;
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
                    pouringNoteMaster.Details.Remove(dr.Tag as PouringNoteDetail);
                    movedDtlList.Add(dr.Tag as PouringNoteDetail);
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

        void txtSupply_TextChanged(object sender, EventArgs e)
        {
            ////判断当前日期是否已经生成对账单
            //if (txtSupply.Text != "")
            //{
            //    string lastConCheckDate = model.ConcreteMngSrv.GetLastEndDate(txtSupply.Result[0] as SupplierRelationInfo);
            //    if (lastConCheckDate != "")
            //    {
            //        DateTime LastDate = Convert.ToDateTime(lastConCheckDate);
            //        if (DateTime.Compare(LastDate, ServerDate) >= 0)
            //        {
            //            MessageBox.Show("当前供应商当前日期已生成对账单，不能再做单！");

            //            txtSupply.Clear();
            //            txtSupply.Result.Clear();
            //            txtSupply.Tag = null;

            //            return;
            //        }
            //    }
            //}
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
                pouringNoteMaster = new PouringNoteMaster();
                pouringNoteMaster.CreatePerson = ConstObject.LoginPersonInfo;
                pouringNoteMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                pouringNoteMaster.CreateDate = ConstObject.LoginDate;
                pouringNoteMaster.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                pouringNoteMaster.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                pouringNoteMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                pouringNoteMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                pouringNoteMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                pouringNoteMaster.HandlePerson = ConstObject.LoginPersonInfo;
                pouringNoteMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                pouringNoteMaster.DocState = DocumentState.Edit;


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
                    pouringNoteMaster.ProjectId = projectInfo.Id;
                    pouringNoteMaster.ProjectName = projectInfo.Name;
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
                txtCode.Focus();
                if (!ViewToModel()) return false;
                pouringNoteMaster.DocState = DocumentState.InExecute;
                pouringNoteMaster.CreateDate = ConstObject.LoginDate;
                pouringNoteMaster = model.ConcreteMngSrv.SavePouringNoteMaster(pouringNoteMaster, movedDtlList);
                movedDtlList = new ArrayList();

                ////插入日志
                ////MStockIn.InsertLogData(pouringNoteMaster.Id, "保存", pouringNoteMaster.Code, pouringNoteMaster.CreatePerson.Name, "浇筑记录单","");
                txtCode.Text = pouringNoteMaster.Code;
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
                        pouringNoteMaster = model.ConcreteMngSrv.GetPouringNoteMasterById(pouringNoteMaster.Id);
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
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            //判断当前日期内是否已经生成对账单
            string lastConCheckDate = model.ConcreteMngSrv.GetLastEndDate(txtSupply.Result[0] as SupplierRelationInfo);
            if (lastConCheckDate != "")
            {
                DateTime LastDate = Convert.ToDateTime(lastConCheckDate);
                if (DateTime.Compare(LastDate, ServerDate) >= 0)
                {
                    MessageBox.Show("当前日期已生成对账单，不能修改！");
                    return false;
                }
            }
            if (pouringNoteMaster.DocState == DocumentState.Edit || pouringNoteMaster.DocState == DocumentState.Valid)
            {
                movedDtlList = new ArrayList();
                base.ModifyView();
                pouringNoteMaster = model.ConcreteMngSrv.GetPouringNoteMasterById(pouringNoteMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(pouringNoteMaster.DocState));
            MessageBox.Show(message);
            return false;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                pouringNoteMaster = model.ConcreteMngSrv.GetPouringNoteMasterById(pouringNoteMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
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
                //判断当前日期内是否已经生成对账单
                string lastConCheckDate = model.ConcreteMngSrv.GetLastEndDate(txtSupply.Result[0] as SupplierRelationInfo);
                if (lastConCheckDate != "")
                {
                    DateTime LastDate = Convert.ToDateTime(lastConCheckDate);
                    if (DateTime.Compare(LastDate, ServerDate) >= 0)
                    {
                        MessageBox.Show("当前日期已生成对账单，不能删除！");
                        return false;
                    }
                }
                pouringNoteMaster = model.ConcreteMngSrv.GetPouringNoteMasterById(pouringNoteMaster.Id);

                if (pouringNoteMaster.DocState == DocumentState.Valid || pouringNoteMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ConcreteMngSrv.DeletePouringNoteMaster(pouringNoteMaster)) return false;
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(pouringNoteMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colPart.Name))
                {
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    List<TreeNode> list = frm.SelectResult;
                    if (list.Count > 0)
                    {
                        dgDetail.CurrentRow.Cells[colPart.Name].Tag = (list[0] as TreeNode).Tag as GWBSTree;
                        dgDetail.CurrentRow.Cells[colPart.Name].Value = (list[0] as TreeNode).Text;
                        txtCode.Focus();
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSubject.Name))
                {
                    //双击核算科目
                    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                    frm.ShowDialog();
                    CostAccountSubject cost = frm.SelectAccountSubject;
                    if (cost != null)
                    {
                        if (dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colSubject.Name].Tag = cost;
                            this.dgDetail.CurrentRow.Cells[colSubject.Name].Value = cost.Name;
                        }
                    }
                    this.txtCode.Focus();
                }
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
                this.txtCode.Text = pouringNoteMaster.Code;
                //operDate.Value = pouringNoteMaster.RealOperationDate;
                this.txtTicketNum.Text = pouringNoteMaster.TicketNum;

                if (pouringNoteMaster.ForwardBillCode != null)
                {
                    txtForward.Text = pouringNoteMaster.ForwardBillCode;
                    txtForward.Tag = pouringNoteMaster.ForwardBillId;
                }
                if (pouringNoteMaster.HandlePerson != null)
                {
                    txtHandlePerson.Tag = pouringNoteMaster.HandlePerson;
                    txtHandlePerson.Text = pouringNoteMaster.HandlePersonName;
                }
                if (pouringNoteMaster.UsedPart != null)
                {
                    txtUsedPart.Tag = pouringNoteMaster.UsedPart;
                    txtUsedPart.Text = pouringNoteMaster.UsedPartName;
                }
                if (pouringNoteMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = pouringNoteMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(pouringNoteMaster.TheSupplierRelationInfo);
                    txtSupply.Value = pouringNoteMaster.SupplierName;
                }
                txtInportSupplier.Text = pouringNoteMaster.InportSupplierName;

                txtExportSupplier.Text = pouringNoteMaster.ExportSupplierName;
                if (pouringNoteMaster.CreatePerson != null)
                {
                    txtCreatePerson.Tag = pouringNoteMaster.CreatePerson;
                    txtCreatePerson.Text = pouringNoteMaster.CreatePersonName;
                }
                txtRemark.Text = pouringNoteMaster.Descript;
                txtCreateDate.Text = pouringNoteMaster.CreateDate.ToShortDateString();
                txtSumQuantity.Text = pouringNoteMaster.SumQuantity.ToString();
                txtProject.Text = pouringNoteMaster.ProjectName;
                txtProject.Tag = pouringNoteMaster.ProjectId;

                //显示收料单明细
                this.dgDetail.Rows.Clear();

                //查找浇筑记录单引用的日常计划

                foreach (PouringNoteDetail var in pouringNoteMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    //设置该物料的计量单位
                    if (var.MatStandardUnit != null)
                    {
                        this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                        this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    }

                    dgDetail[colSubject.Name, i].Tag = var.SubjectGUID;
                    dgDetail[colSubject.Name, i].Value = var.SubjectName;

                    this.dgDetail[colPlanQuantity.Name, i].Value = var.PlanQuantity;
                    this.dgDetail[colQuantityTemp.Name, i].Value = Math.Abs(var.Quantity);
                    this.dgDetail[colPouringQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colMoney.Name, i].Value = var.Money;
                    //消耗数量
                    this.dgDetail[colConsumeQty.Name, i].Value = var.ConsumeQty;
                    this.dgDetail[colConsumePrice.Name, i].Value = var.ConsumePrice;
                    this.dgDetail[colConsumeMoney.Name, i].Value = var.ConsumeMoney;
                    //调入数量
                    this.dgDetail[colImportQty.Name, i].Value = var.ImportQty;
                    this.dgDetail[colImportPrice.Name, i].Value = var.ImportPrice;
                    this.dgDetail[colImportMoney.Name, i].Value = var.ImportMoney;
                    //调出数量
                    this.dgDetail[colExportQty.Name, i].Value = var.ExportQty;
                    this.dgDetail[colExportPrice.Name, i].Value = var.ExportPrice;
                    this.dgDetail[colExportMoney.Name, i].Value = var.ExportMoney;
                    if (var.UsedPart != null)
                    {
                        var.UsedPart.SysCode = var.UsedPartSysCode;
                        this.dgDetail[colPart.Name, i].Tag = var.UsedPart;
                        this.dgDetail[colPart.Name, i].Value = var.UsedPartName;
                    }

                    this.dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;
                    //DailyPlanDetail DiaryDetail = model.ConcreteMngSrv.GetDailyPlanDetail(var.ForwardDetailId);
                    //if (DiaryDetail != null)
                    //{
                    //    this.dgDetail[colPlanQuantity.Name, i].Value = DiaryDetail.Quantity;
                    //}

                    if (var.IsPump == true)
                    {
                        this.dgDetail[colIsPump.Name, i].Value = true;
                    }
                    else
                    {
                        this.dgDetail[colIsPump.Name, i].Value = false;
                    }
                    this.dgDetail[colDate.Name, i].Value = var.PouringDate;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    dgDetail[colReasonDes.Name, i].Value = var.ReasonDes;
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
                    pouringNoteMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    pouringNoteMaster.SupplierName = this.txtSupply.Text;
                }
                pouringNoteMaster.InportSupplierName = txtInportSupplier.Text;
                pouringNoteMaster.ExportSupplierName = txtExportSupplier.Text;


                ////使用部位
                //pouringNoteMaster.UsedPart = txtUsedPart.Tag as GWBSTree;
                //pouringNoteMaster.UsedPartName = txtUsedPart.Text;
                pouringNoteMaster.Descript = this.txtRemark.Text;
                pouringNoteMaster.TicketNum = this.txtTicketNum.Text;

                //pouringNoteMaster.RealOperationDate = operDate.Value.Date;
                pouringNoteMaster.SumQuantity = ClientUtil.ToDecimal(this.txtSumQuantity.Text);
                pouringNoteMaster.ForwardBillId = ClientUtil.ToString(txtForward.Tag);
                pouringNoteMaster.ForwardBillCode = ClientUtil.ToString(txtForward.Text);

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    PouringNoteDetail thePouringNoteDetail = new PouringNoteDetail();
                    if (var.Tag != null)
                    {
                        thePouringNoteDetail = var.Tag as PouringNoteDetail;
                        if (thePouringNoteDetail.Id == null)
                        {
                            pouringNoteMaster.Details.Remove(thePouringNoteDetail);
                        }
                    }
                    thePouringNoteDetail.ConcreteCheckState = 0;
                    //材料
                    thePouringNoteDetail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    thePouringNoteDetail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    thePouringNoteDetail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    thePouringNoteDetail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    //计量单位
                    thePouringNoteDetail.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    thePouringNoteDetail.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    thePouringNoteDetail.ForwardDetailId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);
                    //浇筑数量 单价 金额
                    thePouringNoteDetail.Quantity = ClientUtil.ToDecimal(var.Cells[colPouringQuantity.Name].Value);
                    thePouringNoteDetail.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    thePouringNoteDetail.Money = ClientUtil.ToDecimal(var.Cells[colMoney.Name].Value);
                    thePouringNoteDetail.PlanQuantity = ClientUtil.ToDecimal(var.Cells[colPlanQuantity.Name].Value);
                    //调入数量 单价 金额
                    thePouringNoteDetail.ImportQty = ClientUtil.ToDecimal(var.Cells[colImportQty.Name].Value);
                    thePouringNoteDetail.ImportPrice = ClientUtil.ToDecimal(var.Cells[colImportPrice.Name].Value);
                    thePouringNoteDetail.ImportMoney = ClientUtil.ToDecimal(var.Cells[colImportMoney.Name].Value);

                    //调出数量 单价 金额
                    thePouringNoteDetail.ExportQty = ClientUtil.ToDecimal(var.Cells[colExportQty.Name].Value);
                    thePouringNoteDetail.ExportPrice = ClientUtil.ToDecimal(var.Cells[colExportPrice.Name].Value);
                    thePouringNoteDetail.ExportMoney = ClientUtil.ToDecimal(var.Cells[colExportMoney.Name].Value);
                    //使用部位
                    thePouringNoteDetail.UsedPart = var.Cells[colPart.Name].Tag as GWBSTree;
                    thePouringNoteDetail.UsedPartName = ClientUtil.ToString(var.Cells[colPart.Name].Value);
                    thePouringNoteDetail.UsedPartSysCode = thePouringNoteDetail.UsedPart.SysCode;
                    //消耗数量 单价 金额
                    thePouringNoteDetail.ConsumeQty = ClientUtil.ToDecimal(var.Cells[colConsumeQty.Name].Value);
                    thePouringNoteDetail.ConsumePrice = ClientUtil.ToDecimal(var.Cells[colConsumePrice.Name].Value);
                    thePouringNoteDetail.ConsumeMoney = ClientUtil.ToDecimal(var.Cells[colConsumeMoney.Name].Value);

                    thePouringNoteDetail.SubjectGUID = var.Cells[colSubject.Name].Tag as CostAccountSubject;
                    thePouringNoteDetail.SubjectName = ClientUtil.ToString(var.Cells[colSubject.Name].Value);
                    thePouringNoteDetail.SubjectSysCode = ClientUtil.ToString((var.Cells[colSubject.Name].Tag as CostAccountSubject).SysCode);

                    decimal quantityTemp = StringUtil.StrToDecimal(ClientUtil.ToString(var.Cells[colQuantityTemp.Name].Value));
                    thePouringNoteDetail.TempData = quantityTemp.ToString();

                    bool IsChecked = Convert.ToBoolean(var.Cells[colIsPump.Name].EditedFormattedValue);
                    if (IsChecked == true)
                    {
                        thePouringNoteDetail.IsPump = true;
                    }
                    else
                    {
                        thePouringNoteDetail.IsPump = false;
                    }
                    thePouringNoteDetail.PouringDate = Convert.ToDateTime(var.Cells[colDate.Name].Value);
                    thePouringNoteDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    thePouringNoteDetail.ReasonDes = ClientUtil.ToString(var.Cells[colReasonDes.Name].Value);
                    thePouringNoteDetail.ProjectId = pouringNoteMaster.ProjectId;
                    pouringNoteMaster.AddDetail(thePouringNoteDetail);
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
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("浇筑记录明细不能为空");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            if (txtSupply.Result.Count == 0)
            {
                validMessage += "供应商不能为空！\n";
            }
            //if (txtUsedPart.Tag == null || txtUsedPart.Text == "")
            //{
            //    validMessage += "浇筑部位不能为空！\n";
            //}
            int count_inport = 0;
            int count_export = 0;
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if (dr.Cells[colImportQty.Name].Value != null && ClientUtil.ToDecimal(dr.Cells[colImportQty.Name].Value) != 0)
                {
                    count_inport++;
                }
                if (dr.Cells[colExportQty.Name].Value != null && ClientUtil.ToDecimal(dr.Cells[colExportQty.Name].Value) != 0)
                {
                    count_export++;
                }
            }
            if (count_inport > 0)
            {
                if (ClientUtil.ToString(txtInportSupplier.Text) == "")
                {
                    MessageBox.Show("当前存在调入数量，发出单位不能为空！");
                    return false;
                }
            }
            else
            {
                if (ClientUtil.ToString(txtInportSupplier.Text) != "")
                {
                    MessageBox.Show("当前存在发出单位，调入数量不能为空！");
                    return false;
                }
            }
            if (count_export > 0)
            {
                if (ClientUtil.ToString(this.txtExportSupplier.Text) == "")
                {
                    MessageBox.Show("当前存在调出数量，接收单位不能为空！");
                    return false;
                }
            }
            else
            {
                if (ClientUtil.ToString(txtExportSupplier.Text) != "")
                {
                    MessageBox.Show("当前存在接收单位，调出数量不能为空！");
                    return false;
                }
            }

            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            //浇筑记录明细表数据校验
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
                if (dr.Cells[colDate.Name].Value == null)
                {
                    MessageBox.Show("浇筑日期不能为空！");
                    return false;
                }
                //浇筑日期不能大于服务器日期
                DateTime pouringDate = ClientUtil.ToDateTime(dr.Cells[colDate.Name].Value);
                if (pouringDate > ServerDate)
                {
                    MessageBox.Show("[" + dr.Cells[colMaterialName.Name].Value + "][" + dr.Cells[this.colMaterialSpec.Name].Value + "]" + "的浇筑日期不能大于制单日期！");
                    return false;
                }
                if (dr.Cells[colImportQty.Name].Value != null)
                {
                    if (dr.Cells[colImportPrice.Name].Value == null)
                    {
                        MessageBox.Show("调入单价不能为空");
                        return false;
                    }
                }
                if (dr.Cells[colImportPrice.Name].Value != null)
                {
                    if (dr.Cells[colImportQty.Name].Value == null)
                    {
                        MessageBox.Show("调入数量不能为空");
                        return false;
                    }
                }

                if (dr.Cells[colPart.Name].Tag == null)
                {
                    MessageBox.Show("浇筑部位不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colPart.Name];
                    return false;
                }

                if (dr.Cells[this.colSubject.Name].Tag == null)
                {
                    MessageBox.Show("核算科目不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colSubject.Name];
                    return false;
                }

                if (dr.Cells[colPouringQuantity.Name].Value == null || dr.Cells[colPouringQuantity.Name].Value.ToString() == "")
                {
                    MessageBox.Show("浇筑数量不允许为空");
                    dgDetail.CurrentCell = dr.Cells[colPouringQuantity.Name];
                    return false;
                }
                //判断浇筑数量是否大于计划数量，如果大于必须填写"超计划原因说明";
                decimal pouringQty = ClientUtil.ToDecimal(dr.Cells[colPouringQuantity.Name].Value);
                decimal moveQty = ClientUtil.ToDecimal(dr.Cells[this.colImportQty.Name].Value);
                decimal planQty = ClientUtil.ToDecimal(dr.Cells[colPlanQuantity.Name].Value);
                if ((pouringQty + moveQty) > planQty)
                {
                    if (dr.Cells[colReasonDes.Name].Value == null || dr.Cells[colReasonDes.Name].Value.ToString() == "")
                    {
                        MessageBox.Show("[浇筑数量+调入数量]大于[计划数量]，请注明超计划原因！");
                        dgDetail.CurrentCell = dr.Cells[colReasonDes.Name];
                        return false;
                    }
                }

                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                if (forwardDtlId != null)
                {
                    DailyPlanDetail forwardDetail = model.ConcreteMngSrv.GetDailyPlanDetail(forwardDtlId.ToString());
                    if (forwardDetail == null)
                    {
                        MessageBox.Show("未找到前驱单据明细,请重新引用。");
                        dgDetail.CurrentCell = dr.Cells[colPouringQuantity.Name];
                        return false;
                    }
                    else
                    {
                        decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;
                        decimal currentQty = decimal.Parse(dr.Cells[colPouringQuantity.Name].Value.ToString());
                        object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;
                        decimal qtyTemp = 0;
                        if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                        {
                            qtyTemp = decimal.Parse(qtyTempObj.ToString());
                        }

                        //if (currentQty - qtyTemp - canUseQty > 0)
                        //{
                        //    MessageBox.Show("输入数量【" + currentQty + "】大于可引用数量【" + (canUseQty + qtyTemp) + "】。");
                        //    dgDetail.CurrentCell = dr.Cells[colPouringQuantity.Name];
                        //    return false;
                        //}
                    }
                }
            }
            dgDetail.Update();
            return true;
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
                    pouringNoteMaster = model.ConcreteMngSrv.GetPouringNoteMasterById(id);
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
            //object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtExportSupplier, txtInportSupplier };
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name ,colPart.Name};
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
            if (txtForward.Tag == null || txtForward.Tag == "")
            {
                MessageBox.Show(" 请选择前驱单据！");
                return;
            }
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            //decimal inportMoney = 0, exportMoney = 0, money = 0;
            //decimal inpirtQty = 0, exportQty = 0, pouringQty = 0;

            object pouringQty = dgDetail.Rows[e.RowIndex].Cells[colPouringQuantity.Name].Value;//采购数量   采购数量-调出量+调入量=消耗量
            object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;//采购单价
            object consumeQty = dgDetail.Rows[e.RowIndex].Cells[colConsumeQty.Name].Value;//消耗数量
            object inportQty = dgDetail.Rows[e.RowIndex].Cells[colImportQty.Name].Value;//调入数量
            object inportPrice = dgDetail.Rows[e.RowIndex].Cells[colImportPrice.Name].Value;//调入单价

            object exportQty = dgDetail.Rows[e.RowIndex].Cells[colExportQty.Name].Value;//调出数量
            object exportPrice = dgDetail.Rows[e.RowIndex].Cells[colExportPrice.Name].Value;//调出单价

            if (colName == this.colIsPump.Name || colName == colPouringQuantity.Name || colName == colPrice.Name || colName == colImportQty.Name || colName == colImportPrice.Name || colName == colExportQty.Name || colName == colExportPrice.Name)
            {
                bool pumpFlag = ClientUtil.ToBool(dgDetail.Rows[e.RowIndex].Cells[colIsPump.Name].Value);
                bool oldPumpFlag = ClientUtil.ToBool(dgDetail.Rows[e.RowIndex].Cells[colIsPump.Name].Tag);
                if (colName == this.colIsPump.Name)
                {
                    if (pumpFlag != oldPumpFlag)
                    {
                        if (pumpFlag == true)
                        {
                            price = ClientUtil.ToDecimal(price) + curOrderMaster.PumpMoney;
                        }
                        else
                        {
                            price = ClientUtil.ToDecimal(price) - curOrderMaster.PumpMoney;
                        }
                        dgDetail.Rows[e.RowIndex].Cells[colIsPump.Name].Tag = ClientUtil.ToBool(dgDetail.Rows[e.RowIndex].Cells[colIsPump.Name].Value);
                    }
                }
                dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = price;
                #region 校验是否是数字
                if (dgDetail.Rows[e.RowIndex].Cells[colPouringQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colPouringQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_value = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_value);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colImportQty.Name].Value != null)
                {
                    string temp_value = dgDetail.Rows[e.RowIndex].Cells[colImportQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_value);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colImportPrice.Name].Value != null)
                {
                    string temp_value = dgDetail.Rows[e.RowIndex].Cells[colImportPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_value);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colExportQty.Name].Value != null)
                {
                    string temp_value = dgDetail.Rows[e.RowIndex].Cells[colExportQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_value);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colExportPrice.Name].Value != null)
                {
                    string temp_value = dgDetail.Rows[e.RowIndex].Cells[colExportPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_value);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                //计算浇筑金额
                if (colName == colPrice.Name || colName == colPouringQuantity.Name)
                {
                    if (pouringQty == null) pouringQty = 0;
                    if (price == null) price = 0;
                    decimal momey = Decimal.Multiply(ClientUtil.ToDecimal(pouringQty), ClientUtil.ToDecimal(price));

                    dgDetail.CurrentRow.Cells[colMoney.Name].Value = momey;
                }
                #endregion

                //计算采购金额
                if (pouringQty == null) pouringQty = 0;
                if (price == null) price = 0;
                decimal pouringMoney = Decimal.Multiply(ClientUtil.ToDecimal(pouringQty), ClientUtil.ToDecimal(price));
                dgDetail.Rows[e.RowIndex].Cells[colMoney.Name].Value = pouringMoney;

                //计算调入金额
                if (inportQty == null) inportQty = 0;
                if (inportPrice == null) inportPrice = 0;
                decimal inportMoney = Decimal.Multiply(ClientUtil.ToDecimal(inportQty), ClientUtil.ToDecimal(inportPrice));
                dgDetail.Rows[e.RowIndex].Cells[colImportMoney.Name].Value = inportMoney;

                //计算调出金额
                if (exportQty == null) exportQty = 0;
                if (exportPrice == null) exportPrice = 0;
                decimal exportMoney = Decimal.Multiply(ClientUtil.ToDecimal(exportQty), ClientUtil.ToDecimal(exportPrice));
                dgDetail.Rows[e.RowIndex].Cells[colExportMoney.Name].Value = exportMoney;

                //计算消耗数量、金额、单价
                consumeQty = ClientUtil.ToDecimal(pouringQty) + ClientUtil.ToDecimal(inportQty) - ClientUtil.ToDecimal(exportQty);
                this.dgDetail.Rows[e.RowIndex].Cells[colConsumeQty.Name].Value = consumeQty;
                decimal consumeMoney = pouringMoney + inportMoney - exportMoney;
                dgDetail.Rows[e.RowIndex].Cells[colConsumeMoney.Name].Value = consumeMoney;
                decimal consumePrice = 0;
                if (ClientUtil.ToDecimal(consumeQty) != 0)
                {
                    consumePrice = consumeMoney / ClientUtil.ToDecimal(consumeQty);
                }
                dgDetail.Rows[e.RowIndex].Cells[colConsumePrice.Name].Value = consumePrice;
                //计算数量  
                decimal sumqty = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    pouringQty = dgDetail.Rows[i].Cells[colPouringQuantity.Name].Value;
                    if (pouringQty == null) pouringQty = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(pouringQty);
                } 

                txtSumQuantity.Text = sumqty.ToString();
            }
        } 

        void btnForward_Click(object sender, EventArgs e)
        {
            if (txtSupply.Result.Count == 0)
            {
                MessageBox.Show("请先选择供应商！");
                return;
            }
            if (this.txtSupply.Result.Count > 0)
            {
                SupplierRelationInfo rel = this.txtSupply.Result[0] as SupplierRelationInfo;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
                oq.AddCriterion(Expression.Eq("SupplierName", this.txtSupply.Text));//供应商
                IList orderList = orderModel.SupplyOrderSrv.GetSupplyOrder(oq);
                foreach (SupplyOrderMaster master in orderList)
                {
                    if (orderList.Count == 1)
                    {
                        curOrderMaster = master;
                    }
                    else
                    {
                        if (ClientUtil.ToString( master.MaterialCategoryCode).IndexOf("I112") != -1)
                        {
                            curOrderMaster = master;
                        }
                    }
                }
            }
            decimal sumqty = 0;
            VDailyPlanSelector theVDailyPlanSelector = new VDailyPlanSelector();
            theVDailyPlanSelector.selectType = 1;
            theVDailyPlanSelector.ShowDialog();

            IList list = theVDailyPlanSelector.Result;
            //从日常需求那边获得数据
            if (list == null || list.Count == 0) return;

            DailyPlanMaster theDailyPlanMaster = list[0] as DailyPlanMaster;
            txtForward.Tag = theDailyPlanMaster.Id;
            txtForward.Text = theDailyPlanMaster.Code;

            //处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                PouringNoteDetail dtl = dr.Tag as PouringNoteDetail;
                if (dtl != null)
                {
                    if (pouringNoteMaster != null)
                    {
                        pouringNoteMaster.Details.Remove(dtl);
                        if (dtl.Id != null)
                        {
                            movedDtlList.Add(dtl);
                        }
                    }
                }
            }

            //显示引用的明细
            this.dgDetail.Rows.Clear();
            foreach (DailyPlanDetail var in theDailyPlanMaster.Details)
            {
                if (var.IsSelect == false) continue;
                int i = this.dgDetail.Rows.Add();

                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                var.UsedPart.SysCode = var.ProjectTaskSysCode;
                this.dgDetail[colPart.Name, i].Tag = var.UsedPart;
                this.dgDetail[colPart.Name, i].Value = var.UsedPartName;
                if (curOrderMaster != null)
                {
                    foreach (SupplyOrderDetail detail in curOrderMaster.Details)
                    {
                        if (detail.MaterialCode == var.MaterialCode)
                        {
                            this.dgDetail[colPrice.Name, i].Value = detail.ModifyPrice;
                        }
                    }
                }
                else
                {
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                }
                this.dgDetail[this.colIsPump.Name, i].Tag = false;
                this.dgDetail[colPlanQuantity.Name, i].Value = var.Quantity;
                this.dgDetail[colCanUseQty.Name, i].Value = var.Quantity - var.RefQuantity;
                this.dgDetail[colPouringQuantity.Name, i].Value = var.Quantity - var.RefQuantity;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;

                #region 计算（浇筑）采购金额
                object price = var.Price;
                object quantity = var.Quantity - var.RefQuantity;
                if (price == null) price = 0;
                if (quantity == null) quantity = 0;
                decimal money = Decimal.Multiply(ClientUtil.ToDecimal(quantity), ClientUtil.ToDecimal(price));
                dgDetail[colMoney.Name, i].Value = money;
                #endregion

                #region 计算消耗数量、消耗金额和消耗单价(消耗数量=采购数量+调入数量-调出数量；消耗金额=采购金额+调入金额-调出金额；消耗单价=消耗金额/消耗数量)
                //1.计算消耗数量
                object inporyQty = dgDetail[colImportQty.Name, i].Value;
                object exportQty = dgDetail[colExportQty.Name, i].Value;
                if (inporyQty == null) inporyQty = 0;
                if (exportQty == null) exportQty = 0;
                decimal consumeQty = ClientUtil.ToDecimal(quantity) + ClientUtil.ToDecimal(inporyQty) - ClientUtil.ToDecimal(exportQty);
                dgDetail[colConsumeQty.Name, i].Value = consumeQty;
                //2.计算消耗金额
                object inportMoney = dgDetail[colImportMoney.Name, i].Value;
                object exportMoney = dgDetail[colExportMoney.Name, i].Value;
                if (inportMoney == null) inportMoney = 0;
                if (exportMoney == null) exportMoney = 0;
                decimal consumeMoney = money + ClientUtil.ToDecimal(inportMoney) - ClientUtil.ToDecimal(exportMoney);
                dgDetail[colConsumeMoney.Name, i].Value = consumeMoney;
                //3.根据消耗金额和消耗数量计算消耗单价
                dgDetail[colConsumePrice.Name, i].Value = consumeMoney / consumeQty;
                #endregion
                sumqty = sumqty + var.Quantity - var.RefQuantity;
            }
            txtSumQuantity.Text = sumqty.ToString();

        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"商品砼浇筑记录单.flx") == false) return false;
            FillFlex(pouringNoteMaster);
            flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"商品砼浇筑记录单.flx") == false) return false;
            FillFlex(pouringNoteMaster);
            flexGrid1.Print();
            pouringNoteMaster.PrintTimes = pouringNoteMaster.PrintTimes + 1;
            pouringNoteMaster = model.ConcreteMngSrv.SavePouringNoteMaster(pouringNoteMaster, movedDtlList);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"商品砼浇筑记录单.flx") == false) return false;
            FillFlex(pouringNoteMaster);
            flexGrid1.ExportToExcel("商品砼浇筑记录单【" + pouringNoteMaster.Code + "】", false, false, true);
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

        private void FillFlex(PouringNoteMaster billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1,flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //主表数据
            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 7).Text = billMaster.SupplierName;
            flexGrid1.Cell(4, 7).Text = billMaster.UsedPartName;
            flexGrid1.Cell(4, 2).Text = billMaster.ForwardBillCode;

            //填写明细数据
            decimal sumQuantity = 0;
            for (int i = 0; i < detailCount; i++)
            {
                PouringNoteDetail detail = (PouringNoteDetail)billMaster.Details.ElementAt(i);
                //物资名称
                //string a1 = detail.MaterialResource.CreatedDate.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;
                
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                //计划数量
                DailyPlanDetail DiaryDetail = model.ConcreteMngSrv.GetDailyPlanDetail(detail.ForwardDetailId);
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.PlanQuantity);
                //金额
                if (DiaryDetail != null)
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(DiaryDetail.Money);
                }
                //浇筑数量
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Quantity);
                sumQuantity += detail.Quantity;
                //是否泵送
                if (detail.IsPump)
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 7).Text = "是";
                    
                }
                else
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 7).Text = "否";
                    
                }
                //浇筑时间
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.PouringDate.ToShortDateString());
                
                //超计划原因分析
                if (detail.ReasonDes != null)
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.ReasonDes);
                    flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                }
                
                //备注
                if (detail.Descript != null)
                {
                    flexGrid1.Cell(detailStartRowNumber + i, 10).Text = ClientUtil.ToString(detail.Descript);
                    flexGrid1.Cell(detailStartRowNumber + i, 10).WrapText = true;
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            flexGrid1.Cell(7 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(6 + detailCount, 4).Text = ClientUtil.ToString(sumQuantity);
            flexGrid1.Cell(7 + detailCount, 7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(7 + detailCount, 9).Text = billMaster.CreatePersonName;

        }

    }
}
