using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn
{
    public enum CMaterialReturn_ExecType
    {
        /// <summary>
        /// 料具退料单查询
        /// </summary>
        MaterialReturnQuery
    }

    public class CMaterialReturn
    {
        private static IFramework framework = null;
        string mainViewName = "料具退料单";
        private static VMaterialReturnSearchList searchList;

        public CMaterialReturn(IFramework fm, int returnType)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialReturnSearchList(this, returnType);
        }

        public void Start(int type)
        {
            Find("空", "空", type);
        }

        public void Find(string name, string id, int type)
        {
            if (type == 1)
            {
                mainViewName = "料具退料单";
            }
            else if (type == 2)
            {
                mainViewName = "料具退料单(损耗)";
            }
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
            VMaterialReturn vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialReturn;

            if (vMainView == null)
            {
                vMainView = new VMaterialReturn(type);
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMaterialReturnSearchCon searchCon = new VMaterialReturnSearchCon(searchList, type);

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
            object obj = args[0];
            if (Convert.ToInt32(obj) == 1)
            {
                Start(Convert.ToInt32(obj));
            }
            else if (Convert.ToInt32(obj) == 2)
            {
                Start(Convert.ToInt32(obj));
            }
            else
            {
                //object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CMaterialReturn_ExecType))
                {
                    CMaterialReturn_ExecType execType = (CMaterialReturn_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialReturn_ExecType.MaterialReturnQuery:
                            IMainView mroqMv = framework.GetMainView("退料单统计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialReturnQuery vmcq = new VMaterialReturnQuery();
                            vmcq.ViewCaption = "退料单统计查询";
                            framework.AddMainView(vmcq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
