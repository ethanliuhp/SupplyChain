using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VFundSchemeFormulaCheck : Form
    {
        public VFundSchemeFormulaCheck()
        {
            InitializeComponent();

            btnStartCheck.Click += btnStartCheck_Click;

            InitCheckItem();
        }

        private void InitCheckItem()
        {
            lvCheckItems.Items.Clear();
            lvCheckItems.Items.Add(string.Empty);

            var formulas = new List<string>();
            formulas.Add("表5项目进度报量及成本测算表.合计进项税 - 表6项目成本价税测算表.合计进项税 = 0");
            formulas.Add("表5项目进度报量及成本测算表.合计本期成本小计 - 表6项目成本价税测算表.合计小计 = 0");
            formulas.Add("表9项目资金收款测算表.合计累计含税报量小计 - 表9项目资金收款测算表.合计累计收含税价款小计 = 0");
            formulas.Add("表10项目资金支付款测算表.合计累计资金支付款 - 表5项目进度报量及成本测算表.合计本期成本小计 - 表5项目进度报量及成本测算表.合计进项税 - 表5项目进度报量及成本测算表.合计本期应支付增值税 - 表5项目进度报量及成本测算表.合计计提税金附加 - 表5项目进度报量及成本测算表.合计财务费用 = 0");
            formulas.Add("表4项目资金策划表.合计累计自行收款 + 表4项目资金策划表.合计累计内部安装收款 + 表4项目资金策划表.合计累计甲分包收款 + 表4项目资金策划表.合计销项税额 - 表9项目资金收款测算表.合计累计收含税价款小计 = 0");
            formulas.Add("表4项目资金策划表.合计累计自行支出 + 表4项目资金策划表.合计累计内部安装支出 + 表4项目资金策划表.合计累计甲分包支出 + 表4项目资金策划表.合计支付增值税额 + 表4项目资金策划表.合计支付附加费 - 表10项目资金支付款测算表.合计累计资金支付款 = 0");
            formulas.Add("表4项目资金策划表.合计累计自行收款 + 表4项目资金策划表.合计累计内部安装收款 + 表4项目资金策划表.合计累计甲分包收款 + 表4项目资金策划表.合计销项税额 - 表5项目进度报量及成本测算表.合计累计含税报量小计 = 0");

            foreach (var fs in formulas)
            {
                var item = new ListViewItem(fs);
                item.SubItems.Add("-");

                lvCheckItems.Items.Add(item);
            }
        }

        public delegate void FormulaStartCheckClickEvent(ListView sender);

        public FormulaStartCheckClickEvent FormulaStartCheckClick;

        private void btnStartCheck_Click(object sender, EventArgs e)
        {
            if (FormulaStartCheckClick != null)
            {
                FormulaStartCheckClick(lvCheckItems);
            }
        }
    }
}
