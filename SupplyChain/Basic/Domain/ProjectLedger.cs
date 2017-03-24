using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    [Serializable]
    public class ProjectLedger : BaseMaster
    {
        private string _FullName;
        private string _ShortName;
        private string _ConstructionStage;
        private string _PermitTransact;
        private string _Province;
        private string _City;
        private string _Address;
        private string _BuildUnit;
        private string _FirstPartyPrincipal;
        private string _PrincipalJob;
        private string _PrincipalPhone;
        private string _SupervisorUnit;
        private string _DesignUnit;
        private string _ProjectManager;
        private string _ManagerPhone;
        private string _StartWorkDate;
        private string _CompleteDate;
        private string _CalendarDay;
        private string _BuildingCost;
        private string _OutputValue;
        private string _StructureArea;
        private string _BuildHeight;
        private string _StoreyNumber;
        private string _Scale;
        private string _QualityTarget;
        private string _ExcellenceTarget;
        private string _SecurityTarget;
        private DateTime _ModifyDate;
        private bool _IsEditable;
        private string _supervisorPrincipal;
        private string _supervisorPhone;
        private string _realstartDate;
        private string _realCompleteDate;
        private int _state;

        public virtual string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        public virtual string ShortName
        {
            get { return _ShortName; }
            set { _ShortName = value; }
        }

        public virtual string ConstructionStage
        {
            get { return _ConstructionStage; }
            set { _ConstructionStage = value; }
        }

        public virtual string PermitTransact
        {
            get { return _PermitTransact; }
            set { _PermitTransact = value; }
        }

        public virtual string Province
        {
            get { return _Province; }
            set { _Province = value; }
        }

        public virtual string City
        {
            get { return _City; }
            set { _City = value; }
        }

        public virtual string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public virtual string BuildUnit
        {
            get { return _BuildUnit; }
            set { _BuildUnit = value; }
        }

        public virtual string FirstPartyPrincipal
        {
            get { return _FirstPartyPrincipal; }
            set { _FirstPartyPrincipal = value; }
        }

        public virtual string PrincipalJob
        {
            get { return _PrincipalJob; }
            set { _PrincipalJob = value; }
        }

        public virtual string PrincipalPhone
        {
            get { return _PrincipalPhone; }
            set { _PrincipalPhone = value; }
        }

        public virtual string SupervisorUnit
        {
            get { return _SupervisorUnit; }
            set { _SupervisorUnit = value; }
        }

        public virtual string DesignUnit
        {
            get { return _DesignUnit; }
            set { _DesignUnit = value; }
        }

        public virtual string ProjectManager
        {
            get { return _ProjectManager; }
            set { _ProjectManager = value; }
        }

        public virtual string ManagerPhone
        {
            get { return _ManagerPhone; }
            set { _ManagerPhone = value; }
        }

        public virtual string StartWorkDate
        {
            get { return _StartWorkDate; }
            set { _StartWorkDate = value; }
        }

        public virtual string CompleteDate
        {
            get { return _CompleteDate; }
            set { _CompleteDate = value; }
        }

        public virtual string CalendarDay
        {
            get { return _CalendarDay; }
            set { _CalendarDay = value; }
        }

        public virtual string BuildingCost
        {
            get { return _BuildingCost; }
            set { _BuildingCost = value; }
        }

        public virtual string OutputValue
        {
            get { return _OutputValue; }
            set { _OutputValue = value; }
        }

        public virtual string StructureArea
        {
            get { return _StructureArea; }
            set { _StructureArea = value; }
        }

        public virtual string BuildHeight
        {
            get { return _BuildHeight; }
            set { _BuildHeight = value; }
        }

        public virtual string StoreyNumber
        {
            get { return _StoreyNumber; }
            set { _StoreyNumber = value; }
        }

        public virtual string Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }

        public virtual string QualityTarget
        {
            get { return _QualityTarget; }
            set { _QualityTarget = value; }
        }

        public virtual string ExcellenceTarget
        {
            get { return _ExcellenceTarget; }
            set { _ExcellenceTarget = value; }
        }

        public virtual string SecurityTarget
        {
            get { return _SecurityTarget; }
            set { _SecurityTarget = value; }
        }

        public virtual DateTime ModifyDate
        {
            get { return _ModifyDate; }
            set { _ModifyDate = value; }
        }

        public virtual bool IsEditable
        {
            get { return _IsEditable; }
            set { _IsEditable = value; }
        }

        public virtual string ModifyDateStr
        {
            get { return ModifyDate.ToString(); }
        }

        public virtual string ProjectAddress
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(Province))
                {
                    sb.Append(Province + "省");
                }
                if (!string.IsNullOrEmpty(City))
                {
                    sb.Append(City + "市");
                }

                sb.Append(Address);
                return sb.ToString();
            }
        }

        public virtual string SupervisorPrincipal
        {
            get { return _supervisorPrincipal; }
            set { _supervisorPrincipal = value; }
        }

        public virtual string SupervisorPhone
        {
            get { return _supervisorPhone; }
            set { _supervisorPhone = value; }
        }

        public virtual string RealStartDate
        {
            get { return _realstartDate; }
            set { _realstartDate = value; }
        }

        public virtual string RealCompleteDate
        {
            get { return _realCompleteDate; }
            set { _realCompleteDate = value; }
        }

        public virtual int State
        {
            get { return _state; }
            set { _state = value; }
        }

    }
}
