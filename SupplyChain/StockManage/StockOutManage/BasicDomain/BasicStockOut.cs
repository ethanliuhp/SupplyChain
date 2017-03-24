using System;
using System.Collections.Generic;
using System.Text;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain
{
    /// <summary>
    /// ���ⵥ����
    /// </summary>
    [Entity]
    [Serializable]
    public abstract class BasicStockOut : BaseMaster
    {
        private StationCategory theStationCategory;
        private int isTally;
        private decimal refQuantity;

        private EnumStockInOutManner stockOutManner;
        private SupplierRelationInfo theSupplierRelationInfo;
        private string theSupplierName;
        private string special;
        private MaterialCategory materialCategory;
        private string matCatName;
        private string professionCategory;
        private string monthConsumeId;
        private long theStockInOutKind;

        /// <summary>
        /// ����������
        /// </summary>
        public virtual long TheStockInOutKind
        {
            get { return theStockInOutKind; }
            set { theStockInOutKind = value; }
        }
       
        /// <summary>
        /// �¶�ʵ�ʺ���ID
        /// </summary>
        public virtual string MonthConsumeId
        {
            get { return monthConsumeId; }
            set { monthConsumeId = value; }
        }
        /// <summary>
        /// רҵ����
        /// </summary>
        public virtual string ProfessionCategory
        {
            get { return professionCategory; }
            set { professionCategory = value; }
        }

        /// <summary>
        /// ���ʷ�������
        /// </summary>
        public virtual string MatCatName
        {
            get { return matCatName; }
            set { matCatName = value; }
        }

        /// <summary>
        /// ���ʷ���
        /// </summary>
        public virtual MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }

        /// <summary>
        /// רҵ ���ֵ���
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        /// <summary>
        /// ʹ�������������
        /// </summary>
        public virtual string TheSupplierName
        {
            get { return theSupplierName; }
            set { theSupplierName = value; }
        }

        /// <summary>
        /// ʹ���������
        /// </summary>
        public virtual SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        /// <summary>
        /// ���ⷽʽ
        /// </summary>
        virtual public EnumStockInOutManner StockOutManner
        {
            get { return stockOutManner; }
            set { stockOutManner = value; }
        }

        /// <summary>
        /// ����������(��Map���ݿ�)
        /// </summary>
        virtual public decimal RefQuantity
        {
            get { return refQuantity; }
            set { refQuantity = value; }
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

        private string concreteBalId;

        /// <summary>
        /// ��Ʒ�Ž�������ID
        /// </summary>
        virtual public string ConcreteBalID
        {
            get { return concreteBalId; }
            set { concreteBalId = value; }
        }
       
    }
}
