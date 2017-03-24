using System;
using System.Collections.Generic;
using System.Text;

using VirtualMachine.Component.WinMVC.core;
using VirtualMachine.Component.WinMVC.generic;

using VirtualMachine.Component.WinControls.Controls;

using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StationCategoryUI
{
    public enum StationCategoryExcuteType
    {
        OpenSelect,        
        CommonSelect
    }
    public class CStationCategory
    {
        static IFramework framework;

        public CStationCategory(IFramework fw)
        {
            if (framework==null)
                framework = fw;
        }
        public void Start()
        {
            string viewName = "²Ö¿â·ÖÀàÎ¬»¤";
            IMainView mv = framework.GetMainView(viewName);
            if (mv != null)
                mv.ViewShow();
            else
            {
                VStationCategory vmc = new VStationCategory(this);
                vmc.ViewCaption = viewName;
                framework.AddMainView(vmc);
                vmc.Start();
            }
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
                Start();
            else
            {
                object o = args[0];
                StationCategoryExcuteType excuteType = (StationCategoryExcuteType)o;
                ObjectQuery oq = null;
                if (args.Length > 1)
                    oq = args[1] as ObjectQuery;
                switch (excuteType)
                {
                    case StationCategoryExcuteType.CommonSelect:
                        VCommonStationCategory aa = new VCommonStationCategory();
                        aa.OpenSelectView(oq, args[2] as IWin32Window);
                        return aa.Result;
                    default:
                        break;
                }  
            }
            return null;
        }
        public void UpdateObjectPreview(object o)
        {
            IView v = o as IView;
            framework.UpdateObjectPreview(v);
        }
    }
}
