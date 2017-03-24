using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage
{
    public partial class VGwbsManagedetail : TBasicToolBarByMobile
    {
        MGWBSTree model = new MGWBSTree();
        private AutomaticSize automaticSize = new AutomaticSize();
        public GWBSTree DefaultGWBSTree = null;


        public VGwbsManagedetail()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitEvent();
        }

        private void InitEvent()
        {   
            this.Load += new EventHandler(VGwbsManagedetail_Load);            
        }
        private string GetFullPath(ProjectTaskTypeTree taskType)
        {
            string path = string.Empty;

            path = taskType.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", taskType.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

            taskType = list[0] as ProjectTaskTypeTree;

            CategoryNode parent = taskType.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }
        void VGwbsManagedetail_Load(object sender, EventArgs e)
        {



            if(DefaultGWBSTree!=null)
            {
                txtTaskName.Text = DefaultGWBSTree.Name;
                this.txtOwner.Text = DefaultGWBSTree.OwnerName;
               this. txtFigureProgress.Text = DefaultGWBSTree.AddUpFigureProgress.ToString();
                this.txtCheckBatchNum.Text = DefaultGWBSTree.CheckBatchNumber.ToString();
                this.txtTaskDesc.Text = DefaultGWBSTree.Describe;

                if (DefaultGWBSTree.ProjectTaskTypeGUID != null)
                {

                    this.txtTaskWBSType.Text = GetFullPath(DefaultGWBSTree.ProjectTaskTypeGUID);
                    this.txtTaskWBSType.Tag = DefaultGWBSTree.ProjectTaskTypeGUID;
                }

               



                if (DefaultGWBSTree.ListRelaPBS.Count > 0)//关联的PBS
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddFetchMode("ThePBS", NHibernate.FetchMode.Eager);

                    Disjunction dis = new Disjunction();
                    foreach (GWBSRelaPBS rela in DefaultGWBSTree.ListRelaPBS)
                    {
                        dis.Add(Expression.Eq("Id", rela.Id));
                    }
                    oq.AddCriterion(dis);

                    IList listRela = model.ObjectQuery(typeof(GWBSRelaPBS), oq);

                    List<PBSTree> listPBS = new List<PBSTree>();
                    for (int i = 0; i < listRela.Count; i++)
                    {
                        PBSTree pbs = (listRela[i] as GWBSRelaPBS).ThePBS;
                        pbs.FullPath = GetFullPath(pbs);

                        cbRelaPBS.Items.Add(pbs);
                        
                        listPBS.Add(pbs);
                    }  

                    cbRelaPBS.DisplayMember = "FullPath";
                    cbRelaPBS.ValueMember = "Id";
                    cbRelaPBS.Tag = listPBS;

                    //cbRelaPBS.SelectedIndex = 0;
                }
            }
        }

        private string GetFullPath(PBSTree wbs)
        {
            string path = string.Empty;

            path = wbs.Name;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wbs.Id));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(PBSTree), oq);

            wbs = list[0] as PBSTree;

            CategoryNode parent = wbs.ParentNode;
            while (parent != null)
            {
                path = parent.Name + "\\" + path;

                oq.Criterions.Clear();
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Id", parent.Id));
                oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
                list = model.ObjectQuery(typeof(PBSTree), oq);

                parent = (list[0] as CategoryNode).ParentNode;
            }

            return path;
        }

        private void VGwbsManagedetail_Load_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

     
    }
}
