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

namespace Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain
{
    /// <summary>
    /// 检验批
    /// </summary>
    [Serializable]
    [Entity]
    public class InspectionLot : BaseMaster
    {
        private string serialNumber;
        /// <summary>
        /// 序号
        /// </summary>
        virtual public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        private string name;
        /// <summary>
        /// 名称
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private GWBSTree projectTask;
        private string projectTaskName;
        private DateTime insUpdateDate;
        private string inspectionDocument;
        private string accountStatus;
        private PersonInfo super;
        private string superName;

        /// <summary>
        /// 外部监理
        /// </summary>
        virtual public PersonInfo Super
        {
            get { return super; }
            set { super = value; }
        }

        /// <summary>
        /// 外部监理姓名
        /// </summary>
        virtual public string SuperName
        {
            get { return superName; }
            set { superName = value; }
        }

        /// <summary>
        /// 归属工程项目任务
        /// </summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        /// <summary>
        /// 工程任务名称
        /// </summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        /// <summary>
        /// 检查状态更新时间
        /// </summary>
        virtual public DateTime InsUpdateDate
        {
            get { return insUpdateDate; }
            set { insUpdateDate = value; }
        }
        /// <summary>
        /// 检验批文档
        /// </summary>
        virtual public string InspectionDocument
        {
            get { return inspectionDocument; }
            set { inspectionDocument = value; }
        }
        /// <summary>
        /// 验收结算状态
        /// </summary>
        virtual public string AccountStatus
        {
            get { return accountStatus; }
            set { accountStatus = value; }
        }

    }
}
