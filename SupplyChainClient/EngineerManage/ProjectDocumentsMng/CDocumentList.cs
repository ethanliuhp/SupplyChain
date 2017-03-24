using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public enum CDocumentList_ExecType
    {
        VDocumentLisBak = 1,
        文档分类编码前缀映射配置 = 2,
        对象类型关联文档分类配置 = 3
    }
    public class CDocumentList
    {
        private static IFramework framework = null;
        //string mainViewName = "常用短语维护";

        public CDocumentList(IFramework fm)
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
                if (obj != null && obj.GetType() == typeof(CDocumentList_ExecType))
                {
                    CDocumentList_ExecType execType = (CDocumentList_ExecType)obj;
                    IMainView mv = null;
                    switch (execType)
                    {
                        case CDocumentList_ExecType.VDocumentLisBak:
                            mv = framework.GetMainView("文档管理");
                            if (mv != null)
                            {
                                mv.ViewShow();
                                return null;
                            }
                            VDocumentsListBak vdl = new VDocumentsListBak();
                            vdl.ViewCaption = "文档管理";
                            framework.AddMainView(vdl);
                            return null;
                        case CDocumentList_ExecType.文档分类编码前缀映射配置:
                            string viewName = "文档分类编码前缀映射配置";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                MDocumentMng mmc = new MDocumentMng();
                                VDocCateCodeProfixMap vmc = new VDocCateCodeProfixMap();
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        case CDocumentList_ExecType.对象类型关联文档分类配置:
                            viewName = "对象类型关联文档分类配置";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                MDocumentMng mmc = new MDocumentMng();
                                VDocCateCodeProfixMap vmc = new VDocCateCodeProfixMap();
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return null;
        }
    }
}
