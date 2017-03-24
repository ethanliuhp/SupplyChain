using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonForm.Controls
{
    public class AccountPeriod
    {
        public int AccountYear { get; set; }

        public int AccountMonth { get; set; }

        public DateTime EndDate { get; set; }

        public string DisplayText
        {
            get
            {
                return string.Format("{0}年{1}月[{2}]",
                    AccountYear,
                    AccountMonth.ToString().PadLeft(2, '0'),
                    EndDate.ToString("yyyy-MM-dd"));
            }
        }

        public string DisplayYear
        {
            get
            {
                return string.Concat(AccountYear, "年");
            }
        }

        public string DisplayMonth
        {
            get
            {
                return string.Concat(AccountMonth.ToString().PadLeft(2, ' '), "月");
            }
        }
    }
}
