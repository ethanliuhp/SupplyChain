using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng
{
    public partial class VProjectCopy : TBasicDataView
    {
        string reportName = "项目工程复制";
        /// <summary>
        /// 结构类型
        /// </summary>
        private List<BasicDataOptr> listStructureType;
        private TreeNode currNode = null;
        MProjectCopy model = new MProjectCopy();
        CurrentProjectInfo ProjectInfoLeft = new CurrentProjectInfo();
        CurrentProjectInfo ProjectInfoRight = new CurrentProjectInfo();

        IList listYPBS = new ArrayList();
        IList listYWBS = new ArrayList();

        private  PBSTree curPBS;
        public PBSTree CurPBS
        {
            get { return curPBS; }
            set { curPBS = value; }
        }
        private GWBSTree curGWBS;
        public GWBSTree CurGWBS
        {
            get { return curGWBS; }
            set { curGWBS = value; }
        }

        public MPBSTree modelPBS;
        public MGWBSTree modelWBS;
        public VProjectCopy(MPBSTree mPBS, MGWBSTree mWBS)
        {
            InitializeComponent();
            modelPBS = mPBS;
            modelWBS = mWBS;
            string strName = "";
            InitEvent();
            InitData(strName);
            InitDataRight(strName);
        }

        private void InitEvent()
        {
            btnCopy.Click +=new EventHandler(btnCopy_Click);
            listBoxType.SelectedIndexChanged += new EventHandler(listBoxType_SelectedIndexChanged);
            listBoxTypeRight.SelectedIndexChanged +=new EventHandler(listBoxTypeRight_SelectedIndexChanged);
            //结构类型
            listStructureType = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.PBS_StructType).OfType<BasicDataOptr>().ToList();
            PBSCategory.CheckBoxes = false;
            WBSCategory.CheckBoxes = false;
            btnYSearch.Click += new EventHandler(btnYSearch_Click);
            btnMSearch.Click +=new EventHandler(btnMSearch_Click);
        }

        void btnYSearch_Click(object sender,EventArgs e)
        {
            string strName = txtYProject.Text;
            InitData(strName);
        }

        void btnMSearch_Click(object sender,EventArgs e)
        {
            string strName = txtMProject.Text;
            InitDataRight(strName);
        }

        private void InitData(string strName)
        {
            //listBoxType = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.listBoxType.HorizontalScrollbar = true;
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddOrder(Order.Asc("Name"));
            if (strName != "")
            {
                objectQuery.AddCriterion(Expression.Like("Name", strName,MatchMode.Anywhere));
            }
            IList list = model.ProjectCopySrv.GetDomainByCondition(typeof(CurrentProjectInfo), objectQuery);
            listBoxType.DataSource = null;
            foreach (CurrentProjectInfo obj in list)
            {
                listBoxType.Items.Add(obj);
            }
            try
            {
                listBoxType.DataSource = list;
                listBoxType.DisplayMember = "Name";
            }
            catch (Exception e)
            {
                string a = "";
            }
            if (listBoxType.Items.Count > 0)
            {
                listBoxType.SelectedIndex = 0;
                ProjectInfoLeft = listBoxType.SelectedItem as CurrentProjectInfo;
                LoadPBS();
                LoadWBS();
            }
        }

        private void InitDataRight(string strName)
        {
            //listBoxTypeRight = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.listBoxTypeRight.HorizontalScrollbar = true;
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddOrder(Order.Asc("Name"));
            if (strName != "")
            {
                objectQuery.AddCriterion(Expression.Like("Name", strName, MatchMode.Anywhere));
            }
            IList list = model.ProjectCopySrv.GetDomainByCondition(typeof(CurrentProjectInfo), objectQuery);
            listBoxTypeRight.DataSource = null;
            foreach (CurrentProjectInfo obj in list)
            {
                listBoxTypeRight.Items.Add(obj);
            }
            try
            {
                listBoxTypeRight.DataSource = list;
                listBoxTypeRight.DisplayMember = "Name";
            }
            catch (Exception e)
            {
            }
            if (listBoxTypeRight.Items.Count > 0)
            {
                listBoxTypeRight.SelectedIndex = 0;
                ProjectInfoRight = listBoxTypeRight.SelectedItem as CurrentProjectInfo;
            }
        }

        private void LoadPBS()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                PBSCategory.Nodes.Clear();
                IList list = modelPBS.GetPBSTreesByInstance(ProjectInfoLeft.Id);
                listYPBS = list;
                if (list.Count == 0)
                {
                }
                else if (list.Count == 1)//只有根节点
                {
                    IList listUpdate = new ArrayList();

                    PBSTree root = list[0] as PBSTree;
                    if (ProjectInfoLeft != null)
                    {
                        root.TheProjectGUID = ProjectInfoLeft.Id;
                        root.TheProjectName = ProjectInfoLeft.Name;

                        root.Name = ProjectInfoLeft.Name;
                        root.SysCode = root.Id + ".";
                    }

                    var listBasicData = from t in listStructureType
                                        where t.BasicName == "项目"
                                        select t;

                    if (listBasicData.Count() > 0)
                    {
                        BasicDataOptr baseData = listBasicData.ElementAt(0);

                        root.StructTypeGUID = baseData.Id;
                        root.StructTypeName = baseData.BasicName;


                        root.Code = ProjectInfoLeft.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + modelPBS.GetCode(typeof(PBSTree));
                    }

                    listUpdate.Add(root);


                    listBasicData = from t in listStructureType
                                    where t.BasicName == "建筑空间结构" || t.BasicName == "服务系统结构"
                                    orderby t.BasicCode ascending
                                    select t;

                    if (listBasicData.Count() > 0)
                    {
                        foreach (BasicDataOptr baseData in listBasicData)
                        {
                            PBSTree node = new PBSTree();
                            node.Name = baseData.BasicName;
                            node.StructTypeGUID = baseData.Id;
                            node.StructTypeName = baseData.BasicName;
                            node.Code = ProjectInfoLeft.Code.PadLeft(4, '0') + "-" + baseData.BasicCode + "-" + modelPBS.GetCode(typeof(PBSTree));

                            if (ProjectInfoLeft != null)
                            {
                                node.TheProjectGUID = ProjectInfoLeft.Id;
                                node.TheProjectName = ProjectInfoLeft.Name;
                            }

                            node.ParentNode = root;

                            listUpdate.Add(node);
                            //root.ChildNodes.Add(node);
                        }
                    }

                    modelPBS.SavePBSTrees(listUpdate);

                    list = modelPBS.GetPBSTreesByInstance(ProjectInfoLeft.Id);
                }

                foreach (PBSTree childNode in list)
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
                        PBSCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.PBSCategory.SelectedNode = this.PBSCategory.Nodes[0];
                    this.PBSCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        private void LoadWBS()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                WBSCategory.Nodes.Clear();
                IList list = modelWBS.GetGWBSTreesByInstance(ProjectInfoLeft.Id);
                listYWBS = list;
                foreach (GWBSTree childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

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
                        WBSCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.WBSCategory.SelectedNode = this.WBSCategory.Nodes[0];
                    this.WBSCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载数据出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        void listBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxType.SelectedItem != null)
            {
                ProjectInfoLeft = listBoxType.SelectedItem as CurrentProjectInfo;
                LoadPBS();
                LoadWBS();
            }
           
        }

        void listBoxTypeRight_SelectedIndexChanged(object sender,EventArgs e)
        {
            if (listBoxTypeRight.SelectedItem != null)
            {
                ProjectInfoRight = listBoxTypeRight.SelectedItem as CurrentProjectInfo;
                if (listBoxType.SelectedIndex != 0 && listBoxTypeRight.SelectedIndex != 0)
                {
                    if (listBoxTypeRight.Text == listBoxType.Text)
                    {
                        MessageBox.Show("源项目与目标项目不能相同");
                        return;
                    }
                }
            }
        }


        void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxType.Text == listBoxTypeRight.Text)
                {
                    MessageBox.Show("源项目与目标项目不能相同");
                    return;
                }
                //查询目标项目下的PBS信息
                IList listMPBS = modelPBS.GetPBSTreesByInstance(ProjectInfoRight.Id);
                //查询目标项目下的WBS信息
                IList listMWBS = modelWBS.GetGWBSTreesByInstance(ProjectInfoRight.Id);
                if (listMWBS.Count > 0)
                {
                    if (MessageBox.Show("目标项目工程不为空，是否复制？", "复制记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        //将原有的信息删除
                        model.ProjectCopySrv.DeleteCopy(ProjectInfoLeft, ProjectInfoRight, listYPBS, listYWBS);
                        MessageBox.Show("复制完毕！");
                    }
                }
                else
                {
                    model.ProjectCopySrv.SaveCopy(ProjectInfoLeft, ProjectInfoRight, listYPBS, listYWBS);
                    MessageBox.Show("复制完毕！");
                }

            }
            catch (Exception err)
            {
                MessageBox.Show("复制失败！");
            }
        }

        //private void SavePBS(IList listYWBS)
        //{
        //    IList RootPbS = new ArrayList();
        //    var queryRoot = from p in listYPBS.OfType<PBSTree>()
        //                    where p.CategoryNodeType == NodeType.RootNode
        //                    select p;

        //    PBSTree oldPBS = queryRoot.ElementAt(0);
        //    PBSTree newPBSRoot = new PBSTree();
        //    newPBSRoot.TheProjectGUID = ProjectInfoRight.Id;
        //    newPBSRoot.TheProjectName = ProjectInfoRight.Name;
        //    newPBSRoot.TheTree = oldPBS.TheTree;
        //    newPBSRoot.CategoryNodeType = oldPBS.CategoryNodeType;
        //    newPBSRoot.Describe = oldPBS.Describe;
        //    newPBSRoot.DocumentModelGUID = oldPBS.DocumentModelGUID;
        //    newPBSRoot.FullPath = oldPBS.FullPath;
        //    newPBSRoot.Level = oldPBS.Level;
        //    newPBSRoot.Name = oldPBS.Name;
        //    newPBSRoot.NodeDesc = oldPBS.NodeDesc;
        //    newPBSRoot.OrderNo = oldPBS.OrderNo;
        //    newPBSRoot.OwnerGUID = oldPBS.OwnerGUID;
        //    newPBSRoot.OwnerName = oldPBS.OwnerName;
        //    newPBSRoot.OwnerOrgSysCode = oldPBS.OwnerOrgSysCode;
        //    newPBSRoot.State = oldPBS.State;
        //    newPBSRoot.StructTypeGUID = oldPBS.StructTypeGUID;
        //    newPBSRoot.StructTypeName = oldPBS.StructTypeName;
        //    newPBSRoot.SysCode = oldPBS.SysCode;
        //    newPBSRoot.CreateDate = DateTime.Now;
        //    if (ProjectInfoRight != null && queryRoot.Count() > 0)
        //    {
        //        newPBSRoot.Code = ProjectInfoRight.Code.PadLeft(4, '0') + "-" + queryRoot.ElementAt(0).Code + "-" + modelPBS.GetCode(typeof(PBSTree));
        //    }
        //    listYPBS.RemoveAt(0);//将根节点删除
        //    RootPbS.Add(newPBSRoot);
        //    CopyPBSNode(newPBSRoot, oldPBS, listYPBS, ref RootPbS, ProjectInfoRight);
        //    RootPbS = modelPBS.SavePBSTrees(RootPbS);

        //    //if (listYWBS.Count != 0)
        //    //{
        //    //    IList RootGWBS = new ArrayList();
        //    //    var query = from p in listYWBS.OfType<GWBSTree>()
        //    //                where p.CategoryNodeType == NodeType.RootNode
        //    //                select p;

        //    //    if (query.Count() > 0)
        //    //    {
        //    //        foreach (GWBSTree gwbs in query)
        //    //        {
        //    //            GWBSTree childGWBS = new GWBSTree();
        //    //            childGWBS.Name = gwbs.Name;

        //    //            childGWBS.Describe = gwbs.Describe;
        //    //            childGWBS.Level = gwbs.Level;

        //    //            //foreach (GWBSRelaPBS rela in gwbs.ListRelaPBS)
        //    //            //{
        //    //            //    GWBSRelaPBS newRela = new GWBSRelaPBS();
        //    //            //    newRela.ThePBS = rela.ThePBS;
        //    //            //    newRela.PBSName = rela.PBSName;

        //    //            //    newRela.TheGWBSTree = childGWBS;
        //    //            //    childGWBS.ListRelaPBS.Add(newRela);
        //    //            //}
        //    //            //childGWBS.NGUID = gwbs.NGUID;
        //    //            childGWBS.NodeType = gwbs.NodeType;
        //    //            childGWBS.OrderNo = gwbs.OrderNo;
        //    //            childGWBS.OwnerGUID = gwbs.OwnerGUID;
        //    //            childGWBS.OwnerName = gwbs.OwnerName;
        //    //            childGWBS.OwnerOrgSysCode = gwbs.OwnerOrgSysCode;
        //    //            //childGWBS.State = gwbs.State;
        //    //            //childGWBS.SubContractFeeFlag = gwbs.SubContractFeeFlag;
        //    //            //childGWBS.Summary = gwbs.Summary;
        //    //            //childGWBS.SysCode = gwbs.SysCode;
        //    //            //childGWBS.TaskPlanEndTime = gwbs.TaskPlanEndTime;
        //    //            //childGWBS.TaskPlanStartTime = gwbs.TaskPlanStartTime;
        //    //            //childGWBS.TaskState = gwbs.TaskState;
        //    //            //childGWBS.TaskStateTime = gwbs.TaskStateTime;
        //    //            childGWBS.TheProjectGUID = ProjectInfoRight.Id;
        //    //            childGWBS.TheProjectName = ProjectInfoRight.Name;
        //    //            childGWBS.TheTree = gwbs.TheTree;
        //    //            //childGWBS.PlanTotalPrice = gwbs.PlanTotalPrice;
        //    //            //childGWBS.PriceAmountUnitGUID = gwbs.PriceAmountUnitGUID;
        //    //            //childGWBS.PriceAmountUnitName = gwbs.PriceAmountUnitName;
        //    //            //childGWBS.ProductConfirmFlag = gwbs.ProductConfirmFlag;
        //    //            childGWBS.ProjectTaskTypeGUID = gwbs.ProjectTaskTypeGUID;
        //    //            childGWBS.ProjectTaskTypeName = gwbs.ProjectTaskTypeName;
        //    //            //childGWBS.ResponsibilityTotalPrice = gwbs.ResponsibilityTotalPrice;
        //    //            //childGWBS.ResponsibleAccFlag = gwbs.ResponsibleAccFlag;
        //    //            if (ProjectInfoRight != null)
        //    //            {
        //    //                childGWBS.Code = ProjectInfoRight.Code.PadLeft(4, '0') + "-" + modelWBS.GetCode(typeof(GWBSTree));
        //    //            }
        //    //            //childGWBS.ParentNode = parentNode;
        //    //            //parentNode.ChildNodes.Add(childGWBS);
        //    //            listYWBS.RemoveAt(0);//将根节点删除
        //    //            RootGWBS.Add(childGWBS);
        //    //            CopyGWBSNode(childGWBS, gwbs, listYWBS, ref RootGWBS);
        //    //            //RootGWBS = modelWBS.SaveGWBSTreeRootNode(RootGWBS);
        //    //        }
        //    //    }
        //    //}
        //}


        ///// <summary>
        ///// 复制PBS节点
        ///// </summary>
        ///// <param name="parentNode">新生成的父节点</param>
        ///// <param name="copyParentNode">复制的父节点</param>
        ///// <param name="listPBS">复制的pbs节点集</param>
        //private void CopyPBSNode(PBSTree parentNode, PBSTree copyParentNode, IList listPBS, ref IList lst, CurrentProjectInfo ProjectInfoRight)
        //{
        //    var query = from p in listPBS.OfType<PBSTree>()
        //                where p.ParentNode.Id == copyParentNode.Id
        //                select p;

        //    if (query.Count() > 0)
        //    {
        //        foreach (PBSTree pbs in query)
        //        {
        //            PBSTree childPBS = new PBSTree();
        //            if (ProjectInfoRight != null && query.Count() > 0)
        //            {
        //                childPBS.Code = ProjectInfoRight.Code.PadLeft(4, '0') + "-" + query.ElementAt(0).Code + "-" + modelPBS.GetCode(typeof(PBSTree));
        //            }
        //            childPBS.CategoryNodeType = pbs.CategoryNodeType;
        //            childPBS.Describe = pbs.Describe;
        //            childPBS.DocumentModelGUID = pbs.DocumentModelGUID;
        //            childPBS.FullPath = pbs.FullPath;
        //            childPBS.Level = pbs.Level;
        //            childPBS.Name = pbs.Name;
        //            childPBS.NodeDesc = pbs.NodeDesc;
        //            childPBS.OrderNo = pbs.OrderNo;
        //            childPBS.OwnerGUID = pbs.OwnerGUID;
        //            childPBS.OwnerName = pbs.OwnerName;
        //            childPBS.OwnerOrgSysCode = pbs.OwnerOrgSysCode;
        //            childPBS.State = pbs.State;
        //            childPBS.StructTypeGUID = pbs.StructTypeGUID;
        //            childPBS.StructTypeName = pbs.StructTypeName;
        //            childPBS.SysCode = pbs.SysCode;
        //            childPBS.CreateDate = DateTime.Now;
        //            childPBS.ParentNode = parentNode;
        //            //parentNode.ChildNodes.Add(childPBS);
        //            lst.Add(childPBS);
        //            CopyPBSNode(childPBS, pbs, listPBS, ref lst, ProjectInfoRight);

        //        }
        //    }
        //}

        ///// <summary>
        ///// 复制GWBS节点
        ///// </summary>
        ///// <param name="parentNode">新生成的父节点</param>
        ///// <param name="copyParentNode">复制的父节点</param>
        ///// <param name="listPBS">复制的wbs节点集</param>
        //private void CopyGWBSNode(GWBSTree parentNode, GWBSTree copyParentNode, IList listGWBS, ref IList lst)
        //{
        //    var query = from p in listGWBS.OfType<GWBSTree>()
        //                where p.ParentNode.Id == copyParentNode.Id
        //                select p;

        //    if (query.Count() > 0)
        //    {
        //        foreach (GWBSTree gwbs in query)
        //        {
        //            GWBSTree childGWBS = new GWBSTree();
        //            childGWBS.Name = gwbs.Name;
        //            childGWBS.Describe = gwbs.Describe;
        //            childGWBS.Level = gwbs.Level;

        //            //foreach (GWBSRelaPBS rela in gwbs.ListRelaPBS)
        //            //{
        //            //    GWBSRelaPBS newRela = new GWBSRelaPBS();
        //            //    newRela.ThePBS = rela.ThePBS;
        //            //    newRela.PBSName = rela.PBSName;

        //            //    newRela.TheGWBSTree = childGWBS;
        //            //    childGWBS.ListRelaPBS.Add(newRela);
        //            //}
        //            //childGWBS.NGUID = gwbs.NGUID;
        //            childGWBS.NodeType = gwbs.NodeType;
        //            childGWBS.OrderNo = gwbs.OrderNo;
        //            childGWBS.OwnerGUID = gwbs.OwnerGUID;
        //            childGWBS.OwnerName = gwbs.OwnerName;
        //            childGWBS.OwnerOrgSysCode = gwbs.OwnerOrgSysCode;
        //            //childGWBS.State = gwbs.State;
        //            //childGWBS.SubContractFeeFlag = gwbs.SubContractFeeFlag;
        //            //childGWBS.Summary = gwbs.Summary;
        //            //childGWBS.SysCode = gwbs.SysCode;
        //            //childGWBS.TaskPlanEndTime = gwbs.TaskPlanEndTime;
        //            //childGWBS.TaskPlanStartTime = gwbs.TaskPlanStartTime;
        //            //childGWBS.TaskState = gwbs.TaskState;
        //            //childGWBS.TaskStateTime = gwbs.TaskStateTime;
        //            childGWBS.TheProjectGUID = ProjectInfoRight.Id;
        //            childGWBS.TheProjectName = ProjectInfoRight.Name;
        //            childGWBS.TheTree = gwbs.TheTree;
        //            //childGWBS.PlanTotalPrice = gwbs.PlanTotalPrice;
        //            //childGWBS.PriceAmountUnitGUID = gwbs.PriceAmountUnitGUID;
        //            //childGWBS.PriceAmountUnitName = gwbs.PriceAmountUnitName;
        //            //childGWBS.ProductConfirmFlag = gwbs.ProductConfirmFlag;
        //            childGWBS.ProjectTaskTypeGUID = gwbs.ProjectTaskTypeGUID;
        //            childGWBS.ProjectTaskTypeName = gwbs.ProjectTaskTypeName;
        //            //childGWBS.ResponsibilityTotalPrice = gwbs.ResponsibilityTotalPrice;
        //            //childGWBS.ResponsibleAccFlag = gwbs.ResponsibleAccFlag;
        //            if (ProjectInfoRight != null)
        //            {
        //                childGWBS.Code = ProjectInfoRight.Code.PadLeft(4, '0') + "-" + modelWBS.GetCode(typeof(GWBSTree));
        //            }
        //            //childGWBS.ParentNode = parentNode;
        //            //parentNode.ChildNodes.Add(childGWBS);
        //            lst.Add(childGWBS);
        //            CopyGWBSNode(childGWBS, gwbs, listGWBS, ref lst);
        //        }
        //    }
        //}

      
    }
}
