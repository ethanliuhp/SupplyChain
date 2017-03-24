using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockRelationUI;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public partial class VProfitIn : TMasterDetailView
    {
        MProfitIn theMProfitIn = new MProfitIn();
        private MStockMng modelStockIn = new MStockMng();
        public ProfitIn theProfitIn = new ProfitIn();
        private   EnumStockExecType execType;
        public EnumStockExecType ExecType
        {
            get { return execType; }
            set { execType = value; }
        }
        public ProfitIn CurrProfitIn
        {
            get { return this.theProfitIn; }
            set { this.theProfitIn = value; }
        }
        public VProfitIn()
        {
            InitializeComponent();
            Title = "盘盈单";
            InitEvent();
            InitData();
        }
        public VProfitIn(EnumStockExecType ExecType)
        {
            this.ExecType = ExecType;
            InitializeComponent();
            Title = "盘盈单";
            InitEvent();
            InitData();
        }

        public void InitEvent()
        {
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidated += new DataGridViewCellEventHandler(dgDetail_CellValidated);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
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
                    theProfitIn.Details.Remove(dr.Tag as ProfitInDtl);
                }
            }
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string columName = dgDetail.Columns[e.ColumnIndex].Name;
            bool valididy = true;
            if (columName == "Quantity" || columName == "Price")
            {
                if (dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                    valididy = CommonMethod.VeryValid(temp_quantity);
                    if (valididy == false)
                        MessageBox.Show("请输入数字！");
                }

                if (dgDetail.Rows[e.RowIndex].Cells["Price"].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                    valididy = CommonMethod.VeryValid(temp_price);
                    if (valididy == false)
                        MessageBox.Show("请输入数字！");
                }
            }
        }

        void dgDetail_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name == "Price" ||
                this.dgDetail.Columns[e.ColumnIndex].Name == "Quantity")
            {
                this.dgDetail["Money", e.RowIndex].Value = ClientUtil.TransToDecimal(this.dgDetail["Price", e.RowIndex].Value) *
                                                           ClientUtil.TransToDecimal(this.dgDetail["Quantity", e.RowIndex].Value);
                this.dgDetail["Money", e.RowIndex].Value = ClientUtil.TransToDecimal(this.dgDetail["Money", e.RowIndex].Value).ToString("#,###.##");
            }
        }

        public void InitData()
        {
            switch (this.ExecType)
            {
                case EnumStockExecType.安装:
                    {
                        this.lblSpecailType.Visible = true;
                        this.comSpecailType.Visible = true;
                        //添加专业分类下拉框
                        VBasicDataOptr.InitProfessionCategory(comSpecailType, false);
                        break;
                    }
                default:
                    {
                        this.lblSpecailType.Visible = false;
                        this.comSpecailType.Visible = false;
                        break;
                    }
            }
            dgDetail.ContextMenuStrip = cmsDg;
            

        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex<0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals("MaterialCode"))
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];

                    object tempValue = cell.EditedFormattedValue;
                    if (tempValue != null && !tempValue.Equals(""))
                    {
                        materialSelector.OpenSelect(tempValue.ToString());
                    }
                    else
                    {
                        materialSelector.OpenSelect();
                    }

                    IList list = materialSelector.Result;
                    Material selectedMaterial = null;
                    foreach (Material theMaterial in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail["MaterialCode", i].Tag = theMaterial;
                        this.dgDetail["MaterialCode", i].Value = theMaterial.Code;
                        this.dgDetail["MaterialName", i].Value = theMaterial.Name;
                        this.dgDetail["MaterialSpec", i].Value = theMaterial.Specification;
                        this.dgDetail["Stuff", i].Value = theMaterial.Stuff;
                        this.dgDetail["Quantity", i].Value = 0;
                        this.dgDetail["Unit", i].Tag = theMaterial.BasicUnit;
                        if (theMaterial.BasicUnit != null)
                            this.dgDetail["Unit", i].Value = theMaterial.BasicUnit.Name;

                        this.dgDetail.Rows[i].Tag = theMaterial;
                        i++;
                    }
                }
            }
        }

        void cbStockMoveType_SelectedIndexChanged(Object sender, EventArgs e)
        {
            ClearView();
        }

        void btnForward_Click(Object sender, EventArgs e)
        {
        }

        public void Start(string code)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    theProfitIn = theMProfitIn.GetObject(code,Enum.GetName(typeof(EnumStockExecType),execType),StaticMethod.GetProjectInfo().Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e)); 
            }
        }
        public void StartByID(string sID)
        {
            try
            {
                if (sID == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    theProfitIn = theMProfitIn.GetObjectById(sID);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate ,txtProject};
            ObjectLock.Lock(os);
            string[] lockCols = new string[] { MaterialName.Name, MaterialSpec.Name, Unit.Name,Stuff.Name,Money.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
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

        public override bool NewView()
        {
            try
            {
                //base.NewView();
                //theMProfitIn.New();
                //theProfitIn = new ProfitIn();
                //ClearView();
                //theProfitIn.Code = DateTime.Now.TimeOfDay.ToString();
                //txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                //txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                base.NewView();
                ClearView();
                this.theProfitIn = new ProfitIn();
                theProfitIn.CreatePerson = ConstObject.LoginPersonInfo;
                theProfitIn.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                theProfitIn.CreateDate = ConstObject.LoginDate;
                theProfitIn.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                theProfitIn.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                theProfitIn.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                theProfitIn.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                theProfitIn.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                //curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                //curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                theProfitIn.DocState = DocumentState.Edit;
                theProfitIn.TheStationCategory = StaticMethod.GetStationCategory();

                theProfitIn.Special = Enum.GetName(typeof(EnumStockExecType), execType);

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
               // txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    theProfitIn.ProjectId = projectInfo.Id;
                    theProfitIn.ProjectName = projectInfo.Name;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        public override bool ModifyView()
        {
            if (theProfitIn.IsTally == 1)
            {
                MessageBox.Show("此单已记账，不能修改！");
                return false;
            }
            base.ModifyView();
            theProfitIn = theMProfitIn.GetObjectById(theProfitIn.Id);
            ModelToView();
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            bool isSaveSuccess = false;
            try
            {
                if (!ViewToModel()) return false;
               
                if (theProfitIn.Id == null)
                {
                    theProfitIn = theMProfitIn.Save(theProfitIn);
                    //插入日志
                    StaticMethod.InsertLogData(theProfitIn.Id, "新增", theProfitIn.Code, ConstObject.LoginPersonInfo.Name, "盘盈单", "", theProfitIn.ProjectName);
                }
                else
                {
                    theProfitIn = theMProfitIn.Save(theProfitIn);
                    //插入日志
                    StaticMethod.InsertLogData(theProfitIn.Id, "修改", theProfitIn.Code, ConstObject.LoginPersonInfo.Name, "盘盈单", "", theProfitIn.ProjectName);
                }
                //theMProfitIn = theMProfitIn.SaveProfitInMaster(theMProfitIn);
                txtCode.Text = theProfitIn.Code;
                //txtHandlePerson.Text = 
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                isSaveSuccess = true;
                DialogResult aa = MessageBox.Show("保存成功，是否记账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (aa == DialogResult.Yes)
                {
                    Hashtable hashTally = new Hashtable();
                    Hashtable hashCode = new Hashtable();
                    IList lstTally = new ArrayList();
                    IList lstCode = new ArrayList();
                    lstTally.Add(theProfitIn.Id);
                    lstCode.Add(theProfitIn.Code);

                    hashTally.Add("ProfitIn", lstTally);
                    hashCode.Add("ProfitInCode", lstCode);

                    Hashtable dicList = theMProfitIn.Tally(hashTally, hashCode);
                    string errMsg = ClientUtil.ToString(dicList["err"]);
                    //报错
                    if (errMsg != "")
                        MessageBox.Show(errMsg);
                    else
                    {
                        theProfitIn.IsTally = 1;
                        MessageBox.Show("记账成功！");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e)); ;
                return isSaveSuccess;
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
                if (theProfitIn.IsTally == 1)
                {
                    MessageBox.Show("此单已记账，不能删除！");
                    return false;
                }

                if (!theMProfitIn.Delete(theProfitIn)) return false;
                //插入日志
                StaticMethod.InsertLogData(theProfitIn.Id, "删除", theProfitIn.Code, ConstObject.LoginPersonInfo.Name, "盘盈单", "", theProfitIn.ProjectName);
                ClearView();
                return true;
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
                        theProfitIn = theMProfitIn.GetObjectById(theProfitIn.Id);
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
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e)); ;
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
                theProfitIn = theMProfitIn.GetObjectById(theProfitIn.Id);
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
            if (this.txtSupplier.Result == null || this.txtSupplier.Text == "")
            {
                MessageBox.Show("供应商不能为空！");
                return false;
            }

            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空!");
                return false;
            }

            //this.dgDetail.CurrentCell = this.dgDetail[0, 0];
            //SupplierRelationInfo tmpSupplierRelationInfo = this.txtSupplier.Result[0] as SupplierRelationInfo;

            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells["MaterialCode"].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells["MaterialCode"];
                    return false;
                }

                if (dr.Cells["Quantity"].Value == null || dr.Cells["Quantity"].Value.ToString() == "")
                {
                    MessageBox.Show("数量不允许为空！");
                    dgDetail.CurrentCell = dr.Cells["Quantity"];
                    return false;
                }

                if (dr.Cells["Remark"].Value == null)
                    dr.Cells["Remark"].Value = "";
            }
            dgDetail.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
            return true;
        }
        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;

            try
            {
                this.txtCode.Focus();
                theProfitIn.CreateDate = dtpDateBegin .Value ;

                DataTable oTable = modelStockIn.StockInSrv.GetFiscaDate(theProfitIn.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    theProfitIn.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    theProfitIn.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }

                if (this.txtSupplier.Result.Count > 0 || this.txtSupplier.Text.ToString() != "")
                {
                    theProfitIn.TheSupplierRelationInfo = this.txtSupplier.Result[0] as SupplierRelationInfo;
                    theProfitIn.TheSupplierName = txtSupplier.Text;
                }
                 
                theProfitIn.CreateDate  =dtpDateBegin.Value;
                theProfitIn.LastModifyDate = CommonMethod.GetServerDateTime();   
                theProfitIn.Descript = this.txtRemark.Text;
                if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
                {
                    theProfitIn.HandlePerson = txtHandlePerson.Result[0] as PersonInfo;
                    theProfitIn.HandlePersonName = txtHandlePerson.Text;
                } else
                {
                    theProfitIn.HandlePerson = null;
                    theProfitIn.HandlePersonName = null;
                }
                if (this.comSpecailType.Visible)
                {
                    theProfitIn.Special = this.comSpecailType.Text;
                }
                foreach (DataGridViewRow dr in this.dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    ProfitInDtl theProfitInDtl = new ProfitInDtl();
                    theProfitInDtl = dr.Tag as ProfitInDtl;
                    if (theProfitInDtl == null)
                        theProfitInDtl = new ProfitInDtl();
                    else if (theProfitInDtl.Id == null)
                    {
                        theProfitIn.Details.Remove(theProfitInDtl);
                    }

                    theProfitInDtl.MaterialResource = dr.Cells["MaterialCode"].Tag as Material;
                    theProfitInDtl.MaterialCode = ClientUtil.ToString(dr.Cells["MaterialCode"].Value);
                    theProfitInDtl.MaterialName=ClientUtil.ToString(dr.Cells["MaterialName"].Value);
                    theProfitInDtl.MaterialSpec = ClientUtil.ToString(dr.Cells[MaterialSpec.Name].Value);
                    theProfitInDtl.MaterialStuff = ClientUtil.ToString(dr.Cells["Stuff"].Value);
                    
                    theProfitInDtl.MatStandardUnit = dr.Cells["Unit"].Tag as StandardUnit;
                    theProfitInDtl.MatStandardUnitName = ClientUtil.ToString(dr.Cells["Unit"].Value);

                    theProfitInDtl.Quantity = StringUtil.StrToDecimal(ClientUtil.ToString(dr.Cells["Quantity"].Value));
                    theProfitInDtl.Price = StringUtil.StrToDecimal(ClientUtil.ToString(dr.Cells["Price"].Value));
                    theProfitInDtl.Money = StringUtil.StrToDecimal(ClientUtil.ToString(dr.Cells["Money"].Value));
                    theProfitInDtl.Descript = ClientUtil.ToString(dr.Cells["Remark"].Value);
                    theProfitInDtl.DiagramNumber = ClientUtil.ToString(dr.Cells["DiagramNum"].Value);
                    theProfitIn.AddDetail(theProfitInDtl);
                    dr.Tag = theProfitInDtl;
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
                this.txtCode.Text = theProfitIn.Code;
                this.txtCode.Focus();
                this.txtSupplier.Result.Clear();
                if (theProfitIn.TheSupplierRelationInfo != null)
                {
                    this.txtSupplier.Result.Add(theProfitIn.TheSupplierRelationInfo);
                    txtSupplier.Tag = theProfitIn.TheSupplierRelationInfo;
                    this.txtSupplier.Value   = theProfitIn.TheSupplierName;
                }

                dtpDateBegin.Value = theProfitIn.CreateDate ;

                this.txtHandlePerson.Value  = theProfitIn.HandlePersonName;
                if (theProfitIn.HandlePerson != null)
                {
                    txtHandlePerson.Result.Clear();
                    txtHandlePerson.Result.Add(theProfitIn.HandlePerson);
                    txtHandlePerson.Tag = theProfitIn.HandlePerson;
                }

                this.txtRemark.Text = theProfitIn.Descript;
                this.txtCreatePerson.Text = theProfitIn.CreatePersonName;
               // this.txtCreateDate.Text = theProfitIn.CreateDate.ToShortDateString();
                this.txtProject.Text = theProfitIn.ProjectName;
                //专业分类
                if (this.comSpecailType.Visible)
                {
                    this.comSpecailType.Text = this.theProfitIn.Special;
                }
                //明细
                this.dgDetail.Rows.Clear();
                foreach (ProfitInDtl smDtl in theProfitIn.Details)
                {
                    int i = dgDetail.Rows.Add();
                    DataGridViewRow row = dgDetail.Rows[i];

                    Material material = smDtl.MaterialResource;

                    row.Cells["MaterialCode"].Tag = material;
                    row.Cells["MaterialCode"].Value = smDtl.MaterialCode;
                    row.Cells["MaterialName"].Value = smDtl.MaterialName;
                    row.Cells["MaterialSpec"].Value = smDtl.MaterialSpec;
                    row.Cells["Stuff"].Value = smDtl.MaterialStuff;

                    //设置该物料的计量单位
                    row.Cells["Unit"].Tag = smDtl.MatStandardUnit;
                    row.Cells["Unit"].Value = smDtl.MatStandardUnitName;
                    row.Cells["Quantity"].Value = smDtl.Quantity;
                    row.Cells["Price"].Value = smDtl.Price;
                    row.Cells["Money"].Value = smDtl.Money.ToString("########.##");          
                    row.Cells["Remark"].Value = smDtl.Descript;
                    row.Cells["DiagramNum"].Value = smDtl.DiagramNumber;
                    row.Tag = smDtl;
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + ExceptionUtil.ExceptionMessage(e));;
                //return false;
            }
        }

        public override bool Preview()
        {
            //VBillPrint vprint = new VBillPrint(BillType.btStockProfit, "盘盈单打印", this.txtCode.Text, true);
            //vprint.ShowDialog();
            //theMProfitIn.Preview();
            return true;
        }

        public override bool Print()
        {
            //VBillPrint vprint = new VBillPrint(BillType.btStockProfit, "盘盈单打印", this.txtCode.Text, false);
            //vprint.ShowDialog();
            //theMProfitIn.Print();
            return true;
        }
    }
}

