using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI
{
    public partial class VStockInRed : TMasterDetailView
    {
        private MStockMng model = new MStockMng();
        private StockInRed curBillMaster;
        private StockInBalRedMaster curBillBalMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        private EnumStockExecType execType;
        public EnumStockExecType ExecType
        {
            get { return execType; }
            set { execType = value; }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public StockInRed CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VStockInRed()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            //冲红类型
            InitForRedType();

            dgDetail.ContextMenuStrip = cmsDg;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
       
        }

        private void InitForRedType()
        {
            foreach (EnumForRedType redType in Enum.GetValues(typeof(EnumForRedType)))
            {
                cboForRedType.Items.Add(redType);
            }
            if (cboForRedType.Items.Count > 0)
            {
                cboForRedType.SelectedItem = cboForRedType.Items[0];
            }
        }

        private void InitEvent()
        {
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            //引用前驱
            btnForward.Click += new EventHandler(btnForward_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            InitControls();
        }

        /// <summary>
        /// 根据土建、安装 界面不同显示
        /// </summary>
        private void InitControls()
        {
            if (ExecType == EnumStockExecType.土建)
            {
                lblDate.Text = "取价日期:";
                colProfessionalCategory.Visible = false;

                lblCat.Text = "物资分类:";
                txtMaterialCategory.BringToFront();
                txtMaterialCategory.Visible = true;
                txtProfessionCategory.Visible = false;
            }
            else if (ExecType == EnumStockExecType.安装)
            {
                lblDate.Text = "收料日期:";
                colProfessionalCategory.Visible = true;

                lblCat.Text = "专业分类:";
                txtMaterialCategory.Visible = false;
                txtProfessionCategory.Visible = true;
            }
        }

        void btnForward_Click(object sender, EventArgs e)
        {
            VStockInSelector4Red vsos = new VStockInSelector4Red(ExecType);
            vsos.ShowDialog();
            IList list = vsos.Result;
            if (list == null || list.Count == 0) return;

            StockIn tempMaster = list[0] as StockIn;
            txtForward.Tag = tempMaster.Id;
            txtForward.Text = tempMaster.Code;
            if (tempMaster.TheSupplierRelationInfo != null)
            {
                txtSupply.Result.Clear();
                txtSupply.Result.Add(tempMaster.TheSupplierRelationInfo);
                txtSupply.Text = tempMaster.TheSupplierName;
            }
            this.dtpDateBegin.Value = tempMaster.CreateDate;
            //物资分类
            if (tempMaster.MaterialCategory != null)
            {
                txtMaterialCategory.ReadOnly = false;
                txtMaterialCategory.Result.Clear();
                txtMaterialCategory.Result.Add(tempMaster.MaterialCategory);
                txtMaterialCategory.Value = tempMaster.MatCatName;
                txtMaterialCategory.ReadOnly = true;
            }
            //专业分类
            txtProfessionCategory.Text = tempMaster.ProfessionCategory;

            //处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                StockInRedDtl dtl = dr.Tag as StockInRedDtl;
                if (dtl != null)
                {
                    if (CurBillMaster != null)
                    {
                        CurBillMaster.Details.Remove(dtl);
                        if (dtl.Id != null)
                        {
                            movedDtlList.Add(dtl);
                        }
                    }
                }
            }

            //显示引用的明细
            this.dgDetail.Rows.Clear();
            foreach (StockInDtl var in tempMaster.Details)
            {
                if (var.IsSelect == false) continue;
                int i = this.dgDetail.Rows.Add();

                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colMaterialStuff.Name, i].Value = var.MaterialStuff;
                this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                if (this.ExecType == EnumStockExecType.安装)
                {
                    this.dgDetail[colCanUseQty.Name, i].Value = var.Quantity - var.RefQuantity ;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity - var.RefQuantity ;
                }
                else
                {   
                    this.dgDetail[colCanUseQty.Name, i].Value = var.Quantity - var.RefQuantity - var.BalQuantity;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity - var.RefQuantity - var.BalQuantity;
                }
                this.dgDetail[colPrice.Name, i].Value = var.Price;
                this.dgDetail[colNewPrice.Name, i].Value = var.Price;
                this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;
                this.dgDetail[colProfessionalCategory.Name, i].Value = var.ProfessionalCategory;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                this.dgDetail[colUsePart.Name, i].Value = var.UsedPartName;
                this.dgDetail[colUsePart.Name, i].Tag = var.UsedPart;
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
                    curBillMaster.Details.Remove(dr.Tag as BaseDetail);
                    movedDtlList.Add(dr.Tag as StockInRedDtl);
                }
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
                    curBillMaster = model.StockInSrv.GetStockInRedByCode(code, Enum.GetName(typeof(EnumStockExecType), ExecType), StaticMethod.GetProjectInfo().Id);
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
        public void Start(string code, string Id)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.StockInSrv.GetStockInRedById(Id);
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
            cboForRedType.Enabled = false;

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtSumMoney, txtForward, txtProfessionCategory };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialStuff.Name, colMaterialCode.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colCanUseQty.Name, colPrice.Name, colProfessionalCategory.Name };
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
                movedDtlList = new ArrayList();

                this.curBillMaster = new StockInRed();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                curBillMaster.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                curBillMaster.StockInManner = EnumStockInOutManner.收料入库;
                curBillMaster.Special =Enum.GetName(typeof(EnumStockExecType), ExecType);

                //仓库
                curBillMaster.TheStationCategory = StaticMethod.GetStationCategory();

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

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.IsTally == 1)
            {
                MessageBox.Show("此单己记账，不能修改！");
                return false;
            }
            movedDtlList = new ArrayList();
            base.ModifyView();
            curBillMaster = model.StockInSrv.GetStockInRedById(curBillMaster.Id);
            ModelToView();
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
                if (curBillMaster.Id == null)
                {
                    curBillMaster = model.StockInSrv.SaveStockInRed(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "收料入库红单", "", curBillMaster.ProjectName);
                    //if (this.execType == EnumStockExecType.安装)
                    //{
                    //    this.CreateStockInBalMaster();
                    //    this.curBillBalMaster = model.StockInSrv.SaveStockInBalRedMaster(curBillBalMaster, movedDtlList);
                    //    //插入日志
                    //    StaticMethod.InsertLogData(curBillBalMaster.Id, "新增", curBillBalMaster.Code, ConstObject.LoginPersonInfo.Name, "验收结算单红单", "", curBillBalMaster.ProjectName);
                    //}
                }
                else
                {
                    curBillMaster = model.StockInSrv.SaveStockInRed(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "收料入库红单", "", curBillMaster.ProjectName);
                }
                movedDtlList = new ArrayList();

                
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                try
                {
                    Tally();
                    if (curBillMaster.Special == "安装")
                    {
                        foreach (StockInRedDtl dtl in curBillMaster.Details)
                        {
                            BackResourcePlan(dtl);
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("记账出错。"+ex.Message);
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        public void BackResourcePlan(StockInRedDtl dtl)
        {
            if (dtl.ForwardDetailId != null)
            {
                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddCriterion(Expression.Eq("MaterialResource.Id", dtl.MaterialResource.Id));
                if (dtl.DiagramNumber == "")
                {
                    oq1.AddCriterion(Expression.IsNull("DiagramNumber"));
                }
                else
                {
                    oq1.AddCriterion(Expression.Eq("DiagramNumber", dtl.DiagramNumber));
                }

                //oq1.AddCriterion(Expression.Eq("TheGWBSTaskGUID.Id", dtl.UsedPart.Id));
                IList list = model.StockInSrv.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                if (list == null || list.Count == 0) return;
                foreach (ResourceRequirePlanDetail plan in list)
                {
                    plan.ExecutedQuantity += dtl.Quantity;
                }
                model.StockInSrv.SaveOrUpdateByDao(list);
            }
        }

        /// <summary>
        /// 记账
        /// </summary>
        private void Tally()
        {
            //记账
            //if (MessageBox.Show("保存成功，是否记账", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //{
            IList billIdList = new ArrayList();
            billIdList.Add(curBillMaster.Id);

            IList billCodeList = new ArrayList();
            billCodeList.Add(curBillMaster.Code);

            Hashtable hashBillId = new Hashtable();
            hashBillId.Add("StockInRed", billIdList);

            Hashtable hashBillCode = new Hashtable();
            hashBillCode.Add("StockInRed", billCodeList);

            Hashtable tallyResult = model.TallyStockIn(hashBillId, hashBillCode);
            if (tallyResult != null)
            {
                string errMsg = (string)tallyResult["err"];
                if (!string.IsNullOrEmpty(errMsg))
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    curBillMaster.IsTally = 1;
                    MessageBox.Show("记账成功。");
                }
            }
            //}
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                if (curBillMaster.IsTally == 1)
                {
                    MessageBox.Show("此单己记账，不能删除！");
                    return false;
                }                
                model.StockInSrv.DeleteStockInRed(curBillMaster);
                //插入日志
                StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "收料入库红单", "", curBillMaster.ProjectName);
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
                        curBillMaster = model.StockInSrv.GetStockInRedById(curBillMaster.Id);
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
                curBillMaster = model.StockInSrv.GetStockInRedById(curBillMaster.Id);
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
            if (this.txtForward.Text == "")
            {
                MessageBox.Show("前驱单据不能为空！");
                return false;
            }

            if (cboForRedType.SelectedItem == null)
            {
                MessageBox.Show("冲红类型不能为空。");
                cboForRedType.Focus();
                return false;
            }

            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
              
            //MStockMng model = new MStockMng();
            DateTime oDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(dtpDateBegin.Value).ToShortDateString());
            if (oDate > ConstObject.TheLogin.FiscalModule.EndDate)
            {
                MessageBox.Show("当前的业务日期[" + oDate.ToShortDateString() + "]必须小于本会计期的最后日期[" + ConstObject.TheLogin.FiscalModule.EndDate.ToShortDateString() + "]!");
                return false;
            }
            string sMsg = model.StockInSrv.CheckAccounted(ConstObject.TheLogin.TheAccountOrgInfo, ClientUtil.ToDateTime(this.dtpDateBegin.Text), StaticMethod.GetProjectInfo().Id);
            if (sMsg != string.Empty)
            {
                MessageBox.Show(sMsg);
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            IList materialList = new ArrayList();//校验业务日期
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
                else
                {
                    Material material = dr.Cells[colMaterialCode.Name].Tag as Material;
                    materialList.Add(material.Id);
                }

                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }

                object objQty = dr.Cells[colQuantity.Name].Value;
                if (objQty == null || objQty.ToString() == "" || !CommonMethod.VeryValid(objQty.ToString()) || ClientUtil.TransToDecimal(objQty) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                StockInDtl forwardDetail = model.StockInSrv.GetStockInDtl(forwardDtlId.ToString());
                if (forwardDetail == null)
                {
                    MessageBox.Show("未找到前驱单据明细,请重新引用。");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                } else
                {
                    decimal canUseQty = 0;
                    if (this.execType == EnumStockExecType.安装)
                    {
                        canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;
                    }
                    else
                    {
                        canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity - forwardDetail.BalQuantity;
                    }
                    decimal currentQty = decimal.Parse(dr.Cells[colQuantity.Name].Value.ToString());
                    object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;
                    decimal qtyTemp = 0;
                    if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                    {
                        qtyTemp = decimal.Parse(qtyTempObj.ToString());
                    }

                    if (currentQty - qtyTemp - canUseQty > 0)
                    {
                        MessageBox.Show("输入数量【" + currentQty + "】大于可冲红数量【" + (canUseQty + qtyTemp) + "】。");
                        dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                        return false;
                    }
                }

                object newPrice = dr.Cells[colNewPrice.Name].Value;
                if (newPrice == null || newPrice.ToString() == "") newPrice = "0";
                if (!CommonMethod.VeryValid(newPrice.ToString()) || ClientUtil.TransToDecimal(newPrice) < 0)
                {
                    MessageBox.Show("请输入正确的新单价。");
                    dgDetail.CurrentCell = dr.Cells[colNewPrice.Name];
                    return false;
                }
                if (decimal.Parse(newPrice.ToString()) == 0)
                {
                    if ((EnumForRedType)cboForRedType.SelectedItem == EnumForRedType.冲单价)
                    {
                        MessageBox.Show("新单价不能为0。");
                        dgDetail.CurrentCell = dr.Cells[colNewPrice.Name];
                        return false;
                    }
                }
            }
            if (ExecType == EnumStockExecType.土建)
            {
                sMsg = model.StockInSrv.CheckBusinessDate(ConstObject.TheLogin.TheAccountOrgInfo, ClientUtil.ToDateTime(this.dtpDateBegin.Text), materialList, StaticMethod.GetProjectInfo().Id);
                if (sMsg != string.Empty)
                {
                    MessageBox.Show(sMsg);
                    return false;
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
                curBillMaster.ForwardBillId = txtForward.Tag as string;
                curBillMaster.ForwardBillCode = txtForward.Text;
                curBillMaster.ForwardBillType = "StockIn";
                
                if (this.txtSupply.Result.Count > 0)
                {
                    curBillMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    curBillMaster.TheSupplierName = txtSupply.Text;
                }
                curBillMaster.CreateDate = ClientUtil.ToDateTime(dtpDateBegin.Value.Date.ToShortDateString());
                curBillMaster.Descript = this.txtRemark.Text;

                DataTable oTable = model.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }
    
                curBillMaster.SumMoney =-Math.Abs( ClientUtil.TransToDecimal(txtSumMoney.Text));
                curBillMaster.SumQuantity =-Math.Abs( ClientUtil.TransToDecimal(txtSumQuantity.Text));

                curBillMaster.ForRedType = (EnumForRedType)cboForRedType.SelectedItem;

                curBillMaster.ProfessionCategory = txtProfessionCategory.Text;
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MatCatName = txtMaterialCategory.Text;
                }

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    StockInRedDtl curBillDtl = new StockInRedDtl();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as StockInRedDtl;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    Application.Resource.MaterialResource.Domain.Material material = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialResource = material;
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colMaterialStuff.Name].Value);
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);

                    decimal quantity = StringUtil.StrToDecimal(ClientUtil.ToString(var.Cells[colQuantity.Name].Value));
                    decimal quantityTemp = StringUtil.StrToDecimal(ClientUtil.ToString(var.Cells[colQuantityTemp.Name].Value));

                    curBillDtl.Quantity = -Math.Abs(quantity);
                    curBillDtl.QuantityTemp = quantityTemp;

                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);
                    curBillDtl.NewPrice = ClientUtil.TransToDecimal(var.Cells[colNewPrice.Name].Value);
                    curBillDtl.Money = curBillDtl.Price * curBillDtl.Quantity;
                    //前驱明细Id
                    curBillDtl.ForwardDetailId = var.Cells[colForwardDtlId.Name].Value.ToString();
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    object obj = var.Cells[this.colUsePart.Name].Tag;
                    if (obj != null)
                    {
                        curBillDtl.UsedPart = obj as GWBSTree;
                        if(curBillDtl.UsedPart !=null)
                        {
                            curBillDtl.UsedPartName = curBillDtl.UsedPart.Name;
                            curBillDtl.UsedPartSysCode = curBillDtl.UsedPart.SysCode;
                        }
                    }
                    //专业分类
                    if (var.Cells[colProfessionalCategory.Name].Value != null)
                    {
                        curBillDtl.ProfessionalCategory = var.Cells[colProfessionalCategory.Name].Value.ToString();
                    } else
                    {
                        curBillDtl.ProfessionalCategory = null;
                    }
                    var.Tag = curBillDtl;
                    curBillMaster.AddDetail(curBillDtl);
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
                txtForward.Tag = curBillMaster.ForwardBillId;
                txtForward.Text = curBillMaster.ForwardBillCode;
                this.txtCode.Text = curBillMaster.Code;
                if (curBillMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = curBillMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(curBillMaster.TheSupplierRelationInfo);
                    txtSupply.Value = curBillMaster.TheSupplierName;
                }

                cboForRedType.SelectedItem = curBillMaster.ForRedType;

                dtpDateBegin.Value = curBillMaster.CreateDate ;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName; 
                txtRemark.Text = curBillMaster.Descript;                
                
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                //txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();

                txtProject.Text = curBillMaster.ProjectName;
                txtSumQuantity.Text = Math.Abs(curBillMaster.SumQuantity).ToString();
                txtSumMoney.Text = Math.Abs(curBillMaster.SumMoney).ToString();

                //物资分类
                if (curBillMaster.MaterialCategory != null)
                {
                    txtMaterialCategory.ReadOnly = false;
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);
                    txtMaterialCategory.Value = curBillMaster.MatCatName;
                    txtMaterialCategory.ReadOnly = true;
                }
                //专业分类
                txtProfessionCategory.Text = curBillMaster.ProfessionCategory;
                                       
                this.dgDetail.Rows.Clear();
                foreach (StockInRedDtl var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colMaterialStuff.Name, i].Value = var.MaterialStuff;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;

                    this.dgDetail[colQuantity.Name, i].Value = Math.Abs(var.Quantity);
                    this.dgDetail[colQuantityTemp.Name, i].Value = Math.Abs(var.Quantity);
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colNewPrice.Name, i].Value = var.NewPrice;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    dgDetail[colProfessionalCategory.Name, i].Value =var.ProfessionalCategory;

                    dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;
                    dgDetail[colUsePart.Name, i].Value = var.UsedPartName;
                    dgDetail[colUsePart.Name, i].Tag = var.UsedPart;
                    this.dgDetail.Rows[i].Tag = var;
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
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colQuantity.Name)
            {
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
                if (quantity != null)
                {
                    string temp_quantity = quantity.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail[e.ColumnIndex, e.RowIndex].Selected = true;
                        dgDetail.BeginEdit(false);
                        return;
                    }
                } else
                {
                    return;
                }

                //根据数量计算金额                
                decimal sumqty = 0,sumMoney=0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                    if (quantity == null) quantity = 0;

                    decimal decQty = ClientUtil.TransToDecimal(quantity);

                    decimal price = ClientUtil.TransToDecimal(dgDetail.Rows[i].Cells[colPrice.Name].Value);
                    decimal tempMoney = price * decQty;
                    sumMoney += tempMoney;

                    sumqty = sumqty + decQty;
                }

                txtSumQuantity.Text = sumqty.ToString();
                txtSumMoney.Text = sumMoney.ToString("####.##");
            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"收料单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else
            {
                if (LoadTempleteFile(@"收料单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            return true;
        }

        public override bool Print()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"收料单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.Print();
            }
            else
            {
                if (LoadTempleteFile(@"收料单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.Print();
            }
            return true;
        }

        public override bool Export()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"收料单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.ExportToExcel("收料入库红单【" + curBillMaster.Code + "】", false, false, true);
            }
            else
            {
                if (LoadTempleteFile(@"收料单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.ExportToExcel("收料入库红单【" + curBillMaster.Code + "】", false, false, true);
            }
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
            } else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName+"】");
                return false;
            }
            return true;
        }

        private void FillFlex(StockInRed billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);
            ////flexGrid1.FixedRows = 5;//固定5行显示不变

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            //主表数据
            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 6).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 6).Text = billMaster.SupplyOrderCode;
            flexGrid1.Cell(4, 2).Text = billMaster.MatCatName;

            decimal sumBlueQuantity = 0;
            decimal sumRedQuantity = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockInRedDtl detail = (StockInRedDtl)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                //蓝单数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.RefQuantity.ToString();
                sumBlueQuantity += ClientUtil.ToDecimal(detail.RefQuantity);

                //冲红数量
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.Quantity.ToString();
                sumRedQuantity += ClientUtil.ToDecimal(detail.Quantity);

                //蓝单编号
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.Master.ForwardBillCode.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                //外观质量
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.MaterialGrade;
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;

                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumBlueQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1,5).Text = ClientUtil.ToString(sumRedQuantity);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumRedQuantity));
            this.flexGrid1.Cell(1, 6).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 6).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 6).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            flexGrid1.Cell(7 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(7 + detailCount, 6).Text = billMaster.RealOperationDate.ToShortDateString();
            flexGrid1.Cell(7 + detailCount, 8).Text = billMaster.CreatePersonName;

        }

        private void FillFlex1(StockInRed billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);
            ////flexGrid1.FixedRows = 5;//固定5行显示不变

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            //主表数据
            flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(3, 6).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(3, 8).Text = billMaster.Code;
            flexGrid1.Cell(4, 2).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 6).Text = billMaster.ProfessionCategory;
            flexGrid1.Cell(4, 8).Text = billMaster.ContractNo;

            decimal sumMoney = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockInRedDtl detail = (StockInRedDtl)billMaster.Details.ElementAt(i);
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = ClientUtil.ToString(i + 1);
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MaterialSpec;
                flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.DiagramNumber;
                flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.MatStandardUnitName;
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.RefQuantity);
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;

                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.Price);
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.Money);
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                sumMoney += detail.Money;
                flexGrid1.Cell(detailStartRowNumber + i, 10).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 10).WrapText = true;

                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 2).Text = ClientUtil.ToString(decimal.Round(sumMoney, 2));
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 7).Text = ClientUtil.ToString(Moneybig);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
        }
    }
}
