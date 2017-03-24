using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.HelpOnlineManage.Domain;
using System.Data;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.HelpOnlineManage.Service
{
    public interface IHelpOnlineSrv : IBaseService
    {
        HelpOnlineMng saveImp(HelpOnlineMng obj);
        DataSet HelpOnlineQuery(string condition);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
    }
}
