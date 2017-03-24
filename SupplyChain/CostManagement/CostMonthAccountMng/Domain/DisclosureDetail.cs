using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    [Serializable]
    [Entity]
    public class DisclosureDetail : BaseDetail
    {
        private string subPackage;  //分包承担范围
        private string contractType;   //分包合同类型
        private decimal contractInterimMoney = 0;   //分包合同价格
        private string qualityBreachDuty;   //质量责任及违约责任
        private string durationBreachDuty;  //工程目标及违约责任
        private string safetyBreachDuty;    // 安全目标及违约责任
        private string civilizedBreachDuty; //文明施工及违约责任
        private string laborDemand; //劳动力需求
        private string materialDemand;  //主要材料需求
        private string paymentType; //付款方式
        private string warrantyDateMoney;   //保修期及保修金
        private string otherDesc;   //其他说明事项
        /// <summary>
        /// 分包承担范围
        /// </summary>
        virtual public string SubPackage
        {
            get { return subPackage; }
            set { subPackage = value; }
        }
        /// <summary>
        /// 分包合同类型
        /// </summary>
        virtual public string ContractType
        {
            get { return contractType; }
            set { contractType = value; }
        }
        /// <summary>
        /// 分包合同价格
        /// </summary>
        virtual public decimal ContractInterimMoney
        {
            get { return contractInterimMoney; }
            set { contractInterimMoney = value; }
        }
        /// <summary>
        /// 质量责任及违约责任
        /// </summary>
        virtual public string QualityBreachDuty
        {
            get { return qualityBreachDuty; }
            set { qualityBreachDuty = value; }
        }
        /// <summary>
        /// 工程目标及违约责任
        /// </summary>
        virtual public string DurationBreachDuty
        {
            get { return durationBreachDuty; }
            set { durationBreachDuty = value; }
        }
        /// <summary>
        /// 安全目标及违约责任
        /// </summary>
        virtual public string SafetyBreachDuty
        {
            get { return safetyBreachDuty; }
            set { safetyBreachDuty = value; }
        }
        /// <summary>
        /// 文明施工及违约责任
        /// </summary>
        virtual public string CivilizedBreachDuty
        {
            get { return civilizedBreachDuty; }
            set { civilizedBreachDuty = value; }
        }
        /// <summary>
        /// 劳动力需求
        /// </summary>
        virtual public string LaborDemand
        {
            get { return laborDemand; }
            set { laborDemand = value; }
        }
        /// <summary>
        /// 主要材料需求
        /// </summary>
        virtual public string MaterialDemand
        {
            get { return materialDemand; }
            set { materialDemand = value; }
        }
        /// <summary>
        /// 付款方式
        /// </summary>
        virtual public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
        /// <summary>
        /// 保修期及保修金
        /// </summary>
        virtual public string WarrantyDateMoney
        {
            get { return warrantyDateMoney; }
            set { warrantyDateMoney = value; }
        }
        /// <summary>
        /// 其他说明事项
        /// </summary>
        virtual public string OtherDesc
        {
            get { return otherDesc; }
            set { otherDesc = value; }
        }
    }
}
