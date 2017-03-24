using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    public class FundManagement : ICloneable
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 归属组织ID
        /// </summary>
        public string OperationOrg { get; set; }
        /// <summary>
        /// 归属组织名称
        /// </summary>
        public string OperOrgName { get; set; }
        /// <summary>
        /// 归属组织层次码
        /// </summary>
        public string OpgSysCode { get; set; }
        /// <summary>
        /// 组织层级
        /// </summary>
        public string OrganizationLevel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Descript { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public string IndexName { get; set; }
        /// <summary>
        /// 指标ID
        /// </summary>
        public string IndexID { get; set; }
        /// <summary>
        /// 时间宾栏ID
        /// </summary>
        public string TimeID { get; set; }
        /// <summary>
        /// 时间宾栏名称
        /// </summary>
        public string TimeName { get; set; }
        /// <summary>
        /// 计量单位ID
        /// </summary>
        public string MeasurementUnitID { get; set; }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        public string MeasurementUnitName { get; set; }
        /// <summary>
        /// 预警级别ID
        /// </summary>
        public string WarningLevelID { get; set; }
        /// <summary>
        /// 预警级别名称
        /// </summary>
        public string WarningLevelName { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public decimal NumericalValue { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 跳转的url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 预警数量
        /// </summary>
        public int WarningCount { get; set; }
        /// <summary>
        /// 传递数据1
        /// </summary>
        public decimal Temp1 { get; set; }
        /// <summary>
        /// 传递数据2
        /// </summary>
        public decimal Temp2 { get; set; }
        /// <summary>
        /// 指标类型
        /// </summary>
        public IndexType Type { get; set; }
        /// <summary>
        /// 克隆自身（浅复制）
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var tag = (FundManagement)this.MemberwiseClone();
            tag.ID = _idGen.GeneratorIFCGuid();
            return tag;
        }

        private VirtualMachine.Component.Util.IFCGuidGenerator _idGen = new VirtualMachine.Component.Util.IFCGuidGenerator();
    }
}
