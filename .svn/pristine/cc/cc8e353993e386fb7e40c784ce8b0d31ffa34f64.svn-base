using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using System.IO;
using System.Data.OleDb;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.Main;



namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng
{
    public partial class VCostItemCategoryImport : TBasicDataView
    {
        private CostItemCategory oprNode = null;
        private bool isNew = true;
        //有权限的成本项分类
        private IList lstInstance;
        //唯一编码
        private string uniqueCode;

        private Hashtable hashtableRules = new Hashtable();

        private Dictionary<string, TreeNode> listCheckedNode = new Dictionary<string, TreeNode>();
        private List<TreeNode> listCopyNode = new List<TreeNode>();

        public MCostItemCategory model;

        public VCostItemCategoryImport(MCostItemCategory mot)
        {
            model = mot;
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            LoadCostItemCategoryTree();

        }

        private void InitEvents()
        {
            //tvwCategory.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvwCategory_NodeMouseClick);

            //tvwCategory.AfterCheck += new TreeViewEventHandler(tvwCategory_AfterCheck);

            //mnuTree.ItemClicked += new ToolStripItemClickedEventHandler(mnuTree_ItemClicked);

            //tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            //tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            //tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            //tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            linkRefresh.Click += new EventHandler(linkRefresh_Click);

            btnBrownFile.Click += new EventHandler(btnBrownFile_Click);
            btnImport.Click += new EventHandler(btnImport_Click);

            btnCostItemCategoryMng.Click += new EventHandler(btnCostItemCategoryMng_Click);

            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnCheckNode.Click += new EventHandler(btnCheckNode_Click);
        }

        void btnBrownFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                txtExcelFilePath.Text = filePath;

                OleDbConnection conpart = null;
                try
                {
                    string ConnectionString = string.Empty;

                    if (Path.GetExtension(filePath).ToLower() == ".xls")
                        ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                    else
                        ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                    conpart = new OleDbConnection(ConnectionString);
                    conpart.Open();

                    DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    List<CostItemCategory> listCate = new List<CostItemCategory>();

                    cbTableNames.Items.Clear();
                    for (int i = 0; i < tables.Rows.Count; i++)
                    {
                        DataRow row = tables.Rows[i];

                        string tableName = row["TABLE_NAME"].ToString().Trim();

                        if (tableName != "" && tableName.Substring(tableName.Length - 1) == "$")
                        {
                            cbTableNames.Items.Add(tableName);
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    if (conpart != null)
                    {
                        conpart.Close();
                        conpart.Dispose();
                    }
                }
            }
        }

        void btnImport_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<CostItemCategory> listCate = new List<CostItemCategory>();

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }

                #region 导入数据

                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                CostItemCategory rootNode = null;
                
                List<CostItemCategory> listSaveNode = new List<CostItemCategory>();
                if (tvwCategory.SelectedNode.Tag != null)
                {
                    CostItemCategory cate = tvwCategory.SelectedNode.Tag as CostItemCategory;
                    ObjectQuery oq1 = new ObjectQuery();
                    oq1.AddCriterion(Expression.Eq("Id", cate.Id));
                    oq1.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
                    cate = model.ObjectQuery(typeof(CostItemCategory),oq1)[0] as CostItemCategory;
                    listSaveNode.Add(cate);
                }
                ObjectQuery oq = new ObjectQuery();
                if (rootNode == null)
                {
                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();

                    oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                    oq.AddCriterion(Expression.IsNull("ParentNode"));
                    IList list = model.modelCostItem.ObjectQuery(typeof(CostItemCategory), oq);
                    if (list.Count == 0)
                    {
                        MessageBox.Show("未找到分类根节点.");
                        return;
                    }
                    rootNode = list[0] as CostItemCategory;
                }
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);


                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];

                            string code = dataRow["分类代码"].ToString().Trim();//代码
                            if (string.IsNullOrEmpty(code))
                            {
                                logMsg.Append("Excel表" + tableName + "第" + j + "行数据的分类编码为空.");
                                logMsg.Append(Environment.NewLine);
                                continue;
                            }

                            if (code.IndexOf("0") > 0 || code.IndexOf("0") == -1)//分类第一位0丢失的情况
                                code = "0" + code;

                            string name = dataRow["分类名称"].ToString().Trim();
                            string desc = dataRow["分类说明"].ToString().Trim();
                            CostItemCategoryTypeEnum cateType = (CostItemCategoryTypeEnum)Convert.ToInt32(dataRow["分类类型"]);


                            if (code.Length == 2)//一级分类,加载到根节点
                            {

                                CostItemCategory childNode = new CostItemCategory();
                                childNode.ParentNode = rootNode;

                                childNode.Code = code;
                                childNode.Name = name;
                                childNode.Describe = desc;

                                childNode.CategoryState = CostItemCategoryState.制定;
                                childNode.TheProjectGUID = rootNode.TheProjectGUID;
                                childNode.TheProjectName = rootNode.TheProjectName;

                                childNode.OrderNo = model.GetMaxOrderNo(childNode) + 1;

                                childNode.CategoryType = cateType;

                                listSaveNode.Add(childNode);
                            }
                            else
                            {
                                string parentCode = code.Substring(0, code.Length - 2);

                                var queryParent = from c in listSaveNode
                                                  where c.Code == parentCode
                                                  select c;

                                
                                //if (queryParent.Count() == 0)
                                //{
                                //    oq.Criterions.Clear();
                                //    oq.FetchModes.Clear();

                                //    oq.AddCriterion(Expression.Eq("Code", parentCode));
                                //    oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);

                                //    queryParent = model.modelCostItem.ObjectQuery(typeof(CostItemCategory), oq).OfType<CostItemCategory>();

                                //    //if (queryParent.Count() > 0)
                                //    //    listSaveNode.Add(queryParent.ElementAt(0));
                                //}

                                if (queryParent.Count() == 0)
                                {
                                    logMsg.Append("Excel表" + tableName + "第" + j + "行,分类代码为"+code+"的分类未找到所属父分类.");
                                    logMsg.Append(Environment.NewLine);
                                    continue;
                                }

                                CostItemCategory parentNode = queryParent.ElementAt(0);

                                CostItemCategory childNode = new CostItemCategory();
                                childNode.ParentNode = parentNode;
                                parentNode.ChildNodes.Add(childNode);

                                childNode.Code = code;
                                childNode.Name = name;
                                childNode.Describe = desc;
                                childNode.CategoryState = CostItemCategoryState.制定;
                                childNode.TheProjectGUID = rootNode.TheProjectGUID;
                                childNode.TheProjectName = rootNode.TheProjectName;
                                childNode.OrderNo = parentNode.ChildNodes.Count;
                                childNode.CategoryType = cateType;

                                listSaveNode.Add(childNode);
                            }
                        }
                    }
                }

                foreach (CostItemCategory cate in listSaveNode)
                {
                    cate.ChildNodes.Clear();
                }

                if (listSaveNode.Count > 0)
                {
                    //保存节点
                    model.SaveCostItemCategorys(listSaveNode);
                }

                if (logMsg.Length > 0)
                {
                    //写日志
                    StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
                    write.Write(logMsg.ToString());
                    write.Close();
                    write.Dispose();
                }

                MessageBox.Show("数据导入完毕！");

                //打开错误日志
                if (logMsg.Length > 0)
                {
                    FileInfo file = new FileInfo(logFilePath);

                    //定义一个ProcessStartInfo实例
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //设置启动进程的初始目录
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //设置启动进程的应用程序或文档名
                    info.FileName = file.Name;
                    //设置启动进程的参数
                    info.Arguments = "";
                    //启动由包含进程启动信息的进程资源
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch (System.ComponentModel.Win32Exception we)
                    {
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        void btnCostItemCategoryMng_Click(object sender, EventArgs e)
        {
            UCL.Locate("成本项分类", OperationCostItemCategoryType.成本项分类);
        }

        /// <summary>
        /// 去掉编码尾部无效的0
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetFirstCode(string code)
        {
            string str = string.Empty;

            int index = 0;
            char[] chs = code.ToCharArray();
            for (int i = chs.Length - 1; i > -1; i--)
            {
                if (chs[i] != '0')
                {
                    index = i;
                    break;
                }
            }
            for (int i = 0; i <= index; i++)
            {
                str += chs[i];
            }

            return str;
        }

        void linkRefresh_Click(object sender, EventArgs e)
        {
            LoadCostItemCategoryTree();
        }

        void tvwCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) != 0 || (Control.ModifierKeys & Keys.Control) != 0)//如果同时按下了Ctrl //(Control.ModifierKeys & Keys.Shift) != 0 || 
            {
                if (e.Node.Checked)
                {
                    e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                    //e.Node.BackColor = SystemColors.Control;
                    //e.Node.ForeColor = SystemColors.ControlText;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode[e.Node.Name] = e.Node;
                    else
                        listCheckedNode.Add(e.Node.Name, e.Node);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    e.Node.BackColor = tempNode.BackColor;
                    e.Node.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode.Remove(e.Node.Name);
                }

                SetChildChecked(e.Node);
            }
            else
            {
                if (e.Node.Checked)
                {
                    e.Node.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    e.Node.ForeColor = ColorTranslator.FromHtml("#000000");

                    //e.Node.BackColor = SystemColors.Control;
                    //e.Node.ForeColor = SystemColors.ControlText;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode[e.Node.Name] = e.Node;
                    else
                        listCheckedNode.Add(e.Node.Name, e.Node);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    e.Node.BackColor = tempNode.BackColor;
                    e.Node.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(e.Node.Name))
                        listCheckedNode.Remove(e.Node.Name);
                }
            }

            RefreshControls(MainViewState.Check);
        }

        private void SetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                SetChildChecked(tn);
                tn.Checked = parentNode.Checked;

                if (tn.Checked)
                {
                    tn.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                    tn.ForeColor = ColorTranslator.FromHtml("#000000");

                    //tn.BackColor = SystemColors.Control;
                    //tn.ForeColor = SystemColors.ControlText;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                else
                {
                    TreeNode tempNode = new TreeNode();
                    tn.BackColor = tempNode.BackColor;
                    tn.ForeColor = tempNode.ForeColor;

                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        private void GetChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                GetChildChecked(tn);
            }
        }

        private void RemoveChildChecked(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                RemoveChildChecked(tn);
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
            }
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                oprNode = tvwCategory.SelectedNode.Tag as CostItemCategory;
                this.GetNodeDetail();
                txtNode.Text = "";
                txtNode.Tag = null;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void GetNodeDetail()
        {
            try
            {
                ClearAll();

                this.txtName.Text = oprNode.Name;
                this.txtCode.Text = oprNode.Code;
                this.txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath;

                if (oprNode.CategoryState != 0)
                    this.txtState.Text = oprNode.CategoryState.ToString();
                else
                    this.txtState.Text = CostItemCategoryState.制定.ToString();

                this.txtDesc.Text = oprNode.Describe;
                this.txtSummary.Text = oprNode.Summary;

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private void ClearAll()
        {
            this.txtCurrentPath.Text = "";
            this.txtCode.Text = "";
            this.txtName.Text = "";
            this.txtState.Text = "";
            this.txtDesc.Text = "";
            this.txtSummary.Text = "";
        }

        private void DeleteNode()
        {
            try
            {
                if (!ValideDelete())
                    return;
                bool reset = false;
                //父节点只有这一个子节点，并且父节点有权限操作，删除后要重新设置父节点tag
                if (tvwCategory.SelectedNode.Parent.Nodes.Count == 1)// && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Parent.Tag as CategoryNode)
                {
                    reset = true;
                }
                model.DeleteCostItemCategory(oprNode);

                if (reset)
                {
                    CostItemCategory org = model.GetCostItemCategoryById((tvwCategory.SelectedNode.Parent.Tag as CostItemCategory).Id);
                    if (org != null)
                        tvwCategory.SelectedNode.Parent.Tag = org;
                }

                //如果复制的节点有勾选的从选中集合中移除
                if (tvwCategory.SelectedNode.Checked)
                {
                    if (listCheckedNode.ContainsKey(tvwCategory.SelectedNode.Name))
                        listCheckedNode.Remove(tvwCategory.SelectedNode.Name);

                    RemoveChildChecked(tvwCategory.SelectedNode);
                }

                this.tvwCategory.Nodes.Remove(this.tvwCategory.SelectedNode);
            }
            catch (Exception exp)
            {
                string message = exp.Message;
                Exception ex1 = exp.InnerException;
                while (ex1 != null && !string.IsNullOrEmpty(ex1.Message))
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                if (message.IndexOf("违反") > -1 && message.IndexOf("约束") > -1)
                {
                    MessageBox.Show("该节点被成本项或其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("删除节点出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        private bool ValideDelete()
        {
            try
            {
                TreeNode tn = tvwCategory.SelectedNode;
                if (tn == null)
                {
                    MessageBox.Show("请先选择要删除的节点！");
                    return false;
                }
                if (tn.Parent == null)
                {
                    MessageBox.Show("根节点不允许删除！");
                    return false;
                }
                string text = "要删除当前选中的节点吗？该操作将连它的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

        void add_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll();
                oprNode = new CostItemCategory();
                oprNode.CategoryState = CostItemCategoryState.制定;
                txtState.Text = oprNode.CategoryState.ToString();

                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    oprNode.TheProjectGUID = projectInfo.Id;
                    oprNode.TheProjectName = projectInfo.Name;
                }

                oprNode.ParentNode = this.tvwCategory.SelectedNode.Tag as CostItemCategory;

                txtCurrentPath.Text = tvwCategory.SelectedNode.FullPath + "\\";
                txtCode.Focus();
            }
            catch (Exception exp)
            {
                MessageBox.Show("增加节点出错：" + ExceptionUtil.ExceptionMessage(exp));
            }
        }

        void delete_Click(object sender, EventArgs e)
        {
            DeleteNode();
        }

        void saveItem_Click(object sender, EventArgs e)
        {
            SaveView();
        }

        private bool ValideSave()
        {
            try
            {
                if (oprNode == null)
                {
                    oprNode = new CostItemCategory();
                    oprNode.CategoryState = CostItemCategoryState.制定;

                    txtState.Text = oprNode.CategoryState.ToString();
                }
                else if (oprNode.CategoryState == 0)
                {
                    oprNode.CategoryState = CostItemCategoryState.制定;
                    txtState.Text = oprNode.CategoryState.ToString();
                }

                if (string.IsNullOrEmpty(oprNode.TheProjectGUID) || string.IsNullOrEmpty(oprNode.TheProjectName))
                {
                    CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        oprNode.TheProjectGUID = projectInfo.Id;
                        oprNode.TheProjectName = projectInfo.Name;
                    }
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("名称不能为空!");
                    txtName.Focus();
                    return false;
                }
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("编码不能为空!");
                    txtCode.Focus();
                    return false;
                }

                oprNode.Name = txtName.Text.Trim();
                oprNode.Code = txtCode.Text.Trim();
                oprNode.Describe = txtDesc.Text.Trim();
                oprNode.Summary = txtSummary.Text.Trim();
                return true;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);

            tvwCategory.ItemDrag += new ItemDragEventHandler(tvwCategory_ItemDrag);
            tvwCategory.DragEnter += new DragEventHandler(tvwCategory_DragEnter);
            tvwCategory.DragDrop += new DragEventHandler(tvwCategory_DragDrop);
            tvwCategory.DragOver += new DragEventHandler(tvwCategory_DragOver);

            LoadCostItemCategoryTree();
        }

        private void LoadCostItemCategoryTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                IList list = model.GetCostItemCategoryByInstance();
                //lstInstance = listAll[1] as IList;
                //IList list = listAll[0] as IList;

                if (list.Count > 0)
                {
                    IEnumerable<CostItemCategory> listTemp = from t in list.OfType<CostItemCategory>()
                                                             where t.CategoryNodeType == NodeType.RootNode || t.ParentNode == null
                                                             select t;

                    if (listTemp != null && listTemp.Count() > 0 && projectInfo != null)
                    {
                        CostItemCategory root = listTemp.ElementAt(0);

                        if (string.IsNullOrEmpty(root.TheProjectGUID))
                        {
                            root.Name = projectInfo.Name;
                            root.SysCode = root.Id + ".";

                            root.TheProjectGUID = projectInfo.Id;
                            root.TheProjectName = projectInfo.Name;

                            if (string.IsNullOrEmpty(root.Code))
                                root.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

                            model.SaveCostItemCategory(root);

                            list = model.GetCostItemCategoryByInstance();
                            //lstInstance = listAll[1] as IList;
                            //list = listAll[0] as IList;
                        }
                    }
                }
                else
                {
                    IList listAdd = new List<CostItemCategory>();

                    CostItemCategory root = new CostItemCategory();
                    root.Name = projectInfo.Name;
                    root.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                    root.TheProjectGUID = projectInfo.Id;
                    root.TheProjectName = projectInfo.Name;

                    listAdd.Add(root);
                    model.SaveCostItemCategoryRootNode(listAdd);

                    list = model.GetCostItemCategoryByInstance();
                    //lstInstance = listAll[1] as IList;
                    //list = listAll[0] as IList;
                }

                foreach (CostItemCategory childNode in list)
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
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("查询成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    this.mnuTree.Items["撤销"].Enabled = true;
                    this.mnuTree.Items["保存节点"].Enabled = true;
                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;

                    this.mnuTree.Items["发布节点"].Enabled = false;
                    this.mnuTree.Items["冻结节点"].Enabled = false;
                    this.mnuTree.Items["作废节点"].Enabled = false;

                    this.linkAdd.Enabled = false;
                    this.linkUpdate.Enabled = false;
                    this.linkDelete.Enabled = false;
                    this.linkCancel.Enabled = true;
                    this.linkSave.Enabled = true;

                    txtCode.ReadOnly = false;
                    txtName.ReadOnly = false;
                    txtState.ReadOnly = true;
                    txtDesc.ReadOnly = false;
                    txtSummary.ReadOnly = false;
                    break;

                case MainViewState.Browser:

                    this.mnuTree.Items["增加子节点"].Enabled = false;
                    this.mnuTree.Items["修改节点"].Enabled = false;
                    this.mnuTree.Items["删除节点"].Enabled = false;

                    this.mnuTree.Items["撤销"].Enabled = false;
                    this.mnuTree.Items["保存节点"].Enabled = false;

                    this.mnuTree.Items["发布节点"].Enabled = false;
                    this.mnuTree.Items["冻结节点"].Enabled = false;
                    this.mnuTree.Items["作废节点"].Enabled = false;

                    this.linkAdd.Enabled = false;
                    this.linkUpdate.Enabled = false;

                    this.linkCancel.Enabled = false;
                    this.linkSave.Enabled = false;

                    this.linkDelete.Enabled = false;

                    if (oprNode.CategoryState == 0 || oprNode.CategoryState == CostItemCategoryState.制定)
                    {
                        this.mnuTree.Items["增加子节点"].Enabled = true;
                        this.mnuTree.Items["修改节点"].Enabled = true;
                        this.mnuTree.Items["删除节点"].Enabled = true;
                        this.mnuTree.Items["发布节点"].Enabled = true;

                        this.linkAdd.Enabled = true;
                        this.linkUpdate.Enabled = true;
                        this.linkDelete.Enabled = true;
                    }
                    else if (oprNode.CategoryState == CostItemCategoryState.发布)
                    {
                        this.mnuTree.Items["增加子节点"].Enabled = true;
                        this.mnuTree.Items["冻结节点"].Enabled = true;
                        this.mnuTree.Items["作废节点"].Enabled = true;

                        linkAdd.Enabled = true;
                    }
                    else if (oprNode.CategoryState == CostItemCategoryState.冻结)
                    {
                        this.mnuTree.Items["发布节点"].Enabled = true;
                        this.mnuTree.Items["作废节点"].Enabled = true;
                    }

                    if (tvwCategory.SelectedNode != null && tvwCategory.SelectedNode.Parent == null)
                        this.linkDelete.Enabled = false;

                    txtCode.ReadOnly = true;
                    txtName.ReadOnly = true;
                    txtState.ReadOnly = true;
                    txtDesc.ReadOnly = true;
                    txtSummary.ReadOnly = true;
                    break;
            }
        }

        public override bool ModifyView()
        {
            return true;
        }

        public override bool CancelView()
        {
            try
            {
                LoadCostItemCategoryTree();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        public override void RefreshView()
        {
            try
            {
                LoadCostItemCategoryTree();
            }
            catch (Exception e)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override bool SaveView()
        {
            bool isNew = false;
            try
            {
                if (!ValideSave())
                    return false;
                if (oprNode.Id == null)
                    isNew = true;
                else
                    isNew = false;
                oprNode = model.SaveCostItemCategory(oprNode);

                if (isNew)
                {
                    //要添加子节点的节点以前没有子节点，需要重新设置Tag
                    if (tvwCategory.SelectedNode.Nodes.Count == 0)
                        tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                    TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                    //新增节点要有权限操作
                    //lstInstance.Add(oprNode);
                    tn.Tag = oprNode;
                    this.tvwCategory.SelectedNode = tn;
                    tn.Expand();
                }
                else
                {
                    this.tvwCategory.SelectedNode.Text = oprNode.Name.ToString();
                }
                return true;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null && exp.InnerException.Message.Contains("违反唯一约束条件"))
                    MessageBox.Show("编码必须唯一！");
                else
                    MessageBox.Show("保存成本项分类树错误：" + ExceptionUtil.ExceptionMessage(exp));
            }
            return false;
        }

        #region 节点拖拽移动

        private void tvwCategory_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CostItemCategory org = (e.Item as TreeNode).Tag as CostItemCategory;
                //有权限的节点才允许拖动操作
                if (org != null)// && ConstMethod.Contains(lstInstance, org)
                {
                    DoDragDrop(e.Item, DragDropEffects.All);
                }
            }
        }

        private void tvwCategory_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void tvwCategory_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
                TreeNode targetNode = tvwCategory.GetNodeAt(targetPoint);
                //目标节点没有权限不允许操作
                if (targetNode == null)// || !ConstMethod.Contains(lstInstance, targetNode.Tag as CostItemCategory)
                    return;
                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

                if (TreeViewUtil.CanMoveNode(draggedNode, targetNode))
                {
                    //以前的父节点
                    TreeNode oldParentNode = draggedNode.Parent;
                    bool reset = false;
                    //父节点只有这一个子节点，并且父节点有权限操作，移动后要重新设置父节点tag
                    if (oldParentNode.Nodes.Count == 1)// && ConstMethod.Contains(lstInstance, oldParentNode.Tag as CategoryNode)
                    {
                        reset = true;
                    }

                    frmTreeMoveCopy frmTmp = new frmTreeMoveCopy();
                    frmTmp.TargetNode = targetNode;
                    frmTmp.DraggedNode = draggedNode;
                    if (draggedNode.Parent == targetNode.Parent)
                        frmTmp.IsOrder = true;
                    frmTmp.ShowDialog();
                    if (frmTmp.IsOK == true)
                    {
                        //复制树节点
                        if (frmTmp.MoveOrCopy == enmMoveOrCopy.copy)
                        {
                            draggedNode = frmTmp.DraggedNode;
                            CostItemCategory catTmp = (draggedNode.Tag as CostItemCategory).Clone();
                            //系统生存一个唯一编码
                            catTmp.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                            uniqueCode = catTmp.Code;
                            catTmp.ParentNode = targetNode.Tag as CostItemCategory;
                            catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;
                            //draggedNode.Tag = catTmp;

                            IList lst = new ArrayList();
                            lst.Add(catTmp);
                            //克隆要复制的节点和其子节点的对象
                            PopulateList(draggedNode, lst, catTmp);
                            lst = model.SaveCostItemCategorys(lst);
                            //新增节点要有权限操作
                            //(lstInstance as ArrayList).AddRange(lst);
                            //给复制节点的新父节点tag设值
                            targetNode.Tag = (lst[0] as CostItemCategory).ParentNode;
                            int i = 0;
                            CopyObjToTag(draggedNode, lst, ref i);

                            //如果复制的节点有勾选的加入到选中集合
                            foreach (TreeNode tn in targetNode.Nodes)
                            {
                                if (tn.Checked)
                                {
                                    if (listCheckedNode.ContainsKey(tn.Name))
                                        listCheckedNode[tn.Name] = tn;
                                    else
                                        listCheckedNode.Add(tn.Name, tn);
                                }
                                GetChildChecked(tn);
                            }
                        }
                        //移动树节点
                        else if (frmTmp.MoveOrCopy == enmMoveOrCopy.move)
                        {
                            CostItemCategory toObj = targetNode.Tag as CostItemCategory;
                            IDictionary dic = model.MoveNode(draggedNode.Tag as CostItemCategory, toObj);
                            if (reset)
                            {
                                CostItemCategory cat = model.GetCostItemCategoryById((oldParentNode.Tag as CostItemCategory).Id);
                                oldParentNode.Tag = cat;
                            }
                            targetNode.Tag = dic[(targetNode.Tag as CostItemCategory).Id.ToString()];
                            //根据返回的数据进行节点tag赋值
                            ResetTagAfterMove(draggedNode, dic);
                        }
                        //排序
                        else if (draggedNode.Parent == targetNode.Parent)
                        {
                            if (draggedNode.PrevNode != null)
                            {
                                IList result = new ArrayList();
                                CostItemCategory prevOrg = draggedNode.PrevNode.Tag as CostItemCategory;
                                SetNextNodeOrder(draggedNode, result, prevOrg.OrderNo + 1);
                                result = model.SaveCostItemCategorys(result);
                                ResetTagAfterOrder(draggedNode, result, 0);
                            }
                            else
                            {
                                CostItemCategory fromOrg = draggedNode.Tag as CostItemCategory;
                                CostItemCategory toOrg = targetNode.Tag as CostItemCategory;
                                fromOrg.OrderNo = toOrg.OrderNo - 1;
                                draggedNode.Tag = model.SaveCostItemCategory(fromOrg);
                            }
                        }
                        //保证拖动后马上修改保存不出错
                        this.tvwCategory.SelectedNode = draggedNode;
                    }
                }//用户如果把节点移到空白区再选中被拖动节点
                else if (targetNode == null)
                {
                    tvwCategory.SelectedNode = draggedNode;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("移动分类出错：" + ExceptionUtil.ExceptionMessage(ee));
            }
        }
        //设置后续节点的排序号
        private void SetNextNodeOrder(TreeNode node, IList list, long order)
        {
            CostItemCategory org = node.Tag as CostItemCategory;
            org.OrderNo = order;
            list.Add(org);
            if (node.NextNode != null)
            {
                SetNextNodeOrder(node.NextNode, list, order + 1);
            }
        }
        //排序后重新设置节点的Tag
        private void ResetTagAfterOrder(TreeNode node, IList lst, int i)
        {
            node.Tag = lst[i];
            if (node.NextNode != null)
                ResetTagAfterOrder(node.NextNode, lst, i + 1);
        }
        //移动后重新设置节点的Tag
        private void ResetTagAfterMove(TreeNode node, IDictionary dic)
        {
            node.Tag = dic[(node.Tag as CostItemCategory).Id.ToString()];
            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                ResetTagAfterMove(var, dic);
            }
        }
        //复制后重新设置节点的Tag
        private void CopyObjToTag(TreeNode node, IList lst, ref int i)
        {
            node.Name = (lst[i] as CostItemCategory).Id;
            node.Tag = lst[i];

            if (node.Nodes.Count == 0)
                return;
            foreach (TreeNode var in node.Nodes)
            {
                i++;
                CopyObjToTag(var, lst, ref i);
            }
        }

        private void PopulateList(TreeNode node, IList lst, CostItemCategory parent)
        {
            if (node.Nodes.Count == 0)
                return;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode var = node.Nodes[i];
                CostItemCategory matCatTmp = (var.Tag as CostItemCategory).Clone();
                uniqueCode = ConstMethod.GetNextCode(uniqueCode);
                matCatTmp.Code = uniqueCode;
                matCatTmp.ParentNode = parent;
                matCatTmp.OrderNo = i + 1;
                //var.Tag = matCatTmp;
                lst.Add(matCatTmp);
                PopulateList(var, lst, matCatTmp);
            }
        }

        private void tvwCategory_DragOver(object sender, DragEventArgs e)
        {
            //Point targetPoint = tvwCategory.PointToClient(new Point(e.X, e.Y));
            //tvwCategory.SelectedNode = tvwCategory.GetNodeAt(targetPoint);
        }

        #endregion

        void mnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "增加子节点")
            {
                RefreshControls(MainViewState.Modify);
                add_Click(null, new EventArgs());
            }
            else if (e.ClickedItem.Text.Trim() == "修改节点")
            {
                RefreshControls(MainViewState.Modify);
            }
            else if (e.ClickedItem.Text.Trim() == "删除节点")
            {
                mnuTree.Hide();
                delete_Click(null, new EventArgs());
                RefreshControls(MainViewState.Browser);
                this.Refresh();
            }
            else if (e.ClickedItem.Text.Trim() == "撤销")
            {
                mnuTree.Hide();
                RefreshControls(MainViewState.Browser);
                this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
            }
            else if (e.ClickedItem.Text.Trim() == "保存节点")
            {
                mnuTree.Hide();
                if (!SaveView()) return;
                this.RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "复制勾选节点")
            {
                mnuTree.Hide();

                listCopyNode.Clear();
                foreach (var dic in listCheckedNode)
                {
                    listCopyNode.Add(dic.Value);
                }

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "粘贴节点")
            {
                mnuTree.Hide();
                SaveCopyNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "删除勾选节点")
            {
                mnuTree.Hide();
                DeleteCheckedNode();

                RefreshControls(MainViewState.Check);
            }
            else if (e.ClickedItem.Text.Trim() == "发布节点")
            {
                mnuTree.Hide();
                PublishNode();
                RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "冻结节点")
            {
                mnuTree.Hide();
                FreezeNode();
                RefreshControls(MainViewState.Browser);
            }
            else if (e.ClickedItem.Text.Trim() == "作废节点")
            {
                mnuTree.Hide();
                CancellationNode();
                RefreshControls(MainViewState.Browser);
            }
        }

        private void GetCopyNode(TreeNode parentNode)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Checked)
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode[tn.Name] = tn;
                    else
                        listCheckedNode.Add(tn.Name, tn);
                }
                else
                {
                    if (listCheckedNode.ContainsKey(tn.Name))
                        listCheckedNode.Remove(tn.Name);
                }
                GetCopyNode(tn);
            }
        }

        private void SaveCopyNode()
        {
            if (listCopyNode.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (TreeNode draggedNode in listCopyNode)
                {
                    CostItemCategory catTmp = (draggedNode.Tag as CostItemCategory).Clone();

                    //系统生存一个唯一编码
                    catTmp.Code = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                    uniqueCode = catTmp.Code;
                    catTmp.ParentNode = tvwCategory.SelectedNode.Tag as CostItemCategory;
                    catTmp.OrderNo = model.GetMaxOrderNo(catTmp) + 1;

                    lst.Add(catTmp);
                }
                //保存复制的节点
                lst = model.SaveCostItemCategorys(lst);
                //新增节点要有权限操作
                //(lstInstance as ArrayList).AddRange(lst);
                //给复制节点的新父节点tag设值
                tvwCategory.SelectedNode.Tag = (lst[0] as CostItemCategory).ParentNode;

                foreach (CostItemCategory pbs in lst)
                {
                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = pbs.Id.ToString();
                    tnTmp.Text = pbs.Name;
                    tnTmp.Tag = pbs;

                    tvwCategory.SelectedNode.Nodes.Add(tnTmp);
                }

                listCopyNode.Clear();
            }
        }

        private void DeleteCheckedNode()
        {
            try
            {
                IList list = new ArrayList();
                foreach (var dic in listCheckedNode)
                {
                    if (dic.Value.Parent == null)
                    {
                        MessageBox.Show("根节点不允许删除！");
                        return;
                    }
                    list.Add(dic.Value.Tag as CostItemCategory);
                }

                string text = "要删除勾选的所有节点吗？该操作将连它们的所有子节点一同删除！";
                if (MessageBox.Show(text, "删除节点", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                if (model.DeleteCostItemCategory(list))//删除成功
                {
                    //从PBS树上移除对应的节点
                    foreach (CostItemCategory pbs in list)
                    {
                        foreach (TreeNode tn in tvwCategory.Nodes)
                        {
                            if (tn.Name == pbs.Id)
                            {
                                tvwCategory.Nodes.Remove(tn);
                                break;
                            }

                            if (tn.Nodes.Count > 0)
                            {
                                if (RemoveTreeNode(tn, pbs))
                                    break;
                            }
                        }
                    }
                    listCheckedNode.Clear();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Exception ex1 = ex.InnerException;
                while (ex1 != null && !string.IsNullOrEmpty(ex1.Message))
                {
                    message += ex1.Message;

                    ex1 = ex1.InnerException;
                }

                if (message.IndexOf("违反") > -1 && message.IndexOf("约束") > -1)
                {
                    MessageBox.Show("勾选节点中有节点被成本项或其它数据所引用，删除前请先删除引用的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private bool RemoveTreeNode(TreeNode parentNode, CostItemCategory pbs)
        {
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name == pbs.Id)
                {
                    parentNode.Nodes.Remove(tn);
                    return true;
                }
                if (tn.Nodes.Count > 0)
                {
                    if (RemoveTreeNode(tn, pbs))
                        return true;
                }
            }
            return false;
        }

        private void PublishNode()
        {
            try
            {
                oprNode = model.GetCostItemCategoryById(oprNode.Id) as CostItemCategory;
                oprNode.CategoryState = CostItemCategoryState.发布;

                oprNode = model.SaveCostItemCategory(oprNode);
                tvwCategory.SelectedNode.Tag = oprNode;

                txtState.Text = oprNode.CategoryState.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private void FreezeNode()
        {
            try
            {
                oprNode = model.GetCostItemCategoryById(oprNode.Id) as CostItemCategory;
                oprNode.CategoryState = CostItemCategoryState.冻结;

                oprNode = model.SaveCostItemCategory(oprNode);
                tvwCategory.SelectedNode.Tag = oprNode;

                txtState.Text = oprNode.CategoryState.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private void CancellationNode()
        {
            try
            {
                oprNode = model.GetCostItemCategoryById(oprNode.Id) as CostItemCategory;
                oprNode.CategoryState = CostItemCategoryState.作废;

                oprNode = model.SaveCostItemCategory(oprNode);
                tvwCategory.SelectedNode.Tag = oprNode;

                txtState.Text = oprNode.CategoryState.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void tvwCategory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)// && ConstMethod.Contains(lstInstance, tvwCategory.SelectedNode.Tag as CategoryNode)
            {
                tvwCategory.SelectedNode = e.Node;

                if (e.Node.Parent == null)
                    mnuTree.Items["删除节点"].Enabled = false;

                if (listCheckedNode.Count == 0)
                {
                    mnuTree.Items["复制勾选节点"].Enabled = false;
                    mnuTree.Items["删除勾选节点"].Enabled = false;
                }
                else
                {
                    mnuTree.Items["复制勾选节点"].Enabled = true;
                    mnuTree.Items["删除勾选节点"].Enabled = true;
                }

                if (listCopyNode.Count == 0)
                    mnuTree.Items["粘贴节点"].Enabled = false;
                else
                    mnuTree.Items["粘贴节点"].Enabled = true;


                mnuTree.Show(tvwCategory, new Point(e.X, e.Y));
            }
        }

        public void ReloadTreeNode()
        {
            if (isNew)
            {
                //要添加子节点的节点以前没有子节点，需要重新设置Tag
                if (tvwCategory.SelectedNode.Nodes.Count == 0)
                    tvwCategory.SelectedNode.Tag = oprNode.ParentNode;
                TreeNode tn = this.tvwCategory.SelectedNode.Nodes.Add(oprNode.Id.ToString(), oprNode.Name.ToString());
                //新增节点要有权限操作
                //lstInstance.Add(oprNode);
                tn.Tag = oprNode;
                this.tvwCategory.SelectedNode = tn;
                tn.Expand();
            }
            else
            {
                this.tvwCategory.SelectedNode.Text = oprNode.Name.ToString();
            }
        }

        #region 操作按钮
        private void linkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshControls(MainViewState.Modify);
            add_Click(null, new EventArgs());
        }

        private void linkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshControls(MainViewState.Modify);
        }

        private void linkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            delete_Click(null, new EventArgs());
            RefreshControls(MainViewState.Browser);
            this.Refresh();
        }

        private void linkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            RefreshControls(MainViewState.Browser);
            this.tvwCategory_AfterSelect(this.tvwCategory, new TreeViewEventArgs(this.tvwCategory.SelectedNode));
        }

        private void linkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            if (!SaveView()) return;
            this.RefreshControls(MainViewState.Browser);
        }

        private void linkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();

            listCopyNode.Clear();
            foreach (var dic in listCheckedNode)
            {
                listCopyNode.Add(dic.Value);
            }
            RefreshControls(MainViewState.Check);
        }

        private void linkPaste_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            SaveCopyNode();
            RefreshControls(MainViewState.Check);
        }

        private void linkDeleteChecked_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mnuTree.Hide();
            DeleteCheckedNode();
            RefreshControls(MainViewState.Check);
        }

        private void linkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkCancellation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        #endregion

        void btnCheckNode_Click(object sender, EventArgs e)
        {
            if (tvwCategory.SelectedNode.Tag != null)
            {
                CostItemCategory cate = tvwCategory.SelectedNode.Tag as CostItemCategory;
                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddCriterion(Expression.Eq("Id", cate.Id));
                oq1.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
                cate = model.ObjectQuery(typeof(CostItemCategory), oq1)[0] as CostItemCategory;
                txtNode.Tag = cate;
                txtNode.Text = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(CostItemCategory), cate.Name, cate.SysCode);
            }
            else
            {
                MessageBox.Show("请选择一个分类！");
            }
        }
        //批量添加
        void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNode.Text.Trim()) && txtNode.Tag != null)
            {
                CostItemCategory cate = txtNode.Tag as CostItemCategory;
                VCostItemCategoryAdd frm = new VCostItemCategoryAdd(cate);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先确定节点！");
            }
        }
    }
}
