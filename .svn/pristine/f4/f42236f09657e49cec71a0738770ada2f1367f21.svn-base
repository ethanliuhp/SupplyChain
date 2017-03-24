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
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI
{
    public partial class VStockOut : TMasterDetailView
    {
        private MStockMng model = new MStockMng();
        private StockOut curBillMaster;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        private EnumStockExecType execType;
        IList listSetUpSubject = null;
        IList lstMaterial = null;//根据库存获取的物资列表
        bool IsSelectRelation = true;
        public EnumStockExecType ExecType
        {
            get { return execType; }
            set { 
                execType = value;
                bool bFlag = (ExecType == EnumStockExecType.安装);
                colConfirmPrice.Visible = bFlag;
                colConfirmMoney.Visible = bFlag;
                if (this.execType == EnumStockExecType.安装)
                {
                    colSubject.Visible = false;
                    colSetUpSubject.Visible = true;
                    listSetUpSubject = model.StockOutSrv.GetSetUpCostAccountSubject();
                }
                else
                {
                    colSubject.Visible = true;
                    colSetUpSubject.Visible = false;
                }
            }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public StockOut CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VStockOut()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            this.txtMaterialCategory.rootCatCode = "01";
            this.txtMaterialCategory.rootLevel = "3";
            //专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
            dgDetail.ContextMenuStrip = cmsDg;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3;
            projectInfo = StaticMethod.GetProjectInfo();
            
          
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            this.btnSetSubject.Click += new EventHandler(btnSetSubject_Click);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);
            flexGrid1.PrintPage += new FlexCell.Grid.PrintPageEventHandler(btnPrintPage_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }
        void btnSetSubject_Click(object sender, EventArgs e)
        {
            string sValue = string.Empty;
            string setValue = string.Empty;
            if (dgDetail.Rows.Count > 0 && !this.dgDetail.Rows[0].IsNewRow)
            {
                sValue = (this.dgDetail.Rows[0].Cells[colSubject.Name].Value == null ? string.Empty : this.dgDetail.Rows[0].Cells[colSubject.Name].Value.ToString());
                setValue = (this.dgDetail.Rows[0].Cells[colSetUpSubject.Name].Value == null ? string.Empty : this.dgDetail.Rows[0].Cells[colSetUpSubject.Name].Value.ToString());
                object oTag = this.dgDetail.Rows[0].Cells[colSubject.Name].Tag;
                object osTag = this.dgDetail.Rows[0].Cells[colSetUpSubject.Name].Tag;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    var.Cells[colSubject.Name].Value = sValue;
                    var.Cells[colSubject.Name].Tag = oTag;
                    var.Cells[colSetUpSubject.Name].Value = setValue;
                    var.Cells[colSetUpSubject.Name].Tag = osTag;
                }
            }
        }
        void cboProfessionCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            object pc = cboProfessionCat.SelectedItem;
            if (pc != null)
            {
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    if (dr.IsNewRow) continue;
                    dr.Cells[colProfessionalCategory.Name].Value = pc;
                    Material material = dr.Cells[colMaterialCode.Name].Tag as Material;
                    //查询库存数量
                    try
                    {
                        dr.Cells[colCanUseQty.Name].Value = 0;                        
                        if (material != null)
                        {
                            IList stockRelList = null;
                            if (ExecType == EnumStockExecType.安装)
                            {
                                stockRelList = GetStockRelation(material.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), ClientUtil .ToString (dr.Cells[colDiagramNum.Name].Value ));
                            }
                            else
                            {
                                   stockRelList = GetStockRelation(material.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), "");
                            }
                            dr.Cells[colCanUseQty.Name].Value = GetStockQuantity(stockRelList);
                            dr.Cells[colCanUseQty.Name].Tag = stockRelList;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("查询物资【{0} {1}】库存数量出错。", material.Name, material.Specification));
                    }
                }
            }
        }

        private void ReadOnlyCat(bool enabled)
        {
            txtMaterialCategory.ReadOnly = enabled;
            cboProfessionCat.ReadOnly = enabled;
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
                //colPrice.Visible = false;
                //colMoney.Visible = false;

                lblCat.Text = "物资分类:";
                txtMaterialCategory.Visible = true;
                cboProfessionCat.Visible = false;
            }
            else if (execType == EnumStockExecType.安装)
            {
                lblCat.Text = "专业分类:";
                txtMaterialCategory.Visible = false;
                cboProfessionCat.Visible = true;
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
                }
            }
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            curBillMaster.Details.Remove(e.Row.Tag as BaseDetail);
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
                    curBillMaster = model.StockOutSrv.GetStockOutByCode(code, Enum.GetName(typeof(EnumStockExecType), execType), StaticMethod.GetProjectInfo().Id);
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
                    curBillMaster = model.StockOutSrv.GetStockOutById(id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate,txtHandlePerson,txtSumQuantity,txtProject,txtSumMoney};
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialStuff.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colMoney.Name, colCanUseQty.Name, colPrice.Name, colMoney.Name, colProfessionalCategory.Name, colUsedPart.Name, colSubject.Name,colConfirmMoney .Name ,colConfirmPrice .Name  };
            dgDetail.SetColumnsReadOnly(lockCols);

            if (curBillMaster != null && curBillMaster.Code != null)
            {
                ReadOnlyCat(true);
            }
            else
            {
                ReadOnlyCat(false);
            }
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
                ReadOnlyCat(false);

                this.curBillMaster = new StockOut();
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
                curBillMaster.Special = Enum.GetName(typeof(EnumStockExecType), execType);

                //仓库
                curBillMaster.TheStationCategory = StaticMethod.GetStationCategory();

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
               // txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
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
            base.ModifyView();
            curBillMaster = model.StockOutSrv.GetStockOutById(curBillMaster.Id);
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
                    curBillMaster = model.StockOutSrv.SaveStockOut(curBillMaster);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库单", "", curBillMaster.ProjectName);
                }
                else
                {
                    curBillMaster = model.StockOutSrv.SaveStockOut(curBillMaster);
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库单", "", curBillMaster.ProjectName);
                }

                //插入日志
                //StaticMethod.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, curBillMaster.CreatePerson.Name, "收料单","");
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;

                try
                {
                    Tally();
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
                hashBillId.Add("StockOut", billIdList);

                Hashtable hashBillCode = new Hashtable();
                hashBillCode.Add("StockOut", billCodeList);

                Hashtable tallyResult = model.TallyStockOut(hashBillId, hashBillCode);
                if (tallyResult != null)
                {
                    string errMsg = (string)tallyResult["err"];
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        MessageBox.Show(errMsg);
                    } else
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
                curBillMaster = model.StockOutSrv.GetStockOutById(curBillMaster.Id);
                if(curBillMaster.IsTally==0)
                {
                    if (!model.StockOutSrv.DeleteByDao(curBillMaster)) return false;
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库单", "", curBillMaster.ProjectName);
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单己记账，不能删除！");
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
                        curBillMaster = model.StockOutSrv.GetStockOutById(curBillMaster.Id);
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
                curBillMaster = model.StockOutSrv.GetStockOutById(curBillMaster.Id);
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
            string validMessage ="";

            if (ExecType == EnumStockExecType.安装)
            {
                if (cboProfessionCat.SelectedItem == null)
                {
                    MessageBox.Show("请选择专业分类");
                    return false;
                }
            }
            else if (ExecType == EnumStockExecType.土建)
            {
                if (txtMaterialCategory.Text == "" || txtMaterialCategory.Result == null || txtMaterialCategory.Result.Count == 0)
                {
                    MessageBox.Show("请选择物资分类。");
                    return false;
                }
            }

            if (this.dgDetail.Rows.Count - 1 == 0)
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

            //if (txtContractNo.Text == "")
            //{
            //    validMessage += "采购合同号不能为空！\n";
            //}
            if (txtSupply.Result.Count == 0)
            {
                validMessage += "使用劳务队伍不能为空！\n";
            }
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }

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

                decimal canUseQty=ClientUtil.TransToDecimal(dr.Cells[colCanUseQty.Name].Value);
                if (canUseQty <= 0)
                {
                    MessageBox.Show("物资【" + dr.Cells[colMaterialName.Name].Value + " " + dr.Cells[colMaterialSpec.Name].Value + "】库存数量不足，不能领料出库。");
                    dgDetail.CurrentCell = dr.Cells[colCanUseQty.Name];
                    return false;
                }

                object objQty=dr.Cells[colQuantity.Name].Value;
                if (objQty == null || objQty.ToString() == "" || ClientUtil.TransToDecimal(objQty) <= 0)
                {
                    MessageBox.Show("数量不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                if (canUseQty - decimal.Parse(objQty.ToString()) < 0)
                {
                    MessageBox.Show("出库数量不能大于库存数量！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                GWBSTree usedPart = dr.Cells[colUsedPart.Name].Tag as GWBSTree;
                if (usedPart == null || usedPart.Id  == null )
                {
                    MessageBox.Show("使用部位不能为空，请双击[使用部位]单元格选择。");
                    dgDetail.CurrentCell = dr.Cells[colUsedPart.Name];
                    return false;
                }
                CostAccountSubject subject = null;
                if (this.execType == EnumStockExecType.安装)
                {
                    DataGridViewComboBoxCell oCell = dr.Cells[colSetUpSubject.Name] as DataGridViewComboBoxCell;
                    subject = GetCostCell(oCell);
                }
                else
                {
                      subject = dr.Cells[this.colSubject.Name].Tag as CostAccountSubject;
                   
                }
                 if (subject == null || subject.Id == null ||  ClientUtil.ToString(subject.Name) == "")
                    {
                        MessageBox.Show("核算科目不能为空，请双击[核算科目]单元格选择。");
                        dgDetail.CurrentCell = dr.Cells[colSubject.Name];
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
            dgDetail.Update();
            return true;
        }
        public string  IsDifferentUnit()
        {
            //this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
            string sMsg = string.Empty;
            IList lstMaterial = new ArrayList();
            IList lstDiagramNum =new ArrayList ();
            Material oMaterial = null;
            string sDiagramNum=string.Empty ;
            foreach (DataGridViewRow oRow in this.dgDetail.Rows )
            {
                if (oRow.IsNewRow) break;
                oMaterial = oRow.Cells[colMaterialCode.Name].Tag as Material;
                if (oMaterial != null)
                {
                    lstMaterial.Insert(lstMaterial.Count ,oMaterial);
                }
                if (execType == EnumStockExecType.安装)
                {
                    sDiagramNum = ClientUtil.ToString(oRow.Cells[this.colDiagramNum.Name].Value);
                    lstDiagramNum.Insert(lstDiagramNum.Count, sDiagramNum);
                }
            }
            if (lstMaterial.Count > 0)
            {
                if (execType == EnumStockExecType.安装)
                {
                    sMsg = model.StockOutSrv.GetUnitDiffMaterial(lstMaterial, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), this.cboProfessionCat.SelectedItem.ToString(), lstDiagramNum);
                }
                else
                {
                    sMsg = model.StockOutSrv.GetUnitDiffMaterial(lstMaterial, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), "", null);
                }
            }
            return sMsg;
        }
        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
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

                curBillMaster.IsLimited = chkIsLimited.Checked ? 1 : 0;
                curBillMaster.Descript = this.txtRemark.Text;
    
                curBillMaster.SumMoney = ClientUtil.TransToDecimal(txtSumMoney.Text);
                curBillMaster.SumQuantity = ClientUtil.TransToDecimal(txtSumQuantity.Text);
               // curBillMaster.SubmitDate = DateTime.Now;
                //物资分类
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MatCatName = txtMaterialCategory.Text;
                }

                //专业分类
                curBillMaster.ProfessionCategory = cboProfessionCat.SelectedItem == null ? null : cboProfessionCat.SelectedItem.ToString();

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    StockOutDtl curBillDtl = new StockOutDtl();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as StockOutDtl;
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
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colQuantity.Name].Value);
                    curBillDtl.Price = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[colMoney.Name].Value);
                    if (execType == EnumStockExecType.安装)
                    {
                        curBillDtl.ConfirmMoney = ClientUtil.TransToDecimal(var.Cells[colConfirmMoney.Name].Value);
                        curBillDtl.ConfirmPrice = ClientUtil.TransToDecimal(var.Cells[colConfirmPrice.Name].Value);
                    }
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    if (var.Cells[colProfessionalCategory.Name].Value != null)
                    {
                        curBillDtl.ProfessionalCategory = var.Cells[colProfessionalCategory.Name].Value.ToString();
                    } else
                    {
                        curBillDtl.ProfessionalCategory = null;
                    }

                    //使用部位
                    curBillDtl.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                    if (var.Cells[colUsedPart.Name].Value != null)
                    {
                        curBillDtl.UsedPartName = var.Cells[colUsedPart.Name].Value.ToString();
                        curBillDtl.UsedPartSysCode = curBillDtl.UsedPart.SysCode;
                    } else
                    {
                        curBillDtl.UsedPartName = null;
                    }
                    if (this.ExecType == EnumStockExecType.安装)
                    {
                        DataGridViewComboBoxCell oCell=var.Cells[ colSetUpSubject .Name ] as DataGridViewComboBoxCell;
                        CostAccountSubject oCostAccountSubject = GetCostCell(oCell);
                        curBillDtl.SubjectGUID = oCostAccountSubject ;
                        curBillDtl.SubjectName = oCostAccountSubject.Name;
                        curBillDtl.SubjectSysCode = oCostAccountSubject.SysCode ;
                    }
                    else
                    {
                        curBillDtl.SubjectGUID = var.Cells[colSubject.Name].Tag as CostAccountSubject;
                        curBillDtl.SubjectName = ClientUtil.ToString(var.Cells[colSubject.Name].Value);
                        curBillDtl.SubjectSysCode = ClientUtil.ToString((var.Cells[colSubject.Name].Tag as CostAccountSubject).SysCode);
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

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals(colMaterialCode.Name))
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
            IList listDiagram = null;
            if (e.ColumnIndex < 0 || e.RowIndex<0) return;

            if (ExecType == EnumStockExecType.安装)
            {
                if (cboProfessionCat.SelectedItem == null)
                {
                    MessageBox.Show("请选择专业分类。");
                    return;
                }
            }
            if (this.txtMaterialCategory.Visible ==true && (  this.txtMaterialCategory.Result == null || this.txtMaterialCategory.Result.Count == 0))
            {
                MessageBox.Show("请先选择物资分类！");
                return;
            }
            MaterialCategory cat = null;
            if (ExecType == EnumStockExecType.安装)
            {
                cat = null;
            }
            else
            {

              cat = this.txtMaterialCategory.Result[0] as MaterialCategory;
                if (cat.Level != 3)
                {
                    MessageBox.Show("请选择一级物资分类！");
                    return;
                }
            }
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                if (this.ExecType == EnumStockExecType.土建)
                {
                    if (!IsSelectRelation)
                    {
                        #region 走物资
                        CommonMaterial materialSelector = new CommonMaterial();
                        if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                        {
                            materialSelector.materialCatCode = (txtMaterialCategory.Result[0] as MaterialCategory).Code;
                            materialSelector.projectId = projectInfo.Id;
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
                        #endregion
                    }
                    else
                    {
                        #region 走库存
                        if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                        {
                            VStockSelectMaterial oVStockSelectMaterial = new VStockSelectMaterial(txtMaterialCategory.Result[0] as MaterialCategory, this.execType, "", false);
                            oVStockSelectMaterial.ShowDialog();
                              lstMaterial=oVStockSelectMaterial .MatResult ;
                            //list = new ArrayList();
                            //foreach (SelectRelationInfo oSelectRelationInfo in lstMaterial)
                            //{
                            //    list.Add(oSelectRelationInfo.material);
                            //}
                        }
                        #endregion
                    }
                }
                else
                {
                    if (IsSelectRelation)
                    {
                        VStockSelectMaterial oVStockSelectMaterial = new VStockSelectMaterial(null, this.execType, this.cboProfessionCat.SelectedItem.ToString(), false);
                        oVStockSelectMaterial.ShowDialog();
                        lstMaterial = oVStockSelectMaterial.MatResult;
                    }
                    else
                    {
                        VStockOutSelectList oVStockOutSearchList = new VStockOutSelectList(this.cboProfessionCat.SelectedItem.ToString());
                        oVStockOutSearchList.ShowDialog();
                        list = oVStockOutSearchList.LstSelectMaterial;
                        listDiagram = oVStockOutSearchList.LstSelectDiagram;
                        //list = oVStockOutSearchList.LstSelectResult;

                        //if (list != null && list.Count > 0)
                        //{
                        //    list = model.StockOutSrv.GetMaterialLst(list);
                        //}
                    }
                }
                if (IsSelectRelation)
                {
                    //int iCount = 0;
                    Material theMaterial = null;
                    foreach (SelectRelationInfo oSelectRelationInfo in lstMaterial)
                    {
                        if (!IsSameMaterialAndUnit(oSelectRelationInfo))
                        {
                            theMaterial = oSelectRelationInfo.material;
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                            this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                            this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                            this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                            this.dgDetail[colMaterialStuff.Name, i].Value = theMaterial.Stuff;
                            this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                            //if (theMaterial.BasicUnit != null)
                            this.dgDetail[colUnit.Name, i].Value = oSelectRelationInfo.MaterialUnitName;
                            dgDetail[colProfessionalCategory.Name, i].Value = cboProfessionCat.SelectedItem;
                            //查询库存数量
                            try
                            {
                                dgDetail[colCanUseQty.Name, i].Value = 0;
                                IList stockRelList = null;
                                if (ExecType == EnumStockExecType.安装)
                                {
                                    this.dgDetail[this.colDiagramNum.Name, i].Value = oSelectRelationInfo.MaterialDiagramNum;
                                    //stockRelList = GetStockRelation(theMaterial.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), ClientUtil.ToString(this.dgDetail[this.colDiagramNum.Name, i].Value) );
                                    stockRelList = GetStockRelation(theMaterial.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), "", oSelectRelationInfo.MaterialUnitName);
                                    // iCount++;
                                }
                                else
                                {
                                    stockRelList = GetStockRelation(theMaterial.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), "", oSelectRelationInfo.MaterialUnitName);
                                }
                                dgDetail[colCanUseQty.Name, i].Value = GetStockQuantity(stockRelList);
                                dgDetail[colCanUseQty.Name, i].Tag = stockRelList;

                                //使用部位
                                if (stockRelList != null && stockRelList.Count > 0)
                                {
                                    StockRelation sr = stockRelList[0] as StockRelation;
                                    //StockInDtl stockInDtl= model.StockInSrv.GetStockInDtl(sr.StockInDtlId);
                                    ObjectQuery oq = new ObjectQuery();
                                    oq.AddFetchMode("UsedPart", NHibernate.FetchMode.Eager);
                                    oq.AddCriterion(Expression.Eq("Id", sr.StockInDtlId));
                                    IList list_1 = model.StockInSrv.GetStockInDtl(oq);
                                    if (list_1 != null && list_1.Count > 0)
                                    {
                                        StockInDtl stockInDtl = list_1[0] as StockInDtl;
                                        this.dgDetail[colUsedPart.Name, i].Tag = stockInDtl.UsedPart;
                                        this.dgDetail[colUsedPart.Name, i].Value = stockInDtl.UsedPartName;
                                        if (execType == EnumStockExecType.安装)
                                        {
                                        }
                                        else
                                        {
                                            this.dgDetail[this.colDiagramNum.Name, i].Value = stockInDtl.DiagramNumber;
                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(string.Format("查询物资【{0} {1}】库存数量出错。", theMaterial.Name, theMaterial.Specification));
                            }
                            DataGridViewComboBoxCell oCell = this.dgDetail[colSetUpSubject.Name, i] as DataGridViewComboBoxCell;
                            if (this.ExecType == EnumStockExecType.安装)
                            {
                                BindCell(oCell, "安装主材费");//绑定下拉框的值
                            }
                        }
                    }
                }
                else
                {
                    int iCount = 0;
                    foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                        this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                        this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                        this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                        this.dgDetail[colMaterialStuff.Name, i].Value = theMaterial.Stuff;
                        this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                        if (theMaterial.BasicUnit != null)
                            this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                        dgDetail[colProfessionalCategory.Name, i].Value = cboProfessionCat.SelectedItem;
                        //查询库存数量
                        try
                        {
                            dgDetail[colCanUseQty.Name, i].Value = 0;
                            IList stockRelList = null;
                            if (ExecType == EnumStockExecType.安装)
                            {
                                this.dgDetail[this.colDiagramNum.Name, i].Value = ClientUtil.ToString(listDiagram[iCount]);
                                stockRelList = GetStockRelation(theMaterial.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), ClientUtil.ToString(this.dgDetail[this.colDiagramNum.Name, i].Value));
                                iCount++;
                            }
                            else
                            {
                                stockRelList = GetStockRelation(theMaterial.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), "");
                            }
                            dgDetail[colCanUseQty.Name, i].Value = GetStockQuantity(stockRelList);
                            dgDetail[colCanUseQty.Name, i].Tag = stockRelList;

                            //使用部位
                            if (stockRelList != null && stockRelList.Count > 0)
                            {
                                StockRelation sr = stockRelList[0] as StockRelation;
                                //StockInDtl stockInDtl= model.StockInSrv.GetStockInDtl(sr.StockInDtlId);
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddFetchMode("UsedPart", NHibernate.FetchMode.Eager);
                                oq.AddCriterion(Expression.Eq("Id", sr.StockInDtlId));
                                IList list_1 = model.StockInSrv.GetStockInDtl(oq);
                                if (list_1 != null && list_1.Count > 0)
                                {
                                    StockInDtl stockInDtl = list_1[0] as StockInDtl;
                                    this.dgDetail[colUsedPart.Name, i].Tag = stockInDtl.UsedPart;
                                    this.dgDetail[colUsedPart.Name, i].Value = stockInDtl.UsedPartName;
                                    if (execType == EnumStockExecType.安装)
                                    {
                                    }
                                    else
                                    {
                                        this.dgDetail[this.colDiagramNum.Name, i].Value = stockInDtl.DiagramNumber;
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format("查询物资【{0} {1}】库存数量出错。", theMaterial.Name, theMaterial.Specification));
                        }
                        DataGridViewComboBoxCell oCell = this.dgDetail[colSetUpSubject.Name, i] as DataGridViewComboBoxCell;
                        if (this.ExecType == EnumStockExecType.安装)
                        {
                            BindCell(oCell, "安装主材费");//绑定下拉框的值
                        }
                    }
                }
            }
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
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
                        this.dgDetail.Rows[e.RowIndex].Cells[colUsedPart.Name].Value = task.Name;
                        this.dgDetail.Rows[e.RowIndex].Cells[colUsedPart.Name].Tag = task;
                        this.txtCode.Focus();
                    }
                }
            }
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSubject.Name))
            {
                VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                frm.ShowDialog();
                CostAccountSubject cost = frm.SelectAccountSubject;
                if (cost != null)
                {
                    //if (dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null )
                    //{
                        this.dgDetail.CurrentRow.Cells[colSubject.Name].Tag = cost;
                        this.dgDetail.CurrentRow.Cells[colSubject.Name].Value = cost.Name;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("请先选择物资信息！");
                    //    return;
                    //}
                }
                this.txtCode.Focus();
            }
        }
        public void BindCell(DataGridViewComboBoxCell oCell,string sSelectValue)
        {
            oCell.Items.Clear();
            foreach (CostAccountSubject oCostAccountSubject in listSetUpSubject)
            {
                oCell.Items.Add(oCostAccountSubject.Name);
                oCell.Value = sSelectValue;
            }
        }
        public CostAccountSubject GetCostCell(DataGridViewComboBoxCell oCell)
        {
            CostAccountSubject oCostAccountSubject = null;
            string sName=oCell .Value.ToString () ;
            foreach (CostAccountSubject o in listSetUpSubject)
            {
                if (string.Equals(o.Name, sName))
                {
                    oCostAccountSubject = o;
                    break;
                }
            }
            return oCostAccountSubject;
        }
        //显示数据
        private bool ModelToView()
        {
            try
            {
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
                chkIsLimited.Checked = (curBillMaster.IsLimited == 1);
               // txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;

               // this.txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                txtProject.Text = curBillMaster.ProjectName;
                txtSumQuantity.Text = curBillMaster.SumQuantity.ToString();
                txtSumMoney.Text = curBillMaster.SumMoney.ToString();

                ReadOnlyCat(false);

                //专业分类
                if (curBillMaster.ProfessionCategory != null)
                {
                    cboProfessionCat.SelectedItem = curBillMaster.ProfessionCategory;
                }

                //物资分类
                if (curBillMaster.MaterialCategory != null)
                {
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);
                    txtMaterialCategory.Value = curBillMaster.MatCatName;
                }
                if (string.IsNullOrEmpty(curBillMaster.Code))
                {
                    ReadOnlyCat(false);
                }
                else
                {
                    ReadOnlyCat(true);
                }
                                       
                this.dgDetail.Rows.Clear();
                foreach (StockOutDtl var in curBillMaster.Details)
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

                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    dgDetail[colMoney.Name, i].Value = var.Money;

                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    dgDetail[colProfessionalCategory.Name, i].Value =var.ProfessionalCategory;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    //使用部位
                    dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    if (this.ExecType == EnumStockExecType.安装)
                    {
                        DataGridViewComboBoxCell oCell = dgDetail[colSetUpSubject.Name, i] as DataGridViewComboBoxCell;
                        BindCell(oCell, var.SubjectName);
                    }
                    else
                    {
                        dgDetail[colSubject.Name, i].Tag = var.SubjectGUID;
                        dgDetail[colSubject.Name, i].Value = var.SubjectName ;
                    }
                   
                    if (execType == EnumStockExecType.安装)
                    {
                        dgDetail[colConfirmPrice.Name, i].Value = var.ConfirmPrice;
                        dgDetail[colConfirmMoney.Name, i].Value = var.ConfirmMoney;
                    }
                    //查询库存数量
                    try
                    {
                        dgDetail[colCanUseQty.Name, i].Value = 0;
                        IList stockRelList;
                        if (ExecType == EnumStockExecType.安装)
                        {
                            stockRelList = GetStockRelation(var.MaterialResource.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), ClientUtil.ToString(this.dgDetail[this.colDiagramNum.Name, i].Value));   
                            
                        }
                        else
                        {
                            stockRelList = GetStockRelation(var.MaterialResource.Id, curBillMaster.ProjectId, Enum.GetName(typeof(EnumStockExecType), execType), ClientUtil.ToString(this.dgDetail[this.colDiagramNum.Name, i].Value));   
                        }
                        dgDetail[colCanUseQty.Name, i].Value = GetStockQuantity(stockRelList);
                        dgDetail[colCanUseQty.Name, i].Tag = stockRelList;
                    } catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("查询物资【{0} {1}】库存数量出错。", var.MaterialName, var.MaterialSpec));
                    }

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

        private IList GetStockRelation(string materialId, string projectId, string special, string sDiagramNumber)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Material", materialId));
            oq.AddCriterion(Expression.Gt("RemainQuantity", 0M));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            
            if (ExecType == EnumStockExecType.安装)
            {
                oq.AddCriterion(Expression.Eq("ProfessionCategory", cboProfessionCat.SelectedItem.ToString()));
                if (string.IsNullOrEmpty(sDiagramNumber))
                {
                    oq.AddCriterion(Expression .IsNull ("DiagramNumber"));
                }
                else
                {
                    oq.AddCriterion(Expression.Eq("DiagramNumber", sDiagramNumber));
                }
            }
            oq.AddOrder(new Order("SeqCreateDate", true));
            return model.StockRelationSrv.GetStockRelation(oq);
        }
        private IList GetStockRelation(string materialId, string projectId, string special, string sDiagramNumber,string sMatStandUnitName)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Material", materialId));
            oq.AddCriterion(Expression.Gt("RemainQuantity", 0M));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddCriterion(Expression.Eq("MatStandardUnitName", sMatStandUnitName));

            if (ExecType == EnumStockExecType.安装)
            {
                oq.AddCriterion(Expression.Eq("ProfessionCategory", cboProfessionCat.SelectedItem.ToString()));
                if (string.IsNullOrEmpty(sDiagramNumber))
                {
                    oq.AddCriterion(Expression.IsNull("DiagramNumber"));
                }
                else
                {
                    oq.AddCriterion(Expression.Eq("DiagramNumber", sDiagramNumber));
                }
            }
            oq.AddOrder(new Order("SeqCreateDate", true));
            return model.StockRelationSrv.GetStockRelation(oq);
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

                //根据数量计算单价和金额                
                decimal sumqty = 0,sumMoney=0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                    if (quantity == null) quantity = 0;

                    decimal decQty=ClientUtil.TransToDecimal(quantity);
                    
                    if (i == e.RowIndex)
                    {
                        //根据输入数量，从库存列表中查找单价，计算金额
                        decimal canUseQty =ClientUtil.TransToDecimal( dgDetail.Rows[i].Cells[colCanUseQty.Name].Value);
                        IList stockRelList = dgDetail.Rows[i].Cells[colCanUseQty.Name].Tag as IList;
                        if (stockRelList == null || stockRelList.Count == 0)
                        {
                            dgDetail.Rows[i].Cells[colMoney.Name].Value = 0;
                            continue;
                        }
                        decimal tempMoney = 0M;
                        decimal dConfirmMoney = 0M;
                        decimal tempQuantity=decQty;
                        foreach (StockRelation obj in stockRelList)
                        {
                            if (obj.RemainQuantity >= tempQuantity)
                            {
                                tempMoney += tempQuantity * obj.Price;
                                if (execType == EnumStockExecType.安装)
                                {
                                    dConfirmMoney += tempQuantity * obj.Confirmprice;
                                }
                                break;
                            } else
                            {
                                tempMoney += obj.Price*obj.RemainQuantity;
                                if (execType == EnumStockExecType.安装)
                                {
                                    dConfirmMoney += obj.RemainQuantity * obj.Confirmprice;
                                }
                                tempQuantity = tempQuantity - obj.RemainQuantity;
                            }
                        }
                        dgDetail.Rows[i].Cells[colMoney.Name].Value = tempMoney.ToString("###########.##");
                        if (execType == EnumStockExecType.安装)
                        {
                            dgDetail.Rows[i].Cells[colConfirmMoney.Name].Value = dConfirmMoney.ToString("###########.##");
                        }
                        if (decQty != 0)
                        {
                            dgDetail.Rows[i].Cells[colPrice.Name].Value =decimal.Round(decimal.Divide(tempMoney, decQty), 8).ToString("##############.########");
                            if (execType == EnumStockExecType.安装)
                            {
                                dgDetail.Rows[i].Cells[colConfirmPrice.Name].Value = decimal.Round(decimal.Divide(dConfirmMoney, decQty), 4).ToString("##############.###");
                            }
                        }

                        sumMoney += tempMoney;

                    } else
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
        /// 汇总库存数量
        /// </summary>
        /// <param name="stockRelationList"></param>
        /// <returns></returns>
        private decimal GetStockQuantity(IList stockRelationList)
        {
            if (stockRelationList == null || stockRelationList.Count == 0) return 0M;
            decimal quantity = 0M;
            foreach (StockRelation obj in stockRelationList)
            {
                quantity += obj.RemainQuantity;
            }
            return quantity;
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"领料单.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            else
            {
                if (LoadTempleteFile(@"领料单_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            }
            return true;
        }

        public override bool Print()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"领料单.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.Print();
            }
            else
            {
                if (LoadTempleteFile(@"领料单_安装.flx") == false) return false;
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
                StaticMethod.InsertLogData(curBillMaster.Id, "打印", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "领料出库单", "", curBillMaster.ProjectName);
            }
        }
        public override bool Export()
        {
            if (curBillMaster.Special == "土建")
            {
                if (LoadTempleteFile(@"领料单.flx") == false) return false;
                FillFlex(curBillMaster);
                flexGrid1.ExportToExcel("领料出库单【" + curBillMaster.Code + "】", false, false, true);
            }
            else
            {
                if (LoadTempleteFile(@"领料单_安装.flx") == false) return false;
                FillFlex1(curBillMaster);
                flexGrid1.ExportToExcel("领料出库单【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(StockOut billMaster)
        {
           // int detailStartRowNumber = 5;//5为模板中的行号
            int detailStartRowNumber = 6;//5为模板中的行号
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
            //CommonUtil.SetFlexGridPrintByA4(this.flexGrid1);
            //主表数据

            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            string strLimit = "";
            if (billMaster.IsLimited.Equals("0"))
            {
                strLimit = "是";
            }
            else 
            {
                strLimit = "否";
            }
            flexGrid1.Cell(3, 5).Text = strLimit;
            flexGrid1.Cell(3, 7).Text = ClientUtil.ToString(billMaster.MatCatName);
            flexGrid1.Cell(4, 2).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 7).Text = billMaster.CreateDate.ToShortDateString();
            decimal sumQuantity = 0;
            decimal sumStockQuantity = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockOutDtl detail = (StockOutDtl)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;
                flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
                //数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
                sumQuantity += ClientUtil.ToDecimal(detail.Quantity.ToString());
                //使用部位
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.UsedPartName;
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                flexGrid1.Column(5).AutoFit();

                //核算科目
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.SubjectName;
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
               
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
            this.flexGrid1.Cell(1, 6).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 6).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 6).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            flexGrid1.Cell(detailStartRowNumber + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(detailStartRowNumber + detailCount, 5).Text = billMaster.RealOperationDate.ToShortDateString();
            flexGrid1.Cell(detailStartRowNumber + detailCount, 7).Text = billMaster.CreatePersonName;
        }
        private void FillFlex1(StockOut billMaster)
        {
            int detailStartRowNumber = 6;//5为模板中的行号
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
            flexGrid1.Cell(3, 6).Text = billMaster.Code;
            flexGrid1.Cell(4, 6).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 2).Text = billMaster.TheSupplierName;
            flexGrid1.Cell(4, 8).Text = billMaster.ProfessionCategory;
            decimal sumMoney = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                StockOutDtl detail = (StockOutDtl)billMaster.Details.ElementAt(i);
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
                flexGrid1.Column(5).AutoFit();

                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Quantity);
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.Price);
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.Money);
                flexGrid1.Cell(detailStartRowNumber + i, 8).WrapText = true;
                sumMoney += detail.Money;
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 9).WrapText = true;
                
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
                if (i == detailCount - 1)
                {
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 2).Text = ClientUtil.ToString(decimal.Round(sumMoney, 4));
                    string Moneybig = CurrencyComUtil.GetMoneyChinese(sumMoney.ToString());
                    flexGrid1.Cell(detailStartRowNumber + i + 1, 6).Text = ClientUtil.ToString(Moneybig);
                }
            }
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumMoney));
            this.flexGrid1.Cell(1, 8).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
            this.flexGrid1.Cell(1, 8).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 8).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
        }
        public bool IsSameMaterialAndUnit(SelectRelationInfo oSelectRelationInfo)
        {
            bool bFlag = false;
            Material oMaterial = null;
            string sMatUnitName = string.Empty;
            foreach (DataGridViewRow oRow in this.dgDetail.Rows)
            {
                if (oRow.IsNewRow) break;
                oMaterial = oRow.Cells[colMaterialCode.Name].Tag as Material;
                sMatUnitName = ClientUtil.ToString(oRow.Cells[colUnit.Name].Value);
                if (oMaterial != null)
                {
                    if (string.Equals(oMaterial.Id, oSelectRelationInfo.material.Id) && string.Equals(sMatUnitName, oSelectRelationInfo.MaterialUnitName))
                    {
                        bFlag = true;
                    }
                }
            }
            return bFlag;
        }
    }
}
