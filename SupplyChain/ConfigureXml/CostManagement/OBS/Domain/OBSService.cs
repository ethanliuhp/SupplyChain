using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;


namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain
{
    /// <summary>
    /// 服务OBS
    /// </summary>
    public class OBSService : BaseMaster
    {
        private SupplierRelationInfo supplierId;
        private string supplierName;
        private string supplierCode;
        private DateTime beginDate = ClientUtil.ToDateTime("1900-1-1");
        private DateTime endDate = ClientUtil.ToDateTime("1900-1-1");
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskCode;
        private string personName;
        private string personNumber;
        private int serviceState;

        ///<summary>
        /// 状态
        ///</summary>
        virtual public int ServiceState
        {
            get { return serviceState; }
            set { serviceState = value; }
        }

        ///<summary>
        ///服务供应商
        ///</summary>
        virtual public SupplierRelationInfo SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        ///<summary>
        ///服务供应商名称
        ///</summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        ///<summary>
        ///服务供应商层次码
        ///</summary>
        virtual public string SupplierCode
        {
            get { return supplierCode; }
            set { supplierCode = value; }
        }
        ///<summary>
        ///开始时间
        ///</summary>
        virtual public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }
        ///<summary>
        ///结束时间
        ///</summary>
        virtual public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        ///<summary>
        ///工程项目任务
        ///</summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        ///<summary>
        ///工程项目任务名称
        ///</summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        ///<summary>
        ///工程项目任务层次码
        ///</summary>
        virtual public string ProjectTaskCode
        {
            get { return projectTaskCode; }
            set { projectTaskCode = value; }
        }
        ///<summary>
        ///负责人姓名
        ///</summary>
        virtual public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }
        ///<summary>
        ///负责人身份证号
        ///</summary>
        virtual public string PersonNumber
        {
            get { return personNumber; }
            set { personNumber = value; }
        }
    }
}
