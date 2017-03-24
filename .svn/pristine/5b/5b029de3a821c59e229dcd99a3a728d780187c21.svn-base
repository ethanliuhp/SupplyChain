using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Service
{
   

    /// <summary>
    /// 检测回执单服务
    /// </summary>
    public interface IDetectionReceiptSrv : IBaseService
    {
        IDao Dao { get; set; }

        #region 检测回执单
        /// <summary>
        /// 通过ID查询检测回执单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DetectionReceiptMaster GetDetectionReceiptById(string id);
        /// <summary>
        /// 通过Code查询检测回执单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DetectionReceiptMaster GetDetectionReceiptByCode(string code);
        /// <summary>
        /// 查询检测回执单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetDetectionReceipt(ObjectQuery objeceQuery);
        /// <summary>
        /// 检测回执单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet DetectionReceiptQuery(string condition);
        DetectionReceiptMaster SaveDetectionReceipt(DetectionReceiptMaster obj);

        //DemandMasterPlanDetail GetDemandDetailById(string DemandDtlId);
        #endregion

        
    }




}
