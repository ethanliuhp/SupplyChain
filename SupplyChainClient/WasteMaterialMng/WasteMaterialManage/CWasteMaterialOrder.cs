using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage
{
    public enum CWasteMaterialOrder_ExecType
    {
        /// <summary>
        /// 废旧物资申请查询
        /// </summary>
        WasteMatApplyQuery,
    }

    public class CWasteMaterialOrder
    {
        private static IFramework framework = null;
        string mainViewName = "废旧物资申请信息";
        private static VWasteMaterialApplySearchList searchList;

        public CWasteMaterialOrder(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VWasteMaterialApplySearchList(this);
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

            VWasteMaterialApply vMainView = framework.GetMainView(mainViewName + "-空") as VWasteMaterialApply;

            if (vMainView == null)
            {
                vMainView = new VWasteMaterialApply();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VWasteMaterialApplySearchCon searchCon = new VWasteMaterialApplySearchCon(searchList);

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
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CWasteMaterialOrder_ExecType))
                {
                    CWasteMaterialOrder_ExecType execType = (CWasteMaterialOrder_ExecType)obj;
                    switch (execType)
                    {
                        case CWasteMaterialOrder_ExecType.WasteMatApplyQuery:
                            IMainView mroqMv = framework.GetMainView("废旧物资申请查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VWasteMaterialApplyQuery vmroq = new VWasteMaterialApplyQuery();
                            vmroq.ViewCaption = "废旧物资申请查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
