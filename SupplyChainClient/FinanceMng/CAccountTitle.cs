using System;
using System.Collections.Generic;
using System.Text;
//using Application.Business.Erp.Financial.Client.Main;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;
using VirtualMachine.SystemAspect.Security.FunctionSecurity.Domain;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;
using Application.Business.Erp.SupplyChain.Client.FinanceMng;

namespace Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI
{
    public enum AccountTitleType
    {
        会计科目=0,
        会计科目树=1
    }
    /// <summary>
    /// 会计科目
    /// </summary>
    public class CAccountTitle
    {
        static IFramework framework;
        static IAccountTitleService titleSrv = null;
        static IAccountTitleTreeSvr accountTitleTreeSvr;
        private string viewName;
        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }



        public CAccountTitle(IFramework fw)
        {
            if (framework==null)
                framework = fw;     //指明Framework
            if (titleSrv == null)
                titleSrv = StaticMethods.GetService(typeof(IAccountTitleService)) as IAccountTitleService; //注入服务
            if (accountTitleTreeSvr == null)
            {
                accountTitleTreeSvr = StaticMethods.GetService(typeof(IAccountTitleTreeSvr)) as IAccountTitleTreeSvr; //注入服务
            }

        }

        public void Start()
        {
            //if (!ConstObject.FrameWorkNewFlag)
            //{
                IMainView mv = framework.GetMainView(ViewName);
                if (mv != null)
                    mv.ViewShow();
                else
                {
                    MAccountTitle model = new MAccountTitle();
                    model.titleService = titleSrv;
                    model.AccountTitleTreeSvr = accountTitleTreeSvr;
                    VAccountTitleTree mainView = new VAccountTitleTree(model);
                    mainView.ViewCaption = "会计科目";

                    //显示进度条
                    // (framework as Framework).ShowProcessBar();


                    framework.AddMainView(mainView);
                    mainView.Start();
                    //关闭进度条
                    // (framework as Framework).CloseProcessBar();
                }
            //}
            //else
            //{
            //    MAccountTitle model = new MAccountTitle();
            //    model.titleService = titleSrv;
            //    VAccountTitle mainView = new VAccountTitle(model);
            //    mainView.ViewCaption = ViewName;

            //    UCL.mvv = mainView;
            //    mainView.Start();
            //}

        }

        public void Start(SysMenu currMenu, params object[] args)
        {
            this.viewName = currMenu.Name;
            //if (!ConstObject.FrameWorkNewFlag)
            //{
                IMainView mv = framework.GetMainView(ViewName);
                if (mv != null)
                    mv.ViewShow();
                else
                {
                    MAccountTitle model = new MAccountTitle();
                    model.titleService = titleSrv;
                    VAccountTitle mainView = new VAccountTitle(model);
                    // mainView.Menu = currMenu;
                    mainView.ViewCaption = ViewName;
                    //显示进度条
                    //(framework as Framework).ShowProcessBar();

                    mainView.Start();
                    framework.AddMainView(mainView);

                    //关闭进度条
                    //(framework as Framework).CloseProcessBar();
                }
            //}
            //else
            //{
            //    MAccountTitle model = new MAccountTitle();
            //    model.titleService = titleSrv;
            //    VAccountTitle mainView = new VAccountTitle(model);
            //    // mainView.Menu = currMenu;
            //    mainView.ViewCaption = ViewName;
            //    UCL.mvv = mainView;
            //    mainView.Start();
            //}
        }

        //public object Excute(SysMenu currMenu, params object[] args)
        //{
        //    Start(currMenu, args);
        //    return null;
        //}

        //public object Excute(string viewName,params object[] args)
        //{
        //    if (args.Length == 0)
        //    {
        //        this.ViewName = viewName;
        //        Start();
        //    }
        //    return null;
        //}
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object o = args[0];
                AccountTitleType executeType = (AccountTitleType)o;

                switch (executeType)
                {
                    case AccountTitleType.会计科目:
                        {
                            string viewName = "会计科目";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                MAccountTitle mmc = new MAccountTitle();
                                mmc.titleService = titleSrv;
                                mmc.AccountTitleTreeSvr = accountTitleTreeSvr;
                                VAccountTitle vmc = new VAccountTitle(mmc);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case AccountTitleType.会计科目树:
                        {
                            string viewName = "会计科目";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                MAccountTitle mmc = new MAccountTitle();
                                mmc.titleService = titleSrv;
                                mmc.AccountTitleTreeSvr = accountTitleTreeSvr;
                                VAccountTitleTree vmc = new VAccountTitleTree(mmc);
                                
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    default:
                        {
                            Start();
                            break;
                        }
                }
            }

            return null;
        }
    }
}
