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
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng
{
    public partial class VMonthlyPlanMngQuery : TBasicDataView
    {
        private MMonthlyPlanMng model = new MMonthlyPlanMng();
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public VMonthlyPlanMngQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitZYFL();
        }

        private void InitZYFL()
        {
            ////添加专业分类下拉框
            VBasicDataOptr.InitProfessionCategory(comSpecailType, false);
            //comSpecailType.ContextMenuStrip = cmsDg;
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
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                MonthlyPlanMaster master =dgMaster.Rows[e.RowIndex].Tag as MonthlyPlanMaster;
                VMonthlyPlanMng vmro = new VMonthlyPlanMng();
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
                    VMonthlyPlanMng vOrder = new VMonthlyPlanMng();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
           // if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            string billId = dgDetail.Rows[e.RowIndex].Tag as string;
            if (string.IsNullOrEmpty(billId)) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
               // MonthlyPlanMaster master = model.MonthlyPlanSrv.GetMonthlyPlanByCode(dgvCell.Value.ToString());
                MonthlyPlanMaster master = model.MonthlyPlanSrv.GetMonthlyPlanById(billId);
                VMonthlyPlanMng vmro = new VMonthlyPlanMng();
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
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                #region 查询条件处理
                FlashScreen.Show("正在查询信息......");
                string condition = "";
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "' and t1.plantypebydept=1 ";
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                    {
                        condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";
                    }
                    else
                    {
                        condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                    }
                }
                //业务时间
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
                //制单年
                if (txtYear.Text != "")
                {
                    condition += "and t1.CreateYear like '" + txtYear.Text + "'";
                }

                //制单月
                if (txtMonth.Text != "")
                {
                    condition += " and t1.CreateMonth like '" + txtMonth.Text + "%'";
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
                //专业分类
                if (this.comSpecailType.SelectedItem != null)
                {
                    condition += "and t2.SpecialType = '" + comSpecailType.SelectedItem + "'";
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

                    IEnumerable<AuthConfigOrgSysCode> listAuth = model.MonthlyPlanSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();
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
                DataSet dataSet = model.MonthlyPlanSrv.MonthlyPlanQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];

                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    object objState = dataRow["State"];
                    //if (objState != null)
                    //{
                    //    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    //}

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    object quantity = dataRow["Quantity"];
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                    dgDetail[colMoney.Name, rowIndex].Value = dataRow["Money"].ToString();
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["Price"].ToString();
                    string b = dataRow["CreateDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetail[colCreateDate.Name, rowIndex].Value = strb;
                    dgDetail[colRealOperationDate.Name, rowIndex].Value = dataRow["RealOperationDate"].ToString();
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                    dgDetail[colUsedRanks.Name, rowIndex].Value = dataRow["UsedRankName"].ToString();
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();
                    dgDetail[colUsedPart.Name, rowIndex].Value = dataRow["UsedPartName"].ToString();
                    dgDetail[colDiagramNum.Name, rowIndex].Value = dataRow["DiagramNumber"].ToString();
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
                    this.txtSumMoney.Text = ClientUtil.ToString(dataRow["sumMoney"]);
                }
                FlashScreen.Close();
                this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
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
                list = model.MonthlyPlanSrv.GetMonthlyPlan(objectQuery);
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
            foreach (MonthlyPlanMaster master in masterList)
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
            MonthlyPlanMaster master = dgMaster.CurrentRow.Tag as MonthlyPlanMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            decimal sumQuantity=0;
            foreach (MonthlyPlanDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                           //物资编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                           //物资名称
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                           //规格型号
                dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                              //计量单位
                dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                object quantity = dtl.Quantity;
                if (quantity != null)
                {
                    sumQuantity += decimal.Parse(quantity.ToString());
                }
                dgDetailBill[colQuantityBill.Name, rowIndex].Value = quantity;                                                  //数量
                 dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                dgDetailBill[colMaterialStuffBill.Name, rowIndex].Value = dtl.MaterialStuff;                                  //材质
                dgDetailBill[colMaterialCategoryBill.Name, rowIndex].Value = dtl.MaterialCategoryName;                   //材料类型名称
                dgDetailBill[colUsedPartBill.Name, rowIndex].Value = dtl.UsedPartName;                   //材料类型名称
                dgDetailBill[colUsedRanksBill.Name, rowIndex].Value = dtl.UsedRankName;                   //材料类型名称
                dgDetailBill[colDescriptDtl.Name, rowIndex].Value = dtl.Descript;                                           //备注
            }
        }
    }
}
