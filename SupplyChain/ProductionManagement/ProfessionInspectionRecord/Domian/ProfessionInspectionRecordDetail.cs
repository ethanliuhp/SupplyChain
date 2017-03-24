using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain
{
    /// <summary>
    /// 检查结论
    /// </summary>
    public enum EnumConclusionType
    {
        检查通过 = 0,
        检查不通过 = 1
    }

    /// <summary>
    /// 专业检查记录明细
    /// </summary>
    [Serializable]
    public class ProfessionInspectionRecordDetail : BaseDetail
    {
        private int inspectionConclusion;//检查结论
        private int deductionSign;//罚扣款标志
        private int correctiveSign;//整改标志
        private string inspectionContent;//检查内容说明
        private string inspectionSituation;//检查情况
        private SubContractProject inspectionSupplier;//受检承担单位
        private string inspectionSupplierName;//受检承担单位名称
        private PersonInfo inspectionPerson;//受检管理责任者
        private string inspectionPersonName;//受检管理责任人名称
        private DateTime inspectionDate = DateTime.Now;//要求整改完成时间
        private string dangerLevel;//隐患级别
        private string dangerPart;//隐患部位
        private string dangerType;//隐患类型
        private string measureRequired;//整改措施要求

        /// <summary>
        /// 检查结论
        /// </summary>
        virtual public int  InspectionConclusion
        {
            get { return inspectionConclusion; }
            set { inspectionConclusion = value; }
        }
        /// <summary>
        /// 罚扣款标志
        /// </summary>
        virtual public int DeductionSign
        {
            get { return deductionSign; }
            set { deductionSign = value; }
        }
        /// <summary>
        /// 整改标志
        /// </summary>
        virtual public int CorrectiveSign
        {
            get { return correctiveSign; }
            set { correctiveSign = value; }
        }
        /// <summary>
        /// 检查内容说明
        /// </summary>
        virtual public string  InspectionContent
        {
            get { return inspectionContent; }
            set { inspectionContent = value; }
        }
        /// <summary>
        /// 检查情况
        /// </summary>
        virtual public string InspectionSituation
        {
            get { return inspectionSituation; }
            set { inspectionSituation = value; }
        }
        /// <summary>
        /// 受检承担单位
        /// </summary>
        virtual public SubContractProject InspectionSupplier
        {
            get { return inspectionSupplier; }
            set { inspectionSupplier = value; }
        }
        /// <summary>
        /// 受检承担单位名称
        /// </summary>
        virtual public string InspectionSupplierName
        {
            get { return inspectionSupplierName; }
            set { inspectionSupplierName = value; }
        }
        /// <summary>
        /// 受检管理负责者
        /// </summary>
        virtual public PersonInfo InspectionPerson
        {
            get { return inspectionPerson; }
            set { inspectionPerson = value; }
        }
        /// <summary>
        /// 受检管理负责人名称
        /// </summary>
        virtual public string InspectionPersonName
        {
            get { return inspectionPersonName; }
            set { inspectionPersonName = value; }
        }
        /// <summary>
        /// 要求整改完成时间
        /// </summary>
        virtual public DateTime InspectionDate
        {
            get { return inspectionDate; }
            set { inspectionDate = value; }
        }
        /// <summary>
        /// 隐患级别（1 一般，2 重要，3 紧急）
        /// </summary>
        virtual public string DangerLevel
        {
            get { return dangerLevel; }
            set { dangerLevel = value; }
        }
        /// <summary>
        /// 隐患部位
        /// </summary>
        virtual public string DangerPart
        {
            get { return dangerPart; }
            set { dangerPart = value; }
        }
        /// <summary>
        /// 隐患类型
        /// </summary>
        virtual public string DangerType
        {
            get { return dangerType; }
            set { dangerType = value; }
        }
        /// <summary>
        /// 整改措施要求
        /// </summary>
        virtual public string MeasureRequired
        {
            get { return measureRequired; }
            set { measureRequired = value; }
        }

       
    }
}
