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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public class MWBSManagement
    {
        private static IProjectTaskTypeSrv model;
        private static IProObjectRelaDocumentSrv modelDoc;

        public MWBSManagement()
        {
            if (model == null)
                model = ConstMethod.GetService("ProjectTaskTypeSrv") as IProjectTaskTypeSrv;
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
        }

        public System.Data.DataSet SearchSQL(string sql)
        {
            return model.SearchSQL(sql);
        }
        //���ؽڵ�node�������ĵ�ǰ��ε����OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return model.GetMaxOrderNo(node);
        }

        public ProjectTaskTypeTree GetTaskTypeByCode(string code)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            var list = model.GetProjectTaskTypeTrees(typeof(ProjectTaskTypeTree), oq);

            if (list != null && list.Count > 0)
            {
                return list[0] as ProjectTaskTypeTree;
            }

            return null;
        }

        //������֯����
        public IList SaveProjectTaskTypeTrees(IList lst)
        {
            return model.SaveProjectTaskTypeTrees(lst);
        }

        public IDictionary MoveNode(ProjectTaskTypeTree fromNode, ProjectTaskTypeTree toNode)
        {
            ProjectTaskTypeTree cat = model.MoveProjectTaskTypeTree(fromNode, toNode);
            IDictionary dic = new Hashtable();
            dic.Add(cat.ParentNode.Id.ToString(), cat.ParentNode);
            dic.Add(cat.Id.ToString(), cat);
            IList lstChildNodes = model.GetALLChildNodes(cat);
            foreach (ProjectTaskTypeTree var in lstChildNodes)
            {
                dic.Add(var.Id.ToString(), var);
            }
            return dic;
        }
        /// <summary>
        /// �������Ʋ���ҵ����֯
        /// </summary>
        public ProjectTaskTypeTree GetOpeOrgByName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
            IList list = model.GetProjectTaskTypeTrees(typeof(ProjectTaskTypeTree), oq);
            if (list.Count == 1)
                return list[0] as ProjectTaskTypeTree;
            else
                return null;
        }

        /// <summary>
        /// ��ȡҵ����֯�ڵ㼯��(������Ȩ�޺���Ȩ�޵ļ���)
        /// </summary>
        public IList GetProjectTaskTypeByInstance()
        {
            return model.GetProjectTaskTypeByInstance();
        }

        /// <summary>
        /// ��ȡ���������������ڵ㼯��
        /// </summary>
        public IList GetProjectTaskTypeByInstance(IList listParentTaskType)
        {
            return model.GetProjectTaskTypeByInstance(listParentTaskType);
        }

        public IList GetProjectTaskTypeTrees(Type t)
        {
            return model.GetProjectTaskTypeTrees(t);
        }
        public IList GetProjectTaskTypeTreeAndJobs(Type t)
        {
            return model.GetProjectTaskTypeTreeAndJobs(t);
        }


        public ProjectTaskTypeTree GetProjectTaskTypeTreeById(string id)
        {
            //ProjectTaskTypeTree org = mm.GetProjectTaskTypeTreeById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = model.GetProjectTaskTypeTrees(typeof(ProjectTaskTypeTree), oq);
            if (list.Count == 1)
                return list[0] as ProjectTaskTypeTree;
            else
                return null;
        }


        public ProjectTaskTypeTree SaveProjectTaskTypeTree(ProjectTaskTypeTree childOrg)
        {
            return model.SaveProjectTaskTypeTree(childOrg);
        }

        public bool DeleteProjectTaskTypeTree(ProjectTaskTypeTree childOrg)
        {
            return model.DeleteProjectTaskTypeTree(childOrg);
        }

        public bool DeleteProjectTaskTypeTree(IList lst)
        {
            return model.DeleteProjectTaskTypeTree(lst);
        }

        public bool InitRule()
        {
            return model.InitRule();
        }

        public IList GetProjectTaskTypeTreeRules(Type treeType)
        {
            return model.GetProjectTaskTypeTreeRules(treeType);
        }

        public IList GetProjectTaskTypeTreeRules(Type treeType, Type parentNodeType)
        {
            return model.GetProjectTaskTypeTreeRules(treeType, parentNodeType);
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
        public IList SaveProjectTaskTypeRootNode(IList list)
        {
            return model.SaveProjectTaskTypeRootNode(list);
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
        public ProjectTaskTypeTree InsertOrUpdateTaskTypeTree(ProjectTaskTypeTree childNode)
        {
            return model.InsertOrUpdateTaskTypeTree(childNode);
        }

        /// <summary> 
        /// ���루�������Թ̶������޸�PBS�ڵ㼯��
        /// </summary>
        [TransManager]
        public IList InsertOrUpdateTaskTypeTrees(IList lst)
        {
            return model.InsertOrUpdateTaskTypeTrees(lst);
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
        public IList PublisthTaskNodeAndChilds(string sNodeSysCode)
        {
            return model.PublisthTaskNodeAndChilds(sNodeSysCode);
        }
        public IList InvalidTaskNodeAndChilds(string sNodeSysCode)
        {
            return model.InvalidTaskNodeAndChilds(sNodeSysCode);
        }
        #region   �������������ĵ�ģ��********************************************************************************************
        /// <summary>
        /// ������޸Ĺ������������ĵ�ģ��
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public ProTaskTypeDocumentStencil SaveOrUpDateDocStencil(ProTaskTypeDocumentStencil obj)
        {
            return model.SaveOrUpDateDocStencil(obj);
        }
        /// <summary>
        /// �������湤�����������ĵ�ģ��
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        [TransManager]
        public IList saveDaoStencilList(IList objs)
        {
            return model.SaveDaoStencilList(objs);
        }
        /// <summary>
        /// ɾ���������������ĵ�ģ��
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteDocStencilList(IList objs)
        {
            return model.DeleteDocStencilList(objs);
        }
        #endregion

        #region �ĵ�����

        /// <summary>
        /// ������޸Ķ��󼯺�
        /// </summary>
        /// <param name="list">���󼯺�</param>
        /// <returns></returns>
        public IList SaveOrUpdate(IList list)
        {
            return modelDoc.SaveOrUpdate(list);
        }

        /// <summary>
        /// ������޸Ĺ��̶�������ĵ�
        /// </summary>
        /// <param name="item">���̶�������ĵ�</param>
        /// <returns></returns>
        public ProObjectRelaDocument SaveOrUpdateProObjRelaDoc(ProObjectRelaDocument item)
        {
            return modelDoc.SaveOrUpdate(item);
        }

        /// <summary>
        /// ɾ�����̶�������ĵ����󼯺�
        /// </summary>
        /// <param name="list">���̶�������ĵ����󼯺�</param>
        /// <returns></returns>
        public bool DeleteProObjRelaDoc(IList list)
        {
            return modelDoc.DeleteProObjRelaDoc(list);
        }

        #endregion �ĵ�����

        public bool UpdateProjectTaskTypeTreeOrderNO(List<ProjectTaskTypeTree> ilst)
        {
            return model.UpdateProjectTaskTypeTreeOrderNO(ilst);
        }
    }
}