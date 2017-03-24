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

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentSortSelect : TBasicDataView
    {
        string categoryTypeName = string.Empty;
        string userName = string.Empty;
        string jobId = string.Empty;
        private PLMWebServicesByKB.CategoryNode resultCate;

        public PLMWebServicesByKB.CategoryNode ResultCate
        {
            get { return resultCate; }
            set { resultCate = value; }
        }
        public VDocumentSortSelect()
        {
            InitializeComponent();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            InitEvent();
            InitData();
        }
        public VDocumentSortSelect(string objectTypeName)
        {
            InitializeComponent();
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            categoryTypeName = objectTypeName;
            InitEvent();
            InitData();
        }
        private void InitEvent()
        {
            this.btnSortTree.Click += new EventHandler(btnSortTree_Click);
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        void InitData()
        {
            dgvSortList.Visible = false;
            tvwSort.Visible = true;
            SortTree();
        }
        /// <summary>
        /// 表查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelect_Click(object sender, EventArgs e)
        {
            tvwSort.Visible = false;
            dgvSortList.Visible = true;
            List<PLMWebServicesByKB.CategoryNode> nodeList = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryNode>();
            PLMWebServicesByKB.CategoryNode[] resultList = null;

            PLMWebServicesByKB.ErrorStack es = StaticMethod.GetDocumentCategoryByKB(StaticMethod.ProjectDocumentCategoryTypeEnum.CLASSDOCUMENT, Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryQueryModeEnum.表方式,
                txtKeyword.Text.Trim(), null, StaticMethod.KB_System_UserName, StaticMethod.KB_System_JobId, null, out resultList);

            if (es != null)
            {
                MessageBox.Show("查询失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (resultList != null)
            {
                nodeList.AddRange(resultList);
                //if (list.Count > 0 && list != null)
                //{
                dgvSortList.Rows.Clear();
                foreach (PLMWebServicesByKB.CategoryNode node in nodeList)
                {
                    int rowIndex = dgvSortList.Rows.Add();
                    dgvSortList[colSortCode.Name, rowIndex].Value = node.CategoryCode;
                    dgvSortList[colSortName.Name, rowIndex].Value = node.CategoryName;
                    dgvSortList[colSortExplain.Name, rowIndex].Value = node.CategoryDesc;
                    dgvSortList.Rows[rowIndex].Tag = node;
                }
            }
            else
            {
                MessageBox.Show("未找到数据！");
            }
        }

        List<PLMWebServicesByKB.CategoryNode> list = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryNode>();
        /// <summary>
        /// 树查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSortTree_Click(object sender, EventArgs e)
        {
            SortTree();
        }
        void SortTree()
        {
            dgvSortList.Visible = false;
            tvwSort.Visible = true;
            list.Clear();
            tvwSort.Nodes.Clear();

            PLMWebServicesByKB.CategoryNode[] resultList = null;

            PLMWebServicesByKB.ErrorStack es = StaticMethod.GetDocumentCategoryByKB(StaticMethod.ProjectDocumentCategoryTypeEnum.CLASSDOCUMENT, Application.Business.Erp.SupplyChain.Client.PLMWebServicesByKB.CategoryQueryModeEnum.树方式,
                null, null, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_UserName, Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.KB_System_JobId, null, out resultList);

            if (es != null)
            {
                MessageBox.Show("查询失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            list.AddRange(resultList);

            if (list.Count > 0 && list != null)
            {
                IEnumerable<PLMWebServicesByKB.CategoryNode> listCate = list.OfType<PLMWebServicesByKB.CategoryNode>();

                var queryCate = from c in listCate
                                where c.ParentNode == null
                                select c;
                if (queryCate != null && queryCate.Count() > 0)
                {
                    for (int i = 0; i < queryCate.Count(); i++)
                    {
                        PLMWebServicesByKB.CategoryNode rootNode = queryCate.ElementAt(i);

                        TreeNode root = new TreeNode();
                        root.Text = rootNode.CategoryName;
                        root.Tag = rootNode;

                        LoadChildNode(rootNode, root, listCate);

                        tvwSort.Nodes.Add(root);
                    }
                }
            }
            else
            {
                //MessageBox.Show("未找到数据！");
            }
        }

        private void LoadChildNode(PLMWebServicesByKB.CategoryNode parentNode, TreeNode treeParentNode, IEnumerable<PLMWebServicesByKB.CategoryNode> listCate)
        {
            var queryCate = from c in listCate
                            where c.ParentNode != null && c.ParentNode.EntityID == parentNode.EntityID
                            select c;

            if (queryCate != null)
            {
                foreach (PLMWebServicesByKB.CategoryNode child in queryCate)
                {
                    TreeNode childNode = new TreeNode();
                    childNode.Text = child.CategoryName;
                    childNode.Tag = child;

                    treeParentNode.Nodes.Add(childNode);

                    LoadChildNode(child, childNode, listCate);
                }
            }
        }

        private string GetExceptionMessage(PLMWebServicesByKB.ErrorStack es)//PLMWebServices.ErrorStack es
        {
            string msg = es.Message;
            PLMWebServicesByKB.ErrorStack esTemp = es.InnerErrorStack;//PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1) || msg.IndexOf("违反唯一约束") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }
        void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvSortList.Visible == true)
            {
                if (dgvSortList.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                resultCate = dgvSortList.CurrentRow.Tag as PLMWebServicesByKB.CategoryNode;
            }
            if (tvwSort.Visible == true)
            {
                if (tvwSort.SelectedNode == null)
                {
                    MessageBox.Show("请选择一个任务节点！");
                    return;
                }
                resultCate = tvwSort.SelectedNode.Tag as PLMWebServicesByKB.CategoryNode;
            }
            this.Close();
        }


    }
}
