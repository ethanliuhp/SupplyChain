using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;

namespace Application.Business.Erp.SupplyChain.Base.Domain
{
    [Serializable]
    public abstract class BaseBillDetail : BusinessEntityDetails
    {
        private Material materialResource;
        private StandardUnit matStandardUnit;
        private decimal quantity = 0;
        private decimal price = 0;
        private decimal money = 0;
        private string descript;
        private bool isSelect = true;
        private CostProject costProject;


        private decimal _PieceQty;
        private decimal _GrossQty;


        /// <summary>
        /// 毛重
        /// </summary>
        virtual public decimal GrossQty
        {
            get { return _GrossQty; }
            set { _GrossQty = value; }
        }

        /// <summary>
        /// 件数
        /// </summary>
        virtual public decimal PieceQty
        {
            get { return _PieceQty; }
            set { _PieceQty = value; }
        }

        /// <summary>
        /// 是否选择(在界面上用，不Map到数据库)
        /// </summary>
        virtual public bool IsSelect
        {
            get { return isSelect; }
            set { isSelect = value; }
        }

        /// <summary>
        /// 物料
        /// </summary>        
        virtual public Material MaterialResource
        {
            get { return materialResource; }
            set { materialResource = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        /// <summary>
        /// 单价
        /// </summary>

        virtual public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        /// <summary>
        /// 金额
        /// </summary>
        virtual public decimal Money
        {
            get { return money; }
            set { money = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        virtual public StandardUnit MatStandardUnit
        {
            get { return matStandardUnit; }
            set { matStandardUnit = value; }
        }
        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public CostProject CostProject
        {
            get { return costProject; }
            set { costProject = value; }
        }
    }
}
