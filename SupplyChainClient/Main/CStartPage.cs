using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.Main
{
    public class CStartPage
    {
        IFramework framework;

        string mainViewName = "���˹���̨";

        public IFramework Framework
        {
            get { return framework; }
            set { framework = value; }
        }

        public CStartPage(IFramework fw)
        {
            framework = fw;
        }

        public void Start()
        {
            IMainView mv = framework.GetMainView(mainViewName);
            //XycEncrypt.EncryptSrv es = new XycEncrypt.EncryptSrv();
            //if (es.EncryptRead(XycEncrypt.enmVirtualMachineType.All, XycEncrypt.enmFunctionals.SupplyRelationshipManagement)==false)
            //{
            //    MessageBox.Show("�Ҳ�����������");
            //    return;
            //}
            if (mv != null)
            {
                //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                mv.ViewShow();
                return;
            }

            VStartPage vsp = new VStartPage();
            vsp.ViewName = mainViewName;
            vsp.ViewCaption = mainViewName;
            vsp.Start();
            framework.AddMainView(vsp);

        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
                Start();

            return null;
        }
    }
}
