using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm
{
    public partial class VCheckConfirmState : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private List<string> stateList;
        private int page = 1;
        private int pageCount = 0;
        private int pageSize = 5;
        List<CheckBox> listCheckBox = new List<CheckBox>();

        private string result;
        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        public VCheckConfirmState(List<string> list)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            stateList = list;
            InitEvent();
            InitDate();
        }

        void InitEvent()
        {
            btnNext.Click += new EventHandler(btnNext_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
        }

        void InitDate()
        {
            if (stateList.Count % pageSize == 0)
                pageCount = stateList.Count / pageSize;
            else
                pageCount = stateList.Count / pageSize + 1;

            
        }
        void ShowData()
        {
            //dgDetailList.Rows.Clear();
            listCheckBox.Clear();
            this.pnlShow.Controls.Clear();
            //this.WindowState = FormWindowState.Minimized;

            int begionNum = (page-1)*pageSize;
            int endNum = begionNum + pageSize;
            if (page == pageCount)
            {
                endNum = stateList.Count;
                btnNext.Enabled = false;
            }
            else
                btnNext.Enabled = true;

            if(page==1)
                btnBack.Enabled = false;
            else
                btnBack.Enabled = true;

            int width = pnlShow.Width - 20;
            int height = pnlShow.Height / pageSize;

            for (int i = begionNum; i < endNum; i++)
            {
                string state = stateList[i] ;
                CheckBox cb = new CheckBox();
                cb.Location = new Point(20, 0);
                cb.AutoEllipsis = true;
                cb.AutoSize = false;
                cb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                cb.Size = new Size(width, height);
                cb.Name = "cb" + i;

                cb.Text = state;

                listCheckBox.Add(cb);
                cb.Visible = false;
                this.pnlShow.Controls.Add(cb);
            }
            for (int i = 0; i < listCheckBox.Count; i++)
            {
                CheckBox box = listCheckBox[i] as CheckBox;
                box.Visible = true;
                box.Top = height * i;
            }

            //this.WindowState = FormWindowState.Maximized;
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);//上一页
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            List<string> resultList = GetCheck();
            if (resultList.Count == 0 || resultList == null)
            {
                //MessageBox.Show("请选择一个工程任务明细！");
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "请选择一个工程任务明细！";
                vmb.ShowDialog();
                return;
            }
            if (resultList.Count > 1)
            {
                //MessageBox.Show("");
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "只能选择一个工程任务明细！";
                vmb.ShowDialog();
                return;
            }
            result = resultList[0];
            this.Close();
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            page--;
            ShowData();
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            page++;
            ShowData();
        }

        List<string> GetCheck()
        {
            List<string> checkList = new List<string>();
            for (int i = 0; i < listCheckBox.Count; i++)
            {
                CheckBox box = listCheckBox[i] as CheckBox;
                if (box.Checked)
                {
                    string state = box.Text;
                    checkList.Add(state);
                }
            }
            return checkList;
        }

        private void VAddGWBSDetailList_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            ShowData();
        }
    }
}
