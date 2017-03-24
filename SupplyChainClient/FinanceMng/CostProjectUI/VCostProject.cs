using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.FinancialResource.RelateClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using ObjectLock = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ObjectLock;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Domain;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI
{
    /// <summary>
    /// 成本项目分类
    /// </summary>
    public partial class VCostProject : TBasicDataView
    {
        IList lstTitles = new ArrayList();
        IList lstCurrency = new ArrayList();
        IList lstDeskAcc = new ArrayList();
        MCostProject model;
        CostProject curTitle = null;
        TreeNode tnRoot = null;
        TreeNode tnCurr = null;
        string ActionType = "";

        //根节点
        string rootId = "0";

        public VCostProject(MCostProject model)
        {
            this.model = model;
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            //加载费用项目
            this.txtExpenseItem.Items.Clear();
            //this.txtExpenseItem.Items.Count;
            IList expenseItemList = model.GetAllExpenseItem();
            if (expenseItemList != null && expenseItemList.Count > 0)
            {
                foreach (ExpenseItem item in expenseItemList)
                {
                    if (item.OperTache == enmOperationTache.采购环节)
                    {
                        this.txtExpenseItem.Items.Add(item);
                    }

                }
            }

            //if (ConstObject.FrameWorkNewFlag)
            //{
            //    this.barOperation.Visible = true;
            //    this.UserCode = LoginInfomation.LoginInfo.ThePerson.Code;
            //    this.BarKeyCode = "CostProject";
            //}
            //else
            //{
            // this.barOperation.Visible = false;
            //}

        }
        private void InitEvents()
        {
            this.tsiSame.Click += new EventHandler(tsiSame_Click);
            this.tsiDown.Click += new EventHandler(tsiDown_Click);
            this.tsiDelete.Click += new EventHandler(tsiDelete_Click);

            this.tvTitle.MouseDown += new MouseEventHandler(tvTitle_MouseDown);
            this.tvTitle.AfterSelect += new TreeViewEventHandler(tvTitle_AfterSelect);
        }
        protected void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string ItemName = e.ClickedItem.Name;
            switch (ItemName)
            {
                case "AddSame":
                    this.AddSame();
                    break;
                case "AddChild":
                    this.AddChild();
                    break;
                case "Save":
                    SaveAccData();
                    break;
                case "Delete":
                    this.DeleteCostProject();
                    break;
                default:
                    break;
            }
        }

        void AssControl_CheckedChanged(object sender, EventArgs e)
        {
            AssControl(sender as CheckBox);
        }

        void tvTitle_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tnCurr = e.Node;
            GetTitleDetail();
        }
        CheckBox lastSelect;

        /// <summary>
        /// 辅助核算控件关系控制
        /// </summary>
        void AssControl(CheckBox ctr)
        {
            int count = 0;
            object[] ctrs = null;
            ctrs = new object[] { };
            ObjectLock.Lock(ctrs);
            foreach (Object o in ctrs)
            {
                CheckBox co = o as CheckBox;
                if (co.Checked)
                {
                    lastSelect = co;
                    co.Enabled = true;
                    count++;
                }
                if (ctr.Name == co.Name)
                    ctr.Enabled = true;
            }
            if (count == 0)
            {
                ObjectLock.Unlock(ctrs);
            }
            if (count > 1) return;

            if (count == 2)
            {
                foreach (Object o in ctrs)
                {
                    if (!(o as CheckBox).Checked)
                    {
                        (o as CheckBox).Enabled = false;
                    }
                }
            }

            if (lastSelect == null) return;
        }

        void GetTitleDetail()
        {
            errInfo.Clear();
            CostProject title = tnCurr.Tag as CostProject;

            this.txtName.Text = title.Name;
            this.txtCode.Text = title.Code;
            this.txtUnit.Text = title.Unit;
            this.commonCostType1.Text = EnumUtil<enmCostType>.GetDescription(title.CostType);
            if (title.InfluxCodex == 1)
            {
                this.rdBtnAdd.Checked = true;
            }
            else
            {
                this.rdBtnminus.Checked = true;
            }
            this.commonCostProjectType1.Text = EnumUtil<enmCostProjectType>.GetDescription(title.CostProjectType);

            try
            {
                commonAccTitle1.SelectedAccTitle = null;
                this.commonAccTitle1.Result.Clear();
                this.commonAccTitle1.Result.Add(title.AccTitle);
                this.commonAccTitle1.Text = title.AccTitle.Name;
            }
            catch (Exception ex)
            {
                this.commonAccTitle1.Result.Clear();
                this.commonAccTitle1.Text = "";

            }
            try
            {
                this.txtExpenseItem.SelectedText = title.ExpenseItem.Name;
                this.txtExpenseItem.SelectedItem = title.ExpenseItem;
            }
            catch (Exception ex)
            {
                this.txtExpenseItem.SelectedText = "";
                this.txtExpenseItem.SelectedItem = null;

            }

            this.tboxDescribe.Text = title.Describe;
            if (title.ExpenseItem != null)
                this.txtExpenseItem.Text = title.ExpenseItem.Name;
            else
                this.txtExpenseItem.Text = "";
        }

        void tvTitle_MouseDown(object sender, MouseEventArgs e)
        {
            tnCurr = tvTitle.GetNodeAt(e.X, e.Y);
            if (tnCurr == null) return;
            tvTitle.SelectedNode = tnCurr;
        }
        public override bool SaveView()
        {
            SaveAccData();
            this.RefreshControls(MainViewState.Browser);
            return true;
        }
        public override bool ModifyView()
        {
            ActionType = "Modify";
            base.RefreshControls(MainViewState.Modify);
            return true;
        }

        public override bool DeleteView()
        {
            DeleteCostProject();
            RefreshControls(MainViewState.Browser);
            return true;
        }
        public override bool CancelView()
        {
            switch (ViewState)
            {
                case MainViewState.Modify:
                    break;
                default:
                    break;
            }
            return true;
        }
        private bool ViewToModel()
        {
            return true;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            SaveAccData();
        }

        private void SaveAccData()
        {
            try
            {
                if (ActionType.Equals("AddSame") || ActionType.Equals("AddChild"))
                {
                    AddNewTitle();
                }
                else if (ActionType.Equals("Modify"))
                {
                    UpdateTitle();
                }
            }
            catch (Exception exp)
            {
                StaticMethods.ShowMessage(StaticMethods.ExceptionMessage(exp));
            }
        }

        private void AddNewTitle()
        {
            curTitle = new CostProject();

            SaveNodeDetail(curTitle);


            TreeNode tnNew = new TreeNode();
            tnNew.Text = this.txtCode.Text + "  " + this.txtName.Text;
            tnNew.Name = this.txtCode.Text;



            #region 新增同级节点

            if (ActionType.Equals("AddSame"))
            {
                if (tnCurr != null && tnCurr.Parent != null)
                {
                    CostProject parentTitle = tnCurr.Parent.Tag as CostProject;
                    curTitle.ParentNode = parentTitle;
                    curTitle.Unit = this.txtUnit.Text;
                    if (!ValidateData(curTitle))
                    {
                        return;
                    }
                    string t = "";
                    curTitle.AccLevelName = t.PadLeft(curTitle.ParentNode.Level * 2, ' ') + curTitle.Name;

                    model.SaveCostProject(curTitle);
                    tnCurr.Parent.Nodes.Add(tnNew);
                    tnCurr.Tag = curTitle.ParentNode as CostProject;
                }
                else
                {
                    CostProject parentTitle = tnRoot.Tag as CostProject;
                    curTitle.ParentNode = parentTitle;
                    if (!ValidateData(curTitle))
                    {
                        return;
                    }
                    curTitle = model.SaveCostProject(curTitle);
                    this.tvTitle.Nodes.Add(tnNew);
                    tnCurr.Tag = curTitle.ParentNode as CostProject;
                }
            }
            #endregion

            #region 新增下级节点

            if (ActionType.Equals("AddChild"))
            {
                CostProject parentTitle = tvTitle.SelectedNode.Tag as CostProject;
                curTitle.ParentNode = parentTitle;
                curTitle.TheTree = parentTitle.TheTree;
                curTitle.Unit = this.txtUnit.Text;
                if (!ValidateData(curTitle))
                {
                    return;
                }
                string pre = "";
                curTitle.AccLevelName = pre.PadLeft(2, ' ') + curTitle.Name;
                curTitle = model.SaveCostProject(curTitle);
                tnCurr.Tag = curTitle.ParentNode as CostProject;
                tnCurr.Nodes.Add(tnNew);
            }

            #endregion

            tnNew.Tag = curTitle;
            lstTitles.Add(curTitle);
            this.tvTitle.SelectedNode = tnNew;
        }

        /// <summary>
        /// 更新科目
        /// </summary>
        void UpdateTitle()
        {
            CostProject curTitle = tnCurr.Tag as CostProject;
            SaveNodeDetail(curTitle);

            if (!ValidateData(curTitle))
            {
                return;
            }
            string t = "";
            curTitle.AccLevelName = t.PadLeft(curTitle.ParentNode.Level * 2, ' ') + curTitle.Name;
            curTitle = model.SaveCostProject(curTitle);
            tnCurr.Tag = curTitle;
            tnCurr.Text = curTitle.Code + "  " + curTitle.Name;
        }

        private void SaveNodeDetail(CostProject title)
        {
            title.Code = this.txtCode.Text;//成本项目编码
            title.Name = this.txtName.Text;//科目名称
            title.Unit = this.txtUnit.Text;
            title.CostType = EnumUtil<enmCostType>.FromDescription(this.commonCostType1.Text);
            if (rdBtnAdd.Checked == true)
            {
                title.InfluxCodex = 1;
            }
            else
            {
                title.InfluxCodex = -1;
            }
            title.CostProjectType = EnumUtil<enmCostProjectType>.FromDescription(this.commonCostProjectType1.Text);
            try
            {
                if (commonAccTitle1.Text != "" && commonAccTitle1.Result.Count != 0)
                {
                    title.AccTitle = commonAccTitle1.SelectedAccTitle;
                }
                else
                {
                    title.AccTitle = null;
                    commonAccTitle1.Text = "";
                }
            }
            catch (Exception ex)
            {
                title.AccTitle = null;
            }
            if (this.txtExpenseItem.Items.Count > 0 && this.txtExpenseItem.Text != "")
            {
                title.ExpenseItem = this.txtExpenseItem.Items[0] as ExpenseItem;
            }
            else
            {
                title.ExpenseItem = null;
            }
            title.Describe = this.tboxDescribe.Text;
        }


        void tsiCopy_Click(object sender, EventArgs e)
        {
            ActionType = "Copy";
        }

        void tsiDelete_Click(object sender, EventArgs e)
        {
            DeleteCostProject();
        }

        /// <summary>
        /// 删除成本项目
        /// </summary>
        private void DeleteCostProject()
        {
            ActionType = "Delete";
            string text = "";
            CostProject currTitle = tnCurr.Tag as CostProject;
            if (currTitle.CategoryNodeType == NodeType.LeafNode)
            {
                text = "删除当前选中的成本项目吗?";
                if (model.IsReferByCostAccount(currTitle.Id))
                {
                    MessageBox.Show("该成本项目已经被使用，不能删除！", "提示信息");
                    return;
                }

            }
            else
            {
                MessageBox.Show("请选择子节点进行删除！", "提示信息");
                return;
            }



            if (!StaticMethods.ConfirmMessage(text)) return;
            try
            {
                CostProject parTitle = model.DeleteCostProject(currTitle);
                TreeNode tnNextNode = tnCurr.NextNode;
                TreeNode tnPrevNode = tnCurr.PrevNode;

                IList lstRemove = new ArrayList();

                tvTitle.GetALLChildNodes(tnCurr, lstRemove);

                foreach (TreeNode tn in lstRemove)
                {

                    CostProject acc = tn.Tag as CostProject;
                    lstTitles.Remove(acc);
                }

                if (tnCurr.Parent != null)
                {
                    tnCurr.Parent.Tag = parTitle;
                    tnCurr.Parent.Nodes.Remove(tnCurr);
                }
                else
                {
                    tvTitle.Nodes.Remove(tnCurr);
                }

                if (tnNextNode != null)
                {
                    tvTitle.SelectedNode = tnNextNode;
                }
                else if (tnPrevNode != null)
                {
                    tvTitle.SelectedNode = tnPrevNode;
                }
            }
            catch (Exception exp)
            {
                StaticMethods.ShowErrorMessage("删除科目出错：" + StaticMethods.ExceptionMessage(exp));
            }
        }



        void tsiDown_Click(object sender, EventArgs e)
        {
            AddChild();
        }

        private void AddChild()
        {
            object[] os = new object[] { };
            ObjectLock.Lock(os);

            ActionType = "AddChild";
            base.RefreshState(MainViewState.AddNew);
            txtCode.Focus();
        }

        void tsiSame_Click(object sender, EventArgs e)
        {
            AddSame();
        }

        private void AddSame()
        {
            ObjectLock.Unlock(pnlFloor, true);
            object[] os = new object[] { };
            CostProject title = tnCurr.Tag as CostProject;
            if (title.ParentNode != null)
            {
                if (title.ParentNode.CategoryNodeType != NodeType.RootNode)
                {
                    ObjectLock.Lock(os);
                }
            }
            ActionType = "AddSame";
            this.RefreshState(MainViewState.AddNew);
            txtCode.Focus();
        }

        /// <summary>
        /// 启动视图
        /// </summary>
        // public override void StartView()
        public void Start()
        {
            GetAccountTitles();
            GetAccountDic();
            BindTreeByAccType();
            //InitData();
            //base.ShowDialog();
        }

        /// <summary>
        /// 修改视图
        /// </summary>
        /// <returns></returns>
        //public override bool ModifyView()
        //{
        //    return base.ModifyView();
        //}

        /// <summary>
        /// 取消视图
        /// </summary>
        /// <returns></returns>
        //public override bool CancelView()
        //{
        //    return base.CancelView();
        //}

        /// <summary>
        /// 初始化数据
        /// </summary>
        //void InitData()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("DictionaryId", typeof(Int32));
        //        dt.Columns.Add("Name", typeof(string));
        //        dt.Columns.Add("Describe", typeof(string));
        //        dt.Rows.Clear();





        //    }
        //    catch (Exception exp)
        //    {
        //        StaticMethods.ShowMessage(StaticMethods.ExceptionMessage(exp));
        //    }

        //}




        /// <summary>
        /// 获取会计科目分类树
        /// </summary>
        void GetAccountTitles()
        {
            lstTitles.Clear();
            lstTitles = model.GetCostProjects();
            if (lstTitles.Count == 0)
            {
                StaticMethods.ShowMessage("未定义会计科目信息！");
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
                if (count == 0)
                {
                    CostProject cp = new CostProject();
                    cp.Name = "新建";
                    cp.Code = "01";
                    cp.CostType = enmCostType.RawMaterial;
                    cp.CostProjectType = enmCostProjectType.elses;
                    TreeNode TRoot = new TreeNode();
                    TRoot.Tag = cp;
                    TRoot.Name = cp.Code;
                    TRoot.Text = cp.Name;
                    tvTitle.Nodes.Add(TRoot);
                    return;
                }
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
                    this.GetTitleDetail();
                }
            }
            catch (Exception exp)
            {
                StaticMethods.AppExceptionHandle(exp, "101");
            }

        }

        #endregion


        /// <summary>
        /// 相关校验
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool ValidateData(CostProject title)
        {
            bool isValid = true;
            errInfo.Clear();

            #region 重要属性检查
            if (txtCode.Text.Trim().Equals(""))
            {
                errInfo.SetError(txtName, "请填写成本项目编码");
                isValid = false;
            }

            if (txtName.Text.Trim().Equals(""))
            {
                errInfo.SetError(txtName, "请填写成本项目名称");
                isValid = false;
            }

            #endregion

            if (!model.ValidateCostProjectCode(title))
            {
                errInfo.SetError(txtCode, "成本项目编码已存在");
                isValid = false;
            }

            return isValid;
        }

        private void VAccountTitle_Load(object sender, EventArgs e)
        {

        }

    }
}