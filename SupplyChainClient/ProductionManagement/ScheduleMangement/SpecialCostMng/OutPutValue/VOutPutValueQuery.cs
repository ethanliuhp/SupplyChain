using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Collections;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;


namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue
{
    public partial class VOutPutValueQuery : TBasicDataView
    {
        private MOutPutValue model = new MOutPutValue();
        private int baseYear = 1990;
        private int baseStartMonth = 1;
        private int baseEndMonth = 1;
        public VOutPutValueQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            //时间下拉框赋值
            List<int> year = new List<int>();
            for (int i = baseYear; i < baseYear + 100; i++)
            {
                year.Add(i);
            }
            this.cmoYear.DataSource = year;
            this.cmoYear.Text = ConstObject.LoginDate.Year.ToString();
            List<int> startMonth = new List<int>();
            for (int i = baseStartMonth; i < baseStartMonth + 12; i++)
            {
                startMonth.Add(i);
            }
            List<int> endMonth = new List<int>();
            for (int i = baseEndMonth; i < baseEndMonth + 12; i++)
            {
                endMonth.Add(i);
            }

            this.cmoEndMonth.DataSource = endMonth;
            this.cmoEndMonth.Text = ConstObject.LoginDate.Month.ToString();
            this.cmoStartMonth.DataSource = startMonth;
            this.cmoStartMonth.Text = (ClientUtil.ToInt(cmoEndMonth.Text) - 1).ToString();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                txtPrjName.Tag = projectInfo;
                txtPrjName.Text = projectInfo.Name;
            }
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            this.btnSearchPrj.Click += new EventHandler(btnSearchPrj_Click);
        }

        //项目选择按钮事件
        void btnSearchPrj_Click(object sender, EventArgs e)
        {
            //VDepartSelector frm = new VDepartSelector("1");
            VSelectProjectInfo frm = new VSelectProjectInfo();
            frm.ShowDialog();

            if (frm.Result != null && frm.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    this.txtPrjName.Text = selectProject.Name;
                }
            }

        }
        //任务节点按钮事件
        void btnSearchGWBS_Click(object sender, EventArgs e)
        {
            DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];
                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    this.txtGWBS.Text = task.Name;
                    this.txtGWBS.Tag = task;
                }
            }
        }

        //Excel按钮事件
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        //查询按钮事件
        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理

            //验证结束月与起始月
            if (ClientUtil.ToInt(this.cmoStartMonth.Text) > ClientUtil.ToInt(this.cmoEndMonth.Text))
            {
                MessageBox.Show("起始月必须小于等于结束月，请检查");
                return;
            }

            string condition = "";
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            projectInfo = StaticMethod.GetProjectInfo();
            condition += " and t1.ProjectId = '" + projectInfo.Id + "'";
            //项目名称验证
            //if (this.txtPrjName.Text != "")
            //{
            //    condition += " and t1.ProjectName like '%" + txtPrjName.Text + "%'";
            //}
            //年
            if (!string.IsNullOrEmpty(this.cmoYear.Text))
            {
                condition += " and t1.AccountYear = " + ClientUtil.ToInt(cmoYear.Text);
            }

            //起始月
            if (!string.IsNullOrEmpty(this.cmoStartMonth.Text))
            {
                condition += " and t1.AccountMonth >= " + ClientUtil.ToInt(cmoStartMonth.Text);
            }
            //结束月
            if (!string.IsNullOrEmpty(this.cmoEndMonth.Text))
            {
                condition += " and t1.AccountMonth <= " + ClientUtil.ToInt(cmoEndMonth.Text);
            }
            // GWBS
            if (this.txtGWBS.Text != "" && txtGWBS.Tag != null)
            {
                condition += " and t2.GWBSTreeSysCode like '" + (txtGWBS.Tag as GWBSTree).SysCode + "%'";
            }

            condition += " order by t1.AccountYear,t1.AccountMonth asc";

            #endregion

            //datagridview数据显示
            DataSet dataSet = model.SpecialCostSrv.OutPutValueQuery(condition);
            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            decimal sumPlanQuantity = 0;
            decimal sumRealQuantity = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["Id"]);
                dgDetail[colAccoutYear.Name, rowIndex].Value = ClientUtil.ToString(dataRow["AccountYear"]);
                dgDetail[colAccoutMonth.Name, rowIndex].Value = ClientUtil.ToString(dataRow["AccountMonth"]);
                dgDetail[colPlanValue.Name, rowIndex].Value = ClientUtil.ToString(dataRow["PlanValue"]);
                dgDetail[colActualValue.Name, rowIndex].Value = ClientUtil.ToString(dataRow["RealValue"]);
                dgDetail[colTaskPath.Name, rowIndex].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dataRow["GWBSTreeName"].ToString(), dataRow["GWBSTreeSysCode"].ToString());

                //合计
                object planQuantity = dataRow["PlanValue"];
                if (planQuantity != null)
                {
                    sumPlanQuantity += decimal.Parse(planQuantity.ToString());
                }
                //实际产值累加方法
                object realQuantity = dataRow["RealValue"];
                if (realQuantity != null)
                {
                    sumRealQuantity += decimal.Parse(realQuantity.ToString());
                }
                //显示累加结果
                this.txtPlanOutPutValue.Text = sumPlanQuantity.ToString("#,###.####");
                this.txtActualValue.Text = sumRealQuantity.ToString("#,###.####");
                if (sumPlanQuantity != 0)
                {
                    this.txtOutPutScale.Text = ((sumRealQuantity / sumPlanQuantity) * 100).ToString("#,###.##") + "%";
                }
            }

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");

        }
    }
}
