using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using System.Collections;

using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewCollectRule : Form
    {
        public ViewMain vMain = new ViewMain();
        private MCubeManager model = new MCubeManager();
        private IList style_list;

        public VViewCollectRule()
        {
            InitializeComponent();
            InitialEvents();
        }

        private void VViewCollectRule_Load(object sender, EventArgs e)
        {
            InitialControls();
            LoadRuleDefData();
        }

        private void InitialEvents()
        {                        
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        private void InitialControls()
        {
            style_list = model.ViewService.GetViewStyleByViewMain(vMain, false);
            foreach (ViewStyle vs in style_list)
            {
                if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) == -1 && vs.OldCatRootName.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) == -1)
                {
                    DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                    column.HeaderText = vs.OldCatRootName;
                    column.DataSource = vs.Details;
                    column.DisplayMember = "Name";
                    column.ValueMember = "DimCatId";
                    dgvData.Columns.Add(column);
                }
            }
            DataGridViewTextBoxColumn data_sign = new DataGridViewTextBoxColumn();
            data_sign.HeaderText = "代号";
            data_sign.Width = 60;
            dgvData.Columns.Add(data_sign);

            DataGridViewTextBoxColumn express = new DataGridViewTextBoxColumn();
            express.HeaderText = "计算表达式";
            express.Width = 180;
            dgvData.Columns.Add(express);
        }

        /// <summary>
        /// 引导规则数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadRuleDefData()
        {
            dgvData.Rows.Clear();
            if (vMain == null)
            {
                return;
            }
            IList rule_list = model.ViewService.GetViewRuleDefByViewMain(vMain);

            foreach (ViewRuleDef vrd in rule_list)
            {

                if (vrd.SaveExpress != null && !"".Equals(vrd.SaveExpress))
                {
                    int i = dgvData.Rows.Add();
                    DataGridViewRow data_row = dgvData.Rows[i];

                    int k = 0;
                    string link_str = vrd.SaveExpress;
                    char[] patten = { '_' };//分隔符
                    string[] temp = link_str.Split(patten);
                    for (int m = 1; m < temp.Length; m++)
                    {
                        data_row.Cells[k].Value = long.Parse(temp[m]);
                        k++;
                    }

                    data_row.Cells[k].Value = vrd.CellSign;
                    data_row.Cells[k + 1].Value = vrd.CalExpress;
                    data_row.Tag = vrd;
                }
            }
        }


        /// <summary>
        /// 数据区保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0)
            {
                try
                {
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        ViewRuleDef vrd = row.Tag as ViewRuleDef;
                        if (vrd == null)
                        {
                            vrd = new ViewRuleDef();
                        }

                        vrd.Main = vMain;
                        string link_str = "";
                        int i = 0;
                        foreach (ViewStyle vs in style_list)
                        {
                            if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) == -1 && vs.OldCatRootName.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) == -1)
                            {
                                if (row.Cells[i].Value == null || "".Equals(row.Cells[i].Value))
                                {
                                    MessageBox.Show("第" + (row.Index + 1) + "行,第" + (i + 1) + "列不能为空！");
                                    return;
                                }
                                link_str += "_" + KnowledgeUtil.TransToString(row.Cells[i].Value);
                                i++;
                            }
                            
                        }
                        if (row.Cells[i].Value == null || "".Equals(row.Cells[i].Value))
                        {
                            MessageBox.Show("第" + (row.Index + 1) + "行‘单元格代号’不能为空！");
                            return;
                        }
                        vrd.CellSign = KnowledgeUtil.TransToString(row.Cells[i].Value);
                        vrd.CalExpress = KnowledgeUtil.TransToString(row.Cells[i + 1].Value);
                        vrd.SaveExpress = link_str;
                        model.ViewService.SaveViewRuleDef(vrd);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存数据区规则定义出错：" + StaticMethod.ExceptionMessage(ex));
                    return;
                }
                MessageBox.Show("保存数据区规则定义完成！");
                LoadRuleDefData();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}