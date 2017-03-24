using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.BasicData.UnitMng.Domain
{
    /// <summary>
    /// 计量单位明细
    /// </summary>
    [Serializable]
    public class UnitDetail : BaseDetail
    {
        private StandardUnit unitId;
        private string unitName;
        private Dimension dimensionId;
        private string dimensionName;
        
        ///<summary>
        /// 计量单位
        ///</summary>
        virtual public StandardUnit UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }
        ///<summary>
        ///计量单位名称
        ///</summary>
        virtual public string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }
        ///<summary>
        ///量纲
        ///</summary>
        virtual public Dimension DimensionId
        {
            get { return dimensionId; }
            set { dimensionId = value; }
        }
        ///<summary>
        ///量纲名称
        ///</summary>
        virtual public string DimensionName
        {
            get { return dimensionName; }
            set { dimensionName = value; }
        }
    }
}
