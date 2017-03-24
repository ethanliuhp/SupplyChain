using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using FlexCell;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;


namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VScheduleQuery : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();

        private Hashtable detailHashtable = new Hashtable();
        private ProductionScheduleMaster CurBillMaster;
        //private ProductionScheduleDetail ChildRootNode;

        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";

        private int startImageCol = 1, endImageCol = 19;

        private PlanTypeQueryEnum optPlanType = PlanTypeQueryEnum.总体进度计划;

        CurrentProjectInfo projectInfo = null;

        /// <summary>
        /// 当前菜单所属的权限菜单
        /// </summary>
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;

        public VScheduleQuery()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitEvents()
        {
            cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);

            cboPlanName.SelectedIndexChanged += new EventHandler(cboPlanName_SelectedIndexChanged);

            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);

            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExportToMPP.Click += new EventHandler(btnExportToMPP_Click);

            gridMasterSchedulePlan.CellClick += new DataGridViewCellEventHandler(gridMasterSchedulePlan_CellClick);
            gridMaster.CellClick += new DataGridViewCellEventHandler(gridMaster_CellClick);
            btnViewComments.Click += new EventHandler(btnViewComments_Click);
            btnViewCommentMaster.Click += new EventHandler(btnViewCommentMaster_Click);
        }

        #region 查看评论
        void btnViewComments_Click(object sender, EventArgs e)
        {
            if (gridMasterSchedulePlan.SelectedRows.Count > 0)
            {
                ProductionScheduleMaster m = gridMasterSchedulePlan.SelectedRows[0].Tag as ProductionScheduleMaster;
                VBillComments frm = new VBillComments(m.Id, m.ScheduleTypeDetail, m.ScheduleType.ToString(), m.HandlePerson, m.HandlePersonName, m.OperOrgInfo, m.OperOrgInfoName, m.OpgSysCode, m.CreateDate);
                frm.ShowDialog();
                lblCount.Text = StaticMethod.GetCommentsCountByBillId(m.Id).ToString();
            }
            else
            {
                MessageBox.Show("请选择操作单据！");
            }
        }
        void btnViewCommentMaster_Click(object sender, EventArgs e)
        {
            if (gridMaster.SelectedRows.Count > 0)
            {
                WeekScheduleMaster m = gridMaster.SelectedRows[0].Tag as WeekScheduleMaster;
                VBillComments frm = new VBillComments(m.Id, m.PlanName, m.ExecScheduleType.ToString(), m.HandlePerson, m.HandlePersonName, m.HandleOrg, m.HandleOrg.Name, m.HandleOrg.SysCode, m.CreateDate);
                frm.ShowDialog();
                lblCountMaster.Text = StaticMethod.GetCommentsCountByBillId(m.Id).ToString();
            }
            else
            {
                MessageBox.Show("请选择操作单据！");
            }
        }
        #endregion

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();

            InitFlexGrid(1);

            foreach (string planType in Enum.GetNames(typeof(PlanTypeQueryEnum)))
            {
                cbPlanType.Items.Add(planType);
            }
            cbPlanType.SelectedIndex = 0;

            optPlanType = VirtualMachine.Component.Util.EnumUtil<PlanTypeQueryEnum>.FromDescription(cbPlanType.SelectedItem.ToString());

            SetVisibleControlAndInitData(optPlanType);

            dtBeginCreateDate.Value = DateTime.Now.AddMonths(-1);
            dtEndCreateDate.Value = DateTime.Now;

            btnViewCommentMaster.Visible = false;
            btnViewComments.Visible = false;
        }

        private void SetVisibleControlAndInitData(PlanTypeQueryEnum type)
        {
            if (type == PlanTypeQueryEnum.总滚动进度计划)
            {
                splitContainerSchedulePlan.Visible = true;
                btnExportToMPP.Visible = true;

                splitContainerPlan.Visible = false;

                splitContainerSchedulePlan.Dock = DockStyle.Fill;

                    ShowPlanFactInfo(true);
                

            }
            else if (type == PlanTypeQueryEnum.季度进度计划 || type == PlanTypeQueryEnum.总体进度计划 || type == PlanTypeQueryEnum.月度进度计划 || type == PlanTypeQueryEnum.工区周进度计划 || type == PlanTypeQueryEnum.项目周进度计划)
            {
                splitContainerSchedulePlan.Visible = false;
                btnExportToMPP.Visible = false;

                splitContainerPlan.Visible = true;

                splitContainerPlan.Dock = DockStyle.Fill;
            }
        }

        private void ShowPlanFactInfo(bool isShow)
        {
            flexGrid.Column(endImageCol + 5).Visible = isShow;
            flexGrid.Column(endImageCol + 6).Visible = isShow;
            flexGrid.Column(endImageCol + 7).Visible = isShow;
        }

        void gridMasterSchedulePlan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            CurBillMaster = gridMasterSchedulePlan.Rows[e.RowIndex].Tag as ProductionScheduleMaster;
            int commentCount = StaticMethod.GetCommentsCountByBillId(CurBillMaster.Id);
            lblCount.Text = commentCount.ToString();
            FillFlex();
        }

        void gridMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            WeekScheduleMaster master = gridMaster.Rows[e.RowIndex].Tag as WeekScheduleMaster;
            lblCountMaster.Text = StaticMethod.GetCommentsCountByBillId(master.Id).ToString();
            gridDetail.Rows.Clear();
            if (master != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
                oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("SubContractProject", NHibernate.FetchMode.Eager);

                IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
                AddWeekPlanDetailInGrid(listDtl);
            }
        }

        void btnExportToMPP_Click(object sender, EventArgs e)
        {
            if (optPlanType == PlanTypeQueryEnum.总滚动进度计划)
            {
                //OpenFileDialog ofg = new OpenFileDialog();
                if (CurBillMaster == null)
                {
                    btnQuery_Click(sender, new EventArgs());
                }
                SaveFileDialog sfd = new SaveFileDialog();
                //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.Filter = "项目 (*.MPP)|*.MPP";
                sfd.RestoreDirectory = true;
                sfd.FileName = CurBillMaster.ScheduleName;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string fileName = sfd.FileName;
                    if (System.IO.File.Exists(fileName))
                    {
                        IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                        MSProjectUtil mSProjectUtil = new MSProjectUtil();
                        mSProjectUtil.UpdateMPP(fileName, list);
                    }
                    else
                    {
                        IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
                        MSProjectUtil.CreateMPP(fileName, list, this.Handle);
                    }
                }
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            if (optPlanType == PlanTypeQueryEnum.总滚动进度计划)
            {
                gridMasterSchedulePlan.Rows.Clear();

                EnumScheduleType enumScheduleType = VirtualMachine.Component.Util.EnumUtil<EnumScheduleType>.FromDescription(optPlanType.ToString());
                oq.AddCriterion(Expression.Eq("ScheduleType", enumScheduleType));


                if (cboPlanName.Text.Trim() != "")
                    oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", cboPlanName.Text.Trim()));

                if (cbPlanVersion.Text.Trim() != "")
                    oq.AddCriterion(Expression.Eq("ScheduleName", cbPlanVersion.Text.Trim()));

                //if (cbState.Text.Trim() != "")
                //    oq.AddCriterion(Expression.Eq("State", VirtualMachine.Component.Util.EnumUtil<EnumScheduleState>.FromDescription(cbState.Text.Trim())));

                if (cbState.Text.Trim() != "" && cbState.SelectedItem != null)
                {
                    System.Web.UI.WebControls.ListItem li = cbState.SelectedItem as System.Web.UI.WebControls.ListItem;
                    if (li != null)
                        oq.AddCriterion(Expression.Eq("DocState", (DocumentState)Convert.ToInt32(li.Value)));
                }

                if (txtHandlePerson.Text.Trim() != "" && txtHandlePerson.Result != null && txtHandlePerson.Result.Count > 0)
                {
                    PersonInfo per = txtHandlePerson.Result[0] as PersonInfo;
                    oq.AddCriterion(Expression.Eq("HandlePerson.Id", per.Id));
                }

                oq.AddCriterion(Expression.Ge("CreateDate", dtBeginCreateDate.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", dtEndCreateDate.Value.Date.AddDays(1)));

                IList list = model.ProductionManagementSrv.GetObjects(typeof(ProductionScheduleMaster), oq);

                gridMasterSchedulePlan.Rows.Clear();
                flexGrid.Rows = 1;
                AddSchedulePlanMasterInfoInGrid(list);

                if (gridMasterSchedulePlan.Rows.Count > 0)
                {
                    gridMasterSchedulePlan.CurrentCell = gridMasterSchedulePlan.Rows[0].Cells[0];
                    gridMasterSchedulePlan_CellClick(gridMasterSchedulePlan, new DataGridViewCellEventArgs(0, 0));
                }

            }
            else if (optPlanType == PlanTypeQueryEnum.季度进度计划 || optPlanType == PlanTypeQueryEnum.总体进度计划 || optPlanType == PlanTypeQueryEnum.工区周进度计划 || optPlanType == PlanTypeQueryEnum.项目周进度计划 || optPlanType == PlanTypeQueryEnum.月度进度计划)
            {
                gridDetail.Rows.Clear();
                gridMaster.Rows.Clear();

                if(optPlanType == PlanTypeQueryEnum.总体进度计划)
                    oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.总体进度计划));
                if(optPlanType == PlanTypeQueryEnum.月度进度计划)
                    oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.月度进度计划));
                if(optPlanType == PlanTypeQueryEnum.季度进度计划)
                    oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.季度进度计划));


                if (optPlanType == PlanTypeQueryEnum.工区周进度计划)
                {
                    oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.周进度计划));
                    oq.AddCriterion(Expression.Not(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成)));
                }
                else if (optPlanType == PlanTypeQueryEnum.项目周进度计划)
                {
                    oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.周进度计划));
                    oq.AddCriterion(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成));
                }

                if (cboPlanName.Text.Trim() != "")
                    oq.AddCriterion(Expression.Eq("PlanName", cboPlanName.Text.Trim()));

                if (cbState.Text.Trim() != "" && cbState.SelectedItem != null)
                {
                    System.Web.UI.WebControls.ListItem li = cbState.SelectedItem as System.Web.UI.WebControls.ListItem;
                    if (li != null)
                        oq.AddCriterion(Expression.Eq("DocState", (DocumentState)Convert.ToInt32(li.Value)));
                }

                if (txtHandlePerson.Text.Trim() != "" && txtHandlePerson.Result != null && txtHandlePerson.Result.Count > 0)
                {
                    PersonInfo per = txtHandlePerson.Result[0] as PersonInfo;
                    oq.AddCriterion(Expression.Eq("HandlePerson.Id", per.Id));
                }

                oq.AddCriterion(Expression.Ge("CreateDate", dtBeginCreateDate.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", dtEndCreateDate.Value.Date.AddDays(1)));

                #region 过滤数据权限
                //if (StaticMethod.IsEnabledDataAuth && ConstObject.TheLogin.TheAccountOrgInfo != null && ConstObject.IsSystemAdministrator() == false && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
                //{
                //    //1.查询数据权限配置
                //    ObjectQuery oqAuth = new ObjectQuery();
                //    oqAuth.AddCriterion(Expression.Eq("AuthMenu.Id", TheAuthMenu.Id));

                //    Disjunction disAuth = new Disjunction();
                //    foreach (OperationRole role in ConstObject.TheRoles)
                //    {
                //        disAuth.Add(Expression.Eq("AppRole.Id", role.Id));
                //    }
                //    oqAuth.AddCriterion(disAuth);

                //    IEnumerable<AuthConfigOrgSysCode> listAuth = model.ProductionManagementSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();

                //    if (listAuth.Count() > 0)//如果配置了权限规则
                //    {
                //        var query = from a in listAuth
                //                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.无约束
                //                    select a;

                //        if (query.Count() == 0)//未设置“无约束”规则
                //        {
                //            disAuth = new Disjunction();

                //            //2.根据数据权限定义的规则过滤数据
                //            query = from a in listAuth
                //                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.该核算组织的
                //                    select a;
                //            if (query.Count() > 0)//如果配置中包含操作“该核算组织的”的权限，则无需再添加其它规则
                //            {
                //                disAuth.Add(Expression.Like("HandlePersonSyscode", ConstObject.TheLogin.TheAccountOrgInfo.SysCode, MatchMode.Start));
                //            }
                //            else
                //            {
                //                foreach (AuthConfigOrgSysCode config in listAuth)
                //                {
                //                    WeekScheduleMaster m = new WeekScheduleMaster();

                //                    if (config.ApplyRule == AuthOrgSysCodeRuleEnum.本人的)
                //                    {
                //                        disAuth.Add(Expression.Eq("HandlePerson.Id", ConstObject.LoginPersonInfo.Id));
                //                    }
                //                    else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.本部门的)
                //                    {
                //                        disAuth.Add(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id));
                //                    }
                //                    else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.兄弟部门的)
                //                    {
                //                        string theSysCode = ConstObject.TheOperationOrg.SysCode;
                //                        if (!string.IsNullOrEmpty(theSysCode) && theSysCode.IndexOf(".") > -1)
                //                        {
                //                            //获取父组织层次码
                //                            theSysCode = theSysCode.Substring(0, theSysCode.Length - 1);
                //                            theSysCode = theSysCode.Substring(0, theSysCode.LastIndexOf("."));

                //                            AbstractCriterion exp = Expression.And(Expression.Eq("HandOrgLevel", ConstObject.TheOperationOrg.Level), 
                //                                Expression.And(Expression.Like("HandlePersonSyscode", theSysCode, MatchMode.Start),Expression.Not(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id))));

                //                            disAuth.Add(exp);

                //                        }
                //                    }
                //                    else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.下属部门的)//允许看下级部门的
                //                    {
                //                        disAuth.Add(Expression.And(Expression.Like("HandlePersonSyscode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start), Expression.Not(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id))));
                //                    }
                //                    else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.上级的)//允许看上级部门的
                //                    {
                //                        string theSysCode = ConstObject.TheOperationOrg.SysCode;
                //                        if (!string.IsNullOrEmpty(theSysCode))
                //                        {
                //                            string[] sysCodes = theSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                //                            for (int i = 0; i < sysCodes.Length - 1; i++)
                //                            {
                //                                string sysCode = "";
                //                                for (int j = 0; j <= i; j++)
                //                                {
                //                                    sysCode += sysCodes[j] + ".";
                //                                }

                //                                disAuth.Add(Expression.Eq("HandlePersonSyscode", sysCode));
                //                            }
                //                        }
                //                    }
                //                }
                //            }

                //            string term = disAuth.ToString();
                //            if (term != "()")//不加条件时为()
                //                oq.AddCriterion(disAuth);
                //        }
                //    }
                //    else//未配置数据权限缺省为查看本部门和下属部门的数据
                //    {
                //        oq.AddCriterion(Expression.Like("HandlePersonSyscode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
                //    }
                //}
                #endregion


                oq.AddOrder(Order.Asc("Code"));
                oq.AddFetchMode("HandleOrg", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
                IList list = model.ProductionManagementSrv.GetObjects(typeof(WeekScheduleMaster), oq);

                gridMaster.Rows.Clear();
                AddWeekPlanMasterInfoInGrid(list);

                if (gridMaster.Rows.Count > 0)
                {
                    gridMaster.CurrentCell = gridMaster.Rows[0].Cells[0];
                    gridMaster_CellClick(gridMaster, new DataGridViewCellEventArgs(0, 0));
                }
            }

        }

        private IList ViewToDetails()
        {
            IList list = new ArrayList();
            for (int i = 1; i < flexGrid.Rows; i++)
            {
                string detailId = flexGrid.Cell(i, 0).Tag;
                if (detailId == null || detailId.Equals("")) continue;
                ProductionScheduleDetail detail = null;
                foreach (ProductionScheduleDetail tempDetail in CurBillMaster.Details)
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
                //工期计量单位
                detail.ScheduleUnit = flexGrid.Cell(i, endImageCol + 3).Text;
                //计划工期
                detail.PlannedDuration = flexGrid.Cell(i, endImageCol + 4).IntegerValue;
                //实际开始时间
                string actualBeginDate = flexGrid.Cell(i, endImageCol + 5).Text;
                if (actualBeginDate != null && !actualBeginDate.Equals(""))
                {
                    detail.ActualBeginDate = DateTime.Parse(actualBeginDate);
                }
                else
                {
                    detail.ActualBeginDate = new DateTime(1900, 1, 1);
                }

                //实际结束时间
                string actualEndDate = flexGrid.Cell(i, endImageCol + 6).Text;
                if (actualEndDate != null && !actualEndDate.Equals(""))
                {
                    detail.ActualEndDate = DateTime.Parse(actualEndDate);
                }
                else
                {
                    detail.ActualEndDate = new DateTime(1900, 1, 1);
                }
                //实际工期
                detail.ActualDuration = flexGrid.Cell(i, endImageCol + 7).IntegerValue;
                //计划说明
                detail.TaskDescript = flexGrid.Cell(i, endImageCol + 8).Text;
                CurBillMaster.AddDetail(detail);
            }
            return list;
        }

        void flexGrid_Click(object Sender, EventArgs e)
        {
            if (flexGrid.ActiveCell.ImageKey == imageExpand)
            {
                flexGrid.ActiveCell.SetImage(imageCollapse);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
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
                int childs = model.ProductionManagementSrv.CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, false);
                }
            }
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

        void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            optPlanType = VirtualMachine.Component.Util.EnumUtil<PlanTypeQueryEnum>.FromDescription(cbPlanType.SelectedItem.ToString());

            SetVisibleControlAndInitData(optPlanType);

            cbPlanVersion.Text = "";
            cbPlanVersion.Items.Clear();
            SetPlanName(optPlanType);

            SetPlanState(optPlanType);
        }

        void cboPlanName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPlanVersion(optPlanType, cboPlanName.Text.Trim());
        }

        private void SetPlanName(PlanTypeQueryEnum queryPlanType)
        {
            try
            {
                cboPlanName.Text = "";
                cboPlanName.Items.Clear();

                if (queryPlanType == PlanTypeQueryEnum.总滚动进度计划)
                {
                    EnumScheduleType enumScheduleType =  EnumScheduleType.总滚动进度计划;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                    oq.AddCriterion(Expression.Eq("ScheduleType", enumScheduleType));

                    IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

                    if (listMaster.Count > 0)
                    {
                        for (int i = 0; i < listMaster.Count; i++)
                        {
                            ProductionScheduleMaster item = listMaster[i] as ProductionScheduleMaster;

                            if (!string.IsNullOrEmpty(item.ScheduleTypeDetail) && !cboPlanName.Items.Contains(item.ScheduleTypeDetail))//有版本控制
                                cboPlanName.Items.Add(item.ScheduleTypeDetail);
                        }
                    }
                }
                else
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

                    if (queryPlanType == PlanTypeQueryEnum.工区周进度计划)
                    {
                        oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.周进度计划));
                        oq.AddCriterion(Expression.Not(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成)));
                    }
                    else if (queryPlanType == PlanTypeQueryEnum.项目周进度计划)
                    {
                        oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.周进度计划));
                        oq.AddCriterion(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成));
                    }
                    else if (queryPlanType == PlanTypeQueryEnum.月度进度计划)
                    {
                        oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.月度进度计划));
                    }
                    else if (queryPlanType == PlanTypeQueryEnum.季度进度计划)
                    {
                        oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.季度进度计划));
                    }
                    else if (queryPlanType == PlanTypeQueryEnum.总体进度计划)
                    {
                        oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.总体进度计划));
                    }

                    IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleMaster), oq);

                    if (listMaster.Count > 0)
                    {
                        for (int i = 0; i < listMaster.Count; i++)
                        {
                            WeekScheduleMaster item = listMaster[i] as WeekScheduleMaster;

                            if (!string.IsNullOrEmpty(item.PlanName))
                                cboPlanName.Items.Add(item.PlanName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void SetPlanState(PlanTypeQueryEnum queryPlanType)
        {
            try
            {
                cbState.Text = "";
                cbState.Items.Clear();

                //if (queryPlanType == PlanTypeQueryEnum.总进度计划 || queryPlanType == PlanTypeQueryEnum.总滚动进度计划)
                //{
                //    foreach (string state in Enum.GetNames(typeof(EnumScheduleState)))
                //    {
                //        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                //        li.Text = state;
                //        li.Value = state;

                //        cbState.Items.Add(li);
                //    }
                //}
                //else
                //{
                foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = ClientUtil.GetDocStateName(state);
                    li.Value = ((int)state).ToString();
                    cbState.Items.Add(li);
                }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void SetPlanVersion(PlanTypeQueryEnum queryPlanType, string scheduleName)
        {
            try
            {
                cbPlanVersion.Text = "";
                cbPlanVersion.Items.Clear();

                if (queryPlanType == PlanTypeQueryEnum.总滚动进度计划)
                {
                    EnumScheduleType enumScheduleType = EnumScheduleType.总滚动进度计划;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                    oq.AddCriterion(Expression.Eq("ScheduleType", enumScheduleType));
                    oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", scheduleName));

                    IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);


                    if (listMaster.Count > 0)
                    {
                        for (int i = 0; i < listMaster.Count; i++)
                        {
                            ProductionScheduleMaster item = listMaster[i] as ProductionScheduleMaster;

                            if (!string.IsNullOrEmpty(item.ScheduleName))
                                cbPlanVersion.Items.Add(item.ScheduleName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void FillFlex()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 3).Locked = true;
            detailHashtable.Clear();
            IList list = model.ProductionManagementSrv.GetChilds(CurBillMaster);
            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                foreach (ProductionScheduleDetail detail in list)
                {
                    //if (detail.State == EnumScheduleDetailState.失效)
                    //{
                    //    flexGrid.Rows = flexGrid.Rows - 1;
                    //    continue;
                    //}
                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                    detailHashtable.Add(detail.Id, detail);
                    flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();
                    //rangeTemp.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;
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
                    //工期计量单位                    
                    //flexGrid.Cell(rowIndex, endImageCol + 3).Text =detail.ScheduleUnit; //"";//22
                    flexGrid.Cell(rowIndex, endImageCol + 3).Text = "天";


                    //计划工期
                    // flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.PlannedDuration == 0 ? "" : detail.PlannedDuration.ToString(); //"计划工期";//23
                    if (detail.PlannedBeginDate != (new DateTime(1900, 1, 1)) && detail.PlannedEndDate != (new DateTime(1900, 1, 1)) && detail.PlannedBeginDate <= detail.PlannedEndDate)
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 4).Text = ((detail.PlannedEndDate - detail.PlannedBeginDate).Days + 1).ToString();
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 4).Text = "";
                    }


                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 5).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 5).Text = detail.ActualBeginDate.ToShortDateString();
                    }
                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.ActualEndDate.ToShortDateString();
                    }
                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();
                    //计划说明
                    flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.TaskDescript; //"计划说明";//27

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
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private int NodesCount(TreeNode treeNode)
        {
            int result = 0;
            if (treeNode != null)
            {
                result = treeNode.Nodes.Count;
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    result += NodesCount(tn);
                }
            }
            return result;
        }

        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows = rows;
            flexGrid.Cols = 28;//其中0列隐藏 1-19 为放置图片列 20-27为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Vertical;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);

            FlexCell.Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }

            range = flexGrid.Range(0, startImageCol, 0, endImageCol);
            range.Merge();
            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            flexGrid.Cell(0, endImageCol + 1).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 2).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "工期计量单位";//22
            flexGrid.Cell(0, endImageCol + 4).Text = "计划工期";//23
            flexGrid.Cell(0, endImageCol + 5).Text = "实际开始时间";//24
            flexGrid.Cell(0, endImageCol + 6).Text = "实际结束时间";//25
            flexGrid.Cell(0, endImageCol + 7).Text = "实际工期";//26
            flexGrid.Cell(0, endImageCol + 8).Text = "计划说明";//27

            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).CellType = FlexCell.CellTypeEnum.ComboBox;
            flexGrid.Column(endImageCol + 5).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 6).CellType = FlexCell.CellTypeEnum.Calendar;

            flexGrid.Column(endImageCol + 4).Mask = FlexCell.MaskEnum.Digital;
            flexGrid.Column(endImageCol + 7).Mask = FlexCell.MaskEnum.Digital;

            FlexCell.ComboBox cb = flexGrid.ComboBox(endImageCol + 3);

            for (int i = endImageCol + 1; i < flexGrid.Cols; i++)
            {
                flexGrid.Column(i).Locked = true;
            }
            try
            {
                IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleUnit);
                if (list != null && list.Count > 0)
                {
                    foreach (BasicDataOptr bd in list)
                    {
                        cb.Items.Add(bd.BasicName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取工期计量单位出错。");
            }

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void AddSchedulePlanMasterInfoInGrid(IList listMaster)
        {
            foreach (ProductionScheduleMaster master in listMaster)
            {
                int index = gridMasterSchedulePlan.Rows.Add();
                DataGridViewRow row = gridMasterSchedulePlan.Rows[index];

                row.Cells[colSchedulePlanName.Name].Value = master.ScheduleTypeDetail;
                row.Cells[colSchedulePlanVersion.Name].Value = master.ScheduleName;
                row.Cells[colSchedulePlanState.Name].Value = ClientUtil.GetDocStateName(master.DocState);
                row.Cells[colSchedulePlanCaliber.Name].Value = master.ScheduleCaliber;
                row.Cells[colSchedulePlanCreatePerson.Name].Value = master.HandlePersonName;
                row.Cells[colSchedulePlanCreateDate.Name].Value = master.CreateDate.ToShortDateString();
                row.Cells[colSchedulePlanDesc.Name].Value = master.Descript;

                row.Tag = master;
            }
        }
        private void AddSchedulePlanMasterInfoInGrid(ProductionScheduleMaster master)
        {
            int index = gridMasterSchedulePlan.Rows.Add();
            DataGridViewRow row = gridMasterSchedulePlan.Rows[index];

            row.Cells[colSchedulePlanName.Name].Value = master.ScheduleTypeDetail;
            row.Cells[colSchedulePlanVersion.Name].Value = master.ScheduleName;
            row.Cells[colSchedulePlanState.Name].Value = master.DocState;
            row.Cells[colSchedulePlanCaliber.Name].Value = master.ScheduleCaliber;
            row.Cells[colSchedulePlanCreatePerson.Name].Value = master.HandlePersonName;
            row.Cells[colSchedulePlanCreateDate.Name].Value = master.CreateDate.ToShortDateString();
            row.Cells[colSchedulePlanDesc.Name].Value = master.Descript;

            row.Tag = master;
        }

        private void AddWeekPlanMasterInfoInGrid(IList listMaster)
        {
            foreach (WeekScheduleMaster master in listMaster)
            {
                int index = gridMaster.Rows.Add();
                DataGridViewRow row = gridMaster.Rows[index];

                row.Cells[colMasterPlanName.Name].Value = master.PlanName;
                row.Cells[colMasterPlanState.Name].Value = ClientUtil.GetDocStateName(master.DocState);
                row.Cells[colMasterSumState.Name].Value = master.SummaryStatus;
                row.Cells[colMasterHandlePerson.Name].Value = master.HandlePersonName;
                row.Cells[colMasterCreateDate.Name].Value = master.CreateDate.ToShortDateString();
                row.Cells[colMasterPlannedBeginDate.Name].Value = master.PlannedBeginDate.ToShortDateString();
                row.Cells[colMasterPlannedDateEnd.Name].Value = master.PlannedEndDate.ToShortDateString();
                row.Cells[colMasterDescript.Name].Value = master.Descript;

                row.Tag = master;
            }
        }
        private void AddWeekPlanMasterInfoInGrid(WeekScheduleMaster master)
        {
            int index = gridMaster.Rows.Add();
            DataGridViewRow row = gridMaster.Rows[index];

            row.Cells[colMasterPlanName.Name].Value = master.PlanName;
            row.Cells[colMasterPlanState.Name].Value = ClientUtil.GetDocStateName(master.DocState);
            row.Cells[colMasterSumState.Name].Value = master.SummaryStatus;
            row.Cells[colMasterHandlePerson.Name].Value = master.HandlePersonName;
            row.Cells[colMasterCreateDate.Name].Value = master.CreateDate.ToShortDateString();
            row.Cells[colMasterPlannedBeginDate.Name].Value = master.PlannedBeginDate.ToShortDateString();
            row.Cells[colMasterPlannedDateEnd.Name].Value = master.PlannedEndDate.ToShortDateString();
            row.Cells[colMasterDescript.Name].Value = master.Descript;

            row.Tag = master;
        }

        private void AddWeekPlanDetailInGrid(IList listDtl)
        {
            foreach (WeekScheduleDetail dtl in listDtl)
            {
                int index = gridDetail.Rows.Add();
                DataGridViewRow row = gridDetail.Rows[index];

                if (dtl.GWBSTree != null)
                {
                    row.Cells[colDtlTaskName.Name].Value = dtl.GWBSTreeName;
                    //row.Cells[colDtlTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTreeName, dtl.GWBSTreeSysCode);
                    row.Cells[colDtlTaskName.Name].ToolTipText = dtl.GWBSTree.FullPath;
                }

                row.Cells[colDtlWorkContent.Name].Value = dtl.MainTaskContent;

                row.Cells[colDtlBear.Name].Value = dtl.SupplierName;

                row.Cells[colDtlPlanBeginTime.Name].Value = dtl.PlannedBeginDate.ToShortDateString();
                row.Cells[colDtlPlanOverTime.Name].Value = dtl.PlannedEndDate.ToShortDateString();
                row.Cells[colDtlPlannedDuration.Name].Value = dtl.PlannedDuration;
                row.Cells[colDtlPlannedWrokQuantity.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.PlannedWrokload);

                row.Cells[colDtlFactBeginTime.Name].Value = StaticMethod.GetShowDateTimeStr(dtl.ActualBeginDate, false);
                row.Cells[colDtlFactEndTime.Name].Value = StaticMethod.GetShowDateTimeStr(dtl.ActualEndDate, false);
                row.Cells[colDtlFactDuration.Name].Value = dtl.ActualDuration;
                row.Cells[colDtlFactWrokQuantity.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.ActualWorklaod);

                row.Cells[colDtlFiguregress.Name].Value = dtl.TaskCompletedPercent;

                row.Cells[colDtlCheckState.Name].ToolTipText = StaticMethod.GetCheckStateShowText(dtl.TaskCheckState);
                row.Cells[colDtlCheckState.Name].Value = dtl.TaskCheckState;

                row.Cells[colDtlRemark.Name].Value = dtl.Descript;

                row.Tag = dtl;
            }
        }
        private void AddWeekPlanDetailInGrid(WeekScheduleDetail dtl)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            if (dtl.GWBSTree != null)
            {
                row.Cells[colDtlTaskName.Name].Value = dtl.GWBSTree.Name;
                row.Cells[colDtlTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTree);
            }

            row.Cells[colDtlWorkContent.Name].Value = dtl.MainTaskContent;

            row.Cells[colDtlBear.Name].Value = dtl.SupplierName;

            row.Cells[colDtlPlanBeginTime.Name].Value = dtl.PlannedBeginDate.ToShortDateString();
            row.Cells[colDtlPlanOverTime.Name].Value = dtl.PlannedEndDate.ToShortDateString();
            row.Cells[colDtlPlannedDuration.Name].Value = dtl.PlannedDuration;
            row.Cells[colDtlPlannedWrokQuantity.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.PlannedWrokload);

            row.Cells[colDtlFactBeginTime.Name].Value = StaticMethod.GetShowDateTimeStr(dtl.ActualBeginDate, false);
            row.Cells[colDtlFactEndTime.Name].Value = StaticMethod.GetShowDateTimeStr(dtl.ActualEndDate, false);
            row.Cells[colDtlFactDuration.Name].Value = dtl.ActualDuration;
            row.Cells[colDtlFactWrokQuantity.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.ActualWorklaod);

            row.Cells[colDtlFiguregress.Name].Value = dtl.TaskCompletedPercent;

            row.Cells[colDtlCheckState.Name].ToolTipText = StaticMethod.GetCheckStateShowText(dtl.TaskCheckState);
            row.Cells[colDtlCheckState.Name].Value = dtl.TaskCheckState;

            row.Cells[colDtlRemark.Name].Value = dtl.Descript;

            row.Tag = dtl;
        }
    }

    /// <summary>
    /// 查询计划类型
    /// </summary>
    public enum PlanTypeQueryEnum
    {
        [Description("总体进度计划")]
        总体进度计划 = 1,
        [Description("总滚动进度计划")]
        总滚动进度计划 = 2,
        [Description("月度进度计划")]
        月度进度计划 = 3,
        [Description("工区周进度计划")]
        工区周进度计划 = 4,
        [Description("项目周进度计划")]
        项目周进度计划 = 5,
        [Description("季度进度计划")]
        季度进度计划 = 6
    }
}
