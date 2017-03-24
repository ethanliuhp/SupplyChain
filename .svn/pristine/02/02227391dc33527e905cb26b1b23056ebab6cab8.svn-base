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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public partial class VCostItemAdd : Form
    {
        private decimal personQuotaPrice;//人工费定额单价
        private BasicDataOptr manageMode;
        private CostItemApplyLeve usedLevel;
        private CostItemContentType contentType;
        private CurrentProjectInfo projectInfo;
        //public int iMaxCount = 10000;
        private bool isOrNotQuotaPrice = false;
        public MCostItem model = new MCostItem();
        private CostItemCategory cate;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">选择的成本项分类</param>
        /// <param name="price">人工定额单价</param>
        /// <param name="manage">管理模式</param>
        /// <param name="leve">使用级别</param>
        /// <param name="type">内容类型</param>
        /// <param name="type">是否定额基价为合价</param>
        public VCostItemAdd(CostItemCategory c, decimal price, BasicDataOptr manage, CostItemApplyLeve leve, CostItemContentType type, bool isQuotaPrice)
        {
            InitializeComponent();
            cate = c;
            personQuotaPrice = price;
            manageMode = manage;
            usedLevel = leve;
            contentType = type;
            isOrNotQuotaPrice = isQuotaPrice;
            InitEvent();
            InitData();
            lblText.Text = string.Format(">>[{1}]{0}", cate.Code, cate.Name);
        }

        public void InitEvent()
        {
            InitalFlexCell();
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
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
            flexGrid.Rows = 1;
            flexGrid.Cols = 11;
            //flexGrid.Column(0).Visible = false;
            //flexGrid.Column(1).Visible = false;

            //flexGrid.Column(1).Locked = true;
            FlexCell.Cell oCell = flexGrid.Cell(0, 1);
            oCell.Text = "序号";

            oCell = flexGrid.Cell(0, 2);
            oCell.Text = "定额号";

            oCell = flexGrid.Cell(0, 3);
            oCell.Text = "定额名称";

            oCell = flexGrid.Cell(0, 4);
            oCell.Text = "单位";

            oCell = flexGrid.Cell(0, 5);
            oCell.Text = "工程量";

            oCell = flexGrid.Cell(0, 6);
            oCell.Text = "定额基价";

            oCell = flexGrid.Cell(0, 7);
            oCell.Text = "定额合价";

            oCell = flexGrid.Cell(0, 8);
            oCell.Text = "定额所含数量";

            oCell = flexGrid.Cell(0, 9);
            oCell.Text = "分类编码";

            oCell = flexGrid.Cell(0, 10);
            oCell.Text = "物资编码";

            for (int i = 1; i < 11; i++)
            {
                if (i == 1 || i == 4)
                {
                    flexGrid.Column(i).Width = 60;
                }
                else if (i == 2 || i == 8)
                {
                    flexGrid.Column(i).Width = 80;
                }
                else if (i == 5 || i == 6 || i == 7 || i == 9 || i == 10)
                {
                    flexGrid.Column(i).Width = 100;
                }
                else
                {
                    flexGrid.Column(i).Width = 190;
                }
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
                if (txtRowCount.Text.Trim() == "")
                {
                    MessageBox.Show("无法添加;[添加的行数应大于零]");
                    this.txtRowCount.Focus();
                    return;
                }
                else
                {
                    iRowCount = int.Parse(txtRowCount.Text);
                }
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
        //添加行
        void btnAdd_Click(object sender, EventArgs e)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.InsertRow(flexGrid.Rows, 1);
            flexGrid.Rows += 1;
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
            //AddRow();
        }
        private void AddRow()
        {
            flexGrid.InsertRow(flexGrid.Rows, 1);
            flexGrid.Rows += 1;
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
            if (flexGrid.Rows <= 0)
            {
                MessageBox.Show("无保存数据");
                return;
            }
            try
            {
                List<StandardUnit> listAllUnit = new List<StandardUnit>();//存使用的所有计量单位

                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "errorLog.txt";
                StringBuilder logMsg = new StringBuilder();

                List<Material> listAllMat = new List<Material>();//存使用的所有资源类型

                List<CostItemCategory> listCostItemCategory = new List<CostItemCategory>();//存使用的所有成本项分类

                IList listSaveCostItem = new ArrayList();//保存的成本项

                CostItem optItem = null;
                IList listTemp = new ArrayList();

                ObjectQuery oq = new ObjectQuery();
                #region 把选择的成本项分类及其子节点全部加载出来
                oq.AddCriterion(Expression.Like("SysCode", cate.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("TheProjectGUID", cate.TheProjectGUID));
                listCostItemCategory = model.ObjectQuery(typeof(CostItemCategory), oq).OfType<CostItemCategory>().ToList<CostItemCategory>();
                oq.Criterions.Clear();
                #endregion
                #region 加载所有计量单位
                listAllUnit = model.ObjectQuery(typeof(StandardUnit), oq).OfType<StandardUnit>().ToList<StandardUnit>();
                oq.Criterions.Clear();
                #endregion

                StandardUnit priceUnit = null;
                StandardUnit personUnit = null;//人工单位
                StandardUnit materialUnit = null;//材料和机械单位
                int num1 = 0;
                int num2 = 0;
                int num3 = 0;
                foreach (StandardUnit nuit in listAllUnit)
                {
                    if (nuit.Name == "元")
                    {
                        if (num1 == 0)
                        {
                            priceUnit = nuit;
                            num1++;
                        }
                    }
                    else if (nuit.Name == "项")
                    {
                        if (num2 == 0)
                        {
                            materialUnit = nuit;
                            num2++;
                        }
                    }
                    else if (nuit.Name == "工日")
                    {
                        if (num3 == 0)
                        {
                            personUnit = nuit;
                            num3++;
                        }
                    }
                    if (num1 == 1 && num2 == 1 && num3 == 1)
                    {
                        break;
                    }
                }
                //oq.AddCriterion(Expression.Eq("Name", "元"));
                //IList listTemp = model.ObjectQuery(typeof(StandardUnit), oq);
                //if (listTemp.Count > 0)
                //{
                //    priceUnit = listTemp[0] as StandardUnit;
                //    listAllUnit.Add(priceUnit);
                //}
                //oq.Criterions.Clear();

                //oq.AddCriterion(Expression.Eq("Name", "项"));
                //listTemp = model.ObjectQuery(typeof(StandardUnit), oq);
                //if (listTemp.Count > 0)
                //{
                //    materialUnit = listTemp[0] as StandardUnit;
                //    listAllUnit.Add(materialUnit);
                //}
                //oq.Criterions.Clear();

                //oq.AddCriterion(Expression.Eq("Name", "工日"));
                //listTemp = model.ObjectQuery(typeof(StandardUnit), oq);
                //if (listTemp.Count > 0)
                //{
                //    personUnit = listTemp[0] as StandardUnit;
                //    listAllUnit.Add(personUnit);
                //}
                //oq.Criterions.Clear();


                Material personMat = null;//人工资源类型
                Material materialMat = null;//材料资源类型
                Material machineMat = null;//机械资源类型
                Material managerMat = null;//综合费
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

                oq.AddCriterion(Expression.Eq("Code", "R30100000"));
                listTemp = model.ObjectQuery(typeof(Material), oq);
                if (listTemp.Count > 0)
                {
                    managerMat = listTemp[0] as Material;
                    listAllMat.Add(managerMat);
                }
                oq.Criterions.Clear();

                CostAccountSubject personSubject = null;//人工核算科目
                CostAccountSubject materialSubject = null;//材料核算科目
                CostAccountSubject machineSubject = null;//机械核算科目
                CostAccountSubject masterMatSubject = null;//主材核算科目
                CostAccountSubject managerSubject = null;//管理

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

                oq.AddCriterion(Expression.Eq("Code", "C513"));
                listTemp = model.ObjectQuery(typeof(CostAccountSubject), oq);
                if (listTemp.Count > 0)
                {
                    managerSubject = listTemp[0] as CostAccountSubject;
                }
                oq.Criterions.Clear();
                //decimal personQuotaPrice = Convert.ToDecimal(txtPersonQuotaPrice.Text);//人工费定额单价
                //BasicDataOptr manageMode = cbManagemode.SelectedItem as BasicDataOptr;
                //CostItemApplyLeve usedLevel = VirtualMachine.Component.Util.EnumUtil<CostItemApplyLeve>.FromDescription(cbUsedLevel.Text.Trim());
                //CostItemContentType contentType = VirtualMachine.Component.Util.EnumUtil<CostItemContentType>.FromDescription(cbContentType.Text.Trim());

                Dictionary<string, int> dicCateMaxCostItemCode = new Dictionary<string, int>();

                #region 导入数据

                FlexCell.Cell cell = null;
                for (int i = 1; i < flexGrid.Rows; i++)
                {

                    cell = flexGrid.Cell(i, 1);
                    string orderNo = cell.Text.Trim();//序号
                    cell = flexGrid.Cell(i, 2);
                    string quotaCode = cell.Text.Trim();//定额号
                    cell = flexGrid.Cell(i, 3);
                    string quotaName = cell.Text;//定额名称
                    cell = flexGrid.Cell(i, 8);
                    string quotaRate = cell.Text.Trim();//定额所含数量
                    cell = flexGrid.Cell(i, 4);
                    string projectUnitName = cell.Text.Trim();//单位
                    cell = flexGrid.Cell(i, 5);
                    string projectQuantity = cell.Text.Trim();//工程量
                    cell = flexGrid.Cell(i, 6);
                    string quotaPrice = cell.Text.Trim();//定额基价
                    cell = flexGrid.Cell(i, 9);
                    string cateCode = cell.Text.Trim();//分类编码
                    cell = flexGrid.Cell(i, 10);
                    string materialCode = cell.Text.Trim();//物资编码

                    if (!string.IsNullOrEmpty(orderNo) && string.IsNullOrEmpty(cateCode))//成本项
                    {
                        logMsg.Append("第" + i + "行数据的成本项所属分类编码为空.");
                        logMsg.Append(Environment.NewLine);

                        optItem = null;

                        continue;
                    }

                    if (quotaCode != "人工" && quotaCode != "材料" && quotaCode != "机械" && quotaCode != "综合费")
                    {
                        #region 计量单位判断
                        if (projectUnitName.IndexOf(quotaRate) == 0 && quotaRate != "")
                        {
                            projectUnitName = projectUnitName.Replace(quotaRate, "");//去点单位前的系数
                        }

                        var query = from u in listAllUnit where u.Name == projectUnitName select u;
                        if (query.Count() == 0)
                        {

                            if (string.IsNullOrEmpty(projectUnitName))
                            {
                                logMsg.Append("第" + i + "行数据的计量单位为空.");
                                logMsg.Append(Environment.NewLine);
                            }
                            else
                            {
                                logMsg.Append("第" + i + "行数据的工程量单位“" + projectUnitName + "”未找到.");
                                logMsg.Append(Environment.NewLine);
                            }
                        }
                        #endregion
                    }

                    StandardUnit projectUnit = null;

                    var queryUnit = from u in listAllUnit where u.Name == projectUnitName select u;

                    if (queryUnit.Count() > 0)
                        projectUnit = queryUnit.ElementAt(0);

                    if (!string.IsNullOrEmpty(orderNo) && !string.IsNullOrEmpty(cateCode))
                    {
                        #region 成本项
                        if (cateCode.IndexOf("0") == -1 || cateCode.IndexOf("0") > 0)
                            cateCode = "0" + cateCode;

                        //确定成本项分类
                        if (listCostItemCategory.Count == 0 || (from m in listCostItemCategory where m.Code == cateCode select m).Count() == 0)
                        {
                            //oq.Criterions.Clear();
                            //oq.FetchModes.Clear();

                            //oq.AddCriterion(Expression.Eq("Code", cateCode));
                            //oq.AddCriterion(Expression.Like("SysCode", cate.SysCode, MatchMode.Start));
                            //IList list = model.ObjectQuery(typeof(CostItemCategory), oq);

                            //if (list.Count == 0)
                            //{
                            logMsg.Append("第" + i + "行数据的成本项(定额编号“" + quotaCode + "”,成本项名称“" + quotaName + "”)所属分类“" + cateCode + "”未找到,该成本项及下属定额数据导入失败.");
                            logMsg.Append(Environment.NewLine);

                            optItem = null;

                            continue;
                            //}

                            //listCostItemCategory.Add(list[0] as CostItemCategory);
                        }

                        CostItemCategory optCate = (from m in listCostItemCategory where m.Code == cateCode select m).ElementAt(0);


                        optItem = new CostItem();

                        optItem.TheCostItemCategory = optCate;
                        optItem.TheCostItemCateSyscode = optCate.SysCode;
                        //optItem.TheProjectGUID = pro

                        int costItemCode = 0;
                        if (dicCateMaxCostItemCode.Count == 0)
                        {
                            costItemCode = model.GetMaxCostItemCodeByCate(optCate.Id) + 1;

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
                            costItemCode = model.GetMaxCostItemCodeByCate(optCate.Id) + 1;
                            dicCateMaxCostItemCode.Add(optCate.Id, costItemCode);
                        }

                        optItem.Code = optCate.Code + "-" + costItemCode.ToString().PadLeft(5, '0');
                        optItem.Name = quotaName;
                        optItem.QuotaCode = quotaCode;

                        optItem.ItemState = CostItemState.制定;

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
                        #endregion
                    }
                    else
                    {
                        if (optItem == null)
                            continue;

                        List<SubjectCostQuota> listQuota = new List<SubjectCostQuota>();

                        #region 构造成本项的定额集
                        for (int z = i; z < flexGrid.Rows; z++)
                        {
                            cell = flexGrid.Cell(z, 1);
                            orderNo = cell.Text.Trim();//序号
                            cell = flexGrid.Cell(z, 2);
                            quotaCode = cell.Text.Trim();//定额号
                            cell = flexGrid.Cell(z, 3);
                            quotaName = cell.Text;//定额名称
                            cell = flexGrid.Cell(i - 1, 8);
                            quotaRate = cell.Text.Trim();//定额所含数量
                            cell = flexGrid.Cell(z, 4);
                            projectUnitName = cell.Text.Trim();//单位
                            cell = flexGrid.Cell(z, 5);
                            projectQuantity = cell.Text.Trim();//工程量
                            cell = flexGrid.Cell(z, 6);
                            quotaPrice = cell.Text.Trim();//定额基价
                            cell = flexGrid.Cell(z, 9);
                            cateCode = cell.Text.Trim();//分类编码
                            cell = flexGrid.Cell(z, 10);
                            materialCode = cell.Text.Trim();//物资编码
                            //cateCode = dataRow1["分类编码"].ToString().Trim();//定额不使用，用来判断下一个成本项
                            //orderNo = dataRow1["序号"].ToString().Trim();//定额不使用，用来判断下一个成本项

                            //materialCode = dataRow1["物资编码"].ToString().Trim();

                            //quotaCode = dataRow1["定额号"].ToString().Trim();
                            //quotaName = dataRow1["定额名称"].ToString().Trim();
                            //quotaRate = dt.Rows[j - 1]["定额所含数量"].ToString().Trim();//使用成本项的定额所含数量
                            //projectUnitName = dataRow1["单位"].ToString().Trim();
                            //projectQuantity = dataRow1["工程量"].ToString().Trim();
                            //quotaPrice = dataRow1["定额基价"].ToString().Trim();


                            quotaRate = string.IsNullOrEmpty(quotaRate) ? "1" : quotaRate;

                            projectQuantity = string.IsNullOrEmpty(projectQuantity) ? "0" : projectQuantity;
                            quotaPrice = string.IsNullOrEmpty(quotaPrice) ? "0" : quotaPrice;

                            if (!string.IsNullOrEmpty(orderNo) && !string.IsNullOrEmpty(cateCode))//下一个成本项跳出
                            {
                                i = z - 1;
                                break;
                            }
                            if (z == flexGrid.Rows - 1)
                            {
                                i = z;
                            }

                            if (quotaCode != "人工" && quotaCode != "材料" && quotaCode != "机械" && quotaCode != "综合费" && string.IsNullOrEmpty(quotaName))
                            {
                                logMsg.Append("第" + i + "行数据的定额名称为空");
                                logMsg.Append(Environment.NewLine);
                                continue;
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
                                        logMsg.Append("第" + i + "行数据的耗用定额号“" + quotaCode + "”、定额名称“" + quotaName + "”的资源编码“" + materialCode + "”未找到指定资源对象.");
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
                            quota.State = SubjectCostQuotaState.编制;
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

                            quota.QuotaPrice = Convert.ToDecimal(quotaPrice) / Convert.ToDecimal(quotaRate);//临时存储
                            quota.QuotaProjectAmount = Convert.ToDecimal(projectQuantity);//临时存储
                            quota.QuotaMoney = quota.QuotaProjectAmount * quota.QuotaPrice;//临时存储
                            if (quotaCode == "人工" && isOrNotQuotaPrice)
                            {
                                quota.QuotaMoney = quota.QuotaPrice;
                            }

                            if (quotaCode != "人工" && quotaCode != "材料" && quotaCode != "机械" && quotaCode != "综合费" && quota.QuotaPrice == 0)//为主材时且定额单价为0
                            {
                                quota.QuotaProjectAmount = Convert.ToDecimal(projectQuantity) / Convert.ToDecimal(quotaRate);//临时存储
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
                            else if (quotaCode.Trim() == "综合费")
                            {
                                optSubject = managerSubject;
                            }
                            else
                                optSubject = masterMatSubject;

                            quota.CostAccountSubjectGUID = optSubject;
                            if (optSubject != null)
                                quota.CostAccountSubjectName = optSubject.Name;

                            listQuota.Add(quota);
                        }
                        #endregion

                        #region 更新成本项
                        if (listQuota.Count > 0)
                        {
                            IEnumerable<SubjectCostQuota> queryResType = from q in listQuota
                                                                         where q.Code == "人工" || q.Code == "材料" || q.Code == "机械" || q.Code == "综合费"
                                                                         select q;

                            var queryResTypeGroup = from q in queryResType
                                                    group q by new { resType = q.Code } into g
                                                    select new { g.Key.resType };

                            #region 将 人工、材料、机械、综合费的耗用定额分组生成一个耗用定额
                            foreach (var obj in queryResTypeGroup)
                            {
                                SubjectCostQuota optQuota = new SubjectCostQuota();
                                optQuota.TheCostItem = optItem;
                                optItem.ListQuotas.Add(optQuota);

                                //设置基本属性
                                optQuota.State = SubjectCostQuotaState.编制;

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
                                else if (obj.resType == "综合费")//单位为“项”
                                {
                                    optQuota.ProjectAmountUnitGUID = materialUnit;
                                    optQuota.ProjectAmountUnitName = materialUnit.Name;

                                    optQuota.Name = "综合费";

                                    optQuota.CostAccountSubjectGUID = managerSubject;
                                    if (managerSubject != null)
                                        optQuota.CostAccountSubjectName = managerSubject.Name;
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
                                else if (obj.resType == "机械")
                                    mat = machineMat;
                                else
                                    mat = managerMat;

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

                            #region 将除 人工、材料、机械 综合 费外的的耗用定额每个生成一个耗用定额
                            int count = 0;
                            for (int k = 0; k < listQuota.Count; k++)
                            {
                                SubjectCostQuota quota = listQuota[k];

                                if (quota.Code != "人工" && quota.Code != "材料" && quota.Code != "机械" && quota.Code != "综合费")
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
                        #endregion

                        decimal projectQnyPrice = 0;//成本项单价取下属工程量单位之和
                        foreach (SubjectCostQuota quota in optItem.ListQuotas)
                        {
                            projectQnyPrice += quota.QuotaMoney;
                        }

                        optItem.Price = decimal.Round(projectQnyPrice, 5);
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

                    //logMsg = new StringBuilder();
                }
                else
                {
                    //写日志
                    StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
                    write.WriteLine("");
                    write.WriteLine("");
                    write.WriteLine("");
                    write.WriteLine("共导入成本项：" + listSaveCostItem.Count + "项。");
                    write.Close();
                    write.Dispose();

                    //listSaveCostItem.Clear();

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
                    if (listSaveCostItem.Count > 0)
                    {
                        //保存数据
                        //model.SaveOrUpdateCostItem(listSaveCostItem);
                        model.Save(listSaveCostItem);
                    }
                    MessageBox.Show("数据导入完毕！");

                    this.Close();
                }
                //System.Threading.Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {

            }
        }
    }
}
