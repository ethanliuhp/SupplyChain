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
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement
{
    public partial class VConstruction : TMasterDetailView
    {
        private MConstruction model = new MConstruction();
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
        private ConstructionManage curBillMaster;
        WeatherInfo weaInfo = new WeatherInfo();
        /// <summary>
        /// 当前单据
        /// </summary>
        public ConstructionManage CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VConstruction()
        {
            InitializeComponent();
            InitEvent();
        }
        public void InitEvent()
        {
            this.txtDate.CloseUp += new EventHandler(txtDate_CloseUp);
            this.butAdd.Click +=new EventHandler(butAdd_Click);
        }

        void butAdd_Click(object sender,EventArgs e)
        {
            butAddManage(txtDate.Value.Date);
        }

        private bool butAddManage(DateTime dt)
        {
            if (!SearchWeather(dt))
            {
                return false;
            }
            IList ResultList = model.ConstructionSrv.GetConstructionList(projectInfo, dt);
            if (ResultList != null)
            {
                ConstructionManage manage = ResultList[0] as ConstructionManage;
                if (manage.ProductionRecord != null)
                {
                    txtProductRecord.Text = manage.ProductionRecord;
                }
                else
                {
                    txtProductRecord.Text = "";
                }
                if (manage.WorkRecord != null)
                {
                    txtTechnologyRecord.Text = manage.WorkRecord;
                }
                else
                {
                    txtTechnologyRecord.Text = "";
                }
                if (manage.QualityWorkRecord != null)
                {
                    txtQulityRecord.Text = manage.QualityWorkRecord;
                }
                else
                {
                    txtQulityRecord.Text = "";
                }
                if (manage.SaftyWorkRecord != null)
                {
                    txtSaftyRecord.Text = manage.SaftyWorkRecord;
                }
                else
                {
                    txtSaftyRecord.Text = "";
                }
                if (manage.ConstructSite != null)
                {
                    txtPart.Text = manage.ConstructSite;
                }
                else
                {
                    txtPart.Text = "";
                }
            }
            else
            {
                txtProductRecord.Text = "";
                txtTechnologyRecord.Text = "";
                txtPart.Text = "";
            }
            return true;
        }
        //时间控件的下拉框关闭事件
        private void txtDate_CloseUp(object sender, EventArgs e)
        {
            
            //查找对应的天气
            DateTime dtDdate = Convert.ToDateTime(txtDate.Value.Date);

            if (SearchResult(dtDdate) == 1)
            {
                ObjectLock.Unlock(pnlFloor, true);
                object[] os = new object[] { txtHandlePerson, txtProject, txtCode, txtWeather, txtWind, txtTemperature, txtHumidity, txtProductRecord, txtTechnologyRecord, txtPart, txtEmergency };
                ObjectLock.Lock(os);
                txtProductRecord.Enabled = false;
                txtTechnologyRecord.Enabled = false;
                txtPart.Enabled = false;
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                ObjectLock.Unlock(txtProductRecord, true);
                ObjectLock.Unlock(txtTechnologyRecord, true);
                ObjectLock.Unlock(txtPart, true);
                ObjectLock.Unlock(txtEmergency, true);
                ObjectLock.Unlock(txtDate, true);
                txtProductRecord.Enabled = true;
                txtTechnologyRecord.Enabled = true;
                txtPart.Enabled = true;
            }
        }
        //查询晴雨表信息
        private bool SearchWeather(DateTime strDate)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate", strDate));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));//查询提交的信息
            IList list = model.ConstructionSrv.GetWeather(objectQuery);
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
                    curBillMaster = model.ConstructionSrv.GetConstructionById(Id);
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
                    txtProductRecord.Enabled = true;
                    txtTechnologyRecord.Enabled = true;
                    txtPart.Enabled = true;
                    cmsDg.Enabled = true;
                    butAdd.Enabled = true;
                    txtQulityRecord.Enabled = true;
                    txtSaftyRecord.Enabled = true;
                    ObjectLock.Unlock(txtDate, true);
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    txtDate.Enabled = false;
                    txtHumidity.Enabled = false;
                    txtTemperature.Enabled = false;
                    txtWeather.Enabled = false;
                    txtWind.Enabled = false;
                    txtProductRecord.Enabled = false;
                    txtTechnologyRecord.Enabled = false;
                    txtPart.Enabled = false;
                    txtSaftyRecord.Enabled = false;
                    txtQulityRecord.Enabled = false;
                    butAdd.Enabled = false;
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
            object[] os = new object[] { txtHandlePerson, txtProject, txtCode, txtWeather, txtWind, txtTemperature, txtHumidity };
            ObjectLock.Lock(os);
        }

        //清空数据
        private void ClearView()
        {
            txtCode.Text = "";
            txtHandlePerson.Text = "";
            txtEmergency.Text = "";
            txtHumidity.Text = "";
            txtPart.Text = "";
            txtProductRecord.Text = "";
            txtTechnologyRecord.Text = "";
            txtTemperature.Text = "";
            txtQulityRecord.Text = "";
            txtSaftyRecord.Text = "";
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
                this.curBillMaster = new ConstructionManage();
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
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate",ClientUtil.ToDateTime(strDate.ToShortDateString())));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.ConstructionSrv.GetConstruction(objectQuery);
            if (list != null && list.Count > 0)
            {
                ConstructionManage manage = list[0] as ConstructionManage;
                curBillMaster = manage;
                this.txtCode.Text = manage.Code;
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
                this.txtEmergency.Text = manage.Emergency;
                this.txtProductRecord.Text = manage.ProductionRecord;
                this.txtTechnologyRecord.Text = manage.WorkRecord;
                this.txtQulityRecord.Text = manage.QualityWorkRecord;
                this.txtSaftyRecord.Text = manage.SaftyWorkRecord;
                if (manage.DocState == DocumentState.InExecute || manage.DocState == DocumentState.InAudit)
                {
                    //MessageBox.Show("施工信息已经存在并且已经提交！");
                    i = 1;
                    return i;
                }
                //else
                //{
                //    MessageBox.Show("施工信息已经存在！");
                //}
            }
            else
            {
                if (!butAddManage(strDate))
                {
                    return i;
                }
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
                curBillMaster = model.ConstructionSrv.SaveConstruction(curBillMaster);
                
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
                curBillMaster = model.ConstructionSrv.GetConstructionById(curBillMaster.Id);
                this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString().Replace("-", "");
                ModelToView();
                //DateTime dtTime = Convert.ToDateTime(txtDate.Value.Date);
                //SearchWeather(dtTime);
                //DateTime strDate = Convert.ToDateTime(txtDate.Value.Date);
                //IList ResultList = model.ConstructionSrv.GetConstructionList(projectInfo, strDate);
                //if (ResultList != null)
                //{
                //    ConstructionManage manage = ResultList[0] as ConstructionManage;
                //    if (manage.ProductionRecord != null)
                //    {
                //        txtProductRecord.Text = manage.ProductionRecord;
                //    }
                //    if (manage.WorkRecord != null)
                //    {
                //        txtSaftyRecord.Text = manage.WorkRecord;
                //    }
                //    if (manage.ConstructSite != null)
                //    {
                //        txtPart.Text = manage.ConstructSite;
                //    }
                //}
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
                curBillMaster = model.ConstructionSrv.SaveConstruction(curBillMaster);
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "施工日志表";
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
                this.txtCode.Text = curBillMaster.Code.ToString();
                this.ViewCaption = ViewName + "-" + txtCode.Text.ToString();
                this.txtCode.Text = curBillMaster.Code.ToString();
                MessageBox.Show("保存成功！");
                this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString().Replace("-", "");
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
                curBillMaster = model.ConstructionSrv.GetConstructionById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ConstructionSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "施工日志";
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
                        curBillMaster = model.ConstructionSrv.GetConstructionById(curBillMaster.Id);
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
                curBillMaster = model.ConstructionSrv.GetConstructionById(curBillMaster.Id);
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
            if (txtEmergency.Text.Equals(""))
            {
                MessageBox.Show("突发事件不能为空！");
                return false;
            }
            if (txtWeather.Text.Equals(""))
            {
                MessageBox.Show("天气状况不能为空！");
                return false;
            }
            if (txtProductRecord.Text.Equals(""))
            {
                MessageBox.Show("生产情况记录不能为空！");
                return false;
            }
            if (txtTechnologyRecord.Text.Equals(""))
            {
                MessageBox.Show("技术质量安全工作记录不能为空！");
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
                curBillMaster.WeatherGlass = txtWeather.Tag as WeatherInfo;
                curBillMaster.Emergency = ClientUtil.ToString(txtEmergency.Text);
                curBillMaster.ProductionRecord = ClientUtil.ToString(txtProductRecord.Text);
                curBillMaster.WorkRecord = ClientUtil.ToString(txtTechnologyRecord.Text);
                curBillMaster.QualityWorkRecord = ClientUtil.ToString(txtQulityRecord.Text);
                curBillMaster.SaftyWorkRecord = ClientUtil.ToString(txtSaftyRecord.Text);
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
                this.txtCode.Text = curBillMaster.Code;
                this.txtPart.Text = curBillMaster.ConstructSite;
                this.txtProject.Text = curBillMaster.ProjectName;
                this.txtProject.Tag = curBillMaster.ProjectId;
                this.txtTemperature.Text = curBillMaster.WeatherGlass.Temperature;
                this.txtWeather.Text = curBillMaster.WeatherGlass.WeatherCondition;
                this.txtWeather.Tag = curBillMaster.WeatherGlass;
                this.txtWind.Text = curBillMaster.WeatherGlass.WindDirection;
                this.txtHumidity.Text = curBillMaster.WeatherGlass.RelativeHumidity;
                this.txtDate.Value = curBillMaster.CreateDate;
                this.txtHandlePerson.Text = curBillMaster.HandlePersonName;
                this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                this.txtTechnologyRecord.Text = curBillMaster.WorkRecord;
                this.txtQulityRecord.Text = curBillMaster.QualityWorkRecord;
                this.txtSaftyRecord.Text = curBillMaster.SaftyWorkRecord;
                this.txtProductRecord.Text = curBillMaster.ProductionRecord;
                this.txtEmergency.Text = curBillMaster.Emergency;
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
            if (LoadTempleteFile(@"项目施工日志.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"项目施工日志.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"项目施工日志.flx") == false) return false;
            FillFlex(curBillMaster);
            flexGrid1.ExportToExcel("项目施工日志【" + curBillMaster.Code + "】", false, false, true);
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

        private void FillFlex(ConstructionManage billMaster)
        {
            int detailStartRowNumber = 8;
            int detailCount = billMaster.Details.Count;
            //设置单元格的边框，对齐方式
            
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            //CommonUtil.SetFlexGridAutoFit(this.flexGrid1);
            ////CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            pageSetup.LeftMargin = 0;
            pageSetup.RightMargin = 0;
            pageSetup.BottomMargin = 1;
            pageSetup.TopMargin = 1;
            pageSetup.Landscape = true;
            pageSetup.CenterHorizontally = true;
            //主表数据

            flexGrid1.Cell(3, 2).Text = ClientUtil.ToString(billMaster.ProjectName);
            flexGrid1.Cell(4, 2).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(4, 6).Text = ClientUtil.ToString(billMaster.ConstructSite);

            flexGrid1.Cell(5, 2).Text = ClientUtil.ToString(billMaster.WeatherGlass.Temperature);
            flexGrid1.Cell(5, 5).Text = billMaster.WeatherGlass.WeatherCondition;//天气
            flexGrid1.Cell(5, 8).Text = billMaster.WeatherGlass.WindDirection;

            flexGrid1.Cell(6, 2).Text = ClientUtil.ToString(billMaster.Emergency);
            flexGrid1.Cell(6, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(6, 2).WrapText = true;
            //flexGrid1.Row(6).AutoFit();
            flexGrid1.Cell(7, 2).Text = ClientUtil.ToString(billMaster.ProductionRecord);
            flexGrid1.Cell(7, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(7, 2).WrapText = true;
            //flexGrid1.Row(7).AutoFit();
            flexGrid1.Cell(8, 3).Text = billMaster.WorkRecord;
            flexGrid1.Cell(8, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            flexGrid1.Cell(8, 3).WrapText = true;
            this.flexGrid1.Cell(1, 9).Text = billMaster.Code.Substring(billMaster.Code.Length - 11);
            this.flexGrid1.Cell(1, 9).CellType = FlexCell.CellTypeEnum.BarCode;
            this.flexGrid1.Cell(1, 9).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
            //flexGrid1.Row(8).AutoFit();
        }
    }
}
