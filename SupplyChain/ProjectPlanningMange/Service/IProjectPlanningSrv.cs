using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.ProjectPlanningMange.Service
{
    /// <summary>
    /// 项目专业策划
    /// </summary>
    public interface IProjectPlanningSrv : IBaseService
    {
        /// <summary>
        /// 通过ID查询项目专业策划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SpecialityProposal GetSpecialityProposalById(string id);
        /// <summary>
        /// 项目专业策划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetSpecialityProposal(ObjectQuery objectQuery);
        /// <summary>
        /// 保存项目专业策划信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        SpecialityProposal SaveSpecialityProposal(SpecialityProposal obj);
         /// <summary>
        /// 项目专业策划信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet SpecialityProposalQuery(string condition);
        /// <summary>
        /// 通过ID查询项目专业商务策划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BusinessProposal GetBusinessProposalById(string id);
        /// <summary>
        /// 商务策划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
   
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        bool Delete(IList list);
        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item);
        IList GetBusinessProposal(ObjectQuery objectQuery);
        BusinessProposal SaveBusinessProposal(BusinessProposal obj);
        DataSet BusinessProposalQuery(string condition);
        BusinessProposalItem GetBusinessProposalItemById(string id);
        IList GetBusinessProposalItem(ObjectQuery objectQuery);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
      
    }
}
