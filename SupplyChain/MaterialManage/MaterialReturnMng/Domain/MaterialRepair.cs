using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain
{
    /// <summary>
    /// 料具维修
    /// </summary>
    [Serializable]
    public class MaterialRepair : BaseDetail
    {
        private string workContent;
        private MaterialReturnDetail master;
        /// <summary>
        /// 工作内容
        /// </summary>
        virtual public string WorkContent
        {
            get { return workContent; }
            set { workContent = value; }
        }
        /// <summary>
        /// 料具退料明细(GUID)
        /// </summary>
        virtual public MaterialReturnDetail Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
