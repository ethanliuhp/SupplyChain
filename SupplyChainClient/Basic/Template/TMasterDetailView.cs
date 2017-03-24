using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinMVC.core;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    public delegate void ViewDeletedHandle(object sender);

    public partial class TMasterDetailView : Form, IMainView
    {
        public event ViewDeletedHandle ViewDeletedEvent;

        static IDictionary<string, int> dicViews = new Dictionary<string, int>();
        private List<string> submitViews;

        public TMasterDetailView()
        {
            InitializeComponent();
            floor = this.pnlFloor;

            InitSubmitViews();
        }

        #region 实现IView接口


        public event ViewCaptionChangedHandle ViewCaptionChangedEvent;
        public event ViewShowHandle ViewShowEvent;

        private Panel floor;

        public Panel Floor
        {
            get { return floor; }
            set { floor = value; }
        }

        private string viewName;

        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }

        private ViewType viewType;

        public ViewType ViewType
        {
            get { return viewType; }
            set { viewType = value; }
        }

        private string viewCaption;

        public string ViewCaption
        {
            get { return viewCaption; }
            set
            {
                if (ViewCaptionChangedEvent != null)
                    ViewCaptionChangedEvent(viewCaption, value);

                viewCaption = value;
            }
        }

        protected bool viewActived = false;
        public bool ViewActived
        {
            get { return viewActived; }
        }

        //激活视图
        public virtual void ViewActive()
        {
            if (ViewActived) return;

            viewActived = true;

            //订阅菜单消息
            toolMenu.ToolMenuItemClickEvent += new ToolMenuItemClickHandle(ToolMenuItem_Click);

            RefreshState(viewState);
        }

        //弃活视图
        public virtual void ViewDeactive()
        {
            if (!ViewActived) return;

            viewActived = false;

            //取消菜单消息
            toolMenu.ToolMenuItemClickEvent -= new ToolMenuItemClickHandle(ToolMenuItem_Click);

        }

        //显示视图
        public virtual void ViewShow()
        {
            if (ViewShowEvent != null)
                ViewShowEvent(this);
        }

        //准备关闭
        public virtual void ViewClosing(ref bool cancel)
        {
            if (viewState == MainViewState.Modify || viewState == MainViewState.AddNew)
            {
                DialogResult aa = MessageBox.Show("[" + this.viewCaption + "]正处于修改状态，是否关闭？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (aa == DialogResult.No)
                    cancel = true;
            }

        }

        //关闭
        public virtual void ViewClose()
        {
            //取消菜单消息
            toolMenu.ToolMenuItemClickEvent -= new ToolMenuItemClickHandle(ToolMenuItem_Click);

            this.Dispose();
        }

        #endregion

        #region 实现IMainView接口
        private IToolMenu toolMenu;
        public IToolMenu ToolMenu
        {
            get { return toolMenu; }
            set { toolMenu = value; }
        }

        private IList<IView> assistViews = new List<IView>();

        public IList<IView> AssistViews
        {
            get { return assistViews; }
        }

        #endregion

        private void InitSubmitViews()
        {
            submitViews = new List<string>();
            submitViews.Add("验收结算单");
            submitViews.Add("废旧物资处理单");
            submitViews.Add("废旧物资申请信息");
            submitViews.Add("需求管理");
            submitViews.Add("商品砼结算单");
            submitViews.Add("商品砼结算红单");
            submitViews.Add("执行进度计划");
            submitViews.Add("工程量确认单");
            submitViews.Add("日常需求计划");
            submitViews.Add("分包结算单维护");
            submitViews.Add("零星用工单维护");
            submitViews.Add("代工单维护");
            submitViews.Add("逐日派工");
            submitViews.Add("罚款单维护");
            submitViews.Add("费用结算单维护");
            submitViews.Add("材料耗用结算单维护");
            submitViews.Add("料具租赁结算单维护");
            submitViews.Add("机械租赁结算单");
            submitViews.Add("工程任务核算维护");
            submitViews.Add("工程任务核算单维护");
            submitViews.Add("整改通知单");
            submitViews.Add("进度计划编制");
            submitViews.Add("工区周计划维护");
            submitViews.Add("项目周计划维护");
            submitViews.Add("生成执行需求计划");
            submitViews.Add("施工组织设计维护");
            submitViews.Add("专业策划维护");
            submitViews.Add("专项费用结算单");
            submitViews.Add("专项费用管理单");
            submitViews.Add("商务策划维护");
            submitViews.Add("月度需求计划");
            submitViews.Add("日施工情况");
            submitViews.Add("需求总计划单");
            submitViews.Add("采购合同单");
            submitViews.Add("晴雨表信息");
            submitViews.Add("管理人员日志信息");
            submitViews.Add("总进度计划");
            submitViews.Add("月度盘点单");
            submitViews.Add("业主报量单");
            submitViews.Add("检查记录");
            submitViews.Add("会议纪要管理");
            submitViews.Add("收发函管理");
            submitViews.Add("项目目标责任书");
            submitViews.Add("项目实施策划书");
            submitViews.Add("工程文档审批管理");
            submitViews.Add("竣工结算");
            submitViews.Add("合同交底样稿");
            submitViews.Add("暂扣款单维护");
            submitViews.Add("分包签证单维护");
            submitViews.Add("计时派工单维护");

            submitViews.Add("料具封存单");
            submitViews.Add("料具封存单(钢管)");
            submitViews.Add("料具封存单(碗扣)");
            submitViews.Add("运输单");
        }

        public void RegisteViewToSubmit()
        {
            if(string.IsNullOrEmpty(ViewName))
            {
                throw new ArgumentNullException(ViewName, "视图名称不允许为空");
            }

            if (submitViews != null && !submitViews.Exists(s => s.Equals(ViewName)))
            {
                submitViews.Add(ViewName);
            }
        }

        public void RemoveViewFromSubmit()
        {
            if (string.IsNullOrEmpty(ViewName))
            {
                throw new ArgumentNullException(ViewName, "视图名称不允许为空");
            }

            if (submitViews != null && submitViews.Exists(s => s.Equals(ViewName)))
            {
                submitViews.Remove(ViewName);
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.lblTitle.Text = value;
                SetTitleLocation();


            }
        }

        private void SetTitleLocation()
        {
            this.lblTitle.Left = (this.lblTitle.Parent.Width - this.lblTitle.Width) / 2;
        }

        private MainViewState viewState;

        public MainViewState ViewState
        {
            get { return viewState; }
            set { viewState = value; }
        }

        //刷新状态（查看单据）
        public virtual void RefreshStateByQuery()
        {
            if (toolMenu == null)
                return;

            toolMenu.SetTemplate(MainViewType.MasterDetail);

            toolMenu.LockItem(ToolMenuItem.Check);
            toolMenu.LockItem(ToolMenuItem.Submit);
            toolMenu.LockItem(ToolMenuItem.Approve);
            toolMenu.LockItem(ToolMenuItem.Abolish);

            toolMenu.LockItem(ToolMenuItem.AddNew);
            toolMenu.LockItem(ToolMenuItem.Modify);
            toolMenu.LockItem(ToolMenuItem.Delete);
            toolMenu.LockItem(ToolMenuItem.Refresh);

            toolMenu.LockItem(ToolMenuItem.Save);
            toolMenu.LockItem(ToolMenuItem.Cancel);

            toolMenu.UnlockItem(ToolMenuItem.Preview);
            toolMenu.UnlockItem(ToolMenuItem.Print);
            toolMenu.UnlockItem(ToolMenuItem.Export);
        }

        //刷新状态
        public virtual void RefreshStateBySubmit(MainViewState state)
        {
            if (toolMenu == null)
                return;

            viewState = state;
            toolMenu.SetTemplate(MainViewType.BasicData);

            switch (viewState)
            {
                case MainViewState.Modify:

                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Refresh);
                    toolMenu.UnlockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Save);
                    ToolMenu.UnlockItem(ToolMenuItem.Delete);
                    break;

                case MainViewState.AddNew:

                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Refresh);

                    toolMenu.UnlockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Save);
                    toolMenu.UnlockItem(ToolMenuItem.Submit);
                    toolMenu.LockItem(ToolMenuItem.Refresh);
                    toolMenu.LockItem(ToolMenuItem.Check);
                    ToolMenu.LockItem(ToolMenuItem.Delete);
                    break;
                case MainViewState.Browser:

                    toolMenu.UnlockItem(ToolMenuItem.AddNew);
                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.UnlockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);

                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.Initialize:

                    toolMenu.UnlockItem(ToolMenuItem.AddNew);
                    toolMenu.UnlockItem(ToolMenuItem.Search);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);

                    toolMenu.LockItem(ToolMenuItem.Preview);
                    toolMenu.LockItem(ToolMenuItem.Print);
                    toolMenu.LockItem(ToolMenuItem.Export);
                    break;
            }

            RefreshControls(viewState);

        }

        private void SetToolMenuState(MainViewState state)
        {
            if (state == MainViewState.AddNew || state == MainViewState.Modify)
            {
                if (submitViews.Exists(s => ViewName.IndexOf(s) != -1))
                {
                    toolMenu.UnlockItem(ToolMenuItem.Submit);
                }
            }
        }

        //刷新状态
        public virtual void RefreshState(MainViewState state)
        {
            viewState = state;
            if (toolMenu == null)
                return;

            toolMenu.SetTemplate(MainViewType.MasterDetail);

            toolMenu.LockItem(ToolMenuItem.Check);
            toolMenu.LockItem(ToolMenuItem.Submit);
            toolMenu.LockItem(ToolMenuItem.Approve);
            toolMenu.LockItem(ToolMenuItem.Abolish);
            toolMenu.LockItem(ToolMenuItem.Execute);
            toolMenu.LockItem(ToolMenuItem.Red);

            switch (viewState)
            {
                case MainViewState.Initialize:
                    toolMenu.UnlockItem(ToolMenuItem.AddNew);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.LockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);

                    toolMenu.LockItem(ToolMenuItem.Preview);
                    toolMenu.LockItem(ToolMenuItem.Print);
                    toolMenu.LockItem(ToolMenuItem.Export);

                    ViewCaption = ViewName + "-空";
                    break;
                case MainViewState.AddNew:
                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.LockItem(ToolMenuItem.Refresh);

                    toolMenu.UnlockItem(ToolMenuItem.Save);
                    toolMenu.UnlockItem(ToolMenuItem.Cancel);

                    toolMenu.LockItem(ToolMenuItem.Preview);
                    toolMenu.LockItem(ToolMenuItem.Print);
                    toolMenu.LockItem(ToolMenuItem.Export);
                    SetToolMenuState(state);
                    break;
                case MainViewState.Modify:
                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.LockItem(ToolMenuItem.Refresh);

                    toolMenu.UnlockItem(ToolMenuItem.Save);
                    toolMenu.UnlockItem(ToolMenuItem.Cancel);

                    toolMenu.LockItem(ToolMenuItem.Preview);
                    toolMenu.LockItem(ToolMenuItem.Print);
                    toolMenu.LockItem(ToolMenuItem.Export);
                    SetToolMenuState(state);
                    break;
                case MainViewState.Browser:

                    toolMenu.UnlockItem(ToolMenuItem.AddNew);
                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.UnlockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);

                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.Approve:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Approve);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.Check:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Check);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.Execute:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Execute);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.Red:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Red);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.ModifyAndBrowser:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.LockItem(ToolMenuItem.Execute);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.ModifyAndApprove:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Approve);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.ModifyAndExecute:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Execute);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;
                case MainViewState.ModifyAndCheck:

                    toolMenu.LockItem(ToolMenuItem.AddNew);
                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Delete);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Check);
                    toolMenu.UnlockItem(ToolMenuItem.Preview);
                    toolMenu.UnlockItem(ToolMenuItem.Print);
                    toolMenu.UnlockItem(ToolMenuItem.Export);
                    break;

            }
            RefreshControls(viewState);
        }

        //刷新自身
        public virtual void RefreshControls(MainViewState state)
        {

        }
        //响应ToolMenuItemClick
        void ToolMenuItem_Click(ToolMenuItem item)
        {
            switch (item)
            {
                case ToolMenuItem.AddNew:
                    if (NewView())
                    {
                        if (dicViews.ContainsKey(ViewName))
                            dicViews[ViewName]++;
                        else
                            dicViews.Add(ViewName, 1);

                        ViewCaption = ViewName + "-新建" + dicViews[ViewName];

                        RefreshState(MainViewState.AddNew);
                    }
                    break;
                case ToolMenuItem.Modify:
                    if (ModifyView())
                        RefreshState(MainViewState.Modify);
                    break;
                case ToolMenuItem.Save:
                    if (SaveView())
                        RefreshState(MainViewState.Browser);
                    break;
                case ToolMenuItem.Delete:
                    DialogResult dr = MessageBox.Show("是否要删除当前单据吗？", "删除", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        if (DeleteView())
                        {
                            RefreshState(MainViewState.Initialize);
                            if (ViewDeletedEvent != null)
                                ViewDeletedEvent(this);
                        }
                    }
                    break;
                case ToolMenuItem.Cancel:
                    if (CancelView())
                        if (ViewState == MainViewState.AddNew)
                        {
                            RefreshState(MainViewState.Initialize);

                            if (ViewDeletedEvent != null)
                                ViewDeletedEvent(this);
                        }
                        else
                            RefreshState(MainViewState.Browser);
                    break;
                case ToolMenuItem.Refresh:
                    RefreshView();
                    break;
                case ToolMenuItem.Preview:
                    this.Preview();
                    break;
                case ToolMenuItem.Print:
                    this.Print();
                    break;
                case ToolMenuItem.Export:
                    this.Export();
                    break;
                case ToolMenuItem.Approve:
                    this.ApproveView();
                    break;
                case ToolMenuItem.Check:
                    this.CheckView();
                    break;
                case ToolMenuItem.Submit:
                    if (SubmitView())
                        RefreshState(MainViewState.Browser);
                    break;
                case ToolMenuItem.Execute:
                    this.ExecuteView();
                    break;
                case ToolMenuItem.Red:
                    this.RedView();
                    break;
                case ToolMenuItem.Search:
                    this.Search();
                    break;
            }
        }
        //
        public virtual bool Search()
        {
            return false;
        }

        //审批
        public virtual bool ApproveView()
        {
            return false;
        }
        //提交
        public virtual bool SubmitView()
        {
            return false;
        }
        //制单
        public virtual bool ExecuteView()
        {
            return false;
        }
        //冲红
        public virtual bool RedView()
        {
            return false;
        }
        //复核
        public virtual bool CheckView()
        {
            return false;
        }

        //新建
        public virtual bool NewView()
        {
            return false;
        }
        //修改
        public virtual bool ModifyView()
        {
            return false;
        }

        //保存
        public virtual bool SaveView()
        {
            return false;
        }

        //删除
        public virtual bool DeleteView()
        {
            return false;
        }

        //撤销
        public virtual bool CancelView()
        {
            return false;
        }

        //刷新
        public virtual void RefreshView()
        {

        }
        //打印预览
        public virtual bool Preview()
        {
            return false;
        }
        //打印
        public virtual bool Print()
        {
            return false;
        }

        public virtual bool Export()
        {
            return false;
        }

    }
}
