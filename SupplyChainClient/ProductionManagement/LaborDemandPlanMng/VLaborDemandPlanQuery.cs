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
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng
{
    public partial class VLaborDemandPlanQuery : TBasicDataView
    {
        private MLaborDemandPlanMng model = new MLaborDemandPlanMng();
        public VLaborDemandPlanQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitUsedRankType();
        }

        private IList list = new ArrayList();
        /// <summary>
        /// 保存结果
        /// </summary>
        virtual public IList List
        {
            get { return list; }
            set { list = value; }
        }

        private void InitData()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitUsedRankType()
        {
            //添加劳务队伍类型下拉框
            VBasicDataOptr.InitUsedRankType(txtUsedRankType, false);
            //comSpecailType.ContextMenuStrip = cmsDg;
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                LaborDemandPlanMaster master = model.LaborDemandPlanSrv.GetLaborDemandPlanByCode(ClientUtil.ToString(dgvCell.Value));
                VLaborDemandPlan vWasteApply = new VLaborDemandPlan();
                vWasteApply.CurBillMaster = master;
                vWasteApply.Preview();
            }
            else
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                    if (ClientUtil.ToString(billId) != "")
                    {
                        VLaborDemandPlan vOrder = new VLaborDemandPlan();
                        vOrder.Start(billId);
                        vOrder.ShowDialog();
                    }
                }
            }
        }

        /// <summary>
        /// 工种列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colWokerType.Name))
            {
                string strId = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colDetailId.Name].Value);
                list = model.LaborDemandPlanSrv.GetWorkTypeByParentId(strId);
                VWokerSelector WokerType = new VWokerSelector();
                WokerType.Result = list;
                WokerType.ShowDialog();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                {
                    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";
                }
                else
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                }
            }
            //业务日期
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }
            //制单人
            if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
            }
            //计划名称
            if (this.txtPlanName.Text != "")
            {
                condition += " and t1.PlanName like '%" + this.txtPlanName.Text + "%'";
            }
            //任务名称
            if (this.txtProjectTask.Text != "")
            {
                condition += " and t2.ProjectTask like '%" + this.txtProjectTask.Text + "%'";
            }
            //劳务队伍类型
            if (this.txtUsedRankType.SelectedItem != null)
            {
                condition += "and t2.UsedRankType = '" + this.txtUsedRankType.SelectedItem + "'";
            }
            #endregion
            DataSet dataSet = model.LaborDemandPlanSrv.LaborDemandPlanQuery(condition);
            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCode.Name, rowIndex].Value = dataRow["code"].ToString();
                dgDetail[colDetailId.Name, rowIndex].Value = dataRow["Id"].ToString();
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                string strId = ClientUtil.ToString(dataRow["Id"]);
                list = model.LaborDemandPlanSrv.GetWorkTypeByParentId(strId);
                if (list.Count == 0)
                {
                    dgDetail[colWokerType.Name, rowIndex].Value = "无工种";
                }
                else
                {
                    dgDetail[colWokerType.Name, rowIndex].Value = "有工种";
                }
                dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                string b = dataRow["CreateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colCreateDate.Name, rowIndex].Value = strb;
                dgDetail[colEstimateProjectQuantity.Name, rowIndex].Value = dataRow["EstimateProjectQuantity"].ToString();
                dgDetail[colEstimateProjectTimeLimit.Name, rowIndex].Value = dataRow["EstimateProjectTimeLimit"].ToString();
                string a = dataRow["LaborRankInTime"].ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dgDetail[colLaborRankInTime.Name, rowIndex].Value = stra;
                dgDetail[colMainJobDescript.Name, rowIndex].Value = dataRow["MainJobDescript"].ToString();
                dgDetail[colPlanName.Name, rowIndex].Value = dataRow["PlanName"].ToString();
                dgDetail[colProjectQuantityUnit.Name, rowIndex].Value = dataRow["ProjectQuantityUnit"].ToString();
                dgDetail[colProjectTask.Name, rowIndex].Value = dataRow["ProjectTaskName"].ToString();
                dgDetail[colProjectTimeLimitUnit.Name, rowIndex].Value = dataRow["ProjectTimeLimitUnitName"].ToString();
                dgDetail[colQualitySafetyRequirement.Name, rowIndex].Value = dataRow["QualitySafetyRequirement"].ToString();
                dgDetail[colUsedRankType.Name, rowIndex].Value = dataRow["UsedRankType"].ToString();
                dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();
                dgDetail[colPlanLaborDemandNumber.Name, rowIndex].Value = dataRow["PlanLaborDemandNumber"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                }
                //dgDetail[colWokerType.Name,rowIndex].Value = dataRow["WokerType"];
            }
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
