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
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng
{
    public partial class VWeatherMng : TMasterDetailView
    {
        private MWeatherMng model = new MWeatherMng();
        private WeatherInfo curBillMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public WeatherInfo CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        CurrentProjectInfo projectInfo;
        public VWeatherMng()
        {
            InitializeComponent();
            this.txtDate.CloseUp +=new EventHandler(txtDate_CloseUp);
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void txtDate_CloseUp(object sender, EventArgs e)
        {
            curBillMaster = new WeatherInfo();
            curBillMaster.CreateDate = ConstObject.LoginDate;
            curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
            curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
            curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
            curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
            curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.DocState = DocumentState.Edit;//状态
            CurrentProjectInfo projectIn = StaticMethod.GetProjectInfo();
            curBillMaster.ProjectId = projectIn.Id;
            curBillMaster.ProjectName = projectIn.Name;
            ClearControl(pnlFloor);
            DayOfWeek();
            txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
            txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
            //归属项目
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                txtProject.Tag = projectInfo;
                txtProject.Text = projectInfo.Name;
            }
            DateTime strDate = Convert.ToDateTime(txtDate.Value.Date);
            if (!SearchWeatherManage(strDate))
            {
                ObjectLock.Lock(pnlFloor, true);
                ObjectLock.Unlock(txtDate, true);

            }
            else
            {
                ObjectLock.Unlock(pnlFloor, true);
                object[] os = new object[] { txtHandlePerson, txtProject, txtWeek };
                ObjectLock.Lock(os);
                txtWeather.Enabled = true;
                txtTemperature.Enabled = true;
                txtHumidity.Enabled = true;
                txtWind.Enabled = true;
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
                    curBillMaster = model.WeatherSrv.GetWeatherById(Id);
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
                    if (curBillMaster.DocState == DocumentState.InExecute)
                    {
                        txtHumidity.Enabled = false;
                        txtTemperature.Enabled = false;
                        txtWeather.Enabled = false;
                        txtWind.Enabled = false;
                    }
                    else
                    {
                        txtHumidity.Enabled = true;
                        txtTemperature.Enabled = true;
                        txtWeather.Enabled = true;
                        txtWind.Enabled = true;
                    }
                    cmsDg.Enabled = true;
                    ObjectLock.Unlock(txtDate, true);
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    txtDate.Enabled = false;
                    txtHumidity.Enabled = false;
                    txtTemperature.Enabled = false;
                    txtWeather.Enabled = false;
                    txtWind.Enabled = false;    
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
                ObjectLock.Unlock(pnlFloor, true);
                txtDate.Enabled = true;

            }
            else
            {
                txtDate.Enabled = false;
                 ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtHandlePerson, txtProject,txtWeek,txtDate };
            ObjectLock.Lock(os);
            //object[] os1 = new object[] { txtDate};
            //ObjectLock.Unlock(os1);
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
                curBillMaster = new WeatherInfo();
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
                curBillMaster.DocState = DocumentState.Edit;//状态
                DayOfWeek();
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
                if (!SearchWeatherManage(txtDate.Value))
                {

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            
            return true;
        }
        //查询晴雨表信息
        private bool SearchWeatherManage(DateTime strDate)
        {
            DayOfWeek();
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate", ClientUtil.ToDateTime(strDate.ToShortDateString())));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.WeatherSrv.GetWeather(objectQuery);
            if (list != null && list.Count > 0)
            {
                WeatherInfo weather = list[0] as WeatherInfo;
                curBillMaster = weather;
                this.txtHandlePerson.Text = weather.HandlePersonName;
                this.txtHandlePerson.Tag = weather.HandlePerson;
                this.txtHumidity.Text = weather.RelativeHumidity;
                this.txtTemperature.Text = weather.Temperature;
                this.txtWeather.Text = weather.WeatherCondition;
                this.txtWind.Text = weather.WindDirection;
                if (weather.DocState == DocumentState.InAudit || weather.DocState == DocumentState.InExecute)
                {
                    MessageBox.Show("当日晴雨信息已经存在并且信息已经提交！");
                    return false;
                }
                else
                {
                    MessageBox.Show("当日晴雨信息已经存在！");
                }
            }
            
            return true;
        }

        //通过日期查找星期
        private void DayOfWeek()
        {
            string strWeek = ConvertDayOfWeekToZh(txtDate.Value.DayOfWeek);
            txtWeek.Text = strWeek;
            
            DateTime strDate = Convert.ToDateTime(txtDate.Value.Date);
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateDate",ClientUtil.ToDateTime(strDate.ToShortDateString())));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.WeatherSrv.GetWeather(objectQuery);
            if (list != null && list.Count > 0)
            {
                WeatherInfo weather = list[0] as WeatherInfo;
                curBillMaster = weather;
                this.txtHumidity.Text = weather.RelativeHumidity;
                this.txtTemperature.Text = weather.Temperature;
                this.txtWeather.Text = weather.WeatherCondition;
                this.txtWind.Text = weather.WindDirection;
            }
        }
        private string ConvertDayOfWeekToZh(System.DayOfWeek strWeek)
        {
            string DayOfWeekZh = " ";
            switch (strWeek.ToString())
            {
                case "Sunday":
                    DayOfWeekZh = "星期日";
                    break;
                case "Monday":
                    DayOfWeekZh = "星期一";
                    break;
                case "Tuesday":
                    DayOfWeekZh = " 星期二";
                    break;
                case "Wednesday":
                    DayOfWeekZh = "星期三";
                    break;
                case "Thursday":
                    DayOfWeekZh = "星期四";
                    break;
                case "Friday":
                    DayOfWeekZh = "星期五";
                    break;
                case "Saturday":
                    DayOfWeekZh = "星期六";
                    break;
            }
            return DayOfWeekZh;
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
                curBillMaster = model.WeatherSrv.GetWeatherById(curBillMaster.Id);
                this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString().Replace("-", "");
                //ModelToView();
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
                curBillMaster = model.WeatherSrv.SaveWeather(curBillMaster);
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
                    MessageBox.Show("信息已经提交，不能再做修改！");
                    return false;
                }
                else
                {
                    curBillMaster = model.WeatherSrv.SaveWeather(curBillMaster);
                    //更新Caption
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "晴雨表";
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
                    this.ViewCaption = ViewName + "-" + txtDate.Value.Date.ToShortDateString().Replace("-","");
                    MessageBox.Show("保存成功！");
                    return true;
                }
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
                curBillMaster = model.WeatherSrv.GetWeatherById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.WeatherSrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "晴雨表";
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
                        curBillMaster = model.WeatherSrv.GetWeatherById(curBillMaster.Id);
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
                curBillMaster = model.WeatherSrv.GetWeatherById(curBillMaster.Id);
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
            bool validity = true;
            if (txtHumidity.Text.Equals(""))
            {
                MessageBox.Show("湿度不能为空！");
                return false;
            }
            if (txtDate.Value > DateTime.Now)
            {
                MessageBox.Show("不可设置今天以后的天气状况！");
                return false;
            }
            if (txtTemperature.Text.Equals(""))
            {
                MessageBox.Show("温度不能为空！");
                return false;
            }
            if (txtWeather.Text.Equals(""))
            {
                MessageBox.Show("天气状况不能为空！");
                return false;
            }
            if (txtWind.Text.Equals(""))
            {
                MessageBox.Show("风力风向不能为空！");
                return false;
            }
            //if (curBillMaster.Id == null)
            //{
            //    if (!SearchWeatherManage(txtDate.Value.Date))
            //    {
            //        return false;
            //    }
            //}
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.CreateDate = txtDate.Value.Date;
                curBillMaster.RelativeHumidity = ClientUtil.ToString(txtHumidity.Text);
                curBillMaster.CreateDate = txtDate.Value.Date;
                curBillMaster.Temperature = ClientUtil.ToString(txtTemperature.Text);
                curBillMaster.WeatherCondition = ClientUtil.ToString(txtWeather.Text);
                string strWeek = ClientUtil.ToString(txtWeek.Text);
                int IntWeek = 0;
                if(strWeek.Trim().Equals("星期一"))
                {
                    IntWeek = 1;
                }
                if(strWeek.Trim().Equals("星期二"))
                {
                    IntWeek = 2;
                }
                if (strWeek.Trim().Equals("星期三"))
                {
                    IntWeek = 3;
                }
                if (strWeek.Trim().Equals("星期四"))
                {
                    IntWeek = 4;
                }
                if (strWeek.Trim().Equals("星期五"))
                {
                    IntWeek = 5;
                }
                if (strWeek.Trim().Equals("星期六"))
                {
                    IntWeek = 6;
                }
                if (strWeek.Trim().Equals("星期日") || strWeek.Trim().Equals("星期天"))
                {
                    IntWeek = 7;
                }
                if(IntWeek != 0)
                {
                     curBillMaster.Week = IntWeek;
                }
                curBillMaster.WindDirection = ClientUtil.ToString(txtWind.Text);
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
                this.txtWind.Text = curBillMaster.WindDirection;
                this.txtDate.Value = Convert.ToDateTime(curBillMaster.CreateDate);
                string strWeek = ClientUtil.ToString(curBillMaster.Week);
                if (strWeek.Equals("1"))
                {
                    strWeek = "星期一";
                }
                if (strWeek.Equals("2"))
                {
                    strWeek = "星期二";
                }
                if (strWeek.Equals("3"))
                {
                    strWeek = "星期三";
                }
                if (strWeek.Equals("4"))
                {
                    strWeek = "星期四";
                }
                if (strWeek.Equals("5"))
                {
                    strWeek = "星期五";
                }
                if (strWeek.Equals("6"))
                {
                    strWeek = "星期六";
                }
                if (strWeek.Equals("7"))
                {
                    strWeek = "星期日";
                }
                if (strWeek != "0")
                {
                    this.txtWeek.Text = strWeek;
                }
                else
                {
                    DayOfWeek();
                }
                this.txtWeather.Text = curBillMaster.WeatherCondition;
                this.txtTemperature.Text = curBillMaster.Temperature;
                this.txtHumidity.Text = curBillMaster.RelativeHumidity;
                this.txtProject.Tag = curBillMaster.ProjectId;
                this.txtProject.Text = curBillMaster.ProjectName;
                this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                this.txtHandlePerson.Text = curBillMaster.HandlePersonName;
                
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
        //    return true;
        //}

        //private bool LoadTempleteFile(string modelName)
        //{
        //    ExploreFile eFile = new ExploreFile();
        //    string path = eFile.Path;
        //    if (eFile.IfExistFileInServer(modelName))
        //    {
        //        eFile.CreateTempleteFileFromServer(modelName);
        //        //载入格式和数据
        //        flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
        //    }
        //    else
        //    {
        //        MessageBox.Show("未找到模板格式文件【" + modelName + "】");
        //        return false;
        //    }
        //    return true;
        //}

        //private void FillFlex(MaterialRentalOrderMaster billMaster)
        //{
        //    int detailStartRowNumber = 7;//7为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;

        //    //主表数据

        //    flexGrid1.Cell(2, 1).Text = "使用单位：";
        //    flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
        //    flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
        //    flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
        //    flexGrid1.Cell(4, 5).Text = "";//合同名称
        //    flexGrid1.Cell(4, 7).Text = "";//材料分类
        //    flexGrid1.Cell(5, 2).Text = "";//租赁单位
        //    flexGrid1.Cell(5, 2).WrapText = true;
        //    flexGrid1.Cell(5, 5).Text = "";//承租单位
        //    flexGrid1.Row(5).AutoFit();
        //    flexGrid1.Cell(5, 7).Text = billMaster.RealOperationDate.ToShortDateString();//签订日期

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //结算规则
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = Enum.GetName(typeof(EnumMaterialMngBalRule), detail.BalRule);
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //数量
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

        //        //日租金
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

        //        //金额
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = "";

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //    }
        //}
    }
}
