using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonForm;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VBasicDataOptr : TBasicDataView
    {
        static CurrentProjectInfo projectInfo = null;

        MStockMng mStockIn = new MStockMng();
        BasicDataOptr currBasicData = null;
        Hashtable query_ht = new Hashtable();

        #region 基础数据名称
        /// <summary>
        /// 二维码使用类型
        /// </summary>
        public static string BASICDATANAME_QRCODETYPE = "二维码使用类型";
        /// <summary>
        /// 专业分类
        /// </summary>
        public static string BASICDATANAME_BillTYPE = "票据种类";
        /// <summary>
        /// 专项费用类型
        /// </summary>
        public static string BASICDATANAME_ZXCOSTTYPE = "专项费用类型";
        /// <summary>
        /// 项目类型
        /// </summary>
        public static string BASICDATANAME_PROJECTTYPE = "项目类型";
        /// <summary>
        /// 项目施工阶段
        /// </summary>
        public static string BASICDATANAME_PROJECTLIFTCYCLE = "工程项目施工阶段";
        /// <summary>
        /// 工程项目执行状态
        /// </summary>
        public static string BASICDATANAME_PROJECTCURRSTATE = "工程项目执行状态";
        /// <summary>
        /// 服务OBS类型
        /// </summary>
        public static string BASICDATANAME_OBSTYPE = "服务OBS类型";
        /// <summary>
        /// 承包方式
        /// </summary>
        public static string BASICDATANAME_CONTRACTWAY = "承包方式";
        /// <summary>
        /// 工程相关单位类型
        /// </summary>
        public static string BASICDATANAME_PROJECTDEPARTTYPE = "工程相关单位类型";
        /// <summary>
        /// 专业分类
        /// </summary>
        public static string BASICDATANAME_PROFESSIONALCATEGORY = "专业分类";
        /// <summary>
        /// 工程项目质量等级
        /// </summary>
        public static string BASICDATANAME_PROJECTLIVEL = "工程项目质量等级";
        /// <summary>
        /// 工程项目安全等级
        /// </summary>
        public static string BASICDATANAME_PROJECTSAFTY = "工程项目安全等级";
        /// <summary>
        /// 专业策划类型
        /// </summary>
        public static string BASICDATANAME_ENGINNERTYPE = "专业策划类型";
        /// <summary>
        /// 专业分包工作类型
        /// </summary>
        public static string BASICDATANAME_SUBCONTRACTPEOJECT = "专业分包工作类型";
        /// <summary>
        /// 物资档次
        /// </summary>
        public static string BASICDATANAME_MATERIALGRADE = "物资档次";
        /// <summary>
        /// 费用类型
        /// </summary>
        public static string BASICDATANAME_COSTTYPE = "费用类型";
        /// <summary>
        /// 岗位类型
        /// </summary>
        public static string BASICDATANAME_POSTTYPE = "岗位类型";
        /// <summary>
        /// 报销费用类型
        /// </summary>
        public static string BASICDATANAME_EXPENSECOSTTYPE = "报销费用类型";
        /// <summary>
        /// 工种类别
        /// </summary>
        public static string BASICDATANAME_WOKERTYPE = "工种";
        /// <summary>
        /// 隐患类型
        /// </summary>
        public static string BASICDATANAME_SAFTY = "隐患类型";
        /// <summary>
        /// 工种类别
        /// </summary>
        public static string BASICDATANAME_OBSSERVICE = "OBS服务状态";
        /// <summary>
        /// 工程项目基础形式
        /// </summary>
        public static string BASIC_FORM = "工程项目基础形式";
        /// <summary>
        /// 工程项目结构形式
        /// </summary>
        public static string STRUCT_FORM = "工程项目结构形式";
   
        /// <summary>
        /// 设备租赁费用
        /// </summary>
        public static string BASICDATANAME_MATERIALRENTAL = "设备租赁费用";
        /// <summary>
        /// 劳务队伍类型
        /// </summary>
        public static string BASICDATANAME_USEDRANKTYPE = "劳务队伍类型";
        /// <summary>
        /// 劳务队伍类型
        /// </summary>
        public static string BASICDATANAME_CONSTRACTTYPE = "劳务分包承包方式";
        /// <summary>
        /// 调拨单位
        /// </summary>
        public static string BASICDATANAME_MOVEPROJECT = "调拨单位";
        #region 项目上料具
        /// <summary>
        /// 料具费用类型
        /// </summary>
        public static string BASICDATENAME_MATERIALCOSTTYPE = "料具费用类型";
        /// <summary>
        /// 价格类型
        /// </summary>
        public static string BASICDATENAME_PRICETYPE = "价格类型";
        /// <summary>
        /// 料具维修内容
        /// </summary>
        public static string BASICDATENAME_MATREPAIRCON = "料具维修内容";
        #endregion
        #region 料具站
        /// <summary>
        /// 料具费用类型
        /// </summary>
        public static string BASICDATENAME_StationMATERIALCOSTTYPE = "料具站费用类型";
        /// <summary>
        /// 价格类型
        /// </summary>
        public static string BASICDATENAME_StationPRICETYPE = "料具站价格类型";
        /// <summary>
        /// 料具维修内容
        /// </summary>
        public static string BASICDATENAME_StationMATREPAIRCON = "料具站保养及维修内容";
        /// <summary>
        /// 料具站尺寸分段统计物资
        /// </summary>
        public static string BASICDATENAME_StationMaterialSize = "料具站尺寸分段统计物资";
        /// <summary>
        /// 料具站支撑体系分布表物资
        /// </summary>
        public static string BASICDATENAME_StationMaterialDistribute = "料具站支撑体系分布表物资";
        #endregion
        /// <summary>
        /// 编制依据
        /// </summary>
        public static string BASICDATANAME_COMPILATION = "编制依据";
        /// <summary>
        /// 工程状态
        /// </summary>
        public static string BASICDATANAME_PROJECTSTATUS = "工程状态";
        public static string BASICDATANAME_PROJECTSTATE = "项目工程状态";
        /// <summary>
        /// 支付方式
        /// </summary>
        public static string BASICDATANAME_PAYMENT = "支付方式";
        /// <summary>
        /// 验收结算单不需汇总分类
        /// </summary>
        public static string BASICDATANAME_StockBalNoSumCategory = "验收结算单不需汇总分类";

        /// <summary>
        /// 预警物资分类
        /// </summary>
        public static string BASICDATANAME_WarningCategory = "预警物资分类";

        /// <summary>
        /// 施工专业
        /// </summary>
        public static string BASICDATANAME_ProfessionalConstruction = "施工专业";
        #endregion

        #region 成本核算部分
        /// <summary>
        /// PBS结构类型
        /// </summary>
        public static string PBS_StructType = "结构类型";
         /// <summary>
        /// 函件类型
        /// </summary>
        public static string Letters_Style = "函件类型";
        /// 收发函类型
        /// </summary>
        public static string Send_Style = "收发函类型";
        /// <summary>
        /// WBS工程任务类型检查要求
        /// </summary>
        public static string WBS_CheckRequire = "检查要求";
        /// <summary>
        /// 管理模式（成本项中适用模式）
        /// </summary>
        public static string CostItem_ManagementMode = "管理模式";

        /// <summary>
        /// 契约组类型
        /// </summary>
        public static string WBS_ContractGroupType = "契约组类型";

        /// <summary>
        /// 契约明细类型
        /// </summary>
        public static string WBS_ContractGroupDetailType = "契约明细类型";

        /// <summary>
        /// 进度计划版本
        /// </summary>
        public static string PLAN_ScheduleVersion = "进度计划版本";

        /// <summary>
        /// 进度计划口径
        /// </summary>
        public static string PLAN_ScheduleCaliber = "进度计划口径";

        /// <summary>
        /// 总进度计划类型
        /// </summary>
        public static string PLAN_ScheduleType = "总进度计划类型";

        /// <summary>
        /// 总滚动进度计划类型
        /// </summary>
        public static string PLAN_ScheduleTypeRolling = "总滚动进度计划类型";

        /// <summary>
        /// 工期计量单位
        /// </summary>
        public static string PLAN_ScheduleUnit = "工期计量单位";

        /// <summary>
        /// 资源需求计划类型
        /// </summary>
        public static string PLAN_ResourceRequirePlanType = "资源需求计划类型";

        /// <summary>
        /// 执行进度计划类型
        /// </summary>
        public static string PLAN_ExecScheduleType = "执行进度计划类型";

        public static string PLAN_MonthScheduleDefaultDateArea= "月进度计划默认时间段";

        public static string PLAN_ResourceRequirePlanTypeCate = "资源需求计划类型编码";
        public static string SelFeeTemplateSpecialType = "取费专业类型";
        #endregion

        #region 指标管理基础数据
        /// <summary>
        /// 指标管理计量单位
        /// </summary>
        public static string INDICATOR_Unit = "指标管理计量单位";
        /// <summary>
        /// 指标管理报表类型
        /// </summary>
        public static string INDICATOR_ReportType = "指标管理报表类型";

        /// <summary>
        /// 初始化 指标管理报表类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitIndicatorReportType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.INDICATOR_ReportType);
            InitDropDownObjectByDataSource(dropDownObject, addBlank, list, "--请选择--");
        }

        /// <summary>
        /// 初始化 指标管理计量单位 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitIndicatorUnit(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.INDICATOR_Unit);
            InitDropDownObjectByDataSource(dropDownObject, addBlank, list, "--请选择--");
        }
        #endregion

        #region 基础数据初始化下拉筐方法

        /// <summary>
        /// 初始化 下拉筐 通过给DataSource赋值的方式
        /// </summary>
        /// <param name="dropDownObject">下拉筐对象</param>
        /// <param name="addBlank">为true时增加一空行</param>
        /// <param name="list">要显示的基础数据</param>
        /// <param name="blankValueName">添加一行空行时，空行处用来显示的名称</param>
        private static void InitDropDownObjectByDataSource(object dropDownObject, bool addBlank, IList list, string blankValueName)
        {
            if (list != null)
            {
                if (addBlank)
                {
                    BasicDataOptr bdo = new BasicDataOptr();
                    bdo.BasicCode = "";
                    bdo.BasicName = blankValueName;
                    bdo.Id = "";
                    bdo.ParentId = "";
                    bdo.State = 0;
                    list.Insert(0, bdo);
                }
                if (dropDownObject.GetType() == typeof(DataGridViewComboBoxColumn))
                {
                    ((DataGridViewComboBoxColumn)dropDownObject).DataSource = list;
                    ((DataGridViewComboBoxColumn)dropDownObject).DisplayMember = "BasicName";
                    ((DataGridViewComboBoxColumn)dropDownObject).ValueMember = "BasicCode";

                }
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    ((CustomComboBox)dropDownObject).DataSource = list;
                    ((CustomComboBox)dropDownObject).DisplayMember = "BasicName";
                    ((CustomComboBox)dropDownObject).ValueMember = "BasicCode";
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)dropDownObject).DataSource = list;
                    ((ComboBox)dropDownObject).DisplayMember = "BasicName";
                    ((ComboBox)dropDownObject).ValueMember = "BasicCode";
                }
            }
        }

        /// <summary>
        /// 初始化 下拉筐
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        /// <param name="list"></param>
        private static void InitDropDownObject(object dropDownObject, bool addBlank, IList list)
        {
            if (list != null)
            {
                if (dropDownObject.GetType() == typeof(DataGridViewComboBoxColumn))
                {
                    if (addBlank) ((DataGridViewComboBoxColumn)dropDownObject).Items.Add("");
                    foreach (BasicDataOptr bdo in list)
                    {
                        ((DataGridViewComboBoxColumn)dropDownObject).Items.Add(bdo.BasicName);
                    }
                }
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    if (addBlank) ((CustomComboBox)dropDownObject).Items.Add("");
                    foreach (BasicDataOptr bdo in list)
                    {
                        ((CustomComboBox)dropDownObject).Items.Add(bdo.BasicName);
                    }
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    if (addBlank) ((ComboBox)dropDownObject).Items.Add("");
                    foreach (BasicDataOptr bdo in list)
                    {
                        ((ComboBox)dropDownObject).Items.Add(bdo.BasicName);
                    }
                }
            }
        }
        /// <summary>
        /// 初始化 二维码使用类型
        /// </summary
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitQRCodeType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_QRCODETYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化结构类型下拉框
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void InitImplantType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PBS_StructType);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化 执行进度计划类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitExecScheduleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ExecScheduleType);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 取费专业类型
        /// </summary>
        /// <returns></returns>
        public static List<BasicDataOptr> GetSelFeeTemplateSpecialType()
        {
            IList lst = StaticMethod.GetBasicDataByName(VBasicDataOptr.SelFeeTemplateSpecialType);
            return lst == null || lst.Count == 0 ? null : lst.OfType < BasicDataOptr>().ToList();
        }
        /// <summary>
        /// 初始化收发函类型
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void SendStyleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.Send_Style);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// 函件类型
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void LettersStyleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.Letters_Style);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 工期计量单位 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitScheduleUnit(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleUnit);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 工程项目基础形式 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitBasicFrom(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASIC_FORM);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化 施工阶段 下拉筐
        /// </summary
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProjectConstractStage(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTLIFTCYCLE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
         
        /// <summary>
        /// 初始化 工程项目执行状态 下拉筐
        /// </summary
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProjectCurrState(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTCURRSTATE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化 项目类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProjectType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化 服务OBS类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitOBSType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_OBSTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化 资源需求计划类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitResReqirePlan(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ResourceRequirePlanType);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// 初始化 承包方式 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitContractWay(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_CONTRACTWAY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化 专项费用类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitZXCostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_ZXCOSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        
        /// <summary>
        /// 初始化 工程项目结构形式 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitStructFrom(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.STRUCT_FORM);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 总进度计划类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitScheduleType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByNameAndProjectName(VBasicDataOptr.PLAN_ScheduleType, projectInfo.Name);
            if (list != null && list.Count > 0)
            {
                InitDropDownObject(dropDownObject, addBlank, list);
            }
            else
            {
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    if (addBlank) ((CustomComboBox)dropDownObject).Items.Add("");
                    ((CustomComboBox)dropDownObject).Items.Add("土建进度计划");
                    ((CustomComboBox)dropDownObject).Items.Add("安装进度计划");
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    if (addBlank) ((ComboBox)dropDownObject).Items.Add("");
                    ((ComboBox)dropDownObject).Items.Add("土建进度计划");
                    ((ComboBox)dropDownObject).Items.Add("安装进度计划");
                }
            }
        }

        /// <summary>
        /// 初始化 总滚动进度计划类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitScheduleTypeRolling(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByNameAndProjectName(VBasicDataOptr.PLAN_ScheduleTypeRolling, projectInfo.Name);
            if (list != null && list.Count > 0)
            {
                InitDropDownObject(dropDownObject, addBlank, list);
            }
            else
            {
                if (dropDownObject.GetType() == typeof(CustomComboBox))
                {
                    if (addBlank) ((CustomComboBox)dropDownObject).Items.Add("");
                    ((CustomComboBox)dropDownObject).Items.Add("土建总滚动进度计划");
                    ((CustomComboBox)dropDownObject).Items.Add("安装总滚动进度计划");
                }
                if (dropDownObject.GetType() == typeof(ComboBox))
                {
                    if (addBlank) ((ComboBox)dropDownObject).Items.Add("");
                    ((ComboBox)dropDownObject).Items.Add("土建总滚动进度计划");
                    ((ComboBox)dropDownObject).Items.Add("安装总滚动进度计划");
                }
            }
        }

        /// <summary>
        /// 根据当前项目初始化基础数据
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitBasicDataByCurrProjectInfo(string basicName, object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByNameAndProjectName(basicName, projectInfo.Name);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 支付方式 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitPayment(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PAYMENT);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// 初始化 进度计划口径 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitScheduleCaliber(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleCaliber);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 进度计划版本 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitScheduleVersion(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ScheduleVersion);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        //
        /// <summary>
        /// 初始化 工程相关单位类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProjectDepartType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTDEPARTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 专业分类 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProfessionCategory(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROFESSIONALCATEGORY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        public static void InitBillType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_BillTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始化 工程项目质量等级 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProjectLivel(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTLIVEL);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 工程项目安全等级 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProjectSafty(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTSAFTY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 专业策划类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitEnginnerType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_ENGINNERTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        
        /// <summary>
        /// 初始化 工种 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitWokerType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_WOKERTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        
        /// <summary>
        /// 初始化 隐患类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitDangerType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_SAFTY);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 OBS服务状态 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitOBSService(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_OBSSERVICE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 设备租赁费用 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitMaterialRental(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_MATERIALRENTAL);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// 初始化 报销费用类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitExpensesCose(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_EXPENSECOSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 劳务队伍类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitUsedRankType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_USEDRANKTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 专业分包工作类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitSubContractProject(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_SUBCONTRACTPEOJECT);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 劳务分包承包方式 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitContractType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_CONSTRACTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 工程状态 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitProjectStatus(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTSTATUS);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        public static void InitProjectState(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_PROJECTSTATE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 物资档次 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitMaterialGrade(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_MATERIALGRADE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 编制依据 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitDemandCompilation(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_COMPILATION);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 费用类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitCostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_COSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }

        /// <summary>
        /// 初始化 岗位类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitPostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_POSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }


        /// <summary>
        /// 初始化 调拨单位 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitMoveProject(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_MOVEPROJECT);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        #region 项目上料具
        /// <summary>
        /// 初始 料具费用类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitMatCostType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_MATERIALCOSTTYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        
        /// <summary>
        /// 初始 价格类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitPriceType(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_PRICETYPE);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始 料具维修内容 下拉筐(根据料具类型加载维修内容)
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitMatRepairCon(object dropDownObject, bool addBlank, Material material)
        {
            IList list_BasicData = new ArrayList();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_MATREPAIRCON);
            foreach (BasicDataOptr basicData in list)
            {
                if (basicData.BasicCode == material.Name)
                {
                    list_BasicData.Add(basicData);
                }
            }
            InitDropDownObject(dropDownObject, addBlank, list_BasicData);
        }
        #endregion
        #region 料具站上料具
        public static  IList GetData(IList lstData,string sMaterialName)
        {//1、备注为空 表明符合任何物资匹配 选中    
            //2、备注中包含物资名称  选中 
            //3、如果第一种和第二种获取的都是空 所有备注信息为[其他]的都被选中
            IList lstResult = null;
            string sOtherMaterial = "其他";
            IEnumerable<BasicDataOptr> lstTemp=null;
            if (lstData != null)
            {
                lstTemp=lstData.OfType<BasicDataOptr>();
                lstResult = lstTemp.Where(a =>
                    string.IsNullOrEmpty(a.Descript) ||(a.Descript!=sOtherMaterial && a.Descript.Split('|').Contains(sMaterialName))).ToList();
                if (lstResult == null || lstResult.Count == 0)
                {
                    lstResult = lstTemp.Where(a=>a.Descript == sOtherMaterial).ToList();
                }
            }
            return lstResult;
        }
        /// <summary>
        /// 初始 料具站 料具费用类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitStationMatCostType(object dropDownObject, bool addBlank, string sMaterianName)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMATERIALCOSTTYPE);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 根据物资名称获取
        /// </summary>
        /// <param name="sMaterianName"></param>
        /// <returns></returns>
        public static IList GetStationMatCostType(string sMaterianName)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMATERIALCOSTTYPE);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            return list;
        }
        /// <summary>
        /// 获取 料具站尺寸分段统计物资
        /// </summary>
        /// <returns></returns>
        public static IList GetStationMaterialSize()
        {
            return StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMaterialSize);
        }
        /// <summary>
        /// 获取 料具站支撑体系分布表物资
        /// </summary>
        /// <returns></returns>
        public static IList GetStationMaterialDistribute()
        {
            return StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMaterialDistribute);
        }
        /// <summary>
        /// 初始 料具站 价格类型 下拉筐
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitStationPriceType(object dropDownObject, bool addBlank, string sMaterianName)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationPRICETYPE);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        /// <summary>
        /// 初始 料具站 料具维修内容 下拉筐(根据料具类型加载维修内容)
        /// </summary>
        /// <param name="dropDownObject">DataGridViewComboBoxColumn或CustomComboBox</param>
        /// <param name="addBlank">true 增加一行空值</param>
        public static void InitStationMatRepairCon(object dropDownObject, bool addBlank, string sMaterianName)
        {
            string sOtherMaterial="其他";
            //IList list_BasicData = new ArrayList();
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_StationMATREPAIRCON);
            if (!string.IsNullOrEmpty(sMaterianName))
            {
                list = GetData(list, sMaterianName);
            }
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        #endregion
        
        /// <summary>
        /// 检查要求(检查专业)
        /// </summary>
        /// <param name="dropDownObject"></param>
        /// <param name="addBlank"></param>
        public static void InitWBSCheckRequir(object dropDownObject, bool addBlank)
        {
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.WBS_CheckRequire);
            InitDropDownObject(dropDownObject, addBlank, list);
        }
        #endregion

        public VBasicDataOptr()
        {
            InitializeComponent();
            InitialEvents();

            InitData();
        }
        private void InitData()
        {
            LoadBasicDefine();
            LoadBasicDetail();
        }
        static VBasicDataOptr()
        {
            if (projectInfo == null)
                projectInfo = StaticMethod.GetProjectInfo();
        }

        internal void Start()
        {

        }

        private void InitialEvents()
        {
            btnDelJob.Click += new EventHandler(btnDelJob_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnHelp.Click += new EventHandler(btnHelp_Click);

            listBoxType.SelectedIndexChanged += new EventHandler(listBoxType_SelectedIndexChanged);

            btnAddType.Click += new EventHandler(btnAddType_Click);
            btnUpdateType.Click += new EventHandler(btnUpdateType_Click);
            btnDeleteType.Click += new EventHandler(btnDeleteType_Click);

            dgvOptr.CellDoubleClick += new DataGridViewCellEventHandler(dgvOptr_CellDoubleClick);
        }

        void btnDeleteType_Click(object sender, EventArgs e)
        {
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;
            if (currBasicData == null)
            {
                MessageBox.Show("请选择要删除的类型！");
                return;
            }
            if (MessageBox.Show("您确认要删除“" + currBasicData.BasicName + "”?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Or(Expression.Eq("Id", currBasicData.Id), Expression.Eq("ParentId", currBasicData.Id)));

                IList list = mStockIn.StockInSrv.GetObjects(typeof(BasicDataOptr), oq);

                mStockIn.StockInSrv.DeleteByDao(list);

                listBoxType.Items.RemoveAt(listBoxType.SelectedIndex);

                dgvOptr.Rows.Clear();
                query_ht.Clear();
            }
        }

        void btnUpdateType_Click(object sender, EventArgs e)
        {
            if (currBasicData == null)
            {
                MessageBox.Show("请选择要修改的类型！");
                listBoxType.Focus();
                return;
            }

            string typeName = txtTypeName.Text.Trim();
            if (typeName == "")
            {
                MessageBox.Show("请输入类型名称！");
                txtTypeName.Focus();
                return;
            }

            if (currBasicData.BasicName != typeName)
            {
                currBasicData.BasicName = typeName;

                currBasicData = mStockIn.StockInSrv.SaveBasicData(currBasicData);

                listBoxType.DisplayMember = "BasicName";
                listBoxType.Items[listBoxType.SelectedIndex] = currBasicData;
                listBoxType.SelectedItem = currBasicData;
            }

        }

        void btnAddType_Click(object sender, EventArgs e)
        {
            string typeName = txtTypeName.Text.Trim();
            if (typeName == "")
            {
                MessageBox.Show("请输入类型名称！");
                txtTypeName.Focus();
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BasicName", typeName));
            oq.AddCriterion(Expression.Eq("State", -1));
            IList list = mStockIn.StockInSrv.GetObjects(typeof(BasicDataOptr), oq);
            if (list.Count > 0)
            {
                MessageBox.Show("该类型已存在！");
                txtTypeName.Focus();
                return;
            }

            BasicDataOptr addBasic = new BasicDataOptr();
            addBasic.BasicName = typeName;
            addBasic.State = -1;

            addBasic = mStockIn.StockInSrv.SaveBasicData(addBasic);

            listBoxType.Items.Add(addBasic);
            listBoxType.SelectedItem = addBasic;
            currBasicData = addBasic;

            dgvOptr.Rows.Clear();
            query_ht.Clear();
        }

        void dgvOptr_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvOptr.Columns[e.ColumnIndex].Name == colExtendField1.Name)
            {
                VDepartSelector frm = new VDepartSelector("1");

                frm.ShowDialog();

                if (frm.Result != null && frm.Result.Count > 0)
                {
                    CurrentProjectInfo selectProject = frm.Result[0] as CurrentProjectInfo;
                    if (selectProject != null)
                    {
                        dgvOptr.Rows[e.RowIndex].Cells[colExtendField1.Name].Value = selectProject.Name;
                    }
                }
            }
        }

        private void LoadBasicDefine()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("State", -1));
            objectQuery.AddOrder(Order.Asc("BasicName"));

            IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr obj in list)
            {
                listBoxType.Items.Add(obj);
            }

            try
            {
                //listBoxType.DataSource = list;
                listBoxType.DisplayMember = "BasicName";
            }
            catch (Exception e)
            {
                string a = "";
            }

            if (listBoxType.Items.Count > 0)
            {
                listBoxType.SelectedIndex = 0;
            }
        }

        private void LoadBasicDetail()
        {
            dgvOptr.Rows.Clear();
            query_ht.Clear();
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;

            if (currBasicData == null) return;
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ParentId", currBasicData.Id));
            objectQuery.AddOrder(Order.Asc("Id"));
            IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);
            foreach (BasicDataOptr model in list)
            {
                int rowIndex = dgvOptr.Rows.Add();
                DataGridViewRow row = dgvOptr.Rows[rowIndex];
                row.Tag = model;
                row.Cells[colExtendField1.Name].Value = model.ExtendField1;
                row.Cells["Code"].Value = model.BasicCode;
                row.Cells["BName"].Value = model.BasicName;
                row.Cells["Remark"].Value = model.Descript;
            }
        }

        void listBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {

            dgvOptr.Rows.Clear();
            query_ht.Clear();

            //添加基础数据明细列表
            try
            {
                if (currBasicData != null)
                {
                    ObjectQuery objectQuery = new ObjectQuery();
                    objectQuery.AddCriterion(Expression.Eq("ParentId", currBasicData.Id));
                    objectQuery.AddOrder(Order.Asc("Id"));
                    IList list = mStockIn.StockInSrv.GetBasicData(objectQuery);

                    foreach (BasicDataOptr mx in list)
                    {
                        int i = this.dgvOptr.Rows.Add();
                        DataGridViewRow row = dgvOptr.Rows[i];
                        row.Tag = mx;

                        row.Cells[colExtendField1.Name].Value = mx.ExtendField1;
                        row.Cells["Code"].Value = mx.BasicCode;
                        row.Cells["BName"].Value = mx.BasicName;
                        row.Cells["Remark"].Value = mx.Descript;
                        query_ht.Add(mx.Id, mx.BasicCode + "_" + mx.BasicName + "_" + mx.Descript + "_" + mx.ExtendField1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查找基础数据明细出错。", ExceptionUtil.ExceptionMessage(ex));
            }

        }

        void btnHelp_Click(object sender, EventArgs e)
        {
            //VHelp vHelp = new VHelp();
            //vHelp.helpInfo = " 1:“任务权限”栏输入:录入或者审核  \n 2:“人员名称”栏输入系统注册的人员姓名 \n 3:“备注”栏可不输入内容 ";
            //vHelp.ShowDialog();
        }

        void btnDelJob_Click(object sender, EventArgs e)
        {
            if (dgvOptr.CurrentRow == null || dgvOptr.CurrentRow.Tag == null)
            {
                MessageBox.Show("基础数据表没有明细。");
                return;
            }

            DialogResult dr = MessageBox.Show("确定要删除吗？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            try
            {

                BasicDataOptr model = dgvOptr.CurrentRow.Tag as BasicDataOptr;
                mStockIn.StockInSrv.DelBasicDataBySql(model);
                dgvOptr.Rows.Remove(dgvOptr.CurrentRow);

                ////写入日志
                //LogData model_1 = new LogData();
                //model_1.Code = model.BasicName;
                //model_1.Descript = "[删除基础数据][ID:" + model.Id + "]";
                //model_1.OperPerson = LoginInfomation.LoginInfo.ThePerson.Name;
                //mOrder.InsertLogDate(model_1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除基础数据明细出错。" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;

            //foreach (DataGridViewRow row in dgvOptr.Rows)
            //{
            //    if ((row.Cells["BName"].Value == null || "".Equals(row.Cells["BName"].Value)) && !row.IsNewRow)
            //    {
            //        string no = row.Cells["No"].Value.ToString();
            //        MessageBox.Show("序号为[" + no + "]的名称不能为空！");
            //        return;
            //    }
            //    if (!row.IsNewRow && currBasicData != null && currBasicData.Id == 4 && ClientUtil.ToString(row.Cells["Code"].Value) != "录入" && ClientUtil.ToString(row.Cells["Code"].Value) != "审核")
            //    {
            //        MessageBox.Show("任务栏输入必须为[录入]或[审核]！");
            //        return;
            //    }
            //}

            foreach (DataGridViewRow row in dgvOptr.Rows)
            {
                if (!row.IsNewRow)
                {

                    if (row.Cells["BName"].Value == null || "".Equals(row.Cells["BName"].Value))
                    {
                        string no = row.Cells["No"].Value.ToString();
                        MessageBox.Show("序号为[" + no + "]的名称不能为空！");
                        return;
                    }
                    if (currBasicData != null && currBasicData.Id == "4" && ClientUtil.ToString(row.Cells["Code"].Value) != "录入" && ClientUtil.ToString(row.Cells["Code"].Value) != "审核")
                    {
                        MessageBox.Show("任务栏输入必须为[录入]或[审核]！");
                        return;
                    }

                    BasicDataOptr model = row.Tag as BasicDataOptr;
                    if (model == null)
                    {
                        model = new BasicDataOptr();
                    }

                    model.BasicCode = ClientUtil.ToString(row.Cells["Code"].Value);
                    model.BasicName = ClientUtil.ToString(row.Cells["BName"].Value);
                    model.Descript = ClientUtil.ToString(row.Cells["Remark"].Value);
                    model.ParentId = currBasicData.Id;

                    object ExtendField1Value = row.Cells[colExtendField1.Name].Value;
                    model.ExtendField1 = ExtendField1Value != null ? ExtendField1Value.ToString() : "";

                    try
                    {
                        if (model.Id != null)
                        {
                            if (query_ht.Contains(model.Id))
                            {
                                string str = ClientUtil.ToString(query_ht[model.Id]);
                                if (str != model.BasicCode + "_" + model.BasicName + "_" + model.Descript + "_" + model.ExtendField1)
                                {
                                    model = mStockIn.StockInSrv.SaveBasicData(model);
                                }
                            }
                        }
                        else
                        {
                            model = mStockIn.StockInSrv.SaveBasicData(model);
                        }
                        row.Tag = model;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存出错。\n" + ExceptionUtil.ExceptionMessage(ex));
                        return;
                    }
                }
            }
            ////写入日志
            //LogData model_1 = new LogData();
            //model_1.Code = currBasicData.BasicName;
            //model_1.Descript = "[保存基础数据][ID:" + currBasicData.Id + "]";
            //model_1.OperPerson = LoginInfomation.LoginInfo.ThePerson.Name;
            //mOrder.InsertLogDate(model_1);

            MessageBox.Show("保存成功！");
        }

        private void listBoxType_MouseClick(object sender, MouseEventArgs e)
        {
            currBasicData = listBoxType.SelectedItem as BasicDataOptr;
            txtTypeName.Text = currBasicData.BasicName;

            this.btnHelp.Visible = false;
            if (currBasicData != null && currBasicData.Id == "4")
            {
                dgvOptr.Columns[this.Code.Name].HeaderText = "任务权限";
                dgvOptr.Columns[this.BName.Name].HeaderText = "人员名称";
                dgvOptr.Columns[this.Remark.Name].HeaderText = "备注(任务栏输入[录入]或[审核])";
                this.btnHelp.Visible = true;
            }

            else
            {
                dgvOptr.Columns[colExtendField1.Name].HeaderText = "扩展字段1";
                dgvOptr.Columns[this.Code.Name].HeaderText = "编码";
                dgvOptr.Columns[this.BName.Name].HeaderText = "名称";
                dgvOptr.Columns[this.Remark.Name].HeaderText = "备注";
            }
        }

        internal static void InitPBS_StructType(ComboBox txtConstructionStyle, bool p)
        {
            throw new NotImplementedException();
        }

        internal static void SendStyleType()
        {
            throw new NotImplementedException();
        }
    }
}