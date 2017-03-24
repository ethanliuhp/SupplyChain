using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using VirtualMachine.Core.Attributes;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain
{
    /// <summary>
    /// 检验批
    /// </summary>
    [Serializable]
    [Entity]
    public class InspectionLotMaster : BaseMaster
    {
        private DateTime responseDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime stateSetDate = StringUtil.StrToDateTime("1900-01-01");

        //private Iesi.Collections.Generic.ISet<InspectionLotDetail> details = new Iesi.Collections.Generic.HashedSet<InspectionLotDetail>();
        ///// <summary>
        ///// 检验明细
        ///// </summary>
        //public virtual ISet<InspectionLotDetail> Details
        //{
        //    get { return details; }
        //    set { details = value; }
        //}
        /// <summary>
        /// 增加明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddInspectionLotDetail(InspectionLotDetail detail)
        {
            detail.Master = this;
            Details.Add(detail);
        }
        /// <summary>
        /// 回复时间
        /// </summary>
        virtual public DateTime ResponseDate
        {
            get { return responseDate; }
            set { responseDate = value; }
        }
        /// <summary>
        /// 状态设置时间
        /// </summary>
        virtual public DateTime StateSetDate
        {
            get { return stateSetDate; }
            set { stateSetDate = value; }
        }

    }
}
