using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.CompleteSettlementBook.Domain
{
    /// <summary>
    /// 竣工结算书表
    /// </summary>
    [Serializable]
   public class CompleteInfo:BaseMaster
    {
        //id
        //PROJECTNAME
        //STATE

        private decimal submitMoney;
        private decimal beginMoney;
        private string priceDanWei;
        private StandardUnit priceDWid;
        private DateTime planTime;
        private DateTime endTime;
        private decimal sureMoney;
        private decimal shendingMoney;
        private decimal realMoney;
        //private string projectid;
        //private string accountName;
        private decimal benefit;
        private decimal bennefitlv;
        //private PersonInfo person;
        //private string personName;
        //private string personTeam;
        private decimal zhenquMoney;
        private string contractDocName;

        /// <summary>
        /// 文档名称
        /// </summary>
        virtual public string ContractDocName
        {
            get { return contractDocName; }
            set { contractDocName = value; }
        }
        
        /// <summary>
        /// 报送总金额
        /// </summary>
        virtual public decimal SubmitMoney
        {
            get { return submitMoney; }
            set { submitMoney = value; }
        }

        
        /// <summary>
        /// 初次审定总金额
        /// </summary>
        virtual public decimal BeginMoney
        {
            get { return beginMoney; }
            set { beginMoney = value; }
        }
       
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public string PriceDanWei
        {
            get { return priceDanWei; }
            set { priceDanWei = value; }
        }
       
        /// <summary>
        /// 价格计量单位GUID
        /// </summary>
        virtual public StandardUnit PriceDWid
        {
            get { return priceDWid; }
            set { priceDWid = value; }
        }
       
       
        /// <summary>
        /// 结算计划完成时间
        /// </summary>
        virtual public DateTime PlanTime
        {
            get { return planTime; }
            set { planTime = value; }
        }
        
        /// <summary>
        /// 结算实际完成时间
        /// </summary>
        virtual public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        
        /// <summary>
        /// 确保结算金额
        /// </summary>
        virtual public decimal SureMoney
        {
            get { return sureMoney; }
            set { sureMoney = value; }
        }
       
        /// <summary>
        /// 审定总金额
        /// </summary>
        virtual public decimal ShendingMoney
        {
            get { return shendingMoney; }
            set { shendingMoney = value; }
        }
       
        /// <summary>
        /// 实际成本
        /// </summary>
        virtual public decimal RealMoney
        {
            get { return realMoney; }
            set { realMoney = value; }
        }
       
        
        ///// <summary>
        ///// 项目GUID
        ///// </summary>
        //virtual public string Projectid
        //{
        //    get { return projectid; }
        //    set { projectid = value; }
        //}



        ///// <summary>
        ///// 项目名称
        ///// </summary>
        //virtual public string AccountName
        //{
        //    get { return accountName; }
        //    set { accountName = value; }
        //}
       
        /// <summary>
        /// 效益额
        /// </summary>
        virtual public decimal Benefit
        {
            get { return benefit; }
            set { benefit = value; }
        }
        
        /// <summary>
        /// 效益率
        /// </summary>
        virtual public decimal Bennefitlv
        {
            get { return bennefitlv; }
            set { bennefitlv = value; }
        }
       
        ///// <summary>
        ///// 责任人
        ///// </summary>
        //virtual public PersonInfo Person
        //{
        //    get { return person; }
        //    set { person = value; }
        //}
        
        /// <summary>
        /// 责任人名称
        /// </summary>
        //virtual public string PersonName
        //{
        //    get { return personName; }
        //    set { personName = value; }
        //}
       
        ///// <summary>
        ///// 责任人组织层次码
        ///// </summary>
        //virtual public string PersonTeam
        //{
        //    get { return personTeam; }
        //    set { personTeam = value; }
        //}
        
        /// <summary>
        /// 争取结算金额
        /// </summary>
        virtual public decimal ZhenquMoney
        {
            get { return zhenquMoney; }
            set { zhenquMoney = value; }
        }
      

   }
}
