using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using VirtualMachine.SystemAspect.Security;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Resource.FinancialResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.CommonClass.Domain;
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Service
{
    /// <summary>
    /// 工程任务类型服务
    /// </summary>
    public class CostItemSrv : ICostItemSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        public string GetCostItemCode()
        {
            return "QY-" + string.Format("{0:yyyyMMdd}", DateTime.Now);
        }

        /// <summary>
        /// 获取成本项明细编号
        /// </summary>
        /// <returns></returns>
        public string GetCostItemDetailCode(string CostItemCode, int detailNum)
        {
            return CostItemCode + detailNum.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 保存或修改成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateCostItem(IList list)
        {

            dao.SaveOrUpdate(list);

            return list;
        }
        /// <summary>
        /// 保存成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        [TransManager]
        public IList Save(IList list)
        {
            dao.Save(list);
            return list;
        }
        /// <summary>
        /// 保存或修改成本项
        /// </summary>
        /// <param name="item">成本项</param>
        /// <returns></returns>
        [TransManager]
        public CostItem SaveOrUpdateCostItem(CostItem item)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                item.UpdateTime = DateTime.Now;
                item.ItemState = CostItemState.制定;
            }


            dao.SaveOrUpdate(item);
            return item;
        }

        /// <summary>
        /// 保存或修改资源组
        /// </summary>
        /// <param name="group">资源组</param>
        /// <returns></returns>
        [TransManager]
        public ResourceGroup SaveOrUpdateGroup(ResourceGroup group)
        {
            //if (string.IsNullOrEmpty(group.Id))
            dao.SaveOrUpdate(group);
            return group;
        }

        /// <summary>
        /// 删除成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCostItem(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (CostItem cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                }
            }
            oq.AddCriterion(dis);
            IList listCostitem = dao.ObjectQuery(typeof(CostItem), oq);
            return dao.Delete(listCostitem);//级联删除定额和资源组
        }

        /// <summary>
        /// 删除成本定额集合
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCostItemQuota(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (SubjectCostQuota dtl in list)
            {
                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dis.Add(Expression.Eq("Id", dtl.Id));
                }
            }
            oq.AddCriterion(dis);
            IList listQuota = dao.ObjectQuery(typeof(SubjectCostQuota), oq);
            return dao.Delete(listQuota);//级联删除资源组

        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return dao.Get(entityType, Id);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 保存或修改成本定额
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateCostItemQuota(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 保存或修改成本定额
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        public SubjectCostQuota SaveOrUpdateSubjectCostQuota(SubjectCostQuota obj)
        {
            if (obj.Id == null)
            {
                //obj.Code = GetCode(typeof(SubjectCostQuota), obj.TheProjectGUID);
            }
            dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 根据成本项分类Id获取最大成本项号
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        public int GetMaxCostItemCodeByCate(string cateId)
        {
            int code = 0;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheCostItemCategory.Id", cateId));
            IList listCostItem = dao.ObjectQuery(typeof(CostItem), oq);

            if (listCostItem.Count > 0)
            {
                //获取父对象下最大明细号
                foreach (CostItem item in listCostItem)
                {
                    if (item != null && !string.IsNullOrEmpty(item.Code) && item.Code.IndexOf("-") > -1)
                    {
                        try
                        {
                            int tempCode = Convert.ToInt32(item.Code.Substring(item.Code.LastIndexOf("-") + 1));
                            if (tempCode >= code)
                                code += 1;
                        }
                        catch
                        {

                        }
                    }
                }
            }
            return code;
        }

        #region 劳动力定额
        /// <summary>
        /// 保存或修改劳动力定额
        /// </summary>
        /// <returns></returns>
        public CostWorkForce SaveOrUpdateCostWorkForce(CostWorkForce obj)
        {
            dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 保存或修改成本定额
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateCostWorkForce(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }
        /// <summary>
        /// 删除劳动力定额集合
        /// </summary>
        /// <param name="list">劳动力定额集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCostWorkForce(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (CostWorkForce dtl in list)
            {
                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dis.Add(Expression.Eq("Id", dtl.Id));
                }
            }
            oq.AddCriterion(dis);
            IList listQuota = dao.ObjectQuery(typeof(CostWorkForce), oq);
            return dao.Delete(listQuota);//级联删除资源组

        }
        #endregion
    }
}
