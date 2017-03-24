using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Service;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord
{
    public class MProRecord
    {
        private IContractExcuteSrv contractExcuteSrv;
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        public IContractExcuteSrv ContractExcuteSrv
        {
            get { return contractExcuteSrv; }
            set { contractExcuteSrv = value; }
        }
        public MProRecord()
        {
            if (contractExcuteSrv == null)
            {
                contractExcuteSrv = StaticMethod.GetService("ContractExcuteSrv") as IContractExcuteSrv;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;


            if (professionInspectionSrv == null)
            {
                professionInspectionSrv = StaticMethod.GetService("ProfessionInspectionSrv") as IProfessionInspectionSrv;
            }
        }
        #region 分包项目
        /// <summary>
        /// 保存分包项目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SubContractProject SaveContracExcuteMaster(SubContractProject obj)
        {
            return contractExcuteSrv.SaveContractExcute(obj);
        }
        #endregion
         private IProfessionInspectionSrv professionInspectionSrv;

        public IProfessionInspectionSrv ProfessionInspectionSrv
        {
            get { return professionInspectionSrv; }
            set { professionInspectionSrv = value; }
        }

       

        #region 专业检查记录
        /// <summary>
        /// 保存专业检查记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProfessionInspectionRecordMaster SaveProfessionInspectionMaster(ProfessionInspectionRecordMaster obj)
        {
            return professionInspectionSrv.SaveProfessionInspectionRecordPlan(obj);
        }

        #endregion
    }
}
