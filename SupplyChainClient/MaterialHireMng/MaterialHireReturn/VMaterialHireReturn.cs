using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using IRPServiceModel.Domain.Document;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.FileUpload;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.IO;
using IRPServiceModel.Basic;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireLedger;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection;
using System.Text.RegularExpressions;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn
{
    public partial class VMaterialHireReturn : TMasterDetailView
    {
        private MMaterialHireMng model = new MMaterialHireMng();
        private MatHireReturnMaster Master;
        private const string ConstRepairCostType ="维修费用";
        private string[] arrLossType = { "报损", "消耗" ,"赔偿"};
        private string[] arrReadOnlyType = { ConstRepairCostType };
        private string[] arrReturnMustCostType = { ConstRepairCostType };

        bool IsLoss=false;
        EnumMatHireType MatHireType;
        private bool CanReturnUp = true;//允许退超
        //CurrentProjectInfo ProjectInfo;
    
        private MatHireOrderMaster OrderMaster;
        List<CostColumnInfo> lstCostColumnInfo = new List<CostColumnInfo>();
        private bool IsProject = false;


        
        //IList MatRenLed_list = null;
        //IList MatReturnDetSeq_list = null;
        //IList list_MatReturnDtlSeq = null;
        ////非数量计费
        //IList list_index_detail = new ArrayList();
        //IList list_costType_detail = new ArrayList();
        ////按数量计费
        //IList list_index_costDetail = new ArrayList();
        //IList list_costType_costDetail = new ArrayList();

               //type  1:料具退料单、 2：料具退料单(损耗)

        public VMaterialHireReturn(bool IsLoss, EnumMatHireType matHireType)
        {
            InitializeComponent();
            this.IsLoss = IsLoss;
            this.MatHireType = matHireType;
            this.InitData( );
            this.InitEvent();
        }

        private void InitData( )
        {
            txtBalRule.DataSource = (Enum.GetNames(typeof(EnumMaterialHireMngBalRule)));
            this.lblOneLen.Visible = this.txtOneLen.Visible = false;
            if (!IsLoss)
            {
                this.dgDetail.Columns[colLossQty.Name].Visible = false;
                this.dgDetail.Columns[colConsumeQuantity.Name].Visible = false;
                colDisCardQty.Visible = this.MatHireType == EnumMatHireType.钢管;
            }
            else  
            {
                this.dgDetail.Columns[colBroachQuantity.Name].Visible = false;
                this.dgDetail.Columns[colDisCardQty.Name].Visible = false;
                this.dgDetail.Columns[colRepairQty.Name].Visible = false;
                colDisCardQty.Visible = false;
            }
            if (this.MatHireType == EnumMatHireType.钢管)
            {
                this.colLength.Visible = true;
                this.colType.Visible = false;
                this.lblOneLen.Visible = this.txtOneLen.Visible = true;
               
            }
            else if (this.MatHireType == EnumMatHireType.普通料具)
            {
                this.colLength.Visible = false;
                this.colType.Visible = false;
            }
            else if (this.MatHireType == EnumMatHireType.碗扣)
            {
                this.colLength.Visible = false;
                this.colType.Visible = true;
               // this.colType.Items.AddRange(VMaterialHireCollection.arrWKType);
            }
            #region 是否使项目
            this.colBorrowUnit.Visible = IsProject;
            this.colSubject.Visible = IsProject;
            this.colUsedPart.Visible = IsProject;
         
            this.colUsedPart.Visible = IsProject;
            this.colSubject.Visible = IsProject;
            this.colBorrowUnit.Visible = IsProject;
            this.customLabel2.Visible = this.txtTransChagre.Visible = IsProject;
            #endregion
        }

        private void InitEvent()
        {
            btnForward.Click+=new EventHandler(btnForward_Click);
            txtOneLen.tbTextChanged+=new EventHandler(txtOneLen_tbTextChanged);
            //txtSupply.TextChanged += new EventHandler(txtSupply_TextChanged);
            //this.TenantSelector.TenantSelectorAfterEvent += new UcTenantSelector.TenantSelectorAfterEventHandler(TenantSelectorAfter);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_RowHeaderMouseClick);
            this.dgDelete.Click += new EventHandler(dgDelete_Click);
       
            txtTransChagre.tbTextChanged += new EventHandler(txtTransChagre_tbTextChanged);
             
        }

        //void TenantSelectorAfter(UcTenantSelector a)
        //{
        //    this.ProjectInfo = a.SelectedProject;
        //    if (Master != null)
        //    {
        //        Master.ProjectId = ProjectInfo == null ? "" : ProjectInfo.Id;
        //        Master.ProjectName = ProjectInfo == null ? "" : ProjectInfo.Name;
        //    }
        //    //if (this.ProjectInfo != null && txtSupply.Result != null && txtSupply.Result.Count > 0)
        //    //{
        //    //    txtSupply_TextChanged(null, null);
        //    //}
        //}
        void txtOneLen_tbTextChanged(object sender, EventArgs e)
        {
            if (!CommonMethod.VeryValid(txtOneLen.Text))
            {
                ShowMessage("请输入数字");
                txtOneLen.Focus();
            }
        }
        void btnForward_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataGridViewRow oRow = null;
            MatHireOrderMaster oTempOrderMaster = null;
            string[] arrMaterialIDs;
            VMaterialHireOrderSelectMaterial oVMaterialHireOrderSelectMaterial = new VMaterialHireOrderSelectMaterial(this.MatHireType);
            oVMaterialHireOrderSelectMaterial.ShowDialog();
            if (oVMaterialHireOrderSelectMaterial.Result != null && oVMaterialHireOrderSelectMaterial.Result.Count > 0)
            {
                oTempOrderMaster = oVMaterialHireOrderSelectMaterial.Result[0] as MatHireOrderMaster;
                if (oTempOrderMaster.TempData.OfType<MatHireOrderDetail>().FirstOrDefault(a => a.IsSelect) != null)
                {
                    #region 数据填充
                    #region 主表数据填充
                    txtContract.Text = oTempOrderMaster.Code;
                    txtContract.Tag = oTempOrderMaster;
                    txtProjectName.Tag = oTempOrderMaster.ProjectId;
                    txtProjectName.Text = oTempOrderMaster.ProjectName;
                    txtSupply.Tag = oTempOrderMaster.TheSupplierRelationInfo;
                    txtSupply.Text = oTempOrderMaster.SupplierName;
                    txtBalRule.Text = oTempOrderMaster.BalRule;
                    txtContractNo.Text = oTempOrderMaster.OriginalContractNo;
                   
                    #endregion
                    #region 明细数据填充
                    if (MatHireType == EnumMatHireType.普通料具)
                    {
                        if (OrderMaster == null || OrderMaster.Id != oTempOrderMaster.Id)//合同不相同清除费用列
                        {
                            OrderMaster = oTempOrderMaster;
                            dgDetail.Rows.Clear();
                            //清除以前的列
                            foreach (CostColumnInfo oColumnInfo in this.lstCostColumnInfo)
                            {
                                dgDetail.Columns.Remove(oColumnInfo.CostType);
                            }
                            lstCostColumnInfo.Clear();
                            RefleshColumn(null);
                        }
                        else
                        {
                            OrderMaster = oTempOrderMaster;
                            RefleshColumn(null);
                        }
                        arrMaterialIDs = dgDetail.Rows.OfType<DataGridViewRow>().Where(a=>a.IsNewRow==false ).Select(a =>ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value)).ToArray();
                        #region 明细
                        foreach (MatHireOrderDetail oOrderDetail in OrderMaster.TempData)
                        {
                            if (!oOrderDetail.IsSelect) continue;//选中合同明细
                            //不出现重复物质行
                            if (arrMaterialIDs.Length > 0 && arrMaterialIDs.Contains(oOrderDetail.MaterialCode))//(dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value) == oOrderDetail.MaterialCode) != null)
                            {
                                continue;
                            }
                            i = this.dgDetail.Rows.Add();
                            oRow = this.dgDetail.Rows[i];
                            oRow.Cells[colMaterialCode.Name].Tag = oOrderDetail.MaterialResource;
                            oRow.Cells[colMaterialCode.Name].Value = oOrderDetail.MaterialCode;
                            oRow.Cells[colMaterialName.Name].Value = oOrderDetail.MaterialName;
                            oRow.Cells[colMaterialSpec.Name].Value = oOrderDetail.MaterialSpec;
                            oRow.Cells[colUnit.Name].Tag = oOrderDetail.MatStandardUnit;
                            oRow.Cells[colUnit.Name].Value = oOrderDetail.MatStandardUnitName;
                            oRow.Cells[colPrice.Name].Value = oOrderDetail.Price;
                            oRow.Cells[colPrice.Name].Tag = oOrderDetail;
                            oRow.Cells[colLength.Name].Value = 1;
                            oRow.Cells[colCategoryQuantity.Name].Value =ClientUtil.ToDecimal( oOrderDetail.TempData3).ToString("N2");
                        }
                        #endregion
                    }
                    else
                    {
                        MatHireOrderDetail oMatHireOrderDetail = oTempOrderMaster.TempData.OfType<MatHireOrderDetail>().FirstOrDefault(a => a.IsSelect);
                        if (OrderMaster != oTempOrderMaster || ClientUtil.ToString(dgDetail[colMaterialCode.Name, 0].Value) != oMatHireOrderDetail.MaterialCode)//合同不相同或者相同合同不同物资 清除费用列
                        {
                            OrderMaster = oTempOrderMaster;
                            dgDetail.Rows.Clear();
                            //清除以前的列
                            foreach (CostColumnInfo oColumnInfo in this.lstCostColumnInfo)
                            {
                                dgDetail.Columns.Remove(oColumnInfo.CostType);
                            }
                            lstCostColumnInfo.Clear();
                            RefleshColumn(oMatHireOrderDetail);
                            #region 明细
                            AddRow(oMatHireOrderDetail);
                            #endregion
                        }
                    }
                  
                    #endregion
                    #endregion
                }
            }
        }
        public void RefleshColumn(MatHireOrderDetail oMatHireOrderDetail)
        {
            int iInsertColumn = GetInsertColumnIndex();
            IEnumerable<OrderMasterCostSetItem> lstOrderMasterCostSetItem = null;
            IEnumerable<OrderDetailCostSetItem> lstOrderDetailCostSetItem = null;
            DataGridViewTextBoxColumn oColumn = null;
            CostColumnInfo oCostColumnInfo = null;
            Hashtable hsCostType =  new Hashtable();
            #region 动态插入列
            
                IList list_temp = new ArrayList();
               
                if (OrderMaster != null)
                {
                    if (string.IsNullOrEmpty(Master.Id))
                    {
                        if (oMatHireOrderDetail == null)//这是普通
                        {
                            #region
                            //lstOrderMasterCostSetItem = IsLoss ?
                            //   OrderMaster.BasiCostSets.Where(a => a.ExitCalculation == 1 && arrLossType.Contains(a.MatCostType)) :
                            //   OrderMaster.BasiCostSets.Where(a => a.ExitCalculation == 1 && !arrLossType.Contains(a.MatCostType));
                            //foreach (OrderMasterCostSetItem costSet in lstOrderMasterCostSetItem)
                            //{
                            //    oColumn = new DataGridViewTextBoxColumn();
                            //    oColumn.HeaderText = costSet.MatCostType;
                            //    oColumn.Name = costSet.MatCostType;
                            //    oColumn.ReadOnly = arrReadOnlyType.Contains(costSet.MatCostType);
                            //    dgDetail.Columns.Insert(iInsertColumn + lstCostColumnInfo.Count, oColumn);
                            //    oCostColumnInfo = new CostColumnInfo()
                            //    {
                            //        ColumnIndex = oColumn.Index,
                            //        ColumnName = costSet.MatCostType,
                            //        CostType = costSet.MatCostType
                            //    };
                            //    lstCostColumnInfo.Add(oCostColumnInfo);
                            //}
                            #endregion
                            hsCostType = new Hashtable();
                            foreach (CostColumnInfo oCostColumnInfoTemp in lstCostColumnInfo)
                            {
                                hsCostType.Add(oCostColumnInfoTemp.CostType, "");
                            }
                            foreach (MatHireOrderDetail oOrderDetailTemp in OrderMaster.Details)
                            {
                                if (oOrderDetailTemp.IsSelect)
                                {
                                    foreach (OrderDetailCostSetItem oOrderDetailCostSetItem in oOrderDetailTemp.BasicDtlCostSets)
                                    {
                                        if (string.IsNullOrEmpty(oOrderDetailCostSetItem.ExitExpression) || string.IsNullOrEmpty(oOrderDetailCostSetItem.ExitExpression.Trim())) continue;
                                         
                                            if (IsLoss && string.IsNullOrEmpty(arrLossType.FirstOrDefault(a=> oOrderDetailCostSetItem.CostType.Contains(a))))  continue;
                                                if(!IsLoss &&!string.IsNullOrEmpty( arrLossType.FirstOrDefault(a=>oOrderDetailCostSetItem.CostType.Contains(a)))) continue;

                                            if (!hsCostType.ContainsKey(oOrderDetailCostSetItem.CostType))
                                            {
                                                hsCostType.Add(oOrderDetailCostSetItem.CostType,null);
                                                oColumn = new DataGridViewTextBoxColumn();
                                                oColumn.DefaultCellStyle.Format = "N6";
                                                oColumn.HeaderText = oOrderDetailCostSetItem.CostType;
                                                oColumn.Name = oOrderDetailCostSetItem.CostType;
                                                oColumn.ReadOnly = true; arrReadOnlyType.Contains(oOrderDetailCostSetItem.CostType);
                                                oColumn.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                                                dgDetail.Columns.Insert(iInsertColumn , oColumn);
                                                oCostColumnInfo = new CostColumnInfo()
                                                {
                                                    ColumnIndex = oColumn.Index,
                                                    ColumnName = oOrderDetailCostSetItem.CostType,
                                                    CostType = oOrderDetailCostSetItem.CostType
                                                };
                                                lstCostColumnInfo.Add(oCostColumnInfo);
                                            }
                                         
                                    }
                                }
                            }
                        }
                        else//钢管
                        {
                            //lstOrderDetailCostSetItem = IsLoss ?
                            //    oMatHireOrderDetail.BasicDtlCostSets.Where(a => !string.IsNullOrEmpty(a.ExitExpression) && !string.IsNullOrEmpty(a.ExitExpression.Trim()) && arrLossType.Contains(a.CostType) :
                            //    oMatHireOrderDetail.BasicDtlCostSets.Where(a => !string.IsNullOrEmpty(a.ExitExpression) && !string.IsNullOrEmpty(a.ExitExpression.Trim()) && !arrLossType.Contains(a.CostType));
                            foreach (OrderDetailCostSetItem costSet in oMatHireOrderDetail.BasicDtlCostSets)
                            {
                                if (string.IsNullOrEmpty(costSet.ExitExpression) || string.IsNullOrEmpty(costSet.ExitExpression.Trim())) continue;
                                if (IsLoss && string.IsNullOrEmpty(arrLossType.FirstOrDefault(a => costSet.ExitExpression.Contains(a)))) continue;
                                if (!IsLoss && !string.IsNullOrEmpty(arrLossType.FirstOrDefault(a => costSet.ExitExpression.Contains(a)))) continue;
                                oColumn = new DataGridViewTextBoxColumn();
                                oColumn.DefaultCellStyle.Format = "N6";
                                oColumn.HeaderText = costSet.CostType;
                                oColumn.Name = costSet.CostType;
                                oColumn.ReadOnly = true;// arrReadOnlyType.Contains(costSet.CostType);
                                oColumn.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                                dgDetail.Columns.Insert(iInsertColumn , oColumn);
                                oCostColumnInfo = new CostColumnInfo()
                                {
                                    ColumnIndex = oColumn.Index,
                                    ColumnName = costSet.CostType,
                                    CostType = costSet.CostType
                                };
                                lstCostColumnInfo.Add(oCostColumnInfo);
                            }
                        }
                        foreach (string sCostType in arrReturnMustCostType)//解决必须添加的费用列
                        {
                            if (IsLoss && !arrLossType.Contains(sCostType)) continue;
                            if (lstCostColumnInfo.FirstOrDefault(a => a.ColumnName == sCostType) == null)
                            {
                                oColumn = new DataGridViewTextBoxColumn();
                                oColumn.DefaultCellStyle.Format = "N6";
                                oColumn.HeaderText = sCostType;
                                oColumn.Name = sCostType;
                                oColumn.ReadOnly = true;//  arrReadOnlyType.Contains(sCostType);
                                oColumn.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                                dgDetail.Columns.Insert(iInsertColumn + lstCostColumnInfo.Count, oColumn);
                                oCostColumnInfo = new CostColumnInfo()
                                {
                                    ColumnIndex = oColumn.Index,
                                    ColumnName =sCostType,
                                    CostType = sCostType
                                };
                                lstCostColumnInfo.Add(oCostColumnInfo);
                            }
                        }
                    }
                    else
                    {
                       
                        foreach (MatHireReturnDetail theMaterialReturnDetail in Master.Details)
                        {
                            foreach (MatHireReturnCostDtl theMaterialReturnCostDtl in theMaterialReturnDetail.MatReturnCostDtls)
                            {
                                if (!hsCostType.ContainsKey(theMaterialReturnCostDtl.CostType))
                                {
                                    hsCostType.Add(theMaterialReturnCostDtl.CostType, null);
                                }
                            }
                        }
                        foreach (string key in hsCostType.Keys)
                        {
                            oColumn = new DataGridViewTextBoxColumn();
                            oColumn.DefaultCellStyle.Format = "N6";
                            oColumn.HeaderText = key;
                            oColumn.Name = key;
                            oColumn.ReadOnly = true;// arrReadOnlyType.Contains(key);
                            oColumn.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                            dgDetail.Columns.Insert(iInsertColumn + lstCostColumnInfo.Count, oColumn);
                            oCostColumnInfo = new CostColumnInfo()
                            {
                                ColumnIndex = oColumn.Index,
                                ColumnName = key,
                                CostType = key
                            };
                            lstCostColumnInfo.Add(oCostColumnInfo);
                        }
                    }
                }
         
            #endregion
        }
        public int GetInsertColumnIndex()
        {
           
            int iIndex=colConsumeQuantity.Index+1 ;
            foreach (CostColumnInfo oCostColumnInfo in lstCostColumnInfo)
            {
                if ( oCostColumnInfo.CostType==ConstRepairCostType)
                {
                    iIndex = dgDetail.Columns[ConstRepairCostType].Index;
                }
            }
            return iIndex;
        }
        void AddRow(MatHireOrderDetail oOrderDetail)
        {
            dgDetail.Rows.Clear();
            if (MatHireType == EnumMatHireType.钢管)
            {
                AddGGRow(oOrderDetail);
            }
            else if (MatHireType == EnumMatHireType.碗扣)
            {
                AddWKRow(oOrderDetail);
            }
        }
        void AddWKRow(MatHireOrderDetail oOrderDetail)
        {
            decimal dLen = 0;
            DataGridViewRow oRow = null;
            int i = 0;
            foreach (string sType in VMaterialHireCollection.arrWKType)
            {
                i = this.dgDetail.Rows.Add();
                this.dgDetail[colMaterialCode.Name, i].Value = oOrderDetail.MaterialCode;
                this.dgDetail[colMaterialCode.Name, i].Tag = oOrderDetail.MaterialResource;
                this.dgDetail[colMaterialName.Name, i].Value = oOrderDetail.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = oOrderDetail.MaterialSpec;
                dLen = GetLength(sType);
                this.dgDetail[this.colLength.Name, i].Value = dLen;
                this.dgDetail[colCategoryQuantity.Name, i].Value =Math.Round( ClientUtil.ToDecimal(oOrderDetail.TempData3) / dLen,2).ToString("N2");
                this.dgDetail[colUnit.Name, i].Value = oOrderDetail.MatStandardUnitName;
                this.dgDetail[colUnit.Name, i].Tag = oOrderDetail.MatStandardUnit;
                this.dgDetail[colPrice.Name, i].Value = oOrderDetail.Price;// master.RentalPrice;
                this.dgDetail[colPrice.Name, i].Tag = oOrderDetail;
                this.dgDetail[this.colType.Name, i].Value = sType;
                

            }
        }
        void AddGGRow(MatHireOrderDetail oOrderDetail )
        {
            decimal iLen = 0;
            DataGridViewRow oRow = null;
            int i = 0;
            for (decimal j = 60; j >= 10; j--)
            {
                i = this.dgDetail.Rows.Add();
                this.dgDetail[colMaterialCode.Name, i].Value = oOrderDetail.MaterialCode;
                this.dgDetail[colMaterialCode.Name, i].Tag = oOrderDetail.MaterialResource;
                this.dgDetail[colMaterialName.Name, i].Value = oOrderDetail.MaterialName;
                this.dgDetail[colMaterialSpec.Name, i].Value = oOrderDetail.MaterialSpec;
                iLen = j / 10;
                this.dgDetail[this.colLength.Name, i].Value = iLen;
                this.dgDetail[colCategoryQuantity.Name, i].Value = Math.Round(ClientUtil.ToDecimal(oOrderDetail.TempData3) /iLen, 2).ToString("N2");
                this.dgDetail[colUnit.Name, i].Value = oOrderDetail.MatStandardUnitName;
                this.dgDetail[colUnit.Name, i].Tag = oOrderDetail.MatStandardUnit;
                this.dgDetail[colPrice.Name, i].Value = oOrderDetail.Price;// master.RentalPrice;
                this.dgDetail[colPrice.Name, i].Tag = oOrderDetail;
                //oRow.Cells[colLength.Name].Value = string.Format("{0:N1}", j / 10);

            }
        }
        //void txtSupply_TextChanged(object sender, EventArgs e)
        //{
        //    IEnumerable<OrderMasterCostSetItem> lstOrderMasterCostSetItem = null;
        //    DataGridViewTextBoxColumn oColumn = null;
        //    CostColumnInfo oCostColumnInfo = null;
        //    Hashtable hsCostType = null;
        //    if (txtSupply.Result!=null && txtSupply.Result.Count > 0 && this.ProjectInfo != null)
        //    {
        //        #region 动态插入列
        //        MatHireOrderMaster theOrderMaster = GetOrderMaster();
        //        if (theOrderMaster != null)
        //        {
                   
        //            OrderMaster = theOrderMaster;
        //            IList list_temp = new ArrayList();
        //            txtBalRule.Text = OrderMaster.BalRule;
        //            txtContractNo.Text = OrderMaster.OriginalContractNo;

        //            //清除以前的列
        //            foreach (CostColumnInfo oColumnInfo in this.lstCostColumnInfo)
        //            {
        //                dgDetail.Columns.Remove(oColumnInfo.CostType);
        //            }
        //            lstCostColumnInfo.Clear();
        //            if (string.IsNullOrEmpty(Master.Id))
        //            {
        //                lstOrderMasterCostSetItem = IsLoss ?
        //                    OrderMaster.BasiCostSets.Where(a => a.ExitCalculation==1 && arrLossType.Contains(a.MatCostType)) :
        //                    OrderMaster.BasiCostSets.Where(a => a.ExitCalculation == 1 && !arrLossType.Contains(a.MatCostType));
        //                foreach (OrderMasterCostSetItem costSet in lstOrderMasterCostSetItem)
        //                {
        //                    oColumn = new DataGridViewTextBoxColumn();
        //                    oColumn.HeaderText = costSet.MatCostType;
        //                    oColumn.Name = costSet.MatCostType;
        //                    oColumn.ReadOnly = arrReadOnlyType.Contains(costSet.MatCostType);
        //                    dgDetail.Columns.Insert(colSubject.Index, oColumn);
        //                    oCostColumnInfo = new CostColumnInfo()
        //                    {
        //                        ColumnIndex = oColumn.Index,
        //                        ColumnName = costSet.MatCostType,
        //                        CostType = costSet.MatCostType
        //                    };
        //                    lstCostColumnInfo.Add(oCostColumnInfo);
        //                }
        //            }
        //            else
        //            {
        //                hsCostType = new Hashtable();
        //                foreach (MatHireReturnDetail theMaterialReturnDetail in Master.Details)
        //                {
        //                    foreach (MatHireReturnCostDtl theMaterialReturnCostDtl in theMaterialReturnDetail.MatReturnCostDtls)
        //                    {
        //                        if (!hsCostType.ContainsKey(theMaterialReturnCostDtl.CostType))
        //                        {
        //                            hsCostType.Add(theMaterialReturnCostDtl.CostType, null);
        //                        }
        //                    }
        //                }
        //                foreach (string key in hsCostType.Keys)
        //                {
        //                    oColumn = new DataGridViewTextBoxColumn();
        //                    oColumn.HeaderText = key;
        //                    oColumn.Name = key;
        //                    oColumn.ReadOnly = arrReadOnlyType.Contains(key);
        //                    dgDetail.Columns.Insert(colSubject.Index, oColumn);
        //                    oCostColumnInfo = new CostColumnInfo()
        //                    {
        //                        ColumnIndex = oColumn.Index,
        //                        ColumnName = key,
        //                        CostType = key
        //                    };
        //                    lstCostColumnInfo.Add(oCostColumnInfo);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ShowMessage(string.Format("出租方[{0}]与租赁方[{1}]未签订采购合同",this.txtSupply.Text,ProjectInfo.Name ));
        //        }
        //        #endregion
        //    }
        //}
        void txtTransChagre_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string TransCharge = this.txtTransChagre.Text;
            validity = CommonMethod.VeryValid(TransCharge);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                return;
            }
        }

        void dgDetail_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgDetail.PointToClient(Cursor.Position);
                this.contextMenuStrip1.Show(dgDetail, _Point);
            }
        }

        void dgDelete_Click(object sender, EventArgs e)
        {
        
            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null || dr.IsNewRow) return;
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Hashtable ht = new Hashtable();
                dgDetail.Rows.Remove(dr);
                bool isDeleteAll = dgDetail.Rows.Count == 0 || (dgDetail.Rows.Count == 1 && dgDetail.Rows[0].IsNewRow);
                if (!isDeleteAll)
                {
                    string[] arrMaterialIds = dgDetail.Rows.OfType<DataGridViewRow>().Where(a => !a.IsNewRow && a != dr).Select(a => (a.Cells[colMaterialCode.Name].Tag as Material).Id).ToArray();
                    if (arrMaterialIds != null && arrMaterialIds.Length > 0)
                    {
                        foreach (MatHireOrderDetail oMatHireOrderDetail in OrderMaster.Details)
                        {
                            if (oMatHireOrderDetail.MaterialResource != null && arrMaterialIds.Contains(oMatHireOrderDetail.MaterialResource.Id))
                            {
                                foreach (OrderDetailCostSetItem oOrderDetailCostSetItem in oMatHireOrderDetail.BasicDtlCostSets)
                                {
                                    if (!string.IsNullOrEmpty(oOrderDetailCostSetItem.ExitExpression) &&
                                        !string.IsNullOrEmpty(oOrderDetailCostSetItem.ExitExpression.Trim()) &&
                                        !ht.ContainsKey(oOrderDetailCostSetItem.CostType))
                                    {
                                        ht.Add(oOrderDetailCostSetItem.CostType, null);
                                    }
                                }
                            }
                        }

                    }
                }
               
                for (int i = lstCostColumnInfo.Count - 1; i > -1; i--)
                {
                    if (  isDeleteAll||(!this.arrReturnMustCostType.Contains(lstCostColumnInfo[i].CostType) && !ht.ContainsKey(lstCostColumnInfo[i].CostType)))
                    {
                        this.dgDetail.Columns.Remove(lstCostColumnInfo[i].CostType);
                        lstCostColumnInfo.RemoveAt(i);
                    }
                }
                
            
                if (dr.Tag != null)
                {
                    (dr.Tag as MatHireReturnDetail).TempData = "删除";
                }
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MatHireOrderDetail oOrderDetail = null;
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (this.OrderMaster == null)
            {
                ShowMessage("请选择料租赁合同！");
                btnForward.Focus();
                return;
            }
            //if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            //{
            //    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            //    CurrentProjectInfo ProjectInfo = new CurrentProjectInfo() { Id = OrderMaster.ProjectId, Name=OrderMaster.ProjectName };
            //    VMaterialHireLedgerSelector VMaterialRenLedSelector = new VMaterialHireLedgerSelector(this.txtSupply.Tag as SupplierRelationInfo, ProjectInfo, this.MatHireType);
            //    VMaterialRenLedSelector.OpenSelector();

            //    IList list = VMaterialRenLedSelector.Result;
            //    foreach (MatHireLedgerMaster master in list)
            //    {
            //        if (this.MatHireType == EnumMatHireType.普通料具)
            //        {
            //            if (dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value) == oOrderDetail.MaterialCode) != null)
            //            {
            //                continue;
            //            }
            //        }
            //        else
            //        {
            //            if (dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value) == oOrderDetail.MaterialCode) != null)
            //            {
            //                break;
            //            }
            //            else
            //            {
            //                oOrderDetail = OrderMaster.Details.OfType<MatHireOrderDetail>().FirstOrDefault(a => a.MaterialResource == master.MaterialResource);
            //                if (oOrderDetail == null) break;
            //                AddRow(oOrderDetail);
            //                break;
            //            }
            //        }
            //        oOrderDetail = OrderMaster.Details.OfType<MatHireOrderDetail>().FirstOrDefault(a => a.MaterialResource == master.MaterialResource);
            //        if (oOrderDetail == null) continue;
            //        int i = this.dgDetail.Rows.Add();
            //        this.dgDetail[colMaterialCode.Name, i].Value = master.MaterialCode;
            //        this.dgDetail[colMaterialCode.Name, i].Tag = master.MaterialResource;
            //        this.dgDetail[colMaterialName.Name, i].Value = master.MaterialName;
            //        this.dgDetail[colMaterialSpec.Name, i].Value = master.MaterialSpec;
            //        this.dgDetail[colCategoryQuantity.Name, i].Value = master.LeftQuantity;
            //        this.dgDetail[colUnit.Name, i].Value = master.MatStandardUnitName;
            //        this.dgDetail[colUnit.Name, i].Tag = master.MatStandardUnit;
            //        this.dgDetail[colPrice.Name, i].Value = oOrderDetail.Price;// master.RentalPrice;
            //        this.dgDetail[this.colLength.Name, i].Value = "1";
            //        //this.dgDetail[colSubject.Name, i].Tag = master.SubjectGUID;
            //        //this.dgDetail[colSubject.Name, i].Value = master.SubjectName;
            //        //this.dgDetail[colUsedPart.Name, i].Tag = master.UsedPart;
            //        //this.dgDetail[colUsedPart.Name, i].Value = master.UsedPartName;
            //        //this.dgDetail[this.colBorrowUnit.Name, i].Tag = master.TheRank;
            //        //this.dgDetail[colBorrowUnit.Name, i].Value = master.TheRankName;

            //    }

            //}
            //else 
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Contains(ConstRepairCostType)) //this.dgDetail.Columns[e.ColumnIndex].Name.Contains("维修费"))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                if (theCurrentRow.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("请先选择料具信息！");
                    return;
                }
                else
                {
                    //查询当前合同的维修内容(根据料具区分)
                    IList list_RepairCon = new ArrayList();
                    Material theMaterial = theCurrentRow.Cells[colMaterialCode.Name].Tag as Material;
                    foreach (MatHireOrderDetail theMaterialRentalOrderDetail in OrderMaster.Details)
                    {
                        if (theMaterialRentalOrderDetail.MaterialResource.Id == theMaterial.Id)
                        {
                            foreach (OrderDetailCostSetItem theBasicDtlCostSet in theMaterialRentalOrderDetail.BasicDtlCostSets)
                            {
                                if (theBasicDtlCostSet.SetType == SetType.维修设置.ToString())
                                {
                                    list_RepairCon.Add(theBasicDtlCostSet.WorkContent);
                                }
                            }
                        }
                    }
                    if (list_RepairCon.Count == 0)
                    {
                        MessageBox.Show("当前料具供应商的合同未设置维修内容，请先设置维修内容和单价！");
                        return;
                    }
                    else
                    {
                        VMaterialHireRepair vMaterialRepair = new VMaterialHireRepair(theMaterial, list_RepairCon);
                        vMaterialRepair.OpenMaterialRepair(theCurrentRow.Cells[ConstRepairCostType].Tag as IList);
                        IList list_repairContent = vMaterialRepair.Result;
                        theCurrentRow.Cells[ConstRepairCostType].Tag = vMaterialRepair.Result;

                        //查询并设置维修单价
                        decimal repairMoney = 0;
                        foreach (MatHireRepair repair in list_repairContent)
                        {
                            int temp_count = 0;
                            foreach (MatHireOrderDetail detail in OrderMaster.Details)
                            {
                                Material currentMaterial = theCurrentRow.Cells[colMaterialCode.Name].Tag as Material;
                                if (detail.MaterialResource.Id == currentMaterial.Id)
                                {
                                    foreach (OrderDetailCostSetItem costSetDtl in detail.BasicDtlCostSets)
                                    {
                                        if (costSetDtl.SetType == "维修设置")
                                        {
                                            if (costSetDtl.WorkContent == repair.WorkContent)
                                            {
                                                repair.Price = costSetDtl.Price;
                                                temp_count++;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                }
                                //如果未找到价格定义，则赋值 0
                                if (temp_count == 0)
                                {
                                    repair.Price = 0;
                                }
                            }
                            repairMoney = repairMoney + repair.Quantity * repair.Price;
                        }
                        theCurrentRow.Cells[ConstRepairCostType].Value = repairMoney.ToString();

                    }
                }
            }
            // }
        }
      
        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                Master = new MatHireReturnMaster();
                Master.CreatePerson = ConstObject.LoginPersonInfo;
                Master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                Master.CreateDate = ConstObject.LoginDate;
                Master.CreateYear = ConstObject.LoginDate.Year;
                Master.CreateMonth = ConstObject.LoginDate.Month;
                Master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                Master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                Master.HandlePerson = ConstObject.LoginPersonInfo;
                Master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                Master.DocState = DocumentState.Edit;
                Master.BalState = 0;//结算状态 0：未结算  1; 已结算
                Master.MatHireType = this.MatHireType;
                Master.IsLoss = this.IsLoss;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
      
                //归属项目
                //btnStates(0);
                //if (ProjectInfo != null)
                //{
                //    //txtProject.Tag = ProjectInfo;
                //    //txtProject.Text = ProjectInfo.Name;
                //    Master.ProjectId = ProjectInfo.Id;
                //    Master.ProjectName = ProjectInfo.Name;
                //}

                txtContractNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("新建单据错误：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            Hashtable htStockQty = null;
            try
            {
                if (!ViewToModel()) return false;
                //if (!ValidKC()) return false;
                string sMaterialIDs = string.Join(",", Master.Details.OfType<MatHireReturnDetail>().Select(a => string.Format("'{0}'", a.MaterialResource.Id)).Distinct().ToArray());
                 htStockQty =string.IsNullOrEmpty(sMaterialIDs)?null: model.MaterialHireMngSvr.GetPreviousJC(Master.CreateDate, sMaterialIDs, Master.ProjectId, Master.TheSupplierRelationInfo.Id);
                 if (htStockQty != null && htStockQty.Count>0)
                 {
                     foreach (MatHireReturnDetail oDetail in Master.Details)
                     {
                         if (htStockQty.ContainsKey(oDetail.MaterialResource.Id))
                         {
                             oDetail.BeforeStockQty = ClientUtil.ToDecimal(htStockQty[oDetail.MaterialResource.Id]);
                         }
                     }
                 }
                if (Master.Id == null)
                {
                    Master.DocState = DocumentState.InExecute;
                    Master = model.MaterialHireMngSvr.SaveMaterialHireReturnMaster(Master, CanReturnUp);
                }
                else
                {
                    Master.DocState = DocumentState.InExecute;
                    Master = model.MaterialHireMngSvr.UpdateMaterialHireReturnMaster(Master);
                }

                //插入日志
                //MStockIn.InsertLogData(matReturnMaster.Id, "保存", matReturnMaster.Code, matReturnMaster.CreatePerson.Name, "料具退料单","");
                txtCode.Text = Master.Code;
                txtCode.Focus();
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
   
                //btnStates(0);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {

            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        Master = model.MaterialHireMngSvr.GetMaterialHireReturnById(Master.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                ShowMessage("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (Master.DocState == DocumentState.Edit || Master.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                Master = model.MaterialHireMngSvr.GetMaterialHireReturnById(Master.Id);
                ModelToView();
    
                //btnStates(1);
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(Master.DocState));
            MessageBox.Show(message);
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                Master = model.MaterialHireMngSvr.GetMaterialHireReturnById(Master.Id);
                if (Master.DocState == DocumentState.Valid || Master.DocState == DocumentState.Edit)
                {
                    if (!model.MaterialHireMngSvr.DeleteMaterialHireReturn(Master)) return false;
                    ClearView();
  
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(Master.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            base.RefreshView();
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = Master.Code;
                operDate.Value = Master.CreateDate ;
                if (Master.CreatePerson != null)
                {
                    txtCreatePerson.Tag = Master.CreatePerson;
                    txtCreatePerson.Text = Master.CreatePersonName;
                }
                txtRemark.Text = Master.Descript;
                txtCreateDate.Text = Master.CreateDate.ToShortDateString();
                txtSumQuantity.Text = Master.SumExitQuantity.ToString();
              
                txtTransChagre.Text = ClientUtil.ToString(Master.TransportCharge);
                txtBalRule.Text = Master.BalRule;
                txtContractNo.Text = Master.OldContractNum;
                OrderMaster=Master.Contract;
                txtContract.Tag = OrderMaster;
                txtContract.Text = Master.ContractCode;
                txtProjectName.Tag = Master.ProjectId;
                txtProjectName.Text = Master.ProjectName;
                txtSupply.Tag = Master.TheSupplierRelationInfo;
                txtSupply.Text = Master.SupplierName;
                txtOldCode.Text = Master.BillCode;
                txtOneLen.Text = Master.LessOneQuanity.ToString();
                RefleshColumn(null);
                
                //显示收料单明细
                this.dgDetail.Rows.Clear();

                //查询当前料具供应商合同租赁单价


                foreach (MatHireReturnDetail var in Master.Details.OfType<MatHireReturnDetail>().OrderByDescending(a=>a.MaterialLength))
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;

                    this.dgDetail[colPrice.Name, i].Value = var.RentalPrice;
                    //this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    if (IsProject)
                    {
                        //设置使用部位
                        this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                        this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;

                        //设置使用队伍
                        this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.BorrowUnit;
                        this.dgDetail[colBorrowUnit.Name, i].Value = var.BorrowUnitName;
                        //科目
                        this.dgDetail[colSubject.Name, i].Tag = var.SubjectGUID;
                        this.dgDetail[colSubject.Name, i].Value = var.SubjectName;
                    }
                    this.dgDetail[colCategoryQuantity.Name, i].Value =Math.Round( var.BeforeStockQty / var.MaterialLength,2);
                    this.dgDetail[colExitQuantity.Name, i].Value = var.ExitQuantity;
                    this.dgDetail[colExitQuantity.Name, i].Tag = var.ExitQuantity;
                    this.dgDetail[colTempQty.Name, i].Value = var.ExitQuantity;
                    this.dgDetail[colBroachQuantity.Name, i].Value = var.BroachQuantity;
                    this.dgDetail[colRejectQuantity.Name, i].Value = var.RejectQuantity;
                    this.dgDetail[colConsumeQuantity.Name, i].Value = var.ConsumeQuantity;
                    this.dgDetail[colDisCardQty.Name, i].Value = var.DisCardQty;
                    this.dgDetail[colRepairQty.Name, i].Value = var.RepairQty;
                    this.dgDetail[colLossQty.Name, i].Value = var.LossQty;
                    this.dgDetail[colLength.Name, i].Value = var.MaterialLength;
                    this.dgDetail[this.colType.Name, i].Value = var.MaterialType;
                    //查询当前出租方，队伍料具库存
                    //decimal stockQty = model.MaterialHireMngSvr.GetMatStockQty(this.txtSupply.Result[0] as SupplierRelationInfo, var.BorrowUnit, var.MaterialResource, ProjectInfo.Id,Master.MatHireType);

                    this.dgDetail[colCategoryQuantity.Name, i].Value = var.BeforeStockQty; //stockQty;

                    //foreach (MatHireOrderDetail theDetail in theMaterialRentalOrderMaster.Details)
                    //{
                    //    if (theDetail.MaterialResource.Id == var.MaterialResource.Id)
                    //    {
                    //        this.dgDetail[colPrice.Name, i].Value = theDetail.Price;
                    //    }
                    //}
                    foreach (MatHireReturnCostDtl costDtl in var.MatReturnCostDtls)
                    {
                        IList list_temp = new ArrayList();
                        if (costDtl.CostType == "维修费用")
                        {
                            foreach (MatHireRepair materialRepait in var.MatRepairs)
                            {
                                list_temp.Add(materialRepait);
                            }
                            this.dgDetail[costDtl.CostType, i].Tag = list_temp;
                        }
                        this.dgDetail[costDtl.CostType, i].Value = costDtl.Money;
                    }
                    this.dgDetail.Rows[i].Tag = var;
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                MatHireReturnDetail theMaterialReturnDetail = null;
                decimal sumMoney = 0;
                Master.CreateDate  = operDate.Value.Date;
                
                Master.Descript = this.txtRemark.Text;
                Master.SumExitQuantity = ClientUtil.ToDecimal(txtSumQuantity.Text);
              
                if (txtTransChagre.Text != "")
                {
                    Master.TransportCharge = TransUtil.ToDecimal(this.txtTransChagre.Text);
                }
                Master.IsLoss = IsLoss;
                Master.MatHireType = MatHireType;
                Master.Contract = txtContract.Tag as MatHireOrderMaster ;
                Master.ContractCode = txtContract.Text;
                Master.ProjectId =ClientUtil.ToString( txtProjectName.Tag);
                Master.ProjectName = txtProjectName.Text;
                Master.TheSupplierRelationInfo = txtSupply.Tag as SupplierRelationInfo;
                Master.SupplierName = txtSupply.Text;
                Master.OldContractNum = this.txtContractNo.Text;
                Master.BalRule = txtBalRule.Text;
                Master.BillCode = txtOldCode.Text;
                Master.LessOneQuanity = ClientUtil.ToDecimal(txtOneLen.Text);
                //退料明细
                //MatRenLed_list = new ArrayList();
                Master.Details.Clear();
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    if (var.Tag == null)
                    {
                        theMaterialReturnDetail = new MatHireReturnDetail();
                    }
                    else
                    {
                        theMaterialReturnDetail = var.Tag as MatHireReturnDetail;
                        Master.Details.Remove(theMaterialReturnDetail);
                    }
                    
                    //材料
                    theMaterialReturnDetail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    theMaterialReturnDetail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    theMaterialReturnDetail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    theMaterialReturnDetail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    //计量单位
                    theMaterialReturnDetail.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    theMaterialReturnDetail.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);

                    theMaterialReturnDetail.ExitQuantity = ClientUtil.ToDecimal(var.Cells[colExitQuantity.Name].Value);//退场数量
                    decimal tempQty = ClientUtil.ToDecimal(var.Cells[colTempQty.Name].Value);
                    theMaterialReturnDetail.TempData = tempQty.ToString();
                    theMaterialReturnDetail.RejectQuantity = ClientUtil.ToDecimal(var.Cells[colRejectQuantity.Name].Value);//报废数量
                    theMaterialReturnDetail.BroachQuantity = ClientUtil.ToDecimal(var.Cells[colBroachQuantity.Name].Value);//完好数量
                    theMaterialReturnDetail.ConsumeQuantity = ClientUtil.ToDecimal(var.Cells[colConsumeQuantity.Name].Value);//消耗数量
                    theMaterialReturnDetail.LossQty = ClientUtil.ToDecimal(var.Cells[colLossQty.Name].Value);//报损数量
                    theMaterialReturnDetail.DisCardQty = ClientUtil.ToDecimal(var.Cells[colDisCardQty.Name].Value);//切头数量
                    theMaterialReturnDetail.RepairQty = ClientUtil.ToDecimal(var.Cells[colRepairQty.Name].Value);//维修数量
                    theMaterialReturnDetail.MaterialLength = 1;
                    if (this.MatHireType == EnumMatHireType.钢管)
                    {
                        theMaterialReturnDetail.MaterialLength = ClientUtil.ToDecimal(var.Cells[this.colLength.Name].Value);
                    }
                    else if (this.MatHireType == EnumMatHireType.碗扣)
                    {
                        theMaterialReturnDetail.MaterialType = ClientUtil.ToString(var.Cells[this.colType.Name].Value);
                        theMaterialReturnDetail.MaterialLength = ClientUtil.ToDecimal(var.Cells[this.colLength.Name].Value);
                    }
                    //使用部位
                    if (var.Cells[colUsedPart.Name].Tag != null)
                    {
                        theMaterialReturnDetail.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                        theMaterialReturnDetail.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                        theMaterialReturnDetail.UsedPartSysCode = ClientUtil.ToString((var.Cells[colUsedPart.Name].Tag as GWBSTree).SysCode);
                    }
                    if (var.Cells[colSubject.Name].Tag != null)
                    {
                        theMaterialReturnDetail.SubjectGUID = var.Cells[colSubject.Name].Tag as CostAccountSubject;
                        theMaterialReturnDetail.SubjectName = ClientUtil.ToString(var.Cells[colSubject.Name].Value);
                        theMaterialReturnDetail.SubjectSysCode = ClientUtil.ToString((var.Cells[colSubject.Name].Tag as CostAccountSubject).SysCode);
                    }
                    if (var.Cells[this.colBorrowUnit.Name].Tag != null)
                    {
                        theMaterialReturnDetail.BorrowUnit = var.Cells[colBorrowUnit.Name].Tag as SupplierRelationInfo;
                        theMaterialReturnDetail.BorrowUnitName = ClientUtil.ToString(var.Cells[colBorrowUnit.Name].Value);
                    }
                    theMaterialReturnDetail.RentalPrice = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    theMaterialReturnDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    //处理费用明细
                    //先清除已有费用明细
                    theMaterialReturnDetail.MatReturnCostDtls.Clear();
                    //IList list_temp = new ArrayList();
                    //foreach (MatHireReturnCostDtl returnCostDtl in theMaterialReturnDetail.MatReturnCostDtls)
                    //{
                    //    list_temp.Add(returnCostDtl);
                    //}
                    //foreach (MatHireReturnCostDtl detail in list_temp)
                    //{
                    //    theMaterialReturnDetail.MatReturnCostDtls.Remove(detail);
                    //}
                    //然后新增
                    foreach ( CostColumnInfo oCostColumnInfo in  lstCostColumnInfo)
                    {
                        MatHireReturnCostDtl matCost = new MatHireReturnCostDtl();
                        if (oCostColumnInfo.CostType == "维修费用")
                        {
                            if (var.Cells[oCostColumnInfo.CostType].Tag != null)
                            {
                                foreach (MatHireRepair materialRepair in var.Cells[oCostColumnInfo.CostType].Tag as IList)
                                {
                                    theMaterialReturnDetail.AddMatRepairs(materialRepair);
                                }
                            }
                            matCost.CostType = oCostColumnInfo.CostType;
                            matCost.Money = ClientUtil.ToDecimal(var.Cells[oCostColumnInfo.CostType].Value);
                            sumMoney += matCost.Money;
                        }
                        else
                        {
                            matCost.CostType = oCostColumnInfo.CostType;
                            matCost.Money = ClientUtil.ToDecimal(var.Cells[oCostColumnInfo.CostType].Value);
                            sumMoney += matCost.Money;
                        }
                        if (matCost.Money != 0) GetExpressionInfo(matCost, theMaterialReturnDetail.MaterialResource);
                        theMaterialReturnDetail.AddMatReturnCostDtls(matCost);
                    }

                    Master.AddDetail(theMaterialReturnDetail);
                }

                Master.SumExtMoney = sumMoney;

                //运输费
                decimal TransChagre = 0;
                if (txtTransChagre.Text == "")
                {
                    TransChagre = 0;
                }
                else
                {
                    TransChagre = ClientUtil.ToDecimal(txtTransChagre.Text);
                }
                Master.SumExtMoney += TransChagre;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取公式 理论值 价格
        /// </summary>
        /// <param name="oCostDtl"></param>
        /// <param name="oMaterial"></param>
        public void GetExpressionInfo(MatHireReturnCostDtl oCostDtl, Material oMaterial)
        {
            MatHireOrderDetail oOrderDetail = null;
            OrderDetailCostSetItem oExpressionItem;
            if (OrderMaster != null && oCostDtl != null)
            {
                oCostDtl.Price = 0;
                oCostDtl.ConstValue = 1;
                oCostDtl.Quantity = 0;
                oCostDtl.Expression = string.Empty;
                oOrderDetail = OrderMaster.Details.OfType<MatHireOrderDetail>().FirstOrDefault(a => a.MaterialResource.Id == oMaterial.Id);
                if (oOrderDetail != null)
                {
                    oExpressionItem = oOrderDetail.BasicDtlCostSets.FirstOrDefault(a =>
                          a.SetType == Enum.GetName(typeof(SetType), SetType.公式定义) &&
                          !string.IsNullOrEmpty(a.ExitExpression) && !string.IsNullOrEmpty(a.ExitExpression.Trim()) &&
                          a.CostType == oCostDtl.CostType);
                    if (oExpressionItem != null)
                    {
                        oCostDtl.Expression = oExpressionItem.ExitExpression;//获取公式
                        foreach (OrderDetailCostSetItem oItem in oOrderDetail.BasicDtlCostSets.Where(a => a.SetType == Enum.GetName(typeof(SetType), SetType.价格定义) && a.CostType.IndexOf("单价") > -1))
                        {
                            if (oExpressionItem.ExitExpression.IndexOf(string.Format("[{0}]", oItem.CostType)) > -1)
                            {
                                oCostDtl.Price = oItem.Price;
                                break;
                            }
                        }
                        foreach (OrderDetailCostSetItem oItem in oOrderDetail.BasicDtlCostSets.Where(a => a.SetType == Enum.GetName(typeof(SetType), SetType.价格定义) && a.CostType.IndexOf("理论") > -1))
                        {
                            if (oExpressionItem.ExitExpression.IndexOf(string.Format("[{0}]", oItem.CostType)) > -1)
                            {
                                oCostDtl.ConstValue = oItem.Price;//理论值
                                break;
                            }
                        }
                        if (oCostDtl.Price != 0)
                        {
                            oCostDtl.Quantity = oCostDtl.Money / (oCostDtl.Price * oCostDtl.ConstValue);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 保存前校验库存
        /// </summary>
        /// <returns></returns>
        private bool ValidKC()
        {
            //校验当前退料单是否是插入的单据
            bool value = model.MaterialHireMngSvr.VerifyReturnMatBusinessDate(operDate.Value.Date, Master.ProjectId);
            //校验库存情况
            DataDomain domain = model.MaterialHireMngSvr.VerifyReturnMatKC(Master, value);
            if (ClientUtil.ToInt(domain.Name1) == 0)
            {
                //通过
                return true;
            }
            else if (ClientUtil.ToInt(domain.Name1) == 1)
            {
                //当前库存不足
                MessageBox.Show(domain.Name2.ToString());
                return false;
            }
            else if (ClientUtil.ToInt(domain.Name1) == 2)
            {
                //插入业务日期的库存不足
                MessageBox.Show(domain.Name2.ToString());
                return false;
            }
            else if (ClientUtil.ToInt(domain.Name1) == 3)
            {
                //插入该笔退料后，业务日期[yyyy-MM-dd]的库存为负[yyyy-MM-dd]是计算中的第一笔负数的日期
                MessageBox.Show(domain.Name2.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string validMessage = "";
            if (this.dgDetail.Rows.Count == 0 || (dgDetail.Rows.Count == 1 && dgDetail.Rows[0].IsNewRow))
            {
                MessageBox.Show("发料单明细不能为空");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            //业务日期不得大于服务器日期
            //DateTime ServerDate = CommonMethod.GetServerDateTime();
            //DateTime OperDate = this.operDate.Value.Date;
            //if (DateTime.Compare(OperDate, ServerDate) > 0)
            //{
            //    validMessage += "业务日期不得大于服务器日期！";
            //}
            //if (txtContractNo.Text == "")
            //{
            //    validMessage += "原始合同号不能为空！\n";
            //}
            //if (txtSupply.Result.Count == 0)
            //{
            //    validMessage += "出租方不能为空！\n";
            //}
            if (txtContract.Tag == null)
            {
                ShowMessage("合同不能为空");
                return false;
            }
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            //收料单明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                   ShowMessage("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }

                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    ShowMessage("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                     
                }
                if (this.MatHireType == EnumMatHireType.钢管 )
                {
                    if (dr.Cells[colLength.Name].Value == null)
                    {
                        ShowMessage("请输入钢管长度");
                       dgDetail.CurrentCell=dr.Cells[colLength.Name];
                       return false;
                    }
                }
                else if (this.MatHireType == EnumMatHireType.碗扣)
                {
                    if (dr.Cells[colType.Name].Value == null)
                    {
                        ShowMessage("请选择碗扣型号");
                        dgDetail.CurrentCell = dr.Cells[colType.Name];
                        return false;
                    }
                }

                //输入维修数量时必须选择维修内容和维修数量
                if (dr.Cells[colRepairQty.Name].Value != null && dr.Cells[colRepairQty.Name].Value.ToString() != "")
                {
                    decimal repairQty = ClientUtil.ToDecimal(dr.Cells[colRepairQty.Name].Value);
                    if (repairQty != 0)
                    {
                        //查找是否有维修费列
                        int count = 0;
                        foreach (DataGridViewColumn theDataGridViewColumn in dr.DataGridView.Columns)
                        {
                            if (theDataGridViewColumn.HeaderText == "维修费用")
                            {
                                count++;
                                break;
                            }
                        }
                        if (count > 0)
                        {
                            DataGridViewCell theDataGridViewCell = dr.Cells["维修费用"] as DataGridViewCell;
                            if (theDataGridViewCell != null)
                            {
                                IList lst = dr.Cells["维修费用"].Tag as IList;
                                if (lst == null || lst.Count == 0)
                                {
                                    ShowMessage("请双击 [维修费用] 单元格设置维修内容和数量，系统自动计算维修费！");
                                    this.dgDetail.CurrentCell = theDataGridViewCell;
                                    this.dgDetail.BeginEdit(true);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            ShowMessage("该料具商合同中未定义维修费用，请修改合同，此单不能保存！");
                            return false;
                        }
                    }
                }
                if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                {
                    ShowMessage("单价不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }
            }
            dgDetail.Update();
            return true;
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            decimal dLength = 0;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            string sValue = string.Empty;
            Material oMaterial=null;
            string oldExpression = string.Empty;
            if (colName == colBroachQuantity.Name || colName == colRepairQty.Name || colName == colDisCardQty.Name ||
                 colName == colRejectQuantity.Name || colName == colConsumeQuantity.Name || colName == colLossQty.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colBroachQuantity.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colBroachQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colRejectQuantity.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colRejectQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colConsumeQuantity.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colConsumeQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colLossQty.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colLossQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colDisCardQty.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colDisCardQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colRepairQty.Name].Value != null)
                {
                    string temp_qty = dgDetail.Rows[e.RowIndex].Cells[colRepairQty.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_qty);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                //完整数量
                decimal broachQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colBroachQuantity.Name].Value);
                //报废数量  
                decimal rejectQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colRejectQuantity.Name].Value);
                //消耗数量
                decimal consumeQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colConsumeQuantity.Name].Value);
                //报损数量
                decimal lossQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colLossQty.Name].Value);
                //切头数量
                decimal disCardQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colDisCardQty.Name].Value);
                //维修数量
                decimal repairQuantity = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colRepairQty.Name].Value);
                //报废不参加计算  只有损耗 报废才参加计算
                this.dgDetail.Rows[e.RowIndex].Cells[colExitQuantity.Name].Value =
                   this.IsLoss? broachQuantity + rejectQuantity + consumeQuantity + lossQuantity + disCardQuantity + repairQuantity
                   : broachQuantity + consumeQuantity + lossQuantity + disCardQuantity + repairQuantity;

                //计算退料总数量
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colExitQuantity.Name].Value;
                decimal sumqty = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colExitQuantity.Name].Value;
                    if (quantity == null) quantity = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                }

                txtSumQuantity.Text = sumqty.ToString();

                //计算当前全部费用金额
                foreach (CostColumnInfo oCostColumnInfo in lstCostColumnInfo)
                {
                    if (oCostColumnInfo.CostType!=ConstRepairCostType)
                    {
                        oMaterial =this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag as Material;
                        string expression = GetCostExpression(oCostColumnInfo.CostType, oMaterial, out oldExpression);
                        if (expression != "" && expression != null)
                        {
                            MatHireReturnDetail detail = new MatHireReturnDetail();
                            detail.BroachQuantity = broachQuantity;
                            detail.RejectQuantity = rejectQuantity;
                            detail.ConsumeQuantity = consumeQuantity;
                            detail.LossQty = lossQuantity;
                            detail.DisCardQty = disCardQuantity;
                            detail.RepairQty = repairQuantity;
                            try
                            {
                                this.dgDetail.CurrentRow.Cells[oCostColumnInfo.CostType].Value = GetCalResult(detail,ref expression);
                            }
                            catch (Exception ex)
                            {
                                string sMsg = string.Join(";", Regex.Matches(expression, "\\[[^\\[\\]]+\\]").OfType<Match>().Select(a => a.Value).ToArray());
                                if (!string.IsNullOrEmpty(sMsg))
                                {
                                    this.ShowMessage(string.Format("合同【{0}】中物资【{1}】定义退料【{2}】公式【{3}】解析后[{4}](可能因为【{5}】没有定义或无法识别)",
                                        OrderMaster.Code, oMaterial.Code, oCostColumnInfo.CostType,oldExpression, expression, sMsg));
                                    this.dgDetail.CurrentRow.Cells[oCostColumnInfo.CostType].Value = 0;
                                    dgDetail.CancelEdit();
                                    dgDetail.BeginEdit(true);
                                }
                            }
                        }
                        else
                        {
                            this.dgDetail.CurrentRow.Cells[oCostColumnInfo.CostType].Value = 0;
                        }
                    }
                }
            }
            else if (this.colType.Name == colName)
            {
                DataGridViewRow oRow = dgDetail.Rows[e.RowIndex];
               
                    oRow.Cells[colLength.Name].Value = this.GetLength(oRow.Cells[colType.Name].Value);
                
            }
            else if (this.colLength.Name == colName)
            {
                DataGridViewRow oRow = dgDetail.Rows[e.RowIndex];
                string sTemp_qty = string.Empty;
                if (oRow.Cells[this.colLength.Name].Value != null)
                {
                    sTemp_qty = ClientUtil.ToString(oRow.Cells[this.colLength.Name].Value);
                    validity = CommonMethod.VeryValid(sTemp_qty);
                    if (!validity)
                    {
                        ShowMessage("请输入数字！");
                        dgDetail.CurrentCell = oRow.Cells[this.colLength.Name];
                    }
                }
            }
        }

      

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string id)
        {
            try
            {
                if (id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    Master = model.MaterialHireMngSvr.GetMaterialHireReturnById(id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
 
                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtSumQuantity, txtContractNo, txtBalRule,txtSupply,txtContract,txtProjectName };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialCode.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name,
                                                colBorrowUnit.Name, colUsedPart.Name, colSubject.Name,
                                                this.colPrice.Name,this.colType.Name,this.colLength.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            txtContract.Tag = null;
            txtSupply.Tag = null;
            txtProjectName.Tag = null;
            if (lstCostColumnInfo != null && lstCostColumnInfo.Count > 0)
            {
                foreach (CostColumnInfo oCostColumnInfo in lstCostColumnInfo)
                {
                    dgDetail.Columns.Remove(oCostColumnInfo.ColumnName);
                }
                lstCostColumnInfo.Clear();
            }
        }

        private void ClearControl(Control c)
        {
            try
            {
                foreach (Control cd in c.Controls)
                {
                    ClearControl(cd);
                }
                //自定义控件清空
                if (c is CustomEdit)
                {
                    c.Tag = null;
                    c.Text = "";
                }
                else if (c is CustomDataGridView)
                {
                    (c as CustomDataGridView).Rows.Clear();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region 解析表达式，计算费用金额
        //通过计算表达式，计算本单元格的计算值
        private decimal GetCalResult(MatHireReturnDetail detail, ref string express)
        {
            decimal result = 0;
            if (!string.IsNullOrEmpty(express))
            {
                if (express.IndexOf("[完好]") != -1)
                {
                    express = express.Replace("[完好]", detail.BroachQuantity.ToString());
                }
                if (express.IndexOf("[报损]") != -1)
                {
                    express = express.Replace("[报损]", detail.LossQty.ToString());
                }
                if (express.IndexOf("[报废]") != -1)
                {
                    express = express.Replace("[报废]", detail.RejectQuantity.ToString());
                }
                if (express.IndexOf("[消耗]") != -1)
                {
                    express = express.Replace("[消耗]", detail.ConsumeQuantity.ToString());
                }
                if (express.IndexOf("[切头]") != -1)
                {
                    express = express.Replace("[切头]", detail.DisCardQty.ToString());
                }
                if (express.IndexOf("[维修]") != -1)
                {
                    express = express.Replace("[维修]", detail.RepairQty.ToString());
                }
                result = Decimal.Parse(CommonUtil.CalculateExpression(express, 6));
            }
            return result;
        }
        //获取表达式
        private string GetCostExpression(string costType, Material material, out string oldExpression)
        {
            string sDefaultExpression = string.Empty, sExpression = string.Empty; oldExpression = string.Empty;
            //if (lstPriceType == null || lstPriceType.Count == 0) { return sResultExpression; }
            MatHireOrderDetail oMatHireOrderDetail = OrderMaster.Details.OfType<MatHireOrderDetail>().FirstOrDefault(a => a.MaterialResource.Id == material.Id);
            if (oMatHireOrderDetail == null) { return sDefaultExpression; }
            OrderDetailCostSetItem oOrderDetailCostSetItem = oMatHireOrderDetail.BasicDtlCostSets.FirstOrDefault(a => a.CostType == costType);
            if (oOrderDetailCostSetItem == null || string.IsNullOrEmpty(oOrderDetailCostSetItem.ExitExpression) ||string.IsNullOrEmpty(oOrderDetailCostSetItem.ExitExpression.Trim())) { return sDefaultExpression; }
            oldExpression=sExpression = oOrderDetailCostSetItem.ExitExpression;
            foreach (OrderDetailCostSetItem oItem in oMatHireOrderDetail.BasicDtlCostSets.Where(a => a.SetType == "价格定义"))
            {
                sExpression = sExpression.Replace(string.Format("[{0}]", oItem.CostType), oItem.Price.ToString());
            }
            return sExpression;

        }
        //private string GetCostExpression(string costType, Material material)
        //{
        //    string expression = "";
        //    foreach (MatHireOrderDetail detail in OrderMaster.Details)
        //    {
        //        if (material != null && detail.MaterialResource.Id == material.Id)
        //        {
        //            foreach (OrderDetailCostSetItem theBasicDtlCostSet in detail.BasicDtlCostSets)
        //            {
        //                if (theBasicDtlCostSet.CostType == costType && theBasicDtlCostSet.SetType == "公式定义")
        //                {
        //                    expression = theBasicDtlCostSet.ExitExpression;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    //获取全部价格类型
        //    IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_PRICETYPE);
        //    foreach (MatHireOrderDetail detail in OrderMaster.Details)
        //    {
        //        if (material != null && detail.MaterialResource.Id == material.Id)
        //        {
        //            foreach (BasicDataOptr theBasicDataOptr in list)
        //            {
        //                string priceType = "";
        //                if (expression != null && expression.IndexOf(theBasicDataOptr.BasicName) != -1)
        //                {
        //                    priceType = theBasicDataOptr.BasicName;
        //                }
        //                if (priceType != "")
        //                {
        //                    int count = 0;
        //                    foreach (OrderDetailCostSetItem theBasicDtlCostSet in detail.BasicDtlCostSets)
        //                    {
        //                        //取基本费用明细中的价格类型
        //                        if (theBasicDtlCostSet.SetType == "价格定义")
        //                        {
        //                            if (theBasicDtlCostSet.CostType == priceType)
        //                            {
        //                                string price = theBasicDtlCostSet.Price.ToString();
        //                                expression = expression.Replace(priceType, price);
        //                                count++;
        //                            }
        //                        }
        //                    }
        //                    //count=0：用户未定义当前价格类型，此时赋值0
        //                    if (count == 0)
        //                    {
        //                        expression = expression.Replace(priceType, ClientUtil.ToString(0));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return expression;
        //}
        #endregion

        #region
        public void ShowMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //public MatHireOrderMaster GetOrderMaster()
        //{

        //    ObjectQuery oQuery = new ObjectQuery();
        //    oQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Result[0] as SupplierRelationInfo));
        //    oQuery.AddCriterion(Expression.Eq("ProjectId", this.ProjectInfo.Id));
        //    IList list = model.MaterialHireMngSvr.GetMaterialHireOrder(oQuery) as IList;
        //    return list == null || list.Count == 0 ? null : list[0] as MatHireOrderMaster;
        //}
        public decimal GetLength(object oValue)
        {
            decimal dLen = 0;
            string sValue = string.Empty;
            if (oValue != null)
            {
                sValue = ClientUtil.ToString(oValue);
                sValue = sValue.Replace("LG", "").Replace("HG", "");
                dLen = ClientUtil.ToDecimal(sValue);
            }
            return dLen;
        }
        #endregion
   


    }
    public class CostColumnInfo
    {
        public int ColumnIndex;
        public string ColumnName;
        public string CostType;

    }
}
