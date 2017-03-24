using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage
{
    public partial class VWebSitePlanQuery : TBasicDataView
    {
        private MProjectPlanningMng model = new MProjectPlanningMng();

        public VWebSitePlanQuery()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        public void InitData()
        {
            VBasicDataOptr.InitEnginnerType(cmbEnginnerType, false);
            cmbEvaluationWay.Items.AddRange(new object[] { "项目内部评审", "公司评审", "专家评审" });
        }

        public void InitEvents()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VWebSitePlan vPlan = new VWebSitePlan();
                    vPlan.Start(billId);
                    vPlan.ShowDialog();
                }
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            projectInfo = StaticMethod.GetProjectInfo();
            condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
            //专业策划名称
            if (this.txtEnginnerName.Text != "")
            {
                condition = condition + " and t1.EnginnerName like '%" + this.txtEnginnerName.Text + "%'";
            }
            //提交时间
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.SubmitDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.SubmitDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.SubmitDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.SubmitDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }
            //负责人
            if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null)
            {
                condition = condition + " and t1.CreatePersonName='" + (txtCreatePerson.Result[0] as PersonInfo).Name + "'";
            }
            //专业策划类型
            if (this.cmbEnginnerType.SelectedItem != null)
            {
                condition += "and t1.EnginnerType = '" + cmbEnginnerType.SelectedItem + "'";
            }
            //评审方式
            if (this.cmbEvaluationWay.SelectedItem != null)
            {
                string evaluationway;
                if (cmbEvaluationWay.SelectedItem.Equals("项目内部评审"))
                {
                    evaluationway = ClientUtil.ToString("0");
                    condition += "and t1.EvaluationWay = '" + evaluationway + "'";
                }
                if (cmbEvaluationWay.SelectedItem.Equals("公司评审"))
                {
                    evaluationway = ClientUtil.ToString("1");
                    condition += "and t1.EvaluationWay = '" + evaluationway + "'";
                }
                if (cmbEvaluationWay.SelectedItem.Equals("专家评审"))
                {
                    evaluationway = ClientUtil.ToString("2");
                    condition += "and t1.EvaluationWay = '" + evaluationway + "'";
                }
            }
            condition += "and t1.DocState <>" + "0" + "";
            string planninglevel = "专业策划";
            condition = condition + " and t1.PlanningLevel='" + planninglevel + "'";
            #endregion
            DataSet dataSet = model.ProjectPlanningSrv.SpecialityProposalQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();

                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                dgDetail[colEnginnerName.Name, rowIndex].Value = dataRow["EnginnerName"].ToString(); //项目名称
                if (ClientUtil.ToDateTime(dataRow["SubmitDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colSubmitDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["SubmitDate"]).ToString();                     //提交时间
                }

                if (ClientUtil.ToDateTime(dataRow["CreateDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();     //业务时间;
                }
                dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                dgDetail[colRemark.Name, rowIndex].Value = dataRow["Descript"].ToString();
                dgDetail[colEnginnerType.Name, rowIndex].Value = dataRow["EnginnerType"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                }
                //dgDetail[colEvaluationWay.Name, rowIndex].Value = dataRow["EvaluationWay"].ToString();
                string evaluationway = dataRow["EvaluationWay"].ToString();
                if (evaluationway.Equals("0"))
                {
                    dgDetail[colEvaluationWay.Name, rowIndex].Value = "项目内部评审";
                }
                if (evaluationway.Equals("1"))
                {
                    dgDetail[colEvaluationWay.Name, rowIndex].Value = "公司评审";
                }
                if (evaluationway.Equals("2"))
                {
                    dgDetail[colEvaluationWay.Name, rowIndex].Value = "专家评审";
                }
                object objState = dataRow["DocState"];
                if (objState != null)
                {
                    dgDetail[colDcoState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
            }
            //lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
