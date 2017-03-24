using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement
{
    public enum CPersonManageMng_ExecType
    {
        /// <summary>
        /// 管理人员日志查询
        /// </summary>
        PersonManageQuery
    }

    public class CPersonManage
    {
        private static IFramework framework = null;
        string mainViewName = "管理人员日志信息";
        private static VPersonManageSearchList searchList;

        public CPersonManage(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VPersonManageSearchList(this);
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

            VPersonManage vMainView = framework.GetMainView(mainViewName) as VPersonManage;

            if (vMainView == null)
            {
                vMainView = new VPersonManage();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VPersonManageSearchCon searchCon = new VPersonManageSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CPersonManageMng_ExecType))
                {
                    CPersonManageMng_ExecType execType = (CPersonManageMng_ExecType)obj;
                    switch (execType)
                    {
                        case CPersonManageMng_ExecType.PersonManageQuery:
                            IMainView mroqMv = framework.GetMainView("管理人员日志信息查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VPersonManageQuery vmroq = new VPersonManageQuery();
                            vmroq.ViewCaption = "管理人员日志信息查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
