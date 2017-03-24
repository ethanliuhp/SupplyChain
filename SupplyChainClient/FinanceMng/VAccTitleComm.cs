using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.Financial.Client.Basic.CommonClass;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Component;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;

namespace Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI
{
    public partial class VAccTitleComm : Application.Business.Erp.Financial.Client.Basic.CommonClass.BaseForm
    {
        static  IAccountTitleService titleSrv;
        public  AccountType accType=AccountType.ALL;
        public  IList lstObjects=new ArrayList();
        public  int AccLevel=0;
        public AccountTitle SelectedTitle = null;

        public VAccTitleComm()
        {
            InitializeComponent();
            this.Load += new EventHandler(VAccTitleSelect_Load);
            this.tvTitle.AfterSelect +=new TreeViewEventHandler(tvTitle_AfterSelect);
            this.tvTitle.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvTitle_NodeMouseDoubleClick);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        void tvTitle_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.SelectedTitle = this.tvTitle.SelectedNode.Tag as AccountTitle;
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.SelectedTitle = this.tvTitle.SelectedNode.Tag as AccountTitle;
            this.Close();
        }


        void VAccTitleSelect_Load(object sender, EventArgs e)
        {
            InitData();
        }

        void InitData()
        {
            if (titleSrv == null)
            {
                titleSrv = StaticMethods.GetService("AccountTitleService") as IAccountTitleService;
            }
            AccountTitle title = new AccountTitle();
            ObjectQuery query = new ObjectQuery();
            if (accType != AccountType.ALL)
                query.AddCriterion(Expression.Eq("AccType", accType));
            if (AccLevel != 0)//为零则不限制科目显示层次
                query.AddCriterion(Expression.Le("Level", 1L));
            query.AddOrder(Order.Asc("AccountCode"));
            query.AddCriterion(Expression.Le("State", 1L));
            query.AddCriterion(Expression.Le("FreezeAccount", false));
            query.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            GetAccountTitles(query);
            BindAccTree();
        }


        IList lstTitles = new ArrayList();
        /// <summary>
        /// 获取会计科目分类树
        /// </summary>
        void GetAccountTitles(ObjectQuery oq)
        {
            if (lstObjects != null && lstObjects.Count > 0)
            {
                lstTitles = lstObjects;
            }
            else
            {
                lstTitles = titleSrv.GetAccountTitles(oq);
            }
        }

        /// <summary>
        /// 绑定所有科目
        /// </summary>
        void BindAccTree()
        {
            TreeNode historySelected = null;
            int count = 0;
            try
            {
                TreeNodeUtil.dicNode.Clear();
                this.tvTitle.Nodes.Clear();
                foreach (object o in lstTitles)
                {
                    count++;
                    AccountTitle title = o as AccountTitle;

                    if (title.CategoryNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode)
                    {
                        continue;
                    }
                    TreeNode tn = new TreeNode();
                    tn.Name = title.Code;
                    tn.Text = title.Code + "  " + title.Name;
                    tn.Tag = title;
                    TreeNodeUtil.AddDic(title.SysCode, tn);
                    if (this.SelectedTitle!=null && title.Id == SelectedTitle.Id)
                    {
                        historySelected = tn;
                    }

                    if (title.ParentNode != null && title.ParentNode.CategoryNodeType != VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode)
                    {
                        TreeNode tnp = null;
                        TreeNodeUtil.GetDic(title.ParentNode.SysCode, out tnp);
                        tnp.Nodes.Add(tn);
                    }
                    else
                    {
                        tvTitle.Nodes.Add(tn);
                    }
                    TreeNodeUtil.AddDic(title.SysCode, tn);
                }
                if (tvTitle.Nodes.Count > 0)
                {
                    if (historySelected != null)
                    {
                        this.tvTitle.SelectedNode = historySelected;
                        this.tvTitle.Focus();
                    }
                    else
                    {
                        this.tvTitle.SelectedNode = tvTitle.Nodes[0];
                        this.tvTitle.Focus();
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(count);
                StaticMethods.AppExceptionHandle(exp, "101");
            }
        }


        /// <summary>
        /// 绑定末级科目
        /// </summary>
        void BindLastAccTree()
        {
            try
            {
                TreeNodeUtil.dicNode.Clear();
                this.tvTitle.Nodes.Clear();
                foreach (object o in lstTitles)
                {
                    AccountTitle title = o as AccountTitle;
                    TreeNode tn = new TreeNode();
                    tn.Name = title.Code;
                    tn.Text = title.Code + "  " + title.Name;
                    tn.Tag = title;
                    TreeNodeUtil.AddDic(title.SysCode, tn);

                    tvTitle.Nodes.Add(tn);
                    TreeNodeUtil.AddDic(title.SysCode, tn);
                }
                if (tvTitle.Nodes.Count > 0)
                {
                    this.tvTitle.SelectedNode = tvTitle.Nodes[0];
                    this.tvTitle.Focus();
                }
            }
            catch (Exception exp)
            {
                StaticMethods.AppExceptionHandle(exp, "101");
            }
        }

        private void tvTitle_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tnCurr = e.Node;
        }
    }
}

