using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    public enum EnumTargetRespBook
    {
        /// <summary>
        /// 目标责任书查询
        /// </summary>
        TargetRerspBookSearch,
        /// <summary>
        /// 目标责任书
        /// </summary>
        TargetRespBook,

    }
    public class CTargetRespBook
    {
        private static IFramework framework = null;
        string mainViewName = "目标责任书";
        private static VTargetRespBookSearchList searchList;

        public CTargetRespBook(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VTargetRespBookSearchList(this);
        }

        public void Start()
        {
            Find( "空", "空");
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

            VTargetRespBook vMainView = framework.GetMainView(mainViewName + "-空") as VTargetRespBook;

            if (vMainView == null)
            {
                vMainView = new VTargetRespBook();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VTargetRespBookSearchCon searchCon = new VTargetRespBookSearchCon(searchList);
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
                if (obj != null && obj.GetType() == typeof(EnumTargetRespBook))
                {
                    EnumTargetRespBook execType = new EnumTargetRespBook();
                    switch (execType)
                    {
                        case EnumTargetRespBook.TargetRerspBookSearch:
                            IMainView mroqMv = framework.GetMainView("目标责任书查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VTargetRespBookQuery vtrbq = new VTargetRespBookQuery();
                            vtrbq.ViewCaption = "目标责任书查询";
                            framework.AddMainView(vtrbq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
