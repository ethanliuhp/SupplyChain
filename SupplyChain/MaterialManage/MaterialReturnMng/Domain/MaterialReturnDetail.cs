using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain
{
    /// <summary>
    /// 料具退料单明细
    /// </summary>
    [Serializable]
    public class MaterialReturnDetail : BaseDetail
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

        private Iesi.Collections.Generic.ISet<MaterialReturnDetailSeq> materialReturnDetailSeqs = new Iesi.Collections.Generic.HashedSet<MaterialReturnDetailSeq>();
        virtual public Iesi.Collections.Generic.ISet<MaterialReturnDetailSeq> MaterialReturnDetailSeqs
        {
            get { return materialReturnDetailSeqs; }
            set { materialReturnDetailSeqs = value; }
        }

        /// <summary>
        /// 增加退料时序
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatReturnDtlSeq(MaterialReturnDetailSeq detail)
        {
            detail.MatReturnDtlMaster = this;
            MaterialReturnDetailSeqs.Add(detail);
        }

        private Iesi.Collections.Generic.ISet<MaterialReturnCostDtl> matReturnCostDtls = new Iesi.Collections.Generic.HashedSet<MaterialReturnCostDtl>();
        virtual public Iesi.Collections.Generic.ISet<MaterialReturnCostDtl> MatReturnCostDtls
        {
            get { return matReturnCostDtls; }
            set { matReturnCostDtls = value; }
        }

        /// <summary>
        /// 增加退料费用明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatReturnCostDtls(MaterialReturnCostDtl detail)
        {
            detail.Master = this;
            MatReturnCostDtls.Add(detail);
        }

        private Iesi.Collections.Generic.ISet<MaterialRepair> matRepairs = new Iesi.Collections.Generic.HashedSet<MaterialRepair>();
        virtual public Iesi.Collections.Generic.ISet<MaterialRepair> MatRepairs
        {
            get { return matRepairs; }
            set { matRepairs = value; }
        }

        /// <summary>
        /// 增加维修内容
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatRepairs(MaterialRepair detail)
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
            get { return borrowQuantity; }
            set { borrowQuantity = value; }
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
    }
}
