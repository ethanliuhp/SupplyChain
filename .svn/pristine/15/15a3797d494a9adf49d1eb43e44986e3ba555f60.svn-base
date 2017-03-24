using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client
{
    public class KnowledgeMessageBox
    {
        private static string caption = "综合信息管理系统";

        public static DialogResult InforMessage(string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult InforMessage(string message,Exception ex)
        {
            return MessageBox.Show(message + "\n" + StaticMethod.ExceptionMessage(ex), caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult QuestionMessage(string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
