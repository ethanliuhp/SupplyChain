using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng
{
    public enum CInspectionLotMng_ExecType
    {
        /// <summary>
        /// 检验批查询
        /// </summary>
        InspectionLotQuery
    }
    
    public class CInspectionLotMng
    {
        private static IFramework framework = null;
        string mainViewName = "检验批";
        private static VInspectionLotSearchList searchList;

        public CInspectionLotMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VInspectionLotSearchList(this);
        }

        public void Start()
        {
            Find("空","空");
        }

        public void Find(string name,string Id)
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

            VInspectionLot vMainView = framework.GetMainView(mainViewName + "-空") as VInspectionLot;

            if (vMainView == null)
            {
                vMainView = new VInspectionLot();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VInspectionLotSearchCon searchCon = new VInspectionLotSearchCon(searchList);
                vMainView.AssistViews.Add(searchCon);
                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            } else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CInspectionLotMng_ExecType))
                {
                    CInspectionLotMng_ExecType execType = (CInspectionLotMng_ExecType)obj;
                    switch (execType)
                    {
                        case CInspectionLotMng_ExecType.InspectionLotQuery:
                            IMainView mroqMv = framework.GetMainView("检验批查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VInspectionLotQuery vmroq = new VInspectionLotQuery();
                            vmroq.ViewCaption = "检验批查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
