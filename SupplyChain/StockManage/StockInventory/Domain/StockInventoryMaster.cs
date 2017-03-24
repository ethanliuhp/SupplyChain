using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain
{
    /// <summary>
    /// 月度盘点主表
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInventoryMaster : BaseMaster
    {
        private StationCategory stockId;
        private string inventoryAddress;
        private decimal sumQuantity;
        private MaterialCategory materialCategory;
        private string matCatName;
        private string special;
        private SupplierRelationInfo usedRank;
        private string usedRankName;
        private string professionCategory;
        private GWBSTree userPart;
        private string userPartName;
        private string userPartSysCode;
        /// <summary>
        /// 使用部位名称
        /// </summary>
        public virtual string UserPartName
        {
            get { return userPartName; }
            set { userPartName = value; }
        }
        /// <summary>
        /// 使用部位的
        /// </summary>
        public virtual GWBSTree UserPart
        {
            get { return userPart; }
            set { userPart = value; }
        }
        /// <summary>
        /// 使用部位层次码
        /// </summary>
        public virtual string UserPartSysCode
        {
            get { return userPartSysCode; }
            set { userPartSysCode = value; }
        }
        public virtual string ProfessionCategory
        {
            get { return professionCategory; }
            set { professionCategory = value; }
        }
        public virtual SupplierRelationInfo UsedRank
        {
            get { return usedRank; }
            set { usedRank = value; }
        }
        public virtual string UsedRankName
        {
            get { return usedRankName; }
            set { usedRankName = value; }
        }
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public virtual StationCategory StockId
        {
            get { return stockId; }
            set { stockId = value; }
        }

        /// <summary>
        /// 盘点地点
        /// </summary>
        public virtual string InventoryAddress
        {
            get { return inventoryAddress; }
            set { inventoryAddress = value; }
        }
        /// <summary>
        /// 总数量
        /// </summary>
        public virtual decimal SumQuantity
        {
            get
            {
                decimal tmpQuantity = 0;
                //汇总
                foreach (StockInventoryDetail var in Details)
                {
                    tmpQuantity += var.InventoryQuantity;
                }
                sumQuantity = tmpQuantity;
                return sumQuantity;
            }
            set { sumQuantity = value; }
        }

        /// <summary>
        /// 物资分类名称
        /// </summary>
        public virtual string MatCatName
        {
            get { return matCatName; }
            set { matCatName = value; }
        }

        /// <summary>
        /// 物资分类
        /// </summary>
        public virtual MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }


    }
}
