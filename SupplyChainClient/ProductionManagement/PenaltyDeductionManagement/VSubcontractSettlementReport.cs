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
using System.Linq;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VSubcontractSettlementReport : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string detailExptr = "分包结算台帐";
        string flexName = "分包结算台帐.flx";
        CurrentProjectInfo projectInfo;

        public VSubcontractSettlementReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            this.fGridDetail.Rows = 1;
            LoadTempleteFile(flexName);
            projectInfo = StaticMethod.GetProjectInfo();
             
            for (int i = 2007; i <= DateTime.Now.Year; i++)
            {
                cbYear.Items.Add(i);
            }
            cbYear.Text = DateTime.Now.Year.ToString();
            cbType.SelectedIndex = 0;
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = projectInfo.Name;
                txtOperationOrg.Tag = projectInfo;
                btnOperationOrg.Visible = false;
            }
            else
            {
                btnOperationOrg.Visible = true;
            }
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click += btnOperationOrg_Click;
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            var frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
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
            string sProjectID = string.Empty;
            string sSysCode = string.Empty;
            if (this.btnOperationOrg.Visible)
            {
                if (this.txtOperationOrg.Tag == null)
                {
                    MessageBox.Show("请选择组织机构");
                    return;
                }
                else
                {
                    OperationOrgInfo oOrgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
                    sProjectID = service.GetProjectIDByOperationOrg(oOrgInfo.Id);
                    if (string.IsNullOrEmpty(sProjectID))
                    {
                        sSysCode = oOrgInfo.SysCode;
                    }
                }
            }
            else
            {
                sProjectID = projectInfo.Id;
            }
            FlashScreen.Show("正在生成[" + detailExptr + "]报告...");
            try
            {
                fGridDetail.AutoRedraw = false;
                //// 首先取出统计的时间段
                TimeSlice ts = new TimeSlice(Convert.ToInt32(cbYear.Text));

                var startTime = ts.Slice[0].Value[0];
                var endTime = ts.Slice[11].Value[1];

                // 其次查询出所有的统计数据
                var condition = string.Empty;
                if (!string.IsNullOrEmpty(sProjectID))
                {
                    condition = string.Format(" and t1.createdate<=to_date('{0}','yyyy-mm-dd') and t2.contracttype={1} and t2.projectid='{2}'", endTime.ToString("yyyy-MM-dd"), cbType.SelectedIndex, sProjectID);
                }
                else
                {
                    condition = string.Format(" and t1.createdate<=to_date('{0}','yyyy-mm-dd') and t2.contracttype={1} and t2.ownerorgsyscode like '{2}%'", endTime.ToString("yyyy-MM-dd"), cbType.SelectedIndex, sSysCode);
                }
               
                string sql = "select nvl(t1.createdate,sysdate) time,nvl(t1.subcontractprojectid,'') id,nvl(t1.subcontractunitname,'') name,nvl(t2.SUBPACKAGE,'') content,nvl(t1.balancemoney,0) money,nvl(t2.contractsummoney,0) sum from thd_subcontractbalancebill t1,thd_subcontractproject t2 where t1.state=5 and t1.subcontractprojectid=t2.id " + condition;
                var dt = service.GetData(sql).Tables[0];

                if (dt == null && dt.Rows.Count == 0) return;
                var result = dt.Select().Select(a => new { CreateTime = Convert.ToDateTime(a["time"]), Name = a["name"].ToString(), Money = Convert.ToDecimal(a["money"]), Id = a["id"].ToString(), Sum = Convert.ToDecimal(a["sum"]), Content = a["content"] + "" });
                var resultGroup = result.GroupBy(a => a.Id);                // 根据单位id分组
          
                fGridDetail.InsertRow(6, resultGroup.Count() - 1);          // 插入用于存储所有单位的行数

                int num = 0;
                foreach (var item in resultGroup)
                {
                    // 各单位分包结算记录
                    var departmentResult = result.Where(a => a.Id == item.Key).ToList();
                    // 各单位今年的分包结算记录
                    var tempResult = departmentResult.Where(a => a.CreateTime >= startTime && a.CreateTime <= endTime).ToList();
                    // 今年之前的累计
                    decimal total = departmentResult.Where(a => a.CreateTime < startTime).Sum(a => a.Money);
                    fGridDetail.Cell(5 + num, 1).Text = (num + 1).ToString();
                    var contentAll = new List<string>();
                    fGridDetail.Cell(5 + num, 2).Text = departmentResult[0].Name;
                    fGridDetail.Cell(5 + num, 3).Text = departmentResult[0].Content;
                    fGridDetail.Cell(5 + num, 4).Text = departmentResult[0].Sum.ToString();

                    foreach (var time in ts.Slice)
                    {
                        if (time.Value[0] > DateTime.Now) break;
                        var summary = tempResult.Where(a => a.CreateTime >= time.Value[0] && a.CreateTime <= time.Value[1]).Sum(a => a.Money);
                        total += summary;
                        fGridDetail.Cell(5 + num, time.Key * 2 + 3).Text = summary.ToString();
                        fGridDetail.Cell(5 + num, time.Key * 2 + 4).Text = total.ToString();
                    }
                    num++;
                }

                string sTitle = string.Format("{0}年 {1}项目{2}结算台帐", cbYear.Text,this.txtOperationOrg.Text, cbType.Text);
                fGridDetail.Cell(1, 1).Text = sTitle;
                fGridDetail.Cell(2, 5).Text = cbYear.Text + "年每月结算金额";

                for (int tt = 0; tt < fGridDetail.Cols; tt++)
                {
                    if (tt == 3) continue;
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