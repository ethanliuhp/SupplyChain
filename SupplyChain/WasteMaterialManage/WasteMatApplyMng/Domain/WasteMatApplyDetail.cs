using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain
{

    /// <summary>
    /// 废旧物资申请明细
    /// </summary>
    [Serializable]
    public class WasteMatApplyDetail : BaseDetail
    {

        private string grade;
        private decimal leftQuantity;
        private DateTime applyDate;
        /// <summary>
        /// 成色/物资档次
        /// </summary>
        virtual public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        /// <summary>
        /// 剩余数量
        /// </summary>
        virtual public decimal LeftQuantity
        {
            get { return leftQuantity; }
            set { leftQuantity = value; }
        }
        /// <summary>
        /// 申请处理时间
        /// </summary>
        virtual public DateTime ApplyDate
        {
            get { return applyDate; }
            set { applyDate = value; }
        }
        private GWBSTree usedPart;
        private string usedPartName;
        private string usedPartSysCode;
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
