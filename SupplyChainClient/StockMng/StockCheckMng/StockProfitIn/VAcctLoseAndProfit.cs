using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
//using Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockRelationUI;
//using Application.Business.Erp.SupplyChain.Client.Basic.CommonForm.BillPrint;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
//using Application.Business.Erp.SupplyChain.StockAccManage.OutGo1Mng.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.Util;
//using Application.Business.Erp.SupplyChain.Client.StockAccManage.MonthEndPriceUI;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public partial class VAcctLoseAndProfit : TMasterDetailView
    {
        private IMProfitIn theMProfitIn = StaticMethod.GetRefModule(typeof(MProfitIn)) as IMProfitIn;
        public AcctLoseAndProfit theAcctLostAndProfit = new AcctLoseAndProfit();
        public VAcctLoseAndProfitList theVList;

        public VAcctLoseAndProfit()
        {
            InitializeComponent();
            Title = "������ӯ�̿���";
            InitEvent();
            InitData();
        }

        public void InitEvent()
        {
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellEnter += new DataGridViewCellEventHandler(dgDetail_CellEnter);
            this.dgDetail.CellValidated += new DataGridViewCellEventHandler(dgDetail_CellValidated);
        }

        void dgDetail_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name == "Price" ||
                this.dgDetail.Columns[e.ColumnIndex].Name == "Quantity")
            {
                this.dgDetail["Money", e.RowIndex].Value = ClientUtil.TransToDecimal(this.dgDetail["Price", e.RowIndex].Value) *
                                                           ClientUtil.TransToDecimal(this.dgDetail["Quantity", e.RowIndex].Value);
                this.dgDetail["Money", e.RowIndex].Value = ClientUtil.TransToDecimal(this.dgDetail["Money", e.RowIndex].Value).ToString("#,###.##");
            }
        }

        void dgDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            switch (this.dgDetail.Columns[e.ColumnIndex].Name)
            {
                case "MaterialName":
                case "MaterialSpec":
                case "Stuff":
                case "Unit":
                case "Money":
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    break;
                default:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    this.dgDetail.BeginEdit(true);
                    break;
            }
        }

        public void InitData()
        {
            this.lblCustomer.Visible = false;
            this.txtSupplier.Visible = false;
            this.lblSupplier.Visible = false;
            this.txtCustomer.Visible = false;
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string busType = cboBusType.Text;
            if (busType == null || "".Equals(busType))
            {
                MessageBox.Show("��ѡ��ҵ�����ͣ�");
                return;
            }

            if (this.txtSupplier.Result == null || this.txtSupplier.Result.Count == 0)
            {
                MessageBox.Show("��ѡ��Ӧ�̣�");
                return;
            }

            /*if (busType.IndexOf("����") != -1 && (this.txtCustomer.Result == null || this.txtCustomer.Result.Count == 0))
            {
                MessageBox.Show("��ѡ��ͻ���");
                return;
            }*/

            if (e.ColumnIndex == -1) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals("MaterialCode"))
            {
                CommonMaterial materialSelector = new CommonMaterial();
                DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];

                object tempValue = cell.EditedFormattedValue;
                if (tempValue != null && !tempValue.Equals(""))
                {
                    materialSelector.OpenSelect(tempValue.ToString());
                }
                else
                {
                    materialSelector.OpenSelect();
                }

                IList list = materialSelector.Result;
                Material selectedMaterial = null;
                if (list != null && list.Count > 0)
                {
                    selectedMaterial = list[0] as Material;
                    cell.Tag = selectedMaterial;
                    cell.Value = selectedMaterial.Code;
                    this.dgDetail.CurrentRow.Cells["MaterialName"].Value = selectedMaterial.Name;
                    this.dgDetail.CurrentRow.Cells["MaterialSpec"].Value = selectedMaterial.Specification;
                    this.dgDetail.CurrentRow.Cells["Stuff"].Value = selectedMaterial.Stuff;
                    this.dgDetail.CurrentRow.Cells["Unit"].Value = selectedMaterial.BasicUnit.Name;
                    this.dgDetail.CurrentRow.Cells["Unit"].Tag = selectedMaterial.BasicUnit;

                    if (list.Count > 1)
                    {
                        for (int i = 1; i < list.Count; i++)
                        {
                            selectedMaterial = list[i] as Material;
                            int newRowIndex = this.dgDetail.Rows.Add();
                            DataGridViewRow newRow = this.dgDetail.Rows[newRowIndex];

                            newRow.Cells["MaterialCode"].Value = selectedMaterial.Code;
                            newRow.Cells["MaterialName"].Value = selectedMaterial.Name;
                            newRow.Cells["MaterialSpec"].Value = selectedMaterial.Specification;
                            newRow.Cells["Stuff"].Value = selectedMaterial.Stuff;

                            newRow.Cells["Unit"].Value = selectedMaterial.BasicUnit.Name;
                            newRow.Cells["Unit"].Tag = selectedMaterial.BasicUnit;
                        }
                    }
                    this.dgDetail.RefreshEdit();
                }
            }

        }

        public void Start(string code)
        {
            try
            {
                if (code == "��")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    theAcctLostAndProfit = theMProfitIn.GetObjectByAcct(code);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("��ͼ��������" + ExceptionUtil.ExceptionMessage(e)); ;
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //��������ؼ�
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //��������
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate };
            ObjectLock.Lock(os);
        }

        private void ClearView()
        {
            ClearControl(pnlFloor);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //�Զ���ؼ����
            if (c is CustomEdit)
            {
                if (c.Name != "txtCreatePerson" && c.Name != "txtCreateDate")
                {
                    c.Tag = null;
                    c.Text = "";
                }
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        public override bool NewView()
        {
            try
            {
                base.NewView();
                theMProfitIn.New();
                theAcctLostAndProfit = new AcctLoseAndProfit();
                ClearView();

                theAcctLostAndProfit.Code = DateTime.Now.TimeOfDay.ToString();
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        public override bool ModifyView()
        {
            if (ifHaveModify() == false)
            {
                MessageBox.Show("�˵������ͻ�����ѹ��������޸ģ�");
                return false;
            }
            base.ModifyView();
            theMProfitIn.Modify();
            theAcctLostAndProfit = theMProfitIn.GetObjectByAcct(theAcctLostAndProfit.Id);
            ModelToView();
            return true;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            bool isSaveSuccess = false;
            try
            {
                if (!ViewToModel()) return false;
                theAcctLostAndProfit = theMProfitIn.SaveByAcct(theAcctLostAndProfit);
                txtCode.Text = theAcctLostAndProfit.Code;

                //����Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;

                isSaveSuccess = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(e)); ;
                return isSaveSuccess;
            }

        }

        //�ж��Ƿ���Խ���ɾ�����޸�
        private bool ifHaveModify()
        {
            bool ifHave = true;
            int t_kjn = theAcctLostAndProfit.CreateYear;
            int t_kjy = theAcctLostAndProfit.CreateMonth;
            bool ifAcct = theMProfitIn.QueryStockAcct(t_kjn, t_kjy);

            if (ifAcct == true)
            {
                ifHave = false;
            }
            return ifHave;
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                if (ifHaveModify() == false)
                {
                    throw new Exception("�˵������ͻ�����ѹ�������ɾ����");
                    return false;
                }

                if (!theMProfitIn.DeleteByAcct(theAcctLostAndProfit)) return false;

                ClearView();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("����ɾ������" + ExceptionUtil.ExceptionMessage(e)); ;
                return false;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //���²�ѯ����
                        theAcctLostAndProfit = theMProfitIn.GetObjectByAcct(theAcctLostAndProfit.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݳ�������" + ExceptionUtil.ExceptionMessage(e)); ;
                return false;
            }
        }

        /// <summary>
        /// ˢ��
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //���»�õ�ǰ�����ֵ
                theAcctLostAndProfit = theMProfitIn.GetObjectByAcct(theAcctLostAndProfit.Id);
                //�����渳ֵ
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("����ˢ�´���" + ExceptionUtil.ExceptionMessage(e)); ;
            }
        }

        /// <summary>
        /// ��������ǰУ������
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            string busType = cboBusType.Text;
            if (busType == null || "".Equals(busType))
            {
                MessageBox.Show("��ѡ��ҵ�����ͣ�");
                return false;
            }

            if (this.txtSupplier.Result == null || this.txtSupplier.Result.Count == 0)
            {
                MessageBox.Show("��ѡ��Ӧ�̣�");
                return false;
            }

            /*if (busType.IndexOf("����") != -1 && (this.txtCustomer.Result == null || this.txtCustomer.Result.Count == 0))
            {
                MessageBox.Show("��ѡ��ͻ���");
                return false;
            }*/

            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("��ϸ����Ϊ��!");
                return false;
            }

            this.dgDetail.CurrentCell = this.dgDetail[0, 0];
            //SupplierRelationInfo tmpSupplierRelationInfo = this.txtSupplier.Result[0] as SupplierRelationInfo;

            //��ϸ������У��
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //���һ�в�����У��
                if (dr.IsNewRow) break;

                if (dr.Cells["MaterialCode"].Tag == null)
                {
                    MessageBox.Show("���ϲ�����Ϊ�գ�");
                    dgDetail.CurrentCell = dr.Cells["MaterialCode"];
                    return false;
                }

                if (dr.Cells["Quantity"].Value == null || dr.Cells["Quantity"].Value.ToString() == "")
                {
                    MessageBox.Show("����������Ϊ�գ�");
                    dgDetail.CurrentCell = dr.Cells["Quantity"];
                    return false;
                }

                if (dr.Cells["Remark"].Value == null)
                    dr.Cells["Remark"].Value = "";
            }
            dgDetail.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
            return true;
        }
        //��������
        private bool ViewToModel()
        {
            if (!ValidView()) return false;

            try
            {
                if (this.txtSupplier.Text != null && !"".Equals(this.txtSupplier.Text))
                {
                    theAcctLostAndProfit.TheSupplierRelationInfo = this.txtSupplier.Result[0] as SupplierRelationInfo;
                }

                /*if (this.txtCustomer.Text != null && !"".Equals(this.txtCustomer.Text))
                {
                    theAcctLostAndProfit.TheCustomerRelationInfo = this.txtCustomer.Result[0] as CustomerRelationInfo;
                }*/

                theAcctLostAndProfit.CreatePerson = this.txtCreatePerson.Tag as PersonInfo;
                theAcctLostAndProfit.CreateDate = StringUtil.StrToDateTime(this.txtCreateDate.Text);
                theAcctLostAndProfit.BusinessTypeName = cboBusType.Text;
                theAcctLostAndProfit.BusinessType = cboBusType.SelectedIndex;
                theAcctLostAndProfit.Descript = this.txtRemark.Text;
                foreach (DataGridViewRow dr in this.dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    AcctLoseAndProfitDtl theAcctLostAndProfitDtl = new AcctLoseAndProfitDtl();
                    theAcctLostAndProfitDtl = dr.Tag as AcctLoseAndProfitDtl;

                    if (theAcctLostAndProfitDtl == null)
                        theAcctLostAndProfitDtl = new AcctLoseAndProfitDtl();
                    else if (theAcctLostAndProfitDtl.Id == "")
                    {
                        theAcctLostAndProfit.Details.Remove(theAcctLostAndProfitDtl);
                    }

                    theAcctLostAndProfitDtl.MaterialResource = dr.Cells["MaterialCode"].Tag as Material;
                    theAcctLostAndProfitDtl.MatStandardUnit = dr.Cells["Unit"].Tag as StandardUnit;
                    theAcctLostAndProfitDtl.Quantity = StringUtil.StrToDecimal(ClientUtil.ToString(dr.Cells["Quantity"].Value));
                    theAcctLostAndProfitDtl.Price = StringUtil.StrToDecimal(ClientUtil.ToString(dr.Cells["Price"].Value));
                    theAcctLostAndProfitDtl.Money = StringUtil.StrToDecimal(ClientUtil.ToString(dr.Cells["Money"].Value));
                    theAcctLostAndProfitDtl.Descript = ClientUtil.ToString(dr.Cells["Remark"].Value);

                    if (theAcctLostAndProfitDtl.Id == "")
                    {
                        theAcctLostAndProfit.AddDetail(theAcctLostAndProfitDtl);
                        dr.Tag = theAcctLostAndProfitDtl;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݴ���" + ExceptionUtil.ExceptionMessage(e)); ;
                return false;
            }
        }

        //��ʾ����
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = theAcctLostAndProfit.Code;


                this.txtSupplier.Result.Clear();
                if (theAcctLostAndProfit.TheSupplierRelationInfo != null)
                {
                    this.txtSupplier.Result.Add(theAcctLostAndProfit.TheSupplierRelationInfo);
                    this.txtSupplier.Text = theAcctLostAndProfit.TheSupplierRelationInfo.SupplierInfo.Name;
                }

                /*if (theAcctLostAndProfit.TheCustomerRelationInfo != null)
                {
                    this.txtCustomer.Result.Add(theAcctLostAndProfit.TheCustomerRelationInfo);
                    this.txtCustomer.Text = theAcctLostAndProfit.TheCustomerRelationInfo.CustomerInfo.Name;
                }*/

                this.cboBusType.SelectedIndex = theAcctLostAndProfit.BusinessType;
                this.txtRemark.Text = theAcctLostAndProfit.Descript;
                this.txtCreatePerson.Tag = theAcctLostAndProfit.CreatePerson;
                if (theAcctLostAndProfit.CreatePerson != null)
                    this.txtCreatePerson.Text = theAcctLostAndProfit.CreatePerson.Name;
                this.txtCreateDate.Text = theAcctLostAndProfit.CreateDate.ToShortDateString();

                //��ϸ
                this.dgDetail.Rows.Clear();
                foreach (AcctLoseAndProfitDtl alapDtl in theAcctLostAndProfit.Details)
                {
                    int i = dgDetail.Rows.Add();
                    DataGridViewRow row = dgDetail.Rows[i];

                    Material material = alapDtl.MaterialResource;

                    row.Cells["MaterialCode"].Tag = material;
                    row.Cells["MaterialCode"].Value = material.Code;
                    row.Cells["MaterialName"].Value = material.Name;
                    row.Cells["MaterialSpec"].Value = material.Specification;
                    row.Cells["Stuff"].Value = material.Stuff;

                    //���ø����ϵļ�����λ
                    row.Cells["Unit"].Tag = alapDtl.MatStandardUnit;
                    if (alapDtl.MatStandardUnit != null)
                        row.Cells["Unit"].Value = alapDtl.MatStandardUnit.Name;
                    row.Cells["Quantity"].Value = alapDtl.Quantity;
                    row.Cells["Price"].Value = alapDtl.Price;
                    row.Cells["Money"].Value = alapDtl.Money.ToString("#,###.##");
                    row.Cells["Remark"].Value = alapDtl.Descript;
                    row.Tag = alapDtl;
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override bool Preview()
        {
            //    VBillPrint vprint = new VBillPrint(BillType.btAcctLoseAndProfit, "������ӯ�̿�����ӡ", this.txtCode.Text, true);
            //    vprint.ShowDialog();
            //    theMProfitIn.Preview();
            return true;
        }

        public override bool Print()
        {
            //VBillPrint vprint = new VBillPrint(BillType.btAcctLoseAndProfit, "������ӯ�̿�����ӡ", this.txtCode.Text, false);
            //vprint.ShowDialog();
            //theMProfitIn.Print();
            return true;
        }

        private void cboBusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtSupplier.Visible = true;
            this.lblSupplier.Visible = true;
            this.lblCustomer.Visible = false;
            this.txtCustomer.Text = "";
            this.txtCustomer.Visible = false;
            /*if (!"".Equals(cboBusType.Text) && cboBusType.Text.IndexOf("����") != -1)
            {               
                this.txtSupplier.Visible = true;
                this.lblSupplier.Visible = true;
                this.lblCustomer.Visible = false;
                this.txtCustomer.Text = "";
                this.txtCustomer.Visible = false;
            }
            if (!"".Equals(cboBusType.Text) && cboBusType.Text.IndexOf("����") != -1)
            {
                this.lblCustomer.Visible = true;
                this.txtCustomer.Visible = true;
                this.txtSupplier.Text = "";
                this.txtSupplier.Visible = false;
                this.lblSupplier.Visible = false;               
            }*/
        }
    }
}

