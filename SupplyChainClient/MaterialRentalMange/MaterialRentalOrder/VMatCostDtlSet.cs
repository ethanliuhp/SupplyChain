using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder
{
    public enum SetType
    {
        价格定义,
        公式定义,
        维修设置
    }
    public partial class VMatCostDtlSet : TBasicDataView
    {
        public IList Result;
        public IList list_BasicCostSet;
        BasicDtlCostSet dtlCostSet = null;
        public Material Material;
        public VMatCostDtlSet(Material material, IList list)
        {
            Material = material;
            list_BasicCostSet = list;
            InitializeComponent();
            InitData(list);
            InitEvent();
        }

        private void InitData(IList list)
        {
            //初始化料具费用类型
            //VBasicDataOptr.InitMatCostType(colCostType, true);
            colCostType.Items.Clear();
            colCostType.Items.Add("");
            foreach (BasicCostSet CostSet in list)
            {
                colCostType.Items.Add(CostSet.MatCostType);
            }
            //初始化料具维修内容
            VBasicDataOptr.InitMatRepairCon(colWorkContent, true, Material);
            //初始化价格定义
            VBasicDataOptr.InitPriceType(colPriceDefine, true);
            //初始化价格类型
            VBasicDataOptr.InitPriceType(cbPriceType, true);
            cbPriceType.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cbPriceType.Items.Count > 1)
            {
                cbPriceType.SelectedIndex = 1;
            }
            cbMatState.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cbMatState.Items.Count > 0)
            {
                cbMatState.SelectedIndex = 0;
            }
        }

        private void InitEvent()
        {
            btnClearExp.Click += new EventHandler(btnClearExp_Click);
            btnOk.Click += new EventHandler(btnOk_Click);
            btnInsert.Click += new EventHandler(btnInsert_Click);
            btnConfirm.Click += new EventHandler(btnConfirm_Click);
            btnCancle.Click += new EventHandler(btnCancle_Click);
            dgExpression.CellDoubleClick += new DataGridViewCellEventHandler(dgExpression_CellDoubleClick);
            dgExpression.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgExpression_RowHeaderMouseClick);
            dgPrice.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgPrice_RowHeaderMouseClick);
            dgRepair.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgRepair_RowHeaderMouseClick);
            dgExpressionDelete.Click += new EventHandler(dgExpressionDelete_Click);
            dgPriceDelete.Click += new EventHandler(dgPriceDelete_Click);
            dgRepairDelete.Click += new EventHandler(dgRepairDelete_Click);
            cbMatState .SelectedIndexChanged +=new EventHandler(cbMatState_SelectedIndexChanged);
            cbPriceType .SelectedIndexChanged +=new EventHandler(cbPriceType_SelectedIndexChanged);
        }
          public void   cbMatState_SelectedIndexChanged (object sender,EventArgs e)
           {
              
               cbPriceType.SelectedIndexChanged -= new EventHandler(cbPriceType_SelectedIndexChanged);
               SetSelectText(cbMatState, cbPriceType);
               
               cbPriceType.SelectedIndexChanged += new EventHandler(cbPriceType_SelectedIndexChanged);
           }
          public void cbPriceType_SelectedIndexChanged(object sender, EventArgs e)
           {
               cbMatState.SelectedIndexChanged -= new EventHandler(cbMatState_SelectedIndexChanged);
               SetSelectText(cbPriceType,cbMatState);
               cbMatState.SelectedIndexChanged += new EventHandler(cbMatState_SelectedIndexChanged);
           }
          public void SetSelectText(VirtualMachine.Component.WinControls.Controls.CustomComboBox oCbFirst,VirtualMachine.Component.WinControls.Controls.CustomComboBox oCbSecond)
          {
              if (oCbFirst.SelectedIndex == 0)
              {
                  if (oCbSecond.SelectedIndex != 0)
                  {
                  }
                  else
                  {
                      oCbSecond.SelectedIndex = 1;
                  }
              }
              else
              {
                  if (oCbSecond.SelectedIndex != 0)
                  {
                      oCbSecond.SelectedIndex = 0;
                  }
                  else
                  {
                     
                  }
              }
          }
        void dgRepair_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgRepair.PointToClient(Cursor.Position);
                this.contextMenuStrip3.Show(dgRepair, _Point);
            }
        }

        void dgRepairDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgRepair.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgRepair.Rows.Remove(dr);
            }
        }

        void dgPrice_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgPrice.PointToClient(Cursor.Position);
                this.contextMenuStrip1.Show(dgPrice, _Point);
            }
        }

        void dgPriceDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgPrice.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgPrice.Rows.Remove(dr);
            }
        }

        void dgExpression_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgExpression.PointToClient(Cursor.Position);
                this.contextMenuStrip2.Show(dgExpression, _Point);
            }
        }

        void dgExpressionDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgExpression.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgExpression.Rows.Remove(dr);
            }
        }

        void dgExpression_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = "";
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
           
            DataGridViewRow theCurrentRow = dgExpression.CurrentRow;
            if (theCurrentRow.Cells[colCostType.Name].Value == null)
            {
                MessageBox.Show("请先选择[费用类型]，再定义进场或者退场公式！");
                return;
            }
           
            //当前费用类型
            string costType = theCurrentRow.Cells[colCostType.Name].Value.ToString();
            foreach (BasicCostSet CostSet in list_BasicCostSet)
            {
                if (costType == CostSet.MatCostType)
                {
                    cbMatState.Items.Clear();
                    colName = dgExpression.Columns[e.ColumnIndex].Name;
                    if (colName == colApproachExpression.Name)
                    {
                        if (CostSet.ApproachCalculation == 1)
                        {
                            this.gbExpression.Enabled = true;
                        }
                        else
                        {
                            this.gbExpression.Enabled = false;
                        }
                        //料具状态（进场）
                        cbMatState.Items.Clear();
                        this.cbMatState.Items.AddRange(new object[] { "","收料" });
                        this.cbMatState.SelectedIndex = 0;

                    }
                    else if (colName == colExitExpression.Name)
                    {
                        if (CostSet.ExitCalculation == 1)
                        {
                            this.gbExpression.Enabled = true;
                        }
                        else
                        {
                            this.gbExpression.Enabled = false;
                        }
                        //料具状态（退场）
                        cbMatState.Items.Clear();
                        this.cbMatState.Items.AddRange(new object[] { "","完好", "报损", "报废", "消耗", "切头", "维修" });
                        this.cbMatState.SelectedIndex = 0;
                    }
                    else
                    {
                        this.gbExpression.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConfirm_Click(object sender, EventArgs e)
        {
            //校验重复数据
            string str_expression = "";
            string str_price = "";
            string str_repair = "";

            #region 校验数据完成性
            //校验价格定义的完整性
            foreach (DataGridViewRow var in dgPrice.Rows)
            {
                if (var.IsNewRow) break;
                if (var.Cells[colPriceDefine.Name].Value == null)
                {
                    MessageBox.Show("[价格定义]不能空！");
                    dgPrice.CurrentCell = var.Cells[colPriceDefine.Name];
                    return;
                }
                if (var.Cells[colPrice.Name].Value == null || var.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value) < 0)
                {
                    MessageBox.Show("[价格定义]中[单价]不允许为空或者小于0！");
                    dgPrice.CurrentCell = var.Cells[colPrice.Name];
                    return;
                }
            }

            //校验维修内容的完整性
            foreach (DataGridViewRow var in dgRepair.Rows)
            {
                if (var.IsNewRow) break;
                if (var.Cells[colWorkContent.Name].Value == null)
                {
                    MessageBox.Show("[维修设置]中的[工作内容]不能为空！");
                    //dgPrice.CurrentCell = var.Cells[colWorkContent.Name];
                    return;
                }
                if (var.Cells[colRepairPrice.Name].Value == null || var.Cells[colRepairPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(var.Cells[colRepairPrice.Name].Value) <= 0)
                {
                    MessageBox.Show("[维修设置]中的[维修价格]不允许为空或者小于0！");
                    //dgPrice.CurrentCell = var.Cells[colRepairPrice.Name];
                    return;
                }
            }
            #endregion

            #region 校验重复项
            foreach (DataGridViewRow var in dgExpression.Rows)
            {
                if (var.IsNewRow) break;
                if (str_expression.Contains(var.Cells[colCostType.Name].Value.ToString()))
                {
                    MessageBox.Show("[公式定义]中存在重复[费用类型]，请右键删除！");
                    return;
                }
                str_expression += var.Cells[colCostType.Name].Value.ToString();
            }

            foreach (DataGridViewRow var in dgPrice.Rows)
            {
                if (var.IsNewRow) break;
                if (str_price.Contains(var.Cells[colPriceDefine.Name].Value.ToString()))
                {
                    MessageBox.Show("[价格定义]中存在重复[价格类型]，请右键删除！");
                    return;
                }
                str_price += var.Cells[colPriceDefine.Name].Value.ToString();
            }

            foreach (DataGridViewRow var in dgRepair.Rows)
            {
                if (var.IsNewRow) break;
                if (str_repair.Contains(var.Cells[colWorkContent.Name].Value.ToString()))
                {
                    MessageBox.Show("[维修设置]中存在重复[维修内容]，请右键删除！");
                    return;
                }
                str_repair += var.Cells[colWorkContent.Name].Value.ToString();
            }
            #endregion

            GetBasicDtlCostSet();
            this.Close();
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnInsert_Click(object sender, EventArgs e)
        {
            if (cbPriceType.Text != "")
            {
                AddButtonText(this.cbPriceType.Text);
            }
            if (cbMatState.Text != "")
            {
                AddButtonText(this.cbMatState.Text);
            }

            this.cbMatState.Text = "";
            this.cbPriceType.Text = "";
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancle_Click(object sender, EventArgs e)
        {
            GetBasicDtlCostSet();
            this.Close();
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOk_Click(object sender, EventArgs e)
        {
            this.dgExpression.CurrentRow.Cells[dgExpression.CurrentCell.ColumnIndex].Value = this.txtCalExp.Text;
            this.txtCalExp.Text = "";
            this.gbExpression.Enabled = false;
        }
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClearExp_Click(object sender, EventArgs e)
        {
            txtCalExp.Text = "";
            txtCalExp.Tag = null;
        }

        private void UpdateTxtTag(string text)
        {
            string calExp = txtCalExp.Tag as string;
            if (calExp == null) calExp = "";
            calExp += text;
            txtCalExp.Tag = calExp;
        }

        #region 数字、运算符事件处理

        private void AddButtonText(String text)
        {
            txtCalExp.AppendText(text);
            UpdateTxtTag(text);
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnMulti_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnLeftBracket_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnRightBracket_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }
        #endregion

        public void OpenCostSet(IList list)
        {
            Clear();
            try
            {
                if (list != null)
                {
                    //显示数据
                    //价格定义
                    foreach (BasicDtlCostSet detail in list)
                    {
                        if (detail.SetType == SetType.价格定义.ToString())
                        {
                            int i1 = this.dgPrice.Rows.Add();
                            this.dgPrice[colPriceDefine.Name, i1].Value = detail.CostType;
                            this.dgPrice[colPrice.Name, i1].Value = detail.Price;
                            this.dgPrice[colPriceDescript.Name, i1].Value = detail.Descript;
                        }
                        else if (detail.SetType == SetType.公式定义.ToString())
                        {
                            int i2 = this.dgExpression.Rows.Add();
                            this.dgExpression[colCostType.Name, i2].Value = detail.CostType;
                            this.dgExpression[colApproachExpression.Name, i2].Value = detail.ApproachExpression;
                            this.dgExpression[colExitExpression.Name, i2].Value = detail.ExitExpression;
                            this.dgExpression[colExpressionDescript.Name, i2].Value = detail.Descript;
                        }
                        else if (detail.SetType == SetType.维修设置.ToString())
                        {
                            int i3 = this.dgRepair.Rows.Add();
                            this.dgRepair[colWorkContent.Name, i3].Value = detail.WorkContent;
                            this.dgRepair[colRepairPrice.Name, i3].Value = detail.Price;
                            this.dgRepair[colRepairDescript.Name, i3].Value = detail.Descript;
                        }
                    }
                }
            }
            catch
            {

            }
            ShowDialog();
        }

        private void Clear()
        {
            this.dgExpression.Rows.Clear();
            this.dgPrice.Rows.Clear();
            this.dgRepair.Rows.Clear();
            this.txtCalExp.Text = "";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            GetBasicDtlCostSet();
            base.OnClosing(e);
        }
        private void GetBasicDtlCostSet()
        {
            Result = new ArrayList();
            //价格定义
            foreach (DataGridViewRow var in dgPrice.Rows)
            {
                if (var.IsNewRow) break;
                dtlCostSet = new BasicDtlCostSet();
                dtlCostSet.CostType = ClientUtil.ToString(var.Cells[colPriceDefine.Name].Value);
                dtlCostSet.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                dtlCostSet.Descript = ClientUtil.ToString(var.Cells[colPriceDescript.Name].Value);
                dtlCostSet.SetType = SetType.价格定义.ToString();

                Result.Add(dtlCostSet);
            }
            //公式定义
            foreach (DataGridViewRow var in dgExpression.Rows)
            {
                if (var.IsNewRow) break;
                dtlCostSet = new BasicDtlCostSet();
                dtlCostSet.CostType = ClientUtil.ToString(var.Cells[colCostType.Name].Value);
                dtlCostSet.ApproachExpression = ClientUtil.ToString(var.Cells[colApproachExpression.Name].Value);
                dtlCostSet.ExitExpression = ClientUtil.ToString(var.Cells[colExitExpression.Name].Value);
                dtlCostSet.Descript = ClientUtil.ToString(var.Cells[colExpressionDescript.Name].Value);
                dtlCostSet.SetType = SetType.公式定义.ToString();
                Result.Add(dtlCostSet);
            }
            //维修设置
            foreach (DataGridViewRow var in dgRepair.Rows)
            {
                if (var.IsNewRow) break;
                dtlCostSet = new BasicDtlCostSet();
                dtlCostSet.WorkContent = ClientUtil.ToString(var.Cells[colWorkContent.Name].Value);
                dtlCostSet.Price = ClientUtil.ToDecimal(var.Cells[colRepairPrice.Name].Value);
                dtlCostSet.Descript = ClientUtil.ToString(var.Cells[colRepairDescript.Name].Value);
                dtlCostSet.SetType = SetType.维修设置.ToString();
                Result.Add(dtlCostSet);
            }
        }
    }

}
