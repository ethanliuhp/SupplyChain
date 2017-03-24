using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using NHibernate.Criterion;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using CustomServiceClient.CustomWebSrv;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder;
using System.Text.RegularExpressions;
 

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireCollection : TMasterDetailView
    {
        #region 变量
        public EnumMatHireType MatHireType;
        //public CurrentProjectInfo ProjectInfo=null;
        private MMaterialHireMng model = new MMaterialHireMng();
        private MatHireOrderMaster OrderMaster=null;
        public  MatHireCollectionMaster Master;
        private IList<CostColumnInfo> lstCostColumnInfo = new List<CostColumnInfo>();
        private IList  lstDelDetial = new ArrayList();
        public static  string [] arrWKType =  { "LG3.0", "LG2.4", "LG1.8", "LG1.5", "LG1.2", "LG1.0", "LG0.9", "LG0.6", "HG1.5", "HG1.2", "HG0.9", "HG0.6", "HG0.3" };
        private DataGridViewRow oCopyRow = null;
        private DataGridViewRow oSelectRow = null;
        private bool IsProject = false;
        private bool CanReturnUp = true;//允许退超
        private const string ConstCollectionName = "发料";
       // IList lstPriceType =null;
        #endregion
        public VMaterialHireCollection(EnumMatHireType matHireType )
        {
            InitializeComponent();
            this.MatHireType = matHireType;
            InitData();
            InitEvent();
        }

        #region 
        public void InitData()
        {
            txtBalRule.DataSource = (Enum.GetNames(typeof(EnumMaterialHireMngBalRule)));
            if (this.MatHireType == EnumMatHireType.普通料具)
            {
                dgDetail.ContextMenuStrip = cmsDg;
                ToolStripMenuItemPaste.Visible = false;
            }
            this.txtSupply.Enabled = true;
            //lstPriceType = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_PRICETYPE);
            if (MatHireType == EnumMatHireType.碗扣)
            {
               // colType.Items.AddRange(arrWKType);
               
                colType.Visible = true;
                colLength.Visible = false;
            }
            else if (MatHireType == EnumMatHireType.钢管)
            {
                colType.Visible = false;
                colLength.Visible = true;
            }
            else
            {
                colType.Visible = false;
                colLength.Visible = false;
            }
            #region 是否使项目
            this.colBorrowUnit.Visible = IsProject;
            this.colSubject.Visible = IsProject;
            this.colUsedPart.Visible = IsProject;
            groupSame.Visible = IsProject;
            this.btnSetDW.Visible = IsProject;
            this.btnSetPart.Visible = IsProject;
            this.btnSetZL.Visible = IsProject;
            #endregion
        }
        public void InitEvent()
        {

            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
           // this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            //this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            //this.dgDetail.MouseDown += new MouseEventHandler(dgDetail_MouseDown);
            this.dgDelete.Click += new EventHandler(dgDelete_Click);
            this.txtTransCharge.tbTextChanged += new EventHandler(txtTransCharge_tbTextChanged);
            this.btnForward.Click += new EventHandler(btnForward1_Click);
            this.btnSetDW.Click += new EventHandler(btnSetDW_Click);
            this.btnSetPart.Click += new EventHandler(btnSetPart_Click);
            this.btnSetZL.Click += new EventHandler(btnSetZL_Click);
           // this.TenantSelector.TenantSelectorAfterEvent += new UcTenantSelector.TenantSelectorAfterEventHandler(TenantSelectorAfter);
           //this.txtSupply.TextChanged+=new EventHandler(txtSupply_TextChanged);
            //右键复制菜单
           this.ToolStripMenuItemDelete.Click += new EventHandler(dgDelete_Click);
           //this.cmsDg.ItemClicked += new ToolStripItemClickedEventHandler(cmsDg_ItemClicked);
        }
     
        void cmsDg_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewCell oCell = null;

            if (e.ClickedItem == this.ToolStripMenuItemDelete)
            {
                if (dgDetail.CurrentRow != null && !dgDetail.CurrentRow.IsNewRow)
                {
                    if (dgDetail.CurrentRow.Tag != null)
                    {
                        Master.Details.Remove(dgDetail.CurrentRow.Tag as BaseDetail);
                    }
                    dgDetail.Rows.Remove(dgDetail.CurrentRow);

                }
            }
            else if (e.ClickedItem == this.ToolStripMenuItemPaste)
            {


                DataGridViewRow oSelectRow = dgDetail.Rows[dgDetail.Rows.Add()];
                foreach (DataGridViewColumn oColumn in dgDetail.Columns)
                {
                    if (oColumn.Name != this.colTempQty.Name)
                    {
                        oSelectRow.Cells[oColumn.Name].Value = this.dgDetail.CurrentRow.Cells[oColumn.Name].Value;
                        oSelectRow.Cells[oColumn.Name].Tag = this.dgDetail.CurrentRow.Cells[oColumn.Name].Tag;
                    }
                }

            }

        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //#region 选择部位
            //if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
            //{
            //    if (this.TenantSelector.SelectedProject == null)
            //    {
            //        ShowMessage("请选择租赁单位");
            //        this.TenantSelector.Focus();
            //        return;
            //    }
            //    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            //    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree(), this.TenantSelector.SelectedProject);
            //    frm.ProjectInfo = this.TenantSelector.SelectedProject;
            //    frm.IsTreeSelect = true;
            //    frm.ShowDialog();
            //    if (frm.SelectResult.Count > 0)
            //    {
            //        TreeNode root = frm.SelectResult[0];

            //        GWBSTree task = root.Tag as GWBSTree;
            //        if (task != null)
            //        {
            //            this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Value = task.Name;
            //            this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Tag = task;
            //            this.txtCode.Focus();
            //        }
            //    }
            //}
            //#endregion
            //#region 选择科目
            //else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSubject.Name))
            //{
            //    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            //    frm.ProjectInfo = this.TenantSelector.SelectedProject;
            //    frm.ShowDialog();
            //    CostAccountSubject cost = frm.SelectAccountSubject;
            //    if (cost != null)
            //    {
            //        if (dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null)
            //        {
            //            this.dgDetail.CurrentRow.Cells[colSubject.Name].Tag = cost;
            //            this.dgDetail.CurrentRow.Cells[colSubject.Name].Value = cost.Name;
            //        }
            //        else
            //        {
            //            ShowMessage("请先选择物资信息！");
            //            return;
            //        }
            //    }
            //    this.txtCode.Focus();
            //}
            //#endregion
            //#region 选择使用队伍
            //else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colBorrowUnit.Name))
            //{
            //    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
            //    VCommonSupplierRelationSelect comSelect = new VCommonSupplierRelationSelect();
            //    comSelect.OpenSelectView("", CommonUtil.SupplierCatCode3);
            //    IList list = comSelect.Result;
            //    if (list != null && list.Count > 0)
            //    {
            //        SupplierRelationInfo relInfo = list[0] as SupplierRelationInfo;
            //        this.dgDetail.CurrentRow.Cells[colBorrowUnit.Name].Tag = relInfo;
            //        this.dgDetail.CurrentRow.Cells[colBorrowUnit.Name].Value = relInfo.SupplierInfo.Name;
            //    }
            //    this.txtCode.Focus();
            //}
            //#endregion
        }
        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
              Material oMaterial=null;
            bool validity = true;
            string expression = string.Empty ;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            string oldExpression;
            if (colName == colPrice.Name || colName == colCollQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        ShowMessage("请输入数字！");
                        //dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value = null;
                        dgDetail.BeginEdit(true);
                    }
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        ShowMessage("请输入数字！");
                        dgDetail.BeginEdit(true);
                    }
                }
                //根据单价和数量计算金额  
                object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value;
                decimal sumMoney = 0;
                decimal sumqty = 0;
                DataGridViewCell oCell = null;
                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colCollQuantity.Name].Value;
                    if (quantity == null) quantity = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                }

                txtSumQuantity.Text = sumqty.ToString("N4");
                if (dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value != null)
                {
                    //计算当前的全部费用金额
                    foreach (CostColumnInfo oCostColumnInfo in lstCostColumnInfo)
                    {
                        oMaterial = dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag as Material;
                        expression = GetCostExpression(oCostColumnInfo.CostType, oMaterial, out oldExpression);
                        if (!string.IsNullOrEmpty(expression))
                        {
                            string qty = ClientUtil.ToString(ClientUtil.ToDecimal(this.dgDetail.CurrentRow.Cells[colCollQuantity.Name].Value) * ClientUtil.ToDecimal(this.dgDetail.CurrentRow.Cells[this.colLength.Name].Value));
                            if (!string.IsNullOrEmpty(expression))
                            {
                                try
                                {
                                    expression = expression.Replace(string.Format("[{0}]", ConstCollectionName), qty);
                                    this.dgDetail.CurrentRow.Cells[oCostColumnInfo.CostType].Value = GetCalResult(qty,ref  expression);
                                }
                                catch (Exception ex)
                                {
                                    string sMsg = string.Join(";", Regex.Matches(expression, "\\[[^\\[\\]]+\\]").OfType<Match>().Select(a => a.Value).ToArray());
                                    if (!string.IsNullOrEmpty(sMsg))
                                    {
                                        this.ShowMessage(string.Format("合同【{0}】中物资【{1}】定义发料【{2}】公式【{3}】解析后[{4}](可能因为【{5}】没有定义或无法识别)",
                                        OrderMaster.Code, oMaterial.Code, oCostColumnInfo.CostType, oldExpression, expression, sMsg));
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
                        else
                        {
                            this.dgDetail.CurrentRow.Cells[oCostColumnInfo.CostType].Value = 0;
                        }
                    }
                }
                sumMoney = ClientUtil.ToDecimal(txtTransCharge.Text);
                foreach (DataGridViewRow oRow in dgDetail.Rows)
                {
                    if (!oRow.IsNewRow)
                    {
                        foreach (CostColumnInfo oCostColumnInfo in lstCostColumnInfo)
                        {
                            oCell = oRow.Cells[oCostColumnInfo.ColumnName];
                            if (oCell.Value != null)
                            {
                                sumMoney += ClientUtil.ToDecimal(oCell.Value);
                            }
                        }
                    }
                }

                txtSumMoney.Text = sumMoney.ToString("N2");

            }
            //else if (colLength.Name == colName || colType.Name == colName)
            //{
            //    if (colLength.Name == colName)
            //    {
            //        if (dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value != null)
            //        {
            //            string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colLength.Name].Value.ToString();
            //            validity = CommonMethod.VeryValid(temp_quantity);
            //            if (validity == false)
            //            {
            //                ShowMessage("请输入数字！");
            //                dgDetail.BeginEdit(true);
            //            }
            //        }
            //    }
            //    else if (colType.Name == colName)
            //    {
            //        dgDetail.Rows[e.RowIndex].Cells[colLength.Name].Value = GetLength(dgDetail.Rows[e.RowIndex].Cells[colType.Name].Value);
            //    }
            //}
        }
        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow oCurrentRow = e.Row;
            if (oCurrentRow == null || oCurrentRow.IsNewRow) return;
            if (oCurrentRow.Tag != null)
            {
                MatHireCollectionDetail oMatHireCollectionDetail = e.Row.Tag as MatHireCollectionDetail;
                Master.Details.Remove(oMatHireCollectionDetail);
                lstDelDetial.Add(oMatHireCollectionDetail);
            }
            this.dgDetail.Rows.Remove(oCurrentRow);
        }

        void dgDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDetail.CurrentRow;
           
            if (dr == null || dr.IsNewRow) return;
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                dgDetail.Rows.Remove(dr);
                Hashtable ht = new Hashtable();
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
                                    if (!string.IsNullOrEmpty(oOrderDetailCostSetItem.ApproachExpression) &&
                                        !string.IsNullOrEmpty(oOrderDetailCostSetItem.ApproachExpression.Trim()) &&
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
                    if  (  isDeleteAll||!ht.ContainsKey(lstCostColumnInfo[i].CostType))
                    {
                        this.dgDetail.Columns.Remove(lstCostColumnInfo[i].CostType);
                        lstCostColumnInfo.RemoveAt(i);
                    }
                }
           
                if (dr.Tag != null)
                {
                    MatHireCollectionDetail oMatHireCollectionDetail = dr.Tag as MatHireCollectionDetail;
                    this.lstDelDetial.Add(oMatHireCollectionDetail);
                    Master.Details.Remove(oMatHireCollectionDetail);
                }
            }
        }
        void btnForward1_Click(object sender, EventArgs e)
        {
            DataGridViewRow oRow = null;
            MatHireOrderMaster oTempOrderMaster = null;
            string[] arrMaterialCodes;
            int i = 0;
            VMaterialHireOrderSelectMaterial oVMaterialHireOrderSelector = new VMaterialHireOrderSelectMaterial(this.MatHireType);
            oVMaterialHireOrderSelector.ShowDialog();
            if (oVMaterialHireOrderSelector.Result != null && oVMaterialHireOrderSelector.Result.Count > 0)
            {
                oTempOrderMaster = oVMaterialHireOrderSelector.Result[0] as MatHireOrderMaster;
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
                        if (OrderMaster==null || OrderMaster.Id != oTempOrderMaster.Id)//合同不相同清除费用列
                        {
                            OrderMaster = oTempOrderMaster;
                            dgDetail.Rows.Clear();
                            #region 清除
                            foreach (CostColumnInfo item in lstCostColumnInfo)
                            {
                                dgDetail.Columns.Remove(item.ColumnName);
                            }
                            lstCostColumnInfo.Clear();
                            #endregion
                            RefleshColumn(null);
                        }
                        else
                        {
                            OrderMaster = oTempOrderMaster;
                            RefleshColumn(null);
                        }
                        arrMaterialCodes = dgDetail.Rows.OfType<DataGridViewRow>().Where(a => a.IsNewRow == false ).Select(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value)).ToArray();
                        #region 明细
                        foreach (MatHireOrderDetail oOrderDetail in OrderMaster.TempData)
                        {
                            if (!oOrderDetail.IsSelect) continue;//选中合同明细
                            if (arrMaterialCodes.Length > 0 && arrMaterialCodes.Contains(oOrderDetail.MaterialCode))//(dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value) == oOrderDetail.MaterialCode) != null)//不出现重复物质行
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
                            oRow.Cells[colLeftQty.Name].Value = oOrderDetail.TempData3;
                            //oRow.Cells[colUsedPart.Name].Tag = oOrderDetail.UsedPart;
                            //oRow.Cells[colUsedPart.Name].Value = oDailyPlan.UsedPartName;
                            //oRow.Cells[this.colBorrowUnit.Name].Tag = oDailyPlan.UsedRank;
                            //oRow.Cells[this.colBorrowUnit.Name].Value = oDailyPlan.UsedRankName;
                            // oMatHireOrderDetail = lstMatHireOrderDetail.FirstOrDefault(a => a.MaterialResource.Id == oDailyPlan.MaterialResource.Id);
                            oRow.Cells[colPrice.Name].Value = oOrderDetail.Price;
                            oRow.Cells[colPrice.Name].Tag = oOrderDetail.Id;
                            oRow.Cells[colLength.Name].Value = 1;
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
                            #region 清除
                            foreach (CostColumnInfo item in lstCostColumnInfo)
                            {
                                dgDetail.Columns.Remove(item.ColumnName);
                            }
                            lstCostColumnInfo.Clear();
                            #endregion
                            RefleshColumn( oMatHireOrderDetail);
                            #region 明细
                            AddRow(oMatHireOrderDetail);
                            #endregion
                        }
                    }
                    #region 废弃
                    //foreach (MatHireOrderDetail oOrderDetail in OrderMaster.TempData)
                    //{
                    //    if (!oOrderDetail.IsSelect) continue;
                    //    if (MatHireType == EnumMatHireType.普通料具)//普通料具相同物资不能出现重复
                    //    {
                    //        if (dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value) == oOrderDetail.MaterialCode) != null)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value) == oOrderDetail.MaterialCode) != null)
                    //        {
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            AddRow(oOrderDetail);
                    //            break;
                    //        }
                    //    }
                    //     i = this.dgDetail.Rows.Add();
                    //    oRow = this.dgDetail.Rows[i];
                    //    oRow.Cells[colMaterialCode.Name].Tag = oOrderDetail.MaterialResource;
                    //    oRow.Cells[colMaterialCode.Name].Value = oOrderDetail.MaterialCode;
                    //    oRow.Cells[colMaterialName.Name].Value = oOrderDetail.MaterialName;
                    //    oRow.Cells[colMaterialSpec.Name].Value = oOrderDetail.MaterialSpec;
                    //    oRow.Cells[colUnit.Name].Tag = oOrderDetail.MatStandardUnit;
                    //    oRow.Cells[colUnit.Name].Value = oOrderDetail.MatStandardUnitName;
                    //    //oRow.Cells[colUsedPart.Name].Tag = oOrderDetail.UsedPart;
                    //    //oRow.Cells[colUsedPart.Name].Value = oDailyPlan.UsedPartName;
                    //    //oRow.Cells[this.colBorrowUnit.Name].Tag = oDailyPlan.UsedRank;
                    //    //oRow.Cells[this.colBorrowUnit.Name].Value = oDailyPlan.UsedRankName;
                    //    // oMatHireOrderDetail = lstMatHireOrderDetail.FirstOrDefault(a => a.MaterialResource.Id == oDailyPlan.MaterialResource.Id);
                    //    oRow.Cells[colPrice.Name].Value = oOrderDetail.Price;
                    //    oRow.Cells[colPrice.Name].Tag = oOrderDetail.Id;
                    //    oRow.Cells[colLength.Name].Value = 1;

                    //}
                    #endregion
                    #endregion
                    #endregion
                }
            }
           
        }
        void RefleshColumn(MatHireOrderDetail oMatHireOrderDetail)
        {
            DataGridViewTextBoxColumn oColumn = null;
            int iInsertColumn = GetInsertColumnIndex();
            CostColumnInfo oCostColumnInfo = null;
            Hashtable hsCostType = new Hashtable();
           

            #region 添加列
            if (OrderMaster != null)
            {
                if (string.IsNullOrEmpty(Master.Id))
                {
                    if (oMatHireOrderDetail == null)
                    {
                        #region
                        //foreach (OrderMasterCostSetItem oOrderMasterCostSetItem in OrderMaster.BasiCostSets)
                        //{
                        //    if (oOrderMasterCostSetItem.ApproachCalculation == 1)
                        //    {
                        //        oColumn = new DataGridViewTextBoxColumn();
                        //        oColumn.HeaderText = oOrderMasterCostSetItem.MatCostType;
                        //        oColumn.Name = oOrderMasterCostSetItem.MatCostType;
                        //        oColumn.ReadOnly = true;
                        //        dgDetail.Columns.Insert(iInsertColumn + lstCostColumnInfo.Count, oColumn);
                        //        oCostColumnInfo = new CostColumnInfo()
                        //        {
                        //            ColumnName = oOrderMasterCostSetItem.MatCostType,
                        //            ColumnIndex = oColumn.Index,
                        //            CostType = oOrderMasterCostSetItem.MatCostType
                        //        };
                        //        lstCostColumnInfo.Add(oCostColumnInfo);
                        //    }
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
                                    if (!string.IsNullOrEmpty(oOrderDetailCostSetItem.ApproachExpression) && !string.IsNullOrEmpty(oOrderDetailCostSetItem.ApproachExpression.Trim()))
                                    {
                                        if (!hsCostType.ContainsKey(oOrderDetailCostSetItem.CostType))
                                        {
                                            hsCostType.Add(oOrderDetailCostSetItem.CostType, null);
                                            oColumn = new DataGridViewTextBoxColumn();
                                            oColumn.DefaultCellStyle.Format = "N6";
                                            oColumn.HeaderText = oOrderDetailCostSetItem.CostType;
                                            oColumn.Name = oOrderDetailCostSetItem.CostType;
                                            oColumn.ReadOnly = true;
                                            oColumn.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                                            dgDetail.Columns.Insert(iInsertColumn + lstCostColumnInfo.Count, oColumn);
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
                    }
                    else//从合同明细中取费用类型  主要针对钢管碗扣 每次只选择一种物资 所以获取费用信息只取合同明细上的 公式定义
                    {
                        foreach (OrderDetailCostSetItem oOrderDetailCostSetItem in oMatHireOrderDetail.BasicDtlCostSets)
                        {
                            if (!string.IsNullOrEmpty(oOrderDetailCostSetItem.ApproachExpression))//获取有入场公式的费用明细
                            {
                                oColumn = new DataGridViewTextBoxColumn();
                                oColumn.DefaultCellStyle.Format = "N6";
                                oColumn.HeaderText = oOrderDetailCostSetItem.CostType;
                                oColumn.Name = oOrderDetailCostSetItem.CostType;
                                oColumn.ReadOnly = true;
                                oColumn.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                                dgDetail.Columns.Insert(iInsertColumn + lstCostColumnInfo.Count, oColumn);
                                oCostColumnInfo = new CostColumnInfo()
                                {
                                    ColumnName = oOrderDetailCostSetItem.CostType,
                                    ColumnIndex = oColumn.Index,
                                    CostType = oOrderDetailCostSetItem.CostType
                                };
                                lstCostColumnInfo.Add(oCostColumnInfo);
                            }
                        }
                    }
                }
                else
                {
                    foreach (MatHireCollectionDetail oMatHireCollectionDetail in Master.Details)
                    {
                        foreach (MatHireCollectionCostDtl oMatHireCollectionCostDtl in oMatHireCollectionDetail.MatCostDtls)
                        {
                            if (!hsCostType.ContainsKey(oMatHireCollectionCostDtl.CostType))
                            {
                                oColumn = new DataGridViewTextBoxColumn();
                                oColumn.DefaultCellStyle.Format = "N6";
                                oColumn.HeaderText = oMatHireCollectionCostDtl.CostType;
                                oColumn.Name = oMatHireCollectionCostDtl.CostType;
                                oColumn.ReadOnly = true;
                                oColumn.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                                dgDetail.Columns.Insert(iInsertColumn + lstCostColumnInfo.Count, oColumn);
                                oCostColumnInfo = new CostColumnInfo()
                                {
                                    ColumnName = oMatHireCollectionCostDtl.CostType,
                                    ColumnIndex = oColumn.Index,
                                    CostType = oMatHireCollectionCostDtl.CostType
                                };
                                hsCostType.Add(oMatHireCollectionCostDtl.CostType, null);
                                lstCostColumnInfo.Add(oCostColumnInfo);
                            }
                        }
                    }
                }
                dgDetail.SetColumnsReadOnly( lstCostColumnInfo.Select(a=>a.ColumnName).ToArray() );
            }
            #endregion
        }
        public int GetInsertColumnIndex()
        {
            int iIndex = colPrice.Index+1;
           
            return iIndex ;
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
            DataGridViewRow oRow = null;
            int i = 0;
            foreach (string sType in arrWKType)
            {
                 i = this.dgDetail.Rows.Add();
                oRow = this.dgDetail.Rows[i];
                oRow.Cells[colMaterialCode.Name].Tag = oOrderDetail.MaterialResource;
                oRow.Cells[colMaterialCode.Name].Value = oOrderDetail.MaterialCode;
                oRow.Cells[colMaterialName.Name].Value = oOrderDetail.MaterialName;
                oRow.Cells[colMaterialSpec.Name].Value = oOrderDetail.MaterialSpec;
                oRow.Cells[colUnit.Name].Tag = oOrderDetail.MatStandardUnit;
                oRow.Cells[colUnit.Name].Value = oOrderDetail.MatStandardUnitName;
                oRow.Cells[colPrice.Name].Value = oOrderDetail.Price;
                oRow.Cells[colPrice.Name].Tag = oOrderDetail.Id;
                oRow.Cells[colType.Name].Value = sType;
                oRow.Cells[colLength.Name].Value = GetLength(sType);
                oRow.Cells[colLeftQty.Name].Value = oOrderDetail.TempData3;
            }
        }
        void AddGGRow(MatHireOrderDetail oOrderDetail)
        {
            DataGridViewRow oRow = null;
            int i = 0;
            for (decimal j = 60; j >=10; j--)
            {
                 i = this.dgDetail.Rows.Add();
                oRow = this.dgDetail.Rows[i];
                oRow.Cells[colMaterialCode.Name].Tag = oOrderDetail.MaterialResource;
                oRow.Cells[colMaterialCode.Name].Value = oOrderDetail.MaterialCode;
                oRow.Cells[colMaterialName.Name].Value = oOrderDetail.MaterialName;
                oRow.Cells[colMaterialSpec.Name].Value = oOrderDetail.MaterialSpec;
                oRow.Cells[colUnit.Name].Tag = oOrderDetail.MatStandardUnit;
                oRow.Cells[colUnit.Name].Value = oOrderDetail.MatStandardUnitName;
                oRow.Cells[colPrice.Name].Value = oOrderDetail.Price;
                oRow.Cells[colPrice.Name].Tag = oOrderDetail.Id;
                oRow.Cells[colLength.Name].Value = string.Format("{0:N1}", j / 10);
                oRow.Cells[colLeftQty.Name].Value = oOrderDetail.TempData3;

            }
        }

        //void btnForward_Click(object sender, EventArgs e)
        //{
        //    MatHireOrderMaster oOrder = null;
        //    List<string> lstMaterialCodes = null;
        //    DailyPlanMaster oDailyPlanMaster = null;
        //    List<DailyPlanDetail> lstDailyPlanDetail = null;
        //    MatHireOrderDetail oMatHireOrderDetail = null;
        //    List<MatHireOrderDetail> lstMatHireOrderDetail=null;
        //    DataGridViewRow oRow=null;
        //    #region  验证
        //    if (txtSupply.Result == null || txtSupply.Result.Count == 0)
        //    {
        //        ShowMessage("请选择料具出租方");
        //        this.txtSupply.Focus();
        //        return;
        //    }
        //    else if (this.TenantSelector.SelectedProject == null)
        //    {
        //        ShowMessage("请选择租赁方");
        //        this.TenantSelector.Focus();
        //        return;
        //    }
        //    oOrder = GetOrderMaster();
        //    if (oOrder == null)
        //    {
        //        ShowMessage(string.Format("出租方[{0}]与租赁方[{1}]未签订租赁合同,无法收料(检尺)", txtSupply.Text, TenantSelector.SelectedProject.Name));
        //        return;
        //    }
        //    if (MatHireType == EnumMatHireType.普通料具)
        //    {
        //        lstMaterialCodes = oOrder.Details.OfType<MatHireOrderDetail>().
        //            Where(a => !(a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateGGCode) || a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateWKCode))).
        //            Select(a => a).Select(a => a.MaterialCode).ToList();
        //    }
        //    else if (MatHireType == EnumMatHireType.钢管)
        //    {
        //        lstMaterialCodes = oOrder.Details.OfType<MatHireOrderDetail>().
        //            Where(a => a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateGGCode)).
        //            Select(a => a.MaterialCode).ToList();
        //    }
        //    else if (MatHireType == EnumMatHireType.碗扣)
        //    {
        //        lstMaterialCodes = oOrder.Details.OfType<MatHireOrderDetail>().
        //            Where(a => a.MaterialResource.Code.StartsWith(CommonUtil.MaterialCateWKCode)).
        //            Select(a => a.MaterialCode).ToList();
        //    }
        //    if (lstMaterialCodes == null || lstMaterialCodes.Count == 0)
        //    {
        //        ShowMessage(string.Format("出租方[{0}]与租赁方[{1}]签订租赁合同中不包含[{2}]物资,无法收料(检尺)", txtSupply.Text, TenantSelector.SelectedProject.Name, Enum.GetName(typeof(EnumMatHireType), MatHireType)));
        //        return;
        //    }
        //    #endregion
        //    VDailyPlanSelector theVDailyPlanSelector = new VDailyPlanSelector();
        //    theVDailyPlanSelector.ProjectInfo = this.TenantSelector.SelectedProject;
        //    theVDailyPlanSelector.MaterialCodes = lstMaterialCodes;
        //    theVDailyPlanSelector.selectType = 0;
        //    theVDailyPlanSelector.ShowDialog();
        //    IList list = theVDailyPlanSelector.Result;
        //    if (list == null || list.Count == 0) return;
        //    oDailyPlanMaster = list[0] as DailyPlanMaster;
        //    lstDailyPlanDetail = oDailyPlanMaster.Details.OfType<DailyPlanDetail>().Where(a => a.IsSelect).ToList();
        //    if (lstDailyPlanDetail == null || lstDailyPlanDetail.Count == 0) return;
        //    this.btnForward.Enabled = false;
        //    this.TenantSelector.Enabled = false;
        //    this.OrderMaster = oOrder;
        //    lstMatHireOrderDetail=OrderMaster.Details.OfType<MatHireOrderDetail>().ToList();
        //    if (ClientUtil.ToString(customEdit1.Tag) != oDailyPlanMaster.Id)
        //    {
        //        dgDetail.Rows.Clear();
        //        customEdit1.Tag=oDailyPlanMaster.Id;
        //        customEdit1.Text=oDailyPlanMaster.Code;
        //    }
        //    foreach (DailyPlanDetail oDailyPlan in lstDailyPlanDetail)
        //    {
        //        if (MatHireType == EnumMatHireType.普通料具)//普通料具相同物资不能出现重复
        //        {
        //            if (dgDetail.Rows.OfType<DataGridViewRow>().FirstOrDefault(a => ClientUtil.ToString(a.Cells[colMaterialCode.Name].Value) == oDailyPlan.MaterialCode) != null)
        //            {
        //                continue;
        //            }
        //        }
        //        int i = this.dgDetail.Rows.Add();
        //        oRow = this.dgDetail.Rows[i];
        //        oRow.Cells[colMaterialCode.Name].Tag = oDailyPlan.MaterialResource;
        //        oRow.Cells[colMaterialCode.Name].Value = oDailyPlan.MaterialCode;
        //        oRow.Cells[colMaterialName.Name].Value = oDailyPlan.MaterialName;
        //        oRow.Cells[colMaterialSpec.Name].Value = oDailyPlan.MaterialSpec;
        //        oRow.Cells[colUnit.Name].Tag = oDailyPlan.MatStandardUnit;
        //        oRow.Cells[colUnit.Name].Value = oDailyPlan.MatStandardUnitName;
        //        oRow.Cells[colUsedPart.Name].Tag = oDailyPlan.UsedPart;
        //        oRow.Cells[colUsedPart.Name].Value = oDailyPlan.UsedPartName;
        //        oRow.Cells[this.colBorrowUnit.Name].Tag = oDailyPlan.UsedRank;
        //        oRow.Cells[this.colBorrowUnit.Name].Value = oDailyPlan.UsedRankName;
        //        oMatHireOrderDetail = lstMatHireOrderDetail.FirstOrDefault(a => a.MaterialResource.Id == oDailyPlan.MaterialResource.Id);
        //        oRow.Cells[colPrice.Name].Value = oMatHireOrderDetail.Price;
        //        oRow.Cells[colPrice.Name].Tag = oMatHireOrderDetail.Id;
        //        if (MatHireType == EnumMatHireType.碗扣)
        //        {
        //            oRow.Cells[colType.Name].Value =arrWKType[0];
        //            oRow.Cells[colLength.Name].Value = GetLength(oRow.Cells[colType.Name].Value);
        //        }
        //    }
        //}
        void btnSetDW_Click(object sender, EventArgs e)
        {
            DataGridViewCell oCell = dgDetail.Rows.OfType<DataGridViewRow>().Select(a => a.Cells[colBorrowUnit.Name]).FirstOrDefault(a => a.Tag != null);
            if (oCell == null)
            {
                if (dgDetail.Rows.Count > 0 && dgDetail.Rows[0].IsNewRow == false)
                {
                    ShowMessage("请选择对应队伍");
                    dgDetail.Rows[0].Cells[colBorrowUnit.Name].Selected = true;
                }
                else
                {
                    ShowMessage("请选择添加明细,再添加对应的队伍");
                }
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (ClientUtil.ToString(dgDetail[this.colBorrowUnit.Name, 0].Value) != "")
                {
                    var.Cells[colBorrowUnit.Name].Value = oCell.Value;
                    var.Cells[colBorrowUnit.Name].Tag = oCell.Tag;
                }
            }
        }
        void btnSetPart_Click(object sender, EventArgs e)
        {
            DataGridViewCell oCell = dgDetail.Rows.OfType<DataGridViewRow>().Select(a => a.Cells[colUsedPart.Name]).FirstOrDefault(a => a.Tag != null);
            if (oCell == null)
            {
                if (dgDetail.Rows.Count > 0 && dgDetail.Rows[0].IsNewRow == false)
                {
                    ShowMessage("请选择对应部位");
                    dgDetail.Rows[0].Cells[colUsedPart.Name].Selected = true;
                }
                else
                {
                    ShowMessage("请选择添加明细,再添加对应的部位");
                }
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (ClientUtil.ToString(dgDetail[this.colUsedPart.Name, 0].Value) != "")
                {
                    var.Cells[colUsedPart.Name].Value = oCell.Value;
                    var.Cells[colUsedPart.Name].Tag = oCell.Tag;
                }
            }
        }
        void btnSetZL_Click(object sender, EventArgs e)
        {
            DataGridViewCell oCell = dgDetail.Rows.OfType<DataGridViewRow>().Select(a => a.Cells[colSubject.Name]).FirstOrDefault(a => a.Tag != null);
            if (oCell == null)
            {
                if (dgDetail.Rows.Count > 0 && dgDetail.Rows[0].IsNewRow == false)
                {
                    ShowMessage("请选择对应科目");
                    dgDetail.Rows[0].Cells[colSubject.Name].Selected = true;
                }
                else
                {
                    ShowMessage("请选择添加明细,再添加对应的科目");
                }
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if (ClientUtil.ToString(dgDetail[this.colSubject.Name, 0].Value) != "")
                {
                    var.Cells[colSubject.Name].Value = oCell.Value;
                    var.Cells[colSubject.Name].Tag = oCell.Tag;
                }
            }
        }
        void txtTransCharge_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string TransCharge = this.txtTransCharge.Text;
            validity = CommonMethod.VeryValid(TransCharge);
            if (validity == false)
            {
                ShowMessage("请输入数字！");
                return;
            }
        }
        //void txtSupply_TextChanged(object sender, EventArgs e)
        //{
        //    MatHireOrderMaster oOrder=null;
        //    DataGridViewTextBoxColumn oColumn = null;
        //    int iInsertColumn = dgDetail.Columns[colPrice.Name].Index+1;
        //    CostColumnInfo oCostColumnInfo = null;
        //    Hashtable htColumn = new Hashtable();
        //    #region 清除
           
        //    //foreach (MatHireCollectionDetail oDetail in Master.Details)
        //    //{
        //    //   if(!string.IsNullOrEmpty(oDetail.Id)) lstDelDetial.Add(oDetail);
        //    //}
        //   // Master.Details.Clear();
        //    //dgDetail.Rows.Clear();
        //    foreach (CostColumnInfo item in lstCostColumnInfo)
        //    {
        //        dgDetail.Columns.Remove(item.ColumnName);
        //    }
        //    lstCostColumnInfo.Clear();
        //   #endregion
        //    #region 判断
        //    if (txtSupply.Result == null || txtSupply.Result.Count == 0)
        //    {
        //      //  ShowMessage("请选择料具出租方");
        //        this.txtSupply.Focus();
        //        return;
        //    }
        //    else if (this.TenantSelector.SelectedProject == null)
        //    {
        //       // ShowMessage("请选择租赁方");
        //        this.TenantSelector.Focus();
        //        return;
        //    }
        //    oOrder = GetOrderMaster();
        //    if (oOrder == null)
        //    {
        //        ShowMessage(string.Format("出租方[{0}]与租赁方[{1}]未签订租赁合同,无法收料(检尺)", txtSupply.Text, TenantSelector.SelectedProject.Name));
        //        return;
        //    }
        //    #endregion
        //    #region 添加列
        //    OrderMaster = oOrder;
        //    txtBalRule.Text = OrderMaster.BalRule;
        //    txtContractNo.Text = OrderMaster.OriginalContractNo;
        //    if (string.IsNullOrEmpty(Master.Id))
        //    {
        //        foreach (OrderMasterCostSetItem oOrderMasterCostSetItem in oOrder.BasiCostSets)
        //        {
        //            if (oOrderMasterCostSetItem.ApproachCalculation == 1)
        //            {
        //                oColumn = new DataGridViewTextBoxColumn();
        //                oColumn.HeaderText = oOrderMasterCostSetItem.MatCostType;
        //                oColumn.Name = oOrderMasterCostSetItem.MatCostType;
        //                oColumn.ReadOnly = true;
        //                dgDetail.Columns.Insert(iInsertColumn, oColumn);
        //                oCostColumnInfo = new CostColumnInfo()
        //                {
        //                    ColumnName = oOrderMasterCostSetItem.MatCostType,
        //                    ColumnIndex = oColumn.Index,
        //                    CostType = oOrderMasterCostSetItem.MatCostType
        //                };
        //                lstCostColumnInfo.Add(oCostColumnInfo);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (MatHireCollectionDetail oMatHireCollectionDetail in Master.Details)
        //        {
        //            foreach (MatHireCollectionCostDtl oMatHireCollectionCostDtl in oMatHireCollectionDetail.MatCostDtls)
        //            {
        //                if (!htColumn.ContainsKey(oMatHireCollectionCostDtl.CostType))
        //                {
        //                    oColumn = new DataGridViewTextBoxColumn();
        //                    oColumn.HeaderText = oMatHireCollectionCostDtl.CostType;
        //                    oColumn.Name = oMatHireCollectionCostDtl.CostType;
        //                    oColumn.ReadOnly = true;
        //                    dgDetail.Columns.Insert(iInsertColumn, oColumn);
        //                    oCostColumnInfo = new CostColumnInfo()
        //                    {
        //                        ColumnName = oMatHireCollectionCostDtl.CostType,
        //                        ColumnIndex = oColumn.Index,
        //                        CostType = oMatHireCollectionCostDtl.CostType
        //                    };
        //                    htColumn.Add(oMatHireCollectionCostDtl.CostType, null);
        //                    lstCostColumnInfo.Add(oCostColumnInfo);
        //                }
        //            }
        //        }
        //    }
        //    #endregion
        //}
      
        //void TenantSelectorAfter(UcTenantSelector a)
        //{
        //    this.ProjectInfo = a.SelectedProject;
        //    if (Master != null)
        //    {
        //        Master.ProjectId = ProjectInfo == null ? "" : ProjectInfo.Id;
        //        Master.ProjectName = ProjectInfo == null ? "" : ProjectInfo.Name;
        //    }
        //    //if (this.ProjectInfo!=null &&txtSupply.Result!=null && txtSupply.Result.Count > 0)
        //    //{
        //    //    //txtSupply_TextChanged(null, null);
        //    //}
        //}
        void tsmiDel_Click(object sender, EventArgs e)
        {
        }
        #endregion
        #region 固定代码
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
                this.Master = new MatHireCollectionMaster();
                Master.CreatePerson = ConstObject.LoginPersonInfo;
                Master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                Master.CreateDate = ConstObject.LoginDate;
                Master.CreateYear = ConstObject.LoginDate.Year;
                Master.CreateMonth = ConstObject.LoginDate.Month;
                Master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                Master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                Master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                Master.HandlePerson = ConstObject.LoginPersonInfo;
                Master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                Master.DocState = DocumentState.Edit;
                Master.BalState = 0;//结算状态 0：未结算  1; 已结算
                Master.MatHireType = this.MatHireType;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
            
                //归属项目
                //if (this.ProjectInfo != null)
                //{
                //    //txtProject.Tag = projectInfo;
                //    //txtProject.Text = projectInfo.Name;
                //    Master.ProjectId = ProjectInfo.Id;
                //    Master.ProjectName = ProjectInfo.Name;
                //}
                txtContractNo.Focus();
            }
            catch (Exception ex)
            {
                ShowMessage("新建单据错误：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (this.Master.DocState == DocumentState.Edit || Master.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                Master = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(Master.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(Master.DocState));
            ShowMessage(message);
            return false;
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
                        Master = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(Master.Id);
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                Master = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(Master.Id);
                if (Master.DocState == DocumentState.Valid || Master.DocState == DocumentState.Edit)
                {
                    if (!model.MaterialHireMngSvr.DeleteMaterialHireCollectionMaster(Master)) return false;
                    ClearView();
                    return true;
                }
                ShowMessage("此单状态为【" + ClientUtil.GetDocStateName(Master.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                ShowMessage("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public override void Refresh()
        {
            try
            {
                //重新获得当前对象的值
                Master = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(this.Master.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                ShowMessage("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }
        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            txtProjectName.Tag = null;
            txtSupply.Tag = null;
            txtContract.Tag = null;
            this.RefleshColumn(null);
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
            else if (c is CommonSupplier)
            {
                c.Tag = null;
                c.Text = "";
            }
        }
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
                    Master = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(id);
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
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
                    break;
                default:
                    break;
            }
        }
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtSumQuantity, txtContractNo, txtBalRule, this.txtSupply, this.txtProjectName, txtContract, txtSumMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialCode.Name, colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colPrice.Name,colUsedPart.Name,
               colSubject.Name,colBorrowUnit.Name,colType.Name,colLength.Name,colLeftQty.Name};
            dgDetail.SetColumnsReadOnly(lockCols);
            
        }
        #endregion
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            Hashtable htStockQty = null;
            try
            {
                if (!IsAllSet())//检查收料的料具对应的合同明细上是否设置了计价方式
                {
                    return false;//
                }
                if (!ViewToModel()) return false;
               // if (!ValidKC()) return false;
                string MaterialIDs =string.Join ("," ,Master.Details.OfType<MatHireCollectionDetail>().Select(a => string.Format("'{0}'",a.MaterialResource.Id)).Distinct().ToArray());
                 htStockQty =string.IsNullOrEmpty(MaterialIDs)?null: model.MaterialHireMngSvr.GetPreviousJC(Master.CreateDate,  MaterialIDs, Master.ProjectId, Master.TheSupplierRelationInfo.Id);
                 if (htStockQty!=null &&htStockQty.Count > 0)
                {
                    foreach (MatHireCollectionDetail oDetail in Master.Details)
                    {
                        if (htStockQty.ContainsKey(oDetail.MaterialResource.Id))
                        {
                            oDetail.BeforeStockQty = ClientUtil.ToDecimal(htStockQty[oDetail.MaterialResource.Id]);
                        }
                        else
                        {
                            oDetail.BeforeStockQty = 0;
                        }
                    }
                }
                if (Master.Id == null)
                {
                    Master.DocState = DocumentState.InExecute;
                    Master = model.MaterialHireMngSvr.SaveMaterialHireCollectionMaster(Master, this.CanReturnUp);
                }
                else
                {
                    Master.DocState = DocumentState.InExecute;
                    Master = model.MaterialHireMngSvr.UpdateMaterialHireCollectionMaster(Master, this.lstDelDetial);
                }

                ////插入日志
                ////MStockIn.InsertLogData(Master.Id, "保存", Master.Code, matColMaster.CreatePerson.Name, "料具发料单","");
                txtCode.Text = Master.Code;
                txtCode.Focus();
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception ex)
            {
                ShowMessage("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                if (Master.Id != null)
                {
                    if (model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(Master.Id) == null)
                    {
                        NewView();
                    }
                }
                return false;
            }
        
        }
        public bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                decimal sumExtMoney = 0;
              
                MatHireCollectionDetail theMaterialCollectionDetail = null;
                MatHireCollectionCostDtl theMatHireCollectionCostDtl = null;
                List<MatHireCollectionCostDtl> lstMatHireCollectionCostDtl = null;
                List<string> lstCostType = lstCostColumnInfo.Select(a => a.CostType).ToList();
                Master.TheSupplierRelationInfo = this.txtSupply.Tag as SupplierRelationInfo;
                Master.SupplierName = this.txtSupply.Text;
                Master.ContractCode = txtContract.Text;
                Master.BillCode = txtOldCode.Text;
                if (txtContract.Tag != null)
                {
                    Master.Contract = txtContract.Tag as MatHireOrderMaster; ;
                }
                Master.ProjectName = txtProjectName.Text;
                Master.ProjectId =ClientUtil.ToString( txtProjectName.Tag);
                Master.CreateDate = operDate.Value.Date;
                Master.OldContractNum = this.txtContractNo.Text;
                if (txtTransCharge.Text != "")
                {
                    Master.TransportCharge = ClientUtil.ToDecimal(txtTransCharge.Text);
                }
                Master.Descript = this.txtRemark.Text;
                Master.BalRule = this.txtBalRule.Text;
                Master.Details.Clear();
                #region 明细数据
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    if (var.Tag != null)
                    {
                        theMaterialCollectionDetail = var.Tag as MatHireCollectionDetail;
                    }
                    else
                    {
                        theMaterialCollectionDetail = new MatHireCollectionDetail();
                        theMaterialCollectionDetail.Master = Master;
                       
                    }
                    Master.AddDetail(theMaterialCollectionDetail);
                    //材料
                    theMaterialCollectionDetail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    theMaterialCollectionDetail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    theMaterialCollectionDetail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    theMaterialCollectionDetail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    //计量单位
                    theMaterialCollectionDetail.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    theMaterialCollectionDetail.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    theMaterialCollectionDetail.Quantity = ClientUtil.ToDecimal(var.Cells[colCollQuantity.Name].Value);
                    //theMaterialCollectionDetail.RealQuantity = theMaterialCollectionDetail.Quantity;
                    theMaterialCollectionDetail.MaterialLength = 1;
                    if (MatHireType != EnumMatHireType.普通料具)
                    {
                        theMaterialCollectionDetail.MaterialLength = ClientUtil.ToDecimal(var.Cells[this.colLength.Name].Value);
                        //theMaterialCollectionDetail.Quantity = theMaterialCollectionDetail.RealQuantity * theMaterialCollectionDetail.MaterialLength;
                        theMaterialCollectionDetail.MaterialType = ClientUtil.ToString(var.Cells[this.colType.Name].Value);
                    }
                    decimal tempQty = ClientUtil.ToDecimal(var.Cells[colTempQty.Name].Value);
                    theMaterialCollectionDetail.TempData = tempQty.ToString();
                    theMaterialCollectionDetail.RentalPrice = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    if (IsProject)
                    {
                        //使用部位
                        if (var.Cells[colUsedPart.Name].Value != null)
                        {
                            theMaterialCollectionDetail.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                            theMaterialCollectionDetail.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                            theMaterialCollectionDetail.UsedPartSysCode = theMaterialCollectionDetail.UsedPart.SysCode;
                        }
                        //核算科目
                        if (var.Cells[colSubject.Name].Value != null)
                        {
                            theMaterialCollectionDetail.SubjectGUID = var.Cells[colSubject.Name].Tag as CostAccountSubject;
                            theMaterialCollectionDetail.SubjectName = ClientUtil.ToString(var.Cells[colSubject.Name].Value);
                            theMaterialCollectionDetail.SubjectSysCode = ClientUtil.ToString((var.Cells[colSubject.Name].Tag as CostAccountSubject).SysCode);
                        }
                        //使用队伍
                        if (var.Cells[this.colBorrowUnit.Name].Value != null)
                        {
                            theMaterialCollectionDetail.BorrowUnit = var.Cells[colBorrowUnit.Name].Tag as SupplierRelationInfo;
                            theMaterialCollectionDetail.BorrowUnitName = ClientUtil.ToString(var.Cells[colBorrowUnit.Name].Value);
                        }
                    }
                    theMaterialCollectionDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    #region 费用明细
                    //处理费用明细
                    //先清除已有费用明细
                    //theMaterialCollectionDetail.MatCostDtls.Clear();
                    if (lstCostType != null && lstCostType.Count > 0)
                    {
                        lstMatHireCollectionCostDtl = theMaterialCollectionDetail.MatCostDtls.Where(a => !lstCostType.Contains(a.CostType)).ToList();
                        if (lstMatHireCollectionCostDtl != null && lstMatHireCollectionCostDtl.Count > 0)
                        {
                            theMaterialCollectionDetail.MatCostDtls.RemoveAll(lstMatHireCollectionCostDtl);
                        }
                    }
                    //新增和修改
                   
                    foreach (CostColumnInfo oCostColumnInfo in lstCostColumnInfo)
                    {
                        theMatHireCollectionCostDtl = theMaterialCollectionDetail.MatCostDtls.FirstOrDefault(a => a.CostType == oCostColumnInfo.CostType);
                        if (theMatHireCollectionCostDtl == null)
                        {
                            theMatHireCollectionCostDtl = new MatHireCollectionCostDtl();
                            theMaterialCollectionDetail.AddMatCostDtl(theMatHireCollectionCostDtl);
                            theMatHireCollectionCostDtl.CostType = oCostColumnInfo.CostType;
                            theMatHireCollectionCostDtl.Money = ClientUtil.ToDecimal(var.Cells[oCostColumnInfo.CostType].Value);
                        
                        }
                        theMaterialCollectionDetail.Money = ClientUtil.ToDecimal(var.Cells[oCostColumnInfo.CostType].Value);
                        if (theMatHireCollectionCostDtl.Money != 0)
                        {
                            GetExpressionInfo(theMatHireCollectionCostDtl, theMaterialCollectionDetail.MaterialResource);
                        }
                        sumExtMoney += theMatHireCollectionCostDtl.Money;
                    }
                    #endregion

                }
                #endregion
                Master.SumExtMoney = sumExtMoney;
                #region 运输费
                decimal TransChagre = 0;
                if (txtTransCharge.Text == "")
                {
                    TransChagre = 0;
                }
                else
                {
                    TransChagre = ClientUtil.ToDecimal(txtTransCharge.Text);
                }
                Master.SumExtMoney += TransChagre;
                #endregion
            }
            catch (Exception ex)
            {
                ShowMessage("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
       /// <summary>
       /// 获取公式 理论值 价格
       /// </summary>
       /// <param name="oCostDtl"></param>
       /// <param name="oMaterial"></param>
        public void GetExpressionInfo(MatHireCollectionCostDtl oCostDtl, Material oMaterial)
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
                          !string.IsNullOrEmpty(a.ApproachExpression) && !string.IsNullOrEmpty(a.ApproachExpression.Trim()) &&
                          a.CostType == oCostDtl.CostType);
                    if (oExpressionItem != null)
                    {
                        oCostDtl.Expression = oExpressionItem.ApproachExpression;//获取公式
                        foreach (OrderDetailCostSetItem oItem in oOrderDetail.BasicDtlCostSets.Where(a => a.SetType == Enum.GetName(typeof(SetType), SetType.价格定义) && a.CostType.IndexOf("单价")>-1))
                        {
                            if (oExpressionItem.ApproachExpression.IndexOf(string.Format("[{0}]", oItem.CostType)) > -1)
                            {
                                oCostDtl.Price = oItem.Price;
                                break;
                            }
                        }
                        foreach (OrderDetailCostSetItem oItem in oOrderDetail.BasicDtlCostSets.Where(a => a.SetType == Enum.GetName(typeof(SetType), SetType.价格定义) && a.CostType.IndexOf("理论")>-1))
                        {
                            if (oExpressionItem.ApproachExpression.IndexOf(string.Format("[{0}]", oItem.CostType)) > -1)
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
        public void ModelToView()
        {
            try
            {
                this.txtCode.Text = Master.Code;
                operDate.Value = Master.CreateDate;
                OrderMaster = Master.Contract;
                txtContract.Text = Master.ContractCode;
                txtContract.Tag = Master.Contract;
                this.txtContractNo.Text = Master.OldContractNum;
                this.txtProjectName.Tag = Master.ProjectId;
                this.txtProjectName.Text = Master.ProjectName;
                    txtSupply.Tag = Master.TheSupplierRelationInfo;
                    txtSupply.Text = Master.SupplierName;
                    txtOldCode.Text = Master.BillCode;
               
                //if (Master.TheRank != null)
                //{
                //    txtRank.Result.Clear();
                //    txtRank.Tag = Master.TheRank;
                //    txtRank.Result.Add(Master.TheRank);
                //    txtRank.Value = Master.TheRankName;
                //}
                if (Master.CreatePerson != null)
                {
                    txtCreatePerson.Tag = Master.CreatePerson;
                    txtCreatePerson.Text = Master.CreatePersonName;
                }
                txtRemark.Text = Master.Descript;
                txtCreateDate.Text = Master.CreateDate.ToShortDateString();
                txtSumMoney.Text = Master.SumExtMoney.ToString("N4");
                txtSumQuantity.Text = ClientUtil.ToString(Master.SumQuantity);
                txtTransCharge.Text = ClientUtil.ToString(Master.TransportCharge);
               
                txtBalRule.Text = Master.BalRule;

                //显示收料单明细
                this.dgDetail.Rows.Clear();

                //查询当前料具供应商合同租赁单价

                OrderMaster = Master.Contract ;
                txtContract.Text = OrderMaster.Code;
                RefleshColumn(null);
                foreach (MatHireCollectionDetail var in Master.Details.OfType<MatHireCollectionDetail>().OrderByDescending(a=>a.MaterialLength))
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
                    if (MatHireType == EnumMatHireType.碗扣)
                    {
                        this.dgDetail[this.colType.Name, i].Value = var.MaterialType;
                    }
                    this.dgDetail[this.colLength.Name, i].Value = var.MaterialLength.ToString("N1");
                    this.dgDetail[colCollQuantity.Name, i].Value = var.Quantity;
                    this.dgDetail[colLeftQty.Name, i].Value = var.BeforeStockQty;
                    this.dgDetail[colTempQty.Name, i].Value = var.Quantity;
                    this.dgDetail[colPrice.Name, i].Value = var.RentalPrice;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    if (IsProject)
                    {
                        //设置使用部位
                        this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                        this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                        this.dgDetail[colSubject.Name, i].Tag = var.SubjectGUID;
                        this.dgDetail[colSubject.Name, i].Value = var.SubjectName;
                        //使用队伍
                        this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.BorrowUnit;
                        this.dgDetail[colBorrowUnit.Name, i].Value = var.BorrowUnitName;
                    }

                    //foreach (MatHireOrderDetail theDetail in OrderMaster.Details)
                    //{
                    //    if (theDetail.MaterialResource.Id == var.MaterialResource.Id)
                    //    {
                    //        this.dgDetail[colPrice.Name, i].Value = theDetail.Price;
                    //    }
                    //}
                    foreach (MatHireCollectionCostDtl costDtl in var.MatCostDtls)
                    {
                        this.dgDetail[costDtl.CostType, i].Value = costDtl.Money;
                    }
                    this.dgDetail.Rows[i].Tag = var;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ExceptionUtil.ExceptionMessage(ex));
            }
          
        }
        public bool IsAllSet()
        {
            string sErrMsg = string.Empty;
            MatHireOrderDetail oMatHireOrderDetail = null;
            List<MatHireOrderDetail> lstOrderDetail = null;
            List<Material> lstMaterial = null;
               lstMaterial= dgDetail.Rows.OfType<DataGridViewRow>().Select(a => a.Cells[colMaterialCode.Name].Tag as Material).Where(a => a != null).Distinct().ToList();
               if (lstMaterial == null && lstMaterial.Count == 0)
               {
                   sErrMsg = "请添加发料明细";
               }
               else
               {
                   if (OrderMaster == null)
                   {
                       sErrMsg = "没有找到料具租赁合同";
                   }
                   else
                   {
                       if (OrderMaster.Details == null || OrderMaster.Details.Count == 0)
                       {
                           sErrMsg = "该料具合同上没有明细";
                       }
                       else
                       {
                           lstOrderDetail = OrderMaster.Details.OfType<MatHireOrderDetail>().ToList();

                           foreach (Material oMaterial in lstMaterial)
                           {
                               oMatHireOrderDetail = lstOrderDetail.FirstOrDefault(a => a.MaterialResource.Id == oMaterial.Id);
                               if (oMatHireOrderDetail == null)
                               {
                                   string.Format("(物资名称【{0}】、编号【{1}】、材质【{2}】、规格【{3}】)未签订租赁合同\n", oMatHireOrderDetail.MaterialResource.Name, oMatHireOrderDetail.MaterialResource.Code, oMatHireOrderDetail.MaterialResource.Stuff, oMatHireOrderDetail.MaterialResource.Specification);
                               }
                               else
                               {
                                   if (oMatHireOrderDetail.BasicDtlCostSets == null || oMatHireOrderDetail.BasicDtlCostSets.Count == 0)
                                   {
                                       sErrMsg = sErrMsg += string.Format("(物资名称【{0}】、编号【{1}】、材质【{2}】、规格【{3}】)未设置计价方式\n", oMatHireOrderDetail.MaterialResource.Name, oMatHireOrderDetail.MaterialResource.Code, oMatHireOrderDetail.MaterialResource.Stuff, oMatHireOrderDetail.MaterialResource.Specification);
                                   }
                               }
                           }
                       }
                   }
               }
               if (string.IsNullOrEmpty(sErrMsg))
               {
                   return true;
               }
               else
               {
                   ShowMessage(sErrMsg);
                   return false;
               }
            #region old代码
            //if (OrderMaster != null)
            //{
            //    if (OrderMaster.Details.Count > 0)
            //    {

            //        foreach (MaterialRentalOrderDetail oMaterialRentalOrderDetail in orderMaster.Details)
            //        {
            //            if (oMaterialRentalOrderDetail.BasicDtlCostSets.Count == 0)
            //            {
            //                bFlag = false;
            //                if (string.IsNullOrEmpty(sErrMsg))
            //                {
            //                    sErrMsg = string.Format("以下物资未设置计价方式\t(物资名称【{0}】、编号【{1}】、材质【{2}】、规格【{3}】)\t", oMaterialRentalOrderDetail.MaterialResource .Name, oMaterialRentalOrderDetail.MaterialResource .Code , oMaterialRentalOrderDetail.MaterialResource .Stuff , oMaterialRentalOrderDetail.MaterialResource .Specification);
                                 
            //                }
            //                else
            //                {
            //                    sErrMsg += string.Format("(物资名称【{0}】、编号【{1}】、材质【{2}】、规格【{3}】)\t", oMaterialRentalOrderDetail.MaterialResource.Name, oMaterialRentalOrderDetail.MaterialResource.Code, oMaterialRentalOrderDetail.MaterialResource.Stuff, oMaterialRentalOrderDetail.MaterialResource.Specification);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        sErrMsg = "该料具合同上没有明细";
            //        bFlag = false;
            //    }
            //}
            //else
            //{
            //    sErrMsg = "没有找到料具租赁合同";
            //    bFlag = false;
            //}
            //if (!bFlag)
            //{
            //    ShowMessage(sErrMsg);
            //}
            //return bFlag;
            #endregion

        }
        private bool ValidKC()
        {
            int temp = 0;
            foreach (MatHireCollectionDetail detail in Master.Details)
            {
                if (detail.Quantity < 0)
                {
                    temp++;
                }
            }
            if (temp > 0)
            {
                //校验当前收料单是否是插入的单据
                bool value = model.MaterialHireMngSvr.VerifyCollMatBusinessDate(operDate.Value.Date, Master.ProjectId);
                //校验库存情况
                //根据收料单构建一个退料单(明细只处理数量小于0)
                MatHireReturnMaster matReturnMaster = new MatHireReturnMaster();
                matReturnMaster.TheSupplierRelationInfo = Master.TheSupplierRelationInfo;
                matReturnMaster.TheRank = Master.TheRank;
                matReturnMaster.RealOperationDate = Master.RealOperationDate;
                matReturnMaster.ProjectId = Master.ProjectId;
                matReturnMaster.MatHireType = Master.MatHireType;
                foreach (MatHireCollectionDetail detail in Master.Details)
                {
                    if (detail.Quantity < 0)
                    {
                        MatHireReturnDetail MaterialReturnDetail = new MatHireReturnDetail();
                        MaterialReturnDetail.MaterialResource = detail.MaterialResource;
                        MaterialReturnDetail.ExitQuantity = Math.Abs(detail.Quantity);
                        MaterialReturnDetail.MaterialType = detail.MaterialType;
                        MaterialReturnDetail.MaterialLength = detail.MaterialLength;
                    }
                }
                DataDomain domain = model.MaterialHireMngSvr.VerifyReturnMatKC(matReturnMaster, value);
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
            else
            {
                return true;
            }
        }
        private bool ValidView()
        {
            string validMessage = "";
            string linkStr = "";
            if (this.dgDetail.Rows.Count == 0 || (this.dgDetail.Rows.Count - 1 == 0 && this.dgDetail.Rows[0].IsNewRow))
            {
                ShowMessage("发料单明细不能为空");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            //业务日期不得大于服务器日期
            //DateTime ServerDate = CommonMethod.GetServerDateTime();
            //DateTime OperDate = this.operDate.Value.Date;
            //if (DateTime.Compare(OperDate, ServerDate) > 0)
            //{
            //    validMessage += string.Format("业务日期不得大于服务器日期[{0}]！\n",ServerDate.ToString("yyyy-MM-dd"));
            //}
            
            if (OrderMaster == null)
            {
                validMessage += "租赁合同不能为空！\n";
            }
            if (validMessage != "")
            {
                ShowMessage("无法保存:"+validMessage);
                return false;
            }
            //收料单明细表数据校验
            Hashtable ht_repeat = new Hashtable();
            //MatHireCollectionDetail dtl = new MatHireCollectionDetail();
            string sKey=string.Empty , sMaterialCode = string.Empty, sUsedPartName = string.Empty, sBorrowUnitName = string.Empty,sMaterialType=string.Empty ;
            decimal dMaterialLen = 0;
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    ShowMessage("无法保存:物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                else
                {
                    sMaterialCode = ClientUtil.ToString(dr.Cells[colMaterialCode.Name].Value);
                }
                #region 项目上验证 科目 使用部位 使用队伍
                if (IsProject)
                {
                    if (dr.Cells[colUsedPart.Name].Tag == null)
                    {
                        ShowMessage("无法保存:使用部位不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colUsedPart.Name];
                        return false;
                    }
                    else
                    {
                        sUsedPartName = ClientUtil.ToString(dr.Cells[colUsedPart.Name].Value);
                    }
                
               
                    if (dr.Cells[colSubject.Name].Tag == null)
                    {
                        ShowMessage("无法保存:核算科目不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colSubject.Name];
                        return false;
                    }
                    if (dr.Cells[this.colBorrowUnit.Name].Tag == null)
                    {
                        ShowMessage("无法保存:使用队伍不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colBorrowUnit.Name];
                        return false;
                    }
                    else
                    {
                        sBorrowUnitName = ClientUtil.ToString(dr.Cells[colBorrowUnit.Name].Value);
                    }
                }
                #endregion
                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    ShowMessage("无法保存:计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }
                if (this.MatHireType == EnumMatHireType.普通料具)
                {
                    if (dr.Cells[colCollQuantity.Name].Value == null || ClientUtil.ToDecimal(dr.Cells[colCollQuantity.Name].Value) == 0)
                    {
                        ShowMessage("无法保存:数量不能为空，不能等于0！");
                        dgDetail.CurrentCell = dr.Cells[colCollQuantity.Name];
                        return false;
                    }
                }
               
                 
                if (MatHireType == EnumMatHireType.钢管)
                {
                    if (dr.Cells[this.colLength.Name].Value == null || ClientUtil.ToDecimal(dr.Cells[colLength.Name].Value) < 0)
                    {
                        ShowMessage("无法保存:长度不能为空，必须大于0！");
                        dgDetail.CurrentCell = dr.Cells[colLength.Name];
                        return false;
                    }
                    else
                    {
                        dMaterialLen = ClientUtil.ToDecimal(dr.Cells[colLength.Name].Value);
                    }
                }
                else if (MatHireType == EnumMatHireType.碗扣)
                {
                    if (dr.Cells[this.colType.Name].Value == null)
                    {
                        ShowMessage("无法保存:碗扣类型不能为空！");
                        dgDetail.CurrentCell = dr.Cells[colType.Name];
                        return false;
                    }
                    else
                    {
                        sMaterialType = ClientUtil.ToString(dr.Cells[this.colType.Name].Value);
                    }
                }
                if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                {
                    ShowMessage("无法保存:单价不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }
                if (IsProject)
                {
                    linkStr =string.Format("{0}-{1}-{2}",ClientUtil.ToString( dr.Cells[colMaterialCode.Name].Value) ,ClientUtil.ToString(  dr.Cells[this.colBorrowUnit.Name].Value) ,ClientUtil.ToString( dr.Cells[colUsedPart.Name].Value));
                }
                else
                {
                    linkStr =ClientUtil.ToString( dr.Cells[colMaterialCode.Name].Value) ;
                }
                    if (MatHireType == EnumMatHireType.钢管)
                {
                    linkStr = string.Format("{0}-{1}", linkStr, ClientUtil.ToString(dr.Cells[colLength.Name].Value));
                }
                else if (MatHireType == EnumMatHireType.碗扣)
                {
                    linkStr = string.Format("{0}-{1}", linkStr, ClientUtil.ToString(dr.Cells[this.colType.Name].Value));
                }
                if (!ht_repeat.Contains(linkStr))
                {
                    ht_repeat.Add(linkStr, "");
                }
                else
                {
                    if (IsProject)
                    {
                        if (MatHireType == EnumMatHireType.普通料具)
                        {
                            ShowMessage("无法保存:[物资]+[使用部位]+[使用队伍]有重复信息，不可保存");
                        }
                        else if (MatHireType == EnumMatHireType.钢管)
                        {
                            ShowMessage("无法保存:[物资]+[使用部位]+[使用队伍]+[长度]有重复信息，不可保存");
                        }
                        else if (MatHireType == EnumMatHireType.碗扣)
                        {
                            ShowMessage("无法保存:[物资]+[使用部位]+[使用队伍]+[型号]有重复信息");
                        }
                    }
                    else
                    {
                        if (MatHireType == EnumMatHireType.普通料具)
                        {
                            ShowMessage("无法保存:[物资]有重复信息，不可保存");
                        }
                        else if (MatHireType == EnumMatHireType.钢管)
                        {
                            ShowMessage("无法保存:[物资]+[长度]有重复信息，不可保存");
                        }
                        else if (MatHireType == EnumMatHireType.碗扣)
                        {
                            ShowMessage("无法保存:[物资]+[型号]有重复信息");
                        }
                    }
                   
                    return false;
                }
                //收料负数(退料)  校验库存
                //if (ClientUtil.TransToDecimal(dr.Cells[colCollQuantity.Name].Value) < 0)
                //{
                //    decimal stockQty = model.MaterialHireMngSvr.GetMatStockQty(txtSupply.Result[0] as SupplierRelationInfo, dr.Cells[this.colBorrowUnit.Name].Tag as SupplierRelationInfo, dr.Cells[colMaterialCode.Name].Tag as Material, ProjectInfo.Id, MatHireType);
                //    decimal collQty = Math.Abs(ClientUtil.ToDecimal(dr.Cells[colCollQuantity.Name].Value));
                //    if (MatHireType == EnumMatHireType.钢管 || MatHireType == EnumMatHireType.碗扣)
                //    {
                //        collQty *= ClientUtil.ToDecimal(dr.Cells[this.colLength.Name].Value);
                //    }
                //    if (stockQty - collQty < 0)
                //    {
                //        ShowMessage("无法保存:[" + ProjectInfo.Name + "][" + txtSupply.Text + "][" + ClientUtil.ToString(dr.Cells[this.colBorrowUnit.Name].Value) + "][" + dr.Cells[colMaterialName.Name].Value.ToString() + "],库存量为[" + stockQty + "]，退料数量大于库存量，请修改！");
                //        dgDetail.CurrentCell = dr.Cells[colCollQuantity.Name];
                //        return false;
                //    }
                //}
              
                
            }

            dgDetail.Update();
            return true;
        }
        #region
        public decimal GetLength(object oValue)
        {
            decimal dLen = 0;
            string sValue=string.Empty ;
            if (oValue != null)
            {
                sValue = ClientUtil.ToString(oValue);
                sValue = sValue.Replace("LG", "").Replace("HG", "");
                dLen = ClientUtil.ToDecimal(sValue);
            }
            return dLen;
        }
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
        #endregion
        #region 解析表达式，计算费用金额
        //通过计算表达式，计算本单元格的计算值
        private decimal GetCalResult(string collQuantity,ref  string express )
        {
            decimal result = 0;

            if (!string.IsNullOrEmpty(collQuantity)  )
            {
               
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
            if (oOrderDetailCostSetItem == null || string.IsNullOrEmpty(oOrderDetailCostSetItem.ApproachExpression)||string.IsNullOrEmpty(oOrderDetailCostSetItem.ApproachExpression.Trim())) { return sDefaultExpression; }
            oldExpression=sExpression = oOrderDetailCostSetItem.ApproachExpression;
            foreach (OrderDetailCostSetItem oItem in oMatHireOrderDetail.BasicDtlCostSets.Where(a=>a.SetType=="价格定义"))
            {
                sExpression = sExpression.Replace(string.Format("[{0}]",oItem.CostType), oItem.Price.ToString());
            }
            return sExpression;
        
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
