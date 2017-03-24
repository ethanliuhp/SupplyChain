using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using System.Reflection;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI
{
    public enum EnumDataType
    {
        DateTime,
        Number,
        String,
        Bool
    }

    public partial class VAppPropertySet : TBasicDataView
    {
        private MAppPlatform Model = new MAppPlatform();
        private AppTableSet curTableSet = null;
        private IList List = new ArrayList();
        private IList List_MasterProperty = new ArrayList();
        private IList List_DetailProperty = new ArrayList();

        public VAppPropertySet()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            Search();
            colMasterDataType.DataSource = Enum.GetNames(typeof(EnumDataType));
            colDetailDataType.DataSource = Enum.GetNames(typeof(EnumDataType));
        }

        private void InitEvent()
        {
            btnSaveMaster.Click += new EventHandler(btnSaveMaster_Click);
            btnSaveDetail.Click += new EventHandler(btnSaveDetail_Click);
            dgMaster.RowsAdded += new DataGridViewRowsAddedEventHandler(dgMaster_RowsAdded);
            dgDetail.RowsAdded += new DataGridViewRowsAddedEventHandler(dgDetail_RowsAdded);
            dgBill.SelectionChanged += new EventHandler(dgBill_SelectionChanged);
            dgMaster.CellEndEdit += new DataGridViewCellEventHandler(dgMaster_CellEndEdit);
            dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
        }

        void dgDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = dgBill.SelectedRows[0] as DataGridViewRow;
            if (row != null)
            {
                DataGridViewRow theCurrentRow = dgDetail.CurrentRow;
                if (theCurrentRow != null)
                {
                    theCurrentRow.Cells[coDtlClassName.Name].Value = ClientUtil.ToString(row.Cells[colDetailClassName.Name].Value);
                    theCurrentRow.Cells[colDetailPropertyVisible.Name].Value = true;
                }
            }
            else
            {
                MessageBox.Show("请选择一条单据定义信息！");
            }
        }

        void dgMaster_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = dgBill.SelectedRows[0] as DataGridViewRow;
            if (row != null)
            {
                DataGridViewRow theCurrentRow = dgMaster.CurrentRow;
                if (theCurrentRow != null)
                {
                    theCurrentRow.Cells[colMasterClassName.Name].Value = ClientUtil.ToString(row.Cells[colClassName.Name].Value);
                    theCurrentRow.Cells[colMasterPropertyVisible.Name].Value = true;
                }
            }
            else
            {
                MessageBox.Show("请选择一条单据定义信息！");
            }
        }

        /// <summary>
        /// 保存明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveDetail_Click(object sender, EventArgs e)
        {
            if (!ValidDetailProperty()) return;
            IList list = new ArrayList();
            DataGridViewRow theCurrent = dgBill.SelectedRows[0];

            if (theCurrent != null)
            {
                AppTableSet theAppTableSet = theCurrent.Tag as AppTableSet;
                if (theAppTableSet != null)
                {
                    foreach (DataGridViewRow var in dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;

                        AppDetailPropertySet master = var.Tag as AppDetailPropertySet;
                        if (master == null)
                        {
                            master = new AppDetailPropertySet();
                        }
                        master.ParentId = theAppTableSet;
                        master.DetailClassName = ClientUtil.ToString(var.Cells[coDtlClassName.Name].Value);
                        master.DetailPropertyName = ClientUtil.ToString(var.Cells[colDetailPropertyName.Name].Value);
                        master.DetailPropertyChineseName = ClientUtil.ToString(var.Cells[colDetailPropertyChineseName.Name].Value);
                        master.DBFieldName = ClientUtil.ToString(var.Cells[colDetailFieldName.Name].Value);
                        master.DataType = ClientUtil.ToString(var.Cells[colDetailDataType.Name].Value);
                        bool IsChecked = true;
                        IsChecked = Convert.ToBoolean(var.Cells[colDetailPropertyVisible.Name].EditedFormattedValue);
                        if (IsChecked == true)
                        {
                            master.DetailPropertyVisible = true;
                        }
                        else
                        {
                            master.DetailPropertyVisible = false;
                        }
                        IsChecked = Convert.ToBoolean(var.Cells[colDetailPropertyReadOnly.Name].EditedFormattedValue);
                        if (IsChecked == true)
                        {
                            master.DetailPropertyReadOnly = true;
                        }
                        else
                        {
                            master.DetailPropertyReadOnly = false;
                        }
                        master.SerialNumber = Convert.ToInt32(var.Cells[colDetailSerialNum.Name].Value);
                        list.Add(master);
                    }
                }
                List_DetailProperty = Model.Save(list);
                ReloadDetailProperty(List_DetailProperty);
                MessageBox.Show("保存成功！");
            }
            else
            {
                MessageBox.Show("请选择一条审批单据定义信息！");
                return;
            }

        }

        private void ReloadDetailProperty(IList list)
        {
            if (list.Count > 0)
            {
                dgDetail.Rows.Clear();
                foreach (AppDetailPropertySet master in list)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail[coDtlClassName.Name, rowIndex].Value = master.DetailClassName;
                    dgDetail[colDetailPropertyName.Name, rowIndex].Value = master.DetailPropertyName;
                    dgDetail[colDetailPropertyChineseName.Name, rowIndex].Value = master.DetailPropertyChineseName;
                    dgDetail[colDetailFieldName.Name, rowIndex].Value = master.DBFieldName;
                    dgDetail[colDetailDataType.Name, rowIndex].Value = master.DataType;
                    bool propertyVisible = true;
                    propertyVisible = master.DetailPropertyVisible;
                    if (propertyVisible == true)
                    {
                        dgDetail[colDetailPropertyVisible.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgDetail[colDetailPropertyVisible.Name, rowIndex].Value = false;
                    }
                    bool propertyReadOnly = true;
                    propertyReadOnly = master.DetailPropertyReadOnly;
                    if (propertyReadOnly == true)
                    {
                        dgDetail[colDetailPropertyReadOnly.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgDetail[colDetailPropertyReadOnly.Name, rowIndex].Value = false;
                    }
                    dgDetail[colDetailSerialNum.Name, rowIndex].Value = master.SerialNumber;
                    dgDetail.Rows[rowIndex].Tag = master;
                }
            }
        }

        private void ReloadMasterProperty(IList list)
        {
            if (list.Count > 0)
            {
                dgMaster.Rows.Clear();
                foreach (AppMasterPropertySet master in list)
                {
                    int rowIndex = dgMaster.Rows.Add();
                    dgMaster[colMasterClassName.Name, rowIndex].Value = master.MasterClassName;
                    dgMaster[colMasterPropertyName.Name, rowIndex].Value = master.MasterPropertyName;
                    dgMaster[colMasterPropertyChineseName.Name, rowIndex].Value = master.MasterPpropertyChineseName;
                    dgMaster[colMasterFieldName.Name, rowIndex].Value = master.DBFieldName;
                    dgMaster[colMasterDataType.Name, rowIndex].Value = master.DataType;
                    bool propertyVisible = true;
                    propertyVisible = master.MasterPropertyVisible;
                    if (propertyVisible == true)
                    {
                        dgMaster[colMasterPropertyVisible.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgMaster[colMasterPropertyVisible.Name, rowIndex].Value = false;
                    }
                    bool propertyReadOnly = true;
                    propertyReadOnly = master.MasterPropertyReadOnly;
                    if (propertyReadOnly == true)
                    {
                        dgMaster[colMasterPropertyReadOnly.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgMaster[colMasterPropertyReadOnly.Name, rowIndex].Value = false;
                    }
                    dgMaster[colMasterSerialNum.Name, rowIndex].Value = master.SerialNumber;
                    dgMaster.Rows[rowIndex].Tag = master;
                }
            }
        }

        /// <summary>
        /// 保存主表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSaveMaster_Click(object sender, EventArgs e)
        {
            if (!ValidMasterProperty()) return;
            IList list = new ArrayList();
            DataGridViewRow theCurrent = dgBill.SelectedRows[0];
            if (theCurrent != null)
            {
                AppTableSet theAppTableSet = theCurrent.Tag as AppTableSet;
                if (theAppTableSet != null)
                {
                    foreach (DataGridViewRow var in dgMaster.Rows)
                    {
                        if (var.IsNewRow) break;

                        AppMasterPropertySet master = var.Tag as AppMasterPropertySet;
                        if (master == null)
                        {
                            master = new AppMasterPropertySet();
                        }
                        master.ParentId = theAppTableSet;
                        master.MasterClassName = ClientUtil.ToString(var.Cells[colMasterClassName.Name].Value);
                        master.MasterPropertyName = ClientUtil.ToString(var.Cells[colMasterPropertyName.Name].Value);
                        master.MasterPpropertyChineseName = ClientUtil.ToString(var.Cells[colMasterPropertyChineseName.Name].Value);
                        master.DBFieldName = ClientUtil.ToString(var.Cells[colMasterFieldName.Name].Value);
                        master.DataType = ClientUtil.ToString(var.Cells[colMasterDataType.Name].Value);
                        bool IsChecked = true;
                        IsChecked = Convert.ToBoolean(var.Cells[colMasterPropertyVisible.Name].EditedFormattedValue);
                        if (IsChecked == true)
                        {
                            master.MasterPropertyVisible = true;
                        }
                        else
                        {
                            master.MasterPropertyVisible = false;
                        }
                        IsChecked = Convert.ToBoolean(var.Cells[colMasterPropertyReadOnly.Name].EditedFormattedValue);
                        if (IsChecked == true)
                        {
                            master.MasterPropertyReadOnly = true;
                        }
                        else
                        {
                            master.MasterPropertyReadOnly = false;
                        }
                        master.SerialNumber = Convert.ToInt32(var.Cells[colMasterSerialNum.Name].Value);
                        list.Add(master);
                    }
                }
                List_MasterProperty = Model.Save(list);
                ReloadMasterProperty(List_MasterProperty);
                MessageBox.Show("保存成功！");
            }
            else
            {
                MessageBox.Show("请选择一条审批单据定义信息！");
                return;
            }
        }

        void dgBill_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow SelectRow = dgBill.SelectedRows[0] as DataGridViewRow;
            colMasterPropertyName.Items.Clear();
            colDetailPropertyName.Items.Clear();
            if (SelectRow != null)
            {
                AppTableSet theAppTableSet = SelectRow.Tag as AppTableSet;
                #region 加载当前主表及明细的属性
                if (theAppTableSet != null)
                {
                    IList masterProperties = Model.GetMasterProperties(theAppTableSet.MasterNameSpace);
                    IList detailProperties = new ArrayList();
                    if (theAppTableSet.DetailNameSpace != null)
                    {
                        detailProperties = Model.GetDetailProperties(theAppTableSet.DetailNameSpace);
                    }
                    foreach (PropertyInfo thePropertyInfo in masterProperties)
                    {
                        string propertyName = thePropertyInfo.Name;
                        //if (thePropertyInfo.DeclaringType.Name.ToString() == "BaseMaster")
                        //{
                        //    if (propertyName == "CreatePersonName" || propertyName == "HandlePersonName" || propertyName == "OperOrgInfoName" || propertyName == "ProjectName" || propertyName == "SumMoney" || propertyName == "SumQuantity" || propertyName == "Code" || propertyName == "RealOperationDate" || propertyName == "Descript" || propertyName == "CreateDate" || propertyName == "Id" || propertyName == "DocState")
                        //    {
                        //        colMasterPropertyName.Items.Add(propertyName);
                        //    }
                        //}
                        //else
                        //{
                        //    colMasterPropertyName.Items.Add(thePropertyInfo.Name);
                        //}
                        colMasterPropertyName.Items.Add(propertyName);
                    }
                    foreach (PropertyInfo thePropertyInfo in detailProperties)
                    {
                        string propertyName = thePropertyInfo.Name;
                        //if (thePropertyInfo.DeclaringType.Name.ToString() == "BaseDetail")
                        //{
                        //    if (propertyName == "MatStandardUnitName" || propertyName == "MaterialSpec" || propertyName == "UsedPartName" || propertyName == "MaterialName" || propertyName == "MaterialCode" || propertyName == "MaterialResource" || propertyName == "Quantity" || propertyName == "Price" || propertyName == "Money" || propertyName == "Descript" || propertyName == "Id")
                        //    {
                        //        colDetailPropertyName.Items.Add(propertyName);
                        //    }
                        //}
                        //else
                        //{
                        //    colDetailPropertyName.Items.Add(propertyName);
                        //}
                        colDetailPropertyName.Items.Add(propertyName);
                    }
                }
                #endregion

                #region 加载已定义的主表及明细的审批字段
                if (theAppTableSet != null)
                {
                    IList list_AppMaster = new ArrayList();
                    IList list_AppDetail = new ArrayList();
                    dgMaster.Rows.Clear();
                    dgDetail.Rows.Clear();

                    list_AppMaster = Model.GetAppMasterProperties(theAppTableSet.Id);
                    if (list_AppMaster.Count > 0)
                    {
                        foreach (AppMasterPropertySet master in list_AppMaster)
                        {
                            int rowIndex = dgMaster.Rows.Add();
                            dgMaster[colMasterClassName.Name, rowIndex].Value = master.MasterClassName;
                            dgMaster[colMasterPropertyName.Name, rowIndex].Value = master.MasterPropertyName;
                            dgMaster[colMasterPropertyChineseName.Name, rowIndex].Value = master.MasterPpropertyChineseName;
                            dgMaster[colMasterFieldName.Name, rowIndex].Value = master.DBFieldName;
                            dgMaster[colMasterDataType.Name, rowIndex].Value = master.DataType;
                            bool propertyVisible = true;
                            propertyVisible = master.MasterPropertyVisible;
                            if (propertyVisible == true)
                            {
                                dgMaster[colMasterPropertyVisible.Name, rowIndex].Value = true;
                            }
                            else
                            {
                                dgMaster[colMasterPropertyVisible.Name, rowIndex].Value = false;
                            }
                            bool propertyReadOnly = true;
                            propertyReadOnly = master.MasterPropertyReadOnly;
                            if (propertyReadOnly == true)
                            {
                                dgMaster[colMasterPropertyReadOnly.Name, rowIndex].Value = true;
                            }
                            else
                            {
                                dgMaster[colMasterPropertyReadOnly.Name, rowIndex].Value = false;
                            }
                            dgMaster[colMasterSerialNum.Name, rowIndex].Value = master.SerialNumber;
                            dgMaster.Rows[rowIndex].Tag = master;
                        }

                    }
                    list_AppDetail = Model.GetAppDetailProperties(theAppTableSet.Id);

                    if (list_AppDetail.Count > 0)
                    {
                        foreach (AppDetailPropertySet master in list_AppDetail)
                        {
                            int rowIndex = dgDetail.Rows.Add();
                            dgDetail[coDtlClassName.Name, rowIndex].Value = master.DetailClassName;
                            dgDetail[colDetailPropertyName.Name, rowIndex].Value = master.DetailPropertyName;
                            dgDetail[colDetailPropertyChineseName.Name, rowIndex].Value = master.DetailPropertyChineseName;
                            dgDetail[colDetailFieldName.Name, rowIndex].Value = master.DBFieldName;
                            dgDetail[colDetailDataType.Name, rowIndex].Value = master.DataType;
                            bool propertyVisible = true;
                            propertyVisible = master.DetailPropertyVisible;
                            if (propertyVisible == true)
                            {
                                dgDetail[colDetailPropertyVisible.Name, rowIndex].Value = true;
                            }
                            else
                            {
                                dgDetail[colDetailPropertyVisible.Name, rowIndex].Value = false;
                            }
                            bool propertyReadOnly = true;
                            propertyReadOnly = master.DetailPropertyReadOnly;
                            if (propertyReadOnly == true)
                            {
                                dgDetail[colDetailPropertyReadOnly.Name, rowIndex].Value = true;
                            }
                            else
                            {
                                dgDetail[colDetailPropertyReadOnly.Name, rowIndex].Value = false;
                            }
                            dgDetail[colDetailSerialNum.Name, rowIndex].Value = master.SerialNumber;
                            dgDetail.Rows[rowIndex].Tag = master;
                        }
                    }
                }
                #endregion
            }
        }

        private void Search()
        {
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();

            //审批有效表单
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Status", 0));
            //oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            List = Model.GetObjects(typeof(AppTableSet), oq);
            foreach (AppTableSet item in List)
            {
                int rowIndex = dgBill.Rows.Add();
                dgBill[collBillName.Name, rowIndex].Value = item.TableName;
                dgBill[colClassName.Name, rowIndex].Value = item.ClassName;
                dgBill[colPhysicsName.Name, rowIndex].Value = item.PhysicsName;
                dgBill[colDetailClassName.Name, rowIndex].Value = item.DetailClassName;
                dgBill[colDetailPhysicsName.Name, rowIndex].Value = item.DetailPhysicsName;
                dgBill[colExcuCode.Name, rowIndex].Value = item.ExecCode;
                dgBill[colStatusName.Name, rowIndex].Value = item.StatusName;
                dgBill[colStatusValue.Name, rowIndex].Value = item.StatusValueAgr;
                dgBill[colDescript.Name, rowIndex].Value = item.Remark;
                if (item.Status == 0)
                {
                    dgBill[colStatus.Name, rowIndex].Value = "启用";
                }
                else
                {
                    dgBill[colStatus.Name, rowIndex].Value = "停用";
                }
                dgBill.Rows[rowIndex].Tag = item;
            }

            if (dgBill.Rows.Count > 2)
            {
                dgBill.Rows[1].Selected = true;
                curTableSet = dgBill.Rows[1].Tag as AppTableSet;
            }
        }

        private bool ValidMasterProperty()
        {
            foreach (DataGridViewRow row in dgMaster.Rows)
            {
                //最后一行不进行校验;
                if (row.IsNewRow) break;
                if (row.Cells[colMasterPropertyName.Name].Value == null)
                {
                    MessageBox.Show("主表属性名称不能为空！");
                    return false;
                }
                if (row.Cells[colMasterPropertyChineseName.Name].Value == null)
                {
                    MessageBox.Show("主表属性中文名称不能为空");
                    return false;
                }
                if (row.Cells[colMasterFieldName.Name].Value == null)
                {
                    MessageBox.Show("主表数据库字段名不能为空");
                    return false;
                }
                if (row.Cells[colMasterDataType.Name].Value == null)
                {
                    MessageBox.Show("主表属性的数据类型不能为空！");
                    return false;
                }
            }

            return true;
        }

        private bool ValidDetailProperty()
        {
            foreach (DataGridViewRow row in dgDetail.Rows)
            {
                //最后一行不进行校验;
                if (row.IsNewRow) break;
                if (row.Cells[colDetailPropertyName.Name].Value == null)
                {
                    MessageBox.Show("明细属性名称不能为空！");
                    return false;
                }
                if (row.Cells[colDetailPropertyChineseName.Name].Value == null)
                {
                    MessageBox.Show("明细属性中文名称不能为空");
                    return false;
                }
                if (row.Cells[colDetailFieldName.Name].Value == null)
                {
                    MessageBox.Show("明细数据库字段名不能为空");
                    return false;
                }
                if (row.Cells[colDetailDataType.Name].Value == null)
                {
                    MessageBox.Show("明细属性的数据类型不能为空！");
                    return false;
                }
            }

            return true;
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            bool validity = true;

            if (colName == colDetailSerialNum.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colDetailSerialNum.Name].Value != null)
                {
                    string temp_value = dgDetail.Rows[e.RowIndex].Cells[colDetailSerialNum.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_value);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail.Rows[e.RowIndex].Cells[colDetailSerialNum.Name].Value = null;
                        dgDetail.CurrentCell = dgDetail.Rows[e.RowIndex].Cells[this.colDetailSerialNum.Name];
                    }
                }
            }
            if (colName == colDetailPropertyName.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colDetailPropertyName.Name].Value != null)
                {
                    object propertyname = dgDetail.Rows[e.RowIndex].Cells[colDetailPropertyName.Name].Value;
                    dgDetail.Rows[e.RowIndex].Cells[colDetailFieldName.Name].Value = propertyname.ToString();
                }
            }
        }

        void dgMaster_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgMaster.Columns[e.ColumnIndex].Name;
            bool validity = true;

            if (colName == colMasterSerialNum.Name)
            {
                if (dgMaster.Rows[e.RowIndex].Cells[colMasterSerialNum.Name].Value != null)
                {
                    string temp_value = dgMaster.Rows[e.RowIndex].Cells[colMasterSerialNum.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_value);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgMaster.Rows[e.RowIndex].Cells[colMasterSerialNum.Name].Value = null;
                        dgMaster.CurrentCell = dgMaster.Rows[e.RowIndex].Cells[colMasterSerialNum.Name];
                    }
                }
            }
            if (colName == colMasterPropertyName.Name)
            {
                if (dgMaster.Rows[e.RowIndex].Cells[colMasterPropertyName.Name].Value != null)
                {
                    object propertyname = dgMaster.Rows[e.RowIndex].Cells[colMasterPropertyName.Name].Value;
                    dgMaster.Rows[e.RowIndex].Cells[colMasterFieldName.Name].Value = propertyname.ToString();
                }
            }
        }

    }
}