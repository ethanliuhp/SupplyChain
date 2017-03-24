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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialBalanceMng.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using System.IO;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VCompanyIndexGenerate : TBasicDataView
    {
        MIndirectCost model = new MIndirectCost();
        IIndirectCostSvr indirectModel = null;
        IProductionManagementSrv proMSrv = null;

        public VCompanyIndexGenerate()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            if (indirectModel == null)
                indirectModel = StaticMethod.GetService("IndirectCostSvr") as IIndirectCostSvr;

            if (proMSrv == null)
                proMSrv = StaticMethod.GetService("ProductionManagementSrv") as IProductionManagementSrv;

            dtpEndDate.Value = ConstObject.TheLogin.LoginDate;
        }

        private void InitEvent()
        {
            this.btnGenerate.Click += new EventHandler(btnGenerate_Click);
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            this.btnDuration.Click += new EventHandler(btnDuration_Click);
            this.btnProState.Click += new EventHandler(btnProState_Click);
        }

        void btnProState_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在进行项目状态值计算...");
            try
            {
                indirectModel.CalulationProjectState();
            }
            catch(Exception ex)
            {
                throw new Exception("项目状态值计算异常[" + ex.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }
            MessageBox.Show("项目状态值计算完成！");
        }

        void btnDuration_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在进行工期延期指标计算...");
            try
            {
                proMSrv.CreateProjectDelayDays("");
            }
            catch (Exception ex)
            {
                throw new Exception("工期延期计算异常[" + ex.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }
            MessageBox.Show("工期延期计算完成！");
            
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            DateTime currDate = ClientUtil.ToDateTime(dtpEndDate.Value.ToShortDateString());
            IList list = model.IndirectCostSvr.QueryCompanyIndexInfoByDate(currDate);
            this.dgMaster.Rows.Clear();
            foreach (DataDomain domain in list)
            {
                int i = this.dgMaster.Rows.Add();
                this.dgMaster[this.colOperDate.Name, i].Value = ClientUtil.ToDateTime(domain.Name1).ToShortDateString();
                this.dgMaster[this.colProjectCount.Name, i].Value = domain.Name2;
                this.dgMaster[this.colCount.Name, i].Value = domain.Name3;
            }
            
        }
        //公司关键指标计算
        void btnGenerate_Click(object sender, EventArgs e)
        {
            
            DateTime currDate = ClientUtil.ToDateTime(dtpEndDate.Value.ToShortDateString());
            IList list = model.IndirectCostSvr.QueryCompanyIndexInfoByDate(currDate);
            if (list.Count > 0)
            {
                DialogResult dr = MessageBox.Show("是否重新计算公司关键指标？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.No) return;
                model.IndirectCostSvr.CompanyKeyInfoService(currDate);
            }
            else
            {
                DialogResult dr = MessageBox.Show("是否计算公司关键指标？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.No) return;
                model.IndirectCostSvr.CompanyKeyInfoService(currDate);
            }

            FlashScreen.Show("正在进行公司关键指标计算...");
            try
            {
                string msgStr = "";
                if (msgStr != "")
                {
                    MessageBox.Show(msgStr);
                    return;
                }
            }
            catch (Exception e1)
            {
                throw new Exception("公司关键指标计算异常[" + e1.Message + "]");
            }
            finally {
                FlashScreen.Close();
            }
            this.btnGenerate.Enabled = true;
            LogData log = new LogData();
            log.BillType = "公司关键指标计算";
            log.Code = "";
            log.OperType = "月结";
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            StaticMethod.InsertLogData(log);
            MessageBox.Show("公司关键指标计算完成！");

        }

    }
}

