using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Resource.MaterialResource.Domain;
using System.Collections;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    public partial class MaterialSelectorGrid : UserControl
    {
        virtual public event EventHandler EditingControlShowing;

        public MaterialSelectorGrid()
        {
            InitializeComponent();
            InitEvent();
            this.dgMaterialSelector.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InitEvent()
        {
            this.dgMaterialSelector.SetColumnsReadOnly(this.MaterialName);

            this.MaterialName.ReadOnly = true;
            this.dgMaterialSelector.SetColumnsReadOnly(this.MaterialSpec);
            this.MaterialSpec.ReadOnly = true;

            this.dgMaterialSelector.SetColumnsReadOnly(this.Stuff);
            this.Stuff.ReadOnly = true;

            //this.dgMaterialSelector.SetColumnsReadOnly(this.SupplyMoney);
            this.SupplyMoney.ReadOnly = true;

            this.dgMaterialSelector.CellDoubleClick += new DataGridViewCellEventHandler(dgMaterialSelector_CellDoubleClick);
            //     this.dgMaterialSelector.CellValidating += new DataGridViewCellValidatingEventHandler(dgMaterialSelector_CellValidating);
            this.dgMaterialSelector.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgMaterialSelector_EditingControlShowing);
            this.dgMaterialSelector.CellBeginEdit += new DataGridViewCellCancelEventHandler(dgMaterialSelector_CellBeginEdit);
            this.menuItemDelete.Click += new EventHandler(menuItemDelete_Click);

            this.dgMaterialSelector.CellParsing += new DataGridViewCellParsingEventHandler(dgMaterialSelector_CellParsing);
            this.dgMaterialSelector.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgMaterialSelector_RowHeaderMouseClick);
            this.dgMaterialSelector.CellEnter += new DataGridViewCellEventHandler(dgMaterialSelector_CellEnter);
        }

        void dgMaterialSelector_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            this.dgMaterialSelector.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        void dgMaterialSelector_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.dgMaterialSelector.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgMaterialSelector.EndEdit();
        }

        void dgMaterialSelector_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (this.dgMaterialSelector.Columns[e.ColumnIndex].Name.Equals("MaterialUnit"))
            {
                e.ParsingApplied = true;

                DataGridViewComboBoxCell cell = this.dgMaterialSelector.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;

                string tempValue = e.Value.ToString();
                //foreach (StandardUnit item in cell.Items)
                //{
                //    if (tempValue.Equals(item.Name))
                //    {
                //        e.Value = item;
                //    }
                //}
            }
        }

        void menuItemDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dgMaterialSelector.SelectedRows)
            {
                if (!row.IsNewRow)
                    this.dgMaterialSelector.Rows.Remove(row);
            }

            this.dgMaterialSelector.RefreshEdit();
        }

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaterialSelector_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (this.dgMaterialSelector.Columns[e.ColumnIndex].Name.Equals("MaterialCode"))
            {
                Material material = this.dgMaterialSelector.CurrentCell.Tag as Material;
                if (material == null)
                {
                    e.Cancel = true;
                }
            }
        }


        void dgMaterialSelector_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!this.dgMaterialSelector.Columns[e.ColumnIndex].Name.Equals("MaterialCode"))
            {
                Material material = this.dgMaterialSelector.Rows[e.RowIndex].Cells["MaterialCode"].Tag as Material;
                if (material == null)
                {
                    e.Cancel = true;
                    this.dgMaterialSelector.Rows[e.RowIndex].Cells["MaterialCode"].Selected = true;
                    //this.dgMaterialSelector.BeginEdit(false);
                }
            }

        }

        /// <summary>
        /// 物料编码列增加事件监听，已支持处理键盘回车查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaterialSelector_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.PreviewKeyDown -= new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                textBox.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);

                textBox.KeyUp -= new KeyEventHandler(textBox_KeyUp_SupplyQuantity);
                textBox.KeyUp -= new KeyEventHandler(textBox_KeyUp_SupplyPrice);

                if (this.dgMaterialSelector.Columns[this.dgMaterialSelector.CurrentCell.ColumnIndex].Name.Equals("MaterialCode"))
                {
                    textBox.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }
                else if (this.dgMaterialSelector.Columns[this.dgMaterialSelector.CurrentCell.ColumnIndex].Name.Equals("SupplyQuantity"))
                {
                    textBox.KeyUp += new KeyEventHandler(textBox_KeyUp_SupplyQuantity);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress_IsNumber);
                    textBox.KeyDown += new KeyEventHandler(textBox_KeyDown_IsNumber);
                }
                else if (this.dgMaterialSelector.Columns[this.dgMaterialSelector.CurrentCell.ColumnIndex].Name.Equals("SupplyPrice"))
                {
                    textBox.KeyUp += new KeyEventHandler(textBox_KeyUp_SupplyPrice);
                    textBox.KeyDown += new KeyEventHandler(textBox_KeyDown_IsNumber);
                }
                if (EditingControlShowing != null)
                    EditingControlShowing(sender, e);
            }
        }


        private bool nonNumberEntered = false;
        void textBox_KeyPress_IsNumber(object sender, KeyPressEventArgs e)
        {
            TextBox temp = sender as TextBox;

            if (nonNumberEntered)
            {
                e.Handled = true;
            }

            if (e.KeyChar.Equals('.'))
            {
                if (temp.Text.Contains(e.KeyChar.ToString()))
                {
                    e.Handled = true;

                }
            }

            if (e.KeyChar.Equals('-'))
            {
                if (this.Text.Contains(e.KeyChar.ToString()))
                {
                    e.Handled = true;
                }

            }
        }


        void textBox_KeyDown_IsNumber(object sender, KeyEventArgs e)
        {
            DataGridViewTextBoxEditingControl txt = sender as DataGridViewTextBoxEditingControl;
            CustomDataGridView aa = txt.EditingControlDataGridView as CustomDataGridView;

            if (aa.Columns[aa.CurrentCell.ColumnIndex].Name != "SupplyQuantity" && aa.Columns[aa.CurrentCell.ColumnIndex].Name != "SupplyPrice")
            {
                return;
            }
            //存放临时值
            TextBox temp = sender as TextBox;

            //做是否驶入数字处理
            nonNumberEntered = false;
            if (e.KeyValue == 190)
                nonNumberEntered = false;
            else
                if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9))
                {
                    if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                    {
                        if (e.KeyCode != Keys.Back && e.KeyCode != Keys.OemPeriod && e.KeyValue != 189)
                        {
                            nonNumberEntered = true;
                        }
                    }
                }
        }

        /// <summary>
        ///  更改单价，计算金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyUp_SupplyPrice(object sender, KeyEventArgs e)
        {
            TextBox temp = sender as TextBox;
            string currentValue = temp.Text;

            //if (!ClientUtil.isEmpty(currentValue))
            //{
            decimal price = ClientUtil.TransToDecimal(currentValue);
            decimal quantity = ClientUtil.TransToDecimal(this.dgMaterialSelector.CurrentRow.Cells["SupplyQuantity"].Value);

            //this.dgMaterialSelector.CurrentRow.Cells["SupplyMoney"].Value = decimal.Round(decimal.Multiply(quantity, price), 2);


            //}

        }
        /// <summary>
        /// 更改数量，计算金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyUp_SupplyQuantity(object sender, KeyEventArgs e)
        {
            ///张成儒 修改20090223日 控制结算单只允许修改金额；
            return ;
            TextBox temp = sender as TextBox;

            if (!ClientUtil.isEmpty(temp.Text))

                if (temp.Text.Contains("-"))
                {
                    int index = temp.Text.LastIndexOf('-');
                    if (index != 0)
                    {
                        string bb = temp.Text.Remove(index, 1);
                        temp.Text = bb;


                    }
                }
            string currentValue = temp.Text;
            if (!ClientUtil.isEmpty(currentValue))
            {
                if (!currentValue.Equals("-"))
                {
                    decimal quantity = ClientUtil.TransToDecimal(currentValue);
                    decimal price = ClientUtil.TransToDecimal(this.dgMaterialSelector.CurrentRow.Cells["SupplyPrice"].Value);

                    this.dgMaterialSelector.CurrentRow.Cells["SupplyMoney"].Value = decimal.Round(decimal.Multiply(quantity, price), 2);
                }
            }
        }




        /// <summary>
        /// 键盘回车查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CommonMaterial materialSelector = new CommonMaterial();

                TextBox textBox = sender as TextBox;
                if (textBox.Text != null && !textBox.Text.Equals(""))
                {
                    materialSelector.OpenSelect(textBox.Text);
                }
                else
                {
                    materialSelector.OpenSelect();
                }
                IList list = materialSelector.Result;

                if (list != null && list.Count > 0)
                {

                    Material selectedMaterial = list[0] as Material;
                    IList unitList = selectedMaterial.GetMaterialAllUnit();
                    this.dgMaterialSelector.CurrentCell.Tag = selectedMaterial;
                    this.dgMaterialSelector.CurrentCell.Value = selectedMaterial.Id;
                    this.dgMaterialSelector.CurrentRow.Cells["MaterialName"].Value = selectedMaterial.Name;
                    this.dgMaterialSelector.CurrentRow.Cells["MaterialSpec"].Value = selectedMaterial.Specification;
                    this.dgMaterialSelector.CurrentRow.Cells["Stuff"].Value = selectedMaterial.Stuff;

                    InitUnit(unitList);

                    this.dgMaterialSelector.RefreshEdit();
                }
            }
        }



        private void InitUnit(IList list)
        {
            DataGridViewComboBoxCell cbo = this.dgMaterialSelector.CurrentRow.Cells["MaterialUnit"] as DataGridViewComboBoxCell;
            cbo.Items.Clear();
            if (list != null && list.Count > 0)
            {
                foreach (StandardUnit unit in list)
                {

                    cbo.Items.Add(unit);
                }
            }

            cbo.Value = list[0];

        }

        /// <summary>
        /// 在物料编码列，敲击键盘时，取消原来已经选择的物料，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgMaterialSelector.CurrentCell.Tag = null;
            this.dgMaterialSelector.Rows[this.dgMaterialSelector.CurrentCell.RowIndex].Cells["MaterialName"].Value = "";
            this.dgMaterialSelector.Rows[this.dgMaterialSelector.CurrentCell.RowIndex].Cells["MaterialSpec"].Value = "";
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dgMaterialSelector_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;

            if (this.dgMaterialSelector.Columns[e.ColumnIndex].Name.Equals("MaterialCode"))
            {
                CommonMaterial materialSelector = new CommonMaterial();
                DataGridViewCell cell = this.dgMaterialSelector[e.ColumnIndex, e.RowIndex];

                object tempValue = cell.EditedFormattedValue;
                if (tempValue != null && !tempValue.Equals(""))
                {
                    materialSelector.OpenSelect(tempValue.ToString());
                }
                else
                {
                    materialSelector.OpenSelect();
                }

                IList list = materialSelector.Result;
                Material selectedMaterial = null;
                if (list != null && list.Count > 0)
                {
                    selectedMaterial = list[0] as Material;

                    IList unitList = selectedMaterial.GetMaterialAllUnit();

                    cell.Tag = selectedMaterial;
                    cell.Value = selectedMaterial.Code;
                    this.dgMaterialSelector.CurrentRow.Cells["MaterialName"].Value = selectedMaterial.Name;
                    this.dgMaterialSelector.CurrentRow.Cells["MaterialSpec"].Value = selectedMaterial.Specification;
                    this.dgMaterialSelector.CurrentRow.Cells["Stuff"].Value = selectedMaterial.Stuff;
                    InitUnit(unitList);

                    if (list.Count > 1)
                    {
                        for (int i = 1; i < list.Count; i++)
                        {

                            selectedMaterial = list[i] as Material;
                            int newRowIndex = this.dgMaterialSelector.Rows.Add();
                            DataGridViewRow newRow = this.dgMaterialSelector.Rows[newRowIndex];

                            newRow.Cells["MaterialCode"].Value = selectedMaterial.Id;
                            newRow.Cells["MaterialName"].Value = selectedMaterial.Name;
                            newRow.Cells["MaterialSpec"].Value = selectedMaterial.Specification;
                            newRow.Cells["Stuff"].Value = selectedMaterial.Stuff;

                            unitList = selectedMaterial.GetMaterialAllUnit();

                            InitUnit(unitList);
                        }
                    }

                    this.dgMaterialSelector.RefreshEdit();

                }

            }
        }


        public void AddColumn(DataGridViewColumn column)
        {
            this.dgMaterialSelector.Columns.Add(column);
        }



    }
}
