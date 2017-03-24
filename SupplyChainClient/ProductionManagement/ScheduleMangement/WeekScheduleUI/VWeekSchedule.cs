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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;

using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using FlexCell;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekSchedule : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        MStockMng stockModel = new MStockMng();
        private WeekScheduleMaster curBillMaster;

        private EnumExecScheduleType execScheduleType;
        CurrentProjectInfo projectInfo = null;

        private int baseYear = 1990;
        private int baseStartMonth = 1;


        #region 树方式
        private Hashtable detailHashtable = new Hashtable();

        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";

        private List<string> listDtlIds = new List<string>();
        private int startImageCol = 1, endImageCol = 19;
        #endregion

        /// <summary>
        /// 当前单据
        /// </summary>
        public WeekScheduleMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VWeekSchedule(EnumExecScheduleType execScheduleType)
        {
            InitializeComponent();
            projectInfo = StaticMethod.GetProjectInfo();
            this.execScheduleType = execScheduleType;
            InitComboBox(EnumExecScheduleType.总体进度计划);

            InitEvent();
            InitData();
        }

        private void InitComboBox(EnumExecScheduleType frontPlanType)
        {
            #region 计划类型

            if (cbPlanType.Items.Count == 0)
            {
                foreach (string planType in Enum.GetNames(typeof(EnumExecScheduleType)))
                {
                    if (planType != "周进度计划" && planType != "总体进度计划" && planType != "季度进度计划")
                    {
                        cbPlanType.Items.Add(planType);
                    }
                }
            }
            #endregion 

            #region 前驱进度计划
            if (projectInfo == null)
            {
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ExecScheduleType", frontPlanType));

            //oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute)); //审核过的 状态为执行中
            oq.AddOrder(Order.Desc("CreateDate"));

            var oqResult = model.ObjectQuery(typeof(WeekScheduleMaster), oq);
            if (oqResult != null && oqResult.Count > 0)
            {
                cboSchedulePlanName.DataSource = oqResult.OfType<WeekScheduleMaster>().ToList();
                cboSchedulePlanName.DisplayMember = "PlanName";
                cboSchedulePlanName.ValueMember = "Id";

            }
            else
            {
                cboSchedulePlanName.DataSource = null;
            }
            #endregion

        }

        public void InitData()
        {
            InitFlexGrid(1);


            List<int> year = new List<int>();
            for (int i = baseYear; i < baseYear + 100; i++)
            {
                year.Add(i);
            }
            this.cmoYear.DataSource = year;
            this.cmoYear.Text = ConstObject.LoginDate.Year.ToString();
            List<int> startMonth = new List<int>();
            for (int i = baseStartMonth; i < baseStartMonth + 12; i++)
            {
                startMonth.Add(i);
            }
            this.cmoStartMonth.DataSource = startMonth;
            //this.cmoStartMonth.Text = (ClientUtil.ToInt(cmoEndMonth.Text) - 1).ToString();
            this.cmoStartMonth.Text = ConstObject.LoginDate.Month.ToString();



            //缺省计划时间段从基础数据中取

            int startDate = 21;
            int endDate = 20;

            IList listDateArea = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_MonthScheduleDefaultDateArea);

            if (listDateArea != null && listDateArea.Count > 0)
            {
                BasicDataOptr basicData = listDateArea[0] as BasicDataOptr;
                if (basicData != null)
                {
                    try
                    {
                        startDate = Convert.ToInt32(basicData.BasicCode);
                        endDate = Convert.ToInt32(basicData.BasicName);
                    }
                    catch { }
                }
            }

        }

        private void InitEvent()
        {

            btnProductSchedule.Click += new EventHandler(btnProductSchedule_Click);

            dtpDateBegin.ValueChanged += new EventHandler(dtpDateBegin_ValueChanged);
            dtpDateEnd.ValueChanged += new EventHandler(dtpDateEnd_ValueChanged);

      
            //右键复制菜单
            tsmiCopy.Click += new EventHandler(tsmiCopy_Click);
            tsmiDgOtherDel.Click += new EventHandler(tsmiDgOtherDel_Click);

            cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);

            btnExportToMPP.Click += new EventHandler(btnExportToMPP_Click);
            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            flexGrid.LeaveCell += new Grid.LeaveCellEventHandler(flexGrid_LeaveCell);

            //会计期间【月】
            this.cmoStartMonth.SelectedIndexChanged += new EventHandler(cmoStartMonth_SelectedIndexChanged);


        }

        //会计期间月
        void cmoStartMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshAccountList();
        }

        private void RefreshAccountList()
        {
            //获取当前会计期的最后一天
            string startDate = stockModel.StockInOutSrv.GetStartDateByFiscalPeriod(ClientUtil.ToInt(this.cmoYear.Text), ClientUtil.ToInt(this.cmoStartMonth.Text));
            string endDate = stockModel.StockInOutSrv.GetEndDateByFiscalPeriod(ClientUtil.ToInt(this.cmoYear.Text), ClientUtil.ToInt(this.cmoStartMonth.Text));
            this.dtpDateBegin.Value = ClientUtil.ToDateTime(startDate);
            this.dtpDateEnd.Value = ClientUtil.ToDateTime(endDate);
        }

        void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearView();
            this.execScheduleType = VirtualMachine.Component.Util.EnumUtil<EnumExecScheduleType>.FromDescription(cbPlanType.Text.Trim());

            switch (cbPlanType.Text)
            {
                case "季度进度计划":
                    //customLabel7.Text = "年度进度计划:";
                    btnProductSchedule.Text = "生成季度进度计划";
                    break;
                case "月度进度计划":
                    //customLabel7.Text = "年度进度计划:";
                    btnProductSchedule.Text = "生成月度进度计划";
                    break;

                case "年度进度计划":
                    //customLabel7.Text = "总进度计划:";
                    btnProductSchedule.Text = "生成年度进度计划";
                    break;
                default:
                    break;
            }

            InitComboBox(EnumExecScheduleType.总体进度计划);
            
        }
            

        void tsmiCopy_Click(object sender, EventArgs e)
        {
           

        }

        void tsmiDgOtherDel_Click(object sender, EventArgs e)
        {
          
        }

        void dtpDateEnd_ValueChanged(object sender, EventArgs e)
        {

            if (dtpDateEnd.Value.Date < dtpDateBegin.Value.Date)
            {
                dtpDateBegin.Value = dtpDateEnd.Value.AddMonths(-1).AddDays(1);
            }

        }

        void dtpDateBegin_ValueChanged(object sender, EventArgs e)
        {

            if (dtpDateEnd.Value.Date < dtpDateBegin.Value.Date)
            {
                dtpDateEnd.Value = dtpDateBegin.Value.AddMonths(1).AddDays(-1);
            }

        }

        void btnProductSchedule_Click(object sender, EventArgs e)
        {
            if (this.txtPlanName.Text.Trim() == "")
            {
                MessageBox.Show("请先指定 " + cbPlanType.Text.Trim() + "的“计划名称”！");
                this.txtPlanName.Focus();
                return;    
            }
            //NewWeekPlan();
            string newVer = this.txtPlanName.Text.Trim();
            WeekScheduleMaster weekmaster = cboSchedulePlanName.SelectedItem as WeekScheduleMaster;

            CreateNew(newVer, weekmaster);
            GridFillFlex();

           //RefreshState(MainViewState.Browser);

        }

       

        private void CreateNew(string newVer, WeekScheduleMaster monthPlan)
        {
            monthPlan = model.ProductionManagementSrv.GetWeekScheduleMasterById(monthPlan.Id);
            if (monthPlan == null)
            {
                return;
            }

            FlashScreen.Show("正在生成"+this.cbPlanType.Text+"，请稍候...");

            var bgDate = dtpDateBegin.Value.Date;
            var endDate = dtpDateEnd.Value.Date;

     

            var details = from d in monthPlan.Details
                          where d.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode
                                &&d.State==DocumentState.InExecute
                                && ((d.PlannedBeginDate <= bgDate && d.PlannedEndDate >= bgDate)
                                || (d.PlannedBeginDate <= endDate && d.PlannedEndDate >= endDate)
                                || (d.PlannedBeginDate > bgDate && d.PlannedEndDate < endDate))
                          select d;

            var pNodes = new List<WeekScheduleDetail>();
            foreach (var dt in details)
            {
                var pNode = dt.ParentNode;
                while (pNode != null)
                {
                    if (!pNodes.Contains(pNode))
                    {
                        pNodes.Add(pNode);
                    }

                    pNode = pNode.ParentNode;
                }
            }

            pNodes.AddRange(details);

            var wkPlan = NewWeekPlan(monthPlan);
            wkPlan.PlanName = newVer;
            wkPlan.PlannedBeginDate = bgDate;
            wkPlan.PlannedEndDate = endDate;

            curBillMaster = model.ProductionManagementSrv.CreateSubSchdulePlan(wkPlan, pNodes);

            FlashScreen.Close();

        }

        private WeekScheduleMaster NewWeekPlan(WeekScheduleMaster monthPlan)
        {
            var wkPlan = new WeekScheduleMaster();
            wkPlan.ProjectId = projectInfo.Id;
            wkPlan.ProjectName = projectInfo.Name;
            wkPlan.ExecScheduleType = this.execScheduleType;
            wkPlan.CreateDate = model.ProductionManagementSrv.GetServerTime();
            wkPlan.HandlePerson = ConstObject.LoginPersonInfo;
            wkPlan.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            wkPlan.CreatePerson = ConstObject.LoginPersonInfo;
            wkPlan.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            wkPlan.HandleOrg = ConstObject.TheOperationOrg;
            wkPlan.HandlePersonSyscode = ConstObject.TheOperationOrg.SysCode;
            //wkPlan.DocState = monthPlan.DocState;

            wkPlan.DocState = monthPlan.DocState;
            wkPlan.ForwardBillCode = monthPlan.Code;
            wkPlan.ForwardBillId = monthPlan.Id;

            return wkPlan;
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
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Code", code));
                    oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));

                    curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterByCode(oq);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                   
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
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
                ObjectLock.Unlock(btnExportToMPP, true);
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
                InitFlexGrid(1);

                this.curBillMaster = new WeekScheduleMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.HandlePersonSyscode = ConstObject.TheOperationOrg.SysCode;
                curBillMaster.HandleOrg = ConstObject.TheOperationOrg;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;

                curBillMaster.SummaryStatus = EnumSummaryStatus.未汇总;
                curBillMaster.ExecScheduleType = this.execScheduleType;

                //制单人
                //txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                //txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //归属项目
                projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    //txtProject.Tag = projectInfo;
                    //txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }

                cbPlanType.Text = "月度进度计划";

               // curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);
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
            curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);
            if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();
                ModelToView();
                return true;
            }
            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能修改！");
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
                if (!ViewToModel())
                    return false;

                if (curBillMaster.Id == null)
                {
                    curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);
                    if (execScheduleType == EnumExecScheduleType.年度进度计划)
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "年度进度计划", "", curBillMaster.ProjectName);
                    }
                    else
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月进度计划", "", curBillMaster.ProjectName);
                    }
                }
                else
                {
                    curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);
                    if (execScheduleType == EnumExecScheduleType.年度进度计划)
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "年度进度计划", "", curBillMaster.ProjectName);
                    }
                    else
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月进度计划", "", curBillMaster.ProjectName);
                    }
                }

                //更新Caption
                this.ViewCaption = ViewName + "-" + curBillMaster.Code;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        public override bool SubmitView()
        {
            if (!ViewToModel())
                return false;

            if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    FlashScreen.Show("正在执行提交，请稍候........");

                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster.SubmitDate = DateTime.Now;
                    LogData log = new LogData();
                    log.BillType = execScheduleType.ToString() + "单";
                    if (string.IsNullOrEmpty(curBillMaster.Id))
                        log.OperType = "新增提交";
                    else
                        log.OperType = "修改提交";

                    curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);

                    log.BillId = curBillMaster.Id;
                    log.Code = curBillMaster.Code;
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    //更新Caption
                    this.ViewCaption = ViewName + "-" + curBillMaster.Code;

                    return true;
                }
                catch (Exception e)
                {
                    FlashScreen.Close();
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
                finally
                {
                    FlashScreen.Close();
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
                string msg = "";
                if (!IsHaveFollowBill(out msg))
                {

                    curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);

                    if (!model.ProductionManagementSrv.DeleteByDao(curBillMaster))
                        return false;

                    string ss = "月度进度计划";
                    if (this.execScheduleType == EnumExecScheduleType.年度进度计划)
                        ss = "年度进度计划";
                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, ss, "", curBillMaster.ProjectName);


                    ClearView();
                    flexGrid.Rows = 1;
                    return true;
                }
                else
                {
                    MessageBox.Show(msg + "不能删除！");
                    return false;
                }

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
                        curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);
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
                curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);
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
            if (txtPlanName.Text.Trim() == "")
            {
                MessageBox.Show("计划名称不能为空！");
                txtPlanName.Focus();
                return false;
            }

           if (execScheduleType == EnumExecScheduleType.月度进度计划)
            {
                
            }

            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView())
                return false;
            try
            {
                curBillMaster.ForwardBillCode = txtProductSchedule.Text;

                curBillMaster.PlanName = txtPlanName.Text.Trim();
                curBillMaster.PlannedBeginDate = dtpDateBegin.Value.Date;
                curBillMaster.PlannedEndDate = dtpDateEnd.Value.Date;
                curBillMaster.Descript = txtRemark.Text;
                //curBillMaster.CreateDate = dtpBusinessDate.Value.Date;
                curBillMaster.AccountYear = ClientUtil.ToInt(cmoYear.Text);
                curBillMaster.AccountMonth = ClientUtil.ToInt(cmoStartMonth.Text);

                if (this.execScheduleType == EnumExecScheduleType.周进度计划)
                {
                    curBillMaster.ExecScheduleType = this.execScheduleType;
                }
                else
                {
                    if (cbPlanType.Text == "月度进度计划")
                    {
                        execScheduleType = EnumExecScheduleType.月度进度计划;
                        curBillMaster.ExecScheduleType = this.execScheduleType;
                    }
                    else if (cbPlanType.Text == "季度进度计划")
                    {
                        execScheduleType = EnumExecScheduleType.季度进度计划;
                        curBillMaster.ExecScheduleType = this.execScheduleType;
                    }
                    else
                    {
                        execScheduleType = EnumExecScheduleType.总体进度计划;
                        curBillMaster.ExecScheduleType = this.execScheduleType;
                    }
                }


                ViewToDetails();
                curBillMaster = model.ProductionManagementSrv.SaveOrUpdateByDao(curBillMaster) as WeekScheduleMaster;


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
                this.cbPlanType.SelectedItem = ClientUtil.ToString(curBillMaster.ExecScheduleType);

                txtProductSchedule.Text = curBillMaster.ForwardBillCode;

                dtpDateBegin.Value = curBillMaster.PlannedBeginDate;
                dtpDateEnd.Value = curBillMaster.PlannedEndDate;

                txtPlanName.Text = curBillMaster.PlanName;
                txtRemark.Text = curBillMaster.Descript;

                cmoYear.Text = curBillMaster.AccountYear.ToString();
                cmoStartMonth.Text = curBillMaster.AccountMonth.ToString();

                GridFillFlex();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 是否存在后续单据
        /// </summary>
        /// <param name="msg">返回的字符串</param>
        /// <returns></returns>
        private bool IsHaveFollowBill(out string msg)
        {
            bool rtn_bool = false;
            StringBuilder sb = new StringBuilder("该单据存在以下后续单：\n");
        
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            oq.AddCriterion(Expression.Eq("ForwardBillId", curBillMaster.Id)); //审核过的 状态为执行中

            var oqResult = model.ObjectQuery(typeof(WeekScheduleMaster), oq);
            if (oqResult != null && oqResult.Count > 0)
            {
                rtn_bool = true;
                for (int i = 0; i < oqResult.Count; i++)
                {
                    WeekScheduleMaster wsm = oqResult[i] as WeekScheduleMaster;
                    switch (wsm.ExecScheduleType)
                    {
                        case EnumExecScheduleType.周进度计划:
                            sb.Append("周进度计划【");
                            sb.Append(wsm.PlanName);
                            sb.Append("】");
                            sb.Append("\n");
                            break;
                        case EnumExecScheduleType.月度进度计划:
                            sb.Append("月度进度计划【");
                            sb.Append(wsm.PlanName);
                            sb.Append("】");
                            sb.Append("\n");
                            break;
                        case EnumExecScheduleType.季度进度计划:
                            sb.Append("季度进度计划【");
                            sb.Append(wsm.PlanName);
                            sb.Append("】");
                            sb.Append("\n");
                            break;
                        case EnumExecScheduleType.年度进度计划:
                            sb.Append("年度进度计划【");
                            sb.Append(wsm.PlanName);
                            sb.Append("】");
                            sb.Append("\n");
                            break;
                        case EnumExecScheduleType.总体进度计划:
                            sb.Append("总体进度计划【");
                            sb.Append(wsm.PlanName);
                            sb.Append("】");
                            sb.Append("\n");
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            msg = sb.ToString();
            return rtn_bool;

        }

        #region 打印处理
        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"施工生产周进度计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"施工生产周进度计划.flx") == false) return false;
        //FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"施工生产周进度计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("施工生产周进度计划【" + curBillMaster.Code + "】", false, false, true);
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

        //private void FillFlex(WeekScheduleMaster billMaster)
        //{
        //    int detailStartRowNumber = 4;//7为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;
        //    CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
        //    //主表数据
        //    flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
        //    flexGrid1.Cell(3, 3).Text = billMaster.CreateDate.Year.ToString() +"年"+ billMaster.CreateDate.Month.ToString() +"月";
        //    //flexGrid1.Cell(3, 4).Text = billMaster.Code;
        //    flexGrid1.Cell(3, 6).Text = billMaster.CreateDate.ToShortDateString();

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        WeekScheduleDetail detail = (WeekScheduleDetail)billMaster.Details.ElementAt(i);
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = ClientUtil.ToString(i + 1);
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MainTaskContent;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.PlannedBeginDate.ToShortDateString();
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.PlannedEndDate.ToShortDateString();
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.ForwardBillMasterOwner;
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Row(detailStartRowNumber + i).AutoFit();
        //    }
        //}
        #endregion


        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = true;

            flexGrid.Rows = rows;
            flexGrid.Cols = 28;//其中0列隐藏 1-19 为放置图片列 20-24为数据列

            flexGrid.SelectionMode = SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FocusRectEnum.Solid;
            flexGrid.BorderStyle = BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = ScrollBarsEnum.Both;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);
            flexGrid.DisplayRowArrow = true;
            flexGrid.DisplayRowNumber = true;

            Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }
            flexGrid.Column(endImageCol + 5).Width = 300;//计划说明

            for (int i = 0; i < rows; i++)
            {
                range = flexGrid.Range(i, startImageCol, i, endImageCol);
                range.Merge();

                flexGrid.Cell(i, endImageCol + 3).Text = "天";
            }

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = AlignmentEnum.CenterCenter;

            flexGrid.Cell(0, endImageCol + 1).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 2).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "计划工期";//22
            flexGrid.Cell(0, endImageCol + 4).Text = "工期计量单位";//23
            flexGrid.Cell(0, endImageCol + 5).Text = "计划说明";//27
            flexGrid.Cell(0, endImageCol + 6).Text = "实际开始时间";//24
            flexGrid.Cell(0, endImageCol + 7).Text = "实际结束时间";//25
            flexGrid.Cell(0, endImageCol + 8).Text = "实际工期";//26

            flexGrid.Column(endImageCol + 1).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).Mask = MaskEnum.Digital;
            flexGrid.Column(endImageCol + 6).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 7).CellType = CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 8).Mask = MaskEnum.Digital;

            flexGrid.Column(endImageCol + 6).Locked = true;
            flexGrid.Column(endImageCol + 7).Locked = true;
            flexGrid.Column(endImageCol + 8).Locked = true;


            //暂时先隐藏 实际工期相关的三列
            flexGrid.Column(endImageCol + 6).Visible = false;
            flexGrid.Column(endImageCol + 7).Visible = false;
            flexGrid.Column(endImageCol + 8).Visible = false;

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private IList ViewToDetails()
        {
            IList list = new ArrayList();

            if (!string.IsNullOrEmpty(curBillMaster.Id))
            {
                for (int i = 1; i < flexGrid.Rows; i++)
                {
                    string detailId = flexGrid.Cell(i, 0).Tag;

                    if (detailId == null || detailId.Equals(""))
                        continue;

                    WeekScheduleDetail detail = null;
                    foreach (WeekScheduleDetail tempDetail in curBillMaster.Details)
                    {
                        if (detailId == tempDetail.Id)
                        {
                            detail = tempDetail;
                            break;
                        }

                    }
                    //计划开始时间
                    string PlannedBeginDateStr = flexGrid.Cell(i, endImageCol + 1).Text;
                    if (PlannedBeginDateStr != null && !PlannedBeginDateStr.Equals(""))
                    {
                        detail.PlannedBeginDate = DateTime.Parse(PlannedBeginDateStr);
                    }
                    else
                    {
                        detail.PlannedBeginDate = new DateTime(1900, 1, 1);
                    }
                    //计划结束时间
                    string PlannedEndDateStr = flexGrid.Cell(i, endImageCol + 2).Text;
                    if (PlannedEndDateStr != null && !PlannedEndDateStr.Equals(""))
                    {
                        detail.PlannedEndDate = DateTime.Parse(PlannedEndDateStr);
                    }
                    else
                    {
                        detail.PlannedEndDate = new DateTime(1900, 1, 1);
                    }
                    //计划工期
                    detail.PlannedDuration = flexGrid.Cell(i, endImageCol + 3).IntegerValue;

                    //计划说明
                    detail.ScheduleUnit = flexGrid.Cell(i, endImageCol + 4).Text; ;

                    //计划说明
                    detail.Descript = flexGrid.Cell(i, endImageCol + 5).Text;
                    CurBillMaster.AddDetail(detail);

                }
            }
            return list;
        }

        void btnExportToMPP_Click(object sender, EventArgs e)
        {

            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要导出的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //OpenFileDialog ofg = new OpenFileDialog();
            SaveFileDialog sfd = new SaveFileDialog();
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.Filter = "项目 (*.MPP)|*.MPP";
            sfd.RestoreDirectory = true;
            sfd.FileName = CurBillMaster.PlanName + "_" + string.Format("{0:yyyy年MM月dd日HH点mm分}", DateTime.Now) + ".mpp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    IList list = model.ProductionManagementSrv.GetWeekChilds(CurBillMaster);
                    MSProjectUtil.UpdateProject(fileName, list, listDtlIds, this.Handle);
                }
                else
                {

                    IList list = model.ProductionManagementSrv.GetWeekChilds(CurBillMaster);//这是全部树
                    CreateMPP(fileName, list, listDtlIds, this.Handle);

                }
            }
        }

    

      

        private void GridFillFlex()
        {
            detailHashtable.Clear();

            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 3).Locked = true;

            IList list = null;
            if (CurBillMaster != null && !string.IsNullOrEmpty(CurBillMaster.Id))
            {
                list = model.ProductionManagementSrv.GetWeekChilds(CurBillMaster);
            }

            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                listDtlIds.Clear();
                CurBillMaster.Details.Clear();
                foreach (WeekScheduleDetail detail in list)
                {
                    detail.RowIndex = rowIndex;

                    listDtlIds.Add(detail.Id);
                    detailHashtable.Add(detail.Id, detail);
                    CurBillMaster.Details.Add(detail);

                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                    if (detail.ChildCount > 0)
                    {
                        flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    }

                    SetRowBold(rowIndex, detail.ChildCount > 0);

                    Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();

                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = detail.GWBSTreeName ?? projectInfo.Name + "总进度计划";

                    //行是否只读
                    flexGrid.Row(rowIndex).Locked = detail.ChildCount > 0 || detail.State != DocumentState.Edit;

                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }

                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                    }

                    //计划工期
                    if (detail.PlannedBeginDate != (new DateTime(1900, 1, 1)) && detail.PlannedEndDate != (new DateTime(1900, 1, 1)) && detail.PlannedBeginDate <= detail.PlannedEndDate)
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = ((detail.PlannedEndDate - detail.PlannedBeginDate).Days + 1).ToString();
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = "";
                    }
                    flexGrid.Cell(rowIndex, endImageCol + 3).Alignment = AlignmentEnum.CenterCenter;

                    //工期计量单位                    
                    flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.ScheduleUnit;
                    flexGrid.Cell(rowIndex, endImageCol + 4).Alignment = AlignmentEnum.CenterCenter;

                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = detail.Descript; //"计划说明";//27
                    flexGrid.Cell(rowIndex, endImageCol + 5).Locked = false;

                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.ActualBeginDate.ToShortDateString();
                    }

                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualEndDate.ToShortDateString();
                    }

                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();

                    rowIndex = rowIndex + 1;
                }
            }
            //1-19列的背景色
            FlexCell.Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }
            //设置计划实际信息列的背景色
            range = flexGrid.Range(1, endImageCol + 6, flexGrid.Rows - 1, endImageCol + 8);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void SetRowBold(int rowIndex, bool isBold)
        {
            for (var i = 0; i < flexGrid.Cols; i++)
            {
                flexGrid.Cell(rowIndex, i).FontBold = isBold;
            }
        }

        public bool IsChild(string sChildSysCode, string sParentSysCode,int i)
        {
            bool bFlag = false;
            if (sChildSysCode.IndexOf(sParentSysCode) > -1)
            {
                string s = sChildSysCode.Replace(sParentSysCode, "");
                if (s.Split('.').Length == i)
                {
                    bFlag = true;
                }
            }
            return bFlag;
        }

        public static void CreateMPP(string fileName, IList scheduleList, List<string> lstID, IntPtr Handle)
        {

            ExportTask(scheduleList, lstID, fileName, Handle);

        }

        [DllImport("shell32.dll")]
        public extern static IntPtr ShellExecute(IntPtr hwnd,
                                                 string lpOperation,
                                                 string lpFile,
                                                 string lpParameters,
                                                 string lpDirectory,
                                                 int nShowCmd
                                                );

        public static void ExportTask(IList scheduleList, List<string> lstID, string sFilePath, IntPtr Handle)
        {
            object missing = Type.Missing;
            string sXmlFilePath = @"d:\\temp\\1.xml";
            string sID = string.Empty;
            string Level = string.Empty;
            int iCount = 0;
            string sNewTag = "(最新)";
            try
            {
                //获取xml路径
                if (sFilePath.LastIndexOf(".") > 0)
                {
                    sXmlFilePath = sFilePath.Substring(0, sFilePath.LastIndexOf(".")) + ".xml";
                }
                else
                {
                    sXmlFilePath = sFilePath + ".xml";
                }
                if (File.Exists(sXmlFilePath))//删除原有的xml文件
                {
                    File.Delete(sXmlFilePath);
                }
                if (scheduleList != null && scheduleList.Count > 0)
                {
                    #region 创建task集合
                    //string sMsg = string.Empty;
                    //for (int i = 0; i < 5; i++)
                    //{
                    //    sMsg += "1111111111111111111111111111111111111111";
                    //}
                    Project2007.ProjectTask[] arrTask = new Project2007.ProjectTask[scheduleList.Count];
                    string sParentID = string.Empty;
                    foreach (WeekScheduleDetail detail in scheduleList)
                    {
                        if (lstID.Contains(detail.Id))
                        {
                            Project2007.ProjectTask task = new Project2007.ProjectTask();
                            task.Name = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName + sNewTag;
                            task.Start111 = detail.PlannedBeginDate;
                            task.Finish111 = detail.PlannedEndDate;
                            task.ID = detail.Id;
                            task.OutlineLevel = detail.GWBSTree.Level.ToString();
                            sParentID = detail.GWBSTree.ParentNode == null ? "" : detail.GWBSTree.ParentNode.Id;
                            task.Notes = sParentID + "|" + string.Format(task.Start111.ToString("G")) + "|" + string.Format(task.Finish111.ToString("G")); ;
                            // task.Notes = sParentID + "|" + "" + "|" + ""; 
                            task.Contact = detail.GWBSTreeName == null ? "root" : detail.Id;
                            task.WBS = detail.GWBSTreeName == null ? "1" : "2";
                            //task.UID = detail.Id;
                            arrTask[iCount++] = task;

                        }
                    }
                    #endregion
                    #region 创建一个project2007的类
                    Project2007.Project p = new Project2007.Project();
                    p.Tasks = arrTask;
                    XmlSerializer xs = new XmlSerializer(typeof(Project2007.Project));

                    Stream stream = new FileStream(sXmlFilePath, FileMode.OpenOrCreate);
                    xs.Serialize(stream, p);//序列化
                    stream.Close();
                    #endregion
                    #region 创建project
                    Microsoft.Office.Interop.MSProject.Application appProject = null;
                    //try
                    //{
                    //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    //}
                    //catch {
                    //    ShellExecute(Handle, "open", @"C:\Program Files\Microsoft Office\Office12\WINPROJ.EXE", null, null, (int)ShowWindowCommands.SW_SHOW);

                    //    Thread.Sleep(3000);
                    //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    //}
                    ShellExecute(Handle, "open", ProjectPath.GetPath(), null, null, (int)Application.Business.Erp.SupplyChain.Client.Util.MSProjectUtil.ShowWindowCommands.SW_SHOW);

                    Thread.Sleep(3000);
                    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    appProject.Visible = true;
                    appProject.FileNew(missing, missing, missing, false);//新建一个project
                    appProject.FileOpen(sXmlFilePath, true, Microsoft.Office.Interop.MSProject.PjMergeType.pjAppend, missing, missing, missing, missing, missing, missing, "MSProject.mpp", missing, Microsoft.Office.Interop.MSProject.PjPoolOpen.pjPoolReadWrite, missing, missing, false, missing);//建数据加载到文件中

                    ///
                    ///  appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml1.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjXLS, missing, missing, missing, missing, missing, missing, missing, "MSProject.xml", missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    // appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml2.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjTXT , missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    ///
                    appProject.FileSaveAs(sFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//保存
                    #endregion
                    if (File.Exists(sXmlFilePath))//删除原有的xml文件
                    {
                        File.Delete(sXmlFilePath);
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
        }

        void flexGrid_Click(object Sender, EventArgs e)
        {
            flexGrid.AutoRedraw = false;

            if (flexGrid.ActiveCell.ImageKey == imageExpand)
            {
                flexGrid.ActiveCell.SetImage(imageCollapse);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, true);
                }
            }
            else if (flexGrid.ActiveCell.ImageKey == imageCollapse)
            {
                flexGrid.ActiveCell.SetImage(imageExpand);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, false);
                }
            }

            flexGrid.AutoRedraw = true;
        }

        private int CountAllChildNodes(string dtlId)
        {
            var query = from d in curBillMaster.Details
                        where d.Id == dtlId
                        select d;
            WeekScheduleDetail thePlanDtl = query.ElementAt(0);

            var queryChilds = from d in curBillMaster.Details
                              where d.Id != dtlId && d.GWBSTreeSysCode.IndexOf(thePlanDtl.GWBSTreeSysCode) > -1
                              select d;

            return queryChilds.Count();
        }

        private void SetRowVisible(int row1, int row2, bool value)
        {
            flexGrid.AutoRedraw = false;
            for (int i = row1; i <= row2; i++)
            {
                flexGrid.Row(i).Visible = value;
                if (value)
                {
                    for (int j = startImageCol; j <= endImageCol; j++)
                    {
                        if (flexGrid.Cell(i, j).ImageKey != null && !flexGrid.Cell(i, j).ImageKey.Equals(""))
                        {
                            flexGrid.Cell(i, j).SetImage(imageCollapse);
                            break;
                        }
                    }
                }
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        public void flexGrid_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if (e.Row > 0 && e.Col > 19 && e.Col < 24)
            {
                string sStartTime = flexGrid.Cell(e.Row, endImageCol + 1).Text;
                string sFinishTime = flexGrid.Cell(e.Row, endImageCol + 2).Text;
                if (!string.IsNullOrEmpty(sStartTime) && !string.IsNullOrEmpty(sFinishTime))
                {
                    try
                    {
                        DateTime StartTime = DateTime.Parse(sStartTime);
                        DateTime FinnishTime = DateTime.Parse(sFinishTime);
                        if (StartTime <= FinnishTime)
                        {
                            flexGrid.Cell(e.Row, endImageCol + 3).Text = ((FinnishTime - StartTime).Days + 1).ToString();
                        }
                        else
                        {
                            //flexGrid.Cell(e.Row, endImageCol + 3).Text = string.Empty;
                            MessageBox.Show("计划开始时间大于计划结束时间！");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
