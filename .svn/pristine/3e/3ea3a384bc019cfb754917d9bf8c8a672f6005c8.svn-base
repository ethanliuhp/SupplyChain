using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinMVC.core;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    

    public  class TBasicDataView :Form,IMainView
    {
        public System.Windows.Forms.Panel pnlFloor = new System.Windows.Forms.Panel();
        public event ViewDeletedHandle ViewDeletedEvent;

        public TBasicDataView()
            : base()
        {
            InitializeComponent();
            Floor = pnlFloor;
            this.Resize += new EventHandler(TBasicDataView_Resize);
        }

        void TBasicDataView_Resize(object sender, EventArgs e)
        {
            SetTitlePosition();
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
            this.pnlFloor = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.lblTitle);
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFloor.Location = new System.Drawing.Point(0, 0);
            this.pnlFloor.Name = "pnlFloor";
            this.pnlFloor.Size = new System.Drawing.Size(389, 283);
            this.pnlFloor.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("KaiTi_GB2312", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTitle.Location = new System.Drawing.Point(177, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(51, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "标题";
            this.lblTitle.Visible = false;
            // 
            // TBasicDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 283);
            this.Controls.Add(this.pnlFloor);
            this.Name = "TBasicDataView";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
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
                case MainViewState.Modify:
                    toolMenu.LockItem(ToolMenuItem.Modify);
                    toolMenu.LockItem(ToolMenuItem.Refresh);

                    toolMenu.UnlockItem(ToolMenuItem.Cancel);
                    toolMenu.UnlockItem(ToolMenuItem.Save);

                    break;
                case MainViewState.Browser:
                case MainViewState.Initialize:

                    toolMenu.LockItem(ToolMenuItem.Save);
                    toolMenu.LockItem(ToolMenuItem.Cancel);

                    toolMenu.UnlockItem(ToolMenuItem.Modify);
                    toolMenu.UnlockItem(ToolMenuItem.Refresh);
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
                case ToolMenuItem.Modify:
                    if (ModifyView())
                        RefreshState(MainViewState.Modify);
                    break;
                case ToolMenuItem.Save:
                    if (SaveView())
                        RefreshState(MainViewState.Browser);
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
                case ToolMenuItem.Preview:
                    this.Preview();
                    break;
                case ToolMenuItem.Print:
                    this.Print();
                    break;
                case ToolMenuItem.Export:
                    this.Export();
                    break;
            }
        }

        //修改
        public virtual bool ModifyView()
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

        public Label lblTitle;

    }
}
