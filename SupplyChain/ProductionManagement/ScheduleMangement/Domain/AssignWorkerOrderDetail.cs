using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;


namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    [Serializable]
    [Entity]
    public class AssignWorkerOrderDetail
    {
        private string _id;
        /// <summary>主键</summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private AssignWorkerOrderMaster _master;
        /// <summary>主表主键</summary>
        public virtual AssignWorkerOrderMaster Master
        {
            get { return _master; }
            set { _master = value; }
        }

        private GWBSTree _gWBSTree;
        /// <summary>工程任务</summary>
        public virtual GWBSTree GWBSTree
        {
            get { return _gWBSTree; }
            set { _gWBSTree = value; }
        }

        private string _gWBSTreeName;
        /// <summary>工程任务名称</summary>
        public virtual string GWBSTreeName
        {
            get { return _gWBSTreeName; }
            set { _gWBSTreeName = value; }
        }

        private GWBSDetail _gWBSDetail;
        /// <summary>工程任务明细</summary>
        public virtual GWBSDetail GWBSDetail
        {
            get { return _gWBSDetail; }
            set { _gWBSDetail = value; }
        }

        private string _gWBSDetailName;
        /// <summary>工程任务明细名称</summary>
        public virtual string GWBSDetailName
        {
            get { return _gWBSDetailName; }
            set { _gWBSDetailName = value; }
        }

        private DateTime _planBeginDate = new DateTime(1900, 1, 1);
        /// <summary>计划开始时间</summary>
        public virtual DateTime PlanBeginDate
        {
            get { return _planBeginDate; }
            set { _planBeginDate = value; }
        }

        private DateTime _planEndDate = new DateTime(1900, 1, 1);
        /// <summary>计划结束时间</summary>
        public virtual DateTime PlanEndDate
        {
            get { return _planEndDate; }
            set { _planEndDate = value; }
        }

        private decimal _planWorkDays;
        /// <summary>计划工期</summary>
        public virtual decimal PlanWorkDays
        {
            get { return _planWorkDays; }
            set { _planWorkDays = value; }
        }

        private DateTime _actualBenginDate = new DateTime(1900,1,1);
        /// <summary>实际开始时间</summary>
        public virtual DateTime ActualBenginDate
        {
            get { return _actualBenginDate; }
            set { _actualBenginDate = value; }
        }

        private DateTime _actualEndDate = new DateTime(1900, 1, 1);
        /// <summary>实际结束时间</summary>
        public virtual DateTime ActualEndDate
        {
            get { return _actualEndDate; }
            set { _actualEndDate = value; }
        }

        private decimal _actualWorkDays;
        /// <summary>实际工期</summary>
        public virtual decimal ActualWorkDays
        {
            get { return _actualWorkDays; }
            set { _actualWorkDays = value; }
        }

        private string  _assWorkDesc;

        ///<summary>
        ///派工说明
        ///</summary>
        public virtual string  AssWorkDesc
        {
            set { this._assWorkDesc = value; }
            get { return this._assWorkDesc; }
        }
    }
}
