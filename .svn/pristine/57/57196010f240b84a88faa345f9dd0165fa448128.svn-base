using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using Application.Business.Erp.Financial.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using Application.Business.Erp.Financial.Client.Basic.CommonClass;
using Application.Business.Erp.Financial.FIUtils;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.FinancialResource.RelateClass;
using Application.Business.Erp.Financial.InitialData.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;
using System.Data.OleDb;
using VirtualMachine.Component.Util;
using C1.Win.C1FlexGrid;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using ObjectLock = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ObjectLock;

namespace Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI
{
    /// <summary>
    /// 会计科目
    /// </summary>
    public partial class VAccountTitle : TBasicDataView
    {
        IList lstTitles = new ArrayList();
        IList lstCurrency = new ArrayList();
        IList lstDeskAcc = new ArrayList();
        MAccountTitle model;
        AccountTitle curTitle = null;
        TreeNode tnRoot = null;
        TreeNode tnCurr = null;
        string ActionType = "";
        AccountLevel accLevel;

        AccountTitle parenttitle = null;

        //根节点
        string rootId = "0";

        public VAccountTitle(MAccountTitle model)
        {
            this.model = model;
            InitializeComponent();
            InitEvents();
            //if (ConstObject.FrameWorkNewFlag)
            //{
            //    this.UserCode = LoginInfomation.LoginInfo.ThePerson.Code;
            //    this.BarKeyCode = "AccountTitle";
            //}
        }

        private void InitEvents()
        {
            this.tsiSame.Click += new EventHandler(tsiSame_Click);
            this.tsiDown.Click += new EventHandler(tsiDown_Click);
            this.tsiDelete.Click += new EventHandler(tsiDelete_Click);
            this.tsiCopy.Click += new EventHandler(tsiCopy_Click);
            this.tsiFreeze.Click += new EventHandler(tsiFreeze_Click);

            this.tvTitle.MouseDown += new MouseEventHandler(tvTitle_MouseDown);
            this.tvTitle.AfterSelect += new TreeViewEventHandler(tvTitle_AfterSelect);
            this.chkQuantityAccount.CheckedChanged += new EventHandler(chkQuantityAccount_CheckedChanged);
            this.chkForeignAcc.CheckedChanged += new EventHandler(chkForeignAcc_CheckedChanged);
            this.chkClient.CheckedChanged += new EventHandler(AssControl_CheckedChanged);
            this.chkDepartmentAcc.CheckedChanged += new EventHandler(AssControl_CheckedChanged);
            this.chkProject.CheckedChanged += new EventHandler(AssControl_CheckedChanged);
            this.chkSupplier.CheckedChanged += new EventHandler(AssControl_CheckedChanged);
            this.chkPersonAcc.CheckedChanged += new EventHandler(AssControl_CheckedChanged);
        }

