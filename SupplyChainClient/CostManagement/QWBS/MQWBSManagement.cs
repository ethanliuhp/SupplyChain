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

        //���ؽڵ�node�������ĵ�ǰ��ε����OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return model.GetMaxOrderNo(node);
        }

        /// <summary>
        /// ��ȡҵ����֯�ڵ㼯��(������Ȩ�޺���Ȩ�޵ļ���)
        /// </summary>
        public IList GetGWBSTreesByInstance(string projectId)
        {
            return model.GetGWBSTreesByInstance(projectId);
        }

        //������֯����
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
        /// �������Ʋ���ҵ����֯
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
        /// ��ȡҵ����֯�ڵ㼯��(������Ȩ�޺���Ȩ�޵ļ���)
        /// </summary>
        public IList GetQWBSManageByInstance(string projectId)
        {
            return model.GetQWBSManageByInstance(projectId);
        }

        /// <summary>
        /// ��ȡ���������������ڵ㼯��
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
        /// ����ж�����󣬵�һ������Ϊ���ڵ㣬����Ϊ�ӽڵ�
        /// </summary>
        /// <param name="lst">���ڵ㼯��</param>
        /// <returns></returns>
        public IList SaveQWBSManageRootNode(IList list)
        {
            return model.SaveQWBSRootNode(list);
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return model.GetCode(type);
        }

        /// <summary>
        /// ���루�������Թ̶������޸������������ڵ�
        /// </summary>
        /// <param name="childNode">���ڵ�</param>
        /// <returns></returns>
        [TransManager]
        public QWBSManage InsertOrUpdateQWBSManagee(QWBSManage childNode)
        {
            return model.InsertOrUpdateQWBSManage(childNode);
        }

        /// <summary> 
        /// ���루�������Թ̶������޸�PBS�ڵ㼯��
        /// </summary>
        [TransManager]
        public IList InsertOrUpdateQWBSManages(IList lst)
        {
            return model.InsertOrUpdateQWBSManages(lst);
        }

        /// <summary>
        /// �����ѯ
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <param name="oq">��ѯ����</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }

        
    }
}
