using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.BasicData.Domain;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewRuleDef : Form
    {
        public ViewMain vMain = new ViewMain();
        private string ifYwzz;
        private string ifTime;
        private MCubeManager model = new MCubeManager();
        private IList style_list;

        public VViewRuleDef()
        {
            InitializeComponent();
            InitialEvents();
            cboVariable.DataSource = model.InitRuleVarType() ;
        }

        private void VViewRuleDef_Load(object sender, EventArgs e)
        {
            InitialControls();
            LoadRuleDefData();
        }

        private void InitialEvents()
        {                        
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnAdd1.Click += new EventHandler(btnAdd1_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSave1.Click += new EventHandler(btnSave1_Click);
            btnDel.Click += new EventHandler(btnDel_Click);
            btnDel1.Click += new EventHandler(btnDel1_Click);
            dgvDisplay.CellContentClick += new DataGridViewCellEventHandler(dgvDisplay_CellContentClick);
            
            btnInsert.Click += new EventHandler(btnInsert_Click);
            btnExpress.Click += new EventHandler(btnExpress_Click);
            btnClear.Click += new EventHandler(btnClear_Click);

            btnCheck.Click += new EventHandler(btnCheck_Click);
        }

        private void InitialControls()
        {
            style_list = model.ViewService.GetViewStyleByViewMain(vMain, false);
            ifYwzz = vMain.IfYwzz;
            ifTime = vMain.IfTime;
            foreach (ViewStyle vs in style_list)
            {
                if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) == -1 && (vs.OldCatRootName.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) == -1 || "1".Equals(ifYwzz)))
                {                   
                    //特殊处理，因为ComboBoxColumn显示的名称相同时，取不到正确的value
                    IList trans_mx_list = new ArrayList();//处理后的集合
                    IList temp = new ArrayList();
                    int i = 1;
                    foreach (ViewStyleDimension vsd in vs.Details)
                    {
                        string _name = vsd.Name;
                        if (!temp.Contains(_name))
                        {
                            temp.Add(_name);
                        }
                        else {
                            _name = _name + "_" + i;
                            vsd.Name = _name;
                            temp.Add(_name);
                            i++;
                        }
                        trans_mx_list.Add(vsd);
                    }

                    DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                    if (vs.OldCatRootName.IndexOf("指标") != -1)
                    {
                        column.Width = 200;
                    }
                    column.HeaderText = vs.OldCatRootName;
                    column.DataSource = trans_mx_list;
                    column.DisplayMember = "Name";
                    column.ValueMember = "DimCatId";
                    dgvData.Columns.Add(column);
                }
            }

            IList factList = model.GetFactDefineByCubeRegisterId(vMain.CubeRegId.Id);
            DataGridViewComboBoxColumn colFact = new DataGridViewComboBoxColumn();
            colFact.HeaderText = "事实";
            colFact.Name = "colFact";
            colFact.Width = 80;
            colFact.DataSource = factList;
            colFact.DisplayMember="FactName";
            colFact.ValueMember="Id";
            dgvData.Columns.Add(colFact);

            if ( "1".Equals(vMain.IfTime) )
            {
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                column.HeaderText = "时间变量";
                IList list = new ArrayList();
                for (int i = 0; i < KnowledgeUtil.TimeVarCode.Length; i++)
                {
                    IndicatorBasicValue ibv = new IndicatorBasicValue();
                    ibv.Code = KnowledgeUtil.TimeVarCode[i].ToString();
                    ibv.Name = KnowledgeUtil.TimeVarName[i].ToString();
                    list.Add(ibv);
                }
                column.DataSource = list;
                column.DisplayMember = "Name";
                column.ValueMember = "Code";
                dgvData.Columns.Add(column);
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
        /// 校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCheck_Click(object sender, EventArgs e)
        {
            string str = CheckExpress();
            if (!"".Equals(str))
            {
                MessageBox.Show(str);
                return;
            }
            MessageBox.Show("校验通过！");
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="sender"></param>
        private string CheckExpress()
        {
            string return_str = "";
            IList sign_list = new ArrayList();
            if (dgvData.Rows.Count > 0)
            {
                int r = 1;
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    int i = 1;
                    foreach (ViewStyle vs in style_list)
                    {
                        if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) == -1 && (vs.OldCatRootName.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) == -1 || "1".Equals(ifYwzz)))
                        {
                            i++;
                        }
                    }

                    if("1".Equals(ifTime)){
                        i++;
                    }

                    //跳过事实列
                    i++;

                    string sign = KnowledgeUtil.TransToString(row.Cells[i].Value);
                    if (!sign_list.Contains(sign))
                    {
                        sign_list.Add(sign);
                    }
                    else
                    {
                        return "第" + r + "行 代号 " + sign + " 重复！";
                    }

                    string express = KnowledgeUtil.TransToString(row.Cells[i + 1].Value);
                    int firstIndex = express.IndexOf("[");
                    int lastIndex = express.IndexOf("]");
                    if (firstIndex >= 0 && lastIndex > 0)
                    { 
                        //把括号里的字符串替换为1
                        string idStr = express.Substring(firstIndex, lastIndex - firstIndex + 1);
                        express = express.Replace(idStr, "1");
                    }
                    int t = 0;
                    int k = 0;
                    if (express != null && !"".Equals(express))
                    {
                        t = KnowledgeUtil.CheckEnergyExpressByZb(express);
                    }
                    if (sign != null && !"".Equals(sign))
                    {
                        k = KnowledgeUtil.CheckEnergyExpress(sign);
                    }
                    if (t == -1 || k == -1)
                    {
                        return "第" + r + "行 代号或者表达式 中存在非法字符！";
                    }
                    else if (t == -2)
                    {
                        return "第" + r + "行中 表达式“[]”个数不等！";
                    }
                    else if (t == -3)
                    {
                        return "第" + r + "行中 表达式“()”个数不等！";
                    }
                    else if (t == -4)
                    {
                        return "第" + r + "行中 表达式“:”存在问题！";
                    }
                    r++;
                }

            }
            return return_str;
        }

        /// <summary>
        /// 引导规则数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadRuleDefData()
        {
            dgvData.Rows.Clear();
            dgvDisplay.Rows.Clear();
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

                    int k = 1;
                    string link_str = vrd.SaveExpress;
                    string[] temp = link_str.Split(MCubeManager.SaveExpressDelimiter,StringSplitOptions.None);
                    for (int m = 1; m < temp.Length; m++)
                    {
                        data_row.Cells[k].Value =temp[m];
                        k++;
                    }

                    if ("1".Equals(vMain.IfTime))
                    {
                        data_row.Cells[k].Value = vrd.TimeVar;
                        k++;
                    }

                    data_row.Cells[k].Value = vrd.CellSign;
                    data_row.Cells[k + 1].Value = vrd.CalExpress;
                    data_row.Tag = vrd;
                }
                else
                {
                    int j = dgvDisplay.Rows.Add();
                    DataGridViewRow display_row = dgvDisplay.Rows[j];
                    display_row.Tag = vrd;
                    display_row.Cells[sign.Name].Value = vrd.CellSign;
                    display_row.Cells[rule.Name].Value = vrd.DisplayRule;
                }
            }
        }

        /// <summary>
        /// 数据区新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAdd_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Add();
        }

        /// <summary>
        /// 数据区删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.SelectedRows)
                {
                    ViewRuleDef vrd = row.Tag as ViewRuleDef;
                    dgvData.Rows.Remove(row);
                    if (vrd != null && !string.IsNullOrEmpty(vrd.Id))
                    {
                        model.ViewService.DeleteViewRuleDef(vrd);
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的数据规则行！");
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
                string str = CheckExpress();
                if (!"".Equals(str))
                {
                    MessageBox.Show(str);
                    return;
                }

                try
                {
                    IList insert_list = new ArrayList();
                    IList update_list = new ArrayList();
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        ViewRuleDef vrd = row.Tag as ViewRuleDef;
                        if (vrd == null)
                        {
                            vrd = new ViewRuleDef();
                        }

                        vrd.Main = vMain;
                        string link_str = "";
                        int i = 1;
                        foreach (ViewStyle vs in style_list)
                        {
                            if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) == -1 && (vs.OldCatRootName.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) == -1 || "1".Equals(ifYwzz)))
                            {
                                if (row.Cells[i].Value == null || "".Equals(row.Cells[i].Value))
                                {
                                    MessageBox.Show("第" + (row.Index + 1) + "行,第" + (i + 1) + "列不能为空！");
                                    return;
                                }
                                link_str += MCubeManager.SaveExpressDelimiter[0] + KnowledgeUtil.TransToString(row.Cells[i].Value);
                                i++;
                            }
                            
                        }
                        //加上事实
                        object objFact = row.Cells["colFact"].Value;
                        if (objFact == null || objFact.ToString().Equals(""))
                        {
                            MessageBox.Show("事实列不能为空。");
                            dgvData.CurrentCell = row.Cells["colFact"];
                            return;
                        }
                        i++;
                        link_str += MCubeManager.SaveExpressDelimiter[0] + objFact.ToString();

                        if (row.Cells[i].Value == null || "".Equals(row.Cells[i].Value))
                        {
                            MessageBox.Show("第" + (row.Index + 1) + "行‘单元格代号’不能为空！");
                            return;
                        }

                        if ("1".Equals(vMain.IfTime))
                        {
                            vrd.TimeVar = KnowledgeUtil.TransToString(row.Cells[i].Value);
                            i++;
                        }

                        vrd.CellSign = KnowledgeUtil.TransToString(row.Cells[i].Value);
                        vrd.CalExpress = KnowledgeUtil.TransToString(row.Cells[i + 1].Value);
                        vrd.SaveExpress = link_str;
                        if (!string.IsNullOrEmpty(vrd.Id))
                        {
                            update_list.Add(vrd);
                        }
                        else {
                            insert_list.Add(vrd);
                        }
                        //model.ViewService.SaveViewRuleDef(vrd);
                    }
                    if (insert_list.Count > 0) {
                        model.ViewService.BatchUpdateRuleDefByProc(1, insert_list);
                    }

                    if (update_list.Count > 0)
                    {
                        model.ViewService.BatchUpdateRuleDefByProc(2, update_list);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存数据区规则定义出错：" + ExceptionUtil.ExceptionMessage(ex));
                    return;
                }
                MessageBox.Show("保存数据区规则定义完成！");
                LoadRuleDefData();
            }
        }

        /// <summary>
        /// 显示区新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAdd1_Click(object sender, EventArgs e)
        {
            dgvDisplay.Rows.Add();
        }

        /// <summary>
        /// 显示区删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDel1_Click(object sender, EventArgs e)
        {
            if (dgvDisplay.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDisplay.SelectedRows)
                {
                    ViewRuleDef vrd = row.Tag as ViewRuleDef;
                    dgvDisplay.Rows.Remove(row);
                    if (vrd != null && !string.IsNullOrEmpty(vrd.Id))
                    {
                        model.ViewService.DeleteViewRuleDef(vrd);
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的显示区规则行！");
            }
        }

        /// <summary>
        /// 显示区保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave1_Click(object sender, EventArgs e)

        {
            if (dgvDisplay.Rows.Count > 0)
            {
                try
                {
                    foreach (DataGridViewRow row in dgvDisplay.Rows)
                    {
                        ViewRuleDef vrd = row.Tag as ViewRuleDef;
                        if (vrd == null)
                        {
                            vrd = new ViewRuleDef();
                        }
                        if (row.Cells[sign.Name].Value == null || "".Equals(row.Cells[sign.Name].Value))
                        {
                            MessageBox.Show("第" + (row.Index + 1) + "行‘代号’不能为空！");
                            return;
                        }
                        if (row.Cells[rule.Name].Value == null || "".Equals(row.Cells[rule.Name].Value))
                        {
                            MessageBox.Show("第" + (row.Index + 1) + "行‘规则表达式’不能为空！");
                            return;
                        }
                        vrd.Main = vMain;
                        vrd.CellSign = KnowledgeUtil.TransToString(row.Cells[sign.Name].Value);
                        vrd.DisplayRule = KnowledgeUtil.TransToString(row.Cells[rule.Name].Value);

                        model.ViewService.SaveViewRuleDef(vrd);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存显示区规则定义出错！" + StaticMethod.ExceptionMessage(ex));
                    return;
                }
                MessageBox.Show("保存显示区规则定义完成！");
                LoadRuleDefData();
            }
        }

        /// <summary>
        /// 插入表达式变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnInsert_Click(object sender, EventArgs e)
        {
            string curr_express = rtbExpress.Text;
            string cbo_text = cboVariable.Text;//下拉框的值
            if ("文本".Equals(cbo_text) && (edtText.Text == null || "".Equals(edtText.Text)))
            {
                MessageBox.Show("文本框不能为空！");
                edtText.Focus();
                return;
            }


            if ("文本".Equals(cbo_text))
            {
                curr_express += edtText.Text;
            }
            else
            {
                curr_express += cbo_text;
            }
            rtbExpress.Text = curr_express;

        }

        /// <summary>
        /// 把规则表达式插入显示区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExpress_Click(object sender, EventArgs e)
        {
            dgvDisplay.CurrentRow.Cells[1].Value = rtbExpress.Text;
        }

        /// <summary>
        /// 清空规则表达式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClear_Click(object sender, EventArgs e)
        {
            rtbExpress.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvDisplay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string curr_express = KnowledgeUtil.TransToString(dgvDisplay.CurrentRow.Cells[1].Value);
            rtbExpress.Text = curr_express;
        }

        
    }
}