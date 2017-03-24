using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    public partial class VAccountTitleTreeSelect : Application.Business.Erp.Financial.Client.Basic.CommonClass.BaseForm
    {
        private IAccountTitleTreeSvr accountTitleTreeSvr;
        public IAccountTitleTreeSvr AccountTitleTreeSvr
        {
            get
            {
                if (accountTitleTreeSvr == null)
                {
                    accountTitleTreeSvr = StaticMethod.GetService("AccountTitleTreeSvr") as IAccountTitleTreeSvr;
                }
                return accountTitleTreeSvr;
            }
        }
        public string _RootCode = "";
        public  IList lstObjects=new ArrayList();
 
        public AccountTitleTree SelectedTitle = null;
        public bool IsAllowMulti = false;//多科目选择
        public string SelectedTitleCodes = "";
        public bool OnlySelectLeafNode = false;//只允许选择末级科目 
        private List<AccountTitleTree> selectNode = new List<AccountTitleTree>();
        public VAccountTitleTreeSelect()
        {

            InitializeComponent();
            this.Load += new EventHandler(VAccTitleSelect_Load);
           
           
            //this.tvTitle.AfterSelect +=new TreeViewEventHandler(tvTitle_AfterSelect);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            
            this.tvTitle.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvTitle_NodeMouseDoubleClick);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            //this.tvTitle.AfterCheck += new TreeViewEventHandler(tvTitle_AfterCheck);
        }
        public VAccountTitleTreeSelect(string sRootCode):this()
        {
            this._RootCode = sRootCode;
        }
        void tvTitle_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode tnCurr = e.Node;
            //SetChildNodeCheck(tnCurr, tnCurr.Checked);
        }

        private void SetChildNodeCheck(TreeNode tnParent, bool check)
        {
            foreach (TreeNode tn in tnParent.Nodes)
            {
                if (tn.Nodes.Count != 0)
                {
                    SetChildNodeCheck(tn, check);
                }
                tn.Checked = check;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        void tvTitle_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (OnlySelectLeafNode && e.Node.Nodes.Count != 0)
            {
                StaticMethods.ShowErrorMessage("请选择末级会计科目");
                return;
            }
            if (!this.IsAllowMulti)
            {
                this.SelectedTitle = this.tvTitle.SelectedNode.Tag as AccountTitleTree;
                SelectedNodes.Add(this.SelectedTitle);
                this.Close();
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.IsAllowMulti)
            {
                if (OnlySelectLeafNode && this.tvTitle.SelectedNode.Nodes.Count != 0)
                {
                    StaticMethods.ShowErrorMessage("请选择末级会计科目");
                    return;
                }
                this.SelectedTitle = this.tvTitle.SelectedNode.Tag as AccountTitleTree;
                SelectedNodes.Add(this.tvTitle.SelectedNode.Tag as AccountTitleTree);
              
            }
            else
            {
                foreach (TreeNode tn in tvTitle.Nodes)
                {
                    if (tn.Checked)
                    {
                        this.SelectedTitleCodes += "," + (tn.Tag as AccountTitleTree).Code;
                        SelectedNodes.Insert(SelectedNodes.Count,tn.Tag as AccountTitleTree);
                    }
                    if (tn.Nodes.Count != 0)
                        GetChildNodes(tn, ref this.SelectedTitleCodes);
                }
                if (!this.SelectedTitleCodes.Trim().Equals(""))
                {
                    SelectedTitleCodes = SelectedTitleCodes.Substring(1);
                }
            }
            this.Close();
        }


        private void GetChildNodes(TreeNode tnParent, ref string SelectedTitleCodes)
        {
            foreach (TreeNode tn in tnParent.Nodes)
            {
               // if (tn.Checked)
               // {
                    if (tn.Checked)
                    {
                        SelectedTitleCodes += "," + (tn.Tag as AccountTitleTree).Code;
                        SelectedNodes.Insert(SelectedNodes.Count,tn.Tag as AccountTitleTree);
                    }
                    if(tn.Nodes.Count != 0)
                        GetChildNodes(tn, ref SelectedTitleCodes);
                }
           // }
        }


        
        void VAccTitleSelect_Load(object sender, EventArgs e)
        {
            if (lstTitles == null)
            {
                InitData();
            }
            BindAccTree();
        }

        void InitData()
        {
            if (IsAllowMulti) this.tvTitle.CheckBoxes = true;

                lstTitles = AccountTitleTreeSvr.GetAccountTitleTreeByInstance(_RootCode);

            
        }


        IList lstTitles = null;
      
      

        /// <summary>
        /// 绑定所有科目
        /// </summary>
        void BindAccTree()
        {
            Hashtable hashtable = new Hashtable();
            int count = 0;
            try
            {
                this.tvTitle.Nodes.Clear();
                AccountTitleTree childNode = null;
                for (int i = 0; i < lstTitles.Count; i++)
                {
                    childNode = lstTitles[i] as AccountTitleTree;
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;
                    if (childNode.ParentNode == null || i == 0)
                    {
                        this.tvTitle.Nodes.Add(tnTmp);
                    }
                    else
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                            tnp.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);

                }
                    //foreach (AccountTitleTree childNode in lstTitles)
                    //{
                    //    TreeNode tnTmp = new TreeNode();
                    //    tnTmp.Name = childNode.Id.ToString();
                    //    tnTmp.Text = childNode.Name;
                    //    tnTmp.Tag = childNode;
                    //    if (childNode.ParentNode != null)
                    //    {
                    //        TreeNode tnp = null;
                    //        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                    //        if (tnp != null)
                    //            tnp.Nodes.Add(tnTmp);
                    //    }
                    //    else
                    //    {
                    //        this.tvTitle.Nodes.Add(tnTmp);
                    //    }
                    //    hashtable.Add(tnTmp.Name, tnTmp);
                    //} 
            }
            catch (Exception exp)
            {
                Console.WriteLine(count);
                StaticMethods.AppExceptionHandle(exp, "101");
            }
        }
        public List<AccountTitleTree> SelectedNodes
        {
            get { return selectNode; }
        }

       
 


        
        ArrayList lstFindNodes = new ArrayList();

        
    }
}
