using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VQuantitiesAffirmSearchCon : BasicUserControl
    {
        private MProjectTaskAccount model = new MProjectTaskAccount();
        private VQuantitiesAffirmSearchList searchList;

        public VQuantitiesAffirmSearchCon()
        {
            InitializeComponent();

            InitForm();
        }

        public VQuantitiesAffirmSearchCon(VQuantitiesAffirmSearchList searchList)
        {
            InitializeComponent();
            this.searchList = searchList;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            btnOK.Focus();
        }

        private void InitForm()
        {
            this.dtMadeBillStartDate.Value = ConstObject.LoginDate.AddDays(-7);
            InitialEvents();
        }

        private void InitialEvents()
        {
            btnOK.Click += new EventHandler(btnQuery_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            this.txtBillStartCode.TextChanged += new EventHandler(txtBillStartCode_TextChanged);
        }

        private void txtBillStartCode_TextChanged(object sender, EventArgs e)
        {
            this.txtBillEndCode.Text = this.txtBillStartCode.Text;
        }

        //查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string startCode = txtBillStartCode.Text.Trim();
                string endCode = txtBillEndCode.Text.Trim();

                DateTime madeStartDate = dtMadeBillStartDate.Value.Date;
                DateTime madeEndDate = dtMadeBillEndDate.Value.Date.AddDays(1).AddSeconds(-1);

                if (madeStartDate > madeEndDate)
                {
                    MessageBox.Show("制单起始日期不能大于结束日期！");
                    dtMadeBillEndDate.Focus();
                    return;
                }

                ObjectQuery oq = new ObjectQuery();
                //默认制单人和项目作为查询条件
                PersonInfo loginUser =
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (loginUser == null || string.IsNullOrEmpty(loginUser.Id))
                {
                    MessageBox.Show("未获取到登录用户信息，查询终止!");
                    return;
                }
                else if (projectInfo == null || string.IsNullOrEmpty(projectInfo.Id))
                {
                    MessageBox.Show("未获取到当前项目信息，查询终止!");
                    return;
                }
                oq.AddCriterion(Expression.Eq("AccountPersonGUID.Id", loginUser.Id));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

                if (startCode != "")
                    oq.AddCriterion(Expression.Ge("Code", startCode));
                if (endCode != "")
                    oq.AddCriterion(Expression.Le("Code", endCode));

                oq.AddCriterion(Expression.Ge("CreateDate", madeStartDate));
                oq.AddCriterion(Expression.Le("CreateDate", madeEndDate));


                IList list = model.ObjectQuery(typeof (ProjectTaskAccountBill), oq);

                searchList.RefreshData(list);
                this.btnOK.FindForm().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        //取消  
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
