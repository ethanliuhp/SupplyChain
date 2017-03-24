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
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI
{
    public enum CostPtojectType
    {
        costProjectSelect
    }
    /// <summary>
    /// 成本项目分类
    /// </summary>
    public class CCostProject
    {
        static IFramework framework;
        static ICostProjectSrv titleSrv = null;

        private string viewName;
        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }



        public CCostProject(IFramework fw)
        {
            if (framework == null)
                framework = fw;     //指明Framework
            if (titleSrv == null)
                titleSrv = StaticMethods.GetService(typeof(ICostProjectSrv)) as ICostProjectSrv; //注入服务
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
                MCostProject model = new MCostProject();
                model.titleService = titleSrv;
                VCostProject mainView = new VCostProject(model);
                mainView.ViewCaption = ViewName;

                //显示进度条
                // (framework as Framework).ShowProcessBar();


                //if (!ConstObject.FrameWorkNewFlag)
                //{
                framework.AddMainView(mainView);
                //}
                //else
                //{
                //    UCL.mvv = mainView;
                //}
                mainView.Start();
                //关闭进度条
                // (framework as Framework).CloseProcessBar();
            }
            //}
            //else
            //{
            //    MCostProject model = new MCostProject();
            //    model.titleService = titleSrv;
            //    VCostProject mainView = new VCostProject(model);
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
                MCostProject model = new MCostProject();
                model.titleService = titleSrv;
                VCostProject mainView = new VCostProject(model);
                // mainView.Menu = currMenu;
                mainView.ViewCaption = ViewName;
                mainView.Start();
                //if (!ConstObject.FrameWorkNewFlag)
                //{
                framework.AddMainView(mainView);
                //}
                //else
                //{
                //    UCL.mvv = mainView;
                //}
            }
            //}
            //else
            //{
            //    MCostProject model = new MCostProject();
            //    model.titleService = titleSrv;
            //    VCostProject mainView = new VCostProject(model);
            //    mainView.ViewCaption = ViewName;
            //    UCL.mvv = mainView;

            //    mainView.Start();

            //}

        }

        public object Excute(SysMenu currMenu, params object[] args)
        {
            Start(currMenu, args);
            return null;
        }

        public object Excute(string viewName, params object[] args)
        {
            if (args.Length == 0)
            {
                this.ViewName = viewName;
                Start();
            }
            return null;
        }
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                this.ViewName = viewName;
                Start();
            }
            else
            {
                object o = args[0];
                if (o.GetType() == typeof(CostPtojectType))
                {
                    CostPtojectType excuteType = (CostPtojectType)o;

                    switch (excuteType)
                    {
                        case CostPtojectType.costProjectSelect:
                            VCostProjectSelect theVCostProjectSelect = new VCostProjectSelect();
                            theVCostProjectSelect.ShowDialog();
                            return theVCostProjectSelect.Result;
                    }
                }
                //else if (o.GetType() == typeof(long))
                //{
                //    Find(ClientUtil.ToString(o));
                //}
            }
            return null;
        }
    }
}
