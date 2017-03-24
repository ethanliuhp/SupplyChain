using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VShowPenaltyDeduction : TMasterDetailView
    {
        //罚扣款
        private PenaltyDeductionMaster master;
        public PenaltyDeductionMaster Master
        {
            get { return master; }
            set { master = value; }
        }

        public VShowPenaltyDeduction()
        {
            InitializeComponent();
            this.Load += new EventHandler(VShowPenaltyDeduction_Load);
        }
        //显示罚扣款单数据
        void VShowPenaltyDeduction_Load(object sender, EventArgs e)
        {
            if (Master != null)
            {
                this.txtCode.Text = Master.Code;
                txtHandlePerson.Text = Master.HandlePersonName;
                txtRemark.Text = Master.Descript;
                txtCreateDate.Text = Master.CreateDate.ToShortDateString();
                txtCreatePerson.Text = Master.CreatePersonName;
                this.txtCreatePerson.Text = Master.CreatePersonName;
                this.txtCreateDate.Text = Master.CreateDate.ToShortDateString();
                txtSumQuantity.Text = Master.SumQuantity.ToString("#,###.####");
                txtSumMoney.Text = Master.SumMoney.ToString("#,###.####");
                txtProject.Text = Master.ProjectName.ToString();
                txtProject.Text = Master.ProjectName;
                this.dgDetail.Rows.Clear();
                this.txtPenaltyRank.Text = Master.PenaltyDeductionRantName;
                this.txtPenaltyReason.Text = Master.PenaltyDeductionReason;
                foreach (PenaltyDeductionDetail var in Master.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colAccountMoney.Name, i].Value = var.AccountMoney;
                    this.dgDetail[colAccountPrice.Name, i].Value = var.AccountPrice;
                    this.dgDetail[colAccountQuantity.Name, i].Value = var.AccountQuantity;
                    string a = var.BusinessDate.ToString();
                    string[] aArray = a.Split(' ');
                    string strz = aArray[0];
                    this.dgDetail[colBusinessDate.Name, i].Value = strz;
                    this.dgDetail[colMoneyUnit.Name, i].Value = var.MoneyUnitName;
                    this.dgDetail[colPenaltyMoney.Name, i].Value = var.PenaltyMoney;
                    this.dgDetail[colPenaltyQuantity.Name, i].Value = var.PenaltyQuantity;
                    this.dgDetail[colPenaltySubject.Name, i].Value = var.PenaltySubject;
                    this.dgDetail[colProductUnit.Name, i].Value = var.ProductUnitName;
                    this.dgDetail[colProjectDetail.Name, i].Value = var.TaskDetailName;
                    this.dgDetail[colProjectType.Name, i].Value = var.ProjectTaskName;
                }
            }
        }
    }
}
