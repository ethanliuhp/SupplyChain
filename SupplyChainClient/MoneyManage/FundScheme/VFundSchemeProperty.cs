using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class VFundSchemeProperty : TBasicDataView
    {
        private FundSchemeOperate fundSchemeOperate;
        private CustomFlexGrid selectGrid;
        private FundSchemeCell activeCell;

        public VFundSchemeProperty(FundSchemeOperate fundOperate, CustomFlexGrid grid, FundSchemeCell cell)
        {
            InitializeComponent();

            tabControl1.SelectedIndex = 1;

            fundSchemeOperate = fundOperate;
            selectGrid = grid;
            activeCell = cell;

            ShowPropertys();
        }

        public void ShowPropertys(FundSchemeOperate fundOperate, CustomFlexGrid grid, FundSchemeCell cell)
        {
            fundSchemeOperate = fundOperate;
            selectGrid = grid;
            activeCell = cell;

            ShowPropertys();
        }

        public void ShowCheckResult(Dictionary<string,List<string>> result)
        {
            lvCheckResult.Items.Clear();
            foreach (var res in result)
            {
                var item = new ListViewItem(res.Key);
                item.SubItems.AddRange(res.Value.ToArray());
                lvCheckResult.Items.Add(item);
            }
        }

        private void ShowPropertys()
        {
            GetGridInfo();

            GetCellInfo();
        }

        private void AddPropertyItems(ListView lv, Dictionary<string, string> pValues)
        {
            lv.Items.Clear();

            foreach (var pValue in pValues)
            {
                var item = new ListViewItem(pValue.Key);
                item.SubItems.Add(pValue.Value);
                lv.Items.Add(item);
            }
        }

        private void GetGridInfo()
        {
            if (fundSchemeOperate == null || selectGrid == null || activeCell == null)
            {
                return;
            }

            var tbPropertys = new Dictionary<string, string>();
            tbPropertys.Add("中文名称", activeCell.TableName);
            tbPropertys.Add("英文名称", selectGrid.Name);
            tbPropertys.Add("总行数", selectGrid.Rows.ToString());
            tbPropertys.Add("总列数", selectGrid.Cols.ToString());
            tbPropertys.Add("公式个数", fundSchemeOperate.GetFormulaCount(selectGrid.Name).ToString());

            AddPropertyItems(lvTableProperty, tbPropertys);
        }

        private void GetCellInfo()
        {
            if (fundSchemeOperate == null || selectGrid == null || activeCell == null)
            {
                return;
            }

            var tbPropertys = new Dictionary<string, string>();
            tbPropertys.Add("单元格地址", activeCell.CellAddress);
            tbPropertys.Add("所在行", activeCell.RowIndex.ToString());
            tbPropertys.Add("所在列", activeCell.ColIndex.ToString());
            tbPropertys.Add("格式化", activeCell.Formatter);
            tbPropertys.Add("原值", activeCell.CellValue.ToString());
            tbPropertys.Add("格式化值",
                            !string.IsNullOrEmpty(activeCell.Formatter)
                                ? (activeCell.CellValue ?? 0).ToString(activeCell.Formatter)
                                : activeCell.CellValue.ToString());
            tbPropertys.Add("绑定对象", activeCell.BindObject.ToString());
            tbPropertys.Add("绑定属性", activeCell.DataMember);
            tbPropertys.Add("是否只读", activeCell.Formula == null ? "否" : "是");

            AddPropertyItems(lvCellProperty, tbPropertys);

            if(activeCell.Formula　==　null)
            {
                txtFormular.Text = string.Empty;
            }
            else
            {
                txtFormular.Text = activeCell.Formula.FormulaExpression;
            }
        }
   }
}
