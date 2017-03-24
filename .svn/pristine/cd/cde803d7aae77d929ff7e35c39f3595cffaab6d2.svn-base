using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.PumpingPoundsMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng
{
    public partial class VPumpingPoundQuery : TBasicDataView
    {
        MConcreteMng model = new MConcreteMng();
        public VPumpingPoundQuery()
        {
            InitializeComponent();
            this.InitData();
            this.InitEvent();
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
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
        }
        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            btnSelectWBS.Click += new EventHandler(btnSelectWBS_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VPumpingPound vOrder = new VPumpingPound();
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
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                PumpingPoundsMaster master = model.ConcreteMngSrv.GetPumpingPoundsMasterByCode(ClientUtil.ToString(dgvCell.Value));
                VPumpingPound vPumPouring = new VPumpingPound();
                vPumPouring.PumpPoundsMaster = master;
                vPumPouring.Preview();
            }
        }

        void btnSelectWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                txtUesdPart.Tag = (list[0] as TreeNode).Tag as GWBSTree;
                txtUesdPart.Text = (list[0] as TreeNode).Text;
            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
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
                if (this.txtCodeBegin.Text != "")
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";
                }

                if (StaticMethod.IsUseSQLServer())
                {
                    if (rbCreateDate.Checked == true)
                    {
                        condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                    }
                    if (rbPouringDate.Checked == true)
                    {
                        condition += " and t1.PouringDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.PouringDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                    }
                }
                else
                {
                    if (rbCreateDate.Checked == true)
                    {
                        condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                    }
                    if (rbPouringDate.Checked == true)
                    {
                        condition += " t1.PouringDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.PouringDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                    }
                }
                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }
                if (txtUesdPart.Text != "")
                {
                    condition = condition + "and t1.UsedPartName like'" + txtUesdPart.Text.ToString() + "'";
                }
                if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
                {
                    condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
                }

                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }

                if (this.txtMaterial.Text != "")
                {
                    condition = condition + " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
                }
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
                DataSet dataSet = model.ConcreteMngSrv.GetPumpingPoundsMasterQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                decimal sumNetWeight = 0;
                decimal sumTicketWeight = 0;
                decimal sumDiffAmount = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);

                    dgDetail[colSupplier.Name, rowIndex].Value = dataRow["SupplierName"].ToString();

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                    this.dgDetail[colTareWeight.Name, rowIndex].Value = dataRow["TareWeight"].ToString();
                    this.dgDetail[colGrossWeight.Name, rowIndex].Value = dataRow["GrossWeight"].ToString();
                    this.dgDetail[colNetWeight.Name, rowIndex].Value = dataRow["NetWeight"].ToString();
                    object netWeight = dataRow["NetWeight"];
                    if (netWeight != null)
                    {
                        sumNetWeight += decimal.Parse(netWeight.ToString());
                    }
                    this.dgDetail[colTicketVolume.Name, rowIndex].Value = dataRow["TicketVolume"].ToString();
                    this.dgDetail[colTicketWeight.Name, rowIndex].Value = dataRow["TicketWeight"].ToString();
                    object ticketWeight = dataRow["TicketWeight"];
                    if (netWeight != null)
                    {
                        sumTicketWeight += decimal.Parse(netWeight.ToString());
                    }
                    this.dgDetail[colPlateNum.Name, rowIndex].Value = dataRow["PlateNumber"].ToString();
                    this.dgDetail[colDiffAmount.Name, rowIndex].Value = dataRow["DiffAmount"].ToString();
                    dgDetail[colUsedPart.Name, rowIndex].Value = dataRow["UsedPartName"].ToString();
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
                    object diffAmount = dataRow["DiffAmount"];
                    if (netWeight != null)
                    {
                        sumDiffAmount += decimal.Parse(diffAmount.ToString());
                    }
                }
                FlashScreen.Close();
                this.txtSumWeight.Text = sumNetWeight.ToString("#,###.####");
                this.txtSumDiffAmount.Text = sumDiffAmount.ToString("#,###.####");
                this.txtSumTicketWeight.Text = sumTicketWeight.ToString("#,###.####");
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
            //规格型号
            if (txtSpecBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("MaterialSpec", txtSpecBill.Text, MatchMode.Anywhere));
            }
            try
            {
                list = model.ConcreteMngSrv.GetPumpingPoundsMaster(objectQuery);
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
            foreach (PumpingPoundsMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colUsedPartBill.Name, rowIndex].Value = master.UsedPartName;                           //使用部位
                dgMaster[colSupplierBill.Name, rowIndex].Value = master.SupplierName;                                                 //供应商
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;                                        //制单人
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();     //制单时间;
                }
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);                                     //状态
                dgMaster[colPrintTimesBill.Name, rowIndex].Value = master.PrintTimes;                                           //打印次数
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
            PumpingPoundsMaster master = dgMaster.CurrentRow.Tag as PumpingPoundsMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl; 
            foreach (PumpingPoundsDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                              //规格型号   
                dgDetailBill[colTareWeightBill.Name, rowIndex].Value = dtl.TareWeight;                                               //皮重
                dgDetailBill[colGrossWeightBill.Name, rowIndex].Value = dtl.GrossWeight;                                     //毛重
                dgDetailBill[colNetWeightBill.Name, rowIndex].Value = dtl.NetWeight;                                              //净重
                dgDetailBill[colTicketVolumeBill.Name, rowIndex].Value = dtl.TicketVolume;                                       //小票方量
                dgDetailBill[colTicketWeightBill.Name, rowIndex].Value = dtl.TicketWeight;                                                 //小票重量
                dgDetailBill[colDiffAmountBill.Name, rowIndex].Value = dtl.DiffAmount;                                             //量差
                dgDetailBill[colPlateNumBill.Name, rowIndex].Value = dtl.PlateNumber;                                                        //车牌号
                dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript;                                                    //备注
   
            }
        }
    }
}
