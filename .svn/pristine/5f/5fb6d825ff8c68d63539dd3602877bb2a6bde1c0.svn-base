using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    public enum EnumMaterialHireOrder_ExecType
    {
        料具租赁合同 = 0,
        料具租赁合同查询 = 1,
        料具租赁合同引用 = 2,
        料具租赁复制 = 3,

        料具封存单 = 4,
        碗扣封存单 = 5,
        钢管封存单=6,
        料具封存单查询=7
    }
    public class CMaterialHireOrder
    {
        private static IFramework framework = null;
        //string mainViewName = "料具租赁合同";

        public CMaterialHireOrder(IFramework oFramework)
        {
            //framework = oFramework;
            if (framework == null)
            {
                framework = oFramework;
            }

        }
        public void Start(EnumMaterialHireOrder_ExecType collectionType)
        {
            Find("空", "空", collectionType);
        }

        public void Find(string name, string id, EnumMaterialHireOrder_ExecType collectionType)
        {
         

            switch (collectionType)
            {
                case EnumMaterialHireOrder_ExecType.料具租赁合同:
                    {
                        string mainViewName = collectionType.ToString();
                        string captionName = mainViewName;
                        if (name is string)
                        {
                            captionName = mainViewName + "-" + name;
                        }

                        IMainView mv = framework.GetMainView(captionName);

                        if (mv != null)
                        {
                            //如果当前视图已经存在，直接显示
                            mv.ViewShow();
                            return;
                        }
                        VMaterialHireOrder vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialHireOrder;
                        if (vMainView == null)
                        {
                            vMainView = new VMaterialHireOrder();
                            vMainView.ViewName = mainViewName;

                            VMaterialHireOrderSearchList searchList = new VMaterialHireOrderSearchList(this);
                            //载入查询视图
                            //分配辅助视图
                            vMainView.AssistViews.Add(searchList);
                            VMaterialHireOrderSearchCon searchCon = new VMaterialHireOrderSearchCon(searchList, collectionType);

                            vMainView.AssistViews.Add(searchCon);

                            //载入框架
                            framework.AddMainView(vMainView);
                        }

                        vMainView.ViewCaption = captionName;
                        vMainView.ViewName = mainViewName;
                        vMainView.Start(id);

                        vMainView.ViewShow();
                        break;
                    }
                case EnumMaterialHireOrder_ExecType.料具封存单:
                case EnumMaterialHireOrder_ExecType.钢管封存单:
                case EnumMaterialHireOrder_ExecType.碗扣封存单:
                    {
                        string mainViewName = collectionType == EnumMaterialHireOrder_ExecType.料具封存单?"料具封存单":
                             (EnumMaterialHireOrder_ExecType.钢管封存单 == collectionType ? "料具封存单(钢管)" : "料具封存单(碗扣)"); ;
                        string captionName = mainViewName;
                        if (name is string)
                        {
                            captionName = mainViewName + "-" + name;
                        }

                        IMainView mv = framework.GetMainView(captionName);

                        if (mv != null)
                        {
                            //如果当前视图已经存在，直接显示
                            mv.ViewShow();
                            return;
                        }
                        EnumMatHireType MatHireType;
                        MatHireType = EnumMaterialHireOrder_ExecType.料具封存单 == collectionType ? EnumMatHireType.普通料具 :
                            (EnumMaterialHireOrder_ExecType.钢管封存单 == collectionType ? EnumMatHireType.钢管 : EnumMatHireType.碗扣);
                        VStockBlockMaterial vMainView = framework.GetMainView(mainViewName + "-空") as VStockBlockMaterial;

                        if (vMainView == null)
                        {
                            vMainView = new VStockBlockMaterial(MatHireType);
                            vMainView.ViewName = mainViewName;
                            VStockBlockMaterialSearchList searchList = new VStockBlockMaterialSearchList(this, MatHireType);
                            //载入查询视图
                            //分配辅助视图
                            vMainView.AssistViews.Add(searchList);
                            VStockBlockMaterialSearchCon searchCon = new VStockBlockMaterialSearchCon(searchList, MatHireType);
                            vMainView.AssistViews.Add(searchCon);
                            //载入框架
                            framework.AddMainView(vMainView);
                        }
                        vMainView.ViewCaption = captionName;
                        vMainView.ViewName = mainViewName;
                        vMainView.Start(id);
                        vMainView.ViewShow();
                        break;
                    }
            }
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start(EnumMaterialHireOrder_ExecType.料具租赁合同);
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(EnumMaterialHireOrder_ExecType))
                {
                    EnumMaterialHireOrder_ExecType execType = (EnumMaterialHireOrder_ExecType)obj;
                    switch (execType)
                    {
                        case EnumMaterialHireOrder_ExecType.料具租赁合同:
                            {
                                Start(EnumMaterialHireOrder_ExecType.料具租赁合同);
                                return null;
                            }
                        case EnumMaterialHireOrder_ExecType.料具租赁合同查询:
                            {
                                IMainView mroqMv = framework.GetMainView("料具租赁合同查询");
                                if (mroqMv != null)
                                {
                                    mroqMv.ViewShow();
                                    return null;
                                }
                                VMaterialHireOrderQuery vmroq = new VMaterialHireOrderQuery();
                                vmroq.ViewCaption = "料具租赁合同查询";
                                framework.AddMainView(vmroq);
                                return null;
                            }
                        case EnumMaterialHireOrder_ExecType.料具租赁复制:
                            {
                                IMainView mroqMv = framework.GetMainView("料具租赁复制");
                                if (mroqMv != null)
                                {
                                    mroqMv.ViewShow();
                                    return null;
                                }
                                VMaterialHireOrderCopy vmroq = new VMaterialHireOrderCopy();
                                vmroq.ViewCaption = "料具租赁复制";
                                framework.AddMainView(vmroq);
                                return null;
                            }
                        //case EnumMaterialHireOrder_ExecType.料具租赁合同引用:
                        //    {
                        //        IMainView mroqMv = framework.GetMainView("料具租赁合同引用");
                        //        if (mroqMv != null)
                        //        {
                        //            mroqMv.ViewShow();
                        //            return null;
                        //        }
                        //        VMaterialHireOrderSelector vmroq = new VMaterialHireOrderSelector();
                        //        vmroq.ViewCaption = "料具租赁合同引用";
                        //        framework.AddMainView(vmroq);
                        //        return null;
                        //    }
                        case EnumMaterialHireOrder_ExecType.料具封存单:
                        case EnumMaterialHireOrder_ExecType.钢管封存单:
                        case EnumMaterialHireOrder_ExecType.碗扣封存单:
                            {
                                Start(execType);
                                return null;
                            }
                        case EnumMaterialHireOrder_ExecType.料具封存单查询:
                            {
                                IMainView mroqMv = framework.GetMainView("料具封存单查询");
                                if (mroqMv != null)
                                {
                                    mroqMv.ViewShow();
                                    return null;
                                }
                                VStockBlockMaterialQuery vmroq = new VStockBlockMaterialQuery();
                                vmroq.ViewCaption = "料具封存单查询";
                                framework.AddMainView(vmroq);
                                return null;
                            }

                    }
                }
            }
            return null;
        }
    }
}