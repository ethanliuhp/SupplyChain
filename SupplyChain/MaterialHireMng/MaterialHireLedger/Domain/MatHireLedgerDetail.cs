using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain
{
    /// <summary>
    /// 料具租赁台账明细
    /// </summary>

    [Serializable]
    public class MatHireLedgerDetail : BaseDetail
    {
        private SupplierRelationInfo theRank;
        private string theRankName;
        private string matCollReturnDtlId;
        private int tempIndex;
        private string type;

        /// <summary>
        /// 临时ID，不做Map
        /// </summary>
        public virtual int TempIndex
        {
            get { return tempIndex; }
            set { tempIndex = value; }
        }
        /// <summary>
        /// 队伍/供应商
        /// </summary>
        virtual public SupplierRelationInfo TheRank
        {
            get { return theRank; }
            set { theRank = value; }
        }
        /// <summary>
        /// 队伍/供应商名称
        /// </summary>
        virtual public string TheRankName
        {
            get { return theRankName; }
            set { theRankName = value; }
        }
        /// <summary>
        /// 收退料明细GUID
        /// </summary>
        virtual public string MatCollReturnDtlId
        {
            get { return matCollReturnDtlId; }
            set { matCollReturnDtlId = value; }
        }
        /// <summary>
        /// 类型(收料，完好退料，报废退料，消耗退料，丢失退料)
        /// </summary>
        virtual public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
