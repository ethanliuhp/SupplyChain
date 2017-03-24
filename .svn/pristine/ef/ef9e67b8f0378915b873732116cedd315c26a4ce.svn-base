using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.InitialData.Domain;
using System.Collections;

namespace Application.Business.Erp.Financial.InitialData.Service
{
    public class IAccountLevelService : Application.Business.Erp.Financial.InitialData.Service.IIAccountLevelService
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        public bool SaveData(AccountLevel level)
        {
            return dao.SaveOrUpdate(level);
        }

        public bool DeleteData(AccountLevel level)
        {
            return dao.Delete(level);
        }

        public IList QueryData(ObjectQuery oq)
        {
            return dao.ObjectQuery(typeof(AccountLevel),oq);
        }
    }
}
