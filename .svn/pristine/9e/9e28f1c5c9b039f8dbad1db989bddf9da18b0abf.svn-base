using System;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.FinancialResource.RelateClass;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ExpenseItemMng
{
    public enum ExpenseItemExcType
    {
        commonSelect
    }
    public partial class VExpenseItem : TBasicDataView
    {
        private MExpenseItem mExpenseItem;
        private CExpenseItem cExpenseItem;
        private CommonAccTitle acctsel;
        public VExpenseItem(MExpenseItem m, CExpenseItem c)
        {
            mExpenseItem = m;
            cExpenseItem = c;

            InitializeComponent();

            this.Title = "费用项目";

            InitOperationTache();
            InitData();
            InitViewData();
            acctsel = new CommonAccTitle();
            acctsel.OnlyLeafTitle = true;

            this.menuItemDeleteRow.Click += new EventHandler(menuItemDeleteRow_Click);
            this.dgExpenseItem.DoubleClick+=new EventHandler(dgExpenseItem_DoubleClick);
        }

        void dgExpenseItem_DoubleClick(object sender, EventArgs e)
        {
            if (dgExpenseItem.ReadOnly)
                return;
            string colName = dgExpenseItem.CurrentCell.OwningColumn.Name;
            string cellvalue = ClientUtil.ToString(dgExpenseItem.CurrentCell.Value);
            if (colName.Equals("AccountTitle"))
            {
                acctsel.Text = cellvalue;
                acctsel.OpenSelect();
                if (acctsel.Result != null && acctsel.Result.Count > 0)
                {
                    AccountTitleInfo attmp = acctsel.Result[0] as AccountTitleInfo;
                    dgExpenseItem.CurrentCell.Value = attmp.AccountCode + attmp.AccLevelName;
                    dgExpenseItem.CurrentCell.Tag = attmp;
                }
                dgExpenseItem.RefreshEdit();
            }
        }

        private void InitViewData()
        {
            this.ExpItemType.DataSource = EnumUtil<enmExpItemType>.GetDescriptions();
        }
        private void InitOperationTache()
        {
            this.OperationTache.DataSource = Enum.GetNames(typeof(enmOperationTache));
        }
        void menuItemDeleteRow_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确实要删除选择的行吗？", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in this.dgExpenseItem.SelectedRows)
                {
                    if (row.IsNewRow)
                        break;

                    ExpenseItem item = row.Tag as ExpenseItem;
                    if (item != null)
                    {
                        mExpenseItem.Delete(item);
                        this.dgExpenseItem.Rows.Remove(row);
                    }
                }
            }
        }

        public void Start(string name)
        {
            mExpenseItem.New();
            this.RefreshControls(MainViewState.Initialize);
        }


        public override bool ModifyView()
        {
            this.RefreshControls(MainViewState.Modify);
            return true;
        }



        public override bool SaveView()
        {
            this.dgExpenseItem.FindForm().Validate();

            if (ViewToModel())
            {
                mExpenseItem.Save();
                this.RefreshControls(MainViewState.Browser);
                return true;
            }



            return false;
        }


        public bool ViewToModel()
        {
            if (!VerifyView())
                return false;
            if (!VerifyExpItemType())
                return false;

            mExpenseItem.expenseItemList.Clear();

            foreach (DataGridViewRow row in this.dgExpenseItem.Rows)
            {
                if (row.IsNewRow)
                    break;

                ExpenseItem item = row.Tag as ExpenseItem;
                if (item == null)
                    item = new ExpenseItem();

                item.Code = row.Cells["Code"].Value.ToString();
                item.Name = row.Cells["TacheName"].Value.ToString();
                item.StockLine =ClientUtil.ToBool( row.Cells["StockLine"].Value);
                item.SaleLine =ClientUtil.ToBool( row.Cells["SaleLine"].Value);
                item.InComeExp =ClientUtil.ToBool( row.Cells["InComeExp"].Value);
                item.OutPutExp =ClientUtil.ToBool( row.Cells["OutPutExp"].Value);
                item.Hoisting =ClientUtil.ToBool( row.Cells["Hoisting"].Value);

                string temp = row.Cells["OperationTache"].Value.ToString();
                item.OperTache = (enmOperationTache)Enum.Parse(typeof(enmOperationTache), temp);
                item.ExpItemType = EnumUtil<enmExpItemType>.FromDescription(ClientUtil.ToString(row.Cells["ExpItemType"].Value));
                if (row.Cells["Descript"].Value != null)
                    item.Descript = row.Cells["Descript"].Value.ToString();

                
                item.AccountTitle= row.Cells["AccountTitle"].Tag as AccountTitleInfo;

                mExpenseItem.expenseItemList.Add(item);
            }

            return true;
        }

        private bool VerifyExpItemType()
        {
            int i = 0;
            int t = 0;
            int O = 0;
            int P = 0;
            int H = 0;
            string err="";
            bool isError = false;
            string stockmoney = EnumUtil<enmExpItemType>.GetDescription(enmExpItemType.stockmoney);
            string salemoney = EnumUtil<enmExpItemType>.GetDescription(enmExpItemType.salemoney);
            string outMakemoney = EnumUtil<enmExpItemType>.GetDescription(enmExpItemType.outMakemoney);
            string Processingmoney = EnumUtil<enmExpItemType>.GetDescription(enmExpItemType.Processingmoney);
            string Hoistingmoney = EnumUtil<enmExpItemType>.GetDescription(enmExpItemType.Hoistingmoney);

            foreach (DataGridViewRow row in this.dgExpenseItem.Rows)
            {
                if (ClientUtil.ToString(row.Cells["ExpItemType"].Value) == stockmoney)
                {
                    i++;
                }
                if (ClientUtil.ToString(row.Cells["ExpItemType"].Value) == salemoney)
                {
                    t++;
                }

                if (ClientUtil.ToString(row.Cells["ExpItemType"].Value) == outMakemoney)
                {
                    O++;
                }
                if (ClientUtil.ToString(row.Cells["ExpItemType"].Value) == Processingmoney)
                {
                    P++;
                }
                if (ClientUtil.ToString(row.Cells["ExpItemType"].Value) == Hoistingmoney)
                {
                    H++;
                }
            }
            if (i>1)
            {
                err=err + "费用类型存在多个【"+stockmoney+"】请检查！" + "\t\n";
                isError = true;
            }
            if (t > 1)
            {
                err=err + "费用类型存在多个【"+salemoney+"】请检查！" + "\t\n";
                isError = true;
            }

            if (O > 1)
            {
                err = err + "费用类型存在多个【" + outMakemoney + "】请检查！" + "\t\n";
                isError = true;
            }
            if (P > 1)
            {
                err = err + "费用类型存在多个【" + Processingmoney + "】请检查！" + "\t\n";
                isError = true;
            }
            //if (H > 1)
            //{
            //    err = err + "费用类型存在多个【" + Hoistingmoney + "】请检查！" + "\t\n";
            //    isError = true;
            //}
            if (isError)
            {
                MessageBox.Show(err);
                return false;
            }
            return true;
        }


        private bool VerifyView()
        {
            foreach (DataGridViewRow row in this.dgExpenseItem.Rows)
            {

                if (row.IsNewRow)
                    break;

                if (ClientUtil.isEmpty(row.Cells["Code"].Value))
                {
                    MessageBox.Show("第" + (row.Index + 1) + "行的编码为空");
                    return false;
                }

                if (ClientUtil.isEmpty(row.Cells["TacheName"].Value))
                {
                    MessageBox.Show("第" + (row.Index + 1) + "行的名称为空");
                    return false;
                }

                if (ClientUtil.isEmpty(row.Cells["OperationTache"].Value))
                {
                    MessageBox.Show("第" + (row.Index + 1) + "行的业务环节为空");
                    return false;
                }

            }

            return true;
        }



        private void InitData()
        {
            mExpenseItem.FindAll();
            ModelToView();
        }

        private void ModelToView()
        {
            if (mExpenseItem.expenseItemList != null && mExpenseItem.expenseItemList.Count > 0)
            {
                this.dgExpenseItem.Rows.Clear();
                foreach (ExpenseItem item in mExpenseItem.expenseItemList)
                {
                    int rowIndex = this.dgExpenseItem.Rows.Add();
                    DataGridViewRow row = this.dgExpenseItem.Rows[rowIndex];
                    row.Cells["Code"].Value = item.Code;
                    row.Cells["TacheName"].Value = item.Name;
                    row.Cells["OperationTache"].Value = Enum.GetName(typeof(enmOperationTache), item.OperTache);
                    row.Cells["ExpItemType"].Value = EnumUtil<enmExpItemType>.GetDescription(item.ExpItemType);
                    row.Cells["Descript"].Value = item.Descript;

                    row.Cells["StockLine"].Value = item.StockLine;
                    row.Cells["SaleLine"].Value = item.SaleLine;
                    row.Cells["InComeExp"].Value = item.InComeExp;
                    row.Cells["OutPutExp"].Value = item.OutPutExp;
                    row.Cells["Hoisting"].Value = item.Hoisting;

                    if (item.AccountTitle != null)
                    {
                        row.Cells["AccountTitle"].Value = item.AccountTitle.AccountCode + item.AccountTitle.AccLevelName;
                        row.Cells["AccountTitle"].Tag = item.AccountTitle;
                    }
                    row.Tag = item;
                }
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            RefreshControl(state, this.pnlFloor);
        }


        private void RefreshControl(MainViewState state, Control c)
        {
            foreach (Control cd in c.Controls)
            {

                RefreshControl(state, cd);
            }
            //自定义控件清空

            if (c is CustomDataGridView)
            {
                if (state == MainViewState.Initialize || state == MainViewState.Browser)
                    c.Enabled = false;
                else if (state == MainViewState.AddNew || state == MainViewState.Modify)
                    c.Enabled = true;
            }



        }


    }
}
