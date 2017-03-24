using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng
{
    public enum CConPouringNote_ExceType
    {
        /// <summary>
        /// 浇筑记录单查询
        /// </summary>
        ConPouringNoteQuery
    }
    public class CConPouringNote
    {
        private static IFramework framework = null;
        string mainViewName = "浇筑记录单";
        private static VConPouringNoteSearchList searchList;

        public CConPouringNote(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VConPouringNoteSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }
        public void Find(string name, string id)
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
            VConPouringNote vMainView = framework.GetMainView(mainViewName + "-空") as VConPouringNote;

            if (vMainView == null)
            {
                vMainView = new VConPouringNote();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConPouringNoteSearchCon searchCon = new VConPouringNoteSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(id);

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
                if (obj != null && obj.GetType() == typeof(CConPouringNote_ExceType))
                {
                    CConPouringNote_ExceType execType = (CConPouringNote_ExceType)obj;
                    switch (execType)
                    {
                        case CConPouringNote_ExceType.ConPouringNoteQuery:
                            IMainView mroqMv = framework.GetMainView("浇筑记录单统计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VConPouringNoteQuery vcpnq = new VConPouringNoteQuery();
                            vcpnq.ViewCaption = "浇筑记录单统计查询";
                            framework.AddMainView(vcpnq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
