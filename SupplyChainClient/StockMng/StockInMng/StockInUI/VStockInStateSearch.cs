using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI
{
    public partial class VStockInStateSearch : TBasicDataView
    {
        //private IMStockIn theMStockIn = StaticMethod.GetRefModule(typeof(MStockIn)) as IMStockIn;
        MStockMng theMStockIn = new MStockMng();
        IList info_list = new ArrayList();
        private Hashtable cat_ht = new Hashtable();

        public VStockInStateSearch()
        {
            InitializeComponent();
            InitEvent();
            InitData();

            //info_list = MStockIn.supplyOrderSrv.GetPersonsInfo();
        }
        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            //cat_ht = ClientUtil.GetAllNewCategory();

            //if (LoginInfomation.LoginInfo.TheOperationOrgInfo.CkRight == "1")
            //{
            //    this.txtSupplier.Enabled = false;
            //    this.Price.Visible = false;
            //    this.Money.Visible = false;
            //    this.SupplyInfo.Visible = false;
            //    this.InitSupplier.Visible = false;
            //}
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.txtShBeginNo.tbTextChanged += new EventHandler(txtShBeginNo_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.txtMaterial.Validating += new CancelEventHandler(txtMaterial_Validating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.Click += new EventHandler(dgDetail_Click);
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal qty = 0;
            decimal mng = 0;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.Cells["Select"].Value != null && (Boolean)var.Cells["Select"].Value == true)
                {
                    qty += ClientUtil.TransToDecimal(var.Cells["Quantity"].Value);
                    mng += ClientUtil.TransToDecimal(var.Cells["Money"].Value);
                }
            }
            this.txtSelectQty.Text = qty.ToString();
            this.txtSelectMny.Text = mng.ToString();

        }

        void dgDetail_Click(object sender, EventArgs e)
        {
            if (this.dgDetail.Rows.Count > 0)
            {
                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name == "Select")
                {
                    this.dgDetail.BeginEdit(false);
                }
                else
                {
                    this.dgDetail.EndEdit();
                }
            }
        }

        void txtMaterial_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtMaterial.Result != null && this.txtMaterial.Result.Count > 0)
            {
                Material a = this.txtMaterial.Result[0] as Material;
                if (this.txtMaterial.Text != "")
                {
                    if (a.Specification != this.txtSpec.Text)
                    {
                        this.txtSpec.Text = a.Specification;
                    }
                    if (a.Stuff != this.txtStuff.Text)
                    {
                        this.txtStuff.Text = a.Stuff;
                    }
                }
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void txtShBeginNo_tbTextChanged(object sender, EventArgs e)
        {
            this.txtShEndNo.Text = this.txtShBeginNo.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {

            Hashtable hsStockIn = new Hashtable();
            Hashtable hsStockInRed = new Hashtable();
            decimal stockInQuantity = 0;
            decimal stockInRedQuantity = 0;
            string condition = " 1=1 and t1.StockInManner not in (103,90) ";
            #region 定义条件

            if (this.chkSh.Checked)
            {
                condition = condition + " and t1.AuditDate between ('" + this.dtpDateBegin.Value.ToShortDateString() + "') and ('" + this.dtpDateEnd.Value.ToShortDateString() + "')";
            }
            else
            {
                condition = condition + " and t1.CreateDate between ('" + this.dtpDateBegin.Value.ToShortDateString() + "') and ('" + this.dtpDateEnd.Value.ToShortDateString() + "')";
            }

            if (this.txtCodeBegin.Text != "")
            {
                condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
            }
            //if (LoginInfomation.LoginInfo.TheOperationOrgInfo.CkRight == "1")
            //{
            //    condition = condition + " and t1.operationorg=" + LoginInfomation.LoginInfo.TheOperationOrgInfo.Id;
            //}

            if (this.txtShBeginNo.Text != "")
            {
                condition = condition + " and t9.FORWARDBUSENTITYCODE between '" + this.txtShBeginNo.Text + "' and '" + this.txtShEndNo.Text + "'";
                condition = condition + " and t9.FORWARDCLSNAME like '%SupplyReceving%' ";
            }

            if (this.txtBundleNo.Text != "")
            {
                condition += " and t2.bundleno like '%" + this.txtBundleNo.Text + "%' ";
            }

            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                condition = condition + " and t1.SupplierRelation=" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString();
            }

            if (this.optNoTally.Checked)
            {
                condition = condition + " and t1.IsTally=0";
            }

            if (this.optTally.Checked)
            {
                condition = condition + " and t1.IsTally=1";

            }

            if (this.chkSh1.Checked)
            {
                if (this.txtFiscalYear.Text != "")
                {
                    condition += " and t1.auditYear = '" + this.txtFiscalYear.Text + "' ";
                }

                if (this.txtFiscalMonth.Text != "")
                {
                    condition += " and t1.auditMonth = '" + this.txtFiscalMonth.Text + "' ";
                }
            }
            else
            {
                if (this.txtFiscalYear.Text != "")
                {
                    condition += " and t1.createYear = '" + this.txtFiscalYear.Text + "' ";
                }

                if (this.txtFiscalMonth.Text != "")
                {
                    condition += " and t1.createMonth = '" + this.txtFiscalMonth.Text + "' ";
                }
            }

            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t7.matName like '%" + this.txtMaterial.Text + "%'";
            }
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and t7.MatSpecification like '%" + this.txtSpec.Text + "%'";
            }
            if (this.txtStuff.Text != "")
            {
                condition = condition + " and t7.stuff like '%" + this.txtStuff.Text + "%'";
            }
            if (this.txtJBR.Text != "" || this.txtJBR.Result.Count > 0)
            {
                condition = condition + " and t1.CREATEPERSON=" + (this.txtJBR.Result[0] as PersonInfo).Id;
            }
            if (this.txtContractNo.Text != "")
            {
                condition = condition + " and t1.contractno like '%" + this.txtContractNo.Text + "%' ";
            }
            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
            {
                condition = condition + " and t1.handleperson=" + (txtHandlePerson.Result[0] as PersonInfo).Id;
            }

            if (txtStockInManner.Text != "" && txtStockInManner.Result != null && txtStockInManner.Result.Count > 0)
            {
                condition = condition + " and t1.StockInManner=" + (txtStockInManner.Result[0] as StockInManner).Id.ToString();
            }
            if (this.txtStationCategory.Text != "" && this.txtStationCategory.Result != null && this.txtStationCategory.Result.Count > 0)
            {
                condition = condition + " and t1.thestationcategory=" + (this.txtStationCategory.Result[0] as StationCategory).Id;
            }

            if (this.txtOrg.Text != "" && this.txtOrg.Result != null && this.txtOrg.Result.Count > 0)
            {
                condition = condition + " and t1.handleorg=" + (this.txtOrg.Result[0] as OperationOrgInfo).Id;
            }

            #endregion
            DataSet dataSet = theMStockIn.StockInSrv.StockInStateSearch(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                //if (typeof(SupplyRecevingMaster).AssemblyQualifiedName.Contains(dataRow["前驱类型"].ToString()))
                //{
                //    if (!hsStockIn.Contains(dataRow["明细ID"]))
                //        hsStockIn.Add(dataRow["明细ID"], dataRow["明细ID"]);
                //}

                long handlePerson = ClientUtil.ToLong(dataRow["业务员ID"]);
                DateTime createDate = ClientUtil.ToDateTime(dataRow["CREATEDATE"]);

                int row = this.dgDetail.Rows.Add();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    switch (dataTable.Columns[i].ColumnName.ToString())
                    {
                        case "单号":
                            this.dgDetail["Code", row].Value = dataRow[i];
                            break;
                        case "仓库编码":
                            this.dgDetail["StockCode", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "仓库名称":
                            this.dgDetail["Stock", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "重量类型":
                            this.dgDetail["WeightType", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "入库方式":
                            this.dgDetail["StockInManner", row].Value = dataRow[i];
                            break;
                        case "记帐":
                            this.dgDetail["Tally", row].Value = dataRow[i];
                            break;
                        case "供应商名称":
                            this.dgDetail["SupplyInfo", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "原始供应商":
                            this.dgDetail["InitSupplier", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "物料编码":
                            this.dgDetail["MaterialCode", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "物料名称":
                            this.dgDetail["MaterialName", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "规格":
                            this.dgDetail["Spec", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "材质":
                            this.dgDetail["Stuff", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "父分类":
                            string catId = ClientUtil.ToString(dataRow[i]);
                            MaterialCategory cate = (MaterialCategory)cat_ht[catId];
                            if (cate != null)
                            {
                                string parentCatId = cate.ParentNode.Id;
                                MaterialCategory cate_1 = (MaterialCategory)cat_ht[parentCatId];
                                this.dgDetail["FirstCat", row].Value = cate_1.Name;
                                this.dgDetail["SecondCat", row].Value = cate.Name;
                            }
                            break;
                        case "计量单位":
                            this.dgDetail["Unit", row].Value = dataRow[i];
                            break;
                        case "数量":
                            decimal qty = ClientUtil.TransToDecimal(dataRow[i]);
                            this.dgDetail["Quantity", row].Value = qty;
                            if (qty < 0)
                            {
                                stockInRedQuantity += -qty;
                                hsStockInRed.Add(dataRow["明细ID"], dataRow["明细ID"]);
                            }
                            else
                            {
                                stockInQuantity += qty;
                            }
                            break;
                        case "冲红量":
                            this.dgDetail["RedQty", row].Value = dataRow[i];
                            break;
                        case "单价":
                            this.dgDetail["Price", row].Value = dataRow[i];
                            break;
                        case "成材率":
                            this.dgDetail["Rate", row].Value = dataRow[i];
                            break;
                        case "金额":
                            this.dgDetail["Money", row].Value = dataRow[i];
                            break;
                        case "备注":
                            this.dgDetail["Remark", row].Value = dataRow[i];
                            break;
                        case "createdate":
                            this.dgDetail["crtdate", row].Value = dataRow[i];
                            break;
                        case "审核日期":
                            DateTime auditDate = ClientUtil.ToDateTime(dataRow[i]);
                            if (auditDate > ClientUtil.ToDateTime("2000-01-01"))
                            {
                                this.dgDetail["AuditDate", row].Value = dataRow[i];
                            }
                            break;
                        case "制单人":
                            this.dgDetail["person", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "合同号":
                            this.dgDetail["ContractNo", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "前驱单号":
                            this.dgDetail["ForwardCode", row].Value = dataRow[i];
                            break;
                        case "经办人":
                            this.dgDetail["JBR", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        //case "前驱类型":
                        //    if (typeof(SupplyRecevingMaster).AssemblyQualifiedName.Contains(dataRow[i].ToString()))
                        //    {
                        //        this.dgDetail["ForwardType", row].Value = "到货单";
                        //    }
                        //    else if (typeof(StockIn).AssemblyQualifiedName.Contains(dataRow[i].ToString()))
                        //    {
                        //        this.dgDetail["ForwardType", row].Value = "入库单";
                        //    }
                        //    else if (typeof(StockMove).AssemblyQualifiedName.Contains(dataRow[i].ToString()))
                        //    {
                        //        this.dgDetail["ForwardType", row].Value = "调拨单";
                        //    }
                        //    else if (typeof(ProfitIn).AssemblyQualifiedName.Contains(dataRow[i].ToString()))
                        //    {
                        //        this.dgDetail["ForwardType", row].Value = "盘盈单";
                        //    }
                        //    break;
                        case "厂家":
                            this.dgDetail["FromFactory", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "品牌":
                            this.dgDetail["Brand", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "捆包号":
                            this.dgDetail["BundleNo", row].Value = ClientUtil.ToString(dataRow[i]);
                            break;
                        case "长度":
                            this.dgDetail["Other1", row].Value = dataRow[i];
                            break;
                        case "业务部门":
                            string ywbb = ClientUtil.ToString(dataRow[i]);
                            this.dgDetail["HandleOrg", row].Value = ywbb;
                            break;
                        default:
                            break;
                    }
                }

            }

            //计算合计数量
            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            this.txtSumQuantity.Text = "0";
            this.txtSumMoney.Text = "0";
            foreach (DataGridViewRow row in this.dgDetail.Rows)
            {
                if (row.IsNewRow) break;
                sumQuantity += ClientUtil.ToDecimal(ClientUtil.ToString(row.Cells["Quantity"].Value));
                sumMoney += ClientUtil.TransToDecimal((row.Cells["Money"].Value));
            }
            this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
            this.txtSumMoney.Text = sumMoney.ToString("#,###.####");
            this.lblStockInCount.Text = "蓝单:[" + hsStockIn.Count.ToString() + "]份";
            this.lblStockInRedCount.Text = "红单:[" + hsStockInRed.Count.ToString() + "]份";
            this.txtStockInQuantity.Text = stockInQuantity.ToString("#,###.####");
            this.txtStockInRedQuantity.Text = stockInRedQuantity.ToString("#,###.####");

            if (this.dgDetail.Rows.Count > 0)
            {
                this.dgDetail.Rows[0].Selected = true;
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }

    }
}
