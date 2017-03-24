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
    public partial class VTaskHandler : TBasicToolBarByMobile
    {
        //public GWBSTaskConfirm gbstk = null;
        private AutomaticSize automaticSize = new AutomaticSize();
        MConfirmmng model = new MConfirmmng();

        private List<BearersProjectAmount> bpaList = null;
        CurrentProjectInfo projectInfo;//当前操作项目
        //private BearersProjectAmount bpa = new BearersProjectAmount();
        GWBSTaskConfirm taskConfirm = null;

        private int pageCount = 0;
        private int pageSize = 2;
        private int page = 1;

        private List<Panel> pnlList = new List<Panel>();
        private List<TextBox> actualCompletedQuantityTxtList = new List<TextBox>();
        private List<TextBox> progressAfterConfirmTxtList = new List<TextBox>();

        //返回参数
        public IList addTaskConfirmList = new ArrayList();
        public IList updateTaskConfirmList = new ArrayList();
        public IList deleteTaskConfirmList = new ArrayList();


        private IList result;
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }



        public VTaskHandler(List<BearersProjectAmount> owerList, GWBSTaskConfirm taskCon)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            var list = from bpa in owerList
                       orderby bpa.ProjectAmount descending
                       select bpa;
            bpaList = list.ToList();
            taskConfirm = taskCon;
            InitEvent();
            InitDate();
        }
        void InitDate()
        {
            pageCount = bpaList.Count % pageSize == 0 ? bpaList.Count / pageSize : bpaList.Count / pageSize + 1;
            ViewToModel();
        }

        void InitEvent()
        {
            btnBack.Click += new EventHandler(btnBack_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            //btnCansle.Click += new EventHandler(btnCansle_Click);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            //foreach (BearersProjectAmount bpa in bpaList)
            for (int i = 0; i < bpaList.Count; i++)
            {
                BearersProjectAmount bpa = bpaList[i] as BearersProjectAmount;
                if (bpa.ProjectAmount != 0 && bpa.ProjectTaskConfirmDetail == null)
                {
                    GWBSTaskConfirm con = new GWBSTaskConfirm();
                    con.Master = taskConfirm.Master;

                    con.GWBSTree = taskConfirm.GWBSTree;
                    con.GWBSTreeName = taskConfirm.GWBSTreeName;
                    con.GwbsSysCode = taskConfirm.GWBSDetail.TheGWBSSysCode;

                    con.GWBSDetail = taskConfirm.GWBSDetail;
                    con.GWBSDetailName = taskConfirm.GWBSDetailName;

                    con.TaskHandler = bpa.ProjectSubContract;
                    con.TaskHandlerName = bpa.ProjectSubContract.BearerOrgName;

                    con.WorkAmountUnitId = taskConfirm.GWBSDetail.WorkAmountUnitGUID;
                    con.WorkAmountUnitName = taskConfirm.GWBSDetail.WorkAmountUnitGUID.Name;

                    if (!string.IsNullOrEmpty(taskConfirm.WeekScheduleDetailGUID.TaskCheckState))
                        con.DailyCheckState = "2" + taskConfirm.WeekScheduleDetailGUID.TaskCheckState.Substring(1);

                    con.CostItem = taskConfirm.CostItem;
                    con.CostItemName = taskConfirm.CostItem.Name;

                    con.WeekScheduleDetailGUID = taskConfirm.WeekScheduleDetailGUID;

                    con.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                    //con.DailyCheckState = "2" + taskConfirm.WeekScheduleDetailGUID.TaskCheckState.Substring(1);
                    con.PlannedQuantity = taskConfirm.GWBSDetail.PlanWorkAmount;
                    con.QuantityBeforeConfirm = taskConfirm.GWBSDetail.QuantityConfirmed;
                    con.ActualCompletedQuantity = bpa.ProjectAmount;
                    con.QuantiyAfterConfirm = con.QuantityBeforeConfirm + con.ActualCompletedQuantity;
                    con.ProgressBeforeConfirm = taskConfirm.GWBSDetail.ProgressConfirmed;
                    con.ProgressAfterConfirm = bpa.ProgressAfterConfirm;
                    con.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.不结算;
                    con.RealOperationDate = DateTime.Now;
                    addTaskConfirmList.Add(con);
                }
                if (bpa.ProjectAmount != 0 && bpa.ProjectTaskConfirmDetail != null)
                {
                    GWBSTaskConfirm con = bpa.ProjectTaskConfirmDetail;
                    con.ActualCompletedQuantity = ClientUtil.ToDecimal((actualCompletedQuantityTxtList[i] as TextBox).Text);
                    //con.ProgressAfterConfirm
                    updateTaskConfirmList.Add(con);
                }
                if (bpa.ProjectAmount == 0 && bpa.ProjectTaskConfirmDetail != null)
                {
                    deleteTaskConfirmList.Add(bpa.ProjectTaskConfirmDetail);
                }
            }
            this.Close();
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);//上一页
            this.Close();
        }


        void btnNext_Click(object sender, EventArgs e)
        {
            Panel p1 = pnlList[(page - 1)] as Panel;
            p1.Visible = false;
            page++;
            Panel p2 = pnlList[(page - 1)] as Panel;
            p2.Visible = true;

            //ViewToModel();
            if (page == pageCount)
                btnNext.Enabled = false;
            else
                btnNext.Enabled = true;

            if (page == 1)
                btnBack.Enabled = false;
            else
                btnBack.Enabled = true;
        }

        void btnBack_Click(object sender, EventArgs e)
        {

            Panel p1 = pnlList[(page - 1)] as Panel;
            p1.Visible = false;
            page--;
            Panel p2 = pnlList[(page - 1)] as Panel;
            p2.Visible = true;
            //ViewToModel();
            if (page == pageCount)
                btnNext.Enabled = false;
            else
                btnNext.Enabled = true;

            if (page == 1)
                btnBack.Enabled = false;
            else
                btnBack.Enabled = true;
        }




        /// <summary>
        /// 进界面显示多个队伍
        /// </summary>
        /// <param name="list1"></param>
        private void ViewToModel()
        {
            try
            {
                if (page == pageCount)
                    btnNext.Enabled = false;
                else
                    btnNext.Enabled = true;

                if (page == 1)
                    btnBack.Enabled = false;
                else
                    btnBack.Enabled = true;

                for (int i = 0; i < pageCount; i++)
                {
                    Panel pnl = new Panel();
                    pnl.Location = new Point(0, 0);
                    pnl.Name = "pnl" + (i + 1);
                    pnl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                    pnl.Size = new Size(312, 310);
                    pnl.BackColor = System.Drawing.Color.Red;
                    pnl.Visible = false;
                    pnlList.Add(pnl);
                    // pnlFloor.Controls.Add(pnl);
                    pnlShow.Controls.Add(pnl);

                    int height = 10;
                    for (int j = 0; j < pageSize; j++)
                    {
                        int index = i * pageSize + j;
                        BearersProjectAmount bpa = bpaList[index] as BearersProjectAmount;

                        Label lblTaskHandler = new Label();
                        Label lblQuantiyAfterConfirm = new Label();
                        TextBox txtQuantiyAfterConfirm = new TextBox();
                        Label lblProgressAfterConfirm = new Label();
                        TextBox txtProgressAfterConfirm = new TextBox();
                        Label lblPercent = new Label();

                        //第一个lable标签，显示当前队伍的信息
                        lblTaskHandler.Location = new Point(0, height);
                        lblTaskHandler.Name = "lblInfor" + (index + 1);
                        lblTaskHandler.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        lblTaskHandler.Size = new Size(310, 20);
                        lblTaskHandler.Text = bpa.ProjectSubContract.BearerOrgName + "  队伍已确认累计工程量：" + model.GetTheTeamQuantityConfirmed(projectInfo, taskConfirm.GWBSDetail, bpa.ProjectSubContract).ToString();
                        pnl.Controls.Add(lblTaskHandler);

                        //第二个lable标签，显示本次完成工作量
                        height += 50;
                        lblQuantiyAfterConfirm.Location = new Point(0, height);
                        lblQuantiyAfterConfirm.Name = "lblQuantiyAfterConfirm" + (index + 1);
                        lblQuantiyAfterConfirm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        lblQuantiyAfterConfirm.Size = new Size(130, 20);
                        lblQuantiyAfterConfirm.TextAlign = ContentAlignment.MiddleRight;
                        lblQuantiyAfterConfirm.Text = "本次完成工作量：";
                        pnl.Controls.Add(lblQuantiyAfterConfirm);

                        //第1个文本框：本次完成工作量
                        //txtQuantiyAfterConfirm.LostFocus += new EventHandler(txtQuantiyAfterConfirm_LostFocus);
                        txtQuantiyAfterConfirm.Location = new Point(130, height);
                        txtQuantiyAfterConfirm.Name = "txtQuantiyAfterConfirm" + (index + 1);
                        txtQuantiyAfterConfirm.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
                        txtQuantiyAfterConfirm.Size = new Size(150, 20);
                        pnl.Controls.Add(txtQuantiyAfterConfirm);
                        actualCompletedQuantityTxtList.Add(txtQuantiyAfterConfirm);

                        height += 50;
                        //第三个lable标签，已达到的累积形象进度
                        lblProgressAfterConfirm.Location = new Point(0, height);
                        lblProgressAfterConfirm.Name = "lblProgressAfterConfirm" + (index + 1);
                        lblProgressAfterConfirm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        lblProgressAfterConfirm.Size = new Size(130, 20);
                        lblProgressAfterConfirm.TextAlign = ContentAlignment.MiddleRight;
                        lblProgressAfterConfirm.Text = "达到的累积形象进度：";
                        pnl.Controls.Add(lblProgressAfterConfirm);

                        //第2个文本框：用来输入已达到的累积形象进度
                        //txtQuantiyAfterConfirm.LostFocus += new EventHandler(txtQuantiyAfterConfirm_LostFocus);
                        txtProgressAfterConfirm.Location = new Point(130, height);
                        txtProgressAfterConfirm.Name = "txtQuantiyAfterConfirm" + (index + 1);
                        txtProgressAfterConfirm.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
                        txtProgressAfterConfirm.Size = new Size(150, 20);
                        pnl.Controls.Add(txtProgressAfterConfirm);
                        progressAfterConfirmTxtList.Add(txtProgressAfterConfirm);

                        //第四个lable标签，百分比
                        lblPercent.Location = new Point(280, height);
                        lblPercent.Name = "lblPercent" + (index + 1);
                        lblPercent.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                        lblPercent.Size = new Size(10, 20);
                        //lblPercent.TextAlign = ContentAlignment.MiddleRight;
                        lblPercent.Text = "%";
                        pnl.Controls.Add(lblPercent);
                        height += 50;
                    }
                }

                Panel p = pnlList[(page - 1)] as Panel;
                p.Visible = true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// 本次确认工程量验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtQuantiyAfterConfirm_LostFocus(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

        }

        private void VTaskHandler_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            //ViewToModel();
        }
    }
}
