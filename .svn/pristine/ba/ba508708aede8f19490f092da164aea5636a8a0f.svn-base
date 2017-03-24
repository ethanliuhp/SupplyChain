using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCopyConfigSet : Form
    {
        MCostMonthAccount model = new MCostMonthAccount();
        string sCateTypeName = "资源分类";
        string sCheckTypeName = "核算科目";
        CurrentProjectInfo oProject = StaticMethod.GetProjectInfo();
        public VCopyConfigSet()
        {
            InitializeComponent();
            InitEvent();
            ShowProject();
        }
        public void InitEvent()
        {
            this.btnSearch .Click+=new EventHandler(btnSearch_Click);
            dtGdProject.SelectionChanged += new EventHandler(dtGdProject_SelectionChanged);
            this.btnCancel .Click +=new EventHandler(btnCancel_Click);
            this.btnCopy .Click+=new EventHandler(btnCopy_Click);
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
        }
        public void dtGdProject_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dtGdProject.Rows.Count > 0)
            {
                if (this.dtGdProject.SelectedRows.Count > 0)
                {
                    string sProjectID = this.dtGdProject.SelectedRows[0].Cells[colProjectID.Name].Value as string;
                    ShowMaterial(sProjectID);
                }
            }
        }
        public void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                IList listBill = new ArrayList();
                string sProjectName=string.Empty ;
                if (this.dtgdCategoryList.Rows.Count > 0 || this.dtgdCheckList.Rows.Count > 0)
                {
                    foreach (DataGridViewRow oRow in this.dtgdCategoryList.Rows)
                    {
                        if (oRow.Tag != null)
                        {
                            CostReporterConfig oCostReporterConfig = oRow.Tag as CostReporterConfig;
                            if (oCostReporterConfig != null)
                            {
                                CostReporterConfig CostReporterConfigTmp = new CostReporterConfig();
                                CostReporterConfigTmp.MaterialCategoryCode = oCostReporterConfig.MaterialCategoryCode;
                               sProjectName=oCostReporterConfig.ProjectName ;
                                CostReporterConfigTmp.CategoryType = oCostReporterConfig.CategoryType;
                                CostReporterConfigTmp.DisplayName = oCostReporterConfig.DisplayName;
                                CostReporterConfigTmp.MaterialCategoryID = oCostReporterConfig.MaterialCategoryID;
                                CostReporterConfigTmp.MaterialCategoryName = oCostReporterConfig.MaterialCategoryName;
                                CostReporterConfigTmp.OrderNo = oCostReporterConfig.OrderNo;
                                CostReporterConfigTmp.Path = oCostReporterConfig.Path;
                                CostReporterConfigTmp.Project = oProject;
                                CostReporterConfigTmp.ProjectName = oProject.Name;
                                listBill.Add(CostReporterConfigTmp);
                            }
                        }
                    }
                    foreach (DataGridViewRow oRow in this.dtgdCheckList.Rows)
                    {
                        if (oRow.Tag != null)
                        {
                            CostReporterConfig oCostReporterConfig = oRow.Tag as CostReporterConfig;
                            if (oCostReporterConfig != null)
                            {
                                CostReporterConfig CostReporterConfigTmp = new CostReporterConfig();
                                CostReporterConfigTmp.MaterialCategoryCode = oCostReporterConfig.MaterialCategoryCode;
                                 sProjectName=oCostReporterConfig.ProjectName ;
                                CostReporterConfigTmp.CategoryType = oCostReporterConfig.CategoryType;
                                CostReporterConfigTmp.DisplayName = oCostReporterConfig.DisplayName;
                                CostReporterConfigTmp.MaterialCategoryID = oCostReporterConfig.MaterialCategoryID;
                                CostReporterConfigTmp.MaterialCategoryName = oCostReporterConfig.MaterialCategoryName;
                                CostReporterConfigTmp.OrderNo = oCostReporterConfig.OrderNo;
                                CostReporterConfigTmp.Path = oCostReporterConfig.Path;
                                CostReporterConfigTmp.Project = oProject;
                                CostReporterConfigTmp.ProjectName = oProject.Name;
                                listBill.Add(CostReporterConfigTmp);
                            }
                        }
                    }

                }
                if (listBill.Count > 0)
                {
                    string sMsg = string.Empty;
                    sMsg = string.Format("你确定将[{0}]项目的配置复制到[{1}]项目",sProjectName ,oProject .Name );
                    if (MessageBox.Show(sMsg,"确认",  MessageBoxButtons.OKCancel )==DialogResult .OK )
                    {
                        model.CostMonthAccSrv.SaveCostReporterConfig(listBill, null);
                        MessageBox.Show("保存成功！");
                        this.Close();
                    }
                }
                else
                {
                    throw new Exception("配置集合为空，无法复制");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("复制失败:"+ex.Message );
            }
        }
        public void ShowProject()
        {
            this.dtGdProject .Rows.Clear ();
            DataTable oTable = model.CostMonthAccSrv.GetConfigSetProjectInfo();
            int iRow = 0;
            if (oTable != null && oTable.Rows.Count > 0)
            {
                foreach (DataRow oRow in oTable.Rows)
                {
                    iRow=this.dtGdProject.Rows.Add();
                    this.dtGdProject.Rows[iRow].Cells[colProjectID .Name  ].Value = ClientUtil.ToString(oRow["projectid"]);
                    this.dtGdProject.Rows[iRow].Cells[colProjectName.Name].Value = ClientUtil.ToString(oRow["projectname"]);
                }
            }
            if(this.dtGdProject .Rows .Count >0)
            {
                this.dtGdProject.Rows[0].Selected = true;
            }
        }
        public void ShowMaterial(string sProjectId)
        {
            
             Hashtable oHashTable = model.CostMonthAccSrv.GetConfigSet(sProjectId);
             
             IList list = oHashTable[sCheckTypeName] as IList;
             int iRow = 0;
             DataGridViewRow oRow = null;
             this.dtgdCheckList.Rows.Clear();
             if (list != null)
             {
                 foreach (CostReporterConfig oCostReporterConfig in list)
                 {
                    iRow= dtgdCheckList.Rows.Add();
                    oRow = dtgdCheckList.Rows[iRow];
                    oRow.Cells[colChkCategoryCode.Name].Value = oCostReporterConfig.MaterialCategoryCode;
                    oRow.Cells[colChkCategoryName.Name].Value = oCostReporterConfig.MaterialCategoryName;
                    oRow.Cells[colChkPath.Name].Value = oCostReporterConfig.Path;
                    oRow.Tag = oCostReporterConfig;
                 }
             }
             list = oHashTable[sCateTypeName] as IList;
             dtgdCategoryList.Rows.Clear();
             if (list != null)
             {
                 foreach (CostReporterConfig oCostReporterConfig in list)
                 {
                     iRow = dtgdCategoryList.Rows.Add();
                     oRow = dtgdCategoryList.Rows[iRow];
                     oRow.Cells[colCategoryCode.Name].Value = oCostReporterConfig.MaterialCategoryCode;
                     oRow.Cells[colCategoryName.Name].Value = oCostReporterConfig.MaterialCategoryName;
                     oRow.Cells[colPath.Name].Value = oCostReporterConfig.Path;
                     oRow.Tag = oCostReporterConfig;
                 }
             }
        }
        
    }
}
