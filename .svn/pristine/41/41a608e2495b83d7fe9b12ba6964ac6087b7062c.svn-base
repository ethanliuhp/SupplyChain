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
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng
{
    public class MCostAccountSubject
    {
        private ICostAccountSubjectSrv mm;
        public ICostAccountSubjectSrv Mm
        {
            get { return mm; }
            set { mm = value; }
        }

        public MCostAccountSubject()
        {
            if (mm == null)
                mm = ConstMethod.GetService("CostAccountSubjectSrv") as ICostAccountSubjectSrv;
        }

        //返回节点node所在树的当前层次的最大OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return mm.GetMaxOrderNo(node, null);
        }

        //保存组织集合
        public IList SaveCostAccountSubjects(IList lst)
        {
            return mm.SaveCostAccountSubjects(lst);
        }

        public IDictionary MoveNode(CostAccountSubject fromNode, CostAccountSubject toNode)
        {
            CostAccountSubject cat = mm.MoveCostAccountSubject(fromNode, toNode);
            IDictionary dic = new Hashtable();
            dic.Add(cat.ParentNode.Id.ToString(), cat.ParentNode);
            dic.Add(cat.Id.ToString(), cat);
            IList lstChildNodes = mm.GetALLChildNodes(cat);
            foreach (CostAccountSubject var in lstChildNodes)
            {
                dic.Add(var.Id.ToString(), var);
            }
            return dic;
        }
        /// <summary>
        /// 根据名称查找成本项分类
        /// </summary>
        public CostAccountSubject GetOpeOrgByName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
            IList list = mm.GetCostAccountSubjects(typeof(CostAccountSubject), oq);
            if (list.Count == 1)
                return list[0] as CostAccountSubject;
            else
                return null;
        }

        /// <summary>
        /// 获取成本项分类节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetCostAccountSubjectByInstance()
        {
            return mm.GetCostAccountSubjectByInstance();
        }

        public IList GetCostAccountSubjects(Type t)
        {
            return mm.GetCostAccountSubjects(t);
        }
        public IList GetCostAccountSubjectAndJobs(Type t)
        {
            return mm.GetCostAccountSubjectAndJobs(t);
        }


        public CostAccountSubject GetCostAccountSubjectById(string id)
        {
            //CostAccountSubject org = mm.GetCostAccountSubjectById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = mm.GetCostAccountSubjects(typeof(CostAccountSubject), oq);
            if (list.Count == 1)
                return list[0] as CostAccountSubject;
            else
                return null;
        }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        public IList SaveCostAccountSubjectRootNode(IList lst)
        {
            return mm.SaveCostAccountSubjectRootNode(lst);
        }

        public CostAccountSubject SaveCostAccountSubject(CostAccountSubject childOrg)
        {
            return mm.SaveCostAccountSubject(childOrg);
        }

        public bool DeleteCostAccountSubject(CostAccountSubject childOrg)
        {
            return mm.DeleteCostAccountSubject(childOrg);
        }

        public bool DeleteCostAccountSubject(IList lst)
        {
            return mm.DeleteCostAccountSubject(lst);
        }

        public bool InitRule()
        {
            return mm.InitRule();
        }


        public IList GetCostAccountSubjectRules(Type treeType)
        {
            return mm.GetCostAccountSubjectRules(treeType);
        }


        public IList GetCostAccountSubjectRules(Type treeType, Type parentNodeType)
        {
            return mm.GetCostAccountSubjectRules(treeType, parentNodeType);
        }


        public string GetNextOPGCode()
        {
            return mm.GetNextOPGCode();
        }

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return mm.GetCode(type);
        }
    }
}
