using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VSubcontractsLedger : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        private CurrentProjectInfo projectInfo;


        public VSubcontractsLedger()
        {
            InitializeComponent();
            InitData();
            InitEvents();
        }

        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddMonths(-1);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.fGridDetail.Rows = 1;
            this.btnExcel.Visible = true;
            this.contractType.Items.AddRange(new object[] { "补充协议", "劳务分包合同", "专业分包合同", "分包合同交底" });
            VisualOperationOrg();
        }

        private void InitEvents()
        {
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
        }

        private void VisualOperationOrg()
        {
            bool flag = false;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                flag = true;
            }
            else
            {
                txtOperationOrg.Tag = projectInfo;
                txtOperationOrg.Text = projectInfo.Name;
                flag = false;
            }
            this.btnOperationOrg.Visible = flag;
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string flxname = null;
                if (ClientUtil.ToString(contractType.SelectedItem) == "补充协议")
                {
                    flxname = "自签协议台账.flx";
                }
                else if (ClientUtil.ToString(contractType.SelectedItem) == "分包合同交底")
                {
                    flxname = "分包合同交底.flx";
                }
                else {
                    flxname = "分包合同台账.flx";
                }
                Boolean bl =  this.LoadFLXFile(flxname);
                if (bl)
                {
                    this.LoadData();
                }
                else {
                    MessageBox.Show("未找到模板格式文件--" + flxname);
                }
                    
            //设置外观
            fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            }
            catch (Exception e1)
            {
                MessageBox.Show("生成[台账]报告异常[" + ExceptionUtil.ExceptionMessage(e1) + "]");
                
            }
            finally
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel("台账", false, false, true);
        }

        private void LoadData()
        {
            DateTime startDate = dtpDateBegin.Value.Date;
            DateTime endDate = dtpDateEnd.Value.Date;
            string condition = null;
            string sOrgSysCode = null;
            string sProjectId = null;
            int type;
            if (ClientUtil.ToString(contractType.SelectedItem) == "分包合同交底")
            {
                type = 1;
            }
            else if (ClientUtil.ToString(contractType.SelectedItem) == "补充协议")
            {
                type = 3;
                condition += " and t3.contracttype='分包合同补充协议'";
            }
            else{
                type = 2;
                //if (ClientUtil.ToString(contractType.SelectedItem) == "补充协议")
                //{
                //    //condition += " and t2.CONTRACTTYPE = 6";
                //    condition += "and t1.contractgrouptype='分包合同补充协议'";
                //}
                if (ClientUtil.ToString(contractType.SelectedItem) == "劳务分包合同")
                {
                    condition += " and t2.CONTRACTTYPE = 0";
                }
                if (ClientUtil.ToString(contractType.SelectedItem) == "专业分包合同")
                {
                    condition += " and t2.CONTRACTTYPE = 1";
                }
               
            }
            if (!ClientUtil.isEmpty(startDate) && !ClientUtil.isEmpty(endDate))
            {
                if (type == 1)
                {
                    condition += " and t1.CREATEDATE>=to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and t1.CREATEDATE<=to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')";
                }
                else
                {
                    condition += " and t2.CREATETIME>=to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') and t2.CREATETIME<=to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')";

                }
            }
            
            if (this.btnOperationOrg.Visible)
            {
                if (this.txtOperationOrg.Tag == null)
                {
                    MessageBox.Show("请选择查询的分支机构/项目");
                    return;
                }
                if (ClientUtil.isEmpty(contractType.SelectedItem))
                {
                    MessageBox.Show("请选择合同类型");
                    return;
                }
                OperationOrgInfo OrgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
                sProjectId = model.IndirectCostSvr.GetProjectIDByOperationOrg(OrgInfo.Id);
                if (string.IsNullOrEmpty(sProjectId))
                {
                    sOrgSysCode = OrgInfo.SysCode;
                    if (type == 2)
                    {
                        condition += " and t1.CREATEPERSONSYSCODE like '%" + sOrgSysCode + "%'";
                    }
                    else
                    {
                        condition += " and t1.OPGSYSCODE like '%" + sOrgSysCode + "%'";
                    }
                }
                else
                {
                    condition += " and t1.PROJECTID = '" + sProjectId + "'";
                }
            }
            else 
            {
                sProjectId = projectInfo.Id;
                condition += " and t1.PROJECTID = '" + sProjectId + "'";
            }


            if (type == 2)
                condition += " and nvl(t1.state,0)<>5 ";

            IList list = model.CostMonthAccSrv.QuerySubcontractsLedger(condition, type);
            if (!ClientUtil.isEmpty(list))
            {
                LoadDataToGrid(list);
            }
            else 
            {
                MessageBox.Show("台账数据为空!");
            } 
        }

        private void LoadDataToGrid(IList list)
        {
            int iCount = list.Count;
            int iStart = 3;
            fGridDetail.AutoRedraw = false;
            fGridDetail.InsertRow(iStart, iCount);
            FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);
            CommonUtil.SetFlexGridDetailCenter(range);
            iCount = 0;
            int index = 0;
            decimal sum = 0;
            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (!ClientUtil.isEmpty(info))
            {
                this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() + "到" + dtpDateEnd.Value.Date.ToShortDateString() + " [" + info.Name + "] [" + contractType.SelectedItem + "] 台账";
            }
            else
            {
                this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() + "到" + dtpDateEnd.Value.Date.ToShortDateString() + " [" + projectInfo.Name + "] [" + contractType.SelectedItem + "] 台账";
            }

            if (ClientUtil.ToString(contractType.SelectedItem) == "补充协议")
            {
                foreach (DataDomain dd in list)
                {
                    index = iStart + iCount;
                    fGridDetail.Cell(index, 1).Text = (iCount + 1).ToString();
                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(dd.Name1);
                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(dd.Name2);
                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(dd.Name3);
                    fGridDetail.Cell(index, 5).Text = ClientUtil.ToString(dd.Name9);
                    fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(dd.Name4);
                    fGridDetail.Cell(index, 7).Text = ClientUtil.ToString(dd.Name5);
                    fGridDetail.Cell(index, 8).Text = ClientUtil.ToDateTime(dd.Name6).ToShortDateString();
                    fGridDetail.Cell(index, 10).Text = ClientUtil.ToString(dd.Name8);
                    sum += ClientUtil.ToDecimal(dd.Name4);
                    iCount++;
                }
                fGridDetail.Cell(iStart + iCount, 1).Text = ClientUtil.ToString("合计");
                fGridDetail.Cell(iStart + iCount, 6).Text = ClientUtil.ToDecimal(sum).ToString("N2");
                
            }
            else if (ClientUtil.ToString(contractType.SelectedItem) == "分包合同交底")
            {
                foreach (DataDomain dd in list)
                {
                    index = iStart + iCount;
                    fGridDetail.Cell(index, 1).Text = (iCount + 1).ToString();
                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(dd.Name1);
                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(dd.Name2);
                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(dd.Name3);
                    fGridDetail.Cell(index, 5).Text = ClientUtil.ToDateTime(dd.Name4).ToShortDateString(); ;
                    fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(dd.Name5);
                    iCount++;
                }
            }
            else
            {
                foreach (DataDomain dd in list)
                {
                    index = iStart + iCount;
                    fGridDetail.Cell(index, 1).Text = (iCount + 1).ToString();
                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(dd.Name1);
                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(dd.Name2);
                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(dd.Name3);
                    fGridDetail.Cell(index, 5).Text = ClientUtil.ToString(dd.Name4);
                    fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(dd.Name5);
                    fGridDetail.Cell(index, 7).Text = ClientUtil.ToDateTime(dd.Name6).ToShortDateString();
                    fGridDetail.Cell(index, 9).Text = ClientUtil.ToString(dd.Name8);
                    sum += ClientUtil.ToDecimal(dd.Name4);
                    iCount++;
                }
                fGridDetail.Cell(iStart + iCount, 1).Text = ClientUtil.ToString("合计");
                fGridDetail.Cell(iStart + iCount, 5).Text = ClientUtil.ToDecimal(sum).ToString("N2");
            }
        }

        private Boolean LoadFLXFile(string flxName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(flxName))
            {
                eFile.CreateTempleteFileFromServer(flxName);
                fGridDetail.OpenFile(path + "\\" + flxName);//载入格式
                return true;
            }
            return false;
        }
    }
}
