using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;

namespace Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng
{
    public enum CDocumentCategory_ExecType
    {
        VDocumentCategory = 1,
        VDocumentCategoryManage = 2,
        VDocumentsSelect = 3,
    }
    public class CDocumentCategory
    {
        private static IFramework framework = null;
        public CDocumentCategory(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CDocumentCategory_ExecType))
                {
                    CDocumentCategory_ExecType execType = (CDocumentCategory_ExecType)obj;
                    IMainView mv = null;
                    switch (execType)
                    {
                        case CDocumentCategory_ExecType.VDocumentCategory:
                            mv = framework.GetMainView("文档分类维护");
                            if (mv != null)
                            {
                                mv.ViewShow();
                                return null;
                            }
                            VDocumentCategoryMng vdl = new VDocumentCategoryMng();
                            vdl.ViewCaption = "文档分类维护";
                            framework.AddMainView(vdl);
                            return null;
                        case CDocumentCategory_ExecType.VDocumentCategoryManage:
                            mv = framework.GetMainView("文件柜详细信息维护");
                            if (mv != null)
                            {
                                mv.ViewShow();
                                return null;
                            }
                            VDocumentCabinetMng mng = new VDocumentCabinetMng();
                            mng.ViewCaption = "文件柜详细信息维护";
                            framework.AddMainView(mng);
                            return null;
                        case CDocumentCategory_ExecType.VDocumentsSelect:
                            mv = framework.GetMainView("文档综合查询");
                            if (mv != null)
                            {
                                mv.ViewShow();
                                return null;
                            }
                            VDocumentsSelect vds = new VDocumentsSelect();
                            vds.ViewCaption = "文档综合查询";
                            framework.AddMainView(vds);
                            return null;
                        default:
                            break;
                    }
                }
            }
            return null;
        }
    }
}
