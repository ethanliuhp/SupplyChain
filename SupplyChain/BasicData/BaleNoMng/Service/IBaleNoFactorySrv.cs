using System;
using Application.Resource.MaterialResource.Domain;
namespace Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Service
{
    public interface IBaleNoFactorySrv
    {
        string GetMaxBaleNo(string prefix, int seqLength, DateTime createDate);
        //string GetMaxBaleNo(enmOpeRole aOpeRole, DateTime createDate);
    }
}
