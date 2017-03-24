using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain
{
    /// <summary>
    /// 整改通知单主表
    /// </summary>
    [Serializable]
    public class RectificationNoticeMaster : BaseMaster
    {
        private SubContractProject supplierUnit;
        private string supplierUnitName;
        private int inspectionType;
        private int isCreate = 0;
        private string tempDebitId;

        virtual public int IsCreate
        {
            get { return isCreate; }
            set { isCreate = value; }
        }
        /// <summary>
        /// 承担单位
        /// </summary>
        virtual public SubContractProject SupplierUnit
        {
            get { return supplierUnit; }
            set { supplierUnit = value; }
        }
        /// <summary>
        /// 承担单位名称
        /// </summary>
        virtual public string SupplierUnitName
        {
            get { return supplierUnitName; }
            set { supplierUnitName = value; }
        }
        /// <summary>
        /// 检查类型
        /// </summary>
        virtual public int InspectionType
        {
            get { return inspectionType; }
            set { inspectionType = value; }
        }
        /// <summary>
        /// 暂扣款Id
        /// </summary>
        virtual public string TempDebitId
        {
            get { return tempDebitId; }
            set { tempDebitId = value; }
        }
    }
}
