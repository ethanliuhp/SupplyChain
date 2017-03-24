using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VConstructNodeQuery : TBasicDataView
    {
        private CurrentProjectInfo projectInfo;
        private MFinanceMultData mOperate = new MFinanceMultData();

        public VConstructNodeQuery()
        {
            InitializeComponent();
            
            InitEvents();

            InitData();
        }

        private void InitData()
        {
            ucProjectSelector1.InitData();
            projectInfo = ucProjectSelector1.SelectedProject;

            LoadConstructNode();
            radWbs.Checked = true;
        }

        private void LoadConstructNode()
        {
            if (projectInfo == null)
            {
                return;
            }

            var list = mOperate.FinanceMultDataSrv.GetConstructNodeByProject(projectInfo.Id);
            if (list != null)
            {
                dgMaster.DataSource = list.OfType<ConstructNode>().ToArray();

                radWbs_CheckedChanged(null, null);
            }
        }

        private void InitEvents()
        {
            dgMaster.BorderStyle = BorderStyle.FixedSingle;
            dgMaster.AutoGenerateColumns = false;

            radWbs.CheckedChanged += radWbs_CheckedChanged;

            btnQuery.Click += btnQuery_Click;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            projectInfo = ucProjectSelector1.SelectedProject;
            if(projectInfo==null)
            {
                MessageBox.Show("请选择项目");
                return;
            }

            LoadConstructNode();
        }

        private void radWbs_CheckedChanged(object sender, EventArgs e)
        {
            var list = dgMaster.DataSource as ConstructNode[];
            if (list == null)
            {
                return;
            }
            if (radWbs.Checked)
            {
                colWbsName.DisplayIndex = 1;
                colPeriodCode.DisplayIndex = 2;
                colPeriodName.DisplayIndex = 3;

                var setList =
                    list.ToList().FindAll(d => d.DatePeriod != null).OrderBy(d => d.WBSTree.Name).ThenBy(d => d.DatePeriod.PeriodCode).ToList();
                setList.AddRange(list.ToList().FindAll(d => d.DatePeriod == null));

                dgMaster.DataSource = setList.ToArray();
            }
            else
            {
                colPeriodCode.DisplayIndex = 1;
                colPeriodName.DisplayIndex = 2;
                colWbsName.DisplayIndex = 3;

                var setList =
                    list.ToList().FindAll(d => d.DatePeriod != null).OrderBy(d => d.DatePeriod.PeriodCode).ThenBy(
                        d => d.WBSTree.Name).ToList();
                setList.AddRange(list.ToList().FindAll(d => d.DatePeriod == null));

                dgMaster.DataSource = setList.ToArray();
            }
        }
    }
}
