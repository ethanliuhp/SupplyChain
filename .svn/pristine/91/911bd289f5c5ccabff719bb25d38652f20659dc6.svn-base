using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany
{
    //public enum CSupplyOrderMng_ExecType
    //{
    //    /// <summary>
    //    /// 采购合同查询
    //    /// </summary>
    //    SupplyOrderQuery
    //}
    
    public class CSupplyOrderMngCompany
    {
        private static IFramework framework = null;
        string mainViewName = "采购合同单(公司)";
        private static VSupplyOrderSearchListCompany searchList;

        public CSupplyOrderMngCompany(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VSupplyOrderSearchListCompany(this);
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

            VSupplyOrderCompany vMainView = framework.GetMainView(mainViewName + "-空") as VSupplyOrderCompany;

            if (vMainView == null)
            {
                vMainView = new VSupplyOrderCompany();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                vMainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VSupplyOrderSearchConCompany searchCon = new VSupplyOrderSearchConCompany(searchList);

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

            VSupplyOrderCompany vDmand = mv as VSupplyOrderCompany;
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
                Start();
            } else
            {
                object obj=args[0];
                if (obj != null && typeof(EnumSupplyType) == obj.GetType())
                {
                    EnumSupplyType excuteType = (EnumSupplyType)obj;
                    switch (excuteType)
                    {
                        case EnumSupplyType.supplySearch:
                            IMainView mroqMv = framework.GetMainView("采购合同查询(公司)");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VSupplyOrderQueryCompany vmroq = new VSupplyOrderQueryCompany();
                            vmroq.ViewCaption = "采购合同查询(公司)";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
