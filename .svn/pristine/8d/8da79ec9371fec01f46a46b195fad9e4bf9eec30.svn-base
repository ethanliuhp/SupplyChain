using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Component.WinMVC.generic.print;
using Application.Business.Erp.Secure.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class PrintFactory
    {
        private static IPrintSolutionAccess _SolutionAccess;

        static IPrintSolutionAccess SolutionAccess
        {
            get
            {
                if (_SolutionAccess == null)
                    _SolutionAccess = StaticMethods.GetService(typeof(IPrintSolutionAccess)) as IPrintSolutionAccess;

                return _SolutionAccess;
            }
        }

        public static PreviewExplorer CreatePrintPreview()
        {

            PreviewExplorer pe = new PreviewExplorer();
            pe.LoadPrintSolutions += new LoadPrintSolutionsHandler(PrintExplorer_LoadPrintSolutions);
            pe.SavePrintSolutions += new SavePrintSolutionsHandler(PrintExplorer_SavePrintSolutions);
            return pe;
        }

        public static PreviewExplorer CreatePrintPreview(string documentType)
        {

            PreviewExplorer pe = new PreviewExplorer();
            pe.DocumentType = documentType;

            pe.LoadPrintSolutionsByType += new LoadPrintSolutionsByTypeHandler(pe_LoadPrintSolutionsByType);
            pe.SavePrintSolutionsByType += new SavePrintSolutionsByTypeHandler(pe_SavePrintSolutionsByType);
            return pe;
        }

        static void pe_SavePrintSolutionsByType(string documentName, string documentType, IDictionary<string, byte[]> solutions)
        {
            if (documentName != null && documentName.Trim() != "")
                SolutionAccess.Save(documentName,documentType, solutions);
        }

        static void pe_LoadPrintSolutionsByType(string documentName, string documentType, IDictionary<string, byte[]> solutions)
        {
            if (documentName != null && documentName.Trim() != "")
            {
                IDictionary<string, byte[]> dic = SolutionAccess.Load(documentName, documentType);
                foreach (KeyValuePair<string, byte[]> kv in dic)
                {
                    solutions.Add(kv.Key, kv.Value);
                }
            }
        }

        static void PrintExplorer_LoadPrintSolutions(string documentName,IDictionary<string, byte[]> solutions)
        {
            if (documentName != null && documentName.Trim() != "")
            {
                IDictionary<string, byte[]> dic = SolutionAccess.Load(documentName);
                foreach (KeyValuePair<string, byte[]> kv in dic)
                {
                    solutions.Add(kv.Key, kv.Value);
                }
            }
        }

        static void PrintExplorer_SavePrintSolutions(string documentName, IDictionary<string, byte[]> solutions)
        {
            if (documentName != null && documentName.Trim() != "")
                SolutionAccess.Save(documentName, solutions);
        }

        public static IDictionary<string, byte[]> GetSolutions(string documentName)
        {
            IDictionary<string, byte[]> dic=new Dictionary<string,byte[]>();

            if (documentName != null && documentName.Trim() != "")
                dic = SolutionAccess.Load(documentName);
            
            return dic;
        }

        public static IDictionary<string, byte[]> GetSolutions(string documentName,string documentType)
        {
            IDictionary<string, byte[]> dic = new Dictionary<string, byte[]>();

            if (documentName != null && documentName.Trim() != "")
                dic = SolutionAccess.Load(documentName, documentType);
            
            return dic;
        }
    }
}
