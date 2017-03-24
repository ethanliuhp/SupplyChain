using System;
using System.Collections;
using System.Windows.Forms;


namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public static class TreeViewUtil
    {
        /// <summary> 判断第一个节点能否移到第二个节点
        /// </summary>
        public static bool CanMoveNode(TreeNode node1, TreeNode node2)
        {
            //不能移到自己
            if ( node1.Equals(node2) )
                return false;
            //根节点不允许移动
            if ( node1.Parent == null )
                return false;
            //不能移到一个空节点上
            if ( node2 == null )
                return false;
            return !ContainsNode(node1, node2);
        }

        /// <summary> 判断第一个节点是否为第二个节点的父节点
        /// </summary>
        public static bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if ( node2.Parent == null )
                return false;
            //父节点不能移到子节点
            if ( node2.Parent.Equals(node1) )
                return true;
            return ContainsNode(node1, node2.Parent);
        }

        /// <summary> 设置节点的所有子节点和上层父节点checkbox是否选中
        /// </summary>
        public static void setNodeCheck(TreeNode tn)
        {
            SetChildCheck(tn);
            SetParentCheck(tn);
        }

        /// <summary> 设置节点的所有子节点和该节点checkbox状态一致
        /// </summary>
        public static void SetChildCheck(TreeNode tnParent)
        {
            foreach ( TreeNode tn in tnParent.Nodes )
            {
                tn.Checked = tnParent.Checked;
                if ( tn.Nodes.Count != 0 )
                {
                    SetChildCheck(tn);
                }
            }
        }

        /// <summary> 设置节点的所有上层父节点checkbox是否选中
        /// </summary>
        public static void SetParentCheck(TreeNode tn)
        {
            if ( tn.Parent != null && tn.Parent.Checked != tn.Checked )
            {
                if ( tn.Checked == false )
                    tn.Parent.Checked = false;
                else
                {
                    bool allCheck = true;
                    foreach ( TreeNode var in tn.Parent.Nodes )
                    {
                        if ( var.Checked == false )
                        {
                            allCheck = false;
                            break;
                        }
                    }
                    tn.Parent.Checked = allCheck;
                }
                SetParentCheck(tn.Parent);
            }
        }
    }
}
