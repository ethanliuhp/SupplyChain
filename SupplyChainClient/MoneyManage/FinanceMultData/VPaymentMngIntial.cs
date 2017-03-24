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
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Client.PLMWebServices;
using NPOI.SS.UserModel;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using NHibernate;
 

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VPaymentMngIntial : TBasicDataView
    {
        private MFinanceMultData model = new MFinanceMultData();
        IList lstProjectInfo = null;
        IList lstAccountTitle = null;
        IList lstSupInfo = null;
 
        public int iMaxCount = 500;
        public IList<string> lstColumnName = new List<string>();
        public int RowCount = 0;
        CPaymentMng_ExecType _execType;
        public VPaymentMngIntial(CPaymentMng_ExecType _execType)
        {
            InitializeComponent();
            this._execType = _execType;
            IntialData();
            IntialEvent();
        }
        public void IntialData()
        {
            lstColumnName.Add("项目名称");
            lstColumnName.Add("付款类型");
            lstColumnName.Add("付款时间");
            lstColumnName.Add("分供商单位");
            lstColumnName.Add("金额");
            lstColumnName.Add("制单人");
            lstProjectInfo = model.FinanceMultDataSrv.GetProjectInfo();
            lstAccountTitle = this.GetAccountTitle();
            lstSupInfo = model.FinanceMultDataSrv.QuerySupInfo();
            IntialFlexGrid();
        }
     
        public void IntialFlexGrid()
        {
           
            flexGrid.Rows = RowCount+1;
            flexGrid.Cols = lstColumnName.Count + 1;
            for (int iCol = 1; iCol < flexGrid.Cols; iCol++)
            {
                flexGrid.Cell(0, iCol).Text = lstColumnName[iCol - 1];
                flexGrid.Column(iCol).Width = 120;
            }

        }
        public void IntialEvent()
        {
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnInsertRows.Click += new EventHandler(btnInsertRows_Click);
            btnSure.Click += new EventHandler(btnSure_Click);
            btnCancel1.Click += new EventHandler(btnCancel);
        }

        public void btnInsertRows_Click(object sender, System.EventArgs e)
        {
            int iRowCount = 0;
            try
            {
                iRowCount = int.Parse(txtRowCount.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.txtRowCount.Focus();
                return;
            }
            if (iRowCount <= 0)
            {
                MessageBox.Show("无法添加;[添加的行数大于零]");
                this.txtRowCount.Focus();
                return;
            }
            if (flexGrid.Rows == iMaxCount + 1)
            {
                MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,已经添加了{0}条。]", iMaxCount));
                this.txtRowCount.Focus();
                return;
            }
            if (flexGrid.Rows + iRowCount > iMaxCount + 1)
            {
                MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,此次只能添加{1}条记录。]", iMaxCount, iMaxCount + 1 - flexGrid.Rows));
                this.txtRowCount.Focus();
                return;
            }
            for (int i = 0; i < iRowCount; i++)
            {
                btnAddRow();
            }
        }

        public void btnCancel(object sender, System.EventArgs e)
        {
            this.Close();
        }
        public void btnAddRow()
        {
            flexGrid.AutoRedraw = false;

            flexGrid.InsertRow(flexGrid.Rows, 1);
            flexGrid.Rows += 1;
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        public void btnDeleteRow(int iRow)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.RemoveItem(iRow);
            //flexGrid.Rows -= 1;

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        public void btnDelete_Click(object sender, System.EventArgs e)
        {
            btnDeleteRow(flexGrid.Selection.FirstRow);
        }
        public void btnAdd_Click(object sender, System.EventArgs e)
        {
            btnAddRow();
        }
        public bool IsEmpty(int iRow)
        {
            bool bFlag = true;
            if (flexGrid.Cols > 1)
            {
                for (int i = 1; i < flexGrid.Cols; i++)
                {
                    if (!string.IsNullOrEmpty(flexGrid.Cell(iRow, i).Text.Trim()))
                    {
                        bFlag = false;
                        break;
                    }
                }
            }

            return bFlag;
        }

        public void btnSure_Click(object sender, System.EventArgs e)
        {
            
            string sValue = string.Empty;
            string sError = string.Empty;
            try
            {
                if (flexGrid.Rows > 1)
                {
                    if (MessageBox.Show("确定导入?", "提示",MessageBoxButtons.OKCancel)==DialogResult.OK)
                    {
                        FlashScreen.Show("正在导入中....");
                        IList lstMaster = CreateBills();
                        if (lstMaster != null && lstMaster.Count > 0)
                        {
                            if (this.ValidateData(lstMaster))
                            {
                                lstMaster = model.FinanceMultDataSrv.SavePaymentMaster(lstMaster);
                                PaymentMaster curBillMaster = lstMaster[0] as PaymentMaster;
                                StaticMethod.InsertLogData(curBillMaster.Id, "批量导入付款单", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, this._execType.ToString(), "", string.IsNullOrEmpty(curBillMaster.ProjectName) ? "" : curBillMaster.ProjectName);
                                                
                                MessageBox.Show("保存成功！");
                                FlashScreen.Close();
                            }
                            else
                            {
                                throw new Exception("该单据数据已经导入过！");
                            }
                        }
                        else
                        {
                           
                            MessageBox.Show("保存失败:请输入付款数据");
                            FlashScreen.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("保存失败:请输入收款单数据");
                }
               
            }
            catch (Exception ex)
            {
               
                MessageBox.Show("保存失败:"+ex.Message);
                FlashScreen.Close();
                return;
            }
           
           

        }
        public bool ValidateData(IList lstMaster)
        {
            bool bResult=false;
            if (lstMaster != null && lstMaster.Count > 0)
            {
                foreach (PaymentMaster pm in lstMaster)
                {
                    if (!CheckAccountLock(pm.CreateDate.Date))
                    {
                        return false;
                    }
                }

                PaymentMaster oMaster = lstMaster[0] as PaymentMaster;
                ObjectQuery oQuery = new ObjectQuery();
                oQuery.AddCriterion(Expression.Eq("ProjectId",oMaster.ProjectId));
                oQuery.AddCriterion(Expression.Eq("AccountTitleID", oMaster.AccountTitleID));
                oQuery.AddCriterion(Expression.Eq("CreateDate", oMaster.CreateDate));
                oQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo", oMaster.TheSupplierRelationInfo));
                oQuery.AddCriterion(Expression.Eq("SumMoney", oMaster.SumMoney));
                  oQuery.AddCriterion(Expression.Eq("Descript", oMaster.Descript));
                oQuery.AddFetchMode("Detail", FetchMode.Eager);
                IList lstPaymentMaster = model.FinanceMultDataSrv.Query(typeof(PaymentMaster), oQuery);
                if (lstPaymentMaster != null && lstPaymentMaster.Count > 0)
                {
                    bResult = false;
                }
                else
                {
                    bResult = true;
                }
            }
            return bResult;
        }
        private IList CreateBills()
        {
            IList lstMaster = new ArrayList();

            //lstMaster.Clear();
            PaymentMaster oMaster = null;
            AccountTitleTree oAccount = null;
            OperationOrgInfo oOperationOrgInfo = null;
            PersonInfo oPersonInfo = null;
            DateTime dtPayountDate;
            DataDomain dataProject = null;
            DataDomain dataSupInfo = null;
            string sProjectName = string.Empty;
            string sAccountType = string.Empty;
            string sSupInfoName = string.Empty;
            string sUserCode = string.Empty;
            string sMessage = string.Empty;
            decimal dMoney = 0;
            bool bFlag = false;
            FlexCell.Row oRow = null;


            for (int iStartRow = 1; iStartRow < flexGrid.Rows; iStartRow++)
            {
                bFlag = false;
                try
                {

                    sProjectName = flexGrid.Cell(iStartRow, 1).Text.Trim();
                    sAccountType = flexGrid.Cell(iStartRow, 2).Text.Trim();
                    dtPayountDate = ClientUtil.ToDateTime(flexGrid.Cell(iStartRow, 3).Text.Trim());
                    sSupInfoName = flexGrid.Cell(iStartRow, 4).Text.Trim();
                    dMoney = ClientUtil.ToDecimal(flexGrid.Cell(iStartRow, 5).Text.Trim());
                    sUserCode = flexGrid.Cell(iStartRow, 6).Text.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(sProjectName))
                    {
                        oMaster = NewMaster();
                        dataProject = lstProjectInfo == null ? null : lstProjectInfo.OfType<DataDomain>().FirstOrDefault(a => a.Name5.ToString() == sProjectName);
                        if (dataProject != null)
                        {
                            //项目信息
                            oMaster.ProjectId = dataProject.Name4.ToString();
                            oMaster.ProjectName = dataProject.Name5.ToString();
                            //组织信息
                            oOperationOrgInfo = GetOperationOrgInfo(dataProject.Name1.ToString());
                            if (oOperationOrgInfo != null)
                            {
                                oMaster.OperOrgInfo = oOperationOrgInfo;
                                oMaster.OperOrgInfoName = oOperationOrgInfo.Name;
                                oMaster.OpgSysCode = oOperationOrgInfo.SysCode;
                            }
                            else
                            {
                                throw new Exception(string.Format("根据第[{0}行]的[{1}]项目名称无法找到。", iStartRow, sProjectName));
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("根据第[{0}行]的[{1}]项目名称无法找到。", iStartRow, sProjectName));
                        }
                        oAccount = lstAccountTitle == null ? null : lstAccountTitle.OfType<AccountTitleTree>().FirstOrDefault(a => a.Name == sAccountType);
                        if (oAccount != null)
                        {
                            oMaster.AccountTitleCode = oAccount.Code;
                            oMaster.AccountTitleID = oAccount.Id;
                            oMaster.AccountTitleName = oAccount.Name;
                            oMaster.AccountTitleSyscode = oAccount.SysCode;
                        }
                        else
                        {
                            throw new Exception(string.Format("根据第[{0}行]的[{1}]付款类型无法找到。", iStartRow, sAccountType));
                        }
                        dataSupInfo = lstSupInfo == null ? null : lstSupInfo.OfType<DataDomain>().FirstOrDefault(a => a.Name4.ToString() == sSupInfoName);
                        if (dataSupInfo != null)
                        {
                            oMaster.TheSupplierName = dataSupInfo.Name4.ToString();
                            oMaster.TheSupplierRelationInfo = dataSupInfo.Name2.ToString();
                            oMaster.BankAccountNo = dataSupInfo.Name5.ToString();
                            oMaster.BankAddress = dataSupInfo.Name7.ToString();
                            oMaster.BankName = dataSupInfo.Name6.ToString();
                        }
                        else
                        {
                            throw new Exception(string.Format("根据第[{0}行]的[{1}]供应商单位名称无法找到。", iStartRow, sSupInfoName));
                        }
                        oPersonInfo = GetPersonInfo(sUserCode);
                        if (oPersonInfo != null)
                        {
                            oMaster.HandlePerson = oPersonInfo;
                            oMaster.HandlePersonName = oPersonInfo.Name;
                            oMaster.CreatePerson = oPersonInfo;
                            oMaster.CreatePersonName = oPersonInfo.Name;
                        }
                        else
                        {
                            throw new Exception(string.Format("根据第[{0}行]的[{1}]登陆账户无法找到。", iStartRow, sUserCode));
                        }
                        oMaster.CreateDate = dtPayountDate;
                        oMaster.Details.Add(new PaymentDetail() { Money = dMoney, PaymentMoney = dMoney, Master = oMaster });
                        lstMaster.Insert(lstMaster.Count,oMaster);
                    }
                    else
                    {
                        break;
                        // throw new Exception(string.Format("第[{0}]的[项目名称]不能为空",iStartRow));
                    }
                }
                catch (Exception ex)
                {
                    sMessage += ex.Message + "\n";
                }
            }
            if (!string.IsNullOrEmpty(sMessage))
            {
                throw new Exception(sMessage);
            }
            return lstMaster;
        }
     
        
        private PaymentMaster NewMaster()
        {

            PaymentMaster curBillMaster = new PaymentMaster();
           // curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
            //curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.CreateYear = ConstObject.LoginDate.Year;
            curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
           // curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
           // curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
           // curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
           // curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
           // curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
           // curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.DocState = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute;
            curBillMaster.CreateDate = DateTime.Now;
            curBillMaster.IfProjectMoney = 0;
            curBillMaster.Descript = "数据初始化导入";
            return curBillMaster;
        }
        public IList GetAccountTitle()
        {
            MAccountTitle titleModel = new MAccountTitle();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(  Expression.Or(Expression.Eq("BusinessFlag", "02"), Expression.Eq("BusinessFlag", "01")));
            oq.AddCriterion(Expression.In("Name", new ArrayList() { 
                "应付劳务款","应付工程款","应付购货款","应付租赁费",
            }));//"投标保证金","履约保证金","安全保证金","质量保证金","诚信保证金","其他保证金","预付款保证金" 
           IList acctTitleList = titleModel.AccountTitleTreeSvr.GetAccountTitleTreeByQuery(oq);
           return acctTitleList;
        }
        public OperationOrgInfo GetOperationOrgInfo(string sID)
        {
            OperationOrgInfo oOperationOrgInfo = null;
            oOperationOrgInfo = model.OrganizationResSrv.GetOperationOrg(sID);
            return oOperationOrgInfo;
        }
        public PersonInfo GetPersonInfo(string sCode)
        {
            PersonInfo oPersonInfo = null;
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Code",sCode));
            IList lst = model.PersonManager.GetPersonInfo(oQuery);
            oPersonInfo = lst==null || lst.Count == 0 ? null : lst[0] as PersonInfo;
            return oPersonInfo;
        }

        private bool CheckAccountLock(DateTime businessDate)
        {
            var projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                var errorMes = model.FinanceMultDataSrv.IsAllowBusinessHappend(projectInfo.Id, businessDate);
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
