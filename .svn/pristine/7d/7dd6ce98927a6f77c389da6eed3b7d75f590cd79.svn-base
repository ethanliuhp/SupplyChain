using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch
{
    public partial class VDailyCorrectionSearch : TBasicToolBarByMobile
    {
        MRectificationNoticeMng rm = new MRectificationNoticeMng();
        MDailyCorrectioSearch model = new MDailyCorrectioSearch();
        private AutomaticSize automaticSize = new AutomaticSize();
        //WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        InspectionRecord record = new InspectionRecord();
        RectificationNoticeDetail detail = new RectificationNoticeDetail();
        int page = 0;
        int l = 0;
        string strBtn = "";
        IList list = new ArrayList();
        public VDailyCorrectionSearch(string btn)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            strBtn = btn;
            //this.list = lists;
            //this.page = pageIndex;
            InitialEvent();
            Contents();
        }
        public void InitialEvent()
        {
            ShowDailyCorrection();
            btnAfter.Click += new EventHandler(btnAfter_Click);
            btnBefore.Click += new EventHandler(btnBefore_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);
            btnStart.Click += new EventHandler(btnStart_Click);
        }

        public void Contents()
        { 
            this.功能菜单2Item.Visible = true;
            this.功能菜单2Item.Text = "删除整改单查询";
            this.功能菜单3Item.Visible = true;
            this.功能菜单3Item.Text = "放弃整改单查询";
        }
        public override void 功能菜单2Item_Click(object sender, EventArgs e)
        {
            if (!rm.RectificationNoticeSrv.DeleteByDao(detail)) return;
            VMessageBox v_show = new VMessageBox();
            v_show.txtInformation.Text = "删除成功！";
            v_show.ShowDialog();
            return;
        }
        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //上一页
          public  override  void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);
            //TBtnPageUp.Enabled = false;
            //MessageBox.Show("当前是第一页");
            this.Close();
        }

        //首条
        void btnStart_Click(object sender, EventArgs e)
        {
            ShowDailyCorrection();
            btnBefore.Enabled = false;
            btnStart.Enabled = false;
            btnAfter.Enabled = true;
            btnEnd.Enabled = true;
            l = 1;
        }
        //末条
        void btnEnd_Click(object sender,EventArgs e)
        {
            btnBefore.Enabled = true;
            btnStart.Enabled = true;
            btnAfter.Enabled = false;
            btnEnd.Enabled = false;
            RectificationNoticeDetail detail = list[list.Count - 1] as RectificationNoticeDetail;
             txtAccepType.Text= ClientUtil.ToString("日常检查");
            txtInspectionPerson.Text=ClientUtil.ToString(detail.Master.CreatePersonName);
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtBeginDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtEndDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
            txtSuppiler.Text=ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtFnDate.Text= ClientUtil.ToString(detail.RequiredDate);
            txtState.Text = ClientUtil.GetDocStateName(detail.Master.DocState); 
           int i = detail.RectConclusion;
            if (i == 0)
            {
                txtResult.Text = "整改中";
            }
            if (i == 1)
            {
                txtResult.Text = "整改未通过";
            }
            if (i == 2)
            {
                txtResult.Text = "整改通过";
            }
            label10.Text = "[第" + (list.Count) + "条";
            l = list.Count;
        }
        //上一条
         void btnBefore_Click(object sender,EventArgs e)
        {
            if (l - 1 == 0) return;
            if (l - 1 == 1)
            {
                btnBefore.Enabled = false;
                btnStart.Enabled = false;
                
            }
            btnAfter.Enabled = true;
            btnEnd.Enabled = true;
            RectificationNoticeDetail detail = list[l-2] as RectificationNoticeDetail;
              txtAccepType.Text= ClientUtil.ToString("日常检查");
            txtInspectionPerson.Text=ClientUtil.ToString(detail.Master.CreatePersonName);
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtBeginDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtEndDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
            txtSuppiler.Text=ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtFnDate.Text= ClientUtil.ToString(detail.RequiredDate);
            txtState.Text = ClientUtil.GetDocStateName(detail.Master.DocState); 
           int i = detail.RectConclusion;
            if (i == 0)
            {
                txtResult.Text = "整改中";
            }
            if (i == 1)
            {
                txtResult.Text = "整改未通过";
            }
            if (i == 2)
            {
                txtResult.Text = "整改通过";
            }
            label10.Text = "[第" + (l - 1) + "条";
            l--;
        }
        //下一条
        void btnAfter_Click(object sender,EventArgs e)
        {
            
            if (l + 1 > list.Count) return;
            if (l + 1 == list.Count)
            {
                btnAfter.Enabled = false;
                btnEnd.Enabled = false;
               
            }
            btnBefore.Enabled = true;
            btnStart.Enabled = true;
            RectificationNoticeDetail detail = list[l] as RectificationNoticeDetail;
             txtAccepType.Text= ClientUtil.ToString("日常检查");
            txtInspectionPerson.Text=ClientUtil.ToString(detail.Master.CreatePersonName);
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtBeginDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtEndDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
            txtSuppiler.Text=ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtFnDate.Text= ClientUtil.ToString(detail.RequiredDate.ToShortDateString());
            txtState.Text = ClientUtil.GetDocStateName(detail.Master.DocState); 
           int i = detail.RectConclusion;
            if (i == 0)
            {
                txtResult.Text = "整改中";
            }
            if (i == 1)
            {
                txtResult.Text = "整改未通过";
            }
            if (i == 2)
            {
                txtResult.Text = "整改通过";
            }
            label10.Text = "[第" + (l + 1) + "条";
            l++;
        }
        //加载显示数据
        void ShowDailyCorrection()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.IsNotNull("ForwordInsLot"));
            oq.AddFetchMode("ForwordInsLot", NHibernate.FetchMode.Eager);
            list = model.RectificationNoticeSrv.GetRectificationDetail(oq);
            if (list.Count == 1)
            {
                this.btnBefore.Enabled = false;
                this.btnStart.Enabled = false;
                this.btnAfter.Enabled = false;
                btnEnd.Enabled = false;
            }
            label10.Text = "[第1条";
            page = 1;
            l = page;
            label11.Text = "共" + list.Count + "条]";
            if (list.Count == 0) return;
            RectificationNoticeDetail detail = list[0] as RectificationNoticeDetail;
            txtAccepType.Text= ClientUtil.ToString("日常检查");
            txtInspectionPerson.Text=ClientUtil.ToString(detail.Master.CreatePersonName);
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtBeginDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
              if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtEndDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
            txtSuppiler.Text=ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            if (detail.RequiredDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtFnDate.Text = ClientUtil.ToString(detail.RequiredDate.ToShortDateString());
            }
            txtState.Text = ClientUtil.GetDocStateName(detail.Master.DocState); 
           int i = detail.RectConclusion;
            if (i == 0)
            {
                txtResult.Text = "整改中";
            }
            if (i == 1)
            {
                txtResult.Text = "整改未通过";
            }
            if (i == 2)
            {
                txtResult.Text = "整改通过";
            }
            }
        //下一页
        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            //if (this.txtAccepType != "")
            //{
            //    objectQuery.AddCriterion(Expression.Like("Master.PlanName", this.txtAccepType.Text, MatchMode.Anywhere));
            //}
            if (this.txtInspectionPerson.Text!= "")
            {
                objectQuery.AddCriterion(Expression.Like("Master.CreatePersonName", this.txtInspectionPerson.Text, MatchMode.Anywhere));
            }
            if (this.txtSuppiler.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("ForwordInsLot.BearTeamName", this.txtSuppiler.Text, MatchMode.Anywhere));
            }
            if (this.txtFnDate.Text!= "")
            {
                objectQuery.AddCriterion(Expression.Eq("RequiredDate", ClientUtil.ToDateTime(this.txtFnDate.Text)));
            }
            if (this.txtState.Text != "")
            {
                //objectQuery.AddCriterion(Expression.Like("Master.DocState", EnumUtil<DocumentState>.FromDescription(this.txtState.Text).ToString(), MatchMode.Anywhere));
                //objectQuery.AddCriterion(Expression.Like("Master.DocState", （EnumUtil<DocumentState>.FromDescription(this.txtState.Text)）.ToString(), MatchMode.Anywhere));

                 //this.txtState.Text
            }

            IList list = model.RectificationNoticeSrv.GetRectificationDetail(objectQuery);
            if (list.Count == 0 || list == null)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据为空！";
                v_show.ShowDialog();
                return;
            }
            VDailyCorrectionMaster v_result = new VDailyCorrectionMaster(list, page);
            v_result.Show();
        
        }


        private void VDailyCorrectionSearch_Load(object sender, EventArgs e)
        {
             this.WindowState = FormWindowState.Maximized;
        }
        
        

    }
}
