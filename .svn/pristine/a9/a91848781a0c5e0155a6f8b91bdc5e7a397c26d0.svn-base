using System;
using System.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Base.Service
{
    public interface IPrintSolutionAccess
    {
        IDictionary<string, byte[]> Load(string title);
        IDictionary<string, byte[]> Load(string title, string menuStr);
        void Save(string title, IDictionary<string, byte[]> solutions);
        void Save(string title, string menuStr, IDictionary<string, byte[]> solutions);
    }
}
