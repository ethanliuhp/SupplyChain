using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Properties;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VScheduleSelector : TBasicDataView
    {
        MProductionMng model = new MProductionMng();

        ///// <summary>
        ///// 前驱引用计划类型
        ///// </summary>
        //public EnumExecScheduleType FrontPlanType = EnumExecScheduleType.周进度计划;

        private EnumExecScheduleType _frontPlanType;

        ///<summary>
        ///前驱引用计划类型
        ///</summary>
        public  EnumExecScheduleType FrontPlanType
        {
            set
            {
                InitWindowText(value);
                InitComboBox(value);
                
                this._frontPlanType = value;
            }
            get { return this._frontPlanType; }
        }

        /// <summary>
        /// 周进度计划的起始时间
        /// </summary>
        public DateTime? DefaultBeginDate = null;
        /// <summary>
        /// 周进度计划结束时间
        /// </summary>
        public DateTime? DefaultEndDate = null;

        CurrentProjectInfo projectInfo = null;

        private int totalRecords = 0;
  

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        Color unFinishTaskColor = ColorTranslator.FromHtml("#D7E8FE");//未完工任务颜色


        #region 树方式

        private Hashtable detailHashtable = new Hashtable();
        private WeekScheduleMaster CurBillMaster;

        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";
        private EnumScheduleType enumScheduleType = EnumScheduleType.总滚动进度计划;
        private int startImageCol = 1, endImageCol = 19;

        #endregion

        public VScheduleSelector()
        {
            InitializeComponent();
            this.tabPageTree.Visible = false;

            projectInfo = StaticMethod.GetProjectInfo();
            InitForm();

            InitEvent();
        }

        public VScheduleSelector(EnumExecScheduleType frontPlanType)
        {
            InitializeComponent();

            this.tabPageTree.Visible = false;

            projectInfo = StaticMethod.GetProjectInfo();
            FrontPlanType = frontPlanType;

            InitForm();

            InitEvent();
        }

        private void InitForm()
        {
            InitFlexGrid(1);
            InitFlexGridM(10);


            txtColorFlag.BackColor = unFinishTaskColor;
            txtColorFlag1.BackColor = unFinishTaskColor;


            foreach (string state in Enum.GetNames(typeof(EnumScheduleDetailState)))
            {
                cbState.Items.Add(state);
                if (state == EnumScheduleDetailState.有效.ToString())
                    cbState.SelectedItem = state;
            }

        }

        private void InitWindowText(EnumExecScheduleType frontPlanType)
        {
            string str = "";
            switch (frontPlanType)
            {
                case EnumExecScheduleType.周进度计划:
                    str = "周进度计划";
                    break;
                case EnumExecScheduleType.月度进度计划:
                    str = "月度进度计划";
                    break;
                case EnumExecScheduleType.季度进度计划:
                    str = "季度进度计划";
                    break;
                case EnumExecScheduleType.年度进度计划:
                    str = "年度进度计划";
                    break;
                case EnumExecScheduleType.总体进度计划:
                    str = "总体进度计划";
                    break;
                default:
                    break;
            }
            this.Text = str + "引用";

        }
        private void InitComboBox(EnumExecScheduleType frontPlanType)
        {
            if (projectInfo == null)
            {
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ExecScheduleType", frontPlanType));

            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute)); //审核过的 状态为执行中
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

        }

       

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);

            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);

            cboSchedulePlanName.SelectedIndexChanged += new EventHandler(cboScheduleType_SelectedIndexChanged);

            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            this.Load += new EventHandler(VScheduleSelector_Load);



            //树方式
            btnSelectChildTask.Click += new EventHandler(btnSelectChildTask_Click);

            cbShowAllPlanDtl.Click += new EventHandler(cbShowAllPlanDtl_Click);

            cboScheduleName.SelectedIndexChanged += new EventHandler(cboScheduleName_SelectedIndexChanged);
            //cbScheduleVersion.SelectedIndexChanged += new EventHandler(cbScheduleVersion_SelectedIndexChanged);

            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);

            btnAddInSelectGrid.Click += new EventHandler(btnAddInSelectGrid_Click);
            btnAddLeftNodeInSelectGrid.Click += new EventHandler(btnAddLeftNodeInSelectGrid_Click);

            btnRemoveSelect.Click += new EventHandler(btnRemoveSelect_Click);
            btnRemoveAll.Click += new EventHandler(btnRemoveAll_Click);

            cbSelectTable.Click += new EventHandler(cbSelectTable_Click);
            cbSelectTree.Click += new EventHandler(cbSelectTree_Click);
        }


        //移除选择
        void btnRemoveSelect_Click(object sender, EventArgs e)
        {
            for (int i = gridPlanDtlTree.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = gridPlanDtlTree.Rows[i];
                if (row.Cells[colSelectTree.Name].Value != null && row.Cells[colSelectTree.Name].Value.ToString().ToLower() == "true")
                    gridPlanDtlTree.Rows.RemoveAt(i);
            }
        }

        //移除全部
        void btnRemoveAll_Click(object sender, EventArgs e)
        {
            gridPlanDtlTree.Rows.Clear();
        }

        #region 树方式


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

        //选择子任务
        void btnSelectChildTask_Click(object sender, EventArgs e)
        {
            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要操作的计划！");
                return;
            }
            else if (CurBillMaster.Details.Count == 0)
            {
                MessageBox.Show("当前计划尚没有计划明细！");
                return;
            }

            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;

            if (list != null && list.Count > 0)
            {
                GWBSTree selectTaskNode = list[0].Tag as GWBSTree;
                txtChildTask.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), selectTaskNode);

                WeekScheduleDetail selectPlanDtl = null;
                if (!IsContainTaskInPlanDtl(selectTaskNode, ref selectPlanDtl))
                {
                    MessageBox.Show("计划中不包含该任务！");
                    btnSelectChildTask.Focus();
                    return;
                }

                ShowPlanDtlInFlex(selectPlanDtl);

                cbShowAllPlanDtl.Checked = false;
            }
        }

        //显示全部计划
        void cbShowAllPlanDtl_Click(object sender, EventArgs e)
        {
            if (cbShowAllPlanDtl.Checked)
            {
                txtChildTask.Text = "";

                for (int i = 0; i < flexGrid.Rows; i++)
                {
                    flexGrid.Row(i).Visible = true;
                }
            }
        }

        void cboScheduleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scheduleName = cboScheduleName.SelectedItem as string;
            try
            {
                FlashScreen.Show("正在加载滚动进度计划......");

                CurBillMaster = null;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
                oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", scheduleName));
                oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
                //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleMaster), oq);

                //cbScheduleVersion.SelectedIndexChanged -= new EventHandler(cbScheduleVersion_SelectedIndexChanged);
                //cbScheduleVersion.Items.Clear();
                if (listMaster.Count > 0)
                {
                    for (int i = 0; i < listMaster.Count; i++)
                    {
                        WeekScheduleMaster item = listMaster[i] as WeekScheduleMaster;

                        //if (!string.IsNullOrEmpty(item.ScheduleName))
                        //{
                        //    cbScheduleVersion.Items.Add(item.ScheduleName);
                        //}
                    }

                    //默认显示执行状态的计划，如果没有则显示编辑状态的计划，否则显示第一个
                    IEnumerable<WeekScheduleMaster> listPlanMaster = listMaster.OfType<WeekScheduleMaster>();
                    var queryMaster = from m in listPlanMaster
                                      where m.DocState == DocumentState.InExecute
                                      select m;

                    if (queryMaster.Count() == 0)
                    {
                        queryMaster = from m in listPlanMaster
                                      where m.DocState == DocumentState.Edit
                                      select m;

                        if (queryMaster.Count() == 0)
                            queryMaster = listPlanMaster;
                    }

                    CurBillMaster = queryMaster.ElementAt(0);

                    //cbScheduleVersion.Text = CurBillMaster.ScheduleName;

                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("Id", CurBillMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList listPlan = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleMaster), oq);
                    CurBillMaster = listPlan[0] as WeekScheduleMaster;

                }

                //cbScheduleVersion.SelectedIndexChanged += new EventHandler(cbScheduleVersion_SelectedIndexChanged);

                if (CurBillMaster == null)
                {
                    ClearView();
                    return;
                }
                else
                {
                    //ChildRootNode = CurBillMaster.GetChildRootNode();
                    FreshMasterInfo(CurBillMaster);
                }

                FillFlex();


            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询进度计划出错。\n" + ex.Message);
                return;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        //void cbScheduleVersion_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cbScheduleVersion.Text.Trim() == "")
        //    {
        //        MessageBox.Show("请选择一个计划版本！");
        //        cbScheduleVersion.Focus();
        //        return;
        //    }

        //    //string planName = cboScheduleName.SelectedItem as string;
        //    WeekScheduleMaster master = cbScheduleVersion.SelectedItem as WeekScheduleMaster;
        //    try
        //    {
        //        FlashScreen.Show("正在加载滚动进度计划......");

        //        ObjectQuery oq = new ObjectQuery();
        //        //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
        //        //oq.AddCriterion(Expression.Eq("ScheduleType", this.enumScheduleType));
        //        //oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", planName));
        //        //oq.AddCriterion(Expression.Eq("ScheduleName", planVersion));

        //        oq.AddCriterion(Expression.Eq("Id", master.Id));
        //        oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

        //        IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleMaster), oq);

        //        if (listMaster.Count > 0)
        //            CurBillMaster = listMaster[0] as WeekScheduleMaster;
        //        else
        //        {
        //            CurBillMaster = null;
        //            ClearVersionData();
        //            return;
        //        }

        //        //ChildRootNode = CurBillMaster.GetChildRootNode();
        //        FreshMasterInfo(CurBillMaster);

        //        FillFlex();
        //    }
        //    catch (Exception ex)
        //    {
        //        FlashScreen.Close();
        //        MessageBox.Show("查询进度计划出错。\n" + ex.Message);
        //        return;
        //    }
        //    finally
        //    {
        //        FlashScreen.Close();
        //    }
        //}

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
        private void ShowPlanDtlInFlex(WeekScheduleDetail planDtl)
        {
            //var queryChild = from d in CurBillMaster.Details
            //                 where d.ParentNode == null || d.SysCode.IndexOf(planDtl.SysCode) > -1
            //                 select d;

            //List<string> listDtlIds = new List<string>();
            //foreach (WeekScheduleDetail dtl in queryChild)
            //{
            //    listDtlIds.Add(dtl.Id);
            //}

            //for (int i = 1; i < flexGrid.Rows; i++)
            //{
            //    string dtlId = flexGrid.Cell(i, 0).Tag;
            //    if (listDtlIds.Contains(dtlId))
            //        flexGrid.Row(i).Visible = true;
            //    else
            //        flexGrid.Row(i).Visible = false;
            //}
        }
        private bool IsContainTaskInPlanDtl(GWBSTree theGWBSNode, ref WeekScheduleDetail selectPlanDtl)
        {
            if (CurBillMaster == null)
                return false;

            foreach (WeekScheduleDetail dtl in CurBillMaster.Details)
            {
                if (dtl.GWBSTree != null && dtl.GWBSTree.Id == theGWBSNode.Id)
                {
                    selectPlanDtl = dtl;
                    return true;
                }
            }

            return false;
        }
        private void ClearView()
        {
            flexGrid.Rows = 1;

            txtChildTask.Text = "";
            txtChildTask.Tag = null;

            cbShowAllPlanDtl.Checked = false;

            //cbScheduleVersion.Items.Clear();
        }
        private void ClearVersionData()
        {
            flexGrid.Rows = 1;

            txtChildTask.Text = "";
            txtChildTask.Tag = null;

            cbShowAllPlanDtl.Checked = false;

        }
        private void FreshMasterInfo(WeekScheduleMaster master)
        {
            //cbScheduleVersion.Text = string.IsNullOrEmpty(master.ScheduleName) ? "" : master.ScheduleName;
        }
        private void FillFlex()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 5).Locked = true;

            detailHashtable.Clear();


            IList list = null;
            //if (!string.IsNullOrEmpty(CurBillMaster.Id))
            //    list = model.ProductionManagementSrv.GetChilds(CurBillMaster);

            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                foreach (WeekScheduleDetail detail in list)
                {
                    //if (detail.State == EnumScheduleDetailState.失效)
                    //{
                    //    flexGrid.Rows = flexGrid.Rows - 1;
                    //    continue;
                    //}

                    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                    detailHashtable.Add(detail.Id, detail);

                    //flexGrid.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    //FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    //rangeTemp.Merge();

                    //rangeTemp.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                    //flexGrid.Cell(rowIndex, detail.Level + 1).Text = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;

                    //if (detail.State == EnumScheduleDetailState.失效)
                    //    flexGrid.Cell(rowIndex, endImageCol + 1).ForeColor = Color.Red;
                    //else if (detail.State == EnumScheduleDetailState.有效)
                    //    flexGrid.Cell(rowIndex, endImageCol + 1).ForeColor = Color.Blue;

                    //flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.State.ToString();

                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }
                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 3).Text = detail.PlannedEndDate.ToShortDateString();
                    }

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

                    //工期计量单位                    
                    //flexGrid.Cell(rowIndex, endImageCol + 5).Text =detail.ScheduleUnit; //"";//22
                    flexGrid.Cell(rowIndex, endImageCol + 5).Text = "天";

                    ////计划说明
                    //flexGrid.Cell(rowIndex, endImageCol + 6).Text = detail.TaskDescript; //"计划说明";//27


                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 7).Text = detail.ActualBeginDate.ToShortDateString();
                    }
                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = "";
                    }
                    else
                    {
                        flexGrid.Cell(rowIndex, endImageCol + 8).Text = detail.ActualEndDate.ToShortDateString();
                    }
                    //实际工期
                    flexGrid.Cell(rowIndex, endImageCol + 9).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();

                    ////任务累积形象进度百分比
                    //flexGrid.Cell(rowIndex, endImageCol + 10).Text = StaticMethod.DecimelTrimEnd0(detail.AddupFigureProgress);


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
            range = flexGrid.Range(1, endImageCol + 1, flexGrid.Rows - 1, endImageCol + 10);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private void AddPlanDtlInSelectGrid(WeekScheduleDetail obj)
        {
            int rowIndex = gridPlanDtlTree.Rows.Add();
            gridPlanDtlTree.Rows[rowIndex].Tag = obj;

            gridPlanDtlTree[colSelectTree.Name, rowIndex].Value = true;

            //gridPlanDtlTree[colPlanVersionTree.Name, rowIndex].Value = obj.Master.ScheduleName;//计划版本

            //gridPlanDtlTree[colSelectState.Name, rowIndex].Value = obj.State;

            gridPlanDtlTree[colGWBSTreeNameTree.Name, rowIndex].Value = obj.GWBSTreeName;
            gridPlanDtlTree[colGWBSTreeNameTree.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), obj.GWBSTreeName, obj.GWBSTreeSysCode);

            //gridPlanDtlTree[colGWBSTreeNodeTypeTree.Name, rowIndex].Value = StaticMethod.GetNodeTypeStr(obj.GWBSNodeType);

            //gridPlanDtlTree[colFigureprogressTree.Name, rowIndex].Value = obj.AddupFigureProgress;

            gridPlanDtlTree[colPlannedBeginDateTree.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.PlannedBeginDate, false);
            gridPlanDtlTree[colPlannedEndDateTree.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.PlannedEndDate, false);
            gridPlanDtlTree[colPlannedDurationTree.Name, rowIndex].Value = obj.PlannedDuration;

            gridPlanDtlTree[colActualBeginDateTree.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualBeginDate, false);
            gridPlanDtlTree[colActualEndDateTree.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualEndDate, false);
            gridPlanDtlTree[colActualDurationTree.Name, rowIndex].Value = obj.ActualDuration;

            //gridPlanDtlTree[colUnitTree.Name, rowIndex].Value = obj.ScheduleUnit;
            //gridPlanDtlTree[colPlanDescTree.Name, rowIndex].Value = obj.Master.Descript;
            //gridPlanDtlTree[colDtlRemarkTree.Name, rowIndex].Value = obj.TaskDescript;

            //if (obj.AddupFigureProgress < 100)
            //{
            //    DataGridViewCellStyle style = new DataGridViewCellStyle();
            //    style.BackColor = unFinishTaskColor;
            //    gridPlanDtlTree.Rows[rowIndex].DefaultCellStyle = style;
            //}
        }

        void btnAddInSelectGrid_Click(object sender, EventArgs e)
        {
            if (flexGrid.ActiveCell == null || flexGrid.ActiveCell.Row == 0)
            {
                MessageBox.Show("请选择一个计划明细!");
                return;
            }

            int activeRowIndex = flexGrid.ActiveCell.Row;

            if (activeRowIndex == 1)
            {
                MessageBox.Show("不能选择计划根节点!");
                return;
            }

            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;

            //var queryDtl = from d in CurBillMaster.Details
            //               where
            //               d.Id == detailId
            //               && d.State == EnumScheduleDetailState.有效
            //               select d;

            //if (queryDtl.Count() > 0)
            //{
            //    WeekScheduleDetail dtl = queryDtl.ElementAt(0);
            //    if (IsExistsPlanDetail(dtl) == false)
            //    {
            //        ObjectQuery oq = new ObjectQuery();
            //        oq.AddCriterion(Expression.Eq("Id", dtl.Id));
            //        oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            //        dtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq)[0] as WeekScheduleDetail;

            //        dtl.Master = CurBillMaster;

            //        AddPlanDtlInSelectGrid(dtl);
            //    }
            //}
        }


        void btnAddLeftNodeInSelectGrid_Click(object sender, EventArgs e)
        {
            if (flexGrid.ActiveCell == null || flexGrid.ActiveCell.Row == 0)
            {
                MessageBox.Show("请选择一个计划明细!");
                return;
            }

            int activeRowIndex = flexGrid.ActiveCell.Row;

            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;

            var queryDtl = from d in CurBillMaster.Details
                           where d.Id == detailId
                           select d;


            //if (queryDtl.Count() > 0)
            //{
            //    WeekScheduleDetail dtl = queryDtl.ElementAt(0);

            //    queryDtl = from d in CurBillMaster.Details
            //               where
            //               d.State == EnumScheduleDetailState.有效
            //                   //&& d.GWBSNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode
            //               && d.SysCode.IndexOf(dtl.SysCode) > -1
            //               select d;


            //    bool flag = false;

            //    ObjectQuery oq = new ObjectQuery();
            //    Disjunction dis = new Disjunction();

            //    foreach (WeekScheduleDetail dtlNode in queryDtl)
            //    {
            //        if (IsExistsPlanDetail(dtlNode) == false)
            //        {
            //            dis.Add(Expression.Eq("Id", dtlNode.Id));
            //            flag = true;
            //        }
            //    }
            //    if (flag)
            //    {
            //        oq.AddCriterion(dis);
            //        oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);//引用的界面用到了项目任务属性
            //        IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
            //        foreach (WeekScheduleDetail dtlNode in listDtl)
            //        {
            //            dtlNode.Master = CurBillMaster;

            //            AddPlanDtlInSelectGrid(dtlNode);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 选择的计划明细是否存在
        /// </summary>
        /// <returns></returns>
        private bool IsExistsPlanDetail(WeekScheduleDetail dtl)
        {
            foreach (DataGridViewRow row in gridPlanDtlTree.Rows)
            {
                WeekScheduleDetail item = row.Tag as WeekScheduleDetail;
                if (item.Id == dtl.Id)
                    return true;
            }

            return false;
        }

        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows = rows;
            flexGrid.Cols = 30;//其中0列隐藏 1-19 为放置图片列 20-24为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);

            FlexCell.Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }
            flexGrid.Column(endImageCol + 1).Width = 30;//状态
            flexGrid.Column(endImageCol + 4).Width = 55;//计划工期
            flexGrid.Column(endImageCol + 9).Width = 55;//实际工期
            flexGrid.Column(endImageCol + 10).Width = 130;//任务累积形象进度百分比


            range = flexGrid.Range(0, startImageCol, 0, endImageCol);
            range.Merge();
            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            flexGrid.Cell(0, endImageCol + 1).Text = "状态";

            flexGrid.Cell(0, endImageCol + 2).Text = "计划开始时间";
            flexGrid.Cell(0, endImageCol + 3).Text = "计划结束时间";
            flexGrid.Cell(0, endImageCol + 4).Text = "计划工期";
            flexGrid.Cell(0, endImageCol + 5).Text = "工期计量单位";
            flexGrid.Cell(0, endImageCol + 6).Text = "计划说明";

            flexGrid.Cell(0, endImageCol + 7).Text = "实际开始时间";
            flexGrid.Cell(0, endImageCol + 8).Text = "实际结束时间";
            flexGrid.Cell(0, endImageCol + 9).Text = "实际工期";
            flexGrid.Cell(0, endImageCol + 10).Text = "任务累积形象进度百分比";




            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.TextBox;

            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 3).CellType = FlexCell.CellTypeEnum.Calendar;

            flexGrid.Column(endImageCol + 4).Mask = FlexCell.MaskEnum.Digital;

            flexGrid.Column(endImageCol + 5).CellType = FlexCell.CellTypeEnum.ComboBox;

            flexGrid.Column(endImageCol + 7).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 8).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 9).Mask = FlexCell.MaskEnum.Digital;
            flexGrid.Column(endImageCol + 10).Mask = FlexCell.MaskEnum.Digital;

            flexGrid.Column(endImageCol + 7).Locked = true;
            flexGrid.Column(endImageCol + 8).Locked = true;
            flexGrid.Column(endImageCol + 9).Locked = true;
            flexGrid.Column(endImageCol + 10).Locked = true;

            FlexCell.ComboBox cb = flexGrid.ComboBox(endImageCol + 5);
            flexGrid.ComboBox(endImageCol + 5).Locked = true;

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

        //树方式全选
        void cbSelectTree_Click(object sender, EventArgs e)
        {
            if (cbSelectTree.Checked)
            {
                foreach (DataGridViewRow var in this.gridPlanDtlTree.Rows)
                {
                    var.Cells[colSelectTree.Name].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow var in this.gridPlanDtlTree.Rows)
                {
                    var.Cells[colSelectTree.Name].Value = false;
                }
            }
        }

        #endregion


        void VScheduleSelector_Load(object sender, EventArgs e)
        {

            //this.WindowState = FormWindowState.Maximized;

            if (FrontPlanType == EnumExecScheduleType.周进度计划)
            {
                if (DefaultBeginDate != null)
                {
                    dtpDateBegin.Value = DefaultBeginDate.Value;

                    //dtpDateBegin.Value = DefaultBeginDate.Value.AddDays(-6);
                }
                if (DefaultEndDate != null)
                {
                    dtpDateEnd.Value = DefaultEndDate.Value;

                    //dtpDateEnd.Value = DefaultEndDate.Value.AddDays(6);
                }

                btnAddInSelectGrid.Visible = false;
            }
            else if (FrontPlanType == EnumExecScheduleType.月度进度计划)
            {
                if (DefaultBeginDate != null)
                    dtpDateBegin.Value = DefaultBeginDate.Value.AddDays(-1).AddDays(1);
                if (DefaultEndDate != null)
                    dtpDateEnd.Value = DefaultEndDate.Value.AddDays(1).AddDays(-1);
            }
        }

        void cboScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    string planName = cboSchedulePlanName.SelectedItem + "";

            //    cbPlanVersion.Text = "";
            //    cbPlanVersion.Items.Clear();

            //    EnumScheduleType enumScheduleType = EnumScheduleType.总滚动进度计划;

            //    ObjectQuery oq = new ObjectQuery();
            //    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //    oq.AddCriterion(Expression.Eq("ScheduleType", enumScheduleType));
            //    oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", planName));
            //    oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));

            //    IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleMaster), oq);

            //    cbPlanVersion.Items.Clear();
            //    if (listMaster.Count > 0)
            //    {
            //        for (int i = 0; i < listMaster.Count; i++)
            //        {
            //            WeekScheduleMaster item = listMaster[i] as WeekScheduleMaster;


            //            //if (!string.IsNullOrEmpty(item.ScheduleName))
            //            //    cbPlanVersion.Items.Add(item);

            //        }

            //        if (cbPlanVersion.Items.Count > 0)
            //            cbPlanVersion.SelectedIndex = 0;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            //}
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSelect.Name)
            {
                dgDetail.EndEdit();
            }
        }

        void dgDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (cell.OwningColumn.Name == colSelect.Name)
            {
                bool selected = (bool)cell.Value;
                if (selected)
                {
                    totalRecords += 1;
                }
                else
                {
                    totalRecords -= 1;
                }
                lblRecordTotal.Text = string.Format("共选择【{0}】条记录", totalRecords);
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                bool isSelected = (bool)var.Cells[colSelect.Name].Value;
                var.Cells[colSelect.Name].Value = !isSelected;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                var.Cells[colSelect.Name].Value = true;
            }
        }

        //表方式全选
        void cbSelectTable_Click(object sender, EventArgs e)
        {
            if (cbSelectTable.Checked)
            {
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    var.Cells[colSelect.Name].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    var.Cells[colSelect.Name].Value = false;
                }

                //foreach (DataGridViewRow var in this.dgDetail.Rows)
                //{
                //    bool isSelected = (bool)var.Cells[colSelect.Name].Value;
                //    var.Cells[colSelect.Name].Value = !isSelected;
                //}
            }
        }

        //确定
        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();

            int selectCountRecord = 0;

            if (tabControl1.SelectedTab == tabPageTable)//表方式
            {
                foreach (DataGridViewRow row in this.dgDetail.Rows)
                {
                    if (row.IsNewRow)
                        break;
                    object objColSelect = row.Cells[colSelect.Name].Value;
                    if (objColSelect != null && (bool)objColSelect)
                    {
                        WeekScheduleDetail dtl = row.Tag as WeekScheduleDetail;

                        if (DefaultBeginDate == null || DefaultEndDate == null)
                            result.Add(dtl);
                        else if ((dtl.PlannedBeginDate <= DefaultBeginDate && DefaultBeginDate <= dtl.PlannedEndDate)
                            || (dtl.PlannedBeginDate <= DefaultEndDate && DefaultEndDate <= dtl.PlannedEndDate)
                            || (dtl.PlannedBeginDate > DefaultBeginDate && dtl.PlannedEndDate < DefaultEndDate)
                            || (dtl.PlannedEndDate < DefaultBeginDate && dtl.TaskCompletedPercent < 100))//“时间有交集”或者“滚动计划时间在前且任务未完工”

                            if (dtl.State == DocumentState.InExecute)
                                result.Add(dtl);

                        selectCountRecord += 1;
                    }
                }

                if (result.Count > 0)
                {
                    ObjectQuery oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    foreach (WeekScheduleDetail dtl in result)
                    {
                        dis.Add(Expression.Eq("Id", dtl.Id));
                    }
                    oq.AddCriterion(dis);
                    oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                    IList listTempResult = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);

                    WeekScheduleMaster master = (result[0] as WeekScheduleDetail).Master;
                    foreach (WeekScheduleDetail d in listTempResult)
                    {
                        d.Master = master;
                    }
                    result = listTempResult;
                }
            }
            else
            {
                foreach (DataGridViewRow row in this.gridPlanDtlTree.Rows)
                {
                    object objColSelect = row.Cells[colSelectTree.Name].Value;
                    if (objColSelect != null && (bool)objColSelect)
                    {
                        WeekScheduleDetail dtl = row.Tag as WeekScheduleDetail;

                        if (DefaultBeginDate == null || DefaultEndDate == null)
                            result.Add(dtl);
                        else if (((dtl.PlannedBeginDate <= DefaultBeginDate && DefaultBeginDate <= dtl.PlannedEndDate)
                            || (dtl.PlannedBeginDate <= DefaultEndDate && DefaultEndDate <= dtl.PlannedEndDate)
                            || (dtl.PlannedBeginDate > DefaultBeginDate && dtl.PlannedEndDate < DefaultEndDate)
                            || (dtl.PlannedEndDate < DefaultBeginDate && dtl.TaskCompletedPercent < 100)))//“时间有交集”或者“滚动计划时间在前且任务未完工”

                            if (dtl.State == DocumentState.InExecute)
                                result.Add(dtl);

                        selectCountRecord += 1;
                    }
                }
            }

            if (selectCountRecord == 0)
            {
                MessageBox.Show("没有选中记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (result.Count == 0)
            {
                MessageBox.Show("选择中不存在“有效”状态的滚动计划,请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //MessageBox.Show("当前编制的" + FrontPlanType + "时间范围和选择的滚动计划时间范围不存在任何交集或任务已完工，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.btnOK.FindForm().Close();
        }

        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
            totalRecords = 0;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {

            WeekScheduleMaster master = cboSchedulePlanName.SelectedItem as WeekScheduleMaster;

            try
            {

                FlashScreen.Show("正在查询滚动进度计划,请稍候......");

                ObjectQuery oq = new ObjectQuery();

                oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
                oq.AddCriterion(Expression.Not(Expression.Eq("PlannedBeginDate", new DateTime(1900, 1, 1))));
                oq.AddCriterion(Expression.Not(Expression.Eq("PlannedEndDate", new DateTime(1900, 1, 1))));

                oq.AddCriterion(Expression.Eq("NodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)); //所有的叶节点


                if (cbState.Text.Trim() != "")
                    oq.AddCriterion(Expression.Eq("State", VirtualMachine.Component.Util.EnumUtil<EnumScheduleDetailState>.FromDescription(cbState.Text.Trim())));

                //if (FrontPlanType == EnumExecScheduleType.周进度计划)
                //    oq.AddCriterion(Expression.Eq("GWBSNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode));

                //oq.AddCriterion(Expression.Lt("AddupFigureProgress", (decimal)1));//未完工
                //oq.AddCriterion(Expression.Not(Expression.Eq("Level", 1)));//不是根节点

                //if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
                //{
                //    oq.AddCriterion(Expression.Eq("Master.HandlePerson.Id", (txtHandlePerson.Result[0] as PersonInfo).Id));
                //}
                //if (txtGWBSName.Text != "")
                //{
                //    oq.AddCriterion(Expression.Like("GWBSTreeName", txtGWBSName.Text, MatchMode.Anywhere));
                //}

                WeekScheduleDetail d = new WeekScheduleDetail();

                //时间筛选
                DateTime beginTime = dtpDateBegin.Value.Date;
                DateTime endTime = dtpDateEnd.Value.Date;
                //oq.AddCriterion(Expression.Ge("PlannedBeginDate", beginTime));
                //oq.AddCriterion(Expression.Lt("PlannedBeginDate", endTime));

                Disjunction dis = new Disjunction();
                dis.Add(Expression.And(Expression.Le("PlannedBeginDate", beginTime), Expression.Ge("PlannedEndDate", beginTime)));//周（月）计划开始时间和滚动计划时间有交集
                dis.Add(Expression.And(Expression.Le("PlannedBeginDate", endTime), Expression.Ge("PlannedEndDate", endTime)));//周（月）计划结束时间和滚动计划时间有交集
                dis.Add(Expression.And(Expression.Gt("PlannedBeginDate", beginTime), Expression.Lt("PlannedEndDate", endTime)));//周（月）计划时间包含滚动计划时间
                //dis.Add(Expression.Lt("PlannedEndDate", beginTime));//周（月）计划起始时间大于滚动计划结束时间且形象进度小于1

                oq.AddCriterion(dis);


                oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);

                oq.AddOrder(Order.Asc("PlannedBeginDate"));


                //IList list = model.ProductionManagementSrv.GetScrollPlanDtl(oq, projectInfo.Id);
                IList list = model.ProductionManagementSrv.GetFrontSchedule(oq, projectInfo.Id);
                //ShowMasterList(list);
                FillFlex(list);
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询数据出错。\n" +  ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList list)
        {
            dgDetail.Rows.Clear();
            if (list == null || list.Count == 0)
                return;

            foreach (WeekScheduleDetail obj in list)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = obj;

                //dgDetail[colPlanVersion.Name, rowIndex].Value = obj.Master.ScheduleName;//计划版本

                dgDetail[colPlanState.Name, rowIndex].Value = obj.State;//计划状态

                dgDetail[colGWBSTreeName.Name, rowIndex].Value = obj.GWBSTreeName;
                if (obj.GWBSTree != null)
                    dgDetail[colGWBSTreeName.Name, rowIndex].ToolTipText = obj.GWBSTree.FullPath; //StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), obj.GWBSTreeName, obj.GWBSTreeSysCode);

                //dgDetail[colGWBSTreeNodeType.Name, rowIndex].Value = StaticMethod.GetNodeTypeStr(obj.GWBSNodeType);

                //dgDetail[colFigureprogress.Name, rowIndex].Value = obj.AddupFigureProgress;

                dgDetail[colPlannedBeginDate.Name, rowIndex].Value = obj.PlannedBeginDate.ToShortDateString();
                dgDetail[colPlannedEndDate.Name, rowIndex].Value = obj.PlannedEndDate.ToShortDateString();
                dgDetail[colPlannedDuration.Name, rowIndex].Value = obj.PlannedDuration;

                dgDetail[colActualBeginDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualBeginDate, false);
                dgDetail[colActualEndDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualEndDate, false);
                dgDetail[colActualDuration.Name, rowIndex].Value = obj.ActualDuration;

                //dgDetail[colUnit.Name, rowIndex].Value = obj.ScheduleUnit;
                //dgDetail[colPlanDesc.Name, rowIndex].Value = obj.Master.Descript;
                //dgDetail[colDtlRemark.Name, rowIndex].Value = obj.TaskDescript;

                //if (obj.AddupFigureProgress < 100 && obj.PlannedEndDate < DateTime.Now.Date)
                //{
                //    DataGridViewCellStyle style = new DataGridViewCellStyle();
                //    style.BackColor = unFinishTaskColor;
                //    dgDetail.Rows[rowIndex].DefaultCellStyle = style;
                //}

            }
        }


        private void InitFlexGridM(int rows)
        {
            flexGridM.AutoRedraw = false;
            flexGridM.Column(0).Visible = true;

            flexGridM.Rows = rows;
            flexGridM.Cols = 28;//其中0列隐藏 1-19 为放置图片列 20-24为数据列

            flexGridM.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGridM.ExtendLastCol = true;
            flexGridM.DisplayFocusRect = false;
            flexGridM.LockButton = true;
            flexGridM.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGridM.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGridM.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            flexGridM.BackColorBkg = SystemColors.Control;
            flexGridM.DefaultFont = new Font("Tahoma", 8);
            flexGridM.DisplayRowArrow = true;
            flexGridM.DisplayRowNumber = true;

            FlexCell.Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGridM.Column(i).TabStop = false;
                flexGridM.Column(i).Width = 20;
            }
            flexGridM.Column(endImageCol + 5).Width = 300;//计划说明

            for (int i = 0; i < rows; i++)
            {
                range = flexGridM.Range(i, startImageCol, i, endImageCol);
                range.Merge();

                flexGridM.Cell(i, endImageCol + 3).Text = "天";
            }

            // 加载图片
            flexGridM.Images.Add(Resources.ImageExpend, imageExpand);
            flexGridM.Images.Add(Resources.ImageFold, imageCollapse);

            flexGridM.Cell(0, 1).Text = "任务名称";
            flexGridM.Cell(0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;

            flexGridM.Cell(0, endImageCol + 1).Text = "计划开始时间";//20
            flexGridM.Cell(0, endImageCol + 2).Text = "计划结束时间";//21
            flexGridM.Cell(0, endImageCol + 3).Text = "计划工期";//22
            flexGridM.Cell(0, endImageCol + 4).Text = "工期计量单位";//23
            flexGridM.Cell(0, endImageCol + 5).Text = "计划说明";//27
            flexGridM.Cell(0, endImageCol + 6).Text = "实际开始时间";//24
            flexGridM.Cell(0, endImageCol + 7).Text = "实际结束时间";//25
            flexGridM.Cell(0, endImageCol + 8).Text = "实际工期";//26

            flexGridM.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGridM.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGridM.Column(endImageCol + 3).Mask = FlexCell.MaskEnum.Digital;
            flexGridM.Column(endImageCol + 6).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGridM.Column(endImageCol + 7).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGridM.Column(endImageCol + 8).Mask = FlexCell.MaskEnum.Digital;

            flexGridM.Column(endImageCol + 6).Locked = true;
            flexGridM.Column(endImageCol + 7).Locked = true;
            flexGridM.Column(endImageCol + 8).Locked = true;

            // Refresh
            flexGridM.AutoRedraw = true;
            flexGridM.Refresh();
        }


        private void FillFlex(IList list_leaf)
        {

            WeekScheduleMaster master = cboSchedulePlanName.SelectedItem as WeekScheduleMaster;
            //detailHashtable.Clear();

            flexGridM.AutoRedraw = false;
            flexGridM.Rows = 1;
            flexGridM.Column(endImageCol + 3).Locked = true;

            IList list_tree = null;
            if (!string.IsNullOrEmpty(master.Id))
            {
                list_tree = model.ProductionManagementSrv.GetWeekChilds(master);
            }

            IEnumerable<WeekScheduleDetail> list = GetTheTreeWithNeedLeaf(list_leaf,list_tree);
            if (list != null && list.Count() > 0)
            {
                flexGridM.Rows = list.Count() + 1;
                int rowIndex = 1;
                //listDtlIds.Clear();
                //master.Details.Clear();
                foreach (WeekScheduleDetail detail in list)
                {
                    detail.RowIndex = rowIndex;

                    //listDtlIds.Add(detail.Id);
                    //detailHashtable.Add(detail.Id, detail);
                    //master.Details.Add(detail);

                    flexGridM.Cell(rowIndex, 0).Tag = detail.Id;
                    if (detail.ChildCount > 0)
                    {
                        flexGridM.Cell(rowIndex, detail.Level).SetImage(imageCollapse);
                    }

                    //SetRowBold(rowIndex, detail.ChildCount > 0);

                   
                    FlexCell.Range rangeTemp = flexGridM.Range(rowIndex, detail.Level + 1, rowIndex, endImageCol);
                    rangeTemp.Merge();

                    flexGridM.Cell(rowIndex, detail.Level + 1).Text = detail.GWBSTreeName ?? projectInfo.Name + "总进度计划";

                    //行是否只读
                    flexGridM.Row(rowIndex).Locked = detail.ChildCount > 0 || detail.State != DocumentState.Edit;

                    //计划开始时间
                    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 1).Text = ""; //"计划开始时间";//20
                    }
                    else
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                    }

                    //计划结束时间
                    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                    }
                    else
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                    }

                    //计划工期
                    if (detail.PlannedBeginDate != (new DateTime(1900, 1, 1)) && detail.PlannedEndDate != (new DateTime(1900, 1, 1)) && detail.PlannedBeginDate <= detail.PlannedEndDate)
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 3).Text = ((detail.PlannedEndDate - detail.PlannedBeginDate).Days + 1).ToString();
                    }
                    else
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 3).Text = "";
                    }
                    flexGridM.Cell(rowIndex, endImageCol + 3).Alignment = FlexCell.AlignmentEnum.CenterCenter;

                    //工期计量单位                    
                    flexGridM.Cell(rowIndex, endImageCol + 4).Text = detail.ScheduleUnit;
                    flexGridM.Cell(rowIndex, endImageCol + 4).Alignment = FlexCell.AlignmentEnum.CenterCenter;

                    //计划说明
                    flexGridM.Cell(rowIndex, endImageCol + 5).Text = detail.Descript; //"计划说明";//27
                    flexGridM.Cell(rowIndex, endImageCol + 5).Locked = false;

                    //实际开始时间
                    if (detail.ActualBeginDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 6).Text = "";
                    }
                    else
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 6).Text = detail.ActualBeginDate.ToShortDateString();
                    }

                    //实际结束时间
                    if (detail.ActualEndDate == (new DateTime(1900, 1, 1)))
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 7).Text = "";
                    }
                    else
                    {
                        flexGridM.Cell(rowIndex, endImageCol + 7).Text = detail.ActualEndDate.ToShortDateString();
                    }

                    //实际工期
                    flexGridM.Cell(rowIndex, endImageCol + 8).Text = detail.ActualDuration == 0 ? "" : detail.ActualDuration.ToString();

                    rowIndex = rowIndex + 1;
                }
            }

            //1-19列的背景色
            FlexCell.Range range = flexGridM.Range(1, startImageCol, flexGridM.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            //设置计划实际信息列的背景色
            range = flexGridM.Range(1, endImageCol + 6, flexGridM.Rows - 1, endImageCol + 8);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            flexGridM.AutoRedraw = true;
            flexGridM.Refresh();

            //lbRowCount.Text = string.Format("共计 {0} 行", flexGridM.Rows);
        }

        /// <summary>
        /// 获取时间内有效的任务树
        /// </summary>
        /// <param name="list_leaf">需要保留的叶子</param>
        /// <param name="list_tree">前驱任务树</param>
        private IEnumerable<WeekScheduleDetail> GetTheTreeWithNeedLeaf(IList list_leaf, IList list_tree)
        {
            if (list_leaf == null || list_leaf.Count == 0 || list_tree == null || list_tree.Count == 0)
                return null;

            var list_allleaf = list_tree.OfType<WeekScheduleDetail>().Where(d => d.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode);

            var l_leaf = from m in list_leaf.OfType<WeekScheduleDetail>()
                         select m.Id;
           

            var ll = list_allleaf.Where<WeekScheduleDetail>(m => !l_leaf.Contains(m.Id));

      

            var l_rtn = list_tree.OfType<WeekScheduleDetail>().Where(d => !ll.Contains(d));

            return l_rtn;
        }

    }
}
