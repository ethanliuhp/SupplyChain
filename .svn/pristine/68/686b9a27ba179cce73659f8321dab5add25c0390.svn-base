using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 非实体部分成本取费费率维护
    /// </summary>
    [Serializable]
    public class GWBSDtlCostSubRate
    {
        private string _id;
        ///<summary>主键</summary>
        public virtual string Id
        {
            set { this._id = value; }
            get { return this._id; }
        }

        private GWBSDetailCostSubject _master;
        ///<summary>parent</summary>
        public virtual GWBSDetailCostSubject Master
        {
            set { this._master = value; }
            get { return this._master; }
        }

        private SelFeeDtl _selfeedtl;
        ///<summary></summary>
        public virtual SelFeeDtl SelFeelDtl
        {
            set { this._selfeedtl = value; }
            get { return this._selfeedtl; }
        }

        private decimal _rate;
        ///<summary></summary>
        public virtual decimal Rate
        {
            set { this._rate = value; }
            get { return this._rate; }
        }

        public virtual GWBSDtlCostSubRate Clone(GWBSDetailCostSubject objParent)
        {
            GWBSDtlCostSubRate objRate = this.MemberwiseClone() as GWBSDtlCostSubRate;
            objRate.Id = null;
            objRate.Master = objParent;
            return objRate;
        }
    }
}
