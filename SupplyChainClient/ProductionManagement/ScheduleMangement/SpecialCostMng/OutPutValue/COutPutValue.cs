using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue
{
    public enum COutPutValue_ExexType
    {
        OutPutValueQuery,
        RealOutputValueQuery
    }

    public class COutPutValue
    {
        private static IFramework framework = null;
        string mainViewName = "产值汇总查询";

        public COutPutValue(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public void Start()
        {
            Find("空", "空");
        }

        public void Find(string name, string Id)
        {
            string captionName = mainViewName;
            if (name is string)
            {
                captionName = this.mainViewName + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }
        }


        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(COutPutValue_ExexType))
                {
                    COutPutValue_ExexType execType = (COutPutValue_ExexType)obj;
                    switch (execType)
                    {
                        case COutPutValue_ExexType.OutPutValueQuery:
                            IMainView mroqMv = framework.GetMainView("产值汇总查询(计划节点)");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VOutPutValueQuery vcq = new VOutPutValueQuery();
                            vcq.ViewCaption = "产值汇总查询(计划节点)";
                            framework.AddMainView(vcq);
                            return null;
                        case COutPutValue_ExexType.RealOutputValueQuery:
                            IMainView realOutputValQueryView = framework.GetMainView("产值汇总查询");
                            if (realOutputValQueryView != null)
                            {
                                realOutputValQueryView.ViewShow();
                                return null;
                            }
                            VRealOutputValueQuery realOutputValQuery = new VRealOutputValueQuery();
                            realOutputValQuery.ViewCaption = "产值汇总查询";
                            framework.AddMainView(realOutputValQuery);
                            return null;
                        default:
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
