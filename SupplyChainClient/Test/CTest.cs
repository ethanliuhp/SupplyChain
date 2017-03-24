using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.Test
{
    public class CTest
    {
        private static IFramework _framework = null;
        public CTest(IFramework framework)
        {
            if (_framework == null)
            {
                _framework = framework;
            }
        }

        public object Execute(params object[] args)
        {
            IMainView mv = _framework.GetMainView(args[0].ToString());
            if (mv != null)
            {
                mv.ViewShow();
                return null;
            }

            var testForm = new TestForm();
            testForm.ViewCaption = args[0].ToString();
            testForm.ViewName = args[0].ToString();

            _framework.AddMainView(testForm);

            return null;
        }
    }
}
