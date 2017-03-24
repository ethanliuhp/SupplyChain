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
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng
{
    public partial class VSupplyPlanMngQuery : TBasicDataView
    {
        private MSupplyPlanMng model = new MSupplyPlanMng();
        EnumStockExecType execelType;
        /// <summary>
        /// 
        /// </summary>
        public EnumStockExecType ExecType
        {
            get { return execelType; }
            set { execelType = value; }
        }

        public VSupplyPlanMngQuery(EnumStockExecType enumStockExecType)
        {
            this.ExecType = enumStockExecType;
            InitializeComponent();
            InitEvent();
            InitData();
            InitZYFL();
        }
        public VSupplyPlanMngQuery()
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
            VBasicDataOptr.InitProfessionCategory(comSpecailTypeBill, false);
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
            this.btnExcel1.Click += new EventHandler(btnExcel_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }
        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name == colCodeBill.Name)
            {
                SupplyPlanMaster master = dgMaster.Rows[e.RowIndex].Tag as SupplyPlanMaster;
                VSupplyPlanMng vmro = new VSupplyPlanMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VSupplyPlanMng vOrder = new VSupplyPlanMng();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            //if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            string billId = dgDetail.Rows[e.RowIndex].Tag as string;
            if (string.IsNullOrEmpty(billId)) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                //SupplyPlanMaster master = model.SupplyPlanSrv.GetSupplyPlanByCode(dgvCell.Value.ToString());
                SupplyPlanMaster master = model.SupplyPlanSrv.GetSupplyPlanById(billId);
                VSupplyPlanMng vmro = new VSupplyPlanMng();
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
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                    {
                        condition += "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";
                    }
                    else
                    {
                        condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                    }
                }
                //制单时间
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
                //专业分类
                if (this.comSpecailType.SelectedItem != "" && this.comSpecailType.SelectedItem != null)
                {
                    condition += "and t1.SpecialType = '" + this.comSpecailType.SelectedItem + "'";
                }
                if (this.ExecType == EnumStockExecType.安装)
                {
                    condition += "and t1.SpecialType is not null ";
                }
                //物资
                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
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
                DataSet dataSet = model.SupplyPlanSrv.SupplyPlanQuery(condition);
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
                    if (objState != null)
                    {
                        dgDetail[colStatus.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }
                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    object quantity = dataRow["Quantity"];
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                    string b = dataRow["CreateDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetail[colCreateDate.Name, rowIndex].Value = strb;
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                    dgDetail[colUint.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();
                    dgDetail[colQualityStandard.Name, rowIndex].Value = dataRow["QualityStandard"].ToString();//质量标准
                    dgDetail[colSpecialType.Name, rowIndex].Value = dataRow["SpecialType"].ToString();//专业分类
                    dgDetail[colDiagramNum.Name, rowIndex].Value = dataRow["DiagramNumber"].ToString();
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();//打印次数
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                    dgDetail[colTecNumber.Name, rowIndex].Value = dataRow["TechnologyParameter"].ToString();//技术参数
                    dgDetail[colPlanNameBill.Name, rowIndex].Value = dataRow["PlanName"].ToString();
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
            //专业分类
            if (comSpecailTypeBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("SpecialType", comSpecailTypeBill.Text, MatchMode.Anywhere));
            }
            try
            {
                list = model.SupplyPlanSrv.GetSupplyPlan(objectQuery);
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
            foreach (SupplyPlanMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colPlanTypeBill.Name, rowIndex].Value = master.PlanType;                                             //计划类型
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                         //制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;                                                 //备注
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);       //状态
                dgMaster[colbzyjBill.Name, rowIndex].Value = master.Compilation;                                                   //编制依据
                dgMaster[colPrintTimesBill.Name, rowIndex].Value = master.PrintTimes;                                                     //打印次数
                dgMaster[colPlanName.Name, rowIndex].Value = master.PlanName;
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
            SupplyPlanMaster master = dgMaster.CurrentRow.Tag as SupplyPlanMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            foreach (SupplyPlanDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                           //物资编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                           //物资名称
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                           //规格型号
                dgDetailBill[colUnitBill.Name, rowIndex].Value = dtl.MatStandardUnitName;                              //计量单位
                dgDetailBill[colDiagramNumBill.Name, rowIndex].Value = dtl.DiagramNumber;                           //图号
                dgDetailBill[colQuantityBill.Name, rowIndex].Value = dtl.Quantity;                           //数量
                dgDetailBill[colMaterialStuffBill.Name, rowIndex].Value = dtl.MaterialStuff;                                  //材质
                dgDetailBill[colMaterialCategoryBill.Name, rowIndex].Value = dtl.MaterialCategoryName;                   //材料类型名称
                dgDetailBill[colUsedPartBill.Name, rowIndex].Value = dtl.UsedPartName;                   ////使用部位名称
                dgDetailBill[colUsedRanksBill.Name, rowIndex].Value = dtl.UsedRankName;                   ////使用队伍名称
                dgDetailBill[colDescriptDtl.Name, rowIndex].Value = dtl.Descript;                                           //备注
                dgDetailBill[colTecNumberBill.Name, rowIndex].Value = dtl.TechnologyParameter;         //技术参数
                dgDetailBill[colQualityStandardBill.Name, rowIndex].Value = dtl.QualityStandard;         //质量标准
                dgDetailBill[colSpecialTypeBill.Name, rowIndex].Value = dtl.SpecialType;         //专业分类
            }
        }
    }
}
