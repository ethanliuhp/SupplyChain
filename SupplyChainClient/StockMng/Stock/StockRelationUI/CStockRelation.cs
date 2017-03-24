using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockRelationUI
{
    public enum StockRelationExcType
    {
        forwardSearch,
        StockRelationSelect,
        manageStateUsable,
        Unlock,
        /// <summary>
        /// 物料价格定义
        /// </summary>
        MaterialPrice,
        /// <summary>
        /// 物料价格定义审核
        /// </summary>
        MaterialPriceAudit
    }
    public class CStockRelation
    {
        private static IFramework framework = null;
        string mainViewName = "库存查询";

        public CStockRelation(IFramework fm)
        {
            if (framework == null)
                framework = fm;

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

            VStockRelation vSaleBudget = framework.GetMainView(mainViewName + "-空") as VStockRelation;

            if (vSaleBudget == null)
            {
                vSaleBudget = new VStockRelation();
                vSaleBudget.ViewName = mainViewName;

                //载入查询视图
                vSaleBudget.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //分配辅助视图
                //vSaleBudget.AssistViews.Add(searchListView);
                //vSaleBudget.theVStockInList = searchListView;
                //载入框架
                framework.AddMainView(vSaleBudget);
            }

            vSaleBudget.ViewCaption = captionName;
            vSaleBudget.ViewName = mainViewName;
            vSaleBudget.Start(name);

            vSaleBudget.ViewShow();
        }

        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object o = args[0];
                StockRelationExcType excuteType = (StockRelationExcType)o;

                switch (excuteType)
                {
                    case StockRelationExcType.StockRelationSelect:
                        VStockRelationSelect theVStockRelationSelect = new VStockRelationSelect();
                        if (args.Length == 2)
                            return theVStockRelationSelect.ShowSelect(args[1] as StationCategory, null, null, "");
                        else if (args.Length == 3)
                            return theVStockRelationSelect.ShowSelect(args[1] as StationCategory, args[2] as SupplierRelationInfo, null, "");
                        else if (args.Length == 4)
                            return theVStockRelationSelect.ShowSelect(args[1] as StationCategory, args[2] as SupplierRelationInfo, args[3] as OperationOrg, "");
                        else if (args.Length == 5)
                            return theVStockRelationSelect.ShowSelect(args[1] as StationCategory, args[2] as SupplierRelationInfo, args[3] as OperationOrg, ClientUtil.ToString(args[4]));
                        else
                            return null;
                    //case StockRelationExcType.manageStateUsable:
                    //    VManageStateUsableQuantity theVManageStateUsableQuantity = new VManageStateUsableQuantity();

                    //    theVManageStateUsableQuantity.ViewCaption = "可开单物料查询";
                    //    theVManageStateUsableQuantity.ViewName = "可开单物料查询";
                    //    framework.AddMainView(theVManageStateUsableQuantity);
                    //    theVManageStateUsableQuantity.ViewShow();
                    //    break;
                    //case StockRelationExcType.Unlock:
                    //    VStockRelationUnLock theVStockRelUnLock = new VStockRelationUnLock();

                    //    theVStockRelUnLock.ViewCaption = "库存解锁";
                    //    theVStockRelUnLock.ViewName = "库存解锁";
                    //    framework.AddMainView(theVStockRelUnLock);
                    //    theVStockRelUnLock.ViewShow();
                    //    break;
                    //case StockRelationExcType.MaterialPrice:
                    //    if (framework.ExistMainView("销售价格制定-空"))
                    //    {
                    //        framework.GetMainView("销售价格制定-空").ViewShow();
                    //    } else
                    //    {
                    //        VMaterialPrice2 vmp = new VMaterialPrice2();
                    //        vmp.ViewCaption = "销售价格制定";
                    //        vmp.ViewName = "销售价格制定";
                    //        framework.AddMainView(vmp);
                    //        vmp.ViewShow();
                    //    }
                    //    break;
                    //case StockRelationExcType.MaterialPriceAudit:
                    //    if(framework.ExistMainView("销售价格审核"))
                    //    {
                    //        framework.GetMainView("销售价格审核").ViewShow();
                    //    }else
                    //    {
                    //        VMaterialPriceAudit2 vmpa = new VMaterialPriceAudit2();
                    //        vmpa.ViewCaption = "销售价格审核";
                    //        vmpa.ViewName = "销售价格审核";
                    //        framework.AddMainView(vmpa);
                    //        vmpa.ViewShow();
                    //    }

                    //    break;
                }
            }
            return null;
        }
    }
}
