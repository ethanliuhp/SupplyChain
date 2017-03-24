using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.MaterialResource.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using System.Data.OleDb;
using System.IO;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VImportGWBSDetail : TBasicDataView
    {
        /// <summary>
        /// 选择的项目任务
        /// </summary>
        public TreeNode DefaultGWBSTreeNode = null;

        private GWBSTree OptGWBSTreeObj = null;

        public bool IsOK = true;

        public MGWBSTree model = new MGWBSTree();
        public IList zoningList = new ArrayList();//区域
        public VImportGWBSDetail()
        {
            InitializeComponent();
            InitialForm();
        }

        private void InitialForm()
        {
            InitialEvents();
        }
        private void InitialEvents()
        {
            this.Load += new EventHandler(VGWBSDetailUsageInfoEdit_Load);


            btnBrownFile.Click += new EventHandler(btnBrownFile_Click);
            btnLoadExcelData.Click += new EventHandler(btnLoadExcelData_Click);

            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);

            btnLookCostItemInfo.Click += new EventHandler(btnLookCostItemInfo_Click);
            btnRemoveSelectedRows.Click += new EventHandler(btnRemoveSelectedRows_Click);
            btnSave.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            gridDtl.CellDoubleClick += new DataGridViewCellEventHandler(gridDtl_CellDoubleClick);
        }

        void btnRemoveSelectedRows_Click(object sender, EventArgs e)
        {
            List<int> listIndex = new List<int>();
            foreach (DataGridViewRow row in gridDtl.SelectedRows)
            {
                listIndex.Add(row.Index);
            }
            listIndex.Sort();

            for (int i = listIndex.Count - 1; i > -1; i--)
            {
                int rowIndex = listIndex[i];
                gridDtl.Rows.RemoveAt(rowIndex);
            }
        }

        //查看或修改成本项
        void gridDtl_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnLookCostItemInfo_Click(btnLookCostItemInfo, new EventArgs());

        }

        void btnLookCostItemInfo_Click(object sender, EventArgs e)
        {
            if (gridDtl.CurrentRow == null)
            {
                MessageBox.Show("请选择一条成本明细！");
                return;
            }
            GWBSDetail optDtl = gridDtl.CurrentRow.Tag as GWBSDetail;

            VSelectCostItem frm = new VSelectCostItem(new MCostItem());
            frm.DefaultSelectedCostItem = optDtl.TheCostItem;
            frm.ShowDialog();

            if (frm.isOK)
            {
                CostItem optItem = frm.SelectCostItem;

                optDtl.TheCostItem = optItem;
                optDtl.TheCostItemCateSyscode = optItem.TheCostItemCateSyscode;

                gridDtl.CurrentRow.Cells[colQuotaCode.Name].Value = optItem.QuotaCode;
                gridDtl.CurrentRow.Tag = optDtl;
            }
        }



        //浏览选择文件
        void btnBrownFile_Click(object sender, EventArgs e)
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
                    btnBrownFile_Click(btnBrownFile, new EventArgs());
                    return;
                }
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
                        //string endStr=tableName.Substring(tableName.Length - 1);
                        if (tableName != "")//&& tableName.Substring(tableName.Length - 1) == "$"
                        {
                            cbTableNames.Items.Add(tableName);
                        }
                    }

                    if (cbTableNames.Items.Count == 1)
                    {
                        cbTableNames.SetSelected(0, true);
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

        //加载数据        
        void btnLoadExcelData_Click(object sender, EventArgs e)
        {
            OleDbConnection conpart = null;
            try
            {
                string filePath = txtExcelFilePath.Text.Trim();

                if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                {
                    MessageBox.Show("请先选择一个Excel文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnBrownFile.Focus();
                    return;
                }
                else if (cbTableNames.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请选择要导入数据的一个或多个Excel数据表！");
                    cbTableNames.Focus();
                    return;
                }

                FlashScreen.Show("正在读取并加载数据,请稍候......");

                string ConnectionString = string.Empty;


                if (Path.GetExtension(filePath).ToLower() == ".xls")
                {
                    ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                }
                else
                {
                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;";
                }
                conpart = new OleDbConnection(ConnectionString);

                conpart.Open();

                DataTable tables = conpart.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                List<string> listTableNames = new List<string>();
                foreach (string tableName in cbTableNames.SelectedItems)
                {
                    listTableNames.Add(tableName);
                }


                #region 导入数据到成本明细列表

                List<CostItemCategory> listCategory = new List<CostItemCategory>();
                List<CostItem> listCostItem = new List<CostItem>();
                List<string> quotaCodeList = new List<string>();

                ObjectQuery oqCate = new ObjectQuery();
                ObjectQuery oqCostItem = new ObjectQuery();
                Disjunction disCate = new Disjunction();
                Disjunction disCostItem = new Disjunction();

                string errMsg = "";

                //查询并校验基础数据
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

                            string costItemCateCode = dataRow["成本项直接或间接父分类编码"].ToString().Trim();
                            string quotaCode = dataRow["定额编号"].ToString().Trim().ToUpper();
                            quotaCodeList.Add(quotaCode);

                            if (!string.IsNullOrEmpty(costItemCateCode))
                                disCate.Add(Expression.Eq("Code", costItemCateCode));

                            if (string.IsNullOrEmpty(quotaCode))
                                errMsg += "Excel数据表“" + tableName + "”第" + (j + 2) + "行定额编号为空" + Environment.NewLine;
                            else
                                disCostItem.Add(Expression.Eq("QuotaCode", quotaCode));

                            continue;
                        }
                    }
                }
                if (errMsg == "")
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    Disjunction dis1 = new Disjunction();
                    string[] sysCodes = OptGWBSTreeObj.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sysCodes.Length; i++)
                    {
                        string sysCode = "";
                        for (int j = 0; j <= i; j++)
                        {
                            sysCode += sysCodes[j] + ".";
                        }
                        dis1.Add(Expression.Eq("GwbsSyscode", sysCode));
                    }
                    oq1.AddCriterion(dis1);
                    oq1.AddCriterion(Expression.Eq("ProjectId", OptGWBSTreeObj.TheProjectGUID));

                    zoningList = model.ObjectQuery(typeof(CostItemsZoning), oq1);
                    oq1.Criterions.Clear();
                    dis1.Criteria.Clear();
                    if (zoningList.Count == 0)
                    {
                        if (quotaCodeList != null && quotaCodeList.Count > 0)
                        {
                            string sql = "";
                            for (int z = 0; z < quotaCodeList.Count - 1; z++)
                            {
                                sql += "'" + quotaCodeList[z] + "',";
                            }
                            sql += "'" + quotaCodeList[quotaCodeList.Count - 1] + "'";
                            if (model.CheckQutaCodeIsRepeat(sql))
                            {
                                errMsg += "这些定额编号里有些在成本项里有重复,请先选择一个区域";
                            }
                        }
                    }
                }
                if (errMsg != "")
                {
                    //写日志
                    string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "importErrorLog.txt";
                    StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
                    write.WriteLine("在加载成本数据时出现以下错误：" + Environment.NewLine + errMsg);
                    write.Close();
                    write.Dispose();

                    FileInfo file = new FileInfo(logFilePath);
                    //定义一个ProcessStartInfo实例
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //设置启动进程的初始目录
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //设置启动进程的应用程序或文档名
                    info.FileName = file.Name;
                    //设置启动进程的参数
                    info.Arguments = "";
                    //启动由包含进程启动信息的进程资源
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch
                    {
                    }

                    return;
                }


                if (disCate.Criteria.Count > 0)
                {
                    oqCate.AddCriterion(disCate);
                    listCategory = model.ObjectQuery(typeof(CostItemCategory), oqCate).OfType<CostItemCategory>().ToList();
                }
                if (disCostItem.Criteria.Count > 0)
                {
                    oqCostItem.AddCriterion(disCostItem);
                    if (zoningList != null && zoningList.Count > 0)
                    {
                        CostItemsZoning z = zoningList[0] as CostItemsZoning;
                        oqCostItem.AddCriterion(Expression.Like("TheCostItemCateSyscode", z.CostItemsCateSyscode, MatchMode.Start));
                    }

                    oqCostItem.AddCriterion(Expression.Eq("ItemState", CostItemState.发布));
                    listCostItem = model.ObjectQuery(typeof(CostItem), oqCostItem).OfType<CostItem>().ToList();
                }


                if (rdClearTaskDtl.Checked)
                    gridDtl.Rows.Clear();


                ObjectQuery oq = new ObjectQuery();

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

                            string costItemCateCode = dataRow["成本项直接或间接父分类编码"].ToString().Trim();
                            string quotaCode = dataRow["定额编号"].ToString().Trim();
                            string dtlName = dataRow["明细名称"].ToString().Trim();
                            string planQuantity = dataRow["计划工程量"].ToString().Trim();

                            quotaCode = string.IsNullOrEmpty(quotaCode) ? "" : quotaCode.ToUpper();

                            var query = from c in listCostItem
                                        where c.QuotaCode == quotaCode
                                        select c;

                            CostItem currItem = null;

                            if (query.Count() == 0)
                            {
                                errMsg += "Excel数据表“" + tableName + "”第" + (j + 2) + "行根据定额编号“" + quotaCode + "”未找到相关成本项" + Environment.NewLine;
                                continue;
                            }
                            else if (query.Count() > 1)
                            {
                                if (string.IsNullOrEmpty(costItemCateCode))
                                {
                                    errMsg += "Excel数据表“" + tableName + "”第" + (j + 2) + "行根据定额编号“" + quotaCode + "”找到" + query.Count() + "个成本项" + Environment.NewLine;
                                    continue;
                                }

                                var queryCate = from c in listCategory
                                                where c.Code == costItemCateCode
                                                select c;

                                if (queryCate.Count() == 0)
                                {
                                    errMsg += "Excel数据表“" + tableName + "”第" + (j + 2) + "行根据指定成本项分类编码“" + costItemCateCode + "”未找到相关成本项分类" + Environment.NewLine;
                                    continue;
                                }

                                CostItemCategory cate = queryCate.ElementAt(0);

                                query = from c in listCostItem
                                        where c.QuotaCode == quotaCode && c.TheCostItemCateSyscode.IndexOf(cate.SysCode) > -1
                                        select c;

                                if (query.Count() == 0)
                                {
                                    errMsg += "Excel数据表“" + tableName + "”第" + (j + 2) + "行根据定额编号“" + quotaCode + "”在指定分类编码“" + costItemCateCode + "”下未找到相关成本项" + Environment.NewLine;
                                    continue;
                                }

                                currItem = query.ElementAt(0);
                            }
                            else
                                currItem = query.ElementAt(0);

                            //生成GWBS明细

                            GWBSDetail oprDtl = new GWBSDetail();
                            oprDtl.TheCostItem = currItem;
                            oprDtl.TheCostItemCateSyscode = currItem.TheCostItemCateSyscode;
                            oprDtl.Name = dtlName;
                            oprDtl.PlanWorkAmount = ClientUtil.ToDecimal(planQuantity);

                            AddGWBSDetailInfoInGrid(oprDtl);
                        }
                    }
                }

                FlashScreen.Close();

                if (errMsg != "")
                {
                    //写日志
                    string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "importErrorLog.txt";
                    StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
                    write.WriteLine("在加载成本数据时出现以下错误：" + Environment.NewLine + errMsg);
                    write.Close();
                    write.Dispose();

                    FileInfo file = new FileInfo(logFilePath);
                    //定义一个ProcessStartInfo实例
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    //设置启动进程的初始目录
                    info.WorkingDirectory = file.DirectoryName;//Application.StartupPath; 
                    //设置启动进程的应用程序或文档名
                    info.FileName = file.Name;
                    //设置启动进程的参数
                    info.Arguments = "";
                    //启动由包含进程启动信息的进程资源
                    try
                    {
                        System.Diagnostics.Process.Start(info);
                    }
                    catch
                    {
                    }

                    //gridDtl.Rows.Clear();
                }

            }
            catch (Exception)
            {
                FlashScreen.Close();
                throw;
            }
            finally
            {
                FlashScreen.Close();
            }
                #endregion
            //}
            //catch (Exception ex)
            //{
            //    FlashScreen.Close();
            //    MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            //}
            //finally
            //{
            //    FlashScreen.Close();
            //    if (conpart != null)
            //    {
            //        conpart.Close();
            //        conpart.Dispose();
            //    }
            //}
        }

        //选择契约组
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContractGroupName.Tag != null)
            {
                frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                txtContractGroupName.Text = cg.ContractName;
                txtContractGroupName.Tag = cg;

                txtContractGroupType.Text = cg.ContractGroupType;
            }
        }

        //保存
        void btnEnter_Click(object sender, EventArgs e)
        {
            if (SaveGWBSDetail())
                this.Close();
        }
        private bool valideContractGroup()
        {
            if (txtContractGroupName.Tag == null)
            {
                MessageBox.Show("请选择驱动契约组！");

                VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                if (txtContractGroupName.Tag != null)
                {
                    frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
                }
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                    txtContractGroupName.Text = cg.ContractName;
                    txtContractGroupType.Text = cg.ContractGroupType;
                    txtContractGroupName.Tag = cg;

                    return true;
                }
                return false;
            }
            return true;
        }
        private bool SaveGWBSDetail()
        {
            try
            {

                if (gridDtl.Rows.Count == 0)
                {
                    MessageBox.Show("当前没有要保存的成本明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!valideContractGroup())
                {
                    return false;
                }

                FlashScreen.Show("正在保存数据,请稍候......");

                ContractGroup contract = txtContractGroupName.Tag as ContractGroup;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", OptGWBSTreeObj.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                OptGWBSTreeObj = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;


                int maxOrderNo = 1;

                int code = OptGWBSTreeObj.Details.Count + 1;

                //获取父对象下最大明细号
                foreach (GWBSDetail dtl in OptGWBSTreeObj.Details)
                {
                    if (dtl.OrderNo >= maxOrderNo)
                        maxOrderNo = dtl.OrderNo;

                    if (!string.IsNullOrEmpty(dtl.Code))
                    {
                        try
                        {
                            if (dtl.Code.IndexOf("-") > -1)
                            {
                                int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                                if (tempCode >= code)
                                    code = tempCode + 1;
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                DateTime serverTime = model.GetServerTime();


                IList listSaveDtl = new ArrayList();

                foreach (DataGridViewRow row in gridDtl.Rows)
                {

                    GWBSDetail oprDtl = row.Tag as GWBSDetail;

                    oprDtl.Name = ClientUtil.ToString(row.Cells[colDtlName.Name].Value);
                    decimal planQuantity = ClientUtil.ToDecimal(row.Cells[colPlanQuantity.Name].Value);

                    if (cbResponseAccountFlag.Checked)
                    {
                        oprDtl.ResponseFlag = 1;
                        oprDtl.ResponsibilitilyWorkAmount = planQuantity;
                    }
                    if (cbCostAccountFlag.Checked)
                    {
                        oprDtl.CostingFlag = 1;
                        oprDtl.PlanWorkAmount = planQuantity;
                    }
                    if (ckbProduceConfirmFlag.Checked)
                    {
                        oprDtl.ProduceConfirmFlag = 1;
                    }

                    oprDtl.TheGWBS = OptGWBSTreeObj;
                    oprDtl.TheGWBSSysCode = OptGWBSTreeObj.SysCode;

                    if (OptGWBSTreeObj.ProjectTaskTypeGUID != null)
                        oprDtl.ProjectTaskTypeCode = OptGWBSTreeObj.ProjectTaskTypeGUID.Code;

                    oprDtl.TheProjectGUID = OptGWBSTreeObj.TheProjectGUID;
                    oprDtl.TheProjectName = OptGWBSTreeObj.TheProjectName;

                    oprDtl.ContractGroupCode = contract.Code;
                    oprDtl.ContractGroupGUID = contract.Id;
                    oprDtl.ContractGroupName = contract.ContractName;
                    oprDtl.ContractGroupType = contract.ContractGroupType;


                    oprDtl.Code = OptGWBSTreeObj.Code + "-" + code.ToString().PadLeft(5, '0');
                    oprDtl.OrderNo = maxOrderNo + 1;
                    oprDtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
                    oprDtl.CurrentStateTime = serverTime;

                    //套项
                    CostItem item = oprDtl.TheCostItem;
                    if (item != null)
                    {
                        ObjectQuery oqQuota = new ObjectQuery();

                        oqQuota.AddCriterion(Expression.Eq("TheCostItem.Id", item.Id));
                        //oqQuota.AddCriterion(Expression.Eq("State", SubjectCostQuotaState.生效));
                        oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                        oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                        oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                        oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

                        IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);

                        ResourceGroup mainResource = null;
                        //加载资源耗用明细
                        foreach (SubjectCostQuota quota in listQuota)
                        {
                            if (quota.MainResourceFlag && quota.ListResources.Count > 0)
                            {
                                var queryMainRes = from r in quota.ListResources
                                                   where r.ResourceTypeGUID == quota.ResourceTypeGUID
                                                   select r;

                                if (queryMainRes.Count() > 0)
                                {
                                    mainResource = queryMainRes.ElementAt(0);
                                }
                                else
                                    mainResource = quota.ListResources.ElementAt(0);
                            }

                            var query = from q in oprDtl.ListCostSubjectDetails
                                        where q.ResourceUsageQuota.Id == quota.Id
                                        select q;

                            if (query.Count() > 0)
                            {
                                continue;
                            }

                            AddResourceUsageInTaskDetail(ref oprDtl, quota);
                        }

                        if (mainResource != null)
                        {
                            oprDtl.MainResourceTypeId = mainResource.ResourceTypeGUID;
                            oprDtl.MainResourceTypeName = mainResource.ResourceTypeName;
                            oprDtl.MainResourceTypeQuality = mainResource.ResourceTypeQuality;
                            oprDtl.MainResourceTypeSpec = mainResource.ResourceTypeSpec;
                        }

                        oprDtl.WorkAmountUnitGUID = item.ProjectUnitGUID;
                        oprDtl.WorkAmountUnitName = item.ProjectUnitName;
                        oprDtl.PriceUnitGUID = item.PriceUnitGUID;
                        oprDtl.PriceUnitName = item.PriceUnitName;

                    }


                    listSaveDtl.Add(oprDtl);

                    code += 1;
                    maxOrderNo += 1;
                }

                //保存
                if (listSaveDtl.Count > 0)
                {
                    //根据明细设置任务对象的标记
                    bool taskResponsibleFlag = false;
                    bool taskCostAccFlag = false;
                    bool ProduceConfirmFlag = false;
                    foreach (GWBSDetail dtl in OptGWBSTreeObj.Details)
                    {
                        if (dtl.ResponseFlag == 1)
                            taskResponsibleFlag = true;
                        if (dtl.CostingFlag == 1)
                            taskCostAccFlag = true;
                        if (dtl.ProduceConfirmFlag == 1)
                            ProduceConfirmFlag = true;
                    }


                    //计算任务明细的量价
                    foreach (GWBSDetail oprDtl in listSaveDtl)
                    {
                        decimal dtlUsageProjectAmountPriceByContract = 0;
                        decimal dtlUsageProjectAmountPriceByResponsible = 0;
                        decimal dtlUsageProjectAmountPriceByPlan = 0;
                        foreach (GWBSDetailCostSubject subject in oprDtl.ListCostSubjectDetails)
                        {
                            dtlUsageProjectAmountPriceByContract += subject.ContractPrice;
                            dtlUsageProjectAmountPriceByResponsible += subject.ResponsibleWorkPrice;
                            dtlUsageProjectAmountPriceByPlan += subject.PlanWorkPrice;
                        }

                        oprDtl.ContractPrice = dtlUsageProjectAmountPriceByContract;
                        oprDtl.ResponsibilitilyPrice = dtlUsageProjectAmountPriceByResponsible;
                        oprDtl.PlanPrice = dtlUsageProjectAmountPriceByPlan;


                        oprDtl.ContractTotalPrice = oprDtl.ContractProjectQuantity * oprDtl.ContractPrice;
                        oprDtl.ResponsibilitilyTotalPrice = oprDtl.ResponsibilitilyWorkAmount * oprDtl.ResponsibilitilyPrice;
                        oprDtl.PlanTotalPrice = oprDtl.PlanWorkAmount * oprDtl.PlanPrice;

                        if (oprDtl.ResponseFlag == 1)
                            taskResponsibleFlag = true;
                        if (oprDtl.CostingFlag == 1)
                            taskCostAccFlag = true;
                        if (oprDtl.ProduceConfirmFlag == 1)
                            ProduceConfirmFlag = true;
                    }


                    //计算项目任务的合价
                    GWBSTree tempNode = new GWBSTree();
                    tempNode.Id = OptGWBSTreeObj.Id;
                    tempNode.SysCode = OptGWBSTreeObj.SysCode;
                    //tempNode = model.AccountTotalPriceByChilds(tempNode);
                    tempNode = model.AccountTotalPrice(tempNode);

                    List<Decimal> listTotalPrice = StaticMethod.GetTaskDtlTotalPrice(listSaveDtl);

                    OptGWBSTreeObj = model.GetObjectById(typeof(GWBSTree), OptGWBSTreeObj.Id) as GWBSTree;

                    OptGWBSTreeObj.ContractTotalPrice = tempNode.ContractTotalPrice + listTotalPrice[0];
                    OptGWBSTreeObj.ResponsibilityTotalPrice = tempNode.ResponsibilityTotalPrice + listTotalPrice[1];
                    OptGWBSTreeObj.PlanTotalPrice = tempNode.PlanTotalPrice + listTotalPrice[2];

                    OptGWBSTreeObj.ResponsibleAccFlag = taskResponsibleFlag;
                    OptGWBSTreeObj.CostAccFlag = taskCostAccFlag;

                    IList listGWBS = new ArrayList();
                    listGWBS.Add(OptGWBSTreeObj);

                    model.SaveOrUpdateDetail(listGWBS, listSaveDtl, false);
                }

                FlashScreen.Close();

                MessageBox.Show("保存成功！");

                return true;
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        /// <summary>
        /// 添加资源耗用到工程任务明细
        /// </summary>
        /// <param name="quota"></param>
        private void AddResourceUsageInTaskDetail(ref GWBSDetail oprDtl, SubjectCostQuota quota)
        {
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = quota.Name;
            subject.MainResTypeFlag = quota.MainResourceFlag;

            subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

            subject.PriceUnitGUID = quota.PriceUnitGUID;
            subject.PriceUnitName = quota.PriceUnitName;

            //合同
            subject.ContractQuotaQuantity = quota.QuotaProjectAmount;

            subject.ContractQuantityPrice = quota.QuotaPrice;
            subject.ContractPricePercent = 1;
            subject.ContractBasePrice = subject.ContractQuantityPrice;

            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = oprDtl.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = quota.QuotaProjectAmount;
            subject.ResponsibilitilyPrice = quota.QuotaPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibleBasePrice = subject.ResponsibilitilyPrice;
            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = oprDtl.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = quota.QuotaProjectAmount;
            subject.PlanPrice = quota.QuotaPrice;
            subject.PlanPricePercent = 1;
            subject.PlanBasePrice = subject.PlanPrice;
            subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;
            subject.PlanWorkAmount = oprDtl.PlanWorkAmount * subject.PlanQuotaNum;
            subject.PlanTotalPrice = subject.PlanWorkAmount * subject.PlanPrice;//计划包干单价未知

            subject.AssessmentRate = quota.AssessmentRate;

            subject.ResourceUsageQuota = quota;

            if (quota.ListResources.Count > 0)
            {
                ResourceGroup itemResource = quota.ListResources.ElementAt(0);

                subject.ResourceTypeGUID = itemResource.ResourceTypeGUID;
                subject.ResourceTypeCode = itemResource.ResourceTypeCode;
                subject.ResourceTypeName = itemResource.ResourceTypeName;
                subject.ResourceTypeQuality = itemResource.ResourceTypeQuality;
                subject.ResourceTypeSpec = itemResource.ResourceTypeSpec;
                subject.ResourceCateSyscode = itemResource.ResourceCateSyscode;
            }

            subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
            subject.CostAccountSubjectName = quota.CostAccountSubjectName;
            if (quota.CostAccountSubjectGUID != null)
                subject.CostAccountSubjectSyscode = quota.CostAccountSubjectGUID.SysCode;

            subject.ProjectAmountWasta = quota.Wastage;

            subject.State = GWBSDetailCostSubjectState.编制;

            subject.TheProjectGUID = oprDtl.TheProjectGUID;
            subject.TheProjectName = oprDtl.TheProjectName;

            subject.TheGWBSDetail = oprDtl;

            subject.TheGWBSTree = oprDtl.TheGWBS;
            subject.TheGWBSTreeName = oprDtl.TheGWBS.Name;
            subject.TheGWBSTreeSyscode = oprDtl.TheGWBS.SysCode;

            oprDtl.ListCostSubjectDetails.Add(subject);
        }
        //private decimal DecimalRound(Decimal val)
        //{
        //    return decimal.Round(val, 5);
        //}
        void btnCancel_Click(object sender, EventArgs e)
        {
            IsOK = false;
            this.Close();
        }

        void VGWBSDetailUsageInfoEdit_Load(object sender, EventArgs e)
        {
            if (DefaultGWBSTreeNode != null)
            {
                txtCurrentPath.Text = DefaultGWBSTreeNode.FullPath;

                OptGWBSTreeObj = DefaultGWBSTreeNode.Tag as GWBSTree;
            }
        }

        private void AddGWBSDetailInfoInGrid(GWBSDetail item)
        {
            int index = gridDtl.Rows.Add();
            DataGridViewRow row = gridDtl.Rows[index];
            row.Cells[colDtlName.Name].Value = item.Name;
            row.Cells[colQuotaCode.Name].Value = item.TheCostItem.QuotaCode;
            row.Cells[colPlanQuantity.Name].Value = item.PlanWorkAmount;

            item.PlanWorkAmount = 0;//在保存的时候从界面并根据标记（责任、计划）取

            row.Tag = item;

        }
    }
}
