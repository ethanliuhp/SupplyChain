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
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalRedUI;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalUI
{
    public partial class VStockInBalQuery : TBasicDataView
    {
        CurrentProjectInfo projectInfo;
        private MStockMng model = new MStockMng();
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public VStockInBalQuery()
        {
            InitializeComponent();
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
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            //专业分类
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);
            VBasicDataOptr.InitProfessionCategory(cboProfessionalCategoryBill, true);

            btnAll.Checked = true;

            this.txtMaterialCategory.rootCatCode = "01";

            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
                btnOperationOrg.Visible = false;
                txtOperationOrgDetail.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrgDetail.Tag = ConstObject.TheOperationOrg;
                btnOperationOrgDetail.Visible = false;
            }
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
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.btnOperationOrgDetail.Click += new EventHandler(btnOperationOrgDetail_Click);
        }
        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var selOrg = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = selOrg;
                txtOperationOrg.Text = selOrg.Name;
            }
        }
        private void btnOperationOrgDetail_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var selOrg = frm.Result[0] as OperationOrgInfo;
                txtOperationOrgDetail.Tag = selOrg;
                txtOperationOrgDetail.Text = selOrg.Name;
            }
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                if (projectInfo == null || string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))//分公司和公司无法打印
                {
                    MessageBox.Show("公司或者分公司无法打印验收结算单");
                    return;
                }
                else
                {
                    int stockInOutKind = int.Parse(dgMaster.Rows[e.RowIndex].Cells[colTheStockInOutKindBill.Name].Value.ToString());
                    if (stockInOutKind == 0)
                    {
                        VStockInBal vsib = new VStockInBal();
                        vsib.CurBillMaster = model.StockInSrv.GetStockInBalMasterByCode(dgMaster.Rows[e.RowIndex].Cells[colCodeBill.Name].Value.ToString(), StaticMethod.GetProjectInfo().Id);
                        vsib.Preview();
                    }
                    else
                    {
                        VStockInBalRed vsibr = new VStockInBalRed();
                        vsibr.CurBillMaster = model.StockInSrv.GetStockInBalRedMasterByCode(dgMaster.Rows[e.RowIndex].Cells[colCodeBill.Name].Value.ToString(), StaticMethod.GetProjectInfo().Id);
                        vsibr.Preview();
                    }
                }
            }
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                string stockBalType = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colTheStockInOutKind.Name].Value);

                if (stockBalType == "1")
                {
                    //领料出库红单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockInBalRed vsbred = new VStockInBalRed();
                        vsbred.Start(code,billId);
                        vsbred.ShowDialog();
                    }
                }

                if (stockBalType == "0")
                {
                    //领料出库蓝单
                    if (ClientUtil.ToString(billId) != null)
                    {
                        string code = billId;
                        VStockInBal vsb = new VStockInBal();
                        vsb.Start(code,billId);
                        vsb.ShowDialog();
                    }
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell=dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                if (projectInfo == null || string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))//分公司和公司无法打印
                {
                    MessageBox.Show("公司或者分公司无法打印验收结算单");
                    return;
                }
                else
                {
                    int stockInOutKind = int.Parse(dgDetail.Rows[e.RowIndex].Cells[colTheStockInOutKind.Name].Value.ToString());
                    if (stockInOutKind == 0)
                    {
                        VStockInBal vsib = new VStockInBal();
                        vsib.CurBillMaster = model.StockInSrv.GetStockInBalMasterByCode(dgDetail.Rows[e.RowIndex].Cells[colCode.Name].Value.ToString(), StaticMethod.GetProjectInfo().Id);
                        vsib.Preview();
                    }
                    else
                    {
                        VStockInBalRed vsibr = new VStockInBalRed();
                        vsibr.CurBillMaster = model.StockInSrv.GetStockInBalRedMasterByCode(dgDetail.Rows[e.RowIndex].Cells[colCode.Name].Value.ToString(), StaticMethod.GetProjectInfo().Id);
                        vsibr.Preview();
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

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
               // CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null && !string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))
                {
                    condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                }
                else if (txtOperationOrgDetail.Tag != null)
                {
                    condition += "and t1.opgsyscode like '" + (txtOperationOrgDetail.Tag as OperationOrgInfo).SysCode + "%'";
                }
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
                //物资
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
                if (this.txtMaterial.Text != "" && this.txtMaterial.Result != null && this.txtMaterial.Result.Count != 0)
                {
                    Material material = this.txtMaterial.Result[0] as Material;
                    if (material.Name == this.txtMaterial.Text)
                    {
                        condition = condition + " and t2.material='" + material.Id + "'";
                    }
                }
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
                }

                object proCat = cboProfessionalCategory.SelectedItem;
                if (proCat != null && !string.IsNullOrEmpty(proCat.ToString()))
                {
                    condition += " and t2.ProfessionalCategory='" + proCat + "'";
                }

                //蓝单
                if (btnBlue.Checked)
                {
                    condition += " and t1.TheStockInOutKind=0";
                }
                //红单
                if (btnRed.Checked)
                {
                    condition += " and t1.TheStockInOutKind=1";
                }
                //物资分类
                if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    MaterialCategory materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    condition += " and t2.materialcode like '" + materialCategory.Code + "%'";
                }

                #endregion
                #region 过滤数据权限
                //if (StaticMethod.IsEnabledDataAuth && ConstObject.TheLogin.TheAccountOrgInfo != null && ClientUtil.ToString(ConstObject.TheLogin.TheAccountOrgInfo.Id) != "" && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
                //{
                //    //1.查询数据权限配置
                //    ObjectQuery oqAuth = new ObjectQuery();
                //    oqAuth.AddCriterion(Expression.Eq("AuthMenu.Id", TheAuthMenu.Id));

                //    Disjunction disAuth = new Disjunction();
                //    foreach (OperationRole role in ConstObject.TheRoles)
                //    {
                //        disAuth.Add(Expression.Eq("AppRole.Id", role.Id));
                //    }
                //    oqAuth.AddCriterion(disAuth);

                //    IEnumerable<AuthConfigOrgSysCode> listAuth = model.StockInSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();
                //    if (listAuth.Count() > 0)//如果配置了权限规则
                //    {
                //        var query = from a in listAuth
                //                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.无约束
                //                    select a;

                //        if (query.Count() == 0)//未设置“无约束”规则
                //        {
                //            disAuth = new Disjunction();

                //            //2.根据数据权限定义的规则过滤数据
                //            query = from a in listAuth
                //                    where a.ApplyRule == AuthOrgSysCodeRuleEnum.该核算组织的
                //                    select a;
                //            if (query.Count() > 0)//如果配置中包含操作“该核算组织的”的权限，则无需再添加其它规则
                //            {
                //                condition += "and t1.OPGSYSCODE like  '" + ConstObject.TheLogin.TheAccountOrgInfo.SysCode + "%'";
                //            }
                //        }
                //    }
                //}
                #endregion
                DataSet dataSet = model.StockInSrv.StockInBalQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0, sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierRelationName"];

                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];

                    object quantity = dataRow["Quantity"];
                    if (quantity != null)
                    {
                        sumQuantity += ClientUtil.ToDecimal(quantity);
                    }

                    dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["price"];

                    object money = dataRow["Money"];
                    if (money != null)
                    {
                        sumMoney += ClientUtil.ToDecimal(money);
                    }
                    dgDetail[colMoney.Name, rowIndex].Value = money;

                    dgDetail[colProfessionalCategory.Name, rowIndex].Value = dataRow["ProfessionalCategory"];
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];

                    dgDetail[colTheStockInOutKind.Name, rowIndex].Value = dataRow["TheStockInOutKind"];
                    dgDetail[colCreatePersonName.Name, rowIndex].Value = dataRow["CreatePersonName"];
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"];//备注
                    dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                    dgDetail[colDiagramNum.Name, rowIndex].Value = ClientUtil.ToString(dataRow["DiagramNumber"]);

                }
                FlashScreen.Close();
                this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
                txtSumMoney.Text = sumMoney.ToString("#,###.####");
                lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
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
           // CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))
            {
                objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            }
            else if (txtOperationOrg.Tag != null)
            {
                objectQuery.AddCriterion(Expression.Like("OpgSysCode", (txtOperationOrg.Tag as OperationOrgInfo).SysCode, MatchMode.Start));
            }
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
                list = model.StockInSrv.GetStockInBal(objectQuery);
                IList redList = model.StockInSrv.GetStockInBalRedMaster(objectQuery);
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
        private void ShowMasterList(IList masterList,IList redMasterList)
        {
            if (masterList.Count == 0 && redMasterList.Count==0) return;
            foreach (StockInBalMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colSupplierBill.Name, rowIndex].Value = master.TheSupplierName;                                                 //供应商
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                dgMaster[colTheStockInOutKindBill.Name, rowIndex].Value =0;                                        //红蓝单类型
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                                     //状态
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                 //备注

            }
            foreach (StockInBalRedMaster master in redMasterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colSupplierBill.Name, rowIndex].Value = master.TheSupplierName;                                                 //供应商
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value =master.CreatePersonName;                                        //制单人
                dgMaster[colTheStockInOutKindBill.Name, rowIndex].Value = 1;                                        //红蓝单类型
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
            BaseStockInBalMaster master = null;
            if (stockInOutKind == 0)
            {
                master = dgMaster.CurrentRow.Tag as StockInBalMaster;
                foreach (StockInBalDetail dtl in master.Details)
                {
                    int rowIndex = dgDetailBill.Rows.Add();
                    dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                    dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                    dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                 //规格型号   
                    object quantity = dtl.Quantity;//总数量
                    dgDetailBill[colQuantityBill.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);//数量
                    dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;                                               //单价
                    dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                    dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                                       //金额
                    dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                 //计量单位名称
                    dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                           //备注
                }
            }
            else if (stockInOutKind == 1)
             {
                master = dgMaster.CurrentRow.Tag as StockInBalRedMaster;
                foreach (StockInBalRedDetail dtl in master.Details)
                {   
                    int rowIndex = dgDetailBill.Rows.Add();
                    dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                    dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                    dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                 //规格型号   
                    object quantity = dtl.Quantity;//总数量
                    dgDetailBill[colQuantityBill.Name, rowIndex].Value = ClientUtil.ToDecimal(quantity);//数量
                    dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;                                               //单价
                    dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                    dgDetailBill[colSumMoneyBill.Name, rowIndex].Value = dtl.Money;                                       //金额
                    dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                                 //计量单位名称
                    dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                           //备注
                }
            }
        }
    }
}
