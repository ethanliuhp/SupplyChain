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
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
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
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using System.IO;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng
{
    public partial class VMaterialSettleMng : TMasterDetailView
    {
        private MMaterialSettleMng model = new MMaterialSettleMng();
        private MaterialSettleMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空

        //private EnumMaterialSettleType materialType;
        ///// <summary>
        ///// 用来区分结算类型
        ///// </summary>
        //public EnumMaterialSettleType MaterialType
        //{
        //    get { return materialType; }
        //    set { materialType = value; }
        //}
        EnumMaterialSettleType materialType;
        /// <summary>
        /// 当前单据
        /// </summary>
        public MaterialSettleMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VMaterialSettleMng()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            if (btnExcel.Enabled)
            {
                string strFilePash = SearchExcel();
                string extFile = Path.GetExtension(strFilePash).ToUpper();//获得Fielupload的值
                if (extFile == ".XLS" || extFile == ".XLSX")//如果文件是.XLS格式的
                {
                    string str = System.IO.Path.GetFileName(strFilePash);
                    DataSet ds = ExploreFile.ReadDataFromExcel(strFilePash);
                    IList list = model.MaterialSettleSrv.GetExcel(ds);
                    if (list == null && list.Count == 0) return;
                    dgDetail.Rows.Clear();
                    //for (int i = 0; i < list.Count; i++)
                    foreach (MaterialSettleDetail lst in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialType.Name, i].Value = lst.MaterialName;
                        //Application.Resource.MaterialResource.Domain.Material mat = lst[1];
                        this.dgDetail[colMaterialType.Name, i].Tag = lst.MaterialResource;
                        this.dgDetail[colProjectTask.Name, i].Value = lst.ProjectTaskName;
                        this.dgDetail[colProjectTask.Name, i].Tag = lst.ProjectTask;
                        this.dgDetail[colAccountSubject.Name, i].Value = lst.AccountCostName;
                        this.dgDetail[colAccountSubject.Name, i].Tag = lst.AccountCostSubject;
                        this.dgDetail[colQuantity.Name, i].Value = lst.Quantity;
                        this.dgDetail[colQuantityUnit.Name, i].Value = lst.QuantityUnitName;
                        this.dgDetail[colQuantityUnit.Name, i].Tag = lst.QuantityUnit;
                        this.dgDetail[colPriceUnit.Name, i].Value = lst.PriceUnitName;
                        this.dgDetail[colPriceUnit.Name, i].Tag = lst.PriceUnit;
                        this.dgDetail[colPrice.Name, i].Value = lst.Price;
                        this.dgDetail[colSumMoney.Name, i].Value = lst.Money;
                        this.dgDetail[colDescript.Name, i].Value = lst.Descript;
                        this.dgDetail[colMaterialSpec.Name, i].Value = lst.MaterialSpec;
                        this.dgDetail[colMaterialStuff.Name, i].Value = lst.MaterialStuff;
                        this.dgDetail[colMaterialCode.Name, i].Value = lst.MaterialCode;
                        this.dgDetail[colMaterialSysCode.Name, i].Value = lst.MaterialSysCode;
                        this.dgDetail[colSubjectSysCode.Name, i].Value = lst.AccountCostCode;
                        this.dgDetail[colProjectSysCode.Name, i].Value = lst.ProjectTaskCode;
                    }
                }
            }
        }

        protected string SearchExcel()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "所有文件(*.*)|*.*";
            openFile.ShowDialog();
            string fileName = openFile.FileName;
            if (fileName.Equals("") || !System.IO.File.Exists(fileName))
            {
            }
            else
            {
                FileInfo finfo = new FileInfo(fileName);
                if (finfo.Length > int.MaxValue)
                {
                    MessageBox.Show("文件太大！系统加载失败！", "系统提示", MessageBoxButtons.OK);
                }
            }
            return fileName;
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
                    curBillMaster.Details.Remove(dr.Tag as DemandMasterPlanDetail);
                }
            }
        }


        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string Id, EnumMaterialSettleType SettleType)
        {
            try
            {
                materialType = SettleType;
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.MaterialSettleSrv.GetMaterialSettleById(Id);
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
                    btnExcel.Enabled = true;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    btnExcel.Enabled = false;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtProject };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colSumMoney.Name, colAccountSubject.Name,colMaterialSpec.Name, colPriceUnit.Name, colQuantityUnit.Name, colPriceUnit.Name, colQuantityUnit.Name ,colMaterialType.Name,colProjectTask.Name};
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

                this.curBillMaster = new MaterialSettleMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                curBillMaster.SettleState = Enum.GetName(typeof(EnumMaterialSettleType), materialType);

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单年月
                txtCreateMonth.Text = ClientUtil.ToString(ConstObject.LoginDate.Month);
                txtCreateYear.Text = ClientUtil.ToString(ConstObject.LoginDate.Year);
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                //txtContractNo.Focus();
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
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.MaterialSettleSrv.GetMaterialSettleById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
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
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            }
            curBillMaster = model.MaterialSettleSrv.SaveMaterialSettle(curBillMaster);
            this.txtCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            if (materialType == EnumMaterialSettleType.料具结算单维护)
            {
                log.BillType = "料具租赁结算单维护";
            }
            if (materialType == EnumMaterialSettleType.物资耗用结算单维护)
            {
                log.BillType = "材料耗用结算单维护";
            }
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    if (SaveOrSubmitBill(1) == false) return false;
                    MessageBox.Show("保存成功！");
                    return true;
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
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    if (SaveOrSubmitBill(2) == false) return false;
                    MessageBox.Show("提交成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能提交！");
                return false;
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
                curBillMaster = model.MaterialSettleSrv.GetMaterialSettleById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.MaterialSettleSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    if (materialType == EnumMaterialSettleType.料具结算单维护)
                    {
                        log.BillType = "料具租赁结算单维护";
                    }
                    if (materialType == EnumMaterialSettleType.物资耗用结算单维护)
                    {
                        log.BillType = "材料耗用结算单维护";
                    }
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
                        curBillMaster = model.MaterialSettleSrv.GetMaterialSettleById(curBillMaster.Id);
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
                curBillMaster = model.MaterialSettleSrv.GetMaterialSettleById(curBillMaster.Id);
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
            if (this.dgDetail.Rows.Count - 1 == 0)
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
                if (dr.Cells[colProjectTask.Name].Value != null)
                {
                    if ((dr.Cells[colProjectTask.Name].Tag as GWBSTree).Id == null)
                    {
                        MessageBox.Show("未找到工程任务。");
                        dgDetail.CurrentCell = dr.Cells[colProjectTask.Name];
                        return false;
                    }
                }
                if (dr.Cells[colPriceUnit.Name].Value != null)
                {
                    if ((dr.Cells[colPriceUnit.Name].Tag as StandardUnit).Id == null)
                    {
                        MessageBox.Show("未找到对应的价格计量单位。");
                        dgDetail.CurrentCell = dr.Cells[colPriceUnit.Name];
                        return false;
                    }
                }
                if (dr.Cells[colQuantityUnit.Name].Value != null)
                {
                    if ((dr.Cells[colQuantityUnit.Name].Tag as StandardUnit).Id == null)
                    {
                        MessageBox.Show("未找到对应的数量计量单位。");
                        dgDetail.CurrentCell = dr.Cells[colQuantityUnit.Name];
                        return false;
                    }
                }
                if (dr.Cells[colAccountSubject.Name].Value != null)
                {
                    if ((dr.Cells[colAccountSubject.Name].Tag as CostAccountSubject).Id == null)
                    {
                        MessageBox.Show("未找到对应的核算科目。");
                        dgDetail.CurrentCell = dr.Cells[colAccountSubject.Name];
                        return false;
                    }
                }
                if (dr.Cells[colMaterialType.Name].Value != null)
                {
                    if ((dr.Cells[colMaterialType.Name].Tag as Application.Resource.MaterialResource.Domain.Material).Id == null || (dr.Cells[colMaterialType.Name].Tag as Application.Resource.MaterialResource.Domain.Material).Id == "")
                    {
                        MessageBox.Show("未找到对应的资源。");
                        dgDetail.CurrentCell = dr.Cells[colMaterialType.Name];
                        return false;
                    }
                }
                if (dr.Cells[colProjectTask.Name].Value == null)
                {
                    MessageBox.Show("工程任务不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colProjectTask.Name];
                    return false;
                }
                if (dr.Cells[colQuantity.Name].Value == null)
                {
                    MessageBox.Show("数量不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }
                if (dr.Cells[colPrice.Name].Value == null)
                {
                    MessageBox.Show("单价不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }

            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                txtCode.Focus();
                curBillMaster.CreateDate = ClientUtil.ToDateTime(this.dtpDateBegin.Text);
                curBillMaster.Descript = ClientUtil.ToString(this.txtRemark.Text);
                curBillMaster.CreateYear = ClientUtil.ToInt(this.txtCreateYear.Text);
                curBillMaster.CreateMonth = ClientUtil.ToInt(this.txtCreateMonth.Text);
                if (materialType == EnumMaterialSettleType.料具结算单维护)
                {
                    curBillMaster.SettleState = "materialQuery";
                }
                if (materialType == EnumMaterialSettleType.物资耗用结算单维护)
                {
                    curBillMaster.SettleState = "materialSettleQuery";
                }
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MaterialSettleDetail curBillDtl = new MaterialSettleDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as MaterialSettleDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.AccountCostName = ClientUtil.ToString(var.Cells[colAccountSubject.Name].Value);//核算科目名称
                    curBillDtl.AccountCostSubject = var.Cells[colAccountSubject.Name].Tag as CostAccountSubject;//核算科目
                    curBillDtl.AccountCostCode = ClientUtil.ToString(var.Cells[colSubjectSysCode.Name].Value);//核算科目层次码
                    //curBillDtl.CostName = ClientUtil.ToString(var.Cells[colCoseName.Name].Value);//费用名称
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[colSumMoney.Name].Value);//费用金额
                    //curBillDtl.MonthlyCostSuccess = ClientUtil.ToInt();//月度核算成功标志
                    curBillDtl.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);//单价
                    curBillDtl.PriceUnit = var.Cells[colPriceUnit.Name].Tag as StandardUnit;//价格计量单位
                    curBillDtl.PriceUnitName = ClientUtil.ToString(var.Cells[colPriceUnit.Name].Value);//价格计量单位名称
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillDtl.ProjectTask = var.Cells[colProjectTask.Name].Tag as GWBSTree;//工程任务
                    curBillDtl.ProjectTaskCode = ClientUtil.ToString(var.Cells[colProjectSysCode.Name].Value);//部位层次码
                    curBillDtl.ProjectTaskName = ClientUtil.ToString(var.Cells[colProjectTask.Name].Value);//工程任务名称
                    curBillDtl.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);//数量
                    curBillDtl.QuantityUnit = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;//数量计量单位
                    curBillDtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);//数量计量单位名称
                    if (var.Cells[colMaterialType.Name].Tag != null)
                    {
                        Application.Resource.MaterialResource.Domain.Material theMaterial = var.Cells[colMaterialType.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                        //theMaterial = model.MaterialSettleSrv.Dao.Get(typeof(Application.Resource.MaterialResource.Domain.Material), theMaterial.Id) as Application.Resource.MaterialResource.Domain.Material;
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", theMaterial.Id));
                        IList lists = model.MaterialSettleSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.Material), oq);
                        if (lists != null && lists.Count > 0)
                        {
                            theMaterial = lists[0] as Application.Resource.MaterialResource.Domain.Material;
                        }
                        curBillDtl.MaterialResource = theMaterial;
                        curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialType.Name].Value);//物资名称
                        curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                        curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格
                        curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colMaterialStuff.Name].Value);//材质
                        curBillDtl.MaterialSysCode = ClientUtil.ToString(var.Cells[colMaterialSysCode.Name].Value);//层次码
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

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("数量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value = "";
                        flag = false;
                        return;
                    }
                }
            }
            if (colName == colPrice.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("单价为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value = "";
                        flag = false;
                        return;
                    }
                }
            }
            if (flag)
            {
                decimal sumqty = 0;
                decimal money = 0;
                decimal summoney = 0;
                object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;
                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                    if (quantity == null) quantity = 0;


                    if (ClientUtil.ToString(dgDetail.Rows[i].Cells[colQuantity.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[i].Cells[colPrice.Name].Value) != "")
                    {
                        money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colPrice.Name].Value);
                        dgDetail.Rows[i].Cells[colSumMoney.Name].Value = ClientUtil.ToString(money);
                        summoney = summoney + money;
                    }

                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                }
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
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProjectTask.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        TreeNode root = frm.SelectResult[0];
                        GWBSTree task = root.Tag as GWBSTree;
                        if (task != null)
                        {
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colProjectTask.Name, i].Value = task.Name;
                            this.dgDetail[colProjectTask.Name, i].Tag = task;
                            i++;
                        }
                    }
                    this.txtCode.Focus();


                    //VSelectGWBSDetail frm = new VSelectGWBSDetail(new MGWBSTree());
                    //frm.ShowDialog();

                    //if (frm.IsOk)
                    //{
                    //    List<GWBSDetail> list = frm.SelectGWBSDetail;
                    //    //dgDetail.Rows.Clear();
                    //    foreach (GWBSDetail gwbsTree in list)
                    //    {
                    //        if (dgDetail.CurrentRow.Cells[colAccountSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialType.Name].Value != null || dgDetail.CurrentRow.Cells[colPrice.Name].Value != null || dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantity.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colSumMoney.Name].Value != null)
                    //        {
                    //            this.dgDetail.CurrentRow.Cells[colProjectTask.Name].Value = gwbsTree.TheGWBS.Name;
                    //            this.dgDetail.CurrentRow.Cells[colProjectTask.Name].Tag = gwbsTree.TheGWBS;
                    //        }
                    //        else
                    //        {
                    //            int i = dgDetail.Rows.Add();
                    //            this.dgDetail[colProjectTask.Name, i].Value = gwbsTree.TheGWBS.Name;
                    //            this.dgDetail[colProjectTask.Name, i].Tag = gwbsTree.TheGWBS;
                    //            i++;

                    //        }
                    //    }
                    //    this.txtCode.Focus();
                    //}
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colAccountSubject.Name))
                {
                    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                    frm.ShowDialog();
                    CostAccountSubject cost = frm.SelectAccountSubject;
                    if (cost != null)
                    {
                        if (dgDetail.CurrentRow.Cells[colProjectTask.Name].Value != null || dgDetail.CurrentRow.Cells[colDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialType.Name].Value != null || dgDetail.CurrentRow.Cells[colPrice.Name].Value != null || dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantity.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colSumMoney.Name].Value != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colAccountSubject.Name].Tag = cost;
                            this.dgDetail.CurrentRow.Cells[colAccountSubject.Name].Value = cost.Name;
                        }
                        else
                        {
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colAccountSubject.Name,i].Tag = cost;
                            this.dgDetail[colAccountSubject.Name,i].Value = cost.Name;
                            i++;
                        }
                    }
                    this.txtCode.Focus();
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialType.Name))
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list.Count > 0)
                    {
                        List<Application.Resource.MaterialResource.Domain.Material> listResourceType = dgDetail.Tag as List<Application.Resource.MaterialResource.Domain.Material>;

                        if (listResourceType != null)
                        {
                            foreach (Application.Resource.MaterialResource.Domain.Material mat in list)
                            {
                                var query = from m in listResourceType
                                            where m.Id == mat.Id
                                            select m;

                                if (query.Count() == 0)
                                    listResourceType.Add(mat);
                            }
                        }
                        else
                            listResourceType = list.OfType<Application.Resource.MaterialResource.Domain.Material>().ToList();

                        //dgDetail.Rows.Clear();
                        foreach (Application.Resource.MaterialResource.Domain.Material mat in listResourceType)
                        {
                            if (dgDetail.CurrentRow.Cells[colProjectTask.Name].Value != null || dgDetail.CurrentRow.Cells[colDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colAccountSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colPrice.Name].Value != null || dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantity.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colSumMoney.Name].Value != null)
                            {
                                dgDetail.CurrentRow.Cells[colMaterialType.Name].Value = mat.Name;
                                dgDetail.CurrentRow.Cells[colMaterialType.Name].Tag = mat;
                                dgDetail.CurrentRow.Cells[colMaterialSysCode.Name].Value = mat.TheSyscode;
                                dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value = mat.Code;
                                dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = mat.Specification;
                               dgDetail.CurrentRow.Cells[colMaterialStuff.Name].Value = mat.Stuff;            
                            }
                            else
                            {
                                int i = dgDetail.Rows.Add();
                                dgDetail[colMaterialType.Name,i].Value = mat.Name;
                                dgDetail[colMaterialType.Name,i].Tag = mat;
                                i++;
                            }
                        }
                        this.txtCode.Focus();
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colPriceUnit.Name))
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        if (dgDetail.CurrentRow.Cells[colProjectTask.Name].Value != null || dgDetail.CurrentRow.Cells[colDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colAccountSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colPrice.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialType.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantity.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colSumMoney.Name].Value != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Tag = su;
                            this.dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value = su.Name;
                        }
                        else
                        {
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colPriceUnit.Name,i].Tag = su;
                            this.dgDetail[colPriceUnit.Name,i].Value = su.Name;
                            i++;
                        }
                        this.txtCode.Focus();
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colQuantityUnit.Name))
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        if (dgDetail.CurrentRow.Cells[colProjectTask.Name].Value != null || dgDetail.CurrentRow.Cells[colDescript.Name].Value != null || dgDetail.CurrentRow.Cells[colAccountSubject.Name].Value != null || dgDetail.CurrentRow.Cells[colPrice.Name].Value != null || dgDetail.CurrentRow.Cells[colMaterialType.Name].Value != null || dgDetail.CurrentRow.Cells[colQuantity.Name].Value != null || dgDetail.CurrentRow.Cells[colPriceUnit.Name].Value != null || dgDetail.CurrentRow.Cells[colSumMoney.Name].Value != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = su;
                            this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = su.Name;
                        }
                        else
                        {
                            int i = dgDetail.Rows.Add();
                            this.dgDetail[colQuantityUnit.Name,i].Tag = su;
                            this.dgDetail[colQuantityUnit.Name,i].Value = su.Name;
                            i++;
                        }
                        this.txtCode.Focus();
                    }
                }
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                dtpDateBegin.Value = curBillMaster.RealOperationDate;
                txtRemark.Text = curBillMaster.Descript;
                txtCreateMonth.Text = ClientUtil.ToString(curBillMaster.CreateMonth);
                txtCreateYear.Text = ClientUtil.ToString(curBillMaster.CreateYear);
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtProject.Text = curBillMaster.ProjectName.ToString();
                this.dgDetail.Rows.Clear();
                decimal sumMoney = 0;
                foreach (MaterialSettleDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colAccountSubject.Name, i].Tag = var.AccountCostSubject;
                    this.dgDetail[colAccountSubject.Name, i].Value = var.AccountCostName;
                    //this.dgDetail[colCoseName.Name, i].Value = var.CostName;//费用名称
                    this.dgDetail[colMaterialType.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialType.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colPriceUnit.Name, i].Value = var.PriceUnitName;
                    this.dgDetail[colPriceUnit.Name, i].Tag = var.PriceUnit;
                    this.dgDetail[colProjectTask.Name, i].Value = var.ProjectTaskName;
                    this.dgDetail[colProjectTask.Name, i].Tag = var.ProjectTask;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnitName;
                    this.dgDetail[colQuantityUnit.Name, i].Tag = var.QuantityUnit;
                    this.dgDetail[colSumMoney.Name, i].Value = var.Money;
                    object quantity = var.Money;
                    if (quantity != null)
                    {
                        sumMoney += decimal.Parse(quantity.ToString());
                    }
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[colSubjectSysCode.Name, i].Value = var.AccountCostCode;
                    this.dgDetail[colMaterialSysCode.Name, i].Value = var.MaterialSysCode;
                    this.dgDetail[colMaterialStuff.Name, i].Value = var.MaterialStuff;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colProjectSysCode.Name, i].Value = var.ProjectTaskCode;
                    this.dgDetail.Rows[i].Tag = var;
                }
                this.txtSumMoney.Text = sumMoney.ToString("#,###.####");
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"物资需求总计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("物资需求总计划【" + curBillMaster.Code + "】", false, false, true);
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

        //private void FillFlex(DemandMasterPlanMaster billMaster)
        //{
        //    int detailStartRowNumber = 5;//5为模板中的行号
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
        //    flexGrid1.Cell(3, 2).Text = billMaster.Code;
        //    flexGrid1.Cell(3, 4).Text = billMaster.MaterialCategoryName;
        //    flexGrid1.Cell(3, 6).Text = billMaster.RealOperationDate.ToShortDateString();
        //    flexGrid1.Cell(3, 7).Text = "编制依据：" + billMaster.Compilation;

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    decimal sumQuantity = 0;
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        DemandMasterPlanDetail detail = (DemandMasterPlanDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;

        //        //计量单位
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;

        //        //需用计划
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
        //        sumQuantity += detail.Quantity;

        //        //质量标准
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.QualityStandard);

        //        //生产厂家
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.Manufacturer);

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.Descript);
        //        if (i == detailCount - 1)
        //        {
        //            flexGrid1.Cell(detailStartRowNumber + i + 1, 4).Text = ClientUtil.ToString(sumQuantity);
        //        }
        //    }
        //    flexGrid1.Cell(6 + detailCount, 2).Text = billMaster.ProjectName;
        //    flexGrid1.Cell(6 + detailCount, 5).Text = billMaster.CreateDate.ToShortDateString();
        //    flexGrid1.Cell(6 + detailCount, 7).Text = billMaster.CreatePersonName;
        //}
    }
}
