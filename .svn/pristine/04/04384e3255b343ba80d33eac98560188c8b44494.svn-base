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
    public partial class VAddGWBSDetailList : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private IList addGWBSDetailList = null;
        private int page = 1;
        private int pageCount = 0;
        private int pageSize = 3;
        List<CheckBox> listCheckBox = new List<CheckBox>();

        private GWBSDetail resultGWBSDetail;

        public GWBSDetail ResultGWBSDetail
        {
            get { return resultGWBSDetail; }
            set { resultGWBSDetail = value; }
        }

        public VAddGWBSDetailList(IList list)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            addGWBSDetailList = list;
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
            if (addGWBSDetailList.Count % pageSize == 0)
                pageCount = addGWBSDetailList.Count / pageSize;
            else
                pageCount = addGWBSDetailList.Count / pageSize + 1;

            
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
                endNum = addGWBSDetailList.Count;
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
                GWBSDetail dtl = addGWBSDetailList[i] as GWBSDetail;
                CheckBox cb = new CheckBox();
                cb.Location = new Point(20, 0);
                cb.AutoEllipsis = true;
                cb.AutoSize = false;
                cb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                cb.Size = new Size(width, height);
                cb.Name = "cb" + i;

                cb.Text = dtl.Name + " + " + dtl.MainResourceTypeName;
                if (dtl.TheCostItem != null && dtl.TheCostItem.Name != "")
                    cb.Text += "+" + dtl.TheCostItem.Name;

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
            IList resultList = GetCheck();
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
            resultGWBSDetail = resultList[0] as GWBSDetail;
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

        IList GetCheck()
        {
            IList checkList = new ArrayList();
            for (int i = 0; i < listCheckBox.Count; i++)
            {
                CheckBox box = listCheckBox[i] as CheckBox;
                if (box.Checked)
                {
                    int boxIndex = (page - 1) * pageSize + i;
                    GWBSDetail dtl = addGWBSDetailList[boxIndex] as GWBSDetail;
                    checkList.Add(dtl);
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
