using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public class CSubContractBalance
    {
        public enum CSubContractBalance_ExecType
        {
            分包结算管理 = 1,
            分包结算查询 = 2,
            单据核算科目调整 = 3,
            项目分包结算生成 = 4,
            分包终结结算单生成 = 5,//20160904 ADD
            分包终结结算单审核 = 6//20160918 ADD
        }

        private static IFramework framework = null;
        string mainViewName = "分包结算单维护";
        private static VSubContractBalanceBillSearchList searchList;

        public CSubContractBalance(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VSubContractBalanceBillSearchList(this);
        }

        public void Start()
        {
            Find("空", "");
        }
        public void Find(string code, string GUID)
        {
            string captionName = this.mainViewName + "-" + code;

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VSubContractBalanceBill vMainView = new VSubContractBalanceBill();

            //载入查询视图
            //分配辅助视图
            vMainView.AssistViews.Add(searchList);

            VSubContractBalanceBillSearchCon searchCon = new VSubContractBalanceBillSearchCon(searchList);
            vMainView.AssistViews.Add(searchCon);

            //载入框架
            framework.AddMainView(vMainView);


            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(code, GUID);

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
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (obj != null && obj.GetType() == typeof(CSubContractBalance_ExecType))
                {
                    IMainView mainView = null;
                    CSubContractBalance_ExecType execType = (CSubContractBalance_ExecType)obj;
                    switch (execType)
                    {
                        case CSubContractBalance_ExecType.分包结算管理:
                            mainView = framework.GetMainView("分包结算单维护");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                return null;
                            }
                            VSubContractBalanceBill frm = new VSubContractBalanceBill();
                            frm.ViewCaption = "分包结算单维护";
                            framework.AddMainView(frm);
                            return null;
                        case CSubContractBalance_ExecType.分包结算查询:
                            mainView = framework.GetMainView("分包结算单查询");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                return null;
                            }
                            VSubContractBalanceQuery frmQuery = new VSubContractBalanceQuery(this);
                            frmQuery.TheAuthMenu = theMenu;
                            frmQuery.ViewCaption = "分包结算单查询";
                            framework.AddMainView(frmQuery);
                            return null;
                        case CSubContractBalance_ExecType.单据核算科目调整:
                            mainView = framework.GetMainView("单据核算科目调整");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                return null;
                            }
                            VBillSubjectUpdate vbsu = new VBillSubjectUpdate(this);
                            vbsu.TheAuthMenu = theMenu;
                            vbsu.ViewCaption = "单据核算科目调整";
                            framework.AddMainView(vbsu);
                            return null;
                        case CSubContractBalance_ExecType.项目分包结算生成:
                            mainView = framework.GetMainView("项目分包结算生成");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                return null;
                            }
                            VProjectSubContractBalance vProjectSub = new VProjectSubContractBalance();
                            vProjectSub.ViewCaption = "项目分包结算生成";
                            framework.AddMainView(vProjectSub);
                            return null;

                        case CSubContractBalance_ExecType.分包终结结算单生成:
                            mainView = framework.GetMainView("分包终结结算单生成");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                return null;
                            }
                            VEndAccountAudit vEA = new VEndAccountAudit("生成");
                            vEA.ViewCaption = "分包终结结算单生成";
                            vEA.flag = "生成";
                            framework.AddMainView(vEA);
                            return null;

                        case CSubContractBalance_ExecType.分包终结结算单审核:
                            mainView = framework.GetMainView("分包终结结算单审核");
                            if (mainView != null)
                            {
                                mainView.ViewShow();
                                return null;
                            }
                            VEndAccountAudit vEAs = new VEndAccountAudit("审核");
                            vEAs.ViewCaption = "分包终结结算单审核";
                            vEAs.flag = "审核";
                            framework.AddMainView(vEAs);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
