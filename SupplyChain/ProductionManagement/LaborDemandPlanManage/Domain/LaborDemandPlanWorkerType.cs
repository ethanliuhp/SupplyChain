using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain
{
    /// <summary>
    /// 工种明细
    /// </summary>
    [Serializable]
    public class LaborDemandWorkerType
    {

        private string id;
        private long version;
        private int peopleNum;
        private string workerType;
        private LaborDemandPlanDetail master;

        virtual public LaborDemandPlanDetail Master
        {
            get { return master; }
            set { master = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public virtual long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 工种
        /// </summary>
        virtual public string WorkerType
        {
            get { return workerType; }
            set { workerType = value; }
        }
        /// <summary>
        /// 所需人数
        /// </summary>
        virtual public int PeopleNum
        {
            get { return peopleNum; }
            set { peopleNum = value; }
        }
    }
}
