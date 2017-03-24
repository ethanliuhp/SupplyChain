using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 分包终结报审统计表
    /// </summary>
    [Serializable]
    [Entity]
    public class SubcontractAuditDtl : BaseDetail
    {
        private string subentryProjectName;     //分项工程名称
        private string bearerOrgName;       //分包单位名称
        private decimal subcontractAmount;      //分包合同金额
        private decimal accumulateAmount;       //累计结算金额
        private DateTime makespan;      //完工时间
        private decimal expectSubcontractAmount;    //预计分包结算额
        private string isAudit; //分包结算是否报审
        private DateTime expectAuditTime;   //计划报审时间
        private int orderNo;
        /// <summary>
        /// 序号
        /// </summary>
        virtual public int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        virtual public string SubentryProjectName
        {
            get { return subentryProjectName; }
            set { subentryProjectName = value; }
        }

        virtual public string BearerOrgName
        {
            get { return bearerOrgName; }
            set { bearerOrgName = value; }
        }

        virtual public decimal SubcontractAmount
        {
            get { return subcontractAmount; }
            set { subcontractAmount = value; }
        }

        virtual public decimal AccumulateAmount
        {
            get { return accumulateAmount; }
            set { accumulateAmount = value; }
        }

        virtual public DateTime Makespan
        {
            get { return makespan; }
            set { makespan = value; }
        }

        virtual public decimal ExpectSubcontractAmount
        {
            get { return expectSubcontractAmount; }
            set { expectSubcontractAmount = value; }
        }

        virtual public string IsAudit
        {
            get { return isAudit; }
            set { isAudit = value; }
        }

        virtual public DateTime ExpectAuditTime
        {
            get { return expectAuditTime; }
            set { expectAuditTime = value; }
        }
    }
}
