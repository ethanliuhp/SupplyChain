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
using NHibernate;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;

namespace Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI
{
    public partial class VAccTitleSelect : Application.Business.Erp.Financial.Client.Basic.CommonClass.BaseForm
    {
        static  IAccountTitleService titleSrv;
        public  AccountType accType=AccountType.ALL;
        public  IList lstObjects=new ArrayList();
        public  int AccLevel=0;
        public AccountTitle SelectedTitle = null;
        public bool IsAllowMulti = false;//多科目选择
        public string SelectedTitleCodes = "";
        public bool OnlySelectLeafNode = false;//只允许选择末级科目 

        public VAccTitleSelect()
        {
            InitializeComponent();
            this.Load += new EventHandler(VAccTitleSelect_Load);
            this.btnSearch.Click +=new EventHandler(btnSearch_Click);
            btnFiltrate.Click += new EventHandler(btnFiltrate_Click);
            this.tvTitle.AfterSelect +=new TreeViewEventHandler(tvTitle_AfterSelect);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.tvTitle.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvTitle_NodeMouseDoubleClick);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.tvTitle.AfterCheck += new TreeViewEventHandler(tvTitle_AfterCheck);
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
                StaticMethods.ShowErrorMessage("请选择末级科目");
                return;
            }
            if (!this.IsAllowMulti)
            {
                this.SelectedTitle = this.tvTitle.SelectedNode.Tag as AccountTitle;
                this.Close();
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.IsAllowMulti)
            {
                if (OnlySelectLeafNode && this.tvTitle.SelectedNode.Nodes.Count != 0)
                {
                    StaticMethods.ShowErrorMessage("请选择末级科目");
                    return;
                }
                this.SelectedTitle = this.tvTitle.SelectedNode.Tag as AccountTitle;
            }
            else
            {
                foreach (TreeNode tn in tvTitle.Nodes)
                {
                    if (tn.Checked)
                    {
                        this.SelectedTitleCodes += "," + (tn.Tag as AccountTitle).AccountCode;
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
                if (tn.Checked)
                {
                    if (tn.Checked)
                    {
                        SelectedTitleCodes += "," + (tn.Tag as AccountTitle).AccountCode;
                    }
                    if(tn.Nodes.Count != 0)
                        GetChildNodes(tn, ref SelectedTitleCodes);
                }
            }
        }


        void btnFiltrate_Click(object sender, EventArgs e)
        {
            AccountTitle title = new AccountTitle();
            ObjectQuery query = new ObjectQuery();
            if (accType != AccountType.ALL)
                query.AddCriterion(Expression.Eq("AccType", accType));
            if (AccLevel != 0)//为零则不限制科目显示层次
                query.AddCriterion(Expression.Le("Level", 1L));
            query.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.LeafNode));
            query.AddOrder(Order.Asc("AccountCode"));
            query.AddCriterion(Expression.Le("State", 1L));
            query.AddCriterion(Expression.Le("FreezeAccount", false));
            query.AddCriterion(Expression.Eq("BankAccBook", chkBank.Checked));
            query.AddCriterion(Expression.Eq("BudgetManage", chkBudget.Checked));
            query.AddCriterion(Expression.Eq("ClientAccount", chkClient.Checked));
            query.AddCriterion(Expression.Eq("DailyAccBook", chkDaily.Checked));
            query.AddCriterion(Expression.Eq("DepartmentAccount", chkDepartmentAcc.Checked));
            query.AddCriterion(Expression.Eq("EndorsementManage", chkEndorsement.Checked));
            query.AddCriterion(Expression.Eq("ForeignAccount", chkForeignAcc.Checked));
            query.AddCriterion(Expression.Eq("PersonAccount", chkPersonAcc.Checked));
            query.AddCriterion(Expression.Eq("ProjectAccount", chkProject.Checked));
            query.AddCriterion(Expression.Eq("QuantityAccount", chkQuantityAccount.Checked));
            query.AddCriterion(Expression.Eq("SupplierAccount", chkSupplier.Checked));
            //query.AddCriterion(Expression.Eq("FiscalYear", GlobalObjects.LoginInfo.CompPeriod.NowYear));

