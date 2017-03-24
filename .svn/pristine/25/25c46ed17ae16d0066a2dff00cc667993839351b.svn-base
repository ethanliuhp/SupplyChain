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
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Media.Media3D;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement
{
    public partial class VPersonManage : TMasterDetailView
    {
        private MPersonManage model = new MPersonManage();
        private PersonManage curBillMaster;
        CurrentProjectInfo projectInfo;
        WeatherInfo weaInfo = new WeatherInfo();
        /// <summary>
        /// 当前单据
        /// </summary>
        public PersonManage CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VPersonManage()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        public void InitEvent()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.txtPostType.SelectedIndexChanged +=new EventHandler(txtPostType_SelectedIndexChanged);
            this.txtDate.CloseUp += new EventHandler(txtDate_CloseUp);
        }

        private void txtDate_CloseUp(object sender, EventArgs e)
        {
            //查找对应的天气
            DateTime dtDdate = Convert.ToDateTime(txtDate.Value.Date);
            if (!SearchWeather(dtDdate))
            {
                txtWeather.Text = "";
                txtWind.Text = "";
                txtTemperature.Text = "";
                txtHumidity.Text = "";
                txtPart.Text = "";
                txtMainWork.Text = "";
                txtActivity.Text = "";
                txtProblem.Text = "";
                txtManage.Text = "";
                txtDate.Value = dtDdate;
                //RefreshControls(MainViewState.Browser);
            }
            if (txtPostType.Text != "")
            {
                txtPostType_SelectedIndexChanged(sender,e);
            }
        }

        private bool SearchWeather(DateTime strDate)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate", strDate));
            objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));//查询提交的信息
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.PersonManageSrv.GetWeather(objectQuery);
            if (list != null && list.Count > 0)
            {
                weaInfo = list[0] as WeatherInfo;
                txtHumidity.Text = weaInfo.RelativeHumidity;
                txtTemperature.Text = weaInfo.Temperature;
                txtWeather.Text = weaInfo.WeatherCondition;
                txtWind.Text = weaInfo.WindDirection;
                txtWeather.Tag = weaInfo;
                return true;
            }
            else
            {
                MessageBox.Show("没有晴雨表信息！");
                if (weaInfo.Id != null)
                {
                    txtDate.Value = weaInfo.CreateDate;
                    return false;
                }
                //SearchWeather(txtDate.Value);
            }
            return true;
        }

        public void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            VBasicDataOptr.InitPostType(txtPostType, false);
        }
        //岗位类型下拉框信息更改
        private void txtPostType_SelectedIndexChanged(object sender,EventArgs e)
        {
            txtProblem.Enabled = true;
            txtMainWork.Enabled = true;
            txtActivity.Enabled = true;
            txtManage.Enabled = true;
            if (txtPostType.SelectedItem.Equals("技术员"))
            {
                customLabel10.Visible = true;
                txtManage.Visible = true;
            }
            else
            {
                customLabel10.Visible = false;
                txtManage.Visible = false; 
            }
            txtMainWork.Text = "";

            if (txtPostType.Text == "施工员")
            {
                //通过人员读取当天周计划信息
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Master.CreateDate", this.txtDate.Value.Date));
                oq.AddCriterion(Expression.Eq("Master.CreatePersonName", this.txtHandlePerson.Text));
                oq.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
                IList list = model.GetWeekDetail(oq);
                Hashtable ht = new Hashtable();
                if (list.Count > 0)
                {
                    foreach (WeekScheduleDetail weekdetail in list)
                    {
                        string strName = weekdetail.SupplierName + "、" + StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode) + ";" + ClientUtil.ToString(weekdetail.TaskCompletedPercent);
                        if (ht.Count == 0)
                        {
                            if (weekdetail.SupplierName != null)
                            {
                                ht.Add(weekdetail.SupplierName, strName);
                            }
                        }
                        else
                        {
                            if (ht.Contains(weekdetail.SupplierName))
                            {
                                string strMain = ht[weekdetail.SupplierName] as string;
                                ht.Remove(weekdetail.SupplierName);
                                strMain += ";" + strName;
                                ht.Add(weekdetail.SupplierName, strMain);
                            }
                        }
                    }
                }
                foreach (System.Collections.DictionaryEntry objPriceUnit in ht)
                {
                    //将不同承担队伍的换行分开
                    txtMainWork.Text += objPriceUnit.Value.ToString() + "\r\n";
                }
            }
            if (txtPostType.Text == "材料员")
            {
                //查询收料入库，调拨入库出库信息  物资名称 型号 数量 单位
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("CreateDate", this.txtDate.Value.Date));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

                IList listStockIn = model.GetStockIn(oq);
                if (listStockIn.Count > 0)
                {
                    foreach (StockIn stockIn in listStockIn)
                    {
                        foreach (StockInDtl dtl in stockIn.Details)
                        {
                            txtMainWork.Text += ClientUtil.ToString(dtl.MaterialName) + "、" + ClientUtil.ToString(dtl.MaterialSpec) + "、" + ClientUtil.ToString(dtl.Quantity) + "、" + ClientUtil.ToString(dtl.MatStandardUnitName) + "\r\n";
                        }
                    }
                }
                IList listStockMove = model.GetStockMoveIn(oq);
                if (listStockMove.Count > 0)
                {
                    foreach (StockMoveIn moveIn in listStockMove)
                    {
                        foreach (StockMoveInDtl moveInDtl in moveIn.Details)
                        {
                            txtMainWork.Text += ClientUtil.ToString(moveInDtl.MaterialName) + "、" + ClientUtil.ToString(moveInDtl.MaterialSpec) + "、" + ClientUtil.ToString(moveInDtl.Quantity) + "、" + ClientUtil.ToString(moveInDtl.MatStandardUnitName) + "\r\n";
                        }
                    }
                }
                IList listStockMoveOut = model.GetStockMoveOut(oq);
                if (listStockMoveOut.Count > 0)
                {
                    foreach (StockMoveOut moveOut in listStockMoveOut)
                    {
                        foreach (StockMoveOutDtl moveOutDtl in moveOut.Details)
                        {
                            txtMainWork.Text += ClientUtil.ToString(moveOutDtl.MaterialName) + "、" + ClientUtil.ToString(moveOutDtl.MaterialSpec) + "、" + ClientUtil.ToString(moveOutDtl.Quantity) + "、" + ClientUtil.ToString(moveOutDtl.MatStandardUnitName) + "\r\n";
                        }
                    }
                }
            }
            if (txtPostType.Text == "质检员")
            {
                //日常检查记录和专业检查记录
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("CreateDate", this.txtDate.Value.Date));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                //通过质量验收的
                oq.AddCriterion(Expression.Eq("InspectionConclusion", "不通过"));
                //形象进度为100%
                oq.AddCriterion(Expression.Eq("GWBSTree.AddUpFigureProgress",ClientUtil.ToDecimal(100)));
                //日常检查记录
                IList listRecord = model.GetInspectionRecord(oq);
                if (listRecord.Count > 0)
                {
                    foreach (InspectionRecord rcd in listRecord)
                    {
                        txtMainWork.Text += ClientUtil.ToString(rcd.InspectionContent) + ";";
                    }
                }

                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("CreateDate", this.txtDate.Value.Date));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                //通过质量验收的
                //oq.AddCriterion(Expression.Eq("Details.InspectionConclusion", 1));
                //专业检查记录
                IList listPro = model.GetProfessionInspectionRecordPlan(oq);
                if (listPro.Count > 0)
                {
                    foreach (ProfessionInspectionRecordMaster master in listPro)
                    {
                        foreach (ProfessionInspectionRecordDetail dtl in master.Details)
                        {
                            txtMainWork.Text += ClientUtil.ToString(dtl.InspectionContent);
                        }
                    }
                }
            }
            if (txtPostType.Text == "安全员")
            {
                //日常检查记录和专业检查记录
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("CreateDate", this.txtDate.Value.Date));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                IList listRecord = model.GetInspectionRecord(oq);
                if (listRecord.Count > 0)
                {
                    foreach (InspectionRecord rcd in listRecord)
                    {
                        if (rcd.InspectionContent != null)
                        {
                            txtMainWork.Text += ClientUtil.ToString(rcd.InspectionContent) + ";";
                        }
                    }
                }
                IList listPro = model.GetProfessionInspectionRecordPlan(oq);
                if (listPro.Count > 0)
                {
                    foreach (ProfessionInspectionRecordMaster master in listPro)
                    {
                        foreach (ProfessionInspectionRecordDetail dtl in master.Details)
                        {
                            txtMainWork.Text += ClientUtil.ToString(dtl.InspectionContent);
                        }
                    }
                }
            }
            if (!SearchPersonManage(ClientUtil.ToDateTime(txtDate.Value.ToShortDateString())))
            {
                txtProblem.Enabled = false;
                txtMainWork.Enabled = false;
                txtActivity.Enabled = false;
                txtManage.Enabled = false;
                txtPart.Enabled = false;
            }
            else
            {
                txtProblem.Enabled = true;
                txtMainWork.Enabled = true;
                txtActivity.Enabled = true;
                txtManage.Enabled = true;
                txtPart.Enabled = true;
            }
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string Id)
        {
            try
            {
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.PersonManageSrv.GetPersonManageById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtHandlePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    txtDate.Enabled = true;
                    txtPostType.Enabled = true;
                    txtPart.Enabled = true;
                    cmsDg.Enabled = true;
                    //ObjectLock.Unlock(txtDate);
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    txtDate.Enabled = false;
                    txtHumidity.Enabled = false;
                    txtTemperature.Enabled = false;
                    txtWeather.Enabled = false;
                    txtWind.Enabled = false;
                    txtPostType.Enabled = false;
                    txtProblem.Enabled = false;
                    txtMainWork.Enabled = false;
                    txtActivity.Enabled = false;
                    txtManage.Enabled = false;
                    txtPart.Enabled = false;
                    //ObjectLock.Unlock(txtDate);
                    cmsDg.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                txtDate.Enabled = true;
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                txtDate.Enabled = false;
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtHandlePerson, txtProject, txtWeather, txtWind, txtTemperature, txtHumidity };
            ObjectLock.Lock(os);
            object[] os1 = new object[] { txtDate };
            ObjectLock.Unlock(os1);
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
            this.txtMainWork.Text = "";
            this.txtActivity.Text = "";
            this.txtProblem.Text = "";
            this.txtManage.Text = "";
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                this.curBillMaster = new PersonManage();
                txtDate.Value = DateTime.Now;
                DateTime strDate = Convert.ToDateTime(txtDate.Value.Date);
                //SearchPersonManage(strDate);
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;//状态
                txtPostType.Text = "";
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                }
                NewManage();
                SearchWeather(strDate);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        void NewManage()
        {
            curBillMaster = new PersonManage();
            curBillMaster.CreateDate = ConstObject.LoginDate;
            curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
            curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
            curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
            curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
            curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.ProjectId = projectInfo.Id;
            curBillMaster.ProjectName = projectInfo.Name;
        }

        bool SearchPersonManage(DateTime strDate)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate", strDate));
            objectQuery.AddCriterion(Expression.Eq("Post", txtPostType.Text));
            objectQuery.AddCriterion(Expression.Eq("HandlePerson", ConstObject.LoginPersonInfo));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.PersonManageSrv.GetPersonManage(objectQuery);
            if (list != null && list.Count > 0)
            {
                PersonManage weather = list[0] as PersonManage;
                curBillMaster = weather;
                if (weather.WeatherGlass != null)
                {
                    this.txtHumidity.Text = weather.WeatherGlass.RelativeHumidity;
                    this.txtTemperature.Text = weather.WeatherGlass.Temperature;
                    this.txtWeather.Text = weather.WeatherGlass.WeatherCondition;
                    this.txtWind.Text = weather.WeatherGlass.WindDirection;
                }
                this.txtPostType.Text = ClientUtil.ToString(weather.Post);
                this.txtMainWork.Text = ClientUtil.ToString(weather.MainWork);
                this.txtActivity.Text = ClientUtil.ToString(weather.OtherActivities);
                this.txtProblem.Text = ClientUtil.ToString(weather.Problem);
                this.txtPart.Text = ClientUtil.ToString(weather.ConstructSite);
                this.txtManage.Text = ClientUtil.ToString(weather.ProjectManage);
                if (weather.DocState == DocumentState.InAudit || weather.DocState == DocumentState.InExecute)
                {
                    //MessageBox.Show(strDate.ToShortDateString() + "管理人员日志已经存在并且已经提交！");
                    return false;
                }
            }
            else
            {
                //存在问题
                //日常检查记录和专业检查记录
                NewManage();
                ObjectQuery objecctQuery = new ObjectQuery();
                objecctQuery.AddCriterion(Expression.Eq("CreateDate", this.txtDate.Value.Date));
                objecctQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                IList lstRecord = model.GetInspectionRecord(objecctQuery);
                if (lstRecord.Count > 0)
                {
                    foreach (InspectionRecord rcd in lstRecord)
                    {
                        if (rcd.GWBSTree != null)
                        {
                            txtProblem.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), rcd.GWBSTreeName, (rcd.GWBSTree as GWBSTree).SysCode) + ";" + ClientUtil.ToString(rcd.InspectionContent) + "\r\n";
                        }
                    }
                }
                txtActivity.Text = "";
                txtMainWork.Text = "";
                txtManage.Text = "";
                txtProblem.Text = "";
            }
            return true;           
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.PersonManageSrv.GetPersonManageById(curBillMaster.Id);
                this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString().Replace("-", "");
                ModelToView();
                txtProblem.Enabled = true;
                txtMainWork.Enabled = true;
                txtActivity.Enabled = true;
                txtManage.Enabled = true;
                return true;
            }
            string message = "此单状态为非编辑状态，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                if (!ViewToModel()) return false;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster = model.PersonManageSrv.SavePersonManage(curBillMaster);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                if (curBillMaster.DocState == DocumentState.InAudit || curBillMaster.DocState == DocumentState.InExecute)
                {
                    MessageBox.Show("信息已经提交不能再次保存！");
                    return true;
                }
                curBillMaster = model.PersonManageSrv.SavePersonManage(curBillMaster);
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "管理人员日志";
                log.Code = curBillMaster.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                if (flag)
                {
                    log.OperType = "新增";
                }
                else
                {
                    log.OperType = "修改";
                }
                StaticMethod.InsertLogData(log);
                this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString().Replace("-", "");
                MessageBox.Show("保存成功！");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.PersonManageSrv.GetPersonManageById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.PersonManageSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "检验批";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
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
                        curBillMaster = model.PersonManageSrv.GetPersonManageById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
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
                curBillMaster = model.PersonManageSrv.GetPersonManageById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string validMessage = "";
           
            if (txtMainWork.Text.Equals(""))
            {
                MessageBox.Show("主要工作内容不能为空！");
                return false;
            }
            if (txtPostType.SelectedItem == null)
            {
                MessageBox.Show("岗位类型不能为空！");
                return false;
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.CreateDate = txtDate.Value.Date;
                if (txtWeather.Text != "" && txtWeather.Text != null)
                {
                    curBillMaster.WeatherGlass = txtWeather.Tag as WeatherInfo;
                }
                curBillMaster.Problem = ClientUtil.ToString(txtProblem.Text);
                curBillMaster.ProjectManage = ClientUtil.ToString(txtManage.Text);
                curBillMaster.Post = ClientUtil.ToString(txtPostType.SelectedItem);
                curBillMaster.OtherActivities = ClientUtil.ToString(txtActivity.Text);
                curBillMaster.MainWork = ClientUtil.ToString(txtMainWork.Text);
                curBillMaster.ConstructSite = ClientUtil.ToString(txtPart.Text);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtPart.Text = curBillMaster.ConstructSite;
                this.txtPostType.SelectedItem = curBillMaster.Post;
                this.txtProblem.Text = curBillMaster.Problem;
                this.txtProject.Text = curBillMaster.ProjectName;
                this.txtProject.Tag = curBillMaster.ProjectId;
                if (curBillMaster.WeatherGlass != null)
                {
                    this.txtTemperature.Text = curBillMaster.WeatherGlass.Temperature;
                    this.txtWeather.Text = curBillMaster.WeatherGlass.WeatherCondition;
                    this.txtWeather.Tag = curBillMaster.WeatherGlass;
                    this.txtWind.Text = curBillMaster.WeatherGlass.WindDirection;
                    this.txtHumidity.Text = curBillMaster.WeatherGlass.RelativeHumidity;
                }
                this.txtActivity.Text = curBillMaster.OtherActivities;
                this.txtDate.Value = curBillMaster.CreateDate;
                this.txtHandlePerson.Text = curBillMaster.HandlePersonName;
                this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                
                this.txtMainWork.Text = curBillMaster.MainWork;
                this.txtManage.Text = curBillMaster.ProjectManage;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"项目管理人员工作日志.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"项目管理人员工作日志.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"项目管理人员工作日志.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("项目管理人员工作日志【" + curBillMaster + "】", false, false, true);
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

        private void FillFlex(PersonManage billMaster)
        {
            int detailStartRowNumber = 7;//7为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            //flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            
            flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(4, 2).Text = billMaster.WeatherGlass.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 5).Text = billMaster.WeatherGlass.WeatherCondition;
            flexGrid1.Cell(4, 8).Text = billMaster.Post;

            flexGrid1.Cell(5, 2).Text = billMaster.WeatherGlass.RelativeHumidity;
            flexGrid1.Cell(5, 5).Text = billMaster.WeatherGlass.Temperature;
            flexGrid1.Cell(5, 8).Text = billMaster.WeatherGlass.WindDirection;


            flexGrid1.Cell(6, 2).Text = billMaster.MainWork;
            flexGrid1.Cell(6, 2).WrapText = true;
            flexGrid1.Row(6).AutoFit();
            flexGrid1.Cell(7, 2).Text = billMaster.ProjectManage;
            flexGrid1.Cell(7, 2).WrapText = true;
            flexGrid1.Row(7).AutoFit();
            flexGrid1.Cell(8, 2).Text = billMaster.OtherActivities;
            flexGrid1.Cell(8, 2).WrapText = true;
            flexGrid1.Row(8).AutoFit();
            flexGrid1.Cell(9, 2).Text = billMaster.Problem;
            flexGrid1.Cell(9, 2).WrapText = true;
            flexGrid1.Row(9).AutoFit();
        }
    }
}
