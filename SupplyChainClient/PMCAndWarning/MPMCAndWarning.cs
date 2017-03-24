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
using Application.Business.Erp.SupplyChain.PMCAndWarning.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public class MPMCAndWarning
    {
        private static IPMCAndWarningSrv model;

        public IPMCAndWarningSrv PMCAndWarningSrv
        {
            get { return model; }
        }

        public MPMCAndWarning()
        {
            if (model == null)
            {
                model = StaticMethod.GetService("PMCAndWarningSrv") as IPMCAndWarningSrv;
                //model = ConstMethod.GetService("PMCAndWarningSrv") as IPMCAndWarningSrv;
            }
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
        /// ���ݶ������ͺ�GUID��ȡ����
        /// </summary>
        /// <param name="entityType">��������</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return model.GetObjectById(entityType, Id);
        }

        /// <summary>
        /// ��ȡ������ʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return model.GetServerTime();
        }

        /// <summary>
        /// ������޸Ķ��󼯺�
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>        
        public IList SaveOrUpdate(IList list)
        {
            return model.SaveOrUpdate(list);
        }

        /// <summary>
        /// ������޸Ķ���
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object SaveOrUpdate(object obj)
        {
            return model.SaveOrUpdate(obj);
        }

        /// <summary>
        /// ɾ���������󼯺�
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteObjList(IList list)
        {
            return model.DeleteObjList(list);
        }

        /// <summary>
        /// ����Ԥ������
        /// </summary>
        /// <returns></returns>
        public bool StartWarningServer()
        {
            return model.StartWarningServer();
        }

        /// <summary>
        /// ֹͣԤ������
        /// </summary>
        /// <returns></returns>
        public bool StopWarningServer()
        {
            return model.StopWarningServer();
        }
    }
}
