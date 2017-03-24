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
using Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockRelationUI;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut
{
    public partial class VLossOut : TMasterDetailView
    {
        MLossOut theMLossOut = new MLossOut();
        private MStockMng modelStockIn = new MStockMng();
        public LossOut theLossOut = new LossOut();
        public VLossOutList theVLossOutList;
        private EnumStockExecType execType;
        public EnumStockExecType ExecType
        {
            get { return execType; }
            set { execType = value; }
        }

        public VLossOut()
        {
            InitializeComponent();
            Title = "�̿���";
            InitEvent();
            InitData();
        }
        public VLossOut(EnumStockExecType ExecType)
        {
            InitializeComponent();
            this.ExecType = ExecType;
            Title = "�̿���";
            InitEvent();
            InitData();
        }

        public void InitEvent()
        {
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //   this.txtStockOutPurpose.Validated += new EventHandler(txtStockOutPurpose_Validated);
            //�Ҽ�ɾ���˵�
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ȷ��Ҫɾ����ǰѡ�еļ�¼��", "ɾ����¼", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    if (dr.Tag is LossOutDtl)
                    {
                        theLossOut.Details.Remove(dr.Tag as LossOutDtl);
                    }
                }
            }
        }


        public void InitData()
        {
            switch (this.ExecType)
            {
                case EnumStockExecType.��װ:
                    {
                        this.lblSpecailType.Visible = true;
                        this.comSpecailType.Visible = true;
                        //���רҵ����������
                        VBasicDataOptr.InitProfessionCategory(comSpecailType, false);
                        break;
                    }
                default:
                    {
                        this.lblSpecailType.Visible = false;
                        this.comSpecailType.Visible = false;
                        break;
                    }
            }
            dgDetail.ContextMenuStrip = cmsDg;
            
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex < 0) return;
            if (this.dgDetail.EditMode == DataGridViewEditMode.EditOnEnter)
            {
                if (dgDetail.Columns[e.ColumnIndex].Name.Equals("MaterialCode"))
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
                    foreach (Material theMaterial in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail["MaterialCode", i].Tag = theMaterial;
                        this.dgDetail["MaterialCode", i].Value = theMaterial.Code;
                        this.dgDetail["MaterialName", i].Value = theMaterial.Name;
                        this.dgDetail["MaterialSpec", i].Value = theMaterial.Specification;
                        this.dgDetail["Stuff", i].Value = theMaterial.Stuff;
                        this.dgDetail["Quantity", i].Value = 0;
                        this.dgDetail["Unit", i].Tag = theMaterial.BasicUnit;
                        if (theMaterial.BasicUnit != null)
                            this.dgDetail["Unit", i].Value = theMaterial.BasicUnit.Name;

                        this.dgDetail.Rows[i].Tag = theMaterial;
                        i++;
                    }
                }
            }
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validy = true;
            string columnName = this.dgDetail.Columns[e.ColumnIndex].Name;
            if (columnName == "Quantity")
            {
                if (this.dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value != null)
                {
                    string temp_quantity =ClientUtil .ToString ( this.dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value);
                    validy = CommonMethod.VeryValid(temp_quantity);
                    if (validy == false)
                    {
                        MessageBox.Show("���������֣�");
                        return;
                    }
                }
            }
            else if (columnName == colPrice.Name)
            {
                string temp_Price = ClientUtil .ToString (this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value );
                validy = CommonMethod.VeryValid(temp_Price);
                if (validy == false)
                {
                    MessageBox.Show("���������֣�");
                    return;
                }
            }
            else
            {
                return;
            }
            if (columnName == "Quantity" || columnName == colPrice.Name)
            {
                this.dgDetail.Rows[e.RowIndex].Cells[colMoney.Name].Value = ClientUtil.ToDecimal(this.dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value) * ClientUtil.ToDecimal(this.dgDetail.Rows[e.RowIndex].Cells["Quantity"].Value);
            }

        }

        void cbStockMoveType_SelectedIndexChanged(Object sender, EventArgs e)
        {
            ClearView();
        }

        public void Start(string code)
        {
            try
            {
                if (code == "��")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    theLossOut = theMLossOut.GetObject(code, Enum.GetName(typeof(EnumStockExecType), execType),StaticMethod.GetProjectInfo().Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("��ͼ��������" + ExceptionUtil.ExceptionMessage(e)); ;
            }
        }
        public void StartByID(string sID)
        {
            try
            {
                if (sID == "��")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    theLossOut = theMLossOut.GetObjectById(sID);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate,txtProject };
            ObjectLock.Lock(os);
            string[] lockCols = new string[] { MaterialName.Name, MaterialSpec.Name, Unit.Name, Stuff.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
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
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
                    break;
                default:
                    break;
            }
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
                c.Tag = null;
                c.Text = "";
            } else if (c is CommonPerson)
            {
                (c as CommonPerson).Result.Clear();
                (c as CommonPerson).Text = "";
            
            } else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                this.theLossOut = new LossOut(); ;
                theLossOut.CreatePerson = ConstObject.LoginPersonInfo;
                theLossOut.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                theLossOut.CreateDate = ConstObject.LoginDate;
                theLossOut.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                theLossOut.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                theLossOut.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//��¼������
                theLossOut.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                theLossOut.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                theLossOut.TheStationCategory = StaticMethod.GetStationCategory();
                theLossOut.DocState = DocumentState.Edit;

                theLossOut.Special = Enum.GetName(typeof(EnumStockExecType), execType);

                //�Ƶ���
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //�Ƶ�����
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();

                //������Ŀ
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    theLossOut.ProjectId = projectInfo.Id;
                    theLossOut.ProjectName = projectInfo.Name;
                }
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
            if (theLossOut.IsTally == 1)
            {
                MessageBox.Show("�˵��Ѿ����ʣ������޸ģ�");
                return false;
            }
            base.ModifyView();
            theLossOut = theMLossOut.GetObjectById(theLossOut.Id);
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
                if (theLossOut.Id == null)
                {
                    
                    theLossOut = theMLossOut.Save(theLossOut);
                    //������־
                    StaticMethod.InsertLogData(theLossOut.Id, "����", theLossOut.Code, ConstObject.LoginPersonInfo.Name, "�̿���", "", theLossOut.ProjectName);
                }
                else
                {
                    theLossOut = theMLossOut.Save(theLossOut);
                    //������־
                    StaticMethod.InsertLogData(theLossOut.Id, "�޸�", theLossOut.Code, ConstObject.LoginPersonInfo.Name, "�̿���", "", theLossOut.ProjectName);
                }
                txtCode.Text = theLossOut.Code;

                //����Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                isSaveSuccess = true;

                DialogResult aa = MessageBox.Show("����ɹ����Ƿ���ˣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (aa == DialogResult.Yes)
                {
                    Hashtable hashTally = new Hashtable();
                    Hashtable hashCode = new Hashtable();
                    IList lstTally = new ArrayList();
                    IList lstCode = new ArrayList();
                    lstTally.Add(theLossOut.Id);
                    lstCode.Add(theLossOut.Code);

                    hashTally.Add("LossOut", lstTally);
                    hashCode.Add("LossOutCode", lstCode);

                    Hashtable dicList = theMLossOut.Tally(hashTally, hashCode);
                    string errMsg = ClientUtil.ToString(dicList["err"]);
                    IList list = dicList["Succ"] as IList;
                    //����
                    if (errMsg != "")
                        MessageBox.Show(errMsg);
                    else
                    {
                        theLossOut.IsTally = 1;
                        MessageBox.Show("���˳ɹ���");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("���ݱ������" + ExceptionUtil.ExceptionMessage(e)); ;
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
                if (theLossOut.IsTally == 1)
                {
                    MessageBox.Show("�˵��Ѿ����ˣ�����ɾ����");
                    return false;
                }
                theLossOut = theMLossOut.GetObjectById(theLossOut.Id);
                if (!theMLossOut.Delete(theLossOut)) return false;
                //������־
                StaticMethod.InsertLogData(theLossOut.Id, "ɾ��", theLossOut.Code, ConstObject.LoginPersonInfo.Name, "�̿���", "", theLossOut.ProjectName);
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
                        theLossOut = theMLossOut.GetObjectById(theLossOut.Id);
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
                theLossOut = theMLossOut.GetObjectById(theLossOut.Id);
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
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("��ϸ����Ϊ��!");
                return false;
            }

            dgDetail.EndEdit();

            //��ϸ������У��
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //���һ�в�����У��
                if (dr.IsNewRow) break;

                object objQty=dr.Cells["Quantity"].Value;
                if (objQty == null || objQty.ToString() == "")
                {
                    MessageBox.Show("����������Ϊ�գ�");
                    dgDetail.CurrentCell = dr.Cells["Quantity"];
                    return false;
                }
                if (!CommonMethod.VeryValid(objQty.ToString()))
                {
                    MessageBox.Show("��������ȷ��������");
                    dgDetail.CurrentCell = dr.Cells["Quantity"];
                    return false;
                }
                if (decimal.Parse(objQty.ToString()) <= 0)
                {
                    MessageBox.Show("��������С�ڵ���0��");
                    dgDetail.CurrentCell = dr.Cells["Quantity"];
                    return false;
                }
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
                //theLossOut.TheStationCategory = this.txtStationCategory.Result[0] as StationCategory;
                this.txtCode.Focus();
                theLossOut.CreateDate = dtpDateBegin.Value;

                DataTable oTable = modelStockIn.StockInSrv.GetFiscaDate(theLossOut.CreateDate);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    theLossOut.CreateYear = int.Parse(oTable.Rows[0]["year"].ToString());
                    theLossOut.CreateMonth = int.Parse(oTable.Rows[0]["month"].ToString());
                }


                if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
                {
                    theLossOut.HandlePerson = txtHandlePerson.Result[0] as PersonInfo;
                    theLossOut.HandlePersonName = txtHandlePerson.Text;
                } else
                {
                    theLossOut.HandlePerson = null;
                    theLossOut.HandlePersonName = null;
                }
                if (this.comSpecailType.Visible)
                {
                    theLossOut.Special = this.comSpecailType.Text;
                }
               // theLossOut.RealOperationDate = dtpDateBegin.Value.Date;
                theLossOut.LastModifyDate = CommonMethod.GetServerDateTime();
                theLossOut.Descript = this.txtRemark.Text;
                
                foreach (DataGridViewRow dr in this.dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    LossOutDtl theLossOutDtl = new LossOutDtl();
                    theLossOutDtl = dr.Tag as LossOutDtl;

                    if (theLossOutDtl == null)
                        theLossOutDtl = new LossOutDtl();
                    else if (theLossOutDtl.Id == null)
                    {
                        theLossOut.Details.Remove(theLossOutDtl);
                    }

                    theLossOutDtl.MaterialResource = dr.Cells["MaterialCode"].Tag as Material;
                    theLossOutDtl.MaterialCode = ClientUtil.ToString(dr.Cells["MaterialCode"].Value);
                    theLossOutDtl.MaterialName=ClientUtil.ToString(dr.Cells["MaterialName"].Value);
                    theLossOutDtl.MaterialSpec = ClientUtil.ToString(dr.Cells[MaterialSpec.Name].Value);
                    theLossOutDtl.MaterialStuff = ClientUtil.ToString(dr.Cells["Stuff"].Value);
                    theLossOutDtl.MatStandardUnit = dr.Cells["Unit"].Tag as StandardUnit;
                    theLossOutDtl.MatStandardUnitName = ClientUtil.ToString(dr.Cells["Unit"].Value);
                    theLossOutDtl.Quantity = ClientUtil.ToDecimal(dr.Cells["Quantity"].Value);
                    theLossOutDtl.Descript = ClientUtil.ToString(dr.Cells["Remark"].Value);
                    theLossOutDtl.DiagramNumber = ClientUtil.ToString(dr.Cells["DiagramNum"].Value);
                    theLossOutDtl.Price = ClientUtil.ToDecimal(dr.Cells[colPrice.Name].Value);
                    theLossOutDtl.Money = theLossOutDtl.Quantity * theLossOutDtl.Price;
                   
                    theLossOut.AddDetail(theLossOutDtl);
                    dr.Tag = theLossOutDtl;
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
                this.txtCode.Text = theLossOut.Code;
                dtpDateBegin.Value = theLossOut.CreateDate ;
                this.txtProject.Text = theLossOut.ProjectName;
                this.txtRemark.Text = theLossOut.Descript;

                this.txtCreatePerson.Text = theLossOut.CreatePersonName;
                if (theLossOut.HandlePerson != null)
                {
                    txtHandlePerson.Result.Clear();
                    txtHandlePerson.Result.Add(theLossOut.HandlePerson);
                }

                this.txtHandlePerson.Value  = theLossOut.HandlePersonName;
               // this.txtCreateDate.Text = theLossOut.CreateDate.ToShortDateString();
               // this.dtpDateBegin.Value = theLossOut.CreateDate;
                if (this.comSpecailType.Visible)
                {
                    this.comSpecailType.Text = theLossOut.Special;
                }
                //��ϸ
                this.dgDetail.Rows.Clear();
                foreach (LossOutDtl smDtl in theLossOut.Details)
                {
                    int i = dgDetail.Rows.Add();
                    DataGridViewRow row = dgDetail.Rows[i];

                    Material material = smDtl.MaterialResource;
                    row.Cells["MaterialCode"].Tag = material;
                    row.Cells["MaterialCode"].Value = smDtl.MaterialCode;
                    row.Cells["MaterialName"].Value = smDtl.MaterialName;
                    row.Cells["MaterialSpec"].Value = smDtl.MaterialSpec;
                    row.Cells["Stuff"].Value = smDtl.MaterialStuff;

                    //���ø����ϵļ�����λ
                    row.Cells["Unit"].Tag = smDtl.MatStandardUnit;
                    row.Cells["Unit"].Value = smDtl.MatStandardUnitName;
                    row.Cells["Quantity"].Value = smDtl.Quantity;                    
                    row.Cells["Remark"].Value = smDtl.Descript;
                    row.Cells["DiagramNum"].Value = smDtl.DiagramNumber;
                    row.Cells[colPrice.Name].Value = smDtl.Price;
                    row.Cells[colMoney.Name].Value = smDtl.Money;

                    row.Tag = smDtl;
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

        public override bool Preview()
        {
            //VBillPrint vprint = new VBillPrint(BillType.btStockLossOut, "�̿�����ӡ", this.txtCode.Text, true);
            //vprint.ShowDialog();
            //theMLossOut.Preview();
            return true;
        }

        public override bool Print()
        {
            //    VBillPrint vprint = new VBillPrint(BillType.btStockLossOut, "�̿�����ӡ", this.txtCode.Text, false);
            //    vprint.ShowDialog();
            //    theMLossOut.Print();
            return true;
        }
    }
}

