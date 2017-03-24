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
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng
{
    public partial class VContractExcuteQuery : TBasicDataView
    {
        private MContractExcuteMng model = new MContractExcuteMng();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        public VContractExcuteQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            ((ComboBox)comConstractType).Items.AddRange(Enum.GetNames(typeof(SubContractType)));
            //txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3;
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-" + CommonUtil.SupplierCatCode4;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            projectInfo = StaticMethod.GetProjectInfo();
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
            this.dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            this.dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);
        }

        void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = (dgMaster.CurrentRow.Tag as SubContractProject).Id;
                if (ClientUtil.ToString(billId) != "")
                {
                    VContractExcuteMng vOrder = new VContractExcuteMng();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                SubContractProject master = model.ContractExcuteSrv.GetContractExcuteByCode(dgvCell.Value.ToString());
                VContractExcuteMng vmro = new VContractExcuteMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
            SubContractProject pro = dgMaster.CurrentRow.Tag as SubContractProject;
            if (pro == null) return;
            FillDgDetail(pro);
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
            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            if (txtCodeBegin.Text != "" && txtCodeEnd.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));

                //oq.AddCriterion(Expression.Like("Code","%"+ txtCodeBegin.Text+"%"));
                //oq.AddCriterion(Expression.Like("Code","%"+ txtCodeEnd.Text+"%"));
            }
            if (txtHandlePerson.Text != null)
            {
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            }
            if (txtSupply.Text != "" && txtSupply.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("BearerOrgName", txtSupply.Text));
            }
            if (comConstractType.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Eq("ContractType", EnumUtil<SubContractType>.FromDescription(comConstractType.SelectedItem)));
            }

            try
            {
                list = model.ContractExcuteSrv.GetContractExcute(oq);
                ShowMasterList(list);
                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                //MessageBox.Show("查询完毕！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0)
            {
                this.lblRecordTotal.Text = "共【" + masterList.Count + "】条记录";
                return;
            }
            decimal sumMoney = 0;
            decimal ContMoney = 0;
            foreach (SubContractProject master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                DataGridViewRow currRow = dgMaster.Rows[rowIndex];
                //dgMaster.CurrentRow.Tag = ClientUtil.ToString(master.Id);
                object state = master.DocState;
                dgMaster[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                dgMaster[colCode.Name, rowIndex].Value = master.Code;
                dgMaster[colBear.Name, rowIndex].Value = master.BearerOrgName;
                dgMaster[colBear.Name, rowIndex].Tag = master.BearerOrg;
                dgMaster[colMainSumMoney.Name, rowIndex].Value = master.ContractSumMoney;
                if (master.TheContractGroup != null)
                {
                    dgMaster[colContractName.Name, rowIndex].Value = master.TheContractGroup.ContractName;
                }
                decimal money = 0;
                foreach (SubContractChangeItem item in master.Details)
                {
                    money += item.ChangeMoney;
                }
                dgMaster[colTempMoney.Name, rowIndex].Value = ClientUtil.ToString(master.ContractInterimMoney);//合同暂定额
                dgMaster[colTotalSettleMoney.Name, rowIndex].Value = ClientUtil.ToString(master.AddupBalanceMoney);//累计结算额
                if (master.ContractInterimMoney != 0)
                {
                    dgMaster[colSettleRace.Name, rowIndex].Value = (master.AddupBalanceMoney / master.ContractInterimMoney * 100).ToString("0.00");//结算金额占比
                }
                dgMaster[colSHDRace.Name, rowIndex].Value = (master.UtilitiesRate * 100).ToString("0.00");//代缴水电费费率
                dgMaster[colSHDWay.Name, rowIndex].Value = ClientUtil.ToString(master.UtilitiesRemMethod);//代缴水电费计取方式
                dgMaster[colContractRace.Name, rowIndex].Value = (master.ManagementRate * 100).ToString("0.00");//代缴建管费费率
                dgMaster[colLWRace.Name, rowIndex].Value = (master.LaobrRace * 100).ToString("0.00");//分包劳务税金费率
                dgMaster[colCHJRace.Name, rowIndex].Value = (master.AllowExceedPercent * 100).ToString("0.00");//允许超结比率
                if (master.TheContractGroup != null)
                {
                    dgMaster[colSettleWay.Name, rowIndex].Value = ClientUtil.ToString(master.TheContractGroup.SettleType);//结算方式
                    dgMaster[colBearRange.Name, rowIndex].Value = ClientUtil.ToString(master.TheContractGroup.BearRange);//承担范围
                    if (master.TheContractGroup.SingDate > ClientUtil.ToDateTime("1900-1-1"))
                    {
                        dgMaster[colSignDate.Name, rowIndex].Value = ClientUtil.ToString(master.TheContractGroup.SingDate);//签订时间
                    }
                }
                dgMaster[colContractWay.Name, rowIndex].Value = ClientUtil.ToString(master.ManagementRemMethod);//建设管理费计取方式
                dgMaster[colConType.Name, rowIndex].Value = ClientUtil.ToString(master.ContractType);
                dgMaster[colCreatDate.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();//制单时间
                dgMaster[colHandlePerson.Name, rowIndex].Value = master.CreatePersonName;
                dgMaster[colHandlePerson.Name, rowIndex].Tag = master.CreatePerson;
                sumMoney += master.AddupBalanceMoney;//累计已结算金额
                ContMoney += master.ContractInterimMoney;//合同暂定金额
            }
            dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster.Rows[0].Cells[0];
                dgMaster_CellClick(dgMaster, new DataGridViewCellEventArgs(0, 0));
            }
            this.lblRecordTotal.Text = "共【" + masterList.Count + "】条记录";
            this.txtSumMoney.Text = sumMoney.ToString();
            if (ContMoney != 0)
            {
                this.txtRace.Text = (sumMoney/ContMoney * 100).ToString("0.00");
            }
        }

        void dgMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null) return;
            SubContractProject master = dr.Tag as SubContractProject;
            if (master == null) return;
            FillDgDetail(master);
        }

        private void FillDgDetail(SubContractProject master)
        {
            dgDetail.Rows.Clear();
            foreach (SubContractChangeItem dtl in master.Details)
            {
                int i = this.dgDetail.Rows.Add();
                DataGridViewRow row = dgDetail.Rows[i];
                dgDetail[colConstractName.Name, i].Value = dtl.ContractName;
                dgDetail[colConstractType.Name, i].Value = dtl.ContractType;
                dgDetail[colConstractChangeMoney.Name, i].Value = dtl.ChangeMoney;
                dgDetail[colConstractChangeDescript.Name, i].Value = dtl.ChangeDesc;
                row.Tag = dtl;
            }
        }
    }
}
