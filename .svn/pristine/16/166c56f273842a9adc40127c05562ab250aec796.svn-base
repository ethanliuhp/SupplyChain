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
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using FlexCell;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    /// <summary>
    /// 工程成本分摊比例设置
    /// </summary>
    public partial class VPlanProjectAmountShareRateSet : TBasicDataView
    {
        /// <summary>
        /// 父节点ID
        /// </summary>
        string WBSTreeId = string.Empty;
        /// <summary>
        /// 父节点sysCode
        /// </summary>
        string sysCode = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        MGWBSTree model = new MGWBSTree();
        /// <summary>
        /// 契约
        /// </summary>
        ContractGroup contractGroup = null;
        /// <summary>
        /// 源任务节点
        /// </summary>
        GWBSTree sourseGWBSTree = null;
        /// <summary>
        /// 叶节点的分摊比例关系
        /// </summary>
        List<GWBSTreeDetailRelationship> lstRelation = null;
        /// <summary>
        /// 父节点下的叶节点
        /// </summary>
        List<GWBSTree> lstSourseLeafGWBSTree = new List<GWBSTree>();
        /// <summary>
        /// 叶节点上变更的明细
        /// </summary>
        List<GWBSDetail> lstDetailChanged = new List<GWBSDetail>();
        //List<GWBSDetail> lstDetailDelete = new List<GWBSDetail>();
        /// <summary>
        /// 叶节点变更的分摊比例关系
        /// </summary>
        List<GWBSTreeDetailRelationship> lstRelationChanged = new List<GWBSTreeDetailRelationship>();
        List<GWBSTreeDetailRelationship> lstRelationDelete = new List<GWBSTreeDetailRelationship>();

        //public VPlanProjectAmountShareRateSet()
        //{
        //    InitializeComponent();
        //}   

        public VPlanProjectAmountShareRateSet(string strWBSTreeId, string strSysCode)
        {
            WBSTreeId = strWBSTreeId;
            sysCode = strSysCode;
            InitializeComponent();
            InitData();
            InitEvent();
        }
        public void InitData()
        {
            //获取父节点下的所有叶节点
            //ObjectQuery oq = new ObjectQuery();
            //1：分摊时是否删除原有的详细，耗用【目前是按照直接分摊，不删除已添加的处理】
            //2：获取当前节点的明细，明细，资源耗用              
            //oq.AddCriterion(Expression.Eq("Id", WBSTreeId));
            //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("Details.ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            //var ilstSourseGWBSTree = model.GetGWBSTreeById(typeof(GWBSDetail), oq);
            //if (ilstSourseGWBSTree == null || ilstSourseGWBSTree.Count)
            //{
            //    return;
            //}
            //sourseGWBSTree = ilstSourseGWBSTree[0] as GWBSTree;

            //ObjectQuery opleaf = new ObjectQuery();
            //opleaf.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.LeafNode));
            //opleaf.AddCriterion(Expression.Like("SysCode", sysCode, MatchMode.Start));

            Disjunction disOqSourseNode = new Disjunction();
            disOqSourseNode.Add(Expression.Eq("Id", WBSTreeId));
            disOqSourseNode.Add(Expression.And(Expression.Eq("CategoryNodeType", NodeType.LeafNode), Expression.Like("SysCode", sysCode, MatchMode.Start)));

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(disOqSourseNode);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails.ListGWBSDtlCostSubRate", NHibernate.FetchMode.Eager);
            //oq.AddOrder(Order.Asc("Level"));
            //oq.AddOrder(Order.Asc("OrderNo"));
            var lstGWBSTree = model.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>().ToList();
            if (lstGWBSTree == null || lstGWBSTree.Count == 0)
            {
                return;
            }
            sourseGWBSTree = lstGWBSTree.FirstOrDefault(p => p.Id == WBSTreeId); ;
            lstSourseLeafGWBSTree = lstGWBSTree.Where(p => p.Id != WBSTreeId).OrderBy(P => P.Name).ToList();

            //任务节点与明细分摊比例
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("SourseGWBSTreeID", WBSTreeId));
            lstRelation = model.ObjectQuery(typeof(GWBSTreeDetailRelationship), oq).OfType<GWBSTreeDetailRelationship>().ToList();

            //设置契约
            if (sourseGWBSTree.Details != null && sourseGWBSTree.Details.Count > 0)
            {
                var objdetail = sourseGWBSTree.Details.ToList()[0];
                contractGroup = new ContractGroup()
                {
                    Code = objdetail.ContractGroupCode,
                    Id = objdetail.ContractGroupGUID,
                    ContractName = objdetail.ContractGroupName,
                    ContractGroupType = objdetail.ContractGroupType
                };
            }
            var lstdetail = sourseGWBSTree.Details.Where(p => p.State != DocumentState.Edit).ToList();//目前只显示已发布的明细
            InitGrid(detailgrid, new List<string> { "全选", "定额编号", "明细名称" }, lstdetail == null ? 1 : (lstdetail.Count + 1), 4);
            InitGrid(treeNodeGrid, new List<string> { "节点名称", "百分比（%）" }, lstSourseLeafGWBSTree == null ? 1 : (lstSourseLeafGWBSTree.Count + 1), 3);
            ModelToView();
        }

        public void InitEvent()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnAvg.Click += new EventHandler(btnAvg_Click);
            btnClearPercent.Click += new EventHandler(btnClearPercent_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            cbxAllSel.CheckStateChanged += new EventHandler(cbxAllSel_CheckedChanged);
            detailgrid.CellChange += new Grid.CellChangeEventHandler(detailgrid_CellChange);
        }


        #region 事件
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRate())
                {
                    ViewToModel();

                    if (lstDetailChanged == null || lstDetailChanged.Count == 0)
                    {
                        MessageBox.Show("请至少选中一个明细信息！");
                        return;
                    }
                    if (lstRelationChanged == null || lstRelationChanged.Count == 0)
                    {
                        MessageBox.Show("请至少设置一个节点的分摊百分比！");
                        return;
                    }
                    //model.SaveOrUpdate((IList)lstDetailChanged);
                    //model.SaveOrUpdate((IList)lstRelationChanged);//.OfType<GWBSTreeDetailRelationship>().ToList();

                    //model.Delete((IList)lstDetailDelete);
                    //model.Delete((IList)lstRelationDelete);

                    model.SaveOrUpdateShareRateInfo((IList)lstRelationChanged, (IList)lstRelationDelete);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("请确保所有节点的百分比之和为100！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("分摊保存报错：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnAvg_Click(object sender, EventArgs e)
        {
            treeNodeGrid.AutoRedraw = false;
            decimal avgRate = Math.Round((decimal)100 / lstSourseLeafGWBSTree.Count, 2);
            decimal lastAvgRate = 100;
            //前n-1项取平均值，最后一项取
            for (int i = 1; i < lstSourseLeafGWBSTree.Count; i++)
            {
                treeNodeGrid.Cell(i, 2).Text = avgRate.ToString();
                lastAvgRate = lastAvgRate - avgRate;
            }
            treeNodeGrid.Cell(lstSourseLeafGWBSTree.Count, 2).Text = lastAvgRate.ToString();

            treeNodeGrid.Refresh();
            treeNodeGrid.AutoRedraw = true;
        }

        void btnClearPercent_Click(object sender, EventArgs e)
        {
            treeNodeGrid.AutoRedraw = false;
            //前n-1项取平均值，最后一项取
            for (int i = 1; i < treeNodeGrid.Rows; i++)
            {
                treeNodeGrid.Cell(i, 2).Text = "";
            }

            treeNodeGrid.Refresh();
            treeNodeGrid.AutoRedraw = true;
        }

        void cbxAllSel_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < detailgrid.Rows; i++)
            {
                detailgrid.Cell(i, 1).Text = cbxAllSel.Checked.ToString();
            }
        }

        void detailgrid_CellChange(object Sender, Grid.CellChangeEventArgs e)
        {
            int rowindex = e.Row, colIndex = e.Col;
            if (rowindex > 0 && colIndex == 1)
            {
                SetRateAfterDetailChanged();
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private bool ValidateRate()
        {
            decimal sumRate = 0;
            for (int i = 1; i < treeNodeGrid.Rows; i++)
            {
                sumRate += Math.Abs(ClientUtil.ToDecimal(treeNodeGrid.Cell(i, 2).Text.Trim()));
            }
            return sumRate == 100;
        }

        #region Flex处理
        private void InitGrid(FlexCell.Grid grid, List<string> lstHeader, int rows, int cols)
        {
            grid.AutoRedraw = false;
            grid.Rows = rows;
            grid.Cols = cols;
            grid.Locked = false;
            grid.Cell(0, 0).Locked = true;
            grid.Row(0).Locked = false;
            grid.DisplayRowNumber = true;
            grid.StartRowNumber = 1;
            grid.Column(0).Visible = true;
            grid.Column(0).AutoFit();
            grid.SelectionMode = FlexCell.SelectionModeEnum.Free;
            grid.DisplayFocusRect = true;
            grid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            grid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            grid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            grid.BackColorBkg = SystemColors.Control;
            grid.DefaultFont = new Font("Tahoma", 8);

            int rowIndex = 0, colIndex = 0;
            for (int i = 0; i < lstHeader.Count; i++)
            {
                colIndex = i + 1;
                grid.Cell(rowIndex, colIndex).Text = lstHeader[i];
            }

            if (grid == detailgrid)
            {
                grid.Column(1).CellType = CellTypeEnum.CheckBox;
            }
            grid.Column(0).AutoFit();
            grid.AutoRedraw = true;
            grid.Refresh();
        }

        private void ViewToModel()
        {
            lstDetailChanged.Clear();
            lstRelationChanged.Clear();
            int rowIndex = 0;
            for (int i = 1; i < detailgrid.Rows; i++)
            {
                rowIndex = i;
                string detailid = detailgrid.Cell(rowIndex, 0).Tag.Trim();
                bool isChecked = ClientUtil.ToBool(detailgrid.Cell(rowIndex, 1).Text.Trim());
                if (isChecked == false)
                {
                    continue;
                }

                //源明细
                GWBSDetail parentDetail = sourseGWBSTree.Details.FirstOrDefault(p => p.Id == detailid);
                if (parentDetail == null)
                {
                    continue;
                }
                for (int j = 1; j < treeNodeGrid.Rows; j++)
                {
                    rowIndex = j;
                    string childNodeId = treeNodeGrid.Cell(rowIndex, 0).Tag.Trim();
                    GWBSTree targetGWBSTree = lstSourseLeafGWBSTree.FirstOrDefault(p => p.Id == childNodeId);
                    decimal rate = Math.Abs(ClientUtil.ToDecimal(treeNodeGrid.Cell(rowIndex, 2).Text.Trim()) / 100);

                    //如果百分比为0，则表示此节点无该明细信息
                    if (rate <= 0)
                    {
                        continue;
                    }

                    //根据 源节点ID，源明细ID，目标节点ID 获取相关数据
                    GWBSTreeDetailRelationship obj = lstRelation.FirstOrDefault(p => p.SourseGWBSTreeID == WBSTreeId
                                                                                    && p.SourseGWBSDetailID == detailid
                                                                                    && p.TargetGWBSTreeID == childNodeId);
                    GWBSDetail targetDetail = null;
                    #region 分摊百分比
                    if (obj == null)
                    {
                        targetDetail = parentDetail.CloneByRate(targetGWBSTree, contractGroup, rate);
                        targetDetail.ProduceConfirmFlag = 1;//设置成：生产节点 
                        targetDetail.CostingFlag = 0;
                        targetDetail.ResponseFlag = 0;

                        obj = new GWBSTreeDetailRelationship()
                        {
                            SourseGWBSTreeID = WBSTreeId,
                            SourseGWBSDetailID = detailid,
                            TargetGWBSTreeID = childNodeId,
                            TargetGWBSDetail = targetDetail,
                            Rate = rate
                        };
                    }
                    else
                    {
                        if (targetGWBSTree.Details == null || targetGWBSTree.Details.FirstOrDefault(p => p.Id == obj.TargetGWBSDetailID) == null)
                        {
                            targetDetail = parentDetail.CloneByRate(targetGWBSTree, contractGroup, rate);
                            targetDetail.ProduceConfirmFlag = 1;//设置成：生产节点 
                            targetDetail.CostingFlag = 0;
                            targetDetail.ResponseFlag = 0;
                        }
                        else
                        {
                            targetDetail = targetGWBSTree.Details.FirstOrDefault(p => p.Id == obj.TargetGWBSDetailID);
                        }

                        UpdateDetail(targetDetail, parentDetail, rate);
                        obj.TargetGWBSDetail = targetDetail;
                        obj.Rate = rate;
                    }
                    lstDetailChanged.Add(targetDetail);
                    lstRelationChanged.Add(obj);
                    #endregion
                }
            }
            //当前关联关系在数据库中存在，在lstRelationChanged不存在，则删除
            var lstDetailShareChanged = lstRelation.Where(P => lstRelationChanged.Any(M => M.SourseGWBSDetailID == P.SourseGWBSDetailID));
            lstRelationDelete = lstDetailShareChanged.Where(p => lstRelationChanged.Any(n => n.TargetGWBSTreeID == p.TargetGWBSTreeID && n.SourseGWBSDetailID == p.SourseGWBSDetailID) == false).ToList();
            foreach (var item in lstRelationDelete)
            {
                if (lstRelationChanged.Any(n => n.TargetGWBSTreeID == item.TargetGWBSTreeID && n.SourseGWBSDetailID == item.SourseGWBSDetailID) == false)
                {
                    var objtree = lstSourseLeafGWBSTree.FirstOrDefault(p => p.Id == item.TargetGWBSTreeID);
                    item.TargetGWBSDetail = objtree.Details.FirstOrDefault(p => p.Id == item.TargetGWBSDetailID);
                }
            }
        }

        private void ModelToView()
        {
            #region 明细
            detailgrid.AutoRedraw = false;
            int rowIndex = 0;
            GWBSDetail objDetail = null;
            var lstdetail = sourseGWBSTree.Details.Where(p => p.State != DocumentState.Edit).ToList();
            for (int i = 0; i < lstdetail.Count; i++)
            {
                objDetail = lstdetail[i];
                rowIndex = i + 1;
                detailgrid.Cell(rowIndex, 0).Tag = objDetail.Id;
                detailgrid.Cell(rowIndex, 1).Text = "False";
                detailgrid.Cell(rowIndex, 2).Text = objDetail.TheCostItem.QuotaCode;
                detailgrid.Cell(rowIndex, 3).Text = objDetail.Name;
            }

            detailgrid.Column(0).AutoFit();
            detailgrid.Column(1).AutoFit();
            detailgrid.Column(2).AutoFit();
            detailgrid.Column(3).AutoFit();
            detailgrid.Refresh();
            detailgrid.AutoRedraw = true;
            #endregion

            #region 叶节点
            treeNodeGrid.AutoRedraw = false;
            GWBSTree objTree = null;
            for (int i = 0; i < lstSourseLeafGWBSTree.Count; i++)
            {
                rowIndex = i + 1;
                objTree = lstSourseLeafGWBSTree[i];
                treeNodeGrid.Cell(rowIndex, 0).Tag = objTree.Id;
                treeNodeGrid.Cell(rowIndex, 1).Text = objTree.Name;
            }

            treeNodeGrid.Column(0).AutoFit();
            treeNodeGrid.Column(1).AutoFit();
            treeNodeGrid.Column(2).AutoFit();

            treeNodeGrid.Refresh();
            treeNodeGrid.AutoRedraw = true;
            #endregion
        }

        private List<GWBSDetail> GetListCheckedDetail()
        {
            var lstCheckedDetail = new List<GWBSDetail>();
            for (int i = 1; i < detailgrid.Rows; i++)
            {
                bool isChecked = ClientUtil.ToBool(detailgrid.Cell(i, 1).Text.Trim());
                if (isChecked == false)
                {
                    continue;
                }
                var id = detailgrid.Cell(i, 0).Tag.Trim();
                GWBSDetail obj = sourseGWBSTree.Details.FirstOrDefault(p => p.Id == id);
                if (obj != null)
                {
                    lstCheckedDetail.Add(obj);
                }
            }
            return lstCheckedDetail;
        }

        private void SetRateAfterDetailChanged()
        {
            var lstCheckedDetail = GetListCheckedDetail();
            for (int i = 1; i < treeNodeGrid.Rows; i++)
            {
                string rate = treeNodeGrid.Cell(i, 2).Text.Trim();
                var id = treeNodeGrid.Cell(i, 0).Tag.Trim();
                //获取当前已选明细对应的节点,
                var lstobj = lstRelation.Where(p => lstCheckedDetail.Any(m => m.Id == p.SourseGWBSDetailID) && p.TargetGWBSTreeID == id).ToList();
                //如果相同，则说明一一对应关系都存在
                if (lstobj != null && lstobj.Count > 0 && lstobj.Count == lstCheckedDetail.Count)
                {
                    var obj = lstobj[0];
                    if (lstobj.Count == 1)
                    {
                        treeNodeGrid.Cell(i, 2).Text = (obj.Rate * 100).ToString();
                    }
                    else
                    {
                        bool isRateMatch = true;
                        for (int j = 1; j < lstobj.Count; j++)
                        {
                            if (obj.Rate != lstobj[j].Rate)
                            {
                                isRateMatch = false;
                            }
                        }
                        //如果包含当前节点，则设置相应的值，否则情况
                        if (isRateMatch)
                        {
                            treeNodeGrid.Cell(i, 2).Text = (obj.Rate * 100).ToString();
                        }
                        else
                        {
                            treeNodeGrid.Cell(i, 2).Text = string.Empty;
                        }
                    }
                }
                else//不是全部都有，故清空
                {
                    treeNodeGrid.Cell(i, 2).Text = string.Empty;
                }

            }
        }

        private void UpdateDetail(GWBSDetail targetDetail, GWBSDetail sourseDetail, decimal rate)
        {
            targetDetail.ContractProjectQuantity = sourseDetail.ContractProjectQuantity * rate;// 0;
            targetDetail.ContractTotalPrice = sourseDetail.ContractTotalPrice * rate;// 0;
            targetDetail.PlanWorkAmount = sourseDetail.PlanWorkAmount * rate;// 0;
            targetDetail.PlanTotalPrice = sourseDetail.PlanTotalPrice * rate;//0;
            targetDetail.ResponsibilitilyWorkAmount = sourseDetail.ResponsibilitilyWorkAmount * rate;//0;
            targetDetail.ResponsibilitilyTotalPrice = sourseDetail.ResponsibilitilyTotalPrice * rate;//0; 
            targetDetail.AddupAccFigureProgress = GetProgressPercent(targetDetail.AddupAccQuantity, targetDetail.PlanWorkAmount);//targetDetail.PlanWorkAmount == 0 ? 0 : Math.Round(targetDetail.AddupAccQuantity / targetDetail.PlanWorkAmount, 4) * 100;
            targetDetail.ProgressConfirmed = GetProgressPercent(targetDetail.QuantityConfirmed, targetDetail.PlanWorkAmount);// targetDetail.PlanWorkAmount == 0 ? 0 : Math.Round(targetDetail.QuantityConfirmed / targetDetail.PlanWorkAmount, 4) * 100;
            targetDetail.State = DocumentState.InExecute;//
            if (sourseDetail.ListCostSubjectDetails == null || sourseDetail.ListCostSubjectDetails.Count == 0)
            {
                return;
            }

            if (targetDetail.ListCostSubjectDetails == null || targetDetail.ListCostSubjectDetails.Count == 0)
            {
                targetDetail = sourseDetail.CloneByRate(targetDetail.TheGWBS, contractGroup, rate);
                return;
            }

            //如果soursedetail中存在,源targetdetail中不存在则新增
            foreach (GWBSDetailCostSubject sourseCost in sourseDetail.ListCostSubjectDetails)
            {
                GWBSDetailCostSubject targetCost = sourseDetail.ListCostSubjectDetails.FirstOrDefault(p => p.ForwardCostSubjectId == sourseCost.Id); ;
                //由于前期未加字段ForwardCostSubjectId,故保留原有的匹配方式
                if (targetCost == null)
                {
                    targetCost = sourseDetail.ListCostSubjectDetails.FirstOrDefault(p => sourseCost.ResourceTypeGUID == p.ResourceTypeGUID
                        && sourseCost.CostAccountSubjectGUID == p.CostAccountSubjectGUID
                        && sourseCost.Name == p.Name
                        && sourseCost.DiagramNumber == p.DiagramNumber);
                }

                //如果存在则更新,否则新增
                if (targetCost == null)
                {
                    targetCost = sourseCost.CloneByRate(sourseDetail.TheGWBS, sourseDetail, rate);
                    targetDetail.ListCostSubjectDetails.Add(targetCost);
                }
                //源targetdetail中也存在,则更新
                //else
                //{
                //    UpdateDetailCostSubject(targetCost, sourseCost, rate);
                //}
            }
            //如果targetdetail中存在,源soursedetail中也存在,则更新,源soursedetail中不存在则删除
            foreach (GWBSDetailCostSubject targetCost in targetDetail.ListCostSubjectDetails)
            {
                GWBSDetailCostSubject sourseCost = null;
                //由于前期未加字段ForwardCostSubjectId,故保留原有的匹配方式
                if (!string.IsNullOrEmpty(targetCost.ForwardCostSubjectId))
                {
                    sourseCost = sourseDetail.ListCostSubjectDetails.FirstOrDefault(p => p.Id == targetCost.ForwardCostSubjectId);
                }
                else
                {
                    sourseCost = sourseDetail.ListCostSubjectDetails.FirstOrDefault(p => targetCost.ResourceTypeGUID == p.ResourceTypeGUID
                        && targetCost.CostAccountSubjectGUID == p.CostAccountSubjectGUID
                        && targetCost.Name == p.Name
                        && targetCost.DiagramNumber == p.DiagramNumber);
                }

                if (sourseCost == null)
                {
                    continue;
                }
                UpdateDetailCostSubject(targetCost, sourseCost, rate);
            }
        }

        private void UpdateDetailCostSubject(GWBSDetailCostSubject oTempCostSubject, GWBSDetailCostSubject sourseCostSub, decimal rate)
        {
            oTempCostSubject.ContractProjectAmount = sourseCostSub.ContractProjectAmount * rate;// 0;
            oTempCostSubject.ContractQuotaQuantity = sourseCostSub.ContractQuotaQuantity;// 0;
            oTempCostSubject.ContractTotalPrice = sourseCostSub.ContractTotalPrice * rate;// 0;
            //oTempCostSubject.CurrentPeriodAccountCost *= rate;// 0;
            //oTempCostSubject.CurrentPeriodAccountProjectAmount *= rate;// 0;
            //oTempCostSubject.CurrentPeriodBalanceProjectAmount *= rate;// 0;
            //oTempCostSubject.CurrentPeriodBalanceTotalPrice *= rate;// 0;
            oTempCostSubject.PlanTotalPrice = sourseCostSub.PlanTotalPrice * rate;//0;
            oTempCostSubject.PlanWorkAmount = sourseCostSub.PlanWorkAmount * rate;//0;
            oTempCostSubject.PlanQuotaNum = sourseCostSub.PlanQuotaNum;//0;
            //oTempCostSubject.ProjectAmountWasta = sourseCostSub.ProjectAmountWasta * rate;// 0;
            oTempCostSubject.ResponsibilitilyTotalPrice = sourseCostSub.ResponsibilitilyTotalPrice * rate;//0;
            oTempCostSubject.ResponsibilitilyWorkAmount = sourseCostSub.ResponsibilitilyWorkAmount * rate;//0;
            oTempCostSubject.ResponsibleQuotaNum = sourseCostSub.ResponsibleQuotaNum;//0;
            oTempCostSubject.ForwardCostSubjectId = sourseCostSub.Id;
        }

        private decimal GetProgressPercent(decimal accQuantity, decimal planQuantity)
        {
            return planQuantity == 0 ? 0 : Math.Round(accQuantity / planQuantity, 4) * 100;
        }
        #endregion
    }
}
