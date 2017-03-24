using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
//using Application.Business.Erp.SupplyChain.SaleManage.SaleBudget.Service;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI
{
    public class CStockOutRed
    {
        private static IFramework framework = null;
        string mainViewName = "���ϳ���쵥";
        private static VStockOutRedList searchListView;

        public CStockOutRed(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchListView = new VStockOutRedList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("��", execType);
        }

        public void Find(string name, EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.��װ)
            {
                mainViewName = "���ϳ���쵥(��װ)";
            } else if (execType == EnumStockExecType.����)
            {
                mainViewName = "���ϳ���쵥(����)";
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

            VStockOutRed mainView = framework.GetMainView(mainViewName + "-��") as VStockOutRed;

            if (mainView == null)
            {
                mainView = new VStockOutRed();
                mainView.ExecType = execType;
                mainView.ViewName = mainViewName;

                //�����ѯ��ͼ
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //���丨����ͼ
                mainView.AssistViews.Add(searchListView);
                VStockOutRedSthCon theVStockOutRedSthCon = new VStockOutRedSthCon(searchListView,execType);
                mainView.AssistViews.Add(theVStockOutRedSthCon);
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
            VStockOutRed vStockOutRed = mv as VStockOutRed;
            if (vStockOutRed != null)
                searchListView.RemoveRow(vStockOutRed.CurBillMaster.Id);

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
                if (o!=null&&o.GetType() == typeof(EnumStockExecType))
                {
                    EnumStockExecType excuteType = (EnumStockExecType)o;

                    switch (excuteType)
                    {
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
