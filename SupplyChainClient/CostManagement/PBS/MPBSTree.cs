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
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public class MPBSTree
    {
        private static IPBSTreeSrv model;
        private static IProObjectRelaDocumentSrv modelDoc;
        #region ����������ԣ���õ��ö���һ��̫�鷳

        public IPBSTreeSrv Service { get; set; }
        #endregion
        public MPBSTree()
        {
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            Service = model;
        }

        //���ؽڵ�node�������ĵ�ǰ��ε����OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return model.GetMaxOrderNo(node);
        }

        /// <summary>
        /// ����ж�����󣬵�һ������Ϊ���ڵ㣬����Ϊ�ӽڵ�
        /// </summary>
        /// <param name="lst">���ڵ㼯��</param>
        /// <returns></returns>
        public IList SavePBSTreeRootNode(IList list)
        {
            return model.SavePBSTreeRootNode(list);
        }

        /// <summary>
        /// ����PBS���ڵ㼯��
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public IList SavePBSTrees(IList lst)
        {
            return model.SavePBSTrees(lst);
        }
        public IList PBSTreeOrder(IList list)
        {
            return model.PBSTreeOrder(list);
        }
        /// <summary>
        /// ���루�������Թ̶������޸�PBS���ڵ�
        /// </summary>
        /// <param name="childNode">���ڵ�</param>
        /// <returns></returns>
        public PBSTree InsertOrUpdatePBSTree(PBSTree childNode)
        {
            return model.InsertOrUpdatePBSTree(childNode);
        }

        /// <summary> 
        /// ���루�������Թ̶������޸�PBS�ڵ㼯��
        /// </summary>
        public IList InsertOrUpdatePBSTrees(IList lst)
        {
            return model.InsertOrUpdatePBSTrees(lst);
        }

        public Hashtable MoveNode(PBSTree fromNode, PBSTree toNode)
        {
            return model.MovePBSTree(fromNode, toNode);
        }

        /// <summary>
        /// �������Ʋ���ҵ����֯
        /// </summary>
        public PBSTree GetOpeOrgByName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
            IList list = model.GetPBSTrees(typeof(PBSTree), oq);
            if (list.Count == 1)
                return list[0] as PBSTree;
            else
                return null;
        }

        /// <summary>
        /// ��ȡҵ����֯�ڵ㼯��(������Ȩ�޺���Ȩ�޵ļ���)
        /// </summary>
        public IList GetPBSTreesByInstance(string projectId)
        {
            return model.GetPBSTreesByInstance(projectId);
        }

        /// <summary>
        /// ��ȡPBS�ڵ㼯��(������Ȩ�޺���Ȩ�޵ļ���)
        /// </summary>
        public DataTable GetPBSTreesInstanceBySql(string projectId)
        {
            return model.GetPBSTreesInstanceBySql(projectId);
        }

        /// <summary>
        /// ��ȡPBS�ڵ㼯��
        /// </summary>
        public IList GetPBSTreesByInstance(string projectId, IList listParentPBS)
        {
            return model.GetPBSTreesByInstance(projectId, listParentPBS);
        }

        public IList GetPBSTrees(Type t)
        {
            return model.GetPBSTrees(t);
        }
        public IList GetPBSTreeAndJobs(Type t)
        {
            return model.GetPBSTreeAndJobs(t);
        }


        public PBSTree GetPBSTreeById(string id)
        {
            //PBSTree org = mm.GetPBSTreeById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = model.GetPBSTrees(typeof(PBSTree), oq);
            if (list.Count == 1)
                return list[0] as PBSTree;
            else
                return null;
        }


        public PBSTree SavePBSTree(PBSTree childOrg)
        {
            return model.SavePBSTree(childOrg);
        }

        public bool DeletePBSTree(PBSTree childOrg)
        {
            return model.DeletePBSTree(childOrg);
        }

        public bool DeletePBSTree(IList lst)
        {
            return model.DeletePBSTree(lst);
        }

        public bool InitRule()
        {
            return model.InitRule();
        }


        public IList GetPBSTreeRules(Type treeType)
        {
            return model.GetPBSTreeRules(treeType);
        }


        public IList GetPBSTreeRules(Type treeType, Type parentNodeType)
        {
            return model.GetPBSTreeRules(treeType, parentNodeType);
        }


        public string GetNextOPGCode()
        {
            return model.GetNextOPGCode();
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
        /// �����ѯ
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <param name="oq">��ѯ����</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }

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


        /// <summary>
        /// ���� �޸� ���Լ� 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList SaveOrUpdateFeatureSet(IList list)
        {
            return model.SaveOrUpdateFeatureSet(list);
        }

        public bool Delete(IList list)
        {
            return model.Delete(list);
        }
        public bool Delete(object obj)
        {
            return model.Delete(obj);
        }

        /// <summary>
        /// ɾ��Ԫ��
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool DeleteElement(Elements element)
        {
            return model.DeleteElement(element);
        }

        public void SaveBatchPBSTreeName(IList lstPBSTree)
        {
            model.SaveBatchPBSTreeName(lstPBSTree);
        }
        public DataSet GetPBSTreesByInstanceSql(string projectId, string sParentID, string sName)
        {
            return model.GetPBSTreesByInstanceSql(projectId, sParentID, sName);
        }

        #region ������ʩ�޸�

        public bool IsLeafNode(PBSTree pbs)
        {
            var children = model.GetALLChildNodes(pbs);
            return children.Count <= 0;
        }
        #endregion
    }
}