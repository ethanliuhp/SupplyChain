using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using VirtualMachine.Component.Util;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using VirtualMachine.Core;


namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection
{
    public partial class VCorrectionReply : TBasicToolBarByMobile
    {
        MRectificationNoticeMng rm = new MRectificationNoticeMng();
        MDailyCorrection model = new MDailyCorrection();
        private AutomaticSize automaticSize = new AutomaticSize();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        InspectionRecord record = new InspectionRecord();
        RectificationNoticeDetail detail = new RectificationNoticeDetail();
    

        int page = 0;
        public int l = 0;

        IList list = new ArrayList();
        public VCorrectionReply(int pageIndex, IList listRecord)
        {
            l = pageIndex;
            list = listRecord;
            InitializeComponent();
            automaticSize.SetTag(this);
            InitialEvent();
            InitData();
            Contents();
        }
        private void InitData()
        {
            txtResult.Items.AddRange(new object[] { "整改中", "整改未通过" ,"整改通过"});
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
            try{
                 //if (detail.Master.DocState == DocumentState.Valid)
                 //   {
 
                 //   }
                 //   if (detail.Master.DocState == DocumentState.InAudit)
                 //   {
                        //if (detail.RectConclusion == 1)
                        //{
                            detail.RectContent = ClientUtil.ToString(this.txtStepMark.Text);
                            string strResult = txtResult.Text;
                                if (strResult == "整改通过")
                            {
                                detail.RectConclusion = 2;
                            }
                            if (strResult == "整改未通过")
                            {
                                detail.RectConclusion = 1;
                            }
                            if (strResult == "整改中")
                            {
                                detail.RectConclusion = 0;
                            }
                            detail.RectDate = ConstObject.LoginDate;
                            detail = rm.SaveRectificationNotice(detail);
                            VMessageBox v_show = new VMessageBox();
                            v_show.txtInformation.Text = "保存成功！";
                            v_show.ShowDialog();
                            return;
                        //}
                    //}
                    foreach (RectificationNoticeDetail dtl in list)
                    {
                        if (dtl.RectConclusion == 0)
                        {
                            return;
                        }
                    }
                    RectificationNoticeMaster mat = detail.Master as RectificationNoticeMaster;
                    mat.DocState = DocumentState.Suspend;
                    mat = rm.SaveRectificationNotice(mat);
                    
                }
            
        catch (Exception b)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(b));
            }
    }
            

        
        public override void 功能菜单2Item_Click(object sender, EventArgs e)
        { 
             if (!rm.RectificationNoticeSrv.DeleteByDao(detail)) return ;
             VMessageBox v_show = new VMessageBox();
             v_show.txtInformation.Text = "删除成功！";
             v_show.ShowDialog();
             return;
        }
        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnAfter_Click(object sender, EventArgs e)
        {
            if (l + 1 > list.Count) return;
            if (l + 1 == list.Count)
            {
                btnAfter.Enabled = false;
                btnEnd.Enabled = false;

            }
            btnBefore.Enabled = true;
            btnStart.Enabled = true;
            detail = list[l] as RectificationNoticeDetail;
            txtStepMark.Text = ClientUtil.ToString(detail.RectContent);
            //txtResult.Text = ClientUtil.ToString(detail.RectConclusion);
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
            detail = list[l - 2] as RectificationNoticeDetail;
            txtStepMark.Text = ClientUtil.ToString(detail.RectContent);
            //txtResult.Text = ClientUtil.ToString(detail.RectConclusion);
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
        void btnEnd_Click(object sender, EventArgs e)
        {
            btnBefore.Enabled = true;
            btnStart.Enabled = true;
            btnAfter.Enabled = false;
            btnEnd.Enabled = false;
            detail = list[list.Count - 1] as RectificationNoticeDetail;
            txtStepMark.Text = ClientUtil.ToString(detail.RectContent);
            //txtResult.Text = ClientUtil.ToString(detail.RectConclusion);
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
   public  override   void TBtnPageDown_Click(object sender, EventArgs e)
        {
            base.TBtnPageDown_Click(sender, e);

        }
  public  override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);
            this.Close();
        }

        void weekDetail()
  {
              ObjectQuery oq = new ObjectQuery();
              oq.AddCriterion(Expression.IsNotNull("ForwordInsLot"));
              oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
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
              label11.Text = "共" + list.Count + "条]";

            detail = list[l-1] as RectificationNoticeDetail;
            txtStepMark.Text = ClientUtil.ToString(detail.RectContent);
            int i = detail.RectConclusion;
            if (i == 0)
            {
                txtResult.Text ="整改中";
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

        private void VCorrectionReply_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
     
    }
}
