using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.EngineerManage.CollectionManage.Domain;
using NHibernate.Criterion;
using System.Collections;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage
{
    public partial class VCollectionQuery : TBasicDataView
    {
        private MCollectionManage model = new MCollectionManage();

        public VCollectionQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        public void InitData()
        {
            VBasicDataOptr.SendStyleType(cmoSendStyle, true);
            VBasicDataOptr.LettersStyleType(cmoLettersStyle, true);
        }
        /// <summary>
        /// 当前菜单所属的权限菜单
        /// </summary>
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VCollectionManage vImple = new VCollectionManage();
                    vImple.Start(billId);
                    vImple.ShowDialog();
                }
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            projectInfo = StaticMethod.GetProjectInfo();
            condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
            //创建日期

            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.SendLettersDate>='" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "' and t1.SendLettersDate<'" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.SendLettersDate>=to_date('" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.SendLettersDate<to_date('" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }
            condition += "and t1.DocState <>" + "0" + "";

            //函件编号
            if (this.txtLettersId.Text != "")
            {
                condition = condition + " and t1.LettersId like '%" + this.txtLettersId.Text + "%'";
            }

            //函件名称
            if (this.txtPlanName.Text != "")
            {
                condition = condition + " and t1.LettersName like '%" + this.txtPlanName.Text + "%'";
            }
            //函件类型
            if (this.cmoLettersStyle.SelectedItem != null)
            {
                condition += "and t1.LettersStyle = '" + cmoLettersStyle.Text + "'";
            }
            //发函单位
            if (this.txtSendUnits.Text != "")
            {
                condition = condition + " and t1.SendUnits like '%" + this.txtSendUnits.Text + "%'";
            }
            //收函单位
            if (this.txtGetUnits.Text != "")
            {
                condition = condition + " and t1.GetUnits like '%" + this.txtGetUnits.Text + "%'";
            }
            //收发类型
            if (this.cmoSendStyle.SelectedItem != null)
            {
                condition += "and t1.SendStyle = '" + cmoSendStyle.Text + "'";
            }
            #endregion

            DataSet dataSet = model.ICollectionMngSrv.CollectionQuery(condition);
            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["Id"]);
                dgDetail[colLettersId.Name, rowIndex].Value = dataRow["LettersId"].ToString();
                dgDetail[colLettersName.Name, rowIndex].Value = dataRow["LettersName"].ToString();
                dgDetail[colLettersStyle.Name, rowIndex].Value = dataRow["LettersStyle"].ToString();
                dgDetail[colSendUnits.Name, rowIndex].Value = dataRow["SendUnits"].ToString();
                dgDetail[colGetUnits.Name, rowIndex].Value = dataRow["GetUnits"].ToString();
                dgDetail[colRemark.Name, rowIndex].Value = dataRow["Remark"].ToString();
                dgDetail[colSendStyle.Name, rowIndex].Value = dataRow["SendStyle"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                }
                string b = dataRow["SendLettersDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colSendLettersDate.Name, rowIndex].Value = strb;
            }

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
