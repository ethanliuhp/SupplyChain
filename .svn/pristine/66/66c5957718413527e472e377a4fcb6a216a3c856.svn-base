using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;


namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection
{
    public partial class VProblemRequire : TBasicToolBarByMobile
    {
        MDailyCorrection model = new MDailyCorrection();
        private AutomaticSize automaticSize = new AutomaticSize();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        InspectionRecord record = new InspectionRecord();
        RectificationNoticeDetail detail = new RectificationNoticeDetail();
        MRectificationNoticeMng rm = new MRectificationNoticeMng();
        int page = 0;
        public  int l = 0;

        IList list = new ArrayList();

        public VProblemRequire(int pageIndex, IList listRecord)
        {
            l = pageIndex;
            list = listRecord;
            InitializeComponent();
            automaticSize.SetTag(this);
            InitialEvent();
            Contents();
        }
        public void InitialEvent()
        {
            weekDetail();
            btnAfter.Click += new EventHandler(btnAfter_Click);
            btnBefore.Click += new EventHandler(btnBefore_Click);
            btnEnd.Click += new EventHandler(btnEnd_Click);
            btnStart.Click += new EventHandler(btnStart_Click);
       
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
        public override void 功能菜单1Item_Click(object sender, EventArgs e)
        {
            //try
            //{
                
            //        detail.RectContent = ClientUtil.ToString(this.txtStepMark.Text);
            //        detail.RectConclusion = Convert.ToInt32(this.txtResult.Text);
            //        detail = rm.SaveRectificationNotice(detail);
            //        MessageBox.Show("保存成功");
              
            //}
            //catch (Exception b)
            //{
            //    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(b));
            //}
        }

        public override void 功能菜单2Item_Click(object sender, EventArgs e)
        {
            if (!rm.RectificationNoticeSrv.DeleteByDao(detail)) return;
            VMessageBox v_show = new VMessageBox();
            v_show.txtInformation.Text = "删除成功！";
            v_show.ShowDialog();
            return;

            if (list.Count - 1 == 0)
            {
                txtProblem.Text = "";
                txtRequire.Text = "";
                txtFnDate.Text = "";
                label10.Text = "[第" + (l - 1) + "条";
                label11.Text = "共" + (list.Count - 1) + "条]";
            }
            else
            {
                label10.Text = "[第" + (l - 1) + "条";
                label11.Text = "共" + (list.Count - 1) + "条]";
                list.Remove(detail);
                if (l != 0)
                {
                    detail = list[l - 1] as RectificationNoticeDetail;
                    txtProblem.Text = ClientUtil.ToString(detail.QuestionState);
                    txtRequire.Text = ClientUtil.ToString(detail.Rectrequired);
                    txtFnDate.Text = ClientUtil.ToString(detail.RequiredDate);
                }
            }
        }
        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            this.Close();
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
             txtProblem.Text = ClientUtil.ToString(detail.QuestionState);
             txtRequire.Text = ClientUtil.ToString(detail.Rectrequired);
             txtFnDate.Text = ClientUtil.ToString(detail.RequiredDate);
             label10.Text = "[第" + (l + 1) + "条";
             l++;
         }
         void btnBefore_Click(object sender, EventArgs e)
          {
              if (l - 1 == 0) return;
              if (l - 1 == 1)
              {
                  btnBefore.Enabled = false;
                  btnStart.Enabled = false;

              }
              btnAfter.Enabled = true;
              btnEnd.Enabled = true;
              RectificationNoticeDetail detail = list[l - 2] as RectificationNoticeDetail;
              txtProblem.Text = ClientUtil.ToString(detail.QuestionState);
              txtRequire.Text = ClientUtil.ToString(detail.Rectrequired);
              txtFnDate.Text = ClientUtil.ToString(detail.RequiredDate);
              label10.Text = "[第" + (l - 1) + "条";
              l--;
         }
         void btnEnd_Click(object sender, EventArgs e)
         {
             btnBefore.Enabled = true;
             btnStart.Enabled = true;
             btnAfter.Enabled = false;
             btnEnd.Enabled = false;
             RectificationNoticeDetail detail = list[list.Count - 1] as RectificationNoticeDetail;
             txtProblem.Text = ClientUtil.ToString(detail.QuestionState);
             txtRequire.Text = ClientUtil.ToString(detail.Rectrequired);
             txtFnDate.Text = ClientUtil.ToString(detail.RequiredDate);
             label10.Text = "[第" + (list.Count) + "条";
             l = list.Count;
         }
         void btnStart_Click(object sender, EventArgs e)
         {
             weekDetail();
             btnBefore.Enabled = false;
             btnStart.Enabled = false;
             btnAfter.Enabled = true;
             btnEnd.Enabled = true;
             l = 1;
         }
    public override  void TBtnPageDown_Click(object sender, EventArgs e)
          {
              base.TBtnPageDown_Click(sender, e);
              VCorrectionReply vcr = new VCorrectionReply(l, list);
              vcr.ShowDialog();
              l = vcr.l;
              label10.Text = "[第" + l + "条";
              RectificationNoticeDetail detail = list[l-1] as RectificationNoticeDetail;
              txtProblem.Text = ClientUtil.ToString(detail.QuestionState);
              txtRequire.Text = ClientUtil.ToString(detail.Rectrequired);
              txtFnDate.Text = ClientUtil.ToString(detail.RequiredDate);

         }
      public override  void TBtnPageUp_Click(object sender, EventArgs e)
          {
              base.TBtnPageUp_Click(sender, e);
              this.Close();
          }
        
             void weekDetail()
        {
            if (list.Count == 1)
            {
                btnBefore.Enabled = false;
                btnStart.Enabled = false;
                btnAfter.Enabled = false;
                btnEnd.Enabled = false;
            }
            label10.Text = "[第" + l + "条";
            label11.Text = "共" + list.Count + "条]";
             detail = list[l-1] as RectificationNoticeDetail;
             txtProblem.Text=ClientUtil.ToString(detail.QuestionState);
              txtRequire.Text = ClientUtil.ToString(detail.Rectrequired);
              if (detail.RequiredDate > ClientUtil.ToDateTime("0001-01-01"))
              {
                  txtFnDate.Text = ClientUtil.ToString(detail.RequiredDate.ToShortDateString());
              }
        }

             private void VProblemRequire_Load(object sender, EventArgs e)
             {
                 this.WindowState = FormWindowState.Maximized;
             }
        }

}
