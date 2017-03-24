using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using System.Text.RegularExpressions;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public partial class VTemplateType : TBasicDataView
    {
        /// <summary>
        /// 所有的结构类型
        /// </summary>
        public List<PBSTemplateType> TypeList { get; set; }
        /// <summary>
        /// 定义一个变量，提供数据库服务
        /// </summary>
        public IPBSTreeSrv service;
        /// <summary>
        /// 是否是新增
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 当前选择的结构类型
        /// </summary>
        public PBSTemplateType SelectPBS { get; set; }
        /// <summary>
        /// 当前选择的行
        /// </summary>
        public DataGridViewRow SelectRow { get; set; }

        public VTemplateType(MPBSTree model)
        {
            service = model.Service;
            TypeList = new List<PBSTemplateType>();
            InitializeComponent();
            InitData();
            InitEvent();
            ChangeControlState(false);
        }

        /// <summary>
        /// 页面数据初始化
        /// </summary>
        private void InitData()
        {
            var oq = new ObjectQuery();
            oq.AddOrder(Order.Asc("CreateTime"));
            var result = service.ObjectQuery(typeof(PBSTemplateType), oq);
            if (result == null || result.Count == 0) return;
            foreach (PBSTemplateType item in result)
                TypeList.Add(item);
            LoadList(TypeList);
        }

        /// <summary>
        /// 事件初始化
        /// </summary>
        private void InitEvent()
        {
            txtCode.KeyPress+=new KeyPressEventHandler(txtCode_KeyPress);
            txtKey.KeyDown += (a, b) =>
            {
                if (b.KeyCode != Keys.Enter) return;
                Search(txtKey.Text);
            };
            btnSearch.Click += (a, b) => Search(txtKey.Text);
            btnAdd.Click += (a, b) => { Add(); ChangeControlState(true); };
            btnDelete.Click += (a, b) => Delete();
            btnAllDelete.Click += (a, b) => DeleteAll();
            btnModify.Click += (a, b) => { IsNew = false; ChangeControlState(true); };
            btnSave.Click += (a, b) =>
            {
                if (ValidateData())
                {
                    Save();
                    ChangeControlState(false);
                }
            };
            gvTable.CellMouseClick += (a, b) =>
            {
                if (b.ColumnIndex == 0) return;
                ClickRow(b.RowIndex);
                ChangeControlState(false);
            };
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        private void LoadList(IEnumerable<PBSTemplateType> list)
        {
            gvTable.Rows.Clear();
            foreach (var item in list)
                AddRow(item);
        }
        public void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')//删除
            {
                e.Handled = false;
            }
            else
            {
                if (txtCode.Text.Length >= 4)//长度大于4
                {
                    e.Handled = true;
                }
                else
                {
                    if ((e.KeyChar >= 'a' && e.KeyChar <= 'z'))
                    {
                        e.KeyChar = e.KeyChar.ToString().ToUpper()[0];
                        e.Handled = false;
                    }
                    else if (e.KeyChar >= 'A' && e.KeyChar <= 'Z')
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }
        /// <summary>
        /// 表格增加一行
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        private DataGridViewRow AddRow(PBSTemplateType type)
        {
            var rowIndex = gvTable.Rows.Add();
            var row = gvTable.Rows[rowIndex];
            ReloadRow(row, type);
            row.Tag = type;
            return row;
        }

        /// <summary>
        /// 重新加载指定的行
        /// </summary>
        private void ReloadRow(DataGridViewRow row, PBSTemplateType type)
        {
            row.Cells[colCode.Name].Value = type.Code;
            row.Cells[colName.Name].Value = type.Name;
            row.Cells[colBit.Name].Value = type.CodeBit;
            row.Cells[colRemark.Name].Value = type.Remark;
            row.Tag = type;
        }

        /// <summary>
        /// 选择的行改变时
        /// </summary>
        private void ClickRow(int index)
        {
            if (index < 0) return;
            SelectRow = gvTable.Rows[index];
            SelectPBS = SelectRow.Tag as PBSTemplateType;
            txtCode.Text = SelectPBS.Code;
            txtName.Text = SelectPBS.Name;
            txtBit.Text = SelectPBS.CodeBit;
            txtRemark.Text = SelectPBS.Remark;
        }

        /// <summary>
        /// 根据关键字搜索指定的结构类型
        /// </summary>
        /// <param name="key"></param>
        private void Search(string key)
        {
            var result = TypeList.Where(a => a.Code.Contains(key) || a.Name.Contains(key) || a.CodeBit.Contains(key));
            //if (result.Count() == 0) return;
            LoadList(result);
        }

        /// <summary>
        /// 新增
        /// </summary>
        private void Add()
        {
            txtCode.Text = txtName.Text = txtBit.Text = txtRemark.Text = string.Empty;
            IsNew = true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete()
        {
            if (SelectRow == null) return;
            var result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            service.Delete(SelectPBS);
            TypeList.Remove(SelectPBS);
            gvTable.Rows.Remove(gvTable.CurrentRow);
        }

        /// <summary>
        /// 删除所有选中的行
        /// </summary>
        private void DeleteAll()
        {
            var result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            var selectedPBS = new List<PBSTemplateType>();
            var selectedRow = new List<DataGridViewRow>();
            foreach (DataGridViewRow item in gvTable.Rows)
            {
                if (Convert.ToBoolean(item.Cells[colSelect.Name].Value))
                {
                    selectedPBS.Add(item.Tag as PBSTemplateType);
                    selectedRow.Add(item);
                }
            }
            if (selectedPBS.Count == 0) return;
            foreach (var item in selectedPBS)
            {
                TypeList.Remove(item);
                service.Delete(item);
            }
            foreach (var item in selectedRow)
                gvTable.Rows.Remove(item);
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtBit.Text))
            {
                MessageBox.Show("请将信息输入完整...");
                return false;
            }
            else if (txtCode.Text.Trim().Length != 4)
            {
                MessageBox.Show("请输入四位有效编码");
                txtCode.Focus();
                return false;
            }
            else if (!Regex.Match(txtCode.Text.Trim(), "^[A-Z]{4}$").Success)
            {
                MessageBox.Show("请输入由四个字母组成的有效编码");
                txtCode.Focus();
                return false;
            }
            int num = 0;
            if (!int.TryParse(txtBit.Text, out num))
            {
                MessageBox.Show("编号位数必须输入数字...");
                return false;
            }
            if (num > 4)
            {
                MessageBox.Show("编号位数不能大于4...");
                return false;
            }
            if (TypeList.FirstOrDefault(a => a.Code == txtCode.Text) != null)// 编码不能重复
            {
                MessageBox.Show("已存在编码...");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            PBSTemplateType model;
            if (IsNew)
            {
                // 新增
                model = new PBSTemplateType()
                {
                    CreateTime = DateTime.Now,
                    CreatePerson = ConstObject.LoginPersonInfo,
                    CreatePersonName = ConstObject.LoginPersonInfo.Name,
                };
                TypeList.Add(model);
            }
            else
            {
                // 修改
                model = SelectPBS;
            }
            model.Code = txtCode.Text ;
            model.Name = txtName.Text;
            model.CodeBit = txtBit.Text;
            model.Remark = txtRemark.Text;
            model.ModifyTime = DateTime.Now;
            service.Save(model);
            if (IsNew)
            {
                var row = AddRow(model);
                row.Selected = true;
            }
            else
            {
                ReloadRow(gvTable.CurrentRow, model);
            }
        }

        /// <summary>
        /// 修改控件状态
        /// </summary>
        private void ChangeControlState(bool state)
        {
            var color = state ? SystemColors.Window : SystemColors.Control;
            txtCode.Enabled = txtName.Enabled = txtBit.Enabled = txtRemark.Enabled = btnSave.Enabled = state;
            txtCode.BackColor = txtName.BackColor = txtBit.BackColor = txtRemark.BackColor = color;
            if (state)
                txtCode.Select();
        }

    }
}
