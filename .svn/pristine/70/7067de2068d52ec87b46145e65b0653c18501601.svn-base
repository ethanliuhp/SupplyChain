using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.core;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.ResourceManager.Client.Main;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using Application.Business.Erp.SupplyChain.Client.StockMng.Report;

namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{
    public enum CProjectDepartment_ExecType
    {
        /// <summary>
        /// 项目部基本信息
        /// </summary>
        ProjectDepartment,
        ProjectDepartmentSearch,
        /// <summary>
        /// 项目商务状态维护
        /// </summary>
        ProjectBusinessStateMng,
        /// <summary>
        /// 项目工程状态维护
        /// </summary>
        ProjectStateMng,
        /// <summary>
        /// 项目物资状态维护
        /// </summary>
        ProjectMaterialStateMng,
        /// <summary>
        /// 项目业务信息
        /// </summary>
        ProjectBusiInfo,
        ProjectStateReport,
        ProjectDataInfo,
        OrgAsProjectInfo,
        AllProjectState,
        
    }
    
    public class CProjectDepartment
    {
        private static IFramework framework = null;
        string mainViewName = "项目基本信息维护";
        
        public CProjectDepartment(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }
        public object Excute(params object[] args)
        {
            object obj = args[0];
            if (obj != null && obj.GetType() == typeof(CProjectDepartment_ExecType))
            {
                CProjectDepartment_ExecType execType = (CProjectDepartment_ExecType)obj;
                switch (execType)
                {
                    case CProjectDepartment_ExecType.ProjectDepartment:
                        {
                            IMainView mroqMv = framework.GetMainView("项目基本信息维护");
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
                                string strName = "项目基本信息维护";
                                VProDepart vmroq = new VProDepart(mpd, poj, pdt, strName);
                                vmroq.ViewCaption = "项目基本信息维护";
                                framework.AddMainView(vmroq);
                            }
                            return null;
                        }
                    case CProjectDepartment_ExecType.ProjectDepartmentSearch:
                        {
                            IMainView mroqMv11 = framework.GetMainView("项目基本信息查询");
                            if (mroqMv11 != null)
                            {
                                mroqMv11.ViewShow();
                                return null;
                            }
                            else
                            {
                                MOperationJob mpd1 = new MOperationJob();
                                MPersonOnJob poj1 = new MPersonOnJob();
                                MProjectDepartment pdt1 = new MProjectDepartment();
                                string strName1 = "项目基本信息查询";
                                VProDepart vmroq11 = new VProDepart(mpd1, poj1, pdt1, strName1);
                                vmroq11.ViewCaption = "项目基本信息查询";
                                framework.AddMainView(vmroq11);
                            }
                            return null;
                        }
                    case CProjectDepartment_ExecType.ProjectBusiInfo:
                        {
                            IMainView mv3 = framework.GetMainView("项目业务信息");
                            if (mv3 != null)
                            {
                                mv3.ViewShow();
                                return null;
                            }
                            VProjectBusinessInfo vpbi = new VProjectBusinessInfo();
                            vpbi.ViewCaption = "项目业务信息";
                            framework.AddMainView(vpbi);
                            vpbi.ViewShow();
                            return null;
                        }
                    case CProjectDepartment_ExecType.ProjectStateReport:
                        {
                            IMainView mv4 = framework.GetMainView("项目使用状态报告");
                            if (mv4 != null)
                            {
                                mv4.ViewShow();
                                return null;
                            }
                            VProjectStateReport vpsp = new VProjectStateReport();
                            vpsp.ViewCaption = "项目使用状态报告";
                            framework.AddMainView(vpsp);
                            vpsp.ViewShow();
                            return null;
                        }
                    case CProjectDepartment_ExecType.ProjectDataInfo:
                        {
                            IMainView mv5 = framework.GetMainView("项目综合数据统计");
                            if (mv5 != null)
                            {
                                mv5.ViewShow();
                                return null;
                            }
                            VProjectDataInfo vpdi = new VProjectDataInfo();
                            vpdi.ViewCaption = "项目综合数据统计";
                            framework.AddMainView(vpdi);
                            vpdi.ViewShow();
                            return null;
                        }
                    case CProjectDepartment_ExecType.OrgAsProjectInfo:
                        {
                            IMainView mv6 = framework.GetMainView("工程项目维护");
                            if (mv6 != null)
                            {
                                mv6.ViewShow();
                                return null;
                            }
                            VOperationOrgAsProject vooap = new VOperationOrgAsProject();
                            vooap.ViewCaption = "工程项目维护";
                            framework.AddMainView(vooap);
                            vooap.ViewShow();
                            return null;
                        }
                    case CProjectDepartment_ExecType.AllProjectState:
                        {
                            IMainView mv7 = framework.GetMainView("公司项目使用状况统计");
                            if (mv7 != null)
                            {
                                mv7.ViewShow();
                                return null;
                            }
                            VAllProjectStateReport vapsr = new VAllProjectStateReport();
                            vapsr.ViewCaption = "公司项目使用状况统计";
                            framework.AddMainView(vapsr);
                            vapsr.ViewShow();
                            return null;
                        }
                    case CProjectDepartment_ExecType.ProjectBusinessStateMng:
                        {
                            string sName = "项目商务状态维护";
                            IMainView mroqMv = framework.GetMainView(sName);
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
                                VProjectBusinessStateMng vmroq = new VProjectBusinessStateMng(execType);
                                vmroq.ViewCaption = sName;
                                framework.AddMainView(vmroq);
                            }
                            return null;
                        }
                    case CProjectDepartment_ExecType.ProjectMaterialStateMng:
                        {
                            string sName = "项目物资排名维护";
                            IMainView mroqMv = framework.GetMainView(sName);
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            else
                            {
                                VProjectMaterialStateMng vmroq = new VProjectMaterialStateMng(execType);
                                vmroq.ViewCaption = sName;
                                framework.AddMainView(vmroq);
                            }
                            return null;
                        }
                    case CProjectDepartment_ExecType.ProjectStateMng:
                        {
                            string sName = "项目工程状态维护";
                            IMainView mroqMv = framework.GetMainView(sName);
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
                                VProjectBusinessStateMng vmroq = new VProjectBusinessStateMng(execType);
                                vmroq.ViewCaption = sName;
                                framework.AddMainView(vmroq);
                            }
                            return null;
                        }
                }
            }
            return null;
        }
    }
}
