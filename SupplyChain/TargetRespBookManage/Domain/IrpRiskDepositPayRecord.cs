using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.TargetRespBook.Domain
{
    /// <summary>
    /// 风险抵押金缴纳记录
    /// </summary>
    [Serializable]
    public class IrpRiskDepositPayRecord : BaseMaster
    {
        //private string id;
        private string pricklename;   //计量单位名称
        private string priceprickle;   //价格计量单位
        private string targetrespbook;   //目标责任书
        private decimal paidinamount;   //实缴额
        private DateTime paidindate;   //实缴日期
        private string projectposition;   //项目职务
        private string projectlevelposition;   //项目职务等级
        private decimal payable;  //应缴额
        private string responsiblename;   //责任人
        private string responsibleid;   //责任人GUID

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
        /// 目标责任书
        /// </summary>
        virtual public string Targetrespbook
        {
            get { return targetrespbook; }
            set { targetrespbook = value; }
        }
        /// <summary>
        /// 实缴额
        /// </summary>
        virtual public decimal Paidinamount
        {
            get { return paidinamount; }
            set { paidinamount = value; }
        }
        /// <summary>
        /// 实缴日期
        /// </summary>
        virtual public DateTime Paidindate
        {
            get { return paidindate; }
            set { paidindate = value; }
        }
        /// <summary>
        /// 项目职务
        /// </summary>
        virtual public string Projectposition
        {
            get { return projectposition; }
            set { projectposition = value; }
        }
        /// <summary>
        /// 项目职务级别
        /// </summary>
        virtual public string Projectlevelposition
        {
            get { return projectlevelposition; }
            set { projectlevelposition = value; }
        }       
        /// <summary>
        /// 应缴额
        /// </summary>
        virtual public decimal Payable
        {
            get { return payable; }
            set { payable = value; }
        }       
        /// <summary>
        /// 责任人
        /// </summary>
        virtual public string Responsiblename
        {
            get { return responsiblename; }
            set { responsiblename = value; }
        }
        /// <summary>
        /// 责任人GUID
        /// </summary>
        virtual public string Responsibleid
        {
            get { return responsibleid; }
            set { responsibleid = value; }
        }
    }
}
