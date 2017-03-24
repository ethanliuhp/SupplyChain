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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VResourcesDemandManagement : TBasicDataView
    {
        MRollingDemandPlan model = new MRollingDemandPlan();
        private CurrentProjectInfo projectInfo = null;
        private PersonInfo loginPerson = null;

        public VResourcesDemandManagement()
        {
            InitializeComponent();
            InitData();
        }
        void InitData()
        {
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            loginPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
            txtProjectInfoName.Text = projectInfo.Name;
            Load_cmb();
            ShowRollingDemandPlan();

            InitEvents();
        }

        void Load_cmb()
        {
            IList planTypeList = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ResourceRequirePlanTypeCate);
            if (planTypeList != null)
            {
                foreach (BasicDataOptr bdo in planTypeList)
                {
                    cmbPlanType.Items.Add(bdo.BasicName);
                }
            }

            foreach (string state in Enum.GetNames(typeof(ResourceRequirePlanState)))
            {
                cmbState.Items.Add(state);
            }

        }
        void InitEvents()
        {
            btnFilter.Click += new EventHandler(btnFilter_Click);
            gridRollingDemandPlan.CellClick += new DataGridViewCellEventHandler(gridRollingDemandPlan_CellClick);
            gridRollingDemandPlan.CellMouseClick += new DataGridViewCellMouseEventHandler(gridRollingDemandPlan_CellMouseClick);
            gridResourceRequireReceipt.CellMouseClick += new DataGridViewCellMouseEventHandler(gridResourceRequireReceipt_CellMouseClick);
            mnuRollingDemandPlan.ItemClicked += new ToolStripItemClickedEventHandler(mnuRollingDemandPlan_ItemClicked);
            gridRollingDemandPlan.MouseClick += new MouseEventHandler(gridRollingDemandPlan_MouseClick);
            mnuResourceRequireReceipt.ItemClicked += new ToolStripItemClickedEventHandler(mnuResourceRequireReceipt_ItemClicked);
            gridResourceRequireReceipt.MouseClick += new MouseEventHandler(gridResourceRequireReceipt_MouseClick);
        }

        void gridRollingDemandPlan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ResourceRequirePlan rrp = gridRollingDemandPlan.Rows[e.RowIndex].Tag as ResourceRequirePlan;
                ShowResourceRequireReceipt(rrp);
            }
        }

        /// <summary>
        /// 过滤需求计划的单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                ResourceRequirePlan rrp = gridRollingDemandPlan.CurrentRow.Tag as ResourceRequirePlan;
                ShowResourceRequireReceipt(rrp);
            }
            catch (Exception)
            {
                MessageBox.Show("请选择一条滚动需求计划");
            }

        }
        /// <summary>
        /// 加载滚动资源需求计划
        /// </summary>
        void ShowRollingDemandPlan()
        {
            ResourceRequirePlan p = new ResourceRequirePlan();
            //p.ProjectId
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //oq.AddCriterion(Expression.Not(Expression.Eq("State", ResourceRequirePlanState.作废)));
            oq.AddFetchMode("TheGWBSTreeGUID", NHibernate.FetchMode.Eager);
            IList rrpList = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
            if (rrpList != null && rrpList.Count > 0)
            {
                gridRollingDemandPlan.Rows.Clear();
                foreach (ResourceRequirePlan rrp in rrpList)
                {
                    AddGridRollingDemandPlan(rrp);
                }
                gridRollingDemandPlan.CurrentCell = gridRollingDemandPlan[1, 0];
                ResourceRequirePlan defaultRRP = gridRollingDemandPlan.Rows[0].Tag as ResourceRequirePlan;
                ShowResourceRequireReceipt(defaultRRP);
            }
        }
        /// <summary>
        /// 根据滚动资源需求计划加载滚动需求计划单
        /// </summary>
        /// <param name="rrp"></param>
        void ShowResourceRequireReceipt(ResourceRequirePlan rrp)
        {
            IList rrrList = SearchResourceRequireReceipt(rrp);
            gridResourceRequireReceipt.Rows.Clear();
            if (rrrList != null && rrrList.Count > 0)
            {
                foreach (ResourceRequireReceipt rrr in rrrList)
                {
                    AddGridResourceRequireReceipt(rrr);
                }
            }
            if (rrp.State != ResourceRequirePlanState.发布)
            {
                mnuResourceRequireReceipt.Enabled = false;
            }
            else
            {
                mnuResourceRequireReceipt.Enabled = true;
            }
        }
        IList SearchResourceRequireReceipt(ResourceRequirePlan rrp)
        {
            //ResourceRequireReceipt r = new ResourceRequireReceipt();
            //r.HandlePerson.Id
            ResourceRequirePlanState state = 0;
            foreach (ResourceRequirePlanState s in Enum.GetValues(typeof(ResourceRequirePlanState)))
            {
                if (s.ToString() == cmbState.Text.Trim())
                {
                    state = s;
                    break;
                }
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ResourceRequirePlan.Id", rrp.Id));

            if (cmbPlanType.SelectedItem != null && !string.IsNullOrEmpty(cmbPlanType.SelectedItem.ToString()))
                oq.AddCriterion(Expression.Eq("ResourceRequirePlanTypeWord", cmbPlanType.SelectedItem.ToString()));
            if (state != 0)
                oq.AddCriterion(Expression.Eq("State", state));
            if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
                oq.AddCriterion(Expression.Eq("HandlePerson.Id", (txtHandlePerson.Result[0] as PersonInfo).Id));

            return model.ObjectQuery(typeof(ResourceRequireReceipt), oq);
        }
        /// <summary>
        /// 向列表添加数据
        /// </summary>
        /// <param name="rrp"></param>
        void AddGridRollingDemandPlan(ResourceRequirePlan rrp)
        {
            int rowIndex = gridRollingDemandPlan.Rows.Add();
            DataGridViewRow row = gridRollingDemandPlan.Rows[rowIndex];
            row.Cells[colMasterPlanName.Name].Value = rrp.RequirePlanVersion;
            row.Cells[colMasterResponsiblePerson.Name].Value = rrp.HandlePersonName;
            row.Cells[colMasterState.Name].Value = rrp.State;
            row.Cells[colMasterCreateTime.Name].Value = rrp.CreateDate;
            row.Tag = rrp;

        }

        void AddGridResourceRequireReceipt(ResourceRequireReceipt rrr)
        {
            int rowIndex = gridResourceRequireReceipt.Rows.Add();
            DataGridViewRow row = gridResourceRequireReceipt.Rows[rowIndex];
            row.Cells[ReceiptPlanType.Name].Value = rrr.ResourceRequirePlanTypeWord;
            row.Cells[ReceiptName.Name].Value = rrr.ReceiptName;
            row.Cells[ReceiptResponsiblePerson.Name].Value = rrr.HandlePersonName;
            row.Cells[ReceiptCreateTime.Name].Value = rrr.RealOperationDate;
            row.Cells[ReceiptState.Name].Value = rrr.State;
            if (rrr.ResourceRequirePlanTypeCode.Substring(2, 1) == "0")
            {
                //PlanType = "总量";
            }
            else
            {
                //PlanType = "期间";
                row.Cells[ReceiptPlanStartTime.Name].Value = rrr.PlanRequireDateBegin.ToShortDateString();
                row.Cells[ReceiptPlanEndTime.Name].Value = rrr.PlanRequireDateEnd.ToShortDateString();

            }
            row.Tag = rrr;
        }

        #region 滚动需求计划右键菜单

        void gridRollingDemandPlan_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mnuRollingDemandPlan.Items[itemRDPShow.Name].Enabled = false;
                mnuRollingDemandPlan.Items[itemRDPDelete.Name].Enabled = false;
                mnuRollingDemandPlan.Items[itemRDPUpdate.Name].Enabled = false;
                mnuRollingDemandPlan.Items[itemRDPInvalid.Name].Enabled = false;
                mnuRollingDemandPlan.Show(MousePosition.X, MousePosition.Y);
            }
        }

        void gridRollingDemandPlan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    if (gridRollingDemandPlan.Rows[e.RowIndex].Selected == false)
                    {
                        gridRollingDemandPlan.ClearSelection();
                        gridRollingDemandPlan.Rows[e.RowIndex].Selected = true;
                    }
                    ResourceRequirePlan rrp = gridRollingDemandPlan.SelectedRows[0].Tag as ResourceRequirePlan;
                    if (rrp.State == ResourceRequirePlanState.作废)
                    {
                        mnuRollingDemandPlan.Enabled = false;
                    }
                    else
                    {
                        mnuRollingDemandPlan.Enabled = true;
                    }
                    mnuRollingDemandPlan.Items[itemRDPShow.Name].Enabled = true;
                    if (rrp.State != ResourceRequirePlanState.发布)
                    {
                        mnuRollingDemandPlan.Items[itemRDPDelete.Name].Enabled = true;
                        mnuRollingDemandPlan.Items[itemRDPInvalid.Name].Enabled = false;
                    }
                    else
                    {
                        mnuRollingDemandPlan.Items[itemRDPInvalid.Name].Enabled = true;
                    }

                    mnuRollingDemandPlan.Items[itemRDPUpdate.Name].Enabled = true;
                    mnuRollingDemandPlan.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        void mnuRollingDemandPlan_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ResourceRequirePlan result = new ResourceRequirePlan();
            if (e.ClickedItem.Name == itemRDPShow.Name)//显示
            {
                mnuRollingDemandPlan.Hide();
                ResourceRequirePlan rrp = gridRollingDemandPlan.SelectedRows[0].Tag as ResourceRequirePlan;
                VRollingResourceRequirePlanMaintain vrrrp = new VRollingResourceRequirePlanMaintain(rrp, e.ClickedItem.Text.Trim());
                vrrrp.ShowDialog();
                result = vrrrp.Result;
            }
            else if (e.ClickedItem.Name == itemRDPAdd.Name)//新增
            {
                mnuRollingDemandPlan.Hide();
                VRollingResourceRequirePlanMaintain vrrrp = new VRollingResourceRequirePlanMaintain(null, e.ClickedItem.Text.Trim());
                vrrrp.ShowDialog();
                result = vrrrp.Result;
            }
            else if (e.ClickedItem.Name == itemRDPUpdate.Name)//修改
            {
                mnuRollingDemandPlan.Hide();
                ResourceRequirePlan rrp = gridRollingDemandPlan.SelectedRows[0].Tag as ResourceRequirePlan;
                if (rrp.State == ResourceRequirePlanState.制定 || rrp.State == ResourceRequirePlanState.发布)
                {
                    VRollingResourceRequirePlanMaintain vrrrp = new VRollingResourceRequirePlanMaintain(rrp, e.ClickedItem.Text.Trim());
                    vrrrp.ShowDialog();
                    result = vrrrp.Result;
                }
                else
                {
                    MessageBox.Show("此状态的滚动需求计划不允许修改！");
                }
            }
            else if (e.ClickedItem.Name == itemRDPDelete.Name)//删除
            {
                mnuRollingDemandPlan.Hide();
                ResourceRequirePlan rrp = gridRollingDemandPlan.SelectedRows[0].Tag as ResourceRequirePlan;
                if (rrp.State == ResourceRequirePlanState.制定)
                {
                    if (MessageBox.Show("确定删除吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        IList deleteRRPList = new ArrayList();
                        deleteRRPList.Add(rrp);
                        bool flag = model.DeleteResourceRequirePlan(deleteRRPList);
                        if (flag)
                        {
                            MessageBox.Show("删除成功！");
                            gridRollingDemandPlan.Rows.Remove(gridRollingDemandPlan.SelectedRows[0]);
                        }
                        else
                        {
                            MessageBox.Show("删除失败！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("只能删除制定状态下的滚动需求计划！");
                }
            }
            else if (e.ClickedItem.Name == itemRDPInvalid.Name)//作废
            {
                mnuRollingDemandPlan.Hide();
                ResourceRequirePlan rrp = gridRollingDemandPlan.SelectedRows[0].Tag as ResourceRequirePlan;
                if (rrp.State == ResourceRequirePlanState.发布)
                {
                    if (MessageBox.Show("确定要作废吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        rrp = model.Mm.modifyPlanDetailState(rrp);

                        rrp.State = ResourceRequirePlanState.作废;

                        rrp = model.SaveOrUpdateResourceRequirePlan(rrp) as ResourceRequirePlan;
                        ShowRollingDemandPlan();
                    }
                }
            }
            if (result != null)
            {
                if (result.Id != null)
                {
                    int rowIndex = -1;
                    if (e.ClickedItem.Name == itemRDPAdd.Name)
                    {
                        rowIndex = gridRollingDemandPlan.Rows.Add();
                    }
                    if (e.ClickedItem.Name == itemRDPUpdate.Name || e.ClickedItem.Name == itemRDPShow.Name)
                    {
                        rowIndex = gridRollingDemandPlan.SelectedRows[0].Index;
                    }

                    if (rowIndex >= 0)
                    {
                        DataGridViewRow row = gridRollingDemandPlan.Rows[rowIndex];
                        row.Cells[colMasterPlanName.Name].Value = result.RequirePlanVersion;
                        row.Cells[colMasterResponsiblePerson.Name].Value = result.HandlePersonName;
                        row.Cells[colMasterState.Name].Value = result.State;
                        row.Cells[colMasterCreateTime.Name].Value = result.CreateDate;
                        row.Tag = result;
                        gridRollingDemandPlan.ClearSelection();
                        gridRollingDemandPlan.Rows[rowIndex].Selected = true;
                        gridRollingDemandPlan_CellClick(gridRollingDemandPlan, new DataGridViewCellEventArgs(0, rowIndex));
                    }
                }
            }
        }
        #endregion

        #region 需求计划单右键菜单

        void gridResourceRequireReceipt_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mnuResourceRequireReceipt.Items[itemRRRShow.Name].Enabled = false;
                mnuResourceRequireReceipt.Items[itemRRRDelete.Name].Enabled = false;
                mnuResourceRequireReceipt.Items[itemRRRUpdate.Name].Enabled = false;
                mnuResourceRequireReceipt.Items[itemRRRMerger.Name].Enabled = false;
                mnuResourceRequireReceipt.Show(MousePosition.X, MousePosition.Y);
            }
        }

        void gridResourceRequireReceipt_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    if (gridResourceRequireReceipt.Rows[e.RowIndex].Selected == false)
                    {
                        gridResourceRequireReceipt.ClearSelection();
                        gridResourceRequireReceipt.Rows[e.RowIndex].Selected = true;
                    }
                    mnuResourceRequireReceipt.Items[itemRRRShow.Name].Enabled = true;
                    mnuResourceRequireReceipt.Items[itemRRRDelete.Name].Enabled = true;
                    ResourceRequireReceipt rrr = gridResourceRequireReceipt.SelectedRows[0].Tag as ResourceRequireReceipt;
                    if (rrr.State != ResourceRequirePlanState.提交)
                    {
                        mnuResourceRequireReceipt.Items[itemRRRUpdate.Name].Enabled = true;
                    }
                    mnuResourceRequireReceipt.Items[itemRRRMerger.Name].Enabled = true;
                    mnuResourceRequireReceipt.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        void mnuResourceRequireReceipt_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ResourceRequireReceipt resultRRR = new ResourceRequireReceipt();
            ResourceRequireReceipt rrr = new ResourceRequireReceipt();
            //if (!(e.ClickedItem.Text.Trim() == "新增总量需求单" || e.ClickedItem.Text.Trim() == "新增期间需求单"))
            if (!(e.ClickedItem.Name == itemRRRAddTotal.Name || e.ClickedItem.Name == itemRRRPeriod.Name))
            {
                rrr = gridResourceRequireReceipt.SelectedRows[0].Tag as ResourceRequireReceipt;
            }
            ResourceRequirePlan rrp = gridRollingDemandPlan.SelectedRows[0].Tag as ResourceRequirePlan;
            int Opcode = 0;
            // string PlanType = "";

            //if (e.ClickedItem.Text.Trim() == "显示")
            if (e.ClickedItem.Name == itemRRRShow.Name)
            {
                #region 显示
                mnuResourceRequireReceipt.Hide();
                Opcode = 3;
                if (rrr.ResourceRequirePlanTypeCode.Substring(2, 1) == "0")
                {
                    //PlanType = "总量";
                    VTotalDemandPlanQuery vtdpq = new VTotalDemandPlanQuery(Opcode, rrp, rrr);
                    vtdpq.ShowDialog();
                    //resultRRR = vtdpq.r
                }
                else
                {
                    //PlanType = "期间";
                    VPeriodRequirePlan vprp = new VPeriodRequirePlan(Opcode, rrp, rrr);
                    vprp.ShowDialog();
                }
                #endregion
            }
            else if (e.ClickedItem.Name == itemRRRAddTotal.Name)//else if (e.ClickedItem.Text.Trim() == "新增总量需求单")
            {
                #region 新增总量需求单
                mnuResourceRequireReceipt.Hide();
                Opcode = 1;
                VTotalDemandPlanQuery vtdpq = new VTotalDemandPlanQuery(Opcode, rrp, rrr);
                vtdpq.ShowDialog();
                resultRRR = vtdpq.ResReceipt;
                #endregion

            }
            else if (e.ClickedItem.Name == itemRRRPeriod.Name)//else if (e.ClickedItem.Text.Trim() == "新增期间需求单")
            {
                #region 新增期间需求单
                mnuResourceRequireReceipt.Hide();
                Opcode = 1;
                VPeriodRequirePlan vprp = new VPeriodRequirePlan(Opcode, rrp, rrr);
                vprp.ShowDialog();
                resultRRR = vprp.ResReceipt;
                #endregion
            }
            else if (e.ClickedItem.Name == itemRRRUpdate.Name)//else if (e.ClickedItem.Text.Trim() == "修改")
            {
                #region 修改
                mnuResourceRequireReceipt.Hide();
                Opcode = 2;
                if (rrr.ResourceRequirePlanTypeCode.Substring(2, 1) == "0")
                {
                    //PlanType = "总量";
                    VTotalDemandPlanQuery vtdpq = new VTotalDemandPlanQuery(Opcode, rrp, rrr);
                    vtdpq.ShowDialog();
                    resultRRR = vtdpq.ResReceipt;
                }
                else
                {
                    //PlanType = "期间";
                    VPeriodRequirePlan vprp = new VPeriodRequirePlan(Opcode, rrp, rrr);
                    vprp.ShowDialog();
                    resultRRR = vprp.ResReceipt;
                }
                #endregion
            }
            else if (e.ClickedItem.Name == itemRRRDelete.Name)//else if (e.ClickedItem.Text.Trim() == "删除")
            {
                #region 删除
                mnuResourceRequireReceipt.Hide();
                IList rrrDeleteList = new ArrayList();
                List<int> indextList = new List<int>();
                if (gridResourceRequireReceipt.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要删除的数据！");
                    return;
                }
                foreach (DataGridViewRow row in gridResourceRequireReceipt.SelectedRows)
                {
                    ResourceRequireReceipt rrrDelete = row.Tag as ResourceRequireReceipt;
                    if (rrrDelete.State != ResourceRequirePlanState.制定)
                    {
                        MessageBox.Show("删除的数据状态必须为制定！");
                        return;
                    }
                    rrrDeleteList.Add(rrrDelete);
                    indextList.Add(row.Index);
                }

                if (MessageBox.Show("确定删除吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool flag = model.DeleteResourceRequireReceiptList(rrrDeleteList);
                    if (flag)
                    {
                        MessageBox.Show("删除成功！");
                        int num = 0;
                        indextList.Sort();
                        for (int i = 0; i < indextList.Count; i++)
                        {
                            gridResourceRequireReceipt.Rows.Remove(gridResourceRequireReceipt.Rows[indextList[i] - num]);
                            num++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("删除失败！");
                    }
                }

                #endregion
            }
            else if (e.ClickedItem.Name == itemRRRMerger.Name)//else if (e.ClickedItem.Text.Trim() == "合并")
            {
                mnuResourceRequireReceipt.Hide();
                resultRRR = MergerResourceRequireReceipt();
            }
            if (resultRRR != null)
            {
                if (resultRRR.Id != null)
                {
                    if (Opcode != 2)
                    {
                        AddGridResourceRequireReceipt(resultRRR);
                    }
                    else
                    {
                        DataGridViewRow row = gridResourceRequireReceipt.SelectedRows[0];
                        row.Cells[ReceiptPlanType.Name].Value = resultRRR.ResourceRequirePlanTypeWord;
                        row.Cells[ReceiptName.Name].Value = resultRRR.ReceiptName;
                        row.Cells[ReceiptResponsiblePerson.Name].Value = resultRRR.HandlePersonName;
                        row.Cells[ReceiptCreateTime.Name].Value = resultRRR.CreateDate;
                        row.Cells[ReceiptState.Name].Value = resultRRR.State;
                        if (rrr.ResourceRequirePlanTypeCode.Substring(2, 1) == "0")
                        {
                            //PlanType = "总量";
                        }
                        else
                        {
                            //PlanType = "期间";
                            row.Cells[ReceiptPlanStartTime.Name].Value = resultRRR.PlanRequireDateBegin.ToShortDateString();
                            row.Cells[ReceiptPlanEndTime.Name].Value = resultRRR.PlanRequireDateEnd.ToShortDateString();

                        }
                        row.Tag = resultRRR;
                    }
                }
            }
        }

        /// <summary>
        /// 合并资源需求计划单
        /// </summary>
        ResourceRequireReceipt MergerResourceRequireReceipt()
        {
            if (!Verify()) return null;
            try
            {
                List<ResourceRequireReceipt> listRRR = new List<ResourceRequireReceipt>();
                foreach (DataGridViewRow row in gridResourceRequireReceipt.SelectedRows)
                {
                    ResourceRequireReceipt rrr = row.Tag as ResourceRequireReceipt;
                    //rrr.ResourceCategorySysCode
                    listRRR.Add(rrr);
                }
                #region 取<合并{资源需求计划单}集>中各个对象的【资源分类代码】关联{资源分类}_【层次码】最大共有字符串
                var query = listRRR.OrderBy(p => p.ResourceCategorySysCode.Length).ToList<ResourceRequireReceipt>();

                bool flag = true;
                string sysCodeMax = "";
                string[] sysCodes = query[0].ResourceCategorySysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = sysCodes.Length - 1; i >= 0; i--)
                {
                    flag = true;
                    string sysCode = "";
                    for (int j = 0; j <= i; j++)
                    {
                        sysCode += sysCodes[j] + ".";
                    }

                    for (int g = 1; g < query.Count; g++)
                    {
                        ResourceRequireReceipt r = query[g];
                        if (!r.ResourceCategorySysCode.Contains(sysCode))
                        {
                            flag = false;
                            break;
                        }
                    }
                    //flag = true;
                    if (flag)
                    {
                        sysCodeMax = sysCode;
                        break;
                    }
                }
                #endregion
                //根据资源分类层次码查询资源分类
                ObjectQuery oqCate = new ObjectQuery();
                oqCate.AddCriterion(Expression.Eq("SysCode", sysCodeMax));
                MaterialCategory cate = model.ObjectQuery(typeof(MaterialCategory), oqCate)[0] as MaterialCategory;

                #region 合并资源需求计划单
                ResourceRequireReceipt addRRR = new ResourceRequireReceipt();
                //addRRR.Code
                addRRR.ProjectId = projectInfo.Id;
                addRRR.ProjectName = projectInfo.Name;
                addRRR.HandlePerson = loginPerson;
                addRRR.HandlePersonName = loginPerson.Name;
                addRRR.OwnerOrgSysCode = loginPerson.Code;
                addRRR.ResourceRequirePlanTypeWord = listRRR[0].ResourceRequirePlanTypeWord;
                addRRR.ResourceRequirePlanTypeCode = listRRR[0].ResourceRequirePlanTypeCode;
                addRRR.ResourceRequirePlan = listRRR[0].ResourceRequirePlan;
                addRRR.ResourceRequirePlanName = listRRR[0].ResourceRequirePlanName;
                addRRR.ResourceCategory = cate;
                addRRR.ResourceCategorySysCode = sysCodeMax;
                addRRR.PlanRequireDateBegin = listRRR[0].PlanRequireDateBegin;
                addRRR.PlanRequireDateEnd = listRRR[0].PlanRequireDateEnd;
                addRRR.SchedulingProduction = listRRR[0].SchedulingProduction;
                addRRR.SchedulingProductionName = listRRR[0].SchedulingProductionName;
                addRRR.CreateDate = DateTime.Now;
                addRRR.State = ResourceRequirePlanState.制定;
                //addRRR.OperatioOrg = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.
                //addRRR.OperatioOrgName
                //ResourceRequireReceipt mergerRRR = model.SaveOrUpdateResourceRequireReceipt(addRRR);
                #endregion

                #region 合并资源需求计划单明细
                IList mergerRRRDtlList = new ArrayList();

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (ResourceRequireReceipt receipt in listRRR)
                {
                    dis.Add(Expression.Eq("TheResReceipt.Id", receipt.Id));
                }
                oq.AddCriterion(dis);
                IList rrrDtlList = model.ObjectQuery(typeof(ResourceRequireReceiptDetail), oq);
                if (rrrDtlList != null && rrrDtlList.Count > 0)
                {
                    foreach (ResourceRequireReceiptDetail rrrDtl in rrrDtlList)
                    {
                        bool isHas = false;//存在修改，不存在新建
                        if (mergerRRRDtlList != null && mergerRRRDtlList.Count > 0)
                        {
                            for (int i = 0; i < mergerRRRDtlList.Count; i++)
                            {
                                //foreach (ResourceRequireReceiptDetail merger in mergerRRRDtlList)
                                //{
                                ResourceRequireReceiptDetail merger = mergerRRRDtlList[i] as ResourceRequireReceiptDetail;
                                if (rrrDtl.TheGWBSTaskGUID.Id == merger.TheGWBSTaskGUID.Id
                                    && rrrDtl.MaterialResource.Id == merger.MaterialResource.Id
                                    && rrrDtl.DiagramNumber == merger.DiagramNumber)
                                {
                                    merger.PeriodQuantity += rrrDtl.PeriodQuantity;
                                    merger.ApproachRequestDesc += "\r\n" + rrrDtl.ApproachRequestDesc;
                                    isHas = true;
                                }
                            }
                        }
                        //新建
                        if (!isHas)
                        {
                            ResourceRequireReceiptDetail newMergerDtl = new ResourceRequireReceiptDetail();
                            //newMergerDtl.TheResourceRequireReceipt = mergerRRR;
                            newMergerDtl.ApproachRequestDesc = rrrDtl.ApproachRequestDesc;
                            newMergerDtl.CostQuantity = rrrDtl.CostQuantity;
                            newMergerDtl.DailyPlanPublishQuantity = rrrDtl.DailyPlanPublishQuantity;
                            newMergerDtl.DiagramNumber = rrrDtl.DiagramNumber;
                            newMergerDtl.FirstOfferRequireQuantity = rrrDtl.FirstOfferRequireQuantity;
                            newMergerDtl.MaterialCode = rrrDtl.MaterialCode;
                            newMergerDtl.MaterialResource = rrrDtl.MaterialResource;
                            newMergerDtl.MaterialName = rrrDtl.MaterialName;
                            newMergerDtl.MaterialStuff = rrrDtl.MaterialStuff;
                            newMergerDtl.MaterialSpec = rrrDtl.MaterialSpec;
                            newMergerDtl.PeriodQuantity = rrrDtl.PeriodQuantity;
                            newMergerDtl.PlanInRequireQuantity = rrrDtl.PlanInRequireQuantity;
                            newMergerDtl.PlannedCostQuantity = rrrDtl.PlannedCostQuantity;
                            newMergerDtl.PlanOutRequireQuantity = rrrDtl.PlanOutRequireQuantity;
                            newMergerDtl.QuantityUnitGUID = rrrDtl.QuantityUnitGUID;
                            newMergerDtl.QuantityUnitName = rrrDtl.QuantityUnitName;
                            newMergerDtl.MaterialCategory = rrrDtl.MaterialCategory;
                            newMergerDtl.MaterialCategoryName = rrrDtl.MaterialCategoryName;
                            newMergerDtl.ResponsibilityCostQuantity = rrrDtl.ResponsibilityCostQuantity;
                            newMergerDtl.State = rrrDtl.State;
                            newMergerDtl.TheGWBSSysCode = rrrDtl.TheGWBSSysCode;
                            newMergerDtl.TheGWBSTaskGUID = rrrDtl.TheGWBSTaskGUID;
                            newMergerDtl.TheGWBSTaskName = rrrDtl.TheGWBSTaskName;
                            //newMergerDtl.TheProjectGUID = rrrDtl.TheProjectGUID;
                            //newMergerDtl.TheProjectName = rrrDtl.TheProjectName;
                            mergerRRRDtlList.Add(newMergerDtl);
                        }
                    }
                }

                ResourceRequireReceipt mergerRRR = model.SaveResourceRequireReceiptAndDetail(addRRR, mergerRRRDtlList);
                #endregion
                MessageBox.Show("合并成功！");
                return mergerRRR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("合并失败，错误信息：" + ExceptionUtil.ExceptionMessage(ex));
                return null;
            }
        }
        /// <summary>
        /// 合并前验证
        /// </summary>
        /// <returns></returns>
        bool Verify()
        {
            if (gridResourceRequireReceipt.SelectedRows.Count < 2)
            {
                MessageBox.Show("请选择需要合并的资源需求计划单,至少要有两条数据！");
                return false;
            }

            ResourceRequireReceipt rrr = gridResourceRequireReceipt.SelectedRows[0].Tag as ResourceRequireReceipt;
            if (rrr.State != ResourceRequirePlanState.制定)
            {
                MessageBox.Show("要合并的资源需求计划单的状态都必须是制定！");
                return false;
            }
            for (int i = 1; i < gridResourceRequireReceipt.SelectedRows.Count; i++)
            {
                ResourceRequireReceipt r = gridResourceRequireReceipt.SelectedRows[i].Tag as ResourceRequireReceipt;
                if (r.ResourceRequirePlanTypeCode.Substring(2, 1) == "0")
                {
                    MessageBox.Show("只能合并期间需求计划单！");
                    return false;
                }
                if (r.State != ResourceRequirePlanState.制定)
                {
                    MessageBox.Show("要合并的资源需求计划单每个状态都必须是制定！");
                    return false;
                }
                if (r.ResourceRequirePlanTypeWord != rrr.ResourceRequirePlanTypeWord)
                {
                    MessageBox.Show("要合并的资源需求计划单的【计划类型】都必须是相同！");
                    return false;
                }
                if (r.PlanRequireDateBegin.ToShortDateString() != rrr.PlanRequireDateBegin.ToShortDateString() || r.PlanRequireDateEnd.ToShortDateString() != rrr.PlanRequireDateEnd.ToShortDateString())
                {
                    MessageBox.Show("要合并的资源需求计划单的【计划的开始时间】、【计划的结束时间】都必须是相同！");
                    return false;
                }
                if (r.SchedulingProduction == null || rrr.SchedulingProduction == null)
                {
                    if (!(r.SchedulingProduction == null && rrr.SchedulingProduction == null))
                    {
                        MessageBox.Show("要合并的资源需求计划单的【参照进度计划】都必须是相同的！");
                        return false;
                    }
                }
                else
                {
                    if (r.SchedulingProduction.Id != rrr.SchedulingProduction.Id)
                    {
                        MessageBox.Show("要合并的资源需求计划单的【参照进度计划】都必须是相同的！");
                        return false;
                    }
                }


                if (string.IsNullOrEmpty(r.ResourceCategorySysCode))
                {
                    MessageBox.Show("要合并的资源需求计划单中有的【资源分类】为空，不能合并！");
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
