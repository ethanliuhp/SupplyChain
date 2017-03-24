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
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage
{
    public partial class VGwbsManagedetails : TBasicToolBarByMobile
    {
        public MGWBSTree model = new MGWBSTree();
        private AutomaticSize automaticSize = new AutomaticSize();
        public GWBSTree DefaultGWBSTree = null;
        public VGwbsManagedetails()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitialEvent();
        }
        CurrentProjectInfo projectInfo = null;


        private void InitialEvent()
        {
            detailsSearchBtn.Click += new EventHandler(detailsSearchBtn_Click);
            detailsTxt.TextChanged += new EventHandler(detailsTxt_TextChanged);
            detailsTxt.DoubleClick += new EventHandler(detailsTxt_DoubleClick);
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            this.Load += new EventHandler(VGwbsManagedetails_Load);

        }

        void detailsTxt_DoubleClick(object sender, EventArgs e)
        {
            string interphaseID = this.Name;
            string controlID = "detailsTxt";
            string userID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            string oftenWord = detailsTxt.Text;
            VOftenWords vow = new VOftenWords(userID, interphaseID, controlID, oftenWord);
            vow.ShowDialog();
            string result = vow.Result;
            if (result != null)
            {
                detailsTxt.Text = result;
            }
        }

        void VGwbsManagedetails_Load(object sender, EventArgs e)
        {
            if (DefaultGWBSTree != null)
            {
                ObjectQuery oq = new ObjectQuery();
                //模糊查询（当两者条件符合其中一个 ，就可以查询）
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("TheGWBS.Id", DefaultGWBSTree.Id));
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                IList list = model.ObjectQuery(typeof(GWBSDetail), oq);

                dataGvDetails.Rows.Clear();
                //传值
                foreach (GWBSDetail item in list)
                {
                    int rowIndex = dataGvDetails.Rows.Add();
                    DataGridViewRow row = dataGvDetails.Rows[rowIndex];
                    row.Tag = item;
                    row.Cells[code.Name].Value = item.Code;
                    row.Cells[colDtlName.Name].Value = item.Name;
                    row.Cells[colDtlDesc.Name].Value = item.ContentDesc;

                    if (item.TheCostItem != null)
                    {
                        row.Cells[txtDtlCostItem.Name].Value = item.TheCostItem.Name;
                        row.Cells[txtDtlCostItem.Name].Value = item.TheCostItem.QuotaCode;
                    }
                    row.Cells[ResourceCol.Name].Value = item.MainResourceTypeName;
                    row.Cells[QuantitiesCol.Name].Value = item.QuantityConfirmed.ToString();
                    row.Cells[PlanCol.Name].Value = item.ProgressConfirmed.ToString();
                    row.Cells[StateCol.Name].Value = item.ProgressConfirmed.ToString();

                }

            }
        }


        private void detailsTxt_TextChanged(object sender, EventArgs e)
        {
            if (detailsTxt.Text.Trim() == "")
            {
                dataGvDetails.Rows.Clear();
            }
        }

        void detailsSearchBtn_Click(object sender, EventArgs e)
        {

            string text = detailsTxt.Text;

            this.dataGvDetails.Visible = true;

            if (text == "")
            {
                MessageBox.Show("搜索条件不可以为空，请检查！");
                return;
            }


            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            ObjectQuery oq = new ObjectQuery();
            //模糊查询（当两者条件符合其中一个 ，就可以查询）
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Or(Expression.Like("Name", text, MatchMode.Anywhere),
                Expression.Like("TheCostItem.Name", text, MatchMode.Anywhere)));
            //oq.AddCriterion(Expression.Like("TheGWBS.Id", text, MatchMode.Anywhere));
            IList list = model.ObjectQuery(typeof(GWBSDetail), oq);
            if (list.Count == 0)
            {
                MessageBox.Show("您所搜索的内容不存在，请检查！");
            }





            dataGvDetails.Rows.Clear();
            //传值
            foreach (GWBSDetail item in list)
            {
                int rowIndex = dataGvDetails.Rows.Add();
                DataGridViewRow row = dataGvDetails.Rows[rowIndex];
                row.Tag = item;
                row.Cells[code.Name].Value = item.Code;
                row.Cells[colDtlName.Name].Value = item.Name;
                row.Cells[colDtlDesc.Name].Value = item.ContentDesc;

                if (item.TheCostItem != null)
                {
                    row.Cells[txtDtlCostItem.Name].Value = item.TheCostItem.Name;
                    row.Cells[txtDtlCostItem.Name].Value = item.TheCostItem.QuotaCode;
                }
                row.Cells[ResourceCol.Name].Value = item.MainResourceTypeName;
                row.Cells[QuantitiesCol.Name].Value = item.QuantityConfirmed.ToString();
                row.Cells[PlanCol.Name].Value = item.ProgressConfirmed.ToString();
                row.Cells[StateCol.Name].Value = item.ProgressConfirmed.ToString();

            }




        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void VGwbsManagedetails_Load_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        
    }
}

