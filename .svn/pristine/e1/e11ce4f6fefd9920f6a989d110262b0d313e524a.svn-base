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
using Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport
{
    public partial class VConstructionReport : TMasterDetailView
    {
        private MConstructionReport model = new MConstructionReport();
        private ConstructReport curBillMaster;
        CurrentProjectInfo projectInfo;
        WeatherInfo weaInfo = new WeatherInfo();
        /// <summary>
        /// 当前单据
        /// </summary>
        public ConstructReport CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VConstructionReport()
        {
            InitializeComponent();
            InitEvent();
        }
        public void InitEvent()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.txtDate.CloseUp += new EventHandler(txtDate_CloseUp);
            this.butAdd.Click +=new EventHandler(butAdd_Click);
        }
        void butAdd_Click(object sender,EventArgs e)
        {
            butAddManage(txtDate.Value.Date);
        }

        void butAddManage(DateTime dt)
        {
            if (!SearchWeather(dt))
            {
                return;
            }
            IList ResultList = model.ConstructionReportSrv.GetConstructReportList(projectInfo,dt);
            if (ResultList.Count > 0)
            {
                ConstructReport manage = ResultList[0] as ConstructReport;
                if (manage.ConstructSite != null)
                {
                    txtPart.Text = manage.ConstructSite;
                }
                else
                {
                    txtPart.Text = "";
                }
                if (manage.CompletionSchedule != null)
                {
                    txtMainWork.Text = manage.CompletionSchedule;
                }
                else
                {
                    txtMainWork.Text = "";
                }
                if (manage.MaterialCase != null)
                {
                    txtMaterialManage.Text = manage.MaterialCase;
                }
                else
                {
                    txtMaterialManage.Text = "";
                }
                if (manage.OtherActivities != null)
                {
                    txtOtherActivity.Text = manage.OtherActivities;
                }
                else
                {
                    txtOtherActivity.Text = "";
                }
                if (manage.Problem != null)
                {
                    txtProblem.Text = manage.Problem;
                }
                else
                {
                    txtProblem.Text = "";
                }
                if (manage.SafetyControl != null)
                {
                    txtProductManage.Text = manage.SafetyControl;
                }
                else
                {
                    txtProductManage.Text = "";
                }
                if (manage.ProjectId != null)
                {
                    txtProjectManage.Text = manage.ProjectManage;
                }
                else
                {
                    txtProjectManage.Text = "";
                }
            }
            else
            {
                txtMainWork.Text = "";
                txtProjectManage.Text = "";
                txtMaterialManage.Text = "";
                txtProductManage.Text = "";
                txtOtherActivity.Text = "";
                txtProblem.Text = "";
                txtPart.Text = "";
            }
        }

        private void txtDate_CloseUp(object sender, EventArgs e)
        {
            //查找对应的天气
            DateTime dtDdate = Convert.ToDateTime(txtDate.Value.Date);
            if (SearchResult(dtDdate) == 1)
            {
                ObjectLock.Unlock(pnlFloor, true);
                object[] os = new object[] { txtHandlePerson, txtProject, txtWeather, txtWind, txtTemperature, txtHumidity, txtMainWork, txtProjectManage, txtMaterialManage, txtProductManage, txtOtherActivity, txtProblem };
                ObjectLock.Lock(os);
                txtMainWork.Enabled = false;
                txtProjectManage.Enabled = false;
                txtMaterialManage.Enabled = false;
                txtProductManage.Enabled = false;
                txtOtherActivity.Enabled = false;
                txtProblem.Enabled = false;
                txtPart.Enabled = false;
            }
            else
            {
                if (curBillMaster.DocState == DocumentState.InExecute || curBillMaster.DocState == DocumentState.InAudit)
                {
                    ObjectLock.Lock(pnlFloor, true);
                    ObjectLock.Unlock(txtDate, true);
                }
                txtMainWork.Enabled = true;
                txtProjectManage.Enabled = true;
                txtMaterialManage.Enabled = true;
                txtProductManage.Enabled = true;
                txtOtherActivity.Enabled = true;
                txtProblem.Enabled = true;
                txtPart.Enabled = true;
            }
        }

        private bool SearchWeather(DateTime strDate)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate", strDate));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));//查询提交的信息
            IList list = model.ConstructionReportSrv.GetWeather(objectQuery);
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
                }
                else
                {
                    txtDate.Value = DateTime.Now;
                }
                txtHumidity.Text = "";
                txtTemperature.Text = "";
                txtWeather.Text = "";
                txtWind.Text = "";
                txtWeather.Tag = null;
                return false;
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
                    curBillMaster = model.ConstructionReportSrv.GetConstructReportById(Id);
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
                    txtMainWork.Enabled = true;
                    txtProjectManage.Enabled = true;
                    txtPart.Enabled = true;
                    txtMainWork.Enabled = true;
                    txtMaterialManage.Enabled = true;
                    txtOtherActivity.Enabled = true;
                    txtProductManage.Enabled = true;
                    txtProblem.Enabled = true;
                    cmsDg.Enabled = true;
                    butAdd.Enabled = true;
                    ObjectLock.Unlock(txtDate, true);
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    txtDate.Enabled = false;
                    txtHumidity.Enabled = false;
                    txtTemperature.Enabled = false;
                    txtWeather.Enabled = false;
                    txtWind.Enabled = false;
                    txtMainWork.Enabled = false;
                    txtProjectManage.Enabled = false;
                    txtPart.Enabled = false;
                    txtOtherActivity.Enabled = false;
                    txtMaterialManage.Enabled = false;
                    txtMainWork.Enabled = false;
                    butAdd.Enabled = false;
                    txtProductManage.Enabled = false;
                    txtProblem.Enabled = false;
                    cmsDg.Enabled = false;
                    //ObjectLock.Unlock(txtDate, true);
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
            txtHumidity.Text = "";
            txtMainWork.Text = "";
            txtMaterialManage.Text = "";
            txtOtherActivity.Text = "";
            txtPart.Text = "";
            txtProblem.Text = "";
            txtProductManage.Text = "";
            txtProjectManage.Text = "";
            txtTemperature.Text = "";
            txtWeather.Text = "";
            txtWind.Text = "";
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
                this.curBillMaster = new ConstructReport();
                txtDate.Value = DateTime.Now;
                DateTime strDate = Convert.ToDateTime(txtDate.Value.Date);
               
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                int i = SearchResult(strDate);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        private int SearchResult(DateTime strDate)
        {
            //ClearControl(pnlFloor);
            int i = 0;
            if (!SearchWeather(strDate))
            {
                i = 1;
                return i;
            }
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate", strDate));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.ConstructionReportSrv.GetConstructReport(objectQuery);
            if (list != null && list.Count > 0)
            {
                ////存在当日施工信息
                //if (strDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                //{
               
                //}
                ConstructReport manage = list[0] as ConstructReport;
                curBillMaster = manage;
                //this.txtCode.Text = manage.Code;
                this.txtHandlePerson.Text = manage.HandlePersonName;
                this.txtHandlePerson.Tag = manage.HandlePerson;
                this.txtProject.Text = manage.ProjectName;
                this.txtProject.Tag = manage.ProjectId;
                if (manage.WeatherGlass != null)
                {
                    this.txtWeather.Text = manage.WeatherGlass.WeatherCondition;
                    this.txtWeather.Tag = manage.WeatherGlass;
                    this.txtWind.Text = manage.WeatherGlass.WindDirection;
                    this.txtTemperature.Text = manage.WeatherGlass.Temperature;
                    this.txtHumidity.Text = manage.WeatherGlass.RelativeHumidity;
                }
                this.txtPart.Text = manage.ConstructSite;
                txtMainWork.Text = manage.CompletionSchedule;
                txtMaterialManage.Text = manage.MaterialCase;
                txtOtherActivity.Text = manage.OtherActivities;
                txtProblem.Text = manage.Problem;
                txtProductManage.Text = manage.SafetyControl;
                txtProjectManage.Text = manage.ProjectManage;

                if (manage.DocState == DocumentState.InExecute || manage.DocState == DocumentState.InAudit)
                {
                    i = 1;
                    //MessageBox.Show("施工信息已经存在并且已经提交！");
                    return i;
                }
                //else
                //{
                //    MessageBox.Show("施工信息已经存在！");
                //}
            }
            else
            {
                butAddManage(strDate);
            }
            return i;
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
                curBillMaster = model.ConstructionReportSrv.SaveConstructReport(curBillMaster);
                
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
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
                curBillMaster = model.ConstructionReportSrv.GetConstructReportById(curBillMaster.Id);
                this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString().Replace("-", "");
                ModelToView();
                return true;
            }
            string message = "此单状态为非编辑状态，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
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
                if (curBillMaster.DocState == DocumentState.InAudit || this.curBillMaster.DocState == DocumentState.InExecute)
                {
                    MessageBox.Show("信息已经提交，不可再保存！");
                    return false;
                }
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.ConstructionReportSrv.SaveConstructReport(curBillMaster);
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "日施工报告";
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
                curBillMaster = model.ConstructionReportSrv.GetConstructReportById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ConstructionReportSrv.DeleteByDao(curBillMaster)) return false;
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
                        curBillMaster = model.ConstructionReportSrv.GetConstructReportById(curBillMaster.Id);
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
                curBillMaster = model.ConstructionReportSrv.GetConstructReportById(curBillMaster.Id);
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
                MessageBox.Show("工作内容及进度完成情况不能为空！");
                return false;
            }
            if (txtProjectManage.Text.Equals(""))
            {
                MessageBox.Show("项目管理活动不能为空！");
                return false;
            }
            if (txtMaterialManage.Text.Equals(""))
            {
                MessageBox.Show("设备运行、材料进场、材料使用情况不能为空！");
                return false;
            }
            if (txtProductManage.Text.Equals(""))
            {
                MessageBox.Show("生产安全控制情况不能为空！");
                return false;
            }

            if (txtOtherActivity.Text.Equals(""))
            {
                MessageBox.Show("其他活动情况不能为空！");
                return false;
            }
            if (txtProblem.Text.Equals(""))
            {
                MessageBox.Show("存在的问题不能为空！");
                return false;
            }
            if (curBillMaster.Id == null)
            {
                DateTime strDate = Convert.ToDateTime(txtDate.Value.Date);
                ObjectQuery objectQuery = new ObjectQuery();
                objectQuery.AddCriterion(Expression.Eq("CreateDate", strDate));
                IList list = model.ConstructionReportSrv.GetConstructReport(objectQuery);
                if (list != null && list.Count > 0)
                {
                    if (MessageBox.Show("已经存在今天的日施工情况，是否更新？", "更新记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        ConstructReport weather = list[0] as ConstructReport;
                        curBillMaster = weather;
                    }
                    else
                    {
                        txtHumidity.Text = "";
                        txtMainWork.Text = "";
                        txtMaterialManage.Text = "";
                        txtOtherActivity.Text = "";
                        txtPart.Text = "";
                        txtProblem.Text = "";
                        txtProductManage.Text = "";
                        txtProjectManage.Text = "";
                        txtTemperature.Text = "";
                        txtWeather.Text = "";
                        txtWind.Text = "";
                        return false;
                    }
                }
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
                if (txtWeather.Tag == null)
                {
                    MessageBox.Show("没有天气信息不可保存！");
                    return false;
                }
                curBillMaster.WeatherGlass = txtWeather.Tag as WeatherInfo;
                curBillMaster.MaterialCase = ClientUtil.ToString(txtMaterialManage.Text);//材料情况
                curBillMaster.Problem = ClientUtil.ToString(txtProblem.Text);
                curBillMaster.CompletionSchedule = ClientUtil.ToString(txtMainWork.Text);
                curBillMaster.OtherActivities = ClientUtil.ToString(txtOtherActivity.Text);
                curBillMaster.ProjectManage = ClientUtil.ToString(txtProjectManage.Text);
                curBillMaster.SafetyControl = ClientUtil.ToString(txtProductManage.Text);
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
                this.txtDate.Value = curBillMaster.CreateDate;
                this.txtHandlePerson.Text = curBillMaster.HandlePersonName;
                this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                this.txtProjectManage.Text = curBillMaster.ProjectManage;
                this.txtProductManage.Text = curBillMaster.SafetyControl;
                this.txtProblem.Text = curBillMaster.Problem;
                this.txtPart.Text = curBillMaster.ConstructSite;
                this.txtOtherActivity.Text = curBillMaster.OtherActivities;
                this.txtMaterialManage.Text = curBillMaster.MaterialCase;
                this.txtMainWork.Text = curBillMaster.CompletionSchedule;
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
            if (LoadTempleteFile(@"项目每日施工情况报表.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"项目每日施工情况报表.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"项目每日施工情况报表.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("项目每日施工情况报表【" + curBillMaster + "】", false, false, true);
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

        private void FillFlex(ConstructReport billMaster)
        {
            int detailStartRowNumber = 7;//7为模板中的行号
            int detailCount = billMaster.Details.Count;

            ////插入明细行
            //flexGrid1.InsertRow(ConstructReport, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //主表数据


            flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(4, 2).Text = billMaster.WeatherGlass.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 5).Text = billMaster.WeatherGlass.WeatherCondition;
            flexGrid1.Cell(4, 8).Text = billMaster.ConstructSite;

            flexGrid1.Cell(5, 2).Text = billMaster.WeatherGlass.RelativeHumidity;
            flexGrid1.Cell(5, 5).Text = billMaster.WeatherGlass.Temperature;
            flexGrid1.Cell(5, 8).Text = billMaster.WeatherGlass.WindDirection;


            flexGrid1.Cell(6, 2).Text = billMaster.CompletionSchedule;
            flexGrid1.Cell(6, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(6, 2).WrapText = true;
            flexGrid1.Row(6).AutoFit();
            flexGrid1.Cell(7, 2).Text = billMaster.ProjectManage;
            flexGrid1.Cell(7, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(7, 2).WrapText = true;
            flexGrid1.Row(7).AutoFit();
            flexGrid1.Cell(8, 2).Text = billMaster.MaterialCase;
            flexGrid1.Cell(8, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(8, 2).WrapText = true;
            flexGrid1.Row(8).AutoFit();
            flexGrid1.Cell(9, 2).Text = billMaster.SafetyControl;
            flexGrid1.Cell(9, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(9, 2).WrapText = true;
            flexGrid1.Row(9).AutoFit();
            flexGrid1.Cell(10, 2).Text = billMaster.OtherActivities;
            flexGrid1.Cell(10, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(10, 2).WrapText = true;
            flexGrid1.Row(10).AutoFit();
            flexGrid1.Cell(11, 2).Text = billMaster.Problem;
            flexGrid1.Cell(11, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(11, 2).WrapText = true;
            flexGrid1.Row(11).AutoFit();
           

           
        }
    }
}
