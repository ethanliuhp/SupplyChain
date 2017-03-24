using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng
{
    public partial class VDangerTypeSelector : TBasicDataView
    {
        private MProInsRecordMng model = new MProInsRecordMng();
        private string result = null;
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public string Result
        {
            get { return result; }
            set { result = value; }
        }
        public VDangerTypeSelector(string strType)
        {
            InitializeComponent();
            result = strType;
            InitEvent();
            InitData(result);
        }        

        private void InitData(string strDangerType)
        {
            //表格中加载隐患类型信息
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATANAME_SAFTY);
            if (list.Count > 0)
            {
                foreach (BasicDataOptr bdo in list)
                {
                    string strName = bdo.BasicName;
                    int i = this.dgMaster.Rows.Add();
                    dgMaster[colDangerType.Name, i].Value = strName;
                }
            }
            //将传过来的值显示在表格中
            string[] sArray = null;
            if (result != null || result != "")
            {
                //截取字符串以/为界限
                sArray = result.Split('/');
            }
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                for(int i = 0;i < sArray.Length;i++)
                {
                    if (var.Cells[colDangerType.Name].Value.ToString() == sArray[i].ToString().Trim())
                    {
                        var.Cells[colSelect.Name].Value = true;
                    }
                }
            }
        }

        private void InitEvent()
        {
            this.btnOK.Click +=new EventHandler(btnOK_Click);
            this.btnCancel.Click +=new EventHandler(btnCancel_Click);
        }

        void btnOK_Click(object sender,EventArgs e)
        {
            //遍历dgmaster查找选中的信息
            string strNewType = null;
            foreach (DataGridViewRow var in this.dgMaster.Rows)
            {
                if (var.Cells[colSelect.Name].Value != null)
                {
                    if ((bool)var.Cells[colSelect.Name].Value)
                    {
                        strNewType += var.Cells[colDangerType.Name].Value.ToString() + "/";
                    }
                }
            }
            result = strNewType.Substring(0,strNewType.Length - 1);
            this.FindForm().Close();
        }
        void btnCancel_Click(object sender,EventArgs e)
        {
            this.FindForm().Close();
        }
    }
}
