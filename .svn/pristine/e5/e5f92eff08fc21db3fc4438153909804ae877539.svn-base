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
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm
{
    public partial class VGwbsMng : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        MConfirmmng model = new MConfirmmng();

        private GWBSTaskConfirmMaster curBillMaster; //= new GWBSTaskConfirmMaster();
        public GWBSTaskConfirmMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        CurrentProjectInfo projectInfo;//当前操作项目
        private IList taskConfirmDetailList = null;//工程任务确认明细集
        private int index = 0;//当前条 在集合中的下标
        private GWBSTaskConfirm taskConfirmDetail = new GWBSTaskConfirm();//当前操作工程任务确认明细
        private WeekScheduleDetail weekDetail = new WeekScheduleDetail();//操作周进度计划明细

        private SubContractProject subContractPro = null;//承担队伍

        public IList weekList;
        public int pageIndex;

        public string subConId;


        public VGwbsMng()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitData();
            InitEvent();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="taskDtlList">工程任务明细集</param>
        public VGwbsMng(IList taskDtlList)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            taskConfirmDetailList = taskDtlList;
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            ShowData();
        }
        private void InitEvent()
        {
            btnback.Click += new EventHandler(btnback_Click);
            btnFirst.Click += new EventHandler(btnFirst_Click);
            btnnext.Click += new EventHandler(btnnext_Click);
            btnlast.Click += new EventHandler(btnlast_Click);
            txtTaskHandle.Click += new EventHandler(cbTeam_Click);
            txtActualCompletedQuantity.LostFocus += new EventHandler(txtActualCompletedQuantity_LostFocus);
            txtTheTeamQuantityBeforeConfirm.LostFocus += new EventHandler(txtTheTeamQuantityBeforeConfirm_LostFocus);
            txtProgressAfterConfirm.LostFocus += new EventHandler(txtProgressAfterConfirm_LostFocus);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSelect.Click += new EventHandler(btnSelect_Click);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            VMessageBox vbox = new VMessageBox();
            vbox.txtInformation.Text = taskConfirmDetail.GWBSTree.FullPath;
            vbox.ShowDialog();
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            VMessageBox vbox = new VMessageBox();
            vbox.txtInformation.Text = taskConfirmDetail.GWBSDetail.Name;
            vbox.ShowDialog();
        }

        void btnlast_Click(object sender, EventArgs e)
        {
            index = taskConfirmDetailList.Count - 1;
            ShowData();
        }

        void btnnext_Click(object sender, EventArgs e)
        {
            if (taskConfirmDetail.Id == null || txtTheTeamQuantityBeforeConfirm.Text != taskConfirmDetail.QuantiyAfterConfirm.ToString() || txtProgressAfterConfirm.Text != (taskConfirmDetail.ProgressBeforeConfirm + taskConfirmDetail.CompletedPercent).ToString())
            {
                VMessageYesOrNo show = new VMessageYesOrNo();
                show.txtInformation.Text = "是否要保存信息？";
                show.ShowDialog();
                if (show.yesOrNo)
                {
                    SaveView();
                }
            }
            index += 1;
            ShowData();
        }

        void btnFirst_Click(object sender, EventArgs e)
        {
            index = 0;
            ShowData();
        }

        void btnback_Click(object sender, EventArgs e)
        {
            if (taskConfirmDetail.Id == null || txtTheTeamQuantityBeforeConfirm.Text != taskConfirmDetail.QuantiyAfterConfirm.ToString() || txtProgressAfterConfirm.Text != (taskConfirmDetail.ProgressBeforeConfirm + taskConfirmDetail.CompletedPercent).ToString())
            {
                VMessageYesOrNo show = new VMessageYesOrNo();
                show.txtInformation.Text = "是否要保存信息？";
                show.ShowDialog();
                if (show.yesOrNo)
                {
                    SaveView();
                }
            }
            index -= 1;
            ShowData();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void 功能菜单1Item_Click(object sender, EventArgs e)
        {
            base.功能菜单1Item_Click(sender, e);
            SaveView();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void 功能菜单2Item_Click(object sender, EventArgs e)
        {
            base.功能菜单2Item_Click(sender, e);
            SubmitView();

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            base.功能菜单3Item_Click(sender, e);
            DeleteView();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void 功能菜单4Item_Click(object sender, EventArgs e)
        {
            base.功能菜单4Item_Click(sender, e);
            NewGWBSTaskConfirm();
            //AddView();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void 功能菜单5Item_Click(object sender, EventArgs e)
        {
            base.功能菜单5Item_Click(sender, e);
            this.Close();
        }

        /// <summary>
        /// 功能菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TBtnContents_Click(object sender, EventArgs e)
        {
            this.功能菜单1Item.Visible = true;
            this.功能菜单1Item.Text = "保存工程量确认单";

            this.功能菜单2Item.Visible = true;
            this.功能菜单2Item.Text = "提交工程量确认单";

            this.功能菜单3Item.Visible = true;
            this.功能菜单3Item.Text = "删除工程量确认单";

            this.功能菜单4Item.Visible = true;
            this.功能菜单4Item.Text = "新增工程量确认单";

            this.功能菜单5Item.Visible = true;
            this.功能菜单5Item.Text = "放弃工程量确认操作";
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            base.TBtnPageDown_Click(sender, e);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);
            this.Close();
        }

        /// <summary>
        /// 选择承担队伍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbTeam_Click(object sender, EventArgs e)
        {
            //IList selectList = new ArrayList();
            //ObjectQuery oq = new ObjectQuery();
            //if (taskConfirmDetail.TaskHandler != null)
            //{
            //    oq.AddCriterion(Expression.Eq("Id", taskConfirmDetail.TaskHandler.Id));
            //    oq.AddFetchMode("TheContractGroup", NHibernate.FetchMode.Eager);
            //    selectList = model.ProductionManagementSrv.ObjectQuery(typeof(SubContractProject), oq);
            //}
            //else
            //    selectList = null;
            //VChoiseTeam vc = new VChoiseTeam(projectInfo, taskConfirmDetail.GWBSTree, selectList, SubContractType.劳务分包, 1);
            //vc.ShowDialog();
            //IList result = vc.Result;

            //if (result.Count == 0 || result == null)
            //{
            //    return;
            //}
            //else if (result.Count <= 1)
            //{
            //    cbTeam.Items.Clear();
            //    SubContractProject sub = result[0] as SubContractProject;
            //    cbTeam.Items.Add(sub.BearerOrgName);
            //    cbTeam.SelectedIndex = 0;
            //    subContractPro = sub;
            //    txtTheTeamQuantityBeforeConfirm.Text = model.GetTheTeamQuantityConfirmed(projectInfo, taskConfirmDetail.GWBSDetail, sub).ToString();
            //}
            //else
            //{
            //    IList removeTaskConfirmList = new ArrayList();
            //    List<BearersProjectAmount> owerList = new List<BearersProjectAmount>(); //内存对象（承担者工程量集）
            //    foreach (SubContractProject sub in result)
            //    {
            //        BearersProjectAmount bpa = new BearersProjectAmount();
            //        bpa.ProjectSubContract = sub;
            //        bpa.ProjectAmount = 0;
            //        bpa.ProgressAfterConfirm = 0;

            //        foreach (GWBSTaskConfirm con in taskConfirmDetailList)
            //        {
            //            if (con.TaskHandler != null && con.TaskHandler.Id == sub.Id)
            //            {
            //                bpa.ProjectTaskConfirmDetail = con;
            //                bpa.ProjectAmount = con.ActualCompletedQuantity;
            //                bpa.ProgressAfterConfirm = con.ProgressAfterConfirm;
            //                if (bpa.ProjectAmount == 0)
            //                    removeTaskConfirmList.Add(con);
            //                break;
            //            }
            //        }
            //        owerList.Add(bpa);
            //    }
            //    //显示承担队伍信息填写界面
            //    VTaskHandler vth = new VTaskHandler(owerList, taskConfirmDetail);
            //    vth.ShowDialog();
            //    IList addTaskConfirmList = vth.addTaskConfirmList;
            //    IList updateTaskConfirmList = vth.updateTaskConfirmList;
            //    IList deleteTaskConfirmList = vth.deleteTaskConfirmList;
            //    IList resultAddList = model.OperatingAfterReturnAddList(addTaskConfirmList, updateTaskConfirmList, deleteTaskConfirmList);

            //    if (resultAddList != null && resultAddList.Count > 0)
            //    {
            //        foreach (GWBSTaskConfirm con in resultAddList)
            //        {
            //            taskConfirmDetailList.Add(con);
            //        }
            //    }
            //    if (updateTaskConfirmList != null && updateTaskConfirmList.Count > 0)
            //    {
            //    }
            //    if (deleteTaskConfirmList != null && deleteTaskConfirmList.Count > 0)
            //    {
            //        foreach (GWBSTaskConfirm con in deleteTaskConfirmList)
            //        {
            //            taskConfirmDetailList.Remove(con);
            //        }
            //    }
            //    ShowData();
            //}

            VChoiseTeam vct = new VChoiseTeam(projectInfo, null, null, SubContractType.劳务分包, 0);
            vct.ShowDialog();
            IList result = vct.Result;
            if (result != null && result.Count > 0)
            {
                SubContractProject subSelect = result[0] as SubContractProject;
                subConId = subSelect.Id;
                txtTaskHandle.Text = subSelect.BearerOrgName;
            }
        }

        /// <summary>
        /// 保存工程
        /// </summary>
        /// <returns></returns>
        void SaveView()
        {
            try
            {
                if (!ViewToModel()) return;
                taskConfirmDetail = model.ProductionManagementSrv.SaveGWBSTaskConfirm(taskConfirmDetail);
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "保存成功！";
                vmb.ShowDialog();
                index++;
                ShowData();
            }
            catch (Exception e)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "数据保存错误：" + ExceptionUtil.ExceptionMessage(e);
                vmb.ShowDialog();
            }
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        void SubmitView()
        {
            curBillMaster = taskConfirmDetail.Master;
            var query = from cf in curBillMaster.Details.OfType<GWBSTaskConfirm>()
                        where StaticMethod.GetCheckStatePassStr(cf.DailyCheckState) == "未通过"
                        select cf;

            if (query.Count() > 0)//下属明细检查状态存在未通过的
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "下属确认单明细未全部通过检查，该单据不能提交审批！";
                vmb.ShowDialog();
                return;
            }

            VMessageYesOrNo vmyn = new VMessageYesOrNo();
            vmyn.txtInformation.Text = "确定要提交当前单据吗？";
            vmyn.ShowDialog();

            //if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            if (vmyn.yesOrNo)
            {
                try
                {
                    //if (!ViewToModel()) return ;
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Master.Id", curBillMaster.Id));
                    oq.AddFetchMode("WeekScheduleDetailGUID", NHibernate.FetchMode.Eager);
                    IList submitTaskConfirmList = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSTaskConfirm), oq);

                    foreach (GWBSTaskConfirm detail in submitTaskConfirmList)
                    {
                        curBillMaster.AddDetail(detail);
                    }

                    curBillMaster.DocState = DocumentState.InAudit;
                    curBillMaster.ConfirmDate = DateTime.Now;
                    curBillMaster = model.ProductionManagementSrv.SaveGWBSTaskConfirmMaster(curBillMaster);

                    VMessageBox vmb = new VMessageBox();
                    vmb.txtInformation.Text = "成功提交！";
                    vmb.ShowDialog();
                    index++;
                    ShowData();
                }
                catch (Exception e)
                {
                    VMessageBox vmb = new VMessageBox();
                    vmb.txtInformation.Text = "数据保存错误：" + ExceptionUtil.ExceptionMessage(e);
                    vmb.ShowDialog();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        void DeleteView()
        {
            try
            {
                VMessageYesOrNo vmyn = new VMessageYesOrNo();
                vmyn.txtInformation.Text = "确认要删除吗？";
                vmyn.ShowDialog();
                if (vmyn.yesOrNo)
                {
                    //model.ProductionManagementSrv.DeleteByDao(taskConfirmDetail);
                    model.ProductionManagementSrv.DeleteGWBSTaskConfirm(taskConfirmDetail);
                    taskConfirmDetailList.Remove(taskConfirmDetail);
                    VMessageBox vmb = new VMessageBox();
                    vmb.txtInformation.Text = "删除成功！";
                    vmb.ShowDialog();
                    ShowData();
                }
            }
            catch (Exception e)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "数据删除错误：" + ExceptionUtil.ExceptionMessage(e);
                vmb.ShowDialog();
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        void AddView()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBS.Id", taskConfirmDetail.GWBSTree.Id));
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
            IList detailList = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSDetail), oq);
            if (detailList == null || detailList.Count == 0)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "未找到所属工程项目任务下的明细！";
                vmb.ShowDialog();
                return;
            }
            IList addGWBSDetailList = new ArrayList();
            bool flag = false;
            foreach (GWBSDetail dtl in detailList)
            {
                foreach (GWBSTaskConfirm con in taskConfirmDetailList)
                {
                    if (dtl.Id == con.GWBSDetail.Id)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    addGWBSDetailList.Add(dtl);
            }
            if (addGWBSDetailList != null && addGWBSDetailList.Count > 0)
            {
                try
                {
                    //VAddGWBSDetailList vadl = new VAddGWBSDetailList(addGWBSDetailList);
                    //vadl.ShowDialog();
                    //GWBSDetail addGWBSDetail = vadl.ResultGWBSDetail;

                    GWBSDetail newGWBSDetail = addGWBSDetailList[0] as GWBSDetail;

                    if (newGWBSDetail != null)
                    {
                        GWBSTaskConfirm addConfirm = new GWBSTaskConfirm();
                        addConfirm.Master = taskConfirmDetail.Master;

                        addConfirm.GWBSTree = newGWBSDetail.TheGWBS;
                        addConfirm.GWBSTreeName = newGWBSDetail.TheGWBS.Name;
                        addConfirm.GwbsSysCode = newGWBSDetail.TheGWBS.SysCode;

                        addConfirm.GWBSDetail = newGWBSDetail;
                        addConfirm.GWBSDetailName = newGWBSDetail.Name;

                        addConfirm.CostItem = newGWBSDetail.TheCostItem;
                        addConfirm.CostItemName = newGWBSDetail.TheCostItem.Name;

                        addConfirm.CreatePerson = taskConfirmDetail.CreatePerson;
                        addConfirm.CreatePersonName = taskConfirmDetail.CreatePersonName;

                        addConfirm.WeekScheduleDetailGUID = taskConfirmDetail.WeekScheduleDetailGUID;
                        //addConfirm.TaskHandler = taskConfirmDetail.WeekScheduleDetailGUID.SubContractProject;

                        if (taskConfirmDetail.WeekScheduleDetailGUID != null)
                        {
                            if (!string.IsNullOrEmpty(taskConfirmDetail.WeekScheduleDetailGUID.TaskCheckState))
                                addConfirm.DailyCheckState = "2" + taskConfirmDetail.WeekScheduleDetailGUID.TaskCheckState.Substring(1);


                            if (taskConfirmDetail.WeekScheduleDetailGUID.SubContractProject != null)
                            {
                                addConfirm.TaskHandler = model.ProductionManagementSrv.GetObjectById(typeof(SubContractProject), taskConfirmDetail.WeekScheduleDetailGUID.SubContractProject.Id) as SubContractProject;
                                if (addConfirm.TaskHandler != null)
                                    addConfirm.TaskHandlerName = addConfirm.TaskHandler.BearerOrgName;
                            }
                        }


                        //addConfirm.Descript
                        addConfirm.WorkAmountUnitId = newGWBSDetail.WorkAmountUnitGUID;
                        addConfirm.WorkAmountUnitName = newGWBSDetail.WorkAmountUnitName;

                        addConfirm.ProjectTaskType = newGWBSDetail.ProjectTaskTypeCode;

                        addConfirm.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                        addConfirm.PlannedQuantity = newGWBSDetail.PlanWorkAmount;
                        addConfirm.QuantityBeforeConfirm = newGWBSDetail.QuantityConfirmed;
                        //addConfirm.DailyCheckState = "2" + taskConfirmDetail.WeekScheduleDetailGUID.TaskCheckState.Substring(1);
                        addConfirm.ActualCompletedQuantity = 0;
                        addConfirm.ProgressBeforeConfirm = newGWBSDetail.ProgressConfirmed;
                        addConfirm.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.不结算;

                        IList addConfirmList = new ArrayList();
                        addConfirmList.Add(addConfirm);
                        IList resultAddConfirmList = new ArrayList();
                        resultAddConfirmList = model.ProductionManagementSrv.SaveOrUpdate(addConfirmList);
                        GWBSTaskConfirm resultAddConfirm = resultAddConfirmList[0] as GWBSTaskConfirm;
                        taskConfirmDetailList.Add(resultAddConfirm);
                        index = taskConfirmDetailList.Count - 1;
                        ShowData();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            else
            {
                //MessageBox.Show("所属项目任务下面的明细都已有确认明细！");
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "所属项目任务下面的明细都已有确认明细！";
                vmb.ShowDialog();
            }
        }

        private bool ViewToModel()
        {
            if (!Vaild()) return false;
            if (!ProjectAmountVerify()) return false;
            //if (!ScheduleVerify()) return false;
            try
            {
                if (rbNo.Checked == true)
                {
                    taskConfirmDetail.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.不结算;
                }
                else
                {
                    taskConfirmDetail.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.结算;
                }
                taskConfirmDetail.ProgressAfterConfirm = ClientUtil.ToDecimal(txtProgressAfterConfirm.Text);
                taskConfirmDetail.QuantiyAfterConfirm = ClientUtil.ToDecimal(txtTheTeamQuantityBeforeConfirm.Text);
                taskConfirmDetail.ActualCompletedQuantity = ClientUtil.ToDecimal(ClientUtil.ToDecimal(txtTheTeamQuantityBeforeConfirm.Text) - taskConfirmDetail.QuantityBeforeConfirm);

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", subConId));
                IList list = model.ContractExcuteSrv.ObjectQuery(typeof(SubContractProject), oq);
                if (list != null && list.Count > 0)
                {
                    SubContractProject sub = list[0] as SubContractProject;
                    taskConfirmDetail.TaskHandler = sub;
                    taskConfirmDetail.TaskHandlerName = sub.BearerOrgName;
                }

                curBillMaster = taskConfirmDetail.Master;

                if (taskConfirmDetail.Id == null)
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    oq1.AddCriterion(Expression.Eq("GWBSDetail.Id", taskConfirmDetail.GWBSDetail.Id));
                    IList list1 = model.ContractExcuteSrv.ObjectQuery(typeof(GWBSTaskConfirm), oq1);
                    if (list1 != null && list1.Count > 0)
                    {
                        bool flag = false;
                        foreach (GWBSTaskConfirm confirm in list1)
                        {
                            if (confirm.TaskHandler.Id == taskConfirmDetail.TaskHandler.Id
                            && confirm.MaterialFeeSettlementFlag == taskConfirmDetail.MaterialFeeSettlementFlag
                            && confirm.CreatePerson.Id == taskConfirmDetail.CreatePerson.Id)
                            {
                                //MessageBox.Show("工长、承担者、料费结算标记相同的提报信息已经存在，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                VMessageBox vmb = new VMessageBox();
                                vmb.txtInformation.Text = "工长、承担者、料费结算标记相同的提报信息已经存在，请检查！";
                                vmb.ShowDialog();
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            return false;
                        }
                    }
                }
                //curBillMaster.AddDetail(taskConfirmDetail);
                return true;
            }
            catch (Exception e)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "数据错误：" + ExceptionUtil.ExceptionMessage(e);
                vmb.ShowDialog();
                return false;
            }
        }

        /// <summary>
        /// 加载工程任务确认明细数据
        /// </summary>
        private void ShowData()
        {
            try
            {
                if (index >= taskConfirmDetailList.Count)
                    index = taskConfirmDetailList.Count - 1;

                if (index == 0)
                    btnback.Enabled = btnFirst.Enabled = false;
                else
                    btnback.Enabled = btnFirst.Enabled = true;

                if (index == taskConfirmDetailList.Count - 1)
                    btnnext.Enabled = btnlast.Enabled = false;
                else
                    btnnext.Enabled = btnlast.Enabled = true;

                lblRecord.Text = "第【" + (index + 1) + "】条";
                lblRecordTotal.Text = "共【" + taskConfirmDetailList.Count + "】条";

                taskConfirmDetail = taskConfirmDetailList[index] as GWBSTaskConfirm;
                txtName.Text = taskConfirmDetail.GWBSTree.Name;//任务名称
                txtGWBSDetail.Text = taskConfirmDetail.GWBSDetail.Name;//工程任务明细
                txtPlanQuan.Text = ClientUtil.ToString(taskConfirmDetail.PlannedQuantity);//计划工程量
                txtUnit.Text = taskConfirmDetail.WorkAmountUnitName;//计量单位
                decimal responseflag = ClientUtil.ToDecimal(taskConfirmDetail.MaterialFeeSettlementFlag);
                if (responseflag == 0)//未核算
                {
                    rbNo.Checked = true;
                }
                if (responseflag == 1)
                {
                    rbYes.Checked = true;
                }
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", taskConfirmDetail.GWBSDetail.Id));
                IList list = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSDetail), oq);
                if (list != null && list.Count > 0)
                {
                    GWBSDetail detail = list[0] as GWBSDetail;
                    txtProgressBeforeConfirm.Text = detail.ProgressConfirmed.ToString();//已确认累计形象进度
                    txtQuantityBeforeConfirm.Text = detail.QuantityConfirmed.ToString();//已确认累计工程量
                }
                //txtProgressBeforeConfirm.Text = ClientUtil.ToString(taskConfirmDetail.GWBSDetail.ProgressConfirmed);//已确认累计形象进度
                //txtQuantityBeforeConfirm.Text = ClientUtil.ToString(taskConfirmDetail.GWBSDetail.QuantityConfirmed);//已确认累计工程量

                //cbTeam.Items.Clear();
                subContractPro = null;
                if (taskConfirmDetail.TaskHandler != null)
                {
                    //taskConfirmDetail.TaskHandler = model.ProductionManagementSrv.GetObjectById(typeof(SubContractProject), taskConfirmDetail.TaskHandler.Id) as SubContractProject;
                    //cbTeam.Items.Add(taskConfirmDetail.TaskHandler.BearerOrgName);
                    //cbTeam.SelectedIndex = 0;
                    //subContractPro = taskConfirmDetail.TaskHandler;
                    //txtTaskHandle.Tag = taskConfirmDetail.TaskHandler;
                    txtTaskHandle.Text = taskConfirmDetail.TaskHandlerName;
                    subConId = taskConfirmDetail.TaskHandler.Id;
                }

                //txtTheTeamQuantityBeforeConfirm.Text = model.GetTheTeamQuantityConfirmed(projectInfo, taskConfirmDetail.GWBSDetail, taskConfirmDetail.TaskHandler).ToString();
                //txtActualCompletedQuantity.Text = taskConfirmDetail.ActualCompletedQuantity.ToString();


                txtTheTeamQuantityBeforeConfirm.Text = taskConfirmDetail.QuantiyAfterConfirm.ToString();
                txtActualCompletedQuantity.Text = "0";
                txtProgressAfterConfirm.Text = (taskConfirmDetail.ProgressBeforeConfirm + taskConfirmDetail.CompletedPercent).ToString();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// 保存前数据验证
        /// </summary>
        private bool Vaild()
        {
            if (txtProgressAfterConfirm.Text == "")
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "本次形象进度不能为空！";
                vmb.ShowDialog();
                return false;
            }
            if (txtActualCompletedQuantity.Text == "")
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "本次工程量不能为空！";
                vmb.ShowDialog();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 形象进度和工程量验证
        /// </summary>
        bool ProjectAmountVerify()
        {
            try
            {
                decimal actualCompletedQuantity = ClientUtil.ToDecimal(txtActualCompletedQuantity.Text);// 本次工程量
                if (actualCompletedQuantity + taskConfirmDetail.QuantityBeforeConfirm > taskConfirmDetail.PlannedQuantity)
                {
                    VMessageBox vmb = new VMessageBox();
                    vmb.txtInformation.Text = "已超过计划总工程量！请输入正确的本次确认工程量！";
                    vmb.ShowDialog();
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "数据错误：" + ExceptionUtil.ExceptionMessage(e);
                vmb.ShowDialog();
                return false;
            }
        }
        bool ScheduleVerify()
        {
            try
            {
                if (ClientUtil.ToDecimal(txtProgressAfterConfirm.Text) < taskConfirmDetail.ProgressBeforeConfirm)
                {
                    VMessageBox vmb = new VMessageBox();
                    vmb.txtInformation.Text = "本次确认累计形象进度应该大于等于已确认形象进度！";
                    vmb.ShowDialog();
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "数据错误：" + ExceptionUtil.ExceptionMessage(e);
                vmb.ShowDialog();
                return false;
            }
        }

        /// <summary>
        /// 本次确认累计形象进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtProgressAfterConfirm_LostFocus(object sender, EventArgs e)
        {
            txtProgressAfterConfirm.LostFocus -= new EventHandler(txtProgressAfterConfirm_LostFocus);

            decimal progressAfterConfirm = ClientUtil.ToDecimal(this.txtProgressAfterConfirm.Text);
            if (taskConfirmDetail.PlannedQuantity > 0)
            {
                //txtTheTeamQuantityBeforeConfirm.Text = ClientUtil.ToString(taskConfirmDetail.PlannedQuantity * progressAfterConfirm / 100);
                txtActualCompletedQuantity.Text = "0";
                taskConfirmDetail.CompletedPercent = progressAfterConfirm - taskConfirmDetail.ProgressBeforeConfirm;
                txtTheTeamQuantityBeforeConfirm.Text = (taskConfirmDetail.PlannedQuantity * taskConfirmDetail.CompletedPercent / 100 + taskConfirmDetail.QuantityBeforeConfirm).ToString(); ;
            }
            txtProgressAfterConfirm.Focus();
            txtProgressAfterConfirm.LostFocus += new EventHandler(txtProgressAfterConfirm_LostFocus);
        }

        /// <summary>
        /// 累计确认工程量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtTheTeamQuantityBeforeConfirm_LostFocus(object sender, EventArgs e)
        {
            txtTheTeamQuantityBeforeConfirm.LostFocus -= new EventHandler(txtTheTeamQuantityBeforeConfirm_LostFocus);

            decimal QuantityAfterConfirm = ClientUtil.ToDecimal(this.txtTheTeamQuantityBeforeConfirm.Text);
            if (taskConfirmDetail.PlannedQuantity > 0)
            {
                txtProgressAfterConfirm.Text = ClientUtil.ToString(QuantityAfterConfirm / taskConfirmDetail.PlannedQuantity * 100);
                txtActualCompletedQuantity.Text = "0";
                taskConfirmDetail.CompletedPercent = (QuantityAfterConfirm - taskConfirmDetail.QuantityBeforeConfirm) / taskConfirmDetail.PlannedQuantity * 100;
            }
            txtTheTeamQuantityBeforeConfirm.Focus();
            txtTheTeamQuantityBeforeConfirm.LostFocus += new EventHandler(txtTheTeamQuantityBeforeConfirm_LostFocus);
        }
        /// <summary>
        /// 输入 本次确认工程量 失去焦点触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtActualCompletedQuantity_LostFocus(object sender, EventArgs e)
        {
            txtActualCompletedQuantity.LostFocus -= new EventHandler(txtActualCompletedQuantity_LostFocus);
            //验证本次工程量+累计工程量不能大于计划工程量
            decimal actualCompletedQuantity = ClientUtil.ToDecimal(txtActualCompletedQuantity.Text);// 本次工程量
            //if (!ProjectAmountVerify())
            //{
            //    txtActualCompletedQuantity.Focus();
            //    txtActualCompletedQuantity.LostFocus += new EventHandler(txtActualCompletedQuantity_LostFocus);
            //    return;
            //}
            if (taskConfirmDetail.PlannedQuantity > 0)
            {
                //txtProgressAfterConfirm.Text = ClientUtil.ToString(((actualCompletedQuantity + taskConfirmDetail.ProgressBeforeConfirm) / taskConfirmDetail.PlannedQuantity * 100));
                //taskConfirmDetail.QuantiyAfterConfirm = actualCompletedQuantity + taskConfirmDetail.ProgressBeforeConfirm;
                //txtTheTeamQuantityBeforeConfirm.Text = taskConfirmDetail.QuantiyAfterConfirm.ToString();


                txtTheTeamQuantityBeforeConfirm.Text = ClientUtil.ToString((actualCompletedQuantity + ClientUtil.ToDecimal(txtTheTeamQuantityBeforeConfirm.Text)));
                txtProgressAfterConfirm.Text = ClientUtil.ToString(ClientUtil.ToDecimal(txtTheTeamQuantityBeforeConfirm.Text) / taskConfirmDetail.PlannedQuantity * 100);
                taskConfirmDetail.CompletedPercent = (ClientUtil.ToDecimal(txtTheTeamQuantityBeforeConfirm.Text) - taskConfirmDetail.QuantityBeforeConfirm) / taskConfirmDetail.PlannedQuantity * 100;

            }
            txtActualCompletedQuantity.Focus();
            txtActualCompletedQuantity.LostFocus += new EventHandler(txtActualCompletedQuantity_LostFocus);
        }

        //清空数据
        void ClearView()
        {
            txtName.Text = "";
            txtGWBSDetail.Text = "";
            txtPlanQuan.Text = "";
            txtProgressAfterConfirm.Text = "";
            txtProgressBeforeConfirm.Text = "";
            txtActualCompletedQuantity.Text = "";
            txtQuantityBeforeConfirm.Text = "";
            txtUnit.Text = "";
            txtTaskHandle.Text = "";
            //cbTeam.Items.Clear();
        }

        private void VGwbsMng_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }


        public void NewGWBSTaskConfirm()
        {
            GWBSDetail detail = taskConfirmDetail.GWBSDetail;

            GWBSTaskConfirm confirm = new GWBSTaskConfirm();

            confirm.Master = taskConfirmDetail.Master;
            confirm.GWBSDetail = detail;
            confirm.GWBSDetailName = detail.Name;
            confirm.GWBSTree = detail.TheGWBS;
            confirm.GWBSTreeName = detail.TheGWBS.Name;
            confirm.GwbsSysCode = detail.TheGWBS.SysCode;

            confirm.WorkAmountUnitId = detail.PriceUnitGUID;
            confirm.WorkAmountUnitName = detail.PriceUnitName;

            confirm.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
            rbNo.Checked = true;

            confirm.CreatePerson = ConstObject.LoginPersonInfo;
            confirm.CreatePersonName = ConstObject.LoginPersonInfo.Name;

            confirm.PlannedQuantity = detail.PlanWorkAmount;
            confirm.RealOperationDate = DateTime.Now;

            confirm.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
            confirm.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
            confirm.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号

            ShowNewConfirm(confirm);
        }

        public void ShowNewConfirm(GWBSTaskConfirm confrim)
        {
            lblRecord.Text = "第【" + (taskConfirmDetailList.Count + 1) + "】条";
            lblRecordTotal.Text = "共【" + (taskConfirmDetailList.Count + 1) + "】条";

            txtName.Text = confrim.GWBSTree.FullPath;//任务名称
            txtGWBSDetail.Text = confrim.GWBSDetail.Name;//工程任务明细
            txtPlanQuan.Text = ClientUtil.ToString(confrim.PlannedQuantity);//计划工程量
            txtUnit.Text = confrim.WorkAmountUnitName;//计量单位
            decimal responseflag = ClientUtil.ToDecimal(confrim.MaterialFeeSettlementFlag);
            if (responseflag == 0)//未核算
            {
                rbNo.Checked = true;
            }
            if (responseflag == 1)
            {
                rbYes.Checked = true;
            }
            txtProgressBeforeConfirm.Text = ClientUtil.ToString(confrim.GWBSDetail.ProgressConfirmed);//已确认累计形象进度
            txtQuantityBeforeConfirm.Text = ClientUtil.ToString(confrim.GWBSDetail.QuantityConfirmed);//已确认累计工程量

            txtTaskHandle.Text = confrim.TaskHandlerName;
            txtTheTeamQuantityBeforeConfirm.Text = "";
            txtActualCompletedQuantity.Text = "";
            txtProgressAfterConfirm.Text = "";

            taskConfirmDetail = confrim;
        }
    }
}
