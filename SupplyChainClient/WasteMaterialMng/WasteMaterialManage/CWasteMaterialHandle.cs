﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage
{
    public enum CWasteMaterialHandle_ExecType
    {
        /// <summary>
        /// 废旧物料处理信息查询
        /// </summary>
        WasteMatHandleQuery
    }
    public class CWasteMaterialHandle
    {
         private static IFramework framework = null;

        string mainViewName = "废旧物资处理单";
        private static VWasteMaterialHandleSearchList searchList;

        public CWasteMaterialHandle(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VWasteMaterialHandleSearchList(this);
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

            VWasteMaterialHandle vMainView = framework.GetMainView(mainViewName + "-空") as VWasteMaterialHandle;

            if (vMainView == null)
            {
                vMainView = new VWasteMaterialHandle();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VWasteMaterialHandleCon searchCon = new VWasteMaterialHandleCon(searchList);
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
            } else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CWasteMaterialHandle_ExecType))
                {
                    CWasteMaterialHandle_ExecType execType = (CWasteMaterialHandle_ExecType)obj;
                    switch (execType)
                    {
                        case CWasteMaterialHandle_ExecType.WasteMatHandleQuery:
                            IMainView wmhq = framework.GetMainView("废旧物资处理查询");
                            if (wmhq != null)
                            {
                                wmhq.ViewShow();
                                return null;
                            }
                            VWasteMaterialHandleQuery wmhqy = new VWasteMaterialHandleQuery();
                            wmhqy.ViewCaption = "废旧物资处理查询";
                            framework.AddMainView(wmhqy);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}