using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.PumpingPoundsMng.Domain
{
    /// <summary>
    /// 抽磅单明细
    /// </summary>
    [Serializable]
    public class PumpingPoundsDetail : BaseDetail
    {
        private string plateNumber;
        private decimal diffAmount;
        private decimal netWeight;
        private decimal grossWeight;
        private decimal tareWeight;
        private decimal ticketVolume;
        private decimal ticketWeight;

        /// <summary>
        /// 车牌号
        /// </summary>
        virtual public string PlateNumber
        {
            get { return plateNumber; }
            set { plateNumber = value; }
        }
        /// <summary>
        /// 对比量差
        /// </summary>
        virtual public decimal DiffAmount
        {
            get { return diffAmount; }
            set { diffAmount = value; }
        }
        /// <summary>
        /// 净重
        /// </summary>
        virtual public decimal NetWeight
        {
            get { return netWeight; }
            set { netWeight = value; }
        }
        /// <summary>
        /// 毛重
        /// </summary>
        virtual public decimal GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }
        /// <summary>
        /// 皮重
        /// </summary>
        virtual public decimal TareWeight
        {
            get { return tareWeight; }
            set { tareWeight = value; }
        }
        /// <summary>
        /// 小票方量
        /// </summary>
        virtual public decimal TicketVolume
        {
            get { return ticketVolume; }
            set { ticketVolume = value; }
        }
        /// <summary>
        /// 小票重量
        /// </summary>
        virtual public decimal TicketWeight
        {
            get { return ticketWeight; }
            set { ticketWeight = value; }
        }
    }
}
