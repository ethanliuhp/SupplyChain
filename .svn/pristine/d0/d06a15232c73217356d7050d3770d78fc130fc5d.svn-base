using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.IO;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VProjectBalanceOfPaymentsAnalysisRpt : TBasicDataView
    {
        OperationOrgInfo SelectOperationOrgInfo = null;
        ProjectBalanceOfPayment currProjectBalanceOfPayment = null;
        private string rptName = "工程项目收支分析表";
        bool IsProject = false;
        private CurrentProjectInfo projectInfo;
        private FinanceMultDataExecType _exeType;
        private MFinanceMultData model;
        private bool IsUpdate = false;
        private string[] arrProjectType = { "房建(非地产开发)", "房建(地产开发)", "公建", "基础设施", "其他" };
        private string[] arrOwerType = { "外企", "政府", "事业", "民企（上市）", "民企（非上市）" ,"央企","地方国企","其他"};
        private string[] arrProjectState = { "在建（主体结构完成）", "在建(主体结构未完)", "竣工未结算", "竣工已结算", "停工" };
        private string[] arrMustGatheringNotDaly = { "1个月以内", "1个月", "2个月", "3个月及以上" };//应收欠款时间
        private string[] arrWarnCause = { "主合同未签", "主合同节点付款比率＜80%", "主合同节点付款比率≥80%", "主合同月度付款率＜80%", 
                                          "主合同亏损", "施工手续不全", "合同内工程量确权不足", "合同外工程量确权不足",
                                          "业主拖欠", "竣工结算拖延", "竣工备案手续不全", "质量维修影响",
                                          "其他"  };
        private IList lstProjectBalanceOfPayment = null;
        public VProjectBalanceOfPaymentsAnalysisRpt(FinanceMultDataExecType exeType)
        {
            InitializeComponent();

            model = new MFinanceMultData();
            this._exeType = exeType;

            InitData();
            InitEvent();
        }

        private void InitData()
        {
            //LoadTempleteFile(rptName + ".flx");
           

            projectInfo = StaticMethod.GetProjectInfo();
            IsProject=projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode;
            if (IsProject)
            {
                this.txtOperationOrg.Text = projectInfo.Name;
                this.txtOperationOrg.Tag = projectInfo;
                this.btnOperationOrg.Visible = false;
                this.btnQuery.Visible = false;
                this.chkBalance.Visible = false;
            }
            else
            {
                this.btnQuery.Visible = true;
                this.btnOperationOrg.Visible = true;
                this.btnGenerate.Visible = false;
                this.btnSave.Visible = false;
                this.btnSubmit.Visible = false;
                this.chkBalance.Visible = true ;
            }
            this.btnBack.Visible = false;
            this.btnSave.Visible = false;
            this.btnGenerate.Visible = false;
            int iYear = DateTime.Now.Year + 1;
            for (; iYear > 2010; iYear--)
            {
                cmbYear.Items.Insert(0, iYear);
            }
            for (int iMonth = 1; iMonth <= 12; iMonth++)
            {
                cmbMonth.Items.Insert(iMonth-1,iMonth);
            }
            cmbYear.SelectedItem = DateTime.Now.Year;
            cmbMonth.SelectedItem = DateTime.Now.Month;
            if (IsProject)
            {
                btnQuery_Click(null, null);
            }
        }
        public void InitEvent()
        {
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            this.btnExcel.Click+=new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click+=new EventHandler(btnOperationOrg_Click);
            this.btnSave.Click+=new EventHandler(btnSave_Click);
            this.btnGenerate.Click+=new EventHandler(btnGenerate_Click);
            this.btnSubmit.Click+=new EventHandler(btnSubmit_Click);
            this.chkBalance.CheckedChanged+=new EventHandler(chkBalance_CheckedChanged);
            this.chkUnSelect.CheckedChanged+=new EventHandler(chkUnSelect_CheckedChanged);
            this.chkAllSelect.CheckedChanged+=new EventHandler(chkAllSelect_CheckedChanged);
            this.btnDelete.Click+=new EventHandler(btnDelete_Click);
            if (IsProject)
            {
                cmbMonth.SelectedValueChanged += new EventHandler(btnQuery_Click);
                cmbYear.SelectedValueChanged += new EventHandler(btnQuery_Click);
            }
            else
            {
                btnBack.Click+=new EventHandler(btnBack_Click);
            }
        }
        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                fGridDetail.FrozenCols = 4;
                fGridDetail.FrozenRows = 5;
                fGridDetail.Column(7).CellType = FlexCell.CellTypeEnum.ComboBox;//工程类型
                FlexCell.ComboBox oCmbBox = fGridDetail.ComboBox(7);
                oCmbBox.Locked = true;
                foreach (string sValue in arrProjectType)
                {
                    oCmbBox.Items.Insert(oCmbBox.Items.Count, sValue);
                }
                fGridDetail.Column(8).CellType = FlexCell.CellTypeEnum.ComboBox;//业主类型
                oCmbBox = fGridDetail.ComboBox(8);
                oCmbBox.Locked = true;
                foreach (string sValue in arrOwerType)
                {
                    oCmbBox.Items.Insert(oCmbBox.Items.Count, sValue);
                }
                fGridDetail.Column(9).CellType = FlexCell.CellTypeEnum.ComboBox;//工程状态
                oCmbBox = fGridDetail.ComboBox(9);
                oCmbBox.Locked = true;
                foreach (string sValue in arrProjectState)
                {
                    oCmbBox.Items.Insert(oCmbBox.Items.Count, sValue);
                }
                fGridDetail.Column(23).CellType = FlexCell.CellTypeEnum.ComboBox;//应收欠款时间
                oCmbBox = fGridDetail.ComboBox(23);
                oCmbBox.Locked = true;
                foreach (string sValue in arrMustGatheringNotDaly)
                {
                    oCmbBox.Items.Insert(oCmbBox.Items.Count, sValue);
                }
                fGridDetail.Column(47).CellType = FlexCell.CellTypeEnum.ComboBox;//应收欠款时间
                oCmbBox = fGridDetail.ComboBox(47);
                oCmbBox.Locked = true;
                foreach (string sValue in arrWarnCause)
                {
                    oCmbBox.Items.Insert(oCmbBox.Items.Count, sValue);
                }
                FlexCell.Column oCol = fGridDetail.Column(20);
                oCol.Mask = FlexCell.MaskEnum.Numeric; oCol.Position = 2;
                oCol = fGridDetail.Column(60);
                oCol.Mask = FlexCell.MaskEnum.Numeric; oCol.Position = 2;
                FlexCell.Range oRange = fGridDetail.Range(1, 1, fGridDetail.Rows-1, fGridDetail.Cols - 1);
                oRange.Locked = true;
                //fGridDetail.FixedRows = 6;
                //fGridDetail.FixedCols = 3;
               
                fGridDetail.BackColorBkg = fGridDetail.BackColor1 = System.Drawing.SystemColors.Control;
                //fGridDetail.Range(1, 1, 4, fGridDetail.Cols - 1).BackColor = System.Drawing.SystemColors.Control;
                //fGridDetail.FixedCols = 5;
                //fGridDetail.FixedRows = 6;
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
            }
        }

        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
                SelectOperationOrgInfo = info;
            }
        }
        public void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否删除", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool bSave = model.FinanceMultDataSrv.DeleteProjectBalanceOfPayment(this.currProjectBalanceOfPayment);
                    MessageBox.Show("删除成功");
                    btnQuery_Click(null, null);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("删除失败:{0}",ExceptionUtil.ExceptionMessage(ex)));
            }
            
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                IList lst = new ArrayList();
                int iCol = 2, iRow = 6;
                FlexCell.Cell oCell = null;
                ProjectBalanceOfPayment oTemp = null;
                for (; iRow < fGridDetail.Rows; iRow++)
                {
                    oCell = fGridDetail.Cell(iRow, iCol);
                    if (!string.IsNullOrEmpty(oCell.Tag) && oCell.CellType == FlexCell.CellTypeEnum.CheckBox && oCell.Text == "1")
                    {
                        oTemp = lstProjectBalanceOfPayment.OfType<ProjectBalanceOfPayment>().FirstOrDefault(a => a.Id == oCell.Tag);
                        if (oTemp != null)
                        {
                            oTemp.DocState = DocumentState.Edit;
                            lst.Add(oTemp);
                        }
                    }
                }
                if (lst.Count > 0)
                {
                    model.FinanceMultDataSrv.SaveOrUpdateProjectBalanceOfPayment(lst);
                    btnQuery_Click(btnQuery, e);
                }
                else
                {
                    MessageBox.Show("请选择需要退回的行");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:["+ex.Message+"]");

            }
        }
        private void chkBalance_CheckedChanged(object sender, EventArgs e)
        {
            btnQuery_Click(null,null);
        }
        private void chkUnSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnSelect.Checked)
            {
               
                int iCol = 2, iRow = 6;
                FlexCell.Cell oCell = null;
                ProjectBalanceOfPayment oTemp = null;
                for (; iRow < fGridDetail.Rows; iRow++)
                {
                    oCell = fGridDetail.Cell(iRow, iCol);
                    if (!string.IsNullOrEmpty(oCell.Tag) && oCell.CellType == FlexCell.CellTypeEnum.CheckBox)
                    {
                        oCell.Text = (oCell.Text == "1" ? "0" : "1");
                    }
                }
                this.chkAllSelect.CheckedChanged -= new EventHandler(chkAllSelect_CheckedChanged);
                chkAllSelect.Checked = false;
                this.chkAllSelect.CheckedChanged += new EventHandler(chkAllSelect_CheckedChanged);
            }
        }
        private void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllSelect.Checked)
            {
                int iCol = 2, iRow = 6;
                FlexCell.Cell oCell = null;
                ProjectBalanceOfPayment oTemp = null;
                for (; iRow < fGridDetail.Rows; iRow++)
                {
                    oCell = fGridDetail.Cell(iRow, iCol);
                    if (!string.IsNullOrEmpty(oCell.Tag) && oCell.CellType == FlexCell.CellTypeEnum.CheckBox)
                    {
                        oCell.Text = "1";
                    }
                }
                this.chkUnSelect.CheckedChanged -= new EventHandler(chkUnSelect_CheckedChanged);
                chkUnSelect.Checked = false;
                this.chkUnSelect.CheckedChanged += new EventHandler(chkUnSelect_CheckedChanged);
            }
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string projId = string.Empty;
            string orgSysCode = string.Empty;
            int iYear = ClientUtil.ToInt(cmbYear.SelectedItem);
            int iMonth = ClientUtil.ToInt(cmbMonth.SelectedItem);
            try
            {
                if (IsProject)
                {
                    var proj = txtOperationOrg.Tag as CurrentProjectInfo;
                    if (proj != null)
                    {
                        projId = proj.Id;
                    }
                }
                else
                {
                    var org = txtOperationOrg.Tag as OperationOrgInfo;
                    if (org == null)
                    {
                        MessageBox.Show("请选择查询范围");
                        txtOperationOrg.Focus();
                        return;
                    }

                    projId = model.IndirectCostSvr.GetProjectIDByOperationOrg(org.Id);
                    if (string.IsNullOrEmpty(projId))
                    {
                        orgSysCode = org.SysCode;
                    }
                }
                if (chkBalance.Visible = true && !IsProject && chkBalance.Checked == false)//显示未上报的项目
                {
                    btnGenerate.Visible = false;
                    IsUpdate = false;
                    btnSave.Visible = false;
                    btnSubmit.Visible = false;
                    this.chkBalance.Visible = true;
                    var org = txtOperationOrg.Tag as OperationOrgInfo;
                    btnBack.Visible = false;
                    chkAllSelect.Visible = chkUnSelect.Visible = false;
                    lstProjectBalanceOfPayment = model.FinanceMultDataSrv.QueryProjectUnBalanceOfPayment((string.IsNullOrEmpty(projId) ? org.Id : ""), projId, iYear, iMonth);
                    this.btnBack.Visible = false;
                    this.btnDelete.Visible = false;
                    ShowProjectBalanceOfPayment(lstProjectBalanceOfPayment);
                    this.fGridDetail.Range(6, 5, fGridDetail.Rows - 1, fGridDetail.Cols - 1).Locked = true;
                }
                else
                {
                    ObjectQuery oQuery = new ObjectQuery();
                    if (string.IsNullOrEmpty(projId))
                    {
                        oQuery.AddCriterion(Expression.Like("OpgSysCode", orgSysCode, MatchMode.Start));
                    }
                    else
                    {
                        oQuery.AddCriterion(Expression.Eq("ProjectId", projId));
                    }
                    if (!IsProject)
                    {
                        oQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
                    }
                    oQuery.AddCriterion(Expression.Eq("CreateYear", iYear));
                    oQuery.AddCriterion(Expression.Eq("CreateMonth", iMonth));
                    lstProjectBalanceOfPayment = model.FinanceMultDataSrv.QueryProjectBalanceOfPayment(oQuery);
                    if (lstProjectBalanceOfPayment != null && lstProjectBalanceOfPayment.Count > 0)
                    {
                        if (!IsProject)
                        {
                            btnGenerate.Visible = false;
                            IsUpdate = false;
                            btnSave.Visible = false;
                            btnSubmit.Visible = false;
                            this.chkBalance.Visible = true;
                            this.btnDelete.Visible = false;
                            this.btnBack.Visible = (lstProjectBalanceOfPayment != null && lstProjectBalanceOfPayment.Count > 0);
                            chkAllSelect.Visible = chkUnSelect.Visible = this.btnBack.Visible;
                            ShowProjectBalanceOfPayment(lstProjectBalanceOfPayment);
                            //return;
                        }
                        else
                        {
                            currProjectBalanceOfPayment = lstProjectBalanceOfPayment[0] as ProjectBalanceOfPayment;
                            if (currProjectBalanceOfPayment.DocState == DocumentState.InExecute)
                            {
                                btnGenerate.Visible = false;
                                IsUpdate = false;
                                btnSave.Visible = false;
                                btnSubmit.Visible = false;
                                this.btnDelete.Visible = false;
                            }
                            else
                            {
                                btnGenerate.Visible = true;
                                IsUpdate = true;
                                btnSave.Visible = true;
                                btnSubmit.Visible = true;
                                this.btnDelete.Visible = true;
                            }
                            this.chkBalance.Visible = false;
                            this.btnBack.Visible = false;
                            ShowProjectBalanceOfPayment(currProjectBalanceOfPayment, true);
                            //return;
                        }
                    }
                    else
                    {
                        if (!IsProject)
                        {
                            btnGenerate.Visible = false;
                            IsUpdate = false;
                            btnSave.Visible = false;
                            btnSubmit.Visible = false;
                            this.chkBalance.Visible = true;
                        }
                        else
                        {
                            btnGenerate.Visible = true;
                            IsUpdate = true;
                            btnSave.Visible = false;
                            btnSubmit.Visible = false;
                            this.chkBalance.Visible = false;
                        }
                        this.btnDelete.Visible = false;
                        this.btnBack.Visible = false;
                        LoadTempleteFile(rptName + ".flx");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("查询失败:{0}", ExceptionUtil.ExceptionMessage(ex)));
            }
            finally
            {

            }
            SetRange();
           
        }
        public void SetRange()
        {
            fGridDetail.Range(1, 1, 4, fGridDetail.Cols - 1).BackColor = System.Drawing.SystemColors.Control;

           FlexCell.Range oRange=  fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
           oRange.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
           oRange.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ViewModel();
                if (IsSave())
                {
                    currProjectBalanceOfPayment.DocState = DocumentState.Edit;
                    currProjectBalanceOfPayment = model.FinanceMultDataSrv.SaveOrUpdateProjectBalanceOfPayment(currProjectBalanceOfPayment);
                    ShowProjectBalanceOfPayment(currProjectBalanceOfPayment, true);
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:"+ExceptionUtil.ExceptionMessage(ex));
            }
        }
        public bool IsSave()
        {
            if (currProjectBalanceOfPayment.ContractGatheringRate == 0)
            {
                if (MessageBox.Show("是否保存:[当前工程状态合同收款率]为0", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewModel();
                if (IsSave())
                {
                    currProjectBalanceOfPayment.DocState = DocumentState.InExecute;
                    currProjectBalanceOfPayment = model.FinanceMultDataSrv.SaveOrUpdateProjectBalanceOfPayment(currProjectBalanceOfPayment);

                    btnGenerate.Visible = false;
                    IsUpdate = false;
                    btnSave.Visible = false;
                    btnSubmit.Visible = false;
                    btnDelete.Visible = false;
                    ShowProjectBalanceOfPayment(currProjectBalanceOfPayment, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string sProjectID = projectInfo.Id;
            int iYear = ClientUtil.ToInt(cmbYear.SelectedItem);
            int iMonth = ClientUtil.ToInt(cmbMonth.SelectedItem);
            currProjectBalanceOfPayment = model.FinanceMultDataSrv.GenerateProjectBalanceOfPayment(sProjectID, iYear, iMonth);
            btnGenerate.Visible = true;
            IsUpdate = true;
            btnSave.Visible = true;
            btnSubmit.Visible = true;
            btnDelete.Visible = false;
            IntialBill(currProjectBalanceOfPayment);
            ShowProjectBalanceOfPayment(currProjectBalanceOfPayment,true);
           // fGridDetail.Range(1, 1, 4, fGridDetail.Cols - 1).BackColor = System.Drawing.SystemColors.Control;
            SetRange();
        }
        public void IntialBill(ProjectBalanceOfPayment currProjectBalanceOfPayment)
        {
            currProjectBalanceOfPayment.CreatePerson = ConstObject.LoginPersonInfo;
            currProjectBalanceOfPayment.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            currProjectBalanceOfPayment.CreateYear = ClientUtil.ToInt(cmbYear.SelectedItem);
            currProjectBalanceOfPayment.CreateMonth = ClientUtil.ToInt(cmbMonth.SelectedItem);
            currProjectBalanceOfPayment.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
            currProjectBalanceOfPayment.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
            currProjectBalanceOfPayment.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            currProjectBalanceOfPayment.HandOrgLevel = ConstObject.TheOperationOrg.Level;
            currProjectBalanceOfPayment.HandlePerson = ConstObject.LoginPersonInfo;
            currProjectBalanceOfPayment.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            currProjectBalanceOfPayment.DocState = DocumentState.Edit;
            //归属项目
            if (IsProject)
            {
                currProjectBalanceOfPayment.ProjectId = projectInfo.Id;
                currProjectBalanceOfPayment.ProjectName = projectInfo.Name;
            }
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (fGridDetail.Rows > 7)
            {
                fGridDetail.ExportToExcel(string.Format("{0}年{1}月[{2}]{3}", ClientUtil.ToInt(cmbYear.SelectedItem), ClientUtil.ToInt(cmbMonth.SelectedItem), txtOperationOrg.Text, rptName), true, true,true);
            }
            else
            {
                MessageBox.Show("无法导出:请查询/生成数据");
            }
            }
        public void ViewModel()
        {
            bool bSave = true;
            if (IsUpdate)
            {
                ValidateData();
                int iRow, iCol;
                FlexCell.Cell oCell = null;
                iRow = fGridDetail.Rows - 2;

                iCol = 6; oCell = fGridDetail.Cell(iRow, iCol);//合同收款条款
                this.currProjectBalanceOfPayment.ContractContent = oCell.Text; //oCell.Locked = bLock;
                iCol = 7; oCell = fGridDetail.Cell(iRow, iCol);//工程类型
                currProjectBalanceOfPayment.ProjectType = oCell.Text; //oCell.Locked = bLock;
                iCol = 8; oCell = fGridDetail.Cell(iRow, iCol);//业主类型
                currProjectBalanceOfPayment.OwnerType = oCell.Text; //oCell.Locked = bLock;
                iCol = 9; oCell = fGridDetail.Cell(iRow, iCol);//工程状态
                currProjectBalanceOfPayment.ProjectState = oCell.Text; //oCell.Locked = bLock;
                iCol = 20; oCell = fGridDetail.Cell(iRow, iCol);//  当前工程状态合同收款率 
                    currProjectBalanceOfPayment.ContractGatheringRate =ClientUtil.ToDecimal( oCell.Text); //oCell.Locked = bLock;
                  
                iCol = 23; oCell = fGridDetail.Cell(iRow, iCol);//    应收欠款时间    
                currProjectBalanceOfPayment.DelayTime = oCell.Text; //oCell.Locked = bLock;
                iCol = 47; oCell = fGridDetail.Cell(iRow, iCol);//    风险分析预警 负现金流项目原因分析   		
                currProjectBalanceOfPayment.WarnCause = oCell.Text; //oCell.Locked = bLock;
                iCol = 60; oCell = fGridDetail.Cell(iRow, iCol);//  应付款项 应付账款		
                currProjectBalanceOfPayment.MustPayment = ClientUtil.ToDecimal(oCell.Text)*10000;// oCell.Locked = bLock;
                
            }
        }
        public bool ValidateData()
        {
            int iRow = fGridDetail.Rows - 2;
            int iCol = 47;
            FlexCell.Cell oCell = null;
            if (this.currProjectBalanceOfPayment.CBMoneyRemainTotal < 0)
            {

                oCell = fGridDetail.Cell(iRow, iCol);//    风险分析预警 负现金流项目原因分析
                if (string.IsNullOrEmpty(oCell.Text) || string.IsNullOrEmpty(oCell.Text.Trim()))
                {
                    throw new Exception("[总包资金净额累计]小于0时,[负现金流项目原因分析]不允许为空");
                }

            }
            else
            {
                oCell = fGridDetail.Cell(iRow, iCol);//    风险分析预警 负现金流项目原因分析
                if (!string.IsNullOrEmpty(oCell.Text) || !string.IsNullOrEmpty(oCell.Text.Trim()))
                {
                    throw new Exception("[总包资金净额累计]大于等于0时,[负现金流项目原因分析]必须为空");
                }
            }
            iCol = 7;
            oCell = fGridDetail.Cell(iRow, iCol);//工程类型
            if (string.IsNullOrEmpty(oCell.Text))
            {
                throw new Exception("[工程类型]不允许为空");
            }
            iCol = 8;
            oCell = fGridDetail.Cell(iRow, iCol);//业主类型
            if (string.IsNullOrEmpty(oCell.Text))
            {
                throw new Exception("[业主类型]不允许为空");
            }
            iCol = 9;
            oCell = fGridDetail.Cell(iRow, iCol);//工程状态
            if (string.IsNullOrEmpty(oCell.Text))
            {
                throw new Exception("[工程状态]不允许为空");
            }
            iCol = 23; oCell = fGridDetail.Cell(iRow, iCol);
            if (currProjectBalanceOfPayment.MustNotGathering > 0)//应收未收款大于0 必须应收欠款时间
            {
                if (string.IsNullOrEmpty(oCell.Text))
                {
                    throw new Exception("[ 应收未回收款]大于0时,[应收欠款时间]不允许为空");
                }
            }
            //iCol = 20; oCell = fGridDetail.Cell(iRow, iCol);//  当前工程状态合同收款率 
            //if (string.IsNullOrEmpty(oCell.Text))
            //{
            //    throw new Exception("[当前工程状态合同收款率]不能为空");
            //}
            //else
            //{
            //    if (CommonMethod.VeryValid(oCell.Text))
            //    {

            //    }
            //    else
            //    {
            //        throw new Exception("[当前工程状态合同收款率]为数值型");
            //    }
            //}
            return true;
        }
        public void ShowProjectBalanceOfPayment(IList  lst)
        {
            LoadTempleteFile(rptName + ".flx");
            fGridDetail.AutoRedraw = false;
            foreach (ProjectBalanceOfPayment oProjectBalanceOfPayment in lst)
            {
                ShowProjectBalanceOfPayment(oProjectBalanceOfPayment,false);
            }
            //fGridDetail.InsertCol(2, 1);
            //FlexCell.Cell oCell = fGridDetail.Cell(3, 2);
            //oCell.Text = "选择";
            //fGridDetail.Column(2).Locked = false;
            ////fGridDetail.Range(3, 2, 4, 2).Merge();
            //fGridDetail.Column(2).CellType = FlexCell.CellTypeEnum.CheckBox;
            fGridDetail.AutoRedraw = true;
            
            fGridDetail.Refresh();
        }
        public void ShowProjectBalanceOfPayment(ProjectBalanceOfPayment oProjectBalanceOfPayment, bool bClear)
        {
            int iRow = 0;
            int iCol = 0;
            FlexCell.Cell oCell = null;
            bool bLock = !IsUpdate;
            bool bUnBalance = (chkBalance.Visible == true && chkBalance.Checked == false);
            if (bClear)
            {
                LoadTempleteFile(rptName + ".flx");
                fGridDetail.AutoRedraw = false;
            }
            if (fGridDetail.Rows == 7)
            {

                if (IsProject)
                {
                    iRow = 2; iCol = 5; oCell = fGridDetail.Cell(iRow, iCol);//填报单位：
                    oCell.Text = txtOperationOrg.Text;
                    iCol = 3;
                    fGridDetail.Column(iCol).Visible = false;
                }
                else
                {
                    string sOrgType = SelectOperationOrgInfo != null ? model.FinanceMultDataSrv.GetOrgType(SelectOperationOrgInfo.Id) : "";
                    if (string.Equals(sOrgType, "总部"))
                    {
                        iRow = 2; iCol = 1; oCell = fGridDetail.Cell(iRow, iCol);//填报单位：
                        oCell.Text = "";
                        iCol = 3;
                        fGridDetail.Column(iCol).Visible = true;
                    }
                    else
                    {
                        iRow = 2; iCol = 5; oCell = fGridDetail.Cell(iRow, iCol);//填报单位：
                        oCell.Text = txtOperationOrg.Text;
                        iCol = 3;
                        fGridDetail.Column(iCol).Visible = false;
                    }
                }
                iRow = 2; iCol = 25; oCell = fGridDetail.Cell(iRow, iCol);//年月：	
                oCell.Text = string.Format("{0}年{1}月", ClientUtil.ToInt(cmbYear.SelectedItem), ClientUtil.ToInt(cmbMonth.SelectedItem));
            }

            iRow = fGridDetail.Rows - 1;
            fGridDetail.InsertRow(iRow, 1);//添加一行
            fGridDetail.Row(iRow).Locked = false;
            fGridDetail.Row(iRow + 1).Locked = true;

            iCol = 1; oCell = fGridDetail.Cell(iRow, iCol);//序号 
            oCell.Text = (fGridDetail.Rows - 7).ToString(); oCell.Locked = true;

            iCol = 2;//选择
            if (IsProject || bUnBalance)
            {
                fGridDetail.Column(iCol).Visible = false;
            }
            else
            {
                fGridDetail.Column(iCol).Visible = true;
                oCell = fGridDetail.Cell(iRow, iCol);
                oCell.CellType = FlexCell.CellTypeEnum.CheckBox;
                oCell.Tag = oProjectBalanceOfPayment.Id;
            }
            iCol = 3;//所属分公司
            oCell = fGridDetail.Cell(iRow, iCol);
            oCell.Text = oProjectBalanceOfPayment.SubCompanyName;
            oCell.Locked = true;

            iCol = 4; oCell = fGridDetail.Cell(iRow, iCol);//项目
            oCell.Text = oProjectBalanceOfPayment.ProjectName; oCell.Locked = true; oCell.WrapText = false; 
            if (!bUnBalance)
            {
                iCol = 5; oCell = fGridDetail.Cell(iRow, iCol);//合同额
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.ContractTotal); oCell.Locked = true;
                iCol = 6; oCell = fGridDetail.Cell(iRow, iCol);//合同收款条款
                oCell.Text = oProjectBalanceOfPayment.ContractContent; oCell.Locked = bLock;
                iCol = 7; oCell = fGridDetail.Cell(iRow, iCol);//工程类型
                oCell.Text = oProjectBalanceOfPayment.ProjectType; oCell.Locked = bLock;
                iCol = 8; oCell = fGridDetail.Cell(iRow, iCol);//业主类型
                oCell.Text = oProjectBalanceOfPayment.OwnerType; oCell.Locked = bLock;
                iCol = 9; oCell = fGridDetail.Cell(iRow, iCol);//工程状态
                oCell.Text = oProjectBalanceOfPayment.ProjectState; oCell.Locked = bLock;
                iCol = 10; oCell = fGridDetail.Cell(iRow, iCol);// 财务列报结算 累计数  	
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanBalanceTotal); oCell.Locked = true;
                iCol = 11; oCell = fGridDetail.Cell(iRow, iCol);// 财务列报结算 累计数本年数  	
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanBalanceCurrent); oCell.Locked = true;
                iCol = 12; oCell = fGridDetail.Cell(iRow, iCol);//  业主实际确认结算额   	
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.OwnerSure); oCell.Locked = true;
                iCol = 13; oCell = fGridDetail.Cell(iRow, iCol);//   业主实际确认土建结算额    	
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.OwnerSureTJ); oCell.Locked = true;
                iCol = 14; oCell = fGridDetail.Cell(iRow, iCol);//   最后一次业主审量签字时间   	
                oCell.Text = (oProjectBalanceOfPayment.OwnerSureLastTime == DateTime.MinValue ? "" : oProjectBalanceOfPayment.OwnerSureLastTime.ToString("yyyy/MM/dd")); oCell.Locked = true;
                iCol = 15; oCell = fGridDetail.Cell(iRow, iCol);//   业主审量确认到*年*月  	
                oCell.Text = oProjectBalanceOfPayment.OwnerSureYearMonth; oCell.Locked = true;
                iCol = 16; oCell = fGridDetail.Cell(iRow, iCol);//    总包累计成本支出 
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBCostPaymentTotal); oCell.Locked = true;
                iCol = 17; oCell = fGridDetail.Cell(iRow, iCol);//     土建累计成本支出  
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJCostPaymentTotal); oCell.Locked = true;
                iCol = 18; oCell = fGridDetail.Cell(iRow, iCol);// 总包确权率  
                oCell.Text = oProjectBalanceOfPayment.CBCostPaymentTotal == 0 ? "-" : oProjectBalanceOfPayment.CBSureRate; oCell.Locked = true;
                iCol = 19; oCell = fGridDetail.Cell(iRow, iCol);// 土建确权率(%)  
                oCell.Text = oProjectBalanceOfPayment.TJCostPaymentTotal == 0 ? "-" : oProjectBalanceOfPayment.TJSureRate; oCell.Locked = true;
                iCol = 20; oCell = fGridDetail.Cell(iRow, iCol);//  当前工程状态合同收款率  
                oCell.Text = oProjectBalanceOfPayment.ContractGatheringRate.ToString("N2");//oProjectBalanceOfPayment.ContractGatheringRate == 0 ? "-" : oProjectBalanceOfPayment.ContractGatheringRate.ToString("N2"); 
                if (oProjectBalanceOfPayment.ContractGatheringRate == 0)
                {
                    oCell.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    oCell.BackColor = fGridDetail.Cell(iRow, iCol - 1).BackColor;
                }
                oCell.Locked = bLock;
                iCol = 21; oCell = fGridDetail.Cell(iRow, iCol);//   按合同应收款   
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.ContractGathering); oCell.Locked = true;
                iCol = 22; oCell = fGridDetail.Cell(iRow, iCol);//    应收未回收款    
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.MustNotGathering); oCell.Locked = true;
                iCol = 23; oCell = fGridDetail.Cell(iRow, iCol);//    应收欠款时间    
                oCell.Text = oProjectBalanceOfPayment.DelayTime; oCell.Locked = bLock;
                iCol = 24; oCell = fGridDetail.Cell(iRow, iCol);//   主营业务收入 累计数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.MainBusinessTotal); oCell.Locked = true;
                iCol = 25; oCell = fGridDetail.Cell(iRow, iCol);//   主营业务收入 本年数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.MainBusinessCurrYear); oCell.Locked = true;
                iCol = 26; oCell = fGridDetail.Cell(iRow, iCol);//   主营业务收入 本月数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.MainBusinessCurrMonth); oCell.Locked = true;
                iCol = 27; oCell = fGridDetail.Cell(iRow, iCol);//   主营业务收入回款率	累计回款率  		
                oCell.Text = (oProjectBalanceOfPayment.MainBusinessTotal == 0 || oProjectBalanceOfPayment.MainBusinessTotal == decimal.MaxValue) ? "-" : oProjectBalanceOfPayment.MainBusinessGatheringTotalRate.ToString("N2"); oCell.Locked = true;
                iCol = 28; oCell = fGridDetail.Cell(iRow, iCol);//   主营业务收入回款率	本年回款率  		
                oCell.Text = (oProjectBalanceOfPayment.MainBusinessCurrYear == 0 || oProjectBalanceOfPayment.MainBusinessCurrYear == decimal.MaxValue) ? "-" : oProjectBalanceOfPayment.MainBusinessGatheringCurrYearRate.ToString("N2"); oCell.Locked = true;
                iCol = 29; oCell = fGridDetail.Cell(iRow, iCol);//   总  包  工  程  收  款	 累计数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBProjectGatheringTotal); oCell.Locked = true;
                iCol = 30; oCell = fGridDetail.Cell(iRow, iCol);//   总  包  工  程  收  款	 本年数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBProjectGatheringCurrYear); oCell.Locked = true;
                iCol = 31; oCell = fGridDetail.Cell(iRow, iCol);//   总  包  工  程  收  款	 本月数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBProjectGatheringCurrMonth); oCell.Locked = true;
                iCol = 32; oCell = fGridDetail.Cell(iRow, iCol);//  总  包  工  程  支  出	 累计数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBProjectPaymentTotal); oCell.Locked = true;
                iCol = 33; oCell = fGridDetail.Cell(iRow, iCol);//  总  包  工  程  支  出	 本年数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBProjectPaymentCurrYear); oCell.Locked = true;
                iCol = 34; oCell = fGridDetail.Cell(iRow, iCol);//  总  包  工  程  支  出	 本月数  		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBProjectPaymentCurrMonth); oCell.Locked = true;
                iCol = 35; oCell = fGridDetail.Cell(iRow, iCol);//   土  建  工  程  收  款  累计数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJProjectGatheringTotal); oCell.Locked = true;
                iCol = 36; oCell = fGridDetail.Cell(iRow, iCol);//   土  建  工  程  收  款  本年数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJProjectGatheringCurrYear); oCell.Locked = true;
                iCol = 37; oCell = fGridDetail.Cell(iRow, iCol);//   土  建  工  程  收  款  本月数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJProjectGatheringCurrMonth); oCell.Locked = true;
                iCol = 38; oCell = fGridDetail.Cell(iRow, iCol);//   土  建  工  程  支  出  累计数    		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJProjectPaymentTotal); oCell.Locked = true;
                iCol = 39; oCell = fGridDetail.Cell(iRow, iCol);//   土  建  工  程  支  出  本年数    		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJProjectPaymentCurrYear); oCell.Locked = true;
                iCol = 40; oCell = fGridDetail.Cell(iRow, iCol);//   土  建  工  程  支  出  本月数    		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJProjectPaymentCurrMonth); oCell.Locked = true;
                iCol = 41; oCell = fGridDetail.Cell(iRow, iCol);//    总  包  资  金  净  额   累计数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBMoneyRemainTotal); oCell.Locked = true;
                iCol = 42; oCell = fGridDetail.Cell(iRow, iCol);//    总  包  资  金  净  额   本年数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBMoneyRemainCurrYear); oCell.Locked = true;
                iCol = 43; oCell = fGridDetail.Cell(iRow, iCol);//    总  包  资  金  净  额   本月数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.CBMoneyRemainCurrMonth); oCell.Locked = true;
                iCol = 44; oCell = fGridDetail.Cell(iRow, iCol);//    土  建  资  金  净  额   累计数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJMoneyRemainTotal); oCell.Locked = true;
                iCol = 45; oCell = fGridDetail.Cell(iRow, iCol);//    土  建  资  金  净  额   本年数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJMoneyRemainCurrYear); oCell.Locked = true;
                iCol = 46; oCell = fGridDetail.Cell(iRow, iCol);//    土  建  资  金  净  额   本月数   		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.TJMoneyRemainCurrMonth); oCell.Locked = true;
                iCol = 47; oCell = fGridDetail.Cell(iRow, iCol);//    风险分析预警 负现金流项目原因分析   		
                oCell.Text = oProjectBalanceOfPayment.WarnCause; oCell.Locked = bLock;
                iCol = 48; oCell = fGridDetail.Cell(iRow, iCol);//    风险分析预警 现金存量预警   		
                /*oCell.Text = oProjectBalanceOfPayment.WarnMoneyRemain; */
                SetWarnColor(oCell, oProjectBalanceOfPayment.WarnMoneyRemain); oCell.Locked = true;
                iCol = 49; oCell = fGridDetail.Cell(iRow, iCol);//    风险分析预警 收现率预警
                /* oCell.Text = oProjectBalanceOfPayment.WarnMoneyFlow; */
                SetWarnColor(oCell, oProjectBalanceOfPayment.WarnMoneyFlow); oCell.Locked = true;
                iCol = 50; oCell = fGridDetail.Cell(iRow, iCol);//    风险分析预警 应收欠款预警
                /*oCell.Text = oProjectBalanceOfPayment.WarnMustNotGathering;*/
                SetWarnColor(oCell, oProjectBalanceOfPayment.WarnMustNotGathering); oCell.Locked = true;
                iCol = 51; oCell = fGridDetail.Cell(iRow, iCol);//    财务列报期末应收款 应收账款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanYearEndMustGathering); oCell.Locked = true;
                iCol = 52; oCell = fGridDetail.Cell(iRow, iCol);//    财务列报期末应收款 完工未确认款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanYearEndNotSureMoney); oCell.Locked = true;
                iCol = 53; oCell = fGridDetail.Cell(iRow, iCol);//    财务列报期末应收款 合计		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanYearEndTotal); oCell.Locked = true;
                iCol = 54; oCell = fGridDetail.Cell(iRow, iCol);//   财务列报年初应收款 应收账款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanYearBeginMustGathering); oCell.Locked = true;
                iCol = 55; oCell = fGridDetail.Cell(iRow, iCol);//   财务列报年初应收款 完工未确认款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanYearBeginNotSureMoney); oCell.Locked = true;
                iCol = 56; oCell = fGridDetail.Cell(iRow, iCol);//   财务列报年初应收款 合计		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanYearBeginTotal); oCell.Locked = true;
                iCol = 57; oCell = fGridDetail.Cell(iRow, iCol);//  财务列报应收款增长额 应收账款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanIncreaseMustGathering); oCell.Locked = true;
                iCol = 58; oCell = fGridDetail.Cell(iRow, iCol);//  财务列报应收款增长额 完工未确认款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanIncreaseNotSureMoney); oCell.Locked = true;
                iCol = 59; oCell = fGridDetail.Cell(iRow, iCol);//  财务列报应收款增长额 合计		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.FinanIncreaseTotal); oCell.Locked = true;
                iCol = 60; oCell = fGridDetail.Cell(iRow, iCol);//  应付款项 应付账款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.MustPayment); oCell.Locked = bLock;
                iCol = 61; oCell = fGridDetail.Cell(iRow, iCol);//  应付款项 其他应付款		
                oCell.Text = MoneyToString(oProjectBalanceOfPayment.OtherMustPayment); oCell.Locked = true;
                iCol = 62; oCell = fGridDetail.Cell(iRow, iCol);//  应付款项 责任上缴比例		
                oCell.Text = oProjectBalanceOfPayment.CBHandUpRate.ToString("N2"); oCell.Locked = true;
            }
            fGridDetail.Row(iRow).AutoFit();
            if (bClear)
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
            }
        }
        public void SetWarnColor(FlexCell.Cell oCell, string sValue)
        {
             //黄色预警    
                //橙色预警     
                //红色预警 
            Color oColor=System.Drawing.Color.White;
            switch (sValue)
            {
                case "黄色预警":
                    {
                        oColor = System.Drawing.Color.Yellow;
                        break;
                    }
                case "橙色预警":
                    {
                        oColor = System.Drawing.Color.Orange;
                        break;
                    }
                case "红色预警":
                    {
                        oColor = System.Drawing.Color.Red;
                        break;
                    }
                default:
                    {
                        oColor = System.Drawing.Color.White;
                        break;
                    }
            }
            oCell.BackColor = oColor;
            oCell.Text = sValue;
        }
        public string MoneyToString(decimal dValue)
        {
            decimal dUnit = 10000;
            return (dValue / dUnit).ToString("N2");
        }

        
        
    }
}
