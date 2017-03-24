using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain
{
    public enum EnumDocState
    {
        [Description("编制")]
        编制 = 0,
        [Description("提交")]
        提交 = 1,
        [Description("发布")]
        发布 = 2,
    }
    /// <summary>
    /// 商务策划书
    /// </summary>
    [Serializable]
    public class BusinessProposal : BaseMaster
    {                    
        private DateTime submitDate;  //提交时间                     
        private string enginnerName;  //专业策划书名称          
        private decimal projectCost;  //工程造价
        private StandardUnit priceUnit;  //价格计量单位

        /// <summary>
        /// 提交时间
        /// </summary>
        virtual public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }
        /// <summary>
        /// 专业策划书名称
        /// </summary>
        virtual public string EnginnerName
        {
            get { return enginnerName; }
            set { enginnerName = value; }
        }
        /// <summary>
        /// 工程造价（万元）
        /// </summary>
        virtual public decimal ProjectCost
        {
            get { return projectCost; }
            set { projectCost = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }
    }
}
