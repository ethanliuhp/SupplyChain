using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialBalanceMng.Domain
{
    /// <summary>
    ///结算其他费用明细
    /// </summary>
    [Serializable]
    public class MatBalOtherCostDetail : BaseDetail
    {
        private string businessType;
        private string businessCode;
        private string businessMasterId;
        private string businessDetailId;
        private string costType;
        private decimal costMoney;
        private MaterialBalanceMaster master;

        private CostAccountSubject subjectGUID;
        private string subjectName;
        private string subjectSysCode;
        /// <summary>
        /// 核算科目
        /// </summary>
        virtual public CostAccountSubject SubjectGUID
        {
            get { return subjectGUID; }
            set { subjectGUID = value; }
        }
        /// <summary>
        /// 核算科目名称
        /// </summary>
        virtual public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        /// <summary>
        /// 核算科目层次码
        /// </summary>
        virtual public string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }
        private GWBSTree usedPart;
        private string usedPartName;
        private string usedPartSysCode;
        /// <summary>
        /// 浇筑部位
        /// </summary>
        virtual public GWBSTree UsedPart
        {
            get { return usedPart; }
            set { usedPart = value; }
        }
        /// <summary>
        /// 浇筑部位名称
        /// </summary>
        virtual public string UsedPartName
        {
            get { return usedPartName; }
            set { usedPartName = value; }
        }
        /// <summary>
        /// 浇筑部位层次码
        /// </summary>
        virtual public string UsedPartSysCode
        {
            get { return usedPartSysCode; }
            set { usedPartSysCode = value; }
        }


        /// <summary>
        /// 业务类型
        /// </summary>
        virtual public string BusinessType
        {
            get { return businessType; }
            set { businessType = value; }
        }
        /// <summary>
        /// 业务单据号
        /// </summary>
        virtual public string BusinessCode
        {
            get { return businessCode; }
            set { businessCode = value; }
        }
        /// <summary>
        /// 单据主表GUID
        /// </summary>
        virtual public string BusinessMasterId
        {
            get { return businessMasterId; }
            set { businessMasterId = value; }
        }
        /// <summary>
        /// 单据明细GUID
        /// </summary>
        virtual public string BusinessDetailId
        {
            get { return businessDetailId; }
            set { businessDetailId = value; }
        }
        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public string CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 费用金额
        /// </summary>
        virtual public decimal CostMoney
        {
            get { return costMoney; }
            set { costMoney = value; }
        }
        /// <summary>
        /// 结算主表
        /// </summary>
        virtual public MaterialBalanceMaster Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
