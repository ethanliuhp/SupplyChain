using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.QWBS.Service
{
    public interface IQWBSSrv
    {
        IList SaveQWBSRootNode(IList lst);
        QWBSManage SaveQWBSManageTree(QWBSManage childNode);
        IList SaveQWBSManages(IList lst);
        QWBSManage InsertOrUpdateQWBSManage(QWBSManage childNode);
        IList InsertOrUpdateQWBSManages(IList lst);
        bool DeleteQWBSManage(QWBSManage pbs);
        bool DeleteQWBSManage(IList list);
        QWBSManage MoveQWBSManage(QWBSManage pbs, QWBSManage toPbs);
        //void UpdateChildNodes(QWBSManage parentNode);
        IList InvalidateQWBSManage(QWBSManage pbs);
        CategoryTree InitTree(string treeName, Type treeType);
        CategoryTree InitTree(string treeName, Type treeType, StandardPerson aPerson);
        bool InitRule();
        IList GetQWBSManage(Type t);
        IList GetQWBSManageByInstance(string projectId);
        IList GetProjectTaskTypeByInstance(string projectId, IList listParentTaskType);
        long GetMaxOrderNo(CategoryNode node);
        QWBSManage GetQWBSManage(QWBSManage orgCat);
        IList GetQWBSManageAndJobs(Type t);
        IList GetQWBSManageAndJobs();
        //bool Contains(IList lstInstance, CategoryNode node);
        IList GetOpeOrgAndJobsByInstance();
        IList GetQWBSManages(Type t, ObjectQuery oq);
        IList GetALLChildNodes(QWBSManage pbs);
        QWBSManage GetQWBSManageById(string id);
        IList GetQWBSManageRules(Type treeType);
        IList GetQWBSManageRules(Type treeType, Type parentNodeType);
        string GetNextOPGCode();
        IList GetGWBSTreesByInstance(string projectId);
        string GetCode(Type type);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
    }
}
