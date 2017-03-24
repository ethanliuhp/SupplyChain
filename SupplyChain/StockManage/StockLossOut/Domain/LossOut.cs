using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain
{
    /// <summary>
    /// �̿����ⵥ
    /// </summary>
    [Serializable]
    public class LossOut :BaseMaster
    {
        private StationCategory theStationCategory;
        private int isTally;
        private string special;

        /// <summary>
        /// רҵ ���ֵ���
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        virtual public int IsTally
        {
            get { return isTally; }
            set { isTally = value; }
        }

        /// <summary>
        /// �ֿ�
        /// </summary>
        virtual public StationCategory TheStationCategory
        {
            get { return theStationCategory; }
            set { theStationCategory = value; }
        }

    }
}
