using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    public  interface ICashFlowItemSrv
    {
        IList ShowItems(ObjectQuery oq);
        CashFlowItems GetItems(long itemId);
        bool SaveItems(CashFlowItems Items);
        bool UpdateItems(CashFlowItems Items);
        bool DeleteItems(long itemId);
        IList SearchItems(string code, string name);
    }
}
