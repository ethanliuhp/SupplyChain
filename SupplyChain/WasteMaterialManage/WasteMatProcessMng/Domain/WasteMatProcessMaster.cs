using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain
{
    /// <summary>
    /// 废旧物资处理单主表
    /// </summary>
    [Serializable]
    public class WasteMatProcessMaster : BaseMaster
    {
        private string attachmentDocPath;
        private SupplierRelationInfo recycleUnit;
        private string recycleUnitName;
        private decimal sumQuantity;
        private decimal sumMoney;
        private string monthConsumeId;

        /// <summary>
        /// 月度实际耗用ID
        /// </summary>
        public virtual string MonthConsumeId
        {
            get { return monthConsumeId; }
            set { monthConsumeId = value; }
        }

        /// <summary>
        /// 数量汇总
        /// </summary>
        virtual public decimal SumQuantity
        {
            get
            {
                decimal tmpQuantity = 0;
                //汇总
                foreach (WasteMatProcessDetail var in Details)
                {
                    tmpQuantity += var.NetWeight;
                }
                sumQuantity = tmpQuantity;
                return sumQuantity;
            }
            set { sumQuantity = value; }
        }

        /// <summary>
        /// 金额汇总
        /// </summary>
        virtual public decimal SumMoney
        {
            get
            {
                decimal temMoney = 0;
                //汇总
                foreach (WasteMatProcessDetail var in Details)
                {
                    temMoney += var.TotalValue;
                }
                sumMoney = temMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }
        //施工专业基础表

        /// <summary>
        /// 附件路径
        /// </summary>
        virtual public string AttachmentDocPath
        {
            get { return attachmentDocPath; }
            set { attachmentDocPath = value; }
        }
        /// <summary>
        /// 回收单位
        /// </summary>
        virtual public SupplierRelationInfo RecycleUnit
        {
            get { return recycleUnit; }
            set { recycleUnit = value; }
        }
        /// <summary>
        /// 回收单位名称
        /// </summary>
        virtual public string RecycleUnitName
        {
            get { return recycleUnitName; }
            set { recycleUnitName = value; }
        }
    }
}
