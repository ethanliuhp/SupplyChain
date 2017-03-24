using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Domain
{
    /// <summary>
    /// 施工专业基础数据
    /// </summary>
    [Serializable]
    public class ConstructionData : BaseMaster
    {
        private string inspectionType;
        private string inspectionContent;
        private int serailNum;
        private string specail;

        /// <summary>
        /// 检查类型
        /// </summary>
        virtual public string InspectionType
        {
            get { return inspectionType; }
            set { inspectionType = value; }
        }
        /// <summary>
        /// 检查内容
        /// </summary>
        virtual public string InspectionContent
        {
            get { return inspectionContent; }
            set { inspectionContent = value; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        virtual public int SerailNum
        {
            get { return serailNum; }
            set { serailNum = value; }
        }
        /// <summary>
        /// 专业
        /// </summary>
        virtual public string Specail
        {
            get { return specail; }
            set { specail = value; }
        }

    }
}
