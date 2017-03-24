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
using VirtualMachine.Core;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage
{
    public partial class VConstructionDesignQuery : TBasicDataView
    {
        private MProjectPlanningMng model = new MProjectPlanningMng();

        public VConstructionDesignQuery()
        {
            InitializeComponent();
            InitEvents();
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
                    VConstructionDesign vDesign = new VConstructionDesign();
                    vDesign.Start(billId);
                    vDesign.ShowDialog();
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
            //项目名称
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
            condition += "and t1.DocState <>" + "0" + "";
       
            string planninglevel = "施工组织设计";
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
                //提交时间
                if (ClientUtil.ToDateTime(dataRow["SubmitDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colSubmitDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["SubmitDate"]).ToString();     //提交时间;
                }
                //业务时间
                if (ClientUtil.ToDateTime(dataRow["CreateDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colCreate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();     //业务时间;
                }
                dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                dgDetail[colRemark.Name, rowIndex].Value = dataRow["Descript"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                }
                object objState = dataRow["DocState"];
                if (objState != null)
                {
                    dgDetail[colDcoState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
            }

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}