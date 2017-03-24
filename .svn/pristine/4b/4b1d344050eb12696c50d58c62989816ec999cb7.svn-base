using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using FetchMode = NHibernate.FetchMode;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VProjectSubContractBalance : TBasicDataView
    {
        public MSubContractBalance subContractOperate = new MSubContractBalance();
        private CurrentProjectInfo projectInfo;
        private SubContractBalanceBill curBillMaster;
        private string[] arrBalBaseReadOnly = { "计时工", "代工", "罚款", "分包签证" };
        private Dictionary<string, ProjectTaskDetailAccount> accDetailCache;
        private Dictionary<string, decimal> rateChanges; 

        public VProjectSubContractBalance()
        {
            InitializeComponent();

            InitEvents();

            InitData();
        }

        private void InitEvents()
        {
            btnCreate.Click += btnCreate_Click;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            btnSetBalanceBase.Click += new EventHandler(btnSetBalanceBase_Click);
            btnDeleteDetail.Click += new EventHandler(btnDeleteDetail_Click);
            btnSelectBalOrg.Click+=new EventHandler(btnSelectBalOrg_Click);
            btnClearBalOrg.Click+=new EventHandler(btnClearBalOrg_Click);

            dgvBills.AutoGenerateColumns = false;
            dgvBills.AllowUserToOrderColumns = true;
            dgvBills.CellClick += new DataGridViewCellEventHandler(dgvBills_CellClick);
            dgvBills.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dgvBills_RowPostPaint);

            dgvBillDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgvBillDetail_CellDoubleClick);
            dgvBillDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgvBillDetail_CellValidating);
            dgvBillDetail.CellEndEdit += new DataGridViewCellEventHandler(dgvBillDetail_CellEndEdit);

            cmbMonth.SelectedIndexChanged += new EventHandler(cmbMonth_SelectedIndexChanged);
            cmbYear.SelectedIndexChanged += cmbMonth_SelectedIndexChanged;
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();

            DateTime serverTime = subContractOperate.GetServerTime();
            dtpBalanceDate.BeginValue = serverTime.AddDays(-7);
            dtpBalanceDate.EndValue = serverTime;

            GetWbsRootNode();

            InitYear();

            InitMonth();
        }

        private void InitYear()
        {
            var dic = new List<KeyValuePair<int,string>>();
            dic.Add(new KeyValuePair<int, string>(0, ""));
            for (int i = DateTime.Now.Year; i >= 2000; i--)
            {
                dic.Add(new KeyValuePair<int, string>(i, string.Format("{0}年", i)));
            }

            cmbYear.DataSource = dic;
            cmbYear.DisplayMember = "Value";
            cmbYear.ValueMember = "Key";

            cmbYear.SelectedIndex = 1;
        }

        private void InitMonth()
        {
            var dic = new List<KeyValuePair<int, string>>();
            dic.Add(new KeyValuePair<int, string>(0, ""));
            for (int i = 1; i <= 12; i++)
            {
                dic.Add(new KeyValuePair<int, string>(i, string.Format("{0}月", i)));
            }

            cmbMonth.DataSource = dic;
            cmbMonth.DisplayMember = "Value";
            cmbMonth.ValueMember = "Key";
        }

        private void GetWbsRootNode()
        {
            if (projectInfo == null)
            {
                return;
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("ParentNode", null));

            var list = subContractOperate.ObjectQuery(typeof (GWBSTree), objectQuery).OfType<GWBSTree>().ToList();
            if (list == null || list.Count == 0)
            {
                txtRootNode.Clear();
                txtRootNode.Tag = null;
            }
            else
            {
                txtRootNode.Text = list[0].Name;
                txtRootNode.Tag = list[0];
            }
        }

        private void LoadCreatedBills()
        {
            FlashScreen.Show("数据加载中，请稍候...");

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.IsNotNull("CreateBatchNo"));

            var year = 0;
            if (cmbYear.SelectedItem != null)
            {
                year = ((KeyValuePair<int, string>) cmbYear.SelectedItem).Key;
                if (year > 0)
                {
                    oq.AddCriterion(Expression.Eq("CreateYear", year));
                }
            }

            var mon = 0;
            if (cmbMonth.SelectedItem != null)
            {
                mon = ((KeyValuePair<int, string>) cmbMonth.SelectedItem).Key;
                if (mon > 0)
                {
                    oq.AddCriterion(Expression.Eq("CreateMonth", mon));
                }
            }

            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);

            var list = subContractOperate.ObjectQuery(typeof (SubContractBalanceBill), oq)
                .OfType<SubContractBalanceBill>().ToList();
            dgvBills.DataSource = list.OrderBy(a => a.SubContractUnitName).ThenBy(b => b.Code).ToArray();

            curBillMaster = null;
            dgvBills.ClearSelection();
            dgvBillDetail.Rows.Clear();
            accDetailCache = new Dictionary<string, ProjectTaskDetailAccount>();
            rateChanges = new Dictionary<string, decimal>();

            if (year > 0 && mon > 0)
            {
                if (year == DateTime.Now.Year && mon == DateTime.Now.Month)
                {
                    dtpBusinessDate.Value = DateTime.Now.Date;
                }
                else
                {
                    dtpBusinessDate.Value = new DateTime(year, mon, 10);
                }
            }

            FlashScreen.Close();
        }

        private ProjectTaskDetailAccount GetProjectTaskDetailAccountById(string detailId)
        {
            if (string.IsNullOrEmpty(detailId))
            {
                return null;
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", detailId));
            objectQuery.AddFetchMode("Details", FetchMode.Eager);

            var list = subContractOperate.ObjectQuery(typeof (ProjectTaskDetailAccount), objectQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }
            else
            {
                return list[0] as ProjectTaskDetailAccount;
            }
        }

        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmit(SubContractBalanceBill bill, int optrType)
        {
            FlashScreen.Show("处理中，请稍候...");

            bill.CreateDate = dtpBusinessDate.Value.Date;
            bill.CreateYear = bill.CreateDate.Year;
            bill.CreateMonth = bill.CreateDate.Month;
            bill.CreatePerson = ConstObject.LoginPersonInfo;
            bill.CreatePersonName = bill.CreatePerson.Name;

            //判断明细金额和耗用金额是否相等
            foreach (SubContractBalanceDetail subDetial in bill.Details)
            {
                decimal detailMoney = subDetial.BalanceTotalPrice;
                decimal sumSubjectMoney = 0;
                foreach (SubContractBalanceSubjectDtl subject in subDetial.Details)
                {
                    sumSubjectMoney += subject.BalanceTotalPrice;
                }

                if (decimal.Round(detailMoney, 2) != decimal.Round(sumSubjectMoney, 2))
                {
                    FlashScreen.Close();
                    MessageBox.Show("[" + subDetial.BalanceTaskDtlName + "]的明细金额[" + decimal.Round(detailMoney, 2) +
                                    "]和明细对应耗用金额之和[" + decimal.Round(sumSubjectMoney, 2) + "]不相等！");
                    return false;
                }
            }

            //判断金额是否超过百分比
            var subProject = bill.TheSubContractProject;
            if (optrType == 2)
            {
                if (subProject.ContractInterimMoney > 0)
                {
                    decimal sumContractMoney = subProject.ContractInterimMoney*(1 + subProject.AllowExceedPercent);
                    //最大合同额
                    decimal currBalMoney = subProject.AddupBalanceMoney + subProject.AddupWaitApproveBalMoney +
                                           bill.BalanceMoney;
                    if (currBalMoney > sumContractMoney && sumContractMoney > 0)
                    {
                        FlashScreen.Close();
                        MessageBox.Show("累计结算金额[" + currBalMoney + "]大于分包合同最大金额[" + sumContractMoney + "]！");
                        return false;
                    }
                }

                var dts = bill.Details.OfType<SubContractBalanceDetail>().ToList();
                if (dts.Exists(a => string.IsNullOrEmpty(a.BalanceBase)))
                {
                    FlashScreen.Close();
                    MessageBox.Show("结算依据都不能为空，请选择");
                    return false;
                }

                bill.DocState = DocumentState.InAudit;
            }

            subContractOperate.SubBalSrv.SaveSubContractBalBill(bill);

            WriteBackProjectAccountDetail(bill);
            FlashScreen.Close();
            return true;
        }

        private void AfterBalanceValueChanged(SubContractBalanceDetail dt)
        {
            if (dt == null)
            {
                return;
            }
            var bill = dgvBills.SelectedRows[0].DataBoundItem as SubContractBalanceBill;
            if (bill == null || bill.DocState != DocumentState.Edit)
            {
                return;
            }

            var totalMoney = 0m;
            foreach (SubContractBalanceDetail detail in bill.Details)
            {
                if (detail.Id == dt.Id)
                {
                    detail.BalacneQuantity = dt.BalacneQuantity;
                    detail.BalancePrice = dt.BalancePrice;
                    detail.BalanceTotalPrice = dt.BalanceTotalPrice;
                }

                totalMoney += detail.BalanceTotalPrice;
            }

            for (var i = 0; i < dgvBillDetail.Rows.Count; i++)
            {
                var detail = dgvBillDetail.Rows[i].Tag as SubContractBalanceDetail;
                if (detail == null || detail.Id != dt.Id)
                {
                    continue;
                }
                dgvBillDetail.Rows[i].Tag = dt;
                dgvBillDetail[this.colBalMoney.Name, i].Value = dt.BalanceTotalPrice;
                dgvBillDetail[this.colBalMoney.Name, i].Style.ForeColor = Color.Red;
                dgvBillDetail[this.colBalMoney.Name, i].ToolTipText = "数据发生变化，请注意保存数据";

                dgvBillDetail[this.colBalPrice.Name, i].Value = dt.BalancePrice;
                dgvBillDetail[this.colBalPrice.Name, i].Style.ForeColor = Color.Red;
                dgvBillDetail[this.colBalPrice.Name, i].ToolTipText = "数据发生变化，请注意保存数据";

                dgvBillDetail[this.colBalQty.Name, i].Value = dt.BalacneQuantity;

                dgvBillDetail.ShowCellToolTips=true; 
                break;
            }

            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].Value = bill.BalanceMoney = totalMoney;
            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].Style.ForeColor = Color.Red;
            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].ToolTipText = "数据发生变化，请注意保存数据";
            dgvBills.ShowCellToolTips = true; 
            dgvBills.Refresh();
        }

        private void AccountMeansureFee()
        {
            if (curBillMaster == null)
                return;

            List<SubContractBalanceDetail> listMeansureFeeDtl = (from d in curBillMaster.Details.OfType<SubContractBalanceDetail>()
                                                                 where d.FontBillType == FrontBillType.措施
                                                                 select d).ToList();

            if (listMeansureFeeDtl.Count == 0)
                return;

            IEnumerable<SubContractBalanceDetail> listCurrBalDtl = from d in curBillMaster.Details.OfType<SubContractBalanceDetail>()
                                                                   where d.FontBillType == FrontBillType.工程任务核算
                                                                   select d;

            foreach (SubContractBalanceDetail dtl in listMeansureFeeDtl)
            {
                //过滤纳入分包取费范围内的分包结算明细
                //1.过滤指定措施费明细下的结算明细
                IEnumerable<SubContractBalanceDetail> listBalanceDtl = from b in listCurrBalDtl
                                                                       where b.BalanceTaskSyscode != null && b.BalanceTaskSyscode.IndexOf(dtl.BalanceTaskSyscode) > -1
                                                                       select b;

                List<SubContractBalanceDetail> listOptBalDtl = new List<SubContractBalanceDetail>();

                //2.过滤成本项分类
                if (!string.IsNullOrEmpty(dtl.BalanceTaskDtl.TheCostItem.CateFilterSysCode1))
                {
                    listOptBalDtl.AddRange((from b in listBalanceDtl
                                            where b.BalanceTaskDtl.TheCostItemCateSyscode.IndexOf(dtl.BalanceTaskDtl.TheCostItem.CateFilterSysCode1) > -1
                                            select b).ToArray());
                }
                if (!string.IsNullOrEmpty(dtl.BalanceTaskDtl.TheCostItem.CateFilterSysCode2))
                {
                    listOptBalDtl.AddRange((from b in listBalanceDtl
                                            where b.BalanceTaskDtl.TheCostItemCateSyscode.IndexOf(dtl.BalanceTaskDtl.TheCostItem.CateFilterSysCode2) > -1
                                            select b).ToArray());
                }

                listOptBalDtl.Distinct();


                //3.过滤核算科目
                if (listBalanceDtl.Count() > 0)
                {
                    List<SubContractBalanceSubjectDtl> listMeansureFeeSubDtl = new List<SubContractBalanceSubjectDtl>();

                    foreach (SubContractBalanceDetail balDtl in listOptBalDtl)
                    {
                        if (dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter1 != null || dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter2 != null || dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter3 != null)
                        {
                            if (dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter1 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter1.Id
                                                               select d);
                            }
                            if (dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter2 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter2.Id
                                                               select d);
                            }
                            if (dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter3 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.BalanceTaskDtl.TheCostItem.SubjectCateFilter3.Id
                                                               select d);
                            }
                        }
                        else
                        {
                            listMeansureFeeSubDtl.AddRange(balDtl.Details);
                        }
                    }

                    listMeansureFeeSubDtl.Distinct();


                    dtl.BalacneQuantity = 1;
                    dtl.BalancePrice = listMeansureFeeSubDtl.Sum(d => d.BalanceTotalPrice) * dtl.BalanceTaskDtl.SubContractStepRate;
                    dtl.BalanceTotalPrice = dtl.BalacneQuantity * dtl.BalancePrice;

                    //扣税,在生成实体结算明细时已扣
                    //if (subProject.LaborMoneyType == ManagementRememberMethod.按费率计取)
                    //{
                    //    newBalDtl.BalancePrice = newBalDtl.BalancePrice - (newBalDtl.BalancePrice * subProject.LaobrRace);
                    //    newBalDtl.BalanceTotalPrice = newBalDtl.BalanceTotalPrice - (newBalDtl.BalanceTotalPrice * subProject.LaobrRace);
                    //}

                    ObjectQuery oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    foreach (SubContractBalanceSubjectDtl subDtl in dtl.Details)
                    {
                        dis.Add(Expression.Eq("Id", subDtl.FrontBillGUID));
                    }
                    oq.AddCriterion(dis);
                    IEnumerable<GWBSDetailCostSubject> listUsage = 
                        subContractOperate.ObjectQuery(typeof(GWBSDetailCostSubject), oq).OfType<GWBSDetailCostSubject>();

                    foreach (SubContractBalanceSubjectDtl subDtl in dtl.Details)
                    {
                        var query = from u in listUsage
                                    where u.Id == subDtl.FrontBillGUID
                                    select u;

                        decimal AssessmentRate = 0;
                        if (query.Count() > 0)
                        {
                            AssessmentRate = query.ElementAt(0).AssessmentRate;
                        }
                        subDtl.BalanceQuantity = 1;
                        subDtl.BalancePrice = dtl.BalancePrice * AssessmentRate;
                        subDtl.BalanceTotalPrice = subDtl.BalanceQuantity * subDtl.BalancePrice;
                    }
                }

                //更新界面
                foreach (DataGridViewRow row in dgvBillDetail.Rows)
                {
                    SubContractBalanceDetail balDtl = row.Tag as SubContractBalanceDetail;
                    if (balDtl.BalanceTaskDtl != null && balDtl.BalanceTaskDtl.Id == dtl.BalanceTaskDtl.Id)
                    {
                        row.Cells[colBalQty.Name].Value = dtl.BalacneQuantity;
                        row.Cells[colBalPrice.Name].Value = dtl.BalancePrice;
                        row.Cells[colBalMoney.Name].Value = dtl.BalanceTotalPrice;
                        row.Tag = dtl;
                    }
                }
            }
        }

        private decimal GetNewRate(string rateStr)
        {
            rateStr = rateStr.Replace("%", "");
            decimal tmp = 0;
            decimal.TryParse(rateStr, out tmp);
            if (tmp > 1)
            {
                return tmp/100;
            }
            else
            {
                return tmp;
            }
        }

        private void RefreshSumData(FrontBillType billType)
        {
            bool isAccountMeansureFee = true;//是否计算措施费
            bool isAccountLaborMoney = true;//是否计算税金(当直接修改税金时无需计算)
            bool isAccountUtilitiesAndMngFee = true;//是否计算水电建管费(当直接修改水电、建管费时无需计算)

            if (billType == FrontBillType.措施)
            {
                isAccountMeansureFee = false;
            }
            else if (billType == FrontBillType.税金)
            {
                isAccountMeansureFee = false;
                isAccountLaborMoney = false;
            }
            else if (billType == FrontBillType.水电 || billType == FrontBillType.建管)
            {
                isAccountMeansureFee = false;
                isAccountLaborMoney = false;
                isAccountUtilitiesAndMngFee = false;
            }

            //计算措施费
            if (isAccountMeansureFee)
                AccountMeansureFee();

            decimal sumBalMoney = 0;
            var subProject = curBillMaster.TheSubContractProject;
            //计算税金
            if (isAccountLaborMoney)
            {
                sumBalMoney = (from d in curBillMaster.Details.OfType<SubContractBalanceDetail>()
                               where d.FontBillType != FrontBillType.水电 && d.FontBillType != FrontBillType.建管 && d.FontBillType != FrontBillType.税金
                               select d).Sum(d => d.BalanceTotalPrice);
                if (subProject.LaborMoneyType == ManagementRememberMethod.按费率计取)
                {
                    foreach (DataGridViewRow row in dgvBillDetail.Rows)
                    {
                        SubContractBalanceDetail billDtl = row.Tag as SubContractBalanceDetail;
                        if (billDtl.FontBillType == FrontBillType.税金)
                        {
                            billDtl.BalancePrice = sumBalMoney * subProject.LaobrRace;
                            billDtl.BalanceTotalPrice = billDtl.BalancePrice;

                            if (billDtl.Details != null && billDtl.Details.Count > 0)
                            {
                                SubContractBalanceSubjectDtl dtlUsage = billDtl.Details.ElementAt(0);
                                dtlUsage.BalancePrice = billDtl.BalancePrice;
                                dtlUsage.BalanceTotalPrice = dtlUsage.BalancePrice;
                            }

                            row.Tag = billDtl;
                            row.Cells[colBalPrice.Name].Value = billDtl.BalancePrice;
                            row.Cells[colBalMoney.Name].Value = billDtl.BalanceTotalPrice;
                            break;
                        }
                    }
                }
            }

            //水电、建管费在计算税金后计算
            if (isAccountUtilitiesAndMngFee)
            {
                sumBalMoney = (from d in curBillMaster.Details.OfType<SubContractBalanceDetail>()
                               where d.FontBillType != FrontBillType.水电 && d.FontBillType != FrontBillType.建管
                               select d).Sum(d => d.BalanceTotalPrice);

                foreach (DataGridViewRow row in dgvBillDetail.Rows)
                {
                    SubContractBalanceDetail billDtl = row.Tag as SubContractBalanceDetail;
                    if (billDtl.FontBillType == FrontBillType.水电 && subProject.UtilitiesRemMethod == UtilitiesRememberMethod.按费率计取)
                    {
                        billDtl.BalancePrice = -Math.Abs(sumBalMoney * subProject.UtilitiesRate);
                        billDtl.BalanceTotalPrice = billDtl.BalancePrice;

                        if (billDtl.Details != null && billDtl.Details.Count > 0)
                        {
                            SubContractBalanceSubjectDtl dtlUsage = billDtl.Details.ElementAt(0);
                            dtlUsage.BalancePrice = billDtl.BalancePrice;
                            dtlUsage.BalanceTotalPrice = dtlUsage.BalancePrice;
                        }

                        row.Tag = billDtl;
                        row.Cells[colBalPrice.Name].Value = billDtl.BalancePrice;
                        row.Cells[colBalMoney.Name].Value = billDtl.BalanceTotalPrice;
                    }
                    else if (billDtl.FontBillType == FrontBillType.建管 && subProject.ManagementRemMethod == ManagementRememberMethod.按费率计取)
                    {
                        billDtl.BalancePrice = -Math.Abs(sumBalMoney * subProject.ManagementRate);
                        billDtl.BalanceTotalPrice = billDtl.BalancePrice;

                        if (billDtl.Details != null && billDtl.Details.Count > 0)
                        {
                            SubContractBalanceSubjectDtl dtlUsage = billDtl.Details.ElementAt(0);
                            dtlUsage.BalancePrice = billDtl.BalancePrice;
                            dtlUsage.BalanceTotalPrice = dtlUsage.BalancePrice;
                        }

                        row.Tag = billDtl;
                        row.Cells[colBalPrice.Name].Value = billDtl.BalancePrice;
                        row.Cells[colBalMoney.Name].Value = billDtl.BalanceTotalPrice;
                    }
                }
            }

            sumBalMoney = curBillMaster.Details.OfType<SubContractBalanceDetail>().Sum(d => d.BalanceTotalPrice);//总金额
            curBillMaster.BalanceMoney = sumBalMoney;//更新结算金额，在右键删除的时候直接用于计算合价
            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].Value = sumBalMoney;
            dgvBills.Refresh();
        }

        //回写工程任务核算单核算数量、金额、进度
        private void WriteBackProjectAccountDetail(SubContractBalanceBill bill)
        {
            if (rateChanges == null || rateChanges.Count == 0 || accDetailCache == null)
            {
                return;
            }

            var list = new List<ProjectTaskDetailAccount>();
            var subDetails = bill.Details.OfType<SubContractBalanceDetail>().ToList();
            foreach (var change in rateChanges)
            {
                if(!accDetailCache.ContainsKey(change.Key))
                {
                    continue;
                }

                var det = accDetailCache[change.Key];
                if (det.CurrAccFigureProgress == change.Value)
                {
                    continue;
                }

                var subDet = subDetails.Find(s => s.FrontBillGUID.Equals(change.Key));
                if (subDet == null)
                {
                    continue;
                }

                det.AccountProjectAmount = subDet.BalacneQuantity;
                det.AccountTotalPrice = subDet.BalanceTotalPrice;
                det.CurrAccFigureProgress = change.Value;
                foreach (var subjectDet in det.Details)
                {
                    subjectDet.AccountQuantity = det.AccountProjectAmount;
                    subjectDet.AccountTotalPrice = subjectDet.AccountQuantity * subjectDet.AccountPrice;
                }

                list.Add(det);
            }

            subContractOperate.SubBalSrv.SaveOrUpdateSubContractBalanceBill(list);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DateTime balanceStartTime = dtpBalanceDate.BeginValue.Date;
            DateTime balanceEndTime = dtpBalanceDate.EndValue.Date.AddDays(1).AddSeconds(-1);

            if (balanceStartTime > balanceEndTime)
            {
                MessageBox.Show("结算起始时间不能大于结束时间！");
                dtpBalanceDate.Focus();
                return;
            }
            FlashScreen.Show("正在生成，请稍候...");

            DateTime time = dtpBusinessDate.Value.Date;
            var curBillMaster = new SubContractBalanceBill();
            curBillMaster = new SubContractBalanceBill();
            curBillMaster.Code = string.Format("{0:yyyyMMddHHmmss}", time);
            curBillMaster.DocState = DocumentState.Edit;
            curBillMaster.CreateDate = ClientUtil.ToDateTime(time.ToShortDateString());
            curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
            curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
            curBillMaster.CreateYear = curBillMaster.CreateDate.Year;
            curBillMaster.CreateMonth = curBillMaster.CreateDate.Month;
            curBillMaster.ProjectId = projectInfo.Id;
            curBillMaster.ProjectName = projectInfo.Name;

            curBillMaster.OperOrgInfo = ConstObject.TheOperationOrg;
            curBillMaster.OperOrgInfoName = curBillMaster.OperOrgInfo.Name;
            curBillMaster.OpgSysCode = curBillMaster.OperOrgInfo.SysCode;
            curBillMaster.HandlePersonName = curBillMaster.CreatePersonName;

            curBillMaster.BalanceRange = (txtRootNode.Tag as GWBSTree);
            curBillMaster.BalanceTaskName = txtRootNode.Text;
            curBillMaster.BalanceTaskSyscode = curBillMaster.BalanceRange.SysCode;

            curBillMaster.BeginTime = balanceStartTime;
            curBillMaster.EndTime = balanceEndTime;

            var subProject = txtBalOrg.Tag as SubContractProject;
            if (subProject != null)
            {
                curBillMaster.TheSubContractProject = subProject;
            }

            var list = subContractOperate.SubBalSrv.GenerateProjectSubContractBalance(curBillMaster, projectInfo);
            if (list!=null && list.Count > 0)
            {
                LoadCreatedBills();
                FlashScreen.Close();
            }
            else
            {
                FlashScreen.Close();
                MessageBox.Show("此结算期间内没有已审核单据可结算");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBills.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的结算单");
                return;
            }

            var bill = dgvBills.SelectedRows[0].DataBoundItem as SubContractBalanceBill;
            if (bill == null)
            {
                MessageBox.Show("没有获取到所选单据信息");
                return;
            }

            if (bill.DocState != DocumentState.Edit)
            {
                MessageBox.Show(string.Format("结算单【{0}】不可编辑，不能删除", bill.Code));
                return;
            }

            if (MessageBox.Show("您确认要删除结算单【" + bill.Code + "】？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            FlashScreen.Show("数据删除中，请稍候...");

            if(subContractOperate.SubBalSrv.DeleteProjectSubContractBalance(bill))
            {
                LoadCreatedBills();
            }

            FlashScreen.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (dgvBills.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择编辑中的结算单");
                return;
            }

            var bill = dgvBills.SelectedRows[0].DataBoundItem as SubContractBalanceBill;
            if (bill == null)
            {
                MessageBox.Show("没有获取到所选单据信息");
                return;
            }
            else if (bill.DocState != DocumentState.Edit)
            {
                MessageBox.Show("请选择编辑中的结算单");
                return;
            }

            if (
                MessageBox.Show("您确认要提交结算单【" + bill.Code + "】？", "提交确认", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            FlashScreen.Show("处理中，请稍候...");
            if (SaveOrSubmit(bill, 2))
            {
                FlashScreen.Close();

                MessageBox.Show("提交成功");

                LoadCreatedBills();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvBills.SelectedRows.Count <= 0 || curBillMaster == null)
            {
                MessageBox.Show("请选择要修改的单据");
                return;
            }

            var isNeedRefresh = curBillMaster.CreateDate.Month != dtpBusinessDate.Value.Date.Month || rateChanges.Count > 0;
            if (SaveOrSubmit(curBillMaster, 1))
            {
                MessageBox.Show("保存成功");

                if (isNeedRefresh)
                {
                    LoadCreatedBills();
                }
                else
                {
                    dgvBillDetail.Refresh();
                }
            }
        }

        private void btnSetBalanceBase_Click(object sender, EventArgs e)
        {
            if (curBillMaster == null || curBillMaster.DocState != DocumentState.Edit)
            {
                MessageBox.Show("请选择编辑中的分包结算单");
                return;
            }

            if (dgvBillDetail.RowCount == 0)
            {
                MessageBox.Show("没有明细数据可以设置");
                return;
            }

            var baseVal = cmbBalanceBase.Text.Trim();
            if (string.IsNullOrEmpty(baseVal))
            {
                MessageBox.Show("请选择结算依据项");
                groupbox1.Focus();
                return;
            }

            for (int i = 0; i < dgvBillDetail.RowCount; i++)
            {
                var oldBase = dgvBillDetail.Rows[i].Cells[colBalanceBase.Name].Value;
                if (oldBase != null && arrBalBaseReadOnly.Contains(oldBase))
                {
                    continue;
                }

                var detail = dgvBillDetail.Rows[i].Tag as SubContractBalanceDetail;
                if (detail == null)
                {
                    continue;
                }

                dgvBillDetail.Rows[i].Cells[colBalanceBase.Name].Value = detail.BalanceBase = baseVal;
            }
        }

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            List<int> listSelRowIndex = new List<int>();
            foreach (DataGridViewRow row in dgvBillDetail.SelectedRows)
            {
                listSelRowIndex.Add(row.Index);
            }
            listSelRowIndex.Sort();
            if (listSelRowIndex.Count == 0)
            {
                MessageBox.Show("请选择要删除的明细记录");
                return;
            }

            if (
                MessageBox.Show("确定要删除选中的记录？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            bool isChanged = false;
            for (int i = listSelRowIndex.Count - 1; i > -1; i--)
            {
                int rowIndex = listSelRowIndex[i];
                DataGridViewRow dr = dgvBillDetail.Rows[rowIndex];
                var detail = dr.Tag as SubContractBalanceDetail;
                if (detail == null)
                {
                    continue;
                }

                if (detail.FontBillType == FrontBillType.建管 || detail.FontBillType == FrontBillType.水电
                    || detail.FontBillType == FrontBillType.税金)
                {
                    continue;
                }

                dgvBillDetail.Rows.Remove(dr);

                curBillMaster.Details.Remove(detail);
                curBillMaster.BalanceMoney = curBillMaster.BalanceMoney - detail.BalanceTotalPrice;
                isChanged = true;
            }

            if (isChanged)
            {
                RefreshSumData(FrontBillType.工程任务核算);

                MessageBox.Show("数据已发生变化，请注意保存修改");
            }
            else
            {
                MessageBox.Show("请选择前驱单据类型不是建管、水电、税金的记录行");
            }
        }

        private void btnSelectBalOrg_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vSubProject = new VContractExcuteSelector();
            vSubProject.ShowDialog();
            IList list = vSubProject.Result;
            if (list == null || list.Count == 0) return;

            var subProject = list[0] as SubContractProject;
            this.txtBalOrg.Text = subProject.BearerOrgName;
            this.txtBalOrg.Tag = subProject;
        }

        private void btnClearBalOrg_Click(object sender, EventArgs e)
        {
            txtBalOrg.Clear();
            txtBalOrg.Tag = null;
        }

        private void dgvBillDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var detail = dgvBillDetail.Rows[e.RowIndex].Tag as SubContractBalanceDetail;
            if (detail == null)
            {
                return;
            }

            if (e.ColumnIndex != colUseDescript.Index)
            {
                VSubContractBalanceSubject dlg = new VSubContractBalanceSubject(detail);
                dlg.AfterBalanceValueChanged += AfterBalanceValueChanged;
                dlg.ShowDialog();
            }
            else
            {
                var isRead = !btnSave.Enabled;
                VTextEditDialog dlg = new VTextEditDialog(detail.Remarks, isRead);
                dlg.Owner = this.FindForm();
                if (dlg.ShowDialog() == DialogResult.OK && !isRead)
                {
                    detail.Remarks = dlg.RemarkText;
                    dgvBillDetail.Rows[e.RowIndex].Cells[colUseDescript.Name].Value = dlg.RemarkText;
                    dgvBillDetail.Rows[e.RowIndex].Cells[colUseDescript.Name].Style.ForeColor = Color.Red;
                    dgvBillDetail.Rows[e.RowIndex].Cells[colUseDescript.Name].ToolTipText = "数据发生变化，请注意保存数据";
                    dgvBillDetail.ShowCellToolTips = true; 
                }
            }
        }

        private void dgvBillDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && !dgvBillDetail.Rows[e.RowIndex].ReadOnly)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = dgvBillDetail.Columns[e.ColumnIndex].Name;
                        if (colName == colBalQty.Name || colName == colBalPrice.Name)
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输入格式不正确！");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void dgvBillDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object tempValue = dgvBillDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                {
                    value = tempValue.ToString().Trim();
                }

                SubContractBalanceDetail dtl = dgvBillDetail.Rows[e.RowIndex].Tag as SubContractBalanceDetail;
                if (dgvBillDetail.Columns[e.ColumnIndex].Name == colBalQty.Name)//结算数量
                {
                    dtl.BalacneQuantity = value == "" ? 0 : ClientUtil.ToDecimal(value);
                    
                    if (dtl.BalancePrice != 0)
                    {
                        dtl.BalanceTotalPrice = dtl.BalacneQuantity * dtl.BalancePrice;
                        dgvBillDetail.Rows[e.RowIndex].Cells[colBalMoney.Name].Value = dtl.BalanceTotalPrice;
                    }
                    else if (dtl.BalacneQuantity != 0)
                    {
                        dtl.BalancePrice = dtl.BalanceTotalPrice / dtl.BalacneQuantity;
                        dgvBillDetail.Rows[e.RowIndex].Cells[colBalPrice.Name].Value = dtl.BalancePrice;
                    }

                    //更新耗用数量和合价
                    foreach (SubContractBalanceSubjectDtl subDtl in dtl.Details)
                    {
                        subDtl.BalanceQuantity = dtl.BalacneQuantity;
                        subDtl.BalanceTotalPrice = subDtl.BalanceQuantity * subDtl.BalancePrice;
                    }

                    if (accDetailCache.ContainsKey(dtl.FrontBillGUID) && dgvBillDetail[colCurrentRate.Name, e.RowIndex].Tag != null)
                    {
                        var oldQuantity = (decimal) dgvBillDetail[colBalQty.Name, e.RowIndex].Tag;
                        var oldRate = (decimal) dgvBillDetail[colCurrentRate.Name, e.RowIndex].Tag;
                        if (oldQuantity != dtl.BalacneQuantity && oldQuantity != 0)
                        {
                            var frontBill = accDetailCache[dtl.FrontBillGUID];
                            if (rateChanges.ContainsKey(dtl.FrontBillGUID))
                            {
                                rateChanges[dtl.FrontBillGUID] = oldRate*dtl.BalacneQuantity/oldQuantity;
                            }
                            else
                            {
                                rateChanges.Add(dtl.FrontBillGUID,
                                                frontBill.CurrAccFigureProgress*dtl.BalacneQuantity/oldQuantity);
                            }

                            dgvBillDetail.Rows[e.RowIndex].Cells[colCurrentRate.Name].Value =
                                rateChanges[dtl.FrontBillGUID];
                        }
                    }
                }
                else if (dgvBillDetail.Columns[e.ColumnIndex].Name == colBalPrice.Name)//结算单价
                {
                    dtl.BalancePrice = value == "" ? 0 : ClientUtil.ToDecimal(value);

                    if (dtl.BalacneQuantity != 0)
                    {
                        dtl.BalanceTotalPrice = dtl.BalacneQuantity * dtl.BalancePrice;
                        dgvBillDetail.Rows[e.RowIndex].Cells[colBalMoney.Name].Value = dtl.BalanceTotalPrice;
                    }
                    else if (dtl.BalancePrice != 0)
                    {
                        dtl.BalacneQuantity = dtl.BalanceTotalPrice / dtl.BalancePrice;
                        dgvBillDetail.Rows[e.RowIndex].Cells[colBalQty.Name].Value = dtl.BalacneQuantity;
                    }
                }
                else if(e.ColumnIndex == colBalanceBase.Index)
                {
                    dtl.BalanceBase = value;
                }
                else if (e.ColumnIndex == colCurrentRate.Index && accDetailCache.ContainsKey(dtl.FrontBillGUID))
                {
                    var frontBill = accDetailCache[dtl.FrontBillGUID];
                    var newRate = GetNewRate(value);
                    if (newRate > 1)
                    {
                        newRate = 1;
                    }
                    var oldQuantity = (decimal)dgvBillDetail[colBalQty.Name, e.RowIndex].Tag;
                    var oldTotalPrice = (decimal)dgvBillDetail[colBalMoney.Name, e.RowIndex].Tag;

                    dtl.BalacneQuantity = oldQuantity / frontBill.CurrAccFigureProgress * newRate;
                    dtl.BalanceTotalPrice = oldTotalPrice / frontBill.CurrAccFigureProgress * newRate;

                    //更新耗用数量和合价
                    foreach (SubContractBalanceSubjectDtl subDtl in dtl.Details)
                    {
                        subDtl.BalanceQuantity = dtl.BalacneQuantity;
                        subDtl.BalanceTotalPrice = subDtl.BalanceQuantity * subDtl.BalancePrice;
                    }

                    dgvBillDetail.Rows[e.RowIndex].Cells[colCurrentRate.Name].Value = newRate;
                    dgvBillDetail.Rows[e.RowIndex].Cells[colBalQty.Name].Value = dtl.BalacneQuantity;
                    dgvBillDetail.Rows[e.RowIndex].Cells[colBalMoney.Name].Value = dtl.BalanceTotalPrice;
                    if (rateChanges.ContainsKey(dtl.FrontBillGUID))
                    {
                        rateChanges[dtl.FrontBillGUID] = newRate;
                    }
                    else
                    {
                        rateChanges.Add(dtl.FrontBillGUID, newRate);
                    }
                }

                dgvBillDetail.Rows[e.RowIndex].Tag = dtl;

                RefreshSumData(dtl.FontBillType);
            }
        }

        private void dgvBills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            rateChanges.Clear();
            dgvBillDetail.Rows.Clear();
            SubContractBalanceBill master = dgvBills.Rows[e.RowIndex].DataBoundItem as SubContractBalanceBill;
            if (master == null)
            {
                return;
            }

            FlashScreen.Show("明细加载中，请稍候...");

            curBillMaster = master;

            btnSetBalanceBase.Enabled =
                btnDeleteDetail.Enabled =
                btnDelete.Enabled =
                btnSave.Enabled =
                btnSubmit.Enabled =
                master.DocState == DocumentState.Edit;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            oq.AddFetchMode("BalanceTaskDtl", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("BalanceTaskDtl.TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList subContractBalanceDetailList = subContractOperate.ObjectQuery(typeof(SubContractBalanceDetail), oq);
            
            master.Details.Clear();
            var totalMoney = 0m;
            //分包结算明细
            foreach (SubContractBalanceDetail detail in subContractBalanceDetailList)
            {
                master.Details.Add(detail);

                int rowIndex = dgvBillDetail.Rows.Add();
                dgvBillDetail.Rows[rowIndex].Tag = detail;
                dgvBillDetail[this.colDTaskNode.Name, rowIndex].Value = detail.BalanceTaskName;
                dgvBillDetail[this.colDTaskMx.Name, rowIndex].Value = detail.BalanceTaskDtlName;
                dgvBillDetail[this.colBalMoney.Name, rowIndex].Value = detail.BalanceTotalPrice;
                dgvBillDetail[this.colBalMoney.Name, rowIndex].Tag = detail.BalanceTotalPrice;
                dgvBillDetail[this.colBalPrice.Name, rowIndex].Value = detail.BalancePrice;
                dgvBillDetail[this.colBalQty.Name, rowIndex].Value = detail.BalacneQuantity;
                dgvBillDetail[this.colBalQty.Name, rowIndex].Tag = detail.BalacneQuantity;
                dgvBillDetail[this.colForwardType.Name, rowIndex].Value = detail.FontBillType.ToString();
                dgvBillDetail[this.DtlHandlePerson.Name, rowIndex].Value = detail.HandlePersonName;
                dgvBillDetail[this.colQtyUnit.Name, rowIndex].Value = detail.QuantityUnitName;
                dgvBillDetail[this.colPriceUnit.Name, rowIndex].Value = detail.PriceUnitName;
                dgvBillDetail[this.colUseDescript.Name, rowIndex].Value = detail.Remarks;//备注

                if (detail.BalanceTaskDtl != null && detail.BalanceTaskDtl.Id != "0")
                {
                    dgvBillDetail[this.colMainResourceName.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeName;
                    dgvBillDetail[this.colMainResourceSpec.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeSpec;
                    dgvBillDetail[this.colDigramNumber.Name, rowIndex].Value = detail.BalanceTaskDtl.DiagramNumber;
                    if (detail.BalanceTaskDtl.TheCostItem != null)
                        dgvBillDetail[this.colCostItemQuotaCode.Name, rowIndex].Value = detail.BalanceTaskDtl.TheCostItem.QuotaCode;
                }

                dgvBillDetail[colBalanceBase.Name, rowIndex].Value = detail.BalanceBase;
                if (!string.IsNullOrEmpty(detail.BalanceBase) && arrBalBaseReadOnly.Contains(detail.BalanceBase))
                {
                    dgvBillDetail[colBalanceBase.Name, rowIndex].ReadOnly = true;
                    dgvBillDetail[colBalanceBase.Name, rowIndex].Style.BackColor = Color.Silver;
                }
                else
                {
                    dgvBillDetail[colBalanceBase.Name, rowIndex].ReadOnly = false;
                    dgvBillDetail[colBalanceBase.Name, rowIndex].Style.BackColor = Color.White;
                }

                if (detail.FontBillType == FrontBillType.建管 || detail.FontBillType == FrontBillType.水电
                || detail.FontBillType == FrontBillType.税金 || detail.FontBillType == FrontBillType.措施)
                {
                    dgvBillDetail.Rows[rowIndex].Cells[colBalQty.Name].ReadOnly = true;
                    dgvBillDetail.Rows[rowIndex].Cells[colBalQty.Name].Style.BackColor = Color.Silver;
                }
                else
                {
                    dgvBillDetail.Rows[rowIndex].Cells[colBalQty.Name].ReadOnly = false;
                    dgvBillDetail.Rows[rowIndex].Cells[colBalQty.Name].Style.BackColor = Color.White;
                }
           
                totalMoney += detail.BalanceTotalPrice;

                if(detail.FontBillType == FrontBillType.工程任务核算)
                {
                    if (!accDetailCache.ContainsKey(detail.FrontBillGUID))
                    {
                        var accDetail = GetProjectTaskDetailAccountById(detail.FrontBillGUID);
                        accDetailCache.Add(detail.FrontBillGUID, accDetail);
                    }
                    if (accDetailCache[detail.FrontBillGUID] != null)
                    {
                        dgvBillDetail[colCurrentRate.Name, rowIndex].Value =
                            accDetailCache[detail.FrontBillGUID].CurrAccFigureProgress;
                        dgvBillDetail[colCurrentRate.Name, rowIndex].Tag =
                            accDetailCache[detail.FrontBillGUID].CurrAccFigureProgress;
                        dgvBillDetail[colDealRate.Name, rowIndex].Value =
                            accDetailCache[detail.FrontBillGUID].AddupAccountProgress/100;
                    }
                    dgvBillDetail[colCurrentRate.Name, rowIndex].ReadOnly = false;
                }
                else
                {
                    dgvBillDetail[colCurrentRate.Name, rowIndex].ReadOnly = true;
                }
            }
            if(curBillMaster.DocState != DocumentState.Edit)
            {
                dgvBillDetail.ReadOnly = true;
            }

            if (master.BalanceMoney != totalMoney)
            {
                dgvBills.Rows[e.RowIndex].Cells[colBillBalanceMoney.Name].Value = master.BalanceMoney = totalMoney;
            }
            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].Style.ForeColor = Color.Black;
            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].ToolTipText = string.Empty;

            FlashScreen.Close();
        }

        private void dgvBills_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            var bill = dgvBills.Rows[e.RowIndex].DataBoundItem as SubContractBalanceBill;
            if (bill != null)
            {
                dgvBills.Rows[e.RowIndex].Cells[colBillState.Name].Value = ClientUtil.GetDocStateName(bill.DocState);

                if (bill.DocState != DocumentState.Edit)
                {
                    dgvBills.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    dgvBills.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Green;
                }
            }
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCreatedBills();
        }
    }
}
