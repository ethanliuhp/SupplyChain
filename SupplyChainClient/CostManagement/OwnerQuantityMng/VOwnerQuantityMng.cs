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
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    public partial class VOwnerQuantityMng : TMasterDetailView
    {
        private MOwnerQuantityMng model = new MOwnerQuantityMng();
        private OwnerQuantityMaster curBillMaster;
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        /// <summary>
        /// 当前单据
        /// </summary>
        public OwnerQuantityMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        public VOwnerQuantityMng()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
       
        private void InitData()
        {
            txtSign.Items.AddRange(new object[] { "已登帐", "未登帐" });
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
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
                    curBillMaster.Details.Remove(dr.Tag as OwnerQuantityDetail);
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
                    curBillMaster = model.OwnerQuantitySrv.GetOwnerQuantityById(Id);
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
                    this.txtSign.Enabled = true;
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.txtSign.Enabled = false;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtSubmitSumMoney, txtProject, txtConfirmSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colQWBS.Name,colQWBSCode.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
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
                this.curBillMaster = new OwnerQuantityMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
                this.ModelToView();
                //txtContractNo.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            if (!ViewToModel())
                return false;

            LogData log = new LogData();
            if (string.IsNullOrEmpty(curBillMaster.Id))
            {
                if (optrType == 2)
                {
                    log.OperType = "新增提交";
                }
                else
                {
                    log.OperType = "新增保存";
                }

            }
            else
            {
                if (optrType == 2)
                {
                    log.OperType = "修改提交";
                }
                else
                {
                    log.OperType = "修改保存";
                }
            }
            if (optrType == 2)
            {
                //curBillMaster.DocState = DocumentState.InAudit;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            }
            curBillMaster = model.OwnerQuantitySrv.SaveOwnerQuantity(curBillMaster);
            this.txtCode.Text = curBillMaster.Code;
            log.BillId = curBillMaster.Id;
            log.BillType = "业主报量单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);
            this.ViewCaption = ViewName + "-" + txtCode.Text;
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
                curBillMaster = model.OwnerQuantitySrv.GetOwnerQuantityById(curBillMaster.Id);
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
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                //curBillMaster = model.DemandPlanSrv.SaveDemandMasterPlan(curBillMaster, movedDtlList);
                curBillMaster = model.OwnerQuantitySrv.SaveOwnerQuantity(curBillMaster);
                
                txtCode.Text = curBillMaster.Code;
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "业主报量单";
                log.Code = curBillMaster.Code;
                log.OperType = "保存";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                StaticMethod.InsertLogData(log);
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
                this.txtCode.Focus();
                if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
                {
                    if (SaveOrSubmitBill(2) == false) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "业主报量单";
                    log.Code = curBillMaster.Code;
                    log.OperType = "提交";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
                    MessageBox.Show("提交成功！");
                    return true;
                }
                else
                {
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能提交！");
                    return false;
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
                curBillMaster = model.OwnerQuantitySrv.GetOwnerQuantityById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.OwnerQuantitySrv.DeleteByDao(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "月度盘点单";
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
                        curBillMaster = model.OwnerQuantitySrv.GetOwnerQuantityById(curBillMaster.Id);
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
                curBillMaster = model.OwnerQuantitySrv.GetOwnerQuantityById(curBillMaster.Id);
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
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.cmbQuantityType.Text) == "")
            {
                MessageBox.Show("报送类型必须选择！");
                cmbQuantityType.Focus();
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }

            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colQWBS.Name].Value == null)
                {
                    MessageBox.Show("清单WBS不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colQWBS.Name];
                    return false;
                }

                if (dr.Cells[colSubmitMoney.Name].Value == null)
                {
                    MessageBox.Show("报送金额不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colSubmitMoney.Name];
                    return false;
                }
                else
                {
                    if (Math.Abs(ClientUtil.ToDecimal(dr.Cells[colSubmitMoney.Name].Value)) > 100000)
                    {
                        MessageBox.Show("报送金额不能大于10亿！");
                        dgDetail.CurrentCell = dr.Cells[colSubmitMoney.Name];
                        return false;
                    }
                }
                if (Math.Abs(ClientUtil.ToDecimal(dr.Cells[colAcctGatheringMoney.Name].Value)) > 100000)
                {
                    MessageBox.Show("合同应收工程款不能大于10亿！");
                    dgDetail.CurrentCell = dr.Cells[colAcctGatheringMoney.Name];
                    return false;
                }
                if (dr.Cells[colConfirmMoney.Name].Value == null)
                {
                    MessageBox.Show("审定金额不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colConfirmMoney.Name];
                    return false;
                }
                else
                {
                    if (Math.Abs(ClientUtil.ToDecimal(dr.Cells[colConfirmMoney.Name].Value)) > 100000)
                    {
                        MessageBox.Show("审定金额不能大于10亿！");
                        dgDetail.CurrentCell = dr.Cells[colConfirmMoney.Name];
                        return false;
                    }
                }
                if (ClientUtil.ToDateTime(dr.Cells[this.colQuantityEndDate.Name].Value) < ClientUtil.ToDateTime(dr.Cells[this.colQuantityStartDate.Name].Value))
                {
                    MessageBox.Show("审量确认结束日期小于开始日期！");
                    dgDetail.CurrentCell = dr.Cells[colQuantityEndDate.Name];
                    return false;
                }
                if (ClientUtil.ToDateTime(dr.Cells[this.colConfirmDate.Name].Value) < ClientUtil.ToDateTime(dr.Cells[this.colQuantityDate.Name].Value))
                {
                    MessageBox.Show("审定日期小于报送日期！");
                    dgDetail.CurrentCell = dr.Cells[colConfirmDate.Name];
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
                curBillMaster.CreateDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AccountSign = ClientUtil.ToString(txtSign.SelectedItem);//登帐标志
                curBillMaster.QuantityType = ClientUtil.ToString(this.cmbQuantityType.SelectedItem);//报量类型
                curBillMaster.ConfirmSumMoney = ClientUtil.ToDecimal(txtConfirmSumMoney.Text) * 10000;//确认总金额
                curBillMaster.SubmitSumQuantity = ClientUtil.ToDecimal(txtSubmitSumMoney.Text) * 10000;//送报总金额
                curBillMaster.AuditManage = ClientUtil.ToString(txtAudit.Text);//项目报量审核情况
                curBillMaster.ProjectRecovery = ClientUtil.ToString(txtProjectRecovery.Text);//工程款回收情况
                curBillMaster.OwnerBreach = ClientUtil.ToString(txtOwnerBreach.Text);//业主违约情况
                curBillMaster.Descript = ClientUtil.ToString(txtRemark.Text);
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    OwnerQuantityDetail curBillDtl = new OwnerQuantityDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as OwnerQuantityDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.QWBS = var.Cells[colQWBS.Name].Tag as QWBSManage;//清单WBS
                    curBillDtl.QWBSCode = ClientUtil.ToString(var.Cells[colQWBSCode.Name].Value);//清单WBS层次码
                    curBillDtl.QWBSName = ClientUtil.ToString(var.Cells[colQWBS.Name].Value);//清单WBS名称
                    curBillDtl.QuantityDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colQuantityDate.Name].Value).ToShortDateString());//报送日期
                    curBillDtl.SubmitQuantity =  ClientUtil.ToDecimal(var.Cells[colSubmitMoney.Name].Value) * 10000;//报送金额
                    curBillDtl.ConfirmDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colConfirmDate.Name].Value).ToShortDateString());//审定日期
                    curBillDtl.ConfirmMoney = ClientUtil.ToDecimal(var.Cells[colConfirmMoney.Name].Value) * 10000;//审定金额
                    curBillDtl.ConfirmStartDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colQuantityStartDate.Name].Value).ToShortDateString());//报量开始日期
                    curBillDtl.ConfirmEndDate = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[this.colQuantityEndDate.Name].Value).ToShortDateString());//报量结束日期
                    curBillDtl.AcctGatheringMoney = ClientUtil.ToDecimal(var.Cells[this.colAcctGatheringMoney.Name].Value) * 10000;//合同应收工程款
                    curBillDtl.GatheringRate = ClientUtil.ToDecimal(var.Cells[this.colGatheringRate.Name].Value) / 100;//合同对应收款比例
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[this.colDescript.Name].Value);//情况说明
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

      

        /// <summary>
        /// 清单WBS列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colQWBS.Name))
            {
                if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
                {
                    VQWBSSelect frm = new VQWBSSelect(new MQWBSManagement());
                    frm.ShowDialog();
                    if (frm.SelectResult.Count > 0)
                    {
                        TreeNode root = frm.SelectResult[0];

                        QWBSManage task = root.Tag as QWBSManage;
                        if (task != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colQWBS.Name].Tag = task;
                            this.dgDetail.CurrentRow.Cells[colQWBS.Name].Value = task.TaskName;
                            this.dgDetail.CurrentRow.Cells[colQWBSCode.Name].Value = task.SysCode;
                        }
                    }
                    txtCode.Focus();
                }
            }
            
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                if (curBillMaster.CreateDate > ClientUtil.ToDateTime("2000-01-01"))
                {
                    dtpDateBegin.Value = curBillMaster.RealOperationDate;
                }
                txtSubmitSumMoney.Text = ClientUtil.ToString(curBillMaster.SubmitSumQuantity / 10000);
                curBillMaster.Temp1 = ClientUtil.ToString(curBillMaster.SubmitSumQuantity);//报送金额
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtConfirmSumMoney.Text = ClientUtil.ToString(curBillMaster.ConfirmSumMoney / 10000);
                curBillMaster.Temp4 = ClientUtil.ToString(curBillMaster.ConfirmSumMoney);//确认金额
                this.cmbQuantityType.SelectedItem = curBillMaster.QuantityType;//报量类型
                txtProject.Text = curBillMaster.ProjectName.ToString();
                txtOwnerBreach.Text = curBillMaster.OwnerBreach;
                txtAudit.Text = curBillMaster.AuditManage;
                txtProjectRecovery.Text = curBillMaster.ProjectRecovery;
                txtRemark.Text = curBillMaster.Descript;
                txtSign.SelectedItem = ClientUtil.ToString(curBillMaster.AccountSign);//登帐标志
                this.dgDetail.Rows.Clear();
                foreach (OwnerQuantityDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colQWBS.Name, i].Tag = var.QWBS;
                    this.dgDetail[colQWBS.Name, i].Value = var.QWBSName;
                    this.dgDetail[colQWBSCode.Name, i].Value = var.QWBSCode;
                    this.dgDetail[colQuantityDate.Name, i].Value = var.QuantityDate;
                    this.dgDetail[colSubmitMoney.Name, i].Value = var.SubmitQuantity / 10000;
                    this.dgDetail[colConfirmDate.Name, i].Value = var.ConfirmDate;
                    this.dgDetail[colConfirmMoney.Name, i].Value = var.ConfirmMoney / 10000;
                    this.dgDetail[colQuantityStartDate.Name, i].Value = var.ConfirmStartDate;
                    this.dgDetail[colQuantityEndDate.Name, i].Value = var.ConfirmEndDate;
                    this.dgDetail[colAcctGatheringMoney.Name, i].Value = var.AcctGatheringMoney / 10000;
                    this.dgDetail[colGatheringRate.Name, i].Value = var.GatheringRate * 100;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            bool flag = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;

            if (dgDetail.Rows[e.RowIndex].Cells[colSubmitMoney.Name].Value != null)
            {
                string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colSubmitMoney.Name].Value.ToString();
                validity = CommonMethod.VeryValid(temp_quantity);
                if (validity == false)
                {
                    MessageBox.Show("报送金额为数字！");
                    this.dgDetail.Rows[e.RowIndex].Cells[colSubmitMoney.Name].Value = "";
                    flag = false;
                }
                decimal summoney = 0;
                if (flag)
                {
                    decimal money = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colSubmitMoney.Name].Value);
                        summoney += money;
                    }
                }
                txtSubmitSumMoney.Text = ClientUtil.ToString(summoney);
            }

            if (dgDetail.Rows[e.RowIndex].Cells[colConfirmMoney.Name].Value != null)
            {
                string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colConfirmMoney.Name].Value.ToString();
                validity = CommonMethod.VeryValid(temp_quantity);
                if (validity == false)
                {
                    MessageBox.Show("确认金额为数字！");
                    this.dgDetail.Rows[e.RowIndex].Cells[colConfirmMoney.Name].Value = "";
                    flag = false;
                }
                decimal summoney = 0;
                if (flag)
                {
                    decimal money = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colConfirmMoney.Name].Value);
                        summoney += money;
                    }
                }
                txtConfirmSumMoney.Text = ClientUtil.ToString(summoney);
            }     
        }
    }
}
