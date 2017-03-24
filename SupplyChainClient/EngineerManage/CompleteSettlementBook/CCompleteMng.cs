using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.CompleteSettlementBook;

namespace Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook
{
    public enum CCompleteMng_ExecType
    {
        /// <summary>
        /// 竣工结算书维护      
        /// </summary>
        CompleteQuery
    }

    public class CCompleteMng
    {
        private static IFramework framework = null;
        string mainViewName = "竣工结算单";
        private static VCompleteSearchList searchList;

        public CCompleteMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VCompleteSearchList(this);
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

            VCompleteMng vMainView = framework.GetMainView(mainViewName + "-空") as VCompleteMng;

            if (vMainView == null)
            {
                vMainView = new VCompleteMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VCompleteSearchCon searchCon = new  VCompleteSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CCompleteMng_ExecType))
                {
                    CCompleteMng_ExecType execType = (CCompleteMng_ExecType)obj;
                    switch (execType)
                    {
                        case CCompleteMng_ExecType.CompleteQuery:
                            IMainView mroqMv = framework.GetMainView("竣工结算查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VCompleteQuery vmroq = new VCompleteQuery();
                            vmroq.ViewCaption = "竣工结算查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
