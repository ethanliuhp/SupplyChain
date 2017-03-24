using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public enum CProjectTaskAccountExecType
    {
        工程任务核算维护 = 1,
        工程任务核算查询 = 2,
        工程任务核算维护_虚拟工单 = 3,
        工程量确认单 = 4
    }

    public class CProjectTaskAccount
    {
        private static IFramework framework = null;
        private string mainViewName = "工程任务核算维护";
        private static VProjectTaskAccountBillSearchList searchList;

        public CProjectTaskAccount(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VProjectTaskAccountBillSearchList(this);
        }

        public void Start()
        {
            Find("空", string.Empty);
        }

        public void Start2()
        {
            Find2("空", string.Empty);
        }

        public void Find(string code, string GUID)
        {
            string captionName = this.mainViewName + "-" + code;

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VProjectTaskAccountBill vMainView = new VProjectTaskAccountBill();

            //载入查询视图
            //分配辅助视图
            vMainView.AssistViews.Add(searchList);

            VProjectTaskAccountBillSearchCon searchCon = new VProjectTaskAccountBillSearchCon(searchList);
            vMainView.AssistViews.Add(searchCon);

            //载入框架
            framework.AddMainView(vMainView);

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(code, GUID);

            vMainView.ViewShow();
        }

        public void Find2(string code, string GUID)
        {
            mainViewName = "工程任务核算单维护";
            string captionName = this.mainViewName + "-" + code;

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VTaskAccountByVirtualConfirmBill vMainView = new VTaskAccountByVirtualConfirmBill();

            //载入查询视图
            //分配辅助视图
            vMainView.AssistViews.Add(searchList);

            VProjectTaskAccountBillSearchCon searchCon = new VProjectTaskAccountBillSearchCon(searchList);
            vMainView.AssistViews.Add(searchCon);

            //载入框架
            framework.AddMainView(vMainView);

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(code, GUID);

            vMainView.ViewShow();
        }

        public void Find3(string code, string GUID)
        {
            mainViewName = "工程量确认单";
            string captionName = this.mainViewName + "-" + code;

            IMainView mv = framework.GetMainView(captionName);
            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VQuantitiesAffirm vMainView = new VQuantitiesAffirm();

            //载入查询视图
            //分配辅助视图
            var searList = new VQuantitiesAffirmSearchList(this);
            vMainView.AssistViews.Add(searList);

            VQuantitiesAffirmSearchCon searchCon = new VQuantitiesAffirmSearchCon(searList);
            vMainView.AssistViews.Add(searchCon);

            //载入框架
            framework.AddMainView(vMainView);

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(code, GUID);
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
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (obj != null && obj.GetType() == typeof (CProjectTaskAccountExecType))
                {
                    IMainView mainView = null;
                    CProjectTaskAccountExecType execType = (CProjectTaskAccountExecType) obj;
                    switch (execType)
                    {
                        case CProjectTaskAccountExecType.工程任务核算维护:
                            mainView = framework.GetMainView("工程任务核算维护");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                break;
                            }
                            VProjectTaskAccountBill frm = new VProjectTaskAccountBill();
                            frm.ViewCaption = "工程任务核算维护";
                            framework.AddMainView(frm);
                            break;
                        case CProjectTaskAccountExecType.工程任务核算维护_虚拟工单:
                            Start2();
                            break;
                        case CProjectTaskAccountExecType.工程任务核算查询:
                            mainView = framework.GetMainView("工程任务核算查询");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                break;
                            }
                            VProjectTaskAccountBillQuery frmQuery = new VProjectTaskAccountBillQuery(this);
                            frmQuery.TheAuthMenu = theMenu;
                            frmQuery.ViewCaption = "工程任务核算查询";
                            framework.AddMainView(frmQuery);
                            break;
                        case CProjectTaskAccountExecType.工程量确认单:
                            mainViewName = "工程量确认单";
                            Find3("空", string.Empty);
                            break;
                    }
                }
            }
            return null;
        }
    }
}
