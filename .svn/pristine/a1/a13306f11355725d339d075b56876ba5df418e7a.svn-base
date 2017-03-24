using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ConcreteManage.PumpingPoundsMng.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng
{
    public partial class VPumpingPound : TMasterDetailView
    {
        MConcreteMng model = new MConcreteMng();
        PumpingPoundsMaster pumpPoundsMaster = new PumpingPoundsMaster();
        /// <summary>
        /// 当前单据
        /// </summary>
        public PumpingPoundsMaster PumpPoundsMaster
        {
            get { return pumpPoundsMaster; }
            set { pumpPoundsMaster = value; }
        }
        public VPumpingPound()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
        }

        private void InitEvent()
        {
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            btnSelectWBS.Click += new EventHandler(btnSelectWBS_Click);
        }

        void btnSelectWBS_Click(object sender, EventArgs e)
        {
            if (txtSupply.Text == "" || txtSupply.Result.Count == 0)
            {
                MessageBox.Show("请先选择供应商！");
                return;
            }
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                txtUsedPart.Tag = (list[0] as TreeNode).Tag as GWBSTree;
                txtUsedPart.Text = (list[0] as TreeNode).Text;
            }
            //VSelectUsedPart SelectPart = new VSelectUsedPart();
            //SelectPart.Supplier = txtSupply.Result[0] as SupplierRelationInfo;
            //SelectPart.SupplierName = txtSupply.Text;
            //SelectPart.ShowDialog();
            //IList lst = SelectPart.Result;
            //if (lst.Count > 0)
            //{
            //    PouringNoteMaster master = lst[0] as PouringNoteMaster;
            //    PouringNoteDetail detail = master.Details as PouringNoteDetail;
            //    this.txtUsedPart.Tag = detail.UsedPart;
            //    txtUsedPart.Text = detail.UsedPartName;
            //}
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                pumpPoundsMaster = new PumpingPoundsMaster();
                pumpPoundsMaster.CreatePerson = ConstObject.LoginPersonInfo;
                pumpPoundsMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                pumpPoundsMaster.CreateDate = ConstObject.LoginDate;
                pumpPoundsMaster.CreateYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
                pumpPoundsMaster.CreateMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;
                pumpPoundsMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                pumpPoundsMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                pumpPoundsMaster.HandlePerson = ConstObject.LoginPersonInfo;
                pumpPoundsMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                pumpPoundsMaster.DocState = DocumentState.Edit;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    pumpPoundsMaster.ProjectId = projectInfo.Id;
                    pumpPoundsMaster.ProjectName = projectInfo.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("新建单据错误：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (pumpPoundsMaster.DocState == DocumentState.Edit || pumpPoundsMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                pumpPoundsMaster = model.ConcreteMngSrv.GetPumpingPoundsMasterById(pumpPoundsMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(pumpPoundsMaster.DocState));
            MessageBox.Show(message);
            return false;
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        pumpPoundsMaster = model.ConcreteMngSrv.GetPumpingPoundsMasterById(pumpPoundsMaster.Id);
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
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                pumpPoundsMaster.DocState = DocumentState.Edit;
                pumpPoundsMaster = model.ConcreteMngSrv.SavePumpingPoundsMaster(pumpPoundsMaster);

                ////插入日志
                ////MStockIn.InsertLogData(pumpPoundsMaster.Id, "保存", pumpPoundsMaster.Code, pumpPoundsMaster.CreatePerson.Name, "抽磅单","");
                txtCode.Text = pumpPoundsMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                pumpPoundsMaster = model.ConcreteMngSrv.GetPumpingPoundsMasterById(pumpPoundsMaster.Id);

                if (pumpPoundsMaster.DocState == DocumentState.Valid || pumpPoundsMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ConcreteMngSrv.DeleteByDao(pumpPoundsMaster)) return false;
                    //插入日志
                    StaticMethod.InsertLogData(pumpPoundsMaster.Id, "删除", pumpPoundsMaster.Code, ConstObject.LoginPersonInfo.Name, "抽磅单", "", pumpPoundsMaster.ProjectName);
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(pumpPoundsMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }

        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                pumpPoundsMaster = model.ConcreteMngSrv.GetPumpingPoundsMasterById(pumpPoundsMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <returns></returns>
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = pumpPoundsMaster.Code;
                operDate.Value = pumpPoundsMaster.CreateDate ;
                if (pumpPoundsMaster.HandlePerson != null)
                {
                    txtHandlePerson.Tag = pumpPoundsMaster.HandlePerson;
                    txtHandlePerson.Text = pumpPoundsMaster.HandlePersonName;
                }
                if (pumpPoundsMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = pumpPoundsMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(pumpPoundsMaster.TheSupplierRelationInfo);
                    txtSupply.Value = pumpPoundsMaster.SupplierName;
                }
                if (pumpPoundsMaster.CreatePerson != null)
                {
                    txtCreatePerson.Tag = pumpPoundsMaster.CreatePerson;
                    txtCreatePerson.Text = pumpPoundsMaster.CreatePersonName;
                }
                txtUsedPart.Tag = pumpPoundsMaster.UsedPart;
                txtUsedPart.Text = pumpPoundsMaster.UsedPartName;
                txtRemark.Text = pumpPoundsMaster.Descript;
                txtCreateDate.Text = pumpPoundsMaster.CreateDate.ToShortDateString();
                txtSumQuantity.Text = pumpPoundsMaster.SumWeight.ToString();
                txtSumDiffAmount.Text = pumpPoundsMaster.SumDelta.ToString();
                txtProject.Text = pumpPoundsMaster.ProjectName;
                txtProject.Tag = pumpPoundsMaster.ProjectId;

                //显示收料单明细
                this.dgDetail.Rows.Clear();

                foreach (PumpingPoundsDetail var in pumpPoundsMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料s
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;

                    this.dgDetail[colTareWeight.Name, i].Value = var.TareWeight;
                    this.dgDetail[colGrossWeight.Name, i].Value = var.GrossWeight;
                    this.dgDetail[colNetWeight.Name, i].Value = var.NetWeight;
                    this.dgDetail[colTicketVolume.Name, i].Value = var.TicketVolume;
                    this.dgDetail[colTicketWeight.Name, i].Value = var.TicketWeight;
                    this.dgDetail[colPlateNumber.Name, i].Value = var.PlateNumber;
                    this.dgDetail[colDiffAmount.Name, i].Value = var.DiffAmount;

                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail.Rows[i].Tag = var;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                if (this.txtSupply.Result.Count > 0)
                {
                    pumpPoundsMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    pumpPoundsMaster.SupplierName = this.txtSupply.Text;
                    if (txtUsedPart.Tag != null)
                        pumpPoundsMaster.UsedPart = txtUsedPart.Tag as GWBSTree;
                    pumpPoundsMaster.UsedPartName = txtUsedPart.Text;
                    pumpPoundsMaster.Descript = this.txtRemark.Text;

                    pumpPoundsMaster.CreateDate  = operDate.Value.Date;
                    pumpPoundsMaster.SumWeight = ClientUtil.ToDecimal(this.txtSumQuantity.Text);
                    pumpPoundsMaster.SumDelta = ClientUtil.ToDecimal(this.txtSumDiffAmount.Text);

                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        PumpingPoundsDetail thePumpPoundsDetail = new PumpingPoundsDetail();
                        if (var.Tag != null)
                        {
                            thePumpPoundsDetail = var.Tag as PumpingPoundsDetail;
                            if (thePumpPoundsDetail.Id == null)
                            {
                                pumpPoundsMaster.Details.Remove(thePumpPoundsDetail);
                            }
                        }
                        string order = var.Cells[0].Value.ToString();

                        //材料
                        thePumpPoundsDetail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                        thePumpPoundsDetail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                        thePumpPoundsDetail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                        thePumpPoundsDetail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);

                        thePumpPoundsDetail.TareWeight = ClientUtil.ToDecimal(var.Cells[colTareWeight.Name].Value);//皮重
                        thePumpPoundsDetail.GrossWeight = ClientUtil.ToDecimal(var.Cells[colGrossWeight.Name].Value);//毛重
                        thePumpPoundsDetail.NetWeight = ClientUtil.ToDecimal(var.Cells[colNetWeight.Name].Value);//净重
                        thePumpPoundsDetail.TicketVolume = ClientUtil.ToDecimal(var.Cells[colTicketVolume.Name].Value);//小票方量
                        thePumpPoundsDetail.TicketWeight = ClientUtil.ToDecimal(var.Cells[colTicketWeight.Name].Value);//小票重量
                        thePumpPoundsDetail.DiffAmount = ClientUtil.ToDecimal(var.Cells[colDiffAmount.Name].Value);//量差
                        thePumpPoundsDetail.PlateNumber = ClientUtil.ToString(var.Cells[colPlateNumber.Name].Value);//车牌号
                        thePumpPoundsDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);

                        pumpPoundsMaster.AddDetail(thePumpPoundsDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }

        private bool ValidView()
        {
            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("抽磅单明细不能为空");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            if (txtSupply.Result.Count == 0)
            {
                validMessage += "供应商不能为空！\n";
            }
            if (txtUsedPart.Text == "" || txtUsedPart.Tag == null)
            {
                validMessage += "使用部位不能为空！\n";
            }
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            //抽磅单明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }

                if (dr.Cells[colGrossWeight.Name].Value == null || dr.Cells[colGrossWeight.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colGrossWeight.Name].Value) <= 0)
                {
                    MessageBox.Show("毛重不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colGrossWeight.Name];
                    return false;
                }

                if (dr.Cells[colTareWeight.Name].Value == null || dr.Cells[colTareWeight.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colTareWeight.Name].Value) <= 0)
                {
                    MessageBox.Show("皮重不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colGrossWeight.Name];
                    return false;
                }

                if (dr.Cells[colNetWeight.Name].Value == null || dr.Cells[colNetWeight.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colNetWeight.Name].Value) <= 0)
                {
                    MessageBox.Show("净重不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colGrossWeight.Name];
                    return false;
                }

                if (dr.Cells[colDiffAmount.Name].Value == null || dr.Cells[colDiffAmount.Name].Value.ToString() == "")
                {
                    MessageBox.Show("量差不允许为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colGrossWeight.Name];
                    return false;
                }

                if (dr.Cells[colPlateNumber.Name].Value == null || dr.Cells[colPlateNumber.Name].Value.ToString() == "")
                {
                    MessageBox.Show("请输入车牌号");
                    dgDetail.CurrentCell = dr.Cells[colGrossWeight.Name];
                    return false;
                }

            }
            dgDetail.Update();
            return true;
        }


        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string id)
        {
            try
            {
                if (id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    pumpPoundsMaster = model.ConcreteMngSrv.GetPumpingPoundsMasterById(id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
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
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
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
        }
        #endregion

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtProject };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            txtSupply.Result.Clear();
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
            else if (c is CommonSupplier)
            {
                c.Tag = null;
                c.Text = "";
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
                {
                    DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.materialCatCode = "I112";
                    DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                    object tempValue = cell.EditedFormattedValue;
                    if (tempValue != null && !tempValue.Equals(""))
                    {
                        materialSelector.OpenSelect();
                    }
                    else
                    {
                        materialSelector.OpenSelect();
                    }

                    IList list = materialSelector.Result;

                    foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                    {
                        int i = dgDetail.Rows.Add();
                        this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                        this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                        this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                        this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                        i++;
                    }
                }
            }
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            //if (colName == colNetWeight.Name)
            //{
            //    if (dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value != null)
            //    {
            //        string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value.ToString();
            //        validity = CommonMethod.VeryValid(temp_quantity);
            //        if (validity == false)
            //            MessageBox.Show("请输入数字！");
            //    }

            //    //计算净重
            //    object quantity = dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value;
            //    decimal sumqty = 0;

            //    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            //    {
            //        quantity = dgDetail.Rows[i].Cells[colNetWeight.Name].Value;
            //        if (quantity == null) quantity = 0;
            //        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
            //    }

            //    txtSumQuantity.Text = sumqty.ToString();
            //}

            //if (colName == colDiffAmount.Name)
            //{
            //    if (dgDetail.Rows[e.RowIndex].Cells[colDiffAmount.Name].Value != null)
            //    {
            //        string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colDiffAmount.Name].Value.ToString();
            //        validity = CommonMethod.VeryValid(temp_quantity);
            //        if (validity == false)
            //            MessageBox.Show("请输入数字！");
            //    }

            //    //计算量差
            //    object quantity = dgDetail.Rows[e.RowIndex].Cells[colDiffAmount.Name].Value;
            //    decimal sumqty = 0;

            //    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
            //    {
            //        quantity = dgDetail.Rows[i].Cells[colDiffAmount.Name].Value;
            //        if (quantity == null) quantity = 0;
            //        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
            //    }

            //    txtSumDiffAmount.Text = sumqty.ToString();
            //}
            if (colName == colTicketVolume.Name || colName == colTareWeight.Name || colName == colGrossWeight.Name || colName == colTicketWeight.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colTicketVolume.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colTicketVolume.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colTicketWeight.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colTicketWeight.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                if (dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
            }

            object grossWeight = dgDetail.Rows[e.RowIndex].Cells[colGrossWeight.Name].Value;//毛重
            object tareWeight = dgDetail.Rows[e.RowIndex].Cells[colTareWeight.Name].Value;//皮重
            if (grossWeight != null && tareWeight != null)
            {
                dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value = ClientUtil.ToDecimal(grossWeight) - ClientUtil.ToDecimal(tareWeight);
                //计算净重
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value;
                decimal sumqty = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colNetWeight.Name].Value;
                    if (quantity == null) quantity = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                }

                txtSumQuantity.Text = sumqty.ToString();
            }
            object netWeight = dgDetail.Rows[e.RowIndex].Cells[colNetWeight.Name].Value;//净重
            object ticketWeight = dgDetail.Rows[e.RowIndex].Cells[colTicketWeight.Name].Value;//小票重量

            if (netWeight != null && ticketWeight != null)
            {
                dgDetail.Rows[e.RowIndex].Cells[colDiffAmount.Name].Value = ClientUtil.ToDecimal(netWeight) - ClientUtil.ToDecimal(ticketWeight);

                //计算量差
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colDiffAmount.Name].Value;
                decimal sumqty = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colDiffAmount.Name].Value;
                    if (quantity == null) quantity = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                }

                txtSumDiffAmount.Text = sumqty.ToString();
            }
        }

        #region 打印处理
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"砼抽磅单.flx") == false) return false;
            FillFlex(pumpPoundsMaster);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
            return true;
        }

        public override bool Print()
        {
            if (LoadTempleteFile(@"砼抽磅单.flx") == false) return false;
            FillFlex(pumpPoundsMaster);
            flexGrid1.Print();
            pumpPoundsMaster.PrintTimes = pumpPoundsMaster.PrintTimes + 1;
            pumpPoundsMaster = model.ConcreteMngSrv.SavePumpingPoundsMaster(pumpPoundsMaster);
            return true;
        }

        public override bool Export()
        {
            if (LoadTempleteFile(@"砼抽磅单.flx") == false) return false;
            FillFlex(pumpPoundsMaster);
            flexGrid1.ExportToExcel("抽磅单【" + pumpPoundsMaster.Code + "】", false, false, true);
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(PumpingPoundsMaster billMaster)
        {
            int detailStartRowNumber = 5;//5为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            //flexGrid1.Rows =20;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //主表数据

            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 7).Text = billMaster.RealOperationDate.ToShortDateString();
            flexGrid1.Cell(3, 9).Text = billMaster.SupplierName;

            decimal sumQuantity = 0;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                PumpingPoundsDetail detail = (PumpingPoundsDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;

                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;

                //皮重
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = ClientUtil.ToString(detail.TareWeight);

                //毛重
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.GrossWeight);

                //净重
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.NetWeight);

                //小票方量
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.TicketVolume);

                //小票重量
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.TicketWeight);

                //量差
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.DiffAmount);

                //小票单据号
                //flexGrid1.Cell(detailStartRowNumber + i,9).Text = detail.

                //车号
                flexGrid1.Cell(detailStartRowNumber + i, 10).Text = ClientUtil.ToString(detail.PlateNumber);
                flexGrid1.Cell(detailStartRowNumber + i, 10).WrapText = true;

                //备注
                flexGrid1.Cell(detailStartRowNumber + i, 11).Text = ClientUtil.ToString(detail.Descript);
                flexGrid1.Cell(detailStartRowNumber + i, 11).WrapText = true;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();

            }
            flexGrid1.Cell(5 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(5 + detailCount, 7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(5 + detailCount, 10).Text = billMaster.CreatePersonName;

        }
        #endregion


    }
}
