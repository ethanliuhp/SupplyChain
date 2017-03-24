using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using VirtualMachine.Core;
using NHibernate.Criterion;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS
{
    public class MQWBSManagement
    {
        private static IQWBSSrv model;
        private static IProObjectRelaDocumentSrv modelDoc;

        public MQWBSManagement()
        {
            if (model == null)
                model = ConstMethod.GetService("QWBSSrv") as IQWBSSrv;
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
        }

        //返回节点node所在树的当前层次的最大OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return model.GetMaxOrderNo(node);
        }

        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetGWBSTreesByInstance(string projectId)
        {
            return model.GetGWBSTreesByInstance(projectId);
        }

        //保存组织集合
        public IList SaveQWBSManages(IList lst)
        {
            return model.SaveQWBSManages(lst);
        }

        public IDictionary MoveNode(QWBSManage fromNode, QWBSManage toNode)
        {
            QWBSManage cat = model.MoveQWBSManage(fromNode, toNode);
            IDictionary dic = new Hashtable();
            dic.Add(cat.ParentNode.Id.ToString(), cat.ParentNode);
            dic.Add(cat.Id.ToString(), cat);
            IList lstChildNodes = model.GetALLChildNodes(cat);
            foreach (QWBSManage var in lstChildNodes)
            {
                dic.Add(var.Id.ToString(), var);
            }
            return dic;
        }
        /// <summary>
        /// 根据名称查找业务组织
        /// </summary>
        public QWBSManage GetOpeOrgByName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
            IList list = model.GetQWBSManages(typeof(QWBSManage), oq);
            if (list.Count == 1)
                return list[0] as QWBSManage;
            else
                return null;
        }

        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetQWBSManageByInstance(string projectId)
        {
            return model.GetQWBSManageByInstance(projectId);
        }

        /// <summary>
        /// 获取工程任务类型树节点集合
        /// </summary>
        public IList GetQWBSManageByInstance(string projectId, IList listParentTaskType)
        {
            return model.GetQWBSManageByInstance(projectId);
        }

        public IList GetQWBSManage(Type t)
        {
            return model.GetQWBSManage(t);
        }
        public IList GetQWBSManageAndJobs(Type t)
        {
            return model.GetQWBSManageAndJobs(t);
        }


        public QWBSManage GetQWBSManageById(string id)
        {
            //ProjectTaskTypeTree org = mm.GetProjectTaskTypeTreeById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = model.GetQWBSManages(typeof(QWBSManage), oq);
            if (list.Count == 1)
                return list[0] as QWBSManage;
            else
                return null;
        }


        public QWBSManage SaveQWBSManage(QWBSManage childOrg)
        {
            return model.SaveQWBSManageTree(childOrg);
        }

        public bool DeleteQWBSManage(QWBSManage childOrg)
        {
            return model.DeleteQWBSManage(childOrg);
        }

        public bool DeleteQWBSManage(IList lst)
        {
            return model.DeleteQWBSManage(lst);
        }

        public bool InitRule()
        {
            return model.InitRule();
        }

        public IList GetQWBSManageRules(Type treeType)
        {
            return model.GetQWBSManageRules(treeType);
        }

        public IList GetQWBSManageeRules(Type treeType, Type parentNodeType)
        {
            return model.GetQWBSManageRules(treeType, parentNodeType);
        }

        public string GetNextOPGCode()
        {
            return model.GetNextOPGCode();
        }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        public IList SaveQWBSManageRootNode(IList list)
        {
            return model.SaveQWBSRootNode(list);
        }

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return model.GetCode(type);
        }

        /// <summary>
        /// 插入（部分属性固定）或修改任务类型树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public QWBSManage InsertOrUpdateQWBSManagee(QWBSManage childNode)
        {
            return model.InsertOrUpdateQWBSManage(childNode);
        }

        /// <summary> 
        /// 插入（部分属性固定）或修改PBS节点集合
        /// </summary>
        [TransManager]
        public IList InsertOrUpdateQWBSManages(IList lst)
        {
            return model.InsertOrUpdateQWBSManages(lst);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }

        
    }
}
