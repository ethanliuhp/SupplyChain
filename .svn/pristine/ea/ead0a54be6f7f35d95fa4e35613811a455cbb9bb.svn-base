using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage
{
    public partial class VBusinessProposalItem : TBasicDataView
    {
        private MProjectPlanningMng model = new MProjectPlanningMng();
        //BusinessProposalItem item = new BusinessProposalItem();
        private BusinessProposalItem curBillItem;

        public VBusinessProposalItem(BusinessProposalItem item)
        {
            InitializeComponent();
            this.curBillItem = item;
            InitEvents();
            InitData();
            ShowDate();
        }

        /// <summary>
        /// 当前数据
        /// </summary>
        public BusinessProposalItem CurBillItem
        {
            get { return curBillItem; }
            set { curBillItem = value; }
        }

        public void InitData()
        {
            cmbImplementType.Items.AddRange(new object[] { "亏损风险化解", "开源策划", "内部成本控制" });
            cmbPlanningState.Items.AddRange(new object[] { "策划", "实施", "中止", "完成" });
  
        }

        public void InitEvents()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            txtFormerProceeds.tbTextChanged += new EventHandler(txtFormerProceeds_tbTextChanged);
            txtPlannedCost.tbTextChanged += new EventHandler(txtPlannedCost_tbTextChanged);
            txtPlanningIncome.tbTextChanged += new EventHandler(txtPlanningIncome_tbTextChanged);
            txtPlanningPlannedCost.tbTextChanged += new EventHandler(txtPlanningPlannedCost_tbTextChanged);
        }

        void txtFormerProceeds_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtFormerProceeds.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtFormerProceeds.Text = "";
                return;
            }

            if (txtPlanningPlannedCost.Text != "" && txtPlannedCost.Text != "" && txtFormerProceeds.Text != "" && txtPlanningIncome.Text != "")
            {
                decimal a = ClientUtil.ToDecimal(txtFormerProceeds.Text);
                decimal b = ClientUtil.ToDecimal(txtPlannedCost.Text);
                decimal c = ClientUtil.ToDecimal(txtPlanningIncome.Text);
                decimal d = ClientUtil.ToDecimal(txtPlanningPlannedCost.Text);
                decimal result = (c - d) - (a - b);
                txtBenefitRegulation.Text = result.ToString();
            }
        }

        void txtPlannedCost_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtPlannedCost.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtPlannedCost.Text = "";
                return;
            }

            if (txtPlanningPlannedCost.Text != "" && txtPlannedCost.Text != "" && txtFormerProceeds.Text != "" && txtPlanningIncome.Text != "")
            {
                decimal a = ClientUtil.ToDecimal(txtFormerProceeds.Text);
                decimal b = ClientUtil.ToDecimal(txtPlannedCost.Text);
                decimal c = ClientUtil.ToDecimal(txtPlanningIncome.Text);
                decimal d = ClientUtil.ToDecimal(txtPlanningPlannedCost.Text);
                decimal result = (c - d) - (a - b);
                txtBenefitRegulation.Text = result.ToString();
            }
        }

        void txtPlanningIncome_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtPlanningIncome.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtPlanningIncome.Text = "";
                return;
            }
            if (txtPlanningPlannedCost.Text != "" && txtPlannedCost.Text != "" && txtFormerProceeds.Text != "" && txtPlanningIncome.Text != "")
            {
                decimal a = ClientUtil.ToDecimal(txtFormerProceeds.Text);
                decimal b = ClientUtil.ToDecimal(txtPlannedCost.Text);
                decimal c = ClientUtil.ToDecimal(txtPlanningIncome.Text);
                decimal d = ClientUtil.ToDecimal(txtPlanningPlannedCost.Text);
                decimal result = (c - d) - (a - b);
                txtBenefitRegulation.Text = result.ToString();
            }

        }

        void txtPlanningPlannedCost_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string strDate = txtPlanningPlannedCost.Text.ToString();
            validity = CommonMethod.VeryValid(strDate);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtPlanningPlannedCost.Text = "";
                return;
            }
            if (txtPlanningPlannedCost.Text != "" && txtPlannedCost.Text != "" && txtFormerProceeds.Text != "" && txtPlanningIncome.Text != "")
            {
                decimal a = ClientUtil.ToDecimal(txtFormerProceeds.Text);
                decimal b = ClientUtil.ToDecimal(txtPlannedCost.Text);
                decimal c = ClientUtil.ToDecimal(txtPlanningIncome.Text);
                decimal d = ClientUtil.ToDecimal(txtPlanningPlannedCost.Text);
                decimal result = (c - d) - (a - b);
                txtBenefitRegulation.Text = result.ToString();
            }
        }

        public void ShowDate()
        {
            if (CurBillItem != null)
            {
                this.txtPlanningTheme.Text = ClientUtil.ToString(curBillItem.PlanningTheme);
                this.cmbImplementType.SelectedItem = ClientUtil.ToString(curBillItem.PlanningItemType);
                this.cmbPlanningState.SelectedItem = ClientUtil.ToString(curBillItem.PlanningState);
                this.dtpPlanBeginDate.Value = Convert.ToDateTime(curBillItem.PlanningDateEnd);
                this.dtpPlanImplementStartDate.Value = Convert.ToDateTime(curBillItem.PlanningImplementDate);
                this.txtDescript.Text = ClientUtil.ToString(curBillItem.Descript);
                decimal planningPlannedCost = ClientUtil.ToDecimal(curBillItem.PlanningPlannedCost);
                planningPlannedCost = planningPlannedCost / 10000;
                this.txtPlanningPlannedCost.Text = ClientUtil.ToString(planningPlannedCost);
                decimal planningIncome = ClientUtil.ToDecimal(curBillItem.PlanningIncome);
                planningIncome = planningIncome / 10000;
                this.txtPlanningIncome.Text = ClientUtil.ToString(planningIncome);
                decimal plannedCost = ClientUtil.ToDecimal(curBillItem.PlannedCost);
                plannedCost = plannedCost / 10000;
                this.txtPlannedCost.Text = ClientUtil.ToString(plannedCost);
                decimal formerproceeds = ClientUtil.ToDecimal(curBillItem.FormerProceeds);
                formerproceeds = formerproceeds / 10000;
                this.txtFormerProceeds.Text = ClientUtil.ToString(formerproceeds);
                decimal benefit = ClientUtil.ToDecimal(curBillItem.BenefitRegulation);
                benefit = benefit / 10000;
                this.txtBenefitRegulation.Text = ClientUtil.ToString(benefit);
            }
            else
            {
                this.cmbImplementType.SelectedItem = "亏损风险化解";
                this.cmbPlanningState.SelectedItem = "策划";
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (ClientUtil.ToString(this.txtPlanningTheme.Text) == "")
            {
                MessageBox.Show("策划主题不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.cmbImplementType.SelectedItem) == "")
            {
                MessageBox.Show("策划点类型不能为空！");
                return false;
            }
            if (ClientUtil.ToString(this.cmbPlanningState.SelectedItem) == "")
            {
                MessageBox.Show("策划实施状态不能为空！");
                return false;
            }
            return true;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidView()) return;
            try
            {
                if (curBillItem == null)
                {
                    curBillItem = new BusinessProposalItem();
                }
                curBillItem.PlanningTheme = ClientUtil.ToString(this.txtPlanningTheme.Text);
                curBillItem.PlanningState = EnumUtil<EnumItemPlanningState>.FromDescription(cmbPlanningState.SelectedItem);
                curBillItem.PlanningItemType = ClientUtil.ToString(this.cmbImplementType.SelectedItem);
                curBillItem.PlanningImplementStartDate = ClientUtil.ToDateTime(this.dtpPlanImplementStartDate.Value);
                curBillItem.PlanningDateEnd = ClientUtil.ToDateTime(this.dtpPlanEndDate.Value);
                curBillItem.PlanningDateStart = ClientUtil.ToDateTime(this.dtpPlanBeginDate.Value);
                curBillItem.PlanningImplementDate = ClientUtil.ToDateTime(this.dtpPlanImplementEndDate.Value);
                //计量单位
                curBillItem.Descript = ClientUtil.ToString(this.txtDescript.Text);
                string strUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.ProjectPlanningSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                curBillItem.MatStandardUnit = Unit;
                curBillItem.MatStandardUnitName = strUnit;
                curBillItem.RefreshDate = DateTime.Now;
                //策划后收入
                decimal planningIncome = ClientUtil.ToDecimal(this.txtPlanningIncome.Text);
                planningIncome = planningIncome * 10000;
                curBillItem.PlanningIncome = ClientUtil.ToDecimal(planningIncome);
                //策划后成本
                decimal planningPlannedCost = ClientUtil.ToDecimal(this.txtPlanningPlannedCost.Text);
                planningPlannedCost = planningPlannedCost * 10000;
                curBillItem.PlanningPlannedCost = ClientUtil.ToDecimal(planningPlannedCost);
                //原收入
                decimal plannedCost = ClientUtil.ToDecimal(this.txtPlannedCost.Text);
                plannedCost = plannedCost * 10000;
                curBillItem.PlannedCost = ClientUtil.ToDecimal(plannedCost);
                //原计划成本
                decimal formerproceeds = ClientUtil.ToDecimal(this.txtFormerProceeds.Text);
                formerproceeds = formerproceeds * 10000;
                curBillItem.FormerProceeds = ClientUtil.ToDecimal(formerproceeds);
                //效益增减
                decimal benefit = ClientUtil.ToDecimal(this.txtBenefitRegulation.Text);
                benefit = benefit * 10000;
                curBillItem.BenefitRegulation = ClientUtil.ToDecimal(benefit);
                //result.Add(curBillItem);
                this.Close();
            }
            catch (Exception eer)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(eer));
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnSave.FindForm().Close();
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
                    curBillItem = model.ProjectPlanningSrv.GetBusinessProposalItemById(Id);
                    ShowDate();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
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
            //object[] os = new object[] { txtCreatePerson, dtpCreateDate, txtProject, txtState };
            //ObjectLock.Lock(os);
        }

    }
}
