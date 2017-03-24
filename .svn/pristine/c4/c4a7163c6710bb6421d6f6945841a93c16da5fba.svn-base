using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI
{
    public partial class VCostProjectSelect : Form
    {
        public IList Result = new ArrayList();
        IList lstTitles = new ArrayList();
        MCostProject model = new MCostProject();
        CostProject curTitle = null;
        TreeNode tnRoot = null;
        TreeNode tnCurr = null;

        //根节点
        string rootId = "0";

        public VCostProjectSelect()
        {
            model.titleService = StaticMethod.GetService("CostProjectSrv") as ICostProjectSrv;
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            GetAccountTitles();
            GetAccountDic();
            BindTreeByAccType();
            this.tvTitle.ExpandAll();
        }
        private void InitEvent()
        {
            this.tvTitle.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tvTitle_NodeMouseDoubleClick);
        }

        /// <summary>
        /// 获取会计科目分类树
        /// </summary>
        void GetAccountTitles()
        {
            lstTitles.Clear();
            lstTitles = model.GetCostProjects();
            if (lstTitles.Count == 0)
            {
                MessageBox.Show("未定义会计科目信息！");
            }
        }

        #region 构建科目树

        private IDictionary<string, TreeNode> GetAccountDic()
        {
            IDictionary<string, TreeNode> accDic = new Dictionary<string, TreeNode>();
            foreach (CostProject cirAcc in lstTitles)
            {
                TreeNode tn = new TreeNode();
                tn.Name = cirAcc.Id.ToString();
                tn.Text = cirAcc.Code + "  " + cirAcc.Name;
                tn.Tag = cirAcc;
                accDic.Add(cirAcc.Id, tn);
            }
            return accDic;
        }

        /// <summary>
        /// 根据科目类型显示科目树
        /// </summary>
        /// <param name="type"></param>
        void BindTreeByAccType()
        {
            try
            {
                tvTitle.Nodes.Clear();
                IDictionary<string, TreeNode> dicTitles = GetAccountDic();
                int count = dicTitles.Count;

                foreach (KeyValuePair<string, TreeNode> cirKey in dicTitles)
                {
                    CostProject nowAcc = cirKey.Value.Tag as CostProject;
                    if (nowAcc.CategoryNodeType == NodeType.RootNode)
                    {
                        rootId = nowAcc.Id;
                        tnRoot = new TreeNode();
                        tnRoot.Tag = nowAcc;
                        continue;
                    }

                    if (nowAcc.State == 0)
                    {
                        continue;
                    }

                    if (nowAcc.ParentNode != null && nowAcc.ParentNode.Id != rootId)
                    {
                        TreeNode parentNode = null;
                        dicTitles.TryGetValue(nowAcc.ParentNode.Id, out parentNode);
                        parentNode.Nodes.Add(cirKey.Value);
                    }
                    else
                    {
                        tvTitle.Nodes.Add(cirKey.Value);
                    }
                }

                if (tvTitle.Nodes.Count > 0)
                {
                    this.tvTitle.Focus();
                    tnCurr = tvTitle.Nodes[0];
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

        }

        #endregion

        void tvTitle_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tnCurr = e.Node;
            Result.Add(tnCurr.Tag);
            this.Close();
        }
    }
}
