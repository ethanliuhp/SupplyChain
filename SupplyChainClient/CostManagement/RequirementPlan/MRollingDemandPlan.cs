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
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public class MRollingDemandPlan
    {
        //private static IResourceRequirePlanSrv mm;

        private IResourceRequirePlanSrv mm;
        public IResourceRequirePlanSrv Mm
        {
            get { return mm; }
            set { mm = value; }
        }

        public MRollingDemandPlan()
        {
            if (mm == null)
                mm = ConstMethod.GetService("ResourceRequirePlanSrv") as IResourceRequirePlanSrv;
        }

        /// <summary>
        /// ������޸���Դ����ƻ�����
        /// </summary>
        /// <param name="list">��Դ����ƻ�����</param>
        /// <returns></returns>
        public IList SaveOrUpdateResourceRequirePlan(IList list)
        {
            return mm.SaveOrUpdateResourceRequirePlan(list);
        }

        /// <summary>
        /// ������޸���Դ����ƻ�
        /// </summary>
        /// <param name="group">��Դ����ƻ�</param>
        /// <returns></returns>
        public ResourceRequirePlan SaveOrUpdateResourceRequirePlan(ResourceRequirePlan cg)
        {
            return mm.SaveOrUpdateResourceRequirePlan(cg);
        }

        /// <summary>
        /// ���ݶ������ͺ�GUID��ȡ����
        /// </summary>
        /// <param name="entityType">��������</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return mm.GetObjectById(entityType, Id);
        }

        /// <summary>
        /// ɾ����Դ����ƻ�����
        /// </summary>
        /// <param name="list">��Դ����ƻ�����</param>
        /// <returns></returns>
        public bool DeleteResourceRequirePlan(IList list)
        {
            return mm.DeleteResourceRequirePlan(list);
        }

        /// <summary>
        /// ������޸���Դ����ƻ���
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        public ResourceRequireReceipt SaveOrUpdateResourceRequireReceipt(ResourceRequireReceipt rrr)
        {
            return mm.SaveOrUpdateResourceRequireReceipt(rrr);
        }

        /// <summary>
        /// ������޸Ĺ�����Դ����ƻ��ͼƻ���ϸ
        /// </summary>
        /// <param name="plan">������Դ����ƻ�</param>
        /// <param name="list">������Դ����ƻ���ϸ����</param>
        /// <param name="isPublish">ɾ��</param>
        /// <param name="deleteList">�Ƿ񷢲�</param>
        /// <returns></returns>
        public Hashtable SaveOrUpdateResourcePlanAndDetail(ResourceRequirePlan plan, IList list, bool isPublish,IList deleteList)
        {
            return mm.SaveOrUpdateResourcePlanAndDetail(plan, list, isPublish,deleteList);
        }
        /// <summary>
        /// ������޸ļƻ���ϸ
        /// </summary>
        /// <param name="group">�ƻ���ϸ</param>
        /// <returns></returns>
        public ResourceRequirePlanDetail SaveOrUpdateResourcePlanDetail(ResourceRequirePlanDetail dtl)
        {
            return mm.SaveOrUpdateResourcePlanDetail(dtl);
        }

        /// <summary>
        /// ������޸ļƻ���ϸ����
        /// </summary>
        /// <param name="list">�ƻ���ϸ����</param>
        /// <returns></returns>
        public IList SaveOrUpdateResourcePlanDetail(IList list)
        {
            return mm.SaveOrUpdateResourcePlanDetail(list);
        }

        /// <summary>
        /// ��Դ����ƻ���ϸ����
        /// </summary>
        /// <param name="list">��Դ����ƻ���ϸ����</param>
        /// <returns></returns>
        public bool DeleteResourceRequirePlanDetail(IList list)
        {
            return mm.DeleteResourceRequirePlanDetail(list);
        }

        /// <summary>
        /// ɾ����Դ����ƻ���
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequireReceipt(ResourceRequireReceipt rrr)
        {
            return mm.DeleteResourceRequireReceipt(rrr);
        }
        /// <summary>
        /// ɾ����Դ����ƻ���
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteResourceRequireReceiptList(IList list)
        {
            return mm.DeleteResourceRequireReceiptList(list);
        }
        /// <summary>
        /// ������޸���Դ����ƻ�����ϸ����
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList SaveOrUpdateResourceRequireReceiptDetail(IList list)
        {
            return mm.SaveOrUpdateResourceRequireReceiptDetail(list);
        }

         /// <summary>
        /// ������޸���Դ����ƻ���������ϸ����
        /// </summary>
        /// <param name="rrr"></param>
        /// <param name="rrrd"></param>
        /// <returns></returns>
        public ResourceRequireReceipt SaveResourceRequireReceiptAndDetail(ResourceRequireReceipt rrr, IList list)
        {
            return mm.SaveResourceRequireReceiptAndDetail(rrr, list);
        }

        /// <summary>
        /// �����ѯ
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <param name="oq">��ѯ����</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            oq.AddFetchMode("MaterialGUID", NHibernate.FetchMode.Eager);
            return mm.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// ��ȡ������ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return mm.GetServerTime();
        }

        public DataSet SearchSQL(string sql)
        {
            return mm.SearchSQL(sql);
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type, string projectId)
        {
            return mm.GetCode(type, projectId);
        }

        /// <summary>
        /// �����Ͼ���Դ����ƻ�����������Դ����ƻ�
        /// </summary>
        /// <param name="planType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GenerateSupplyResourcePlanBak(RemandPlanType planType, string id)
        {
            return mm.GenerateSupplyResourcePlanBak(planType, id);
        }

       /// <summary>
        /// ��ȡԤ����Դ������
       /// </summary>
       /// <param name="plan"></param>
       /// <param name="projectInfo"></param>
       /// <returns></returns>
        public IList GetBudgetResourcesDemand(ResourceRequirePlan plan, CurrentProjectInfo projectInfo,GWBSTree wbs)
        {
            return mm.GetBudgetResourcesDemand(plan, projectInfo,wbs);
        }

        public IList GetBudgetResourcesDemand(ResourceRequireReceipt plan ,CurrentProjectInfo projectInfo, GWBSTree wbs, DateTime beginDate, DateTime endDate,ResourceTpye rt ,PlanType st)
        {
            return mm.GetBudgetResourcesDemand(plan,projectInfo, wbs, beginDate, endDate,rt,st);
        }

        public IList GenerateSupplyResourcePlan(string id)
        {
            return mm.GenerateSupplyResourcePlan(id);
        }

        public IList GenerateSupplyResourcePlanNew(string id)
        {
            return mm.GenerateSupplyResourcePlanNew(id);
        }
    }
}
