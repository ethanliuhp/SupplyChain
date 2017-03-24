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
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord
{
    public partial class VProInRecordMng : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        MProRecord model = new MProRecord();
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        private ProfessionInspectionRecordMaster curBillMaster;
        CurrentProjectInfo proInfo = null;
        GWBSTree w;
        IList selectedList;
        SubContractType type;
        int flag;
        SubContractProject master = new SubContractProject();//分包项目
        public VProInRecordMng()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitDate();
            Initevent();
        }

        private void Initevent()
        {
            btnTeamSelect.Click += new EventHandler(btnTeamSelect_Click);
               
        }

        void btnTeamSelect_Click(object sender, EventArgs e)
        {
            VChoiseTeam v_team = new VChoiseTeam(proInfo, w, selectedList, type, flag);
            v_team.ShowDialog();
            IList list = v_team.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtConstructionTeam.Text = engineerMaster.BearerOrgName;
            txtConstructionTeam.Tag = engineerMaster;
        }

       
        /// <summary>
        /// 当前单据
        /// </summary>
        public ProfessionInspectionRecordMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        private void InitDate()
        {
            DateTimePicker dp = new DateTimePicker();
            DataGridViewCalendarColumn dc = new DataGridViewCalendarColumn();
            dp.CustomFormat = "yyyy-MM-dd";
            dp.Format = DateTimePickerFormat.Custom;
            dp.Visible = false;
            //dgDetail.Controls.Add(dp);
            dp = new DateTimePicker();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_ProfessionalConstruction);//检查专业
            //BasicDataOptr bdo = new BasicDataOptr();
            //SearchTaskHandler();//承担队伍
            SearchProject(list);//专业检查
            SearchPersonByjob();
            cbState.Text = ClientUtil.GetDocStateName(DocumentState.Edit);
        }

        private void SearchPersonByjob()
        {
            //cbPerson.Tag = ConstObject.LoginPersonInfo;
            cbPerson.Text = ConstObject.LoginPersonInfo.Name;//当前登录人员
            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

        }
        /// <summary>
        /// 把获得的专业检查的数据添加到cbProject中
        /// </summary>
        /// <param name="list"></param>
        private void SearchProject(IList list)
        {
            if (list!=null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    BasicDataOptr bdo = list[i] as BasicDataOptr;
                    cbProject.Items.Add(bdo.BasicName);
                    cbProject.Text = bdo.BasicName;
                }
            }
        }
        /// <summary>
        /// 上一步，返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);
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

            menuForm.ControlsClear(true);
        }
        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
      
            ObjectQuery oq = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));// 项目Id
            oq.AddCriterion(Expression.Eq("Master.DocState", DocumentState.Edit));
            //oq.AddCriterion(Expression.Eq("Master.CreatePersonName",cbPerson.Text)); //操作人
            oq.AddCriterion(Expression.Eq("InspectionSupplierName", txtConstructionTeam.Text));//承担队伍
            IList list = model.ProfessionInspectionSrv.ObjectQuery(typeof(ProfessionInspectionRecordDetail), oq);
            if (list.Count == 0 || list == null)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据为空！";
                v_show.ShowDialog();
                return;
            }
            VProRecord vp = new VProRecord(list,cbProject.Text);
            vp.ShowDialog();
        }
       
        /// <summary>
        /// 窗体更具界面调整的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VProInRecordMng_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        ///// <summary>
        ///// 查询分包项目，获得承担队伍
        ///// </summary>
        //public void SearchTaskHandler() 
        //{
        //    ObjectQuery oq = new ObjectQuery();
        //    IList list = new ArrayList();
        //    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        //    oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
        //    //oq.AddCriterion(Expression.Ge("CreateTime", dtpDateBegin.Value.Date));
        //    //oq.AddCriterion(Expression.Lt("CreateTime", dtpDateEnd.Value.AddDays(1).Date));
        //    try
        //    {
        //        list = model.ContractExcuteSrv.GetContractExcute(oq);
        //        ShowMasterList(list);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("查询数据出错。\n" + ex.Message);
        //    }
        //}
        ///// <summary>
        ///// 显示承担队伍名称
        ///// </summary>
        ///// <param name="list"></param>
        //private void ShowMasterList(IList list)
        //{
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        master = list[i] as SubContractProject;
        //        //cbTeam.Tag = master.BearerOrg;
        //        //cbTeam.Items.Add(master.BearerOrgName);
        //        //cbTeam.Text = master.BearerOrgName;
        //    }
        //}
    }
}
