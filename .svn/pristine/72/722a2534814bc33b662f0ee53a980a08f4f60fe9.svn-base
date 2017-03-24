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
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng
{
    public partial class VConPouringNoteQuery : TBasicDataView
    {
        MConcreteMng model = new MConcreteMng();
        public VConPouringNoteQuery()
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
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            txtInportSupplier.SupplierCatCode = CommonUtil.SupplierCatCode3;
            txtExportSupplier.SupplierCatCode = CommonUtil.SupplierCatCode3;
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            btnSelectWBS.Click += new EventHandler(btnSelectWBS_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = dgDetail.Rows[e.RowIndex].Tag as string;
                if (ClientUtil.ToString(billId) != "")
                {
                    VConPouringNote vOrder = new VConPouringNote();
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
                PouringNoteMaster master = model.ConcreteMngSrv.GetPouringNoteMasterByCode(ClientUtil.ToString(dgvCell.Value));
                VConPouringNote vConPouring = new VConPouringNote();
                vConPouring.PouringNoteMaster = master;
                vConPouring.Preview();
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
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        condition += " and t2.PouringDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t2.PouringDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
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
                        condition += " t2.PouringDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t2.PouringDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                    }
                }

                if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.SupplierRelation='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }
                if (this.txtInportSupplier.Text != "" && this.txtInportSupplier.Result != null && this.txtInportSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.InportSupplier='" + (this.txtInportSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }
                if (this.txtExportSupplier.Text != "" && this.txtExportSupplier.Result != null && this.txtExportSupplier.Result.Count != 0)
                {
                    condition = condition + " and t1.ExportSupplier='" + (this.txtExportSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                }
                if (txtUesdPart.Text != "")
                {
                    condition = condition + "and t2.UsedPartName like'" + txtUesdPart.Text.ToString() + "'";
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
                if (this.cbAll.Checked == true) { }
                if (this.cbIsPump.Checked == true)
                {
                    condition = condition + " and t2.IsPump=1";
                }
                else if (this.cbIsNotPump.Checked == true)
                {
                    condition = condition + " and t2.IsPump=0";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                #endregion
                DataSet dataSet = model.ConcreteMngSrv.GetPouringNoteMasterQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                decimal sumQuantity = 0;
                decimal sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];

                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierName"].ToString();
                    dgDetail[colInportSupplier.Name, rowIndex].Value = dataRow["InportSupplierName"].ToString();
                    dgDetail[colExportSupplier.Name, rowIndex].Value = dataRow["ExportSupplierName"].ToString();
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                    DateTime date = Convert.ToDateTime(dataRow["PouringDate"]);
                    dgDetail[colPouringDate.Name, rowIndex].Value = date.ToShortDateString();
                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    object quantity = dataRow["Quantity"];


                    bool IsPump = true;
                    if (dataRow["IsPump"].ToString() == "1")
                    {
                        IsPump = true;
                    }
                    else if (dataRow["IsPump"].ToString() == "0")
                    {
                        IsPump = false;
                    }
                    if (IsPump == true)
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = true;
                    }
                    else
                    {
                        dgDetail[colIsPump.Name, rowIndex].Value = false;
                    }
                    if (quantity != null)
                    {
                        sumQuantity += decimal.Parse(quantity.ToString());
                    }
                    dgDetail[colInportQty.Name, rowIndex].Value = dataRow["ImportQty"].ToString();
                    dgDetail[colInportPrice.Name, rowIndex].Value = dataRow["ImportPrice"].ToString();
                    dgDetail[colInportMoney.Name, rowIndex].Value = dataRow["IMportMoney"].ToString();

                    dgDetail[colExportQty.Name, rowIndex].Value = dataRow["ExportQty"].ToString();
                    dgDetail[colExportPrice.Name, rowIndex].Value = dataRow["ExportPrice"].ToString();
                    dgDetail[colExportMoney.Name, rowIndex].Value = dataRow["ExportMoney"].ToString();

                    dgDetail[colConsumeQty.Name, rowIndex].Value = dataRow["ConsumeQty"].ToString();
                    dgDetail[colConsumePrice.Name, rowIndex].Value = dataRow["ConsumePrice"].ToString();
                    dgDetail[colConsumeMoney.Name, rowIndex].Value = dataRow["ConsumeMoney"].ToString();

                    dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                    dgDetail[colUsedPart.Name, rowIndex].Value = dataRow["UsedPartName"].ToString();
                    dgDetail[colSubject.Name, rowIndex].Value = dataRow["subjectname"].ToString();
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["Price"].ToString();
                    dgDetail[colMoney.Name, rowIndex].Value = dataRow["Money"].ToString();
                    decimal money = ClientUtil.ToDecimal(dataRow["Money"].ToString());
                    sumMoney += money;
                    dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"].ToString();
                    dgDetail[colPrintTimes.Name, rowIndex].Value = dataRow["PrintTimes"].ToString();
                }
                FlashScreen.Close();
                this.txtMoney.Text = sumMoney.ToString("#,###.####");
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
    }
}
