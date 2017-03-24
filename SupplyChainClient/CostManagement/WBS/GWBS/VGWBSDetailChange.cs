using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailChange : TBasicDataView
    {
        /// <summary>
        /// 页面初始选择的明细集合
        /// </summary>
        public IList listSelectedDtl = new ArrayList();
        /// <summary>
        /// 变更明细集
        /// </summary>
        public List<GWBSDetail> listChangeDtl = new List<GWBSDetail>();
        /// <summary>
        /// 缺省使用的契约
        /// </summary>
        public ContractGroup DefaultContract = null;

        private GWBSDetailLedger optDtlLedger = null;
        private GWBSDetail optDtl = null;

        private MGWBSTree model = new MGWBSTree();

        public VGWBSDetailChange()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            gridTaskDtl.CellClick += new DataGridViewCellEventHandler(gridTaskDtl_CellClick);
            gridTaskDtlChange.CellClick += new DataGridViewCellEventHandler(gridTaskDtlChange_CellClick);

            txtContractQuantityChange.TextChanged += new EventHandler(txtContractQuantityChange_TextChanged);
            txtContractQuantityResult.TextChanged += new EventHandler(txtContractQuantityResult_TextChanged);
            txtContractPriceChange.TextChanged += new EventHandler(txtContractPriceChange_TextChanged);
            txtContractPriceResult.TextChanged += new EventHandler(txtContractPriceResult_TextChanged);


            txtResponsibleQuantityChange.TextChanged += new EventHandler(txtResponsibleQuantityChange_TextChanged);
            txtResponsibleQuantityResult.TextChanged += new EventHandler(txtResponsibleQuantityResult_TextChanged);
            txtResponsiblePriceChange.TextChanged += new EventHandler(txtResponsiblePriceChange_TextChanged);
            txtResponsiblePriceResult.TextChanged += new EventHandler(txtResponsiblePriceResult_TextChanged);

            txtPlanQuantityChange.TextChanged += new EventHandler(txtPlanQuantityChange_TextChanged);
            txtPlanQuantityResult.TextChanged += new EventHandler(txtPlanQuantityResult_TextChanged);
            txtPlanPriceChange.TextChanged += new EventHandler(txtPlanPriceChange_TextChanged);
            txtPlanPriceResult.TextChanged += new EventHandler(txtPlanPriceResult_TextChanged);

            btnAddTaskDtl.Click += new EventHandler(btnSelectTaskDtl_Click);
            btnRemoveTaskDtl.Click += new EventHandler(btnRemoveTaskDtl_Click);

            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);

            btnEditContractUsage.Click += new EventHandler(btnEditContractUsage_Click);
            btnEditResponsibleUsage.Click += new EventHandler(btnEditResponsibleUsage_Click);
            btnEditPlanUsage.Click += new EventHandler(btnEditPlanUsage_Click);

            btnSaveChange.Click += new EventHandler(btnSaveChange_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VGWBSDetailChange_Load);
        }

        //编辑合同耗用
        void btnEditContractUsage_Click(object sender, EventArgs e)
        {
            if (optDtl == null)
            {
                MessageBox.Show("请选择要变更的任务明细！");
                return;
            }

            VGWBSDtlUsageEditByTaskChange frm = new VGWBSDtlUsageEditByTaskChange();
            if (txtContractQuantityResult.Text.Trim() != "" && ClientUtil.ToDecimal(txtContractQuantityResult.Text) != optDtl.ContractProjectQuantity)
            {
                LoadDtlUsageByWBSDetail();
                optDtl.ContractProjectQuantity = ClientUtil.ToDecimal(txtContractQuantityResult.Text);
                //计算耗用量价
                foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
                {
                    item.ContractProjectAmount = item.ContractQuotaQuantity * optDtl.ContractProjectQuantity;
                    item.ContractTotalPrice = item.ContractProjectAmount * item.ContractPrice;
                }
            }
            frm.OptionGWBSDtl = optDtl;
            frm.OptionUsageType = OptUsageType.合同耗用;
            frm.OptionViewState = VirtualMachine.Component.WinMVC.generic.MainViewState.Modify;
            frm.ShowDialog();

            optDtl = frm.OptionGWBSDtl;


            decimal price = 0;
            foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
            {
                price += item.ContractPrice;
            }

            txtContractPriceResult.Text = ToDecimailString(price);
        }
        //编辑责任耗用
        void btnEditResponsibleUsage_Click(object sender, EventArgs e)
        {
            VGWBSDtlUsageEditByTaskChange frm = new VGWBSDtlUsageEditByTaskChange();
            if (txtResponsibleQuantityResult.Text.Trim() != "" && ClientUtil.ToDecimal(txtResponsibleQuantityResult.Text) != optDtl.ResponsibilitilyWorkAmount)
            {
                LoadDtlUsageByWBSDetail();
                optDtl.ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(txtResponsibleQuantityResult.Text);
                //计算耗用量价
                foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
                {
                    item.ResponsibilitilyWorkAmount = item.ResponsibleQuotaNum * optDtl.ResponsibilitilyWorkAmount;
                    item.ResponsibilitilyTotalPrice = item.ResponsibilitilyWorkAmount * item.ResponsibleWorkPrice;
                }
            }
            frm.OptionGWBSDtl = optDtl;
            frm.OptionUsageType = OptUsageType.责任耗用;
            frm.OptionViewState = VirtualMachine.Component.WinMVC.generic.MainViewState.Modify;
            frm.ShowDialog();

            optDtl = frm.OptionGWBSDtl;


            decimal price = 0;
            foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
            {
                price += item.ResponsibleWorkPrice;
            }

            txtResponsiblePriceResult.Text = ToDecimailString(price);
        }
        //编辑计划耗用
        void btnEditPlanUsage_Click(object sender, EventArgs e)
        {
            VGWBSDtlUsageEditByTaskChange frm = new VGWBSDtlUsageEditByTaskChange();
            if (txtPlanQuantityResult.Text.Trim() != "" && ClientUtil.ToDecimal(txtPlanQuantityResult.Text) != optDtl.PlanWorkAmount)
            {
                LoadDtlUsageByWBSDetail();
                optDtl.PlanWorkAmount = ClientUtil.ToDecimal(txtPlanQuantityResult.Text);
                //计算耗用量价
                foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
                {
                    item.PlanWorkAmount = item.PlanQuotaNum * optDtl.PlanWorkAmount;
                    item.PlanTotalPrice = item.PlanWorkAmount * item.PlanWorkPrice;
                }
            }
            frm.OptionGWBSDtl = optDtl;
            frm.OptionUsageType = OptUsageType.计划耗用;
            frm.OptionViewState = VirtualMachine.Component.WinMVC.generic.MainViewState.Modify;
            frm.ShowDialog();

            optDtl = frm.OptionGWBSDtl;


            decimal price = 0;
            foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
            {
                price += item.PlanWorkPrice;
            }

            txtPlanPriceResult.Text = ToDecimailString(price);
        }

        void VGWBSDetailChange_Load(object sender, EventArgs e)
        {
            if (listSelectedDtl.Count > 0)
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (GWBSDetail dtl in listSelectedDtl)
                {
                    dis.Add(Expression.Eq("Id", dtl.Id));
                }

                oq.AddCriterion(dis);
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));

                IList listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);

                foreach (GWBSDetail dtl in listDtl)
                {
                    AddTaskDtlInGrid(dtl);
                }
                gridTaskDtl.ClearSelection();


                //ObjectQuery oq = new ObjectQuery();
                //Disjunction dis = new Disjunction();
                //foreach (GWBSDetail dtl in listSelectedDtl)
                //{
                //    dis.Add(Expression.Eq("ProjectTaskDtlID", dtl.Id));
                //}

                //oq.AddCriterion(dis);
                //oq.AddFetchMode("TheContractGroup", NHibernate.FetchMode.Eager);
                //oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));

                //IList listLedger = model.ObjectQuery(typeof(GWBSDetailLedger), oq);
                //foreach (GWBSDetailLedger item in listLedger)
                //{
                //    AddTaskDtlChangeInGrid(item);
                //}

                //gridTaskDtlChange.ClearSelection();
            }

            if (DefaultContract != null)
            {
                txtContractGroupName.Text = DefaultContract.ContractName;
                txtContractGroupType.Text = DefaultContract.ContractGroupType;
                txtContractGroupDesc.Text = DefaultContract.ContractDesc;
                txtContractGroupName.Tag = DefaultContract;
            }
        }

        void gridTaskDtl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                GWBSDetail dtl = gridTaskDtl.Rows[e.RowIndex].Tag as GWBSDetail;

                if (optDtl == null)
                {
                    optDtl = dtl;
                }
                else if (dtl.Id != optDtl.Id)
                {
                    if (IsChangeDtl() && MessageBox.Show("当前任务明细：" + optDtl.Name + "变更信息尚未保存，是否需要保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveChange())
                        {
                            return;
                        }
                    }

                    optDtl = dtl;
                }
                else
                    return;

                LoadLedgerInfo(dtl);

                LoadGWBSDetailInfo();
            }
        }

        private void LoadLedgerInfo(GWBSDetail dtl)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectTaskDtlID", dtl.Id));
            oq.AddFetchMode("TheContractGroup", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));

            IList list = model.ObjectQuery(typeof(GWBSDetailLedger), oq);
            gridTaskDtlChange.Rows.Clear();
            foreach (GWBSDetailLedger item in list)
            {
                AddTaskDtlChangeInGrid(item);
            }
            gridTaskDtlChange.ClearSelection();

            #region 加载汇总信息

            List<GWBSDetailLedger> listLedgerSum = new List<GWBSDetailLedger>();

            //1.添加变更初始值台帐
            var query = from l in list.OfType<GWBSDetailLedger>()
                        where l.ContractChangeMode == ContractIncomeChangeModeEnum.合同初始值
                        select l;
            if (query.Count() > 0)
                listLedgerSum.AddRange(query.ToList());

            //2.汇总量、价的变更

            decimal contractQnySum = (from l in list.OfType<GWBSDetailLedger>()
                                      where l.ContractChangeMode == ContractIncomeChangeModeEnum.合同收入工程量变化
                                      select l).Sum(o => o.ContractWorkAmount);

            decimal responsibleQnySum = (from l in list.OfType<GWBSDetailLedger>()
                                         where l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任工程量变化
                                         select l).Sum(o => o.ResponsibleWorkAmount);

            decimal planQnySum = (from l in list.OfType<GWBSDetailLedger>()
                                  where l.PlanCostChangeMode == PlanCostChangeModeEnum.计划工程量变化
                                  select l).Sum(o => o.PlanWorkAmount);

            if (contractQnySum > 0 || responsibleQnySum > 0 || planQnySum > 0)
            {
                GWBSDetailLedger led = new GWBSDetailLedger();
                led.ContractWorkAmount = contractQnySum;
                led.ResponsibleWorkAmount = responsibleQnySum;
                led.PlanWorkAmount = planQnySum;

                if (contractQnySum > 0)
                {
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入工程量变化;

                    var query2 = from l in list.OfType<GWBSDetailLedger>()
                                 where l.ContractChangeMode == ContractIncomeChangeModeEnum.合同初始值
                                 select l;
                    led.ContractPrice = query2.ElementAt(0).ContractPrice;
                    led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                }
                else
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;

                if (responsibleQnySum > 0)
                {
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任工程量变化;
                    var query2 = from l in list.OfType<GWBSDetailLedger>()
                                 where l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任成本初始值
                                 select l;

                    led.ResponsiblePrice = query2.ElementAt(0).ResponsiblePrice;
                    led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                }
                else
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;

                if (planQnySum > 0)
                {
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划工程量变化;
                    var query2 = from l in list.OfType<GWBSDetailLedger>()
                                 where l.PlanCostChangeMode == PlanCostChangeModeEnum.计划成本初始值
                                 select l;
                    led.PlanPrice = query2.ElementAt(0).PlanPrice;
                    led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                }
                else
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;

                listLedgerSum.Add(led);
            }


            decimal contractPriceSum = (from l in list.OfType<GWBSDetailLedger>()
                                        where l.ContractChangeMode == ContractIncomeChangeModeEnum.合同单价变化
                                        select l).Sum(o => o.ContractPrice);

            decimal responsiblePriceSum = (from l in list.OfType<GWBSDetailLedger>()
                                           where l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任单价变化
                                           select l).Sum(o => o.ResponsiblePrice);

            decimal planPriceSum = (from l in list.OfType<GWBSDetailLedger>()
                                    where l.PlanCostChangeMode == PlanCostChangeModeEnum.计划单价变化
                                    select l).Sum(o => o.PlanPrice);

            if (contractPriceSum > 0 || responsiblePriceSum > 0 || planPriceSum > 0)
            {
                GWBSDetailLedger led = new GWBSDetailLedger();
                led.ContractPrice = contractPriceSum;
                led.ResponsiblePrice = responsiblePriceSum;
                led.PlanPrice = planPriceSum;

                if (contractPriceSum > 0)
                {
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同单价变化;

                    decimal contractWorkAmount = (from l in listLedgerSum
                                                  where l.ContractChangeMode == ContractIncomeChangeModeEnum.合同收入工程量变化 || l.ContractChangeMode == ContractIncomeChangeModeEnum.合同初始值
                                                  select l).Sum(o => o.ContractWorkAmount);

                    led.ContractWorkAmount = contractWorkAmount;

                    led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                }
                else
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;

                if (responsiblePriceSum > 0)
                {
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任单价变化;

                    decimal responsibleWorkAmount = (from l in listLedgerSum
                                                     where l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任工程量变化 || l.ResponsibleCostChangeMode == ResponsibleCostChangeModeEnum.责任成本初始值
                                                     select l).Sum(o => o.ResponsibleWorkAmount);

                    led.ResponsibleWorkAmount = responsibleWorkAmount;
                    led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                }
                else
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;

                if (planPriceSum > 0)
                {
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划单价变化;

                    decimal planWorkAmount = (from l in listLedgerSum
                                              where l.PlanCostChangeMode == PlanCostChangeModeEnum.计划工程量变化 || l.PlanCostChangeMode == PlanCostChangeModeEnum.计划成本初始值
                                              select l).Sum(o => o.PlanWorkAmount);

                    led.PlanWorkAmount = planWorkAmount;
                    led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                }
                else
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;

                listLedgerSum.Add(led);
            }


            gridLedgerSum.Rows.Clear();
            foreach (GWBSDetailLedger item in listLedgerSum)
            {
                AddTaskDtlChangeInGridBySum(item);
            }
            gridLedgerSum.ClearSelection();

            #endregion
        }

        void btnSelectTaskDtl_Click(object sender, EventArgs e)
        {
            VSelectGWBSDetail frm = new VSelectGWBSDetail(new MGWBSTree());
            frm.InitCondition.Add("State", DocumentState.InExecute);
            frm.ShowDialog();
            if (frm.IsOk)
            {
                List<GWBSDetail> list = frm.SelectGWBSDetail;

                foreach (GWBSDetail dtl in list)
                {
                    if (gridTaskDtl.Rows.Count == 0)
                    {
                        AddTaskDtlInGrid(dtl);
                    }
                    else
                    {
                        for (int i = 0; i < gridTaskDtl.Rows.Count; i++)
                        {
                            GWBSDetail dtlTemp = gridTaskDtl.Rows[i].Tag as GWBSDetail;
                            if (dtlTemp.Id == dtl.Id)
                                break;

                            if (i == gridTaskDtl.Rows.Count - 1)
                                AddTaskDtlInGrid(dtl);
                        }
                    }
                }
            }
        }

        void btnRemoveTaskDtl_Click(object sender, EventArgs e)
        {
            if (gridTaskDtl.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要移除的任务明细！");
                gridTaskDtl.Focus();
                return;
            }

            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridTaskDtl.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();


            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridTaskDtl.Rows.RemoveAt(listRowIndex[i]);
            }
            gridTaskDtl.ClearSelection();
            optDtl = null;

            gridTaskDtlChange.Rows.Clear();

            ClearTaskDtlEditInfo();
        }

        private void ClearTaskDtlEditInfo()
        {
            txtContractGroupName.Text = "";
            txtContractGroupName.Tag = null;
            txtContractGroupDesc.Text = "";
            txtContractGroupType.Text = "";

            txtTaskDtlName.Text = "";

            txtContractQuantity.Text = "0";
            txtContractQuantityChange.Text = "0";
            txtContractQuantityResult.Text = "0";
            txtContractPrice.Text = "0";
            txtContractPriceChange.Text = "0";
            txtContractPriceResult.Text = "0";
            txtContractTotalPrice.Text = "0";

            txtResponsibleQuantity.Text = "0";
            txtResponsibleQuantityChange.Text = "0";
            txtResponsibleQuantityResult.Text = "0";
            txtResponsiblePrice.Text = "0";
            txtResponsiblePriceChange.Text = "0";
            txtResponsiblePriceResult.Text = "0";
            txtResponsibleTotalPrice.Text = "0";

            txtPlanQuantity.Text = "0";
            txtPlanQuantityChange.Text = "0";
            txtPlanQuantityResult.Text = "0";
            txtPlanPrice.Text = "0";
            txtPlanPriceChange.Text = "0";
            txtPlanPriceResult.Text = "0";
            txtPlanTotalPrice.Text = "0";
        }

        void gridTaskDtlChange_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            GWBSDetailLedger dtlLedger = gridTaskDtlChange.Rows[e.RowIndex].Tag as GWBSDetailLedger;


            if (optDtlLedger == null)
            {
                optDtlLedger = dtlLedger;
            }
            else if (dtlLedger.Id != optDtlLedger.Id)
            {
                if (IsChangeDtl())
                {
                    if (MessageBox.Show("当前任务明细：" + optDtl.Name + "变更信息尚未保存，是否需要保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!SaveChange())
                        {
                            return;
                        }
                    }
                }

                optDtlLedger = dtlLedger;
            }
            else
                return;


            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", optDtlLedger.ProjectTaskDtlID));
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

            IList list = model.ObjectQuery(typeof(GWBSDetail), oq);
            if (list.Count > 0)
                optDtl = list[0] as GWBSDetail;


            LoadGWBSDetailInfo();
        }

        private bool IsChangeDtl()
        {
            try
            {
                if (txtTaskDtlName.Text.Trim() != optDtl.Name)
                {
                    return true;
                }

                if (txtContractQuantityChange.Text.Trim() != "" && Convert.ToDecimal(txtContractQuantityChange.Text) != 0)
                {
                    return true;
                }
                if (txtContractPriceChange.Text.Trim() != "" && Convert.ToDecimal(txtContractPriceChange.Text) != 0)
                {
                    return true;
                }


                if (txtResponsibleQuantityChange.Text.Trim() != "" && Convert.ToDecimal(txtResponsibleQuantityChange.Text) != 0)
                {
                    return true;
                }
                if (txtResponsiblePriceChange.Text.Trim() != "" && Convert.ToDecimal(txtResponsiblePriceChange.Text) != 0)
                {
                    return true;
                }


                if (txtPlanQuantityChange.Text.Trim() != "" && Convert.ToDecimal(txtPlanQuantityChange.Text) != 0)
                {
                    return true;
                }
                if (txtPlanPriceChange.Text.Trim() != "" && Convert.ToDecimal(txtPlanPriceChange.Text) != 0)
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        private void LoadGWBSDetailInfo()
        {

            txtProQuantityUnit.Text = optDtl.WorkAmountUnitName;
            txtPriceUnit.Text = optDtl.PriceUnitName;

            txtTaskDtlName.Text = optDtl.Name;

            txtContractQuantity.Text = ToDecimailString(optDtl.ContractProjectQuantity);
            txtContractQuantityChange.Text = "0";
            txtContractQuantityResult.Text = ToDecimailString(optDtl.ContractProjectQuantity);
            txtContractPrice.Text = ToDecimailString(optDtl.ContractPrice);
            txtContractPriceChange.Text = "0";
            txtContractPriceResult.Text = ToDecimailString(optDtl.ContractPrice);
            txtContractTotalPrice.Text = ToDecimailString(optDtl.ContractTotalPrice);

            txtResponsibleQuantity.Text = ToDecimailString(optDtl.ResponsibilitilyWorkAmount);
            txtResponsibleQuantityChange.Text = "0";
            txtResponsibleQuantityResult.Text = ToDecimailString(optDtl.ResponsibilitilyWorkAmount);
            txtResponsiblePrice.Text = ToDecimailString(optDtl.ResponsibilitilyPrice);
            txtResponsiblePriceChange.Text = "0";
            txtResponsiblePriceResult.Text = ToDecimailString(optDtl.ResponsibilitilyPrice);
            txtResponsibleTotalPrice.Text = ToDecimailString(optDtl.ResponsibilitilyTotalPrice);


            txtPlanQuantity.Text = ToDecimailString(optDtl.PlanWorkAmount);
            txtPlanQuantityChange.Text = "0";
            txtPlanQuantityResult.Text = ToDecimailString(optDtl.PlanWorkAmount);
            txtPlanPrice.Text = ToDecimailString(optDtl.PlanPrice);
            txtPlanPriceChange.Text = "0";
            txtPlanPriceResult.Text = ToDecimailString(optDtl.PlanPrice);
            txtPlanTotalPrice.Text = ToDecimailString(optDtl.PlanTotalPrice);

        }

        private string ToDecimailString(decimal value)
        {
            return decimal.Round(value, 5).ToString();
        }

        /// <summary>
        /// 添加工程任务明细到Grid
        /// </summary>
        /// <param name="dtl"></param>
        private void AddTaskDtlInGrid(GWBSDetail dtl)
        {
            int index = gridTaskDtl.Rows.Add();
            DataGridViewRow row = gridTaskDtl.Rows[index];
            row.Cells[colMasterTaskName.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS);
            row.Cells[colMasterDtlName.Name].Value = dtl.Name;
            if (dtl.TheCostItem != null)
            {
                row.Cells[colMasterCostItem.Name].Value = dtl.TheCostItem.Name;
            }
            row.Cells[colMasterMainResourceType.Name].Value = dtl.MainResourceTypeName;
            row.Cells[colMasterResourceTypeSpec.Name].Value = dtl.MainResourceTypeSpec;
            row.Cells[colMasterDesc.Name].Value = dtl.ContentDesc;
            row.Cells[colMasterState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);

            row.Tag = dtl;
        }

        /// <summary>
        //修改添加工程任务明细到Grid
        /// </summary>
        /// <param name="dtl"></param>
        private void UpdateTaskDtlInGrid(GWBSDetail dtl)
        {
            for (int i = 0; i < gridTaskDtl.Rows.Count; i++)
            {
                DataGridViewRow row = gridTaskDtl.Rows[i];
                GWBSDetail dtlTemp = row.Tag as GWBSDetail;
                if (dtl.Id == dtlTemp.Id)
                {
                    row.Cells[colMasterDtlName.Name].Value = dtl.Name;

                    row.Tag = dtl;
                    break;
                }
            }
        }

        /// <summary>
        /// 添加工程任务明细台账记录到Grid
        /// </summary>
        /// <param name="item"></param>
        private void AddTaskDtlChangeInGrid(GWBSDetailLedger item)
        {

            int index = gridTaskDtlChange.Rows.Add();
            DataGridViewRow row = gridTaskDtlChange.Rows[index];

            row.Cells[DtlChangeTime.Name].Value = item.CreateTime;
            if (item.TheContractGroup != null)
            {
                row.Cells[DtlChangeContract.Name].Value = item.TheContractGroup.ContractName;
                row.Cells[DtlChangeContractCode.Name].Value = item.TheContractGroup.Code;
            }
            row.Cells[DtlContractChangeMode.Name].Value = item.ContractChangeMode;
            row.Cells[DtlContractQuantity.Name].Value = ToDecimailString(item.ContractWorkAmount);
            row.Cells[DtlContractPrice.Name].Value = ToDecimailString(item.ContractPrice);
            row.Cells[DtlContractrTotalPrice.Name].Value = ToDecimailString(item.ContractTotalPrice);

            row.Cells[DtlResponsibleChangeMode.Name].Value = item.ResponsibleCostChangeMode;
            row.Cells[DtlResponsibleQuantity.Name].Value = ToDecimailString(item.ResponsibleWorkAmount);
            row.Cells[DtlResponsiblePrice.Name].Value = ToDecimailString(item.ResponsiblePrice);
            row.Cells[DtlResponsibleTotalPrice.Name].Value = ToDecimailString(item.ResponsibleTotalPrice);

            row.Cells[DtlPlanChangeMode.Name].Value = item.PlanCostChangeMode;
            row.Cells[DtlPlanQuantity.Name].Value = ToDecimailString(item.PlanWorkAmount);
            row.Cells[DtlPlanPrice.Name].Value = ToDecimailString(item.PlanPrice);
            row.Cells[DtlPlanUsageTotalPrice.Name].Value = ToDecimailString(item.PlanTotalPrice);

            row.Tag = item;
        }

        private void AddTaskDtlChangeInGridBySum(GWBSDetailLedger item)
        {
            int index = gridLedgerSum.Rows.Add();
            DataGridViewRow row = gridLedgerSum.Rows[index];

            row.Cells[ContractChangeTypeSum.Name].Value = item.ContractChangeMode;
            row.Cells[ContractProjectAmountSum.Name].Value = item.ContractWorkAmount;
            row.Cells[ContractPriceSum.Name].Value = item.ContractPrice;
            row.Cells[ContractTotalPriceSum.Name].Value = item.ContractTotalPrice;

            row.Cells[ResponsibleChangeTypeSum.Name].Value = item.ResponsibleCostChangeMode;
            row.Cells[ResponsibleProjectAmountSum.Name].Value = item.ResponsibleWorkAmount;
            row.Cells[ResponsiblePriceSum.Name].Value = item.ResponsiblePrice;
            row.Cells[ResponsibleTotalPriceSum.Name].Value = item.ResponsibleTotalPrice;

            row.Cells[PlanChangeTypeSum.Name].Value = item.PlanCostChangeMode;
            row.Cells[PlanProjectAmountSum.Name].Value = item.PlanWorkAmount;
            row.Cells[PlanPriceSum.Name].Value = item.PlanPrice;
            row.Cells[PlanTotalPriceSum.Name].Value = item.PlanTotalPrice;

            row.Tag = item;
        }

        /// <summary>
        /// 插入工程任务明细台账记录到Grid
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currRowIndex"></param>
        private void InsertTaskDtlChangeInGrid(GWBSDetailLedger item, int currRowIndex)
        {

            int index = currRowIndex + 1;
            gridTaskDtlChange.Rows.Insert(index, 1);
            DataGridViewRow row = gridTaskDtlChange.Rows[index];

            row.Cells[DtlChangeTime.Name].Value = item.CreateTime;
            if (item.TheContractGroup != null)
            {
                row.Cells[DtlChangeContract.Name].Value = item.TheContractGroup.ContractName;
                row.Cells[DtlChangeContractCode.Name].Value = item.TheContractGroup.Code;
            }
            row.Cells[DtlContractChangeMode.Name].Value = item.ContractChangeMode;
            row.Cells[DtlContractQuantity.Name].Value = ToDecimailString(item.ContractWorkAmount);
            row.Cells[DtlContractPrice.Name].Value = ToDecimailString(item.ContractPrice);
            row.Cells[DtlContractrTotalPrice.Name].Value = ToDecimailString(item.ContractTotalPrice);

            row.Cells[DtlResponsibleChangeMode.Name].Value = item.ResponsibleCostChangeMode;
            row.Cells[DtlResponsibleQuantity.Name].Value = ToDecimailString(item.ResponsibleWorkAmount);
            row.Cells[DtlResponsiblePrice.Name].Value = ToDecimailString(item.ResponsiblePrice);
            row.Cells[DtlResponsibleTotalPrice.Name].Value = ToDecimailString(item.ResponsibleTotalPrice);

            row.Cells[DtlPlanChangeMode.Name].Value = item.PlanCostChangeMode;
            row.Cells[DtlPlanQuantity.Name].Value = ToDecimailString(item.PlanWorkAmount);
            row.Cells[DtlPlanPrice.Name].Value = ToDecimailString(item.PlanPrice);
            row.Cells[DtlPlanUsageTotalPrice.Name].Value = ToDecimailString(item.PlanTotalPrice);

            row.Tag = item;

            gridTaskDtlChange.CurrentCell = row.Cells[0];
        }

        /// <summary>
        /// 更新工程任务明细台账记录到Grid
        /// </summary>
        /// <param name="item"></param>
        private void UpdateTaskDtlChangeInGrid(GWBSDetailLedger item)
        {
            for (int i = 0; i < gridTaskDtlChange.Rows.Count; i++)
            {
                DataGridViewRow row = gridTaskDtlChange.Rows[i];
                GWBSDetailLedger d = row.Tag as GWBSDetailLedger;
                if (d.Id == item.Id)
                {
                    row.Cells[DtlChangeTime.Name].Value = item.CreateTime;
                    if (item.TheContractGroup != null)
                        row.Cells[DtlChangeContract.Name].Value = item.TheContractGroup.Code;

                    row.Cells[DtlContractQuantity.Name].Value = ToDecimailString(item.ContractWorkAmount);
                    row.Cells[DtlContractPrice.Name].Value = ToDecimailString(item.ContractPrice);
                    row.Cells[DtlContractrTotalPrice.Name].Value = ToDecimailString(item.ContractTotalPrice);


                    row.Cells[DtlResponsibleQuantity.Name].Value = ToDecimailString(item.ResponsibleWorkAmount);
                    row.Cells[DtlResponsiblePrice.Name].Value = ToDecimailString(item.ResponsiblePrice);
                    row.Cells[DtlResponsibleTotalPrice.Name].Value = ToDecimailString(item.ResponsibleTotalPrice);

                    row.Cells[DtlPlanQuantity.Name].Value = ToDecimailString(item.PlanWorkAmount);
                    row.Cells[DtlPlanPrice.Name].Value = ToDecimailString(item.PlanPrice);
                    row.Cells[DtlPlanUsageTotalPrice.Name].Value = ToDecimailString(item.PlanTotalPrice);

                    row.Tag = item;

                    gridTaskDtlChange.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        //选择契约
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContractGroupName.Tag != null)
            {
                frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                txtContractGroupName.Text = cg.ContractName;
                txtContractGroupType.Text = cg.ContractGroupType;
                txtContractGroupDesc.Text = cg.ContractDesc;
                txtContractGroupName.Tag = cg;
            }
        }

        //合同
        void txtContractQuantityChange_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtContractQuantityResult.Text = ToDecimailString(optDtl.ContractProjectQuantity + value);

            }
            catch
            {

            }
        }
        void txtContractQuantityResult_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtContractQuantityChange.Text = ToDecimailString(value - optDtl.ContractProjectQuantity);


                if (txtContractPriceResult.Text.Trim() != "")
                {
                    txtContractTotalPrice.Text = ToDecimailString(value * ClientUtil.ToDecimal(txtContractPriceResult.Text));
                }
            }
            catch
            {

            }
        }
        void txtContractPriceChange_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtContractPriceResult.Text = ToDecimailString(optDtl.ContractPrice + value);

            }
            catch
            {

            }
        }
        void txtContractPriceResult_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtContractPriceChange.Text = ToDecimailString(value - optDtl.ContractPrice);


                if (txtContractQuantityResult.Text.Trim() != "")
                {
                    txtContractTotalPrice.Text = ToDecimailString(value * ClientUtil.ToDecimal(txtContractQuantityResult.Text));
                }
            }
            catch
            {

            }
        }

        //责任
        void txtResponsibleQuantityChange_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtResponsibleQuantityResult.Text = ToDecimailString(value + optDtl.ResponsibilitilyWorkAmount);

            }
            catch
            {

            }
        }
        void txtResponsibleQuantityResult_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtResponsibleQuantityChange.Text = ToDecimailString(value - optDtl.ResponsibilitilyWorkAmount);


                if (txtResponsiblePriceResult.Text.Trim() != "")
                {
                    txtResponsibleTotalPrice.Text = ToDecimailString(value * ClientUtil.ToDecimal(txtResponsiblePriceResult.Text));
                }
            }
            catch
            {

            }
        }
        void txtResponsiblePriceChange_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtResponsiblePriceResult.Text = ToDecimailString(value + optDtl.ResponsibilitilyPrice);

            }
            catch
            {

            }

        }
        void txtResponsiblePriceResult_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtResponsiblePriceChange.Text = ToDecimailString(value - optDtl.ResponsibilitilyPrice);


                if (txtResponsibleQuantityResult.Text.Trim() != "")
                {
                    txtResponsibleTotalPrice.Text = ToDecimailString(value * ClientUtil.ToDecimal(txtResponsibleQuantityResult.Text));
                }
            }
            catch
            {

            }
        }

        //计划
        void txtPlanQuantityChange_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtPlanQuantityResult.Text = ToDecimailString(value + optDtl.PlanWorkAmount);

            }
            catch
            {

            }
        }
        void txtPlanQuantityResult_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtPlanQuantityChange.Text = ToDecimailString(value - optDtl.PlanWorkAmount);


                if (txtPlanPriceResult.Text.Trim() != "")
                {
                    txtPlanTotalPrice.Text = ToDecimailString(value * ClientUtil.ToDecimal(txtPlanPriceResult.Text));
                }
            }
            catch
            {

            }
        }
        void txtPlanPriceChange_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtPlanPriceResult.Text = ToDecimailString(value + optDtl.PlanPrice);

            }
            catch
            {

            }
        }
        void txtPlanPriceResult_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            try
            {
                decimal value = 0;
                if (txt.Text.Trim() != "")
                {
                    value = ClientUtil.ToDecimal(txt.Text);
                }

                txtPlanPriceChange.Text = ToDecimailString(value - optDtl.PlanPrice);


                if (txtPlanQuantityResult.Text.Trim() != "")
                {
                    txtPlanTotalPrice.Text = ToDecimailString(value * ClientUtil.ToDecimal(txtPlanQuantityResult.Text));
                }
            }
            catch
            {

            }
        }

        //保存变更
        void btnSaveChange_Click(object sender, EventArgs e)
        {
            SaveChange();

            LoadLedgerInfo(optDtl);
        }

        private bool SaveChange()
        {
            decimal contractQuantityChange = 0;
            decimal contractQuantityResult = 0;
            decimal contractPriceChange = 0;
            decimal contractPriceResult = 0;
            decimal contractTotalPriceChange = 0;
            decimal contractTotalPriceResult = 0;

            decimal responsibleQuantityChange = 0;
            decimal responsibleQuantityResult = 0;
            decimal responsiblePriceChange = 0;
            decimal responsiblePriceResult = 0;
            decimal responsibleTotalPriceChange = 0;
            decimal responsibleTotalPriceResult = 0;

            decimal planQuantityChange = 0;
            decimal planQuantityResult = 0;
            decimal planPriceChange = 0;
            decimal planPriceResult = 0;
            decimal planTotalPriceChange = 0;
            decimal planTotalPriceResult = 0;

            #region 有效性校验
            if (optDtl == null)
            {
                MessageBox.Show("请选择要变更的任务明细！");
                return false;
            }

            if (!valideContractGroup())
            {
                return false;
            }

            if (txtTaskDtlName.Text.Trim() == "")
            {
                MessageBox.Show("任务明细名称不能为空！");
                txtTaskDtlName.Focus();
                return false;
            }

            try
            {
                if (txtContractQuantityChange.Text.Trim() != "")
                    contractQuantityChange = Convert.ToDecimal(txtContractQuantityChange.Text);
            }
            catch
            {
                MessageBox.Show("合同工程量变更值输入格式不正确！");
                txtContractQuantityChange.Focus();
                return false;
            }

            try
            {
                if (txtContractQuantityResult.Text.Trim() != "")
                    contractQuantityResult = Convert.ToDecimal(txtContractQuantityResult.Text);
            }
            catch
            {
                MessageBox.Show("合同工程量变更结果值输入格式不正确！");
                txtContractQuantityResult.Focus();
                return false;
            }

            try
            {
                if (txtContractPriceChange.Text.Trim() != "")
                    contractPriceChange = Convert.ToDecimal(txtContractPriceChange.Text);
            }
            catch
            {
                MessageBox.Show("合同单价变更值输入格式不正确！");
                txtContractPriceChange.Focus();
                return false;
            }

            try
            {
                if (txtContractPriceResult.Text.Trim() != "")
                    contractPriceResult = Convert.ToDecimal(txtContractPriceResult.Text);
            }
            catch
            {
                MessageBox.Show("合同单价变更结果值输入格式不正确！");
                txtContractPriceResult.Focus();
                return false;
            }

            try
            {
                if (txtResponsibleQuantityChange.Text.Trim() != "")
                    responsibleQuantityChange = Convert.ToDecimal(txtResponsibleQuantityChange.Text);
            }
            catch
            {
                MessageBox.Show("责任工程量变更值输入格式不正确！");
                txtResponsibleQuantityChange.Focus();
                return false;
            }

            try
            {
                if (txtResponsibleQuantityResult.Text.Trim() != "")
                    responsibleQuantityResult = Convert.ToDecimal(txtResponsibleQuantityResult.Text);
            }
            catch
            {
                MessageBox.Show("责任工程量变更结果值输入格式不正确！");
                txtResponsibleQuantityResult.Focus();
                return false;
            }

            try
            {
                if (txtResponsiblePriceChange.Text.Trim() != "")
                    responsiblePriceChange = Convert.ToDecimal(txtResponsiblePriceChange.Text);
            }
            catch
            {
                MessageBox.Show("责任单价变更值输入格式不正确！");
                txtResponsiblePriceChange.Focus();
                return false;
            }

            try
            {
                if (txtResponsiblePriceResult.Text.Trim() != "")
                    responsiblePriceResult = Convert.ToDecimal(txtResponsiblePriceResult.Text);
            }
            catch
            {
                MessageBox.Show("责任单价变更结果值输入格式不正确！");
                txtResponsiblePriceResult.Focus();
                return false;
            }

            try
            {
                if (txtPlanQuantityChange.Text.Trim() != "")
                    planQuantityChange = Convert.ToDecimal(txtPlanQuantityChange.Text);
            }
            catch
            {
                MessageBox.Show("计划工程量变更值输入格式不正确！");
                txtPlanQuantityChange.Focus();
                return false;
            }

            try
            {
                if (txtPlanQuantityResult.Text.Trim() != "")
                    planQuantityResult = Convert.ToDecimal(txtPlanQuantityResult.Text);
            }
            catch
            {
                MessageBox.Show("计划工程量变更结果值输入格式不正确！");
                txtPlanQuantityResult.Focus();
                return false;
            }

            try
            {
                if (txtPlanPriceChange.Text.Trim() != "")
                    planPriceChange = Convert.ToDecimal(txtPlanPriceChange.Text);
            }
            catch
            {
                MessageBox.Show("计划单价变更值输入格式不正确！");
                txtPlanPriceChange.Focus();
                return false;
            }

            try
            {
                if (txtPlanPriceResult.Text.Trim() != "")
                    planPriceResult = Convert.ToDecimal(txtPlanPriceResult.Text);
            }
            catch
            {
                MessageBox.Show("计划单价变更结果值输入格式不正确！");
                txtPlanPriceResult.Focus();
                return false;
            }


            #endregion

            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("Id", optDtl.Id));
            //oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

            //IList list = model.ObjectQuery(typeof(GWBSDetail), oq);
            //if (list.Count > 0)
            //optDtl = list[0] as GWBSDetail;

            LoadDtlUsageByWBSDetail();

            optDtl.Name = txtTaskDtlName.Text;

            ContractGroup optContract = txtContractGroupName.Tag as ContractGroup;
            optDtl.ContractGroupGUID = optContract.Id;
            optDtl.ContractGroupName = optContract.ContractName;
            optDtl.ContractGroupCode = optContract.Code;
            optDtl.ContractGroupType = optContract.ContractGroupType;

            IList listLedger = new ArrayList();

            #region 记录明细台账
            if ((txtContractQuantityChange.Text.Trim() != "" && ClientUtil.ToDecimal(txtContractQuantityChange.Text) != 0)
                ||
                (txtResponsibleQuantityChange.Text.Trim() != "" && ClientUtil.ToDecimal(txtResponsibleQuantityChange.Text) != 0)
                ||
                (txtPlanQuantityChange.Text.Trim() != "" && ClientUtil.ToDecimal(txtPlanQuantityChange.Text) != 0))
            {
                GWBSDetailLedger led = new GWBSDetailLedger();

                led.ProjectTaskID = optDtl.TheGWBS.Id;
                led.ProjectTaskName = optDtl.TheGWBS.Name;
                led.TheProjectTaskSysCode = optDtl.TheGWBS.SysCode;

                led.ProjectTaskDtlID = optDtl.Id;
                led.ProjectTaskDtlName = optDtl.Name;

                if (txtContractQuantityChange.Text.Trim() != "" && txtContractQuantityChange.Text.Trim() != "0")
                {
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入工程量变化;
                    led.ContractWorkAmount = Convert.ToDecimal(txtContractQuantityChange.Text);
                    led.ContractPrice = optDtl.ContractPrice;
                    led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;

                    //计算耗用量价
                    foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
                    {
                        item.ContractProjectAmount = item.ContractQuotaQuantity * optDtl.ContractProjectQuantity;
                        item.ContractTotalPrice = item.ContractProjectAmount * item.ContractPrice;
                    }
                }
                else
                {
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;
                    led.ContractWorkAmount = 0;
                    led.ContractPrice = 0;
                    led.ContractTotalPrice = 0;
                }

                if (txtResponsibleQuantityChange.Text.Trim() != "" && txtResponsibleQuantityChange.Text.Trim() != "0")
                {
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任工程量变化;
                    led.ResponsibleWorkAmount = Convert.ToDecimal(txtResponsibleQuantityChange.Text);
                    led.ResponsiblePrice = optDtl.ResponsibilitilyPrice;
                    led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;

                    //计算耗用量价
                    foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
                    {
                        item.ResponsibilitilyWorkAmount = item.ResponsibleQuotaNum * optDtl.ResponsibilitilyWorkAmount;
                        item.ResponsibilitilyTotalPrice = item.ResponsibilitilyWorkAmount * item.ResponsibleWorkPrice;
                    }
                }
                else
                {
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;
                    led.ResponsiblePrice = 0;
                    led.ResponsibleWorkAmount = 0;
                    led.ResponsibleTotalPrice = 0;
                }


                if (txtPlanQuantityChange.Text.Trim() != "" && txtPlanQuantityChange.Text.Trim() != "0")
                {
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划工程量变化;
                    led.PlanWorkAmount = Convert.ToDecimal(txtPlanQuantityChange.Text);
                    led.PlanPrice = optDtl.PlanPrice;
                    led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;

                    //计算耗用量价
                    foreach (GWBSDetailCostSubject item in optDtl.ListCostSubjectDetails)
                    {
                        item.PlanWorkAmount = item.PlanQuotaNum * optDtl.PlanWorkAmount;
                        item.PlanTotalPrice = item.PlanWorkAmount * item.PlanWorkPrice;
                    }
                }
                else
                {
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;
                    led.PlanWorkAmount = 0;
                    led.PlanPrice = 0;
                    led.PlanTotalPrice = 0;
                }

                led.WorkAmountUnit = optDtl.WorkAmountUnitGUID;
                led.WorkAmountUnitName = optDtl.WorkAmountUnitName;

                led.PriceUnit = optDtl.PriceUnitGUID;
                led.PriceUnitName = optDtl.PriceUnitName;

                led.TheContractGroup = txtContractGroupName.Tag as ContractGroup;

                led.TheProjectGUID = optDtl.TheProjectGUID;
                led.TheProjectName = optDtl.TheProjectName;

                listLedger.Add(led);
            }

            optDtl.ContractProjectQuantity = contractQuantityResult;
            optDtl.ResponsibilitilyWorkAmount = responsibleQuantityResult;
            optDtl.PlanWorkAmount = planQuantityResult;

            if ((txtContractPriceChange.Text.Trim() != "" && ClientUtil.ToDecimal(txtContractPriceChange.Text) != 0)
                 ||
                 (txtResponsiblePriceChange.Text.Trim() != "" && ClientUtil.ToDecimal(txtResponsiblePriceChange.Text) != 0)
                 ||
                 (txtPlanPriceChange.Text.Trim() != "" && ClientUtil.ToDecimal(txtPlanPriceChange.Text) != 0))
            {
                GWBSDetailLedger led = new GWBSDetailLedger();

                led.ProjectTaskID = optDtl.TheGWBS.Id;
                led.ProjectTaskName = optDtl.TheGWBS.Name;
                led.TheProjectTaskSysCode = optDtl.TheGWBS.SysCode;

                led.ProjectTaskDtlID = optDtl.Id;
                led.ProjectTaskDtlName = optDtl.Name;

                if (txtContractPriceChange.Text.Trim() != "" && txtContractPriceChange.Text.Trim() != "0")
                {
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同单价变化;
                    led.ContractPrice = Convert.ToDecimal(txtContractPriceChange.Text);
                    led.ContractWorkAmount = optDtl.ContractProjectQuantity;
                    led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                }
                else
                {
                    led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;
                    led.ContractWorkAmount = 0;
                    led.ContractPrice = 0;
                    led.ContractTotalPrice = 0;
                }

                if (txtResponsiblePriceChange.Text.Trim() != "" && txtResponsiblePriceChange.Text.Trim() != "0")
                {
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任单价变化;
                    led.ResponsiblePrice = Convert.ToDecimal(txtResponsiblePriceChange.Text);
                    led.ResponsibleWorkAmount = optDtl.ResponsibilitilyWorkAmount;
                    led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                }
                else
                {
                    led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;
                    led.ResponsiblePrice = 0;
                    led.ResponsibleWorkAmount = 0;
                    led.ResponsibleTotalPrice = 0;
                }


                if (txtPlanPriceChange.Text.Trim() != "" && txtPlanPriceChange.Text.Trim() != "0")
                {
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划单价变化;
                    led.PlanPrice = Convert.ToDecimal(txtPlanPriceChange.Text);
                    led.PlanWorkAmount = optDtl.PlanWorkAmount;
                    led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                }
                else
                {
                    led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;
                    led.PlanWorkAmount = 0;
                    led.PlanPrice = 0;
                    led.PlanTotalPrice = 0;
                }

                led.WorkAmountUnit = optDtl.WorkAmountUnitGUID;
                led.WorkAmountUnitName = optDtl.WorkAmountUnitName;

                led.PriceUnit = optDtl.PriceUnitGUID;
                led.PriceUnitName = optDtl.PriceUnitName;

                led.TheContractGroup = txtContractGroupName.Tag as ContractGroup;

                led.TheProjectGUID = optDtl.TheProjectGUID;
                led.TheProjectName = optDtl.TheProjectName;

                listLedger.Add(led);
            }
            #endregion

            optDtl.ContractPrice = contractPriceResult;
            optDtl.ContractTotalPrice = optDtl.ContractProjectQuantity * optDtl.ContractPrice;

            optDtl.ResponsibilitilyPrice = responsiblePriceResult;
            optDtl.ResponsibilitilyTotalPrice = optDtl.ResponsibilitilyWorkAmount * optDtl.ResponsibilitilyPrice;

            optDtl.PlanPrice = planPriceResult;
            optDtl.PlanTotalPrice = optDtl.PlanWorkAmount * optDtl.PlanPrice;

            try
            {

                IList listResult = model.ChangeTaskDetail(optDtl, listLedger);
                optDtl = listResult[0] as GWBSDetail;
                listLedger = listResult[1] as IList;


                var query = from d in listChangeDtl
                            where d.Id == optDtl.Id
                            select d;

                if (query.Count() == 0)
                    listChangeDtl.Add(optDtl);
                else
                {
                    for (int i = 0; i < listChangeDtl.Count; i++)
                    {
                        GWBSDetail dtl = listChangeDtl[i];
                        if (dtl.Id == optDtl.Id)
                        {
                            listChangeDtl[i] = optDtl;
                            break;
                        }
                    }
                }

                LoadGWBSDetailInfo();

                UpdateTaskDtlInGrid(optDtl);

                int currRowIndex = gridTaskDtlChange.Rows.Count - 1;

                for (int i = listLedger.Count - 1; i > -1; i--)
                {
                    GWBSDetailLedger led = listLedger[i] as GWBSDetailLedger;
                    InsertTaskDtlChangeInGrid(led, currRowIndex);
                }

                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }

        private bool valideContractGroup()
        {
            if (txtContractGroupName.Text.Trim() == "" || txtContractGroupName.Tag == null)
            {
                MessageBox.Show("请选择变更契约！");

                VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                if (txtContractGroupName.Tag != null)
                {
                    frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
                }
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                    txtContractGroupName.Text = cg.ContractName;
                    txtContractGroupName.Tag = cg;

                    txtContractGroupType.Text = cg.ContractGroupType;
                    txtContractGroupDesc.Text = cg.ContractDesc;

                    return true;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 加载工程任务明细耗用
        /// </summary>
        /// <returns></returns>
        private void LoadDtlUsageByWBSDetail()
        {
            try
            {
                int dtlCount = optDtl.ListCostSubjectDetails.Count;//未加载时加载
            }
            catch
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", optDtl.Id));
                oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                optDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;
            }
        }
        //放弃
        void btnCancel_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("您确认要放弃当前更改吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    return;

            this.Close();
        }

    }
}
