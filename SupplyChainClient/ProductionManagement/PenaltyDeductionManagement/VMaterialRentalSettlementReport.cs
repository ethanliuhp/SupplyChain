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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Linq;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VMaterialRentalSettlementReport : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string detailExptr = "机械租赁结算台帐";
        string flexName = "机械租赁结算台帐.flx";
        CurrentProjectInfo projectInfo;

        public VMaterialRentalSettlementReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            this.fGridDetail.Rows = 1;
            for (int i = 2007; i <= DateTime.Now.Year; i++)
            {
                cbYear.Items.Add(i);
            }
            cbYear.Text = DateTime.Now.Year.ToString();
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-" + CommonUtil.SupplierCatCode4 + "-" + CommonUtil.SupplierCatCode2;
            LoadTempleteFile(flexName);
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
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
                fGridDetail.AutoRedraw = false;
                //// 首先取出统计的时间段
                TimeSlice ts = new TimeSlice(Convert.ToInt32(cbYear.Text));

                var startTime = ts.Slice[0].Value[0];
                var endTime = ts.Slice[11].Value[1];

                // 其次查询出所有的统计数据
                var condition = string.Format(" and submitdate<=to_date('{0}','yyyy-mm-dd')", endTime.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(txtSupply.Text))
                {
                    condition += " and supplierrelation='" + (this.txtSupply.Result[0] as SupplierRelationInfo).Id + "'";
                }
                string sql = "select suppliername name, supplierrelation id,summoney money,submitdate time from thd_materialrentelsetmaster where state=5 and projectid='" + projectInfo.Id + "'" + condition + " order by createdate desc";
                var dt = service.GetData(sql).Tables[0];

                if (dt == null && dt.Rows.Count == 0) return;
                var result = dt.Select().Select(a => new { CreateTime = Convert.ToDateTime(a["time"]), Name = a["name"].ToString(), Money = Convert.ToDecimal(a["money"]), Id = a["id"].ToString() });
                var resultGroup = result.GroupBy(a => a.Id);                // 根据单位id分组
                fGridDetail.InsertRow(6, resultGroup.Count() - 1);          // 插入用于存储所有单位的行数

                int num = 0;
                foreach (var item in resultGroup)
                {
                    var departmentResult = result.Where(a => a.Id == item.Key).ToList();
                    var tempResult = departmentResult.Where(a => a.CreateTime >= startTime && a.CreateTime <= endTime).ToList();
                    decimal total = departmentResult.Where(a => a.CreateTime < startTime).Sum(a => a.Money);
                    fGridDetail.Cell(5 + num, 1).Text = (num + 1).ToString();
                    fGridDetail.Cell(5 + num, 2).Text = departmentResult[0].Name;
                    foreach (var time in ts.Slice)
                    {
                        if (time.Value[0] > DateTime.Now) break;
                        var summary = tempResult.Where(a => a.CreateTime >= time.Value[0] && a.CreateTime <= time.Value[1]).Sum(a => a.Money);
                        total += summary;
                        fGridDetail.Cell(5 + num, time.Key * 2 + 1).Text = summary.ToString();
                        fGridDetail.Cell(5 + num, time.Key * 2 + 2).Text = total.ToString();
                    }
                    num++;
                }

                string sTitle = string.Format("{0}年 {1}项目机械租赁结算台帐", cbYear.Text, projectInfo.Name);
                fGridDetail.Cell(1, 1).Text = sTitle;
                fGridDetail.Cell(2, 3).Text = cbYear.Text + "年每月结算金额（元）";

                for (int tt = 0; tt < fGridDetail.Cols; tt++)
                {
                    fGridDetail.Column(tt).AutoFit();
                }
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