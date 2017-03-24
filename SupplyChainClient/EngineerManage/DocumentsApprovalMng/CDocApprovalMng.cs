using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Windows.Documents;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng
{
    public enum CDocApprovalMng_ExexType
    {
        VDocApprovalManage,
        /// <summary>
        /// 工程文档审批查询
        /// </summary>
        DocApprovalManageSearch
    }
    public class CDocApprovalMng
    {
        private static IFramework framework = null;
        string mainViewName = "工程文档审批管理";
        private static VDocApprovalSearchList searchList;


        public CDocApprovalMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VDocApprovalSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }

        public void Find(string name, string Id)
        {
            string captionName = mainViewName;
            if (name is string)
            {
                captionName = this.mainViewName + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VDocApprovalMaintain vMainView = framework.GetMainView(mainViewName + "-空") as VDocApprovalMaintain;

            if (vMainView == null)
            {
                vMainView = new VDocApprovalMaintain();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VDocApprovalSearchCon searchCon = new VDocApprovalSearchCon(searchList);
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CDocApprovalMng_ExexType))
                {
                    CDocApprovalMng_ExexType execType = (CDocApprovalMng_ExexType)obj;
                    switch (execType)
                    {
                        case CDocApprovalMng_ExexType.DocApprovalManageSearch:
                            IMainView mroqMv = framework.GetMainView("工程文档审批查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VDocApprovalQuery vcq = new VDocApprovalQuery();
                            vcq.ViewCaption = "工程文档审批查询";
                            framework.AddMainView(vcq);
                            return null;
                        default:
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
