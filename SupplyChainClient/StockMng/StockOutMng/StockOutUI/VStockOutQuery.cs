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
using VirtualMachine.Core;
using NHibernate.Criterion;
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
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI
{
    public partial class VStockOutQuery : TBasicDataView
    {
        private MStockMng model = new MStockMng();
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        private StockOut curBillMaster;
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

        public VStockOutQuery(string  s)
        {
            InitializeComponent();
            if (s == "领料出库查询")
            {
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Remove(tabPage4);
            }
            InitEvent();
            InitData();
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
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            //专业分类s
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategoryBill, true);
             VBasicDataOptr.InitProfessionCategory(cboProfessionalCategoryMvBill, true);
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
            this.btnSearchMvBill.Click+=new EventHandler(btnSearchMvBill_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgMasterMvBill.CellContentClick+=new DataGridViewCellEventHandler(dgMasterMvBill_CellContentClick);
            btnGWBSSelect.Click += new EventHandler(btnGWBSSelect_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
         
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgMasterMvBill.SelectionChanged+=new EventHandler(dgMasterMvBill_SelectionChanged);
        }

        /// <summary>
        /// 领料出库单据打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                object stockInOutManner = dgMaster.Rows[e.RowIndex].Cells[colStockOutMannerBill.Name].Value;
                int stockInOutKind = int.Parse(dgMaster.Rows[e.RowIndex].Cells[colTheStockInOutKindBill.Name].Value.ToString());
                if (stockInOutManner != null)
                {
                    EnumStockInOutManner manner = (EnumStockInOutManner)Enum.Parse(typeof(EnumStockInOutManner), stockInOutManner.ToString());
                    if (manner == EnumStockInOutManner.领料出库)
                    {
                        //蓝单
                        if (stockInOutKind == 0)
                        {
                            StockOut master = dgMaster.Rows[e.RowIndex].Tag as StockOut;
                            VStockOut vmro = new VStockOut();
                            vmro.CurBillMaster = master;
                            vmro.Preview();
                        }
                        else
                        {
                            StockOutRed master = dgMaster.Rows[e.RowIndex].Tag as StockOutRed;
                            VStockOutRed vsor = new VStockOutRed();
                            vsor.CurBillMaster = master;
                            vsor.Preview();
                        }
                    }
                    else if (manner == EnumStockInOutManner.调拨出库)
                    {
                        if (stockInOutKind == 3)
                        {
                            StockMoveOut master = dgMaster.Rows[e.RowIndex].Tag as StockMoveOut;
                            VStockMoveOut vsmo = new VStockMoveOut();
                            vsmo.CurBillMaster = master;
                            vsmo.Preview();
                        }
                        else
                        {
                            StockMoveOutRed master =dgMaster.Rows[e.RowIndex].Tag as StockMoveOutRed;
                            VStockMoveOutRed vsmor = new VStockMoveOutRed();
                            vsmor.CurBillMaster = master;
                            vsmor.Preview();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 领料出库明细查询双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colCode.Name))
            {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                string stockOutManageType = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colTheStockInOutKind.Name].Value);

                if (stockOutManageType == "1")
                {
                    //领料出库红单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockOutRed vsoRed = new VStockOutRed();
                        vsoRed.Start(code,billId);
                        vsoRed.ShowDialog();
                    }
                }

                if (stockOutManageType == "0")
                {
                    //领料出库蓝单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockOut vso = new VStockOut();
                        vso.Start(code, billId);
                        vso.ShowDialog();
                    }
                }
                if (stockOutManageType == "3")
                {
                    //调拨出库库蓝单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockMoveOut Vsmo = new VStockMoveOut();
                        Vsmo.Start(code,billId);
                        Vsmo.ShowDialog();
                    }
                }
                if (stockOutManageType == "4")
                {
                    //调拨出库红单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockMoveOutRed vsmoRed = new VStockMoveOutRed();
                        vsmoRed.Start(code,billId);
                        vsmoRed.ShowDialog();
                    }
                }

            }
        }
        /// <summary>
        /// 领料出库明细查询打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                object stockInOutManner = dgDetail.Rows[e.RowIndex].Cells[colStockOutManner.Name].Value;
                int stockInOutKind = int.Parse(dgDetail.Rows[e.RowIndex].Cells[colTheStockInOutKind.Name].Value.ToString());
                if (stockInOutManner != null)
                {
                    EnumStockInOutManner manner = (EnumStockInOutManner)Enum.Parse(typeof(EnumStockInOutManner), stockInOutManner.ToString());
                    if (manner == EnumStockInOutManner.领料出库)
                    {
                        //蓝单
                        if (stockInOutKind == 0)
                        {
                            StockOut master = model.StockOutSrv.GetStockOutByCode(dgvCell.Value.ToString(), projectInfo.Id);
                            VStockOut vmro = new VStockOut();
                            vmro.CurBillMaster = master;
                            vmro.Preview();
                        }
                        else
                        {
                            StockOutRed master = model.StockOutSrv.GetStockOutRedByCode(dgvCell.Value.ToString(), projectInfo.Id);
                            VStockOutRed vsor = new VStockOutRed();
                            vsor.CurBillMaster = master;
                            vsor.Preview();
                        }
                    }
                    else if (manner == EnumStockInOutManner.调拨出库)
                    {
                        if (stockInOutKind == 3)
                        {
                            StockMoveOut master = model.StockMoveSrv.GetStockMoveOutByCode(dgvCell.Value.ToString(), projectInfo.Id);
                            VStockMoveOut vsmo = new VStockMoveOut();
                            vsmo.CurBillMaster = master;
                            vsmo.Preview();
                        }
                        else
                        {
                            StockMoveOutRed master = model.StockMoveSrv.GetStockMoveOutRedByCode(dgvCell.Value.ToString(), projectInfo.Id);
                            VStockMoveOutRed vsmor = new VStockMoveOutRed();
                            vsmor.CurBillMaster = master;
                            vsmor.Preview();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 调拨出库打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMasterMvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMasterMvBill[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeMvBill.Name)
            {
                object stockInOutManner = dgMasterMvBill.Rows[e.RowIndex].Cells[colStockOutMannerMvBill.Name].Value;
                int stockInOutKind = int.Parse(dgMasterMvBill.Rows[e.RowIndex].Cells[colTheStockInOutKindMvBill.Name].Value.ToString());
                if (stockInOutManner != null)
                {
                    EnumStockInOutManner manner = (EnumStockInOutManner)Enum.Parse(typeof(EnumStockInOutManner), stockInOutManner.ToString());
                    if (manner == EnumStockInOutManner.领料出库)
                    {
                        //蓝单
                        if (stockInOutKind == 0)
                        {
                            StockOut master = dgMasterMvBill.Rows[e.RowIndex].Tag as StockOut;
                            VStockOut vmro = new VStockOut();
                            vmro.CurBillMaster = master;
                            vmro.Preview();
                        }
                        else
                        {
                            StockOutRed master = dgMasterMvBill.Rows[e.RowIndex].Tag as StockOutRed;
                            VStockOutRed vsor = new VStockOutRed();
                            vsor.CurBillMaster = master;
                            vsor.Preview();
                        }
                    }
                    else if (manner == EnumStockInOutManner.调拨出库)
                    {
                        if (stockInOutKind == 3)
                        {
                            StockMoveOut master = dgMasterMvBill.Rows[e.RowIndex].Tag as StockMoveOut;
                            VStockMoveOut vsmo = new VStockMoveOut();
                            vsmo.CurBillMaster = master;
                            vsmo.Preview();
                        }
                        else
                        {
                            StockMoveOutRed master = dgMasterMvBill.Rows[e.RowIndex].Tag as StockMoveOutRed;
                            VStockMoveOutRed vsmor = new VStockMoveOutRed();
                            vsmor.CurBillMaster = master;
                            vsmor.Preview();
                        }
                    }
                }
            }
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
        #region 领料出库查询
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

                if (stockInManner == EnumStockInOutManner.领料出库)
                {
                    if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                    {
                        condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                    }
                }
                else
                {
                    if (this.txtSupplier.Text != "" )
                    {
                        condition = condition + " and t1.moveoutprojectname like '%" + txtSupplier.Text + "%'";
                    }
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

                //蓝单
                if (btnBlue.Checked)
                {
                    if (stockInManner == EnumStockInOutManner.领料出库)
                    {
                        condition += " and t1.TheStockInOutKind=0";
                    }
                    else if (stockInManner == EnumStockInOutManner.调拨出库)
                    {
                        condition += " and t1.TheStockInOutKind=3";
                    }
                }
                //红单
                if (btnRed.Checked)
                {
                    if (stockInManner == EnumStockInOutManner.领料出库)
                    {
                        condition += " and t1.TheStockInOutKind=1";
                    }
                    else if (stockInManner == EnumStockInOutManner.调拨出库)
                    {
                        condition += " and t1.TheStockInOutKind=4";
                    }
                }

                if (((int)stockInManner) > 0)
                {
                    condition += " and t1.StockOutManner=" + (int)stockInManner;
                }

                #endregion
                #region 过滤数据权限
                if (1 ==2 && StaticMethod.IsEnabledDataAuth && ConstObject.TheLogin.TheAccountOrgInfo != null && ClientUtil.ToString(ConstObject.TheLogin.TheAccountOrgInfo.Id) != "" && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
                {
                    //1.查询数据权限配置
                    ObjectQuery oqAuth = new ObjectQuery();
                    oqAuth.AddCriterion(Expression.Eq("AuthMenu.Id", TheAuthMenu.Id));

                    Disjunction disAuth = new Disjunction();
                    foreach (OperationRole role in ConstObject.TheRoles)
                    {
                        disAuth.Add(Expression.Eq("AppRole.Id", role.Id));
                    }
                    oqAuth.AddCriterion(disAuth);

                    IEnumerable<AuthConfigOrgSysCode> listAuth = model.StockOutSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();
                    if (listAuth.Count() > 0)//如果配置了权限规则
                    {
                        var query = from a in listAuth
                                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.无约束
                                    select a;

                        if (query.Count() == 0)//未设置“无约束”规则
                        {
                            disAuth = new Disjunction();

                            //2.根据数据权限定义的规则过滤数据
                            query = from a in listAuth
                                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.该核算组织的
                                    select a;
                            if (query.Count() > 0)//如果配置中包含操作“该核算组织的”的权限，则无需再添加其它规则
                            {
                                condition += "and t1.OPGSYSCODE like  '" + ConstObject.TheLogin.TheAccountOrgInfo.SysCode + "%'";
                            }
                        }
                    }
                }
                #endregion

                DataSet dataSet = model.StockOutSrv.StockOutQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0, sumMoney = 0;
                int count = 0;
                if (ckWz.Checked)
                {
                    Hashtable ht_material = new Hashtable();
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int stockOutManner = int.Parse(dataRow["StockOutManner"].ToString());
                        string materialCode = ClientUtil.ToString(dataRow["MaterialCode"]);
                        if (!ht_material.Contains(materialCode))
                        {
                            DataDomain domain = new DataDomain();
                            domain.Name1 = ClientUtil.ToString(dataRow["MaterialCode"]);
                            domain.Name2 = ClientUtil.ToString(dataRow["MaterialName"]);
                            domain.Name3 = ClientUtil.ToString(dataRow["MaterialSpec"]);
                            domain.Name4 = ClientUtil.ToString(dataRow["MatStandardUnitName"]);
                            domain.Name5 = ClientUtil.ToString(dataRow["Quantity"]);
                            if (stockOutManner == 21)//调拨出库
                            {
                                domain.Name6 = ClientUtil.ToString(dataRow["MoveMoney"]);
                            }
                            else
                            {
                                domain.Name6 = ClientUtil.ToString(dataRow["Money"]);
                            }
                            ht_material.Add(domain.Name1, domain);
                        }
                        else
                        {
                            DataDomain domain = (DataDomain)ht_material[materialCode];
                            domain.Name5 = (ClientUtil.ToDecimal(domain.Name5) + ClientUtil.ToDecimal(dataRow["Quantity"])) + "";
                            if (stockOutManner == 21)//调拨出库
                            {
                                domain.Name6 = (ClientUtil.ToDecimal(domain.Name6) + ClientUtil.ToDecimal(dataRow["MoveMoney"])) + "";
                            }
                            else
                            {
                                domain.Name6 = (ClientUtil.ToDecimal(domain.Name6) + ClientUtil.ToDecimal(dataRow["Money"])) + "";
                            }
                            ht_material.Remove(materialCode);
                            ht_material.Add(materialCode, domain);
                        }
                    }

                    foreach (DataDomain domain in ht_material.Values)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail[colMaterialCode.Name, rowIndex].Value = domain.Name1;
                        dgDetail[colMaterialName.Name, rowIndex].Value = domain.Name2;
                        dgDetail[colSpec.Name, rowIndex].Value = domain.Name3;
                        dgDetail[colUnit.Name, rowIndex].Value = domain.Name4;
                        dgDetail[colQuantity.Name, rowIndex].Value = domain.Name5;
                        dgDetail[colMoney.Name, rowIndex].Value = domain.Name6;
                        sumQuantity += ClientUtil.ToDecimal(domain.Name5);
                        sumMoney += ClientUtil.ToDecimal(domain.Name6);
                    }
                    count = ht_material.Count;
                }
                else
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        int rowIndex = this.dgDetail.Rows.Add();
                        dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                        if (stockInManner == EnumStockInOutManner.领料出库)
                        {
                            dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierRelationName"];
                        }
                        else
                        {
                            dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["moveoutprojectname"];
                        }
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
                        dgDetail[colSubjectNameDtl.Name, rowIndex].Value = dataRow["subjectname"];
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
                        dgDetail[this.colSubjectNameDtl.Name, rowIndex].Value = dataRow["subjectname"];
                    }
                    count = dataTable.Rows.Count;
                }
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
        /// <summary>
        /// 主从表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //objectQuery.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));

            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            //业务日期
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            //专业分类
            if (cboProfessionalCategoryBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("ProfessionCategory", cboProfessionalCategoryBill.Text));
            }
            try
            {
                list = model.StockOutSrv.GetStockOut(objectQuery);
                IList redList = model.StockOutSrv.GetStockOutRed(objectQuery);
                dgMaster.Rows.Clear();
                dgDetailBill.Rows.Clear();
                ShowMasterList(list,redList);
                FlashScreen.Close();
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
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList, IList redMasterList)
        {
            if ( masterList.Count == 0&& redMasterList.Count==0) return;
            foreach (StockOut master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colSupplyInfoBill.Name, rowIndex].Value = master.TheSupplierName;                           //使用队伍
                dgMaster[colStockOutMannerBill.Name, rowIndex].Value = master.StockOutManner;         //入库类型

                bool isLimited = true;
                if (master.IsLimited.ToString() == "1")
                {
                    isLimited = true;
                }
                else if (master.IsLimited.ToString() == "0")
                {
                    isLimited = false;
                }

                if (isLimited == true)
                {
                    dgMaster[colIsLimitedBill.Name, rowIndex].Value = true;
                }
                else
                {
                    dgMaster[colIsLimitedBill.Name, rowIndex].Value = false;
                }

                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colTheStockInOutKindBill.Name, rowIndex].Value = 0;         //红蓝单类型
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                                     //状态
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                 //备注

            }
            foreach (StockOutRed master in redMasterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colSupplyInfoBill.Name, rowIndex].Value = master.TheSupplierName;                           //使用队伍
                dgMaster[colStockOutMannerBill.Name, rowIndex].Value = master.StockOutManner;         //入库类型

                bool isLimited = true;
                if (master.IsLimited.ToString() == "1")
                {
                    isLimited = true;
                }
                else if (master.IsLimited.ToString() == "0")
                {
                    isLimited = false;
                }

                if (isLimited == true)
                {
                    dgMaster[colIsLimitedBill.Name, rowIndex].Value = true;
                }
                else
                {
                    dgMaster[colIsLimitedBill.Name, rowIndex].Value = false;
                }

                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colTheStockInOutKindBill.Name, rowIndex].Value =1;         //红蓝单类型
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                                     //状态
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                 //备注

            }
            this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            if (dgMaster.CurrentRow.Tag == null) return;
            long stockInOutKind = ClientUtil.ToLong(dgMaster.CurrentRow.Cells["colTheStockInOutKindBill"].Value);
            BasicStockOut master = null;
            if (stockInOutKind == 0)
            {
                master = dgMaster.CurrentRow.Tag as StockOut;
                foreach (StockOutDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailBill.Rows.Add();
                    dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                                              //物资编码
                    dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                                             //物资名称
                    dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                            //规格型号   
                    dgDetailBill[colUsedPartBill.Name, rowIndex].Value = dtl.UsedPartName;                                                 //使用部位
                    dgDetailBill[colMaterialStuffBill.Name, rowIndex].Value = dtl.MaterialStuff;                                               //材质
                    object quantity = dtl.Quantity;                                                                                                                  //总数量
                    dgDetailBill[colQuantityBill.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);                                 //数量
                    dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;                                                                        //单价
                    dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                                         //图号
                    dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                                                           //金额
                    dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                             //计量单位名称
                    dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                                             //备注
                    dgDetailBill[colSubjectName .Name ,rowIndex ].Value =dtl.SubjectName ;
                }
            }
            else
            {
                master = dgMaster.CurrentRow.Tag as StockOutRed;
                foreach (StockOutRedDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailBill.Rows.Add();
                    dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                                              //物资编码
                    dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                                             //物资名称
                    dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                            //规格型号   
                    dgDetailBill[colUsedPartBill.Name, rowIndex].Value = dtl.UsedPartName;                                                 //使用部位
                    dgDetailBill[colMaterialStuffBill.Name, rowIndex].Value = dtl.MaterialStuff;                                               //材质
                    object quantity = dtl.Quantity;                                                                                                                  //总数量
                    dgDetailBill[colQuantityBill.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);                                 //数量
                    dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;                                                                        //单价
                    dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                                         //图号
                    dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                                                           //金额
                    dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                             //计量单位名称
                    dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                                             //备注
                    dgDetailBill[colSubjectName.Name, rowIndex].Value = dtl.SubjectName;
                }
                if (master == null) return;
            }
        }
        #endregion
        #region 调拨出库查询
        void btnSearchMvBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //objectQuery.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));

            //单据
            if (txtCodeBeginMvBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginMvBill.Text, MatchMode.Anywhere));
            }
            //业务日期
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginMvBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndMvBill.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonMvBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonMvBill.Text, MatchMode.Anywhere));
            }
            //专业分类
            if (cboProfessionalCategoryMvBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("ProfessionCategory", cboProfessionalCategoryMvBill.Text));
            }
            try
            {
                list = model.StockOutSrv.GetStockMoveOut(objectQuery);
                IList redList = model.StockOutSrv.GetStockMoveOutRed(objectQuery);
                dgMasterMvBill.Rows.Clear();
                dgDetailMvBill.Rows.Clear();
                ShowMasterMvList(list, redList);
                FlashScreen.Close();
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
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterMvList(IList masterList, IList redMasterList)
        {
            if (masterList.Count == 0 && redMasterList.Count == 0) return;
            foreach (StockMoveOut master in masterList)
            {
                int rowIndex = dgMasterMvBill.Rows.Add();
                dgMasterMvBill.Rows[rowIndex].Tag = master;
                dgMasterMvBill[colCodeMvBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMasterMvBill[colSupplyInfoMvBill.Name, rowIndex].Value = master.TheSupplierName;                           //使用队伍
                dgMasterMvBill[colStockOutMannerMvBill.Name, rowIndex].Value = master.StockOutManner;         //入库类型

                //bool isLimited = true;
                //if (master.IsLimited.ToString() == "1")
                //{
                //    isLimited = true;
                //}
                //else if (master.IsLimited.ToString() == "0")
                //{
                //    isLimited = false;
                //}

                //if (isLimited == true)
                //{
                //    dgMasterMvBill[colIsLimitedBill.Name, rowIndex].Value = true;
                //}
                //else
                //{
                //    dgMasterMvBill[colIsLimitedBill.Name, rowIndex].Value = false;
                //}

                dgMasterMvBill[colCreateDateMvBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMasterMvBill[colTheStockInOutKindMvBill.Name, rowIndex].Value =3;         //红蓝单类型
                dgMasterMvBill[colCreatePersonMvBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMasterMvBill[colRealOperationDateMvBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMasterMvBill[colStateMvBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                                     //状态
                dgMasterMvBill[colDescriptMvBill.Name, rowIndex].Value = master.Descript;                                                 //备注

            }
            foreach (StockMoveOutRed master in redMasterList)
            {
                int rowIndex = dgMasterMvBill.Rows.Add();
                dgMasterMvBill.Rows[rowIndex].Tag = master;
                dgMasterMvBill[colCodeMvBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMasterMvBill[colSupplyInfoMvBill.Name, rowIndex].Value = master.TheSupplierName;                           //使用队伍
                dgMasterMvBill[colStockOutMannerMvBill.Name, rowIndex].Value = master.StockOutManner;         //入库类型

                //bool isLimited = true;
                //if (master.IsLimited.ToString() == "1")
                //{
                //    isLimited = true;
                //}
                //else if (master.IsLimited.ToString() == "0")
                //{
                //    isLimited = false;
                //}

                //if (isLimited == true)
                //{
                //    dgMaster[colIsLimitedBill.Name, rowIndex].Value = true;
                //}
                //else
                //{
                //    dgMaster[colIsLimitedBill.Name, rowIndex].Value = false;
                //}

                dgMasterMvBill[colCreateDateMvBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMasterMvBill[colTheStockInOutKindMvBill.Name, rowIndex].Value = 4;         //红蓝单类型
                dgMasterMvBill[colCreatePersonMvBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMasterMvBill[colRealOperationDateMvBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMasterMvBill[colStateMvBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                                     //状态
                dgMasterMvBill[colDescriptMvBill.Name, rowIndex].Value = master.Descript;                                                 //备注

            }
            this.dgMasterMvBill.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgMasterMvBill.CurrentCell = dgMasterMvBill[1, 0];
            dgMasterMvBill_SelectionChanged(dgMasterMvBill, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMasterMvBill_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailMvBill.Rows.Clear();
            if (dgMasterMvBill.CurrentRow.Tag == null) return;
            long stockInOutKind = ClientUtil.ToLong(dgMasterMvBill.CurrentRow.Cells["colTheStockInOutKindMvBill"].Value);
            BasicStockOut master = null;
            if (stockInOutKind == 3)
            {
                master = dgMasterMvBill.CurrentRow.Tag as StockMoveOut;
                foreach (StockMoveOutDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailMvBill.Rows.Add();
                    dgDetailMvBill[colMaterialCodeMvBill.Name, rowIndex].Value = dtl.MaterialCode;                                              //物资编码
                    dgDetailMvBill[colMaterialNameMvBill.Name, rowIndex].Value = dtl.MaterialName;                                             //物资名称
                    dgDetailMvBill[colSpecMvBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                            //规格型号   
                    dgDetailMvBill[colUsedPartMvBill.Name, rowIndex].Value = dtl.UsedPartName;                                                 //使用部位
                    dgDetailMvBill[colMaterialStuffMvBill.Name, rowIndex].Value = dtl.MaterialStuff;                                               //材质
                    object quantity = dtl.Quantity;                                                                                                                  //总数量
                    dgDetailMvBill[colQuantityMvBill.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);                                 //数量
                    dgDetailMvBill[colPriceMvBill.Name, rowIndex].Value = dtl.Price;                                                                        //单价
                    dgDetailMvBill[colDiagramNumMvBill.Name, rowIndex].Value = dtl.DiagramNumber;                                         //图号
                    dgDetailMvBill[colSumMoneyMvBill.Name, rowIndex].Value = dtl.Money;                                                           //金额
                    dgDetailMvBill[colUnitMvBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                             //计量单位名称
                    dgDetailMvBill[colDescriptMvDg.Name, rowIndex].Value = dtl.Descript;                                                             //备注
                }
            }
            else if (stockInOutKind == 4)
            {
                master = dgMasterMvBill.CurrentRow.Tag as StockMoveOutRed;
                foreach (StockMoveOutRedDtl dtl in master.Details)
                {
                    int rowIndex = dgDetailMvBill.Rows.Add();
                    dgDetailMvBill[colMaterialCodeMvBill.Name, rowIndex].Value = dtl.MaterialCode;                                              //物资编码
                    dgDetailMvBill[colMaterialNameMvBill.Name, rowIndex].Value = dtl.MaterialName;                                             //物资名称
                    dgDetailMvBill[colSpecMvBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                            //规格型号   
                    dgDetailMvBill[colUsedPartMvBill.Name, rowIndex].Value = dtl.UsedPartName;                                                 //使用部位
                    dgDetailMvBill[colMaterialStuffMvBill.Name, rowIndex].Value = dtl.MaterialStuff;                                               //材质
                    object quantity = dtl.Quantity;                                                                                                                  //总数量
                    dgDetailMvBill[colQuantityMvBill.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);                                 //数量
                    dgDetailMvBill[colPriceMvBill.Name, rowIndex].Value = dtl.Price;                                                                        //单价
                    dgDetailMvBill[colDiagramNumMvBill.Name, rowIndex].Value = dtl.DiagramNumber;                                         //图号
                    dgDetailMvBill[colSumMoneyMvBill.Name, rowIndex].Value = dtl.Money;                                                           //金额
                    dgDetailMvBill[colUnitMvBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                             //计量单位名称
                    dgDetailMvBill[colDescriptMvDg.Name, rowIndex].Value = dtl.Descript;                                                             //备注
                }
                if (master == null) return;
            }
        }
        #endregion

    }
}
