using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Domain;
using VirtualMachine.Core.Expression;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.MaterialResource.Service;
using VirtualMachine.Component.Util;
using NHibernate;
using System.Data;
using System.Runtime.Remoting.Messaging;
using Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Service;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.BasicData.BillUserMng.Domain;

namespace Application.Business.Erp.SupplyChain.BasicData.BillUserMng.Service
{
    public class BillUserSrv : BaseBasicDataSrv, IBillUserSrv
    {
        virtual public IList GetObjects(string billCode)
        {
            IList lstReturn = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code",billCode));
            lstReturn = Dao.ObjectQuery(typeof(BillUser), oq);
            return lstReturn;
        }
    }
}
