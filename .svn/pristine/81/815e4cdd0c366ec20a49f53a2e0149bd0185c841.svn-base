using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Service
{
   

    /// <summary>
    /// OBS服务
    /// </summary>
    public class OBSSrv : BaseService, IOBSSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }
        
        private ICategoryNodeService nodeSrv;
        public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }
        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        #endregion

        #region OBSService服务
        /// <summary>
        /// 查询业务组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetOperationOrgs(ObjectQuery oq)
        {
            return nodeSrv.GetNodesByObjectQuery(typeof(OperationOrg), oq);
        }

        /// <summary>
        /// 通过ProjectTaskName查询服务OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetOBSServiceByName(string Name)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectTaskName", Name));
            return Dao.ObjectQuery(typeof(OBSService), objectQuery);
        }
        /// <summary>
        /// 通过ID查询服务OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OBSService GetOBSServiceById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetOBSService(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OBSService;
            }
            return null;
        }

        /// <summary>
        /// OBS服务信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOBSService(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OBSService), objectQuery);
        }


        [TransManager]
        public OBSService SaveOBSService(OBSService obj)
        {
            return SaveOrUpdateByDao(obj) as OBSService;
        }


        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return Dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteService(IList list)
        {
            bool flag = false;
            if (list.Count > 0)
            {
                foreach (OBSService service in list)
                {
                    if (DeleteByDao(service))
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }


        #endregion

        #region OBSManage管理

        /// <summary>
        /// 通过ID查询管理OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OBSManage GetOBSManageById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetOBSManage(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OBSManage;
            }
            return null;
        }

        /// <summary>
        /// 通过ProjectTaskName查询管理OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetOBSManageByName(string Name)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectTaskName", Name));
            return Dao.ObjectQuery(typeof(OBSManage), objectQuery);
        }

        /// <summary>
        /// 通过ProjectTasId查询管理OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetOBSManageByProjectTaskId(string Id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectTask", Id));
            return Dao.ObjectQuery(typeof(OBSManage), objectQuery);
        }


        /// <summary>
        /// OBS管理信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOBSManage(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OBSManage), objectQuery);
        }
        [TransManager]
        public void OBSManagSave(IList lists)
        {
            IList list = lists[0] as ArrayList;
            OperationOrg oprOrg = lists[1] as OperationOrg;
            Hashtable ht = lists[2] as Hashtable;
            if (list.Count != null)
            {
                foreach (OBSManage manage in list)
                {
                    SaveOBSManage(manage);
                }
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OrpJob", oprOrg as OperationOrg));
            IList lst = GetOBSManage(oq);
            if (lst.Count > 0)
            {
                foreach (OBSManage svs in lst)
                {
                    bool flag = false;
                    foreach (System.Collections.DictionaryEntry obj in ht)
                    {
                        TreeNode node = obj.Key as TreeNode;
                        if (svs.ProjectTaskName == node.Text.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }
                    //删除信息
                    if (!flag)
                    {
                        if (!DeleteByDao(svs)) return;
                    }
                }
            }

        }

        [TransManager]
        public void SaveOBSManages(IList list, OperationOrg oprOrg, Hashtable ht)
        {
            if (list.Count != null)
            {
                foreach (OBSManage manage in list)
                {
                    SaveOBSManage(manage);
                }
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OrpJob", oprOrg as OperationOrg));
            IList lst = GetOBSManage(oq);
            if (lst.Count > 0)
            {
                foreach (OBSManage svs in lst)
                {
                    bool flag = false;
                    foreach (System.Collections.DictionaryEntry obj in ht)
                    {
                        GWBSTree node = obj.Key as GWBSTree;
                        if (svs.ProjectTaskName == node.Name.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }
                    //删除信息
                    if (!flag)
                    {
                        if (!DeleteByDao(svs)) return;
                    }
                }
            }
        }

        [TransManager]
        public void SavePersons(IList list, OperationOrg oprOrg, Hashtable ht)
        {
            StandardPerson pers = new StandardPerson();
            OperationRole role = new OperationRole();
            if (list.Count > 0)
            {
                foreach (OBSPerson person in list)
                {
                    pers = person.ManagePerson;
                    role = person.PersonRole;
                    SaveOBSPerson(person);
                }
            }
            ObjectQuery oqy = new ObjectQuery();
            oqy.AddCriterion(Expression.Eq("OrpJob", oprOrg as OperationOrg));
            oqy.AddCriterion(Expression.Eq("PersonRole", role as OperationRole));
            oqy.AddCriterion(Expression.Eq("ManagePerson", pers as StandardPerson));
            IList listPersons = GetOBSPerson(oqy);
            if (listPersons != null)
            {
                if (listPersons.Count > 0)
                {
                    foreach (OBSPerson svs in listPersons)
                    {
                        bool flag = false;
                        foreach (OBSPerson person in list)
                        {
                            if (person.ProjectTask.Id == svs.ProjectTask.Id)
                            {
                                if (person.PersonRole.Id == svs.PersonRole.Id)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        //删除信息
                        if (!flag)
                        {
                            if (!DeleteByDao(svs)) return;
                        }
                    }
                }
            }
        }

        [TransManager]
        public OBSManage SaveOBSManage(OBSManage obj)
        {
            return SaveOrUpdateByDao(obj) as OBSManage;
        }
        [TransManager]
        public OBSPerson SaveOBSPerson(OBSPerson obj)
        {
            return SaveOrUpdateByDao(obj) as OBSPerson;
        }
        /// <summary>
        /// OBS管理信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOBSPerson(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OBSPerson), objectQuery);
        }



        /// <summary>
        /// 用户岗位信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetPersonjob(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("StandardPerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperationJob.OperationOrg", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(PersonOnJob), objectQuery);
        }
        /// <summary>
        /// 岗位角色信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetJobRole(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("OperationJob.OperationOrg", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperationRole", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OperationJobWithRole), objectQuery);
        }






        #endregion

        /// <summary>
        /// 业务组织信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOperationOrg(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperationJobs.JobWithRole", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OperationOrg), objectQuery);
        }
    }
}
