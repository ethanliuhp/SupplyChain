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
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng
{
    public partial class VMaterialRentalSettlement : TMasterDetailView
    {
        private MMatRentalMng model = new MMatRentalMng();
        private MaterialRentalSettlementMaster curBillMaster;
        private MaterialRentalSettlementDetail curBillDetail;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public MaterialRentalSettlementMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public MaterialRentalSettlementDetail CurBillDetail
        {
            get { return curBillDetail; }
            set { curBillDetail = value; }
        }

        private IList list = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList List
        {
            get { return list; }
            set { list = value; }
        }

        private string[] arrDateUnitNames = new string[] { "月", "天", "台班" };

        public VMaterialRentalSettlement()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
            InitData();
        }

        public void InitData()
        {
            //给设备来源添加下拉列表框的信息
            //((DataGridViewComboBoxColumn)colSource).Items.AddRange(Enum.GetNames(typeof(MaterialResource)));

            //colSource.DataSource = Enum.GetValues(typeof(MaterialResource));
            colSource.DataSource = (Enum.GetNames(typeof(MaterialResource)));
            //txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode2;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-" + CommonUtil.SupplierCatCode4 + "-" + CommonUtil.SupplierCatCode2;
            colDateUnit.Items.AddRange(arrDateUnitNames);
        }

        public void InitDate()
        {
            DateTimePicker dp = new DateTimePicker();
            DataGridViewCalendarColumn dc = new DataGridViewCalendarColumn();
            dp.CustomFormat = "yyyy-MM-dd";
            dp.Format = DateTimePickerFormat.Custom;
            dp.Visible = false;
            dgDetail.Controls.Add(dp);
            dp = new DateTimePicker();

        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.CellEnter += new DataGridViewCellEventHandler(dgDetail_CellEnter);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }

        void dgDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            object _DateControl = dgDetail.Controls["DataGridViewCalendarColumn"];
            if (_DateControl == null) return;
            DateTimePicker _DateTimePicker = (DateTimePicker)_DateControl;


            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)//修改想要的列
            {
                Rectangle _Rectangle = dgDetail.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _DateTimePicker.Size = new Size(_Rectangle.Width, _Rectangle.Height);
                _DateTimePicker.Location = new Point(_Rectangle.X, _Rectangle.Y);
                _DateTimePicker.Visible = true;
            }
            else
            {
                _DateTimePicker.Visible = false;
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
                    curBillMaster.Details.Remove(dr.Tag as MaterialRentalSettlementDetail);
                }
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
                    curBillMaster = model.MatMngSrv.GetMaterialRentalSettlementById(Id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtHandlePerson, txtProject, txtSumQuantity, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialCode.Name, colMaterialName.Name, colMaterialSep.Name, colSettleMoney.Name, colSubject.Name, colUsedPart.Name, colQuantityUnit.Name };
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
                this.curBillMaster = new MaterialRentalSettlementMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称

                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                //curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                //curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                curBillMaster.DocState = DocumentState.Edit;//状态
                //curBillMaster.RealOperationDate = DateTime.Now;//业务日期默认为当前时间
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                projectInfo = StaticMethod.GetProjectInfo();
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
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.MatMngSrv.GetMaterialRentalSettlementById(curBillMaster.Id);
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

            //判断明细金额和耗用金额是否相等
            foreach (MaterialRentalSettlementDetail subDetial in curBillMaster.Details)
            {
                decimal detailMoney = subDetial.SettleMoney;
                decimal sumSubjectMoney = 0;
                foreach (MaterialSubjectDetail subject in subDetial.MaterialSubjectDetails)
                {
                    sumSubjectMoney += subject.SettleMoney;
                }

                if (decimal.Round(detailMoney, 2) != decimal.Round(sumSubjectMoney, 2))
                {
                    MessageBox.Show("设备[" + subDetial.MaterialName + "/" + subDetial.MaterialSpec + "]的明细金额["
                        + decimal.Round(detailMoney, 2) + "]和对应分科目费用金额之和[" + decimal.Round(sumSubjectMoney, 2) + "]不相等，请调整分科目费用金额！");
                    return false;
                }
            }

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
                curBillMaster.DocState = DocumentState.InAudit;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            }
            curBillMaster = model.SaveMaterialRentalSettlement(curBillMaster);
            this.txtCode.Text = curBillMaster.Code;
            MessageBox.Show("保存成功！");
            log.BillId = curBillMaster.Id;
            log.BillType = "机械租赁结算单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
            return true;
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
                    return this.SaveOrSubmitBill(2);
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    return this.SaveOrSubmitBill(1);
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能保存！");
                return false;
            }
            catch (Exception e)
            {
                if (e.Message != null && e.Message.IndexOf("SETTLESUBJECT") != -1)
                {
                    MessageBox.Show("数据保存错误：【核算科目不能为空！】");
                }
                else
                {
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
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
                MaterialRentalSettlementDetail ad = new MaterialRentalSettlementDetail();
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (ad.RefQuantity > 0)
                    {
                        MessageBox.Show("此信息被引用，删除失败！");
                        return false;
                    }
                    else
                    {
                        curBillMaster = model.MatMngSrv.GetMaterialRentalSettlementById(curBillMaster.Id);
                        if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                        {
                            if (!model.MatMngSrv.DeleteByDao(curBillMaster)) return false;
                            LogData log = new LogData();
                            log.BillId = curBillMaster.Id;
                            log.BillType = "机械租赁结算单";
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
                }
                string message = "此单状态为【{0}】，不能删除！";
                message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
                MessageBox.Show(message);
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
                        curBillMaster = model.MatMngSrv.GetMaterialRentalSettlementById(curBillMaster.Id);
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
                curBillMaster = model.MatMngSrv.GetMaterialRentalSettlementById(curBillMaster.Id);
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
            if (txtSupply.Text == "")
            {
                MessageBox.Show("供应商信息不能为空！");
                return false;
            }
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
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

                if (dr.Cells[colMaterialCode.Name].Value == null)
                {
                    MessageBox.Show("材料编码不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                if (ClientUtil.ToDateTime(dr.Cells[colStartDate.Name].Value) > ClientUtil.ToDateTime(dr.Cells[colEndDate.Name].Value))
                {
                    MessageBox.Show("租赁结束时间要大于开始时间！");
                    return false;
                }
                if (dr.Cells[colSubject.Name].Value == null)
                {
                    MessageBox.Show("结算科目不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colSubject.Name];
                    return false;
                }
                if (dr.Cells[this.colUsedPart.Name].Value == null)
                {
                    MessageBox.Show("使用部位不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colUsedPart.Name];
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
                txtCode.Focus();
                curBillMaster.CreateDate = dtCreateDate.Value.Date;//业务时间
                curBillMaster.StartDate = dtpDateBegin.Value.Date;
                curBillMaster.EndDate = dtpDateEnd.Value.Date;
                curBillMaster.HandlePerson = this.txtHandlePerson.Tag as PersonInfo;
                curBillMaster.HandlePersonName = ClientUtil.ToString(this.txtHandlePerson.Text);
                if (txtSupply.Text != "")
                {
                    curBillMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    curBillMaster.SupplierName = ClientUtil.ToString(this.txtSupply.Text);
                }
                curBillMaster.Descript = ClientUtil.ToString(this.txtDescript.Text);

                var lstAllUnit = GetDateUnit();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MaterialRentalSettlementDetail curBillDtl = new MaterialRentalSettlementDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as MaterialRentalSettlementDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescription.Name].Value);
                    curBillDtl.MaterialSource = ClientUtil.ToString(var.Cells[colSource.Name].Value);
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    //curBillDtl.MaterialStuff = (var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material).Stuff;
                    curBillDtl.MaterialSysCode = var.Cells[colMaterialName.Name].Tag as string;
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSep.Name].Value);
                    curBillDtl.Money = ClientUtil.ToDecimal(var.Cells[colSettleMoney.Name].Value);

                    curBillDtl.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);
                    curBillDtl.QuantityUnit = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;
                    curBillDtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);
                    curBillDtl.SettleDate = ClientUtil.ToDecimal(var.Cells[colSettleDate.Name].Value);
                    curBillDtl.SettleMoney = ClientUtil.ToDecimal(var.Cells[colSettleMoney.Name].Value);
                    curBillDtl.StartSettleDate = Convert.ToDateTime(var.Cells[colStartDate.Name].Value);
                    curBillDtl.EndSettleDate = Convert.ToDateTime(var.Cells[colEndDate.Name].Value);
                    //curBillDtl.UsedPart = ClientUtil.ToString(var.Cells[colUsedPart.Name].Tag as GWBSTree);
                    //curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                    if (var.Cells[colUsedPart.Name].Value != null)
                    {
                        curBillDtl.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                        curBillDtl.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                        curBillDtl.UsedPartSysCode = curBillDtl.UsedPart.SysCode;
                    }
                    //时间单位分为日和月
                    //if (var.Cells[colDateUnit.Name].Value.Equals("天"))
                    //{
                    //    string strUnit1 = "天";
                    //    Application.Resource.MaterialResource.Domain.StandardUnit Unit1 = null;
                    //    ObjectQuery oq1 = new ObjectQuery();
                    //    oq1.AddCriterion(Expression.Eq("Name", strUnit1));
                    //    IList lists1 = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq1);
                    //    if (lists1 != null && lists1.Count > 0)
                    //    {
                    //        Unit1 = lists1[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                    //        var.Cells[colDateUnit.Name].Value = Unit1.Name;
                    //        var.Cells[colDateUnit.Name].Tag = Unit1;
                    //    }
                    //}
                    //时间单位里面存在月，天，台班，为了避免循环里面访问数据库，故作此修改
                    string strUnit1 = ClientUtil.ToString(var.Cells[colDateUnit.Name].Value);
                    if (lstAllUnit != null && lstAllUnit.Count > 0)
                    {
                        var curDateUnit = lstAllUnit.FirstOrDefault(p => p.Name == strUnit1);
                        if (curDateUnit != null)
                        {
                            var.Cells[colDateUnit.Name].Value = curDateUnit.Name;
                            var.Cells[colDateUnit.Name].Tag = curDateUnit;
                            //curBillDtl.DateUnit = curDateUnitt;
                        }
                    }
                    curBillDtl.DateUnit = var.Cells[colDateUnit.Name].Tag as StandardUnit;
                    curBillDtl.DateUnitName = ClientUtil.ToString(var.Cells[colDateUnit.Name].Value);
                    //价格单位默认为元
                    string strUnit = "元";
                    Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", strUnit));
                    IList lists = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                    if (lists != null && lists.Count > 0)
                    {
                        Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                        curBillDtl.PriceUnit = Unit;
                        curBillDtl.PriceUnitName = Unit.Name;
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
                    this.dgDetail.CurrentRow.Cells[colMaterialSep.Name].Value = selectedMaterial.Specification;
                    this.dgDetail.RefreshEdit();
                }
            }
        }

        /// <summary>
        /// 在设备编码列，敲击键盘时，取消原来已经选择的设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgDetail.CurrentCell.Tag = null;
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.materialCatCode = "02";
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
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                        this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                        this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                        this.dgDetail[colMaterialName.Name, i].Tag = theMaterial.TheSyscode;
                        this.dgDetail[colMaterialSep.Name, i].Value = theMaterial.Specification;
                        this.dgDetail[colEndDate.Name, i].Value = DateTime.Now;
                        this.dgDetail[colStartDate.Name, i].Value = DateTime.Now;
                        this.dgDetail[colSource.Name, i].Value = "内部租赁";
                        string strUnit = "台";
                        Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Name", strUnit));
                        IList lists = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                        if (lists != null && lists.Count > 0)
                        {
                            Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                            this.dgDetail[colQuantityUnit.Name, i].Value = Unit.Name;
                            this.dgDetail[colQuantityUnit.Name, i].Tag = Unit;
                        }
                        string strUnit1 = "月";
                        Application.Resource.MaterialResource.Domain.StandardUnit Unit1 = null;
                        ObjectQuery oq1 = new ObjectQuery();
                        oq1.AddCriterion(Expression.Eq("Name", strUnit1));
                        IList lists1 = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq1);
                        if (lists1 != null && lists1.Count > 0)
                        {
                            Unit1 = lists1[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                            this.dgDetail[colDateUnit.Name, i].Value = Unit1.Name;
                            this.dgDetail[colDateUnit.Name, i].Tag = Unit1;
                        }
                        GWBSTree Part = null;
                        ObjectQuery oqTree = new ObjectQuery();
                        oqTree.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                        oqTree.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                        IList listTree = model.MatMngSrv.GetDomainByCondition(typeof(GWBSTree), oqTree);
                        if (listTree != null && listTree.Count > 0)
                        {
                            Part = listTree[0] as GWBSTree;
                            this.dgDetail[colUsedPart.Name, i].Value = Part.Name;
                            this.dgDetail[colUsedPart.Name, i].Tag = Part;
                        }
                        i++;
                    }
                    txtCode.Focus();
                }
                else
                {
                    if (this.dgDetail[colMaterialCode.Name, e.RowIndex].Value == null)
                    {
                        MessageBox.Show("请先选择设备信息");
                        return;
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSubject.Name))//分科目费用
                {

                    MaterialRentalSettlementDetail materialRentalSettlementDetail = dgDetail.Rows[e.RowIndex].Tag as MaterialRentalSettlementDetail;
                    if (materialRentalSettlementDetail == null)
                    {
                        materialRentalSettlementDetail = new MaterialRentalSettlementDetail();
                        dgDetail.Rows[e.RowIndex].Tag = materialRentalSettlementDetail;
                    }
                    VMaterialSelector MaterialSubject = new VMaterialSelector();
                    IList oldList = dgDetail[e.ColumnIndex, e.RowIndex].Tag as IList;
                    if (materialRentalSettlementDetail.Id == null)
                    {
                        if (oldList != null)
                        {
                            MaterialSubject.Result = oldList;
                        }
                    }
                    materialRentalSettlementDetail.MaterialResource = dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    if (materialRentalSettlementDetail.Id == null)
                    {
                        materialRentalSettlementDetail.MaterialResource = dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                        materialRentalSettlementDetail.MaterialCode = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value);
                        materialRentalSettlementDetail.MaterialName = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colMaterialName.Name].Value);
                        materialRentalSettlementDetail.Quantity = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colQuantity.Name].Value);
                        materialRentalSettlementDetail.SettleDate = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colSettleDate.Name].Value);
                        materialRentalSettlementDetail.DateUnit = dgDetail.CurrentRow.Cells[colDateUnit.Name].Tag as StandardUnit;
                        materialRentalSettlementDetail.DateUnitName = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colDateUnit.Name].Value);
                    }
                    materialRentalSettlementDetail.QuantityUnit = dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag as StandardUnit;
                    materialRentalSettlementDetail.QuantityUnitName = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value);
                    materialRentalSettlementDetail.Quantity = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colQuantity.Name].Value);
                    materialRentalSettlementDetail.SettleDate = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colSettleDate.Name].Value);
                    foreach (MaterialSubjectDetail obj in materialRentalSettlementDetail.MaterialSubjectDetails)
                    {
                        obj.SettleQuantity = materialRentalSettlementDetail.Quantity;
                        //obj.Price = materialRentalSettlementDetail.Price;
                        //if (obj.DateUnitName == "月")
                        //{
                        //    obj.SettleMoney = Math.Round(obj.SettleQuantity * obj.SettlePrice * obj.SettleDate, 2);
                        //}
                        //else 
                        //{
                        //    obj.SettleMoney = Math.Round((obj.SettleQuantity * obj.SettlePrice * obj.SettleDate) / 30, 2);
                        //}
                        //当前单价即为当前单位对应的单价，无需做转换处理【如：除以30】
                        obj.SettleMoney = Math.Round(obj.SettleQuantity * obj.SettlePrice * obj.SettleDate, 2);
                    }
                    MaterialSubject.CurBillDetail = materialRentalSettlementDetail;
                    MaterialSubject.ShowDialog();
                    string stra = "有费用信息";
                    string strb = "无费用信息";
                    IList list = MaterialSubject.Result;
                    if (list == null || list.Count == 0)
                    {
                        dgDetail[colSubject.Name, e.RowIndex].Value = strb;
                        dgDetail.CurrentRow.Cells[colSettleMoney.Name].Value = "";
                        materialRentalSettlementDetail.MaterialSubjectDetails.Clear();
                        txtCode.Focus();
                        return;
                    }
                    dgDetail[e.ColumnIndex, e.RowIndex].Tag = list;
                    dgDetail[colSubject.Name, e.RowIndex].Value = stra;
                    if (materialRentalSettlementDetail.Id == null)
                    {
                        //新建的时候将已经选择的信息清空，重新选择
                        materialRentalSettlementDetail.MaterialSubjectDetails.Clear();
                    }
                    foreach (MaterialSubjectDetail materialSubject in list)
                    {
                        materialRentalSettlementDetail.AddSubjectDetail(materialSubject);
                    }
                    dgDetail.CurrentRow.Cells[colSettleMoney.Name].Value = materialRentalSettlementDetail.SettleMoney;
                    string colName = dgDetail.Columns[e.ColumnIndex].Name;
                    txtCode.Focus();
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
                {
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    List<TreeNode> list = frm.SelectResult;
                    if (list.Count > 0)
                    {
                        dgDetail[colUsedPart.Name, e.RowIndex].Tag = (list[0] as TreeNode).Tag as GWBSTree;
                        dgDetail[colUsedPart.Name, e.RowIndex].Value = (list[0] as TreeNode).Text;
                        txtCode.Focus();
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colQuantityUnit.Name))
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = su;
                        this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = su.Name;
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
                this.dtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                this.dtpDateBegin.Text = curBillMaster.StartDate.ToShortDateString();
                this.dtpDateEnd.Text = curBillMaster.EndDate.ToShortDateString();
                this.txtCode.Text = curBillMaster.Code;
                this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                this.txtHandlePerson.Text = curBillMaster.HandlePersonName;
                if (curBillMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Result.Add(curBillMaster.TheSupplierRelationInfo);
                }
                this.txtSupply.Value = curBillMaster.SupplierName;
                this.txtDescript.Text = curBillMaster.Descript;
                this.txtSumMoney.Text = ClientUtil.ToString(curBillMaster.SumMoney);
                this.txtSumQuantity.Text = ClientUtil.ToString(curBillMaster.SumQuantity);
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtProject.Tag = curBillMaster.ProjectId;
                this.txtProject.Text = curBillMaster.ProjectName.ToString();
                this.dgDetail.Rows.Clear();
                decimal sumMoney = 0;
                foreach (MaterialRentalSettlementDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart as GWBSTree;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialName.Name, i].Tag = var.MaterialSysCode;
                    this.dgDetail[colMaterialSep.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnit;
                    this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnitName;
                    this.dgDetail[colSettleDate.Name, i].Value = var.SettleDate;
                    this.dgDetail[colSettleMoney.Name, i].Value = var.SettleMoney;
                    object quantity = var.SettleMoney;
                    if (quantity != null)
                    {
                        sumMoney += decimal.Parse(quantity.ToString());
                    }
                    this.dgDetail[colSource.Name, i].Value = var.MaterialSource;
                    this.dgDetail[colStartDate.Name, i].Value = var.StartSettleDate;
                    this.dgDetail[colEndDate.Name, i].Value = var.EndSettleDate;
                    this.dgDetail[colDateUnit.Name, i].Tag = var.DateUnit;
                    this.dgDetail[colDescription.Name, i].Value = var.Descript;
                    this.dgDetail[colDateUnit.Name, i].Value = var.DateUnitName;
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    this.dgDetail[colSource.Name, i].Value = var.MaterialSource;
                    string strId = ClientUtil.ToString(var.Id);
                    list = model.MatMngSrv.GetMaterialSubjectByParentId(strId);
                    if (list.Count != 0)
                    {
                        string stra = "有费用信息";
                        this.dgDetail[colSubject.Name, i].Value = stra;
                    }
                    else
                    {
                        string stra = "无费用信息";
                        this.dgDetail[colSubject.Name, i].Value = stra;
                    }
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


        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                if (colName == colSettleMoney.Name)//金额
                {
                    string Money = ClientUtil.ToString(dgDetail.Rows[i].Cells[colSettleMoney.Name].Value);
                    if (Money == null || Money == "")
                    {
                        Money = "0";
                        return;
                    }
                    validity = CommonMethod.VeryValid(Money);
                    if (validity == false)
                    {
                        MessageBox.Show("核算合价为数字！");
                        this.dgDetail.Rows[i].Cells[colSettleMoney.Name].Value = "";
                        return;
                    }
                    sumMoney += ClientUtil.ToDecimal(Money);
                    txtSumMoney.Text = ClientUtil.ToString(sumMoney);
                }
                //string Moneys = ClientUtil.ToString(dgDetail.Rows[i].Cells[colSettleMoney.Name].Value);
                //if (Moneys == null || Moneys == "")
                //{
                //    Moneys = "0";
                //    sumMoney += ClientUtil.ToDecimal(Moneys);
                //    txtSumMoney.Text = ClientUtil.ToString(sumMoney);

                //}
                if (colName == colQuantity.Name)//数量
                {
                    string quantity = ClientUtil.ToString(dgDetail.Rows[i].Cells[colQuantity.Name].Value);
                    if (quantity == null || quantity == "")
                    {
                        quantity = "0";
                        return;
                    }
                    validity = CommonMethod.VeryValid(quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("数量为数字！");
                        this.dgDetail.Rows[i].Cells[colQuantity.Name].Value = "";
                        return;
                    }
                    sumQuantity += ClientUtil.ToDecimal(quantity);
                    txtSumQuantity.Text = ClientUtil.ToString(sumQuantity);
                }
            }
            decimal sumMoney1 = 0;
            for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            {
                string Money = ClientUtil.ToString(dgDetail.Rows[i].Cells[colSettleMoney.Name].Value);
                if (Money == null || Money == "")
                {
                    Money = "0";
                }
                sumMoney1 += ClientUtil.ToDecimal(Money);

            }
            txtSumMoney.Text = ClientUtil.ToString(sumMoney1);

        }

        /// <summary>
        /// 获取时间单位信息
        /// </summary>
        /// <returns></returns>
        private List<StandardUnit> GetDateUnit()
        {
            ObjectQuery oq1 = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string name in arrDateUnitNames)
            {
                dis.Add(Expression.Eq("Name", name));
            }
            oq1.AddCriterion(dis);
            var lst = model.MatMngSrv.GetDomainByCondition(typeof(StandardUnit), oq1).OfType<StandardUnit>().ToList();
            return lst;
        }
        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"废旧物料申请信息打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"废旧物料申请信息打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"废旧物料申请信息打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("废旧物料申请信息【" + curBillMaster.Code + "】", false, false, true);
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

        //private void FillFlex(WasteMatApplyMaster billMaster)
        //{
        //    int detailStartRowNumber = 6;//6为模板中的行号
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

        //    flexGrid1.Cell(2, 1).Text = "单据号：" ;

        //    flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
        //    flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;


        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        WasteMatApplyDetail detail = (WasteMatApplyDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(2, 1).Text = "单据号：" + detail.MaterialResource.Id;
        //        string a1 =  detail.MaterialResource.CreatedDate.ToString();
        //        string[] sArray1 = a1.Split(' ');
        //        string stra1 = sArray1[0];
        //        flexGrid1.Cell(4, 7).Text = stra1;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //数量
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.Quantity.ToString();

        //        //成色
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Grade.ToString();

        //        //申请处理时间
        //        string a = detail.ApplyDate.ToString();
        //        string[] sArray = a.Split(' ');
        //        string stra = sArray[0];
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = stra;

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.Descript;

        //        //物资分类层次
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //    }
        //}
    }
}