        //protected override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        protected void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //   base.ToolStrip_ItemClicked(sender, e);
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
                    this.DeleteAccountTitle();
                    break;
                case "Freeze":
                    if (FreezeAcc())
                    {
                        // StaticMethods.ShowMessage("冻结成功");
                        GetTitleDetail();

                    }
                    break;
                case "UnFreeze":
                    if (UnFreezeAcc())
                    {
                        //  StaticMethods.ShowMessage("解冻成功");
                        GetTitleDetail();
                    }
                    break;
                default:
                    break;
            }
        }

        bool FreezeAcc()
        {
            string text = "";
            AccountTitle currTitle = tnCurr.Tag as AccountTitle;
            if (currTitle.CategoryNodeType == NodeType.LeafNode)
            {
                text = "冻结当前选中的科目吗?";
            }
            else
            {
                text = "冻结当前选中的科目吗？该操作将连它的所有子节点一同冻结！";
            }

            if (!StaticMethods.ConfirmMessage(text)) return false;

            IList lstNodes = model.FreezeAccountTitle(currTitle);

            IList lstTreeNodes = new ArrayList();
            tvTitle.GetALLChildNodes(tnCurr, lstTreeNodes);
            foreach (TreeNode tn in lstTreeNodes)
            {
                AccountTitle t1 = tn.Tag as AccountTitle;
                foreach (AccountTitle t2 in lstNodes)
                {
                    if (t1.Id == t2.Id)
                    {
                        tn.Tag = t2;
                        break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 解冻科目
        /// </summary>
        /// <returns></returns>
        bool UnFreezeAcc()
        {
            string text = "";
            AccountTitle currTitle = tnCurr.Tag as AccountTitle;
            if (currTitle.CategoryNodeType == NodeType.LeafNode)
            {
                text = "解冻当前选中的科目吗?";
            }
            else
            {
                text = "解冻当前选中的科目吗？该操作将连它的所有子节点一同解冻！";
            }
            if (!StaticMethods.ConfirmMessage(text)) return false;


            IList lstNodes = model.UnFreezeAccountTitle(currTitle);

            IList lstTreeNodes = new ArrayList();
            tvTitle.GetALLChildNodes(tnCurr, lstTreeNodes);

            foreach (TreeNode tn in lstTreeNodes)
            {
                AccountTitle t1 = tn.Tag as AccountTitle;
                foreach (AccountTitle t2 in lstNodes)
                {
                    if (t1.Id == t2.Id)
                    {
                        tn.Tag = t2;
                        break;
                    }
                }
            }
            return true;
        }

        void tsiFreeze_Click(object sender, EventArgs e)
        {
            if (tsiFreeze.Text.Equals("冻结"))
            {
                if (FreezeAcc())
                {
                    tsiFreeze.Text = "解冻";
                    StaticMethods.ShowMessage("冻结成功");
                    GetTitleDetail();
                }
            }
            else
            {
                if (UnFreezeAcc())
                {
                    tsiFreeze.Text = "冻结";
                    GetTitleDetail();
                    StaticMethods.ShowMessage("解冻成功");
                }
            }
        }


        void AssControl_CheckedChanged(object sender, EventArgs e)
        {
            AssControl(sender as CheckBox);
        }

        void chkForeignAcc_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkForeignAcc.Checked)
            {
                Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ObjectLock.Lock(cbbForeignCurr);
                cbbForeignCurr.SelectedIndex = -1;
            }
            else if (chkForeignAcc.Checked && chkForeignAcc.Enabled)
            {
                ObjectLock.Unlock(cbbForeignCurr);
            }
        }

        void chkQuantityAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkQuantityAccount.Checked)
            {
                ObjectLock.Lock(new object[] { txtQuanDesc, txtUnit });
                txtQuanDesc.Text = txtUnit.Text = "";
            }
            else if (chkQuantityAccount.Checked && chkQuantityAccount.Enabled)
            {
                ObjectLock.Unlock(new object[] { txtQuanDesc, txtUnit });
            }
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
            ctrs = new object[] { chkPersonAcc, chkProject, chkClient, chkSupplier, chkDepartmentAcc };
            ObjectLock.Lock(ctrs);
            //if (!btnSave.Enabled) return;
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
            switch (lastSelect.Name)
            {
                case "chkPersonAcc":
                    ctrs = new object[] { chkProject };
                    ObjectLock.Unlock(ctrs);
                    break;
                case "chkDepartmentAcc":
                    ctrs = new object[] { chkClient, chkSupplier, chkProject };
                    ObjectLock.Unlock(ctrs);
                    break;
                case "chkClient":
                    ctrs = new object[] { chkDepartmentAcc, chkProject };
                    ObjectLock.Unlock(ctrs);
                    break;
                case "chkSupplier":
                    ctrs = new object[] { chkDepartmentAcc, chkProject };
                    ObjectLock.Unlock(ctrs);
                    break;
                case "chkProject":
                    ctrs = new object[] { chkPersonAcc, chkClient, chkSupplier, chkDepartmentAcc };
                    ObjectLock.Unlock(ctrs);
                    break;
                default:
                    break;
            }
        }


        void GetTitleDetail()
        {
            bool isReferByVoucher = false;
            errInfo.Clear();

            AccountTitle title = tnCurr.Tag as AccountTitle;
            //MessageBox.Show(title.Version.ToString());

            isReferByVoucher = model.IsReferByVoucher(title.AccountCode);

            this.txtName.Text = title.Name;
            this.txtCode.Text = title.Code;
            this.txtAssisCode.Text = title.AssisCode;
            this.txtUnit.Text = title.QuantityUnit;
            txtQuanDesc.Text = title.QuantityDesc;
            this.cbbAccType.SelectedValue = title.AccType;
            this.cbbAboutCash.SelectedValue = title.AboutCash;
            if (title.BalanceDire == 0)
                this.rbJf.Checked = true;
            else
                this.rbDf.Checked = true;
            try
            {
                this.cbbForeignCurr.SelectedValue = title.ForeignCurrency.Id;
            }
            catch
            {
                this.cbbForeignCurr.SelectedIndex = -1;
            }
            try
            {
                this.cbbDeskAccount.SelectedValue = title.DeskAcc.Id;
            }
            catch { }

            this.cbbShowStyle.SelectedValue = FinanceUtil.GetBookStyleInt(title.ShowStyle);
            this.chkBank.Checked = title.BankAccBook;
            this.chkClient.Checked = title.ClientAccount;
            this.chkDaily.Checked = title.DailyAccBook;
            this.chkDepartmentAcc.Checked = title.DepartmentAccount;
            this.chkForeignAcc.Checked = title.ForeignAccount;
            this.chkPersonAcc.Checked = title.PersonAccount;
            this.chkProject.Checked = title.ProjectAccount;
            this.chkSupplier.Checked = title.SupplierAccount;
            this.chkBudget.Checked = title.BudgetManage;
            this.chkEndorsement.Checked = title.EndorsementManage;
            this.chkQuantityAccount.Checked = title.QuantityAccount;

            if (title.FreezeAccount)
            {
                ActionType = "ReadOnly";
                ObjectLock.Lock(pnlFloor, true);
            }
            else
            {
                object[] os = null;

                if (title.CategoryNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                {
                    ObjectLock.Unlock(pnlFloor, true);
                    os = new object[] { txtCode };
                    ObjectLock.Lock(os);
                    if (title.ParentNode.CategoryNodeType == NodeType.RootNode)
                    {
                        cbbAccType.Enabled = true;
                    }
                    else
                    {
                        cbbAccType.Enabled = false;
                    }
                    ActionType = "Modify";
                }
                else
                {
                    ActionType = "ReadOnly";
                    ObjectLock.Lock(pnlFloor, true);
                }
            }
            if (isReferByVoucher)
            {
                ObjectLock.Lock(new object[] { chkClient, chkDepartmentAcc, chkForeignAcc, chkPersonAcc, chkProject, chkQuantityAccount, chkSupplier, txtQuanDesc, this.cbbForeignCurr, txtUnit });
            }
        }

        void tvTitle_MouseDown(object sender, MouseEventArgs e)
        {
            tnCurr = tvTitle.GetNodeAt(e.X, e.Y);
            if (tnCurr == null) return;
            tvTitle.SelectedNode = tnCurr;
            if (!(tnCurr.Tag as AccountTitle).FreezeAccount)
            {
                tsiDelete.Enabled = tsiCopy.Enabled = tsiDown.Enabled = tsiSame.Enabled = true;
                tsiFreeze.Text = "冻结";
            }
            else
            {
                tsiFreeze.Text = "解冻";
                tsiDelete.Enabled = tsiCopy.Enabled = tsiDown.Enabled = tsiSame.Enabled = false;
            }
        }
        //public override bool NewView()
        //{
        //    //mConsignOrder.New();
        //    //ConsignOrder = new ConsignOrder();

        //    //this.ClearView();
        //    //this.txtCreatePerson.Text = ConstObject.TheLogin.ThePerson.Name;
        //    //this.txtCreateDate.Text = ConstObject.TheLogin.LoginDate.ToShortDateString();
        //    this.RefreshControls(MainViewState.AddNew);
        //    return true;
        //}

        public override bool SaveView()
        {
            //this.grdDtl.FindForm().Validate();

            //if (ViewToModel())
            //{
            //    ConsignOrder = mConsignOrder.Save(ConsignOrder);
            //    this.ViewCaption = "来料加工合同-" + ConsignOrder.Code;
            //    this.txtCode.Text = ConsignOrder.Code;

            SaveAccData();
            this.RefreshControls(MainViewState.Browser);
            return true;
            //}

            return false;

        }

        public override bool ModifyView()
        {
            //ConsignOrder = mConsignOrder.Reload(ConsignOrder);

            //ModelToView();
            base.RefreshControls(MainViewState.Modify);

            return true;
        }

        public override bool DeleteView()
        {
            //mConsignOrder.Delete(ConsignOrder);
            // ClearView();
            DeleteAccountTitle();
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
                    //ClearView();
                    break;

            }
            // this.ModelToView();
            return true;
        }


        private bool ViewToModel()
        {
            //if (!VerifyView())
            //    return false;

            //BuildMaster();
            //BuildMaterialDetail();
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
            curTitle = new AccountTitle();

            SaveNodeDetail(curTitle);


            TreeNode tnNew = new TreeNode();
            tnNew.Text = this.txtCode.Text + "  " + this.txtName.Text;
            tnNew.Name = this.txtCode.Text;


            #region 新增同级节点

            if (ActionType.Equals("AddSame"))
            {

                if (tnCurr != null && tnCurr.Parent != null)
                {
                    AccountTitle parentTitle = tnCurr.Parent.Tag as AccountTitle;
                    curTitle.ParentNode = parentTitle;
                    if (!ValidateData(curTitle))
                    {
                        return;
                    }
                    model.SaveAccountTitle(curTitle);
                    tnCurr.Parent.Nodes.Add(tnNew);
                    //tnCurr.Tag = curTitle.ParentNode as AccountTitle;
                }
                else
                {
                    AccountTitle parentTitle = tnRoot.Tag as AccountTitle;
                    curTitle.ParentNode = parentTitle;
                    if (!ValidateData(curTitle))
                    {
                        return;
                    }
                    curTitle = model.SaveAccountTitle(curTitle);
                    this.tvTitle.Nodes.Add(tnNew);
                    tnCurr.Tag = curTitle.ParentNode as AccountTitle;
                }
            }
            #endregion

            #region 新增下级节点

            if (ActionType.Equals("AddChild"))
            {
                AccountTitle parentTitle = tvTitle.SelectedNode.Tag as AccountTitle;
                curTitle.ParentNode = parentTitle;
                if (!ValidateData(curTitle))
                {
                    return;
                }
                curTitle = model.SaveAccountTitle(curTitle);
                tnCurr.Tag = curTitle.ParentNode as AccountTitle;
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
            AccountTitle curTitle = tnCurr.Tag as AccountTitle;
            SaveNodeDetail(curTitle);

            if (!ValidateData(curTitle))
            {
                return;
            }
            curTitle = model.SaveAccountTitle(curTitle);
            tnCurr.Tag = curTitle;
            tnCurr.Text = curTitle.AccountCode + "  " + curTitle.Name;
        }

        private void SaveNodeDetail(AccountTitle title)
        {
            title.Code = this.txtCode.Text;//科目编码
            title.Name = this.txtName.Text;//科目名称

            //现金科目
            title.AboutCash = FinanceUtil.GetCashAccTitleByInt(int.Parse(this.cbbAboutCash.SelectedValue.ToString()));
            title.AccountCode = this.txtCode.Text;//科目编码
            //科目类型
            title.AccType = FinanceUtil.GetAccountType(int.Parse(this.cbbAccType.SelectedValue.ToString()));
            title.AssisCode = this.txtAssisCode.Text.Trim();//助记码
            try
            {
                //余额方向
                title.BalanceDire = (this.rbJf.Checked) ? 0 : 1;
            }
            catch { title.BalanceDire = 0; }
            title.BankAccBook = this.chkBank.Checked;//银行账
            title.BudgetManage = this.chkBudget.Checked;//预算控制
            title.ClientAccount = this.chkClient.Checked;//客户关系核算
            title.DailyAccBook = this.chkDaily.Checked;//日记账
            title.DepartmentAccount = this.chkDepartmentAcc.Checked;//部门核算
            title.EndorsementManage = this.chkEndorsement.Checked; //背书人管理
            title.ForeignAccount = this.chkForeignAcc.Checked;//核算外币
            title.PartnerAccount = (title.ClientAccount || title.SupplierAccount); //伙伴实体核算
            title.PersonAccount = this.chkPersonAcc.Checked; //个人核算
            title.ProjectAccount = this.chkProject.Checked; //项目核算
            title.ShowStyle = FinanceUtil.GetBookStyleById(int.Parse(this.cbbShowStyle.SelectedValue.ToString())); //账页格式
            title.SupplierAccount = this.chkSupplier.Checked; //供应关系核算


            title.QuantityAccount = this.chkQuantityAccount.Checked;//数量核算
            title.QuantityUnit = this.txtUnit.Text.Trim();//计量单位
            title.QuantityDesc = this.txtQuanDesc.Text.Trim();//规格型号

            try
            {
                //台账类别
                title.DeskAcc = GetDeskAccountById(this.cbbDeskAccount.SelectedValue.ToString());
            }
            catch { }
            try
            {
                //核算外币币种
                title.ForeignCurrency = GetCurrencyById(this.cbbForeignCurr.SelectedValue.ToString());
            }
            catch { }
        }

        CurrencyInfo GetCurrencyById(string id)
        {
            foreach (CurrencyInfo curr in lstCurrency)
            {
                if (curr.Id == id)
                {
                    return curr;
                }
            }
            return null;
        }

        DeskAccount GetDeskAccountById(string id)
        {
            foreach (DeskAccount curr in lstDeskAcc)
            {
                if (curr.Id == id)
                {
                    return curr;
                }
            }
            return null;
        }

        void tsiCopy_Click(object sender, EventArgs e)
        {
            ActionType = "Copy";
        }

        void tsiDelete_Click(object sender, EventArgs e)
        {
            DeleteAccountTitle();
        }

        /// <summary>
        /// 删除科目(冻结科目)
        /// </summary>
        private void DeleteAccountTitle()
        {
            ActionType = "Delete";
            string text = "";
            AccountTitle currTitle = tnCurr.Tag as AccountTitle;
            if (currTitle.CategoryNodeType == NodeType.LeafNode)
            {
                text = "删除当前选中的科目吗?";
            }
            else
            {
                text = "删除当前选中的科目吗？该操作将连它的所有子节点一同删除！";
            }
            if (!StaticMethods.ConfirmMessage(text)) return;

            try
            {
                AccountTitle parTitle = model.DeleteAccountTitle(currTitle);
                TreeNode tnNextNode = tnCurr.NextNode;
                TreeNode tnPrevNode = tnCurr.PrevNode;

                IList lstRemove = new ArrayList();

                tvTitle.GetALLChildNodes(tnCurr, lstRemove);

                foreach (TreeNode tn in lstRemove)
                {
                    AccountTitle acc = tn.Tag as AccountTitle;
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
            ObjectLock.Unlock(pnlFloor, true);
            object[] os = new object[] { cbbAccType };
            ObjectLock.Lock(os);


            txtName.Text = txtAssisCode.Text = "";
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
            object[] os = new object[] { cbbAccType };
            AccountTitle title = tnCurr.Tag as AccountTitle;
            if (title.ParentNode.CategoryNodeType != NodeType.RootNode)
            {
                ObjectLock.Lock(os);
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
            InitData();
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
        void InitData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("DictionaryId", typeof(Int32));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Describe", typeof(string));
                dt.Rows.Clear();

                TabPage tbAll = new TabPage("全部");
                tbAll.Name = "tbAll";
                tbAll.Tag = "All";
                tc.TabPages.Add(tbAll);

                foreach (int i in Enum.GetValues(typeof(AccountType)))
                {
                    if (i < 6)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = i;
                        dr[1] = Enum.GetName(typeof(AccountType), i);
                        dr[2] = FinanceUtil.GetAccountTypeByValue(i);
                        dt.Rows.Add(dr);

                        TabPage tp = new TabPage(dr[2].ToString());
                        tp.Name = "tp" + dr[1].ToString();
                        tp.Tag = dr[1].ToString();
                        tc.TabPages.Add(tp);
                    }
                }
                this.cbbAccType.DataSource = dt;
                this.cbbAccType.DisplayMember = "Describe";
                this.cbbAccType.ValueMember = "DictionaryId";



                #region 加载账页格式

                DataTable dt1 = dt.Clone();

                foreach (int i in Enum.GetValues(typeof(BookStyle)))
                {
                    DataRow dr = dt1.NewRow();
                    dr[0] = i;
                    dr[1] = Enum.GetName(typeof(BookStyle), i);
                    dr[2] = FinanceUtil.GetBookStyleStrByInt(i);
                    dt1.Rows.Add(dr);
                }
                this.cbbShowStyle.DataSource = dt1;
                this.cbbShowStyle.DisplayMember = "Describe";
                this.cbbShowStyle.ValueMember = "DictionaryId";

                #endregion

                #region 加载现金科目
                DataTable dt2 = dt.Clone();

                foreach (int i in Enum.GetValues(typeof(CashAccTitle)))
                {
                    DataRow dr = dt2.NewRow();
                    dr[0] = i;
                    dr[1] = Enum.GetName(typeof(CashAccTitle), i);
                    dr[2] = FinanceUtil.GetCashAccTitleStrByInt(i);
                    dt2.Rows.Add(dr);
                }
                this.cbbAboutCash.DataSource = dt2;
                this.cbbAboutCash.DisplayMember = "Describe";
                this.cbbAboutCash.ValueMember = "DictionaryId";

                #endregion

                //#region 加载币种信息
                //lstCurrency = model.GetCurrencyList();
                //if (lstCurrency.Count > 0)
                //{
                //    this.cbbForeignCurr.DataSource = lstCurrency;
                //    this.cbbForeignCurr.ValueMember = "Id";
                //    this.cbbForeignCurr.DisplayMember = "Name";
                //}
                //#endregion


                #region 加载台账列表信息

                lstDeskAcc = model.GetDeskAccounts();
                if (lstDeskAcc.Count > 0)
                {
                    this.cbbDeskAccount.DataSource = lstDeskAcc;
                    this.cbbDeskAccount.ValueMember = "Id";
                    this.cbbDeskAccount.DisplayMember = "Name";
                }

                #endregion

                accLevel = model.GetAccountLevel();

                tc.SelectedIndexChanged += new EventHandler(tc_SelectedIndexChanged);
                tc.SelectedIndex = 0;
                tc_SelectedIndexChanged(null, null);
            }
            catch (Exception exp)
            {
                StaticMethods.ShowMessage(StaticMethods.ExceptionMessage(exp));
            }

        }


        void tc_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tp = tc.TabPages[tc.SelectedIndex] as TabPage;
            if (tp.Name.Equals("tpLast")) return;
            BindTreeByAccType(tp.Tag.ToString());
        }



        /// <summary>
        /// 获取会计科目分类树
        /// </summary>
        void GetAccountTitles()
        {
            lstTitles.Clear();
            lstTitles = model.GetAccountTitles();
            if (lstTitles.Count == 0)
            {
                StaticMethods.ShowMessage("未定义会计科目信息！");
            }
        }

        #region 构建科目树

        private IDictionary<string, TreeNode> GetAccountDic()
        {
            IDictionary<string, TreeNode> accDic = new Dictionary<string, TreeNode>();
            foreach (AccountTitle cirAcc in lstTitles)
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
        void BindTreeByAccType(string accType)
        {
            try
            {
                tvTitle.Nodes.Clear();
                IDictionary<string, TreeNode> dicTitles = GetAccountDic();
                int count = dicTitles.Count;
                if (count == 0)
                {
                    //tvTitle.Nodes.Add(dicTitles[0]);
                    return;
                }
                foreach (KeyValuePair<string, TreeNode> cirKey in dicTitles)
                {
                    AccountTitle nowAcc = cirKey.Value.Tag as AccountTitle;
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
                    if (nowAcc.AccType.ToString() == accType || accType == "All")
                    {
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
                }

                if (tvTitle.Nodes.Count > 0)
                {
                    this.tvTitle.Focus();
                    //TreeViewEventArgs arg = new TreeViewEventArgs(tvTitle.Nodes[0]);
                    //tvTitle_AfterSelect(tvTitle, arg);
                    tnCurr = tvTitle.Nodes[0];
                    this.GetTitleDetail();
                }
                string levelInfo = "科目级长:  " + accLevel.FirstLev.ToString() + "-" + accLevel.SecondLev.ToString() + "-" + accLevel.ThirdLev.ToString() + "-" + accLevel.FourthLev.ToString() + "-" + accLevel.FifthLev.ToString() + "-" + accLevel.SixthLev.ToString();
                lnkInfo.Text = levelInfo + "  科目个数:  " + count.ToString();

            }
            catch (Exception exp)
            {
                StaticMethods.AppExceptionHandle(exp, "101");
            }


            //try
            //{
            //    TreeNodeUtil.dicNode.Clear();
            //    this.tvTitle.Nodes.Clear();
            //    foreach (object o in lstTitles)
            //    {
            //        AccountTitle title = o as AccountTitle;
            //        if (!accType.Equals("All") && !accType.Equals(title.AccType.ToString())) continue;

            //        if (title.CategoryNodeType == NodeType.RootNode)
            //        {
            //            tnRoot = new TreeNode();
            //            tnRoot.Name = title.Code;
            //            tnRoot.Text = title.Code + "  " + title.Name;
            //            tnRoot.Tag = title;
            //            continue;
            //        }
            //        if (title.State == 0) continue;
            //        TreeNode tn = new TreeNode();
            //        tn.Name = title.Code;
            //        tn.Text = title.Code + "  " + title.Name;
            //        tn.Tag = title;

            //        if (title.ParentNode != null && title.ParentNode.CategoryNodeType != NodeType.RootNode)
            //        {
            //            TreeNode tnp = null;
            //            TreeNodeUtil.GetDic(title.ParentNode.SysCode, out tnp);
            //            tnp.Nodes.Add(tn);
            //            //title.ParentNode.ChildNodes.Add(title);
            //        }
            //        else
            //        {
            //            tvTitle.Nodes.Add(tn);
            //        }
            //count++;
            //        TreeNodeUtil.AddDic(title.SysCode, tn);
            //    }
            //    if (tvTitle.Nodes.Count > 0)
            //    {
            //        this.tvTitle.Focus();
            //        //TreeViewEventArgs arg = new TreeViewEventArgs(tvTitle.Nodes[0]);
            //        //tvTitle_AfterSelect(tvTitle, arg);
            //        tnCurr = tvTitle.Nodes[0];
            //        this.GetTitleDetail();
            //    }

            //}
            //catch (Exception exp)
            //{
            //    Console.WriteLine(count);
            //    StaticMethods.AppExceptionHandle(exp, "101");
            //}

        }

        #endregion


        /// <summary>
        /// 相关校验
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        bool ValidateData(AccountTitle title)
        {
            bool isValid = true;
            errInfo.Clear();

            #region 重要属性检查
            if (txtCode.Text.Trim().Equals(""))
            {
                errInfo.SetError(txtName, "请填写科目编码");
                isValid = false;
            }

            if (txtName.Text.Trim().Equals(""))
            {
                errInfo.SetError(txtName, "请填写科目名称");
                isValid = false;
            }

            #endregion

            #region 校验科目级长是否正确
            if (accLevel != null)
            {
                int currLevel = title.ParentNode.Level;
                int levelLength = 0;
                if (currLevel == 1)
                    levelLength = accLevel.FirstLev;
                else if (currLevel == 2)
                    levelLength = accLevel.SecondLev;
                else if (currLevel == 3)
                    levelLength = accLevel.ThirdLev;
                else if (currLevel == 4)
                    levelLength = accLevel.FourthLev;
                else if (currLevel == 5)
                    levelLength = accLevel.FifthLev;
                else if (currLevel == 6)
                    levelLength = accLevel.SixthLev;

                int parentLength = title.ParentNode.Code.Trim().Length;
                if (title.Code.Length <= parentLength)
                {
                    errInfo.SetError(txtCode, "科目编码长度不符合规定");
                    return false;
                }
                int childLength = title.Code.Substring(parentLength).Length;
                if (childLength != levelLength)
                {
                    errInfo.SetError(txtCode, "科目编码长度不符合规定");
                    return false;
                }
                if (!title.Code.Substring(0, parentLength).Equals(title.ParentNode.Code.Trim()))
                {
                    errInfo.SetError(txtCode, "科目编码必须以上级科目编码开始");
                    return false;
                }
            }
            #endregion

            if (!model.ValidateAccCode(title))
            {
                errInfo.SetError(txtCode, "科目编码已存在");
                isValid = false;
            }

            if (!model.ValidateAssCode(title))
            {
                errInfo.SetError(txtAssisCode, "助记码已存在");
                isValid = false;
            }

            #region 其他属性检查

            if (this.chkForeignAcc.Checked && this.cbbForeignCurr.SelectedIndex == -1)
            {
                errInfo.SetError(cbbForeignCurr, "请选择外币币种");
                isValid = false;
            }

            if (this.chkQuantityAccount.Checked)
            {
                if (this.txtQuanDesc.Text.Trim().Equals(""))
                {
                    errInfo.SetError(txtQuanDesc, "请填写型号规格");
                    isValid = false;
                }
                if (this.txtUnit.Text.Trim().Equals(""))
                {
                    errInfo.SetError(txtUnit, "请填写计量单位");
                    isValid = false;
                }
            }
            #endregion

            return isValid;
        }

        private void VAccountTitle_Load(object sender, EventArgs e)
        {

        }
        //导入事件
        private void lnklblImport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //填充数组
            ArrayList aa = new System.Collections.ArrayList();
            aa.Add("ACCCODE");
            aa.Add("ACCNAME");
            aa.Add("ASSISTCODE");
            aa.Add("ACCTYPE");
            aa.Add("ACCBALANCEDIRE");
            aa.Add("FOREIGNACC");
            aa.Add("ACCLEVELNAME");
            aa.Add("ENDORSEMANAGE");
            aa.Add("ACCCURRENT");
            aa.Add("QUANTITYACC");
            aa.Add("QUANTITYUNIT");
            aa.Add("ACCABOUTCASH");
            aa.Add("BANKACCBOOK");
            aa.Add("DAILYACCBOOK");
            aa.Add("PERSONACC");
            aa.Add("PROJECTACC");
            aa.Add("ACCSHOWSTYLE");
            aa.Add("PARTNERACC");
            aa.Add("ORDERNO");
            aa.Add("CURRID");
            OpenFileDialog openDialog = new OpenFileDialog();
            string dir = Environment.CurrentDirectory;
            openDialog.InitialDirectory = "c:\\";

            openDialog.DefaultExt = "xls";
            openDialog.Filter = "Excel文件|*.xls";
            ////DialogResult drt = MessageBox.Show("请导入正确格式的Excel文档,您确定要导入这个Excel文档里的数据吗？", "友情提示!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                string openFileName = openDialog.FileName;

                //AccountTitle AccountTitle = new AccountTitle();

                int result = setExcelToDB(openFileName, aa);
                MessageBox.Show("成功插入 " + result + " 条记录！");
            }
            Environment.CurrentDirectory = dir;
        }
        /// <summary>
        /// 导入客户信息方法
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="alColumn">数组</param>
        /// <param name="baseModel">实体model</param>
        /// <returns></returns>
        public int setExcelToDB(string filename, ArrayList alColumn)
        {
            //返回插入成功的记录数//
            try
            {
                int acctypes = 0;
                string code = "";
                //DateTime date = new DateTime();
                string subcode = "";
                string strConnection = "";
                strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + filename + ";Extended Properties=Excel 8.0;";
                OleDbConnection olecon = new OleDbConnection();
                olecon.ConnectionString = strConnection;
                olecon.Open();

                string aa = "SELECT * FROM [科目]";
                int numJL = 0;

                OleDbDataAdapter oleapt = new OleDbDataAdapter(aa, olecon);

                DataSet ds = new DataSet();
                oleapt.Fill(ds);
                string quary = "select ACCCODE,ACCNAME from thd_FIACCTITLE where ACCNAME='会计科目'";
                DataSet dt = model.select(quary);
                if (dt.Tables[0].Rows.Count <= 0)
                {
                    string addSql = " insert into thd_FIACCTITLE(ACCCODE,ACCNAME,ASSISTCODE,ACCTYPE,ACCBALANCEDIRE,FOREIGNACC,ACCLEVELNAME,ENDORSEMANAGE,ACCCURRENT,QUANTITYACC,QUANTITYUNIT,ACCABOUTCASH,BANKACCBOOK,DAILYACCBOOK,PERSONACC,PROJECTACC,ACCSHOWSTYLE,PARTNERACC,ORDERNO,CURRID,code,VERSION,PERID,ACCCREATEDATE,CATTREEID,ACCSYSCODE,ACCNODELEVEL) values \n" +
                                    "('','会计科目','',0,0,cast('0' as bit),'',\n" +
                                    "Cast('0' as bit),cast('0' as bit),cast('0' as bit),'',\n" +
                                    " 0,cast('0' as bit),cast('0' as bit),\n" +
                                    "cast('0' as bit),cast('0' as bit),0,'',cast('' as bigint),0,'',1,0,'" + DateTime.Now.Date.ToShortDateString() + "',1,1.,1)";
                    model.inserts(addSql);
                }


                for (int i = 0; i < alColumn.Count; i++)
                {
                    ds.Tables[0].Columns[i].ColumnName = (string)alColumn[i];
                    ds.Tables[0].Columns[i].Caption = (string)alColumn[i];

                }
                //AccountTitle ob = (AccountTitle)baseModel;
                //IList list = new ArrayList();///
                //填充数据对象完毕
                DataTable dtError = ds.Tables[0].Copy();
                bool flag = false;
                bool result = false;
                int s = 0;
                dtError.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString() == null || ds.Tables[0].Rows[i][0].ToString() == "")
                        continue;
                    string selects = "select ACCCODE,ACCTITLEID from thd_FIACCTITLE where ACCCODE='" + ds.Tables[0].Rows[i][0].ToString().Replace(".", "") + "'";
                    DataSet dss = model.select(selects);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    if (!flag)
                    {
                        string acctype = ClientUtil.ToString(ds.Tables[0].Rows[i][3]);
                        switch (acctype)
                        {
                            case "流动资产":
                                acctypes = 0;
                                break;
                            case "非流动资产":
                                acctypes = 0;
                                break;
                            case "流动负债":
                                acctypes = 1;
                                break;
                            case "非流动负债":
                                acctypes = 1;
                                break;
                            case "共同":
                                acctypes = 2;
                                break;
                            case "所有者权益":
                                acctypes = 3;
                                break;
                            case "成本":
                                acctypes = 4;
                                break;
                            case "营业收入":
                                acctypes = 5;
                                break;
                            case "其他收益":
                                acctypes = 5;
                                break;
                            case "营业成本及税金":
                                acctypes = 5;
                                break;
                            case "其他损失":
                                acctypes = 5;
                                break;
                            case "期间费用":
                                acctypes = 5;
                                break;
                            case "所得税":
                                acctypes = 5;
                                break;
                            case "以前年度损益调整":
                                acctypes = 5;
                                break;
                            default:
                                acctypes = 6;
                                break;
                        }
                        string ENDORSEMANAGE = ""; string ACCCURRENT = ""; string QUANTITYACC = ""; string ACCABOUTCASH = ""; string BANKACCBOOK = ""; string DAILYACCBOOK = ""; string PERSONACC = ""; string PROJECTACC = ""; string unit = "";
                        string insertSql = "insert into thd_FIACCTITLE(ACCCODE,ACCNAME,ASSISTCODE,ACCTYPE,ACCBALANCEDIRE,FOREIGNACC,ACCLEVELNAME,ENDORSEMANAGE,ACCCURRENT,QUANTITYACC,QUANTITYUNIT,ACCABOUTCASH,BANKACCBOOK,DAILYACCBOOK,PERSONACC,PROJECTACC,ACCSHOWSTYLE,BELONGCODE,ORDERNO,CURRID,code,VERSION,PERID,ACCCREATEDATE,CATTREEID) values \n" +
                            "('" + ClientUtil.ToString(ds.Tables[0].Rows[i][0]).Replace(".", "") + "','" + ClientUtil.ToString(ds.Tables[0].Rows[i][1]) + "','" + ClientUtil.ToString(ds.Tables[0].Rows[i][2]) + "'," + acctypes + "," + ClientUtil.ToInt(ClientUtil.ToString(ds.Tables[0].Rows[i][4]) == "借" ? "0" : "1") + ",cast('" + (ClientUtil.ToString(ds.Tables[0].Rows[i][5]) == "不核算" ? "0" : "1") + "' as bit),'" + ClientUtil.ToString(ds.Tables[0].Rows[i][6]) + "',\n" +
                            "Cast('" + (ENDORSEMANAGE = ClientUtil.ToString(ds.Tables[0].Rows[i][7]) == "是" ? "1" : "0") + "' as bit),cast('" + (ACCCURRENT = ClientUtil.ToString(ds.Tables[0].Rows[i][8]) == "是" ? "1" : "0") + "' as bit),cast('" + (QUANTITYACC = ClientUtil.ToString(ds.Tables[0].Rows[i][9]) == "是" ? "1" : "0") + "' as bit),'" + (unit = ClientUtil.ToString(ds.Tables[0].Rows[i][10] == null ? "千克" : ds.Tables[0].Rows[i][10])) + "',\n" +
                            " " + ClientUtil.ToInt(ACCABOUTCASH = ClientUtil.ToString(ds.Tables[0].Rows[i][11]) == "是" ? "1" : "0") + ",cast('" + (BANKACCBOOK = ClientUtil.ToString(ds.Tables[0].Rows[i][12]) == "是" ? "1" : "0") + "' as bit),cast('" + (DAILYACCBOOK = ClientUtil.ToString(ds.Tables[0].Rows[i][13]) == "是" ? "1" : "0") + "' as bit),\n" +
                            "cast('" + (PERSONACC = ClientUtil.ToString(ds.Tables[0].Rows[i][14]) == "是" ? "1" : "0") + "' as bit),cast('" + (PROJECTACC = ClientUtil.ToString(ds.Tables[0].Rows[i][15]) == "是" ? "1" : "0") + "' as bit)," + ClientUtil.ToInt(ClientUtil.ToString(ds.Tables[0].Rows[i][16])) + ",'" + ClientUtil.ToString(ds.Tables[0].Rows[i][17]) + "',cast('" + ClientUtil.ToString(ds.Tables[0].Rows[i][18]) + "' as bigint)," + ClientUtil.ToInt(ClientUtil.ToString(ds.Tables[0].Rows[i][19])) + ",'" + ClientUtil.ToString(ds.Tables[0].Rows[i][0]).Replace(".", "") + "'," + (s++) + "," + s + ",'" + DateTime.Now.Date.ToShortDateString() + "',1)";

                        numJL += 1;
                        //list.Add(ob);
                        //// list = theMEdiClient.insert(list);
                        if (!model.inserts(insertSql))
                        {
                            result = false;
                            MessageBox.Show("导入第 " + numJL + " 条记录出错! 第一字段值为：" + ds.Tables[0].Rows[i][1].ToString() + "数据的磅单号已存在!");
                        }
                        else
                        {

                            string updateSql = "";
                            //if (dss.Tables[0].Rows.Count > 0)
                            //{
                            //    id = ClientUtil.ToInt(ClientUtil.ToString(dss.Tables[0].Rows[i]["ACCTITLEID"]));
                            //}
                            code = ClientUtil.ToString(ds.Tables[0].Rows[i][0]).Replace(".", "");
                            subcode = ClientUtil.ToString(ds.Tables[0].Rows[i][0]);
                            if (code.Length == 4)
                            {
                                //code = code;
                                updateSql = "update thd_FIACCTITLE set PARENTNODEID=1,ACCNODELEVEL=2,ACCSYSCODE='" + code.Substring(0, 1) + "." + subcode.Substring(1, 3) + "' where ACCCODE='" + ClientUtil.ToString(ds.Tables[0].Rows[i][0]).Replace(".", "") + "' and substring(ACCCODE,1,4)='" + code + "' ";
                                model.updates(updateSql);
                            }
                            if (code.Length == 6)
                            {
                                code = code.Substring(0, 4);
                                updateSql = "update thd_FIACCTITLE  set PARENTNODEID=(select ACCTITLEID from FIACCTITLE where ACCCODE='" + code + "'),ACCNODELEVEL=3,ACCSYSCODE='" + code.Substring(0, 1) + "." + subcode.Substring(1, 6) + "' where ACCCODE='" + ClientUtil.ToString(ds.Tables[0].Rows[i][0]).Replace(".", "") + "' and substring(ACCCODE,1,4)='" + code + "' and exists(select ACCTITLEID from FIACCTITLE  where ACCCODE='" + code + "') ";
                                model.updates(updateSql);
                            }
                            if (code.Length == 8)
                            {
                                code = code.Substring(0, 6);
                                updateSql = "update thd_FIACCTITLE set PARENTNODEID=(select ACCTITLEID from FIACCTITLE where ACCCODE='" + code + "'),ACCNODELEVEL=4,ACCSYSCODE='" + code.Substring(0, 1) + "." + subcode.Substring(1, 9) + "' where ACCCODE='" + ClientUtil.ToString(ds.Tables[0].Rows[i][0]).Replace(".", "") + "' and substring(ACCCODE,1,6)='" + code + "' and exists(select ACCTITLEID from FIACCTITLE  where ACCCODE='" + code + "')  ";
                                model.updates(updateSql);
                            }
                            // model.updates(updateSql);
                            result = true;
                            //if (model.updates(updateSql))
                            //{
                            //    result = true;
                            //}//
                            //else
                            //{
                            //    result = false;
                            //}
                            //string updateSql = "update thd_FIACCTITLE z set z.PARENTNODEID=" + id + " where z.ACCCODE='" + ds.Tables[0].Rows[i][0].ToString().Replace(".", "") + "' and substring(z.ACCCODE,1,4) ";

                        }

                    }
                    else
                    {
                        DataRow dr = dtError.NewRow();
                        dr.ItemArray = ds.Tables[0].Rows[i].ItemArray;
                        dtError.Rows.Add(dr);
                        dtError.AcceptChanges();
                    }
                }
                if (dtError.Rows.Count > 0)
                    dgvToExcel(dtError);

                if (result == false)
                {
                    return 0;
                }
                else
                {

                    return numJL;
                }


            }
            catch (Exception Err)
            {
                MessageBox.Show("读取EXCEL文件出现错误：" + Err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
        }
        /// <summary>
        /// 从数据表导出数据到EXCEL
        /// </summary>
        /// <param name="datetable">传入数据的DataTable名字</param>
        /// <param name="columstart"></param>
        public void dgvToExcel(DataTable datetable)
        {
            try
            {
                Microsoft.Office.Interop.Excel.ApplicationClass MyExcel = new Microsoft.Office.Interop.Excel.ApplicationClass();
                MyExcel.Visible = true;
                if (MyExcel == null)
                {
                    //manage.writerLogFile("BaseForm-dgvToExcel(DataTable datetable) EXCEL没有安装！ ");
                    MessageBox.Show("EXCEL没有安装！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Microsoft.Office.Interop.Excel.Workbooks MyWorkBooks = MyExcel.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook MyWorkBook = MyWorkBooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet MyWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)MyWorkBook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Range MyRange = MyWorkSheet.get_Range(MyWorkSheet.Cells[1, 1], MyWorkSheet.Cells[1, datetable.Columns.Count]);
                object[] MyHeader = new object[datetable.Columns.Count];
                for (int k = 0; k < datetable.Columns.Count; k++)
                {
                    MyHeader[k] = datetable.Columns[k].ToString();
                }
                MyRange.Value2 = MyHeader;
                if (datetable.Rows.Count > 0)
                {
                    MyRange = MyWorkSheet.get_Range("A2", System.Reflection.Missing.Value);
                    String[,] MyData = new String[datetable.Rows.Count, datetable.Columns.Count];
                    for (int i = 0; i < datetable.Rows.Count; i++)
                    {
                        for (int j = 0; j < datetable.Columns.Count; j++)
                        {
                            MyData[i, j] = datetable.Rows[i][j].ToString();
                            //MyData[i,j] = dategridview.Rows[i].Cells[j].Value.ToString();//							
                        }
                    }
                    MyRange = MyRange.get_Resize(datetable.Rows.Count, datetable.Columns.Count);
                    MyRange.Value2 = MyData;
                    MyRange.EntireColumn.AutoFit();
                }
                MyExcel = null;

            }
            catch (Exception Err)
            {
                //manage.writerLogFile("BaseForm-dgvToExcel(DataTable datetable)   " + Err.Message);
                MessageBox.Show("导出EXCEL时出现错误！" + Err.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //            delete from thd_FIACCTITLE where accnodelevel<>1

            //select distinct accnodelevel from thd_FIACCTITLE

            //select * from thd_FIACCTITLE

            //select * from RES_COSTPROJECT
            //UPDATE RES_COSTPROJECT SET ACCTITLE=1
            //UPDATE RES_COSTPROJECT SET ACCTITLE=2986

            //SELECT * FROM  Res_CostStockOutTypeVsAccTitle
            //UPDATE Res_CostStockOutTypeVsAccTitle SET ACCTITLE=1
            //UPDATE Res_CostStockOutTypeVsAccTitle SET ACCTITLE=2986

            //SELECT * FROM Res_BalanceRelationAccTitle
            //UPDATE Res_BalanceRelationAccTitle SET ACCTITLE=1
            //UPDATE Res_BalanceRelationAccTitle SET ACCTITLE=2986

            //SELECT * FROM Res_CostStockInTypeVsAccTitle
            //UPDATE Res_CostStockInTypeVsAccTitle SET ACCTITLE=1
            //UPDATE Res_CostStockInTypeVsAccTitle SET ACCTITLE=2986

            //select * from RESTransVsAcct
            //UPDATE RESTransVsAcct SET ACCTITid=1


            //select * from RESMoneyVsAcct
            //update RESMoneyVsAcct set acctitid=1

            //select * from RESBUSTYPE
            //update RESBUSTYPE set acctitid=1

            //select * from Res_ExpenseItem
            //update Res_ExpenseItem set accounttitle=null

            IList lstCaption = new ArrayList();
            lstCaption.Add("Id");
            lstCaption.Add("科目编码");
            lstCaption.Add("科目名称");
            lstCaption.Add("助记码");
            lstCaption.Add("科目类别");
            lstCaption.Add("余额方向");
            lstCaption.Add("外币核算");
            lstCaption.Add("全名");
            lstCaption.Add("数量金额");
            lstCaption.Add("计量单位");
            lstCaption.Add("现金科目");
            lstCaption.Add("银行科目");
            lstCaption.Add("项目辅助核算");
            VImprotData import = new VImprotData(lstCaption);
            import.ReturnClick += new ReturnClick(import_ReturnClick);
            import.ShowDialog(this);
            import.Close();
        }
        void import_ReturnClick(C1.Win.C1FlexGrid.C1FlexGrid aFlex)
        {
            string err = "";
            DataTable dt = new DataTable();
            DataColumn colKMBM = new DataColumn();
            colKMBM.ColumnName = "KMBM";
            dt.Columns.Add(colKMBM);
            DataColumn colKMMC = new DataColumn();
            colKMMC.ColumnName = "KMMC";
            dt.Columns.Add(colKMMC);
            DataColumn colZJM = new DataColumn();
            colZJM.ColumnName = "ZJM";
            dt.Columns.Add(colZJM);
            DataColumn colKMLB = new DataColumn();
            colKMLB.ColumnName = "KMLB";
            dt.Columns.Add(colKMLB);
            DataColumn colYEFX = new DataColumn();
            colYEFX.ColumnName = "YEFX";
            dt.Columns.Add(colYEFX);
            DataColumn colWBHS = new DataColumn();
            colWBHS.ColumnName = "WBHS";
            dt.Columns.Add(colWBHS);
            DataColumn colQM = new DataColumn();
            colQM.ColumnName = "QM";
            dt.Columns.Add(colQM);
            DataColumn colSLJE = new DataColumn();
            colSLJE.ColumnName = "SLJE";
            dt.Columns.Add(colSLJE);
            DataColumn colJLDW = new DataColumn();
            colJLDW.ColumnName = "JLDW";
            dt.Columns.Add(colJLDW);
            DataColumn colXJKM = new DataColumn();
            colXJKM.ColumnName = "XJKM";
            dt.Columns.Add(colXJKM);
            DataColumn colYHKM = new DataColumn();
            colYHKM.ColumnName = "YHKM";
            dt.Columns.Add(colYHKM);
            DataColumn colXMFZHS = new DataColumn();
            colXMFZHS.ColumnName = "XMFZHS";
            dt.Columns.Add(colXMFZHS);

            for (int i = 1; i < aFlex.Rows.Count; i++)
            {
                Row row = aFlex.Rows[i];
                string sKMBM = ClientUtil.ToString(row["科目编码"]);
                string sKMMC = ClientUtil.ToString(row["科目名称"]).Trim();
                string sZJM = ClientUtil.ToString(row["助记码"]);
                string sKMLB = ClientUtil.ToString(row["科目类别"]);
                string sYEFX = ClientUtil.ToString(row["余额方向"]);
                string sWBHS = ClientUtil.ToString(row["外币核算"]);
                string sQM = ClientUtil.ToString(row["全名"]);
                string sSLJE = ClientUtil.ToString(row["数量金额"]);
                string sJLDW = ClientUtil.ToString(row["计量单位"]);
                string sXJKM = ClientUtil.ToString(row["现金科目"]);
                string sYHKM = ClientUtil.ToString(row["银行科目"]);
                string sXMFZHS = ClientUtil.ToString(row["项目辅助核算"]);



                int FirstLev = accLevel.FirstLev;
                int SecondLev = accLevel.SecondLev;
                int ThirdLev = accLevel.ThirdLev;
                int FourthLev = accLevel.FourthLev;
                int FifthLev = accLevel.FifthLev;
                int SixthLev = accLevel.SixthLev;

                string _ParentCode = "";


                switch (sKMBM.Length)
                {
                    case 4:
                        _ParentCode = "";
                        break;
                    case 6:
                        _ParentCode = sKMBM.Substring(0, 4);
                        break;
                    case 8:
                        _ParentCode = sKMBM.Substring(0, 6);
                        break;
                    case 10:
                        _ParentCode = sKMBM.Substring(0, 8);
                        break;
                    case 12:
                        _ParentCode = sKMBM.Substring(0, 10);
                        break;
                    case 14:
                        _ParentCode = sKMBM.Substring(0, 12);
                        break;
                    default:
                        break;
                }
                //switch (sKMBM.Length)
                //{
                //    case 4:
                //        _ParentCode = "";
                //        break;
                //    case 8:
                //        _ParentCode = sKMBM.Substring(0, 4);
                //        break;
                //    case 12:
                //        _ParentCode = sKMBM.Substring(0, 8);
                //        break;
                //    case 16:
                //        _ParentCode = sKMBM.Substring(0, 12);
                //        break;
                //    case 20:
                //        _ParentCode = sKMBM.Substring(0, 16);
                //        break;
                //    case 24:
                //        _ParentCode = sKMBM.Substring(0, 20);
                //        break;
                //    default:
                //        break;
                //}
                if (_ParentCode != "")
                {
                    parenttitle = model.GetParentAccTitle(_ParentCode);
                }
                else
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Name", "会计科目"));
                    IList ls = model.GetAccTitle(oq);
                    if (ls.Count != 0)
                    {
                        parenttitle = ls[0] as AccountTitle;
                    }
                }
                AccountTitle AT = new AccountTitle();
                AT.Code = sKMBM;
                AT.AccountCode = sKMBM;
                AT.Name = sKMMC;
                switch (sKMLB)
                {
                    case "资产":
                        AT.AccType = AccountType.Asserts;
                        break;
                    case "流动资产":
                        AT.AccType = AccountType.Asserts;
                        break;
                    case "非流动资产":
                        AT.AccType = AccountType.Asserts;
                        break;
                    case "负债":
                        AT.AccType = AccountType.Liabilities;
                        break;
                    case "流动负债":
                        AT.AccType = AccountType.Liabilities;
                        break;
                    case "非流动负债":
                        AT.AccType = AccountType.Liabilities;
                        break;
                    case "成本":
                        AT.AccType = AccountType.Cost;
                        break;
                    case "权益":
                        AT.AccType = AccountType.ProfitAndLoss;
                        break;
                    case "共同":
                        AT.AccType = AccountType.Together;
                        break;
                    case "期间费用":
                        AT.AccType = AccountType.ProfitAndLoss;
                        break;
                    case "其他收益":
                        AT.AccType = AccountType.ProfitAndLoss;
                        break;
                    case "其他损失":
                        AT.AccType = AccountType.ProfitAndLoss;
                        break;
                    case "损益":
                        AT.AccType = AccountType.Interests;
                        break;
                    case "所得税":
                        AT.AccType = AccountType.Interests;
                        break;
                    case "所有者权益":
                        AT.AccType = AccountType.Interests;
                        break;
                    case "以前年度损益调整":
                        AT.AccType = AccountType.Interests;
                        break;
                    case "营业成本及税金":
                        AT.AccType = AccountType.ProfitAndLoss;
                        break;
                    case "营业收入":
                        AT.AccType = AccountType.ProfitAndLoss;
                        break;
                    default:
                        break;
                }
                if (sYEFX == "借")
                {
                    AT.BalanceDire = 0;
                }
                else
                {
                    AT.BalanceDire = 1;
                }
                if (sSLJE == "是")
                {
                    AT.QuantityAccount = true;
                }
                if (sXJKM == "是")
                {
                    AT.AboutCash = CashAccTitle.CashTitle;
                }
                else
                {
                    if (sYHKM == "是")
                    {
                        AT.AboutCash = CashAccTitle.BankTitle;
                    }
                    else
                    {
                        AT.AboutCash = CashAccTitle.NotCash;
                    }
                }

                switch (sWBHS)
                {
                    case "不核算":
                        AT.ForeignAccount = false;
                        break;
                    case "美元":
                        AT.ForeignAccount = true;
                        AT.ForeignCurrency = null;
                        break;
                    case "所有币别":
                        AT.ForeignAccount = true;
                        AT.ForeignCurrency = null;
                        break;
                    default:
                        break;
                }

                AT.AccLevelName = sQM;
                if (sJLDW != "")
                {
                    AT.QuantityUnit = sJLDW;
                }

                switch (sXMFZHS)
                {
                    case "部门":
                        AT.DepartmentAccount = true;
                        break;
                    case "部门/供应商":
                        AT.DepartmentAccount = true;
                        AT.SupplierAccount = true;
                        break;
                    case "部门/销售方向":
                        AT.DepartmentAccount = true;
                        break;
                    case "客户/部门/职员":
                        AT.ClientAccount = true;
                        AT.DepartmentAccount = true;
                        AT.PersonAccount = true;
                        break;
                    case "其他往来项":
                        AT.DepartmentAccount = true;
                        break;
                    case "客户往来":
                        AT.ClientAccount = true;
                        break;
                    case "供应商往来":
                        AT.SupplierAccount = true;
                        break;
                    case "个人往来":
                        AT.PersonAccount = true;
                        break;
                    case "部门核算":
                        AT.DepartmentAccount = true;
                        break;
                    case "项目核算":
                        AT.ProjectAccount = true;
                        break;
                    default:
                        break;
                }

                curTitle = AT;

                TreeNode tnNew = new TreeNode();
                tnNew.Text = sKMBM + "  " + sKMMC;
                tnNew.Name = sKMBM;



                #region 新增下级节点


                curTitle.ParentNode = parenttitle;
                //if (!ValidateData(curTitle))
                //{
                //    return;
                //}
                curTitle = model.SaveAccountTitle(curTitle);
                //tnCurr.Tag = curTitle.ParentNode as AccountTitle;
                //tnCurr.Nodes.Add(tnNew);

                #endregion

                //tnNew.Tag = curTitle;
                //lstTitles.Add(curTitle);
                //this.tvTitle.SelectedNode = tnNew;
            }

            if (err != "")
            {
                err += "导入失败！";
                MessageBox.Show(err);
                return;
            }

        }

    }
}