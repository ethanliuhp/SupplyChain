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
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng
{
    public partial class VLaborDemandPlan : TMasterDetailView
    {
        private MLaborDemandPlanMng model = new MLaborDemandPlanMng();
        private LaborDemandPlanMaster curBillMaster;//当前劳务需求计划主表
        private LaborDemandPlanDetail curBillDetail;//当前劳务需求计划明细表
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public LaborDemandPlanMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public LaborDemandPlanDetail CurBillDetail
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

        public VLaborDemandPlan()
        {
            InitializeComponent();
            InitEvent();
            InitDate();
            //InitUsedRank();
        }

        //private void InitUsedRank()
        //{
        //    //添加劳务队伍下拉框
        //    VBasicDataOptr.InitUsedRankType(colUsedRankType, false);
        //    dgDetail.ContextMenuStrip = cmsDg;
        //}

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
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);//用户双击单元格中的任意位置时发生。
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);//在单元格失去输入焦点时发生，并启用内容验证功能。
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);//显示用于编辑单元格的控件时发生。
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);//当前选定单元格的编辑模式停止时发生。
            this.dgDetail.CellEnter += new DataGridViewCellEventHandler(dgDetail_CellEnter);//发生于DataGridView单元格的焦点获取的时候，或是单元格收到输入焦点的时候。
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }

        void dgDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            object _DateControl = dgDetail.Controls["DataGridViewCalendarColumn"];
            if (_DateControl == null) return;
            DateTimePicker _DateTimePicker = (DateTimePicker)_DateControl;//日期时间选择器
            if (e.ColumnIndex == 5)//修改想要的列
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
                DataGridViewRow dr = dgDetail.CurrentRow;//行数据
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as LaborDemandPlanDetail);
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
                    curBillMaster = model.LaborDemandPlanSrv.GetLaborDemandPlanById(Id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson,  txtProject };
            ObjectLock.Lock(os);

            //string[] lockCols = new string[] { colWokerType.Name, colProjectQuantityUnitName.Name, colProjectTimeLimitUnitName .Name};
            //dgDetail.SetColumnsReadOnly(lockCols);
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
                this.curBillMaster = new LaborDemandPlanMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.CreateDate = ConstObject.LoginDate;//制单时间
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                //curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                //curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                curBillMaster.DocState = DocumentState.Edit;//状态
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
                curBillMaster = model.LaborDemandPlanSrv.GetLaborDemandPlanById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
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
                if (!ViewToModel()) return false;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.SubmitDate = ConstObject.LoginDate;//提交时间
                curBillMaster = model.LaborDemandPlanSrv.SaveLaborDemandPlan(curBillMaster);
                //插入日志
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.SaveLaborDemandPlan(curBillMaster);

                //插入日志
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "劳务需求计划单";
                log.Code = curBillMaster.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                if (flag)
                {
                    log.OperType = "新增";
                }
                else
                {
                    log.OperType = "修改";
                }
                StaticMethod.InsertLogData(log);
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                MessageBox.Show("保存成功！");
                return true;
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
                LaborDemandPlanDetail ad = new LaborDemandPlanDetail();
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (ad.RefQuantity > 0)
                    {
                        MessageBox.Show("此信息被引用，删除失败！");
                        return false;
                    }
                    else
                    {
                        curBillMaster = model.LaborDemandPlanSrv.GetLaborDemandPlanById(curBillMaster.Id);
                        if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                        {
                            if (!model.LaborDemandPlanSrv.DeleteByDao(curBillMaster)) return false;
                            LogData log = new LogData();
                            log.BillId = curBillMaster.Id;
                            log.BillType = "劳务需求计划单";
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
                        curBillMaster = model.LaborDemandPlanSrv.GetLaborDemandPlanById(curBillMaster.Id);
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
                curBillMaster = model.LaborDemandPlanSrv.GetLaborDemandPlanById(curBillMaster.Id);
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
            if (this.txtPlanName.Text == "")
            {
                MessageBox.Show("工程计划不能为空！");
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

                if (dr.Cells[colProjectTask.Name].Value == null)
                {
                    MessageBox.Show("工程任务不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colProjectTask.Name];
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
                curBillMaster.RealOperationDate = ConstObject.LoginDate;
                curBillMaster.ReportTime = dtpSignDate.Value.Date;
                curBillMaster.Descript = ClientUtil.ToString(this.txtDescript.Text);
                curBillMaster.HandlePerson = this.txtHandlePerson.Tag as PersonInfo;
                curBillMaster.PlanName = ClientUtil.ToString(this.txtPlanName.Text);//计划名称
                curBillMaster.HandlePersonName = ClientUtil.ToString(this.txtHandlePerson.Text);
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    LaborDemandPlanDetail curBillDtl = new LaborDemandPlanDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as LaborDemandPlanDetail;
                        if (curBillDtl.Id == null)
                        {
                            //curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.ProjectTask = var.Cells[colProjectTask.Name].Tag as GWBSTree;
                    curBillDtl.ProjectTaskName = ClientUtil.ToString(var.Cells[colProjectTask.Name].Value);
                    //curBillDtl.pro
                    curBillDtl.EstimateProjectQuantity = ClientUtil.ToDecimal(var.Cells[colEstimateProjectQuantity.Name].Value);
                    curBillDtl.LaborRankInTime = Convert.ToDateTime(var.Cells[colLaborRankInTime.Name].Value);
                    //curBillDtl.LaborRankType = ClientUtil.ToString(var.Cells[collab]);
                    curBillDtl.MainJobDescript = ClientUtil.ToString(var.Cells[colMainJobDescript.Name].Value);
                    curBillDtl.ProjectQuantityUnit = var.Cells[colProjectQuantityUnitName.Name].Tag as StandardUnit;
                    curBillDtl.ProjectQuantityUnitName = ClientUtil.ToString(var.Cells[colProjectQuantityUnitName.Name].Value);
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    //if (var.Cells[colProjectTimeLimitUnitName.Name].Tag != null)
                    //{
                    //    curBillDtl.ProjectTimeLimitUnit = var.Cells[colProjectTimeLimitUnitName.Name].Tag as StandardUnit;
                    //}
                    //curBillDtl.ProjectTimeLimitUnit = var.Cells[colProjectTimeLimitUnitName.Name].Tag as StandardUnit;
                    //curBillDtl.ProjectTimeLimitUnitName = ClientUtil.ToString(var.Cells[colProjectTimeLimitUnitName.Name].Value);
                    curBillDtl.QualitySafetyRequirement = ClientUtil.ToString(var.Cells[colQualitySafetyRequirement.Name].Value);
                    //curBillDtl.UsedRankType = ClientUtil.ToString(var.Cells[colUsedRankType.Name].Value);
                    curBillDtl.ProjectTaskSysCode = ClientUtil.ToString((var.Cells[colProjectTask.Name].Tag as GWBSTree).SysCode);//工程任务层次码
                    curBillDtl.PlanLaborDemandNumber = ClientUtil.ToDecimal(var.Cells[colPlanLaborDemandNumber.Name].Value);
                    //curBillDtl.UsedRankTypeName = ClientUtil.ToString(var.Cells[colUsedRankType.Name].Value);
                    //curBillMaster. = var.Cells[colWokerType.Name].Value;
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
        /// 增加事件监听，支持处理键盘回车查询操作
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

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals("ProjectTask"))
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
                    //Application.Resource.MaterialResource.Domain.Material selectedMaterial = list[0] as Application.Resource.MaterialResource.Domain.Material;
                    //this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = selectedMaterial;
                    //this.dgDetail.CurrentCell.Value = selectedMaterial.Code;
                    //this.dgDetail.CurrentRow.Cells[colMaterialName.Name].Value = selectedMaterial.Name;
                    //this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = selectedMaterial.Specification;

                    ////动态分类复合单位                    
                    //DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
                    //cbo.Items.Clear();

                    //StandardUnit first = null;
                    //foreach (StandardUnit cu in selectedMaterial.GetMaterialAllUnit())
                    //{
                    //    cbo.Items.Add(cu.Name);
                    //}
                    //first = selectedMaterial.BasicUnit;
                    //this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = first;
                    //cbo.Value = first.Name;
                    //this.dgDetail.RefreshEdit();
                }
            }
        }

        /// <summary>
        /// 在工种列，敲击键盘时，取消原来已经选择的工种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgDetail.CurrentCell.Tag = null;
        }

        /// <summary>
        /// 工种列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)//指定用户如何在DataGridView控件中启动单元格编辑。
                //EditOnEnter:当单元格接收到焦点时即可开始编辑。在按Tab键在行中横向输入值，或按Enter键在列中纵向输入值时，此模式非常有用。
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProjectTask.Name))
                {
                    //DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    //VSelectGWBSDetail frm = new VSelectGWBSDetail(new MGWBSTree());
                    //frm.ShowDialog();

                    //if (frm.IsOk)
                    //{
                    //    List<GWBSDetail> list = frm.SelectGWBSDetail;
                    //    dgDetail.Rows.Clear();
                    //    foreach (GWBSDetail gwbsTree in list)
                    //    {
                    //        int i = dgDetail.Rows.Add();
                    //        this.dgDetail[colProjectTask.Name, i].Value = gwbsTree.TheGWBS.Name;
                    //        this.dgDetail[colProjectTask.Name, i].Tag = gwbsTree.TheGWBS;
                    //        this.dgDetail[colLaborRankInTime.Name, i].Value = DateTime.Now;
                    //        i++;
                    //    }
                    //}
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        TreeNode root = frm.SelectResult[0];//表示节点的TreeView
                        GWBSTree task = root.Tag as GWBSTree;//工程WBS树
                        if (task != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colProjectTask.Name].Value = task.Name;
                            this.dgDetail.CurrentRow.Cells[colProjectTask.Name].Tag = task;
                            this.dgDetail.CurrentRow.Cells[colLaborRankInTime.Name].Value = DateTime.Now;
                            this.txtCode.Focus();
                        }
                    }
                }

                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))//物资编码列双击时的事件
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    CommonMaterial materialSelector = new CommonMaterial();
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
                    }
                }

                //if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colWokerType.Name))
                //{
                //    LaborDemandPlanDetail laborDemandPlanDetail = dgDetail.Rows[e.RowIndex].Tag as LaborDemandPlanDetail;
                //    if (laborDemandPlanDetail == null)
                //    {
                //        laborDemandPlanDetail = new LaborDemandPlanDetail();
                //        dgDetail.Rows[e.RowIndex].Tag = laborDemandPlanDetail;
                //    }
                //    VWokerTypeSelector WokerType = new VWokerTypeSelector();
                //    IList oldList = dgDetail[e.ColumnIndex, e.RowIndex].Tag as IList;
                //    if (laborDemandPlanDetail.Id == null)
                //    {
                //        if (oldList != null)
                //        {
                //            WokerType.Result = oldList;
                //        }
                //    }
                //    WokerType.CurBillDetail = laborDemandPlanDetail;
                //    WokerType.ShowDialog();
                //    string stra = "有工种";
                //    IList list = WokerType.Result;
                //    if (list == null || list.Count == 0) return;
                //    dgDetail[e.ColumnIndex, e.RowIndex].Tag = list;//将工种信息保存在单元格中
                //    dgDetail[colWokerType.Name, e.RowIndex].Value = stra;
                //    if (laborDemandPlanDetail.Id == null)
                //    {
                //        //新建的时候将已经选择的信息清空，重新选择
                //        laborDemandPlanDetail.WorkerDetails.Clear();
                //    }
                //    foreach (LaborDemandWorkerType workerType in list)
                //    {
                //        laborDemandPlanDetail.AddWoker(workerType);
                //    }
                //    txtCode.Focus();
                //}
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProjectQuantityUnitName.Name))
                {
                    //双击数量单位
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colProjectQuantityUnitName.Name].Tag = su;
                        this.dgDetail.CurrentRow.Cells[colProjectQuantityUnitName.Name].Value = su.Name;
                        this.txtCode.Focus();
                    }
                }

                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());//该类定义了树、节点等的相关属性和方法
                    frm.IsTreeSelect = true;
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        TreeNode root = frm.SelectResult[0];

                        GWBSTree task = root.Tag as GWBSTree;
                        if (task != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Value = task.Name;
                            this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Tag = task;
                            this.txtCode.Focus();
                        }
                    }
                }

                //if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colProjectTimeLimitUnitName.Name))
                //{
                //    //双击数量单位
                //    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                //    if (su != null)
                //    {
                //        this.dgDetail.CurrentRow.Cells[colProjectTimeLimitUnitName.Name].Tag = su;
                //        this.dgDetail.CurrentRow.Cells[colProjectTimeLimitUnitName.Name].Value = su.Name;
                //        this.txtCode.Focus();
                //    }
                //}
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                //dtpDateBegin.Value = curBillMaster.RealOperationDate;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Value = curBillMaster.HandlePersonName;
                txtPlanName.Text = curBillMaster.PlanName;
                txtDescript.Text = curBillMaster.Descript;
                txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                txtProject.Text = curBillMaster.ProjectName.ToString();
                this.dgDetail.Rows.Clear();
                foreach (LaborDemandPlanDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //this.dgDetail[colUsedRankType.Name, i].Value = var.UsedRankType;
                    this.dgDetail[colQualitySafetyRequirement.Name, i].Value = var.QualitySafetyRequirement;

                    string strId = ClientUtil.ToString(var.Id);
                    list = model.LaborDemandPlanSrv.GetWorkTypeByParentId(strId);
                    //if (list.Count != 0)
                    //{
                    //    string stra = "有工种";
                    //    this.dgDetail[colWokerType.Name, i].Value = stra;
                    //}
                    //else
                    //{
                    //    string stra = "无工种";
                    //    this.dgDetail[colWokerType.Name, i].Value = stra;
                    //}
                    //this.dgDetail[colProjectTimeLimitUnitName.Name, i].Value = var.ProjectTimeLimitUnitName;
                    //dgDetail[colProjectTimeLimitUnitName.Name, i].Tag = var.ProjectTimeLimitUnit;
                    this.dgDetail[colProjectQuantityUnitName.Name, i].Tag = var.ProjectQuantityUnit;
                    this.dgDetail[colProjectQuantityUnitName.Name, i].Value = var.ProjectQuantityUnitName;

                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;

                    string a = var.LaborRankInTime.ToString();
                    string[] aArray = a.Split(' ');
                    string strz = aArray[0];
                    this.dgDetail[colLaborRankInTime.Name, i].Value = strz;
                    this.dgDetail[colEstimateProjectQuantity.Name, i].Value = var.EstimateProjectQuantity;
                    this.dgDetail[colProjectTask.Name, i].Value = var.ProjectTaskName;
                    this.dgDetail[colProjectTask.Name, i].Tag = var.ProjectTask;
                    this.dgDetail[colMainJobDescript.Name, i].Value = var.MainJobDescript;
                    this.dgDetail[colPlanLaborDemandNumber.Name, i].Value = var.PlanLaborDemandNumber;
                    this.dgDetail.Rows[i].Tag = var;
                }
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
            if (colName == colEstimateProjectQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colEstimateProjectQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colEstimateProjectQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("预计工程量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colEstimateProjectQuantity.Name].Value = "";
                        return;
                    }
                }
            }
        }
        #region 打印预览
        public override bool Preview()
        {
            if (LoadTempleteFile(@"劳务需求计划单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"劳务需求计划单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"劳务需求计划单.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("劳务需求计划单【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(LaborDemandPlanMaster billmaster)
        {
            int detailstartrownumber = 8;//6为模板中的行号
            int detailcount = 0; ;
            foreach (LaborDemandPlanDetail dtl in billmaster.Details)
            {
                foreach (LaborDemandWorkerType type in dtl.WorkerDetails)
                {
                    detailcount++;
                }
            }
            //插入明细行
            flexGrid1.InsertRow(detailstartrownumber, detailcount);
            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailstartrownumber, 1, detailstartrownumber + detailcount, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //填写明细数据
            int j = 0;
            for (int i = 0; i < billmaster.Details.Count ; i++)
            {
                LaborDemandPlanDetail detail = (LaborDemandPlanDetail)billmaster.Details.ElementAt(i);
                foreach (LaborDemandWorkerType workType in detail.WorkerDetails)
                {
                    flexGrid1.Cell(detailstartrownumber + j, 1).Text = ClientUtil.ToString(j + 1) ;
                    flexGrid1.Cell(detailstartrownumber + j, 1).WrapText = true;
                    flexGrid1.Cell(detailstartrownumber + j, 2).Text = workType.WorkerType;
                    flexGrid1.Cell(detailstartrownumber + j, 2).WrapText = true;
                    j++;
                }

            }
        }
        #endregion
    }
}
