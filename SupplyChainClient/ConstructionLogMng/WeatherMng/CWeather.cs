using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng
{
    public enum CWeatherMng_ExecType
    {
        /// <summary>
        /// 晴雨表查询
        /// </summary>
        WeatherQuery,
        /// <summary>
        /// 二维码管理
        /// </summary>
        QRCode
    }

    public class CWeather
    {
        private static IFramework framework = null;
        string mainViewName = "晴雨表信息";
        private static VWeatherSearchList searchList;

        public CWeather(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VWeatherSearchList(this);
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

            VWeatherMng vMainView = framework.GetMainView(mainViewName + "-空") as VWeatherMng;

            if (vMainView == null)
            {
                vMainView = new VWeatherMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VWeatherSearchCon searchCon = new VWeatherSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            } else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CWeatherMng_ExecType))
                {
                    CWeatherMng_ExecType execType = (CWeatherMng_ExecType)obj;
                    switch (execType)
                    {
                        case CWeatherMng_ExecType.WeatherQuery:
                            IMainView mroqMv = framework.GetMainView("晴雨表信息查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VWeatherQuery vmroq = new VWeatherQuery();
                            vmroq.ViewCaption = "晴雨表信息查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case CWeatherMng_ExecType.QRCode:
                            IMainView imv2 = framework.GetMainView("二维码管理");
                            if (imv2 != null)
                            {
                                imv2.ViewShow();
                                return null;
                            }
                            VQRCodeMng vqrcm = new VQRCodeMng();
                            vqrcm.ViewCaption = "二维码管理";
                            framework.AddMainView(vqrcm);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
