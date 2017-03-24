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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng
{
    public partial class VCompanyDemandMasterPlanQuery : TBasicDataView
    {
        private MDemandMasterPlanMng model = new MDemandMasterPlanMng();

        public VCompanyDemandMasterPlanQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            InitProject();
        }

        private void InitProject()
        {
            this.listBoxType.HorizontalScrollbar = true;
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddOrder(Order.Asc("Name"));
            IList list = model.DemandPlanSrv.GetDomainByCondition(typeof(CurrentProjectInfo), objectQuery);
            //foreach (CurrentProjectInfo obj in list)
            //{
            //    listBoxType.Items.Add(obj);
            //}
            try
            {
                listBoxType.DataSource = list;
                listBoxType.DisplayMember = "Name";
                listBoxType.SelectedItems.Clear();
            }
            catch (Exception e)
            {
                string a = "";
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            //dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            //dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //condition += "and t1.ProjectId = '" + projectInfo.Id + "'  and t1.plantype=1 ";

            //项目
            if (listBoxType.SelectedItems != null && listBoxType.SelectedItems.Count > 0)
            {
                condition += " and (";                
                for (int i = 0; i < listBoxType.SelectedItems.Count; i++)
                {
                    CurrentProjectInfo pi = listBoxType.SelectedItems[i] as CurrentProjectInfo;
                    if (i == 0)
                    {
                        condition += "t1.ProjectId='" + pi.Id + "'";
                    }
                    else
                    {
                        condition += " or t1.ProjectId='" + pi.Id + "'";
                    }                    
                }
                condition += ")";
            }

            //制单日期
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            //物资
            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }
            //规格型号
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
            }

            //物资分类
            if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            {
                MaterialCategory mc = txtMaterialCategory.Result[0] as MaterialCategory;
                condition += " and t2.materialcode like '"+mc.Code+"%'";
            }
            #endregion
            DataSet dataSet = model.DemandPlanSrv.DemandMstPlanQuery4Company(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                string projectName = dataRow["projectname"]+"";
                if(string.IsNullOrEmpty(projectName))
                {
                    projectName = "小计";
                }                

                string materialcode=dataRow["materialcode"]+"";

                if (projectName == "小计" && string.IsNullOrEmpty(materialcode))
                {
                    projectName = "总计";
                }
                dgDetail[colProjectName.Name, rowIndex].Value = projectName;
                dgDetail[colMaterialCode.Name, rowIndex].Value = materialcode;//物资编码
                dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["materialname"]+"";//物资名称
                dgDetail[colSpec.Name, rowIndex].Value = dataRow["materialspec"]+""; //规格型号
                dgDetail[colUnit.Name, rowIndex].Value = dataRow["matstandardunitname"]+"";//计量单位

                decimal quantity = ClientUtil.ToDecimal(dataRow["Quantity"]);
                decimal money = ClientUtil.ToDecimal(dataRow["money"]);
                decimal price = 0M;
                if (quantity != 0)
                {
                    price = money / quantity;
                }

                dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                dgDetail[colPrice.Name, rowIndex].Value = price.ToString("################0.##");//单价
                dgDetail[colSumMoney.Name, rowIndex].Value = money;                
            }

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
