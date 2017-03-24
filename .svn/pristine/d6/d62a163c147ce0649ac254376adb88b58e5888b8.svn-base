using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using VirtualMachine.Component.WinControls.Controls;
using C1.Win.C1FlexGrid;
//using CommonSearchLib.BillCodeMng.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.BasicData.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.MachineClassMng

{
    public partial class VMachineClass : Form
    {
        private static MMachineClass MMachineClass = new MMachineClass();
        private static string Title = "机组信息维护";
        public VMachineClass()
        {
            InitializeComponent();
            InitEvents();
            this.Text = Title;
            this.grpModel.Caption = Title;
            this.txtKind.DataSource = EnumUtil<enmWorkKind>.GetDescriptions();

        }
        private void InitEvents()
        {

            this.Load += new EventHandler(VMachineClass_Load);
            this.grdDtl.AfterSelChange += new RangeEventHandler(grdModel_AfterSelChange);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            barOperation.OnAuthToolBar_Click += new AuthManager.AuthMng.AuthControlsMng.OnAuthToolBar_Click(barOperation_OnAuthToolBar_Click);
            barOperation.MenusCode = this.Name.Substring(1);
            barOperation.TheUsersCode = ConstObject.TheLogin.ThePerson.Code;
        }

        void barOperation_OnAuthToolBar_Click(AuthManagerLib.AuthMng.MenusMng.Domain.Menus aMenus)
        {
            switch (aMenus.Code)
            {
                case "AddNew":
                    NewData();
                    break;
                case "Modify":
                    ModifyData();
                    break;
                case "Delete":
                    DelData();
                    break;
                case "ToExcel":
                    this.grdDtl.SaveToExcel(false);
                    break;
                case "Close":
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void DelData()
        {
            DialogResult dr = MessageBox.Show("是否删除【" + this.txtName.Text + "】？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.No) return;
            if (this.grdDtl.Row < 0)
            {
                MessageBox.Show("没有要删除的记录！");
                return;
            }
            MachineClass tmpMachineClass = this.grdDtl.Rows[this.grdDtl.Row].UserData as MachineClass;
            MMachineClass.Delete(tmpMachineClass);

            this.grdDtl.Rows.Remove(this.grdDtl.Row);
            MessageBox.Show("删除成功！");
            this.grdDtl.AutoSerial();
        }

        void ModifyData()
        {
            if (this.grdDtl.Row < 0)
            {
                MessageBox.Show("没有要修改的记录！");
                return;
            }
            MachineClass tmpMachineClass = this.grdDtl.Rows[this.grdDtl.Row].UserData as MachineClass;
            if (!ValideView()) return;

            #region 检查是否有重名
            IList lst = new ArrayList();
            ObjectQuery oq = new ObjectQuery();

            oq.AddCriterion(Expression.Eq("Name", this.txtName.Text));

            lst = MMachineClass.GetMachineClass(oq);
            if (lst.Count > 0)
            {
                if (tmpMachineClass.Id != (lst[0] as MachineClass).Id)
                {
                    MessageBox.Show("该名称已经存在！");
                    return;
                }
            }
            #endregion

            ViewToModle(tmpMachineClass);
            tmpMachineClass = MMachineClass.Save(tmpMachineClass);
            Row row = this.grdDtl.Rows[this.grdDtl.Row];
            row.UserData = tmpMachineClass;
            row["Code"] = tmpMachineClass.Code;
            row["Name"] = tmpMachineClass.Name;
            row["Kind"] = EnumUtil<enmWorkKind>.GetDescription(tmpMachineClass.WorkKind);
            row["State"] = tmpMachineClass.State;
            MessageBox.Show("修改成功！");
        }

        void NewData()
        {
            MachineClass tmpMachineClass = new MachineClass();
            if (!ValideView()) return;
            #region 检查是否有重名
            IList lst = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", this.txtName.Text));

            lst = MMachineClass.GetMachineClass(oq);
            if (lst.Count > 0)
            {
                MessageBox.Show("该名称已经存在！");
                return;
            }
            #endregion
            ViewToModle(tmpMachineClass);
            tmpMachineClass = MMachineClass.Save(tmpMachineClass);
            Row row = this.grdDtl.Rows.Add();
            row.UserData = tmpMachineClass;
            row["Code"] = tmpMachineClass.Code;
            row["Name"] = tmpMachineClass.Name;
            row["Kind"] = EnumUtil<enmWorkKind>.GetDescription(tmpMachineClass.WorkKind);
            row["State"] = tmpMachineClass.State;
        }

        private bool ValideView()
        {
            if (this.txtName.Text.Equals(""))
            {
                MessageBox.Show("名称不能为空！");
                return false;
            }
            return true;
        }

        private void ViewToModle(MachineClass aMachineClass)
        {
            aMachineClass.Code = this.txtCode.Text;
            aMachineClass.Name = this.txtName.Text;
            aMachineClass.WorkKind = EnumUtil<enmWorkKind>.FromDescription(this.txtKind.Text);
            aMachineClass.State = ClientUtil.ToInt( this.chkState.Checked);


        }

        void grdModel_AfterSelChange(object sender, RangeEventArgs e)
        {
            if (this.grdDtl.Row < 0)
            {
                return;
            }
            else
            {
                MachineClass tmpMachineClass = this.grdDtl.Rows[this.grdDtl.Row].UserData as MachineClass;
                if (tmpMachineClass != null)
                {
                    ModuleToView(tmpMachineClass);
                }
            }
        }
        private void ModuleToView(MachineClass aMachineClass)
        {
            if (aMachineClass == null) return;
            this.txtCode.Text = aMachineClass.Code;
            this.txtName.Text = aMachineClass.Name;
            this.txtKind.Text = EnumUtil<enmWorkKind>.GetDescription(aMachineClass.WorkKind);
            this.chkState.Checked =ClientUtil.ToBool( aMachineClass.State);
        }
        void VMachineClass_Load(object sender, EventArgs e)
        {
            try
            {
                //加载已有表单规则
                ProgressFlash.Show("加载已有数据...");
                this.grdDtl.Rows.Count = 1;
                IList lstModel = MMachineClass.GetMachineClass();
                foreach (MachineClass item in lstModel.Cast<MachineClass>().OrderBy(MachineClass => MachineClass.Name))
                {
                    Row row = this.grdDtl.Rows.Add();
                    row["Code"] = item.Code;
                    row["Name"] = item.Name;
                    row["Kind"] = EnumUtil<enmWorkKind>.GetDescription(item.WorkKind);
                    row["State"] = item.State;
                    row.UserData = item;
                }
                this.grdDtl.AutoSerial();
                if (this.grdDtl.Rows.Count > 1)
                {
                    this.grdDtl.Select(1, 1, true);
                    this.grdModel_AfterSelChange(this.grdDtl, null);
                }

            }
            finally { ProgressFlash.Close(); }
        }

        private void txtPropName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
