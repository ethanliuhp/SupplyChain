using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.Controls;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class ObjectLock
    {
        public static void Lock(Control o, bool isContainer)
        {
            if (isContainer)
            {
                LockControl(o, true);
            }
            else
                Lock(o);
        }

        public static void Unlock(Control o, bool isContainer)
        {
            if (o.GetType() is OnlyReadLabel)
                return;
            if (isContainer)
            {
                LockControl(o, false);
            }
            else
                Unlock(o);
        }


        //遍历控件容器中的每个子控件：如果locked=true,锁定；否则，解锁
        private static void LockControl(Control c, bool locked)
        {
            foreach (Control c1 in c.Controls)
            {
                LockControl(c1, locked);
            }

            if (locked)
                Lock(c);
            else
                Unlock(c);
        }

        public static void Lock(object o)
        {
            string objectType = o.GetType().Name;

            switch (objectType)
            {
                case "CustomLinkLabel":
                    (o as CustomLinkLabel).Enabled = false;
                    break;
                case "ComboBox":
                    (o as ComboBox).Enabled = false;
                    break;
                case "CustomEdit":
                    (o as CustomEdit).ReadOnly = true;
                    break;
                case "CustomCheckBox":
                    (o as CustomCheckBox).ReadOnly = true;
                    break;
                case "CustomDataGridView":
                    (o as CustomDataGridView).ReadOnly = true;
                    break;
                case "CustomButton":
                    (o as CustomButton).Enabled = false;
                    break;
                case "CustomNormalTextBox":
                    (o as CustomNormalTextBox).ReadOnly = true;
                    break;
                case "CustomComboBox":
                    (o as CustomComboBox).ReadOnly = true;
                    break;
                case "CommonCurrency":
                    (o as CommonCurrency).Enabled = false;
                    break;
                case "CustomDateTimePicker":
                    (o as CustomDateTimePicker).Enabled = false;
                    break;
                case "CommonCustomer":
                    (o as CommonCustomer).Enabled = false;
                    break;
                case "Buttom":
                    (o as Button).Enabled = false;
                    break;
                case "CommonSupplier":
                    (o as CommonSupplier).Enabled = false;
                    break;
                case "CommonPerson":
                    (o as CommonPerson).Enabled = false;
                    break;
                case "CustomRichTextBox":
                    (o as CustomRichTextBox).Enabled = false;
                    break;
                case "Button":
                    (o as Button).Enabled = false;
                    break;
                case "TextBox":
                    (o as TextBox).Enabled = false;
                    (o as TextBox).BackColor = System.Drawing.SystemColors.Control;
                    break;
                case "CheckBox":
                    (o as CheckBox).Enabled = false;
                    break;
                //case "CommonGuestCom":
                //    (o as CommonGuestCom).Enabled = false;
                //    break;
                //case "CommonItemMess":
                //    (o as CommonItemMess).Enabled = false;
                //    break;
                //case "UCCommonBase":
                //    (o as UCCommonBase).Enabled = false;
                //    break;

                case"DateTimePicker":
                    (o as DateTimePicker).Enabled = false;
                    break;
            }
        }

        public static void Unlock(object o)
        {
            string objectType = o.GetType().Name;
            switch (objectType)
            {

                case "CustomLinkLabel":
                    (o as CustomLinkLabel).Enabled = true;
                    break;
                case "ComboBox":
                    (o as ComboBox).Enabled = true;
                    break;
                case "CustomEdit":
                    (o as CustomEdit).ReadOnly = false;
                    break;
                case "CustomCheckBox":
                    (o as CustomCheckBox).ReadOnly = false;
                    break;
                case "CustomDataGridView":
                    (o as CustomDataGridView).ReadOnly = false;
                    break;
                case "CustomButton":
                    (o as CustomButton).Enabled = true;
                    break;
                case "CustomNormalTextBox":
                    (o as CustomNormalTextBox).ReadOnly = false;
                    break;
                case "CustomComboBox":
                    (o as CustomComboBox).ReadOnly = false;
                    break;
                case "CommonCurrency":
                    (o as CommonCurrency).Enabled = true;
                    break;
                case "CustomDateTimePicker":
                    (o as CustomDateTimePicker).Enabled = true;
                    break;
                case "CommonCustomer":
                    (o as CommonCustomer).Enabled = true;
                    break;
                case "Buttom":
                    (o as Button).Enabled = true;
                    break;
                case "CommonSupplier":
                    (o as CommonSupplier).Enabled = true;
                    break;
                case "CommonPerson":
                    (o as CommonPerson).Enabled = true;
                    break;
                case "Button":
                    (o as Button).Enabled = true;
                    break;
                //case "CommonItemMess":
                //    (o as CommonItemMess).Enabled = true;
                //    break;
                case "UCCommonBase":
                    (o as UCCommonBase).Enabled = true;
                    break;
                case "CustomRichTextBox":
                    (o as CustomRichTextBox).Enabled = true;
                    break;
                case "TextBox":
                    (o as TextBox).Enabled = true;
                    (o as TextBox).BackColor = System.Drawing.SystemColors.Window; 
                    break;
                case "CheckBox":
                    (o as CheckBox).Enabled = true;
                    break;

                case "DateTimePicker":
                    (o as DateTimePicker).Enabled = true;
                    break;
            }
        }

        public static void Lock(object[] os)
        {
            for (int i = 0; i < os.Length; i++)
            {
                Lock(os[i]);
            }
        }

        public static void Unlock(object[] os)
        {
            for (int i = 0; i < os.Length; i++)
            {
                Unlock(os[i]);
            }
        }
    }
}
