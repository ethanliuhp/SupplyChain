using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using System.Data.OleDb;
using Application.Resource.MaterialResource.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.IO;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    public partial class VFlexCellImport : Form
    {
        public MExcelImportMng model = new MExcelImportMng();
        private CostItemCategory defaultCate = null;
        private CurrentProjectInfo projectInfo;
        private string importName = string.Empty;
        public VFlexCellImport(string name)
        {
            InitializeComponent();
            importName = name;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            InitEvent();
            InitData();
            lblText.Text = string.Format(">>[{0}]", importName);
        }
        public VFlexCellImport(string name, CostItemCategory oprCategory)
        {
            InitializeComponent();
            importName = name;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            defaultCate = oprCategory;
            InitEvent();
            InitData();
            lblText.Text = string.Format(">>[{0}]", importName);
        }
        public void InitEvent()
        {
            InitalFlexCell();
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnCancel1.Click += new EventHandler(btnCancel1_Click);
            this.btnInsertRows.Click += new EventHandler(btnInsertRows_Click);
        }
        private void InitData()
        {
            InitalFlexCell();
        }

        //初始化FlexCell
        public void InitalFlexCell()
        {
            flexGrid.DisplayRowNumber = true;
            if (importName.Equals("物料分类基础数据"))
            {

            }
            else if (importName.Equals("工程WBS任务类型"))
            {

            }
            else if (importName.Equals("基础数据"))
            {

            }
            else if (importName.Equals("成本核算科目"))
            {

            }
            else if (importName.Equals("成本项数据"))
            {
                InitCostItemImportFlexCell();
            }
            else if (importName.Equals("成本项分类数据"))
            {

            }
            else if (importName.Equals("会计科目"))
            {

            }
            else if (importName.Equals("成本项安装分类数据"))
            {

            }


        }
        //初始化成本项FlexCell
        private void InitCostItemImportFlexCell()
        {
            //成本项分类代码	定额编号	成本项名称	成本项说明	适用级别	适用模式	内容类型	
            //基数成本项分类过滤（第一位不是0补0）	基数成本项分类过滤二（第一位不是0补0）	基数成本科目分类过滤一	基数成本科目分类过滤二	基数成本科目分类过滤三	
            //计价费率（转换成小数）	定额耗用名称	资源类型	成本核算科目	成本核算科目名称	损耗	
            //定额单价	定额工程量	定额金额	工程量计量单位	价格计量单位	分摊比率
            flexGrid.Rows = 1;
            flexGrid.Cols = 25;
            //flexGrid.Column(0).Visible = false;
            FlexCell.Cell oCell = flexGrid.Cell(0, 1);
            oCell.Text = "成本项分类代码";
            oCell = flexGrid.Cell(0, 2);
            oCell.Text = "定额编号";
            oCell = flexGrid.Cell(0, 3);
            oCell.Text = "成本项名称";
            oCell = flexGrid.Cell(0, 4);
            oCell.Text = "成本项说明";
            oCell = flexGrid.Cell(0, 5);
            oCell.Text = "适用级别";
            oCell = flexGrid.Cell(0, 6);
            oCell.Text = "适用模式";
            oCell = flexGrid.Cell(0, 7);
            oCell.Text = "内容类型";
            oCell = flexGrid.Cell(0, 8);
            oCell.Text = "基数成本项分类过滤（第一位不是0补0）";
            oCell = flexGrid.Cell(0, 9);
            oCell.Text = "基数成本项分类过滤二（第一位不是0补0）";
            oCell = flexGrid.Cell(0, 10);
            oCell.Text = "基数成本科目分类过滤一";
            oCell = flexGrid.Cell(0, 11);
            oCell.Text = "基数成本科目分类过滤二";
            oCell = flexGrid.Cell(0, 12);
            oCell.Text = "基数成本科目分类过滤三";
            oCell = flexGrid.Cell(0, 13);
            oCell.Text = "计价费率（转换成小数）";
            oCell = flexGrid.Cell(0, 14);
            oCell.Text = "定额耗用名称";
            oCell = flexGrid.Cell(0, 15);
            oCell.Text = "资源类型";
            oCell = flexGrid.Cell(0, 16);
            oCell.Text = "成本核算科目";
            oCell = flexGrid.Cell(0, 17);
            oCell.Text = "成本核算科目名称";
            oCell = flexGrid.Cell(0, 18);
            oCell.Text = "损耗";
            oCell = flexGrid.Cell(0, 19);
            oCell.Text = "定额单价";
            oCell = flexGrid.Cell(0, 20);
            oCell.Text = "定额工程量";
            oCell = flexGrid.Cell(0, 21);
            oCell.Text = "定额金额";
            oCell = flexGrid.Cell(0, 22);
            oCell.Text = "工程量计量单位";
            oCell = flexGrid.Cell(0, 23);
            oCell.Text = "价格计量单位";
            oCell = flexGrid.Cell(0, 24);
            oCell.Text = "分摊比率";

            for (int i = 1; i < 24; i++)
            {
                flexGrid.Column(i).Width = 150;
            }
        }

        //取消
        void btnCancel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //插入行
        void btnInsertRows_Click(object sender, EventArgs e)
        {
            int iRowCount = 0;
            try
            {
                iRowCount = int.Parse(txtRowCount.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.txtRowCount.Focus();
                return;
            }
            if (iRowCount <= 0)
            {
                MessageBox.Show("无法添加;[添加的行数应大于零]");
                this.txtRowCount.Focus();
                return;
            }
            //if (flexGrid.Rows == iMaxCount + 1)
            //{
            //    MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,已经添加了{0}条。]", iMaxCount));
            //    this.txtRowCount.Focus();
            //    return;
            //}
            //if (flexGrid.Rows + iRowCount > iMaxCount + 1)
            //{
            //    MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,此次只能添加{1}条记录。]", iMaxCount, iMaxCount + 1 - flexGrid.Rows));
            //    this.txtRowCount.Focus();
            //    return;
            //}
            flexGrid.AutoRedraw = false;
            for (int i = 0; i < iRowCount; i++)
            {
                flexGrid.InsertRow(flexGrid.Rows, 1);
                flexGrid.Rows += 1;
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        //删除
        void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRow(flexGrid.Selection.FirstRow);
        }
        public void DeleteRow(int iRow)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.RemoveItem(iRow);
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (importName.Equals("物料分类基础数据"))
            {

            }
            else if (importName.Equals("工程WBS任务类型"))
            {

            }
            else if (importName.Equals("基础数据"))
            {

            }
            else if (importName.Equals("成本核算科目"))
            {

            }
            else if (importName.Equals("成本项数据"))
            {
                ImportCostItem();
            }
            else if (importName.Equals("成本项分类数据"))
            {

            }
            else if (importName.Equals("会计科目"))
            {

            }
            else if (importName.Equals("成本项安装分类数据"))
            {

            }
        }
        //成本项导入
        void ImportCostItem()
        {
            #region 加载数据
            Hashtable hashtableCostItemCate = new Hashtable();//成本项分类
            string strCostCostItemCate = "select ID,CODE,SysCode,Name from THD_CostItemCategory";
            if (defaultCate != null)
            {
                strCostCostItemCate += " t1 where t1.syscode like '" + defaultCate.SysCode + "%'";
            }
            DataTable dtCostCostItemCate = model.SearchSql(strCostCostItemCate);
            for (int i = 0; i < dtCostCostItemCate.Rows.Count; i++)
            {
                CostItemCategory cate = new CostItemCategory();
                string strID = dtCostCostItemCate.Rows[i][0].ToString();
                string strCode = dtCostCostItemCate.Rows[i][1].ToString();
                string strSysCode = dtCostCostItemCate.Rows[i][2].ToString();
                string strName = dtCostCostItemCate.Rows[i][3].ToString();
                cate.Id = strID;
                cate.SysCode = strSysCode;
                cate.Name = strName;
                cate.Code = strCode;
                hashtableCostItemCate.Add(strID, cate);
            }

            Hashtable hashtableCostAccountSubject = new Hashtable();//成本核算科目
            string strCostAct = "select ID,CODE,Name,SysCode from thd_CostAccountSubject";
            DataTable dtCostAccountSubject = model.SearchSql(strCostAct);
            for (int k = 0; k < dtCostAccountSubject.Rows.Count; k++)
            {
                CostAccountSubject subject = new CostAccountSubject();
                string strCostAccountId = dtCostAccountSubject.Rows[k][0].ToString();
                string strCostAccountCode = dtCostAccountSubject.Rows[k][1].ToString();
                string strCostAccountName = dtCostAccountSubject.Rows[k][2].ToString();
                string strCostAccountSysCode = dtCostAccountSubject.Rows[k][3].ToString();
                subject.Id = strCostAccountId;
                subject.Code = strCostAccountCode;
                subject.Name = strCostAccountName;
                subject.SysCode = strCostAccountSysCode;
                hashtableCostAccountSubject.Add(strCostAccountId, subject);
            }

            Hashtable hashtableUnit = new Hashtable();//计量单位
            string strSearchUint = "select STANDUNITID,STANDUNITNAME,Version from RESSTANDUNIT";
            DataTable strUnitdt = model.SearchSql(strSearchUint);
            for (int k = 0; k < strUnitdt.Rows.Count; k++)
            {
                StandardUnit unit = new StandardUnit();
                string strUnitId = strUnitdt.Rows[k][0].ToString();
                string strUnitName = strUnitdt.Rows[k][1].ToString();
                unit.Version = ClientUtil.ToLong(strUnitdt.Rows[k][1].ToString());
                unit.Id = strUnitId;
                unit.Name = strUnitName;
                hashtableUnit.Add(strUnitId, unit);
            }

            List<CostItem> costList = new List<CostItem>();//成本项
            string strCost = "select ID,Code,TheCostItemCategory from THD_CostItem";
            if (defaultCate != null)
            {
                strCost += " t1 where t1.thecostitemcatesyscode like '" + defaultCate.SysCode + "%'";
            }
            DataTable dtCost = model.SearchSql(strCost);
            for (int i = 0; i < dtCost.Rows.Count; i++)
            {
                CostItem item = new CostItem();
                item.TheCostItemCategory = new CostItemCategory();
                item.Id = dtCost.Rows[i][0].ToString();
                item.Code = dtCost.Rows[i][1].ToString();
                item.TheCostItemCategory.Id = dtCost.Rows[i][2].ToString();
                costList.Add(item);
            }

            //管理模式
            IList managementModeList = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.CostItem_ManagementMode);
            #endregion

            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
            StringBuilder logMsg = new StringBuilder();

            IList listSaveCostItem = new ArrayList();//保存的成本项
            List<Material> listAllMat = new List<Material>();//存使用的所有资源类型
            FlexCell.Cell cell = null;
            CostItem cost = new CostItem();
            #region 导入成本项
            for (int i = 1; i < flexGrid.Rows; i++)
            {
                #region 读取数据
                //成本项分类代码	定额编号	成本项名称	成本项说明	适用级别	适用模式	内容类型	
                //基数成本项分类过滤（第一位不是0补0）	基数成本项分类过滤二（第一位不是0补0）	基数成本科目分类过滤一	基数成本科目分类过滤二	基数成本科目分类过滤三	
                //计价费率（转换成小数）	定额耗用名称	资源类型	成本核算科目	成本核算科目名称	损耗	
                //定额单价	定额工程量	定额金额	工程量计量单位	价格计量单位	分摊比率
                cell = flexGrid.Cell(i, 1);
                string cateCode = cell.Text.Trim();//成本项分类代码
                cell = flexGrid.Cell(i, 2);
                string quotaCode = cell.Text.Trim();//定额编号
                cell = flexGrid.Cell(i, 3);
                string costName = cell.Text;//成本项名称
                cell = flexGrid.Cell(i, 4);
                string costDesc = cell.Text;//成本项说明
                cell = flexGrid.Cell(i, 5);
                string applyLevel = cell.Text.Trim();//适用级别
                cell = flexGrid.Cell(i, 6);
                string applyMode = cell.Text.Trim();//适用模式
                cell = flexGrid.Cell(i, 7);
                string contentType = cell.Text;//内容类型
                cell = flexGrid.Cell(i, 8);
                string cateFilter1 = cell.Text.Trim();//基数成本项分类过滤（第一位不是0补0）
                cell = flexGrid.Cell(i, 9);
                string cateFilter2 = cell.Text.Trim();//基数成本项分类过滤二（第一位不是0补0）
                cell = flexGrid.Cell(i, 10);
                string subjectCateFilter1 = cell.Text.Trim(); //基数成本科目分类过滤一
                cell = flexGrid.Cell(i, 11);
                string subjectCateFilter2 = cell.Text.Trim(); //基数成本科目分类过滤二
                cell = flexGrid.Cell(i, 12);
                string subjectCateFilter3 = cell.Text.Trim(); //基数成本科目分类过滤三
                cell = flexGrid.Cell(i, 13);
                string pricingRate = cell.Text.Trim();//计价费率（转换成小数）
                cell = flexGrid.Cell(i, 14);
                string subjectCostQuotaName = cell.Text.Trim();//定额耗用名称
                cell = flexGrid.Cell(i, 15);
                string resourceType = cell.Text.Trim();//资源类型
                cell = flexGrid.Cell(i, 16);
                string costAccountSubject = cell.Text.Trim();//成本核算科目
                cell = flexGrid.Cell(i, 17);
                string costAccountSubjectName = cell.Text.Trim();//成本核算科目名称
                cell = flexGrid.Cell(i, 18);
                string loss = cell.Text.Trim();//损耗
                cell = flexGrid.Cell(i, 19);
                string quotaPrice = cell.Text.Trim();//定额单价
                cell = flexGrid.Cell(i, 20);
                string projectQuantity = cell.Text.Trim();//定额工程量
                cell = flexGrid.Cell(i, 21);
                string quotaMoney = cell.Text.Trim();//定额金额
                cell = flexGrid.Cell(i, 22);
                string projectUnit = cell.Text.Trim();//工程量计量单位
                cell = flexGrid.Cell(i, 23);
                string priceUnit = cell.Text.Trim();//价格计量单位
                cell = flexGrid.Cell(i, 24);
                string apportionRatio = cell.Text.Trim();//分摊比率
                #endregion
                if (costName.Equals("") && cost == null) continue;
                if (!costName.Equals(""))
                {
                    if (string.IsNullOrEmpty(cateCode))
                    {
                        logMsg.Append("第" + i + "行数据的成本项所属分类编码为空.");
                        logMsg.Append(Environment.NewLine);
                        cost = new CostItem();
                        continue;
                    }
                    cost = new CostItem();

                    cost.TheProjectGUID = projectInfo.Id;
                    cost.TheProjectName = projectInfo.Name;

                    cost.QuotaCode = quotaCode;
                    cost.Name = costName;
                    cost.CostDesc = costDesc;

                    #region 通过cateCode成本项分类代码查找成本项分类的ID、sysycode(成本项分类、基数成本项分类过滤)

                    if (cateCode.IndexOf("0") > 0 || cateCode.IndexOf("0") == -1)//分类第一位0丢失的情况
                        cateCode = "0" + cateCode;

                    if (!cateFilter1.Equals(""))
                    {
                        if (cateFilter1.IndexOf("0") > 0 || cateFilter1.IndexOf("0") == -1)//分类第一位0丢失的情况
                            cateFilter1 = "0" + cateFilter1;
                    }
                    if (!cateFilter2.Equals(""))
                    {
                        if (cateFilter2.IndexOf("0") > 0 || cateFilter2.IndexOf("0") == -1)//分类第一位0丢失的情况
                            cateFilter2 = "0" + cateFilter2;
                    }
                    bool cateCodeFlag = true;
                    bool cateFilter1Flag = true;
                    bool cateFilter2Flag = true;
                    foreach (System.Collections.DictionaryEntry c in hashtableCostItemCate)
                    {
                        CostItemCategory category = c.Value as CostItemCategory;
                        string strCateCode = category.Code;//c.Key.ToString();
                        if (strCateCode.Equals(cateCode) && cateCodeFlag)
                        {

                            cost.TheCostItemCategory = category;
                            cost.TheCostItemCateSyscode = category.SysCode;
                            cateCodeFlag = false;
                        }
                        if (!cateFilter1.Equals(""))
                        {
                            if (strCateCode.Equals(cateFilter1) && cateFilter1Flag)
                            {
                                //CostItemCategory category = c.Value as CostItemCategory;
                                cost.CateFilter1 = category;
                                cost.CateFilterName1 = category.Name;
                                cost.CateFilterSysCode1 = category.SysCode;
                                cateFilter1Flag = false;
                            }
                        }
                        if (!cateFilter2.Equals(""))
                        {
                            if (strCateCode.Equals(cateFilter2) && cateFilter2Flag)
                            {
                                //CostItemCategory category = c.Value as CostItemCategory;
                                cost.CateFilter2 = category;
                                cost.CateFilterName2 = category.Name;
                                cost.CateFilterSysCode2 = category.SysCode;
                                cateFilter2Flag = false;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(cateCode) && cateCodeFlag)
                    {
                        logMsg.Append("第" + i + "行数据的成本项分类编码不存在");
                        logMsg.Append(Environment.NewLine);
                        cost = new CostItem();
                        continue;
                    }
                    if (!string.IsNullOrEmpty(cateFilter1) && cateFilter1Flag)
                    {
                        logMsg.Append("第" + i + "行数据的基数成本项分类过滤一的成本项分类编码不存在");
                        logMsg.Append(Environment.NewLine);
                    }
                    if (!string.IsNullOrEmpty(cateFilter2) && cateFilter2Flag)
                    {
                        logMsg.Append("第" + i + "行数据的基数成本项分类过滤二的成本项分类编码不存在");
                        logMsg.Append(Environment.NewLine);
                    }
                    #endregion

                    #region 编号
                    int code = 1;
                    foreach (CostItem item in costList)
                    {
                        if (item.TheCostItemCategory.Id == cost.TheCostItemCategory.Id)
                        {
                            if (item != null && !string.IsNullOrEmpty(item.Code))
                            {
                                try
                                {
                                    if (item.Code.IndexOf("-") > -1)
                                    {
                                        int tempCode = Convert.ToInt32(item.Code.Substring(item.Code.LastIndexOf("-") + 1));
                                        if (tempCode >= code)
                                            code = tempCode + 1;
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    foreach (CostItem item in listSaveCostItem)
                    {
                        if (item.TheCostItemCategory.Id == cost.TheCostItemCategory.Id)
                        {
                            if (item != null && !string.IsNullOrEmpty(item.Code))
                            {
                                try
                                {
                                    if (item.Code.IndexOf("-") > -1)
                                    {
                                        int tempCode = Convert.ToInt32(item.Code.Substring(item.Code.LastIndexOf("-") + 1));
                                        if (tempCode >= code)
                                            code = tempCode + 1;
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    cost.Code = cost.TheCostItemCategory.Code + "-" + code.ToString().PadLeft(5, '0');
                    cost.ItemState = CostItemState.制定;
                    #endregion
                    cost.ApplyLevel = VirtualMachine.Component.Util.EnumUtil<CostItemApplyLeve>.FromDescription(applyLevel);
                    cost.ContentType = VirtualMachine.Component.Util.EnumUtil<CostItemContentType>.FromDescription(contentType);
                    //管理模式
                    foreach (BasicDataOptr bdo in managementModeList)
                    {
                        if (bdo.BasicName == applyMode)
                        {
                            cost.ManagementMode = bdo;
                            cost.ManagementModeName = applyMode;
                        }
                    }

                    #region 基数成本科目分类过滤
                    if (!subjectCateFilter1.Equals("") || !subjectCateFilter2.Equals("") || !subjectCateFilter3.Equals(""))
                    {
                        bool subjectCateFilter1Flag = true;
                        bool subjectCateFilter2Flag = true;
                        bool subjectCateFilter3Flag = true;
                        foreach (System.Collections.DictionaryEntry s in hashtableCostAccountSubject)
                        {
                            CostAccountSubject subject = s.Value as CostAccountSubject;
                            string strSubjectCateCode = subject.Code; //s.Key.ToString();
                            if (!subjectCateFilter1.Equals("") && subjectCateFilter1Flag)
                            {
                                if (strSubjectCateCode == subjectCateFilter1)
                                {
                                    cost.SubjectCateFilter1 = subject;
                                    cost.SubjectCateFilterName1 = subject.Name;
                                    cost.SubjectCateFilterSyscode1 = subject.SysCode;
                                    subjectCateFilter1Flag = false;
                                }
                            }
                            if (!subjectCateFilter2.Equals("") && subjectCateFilter2Flag)
                            {
                                if (strSubjectCateCode == subjectCateFilter2)
                                {
                                    //CostAccountSubject subject = s.Value as CostAccountSubject;
                                    cost.SubjectCateFilter2 = subject;
                                    cost.SubjectCateFilterName2 = subject.Name;
                                    cost.SubjectCateFilterSyscode2 = subject.SysCode;
                                    subjectCateFilter2Flag = false;
                                }
                            }
                            if (!subjectCateFilter3.Equals("") && subjectCateFilter3Flag)
                            {
                                if (strSubjectCateCode == subjectCateFilter3)
                                {
                                    //CostAccountSubject subject = s.Value as CostAccountSubject;
                                    cost.SubjectCateFilter3 = subject;
                                    cost.SubjectCateFilterName3 = subject.Name;
                                    cost.SubjectCateFilterSyscode3 = subject.SysCode;
                                    subjectCateFilter3Flag = false;
                                }
                            }
                        }

                        if (!subjectCateFilter1.Equals("") && subjectCateFilter1Flag)
                        {
                            logMsg.Append("第" + i + "行数据的基数成本科目分类过滤一的成本核算科目的编码不存在");
                            logMsg.Append(Environment.NewLine);
                        }
                        if (!subjectCateFilter2.Equals("") && subjectCateFilter2Flag)
                        {
                            logMsg.Append("第" + i + "行数据的基数成本科目分类过滤二的成本核算科目的编码不存在");
                            logMsg.Append(Environment.NewLine);
                        }
                        if (!subjectCateFilter3.Equals("") && subjectCateFilter3Flag)
                        {
                            logMsg.Append("第" + i + "行数据的基数成本科目分类过滤三的成本核算科目的编码不存在");
                            logMsg.Append(Environment.NewLine);
                        }
                    }
                    #endregion

                    if (pricingRate != "")
                    {
                        cost.PricingRate = ClientUtil.ToDecimal(pricingRate.Replace("%", "")) / 100;
                    }
                    #region 价格计量单位 工程量计量单位
                    if (!(priceUnit.Equals("") && projectUnit.Equals("")))
                    {
                        bool priceUnitFlag = true;
                        bool projectUnitFlag = true;
                        foreach (System.Collections.DictionaryEntry u in hashtableUnit)
                        {
                            StandardUnit unit = u.Value as StandardUnit;
                            string unitName = unit.Name;//u.Key.ToString();
                            if (!priceUnit.Equals("") && priceUnitFlag)
                            {
                                if (unitName == priceUnit)
                                {

                                    cost.PriceUnitGUID = unit;
                                    cost.PriceUnitName = unit.Name;
                                    priceUnitFlag = false;
                                }
                            }
                            if (!projectUnit.Equals("") && projectUnitFlag)
                            {
                                if (unitName == projectUnit)
                                {
                                    //StandardUnit unit = u.Value as StandardUnit;
                                    cost.ProjectUnitGUID = unit;
                                    cost.ProjectUnitName = unit.Name;
                                    projectUnitFlag = false;
                                }
                            }
                        }
                        if (!priceUnit.Equals("") && priceUnitFlag)
                        {
                            logMsg.Append("第" + i + "行数据的价格计量单位不存在");
                            logMsg.Append(Environment.NewLine);
                        }
                        if (!projectUnit.Equals("") && projectUnitFlag)
                        {
                            logMsg.Append("第" + i + "行数据的工程量计量单位不存在");
                            logMsg.Append(Environment.NewLine);
                        }
                    }
                    else
                    {
                        if (priceUnit.Equals(""))
                        {
                            logMsg.Append("第" + i + "行数据的价格计量单位为空");
                            logMsg.Append(Environment.NewLine);
                        }
                        if (projectUnit.Equals(""))
                        {
                            logMsg.Append("第" + i + "行数据的价格计量单位为空");
                            logMsg.Append(Environment.NewLine);
                        }
                    }
                    #endregion
                }
                #region 资源耗用定额
                if (resourceType == "" || costAccountSubject == "") continue;
                SubjectCostQuota quota = new SubjectCostQuota();
                quota.TheCostItem = cost;
                cost.ListQuotas.Add(quota);
                quota.Name = subjectCostQuotaName;
                quota.Wastage = ClientUtil.ToDecimal(loss);
                quota.QuotaPrice = ClientUtil.ToDecimal(quotaPrice);
                quota.QuotaProjectAmount = ClientUtil.ToDecimal(projectQuantity);
                quota.QuotaMoney = ClientUtil.ToDecimal(quotaMoney);
                quota.AssessmentRate = ClientUtil.ToDecimal(apportionRatio);
                //成本核算科目
                if (!string.IsNullOrEmpty(costAccountSubject))
                {
                    bool costAccountSubjectFlag = true;
                    foreach (System.Collections.DictionaryEntry s in hashtableCostAccountSubject)
                    {
                        CostAccountSubject subject = s.Value as CostAccountSubject;
                        if (subject.Code == costAccountSubject) //(s.Key.ToString() == costAccountSubject)
                        {
                            quota.CostAccountSubjectGUID = subject;
                            quota.CostAccountSubjectName = costAccountSubjectName;
                            costAccountSubjectFlag = false;
                            break;
                        }
                    }
                    if (costAccountSubjectFlag)
                    {
                        logMsg.Append("第" + i + "行数据的成本核算科目编码不存在");
                        logMsg.Append(Environment.NewLine);
                    }
                }
                #region  资源耗用定额 价格计量单位 工程量计量单位
                if (!(priceUnit.Equals("") && projectUnit.Equals("")))
                {
                    bool priceUnitFlag1 = true;
                    bool projectUnitFlag1 = true;
                    foreach (System.Collections.DictionaryEntry u in hashtableUnit)
                    {
                        StandardUnit unit = u.Value as StandardUnit;
                        string unitName = unit.Name;//u.Key.ToString();
                        if (!priceUnit.Equals("") && priceUnitFlag1)
                        {
                            if (unitName == priceUnit)
                            {
                                quota.PriceUnitGUID = unit;
                                quota.PriceUnitName = unit.Name;
                                priceUnitFlag1 = false;
                            }
                        }
                        if (!projectUnit.Equals("") && projectUnitFlag1)
                        {
                            if (unitName == projectUnit)
                            {
                                //StandardUnit unit = u.Value as StandardUnit;
                                quota.ProjectAmountUnitGUID = unit;
                                quota.ProjectAmountUnitName = unit.Name;
                                projectUnitFlag1 = false;
                            }
                        }
                    }
                    if (!priceUnit.Equals("") && priceUnitFlag1)
                    {
                        logMsg.Append("第" + i + "行数据的价格计量单位不存在");
                        logMsg.Append(Environment.NewLine);
                    }
                    if (!projectUnit.Equals("") && projectUnitFlag1)
                    {
                        logMsg.Append("第" + i + "行数据的工程量计量单位不存在");
                        logMsg.Append(Environment.NewLine);
                    }
                }
                else
                {
                    if (priceUnit.Equals(""))
                    {
                        logMsg.Append("第" + i + "行数据的价格计量单位为空");
                        logMsg.Append(Environment.NewLine);
                    }
                    if (projectUnit.Equals(""))
                    {
                        logMsg.Append("第" + i + "行数据的价格计量单位为空");
                        logMsg.Append(Environment.NewLine);
                    }
                }
                #endregion
                #region 资源组
                if (!resourceType.Equals(""))
                {
                    Material mat = new Material();
                    if (listAllMat != null && listAllMat.Count > 0)
                    {
                        var matQuery =
                            from m in listAllMat
                            where m.Code == resourceType
                            select m;
                        if (matQuery.Count() == 0)
                        {
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Code", resourceType));
                            IList listTemp = model.ObjectQuery(typeof(Material), oq);
                            if (listTemp.Count > 0)
                            {
                                mat = listTemp[0] as Material;
                                listAllMat.Add(mat);
                            }
                        }
                        else
                        {
                            mat = matQuery.ElementAt(0);
                        }
                    }
                    else
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Code", resourceType));
                        IList listTemp = model.ObjectQuery(typeof(Material), oq);
                        if (listTemp.Count > 0)
                        {
                            mat = listTemp[0] as Material;
                            listAllMat.Add(mat);
                        }
                    }
                    if (mat != null && mat.Id != null)
                    {
                        ResourceGroup resGroup = new ResourceGroup();
                        resGroup.TheCostQuota = quota;
                        quota.ListResources.Add(resGroup);

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
                    else
                    {
                        logMsg.Append("第" + i + "行数据的资源类型编码不存在");
                        logMsg.Append(Environment.NewLine);
                    }
                }
                #endregion
                #endregion
                if (!costName.Equals(""))
                {
                    listSaveCostItem.Add(cost);
                }
            }
            #endregion

            if (logMsg.Length > 0)
            {
                //写日志
                StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
                write.Write(logMsg.ToString());
                write.Close();
                write.Dispose();
            }

            //打开错误日志
            if (logMsg.Length > 0)
            {
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
                catch (System.ComponentModel.Win32Exception we)
                {
                }
            }
            else
            {
                if (listSaveCostItem != null && listSaveCostItem.Count > 0)
                {
                    model.Save(listSaveCostItem);
                }
                MessageBox.Show("数据导入完毕！");
                this.Close();
            }
        }
    }
}
