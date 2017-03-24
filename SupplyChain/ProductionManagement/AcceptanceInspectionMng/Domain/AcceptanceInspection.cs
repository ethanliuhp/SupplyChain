using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using VirtualMachine.Core.Attributes;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain
{
    /// <summary>
    /// 验收检查记录
    /// </summary>
    [Serializable]
    [Entity]
    public class AcceptanceInspection : BaseMaster
    {
        private GWBSTree projectTask;
        private string projectTaskName;
        private DateTime inspectionDate = ClientUtil.ToDateTime("1900-01-01");
        private string inspectionSpecial;
        private string inspectionContent;
        private string specialInspectionState;
        private string inspectionStatus;
        private string inspectionConclusion;
        private PersonInfo inspectionPerson;
        private string inspectionPersonName;
        private string inspecPersonOpgSysCode;
        private PersonInfo handlePerson;
        private string handlePersonName;
        private string projectId;
        private string projectName;
        private OperationOrgInfo operOrgInfo;
        private string operOrgInfoName;
        private int deductionSign;
        private int correctiveSign;
        private string insLotCode;
        private InspectionLot insLotGUID;
        private string insTemp;

        /// <summary>
        /// 临时数据，储存检验批验收状态
        /// </summary>
        virtual public string InsTemp
        {
            get { return insTemp; }
            set { insTemp = value; }
        }
        /// <summary>
        /// 检验批GUID
        /// </summary>
        virtual public InspectionLot InsLotGUID
        {
            get { return insLotGUID; }
            set { insLotGUID = value; }
        }

        /// <summary>
        /// 检验批单号
        /// </summary>
        virtual public string InsLotCode
        {
            get { return insLotCode; }
            set { insLotCode = value; }
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
        /// 工程WBS树(工程项目任务)
        /// </summary>
        public virtual GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        /// <summary>
        /// 工程WBS树(工程项目任务名称)
        /// </summary>
        public virtual string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        /// <summary>
        /// 检查时间
        /// </summary>
        public virtual DateTime InspectionDate
        {
            get { return inspectionDate; }
            set { inspectionDate = value; }
        }
        /// <summary>
        /// 检查专业
        /// </summary>
        public virtual string InspectionSpecial
        {
            get { return inspectionSpecial; }
            set { inspectionSpecial = value; }
        }
        /// <summary>
        /// 专业检查状态字位置
        /// </summary>
        public virtual string SpecialInspectionState
        {
            get { return specialInspectionState; }
            set { specialInspectionState = value; }
        }
        /// <summary>
        /// 检查内容说明
        /// </summary>
        public virtual string InspectionContent
        {
            get { return inspectionContent; }
            set { inspectionContent = value; }
        }
        /// <summary>
        /// 检查情况
        /// </summary>
        public virtual string InspectionStatus
        {
            get { return inspectionStatus; }
            set { inspectionStatus = value; }
        }
        /// <summary>
        /// 检查结论
        /// </summary>
        public virtual string InspectionConclusion
        {
            get { return inspectionConclusion; }
            set { inspectionConclusion = value; }
        }
        /// <summary>
        /// 检查责任人名称
        /// </summary>
        public virtual string InspectionPersonName
        {
            get { return inspectionPersonName; }
            set { inspectionPersonName = value; }
        }
        /// <summary>
        /// 检查责任人
        /// </summary>
        public virtual PersonInfo InspectionPerson
        {
            get { return inspectionPerson; }
            set { inspectionPerson = value; }
        }
        /// <summary>
        /// 检查责任人组织层次码
        /// </summary>
        public virtual string InspecPersonOpgSysCode
        {
            get { return inspecPersonOpgSysCode; }
            set { inspecPersonOpgSysCode = value; }
        }
        /// <summary>
        ///责任人名称
        /// </summary>
        public virtual string HandlePersonName
        {
            get { return handlePersonName; }
            set { handlePersonName = value; }
        }
        /// <summary>
        /// 责任人
        /// </summary>
        virtual public PersonInfo HandlePerson
        {
            get { return handlePerson; }
            set { handlePerson = value; }
        }
        /// <summary>
        /// 归属项目
        /// </summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary>
        /// 归属项目名称
        /// </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        /// <summary>
        /// 归属业务组织
        /// </summary>
        virtual public OperationOrgInfo OperOrgInfo
        {
            get { return operOrgInfo; }
            set { operOrgInfo = value; }
        }
        /// <summary>
        /// 归属业务组织名称
        /// </summary>
        public virtual string OperOrgInfoName
        {
            get { return operOrgInfoName; }
            set { operOrgInfoName = value; }
        }

    }
}
