using System;
using System.Collections;
using System.Windows.Forms;


namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public static class TreeViewUtil
    {
        /// <summary> �жϵ�һ���ڵ��ܷ��Ƶ��ڶ����ڵ�
        /// </summary>
        public static bool CanMoveNode(TreeNode node1, TreeNode node2)
        {
            //�����Ƶ��Լ�
            if ( node1.Equals(node2) )
                return false;
            //���ڵ㲻�����ƶ�
            if ( node1.Parent == null )
                return false;
            //�����Ƶ�һ���սڵ���
            if ( node2 == null )
                return false;
            return !ContainsNode(node1, node2);
        }

        /// <summary> �жϵ�һ���ڵ��Ƿ�Ϊ�ڶ����ڵ�ĸ��ڵ�
        /// </summary>
        public static bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if ( node2.Parent == null )
                return false;
            //���ڵ㲻���Ƶ��ӽڵ�
            if ( node2.Parent.Equals(node1) )
                return true;
            return ContainsNode(node1, node2.Parent);
        }

        /// <summary> ���ýڵ�������ӽڵ���ϲ㸸�ڵ�checkbox�Ƿ�ѡ��
        /// </summary>
        public static void setNodeCheck(TreeNode tn)
        {
            SetChildCheck(tn);
            SetParentCheck(tn);
        }

        /// <summary> ���ýڵ�������ӽڵ�͸ýڵ�checkbox״̬һ��
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

        /// <summary> ���ýڵ�������ϲ㸸�ڵ�checkbox�Ƿ�ѡ��
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
