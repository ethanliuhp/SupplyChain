using System;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    public interface ICostProjectSrv
    {
        bool Delete(System.Collections.IList lst);
        bool Delete(object obj);
        bool DeleteCostProject(CostProject title);
        CostProject GetCostProject(string id);
        System.Collections.IList GetCostProjectByQuery(VirtualMachine.Core.ObjectQuery query);
        System.Collections.IList GetCostProjects(VirtualMachine.Core.ObjectQuery oq);
        System.Collections.IList GetCostProjectsWithFathers(VirtualMachine.Core.ObjectQuery accQuy);
        System.Collections.IList GetObjects(Type aType, VirtualMachine.Core.ObjectQuery oq);
        System.Collections.IList GetObjects(Type aType);
        bool IsReferByCostAccount(string CostProjectId);
        System.Data.DataSet ProductCostClassteamSearch(string condition);
        object Save(object obj);
        System.Collections.IList Save(System.Collections.IList lst);
        CostProject SaveCostProject(CostProject title);
        System.Collections.IList Update(System.Collections.IList lst);
        object Update(object obj);
        CostProject UpdateCostProject(CostProject accTitle);
    }
}
