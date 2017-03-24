using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public enum EnumAccountMng
    {
        公司财务账面维护 = 0,
        分公司财务账面维护 = 1,
        公司财务账面维护查询 = 2,
        分公司财务账面维护查询 = 3,
        管理费用台账=4,
        费用信息查询=30
    }
    public class CAccountMng
    {
        public CAccountMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            // ************************searchList = new VIndirectCostSearchList(this);*********************
        }
        private static IFramework framework = null;
        string mainViewName = "";
        private static VAccountMngSearchList searchList;
        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                EnumAccountMng enumType = (EnumAccountMng)args[0];
                switch (enumType)
                {
                    case EnumAccountMng.分公司财务账面维护:
                        {
                            mainViewName = "分公司财务账面维护";
                            Start(enumType);
                            break;
                        }
                    case EnumAccountMng.公司财务账面维护:
                        {
                            mainViewName = "公司财务账面维护";
                            Start(enumType);
                            break;
                        }
                    case EnumAccountMng.分公司财务账面维护查询:
                        {
                            mainViewName = "分公司财务账面维护查询";
                            VAccountMngQuery oQuery = framework.GetMainView(mainViewName) as VAccountMngQuery;
                            if (oQuery == null)
                            {
                                oQuery = new VAccountMngQuery(enumType);
                                oQuery.ViewCaption = mainViewName;
                                framework.AddMainView(oQuery);
                            }
                            oQuery.ViewShow();
                            break;
                        }
                    case EnumAccountMng.公司财务账面维护查询:
                        {
                            mainViewName = "公司财务账面维护查询";
                            VAccountMngQuery oQuery = framework.GetMainView(mainViewName) as VAccountMngQuery;
                            if (oQuery == null)
                            {
                                oQuery = new VAccountMngQuery(enumType);
                                oQuery.ViewCaption = mainViewName;
                                framework.AddMainView(oQuery);
                            }
                            oQuery.ViewShow();
                            break;
                        }
                    case EnumAccountMng.管理费用台账:
                        {
                            mainViewName = "管理费用台账";
                            VAccountMngReport oQuery = framework.GetMainView(mainViewName) as VAccountMngReport;
                            if (oQuery == null)
                            {
                                oQuery = new VAccountMngReport(enumType);
                                oQuery.ViewCaption = mainViewName;
                                framework.AddMainView(oQuery);
                            }
                            oQuery.ViewShow();
                            break;
                        }
                    case EnumAccountMng.费用信息查询:
                        {
                            mainViewName = "费用信息查询";
                            VAccountMngQuery oQuery = framework.GetMainView(mainViewName) as VAccountMngQuery;
                            if (oQuery == null)
                            {
                                oQuery = new VAccountMngQuery(enumType);
                                oQuery.ViewCaption = mainViewName;
                                framework.AddMainView(oQuery);
                            }
                            oQuery.ViewShow();
                            break;
                        }
                }

               
            }
            return null;
        }

        private void Start(EnumAccountMng enumType)
        {
            Find("空", enumType, "空");
        }

        public  void Find(string name, EnumAccountMng enumType, string id)
        {
            string captionName = mainViewName + "-" + name;
            IMainView mv = framework.GetMainView(captionName);
            if (mv != null)
            {
                mv.ViewShow();
                return   ;
            }
            VAccountMng mainView = framework.GetMainView(mainViewName + "-空") as VAccountMng;
            if (mainView == null)
            {

                mainView = new VAccountMng( enumType);
                //mainView.ExcuteType = enumType;
                mainView.ViewName = mainViewName;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);


                //分配辅助视图
                searchList = new VAccountMngSearchList(this, enumType);
                mainView.AssistViews.Add(searchList);
                VAccountMngSearchCon oSearchCon = new VAccountMngSearchCon(searchList, enumType);
                mainView.AssistViews.Add(oSearchCon);

                //载入框架
                framework.AddMainView(mainView);
            }

            mainView.ViewCaption = captionName;
            mainView.ViewName = mainViewName;
            mainView.Start(id);
        }
        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VAccountMng vDmand = mv as VAccountMng;
            if (vDmand != null)
                searchList.RemoveRow(vDmand.CurBillMaster.Id);


            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }


    }
}
 