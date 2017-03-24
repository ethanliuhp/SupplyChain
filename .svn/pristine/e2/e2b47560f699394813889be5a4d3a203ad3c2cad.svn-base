using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VAddPlanInResourceDemand : TBasicDataView
    {
        MRollingDemandPlan model = new MRollingDemandPlan();
        private CurrentProjectInfo projectInfo = null;

        /// <summary>
        /// 操作{滚动资源需求计划}明细
        /// </summary>
        private ResourceRequirePlanDetail opResourceRequirePlanDetail;
        //private ResourceRequirePlan operationResourceRequirePlan;
        //public PlanRequireType typeIsInOrOut;//计划内或计划外
        public GWBSTree rootNode;//根节点（范围）
        private List<ResourceRequirePlanDetail> ResourceRequirePlanDetailList;

        private ResourceRequirePlanDetail result;

        public ResourceRequirePlanDetail Result
        {
            get { return result; }
            set { result = value; }
        }
        private IList resultList;

        public IList ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }

        private GWBSTree wbs;
        private Material mat;
        private string diagramNumber = string.Empty;
        //private StandardUnit su;


        public VAddPlanInResourceDemand(ResourceRequirePlanDetail rrpd, List<ResourceRequirePlanDetail> rrpDtlList)
        {
            InitializeComponent();
            //operationResourceRequirePlan = rrp;
            opResourceRequirePlanDetail = rrpd;
            ResourceRequirePlanDetailList = rrpDtlList;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            InitData();
            InitEvent();
        }
        void InitData()
        {
            //txtGWBSTree.ReadOnly = true;

            if (opResourceRequirePlanDetail.TheGWBSTaskGUID != null)
            {
                wbs = new GWBSTree();
                wbs.Id = opResourceRequirePlanDetail.TheGWBSTaskGUID.Id;
                wbs.Name = opResourceRequirePlanDetail.TheGWBSTaskName;
                wbs.SysCode = opResourceRequirePlanDetail.TheGWBSSysCode;
                txtGWBSTree.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), opResourceRequirePlanDetail.TheGWBSTaskName, opResourceRequirePlanDetail.TheGWBSSysCode);
                txtGWBSTree.Tag = wbs;
            }
            if (opResourceRequirePlanDetail.MaterialResource != null)
            {
                if (opResourceRequirePlanDetail.Id != null)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", opResourceRequirePlanDetail.MaterialResource.Id));
                    mat = model.ObjectQuery(typeof(Material), oq)[0] as Material;
                }
                else
                {
                    mat = opResourceRequirePlanDetail.MaterialResource;
                }
            }
            txtMaterialName.Text = opResourceRequirePlanDetail.MaterialName;
            txtMaterialSpec.Text = opResourceRequirePlanDetail.MaterialSpec;
            txtDiagramNumber.Text = opResourceRequirePlanDetail.DiagramNumber;
            if (opResourceRequirePlanDetail.RequireType == PlanRequireType.计划内需求)
            {
                txtPlanNum.Text = opResourceRequirePlanDetail.PlanInRequireQuantity.ToString();
            }
            else
            {
                txtPlanNum.Text = opResourceRequirePlanDetail.PlanOutRequireQuantity.ToString();
            }
            LoadMaterial();
        }
        void InitEvent()
        {
            btnSelectGWBSTree.Click += new EventHandler(btnSelectGWBSTree_Click);

            btnSelectAll.Click += new EventHandler(btnSelectAll_Click);
            btnInvertSelect.Click += new EventHandler(btnInvertSelect_Click);

            gridMaterial.CellValidating += new DataGridViewCellValidatingEventHandler(gridMaterial_CellValidating);

            btnOK.Click += new EventHandler(btnOK_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        void LoadMaterial()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("TheSyscode", mat.TheSyscode, MatchMode.Start));
            oq.AddFetchMode("Category", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("BasicUnit", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(Material), oq);
            gridMaterial.Rows.Clear();
            if (list != null && list.Count > 0)
            {
                foreach (Material m in list)
                {
                    ResourceRequirePlanDetail rrpd = new ResourceRequirePlanDetail();
                    rrpd.TheProjectGUID = projectInfo.Id;
                    rrpd.TheProjectName = projectInfo.Name;

                    rrpd.MaterialResource = m;
                    rrpd.MaterialCode = m.Code;
                    rrpd.MaterialName = m.Name;
                    rrpd.MaterialStuff = m.Quality;
                    rrpd.MaterialSpec = m.Specification;
                    rrpd.ResourceCategory = m.Category;
                    rrpd.ResourceTypeClassification = m.Category.Name;
                    rrpd.State = ResourceRequirePlanDetailState.编制;
                    rrpd.StateUpdateTime = DateTime.Now;

                    rrpd.MonthPlanPublishQuantity = 0;
                    rrpd.DailyPlanPublishQuantity = 0;
                    rrpd.ExecutedQuantity = 0;

                    rrpd.QuantityUnitGUID = m.BasicUnit;
                    rrpd.QuantityUnitName = m.BasicUnit.Name;
                    rrpd.RequireType = PlanRequireType.计划内需求;
                    rrpd.CreateTime = DateTime.Now;
                    AddGridMaterial(rrpd);
                }
            }
        }
        void AddGridMaterial(ResourceRequirePlanDetail dtl)
        {
            int rowIndex = gridMaterial.Rows.Add();
            gridMaterial[colMaterialName.Name, rowIndex].Value = dtl.MaterialName;
            gridMaterial[colMaterialSpec.Name, rowIndex].Value = dtl.MaterialSpec;
            //gridMaterial[colMaterialStuff.Name, rowIndex].Value = dtl.MaterialStuff;
            gridMaterial.Rows[rowIndex].Tag = dtl;
        }

        void gridMaterial_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == gridMaterial.Rows[e.RowIndex].Cells[colDiagramNumber.Name].ColumnIndex)
            {
                ResourceRequirePlanDetail rrpd = gridMaterial.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                rrpd.DiagramNumber = gridMaterial[colDiagramNumber.Name, e.RowIndex].EditedFormattedValue.ToString();
                gridMaterial.Rows[e.RowIndex].Tag = rrpd;
            }
            else if (e.ColumnIndex == gridMaterial.Rows[e.RowIndex].Cells[colNum.Name].ColumnIndex)
            {
                try
                {
                    ResourceRequirePlanDetail rrpd = gridMaterial.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                    decimal num = Convert.ToDecimal(gridMaterial[colNum.Name, e.RowIndex].EditedFormattedValue.ToString());
                    rrpd.PlanInRequireQuantity = num;
                    gridMaterial.Rows[e.RowIndex].Tag = rrpd;
                }
                catch (Exception)
                {
                    MessageBox.Show("计划量输入有误！");
                    e.Cancel = true;
                }
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (!Verify()) return;
            IList list = new ArrayList();
            foreach (DataGridViewRow row in gridMaterial.Rows)
            {
                if ((bool)row.Cells[colSelect.Name].EditedFormattedValue)
                {
                    ResourceRequirePlanDetail rrpd = row.Tag as ResourceRequirePlanDetail;
                    if (wbs.Id == opResourceRequirePlanDetail.TheGWBSTaskGUID.Id)
                    {
                        rrpd.TheGWBSTaskGUID = opResourceRequirePlanDetail.TheGWBSTaskGUID;
                        rrpd.TheGWBSTaskName = opResourceRequirePlanDetail.TheGWBSTaskName;
                        rrpd.TheGWBSSysCode = opResourceRequirePlanDetail.TheGWBSSysCode;
                    }
                    else
                    {
                        rrpd.TheGWBSTaskGUID = wbs;
                        rrpd.TheGWBSTaskName = wbs.Name;
                        rrpd.TheGWBSSysCode = wbs.SysCode;
                    }
                    list.Add(rrpd);
                }
            }
            resultList = list;
            this.Close();
        }

        void btnSelectGWBSTree_Click(object sender, EventArgs e)
        {
            //VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(rootNode);
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                wbs = (list[0] as TreeNode).Tag as GWBSTree;
                txtGWBSTree.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
                txtGWBSTree.Tag = wbs;
                txtGWBSTree.Focus();
            }
        }

        //验证
        bool Verify()
        {
            if (txtGWBSTree.Tag == null || txtGWBSTree.Text.Trim() == "")
            {
                MessageBox.Show("请选择工程项目任务！");
                return false;
            }
            if (ResourceRequirePlanDetailList != null && ResourceRequirePlanDetailList.Count > 0)
            {
                foreach (DataGridViewRow row in gridMaterial.Rows)
                {
                    if ((bool)row.Cells[colSelect.Name].EditedFormattedValue)
                    {
                        ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                        foreach (ResourceRequirePlanDetail d in ResourceRequirePlanDetailList)
                        {
                            if (d.MaterialResource.Id == dtl.MaterialResource.Id && d.DiagramNumber == dtl.DiagramNumber && d.RequireType == PlanRequireType.计划内需求)
                            {
                                if (d.TheGWBSSysCode.Contains(wbs.SysCode) || wbs.SysCode.Contains(d.TheGWBSSysCode))
                                {
                                    MessageBox.Show("选择节点的父节点或子节点提了相同的资源（第" + (row.Index + 1) + "行资源），请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        #region 全选 反选
        void btnInvertSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridMaterial.Rows)
            {
                if ((bool)row.Cells[colSelect.Name].EditedFormattedValue)
                {
                    row.Cells[colSelect.Name].Value = false;
                }
                else
                {
                    row.Cells[colSelect.Name].Value = true;
                }
            }
        }

        void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridMaterial.Rows)
            {
                row.Cells[colSelect.Name].Value = true;
            }
        }
        #endregion

        void btnQuit_Click(object sender, EventArgs e)
        {
            resultList = null;
            this.Close();
        }
    }
}
