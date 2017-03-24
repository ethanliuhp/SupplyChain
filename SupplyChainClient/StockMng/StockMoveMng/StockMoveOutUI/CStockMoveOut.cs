using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
//using Application.Business.Erp.SupplyChain.SaleManage.SaleBudget.Service;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI
{

    public class CStockMoveOut
    {
        private static IFramework framework = null;
        string mainViewName = "�������ⵥ";

        private static VStockMoveOutSearchList searchListView;


        public CStockMoveOut(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchListView = new VStockMoveOutSearchList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("��",execType);
        }

        public void Find(string name,EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.��װ)
            {
                mainViewName = "�������ⵥ(��װ)";
            } else if(execType==EnumStockExecType.����)
            {
                mainViewName = "�������ⵥ(����)";
            }
            string captionName = mainViewName;
            if (name is string)
            {
                captionName = this.mainViewName + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                mv.ViewShow();
                return;
            }

            VStockMoveOut mainView = framework.GetMainView(mainViewName + "-��") as VStockMoveOut;
            if (mainView == null)
            {
                mainView = new VStockMoveOut();
                mainView.ExecType = execType;
                mainView.ViewName = mainViewName;

                //�����ѯ��ͼ
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //���丨����ͼ
                mainView.AssistViews.Add(searchListView);
                VStockMoveOutSearchCon theVStockOutSthCon = new VStockMoveOutSearchCon(searchListView,execType);
                mainView.AssistViews.Add(theVStockOutSthCon);
                //������
                framework.AddMainView(mainView);
            }

            mainView.ViewCaption = captionName;
            mainView.ViewName = mainViewName;
            mainView.Start(name);
            mainView.ViewShow();
        }


        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VStockMoveOut vStockOut = mv as VStockMoveOut;
            if (vStockOut != null)
                searchListView.RemoveRow(vStockOut.CurBillMaster.Id);

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
                object o = args[0];
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (o != null && o.GetType() == typeof(EnumStockExecType))
                {
                    EnumStockExecType excuteType = (EnumStockExecType)o;
                    switch (excuteType)
                    {
                        case EnumStockExecType.StockMoveOutQuery:
                            IMainView vsoqMV = framework.GetMainView("���������ѯ");
                            if (vsoqMV != null)
                            {
                                vsoqMV.ViewShow();
                                return null;
                            }
                            VStockOutQuery vsoq = new VStockOutQuery("���������ѯ");
                            vsoq.TheAuthMenu = theMenu;
                            vsoq.StockInOutManner = EnumStockInOutManner.��������;
                            vsoq.ViewCaption = "���������ѯ";
                            framework.AddMainView(vsoq);
                            vsoq.ViewShow();
                            return null;
                        case EnumStockExecType.��������ά��:
                            IMainView siqMv = framework.GetMainView("��������ά��");
                            if (siqMv != null)
                            {
                                siqMv.ViewShow();
                                return null;
                            }
                            VSetStockRelationIdleQuantity vsriq = new VSetStockRelationIdleQuantity();
                            vsriq.ViewCaption = "��������ά��";
                            framework.AddMainView(vsriq);
                            return null;
                        case EnumStockExecType.��˾�����ѯ:
                            IMainView mv = framework.GetMainView("��˾�����ѯ");
                            if (mv != null)
                            {
                                mv.ViewShow();
                                return null;
                            }
                            VStockRelationIdleQuantityQuery vsriqq = new VStockRelationIdleQuantityQuery();
                            vsriqq.ViewCaption = "��˾�����ѯ";
                            framework.AddMainView(vsriqq);
                            return null;
                        case EnumStockExecType.����:
                        case EnumStockExecType.��װ:
                            Start(excuteType);
                            break;
                    }
                }
                else if (o.GetType() == typeof(string))
                {
                    //Find(o.ToString());
                }

            }

            return null;
        }
    }
}
