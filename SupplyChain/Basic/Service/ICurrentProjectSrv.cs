using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Basic.Service
{


    /// <summary>
    /// 项目部基本信息
    /// </summary>
    public interface ICurrentProjectSrv : IBaseService
    {

        #region 项目部基本信息

        IList GetSubCompanys();
        OperationOrg SaveOperationOrg(OperationOrg obj);
        IList GetStandardOperationJob();
        CurrentProjectInfo SaveCurrentProjectInfo(CurrentProjectInfo obj);

        /// <summary>
        /// 查询项目业务信息
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetCurrentProjectInfo(ObjectQuery oq);

        CurrentProjectInfo GetProjectInfoByOwnOrg(string ownOrgId);

        IList GetAllValideProject();
        DataSet GetCurrentProjectInfo(string sWhere);
        void SaveUpdateCurrentProjectInfo(IList lstCurProjectSQL);
        CurrentProjectInfo GetProjectById(string id);
        StandardPerson GetStandardPerson(string id);
        string GetBelongOperationOrg(string operOrgSyscode);
        IList QueryCurrentProjectInfo(ObjectQuery oq);
        List<DataDomain> GetAllPersonJobsByOrg(string orgId, string pId);
        List<DataDomain> GetPersonJobRequest();
        List<DataDomain> GetAllJobsByPerson(string perId);
        StandardPerson GetStandardPersonByCode(string code);

        List<DataDomain> GetBranchSyscodesByPersonCode(string code);
        /// <summary>
        /// 根据项目ID获取项目所关联的组织
        /// </summary>
        /// <param name="sProjectId"></param>
        /// <returns></returns>
        OperationOrg GetOwnerOrgByProjectId(string sProjectId);
        #endregion

        #region 物资信息价

        MaterialInterfacePrice GetMaterialPriceById(string id);
        IList GetMaterialPrice(ObjectQuery objectQuery);
        MaterialInterfacePrice SaveMaterialPrice(MaterialInterfacePrice obj);

        #endregion

        #region 项目降低率

        ProgramReduceRate GetProgramRateById(string id);
        IList GetProgramRate(ObjectQuery objectQuery);
        ProgramReduceRate SaveProgramRate(ProgramReduceRate obj);
        bool SaveProgramRate(IList lstProgramReduceRate);

        #endregion

        #region 常用短语查询

        IList GetOftenWords(ObjectQuery objectQuery);
        OftenWord SaveOftenWords(OftenWord obj);
        OftenWord GetOftenWordByOftenWord(string userID, string interphaseID, string controlID, string oftenWord);

        #endregion

        IList QueryAllProjectStateInfo(string opgID, DateTime startDate, DateTime endDate);
        IList GetPersonListByProjectAndRole(string projectId, string roleId);
        Hashtable QueryProjectStateInfo(string projectID, DateTime startDate, DateTime endDate);
        Hashtable QueryProjectDataInfo(string projectID, int kjn, int kjy);
        DataDomain GetProjectInfoByOpgId(string opgId);

        DataSet LoadPerson(string condition);
        bool SaveMaterialInterfacePrice(IList lstMaterialInterfacePrice);
        bool GetProgramRate(string sProjectID, string sMatCatID, string sSupplyID);

        IList GetAllProjectLedger();
        ProjectLedger GetProjectLedgerById(string id);
        ProjectLedger SaveProjectLedger(ProjectLedger obj);
        bool DeleteProjectLedger(ProjectLedger obj);
        List<string> GetColumnValues(string col);

        CurrentProjectInfo AffirmProject(OperationOrg newOrg, CurrentProjectInfo selectProject, IList opgJobs);
    }
}
