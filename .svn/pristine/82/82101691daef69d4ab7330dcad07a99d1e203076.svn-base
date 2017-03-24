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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutRedUI;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng
{
    public partial class VStockOutSelectList : TBasicDataView
    {
        private MStockMng model = new MStockMng();
        private StockOut curBillMaster;
        IList lstSelectResult = new ArrayList();
        IList lstSelectMatUnit = new ArrayList();
        string sProfessionalCategory = string.Empty;
        SupplierRelationInfo sUserBrank = null ;
        public SupplierRelationInfo  UserBrank
        {
            get { return sUserBrank; }
            set { sUserBrank = value;}
        }
        public IList LstSelectMatUnit
        {
            get { return lstSelectMatUnit; }
            set { lstSelectMatUnit = value; }
        }
        public IList LstSelectResult
        {
            get { return lstSelectResult; }
        }
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        /// <summary>
        /// 当前单据
        /// </summary>
        public StockOut CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        private EnumStockInOutManner stockInManner;

        public EnumStockInOutManner StockInOutManner
        {
            get { return stockInManner; }
            set { stockInManner = value; }
        }

        public VStockOutSelectList(string sProfessionalCategory, SupplierRelationInfo UserBrank)
        {
            InitializeComponent();
            InitEvent();
            InitData();
            this.sProfessionalCategory = sProfessionalCategory;
            this.UserBrank = UserBrank;
        }        

        private void InitData()
        {
            comMngType.Items.Clear();
            Array tem = Enum.GetValues(typeof(DocumentState));
            for (int i = 0; i < tem.Length; i++)
            {
                DocumentState s = (DocumentState)tem.GetValue(i);
                int k = (int)s;
                if (k != 0)
                {
                    string strNewName = ClientUtil.GetDocStateName(k);
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Text = strNewName;
                    li.Value = k.ToString();
                    comMngType.Items.Add(li);
                }
            }
            this.txtMaterialCategory.rootCatCode = "01";
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode3;
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            //专业分类s
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);
            projectInfo = StaticMethod.GetProjectInfo();
            btnAll.Checked = true;
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            //dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            btnGWBSSelect.Click += new EventHandler(btnGWBSSelect_Click);
            //dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);

            btnClose.Click += new EventHandler(btnClose_Click);
            btnSure.Click += new EventHandler(btnSure_Click);
        }

       

        void btnGWBSSelect_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtUsedPart.Text = task.Name;
                    txtUsedPart.Tag = task;
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

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                }

                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }

                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }

                if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
                {
                    condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
                }

                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
                }

                IList catResult = txtMaterialCategory.Result;
                if (catResult != null && catResult.Count > 0)
                {
                    MaterialCategory mc = catResult[0] as MaterialCategory;
                    condition = condition + " and t2.materialcode like '" + mc.Code + "%'";
                }

                if (!string.IsNullOrEmpty(txtUsedPart.Text))
                {
                    GWBSTree usedPart = txtUsedPart.Tag as GWBSTree;
                    if (usedPart != null)
                    {
                        condition += " and exists (select id from thd_gwbstree c where syscode like '%" + usedPart.Id + "%' and t2.usedpart=c.id)";
                    }
                }

                object proCat = cboProfessionalCategory.SelectedItem;
                if (proCat != null && !string.IsNullOrEmpty(proCat.ToString()))
                {
                    condition += " and t2.ProfessionalCategory='" + proCat + "'";
                }
                if (this.UserBrank != null)
                {
                    condition = condition + " and t1.SupplierRelation='" + this.UserBrank.Id + "'";
                }
                ////蓝单
                //if (btnBlue.Checked)
                //{
                //    if (stockInManner == EnumStockInOutManner.领料出库)
                //    {
                //        condition += " and t1.TheStockInOutKind=0";
                //    }
                //    else if (stockInManner == EnumStockInOutManner.调拨出库)
                //    {
                //        condition += " and t1.TheStockInOutKind=3";
                //    }
                //}
                ////红单
                //if (btnRed.Checked)
                //{
                //    if (stockInManner == EnumStockInOutManner.领料出库)
                //    {
                //        condition += " and t1.TheStockInOutKind=1";
                //    }
                //    else if (stockInManner == EnumStockInOutManner.调拨出库)
                //    {
                //        condition += " and t1.TheStockInOutKind=4";
                //    }
                //}

                if (((int)stockInManner) > 0)
                {
                    condition += " and t1.StockOutManner=" + (int)stockInManner;
                }

                #endregion

                DataSet dataSet = model.StockOutSrv.StockOutQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0, sumMoney = 0;
                int count = 0;
                //if (true )
                //{
                //    Hashtable ht_material = new Hashtable();
                //    foreach (DataRow dataRow in dataTable.Rows)
                //    {
                //        int stockOutManner = int.Parse(dataRow["StockOutManner"].ToString());
                //        string materialCode = ClientUtil.ToString(dataRow["MaterialCode"]);
                //        if (!ht_material.Contains(materialCode))
                //        {
                //            DataDomain domain = new DataDomain();
                //            domain.Name1 = ClientUtil.ToString(dataRow["MaterialCode"]);
                //            domain.Name2 = ClientUtil.ToString(dataRow["MaterialName"]);
                //            domain.Name3 = ClientUtil.ToString(dataRow["MaterialSpec"]);
                //            domain.Name4 = ClientUtil.ToString(dataRow["MatStandardUnitName"]);
                //            domain.Name5 = ClientUtil.ToString(dataRow["Quantity"]);
                //            if (stockOutManner == 21)//调拨出库
                //            {
                //                domain.Name6 = ClientUtil.ToString(dataRow["MoveMoney"]);
                //            }
                //            else
                //            {
                //                domain.Name6 = ClientUtil.ToString(dataRow["Money"]);
                //            }
                //            ht_material.Add(domain.Name1, domain);
                //        }
                //        else
                //        {
                //            DataDomain domain = (DataDomain)ht_material[materialCode];
                //            domain.Name5 = (ClientUtil.ToDecimal(domain.Name5) + ClientUtil.ToDecimal(dataRow["Quantity"])) + "";
                //            if (stockOutManner == 21)//调拨出库
                //            {
                //                domain.Name6 = (ClientUtil.ToDecimal(domain.Name6) + ClientUtil.ToDecimal(dataRow["MoveMoney"])) + "";
                //            }
                //            else
                //            {
                //                domain.Name6 = (ClientUtil.ToDecimal(domain.Name6) + ClientUtil.ToDecimal(dataRow["Money"])) + "";
                //            }
                //            ht_material.Remove(materialCode);
                //            ht_material.Add(materialCode, domain);
                //        }
                //    }

                //    foreach (DataDomain domain in ht_material.Values)
                //    {
                //        int rowIndex = this.dgDetail.Rows.Add();
                //        dgDetail[colMaterialCode.Name, rowIndex].Value = domain.Name1;
                //        dgDetail[colMaterialName.Name, rowIndex].Value = domain.Name2;
                //        dgDetail[colSpec.Name, rowIndex].Value = domain.Name3;
                //        dgDetail[colUnit.Name, rowIndex].Value = domain.Name4;
                //        dgDetail[colQuantity.Name, rowIndex].Value = domain.Name5;
                //        dgDetail[colMoney.Name, rowIndex].Value = domain.Name6;
                        
                //        sumQuantity += ClientUtil.ToDecimal(domain.Name5);
                //        sumMoney += ClientUtil.ToDecimal(domain.Name6);
                //    }
                //    count = ht_material.Count;
                //}

                //else
                //{
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                        dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierRelationName"];
                        object isLimited = dataRow["IsLimited"];
                        if (isLimited == null || isLimited.ToString() == "") isLimited = "0";
                        dgDetail[colIsLimited.Name, rowIndex].Value = (int.Parse(isLimited.ToString()) == 1) ? "是" : "否";

                        DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                        currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                        object objState = dataRow["State"];
                        if (objState != null)
                        {
                            dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                        }

                        int stockOutManner = int.Parse(dataRow["StockOutManner"].ToString());
                        dgDetail[colStockOutManner.Name, rowIndex].Value = Enum.GetName(typeof(EnumStockInOutManner), stockOutManner);

                        dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];
                        dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];
                        dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];
                        dgDetail[colUsedPart.Name, rowIndex].Value = dataRow["UsedPartName"];

                        object quantity = dataRow["Quantity"];
                        if (quantity != null)
                        {
                            sumQuantity += ClientUtil.ToDecimal(quantity);
                        }

                        dgDetail[colQuantity.Name, rowIndex].Value = quantity;

                        object money = 0;
                        if (stockOutManner == 21)//调拨出库
                        {
                            dgDetail[colPrice.Name, rowIndex].Value = dataRow["movePrice"];
                            money = dataRow["MoveMoney"];
                        }
                        else
                        {
                            dgDetail[colPrice.Name, rowIndex].Value = dataRow["price"];
                            money = dataRow["Money"];
                        }

                        if (money != null)
                        {
                            sumMoney += ClientUtil.ToDecimal(money);
                        }
                        dgDetail[colMoney.Name, rowIndex].Value = money;

                        dgDetail[colProfessionalCategory.Name, rowIndex].Value = dataRow["ProfessionalCategory"];
                        dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];
                        dgDetail[colTheStockInOutKind.Name, rowIndex].Value = dataRow["TheStockInOutKind"];
                        dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                        dgDetail[colCreatePersonName.Name, rowIndex].Value = dataRow["CreatePersonName"];
                        dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"];
                        dgDetail[colDiagramNum.Name, rowIndex].Value = ClientUtil.ToString(dataRow["DiagramNumber"]);
                        dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
                        dgDetail[colSelect.Name, rowIndex].Tag = dataRow["materialID"].ToString();
                    }
                    count = dataTable.Rows.Count;
                //}
                
                FlashScreen.Close();
                this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
                txtSumMoney.Text = sumMoney.ToString("#,###.####");
                lblRecordTotal.Text = "共【" + count + "】条记录";

                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        public void btnClose_Click(object sender, EventArgs e)
        {
            lstSelectResult.Clear();
            this.Close();
        }
        public void btnSure_Click(object sender, EventArgs e)
        {
            DataGridViewRow oRow = null;
            DataGridViewCheckBoxCell oCell = null;
            DataGridViewCell oMatNameCell = null;
            string sMatName=string.Empty ;
            lstSelectResult.Clear();
            for (int i = 0; i < this.dgDetail.Rows.Count; i++)
            {
                oRow = this.dgDetail.Rows[i];
                oCell = oRow.Cells[colSelect.Name] as DataGridViewCheckBoxCell;
                oMatNameCell=oRow.Cells[colUnit.Name ];
                sMatName=oMatNameCell.Value==null?"":oMatNameCell.Value .ToString ();
                if (oCell.Value != null && bool.Parse(oCell.Value.ToString()))
                {
                    lstSelectResult.Add(oCell.Tag.ToString());
                    lstSelectMatUnit.Insert(lstSelectMatUnit.Count, oCell.Tag.ToString());
                    lstSelectMatUnit.Insert(lstSelectMatUnit.Count, sMatName);
                }
            }
            this.Close();
        }
        
    }
}
