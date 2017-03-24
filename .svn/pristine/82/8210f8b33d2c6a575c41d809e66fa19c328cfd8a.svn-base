using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain
{
    /// <summary>
    /// 料具维修
    /// </summary>
    [Serializable]
    public class MatHireRepair : BaseDetail
    {
        private string workContent;
        private MatHireReturnDetail master;
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
        virtual public MatHireReturnDetail Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
