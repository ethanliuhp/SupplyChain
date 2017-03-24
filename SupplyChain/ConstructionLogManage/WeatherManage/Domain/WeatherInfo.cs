using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain
{
    /// <summary>
    /// 晴雨表
    /// </summary>
    [Serializable]
    public class WeatherInfo : BaseMaster
    {
        private string windDirection;
        //private DateTime logDate;
        private string  weatherCondition;
        private string temperature;
        private string relativeHumidity;      
        private int week;

        /// <summary>
        /// 风力风向
        /// </summary>
        virtual public string WindDirection
        {
            get { return windDirection; }
            set { windDirection = value; }
        }
        ///// <summary>
        ///// 日期
        ///// </summary>
        //virtual public DateTime LogDate
        //{
        //    get { return logDate; }
        //    set { logDate = value; }
        //}
        /// <summary>
        /// 天气状况
        /// </summary>
        virtual public string WeatherCondition
        {
            get { return weatherCondition; }
            set { weatherCondition = value; }
        }
        /// <summary>
        /// 温度
        /// </summary>
        virtual public string Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }
        /// <summary>
        /// 相对湿度
        /// </summary>
        virtual public string RelativeHumidity
        {
            get { return relativeHumidity; }
            set { relativeHumidity = value; }
        }
        /// <summary>
        /// 星期
        /// </summary>
        virtual public int Week
        {
            get { return week; }
            set { week = value; }
        }
    }
}
