using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using VirtualMachine.Core;
using IRPServiceModel.Domain.CompanyMng;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using NHibernate.Criterion;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace IRPServiceModel.Services.CompanyMng
{
    public class CompanyInfoSvr : ICompanyInfoSvr
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }
        private ICategoryNodeService nodeSrv;
        public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }
        private ICategoryRuleService ruleSrv;
        public ICategoryRuleService RuleSrv
        {
            get { return ruleSrv; }
            set { ruleSrv = value; }
        }
        private ICategoryTreeService treeService;
        public ICategoryTreeService TreeService
        {
            get { return treeService; }
            set { treeService = value; }
        }
        public IList GetTree()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
           
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(CompanyInfo), oq);
            return list;
        }
        public CompanyInfo SavePlaceTreeRootNode()
        {
            ObjectQuery oqTree = new ObjectQuery();
            oqTree.AddCriterion(Expression.Eq("Name", "公司管理"));
            IList listTree = dao.ObjectQuery(typeof(CategoryTree), oqTree);

            CategoryTree tree = null;
            if (listTree.Count > 0)
                tree = listTree[0] as CategoryTree;

            CompanyInfo oCompanyInfo = new CompanyInfo();
            oCompanyInfo.Name = "公司信息";
            oCompanyInfo.ParentNode = null;
            oCompanyInfo.CategoryNodeType = VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode;
            oCompanyInfo.CreateDate = DateTime.Now;
            oCompanyInfo.Level = 1;
            oCompanyInfo.OrderNo = 1;
            oCompanyInfo.State = 1;
            oCompanyInfo.TheTree = tree;
            oCompanyInfo.Author = Dao.Get(typeof(BusinessOperators), "428") as IBusinessOperators;
            dao.Save(oCompanyInfo);
            oCompanyInfo.SysCode = oCompanyInfo.Id + ".";
            dao.Update(oCompanyInfo);
            return oCompanyInfo;
        }
        public IList Query(Type type, ObjectQuery oQuery)
        {
            return dao.ObjectQuery(type,oQuery);
        }
        public CompanyInfo GetCompanyInfoById(string strID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id",strID));
            oQuery.AddFetchMode("PersonMng", NHibernate.FetchMode.Eager);
            IList lst = dao.ObjectQuery(typeof(CompanyInfo),oQuery);
            return lst == null || lst .Count==0? null : lst[0] as CompanyInfo;
        }
        public CompanyInfo SaveOrUpdate(CompanyInfo oNode)
        {
            if (oNode != null)
            {
                if (oNode.Id == null)
                {
                  
                    ///现在将操作这设置为管理员 admin帐号
                    oNode.Author = Dao.Get(typeof(BusinessOperators), "428") as IBusinessOperators;
                    nodeSrv.AddChildNode(oNode);
                }
                else
                {
                    nodeSrv.UpdateCategoryNode(oNode);
                }
            }
            return oNode;
        }
        [TransManager]
        public bool DeleteCompanyInfoByID(string sID)
        {
            bool bFlag = false;
            CompanyInfo oCompanyInfo = dao.Get(typeof(CompanyInfo), sID) as CompanyInfo;
            if (oCompanyInfo != null)
            {

                ObjectQuery oQuery = new ObjectQuery();
                oQuery.AddCriterion(Expression.Like("SysCode", oCompanyInfo.SysCode, MatchMode.Start));
                IList lst = dao.ObjectQuery(typeof(CompanyInfo), oQuery);
                if (lst != null && lst.Count > 0)
                {
                    dao.Delete(lst);
                    bFlag = true;
                }
            }
            return bFlag;

        }
        public object QueryById(Type type, string strID)
        {
            return dao.Get(type, strID);
        }
    }
}
