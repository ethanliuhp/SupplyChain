using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using System.IO;
using System.Data.OleDb;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.FileUpload
{
    public partial class FrmInitProjectInfo : Form
    {
        private IStockInSrv stockInSrv = null;
        private IGWBSTreeSrv model2 = null;

        public FrmInitProjectInfo()
        {
            InitializeComponent();

            if (model2 == null)
                model2 = ConstMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;
            if (stockInSrv == null)
                stockInSrv = ConstMethod.GetService("StockInSrv") as IStockInSrv;
        }

        private void btnBrownOrgExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "所有文件(*.*)|*.*|Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                string extName = Path.GetExtension(filePath).ToLower();
                if (extName != ".xls" && extName != ".xlsx")
                {
                    MessageBox.Show("请选择Excel文件(支持2003和2007)！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnBrownOrgExcel_Click(btnBrownOrgExcel, new EventArgs());
                    return;
                }
                txtOrgExcel.Text = filePath;

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

                    ListBoxOrg.Items.Clear();
                    for (int i = 0; i < tables.Rows.Count; i++)
                    {
                        DataRow row = tables.Rows[i];

                        string tableName = row["TABLE_NAME"].ToString().Trim();
                        if (tableName != "")
                        {
                            ListBoxOrg.Items.Add(tableName);
                        }
                    }

                    if (ListBoxOrg.Items.Count == 1)
                    {
                        ListBoxOrg.SetSelected(0, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("异常信息：" + getMessage(ex));
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

        }

        protected string getMessage(Exception e)
        {
            if (e.InnerException == null)
                return "Message:" + e.Message;
            else
                return "Message:" + e.Message + Environment.NewLine + "InnerException:" + getMessage(e.InnerException);
        }


        private void btnSaveOrg_Click(object sender, EventArgs e)
        {
            if (txtMaxProjectCode.Text.Trim() == "")
            {
                MessageBox.Show("请设置当前最大项目号！");
                txtMaxProjectCode.Focus();
                return;
            }

             string filePath = txtOrgExcel.Text.Trim();
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("请选择Excel文件！");
                btnBrownOrgExcel_Click(btnBrownOrgExcel, new EventArgs());
                return;
            }
            List<string> listTableNames = new List<string>();
            foreach (string tableName in ListBoxOrg.SelectedItems)
            {
                listTableNames.Add(tableName);
            }

            if (listTableNames.Count == 0)
            {
                MessageBox.Show("请选择要导入的Excel表！");
                return;
            }

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

                string msg = "";
                                        int maxProjectCode = ClientUtil.ToInt(txtMaxProjectCode.Text);
                //查询并校验基础数据
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    DataRow row = tables.Rows[i];

                    string tableName = row["TABLE_NAME"].ToString().Trim();

                    if (listTableNames.Contains(tableName))
                    {
                        #region 导数

                        string sqlStr = "select * from [" + tableName + "]";

                        OleDbDataAdapter da = new OleDbDataAdapter(sqlStr, ConnectionString);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        string orgName = string.Empty;
                        string orgCode = string.Empty;
 
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            DataRow dataRow = dt.Rows[j];
                            orgCode = dataRow["组织代码"].ToString().Trim();
                            orgName = dataRow["组织名称"].ToString().Trim();
                        }
                        #endregion
                    }
                }

                if (msg != "")
                    MessageBox.Show(msg + "其他组织均保存成功！");
                else
                    MessageBox.Show("全部保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常信息：" + getMessage(ex));
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

    }
}
