using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI
{
    public partial class VDepartSelector : TBasicDataView
    {
        MStockMng model = new MStockMng();
        private int totalRecords = 0;
        private decimal sumQuantity = 0;

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
        string j = "0";
        public VDepartSelector(string i)
        {
            InitializeComponent();
            j = i;
            txtName .Text =i;
            InitEvent();
        }
        private void InitEvent()
        {
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            btnSearch.Click += new EventHandler(btnSearch_Click);

            this.Load += new EventHandler(VDepartSelector_Load);
        }

        void VDepartSelector_Load(object sender, EventArgs e)
        {
            string con = "";
            string sValue = txtName.Text.Trim();
            //if (j != "")
            if (!string.IsNullOrEmpty(sValue))
            {
                con = con + " and ProjectName like '%" + sValue + "%'";
            }
            FillData(con);
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
                MessageBox.Show("没有可以引用的合同！");
                return;
            }
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                if (var.IsNewRow) break;
            }
            string strProjectName = ClientUtil.ToString(dgMaster.CurrentRow.Cells[colName.Name].Value);
            CurrentProjectInfo project = new CurrentProjectInfo();
            if (j == "1")
            {
                ObjectQuery objectQuery = new ObjectQuery();
                objectQuery.AddCriterion(Expression.Eq("Name", strProjectName));
                IList SearchResult = model.CurrentProjectSrv.GetCurrentProjectInfo(objectQuery);
                if (SearchResult != null && SearchResult.Count > 0)
                {
                    CurrentProjectInfo projectInfo = SearchResult[0] as CurrentProjectInfo;
                    dgMaster.CurrentRow.Cells[colName.Name].Tag = projectInfo;
                }
                project.Name = ClientUtil.ToString(this.dgMaster.CurrentRow.Cells[colName.Name].Value);
                project = dgMaster.CurrentRow.Cells[colName.Name].Tag as CurrentProjectInfo;
            }
            else
            {
                project.Name = ClientUtil.ToString(this.dgMaster.CurrentRow.Cells[colName.Name].Value);

            }
            result.Add(project);
            this.btnOK.FindForm().Close();
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (j == "0")
                {
                    string condition = "";
                    //查询条件
                    if (this.txtName.Text != "")
                    {
                        condition = condition + " and ProjectName like '%" + this.txtName.Text + "%'";
                    }
                    FillData(condition);
                }
                if (j == "1")
                {
                    string strName = ClientUtil.ToString(this.txtName.Text);
                    ObjectQuery oq = new ObjectQuery();
                    if (strName != "")
                    {
                        oq.AddCriterion(Expression.Like("Name", strName, MatchMode.Anywhere));
                    }
                    FillData(oq);
                }
                else
                {
                    string condition = "";
                    //查询条件
                    if (this.txtName.Text != "")
                    {
                        //condition = condition + " and ProjectName like '%" + j + "%'";
                        condition = condition + " and ProjectName like '%" + this.txtName .Text .Trim ()+ "%'";
                    }
                    FillData(condition);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }
        void FillData(string condition)
        {
            DataSet dataSet = model.StockMoveSrv.DepartQuery(condition);
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
        void FillData(ObjectQuery oq)
        {
            this.dgMaster.Rows.Clear();
            IList list = model.StockMoveSrv.GetDomainByCondition(typeof(CurrentProjectInfo), oq);
            if (list != null && list.Count > 0)
            {
                foreach (CurrentProjectInfo project in list)
                {
                    int rowIndex = this.dgMaster.Rows.Add();
                    dgMaster[colName.Name, rowIndex].Value = project.Name;
                    dgMaster[colName.Name, rowIndex].Tag = project;
                    dgMaster.Tag = project;
                }
            }

            if (ListExtendProject != null && ListExtendProject.Count > 0)
            {
                if (oq.Criterions.Count == 0)
                {
                    foreach (CurrentProjectInfo pro in ListExtendProject)
                    {
                        int rowIndex = this.dgMaster.Rows.Add();
                        dgMaster[colName.Name, rowIndex].Value = pro.Name;
                        dgMaster[colName.Name, rowIndex].Tag = pro;
                    }
                }
                else
                {
                    string proName = txtName.Text.Trim();
                    var queryProject = from p in ListExtendProject
                                       where p.Name.IndexOf(proName) > -1
                                       select p;

                    if (queryProject != null && queryProject.Count() > 0)
                    {
                        foreach (CurrentProjectInfo pro in queryProject)
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
}
