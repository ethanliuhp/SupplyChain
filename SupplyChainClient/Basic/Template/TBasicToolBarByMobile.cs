using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinMVC.core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    public partial class TBasicToolBarByMobile : Form, IMainView
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        public System.Windows.Forms.Panel pnlFloor = new System.Windows.Forms.Panel();
        public event ViewDeletedHandle ViewDeletedEvent;
        int number = 0;

        public TBasicToolBarByMobile()
            :base()
        {
            InitializeComponent();
            Floor = pnlFloor;
            number = 0;
            //this.skinEngine1.SkinFile = CommonUtil.SkinPath;
            InitEvents();
        }

        public void InitEvents()
        {
            this.Resize += new EventHandler(TBasicDataView_Resize);
            this.TBtnContents.Click += new EventHandler(TBtnContents_Click);
            this.TBtnPageUp.Click += new EventHandler(TBtnPageUp_Click);
            this.TBtnPageDown.Click += new EventHandler(TBtnPageDown_Click);
        }
        private void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Image image = Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Images\header.png");
            Bitmap b = new Bitmap(image, this.Width, this.Height);

            Graphics dc = Graphics.FromImage((System.Drawing.Image)b);
            //将要绘制的内容绘制到dc上
            g.DrawImage(b, 0, 0);
            dc.Dispose();
        }
        #region 工具棒事件
        public virtual void TBtnContents_Click(object sender, EventArgs e)
        {

        }

        public virtual void TBtnPageUp_Click(object sender, EventArgs e)
        {

        }

        public virtual void TBtnPageDown_Click(object sender, EventArgs e)
        {

        }

        private void 退出选择Item_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 返回主菜单Item_Click(object sender, EventArgs e)
        {
            IList openList = new ArrayList();
            VMobileMainMenu menuForm = new VMobileMainMenu();
            int menuCount = 0;
            foreach (Form openForm in System.Windows.Forms.Application.OpenForms)
            {
                if (openForm.Name != "VMobileMainMenu" || menuCount > 0)
                {
                    openList.Add(openForm);
                }
                else
                {
                    menuForm = (VMobileMainMenu)openForm;
                    menuCount++;
                }
            }
            int openCount = openList.Count;
            for (int t = 0; t < openCount; t++)
            {
                Form openForm = (Form)openList[t];
                openForm.Close();
            }

            //menuForm.ControlsClear(true);
            this.toolStrip1.Visible = false;
        }

        public virtual void 功能菜单1Item_Click(object sender, EventArgs e)
        {

        }

        public virtual void 功能菜单2Item_Click(object sender, EventArgs e)
        {

        }

        public virtual void 功能菜单3Item_Click(object sender, EventArgs e)
        {

        }

        public virtual void 功能菜单4Item_Click(object sender, EventArgs e)
        {

        }

        public virtual void 功能菜单5Item_Click(object sender, EventArgs e)
        {

        }

        public virtual void 功能菜单6Item_Click(object sender, EventArgs e)
        {

        }
        #endregion

        void TBasicDataView_Resize(object sender, EventArgs e)
        {
            if (number == 1)
            {
                automaticSize.ControlResize(this);
                SetTitlePosition();
            }
            number++;
        }
        private void TBasicDataView_Load(object sender, EventArgs e)
        {
            automaticSize.Width = this.Width;
            automaticSize.Height = this.Height;
            //automaticSize.SetTag(this);
            //if (ConstObject.TheLogin != null)
            //{
            //    float formScale = ConstObject.TheLogin.FormScale;
            //    //窗口大小
            //    float clientRealWidth = this.Width * formScale;
            //    float clientRealHight = this.Height * formScale;
            //    this.ClientSize = new System.Drawing.Size(ClientUtil.ToInt(clientRealWidth), ClientUtil.ToInt(clientRealHight));
            //    //控件大小
            //    float controlRealWidth = Convert.ToSingle(Math.Round(ClientUtil.ToDecimal(this.Width / ClientUtil.Tofloat(formScale)), 2));
            //    float controlRealHight = Convert.ToSingle(Math.Round(ClientUtil.ToDecimal(this.Height / ClientUtil.Tofloat(formScale)), 2));
            //    automaticSize.Width = controlRealWidth;
            //    automaticSize.Height = controlRealHight;
            //    automaticSize.ControlResize(this);

            //    Rectangle ScreenArea = System.Windows.Forms.Screen.GetBounds(this);
            //    int width1 = ScreenArea.Width; //屏幕宽度 
            //    int height1 = ScreenArea.Height; //屏幕高度
            //    this.Location = new System.Drawing.Point(width1 / 2 - ClientUtil.ToInt(clientRealWidth / 2), height1 / 2 - ClientUtil.ToInt(clientRealHight / 2) - 20);

            //    int realHight = (int)(32 * formScale);
            //    this.toolStrip1.Height = realHight;
            //    int buttonHight = (int)(realHight * 0.9);
            //    this.TBtnPageDown.ImageScaling = ToolStripItemImageScaling.None;
            //    this.TBtnPageDown.Size = new System.Drawing.Size(buttonHight, buttonHight);
            //}
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
            if (viewState == MainViewState.Modify)
            {

                if (MessageBox.Show("[" + this.viewCaption + "]正处于修改状态，是否关闭?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
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

        #region 公共方法
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.lblTitle.Text = " " + value + " ";
                SetTitlePosition();

            }
        }

        private void SetTitlePosition()
        {
            if (lblTitle.Parent != null)
                this.lblTitle.Left = (this.lblTitle.Parent.Width - this.lblTitle.Width) / 2;
        }

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

            toolMenu.SetTemplate(MainViewType.BasicData);

            switch (viewState)
            {
                case MainViewState.Browser:
                case MainViewState.Initialize:

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);

                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);
                    break;
            }
        }


        //响应ToolMenuItemClick
        void ToolMenuItem_Click(ToolMenuItem item)
        {
            switch (item)
            {
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
            }
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

        public Label lblTitle;

        protected void NewView()
        {
            throw new NotImplementedException();
        }
        #endregion

        
    }
}
