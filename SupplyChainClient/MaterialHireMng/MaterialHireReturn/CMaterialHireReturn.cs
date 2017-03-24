using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn;


namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn
{
    public enum CMaterialHireReturn_ExecType
    {
        /// <summary> 料具退料单查询 </summary>
        MaterialHireReturnQuery,
        /// <summary> 钢管一米以下料具退料单查询 </summary>
        MaterialHireReturnGGLessOneQuery,
        /// <summary> 普通料具退料</summary>
        MaterialHireReturn,
        /// <summary> 普料具退料(耗损)</summary>
        MaterialHireReturnLoss,
        /// <summary> 碗扣退料</summary>
        MaterialHireWKReturn,
        /// <summary> 碗扣退料(耗损)</summary>
        MaterialHireWKReturnLoss,
        /// <summary> 钢管退料</summary>
        MaterialHireGGReturn,
        /// <summary> 钢管退料(耗损) </summary>
        MaterialHireGGReturnLoss

    }
   public  class CMaterialHireReturn
    {
   private static IFramework framework = null;
        string mainViewName = "料具退料单";
        private  VMaterialHireReturnSearchList searchList;

        public CMaterialHireReturn(IFramework fm)
        {
            if (framework == null)
                framework = fm;
           
        }

        public void Start(bool IsLoss, EnumMatHireType matHireType )
        {
            Find("空", "空", IsLoss, matHireType);
        }

        public void Find(string name, string id, bool IsLoss, EnumMatHireType matHireType)
        {
            mainViewName = matHireType == EnumMatHireType.普通料具 ? "料具退料单" : (matHireType == EnumMatHireType.钢管?"钢管退料单":"碗扣退料单");
            if (IsLoss) { mainViewName += "(损耗)"; }
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
            VMaterialHireReturn vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialHireReturn;

            if (vMainView == null)
            {
                vMainView = new VMaterialHireReturn(IsLoss, matHireType);
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                 searchList = new VMaterialHireReturnSearchList(this);
                searchList.IsLoss=IsLoss;
                searchList.MatHireType=matHireType;
                vMainView.AssistViews.Add(searchList);
                VMaterialHireReturnSearchCon searchCon = new VMaterialHireReturnSearchCon(searchList, IsLoss, matHireType);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            object obj = args[0];
            if (obj != null && obj.GetType() == typeof(CMaterialHireReturn_ExecType))
            {
                CMaterialHireReturn_ExecType execType = (CMaterialHireReturn_ExecType)obj;
                switch (execType)
                {
                    case CMaterialHireReturn_ExecType.MaterialHireReturn:
                        {
                            Start(false, EnumMatHireType.普通料具);
                            break;
                        }
                    case CMaterialHireReturn_ExecType.MaterialHireReturnLoss:
                        {
                            Start(true, EnumMatHireType.普通料具);
                            break;
                        }
                    case CMaterialHireReturn_ExecType.MaterialHireWKReturn:
                        {
                            Start(false, EnumMatHireType.碗扣);
                            break;
                        }
                    case CMaterialHireReturn_ExecType.MaterialHireWKReturnLoss:
                        {
                            Start(true, EnumMatHireType.碗扣);
                            break;
                        }
                    case CMaterialHireReturn_ExecType.MaterialHireGGReturn:
                        {
                            Start(false, EnumMatHireType.钢管);
                            break;
                        }
                    case CMaterialHireReturn_ExecType.MaterialHireGGReturnLoss:
                        {
                            Start(true, EnumMatHireType.钢管);
                            break;
                        }
                    case CMaterialHireReturn_ExecType.MaterialHireReturnQuery:
                        {
                            IMainView mroqMv = framework.GetMainView("退料单统计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialHireReturnQuery vmcq = new VMaterialHireReturnQuery();
                            vmcq.ViewCaption = "退料单统计查询";
                            framework.AddMainView(vmcq);
                            break;
                        }
                    case CMaterialHireReturn_ExecType.MaterialHireReturnGGLessOneQuery:
                        {
                            IMainView mroqMv = framework.GetMainView("一米以下钢管退料查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMaterialHireReturnGGLessOneQuery vmcq = new VMaterialHireReturnGGLessOneQuery();
                            vmcq.ViewCaption = "一米以下钢管退料查询";
                            framework.AddMainView(vmcq);
                            break;
                        }
                }
            }

            return null;
        }
    }
}
