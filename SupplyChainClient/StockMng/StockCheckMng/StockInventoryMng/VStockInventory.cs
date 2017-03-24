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
using Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain;
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
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;


namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng
{
    public partial class VStockInventory : TMasterDetailView
    {
        private MStockInventoryMng model = new MStockInventoryMng();
        private MStockMng modelStockOut = new MStockMng();
        private MStockMng modelStockIn = new MStockMng();
        private StockInventoryMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        CurrentProjectInfo projectInfo;
        CStockInventoryMng_ExecType execType;
        /// <summary>
        /// 当前单据
        /// </summary>
        public StockInventoryMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        public CStockInventoryMng_ExecType ExecType
        {
            get { return execType; }
            set { execType = value; }
        }
        public VStockInventory()
        {
            InitializeComponent();
            InitEvent();
            execType = CStockInventoryMng_ExecType.土建;
            InitalForm();
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
        }
        public VStockInventory(CStockInventoryMng_ExecType execType)
        {
            InitializeComponent();
            InitEvent();
            this.execType = execType;

            InitalForm();
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
        }
        private void InitEvent()
        {
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            projectInfo = StaticMethod.GetProjectInfo();
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            this.btnSelectUsedRank .Click +=new EventHandler(btnSelectUsedRank_Click);
            this.btnSelectUserPart.Click +=new EventHandler(btnSelectUserPart_Click);
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
           
        }
        private void InitalForm()
        {
            if (ExecType == CStockInventoryMng_ExecType.土建)
            { 
                lblCat.Text = "物资分类";
                txtMaterialCategory.Visible = true;
               cboProfessionCat.Visible = false;
                this.lblUsedRank.Visible = false;
                this.txtUsedRank.Visible = false;
                this.btnSelectUsedRank.Visible = false;
                this.dgDetail.Columns[this.colConfirmMoney.Name ].Visible = false;
                this.dgDetail.Columns[this.colConfirmPrice .Name ].Visible = false;
                this.dgDetail.Columns[this.colSupplyMoney .Name].Visible = false;
                this.dgDetail.Columns[this.colSupplyPrice .Name].Visible = false;
                this.lbUserPart.Visible = false;
                this.txtUserPart.Visible = false;
                this.btnSelectUserPart.Visible = false;
            }
            else if (ExecType == CStockInventoryMng_ExecType.安装)
            {
                 
                lblCat.Text = "专业分类";
                txtMaterialCategory.Visible = false;
                cboProfessionCat.Visible = true;
                cboProfessionCat.ReadOnly = false;
                this.lblUsedRank.Visible = true ;
                this.txtUsedRank.Visible = true;
                this.btnSelectUsedRank.Visible = true;
                this.dgDetail.Columns[this.colConfirmMoney.Name].Visible = true;
                this.dgDetail.Columns[this.colConfirmMoney.Name].ReadOnly = true;
                this.dgDetail.Columns[this.colConfirmPrice.Name].Visible = true;
                this.dgDetail.Columns[this.colSupplyMoney.Name].Visible = true;
                this.dgDetail.Columns[this.colSupplyMoney.Name].ReadOnly = true;
                this.dgDetail.Columns[this.colSupplyPrice.Name].Visible = true;
                this.lbUserPart.Visible = true ;
                this.txtUserPart.Visible = true;
                this.btnSelectUserPart.Visible = true;
                txtUserPart.ReadOnly = true;
                this.txtUsedRank.ReadOnly = true;
                
            }

        }
        void btnSelectUserPart_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];
                GWBSTree task = root.Tag as GWBSTree;
                this.txtUserPart.Text = task.Name;
                this.txtUserPart.Tag = task;
                //if (task != null)
                //{
                //    this.dgDetail.Rows[e.RowIndex].Cells[colUsedPart.Name].Value = task.Name;
                //    this.dgDetail.Rows[e.RowIndex].Cells[colUsedPart.Name].Tag = task;
                //    this.txtCode.Focus();
                //}
            }
        }
        void btnSelectUsedRank_Click(object sender, EventArgs e)
        {
            CommonSupplier Supplier = new CommonSupplier();
            Supplier.SupplierCatCode = CommonUtil.SupplierCatCode3 ;
            Supplier.OpenSelect();
            IList list = Supplier.Result;
            foreach (SupplierRelationInfo Suppliers in list)
            {
                this.txtUsedRank .Tag = Suppliers;
                this.txtUsedRank.Text  = Suppliers.SupplierInfo.Name;
                this.txtCode.Focus();
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
                    curBillMaster.Details.Remove(dr.Tag as StockInventoryDetail);
                }
            }
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Tag == null)
            {
                DataGridViewRow dr = e.Row;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dgDetail.Rows.Count == 1)
                {
                    dgDetail.Rows.Add();
                }
            }
            else
            {
                this.movedDtlList.Add(e.Row.Tag as BaseDetail);
                curBillMaster.Details.Remove(e.Row.Tag as BaseDetail);
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
                    curBillMaster = model.StockInventorySrv.GetStockInventoryById(Id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject};
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colStockQuantity.Name,this.colConfirmMoney .Name ,this.colSupplyMoney.Name };
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

                this.curBillMaster = new StockInventoryMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                //curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                //curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
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
               
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ViewToModel())
                return false;

            LogData log = new LogData();
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                if (optrType == 2)
                {
                    log.OperType = "新增提交";
                }
                else
                {
                    log.OperType = "新增保存";
                }

            }
            else
            {
                if (optrType == 2)
                {
                    log.OperType = "修改提交";
                }
                else
                {
                    log.OperType = "修改保存";
                }
            }
            if (optrType == 2)
            {
                DataTable oTable = modelStockIn.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }
                curBillMaster.DocState = DocumentState.InAudit;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            }
            curBillMaster = model.StockInventorySrv.SaveStockInventory(curBillMaster, movedDtlList);
            this.txtCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            log.BillType = "月度盘点单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                curBillMaster = model.StockInventorySrv.GetStockInventoryById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为非编辑状态，不能修改！";
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
                    DataTable oTable =  modelStockIn.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                    if (oTable != null && oTable.Rows.Count > 0)
                    {
                        curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                        curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                    }
                    if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                    {
                        if (SaveOrSubmitBill(1) == false) return false;
                        txtCode.Text = curBillMaster.Code;
                        //更新Caption
                        this.ViewCaption = ViewName + "-" + txtCode.Text;
                        MessageBox.Show("保存成功！");
                        return true;
                        //MessageBox.Show("保存成功！");
                        //return true;
                    }
                   
                        MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
                        return false;
                    
                }

                catch (Exception e)
                {
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
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
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (SaveOrSubmitBill(2) == false) return false;
                    MessageBox.Show("提交成功！");
                    return true;
                }
                else
                {
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能提交！");
                    return false;
                }
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
                curBillMaster = model.StockInventorySrv.GetStockInventoryById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.StockInventorySrv.DeleteByDao(curBillMaster)) return false;
                    //插入日志
                    //StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月度盘点单", "", curBillMaster.ProjectName);
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "月度盘点单";
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
                        curBillMaster = model.StockInventorySrv.GetStockInventoryById(curBillMaster.Id);
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
                curBillMaster = model.StockInventorySrv.GetStockInventoryById(curBillMaster.Id);
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
            if (this.ExecType == CStockInventoryMng_ExecType.土建)
            {
                if (string.IsNullOrEmpty(txtMaterialCategory.Text) || txtMaterialCategory.Result == null && txtMaterialCategory.Result.Count == 0)
                {
                    MessageBox.Show("物资分类不能为空");
                    txtMaterialCategory.Focus();
                    return false;
                }
            }
            else if (this.ExecType == CStockInventoryMng_ExecType.安装)
            {
                if (this.txtUsedRank.Tag == null)
                {
                    MessageBox.Show("请选择使用队伍");
                    return false;
                }
                if (this.cboProfessionCat.SelectedItem == null)
                {
                    MessageBox.Show("请选择专业分类");
                    return false;
                }
                if (this.txtUserPart.Tag == null)
                {
                    MessageBox.Show("请选择使用部位");
                    return false;
                }
                if (this.txtUserPart.Tag == null)
                {
                    MessageBox.Show("请选择使用部位");
                    this.txtUserPart.Focus();
                    return false ;
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

                if (dr.Cells[colInventoryQuantity.Name].Value == null || dr.Cells[colInventoryQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colInventoryQuantity.Name].Value) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colInventoryQuantity.Name];
                    return false;
                }
                if (this.execType == CStockInventoryMng_ExecType.安装)
                {
                    //if (dr.Cells[this.colConfirmPrice.Name].Value == null || dr.Cells[colConfirmPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colConfirmPrice.Name].Value) <= 0)
                    //{
                    //    MessageBox.Show("认价单价不允许为空或小于等于0！");
                    //    dgDetail.CurrentCell = dr.Cells[colConfirmPrice.Name];
                    //    return false;
                    //}
                    if (dr.Cells[this.colSupplyPrice.Name].Value == null || dr.Cells[colSupplyPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colSupplyPrice.Name].Value) <= 0)
                    {
                        MessageBox.Show("采购单价不允许为空或小于等于0！");
                        dgDetail.CurrentCell = dr.Cells[colSupplyPrice.Name];
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
                curBillMaster.CreateDate = ClientUtil.ToDateTime(this.dtpSignDate.Text);
                if (this.execType == CStockInventoryMng_ExecType.安装)
                {
                    SupplierRelationInfo Supplier = this.txtUsedRank.Tag as SupplierRelationInfo;
                    if (Supplier != null)
                    {
                        curBillMaster.UsedRank = Supplier;
                        curBillMaster.UsedRankName = this.txtUsedRank.Text;
                        curBillMaster.ProfessionCategory = this.cboProfessionCat.SelectedItem.ToString();
                    }
                    curBillMaster.UserPart = this.txtUserPart.Tag as GWBSTree ;
                    curBillMaster.UserPartName = this.txtUserPart.Text;
                    curBillMaster .UserPartSysCode =curBillMaster.UserPart==null ? "":curBillMaster.UserPart.SysCode ;
                    curBillMaster.Special = "安装";
                }
                else if (this.execType == CStockInventoryMng_ExecType.土建)
                {
                    curBillMaster.Special = "土建";
                }
                curBillMaster.HandlePersonName = ClientUtil.ToString(this.txtHandlePerson.Text);
                //物资分类
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MatCatName = txtMaterialCategory.Text;
                }
                curBillMaster.Descript = this.txtDescript.Text;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    StockInventoryDetail curBillDtl = new StockInventoryDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as StockInventoryDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);//物资名称
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格型号
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);//计量单位
                    curBillDtl.InventoryQuantity = ClientUtil.TransToDecimal(var.Cells[colInventoryQuantity.Name].Value);//盘点数量
                    curBillDtl.StockQuantity = ClientUtil.ToDecimal(var.Cells[colStockQuantity.Name].Value);//库存数量
                    curBillDtl.MaterialClassify = ClientUtil.ToString(var.Cells[colMaterialClassify.Name].Value);//物资分类层次
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[colDiagramNum.Name].Value);
                    if (this.execType == CStockInventoryMng_ExecType.安装)
                    {
                        curBillDtl.Price = ClientUtil.ToDecimal(var.Cells[this.colSupplyPrice.Name].Value);
                        curBillDtl.ConfirmPrice = ClientUtil.ToDecimal(var.Cells[this.colConfirmPrice.Name].Value);
                        curBillDtl.ConfirmMoney = ClientUtil.ToDecimal(var.Cells[this.colConfirmMoney.Name].Value);
                        curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[this.colSupplyMoney.Name].Value);
                    }
                    DataGridViewComboBoxCell oDataGridViewComboBoxCell = var.Cells[colSubjectName.Name] as DataGridViewComboBoxCell;
                    DataTable oTable = oDataGridViewComboBoxCell.DataSource as DataTable;
                    if (oTable != null &&  oTable .Rows .Count >0)
                    {
                        string sGuidSysCode = oDataGridViewComboBoxCell.Value.ToString();

                        for (int i = 0; i < oTable.Rows.Count; i++)
                        {
                            if (string.Equals(oTable.Rows[i]["id"].ToString(), sGuidSysCode))
                            {
                                string[] arr = sGuidSysCode.Split('-');
                                curBillDtl.SubjectGuid = arr[0];
                                curBillDtl.SubjectName = oTable.Rows[i]["name"].ToString();
                                curBillDtl.SubjectSysCode = arr[1];
                            }
                        }

                        curBillMaster.AddDetail(curBillDtl);
                        var.Tag = curBillDtl;
                    }
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
            IList list = null;
            IList lstMatName = null;
            if (this.execType == CStockInventoryMng_ExecType.土建)
            {
                if (txtMaterialCategory.Result == null || txtMaterialCategory.Result.Count == 0)
                {
                    MessageBox.Show("请选择物资分类");
                    txtMaterialCategory.Focus();
                    return;
                }
            }
            else if (this.execType == CStockInventoryMng_ExecType.安装)
            {
                if (txtUsedRank.Tag == null)
                {
                    MessageBox.Show("请选择使用队伍");
                    this.btnSelectUsedRank.Focus();
                    return;
                }
                if (this.cboProfessionCat.SelectedItem == null)
                {
                    MessageBox.Show("请选择专业分类");
                    this.cboProfessionCat.Focus();
                    return;
                }
                if (this.txtUserPart.Tag == null)
                {
                    MessageBox.Show("请选择使用部位");
                    this.txtUserPart.Focus();
                    return;
                }
            }
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    if (this.execType == CStockInventoryMng_ExecType.土建)
                    {
                        CommonMaterial materialSelector = new CommonMaterial();
                        if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                        {
                            //MaterialCategory m = txtMaterialCategory.Result[0] as MaterialCategory;
                            //materialSelector.materialCatCode = m.Code;
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

                          list = materialSelector.Result;
                    }
                    else
                    {
                        VStockOutSelectList oVStockOutSearchList = new VStockOutSelectList(this.cboProfessionCat.SelectedItem.ToString(), txtUsedRank.Tag as SupplierRelationInfo  );
                        oVStockOutSearchList.ShowDialog();
                        list = oVStockOutSearchList.LstSelectResult;
                        lstMatName = oVStockOutSearchList.LstSelectMatUnit;
                        if (list != null && list.Count > 0)
                        {

                            list = modelStockOut.StockOutSrv.GetMaterialLst(list);
                             
                        }
                    }
                    foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                        this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                        this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                        this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                        //this.dgDetail[colSuff.Name, i].Value = theMaterial.Stuff;
                        this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                        if (theMaterial.BasicUnit != null)
                            this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                       
                        //填充均价
                        if (this.ExecType == CStockInventoryMng_ExecType.安装)
                        {
                            this.dgDetail[colUnit.Name, i].Value = GetUnitName(theMaterial, lstMatName);
                            DataTable oTable = model.StockInventorySrv.GetAvgPrice(theMaterial.Id, projectInfo.Id);
                            if (oTable != null && oTable.Rows.Count > 0)
                            {
                                this.dgDetail[colConfirmPrice.Name, i].Value = oTable.Rows[0]["confirmprice"].ToString();
                                this.dgDetail[colSupplyPrice.Name, i].Value = oTable.Rows[0]["price"].ToString();
                            }
                            DataTable  oTableSublect = model.StockInventorySrv.GetSubject(theMaterial.Id, projectInfo.Id);
                             DataGridViewComboBoxCell oDataGridViewComboBoxCell = this.dgDetail[colSubjectName.Name, i] as DataGridViewComboBoxCell;
                                oDataGridViewComboBoxCell.Items.Clear();
                                oDataGridViewComboBoxCell.DisplayMember ="name";
                                oDataGridViewComboBoxCell .ValueMember="id";
                                if (oTableSublect != null)
                                {
                                    oDataGridViewComboBoxCell.DataSource = oTableSublect;
                                    if (oTableSublect.Rows.Count > 0)
                                    {
                                        oDataGridViewComboBoxCell.Value = oTableSublect.Rows[0]["id"].ToString();
                                    }
                                    
                                }
                        }
                        //查询数据库信息，得出物资的库存数量
                        DataSet dataSet = null;
                        if (this.ExecType == CStockInventoryMng_ExecType.安装)
                        {
                            dataSet = model.stockRelationSrv.StockKCQuantity(ConstObject.LoginDate.Year, ConstObject.LoginDate.Month, projectInfo.Id, theMaterial.Code);
                        }
                        else
                        {
                            string condition = " and t1.projectid =  '" + projectInfo.Id + "'";
                            condition += "and t2.MaterialCode = '" + theMaterial.Code + "'";
                            dataSet = model.stockRelationSrv.StockKCQuantity(condition);
                        }
                        if (dataSet!=null&&dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            this.dgDetail[colStockQuantity.Name, i].Value = dataSet.Tables[0].Rows[0]["RemainQuantity"].ToString();
                        }
                        else
                        {
                            this.dgDetail[colStockQuantity.Name, i].Value = 0;
                        }
                        //foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                        //{
                        //    this.dgDetail[colStockQuantity.Name, i].Value = dataRow["RemainQuantity"].ToString();
                        //}
                        i++;
                    }
                }
               
            }
        }
        public string GetUnitName(Application.Resource.MaterialResource.Domain.Material oMaterial, IList lstMatName)
        {
            string sUnitName = string.Empty;
            string sMatID = string.Empty;
            if (oMaterial != null && lstMatName != null)
            {
                for (int i = 0; i < lstMatName.Count; i += 2)
                {
                    sMatID = lstMatName[i] as string;
                    if (string.Equals(sMatID, oMaterial.Id))
                    {
                        sUnitName = lstMatName[i + 1] as string ;
                        break;
                    }
                }
            }
            return sUnitName;
        }
        public bool ValidateInt(DataGridViewCell oCell,ref int iValue)
        {
            bool bFlag = true ;
             
            try
            {
                string sValue = oCell.Value.ToString();
                if (string.IsNullOrEmpty(sValue.Trim()))
                {
                    iValue = 0;
                    bFlag = true;
                    return bFlag;
                }
                int.Parse(oCell.Value .ToString ());
            }
            catch
            {
                MessageBox.Show("请输入整型");
                oCell.Selected=true ;
                bFlag = false;
            }
            return bFlag;
        }
        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                dtpSignDate.Value = curBillMaster.CreateDate ;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                //txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtSumQuantity.Text = ClientUtil.ToString(curBillMaster.SumQuantity.ToString("#,###.####"));
                this.txtProject.Text = ClientUtil.ToString(curBillMaster.ProjectName);
                this.txtDescript.Text = ClientUtil.ToString(curBillMaster.Descript);
                if (this.execType == CStockInventoryMng_ExecType.安装)
                {

                    if (curBillMaster.UsedRank  != null)
                    {
                        SupplierRelationInfo Supplier = curBillMaster.UsedRank;
                        this.txtUsedRank.Tag = Supplier;
                        this.txtUsedRank.Text = Supplier.SupplierInfo.Name;
                    }
                    this.cboProfessionCat.SelectedItem  = curBillMaster.ProfessionCategory;
                    this.txtUserPart.Text = curBillMaster.UserPartName;
                    this.txtUserPart.Tag = curBillMaster.UserPart ;
                }
                else if (this.execType == CStockInventoryMng_ExecType.土建 )
                {
                    this.cboProfessionCat.SelectedText = curBillMaster.Special;
                }
                //物资分类
                if (curBillMaster.MaterialCategory != null)
                {
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);
                    txtMaterialCategory.Value = curBillMaster.MatCatName;
                }
                this.dgDetail.Rows.Clear();
                foreach (StockInventoryDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnit.Name;
                    this.dgDetail[colInventoryQuantity.Name, i].Value = var.InventoryQuantity;
                    this.dgDetail[colMaterialClassify.Name, i].Value = var.MaterialClassify;
                    this.dgDetail[colStockQuantity.Name, i].Value = var.StockQuantity;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[colDiagramNum.Name, i].Value = var.DiagramNumber;
                    this.dgDetail.Rows[i].Tag = var;
                    if (this.execType == CStockInventoryMng_ExecType.安装)
                    {
                        this.dgDetail[this.colConfirmMoney.Name, i].Value = var.ConfirmMoney;
                        this.dgDetail[this.colConfirmPrice .Name, i].Value = var.ConfirmPrice ;
                        this.dgDetail[this.colSupplyMoney .Name, i].Value = var.Money ;
                        this.dgDetail[this.colSupplyPrice .Name, i].Value = var.Price ;
                    }
                    DataGridViewComboBoxCell oDataGridViewComboBoxCell = this.dgDetail[colSubjectName.Name, i] as DataGridViewComboBoxCell;
                     
                      
                    oDataGridViewComboBoxCell.Items.Clear();
                    oDataGridViewComboBoxCell.DisplayMember = "name";
                    oDataGridViewComboBoxCell.ValueMember = "id";
                    DataTable oTable = new DataTable();
                    string sSysCode = var.SubjectGuid + "-" + var.SubjectSysCode;
                    oTable.Columns.Add("name");
                    oTable.Columns.Add("id");
                    DataRow oRow = oTable.NewRow();
                    oRow["name"] = var.SubjectName;
                    oRow["id"] = sSysCode;
                    oTable .Rows .Add (oRow );
                    oDataGridViewComboBoxCell.DataSource = oTable;
                    oDataGridViewComboBoxCell.Value = sSysCode;
                  
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
            bool validity = false;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            bool flag = true;
            decimal sumInventQuan = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                if (colName == colInventoryQuantity.Name)
                {
                    string quantity = ClientUtil.ToString(dgDetail.Rows[i].Cells[colInventoryQuantity.Name].Value);
                    
                    if (quantity == null)
                    {
                        quantity = "0";
                        return;
                    }
                    validity = CommonMethod.VeryValid(quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        flag = false;
                    }
                    decimal dQuantity = ClientUtil.ToDecimal(quantity);
                    
                    sumInventQuan += dQuantity;
                   
                    //sumInventQuan += ClientUtil.ToDecimal(quantity);
                    txtSumQuantity.Text = sumInventQuan.ToString();
                     
                }   
            }
            if (colName == colStockQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colStockQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colStockQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        flag = false;
                    }
                }
                
            }
            if (colName == colInventoryQuantity.Name)
            {
                string quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colInventoryQuantity.Name].Value);
                string sConfirmPrice = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[this.colConfirmPrice.Name].Value);
                decimal dConfirmPrice = ClientUtil.ToDecimal(sConfirmPrice);
                decimal dQuantity = ClientUtil.ToDecimal(quantity);
                dgDetail.Rows[e.RowIndex].Cells[this.colConfirmMoney.Name].Value = dQuantity * dConfirmPrice;

                //string quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colInventoryQuantity.Name].Value);
                string sPrice = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[this.colSupplyPrice.Name].Value);
                decimal dSupplyPrice = ClientUtil.ToDecimal(sPrice);
                  dQuantity = ClientUtil.ToDecimal(quantity);
                dgDetail.Rows[e.RowIndex].Cells[this.colSupplyMoney.Name].Value = dQuantity * dSupplyPrice;
            }

            if (colName == colConfirmPrice .Name )
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colConfirmPrice.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colConfirmPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        flag = false;
                    }
                }
                string quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colInventoryQuantity.Name].Value);
                string sConfirmPrice = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[this.colConfirmPrice.Name].Value);
                decimal dConfirmPrice = ClientUtil.ToDecimal(sConfirmPrice);
                decimal dQuantity = ClientUtil.ToDecimal(quantity);
                dgDetail.Rows[e.RowIndex].Cells[this.colConfirmMoney .Name].Value = dQuantity * dConfirmPrice;
            }
            if (colName ==this.colSupplyPrice .Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        flag = false;
                    }
                }
                string quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colInventoryQuantity.Name].Value);
                string sPrice = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[this.colSupplyPrice.Name].Value);
                decimal dSupplyPrice = ClientUtil.ToDecimal(sPrice);
                decimal dQuantity = ClientUtil.ToDecimal(quantity);
                dgDetail.Rows[e.RowIndex].Cells[this.colSupplyMoney .Name].Value = dQuantity * dSupplyPrice;
            }
            
        }
    


        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
        //    return true;
        //}

        //private bool LoadTempleteFile(string modelName)
        //{
        //    ExploreFile eFile = new ExploreFile();
        //    string path = eFile.Path;
        //    if (eFile.IfExistFileInServer(modelName))
        //    {
        //        eFile.CreateTempleteFileFromServer(modelName);
        //        //载入格式和数据
        //        flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
        //    }
        //    else
        //    {
        //        MessageBox.Show("未找到模板格式文件【" + modelName + "】");
        //        return false;
        //    }
        //    return true;
        //}

        //private void FillFlex(MaterialRentalOrderMaster billMaster)
        //{
        //    int detailStartRowNumber = 7;//7为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;

        //    //主表数据

        //    flexGrid1.Cell(2, 1).Text = "使用单位：";
        //    flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
        //    flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
        //    flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
        //    flexGrid1.Cell(4, 5).Text = "";//合同名称
        //    flexGrid1.Cell(4, 7).Text = "";//材料分类
        //    flexGrid1.Cell(5, 2).Text = "";//租赁单位
        //    flexGrid1.Cell(5, 2).WrapText = true;
        //    flexGrid1.Cell(5, 5).Text = "";//承租单位
        //    flexGrid1.Row(5).AutoFit();
        //    flexGrid1.Cell(5, 7).Text = billMaster.RealOperationDate.ToShortDateString();//签订日期

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //结算规则
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = Enum.GetName(typeof(EnumMaterialMngBalRule), detail.BalRule);
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //数量
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

        //        //日租金
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

        //        //金额
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = "";

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //    }
        //}
    }
}
