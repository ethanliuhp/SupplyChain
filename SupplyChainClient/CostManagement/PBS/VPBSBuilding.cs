using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Core;
//using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.Main;
using NHibernate.Criterion;
using System.Threading;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSBuilding : TBasicDataView
    {
        #region ���ԡ��ֶ�

        /// <summary>
        /// �ṩ����Ľӿ�
        /// </summary>
        private IPBSTreeSrv service;
        /// <summary>
        /// ��ʶ�Ƿ�����Ҫ����PBSģ��
        /// </summary>
        private bool _isNewLoad = false;
        /// <summary>
        /// ��ǰ��½�����ڵ���Ŀ��Ϣ
        /// </summary>
        public CurrentProjectInfo projectInfo { get; set; }
        /// <summary>
        /// ��ǰ��ѡ���pbs�ڵ�
        /// </summary>
        public PBSTree SelectPbs { get; set; }
        public List<TreeNode> ListChecked { get; set; }

        #endregion

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="mot"></param>
        public VPBSBuilding(MPBSTree mot)
        {
            service = mot.Service;
            ListChecked = new List<TreeNode>();
            InitializeComponent();
            ViewInit();

            EventInit();
            // չ����һ���ڵ�
            if (tvTree.Nodes.Count > 0)
                tvTree.Nodes[0].Expand();
        }
        public void ReName()
        {
            // else if (e.ClickedItem.Name == �������ʩ����������.Name)
            // {
            // mnuTree.Hide();
            VPBSTreeBakNewBatchUpdateName_New oVPBSTreeBakNewBatchUpdateName_New = new VPBSTreeBakNewBatchUpdateName_New(this.tvTree.SelectedNode.Tag as PBSTree, new MPBSTree());
            oVPBSTreeBakNewBatchUpdateName_New.ShowDialog();
            if (oVPBSTreeBakNewBatchUpdateName_New.IsSave)
            {
                tvTree.Nodes.Clear();
                LoadTreeNode();
            }
            // }
        }
        #region ҳ���ʼ��
        /*
         * ҳ���ʼ������
         *  1. ���������ͼʱ����ȡ��Ŀ��PBS������������������ص�TreeView�ϣ�������ʾѡ��PBSģ��
         *  2. ѡ��ģ�������PBS�ڵ����ĸ��ڵ㣬�����浽���ݿ�
         *  3. ���¼�����
        */

        /// <summary>
        /// ҳ���ʼ��
        /// </summary>
        private void ViewInit()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            // �����ж��Ƿ����PBS�ڵ㣬����������ʾѡ��ģ������
            DataTable dt = service.GetPBSTreesInstanceBySql(projectInfo.Id);
            if (dt == null || dt.Rows.Count == 0)
            {
                _isNewLoad = true;
                return;
            }
            // ����PBS�ڵ���
            LoadTreeNode();
        }

        /// <summary>
        /// ����PBS�ڵ���
        /// </summary>
        private void LoadTreeNode()
        {
            // ��ѯ���нڵ�
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddOrder(Order.Asc("OrderNo"));
            oq.AddFetchMode("Template", NHibernate.FetchMode.Eager);
            var list = service.ObjectQuery(typeof(PBSTree), oq).Select<PBSTree>();
            if (list == null || list.Count == 0) return;
            // ���ȼ��ظ��ڵ�
            var pbsRoot = list.FirstOrDefault(a => a.Level == 1);
            if (pbsRoot == null) return;
            var root = CreateTreeNode(pbsRoot);
            tvTree.Nodes.Add(root);
            // Ȼ��ݹ�
            CreateTreeNode(root, list);
        }

        /// <summary>
        /// ���������ӽڵ�
        /// </summary>
        /// <param name="root">�������ӽڵ�Ľڵ�</param>
        /// <param name="list">��ĿPBS�ڵ�</param>
        private void CreateTreeNode(TreeNode root, IEnumerable<PBSTree> list)
        {
            /*
             * �����ӽڵ�����
             *  1. ��ѯ�����Ե��ӽڵ�
             *  2. �����ӽڵ㣬�������ӵ����ڵ���
             *  3. �����ӽڵ�����ԣ��ݹ��������ӽڵ�
            */
            var parent = root.Tag as PBSTree;
            var chilren = list.Where(a => a.ParentNode != null && a.ParentNode.Id == parent.Id);
            if (chilren.Count() == 0) return;
            foreach (var item in chilren)
            {
                var node = CreateTreeNode(item);
                root.Nodes.Add(node);
                CreateTreeNode(node, list);
            }
        }

        /// <summary>
        /// ѡ��PBSģ��
        /// </summary>
        public void SelectPBSTemplate()
        {
            if (!_isNewLoad) return;                // ֻ�в�����PBS�ڵ�ʱ����ʾѡ��ģ��
            tvTree.Nodes.Clear();                   // ���ԭ�нڵ�
            VPBSTemplateSelect vts = new VPBSTemplateSelect() { StartPosition = FormStartPosition.Manual, Location = new Point(200, 100) };
            vts.ShowDialog();
            if (vts.SelectItem == null) return;     // û��ѡ��ģ���򲻽���֮��Ĳ���
            // ���ɸ��ڵ㲢����
            PBSTree root = CreatePBSTree();
            root.Name = projectInfo.Name;
            root.FullPath = projectInfo.Name;
            root.Template = vts.SelectItem;
            root.CodeBit = Convert.ToInt32(vts.SelectItem.TypeBit);
            root.Code = vts.SelectItem.Code;
            root.OrderNo = 1;
            root.Level = 1;
            root.StructTypeGUID = root.Template.Id;
            root.StructTypeName = root.Template.Name;
            // ����
            service.SavePBSTree(root, null);
            // ���¼�����
            LoadTreeNode();
        }

        #endregion

        #region ҳ�����

        private void EventInit()
        {
            tvTree.AfterSelect += (a, b) => { ShowNodeDetail(b.Node); ChangeControlState(false); };
            tvTree.AfterCheck += (a, b) => CheckTreeNode(b);
            btnAdd.Click += (a, b) =>
            {
                if (tvTree.Nodes.Count == 0) return;
                Add(tvTree.SelectedNode);
                ChangeControlState(false);
            };
            btnDelete.Click += (a, b) => Delete(tvTree.SelectedNode);
            btnUpdate.Click += (a, b) =>
            {
                if (tvTree.Nodes.Count == 0) return;
                ChangeControlState(true);
            };
            btnSave.Click += (a, b) => { Save(tvTree.SelectedNode); ChangeControlState(false); };
            btnDelAll.Click += (a, b) =>
            {
                if (tvTree.Nodes.Count == 0) return;
                DeleteAll();
            };
            btnReset.Click += (a, b) => Reset();
            btnRename.Click += (a, b) => ReName();

            ����.Click += (a, b) => Move(true);
            ����.Click += (a, b) => Move(false);
        }

        /// <summary>
        /// ѡ��ڵ��չʾ�ڵ������
        /// </summary>
        /// <param name="node"></param>
        private void ShowNodeDetail(TreeNode node)
        {
            var pbs = node.Tag as PBSTree;
            txtPath.Text = pbs.FullPath;
            txtCode.Text = pbs.Code;
            txtName.Text = pbs.Name;
            txtConstructionArea.Text = ClientUtil.ToString(pbs.ConstructionArea);
            if (pbs.Template != null)
            {
                txtType.Text = pbs.Template.Name;
            }
            txtRemark.Text = pbs.Describe;
            SelectPbs = pbs;
        }

        /// <summary>
        /// ���ӽڵ�
        /// </summary>
        private void Add(TreeNode node)
        {
            var pbs = node.Tag as PBSTree;
            VPBSNodeAdd vna = new VPBSNodeAdd(pbs.Template);
            if (!vna.EnableSelect)
            {
                MessageBox.Show("ѡ��Ľڵ��²��������ӽڵ�...");
                return;
            }
            vna.ShowDialog();
            if (vna.NodeTemplate == null) return;
            // ����ѡ�����Ϣ�����ӽڵ�
            CreateChildren(tvTree.SelectedNode, vna.NodeTemplate, vna.NodeName, vna.NodeCode, vna.Number, vna.NodeRemark);
            node.Expand();
        }

        /// <summary>
        /// �����ӽڵ�
        /// </summary>
        private void CreateChildren(TreeNode root, PBSTemplate template, string name, string code, int number, string remark)
        {
            /*
             * �����ӽڵ������
             *  1. ����PBS�ڵ�[���number��Ϊ1������ÿ���ӽڵ����Ƽ���������ӽڵ���]
             *  2. ����PBS�ڵ�
             *  3. ����TreeView�ڵ�
             
            */
            var parentNode = root.Tag as PBSTree;
            var allChildren = new List<PBSTree>();

            // �������Գ�ʼ��
            var pbs = CreatePBSTree();
            pbs.Code = parentNode.Code + "-" + code;
            pbs.Name = name;
            pbs.Describe = remark;
            pbs.Template = template;
            pbs.CodeBit = Convert.ToInt32(template.TypeBit);
            pbs.ParentNode = parentNode;
            pbs.StructTypeGUID = template.Id;
            pbs.StructTypeName = template.Name;
            pbs.Level = parentNode.Level + 1;

            // ��ǰ�ڵ��µ��ӽڵ���
            int childCount = root.Nodes.Count;

            // ͬ���ͽڵ���
            int order = 0;
            if (childCount > 0)
                order = root.Nodes.Select<TreeNode>().Where(a => (a.Tag as PBSTree).Template.Id == template.Id).Count();

            if (number == 1 && order == 0)
            {
                pbs.OrderNo = order + 1;
                pbs.FullPath = parentNode.FullPath + "\\" + name;
                pbs = service.SavePBSTree(pbs, parentNode.SysCode);
                allChildren.Add(pbs);
            }
            else
            {
                for (int i = 1; i <= number; i++)
                {
                    var child = pbs.Clone(null) as PBSTree;
                    child.OrderNo = order + i;
                    child.Name += (order + i).ToString().PadLeft(pbs.CodeBit, '0');
                    child.FullPath = parentNode.FullPath + "\\" + child.Name;
                    child.Code += (order + i).ToString().PadLeft(pbs.CodeBit, '0');
                    child = service.SavePBSTree(child, parentNode.SysCode);
                    allChildren.Add(child);
                }
            }

            // ������
            foreach (var item in allChildren)
                root.Nodes.Add(CreateTreeNode(item));
        }

        /// <summary>
        /// ɾ���ڵ�
        /// </summary>
        private void Delete(TreeNode node)
        {
            if (node == tvTree.Nodes[0])
            {
                MessageBox.Show("����ɾ�����ڵ�...");
                return;
            }
            var result = MessageBox.Show("ȷ��ɾ���ڵ���", "��ʾ", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;

            var pbs = node.Tag as PBSTree;
            CommonMethod.CommonMethodSrv.InsertData(string.Format("delete thd_pbstree where theprojectguid='{0}' and state = '1' and syscode like '{1}%'", projectInfo.Id, pbs.SysCode));
            tvTree.Nodes.Remove(node);
        }

        /// <summary>
        /// �޸Ľڵ�
        /// </summary>
        private void Save(TreeNode node)
        {
            var pbs = node.Tag as PBSTree;
            pbs.Code = txtCode.Text;
            pbs.Name = txtName.Text;
            int index = pbs.FullPath.LastIndexOf("\\");
            if (index == -1)
                pbs.FullPath = txtName.Text;
            else
                pbs.FullPath = pbs.FullPath.Substring(0, index) + "\\" + txtName.Text;
            pbs.Describe = txtRemark.Text;
            pbs.ConstructionArea = ClientUtil.ToDecimal(txtConstructionArea.Text);
            txtPath.Text = pbs.FullPath;
            service.Save(pbs);
            node.Text = pbs.Name;
        }

        /// <summary>
        /// ��ѡ���ڵ�
        /// </summary>
        /// <param name="note"></param>
        private void CheckTreeNode(TreeViewEventArgs b)
        {
            if (b.Action != TreeViewAction.Unknown)         // ����Ǵ��븳ֵ�������¼�����ִ��CheckNode
                CheckNode(b.Node, b.Node.Checked);

            if (!b.Node.Checked && b.Node.Parent != null && b.Node.Parent.Checked)
            {
                // ���ȡ���˹�ѡ���Ҹ���Ҳ�㹴ѡ�ˣ���ȡ�����ڵ�Ĺ�ѡ
                SetCheckList(b.Node.Parent, false);
            }
        }

        /// <summary>
        /// ��깴ѡʱִ�еĲ���
        /// </summary>
        private void CheckNode(TreeNode node, bool state)
        {
            SetCheckList(node, state);

            // �����ڵ��µ��ӽڵ�
            var children = node.Nodes;
            if (children.Count == 0) return;
            foreach (TreeNode item in children)
            {
                CheckNode(item, state);
            }
        }

        /// <summary>
        /// ���ýڵ㹴ѡ״̬�����ڵ㼯��
        /// </summary>
        /// <param name="node"></param>
        /// <param name="state"></param>
        private void SetCheckList(TreeNode node, bool state)
        {
            if (state && !ListChecked.Contains(node))
                ListChecked.Add(node);
            else if (!state && ListChecked.Contains(node))
                ListChecked.Remove(node);
            node.Checked = state;
        }

        /// <summary>
        /// ɾ��ѡ�еĽڵ�
        /// </summary>
        private void DeleteAll()
        {
            Delete(false);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="state"></param>
        private void Delete(bool state)
        {
            /*
             * ����ɾ����������
             *  1. �ж�ѡ�нڵ㣬����ڵ����ӽڵ㣬����ʾ�û�
             *  2. ��ѡ�еĽڵ㰴����μ���Level����
             *  3. ����ɾ������ڵ�
            */
            if (!state)         // ������ж���Ϊ�������õ�ʱ��ȡ����ʾ���ϵ�
            {
                if (ListChecked.Count == 0)
                {
                    MessageBox.Show("û��ѡ�еĽڵ�...");
                    return;
                }
                if (MessageBox.Show("ȷ��ɾ��ѡ�нڵ���", "��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            // ���ɾ��
            var levelGroup = ListChecked.GroupBy(a => a.Level).Select(a => a.Key).ToList();
            levelGroup.Sort();          // ����
            levelGroup.Reverse();       // ����
            foreach (var level in levelGroup)
            {
                var list = ListChecked.Where(a => a.Level == level).Select(a => a.Tag as PBSTree);
                Delete(list.ToArray());
            }
            foreach (var item in ListChecked)
            {
                item.Remove();
            }
            ListChecked.Clear();
        }

        /// <summary>
        /// ɾ���б�����
        /// </summary>
        /// <param name="list"></param>
        private void Delete(params PBSTree[] list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count(); i++)
            {
                if (i == list.Count() - 1)
                {
                    sb.Append("'" + list[i].Id + "'");
                    continue;
                }
                sb.Append("'" + list[i].Id + "',");
            }
            CommonMethod.CommonMethodSrv.InsertData(string.Format("delete thd_pbstree where theprojectguid = '{0}' and state = '1' and id in ({1})", projectInfo.Id, sb.ToString()));
        }

        private void Reset()
        {
            if (tvTree.Nodes.Count > 0)
            {
                if (MessageBox.Show("����ѡ��ɾ�����нṹ��ȷ������ѡ����", "��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            /*
             * ��������
             * 1. ɾ�����нṹ
             * 2. ѡ��PBSģ��
            
            */
            // ѡ�����нڵ�
            if (tvTree.Nodes.Count > 0)
            {
                tvTree.Nodes[0].Checked = true;
                CheckTreeNode(new TreeViewEventArgs(tvTree.Nodes[0], TreeViewAction.ByMouse));
                Delete(true);        // ɾ�����нڵ�
            }
            // ����ѡ��
            _isNewLoad = true;
            SelectPBSTemplate();
        }

        private void Move(bool isMoveUp)
        {
            #region �ϴ���
            //List<PBSTree> lstPbsUpdate = new List<PBSTree>();
            //PBSTree curPBS = tvTree.SelectedNode.Tag as PBSTree;
            //TreeNode curNode = tvTree.SelectedNode;//��ǰ�ڵ�
            //int curr_index = curNode.Index;//��ǰ����λ��
            //bool ifCheck = curNode.Checked;
            //TreeNode parentNode = curNode.Parent;//���ڵ�
            //if (parentNode != null)
            //{
            //    int new_index = 0;//ǰһ����
            //    //����
            //    if (isMoveUp)
            //    {
            //        if (curr_index > 0)
            //        {
            //            new_index = curr_index - 1;
            //        }

            //        if (curNode.PrevNode != null && curNode.PrevNode.Tag != null)
            //        {
            //            var preTree = curNode.PrevNode.Tag as PBSTree;
            //            ////�����ȣ��򽫽���ǰ�ڵ����ż�һ���Լ���һ���ڵ�֮��������ֵܽڵ����ż�һ,����ֱ�ӽ������ߵ���ż���
            //            if (preTree.OrderNo == curPBS.OrderNo)
            //            {
            //                curPBS.OrderNo = curPBS.OrderNo - 1;
            //                if (curNode.PrevNode.PrevNode != null && curNode.PrevNode.PrevNode.Tag != null)
            //                {
            //                    SetOrderUp(lstPbsUpdate, curNode.PrevNode.PrevNode);
            //                }
            //            }
            //            else
            //            {
            //                var temp = curPBS.OrderNo;
            //                curPBS.OrderNo = preTree.OrderNo;
            //                preTree.OrderNo = temp;

            //                curNode.PrevNode.Tag = preTree;
            //                lstPbsUpdate.Add(preTree);
            //            }
            //        }
            //    }
            //    else//����
            //    {
            //        new_index = curr_index + 1;

            //        if (curNode.NextNode != null && curNode.NextNode.Tag != null)
            //        {
            //            var nextTree = curNode.NextNode.Tag as PBSTree;
            //            //�����ȣ��򽫽���ǰ�ڵ����ż�һ���Լ���һ���ڵ�֮��������ֵܽڵ����ż�һ,����ֱ�ӽ������ߵ���ż���
            //            if ((nextTree.OrderNo == curPBS.OrderNo))
            //            {
            //                curPBS.OrderNo = curPBS.OrderNo + 1;
            //                if (curNode.NextNode.NextNode != null && curNode.NextNode.NextNode.Tag != null)
            //                {
            //                    SetOrderDown(lstPbsUpdate, curNode.NextNode.NextNode);
            //                }
            //            }
            //            else
            //            {
            //                var temp = curPBS.OrderNo;
            //                curPBS.OrderNo = nextTree.OrderNo;
            //                nextTree.OrderNo = temp;

            //                curNode.NextNode.Tag = nextTree;
            //                lstPbsUpdate.Add(nextTree);
            //            }
            //        }
            //    }

            //    //�����½ڵ�
            //    TreeNode tn_new = new TreeNode();
            //    tn_new.Name = curPBS.Id.ToString();
            //    tn_new.Text = curPBS.Name;
            //    tn_new.Tag = curPBS;
            //    tn_new.Checked = ifCheck;
            //    if (curNode.IsExpanded)
            //    {
            //        tn_new.Expand();
            //    }
            //    else
            //    {
            //        tn_new.Collapse();
            //    }
            //    curNode.Remove();
            //    CreateNewTreeNodes(tn_new, curNode.Nodes);

            //    parentNode.Nodes.Insert(new_index, tn_new);//����ڵ�
            //    tvTree.SelectedNode = tn_new;
            //    lstPbsUpdate.Add(curPBS);
            //    service.Save((IList)lstPbsUpdate);
            //}
            #endregion

            #region �·�ʽ
            List<PBSTree> lstPbsUpdate = new List<PBSTree>();
            TreeNode curNode = tvTree.SelectedNode;//��ǰ�ڵ�
            PBSTree curTree  = tvTree.SelectedNode.Tag as PBSTree;
            int curr_index = curNode.Index;//��ǰ����λ��
            bool ifCheck = curNode.Checked;
            TreeNode parentNode = curNode.Parent;//���ڵ�
            int new_index = 0;//�½ڵ�����
            if (parentNode != null)
            {
                //���ƣ���������ƣ�������һ���ڵ�Ե�index
                if (isMoveUp)
                {
                    new_index = curr_index - 1;
                }
                else//���ƣ���������ƣ�������һ���ڵ�Ե�index
                {
                    new_index = curr_index + 1;
                }
                curTree.OrderNo = new_index;
                //�����½ڵ�
                TreeNode tn_new = new TreeNode();
                tn_new.Name = curTree.Id.ToString();
                tn_new.Text = curTree.Name;
                tn_new.Tag = curTree;
                tn_new.Checked = ifCheck;
                tn_new.Tag = curTree;
                if (curNode.IsExpanded)
                {
                    tn_new.Expand();
                }
                else
                {
                    tn_new.Collapse();
                }
                curNode.Remove();
                CreateNewTreeNodes(tn_new, curNode.Nodes);
                parentNode.Nodes.Insert(new_index, tn_new);//����ڵ�
                tvTree.SelectedNode = tn_new;
                foreach (TreeNode itemNode in parentNode.Nodes)
                {
                    if (itemNode != null && itemNode.Tag != null)
                    {
                        var treeModel = itemNode.Tag as PBSTree;
                        treeModel.OrderNo = itemNode.Index;
                        lstPbsUpdate.Add(treeModel);
                    }
                }
                if (lstPbsUpdate != null && lstPbsUpdate.Count > 0)
                {
                    try
                    {
                        service.Save((IList)lstPbsUpdate);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("���������쳣��" + ExceptionUtil.ExceptionMessage(ex));
                    }
                }
            }
            #endregion
        }

        private void SetOrderUp(List<PBSTree> lst, TreeNode tn)
        {
            PBSTree curPBS = tn.Tag as PBSTree;
            curPBS.OrderNo = curPBS.OrderNo - 1;
            tn.Tag = curPBS;
            lst.Add(curPBS);
            if (tn.PrevNode != null && tn.PrevNode.Tag != null)
            {
                TreeNode preNode = tn.PrevNode;
                PBSTree prePBS = preNode.Tag as PBSTree;

                if (curPBS.OrderNo == prePBS.OrderNo)
                {
                    SetOrderUp(lst, preNode);
                }
            }
        }

        private void SetOrderDown(List<PBSTree> lst, TreeNode tn)
        {
            PBSTree curPBS = tn.Tag as PBSTree;
            curPBS.OrderNo = curPBS.OrderNo + 1;
            tn.Tag = curPBS;
            lst.Add(curPBS);
            if (tn.NextNode != null && tn.NextNode.Tag != null)
            {
                TreeNode preNode = tn.NextNode;
                PBSTree prePBS = preNode.Tag as PBSTree;

                if (curPBS.OrderNo == prePBS.OrderNo)
                {
                    SetOrderDown(lst, preNode);
                }
            }
        }

        private void CreateNewTreeNodes(TreeNode node_new, TreeNodeCollection nodes)
        {
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }
            foreach (TreeNode item in nodes)
            {
                node_new.Nodes.Add(item);
            }
        }
        #endregion

        #region ���ܺ���

        /// <summary>
        /// ����PBS�����������ڵ�
        /// </summary>
        /// <param name="pbs"></param>
        private TreeNode CreateTreeNode(PBSTree pbs)
        {
            return new TreeNode() { Tag = pbs, Text = pbs.Name };
        }

        /// <summary>
        /// �����ڵ����
        /// </summary>
        /// <returns></returns>
        private PBSTree CreatePBSTree()
        {
            return new PBSTree()
            {
                CreateDate = DateTime.Now,
                State = 1,
                UpdatedDate = DateTime.Now,
                Author = ConstObject.TheLogin.TheBusinessOperators,
                OwnerGUID = ConstObject.TheLogin.TheBusinessOperators.Id,
                OwnerName = ConstObject.TheLogin.TheBusinessOperators.PerName,
                OwnerOrgSysCode = projectInfo.OwnerOrgSysCode,
                TheProjectGUID = projectInfo.Id,
                TheProjectName = projectInfo.Name
            };
        }

        /// <summary>
        /// �޸Ŀؼ�״̬
        /// </summary>
        /// <param name="state"></param>
        private void ChangeControlState(bool state)
        {
            txtName.ReadOnly = txtRemark.ReadOnly = txtConstructionArea.ReadOnly = !state;
            btnSave.Visible = state;
        }

        /// <summary>
        /// ����PBS�ڵ㼰SysCode
        /// </summary>
        private void SavePBSTree(PBSTree pbs, string parentSysCode)
        {
            var a = service.Save(pbs);
            pbs = a as PBSTree;
            //while (string.IsNullOrEmpty(pbs.Id))
            //    Thread.Sleep(10);
            pbs.SysCode = parentSysCode + pbs.Id + ".";
            service.Save(pbs);
        }

        #endregion
    }
}