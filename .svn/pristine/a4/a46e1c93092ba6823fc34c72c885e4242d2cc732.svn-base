using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan;
using Application.Business.Erp.SupplyChain.Client.TotalDemandPlan;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VResourcesDemandPlanManagement : TMasterDetailView
    {
        MRollingDemandPlan model = new MRollingDemandPlan();
        private CurrentProjectInfo projectInfo = null;
        private PersonInfo loginPerson = null;

        private GWBSTree wbs = null;

        private ResourceRequireReceipt curBillMaster;

        ///<summary>
        ///
        ///</summary>
        public ResourceRequireReceipt CurBillMaster
        {
            set { this.curBillMaster = value; }
            get { return this.curBillMaster; }  
        }

        public VResourcesDemandPlanManagement()
        {
            InitializeComponent();
            InitData();
        }

        void InitData()
        {
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            loginPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
            InitComboBox();
            GetGWBS();

            InitEvents();
        }

        void InitComboBox()
        {
            //计划类型
            foreach (string  item in Enum.GetNames(typeof(PlanType)))
            {
                cmbPlanType.Items.Add(item);
            }

            //资源类型
            foreach (string  item in Enum.GetNames(typeof(ResourceTpye)))
            {
                cbResourceType.Items.Add(item);
            }

            //年月 年
            List<int> year = new List<int>();
            for (int i = DateTime.Now.Year - 30; i < DateTime.Now.Year + 10; i++)
            {
                year.Add(i);
            }
            this.cmoYear.DataSource = year;
            this.cmoYear.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Year.ToString();
         

            //年月 月
            List<int> startMonth = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                startMonth.Add(i);
            }
            this.cmoStartMonth.DataSource = startMonth;
            this.cmoStartMonth.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Month.ToString();
            // 状态
            foreach (string state in Enum.GetNames(typeof(ResourceRequirePlanState)))
            {
                cmbState.Items.Add(state);
            }

        }

       //选取工程任务分解结构的节点
        private void GetGWBS()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

            //项目ID并不是根节点 而是枝节点 用根节点属性过滤不到
            //oq.AddCriterion(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode));

            //改为节点名称 ==项目名称 去过滤
            oq.AddCriterion(Expression.Eq("Name", projectInfo.Name));
            oq.AddCriterion(Expression.Eq("State", 1));

            IList listGWGS = model.ObjectQuery(typeof(GWBSTree), oq);
            if (listGWGS != null && listGWGS.Count > 0)
            {
                wbs = listGWGS[0] as GWBSTree;
            }
        }

        void InitEvents()
        {
            this.cmbPlanType.SelectedIndexChanged += new EventHandler(cmbPlanType_SelectedIndexChanged);
            this.cmoStartMonth.SelectedIndexChanged += new EventHandler(cmoStartMonth_SelectedIndexChanged);
            this.btnGetData.Click += new EventHandler(btnGetData_Click);
            this.gridResourceRequireDetail.CellEndEdit +=new DataGridViewCellEventHandler(gridResourceRequireDetail_CellEndEdit);
            
        }

        //加载明细
        void ShowDetail()
        {
            IList rrpDtlList1 = model.GetBudgetResourcesDemand(curBillMaster, projectInfo, wbs, ClientUtil.ToDateTime(this.dtMadeStartDateQuery.Value), ClientUtil.ToDateTime(this.dtMadeEndDateQuery.Value), EnumUtil<ResourceTpye>.FromDescription(this.cbResourceType.Text), EnumUtil<PlanType>.FromDescription(this.cmbPlanType.Text));
            IList rrpDtlList = new ArrayList();
            if (rrpDtlList1 != null && rrpDtlList1.Count >= 0)
            {
                foreach (ResourceRequireReceiptDetail d in rrpDtlList1)
                {
                    if (d.PlannedCostQuantity != 0)
                    {
                        rrpDtlList.Add(d);
                    }
                }
            }


            gridResourceRequireDetail.Rows.Clear();
            if (rrpDtlList != null && rrpDtlList.Count > 0)
            {

                List<ResourceRequireReceiptDetail> list = rrpDtlList.OfType<ResourceRequireReceiptDetail>().ToList<ResourceRequireReceiptDetail>();
                var quey = from q in list
                           orderby q.TheGWBSSysCode ascending
                           select q;

                foreach (ResourceRequireReceiptDetail rrpDtl in quey)
                {
                    AddGridResourceRequireDetail(rrpDtl,false);
                }
            }

        }

        void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (wbs == null)
                {
                    MessageBox.Show("获取工程任务分解结构失败！");
                    return;
                }

                if (this.cbResourceType.Text == "")
                {
                    MessageBox.Show("请选择资源类型！");
                    return;
                }


                FlashScreen.Show("正在获取预算资源需求量......");
                //CreateNew(newVer, weekmaster);

                DateTime dt_SelectBegin = ClientUtil.ToDateTime(this.dtMadeStartDateQuery.Value);
                DateTime dt_SelectEnd = ClientUtil.ToDateTime(this.dtMadeEndDateQuery.Value);
                IList rrpDtlList1 = model.GetBudgetResourcesDemand(curBillMaster, projectInfo, wbs, dt_SelectBegin, dt_SelectEnd, EnumUtil<ResourceTpye>.FromDescription(this.cbResourceType.Text), EnumUtil<PlanType>.FromDescription(this.cmbPlanType.Text));
                IList rrpDtlList = new ArrayList();
                if (rrpDtlList1 != null && rrpDtlList1.Count >= 0)
                {
                    foreach (ResourceRequireReceiptDetail d in rrpDtlList1)
                    {
                        if (d.PlannedCostQuantity != 0)
                        {
                            rrpDtlList.Add(d);
                        }
                    }
                }
               
                #region  更新资源需求量
                    string messages = "";
                    foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                    {
                        bool flag = false;//旧数据里某资源是否存在新获取的数据里
                        bool isRemove = false;//是否把新集合里的对象移除
                        ResourceRequireReceiptDetail removeDtl = new ResourceRequireReceiptDetail();
                        ResourceRequireReceiptDetail rrrDtlOld = row.Tag as ResourceRequireReceiptDetail;
                        for (int i = 0; i < rrpDtlList.Count; i++)
                        {
                            ResourceRequireReceiptDetail rrpDtlNew = rrpDtlList[i] as ResourceRequireReceiptDetail;
                            
                            removeDtl = rrpDtlNew;
                            //图号和需求类型相等
                            if (rrpDtlNew.DiagramNumber == rrrDtlOld.DiagramNumber && rrpDtlNew.RequireType == rrrDtlOld.RequireType)
                            {
                                bool flagIsEqual = false;
                                #region
                                if (rrpDtlNew.MaterialResource == null || rrrDtlOld.MaterialResource == null)
                                {
                                    if (rrpDtlNew.MaterialResource == null && rrrDtlOld.MaterialResource == null)
                                    {
                                        //新旧料号都为空
                                        flagIsEqual = true;
                                    }
                                }
                                else //新旧料号都不为空
                                {
                                    if (rrpDtlNew.MaterialResource.Id == rrrDtlOld.MaterialResource.Id)
                                    {
                                        flagIsEqual = true;
                                    }
                                }
                                #endregion 

                                #region
                                if (flagIsEqual)
                                {
                                    flag = true;

                                    
                                    if (rrpDtlNew.TheGWBSSysCode == rrrDtlOld.TheGWBSSysCode)
                                    {
                                        #region 修改数据
                                        if (rrrDtlOld.PlanInRequireQuantity == rrrDtlOld.PlannedCostQuantity)
                                        {
                                            if (rrrDtlOld.PlannedCostQuantity != rrpDtlNew.PlannedCostQuantity)
                                            {
                                                rrrDtlOld.PlanInRequireQuantity = rrpDtlNew.PlanInRequireQuantity;
                                                rrrDtlOld.PlannedCostQuantity = rrpDtlNew.PlannedCostQuantity;
                                                row.Cells[PlanTotalRequire.Name].Value = rrpDtlNew.PlanInRequireQuantity ;
                                                row.Cells[PlannedCostQuantity.Name].Value = rrpDtlNew.PlannedCostQuantity;
                                            }
                                        }
                                        else
                                        {
                                            if (rrrDtlOld.PlannedCostQuantity != rrpDtlNew.PlannedCostQuantity)
                                            {
                                                rrrDtlOld.PlannedCostQuantity = rrpDtlNew.PlannedCostQuantity;
                                                row.Cells[PlannedCostQuantity.Name].Value = rrpDtlNew.PlannedCostQuantity;
                                            }
                                            row.Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                                        }
                                        isRemove = true;
                                        break;
                                        #endregion
                                    }
                                    else if (rrpDtlNew.TheGWBSSysCode.Contains(rrrDtlOld.TheGWBSSysCode))
                                    {
                                        messages += "资源类型“" + rrrDtlOld.MaterialName + "“被删除且该在该核算节点“" + row.Cells[TaskPath.Name].Value.ToString() + "”的下属核算节点上提了相同的资源类型";
                                        isRemove = true;
                                        break;
                                    }
                                    else if (rrrDtlOld.TheGWBSSysCode.Contains(rrpDtlNew.TheGWBSSysCode))
                                    {
                                        messages += "在核算节点“" + row.Cells[TaskPath.Name].Value.ToString() + "“的父核算节点上提了相同资源；";
                                        isRemove = true;
                                        break;
                                    }
                                }
                                #endregion
                            }
                        }
                        if (!flag && rrrDtlOld.RequireType == PlanRequireType.计划内需求)
                        {
                            rrrDtlOld.PlannedCostQuantity = 0;
                            rrrDtlOld.State = ResourceRequireReceiptDetailState.执行完毕;
                            row.Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            row.Cells[State.Name].Value = rrrDtlOld.State;
                            row.Cells[PlannedCostQuantity.Name].Value = 0;
                        }
                        if (isRemove)
                        {
                            rrpDtlList.Remove(removeDtl);
                        }

                        row.Tag = rrrDtlOld;
                    }

                    //添加行至DataGrid
                    if (rrpDtlList.Count > 0)
                    {
                        foreach (ResourceRequireReceiptDetail dtl in rrpDtlList)
                        {
                            AddGridResourceRequireDetail(dtl,true);
                        }
                    }
                    if (messages != "")
                    {
                        FlashScreen.Close();
                        MessageBox.Show(messages, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    #endregion
                
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("获取失败：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        /// <summary>
        ///  获取检索时间段与标准值时间段 的交集 占标准时间段的比率
        /// </summary>
        /// <param name="dt_SelectBegin">检索开始时间</param>
        /// <param name="dt_SelectEnd">检索结束时间</param>
        /// <param name="dt_BaseBegin">标准值开始时间</param>
        /// <param name="dt_BaseEnd">标准值结束时间</param>
        /// <returns>检索时间段与标准时间段 的交集 占标准时间段的比率</returns>
        private decimal GetRateByTimeSpan(DateTime dt_SelectBegin, DateTime dt_SelectEnd, DateTime dt_BaseBegin, DateTime dt_BaseEnd)
        { 
            decimal dec_rtn = 0;
            if (DateTime.Compare(dt_SelectBegin, dt_SelectEnd) > 0 || DateTime.Compare(dt_BaseBegin, dt_BaseEnd) > 0)
                return dec_rtn;
            if (DateTime.Compare(dt_BaseBegin, dt_BaseEnd) == 0)
                return ClientUtil.ToDecimal(1);
            DateTime dt_Lower = DateTime.Compare(dt_SelectBegin, dt_BaseBegin) > 0 ? dt_SelectBegin : dt_BaseBegin;
            DateTime dt_Higher = DateTime.Compare(dt_SelectEnd, dt_BaseEnd) < 0 ? dt_SelectEnd : dt_BaseEnd;

            dec_rtn = ClientUtil.ToDecimal(dt_Higher.Subtract(dt_Lower).Days +1) / (dt_BaseEnd.Subtract(dt_BaseBegin).Days + 1);
            return dec_rtn;
        }

        //添加明细行
        void AddGridResourceRequireDetail(ResourceRequireReceiptDetail rrpDtl, bool isGetData)
        {
            int rowIndex = gridResourceRequireDetail.Rows.Add();
            DataGridViewRow row = gridResourceRequireDetail.Rows[rowIndex];
      
            //工程项目任务路径
            //row.Cells[TaskPath.Name].Value = rrpDtl.TheGWBSTaskGUID.FullPath;

            //资源类型
            row.Cells[ResourceType.Name].Value = rrpDtl.MaterialName;

            //规格型号
            row.Cells[MaterialSpec.Name].Value = rrpDtl.MaterialSpec;

            //图号
            row.Cells[DiagramNumber.Name].Value = rrpDtl.DiagramNumber;

            //计划类型
            row.Cells[PlanType.Name].Value = rrpDtl.RequireType;

            //计划成本量
            row.Cells[PlannedCostQuantity.Name].Value = rrpDtl.PlannedCostQuantity;

            

            //甲供需求量
            row.Cells[FirstOfferRequireQuantity.Name].Value = rrpDtl.FirstOfferRequireQuantity;

            //月度计划发布累积量
            //row.Cells[MonthPlanPublishQuantity.Name].Value = rrpDtl.MonthPlanPublishQuantity;

            //日常计划发布累积量
            row.Cells[DailyPlanPublishQuantity.Name].Value = rrpDtl.DailyPlanPublishQuantity;

            //已执行累积量
            //row.Cells[ExecutedQuantity.Name].Value = rrpDtl.ExecutedQuantity;

            //数量计量单位
            row.Cells[QuantityUnitName.Name].Value = rrpDtl.QuantityUnitName;

            //状态
            row.Cells[State.Name].Value = rrpDtl.State;

            //备注
            row.Cells[Descript.Name].Value = rrpDtl.Descript;

            //技术参数
            row.Cells[TechnicalParameters.Name].Value = rrpDtl.TechnicalParameters;

            //质量标准
            row.Cells[colQualityStandards.Name].Value = rrpDtl.QualityStandards;

            Decimal planTotalRequire = 0;
            if (rrpDtl.RequireType == PlanRequireType.计划内需求)
            {
                //计划需求量
                row.Cells[PlanTotalRequire.Name].Value = rrpDtl.PlanInRequireQuantity;
                planTotalRequire = rrpDtl.PlanInRequireQuantity;

            }
            else
            {
                //计划需求量
                row.Cells[PlanTotalRequire.Name].Value = rrpDtl.PlanOutRequireQuantity;
                planTotalRequire = rrpDtl.PlanOutRequireQuantity;
            }
            if (rrpDtl.PlannedCostQuantity != planTotalRequire)
            {
                row.Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            }
            decimal rate = 0;
            DateTime dt_SelectBegin = ClientUtil.ToDateTime(this.dtMadeStartDateQuery.Value);
            DateTime dt_SelectEnd = ClientUtil.ToDateTime(this.dtMadeEndDateQuery.Value);
            if (curBillMaster.StagePlanType == Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain.PlanType.总体计划 || this.cmbPlanType.Text == "总体计划")
                rate = 1;
            else
                rate = GetRateByTimeSpan(dt_SelectBegin, dt_SelectEnd, ClientUtil.ToDateTime(rrpDtl.TheGWBSTaskGUID.TaskPlanStartTime), ClientUtil.ToDateTime(rrpDtl.TheGWBSTaskGUID.TaskPlanEndTime));
            //期间需求量
            row.Cells[PeriodQuantity.Name].Value = isGetData ? ClientUtil.ToDecimal((planTotalRequire * rate).ToString("#0.0000")) : rrpDtl.PeriodQuantity;

            //专业计划发布累积量
            row.Cells[SupplyPlanPublishQuantity.Name].Value = rrpDtl.SupplyPlanPublishQuantity;
            row.Tag = rrpDtl;
        }

        //月份  ===> 开始日期，结束日期
        void cmoStartMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = ClientUtil.ToInt(this.cmoYear.Text);
            int month = ClientUtil.ToInt(this.cmoStartMonth.Text);
        
            
            DateTime dtBegin = new DateTime(year,month,1);

            DateTime dtEnd = dtBegin.AddMonths(1).AddDays(-1);

            dtMadeStartDateQuery.Value = dtBegin;
            dtMadeEndDateQuery.Value = dtEnd;
        }

        void cmbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cmbPlanType.Text)
            {
                case"总体计划":
                    //隐藏年月
                    this.lbYearMonth.Visible = false;
                    this.cmoYear.Visible = false;
                    this.lbYear.Visible = false;
                    this.cmoStartMonth.Visible = false;
                    this.lbMonth.Visible = false;

                    //日期不可用
                    this.dtMadeStartDateQuery.Visible = false;
                    this.dtMadeEndDateQuery.Visible = false;
                    this.label9.Visible = false;
                    this.label8.Visible = false;
                    this.dtMadeStartDateQuery.Enabled = false;
                    this.dtMadeEndDateQuery.Enabled = false;
                    break;
                case "月度计划":
                    //隐藏年月
                    this.lbYearMonth.Visible = true;
                    this.cmoYear.Visible = true;
                    this.lbYear.Visible = true;
                    this.cmoStartMonth.Visible = true;
                    this.lbMonth.Visible = true;

                    //日期不可用
                    this.dtMadeStartDateQuery.Visible = true;
                    this.dtMadeEndDateQuery.Visible = true;
                    this.label9.Visible = true;
                    this.label8.Visible = true;
                    this.dtMadeStartDateQuery.Enabled = false;
                    this.dtMadeEndDateQuery.Enabled = false;

                    break;
                case "日常计划":
                    //隐藏年月
                    this.lbYearMonth.Visible = false;
                    this.cmoYear.Visible = false;
                    this.lbYear.Visible = false;
                    this.cmoStartMonth.Visible = false;
                    this.lbMonth.Visible = false;


                    //日期不可用
                    this.dtMadeStartDateQuery.Visible = true;
                    this.dtMadeEndDateQuery.Visible = true;
                    this.label9.Visible = true;
                    this.label8.Visible = true;
                    this.dtMadeStartDateQuery.Enabled = true;
                    this.dtMadeEndDateQuery.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        //查找 for SearchList
        public void Start(string GUID)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", GUID));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(ResourceRequireReceipt), oq);
                if (list.Count > 0)
                {
                    curBillMaster = list[0] as ResourceRequireReceipt;
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        //显示数据
        private void ModelToView()
        {
            #region 处理主表
            this.cmbPlanType.Text = Enum.GetName(typeof(PlanType),curBillMaster.StagePlanType);
            this.cbResourceType.Text =  Enum.GetName(typeof(ResourceTpye),curBillMaster.MaterialType);
            //计划名称
            this.txtPlanName.Text = curBillMaster.ReceiptName;
            //计划说明
            this.txtRemark.Text = curBillMaster.Descript;
            //状态
            this.cmbState.Text = Enum.GetName(typeof(ResourceTpye), curBillMaster.DocState);
            //开始时间
            this.dtMadeStartDateQuery.Value = curBillMaster.PlanRequireDateBegin;
            //结束时间
            this.dtMadeEndDateQuery.Value = curBillMaster.PlanRequireDateEnd;

            this.cmbState.Text = Enum.GetName(typeof(ResourceRequirePlanState),curBillMaster.State);
            #endregion

            #region  处理明细表
            if (curBillMaster.Details.Count > 0)
            {
                foreach (ResourceRequireReceiptDetail item in curBillMaster.Details)
                {

                    AddGridResourceRequireDetail(item,false);
                }
            }
            #endregion
        }

        #region override methods

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                ClearView();
                base.NewView();

                #region Instantiation curBillMaster and assign values
                this.curBillMaster = new ResourceRequireReceipt();
                AssignCommenValueCurBillMaster();
                #endregion

                #region assign values to controls
                this.cmbPlanType.Text = EnumUtil<PlanType>.GetDescription(Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain.PlanType.月度计划);
                this.cmbPlanType_SelectedIndexChanged(null, null);
                
                this.cmbState.Text = EnumUtil<ResourceRequirePlanState>.GetDescription(ResourceRequirePlanState.制定);
                this.cmoYear.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Year.ToString();
                this.cmoStartMonth.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginDate.Month.ToString();
                #endregion

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
            if (curBillMaster.State != ResourceRequirePlanState.冻结 && curBillMaster.State != ResourceRequirePlanState.发布 && curBillMaster.State != ResourceRequirePlanState.提交)
            {
                MessageBox.Show("信息已经提交,不允许修改！");
                return false;
            }

            if (curBillMaster.DocState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Valid || curBillMaster.DocState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
            {
                base.ModifyView();
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

                bool isAdded = curBillMaster.Id == null;
                curBillMaster = model.SaveOrUpdateResourceRequireReceipt(curBillMaster);

                //写日志
                StaticMethod.InsertLogData(curBillMaster.Id, isAdded ? "新增" : "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "资源需求计划管理", "", curBillMaster.ProjectName);

                //更新Caption
                this.ViewCaption = ViewName + "-" + curBillMaster.ReceiptName;
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
            if (curBillMaster.State == ResourceRequirePlanState.冻结 || curBillMaster.State == ResourceRequirePlanState.发布 || curBillMaster.State == ResourceRequirePlanState.提交)
            {
                MessageBox.Show("信息已经提交！");
                return false;
            }
            if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    FlashScreen.Show("正在执行提交，请稍候........");
                    string errMsg = "";


                    if (curBillMaster.Id != null)
                    {

                        IList list = model.GenerateSupplyResourcePlanNew(curBillMaster.Id);

                        errMsg = list[0] as string;

                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            FlashScreen.Close();
                            MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            curBillMaster.State = ResourceRequirePlanState.提交;
                            curBillMaster = model.SaveOrUpdateResourceRequireReceipt(curBillMaster);
                            RefreshView();
                            FlashScreen.Close();
                            MessageBox.Show("物资需求计划生成成功！");
                        }
                    }
                    else
                    {
                        FlashScreen.Close();
                        MessageBox.Show("请先保存信息！");
                        return false;
                    }
                    return true;
                }
                catch (Exception e)
                {
                    FlashScreen.Close();
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
                if (curBillMaster.State != ResourceRequirePlanState.冻结 && curBillMaster.State != ResourceRequirePlanState.发布 && curBillMaster.State != ResourceRequirePlanState.提交)
                {
                    MessageBox.Show("信息已经提交,不允许删除！");
                    return false;
                }

                if (!model.DeleteResourceRequireReceipt(curBillMaster))
                    return false;

                //插入日志
                StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "资源需求计划管理", "", curBillMaster.ProjectName);

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
                        ClearView();
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
                //清除界面值
                ClearView();
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }


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
                ObjectLock.Lock(cmbState, true);
                ToolMenu.LockItem(ToolMenuItem.Submit);
                if ((curBillMaster.Id != null && (curBillMaster.StagePlanType == Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain.PlanType.月度计划)) || this.cmbPlanType.Text == "月度计划")
                {

                    this.lbYearMonth.Visible = true;
                    this.cmoYear.Visible = true;
                    this.lbYear.Visible = true;
                    this.cmoStartMonth.Visible = true;
                    this.lbMonth.Visible = true;

                    this.dtMadeStartDateQuery.Visible = true;
                    this.dtMadeEndDateQuery.Visible = true;
                    this.label9.Visible = true;
                    this.label8.Visible = true;
                    this.dtMadeStartDateQuery.Enabled = false;
                    this.dtMadeEndDateQuery.Enabled = false;

                }
                if ((curBillMaster.Id != null && (curBillMaster.StagePlanType == Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain.PlanType.总体计划)) || this.cmbPlanType.Text == "总体计划")
                {

                    this.lbYearMonth.Visible = true;
                    this.cmoYear.Visible = true;
                    this.lbYear.Visible = true;
                    this.cmoStartMonth.Visible = true;
                    this.lbMonth.Visible = true;

                    this.dtMadeStartDateQuery.Visible = false;
                    this.dtMadeEndDateQuery.Visible = false;
                    this.label9.Visible = false;
                    this.label8.Visible = false;
                    this.dtMadeStartDateQuery.Enabled = false;
                    this.dtMadeEndDateQuery.Enabled = false;

                }
                if ((curBillMaster.Id != null && (curBillMaster.StagePlanType == Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain.PlanType.日常计划)) || this.cmbPlanType.Text == "日常计划")
                {

                    this.lbYearMonth.Visible = true;
                    this.cmoYear.Visible = true;
                    this.lbYear.Visible = true;
                    this.cmoStartMonth.Visible = true;
                    this.lbMonth.Visible = true;

                    this.dtMadeStartDateQuery.Visible = true;
                    this.dtMadeEndDateQuery.Visible = true;
                    this.label9.Visible = true;
                    this.label8.Visible = true;
                    this.dtMadeStartDateQuery.Enabled = true;
                    this.dtMadeEndDateQuery.Enabled = true;

                }
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                ToolMenu.UnlockItem(ToolMenuItem.Submit);
                this.lbYearMonth.Visible = false;
                this.cmoYear.Visible = false;
                this.lbYear.Visible = false;
                this.cmoStartMonth.Visible = false;
                this.lbMonth.Visible = false;
            }

        }
        #endregion

        #region  由override methods衍生的杂项

        //清空view上的数据
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
        /// 新增时 curBillMaster赋值（非界面控件控制）
        /// </summary>
        private void AssignCommenValueCurBillMaster()
        {
            curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
            curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.CreateDate = ConstObject.LoginDate;
            curBillMaster.CreateYear = ConstObject.LoginDate.Year;
            curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
            curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
            curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
            curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
            curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
            curBillMaster.State = ResourceRequirePlanState.制定;
            if (projectInfo != null)
            {
                curBillMaster.ProjectId = projectInfo.Id;
                curBillMaster.ProjectName = projectInfo.Name;
            }
        }
        private bool ViewToModel()
        {
            if (!ValidView())
                return false;
            try
            {
                curBillMaster.ReceiptName = this.txtPlanName.Text;
                curBillMaster.Descript = this.txtRemark.Text;
                curBillMaster.State = EnumUtil<ResourceRequirePlanState>.FromDescription(this.cmbState.Text);
                curBillMaster.PlanRequireDateBegin = this.dtMadeStartDateQuery.Value;
                curBillMaster.PlanRequireDateEnd = this.dtMadeEndDateQuery.Value;
                //计划类型
                curBillMaster.StagePlanType = EnumUtil<PlanType>.FromDescription(this.cmbPlanType.Text);
                //资源类型
                curBillMaster.MaterialType = EnumUtil<ResourceTpye>.FromDescription(this.cbResourceType.Text);

                curBillMaster = model.SaveOrUpdateResourceRequireReceipt(curBillMaster) as ResourceRequireReceipt;
                ViewToDetails();
                


                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        private void ViewToDetails()
        {
            if (!string.IsNullOrEmpty(curBillMaster.Id))
            {
                foreach (DataGridViewRow var in this.gridResourceRequireDetail.Rows)
                {
                    ResourceRequireReceiptDetail currBillDtl = new ResourceRequireReceiptDetail();
                    if (var.Tag != null)
                    {
                        currBillDtl = var.Tag as ResourceRequireReceiptDetail;

                        currBillDtl.PeriodQuantity = ClientUtil.ToDecimal(var.Cells[PeriodQuantity.Name].Value);
                        currBillDtl.TechnicalParameters = ClientUtil.ToString(var.Cells[TechnicalParameters.Name].Value);
                        currBillDtl.QualityStandards = ClientUtil.ToString(var.Cells[colQualityStandards.Name].Value);
                        currBillDtl.Descript = ClientUtil.ToString(var.Cells[Descript.Name].Value);

                        currBillDtl.TheResReceipt = curBillMaster;
                    }
                    curBillMaster.AddDetail(currBillDtl);
                }
            }
        }

        private bool ValidView()
        {
            if (this.txtPlanName.Text.Trim() == "")
            {
                MessageBox.Show("计划名称不能为空！" );
                this.txtPlanName.Focus();
                return false;
            }
            gridResourceRequireDetail.EndEdit();
            gridResourceRequireDetail_CellEndEdit(this.gridResourceRequireDetail, new DataGridViewCellEventArgs(this.gridResourceRequireDetail.CurrentCell.ColumnIndex, this.gridResourceRequireDetail.CurrentRow.Index));

            foreach (DataGridViewRow dr in gridResourceRequireDetail.Rows)
            {

                if (dr.IsNewRow) break;
                if (ClientUtil.ToDecimal(dr.Cells[PeriodQuantity.Name].EditedFormattedValue) <= 0)
                {
                    MessageBox.Show("期间需求量应大于0！");
                    gridResourceRequireDetail.CurrentCell = dr.Cells[PeriodQuantity.Name];
                    return false;
                }
                else if (ClientUtil.ToDecimal(dr.Cells[PeriodQuantity.Name].EditedFormattedValue) > ClientUtil.ToDecimal(dr.Cells[PlanTotalRequire.Name].EditedFormattedValue))
                {
                    MessageBox.Show("期间需求量须小于计划需求总量！");
                    gridResourceRequireDetail.CurrentCell = dr.Cells[PeriodQuantity.Name];
                    return false;
                }

            }

            return true;
        }

        private void gridResourceRequireDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = gridResourceRequireDetail.Columns[e.ColumnIndex].Name;
            if (colName == PeriodQuantity.Name)
            {
                string temp_quantity = gridResourceRequireDetail.Rows[e.RowIndex].Cells[PeriodQuantity.Name].Value.ToString();
                validity = CommonMethod.VeryValid(temp_quantity);
                if (!validity)
                {
                    MessageBox.Show("请输入数字！");
                    gridResourceRequireDetail.Rows[e.RowIndex].Cells[PeriodQuantity.Name].Value = "";
                }
            }
        }

        #endregion

    }
}
