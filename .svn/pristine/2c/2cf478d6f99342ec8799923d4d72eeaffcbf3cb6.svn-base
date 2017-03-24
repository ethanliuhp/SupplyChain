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
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng.StockInMng.StockInUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalUI;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalRedUI
{
    public partial class VStockInBalRed : TMasterDetailView
    {
        private MStockMng model = new MStockMng();
        private StockInBalRedMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空

        /// <summary>
        /// 当前单据
        /// </summary>
        public StockInBalRedMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VStockInBalRed()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            dgDetail.ContextMenuStrip = cmsDg;
        }

        private void InitEvent()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
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
        void btnPrintPage_Click(object sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            if (e.Preview == false && e.PageNumber == 1)
            {
                model.StockInSrv.UpdateBillPrintTimes(1, curBillMaster.Id);//回写次数
                //写打印日志
                StaticMethod.InsertLogData(curBillMaster.Id, "打印", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "验收结算红单", "", curBillMaster.ProjectName);
            }
        }
        /// <summary>
        /// 根据土建、安装 界面不同显示
        /// </summary>
        private void InitControls()
        {
            colProfessionalCategory.Visible = false;
            colPrice.Visible = false;
        }

        void btnForward_Click(object sender, EventArgs e)
        {
            VStockInBalSelector vsos = new VStockInBalSelector();
            vsos.ShowDialog();
            IList list = vsos.Result;
            if (list == null || list.Count == 0) return;

            StockInBalMaster tempMaster = list[0] as StockInBalMaster;
            txtForward.Tag = tempMaster.Id;
            txtForward.Text = tempMaster.Code;
            if (tempMaster.TheSupplierRelationInfo != null)
            {
                txtSupply.Result = new ArrayList();
                txtSupply.Result.Add(tempMaster.TheSupplierRelationInfo);
                txtSupply.Text = tempMaster.TheSupplierName;
            }
            curBillMaster.ProfessionCategory = tempMaster.ProfessionCategory;
            curBillMaster.MaterialCategory = tempMaster.MaterialCategory;
            this.dtpDateBegin.Value = tempMaster.CreateDate;
            //处理旧明细
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                StockInBalRedDetail dtl = dr.Tag as StockInBalRedDetail;
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
            IList lst = new ArrayList();
            foreach (StockInBalDetail var in tempMaster.Details)
            {
                lst.Add(var.Id);
            }
            Hashtable ht = model.StockInSrv.GetDiffMonthAdjustByStockInBal(lst);
            foreach (StockInBalDetail var in tempMaster.Details)
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
                this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;
                this.dgDetail[colCanUseQty.Name, i].Value = (var.Quantity - var.RefQuantity).ToString("############.####");
                this.dgDetail[colQuantity.Name, i].Value = (var.Quantity - var.RefQuantity).ToString("############.####");
                this.dgDetail[colQuantity.Name, i].ReadOnly = ht.ContainsKey(var.Id);
                this.dgDetail[colPrice.Name, i].Value = var.Price.ToString("###########.########");
                this.dgDetail[colMoney.Name, i].Value = ((var.Quantity - var.RefQuantity) * var.Price).ToString("#################.##");
                //this.dgDetail[colProfessionalCategory.Name, i].Value = var.ProfessionalCategory;
                this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
                this.dgDetail[colCostMoney.Name, i].Value = (var.Quantity - var.RefQuantity) == 0 ? "0" : var.CostMoney.ToString("#################.##");
                //红单明细力资费计算=蓝单明细力资费*(红单数量/蓝单数量)
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
                    movedDtlList.Add(dr.Tag as StockInBalDetail);
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
                    curBillMaster = model.StockInSrv.GetStockInBalRedMasterByCode(code, StaticMethod.GetProjectInfo().Id);
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
        public void Start(string code,string Id)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.StockInSrv.GetStockInBalRedMasterById(Id);
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
            object[] os = new object[] { txtCode,txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject, txtSumMoney, txtForward };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialStuff.Name, colMaterialCode.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colCanUseQty.Name, colPrice.Name, colMoney.Name, colProfessionalCategory.Name };
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

                this.curBillMaster = new StockInBalRedMaster();
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
            if (!(curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid) )
            {
                MessageBox.Show(string.Format("此单状态为【{0}】，不能修改！",ClientUtil.GetDocStateName(curBillMaster.DocState)));
                return false;
            }
            movedDtlList = new ArrayList();
            base.ModifyView();
            curBillMaster = model.StockInSrv.GetStockInBalRedMasterById(curBillMaster.Id);
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
                    curBillMaster = model.StockInSrv.SaveStockInBalRedMaster(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "验收结算单红单", "", curBillMaster.ProjectName);
                }
                else
                {
                    curBillMaster = model.StockInSrv.SaveStockInBalRedMaster(curBillMaster, movedDtlList);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "验收结算单红单", "", curBillMaster.ProjectName);
                }
                movedDtlList = new ArrayList();

                //插入日志
                //StaticMethod.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, curBillMaster.CreatePerson.Name, "收料单","");
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;

                return true;
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
                if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    if (!ViewToModel()) return false;
                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster = model.StockInSrv.SaveStockInBalRedMaster(curBillMaster, movedDtlList);
                    txtCode.Text = curBillMaster.Code;
                    //更新Caption
                    this.ViewCaption = ViewName + "-" + txtCode.Text;

                    return true;
                }
            } catch (Exception ex)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(ex));
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
                if (!(curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid))
                {
                    MessageBox.Show(string.Format("此单状态为【{0}】，不能删除！", ClientUtil.GetDocStateName(curBillMaster.DocState)));
                    return false;
                }
                model.StockInSrv.DeleteStockInBalRedMaster(curBillMaster);
                //插入日志
                StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "验收结算单红单", "", curBillMaster.ProjectName);
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
                        curBillMaster = model.StockInSrv.GetStockInBalRedMasterById(curBillMaster.Id);
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
                curBillMaster = model.StockInSrv.GetStockInBalRedMasterById(curBillMaster.Id);
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
                MessageBox.Show("前续单据不能为空！");
                return false;
            }

            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

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

                object objQty = dr.Cells[colQuantity.Name].Value;
                if (objQty == null || objQty.ToString() == "" || ClientUtil.TransToDecimal(objQty) <= 0 || !CommonMethod.VeryValid(objQty.ToString()))
                {
                    MessageBox.Show("冲红数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                StockInBalDetail forwardDetail = model.StockInSrv.GetStockInBalDetail(forwardDtlId.ToString());
                if (forwardDetail == null)
                {
                    MessageBox.Show("未找到前续单据明细,请重新引用。");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                } else
                {
                    decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;
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

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.ForwardBillId = txtForward.Tag as string;
                curBillMaster.ForwardBillCode = txtForward.Text;
                
                if (this.txtSupply.Result.Count > 0)
                {
                    curBillMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    curBillMaster.TheSupplierName = txtSupply.Text;
                }
                curBillMaster.CreateDate  = dtpDateBegin.Value.Date;

                DataTable oTable = model.StockInSrv.GetFiscaDate(curBillMaster.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    curBillMaster.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    curBillMaster.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }

                curBillMaster.Descript = this.txtRemark.Text;
    
                curBillMaster.SumMoney =-ClientUtil.TransToDecimal(txtSumMoney.Text);
                curBillMaster.SumQuantity =-ClientUtil.TransToDecimal(txtSumQuantity.Text);

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    StockInBalRedDetail curBillDtl = new StockInBalRedDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as StockInBalRedDetail;
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

                    curBillDtl.Quantity = -quantity;
                    curBillDtl.QuantityTemp = quantityTemp;
                    //采购单价、金额
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);
                    curBillDtl.Money =-ClientUtil.TransToDecimal(var.Cells[colMoney.Name].Value);

                    //前驱明细Id
                    curBillDtl.ForwardDetailId = var.Cells[colForwardDtlId.Name].Value.ToString();
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
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
                                       
                this.dgDetail.Rows.Clear();
                IList lst = new ArrayList();
                foreach (StockInBalRedDetail var in curBillMaster.Details)
                {
                    lst.Add(var.ForwardDetailId);
                }
                Hashtable ht = model.StockInSrv.GetDiffMonthAdjustByStockInBal(lst);
                foreach (StockInBalRedDetail var in curBillMaster.Details)
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
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    dgDetail[colProfessionalCategory.Name, i].Value =var.ProfessionalCategory;
                    dgDetail[colCostMoney.Name, i].Value = var.CostMoney;
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
                } else
                {
                    return;
                }
                object objPrice = 0;
                #region 计算力资费
                //红单明细力资费计算=蓝单明细力资费*(红单数量/蓝单数量)
                decimal dCostMoney = 0;
                StockInBalRedDetail oStockInBalRedDetail =this.dgDetail.Rows[e.RowIndex].Tag ==null?null: this.dgDetail.Rows[e.RowIndex].Tag as StockInBalRedDetail;
                decimal dQty = ClientUtil.ToDecimal(quantity);
                decimal dQtyTemp = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[this.colQuantityTemp.Name].Value);
                string sForwardDtlId = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[this.colForwardDtlId.Name].Value);
                if (!string.IsNullOrEmpty(sForwardDtlId))
                {
                   StockInBalDetail oStockInBalDetail= model.StockInSrv.GetStockInBalDetail(sForwardDtlId);
                   #region 回写验收结算单计算方法
                   //if (oStockInBalDetail.Quantity - oStockInBalDetail.RefQuantity != 0)
                   //{
                   //    if (oStockInBalDetail.Quantity - oStockInBalDetail.RefQuantity - dQty == 0)
                   //    {
                   //        dCostMoney = oStockInBalDetail.CostMoney + (oStockInBalRedDetail == null ? 0 : oStockInBalRedDetail.CostMoney);
                   //    }
                   //    else
                   //    {
                   //        dCostMoney = oStockInBalDetail.CostMoney * dQty / (oStockInBalDetail.Quantity - oStockInBalDetail.RefQuantity);
                   //    }
                   //    dgDetail[colCostMoney.Name, e.RowIndex].Value = dCostMoney.ToString("##########.##");
                   //}
                   #endregion
                    #region 不回写
                    dCostMoney=oStockInBalDetail.Quantity ==0?0:oStockInBalDetail.CostMoney * dQty / oStockInBalDetail.Quantity ;
                    dgDetail[colCostMoney.Name, e.RowIndex].Value = dCostMoney.ToString("##########.##");
                    #endregion
                }
                #endregion
                //根据数量、单价计算金额                
                decimal sumqty = 0,sumMoney=0;

                for (int i = 0; i <= dgDetail.RowCount-1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                    if (quantity == null) quantity = 0;

                    decimal decQty=ClientUtil.TransToDecimal(quantity);

                    objPrice = dgDetail.Rows[i].Cells[colPrice.Name].Value;
                    if (objPrice == null) objPrice = 0;
                    decimal decPrice=decimal.Parse(objPrice.ToString());
                    
                    if (i == e.RowIndex)
                    {
                        decimal tempMoney = decPrice * decQty;
                        dgDetail.Rows[i].Cells[colMoney.Name].Value = tempMoney.ToString("##########.##");
                        sumMoney += tempMoney;

                    } else
                    {
                        object money = dgDetail.Rows[i].Cells[colMoney.Name].Value;
                        if (money == null) money = 0;
                        sumMoney += ClientUtil.TransToDecimal(money);                        
                    }

                    sumqty = sumqty + decQty;                    
                }

                txtSumQuantity.Text = sumqty.ToString("############.####");
                txtSumMoney.Text = sumMoney.ToString("#########.##");
            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (curBillMaster.ProfessionCategory == null)
            {
                if (LoadTempleteFile(@"验收单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else
            {
                if (LoadTempleteFile(@"验收单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            return true;
        }

        public override bool Print()
        {
            if (curBillMaster.ProfessionCategory == null)
            {
                if (LoadTempleteFile(@"验收单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.Print();
            }
            else
            {
                if (LoadTempleteFile(@"验收单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.Print();
            }
            return true;
        }

        public override bool Export()
        {
            if (curBillMaster.ProfessionCategory == null)
            {
                if (LoadTempleteFile(@"验收单（红单）.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.ExportToExcel("验收单红单【" + curBillMaster.Code + "】", false, false, true);
            }
            else
            {
                if (LoadTempleteFile(@"验收单（红单）_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.ExportToExcel("验收单红单【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(StockInBalRedMaster billMaster)
        {
            int detailStartRowNumber = 7;//7为模板中的行号
            int detailCount = billMaster.Details.Count;
            //flexGrid1.FixedRows = 6;
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
            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 7).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 2).Text = billMaster.ForwardBillCode;
            if (billMaster.MaterialCategory != null)
            {
                flexGrid1.Cell(4, 7).Text = billMaster.MaterialCategory.Name;//分类编码
            }

            decimal sumBlueQuantity = 0;
            decimal sumRedQuantity = 0;
            decimal sumMoney = 0;
            decimal sumHmoney = 0;
            decimal sumCostMoney = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockInBalRedDetail detail = (StockInBalRedDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                //蓝单数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
                sumBlueQuantity += ClientUtil.ToDecimal(detail.Quantity);
                //红单数量
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.Quantity);
                sumRedQuantity += ClientUtil.ToDecimal(detail.Quantity);
                //红单金额
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Money);
                sumHmoney += ClientUtil.ToDecimal(detail.Money);
                //单价
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.StockInPrice);
                //金额
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.StockInMoney);
                sumMoney += ClientUtil.ToDecimal(detail.StockInMoney);
                //蓝单编号
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = detail.Master.ForwardBillCode;
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                //力资运费
                flexGrid1.Cell(detailStartRowNumber + i, 10).Text = ClientUtil.ToString(detail.CostMoney);
                sumCostMoney += ClientUtil.ToDecimal(detail.CostMoney);
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 11).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 11).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 1).Text = "合计：";
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumBlueQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumRedQuantity);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 6).Text = ClientUtil.ToString(sumHmoney);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 8).Text = ClientUtil.ToString(sumMoney);
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 10).Text = ClientUtil.ToString(sumCostMoney);
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumHmoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 2, 2).Text = ClientUtil.ToString(Moneybig);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumRedQuantity));
            this.flexGrid1.Cell(1, 8).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 8).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 8).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            flexGrid1.Cell(9 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(9 + detailCount, 7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(9 + detailCount, 10).Text = billMaster.CreatePersonName;

            this.flexGrid1.Cell(2, 8).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(billMaster.PrintTimes + 1);
            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.RightFooter = " 打印时间:[" + CommonMethod.GetServerDateTime() + " ],打印人:[" + ConstObject.LoginPersonInfo.Name + "]   " + "第&P页/共&N页  ";

           
        }
        private void FillFlex1(StockInBalRedMaster billMaster)
        {
            int detailStartRowNumber = 7;//7为模板中的行号
            int detailCount = billMaster.Details.Count;
            //flexGrid1.FixedRows = 6;
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
            flexGrid1.Cell(4,7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(3, 8).Text = billMaster.Code;
            flexGrid1.Cell(4, 2).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 10).Text = billMaster.ProfessionCategory;//分类编码

            decimal sumStockInMoney = 0;
            decimal sumConfirmMoney = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockInBalRedDetail detail = (StockInBalRedDetail)billMaster.Details.ElementAt(i);
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
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.StockInPrice);
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = ClientUtil.ToString(detail.StockInMoney);
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                sumStockInMoney += detail.StockInMoney;
                flexGrid1.Cell(detailStartRowNumber + i, 10).Text = ClientUtil.ToString(detail.ConfirmPrice);
                flexGrid1.Cell(detailStartRowNumber + i, 10).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 11).Text = ClientUtil.ToString(detail.ConfirmMoney);
                flexGrid1.Cell(detailStartRowNumber + i, 11).WrapText = true;
                sumConfirmMoney += detail.ConfirmMoney;
                flexGrid1.Cell(detailStartRowNumber + i, 12).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 12).WrapText = true;
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 3).Text = ClientUtil.ToString(sumStockInMoney);
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumStockInMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 9).Text = ClientUtil.ToString(Moneybig);
                    flexGrid1.Cell(detailStartRowNumber + i + 2, 3).Text = ClientUtil.ToString(sumConfirmMoney);
                    string Moneybig1 = CurrencyComUtil.GetMoneyChinese(sumConfirmMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 2, 9).Text = ClientUtil.ToString(Moneybig1);
                }
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumStockInMoney));
            this.flexGrid1.Cell(1, 9).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 9).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 9).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            this.flexGrid1.Cell(2, 9).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(billMaster.PrintTimes + 1);
            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.RightFooter = " 打印时间:[" + CommonMethod.GetServerDateTime() + " ],打印人:[" + ConstObject.LoginPersonInfo.Name + "]   " + "第&P页/共&N页  ";

        }
    }
}
