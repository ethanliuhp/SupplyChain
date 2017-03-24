using System;
using Application.Resource.MaterialResource.Domain;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Base.Service;
namespace Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Service
{
    public interface IBillUserSrv : IBaseBasicDataSrv
    {
        IList GetObjects(string billCode);
    }
}
