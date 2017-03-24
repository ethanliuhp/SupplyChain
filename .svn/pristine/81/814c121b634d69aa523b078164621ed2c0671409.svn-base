using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;


namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public partial class VLaborSporadicPlanSearchCon : BasicUserControl
    {
        private MLaborSporadicMng model = new MLaborSporadicMng();
        private VLaborSporadicPlanSearchList searchList;
        EnumLaborType LaborType;
        public VLaborSporadicPlanSearchCon(VLaborSporadicPlanSearchList searchList, EnumLaborType Type)
        {
            this.searchList = searchList;
            InitializeComponent();
            LaborType = Type;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            this.InitEvent();
            btnOK.Focus();
        }

        private void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
            InitSporadicType();
            txtBuyer.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
        }

        private void InitSporadicType()
        {
            //派工类型
            //txtSporadicType.DataSource = (Enum.GetNames(typeof(LaborType)));
            if (LaborType == EnumLaborType.派工)
            {
                txtSporadicType.Items.AddRange(new object[] { "零星用工" });
            }
            if (LaborType == EnumLaborType.代工)
            {
                txtSporadicType.Items.AddRange(new object[] { "代工" });
            }
            if (LaborType == EnumLaborType.分包签证)
            {
                txtSporadicType.Items.Clear();
                txtSporadicType.Items.AddRange(new object[] { "分包签证" });
                txtSporadicType.Text = "分包签证";
            } if (LaborType == EnumLaborType.计时派工)
            {
                txtSporadicType.Items.AddRange(new object[] { "计时派工" });
            }

        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("IsCreate", 0));//是否复核
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if (txtBuyer.Value != "" && txtBuyer.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("BearTeamName", this.txtBuyer.Value));//根据名字查询承担队伍
            }
            if (txtSporadicType.SelectedItem != "" && txtSporadicType.SelectedItem != null)
            {
                oq.AddCriterion(Expression.Eq("LaborState", this.txtSporadicType.Text));//派工类型
            }
            else
            {
                if (LaborType == EnumLaborType.分包签证)
                {
                    oq.AddCriterion(Expression.Eq("LaborState", "分包签证"));//派工类型
                }
                else
                {
                    if (LaborType == EnumLaborType.派工)
                    {
                        oq.AddCriterion(Expression.Eq("LaborState", "零星用工"));//派工类型
                    }
                    if (LaborType == EnumLaborType.代工)
                    {
                        oq.AddCriterion(Expression.Eq("LaborState", "代工"));//派工类型
                    }
                    if (LaborType == EnumLaborType.计时派工)
                    {
                        oq.AddCriterion(Expression.Eq("LaborState", "计时派工"));//派工类型
                    }

                    if (LaborType == EnumLaborType.逐日派工)
                    {
                        oq.AddCriterion(Expression.Eq("LaborState", "逐日派工"));//派工类型
                    }
                }
            }
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddOrder(new Order("Code", false));
            IList list = model.LaborSporadicSrv.GetLaborSporadic(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
