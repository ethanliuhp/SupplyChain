using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using System.Collections;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.TotalDemandPlan
{
    public partial class VTotalDemandAnalysis : TBasicDataView
    {
        private MTotalDemandPlanMng model = new MTotalDemandPlanMng();
        private ResourceRequirePlan recPlan;
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public ResourceRequirePlan RecPlan
        {
            get { return recPlan; }
            set { recPlan = value; }
        }
        public VTotalDemandAnalysis(ResourceRequirePlan Plan)
        {
            InitializeComponent();
            RecPlan = Plan;
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            //下拉列表中加载滚动需求计划信息
            ObjectQuery oq = new ObjectQuery();
            IList list = model.ResourceRequirePlanSrv.ObjectQuery(typeof(ResourceRequirePlan), oq);
            foreach (ResourceRequirePlan bdo in list)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = bdo.RequirePlanVersion;
                li.Value = bdo.Id;
                comPlanType.Items.Add(li);
            }
        }

        private void InitEvent()
        {
            this.btnTotalAnalysis.Click +=new EventHandler(btnTotalAnalysis_Click);
            this.btnGaveup.Click += new EventHandler(btnGaveup_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnMoreData.Click +=new EventHandler(btnMoreData_Click);
            this.checkBox1.Click +=new EventHandler(checkBox1_Click);
            this.checkBox2.Click +=new EventHandler(checkBox2_Click);
        }

        void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
            }
            else
            {
                checkBox2.Checked = true;
            }
        }
        void checkBox2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }
        }
       
        //总量分析
        void btnTotalAnalysis_Click(object sender,EventArgs e)
        {
            System.Web.UI.WebControls.ListItem li = comPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
            if (li != null)
            {
                string strId = li.Value;
                //通过滚动需求计划主表查总量需求计划单明细信息
                Hashtable rrrht = model.ResourceRequirePlanSrv.GetTotalPlan(strId);
                dgDetail.Rows.Clear();
                if (rrrht.Count > 0)
                {
                    foreach (System.Collections.DictionaryEntry objht in rrrht)
                    {
                        ResourceRequirePlanDetail dtl = new ResourceRequirePlanDetail();
                        dtl = objht.Key as ResourceRequirePlanDetail;
                        int rowIndex = dgDetail.Rows.Add();
                        DataGridViewRow row = dgDetail.Rows[rowIndex];
                        row.Cells[colMaterialType.Name].Value = dtl.ResourceTypeClassification;
                        row.Cells[colMaterialType.Name].Tag = dtl.ResourceCategory;
                        row.Cells[colArmourDemand.Name].Value = dtl.DiagramNumber;
                        row.Cells[colQuantityUnit.Name].Tag = dtl.QuantityUnitGUID;
                        row.Cells[colQuantityUnit.Name].Value = dtl.QuantityUnitName;
                        row.Cells[colTotalDemand.Name].Value = ClientUtil.ToString(dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity);
                        row.Cells[colMonthDemand.Name].Value = dtl.MonthPlanPublishQuantity;
                        row.Cells[colDaliyDemand.Name].Tag = dtl.DailyPlanPublishQuantity;
                        row.Cells[colZHX.Name].Tag = dtl.ExecutedQuantity;
                        row.Tag = dtl;
                    }
                }
                Hashtable ht = model.ResourceRequirePlanSrv.GetTotalReceipt(strId);
                if (ht.Count > 0)
                {
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        foreach (System.Collections.DictionaryEntry objht in ht)
                        {
                            ResourceRequireReceiptDetail dtl = new ResourceRequireReceiptDetail();
                            dtl = objht.Key as ResourceRequireReceiptDetail;


                            ResourceRequirePlanDetail planDtl = new ResourceRequirePlanDetail();
                            planDtl = var.Tag as ResourceRequirePlanDetail;
                            if (dtl.MaterialResource.Id == planDtl.MaterialResource.Id)
                            {
                                var.Cells[colAppDemand.Name].Value = ClientUtil.ToString(dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity);

                                if (checkBox2.Checked)
                                {
                                    if (ClientUtil.ToDecimal(var.Cells[colTotalDemand.Name].Value) < ClientUtil.ToDecimal(var.Cells[colAppDemand.Name].Value))
                                    {
                                        var.Visible = true;
                                    }
                                    else
                                    {
                                        var.Visible = false;
                                    }
                                }
                                else
                                {
                                    var.Visible = true;
                                }
                                break;
                            }
                            if (checkBox2.Checked)
                            {
                                var.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        //详细数据
        void btnMoreData_Click(object sender, EventArgs e)
        {
             System.Web.UI.WebControls.ListItem li = comPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
             if (li != null)
             {
                 string strId = li.Value;
                 IList lists = new ArrayList();
                 MaterialCategory category = new MaterialCategory();
                 category = dgDetail.CurrentRow.Cells[colMaterialType.Name].Tag as MaterialCategory;
                 string strArmour = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colArmourDemand.Name].Value);
                 string strUnit = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value);
                 string strGDDemand = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colTotalDemand.Name].Value);
                 string strAppDemand = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colAppDemand.Name].Value);
                 string strMonthDemand = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colMonthDemand.Name].Value);
                 string strDailyDemand = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colDaliyDemand.Name].Value);
                 string strTotalDemand = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colZHX.Name].Value);
                 lists.Add(strId);
                 lists.Add(category);
                 lists.Add(strArmour);
                 lists.Add(strUnit);
                 lists.Add(strGDDemand);
                 lists.Add(strAppDemand);
                 lists.Add(strMonthDemand);
                 lists.Add(strDailyDemand);
                 lists.Add(strTotalDemand);
                 VDetailInformation information = new VDetailInformation(lists);
                 information.ShowDialog();
             }
           
        }
        //关闭
        void btnGaveup_Click(object sender, EventArgs e)
        {
            this.btnGaveup.FindForm().Close();
        }
        //导出excel
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
       
    }
}
