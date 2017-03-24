using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ExpenseItemMng
{
    public class CExpenseItem
    {
        public static IFramework framework = null;
        public IExpenseItemSrv saleBudgeSrv = null;
        string mainViewName = "������Ŀ";

        public CExpenseItem(IFramework fm)
        {
            if (framework == null)
                framework = fm;

            if (saleBudgeSrv == null)
                saleBudgeSrv = StaticMethod.GetService("ExpenseItemSrv") as IExpenseItemSrv;
        }

        public void Start()
        {
            Find("��");
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
                //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                mv.ViewShow();
                return;
            }

            MExpenseItem mExpenseItem = new MExpenseItem();
            mExpenseItem.saleBudgeSrv = saleBudgeSrv;


            VExpenseItem vExpenseItem = framework.GetMainView(mainViewName + "-��") as VExpenseItem;

            if (vExpenseItem == null)
            {
                vExpenseItem = new VExpenseItem(mExpenseItem, this);
                vExpenseItem.ViewName = mainViewName;

                //�����ѯ��ͼ          


              //  vExpenseItem.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);


                //������
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
