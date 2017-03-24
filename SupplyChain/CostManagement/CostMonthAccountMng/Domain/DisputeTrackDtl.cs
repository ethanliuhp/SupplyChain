using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 项目分包争议问题跟踪
    /// </summary>
    [Serializable]
    [Entity]
    public class DisputeTrackDtl : BaseDetail
    {
        private string bearerUnitName;      //涉及分包合同及分包单位名称
        private string disputeContent;      //分包结算争议内容
        private string bearerSuggestion;    //分包意见
        private string projectSuggestion;   //项目意见
        private decimal involveMoney;       //涉及金额
        private string currentProgress;     //目前进展
        private int orderNo;
        /// <summary>
        /// 序号
        /// </summary>
        virtual public int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        virtual public string BearerUnitName
        {
            get { return bearerUnitName; }
            set { bearerUnitName = value; }
        }

        virtual public string DisputeContent
        {
            get { return disputeContent; }
            set { disputeContent = value; }
        }

        virtual public string BearerSuggestion
        {
            get { return bearerSuggestion; }
            set { bearerSuggestion = value; }
        }

        virtual public string ProjectSuggestion
        {
            get { return projectSuggestion; }
            set { projectSuggestion = value; }
        }

        virtual public decimal InvolveMoney
        {
            get { return involveMoney; }
            set { involveMoney = value; }
        }

        virtual public string CurrentProgress
        {
            get { return currentProgress; }
            set { currentProgress = value; }
        }
    }
}
