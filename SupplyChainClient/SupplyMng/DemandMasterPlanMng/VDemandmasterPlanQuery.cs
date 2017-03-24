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
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng
{
    public partial class VDemandMasterPlanQuery : TBasicDataView
    {
        private MDemandMasterPlanMng model = new MDemandMasterPlanMng();
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public VDemandMasterPlanQuery()
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
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtCreatePerson.OwnerOrgGuid = StaticMethod.GetProjectInfo().OwnerOrg.Id;
            this.txtMaterialCategory.rootCatCode = "01";
            
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                DemandMasterPlanMaster master = dgMaster.CurrentRow.Tag as DemandMasterPlanMaster;
                VDemandMasterPlanMng vmro = new VDemandMasterPlanMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colCode.Name))
            {
                return;
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VDemandMasterPlanMng vOrder = new VDemandMasterPlanMng();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            string billId = dgDetail.Rows[e.RowIndex].Tag as string;
            if (string.IsNullOrEmpty(billId)) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
               // DemandMasterPlanMaster master = model.DemandPlanSrv.GetDemandMasterPlanByCode(dgvCell.Value.ToString());
                DemandMasterPlanMaster master = model.DemandPlanSrv.GetDemandMasterPlanById(billId);
                VDemandMasterPlanMng vmro = new VDemandMasterPlanMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            //this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //SendMessageUtil sUtil = new SendMessageUtil();
                //sUtil.SendICUMsg("桑子敏", "梦龙测试", "项目管理测试11", false, true);
                //return;
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'  and t1.plantype=1 ";
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                    //if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                    //{
                    //    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                    //}
                    //else
                    //{
                    //    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                    //}
                }
                //业务日期
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
                //负责人
                if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
                {
                    condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
                }
                //制单人
                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
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

                //物资分类
                if (txtMaterialCategory.Text != "" && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    MaterialCategory materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    condition += " and t2.materialcode like '" + materialCategory.Code + "%'";
                }
                //规格型号
                if (this.txtSpec.Text != "")
                {
                    condition = condition + " and t2.MaterialSpec like '%" + this.txtSpec.Text + "%'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                //else
                //{
                //    condition += "and t1.State <> " + " 0 " + "";
                //}
                #endregion
                #region 过滤数据权限
                if (StaticMethod.IsEnabledDataAuth && ConstObject.TheLogin.TheAccountOrgInfo != null && ClientUtil.ToString(ConstObject.TheLogin.TheAccountOrgInfo.Id) != "" && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
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

                    IEnumerable<AuthConfigOrgSysCode> listAuth = model.DemandPlanSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();
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
                DataSet dataSet = model.DemandPlanSrv.DemandMstPlanQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0;
                decimal sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];

                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    //dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["originalContractNo"];
                    //dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["ORGNAME"];

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();//物资编码
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();//物资名称
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString(); //规格型号
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();//计量单位
                    object quantity = dataRow["Quantity"];//总数量
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["Price"].ToString();//单价
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                    dgDetail[colbzyj.Name, rowIndex].Value = dataRow["Compilation"].ToString();//编制依据
                    dgDetail[colwzccfl.Name, rowIndex].Value = dataRow["MaterialCategoryName"].ToString();//物资分类层次/材料类型
                    dgDetail[colzyfl.Name, rowIndex].Value = dataRow["SpecialType"].ToString();//专业分类
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();//制单人
                    dgDetail[colDiagramNum.Name, rowIndex].Value = dataRow["DiagramNumber"].ToString();//图号
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();//打印次数
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();//制单日期
                    string b = dataRow["CreateDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetail[colCreateDate.Name, rowIndex].Value = strb;//业务日期
                    dgDetail[colSumMoney.Name, rowIndex].Value = dataRow["Money"].ToString();//金额
                    object money = dataRow["Money"];//总数量
                    if (money != null)
                    {
                        sumMoney += decimal.Parse(money.ToString());
                    }
                }
                FlashScreen.Close();
                this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
                this.txtSumMoney.Text = sumMoney.ToString("#,###,####");
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
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //objectQuery.AddCriterion(Expression.Eq("ProjectName", projectInfo.Name));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

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
            try
            {
                list = model.DemandPlanSrv.GetDemandMasterPlan(objectQuery);
                dgMaster.Rows.Clear();
                dgDetailBill.Rows.Clear();
                ShowMasterList(list);
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
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (DemandMasterPlanMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colPlanBill.Name, rowIndex].Value = master.PlanName;                                                     //计划名称
                dgMaster[colPlanTypeBill.Name, rowIndex].Value = master.PlanType;                                             //计划类型
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                         //制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                 //备注
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);       //状态
                dgMaster[colbzyjBill.Name, rowIndex].Value = master.Compilation;                                                   //编制依据
                dgMaster[colPrintTimesBill.Name, rowIndex].Value = master.PrintTimes;                                                     //打印次数
               
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
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
            DemandMasterPlanMaster master = dgMaster.CurrentRow.Tag as DemandMasterPlanMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            foreach (DemandMasterPlanDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                           //物资编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                           //物资名称
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                           //规格型号
                dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                              //计量单位
                dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                dgDetailBill[colMaterialStuffBill.Name, rowIndex].Value = dtl.MaterialStuff;                                  //材质
                dgDetailBill[colMaterialCategoryBill.Name, rowIndex].Value = dtl.MaterialCategoryName;                   //材料类型名称
                dgDetailBill[colDescriptDtl.Name, rowIndex].Value = dtl.Descript;                                           //备注
                dgDetailBill[colQuantityBill.Name, rowIndex].Value = dtl.Quantity;
                
                    
            }
        }

        private void dgDetailBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
