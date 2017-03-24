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
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
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
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost
{
    public partial class VSpecialCost : TMasterDetailView
    {
        private MSpecialCost model = new MSpecialCost();
        private SpecialCostMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        /// <summary>
        /// 当前单据
        /// </summary>
        public SpecialCostMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VSpecialCost()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            
        }
      
        public void InitData()
        {
            //给费用类型添加下拉列表框的信息
            VBasicDataOptr.InitZXCostType(txtCostType, true);
        }

        private void InitEvent()
        {
            this.txtManageRace.tbTextChanged +=new EventHandler(txtManageRace_tbTextChanged);
            this.txtOrderMoney.tbTextChanged +=new EventHandler(txtOrderMoney_tbTextChanged);
            this.btnSearch.Click +=new EventHandler(btnSearch_Click);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];
                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtGWBSName.Text = task.Name;
                    txtGWBSName.Tag = task;
                }
            }
        }

        void txtManageRace_tbTextChanged(object sender, EventArgs e)
        {
            if (txtOrderMoney.Text != "" && !string.IsNullOrEmpty(txtManageRace.Text))
            {
                decimal planmoney = (1 - ClientUtil.ToDecimal(txtManageRace.Text) / 100) * ClientUtil.ToDecimal(txtOrderMoney.Text);
                txtPlanMoney.Text = ClientUtil.ToString(planmoney);
            }
        }

        void txtOrderMoney_tbTextChanged(object sender, EventArgs e)
        {
            if (txtManageRace.Text != "" && !string.IsNullOrEmpty(txtOrderMoney.Text))
            {
                decimal planmoney = (1 - ClientUtil.ToDecimal(txtManageRace.Text) / 100) * ClientUtil.ToDecimal(txtOrderMoney.Text);
                txtPlanMoney.Text = ClientUtil.ToString(planmoney);
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
                    curBillMaster = model.SpecialCostSrv.GetSpecialCostById(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = ConstObject.LoginPersonInfo;
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
            object[] os = new object[] {dtpCreateDate, txtHandlePerson, txtProject,txtCode,txtPlanMoney };
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
                movedDtlList = new ArrayList();

                this.curBillMaster = new SpecialCostMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//业务组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                curBillMaster.DocState = DocumentState.Edit;//状态
                
                //制单日期
                dtpCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
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
                curBillMaster = model.SpecialCostSrv.GetSpecialCostById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
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
                curBillMaster = model.SpecialCostSrv.SaveSpecialCost(curBillMaster);
                //插入日志
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                curBillMaster = model.SaveSpecialCost(curBillMaster);

                //插入日志
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "专项管理费用单";
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
                 if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                 {
                     curBillMaster = model.SpecialCostSrv.GetSpecialCostById(curBillMaster.Id);
                     if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                     {
                         if (!model.SpecialCostSrv.DeleteByDao(curBillMaster)) return false;
                         LogData log = new LogData();
                         log.BillId = curBillMaster.Id;
                         log.BillType = "专项管理费用单";
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
                 string message = "此单状态为【{0}】，不能删除！";
                 message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
                 MessageBox.Show(message);
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
                        curBillMaster = model.SpecialCostSrv.GetSpecialCostById(curBillMaster.Id);
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
                curBillMaster = model.SpecialCostSrv.GetSpecialCostById(curBillMaster.Id);
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
            if (txtGWBSName.Tag == null)
            {
                MessageBox.Show("工程任务名称不允许为空！");
                txtGWBSName.Focus();
                return false;
            }
            if (txtCostType.Text == "")
            {
                MessageBox.Show("费用类型不允许为空！");
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
                curBillMaster.CreateDate = ConstObject.LoginDate;//业务时间
                curBillMaster.EngTaskName = ClientUtil.ToString(txtGWBSName.Text);
                curBillMaster.EngTaskId = txtGWBSName.Tag as GWBSTree;
                curBillMaster.EngTaskSyscode = curBillMaster.EngTaskId.SysCode;
                curBillMaster.CostType = txtCostType.Text;
                curBillMaster.ContractProfit = ClientUtil.ToDecimal(txtManageRace.Text);
                curBillMaster.ContractTotalIncome = ClientUtil.ToDecimal(txtOrderMoney.Text) * 10000;
                curBillMaster.ContractTotalIPay = ClientUtil.ToDecimal(txtPlanMoney.Text) * 10000;
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
                txtCode.Text = ClientUtil.ToString(curBillMaster.Code);
                this.txtOrderMoney.Text = ClientUtil.ToString(curBillMaster.ContractTotalIncome/10000);
                this.txtManageRace.Text = curBillMaster.ContractProfit+"";
                this.txtPlanMoney.Text = ClientUtil.ToString(curBillMaster.ContractTotalIPay/10000);
                this.txtCostType.SelectedItem = curBillMaster.CostType;
                this.txtHandlePerson.Tag = curBillMaster.HandlePerson;
                this.txtHandlePerson.Value = curBillMaster.HandlePersonName;
                this.txtProject.Text = curBillMaster.ProjectName;
                txtProject.Tag = curBillMaster.ProjectId;
                this.dtpCreateDate.Value = curBillMaster.RealOperationDate;
                this.txtGWBSName.Text = curBillMaster.EngTaskName;
                this.txtGWBSName.Tag = curBillMaster.EngTaskId;
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
        //    if (LoadTempleteFile(@"废旧物料处理申请单.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"废旧物料处理申请单.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"废旧物料处理申请单.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("废旧物料处理申请单【" + curBillMaster.Code + "】", false, false, true);
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

        //private void FillFlex(WasteMatApplyMaster billMaster)
        //{
        //    int detailStartRowNumber = 5;//5为模板中的行号
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
            
        //    flexGrid1.Cell(3, 2).Text = billMaster.Code;
            
        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        WasteMatApplyDetail detail = (WasteMatApplyDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;

        //        //计量单位
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;

        //        //数量
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

        //        //成色
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.Grade.ToString();

        //        //申请处理时间
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.ApplyDate.ToShortDateString();

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
        //    }
        //    flexGrid1.Cell(5 + detailCount, 2).Text = billMaster.ProjectName;
        //    flexGrid1.Cell(5 + detailCount, 5).Text = billMaster.CreateDate.ToShortDateString();
        //    flexGrid1.Cell(5 + detailCount,7).Text = billMaster.CreatePersonName;
        //}

    }
}
