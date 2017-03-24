using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VFactoringData : TMasterDetailView
    {
        private MFinanceMultData service = new MFinanceMultData();
        public FactoringDataMaster Master { get; set; }
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        private Control[] lockClts;

        public VFactoringData()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            lockClts = new Control[] { txtRemark, dgDetail ,this.dtpDateBegin};
        }

        private void InitEvent()
        {
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }

        private void tsmiDel_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null || dr.IsNewRow) return;
            dgDetail.Rows.Remove(dr);
            if (dr.Tag != null)
            {
                Master.Details.Remove(dr.Tag as BaseDetail);
                movedDtlList.Add(dr.Tag as BaseDetail);
            }

        }
        /// <summary>
        /// 编辑完成后验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == this.Balance.Name || colName == this.Rate.Name || colName == this.AmountPayable.Name || colName == this.TotalDay.Name)
            {
                string temp_quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                bool validity = CommonMethod.VeryValid(temp_quantity);
                if (validity == false)
                {
                    MessageBox.Show("请输入数字！");
                    this.dgDetail.Rows[e.RowIndex].Cells[colName].Value = "";
                    dgDetail.Rows[e.RowIndex].Cells[colName].Selected = true;
                    dgDetail.BeginEdit(false);
                }
                if (colName == Balance.Name)
                {
                    decimal sumMoney = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        sumMoney += ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[Balance.Name].Value);
                    }
                    txtTotal.Text = sumMoney.ToString("N0");
                }
            }
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string strID)
        {
            try
            {
                if (string.IsNullOrEmpty(strID) || string.Equals(strID, "空"))
                    RefreshState(MainViewState.Initialize);
                else
                {
                    Master = service.FinanceMultDataSrv.GetFactoringDataById(strID);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreateName.Tag as PersonInfo;
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
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:

                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:

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

                ObjectLock.Unlock(lockClts);
            }
            else
            {
                ObjectLock.Lock(lockClts);
            }
            //永久锁定
            object[] os = new object[] { txtCode, txtCreateName, txtCreateDate, TotalDay, txtTotal };
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
            else if (c is ComboBox)
            {
                c.Text = "";
            }
            else if (c is TextBox)
            {
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
                movedDtlList.Clear();
                Master = new FactoringDataMaster();
                Master.CreatePerson = ConstObject.LoginPersonInfo;
                Master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                //Master.CreateDate = ConstObject.LoginDate;
                Master.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                Master.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                Master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                Master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                Master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                Master.DocState = DocumentState.Edit;

                //制单人
                txtCreateName.Tag = ConstObject.LoginPersonInfo;
                txtCreateName.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
                {
                    Master.ProjectId = projectInfo.Id;
                    Master.ProjectName = projectInfo.Name;
                }
                this.ModelToView();
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
            if (Master.DocState == DocumentState.Edit || Master.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                Master = service.FinanceMultDataSrv.GetFactoringDataById(Master.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(Master.DocState));
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
                if (Master.Id == null)
                {
                    Master.DocState = DocumentState.InExecute;
                    Master = service.FinanceMultDataSrv.Save(Master) as FactoringDataMaster;
                    //插入日志
                    StaticMethod.InsertLogData(Master.Id, "新增", Master.Code, ConstObject.LoginPersonInfo.Name, "aaa", "", Master.ProjectName + string.Empty);
                }
                else
                {
                    if (movedDtlList != null && movedDtlList.Count > 0) //service.FinanceMultDataSrv.Delete(movedDtlList);
                    Master=service.FinanceMultDataSrv.Update(Master) as FactoringDataMaster;
                    //插入日志
                    StaticMethod.InsertLogData(Master.Id, "修改", Master.Code, ConstObject.LoginPersonInfo.Name, "bbb", "", Master.ProjectName + string.Empty);
                }
                txtCode.Text = Master.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                ModelToView();

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
                if (Master.DocState == DocumentState.Valid || Master.DocState == DocumentState.Edit)
                {
                    service.FinanceMultDataSrv.DeleteFinanceMultDataMaster(Master);
                    //插入日志
                    StaticMethod.InsertLogData(Master.Id, "删除", Master.Code + string.Empty, ConstObject.LoginPersonInfo.Name, "ccc", "", Master.ProjectName + string.Empty);
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(Master.DocState) + "】，不能删除！");
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
                        Master = service.FinanceMultDataSrv.GetFactoringDataById(Master.Id);
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
                Master = service.FinanceMultDataSrv.GetFactoringDataById(Master.Id);
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

            return CheckAccountLock(dtpDateBegin.Value.Date);
        }

        //保存数据
        private bool ViewToModel()
        {
            try
            {
                if (!ValidView()) return false;
                Master.CreateDate = dtpDateBegin.Value.Date;
                Master.CreateYear = Master.CreateDate.Year;
                Master.CreateMonth = Master.CreateDate.Month;
                Master.Descript = txtRemark.textBox.Text;
                Master.Details.Clear();
                foreach (DataGridViewRow item in this.dgDetail.Rows)
                {
                    if (item.IsNewRow) break;
                    FactoringDataDetail detail = new FactoringDataDetail();
                    if (item.Tag != null)
                    {
                        detail = item.Tag as FactoringDataDetail;
                        if (detail.Id == null)
                        {
                            Master.Details.Remove(detail);
                        }
                    }

                    detail.DepartmentName = item.Cells[DepartmentName.Name].Value + string.Empty;
                    detail.ProjectName = item.Cells[ProjectName.Name].Value + string.Empty;
                    detail.BankName = item.Cells[BankName.Name].Value + string.Empty;
                    detail.Balance = Convert.ToDecimal(item.Cells[Balance.Name].Value);
                    detail.Rate = Convert.ToDecimal(item.Cells[Rate.Name].Value);
                    detail.StartDate = Convert.ToDateTime(Convert.ToDateTime(item.Cells[StartDate.Name].Value).ToShortDateString());
                    detail.EndDate = Convert.ToDateTime(Convert.ToDateTime(item.Cells[EndDate.Name].Value).ToShortDateString());
                    detail.PayType = item.Cells[PayType.Name].Value + string.Empty;
                    detail.StartChargingDate = Convert.ToDateTime(Convert.ToDateTime(item.Cells[StartChargingDate.Name].Value).ToShortDateString());
                    detail.EndChargingDate = Convert.ToDateTime(Convert.ToDateTime(item.Cells[EndChargingDate.Name].Value).ToShortDateString());
                    detail.TotalDay = ClientUtil.ToInt(item.Cells[this.TotalDay.Name].Value); //(detail.EndChargingDate - detail.StartChargingDate).Days;
                    detail.AmountPayable = Convert.ToDecimal(item.Cells[AmountPayable.Name].Value);
                    detail.Money = detail.Balance;
                    Master.AddDetail(detail);
                    item.Tag = detail;
                }

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
            txtCode.Text = Master.Code;
            txtCreateName.Text = Master.CreatePersonName;
            if (Master.CreateDate > ClientUtil.ToDateTime("2000-01-01"))
            {
                dtpDateBegin.Value = Master.CreateDate;
            }
            txtCreateDate.Text = Master.RealOperationDate.ToShortDateString();
            txtRemark.textBox.Text = Master.Descript;
            dgDetail.Rows.Clear();
            foreach (FactoringDataDetail item in Master.Details)
            {
                int i = this.dgDetail.Rows.Add();

                dgDetail[DepartmentName.Name, i].Value = item.DepartmentName;
                dgDetail[ProjectName.Name, i].Value = item.ProjectName;
                dgDetail[BankName.Name, i].Value = item.BankName;
                dgDetail[Balance.Name, i].Value = item.Balance;
                dgDetail[Rate.Name, i].Value = item.Rate;
                dgDetail[StartDate.Name, i].Value = item.StartDate;
                dgDetail[EndDate.Name, i].Value = item.EndDate;
                dgDetail[PayType.Name, i].Value = item.PayType;
                dgDetail[StartChargingDate.Name, i].Value = item.StartChargingDate;
                dgDetail[EndChargingDate.Name, i].Value = item.EndChargingDate;
                dgDetail[TotalDay.Name, i].Value = item.TotalDay;
                dgDetail[AmountPayable.Name, i].Value = item.AmountPayable;
                dgDetail.Rows[i].Tag = item;
                //sumMoney += item.AmountPayable;
            }
            txtTotal.textBox.Text = Master.SumMoney.ToString("N0");//sumMoney.ToString("N0");
            return true;
        }
        #region 打印处理
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {

            return true;
        }

        public override bool Print()
        {
            return true;
        }

        public override bool Export()
        {
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            return true;
        }
        #endregion
        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                this.txtCode.Focus();
                if (!ViewToModel()) return false;
                Master.DocState = DocumentState.InExecute;

                if (string.IsNullOrEmpty(Master.Code))
                {
                   Master= service.FinanceMultDataSrv.Save(Master) as FactoringDataMaster;
                }
                else
                {
                    Master = service.FinanceMultDataSrv.Update(Master) as FactoringDataMaster;
                }
                LogData log = new LogData();
                log.BillId = Master.Id;
                log.BillType = this.Title;
                log.Code = Master.Code;
                log.OperType = "提交";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = Master.ProjectName;
                StaticMethod.InsertLogData(log);
                txtCode.Text = Master.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                Master.DocState = DocumentState.Edit;
            }
            return false;
        }

        private bool CheckAccountLock(DateTime businessDate)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                var errorMes = new FinanceMultData.MFinanceMultData().FinanceMultDataSrv.IsAllowBusinessHappend(projectInfo.Id, businessDate);
                if (string.IsNullOrEmpty(errorMes))
                {
                    return true;
                }

                MessageBox.Show(errorMes);
                return false;
            }

            return true;
        }
    }
}

