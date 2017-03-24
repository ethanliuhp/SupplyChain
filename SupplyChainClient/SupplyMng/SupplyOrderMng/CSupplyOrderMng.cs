using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng
{
    //public enum CSupplyOrderMng_ExecType
    //{
    //    /// <summary>
    //    /// 采购合同查询
    //    /// </summary>
    //    SupplyOrderQuery
    //}
    
    public class CSupplyOrderMng
    {
        private static IFramework framework = null;
        string mainViewName = "采购合同单";
        private static VSupplyOrderSearchList searchList;

        public CSupplyOrderMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VSupplyOrderSearchList(this);
        }

        public void Start(EnumSupplyType execType)
        {
            Find("空", execType,"空");
            
        }

        public void Find(string name, EnumSupplyType execType,string Id)
        {
            if (execType == EnumSupplyType.土建)
            {
                mainViewName = "采购合同单(土建)";
            }
            else if (execType == EnumSupplyType.安装)
            {
                mainViewName = "采购合同单(安装)";
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

            VSupplyOrder vMainView = framework.GetMainView(mainViewName + "-空") as VSupplyOrder;

            if (vMainView == null)
            {
                vMainView = new VSupplyOrder();
                vMainView.SupplyType = execType;
                vMainView.ViewName = mainViewName;

                //载入查询视图
                vMainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VSupplyOrderSearchCon searchCon = new VSupplyOrderSearchCon(searchList, execType);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VSupplyOrder vDmand = mv as VSupplyOrder;
            if (vDmand != null)
                searchList.RemoveRow(vDmand.CurBillMaster.Id);


            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            } else
            {
                object obj=args[0];
                if (obj != null && typeof(EnumSupplyType) == obj.GetType())
                {
                    EnumSupplyType excuteType = (EnumSupplyType)obj;
                    switch (excuteType)
                    {
                        case EnumSupplyType.土建:
                        case EnumSupplyType.安装:
                            Start(excuteType);
                            break;
                        case EnumSupplyType.supplySearch:
                            IMainView mroqMv = framework.GetMainView("采购合同查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VSupplyOrderQuery vmroq = new VSupplyOrderQuery();
                            vmroq.ViewCaption = "采购合同查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
