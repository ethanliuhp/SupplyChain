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
    public partial class TSystemView : Form, IMainView
    {
        public TSystemView()
        {
            InitializeComponent();
            this.floor = pnlFloor;//界面设计的时候不显示控件，只需注释这一行代码
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
            
            RefreshState(viewState);
        }

        //弃活视图
        public virtual void ViewDeactive()
        {
            if (!ViewActived) return;

            viewActived = false;
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

            toolMenu.SetTemplate(MainViewType.System);

            RefreshControls(viewState);
        }

        //刷新自身
        public virtual void RefreshControls(MainViewState state)
        {

        }
    }
}