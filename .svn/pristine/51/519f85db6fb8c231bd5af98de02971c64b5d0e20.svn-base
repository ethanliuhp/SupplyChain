using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng
{
    public partial class VConCheckQuery : TBasicDataView
    {
        MConcreteMng model = new MConcreteMng();
        public VConCheckQuery()
        {
            InitializeComponent();
            InitData();
            InitEvent();
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
        }
        private void InitEvent()
        {
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
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
                    VConCheck vOrder = new VConCheck();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
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
                DataSet dataSet = model.ConcreteMngSrv.GetConCheckingMasterQuery(condition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                decimal sumVolume = 0;
                decimal sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                    DataGridViewRow currRow = dgDetail.Rows[rowIndex];
                    currRow.Tag = ClientUtil.ToString(dataRow["id"]);
                    dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["SupplierName"].ToString();

                    object objState = dataRow["State"];
                    if (objState != null)
                    {
                        dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                    }

                    dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"].ToString();
                    dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"].ToString();
                    dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"].ToString();
                    dgDetail[colPrice.Name, rowIndex].Value = dataRow["Price"].ToString();
                    dgDetail[colMoney.Name, rowIndex].Value = dataRow["Money"].ToString();
                    dgDetail[colDescript.Name, rowIndex].Value = dataRow["Descript"].ToString();//备注
                    object money = dataRow["Money"];
                    if (money != null)
                    {
                        sumMoney += decimal.Parse(money.ToString());
                    }
                    dgDetail[colTicketVolume.Name, rowIndex].Value = dataRow["TicketVolume"].ToString();
                    object volume = dataRow["TicketVolume"];
                    if (volume != null)
                    {
                        sumVolume += decimal.Parse(volume.ToString());
                    }
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
                    dgDetail[colDeductionVolume.Name, rowIndex].Value = ClientUtil.ToString(dataRow["DeductionVolume"]);
                    dgDetail[this.colLessPumpVolume.Name, rowIndex].Value = ClientUtil.ToString(dataRow["LessPumpVolume"]);
                    dgDetail[colConversionVolume.Name, rowIndex].Value = ClientUtil.ToString(dataRow["ConversionVolume"]);
                    dgDetail[this.colUsedPart.Name, rowIndex].Value = ClientUtil.ToString(dataRow["UsedPartName"]);
                    dgDetail[this.colSubject.Name, rowIndex].Value = ClientUtil.ToString(dataRow["subjectname"]);

                    dgDetail[colBalVolume.Name, rowIndex].Value = ClientUtil.ToString(dataRow["BalVolume"]);
                    dgDetail[colCreateDate.Name, rowIndex].Value = ClientUtil.ToDateTime(dataRow["CreateDate"]).ToShortDateString();
                    dgDetail[colCreatePerson.Name, rowIndex].Value = ClientUtil.ToString(dataRow["CreatePersonName"]);
                }
                FlashScreen.Close();
                this.txtSumQuantity.Text = sumVolume.ToString("#,###.####");
                this.txtSumMoney.Text = sumMoney.ToString("#,###.####");
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

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
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
            //规格型号
            if (txtSpecBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("MaterialSpec", txtSpecBill.Text, MatchMode.Anywhere));
            }
            try
            {
                list = model.ConcreteMngSrv.GetConCheckingMaster(objectQuery);
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
            foreach (ConcreteCheckingMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;                                                           //单据号
                dgMaster[colSupplyInfoBill.Name, rowIndex].Value = master.SupplierName;                           //供应商
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();         //业务日期
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
            ConcreteCheckingMaster master = dgMaster.CurrentRow.Tag as ConcreteCheckingMaster;
            if (master == null) return;
            //dgDetail.Rows[rowIndex].Tag = dtl;
            foreach (ConcreteCheckingDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                               //物资编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                             //物资名称
                dgDetailBill[colSpecBill.Name, rowIndex].Value = dtl.MaterialSpec;                                                 //规格型号   
                dgDetailBill[colUsedPartBill.Name, rowIndex].Value = dtl.UsedPartName;                                           //使用部位
                dgDetailBill[colSubjectBill.Name, rowIndex].Value = dtl.SubjectName;
                bool IsPump = true;
                if (dtl.IsPump == true)
                {
                    IsPump = true;
                }
                else
                {
                    IsPump = false;
                }

                if (IsPump == true)
                {
                    dgDetailBill[colIsPumpBill.Name, rowIndex].Value = true;
                }
                else
                {
                    dgDetailBill[colIsPumpBill.Name, rowIndex].Value = false;
                }
                dgDetailBill[colPriceBill.Name, rowIndex].Value = dtl.Price;  //单价
                dgDetailBill[colMoneyBill.Name, rowIndex].Value = dtl.Money;  //金额
                dgDetailBill[colTicketVolumeBill.Name, rowIndex].Value = dtl.TicketVolume;//小票方量
                dgDetailBill[colDeductionVolumeBill.Name, rowIndex].Value = dtl.DeductionVolume;//其他扣减
                dgDetailBill[this.colLessPumpVolumeBill.Name, rowIndex].Value = dtl.LessPumpVolume;//抽磅扣减
                dgDetailBill[colConversionVolumeBill.Name, rowIndex].Value = dtl.ConversionVolume; //换算抽磅方量
                dgDetailBill[colBalVolumeBill.Name, rowIndex].Value = dtl.BalVolume;                                   
                //应结方量
                dgDetailBill[colDescriptDg.Name, rowIndex].Value = dtl.Descript; //备注
            }
        }
    }
}
