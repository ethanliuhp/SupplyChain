using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.TargetRespBook.Domain
{
    /// <summary>
    /// 目标节点
    /// </summary>
    [Serializable]
    public class TargetProgressNode : BaseMaster
    {
        //private string id;
        private string pricklename;   //计量单位名称
        private string priceprickle;   //价格计量单位
        private string nodenameid;   //节点名称
        private string targetrespbook;   //目标责任书
        private decimal benefitgoal;   //效益目标
        private string figurativeprogress;   //形象进度
        private string effectivestatus; //有效状态
        private decimal predictvalue;   //预计产值
        private DateTime plancompletedate;   //预计完成时间

        ///// <summary>
        ///// id
        ///// </summary>
        //virtual public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        //private decimal version;

        //virtual public decimal Version
        //{
        //    get { return version; }
        //    set { version = value; }
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
        /// 节点名称
        /// </summary>
        virtual public string Nodenameid
        {
            get { return nodenameid; }
            set { nodenameid = value; }
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
        /// 效益目标
        /// </summary>
        virtual public decimal Benefitgoal
        {
            get { return benefitgoal; }
            set { benefitgoal = value; }
        }
        /// <summary>
        /// 形象进度
        /// </summary>
        virtual public string Figurativeprogress
        {
            get { return figurativeprogress; }
            set { figurativeprogress = value; }
        }
        /// <summary>
        /// 有效状态
        /// </summary>
        virtual public string Effectivestatus
        {
            get { return effectivestatus; }
            set { effectivestatus = value; }
        }
        /// <summary>
        /// 预计产值
        /// </summary>
        virtual public decimal Predictvalue
        {
            get { return predictvalue; }
            set { predictvalue = value; }
        }
        /// <summary>
        /// 预计完成时间
        /// </summary>
        virtual public DateTime Plancompletedate
        {
            get { return plancompletedate; }
            set { plancompletedate = value; }
        }
    }
}
