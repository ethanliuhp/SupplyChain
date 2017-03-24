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

        #region ʵ��IView�ӿ�

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

        //������ͼ
        public virtual void ViewActive()
        {
            if (ViewActived) return;

            viewActived = true;

            //���Ĳ˵���Ϣ
            toolMenu.ToolMenuItemClickEvent += new ToolMenuItemClickHandle(ToolMenuItem_Click);

            RefreshState(viewState);
        }

        //������ͼ
        public virtual void ViewDeactive()
        {
            if (!ViewActived) return;

            viewActived = false;

            //ȡ���˵���Ϣ
            toolMenu.ToolMenuItemClickEvent -= new ToolMenuItemClickHandle(ToolMenuItem_Click);
        }

        //��ʾ��ͼ
        public virtual void ViewShow()
        {
            if (ViewShowEvent != null)
                ViewShowEvent(this);
        }

        //׼���ر�
        public virtual void ViewClosing(ref bool cancel)
        {

        }

        //�ر�
        public virtual void ViewClose()
        {
            //ȡ���˵���Ϣ
            toolMenu.ToolMenuItemClickEvent -= new ToolMenuItemClickHandle(ToolMenuItem_Click);

            this.Dispose();
        }

        #endregion

        #region ʵ��IMainView�ӿ�
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

        //ˢ��״̬
        public virtual void RefreshState(MainViewState state)
        {
            viewState = state;

            toolMenu.SetTemplate(MainViewType.Query);

            RefreshControls(viewState);
        }

        //ˢ������
        public virtual void RefreshControls(MainViewState state)
        {

        }

        //��ӦToolMenuItemClick
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