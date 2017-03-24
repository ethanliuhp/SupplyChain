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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    /// <summary>
    /// 工程成本按比例分摊
    /// </summary>
    public partial class VPlanProjectAmountShareByRate : TBasicDataView
    {
        private List<GWBSTree> LstChildGWBSTree = new List<GWBSTree>();
        private List<GWBSDetail> LstDetails = null;
        private ContractGroup ContractGroup = null;
        MGWBSTree model = new MGWBSTree();
        //public VPlanProjectAmountShareByRate()
        //{
        //    InitializeComponent();
        //}   

        public VPlanProjectAmountShareByRate(string strWBSTreeId)
        {
            InitializeComponent();
            InitData(strWBSTreeId);
            InitEvent();
        }
        public void InitData(string strWBSTreeId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", strWBSTreeId));
            oq.AddOrder(Order.Asc("OrderNo"));
            LstChildGWBSTree = model.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>().ToList();

            //1：分摊时是否删除原有的详细，耗用【目前是按照直接分摊，不删除已添加的处理】
            //2：获取当前节点的明细，明细，资源耗用  
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBS.Id", strWBSTreeId));
            oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            LstDetails = model.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>().ToList();

            if (LstDetails != null && LstDetails.Count > 0)
            {
                var objdetail = LstDetails[0];
                ContractGroup = new ContractGroup()
                {
                    Code = objdetail.ContractGroupCode,
                    Id = objdetail.ContractGroupGUID,
                    ContractName = objdetail.ContractGroupName,
                    ContractGroupType = objdetail.ContractGroupType
                };
            }
            InitGrid();
            ModelToView();
        }

        public void InitEvent()
        {
            btnAvgShare.Click += new EventHandler(btnAvgShare_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        #region 事件
        void btnAvgShare_Click(object sender, EventArgs e)
        {
            grid1.AutoRedraw = false;
            int avgRate = 100 / LstChildGWBSTree.Count;
            int lastAvgRate = 100;
            //前n-1项取平均值，最后一项取
            for (int i = 1; i < LstChildGWBSTree.Count; i++)
            {
                grid1.Cell(i, 2).Text = avgRate.ToString();
                lastAvgRate = lastAvgRate - avgRate;
            }
            grid1.Cell(LstChildGWBSTree.Count, 2).Text = lastAvgRate.ToString();

            grid1.Refresh();
            grid1.AutoRedraw = true;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRate())
                {
                    model.SaveOrUpdateDetail((IList)ViewToModel());
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

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private bool ValidateRate()
        {
            decimal sumRate = 0;
            for (int i = 1; i < grid1.Rows - 1; i++)
            {
                sumRate += ClientUtil.ToDecimal(grid1.Cell(i, 2).Text.Trim());
            }
            return sumRate == 100;
        }

        #region Flex处理
        private void InitGrid()
        {
            int iCol, iRow;
            grid1.AutoRedraw = false;
            grid1.Rows = LstDetails == null ? 10 : LstDetails.Count + 1;
            grid1.Cols = 3;
            grid1.Locked = false;
            grid1.Cell(0, 0).Locked = true;
            grid1.Row(0).Locked = false;
            grid1.DisplayRowNumber = true;
            grid1.StartRowNumber = 1;
            grid1.Column(0).Visible = true;
            grid1.Column(0).AutoFit();
            grid1.SelectionMode = FlexCell.SelectionModeEnum.Free;
            grid1.DisplayFocusRect = true;
            grid1.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            grid1.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            grid1.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            grid1.BackColorBkg = SystemColors.Control;
            grid1.DefaultFont = new Font("Tahoma", 8);

            //前置节点行号
            iRow = 0; iCol = 1;
            grid1.Cell(iRow, iCol).Text = "任务节点名称";
            grid1.Column(iCol).Locked = true;

            //前置任务名称
            iCol++;
            grid1.Cell(iRow, iCol).Text = "百分比（%）";

            grid1.AutoRedraw = true;
            grid1.Refresh();
        }

        private List<GWBSDetail> ViewToModel()
        {
            List<GWBSDetail> lstDetailNew = new List<GWBSDetail>();
            //遍历节点
            for (int i = 0; i < grid1.Rows - 1; i++)
            {
                int intRow = i + 1;
                string strId = grid1.Cell(intRow, 0).Tag.Trim();
                decimal percent = ClientUtil.ToDecimal(grid1.Cell(intRow, 2).Text.Trim()) / 100;

                GWBSTree obj = LstChildGWBSTree.FirstOrDefault(p => p.Id == strId);
                if (obj != null)
                {
                    //遍历详细，按照比例进行复制摊派
                    for (int j = 0; j < LstDetails.Count; j++)
                    {
                        GWBSDetail objDetail = LstDetails[j].CloneByRate(obj, ContractGroup, percent);
                        lstDetailNew.Add(objDetail);
                    }
                }
            }
            return lstDetailNew;
        }

        private void ModelToView()
        {
            grid1.AutoRedraw = false;
            int intRow = 0;
            GWBSTree item = null;
            for (int i = 0; i < LstChildGWBSTree.Count; i++)
            {
                intRow = i + 1;
                item = LstChildGWBSTree[i];
                grid1.Cell(intRow, 0).Tag = item.Id;
                grid1.Cell(intRow, 1).Text = item.Name;
            }

            grid1.Column(0).AutoFit();
            grid1.Column(1).Width = 300;
            grid1.Column(2).Width = 100;

            grid1.Refresh();
            grid1.AutoRedraw = true;
        }
        #endregion
    }
}
