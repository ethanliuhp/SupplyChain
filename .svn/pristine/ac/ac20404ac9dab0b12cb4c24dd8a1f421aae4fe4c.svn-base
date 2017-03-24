using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng
{
    public enum CEngineerChangeMng_ExecType
    {
        /// <summary>
        /// 工程更改管理查询
        /// </summary>
        EngineerChangeQuery
    }
    public class CEngineerChangeMng
    {
         private static IFramework framework = null;

        string mainViewName = "工程更改信息";
        private static VEngineerChangeSearchList searchList;

        public CEngineerChangeMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VEngineerChangeSearchList(this);
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

            VEngineerChange vMainView = framework.GetMainView(mainViewName + "-空") as VEngineerChange;

            if (vMainView == null)
            {
                vMainView = new VEngineerChange();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VEngineerChangeSearchCon searchCon = new VEngineerChangeSearchCon(searchList);
                //VWasteMaterialHandleSearchList searchCon = new VWasteMaterialHandleSearchList(searchList);

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
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CEngineerChangeMng_ExecType))
                {
                    CEngineerChangeMng_ExecType execType = (CEngineerChangeMng_ExecType)obj;
                    switch (execType)
                    {
                        case CEngineerChangeMng_ExecType.EngineerChangeQuery:
                            IMainView mroqMv = framework.GetMainView("工程更改查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VEngineerChangeQuery vmroq = new VEngineerChangeQuery();
                            vmroq.ViewCaption = "工程更改查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
