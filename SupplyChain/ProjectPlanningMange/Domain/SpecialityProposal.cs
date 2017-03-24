using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain
{
    /// <summary>
    /// 项目专业策划
    /// </summary>
    [Serializable]
    public class SpecialityProposal : BaseMaster
    {
        private string planningLevel;  //策划级别                     
        private DateTime submitDate;  //提交时间          
        private string enginnerType;  //专业策划类型       
        private decimal evaluationWay;  //评审方式       
        private string enginnerName;  //专业策划名称       

        /// <summary>
        /// 策划级别
        /// </summary>
        virtual public string PlanningLevel
        {
            get { return planningLevel; }
            set { planningLevel = value; }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        virtual public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }
        /// <summary>
        /// 专业策划类型
        /// </summary>
        virtual public string EnginnerType
        {
            get { return enginnerType; }
            set { enginnerType = value; }
        }
        /// <summary>
        /// 评审方式
        /// </summary>
        virtual public decimal EvaluationWay
        {
            get { return evaluationWay; }
            set { evaluationWay = value; }
        }
        /// <summary>
        /// 专业策划书名称
        /// </summary>
        virtual public string EnginnerName
        {
            get { return enginnerName; }
            set { enginnerName = value; }
        }
        ///// <summary>
        ///// 状态
        ///// </summary>
        //virtual public decimal DocState
        //{
        //    get { return docState; }
        //    set { docState = value; }
        //}
    }
}
