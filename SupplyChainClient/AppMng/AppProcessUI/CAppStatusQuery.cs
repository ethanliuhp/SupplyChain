using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.ResourceManager.Client.Main;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using VirtualMachine.Core;
using System.Collections;
using System.Windows.Forms;
using VirtualMachine.Core.Expression;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public class CAppStatusQuery
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework framework;
        public CAppStatusQuery(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            IMainView mv = framework.GetMainView("����״̬��ѯ");
            if (mv != null)
            {
                //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                mv.ViewShow();
                return null;
            }
            VAppStatusQuery theVAppStatusQuery = new VAppStatusQuery();
            theVAppStatusQuery.ViewCaption = "����״̬��ѯ";
            framework.AddMainView(theVAppStatusQuery);
            return null;

        }
        public object ExcuteByBill(object o, string Code)
        {
            IMainView mv = framework.GetMainView("����״̬���ݲ�ѯ");
            if (mv != null)
            {
                //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                mv.ViewShow();
                return null;
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ClassName", o.GetType().Name));
            MAppStatusQuery model = new MAppStatusQuery();
            IList ls = model.GetObjects(typeof(AppTableSet), oq);

            if (ls.Count > 0)
            {
                VAppStatusQuery theVAppStatusQuery = new VAppStatusQuery();
                theVAppStatusQuery.ViewCaption = "����״̬���ݲ�ѯ";
                framework.AddMainView(theVAppStatusQuery);
            }
            else
            {
                MessageBox.Show("û�иõ��ݵ��������壬��ѯʧ�ܣ�");
                return null;
            }
            return null;
        }
    }
}

