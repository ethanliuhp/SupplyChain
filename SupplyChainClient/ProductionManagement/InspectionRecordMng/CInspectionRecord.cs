using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng
{
    public enum CInspectionRecord_ExecType
    {
        /// <summary>
        /// 生产检查记录
        /// </summary>
        InspectionRecord,
        /// <summary>
        /// 质量验收检查查询
        /// </summary>
        InspectionRecordQuery

    }

    public class CInspectionRecord
    {
        static IFramework framework;

        public CInspectionRecord(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CInspectionRecord_ExecType))
                {
                    CInspectionRecord_ExecType execType = (CInspectionRecord_ExecType)obj;
                    string viewName = "";
                    IMainView mv = null;
                    switch (execType)
                    {
                        case CInspectionRecord_ExecType.InspectionRecord:
                            viewName = "生产检查记录";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VInspectionRecord vmroq = new VInspectionRecord();
                                vmroq.ViewCaption = viewName;
                                framework.AddMainView(vmroq);
                            }
                            break;
                        case CInspectionRecord_ExecType.InspectionRecordQuery:
                            IMainView mainView1 = framework.GetMainView("质量验收检查查询");
                            if (mainView1 != null)
                            {
                                mainView1.ViewShow();
                            }
                            else
                            {
                                VInspectionRecordQuery vmroq = new VInspectionRecordQuery();
                                vmroq.ViewCaption = "质量验收检查查询";
                                framework.AddMainView(vmroq);
                            }
                            break;
                    }
                }
            }
            return null;
        }
    }


}
