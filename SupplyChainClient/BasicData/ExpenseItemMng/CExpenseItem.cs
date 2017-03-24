using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ExpenseItemMng
{
    public class CExpenseItem
    {
        public static IFramework framework = null;
        public IExpenseItemSrv saleBudgeSrv = null;
        string mainViewName = "费用项目";

        public CExpenseItem(IFramework fm)
        {
            if (framework == null)
                framework = fm;

            if (saleBudgeSrv == null)
                saleBudgeSrv = StaticMethod.GetService("ExpenseItemSrv") as IExpenseItemSrv;
        }

        public void Start()
        {
            Find("空");
        }

        public void Find(string name)
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

            MExpenseItem mExpenseItem = new MExpenseItem();
            mExpenseItem.saleBudgeSrv = saleBudgeSrv;


            VExpenseItem vExpenseItem = framework.GetMainView(mainViewName + "-空") as VExpenseItem;

            if (vExpenseItem == null)
            {
                vExpenseItem = new VExpenseItem(mExpenseItem, this);
                vExpenseItem.ViewName = mainViewName;

                //载入查询视图          


              //  vExpenseItem.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);


                //载入框架
                framework.AddMainView(vExpenseItem);
            }

            vExpenseItem.ViewCaption = captionName;
            vExpenseItem.ViewName = mainViewName;
            vExpenseItem.Start(name);


            vExpenseItem.ViewShow();

        }

     

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }

            return null;
        }

    }
}
