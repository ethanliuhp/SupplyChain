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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    public partial class VSelectProjectInfo : TBasicDataView
    {
        private MTargetRespBookMng model = new MTargetRespBookMng();
        MStockMng mstock = new MStockMng();
        public List<CurrentProjectInfo> ListExtendProject = new List<CurrentProjectInfo>();


        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VSelectProjectInfo()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);

            this.Load += new EventHandler(VSelectProjectInfo_Load);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有选中项目！");
                return;
            }
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                if (var.IsNewRow) break;
            }
            string strProjectName = ClientUtil.ToString(dgMaster.CurrentRow.Cells[colName.Name].Value);
            CurrentProjectInfo project = new CurrentProjectInfo();
            
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Name", strProjectName));
            IList SearchResult = mstock.CurrentProjectSrv.GetCurrentProjectInfo(objectQuery);
            if (SearchResult != null && SearchResult.Count > 0)
            {
                CurrentProjectInfo projectInfo = SearchResult[0] as CurrentProjectInfo;
                dgMaster.CurrentRow.Cells[colName.Name].Tag = projectInfo;
            }
            project.Name = ClientUtil.ToString(this.dgMaster.CurrentRow.Cells[colName.Name].Value);
            project = dgMaster.CurrentRow.Cells[colName.Name].Tag as CurrentProjectInfo;
           
            result.Add(project);
            this.btnOK.FindForm().Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string condition = "";
            //查询条件
            if (this.txtName.Text != "")
            {
                condition = condition + " and ProjectName like '%" + this.txtName.Text + "%'";
            }
            FillData(condition);  
        }

        void VSelectProjectInfo_Load(object sender, EventArgs e)
        {
            string con = "";
            con = con + " and ProjectName like '%" + this.txtName.Text + "%'"; 
            FillData(con);
        }
      

        void FillData(string condition)
        {
            DataSet dataSet = model.TargetRespBookSrc.GetProjectInfo(condition);
            this.dgMaster.Rows.Clear();
            DataTable dataTable = dataSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgMaster.Rows.Add();
                dgMaster[colName.Name, rowIndex].Value = dataRow["ProjectName"];
            }

            if (condition == "")
            {
                if (ListExtendProject != null && ListExtendProject.Count > 0)
                {
                    foreach (CurrentProjectInfo pro in ListExtendProject)
                    {
                        int rowIndex = this.dgMaster.Rows.Add();
                        dgMaster[colName.Name, rowIndex].Value = pro.Name;
                        dgMaster[colName.Name, rowIndex].Tag = pro;
                    }
                }
            }
        }
    }
}
