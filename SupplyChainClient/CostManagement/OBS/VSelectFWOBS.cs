using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
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
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
//using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OBS
{
    public partial class VSelectFWOBS : TBasicDataView
    {
        private GWBSTree oprNode = null;
        public MGWBSTree model;
        private IList lstInstance;
        public List<GWBSDetail> listCopyNodeDetail = new List<GWBSDetail>();
        public MOBS OBSModel = new MOBS();
     
        private OBSService curBillService;

        /// <summary>
        /// ����OBS����
        /// </summary>
        public OBSService CurBillService
        {
            get { return curBillService; }
            set { curBillService = value; }
        }

        public VSelectFWOBS(MGWBSTree mot)
        {
            model = mot;
            InitializeComponent();
            tvwCategory.CheckBoxes = false;
            InitForm();
        }

        private void InitForm()
        {
            InitEvents();
            LoadGWBSTreeTree();
        }
        private void InitEvents()
        {
            //tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);
            //tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            //btnFWCancel.Click += new EventHandler(btnFWCancel_Click);
            //btnFWOK.Click += new EventHandler(btnFWOK_Click);
        }

        private void LoadGWBSTreeTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();

                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                IList list = model.GetGWBSTreesByInstance(projectInfo.Id);
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;
                foreach (GWBSTree childNode in list)
                {
                    //if (childNode.State == 0)
                    //    continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                            tnp.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("��ѯҵ����֯����" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        ///// <summary>
        ///// ѡ�����ڵ�
        ///// </summary>
        ///// <returns></returns>
        //void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, e.Node.Tag as CategoryNode)
        //    {
        //        tvwCategory.SelectedNode = e.Node;
        //        oprNode = e.Node.Tag as GWBSTree;
        //    }
        //}

        //private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    try
        //    {
        //        oprNode = tvwCategory.SelectedNode.Tag as GWBSTree;
        //        oprNode = LoadRelaPBS(oprNode);
        //        ModelToViewService();
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
        //    }
        //}

        //private GWBSTree LoadRelaPBS(GWBSTree wbs)
        //{
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("Id", wbs.Id));
        //    oq.AddFetchMode("ListRelaPBS", NHibernate.FetchMode.Eager);
        //    IList list = model.ObjectQuery(typeof(GWBSTree), oq);
        //    if (list.Count > 0)
        //    {
        //        wbs = list[0] as GWBSTree;
        //    }
        //    return wbs;
        //}

        ///// <summary>
        ///// ˢ�¹���OBS
        ///// </summary>
        ///// <returns></returns>
        //private bool ModelToViewService()
        //{
        //    try
        //    {
        //        curBillService = new OBSService();
        //        curBillService.ProjectTask = tvwCategory.SelectedNode.Tag as GWBSTree;
        //        ObjectQuery oq = new ObjectQuery();
        //        oq.AddCriterion(Expression.Eq("ProjectTask", curBillService.ProjectTask));
        //        IList lists = OBSModel.OBSSrv.ObjectQuery(typeof(OBSService), oq);
        //        if (lists.Count <= 0 || lists == null)
        //        {
        //            dgService.Rows.Clear();
        //            return false;
        //        }
        //        dgService.Rows.Clear();
        //        foreach (OBSService var in lists)
        //        {
        //            int i = this.dgService.Rows.Add();
        //            this.dgService[colFWBegionDate.Name, i].Value = var.BeginDate;//��ʼʱ��
        //            this.dgService[colFWEndDate.Name, i].Value = var.EndDate;//����ʱ��
        //            this.dgService[colFWProjectTask.Name, i].Value = var.ProjectTaskName;//������������
        //            this.dgService[colFWProjectTask.Name, i].Tag = var.ProjectTask;//��������
        //            this.dgService[colFWSupplier.Name, i].Value = var.SupplierName;//��Ӧ������
        //            this.dgService[colFWSupplier.Name, i].Tag = var.SupplierId;//��Ӧ��
        //            this.dgService[colFWHandlePerson.Name, i].Value = var.PersonName;//������
        //            this.dgService[colFWId.Name, i].Value = var.Id;//���
        //            this.dgService[colFWState.Name, i].Value = var.ServiceState;//״̬
        //            this.dgService[colFWPersonNumber.Name, i].Value = var.PersonNumber;//���֤
        //            this.dgService.Rows[i].Tag = var;
        //        }

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //        //MessageBox.Show("����ӳ�����" + StaticMethod.ExceptionMessage(e));
        //        //return false;
        //    }
        //}

        //#region ��ť����
        ///// <summary>
        ///// ����OBSȷ��
        ///// </summary>
        ///// <returns></returns>
        //void btnFWOK_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (tvwCategory.SelectedNode == null)
        //        {
        //            MessageBox.Show("��ѡ��һ���ڵ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        DataGridViewRow var = dgService.CurrentRow;
        //        if (var.IsNewRow)
        //        {
        //            MessageBox.Show("��ѡ�����OBS��Ϣ");
        //            return;
        //        }
        //        curBillService.BeginDate = Convert.ToDateTime(var.Cells[colFWBegionDate.Name].Value);
        //        curBillService.EndDate = Convert.ToDateTime(var.Cells[colFWEndDate.Name].Value);
        //        curBillService.ProjectTaskName = ClientUtil.ToString(var.Cells[colFWProjectTask.Name].Value);
        //        curBillService.ProjectTask = var.Cells[colFWProjectTask.Name].Tag as GWBSTree;
        //        curBillService.SupplierName = ClientUtil.ToString(var.Cells[colFWSupplier.Name].Value);
        //        curBillService.SupplierId = var.Cells[colFWSupplier.Name].Tag as SupplierRelationInfo;
        //        curBillService.PersonName = ClientUtil.ToString(var.Cells[colFWHandlePerson.Name].Value);
        //        curBillService.Id = ClientUtil.ToString(var.Cells[colFWId.Name].Value);
        //        curBillService.PersonNumber = ClientUtil.ToString(var.Cells[colFWPersonNumber.Name].Value);
                
        //        this.Close();
        //    }
        //    catch (Exception err)
        //    {
        //        MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(err));
        //    }
        //}

        ///// <summary>
        ///// ����OBSȡ��
        ///// </summary>
        ///// <returns></returns>
        //void btnFWCancel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.Close();
                
        //    }
        //    catch (Exception err)
        //    {
        //        MessageBox.Show("���ݴ���" + ExceptionUtil.ExceptionMessage(err));
        //    }
        //}

        //#endregion
    }
}
