using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng
{
    public enum CDetectionReceiptMng_ExecType
    {
        /// <summary>
        /// 检测回执单查询
        /// </summary>
        DetectionReceiptQuery
    }
    
    public class CDetectionReceiptMng
    {
        private static IFramework framework = null;
        string mainViewName = "检测回执单";
        private static VDetectionReceiptSearchList searchList;

        public CDetectionReceiptMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VDetectionReceiptSearchList(this);
        }

        public void Start()
        {
            Find("空");
        }

        public void Find(string name)
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

            VDetectionReceipt vMainView = framework.GetMainView(mainViewName + "-空") as VDetectionReceipt;

            if (vMainView == null)
            {
                vMainView = new VDetectionReceipt();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VDetectionReceiptSearchCon searchCon = new VDetectionReceiptSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(name);

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
                if (obj != null && obj.GetType() == typeof(CDetectionReceiptMng_ExecType))
                {
                    CDetectionReceiptMng_ExecType execType = (CDetectionReceiptMng_ExecType)obj;
                    switch (execType)
                    {
                        case CDetectionReceiptMng_ExecType.DetectionReceiptQuery:
                            IMainView mroqMv = framework.GetMainView("检测回执查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VDetectionReceiptQuery vmroq = new VDetectionReceiptQuery();
                            vmroq.ViewCaption = "检测回执查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
