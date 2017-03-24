using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Iesi.Collections.Generic;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class LockAccountMaster : BaseMaster
    {
        private int accountYear;
        private int accountMonth;
        private DateTime accountEndDate;

        /// <summary>
        /// 会计年
        /// </summary>
        virtual public int AccountYear
        {
            get
            {
                return accountYear;
            }
            set
            {
                accountYear = value;
            }
        }

        /// <summary>
        /// 会计月
        /// </summary>
        virtual public int AccountMonth
        {
            get
            {
                return accountMonth;
            }
            set
            {
                accountMonth = value;
            }
        }

        /// <summary>
        /// 会计结束日期
        /// </summary>
        virtual public DateTime AccountEndDate
        {
            get
            {
                return accountEndDate;
            }
            set
            {
                accountEndDate = value;
            }
        }
    }
}
