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
using Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng
{
    public partial class VContractAdjustPriceQuery : TBasicDataView
    {
        private MContractAdjustPriceMng model = new MContractAdjustPriceMng();
        private ContractAdjustPrice curBillMaster;
        private MSupplyOrderMng modelsupply = new MSupplyOrderMng();
        private SupplyOrderMaster supplyorder;
        private SupplyOrderDetail supplydetail;
        /// <summary>
        /// 当前单据
        /// </summary>
        public ContractAdjustPrice CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }
        public SupplyOrderMaster Supplyorder
        {
            get { return supplyorder; }
            set { supplyorder = value; }
        }
        public SupplyOrderDetail Supplydetail
        {
            get { return supplydetail; }
            set { supplydetail = value; }
        }

        public VContractAdjustPriceQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            txtSupplier.Enabled = true;
            
        }

        private void InitLogin()
        {
            curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
            curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.CreateDate = ConstObject.LoginDate;
            curBillMaster.CreateYear = ConstObject.LoginDate.Year;
            curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
            curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
            curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
            curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
            curBillMaster.DocState = DocumentState.Edit;
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                curBillMaster.ProjectId = projectInfo.Id;
                curBillMaster.ProjectName = projectInfo.Name;
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnSubmit.Click += new EventHandler(btnSumit_Click);
            this.btnReSet.Click += new EventHandler(btnReset_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.dgDetail.CellClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);//单击
            this.dgExtDetail.CellClick += new DataGridViewCellEventHandler(dgExtDetail_CellDoubleClick);//单击
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.ContractAdjustPriceSrv.SaveContractAdjustPrice(curBillMaster);
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "采购合同调价单";
                log.Code = curBillMaster.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                if (flag)
                {
                    log.OperType = "新增";
                }
                else
                {
                    log.OperType = "修改";
                }
                StaticMethod.InsertLogData(log);
                MessageBox.Show("保存成功！");
                string strContractNo = ClientUtil.ToString(this.txtContractNo.Text);
                string strMaterialCode = ClientUtil.ToString(this.txtMaterialCode.Text);
                ShowdgExtDetail(strContractNo,strMaterialCode);
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(err));
            }
        }

        /// <summary>
        /// 调价提交
        /// </summary>
        /// <returns></returns>
        void btnSumit_Click(object sender, EventArgs e)
        {
            if (tabControl1.RowCount - 1 <= 0)
            {
                if (!ViewToModel()) return;
                curBillMaster = model.ContractAdjustPriceSrv.SaveContractAdjustPrice(curBillMaster);

            }
            string id = dgDetail.CurrentRow.Cells[colCode.Name].Tag as string;
            SupplyOrderDetail Supplydetail = model.ContractAdjustPriceSrv.GetSupplyOrderDetail(id);
            Supplydetail.ModifyPrice = ClientUtil.ToDecimal(txtNewPrice.Text);//修改价格
            Supplydetail.ForwardDetailId = curBillMaster.Id;//调价单
            if (!ViewToModel()) return;
            curBillMaster.DocState = DocumentState.InExecute;//提交时将明细表中的状态改为执行中
            curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
            curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
            curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
            curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
            curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
            curBillMaster = model.ContractAdjustPriceSrv.saveaa(curBillMaster, Supplydetail);//修改调价单的信息
            curBillMaster.DocState = DocumentState.Edit;
            MessageBox.Show("提交成功！");
            string strContractNo = ClientUtil.ToString(this.txtContractNo.Text);
            string strMaterialCode = ClientUtil.ToString(this.txtMaterialCode.Text);
            ShowdgExtDetail(strContractNo, strMaterialCode);
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        void btnReset_Click(object sender, EventArgs e)
        {
            this.txtContractReason.Text = "";
            this.txtMaterialCode.Text = "";
            this.txtMaterialName.Text = "";
            this.txtSpec.Text = "";
            this.txtNewPrice.Text = "";
            this.txtOldPrice.Text = "";
            this.txtSupply.Text = "";
            this.txtContractNo.Text = "";
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                {
                    condition += "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                }
                else
                {
                    condition += " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                }
            }
            //制单日期
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

            //供应商
            if (this.txtSupplier.Text != "" && this.txtSupplier.Result != null && this.txtSupplier.Result.Count != 0)
            {
                //condition += " and t1.SupplierName ='" + (this.txtSupplier.Result[0] as SupplierRelationInfo).Id.ToString() + "'";
                condition += " and t1.SupplierName like '%" +  this.txtSupplier .Text.Trim ()  + "%'";
            }

            //采购合同号
            if (!this.txtSupplyNum.Text.Trim().Equals(""))
            {
                condition += "and t1.Code like '%" + txtSupplyNum.Text + "%'";
            }

            //物资
            if (this.txtMaterial.Text != "")
            {
                condition += " and t2.MaterialName like '%" + this.txtMaterial.Text + "%'";
            }
            //规格型号
            if (this.txtMaterialSpec.Text != "")
            {
                condition += " and t2.MaterialSpec like '%" + this.txtMaterialSpec.Text + "%'";
            }
            #endregion
            DataSet dataSet = model.ContractAdjustPriceSrv.ContractAdjustPriceQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                dgDetail[colCode.Name, rowIndex].Tag = dataRow["detailId"];
                object objState = dataRow["State"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
                dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];//物资编码
                dgDetail[colMaterialCode.Name, rowIndex].Tag = dataRow["Material"];//物资编码
                dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];//物资名称
                dgDetail[colMaterialSpec.Name, rowIndex].Value = dataRow["MaterialSpec"]; //规格型号
                dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];//计量单位
                object quantity = dataRow["Quantity"];//总数量
                if (quantity != null)
                {
                    sumQuantity += decimal.Parse(quantity.ToString());
                }
                dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                dgDetail[colContractprice.Name, rowIndex].Value = dataRow["SupplyPrice"];//合同单价
                dgDetail[colSupply.Name, rowIndex].Value = dataRow["SupplierName"];//供应商
                dgDetail[colSupply.Name, rowIndex].Tag = dataRow["Supplierrelation"];//供应商
                dgDetail[colContractNo.Name, rowIndex].Value = dataRow["Code"];//采购合同号
                dgDetail[colNewPrice.Name, rowIndex].Value = dataRow["ModifyPrice"];//合同调价价格
                dgDetail[colMoney.Name, rowIndex].Value = dataRow["Money"];//金额
                dgDetail[colDiagramNum.Name, rowIndex].Value = dataRow["DiagramNumber"];
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }

        /// <summary>
        /// 采购合同调价单列，支持鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgExtDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            //if (this.dgExtDetail.Columns[e.ColumnIndex].Name.Equals(colCGCode.Name))
            //{
                //this.txtSupplier.Text = ClientUtil.ToString(this.dgExtDetail[colCGSupplier.Name,e.RowIndex].Value);
                this.txtSpec.Text = ClientUtil.ToString(this.dgExtDetail[colCGMaterialSpec.Name, e.RowIndex].Value);
                this.txtMaterialName.Text = ClientUtil.ToString(this.dgExtDetail[colCGMaterialName.Name, e.RowIndex].Value);
                this.txtMaterialCode.Text = ClientUtil.ToString(this.dgExtDetail[colCGMaterialCode.Name, e.RowIndex].Value);
                this.txtMaterialCode.Tag = ClientUtil.ToString(this.dgExtDetail[colCGMaterialCode.Name, e.RowIndex].Tag);
                this.txtNewPrice.Text = ClientUtil.ToString(this.dgExtDetail[colCGNewPrice.Name, e.RowIndex].Value);
                this.txtOldPrice.Text = ClientUtil.ToString(this.dgExtDetail[colCGOldPrice.Name, e.RowIndex].Value);
                this.txtContractReason.Text = ClientUtil.ToString(this.dgExtDetail[colCGContractReason.Name, e.RowIndex].Value);
                this.txtContractNo.Text = ClientUtil.ToString(this.dgExtDetail[colCGContractNo.Name, e.RowIndex].Value);
                this.txtRealOperDate.Text = ClientUtil.ToString(this.dgExtDetail[colCGContractDate.Name, e.RowIndex].Value);
                ////curBillMaster.Id = 
                //curBillMaster.Code = ClientUtil.ToString(this.dgExtDetail[colCGCode.Name, e.RowIndex].Value);

            //}
        }

        /// <summary>
        /// 采购合同列，支持鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNewPrice.Text = "";
            txtContractReason.Text = "";
            this.curBillMaster = new ContractAdjustPrice();
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            //if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colContractNo.Name))
            //{
            this.txtSupply.Text = ClientUtil.ToString(this.dgDetail[colSupply.Name, e.RowIndex].Value);//供应商
            this.txtSupply.Tag = this.dgDetail[colSupply.Name, e.RowIndex].Tag;
            this.txtSpec.Text = ClientUtil.ToString(this.dgDetail[colMaterialSpec.Name, e.RowIndex].Value);
            this.txtMaterialName.Text = ClientUtil.ToString(this.dgDetail[colMaterialName.Name, e.RowIndex].Value);
            this.txtMaterialCode.Text = ClientUtil.ToString(this.dgDetail[colMaterialCode.Name, e.RowIndex].Value);
            this.txtMaterialCode.Tag = ClientUtil.ToString(this.dgDetail[colMaterialCode.Name, e.RowIndex].Tag);
            this.txtContractNo.Text = ClientUtil.ToString(this.dgDetail[colContractNo.Name, e.RowIndex].Value);//采购合同号
            this.txtContractNo.Tag = this.dgDetail[colContractNo.Name, e.RowIndex].Tag;
            this.txtOldPrice.Text = ClientUtil.ToString(this.dgDetail[colContractprice.Name, e.RowIndex].Value);
            //}
            string strContractNo = ClientUtil.ToString(this.dgDetail[colContractNo.Name, e.RowIndex].Value);
            string strMaterialCode = ClientUtil.ToString(this.dgDetail[colMaterialCode.Name, e.RowIndex].Value);
            ShowdgExtDetail(strContractNo ,strMaterialCode);
        }

        #region 固定代码

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            //永久锁定
            object[] os = new object[] { txtCreatePerson, txtContractNo,  txtMaterialCode, txtMaterialName, txtSpec, txtOldPrice ,txtSupply};
            ObjectLock.Lock(os);
            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colSupply.Name, colState.Name, colQuantity.Name, colNewPrice.Name, colMoney.Name, colContractNo.Name, colMaterialCode.Name, colContractprice.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
            string[] lockcol = new string[] { colCGCode.Name, colCGContractDate.Name, colCGContractPerson.Name, colCGContractNo.Name, colCGContractPrice.Name, colCGMaterialCode.Name, colCGMaterialName.Name, colCGMaterialSpec.Name, colCGNewPrice.Name, colCGOldPrice.Name, colCGQuantity.Name, colCGState.Name, colCGSupply.Name, colCGContractReason.Name };
            dgExtDetail.SetColumnsReadOnly(lockcol);
        }
        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        /// 
        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    if (txtNewPrice.Text == "" || txtNewPrice.Text == null || txtContractReason.Text == "" || txtContractReason.Text == null)
                    {
                        MessageBox.Show("采购合同信息不可被删除！");
                    }
                    else
                    {
                        curBillMaster = model.ContractAdjustPriceSrv.GetContractAdjustPriceByCode(curBillMaster.Code);
                        if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                        {
                            if (!model.ContractAdjustPriceSrv.DeleteByDao(curBillMaster)) return;
                            //ClearView();
                            string strContractNo = ClientUtil.ToString(this.txtContractNo.Text);
                            string strMaterialCode = ClientUtil.ToString(this.txtMaterialCode.Text);
                            ShowdgExtDetail(strContractNo, strMaterialCode);
                            LogData log = new LogData();
                            log.BillId = curBillMaster.Id;
                            log.BillType = "采购合同调价单";
                            log.Code = curBillMaster.Code;
                            log.OperType = "删除";
                            log.Descript = "";
                            log.OperPerson = ConstObject.LoginPersonInfo.Name;
                            log.ProjectName = curBillMaster.ProjectName;
                            StaticMethod.InsertLogData(log);
                            MessageBox.Show("删除成功！");
                            return;
                        }
                        MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(err));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (this.txtNewPrice.Text.Equals(""))
            {
                MessageBox.Show("调后价格不能为空！");
                return false;
            }
            if (this.txtContractReason.Text.Equals(""))
            {
                MessageBox.Show("调价原因不能为空！");
                return false;
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                InitLogin();
                curBillMaster.RealOperationDate = txtRealOperDate.Value.Date;//业务日期
                curBillMaster.SupplyOrderCode = ClientUtil.ToString(this.txtContractNo.Text);//采购单
                curBillMaster.AvailabilityDate = txtRealOperDate.Value.Date;//生效日期
                curBillMaster.SupplyOrderCode = ClientUtil.ToString(txtContractNo.Text);//采购合同编号
                curBillMaster.ContractNum = ClientUtil.ToString(txtContractNo.Tag);//采购合同号
                curBillMaster.ModifyPriceReason = ClientUtil.ToString(this.txtContractReason.Text);//原因
                curBillMaster.ModifyPrice = ClientUtil.ToDecimal(this.txtNewPrice.Text);//调后价格                
                curBillMaster.MaterialSpec = ClientUtil.ToString(this.txtSpec.Text);//规格型号
                curBillMaster.MaterialName = ClientUtil.ToString(this.txtMaterialName.Text);//物资名称
                curBillMaster.MaterialCode = ClientUtil.ToString(this.txtMaterialCode.Text);//物资编号
                Material material = new Material();
                material.Id = ClientUtil.ToString(this.txtMaterialCode.Tag);
                material.Version = 1;
                material.Code = curBillMaster.MaterialCode;
                curBillMaster.MaterialResource = material;
                curBillMaster.PrePrice = ClientUtil.ToDecimal(this.txtOldPrice.Text);//调前价格
                curBillMaster.TheSupplierRelationInfo = ClientUtil.ToString(this.txtSupply.Tag);//供应商关系ID
                curBillMaster.SupplierName = ClientUtil.ToString(this.txtSupply.Text);//供应商
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }


        private void ShowdgExtDetail(string strContractNo,string strMaterailCode)
        {
            DataSet dataSet = model.ContractAdjustPriceSrv.SelectContractAdjustPrice(strContractNo, strMaterailCode);
            this.dgExtDetail.Rows.Clear();
            //查询此合同单号下的所有采购调价信息
            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgExtDetail.Rows.Add();
                dgExtDetail[colCGContractDate.Name, rowIndex].Value = dataRow["AvailabilityDate"];//调价日期
                //dgExtDetail[colCGChangePerson.Name,rowIndex].Value = dataRow[""];//调价人
                dgExtDetail[colCGCode.Name, rowIndex].Value = dataRow["Code"];//单号
                dgExtDetail[colCGContractNo.Name, rowIndex].Value = dataRow["SupplyOrderCode"];//采购合同号
                //dgExtDetail[colCGContractPrice.Name, rowIndex].Value = dataRow[""];//合同单价
                dgExtDetail[colCGMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];//物资名称
                dgExtDetail[colCGMaterialSpec.Name, rowIndex].Value = dataRow["MaterialSpec"];//规格型号
                dgExtDetail[colCGMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];//物资编号
                dgExtDetail[colCGMaterialCode.Name, rowIndex].Tag = dataRow["Material"];//物资编号
                dgExtDetail[colCGNewPrice.Name, rowIndex].Value = dataRow["ModifyPrice"];//调后价格，新价格
                dgExtDetail[colCGOldPrice.Name, rowIndex].Value = dataRow["PrePrice"];//调前价格
                //dgExtDetail[colCGQuantity.Name, rowIndex].Value = dataRow[""];//数量
                dgExtDetail[colCGContractReason.Name, rowIndex].Value = dataRow["ModifyPriceReason"];//调价原因
                dgExtDetail[colCGState.Name, rowIndex].Value = dataRow["State"];
                object objstate = dataRow["State"];
                if (objstate != null)
                {
                    dgExtDetail[colCGState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objstate.ToString()));
                }
                //dgExtDetail[colCGSupplier.Name,rowIndex].Value = dataRow[""];//供应商
                if (dgExtDetail[colCGState.Name, rowIndex].Value == "编辑")
                {
                    this.txtNewPrice.Text = ClientUtil.ToString(dgExtDetail[colCGNewPrice.Name, rowIndex].Value);
                    this.txtContractReason.Text = ClientUtil.ToString(dgExtDetail[colCGContractReason.Name, rowIndex].Value);
                }
                if (txtOldPrice.Text != "" && txtContractReason.Text != "")
                {
                    //判断是新增信息还是更新信息，如果存在编辑的信息则为更新，否则为新增
                    curBillMaster.Id = ClientUtil.ToString(dataRow["Id"]);
                    curBillMaster.Code = ClientUtil.ToString(dgExtDetail[colCGCode.Name, rowIndex].Value);
                    curBillMaster.Version = Convert.ToInt64(dataRow["Version"]);
                }
            }
            this.dgExtDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }




        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }
    }
}
