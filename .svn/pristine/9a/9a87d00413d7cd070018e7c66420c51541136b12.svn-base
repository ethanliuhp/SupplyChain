using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinMVC.core;


namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    public partial class TQueryView : Form,IMainView
    {
        public TQueryView()
        {
            InitializeComponent();
            floor = this.pnlFloor;
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

        private MainViewState viewState;

        public MainViewState ViewState
        {
            get { return viewState; }
            set { viewState = value; }
        }

        //刷新状态
        public virtual void RefreshState(MainViewState state)
        {
            viewState = state;

            toolMenu.SetTemplate(MainViewType.Query);

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
                case ToolMenuItem.Preview:
                    PreviewView();
                    break;
                case ToolMenuItem.Print:
                    PrintView();
                    break;
            }
        }

        public virtual void PreviewView()
        {
 
        }

        public virtual void PrintView()
        {
 
        }
    }
}