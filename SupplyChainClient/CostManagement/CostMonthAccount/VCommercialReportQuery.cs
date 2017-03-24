using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCommercialReportQuery : TBasicDataView
    {
        private string reportCBJCZB = "成本检查指标统计表";
        private string reportJCBBB = "局成本报表";
        private string reportFBZB = "分包指标统计表";//20160824 其实就是专业分包指标统计表，之前是  专业分包指标统计表 和  劳务分包指标统计表 统称为 分包指标统计表
        private string reportFBZY = "分包争议统计表";
        private string reportFBZJBS = "分包终结报审统计表";
        private string reportGSJSJZ = "公司结算进展月报";


        private string reportLWFBZB = "劳务分包指标统计表";//20160829  
        private string reportQZSPB = "签证索赔情况表";//20160829 

        private string sOrgSysCode = null;
        private string sProjectId = null;
        private int iCount;
        private int iStart;

        MCostMonthAccount model = new MCostMonthAccount();
        private CurrentProjectInfo projectInfo;

        public VCommercialReportQuery()
        {
            InitializeComponent();
            InitData();
            InitEvents();
            VisualOperationOrg();
        }

        private void InitData()
        {
            this.comYear.Items.Clear();
            this.comMonth.Items.Clear();
            for (int i = 0; i < 13; i++)
            {
                this.comYear.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
            }
            for (int i = 1; i < 13; i++)
            {
                this.comMonth.Items.Add(i);
            }
            this.comYear.Text = DateTime.Now.Year.ToString();
            this.comMonth.Text = DateTime.Now.Month.ToString();
            //this.selectType.Items.AddRange(new object[] { reportCBJCZB, reportJCBBB, reportFBZB, reportFBZY, reportFBZJBS, reportGSJSJZ });

            //this.selectType.Items.AddRange(new object[] { reportCBJCZB, reportJCBBB, reportFBZB, reportFBZY, reportFBZJBS, reportGSJSJZ, reportLWFBZB, reportQZSPB });
            this.selectType.Items.AddRange(new object[] { reportCBJCZB, reportJCBBB, reportFBZB, reportFBZY, reportFBZJBS, reportGSJSJZ, reportQZSPB });

        }

        private void VisualOperationOrg()
        {
            bool flag = false;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                flag = true;
            }
            else
            {
                txtOperationOrg.Tag = projectInfo;
                txtOperationOrg.Text = projectInfo.Name;
                flag = false;
            }
            this.btnOperationOrg.Visible = flag;
        }

        private void InitEvents()
        {
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.btnExl.Click += new EventHandler(btnExl_Click);
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClientUtil.isEmpty(selectType.SelectedItem))
                {
                    MessageBox.Show("请选择合同类型");
                    return;
                }
                string flxName = ClientUtil.ToString(selectType.SelectedItem) + ".flx";
                Boolean b = this.LoadFLXFile(flxName);
                if (b)
                {
                    this.QueryData();
                }
                else
                {
                    MessageBox.Show("未找到模板格式文件--" + flxName);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询异常[" + ex.Message + "]");
            }
        }

        void btnExl_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel("商务填报统计", false, false, true);
        }

        private void QueryData()
        {
            try
            {
                fGridDetail.AutoRedraw = false;
                int year = ClientUtil.ToInt(this.comYear.Text);
                int month = ClientUtil.ToInt(this.comMonth.Text);
                if (this.btnOperationOrg.Visible)
                {
                    if (this.txtOperationOrg.Tag == null)
                    {
                        MessageBox.Show("请选择查询的分支机构/项目");
                        return;
                    }
                    if (ClientUtil.isEmpty(selectType.SelectedItem))
                    {
                        MessageBox.Show("请选择合同类型");
                        return;
                    }
                    OperationOrgInfo OrgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
                    sProjectId = model.IndirectCostSvr.GetProjectIDByOperationOrg(OrgInfo.Id);
                    if (string.IsNullOrEmpty(sProjectId))
                    {
                        sOrgSysCode = OrgInfo.SysCode;
                    }
                }
                else
                {
                    sProjectId = projectInfo.Id;
                }
                string type = ClientUtil.ToString(selectType.SelectedItem);
                IList list = model.CostMonthAccSrv.QueryCommercialReport(year, month, type, sOrgSysCode, sProjectId);
                if (!ClientUtil.isEmpty(list))
                {
                    this.LoadData(list, type);
                }
                else
                {
                    MessageBox.Show("查询数据为空!");
                }

                if (type != "签证索赔情况表")
                {
                    fGridDetail.Cell(1, 1).Text = year + "年" + month + "月" + txtOperationOrg.Text + type;
                }
                else //签证索赔情况表  有点特殊，因此另处理
                {
                    fGridDetail.Cell(1, 3).Text = year + "年" + month + "月" + txtOperationOrg.Text + type;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询异常[" + ex.Message + "]");
            }
            finally
            {
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
            }
        }

        private void LoadData(IList list, string type)
        {
            fGridDetail.AutoRedraw = false;
            try
            {
                int start = 0;
                {
                    #region
                    if (type == reportCBJCZB)
                    {
                        CostCheckIndicatorDtl oCostCheckIndicatorDtl=new CostCheckIndicatorDtl ();
                        iCount = 0;
                       
                        foreach (CommercialReportMaster crMaster in list)
                        {
                            ISet<CostCheckIndicatorDtl> cciDtls = crMaster.CciDtl;
                            if (cciDtls.Count > 0)
                            {
                                iCount = cciDtls.Count;
                                iStart = 7 + start;
                                fGridDetail.InsertRow(iStart, iCount);
                                FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                                iCount = 0;
                                int index = 0;
                                foreach (CostCheckIndicatorDtl cci in cciDtls)
                                {
                                    index = iStart + iCount;
                                    fGridDetail.Cell(index, 1).Text = (iCount + start + 1).ToString();
                                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(cci.TenderCalculatePoint);
                                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(cci.LiabilityPaid);
                                    fGridDetail.Cell(index, 5).Text = cci.SelfPlanMoney.ToString("N2");
                                    fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(cci.QuarterBenefitRate);
                                    fGridDetail.Cell(index, 7).Text = ClientUtil.ToString(cci.ConstructionContractIncome);
                                    fGridDetail.Cell(index, 8).Text = cci.Income.ToString("N2");
                                   oCostCheckIndicatorDtl.Income+=cci.Income;
                                    fGridDetail.Cell(index, 9).Text = cci.Cost.ToString("N2");
                                    oCostCheckIndicatorDtl.Cost+=cci.Cost;
                                    fGridDetail.Cell(index, 10).Text = cci.BenefitAmount.ToString("N2");
                                       oCostCheckIndicatorDtl.BenefitAmount+=cci.BenefitAmount;
                                    fGridDetail.Cell(index, 11).Text = ClientUtil.ToString(cci.BenefitRate);
                                    fGridDetail.Cell(index, 12).Text = cci.DutyCost.ToString("N2");
                                   oCostCheckIndicatorDtl.DutyCost+=cci.DutyCost;
                                    fGridDetail.Cell(index, 13).Text = ClientUtil.ToString(cci.OverCostReduceRate);
                                    fGridDetail.Cell(index, 14).Text = cci.SiteFunds.ToString("N2");
                                    oCostCheckIndicatorDtl.SiteFunds += cci.SiteFunds;
                                    fGridDetail.Cell(index, 15).Text = ClientUtil.ToString(cci.TotalOutputValueAccount);
                                    fGridDetail.Cell(index, 16).Text = cci.OccurredMoney.ToString("N2");
                                   oCostCheckIndicatorDtl.OccurredMoney+=cci.OccurredMoney;
                                    fGridDetail.Cell(index, 17).Text = cci.ExpectOccurredMoney.ToString("N2");
                                   oCostCheckIndicatorDtl.ExpectOccurredMoney+=cci.ExpectOccurredMoney;
                                    fGridDetail.Cell(index, 18).Text = cci.TotalMoney.ToString("N2");
                                   oCostCheckIndicatorDtl.TotalMoney+=cci.TotalMoney;
                                    fGridDetail.Cell(index, 19).Text = ClientUtil.ToString(cci.ExpectOutputValueAccount);
                                    fGridDetail.Cell(index, 20).Text = ClientUtil.ToString(cci.ConcreteDrawingBudget);
                                    fGridDetail.Cell(index, 21).Text = ClientUtil.ToString(cci.ConcreteConsumption);
                                    fGridDetail.Cell(index, 22).Text = ClientUtil.ToString(cci.ConcreteSaveRate);
                                    fGridDetail.Cell(index, 23).Text = ClientUtil.ToString(cci.RebarDetailingAmount);
                                    fGridDetail.Cell(index, 24).Text = ClientUtil.ToString(cci.RebarConsumption);
                                    fGridDetail.Cell(index, 25).Text = ClientUtil.ToString(cci.RebarSaveRate);
                                    fGridDetail.Cell(index, 26).Text = ClientUtil.ToString(cci.WasteRebarAmount);
                                    fGridDetail.Cell(index, 27).Text = ClientUtil.ToString(cci.ScrapRate);
                                    fGridDetail.Cell(index, 28).Text = ClientUtil.ToString(cci.RightReportPoint);
                                    fGridDetail.Cell(index, 29).Text = ClientUtil.ToDateTime(cci.ProjectSubmitTime).ToShortDateString();
                                    fGridDetail.Cell(index, 30).Text = ClientUtil.ToDateTime(cci.OwnerConfirmTime).ToShortDateString();
                                    fGridDetail.Cell(index, 31).Text = cci.OwnerRightOutput.ToString("N2");
                                   oCostCheckIndicatorDtl.OwnerRightOutput+=cci.OwnerRightOutput;
                                    fGridDetail.Cell(index, 32).Text = cci.ContractorRightOutput.ToString("N2");
                                   oCostCheckIndicatorDtl.ContractorRightOutput+=cci.ContractorRightOutput;
                                    fGridDetail.Cell(index, 33).Text = cci.ProjectSelfPayment.ToString("N2");
                                   oCostCheckIndicatorDtl.ProjectSelfPayment+=cci.ProjectSelfPayment;
                                    fGridDetail.Cell(index, 34).Text = cci.ProjcetContractorPayment.ToString("N2");
                                   oCostCheckIndicatorDtl.ProjcetContractorPayment+=cci.ProjcetContractorPayment;
                                    fGridDetail.Cell(index, 35).Text = ClientUtil.ToString(cci.SelfOutputRightRate);
                                    fGridDetail.Cell(index, 36).Text = ClientUtil.ToString(cci.ContractorOutputRightRate);
                                    fGridDetail.Cell(index, 37).Text = cci.OutPutRightRate;
                                    fGridDetail.Cell(index, 38).Text =  cci.ReceivableAccount.ToString("N2");
                                   oCostCheckIndicatorDtl.ReceivableAccount+=cci.ReceivableAccount;
                                    fGridDetail.Cell(index, 39).Text = cci.ActualAccount.ToString("N2");
                                   oCostCheckIndicatorDtl.ActualAccount+=cci.ActualAccount;
                                    fGridDetail.Cell(index, 40).Text = ClientUtil.ToString(cci.OverallBusinessPlan);
                                    fGridDetail.Cell(index, 41).Text = ClientUtil.ToString(cci.ResponsibilitySigh);
                                    fGridDetail.Cell(index, 42).Text = cci.ReceivableRiskMortgage.ToString("N2");
                                   oCostCheckIndicatorDtl.ReceivableRiskMortgage+=cci.ReceivableRiskMortgage;
                                    fGridDetail.Cell(index, 43).Text = cci.ActualRiskMortgage.ToString("N2");
                                   oCostCheckIndicatorDtl.ActualRiskMortgage+=cci.ActualRiskMortgage;
                                    fGridDetail.Cell(index, 44).Text = ClientUtil.ToString(cci.RiskMortgageRate);
                                    fGridDetail.Cell(index, 45).Text = cci.OccurredHourlyAccount.ToString("N2");
                                   oCostCheckIndicatorDtl.OccurredHourlyAccount+=cci.OccurredHourlyAccount;
                                    fGridDetail.Cell(index, 46).Text = ClientUtil.ToString(cci.ProportionOfHourlyWork);
                                    fGridDetail.Cell(index, 47).Text = cci.OccurredOEMAccount.ToString("N2");
                                   oCostCheckIndicatorDtl.OccurredOEMAccount+=cci.OccurredOEMAccount;
                                    fGridDetail.Cell(index, 48).Text = cci.DeductedAccount.ToString("N2");
                                   oCostCheckIndicatorDtl.DeductedAccount+=cci.DeductedAccount;
                                    fGridDetail.Cell(index, 49).Text = cci.OuterContractAccount.ToString("N2");
                                   oCostCheckIndicatorDtl.OuterContractAccount+=cci.OuterContractAccount;
                                    fGridDetail.Cell(index, 50).Text = cci.SelfSignedAccount.ToString("N2");
                                   oCostCheckIndicatorDtl.SelfSignedAccount+=cci.SelfSignedAccount;
                                    fGridDetail.Cell(index, 51).Text = ClientUtil.ToString(cci.ProportionOfOuterContractAccount);
                                    iCount++;
                                }
                                start += cciDtls.Count;
                            }
                        }
                        if(iCount>0 && string.IsNullOrEmpty(sProjectId)){
                      
                          int  index=fGridDetail.Rows-1;
                              fGridDetail.Cell(index, 1).Text ="合计:";
                                   
                                    fGridDetail.Cell(index, 7).Text = oCostCheckIndicatorDtl.Income.ToString("N2");
                                    fGridDetail.Cell(index, 8).Text = oCostCheckIndicatorDtl.Cost.ToString("N2");
                                    fGridDetail.Cell(index, 9).Text = oCostCheckIndicatorDtl.BenefitAmount.ToString("N2");
                                    fGridDetail.Cell(index, 11).Text = oCostCheckIndicatorDtl.DutyCost.ToString("N2");
                                    fGridDetail.Cell(index, 13).Text = oCostCheckIndicatorDtl.SiteFunds.ToString("N2");
                                    fGridDetail.Cell(index, 15).Text = oCostCheckIndicatorDtl.OccurredMoney.ToString("N2");
                                    fGridDetail.Cell(index, 16).Text = oCostCheckIndicatorDtl.ExpectOccurredMoney.ToString("N2");
                                    fGridDetail.Cell(index, 17).Text = oCostCheckIndicatorDtl.TotalMoney.ToString("N2");

                                    fGridDetail.Cell(index, 30).Text = oCostCheckIndicatorDtl.OwnerRightOutput.ToString("N2");
                                    fGridDetail.Cell(index, 31).Text = oCostCheckIndicatorDtl.ContractorRightOutput.ToString("N2");
                                    fGridDetail.Cell(index, 32).Text = oCostCheckIndicatorDtl.ProjectSelfPayment.ToString("N2");
                                    fGridDetail.Cell(index, 33).Text = oCostCheckIndicatorDtl.ProjcetContractorPayment.ToString("N2");
                                    fGridDetail.Cell(index, 36).Text = oCostCheckIndicatorDtl.ReceivableAccount.ToString("N2");
                                    fGridDetail.Cell(index, 37).Text = oCostCheckIndicatorDtl.ActualAccount.ToString("N2");
                                    fGridDetail.Cell(index, 40).Text = oCostCheckIndicatorDtl.ReceivableRiskMortgage.ToString("N2");
                                    fGridDetail.Cell(index, 41).Text = oCostCheckIndicatorDtl.ActualRiskMortgage.ToString("N2");
                                    fGridDetail.Cell(index, 43).Text = oCostCheckIndicatorDtl.OccurredHourlyAccount.ToString("N2");
                                    fGridDetail.Cell(index, 45).Text = oCostCheckIndicatorDtl.OccurredOEMAccount.ToString("N2");
                                    fGridDetail.Cell(index, 46).Text = oCostCheckIndicatorDtl.DeductedAccount.ToString("N2");
                                    fGridDetail.Cell(index, 47).Text = oCostCheckIndicatorDtl.OuterContractAccount.ToString("N2");
                                    fGridDetail.Cell(index, 48).Text = oCostCheckIndicatorDtl.SelfSignedAccount.ToString("N2");
                                    //iCount++;
                        }
                    }
                    #endregion

                    #region 局成本报表
                    if (type == reportJCBBB)
                    {
                        List<decimal> sum = new List<decimal>();
                        decimal[] d = new decimal[34];
                        for (int i = 0; i < 34; i++)
                        {
                            d[i] = new decimal(0);
                            sum.Add(d[i]);
                        }
                        int sum34 = 0;
                        int sum35 = 0;
                        foreach (CommercialReportMaster crMaster in list)
                        {
                            ISet<BureauCostDtl> bcDtls = crMaster.BcDtl;
                            if (bcDtls.Count > 0)
                            {
                                iCount = bcDtls.Count;
                                iStart = 8 + start;
                                fGridDetail.InsertRow(iStart, iCount);
                                FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);
                                iCount = 0;
                                int index = 0;
                                foreach (BureauCostDtl bc in bcDtls)
                                {
                                    index = iStart + iCount;
                                    fGridDetail.Cell(index, 1).Text = (iCount + start + 1).ToString();
                                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(bc.ProjectType);
                                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(bc.ProjectScale);
                                    fGridDetail.Cell(index, 5).Text = ClientUtil.ToString(bc.Location);
                                    fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(bc.OwnerName);
                                    fGridDetail.Cell(index, 7).Text = ClientUtil.ToString(bc.GeneralContractName);
                                    fGridDetail.Cell(index, 8).Text = ClientUtil.ToString(bc.OwnerProperty);
                                    fGridDetail.Cell(index, 9).Text = ClientUtil.ToString(bc.IsStractegicCustomer);
                                    fGridDetail.Cell(index, 10).Text = ClientUtil.ToString(bc.ProjectSite);
                                    fGridDetail.Cell(index, 11).Text = bc.ConstructionAcreage.ToString("N2");
                                    fGridDetail.Cell(index, 12).Text = bc.ConstructionHeight.ToString("N2"); ;
                                    fGridDetail.Cell(index, 13).Text = bc.TotalContractAmount.ToString("N2");
                                    fGridDetail.Cell(index, 14).Text = bc.SelfContractAmount.ToString("N2");
                                    fGridDetail.Cell(index, 15).Text = bc.GeneralManageCost.ToString("N2");
                                    fGridDetail.Cell(index, 16).Text = ClientUtil.ToString(bc.ManageCostCalculateRate);
                                    fGridDetail.Cell(index, 17).Text = ClientUtil.ToString(bc.SettlementType);
                                    fGridDetail.Cell(index, 18).Text = ClientUtil.ToDateTime(bc.SettlementTime).ToShortDateString();
                                    fGridDetail.Cell(index, 19).Text = ClientUtil.ToString(bc.InvokeSituation);
                                    fGridDetail.Cell(index, 20).Text = ClientUtil.ToString(bc.ImprestRate);
                                    fGridDetail.Cell(index, 21).Text = ClientUtil.ToString(bc.PaymentMode);
                                    fGridDetail.Cell(index, 22).Text = ClientUtil.ToString(bc.PaymentRate);
                                    fGridDetail.Cell(index, 23).Text = ClientUtil.ToString(bc.PaymentForm);
                                    fGridDetail.Cell(index, 24).Text = ClientUtil.ToDateTime(bc.MaintenancePayTime).ToShortDateString();
                                    fGridDetail.Cell(index, 25).Text = ClientUtil.ToString(bc.ManagementMode);
                                    fGridDetail.Cell(index, 26).Text = bc.TargetScopeMoney.ToString("N2");
                                    fGridDetail.Cell(index, 27).Text = ClientUtil.ToDateTime(bc.DutyAgreementTime).ToShortDateString();
                                    fGridDetail.Cell(index, 28).Text = ClientUtil.ToString(bc.CheckNodeSetting);
                                    fGridDetail.Cell(index, 29).Text = ClientUtil.ToString(bc.ForeBidEarningRate);
                                    fGridDetail.Cell(index, 30).Text = bc.CalculateCost.ToString("N2");
                                    fGridDetail.Cell(index, 31).Text = ClientUtil.ToString(bc.CalculateBraekevenRate);
                                    fGridDetail.Cell(index, 32).Text = bc.TargetCost.ToString("N2");
                                    fGridDetail.Cell(index, 33).WrapText = true; fGridDetail.Cell(index, 33).Text = ClientUtil.ToString(bc.HandInRate);
                                    fGridDetail.Cell(index, 34).Text = ClientUtil.ToString(bc.TargetRemark);
                                    fGridDetail.Cell(index, 35).Text = bc.ShouldPaydeposit.ToString("N2");
                                    fGridDetail.Cell(index, 36).Text = bc.ActualPayDeposit.ToString("N2");
                                    fGridDetail.Cell(index, 37).Text = ClientUtil.ToString(bc.QuarterFulfillTimes);
                                    fGridDetail.Cell(index, 38).Text = bc.QuarterShouldPayMoney.ToString("N2");
                                    fGridDetail.Cell(index, 39).Text = bc.QuarterActualPayMoney.ToString("N2");
                                    fGridDetail.Cell(index, 40).Text = bc.Quarter2015Money.ToString("N2");
                                    fGridDetail.Cell(index, 41).Text = ClientUtil.ToString(bc.NodeFulfillTimes);
                                    fGridDetail.Cell(index, 42).Text = bc.NodeShouldPayMoney.ToString("N2");
                                    fGridDetail.Cell(index, 43).Text = bc.NodeActualPayMoney.ToString("N2");
                                    fGridDetail.Cell(index, 44).Text = bc.Node2015Money.ToString("N2");
                                    fGridDetail.Cell(index, 45).Text = bc.GeneralCompleteOutput.ToString("N2");
                                    fGridDetail.Cell(index, 46).Text = bc.SelefCompleteOutput.ToString("N2");
                                    fGridDetail.Cell(index, 47).Text = bc.ExpectConfirmAmount.ToString("N2");
                                    fGridDetail.Cell(index, 48).Text = ClientUtil.ToDateTime(bc.ExpectConfirmDate).ToShortDateString();
                                    fGridDetail.Cell(index, 49).Text = bc.ConfirmedAmount.ToString("N2");
                                    fGridDetail.Cell(index, 50).Text = ClientUtil.ToDateTime(bc.ConfirmedDate).ToShortDateString();
                                    fGridDetail.Cell(index, 51).Text = bc.EstimateAmount.ToString("N2");
                                    fGridDetail.Cell(index, 52).Text = bc.ProjectActualCost.ToString("N2");
                                    fGridDetail.Cell(index, 53).Text = bc.ProjectProfit.ToString("N2");
                                    fGridDetail.Cell(index, 54).Text = bc.GatheringAmountAtRate.ToString("N2");
                                    fGridDetail.Cell(index, 55).Text = bc.SelfAcceptAmount.ToString("N2");
                                    fGridDetail.Cell(index, 56).Text = bc.SelfImprest.ToString("N2");
                                    fGridDetail.Cell(index, 57).Text = bc.SelfProgressAmount.ToString("N2");
                                    fGridDetail.Cell(index, 58).Text = bc.SelfOthers.ToString("N2");
                                    fGridDetail.Cell(index, 59).Text = bc.PaymentAtRate.ToString("N2");
                                    fGridDetail.Cell(index, 60).Text = bc.PayedAmount.ToString("N2");
                                    fGridDetail.Cell(index, 61).Text = ClientUtil.ToString(bc.ReportedQuantity);
                                    fGridDetail.Cell(index, 62).Text = bc.ReportedAmount.ToString("N2");
                                    fGridDetail.Cell(index, 63).Text = bc.CorrespondingCost.ToString("N2");
                                    fGridDetail.Cell(index, 64).Text = ClientUtil.ToString(bc.OwnerConfirmQuantity);
                                    fGridDetail.Cell(index, 65).Text = bc.OwnerConfirmAmount.ToString("N2");
                                    fGridDetail.Cell(index, 66).Text = bc.PlanReduceAmount.ToString("N2");
                                    fGridDetail.Cell(index, 67).Text = bc.PlanIncreaseAmount.ToString("N2");
                                    fGridDetail.Cell(index, 68).Text = bc.ActualReduceAmount.ToString("N2");
                                    fGridDetail.Cell(index, 69).Text = bc.ActualIncreaseAmount.ToString("N2");
                                    fGridDetail.Cell(index, 70).Text = bc.AccumulatePaidBonus.ToString("N2");
                                    fGridDetail.Cell(index, 71).Text = bc.PaidBonus2015.ToString("N2");
                                    fGridDetail.Cell(index, 72).Text = bc.RebarDetailingAmount.ToString("N2");
                                    fGridDetail.Cell(index, 73).Text = ClientUtil.ToDateTime(bc.ContractStartDate).ToShortDateString();
                                    fGridDetail.Cell(index, 74).Text = ClientUtil.ToDateTime(bc.ContractEndDate).ToShortDateString();
                                    fGridDetail.Cell(index, 75).Text = ClientUtil.ToDateTime(bc.ActualStartDate).ToShortDateString();
                                    fGridDetail.Cell(index, 76).Text = ClientUtil.ToDateTime(bc.ExpectEendDate).ToShortDateString();
                                    fGridDetail.Cell(index, 77).Text = ClientUtil.ToString(bc.IsPerforming);
                                    fGridDetail.Cell(index, 78).Text = ClientUtil.ToString(bc.DelayDays);
                                    fGridDetail.Cell(index, 79).Text = bc.CauseIncreaseCost.ToString("N2");
                                    fGridDetail.Cell(index, 80).Text = ClientUtil.ToString(bc.DurationCaption);
                                    fGridDetail.Cell(index, 81).Text = ClientUtil.ToString(bc.OwnerSignDays);
                                    fGridDetail.Cell(index, 82).Text = bc.ConfirmExpense.ToString("N2");
                                    fGridDetail.Cell(index, 83).Text = bc.ConfrontForfeit.ToString("N2");
                                    fGridDetail.Cell(index, 84).Text = ClientUtil.ToString(bc.ProgramType);
                                    fGridDetail.Cell(index, 85).Text = ClientUtil.ToString(bc.ProgramState);
                                    iCount++;
                                    sum[0] += ClientUtil.ToDecimal(bc.ConstructionAcreage);
                                    sum[1] += ClientUtil.ToDecimal(bc.TotalContractAmount);
                                    sum[2] += ClientUtil.ToDecimal(bc.SelfContractAmount);
                                    sum[3] += ClientUtil.ToDecimal(bc.GeneralManageCost);
                                    sum[4] += ClientUtil.ToDecimal(bc.TargetScopeMoney);
                                    sum[5] += ClientUtil.ToDecimal(bc.CalculateCost);
                                    sum[6] += ClientUtil.ToDecimal(bc.HandInRate);
                                    sum[7] += ClientUtil.ToDecimal(bc.ShouldPaydeposit);
                                    sum[8] += ClientUtil.ToDecimal(bc.ActualPayDeposit);
                                    sum[9] += ClientUtil.ToDecimal(bc.QuarterShouldPayMoney);
                                    sum[10] += ClientUtil.ToDecimal(bc.QuarterActualPayMoney);
                                    sum[11] += ClientUtil.ToDecimal(bc.Quarter2015Money);
                                    sum[12] += ClientUtil.ToDecimal(bc.NodeShouldPayMoney);
                                    sum[13] += ClientUtil.ToDecimal(bc.NodeActualPayMoney);
                                    sum[14] += ClientUtil.ToDecimal(bc.Node2015Money);
                                    sum[15] += ClientUtil.ToDecimal(bc.GeneralCompleteOutput);
                                    sum[16] += ClientUtil.ToDecimal(bc.SelefCompleteOutput);
                                    sum[17] += ClientUtil.ToDecimal(bc.ExpectConfirmAmount);
                                    sum[18] += ClientUtil.ToDecimal(bc.ConfirmedAmount);
                                    sum[19] += ClientUtil.ToDecimal(bc.EstimateAmount);
                                    sum[20] += ClientUtil.ToDecimal(bc.ProjectActualCost);
                                    sum[21] += ClientUtil.ToDecimal(bc.ProjectProfit);
                                    sum[22] += ClientUtil.ToDecimal(bc.SelfAcceptAmount);
                                    sum[23] += ClientUtil.ToDecimal(bc.SelfOthers);
                                    sum[24] += ClientUtil.ToDecimal(bc.PaymentAtRate);
                                    sum[25] += ClientUtil.ToDecimal(bc.PayedAmount);
                                    sum[26] += ClientUtil.ToDecimal(bc.ReportedAmount);
                                    sum[27] += ClientUtil.ToDecimal(bc.OwnerConfirmAmount);
                                    sum[28] += ClientUtil.ToDecimal(bc.PlanReduceAmount);
                                    sum[29] += ClientUtil.ToDecimal(bc.PlanIncreaseAmount);
                                    sum[30] += ClientUtil.ToDecimal(bc.ActualReduceAmount);
                                    sum[31] += ClientUtil.ToDecimal(bc.ActualIncreaseAmount);
                                    sum[32] += ClientUtil.ToDecimal(bc.AccumulatePaidBonus);
                                    sum[33] += ClientUtil.ToDecimal(bc.PaidBonus2015);
                                    sum34 += ClientUtil.ToInt(bc.ReportedQuantity);
                                    sum35 += ClientUtil.ToInt(bc.OwnerConfirmQuantity);
                                }
                                start += bcDtls.Count;
                            }
                        }
                        if (iCount>0 && string.IsNullOrEmpty(sProjectId))//多条时显示合计
                        {
                            fGridDetail.InsertRow(8 + start, 1);
                            fGridDetail.Cell(8 + start, 1).Text = ClientUtil.ToString("合计");
                            fGridDetail.Cell(8 + start, 11).Text = ClientUtil.ToDecimal(sum[0]).ToString("N2");
                            fGridDetail.Cell(8 + start, 13).Text = ClientUtil.ToDecimal(sum[1]).ToString("N2");
                            fGridDetail.Cell(8 + start, 14).Text = ClientUtil.ToDecimal(sum[2]).ToString("N2");
                            fGridDetail.Cell(8 + start, 15).Text = ClientUtil.ToDecimal(sum[3]).ToString("N2");
                            fGridDetail.Cell(8 + start, 26).Text = ClientUtil.ToDecimal(sum[4]).ToString("N2");
                            fGridDetail.Cell(8 + start, 30).Text = ClientUtil.ToDecimal(sum[5]).ToString("N2");
                            fGridDetail.Cell(8 + start, 33).Text = ClientUtil.ToDecimal(sum[6]).ToString("N2");
                            fGridDetail.Cell(8 + start, 35).Text = ClientUtil.ToDecimal(sum[7]).ToString("N2");
                            fGridDetail.Cell(8 + start, 36).Text = ClientUtil.ToDecimal(sum[8]).ToString("N2");
                            fGridDetail.Cell(8 + start, 38).Text = ClientUtil.ToDecimal(sum[9]).ToString("N2");
                            fGridDetail.Cell(8 + start, 39).Text = ClientUtil.ToDecimal(sum[10]).ToString("N2");
                            fGridDetail.Cell(8 + start, 40).Text = ClientUtil.ToDecimal(sum[11]).ToString("N2");
                            fGridDetail.Cell(8 + start, 42).Text = ClientUtil.ToDecimal(sum[12]).ToString("N2");
                            fGridDetail.Cell(8 + start, 43).Text = ClientUtil.ToDecimal(sum[13]).ToString("N2");
                            fGridDetail.Cell(8 + start, 44).Text = ClientUtil.ToDecimal(sum[14]).ToString("N2");
                            fGridDetail.Cell(8 + start, 45).Text = ClientUtil.ToDecimal(sum[15]).ToString("N2");
                            fGridDetail.Cell(8 + start, 46).Text = ClientUtil.ToDecimal(sum[16]).ToString("N2");
                            fGridDetail.Cell(8 + start, 47).Text = ClientUtil.ToDecimal(sum[17]).ToString("N2");
                            fGridDetail.Cell(8 + start, 49).Text = ClientUtil.ToDecimal(sum[18]).ToString("N2");
                            fGridDetail.Cell(8 + start, 51).Text = ClientUtil.ToDecimal(sum[19]).ToString("N2");
                            fGridDetail.Cell(8 + start, 52).Text = ClientUtil.ToDecimal(sum[20]).ToString("N2");
                            fGridDetail.Cell(8 + start, 53).Text = ClientUtil.ToDecimal(sum[21]).ToString("N2");
                            fGridDetail.Cell(8 + start, 55).Text = ClientUtil.ToDecimal(sum[22]).ToString("N2");
                            fGridDetail.Cell(8 + start, 58).Text = ClientUtil.ToDecimal(sum[23]).ToString("N2");
                            fGridDetail.Cell(8 + start, 59).Text = ClientUtil.ToDecimal(sum[24]).ToString("N2");
                            fGridDetail.Cell(8 + start, 60).Text = ClientUtil.ToDecimal(sum[25]).ToString("N2");
                            fGridDetail.Cell(8 + start, 61).Text = ClientUtil.ToDecimal(sum34).ToString("N2");
                            fGridDetail.Cell(8 + start, 62).Text = ClientUtil.ToDecimal(sum[26]).ToString("N2");
                            fGridDetail.Cell(8 + start, 64).Text = ClientUtil.ToDecimal(sum35).ToString("N2");
                            fGridDetail.Cell(8 + start, 65).Text = ClientUtil.ToDecimal(sum[27]).ToString("N2");
                            fGridDetail.Cell(8 + start, 66).Text = ClientUtil.ToDecimal(sum[28]).ToString("N2");
                            fGridDetail.Cell(8 + start, 67).Text = ClientUtil.ToDecimal(sum[29]).ToString("N2");
                            fGridDetail.Cell(8 + start, 68).Text = ClientUtil.ToDecimal(sum[30]).ToString("N2");
                            fGridDetail.Cell(8 + start, 69).Text = ClientUtil.ToDecimal(sum[31]).ToString("N2");
                            fGridDetail.Cell(8 + start, 70).Text = ClientUtil.ToDecimal(sum[32]).ToString("N2");
                            fGridDetail.Cell(8 + start, 71).Text = ClientUtil.ToDecimal(sum[33]).ToString("N2");
                        }
                    }

                    #endregion
                    #region 分包统计
                    if (type == reportFBZB)
                    {
                        //IList<BearerIndicatorDtl> lstTemp = null;//20160829  add
                        string sFlag = "0";//20160829  add  专业分包为0,劳务分包为1
                        #region xl
                        IList lstBit = null;
                        IList lstLWBit = null;//存放劳务分包的结果集  20160901 
                        int iStart = 7;
                        int iCount = 0;
                        int index = 0;

                        #region  统计求和参数 20160829
                        
                        decimal SumCurrenContractMoneyZY = 0.00M;//对应的列：5
                        decimal SumCurrentSettleMoneyZY = 0.00M;//对应的列：6
                        decimal SumCurrentOuterSettleMoneyZY = 0.00M;//对应的列：7
                        decimal SumIncomeMoneyZY = 0.00M;//对应的列：9
                        decimal SumCurrentSelfSignMoneyZY = 0.00M;//对应的列：10
                        decimal SumAccrueSettleMoneyZY = 0.00M;//对应的列：12
                        decimal SumAccrueOuterSettleMoneyZY = 0.00M;//对应的列：13
                        decimal SumAccrueSelfSignMoneyZY = 0.00M;//对应的列：14
                        decimal SumCurrentOemMoneyZY = 0.00M;//对应的列：15
                        decimal SumCurrentBeOemMoneyZY = 0.00M;//对应的列：16
                        decimal SumAccrueOemMoneyZY = 0.00M;//对应的列：17
                        decimal SumAccrueBeOemMoneyZY = 0.00M;//对应的列：18
                        decimal SumCurrentHourlyMoneyZY = 0.00M;//对应的列：19
                        decimal SumAccrueHourlyMoneyZY = 0.00M;//对应的列：21
                       

                       
                        decimal SumCurrenContractMoneyLW = 0.00M;//对应的列：5
                        decimal SumCurrentSettleMoneyLW = 0.00M;//对应的列：6
                        decimal SumCurrentOuterSettleMoneyLW = 0.00M;//对应的列：7
                        decimal SumIncomeMoneyLW = 0.00M;//对应的列：9
                        decimal SumCurrentSelfSignMoneyLW = 0.00M;//对应的列：10
                        decimal SumAccrueSettleMoneyLW = 0.00M;//对应的列：12
                        decimal SumAccrueOuterSettleMoneyLW = 0.00M;//对应的列：13
                        decimal SumAccrueSelfSignMoneyLW = 0.00M;//对应的列：14
                        decimal SumCurrentOemMoneyLW = 0.00M;//对应的列：15
                        decimal SumCurrentBeOemMoneyLW = 0.00M;//对应的列：16
                        decimal SumAccrueOemMoneyLW = 0.00M;//对应的列：17
                        decimal SumAccrueBeOemMoneyLW = 0.00M;//对应的列：18
                        decimal SumCurrentHourlyMoneyLW = 0.00M;//对应的列：19
                        decimal SumAccrueHourlyMoneyLW = 0.00M;//对应的列：21
                      

                        decimal SumCurrenContractMoney = 0.00M;//对应的列：5
                        decimal SumCurrentSettleMoney = 0.00M;//对应的列：6
                        decimal SumCurrentOuterSettleMoney = 0.00M;//对应的列：7
                        decimal SumIncomeMoney = 0.00M;//对应的列：9
                        decimal SumCurrentSelfSignMoney = 0.00M;//对应的列：10
                        decimal SumAccrueSettleMoney = 0.00M;//对应的列：12
                        decimal SumAccrueOuterSettleMoney = 0.00M;//对应的列：13
                        decimal SumAccrueSelfSignMoney = 0.00M;//对应的列：14
                        decimal SumCurrentOemMoney = 0.00M;//对应的列：15
                        decimal SumCurrentBeOemMoney = 0.00M;//对应的列：16
                        decimal SumAccrueOemMoney = 0.00M;//对应的列：17
                        decimal SumAccrueBeOemMoney = 0.00M;//对应的列：18
                        decimal SumCurrentHourlyMoney = 0.00M;//对应的列：19
                        decimal SumAccrueHourlyMoney = 0.00M;//对应的列：21
                        #endregion
                        if (list != null && list.Count > 0)
                        {
                            //if ((sProjectId.Trim().Length >0) &&(sProjectId != null))  //针对单个项目的  劳务分包 专业分包为0,劳务分包为1
                            if (!string.IsNullOrEmpty(sProjectId))  //针对单个项目的  劳务分包 专业分包为0,劳务分包为1
                            {
                                #region 针对单个项目的   劳务分包 和专业分包  20160901
                                foreach (CommercialReportMaster oMaster in list)
                                {
                                    #region 劳务分包
                                    lstLWBit = oMaster.BiDtl.OfType<BearerIndicatorDtl>().Where(a => a.FLAG == "1").OrderBy(A=>A.OrderNo).ToList();
                                    #region 插入劳务分包 4个字  20160901 add
                                    fGridDetail.InsertRow(iStart + iCount, 1);//插入一行单元格   
                                    index = iStart + iCount;
                                    fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontBold = false;
                                    fGridDetail.Cell(index, 1).WrapText = false;
                                    fGridDetail.Cell(index, 1).Text = "劳务分包：";
                                    iCount++;
                                    #endregion
                                    foreach (BearerIndicatorDtl oDetail in lstLWBit)
                                    {
                                        #region 劳务分包单元格填充
                                        fGridDetail.InsertRow(iStart + iCount, 1);//插入一行单元格
                                        index = iStart + iCount;
                                        fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontBold = false;
                                        fGridDetail.Cell(index, 1).Text = (iCount).ToString();
                                        fGridDetail.Cell(index, 2).WrapText = true;
                                        fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(oMaster.ProjectName);
                                        fGridDetail.Cell(index, 3).WrapText = true;
                                        fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(oDetail.ConstructionTeam);
                                        fGridDetail.Cell(index, 4).WrapText = true;
                                        fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(oDetail.ConstructionContent);
                                        fGridDetail.Cell(index, 5).Text = oDetail.CurrenContractMoney.ToString("N2");
                                        fGridDetail.Cell(index, 6).Text = oDetail.CurrentSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 7).Text = oDetail.CurrentOuterSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 8).Text = ClientUtil.ToString(oDetail.IsConrresponding);
                                        fGridDetail.Cell(index, 9).Text = oDetail.IncomeMoney.ToString("N2");
                                        fGridDetail.Cell(index, 10).Text = oDetail.CurrentSelfSignMoney.ToString("N2");
                                        fGridDetail.Cell(index, 11).WrapText = true;
                                        fGridDetail.Cell(index, 11).Text = ClientUtil.ToString(oDetail.Descript);
                                        fGridDetail.Cell(index, 12).Text = oDetail.AccrueSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 13).Text = oDetail.AccrueOuterSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 14).Text = oDetail.AccrueSelfSignMoney.ToString("N2");
                                        fGridDetail.Cell(index, 15).Text = oDetail.CurrentOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 16).Text = oDetail.CurrentBeOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 17).Text = oDetail.AccrueOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 18).Text = oDetail.AccrueBeOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 19).Text = oDetail.CurrentHourlyMoney.ToString("N2");
                                        //fGridDetail.Cell(index, 20).Text = ClientUtil.ToString(oDetail.CurrentHourlyRate);
                                        fGridDetail.Cell(index, 21).Text = oDetail.AccrueHourlyMoney.ToString("N2");
                                        //fGridDetail.Cell(index, 22).Text = ClientUtil.ToString(oDetail.AccrueHourlyRate);
                                        fGridDetail.Row(index).AutoFit();
                                        iCount++;
                                        #endregion
                                        #region  劳务分包统计
                                        SumCurrenContractMoneyLW += oDetail.CurrenContractMoney;//对应的列：5
                                        SumCurrentSettleMoneyLW += oDetail.CurrentSettleMoney;//对应的列：6
                                        SumCurrentOuterSettleMoneyLW += oDetail.CurrentOuterSettleMoney;//对应的列：7
                                        SumIncomeMoneyLW += oDetail.IncomeMoney;//对应的列：9
                                        SumCurrentSelfSignMoneyLW += oDetail.CurrentSelfSignMoney;//对应的列：10
                                        SumAccrueSettleMoneyLW += oDetail.AccrueSettleMoney;//对应的列：12
                                        SumAccrueOuterSettleMoneyLW += oDetail.AccrueOuterSettleMoney;//对应的列：13
                                        SumAccrueSelfSignMoneyLW += oDetail.AccrueSelfSignMoney;//对应的列：14
                                        SumCurrentOemMoneyLW += oDetail.CurrentOemMoney;//对应的列：15
                                        SumCurrentBeOemMoneyLW += oDetail.CurrentBeOemMoney;//对应的列：16
                                        SumAccrueOemMoneyLW += oDetail.AccrueOemMoney;//对应的列：17
                                        SumAccrueBeOemMoneyLW += oDetail.AccrueBeOemMoney;//对应的列：18
                                        SumCurrentHourlyMoneyLW += oDetail.CurrentHourlyMoney;//对应的列：19
                                        SumAccrueHourlyMoneyLW += oDetail.AccrueHourlyMoney;//对应的列：21
                                        #endregion
                                    }
                                    #region 劳务分包小计填充
                                    index = iStart + iCount;
                                    fGridDetail.InsertRow(index, 1);//插入一行单元格
                                    fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontBold = true;
                                    fGridDetail.Cell(index, 1).Text = "劳务分包小计：";
                                    fGridDetail.Cell(index, 1).WrapText = false;
                                    fGridDetail.Cell(index, 5).Text = SumCurrenContractMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 6).Text = SumCurrentSettleMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 7).Text = SumCurrentOuterSettleMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 9).Text = SumIncomeMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 10).Text = SumCurrentSelfSignMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 12).Text = SumAccrueSettleMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 13).Text = SumAccrueOuterSettleMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 14).Text = SumAccrueSelfSignMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 15).Text = SumCurrentOemMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 16).Text = SumCurrentBeOemMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 17).Text = SumAccrueOemMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 18).Text = SumAccrueBeOemMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 19).Text = SumCurrentHourlyMoneyLW.ToString("N2");
                                    fGridDetail.Cell(index, 21).Text = SumAccrueHourlyMoneyLW.ToString("N2");
                                    fGridDetail.Row(index).AutoFit();
                                    iCount++;//20160901 add
                                    #endregion
                                    #endregion
                                    #region 专业分包
                                    lstBit = oMaster.BiDtl.OfType<BearerIndicatorDtl>().Where(a => a.FLAG == "0").OrderBy(A => A.OrderNo).ToList();

                                    #region 插入专业分包 4个字  20160901 add
                                    fGridDetail.InsertRow(iStart + iCount, 1);//插入一行单元格
                                    
                                    index = iStart + iCount;
                                    fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontBold = false;
                                    fGridDetail.Cell(index, 1).WrapText = false;
                                    fGridDetail.Cell(index, 1).Text = "专业分包：";
                                    iCount++;
                                    #endregion

                                    foreach (BearerIndicatorDtl oDetail in lstBit)
                                    {
                                        #region 专业分包单元格填充
                                        fGridDetail.InsertRow(iStart + iCount, 1);
                                        index = iStart + iCount;
                                        fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontBold = false;
                                        //fGridDetail.Cell(index, 1).Text = (iCount + 1).ToString();
                                        fGridDetail.Cell(index, 1).Text = (iCount - 2).ToString();
                                        fGridDetail.Cell(index, 2).WrapText = true;
                                        fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(oMaster.ProjectName);
                                        fGridDetail.Cell(index, 3).WrapText = true;
                                        fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(oDetail.ConstructionTeam);
                                        fGridDetail.Cell(index, 4).WrapText = true;
                                        fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(oDetail.ConstructionContent);
                                        fGridDetail.Cell(index, 5).Text = oDetail.CurrenContractMoney.ToString("N2");
                                        fGridDetail.Cell(index, 6).Text = oDetail.CurrentSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 7).Text = oDetail.CurrentOuterSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 8).Text = ClientUtil.ToString(oDetail.IsConrresponding);
                                        fGridDetail.Cell(index, 9).Text = oDetail.IncomeMoney.ToString("N2");
                                        fGridDetail.Cell(index, 10).Text = oDetail.CurrentSelfSignMoney.ToString("N2");
                                        fGridDetail.Cell(index, 11).WrapText = true;
                                        fGridDetail.Cell(index, 11).Text = ClientUtil.ToString(oDetail.Descript);
                                        fGridDetail.Cell(index, 12).Text = oDetail.AccrueSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 13).Text = oDetail.AccrueOuterSettleMoney.ToString("N2");
                                        fGridDetail.Cell(index, 14).Text = oDetail.AccrueSelfSignMoney.ToString("N2");
                                        fGridDetail.Cell(index, 15).Text = oDetail.CurrentOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 16).Text = oDetail.CurrentBeOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 17).Text = oDetail.AccrueOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 18).Text = oDetail.AccrueBeOemMoney.ToString("N2");
                                        fGridDetail.Cell(index, 19).Text = oDetail.CurrentHourlyMoney.ToString("N2");
                                        //fGridDetail.Cell(index, 20).Text = ClientUtil.ToString(oDetail.CurrentHourlyRate);
                                        fGridDetail.Cell(index, 21).Text = oDetail.AccrueHourlyMoney.ToString("N2");
                                        //fGridDetail.Cell(index, 22).Text = ClientUtil.ToString(oDetail.AccrueHourlyRate);
                                        fGridDetail.Row(index).AutoFit();
                                        iCount++;
                                        #endregion
                                        #region  专业分包小计
                                        SumCurrenContractMoneyZY += oDetail.CurrenContractMoney;//对应的列：5
                                        SumCurrentSettleMoneyZY += oDetail.CurrentSettleMoney;//对应的列：6
                                        SumCurrentOuterSettleMoneyZY += oDetail.CurrentOuterSettleMoney;//对应的列：7
                                        SumIncomeMoneyZY += oDetail.IncomeMoney;//对应的列：9
                                        SumCurrentSelfSignMoneyZY += oDetail.CurrentSelfSignMoney;//对应的列：10
                                        SumAccrueSettleMoneyZY += oDetail.AccrueSettleMoney;//对应的列：12
                                        SumAccrueOuterSettleMoneyZY += oDetail.AccrueOuterSettleMoney;//对应的列：13
                                        SumAccrueSelfSignMoneyZY += oDetail.AccrueSelfSignMoney;//对应的列：14
                                        SumCurrentOemMoneyZY += oDetail.CurrentOemMoney;//对应的列：15
                                        SumCurrentBeOemMoneyZY += oDetail.CurrentBeOemMoney;//对应的列：16
                                        SumAccrueOemMoneyZY += oDetail.AccrueOemMoney;//对应的列：17
                                        SumAccrueBeOemMoneyZY += oDetail.AccrueBeOemMoney;//对应的列：18
                                        SumCurrentHourlyMoneyZY += oDetail.CurrentHourlyMoney;//对应的列：19
                                        SumAccrueHourlyMoneyZY += oDetail.AccrueHourlyMoney;//对应的列：21
                                        #endregion

                                    }
                                    #region 专业分包小计填充
                                    //index = iStart + iCount;
                                    fGridDetail.InsertRow(iStart + iCount, 1);
                                    index = iStart + iCount;
                                    fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontBold = true;
                                    fGridDetail.Cell(index, 1).Text = "专业分包小计：";
                                    fGridDetail.Cell(index, 1).WrapText = false;
                                    fGridDetail.Cell(index, 5).Text = SumCurrenContractMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 6).Text = SumCurrentSettleMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 7).Text = SumCurrentOuterSettleMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 9).Text = SumIncomeMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 10).Text = SumCurrentSelfSignMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 12).Text = SumAccrueSettleMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 13).Text = SumAccrueOuterSettleMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 14).Text = SumAccrueSelfSignMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 15).Text = SumCurrentOemMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 16).Text = SumCurrentBeOemMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 17).Text = SumAccrueOemMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 18).Text = SumAccrueBeOemMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 19).Text = SumCurrentHourlyMoneyZY.ToString("N2");
                                    fGridDetail.Cell(index, 21).Text = SumAccrueHourlyMoneyZY.ToString("N2");
                                    fGridDetail.Row(index).AutoFit();

                                    iCount++;//20160901 add
                                    #endregion



                                    #region 合计填充
                                    //index = iStart + iCount;
                                    fGridDetail.InsertRow(iStart + iCount, 1);
                                    index = iStart + iCount;
                                    fGridDetail.Cell(index, 1).Text = "合计：";
                                    fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontBold = true;
                                    fGridDetail.Range(index, 0, index, fGridDetail.Cols - 1).FontSize = (float)9.5;
                                    fGridDetail.Cell(index, 5).Text = (SumCurrenContractMoneyZY + SumCurrenContractMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 6).Text = (SumCurrentSettleMoneyZY + SumCurrentSettleMoneyLW).ToString("N2");//本期结算金额
                                    fGridDetail.Cell(index, 7).Text = (SumCurrentOuterSettleMoneyZY + SumCurrentOuterSettleMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 9).Text = (SumIncomeMoneyZY + SumIncomeMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 10).Text = (SumCurrentSelfSignMoneyZY + SumCurrentSelfSignMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 12).Text = (SumAccrueSettleMoneyZY + SumAccrueSettleMoneyLW).ToString("N2");//累计结算金额
                                    fGridDetail.Cell(index, 13).Text = (SumAccrueOuterSettleMoneyZY + SumAccrueOuterSettleMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 14).Text = (SumAccrueSelfSignMoneyZY + SumAccrueSelfSignMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 15).Text = (SumCurrentOemMoneyZY + SumCurrentOemMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 16).Text = (SumCurrentBeOemMoneyZY + SumCurrentBeOemMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 17).Text = (SumAccrueOemMoneyZY + SumAccrueOemMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 18).Text = (SumAccrueBeOemMoneyZY + SumAccrueBeOemMoneyLW).ToString("N2");
                                    fGridDetail.Cell(index, 19).Text = (SumCurrentHourlyMoneyZY + SumCurrentHourlyMoneyLW).ToString("N2");//本期计时工金额
                                    fGridDetail.Cell(index, 21).Text = (SumAccrueHourlyMoneyZY + SumAccrueHourlyMoneyLW).ToString("N2");//累计计时工金额

                                    #region  得到计时工比例 = (劳务计时工金额 + 专业计时工金额)/劳务结算金额


                                    //本期计时工比例
                                    SumCurrentHourlyMoney = SumCurrentHourlyMoneyZY + SumCurrentHourlyMoneyLW;//本期计时工金额汇总
                                    SumCurrentSettleMoney = SumCurrentSettleMoneyLW;// SumCurrentSettleMoneyZY + SumCurrentSettleMoneyLW;//本期劳务结算金额
                                    //decimal BQrate = SumCurrentHourlyMoney / SumCurrentSettleMoney;
                                    //decimal BQrate = 0.00M;
                                    decimal BQrate = 0, LJrate = 0;
                                    try
                                    {
                                        BQrate = SumCurrentHourlyMoney / SumCurrentSettleMoney;
                                    }
                                    catch
                                    {
                                        BQrate = 0.00M;
                                    }

                                    //累计计时工比例
                                    SumAccrueHourlyMoney = SumAccrueHourlyMoneyZY + SumAccrueHourlyMoneyLW;//累计计时工金额汇总
                                    SumAccrueSettleMoney = SumAccrueSettleMoneyLW;// SumAccrueSettleMoneyZY + SumAccrueSettleMoneyLW;//累计结算金额
                                    //decimal LJrate = SumAccrueHourlyMoney / SumAccrueSettleMoney;
                                    //decimal LJrate = 0.00M;
                                    try
                                    {
                                        LJrate = SumAccrueHourlyMoney / SumAccrueSettleMoney;
                                    }
                                    catch
                                    {
                                        LJrate = 0.00M;
                                    }
                                    //fGridDetail.Cell(index, 20).Text = "本期计时工比例：" + (BQrate * 100).ToString("N2") + "%";
                                    //fGridDetail.Cell(index, 22).Text = "累计计时工比例：" + (LJrate * 100).ToString("N2") + "%";

                                    fGridDetail.Cell(index, 20).Text = (BQrate * 100).ToString("N2") + "%";
                                    fGridDetail.Cell(index, 22).Text = (LJrate * 100).ToString("N2") + "%";
                                    #endregion


                                    fGridDetail.Row(index).AutoFit();

                                    iCount++;//20160901 add
                                    #endregion



                                    #endregion

                                }
                                #endregion
                            }
                            else //针对多个项目的，多个项目显示的时候需要按照项目来分组，一个项目只需要显示一条数据，多个项目需要显示多条数据  
                            {
                                #region  对多个项目的汇总的值
                                decimal SumCurrenContractMoneyTotal = 0.00M;//对应的列：5
                                decimal SumCurrentSettleMoneyTotal = 0.00M;//对应的列：6
                                decimal SumCurrentOuterSettleMoneyTotal = 0.00M;//对应的列：7
                                decimal SumIncomeMoneyTotal = 0.00M;//对应的列：9
                                decimal SumCurrentSelfSignMoneyTotal = 0.00M;//对应的列：10
                                decimal SumAccrueSettleMoneyTotal = 0.00M;//对应的列：12
                                decimal SumAccrueOuterSettleMoneyTotal = 0.00M;//对应的列：13
                                decimal SumAccrueSelfSignMoneyTotal = 0.00M;//对应的列：14
                                decimal SumCurrentOemMoneyTotal = 0.00M;//对应的列：15
                                decimal SumCurrentBeOemMoneyTotal = 0.00M;//对应的列：16
                                decimal SumAccrueOemMoneyTotal = 0.00M;//对应的列：17
                                decimal SumAccrueBeOemMoneyTotal = 0.00M;//对应的列：18
                                decimal SumCurrentHourlyMoneyTotal = 0.00M;//对应的列：19
                                decimal SumAccrueHourlyMoneyTotal = 0.00M;//对应的列：21
                                decimal SumCurrentSettleMoneyTotalLW = 0m;
                                decimal SumAccrueSettleMoneyTotalLW = 0m;
                                #endregion


                                List<CommercialReportMaster> ListTemp = new List<CommercialReportMaster>();
                                 foreach (CommercialReportMaster item in list)
                                 {
                                     ListTemp.Add(item);
                                 }
            
                               
                                   IEnumerable<IGrouping<string, CommercialReportMaster>> query = ListTemp.GroupBy(x => x.ProjectId);
                                    //#region  第一步： 向表格中插入合计，写在最外面
                                    //fGridDetail.InsertRow(iStart + iCount, 1);//插入一行单元格   
                                    //index = iStart + iCount;
                                    //fGridDetail.Cell(index, 1).WrapText = true;
                                    //fGridDetail.Cell(index, 1).Text = "合计:";
                                    //iCount++;
                                    //#endregion
                                foreach (IGrouping<string, CommercialReportMaster> info in query)
                                {
                                    List<CommercialReportMaster> cr = info.ToList<CommercialReportMaster>();//分组后的集合

                                    foreach (CommercialReportMaster oMaster in cr)
                                    {
                                        #region  每条分组数据代表一个项目，需要在表格中写一行，分为二步进行，每个项目，在进行累加之前，需要把之前的累加的值清空，以免重复添加


                                        SumCurrenContractMoneyZY = 0.00M;//对应的列：5
                                        SumCurrentSettleMoneyZY = 0.00M;//对应的列：6
                                        SumCurrentOuterSettleMoneyZY = 0.00M;//对应的列：7
                                        SumIncomeMoneyZY = 0.00M;//对应的列：9
                                        SumCurrentSelfSignMoneyZY = 0.00M;//对应的列：10
                                        SumAccrueSettleMoneyZY = 0.00M;//对应的列：12
                                        SumAccrueOuterSettleMoneyZY = 0.00M;//对应的列：13
                                        SumAccrueSelfSignMoneyZY = 0.00M;//对应的列：14
                                        SumCurrentOemMoneyZY = 0.00M;//对应的列：15
                                        SumCurrentBeOemMoneyZY = 0.00M;//对应的列：16
                                        SumAccrueOemMoneyZY = 0.00M;//对应的列：17
                                        SumAccrueBeOemMoneyZY = 0.00M;//对应的列：18
                                        SumCurrentHourlyMoneyZY = 0.00M;//对应的列：19
                                        SumAccrueHourlyMoneyZY = 0.00M;//对应的列：21



                                        SumCurrenContractMoneyLW = 0.00M;//对应的列：5
                                        SumCurrentSettleMoneyLW = 0.00M;//对应的列：6
                                        SumCurrentOuterSettleMoneyLW = 0.00M;//对应的列：7
                                        SumIncomeMoneyLW = 0.00M;//对应的列：9
                                        SumCurrentSelfSignMoneyLW = 0.00M;//对应的列：10
                                        SumAccrueSettleMoneyLW = 0.00M;//对应的列：12
                                        SumAccrueOuterSettleMoneyLW = 0.00M;//对应的列：13
                                        SumAccrueSelfSignMoneyLW = 0.00M;//对应的列：14
                                        SumCurrentOemMoneyLW = 0.00M;//对应的列：15
                                        SumCurrentBeOemMoneyLW = 0.00M;//对应的列：16
                                        SumAccrueOemMoneyLW = 0.00M;//对应的列：17
                                        SumAccrueBeOemMoneyLW = 0.00M;//对应的列：18
                                        SumCurrentHourlyMoneyLW = 0.00M;//对应的列：19
                                        SumAccrueHourlyMoneyLW = 0.00M;//对应的列：21


                                        SumCurrenContractMoney = 0.00M;//对应的列：5
                                        SumCurrentSettleMoney = 0.00M;//对应的列：6
                                        SumCurrentOuterSettleMoney = 0.00M;//对应的列：7
                                        SumIncomeMoney = 0.00M;//对应的列：9
                                        SumCurrentSelfSignMoney = 0.00M;//对应的列：10
                                        SumAccrueSettleMoney = 0.00M;//对应的列：12
                                        SumAccrueOuterSettleMoney = 0.00M;//对应的列：13
                                        SumAccrueSelfSignMoney = 0.00M;//对应的列：14
                                        SumCurrentOemMoney = 0.00M;//对应的列：15
                                        SumCurrentBeOemMoney = 0.00M;//对应的列：16
                                        SumAccrueOemMoney = 0.00M;//对应的列：17
                                        SumAccrueBeOemMoney = 0.00M;//对应的列：18
                                        SumCurrentHourlyMoney = 0.00M;//对应的列：19
                                        SumAccrueHourlyMoney = 0.00M;//对应的列：21



                                        #region  第一步：向表格中插入一行
                                        fGridDetail.InsertRow(iStart + iCount, 1);//插入一行单元格   
                                        index = iStart + iCount;
                                        fGridDetail.Cell(index, 1).WrapText = true;
                                        fGridDetail.Cell(index, 1).Text = (iCount+1).ToString();//表格显示的序号
                                        iCount++;
                                        #endregion

                                        #region  第二步：给表格的行赋值

                                        #region  劳务分包
                                        lstLWBit = oMaster.BiDtl.OfType<BearerIndicatorDtl>().Where(a => a.FLAG == "1").ToList();
                                        
                                        foreach (BearerIndicatorDtl oDetail in lstLWBit)
                                        {

                                            SumCurrenContractMoneyLW += oDetail.CurrenContractMoney;//对应的列：5
                                            SumCurrentSettleMoneyLW += oDetail.CurrentSettleMoney;//对应的列：6
                                            SumCurrentOuterSettleMoneyLW += oDetail.CurrentOuterSettleMoney;//对应的列：7
                                            SumIncomeMoneyLW += oDetail.IncomeMoney;//对应的列：9
                                            SumCurrentSelfSignMoneyLW += oDetail.CurrentSelfSignMoney;//对应的列：10
                                            SumAccrueSettleMoneyLW += oDetail.AccrueSettleMoney;//对应的列：12
                                            SumAccrueOuterSettleMoneyLW += oDetail.AccrueOuterSettleMoney;//对应的列：13
                                            SumAccrueSelfSignMoneyLW += oDetail.AccrueSelfSignMoney;//对应的列：14
                                            SumCurrentOemMoneyLW += oDetail.CurrentOemMoney;//对应的列：15
                                            SumCurrentBeOemMoneyLW += oDetail.CurrentBeOemMoney;//对应的列：16
                                            SumAccrueOemMoneyLW += oDetail.AccrueOemMoney;//对应的列：17
                                            SumAccrueBeOemMoneyLW += oDetail.AccrueBeOemMoney;//对应的列：18
                                            SumCurrentHourlyMoneyLW += oDetail.CurrentHourlyMoney;//对应的列：19
                                            SumAccrueHourlyMoneyLW += oDetail.AccrueHourlyMoney;//对应的列：21
                                        }
                                        
                                       
                                        #endregion

                                        #region 专业分包
                                        lstBit = oMaster.BiDtl.OfType<BearerIndicatorDtl>().Where(a => a.FLAG == "0").ToList();
                                       
                                        foreach (BearerIndicatorDtl oDetail in lstBit)
                                        {

                                            SumCurrenContractMoneyZY += oDetail.CurrenContractMoney;//对应的列：5
                                            SumCurrentSettleMoneyZY += oDetail.CurrentSettleMoney;//对应的列：6
                                            SumCurrentOuterSettleMoneyZY += oDetail.CurrentOuterSettleMoney;//对应的列：7
                                            SumIncomeMoneyZY += oDetail.IncomeMoney;//对应的列：9
                                            SumCurrentSelfSignMoneyZY += oDetail.CurrentSelfSignMoney;//对应的列：10
                                            SumAccrueSettleMoneyZY += oDetail.AccrueSettleMoney;//对应的列：12
                                            SumAccrueOuterSettleMoneyZY += oDetail.AccrueOuterSettleMoney;//对应的列：13
                                            SumAccrueSelfSignMoneyZY += oDetail.AccrueSelfSignMoney;//对应的列：14
                                            SumCurrentOemMoneyZY += oDetail.CurrentOemMoney;//对应的列：15
                                            SumCurrentBeOemMoneyZY += oDetail.CurrentBeOemMoney;//对应的列：16
                                            SumAccrueOemMoneyZY += oDetail.AccrueOemMoney;//对应的列：17
                                            SumAccrueBeOemMoneyZY += oDetail.AccrueBeOemMoney;//对应的列：18
                                            SumCurrentHourlyMoneyZY += oDetail.CurrentHourlyMoney;//对应的列：19
                                            SumAccrueHourlyMoneyZY += oDetail.AccrueHourlyMoney;//对应的列：21
                                        }
                                       
                                        #endregion

                                        fGridDetail.Cell(index, 2).Text = oMaster.ProjectName;//给项目名称赋值
 

                                        fGridDetail.Cell(index, 5).Text = (SumCurrenContractMoneyZY + SumCurrenContractMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 6).Text = (SumCurrentSettleMoneyZY + SumCurrentSettleMoneyLW).ToString("N2");//本期结算金额
                                        fGridDetail.Cell(index, 7).Text = (SumCurrentOuterSettleMoneyZY + SumCurrentOuterSettleMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 9).Text = (SumIncomeMoneyZY + SumIncomeMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 10).Text = (SumCurrentSelfSignMoneyZY + SumCurrentSelfSignMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 12).Text = (SumAccrueSettleMoneyZY + SumAccrueSettleMoneyLW).ToString("N2");//累计结算金额
                                        fGridDetail.Cell(index, 13).Text = (SumAccrueOuterSettleMoneyZY + SumAccrueOuterSettleMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 14).Text = (SumAccrueSelfSignMoneyZY + SumAccrueSelfSignMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 15).Text = (SumCurrentOemMoneyZY + SumCurrentOemMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 16).Text = (SumCurrentBeOemMoneyZY + SumCurrentBeOemMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 17).Text = (SumAccrueOemMoneyZY + SumAccrueOemMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 18).Text = (SumAccrueBeOemMoneyZY + SumAccrueBeOemMoneyLW).ToString("N2");
                                        fGridDetail.Cell(index, 19).Text = (SumCurrentHourlyMoneyZY + SumCurrentHourlyMoneyLW).ToString("N2");//本期计时工金额
                                        fGridDetail.Cell(index, 21).Text = (SumAccrueHourlyMoneyZY + SumAccrueHourlyMoneyLW).ToString("N2");//累计计时工金额

                                        #region  得到计时工比例 = (劳务计时工金额 + 专业计时工金额)/劳务结算金额


                                        //本期计时工比例
                                        SumCurrentHourlyMoney = SumCurrentHourlyMoneyZY + SumCurrentHourlyMoneyLW;//本期计时工金额汇总
                                        SumCurrentSettleMoney = SumCurrentSettleMoneyLW;// SumCurrentSettleMoneyZY + SumCurrentSettleMoneyLW;//本期结算金额
                                        //decimal BQrate = SumCurrentHourlyMoney / SumCurrentSettleMoney;
                                        decimal BQrate = 0.00M;
                                        try
                                        {
                                            BQrate = SumCurrentHourlyMoney / SumCurrentSettleMoney;
                                        }
                                        catch
                                        {
                                            BQrate = 0.00M;
                                        }


                                        //累计计时工比例
                                        SumAccrueHourlyMoney = SumAccrueHourlyMoneyZY + SumAccrueHourlyMoneyLW;//累计计时工金额汇总
                                        SumAccrueSettleMoney = SumAccrueSettleMoneyLW;// SumAccrueSettleMoneyZY + SumAccrueSettleMoneyLW;//累计劳务结算金额
                                        //decimal LJrate = SumAccrueHourlyMoney / SumAccrueSettleMoney;
                                        decimal LJrate = 0.00M;
                                        try
                                        {
                                            LJrate = SumAccrueHourlyMoney / SumAccrueSettleMoney;
                                        }
                                        catch
                                        {
                                            LJrate = 0.00M;
                                        }

                                        //fGridDetail.Cell(index, 20).Text = "本期计时工比例：" + (BQrate * 100).ToString("N2") + "%";
                                        //fGridDetail.Cell(index, 22).Text = "累计计时工比例：" + (LJrate * 100).ToString("N2") + "%";

                                        fGridDetail.Cell(index, 20).Text = (BQrate * 100).ToString("N2") + "%";
                                        fGridDetail.Cell(index, 22).Text = (LJrate * 100).ToString("N2") + "%";
                                        #endregion

                                        #region 得到最后合计行的值
                                        SumCurrenContractMoneyTotal += (SumCurrenContractMoneyZY + SumCurrenContractMoneyLW);//对应的列：5
                                        SumCurrentSettleMoneyTotal += (SumCurrentSettleMoneyZY + SumCurrentSettleMoneyLW);//对应的列：6
                                        SumCurrentSettleMoneyTotalLW += SumCurrentSettleMoneyLW;
                                        SumCurrentOuterSettleMoneyTotal += (SumCurrentOuterSettleMoneyZY + SumCurrentOuterSettleMoneyLW);//对应的列：7
                                        SumIncomeMoneyTotal += (SumIncomeMoneyZY + SumIncomeMoneyLW);//对应的列：9
                                        SumCurrentSelfSignMoneyTotal += (SumCurrentSelfSignMoneyZY + SumCurrentSelfSignMoneyLW);//对应的列：10
                                        SumAccrueSettleMoneyTotal += (SumAccrueSettleMoneyZY + SumAccrueSettleMoneyLW);//对应的列：12
                                        SumAccrueSettleMoneyTotalLW += SumAccrueSettleMoneyLW;
                                        SumAccrueOuterSettleMoneyTotal += (SumAccrueOuterSettleMoneyZY + SumAccrueOuterSettleMoneyLW);//对应的列：13
                                        SumAccrueSelfSignMoneyTotal += (SumAccrueSelfSignMoneyZY + SumAccrueSelfSignMoneyLW);//对应的列：14
                                        SumCurrentOemMoneyTotal += (SumCurrentOemMoneyZY + SumCurrentOemMoneyLW);//对应的列：15
                                        SumCurrentBeOemMoneyTotal += (SumCurrentBeOemMoneyZY + SumCurrentBeOemMoneyLW);//对应的列：16
                                        SumAccrueOemMoneyTotal += (SumAccrueOemMoneyZY + SumAccrueOemMoneyLW);//对应的列：17
                                        SumAccrueBeOemMoneyTotal += (SumAccrueBeOemMoneyZY + SumAccrueBeOemMoneyLW);//对应的列：18
                                        SumCurrentHourlyMoneyTotal += (SumCurrentHourlyMoneyZY + SumCurrentHourlyMoneyLW);//对应的列：19
                                        SumAccrueHourlyMoneyTotal += (SumAccrueHourlyMoneyZY + SumAccrueHourlyMoneyLW);//对应的列：21
                                        #endregion


                                        #endregion


                                        #endregion

                                    }
                                }

                                #region  最后一步： 向表格中插入合计，写在最外面
                                fGridDetail.InsertRow(iStart + iCount, 1);//插入一行单元格   
                                index = iStart + iCount;
                                fGridDetail.Cell(index, 1).WrapText = true;
                                fGridDetail.Cell(index, 1).Text = "合计:";


                                fGridDetail.Cell(index, 5).Text = SumCurrenContractMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 6).Text = SumCurrentSettleMoneyTotal.ToString("N2");//本期结算金额
                                fGridDetail.Cell(index, 7).Text = SumCurrentOuterSettleMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 9).Text = SumIncomeMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 10).Text = SumCurrentSelfSignMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 12).Text = SumAccrueSettleMoneyTotal.ToString("N2");//累计结算金额
                                fGridDetail.Cell(index, 13).Text = SumAccrueOuterSettleMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 14).Text = SumAccrueSelfSignMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 15).Text = SumCurrentOemMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 16).Text = SumCurrentBeOemMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 17).Text = SumAccrueOemMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 18).Text = SumAccrueBeOemMoneyTotal.ToString("N2");
                                fGridDetail.Cell(index, 19).Text = SumCurrentHourlyMoneyTotal.ToString("N2");//本期计时工金额
                                fGridDetail.Cell(index, 21).Text = SumAccrueHourlyMoneyTotal.ToString("N2");//累计计时工金额

                                #region  得到计时工比例 = (劳务计时工金额 + 专业计时工金额)/劳务结算金额


                                ////本期计时工比例
                                //SumCurrentHourlyMoney = SumCurrentHourlyMoneyZY + SumCurrentHourlyMoneyLW;//本期计时工金额汇总
                                //SumCurrentSettleMoney = SumCurrentSettleMoneyZY + SumCurrentSettleMoneyLW;//本期结算金额
                                ////decimal BQrate = SumCurrentHourlyMoney / SumCurrentSettleMoney;
                                decimal BQrateT = 0.00M;
                                try
                                {
                                    BQrateT = SumCurrentHourlyMoneyTotal / SumCurrentSettleMoneyTotalLW;
                                }
                                catch
                                {
                                    BQrateT = 0.00M;
                                }


                                ////累计计时工比例
                                //SumAccrueHourlyMoney = SumAccrueHourlyMoneyZY + SumAccrueHourlyMoneyLW;//累计计时工金额汇总
                                //SumAccrueSettleMoney = SumAccrueSettleMoneyZY + SumAccrueSettleMoneyLW;//累计结算金额
                                ////decimal LJrate = SumAccrueHourlyMoney / SumAccrueSettleMoney;
                                decimal LJrateT = 0.00M;
                                try
                                {
                                    LJrateT = SumAccrueHourlyMoneyTotal / SumAccrueSettleMoneyTotalLW;
                                }
                                catch
                                {
                                    LJrateT = 0.00M;
                                }

                                ////fGridDetail.Cell(index, 20).Text = "本期计时工比例：" + (BQrate * 100).ToString("N2") + "%";
                                ////fGridDetail.Cell(index, 22).Text = "累计计时工比例：" + (LJrate * 100).ToString("N2") + "%";

                                fGridDetail.Cell(index, 20).Text = (BQrateT * 100).ToString("N2") + "%";
                                fGridDetail.Cell(index, 22).Text = (LJrateT * 100).ToString("N2") + "%";
                                #endregion



                                iCount++;



                                #endregion



                                
                            }
                            fGridDetail.Column(1).AutoFit();
                            fGridDetail.Column(2).AutoFit();
                            fGridDetail.Column(3).AutoFit();
                        }
                        #endregion
                    }
                    #endregion
                    #region 签证索赔 20160829 add
                    if (type == reportQZSPB)
                    {
                        #region xl
                        int iStart = 5;
                        int iCount = 0;
                        int index = 0;
                        #region  合计 20160829 add
                        decimal SumQZLJBSJE = 0.00M;//对应的列：5
                        decimal SumLJQRSR = 0.00M;//对应的列：6
                        decimal SumLSSJSR = 0.00M;//对应的列：7
                        decimal SumLJBSJE = 0.00M;//对应的列：9
                        decimal SumLJSJCB = 0.00M;//对应的列：10
                        decimal SumQZLJSJCB = 0.00M;//对应的列：11
                        decimal SumJZLJZXSGCZ = 0.00M;//对应的列：13
                        int SumProjectDeclareCount = 0,SumRewardDeclareCount=0;
                        decimal SumProjectRiskSolution = 0, SumProjectAddBenefit = 0, SumRewardRiskSolution = 0, SumRewardAddBenefit = 0, SumOutPutMoney=0;

                        #endregion

                        if (list != null && list.Count > 0)
                        {
                            foreach (CommercialReportMaster oMaster in list)
                            {
                                foreach (VisaClamisDtl dt in oMaster.VcDtl)
                                {
                                    #region 单元格填充
                                    fGridDetail.InsertRow(iStart + iCount, 1);
                                    index = iStart + iCount;
                                    fGridDetail.Cell(index, 1).Text = (iCount + 1).ToString();
                                    fGridDetail.Cell(index, 2).WrapText = true;
                                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(oMaster.ProjectName);
                                    fGridDetail.Cell(index, 3).WrapText = true;
                                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(dt.YBSQZFS);
                                    fGridDetail.Cell(index, 4).WrapText = true;
                                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(dt.YQRQZFS);
                                    fGridDetail.Cell(index, 5).Text = dt.QZLJBSJE.ToString("N2");
                                    fGridDetail.Cell(index, 6).Text = dt.LJQRSR.ToString("N2");
                                    fGridDetail.Cell(index, 7).Text = dt.LSSJSR.ToString("N2");
                                    fGridDetail.Cell(index, 8).Text = ClientUtil.ToString(dt.LJQRQZBFSYL);
                                    fGridDetail.Cell(index, 9).Text = dt.LJBSJE.ToString("N2");
                                    fGridDetail.Cell(index, 10).Text = dt.LJSJCB.ToString("N2");
                                    fGridDetail.Cell(index, 11).Text = dt.QZLJSJCB.ToString("N2");
                                    fGridDetail.Cell(index, 12).Text = ClientUtil.ToString(dt.QZXYL);
                                    fGridDetail.Cell(index, 13).Text = dt.JZLJZXSGCZ.ToString("N2");
                                    fGridDetail.Cell(index, 14).Text = ClientUtil.ToString(dt.QZZEDCZGXL);
                                    fGridDetail.Cell(index, 15).Text = ClientUtil.ToString(dt.QZXYCZGXL);

                                    fGridDetail.Cell(index, 16).Text = ClientUtil.ToString(dt.ProjectDeclareCount);
                                    fGridDetail.Cell(index, 17).Text = ClientUtil.ToString(dt.ProjectRiskSolution);
                                    fGridDetail.Cell(index, 18).Text = ClientUtil.ToString(dt.ProjectAddBenefit);
                                    fGridDetail.Cell(index, 19).Text = ClientUtil.ToString(dt.RewardDeclareCount);
                                    fGridDetail.Cell(index, 20).Text = ClientUtil.ToString(dt.RewardRiskSolution);
                                    fGridDetail.Cell(index, 21).Text = ClientUtil.ToString(dt.RewardAddBenefit);
                                    fGridDetail.Cell(index, 22).Text = ClientUtil.ToString(dt.OutPutMoney);
                                    fGridDetail.Cell(index, 23).Text = ClientUtil.ToString(dt.SelfMoneyRate);
                                    fGridDetail.Row(index).AutoFit();
                                    iCount++;
                                    #endregion
                                    #region  统计
                                    SumQZLJBSJE += dt.QZLJBSJE;//对应的列：5
                                    SumLJQRSR += dt.LJQRSR;//对应的列：6
                                    SumLSSJSR += dt.LSSJSR;//对应的列：7
                                    SumLJBSJE += dt.LJBSJE;//对应的列：9
                                    SumLJSJCB += dt.LJSJCB;//对应的列：10
                                    SumQZLJSJCB += dt.QZLJSJCB;//对应的列：11
                                    SumJZLJZXSGCZ += dt.JZLJZXSGCZ;//对应的列：13
                                    SumProjectDeclareCount += dt.ProjectDeclareCount;
                                    SumProjectRiskSolution += dt.ProjectRiskSolution;
                                    SumProjectAddBenefit += dt.ProjectAddBenefit;
                                    SumRewardDeclareCount += dt.RewardDeclareCount;
                                    SumRewardRiskSolution += dt.RewardRiskSolution;
                                    SumRewardAddBenefit += dt.RewardAddBenefit;
                                    SumOutPutMoney += dt.OutPutMoney;
                                    #endregion
                                }
                            }
                            #region 合计填充
                            if (iCount>0   && string.IsNullOrEmpty(sProjectId))
                            {
                                index = iStart + iCount;
                                fGridDetail.InsertRow(index, 1);
                                fGridDetail.Cell(index, 1).Text = "合计：";
                                fGridDetail.Cell(index, 5).Text = SumQZLJBSJE.ToString("N2");
                                fGridDetail.Cell(index, 6).Text = SumLJQRSR.ToString("N2");
                                fGridDetail.Cell(index, 7).Text = SumLSSJSR.ToString("N2");
                                fGridDetail.Cell(index, 9).Text = SumLJBSJE.ToString("N2");
                                fGridDetail.Cell(index, 10).Text = SumLJSJCB.ToString("N2");
                                fGridDetail.Cell(index, 11).Text = SumQZLJSJCB.ToString("N2");
                                fGridDetail.Cell(index, 13).Text = SumJZLJZXSGCZ.ToString("N2");
                                fGridDetail.Cell(index, 16).Text = SumProjectDeclareCount.ToString("N2");
                                fGridDetail.Cell(index, 17).Text = SumProjectRiskSolution.ToString("N2");
                                fGridDetail.Cell(index, 18).Text = SumProjectAddBenefit.ToString("N2");
                                fGridDetail.Cell(index, 19).Text =SumRewardDeclareCount.ToString("N2");
                                fGridDetail.Cell(index, 20).Text =SumRewardRiskSolution.ToString("N2");
                                fGridDetail.Cell(index, 21).Text =SumRewardAddBenefit.ToString("N2");
                                fGridDetail.Cell(index, 22).Text =SumOutPutMoney.ToString("N2");
                               // fGridDetail.Cell(index, 23).Text = SumSelfMoneyRate.ToString("N2");
                                fGridDetail.Row(index).AutoFit();
                            }

                            #endregion
                        }

                        #endregion
                    }
                    #endregion

                    #region
                    if (type == reportFBZY)
                    {
                        foreach (CommercialReportMaster crMaster in list)
                        {
                            ISet<DisputeTrackDtl> dtDtls = crMaster.DtDtl;
                            if (dtDtls.Count > 0)
                            {
                                iCount = dtDtls.Count;
                                iStart = 5 + start;
                                fGridDetail.InsertRow(iStart, iCount);
                                FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                                iCount = 0;
                                int index = 0;
                                foreach (DisputeTrackDtl dt in dtDtls)
                                {
                                    index = iStart + iCount;
                                    fGridDetail.Cell(index, 1).Text = (iCount + start + 1).ToString();
                                    fGridDetail.Cell(index, 2).WrapText = true;
                                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                                    fGridDetail.Cell(index, 3).WrapText = true;
                                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(dt.BearerUnitName);
                                    fGridDetail.Cell(index, 4).WrapText = true;
                                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(dt.DisputeContent);
                                    fGridDetail.Cell(index, 5).WrapText = true;
                                    fGridDetail.Cell(index, 5).Text = ClientUtil.ToString(dt.BearerSuggestion);
                                    fGridDetail.Cell(index, 6).WrapText = true;
                                    fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(dt.ProjectSuggestion);
                                    fGridDetail.Cell(index, 7).Text = dt.InvolveMoney.ToString("N2");
                                    fGridDetail.Cell(index, 8).WrapText = true;
                                    fGridDetail.Cell(index, 8).Text = ClientUtil.ToString(dt.CurrentProgress);
                                    fGridDetail.Cell(index, 9).WrapText = true;
                                    fGridDetail.Cell(index, 9).Text = ClientUtil.ToString(dt.Descript);
                                    fGridDetail.Row(index).AutoFit();
                                    iCount++;
                                }
                                start += dtDtls.Count;
                            }
                        }
                    }
                    #endregion

                    #region
                    if (type == reportFBZJBS)
                    {
                        foreach (CommercialReportMaster crMaster in list)
                        {
                            ISet<SubcontractAuditDtl> saDtls = crMaster.SaDtl;
                            if (saDtls.Count > 0)
                            {
                                iCount = saDtls.Count;
                                iStart = 5 + start;
                                fGridDetail.InsertRow(iStart, iCount);
                                FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                                iCount = 0;
                                int index = 0;
                                foreach (SubcontractAuditDtl sa in saDtls)
                                {
                                    index = iStart + iCount;
                                    fGridDetail.Cell(index, 2).WrapText = true;
                                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                                    fGridDetail.Cell(index, 3).WrapText = true;
                                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(sa.SubentryProjectName);
                                    fGridDetail.Cell(index, 4).WrapText = true;
                                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(sa.BearerOrgName);
                                    fGridDetail.Cell(index, 5).Text = sa.SubcontractAmount.ToString("N2");
                                    fGridDetail.Cell(index, 6).Text = sa.AccumulateAmount.ToString("N2");
                                    fGridDetail.Cell(index, 7).Text = ClientUtil.ToDateTime(sa.Makespan).ToShortDateString();
                                    fGridDetail.Cell(index, 8).Text = sa.ExpectSubcontractAmount.ToString("N2");
                                    fGridDetail.Cell(index, 9).Text = ClientUtil.ToString(sa.IsAudit);
                                    fGridDetail.Cell(index, 10).Text = ClientUtil.ToDateTime(sa.ExpectAuditTime).ToShortDateString();
                                    fGridDetail.Cell(index, 11).Text = ClientUtil.ToString(sa.Descript);
                                    fGridDetail.Row(index).AutoFit();
                                    iCount++;
                                }
                                start += saDtls.Count;
                            }
                        }
                    }
                    #endregion


                    #region
                    if (type == reportGSJSJZ)
                    {
                        foreach (CommercialReportMaster crMaster in list)
                        {
                            ISet<SettlementProgressReportDtl> sprDtls = crMaster.SprDtl;
                            if (sprDtls.Count > 0)
                            {
                                iCount = sprDtls.Count;
                                iStart = 6 + start;
                                fGridDetail.InsertRow(iStart, iCount);
                                FlexCell.Range range = fGridDetail.Range(iStart, 1, iCount + iStart, fGridDetail.Cols - 1);

                                iCount = 0;
                                int index = 0;
                                foreach (SettlementProgressReportDtl spr in sprDtls)
                                {
                                    index = iStart + iCount;
                                    fGridDetail.Cell(index, 1).Text = (iCount + start + 1).ToString();
                                    fGridDetail.Cell(index, 2).WrapText = true;
                                    fGridDetail.Cell(index, 2).Text = ClientUtil.ToString(crMaster.ProjectName);
                                    fGridDetail.Cell(index, 3).WrapText = true;
                                    fGridDetail.Cell(index, 3).Text = ClientUtil.ToString(spr.ThisMonth);
                                    fGridDetail.Cell(index, 4).WrapText = true;
                                    fGridDetail.Cell(index, 4).Text = ClientUtil.ToString(spr.SettlementProgress);
                                    fGridDetail.Cell(index, 5).WrapText = true;
                                    fGridDetail.Cell(index, 5).Text = ClientUtil.ToString(spr.ImportantFactor);
                                    fGridDetail.Cell(index, 6).WrapText = true;
                                    fGridDetail.Cell(index, 6).Text = ClientUtil.ToString(spr.NextMonthPlan);
                                    fGridDetail.Cell(index, 7).WrapText = true;
                                    fGridDetail.Cell(index, 7).Text = ClientUtil.ToString(spr.Descript);
                                    fGridDetail.Row(index).AutoFit();
                                    iCount++;
                                }
                                start += sprDtls.Count;
                            }
                        }
                    }
                    #endregion
                }
         
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                FlexCell.Range oRange = fGridDetail.Range(0, 0, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
                oRange.Locked = true;
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
            }
        }

        private bool LoadFLXFile(string flxName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;

            if (flxName.IndexOf("分包指标统计表")>-1)  //目的是之前的模板中已经存在这个模板，为了不影响之前的功能，现在需要加载新的模板
            {
                flxName = flxName.Insert(flxName.LastIndexOf(".") , "_");
            }
            if (eFile.IfExistFileInServer(flxName))
            {
                eFile.CreateTempleteFileFromServer(flxName);
                fGridDetail.OpenFile(path + "\\" + flxName);//载入格式
                //fGridDetail.FrozenRows = (flxName.IndexOf(reportQZSPB) > -1 ? 2 : 3);
                fGridDetail.FrozenCols = 2;
                return true;
            }
            return false;
        }
    }
}
