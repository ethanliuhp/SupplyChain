using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload
{
    public enum CFilesUpLoaad_ExecType
    {
        VFilesUpLoad
    }
    public class CFilesUpLoad
    {
        private static IFramework framework = null;
        string mainViewName = "文件上传";

        public CFilesUpLoad(IFramework fm)
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
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CFilesUpLoaad_ExecType))
                {
                    CFilesUpLoaad_ExecType execType = (CFilesUpLoaad_ExecType)obj;
                    switch (execType)
                    {
                        case CFilesUpLoaad_ExecType.VFilesUpLoad:
                            IMainView mroqMv = framework.GetMainView("文件上传");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VFilesUpLoad vfu = new VFilesUpLoad();
                            vfu.ViewCaption = "文件上传";
                            framework.AddMainView(vfu);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
