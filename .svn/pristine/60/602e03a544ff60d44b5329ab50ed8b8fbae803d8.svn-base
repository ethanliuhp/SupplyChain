using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng
{
    //public enum CMaterialSettleMng_ExecType
    //{
    //    /// <summary>
    //    /// 物资耗用结算查询
    //    /// </summary>
    //    MaterialSettleQuery
    //}
    
    public class CMaterialSettleMng
    {
        private static IFramework framework = null;
        string mainViewName = "材料耗用结算单维护";
        private static VMaterialSettleSearchList searchList;

        public CMaterialSettleMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialSettleSearchList(this);
        }

        public void Start(EnumMaterialSettleType SettleType)
        {
            Find("空", SettleType, "空");
        }

        public void Find(string name, EnumMaterialSettleType SettleType, string Id)
        {
            if (SettleType == EnumMaterialSettleType.物资耗用结算单维护)
            {
                mainViewName = "材料耗用结算单维护";
            }
            if (SettleType == EnumMaterialSettleType.料具结算单维护)
            {
                mainViewName = "料具租赁结算单维护";
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

            VMaterialSettleMng vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialSettleMng;

            if (vMainView == null)
            {
                vMainView = new VMaterialSettleMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConstructionDesignSearchCon searchCon = new VConstructionDesignSearchCon(searchList, SettleType);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id, SettleType);

            vMainView.ViewShow();
        }

        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VMaterialSettleMng vDmand = mv as VMaterialSettleMng;
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
            }
            else
            {
                object obj = args[0];
                if (obj != null && typeof(EnumMaterialSettleType) == obj.GetType())
                {
                    EnumMaterialSettleType excuteType = (EnumMaterialSettleType)obj;
                    switch (excuteType)
                    {
                        case EnumMaterialSettleType.物资耗用结算单维护:
                        case EnumMaterialSettleType.料具结算单维护:
                            Start(excuteType);
                            break;
                        case EnumMaterialSettleType.materialSettleQuery:
                            IMainView mroqMv = framework.GetMainView("材料耗用结算单查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialSettleQuery vmroq = new VMaterialSettleQuery(excuteType);
                            //vmroq.StockInManner = EnumDemandType.demandSearch;
                            vmroq.ViewCaption = "材料耗用结算单查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case EnumMaterialSettleType.materialQuery:
                            IMainView mroqMv11 = framework.GetMainView("料具租赁结算单查询");
                            if (mroqMv11 != null)
                            {
                                mroqMv11.ViewShow();
                                return null;
                            }
                            VMaterialSettleQuery vmroq11 = new VMaterialSettleQuery(excuteType);
                            //vmroq.StockInManner = EnumDemandType.demandSearch;
                            vmroq11.ViewCaption = "料具租赁结算单查询";
                            framework.AddMainView(vmroq11);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
