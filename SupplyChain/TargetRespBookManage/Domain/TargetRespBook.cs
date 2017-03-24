using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.TargetRespBook.Domain
{
    /// <summary>
    /// 目标责任书
    /// </summary>
    [Serializable]
    public class TargetRespBook : BaseMaster
    {
        //private string id;    //id
        private string safetycivilizedsign;    //安全文明施工目标
        private decimal installationfreerate;    //安装施工部分配合费比率
        private decimal costcontrolrewardtatio;    //成本控制奖励比率
        private decimal costcontroltarget;    //成本控制目标
        private decimal cashrewardnodenumber;    //兑现奖励节点数量
        private string riskpaymentstate;    //风险抵押金缴纳状况
        private decimal riskrewardratio;   //风险化解奖励比率
        private decimal riskdissolvestarget;   //风险化解目标
        private string projecttime;    //工期
        private string periodmeasuringunit;    //工期计量单位
        private DateTime planenddate;    //计划竣工时间
        private DateTime planbegindate;    //计划开始时间
        private string pricklename;    //计量单位名称
        private string priceprickle;    //价格计量单位
        private string economicgoalenginner;    //经济目标责任范围不含工程
        private DateTime signtime;    //签订时间
        private string ensurelevel;   //确保工程质量等级
        private string signedwhether;    //是否签订
        private string documentname;   //文档名称 
        private string projectid;    //项目GUID 
        private string projectseale;    //项目规模
        private string projectmanagername;    //项目经理
        private string projectmanagerid;    //项目经理GUID
        private string projectname;    //项目名称
        private decimal servicefeerates;    //业务指定分包费用自提比率
        private decimal ownerawardsratio;    //业主奖励自提比率
        private decimal responsibilityratio;    //责任范围外分包工程利润自提比率
        private decimal responsibilityrewardtatio;    //责任上缴奖励比率
        private decimal responsibilitytrunedtarget;    //责任上缴目标
        private decimal state;    //状态


        //virtual public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        /// <summary>
        /// 安全文明施工目标
        /// </summary>
        virtual public string Safetycivilizedsign
        {
            get { return safetycivilizedsign; }
            set { safetycivilizedsign = value; }
        }
        /// <summary>
        /// 安装施工部分配合费比率
        /// </summary>
        virtual public decimal Installationfreerate
        {
            get { return installationfreerate; }
            set { installationfreerate = value; }
        }
        /// <summary>
        /// 成本控制奖励比率
        /// </summary>
        virtual public decimal Costcontrolrewardtatio
        {
            get { return costcontrolrewardtatio; }
            set { costcontrolrewardtatio = value; }
        }
        /// <summary>
        /// 成本控制目标
        /// </summary>
        virtual public decimal Costcontroltarget
        {
            get { return costcontroltarget; }
            set { costcontroltarget = value; }
        }
        /// <summary>
        /// 兑现奖励节点数量
        /// </summary>
        virtual public decimal Cashrewardnodenumber
        {
            get { return cashrewardnodenumber; }
            set { cashrewardnodenumber = value; }
        }
        /// <summary>
        /// 风险抵押金缴纳状况
        /// </summary>
        virtual public string Riskpaymentstate
        {
            get { return riskpaymentstate; }
            set { riskpaymentstate = value; }
        }
        /// <summary>
        /// 风险化解奖励比率
        /// </summary>
        virtual public decimal Riskrewardratio
        {
            get { return riskrewardratio; }
            set { riskrewardratio = value; }
        }
        /// <summary>
        /// 风险化解目标
        /// </summary>
        virtual public decimal Riskdissolvestarget
        {
            get { return riskdissolvestarget; }
            set { riskdissolvestarget = value; }
        }
        /// <summary>
        /// 工期
        /// </summary>
        virtual public string Projecttime
        {
            get { return projecttime; }
            set { projecttime = value; }
        }
        /// <summary>
        /// 工期计量单位
        /// </summary>
        virtual public string Periodmeasuringunit
        {
            get { return periodmeasuringunit; }
            set { periodmeasuringunit = value; }
        }
        /// <summary>
        /// 计划竣工时间
        /// </summary>
        virtual public DateTime Planenddate
        {
            get { return planenddate; }
            set { planenddate = value; }
        }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        virtual public DateTime Planbegindate
        {
            get { return planbegindate; }
            set { planbegindate = value; }
        }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        virtual public string Pricklename
        {
            get { return pricklename; }
            set { pricklename = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public string Priceprickle
        {
            get { return priceprickle; }
            set { priceprickle = value; }
        }
        /// <summary>
        /// 经济目标责任范围不含工程
        /// </summary>
        virtual public string Economicgoalenginner
        {
            get { return economicgoalenginner; }
            set { economicgoalenginner = value; }
        }
        /// <summary>
        /// 签订时间
        /// </summary>
        virtual public DateTime Signtime
        {
            get { return signtime; }
            set { signtime = value; }
        }
        /// <summary>
        /// 确保工程质量等级
        /// </summary>
        virtual public string Ensurelevel
        {
            get { return ensurelevel; }
            set { ensurelevel = value; }
        }
        /// <summary>
        /// 是否签订
        /// </summary>
        virtual public string Signedwhether
        {
            get { return signedwhether; }
            set { signedwhether = value; }
        }
        /// <summary>
        /// 文档名称
        /// </summary>
        virtual public string Documentname
        {
            get { return documentname; }
            set { documentname = value; }
        }
        /// <summary>
        /// 项目GUID 
        /// </summary>
        virtual public string Projectid
        {
            get { return projectid; }
            set { projectid = value; }
        }
        /// <summary>
        /// 项目规模
        /// </summary>
        virtual public string Projectseale
        {
            get { return projectseale; }
            set { projectseale = value; }
        }
        /// <summary>
        /// 项目经理
        /// </summary>
        virtual public string Projectmanagername
        {
            get { return projectmanagername; }
            set { projectmanagername = value; }
        }
        /// <summary>
        /// 项目经理GUID
        /// </summary>
        virtual public string Projectmanagerid
        {
            get { return projectmanagerid; }
            set { projectmanagerid = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        virtual public string Projectname
        {
            get { return projectname; }
            set { projectname = value; }
        }
        /// <summary>
        /// 业务指定分包费用自提比率
        /// </summary>
        virtual public decimal Servicefeerates
        {
            get { return servicefeerates; }
            set { servicefeerates = value; }
        }
        /// <summary>
        /// 业主奖励自提比率
        /// </summary>
        virtual public decimal Ownerawardsratio
        {
            get { return ownerawardsratio; }
            set { ownerawardsratio = value; }
        }
        /// <summary>
        /// 责任范围外分包工程利润自提比率
        /// </summary>
        virtual public decimal Responsibilityratio
        {
            get { return responsibilityratio; }
            set { responsibilityratio = value; }
        }
        /// <summary>
        /// 责任上缴奖励比率
        /// </summary>
        virtual public decimal Responsibilityrewardtatio
        {
            get { return responsibilityrewardtatio; }
            set { responsibilityrewardtatio = value; }
        }
        /// <summary>
        /// 责任上缴目标
        /// </summary>
        virtual public decimal Responsibilitytrunedtarget
        {
            get { return responsibilitytrunedtarget; }
            set { responsibilitytrunedtarget = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        virtual public decimal State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
