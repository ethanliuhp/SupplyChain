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
namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary
{
    public partial class VMeetingQuery : TBasicDataView
    {
        private MMeetingManage model = new MMeetingManage();

        public VMeetingQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        public void InitData()
        {
            this.txtMeetingStyle.Items.AddRange(new object[] { "周例会", "监理会议", "其他" });
        }
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
                    VMeetingManage vImple = new VMeetingManage();
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
            //会议日期

             if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.MeetingDate>='" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "' and t1.MeetingDate<'" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.MeetingDate>=to_date('" + dtpCreateDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.MeetingDate<to_date('" + dtpCreateDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

            //会议主题
             if (this.txtMeetingTopic.Text != "")
            {
                condition = condition + " and t1.MeetingTopic like '%" + this.txtMeetingTopic.Text + "%'";
            }
          
            //会议类型
            if (this.txtMeetingStyle.SelectedItem != null)
            {
                condition += "and t1.MeetingStyle = '" + txtMeetingStyle.SelectedItem + "'";
            }
            condition += "and t1.DocState <>" + "0" + "";

            #endregion 
            DataSet dataSet = model.IMeetingMngSrv.MeetingQuery(condition);
            this.dgDetail.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddOrder(Order.Asc("MeetingDate"));
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                currRow.Tag = ClientUtil.ToString(dataRow["Id"]);
                dgDetail[colMeetingStyle.Name, rowIndex].Value = dataRow["MeetingStyle"].ToString();            
                dgDetail[colMeetingTopic.Name, rowIndex].Value = dataRow["MeetingTopic"].ToString();
                dgDetail[colMeetingRemark.Name, rowIndex].Value = dataRow["MeetingRemark"].ToString();
                dgDetail[colCreatePersonName.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                {
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                }
                string b = dataRow["MeetingDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colMeetingDate.Name, rowIndex].Value = strb;     
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }

}
