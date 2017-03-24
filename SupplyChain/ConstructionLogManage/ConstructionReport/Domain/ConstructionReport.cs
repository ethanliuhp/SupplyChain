using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Domain
{
    /// <summary>
    /// 日施工情况报告
    /// </summary>
    [Serializable]
    public class ConstructReport : BaseMaster
    {
        private string materialCase;
        private string problem;
        private string completionSchedule;
        private WeatherInfo weatherGlass;
        //private DateTime logDate;
        private string otherActivities;
        private string safetyControl;
        private string projectManage;
        private string constructSite;

        /// <summary>
        /// 材料情况
        /// </summary>
        virtual public string MaterialCase
        {
            get { return materialCase; }
            set { materialCase = value; }
        }
        /// <summary>
        /// 存在问题
        /// </summary>
        virtual public string Problem
        {
            get { return problem; }
            set { problem = value; }
        }
        /// <summary>
        /// 工作内容及完成情况
        /// </summary>
        virtual public string CompletionSchedule
        {
            get { return completionSchedule; }
            set { completionSchedule = value; }
        }
        /// <summary>
        /// 其他活动情况
        /// </summary>
        virtual public string OtherActivities
        {
            get { return otherActivities; }
            set { otherActivities = value; }
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
        /// 项目管理情况
        /// </summary>
        virtual public string ProjectManage
        {
            get { return projectManage; }
            set { projectManage = value; }
        }
        /// <summary>
        /// 生产安全控制情况
        /// </summary>
        virtual public string SafetyControl
        {
            get { return safetyControl; }
            set { safetyControl = value; }
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
