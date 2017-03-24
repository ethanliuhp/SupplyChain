using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain
{
    /// <summary>
    /// 专业检查记录主表
    /// </summary>
    [Serializable]
    public class ProfessionInspectionRecordMaster : BaseMaster
    {
        private string inspectionSpecail;
        //private DateTime inspectionDate = StringUtil.StrToDateTime("1900-01-01");
        private PersonInfo inspectionPerson;
        private string inspectionPersonName;
        private string inspectionOpgSysCode;
        private Iesi.Collections.Generic.ISet<ProfessionInspectionRecordDetail> details = new Iesi.Collections.Generic.HashedSet<ProfessionInspectionRecordDetail>();
        /// <summary>
        /// 检验明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<ProfessionInspectionRecordDetail> Details
        {
            get { return details; }
            set { details = value; }
        }
        /// <summary>
        /// 增加明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddDetails(ProfessionInspectionRecordDetail detail)
        {
            detail.Master = this;
            Details.Add(detail);
        }
        /// <summary>
        /// 检查专业
        /// </summary>
        virtual public string InspectionSpecail
        {
            get { return inspectionSpecail; }
            set { inspectionSpecail = value; }
        }
        /// <summary>
        /// 检查负责人
        /// </summary>
        virtual public PersonInfo InspectionPerson
        {
            get { return inspectionPerson; }
            set { inspectionPerson = value; }
        }
        /// <summary>
        /// 检查负责人名称
        /// </summary>
        virtual public string InspectionPersonName
        {
            get { return inspectionPersonName; }
            set { inspectionPersonName = value; }
        }
        /// <summary>
        /// 检查负责人组织层次码
        /// </summary>
        virtual public string InspectionOpgSysCode
        {
            get { return inspectionOpgSysCode; }
            set { inspectionOpgSysCode = value; }
        }
    }
}
