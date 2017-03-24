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
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;


namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue
{
    public partial class VRealOutputValueQuery : TBasicDataView
    {
        private MOutPutValue model = new MOutPutValue();
        private int baseYear = 1990;
        private int baseStartMonth = 1;
        private int baseEndMonth = 1;
        public VRealOutputValueQuery()
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
            SelectGWBSTreeNode();
        }
        private bool SelectGWBSTreeNode()
        {
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
                    return true;
                }
            }
            return false;
        }

        //Excel按钮事件
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        //查询按钮事件
        void btnSearch_Click(object sender, EventArgs e)
        {
            //验证结束月与起始月
            if (ClientUtil.ToInt(this.cmoStartMonth.Text) > ClientUtil.ToInt(this.cmoEndMonth.Text))
            {
                MessageBox.Show("起始月必须小于等于结束月，请检查");
                cmoStartMonth.Focus();
                return;
            }

            if (this.txtGWBS.Text.Trim() == "" || txtGWBS.Tag == null)
            {
                MessageBox.Show("请选择要项目任务！");
                if (SelectGWBSTreeNode() == false)
                    return;
            }

            GWBSTree wbs = txtGWBS.Tag as GWBSTree;

            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            projectInfo = StaticMethod.GetProjectInfo();

            string sqlQuery = "";
            string condition = "";
            DataSet dataSet = null;

            decimal sumPlanQuantity = 0;
            decimal sumRealQuantity = 0;

            List<ProduceSelfValueDetail> listQueryResult = new List<ProduceSelfValueDetail>();

            #region 计划产值汇总查询

            condition += " and t1.ProjectId = '" + projectInfo.Id + "'";

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

            condition += " and t2.GWBSTreeSysCode like '" + wbs.SysCode + "%'";


            sqlQuery = "select t1.accountyear,t1.accountmonth,sum(t2.PlanValue) PlanValue from thd_produceselfvaluemaster t1" +
                               " inner join thd_produceselfvaluedetail t2 on t1.id=t2.parentid" +
                               " where 1=1 " + condition +
                               " group by t1.accountyear,t1.accountmonth" +
                               " order by t1.accountyear,t1.accountmonth asc";

            dataSet = model.GwbsSrv.SearchSQL(sqlQuery);

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                ProduceSelfValueDetail dtl = new ProduceSelfValueDetail();
                dtl.Kjn = ClientUtil.ToInt(row["accountyear"]);
                dtl.Kjy = ClientUtil.ToInt(row["accountmonth"]);
                dtl.PlanValue = ClientUtil.ToDecimal(row["PlanValue"]);
                listQueryResult.Add(dtl);
            }
            #endregion

            #region 实际产值汇总查询

            condition = "";


            condition += " and t1.theprojectguid = '" + projectInfo.Id + "'";

            //年
            if (!string.IsNullOrEmpty(this.cmoYear.Text))
            {
                condition += " and t1.kjn = " + ClientUtil.ToInt(cmoYear.Text);
            }

            //起始月
            if (!string.IsNullOrEmpty(this.cmoStartMonth.Text))
            {
                condition += " and t1.kjy >= " + ClientUtil.ToInt(cmoStartMonth.Text);
            }
            //结束月
            if (!string.IsNullOrEmpty(this.cmoEndMonth.Text))
            {
                condition += " and t1.kjy <= " + ClientUtil.ToInt(cmoEndMonth.Text);
            }
            // GWBS
            condition += " and t2.accounttasknodesyscode like '" + (txtGWBS.Tag as GWBSTree).SysCode + "%'";


            sqlQuery = "select t1.kjn,t1.kjy,sum(t2.currincometotalprice) RealValue" +
                    " from thd_costmonthaccount t1" +
                    " inner join thd_costmonthaccountdtl t2 on t1.id=t2.parentid" +
                    " where 1=1 " + condition +
                    " group by t1.kjn,t1.kjy" +
                    " order by t1.kjn,t1.kjy asc";

            dataSet = model.GwbsSrv.SearchSQL(sqlQuery);

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int kjn = ClientUtil.ToInt(row["kjn"]);
                int kjy = ClientUtil.ToInt(row["kjy"]);
                var query = from d in listQueryResult
                            where d.Kjn == kjn && d.Kjy == kjy
                            select d;
                if (query.Count() > 0)
                {
                    ProduceSelfValueDetail dtl = query.ElementAt(0);
                    dtl.RealValue = ClientUtil.ToDecimal(row["RealValue"]);
                }
                else
                {
                    ProduceSelfValueDetail dtl = new ProduceSelfValueDetail();
                    dtl.Kjn = kjn;
                    dtl.Kjy = kjy;
                    dtl.RealValue = ClientUtil.ToDecimal(row["RealValue"]);
                    listQueryResult.Add(dtl);
                }
            }
            #endregion

            listQueryResult = (from d in listQueryResult
                               orderby d.Kjy ascending
                               orderby d.Kjn ascending
                               select d).ToList();//where d.PlanValue != 0 || d.RealValue != 0

            dgDetail.Rows.Clear();
            string wbsFullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
            foreach (ProduceSelfValueDetail dtl in listQueryResult)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];

                dgDetail[colAccoutYear.Name, rowIndex].Value = dtl.Kjn;
                dgDetail[colAccoutMonth.Name, rowIndex].Value = dtl.Kjy;
                dgDetail[colPlanValue.Name, rowIndex].Value = dtl.PlanValue.ToString("#,###.##");
                dgDetail[colActualValue.Name, rowIndex].Value = dtl.RealValue.ToString("#,###.##");
                dgDetail[colTaskPath.Name, rowIndex].Value = wbsFullPath;

                sumPlanQuantity += dtl.PlanValue;
                sumRealQuantity += dtl.RealValue;
            }

            //显示累加结果
            this.txtPlanOutPutValue.Text = sumPlanQuantity.ToString("#,###.####");
            this.txtActualValue.Text = sumRealQuantity.ToString("#,###.####");
            if (sumPlanQuantity != 0)
                this.txtOutPutScale.Text = ((sumRealQuantity / sumPlanQuantity) * 100).ToString("#,###.##") + "%";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");

        }
    }
}
