using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.Util;
namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain
{
    /// <summary>
    /// 料具退料单明细
    /// </summary>
    [Serializable]
    public class MatHireReturnDetail : BaseDetail
    {
        private decimal rejectQuantity;
        private decimal allocateQuantity;
        private decimal loseQuantity;
        private decimal borrowQuantity;
        private decimal exitQuantity;
        private decimal broachQuantity;
        private decimal projectQuantity;
        private decimal consumeQuantity;
        private SupplierRelationInfo borrowUnit;
        private string borrowUnitName;
        private string balRule;
        private decimal rentalPrice;
        private decimal disCardQty;
        private decimal repairQty;
        private decimal lossQty;

        private CostAccountSubject subjectGUID;
        private string subjectName;
        private string subjectSysCode;
        private decimal materialLength;
        private string materialType;
        private decimal beforeStockQty = 0;
        /// <summary>
        /// 收料前库存数
        /// </summary>
        virtual public decimal BeforeStockQty
        {
            get { return beforeStockQty; }
            set { beforeStockQty = value; }
        }
        /// <summary>
        /// 物资长度
        /// </summary>
        public virtual decimal MaterialLength
        {
            get { return materialLength; }
            set { materialLength = value; }
        }
        /// <summary>
        /// 核算科目
        /// </summary>
        virtual public CostAccountSubject SubjectGUID
        {
            get { return subjectGUID; }
            set { subjectGUID = value; }
        }
        /// <summary>
        /// 核算科目名称
        /// </summary>
        virtual public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        /// <summary>
        /// 核算科目层次码
        /// </summary>
        virtual public string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }

        private Iesi.Collections.Generic.ISet<MatHireReturnDetailSeq> materialReturnDetailSeqs = new Iesi.Collections.Generic.HashedSet<MatHireReturnDetailSeq>();
        virtual public Iesi.Collections.Generic.ISet<MatHireReturnDetailSeq> MaterialReturnDetailSeqs
        {
            get { return materialReturnDetailSeqs; }
            set { materialReturnDetailSeqs = value; }
        }

        /// <summary>
        /// 增加退料时序
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatReturnDtlSeq(MatHireReturnDetailSeq detail)
        {
            detail.MatReturnDtlMaster = this;
            MaterialReturnDetailSeqs.Add(detail);
        }

        private Iesi.Collections.Generic.ISet<MatHireReturnCostDtl> matReturnCostDtls = new Iesi.Collections.Generic.HashedSet<MatHireReturnCostDtl>();
        virtual public Iesi.Collections.Generic.ISet<MatHireReturnCostDtl> MatReturnCostDtls
        {
            get { return matReturnCostDtls; }
            set { matReturnCostDtls = value; }
        }

        /// <summary>
        /// 增加退料费用明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatReturnCostDtls(MatHireReturnCostDtl detail)
        {
            detail.Master = this;
            MatReturnCostDtls.Add(detail);
        }

        private Iesi.Collections.Generic.ISet<MatHireRepair> matRepairs = new Iesi.Collections.Generic.HashedSet<MatHireRepair>();
        virtual public Iesi.Collections.Generic.ISet<MatHireRepair> MatRepairs
        {
            get { return matRepairs; }
            set { matRepairs = value; }
        }

        /// <summary>
        /// 增加维修内容
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatRepairs(MatHireRepair detail)
        {
            detail.Master = this;
            MatRepairs.Add(detail);
        }

        /// <summary>
        /// 报废数量
        /// </summary>
        virtual public decimal RejectQuantity
        {
            get { return rejectQuantity; }
            set { rejectQuantity = value; }
        }
        /// <summary>
        /// 调拨数量
        /// </summary>
        virtual public decimal AllocateQuantity
        {
            get { return allocateQuantity; }
            set { allocateQuantity = value; }
        }
        /// <summary>
        /// 丢失数量
        /// </summary>
        virtual public decimal LoseQuantity
        {
            get { return loseQuantity; }
            set { loseQuantity = value; }
        }
        /// <summary>
        /// 借用数量
        /// </summary>
        virtual public decimal BorrowQuantity
        {
            get { return borrowQuantity; }
            set { borrowQuantity = value; }
        }
        /// <summary>
        /// 退场数量
        /// </summary>
        virtual public decimal ExitQuantity
        {
            get { return exitQuantity; }
            set { exitQuantity = value; }
        }
        /// <summary>
        /// 完好数量
        /// </summary>
        virtual public decimal BroachQuantity
        {
            get { return broachQuantity; }
            set { broachQuantity = value; }
        }
        /// <summary>
        /// 项目部数量
        /// </summary>
        virtual public decimal ProjectQuantity
        {
            get { return projectQuantity; }
            set { projectQuantity = value; }
        }
        /// <summary>
        /// 消耗数量
        /// </summary>
        virtual public decimal ConsumeQuantity
        {
            get { return consumeQuantity; }
            set { consumeQuantity = value; }
        }
        /// <summary>
        /// 借用单位
        /// </summary>
        virtual public SupplierRelationInfo BorrowUnit
        {
            get { return borrowUnit; }
            set { borrowUnit = value; }
        }
        /// <summary>
        /// 借用单位名称
        /// </summary>
        virtual public string BorrowUnitName
        {
            get { return borrowUnitName; }
            set { borrowUnitName = value; }
        }
        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
        }
        /// <summary>
        /// 租赁单价
        /// </summary>
        virtual public decimal RentalPrice
        {
            get { return rentalPrice; }
            set { rentalPrice = value; }
        }
        /// <summary>
        /// 切头数量
        /// </summary>
        virtual public decimal DisCardQty
        {
            get { return disCardQty; }
            set { disCardQty = value; }
        }
        /// <summary>
        /// 维修数量
        /// </summary>
        virtual public decimal RepairQty
        {
            get { return repairQty; }
            set { repairQty = value; }
        }
        /// <summary>
        /// 报损数量
        /// </summary>
        virtual public decimal LossQty
        {
            get { return lossQty; }
            set { lossQty = value; }
        }
        /// <summary>
        /// 碗扣型号
        /// </summary>
        virtual public string MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
        /// <summary>
        /// 实际数量 ExitQuantity*物资长度
        /// </summary>
        virtual public decimal RealExitQuantity
        {
            get { return ExitQuantity * MaterialLength; }
        }
        /// <summary>
        /// 实际临时值  临时数量*物资长度
        /// </summary>
        virtual public decimal RealTempData
        {
            get { return MaterialLength *ClientUtil.ToDecimal( TempData); }
        }
    }
}
