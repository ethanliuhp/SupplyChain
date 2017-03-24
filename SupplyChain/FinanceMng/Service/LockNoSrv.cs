using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.InitialData.Domain;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;

namespace Application.Business.Erp.Financial.InitialData.Service
{
    /// <summary>
    /// ��������
    /// </summary>
    public class LockNoSrv : Application.Business.Erp.SupplyChain.FinanceMng.Service.ILockNoSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool SaveData(LockNo ln)
        {
            return dao.SaveOrUpdate(ln);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool UpdateData(LockNo ln)
        {
            return dao.Update(ln);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool DeleteData(LockNo ln)
        {
            return dao.Delete(ln);
        }

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        virtual public IList QueryData(ObjectQuery oq)
        {
            return dao.ObjectQuery(typeof(LockNo),oq);
        }

    }
}
