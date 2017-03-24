﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Windows.Documents;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage
{
    public enum CCollectionManage_ExexType
    {
        VCollectionManage,
        /// <summary>
        /// 项目实施策划查询
        /// </summary>
        CollectionManageSearch
    }
    public class CCollectionManage
    {
        private static IFramework framework = null;
        string mainViewName = "收发函管理";
        private static VCollectionSearchList searchList;


        public CCollectionManage(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VCollectionSearchList(this);
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

            VCollectionManage vMainView = framework.GetMainView(mainViewName + "-空") as VCollectionManage;

            if (vMainView == null)
            {
                vMainView = new VCollectionManage();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VCollectionSearchCon searchCon = new VCollectionSearchCon(searchList);
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
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (obj != null && obj.GetType() == typeof(CCollectionManage_ExexType))
                {
                    CCollectionManage_ExexType execType = (CCollectionManage_ExexType)obj;
                    switch (execType)
                    {
                        case CCollectionManage_ExexType.CollectionManageSearch:
                            IMainView mroqMv = framework.GetMainView("收发函查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VCollectionQuery vcq = new VCollectionQuery();
                            vcq.ViewCaption = "收发函查询";
                            vcq.TheAuthMenu = theMenu;
                            framework.AddMainView(vcq);
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