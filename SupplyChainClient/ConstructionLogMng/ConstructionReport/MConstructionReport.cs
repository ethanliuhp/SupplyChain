using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Service;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport
{
    public class MConstructionReport
    {
        private IConstructionReportSrv constructionReportSrv;
        public IConstructionReportSrv ConstructionReportSrv
        {
            get { return constructionReportSrv; }
            set { constructionReportSrv = value; }
        }
        public MConstructionReport()
        {
            if (constructionReportSrv == null)
            {
                constructionReportSrv = StaticMethod.GetService("ConstructionReportSrv") as IConstructionReportSrv;
            }
        }
        /// <summary>
        /// 保存日日施工情况
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ConstructReport SaveConstruction(ConstructReport obj)
        {
            return constructionReportSrv.SaveConstructReport(obj);
        }
    }
}
