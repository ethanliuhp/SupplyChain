using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
//using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain
{
    /// <summary>
    /// ����ⷽʽ
    /// </summary>
    public enum EnumStockInOutManner
    {
        ������� = 10,
        ������� = 11,
        ��ӯ��� = 12,
        ���ϳ��� = 20,
        �������� = 21,
        �̿����� = 22,
    }

    /// <summary>
    /// ��ⵥ�������
    /// </summary>
    public enum EnumForRedType
    {
        ������ = 0,
        �嵥�� = 1,
    }

    /// <summary>
    /// ��ⵥ����
    /// </summary>
    [Serializable]
    public abstract class BasicStockIn : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private StationCategory theStationCategory;
        private int isTally = 0;
        private string supplyOrderCode;
        private decimal sumMoney;
        private decimal sumQuantity;
        private string theSupplierName;
        private EnumStockInOutManner stockInManner;
        private string contractNo;
        private decimal sumConfirmMoney;
        private string special;
        private MaterialCategory materialCategory;
        private string matCatName;
        private string professionCategory;
        private int theStockInOutKind;

        /// <summary>
        /// �������
        /// </summary>
        public virtual int TheStockInOutKind
        {
            get { return theStockInOutKind; }
            set { theStockInOutKind = value; }
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
        /// רҵ �������ֵ���
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ContractNo
        {
            get { return contractNo; }
            set { contractNo = value; }
        }

        /// <summary>
        /// ��ⷽʽ
        /// </summary>
        public virtual EnumStockInOutManner StockInManner
        {
            get { return stockInManner; }
            set { stockInManner = value; }
        }

        /// <summary>
        /// ��Ӧ������
        /// </summary>
        public virtual string TheSupplierName
        {
            get { return theSupplierName; }
            set { theSupplierName = value; }
        }

        /// <summary>
        /// �����ϼ�
        /// </summary>
        virtual public decimal SumQuantity
        {
            get
            {
                decimal tmpQuantity = 0;
                //����
                foreach (BasicStockInDtl var in Details)
                {
                    tmpQuantity += var.Quantity;
                }
                sumQuantity = tmpQuantity;
                return sumQuantity;
            }
            set { sumQuantity = value; }
        }

        public virtual decimal SumConfirmMoney
        {
            get
            {
                decimal tmpMoney = 0;
                foreach (BasicStockInDtl var in Details)
                {
                    tmpMoney += var.ConfirmMoney;
                }
                sumConfirmMoney = tmpMoney;
                return sumConfirmMoney;
            }
            set { sumConfirmMoney = value; }
        }

        /// <summary>
        /// ����ܼ�
        /// </summary>
        virtual public decimal SumMoney
        {
            get
            {
                decimal tmpMoney = 0;
                //����
                foreach (BasicStockInDtl var in Details)
                {
                    tmpMoney += var.Money;
                }
                sumMoney = tmpMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }

        /// <summary>
        /// �ɹ�����Code
        /// </summary>
        virtual public string SupplyOrderCode
        {
            get { return supplyOrderCode; }
            set { supplyOrderCode = value; }
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

        /// <summary>
        /// ��Ӧ��
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
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