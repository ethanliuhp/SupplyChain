using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Base.Domain
{
    /// <summary>
    /// 业务单据主表公用属性
    /// </summary>
    [Serializable]
    [Entity]
    public class BaseDetail
    {
        private string id;

        private decimal refQuantity;
        private long version;
        private int isOver;
        private BaseMaster master;
        private Material materialResource;
        private StandardUnit matStandardUnit;
        private decimal quantity = 0;
        private decimal price = 0;
        private decimal money = 0;
        private string descript;
        private bool isSelect = false;

        private string materialCode;
        private string materialName;
        private string materialSpec;
        private string materialStuff;
        private string matStandardUnitName;
        private string diagramNumber;
        private string forwardDetailId;

        //private string usedPart;
        //private string usedPartName;

        private GWBSTree usedPart;
        private string usedPartName;
        private string usedPartSysCode;
        private string tempData1;

        private string materialSysCode;
        /// <summary>
        /// 物资类型层次码
        /// </summary>
        virtual public string MaterialSysCode
        {
            get { return materialSysCode; }
            set { materialSysCode = value; }
        }
        /// <summary>
        /// 临时数据，不做MAP
        /// </summary>
        virtual public string TempData1
        {
            get { return tempData1; }
            set { tempData1 = value; }
        }
        private string tempData2;
        /// <summary>
        /// 临时数据，不做MAP
        /// </summary>
        virtual public string TempData2
        {
            get { return tempData2; }
            set { tempData2 = value; }
        }
        private string tempData3;
        /// <summary>
        /// 临时数据，不做MAP
        /// </summary>
        virtual public string TempData3
        {
            get { return tempData3; }
            set { tempData3 = value; }
        }

        private string tempData;
        /// <summary>
        /// 临时数据，不做MAP
        /// </summary>
        virtual public string TempData
        {
            get { return tempData; }
            set { tempData = value; }
        }
        /// <summary>
        /// 图号
        /// </summary>
        public virtual string DiagramNumber
        {
            get { return diagramNumber; }
            set { diagramNumber = value; }
        }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        public virtual string MatStandardUnitName
        {
            get { return matStandardUnitName; }
            set { matStandardUnitName = value; }
        }

        /// <summary>
        /// 物资规格
        /// </summary>
        public virtual string MaterialSpec
        {
            get { return materialSpec; }
            set { materialSpec = value; }
        }

        /// <summary>
        /// 物资名称
        /// </summary>
        public virtual string MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }

        /// <summary>
        /// 物资编码
        /// </summary>
        public virtual string MaterialCode
        {
            get { return materialCode; }
            set { materialCode = value; }
        }
        /// <summary>
        /// 材质
        /// </summary>
        virtual public string MaterialStuff
        {
            get { return materialStuff; }
            set { materialStuff = value; }
        }

        /// <summary>
        /// GUID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 引用数量
        /// </summary>
        virtual public decimal RefQuantity
        {
            get { return refQuantity; }
            set { refQuantity = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        virtual public int IsOver
        {
            get { return isOver; }
            set { isOver = value; }
        }
        /// <summary>
        /// 主表
        /// </summary>
        virtual public BaseMaster Master
        {
            get { return master; }
            set { master = value; }
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
        /// 前驱明细Id
        /// </summary>
        virtual public string ForwardDetailId
        {
            get { return forwardDetailId; }
            set { forwardDetailId = value; }
        }

        /// <summary> 
        /// 使用部位
        /// </summary>
        virtual public GWBSTree UsedPart
        {
            get { return usedPart; }
            set { usedPart = value; }
        }
        /// <summary>
        /// 使用部位名称
        /// </summary>
        virtual public string UsedPartName
        {
            get { return usedPartName; }
            set { usedPartName = value; }
        }
        /// <summary>
        /// 使用部位层次码
        /// </summary>
        virtual public string UsedPartSysCode
        {
            get { return usedPartSysCode; }
            set { usedPartSysCode = value; }
        }
    }
}
