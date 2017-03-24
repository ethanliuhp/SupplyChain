using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    public enum COwnerQuantityMng_ExecType
    {
        /// <summary>
        /// 业主报量查询
        /// </summary>
        OwnerQuantityQuery,
        /// <summary>
        /// 业主报量状态查询
        /// </summary>
        OwnerQuantitySearch
    }
    
    public class COwnerQuantityMng
    {
        private static IFramework framework = null;
        string mainViewName = "业主报量单";
        private static VOwnerQuantitySearchList searchList;

        public COwnerQuantityMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VOwnerQuantitySearchList(this);
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

            VOwnerQuantityMng vMainView = framework.GetMainView(mainViewName + "-空") as VOwnerQuantityMng;

            if (vMainView == null)
            {
                vMainView = new VOwnerQuantityMng();
                vMainView.ViewName = mainViewName;

                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VOwnerQuantitySearchCon searchCon = new VOwnerQuantitySearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(COwnerQuantityMng_ExecType))
                {
                    COwnerQuantityMng_ExecType execType = (COwnerQuantityMng_ExecType)obj;
                    switch (execType)
                    {
                        case COwnerQuantityMng_ExecType.OwnerQuantityQuery:
                            IMainView mroqMv = framework.GetMainView("业主报量查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VOwnerQuantityQuery vmroq = new VOwnerQuantityQuery();
                            vmroq.ViewCaption = "业主报量查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case COwnerQuantityMng_ExecType.OwnerQuantitySearch:
                            IMainView mroqMv1 = framework.GetMainView("项目业主报量状态查询");
                            if (mroqMv1 != null)
                            {
                                mroqMv1.ViewShow();
                                return null;
                            }
                            VOwnerQuantitySearch vmroq1 = new VOwnerQuantitySearch();
                            vmroq1.ViewCaption = "项目业主报量状态查询";
                            framework.AddMainView(vmroq1);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
