using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinMVC.core;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    public  class TBasicDataViewByMobile :Form,IMainView
    {
        private AutomaticSize automaticSize = new AutomaticSize(); 
        public System.Windows.Forms.Panel pnlFloor = new System.Windows.Forms.Panel();
        public event ViewDeletedHandle ViewDeletedEvent;
        //public Point startPoint = new Point();
        //public Point endPoint = new Point();

        public TBasicDataViewByMobile()
            : base()
        {          
            InitializeComponent();
            Floor = pnlFloor;
            InitEvent();  
        }

        private void InitEvent()
        {
            this.Resize += new EventHandler(TBasicDataView_Resize);
            TBtnClose.Click += new EventHandler(TBtnClose_Click);
            TBtnExit.Click += new EventHandler(TBtnExit_Click);
            TBtnGoMenu.Click += new EventHandler(TBtnGoMenu_Click);
            TBtnBack.Click += new EventHandler(TBtnBack_Click);
        }

        void TBasicDataView_Resize(object sender, EventArgs e)
        {
            automaticSize.ControlResize(this);
            SetTitlePosition();
        }
        private void TBasicDataView_Load(object sender, EventArgs e)
        {
            automaticSize.Width = this.Width;
            automaticSize.Height = this.Height;
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
            //    this.TBtnClose.ImageScaling = ToolStripItemImageScaling.None;
            //    this.TBtnClose.Size = new System.Drawing.Size(buttonHight, buttonHight);
            //}
        }

        void TBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void TBtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void TBtnExit_Click(object sender, EventArgs e)
        {
            //System.Environment.Exit(0);
            System.Windows.Forms.Application.Exit();
        }

        void TBtnGoMenu_Click(object sender, EventArgs e)
        {
            IList openList = new ArrayList();
            VMobileMainMenu menuForm = new VMobileMainMenu();
            int menuCount = 0;
            foreach(Form openForm in System.Windows.Forms.Application.OpenForms)
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

            menuForm.ControlsClear(true);
        }

        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            try
            {
                base.Dispose(disposing);
            }catch(Exception e){
            
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TBasicDataViewByMobile));
            this.pnlFloor = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TBtnClose = new System.Windows.Forms.ToolStripButton();
            this.TBtnExit = new System.Windows.Forms.ToolStripButton();
            this.TBtnGoMenu = new System.Windows.Forms.ToolStripButton();
            this.TBtnBack = new System.Windows.Forms.ToolStripButton();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFloor.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFloor.Controls.Add(this.toolStrip1);
            this.pnlFloor.Controls.Add(this.lblTitle);
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFloor.Location = new System.Drawing.Point(0, 0);
            this.pnlFloor.Name = "pnlFloor";
            this.pnlFloor.Size = new System.Drawing.Size(389, 283);
            this.pnlFloor.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TBtnClose,
            this.TBtnExit,
            this.TBtnGoMenu,
            this.TBtnBack});
            this.toolStrip1.Location = new System.Drawing.Point(0, 229);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(387, 52);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TBtnClose
            // 
            this.TBtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBtnClose.Image = ((System.Drawing.Image)(resources.GetObject("TBtnClose.Image")));
            this.TBtnClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TBtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBtnClose.Margin = new System.Windows.Forms.Padding(0, 1, 2, 2);
            this.TBtnClose.Name = "TBtnClose";
            this.TBtnClose.Size = new System.Drawing.Size(75, 49);
            this.TBtnClose.Text = "关 闭";
            // 
            // TBtnExit
            // 
            this.TBtnExit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TBtnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBtnExit.Image = ((System.Drawing.Image)(resources.GetObject("TBtnExit.Image")));
            this.TBtnExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TBtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBtnExit.Name = "TBtnExit";
            this.TBtnExit.Size = new System.Drawing.Size(75, 49);
            this.TBtnExit.Text = "退 出";
            // 
            // TBtnGoMenu
            // 
            this.TBtnGoMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TBtnGoMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBtnGoMenu.Image = ((System.Drawing.Image)(resources.GetObject("TBtnGoMenu.Image")));
            this.TBtnGoMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TBtnGoMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBtnGoMenu.Name = "TBtnGoMenu";
            this.TBtnGoMenu.Size = new System.Drawing.Size(75, 49);
            this.TBtnGoMenu.Text = "回到主菜单";
            // 
            // TBtnBack
            // 
            this.TBtnBack.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TBtnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBtnBack.Image = ((System.Drawing.Image)(resources.GetObject("TBtnBack.Image")));
            this.TBtnBack.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TBtnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBtnBack.Name = "TBtnBack";
            this.TBtnBack.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.TBtnBack.Size = new System.Drawing.Size(75, 49);
            this.TBtnBack.Text = "回退";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("楷体_GB2312", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTitle.Location = new System.Drawing.Point(177, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(51, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "标题";
            this.lblTitle.Visible = false;
            // 
            // TBasicDataViewByMobile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 283);
            this.Controls.Add(this.pnlFloor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TBasicDataViewByMobile";
            this.Load += new System.EventHandler(this.TBasicDataView_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        //private System.Windows.Forms.Panel pnlFloor;
        #endregion

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

        private string title;

        public string Title
        {
            get { return title; }
            set 
            {
                title = value;
                this.lblTitle.Text =" "+ value+" ";
                SetTitlePosition();
            
            }
        }

        private void SetTitlePosition()
        {
           if(lblTitle.Parent!=null)
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

        public ToolStrip toolStrip1;
        private ToolStripButton TBtnClose;
        private ToolStripButton TBtnExit;
        private ToolStripButton TBtnGoMenu;
        private ToolStripButton TBtnBack;
    }
}
