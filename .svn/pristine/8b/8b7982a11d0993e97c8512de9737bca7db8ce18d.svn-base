using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using System.IO;
using System.Data.OleDb;
using Application.Business.Erp.SupplyChain.Client.ExcelImportMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public partial class VCostItemImport : TBasicDataView
    {
        public MCostItem model = new MCostItem();

        public VCostItemImport(MCostItem mot)
        {
            model = mot;

            InitializeComponent();
            InitEvents();
            InitForm();
        }


        private void InitEvents()
        {
            btnBrownFile.Click += new EventHandler(btnBrownFile_Click);
            btnImport.Click += new EventHandler(btnImport_Click);

            btnCostItemMng.Click += new EventHandler(btnCostItemMng_Click);

            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnAddCostItem1.Click += new EventHandler(btnAddCostItem1_Click);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);
            btnCheckNode.Click += new EventHandler(btnCheckNode_Click);
            btnAddCostitem.Click += new EventHandler(btnAddCostitem_Click);
            //btnGetQuotaHasQuantity.Click += new EventHandler(btnGetQuotaHasQuantity_Click);

            //btnUpdateCostItemPricing.Click += new EventHandler(btnUpdateCostItemPricing_Click);

            //btnUpdateCategory.Click += new EventHandler(btnUpdateCategory_Click);

            //btnUpdateCostItem.Click += new EventHandler(btnUpdateCostItem_Click);

            //btnUpdateCostItemCateFilter.Click += new EventHandler(btnUpdateCostItemCateFilter_Click);
        }

        void btnCheckNode_Click(object sender, EventArgs e)
        {
            if (tvwCategory.SelectedNode.Tag != null)
            {
                CostItemCategory cate = tvwCategory.SelectedNode.Tag as CostItemCategory;
                //ObjectQuery oq1 = new ObjectQuery();
                //oq1.AddCriterion(Expression.Eq("Id", cate.Id));
                //oq1.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
                //cate = model.ObjectQuery(typeof(CostItemCategory), oq1)[0] as CostItemCategory;
                txtNode.Tag = cate;
                txtNode.Text = StaticMethod.GetCategorTreeFullPath(typeof(CostItemCategory), cate.Name, cate.SysCode);//cate.Name;
            }
            else
            {
                MessageBox.Show("请选择一个分类！");
            }
        }
        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtNode.Text = "";
            txtNode.Tag = null;
        }
        //修改成本项分类过滤条件
        void btnUpdateCostItemCateFilter_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }


                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                List<CostItemCategory> listCate = new List<CostItemCategory>();
                IList listUpdate = new ArrayList();

                #region 导入数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);



                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];

                            string cateCode = dataRow["修改后成本项分类代码"].ToString().Trim();
                            string costItemNameUpdate = dataRow["修改后成本项名称"].ToString().Trim();
                            string cateFilter = dataRow["基数成本项分类过滤"].ToString().Trim();

                            if (!string.IsNullOrEmpty(cateFilter) && cateFilter.IndexOf("0") != 0)
                            {
                                cateFilter = "0" + cateFilter;
                            }

                            CostItemCategory cate = null;
                            var query = from c in listCate
                                        where c.Code == cateCode
                                        select c;
                            if (query.Count() > 0)
                            {
                                cate = query.ElementAt(0);
                            }
                            else
                            {
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Code", cateCode));
                                IList listTemp = model.ObjectQuery(typeof(CostItemCategory), oq);
                                if (listTemp.Count > 0)
                                {
                                    cate = listTemp[0] as CostItemCategory;
                                    listCate.Add(cate);
                                }
                                else
                                {
                                    logMsg.Append("根据成本项分类代码“" + cateCode + "”未查询到指定的成本项分类。");
                                    logMsg.Append(Environment.NewLine);
                                    continue;
                                }
                            }

                            ObjectQuery oq2 = new ObjectQuery();
                            oq2.AddCriterion(Expression.Eq("TheCostItemCategory.Id", cate.Id));
                            oq2.AddCriterion(Expression.Eq("Name", costItemNameUpdate));
                            IList listCostItem = model.ObjectQuery(typeof(CostItem), oq2);

                            if (listCostItem.Count > 0)
                            {
                                CostItem item = listCostItem[0] as CostItem;

                                if (string.IsNullOrEmpty(cateFilter))
                                {
                                    item.CateFilter1 = null;
                                    item.CateFilterName1 = "";
                                    item.CateFilterSysCode1 = "";
                                }
                                else
                                {
                                    oq2.Criterions.Clear();
                                    oq2.AddCriterion(Expression.Eq("Code", cateFilter));
                                    IList listTemp = model.ObjectQuery(typeof(CostItemCategory), oq2);
                                    if (listTemp.Count > 0)
                                    {
                                        cate = listTemp[0] as CostItemCategory;
                                        item.CateFilter1 = cate;
                                        item.CateFilterName1 = cate.Name;
                                        item.CateFilterSysCode1 = cate.SysCode;
                                    }
                                    else
                                    {
                                        logMsg.Append("根据成本项分类过滤代码“" + cateFilter + "”未查询到指定的成本项分类。");
                                        logMsg.Append(Environment.NewLine);
                                        continue;
                                    }
                                }

                                listUpdate.Add(item);
                            }
                            else
                            {
                                logMsg.Append("根据成本项分类代码“" + cateCode + "”和成本项名称“" + costItemNameUpdate + "”未查询到指定的成本项。");
                                logMsg.Append(Environment.NewLine);
                                continue;
                            }
                        }
                    }
                }

                if (logMsg.Length > 0)
                {
                    //写日志
                    StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                    write.Write(logMsg.ToString());
                    write.Close();
                    write.Dispose();

                    MessageBox.Show(logMsg.ToString());
                }
                else
                {
                    if (MessageBox.Show("确认要更新选择表格里选择的所有成本项数据吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //保存数据
                        model.SaveOrUpdateCostItem(listUpdate);
                    }
                }




                MessageBox.Show("更新完毕！");


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        //修改成本项分类信息
        void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }


                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                List<CostItemCategory> listCate = new List<CostItemCategory>();
                IList listUpdate = new ArrayList();

                #region 导入数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);



                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];

                            string cateCode = dataRow["分类代码"].ToString().Trim();
                            string cateCodeUpdate = dataRow["修改后分类代码"].ToString().Trim();
                            string cateName = dataRow["修改后分类名称"].ToString().Trim();

                            CostItemCategory cate = null;
                            var query = from c in listCate
                                        where c.Code == cateCode
                                        select c;
                            if (query.Count() > 0)
                            {
                                cate = query.ElementAt(0);
                            }
                            else
                            {
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Code", cateCode));
                                IList listTemp = model.ObjectQuery(typeof(CostItemCategory), oq);
                                if (listTemp.Count > 0)
                                {
                                    cate = listTemp[0] as CostItemCategory;
                                    listCate.Add(cate);
                                }
                                else
                                {
                                    logMsg.Append("根据成本项分类代码“" + cateCode + "”未查询到指定的成本项分类。");
                                    logMsg.Append(Environment.NewLine);
                                    continue;
                                }
                            }

                            cate.Code = cateCodeUpdate;
                            cate.Name = cateName;

                            listUpdate.Add(cate);
                        }
                    }
                }

                if (logMsg.Length > 0)
                {
                    //写日志
                    StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                    write.Write(logMsg.ToString());
                    write.Close();
                    write.Dispose();

                    MessageBox.Show(logMsg.ToString());
                }
                else
                {
                    if (MessageBox.Show("确认要更新选择表格里选择的所有成本项分类的数据吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //保存数据
                        model.SaveOrUpdateCostItem(listUpdate);
                    }
                }




                MessageBox.Show("更新完毕！");


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        //修改成本项信息
        void btnUpdateCostItem_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }


                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                List<CostItemCategory> listCate = new List<CostItemCategory>();
                IList listUpdate = new ArrayList();

                #region 导入数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);



                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];

                            string cateCode = dataRow["成本项分类代码"].ToString().Trim();
                            string costItemName = dataRow["成本项名称"].ToString().Trim();
                            string costItemNameUpdate = dataRow["修改后成本项名称"].ToString().Trim();
                            string cateFilter = dataRow["修改后基数成本项分类过滤"].ToString().Trim();

                            CostItemCategory cate = null;
                            var query = from c in listCate
                                        where c.Code == cateCode
                                        select c;
                            if (query.Count() > 0)
                            {
                                cate = query.ElementAt(0);
                            }
                            else
                            {
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Code", cateCode));
                                IList listTemp = model.ObjectQuery(typeof(CostItemCategory), oq);
                                if (listTemp.Count > 0)
                                {
                                    cate = listTemp[0] as CostItemCategory;
                                    listCate.Add(cate);
                                }
                                else
                                {
                                    logMsg.Append("根据成本项分类代码“" + cateCode + "”未查询到指定的成本项分类。");
                                    logMsg.Append(Environment.NewLine);
                                    continue;
                                }
                            }

                            ObjectQuery oq2 = new ObjectQuery();
                            oq2.AddCriterion(Expression.Eq("TheCostItemCategory.Id", cate.Id));
                            oq2.AddCriterion(Expression.Eq("Name", costItemName));
                            IList listCostItem = model.ObjectQuery(typeof(CostItem), oq2);

                            if (listCostItem.Count > 0)
                            {
                                CostItem item = listCostItem[0] as CostItem;
                                item.Name = costItemNameUpdate;

                                if (string.IsNullOrEmpty(cateFilter))
                                {
                                    item.CateFilter1 = null;
                                    item.CateFilterName1 = "";
                                    item.CateFilterSysCode1 = "";
                                }
                                else
                                {
                                    oq2.Criterions.Clear();
                                    oq2.AddCriterion(Expression.Eq("Code", cateFilter));
                                    IList listTemp = model.ObjectQuery(typeof(CostItemCategory), oq2);
                                    if (listTemp.Count > 0)
                                    {
                                        cate = listTemp[0] as CostItemCategory;
                                        item.CateFilter1 = cate;
                                        item.CateFilterName1 = cate.Name;
                                        item.CateFilterSysCode1 = cate.SysCode;
                                    }
                                    else
                                    {
                                        logMsg.Append("根据成本项分类过滤代码“" + cateFilter + "”未查询到指定的成本项分类。");
                                        logMsg.Append(Environment.NewLine);
                                        continue;
                                    }
                                }

                                listUpdate.Add(item);
                            }
                            else
                            {
                                logMsg.Append("根据成本项分类代码“" + cateCode + "”和成本项名称“" + costItemName + "”未查询到指定的成本项。");
                                logMsg.Append(Environment.NewLine);
                                continue;
                            }
                        }
                    }
                }

                if (logMsg.Length > 0)
                {
                    //写日志
                    StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                    write.Write(logMsg.ToString());
                    write.Close();
                    write.Dispose();

                    MessageBox.Show(logMsg.ToString());
                }
                else
                {
                    if (MessageBox.Show("确认要更新选择表格里选择的所有成本项数据吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //保存数据
                        model.SaveOrUpdateCostItem(listUpdate);
                    }
                }




                MessageBox.Show("更新完毕！");


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        void btnUpdateCostItemPricing_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }


                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                List<CostItemCategory> listCate = new List<CostItemCategory>();
                IList listUpdate = new ArrayList();

                #region 导入数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);



                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];

                            string cateCode = dataRow["成本项分类代码"].ToString().Trim();
                            string costItemName = dataRow["成本项名称"].ToString().Trim();
                            string rate = dataRow["计价费率"].ToString().Trim();

                            CostItemCategory cate = null;
                            var query = from c in listCate
                                        where c.Code == cateCode
                                        select c;
                            if (query.Count() > 0)
                            {
                                cate = query.ElementAt(0);
                            }
                            else
                            {
                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(Expression.Eq("Code", cateCode));
                                IList listTemp = model.ObjectQuery(typeof(CostItemCategory), oq);
                                if (listTemp.Count > 0)
                                {
                                    cate = listTemp[0] as CostItemCategory;
                                    listCate.Add(cate);
                                }
                                else
                                {
                                    logMsg.Append("根据成本项分类代码“" + cateCode + "”未查询到指定的成本项分类。");
                                    logMsg.Append(Environment.NewLine);
                                    continue;
                                }
                            }

                            ObjectQuery oq2 = new ObjectQuery();
                            oq2.AddCriterion(Expression.Eq("TheCostItemCategory.Id", cate.Id));
                            oq2.AddCriterion(Expression.Eq("Name", costItemName));
                            IList listCostItem = model.ObjectQuery(typeof(CostItem), oq2);

                            if (listCostItem.Count > 0)
                            {
                                CostItem item = listCostItem[0] as CostItem;
                                item.PricingRate = ClientUtil.ToDecimal(rate.Replace("%", "")) / 100;
                                listUpdate.Add(item);
                            }
                            else
                            {
                                logMsg.Append("根据成本项分类代码“" + cateCode + "”和成本项名称“" + costItemName + "”未查询到指定的成本项。");
                                logMsg.Append(Environment.NewLine);
                                continue;
                            }
                        }
                    }
                }

                if (logMsg.Length > 0)
                {
                    //写日志
                    StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                    write.Write(logMsg.ToString());
                    write.Close();
                    write.Dispose();

                    MessageBox.Show(logMsg.ToString());
                }
                else
                {
                    if (MessageBox.Show("确认要更新选择表格里所有成本项的计价费率吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //保存数据
                        model.SaveOrUpdateCostItem(listUpdate);
                    }
                }




                MessageBox.Show("更新完毕！");


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        void btnGetQuotaHasQuantity_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }


                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                Dictionary<string, decimal> listCostItemQuotaCode = new Dictionary<string, decimal>();//成本项定额号和定额所含数量

                #region 导入数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];

                            string orderNo = dataRow["序号"].ToString().Trim();
                            string quotaCode = dataRow["定额号"].ToString().Trim();
                            string quotaRate = dataRow["定额所含数量"].ToString().Trim();
                            string cateCode = dataRow["分类编码"].ToString().Trim();

                            if (!string.IsNullOrEmpty(orderNo) && !string.IsNullOrEmpty(quotaCode) && !string.IsNullOrEmpty(cateCode)
                                && !string.IsNullOrEmpty(quotaRate) && ClientUtil.ToDecimal(quotaRate) > 1)//成本项且定额所含数量大于1
                            {
                                bool flag = false;
                                for (int z = j + 1; z < dt.Rows.Count; z++)
                                {
                                    DataRow dataRow1 = dt.Rows[z];

                                    string orderNo1 = dataRow1["序号"].ToString().Trim();//定额不使用，用来判断下一个成本项
                                    string quotaCode1 = dataRow1["定额号"].ToString().Trim();
                                    string cateCode1 = dataRow["分类编码"].ToString().Trim();

                                    if (quotaCode1 == "主材")
                                    {
                                        flag = true;
                                        break;
                                    }

                                    if (!string.IsNullOrEmpty(orderNo1) && !string.IsNullOrEmpty(cateCode1))//下一个成本项跳出
                                    {
                                        j = z - 1;
                                        break;
                                    }
                                    if (z == dt.Rows.Count - 1)
                                    {
                                        j = z;
                                    }
                                }

                                if (flag)
                                    listCostItemQuotaCode.Add(quotaCode, ClientUtil.ToDecimal(quotaRate));
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }


                int count = listCostItemQuotaCode.Count;

                if (MessageBox.Show("确认要更新主材的定额工程量的系数吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SubjectCostQuota q = new SubjectCostQuota();

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("CostAccountSubjectGUID.Id", "2bD8P9jDr6euVU1qnBUl0Q"));
                    Disjunction dis = new Disjunction();
                    foreach (var quota in listCostItemQuotaCode)
                    {
                        dis.Add(Expression.Eq("TheCostItem.QuotaCode", quota.Key.ToUpper()));
                    }
                    oq.AddCriterion(dis);
                    oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

                    IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oq);

                    for (int i = 0; i < listQuota.Count; i++)
                    {
                        SubjectCostQuota quota = listQuota[i] as SubjectCostQuota;
                        decimal rate = listCostItemQuotaCode[quota.TheCostItem.QuotaCode];
                        quota.QuotaProjectAmount = quota.QuotaProjectAmount / rate;
                    }

                    model.SaveOrUpdateCostItem(listQuota);
                }

                //if (logMsg.Length > 0)
                //{
                //    //写日志
                //    StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                //    write.Write(logMsg.ToString());
                //    write.Close();
                //    write.Dispose();
                //}
                //else
                //{
                //    //保存数据
                //    model.SaveOrUpdateCostItem(listSaveCostItem);
                //}

                MessageBox.Show("操作完毕！");


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        private void InitForm()
        {
            LoadCostItemCategoryTree();
            foreach (string usedLevel in Enum.GetNames(typeof(CostItemApplyLeve)))
            {
                cbUsedLevel.Items.Add(usedLevel);
            }
            cbUsedLevel.SelectedIndex = 1;

            foreach (string contentType in Enum.GetNames(typeof(CostItemContentType)))
            {
                cbContentType.Items.Add(contentType);
            }
            cbContentType.SelectedIndex = 0;

            IList list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.CostItem_ManagementMode);
            if (list != null)
            {
                foreach (BasicDataOptr bdo in list)
                {
                    cbManagemode.Items.Add(bdo);
                }
                cbManagemode.DisplayMember = "BasicName";
                cbManagemode.ValueMember = "Id";

                cbManagemode.SelectedIndex = 0;
            }
        }

        void btnBrownFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                txtExcelFilePath.Text = filePath;

                OleDbConnection conpart = null;
                try
                {
                    string ConnectionString = string.Empty;

                    if (Path.GetExtension(filePath).ToLower() == ".xls")
                        ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                    else
                        ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                    conpart = new OleDbConnection(ConnectionString);
                    conpart.Open();

                    DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    List<CostItemCategory> listCate = new List<CostItemCategory>();

                    cbTableNames.Items.Clear();
                    for (int i = 0; i < tables.Rows.Count; i++)
                    {
                        DataRow row = tables.Rows[i];

                        string tableName = row["TABLE_NAME"].ToString().Trim();

                        if (tableName != "" && tableName.Substring(tableName.Length - 1) == "$")
                        {
                            cbTableNames.Items.Add(tableName);
                        }
                    }
                }
                catch
                { }
                finally
                {
                    if (conpart != null)
                    {
                        conpart.Close();
                        conpart.Dispose();
                    }
                }
            }
        }

        void btnImport_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                CostItemCategory selectCate = new CostItemCategory();
                if (tvwCategory.SelectedNode == null)
                {
                    MessageBox.Show("请先选择一个成本项分类！");
                    return;
                }
                else
                {
                    selectCate = tvwCategory.SelectedNode.Tag as CostItemCategory;
                }
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请选择一个Excel文件！");
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                string ConnectionString = string.Empty;

                if (Path.GetExtension(filePath).ToLower() == ".xls")
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                else
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";

                conpart = new OleDbConnection(ConnectionString);
                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }

                List<StandardUnit> listAllUnit = new List<StandardUnit>();//存使用的所有计量单位

                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();


                List<Material> listAllMat = new List<Material>();//存使用的所有资源类型

                List<CostItemCategory> listCostItemCategory = new List<CostItemCategory>();//存使用的所有成本项分类

                IList listSaveCostItem = new ArrayList();//保存的成本项

                CostItem optItem = null;


                ObjectQuery oq = new ObjectQuery();

                StandardUnit priceUnit = null;
                StandardUnit personUnit = null;//人工单位
                StandardUnit materialUnit = null;//材料和机械单位

                oq.AddCriterion(Expression.Eq("Name", "元"));
                IList listTemp = model.ObjectQuery(typeof(StandardUnit), oq);
                if (listTemp.Count > 0)
                {
                    priceUnit = listTemp[0] as StandardUnit;
                    listAllUnit.Add(priceUnit);
                }
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Name", "项"));
                listTemp = model.ObjectQuery(typeof(StandardUnit), oq);
                if (listTemp.Count > 0)
                {
                    materialUnit = listTemp[0] as StandardUnit;
                    listAllUnit.Add(materialUnit);
                }
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Name", "工日"));
                listTemp = model.ObjectQuery(typeof(StandardUnit), oq);
                if (listTemp.Count > 0)
                {
                    personUnit = listTemp[0] as StandardUnit;
                    listAllUnit.Add(personUnit);
                }
                oq.Criterions.Clear();


                Material personMat = null;//人工资源类型
                Material materialMat = null;//材料资源类型
                Material machineMat = null;//机械资源类型

                oq.AddCriterion(Expression.Eq("Code", "R20100000"));
                listTemp = model.ObjectQuery(typeof(Material), oq);
                if (listTemp.Count > 0)
                {
                    personMat = listTemp[0] as Material;
                    listAllMat.Add(personMat);
                }
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Code", "R30300000"));
                listTemp = model.ObjectQuery(typeof(Material), oq);
                if (listTemp.Count > 0)
                {
                    materialMat = listTemp[0] as Material;
                    listAllMat.Add(materialMat);
                }
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Code", "R30400000"));
                listTemp = model.ObjectQuery(typeof(Material), oq);
                if (listTemp.Count > 0)
                {
                    machineMat = listTemp[0] as Material;
                    listAllMat.Add(machineMat);
                }
                oq.Criterions.Clear();

                CostAccountSubject personSubject = null;//人工核算科目
                CostAccountSubject materialSubject = null;//材料核算科目
                CostAccountSubject machineSubject = null;//机械核算科目
                CostAccountSubject masterMatSubject = null;//主材核算科目

                oq.AddCriterion(Expression.Eq("Code", "C5110121"));
                listTemp = model.ObjectQuery(typeof(CostAccountSubject), oq);
                if (listTemp.Count > 0)
                {
                    personSubject = listTemp[0] as CostAccountSubject;
                }
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Code", "C5110122"));
                listTemp = model.ObjectQuery(typeof(CostAccountSubject), oq);
                if (listTemp.Count > 0)
                {
                    materialSubject = listTemp[0] as CostAccountSubject;
                }
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Code", "C5110123"));
                listTemp = model.ObjectQuery(typeof(CostAccountSubject), oq);
                if (listTemp.Count > 0)
                {
                    machineSubject = listTemp[0] as CostAccountSubject;
                }
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Code", "C5110222"));
                listTemp = model.ObjectQuery(typeof(CostAccountSubject), oq);
                if (listTemp.Count > 0)
                {
                    masterMatSubject = listTemp[0] as CostAccountSubject;
                }
                oq.Criterions.Clear();



                decimal personQuotaPrice = ClientUtil.ToDecimal(txtPersonQuotaPrice.Text);//人工费定额单价

                BasicDataOptr manageMode = cbManagemode.SelectedItem as BasicDataOptr;
                CostItemApplyLeve usedLevel = VirtualMachine.Component.Util.EnumUtil<CostItemApplyLeve>.FromDescription(cbUsedLevel.Text.Trim());
                CostItemContentType contentType = VirtualMachine.Component.Util.EnumUtil<CostItemContentType>.FromDescription(cbContentType.Text.Trim());

                Dictionary<string, int> dicCateMaxCostItemCode = new Dictionary<string, int>();

                #region 导入数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        string sqlStr = "select * from [" + tableName + "]";
                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, conpart);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];


                            string orderNo = dataRow["序号"].ToString().Trim();
                            string quotaCode = dataRow["定额号"].ToString().Trim();
                            string quotaName = dataRow["定额名称"].ToString().Trim();
                            string quotaRate = dataRow["定额所含数量"].ToString().Trim();
                            string projectUnitName = dataRow["单位"].ToString().Trim();
                            string projectQuantity = dataRow["工程量"].ToString().Trim();
                            string quotaPrice = dataRow["定额基价"].ToString().Trim();

                            string cateCode = dataRow["分类编码"].ToString().Trim();
                            string materialCode = dataRow["物资编码"].ToString().Trim();

                            if (!string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(cateCode))//成本项
                            {
                                logMsg.Append("Excel表" + tableName + "第" + j + "行数据的成本项所属分类编码为空.");
                                logMsg.Append(Environment.NewLine);

                                optItem = null;

                                continue;
                            }

                            if (quotaCode != "人工" && quotaCode != "材料" && quotaCode != "机械") //需合并 使用成本项的计量单位
                            {
                                if (projectUnitName.IndexOf(quotaRate) == 0 && quotaRate != "")
                                {
                                    projectUnitName = projectUnitName.Replace(quotaRate, "");
                                }

                                var query = from u in listAllUnit where u.Name == projectUnitName select u;
                                if (query.Count() == 0)
                                {

                                    if (string.IsNullOrEmpty(projectUnitName))
                                    {
                                        logMsg.Append("Excel表" + tableName + "第" + j + "行数据的计量单位为空.");
                                        logMsg.Append(Environment.NewLine);
                                    }
                                    else
                                    {
                                        oq.Criterions.Clear();
                                        oq.FetchModes.Clear();

                                        oq.AddCriterion(Expression.Eq("Name", projectUnitName));
                                        IList list = model.ObjectQuery(typeof(StandardUnit), oq);
                                        if (list.Count == 0)
                                        {
                                            logMsg.Append("Excel表" + tableName + "第" + j + "行数据的工程量单位“" + projectUnitName + "”未找到.");
                                            logMsg.Append(Environment.NewLine);
                                        }
                                        else
                                        {
                                            listAllUnit.Add(list[0] as StandardUnit);
                                        }
                                    }
                                }
                            }

                            StandardUnit projectUnit = null;

                            var queryUnit = from u in listAllUnit where u.Name == projectUnitName select u;

                            if (queryUnit.Count() > 0)
                                projectUnit = queryUnit.ElementAt(0);

                            if (!string.IsNullOrEmpty(orderNo) && !string.IsNullOrEmpty(cateCode))//成本项
                            {
                                if (cateCode.IndexOf("0") == -1 || cateCode.IndexOf("0") > 0)
                                    cateCode = "0" + cateCode;

                                //确定成本项分类
                                if (listCostItemCategory.Count == 0 || (from m in listCostItemCategory where m.Code == cateCode select m).Count() == 0)
                                {
                                    oq.Criterions.Clear();
                                    oq.FetchModes.Clear();

                                    oq.AddCriterion(Expression.Eq("Code", cateCode));
                                    oq.AddCriterion(Expression.Like("SysCode", selectCate.SysCode, MatchMode.Start));
                                    IList list = model.ObjectQuery(typeof(CostItemCategory), oq);

                                    if (list.Count == 0)
                                    {
                                        logMsg.Append("Excel表" + tableName + "第" + j + "行数据的成本项(定额编号“" + quotaCode + "”,成本项名称“" + quotaName + "”)所属分类“" + cateCode + "”未找到,该成本项及下属定额数据导入失败.");
                                        logMsg.Append(Environment.NewLine);

                                        optItem = null;

                                        continue;
                                    }

                                    listCostItemCategory.Add(list[0] as CostItemCategory);
                                }

                                CostItemCategory optCate = (from m in listCostItemCategory where m.Code == cateCode select m).ElementAt(0);


                                optItem = new CostItem();

                                optItem.TheCostItemCategory = optCate;
                                optItem.TheCostItemCateSyscode = optCate.SysCode;


                                int costItemCode = 0;
                                if (dicCateMaxCostItemCode.Count == 0)
                                {
                                    costItemCode = model.GetMaxCostItemCodeByCate(optCate.Id);
                                    dicCateMaxCostItemCode.Add(optCate.Id, costItemCode);
                                }
                                else if (dicCateMaxCostItemCode.ContainsKey(optCate.Id))
                                {
                                    costItemCode = dicCateMaxCostItemCode[optCate.Id];
                                    costItemCode += 1;
                                    dicCateMaxCostItemCode[optCate.Id] = costItemCode;
                                }
                                else
                                {
                                    costItemCode = model.GetMaxCostItemCodeByCate(optCate.Id);
                                    dicCateMaxCostItemCode.Add(optCate.Id, costItemCode);
                                }

                                optItem.Code = optCate.Code + "-" + costItemCode.ToString().PadLeft(5, '0');
                                optItem.Name = quotaName;
                                optItem.QuotaCode = quotaCode;

                                optItem.ItemState = CostItemState.发布;

                                optItem.ApplyLevel = usedLevel;
                                optItem.ContentType = contentType;
                                optItem.ManagementMode = manageMode;
                                if (manageMode != null)
                                    optItem.ManagementModeName = manageMode.BasicName;

                                optItem.PricingRate = 1;
                                optItem.PricingType = CostItemPricingType.固定价格;

                                optItem.PriceUnitGUID = priceUnit;
                                if (priceUnit != null)
                                    optItem.PriceUnitName = priceUnit.Name;

                                optItem.ProjectUnitGUID = projectUnit;
                                if (projectUnit != null)
                                    optItem.ProjectUnitName = projectUnit.Name;


                                listSaveCostItem.Add(optItem);
                            }
                            else
                            {
                                if (optItem == null)
                                    continue;

                                List<SubjectCostQuota> listQuota = new List<SubjectCostQuota>();

                                #region 构造成本项的定额集

                                for (int z = j; z < dt.Rows.Count; z++)
                                {
                                    DataRow dataRow1 = dt.Rows[z];

                                    cateCode = dataRow1["分类编码"].ToString().Trim();//定额不使用，用来判断下一个成本项
                                    orderNo = dataRow1["序号"].ToString().Trim();//定额不使用，用来判断下一个成本项

                                    materialCode = dataRow1["物资编码"].ToString().Trim();

                                    quotaCode = dataRow1["定额号"].ToString().Trim();
                                    quotaName = dataRow1["定额名称"].ToString().Trim();
                                    quotaRate = dt.Rows[j - 1]["定额所含数量"].ToString().Trim();//使用成本项的定额所含数量
                                    projectUnitName = dataRow1["单位"].ToString().Trim();
                                    projectQuantity = dataRow1["工程量"].ToString().Trim();
                                    quotaPrice = dataRow1["定额基价"].ToString().Trim();


                                    quotaRate = string.IsNullOrEmpty(quotaRate) ? "1" : quotaRate;

                                    projectQuantity = string.IsNullOrEmpty(projectQuantity) ? "0" : projectQuantity;
                                    quotaPrice = string.IsNullOrEmpty(quotaPrice) ? "0" : quotaPrice;

                                    if (!string.IsNullOrEmpty(orderNo) && !string.IsNullOrEmpty(cateCode))//下一个成本项跳出
                                    {
                                        j = z - 1;
                                        break;
                                    }
                                    if (z == dt.Rows.Count - 1)
                                    {
                                        j = z;
                                    }

                                    Material optMaterial = null;

                                    if (!string.IsNullOrEmpty(materialCode) && (listAllMat.Count == 0 || (from m in listAllMat where m.Code == materialCode select m).Count() == 0))
                                    {
                                        oq.Criterions.Clear();
                                        oq.FetchModes.Clear();

                                        oq.AddCriterion(Expression.Eq("Code", materialCode));
                                        IList list = model.ObjectQuery(typeof(Material), oq);

                                        if (list.Count == 0)
                                        {
                                            oq.Criterions.Clear();

                                            materialCode = materialCode + "00000";
                                            oq.AddCriterion(Expression.Eq("Code", materialCode));
                                            list = model.ObjectQuery(typeof(Material), oq);

                                            if (list.Count == 0)
                                            {
                                                logMsg.Append("Excel表" + tableName + "第" + j + "行数据的耗用定额号“" + quotaCode + "”、定额名称“" + quotaName + "”的资源编码“" + materialCode + "”未找到指定资源对象.");
                                                logMsg.Append(Environment.NewLine);
                                                continue;
                                            }
                                        }

                                        optMaterial = list[0] as Material;

                                        listAllMat.Add(optMaterial);
                                    }

                                    SubjectCostQuota quota = new SubjectCostQuota();

                                    //临时存储（用于分组的时候生成资源组）
                                    if (optMaterial != null)
                                    {
                                        quota.ResourceTypeGUID = optMaterial.Id;
                                        quota.ResourceTypeName = optMaterial.Name;
                                    }
                                    quota.State = SubjectCostQuotaState.生效;
                                    quota.Code = quotaCode;//临时存储（人工，材料，机械），作为资源分组的条件
                                    quota.Name = quotaName;

                                    quota.PriceUnitGUID = priceUnit;
                                    quota.PriceUnitName = priceUnit.Name;

                                    if (quotaCode == "材料" || quotaCode == "机械")//单位为“项”
                                    {
                                        quota.ProjectAmountUnitGUID = materialUnit;
                                        quota.ProjectAmountUnitName = materialUnit.Name;
                                    }
                                    else if (quotaCode == "人工")//单位为“工日”
                                    {
                                        quota.ProjectAmountUnitGUID = personUnit;
                                        quota.ProjectAmountUnitName = personUnit.Name;
                                    }
                                    else
                                    {
                                        quota.ProjectAmountUnitGUID = projectUnit;
                                        if (projectUnit != null)
                                            quota.ProjectAmountUnitName = projectUnit.Name;
                                    }


                                    quota.QuotaPrice = ClientUtil.ToDecimal(quotaPrice) / ClientUtil.ToDecimal(quotaRate);//临时存储
                                    quota.QuotaProjectAmount = ClientUtil.ToDecimal(projectQuantity);//临时存储
                                    quota.QuotaMoney = quota.QuotaProjectAmount * quota.QuotaPrice;//临时存储

                                    if (quotaCode != "人工" && quotaCode != "材料" && quotaCode != "机械" && quota.QuotaPrice == 0)//为主材时且定额单价为0
                                    {
                                        quota.QuotaProjectAmount = ClientUtil.ToDecimal(projectQuantity) / ClientUtil.ToDecimal(quotaRate);//临时存储
                                    }



                                    CostAccountSubject optSubject = null;
                                    if (quotaCode.Trim() == "人工")
                                    {
                                        optSubject = personSubject;
                                    }
                                    else if (quotaCode.Trim() == "材料")
                                    {
                                        optSubject = materialSubject;
                                    }
                                    else if (quotaCode.Trim() == "机械")
                                    {
                                        optSubject = machineSubject;
                                    }
                                    else
                                        optSubject = masterMatSubject;

                                    quota.CostAccountSubjectGUID = optSubject;
                                    if (optSubject != null)
                                        quota.CostAccountSubjectName = optSubject.Name;

                                    listQuota.Add(quota);
                                }

                                #endregion

                                //更新成本项
                                if (listQuota.Count > 0)
                                {

                                    IEnumerable<SubjectCostQuota> queryResType = from q in listQuota
                                                                                 where q.Code == "人工" || q.Code == "材料" || q.Code == "机械"
                                                                                 select q;

                                    var queryResTypeGroup = from q in queryResType
                                                            group q by new { resType = q.Code } into g
                                                            select new { g.Key.resType };

                                    #region 将 人工、材料、机械的耗用定额分组生成一个耗用定额
                                    foreach (var obj in queryResTypeGroup)
                                    {
                                        SubjectCostQuota optQuota = new SubjectCostQuota();
                                        optQuota.TheCostItem = optItem;
                                        optItem.ListQuotas.Add(optQuota);

                                        //设置基本属性
                                        optQuota.State = SubjectCostQuotaState.生效;

                                        optQuota.PriceUnitGUID = priceUnit;
                                        optQuota.PriceUnitName = priceUnit.Name;

                                        if (obj.resType == "人工")//单位为“工日”
                                        {
                                            optQuota.ProjectAmountUnitGUID = personUnit;
                                            optQuota.ProjectAmountUnitName = personUnit.Name;

                                            optQuota.Name = "人工费";

                                            optQuota.CostAccountSubjectGUID = personSubject;
                                            if (personSubject != null)
                                                optQuota.CostAccountSubjectName = personSubject.Name;
                                        }
                                        else if (obj.resType == "材料")//单位为“项”
                                        {
                                            optQuota.ProjectAmountUnitGUID = materialUnit;
                                            optQuota.ProjectAmountUnitName = materialUnit.Name;

                                            optQuota.Name = "材料费";

                                            optQuota.CostAccountSubjectGUID = materialSubject;
                                            if (materialSubject != null)
                                                optQuota.CostAccountSubjectName = materialSubject.Name;
                                        }
                                        else if (obj.resType == "机械")//单位为“项”
                                        {
                                            optQuota.ProjectAmountUnitGUID = materialUnit;
                                            optQuota.ProjectAmountUnitName = materialUnit.Name;

                                            optQuota.Name = "机械费";

                                            optQuota.CostAccountSubjectGUID = machineSubject;
                                            if (machineSubject != null)
                                                optQuota.CostAccountSubjectName = machineSubject.Name;
                                        }


                                        //设置单价、数量等数据
                                        decimal price = 0;//数量单价
                                        decimal quotaQuantity = 0;//定额数量

                                        var queryResTypeQuota = from q in listQuota
                                                                where q.Code == obj.resType
                                                                select q;


                                        if (obj.resType == "人工")
                                        {
                                            price = personQuotaPrice;

                                            decimal totalPrice = 0;
                                            foreach (SubjectCostQuota quotaItem in queryResTypeQuota)
                                            {
                                                totalPrice += quotaItem.QuotaMoney;
                                            }

                                            quotaQuantity = decimal.Round(totalPrice / price, 5);

                                        }
                                        else
                                        {
                                            foreach (SubjectCostQuota quotaItem in queryResTypeQuota)
                                            {
                                                price += quotaItem.QuotaMoney;
                                            }

                                            quotaQuantity = 1;
                                        }

                                        optQuota.QuotaPrice = decimal.Round(price, 5);
                                        optQuota.QuotaProjectAmount = quotaQuantity;
                                        optQuota.QuotaMoney = decimal.Round(price * quotaQuantity, 5);


                                        //生成资源组
                                        ResourceGroup resGroup = new ResourceGroup();
                                        resGroup.TheCostQuota = optQuota;
                                        optQuota.ListResources.Add(resGroup);


                                        Material mat = null;
                                        if (obj.resType == "人工")
                                            mat = personMat;
                                        else if (obj.resType == "材料")
                                            mat = materialMat;
                                        else
                                            mat = machineMat;

                                        resGroup.ResourceCateId = mat.Category.Id;
                                        resGroup.ResourceCateSyscode = mat.TheSyscode;
                                        resGroup.ResourceTypeGUID = mat.Id;
                                        resGroup.ResourceTypeCode = mat.Code;
                                        resGroup.ResourceTypeName = mat.Name;
                                        resGroup.ResourceTypeQuality = mat.Quality;
                                        resGroup.ResourceTypeSpec = mat.Specification;
                                        resGroup.TheCostQuota = optQuota;

                                        resGroup.IsCateResource = mat.IfCatResource == 1;
                                    }
                                    #endregion

                                    #region 将除 人工、材料、机械 费外的的耗用定额每个生成一个耗用定额
                                    int count = 0;
                                    for (int k = 0; k < listQuota.Count; k++)
                                    {
                                        SubjectCostQuota quota = listQuota[k];

                                        if (quota.Code != "人工" && quota.Code != "材料" && quota.Code != "机械")
                                        {

                                            quota.TheCostItem = optItem;
                                            optItem.ListQuotas.Add(quota);

                                            quota.Code = "";
                                            if (count == 0)
                                                quota.MainResourceFlag = true;

                                            quota.QuotaPrice = 0;
                                            quota.QuotaMoney = quota.QuotaPrice * quota.QuotaProjectAmount;

                                            //生成资源组
                                            ResourceGroup resGroup = new ResourceGroup();
                                            resGroup.TheCostQuota = quota;
                                            quota.ListResources.Add(resGroup);

                                            var queryMat = from m in listAllMat
                                                           where m.Id == quota.ResourceTypeGUID
                                                           select m;

                                            if (queryMat.Count() > 0)
                                            {
                                                Material mat = queryMat.ElementAt(0);
                                                resGroup.ResourceCateId = mat.Category.Id;
                                                resGroup.ResourceCateSyscode = mat.TheSyscode;
                                                resGroup.ResourceTypeGUID = mat.Id;
                                                resGroup.ResourceTypeCode = mat.Code;
                                                resGroup.ResourceTypeName = mat.Name;
                                                resGroup.ResourceTypeQuality = mat.Quality;
                                                resGroup.ResourceTypeSpec = mat.Specification;
                                                resGroup.TheCostQuota = quota;

                                                resGroup.IsCateResource = mat.IfCatResource == 1;
                                            }

                                            count += 1;
                                        }
                                    }
                                    #endregion

                                }

                                decimal projectQnyPrice = 0;//成本项单价取下属工程量单位之和
                                foreach (SubjectCostQuota quota in optItem.ListQuotas)
                                {
                                    projectQnyPrice += quota.QuotaMoney;
                                }

                                optItem.Price = decimal.Round(projectQnyPrice, 5);
                            }
                        }


                        if (logMsg.Length > 0)
                        {
                            //写日志
                            StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                            write.Write(logMsg.ToString());
                            write.Close();
                            write.Dispose();

                            logMsg = new StringBuilder();
                        }
                        else
                        {
                            //保存数据
                            model.SaveOrUpdateCostItem(listSaveCostItem);

                            //写日志
                            StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                            write.WriteLine("");
                            write.WriteLine("");
                            write.WriteLine("");
                            write.WriteLine("Excel表" + tableName + "共导入成本项：" + listSaveCostItem.Count + "项。");
                            write.Close();
                            write.Dispose();

                            listSaveCostItem.Clear();
                        }


                        System.Threading.Thread.Sleep(2000);

                    }
                }



                //if (logMsg.Length > 0)
                //{
                //    //写日志
                //    StreamWriter write = new StreamWriter(logFilePath, true, Encoding.Default);
                //    write.Write(logMsg.ToString());
                //    write.Close();
                //    write.Dispose();
                //}
                //else
                //{
                //    //保存数据
                //    model.SaveOrUpdateCostItem(listSaveCostItem);
                //}

                MessageBox.Show("数据导入完毕！");

                //打开错误日志
                //if (logMsg.Length > 0)
                //{
                //    FileInfo file = new FileInfo(logFilePath);

                //    //定义一个ProcessStartInfo实例
                //    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                //    //设置启动进程的初始目录
                //    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                //    //设置启动进程的应用程序或文档名
                //    info.FileName = file.Name;
                //    //设置启动进程的参数
                //    info.Arguments = "";
                //    //启动由包含进程启动信息的进程资源
                //    try
                //    {
                //        System.Diagnostics.Process.Start(info);
                //    }
                //    catch (System.ComponentModel.Win32Exception we)
                //    {
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                if (conpart != null)
                {
                    conpart.Close();
                    conpart.Dispose();
                }
            }
        }

        void btnCostItemMng_Click(object sender, EventArgs e)
        {
            UCL.Locate("成本项维护", OperationCostItemType.成本项维护);
        }

        private void LoadCostItemCategoryTree()
        {
            Hashtable hashtable = new Hashtable();
            try
            {
                tvwCategory.Nodes.Clear();
                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                IList list = model.GetCostItemCategoryByInstance(projectInfo.Id);

                foreach (CostItemCategory childNode in list)
                {
                    if (childNode.State == 0)
                        continue;

                    TreeNode tnTmp = new TreeNode();
                    tnTmp.Name = childNode.Id.ToString();
                    tnTmp.Text = childNode.Name;
                    tnTmp.Tag = childNode;

                    if (childNode.ParentNode != null)
                    {
                        TreeNode tnp = null;
                        tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                        if (tnp != null)
                        {
                            tnp.Nodes.Add(tnTmp);
                        }
                    }
                    else
                    {
                        tvwCategory.Nodes.Add(tnTmp);
                    }
                    hashtable.Add(tnTmp.Name, tnTmp);
                }
                if (list.Count > 0)
                {
                    this.tvwCategory.SelectedNode = this.tvwCategory.Nodes[0];
                    this.tvwCategory.SelectedNode.Expand();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("加载成本项分类出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        //批量添加成本项
        void btnAdd_Click(object sender, EventArgs e)
        {
            CostItemCategory selectCate = new CostItemCategory();
            if (txtNode.Tag == null)
            {
                MessageBox.Show("请先选择一个成本项分类！");
                return;
            }
            else
            {
                decimal personQuotaPrice = ClientUtil.ToDecimal(txtPersonQuotaPrice.Text);//人工费定额单价
                BasicDataOptr manageMode = cbManagemode.SelectedItem as BasicDataOptr;
                CostItemApplyLeve usedLevel = VirtualMachine.Component.Util.EnumUtil<CostItemApplyLeve>.FromDescription(cbUsedLevel.Text.Trim());
                CostItemContentType contentType = VirtualMachine.Component.Util.EnumUtil<CostItemContentType>.FromDescription(cbContentType.Text.Trim());
                //selectCate = tvwCategory.SelectedNode.Tag as CostItemCategory;
                selectCate = txtNode.Tag as CostItemCategory;
                VCostItemAdd frm = new VCostItemAdd(selectCate, personQuotaPrice, manageMode, usedLevel, contentType, false);
                frm.ShowDialog();
            }

        }
        //批量添加成本项
        void btnAddCostItem1_Click(object sender, EventArgs e)
        {
            CostItemCategory selectCate = new CostItemCategory();
            if (txtNode.Tag == null)
            {
                MessageBox.Show("请先选择一个成本项分类！");
                return;
            }
            else
            {
                decimal personQuotaPrice = ClientUtil.ToDecimal(txtPersonQuotaPrice.Text);//人工费定额单价
                BasicDataOptr manageMode = cbManagemode.SelectedItem as BasicDataOptr;
                CostItemApplyLeve usedLevel = VirtualMachine.Component.Util.EnumUtil<CostItemApplyLeve>.FromDescription(cbUsedLevel.Text.Trim());
                CostItemContentType contentType = VirtualMachine.Component.Util.EnumUtil<CostItemContentType>.FromDescription(cbContentType.Text.Trim());
                //selectCate = tvwCategory.SelectedNode.Tag as CostItemCategory;
                selectCate = txtNode.Tag as CostItemCategory;
                VCostItemAdd frm = new VCostItemAdd(selectCate, personQuotaPrice, manageMode, usedLevel, contentType, true);
                frm.ShowDialog();
            }

        }
        //批量添措施费
        void btnAddCostitem_Click(object sender, EventArgs e)
        {
            CostItemCategory selectCate = new CostItemCategory();
            if (txtNode.Tag == null)
            {
                MessageBox.Show("请先选择一个成本项分类！");
                return;
            }
            else
            {
                selectCate = txtNode.Tag as CostItemCategory;
                VFlexCellImport frm = new VFlexCellImport("成本项数据", selectCate);
                frm.ShowDialog();
            }
        }

    }
}
