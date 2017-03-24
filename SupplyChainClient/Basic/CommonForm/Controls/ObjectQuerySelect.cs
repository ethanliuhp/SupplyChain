using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Spring.Context;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.UtilityControlService;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using System.Reflection;

namespace UtilityControl.Controls
{
    public partial class ObjectQuerySelect : UserControl
    {
        private static IUtilityControlSrv srv = null;

        public ObjectQuerySelect()
        {
            InitializeComponent();

            this.txtQueryValue.Focus();

            //try
            //{
            //    srv = StaticMethod.GetService("UtilityControlSrv") as IUtilityControlSrv;
            //}
            //catch
            //{
            //    MessageBox.Show("初始化控件基础服务失败！");
            //}
        }

        private void txtQueryValue_TextChanged(object sender, EventArgs e)
        {
            if (srv == null)
            {
                try
                {
                    srv = StaticMethod.GetService("UtilityControlSrv") as IUtilityControlSrv;
                }
                catch
                {
                }

                if (srv == null)
                {
                    MessageBox.Show("初始化控件基础服务失败！");
                    return;
                }
            }

            if (TargetClass == null && string.IsNullOrEmpty(TargetClassName))
            {
                MessageBox.Show("未设置控件查询的实体类！", "控件设置错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQueryValue.Focus();
                return;
            }
            else if (TargetClass == null && !string.IsNullOrEmpty(TargetClassName))
            {
                TargetClass = FindTypeInCurrentDomain(TargetClassName);

                if (TargetClass == null)
                {
                    MessageBox.Show("控件设置查询的实体类无效！", "控件设置错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQueryValue.Focus();
                    return;
                }
            }

            if (string.IsNullOrEmpty(QueryAttributes))
            {
                MessageBox.Show("未设置控件查询的属性字段！", "控件设置错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQueryValue.Focus();
                return;
            }

            if (srv != null)
            {
                grid.Columns.Clear();
                grid.Rows.Clear();

                grid.Height = 0;
                this.Height = txtQueryValue.Height;

                if (txtQueryValue.Text.Trim() == "")
                {
                    this.SelectObject = null;
                    return;
                }

                char[] ch = { '|' };

                ObjectQuery oq = new ObjectQuery();

                string[] queryAttributes = QueryAttributes.Split(ch);

                Disjunction dis = new Disjunction();
                foreach (string s in queryAttributes)
                {
                    if (MatchAttributeMode == QueryMatchMode.起始位置匹配)
                        dis.Add(Expression.Like(s, txtQueryValue.Text.Trim(), MatchMode.Start));
                    else if (MatchAttributeMode == QueryMatchMode.结束位置匹配)
                        dis.Add(Expression.Like(s, txtQueryValue.Text.Trim(), MatchMode.End));
                    else if (MatchAttributeMode == QueryMatchMode.任意位置匹配)
                        dis.Add(Expression.Like(s, txtQueryValue.Text.Trim(), MatchMode.Anywhere));
                    else if (MatchAttributeMode == QueryMatchMode.精确匹配)
                        dis.Add(Expression.Like(s, txtQueryValue.Text.Trim(), MatchMode.Exact));
                }
                oq.AddCriterion(dis);
                oq.FirstResult = 0;
                oq.MaxResults = MaxRecordCount;

                IList list = srv.ObjectQuery(TargetClass, oq);

                if (list.Count > 0)
                {
                    if (string.IsNullOrEmpty(DisplayAttributes))
                        DisplayAttributes = QueryAttributes;

                    if (string.IsNullOrEmpty(DisplayHeaderText))
                        DisplayHeaderText = DisplayAttributes;

                    string[] headerTexts = DisplayHeaderText.Split(ch);
                    foreach (string headerText in headerTexts)
                    {
                        DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                        column.HeaderText = headerText;
                        grid.Columns.Add(column);
                    }

                    int dataGridViewHeight = 0;

                    string[] displayAttributes = DisplayAttributes.Split(ch);
                    foreach (object obj in list)
                    {
                        int index = grid.Rows.Add();
                        DataGridViewRow row = grid.Rows[index];

                        for (int i = 0; i < displayAttributes.Length; i++)
                        {
                            string displayAttribute = displayAttributes[i];
                            row.Cells[i].Value = obj.GetType().GetProperty(displayAttribute).GetValue(obj, null);
                            row.Tag = obj;
                        }

                        dataGridViewHeight += row.Height;
                    }

                    grid.ColumnHeadersVisible = IsDisplayHeader;
                    if (IsDisplayHeader)
                        dataGridViewHeight += grid.ColumnHeadersHeight;

                    if (dataGridViewHeight > DataViewMaxHeight)
                    {
                        dataGridViewHeight = DataViewMaxHeight;

                    }

                    grid.Height = dataGridViewHeight;
                    this.Height = txtQueryValue.Height + grid.Height;

                    //显示再最前
                    this.BringToFront();
                    grid.BringToFront();
                }
                else
                {
                    grid.Height = 0;
                    this.Height = txtQueryValue.Height;
                }
            }
        }

        private void txtQueryValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                if (grid.Rows.Count > 0)
                {
                    grid.Focus();
                    grid.CurrentCell = grid.Rows[0].Cells[0];
                }
            }
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
            {
                if (grid.CurrentCell != null && grid.CurrentCell.RowIndex == 0)
                {
                    txtQueryValue.Focus();
                }
            }
            else if (e.KeyData == Keys.Enter)
            {
                if (grid.CurrentCell != null)
                {
                    object obj = grid.Rows[grid.CurrentCell.RowIndex].Tag;

                    string displayValue = "";

                    char[] ch = { '|' };

                    string[] disAttributes = null;
                    if (!string.IsNullOrEmpty(PostSelectionDisplayAttributes))
                        disAttributes = PostSelectionDisplayAttributes.Split(ch);
                    else
                        disAttributes = DisplayAttributes.Split(ch);

                    try
                    {
                        foreach (string attName in disAttributes)
                        {
                            displayValue += obj.GetType().GetProperty(attName).GetValue(obj, null).ToString() + PostSelectionDisplayAttributesSplit;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("对象中不存在设置显示的属性！", "设置显示属性错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (displayValue.Length > 0)
                        displayValue = displayValue.Substring(0, displayValue.Length - 1);

                    txtQueryValue.Text = displayValue;
                    SelectObject = obj;

                    grid.Height = 0;
                    this.Height = txtQueryValue.Height;
                    txtQueryValue.Focus();
                }
            }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                object obj = grid.Rows[e.RowIndex].Tag;

                string displayValue = "";

                char[] ch = { '|' };

                string[] disAttributes = null;
                if (!string.IsNullOrEmpty(PostSelectionDisplayAttributes))
                    disAttributes = PostSelectionDisplayAttributes.Split(ch);
                else
                    disAttributes = DisplayAttributes.Split(ch);

                try
                {
                    foreach (string attName in disAttributes)
                    {
                        displayValue += obj.GetType().GetProperty(attName).GetValue(obj, null).ToString() + PostSelectionDisplayAttributesSplit;
                    }
                }
                catch
                {
                    MessageBox.Show("对象中不存在设置显示的属性！", "设置显示属性错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (displayValue.Length > 0)
                    displayValue = displayValue.Substring(0, displayValue.Length - 1);

                txtQueryValue.Text = displayValue;
                SelectObject = obj;

                grid.Height = 0;
                this.Height = txtQueryValue.Height;
                txtQueryValue.Focus();
            }
        }

        private Type FindTypeInCurrentDomain(string typeName)
        {
            Type type = null;

            try
            {
                //如果该类型已经装载
                type = Type.GetType(typeName);
                if (type != null)
                {
                    return type;
                }

                //在EntryAssembly中查找
                if (Assembly.GetEntryAssembly() != null)
                {
                    type = Assembly.GetEntryAssembly().GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }

                //在CurrentDomain的所有Assembly中查找
                Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
                int assemblyArrayLength = assemblyArray.Length;
                for (int i = 0; i < assemblyArrayLength; ++i)
                {
                    type = assemblyArray[i].GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }

                for (int i = 0; (i < assemblyArrayLength); ++i)
                {
                    Type[] typeArray = assemblyArray[i].GetTypes();
                    int typeArrayLength = typeArray.Length;
                    for (int j = 0; j < typeArrayLength; ++j)
                    {
                        if (typeArray[j].Name.Equals(typeName))
                        {
                            return typeArray[j];
                        }
                    }
                }
            }
            catch { }

            return type;
        }

        #region 公共属性
        /// <summary>
        /// 要查询的实体类
        /// </summary>
        [Description("要查询的实体类"), Category("查询设置"), Browsable(false)]
        public Type TargetClass
        {
            get;
            set;
        }

        /// <summary>
        /// 要查询的实体类名称
        /// </summary>
        [Description("要查询的实体类全名"), Category("查询设置"), Browsable(true)]
        public string TargetClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 查询的属性(多个属性用|分隔，如：Name|Code)
        /// </summary>
        [Description("查询的属性(多个属性用|分隔，如：Name|Code)"), Category("查询设置"), Browsable(true)]
        public string QueryAttributes
        {
            get;
            set;
        }

        private int _maxRecordCount = 10;
        /// <summary>
        /// 允许返回的最大记录数
        /// </summary>
        [Description("允许返回的最大记录数"), Category("查询设置"), Browsable(true)]
        public int MaxRecordCount
        {
            get { return _maxRecordCount; }
            set { _maxRecordCount = value; }
        }

        private QueryMatchMode _matchAttributeMode = QueryMatchMode.起始位置匹配;
        /// <summary>
        /// 查询关键字和属性值的匹配模式
        /// </summary>
        [Description("查询关键字和属性值的匹配模式"), Category("查询设置"), Browsable(true)]
        public QueryMatchMode MatchAttributeMode
        {
            get { return _matchAttributeMode; }
            set { _matchAttributeMode = value; }
        }

        /// <summary>
        /// 列表中显示的属性(和DisplayHeaderText要一一对应，多个属性用|分隔，如：Name|Code)
        /// </summary>
        [Description("列表中显示的属性(和DisplayHeaderText要一一对应，多个属性用|分隔，如：Name|Code)"), Category("查询设置"), Browsable(true)]
        public string DisplayAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// 列表中显示的列头文本(和DisplayAttributes要一一对应，多个属性用|分隔，如：姓名|编号)
        /// </summary>
        [Description("列表中显示的列头文本(和DisplayAttributes要一一对应，多个属性用|分隔，如：姓名|编号)"), Category("查询设置"), Browsable(true)]
        public string DisplayHeaderText
        {
            get;
            set;
        }

        private bool _isDisplayHeader = true;
        /// <summary>
        /// 列表中是否显示列头
        /// </summary>
        [Description("列表中是否显示列头"), Category("查询设置"), Browsable(true)]
        public bool IsDisplayHeader
        {
            get { return _isDisplayHeader; }
            set { _isDisplayHeader = value; }
        }

        private int _dataViewHeight = 200;
        /// <summary>
        /// 数据列表的可视高度
        /// </summary>
        [Description("数据列表的可视高度"), Category("查询设置"), Browsable(true)]
        public int DataViewMaxHeight
        {
            get { return _dataViewHeight; }
            set { _dataViewHeight = value; }
        }

        /// <summary>
        /// 选择后显示的属性(多个属性用|分隔，如：Name|Code)
        /// </summary>
        [Description("选择后显示的属性(多个属性用|分隔，如：Name|Code)"), Category("查询设置"), Browsable(true)]
        public string PostSelectionDisplayAttributes
        {
            get;
            set;
        }

        private string _postSelectionDisplayAttributesSplit = "-";
        /// <summary>
        /// 选择后显示的属性之间的分隔符,缺省为'-'
        /// </summary>
        [Description("选择后显示的属性之间的分隔符,缺省为'-'"), Category("查询设置"), Browsable(true)]
        public string PostSelectionDisplayAttributesSplit
        {
            get { return _postSelectionDisplayAttributesSplit; }
            set { _postSelectionDisplayAttributesSplit = value; }
        }

        /// <summary>
        /// 选择的对象
        /// </summary>
        [Description("选择的对象"), Category("查询设置"), Browsable(false)]
        public object SelectObject
        {
            get;
            set;
        }
        #endregion
    }

    /// <summary>
    /// 查询匹配模式
    /// </summary>
    public enum QueryMatchMode
    {
        起始位置匹配 = 1,
        任意位置匹配 = 2,
        结束位置匹配 = 3,
        精确匹配 = 4
    }
}
