using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic.print;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public abstract class BasePrintDoc
    {
        public IList billlist;
        public String printname;

        public bool JudgeNullObject(Object ob)
        {
            if (ob == null)
                return true;
            else return false;
        }

        public void s_AfterCreatePrintObjects(Section s, Page p)
        {
            IList<PrintTable> ptlist = p.GetTables();
            PrintTable pt = p.GetTables()[0];
            int lastRow = pt.Rows.Count - 1;
            IList<string> row = pt.Rows[lastRow];
            row[0] = "ºÏ¡¡¼Æ";
            pt.Style.AddMergeRegion(lastRow, 0, lastRow, 1);
        }
        public abstract Document GenerateDoc(string codebegin,string codeend);

        public void AddFetchModeEx(ObjectQuery oq)
        {
            oq.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialResource.ComplexUnits", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialResource.BasicUnit", NHibernate.FetchMode.Eager);
        }
    }
}
