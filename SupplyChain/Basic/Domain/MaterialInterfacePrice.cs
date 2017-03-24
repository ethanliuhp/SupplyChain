using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    /// <summary>
    /// 物资信息价
    /// </summary>
    [Serializable]
    public class MaterialInterfacePrice
    {
        private string id;
        private string state;
        private Material materialGUID;
        private string matName;
        private string matCode;
        private string matSpec;
        private string matStuff;
        private StandardUnit matUnit;
        private string matUnitName;
        private DateTime lastUpdateDate = DateTime.Now;
        private PersonInfo makePerson;
        private string makePersonName;
        private DateTime makeTime;
        private decimal price;
        private string descript;
        private decimal marketPrice;
        private DateTime dateTimeBegin;
        private DateTime dateTimeEnd;

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime DateTimeBegin
        {
            get { return dateTimeBegin; }
            set { dateTimeBegin = value; }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime DateTimeEnd
        {
            get { return dateTimeEnd; }
            set { dateTimeEnd = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual string State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 市场价
        /// </summary>
        public virtual decimal MarketPrice
        {
            get { return marketPrice; }
            set { marketPrice = value; }
        }

        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 物资GUID
        /// </summary>
        public virtual Material MaterialGUID
        {
            get { return materialGUID; }
            set { materialGUID = value; }
        }
        /// <summary>
        /// 物资名称
        /// </summary>
        public virtual string MatName
        {
            get { return matName; }
            set { matName = value; }
        }
        /// <summary>
        /// 物资编号
        /// </summary>
        public virtual string MatCode
        {
            get { return matCode; }
            set { matCode = value; }
        }
        /// <summary>
        /// 物资规格
        /// </summary>
        public virtual string MatSpec
        {
            get { return matSpec; }
            set { matSpec = value; }
        }
        /// <summary>
        /// 物资材质
        /// </summary>
        public virtual string MatStuff
        {
            get { return matStuff; }
            set { matStuff = value; }
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        public virtual StandardUnit MatUnit
        {
            get { return matUnit; }
            set { matUnit = value; }
        }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        public virtual string MatUnitName
        {
            get { return matUnitName; }
            set { matUnitName = value; }
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }
        /// <summary>
        /// 制定人
        /// </summary>
        public virtual PersonInfo MakePerson
        {
            get { return makePerson; }
            set { makePerson = value; }
        }
        /// <summary>
        /// 制定人名称
        /// </summary>
        public virtual string MakePersonName
        {
            get { return makePersonName; }
            set { makePersonName = value; }
        }
        /// <summary>
        /// 定制时间
        /// </summary>
        public virtual DateTime MakeTime
        {
            get { return makeTime; }
            set { makeTime = value; }
        }
        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
    }
}
