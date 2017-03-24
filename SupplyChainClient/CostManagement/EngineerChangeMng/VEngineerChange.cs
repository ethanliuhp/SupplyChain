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
using Application.Business.Erp.SupplyChain.CostManagement.EngineerChangeMng.Domain;
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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng
{
    public partial class VEngineerChange : TMasterDetailView
    {
        private MEngineerChangeMng model = new MEngineerChangeMng();
        public MWBSContractGroup modelWBS;
        private EngineerChangeMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        
        /// <summary>
        /// 当前单据
        /// </summary>
        public EngineerChangeMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VEngineerChange()
        {
            InitializeComponent();
            InitEvent();
        }

        private void InitEvent()
        {
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);

        }

        void dgDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            object _DateControl = dgDetail.Controls["DataGridViewCalendarColumn"];
            if (_DateControl == null) return;
            DateTimePicker _DateTimePicker = (DateTimePicker)_DateControl;


            if (e.ColumnIndex == 6)//修改想要的列
            {
                Rectangle _Rectangle = dgDetail.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _DateTimePicker.Size = new Size(_Rectangle.Width, _Rectangle.Height);
                _DateTimePicker.Location = new Point(_Rectangle.X, _Rectangle.Y);
                _DateTimePicker.Visible = true;
            }
            else
            {
                _DateTimePicker.Visible = false;
            }
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as EngineerChangeDetail);
                }
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
                    curBillMaster = model.EngineerChangeSrv.GetEngineerChangeById(Id);
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
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtProject,txtContractGroup};
            ObjectLock.Lock(os);
            //string[] lockCols = new string[] { };
            //dgDetail.SetColumnsReadOnly(lockCols);
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
                this.curBillMaster = new EngineerChangeMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.CreateDate = ConstObject.LoginDate;//制单时间
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//登录业务组织
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录组织名称
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;//业务组织编号
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                curBillMaster.DocState = DocumentState.Edit;//状态

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
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
            movedDtlList = new ArrayList();
            //base.ModifyView();
            //curBillMaster = model.WasteMatSrv.GetWasteMatHandleById(curBillMaster.Id);
            //ModelToView();
            //return true;
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.EngineerChangeSrv.GetEngineerChangeById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
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
                this.txtCode.Focus();
                curBillMaster = model.EngineerChangeSrv.SaveEngineerChange(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //txtSumMoney.Text = curBillMaster.SumMoney;
                //更新Caption
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
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                //if (!ViewToModel()) return false;
                //curBillMaster.DocState = DocumentState.InExecute;
                //curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                //curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                //curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                //curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                //curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                //curBillMaster = model.WasteMatSrv.saveWasteMatProcess(curBillMaster, movedDtlList);
                //txtCode.Text = curBillMaster.Code;
                ////更新Caption
                //this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                curBillMaster = model.EngineerChangeSrv.GetEngineerChangeById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.EngineerChangeSrv.DeleteByDao(curBillMaster)) return false;
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
                        curBillMaster = model.EngineerChangeSrv.GetEngineerChangeById(curBillMaster.Id);
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
                curBillMaster = model.EngineerChangeSrv.GetEngineerChangeById(curBillMaster.Id);
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
            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (dr.Cells[colChangeDescript.Name].Value == null)
                {
                    MessageBox.Show("更改说明不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colChangeDescript.Name];
                    return false;
                }
                if (dr.Cells[colHandlePerson.Name].Value == null)
                {
                    MessageBox.Show("更改负责人不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colHandlePerson.Name];
                    return false;
                }
                if (dr.Cells[colEngineerChangeLink.Name].Value == null)
                {
                    MessageBox.Show("工程更改环节不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colEngineerChangeLink.Name];
                    return false;
                }
            }
            dgDetail.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                this.txtCode.Focus();
                curBillMaster.ContractGroup = this.txtContractGroup.Tag as ContractGroup;
                //curBillMaster.ContractGroup = ClientUtil.ToString(this.txtContractGroup.Tag);
                //curBillMaster.ContractGroup = ClientUtil.ToString(this.txtContractGroup.Tag);
                curBillMaster.ContractGroupName = ClientUtil.ToString(this.txtContractGroup.Text);
                curBillMaster.RealOperationDate = dtpDateBegin.Value.Date;
                curBillMaster.Descript = ClientUtil.ToString(txtDescript.Text);
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    EngineerChangeDetail curBillDtl = new EngineerChangeDetail();

                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as EngineerChangeDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.ChangeHandlePerson = var.Cells[colHandlePerson.Name].Tag as PersonInfo;
                    curBillDtl.ChangeHandlePersonName = ClientUtil.ToString(var.Cells[colHandlePerson.Name].Value);
                    curBillDtl.EngineerChangeLink = ClientUtil.ToString(var.Cells[colEngineerChangeLink.Name].Value);
                    curBillDtl.ChangeDescript = ClientUtil.ToString(var.Cells[colChangeDescript.Name].Value);
                    curBillDtl.ComplateDate = Convert.ToDateTime(var.Cells[colChangeDate.Name].Value);
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }


        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                dtpDateBegin.Value = curBillMaster.RealOperationDate;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtDescript.Text = curBillMaster.Descript;
                txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtProject.Text = curBillMaster.ProjectName;
                txtContractGroup.Text = curBillMaster.ContractGroupName;
                txtDescript.Text = curBillMaster.Descript;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                this.dgDetail.Rows.Clear();
                foreach (EngineerChangeDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[colEngineerChangeLink.Name, i].Value = var.EngineerChangeLink;
                    this.dgDetail[colHandlePerson.Name, i].Value = var.ChangeHandlePersonName;
                    this.dgDetail[colChangeDate.Name, i].Value = var.ComplateDate;
                    this.dgDetail[colChangeDescript.Name, i].Value = var.ChangeDescript;
                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

       
        public void btnSearch_Click(object sender, EventArgs e)
        {

            VSelectWBSContractGroup vmros = new VSelectWBSContractGroup(new MWBSContractGroup());
            vmros.ShowDialog();
            IList list = vmros.SelectResult;
            if (list == null || list.Count == 0) return;
            ContractGroup engineerMaster = list[0] as ContractGroup;
            txtContractGroup.Tag = engineerMaster.Id;
            txtContractGroup.Text = engineerMaster.Code;
        }

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\废旧物料处理信息打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\废旧物料处理信息打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\废旧物料处理信息打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("废旧物料申请信息【" + curBillMaster.Code + "】", false, false, true);
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

        //private void FillFlex(WasteMatProcessMaster billMaster)
        //{
        //    int detailStartRowNumber = 6;//6为模板中的行号
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

        //    flexGrid1.Cell(2, 1).Text = "回收单位：" + billMaster.RecycleUnitName;

        //    flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
        //    flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
        //    flexGrid1.Cell(4, 2).Text = billMaster.Code;
        //    flexGrid1.Cell(4, 5).Text = billMaster.CreateDate.ToString();
        //    //flexGrid1.Cell(4, 7).Text = billMaster.CreatePerson.ToString();


        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        WasteMatProcessDetail detail = (WasteMatProcessDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(2, 1).Text = "单据号：" + detail.MaterialResource.Id;
        //        string a1 = detail.MaterialResource.CreatedDate.ToString();
        //        string[] sArray1 = a1.Split(' ');
        //        string stra1 = sArray1[0];
        //        flexGrid1.Cell(4, 7).Text = stra1;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //皮重
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.TareWeight.ToString();

        //        //毛重
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.GrossWeight.ToString();

        //        //净重
        //        //string a = detail.ApplyDate.ToString();
        //        //string[] sArray = a.Split(' ');
        //        //string stra = sArray[0];
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.NetWeight.ToString();

        //        //单价
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = detail.ProcessPrice.ToString();

        //        //金额
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Money.ToString();
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //车牌号
        //        flexGrid1.Cell(detailStartRowNumber + i, 8).Text = detail.PlateNumber.ToString();

        //        //收据号
        //        flexGrid1.Cell(detailStartRowNumber + i, 9).Text = detail.ReceiptCode.ToString();
        //    }
        //}
    }
}
