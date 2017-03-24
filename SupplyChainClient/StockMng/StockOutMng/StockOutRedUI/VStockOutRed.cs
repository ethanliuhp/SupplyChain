﻿using System;
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
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI
{
    public partial class VStockOutRed : TMasterDetailView
    {
        private MStockMng model = new MStockMng();
        private StockOutRed curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        private EnumStockExecType execType;
        public EnumStockExecType ExecType
        {
            get { return execType; }
            set
            {
                execType = value;
                bool bFlag = (ExecType == EnumStockExecType.安装);
                colConfirmPrice.Visible = bFlag;
                colConfirmMoney.Visible = bFlag;
            }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public StockOutRed CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VStockOutRed()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3;
            //专业分类
            VBasicDataOptr.InitProfessionCategory(colProfessionalCategory, true);
            dgDetail.ContextMenuStrip = cmsDg;
        }

        private void InitEvent()
        {
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            flexGrid1.PrintPage += new FlexCell.Grid.PrintPageEventHandler(btnPrintPage_Click);
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
            if (execType == EnumStockExecType.土建)
            {
                colProfessionalCategory.Visible = false;
                lblCat.Text = "物资分类:";
            }
            else if (execType == EnumStockExecType.安装)
            {
                lblCat.Text = "专业分类:";
            }
        }


        void btnForward_Click(object sender, EventArgs e)
        {
            VStockOutSelector vsos = new VStockOutSelector(execType);
            vsos.ShowDialog();
            IList list = vsos.Result;
            if (list == null || list.Count == 0) return;

            StockOut tempMaster = list[0] as StockOut;
            txtForward.Tag = tempMaster.Id;
            txtForward.Text = tempMaster.Code;
            if (tempMaster.TheSupplierRelationInfo != null)
            {
                txtSupply.Result = new ArrayList();
                txtSupply.Result.Add(tempMaster.TheSupplierRelationInfo);
                txtSupply.Text = tempMaster.TheSupplierName;
            }
            this.dtpDateBegin.Value = tempMaster.CreateDate;
            if (ExecType == EnumStockExecType.安装)
            {
                txtCat.Text = tempMaster.ProfessionCategory;
            }
            else if (ExecType == EnumStockExecType.土建)
            {
                txtCat.Text = tempMaster.MatCatName;
                txtCat.Tag = tempMaster.MaterialCategory;
            }

            //处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                StockOutRedDtl dtl = dr.Tag as StockOutRedDtl;
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
            foreach (StockOutDtl var in tempMaster.Details)
            {
                if (var.IsSelect == false) continue;
                int i = this.dgDetail.Rows.Add();
                DataGridViewRow dr = dgDetail.Rows[i];

                StockOutRedDtl detail = new StockOutRedDtl();
                detail.UsedPart = var.UsedPart;
                detail.UsedPartName = var.UsedPartName;
                detail.UsedPartSysCode = var.UsedPartSysCode;

                this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colMaterialStuff.Name, i].Value = var.MaterialStuff;
                this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                this.dgDetail[colCanUseQty.Name, i].Value = var.Quantity - var.RefQuantity;
                this.dgDetail[colQuantity.Name, i].Value = var.Quantity - var.RefQuantity;
                //this.dgDetail["NoRefQuantity", i].Value = var.Quantity;
                decimal money = CalMoneyByQuantity(var.Id, var.Quantity - var.RefQuantity);
                decimal price = money / (var.Quantity - var.RefQuantity);
                if (price == 0)
                {
                    this.dgDetail[colPrice.Name, i].Value = var.Price.ToString("############.########");
                }
                else
                {
                    this.dgDetail[colPrice.Name, i].Value = price.ToString("############.########");
                }
                if (money == 0)
                {
                    this.dgDetail[colMoney.Name, i].Value = var.Money.ToString("################.##");
                }
                else
                {
                    this.dgDetail[colMoney.Name, i].Value = money.ToString("################.##");
                }
                this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;
                this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                this.dgDetail[colProfessionalCategory.Name, i].Value = var.ProfessionalCategory;
                this.dgDetail[colDescript.Name, i].Value = var.Descript;
                this.dgDetail[colDiagramNum.Name, i].Value = var.DiagramNumber;
                detail.SubjectGUID = var.SubjectGUID;
                detail.SubjectName = var.SubjectName;
                detail.SubjectSysCode = var.SubjectSysCode;
                //this.dgDetail["StockOutDtl", i].Tag = var;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                dr.Tag = detail;
            }
        }

        private decimal CalMoneyByQuantity(string stockOutDtlId, decimal quantity)
        {
            decimal money = 0M;
            try
            {
                IList list = model.StockOutSrv.GetStockOutDtlSeqByStockOutDtlId(stockOutDtlId);
                decimal tempQuantity = quantity;
                foreach (StockOutDtlSeq dltSeq in list)
                {
                    if (dltSeq.RemainQuantity >= tempQuantity)
                    {
                        money += tempQuantity * dltSeq.Price;
                       
                        break;
                    }
                    else
                    {
                        tempQuantity = tempQuantity - dltSeq.RemainQuantity;
                        money += dltSeq.RemainQuantity * dltSeq.Price;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出库时序出错。\n"+ex.Message);
            }
            return money;
        }
        private decimal CalConfirmMoneyByQuantity(string stockOutDtlId, decimal dQuantity)
        {
            decimal money = 0M;
            try
            {
                //IList list = model.StockOutSrv.GetStockOutDtlSeqByStockOutDtlId(stockOutDtlId);
                //decimal tempQuantity = quantity;
                //foreach (StockOutDtlSeq dltSeq in list)
                //{
                //    if (dltSeq.RemainQuantity >= tempQuantity)
                //    {
                //        money += tempQuantity * dltSeq.Price;

                //        break;
                //    }
                //    else
                //    {
                //        tempQuantity = tempQuantity - dltSeq.RemainQuantity;
                //        money += dltSeq.RemainQuantity * dltSeq.Price;
                //    }
                //}    
                DataSet oDataSet = model.StockOutSrv.QueryStockInQuantityAndOutPrice(stockOutDtlId);
                decimal dTempQuerty = dQuantity;
                decimal TempQuery = 0M;
                decimal TempPrice = 0M;
                for (int i = 0; i < oDataSet.Tables[0].Rows.Count; i++)
                {
                    TempPrice =ClientUtil .TransToDecimal ( oDataSet.Tables[0].Rows[i]["confirmprice"] ); 
                    TempQuery =ClientUtil .TransToDecimal ( oDataSet.Tables[0].Rows[i]["quantity"] );
                    if (TempQuery >= dTempQuerty)
                    {
                        money += dTempQuerty * TempPrice;
                        break;
                    }
                    else
                    {
                        dTempQuerty -= TempQuery;
                        money += TempQuery * TempPrice;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出库时序出错。\n" + ex.Message);
            }
            return money;
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
                    movedDtlList.Add(dr.Tag as StockOutRedDtl);
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
                    curBillMaster = model.StockOutSrv.GetStockOutRedByCode(code, Enum.GetName(typeof(EnumStockExecType), ExecType), StaticMethod.GetProjectInfo().Id);
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
        public void Start(string code,string id)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.StockOutSrv.GetStockOutRedById(id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtSumMoney ,txtCat,txtForward,txtSupply};
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialStuff.Name, colMaterialCode.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colCanUseQty.Name, colPrice.Name, colMoney.Name, colUsedPart.Name, colProfessionalCategory.Name ,colConfirmMoney .Name ,colConfirmPrice .Name };
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

                this.curBillMaster = new StockOutRed();
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

                curBillMaster.StockOutManner = EnumStockInOutManner.领料出库;
                curBillMaster.Special = Enum.GetName(typeof(EnumStockExecType), ExecType);

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
            curBillMaster = model.StockOutSrv.GetStockOutRedById(curBillMaster.Id);
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
                    curBillMaster = model.StockOutSrv.SaveStockOutRed(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库单", "", curBillMaster.ProjectName);
                }
                else
                {
                    curBillMaster = model.StockOutSrv.SaveStockOutRed(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库单", "", curBillMaster.ProjectName);
                }
                movedDtlList = new ArrayList();

                //插入日志
                //StaticMethod.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, curBillMaster.CreatePerson.Name, "收料单","");
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                try
                {
                    Tally();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("记账出错。" + ex.Message);
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
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
                hashBillId.Add("StockOutRed", billIdList);

                Hashtable hashBillCode = new Hashtable();
                hashBillCode.Add("StockOutRed", billCodeList);

                Hashtable tallyResult = model.TallyStockOut(hashBillId, hashBillCode);
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
                //curBillMaster = model.StockOutSrv.GetStockOutRedById(curBillMaster.Id);
                model.StockOutSrv.DeleteStockOutRed(curBillMaster);
                //插入日志
                StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库单", "", curBillMaster.ProjectName);
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
                        curBillMaster = model.StockOutSrv.GetStockOutRedById(curBillMaster.Id);
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
                curBillMaster = model.StockOutSrv.GetStockOutRedById(curBillMaster.Id);
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
                MessageBox.Show("前驱领料出库单不能为空！");
                return false;
            }

            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
           // MStockMng model = new MStockMng();
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
                if (objQty == null || objQty.ToString() == "" || ClientUtil.TransToDecimal(objQty) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                StockOutDtl forwardDetail = model.StockOutSrv.GetStockOutDtlById(forwardDtlId.ToString());
                if (forwardDetail == null)
                {
                    MessageBox.Show("未找到前续出库单明细,请重新引用。");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }
                else
                {
                    decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;
                    decimal currentQty = decimal.Parse(dr.Cells[colQuantity.Name].Value.ToString());
                    object qtyTempObj = dr.Cells["ColQuantityTemp"].Value;
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
                if (ExecType == EnumStockExecType.土建)
                {
                    sMsg = model.StockInSrv.CheckBusinessDate(ConstObject.TheLogin.TheAccountOrgInfo, ClientUtil.ToDateTime(this.dtpDateBegin.Text), materialList, StaticMethod.GetProjectInfo().Id);
                    if (sMsg != string.Empty)
                    {
                        MessageBox.Show(sMsg);
                        return false;
                    }
                }
                object money = dr.Cells[colMoney.Name].Value;
                //2012-11-07 把此判断去掉 ouyxm
                //if (ClientUtil.ToDecimal(money) == 0)
                //{
                //    MessageBox.Show("金额不能为零！");
                //    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                //    return false;
                //}

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
                curBillMaster.ForwardBillType = "StockOut";

                if (this.txtSupply.Result.Count > 0)
                {
                    curBillMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    curBillMaster.TheSupplierName = txtSupply.Text;
                }
                curBillMaster.CreateDate  = ClientUtil.ToDateTime(dtpDateBegin.Value.Date.ToShortDateString());

                DataTable oTable = model.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }

                curBillMaster.Descript = this.txtRemark.Text;

                curBillMaster.SumMoney = -Math.Abs(ClientUtil.TransToDecimal(txtSumMoney.Text));
                curBillMaster.SumQuantity = -Math.Abs(ClientUtil.TransToDecimal(txtSumQuantity.Text));

                if (ExecType == EnumStockExecType.安装)
                {
                    curBillMaster.ProfessionCategory = txtCat.Text;
                }
                else if (ExecType == EnumStockExecType.土建)
                {
                    curBillMaster.MatCatName = txtCat.Text;
                    curBillMaster.MaterialCategory = txtCat.Tag as MaterialCategory;
                }

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    StockOutRedDtl curBillDtl = new StockOutRedDtl();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as StockOutRedDtl;
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
                    curBillDtl.Money = -Math.Abs(ClientUtil.TransToDecimal(var.Cells[colMoney.Name].Value));
                    if (execType == EnumStockExecType.安装)
                    {
                        curBillDtl.ConfirmPrice = ClientUtil.TransToDecimal(var.Cells[colConfirmPrice.Name].Value);
                        curBillDtl.ConfirmMoney = -Math.Abs(ClientUtil.TransToDecimal(var.Cells[colConfirmMoney.Name].Value));
                    }
                    //前驱明细Id
                    curBillDtl.ForwardDetailId = var.Cells[colForwardDtlId.Name].Value.ToString();
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                   
                    //专业分类
                    if (var.Cells[colProfessionalCategory.Name].Value != null)
                    {
                        curBillDtl.ProfessionalCategory = var.Cells[colProfessionalCategory.Name].Value.ToString();
                    }
                    else
                    {
                        curBillDtl.ProfessionalCategory = null;
                    }

                    //使用部位
                    if (var.Cells[colUsedPart.Name].Tag != null)
                    {
                        curBillDtl.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                    }
                    else
                    {
                        curBillDtl.UsedPart = null;
                    }

                    if (var.Cells[colUsedPart.Name].Value != null)
                    {
                        curBillDtl.UsedPartName = var.Cells[colUsedPart.Name].Value.ToString();
                    }
                    else
                    {
                        curBillDtl.UsedPartName = null;
                    }

                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
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
                dtpDateBegin.Value = curBillMaster.CreateDate ;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtRemark.Text = curBillMaster.Descript;

                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
               // txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();

                txtProject.Text = curBillMaster.ProjectName;
                txtSumQuantity.Text = Math.Abs(curBillMaster.SumQuantity).ToString();
                txtSumMoney.Text = Math.Abs(curBillMaster.SumMoney).ToString();

                if (ExecType == EnumStockExecType.安装)
                {
                    txtCat.Text = curBillMaster.ProfessionCategory;
                }
                else if (ExecType == EnumStockExecType.土建)
                {
                    txtCat.Text = curBillMaster.MatCatName;
                    txtCat.Tag = curBillMaster.MaterialCategory;
                }

                this.dgDetail.Rows.Clear();
                foreach (StockOutRedDtl var in curBillMaster.Details)
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
                    dgDetail[colMoney.Name, i].Value = Math.Abs(var.Money);
                    if (execType == EnumStockExecType.安装)
                    {
                        this.dgDetail[colConfirmPrice.Name, i].Value = var.ConfirmPrice;
                        dgDetail[colConfirmMoney.Name, i].Value = Math.Abs(var.ConfirmMoney);
                    }

                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    dgDetail[colProfessionalCategory.Name, i].Value = var.ProfessionalCategory;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    //使用部位
                    dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;

                    dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;

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
                }
                else
                {
                    return;
                }

                //根据数量计算单价和金额                
                decimal sumqty = 0, sumMoney = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                    if (quantity == null) quantity = 0;

                    decimal decQty = ClientUtil.TransToDecimal(quantity);

                    if (i == e.RowIndex)
                    {
                        string stockOutDtlId=dgDetail.Rows[i].Cells[colForwardDtlId.Name].Value.ToString();
                        decimal tempMoney = CalMoneyByQuantity(stockOutDtlId, decQty);
                       
                        dgDetail.Rows[i].Cells[colMoney.Name].Value = tempMoney.ToString("##########.##");
                        decimal price = tempMoney / decQty;
                        dgDetail.Rows[i].Cells[colPrice.Name].Value = price.ToString("##########.########");
                        sumMoney += tempMoney;
                        if (execType == EnumStockExecType.安装)
                        {
                            decimal ConfirmMoney = CalConfirmMoneyByQuantity(stockOutDtlId, decQty);
                            dgDetail.Rows[i].Cells[colConfirmMoney.Name].Value = ConfirmMoney.ToString("##########.##");
                            price = ConfirmMoney / decQty;
                            dgDetail.Rows[i].Cells[colConfirmPrice.Name].Value = price.ToString("##########.########");
                        }
                    }
                    else
                    {
                        object money = dgDetail.Rows[i].Cells[colMoney.Name].Value;
                        if (money == null) money = 0;
                        sumMoney += ClientUtil.TransToDecimal(money);
                    }

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
                if (LoadTempleteFile(@"领料单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else
            {
                if (LoadTempleteFile(@"领料单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            return true;
        }

        public override bool Print()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"领料单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.Print();
                 }
            else
            {
                if (LoadTempleteFile(@"领料单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.Print();
            }
            return true;
        }
        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                model.StockInSrv.UpdateBillPrintTimes(3, curBillMaster.Id);//回写次数
                //写打印日志
                StaticMethod.InsertLogData(curBillMaster.Id, "打印", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库红单", "", curBillMaster.ProjectName);
            }
        }
        public override bool Export()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"领料单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.ExportToExcel("领料单红单【" + curBillMaster.Code + "】", false, false, true);
            }
            else
            {
                if (LoadTempleteFile(@"领料单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.ExportToExcel("领料单红单【" + curBillMaster.Code + "】", false, false, true);
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
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(StockOutRed billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            //主表数据
             string strLimit = "";
            if (billMaster.IsLimited.Equals("0"))
            {
                strLimit = "是";
            }
            else 
            {
                strLimit = "否";
            }
            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 6).Text = billMaster.MatCatName;
            flexGrid1.Cell(4, 5).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 2).Text = strLimit;

            decimal sumBlueQuantity = 0;
            decimal sumRedQuantity = 0;

            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockOutRedDtl detail = (StockOutRedDtl)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                //蓝单数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();
                sumBlueQuantity += ClientUtil.ToDecimal(detail.Quantity.ToString());
                //冲红数量
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.Quantity.ToString();
                sumRedQuantity += ClientUtil.ToDecimal(detail.Quantity.ToString());
                //使用部位
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.UsedPartName;
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                //蓝单编号
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Master.ForwardBillCode;
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 1).Text = "合计：";
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumBlueQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumRedQuantity);
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
        private void FillFlex1(StockOutRed billMaster)
        {
            int detailStartRowNumber = 6;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            //主表数据
            flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(3, 7).Text = billMaster.Code;
            flexGrid1.Cell(4, 5).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 9).Text = billMaster.ProfessionCategory;
            flexGrid1.Cell(4, 2).Text = billMaster.TheSupplierName;
            decimal sumMoney = 0;

            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockOutRedDtl detail = (StockOutRedDtl)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = ClientUtil.ToString(i + 1);
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MaterialSpec;
                flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.DiagramNumber;
                flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.MatStandardUnitName;
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.RefQuantity.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.Quantity.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Master.ForwardBillCode;
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.Price.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = detail.Money.ToString();
                sumMoney += detail.Money;
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;

                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 2).Text = ClientUtil.ToString(sumMoney);
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 7).Text = ClientUtil.ToString(Moneybig);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
        }
    }
}
