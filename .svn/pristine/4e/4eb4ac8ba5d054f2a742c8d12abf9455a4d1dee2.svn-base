using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection
{
    public partial class VDailyCorrectionMaster : TBasicToolBarByMobile
    {
        MRectificationNoticeMng rm = new MRectificationNoticeMng();
        MDailyCorrection model = new MDailyCorrection();
        private AutomaticSize automaticSize = new AutomaticSize();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        InspectionRecord record = new InspectionRecord();
        RectificationNoticeDetail detail = new RectificationNoticeDetail();
        int page = 0;
        int l = 0;
        IList list = new ArrayList();


        public GWBSTree DefaultGWBSTree = null;

        public VDailyCorrectionMaster(IList lists,int pageIndex)
        {
           
            InitializeComponent();
            automaticSize.SetTag(this);
            this.list = lists;
            this.page = pageIndex;
            InitialEvent();
            Contents();
        }
        public void InitialEvent()
        {
            weekDetail();
            btnAfter.Click +=new EventHandler(btnAfter_Click);
            btnBefore.Click +=new EventHandler(btnBefore_Click);
            btnEnd.Click +=new EventHandler(btnEnd_Click);
            btnStart.Click +=new EventHandler(btnStart_Click);
        }

        public void Contents()
        {
            this.功能菜单1Item.Visible = true;
            this.功能菜单1Item.Text = "保存整改单确认";
            this.功能菜单2Item.Visible = true;
            this.功能菜单2Item.Text = "删除整改单确认";
            this.功能菜单3Item.Visible = true;
            this.功能菜单3Item.Text = "放弃整改单确认";
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

    public override  void TBtnPageDown_Click(object sender, EventArgs e)
        {
            base.TBtnPageDown_Click(sender, e);
            VProblemRequire vpr = new VProblemRequire(l ,list);
            vpr.ShowDialog();
           l = vpr.l;
            label10.Text = "[第" + l + "条";
            RectificationNoticeDetail detail = list[l-1] as RectificationNoticeDetail;

            txtTaskName.Text = ClientUtil.ToString(detail.ForwordInsLot.GWBSTreeName);
            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.ForwordInsLot.GWBSTreeName, (detail.ForwordInsLot.GWBSTree as GWBSTree).SysCode);
            this.txtTaskPath.Text = fullPath;

            txtSep.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionSpecial);
            txtSuppiler.Text = ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtAccepType.Text = ClientUtil.ToString("日常检查");

            txtCreateDate.Text = ClientUtil.ToString(detail.RectSendDate);
            txtInspectionPerson.Text = ClientUtil.ToString(detail.Master.CreatePersonName);
            txtState.Text = ClientUtil.GetDocStateName(detail.Master.DocState);
            txtConclusion.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionConclusion);
            //weekdetail();


        
        }
    public  override  void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);
            //TBtnPageUp.Enabled = false;
            //MessageBox.Show("当前是第一页");
            this.Close();
        }



        void btnStart_Click(object sender ,EventArgs e)
        {
            weekDetail();
            btnBefore.Enabled = false;
            btnStart.Enabled = false;
            btnAfter.Enabled = true;
            btnEnd.Enabled = true;
            l = 1;
        }

        void btnEnd_Click(object sender,EventArgs e)
        {
            btnBefore.Enabled = true;
            btnStart.Enabled = true;
            btnAfter.Enabled = false;
            btnEnd.Enabled = false;
            RectificationNoticeDetail detail = list[list.Count - 1] as RectificationNoticeDetail;

            txtTaskName.Text = ClientUtil.ToString(detail.ForwordInsLot.GWBSTreeName);
            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.ForwordInsLot.GWBSTreeName, (detail.ForwordInsLot.GWBSTree as GWBSTree).SysCode);
            this.txtTaskPath.Text = fullPath;

            txtSep.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionSpecial);
            txtSuppiler.Text = ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtAccepType.Text = ClientUtil.ToString("日常检查");

            txtCreateDate.Text = ClientUtil.ToString(detail.RectSendDate);
            txtInspectionPerson.Text = ClientUtil.ToString(detail.Master.CreatePersonName);
            txtState.Text = ClientUtil.ToString(detail.Master.DocState);
            txtConclusion.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionConclusion);
            label10.Text = "[第" + (list.Count) + "条";
            l = list.Count;
        }

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

            txtTaskName.Text = ClientUtil.ToString(detail.ForwordInsLot.GWBSTreeName);
            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.ForwordInsLot.GWBSTreeName, (detail.ForwordInsLot.GWBSTree as GWBSTree).SysCode);
            this.txtTaskPath.Text = fullPath;

            txtSep.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionSpecial);
            txtSuppiler.Text = ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtAccepType.Text = ClientUtil.ToString("日常检查");

            txtCreateDate.Text = ClientUtil.ToString(detail.RectSendDate);
            txtInspectionPerson.Text = ClientUtil.ToString(detail.Master.CreatePersonName);
            txtState.Text = ClientUtil.ToString(detail.Master.DocState);
            txtConclusion.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionConclusion);
            label10.Text = "[第" + (l-1) + "条";
            l--;
        }

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

            txtTaskName.Text = ClientUtil.ToString(detail.ForwordInsLot.GWBSTreeName);
            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.ForwordInsLot.GWBSTreeName, (detail.ForwordInsLot.GWBSTree as GWBSTree).SysCode);
            this.txtTaskPath.Text = fullPath;

            txtSep.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionSpecial);
            txtSuppiler.Text = ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtAccepType.Text = ClientUtil.ToString("日常检查");
            txtCreateDate.Text = ClientUtil.ToString(detail.RectSendDate);
            txtInspectionPerson.Text = ClientUtil.ToString(detail.Master.CreatePersonName);
            txtState.Text = ClientUtil.ToString(detail.Master.DocState); ;
            txtConclusion.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionConclusion);
            label10.Text = "[第"+ (l + 1) +"条";
            l++;
        }

        void weekDetail()
        {
            WeekScheduleDetail dtl = list[0] as WeekScheduleDetail;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.IsNotNull("ForwordInsLot"));
            //oq.AddCriterion(Expression.Eq("ForwordInsLot.WeekScheduleDetail", dtl));
            list = model.RectificationNoticeSrv.GetRectificationDetail(oq);
            if (list.Count == 1)
            {
                btnBefore.Enabled = false;
                btnStart.Enabled = false;
                btnAfter.Enabled = false;
                btnEnd.Enabled = false;
            }
            label10.Text = "[第1条";
            page = 1;
            l = page;
            label11.Text = "共"+ list.Count +"条]";
            if (list.Count == 0) return;
            RectificationNoticeDetail detail = list[0] as RectificationNoticeDetail;

            txtTaskName.Text = ClientUtil.ToString(detail.ForwordInsLot.GWBSTreeName);
            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.ForwordInsLot.GWBSTreeName, (detail.ForwordInsLot.GWBSTree as GWBSTree).SysCode);
            this.txtTaskPath.Text = fullPath;

            txtSep.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionSpecial);
            txtSuppiler.Text = ClientUtil.ToString(detail.ForwordInsLot.BearTeamName);
            txtAccepType.Text = ClientUtil.ToString("日常检查");
            if (detail.RectSendDate > ClientUtil.ToDateTime("0001-01-01"))
            {
                txtCreateDate.Text = ClientUtil.ToString(detail.RectSendDate.ToShortDateString());
            }
           
            txtInspectionPerson.Text = ClientUtil.ToString(detail.Master.CreatePersonName);

            txtState.Text = ClientUtil.GetDocStateName(detail.Master.DocState); 
            txtConclusion.Text = ClientUtil.ToString(detail.ForwordInsLot.InspectionConclusion);
        }

        private void VDailyCorrectionMaster_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

    }

}
    

