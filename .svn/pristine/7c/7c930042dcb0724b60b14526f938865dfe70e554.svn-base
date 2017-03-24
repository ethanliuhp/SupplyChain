using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using System.Collections;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VSelFeeTemplateSelect :  TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        SelFeeTemplateMaster selectTemplate = null;
        List<BasicDataOptr> lst =null;
        public SelFeeTemplateMaster SelectTemplate
        {
            get {
                return selectTemplate;
            }
        }
        public VSelFeeTemplateSelect()
        {
            InitializeComponent();
            //lst = VBasicDataOptr.GetSelFeeTemplateSpecialType();
            IntEvent();
        }
        public void IntEvent()
        {
            this.btnQuery.Click+=new EventHandler(btnQuery_Click);
            this.btnSure.Click+=new EventHandler(btnSure_Click);
            this.btnCancel.Click+=new EventHandler(btnCancel_Click);
            this.gridMaster.SelectionChanged += new EventHandler(gridMaster_SelectionChanged);
        }

        public void btnQuery_Click(object sender, EventArgs e)
        {
            int iRow = 0;
            this.gridMaster.SelectionChanged -= new EventHandler(gridMaster_SelectionChanged);
            this.gridMaster.Rows.Clear();
            this.gridSelFeeDetial.Rows.Clear();
            this.gridSelFormula.Rows.Clear();
            try
            {
                ObjectQuery oQuery=new ObjectQuery ();
                if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
                {
                    oQuery.AddCriterion(Expression.Like("Code",txtCode.Text,MatchMode.Anywhere));
                }
                if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    oQuery.AddCriterion(Expression.Like("Name", txtCode.Text, MatchMode.Anywhere));
                }
              IList lst=  model.CostMonthAccSrv.QuerySelFeeTemplateMaster(oQuery);
              foreach (SelFeeTemplateMaster oMaster in lst)
              {
                  iRow = gridMaster.Rows.Add();
                  gridMaster[colName.Name, iRow].Value = oMaster.Name;
                  gridMaster[colCode.Name, iRow].Value = oMaster.Code;
                  gridMaster[this.colDescript.Name, iRow].Value = oMaster.Descript;
                  gridMaster.Rows[iRow].Tag = oMaster.Id;
              }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("查询异常:{0}", ExceptionUtil.ExceptionMessage(ex)));
            }
            finally
            {
                this.gridMaster.SelectionChanged += new EventHandler(gridMaster_SelectionChanged);
                if (gridMaster.Rows.Count > 0)
                {
                    gridMaster_SelectionChanged(null,null);
                }
            }
        }
        public void btnSure_Click(object sender, EventArgs e)
        {
            if (gridMaster.SelectedRows.Count > 0)
            {
                selectTemplate = gridMaster.SelectedRows[0].Tag as SelFeeTemplateMaster;
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择对应的取费模板");
            }
           
        }
        public void btnCancel_Click(object sender, EventArgs e)
        {
            selectTemplate = null;
            this.Close();
        }
        public void gridMaster_SelectionChanged(object sender, EventArgs e)
        {
            int iRow = 0;
            this.gridSelFeeDetial.Rows.Clear();
            this.gridSelFormula.Rows.Clear();
            if (gridMaster.SelectedRows.Count > 0)
            {
                if (gridMaster.SelectedRows[0].Tag is string)
                {
                    ObjectQuery oQuery=new ObjectQuery ();
                    oQuery.AddCriterion(Expression.Eq("Id",gridMaster.SelectedRows[0].Tag as string ));
                    gridMaster.SelectedRows[0].Tag = model.CostMonthAccSrv.GetSelFeeTemplateMaster(oQuery);
                }
                if (gridMaster.SelectedRows[0].Tag != null)
                {
                    //BasicDataOptr oBasicDataOptr = null;
                    SelFeeTemplateMaster oMaster= gridMaster.SelectedRows[0].Tag as SelFeeTemplateMaster;
                    foreach (SelFeeTemplateDtl oSelFeeTemplateDtl in oMaster.SelFeeTemplateDetails)
                    {
                       iRow= gridSelFeeDetial.Rows.Add();
                       //if (lst != null)
                       //{
                       //    oBasicDataOptr= lst.FirstOrDefault(a => a.BasicCode == oSelFeeTemplateDtl.SpecialType);
                       //    if (oBasicDataOptr != null)
                       //    {
                       //        gridSelFeeDetial[this.colSpecialType.Name, iRow].Value = oBasicDataOptr.BasicName;
                       //        oSelFeeTemplateDtl.TempData = oBasicDataOptr.BasicName;
                       //    }
                       //}
                       gridSelFeeDetial[this.colSpecialType.Name, iRow].Value = oSelFeeTemplateDtl.SpecialType;
                        gridSelFeeDetial[this.colAccountSubjectName.Name, iRow].Value = oSelFeeTemplateDtl.AccountSubjectName;
                       gridSelFeeDetial[this.colAccountSubjectCode.Name, iRow].Value = oSelFeeTemplateDtl.AccountSubjectCode;
                       gridSelFeeDetial[this.colRate.Name, iRow].Value = oSelFeeTemplateDtl.Rate;
                       gridSelFeeDetial[this.colBeginMoney.Name, iRow].Value = oSelFeeTemplateDtl.BeginMoney;
                       gridSelFeeDetial[ this.colEndMoney.Name, iRow].Value = oSelFeeTemplateDtl.EndMoney;
                       gridSelFeeDetial[ this.colMainAccSubjectCode.Name, iRow].Value = oSelFeeTemplateDtl.MainAccSubjectCode;
                       gridSelFeeDetial[this.colMainAccSubjectName.Name, iRow].Value = oSelFeeTemplateDtl.MainAccSubjectName;
                    }
                    foreach (SelFeeTempFormula oSelFeeTempFormula in oMaster.SelFeeTempFormulas)
                    {
                        iRow = this.gridSelFormula.Rows.Add();
                        gridSelFormula[this.colAccountSubjectCodeFormula.Name, iRow].Value = oSelFeeTempFormula.AccountSubjectCode;
                        gridSelFormula[this.colAccountSubjectNameFormula.Name, iRow].Value = oSelFeeTempFormula.AccountSubjectName;
                        gridSelFormula[this.colFormula.Name, iRow].Value = oSelFeeTempFormula.Formula;

                    }
                }
            }
        }
    }
}