            GetAccountTitles(query);
            BindLastAccTree();
        }

        void VAccTitleSelect_Load(object sender, EventArgs e)
        {
            if (lstTitles == null)
            {
                InitData();
            }
        }

        void InitData()
        {
            if (IsAllowMulti) this.tvTitle.CheckBoxes = true;
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
            query.AddFetchMode("ForeignCurrency", FetchMode.Eager);//ForeignCurrency
            //query.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            //query.AddCriterion(Expression.Eq("FiscalYear", GlobalObjects.LoginInfo.CompPeriod.NowYear));
            GetAccountTitles(query);
            BindAccTree();
        }


        IList lstTitles = null;
        /// <summary>
        /// 获取会计科目分类树
        /// </summary>
        void GetAccountTitles(ObjectQuery oq)
        {
            if (lstTitles == null)
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
                    if (SelectedTitleCodes.Trim().Equals(title.Code))
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
                        this.tvTitle.SelectedNode.Expand();
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
            AccountTitle title = tnCurr.Tag as AccountTitle;
            if (title.CategoryNodeType == NodeType.LeafNode)
            {
                this.chkLeafNode.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                chkLeafNode.ForeColor = System.Drawing.Color.Black;
            }
            chkBank.Checked = title.BankAccBook;
            chkBudget.Checked = title.BudgetManage;
            chkClient.Checked = title.ClientAccount;
            chkDaily.Checked = title.DailyAccBook;
            chkDepartmentAcc.Checked = title.DepartmentAccount;
            chkEndorsement.Checked = title.EndorsementManage;
            chkForeignAcc.Checked = title.ForeignAccount;
            chkPersonAcc.Checked = title.PersonAccount;
            chkProject.Checked = title.ProjectAccount;
            chkQuantityAccount.Checked = title.QuantityAccount;
            chkSupplier.Checked = title.SupplierAccount;
        }


        IDictionary<TreeNode, bool> dicTreeNodes = new Dictionary<TreeNode, bool>();
        ArrayList lstFindNodes = new ArrayList();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (lstTitles.Count == 0)
            {
                StaticMethods.ShowMessage("未定义任何科目");
                return;
            }
            bool isFind = false;
            int  count = 0;

            foreach (KeyValuePair<TreeNode, bool> pair in dicTreeNodes)
            {
                if(bool.Parse(pair.Value.ToString()).Equals(true))
                {
                    count++;
                }
            }
            //if (count == dicTreeNodes.Count && count!=0)
            //{
            //    allFind = true;
            //    DialogResult dr = MessageBox.Show("查找已到最后是否从头搜索？", "查找科目", MessageBoxButtons.YesNo);
            //    dicTreeNodes.Clear();
            //    lstFindNodes.Clear();
            //    if (dr == DialogResult.No)
            //    {
            //        return;
            //    }
            //}

            foreach (TreeNode tn in tvTitle.Nodes)
            {
                IList lstNodes = new ArrayList();
                tvTitle.GetALLChildNodes(tn, lstNodes);
                foreach (TreeNode tn1 in lstNodes)
                {
                    if(!dicTreeNodes.ContainsKey(tn1))
                        dicTreeNodes.Add(tn1, false);
                }
                count = lstNodes.Count;
                foreach (TreeNode tn1 in lstNodes)
                {
                    count++;
                    if (!tn1.Text.Contains(txtCondiction.Text.Trim())) continue;
                    isFind = true;
                    if (lstFindNodes.Contains(tn1) && count>0) continue;
                    this.tvTitle.SelectedNode = tn1;
                    if (!lstFindNodes.Contains(tn1))
                    {
                        lstFindNodes.Add(tn1);
                    }
                    return;
                }
            }
            if (!isFind)
            {
                StaticMethods.ShowMessage("未发现相关匹配的科目");
            }
        }
    }
}

