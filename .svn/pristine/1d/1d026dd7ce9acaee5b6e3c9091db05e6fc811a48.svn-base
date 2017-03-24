using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    /// <summary>
    /// 承包方式
    /// </summary>
    public enum EnumContractType
    {
        [Description("施工总承包")]
        施工总承包 = 1,
        [Description("工程总承包EPC")]
        工程总承包EPC = 2,
        [Description("BOT总承包")]
        BOT总承包 = 3,
        [Description("BT总承包")]
        BT总承包 = 4,
        [Description("机电安装承包")]
        机电安装承包 = 5,
        [Description("其它承包方式")]
        其它承包方式 = 6
    }

    /// <summary>
    /// 结构类型
    /// </summary>
    public enum EnumStructureType
    {
        [Description("框架剪力墙结构")]
        框架剪力墙结构 = 1,
        [Description("钢混")]
        钢混 = 2,
        [Description("砖混")]
        砖混 = 3,
        [Description("钢结构")]
        钢结构 = 4,
        [Description("其它")]
        其它 = 5
    }

    /// <summary>
    /// 项目生命周期
    /// </summary>
    public enum EnumProjectLifeCycle
    {
        [Description("投标阶段")]
        投标阶段 = 1,
        [Description("策划阶段")]
        策划阶段 = 2,
        [Description("施工阶段")]
        施工阶段 = 3,
        [Description("竣工结算阶段")]
        竣工结算阶段 = 4,
        [Description("维护阶段")]
        维护阶段 = 5
    }

    /// <summary>
    /// 项目类型
    /// </summary>
    public enum EnumProjectType
    {
        [Description("建筑工程")]
        建筑工程 = 0,
        [Description("装饰装修工程")]
        装饰装修工程 = 1,
        [Description("安装工程")]
        安装工程 = 2,
        [Description("市政桥梁")]
        市政桥梁 = 3,
        [Description("园林绿化工程")]
        园林绿化工程 = 4,
        [Description("大型土石方工程")]
        大型土石方工程 = 5,
        [Description("钢结构工程")]
        钢结构工程 = 6,
        [Description("公路工程")]
        公路工程 = 7,
        [Description("轻轨")]
        轻轨 = 8,
        [Description("其它")]
        其它 = 9,
        [Description("加工厂")]
        加工厂 = 20
    }

    /// <summary>
    /// 项目信息状态
    /// </summary>
    public enum EnumProjectInfoState
    {
        [Description("老项目")]
        老项目 = 0,
        [Description("新项目")]
        新项目 = 1
    }

    /// <summary>
    /// 资金来源
    /// </summary>
    public enum EnumSourcesOfFunding
    {
        [Description("国有资金")]
        国有资金 = 1,
        [Description("民营或外资")]
        民营或外资 = 2,
        [Description("资金自筹")]
        资金自筹 = 3,
        [Description("其它")]
        其它 = 4
    }

    /// <summary>
    /// 工程项目信息
    /// </summary>
    [Serializable]
    public class CurrentProjectInfo
    {
        private string id;
        private string name;
        private string code;
        private decimal measureAccuracy;
        private decimal length;
        private string contractArea;
        //private EnumContractType contractType;
        private int contractType;
        private DateTime createDate = new DateTime(1900, 1, 1);
        private decimal groundLayers;
        private decimal underGroundLayers;
        private string projectLocationProvince;
        private string projectLocationCity;
        private string projectLocationDescript;
        private decimal projectCost;
        private string subProjectDescript;
        private decimal buildingHeight;
        private decimal buildingArea;
        private int structureType;
        private string structureTypeDescript;
        private PersonInfo handlePerson;
        private string handlePersonName;
        private string handlePersonOrgSysCode;
        private StandardUnit defaultLengthUnit;
        private string defaultLengthUnitName;
        private StandardUnit defaultPriceUnit;
        private string defaultPriceUnitName;
        private StandardUnit defaultAreaUnit;
        private string defaultAreaUnitName;
        private StandardUnit defaultVolumeUnit;
        private string defaultVolumeUnitName;
        private StandardUnit defaultWeightUnit;
        private string defaultWeightUnitName;
        private EnumProjectLifeCycle projectLifeCycle;
        private OperationOrg ownerOrg;
        private string ownerOrgSysCode;
        //private EnumProjectType projectType;
        private int projectType;
        private string projectTypeDescript;
        private string descript;
        private EnumProjectInfoState projectInfoState;
        private int isFundsAvailabed = 0;
        private EnumSourcesOfFunding sourcesOfFunding;
        private DateTime lastUpdateDate = new DateTime(1900, 1, 1);
        private string configPath;
        private string ownerOrgName;
        private DateTime beginDate;
        private DateTime endDate;
        private decimal bigModualGroundUpPrice;
        private decimal bigModualGroundDownPrice;

        private decimal theGroundArea;//地上建筑面积      
        private decimal underGroundArea;//地下建筑面积
        private decimal contractCollectRatio;//合同收款比例
        private string baseForm;//基础形式
        private decimal civilContractMoney;//土建合同金额
        private decimal installOrderMoney;//安装合同金额
        private decimal wallProjectArea;//外墙投影面积
        private decimal resProportion;//责任上缴比例
        private string constractStage;//施工阶段
        private string managerDepart;//项目经理部
        private string data1;
        private string data2;
        private DateTime realKGDate;

        private string mapPoint;

        private string saftyReword;
        private decimal responsCost;//自营责任成本      
        private decimal contractIncome;//自营合同收入
        private decimal riskLimits;//自营风险额度   
        private decimal inproportion;//自营责任上缴比列

        private string effectPicPath;// 效果图路径
        private string effectPicFileCabinetId;//效果图文件柜id
        private string flatPicPath;// 平面图路径
        private string flatPicFileCabinetId;//平面图文件柜id
        private int projectCurrState;//项目执行状态
        private string otherProjectName;//其他系统项目名称
        private decimal accountLeftMoney;//项目账户余额
        private int ifSync;

        private PersonInfo submitPerson;
        private string submitPersonName;
        private DateTime submitTime;
        private string materialNote;
        #region 取费
        private Iesi.Collections.Generic.ISet<SelFeeDtl> selFeeDtls = new Iesi.Collections.Generic.HashedSet<SelFeeDtl>();
        private Iesi.Collections.Generic.ISet<SelFeeFormula> selFeeFormulas = new Iesi.Collections.Generic.HashedSet<SelFeeFormula>();
        private SelFeeTemplateMaster selFeeTemplateMaster;
        private string selFeeTemplateName;
        /// <summary>取费模板名称 </summary>
        public virtual string SelFeeTemplateName
        {
            get { return selFeeTemplateName; }
            set { selFeeTemplateName = value; }
        }
        /// <summary>取费模板 </summary>
        public virtual SelFeeTemplateMaster SelFeeTemplateMaster
        {
            get { return selFeeTemplateMaster; }
            set { selFeeTemplateMaster = value; }
        }
        /// <summary>取费明细 </summary>
        public virtual Iesi.Collections.Generic.ISet<SelFeeDtl> SelFeeDetails
        {
            get { return selFeeDtls; }
            set { selFeeDtls = value; }
        }
        public virtual void AddSelFeeDetails(SelFeeDtl oSelFeeDtl)
        {
            oSelFeeDtl.ProjectInfo = this;
            SelFeeDetails.Add(oSelFeeDtl);

        }
        /// <summary>取费公式 </summary>
        public virtual Iesi.Collections.Generic.ISet<SelFeeFormula> SelFeeFormulas
        {
            get { return selFeeFormulas; }
            set { selFeeFormulas = value; }
        }
        public virtual void AddSelFeeFormulas(SelFeeFormula oSelFeeFormula)
        {
            oSelFeeFormula.ProjectInfo = this;
            SelFeeFormulas.Add(oSelFeeFormula);

        }
        #endregion
        public virtual PersonInfo SubmitPerson
        {
            get { return submitPerson; }
            set { submitPerson = value; }
        }

        public virtual string SubmitPersonName
        {
            get { return submitPersonName; }
            set { submitPersonName = value; }
        }

        public virtual DateTime SubmitTime
        {
            get { return submitTime; }
            set { submitTime = value; }
        }

        /// <summary>
        /// 是否内部项目
        /// </summary>
        public virtual int IfSync
        {
            get { return ifSync; }
            set { ifSync = value; }
        }
        /// <summary>
        /// 项目账户余额
        /// </summary>
        public virtual decimal AccountLeftMoney
        {
            get { return accountLeftMoney; }
            set { accountLeftMoney = value; }
        }

        /// <summary>
        /// 其他系统项目名称
        /// </summary>
        public virtual string OtherProjectName
        {
            get { return otherProjectName; }
            set { otherProjectName = value; }
        }

        /// <summary>
        /// 项目执行状态
        /// </summary>
        public virtual int ProjectCurrState
        {
            get { return projectCurrState; }
            set { projectCurrState = value; }
        }
        /// <summary>
        /// 效果图文件柜id
        /// </summary>
        public virtual string EffectPicFileCabinetId
        {
            get { return effectPicFileCabinetId; }
            set { effectPicFileCabinetId = value; }
        }

        /// <summary>
        /// 平面图文件柜id
        /// </summary>
        public virtual string FlatPicFileCabinetId
        {
            get { return flatPicFileCabinetId; }
            set { flatPicFileCabinetId = value; }
        }

        /// <summary>
        /// 效果图路径
        /// </summary>
        public virtual string EffectPicPath
        {
            get { return effectPicPath; }
            set { effectPicPath = value; }
        }

        /// <summary>
        /// 平面图路径
        /// </summary>
        public virtual string FlatPicPath
        {
            get { return flatPicPath; }
            set { flatPicPath = value; }
        }

        /// <summary>
        /// 自营责任成本
        /// </summary>
        virtual public decimal ResponsCost
        {
            get { return responsCost; }
            set { responsCost = value; }
        }
        /// <summary>
        /// 自营合同收入
        /// </summary>
        virtual public decimal ContractIncome
        {
            get { return contractIncome; }
            set { contractIncome = value; }
        }
        /// <summary>
        /// 自营风险额度
        /// </summary>
        virtual public decimal RiskLimits
        {
            get { return riskLimits; }
            set { riskLimits = value; }
        }
        /// <summary>
        /// 自营责任上缴比列
        /// </summary>
        virtual public decimal Inproportion
        {
            get { return inproportion; }
            set { inproportion = value; }
        }
        /// <summary>
        /// 安全奖惩
        /// </summary>
        virtual public string SaftyReword
        {
            get { return saftyReword; }
            set { saftyReword = value; }
        }
        private string saftyTarget;
        /// <summary>
        /// 安全目标
        /// </summary>
        virtual public string SaftyTarget
        {
            get { return saftyTarget; }
            set { saftyTarget = value; }
        }
        private string projecRewordt;
        /// <summary>
        /// 工期奖惩
        /// </summary>
        virtual public string ProjecRewordt
        {
            get { return projecRewordt; }
            set { projecRewordt = value; }
        }
        private string qualityReword;
        /// <summary>
        /// 质量奖惩
        /// </summary>
        virtual public string QualityReword
        {
            get { return qualityReword; }
            set { qualityReword = value; }
        }
        private string quanlityTarget;
        /// <summary>
        /// 质量目标
        /// </summary>
        virtual public string QuanlityTarget
        {
            get { return quanlityTarget; }
            set { quanlityTarget = value; }
        }
        private decimal realPerMoney;
        /// <summary>
        /// 实际预算总金额
        /// </summary>
        virtual public decimal RealPerMoney
        {
            get { return realPerMoney; }
            set { realPerMoney = value; }
        }

        private DateTime aprroachDate;
        /// <summary>
        /// 进场日期
        /// </summary>
        virtual public DateTime AprroachDate
        {
            get { return aprroachDate; }
            set { aprroachDate = value; }
        }

        private Iesi.Collections.Generic.ISet<PeriodNode> details = new Iesi.Collections.Generic.HashedSet<PeriodNode>();
        virtual public Iesi.Collections.Generic.ISet<PeriodNode> Details
        {
            get { return details; }
            set { details = value; }
        }

        /// <summary>
        /// 工期节点
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddDetail(PeriodNode detail)
        {
            detail.ProjectId = this;
            Details.Add(detail);
        }

        private Iesi.Collections.Generic.ISet<ProRelationUnit> proRelationUnitdetails = new Iesi.Collections.Generic.HashedSet<ProRelationUnit>();
        virtual public Iesi.Collections.Generic.ISet<ProRelationUnit> ProRelationUnitdetails
        {
            get { return proRelationUnitdetails; }
            set { proRelationUnitdetails = value; }
        }

        /// <summary>
        /// 相关单位
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddproRelationUnitDetail(ProRelationUnit proRelationUnitdetails)
        {
            proRelationUnitdetails.ProjectId = this;
            ProRelationUnitdetails.Add(proRelationUnitdetails);
        }

        /// <summary>
        /// 实际开工时间
        /// </summary>
        public virtual DateTime RealKGDate
        {
            get { return realKGDate; }
            set { realKGDate = value; }
        }
        /// <summary>
        /// 数据传递
        /// </summary>
        public virtual string Data2
        {
            get { return data2; }
            set { data2 = value; }
        }

        /// <summary>
        /// 数据传递
        /// </summary>
        public virtual string Data1
        {
            get { return data1; }
            set { data1 = value; }
        }
        /// <summary>
        /// 项目经理部
        /// </summary>
        public virtual string ManagerDepart
        {
            get { return managerDepart; }
            set { managerDepart = value; }
        }

        /// <summary>
        /// 施工阶段
        /// </summary>
        public virtual string ConstractStage
        {
            get { return constractStage; }
            set { constractStage = value; }
        }
        /// <summary>
        /// 地上建筑面积
        /// </summary>
        public virtual decimal TheGroundArea
        {
            get { return theGroundArea; }
            set { theGroundArea = value; }
        }
        /// <summary>
        /// 地下建筑面积
        /// </summary>
        public virtual decimal UnderGroundArea
        {
            get { return underGroundArea; }
            set { underGroundArea = value; }
        }
        /// <summary>
        /// 合同收款比例
        /// </summary>
        public virtual decimal ContractCollectRatio
        {
            get { return contractCollectRatio; }
            set { contractCollectRatio = value; }
        }
        /// <summary>
        /// 基础形式
        /// </summary>
        public virtual string BaseForm
        {
            get { return baseForm; }
            set { baseForm = value; }
        }
        /// <summary>
        /// 土建合同金额
        /// </summary>
        public virtual decimal CivilContractMoney
        {
            get { return civilContractMoney; }
            set { civilContractMoney = value; }
        }
        /// <summary>
        /// 安装合同金额
        /// </summary>
        public virtual decimal InstallOrderMoney
        {
            get { return installOrderMoney; }
            set { installOrderMoney = value; }
        }
        /// <summary>
        /// 墙外投影面积
        /// </summary>
        public virtual decimal WallProjectArea
        {
            get { return wallProjectArea; }
            set { wallProjectArea = value; }
        }
        /// <summary>
        /// 责任上缴比例
        /// </summary>
        public virtual decimal ResProportion
        {
            get { return resProportion; }
            set { resProportion = value; }
        }


        /// <summary>
        /// 大包模板木枋地上单价
        /// </summary>
        public virtual decimal BigModualGroundUpPrice
        {
            get { return bigModualGroundUpPrice; }
            set { bigModualGroundUpPrice = value; }
        }
        /// <summary>
        /// 大包模板木枋地下单价
        /// </summary>
        public virtual decimal BigModualGroundDownPrice
        {
            get { return bigModualGroundDownPrice; }
            set { bigModualGroundDownPrice = value; }
        }
        /// <summary>
        /// 开工日期
        /// </summary>
        public virtual DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public virtual DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        /// <summary>
        /// 所属项目部名称(业务组织名称)
        /// </summary>
        public virtual string OwnerOrgName
        {
            get { return ownerOrgName; }
            set { ownerOrgName = value; }
        }

        /// <summary>
        /// IRP配置文件路径
        /// </summary>
        public virtual string ConfigPath
        {
            get { return configPath; }
            set { configPath = value; }
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// 资金来源
        /// </summary>
        public virtual EnumSourcesOfFunding SourcesOfFunding
        {
            get { return sourcesOfFunding; }
            set { sourcesOfFunding = value; }
        }

        /// <summary>
        /// 资金到位情况 0未到位 1到位
        /// </summary>
        public virtual int IsFundsAvailabed
        {
            get { return isFundsAvailabed; }
            set { isFundsAvailabed = value; }
        }

        /// <summary>
        /// 项目对象信息所处的状态
        /// </summary>
        public virtual EnumProjectInfoState ProjectInfoState
        {
            get { return projectInfoState; }
            set { projectInfoState = value; }
        }

        /// <summary>
        /// 项目说明
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 项目类型说明
        /// </summary>
        public virtual string ProjectTypeDescript
        {
            get { return projectTypeDescript; }
            set { projectTypeDescript = value; }
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        public virtual int ProjectType
        {
            get { return projectType; }
            set { projectType = value; }
        }

        /// <summary>
        /// 所属项目部 组织层次码
        /// </summary>
        public virtual string OwnerOrgSysCode
        {
            get { return ownerOrgSysCode; }
            set { ownerOrgSysCode = value; }
        }

        /// <summary>
        /// 所属项目部
        /// </summary>
        public virtual OperationOrg OwnerOrg
        {
            get { return ownerOrg; }
            set { ownerOrg = value; }
        }

        /// <summary>
        /// 项目生命周期
        /// </summary>
        public virtual EnumProjectLifeCycle ProjectLifeCycle
        {
            get { return projectLifeCycle; }
            set { projectLifeCycle = value; }
        }

        /// <summary>
        /// 缺省重量计量单位名称
        /// </summary>
        public virtual string DefaultWeightUnitName
        {
            get { return defaultWeightUnitName; }
            set { defaultWeightUnitName = value; }
        }

        /// <summary>
        /// 缺省重量计量单位
        /// </summary>
        public virtual StandardUnit DefaultWeightUnit
        {
            get { return defaultWeightUnit; }
            set { defaultWeightUnit = value; }
        }

        /// <summary>
        /// 缺省体积计量单位名称
        /// </summary>
        public virtual string DefaultVolumeUnitName
        {
            get { return defaultVolumeUnitName; }
            set { defaultVolumeUnitName = value; }
        }

        /// <summary>
        /// 缺省体积计量单位
        /// </summary>
        public virtual StandardUnit DefaultVolumeUnit
        {
            get { return defaultVolumeUnit; }
            set { defaultVolumeUnit = value; }
        }

        /// <summary>
        /// 缺省面积计量单位名称
        /// </summary>
        public virtual string DefaultAreaUnitName
        {
            get { return defaultAreaUnitName; }
            set { defaultAreaUnitName = value; }
        }

        /// <summary>
        /// 缺省面积计量单位
        /// </summary>
        public virtual StandardUnit DefaultAreaUnit
        {
            get { return defaultAreaUnit; }
            set { defaultAreaUnit = value; }
        }

        /// <summary>
        /// 缺省价格计量单位名称
        /// </summary>
        public virtual string DefaultPriceUnitName
        {
            get { return defaultPriceUnitName; }
            set { defaultPriceUnitName = value; }
        }

        /// <summary>
        /// 缺省价格计量单位
        /// </summary>
        public virtual StandardUnit DefaultPriceUnit
        {
            get { return defaultPriceUnit; }
            set { defaultPriceUnit = value; }
        }

        /// <summary>
        /// 缺省长度计量单位名称
        /// </summary>
        public virtual string DefaultLengthUnitName
        {
            get { return defaultLengthUnitName; }
            set { defaultLengthUnitName = value; }
        }

        /// <summary>
        /// 缺省长度计量单位
        /// </summary>
        public virtual StandardUnit DefaultLengthUnit
        {
            get { return defaultLengthUnit; }
            set { defaultLengthUnit = value; }
        }

        /// <summary>
        /// 立项责任人组织层次码
        /// </summary>
        public virtual string HandlePersonOrgSysCode
        {
            get { return handlePersonOrgSysCode; }
            set { handlePersonOrgSysCode = value; }
        }

        /// <summary>
        /// 立项责任人名称
        /// </summary>
        public virtual string HandlePersonName
        {
            get { return handlePersonName; }
            set { handlePersonName = value; }
        }

        /// <summary>
        /// 立项责任人
        /// </summary>
        public virtual PersonInfo HandlePerson
        {
            get { return handlePerson; }
            set { handlePerson = value; }
        }

        /// <summary>
        /// 结构类型说明
        /// </summary>
        public virtual string StructureTypeDescript
        {
            get { return structureTypeDescript; }
            set { structureTypeDescript = value; }
        }

        /// <summary>
        /// 结构类型
        /// </summary>
        public virtual int StructureType
        {
            get { return structureType; }
            set { structureType = value; }
        }

        /// <summary>
        /// 建筑面积
        /// </summary>
        public virtual decimal BuildingArea
        {
            get { return buildingArea; }
            set { buildingArea = value; }
        }

        /// <summary>
        /// 建筑高度
        /// </summary>
        public virtual decimal BuildingHeight
        {
            get { return buildingHeight; }
            set { buildingHeight = value; }
        }

        /// <summary>
        /// 甲指分包工程 说明
        /// </summary>
        public virtual string SubProjectDescript
        {
            get { return subProjectDescript; }
            set { subProjectDescript = value; }
        }

        /// <summary>
        /// 工程造价（万元）
        /// </summary>
        public virtual decimal ProjectCost
        {
            get { return projectCost; }
            set { projectCost = value; }
        }

        /// <summary>
        /// 工程地点 说明
        /// </summary>
        public virtual string ProjectLocationDescript
        {
            get { return projectLocationDescript; }
            set { projectLocationDescript = value; }
        }

        /// <summary>
        /// 工程地点 市
        /// </summary>
        public virtual string ProjectLocationCity
        {
            get { return projectLocationCity; }
            set { projectLocationCity = value; }
        }

        /// <summary>
        /// 工程地点 省
        /// </summary>
        public virtual string ProjectLocationProvince
        {
            get { return projectLocationProvince; }
            set { projectLocationProvince = value; }
        }

        /// <summary>
        /// 地下层数
        /// </summary>
        public virtual decimal UnderGroundLayers
        {
            get { return underGroundLayers; }
            set { underGroundLayers = value; }
        }

        /// <summary>
        /// 地上层数
        /// </summary>
        public virtual decimal GroundLayers
        {
            get { return groundLayers; }
            set { groundLayers = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 承包方式
        /// </summary>
        public virtual int ContractType
        {
            get { return contractType; }
            set { contractType = value; }
        }

        /// <summary>
        /// 承包范围
        /// </summary>
        public virtual string ContractArea
        {
            get { return contractArea; }
            set { contractArea = value; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public virtual decimal Length
        {
            get { return length; }
            set { length = value; }
        }

        /// <summary>
        /// 测量精度
        /// </summary>
        public virtual decimal MeasureAccuracy
        {
            get { return measureAccuracy; }
            set { measureAccuracy = value; }
        }

        /// <summary>
        /// 项目代码
        /// </summary>
        public virtual string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 地图坐标
        /// </summary>
        public virtual string MapPoint
        {
            get { return mapPoint; }
            set { mapPoint = value; }
        }
        public virtual string MaterialNote
        {
            get
            {
                return materialNote;
            }
            set
            {
                materialNote = value;
            }
        }


        private int taxType;
        /// <summary>
        /// 计税类型 0简易（人工、分包3%，其他为0） 1一般（11%） 
        /// </summary>
        public virtual int TaxType
        {
            get { return taxType; }
            set { taxType = value; }
        }

        /// <summary>
        /// 增值税率
        /// </summary>
        public virtual decimal VatRate { get; set; }
        /// <summary>
        /// 措施费责任成本测定比值
        /// </summary>
        public virtual decimal MeasuresFeeRatio { get; set; }
        /// <summary>
        /// 规费责任成本测定比值
        /// </summary>
        public virtual decimal FeesRatio { get; set; }
        /// <summary>
        /// 管理费责任成本测定比值
        /// </summary>
        public virtual decimal ManagementFeeRatio { get; set; }
        /// <summary>
        /// 临时建设费责任成本测定比值
        /// </summary>
        public virtual decimal  TConstructionRatio { get; set; }
        /// <summary>
        /// 电费责任成本测定比值
        /// </summary>
        public virtual decimal ElectricRatio { get; set; }


        private Iesi.Collections.Generic.ISet<SumAreaParame> listSumAreaParame = new Iesi.Collections.Generic.HashedSet<SumAreaParame>();
        /// <summary>
        /// 累计建筑面积参数
        /// </summary>
        virtual public Iesi.Collections.Generic.ISet<SumAreaParame> ListSumAreaParame
        {
            get { return listSumAreaParame; }
            set { listSumAreaParame = value; }
        }

        private Iesi.Collections.Generic.ISet<MachineCostParame> listMachineCostParame = new Iesi.Collections.Generic.HashedSet<MachineCostParame>();
        /// <summary>
        /// 机械费责任成本费用参数
        /// </summary>
        virtual public Iesi.Collections.Generic.ISet<MachineCostParame> ListMachineCostParame
        {
            get { return listMachineCostParame; }
            set { listMachineCostParame = value; }
        }

    }
}
