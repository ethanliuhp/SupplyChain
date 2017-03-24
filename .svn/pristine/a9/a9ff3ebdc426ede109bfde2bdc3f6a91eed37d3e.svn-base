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
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.Collections;
using System.Windows.Documents;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    public partial class VLaborSporadicQuery : TBasicDataView
    {
        private MLaborSporadicMng model = new MLaborSporadicMng();
        string strName = "";
        public VLaborSporadicQuery(string name)
        {
            InitializeComponent();
            strName = name;
            InitEvent();
            InitData();
            InitSporadicType();
        }

        private void InitSporadicType()
        {
            if(strName == "零星用工单商务查询")
            {
                colAccountQuantity.Visible = true;
                colAccountPrice.Visible = true;
                colAccountMoney.Visible = true;
            }
            if (strName == "零星用工单生产查询")
            {
                colAccountQuantity.Visible = false;
                colAccountPrice.Visible = false;
                colAccountMoney.Visible = false;
            }
            txtLaborType.Items.AddRange(new object[] { "计时派工", "零星用工", "代工" });
        }

        private void InitData()
        {
            comMngType.Items.Clear();
            string[] tmp = Enum.GetNames(typeof(DocumentState));
            foreach (string strName in tmp)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = strName;
                comMngType.Items.Add(li);
            }
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
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
            txtSporadicRank.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-";
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                LaborSporadicMaster master = model.LaborSporadicSrv.GetLaborSporadicByCode(dgvCell.Value.ToString());
                VLaborSporadic vmro = new VLaborSporadic();
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
                string condition = "";
                FlashScreen.Show("正在查询信息......");
                //单据号
                if (this.txtCodeBegin.Text != "")
                {
                    if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                    {
                        condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                    }
                    else
                    {
                        condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                    }
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

                //制单人
                if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
                {
                    condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
                }

                //承担队伍
                if (txtSporadicRank.Text != "" && txtSporadicRank.Result.Count > 0)
                {
                    condition += "and t1.BearTeamName = '" + txtSporadicRank.Text + "'";
                }

                //派工类型
                if (this.txtLaborType.SelectedItem != null)
                {
                    condition += "and t1.LaborState = '" + txtLaborType.SelectedItem + "'";
                }
                else
                {
                    condition += "and t1.LaborState <> '代工'";
                }
                if (comMngType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int values = ClientUtil.ToInt(li.Value);
                    condition += "and t1.State= '" + values + "'";
                }
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
                #endregion
                DataSet dataSet = model.LaborSporadicSrv.LaborSporadicQuery(condition);
                this.dgDetail.Rows.Clear();
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"].ToString();
                    dgDetail[colBearTeam.Name, rowIndex].Value = dataRow["BearTeamName"].ToString();//承担队伍
                    string b = dataRow["RealOperationDate"].ToString();
                    string[] bArray = b.Split(' ');
                    string strb = bArray[0];
                    dgDetail[colComPleteDate.Name, rowIndex].Value = strb;//完成时间
                    string d = dataRow["CreateDate"].ToString();
                    string[] dArray = d.Split(' ');
                    string strd = dArray[0];
                    dgDetail[colCreateDate.Name, rowIndex].Value = strd;
                    dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"].ToString();
                    string a = dataRow["EndDate"].ToString();
                    string[] aArray = a.Split(' ');
                    string stra = aArray[0];
                    dgDetail[colEndDate.Name, rowIndex].Value = stra;
                    dgDetail[colLaborSubject.Name, rowIndex].Value = dataRow["LaborSubjectName"].ToString();//用工科目
                    dgDetail[colPredictSpotadicNum.Name, rowIndex].Value = dataRow["PredictLaborNum"].ToString();//计划用工量
                    dgDetail[colPriceUnit.Name, rowIndex].Value = dataRow["PriceUnitName"].ToString();//价格单位
                    dgDetail[colAccountMoney.Name, rowIndex].Value = dataRow["AccountSumMoney"].ToString();
                    dgDetail[colAccountQuantity.Name, rowIndex].Value = dataRow["AccountLaborNum"].ToString();
                    dgDetail[colAccountPrice.Name, rowIndex].Value = dataRow["AccountPrice"].ToString();
                    dgDetail[colProjectTastDetail.Name, rowIndex].Value = dataRow["ProjectTastDetailName"].ToString();//工程任务明细
                    dgDetail[colProjectTastType.Name, rowIndex].Value = dataRow["ProjectTastName"].ToString();//工程任务类型
                    dgDetail[colQuantityUnit.Name, rowIndex].Value = dataRow["QuantityUnitName"].ToString();//数量单位
                    dgDetail[colRealSporadicNum.Name, rowIndex].Value = dataRow["RealLaborNum"].ToString();//实际用工量
                    dgDetail[colSporadicType.Name, rowIndex].Value = dataRow["LaborState"].ToString();//用工类型
                    string c = dataRow["StartDate"].ToString();
                    string[] cArray = c.Split(' ');
                    string strc = cArray[0];
                    dgDetail[colStartDate.Name, rowIndex].Value = strc;
                    string strIsCreate = dataRow["IsCreate"].ToString();
                    if (strIsCreate.Equals("0"))
                    {
                        dgDetail[colIsCreate.Name, rowIndex].Value = "未审核";
                    }
                    if (strIsCreate.Equals("1"))
                    {
                        dgDetail[colIsCreate.Name, rowIndex].Value = "已审核";
                    }
                    if (ClientUtil.ToDateTime(dataRow["RealOperationDate"]) > ClientUtil.ToDateTime("1900-01-01"))
                    {
                        dgDetail[colRealOperationDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["RealOperationDate"]).ToString();     //制单时间;
                    }
                }
                FlashScreen.Close();
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
