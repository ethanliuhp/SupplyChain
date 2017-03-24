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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OBS
{
       public enum CDOBS_ExecType
    {
        /// <summary>
        /// OBS查询
        /// </summary>
        OBSManage,

        OBSService
    }
    
    public class COBS
    {
        private static IFramework framework = null;
        string mainViewName = "OBS";

        public COBS(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }
        public object Excute(params object[] args)
        {
            object obj = args[0];
            if (obj != null && obj.GetType() == typeof(CDOBS_ExecType))
            {
                CDOBS_ExecType execType = (CDOBS_ExecType)obj;
                switch (execType)
                {
                    case CDOBS_ExecType.OBSManage:
                        IMainView mroqMv = framework.GetMainView("管理OBS");
                        if (mroqMv != null)
                        {
                            mroqMv.ViewShow();
                            return null;
                        }
                        else
                        {
                            MOperationJob mpd = new MOperationJob();
                            MPersonOnJob poj = new MPersonOnJob();
                            MProjectDepartment pdt = new MProjectDepartment();
                            MGWBSTree mmc = new MGWBSTree();
                            MPerson mp = new MPerson();
                            MOperationOrg org = new MOperationOrg();
                            VOBSMng vmroq = new VOBSMng(mmc, mp, mpd, poj, pdt,org);
                            vmroq.ViewCaption = "管理OBS";
                            framework.AddMainView(vmroq);
                        }
                        return null;
                    case CDOBS_ExecType.OBSService:
                        IMainView mroqMv1 = framework.GetMainView("服务OBS");
                        if (mroqMv1 != null)
                        {
                            mroqMv1.ViewShow();
                            return null;
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            VOBSService vmroq = new VOBSService(mmc);
                            vmroq.ViewCaption = "服务OBS";
                            framework.AddMainView(vmroq);
                        }
                        return null;
                }
            }
            return null;
        }
    }
}
