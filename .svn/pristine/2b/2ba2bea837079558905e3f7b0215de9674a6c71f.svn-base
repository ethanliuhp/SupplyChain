using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage
{
    public partial class VWasteMaterialHandleQuery : TBasicDataView
    {
        private MWasteMaterialMng model = new MWasteMaterialMng();

        public VWasteMaterialHandleQuery()
        {
            InitializeComponent();
            InitEvent();
        }

        public void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.dgMain.SelectionChanged += new EventHandler(dgMain_SelectionChanged);
            this.btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);
            this.dgDetailBill.CellDoubleClick += new DataGridViewCellEventHandler(dgDetailBill_CellDoubleClick);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.dgDetailBill.CellContentClick += new DataGridViewCellEventHandler(dgDetailBill_CellContentClick);
            this.txtCodeBeginBll.tbTextChanged += new EventHandler(txtCodeBeginBll_tbTextChanged);
            this.dgMain.CellContentClick += new DataGridViewCellEventHandler(dgMain_CellContentClick);
        }
        void dgMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMain[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                //DailyPlanMaster master = model.DailyPlanSrv.GetDailyPlanByCode(dgvCell.Value.ToString());
                WasteMatProcessMaster master = dgMain.Rows[e.RowIndex].Tag as WasteMatProcessMaster;
                VWasteMaterialHandle vmro = new VWasteMaterialHandle();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }
        void dgDetailBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetailBill[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                WasteMatProcessMaster master = model.WasteMatSrv.GetWasteMatHandleByCode(ClientUtil.ToString(dgvCell.Value));
                VWasteMaterialHandle vWasteHandle = new VWasteMaterialHandle();
                vWasteHandle.CurBillMaster = master;
                vWasteHandle.Preview();
            }
        }
        void dgDetailBill_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetailBill.Columns[e.ColumnIndex].Name.Equals(this.colCode.Name))
            {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetailBill.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VWasteMaterialHandle vOrder = new VWasteMaterialHandle();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void btnSearchGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                this.txtGWBS.Tag = (list[0] as TreeNode).Tag as GWBSTree;
                this.txtGWBS.Text = (list[0] as TreeNode).Text;
            }
        }

        void txtCodeBeginBll_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEndBill.Text = this.txtCodeBeginBll.Text;
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetailBill, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Like("Code", this.txtCodeBegin.Text, MatchMode.Start));
            }
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if(this.txtRecycleUnit.Text != "")
            {
                oq.AddCriterion(Expression.Eq("RecycleUnitName", txtRecycleUnit.Text));
            }
            if (this.txtCreatePerson.Text != "")
            {
                oq.AddCriterion(Expression.Eq("CreatePersonName", this.txtCreatePerson.Text));
            }
            IList list = model.WasteMatSrv.ObjectQuery(typeof(WasteMatProcessMaster), oq);
            dgMain.Rows.Clear();
            dgDetail.Rows.Clear();
            ShowMasterList(list);
        }

        void ShowMasterList(IList list)
        {
            if (list == null || list.Count == 0) return;
            foreach (WasteMatProcessMaster master in list)
            {
                int i = this.dgMain.Rows.Add();
                this.dgMain[colCode.Name, i].Value = master.Code;
                this.dgMain[colState.Name, i].Value = ClientUtil.GetDocStateName(master.DocState);
                this.dgMain[colCreateDdate.Name, i].Value = master.CreateDate.ToShortDateString();
                this.dgMain[colRecycleUnitName.Name, i].Tag = master.RecycleUnit;
                this.dgMain[colRecycleUnitName.Name, i].Value = master.RecycleUnitName;
                this.dgMain[colRealOperationDate.Name, i].Value = master.RealOperationDate.ToShortDateString();
                this.dgMain[colCreatePerson.Name, i].Tag = master.CreatePerson;
                this.dgMain[colCreatePerson.Name, i].Value = master.CreatePersonName;
                this.dgMain[colRremark.Name, i].Value = master.Descript;
                this.dgMain[colPrintTimes.Name, i].Value = master.PrintTimes;
                this.dgMain.Rows[i].Tag = master;
            }
            this.dgMain.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMain.CurrentCell = dgMain[1, 0];
            dgMain_SelectionChanged(dgMain, new EventArgs());
        }

        void dgMain_SelectionChanged(object sneder, EventArgs e)
        {
            dgDetail.Rows.Clear();
            WasteMatProcessMaster master = dgMain.CurrentRow.Tag as WasteMatProcessMaster;
            if (master == null) return;
            foreach (WasteMatProcessDetail dtl in master.Details)
            {
                int i = this.dgDetail.Rows.Add();
                this.dgDetail[colMaterialCode.Name, i].Value = dtl.MaterialCode;
                this.dgDetail[colMaterialName.Name, i].Tag = dtl.MaterialResource;
                this.dgDetail[colMaterialName.Name, i].Value = dtl.MaterialName;
                this.dgDetail[colSpec.Name, i].Value = dtl.MaterialSpec;
                this.dgDetail[colTareWeight.Name, i].Value = dtl.TareWeight;
                this.dgDetail[colGrossWeight.Name, i].Value = dtl.GrossWeight;
                this.dgDetail[colNetWeight.Name, i].Value = dtl.NetWeight;
                this.dgDetail[colProcessPrice.Name, i].Value = dtl.ProcessPrice;
                this.dgDetail[colTotalValue.Name, i].Value = dtl.TotalValue;
                this.dgDetail[colDiagramNum.Name, i].Value = dtl.DiagramNumber;
                this.dgDetail[colGWBS.Name, i].Tag = dtl.UsedPart;
                this.dgDetail[colGWBS.Name, i].Value = dtl.UsedPartName;
                this.dgDetail[colPlateNumber.Name, i].Value = dtl.PlateNumber;
                this.dgDetail[colReceiptCode.Name, i].Value = dtl.ReceiptCode;
                this.dgDetail[colDescript.Name, i].Value = dtl.Descript;
            }
        }

        void btnSearchBill_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //单据号
                if (this.txtCodeBeginBll.Text != "")
                {
                    if (this.txtCodeBeginBll.Text == this.txtCodeBeginBll.Text)
                    {
                        condition = condition + " and t1.Code like '%" + this.txtCodeBeginBll.Text + "%'";
                    }
                    else
                    {
                        condition = condition + " and t1.Code between '" + this.txtCodeBeginBll.Text + "' and '" + this.txtCodeEndBill.Text + "'";
                    }
                }
                //业务日期
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBeginBill.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEndBill.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBeginBill.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEndBill.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
                //制单人
                if (this.txtCreatePersonBill.Text != "")
                {
                    condition += " and t1.CreatePersonName like '%" + this.txtCreatePersonBill.Text + "%'";
                }

                //物资
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                //回收单位
                if (this.txtRecycleUnitBill.Text != "")
                {
                    condition = condition + "and t1.RecycleUnitName like '%" + this.txtRecycleUnitBill.Text + "%'";
                }
                if (this.txtGWBS.Text != "")
                {
                    condition += "and t2.UsedPartName like '%" + this.txtGWBS.Text + "%'";
                }
                #endregion
                DataSet dataSet = model.WasteMatSrv.WasteMatHandleQuery(condition);
                this.dgDetailBill.Rows.Clear();
                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetailBill.Rows.Add();
                    dgDetailBill[colCodeBill.Name, rowIndex].Value = dataRow["code"];

                    DataGridViewRow currRow = dgDetailBill.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetailBill[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetailBill[colSpecBill.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    dgDetailBill[colTareWeightBill.Name, rowIndex].Value = dataRow["TareWeight"].ToString();
                    dgDetailBill[colGrossWeightBill.Name, rowIndex].Value = dataRow["GrossWeight"].ToString();
                    dgDetailBill[colNetWeightBill.Name, rowIndex].Value = dataRow["NetWeight"].ToString();
                    dgDetailBill[colRecycleUnitNameBill.Name, rowIndex].Value = dataRow["RecycleUnitName"].ToString();
                    dgDetailBill[colCreatePersonBill.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                    dgDetailBill[colRealOperationDateBill.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                    string b = dataRow["CreateDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetailBill[colCreateDdateBill.Name, rowIndex].Value = strb;
                    dgDetailBill[colProcessPriceBill.Name, rowIndex].Value = dataRow["ProcessPrice"].ToString();
                    dgDetailBill[colReceiptCodeBill.Name, rowIndex].Value = dataRow["ReceiptCode"].ToString();
                    dgDetailBill[colPlateNumberBill.Name, rowIndex].Value = dataRow["PlateNumber"].ToString();
                    dgDetailBill[colGWBSBill.Name, rowIndex].Value = dataRow["UsedPartName"].ToString();
                    dgDetailBill[colTotalValueBill.Name, rowIndex].Value = dataRow["TotalValue"].ToString();
                    dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dataRow["DiagramNumber"].ToString();
                    dgDetailBill[colPrintTimesBill.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
                    dgDetailBill[colDescriptBill.Name, rowIndex].Value = dataRow["Descript"].ToString();
                }
                this.dgDetailBill.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }
    }
}
