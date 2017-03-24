using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI
{
    public partial class VSelectOperationRole : Form
    {
        private List<OperationRole> result = new List<OperationRole>();
        private static IOperationJobManager operationJobService;

        public List<OperationRole> Result
        {
            get { return result; }
        }
        
        public VSelectOperationRole()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            btnOk.Click += new EventHandler(btnOk_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            this.Load += new EventHandler(VSelectOperationRole_Load);
        }

        void VSelectOperationRole_Load(object sender, EventArgs e)
        {
            LoadOperationRole();
        }

        private void LoadOperationRole()
        {
            IList roleLst = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", 1));
            try
            {
                roleLst = GetOperationJobService().GetOperationRole(oq);
                if (roleLst != null && roleLst.Count > 0)
                {
                    foreach (OperationRole role in roleLst)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        DataGridViewRow dr = dgDetail.Rows[rowIndex];
                        dr.Tag = role;
                        dr.Cells[colSelect.Name].Value = false;
                        dr.Cells[colRoleName.Name].Value = role.RoleName;
                        dr.Cells[colDescript.Name].Value = role.Descript;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询角色出错了。\n"+ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private IOperationJobManager GetOperationJobService()
        {
            if (operationJobService == null) operationJobService = StaticMethod.GetService("OperationJobManager") as IOperationJobManager;
            return operationJobService;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if ((bool)dr.Cells[colSelect.Name].Value)
                {
                    OperationRole role = dr.Tag as OperationRole;
                    result.Add(role);
                }
            }
            if (result.Count == 0)
            {
                MessageBox.Show("请选择角色。");
                return;
            }
            this.Close();
        }
    }
}
