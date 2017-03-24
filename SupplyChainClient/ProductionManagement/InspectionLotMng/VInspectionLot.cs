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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using Castle.DynamicProxy.Builder.CodeBuilder.SimpleAST;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng
{
    public partial class VInspectionLot : TMasterDetailView
    {
        private MInspectionLotMng model = new MInspectionLotMng();
        private MAcceptanceInspectionMng accemodel = new MAcceptanceInspectionMng(); 
        public MProductionMng weekmodel = new MProductionMng();
        private InspectionLot curBillMaster;
        /// <summary>
        /// 当前单据
        /// </summary>
        public InspectionLot CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VInspectionLot()
        {
            InitializeComponent();
            btnSearch.Click +=new EventHandler(btnSearch_Click);
            //this.btnSelect.Click += new EventHandler(btnSelect_Click);
        }
        void btnSearch_Click(object sender,EventArgs e)
        {
            VWeekSelector vss = new VWeekSelector();
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;
            curBillMaster.Details.Clear();
            foreach (WeekScheduleDetail detail in list)
            {
                txtProjectTask.Text = detail.GWBSTreeName;
                txtProjectTask.Tag = detail.GWBSTree;
            }
        }

        //void btnSelect_Click(object sender,EventArgs e)
        //{
        //    VInspectionLotSelect vss = new VInspectionLotSelect();
        //    vss.ShowDialog();
        //    IList list = vss.Result;
        //    if (list == null || list.Count == 0) return;
        //    curBillMaster.Details.Clear();
        //    foreach (InspectionLot detail in list)
        //    {
        //        txtProjectTask.Text = detail.Code;
        //        txtProjectTask.Tag = detail;
        //    }
        //}


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
                    curBillMaster = model.InspectionLotSrv.GetInspectionLotById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
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
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
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
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }
            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtProject, txtAccountStatues };
            ObjectLock.Lock(os);
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
                this.curBillMaster = new InspectionLot();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                //归属组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                ////责任人
                //txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                //txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
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
                curBillMaster = model.InspectionLotSrv.GetInspectionLotById(curBillMaster.Id);
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
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.InspectionLotSrv.SaveInspectionLot(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "检验批";
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
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                curBillMaster = model.InspectionLotSrv.GetInspectionLotById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    //查询验收检查记录，一并删除
                    IList acceptanceList = accemodel.AcceptanceInspectionSrv.GetAcceptanceInspectionByInsLotGUID(curBillMaster);
                    foreach (AcceptanceInspection acce in acceptanceList)
                    {
                        accemodel.AcceptanceInspectionSrv.DeleteByDao(acce);             
                    }
                    if (!model.InspectionLotSrv.DeleteByDao(curBillMaster)) return false;
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
                        curBillMaster = model.InspectionLotSrv.GetInspectionLotById(curBillMaster.Id);
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
                curBillMaster = model.InspectionLotSrv.GetInspectionLotById(curBillMaster.Id);
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
            if (txtProjectTask.Text.Equals(""))
            {
                MessageBox.Show("工程项目任务不能为空！");
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
                curBillMaster.ProjectTaskName = ClientUtil.ToString(txtProjectTask.Text);//工程项目任务名称
                curBillMaster.ProjectTask = txtProjectTask.Tag as GWBSTree;//工程项目任务
                curBillMaster.InsUpdateDate = Convert.ToDateTime(dtpUpdateDate.Value);//验收更新时间
                //curBillMaster.AccountStatus = ClientUtil.ToString(txtAccountStatues.Text);//验收状态
                curBillMaster.AccountStatus = "0000000";
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
                this.txtProjectTask.Tag = curBillMaster.ProjectTask;
                this.txtProjectTask.Text = curBillMaster.ProjectTaskName;
                //this.txtProjectOrg.Text = curBillMaster.OperOrgInfoName;
                //this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                //this.txtHandlePerson.Text = curBillMaster.HandlePersonName;
                //将信息全部一文字形式写出来
                string strState = curBillMaster.AccountStatus;
                string strName = "";
                string strStates = "";
                for (int i = 0; i < strState.Length; i++)
                {
                    strStates = strState.Substring(i, 1);
                    if (strStates.Equals("1"))
                    {
                        strStates = "检查通过";
                    }
                    if (strStates.Equals("X"))
                    {
                        strStates = "无需检查";
                    }
                    if (strStates.Equals("0"))
                    {
                        strStates = "未检查";
                    }
                    if (i == 0)
                    {
                        strName += "工长质检";
                    }
                    if (i == 1)
                    {
                        strName += "质监员质检";
                    }
                    if (i == 2)
                    {
                        strName += "工程进度";
                    }
                    if (i == 3)
                    {
                        strName += "安全专业";
                    }
                    if (i == 4)
                    {
                        strName += "物资专业";
                    }
                    if (i == 5)
                    {
                        strName += "技术专业";
                    }
                    if (i == 6)
                    {
                        strName += "监理质检";
                    }
                    if (i == 6)
                    {
                        strName += strStates;
                    }
                    else
                    {
                        strName += strStates + "-";
                    }
                }
                this.txtAccountStatues.Text = strName;
                this.dtpUpdateDate.Value = curBillMaster.InsUpdateDate;
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
