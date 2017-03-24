using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockInMng.StockInUI
{
    public partial class VStockInQuery : TBasicDataView
    {
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        private MStockMng model = new MStockMng();
        private EnumStockInOutManner stockInManner;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        public int theStockInOutKind = 0;
        public EnumStockInOutManner StockInManner
        {
            get { return stockInManner; }
            set { stockInManner = value; }
        }

        public VStockInQuery(string s)
        {
            InitializeComponent();
            if (s == "收料入库单查询")
            {
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Remove(tabPage2);
            }
            InitEvent();
            InitData();
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            txtSupplierMv.SupplierCatCode = CommonUtil.SupplierCatCode;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            //收料明细单据状态
            comMngType.Items.Clear();
            Array tem = Enum.GetValues(typeof(DocumentState));
            for (int i = 0; i < tem.Length; i++)
            {
                DocumentState s = (DocumentState)tem.GetValue(i);
                int k = (int)s;
                if (k != 0)
                {
                    string strNewName = ClientUtil.GetDocStateName(k);
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = strNewName;
                    li.Value = k.ToString();
                    comMngType.Items.Add(li);
                }
            }
            //调拨明细单据状态
            comMngTypeMv.Items.Clear();
            Array temMv = Enum.GetValues(typeof(DocumentState));
            for (int i = 0; i < temMv.Length; i++)
            {
                DocumentState s = (DocumentState)tem.GetValue(i);
                int k = (int)s;
                if (k != 0)
                {
                    string strNewName = ClientUtil.GetDocStateName(k);
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = strNewName;
                    li.Value = k.ToString();
                    comMngTypeMv.Items.Add(li);
                }
            }
            //收料物资分类
            this.txtMaterialCategory.rootCatCode = "01";
            //调拨物资分类
            this.txtMaterialCategoryMv.rootCatCode = "01";
            //收料明细
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            //收料单据
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            //调拨单据
            this.dtpDateBeginMvBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndMvBill.Value = ConstObject.TheLogin.LoginDate;
            //调拨明细
            this.dtpDateBeginMv.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            projectInfo = StaticMethod.GetProjectInfo();
            //收料入库专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);
            //调拨入库专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategoryMv, true);
            btnAll.Checked = true;
            btnAllMv.Checked = true;
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.txtCodeBeginMv.tbTextChanged+=new EventHandler(txtCodeBeginMv_tbTextChanged);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnExcelMv.Click+=new EventHandler(btnExcelMv_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSearchMv.Click+=new EventHandler(btnSearchMv_Click);
            this.btnSearchMvBill.Click += new EventHandler(btnSearchMvBill_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgMasterMv.SelectionChanged+=new EventHandler(dgMasterMv_SelectionChanged);
            dgMasterMv.CellContentClick+=new DataGridViewCellEventHandler(dgMasterMv_CellContentClick);
        }
        /// <summary>
        /// 收料入库明细Code变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }
        /// <summary>
        /// 调拨入库明细Code变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtCodeBeginMv_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEndMv.Text = this.txtCodeBeginMv.Text;
        }
        /// <summary>
        ///收料入库单据查询打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void  dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            //if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                object stockInOutManner = dgMaster.Rows[e.RowIndex].Cells[colStockInMannerBill.Name].Value;
                int stockInOutKind = int.Parse(dgMaster.Rows[e.RowIndex].Cells[colTheStockInOutKindBill.Name].Value.ToString());
                if (stockInOutManner != null)
                {
                    EnumStockInOutManner manner = (EnumStockInOutManner)Enum.Parse(typeof(EnumStockInOutManner), stockInOutManner.ToString());
                    if (manner == EnumStockInOutManner.收料入库)
                    {
                        //蓝单
                        if (stockInOutKind == 0)
                        {
                            //StockIn master = model.StockInSrv.GetStockInByCode(dgvCell.Value.ToString(),projectInfo.Id);
                            StockIn master = dgMaster.Rows[e.RowIndex].Tag as StockIn;
                            VStockIn vmro = new VStockIn();
                            vmro.CurBillMaster = master;
                            vmro.Preview();
                        }
                        else
                        {
                            //红单
                            // StockInRed master = model.StockInSrv.GetStockInRedByCode(dgvCell.Value.ToString(),projectInfo.Id);
                            StockInRed master = dgMaster.Rows[e.RowIndex].Tag as StockInRed;
                            VStockInRed vsir = new VStockInRed();
                            vsir.CurBillMaster = master;
                            vsir.Preview();
                        }
                    }
                    else if (manner == EnumStockInOutManner.调拨入库)
                    {
                        if (stockInOutKind == 3)//蓝单
                        {
                            //StockMoveIn master = model.StockMoveSrv.GetStockMoveInByCode(dgvCell.Value.ToString(),projectInfo.Id);
                            StockMoveIn master = dgMaster.Rows[e.RowIndex].Tag as StockMoveIn;
                            VStockMoveIn vsmi = new VStockMoveIn();
                            vsmi.CurBillMaster = master;
                            vsmi.Preview();
                        }
                        else
                        {
                            //红单
                            // StockMoveInRed master = model.StockMoveSrv.GetStockMoveInRedByCode(dgvCell.Value.ToString(),projectInfo.Id);
                            StockMoveInRed master = dgMaster.Rows[e.RowIndex].Tag as StockMoveInRed;
                            VStockMoveInRed vsmir = new VStockMoveInRed();
                            vsmir.CurBillMaster = master;
                            vsmir.Preview();
                        }
                    }
                }
            }
        }
        /// <summary>
        ///调拨入库单据查询打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMasterMv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMasterMv[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeMvBill.Name)
            {
                object stockInOutManner = dgMasterMv.Rows[e.RowIndex].Cells[colStockInMannerMvBill.Name].Value;
                int stockInOutKind = int.Parse(dgMasterMv.Rows[e.RowIndex].Cells[colTheStockInOutKindMvBill.Name].Value.ToString());
                if (stockInOutManner != null)
                {
                    EnumStockInOutManner manner = (EnumStockInOutManner)Enum.Parse(typeof(EnumStockInOutManner), stockInOutManner.ToString());
                    if (manner == EnumStockInOutManner.收料入库)
                    {
                        //蓝单
                        if (stockInOutKind == 0)
                        {
                            //StockIn master = model.StockInSrv.GetStockInByCode(dgvCell.Value.ToString(),projectInfo.Id);
                            StockIn master = dgMasterMv.Rows[e.RowIndex].Tag as StockIn;
                            VStockIn vmro = new VStockIn();
                            vmro.CurBillMaster = master;
                            vmro.Preview();
                        }
                        else
                        {
                            //红单
                            StockInRed master = dgMasterMv.Rows[e.RowIndex].Tag as StockInRed;
                            VStockInRed vsir = new VStockInRed();
                            vsir.CurBillMaster = master;
                            vsir.Preview();
                        }
                    }
                    else if (manner == EnumStockInOutManner.调拨入库)
                    {
                        if (stockInOutKind == 3)//蓝单
                        {
                            StockMoveIn master = dgMasterMv.Rows[e.RowIndex].Tag as StockMoveIn;
                            VStockMoveIn vsmi = new VStockMoveIn();
                            vsmi.CurBillMaster = master;
                            vsmi.Preview();
                        }
                        else
                        {
                            //红单
                            StockMoveInRed master = dgMasterMv.Rows[e.RowIndex].Tag as StockMoveInRed;
                            VStockMoveInRed vsmir = new VStockMoveInRed();
                            vsmir.CurBillMaster = master;
                            vsmir.Preview();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 收料入库明细查询双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colCode.Name))
            {
                return;
            }
            //if(MouseDoubleClick this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colCode.Name)
            //{
            //    MessageBox.Show("请单击单据列");
            //    return;
            //}
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                string stockInOutType = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colTheStockInOutKind.Name].Value);

                if (stockInOutType == "1")
                {
                    //收料红单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockInRed vStockred = new VStockInRed();
                        vStockred.Start(billId, code);
                        vStockred.ShowDialog();
                    }
                }

                if (stockInOutType == "0")
                {
                    //收料蓝单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockIn vStock = new VStockIn();
                        vStock.Start(billId, code);
                        vStock.ShowDialog();
                    }
                }
                if (stockInOutType == "3")
                {
                    //调拨入库蓝单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockMoveIn vm = new VStockMoveIn();
                        vm.Start(billId, code);
                        vm.ShowDialog();
                    }
                }
                if (stockInOutType == "4")
                {
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockMoveInRed vmred = new VStockMoveInRed();
                        vmred.Start(billId, code);
                        vmred.ShowDialog();
                    }
                }
                return;
            }
        }
        /// <summary>
        /// 收料入库明细查询打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            //if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            string billId = dgDetail.Rows[e.RowIndex].Tag as string;
            if (string.IsNullOrEmpty(billId)) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                object stockInOutManner = dgDetail.Rows[e.RowIndex].Cells[colStockInManner.Name].Value;
                int stockInOutKind = int.Parse(dgDetail.Rows[e.RowIndex].Cells[colTheStockInOutKind.Name].Value.ToString());
                if (stockInOutManner != null)
                {
                    EnumStockInOutManner manner = (EnumStockInOutManner)Enum.Parse(typeof(EnumStockInOutManner), stockInOutManner.ToString());
                    if (manner == EnumStockInOutManner.收料入库)
                    {
                        //蓝单
                        if (stockInOutKind == 0)
                        {
                            StockIn master = model.StockInSrv.GetStockInById(billId);
                            VStockIn vmro = new VStockIn();
                            vmro.CurBillMaster = master;
                            vmro.Preview();
                        }
                        else
                        {
                            //红单
                            StockInRed master = model.StockInSrv.GetStockInRedById(billId);
                            VStockInRed vsir = new VStockInRed();
                            vsir.CurBillMaster = master;
                            vsir.Preview();
                        }
                    }
                    else if (manner == EnumStockInOutManner.调拨入库)
                    {
                        if (stockInOutKind == 3)//蓝单
                        {
                            StockMoveIn master = model.StockMoveSrv.GetStockMoveInById(billId);
                            VStockMoveIn vsmi = new VStockMoveIn();
                            vsmi.CurBillMaster = master;
                            vsmi.Preview();
                        }
                        else
                        {
                            //红单
                            StockMoveInRed master = model.StockMoveSrv.GetStockMoveInRedById(billId);
                            VStockMoveInRed vsmir = new VStockMoveInRed();
                            vsmir.CurBillMaster = master;
                            vsmir.Preview();
                        }
                    }
                }
            }
            else if (dgvCell.OwningColumn.Name == colSeq.Name)
            {
                string stockInRedDtlId = dgvCell.Tag as string;
                VStockInSeq vsis = new VStockInSeq(stockInRedDtlId);
                vsis.ShowDialog();
            }
        }
        /// <summary>
        /// 收料入库明细查询Excel打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
           /// <summary>
        /// 调拨入库明细查询Excel打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcelMv_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetailMv, true);
        }
        
        /// <summary>
        /// 收料入库主从表查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            //业务日期
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //采购合同号
            if (txtSupplyNumBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("SupplyOrderCode", txtSupplyNumBill.Text, MatchMode.Anywhere));
            }
            try
            {
                list = model.StockInSrv.GetStockIn(objectQuery);
                IList redList = model.StockInSrv.GetStockInRed(objectQuery);
                dgMaster.Rows.Clear();
                dgDetailBill.Rows.Clear();
                ShowMasterList(list, redList);
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList, IList redMasterList)
        {
            if (masterList.Count == 0 && redMasterList.Count == 0) return;
            foreach (StockIn master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                                       //单据号
                dgMaster[colSupplyContractNumBill.Name, rowIndex].Value = master.SupplyOrderCode;                           //采购合同号
                dgMaster[colCarNoBill.Name, rowIndex].Value = master.Cph;                                                                       //车牌号
                dgMaster[colStockInMannerBill.Name, rowIndex].Value = master.StockInManner;                                   //入库类型           
                dgMaster[colTheStockInOutKindBill.Name, rowIndex].Value =0;                                   //红蓝单类型                                         
                dgMaster[colMatCatNameBill.Name, rowIndex].Value = master.MatCatName;                                               //物资分类名称
                dgMaster[colSupplierBill.Name, rowIndex].Value = master.TheSupplierName;                                                 //供应商
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();                        //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();    //制单时间;
                }
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                            //状态
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                                     //备注
            }
            foreach (StockInRed master in redMasterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                                       //单据号
                dgMaster[colSupplyContractNumBill.Name, rowIndex].Value = master.SupplyOrderCode;                           //采购合同号
                dgMaster[colStockInMannerBill.Name, rowIndex].Value = master.StockInManner;                                   //入库类型           
                dgMaster[colTheStockInOutKindBill.Name, rowIndex].Value = 1;                                   //红蓝单类型                                         
                dgMaster[colMatCatNameBill.Name, rowIndex].Value = master.MatCatName;                                               //物资分类名称
                dgMaster[colSupplierBill.Name, rowIndex].Value = master.TheSupplierName;                                                 //供应商
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();                        //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();    //制单时间;
                }
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                            //状态
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                                     //备注
            }
            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            if (dgMaster.CurrentRow.Tag == null) return;
            long stockInOutKind = ClientUtil.ToLong(dgMaster.CurrentRow.Cells["colTheStockInOutKindBill"].Value);
            BasicStockIn master = null;
            if (stockInOutKind == 0)
            {
                master = dgMaster.CurrentRow.Tag as StockIn;
                foreach (StockInDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailBill.Rows.Add();
                    dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                    dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                    dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                 //规格型号   
                    object quantity = dtl.Quantity;//总数量
                    object balQty = dtl.BalQuantity;
                    object redQty = dtl.RefQuantity;
                    decimal noBalQty = ClientUtil.ToDecimal(quantity) - ClientUtil.ToDecimal(balQty) - ClientUtil.ToDecimal(redQty);
                    dgDetailBill[this.colBalQtyBill.Name, rowIndex].Value = ClientUtil.ToDecimal(balQty);
                    dgDetailBill[this.colRedQtyBill.Name, rowIndex].Value = ClientUtil.ToDecimal(redQty);
                    if (noBalQty <= 0)
                    {
                        dgDetailBill[this.colNoBalQtyBill.Name, rowIndex].Value = ClientUtil.ToDecimal("0");
                    }
                    else
                    {
                        dgDetailBill[this.colNoBalQtyBill.Name, rowIndex].Value = ClientUtil.ToDecimal(noBalQty);   //未结数量
                    }
                    dgDetailBill[colQuantityBill.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);//数量
                    dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;                                               //单价
                    dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                    dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                                       //金额
                    decimal dConfrimPrice = ClientUtil.ToDecimal(dtl.ConfirmPrice);
                    dgDetailBill[colConfrimPriceBill.Name, rowIndex].Value = dConfrimPrice;//认价单价
                    dgDetailBill[colConfrimMoneyBill.Name, rowIndex].Value = decimal.Parse(quantity.ToString()) * dConfrimPrice; //认价金额
                    dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                 //计量单位名称
                    dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                           //备注
                }
            }
            else
            {
                master = dgMaster.CurrentRow.Tag as StockInRed;
                foreach (StockInRedDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailBill.Rows.Add();
                    dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                    dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                    dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                 //规格型号   
                    dgDetailBill[colQuantityBill.Name, rowIndex].Value = dtl.Quantity;//数量
                    dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;                                               //单价
                    dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                    dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                                       //金额
                    decimal dConfrimPrice = ClientUtil.ToDecimal(dtl.ConfirmPrice);
                    dgDetailBill[colConfrimPriceBill.Name, rowIndex].Value = dConfrimPrice;//认价单价
                    dgDetailBill[colConfrimMoneyBill.Name, rowIndex].Value = dtl.Quantity * dConfrimPrice; //认价金额
                    dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                 //计量单位名称
                    dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                           //备注
                }
                if (master == null) return;
            }
        }
        /// <summary>
        /// 收料入库明细查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                }
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }

                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }

                if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
                {
                    condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
                }

                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }
                if (txtOriContractNo.Text != "")
                {
                    condition += " and t1.SupplyOrderCode like '" + txtOriContractNo.Text + "%'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                //物资
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                if (this.txtMaterial.Text != "" && this.txtMaterial.Result != null && this.txtMaterial.Result.Count != 0)
                {
                    Material material = this.txtMaterial.Result[0] as Material;
                    if (material.Name == this.txtMaterial.Text)
                    {
                        condition = condition + " and t2.material='" + material.Id + "'";
                    }
                }
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
                }
                object proCat = cboProfessionalCategory.SelectedItem;
                if (proCat != null && !string.IsNullOrEmpty(proCat.ToString()))
                {
                    condition += " and t2.ProfessionalCategory='" + proCat + "'";
                }
                
                //蓝单
                if (btnBlue.Checked)
                {
                    if (stockInManner == EnumStockInOutManner.收料入库)
                    {
                        condition += " and t1.TheStockInOutKind=0";
                    }
                    else if (stockInManner == EnumStockInOutManner.调拨入库)
                    {
                        condition += " and t1.TheStockInOutKind=3";
                    }
                }
                //红单
                if (btnRed.Checked)
                {
                    if (stockInManner == EnumStockInOutManner.收料入库)
                    {
                        condition += " and t1.TheStockInOutKind=1";
                    }
                    else if (stockInManner == EnumStockInOutManner.调拨入库)
                    {
                        condition += " and t1.TheStockInOutKind=4";
                    }
                }

                if (((int)stockInManner) >0)
                {
                    condition += " and t1.StockInManner=" + (int)stockInManner;
                }
                else
                { 
                
                }
                //物资分类
                if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    MaterialCategory materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    condition += " and t2.materialcode like '" + materialCategory.Code + "%'";
                }

                #endregion
                #region 过滤数据权限
                if (1==2 && StaticMethod.IsEnabledDataAuth && ConstObject.TheLogin.TheAccountOrgInfo != null && ConstObject.TheLogin.TheAccountOrgInfo != null && ClientUtil.ToString(ConstObject.TheLogin.TheAccountOrgInfo.Id) != "" && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
                {
                    //1.查询数据权限配置
                    ObjectQuery oqAuth = new ObjectQuery();
                    oqAuth.AddCriterion(Expression.Eq("AuthMenu.Id", TheAuthMenu.Id));

                    Disjunction disAuth = new Disjunction();
                    foreach (OperationRole role in ConstObject.TheRoles)
                    {
                        disAuth.Add(Expression.Eq("AppRole.Id", role.Id));
                    }
                    oqAuth.AddCriterion(disAuth);
                    IEnumerable<AuthConfigOrgSysCode> listAuth = model.StockInSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();
                    if (listAuth.Count() > 0)//如果配置了权限规则
                    {
                        var query = from a in listAuth
                                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.无约束
                                    select a;

                        if (query.Count() == 0)//未设置“无约束”规则
                        {
                            disAuth = new Disjunction();

                            //2.根据数据权限定义的规则过滤数据
                            query = from a in listAuth
                                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.该核算组织的
                                    select a;
                            if (query.Count() > 0)//如果配置中包含操作“该核算组织的”的权限，则无需再添加其它规则
                            {
                                condition += "and t1.OPGSYSCODE like  '" + ConstObject.TheLogin.TheAccountOrgInfo.SysCode + "%'";
                            }
                        }
                    }
                }
                #endregion
                DataSet dataSet = model.StockInSrv.StockInQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0, sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["SupplyOrderCode"];
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierRelationName"];
                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colStockInManner.Name, rowIndex].Value = Enum.GetName(typeof(EnumStockInOutManner), int.Parse(dataRow["StockInManner"].ToString()));

                    dgDetail[colMaterialCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialCode"]);
                    dgDetail[colMaterialName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialName"]);
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];

                    object quantity = dataRow["Quantity"];
                    object balQty = dataRow["Balquantity"];
                    object redQty = dataRow["Refquantity"];
                    decimal noBalQty = ClientUtil.ToDecimal(quantity) - ClientUtil.ToDecimal(balQty) - ClientUtil.ToDecimal(redQty);
                    dgDetail[this.colBalQty.Name, rowIndex].Value = ClientUtil.ToDecimal(balQty);
                    dgDetail[this.colRedQty.Name, rowIndex].Value = ClientUtil.ToDecimal(redQty);
                    if (noBalQty <= 0)
                    {
                        dgDetail[this.colNoBalQty.Name, rowIndex].Value = ClientUtil.ToDecimal("0");
                    }
                    else
                    {
                        dgDetail[this.colNoBalQty.Name, rowIndex].Value = ClientUtil.ToDecimal(noBalQty);
                    }
                    if (quantity != null)
                    {
                        sumQuantity += ClientUtil.ToDecimal(quantity);
                    }
                    else
                    {
                        quantity = "0";
                    }

                    dgDetail[colQuantity.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);
                    dgDetail[colPrice.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["price"]);

                    object money = dataRow["Money"];
                    if (money != null)
                    {
                        sumMoney += ClientUtil.ToDecimal(money);
                    }
                    dgDetail[colMoney.Name, rowIndex].Value = ClientUtil.ToDecimal(money);
                    dgDetail[colProfessionalCategory.Name, rowIndex].Value = ClientUtil.ToString(dataRow["ProfessionalCategory"]);
                    dgDetail[colUnit.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MatStandardUnitName"]);
                    dgDetail[colDescript.Name, rowIndex].Value = ClientUtil.ToString(dataRow["Descript"]);//备注
                    dgDetail[colTheStockInOutKind.Name, rowIndex].Value = dataRow["TheStockInOutKind"];
                    theStockInOutKind = int.Parse(dataRow["TheStockInOutKind"].ToString());
                    if (theStockInOutKind == 1 || theStockInOutKind == 4)//表示红单
                    {
                        dgDetail[colSeq.Name, rowIndex].Value = "查看";
                    }
                    else
                    {
                        dgDetail[colSeq.Name, rowIndex].Value = "";
                    }
                    dgDetail[colSeq.Name, rowIndex].Tag = dataRow["DtlId"];//冲红信息

                    dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                    dgDetail[colCreatePersonName.Name, rowIndex].Value = dataRow["CreatePersonName"];

                    //认价
                    dgDetail[colConfrimPrice.Name, rowIndex].Value = ClientUtil.ToString(dataRow["confirmprice"]);

                    dgDetail[colConfrimMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["confirmmoney"]);
                    dgDetail[colDiagramNum.Name, rowIndex].Value = ClientUtil.ToString(dataRow["DiagramNumber"]);
                    dgDetail[colPrintTimes.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["PrintTimes"]);
                }
                FlashScreen.Close();
                this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
                txtSumMoney.Text = sumMoney.ToString("#,###.####");
                lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        #region 调拨入库主子表查询
        /// <summary>
        /// 领料入库主从表查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchMvBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
        
            //单据
            if (txtCodeBeginMvBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginMvBill.Text, MatchMode.Anywhere));
            }
            //业务日期
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginMvBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndMvBill.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //采购合同号
            if (txtSupplyNumMvBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("SupplyOrderCode", txtSupplyNumMvBill.Text, MatchMode.Anywhere));
            }
            try
            {
                list = model.StockMoveSrv.GetStockMoveIn(objectQuery);
                IList redList = model.StockMoveSrv.GetStockMoveInRed(objectQuery);
                dgMasterMv.Rows.Clear();
                dgDetailMvBill.Rows.Clear();
                ShowMasterMvList(list, redList);
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterMvList(IList masterList, IList redMasterList)
        {
            if (masterList.Count == 0 && redMasterList.Count == 0) return;
            foreach (StockMoveIn master in masterList)
            {
                int rowIndex = dgMasterMv.Rows.Add();
                dgMasterMv.Rows[rowIndex].Tag = master;
                dgMasterMv[colCodeMvBill.Name, rowIndex].Value = master.Code;                                                                       //单据号
                dgMasterMv[colOriContractNoMvBill.Name, rowIndex].Value = master.SupplyOrderCode;                           //采购合同号
                dgMasterMv[colCarNoMvBill.Name, rowIndex].Value = master.MoveOutProjectName;                                           //调出项目名称
                dgMasterMv[colStockInMannerMvBill.Name, rowIndex].Value = master.StockInManner;                                   //入库类型         
                dgMasterMv[colTheStockInOutKindMvBill.Name, rowIndex].Value = 3;                                                                //红蓝单类型                                         
                dgMasterMv[colMatCatNameMvBill.Name, rowIndex].Value = master.MatCatName;                                               //物资分类名称
                dgMasterMv[colSupplyInfoMvBill.Name, rowIndex].Value = master.TheSupplierName;                                                 //供应商
                dgMasterMv[colCreateDateMvBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();                        //业务日期
                dgMasterMv[colCreatePersonNameMvBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMasterMv[colRealOperationDateMvBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();    //制单时间;
                }
                dgMasterMv[colStateMvBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                            //状态
                dgMasterMv[colDescriptMvBill.Name, rowIndex].Value = master.Descript;                                                                     //备注
            }
            foreach (StockMoveInRed master in redMasterList)
            {
                int rowIndex = dgMasterMv.Rows.Add();
                dgMasterMv.Rows[rowIndex].Tag = master;
                dgMasterMv[colCodeMvBill.Name, rowIndex].Value = master.Code;                                                                       //单据号
                dgMasterMv[colOriContractNoMvBill.Name, rowIndex].Value = master.SupplyOrderCode;                           //采购合同号
                dgMasterMv[colStockInMannerMvBill.Name, rowIndex].Value = master.StockInManner;                                   //入库类型           
                dgMasterMv[colTheStockInOutKindMvBill.Name, rowIndex].Value = 4;                                   //红蓝单类型                                         
                dgMasterMv[colMatCatNameMvBill.Name, rowIndex].Value = master.MatCatName;                                               //物资分类名称
                dgMasterMv[colSupplyInfoMvBill.Name, rowIndex].Value = master.TheSupplierName;                                                 //供应商
                dgMasterMv[colCreateDateMvBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();                        //业务日期
                dgMasterMv[colCreatePersonNameMvBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMasterMv[colRealOperationDateMvBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();    //制单时间;
                }
                dgMasterMv[colStateMvBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                            //状态
                dgMasterMv[colDescriptMvBill.Name, rowIndex].Value = master.Descript;                                                                     //备注
            }
            this.dgMasterMv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMasterMv.CurrentCell = dgMasterMv[1, 0];
            dgMasterMv_SelectionChanged(dgMasterMv, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMasterMv_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailMvBill.Rows.Clear();
            if (dgMasterMv.CurrentRow.Tag == null) return;
            long stockInOutKind = ClientUtil.ToLong(dgMasterMv.CurrentRow.Cells["colTheStockInOutKindMvBill"].Value);
            BasicStockIn master = null;
            if (stockInOutKind == 3)
            {
                master = dgMasterMv.CurrentRow.Tag as StockMoveIn;
                foreach (StockMoveInDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailMvBill.Rows.Add();
                    dgDetailMvBill[colMaterialCodeMvBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                    dgDetailMvBill[colMaterialNameMvBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                    dgDetailMvBill[colSpecMvBill.Name, rowIndex].Value = dtl.MaterialSpec;                                              //规格型号   
                    object quantity = dtl.Quantity;
                    dgDetailMvBill[colQuantityMvBill.Name, rowIndex].Value = ClientUtil.ToDecimal(dtl.Quantity);                    //数量
                    dgDetailMvBill[colDiagramNumMvBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                    dgDetailMvBill[colMoneyMvBill.Name, rowIndex].Value = dtl.Money;                                              //金额
                    dgDetailMvBill[colPriceMvBill.Name, rowIndex].Value = dtl.Price;                                                            //调拨单价   
                    dgDetailMvBill[colProfessionalCategoryMvBill.Name, rowIndex].Value = dtl.ProfessionalCategory;        //专业分类    
                    decimal dConfrimPrice = ClientUtil.ToDecimal(dtl.ConfirmPrice);
                    dgDetailMvBill[colConfrimPriceMvBill.Name, rowIndex].Value = dConfrimPrice;                                         //认价单价
                    dgDetailMvBill[colConfrimMoneyMvBill.Name, rowIndex].Value = decimal.Parse(quantity.ToString()) * dConfrimPrice; //认价金额
                    dgDetailMvBill[colUnitMvBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                 //计量单位名称
                    dgDetailMvBill[colDescriptDgMvBill.Name, rowIndex].Value = dtl.Descript;                                           //备注
                }
            }
            else if (stockInOutKind == 4)
            {
                master = dgMasterMv.CurrentRow.Tag as StockMoveInRed;
                foreach (StockMoveInRedDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailMvBill.Rows.Add();
                    dgDetailMvBill[colMaterialCodeMvBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                    dgDetailMvBill[colMaterialNameMvBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                    dgDetailMvBill[colSpecMvBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                 //规格型号   
                    dgDetailMvBill[colQuantityMvBill.Name, rowIndex].Value = dtl.Quantity;                                               //数量
                    dgDetailMvBill[colPriceMvBill.Name, rowIndex].Value = dtl.Price;                                                               //单价
                    dgDetailMvBill[colDiagramNumMvBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                    dgDetailMvBill[colMoneyMvBill.Name, rowIndex].Value = dtl.Money;                                                  //金额
                    decimal dConfrimPrice = ClientUtil.ToDecimal(dtl.ConfirmPrice);
                    dgDetailMvBill[colConfrimPriceMvBill.Name, rowIndex].Value = dConfrimPrice;                                         //认价单价
                    dgDetailMvBill[colProfessionalCategoryMvBill.Name, rowIndex].Value = dtl.ProfessionalCategory;        //专业分类    
                    dgDetailMvBill[colConfrimMoneyMvBill.Name, rowIndex].Value = dtl.Quantity * dConfrimPrice;                //认价金额
                    dgDetailMvBill[colUnitMvBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                 //计量单位名称
                    dgDetailMvBill[colDescriptDgMvBill.Name, rowIndex].Value = dtl.Descript;                                           //备注
                }
                if (master == null) return;
            }
        }
        #endregion
        /// <summary>
        /// 调拨入库明细查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchMv_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                if (this.txtCodeBeginMv.Text != "")
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBeginMv.Text + "' and '" + this.txtCodeEndMv.Text + "'";
                }

                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBeginMv.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEndMv.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBeginMv.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEndMv.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }

                if (this.txtSupplierMv.Text != "" && this.txtSupplierMv.Result != null && this.txtSupplierMv.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplierMv.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }

                if (!txtHandlePersonMv.Text.Trim().Equals("") && txtHandlePersonMv.Result != null)
                {
                    condition = condition + " and t1.handleperson='" + (txtHandlePersonMv.Result[0] as PersonInfo).Id + "'";
                }

                if (!txtCreatePersonMv.Text.Trim().Equals("") && txtCreatePersonMv.Result != null && txtCreatePersonMv.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePersonMv.Result[0] as PersonInfo).Id + "'";
                }

                if (txtOriContractNoMv.Text != "")
                {
                    condition += " and t1.SupplyOrderCode like '" + txtOriContractNoMv.Text + "%'";
                }
                if (comMngTypeMv.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngTypeMv.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                //物资
                if (this.txtMaterialMv.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterialMv.Text + "%'";
                }
                if (this.txtMaterialMv.Text != "" && this.txtMaterialMv.Result != null && this.txtMaterialMv.Result.Count != 0)
                {
                    Material material = this.txtMaterialMv.Result[0] as Material;
                    if (material.Name == this.txtMaterialMv.Text)
                    {
                        condition = condition + " and t2.material='" + material.Id + "'";
                    }
                }
                if (this.txtSpecMv.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpecMv.Text + "%'";
                }

                object proCat = cboProfessionalCategoryMv.SelectedItem;
                if (proCat != null && !string.IsNullOrEmpty(proCat.ToString()))
                {
                    condition += " and t2.ProfessionalCategory='" + proCat + "'";
                }
                if (chkMaterialProvider.Checked == true)
                {
                    condition += " and t1.materialprovider = 1 ";
                }
                //蓝单
                if (btnBlueMv.Checked)
                {
                    if (stockInManner == EnumStockInOutManner.收料入库)
                    {
                        condition += " and t1.TheStockInOutKind=0";
                    }
                    else if (stockInManner == EnumStockInOutManner.调拨入库)
                    {
                        condition += " and t1.TheStockInOutKind=3";
                    }
                }
                //红单
                if (btnRedMv.Checked)
                {
                    if (stockInManner == EnumStockInOutManner.收料入库)
                    {
                        condition += " and t1.TheStockInOutKind=1";
                    }
                    else if (stockInManner == EnumStockInOutManner.调拨入库)
                    {
                        condition += " and t1.TheStockInOutKind=4";
                    }
                }

                if (((int)stockInManner) > 0)
                {
                    condition += " and t1.StockInManner=" + (int)stockInManner;
                }
                //物资分类
                if (txtMaterialCategoryMv.Text != "" && txtMaterialCategoryMv.Result != null && txtMaterialCategoryMv.Result.Count > 0)
                {
                    MaterialCategory materialCategory = txtMaterialCategoryMv.Result[0] as MaterialCategory;
                    condition += " and t2.materialcode like '" + materialCategory.Code + "%'";
                }

                #endregion
                #region 过滤数据权限
                //if (StaticMethod.IsEnabledDataAuth && ConstObject.TheLogin.TheAccountOrgInfo != null && ClientUtil.ToString(ConstObject.TheLogin.TheAccountOrgInfo.Id) != "" && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
                //{
                //    //1.查询数据权限配置
                //    ObjectQuery oqAuth = new ObjectQuery();
                //    oqAuth.AddCriterion(Expression.Eq("AuthMenu.Id", TheAuthMenu.Id));

                //    Disjunction disAuth = new Disjunction();
                //    foreach (OperationRole role in ConstObject.TheRoles)
                //    {
                //        disAuth.Add(Expression.Eq("AppRole.Id", role.Id));
                //    }
                //    oqAuth.AddCriterion(disAuth);

                //    IEnumerable<AuthConfigOrgSysCode> listAuth = model.StockInSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();
                //    if (listAuth.Count() > 0)//如果配置了权限规则
                //    {
                //        var query = from a in listAuth
                //                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.无约束
                //                    select a;

                //        if (query.Count() == 0)//未设置“无约束”规则
                //        {
                //            disAuth = new Disjunction();

                //            //2.根据数据权限定义的规则过滤数据
                //            query = from a in listAuth
                //                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.该核算组织的
                //                    select a;
                //            if (query.Count() > 0)//如果配置中包含操作“该核算组织的”的权限，则无需再添加其它规则
                //            {
                //                condition += "and t1.OPGSYSCODE like  '" + ConstObject.TheLogin.TheAccountOrgInfo.SysCode + "%'";
                //            }
                //        }
                //    }
                //}
                #endregion
                DataSet dataSet = model.StockInSrv.StockInQuery(condition);
                this.dgDetailMv.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0, sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetailMv.Rows.Add();
                    dgDetailMv[colCodeMv.Name, rowIndex].Value = dataRow["code"];
                    dgDetailMv[colOriContractNoMv.Name, rowIndex].Value = dataRow["SupplyOrderCode"];
                    dgDetailMv[colSupplyInfoMv.Name, rowIndex].Value = dataRow["SupplierRelationName"];
                    DataGridViewRow currRow = dgDetailMv.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetailMv[colStateMv.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetailMv[colStockInMannerMv.Name, rowIndex].Value = Enum.GetName(typeof(EnumStockInOutManner), int.Parse(dataRow["StockInManner"].ToString()));

                    dgDetailMv[colMaterialCodeMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialCode"]);
                    dgDetailMv[colMaterialNameMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MaterialName"]);
                    dgDetailMv[colSpecMv.Name, rowIndex].Value = dataRow["MaterialSpec"];

                    object quantity = dataRow["Quantity"];
                    object balQty = dataRow["Balquantity"];
                    object redQty = dataRow["Refquantity"];
                    decimal noBalQty = ClientUtil.ToDecimal(quantity) - ClientUtil.ToDecimal(balQty) - ClientUtil.ToDecimal(redQty);
                    dgDetailMv[this.colBalQtyMv.Name, rowIndex].Value = ClientUtil.ToDecimal(balQty);
                    dgDetailMv[this.colRedQtyMv.Name, rowIndex].Value = ClientUtil.ToDecimal(redQty);
                    if (noBalQty <= 0)
                    {
                        dgDetailMv[this.colNoBalQtyMv.Name, rowIndex].Value = ClientUtil.ToDecimal("0");
                    }
                    else
                    {
                        dgDetailMv[this.colNoBalQtyMv.Name, rowIndex].Value = ClientUtil.ToDecimal(noBalQty);
                    }
                    if (quantity != null)
                    {
                        sumQuantity += ClientUtil.ToDecimal(quantity);
                    }
                    else
                    {
                        quantity = "0";
                    }

                    dgDetailMv[colQuantityMv.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);
                    dgDetailMv[colPriceMv.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["price"]);

                    object money = dataRow["Money"];
                    if (money != null)
                    {
                        sumMoney += ClientUtil.ToDecimal(money);
                    }
                    dgDetailMv[colMoneyMv.Name, rowIndex].Value = ClientUtil.ToDecimal(money);
                    dgDetailMv[colProfessionalCategoryMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["ProfessionalCategory"]);
                    dgDetailMv[colUnitMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MatStandardUnitName"]);
                    dgDetailMv[colDescriptMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["Descript"]);//备注
                    dgDetailMv[colTheStockInOutKindMv.Name, rowIndex].Value = dataRow["TheStockInOutKind"];
                    theStockInOutKind = int.Parse(dataRow["TheStockInOutKind"].ToString());
                    if (theStockInOutKind == 1 || theStockInOutKind == 4)//表示红单
                    {
                        dgDetailMv[colSeqMv.Name, rowIndex].Value = "查看";
                    }
                    else
                    {
                        dgDetailMv[colSeqMv.Name, rowIndex].Value = "";
                    }
                    dgDetailMv[colSeqMv.Name, rowIndex].Tag = dataRow["DtlId"];//冲红信息

                    dgDetailMv[colCreateDateMv.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                    dgDetailMv[colCreatePersonNameMv.Name, rowIndex].Value = dataRow["CreatePersonName"];

                    //认价
                    dgDetailMv[colConfrimPriceMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["confirmprice"]);

                    dgDetailMv[colConfrimMoneyMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["confirmmoney"]);
                    dgDetailMv[colDiagramNumMv.Name, rowIndex].Value = ClientUtil.ToString(dataRow["DiagramNumber"]);
                    dgDetailMv[colPrintTimesMv.Name, rowIndex].Value = ClientUtil.ToDecimal(dataRow["PrintTimes"]);
                }
                FlashScreen.Close();
                this.txtSumQuantityMv.Text = sumQuantity.ToString("#,###.####");
                txtSumMoneyMv.Text = sumMoney.ToString("#,###.####");
                lblRecordTotalMv.Text = "共【" + dataTable.Rows.Count + "】条记录";
                this.dgDetailMv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
    }
}
