using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain
{
    /// <summary>
    /// 风险抵押金缴纳记录
    /// </summary>
    [Serializable]
    public class IrpRiskDepositPayRecord : BaseDetail
    {
        //private string id;
        private string prickleName;   //计量单位名称
        private StandardUnit pricePrickle;   //价格计量单位
        private TargetRespBook targetRespBookGuid;   //目标责任书
        private decimal paidinAmount;   //实缴额
        private DateTime paidinDate;   //实缴日期
        private string projectPosition;   //项目职务
        private string projectLevelPosition;   //项目职务等级
        private decimal payAble;  //应缴额
        private string responsibleName;   //责任人
        private string responsibleId;   //责任人GUID
        private string name;
        
        ///// <summary>
        ///// id
        ///// </summary>
        //virtual public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        /// <summary>
        /// 计量单位名称
        /// </summary>
        virtual public string PrickleName
        {
            get { return prickleName; }
            set { prickleName = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit PricePrickle
        {
            get { return pricePrickle; }
            set { pricePrickle = value; }
        }
        /// <summary>
        /// 目标责任书
        /// </summary>
        virtual public TargetRespBook TargetRespBookGuid
        {
            get { return targetRespBookGuid; }
            set { targetRespBookGuid = value; }
        }
        /// <summary>
        /// 实缴额
        /// </summary>
        virtual public decimal PaidinAmount
        {
            get { return paidinAmount; }
            set { paidinAmount = value; }
        }
        /// <summary>
        /// 实缴日期
        /// </summary>
        virtual public DateTime PaidinDate
        {
            get { return paidinDate; }
            set { paidinDate = value; }
        }
        /// <summary>
        /// 项目职务
        /// </summary>
        virtual public string ProjectPosition
        {
            get { return projectPosition; }
            set { projectPosition = value; }
        }
        /// <summary>
        /// 项目职务级别
        /// </summary>
        virtual public string ProjectLevelPosition
        {
            get { return projectLevelPosition; }
            set { projectLevelPosition = value; }
        }       
        /// <summary>
        /// 应缴额
        /// </summary>
        virtual public decimal PayAble
        {
            get { return payAble; }
            set { payAble = value; }
        }       
        /// <summary>
        /// 责任人
        /// </summary>
        virtual public string ResponsibleName
        {
            get { return responsibleName; }
            set { responsibleName = value; }
        }
        /// <summary>
        /// 责任人GUID
        /// </summary>
        virtual public string ResponsibleId
        {
            get { return responsibleId; }
            set { responsibleId = value; }
        }
        /// <summary>
        /// 名字
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
