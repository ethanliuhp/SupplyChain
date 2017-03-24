using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using IRPServiceModel.Services.Common;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VFineAccountReport : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string detailExptr = "罚款台帐";
        string flexName = "罚款台帐.flx";
        CurrentProjectInfo projectInfo;

        public VFineAccountReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDateEnd.Value = DateTime.Now;
            this.fGridDetail.Rows = 1;
            LoadTempleteFile(flexName);
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtPenaltyRank.Text = engineerMaster.BearerOrgName;
            txtPenaltyRank.Tag = engineerMaster;
        }


        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == flexName)
                {
                    fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                }
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(flexName);
            LoadDetailFile();
        }

        private void LoadDetailFile()
        {
            FlashScreen.Show("正在生成[" + detailExptr + "]报告...");
            try
            {
                var startDate = dtpDateBegin.Value.Date;
                var endDate = dtpDateEnd.Value.Date;
                var condition = string.Format(" and businessdate between to_date('{0} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date('{1} 23:59:59','yyyy-mm-dd hh24:mi:ss') and projectid='{2}'", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), projectInfo.Id);
                if (!string.IsNullOrEmpty(txtPenaltyRank.Text))
                {
                    condition += " and penaltydeductionrant='" + ((SubContractProject)txtPenaltyRank.Tag).Id + "'";
                }
                string sql = "select t1.id,t2.penaltydeductionrantname as name,t1.cause reason,t3.balancetotalprice money,t1.businessdate,createpersonname personname,t2.descript from THD_PENALTYDEDUCTIONDETAIL t1,THD_PENALTYDEDUCTIONMASTER t2,thd_subcontractbalancedetail t3 where t1.parentid=t2.id and t1.balancedtlguid=t3.id and t1.balancedtlguid is not null and t1.labordetailguid is null" + condition + " order by businessdate desc";
                var dt = service.GetData(sql).Tables[0];
                fGridDetail.AutoRedraw = false;
                fGridDetail.InsertRow(3, dt.Rows.Count - 1);
                string sTitle = string.Format("{0}至{1} {2}项目结算罚款台帐", startDate.Date.ToString("yyyy-MM-dd"), endDate.Date.ToString("yyyy-MM-dd"),projectInfo.Name);
                fGridDetail.Cell(1, 1).Text = sTitle;
                int curRow = 0;
                decimal sumMoney = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    curRow = 3 + i;
                    fGridDetail.Cell(curRow, 1).Text = (i + 1).ToString();
                    fGridDetail.Cell(curRow, 2).Text = dr["name"] + "";
                    fGridDetail.Cell(curRow, 2).WrapText = true;
                    fGridDetail.Cell(curRow, 3).Text = dr["reason"] + "";
                    fGridDetail.Cell(curRow, 3).WrapText = true;
                    var money = Math.Abs(Convert.ToDecimal(dr["money"]));
                    fGridDetail.Cell(curRow, 4).Text = money.ToString();
                    sumMoney += money;
                    fGridDetail.Cell(curRow, 5).Text = Convert.ToDateTime(dr["businessdate"]).ToShortDateString();
                    fGridDetail.Cell(curRow, 6).Text = dr["personname"] + "";
                    fGridDetail.Cell(curRow, 7).Text = dr["descript"] + "";
                    fGridDetail.Cell(curRow, 7).WrapText = true;
                    fGridDetail.Row(curRow).AutoFit();
                }
                fGridDetail.Cell(curRow + 1, 4).Text = sumMoney.ToString();
                //for (int tt = 0; tt < fGridDetail.Cols; tt++)
                //{
                //    fGridDetail.Column(tt).AutoFit();
                //}
            }
            catch (Exception e1)
            {
                throw new Exception("生成[" + detailExptr + "]报告异常[" + e1.Message + "]");
            }
            finally
            {
                fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;

                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                FlashScreen.Close();
            }

        }

    }
}