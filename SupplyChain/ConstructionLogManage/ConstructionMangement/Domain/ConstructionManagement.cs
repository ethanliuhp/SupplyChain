using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Domain
{
    /// <summary>
    /// 施工日志
    /// </summary>
    [Serializable]
    public class ConstructionManage : BaseMaster
    {
        private WeatherInfo weatherGlass;
        //private DateTime logDate;
        private string productionRecord;
        private string emergency;
        private string constructSite;
        private string workRecord;//技术工作记录
        private string qualityWorkRecord;//质检工作记录
        private string saftyWorkRecord;//安全工作记录
        /// <summary>
        /// 技术工作记录
        /// </summary>
        virtual public string WorkRecord
        {
            get { return workRecord; }
            set { workRecord = value; }
        }
        /// <summary>
        /// 质检工作记录
        /// </summary>
        virtual public string QualityWorkRecord
        {
            get { return qualityWorkRecord; }
            set { qualityWorkRecord = value; }
        }
        /// <summary>
        /// 安全工作记录
        /// </summary>
        virtual public string SaftyWorkRecord
        {
            get { return saftyWorkRecord; }
            set { saftyWorkRecord = value; }
        }
        /// <summary>
        /// 晴雨表
        /// </summary>
        virtual public WeatherInfo WeatherGlass
        {
            get { return weatherGlass; }
            set { weatherGlass = value; }
        }
        ///// <summary>
        ///// 日志日期
        ///// </summary>
        //virtual public DateTime LogDate
        //{
        //    get { return logDate; }
        //    set { logDate = value; }
        //}
        /// <summary>
        /// 生产情况记录
        /// </summary>
        virtual public string ProductionRecord
        {
            get { return productionRecord; }
            set { productionRecord = value; }
        }
        /// <summary>
        /// 突发事件
        /// </summary>
        virtual public string Emergency
        {
            get { return emergency; }
            set { emergency = value; }
        }
        /// <summary>
        /// 施工部位
        /// </summary>
        virtual public string ConstructSite
        {
            get { return constructSite; }
            set { constructSite = value; }
        }
    }
}
