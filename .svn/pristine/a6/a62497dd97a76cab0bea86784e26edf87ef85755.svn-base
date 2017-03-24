using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.MaterialResource.Domain;
using FlexCell;
using Iesi.Collections;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VImportGWBSDetail_New : Form
    {
        /// <summary>
        /// 当前选中的节点
        /// </summary>
        public TreeNode DefaultGWBSTreeNode = null;
        public ContractGroup contract = null;

        /// <summary>
        /// 当前节点对应的CWBSTree实体
        /// </summary>
        private GWBSTree OptGWBSTreeObj = null;
        private Hashtable htResult = null;
        private int iMaxCount = 500;
        private int RowCount = 0;
        private MGWBSTree model = new MGWBSTree();
        /// <summary>
        /// 区域
        /// </summary>
        private IList zoningList = new ArrayList();
        public VImportGWBSDetail_New(int RowCount)
        {
            InitializeComponent();

            htResult = new Hashtable();
            this.RowCount = RowCount;
            InitalFlexCell();
            IntialEvent();
        }

        public void InitalFlexCell()
        {
            flexGrid.Rows = RowCount + 1;
            flexGrid.Cols = 12;
            flexGrid.DisplayRowNumber = true;

            flexGrid.Range(1, 1, 2, 1).Merge();
            flexGrid.Cell(1, 1).Text = "成本项编码";

            flexGrid.Range(1, 2, 2, 2).Merge();
            flexGrid.Cell(1, 2).Text = "成本项名称";

            flexGrid.Range(1, 3, 1, 5).Merge();
            flexGrid.Cell(1, 3).Text = "合同收入";
            flexGrid.Cell(2, 3).Text = "其中人工单价";
            flexGrid.Cell(2, 4).Text = "其中材料单价";
            flexGrid.Cell(2, 5).Text = "数量";

            flexGrid.Range(1, 6, 1, 8).Merge();
            flexGrid.Cell(1, 6).Text = "责任成本";
            flexGrid.Cell(2, 6).Text = "其中人工单价";
            flexGrid.Cell(2, 7).Text = "其中材料单价";
            flexGrid.Cell(2, 8).Text = "数量";

            flexGrid.Range(1, 9, 1, 11).Merge();
            flexGrid.Cell(1, 9).Text = "计划成本";
            flexGrid.Cell(2, 9).Text = "其中人工单价";
            flexGrid.Cell(2, 10).Text = "其中材料单价";
            flexGrid.Cell(2, 11).Text = "数量";

            for (int i = 1; i < 12; i++)
            {
                flexGrid.Column(i).AutoFit();
                flexGrid.Column(i).Alignment = AlignmentEnum.CenterCenter;
            }
        }

        public void IntialEvent()
        {
            this.Load += new EventHandler(VImportGWBSDetail_New_Load);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnInsertRows.Click += new EventHandler(btnInsertRows_Click);
            btnSure.Click += new EventHandler(btnSure_Click);
            btnCancel1.Click += new EventHandler(btnCancel);
        }

        public void VImportGWBSDetail_New_Load(object sender, EventArgs e)
        {
            if (DefaultGWBSTreeNode != null)
            {
                OptGWBSTreeObj = DefaultGWBSTreeNode.Tag as GWBSTree;
            }

            ValidContract();
        }

        /// <summary>
        /// 有效
        /// </summary>
        /// <returns>true:有效【已选】，false：无效【未选】</returns>
        private bool ValidContract()
        {
            if (contract == null)
            {
                MessageBox.Show("请选择驱动契约组！");

                VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    contract = frm.SelectResult[0] as ContractGroup;
                    return true;
                }
                return false;
            }
            return true;
        }

        public void btnDelete_Click(object sender, System.EventArgs e)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.RemoveItem(flexGrid.Selection.FirstRow);

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        public void btnAdd_Click(object sender, System.EventArgs e)
        {
            AddNewRow();
        }

        public void btnInsertRows_Click(object sender, System.EventArgs e)
        {
            int iRowCount = 0;

            if (!int.TryParse(txtRowCount.Text, out iRowCount))
            {
                MessageBox.Show("无法添加，[添加的行数需为大于零的整数]");
                this.txtRowCount.Focus();
                return;
            }

            if (iRowCount <= 0)
            {
                MessageBox.Show("无法添加;[添加的行数大于零]");
                this.txtRowCount.Focus();
                return;
            }
            if (flexGrid.Rows == iMaxCount + 1)
            {
                MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,已经添加了{0}条。]", iMaxCount));
                this.txtRowCount.Focus();
                return;
            }
            if (flexGrid.Rows + iRowCount > iMaxCount + 1)
            {
                MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,此次只能添加{1}条记录。]", iMaxCount, iMaxCount + 1 - flexGrid.Rows));
                this.txtRowCount.Focus();
                return;
            }

            AddNewRow(iRowCount);
        }

        public void AddNewRow()
        {
            flexGrid.AutoRedraw = false;

            flexGrid.InsertRow(flexGrid.Rows, 1);
            flexGrid.Rows += 1;
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        public void AddNewRow(int count)
        {
            flexGrid.AutoRedraw = false;

            flexGrid.InsertRow(flexGrid.Rows, count);
            flexGrid.Rows += 1;
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }


        public void btnSure_Click(object sender, System.EventArgs e)
        {
            //工程任务明细GWBSDetail
            //耗用明细GWBSDetailCostSubject 

            FlashScreen.Show("正在保存数据,请稍候......");
            string errMsg = "";
            try
            {
                if (!ValidContract())
                {
                    MessageBox.Show("请选择驱动契约组！");
                    return;
                };

                #region 验证  成本明细列表   成本项数据获取
                //成本项分类集合
                List<CostItemCategory> listCategory = new List<CostItemCategory>();
                //成本项集合
                List<CostItem> listCostItem = new List<CostItem>();
                //成本项编号
                List<string> quotaCodeList = new List<string>();

                ObjectQuery oqCate = new ObjectQuery();
                ObjectQuery oqCostItem = new ObjectQuery();
                Disjunction disCate = new Disjunction();
                Disjunction disCostItem = new Disjunction();

                //查询并校验基础数据
                for (int i = 3; i < flexGrid.Rows; i++)
                {
                    string quotaCode = flexGrid.Cell(i, 1).Text;

                    //if (!string.IsNullOrEmpty(costItemCateCode))
                    //{
                    //    disCate.Add(Expression.Eq("Code", costItemCateCode));
                    //}

                    if (string.IsNullOrEmpty(quotaCode))
                    {
                        errMsg += "第" + i + "行定额编号为空" + Environment.NewLine;
                    }
                    else
                    {
                        quotaCode = quotaCode.Trim().ToUpper();
                        disCostItem.Add(Expression.Eq("QuotaCode", quotaCode));
                        quotaCodeList.Add(quotaCode);
                    }

                    int count1 = 0;

                    if (!string.IsNullOrEmpty(flexGrid.Cell(i, 3).Text))
                    {
                        count1++;
                    }
                    if (!string.IsNullOrEmpty(flexGrid.Cell(i, 4).Text))
                    {
                        count1++;
                    }

                    int count2 = 0;
                    if (!string.IsNullOrEmpty(flexGrid.Cell(i, 6).Text))
                    {
                        count2++;
                    }
                    if (!string.IsNullOrEmpty(flexGrid.Cell(i, 7).Text))
                    {
                        count2++;
                    }

                    int count3 = 0;
                    if (!string.IsNullOrEmpty(flexGrid.Cell(i, 9).Text))
                    {
                        count3++;
                    }
                    if (!string.IsNullOrEmpty(flexGrid.Cell(i, 10).Text))
                    {
                        count3++;
                    }

                    if (count1 != count2 || count1 != count3)
                    {
                        errMsg += "第" + i + "行“合同收入”、“责任成本”、“计划成本”下的“其中人工单价”与“其中材料单价”填写不一致" + Environment.NewLine;
                    }
                }

                if (errMsg == "")
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    Disjunction dis1 = new Disjunction();
                    //层次码
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
                else
                {
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

                #endregion

                //获取工程WBS树
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", OptGWBSTreeObj.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ProjectTaskTypeGUID", NHibernate.FetchMode.Eager);
                OptGWBSTreeObj = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;

                int maxOrderNo = 1;

                int code = OptGWBSTreeObj.Details.Count + 1;

                #region 获取父对象下最大明细号
                foreach (GWBSDetail dtl in OptGWBSTreeObj.Details)
                {
                    if (dtl.OrderNo >= maxOrderNo)
                    {
                        maxOrderNo = dtl.OrderNo;
                    }

                    if (!string.IsNullOrEmpty(dtl.Code) && dtl.Code.IndexOf("-") > -1)
                    {
                        int tempCode = 0;
                        if (Int32.TryParse(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1), out tempCode))
                        {
                            if (tempCode >= code)
                            {
                                code = tempCode + 1;
                            }
                        }
                    }
                }
                #endregion

                DateTime serverTime = model.GetServerTime();

                IList listSaveDtl = new ArrayList();

                #region 循环遍历表格处理数据
                //排除表头【2行】，数据内容从第三行开始
                for (int i = 3; i < flexGrid.Rows; i++)
                {
                    GWBSDetail oprDtl = new GWBSDetail();
                    oprDtl.Code = flexGrid.Cell(i, 1).Text.Trim();
                    oprDtl.Name = flexGrid.Cell(i, 2).Text.Trim();
                    //合同收入
                    oprDtl.ContractProjectQuantity = ClientUtil.ToDecimal(flexGrid.Cell(i, 5).Text.Trim());

                    //责任成本
                    if (cbResponseAccountFlag.Checked)
                    {
                        oprDtl.ResponseFlag = 1;
                        oprDtl.ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(flexGrid.Cell(i, 8).Text.Trim());
                    }

                    //计划成本         
                    if (cbCostAccountFlag.Checked)
                    {
                        oprDtl.CostingFlag = 1;
                        oprDtl.PlanWorkAmount = ClientUtil.ToDecimal(flexGrid.Cell(i, 11).Text.Trim());
                    }

                    oprDtl.TheGWBS = OptGWBSTreeObj;
                    oprDtl.TheGWBSSysCode = OptGWBSTreeObj.SysCode;

                    if (OptGWBSTreeObj.ProjectTaskTypeGUID != null)
                    {
                        oprDtl.ProjectTaskTypeCode = OptGWBSTreeObj.ProjectTaskTypeGUID.Code;
                    }
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

                    #region 获取成本项
                    string quotaCode = flexGrid.Cell(i, 1).Text;
                    quotaCode = string.IsNullOrEmpty(quotaCode) ? "" : quotaCode.ToUpper();

                    var queryQuota = from c in listCostItem
                                     where c.QuotaCode == quotaCode
                                     select c;

                    CostItem currItem = null;

                    if (queryQuota.Count() == 0)
                    {
                        errMsg += "第" + i + "行根据定额编号“" + quotaCode + "”未找到相关成本项" + Environment.NewLine;
                        continue;
                    }
                    else if (queryQuota.Count() > 1)
                    {
                        //if (string.IsNullOrEmpty(costItemCateCode))
                        //{
                        //    errMsg += "第" + i + "行根据定额编号“" + quotaCode + "”找到" + queryQuota.Count() + "个成本项" + Environment.NewLine;
                        //    continue;
                        //}

                        //var queryCate = from c in listCategory
                        //                where c.Code == costItemCateCode
                        //                select c;

                        //if (queryCate.Count() == 0)
                        //{
                        //    errMsg += "第" + i + "行根据指定成本项分类编码“" + costItemCateCode + "”未找到相关成本项分类" + Environment.NewLine;
                        //    continue;
                        //}

                        // CostItemCategory cate = queryCate.ElementAt(0);

                        queryQuota = from c in listCostItem
                                     where c.QuotaCode == quotaCode// && c.TheCostItemCateSyscode.IndexOf(cate.SysCode) > -1
                                     select c;

                        if (queryQuota.Count() == 0)
                        {
                            errMsg += "第" + i + "行根据定额编号“" + quotaCode + /*"”在指定分类编码“" + costItemCateCode +*/ "”未找到相关成本项" + Environment.NewLine;
                            continue;
                        }

                        currItem = queryQuota.ElementAt(0);
                    }
                    else
                    {
                        currItem = queryQuota.ElementAt(0);
                    }
                    if (errMsg != "")
                    {
                        return;
                    }
                    #endregion

                    oprDtl.TheCostItem = currItem;
                    oprDtl.TheCostItemCateSyscode = currItem.TheCostItemCateSyscode;

                    #region 通过成本项获取资源耗用
                    CostItem item = oprDtl.TheCostItem;
                    if (item != null)
                    {
                        ObjectQuery oqQuota = new ObjectQuery();
                        oqQuota.AddCriterion(Expression.Eq("TheCostItem.Id", item.Id));
                        oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                        oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                        oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                        oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

                        IList listQuota = model.ObjectQuery(typeof(SubjectCostQuota), oqQuota);
                        /**
                        * 注：主资源标志【是：材料，否：人工】
                        * 1，用户填写的资源数和关联到的资源数一致，且都为两条时，【其中材料单价】写入主资源，【其中人工单价】写入另外一个资源
                        * 2，【其中材料单价】【其中人工单价】用户只填写了一项且关联到的资源数也为一项时，直接将相应的数据更新到资源上
                        * 3，关联到有两条以上或是多个是/否则不符合要求不导入等其他情况，提示报错误信息给用户，不予以导入
                        * **/
                        #region 资源数一致性验证
                        bool isOnly = false;//是否只有一项
                        if (listQuota.Count == 0)
                        {
                            errMsg += "第" + i + "行根据定额编号“" + quotaCode + "”未找到相关资源耗用" + Environment.NewLine;
                            break;
                        }
                        else if (listQuota.Count > 2)
                        {
                            errMsg += "第" + i + "行根据定额编号“" + quotaCode + "”找到" + listQuota.Count + "个相关资源耗用" + Environment.NewLine;
                            break;
                        }
                        else
                        {
                            int count1 = 0;

                            if (!string.IsNullOrEmpty(flexGrid.Cell(i, 3).Text))
                            {
                                count1++;
                            }
                            if (!string.IsNullOrEmpty(flexGrid.Cell(i, 4).Text))
                            {
                                count1++;
                            }

                            if (count1 != listQuota.Count)
                            {
                                errMsg += "第" + i + "行填写的数据与根据定额编号“" + quotaCode + "”找到相关资源耗用数量不符" + Environment.NewLine;
                                break;
                            }

                            isOnly = listQuota.Count == 1;
                        }
                        #endregion

                        #region 加载资源耗用明细
                        ResourceGroup mainResource = null;

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
                                {
                                    mainResource = quota.ListResources.ElementAt(0);
                                }
                            }
                            var query = from q in oprDtl.ListCostSubjectDetails
                                        where q.ResourceUsageQuota.Id == quota.Id
                                        select q;

                            if (query.Count() > 0)
                            {
                                continue;
                            }

                            AddResourceUsageInTaskDetail(oprDtl, quota, i, isOnly);
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

                        #endregion
                    }
                    #endregion

                    listSaveDtl.Add(oprDtl);

                    code += 1;
                    maxOrderNo += 1;
                }
                #endregion

                #region 保存

                if (listSaveDtl.Count > 0)
                {
                    //根据明细设置任务对象的标记
                    bool taskResponsibleFlag = false;
                    bool taskCostAccFlag = false;

                    foreach (GWBSDetail dtl in OptGWBSTreeObj.Details)
                    {
                        if (dtl.ResponseFlag == 1)
                            taskResponsibleFlag = true;
                        if (dtl.CostingFlag == 1)
                            taskCostAccFlag = true;
                    }

                    #region 计算任务明细的量价

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
                    }
                    #endregion

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
                    FlashScreen.Close();
                    MessageBox.Show("保存成功！");
                    this.Close();
                }
                #endregion
            }
            finally
            {
                FlashScreen.Close();
                if (errMsg != "")
                {
                    WriteLog(errMsg);
                }
            }
        }

        public void btnCancel(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public bool IsEmpty(int iRow)
        {
            bool bFlag = true;
            if (flexGrid.Cols > 1)
            {
                for (int i = 1; i < flexGrid.Cols; i++)
                {
                    if (!string.IsNullOrEmpty(flexGrid.Cell(iRow, i).Text.Trim()))
                    {
                        bFlag = false;
                        break;
                    }
                }
            }

            return bFlag;
        }

        private void AddResourceUsageInTaskDetail(GWBSDetail oprDtl, SubjectCostQuota quota, int rowIndex, bool isOnly)
        {
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = quota.Name;
            subject.MainResTypeFlag = quota.MainResourceFlag;

            subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

            subject.PriceUnitGUID = quota.PriceUnitGUID;
            subject.PriceUnitName = quota.PriceUnitName;
            //如果是表格里面只填写了一项，同时关联到的资源也只有一项，直接赋值【无需区分人工，材料】
            if (isOnly)
            {
                subject.ContractQuantityPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 4).Text.Trim());

                subject.ResponsibilitilyPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 7).Text.Trim());

                subject.PlanPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 10).Text.Trim());
            }
            else if (quota.MainResourceFlag)//如果是主资源，即材料
            {
                subject.ContractQuantityPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 4).Text.Trim());

                subject.ResponsibilitilyPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 7).Text.Trim());

                subject.PlanPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 10).Text.Trim());
            }
            else//人工
            {
                subject.ContractQuantityPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 3).Text.Trim());

                subject.ResponsibilitilyPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 6).Text.Trim());

                subject.PlanPrice = ClientUtil.ToDecimal(flexGrid.Cell(rowIndex, 9).Text.Trim());
            }

            //合同
            subject.ContractQuotaQuantity = quota.QuotaProjectAmount;
            //subject.ContractQuantityPrice = quota.QuotaPrice;
            subject.ContractPricePercent = 1;
            subject.ContractBasePrice = subject.ContractQuantityPrice;
            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = oprDtl.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = quota.QuotaProjectAmount;
            //subject.ResponsibilitilyPrice = quota.QuotaPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibleBasePrice = subject.ResponsibilitilyPrice;
            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = oprDtl.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = quota.QuotaProjectAmount;
            //subject.PlanPrice = quota.QuotaPrice;
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
            {
                subject.CostAccountSubjectSyscode = quota.CostAccountSubjectGUID.SysCode;
            }
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

        private void WriteLog(string errMsg)
        {
            //写日志
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "importErrorLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            StreamWriter write = new StreamWriter(logFilePath, false, Encoding.Default);
            write.WriteLine("在数据导入时出现以下错误：" + Environment.NewLine + errMsg);
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
        }
    }
}
