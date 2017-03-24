using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using CustomServiceClient.CustomWebSrv;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CompleteSettlementBook
{
    public partial class VCompleteQuery : TBasicDataView
    {
        private MCompleteMng model = new MCompleteMng();
        public VCompleteQuery()
        {
            InitializeComponent();
            InitData();
            InitEvent();

        }
        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpCreateDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpCreateDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            //this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            //dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        //双击事件
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VCompleteMng vc = new VCompleteMng();
                    vc.Start(billId);
                    vc.ShowDialog();
                }
            }
        }

        //void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
        //    DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
        //    if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
        //    if (dgvCell.OwningColumn.Name == colCode.Name)
        //    {
        //        DemandMasterPlanMaster master = model.DemandPlanSrv.GetDemandMasterPlanByCode(dgvCell.Value.ToString());
        //        VComplete vc = new VComplete();
        //        vmro.CurBillMaster = master;
        //        vmro.Preview();
        //    }
        //}
        //不用改的
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            condition += "and ProjectId = '" + projectInfo.Id + "'";
            //文档名称
            if (this.txtContractDocName.Text != "")
            {
                condition = condition + " and ContractDocName like '%" + this.txtContractDocName.Text + "%'";//精确查询
             }
            //condition += " and t1.CreateDate>='" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            //计划结算完成时间
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and plantime>='" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "' and plantime<'" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and plantime>=to_date('" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and plantime<to_date('" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            //负责人
            if (this.txtHandlePerson.Text != "")
            {
                condition = condition + " and HandlePersonName like '%" + txtHandlePerson.Text + "%'";
            }
            ////制单人
            //if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            //{
            //    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
            //}

            //项目名称
            if (this.txtProjectName.Text != "")
            {
                condition = condition + "and   PROJECTNAME like '%" + this.txtProjectName.Text + "%'";
            }
           
            #endregion
            DataSet dataSet = model.CompleteSrv.CompleteRelationQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal endMoney = 0;
            decimal realcost = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                dgDetail[No.Name, rowIndex].Value = rowIndex+1;
                string a = dataRow["PlanTime"].ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dgDetail[CreateTime.Name, rowIndex].Value = stra;//创建时间
                //dgDetail[ProjectName.Name, rowIndex].Value = dataRow["ProjectName"].ToString();//项目名称
                dgDetail[ContractDocName.Name, rowIndex].Value = dataRow["ContractDocName"].ToString();//文档名称
                dgDetail[Handleperson.Name, rowIndex].Value = dataRow["HandlePersonName"].ToString();//责任人
                //string c = dataRow["PlanTime"].ToString();
                //string[] aArray2 = c.Split(' ');
                //string st = aArray2[0];
                //dgDetail[PlanTime.Name, rowIndex].Value = st;//计划结算完成时间

                string b = dataRow["EndTime"].ToString();
                string[] aArray1 = b.Split(' ');
                string str = aArray1[0];
                dgDetail[RealTime.Name, rowIndex].Value = str;//实际完成结算时间

                decimal Money = ClientUtil.ToDecimal(dataRow["SubmitMoney"].ToString());
                Money = Money / 10000;
                dgDetail[SubmitMoney.Name,rowIndex].Value = Money.ToString();//报送总金额


                decimal zhenqu = ClientUtil.ToDecimal(dataRow["ZhenquMoney"].ToString());
                zhenqu=zhenqu/10000;
                dgDetail[ZhengquMoney.Name, rowIndex].Value = zhenqu.ToString(); ;//争取结算金额

                decimal sure = ClientUtil.ToDecimal(dataRow["SureMoney"].ToString());
                sure =sure/10000;
                dgDetail[SureMoney.Name, rowIndex].Value = sure.ToString();//确保结算金额

                decimal Begin = ClientUtil.ToDecimal(dataRow["BeginMoney"].ToString());
                Begin=Begin/10000;
                dgDetail[BeginMoney.Name, rowIndex].Value = Begin.ToString();//初次审定总金额

                decimal End = ClientUtil.ToDecimal(dataRow["ShendingMoney"]);
                End=End/10000;
                dgDetail[EndMoney.Name, rowIndex].Value = End.ToString();//审定总金额
                object shendingMoney = End.ToString();
                if (shendingMoney != null)
                {
                    endMoney += decimal.Parse(shendingMoney.ToString());
                }
                decimal cost = ClientUtil.ToDecimal(dataRow["RealMoney"].ToString());
                cost=cost/10000;
                dgDetail[RealCost.Name, rowIndex].Value = cost.ToString();//实际成本
                object realMoney = cost.ToString();
                if (realMoney != null)
                {
                    realcost += decimal.Parse(realMoney.ToString());
                }
                decimal bene = ClientUtil.ToDecimal( dataRow["Benefit"].ToString());
                bene=bene/10000;
                dgDetail[Benefit.Name, rowIndex].Value = bene.ToString();//效益额
                dgDetail[Benefitlv.Name, rowIndex].Value = dataRow["Bennefitlv"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                }
            }
            this.txtSumQuantity.Text = endMoney.ToString("#,###.####");
            this.txtSumMoney.Text = realcost.ToString("#,###,####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
