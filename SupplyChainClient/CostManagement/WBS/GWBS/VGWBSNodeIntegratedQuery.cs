using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.Web.UI.WebControls;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using System.Windows.Documents;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSNodeIntegratedQuery : TBasicDataView
    {
        MGWBSTree model = new MGWBSTree();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        public VGWBSNodeIntegratedQuery()
        {
            InitializeComponent();
            InitDate();
            InitEvent();
        }
        void InitDate()
        {
            //任务级别
            foreach (string level in Enum.GetNames(typeof(ProjectTaskTypeLevel)))
            {
                cmbTaskLevel.Items.Add(level);
            }
            //遵循标准
            foreach (string standard in Enum.GetNames(typeof(ProjectTaskTypeStandard)))
            {
                cmbStandard.Items.Add(standard);
            }
            //状态
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = StaticMethod.GetWBSTaskStateText(state);
                li.Value = ((int)state).ToString();

                cmbState.Items.Add(li);
            }
            //任务标志
            //this.cmbTaskFlag.Items.AddRange(new object[] { "责任节点", "核算节点", "生产确定节点" });
            this.clbTaskFlag.Items.AddRange(new object[] { "成本核算节点", "生产确认节点", "责任核算节点" });
            //是否叶节点
            this.cmbIsLeafNode.Items.AddRange(new object[] { "是", "否" });
            object[] o = new object[] { txtTaskType, txtProjectName };
            ObjectLock.Lock(o);
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            txtProjectName.Text = projectInfo.Name;
            txtTaskName.Focus();

            tvwWBSTree.Visible = false;

        }
        void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSelectTaskType.Click += new EventHandler(btnSelectTaskType_Click);
            this.btnWBSTreeShow.Click += new EventHandler(btnWBSTreeShow_Click);
            this.btnBackGWBSList.Click += new EventHandler(btnBackGWBSList_Click);
            dgWBSShow.CellDoubleClick += new DataGridViewCellEventHandler(dgWBSShow_CellDoubleClick);

            this.btnBusinessStatistics.Click += new EventHandler(btnBusinessStatistics_Click);
            this.btnEngineeringStatistics.Click += new EventHandler(btnEngineeringStatistics_Click);
            this.btnEmpty.Click += new EventHandler(btnEmpty_Click);
        }

        /// <summary>
        /// 清除查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnEmpty_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要清除不可编辑的查询条件", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                txtTaskType.Text = "";
                txtTaskType.Tag = null;
                txtHandlePerson.Text = "";
            }
        }

        /// <summary>
        /// 选择任务类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectTaskType_Click(object sender, EventArgs e)
        {
            MWBSManagement mo = new MWBSManagement();
            VSelectWBSProjectTaskType ptt = new VSelectWBSProjectTaskType(mo);
            ptt.ShowDialog();
            if (ptt.SelectResult != null && ptt.SelectResult.Count > 0)
            {
                ProjectTaskTypeTree type = ptt.SelectResult[0].Tag as ProjectTaskTypeTree;
                txtTaskType.Text = type.Name;
                txtTaskType.Tag = type;
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            dgWBSShow.Visible = true;
            tvwWBSTree.Visible = false;
            try
            {


                FlashScreen.Show("正在查询加载数据，请稍候......");

                ProjectTaskTypeLevel level = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeLevel>.FromDescription(cmbTaskLevel.Text);
                ProjectTaskTypeStandard standard = VirtualMachine.Component.Util.EnumUtil<ProjectTaskTypeStandard>.FromDescription(cmbStandard.Text);
                DocumentState state = 0;
                if (cmbState.SelectedItem != null)
                {
                    System.Web.UI.WebControls.ListItem li = cmbState.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int stateValue = Convert.ToInt32(li.Value);
                    state = (DocumentState)stateValue;
                }

                //GWBSTree s = new GWBSTree();
                //s.ProjectTaskTypeGUID

                ObjectQuery oq = new ObjectQuery();
                if (txtProjectName.Text != "")
                {
                    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                }
                if (txtTaskName.Text != "")
                {
                    oq.AddCriterion(Expression.Like("Name", txtTaskName.Text, MatchMode.Anywhere));
                }
                if (txtTaskType.Text != "")
                {
                    ProjectTaskTypeTree type = txtTaskType.Tag as ProjectTaskTypeTree;
                    oq.AddCriterion(Expression.Eq("ProjectTaskTypeGUID.Id", type.Id));
                }
                if (level != ProjectTaskTypeLevel.项目 || (level == ProjectTaskTypeLevel.项目 && cmbTaskLevel.Text.Trim() == "项目"))
                {
                    oq.AddCriterion(Expression.Eq("ProjectTaskTypeGUID.TypeLevel", level));
                }
                if (standard != 0)
                {
                    oq.AddCriterion(Expression.Eq("ProjectTaskTypeGUID.TypeStandard", standard));
                }
                if (state != 0)
                {
                    oq.AddCriterion(Expression.Eq("TaskState", state));
                }
                for (int i = 0; i < clbTaskFlag.Items.Count; i++)
                {
                    if (clbTaskFlag.GetItemChecked(i))
                    {
                        string flag = clbTaskFlag.Items[i].ToString();
                        if (flag == "成本核算节点")
                        {
                            oq.AddCriterion(Expression.Eq("CostAccFlag", true));
                        }
                        if (flag == "生产确认节点")
                        {
                            oq.AddCriterion(Expression.Eq("ProductConfirmFlag", true));
                        }
                        if (flag == "责任核算节点")
                        {
                            oq.AddCriterion(Expression.Eq("ResponsibleAccFlag", true));
                        }
                    }
                }
                if (cmbIsLeafNode.Text == "是")
                {
                    oq.AddCriterion(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode));
                }
                if (cmbIsLeafNode.Text == "否")
                {
                    oq.AddCriterion(Expression.Not(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)));
                }
                if (txtHandlePerson.Text != "")
                {
                    IList personList = txtHandlePerson.Result;
                    if (personList != null && personList.Count > 0)
                    {
                        PersonInfo owner = personList[0] as PersonInfo;
                        oq.AddCriterion(Expression.Eq("OwnerGUID", owner.Id));
                    }
                }
                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                IList wbsList = model.ObjectQuery(typeof(GWBSTree), oq);
                //IList wbsList = model.GetGWBSTreeAndFullPath(oq);
                dgWBSShow.Rows.Clear();

                if (wbsList != null && wbsList.Count > 0)
                {
                    #region
                    foreach (GWBSTree wbs in wbsList)
                    {
                        int rowIndex = dgWBSShow.Rows.Add();

                        //string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
                        dgWBSShow[colTaskName.Name, rowIndex].ToolTipText = wbs.FullPath;

                        dgWBSShow[colTaskName.Name, rowIndex].Value = wbs.Name;
                        ProjectTaskTypeTree type = wbs.ProjectTaskTypeGUID;
                        dgWBSShow[colTaskLevel.Name, rowIndex].Value = type.TypeLevel.ToString(); //wbs.ProjectTaskTypeGUID.TypeLevel.ToString();
                        dgWBSShow[colTaskType.Name, rowIndex].Value = wbs.ProjectTaskTypeName;
                        //dgWBSShow[colCheckClaim.Name,rowIndex].Value = wbs.

                        #region 检查要求（注释）
                        //string checkRequireShow = string.Empty;
                        ////string checkClaim = wbs.CheckRequire;
                        //string dailyCheckState = wbs.DailyCheckState;
                        //string[] checkRequireName = new string[] { "工长质检", "质检员质检", "监理质检", "工程进度", "安全专业", "物资专业", "技术专业", "", "", "", "", "工程量确认" };
                        //int index = 0;
                        //foreach (char c in checkClaim)
                        //{
                        //    if (index <= 6)
                        //    {
                        //        switch (c)
                        //        {
                        //            case '0':
                        //                checkRequireShow += (checkRequireName[index] + "未检查" + "/");
                        //                break;
                        //            case '1':
                        //                checkRequireShow += (checkRequireName[index] + "检查通过" + "/");
                        //                break;
                        //            case '2':
                        //                checkRequireShow += (checkRequireName[index] + "罚款后检查通过" + "/");
                        //                break;
                        //            case '3':
                        //                checkRequireShow += (checkRequireName[index] + "检查未通过" + "/");
                        //                break;
                        //            case '4':
                        //                checkRequireShow += (checkRequireName[index] + "检查中" + "/");
                        //                break;
                        //            case 'X':
                        //                checkRequireShow += (checkRequireName[index] + "无需检查" + "/");
                        //                break;
                        //        }
                        //    }
                        //    if (index == 11)
                        //    {
                        //        if (c == '0')
                        //        {
                        //            checkRequireShow += (checkRequireName[index] + "未确认");
                        //        }
                        //        if (c == '1')
                        //        {
                        //            checkRequireShow += (checkRequireName[index] + "确认");
                        //        }
                        //    }
                        //    index++;
                        //}
                        #endregion

                        #region 日常检查状态
                        if (wbs.CategoryNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                        {
                            dgWBSShow[colDailyCheckState.Name, rowIndex].Value = StaticMethod.GetCheckStateShowText(wbs.DailyCheckState);

                            dgWBSShow[colAcceptanceCheckState.Name, rowIndex].Value = Enum.GetName(typeof(AcceptanceCheckStateEnum), wbs.AcceptanceCheckState);
                            dgWBSShow[colSuperiorCheckState.Name, rowIndex].Value = Enum.GetName(typeof(SuperiorCheckStateEnum), wbs.SuperiorCheckState); //wbs.SuperiorCheckState;
                        }
                        #endregion

                        dgWBSShow[colStandard.Name, rowIndex].Value = type.TypeStandard.ToString();//wbs.ProjectTaskTypeGUID.TypeStandard.ToString();
                        dgWBSShow[colResponsiblePerson.Name, rowIndex].Value = wbs.OwnerName;
                        dgWBSShow[colState.Name, rowIndex].Value = StaticMethod.GetWBSTaskStateText(wbs.TaskState);
                        dgWBSShow[colCostAccFlag.Name, rowIndex].Value = wbs.CostAccFlag ? "是" : "否";
                        dgWBSShow[colResponsibleAccFlag.Name, rowIndex].Value = wbs.ResponsibleAccFlag ? "是" : "否";
                        dgWBSShow[colProductConfirmFlag.Name, rowIndex].Value = wbs.ProductConfirmFlag ? "是" : "否";
                        dgWBSShow.Rows[rowIndex].Tag = wbs;
                    }
                    #endregion
                }
                else
                {
                    FlashScreen.Close();
                    MessageBox.Show("未找到数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        /// <summary>
        /// 双击任务显示任务明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgWBSShow_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                GWBSTree wbs = dgWBSShow.Rows[e.RowIndex].Tag as GWBSTree;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheGWBS.Id", wbs.Id));
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
                IList list = model.ObjectQuery(typeof(GWBSDetail), oq);
                if (list != null && list.Count > 0)
                {
                    VGWBSDetailShow detailShow = new VGWBSDetailShow(list);
                    detailShow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("其下没有任务明细！");
                }

            }
        }

        /// <summary>
        /// 树方式显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnWBSTreeShow_Click(object sender, EventArgs e)
        {
            tvwWBSTree.Visible = true;
            dgWBSShow.Visible = false;
            if (dgWBSShow.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwWBSTree.Nodes.Clear();
                GWBSTree wbs = dgWBSShow.CurrentRow.Tag as GWBSTree;
                IList list = model.GetGWBSTreeByParentNodeSyscode(wbs.SysCode);
                if (list != null && list.Count > 0)
                {
                    foreach (GWBSTree childNode in list)
                    {
                        if (childNode.State == 0)
                            continue;
                        System.Windows.Forms.TreeNode tnTmp = new System.Windows.Forms.TreeNode();
                        tnTmp.Name = childNode.Id.ToString();
                        tnTmp.Text = childNode.Name;
                        tnTmp.Tag = childNode;
                        if (childNode.ParentNode != null && childNode.Id != wbs.Id)
                        {
                            System.Windows.Forms.TreeNode tn = null;
                            tn = hashtable[childNode.ParentNode.Id.ToString()] as System.Windows.Forms.TreeNode;
                            if (tn != null)
                                tn.Nodes.Add(tnTmp);
                        }
                        else
                        {
                            tvwWBSTree.Nodes.Add(tnTmp);
                        }
                        hashtable.Add(tnTmp.Name, tnTmp);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(ex));
            }

        }

        /// <summary>
        /// 返回查询列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnBackGWBSList_Click(object sender, EventArgs e)
        {
            dgWBSShow.Visible = true;
            tvwWBSTree.Visible = false;
            tvwWBSTree.Nodes.Clear();
        }

        /// <summary>
        /// 商务数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnBusinessStatistics_Click(object sender, EventArgs e)
        {
            if (dgWBSShow.RowCount == 0)
            {
                MessageBox.Show("未有选中数据！");
            }
            else
            {
                GWBSTree wbs = new GWBSTree();
                if (dgWBSShow.Visible == true)
                {
                    wbs = dgWBSShow.CurrentRow.Tag as GWBSTree;
                }
                else
                {
                    wbs = tvwWBSTree.SelectedNode.Tag as GWBSTree;
                }
                VGWBSBusinessStatistics bs = new VGWBSBusinessStatistics(wbs);
                bs.ShowDialog();
            }
        }
        /// <summary>
        /// 工程数据统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnEngineeringStatistics_Click(object sender, EventArgs e)
        {
            if (dgWBSShow.RowCount == 0)
            {
                MessageBox.Show("未有选中数据！");
            }
            else
            {
                GWBSTree wbs = new GWBSTree();
                if (dgWBSShow.Visible == true)
                {
                    wbs = dgWBSShow.CurrentRow.Tag as GWBSTree;
                }
                else
                {
                    wbs = tvwWBSTree.SelectedNode.Tag as GWBSTree;
                }
                VGWBSEngineeringStatistics es = new VGWBSEngineeringStatistics(wbs);
                es.ShowDialog();
            }
        }




    }
}
