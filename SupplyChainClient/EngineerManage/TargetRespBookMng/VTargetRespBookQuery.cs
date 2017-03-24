using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.EngineerManage.Client.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    public partial class VTargetRespBookQuery : TBasicDataView
    {
        private MTargetRespBookMng model = new MTargetRespBookMng();
        MStockMng mstock= new MStockMng();
        public List<CurrentProjectInfo> ListExtendProject = new List<CurrentProjectInfo>();

        public VTargetRespBookQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            this.cbProjectSeale.Items.AddRange(new object[] { "特大", "大型", "中型", "小型", "特小" });
            this.cbSignedWhether.Items.AddRange(new object[] { "未签订", "已签订" });
            this.cbRiskPayed.Items.AddRange(new object[] { "未缴纳", "全部缴纳", "部分缴纳" });
            this.cbCreate.Items.AddRange(new object[] { "已创建", "未创建" });
        }

        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click +=new EventHandler(btnExcel_Click);
            //dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        //void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex > -1 && e.ColumnIndex > -1)
        //    {
        //        string billId = dgDetail.Rows[e.RowIndex].Tag as string;
        //        if (ClientUtil.ToString(billId) != "")
        //        {
        //            VTargetRespBook vPlan = new VTargetRespBook();
        //            vPlan.Start(billId);
        //            vPlan.ShowDialog();
        //        }
        //    }
        //}

        void btnSearch_Click(object sender, EventArgs e)
        {
            Hashtable table = new Hashtable();
            ObjectQuery oq = new ObjectQuery();
            if (txtProjectName.Text != "")
            {
                oq.AddCriterion(Expression.Like("ProjectName", txtProjectName.Text, MatchMode.Anywhere));
            }
            if (txtDocumentName.Text != "")
            {
                oq.AddCriterion(Expression.Like("DocumentName", txtDocumentName.Text, MatchMode.Anywhere));
            }
            if (txtHandlePerson.Text != "")
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Text));
            }
            if (txtProjectManager.Text != "")
            {
                oq.AddCriterion(Expression.Eq("ProjectManagerName", txtProjectManager.Text));
            }
            if (cbSignedWhether.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Like("SignedWhether", cbSignedWhether.SelectedItem));
            }
            if (cbRiskPayed.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Like("RiskPaymentState", cbRiskPayed.SelectedItem));
            }
            if (cbProjectSeale.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Like("ProjectScale", cbProjectSeale.SelectedItem));
            }
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpCreateDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpCreateDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Ge("RealOperationDate", this.dtpSignDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("RealOperationDate", this.dtpSignDateEnd.Value.AddDays(1).Date));
            //oq.AddCriterion(Expression.Not(Expression.Eq("DocState", DocumentState.Edit)));
            IList list = model.TargetRespBookSrc.GetTargetRespBook(oq);

            if (list == null || list.Count == 0) return;
            foreach (TargetRespBook book in list)
            {
                table.Add(book,book.ProjectName);
            }

            string condition = "";
            DataSet dataSet = model.TargetRespBookSrc.GetProjectInfo(condition);
            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                string strProjectName = dataRow["ProjectName"].ToString();
                
                bool flag = false;
                if (cbCreate.SelectedItem == null)
                {
                    foreach (System.Collections.DictionaryEntry objAccount in table)
                    {
                        if (objAccount.Value.ToString().Equals(strProjectName))
                        {
                            int rowIndex = this.dgDetail.Rows.Add();
                            dgDetail[colNO.Name, rowIndex].Value = rowIndex + 1;
                            dgDetail[colProjectName.Name, rowIndex].Value = dataRow["ProjectName"];
                            TargetRespBook tbook = objAccount.Key as TargetRespBook;
                            dgDetail[colDocumentName.Name, rowIndex].Value = tbook.DocumentName;
                            dgDetail[colProjectManager.Name, rowIndex].Value = tbook.ProjectManagerName; //项目经理
                            dgDetail[colProjectScale.Name, rowIndex].Value = tbook.ProjectScale; //项目规模
                            dgDetail[colSignedWether.Name, rowIndex].Value = tbook.SignedWhether; //签订状况
                            dgDetail[colRealOperationDate.Name, rowIndex].Value = tbook.RealOperationDate.ToShortDateString();//制单时间
                            dgDetail[colCreateDate.Name, rowIndex].Value = tbook.CreateDate.ToShortDateString();//业务时间
                            //风险化解目标
                            decimal risk = ClientUtil.ToDecimal(tbook.RiskDissolvesTarget);
                            risk = risk / 10000;
                            dgDetail[colRiskDissolvesTarget.Name, rowIndex].Value = risk.ToString();
                            //责任上缴目标
                            decimal resp = ClientUtil.ToDecimal(tbook.ResponsibilityTurnedTarget);
                            resp = resp / 10000;
                            dgDetail[colResponsibilityTurnedTarget.Name, rowIndex].Value = resp.ToString();
                            //风险抵押金缴纳情况
                            dgDetail[colRiskPaymentState.Name, rowIndex].Value = tbook.RiskPaymentState;
                            //成本控制目标
                            decimal cost = ClientUtil.ToDecimal(tbook.CostcontrolTarget);
                            cost = cost / 10000;
                            dgDetail[colCostcontrolTarget.Name, rowIndex].Value = cost.ToString();
                            dgDetail[colHandlePerson.Name, rowIndex].Value = tbook.HandlePerson;//责任人
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail[colNO.Name, rowIndex].Value = rowIndex + 1;
                        dgDetail[colProjectName.Name, rowIndex].Value = dataRow["ProjectName"];
                        dgDetail[colDocumentName.Name, rowIndex].Value = "未创建";
                    }
                    this.panel1.Enabled = true;
                }
                if (cbCreate.SelectedItem == "已创建")
                {
                    foreach (System.Collections.DictionaryEntry objAccount in table)
                    {
                        if (objAccount.Value.ToString().Equals(strProjectName))
                        {
                            int rowIndex = this.dgDetail.Rows.Add();
                            dgDetail[colNO.Name, rowIndex].Value = rowIndex + 1;
                            dgDetail[colProjectName.Name, rowIndex].Value = dataRow["ProjectName"];
                            TargetRespBook tbook = objAccount.Key as TargetRespBook;
                            dgDetail[colDocumentName.Name, rowIndex].Value = tbook.DocumentName;
                            dgDetail[colProjectManager.Name, rowIndex].Value = tbook.ProjectManagerName; //项目经理
                            dgDetail[colProjectScale.Name, rowIndex].Value = tbook.ProjectScale; //项目规模
                            dgDetail[colSignedWether.Name, rowIndex].Value = tbook.SignedWhether; //签订状况
                            dgDetail[colRealOperationDate.Name, rowIndex].Value = tbook.RealOperationDate.ToShortDateString();//制单时间
                            dgDetail[colCreateDate.Name, rowIndex].Value = tbook.CreateDate.ToShortDateString();//创建时间
                            //风险化解目标
                            decimal risk = ClientUtil.ToDecimal(tbook.RiskDissolvesTarget);
                            risk = risk / 10000;
                            dgDetail[colRiskDissolvesTarget.Name, rowIndex].Value = risk.ToString();
                            //责任上缴目标
                            decimal resp = ClientUtil.ToDecimal(tbook.ResponsibilityTurnedTarget);
                            resp = resp / 10000;
                            dgDetail[colResponsibilityTurnedTarget.Name, rowIndex].Value = resp.ToString();
                            //风险抵押金缴纳情况
                            dgDetail[colRiskPaymentState.Name, rowIndex].Value = tbook.RiskPaymentState;
                            //成本控制目标
                            decimal cost = ClientUtil.ToDecimal(tbook.CostcontrolTarget);
                            cost = cost / 10000;
                            dgDetail[colCostcontrolTarget.Name, rowIndex].Value = cost.ToString();
                            dgDetail[colHandlePerson.Name, rowIndex].Value = tbook.HandlePerson;//责任人
                        }
                    }
                    this.panel1.Enabled = true;
                }

                if (cbCreate.SelectedItem == "未创建")
                {
                    foreach (System.Collections.DictionaryEntry objAccount in table)
                    {
                        if (objAccount.Value.ToString().Equals(strProjectName))
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail[colNO.Name, rowIndex].Value = rowIndex + 1;
                        dgDetail[colProjectName.Name, rowIndex].Value = dataRow["ProjectName"];
                        dgDetail[colDocumentName.Name, rowIndex].Value = "未创建";
                    }
                    this.panel1.Enabled = false;
                }
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
    }
}
