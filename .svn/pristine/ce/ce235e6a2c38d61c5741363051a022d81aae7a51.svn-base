using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;

using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using FlexCell;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekAssign : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        MStockMng stockModel = new MStockMng(); 
        CurrentProjectInfo projectInfo = null;
        private AssignWorkerOrderMaster curBillMaster;
        private DateTime defaultTime = new DateTime(1900, 1, 1);

        ///<summary>
        /// 当前单据
        ///</summary>
        public  AssignWorkerOrderMaster CurBillMaster
        {
            set { this.curBillMaster = value; }
            get { return this.curBillMaster; }
        }


        private void LockControls()
        {
            this.txtPlanName.ReadOnly = true;
            this.txtCreatePerson.ReadOnly = true;
            this.dtpCreatDate.Enabled = false;
            this.colPlanBeginDate.ReadOnly = true;
            this.colPlanEndDate.ReadOnly = true;
            this.cmbMsgState.Enabled = false;
            this.comDocState.Enabled = false;
          
        }

        private void LockControls(bool isLock)
        {
            this.btnSendMsg.Enabled = !isLock;
            this.dtpSetDateTime.Enabled = !isLock;
            this.btnSetSelRowsDate.Enabled = !isLock;
        }

        

        public VWeekAssign()
        {
            InitializeComponent();
            projectInfo = StaticMethod.GetProjectInfo();
            InitEvent();
            InitControlDataSource();

            LockControls();


        }

        private void InitControlDataSource()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.ProductionManagementSrv.GetAssignWorkerOrderMasterByOQ(oq,false);
            var masters =  from f in list.OfType<AssignWorkerOrderMaster>()
                           select f;
            var sources = masters.GroupBy(p => p.WeekSchedule).Select(p => new { WeekSchedule = p.Key, WeekScheduleName = p.FirstOrDefault().WeekScheduleName }).ToList();

            this.cboSchedulePlanName.SelectedIndexChanged -= new EventHandler(cboSchedulePlanName_SelectedIndexChanged);     
            this.cboSchedulePlanName.DataSource = sources;
            this.cboSchedulePlanName.DisplayMember = "WeekScheduleName";
            this.cboSchedulePlanName.ValueMember = "WeekSchedule";
            this.cboSchedulePlanName.SelectedIndexChanged += new EventHandler(cboSchedulePlanName_SelectedIndexChanged);

            if (sources != null && sources.Count > 0)
            {
                this.cboSchedulePlanName.SelectedValue = sources[0].WeekSchedule;
                cboSchedulePlanName_SelectedIndexChanged(null, null);
            }

            #region 状态
            if (this.comDocState.Items.Count == 0)
            {
                Array tem = Enum.GetValues(typeof(DocumentState));
                for (int i = 0; i < tem.Length; i++)
                {
                    DocumentState s = (DocumentState)tem.GetValue(i);
                    int k = (int)s;
                    if (k != 0)
                    {
                        string strNewName = ClientUtil.GetDocStateName(k);
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                        li.Text = strNewName;
                        li.Value = k.ToString();
                        comDocState.Items.Add(li);
                    }
                }
            }

            if (this.cmbMsgState.Items.Count == 0)
            {
                foreach (string  item in Enum.GetNames(typeof(SendMessageState)))
                {
                    cmbMsgState.Items.Add(item);
                }
            }
            #endregion


        }
        
    
        /// <summary>
        /// 注册事件
        /// </summary>
        private void InitEvent()
        {
            this.cboSchedulePlanName.SelectedIndexChanged += new EventHandler(cboSchedulePlanName_SelectedIndexChanged);
            this.cmbTeam.SelectedIndexChanged += new EventHandler(cmbTeam_SelectedIndexChanged);
            this.btnSetSelRowsDate.Click += new EventHandler(btnSetSelRowsDate_Click);
            this.btnSendMsg.Click += new EventHandler(btnSendMsg_Click);

        }

        void btnSendMsg_Click(object sender, EventArgs e)
        {
            if (curBillMaster == null)
                return;
            if (curBillMaster.AssignTeam == null)
            {
                MessageBox.Show("派工队伍不能为空！");
                return;
            }
            if (curBillMaster.DocState != DocumentState.InExecute)
            {
                bool IsSubmit = SubmitView();
                if (!IsSubmit)
                    return;
            }

            try
            {
                 string  sReceiver  = model.ProductionManagementSrv.GetAssignTeamLinkMan(curBillMaster.AssignTeam);
                 if (sReceiver == "")
                 {
                     MessageBox.Show("派工队伍没有维护联系人！");
                     return;
                 }
                
                SendMessageUtil sUtil = new SendMessageUtil(); ;
                string sSender = "系统管理员";
        
                string sContent = string.Format(@"http://localhost:55555/www/index.html#/laborordersdetail/{0}", curBillMaster.Id);
                sUtil.SendICUMsg(sSender, sReceiver, sContent, false, true);
                curBillMaster.MsgState = SendMessageState.已通知;
                curBillMaster.MsgDate = DateTime.Now;
                curBillMaster.MsgPerson = ConstObject.LoginPersonInfo;
                curBillMaster.MsgPersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster = model.ProductionManagementSrv.SaveAssignWorkerOrderMaster(curBillMaster);
                RefreshView();
                MessageBox.Show("短信发送成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void btnSetSelRowsDate_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow  item in this.dgDetail.SelectedRows)
            {
                item.Cells[this.colActualBenginDate.Name].Value = dtpSetDateTime.Value;
            }
        }

        void cboSchedulePlanName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strWeekSchedule = ClientUtil.ToString(this.cboSchedulePlanName.SelectedValue);
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("WeekSchedule", strWeekSchedule));
            IList list = model.ProductionManagementSrv.GetAssignWorkerOrderMasterByOQ(oq, false);
             var masters =  from f in list.OfType<AssignWorkerOrderMaster>()
                           select f;

             var sources = masters.GroupBy(p => p.AssignTeam).Select(p => new { AssignTeam = p.Key, AssignTeamName = p.FirstOrDefault().AssignTeamName }).ToList();

             this.cmbTeam.DataSource = sources;
             this.cmbTeam.DisplayMember = "AssignTeamName";
             this.cmbTeam.ValueMember = "AssignTeam";


        }

        void cmbTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strWeekSchedule = ClientUtil.ToString(this.cboSchedulePlanName.SelectedValue);
            string strAssignTeam = ClientUtil.ToString(this.cmbTeam.SelectedValue);
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("WeekSchedule", strWeekSchedule));
            oq.AddCriterion(Expression.Eq("AssignTeam", strAssignTeam));
            IList list = model.ProductionManagementSrv.GetAssignWorkerOrderMasterByOQ(oq, true);
            if (list != null && list.Count > 0)
            {
                curBillMaster = list[0] as AssignWorkerOrderMaster;
                ModelToView();
                this.ViewCaption = ViewName + "-" + curBillMaster.Code;
                RefreshState(MainViewState.Browser);
            }
            else
            {
                ClearView();
                this.ViewCaption = ViewName + "-空";
            }
        }


       
        #region override methods


        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel())
                    return false;

                if (curBillMaster.Id == null)
                {
                    curBillMaster = model.ProductionManagementSrv.SaveAssignWorkerOrderMaster(curBillMaster,true);

                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "任务单", "", curBillMaster.ProjectName);
                }
                else
                {
                    curBillMaster = model.ProductionManagementSrv.SaveAssignWorkerOrderMaster(curBillMaster,true);

                    //插入日志
                    StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "任务单", "", curBillMaster.ProjectName);
                }

                //更新Caption
                this.ViewCaption = ViewName + "-" + curBillMaster.Code;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        public override bool SubmitView()
        {
            if (!ViewToModel())
                return false;

            if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    FlashScreen.Show("正在执行提交，请稍候........");

                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster.SubmitDate = DateTime.Now;
                    LogData log = new LogData();
                    log.BillType = "任务单";
                    if (string.IsNullOrEmpty(curBillMaster.Id))
                        log.OperType = "新增提交";
                    else
                        log.OperType = "修改提交";

                    curBillMaster = model.ProductionManagementSrv.SaveAssignWorkerOrderMaster(curBillMaster,true);

                    log.BillId = curBillMaster.Id;
                    log.Code = curBillMaster.Code;
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    //更新Caption
                    this.ViewCaption = ViewName + "-" + curBillMaster.Code;
                    RefreshView();
                    return true;
                }
                catch (Exception e)
                {
                    FlashScreen.Close();
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
                finally
                {
                    FlashScreen.Close();
                }
            }
            return false;
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        curBillMaster = model.ProductionManagementSrv.GetAssignWorkerOrderMasterById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
 
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }


        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.ProductionManagementSrv.GetAssignWorkerOrderMasterById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
                LockControls();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);
            ObjectLock.Unlock(pnlFloor, true);    
            LockControls();

            ToolMenu.LockItem(ToolMenuItem.AddNew);
            ToolMenu.LockItem(ToolMenuItem.Delete); 
            switch (state)
            {
                case MainViewState.Browser:
                    if (curBillMaster != null && curBillMaster.DocState == DocumentState.InExecute)
                        ToolMenu.LockItem(ToolMenuItem.Modify);
                    LockControls(true);
                    if (curBillMaster != null && curBillMaster.MsgState == SendMessageState.未通知)
                        this.btnSendMsg.Enabled = true;
                    break;
                
                case MainViewState.Modify:
                    LockControls(false);
                    break;
            }
                         
        }

        public override bool ModifyView()
        {
            curBillMaster = model.ProductionManagementSrv.GetAssignWorkerOrderMasterById(curBillMaster.Id);

            base.ModifyView();
            ModelToView();
            return true;

        }
        #endregion

        #region ViewToModel
        private bool ViewToModel()
        {
            if (!ValidView())
                return false;
            try
            {
                ViewToModel_Master();
                ViewToModel_Detail();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        private bool ValidView()
        {
            for (int i = 0; i < this.dgDetail.Rows.Count ; i++)
            {
                DataGridViewRow item = this.dgDetail.Rows[i];
                if (item.Cells[colActualBenginDate.Name].Value == null || item.Cells[colActualBenginDate.Name].Value.ToString() == "")
                {
                    MessageBox.Show(string.Format(@"请指定第{0}行的实际开始时间！", (i+1).ToString()));
                    return false;
                }
                
            }
            return true;
        
            
        }

        private void ViewToModel_Master()
        {
            this.curBillMaster.AssignWorkerOrderDescription = this.txtRemark.Text;
        }
        private void ViewToModel_Detail()
        {
            foreach (DataGridViewRow item in dgDetail.Rows)
            {
                AssignWorkerOrderDetail detail = item.Tag as AssignWorkerOrderDetail;
                detail.ActualBenginDate =  ClientUtil.ToDateTime(item.Cells[colActualBenginDate.Name].Value);
                detail.AssWorkDesc = ClientUtil.ToString(item.Cells[colAssWorkDesc.Name].Value);
                this.curBillMaster.Details.Add(detail);
            }
        }

        #endregion

        #region ModelToView
        private bool ModelToView()
        {
            try
            {
                ClearView();
                ModelToView_Master();
                ModelToView_Detail();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is TextBox)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                ((CustomDataGridView)c).Rows.Clear(); 
            }
        }
        private void ModelToView_Detail()
        {
            foreach (AssignWorkerOrderDetail item in curBillMaster.Details)
            {
                AddAssignWorkerOrderDetailInGrid(item);
            }
        }

        private void ModelToView_Master()
        {
            this.cboSchedulePlanName.Tag = this.curBillMaster.WeekSchedule;
            this.cboSchedulePlanName.Text = this.curBillMaster.WeekScheduleName;
            this.cmbTeam.Tag = this.curBillMaster.AssignTeam;
            this.cmbTeam.Text = this.curBillMaster.AssignTeamName;
            this.txtCreatePerson.Tag = this.curBillMaster.CreatePerson;
            this.txtCreatePerson.Text = this.curBillMaster.CreatePersonName;
            this.dtpCreatDate.Value = this.curBillMaster.CreateDate;
            this.txtRemark.Text = this.curBillMaster.AssignWorkerOrderDescription;
            this.txtPlanName.Text = this.curBillMaster.Code;
            this.comDocState.Text = ClientUtil.GetDocStateName(curBillMaster.DocState);
            this.cmbMsgState.Text = Enum.GetName(typeof(SendMessageState), curBillMaster.MsgState);
        }
        private void AddAssignWorkerOrderDetailInGrid(AssignWorkerOrderDetail item)
        {
            int index = this.dgDetail.Rows.Add();
            DataGridViewRow row = this.dgDetail.Rows[index];
            row.Cells[colGWBSTree.Name].Value = item.GWBSTreeName;
            row.Cells[colGWBSDetail.Name].Value = item.GWBSDetailName;
            row.Cells[colPlanBeginDate.Name].Value = item.PlanBeginDate == defaultTime ? "" :ClientUtil.ToString(item.PlanBeginDate.ToShortDateString());
            row.Cells[colPlanEndDate.Name].Value =  item.PlanEndDate == defaultTime ? "":ClientUtil.ToString(item.PlanEndDate.ToShortDateString());
            row.Cells[colPlanWorkDays.Name].Value = item.PlanWorkDays == 0? "" :ClientUtil.ToString(item.PlanWorkDays);
            row.Cells[colActualBenginDate.Name].Value =  item.ActualBenginDate == defaultTime ? "":ClientUtil.ToString(item.ActualBenginDate.ToShortDateString());
            row.Cells[colAssWorkDesc.Name].Value = item.AssWorkDesc;
            row.Tag = item;

        }
        #endregion

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"任务单.flx") == false) return false;
            if (!UpdatePrintInfo())
                return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }


        public override bool Print()
        {
            if (LoadTempleteFile(@"任务单.flx") == false) return false;
            if (!UpdatePrintInfo())
                return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            //curBillMaster.PrintTimes = curBillMaster.PrintTimes + 1;
            curBillMaster = model.ProductionManagementSrv.SaveAssignWorkerOrderMaster(curBillMaster);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"任务单.flx") == false) return false;
            if (!UpdatePrintInfo())
                return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("任务单【" + curBillMaster.Code + "】", false, false, true);
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(AssignWorkerOrderMaster billMaster)
        {
            int detailStartRowNumber = 5;//5为模板中的行号
            int detailCount = billMaster.Details.Count;
            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount-1);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);

            decimal sumQuantity = 0;
            //条形码
            string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumQuantity));
            this.flexGrid1.Cell(1, 6).Text = billMaster.Code.Substring(billMaster.Code.Length - 12) + "-" + a;
            this.flexGrid1.Cell(1, 6).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 6).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;

            //主表数据
            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 2).WrapText = true;
            flexGrid1.Cell(3, 6).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(3, 6).WrapText = true;

            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                AssignWorkerOrderDetail detail = (AssignWorkerOrderDetail)billMaster.Details.ElementAt(i);
                //任务名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.GWBSTreeName;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //任务明细
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.GWBSDetailName;
                flexGrid1.Cell(detailStartRowNumber + i, 2).WrapText = true;
                //计划开始日期
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.PlanBeginDate.ToShortDateString();
                flexGrid1.Cell(detailStartRowNumber + i, 3).WrapText = true;
                //结束日期
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.PlanEndDate.ToShortDateString();
                flexGrid1.Cell(detailStartRowNumber + i, 4).WrapText = true;
                //工期
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.PlanWorkDays.ToString();
                flexGrid1.Cell(detailStartRowNumber + i, 5).WrapText = true;
                //实际开始日期
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.ActualBenginDate.ToShortDateString();
                flexGrid1.Cell(detailStartRowNumber + i, 6).WrapText = true;
                //任务说明
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.AssWorkDesc;
                flexGrid1.Cell(detailStartRowNumber + i, 7).WrapText = true;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();

                //if (i == detailCount - 1)
                //{
                //    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Text = ClientUtil.ToString(sumQuantity);
                //    flexGrid1.Cell(detailStartRowNumber + i + 1, 5).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //}
            }
            flexGrid1.Cell(detailStartRowNumber + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(detailStartRowNumber + detailCount, 6).Text = billMaster.CreateDate.ToString();
            flexGrid1.Cell(detailStartRowNumber + detailCount + 1, 2).Text = billMaster.AssignTeamName;
            flexGrid1.Cell(detailStartRowNumber + detailCount + 1, 6).Text = billMaster.CreatePersonName;


            this.flexGrid1.FrozenBottomRows = 2;
            //审批信息打印
            int maxRow = detailStartRowNumber + detailCount + 3;
            CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id, maxRow);
        }

        private bool UpdatePrintInfo()
        {
            try
            {
                this.curBillMaster.PrintCount = ClientUtil.ToInt(this.curBillMaster.PrintCount) + 1;
                this.curBillMaster.LastPrintPerson = ConstObject.LoginPersonInfo;
                this.curBillMaster.LastPrintPersonName = ConstObject.LoginPersonInfo.Name;
                this.curBillMaster.LastPrintTime = model.ProductionManagementSrv.GetServerTime();

                curBillMaster = model.ProductionManagementSrv.SaveAssignWorkerOrderMaster(curBillMaster);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("更新打印信息出错：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

      
    }
}
