using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 维度注册
    /// </summary>
    [Serializable]
    [Entity]
    public class DimensionRegister
    {
        private string id;
        private long version = -1;
        private int state = 1;
        private string code;
        private string name;
        private string dimRights;
        private string originTypeCode;
        private string originTypeName;
        private int ifMeasure=0;
        private string remark;

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 维度编码
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 维度名称
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 维度权限
        /// </summary>
        virtual public string DimRights
        {
            get { return dimRights; }
            set { dimRights = value; }
        }

        /// <summary>
        /// 来源类型 1：外部来源、2：手工维护
        /// </summary>
        virtual public string OriginTypeCode
        {
            get { return originTypeCode; }
            set { originTypeCode = value; }
        }

        /// <summary>
        /// 来源类型 1：外部来源、2：手工维护
        /// </summary>
        virtual public string OriginTypeName
        {
            get { return originTypeName; }
            set { originTypeName = value; }
        }

        /// <summary>
        /// 是否为度量维度  0：默认(不是度量)、1：度量维度
        /// </summary>
        virtual public int IfMeasure
        {
            get { return ifMeasure; }
            set { ifMeasure = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }
}
