using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Drawing;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Linq;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubContractBalanceBill : TMasterDetailView
    {
        public bool IsRefresh = false;
        public MSubContractBalance model = new MSubContractBalance();
        private SubContractBalanceBill curBillMaster = null;
        private SubContractProject subProject = new SubContractProject();
        private SubContractBalanceBill allSubBalanceBill = null;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        string[] arrBalBaseReadOnly = { "计时工", "代工", "罚款" };
        /// <summary>
        /// 当前单据
        /// </summary>
        public SubContractBalanceBill CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        private IList subjectList = null;//存放 人材机 核算科目
        public VSubContractBalanceBill()
        {
            InitializeComponent();
            InitForm();
            RefreshControls(MainViewState.Browser);
        }

        private void InitForm()
        {
            InitEvents();
            projectInfo = StaticMethod.GetProjectInfo();
            DateTime serverTime = model.GetServerTime();
            dtAccountStartTime.Value = serverTime.AddDays(-7);
            dtAccountEndDate.Value = serverTime;
            this.gridDetail.ContextMenuStrip = cmsDg;
            //this.DtlBalBase.Items.AddRange(new object[] { "合同价", "定额价", "协议价", "参考价", "指导价", "其它" });
            this.DtlBalBase.Items.AddRange(new object[] { "合同内", "分包签证", "计时工", "代工", "罚款", "设计变更", "签证变更" });
            
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            dis.Add(Expression.Eq("Code", "C5110121"));
            dis.Add(Expression.Eq("Code", "C5110122"));
            dis.Add(Expression.Eq("Code", "C5110123"));
            oq.AddCriterion(dis);
            subjectList = model.ObjectQuery(typeof(CostAccountSubject), oq);
        }

        private void InitEvents()
        {
            btnSelectBalanceTaskRootNode.Click += new EventHandler(btnSelectBalanceTaskRootNode_Click);
            btnSelectBalOrg.Click += new EventHandler(btnSelectBalOrg_Click);
            btnGeneBalanceBill.Click += new EventHandler(btnGeneBalanceBill_Click);
            btnBalanceMeasureFee.Click += new EventHandler(btnBalanceMeasureFee_Click);
            btnAccountMeansureFee.Click += new EventHandler(btnAccountMeansureFee_Click);

            gridDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridDetail_CellDoubleClick);
            gridDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridDetail_CellValidating);
            gridDetail.CellEndEdit += new DataGridViewCellEventHandler(gridDetail_CellEndEdit);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            tsmiAdd.Click += new EventHandler(tsmiAdd_Click);
            this.tsmiIsBalance.Click+=new EventHandler(tsmiIsBalance_Click);
            btnSetQuality.Click += new EventHandler(btnSetQuality_Click);
            this.btnSubjectUpdate.Click += new EventHandler(btnSubjectUpdate_Click);
            this.btnBatchPrice.Click += new EventHandler(btnBatchPrice_Click);
           
        }
        void btnSubjectUpdate_Click(object sender, EventArgs e)
        {
            if (curBillMaster == null || ClientUtil.ToString(curBillMaster.Id) == "")
            {
                MessageBox.Show("请先保存此结算单！");
                return;
            }
            VSubBalSubjectBatchUpdate ssUpdate = new VSubBalSubjectBatchUpdate(curBillMaster.Id);
            ssUpdate.ShowDialog();
        }
        void btnSetQuality_Click(object sender, EventArgs e)
        {
            if (this.gridDetail.CurrentRow != null)
            {
                string sBalBase = ClientUtil.ToString( this.gridDetail.CurrentRow.Cells[DtlBalBase.Name].Value);
                if (!string.IsNullOrEmpty(sBalBase))
                {
                    foreach (DataGridViewRow var in this.gridDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        if (var.Cells[DtlBalBase.Name].ReadOnly) continue;
                        var.Cells[DtlBalBase.Name].Value = sBalBase;
                        //if (ClientUtil.ToString(gridDetail[DtlBalBase.Name, 0].Value) != "")
                        //{
                        //    var.Cells[DtlBalBase.Name].Value = gridDetail[DtlBalBase.Name, 0].Value;
                        //}
                    }
                }
            }
        }
        #region 事件
        void tsmiIsBalance_Click(object sender, EventArgs e)
        {
            if (this.gridDetail.CurrentRow != null&& gridDetail.CurrentRow!=null)
            {
                SubContractBalanceDetail oDetail = this.gridDetail.CurrentRow.Tag as SubContractBalanceDetail;
                if (oDetail != null && oDetail.FontBillType == FrontBillType.工程任务核算)
                {
                    ProjectTaskDetailAccount oProjectTaskDetailAccount = new ProjectTaskDetailAccount() { Id = oDetail.FrontBillGUID };
                    VSubContractBalanceUpdateMaterial oUpdateIsBalance = new VSubContractBalanceUpdateMaterial(oProjectTaskDetailAccount);
                    
                    oUpdateIsBalance.OptionView = ViewState;
                    oUpdateIsBalance.RefreshControls(oUpdateIsBalance.OptionView);
                    oUpdateIsBalance.StartPosition = FormStartPosition.CenterParent;
                    oUpdateIsBalance.MinimizeBox = false;
                    oUpdateIsBalance.MaximizeBox = false;
                    oUpdateIsBalance.ShowDialog();
                    if (oUpdateIsBalance.IsRefresh)
                    {
                        IsRefresh = IsRefresh || oUpdateIsBalance.IsRefresh;
                    }

                }
            }
        }
        //结算明细删除
        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                List<int> listSelRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridDetail.SelectedRows)
                {
                    listSelRowIndex.Add(row.Index);
                }
                listSelRowIndex.Sort();

                bool isAlert = false;//是否需要提示消息
                bool isRefreshData = false;//是否需要更新数据
                for (int i = listSelRowIndex.Count - 1; i > -1; i--)
                {
                    int rowIndex = listSelRowIndex[i];
                    DataGridViewRow dr = this.gridDetail.Rows[rowIndex];

                    if (dr == null || dr.IsNewRow) return;
                    if (dr.Tag != null)
                    {
                        SubContractBalanceDetail d_dtl = dr.Tag as SubContractBalanceDetail;
                        if (d_dtl.FontBillType == FrontBillType.建管 || d_dtl.FontBillType == FrontBillType.水电 || d_dtl.FontBillType == FrontBillType.税金)
                        {
                            isAlert = true;
                            continue;
                        }

                        isRefreshData = true;

                        curBillMaster.Details.Remove(d_dtl);
                        gridDetail.Rows.Remove(dr);
                        curBillMaster.BalanceMoney = curBillMaster.BalanceMoney - d_dtl.BalanceTotalPrice;
                        this.txtSumMoney.Text = curBillMaster.BalanceMoney + "";
                        this.txtSumQuantity.Text = (ClientUtil.ToDecimal(this.txtSumQuantity.Text) - d_dtl.BalacneQuantity) + "";
                        decimal subjectDtlCoPrice = ClientUtil.ToDecimal(this.txtRCJCoPrice.Text);
                        foreach (SubContractBalanceSubjectDtl dtlConsume in d_dtl.Details)
                        {
                            foreach (CostAccountSubject s in subjectList)
                            {
                                if (dtlConsume.BalanceSubjectGUID.Id == s.Id)
                                {
                                    subjectDtlCoPrice -= dtlConsume.BalanceTotalPrice;
                                    break;
                                }
                            }
                        }
                        txtRCJCoPrice.Text = subjectDtlCoPrice.ToString();
                    }
                }

                RefreshSumData(FrontBillType.工程任务核算);

                if (isAlert)
                    MessageBox.Show("类型为【税金】和【水电】、【建管】的结算明细不能删除！");
            }
        }
        //结算明细增加
        void tsmiAdd_Click(object sender, EventArgs e)
        {
            SubContractBalanceBill addBill = this.GetDiffSubBalBill();
            VSubContractBalDetailSelect frm = new VSubContractBalDetailSelect(addBill);
            frm.ShowDialog();
            IList list = frm.Result;
            if (list == null || list.Count == 0) return;

            foreach (SubContractBalanceDetail detail in list)
            {
                AddBalanceDetailInGrid(detail);
                detail.Master = curBillMaster;
                curBillMaster.Details.Add(detail);
                curBillMaster.BalanceMoney = curBillMaster.BalanceMoney + detail.BalanceTotalPrice;
                this.txtSumQuantity.Text = (ClientUtil.ToDecimal(this.txtSumQuantity.Text) + detail.BalacneQuantity) + "";
                this.txtSumMoney.Text = curBillMaster.BalanceMoney + "";
                decimal subjectDtlCoPrice = ClientUtil.ToDecimal(this.txtRCJCoPrice.Text);
                foreach (SubContractBalanceSubjectDtl dtlConsume in detail.Details)
                {
                    foreach (CostAccountSubject s in subjectList)
                    {
                        if (dtlConsume.BalanceSubjectGUID.Id == s.Id)
                        {
                            subjectDtlCoPrice += dtlConsume.BalanceTotalPrice;
                            break;
                        }
                    }
                }
                txtRCJCoPrice.Text = subjectDtlCoPrice.ToString();
            }
        }
        //选择分包结算根节点
        void btnSelectBalanceTaskRootNode_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtAccountRootNode.Text = task.Name;
                    txtAccountRootNode.Tag = task;
                }
            }
        }
        //选择结算单位
        void btnSelectBalOrg_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vSubProject = new VContractExcuteSelector();
            vSubProject.ShowDialog();
            IList list = vSubProject.Result;
            if (list == null || list.Count == 0) return;

            subProject = list[0] as SubContractProject;
            this.txtBalOrg.Text = subProject.BearerOrgName;
            this.txtBalOrg.Tag = subProject;
        }
        //设置明细的单元格可读属性
        private void SetGridRowReadOnly(bool rowReadOnly)
        {
            //设置编辑的单元格状态
            foreach (DataGridViewRow row in gridDetail.Rows)
            {
                row.ReadOnly = rowReadOnly;
            }
        }
        //明细单元格数据校验
        void gridDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridDetail.Columns[e.ColumnIndex].Name;
                        if (colName == DtlBalanceQuantity.Name || colName == DtlBalancePrice.Name)
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

        void gridDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object tempValue = gridDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                SubContractBalanceDetail dtl = gridDetail.Rows[e.RowIndex].Tag as SubContractBalanceDetail;
                if (gridDetail.Columns[e.ColumnIndex].Name == DtlBalanceQuantity.Name)//结算数量
                {
                    if (value == "")
                    {
                        dtl.BalacneQuantity = 0;
                    }
                    else
                    {
                        dtl.BalacneQuantity = ClientUtil.ToDecimal(value);
                    }
                    if (dtl.ConfirmQuantity < dtl.BalacneQuantity && dtl.ConfirmQuantity > 0)//结算数量必须小于等于确认量
                    {
                 
                        MessageBox.Show(string.Format("结算数量[{0}]必须小于等于工长确认数量[{1}]", dtl.BalacneQuantity, dtl.ConfirmQuantity));
                        gridDetail.BeginEdit(true);
                    }
                    if (dtl.BalancePrice != 0)
                    {
                        dtl.BalanceTotalPrice = dtl.BalacneQuantity * dtl.BalancePrice;
                        gridDetail.Rows[e.RowIndex].Cells[DtlBalanceTotalPrice.Name].Value = dtl.BalanceTotalPrice;
                    }
                    else if (dtl.BalacneQuantity != 0)
                    {
                        dtl.BalancePrice = dtl.BalanceTotalPrice / dtl.BalacneQuantity;
                        gridDetail.Rows[e.RowIndex].Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
                    }

                    //更新耗用数量和合价
                    foreach (SubContractBalanceSubjectDtl subDtl in dtl.Details)
                    {
                        subDtl.BalanceQuantity = dtl.BalacneQuantity;
                        subDtl.BalanceTotalPrice = subDtl.BalanceQuantity * subDtl.BalancePrice;
                    }
                }
                else if (gridDetail.Columns[e.ColumnIndex].Name == DtlBalancePrice.Name)//结算单价
                {
                    if (value == "")
                    {
                        dtl.BalancePrice = 0;
                    }
                    else
                    {
                        dtl.BalancePrice = ClientUtil.ToDecimal(value);
                    }

                    if (dtl.BalacneQuantity != 0)
                    {
                        dtl.BalanceTotalPrice = dtl.BalacneQuantity * dtl.BalancePrice;
                        gridDetail.Rows[e.RowIndex].Cells[DtlBalanceTotalPrice.Name].Value = dtl.BalanceTotalPrice;
                    }
                    else if (dtl.BalancePrice != 0)
                    {
                        dtl.BalacneQuantity = dtl.BalanceTotalPrice / dtl.BalancePrice;
                        gridDetail.Rows[e.RowIndex].Cells[DtlBalanceQuantity.Name].Value = dtl.BalacneQuantity;
                    }
                }

                gridDetail.Rows[e.RowIndex].Tag = dtl;

                RefreshSumData(dtl.FontBillType);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="billType">修改的结算明细类型</param>
        /// <param name="initAccMeansureFee">是否计算措施费初始值</param>
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

            //计算税金
            if (isAccountLaborMoney)
            {
                sumBalMoney = (from d in curBillMaster.Details.OfType<SubContractBalanceDetail>()
                               where d.FontBillType != FrontBillType.水电 && d.FontBillType != FrontBillType.建管 && d.FontBillType != FrontBillType.税金
                               select d).Sum(d => d.BalanceTotalPrice);

                if (subProject.LaborMoneyType == ManagementRememberMethod.按费率计取)
                {
                    foreach (DataGridViewRow row in gridDetail.Rows)
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
                            row.Cells[DtlBalancePrice.Name].Value = billDtl.BalancePrice;
                            row.Cells[DtlBalanceTotalPrice.Name].Value = billDtl.BalanceTotalPrice;
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

                foreach (DataGridViewRow row in gridDetail.Rows)
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
                        row.Cells[DtlBalancePrice.Name].Value = billDtl.BalancePrice;
                        row.Cells[DtlBalanceTotalPrice.Name].Value = billDtl.BalanceTotalPrice;
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
                        row.Cells[DtlBalancePrice.Name].Value = billDtl.BalancePrice;
                        row.Cells[DtlBalanceTotalPrice.Name].Value = billDtl.BalanceTotalPrice;
                    }
                }
            }

            sumBalMoney = curBillMaster.Details.OfType<SubContractBalanceDetail>().Sum(d => d.BalanceTotalPrice);//总金额
            decimal sumBalQuantity = curBillMaster.Details.OfType<SubContractBalanceDetail>().Sum(d => d.BalacneQuantity);//总数量

            curBillMaster.BalanceMoney = sumBalMoney;//更新结算金额，在右键删除的时候直接用于计算合价

            txtSumQuantity.Text = sumBalQuantity.ToString();
            txtSumMoney.Text = sumBalMoney.ToString();
        }

        private void RefreshSumData2()
        {
            decimal sumOtherMoney = 0;//除水电和建管之外的金额
            foreach (DataGridViewRow dr in this.gridDetail.Rows)
            {
                if (dr.IsNewRow) break;
                SubContractBalanceDetail item = dr.Tag as SubContractBalanceDetail;
                if (item.FontBillType != FrontBillType.水电 && item.FontBillType != FrontBillType.建管)
                {
                    sumOtherMoney += item.BalanceTotalPrice;
                }
            }
            //判断分包合同的计费类型
            foreach (DataGridViewRow dr in this.gridDetail.Rows)
            {
                if (dr.IsNewRow) break;
                SubContractBalanceDetail detail = dr.Tag as SubContractBalanceDetail;
                if (detail.FontBillType == FrontBillType.水电 && subProject.UtilitiesRemMethod == UtilitiesRememberMethod.按费率计取)
                {
                    detail.BalancePrice = -Math.Abs(sumOtherMoney * subProject.UtilitiesRate);
                    detail.BalanceTotalPrice = detail.BalancePrice;
                    dr.Cells[this.DtlBalancePrice.Name].Value = detail.BalancePrice;
                    dr.Cells[this.DtlBalanceTotalPrice.Name].Value = detail.BalancePrice;
                }
                if (detail.FontBillType == FrontBillType.建管 && subProject.ManagementRemMethod == ManagementRememberMethod.按费率计取)
                {
                    detail.BalancePrice = -Math.Abs(sumOtherMoney * subProject.ManagementRate);
                    detail.BalanceTotalPrice = detail.BalancePrice;
                    dr.Cells[this.DtlBalancePrice.Name].Value = detail.BalancePrice;
                    dr.Cells[this.DtlBalanceTotalPrice.Name].Value = detail.BalancePrice;
                }
                dr.Tag = detail;
            }

            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            foreach (DataGridViewRow dr in this.gridDetail.Rows)
            {
                if (dr.IsNewRow) break;
                SubContractBalanceDetail item = dr.Tag as SubContractBalanceDetail;
                sumQuantity += item.BalacneQuantity;
                sumMoney += item.BalanceTotalPrice;
            }
            txtSumQuantity.Text = sumQuantity.ToString();
            txtSumMoney.Text = sumMoney.ToString();
        }

        void btnBatchPrice_Click(object sender, EventArgs e)
        {
            if (curBillMaster == null)
            {
                MessageBox.Show("无分包结算单信息！");
                return;
            }
            else {
                VSubBalanceBatchPrice frm = new VSubBalanceBatchPrice(curBillMaster);
                frm.ShowDialog();
                curBillMaster = frm.master;

                foreach (DataGridViewRow var in this.gridDetail.Rows)
                {
                    SubContractBalanceDetail rowDtl = var.Tag as SubContractBalanceDetail;
                    foreach (SubContractBalanceDetail dtl in curBillMaster.Details)
                    {
                        if (rowDtl.FrontBillGUID != null && dtl.FrontBillGUID != null && rowDtl.FrontBillGUID == dtl.FrontBillGUID && rowDtl.AccountQuantity == dtl.AccountQuantity)
                        {
                            //统计合价
                            decimal balanceTotalPrice = 0;
                            decimal balancePrice = 0;
                            foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                            {
                                balanceTotalPrice += subject.BalanceTotalPrice;
                                balancePrice += subject.BalancePrice;
                            }
                            if (balanceTotalPrice == 0)
                                return;
                            dtl.BalanceTotalPrice = balanceTotalPrice;
                            dtl.BalancePrice = balancePrice;

                            var.Cells[DtlBalanceQuantity.Name].Value = dtl.BalacneQuantity;
                            var.Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
                            var.Cells[DtlBalanceTotalPrice.Name].Value = decimal.Round(dtl.BalanceTotalPrice, 4);
                            var.Tag = dtl;
                            RefreshSumData(dtl.FontBillType);
                        }
                    }
                }
            }
        }
        //明细单元格双击事件
        void gridDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                SubContractBalanceDetail dtl = gridDetail.Rows[e.RowIndex].Tag as SubContractBalanceDetail;
                //if (dtl.FontBillType == FrontBillType.工程任务核算)
                //{
                VSubContractBalanceSubject frm = new VSubContractBalanceSubject(dtl);
                frm.ShowDialog();

                dtl = frm.optBalanceDtl;

                //统计合价
                decimal balanceTotalPrice = 0;
                decimal balancePrice = 0;
                foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                {
                    balanceTotalPrice += subject.BalanceTotalPrice;
                    balancePrice += subject.BalancePrice;
                }
                if (balanceTotalPrice == 0)
                    return;
                dtl.BalanceTotalPrice = balanceTotalPrice;
                dtl.BalancePrice = balancePrice;

                gridDetail.Rows[e.RowIndex].Cells[DtlBalanceQuantity.Name].Value = dtl.BalacneQuantity;
                gridDetail.Rows[e.RowIndex].Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
                gridDetail.Rows[e.RowIndex].Cells[DtlBalanceTotalPrice.Name].Value = decimal.Round(dtl.BalanceTotalPrice, 4);
                gridDetail.Rows[e.RowIndex].Tag = dtl;
                //}
                //else
                //{
                //    //MessageBox.Show("请选择前驱类型为" + FrontBillType.工程任务核算明细+"的结算单查看下级结算信息！");
                //}
                RefreshSumData(dtl.FontBillType);
                this.txtBillCode.Focus();
            }
        }
        //生成分包结算单
        void btnGeneBalanceBill_Click(object sender, EventArgs e)
        {
            IsRefresh = false;
            this.gridDetail.Rows.Clear();
            curBillMaster.Details.Clear();
            #region 生成结算单
            DateTime balanceStartTime = dtAccountStartTime.Value.Date;
            DateTime balanceEndTime = dtAccountEndDate.Value.Date.AddDays(1).AddSeconds(-1);

            if (balanceStartTime > balanceEndTime)
            {
                MessageBox.Show("结算起始时间不能大于结束时间！");
                dtAccountEndDate.Focus();
                return;
            }
            if (txtAccountRootNode.Text == "")
            {
                MessageBox.Show("请选择结算根节点！");
                btnSelectBalanceTaskRootNode.Focus();
                return;
            }
            if (this.txtBalOrg.Text == "")
            {
                MessageBox.Show("请选择结算单位！");
                btnSelectBalanceTaskRootNode.Focus();
                return;
            }

            //生成分包结算单
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();

            if (curBillMaster == null)
            {
                curBillMaster = new SubContractBalanceBill();

                DateTime time = model.GetServerTime();
                curBillMaster = new SubContractBalanceBill();
                curBillMaster.Code = string.Format("{0:yyyyMMddHHmmss}", time);
                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.CreateDate = ClientUtil.ToDateTime(time.ToShortDateString());
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;

                curBillMaster.OperOrgInfo = ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName = curBillMaster.OperOrgInfo.Name;
                curBillMaster.OpgSysCode = curBillMaster.OperOrgInfo.SysCode;
                curBillMaster.HandlePersonName = curBillMaster.CreatePersonName;

                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
            }

            curBillMaster.BalanceRange = (txtAccountRootNode.Tag as GWBSTree);
            curBillMaster.BalanceTaskName = txtAccountRootNode.Text;
            curBillMaster.BalanceTaskSyscode = curBillMaster.BalanceRange.SysCode;
            curBillMaster.SubContractUnitGUID = subProject.BearerOrg;
            curBillMaster.SubContractUnitName = subProject.BearerOrgName;
            curBillMaster.TheSubContractProject = subProject;
            curBillMaster.BeginTime = balanceStartTime;
            curBillMaster.EndTime = balanceEndTime;
            curBillMaster.Descript = txtRemark.Text.Trim();
            try
            {
                curBillMaster = model.SubBalSrv.GenSubBalanceBill(curBillMaster, subProject, projectInfo);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.InnerException==null ? ex.Message :ex.InnerException.Message);
                return;
            }
            this.TransAllSubBalBill(curBillMaster);

            //保存数据
            try
            {
                if (curBillMaster.Details.Count == 0)
                {
                    MessageBox.Show("选择根节点和结算时间段内没有需要结算的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ClearBalanceBillData();
                LoadBalanceBillData(curBillMaster);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            #endregion
        }
       
        //结算措施费
        void btnBalanceMeasureFee_Click(object sender, EventArgs e)
        {
            if (txtAccountRootNode.Text.Trim() == "" || txtAccountRootNode.Tag == null)
            {
                MessageBox.Show("请选择结算根节点！");
                btnSelectBalanceTaskRootNode.Focus();
                return;
            }

            GWBSTree balRange = txtAccountRootNode.Tag as GWBSTree;
            VSelectGWBSDetailByMeansureFee frm = new VSelectGWBSDetailByMeansureFee(balRange);
            frm.ShowDialog();
            if (frm.IsOk)
            {
                try
                {
                    FlashScreen.Show("正在生成措施费结算明细,请稍候...");

                    List<GWBSDetail> listMeansureFeeDtl = frm.SelectGWBSDetail;

                    for (int i = listMeansureFeeDtl.Count - 1; i > -1; i--)
                    {
                        GWBSDetail g = listMeansureFeeDtl[i];
                        var query = from d in curBillMaster.Details.OfType<SubContractBalanceDetail>()
                                    where d.BalanceTaskDtl != null && d.BalanceTaskDtl.Id == g.Id
                                    select d;

                        if (query.Count() > 0)
                            listMeansureFeeDtl.RemoveAt(i);
                    }

                    if (listMeansureFeeDtl.Count() > 0)
                    {
                        //计算措施费
                        IEnumerable<SubContractBalanceDetail> listCurrBalDtl = from d in curBillMaster.Details.OfType<SubContractBalanceDetail>()
                                                                               where d.FontBillType == FrontBillType.工程任务核算
                                                                               select d;

                        //List<SubContractBalanceDetail> listBalDtlUsage = model.BalanceSubContractFeeDtl(subProject, listMeansureFeeDtl, listCurrBalDtl.ToList());

                        List<SubContractBalanceDetail> listBalDtlUsage = BalanceMeansureFee(listMeansureFeeDtl, listCurrBalDtl);

                        curBillMaster.Details.AddAll(listBalDtlUsage.ToArray());

                        foreach (SubContractBalanceDetail dtl in listBalDtlUsage)
                        {
                            AddBalanceDetailInGrid(dtl);
                        }
                        RefreshSumData(FrontBillType.措施);
                    }
                }
                catch (Exception ex)
                {
                    FlashScreen.Close();
                    MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }
                finally
                {
                    FlashScreen.Close();
                }
            }
        }

        private List<SubContractBalanceDetail> BalanceMeansureFee(List<GWBSDetail> listMeansureDtl, IEnumerable<SubContractBalanceDetail> listCurrBalDtl)
        {
            List<SubContractBalanceDetail> listResult = new List<SubContractBalanceDetail>();

            foreach (GWBSDetail dtl in listMeansureDtl)//根据措施费明细生成结算明细
            {
                //过滤纳入分包取费范围内的分包结算明细
                //1.过滤指定措施费明细下的结算明细
                IEnumerable<SubContractBalanceDetail> listBalanceDtl = from b in listCurrBalDtl
                                                                       where b.FontBillType == FrontBillType.工程任务核算
                                                                       && (b.BalanceTaskSyscode != null && b.BalanceTaskSyscode.IndexOf(dtl.TheGWBSSysCode) > -1)
                                                                       select b;

                List<SubContractBalanceDetail> listOptBalDtl = new List<SubContractBalanceDetail>();

                //2.过滤成本项分类
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode1))
                {
                    listOptBalDtl.AddRange((from b in listBalanceDtl
                                            where b.BalanceTaskDtl.TheCostItemCateSyscode.IndexOf(dtl.TheCostItem.CateFilterSysCode1) > -1
                                            select b).ToArray());
                }
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode2))
                {
                    listOptBalDtl.AddRange((from b in listBalanceDtl
                                            where b.BalanceTaskDtl.TheCostItemCateSyscode.IndexOf(dtl.TheCostItem.CateFilterSysCode2) > -1
                                            select b).ToArray());
                }

                listOptBalDtl.Distinct();


                //3.过滤核算科目
                if (listBalanceDtl.Count() > 0)
                {
                    List<SubContractBalanceSubjectDtl> listMeansureFeeSubDtl = new List<SubContractBalanceSubjectDtl>();

                    foreach (SubContractBalanceDetail balDtl in listOptBalDtl)
                    {
                        if (dtl.TheCostItem.SubjectCateFilter1 != null || dtl.TheCostItem.SubjectCateFilter2 != null || dtl.TheCostItem.SubjectCateFilter3 != null)
                        {
                            if (dtl.TheCostItem.SubjectCateFilter1 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.TheCostItem.SubjectCateFilter1.Id
                                                               select d);
                            }
                            if (dtl.TheCostItem.SubjectCateFilter2 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.TheCostItem.SubjectCateFilter2.Id
                                                               select d);
                            }
                            if (dtl.TheCostItem.SubjectCateFilter3 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.TheCostItem.SubjectCateFilter3.Id
                                                               select d);
                            }
                        }
                        else
                        {
                            listMeansureFeeSubDtl.AddRange(balDtl.Details);
                        }
                    }

                    listMeansureFeeSubDtl.Distinct();

                    SubContractBalanceDetail newBalDtl = new SubContractBalanceDetail();
                    newBalDtl.FontBillType = FrontBillType.措施;
                    newBalDtl.ForwardDetailId = dtl.Id;
                    newBalDtl.FrontBillGUID = dtl.Id;
                    newBalDtl.BalanceTask = dtl.TheGWBS;
                    newBalDtl.BalanceTaskName = dtl.TheGWBS.Name;
                    newBalDtl.BalanceTaskSyscode = dtl.TheGWBSSysCode;
                    newBalDtl.BalanceTaskDtl = dtl;
                    newBalDtl.BalanceTaskDtlName = dtl.Name;
                    newBalDtl.BalacneQuantity = 1;
                    newBalDtl.BalancePrice = listMeansureFeeSubDtl.Sum(d => d.BalanceTotalPrice) * dtl.SubContractStepRate;
                    newBalDtl.BalanceTotalPrice = newBalDtl.BalacneQuantity * newBalDtl.BalancePrice;

                    //扣税,在税金中扣
                    //if (subProject.LaborMoneyType == ManagementRememberMethod.按费率计取)
                    //{
                    //    newBalDtl.BalancePrice = newBalDtl.BalancePrice - (newBalDtl.BalancePrice * subProject.LaobrRace);
                    //    newBalDtl.BalanceTotalPrice = newBalDtl.BalanceTotalPrice - (newBalDtl.BalanceTotalPrice * subProject.LaobrRace);
                    //}

                    newBalDtl.QuantityUnit = dtl.WorkAmountUnitGUID;
                    newBalDtl.QuantityUnitName = dtl.WorkAmountUnitName;
                    newBalDtl.PriceUnit = dtl.PriceUnitGUID;
                    newBalDtl.PriceUnitName = dtl.PriceUnitName;

                    foreach (GWBSDetailCostSubject dtlUsage in dtl.ListCostSubjectDetails)
                    {
                        SubContractBalanceSubjectDtl newBalSubDtl = new SubContractBalanceSubjectDtl();
                        newBalSubDtl.BalanceSubjectGUID = dtlUsage.CostAccountSubjectGUID;
                        newBalSubDtl.BalanceSubjectName = dtlUsage.CostAccountSubjectName;
                        newBalSubDtl.BalanceSubjectSyscode = dtlUsage.CostAccountSubjectSyscode;

                        newBalSubDtl.ResourceTypeGUID = dtlUsage.ResourceTypeGUID;
                        newBalSubDtl.ResourceTypeName = dtlUsage.ResourceTypeName;
                        newBalSubDtl.ResourceTypeSpec = dtlUsage.ResourceTypeSpec;
                        newBalSubDtl.ResourceTypeStuff = dtlUsage.ResourceTypeQuality;
                        newBalSubDtl.ResourceSyscode = dtlUsage.ResourceCateSyscode;
                        newBalSubDtl.DiagramNumber = dtlUsage.DiagramNumber;
                        newBalSubDtl.BalanceQuantity = 1;
                        newBalSubDtl.BalancePrice = newBalDtl.BalancePrice * dtlUsage.AssessmentRate;
                        newBalSubDtl.BalanceTotalPrice = newBalSubDtl.BalanceQuantity * newBalSubDtl.BalancePrice;
                        newBalSubDtl.CostName = dtlUsage.CostAccountSubjectName;
                        newBalSubDtl.FrontBillGUID = dtlUsage.Id;
                        newBalSubDtl.QuantityUnit = dtlUsage.ProjectAmountUnitGUID;
                        newBalSubDtl.QuantityUnitName = dtlUsage.ProjectAmountUnitName;
                        newBalSubDtl.PriceUnit = dtlUsage.PriceUnitGUID;
                        newBalSubDtl.PriceUnitName = dtlUsage.PriceUnitName;
                        newBalSubDtl.MonthBalanceFlag = MonthBalanceSuccessFlag.未结算;

                        newBalSubDtl.TheBalanceDetail = newBalDtl;
                        newBalDtl.Details.Add(newBalSubDtl);
                    }

                    listResult.Add(newBalDtl);
                }
            }

            return listResult;
        }

        //重新计算措施费
        void btnAccountMeansureFee_Click(object sender, EventArgs e)
        {
            RefreshSumData(FrontBillType.工程任务核算);
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
                    IEnumerable<GWBSDetailCostSubject> listUsage = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq).OfType<GWBSDetailCostSubject>();

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
                foreach (DataGridViewRow row in gridDetail.Rows)
                {
                    SubContractBalanceDetail balDtl = row.Tag as SubContractBalanceDetail;
                    if (balDtl.BalanceTaskDtl != null && balDtl.BalanceTaskDtl.Id == dtl.BalanceTaskDtl.Id)
                    {
                        row.Cells[DtlBalanceQuantity.Name].Value = dtl.BalacneQuantity;
                        row.Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
                        row.Cells[DtlBalanceTotalPrice.Name].Value = dtl.BalanceTotalPrice;
                        row.Tag = dtl;
                    }
                }
            }
        }
        #endregion

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="GUID"></param>
        public void Start(string code, string GUID)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", GUID));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("BalanceRange", NHibernate.FetchMode.Eager);

                oq.AddFetchMode("Details.BalanceTaskDtl", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("Details.BalanceTaskDtl.TheCostItem", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);

                IList list = model.ObjectQuery(typeof(SubContractBalanceBill), oq);
                if (list.Count > 0)
                {
                    curBillMaster = list[0] as SubContractBalanceBill;

                    ModelToView();
                    RefreshState(MainViewState.Browser);
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
        }

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);
            //控制自身控件
            //if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            //{
            //    ObjectLock.Unlock(pnlFloor, true);
            //    this.gridDetail.ContextMenuStrip.Enabled = true;
            //}
            //else
            //{
            //    ObjectLock.Lock(pnlFloor, true);
            //    this.gridDetail.ContextMenuStrip.Enabled = false;
            //}

            switch (state)
            {
                case MainViewState.AddNew:

                    dtAccountStartTime.Enabled = true;
                    dtAccountEndDate.Enabled = true;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = true;
                    btnSelectBalanceTaskRootNode.Enabled = true;
                    btnGeneBalanceBill.Enabled = true;
                    btnBalanceMeasureFee.Enabled = true;
                    btnSelectBalOrg.Enabled = true;
                    btnAccountMeansureFee.Enabled = true;
                    SetGridRowReadOnly(false);
                    break;
                case MainViewState.Modify:

                    dtAccountStartTime.Enabled = false;
                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = false;
                    txtCreateDate.Enabled = true;
                    btnSelectBalanceTaskRootNode.Enabled = false;
                    btnGeneBalanceBill.Enabled = false;
                    btnBalanceMeasureFee.Enabled = true;
                    btnAccountMeansureFee.Enabled = true;
                    btnSelectBalOrg.Enabled = false;
                    SetGridRowReadOnly(false);
                    break;
                case MainViewState.Browser:

                    dtAccountStartTime.Enabled = false;
                    dtAccountEndDate.Enabled = false;
                    txtRemark.ReadOnly = true;
                    txtCreateDate.Enabled = false;
                    btnSelectBalanceTaskRootNode.Enabled = false;
                    btnGeneBalanceBill.Enabled = false;
                    btnBalanceMeasureFee.Enabled = false;
                    btnAccountMeansureFee.Enabled = false;
                    btnSelectBalOrg.Enabled = false;
                    SetGridRowReadOnly(true);
                    break;
            }

            txtBillCode.ReadOnly = true;
            txtTheProject.ReadOnly = true;
            txtSumQuantity.ReadOnly = true;
            txtSumMoney.ReadOnly = true;
            txtRCJCoPrice.ReadOnly = true;
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
        }
        //清除界面数据
        private void ClearBalanceBillData()
        {
            txtBillCode.Text = "";
            txtRemark.Text = "";
            txtTheProject.Text = "";
            this.txtBalOrg.Tag = null;
            this.txtBalOrg.Text = "";
            this.txtAccountRootNode.Tag = null;
            this.txtAccountRootNode.Text = "";
            gridDetail.Rows.Clear();

            txtSumQuantity.Text = "";
            txtSumMoney.Text = "";
            txtRCJCoPrice.Text = "";
        }
        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }

            //自定义控件清空
            if (c is CustomEdit || c is TextBox)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        #endregion

        #region 增删改保存等操作
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

                DateTime time = model.GetServerTime();
                curBillMaster = new SubContractBalanceBill();
                //curBillMaster.Code = string.Format("{0:yyyyMMddHHmmss}", time);
                curBillMaster.DocState = DocumentState.Edit;
                curBillMaster.CreateDate = time;
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfo = ConstObject.TheOperationOrg;
                curBillMaster.OperOrgInfoName = curBillMaster.OperOrgInfo.Name;
                curBillMaster.BalanceRange = null;
                curBillMaster.TheSubContractProject = null;
                curBillMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;

                if (projectInfo != null)
                {
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }

                ClearBalanceBillData();
                LoadBalanceBillData(curBillMaster);
                RefreshControls(MainViewState.AddNew);
                dtAccountStartTime.Focus();
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
            if (curBillMaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();

                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));
                //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("BalanceRange", NHibernate.FetchMode.Eager);

                //curBillMaster = model.ObjectQuery(typeof(SubContractBalanceBill), oq)[0] as SubContractBalanceBill;
                //this.TransAllSubBalBill(curBillMaster);
                //ModelToView();

                return true;
            }
            string message = "此单状态为非制定状态，不能修改！";
            MessageBox.Show(message);
            return false;
        }
        //[optrType=1 保存][optrType=2 提交]
        private bool SaveOrSubmitBill(int optrType)
        {
            this.txtBalOrg.Focus();
            if (IsRefresh)
            {
                MessageBox.Show("你已经修改料费是否结算,请重新生成分包结算单");
                return false;
            }
            //判断明细金额和耗用金额是否相等
            foreach (SubContractBalanceDetail subDetial in curBillMaster.Details)
            {
                if (subDetial.ConfirmQuantity < subDetial.BalacneQuantity && subDetial.ConfirmQuantity > 0)
                {
                    MessageBox.Show(string.Format("[{0}]的明细结算量[{1}]必须小于等于确认量[{2}]", subDetial.BalanceTaskDtlName, subDetial.BalacneQuantity,subDetial.ConfirmQuantity));
                    return false ;
                }
                decimal detailMoney = subDetial.BalanceTotalPrice;
                decimal sumSubjectMoney = 0;
                foreach (SubContractBalanceSubjectDtl subject in subDetial.Details)
                {
                    sumSubjectMoney += subject.BalanceTotalPrice;
                }

                if (decimal.Round(detailMoney, 2) != decimal.Round(sumSubjectMoney, 2))
                {
                    MessageBox.Show("[" + subDetial.BalanceTaskDtlName + "]的明细金额[" + decimal.Round(detailMoney, 2) + "]和明细对应耗用金额之和[" + decimal.Round(sumSubjectMoney, 2) + "]不相等！");
                    return false;
                }
            }

            //判断金额是否超过百分比
            if (optrType == 2 && subProject.ContractInterimMoney > 0)
            {
                decimal sumContractMoney = subProject.ContractInterimMoney * (1 + subProject.AllowExceedPercent);//最大合同额
                decimal currBalMoney = subProject.AddupBalanceMoney + subProject.AddupWaitApproveBalMoney + curBillMaster.BalanceMoney;
                if (currBalMoney > sumContractMoney && sumContractMoney > 0)
                {
                    MessageBox.Show("累计结算金额[" + currBalMoney + "]大于分包合同最大金额[" + sumContractMoney + "]！");
                    return false;
                }
            }

            if (!ValidView())
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
                curBillMaster.DocState = DocumentState.InAudit;
            }
            curBillMaster.CreateDate = ClientUtil.ToDateTime(txtCreateDate.Value.ToShortDateString());
            curBillMaster.BalanceMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
            curBillMaster.CumulativeMoney = subProject.AddupBalanceMoney;
            curBillMaster = model.SubBalSrv.SaveSubContractBalBill(curBillMaster);

            log.BillId = curBillMaster.Id;
            log.BillType = "分包结算单";
            log.Code = curBillMaster.Code;
            log.Descript = "";
            log.OperPerson = ConstObject.LoginPersonInfo.Name;
            log.ProjectName = curBillMaster.ProjectName;
            StaticMethod.InsertLogData(log);

            //#region 短信
            if (optrType == 2)
            {
                MAppPlatMng appModel = new MAppPlatMng();
                appModel.SendMessage(curBillMaster.Id, "SubContractBalanceBill");
            }
            this.ViewCaption = ViewName + "-" + txtBillCode.Text;

            ModelToView();

            //this.txtBillCode.Text = curBillMaster.Code;

            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (SaveOrSubmitBill(1) == false) return false;
                MessageBox.Show("保存成功！");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public override bool SubmitView()
        {
            try
            {
                if (SaveOrSubmitBill(2) == false) return false;
                MessageBox.Show("提交成功！");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (this.gridDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            curBillMaster.Details.Clear();
            StringBuilder oBuilder = new StringBuilder();
            foreach (DataGridViewRow var in this.gridDetail.Rows)
            {
                if (var.IsNewRow) break;
                SubContractBalanceDetail curBillDtl = new SubContractBalanceDetail();
                if (var.Tag != null)
                {
                    curBillDtl = var.Tag as SubContractBalanceDetail;
                }
                //if ((curBillDtl.FontBillType == FrontBillType.水电 || curBillDtl.FontBillType == FrontBillType.建管) &&
                //    ClientUtil.ToDecimal(var.Cells[this.DtlBalancePrice.Name].Value) > 0)
                //{
                //    MessageBox.Show("代扣[水电][建管]类型的明细单价和金额必须为负数！");
                //    this.gridDetail.CurrentCell = var.Cells[DtlBalancePrice.Name];
                //    return false;
                //}
                string[] arrStr = { "建管费", "劳务税金", "水电费" };
                curBillDtl.TempAddBalanceQuantity= curBillDtl.AddBalanceQuantity - ClientUtil.ToDecimal(var.Cells[this.DtlBalanceQuantity.Name].Tag) + curBillDtl.BalacneQuantity;
                curBillDtl.TempAddBalanceMoney= curBillDtl.AddBalanceMoney - ClientUtil.ToDecimal(var.Cells[this.DtlBalanceQuantity.Name].Tag) * ClientUtil.ToDecimal(var.Cells[this.DtlBalancePrice.Name].Tag) + curBillDtl.BalanceTotalPrice;
                //switch (curBillDtl.FontBillType)
                //{
                //    case FrontBillType.工程任务核算:
                //        {
                //            if (curBillDtl.PlanWorkAmount < curBillDtl.TempAddBalanceQuantity)
                //            {
                //                if (oBuilder.Length > 0)
                //                {
                //                    oBuilder.Append("\n");
                //                }
                //                oBuilder.Append(string.Format("[{3}节点下:{0}]类型:[工程任务核算],[计划工程量({1})]小于[累计结算工程量({2})]", curBillDtl.BalanceTaskDtlName, curBillDtl.PlanWorkAmount, curBillDtl.TempAddBalanceQuantity, curBillDtl.BalanceTaskName));
                //            }
                //            break;
                //        }
                //    default:
                //        {
                //            if (!arrStr.Contains(curBillDtl.BalanceTaskDtlName))
                //            {
                //                if (curBillDtl.PlanTotalprice < curBillDtl.TempAddBalanceMoney)
                //                {
                //                    if (oBuilder.Length > 0)
                //                    {
                //                        oBuilder.Append("\n");
                //                    }
                //                    oBuilder.Append(string.Format("[{4}节点下:{0}]类型:[{3}],[计划金额({1})]小于[累计结算金额({2})]", curBillDtl.BalanceTaskDtlName, curBillDtl.PlanTotalprice, curBillDtl.TempAddBalanceMoney, Enum.GetName(typeof(FrontBillType), curBillDtl.FontBillType), curBillDtl.BalanceTaskName));
                //                }
                //            }
                //            break;
                //        }
                //}
                curBillDtl.BalanceBase = ClientUtil.ToString(var.Cells[this.DtlBalBase.Name].Value);
                curBillDtl.Remarks = ClientUtil.ToString(var.Cells[this.DtlRemark.Name].Value);
                var.Tag = curBillDtl;
                curBillDtl.Master = curBillMaster;
                curBillMaster.Details.Add(curBillDtl);
            }
            if (oBuilder.Length == 0)
            {
                foreach (SubContractBalanceDetail oDetail in curBillMaster.Details)
                {
                    oDetail.AddBalanceMoney = oDetail.TempAddBalanceMoney;
                    oDetail.AddBalanceQuantity = oDetail.TempAddBalanceQuantity;
                }
            }
            else
            {
                MessageBox.Show(string.Format("保存(提交)失败:\n{0}",oBuilder.ToString()));
                return false;
            }


            return true;
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                subProject = curBillMaster.TheSubContractProject;
                txtBillCode.Text = curBillMaster.Code;
                dtAccountStartTime.Value = curBillMaster.BeginTime;
                dtAccountEndDate.Value = curBillMaster.EndTime;
                txtCreateDate.Value = curBillMaster.CreateDate;
                txtRemark.Text = curBillMaster.Descript;
                this.txtBalOrg.Text = curBillMaster.SubContractUnitName;
                this.txtBalOrg.Tag = curBillMaster.TheSubContractProject;
                this.txtAccountRootNode.Tag = curBillMaster.BalanceRange;
                this.txtAccountRootNode.Text = curBillMaster.BalanceTaskName;

                txtTheProject.Text = curBillMaster.ProjectName;

                if (curBillMaster.Details.Count > 0)
                {
                    gridDetail.Rows.Clear();

                    decimal sumQuantity = 0;
                    decimal sumMoney = 0;
                    decimal subjectDtlCoPrice = 0;
                    curBillMaster.Details.OfType<SubContractBalanceDetail>().OrderBy(dtl => dtl.BalanceBase);
                    foreach (SubContractBalanceDetail item in curBillMaster.Details)
                    {
                        AddBalanceDetailInGrid(item);

                        sumQuantity += item.BalacneQuantity;
                        sumMoney += item.BalanceTotalPrice;

                        foreach (SubContractBalanceSubjectDtl dtlConsume in item.Details)
                        {
                            foreach (CostAccountSubject s in subjectList)
                            {
                                if (dtlConsume.BalanceSubjectGUID.Id == s.Id)
                                {
                                    subjectDtlCoPrice += dtlConsume.BalanceTotalPrice;
                                    break;
                                }
                            }
                        }
                    }
                    
                    txtSumQuantity.Text = sumQuantity.ToString();
                    txtSumMoney.Text = sumMoney.ToString();
                    txtRCJCoPrice.Text = subjectDtlCoPrice.ToString();
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                //curBillMaster = model.GetObjectById(typeof(SubContractBalanceBill), curBillMaster.Id) as SubContractBalanceBill;
                if (curBillMaster.DocState != DocumentState.Edit)
                {
                    MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                    return false;
                }
                else
                {
                    model.SubBalSrv.DeleteSubContractBalBill(curBillMaster);

                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "分包结算单";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    curBillMaster = null;

                    ClearView();

                    return true;
                }
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

                        Start("", curBillMaster.Id);
                        //ObjectQuery oq = new ObjectQuery();
                        //oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));
                        //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                        //curBillMaster = model.ObjectQuery(typeof(SubContractBalanceBill), oq)[0] as SubContractBalanceBill;

                        //ModelToView();
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
                if (ViewState == MainViewState.Modify)
                {
                    if (MessageBox.Show("当前结算单处于编辑状态，需要保存修改吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveView())
                        {
                            return;
                        }
                    }
                }

                //重新查询加载数据
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                curBillMaster = model.ObjectQuery(typeof(SubContractBalanceBill), oq)[0] as SubContractBalanceBill;
                ModelToView();
                RefreshControls(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        #endregion

        #region 对象转换算法
        //载入分包结算数据
        private void LoadBalanceBillData(SubContractBalanceBill bill)
        {
            txtBillCode.Text = bill.Code;
            txtRemark.Text = bill.Descript;
            txtAccountRootNode.Text = bill.BalanceTaskName;
            txtAccountRootNode.Tag = bill.BalanceRange;
            this.txtBalOrg.Tag = bill.TheSubContractProject;
            this.txtBalOrg.Text = bill.SubContractUnitName;

            txtTheProject.Text = bill.ProjectName;

            if (bill.Details.Count > 0)
            {
                gridDetail.Rows.Clear();

                decimal sumQuantity = 0;
                decimal sumMoney = 0;
                decimal subjectDtlCoPrice = 0;//分包结算单耗用合价
                foreach (SubContractBalanceDetail dtl in bill.Details)
                {
                    AddBalanceDetailInGrid(dtl);
                    foreach (SubContractBalanceSubjectDtl dtlConsume in dtl.Details)
                    {
                        foreach (CostAccountSubject s in subjectList)
                        {
                            if (dtlConsume.BalanceSubjectGUID != null && dtlConsume.BalanceSubjectGUID.Id == s.Id)
                            {
                                subjectDtlCoPrice += dtlConsume.BalanceTotalPrice;
                                break;
                            }
                        }
                    }
                    sumQuantity += dtl.BalacneQuantity;
                    sumMoney += dtl.BalanceTotalPrice;
                }

                txtSumQuantity.Text = sumQuantity.ToString();
                txtSumMoney.Text = sumMoney.ToString();
                txtRCJCoPrice.Text = subjectDtlCoPrice.ToString();

            }
        }
        //增加结算单明细数据
        private void AddBalanceDetailInGrid(SubContractBalanceDetail dtl)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            //设置编辑的单元格状态
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = SystemColors.Control;

            row.Cells[DtlProjectTaskNode.Name].Value = dtl.BalanceTaskName;
            if (dtl.BalanceTaskDtl != null && dtl.BalanceTaskDtl.Id != "0")
            {
                row.Cells[DtlMainResourceName.Name].Value = dtl.BalanceTaskDtl.MainResourceTypeName;
                row.Cells[DtlMainResourceSpec.Name].Value = dtl.BalanceTaskDtl.MainResourceTypeSpec;
                row.Cells[DtlDigramNumber.Name].Value = dtl.BalanceTaskDtl.DiagramNumber;
                if (dtl.BalanceTaskDtl.TheCostItem != null)
                    row.Cells[DtlCostItemQuota.Name].Value = dtl.BalanceTaskDtl.TheCostItem.QuotaCode;
            }
            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.BalanceTaskDtlName;

            row.Cells[DtlFrontType.Name].Value = dtl.FontBillType.ToString();
            row.Cells[DtlHandlePerson.Name].Value = dtl.HandlePersonName;
            if (dtl.BalacneQuantity != 0)
            {
                row.Cells[DtlBalanceQuantity.Name].Value = dtl.BalacneQuantity;
                row.Cells[DtlBalanceQuantity.Name].Tag = dtl.BalacneQuantity;
            }
            if (dtl.BalancePrice != 0)
            {
                row.Cells[DtlBalancePrice.Name].Value = dtl.BalancePrice;
                row.Cells[DtlBalancePrice.Name].Tag = dtl.BalancePrice;
            }
            if (dtl.BalanceTotalPrice != 0)
                row.Cells[DtlBalanceTotalPrice.Name].Value = dtl.BalanceTotalPrice;

            row.Cells[DtlQuantityUnit.Name].Value = dtl.QuantityUnitName;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;
           
            row.Cells[this.DtlBalBase.Name].Value = dtl.BalanceBase;
            // "计时工", "代工", "罚款"为只读
             row.Cells[this.DtlBalBase.Name].ReadOnly=(!string.IsNullOrEmpty(dtl.BalanceBase) && arrBalBaseReadOnly.Contains(dtl.BalanceBase));
           
            row.Cells[DtlRemark.Name].Value = dtl.Remarks;

            row.Tag = dtl;

            if (dtl.FontBillType == FrontBillType.建管 || dtl.FontBillType == FrontBillType.水电
                || dtl.FontBillType == FrontBillType.税金 || dtl.FontBillType == FrontBillType.措施)
            {
                row.Cells[DtlBalanceQuantity.Name].ReadOnly = true;
                row.Cells[DtlBalanceQuantity.Name].Style = style;
            }
            else
            {
                //row.Cells[DtlBalanceQuantity.Name].ReadOnly = false;
                //row.Cells[DtlBalancePrice.Name].ReadOnly = false;
                //row.Cells[DtlRemark.Name].ReadOnly = false;
                //row.Cells[DtlBalBase.Name].ReadOnly = false;
                //row.Cells[DtlProjectTaskNode.Name].ReadOnly = true;
                //row.Cells[DtlProjectTaskNode.Name].Style = style;
                //row.Cells[DtlCostItemQuota.Name].ReadOnly = true;
                //row.Cells[DtlCostItemQuota.Name].Style = style;
                //row.Cells[DtlProjectTaskDetail.Name].ReadOnly = true;
                //row.Cells[DtlProjectTaskDetail.Name].Style = style;
                //row.Cells[DtlFrontType.Name].ReadOnly = true;
                //row.Cells[DtlFrontType.Name].Style = style;
                //row.Cells[DtlQuantityUnit.Name].ReadOnly = true;
                //row.Cells[DtlQuantityUnit.Name].Style = style;
                //row.Cells[DtlPriceUnit.Name].ReadOnly = true;
                //row.Cells[DtlPriceUnit.Name].Style = style;

            }
        }

        private SubContractBalanceBill TransToNewSubContractBalanceBill(SubContractBalanceBill bill)
        {
            SubContractBalanceBill model = new SubContractBalanceBill();
            model.Id = bill.Id;
            model.BalanceRange = bill.BalanceRange;
            model.BalanceTaskName = bill.BalanceTaskName;
            model.SubContractUnitGUID = bill.SubContractUnitGUID;
            model.SubContractUnitName = bill.SubContractUnitName;
            model.BeginTime = bill.BeginTime;
            model.EndTime = bill.EndTime;
            model.ProjectId = bill.ProjectId;

            return model;
        }
        private SubContractBalanceDetail TransNewSubContractBalanceDetail(SubContractBalanceDetail detail)
        {
            SubContractBalanceDetail newDetail = new SubContractBalanceDetail();
            newDetail.AccountPrice = detail.AccountPrice;
            newDetail.AccountQuantity = detail.AccountQuantity;
            newDetail.ConfirmQuantity = detail.ConfirmQuantity;
            newDetail.BalacneQuantity = detail.BalacneQuantity;
            newDetail.BalancePrice = detail.BalancePrice;
            newDetail.BalanceTask = detail.BalanceTask;
            newDetail.BalanceTaskDtl = detail.BalanceTaskDtl;
            newDetail.BalanceTaskDtlName = detail.BalanceTaskDtlName;
            newDetail.BalanceTaskName = detail.BalanceTaskName;
            newDetail.BalanceTaskSyscode = detail.BalanceTaskSyscode;
            newDetail.BalanceTotalPrice = detail.BalanceTotalPrice;
            newDetail.FontBillType = detail.FontBillType;
            if (newDetail.FontBillType == FrontBillType.代工单)
            {
                newDetail.BalanceBase = "代工";
            }
            else if (newDetail.FontBillType == FrontBillType.计时派工 || newDetail.FontBillType == FrontBillType.零星用工单)
            {
                newDetail.BalanceBase = "计时工";
            }
            else if (newDetail.FontBillType == FrontBillType.罚款单 || newDetail.FontBillType == FrontBillType.扣款单 || newDetail.FontBillType == FrontBillType.暂扣款单)
            {
                newDetail.BalanceBase = "罚款";
            }
            else
            {
                newDetail.BalanceBase = "合同内";
            }
            newDetail.FrontBillGUID = detail.FrontBillGUID;
            newDetail.Id = detail.Id;
            newDetail.PriceUnit = detail.PriceUnit;
            newDetail.PriceUnitName = detail.PriceUnitName;
            newDetail.QuantityUnit = detail.QuantityUnit;
            newDetail.QuantityUnitName = detail.QuantityUnitName;
            newDetail.Remarks = detail.Remarks;
            return newDetail;
        }
        private SubContractBalanceSubjectDtl TransNewSubBalSubject(SubContractBalanceSubjectDtl subject)
        {
            SubContractBalanceSubjectDtl newSubject = new SubContractBalanceSubjectDtl();
            newSubject.BalancePrice = subject.BalancePrice;
            newSubject.BalanceQuantity = subject.BalanceQuantity;
            newSubject.BalanceSubjectGUID = subject.BalanceSubjectGUID;
            newSubject.BalanceSubjectName = subject.BalanceSubjectName;
            newSubject.BalanceTotalPrice = subject.BalanceTotalPrice;
            newSubject.CostName = subject.CostName;
            newSubject.FrontBillGUID = subject.FrontBillGUID;
            newSubject.Id = subject.Id;
            newSubject.MonthBalanceFlag = subject.MonthBalanceFlag;
            newSubject.PriceUnit = subject.PriceUnit;
            newSubject.PriceUnitName = subject.PriceUnitName;
            newSubject.QuantityUnit = subject.QuantityUnit;
            newSubject.QuantityUnitName = subject.QuantityUnitName;
            newSubject.ResourceTypeGUID = subject.ResourceTypeGUID;
            newSubject.ResourceTypeName = subject.ResourceTypeName;
            newSubject.ResourceTypeSpec = subject.ResourceTypeSpec;
            newSubject.ResourceTypeStuff = subject.ResourceTypeStuff;
            newSubject.ResourceSyscode = subject.ResourceSyscode;
            newSubject.DiagramNumber = subject.DiagramNumber;
            newSubject.TheBalanceDetail = subject.TheBalanceDetail;

            return newSubject;
        }

        private void TransAllSubBalBill(SubContractBalanceBill bill)
        {
            //if (allSubBalanceBill == null)
            //{
            allSubBalanceBill = this.TransToNewSubContractBalanceBill(bill);
            foreach (SubContractBalanceDetail detail in bill.Details)
            {
                SubContractBalanceDetail newDetail = this.TransNewSubContractBalanceDetail(detail);
                foreach (SubContractBalanceSubjectDtl subject in detail.Details)
                {
                    SubContractBalanceSubjectDtl newSubject = this.TransNewSubBalSubject(subject);
                    newSubject.TheBalanceDetail = newDetail;
                    newDetail.Details.Add(newSubject);
                }
                newDetail.Master = allSubBalanceBill;
                allSubBalanceBill.Details.Add(newDetail);
            }
            
            //bill.Details = bill.Details.OrderBy<
            //bill.Details = from p in bill.Details.OfType<SubContractBalanceDetail>() orderby p.BalanceBase select p;
            //}
        }


        private SubContractBalanceBill GetDiffSubBalBill()
        {
            SubContractBalanceBill diffMaster = this.TransToNewSubContractBalanceBill(curBillMaster);
            foreach (SubContractBalanceDetail detail in allSubBalanceBill.Details)
            {
                bool isExist = false;
                foreach (SubContractBalanceDetail currDetail in curBillMaster.Details)
                {
                    if (currDetail.FrontBillGUID == detail.FrontBillGUID)
                    {
                        isExist = true;
                    }
                }
                if (isExist == false)
                {
                    SubContractBalanceDetail newDetail = this.TransNewSubContractBalanceDetail(detail);
                    foreach (SubContractBalanceSubjectDtl subject in detail.Details)
                    {
                        SubContractBalanceSubjectDtl newSubject = this.TransNewSubBalSubject(subject);
                        newSubject.TheBalanceDetail = newDetail;
                        newDetail.Details.Add(newSubject);
                    }
                    newDetail.Master = diffMaster;
                    diffMaster.Details.Add(newDetail);
                }
            }
            return diffMaster;
        }
        #endregion
    }
}