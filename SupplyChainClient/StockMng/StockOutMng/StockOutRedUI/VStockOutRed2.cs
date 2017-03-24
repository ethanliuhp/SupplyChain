using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Resource.FinancialResource.RelateClass;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI;
using Application.Business.Erp.SupplyChain.Client.StockMng;


namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI
{
    public partial class VStockOutRed2 : TMasterDetailView
    {
        MStockMng model = new MStockMng();
        private StockOutRed curBillMaster;

        /// <summary>
        /// ��ǰ����
        /// </summary>
        public StockOutRed CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        IList moveDtlList = new ArrayList();//�洢ɾ������ϸ��������޸�ʱ���

        public VStockOutRed2()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        public void InitData()
        {

        }
        private void InitEvent()
        {
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            this.btnForward.Click += new EventHandler(btnForward_Click);
        }


        void btnForward_Click(object sender, EventArgs e)
        {
            //IList list = UCL.Locate("���ⵥ", EnumStockExecType.forwardSearch) as IList;
            //if (list == null || list.Count == 0) return;

            //StockOut StockOutTmp = list[0] as StockOut;

            //this.txtForward.Tag = StockOutTmp.Id;
            //this.txtForward.Text = StockOutTmp.Code;

            //this.txtStationCategory.Tag = StockOutTmp.TheStationCategory;

            //if (StockOutTmp.TheStationCategory != null)
            //    this.txtStationCategory.Text = StockOutTmp.TheStationCategory.Name;



    
            ////�������ϸ
            //foreach (DataGridViewRow dr in dgDetail.Rows)
            //{
            //    StockOutRedDtl dtl = dr.Tag as StockOutRedDtl;
            //    if (dtl != null)
            //    {
            //        if (CurBillMaster != null)
            //        {
            //            CurBillMaster.Details.Remove(dtl);
            //            if (dtl.Id != null)
            //            {
            //                moveDtlList.Add(dtl);
            //            }
            //        }
            //    }
            //}
            ////��ʾ���õ���ϸ
            //this.dgDetail.Rows.Clear();
            //foreach (StockOutDtl var in StockOutTmp.Details)
            //{
            //    if (var.IsSelect == false) continue;
            //    int i = this.dgDetail.Rows.Add();

            //    //������ؽ������ж��Ƿ���Գ��
            //    //decimal refTol = theMStockOut.refQuantityByBalance(var.Id);

            //    this.dgDetail["MaterialCode", i].Tag = var.MaterialResource;
            //    this.dgDetail["MaterialCode", i].Value = var.MaterialResource.Code;
            //    this.dgDetail["MaterialName", i].Value = var.MaterialResource.Name;
            //    this.dgDetail["MaterialSpec", i].Value = var.MaterialResource.Specification;

            //    this.dgDetail["Unit", i].Tag = var.MatStandardUnit;
            //    if (var.MatStandardUnit != null)
            //        this.dgDetail["Unit", i].Value = var.MatStandardUnit.Name;

            //    this.dgDetail["Quantity", i].Value = var.Quantity - var.RefQuantity;
            //    this.dgDetail["NoRefQuantity", i].Value = var.Quantity;
            //    this.dgDetail["Price", i].Value = var.Price;
            //    this.dgDetail["Money", i].Value = (var.Quantity - var.RefQuantity) * var.Price;
            //    this.dgDetail["Remark", i].Value = var.Descript;
            //    this.dgDetail["StockOutDtl", i].Tag = var;
            //    this.dgDetail["DtlForward", i].Value = var.Id;

    
            //}
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            StockOutRedDtl dtl = e.Row.Tag as StockOutRedDtl;
            if (dtl != null)
            {
                if (CurBillMaster != null)
                {
                    CurBillMaster.Details.Remove(dtl);
                    if (dtl.Id != null)
                    {
                        moveDtlList.Add(dtl);
                    }
                }
            }
        }

        #region �̶�����


        /// <summary>
        /// ����������,(����״̬�����¼������е�����)
        /// </summary>
        /// <param name="code">����Caption</param>
        public void Start(string code)
        {
            try
            {
                if (code == "��")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    //CurBillMaster = model.GetObject(code);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //�ж��Ƿ�Ϊ�Ƶ���
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("��ͼ��������" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// ˢ��״̬(��ť״̬)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //���Ʊ��
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    break;
                default:
                    break;
            }

            //����������ť
            //...

        }
        #endregion

        /// <summary>
        /// ˢ�¿ؼ�(�����еĿؼ�)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //��������ؼ�
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
                this.btnForward.Enabled = true;
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                this.btnForward.Enabled = false;
            }

            //��������
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtStationCategory, txtCustomer, btnForward, txtForward, txtStockOutPurpose };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { "MaterialName", "MaterialSpec", "Unit", "Money" };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //�������
        private void ClearView()
        {
            ClearControl(pnlContent);
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
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        /// <summary>
        /// �½�
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                CurBillMaster = new StockOutRed();
                ClearView();

                CurBillMaster.Code = DateTime.Now.TimeOfDay.ToString();
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

        /// <summary>
        /// �޸�
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (CurBillMaster.IsTally == 1)
            {
                MessageBox.Show("�˵��Ѿ����ʣ������޸ģ�");
                return false;
            }
            moveDtlList = new ArrayList();
            base.ModifyView();
            //model.Modify();
            //CurBillMaster = model.GetObjectById(CurBillMaster.Id);

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
                //CurBillMaster = model.Save(CurBillMaster, moveDtlList);
                moveDtlList = new ArrayList();//�������
                txtCode.Text = CurBillMaster.Code;

                //����Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                isSaveSuccess = true;
                //if (LoginInfomation.LoginInfo.TheOperationOrgInfo.CkRight == "1")
                //{
                //    return true;
                //}
                //DialogResult aa = MessageBox.Show("����ɹ����Ƿ���ˣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                //if (aa == DialogResult.Yes)
                //{

                //    IDictionary hashTally = new Hashtable();
                //    IDictionary hashCode = new Hashtable();
                //    IList lstTally = new ArrayList();
                //    IList lstCode = new ArrayList();
                //    lstTally.Add(CurBillMaster.Id);
                //    lstCode.Add(CurBillMaster.Code);

                //    hashTally.Add("StockOutRed", lstTally);
                //    hashCode.Add("StockOutRedCode", lstCode);

                //    IDictionary dicList = model.Tally(hashTally, hashCode);
                //    string errMsg = Convert.ToString(dicList["err"]);
                //    IList list = dicList["Succ"] as IList;
                //    //����
                //    if (errMsg != "")
                //        MessageBox.Show(errMsg);
                //    else
                //    {
                //        CurBillMaster.IsTally = 1;
                //        MessageBox.Show("���ʳɹ���");
                //    }
                //}
                //if (CurBillMaster.IsTally==1)
                //    this.lblTally.Text = "�Ѽ���";
                //else
                //    this.lblTally.Text = "δ����";
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(e));
                return isSaveSuccess;
            }

        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                if (CurBillMaster.IsTally == 1)
                {
                    MessageBox.Show("�˵��Ѿ����ʣ�����ɾ����");
                    return false;
                }
                //model.DeleteStockOutRed(CurBillMaster);

                ClearView();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("����ɾ������" + ExceptionUtil.ExceptionMessage(e));
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
                        //CurBillMaster = model.GetObjectById(CurBillMaster.Id);
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
                MessageBox.Show("���ݳ�������" + ExceptionUtil.ExceptionMessage(e));
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
                //CurBillMaster = model.GetObjectById(CurBillMaster.Id);
                //�����渳ֵ
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("����ˢ�´���" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// ��������ǰУ������
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (CurBillMaster.IsTally == 1)
            {
                MessageBox.Show("�˵��Ѿ�����,�����޸�!");
                return false;
            }

            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("��ϸ����Ϊ��!");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));


            //if (this.cboForwardType.Text == "")
            //{
            //    MessageBox.Show("ǰ���ϵ����Ͳ���Ϊ�գ�");
            //    return false;
            //}
            if (this.txtForward.Text == "")
            {
                MessageBox.Show("ǰ���ϵ�����Ϊ�գ�");
                return false;
            }

            if (this.txtStationCategory.Tag == null)
            {
                MessageBox.Show("�ֿⲻ��Ϊ�գ�");
                return false;
            }

            if (this.txtStockOutPurpose.Tag == null)
            {
                MessageBox.Show("������;����Ϊ�գ�");
                return false;
            }
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

                //if (dr.Cells["Unit"].Tag == null)
                //{
                //    MessageBox.Show("������λ������Ϊ�գ�");
                //    dgDetail.CurrentCell = dr.Cells["Unit"];
                //    return false;
                //}

                if (dr.Cells["Price"].Value == null || dr.Cells["Price"].Value.ToString() == "")
                {
                    MessageBox.Show("���۲�����Ϊ�գ�");
                    dgDetail.CurrentCell = dr.Cells["Price"];
                    return false;
                }

                if (dr.Cells["Quantity"].Value == null || dr.Cells["Quantity"].Value.ToString() == "")
                {
                    MessageBox.Show("����������Ϊ�գ�");
                    dgDetail.CurrentCell = dr.Cells["Quantity"];
                    return false;
                }

                if (dr.Cells["Money"].Value == null || dr.Cells["Money"].Value.ToString() == "")
                {
                    MessageBox.Show("������Ϊ�գ�");
                    dgDetail.CurrentCell = dr.Cells["Money"];
                    return false;
                }
                object forwardStockOutDtlId = dr.Cells["DtlForward"].Value;
                StockOutDtl stockOutDtl=null;// = model.GetStockOutDtl(forwardStockOutDtlId.ToString());
                if (stockOutDtl == null)
                {
                    MessageBox.Show("δ�ҵ�ǰ�����ⵥ��ϸ�����������á�");
                    dgDetail.CurrentCell = dr.Cells["Quantity"];
                    return false;
                }
                else
                {
                    decimal canUseQty = stockOutDtl.Quantity - stockOutDtl.RefQuantity;
                    decimal currentQty = decimal.Parse(dr.Cells["Quantity"].Value.ToString());
                    object qtyTempObj = dr.Cells["ColQuantityTemp"].Value;
                    decimal qtyTemp = 0;
                    if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                    {
                        qtyTemp = decimal.Parse(qtyTempObj.ToString());
                    }

                    if (currentQty - qtyTemp - canUseQty > 0)
                    {
                        MessageBox.Show("��������[" + currentQty + "]���ڿ���������[" + canUseQty + "]��");
                        dgDetail.CurrentCell = dr.Cells["Quantity"];
                        return false;
                    }
                }

                if (dr.Cells["Remark"].Value == null)
                    dr.Cells["Remark"].Value = "";
            }
            dgDetail.Update();
            return true;

        }
        //��������
        private bool ViewToModel()
        {
            if (!ValidView()) return false;

            try
            {
                CurBillMaster.TheStationCategory = null;
                CurBillMaster.TheStationCategory = this.txtStationCategory.Tag as StationCategory;

         

                CurBillMaster.CreatePerson = this.txtCreatePerson.Tag as PersonInfo;
                CurBillMaster.CreateDate = StringUtil.StrToDateTime(this.txtCreateDate.Text);
                CurBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                CurBillMaster.CreateYear = ConstObject.LoginDate.Year;
                CurBillMaster.RealOperationDate = CommonMethod.GetServerDateTime();
                CurBillMaster.LastModifyDate = CommonMethod.GetServerDateTime();
                CurBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                CurBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                CurBillMaster.Descript = this.txtRemark.Text;
                //CurBillMaster.ContractNo = this.txtContractNo.Text;
                //CurBillMaster.ForwardCode = this.txtForward.Text;
                //CurBillMaster.ForwardStockOutId = ClientUtil.ToString(this.txtForward.Tag);

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    StockOutRedDtl CurBillMasterDtl = new StockOutRedDtl();
                    CurBillMasterDtl = var.Tag as StockOutRedDtl;
                    if (CurBillMasterDtl == null)
                        CurBillMasterDtl = new StockOutRedDtl();
                    else
                        if (CurBillMasterDtl.Id == null)
                        {
                            CurBillMaster.Details.Remove(CurBillMasterDtl);
                        }
                    CurBillMasterDtl.MaterialResource = var.Cells["MaterialCode"].Tag as Material;
                    CurBillMasterDtl.MatStandardUnit = var.Cells["Unit"].Tag as StandardUnit;
                    decimal quantity = StringUtil.StrToDecimal(Convert.ToString(var.Cells["Quantity"].Value));
                    decimal quantityTemp = StringUtil.StrToDecimal(Convert.ToString(var.Cells["ColQuantityTemp"].Value));

                    CurBillMasterDtl.Quantity = -Math.Abs(quantity);
                    CurBillMasterDtl.QuantityTemp = quantityTemp;
                    CurBillMasterDtl.Price = StringUtil.StrToDecimal(Convert.ToString(var.Cells["Price"].Value));
                    CurBillMasterDtl.Money = -Math.Abs(StringUtil.StrToDecimal(Convert.ToString(var.Cells["Money"].Value)));
                    CurBillMasterDtl.Descript = Convert.ToString(var.Cells["Remark"].Value);
                    //CurBillMasterDtl.TheManageState = var.Cells["theManageState"].Tag as ManageState;
                    string forwardDtlId = ClientUtil.ToString(var.Cells["DtlForward"].Value);
                    //CurBillMasterDtl.ForwardStockOutDtlId = forwardDtlId;
                    CurBillMaster.AddDetail(CurBillMasterDtl);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݴ���" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        //��ʾ����
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = CurBillMaster.Code;
                //this.txtForward.Text = CurBillMaster.ForwardCode;


    


                this.txtStationCategory.Tag = CurBillMaster.TheStationCategory;
                if (CurBillMaster.TheStationCategory != null)
                {
                    this.txtStationCategory.Text = CurBillMaster.TheStationCategory.Name;
                }
                //
  

                this.txtRemark.Text = CurBillMaster.Descript;
                this.txtCreatePerson.Tag = CurBillMaster.CreatePerson;

                if (CurBillMaster.CreatePerson != null)
                    this.txtCreatePerson.Text = CurBillMaster.CreatePerson.Name;
                this.txtCreateDate.Text = CurBillMaster.CreateDate.ToShortDateString();

                if (CurBillMaster.IsTally==1)
                    this.lblTally.Text = "�Ѽ���";
                else
                    this.lblTally.Text = "δ����";
                this.dgDetail.Rows.Clear();
                foreach (StockOutRedDtl var in CurBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail["MaterialCode", i].Tag = var.MaterialResource;
                    this.dgDetail["MaterialCode", i].Value = var.MaterialResource.Code;
                    this.dgDetail["MaterialName", i].Value = var.MaterialResource.Name;
                    this.dgDetail["MaterialSpec", i].Value = var.MaterialResource.Specification;


                    //���ø����ϵļ�����λ

                    this.dgDetail["Unit", i].Tag = var.MatStandardUnit;
                    if (var.MatStandardUnit != null)
                        this.dgDetail["Unit", i].Value = var.MatStandardUnit.Name;

                    this.dgDetail["Quantity", i].Value = Math.Abs(var.Quantity);
                    this.dgDetail["ColQuantityTemp", i].Value = -Math.Abs(var.Quantity);
                    this.dgDetail["Price", i].Value = var.Price;
                    this.dgDetail["Money", i].Value = Math.Abs(var.Money);
                    this.dgDetail["Remark", i].Value = var.Descript;
                    //this.dgDetail["DtlForward", i].Value = var.ForwardStockOutDtlId;
           

                    this.dgDetail.Rows[i].Tag = var;
                }

                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("����ӳ�����" + ExceptionUtil.ExceptionMessage(e));
                //return false;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            bool validity = true;
            if (colName == "Quantity")
            {
                object forwardStockOutDtlId = dgDetail.Rows[e.RowIndex].Cells["DtlForward"].Value;
                StockOutDtl stockOutDtl=null;// = model.GetStockOutDtl(forwardStockOutDtlId.ToString());

                decimal canUseQty = stockOutDtl.Quantity - stockOutDtl.RefQuantity;
                decimal currQuantity = decimal.Parse(dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value.ToString());
                object qtyTempObj = dgDetail.Rows[e.RowIndex].Cells["ColQuantityTemp"].Value;
                decimal tempQuantity = 0;
                if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                {
                    tempQuantity = decimal.Parse(qtyTempObj.ToString());
                }

                if (currQuantity - tempQuantity - canUseQty > 0)
                {
                    MessageBox.Show("��������[" + currQuantity + "]���ڿ���������[" + canUseQty + "],���޸ģ�");
                    return;
                }
            }
            if (colName == "Price" || colName == "Quantity")
            {
                if (dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("���������֣�");
                }

                if (dgDetail.Rows[e.RowIndex].Cells["Price"].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                        MessageBox.Show("���������֣�");
                }

                //���ݵ��ۺ�����������  
                object price = dgDetail.Rows[e.RowIndex].Cells["Price"].Value;
                object quantiy = dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value;
                if (price != null && quantiy != null)
                {
                    decimal money = 0;
                    money = StringUtil.StrToDecimal(price.ToString()) * StringUtil.StrToDecimal(quantiy.ToString());
                    dgDetail.Rows[e.RowIndex].Cells["Money"].Value = money;
                }
            }
            if (colName == "ComplexUnit")
            {
                DataGridViewCell cell = dgDetail.Rows[e.RowIndex].Cells["ComplexUnit"];
                ComplexUnit bcu = cell.Tag as ComplexUnit;
                if (bcu != null && bcu.Unit.Name != cell.Value.ToString())
                {
                    Material bm = dgDetail.Rows[e.RowIndex].Cells["MaterialCode"].Tag as Material;
                    if (bm != null)
                    {
                        bcu = bm.GetComplexUnit(cell.Value.ToString());
                        cell.Tag = bcu;
                    }
                }
            }
        }

        public override bool Preview()
        {
            //VBillPrint vprint = new VBillPrint(BillType.btStockOutRed, "����쵥��ӡ", this.txtCode.Text, true);
            //vprint.ShowDialog();
            //theMStockOutRed.Preview();
            return true;
        }
        public override bool Print()
        {
            //VBillPrint vprint = new VBillPrint(BillType.btStockOutRed, "����쵥��ӡ", this.txtCode.Text, false);
            //vprint.ShowDialog();
            //theMStockOutRed.Print();
            return true;
        }
    }
}