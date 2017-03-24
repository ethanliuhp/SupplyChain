using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.WinMVC.core;
using Application.Business.Erp.Financial.GlobalInfo;
using VirtualMachine.Component;
using System.Data;
using System.Collections;
using VirtualMachine.SystemAspect.Security.FunctionSecurity.Domain;
using System.Drawing;
//using Application.Business.Erp.Financial.Client.Main;
using VirtualMachine.Component.WinControls.Controls;
using System.Diagnostics;
using Application.Business.Erp.SupplyChain.Client.Main;

namespace Application.Business.Erp.Financial.Client.Basic.CommonClass
{

    /// <summary>
    /// ��Ŀ�����з�MVC��ͼ����Ļ���
    /// </summary>
    public class BaseForm : Form
    {
       // protected ICommon comm;

        public BaseForm()
        {
            myFramework = UCL.Framework as Framework;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(BaseForm_KeyDown);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.X))
            {
                //���д�baseform�����Ĵ�������ͳһ�Ŀ���˳���ʽ
                this.Close();
            }
        }

        private SysMenu currMenu;
        /// <summary>
        /// ��ǰ�������ܲ˵�
        /// </summary>
        public SysMenu CurrMenu
        {
            get { return currMenu; }
            set { currMenu = value; }
        }


        private Framework myFramework;

        public Framework MyFramework
        {
            get { return myFramework; }
            set { myFramework = value; }
        }
    }
}
