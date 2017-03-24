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
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VPBSTemplate : TBasicDataView
    {
        /// <summary>
        /// 定义一个变量，提供数据库服务
        /// </summary>
        public IPBSTreeSrv service;
        /// <summary>
        /// 树的根节点
        /// </summary>
        public TreeNode Root { get; set; }
        /// <summary>
        /// 所有模版中的所有节点
        /// </summary>
        public List<PBSTemplate> AllNodes { get; set; }
        /// <summary>
        /// 当前选中的PBS对象
        /// </summary>
        public PBSTemplate SelectedPBS { get; set; }
        /// <summary>
        /// 用来标识当前的状态是修改还是新增
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mot"></param>
        public VPBSTemplate(MPBSTree mot)
        {
            InitializeComponent();
            service = mot.Service;
            InitData();
            InitEvent();
        }

        /// <summary>
        /// 视图初始化
        /// </summary>
        private void InitData()
        {
            AllNodes = new List<PBSTemplate>();
            // 初始化tree的根节点
            Root = new TreeNode();
            Root.Tag = new PBSTemplate() { Type = new PBSTemplateType() };
            Root.Text = "建筑模型";
            tvTmpl.Nodes.Add(Root);
            Root.Expand();          // 展开根节点
            LoadAllNodes();         // 加载所有节点
            // 默认控件不可用
            txtPath.Enabled = false;
        }

        /// <summary>
        /// 事件初始化
        /// </summary>
        private void InitEvent()
        {
            // 选择结构类型
            btnSelect.Click += (a, b) => SelectType();
            // 树节点右击展开菜单
            tvTmpl.NodeMouseClick += (a, b) =>
            {
                if (b.Button == MouseButtons.Right)
                    btnMenu.Show(tvTmpl, new Point(b.X, b.Y));
            };
            // 树节点选择后加载节点详情
            tvTmpl.BeforeSelect += (a, b) =>
            {
                if (tvTmpl.SelectedNode == null) return;
                tvTmpl.SelectedNode.BackColor = Color.White;
                tvTmpl.SelectedNode.ForeColor = Color.Black;
            };
            tvTmpl.AfterSelect += (a, b) => Select();
            // 右键菜单事件
            btnAdd.Click += (a, b) => { Add(); btnMenu.Hide(); };
            btnEdit.Click += (a, b) => { Update(); btnMenu.Hide(); };
            btnDelete.Click += (a, b) => { Delete(); btnMenu.Hide(); };
            btnSave.Click += (a, b) =>
            {
                if (!Validate()) return;
                Save();
                btnMenu.Hide();
            };
        }

        #region 加载树节点
        /// <summary>
        /// 加载所有的模版节点，组成树
        /// </summary>
        private void LoadAllNodes()
        {
            // 获取所有的节点，使用属性AllNodes存储起来
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("Type", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Sort"));
            var list = service.ObjectQuery(typeof(PBSTemplate), oq);
            foreach (PBSTemplate item in list)
                AllNodes.Add(item);
            if (AllNodes == null || AllNodes.Count == 0) return;
            // 首先加载顶层的节点
            var topList = AllNodes.Where(a => string.IsNullOrEmpty(a.ParentId));
            foreach (var item in topList)
                CreateNode(item, Root);
        }

        /// <summary>
        /// 根据模版节点对象创建树节点，并设置为指定节点的子节点
        /// </summary>
        /// <param name="tmpl">模版节点对象</param>
        /// <param name="tn">指定节点</param>
        private void CreateNode(PBSTemplate tmpl, TreeNode tn)
        {
            var node = new TreeNode();
            node.Text = tmpl.Name;
            node.Tag = tmpl;
            LoadNodeChild(node);
            tn.Nodes.Add(node);
        }

        /// <summary>
        /// 加载指定节点的子节点
        /// </summary>
        /// <param name="id"></param>
        private void LoadNodeChild(TreeNode tn)
        {
            var tmpl = tn.Tag as PBSTemplate;
            var childs = AllNodes.Where(a => a.ParentId == tmpl.Id);    // 获取待展开节点的子节点
            if (childs.Count() == 0) return;
            foreach (var item in childs)
                CreateNode(item, tn);
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 选择结构类型
        /// </summary>
        private void SelectType()
        {
            VTemplateSelect typeView = new VTemplateSelect();
            typeView.ShowDialog();
            if (typeView.SelectItem == null) return;
            txtType.Text = typeView.SelectItem.Name;
            txtName.Text = typeView.SelectItem.Name;
            txtCode.Text = typeView.SelectItem.Code;
            txtBit.Text = typeView.SelectItem.CodeBit;
            txtType.Tag = typeView.SelectItem;
        }
        /// <summary>
        /// 更改控件的状态
        /// </summary>
        private void ChangeControlState(bool b)
        {
            txtName.ReadOnly = txtSort.ReadOnly = !b;
            btnSelect.Enabled = btnSave.Visible = txtDescription.Enabled = b;
            txtDescription.BackColor = b ? SystemColors.Window : SystemColors.Control;
        }

        /// <summary>
        /// 添加新节点
        /// </summary>
        private void Add()
        {
            IsNew = true;
            ChangeControlState(true);
            txtPath.Text = txtName.Text = txtDescription.Text = txtType.Text = ""; // 将数据输入框设置为空
            // 获取排序码【取当前选择的节点所属的子节点的最大排序码】
            int max = 0;
            if (AllNodes != null && AllNodes.Count > 0)
            {
                var result = AllNodes.Where(a => a.ParentId == SelectedPBS.Id);
                if (result != null && result.Count() > 0)
                    max = result.Select(a => a.Sort).Max();
            }
            txtSort.Text = (max + 1).ToString();
            txtCode.Text = SelectedPBS.Code + txtSort.Text.PadLeft(2, '0');
        }
        /// <summary>
        /// 输入数据验证
        /// </summary>
        private new bool Validate()
        {
            if (txtCode.Text == "" || txtName.Text == "" || txtType.Text == "")
            {
                MessageBox.Show("请将数据输入完整...");
                return false;
            }
            int num = 0;
            if (!int.TryParse(txtSort.Text, out num))
            {
                MessageBox.Show("排序码输入不正确...");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存新增的节点
        /// </summary>
        private void Save()
        {
            var pbsType = txtType.Tag as PBSTemplateType;
            if (IsNew)      // 新增节点
            {
                var node = new TreeNode();      // 新建一个节点
                var pbs = new PBSTemplate() // 新建一个PBS模版节点
                {
                    Name = txtName.Text,
                    Code = txtCode.Text,
                    Level = SelectedPBS.Level + 1,
                    ParentId = SelectedPBS.Id,
                    Description = txtDescription.Text,
                    SysCode = string.IsNullOrEmpty(SelectedPBS.Id) ? null : string.IsNullOrEmpty(SelectedPBS.SysCode) ? SelectedPBS.Id : SelectedPBS.SysCode + "." + SelectedPBS.Id,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    FullPath = string.IsNullOrEmpty(SelectedPBS.FullPath) ? txtName.Text : SelectedPBS.FullPath + "\\" + txtName.Text,
                    CreatePerson = ConstObject.LoginPersonInfo,
                    CreatePersonName = ConstObject.LoginPersonInfo.Name,
                    Sort = Convert.ToInt32(txtSort.Text),
                    Type = pbsType,
                    TypeBit = txtBit.Text,
                    TypeCode = pbsType.Code,
                    TypeName = pbsType.Name
                };
                // 保存到数据库
                pbs = service.Save(pbs) as PBSTemplate;
                // 加入节点集合
                AllNodes.Add(pbs);
                // 绑定到树节点
                node.Tag = pbs;
                node.Text = pbs.Name;
                tvTmpl.SelectedNode.Nodes.Add(node);
                tvTmpl.SelectedNode = node;
            }
            else
            {
                // 修改节点
                SelectedPBS.Code = txtCode.Text;
                SelectedPBS.Name = txtName.Text;
                SelectedPBS.Description = txtDescription.Text;
                SelectedPBS.Sort = Convert.ToInt32(txtSort.Text);
                SelectedPBS.ModifyTime = DateTime.Now;
                var index = SelectedPBS.FullPath.LastIndexOf('\\');// 最后一个\的序号
                SelectedPBS.FullPath = index > 0 ? SelectedPBS.FullPath.Substring(0, index) + "\\" + SelectedPBS.Name : SelectedPBS.Name;   // 重新设置全路径的值
                SelectedPBS.Type = pbsType;
                SelectedPBS.TypeCode = pbsType.Code;
                SelectedPBS.TypeName = pbsType.Name;
                SelectedPBS.TypeBit = txtBit.Text;
                txtPath.Text = SelectedPBS.FullPath;
                service.Save(SelectedPBS);
            }
            ChangeControlState(false);
        }
        /// <summary>
        /// 点击选择节点
        /// </summary>
        private new void Select()
        {
            tvTmpl.SelectedNode.BackColor = Color.Blue;
            tvTmpl.SelectedNode.ForeColor = Color.White;
            SelectedPBS = tvTmpl.SelectedNode.Tag as PBSTemplate;
            txtPath.Text = SelectedPBS.FullPath;
            txtCode.Text = SelectedPBS.Code;
            txtName.Text = SelectedPBS.Name;
            txtType.Text = SelectedPBS.TypeName;
            txtType.Tag = SelectedPBS.Type;
            txtDescription.Text = SelectedPBS.Description;
            txtSort.Text = SelectedPBS.Sort.ToString();
            txtBit.Text = SelectedPBS.TypeBit;
            // 更改控件状态
            ChangeControlState(false);
        }
        /// <summary>
        /// 修改选中的节点
        /// </summary>
        private new void Update()
        {
            if (tvTmpl.SelectedNode == Root) return;
            IsNew = false;
            ChangeControlState(true);
        }
        /// <summary>
        /// 删除选中的节点
        /// </summary>
        private void Delete()
        {
            if (tvTmpl.SelectedNode == Root) return;
            var result = MessageBox.Show("确定删除节点吗？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            service.Delete(SelectedPBS);
            
            service.SaveSQL("delete thd_pbstemplate where id = '" + SelectedPBS.Id + "'");
            tvTmpl.SelectedNode.Remove();
        }

        #endregion

    }
}
