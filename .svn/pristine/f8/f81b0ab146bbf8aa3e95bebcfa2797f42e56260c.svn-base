using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSupply.Domain
{
    /// <summary>
    /// 采购合同 项目信息
    /// </summary>
    [Serializable]
    public class MatHireSupplyOrderProjectDetail
    {
        private string id;
        private MatHireSupplyOrderMaster master;
        private CurrentProjectInfo projectId;
        private string projectName;

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public virtual CurrentProjectInfo ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        public virtual MatHireSupplyOrderMaster Master
        {
            get { return master; }
            set { master = value; }
        }

        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
