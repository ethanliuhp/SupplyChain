using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonForm
{
    public enum enmMoveOrCopy
    {
        copy=0,
        move=1
    }

    public partial class frmTreeMoveCopy : Form
    {
        private TreeNode draggedNode;
        /// <summary>
        /// 源节点
        /// </summary>
        public TreeNode DraggedNode
        {
            get { return draggedNode; }
            set { draggedNode = value; }
        }
        private TreeNode targetNode;
        /// <summary>
        /// 目标节点
        /// </summary>
        public TreeNode TargetNode
        {
            get { return targetNode; }
            set { targetNode = value; }
        }
        private int nodeDept;

        private bool isOK = false;
        /// <summary>
        /// 是否OK
        /// </summary>
        public bool IsOK
        {
            get { return isOK; }
            set { isOK = value; }
        }
        private enmMoveOrCopy moveOrCopy = 0;
        /// <summary>
        /// 移动或拷贝
        /// </summary>
        public enmMoveOrCopy MoveOrCopy
        {
            get
            {
                if (rdoCopy.Checked == true)
                {
                    moveOrCopy = enmMoveOrCopy.copy;
                }
                else if (rdoMove.Checked == true)
                {
                    moveOrCopy = enmMoveOrCopy.move;
                }
                return moveOrCopy;
            }
            set
            {
                moveOrCopy = value;
                if (moveOrCopy ==enmMoveOrCopy.copy)
                {
                    //rdoCopy.Checked = true;
                }
                else if (moveOrCopy == enmMoveOrCopy.move)
                {
                    //rdoMove.Checked = true;
                }
            }
        }

        public frmTreeMoveCopy()
        {
            InitializeComponent();
            this.InitEvent();
        }
        private void InitEvent()
        {
            this.txtNodeDept.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
        }

        void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            switch (textBox.Name)
            {
                case "txtNodeDept":
                    if (e.KeyChar != '\b')
                        if ("0123456789".IndexOf(e.KeyChar) == -1)
                            e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception exc;
            try
            {
                if (Convert.ToInt32(this.txtNodeDept.Text) <= 0)
                {
                    exc = new Exception("深度不能小于或等于0!");
                    throw exc;
                }
                if (Convert.ToInt32(this.txtNodeDept.Text) > this.nodeDept)
                {
                    exc = new Exception("深度不能大于" + this.nodeDept.ToString() + "!");
                    throw exc;
                }
                //移动节点
                if ( this.moveOrCopy == enmMoveOrCopy.move )
                {
                    //目标节点如果为被拖动节点的父节点，不做任何操作
                    if ( targetNode.Equals(draggedNode.Parent) )
                    {
                        this.Close();
                        return;
                    }
                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);
                }
                //复制源节点到目标节点下
                else
                {
                    //从叶节点删除
                    TreeNode draggedNodeTmp = new TreeNode();
                    TreeNode tn = new TreeNode();
                    draggedNodeTmp = draggedNode.Clone() as TreeNode;

                    int copyCount = 0;
                    copyCount = Convert.ToInt32(this.txtNodeDept.Text);
                    int i = 1;

                    //如果1,只是拷贝当前节点
                    if (copyCount == 1)
                        draggedNodeTmp.Nodes.Clear();
                    else
                        CopyTreeNodeByDept(draggedNodeTmp, copyCount - 1, ref i);


                    //拷贝
                   targetNode.Nodes.Add(draggedNodeTmp);
                   draggedNode = draggedNodeTmp;
                }
                //展开节点
                targetNode.Expand();

                this.nodeDept = Convert.ToInt32(this.txtNodeDept.Text);
                this.isOK = true;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        //按深度拷贝节点
        private void CopyTreeNodeByDept(TreeNode sourceNode, int depth, ref int i)
        {
            foreach (TreeNode node in sourceNode.Nodes)
            {
                if (node.Nodes.Count == 0)
                {
                    i = 1;
                    continue;
                }
                else if (depth == i)
                {
                    node.Nodes.Clear();
                    i = 1;
                    continue;
                }
                else
                {
                    i++;
                    CopyTreeNodeByDept(node, depth, ref i);
                }
            }
        }

        //获得节点的最大的层数
        private int GetTreeNodeMaxLevel(TreeNode sourceNode, ref int i, IList lst)
        {
            if (sourceNode.Nodes.Count == 0)
            {
                lst.Add(i);
                i++;
            }
            else
            {
                i++;
                foreach (TreeNode node in sourceNode.Nodes)
                {
                    GetTreeNodeMaxLevel(node, ref i, lst);
                }
            }
            //排序
            int tmp = 0;
            //递归返回,i依次减少
            if (i > 1)
                i--;
            if (lst.Count > 0 && i == 1)
            {
                tmp = Convert.ToInt32(lst[0]);
                for (int ii = 0; ii < lst.Count; ii++)
                {
                    tmp = Math.Max(Convert.ToInt32(lst[ii]), tmp);
                }
            }
            return tmp;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.isOK = false;
            this.Close();
        }

        private void rdoCopy_Click(object sender, EventArgs e)
        {
            if (rdoCopy.Checked == true)
            {
                this.moveOrCopy = 0;
            }
        }

        private void rdoMove_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMove.Checked == true)
            {
                this.moveOrCopy = enmMoveOrCopy.move;
            }
        }
        private void frmTreeMoveCopy_Load(object sender, EventArgs e)
        {
            int i = 1;
            IList lst = new ArrayList();
            this.txtNodeDept.Text = GetTreeNodeMaxLevel(this.draggedNode, ref i, lst).ToString();
            nodeDept = Convert.ToInt32(this.txtNodeDept.Text.ToString());
            if (this.moveOrCopy == enmMoveOrCopy.copy)
            {
                this.rdoCopy.Checked = true;
            }
            else
                this.rdoMove.Checked = true;
            
        }
    }
}