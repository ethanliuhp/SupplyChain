using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCostReporterConfig : Form
    {
        public IList listCateBill = new ArrayList();
        public IList listCateMoveBill = new ArrayList();
        public IList listChkBill = new ArrayList();
        public IList listChkMoveBill = new ArrayList();
        MCostMonthAccount model = new MCostMonthAccount();
        string sCateTypeName = "资源分类";
        string sCheckTypeName = "核算科目";
        public VCostReporterConfig()
        {
           InitializeComponent();
           IntialEvent();
           SelectData();
           ShowCateData();
           ShowChkData();
           IntialControl();
        }
        public void IntialControl()
        {
            if (listCateBill.Count == 0 && listCateMoveBill.Count == 0 && listChkBill.Count == 0 && listChkMoveBill.Count == 0)
            {
                btnCopyConfig.Enabled = true;
            }
            else
            {
                btnCopyConfig.Enabled = false ;
            }
        }
        public void IntialEvent()
        {
            this.dtgdCategoryList .CellDoubleClick +=new DataGridViewCellEventHandler( dtgdCategoryList_CellDoubleClick); 
            this.dtgdCheckList .CellDoubleClick +=new DataGridViewCellEventHandler(dtgdCheckList_CellDoubleClick);
            this.btnSave .Click +=new EventHandler(btnSave_Click);
            this.btnClose.Click += new EventHandler(btnClose_Click);
            this.btnDelete .Click +=new EventHandler(btnDelete_Click);
            this.btnCopyConfig .Click +=new EventHandler(btnCopyConfig_Click);
        }
        public void btnCopyConfig_Click(object sender, EventArgs e)
        {
            VCopyConfigSet oVCopyConfigSet = new VCopyConfigSet();
            oVCopyConfigSet.ShowDialog();
            SelectData();
            ShowCateData();
            ShowChkData();
            IntialControl();
        }
        public void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void dtgdCheckList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (string.Equals(this.dtgdCheckList.Columns[e.ColumnIndex].Name, colChkCategoryCode.Name))
            {
                VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                frm.IsLeafSelect = false;
                frm.ShowDialog();
                string sPath = frm.Path;
                CostAccountSubject cost = frm.SelectAccountSubject;

                if (cost != null && ! IsSame (cost,sPath ))
                {
                    CostReporterConfig oCostReporterConfig = new CostReporterConfig();
                    oCostReporterConfig.MaterialCategoryID = cost.Id;
                    oCostReporterConfig.MaterialCategoryCode = cost.Code;
                    oCostReporterConfig.MaterialCategoryName = cost.Name;
                    oCostReporterConfig.DisplayName = cost.Name;
                    oCostReporterConfig.Project = StaticMethod.GetProjectInfo();
                    oCostReporterConfig.ProjectName = oCostReporterConfig.Project.Name;
                    oCostReporterConfig.CategoryType = sCheckTypeName;
                    oCostReporterConfig.Path = sPath;
                    oCostReporterConfig.TLevel = cost.Level;
                    oCostReporterConfig.OrderNo = 0;
                    oCostReporterConfig.ProjectName = oCostReporterConfig.Project.Name;
                    listChkBill.Add(oCostReporterConfig);
                    UpdateCheck();
                    ShowChkData();
                }
            }
        }

        public void dtgdCategoryList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(string.Equals (this.dtgdCategoryList .Columns[e.ColumnIndex].Name ,colCategoryCode .Name  ))
            {
                 VCommonMaterialCatSelect catSelect = new VCommonMaterialCatSelect();
                 IList list = catSelect.OpenSelectView(this,"01","") as IList ;
                 string sPath = catSelect.Path;
                 if (list != null && list.Count >0 )
                 {
                    MaterialCategory oMaterialCategory=list[0] as MaterialCategory;

                    if (oMaterialCategory != null && !IsExist(oMaterialCategory, sPath))
                    {
                        CostReporterConfig oCostReporterConfig = new CostReporterConfig();
                        oCostReporterConfig.MaterialCategoryID = oMaterialCategory.Id ;
                        oCostReporterConfig.MaterialCategoryCode = oMaterialCategory.Code;
                        oCostReporterConfig.MaterialCategoryName = oMaterialCategory.Name;
                        oCostReporterConfig.Project = StaticMethod.GetProjectInfo();
                        oCostReporterConfig.ProjectName = oCostReporterConfig.Project.Name;
                        oCostReporterConfig.CategoryType = sCateTypeName;
                        oCostReporterConfig.Path = sPath;
                        oCostReporterConfig.DisplayName = oMaterialCategory.Name;
                        oCostReporterConfig.TLevel = oMaterialCategory.Level;
                        oCostReporterConfig.OrderNo = 0;
                        oCostReporterConfig.ProjectName = oCostReporterConfig.Project.Name;
                        listCateBill.Add(oCostReporterConfig);
                        UpdateCate();
                        ShowCateData();
                    }
                 }

            }
        }

        public void SelectData()
        {
            CurrentProjectInfo oProject = StaticMethod.GetProjectInfo();
            if (oProject != null)
            {
                listCateBill = model.CostMonthAccSrv.QueryCostReporterConfig(oProject .Id , sCateTypeName);
                listChkBill = model.CostMonthAccSrv.QueryCostReporterConfig(oProject.Id, sCheckTypeName);
            }
        }
        public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {             
                if (this.tabPgCategory==this.tabControl1 .SelectedTab )
                {
                    UpdateCate();
                    listCateBill = model.CostMonthAccSrv.SaveCostReporterConfig(listCateBill, listCateMoveBill);
                    listCateMoveBill.Clear();
                    ShowCateData();
                }
                else if (this.tabPgCheck ==this.tabControl1 .SelectedTab )
                {
                    UpdateCheck();
                    listChkBill = model.CostMonthAccSrv.SaveCostReporterConfig(listChkBill, listChkMoveBill);
                    listChkMoveBill.Clear();
                    ShowChkData();
                }
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
            IntialControl();
        }
        public void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要删除此信息吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            int count = 0;
            if (this.tabControl1.SelectedTab == this.tabPgCategory)
            {
                for (int i = dtgdCategoryList.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtgdCategoryList.Rows[i].Tag != null && dtgdCategoryList.Rows[i].Cells[colSelect.Name].Value.ToString() == "1")
                    {
                        //CostReporterConfig oCostReporterConfig = dtgdCategoryList.Rows[i].Tag as CostReporterConfig;
                        for (int j = 0; j < listCateBill.Count; j++)
                        {
                            if (listCateBill[j] == dtgdCategoryList.Rows[i].Tag)
                            {
                                listCateMoveBill.Add(listCateBill[j]);
                                listCateBill.RemoveAt(j);
                                dtgdCategoryList.Rows.RemoveAt(i);
                                count++;
                                break;
                            }
                        }
                    }
                }
            }
            else if (this.tabControl1.SelectedTab == this.tabPgCheck)
            {
                for (int i = dtgdCheckList.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtgdCheckList.Rows[i].Tag != null && dtgdCheckList.Rows[i].Cells[colChkSelect.Name].Value.ToString() == "1")
                    {
                        //CostReporterConfig oCostReporterConfig = dtgdCategoryList.Rows[i].Tag as CostReporterConfig;
                        for (int j = 0; j < listChkBill.Count; j++)
                        {
                            if (listChkBill[j] == dtgdCheckList.Rows[i].Tag)
                            {
                                listChkMoveBill.Add(listChkBill[j]);
                                listChkBill.RemoveAt(j);
                                dtgdCheckList.Rows.RemoveAt(i);
                                count++;
                                break;
                            }
                        }
                    }
                }
            }

            if (count == 0)
            {
                MessageBox.Show("请选择要删除的记录！");
            }
        }

        public void ShowCateData()
        {
            this.dtgdCategoryList.Rows.Clear();
            if (this.listCateBill != null && this.listCateBill.Count > 0)
            {
                this.dtgdCategoryList.Rows.Add(listCateBill.Count);
                int iCount = 0;
                foreach (CostReporterConfig oCostReporterConfig in listCateBill)
                {
                    dtgdCategoryList.Rows[iCount].Cells[colCategoryCode.Name].Value = oCostReporterConfig.MaterialCategoryCode;
                    dtgdCategoryList.Rows[iCount].Cells[colCategoryName.Name].Value = oCostReporterConfig.MaterialCategoryName;
                    dtgdCategoryList.Rows[iCount].Cells[colCategoryType.Name].Value = oCostReporterConfig.CategoryType;
                    dtgdCategoryList.Rows[iCount].Cells[colOrderNo.Name].Value = oCostReporterConfig.OrderNo;
                    dtgdCategoryList.Rows[iCount].Cells[colProjectName.Name].Value = oCostReporterConfig.ProjectName;
                    dtgdCategoryList.Rows[iCount].Cells[colPath.Name ].Value = oCostReporterConfig.Path ;
                    dtgdCategoryList.Rows[iCount].Cells[this.colDisplayName.Name].Value = oCostReporterConfig.DisplayName;
                    dtgdCategoryList.Rows[iCount].Tag = oCostReporterConfig;
                    dtgdCategoryList.Rows[iCount].Cells[colSelect.Name].Value = false;
                    iCount++;
                }
            }
            //dtgdCategoryList.Rows.Add(1);
            dtgdCategoryList.Rows[dtgdCategoryList.Rows.Count - 1].Cells[colSelect.Name].Value = false;
            listCateMoveBill.Clear();
        }
        public void ShowChkData()
        {
           
            this.dtgdCheckList .Rows.Clear();
            if (this.listChkBill != null && this.listChkBill.Count > 0)
            {
                this.dtgdCheckList.Rows.Add(listChkBill.Count);
                int iCount = 0;
                foreach (CostReporterConfig oCostReporterConfig in listChkBill)
                {
                    dtgdCheckList.Rows[iCount].Cells[colChkCategoryCode.Name].Value = oCostReporterConfig.MaterialCategoryCode;
                    dtgdCheckList.Rows[iCount].Cells[colChkCategoryName.Name].Value = oCostReporterConfig.MaterialCategoryName;
                    dtgdCheckList.Rows[iCount].Cells[colChkCategoryType.Name].Value = oCostReporterConfig.CategoryType;
                    dtgdCheckList.Rows[iCount].Cells[colChkOrderNo.Name].Value = oCostReporterConfig.OrderNo;
                    dtgdCheckList.Rows[iCount].Cells[colChkProjectName.Name].Value = oCostReporterConfig.ProjectName;
                    dtgdCheckList.Rows[iCount].Cells[colChkPath.Name].Value = oCostReporterConfig.Path;
                    dtgdCheckList.Rows[iCount].Cells[this.colChkDisplayName.Name].Value = oCostReporterConfig.DisplayName;
                    dtgdCheckList.Rows[iCount].Tag = oCostReporterConfig;
                    dtgdCheckList.Rows[iCount].Cells[colChkSelect.Name].Value = false;
                    iCount++;
                }
            }
            //dtgdCheckList.Rows.Add(1);
            dtgdCheckList.Rows[dtgdCheckList.Rows.Count - 1].Cells[colChkSelect.Name].Value = false;
            listCateMoveBill.Clear();
        }
        public bool IsExist(MaterialCategory oMaterialCategory, string sPath)
        {
            bool bFlag = false;
            string sMsg = string.Empty;
            CostReporterConfig oCostReporterConfig = null;
            if (!string.IsNullOrEmpty(sPath)  )
            {
                for (int i = 0; i < dtgdCategoryList.Rows.Count; i++)
                {
                    oCostReporterConfig = dtgdCategoryList.Rows[i].Tag as CostReporterConfig;
                    if (oCostReporterConfig != null &&  !string.IsNullOrEmpty (oCostReporterConfig.Path)  )
                    {
                         
                        if (string.Equals(oCostReporterConfig.Path, sPath))
                        {
                            bFlag =true ;
                            sMsg = string.Format("请不要添加重复的物资：[{0}] 已经添加。", sPath);
                            dtgdCategoryList.Rows[i].Selected=true ;
                            
                            break;
                        }
                        else if (oCostReporterConfig.Path.StartsWith(sPath))
                        {
                            bFlag = true;
                            sMsg = string.Format("请不要添加包含关系的物资：[{1}] 已经添加，它包含于[{0}]。", oMaterialCategory.Name, oCostReporterConfig.MaterialCategoryName );
                            dtgdCategoryList.Rows[i].Selected = true;
                            
                            break;
                        }
                        else if (sPath.StartsWith(oCostReporterConfig.Path ))
                        {
                            bFlag = true;
                            sMsg = string.Format("请不要添加包含关系的物资：[{1}] 已经添加，它包含[{0}]。", oMaterialCategory.Name, oCostReporterConfig.MaterialCategoryName );
                            dtgdCategoryList.Rows[i].Selected = true;
                           
                            break;
                        }                   
                    }
                }
            }
            if(bFlag  && !string .IsNullOrEmpty ( sMsg))
            {
                 MessageBox.Show(sMsg);
            }
            return bFlag ;
        }
        public bool IsSame(CostAccountSubject cost, string sPath)
        {
            bool bFlag = false;
            CostReporterConfig oCostReporterConfig = null;
            string sMsg = string.Empty;
            if (cost != null && !string.IsNullOrEmpty(sPath))
            {
                for (int i = 0; i < dtgdCheckList.Rows.Count; i++)
                {
                    oCostReporterConfig = dtgdCheckList.Rows[i].Tag as CostReporterConfig;
                    if (oCostReporterConfig != null && !string.IsNullOrEmpty(oCostReporterConfig.Path))
                    {
                        if (string.Equals(sPath, oCostReporterConfig.Path))
                        {
                            bFlag = true;
                            sMsg = string.Format("请不要添加重复的科目：[{0}] 已经添加。", sPath);
                            dtgdCheckList.Rows[i].Selected = true;

                            break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sMsg))
            {
                MessageBox.Show(sMsg);
            }
            return bFlag;
        }
        public void UpdateCate()
        {
            for (int j = 0; j < dtgdCategoryList.Rows.Count; j++)
            {
                DataGridViewRow oRow = dtgdCategoryList.Rows[j];
                if (oRow.Tag != null)
                {
                    for (int i = 0; i < listCateBill.Count; i++)
                    {
                        if (object.Equals(oRow.Tag, listCateBill[i]))
                        {
                            CostReporterConfig oCostReporterConfig = listCateBill[i] as CostReporterConfig;
                            oCostReporterConfig.OrderNo = ClientUtil.ToInt(oRow.Cells[colOrderNo.Name].Value.ToString());
                            oCostReporterConfig.DisplayName = ClientUtil.ToString(oRow.Cells[this.colDisplayName.Name].Value);
                        }
                    }
                }
            }
        }
        public void UpdateCheck()
        {
            for (int j = 0; j < dtgdCheckList.Rows.Count; j++)
            {
                DataGridViewRow oRow = dtgdCheckList.Rows[j];
                for (int i = 0; i < listChkBill.Count; i++)
                {
                    if (object.Equals(oRow.Tag, listChkBill[i]))
                    {
                        CostReporterConfig oCostReporterConfig = listChkBill[i] as CostReporterConfig;
                        oCostReporterConfig.OrderNo = ClientUtil.ToInt(oRow.Cells[colChkOrderNo.Name].Value.ToString());
                        oCostReporterConfig.DisplayName = ClientUtil.ToString(oRow.Cells[this.colChkDisplayName.Name].Value);
                    }
                }
            }
        }
    }
}
