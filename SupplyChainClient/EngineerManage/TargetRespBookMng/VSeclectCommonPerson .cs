using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.EngineerManage.Client.TargetRespBookMng;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    public partial class VSeclectCommonPerson : TBasicDataView
    {
        private MTargetRespBookMng model = new MTargetRespBookMng();
        CurrentProjectInfo projectInfo =  new CurrentProjectInfo();

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VSeclectCommonPerson(CurrentProjectInfo projectInfo)
        {
            InitializeComponent();
            this.projectInfo = projectInfo;
            InitEvents();
        }

        public void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnEnter.Click += new EventHandler(btnEnter_Click);
            cbAllSelect.Click += new EventHandler(cbAllSelect_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            this.Load += new EventHandler(VSeclectCommonPerson_Load);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnEnter.FindForm().Close();
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow.Cells[colSelect.Name].Value == null)
            {
                MessageBox.Show("请选择人员名称......");
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.Cells[colSelect.Name].Value != null && Convert.ToBoolean(var.Cells[colSelect.Name].Value))
                {
                    if (var.IsNewRow) break;
                    IrpRiskDepositPayRecord dtl = new IrpRiskDepositPayRecord();

                    dtl.Name = ClientUtil.ToString(var.Cells[colPersonName.Name].Value);
                    dtl.ProjectPosition = ClientUtil.ToString(var.Cells[colPersonOnJob.Name].Value);
                    result.Add(dtl);
                }
            }
            this.btnEnter.FindForm().Close();
        }

        void cbAllSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgDetail.Rows)
            {
                row.Cells[colSelect.Name].Value = cbAllSelect.Checked;
            }

            int recordCount = 0;
            if (cbAllSelect.Checked)
                recordCount = dgDetail.Rows.Count;

            UpdateRowRecordCount(recordCount);
        }

        private void UpdateRowRecordCount(int recordCount)
        {
            lblSelectCount.Text = "共选择" + recordCount + "条记录";
        }
        private void UpdateRowRecordCount()
        {
            int recordCount = 0;
            foreach (DataGridViewRow row in dgDetail.Rows)
            {
                if (row.Cells[colSelect.Name].Value != null && (bool)row.Cells[colSelect.Name].Value)
                {
                    recordCount += 1;
                }
            }
            lblSelectCount.Text = "共选择" + recordCount + "条记录";
        }

        void VSeclectCommonPerson_Load(object sender, EventArgs e)
        {
            string con = "";
            FillData(con);
            if (dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("该项目下没有相关的人员和职务");
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            string condition = "";
            if (this.txtOperationorg.Text != "")
            {
                condition = condition + "and t4.opgname like '%" + this.txtOperationorg.Text + "%'";
            }
            if (this.txtPersonOnJob.Text != "")
            {
                condition = condition + "and t3.opjname like '%" + this.txtPersonOnJob.Text + "%'";
            }

            FillData(condition);
        }

        void FillData(string condition)
        {
            if (projectInfo != null)
            {
                condition += " and t5.projectname = '" + projectInfo.Name + "'";
            }
            DataSet dataSet = model.TargetRespBookSrc.PersonInfoQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                DataGridViewRow currRow = dgDetail.Rows[rowIndex];

                //currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                dgDetail[colPersonName.Name, rowIndex].Value = dataRow["pername"].ToString();
                dgDetail[colOperationOrg.Name, rowIndex].Value = dataRow["opgname"].ToString();
                dgDetail[colPersonOnJob.Name, rowIndex].Value = dataRow["opjname"].ToString();
                dgDetail[colProjectName.Name, rowIndex].Value = dataRow["projectname"].ToString();
            }
            lblSelectCount.Text = "共【" + dgDetail.Rows.Count + "】条记录";
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
    }
}
