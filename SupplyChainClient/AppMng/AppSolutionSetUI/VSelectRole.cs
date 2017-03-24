using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI
{
    public partial class VSelectRole : Form
    {
        private MAppSolutionSet Model = new MAppSolutionSet();
        private IList lstInstance;
        public SysRole _SysRole;
        public VSelectRole()
        {
            InitializeComponent();
            LoadOrgTree();
        }
        private void LoadOrgTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvOrg.Nodes.Clear();
                //IList list = uim.GetOperationOrgs(typeof(OperationOrg));
                IList listAll = Model.GetOpeOrgsByInstance();
                lstInstance = listAll[1] as IList;
                IList list = listAll[0] as IList;
                foreach (OperationOrg childNode in list)
                {
                    if (childNode.State == 0) continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    //if (childNode.ChildNodes.Count == 0)
                    //{
                    //    tnTmp.Nodes.Add("ZCR");
                    //}
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                            tnp.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        tvOrg.Nodes.Add(tnTmp);
                    }
 
                    hashtable.Add(tnTmp.Name, tnTmp);
                    if (ConstMethod.Contains(lstInstance, childNode) && childNode.OperationJobs.Count > 0)
                    {
                        foreach (OperationJob var in childNode.OperationJobs)
                        {
                            TreeNode tnJobTmp = new TreeNode();
                            tnJobTmp.Name = var.Id.ToString();
                            tnJobTmp.Text = var.Name;
                            tnJobTmp.Tag = var;
                            tnJobTmp.SelectedImageIndex = 1;
                            tnJobTmp.ImageIndex = 1;

                            tnTmp.Nodes.Add(tnJobTmp);

                            //if (condition != null)
                            //{
                            //    for (int i = 0; i < a.Length; i++)
                            //    {
                            //        if (a.GetValue(i).ToString() == tnJobTmp.Name)
                            //        {
                            //            tnJobTmp.Checked = true;
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                    }
                    //if (list.Count > 0)
                    //{
                    //    tvOrg.ExpandAll();
                    //}
                }
                if (list.Count > 0)
                {
                    this.tvOrg.SelectedNode = this.tvOrg.Nodes[0];
                    this.tvOrg.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("查询业务组织出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Selected();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            _SysRole = null;
            this.Close();
        }

        private void tvOrg_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Selected();
        }

        void Selected()
        {
            if (tvOrg.SelectedNode != null)
            {
                OperationJob job = tvOrg.SelectedNode.Tag as OperationJob;
                if (job == null)
                {
                    MessageBox.Show("请重新选择末级岗位！");
                    return;
                }

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", job.Id));
                _SysRole = Model.GetDomain(typeof(SysRole), oq) as SysRole;
            }
            this.Close();
        }

    }
}
