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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng
{
    public partial class VInspectionLotQuery : TBasicDataView
    {
        private MInspectionLotMng model = new MInspectionLotMng();
        public VInspectionLotQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            this.btnSearchProject.Click +=new EventHandler(btnSearchProject_Click);
        }

        //工程项目任务
        void btnSearchProject_Click(object sender,EventArgs e)
        {
            VWeekSelector vss = new VWeekSelector();
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;
            foreach (WeekScheduleDetail detail in list)
            {
                txtProjectTask.Text = detail.GWBSTreeName;
                txtProjectTask.Tag = detail.GWBSTree;
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
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
                InspectionLot master = model.InspectionLotSrv.GetInspectionLotByCode(dgvCell.Value.ToString());
                VInspectionLot vmro = new VInspectionLot();
                vmro.CurBillMaster = master;
                vmro.Preview();
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

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                {
                    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                }
                else
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                }
            }
            //制单日期
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

            //负责人
            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null && txtHandlePerson.Result.Count > 0)
            {
                condition += " and t1.txtHandlePerson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
            }

            //工程项目任务
            if (!txtProjectTask.Text.Trim().Equals(""))
            {
                condition += "and t1.ProjectTaskName = '"+ txtProjectTask.Text +"'";
            }
            #endregion
            DataSet dataSet = model.InspectionLotSrv.InspectionLotQuery(condition);
            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCode.Name, rowIndex].Value = dataRow["Code"];
                object objState = dataRow["State"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
                string b = dataRow["InsUpdateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colUpdateDate.Name, rowIndex].Value = strb;//验收结算时间
                dgDetail[colHandlePerson.Name,rowIndex].Value = dataRow["HandlePersonName"];
                dgDetail[colCreatePerson.Name,rowIndex].Value = dataRow["CreatePersonName"];
                string a = dataRow["CreateDate"].ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dgDetail[colCreateDate.Name, rowIndex].Value = stra;
                dgDetail[colProjectName.Name, rowIndex].Value = dataRow["ProjectTaskName"];
                dgDetail[colAccountStatue.Name, rowIndex].Value = dataRow["AccountStatus"];
            }
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
