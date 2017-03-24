using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;


namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VSelectPersonInfo : TBasicDataView
    {
        private OperationOrg optOrgInfo = null;
        private bool _isOK = false;
        private List<PersonInfo> _listResult = new List<PersonInfo>();
        /// <summary>
        /// 获取选择的人员列表
        /// </summary>
        public List<PersonInfo> ListResult
        {
            get { return _listResult; }
        }
        private MGWBSTree model = new MGWBSTree();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project">查询的项目（无查询范围时设置为NULL）</param>
        public VSelectPersonInfo(CurrentProjectInfo project)
        {
            if (project != null)
            {
                optOrgInfo = model.GetObjectById(typeof(OperationOrg), project.OwnerOrg.Id) as OperationOrg;
            }

            InitializeComponent();

            InitEvents();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optOrg">查询的业务组织（无查询范围时设置为NULL）</param>
        public VSelectPersonInfo(OperationOrg optOrg)
        {
            optOrgInfo = optOrg;

            InitializeComponent();

            InitEvents();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optOrg">查询的业务组织（无查询范围时设置为NULL）</param>
        public VSelectPersonInfo(OperationOrgInfo optOrg)
        {
            if (optOrg != null)
            {
                optOrgInfo = new OperationOrg();
                optOrgInfo.Id = optOrg.Id;
                optOrgInfo.SysCode = optOrg.SysCode;
            }

            InitializeComponent();

            InitEvents();
        }

        private void InitEvents()
        {
            gridQueryDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridQueryDetail_CellDoubleClick);

            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnSelect.Click += new EventHandler(btnSelect_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.FormClosing += new FormClosingEventHandler(VSelectGWBSDetail_FormClosing);
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            if (gridQueryDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请至少选择一条任务明细！");
                return;
            }

            foreach (DataGridViewRow row in gridQueryDetail.SelectedRows)
            {
                PersonInfo p = row.Tag as PersonInfo;
                PersonInfo per = model.GetObjectById(typeof(PersonInfo), p.Id) as PersonInfo;
                _listResult.Add(per);
            }

            _isOK = true;

            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            _listResult.Clear();
            this.Close();
        }

        void VSelectGWBSDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isOK)
                _listResult.Clear();
        }


        void btnQuery_Click(object sender, EventArgs e)
        {
            string opgSyscode = optOrgInfo == null ? "" : optOrgInfo.SysCode;
            string perCode = txtPerCode.Text.Trim();
            string perName = txtPerName.Text.Trim();

            try
            {
                FlashScreen.Show("正在查询数据,请稍候......");

                string sql = "select t3.perid,t3.percode,t3.pername from respersononjob t1"
                            + " inner join resoperationjob t2 on t1.operationjobid=t2.opjid"
                            + " inner join resperson t3 on t1.perid=t3.perid"
                            + " where t3.states=1 and t2.opjsyscode like '" + opgSyscode + "%'";
                if (perCode != "")
                {
                    sql += " and t3.percode like '%" + perCode + "%' ";
                }
                if (perName != "")
                {
                    sql += " and t3.pername like '%" + perName + "%' ";
                }
                sql += " group by t3.perid,t3.percode,t3.pername order by t3.percode,t3.pername";

                DataSet ds = model.SearchSQL(sql);

                gridQueryDetail.Rows.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PersonInfo p = new PersonInfo();
                    p.Id = dr["perid"].ToString();
                    p.Code = dr["percode"].ToString();
                    p.Name = dr["pername"].ToString();

                    int index = gridQueryDetail.Rows.Add();
                    DataGridViewRow row = gridQueryDetail.Rows[index];
                    row.Cells[DtlCode.Name].Value = p.Code;
                    row.Cells[DtlName.Name].Value = p.Name;

                    row.Tag = p;
                }

                gridQueryDetail.ClearSelection();

            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        //双击选择
        void gridQueryDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            btnSelect_Click(btnSelect, new EventArgs());
        }

    }
}