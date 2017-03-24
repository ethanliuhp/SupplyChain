using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VWorkerDetails : TBasicDataView
    {
        public VWorkerDetails()
        {
            InitializeComponent();
        }
        public VWorkerDetails(IList workerDetails)
        {
            InitializeComponent();
            ShowWorkDeails(workerDetails);
        }

        void ShowWorkDeails(IList workerDetails)
        {
            foreach (LaborDemandWorkerType worker in workerDetails)
            {
                int rowIndex = dgWorkerDeails.Rows.Add();
                dgWorkerDeails[colWorkerType.Name, rowIndex].Value = worker.WorkerType;
                dgWorkerDeails[colPeopleNum.Name, rowIndex].Value = worker.PeopleNum;
                dgWorkerDeails.Rows[rowIndex].Tag = worker;
            }
        }
    }
}
