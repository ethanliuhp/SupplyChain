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
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VContractLedgerReport : TBasicDataView
    {
        string reportName = "签证变更统计";
        MProjectDepartment model = new MProjectDepartment();
        MCostMonthAccount modelCost = new MCostMonthAccount();
        CurrentProjectInfo ProjectInfo = new CurrentProjectInfo();
        public VContractLedgerReport()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            listBoxType.SelectedIndexChanged += new EventHandler(listBoxType_SelectedIndexChanged);
        }

        private void InitData()
        {
            this.listBoxType.HorizontalScrollbar = true;
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddOrder(Order.Asc("Name"));
            IList list = model.CurrentSrv.GetDomainByCondition(typeof(CurrentProjectInfo),objectQuery);
            foreach (CurrentProjectInfo obj in list)
            {
                listBoxType.Items.Add(obj);
            }
            try
            {
                listBoxType.DataSource = list;
                listBoxType.DisplayMember = "Name";
            }
            catch (Exception e)
            {
                string a = "";
            }
            if (listBoxType.Items.Count > 0)
            {
                listBoxType.SelectedIndex = 0;
            }
            this.fGrid_.Rows = 1;
        }
        void listBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = fGrid_.ExportToExcel(reportName, false, false, true);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ProjectInfo = listBoxType.SelectedItem as CurrentProjectInfo;
            DateTime dtStart = dtpDateBegin.Value.Date;
            DateTime dtEnd = dtpDateEnd.Value.AddDays(1).Date;
            LoadTempleteFile(reportName + ".flx");
            fGrid_.Cell(2, 2).Text = ProjectInfo.Name;
            fGrid_.Cell(2, 13).Text = ConstObject.LoginDate.ToShortDateString();

            CommonUtil.SetFlexGridFace(this.fGrid_);
            LoadReportData(ProjectInfo,dtStart,dtEnd);

        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                this.fGrid_.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }
        //向FGrid_添加数据
        private void LoadReportData(CurrentProjectInfo ProjectInfo, DateTime dtStartDate, DateTime dtEndDate)
        {
            int detailStartRowNumber = 5;//5为模板中的行号
            IList detaillist = modelCost.CostMonthAccSrv.SearchGWBSDetailLedger(ProjectInfo, dtStartDate, dtEndDate);
            fGrid_.InsertRow(detailStartRowNumber, detaillist.Count);
            foreach (GWBSDetailLedger detailBill in detaillist)
            {
                fGrid_.Cell(detailStartRowNumber, 2).Text = ClientUtil.ToString(detailBill.Temp3);
                fGrid_.Cell(detailStartRowNumber, 3).Text = ClientUtil.ToString(detailBill.Temp2);
                fGrid_.Cell(detailStartRowNumber, 4).Text = ClientUtil.ToString(detailBill.ContractWorkAmount);
                fGrid_.Cell(detailStartRowNumber, 5).Text = ClientUtil.ToString(detailBill.WorkAmountUnitName);
                fGrid_.Cell(detailStartRowNumber, 6).Text = ClientUtil.ToString(detailBill.ContractPrice);
                fGrid_.Cell(detailStartRowNumber, 7).Text = ClientUtil.ToString(detailBill.ContractTotalPrice);
                fGrid_.Cell(detailStartRowNumber, 8).Text = ClientUtil.ToString(detailBill.ResponsibleWorkAmount);
                fGrid_.Cell(detailStartRowNumber, 9).Text = ClientUtil.ToString(detailBill.WorkAmountUnitName);
                fGrid_.Cell(detailStartRowNumber, 10).Text = ClientUtil.ToString(detailBill.ResponsiblePrice);
                fGrid_.Cell(detailStartRowNumber, 11).Text = ClientUtil.ToString(detailBill.ResponsibleTotalPrice);
                fGrid_.Cell(detailStartRowNumber, 12).Text = ClientUtil.ToString(detailBill.PlanWorkAmount);
                fGrid_.Cell(detailStartRowNumber, 13).Text = ClientUtil.ToString(detailBill.WorkAmountUnitName);
                fGrid_.Cell(detailStartRowNumber, 14).Text = ClientUtil.ToString(detailBill.PlanPrice);
                fGrid_.Cell(detailStartRowNumber, 15).Text = ClientUtil.ToString(detailBill.PlanTotalPrice);
                fGrid_.Cell(detailStartRowNumber, 16).Text = ClientUtil.ToString(detailBill.Temp5);
                fGrid_.Cell(detailStartRowNumber, 17).Text = ClientUtil.ToString(detailBill.Temp4);
                //fGrid_.Cell(detailStartRowNumber, 15).Text = ClientUtil.ToString(sumxy);
                //if (detailBill.ContractTotalPrice != 0)
                //{
                //    fGrid_.Cell(detailStartRowNumber, 16).Text = ClientUtil.ToString(sumxy / detailBill.ContractTotalPrice);
                //}
                //else
                //{
                //    fGrid_.Cell(detailStartRowNumber, 16).Text = "0";
                //}
                detailStartRowNumber += 1;
            }
        }
    }
}
