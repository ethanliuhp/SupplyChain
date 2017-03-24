using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain
{
    /// <summary>
    /// 需求总计划主表
    /// </summary>
    [Serializable]
    public class DemandMasterPlanMaster : BaseSupplyMaster
    {
        private string according;
        private string special;
        private string specialType;
        private string planName;
 
        ///<summary>
        /// 专业分类
        /// </summary>
        virtual public string SpecialType
        {
            get { return specialType; }
            set { specialType = value; }
        }
        /// <summary>
        /// 专业 用于区分单据
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }
        /// <summary>
        /// 编制依据
        /// </summary>
        virtual public string According
        {
            get { return according; }
            set { according = value; }
        }
        /// <summary>
        /// 计划名称
        /// </summary>
        public virtual string PlanName
        {
            get { return planName; }
            set { planName = value; }
        }
    }
}
