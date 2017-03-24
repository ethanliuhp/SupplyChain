using System;
using System.Collections.Generic;
using System.Text;

using VirtualMachine.Component.WinMVC.core;
using VirtualMachine.Component.WinMVC.generic;

using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.ResourceManager.Client.Main;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng
{
    public enum CWasteMaterialHandle_ExecType
    {
        /// <summary>
        /// 施工专业基础数据查询
        /// </summary>
        ConstructionData
    }
    
    public class CConstructionData
    {
        private static IFramework framework = null;
        string mainViewName = "施工专业基础表";

        public CConstructionData(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }
       
        public object Excute(params object[] args)
        {
            object obj = args[0];
            if (obj != null && obj.GetType() == typeof(CWasteMaterialHandle_ExecType))
            {
                CWasteMaterialHandle_ExecType execType = (CWasteMaterialHandle_ExecType)obj;
                switch (execType)
                {
                    case CWasteMaterialHandle_ExecType.ConstructionData:
                        IMainView mroqMv = framework.GetMainView("施工专业基础表");
                        if (mroqMv != null)
                        {
                            mroqMv.ViewShow();
                            return null;
                        }
                        VConstruction vmroq = new VConstruction();
                        vmroq.ViewCaption = "施工专业基础表";
                        framework.AddMainView(vmroq);
                        return null;

                }
            }
            return null; 
        }
    }
}
