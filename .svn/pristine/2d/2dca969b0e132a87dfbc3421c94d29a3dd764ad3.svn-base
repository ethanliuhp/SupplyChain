using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Domain
{
    /// <summary>
    /// 仓库收发存报表
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInOut
    {
        private string id;
        private long version = -1;
        private int fiscalYear;
        private int fiscalMonth;

        private int acctType;
        private Material theMaterial;
        private StationCategory theStationCategory;
        private decimal lastQuantity;
        private decimal nowInQuantity;
        private decimal nowInRedQuantity;
        private decimal nowOutQuantity;
        private decimal nowOutRedQuantity;
        private decimal nowQuantity;
        private decimal profitInQuantity;
        private decimal lossOutQuantity;

        private decimal buyInQuantity;
        private decimal saleOutQuantity;
        private decimal moveInQuantity;
        private decimal moveOutQuantity;

        private decimal profitInAddQuantity;
        private decimal lossOutAddQuantity;

        private decimal buyInAddQuantity;
        private decimal saleOutAddQuantity;
        private decimal moveInAddQuantity;
        private decimal moveOutAddQuantity;
        private string matCode;
        private string matName;
        private string matSpec;
        private string unitName;
        private decimal lastMoney;

        private decimal nowInMoney;
        private decimal nowInRedMoney;

        private decimal nowOutMoney;
        private decimal nowOutRedMoney;

        private decimal nowMoney;
        private decimal buyInMoney;
        private decimal buyInAddMoney;
        private decimal saleOutMoney;
        private decimal saleOutAddMoney;
        private decimal lossOutMoney;
        private decimal lossOutAddMoney;
        private decimal profitInMoney;
        private decimal profitInAddMoney;
        private decimal moveInMoney;
        private decimal moveInAddMoney;
        private decimal moveOutMoney;
        private decimal moveOutAddMoney;

        private decimal buyInRedQuantity;
        private decimal buyInRedMoney;
        private decimal buyInRedAddQuantity;
        private decimal buyInRedAddMoney;
        private decimal moveInRedQuantity;
        private decimal moveInRedMoney;
        private decimal moveInRedAddQuantity;
        private decimal moveInRedAddMoney;
        private decimal saleOutRedQuantity;
        private decimal saleOutRedMoney;
        private decimal saleOutRedAddQuantity;
        private decimal saleOutRedAddMoney;
        private decimal moveOutRedQuantity;
        private decimal moveOutRedMoney;
        private decimal moveOutRedAddQuantity;
        private decimal moveOutRedAddMoney;
        private string stockInDtlId;
        private string special;
        private string projectId;

        private string materialStuff;

        private string accountPersonOrgSysCode;
    
        private string accountRange;

        private string accountTaskName;
        private string accountTaskSysCode;
        private string accountOrgGuid;
        private string accountOrgName;
        private string accountOrgSyscode;
        private string  createPersonID;
        private string createPersonName;
        private DateTime realOperationDate;
        private string createPersonOrgSysCode;
        public virtual string CreatePersonOrgSysCode
        {
            get { return createPersonOrgSysCode; }
            set { createPersonOrgSysCode = value; }
        }
        public virtual string CreatePersonName
        {
            get { return createPersonName; }
            set { createPersonName = value; }
        }
        public virtual string CreatePersonID
        {
            get { return createPersonID; }
            set { createPersonID = value; }
        }
        public virtual DateTime  RealOperationDate
        {
            get { return realOperationDate; }
            set { realOperationDate = value; }
        }
        /// <summary>
        /// 结算人员组织的syscode
        /// </summary>
        public virtual string AccountPersonOrgSysCode
        {
            get { return accountPersonOrgSysCode; }
            set { accountPersonOrgSysCode = value; }
        }

        /// <summary>
        /// 结算范围
        /// </summary>
        public virtual string AccountRange
        {
            get { return accountRange; }
            set { accountRange = value; }
        }
        /// <summary>
        /// 结算节点名称
        /// </summary>
        public virtual string AccountTaskName
        {
            get { return accountTaskName; }
            set { accountTaskName = value; }
        }
        /// <summary>
        /// 结算任务节点的syscode
        /// </summary>
        public virtual string AccountTaskSysCode
        {
            get { return accountTaskSysCode; }
            set { accountTaskSysCode = value; }
        }
        /// <summary>
        /// 结算组织的id
        /// </summary>
        public virtual string AccountOrgGuid
        {
            get { return accountOrgGuid; }
            set { accountOrgGuid = value; }
        }
        /// <summary>
        /// 结算组织的名称
        /// </summary>
        public virtual string AccountOrgName
        {
            get { return accountOrgName; }
            set { accountOrgName = value; }
        }
        /// <summary>
        /// 结算组织层次码
        /// </summary>
        public virtual string AccountOrgSyscode
        {
            get { return accountOrgSyscode; }
            set { accountOrgSyscode = value; }
        }
         
        /// <summary>
        /// 材质
        /// </summary>
        public virtual string MaterialStuff
        {
            get { return materialStuff; }
            set { materialStuff = value; }
        }

        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        /// <summary>
        /// 区分土建还是安装
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        /// <summary>
        /// 入库明细ID
        /// </summary>
        public virtual string StockInDtlId
        {
            get { return stockInDtlId; }
            set { stockInDtlId = value; }
        }

        /// <summary>
        /// 累计调拨出库红字金额
        /// </summary>
        public virtual decimal MoveOutRedAddMoney
        {
            get { return moveOutRedAddMoney; }
            set { moveOutRedAddMoney = value; }
        }

        /// <summary>
        /// 累计调拨出库红字数量
        /// </summary>
        public virtual decimal MoveOutRedAddQuantity
        {
            get { return moveOutRedAddQuantity; }
            set { moveOutRedAddQuantity = value; }
        }

        /// <summary>
        /// 本月调拨出库红字金额
        /// </summary>
        public virtual decimal MoveOutRedMoney
        {
            get { return moveOutRedMoney; }
            set { moveOutRedMoney = value; }
        }

        /// <summary>
        /// 本月调拨出库红字数量
        /// </summary>
        public virtual decimal MoveOutRedQuantity
        {
            get { return moveOutRedQuantity; }
            set { moveOutRedQuantity = value; }
        }

        /// <summary>
        /// 累计领料出库红字金额
        /// </summary>
        public virtual decimal SaleOutRedAddMoney
        {
            get { return saleOutRedAddMoney; }
            set { saleOutRedAddMoney = value; }
        }

        /// <summary>
        /// 累计领料出库红字数量
        /// </summary>
        public virtual decimal SaleOutRedAddQuantity
        {
            get { return saleOutRedAddQuantity; }
            set { saleOutRedAddQuantity = value; }
        }

        /// <summary>
        /// 本月领料出库红字金额
        /// </summary>
        public virtual decimal SaleOutRedMoney
        {
            get { return saleOutRedMoney; }
            set { saleOutRedMoney = value; }
        }

        /// <summary>
        /// 本月领料出库红字数量
        /// </summary>
        public virtual decimal SaleOutRedQuantity
        {
            get { return saleOutRedQuantity; }
            set { saleOutRedQuantity = value; }
        }

        /// <summary>
        /// 累计调拨入库红单金额
        /// </summary>
        public virtual decimal MoveInRedAddMoney
        {
            get { return moveInRedAddMoney; }
            set { moveInRedAddMoney = value; }
        }

        /// <summary>
        /// 累计调拨入库红单数量
        /// </summary>
        public virtual decimal MoveInRedAddQuantity
        {
            get { return moveInRedAddQuantity; }
            set { moveInRedAddQuantity = value; }
        }

        /// <summary>
        /// 本月调拨入库红单金额
        /// </summary>
        public virtual decimal MoveInRedMoney
        {
            get { return moveInRedMoney; }
            set { moveInRedMoney = value; }
        }

        /// <summary>
        /// 本月调拨入库红单数量
        /// </summary>
        public virtual decimal MoveInRedQuantity
        {
            get { return moveInRedQuantity; }
            set { moveInRedQuantity = value; }
        }

        /// <summary>
        /// 累计收料入库红单金额
        /// </summary>
        public virtual decimal BuyInRedAddMoney
        {
            get { return buyInRedAddMoney; }
            set { buyInRedAddMoney = value; }
        }

        /// <summary>
        ///  累计收料入库红单
        /// </summary>
        public virtual decimal BuyInRedAddQuantity
        {
            get { return buyInRedAddQuantity; }
            set { buyInRedAddQuantity = value; }
        }

        /// <summary>
        ///  本月收料入库红单金额
        /// </summary>
        public virtual decimal BuyInRedMoney
        {
            get { return buyInRedMoney; }
            set { buyInRedMoney = value; }
        }

        /// <summary>
        /// 本月收料入库红单
        /// </summary>
        public virtual decimal BuyInRedQuantity
        {
            get { return buyInRedQuantity; }
            set { buyInRedQuantity = value; }
        }

        /// <summary>
        /// 本月累计调拨出库金额
        /// </summary>
        public virtual decimal MoveOutAddMoney
        {
            get { return moveOutAddMoney; }
            set { moveOutAddMoney = value; }
        }

        /// <summary>
        /// 本月调拨出库金额
        /// </summary>
        public virtual decimal MoveOutMoney
        {
            get { return moveOutMoney; }
            set { moveOutMoney = value; }
        }

        /// <summary>
        /// 累计调拨入库金额
        /// </summary>
        public virtual decimal MoveInAddMoney
        {
            get { return moveInAddMoney; }
            set { moveInAddMoney = value; }
        }

        /// <summary>
        /// 本月调拨入库金额
        /// </summary>
        public virtual decimal MoveInMoney
        {
            get { return moveInMoney; }
            set { moveInMoney = value; }
        }

        /// <summary>
        /// 累计盘盈金额
        /// </summary>
        public virtual decimal ProfitInAddMoney
        {
            get { return profitInAddMoney; }
            set { profitInAddMoney = value; }
        }

        /// <summary>
        /// 本月盘盈金额
        /// </summary>
        public virtual decimal ProfitInMoney
        {
            get { return profitInMoney; }
            set { profitInMoney = value; }
        }

        /// <summary>
        /// 累计盘亏金额
        /// </summary>
        public virtual decimal LossOutAddMoney
        {
            get { return lossOutAddMoney; }
            set { lossOutAddMoney = value; }
        }

        /// <summary>
        /// 本月盘亏金额
        /// </summary>
        public virtual decimal LossOutMoney
        {
            get { return lossOutMoney; }
            set { lossOutMoney = value; }
        }

        /// <summary>
        /// 累计领料出库金额
        /// </summary>
        public virtual decimal SaleOutAddMoney
        {
            get { return saleOutAddMoney; }
            set { saleOutAddMoney = value; }
        }

        /// <summary>
        /// 本月领料出库金额
        /// </summary>
        public virtual decimal SaleOutMoney
        {
            get { return saleOutMoney; }
            set { saleOutMoney = value; }
        }

        /// <summary>
        /// 累计收料入库金额
        /// </summary>
        public virtual decimal BuyInAddMoney
        {
            get { return buyInAddMoney; }
            set { buyInAddMoney = value; }
        }

        /// <summary>
        /// 本月收料入库金额
        /// </summary>
        public virtual decimal BuyInMoney
        {
            get { return buyInMoney; }
            set { buyInMoney = value; }
        }

        /// <summary>
        /// 本月金额
        /// </summary>
        public virtual decimal NowMoney
        {
            get { return nowMoney; }
            set { nowMoney = value; }
        }

        /// <summary>
        /// 本月红字出库金额
        /// </summary>
        public virtual decimal NowOutRedMoney
        {
            get { return nowOutRedMoney; }
            set { nowOutRedMoney = value; }
        }

        /// <summary>
        /// 本月出库金额
        /// </summary>
        public virtual decimal NowOutMoney
        {
            get { return nowOutMoney; }
            set { nowOutMoney = value; }
        }

        /// <summary>
        /// 本月红字入库金额
        /// </summary>
        public virtual decimal NowInRedMoney
        {
            get { return nowInRedMoney; }
            set { nowInRedMoney = value; }
        }

        /// <summary>
        /// 本月入库金额
        /// </summary>
        public virtual decimal NowInMoney
        {
            get { return nowInMoney; }
            set { nowInMoney = value; }
        }

        /// <summary>
        /// 期初金额
        /// </summary>
        public virtual decimal LastMoney
        {
            get { return lastMoney; }
            set { lastMoney = value; }
        }

        /// <summary>
        /// 计量单位名称
        /// </summary>
        public virtual string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string MatSpec
        {
            get { return matSpec; }
            set { matSpec = value; }
        }

        /// <summary>
        /// 物资名称
        /// </summary>
        public virtual string MatName
        {
            get { return matName; }
            set { matName = value; }
        }

        /// <summary>
        /// 物资编码
        /// </summary>
        public virtual string MatCode 
        {
            get { return matCode; }
            set { matCode = value; }
        }

        /// <summary>
        /// 累计盘盈数量
        /// </summary>
        virtual public decimal ProfitInAddQuantity
        {
            get { return profitInAddQuantity; }
            set { profitInAddQuantity = value; }
        }

        /// <summary>
        /// 累计盘亏数量
        /// </summary>
        virtual public decimal LossOutAddQuantity
        {
            get { return lossOutAddQuantity; }
            set { lossOutAddQuantity = value; }
        }

        /// <summary>
        /// 累计收料入库数量
        /// </summary>
        virtual public decimal BuyInAddQuantity
        {
            get { return buyInAddQuantity; }
            set { buyInAddQuantity = value; }
        }

        /// <summary>
        /// 累计领料出库数量
        /// </summary>
        virtual public decimal SaleOutAddQuantity
        {
            get { return saleOutAddQuantity; }
            set { saleOutAddQuantity = value; }
        }

        /// <summary>
        /// 累计调拨入库数量
        /// </summary>
        virtual public decimal MoveInAddQuantity
        {
            get { return moveInAddQuantity; }
            set { moveInAddQuantity = value; }
        }

        /// <summary>
        /// 累计调拨出库数量
        /// </summary>
        virtual public decimal MoveOutAddQuantity
        {
            get { return moveOutAddQuantity; }
            set { moveOutAddQuantity = value; }
        }

        /// <summary>
        /// 调拨出库
        /// </summary>
        virtual public decimal MoveOutQuantity
        {
            get { return moveOutQuantity; }
            set { moveOutQuantity = value; }
        }

        /// <summary>
        /// 调拨入库
        /// </summary>
        virtual public decimal MoveInQuantity
        {
            get { return moveInQuantity; }
            set { moveInQuantity = value; }
        }

        /// <summary>
        /// 销售出库量
        /// </summary>
        virtual public decimal SaleOutQuantity
        {
            get { return saleOutQuantity; }
            set { saleOutQuantity = value; }
        }

        /// <summary>
        /// 本期采购入库量
        /// </summary>
        virtual public decimal BuyInQuantity
        {
            get { return buyInQuantity; }
            set { buyInQuantity = value; }
        }

        /// <summary>
        /// 盘亏出库
        /// </summary>
        virtual public decimal LossOutQuantity
        {
            get { return lossOutQuantity; }
            set { lossOutQuantity = value; }
        }

        /// <summary>
        /// 盘盈入库
        /// </summary>
        virtual public decimal ProfitInQuantity
        {
            get { return profitInQuantity; }
            set { profitInQuantity = value; }
        }

        /// <summary>
        /// 本月结存量
        /// </summary>
        virtual public decimal NowQuantity
        {
            get { return nowQuantity; }
            set { nowQuantity = value; }
        }
        /// <summary>
        /// 本月发出冲红量
        /// </summary>
        virtual public decimal NowOutRedQuantity
        {
            get { return nowOutRedQuantity; }
            set { nowOutRedQuantity = value; }
        }

        /// <summary>
        /// 本月发出量
        /// </summary>
        virtual public decimal NowOutQuantity
        {
            get { return nowOutQuantity; }
            set { nowOutQuantity = value; }
        }

        /// <summary>
        /// 本月收入冲红量
        /// </summary>
        virtual public decimal NowInRedQuantity
        {
            get { return nowInRedQuantity; }
            set { nowInRedQuantity = value; }
        }

        /// <summary>
        /// 本月收入量
        /// </summary>
        virtual public decimal NowInQuantity
        {
            get { return nowInQuantity; }
            set { nowInQuantity = value; }
        }

        /// <summary>
        /// 上月结存量
        /// </summary>
        virtual public decimal LastQuantity
        {
            get { return lastQuantity; }
            set { lastQuantity = value; }
        }


        /// <summary>
        /// 仓库
        /// </summary>
        virtual public StationCategory TheStationCategory
        {
            get { return theStationCategory; }
            set { theStationCategory = value; }
        }

        /// <summary>
        /// 物料
        /// </summary>
        virtual public Material TheMaterial
        {
            get { return theMaterial; }
            set { theMaterial = value; }
        }
        /// <summary>
        /// 结帐类型
        /// </summary>
        virtual public int AcctType
        {
            get { return acctType; }
            set { acctType = value; }
        }
        /// <summary>
        /// 会计月
        /// </summary>
        virtual public int FiscalMonth
        {
            get { return fiscalMonth; }
            set { fiscalMonth = value; }
        }
        /// <summary>
        ///  会计年
        /// </summary>
        virtual public int FiscalYear
        {
            get { return fiscalYear; }
            set { fiscalYear = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
