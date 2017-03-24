using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireCollectionGGCheck : TMasterDetailView
    {
         
        private MMaterialHireMng model = new MMaterialHireMng();
        private MatHireCollectionMaster matColMaster;
       // private MatHireOrderMaster matRentalOrderMaster;
        private IList list_delMatCollDtl = new ArrayList();
        CurrentProjectInfo projectInfo = null;//StaticMethod.GetProjectInfo();
        IList list_index_detail = new ArrayList();
        IList list_costType_detail = new ArrayList();
        IList list_index_costDetail = new ArrayList();
        IList list_costType_costDetail = new ArrayList();
        MatHireOrderMaster orderMaster = null;
        Material curMaterial = null;
        /// <summary>
        /// 料具收料单
        /// </summary>
        public MatHireCollectionMaster MatColMaster
        {
            get { return matColMaster; }
            set { matColMaster = value; }
        }
        EnumMatHireType enumMatHireType;
        public void IntialGGLenght()
        {
            DataGridViewComboBoxColumn oColumn = dgDetail.Columns[colMaterialLength.Name] as DataGridViewComboBoxColumn;
            oColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            // List<float> lstLen = new List<float>();
            float baseLen = 1.0F;
           for (int i = 0; i < 60; i++)
           {
               oColumn.Items.Insert(oColumn.Items.Count, (baseLen + i * 0.1).ToString("N2"));
               //lstLen.Insert(lstLen.Count, baseLen + i * 0.1);
           }
          // return lstLen;
        }
         
        public VMaterialHireCollectionGGCheck(EnumMatHireType enumMatHireType)
        {
            InitializeComponent();

            this.enumMatHireType = enumMatHireType;
            InitData();
            InitEvent();
        }

        public void InitData()
        {
            //((DataGridViewComboBoxColumn)colBalRule).Items.AddRange(Enum.GetNames(typeof(EnumMaterialMngBalRule)));
            //GetMaterial();
            IntialGGLenght();
        
            txtBalRule.DataSource = (Enum.GetNames(typeof(EnumMaterialHireMngBalRule)));
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
            txtRank.SupplierCatCode = CommonUtil.SupplierCatCode3;
            dgDetail.ContextMenuStrip = cmsDg;
            flexGrid1.Visible = false;
            this.txtSupply.Enabled = true;
        }
        public void GetMaterial()
        {
            ObjectQuery oQuery=new ObjectQuery();
            oQuery.AddFetchMode("BasicUnit", NHibernate.FetchMode.Eager);
            oQuery.AddCriterion(Expression.Eq("Code",enumMatHireType==EnumMatHireType.钢管?CommonUtil.MaterialGGCode:CommonUtil.MaterialWKCode));
            this.curMaterial = MMaterialHireMng.MaterialService.GetMaterial(typeof(Material), oQuery)[0] as Material;
        }
        //private void GetProperties()
        //{
        //    IList list = new ArrayList();
        //    Type t = typeof(MaterialCollectionMaster);
        //    foreach (PropertyInfo propertyInfo in t.GetProperties())
        //    {
        //        list.Add(propertyInfo.Name);
        //    }
        //}

        public void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            this.dgDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_RowHeaderMouseClick);
            this.dgDelete.Click += new EventHandler(dgDelete_Click);
            this.txtTransCharge.tbTextChanged += new EventHandler(txtTransCharge_tbTextChanged);
            //this.btnForward.Click +=new EventHandler(btnForward_Click);
            this.btnSetDW.Click += new EventHandler(btnSetDW_Click);
            this.btnSetPart.Click += new EventHandler(btnSetPart_Click);
            this.btnSetZL.Click += new EventHandler(btnSetZL_Click);
            this.dgDetail.RowsAdded+=new DataGridViewRowsAddedEventHandler(dgDetail_RowsAdded);
            this.txtSupply.ValueChanged+=new EventHandler(txtSupply_ValueChanged);
            this.TenantSelector.TenantSelectorAfterEvent += (a) =>
            {
                if (a.SelectedProject != null)
                {
                    projectInfo = a.SelectedProject;
                    if (matColMaster != null)
                    {
                        matColMaster.ProjectId = projectInfo.Id;
                        matColMaster.ProjectName = projectInfo.Name;
                        GetMaterialHireOrder();
                    }
                }
            };
            //右键复制菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }
        void txtSupply_ValueChanged(object sender, EventArgs e)
        {
            GetMaterialHireOrder();
        }
        void btnSetDW_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                //if (var.IsNewRow) break;
                if (var.Cells[colMaterialCode.Name].Tag!=null &&  ClientUtil.ToString(dgDetail[this.colBorrowUnit.Name, 0].Value) != "")
                {
                    var.Cells[colBorrowUnit.Name].Value = dgDetail[colBorrowUnit.Name, 0].Value;
                    var.Cells[colBorrowUnit.Name].Tag = dgDetail[colBorrowUnit.Name, 0].Tag;
                }
            }
        }

        void btnSetPart_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
               // if (var.IsNewRow) break;
                if (var.Cells[colMaterialCode.Name].Tag != null && ClientUtil.ToString(dgDetail[colUsedPart.Name, 0].Value) != "")
                {
                    var.Cells[colUsedPart.Name].Value = dgDetail[colUsedPart.Name, 0].Value;
                    var.Cells[colUsedPart.Name].Tag = dgDetail[colUsedPart.Name, 0].Tag;
                }
            }
        }
        void btnSetZL_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
               // if (var.IsNewRow) break;
                if (var.Cells[colMaterialCode.Name].Tag != null && ClientUtil.ToString(dgDetail[this.colSubject.Name, 0].Value) != "")
                {
                    var.Cells[colSubject.Name].Value = dgDetail[colSubject.Name, 0].Value;
                    var.Cells[colSubject.Name].Tag = dgDetail[colSubject.Name, 0].Tag;
                }
            }
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要复制当前选中的记录吗？", "复制记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                int i = dgDetail.Rows.Add();
                dgDetail[colMaterialSpec.Name, i].Value = dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value;
                dgDetail[colMaterialName.Name, i].Value = dgDetail.CurrentRow.Cells[colMaterialName.Name].Value;
                dgDetail[colMaterialCode.Name, i].Tag = dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag;
                dgDetail[colMaterialCode.Name, i].Value = dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value;
                dgDetail[colDescript.Name, i].Value = dgDetail.CurrentRow.Cells[colDescript.Name].Value;
                dgDetail[colBorrowUnit.Name, i].Value = dgDetail.CurrentRow.Cells[colBorrowUnit.Name].Value;
                dgDetail[colBorrowUnit.Name, i].Tag = dgDetail.CurrentRow.Cells[colBorrowUnit.Name].Tag;
                dgDetail[colSubject.Name, i].Tag = dgDetail.CurrentRow.Cells[colSubject.Name].Tag;
                dgDetail[colSubject.Name, i].Value = dgDetail.CurrentRow.Cells[colSubject.Name].Value;
                dgDetail[colUsedPart.Name, i].Value = dgDetail.CurrentRow.Cells[colUsedPart.Name].Value;
                dgDetail[colUsedPart.Name, i].Tag = dgDetail.CurrentRow.Cells[colUsedPart.Name].Tag;
                dgDetail[colPrice.Name, i].Value = dgDetail.CurrentRow.Cells[colPrice.Name].Value;
                dgDetail[colTempQty.Name, i].Value = dgDetail.CurrentRow.Cells[colTempQty.Name].Value;
                dgDetail[colUnit.Name, i].Value = dgDetail.CurrentRow.Cells[colUnit.Name].Value;
                dgDetail[colUnit.Name, i].Tag = dgDetail.CurrentRow.Cells[colUnit.Name].Tag;
            }
        }
        public bool IsAllSet()
        {
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Result[0] as SupplierRelationInfo));
            //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //IList lst = model.MaterialHireMngSvr.GetMaterialRentalOrder(oq) as IList;
            //MaterialRentalOrderMaster oMaterialRentalOrderMaster = null;
            bool bFlag=true ;
            string sErrMsg = string.Empty;
            if (orderMaster != null)
            {
                if (orderMaster.Details.Count > 0)
                {

                    foreach (MatHireOrderDetail oMaterialRentalOrderDetail in orderMaster.Details)
                    {
                        if (oMaterialRentalOrderDetail.BasicDtlCostSets.Count == 0)
                        {
                            bFlag = false;
                            if (string.IsNullOrEmpty(sErrMsg))
                            {
                                sErrMsg = string.Format("以下物资未设置计价方式\t(物资名称【{0}】、编号【{1}】、材质【{2}】、规格【{3}】)\t", oMaterialRentalOrderDetail.MaterialResource .Name, oMaterialRentalOrderDetail.MaterialResource .Code , oMaterialRentalOrderDetail.MaterialResource .Stuff , oMaterialRentalOrderDetail.MaterialResource .Specification);
                                 
                            }
                            else
                            {
                                sErrMsg += string.Format("(物资名称【{0}】、编号【{1}】、材质【{2}】、规格【{3}】)\t", oMaterialRentalOrderDetail.MaterialResource.Name, oMaterialRentalOrderDetail.MaterialResource.Code, oMaterialRentalOrderDetail.MaterialResource.Stuff, oMaterialRentalOrderDetail.MaterialResource.Specification);
                            }
                        }
                    }
                }
                else
                {
                    sErrMsg = "该料具合同上没有明细";
                    bFlag = false;
                }
            }
            else
            {
                sErrMsg = "没有找到料具租赁合同";
                bFlag = false;
            }
            if (!bFlag)
            {
                MessageBox.Show(sErrMsg);
            }
            return bFlag;
        }
        public string  GetMaterialHireOrder()
        {
            orderMaster = null;
            string sMessage = string.Empty;
            if (this.txtSupply.Text == "" && this.txtSupply.Tag == null)
            {
                sMessage="请选择料具出租方!";
                return sMessage;
            }
            else if (this.TenantSelector.SelectedProject == null)
            {
                sMessage="请选择料具租赁方!";
                //this.TenantSelector.Focus();
                return sMessage;
            }
            else
            {
                //查询当前料具供应商合同租赁单价
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Result[0] as SupplierRelationInfo));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                IList lst = model.MaterialHireMngSvr.GetMaterialHireOrder(oq) as IList;
                // MaterialRentalOrderMaster orderMaster = new MaterialRentalOrderMaster();
                if (lst.Count > 0)
                {
                    orderMaster = lst[0] as MatHireOrderMaster;
                }
                if (orderMaster == null)
                {
                   sMessage=string.Format("[{0}]与[{1}]供应商没有签订料具租赁合同", projectInfo.Name, this.txtSupply.Text);
                }
            }
            return sMessage;
        }
        void dgDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GetMaterialHireOrder();
            //if (orderMaster != null)
            //{
                int i = e.RowIndex;
                this.dgDetail[colMaterialCode.Name, i].Tag = curMaterial;
                this.dgDetail[colMaterialCode.Name, i].Value = curMaterial.Code;
                this.dgDetail[colMaterialName.Name, i].Value = curMaterial.Name;
                this.dgDetail[colMaterialSpec.Name, i].Value = curMaterial.Specification;
                this.dgDetail[colUnit.Name, i].Tag = curMaterial.BasicUnit;
                this.dgDetail[colUnit.Name, i].Value = curMaterial.BasicUnit.Name;
                //this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                //this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                //this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.UsedRank;
                //this.dgDetail[this.colBorrowUnit.Name, i].Value = var.UsedRankName;
                if (orderMaster != null)
                {
                    foreach (MatHireOrderDetail orderDetail in orderMaster.Details)
                    {
                        if (orderDetail.MaterialResource.Id == curMaterial.Id)
                        {
                            this.dgDetail[colPrice.Name, i].Value = orderDetail.Price;
                        }
                    }
                }
            //}


        }
        void btnForward_Click(object sender,EventArgs e)
        {

             GetMaterialHireOrder();
             if (orderMaster != null)
             {
                 VDailyPlanSelector theVDailyPlanSelector = new VDailyPlanSelector();
                 theVDailyPlanSelector.selectType = 0;
                 theVDailyPlanSelector.ShowDialog();
                 IList list = theVDailyPlanSelector.Result;
                 if (list == null || list.Count == 0) return;
                 this.txtSupply.Enabled = false;
                 DailyPlanMaster theDailyPlanMaster = list[0] as DailyPlanMaster;
                 customEdit1.Tag = theDailyPlanMaster.Id;
                 customEdit1.Text = theDailyPlanMaster.Code;

                 ////显示引用的明细
                 this.dgDetail.Rows.Clear();
                 foreach (DailyPlanDetail var in theDailyPlanMaster.Details)
                 {
                     if (var.IsSelect == false) continue;
                     //if (var.MaterialResource == curMaterial)
                    // {
                         int i = this.dgDetail.Rows.Add();
                         this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                         this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                         this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                         this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                         this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                         this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                         this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                         this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                         this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.UsedRank;
                         this.dgDetail[this.colBorrowUnit.Name, i].Value = var.UsedRankName;
                         foreach (MatHireOrderDetail orderDetail in orderMaster.Details)
                         {
                             if (orderDetail.MaterialResource.Id == var.MaterialResource.Id)
                             {
                                 this.dgDetail[colPrice.Name, i].Value = orderDetail.Price;
                             }
                         }
                     //}
                 }
                // this.matRentalOrderMaster = orderMaster;
             }
        }

        void dgDetail_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point _Point = dgDetail.PointToClient(Cursor.Position);
                this.contextMenuStrip1.Show(dgDetail, _Point);
            }
        }

        void dgDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    list_delMatCollDtl.Add(dr.Tag as MatHireCollectionDetail);
                    matColMaster.Details.Remove(dr.Tag as MatHireCollectionDetail);
                }
            }
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
                matColMaster = new MatHireCollectionMaster();
                matColMaster.CreatePerson = ConstObject.LoginPersonInfo;
                matColMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                matColMaster.CreateDate = ConstObject.LoginDate;
                matColMaster.CreateYear = ConstObject.LoginDate.Year;
                matColMaster.CreateMonth = ConstObject.LoginDate.Month;
                matColMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                matColMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                matColMaster.HandlePerson = ConstObject.LoginPersonInfo;
                matColMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                matColMaster.DocState =   DocumentState.Edit;
                matColMaster.BalState = 0;//结算状态 0：未结算  1; 已结算
                matColMaster.MatHireType = enumMatHireType;
                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
                if (projectInfo != null)
                {
                    //txtProject.Tag = projectInfo;
                    //txtProject.Text = projectInfo.Name;
                    matColMaster.ProjectId = projectInfo.Id;
                    matColMaster.ProjectName = projectInfo.Name;
                }
                txtContractNo.Focus();
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
            if (matColMaster.DocState == DocumentState.Edit || matColMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                matColMaster = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(matColMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(matColMaster.DocState));
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
                        matColMaster = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(matColMaster.Id);
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
                if (!IsAllSet())
                {
                   
                    return false;//
                }
                if (!ViewToModel()) return false;
                if (!ValidKC()) return false;
                
                if (matColMaster.Id == null)
                {
                    matColMaster.DocState = DocumentState.InExecute;
                    matColMaster = model.MaterialHireMngSvr.SaveMaterialHireCollectionMaster(matColMaster);
                }
                else
                {
                    matColMaster.DocState = DocumentState.InExecute;
                    matColMaster = model.MaterialHireMngSvr.UpdateMaterialHireCollectionMaster(matColMaster, list_delMatCollDtl);
                }

                ////插入日志
                ////MStockIn.InsertLogData(matColMaster.Id, "保存", matColMaster.Code, matColMaster.CreatePerson.Name, "料具收料单","");
                txtCode.Text = matColMaster.Code;
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
                matColMaster = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(matColMaster.Id);
                if (matColMaster.DocState == DocumentState.Valid || matColMaster.DocState == DocumentState.Edit)
                {
                    if (!model.MaterialHireMngSvr.DeleteMaterialHireCollectionMaster(matColMaster)) return false;
                    ClearView();
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(matColMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public override void Refresh()
        {
            try
            {
                //重新获得当前对象的值
                matColMaster = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(matColMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {


            string validMessage = "";
            if (this.dgDetail.Rows .Count ==0|| (this.dgDetail.Rows.Count - 1 == 0 && this.dgDetail.Rows[0].IsNewRow))
            {
                MessageBox.Show("收料单明细不能为空");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));

            //业务日期不得大于服务器日期
            DateTime ServerDate = CommonMethod.GetServerDateTime();
            DateTime OperDate = this.operDate.Value.Date;
            if (DateTime.Compare(OperDate, ServerDate) > 0)
            {
                validMessage += "业务日期不得大于服务器日期！";
            }
            if (txtContractNo.Text == "")
            {
                validMessage += "原始合同号不能为空！\n";
            }
            if (txtSupply.Result.Count == 0)
            {
                validMessage += "出租方不能为空！\n";
            }
            //if (txtRank.Result.Count == 0)
            //{
            //    validMessage += "使用队伍不能为空！\n";
            //}

            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            //收料单明细表数据校验
            //Hashtable ht_repeat = new Hashtable();
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.Cells[this.colMaterialCode.Name].Tag==null) break;

                MatHireCollectionDetail dtl = new MatHireCollectionDetail();

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                else
                {
                    dtl.MaterialCode = ClientUtil.ToString(dr.Cells[colMaterialCode.Name].Value);
                }
                if (dr.Cells[colUsedPart.Name].Tag == null)
                {
                    MessageBox.Show("使用部位不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colUsedPart.Name];
                    return false;
                }
                else
                {
                    dtl.UsedPartName = ClientUtil.ToString(dr.Cells[colUsedPart.Name].Value);
                }
                if (dr.Cells[colSubject.Name].Tag == null)
                {
                    MessageBox.Show("核算科目不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colSubject.Name];
                    return false;
                }

                if (dr.Cells[this.colBorrowUnit.Name].Tag == null)
                {
                    MessageBox.Show("使用队伍不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colBorrowUnit.Name];
                    return false;
                }
                else
                {
                    dtl.BorrowUnitName = ClientUtil.ToString(dr.Cells[colBorrowUnit.Name].Value);
                }
                if (dr.Cells[colMaterialLength.Name].Value == null)
                {
                    MessageBox.Show("请选择物资长度");
                    dgDetail.CurrentCell = dr.Cells[colMaterialLength.Name];
                    return false;
                }
                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }
                //收料负数(退料)  校验库存
                if (ClientUtil.TransToDecimal(dr.Cells[colCollQuantity.Name].Value) <= 0)
                {
                    decimal stockQty = model.MaterialHireMngSvr.GetMatStockQty(txtSupply.Result[0] as SupplierRelationInfo, dr.Cells[this.colBorrowUnit.Name].Tag as SupplierRelationInfo, dr.Cells[colMaterialCode.Name].Tag as Material, projectInfo.Id, matColMaster.MatHireType);
                   //数量=根数*长度
                    decimal collQty = Math.Abs(ClientUtil.ToDecimal(dr.Cells[colCollQuantity.Name].Value)) * ClientUtil.ToDecimal(dr.Cells[this.colMaterialLength.Name].Value);
                    if (stockQty - collQty < 0)
                    {
                        MessageBox.Show("在[" + txtSupply.Text + "]" + "[" + dr.Cells[colMaterialName.Name].Value.ToString() + "],库存量为[" + stockQty + "]，退料数量大于库存量，请修改！");
                        dgDetail.CurrentCell = dr.Cells[colCollQuantity.Name];
                        return false;
                    }
                }
                if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) <= 0)
                {
                    MessageBox.Show("单价不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                    return false;
                }
                //去重  物资+使用部位+使用队伍
                //string linkStr = dr.Cells[colMaterialCode.Name].Value + "-" + dr.Cells[this.colBorrowUnit.Name].Value + "-" + dr.Cells[colUsedPart.Name].Value;
                //if (!ht_repeat.Contains(linkStr))
                //{
                //    ht_repeat.Add(linkStr, "");
                //}
                //else
                //{
                //    MessageBox.Show("[物资]+[使用部位]+[使用队伍]有重复信息，不可保存");
                //    return false;
                //}
            }

            dgDetail.Update();
            return true;
        }

        /// <summary>
        /// 保存前校验库存
        /// </summary>
        /// <returns></returns>
        private bool ValidKC()
        {
            int temp = 0;
            foreach (MatHireCollectionDetail detail in matColMaster.Details)
            {
                if (detail.Quantity < 0)
                {
                    temp++;
                }
            }
            if (temp > 0)
            {
                //校验当前收料单是否是插入的单据
                bool value = model.MaterialHireMngSvr.VerifyCollMatBusinessDate(operDate.Value.Date, projectInfo.Id);
                //校验库存情况
                //根据收料单构建一个退料单(明细只处理数量小于0)
                MatHireReturnMaster matReturnMaster = new MatHireReturnMaster();
                matReturnMaster.TheSupplierRelationInfo = matColMaster.TheSupplierRelationInfo;
                matReturnMaster.TheRank = matColMaster.TheRank;
                matReturnMaster.RealOperationDate = matColMaster.RealOperationDate;
                matReturnMaster.MatHireType = matColMaster.MatHireType;
                foreach (MatHireCollectionDetail detail in matColMaster.Details)
                {
                    if (detail.Quantity < 0)
                    {
                        MatHireReturnDetail MaterialReturnDetail = new MatHireReturnDetail() { Master = matReturnMaster };
                        MaterialReturnDetail.MaterialResource = detail.MaterialResource;
                        MaterialReturnDetail.ExitQuantity = Math.Abs(detail.Quantity);
                        matReturnMaster.Details.Add(MaterialReturnDetail);
                    }
                }

                DataDomain domain = model.MaterialHireMngSvr.VerifyReturnMatKC(matReturnMaster, value);
                if (ClientUtil.ToInt(domain.Name1) == 0)
                {
                    //通过
                    return true;
                }
                else if (ClientUtil.ToInt(domain.Name1) == 1)
                {
                    //当前库存不足
                    MessageBox.Show(domain.Name2.ToString());
                    return false;
                }
                else if (ClientUtil.ToInt(domain.Name1) == 2)
                {
                    //插入业务日期的库存不足
                    MessageBox.Show(domain.Name2.ToString());
                    return false;
                }
                else if (ClientUtil.ToInt(domain.Name1) == 3)
                {
                    //插入该笔退料后，业务日期[yyyy-MM-dd]的库存为负[yyyy-MM-dd]是计算中的第一笔负数的日期
                    MessageBox.Show(domain.Name2.ToString());
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = matColMaster.Code;
                operDate.Value = matColMaster.CreateDate ;
                this.txtContractNo.Text = matColMaster.OldContractNum;
                if (matColMaster.HandlePerson != null)
                {
                    txtHandlePerson.Tag = matColMaster.HandlePerson;
                    txtHandlePerson.Text = matColMaster.HandlePersonName;
                }
                if (matColMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Tag = matColMaster.TheSupplierRelationInfo;
                    txtSupply.Result.Add(matColMaster.TheSupplierRelationInfo);
                    txtSupply.Value = matColMaster.SupplierName;
                }
                if (matColMaster.TheRank != null)
                {
                    txtRank.Result.Clear();
                    txtRank.Tag = matColMaster.TheRank;
                    txtRank.Result.Add(matColMaster.TheRank);
                    txtRank.Value = matColMaster.TheRankName;
                }
                if (matColMaster.CreatePerson != null)
                {
                    txtCreatePerson.Tag = matColMaster.CreatePerson;
                    txtCreatePerson.Text = matColMaster.CreatePersonName;
                }
                txtRemark.Text = matColMaster.Descript;
                txtCreateDate.Text = matColMaster.CreateDate.ToShortDateString();
                txtSumQuantity.Text = ClientUtil.ToString(matColMaster.SumQuantity);
                txtTransCharge.Text = ClientUtil.ToString(matColMaster.TransportCharge);
                //txtProject.Text = matColMaster.ProjectName;
                //txtProject.Tag = matColMaster.ProjectId;
                TenantSelector.ProjectID = matColMaster.ProjectId;
                txtBalRule.Text = matColMaster.BalRule;

                //显示收料单明细
                this.dgDetail.Rows.Clear();

                //查询当前料具供应商合同租赁单价
                MatHireOrderMaster theMaterialRentalOrderMaster = new MatHireOrderMaster();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Result[0] as SupplierRelationInfo));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                theMaterialRentalOrderMaster = (model.MaterialHireMngSvr.GetMaterialHireOrder(oq) as IList)[0] as MatHireOrderMaster;

                foreach (MatHireCollectionDetail var in matColMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();
                    //设置物料
                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
                    this.dgDetail[colMaterialLength.Name, i].Value = var.MaterialLength;
                    //主要解决钢管 长度*数量
                    this.dgDetail[colCollQuantity.Name, i].Value = var.MaterialLength == 0 ? 0 : var.Quantity / var.MaterialLength;
                    this.dgDetail[colTempQty.Name, i].Value = var.Quantity;
                    this.dgDetail[colPrice.Name, i].Value = var.Price;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;

                    //设置使用部位
                    this.dgDetail[colUsedPart.Name, i].Tag = var.UsedPart;
                    this.dgDetail[colUsedPart.Name, i].Value = var.UsedPartName;
                    this.dgDetail[colSubject.Name, i].Tag = var.SubjectGUID;
                    this.dgDetail[colSubject.Name, i].Value = var.SubjectName;
                    //使用队伍
                    this.dgDetail[this.colBorrowUnit.Name, i].Tag = var.BorrowUnit;
                    this.dgDetail[colBorrowUnit.Name, i].Value = var.BorrowUnitName;

                    foreach (MatHireOrderDetail theDetail in theMaterialRentalOrderMaster.Details)
                    {
                        if (theDetail.MaterialResource.Id == var.MaterialResource.Id)
                        {
                            this.dgDetail[colPrice.Name, i].Value = theDetail.Price;
                        }
                    }
                    foreach (MatHireCollectionCostDtl costDtl in var.MatCostDtls)
                    {
                        this.dgDetail[costDtl.CostType, i].Value = costDtl.Money;
                    }
                    this.dgDetail.Rows[i].Tag = var;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                decimal sumExtMoney = 0;
                if (matColMaster.Id == null)//业务时间
                {
                    matColMaster.CreateDate = ConstObject.LoginDate;
                }
                if (this.txtSupply.Result.Count > 0)
                {
                    matColMaster.TheSupplierRelationInfo = this.txtSupply.Result[0] as SupplierRelationInfo;
                    matColMaster.SupplierName = this.txtSupply.Text;
                }
                if (this.txtRank.Result.Count > 0)
                {
                    matColMaster.TheRank = this.txtRank.Result[0] as SupplierRelationInfo;
                    matColMaster.TheRankName = this.txtRank.Text;
                }
                if (curMaterial != null)
                {
                    matColMaster.ContractId = curMaterial.Id;
                }
                matColMaster.CreateDate  = operDate.Value.Date;
                matColMaster.OldContractNum = this.txtContractNo.Text;
                if (txtTransCharge.Text != "")
                {
                    matColMaster.TransportCharge = ClientUtil.ToDecimal(txtTransCharge.Text);
                }
                matColMaster.Descript = this.txtRemark.Text;
                matColMaster.BalRule = this.txtBalRule.Text;
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MatHireCollectionDetail theMaterialCollectionDetail = new MatHireCollectionDetail();
                    if (var.Tag != null)
                    {
                        theMaterialCollectionDetail = var.Tag as MatHireCollectionDetail;
                        if (theMaterialCollectionDetail.Id == null)
                        {
                            matColMaster.Details.Remove(theMaterialCollectionDetail);
                        }
                    }
                    //材料
                    theMaterialCollectionDetail.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Material;
                    theMaterialCollectionDetail.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    theMaterialCollectionDetail.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);
                    theMaterialCollectionDetail.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    //材料长度
                   
                    theMaterialCollectionDetail.MaterialLength = ClientUtil.ToDecimal(var.Cells[colMaterialLength.Name].Value);
                    //计量单位
                    theMaterialCollectionDetail.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                    theMaterialCollectionDetail.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                    //主要解决钢管 长度*数量
                    theMaterialCollectionDetail.Quantity = theMaterialCollectionDetail.MaterialLength * TransUtil.ToDecimal(var.Cells[colCollQuantity.Name].Value);
                    decimal tempQty = TransUtil.ToDecimal(var.Cells[colTempQty.Name].Value);
                    theMaterialCollectionDetail.TempData = tempQty.ToString();
                    theMaterialCollectionDetail.RentalPrice = TransUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                    //使用部位
                    if (var.Cells[colUsedPart.Name].Value != null)
                    {
                        theMaterialCollectionDetail.UsedPart = var.Cells[colUsedPart.Name].Tag as GWBSTree;
                        theMaterialCollectionDetail.UsedPartName = ClientUtil.ToString(var.Cells[colUsedPart.Name].Value);
                        theMaterialCollectionDetail.UsedPartSysCode =  theMaterialCollectionDetail.UsedPart.SysCode;
                    }
                    //核算科目
                    if (var.Cells[colSubject.Name].Value != null)
                    {
                        theMaterialCollectionDetail.SubjectGUID = var.Cells[colSubject.Name].Tag as CostAccountSubject;
                        theMaterialCollectionDetail.SubjectName = ClientUtil.ToString(var.Cells[colSubject.Name].Value);
                        theMaterialCollectionDetail.SubjectSysCode = ClientUtil.ToString((var.Cells[colSubject.Name].Tag as CostAccountSubject).SysCode);
                    }
                    //使用队伍
                    if (var.Cells[this.colBorrowUnit.Name].Value != null)
                    {
                        theMaterialCollectionDetail.BorrowUnit = var.Cells[colBorrowUnit.Name].Tag as SupplierRelationInfo;
                        theMaterialCollectionDetail.BorrowUnitName = ClientUtil.ToString(var.Cells[colBorrowUnit.Name].Value);
                    }
                    theMaterialCollectionDetail.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    //处理费用明细
                    //先清除已有费用明细
                    IList list_temp = new ArrayList();
                    foreach (MatHireCollectionCostDtl costDtl in theMaterialCollectionDetail.MatCostDtls)
                    {
                        list_temp.Add(costDtl);
                    }
                    foreach (MatHireCollectionCostDtl detail in list_temp)
                    {
                        theMaterialCollectionDetail.MatCostDtls.Remove(detail);
                    }
                    //然后新增
                    foreach (string costType in list_costType_detail)
                    {
                        MatHireCollectionCostDtl matCost = new MatHireCollectionCostDtl();
                        matCost.CostType = costType;
                        matCost.Money = ClientUtil.ToDecimal(var.Cells[costType].Value);
                        sumExtMoney += matCost.Money;
                        theMaterialCollectionDetail.AddMatCostDtl(matCost);
                    }
                    matColMaster.AddDetail(theMaterialCollectionDetail);
                }

                matColMaster.SumExtMoney = sumExtMoney;


                //运输费
                decimal TransChagre = 0;
                if (txtTransCharge.Text == "")
                {
                    TransChagre = 0;
                }
                else
                {
                    TransChagre = ClientUtil.ToDecimal(txtTransCharge.Text);
                }
                matColMaster.SumExtMoney += TransChagre;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
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
                    matColMaster = model.MaterialHireMngSvr.GetMaterialHireCollectionMasterById(id);
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtBalRule };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name,colSubject.Name,colPrice.Name,colUsedPart.Name,
                colSubject.Name,colBorrowUnit.Name};
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
            txtSupply.Result.Clear();
            txtRank.Result.Clear();
        }

        private void ClearControl(Control c)
        {
            if (c != null && c.Controls != null)
            {
                foreach (Control cd in c.Controls)
                {
                    ClearControl(cd);
                }
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
            else if (c is UcTenantSelector)
            {
                (c as UcTenantSelector).SelectedProject = null;
            }
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
           
                DataGridViewRow oCurrentRow = e.Row;
              if (oCurrentRow == null || oCurrentRow.IsNewRow) return;
              if (oCurrentRow.Tag == null)
              {
                  this.dgDetail.Rows.Remove(oCurrentRow);
              }
              else
              {
                  matColMaster.Details.Remove(e.Row.Tag as BaseDetail);
              }

            
            
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPrice.Name || colName == colCollQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("请输入数字！");
                        dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value = null;
                    }
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                        MessageBox.Show("请输入数字！");
                }
                //根据单价和数量计算金额  
                object price = dgDetail.Rows[e.RowIndex].Cells[colPrice.Name].Value;
                object quantity = dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value;
                object oLen;
                decimal sumqty = 0;

                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    quantity = dgDetail.Rows[i].Cells[colCollQuantity.Name].Value;
                    oLen = dgDetail.Rows[i].Cells[colMaterialLength.Name].Value;
                    if (quantity == null) quantity = 0;
                    if (oLen == null) oLen = 0;
                    sumqty = sumqty + ClientUtil.TransToDecimal(quantity) * ClientUtil.ToDecimal(oLen);
                }

                txtSumQuantity.Text = sumqty.ToString();
                if (dgDetail.Rows[e.RowIndex].Cells[colCollQuantity.Name].Value != null)
                {
                    //计算当前的全部费用金额
                    foreach (string costType in list_costType_detail)
                    {
                        string expression = GetCostExpression(costType, dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag as Material);
                        if (expression != "")
                        {
                            string qty = (ClientUtil.ToDecimal(this.dgDetail.CurrentRow.Cells[colCollQuantity.Name].Value) * ClientUtil.ToDecimal(this.dgDetail.CurrentRow.Cells[this.colMaterialLength.Name].Value)).ToString();
                            this.dgDetail.CurrentRow.Cells[costType].Value = GetCalResult(qty, expression);
                        }
                        else
                        {
                            this.dgDetail.CurrentRow.Cells[costType].Value = 0;
                        }
                    }
                }
            }
        }

        //void txtSumBusQty_tbTextChanged(object sender, EventArgs e)
        //{
        //    bool validity = true;
        //    string sumBusQty_temp = this.txtSumBusQty.Text;
        //    validity = CommonMethod.VeryValid(sumBusQty_temp);
        //    if (validity == false)
        //    {
        //        MessageBox.Show("请输入数字！");
        //        return;
        //    }
        //    else if (validity == true)
        //    {
        //        //计算当前非数量费用金额
        //        if (txtSumBusQty.Text != "")
        //        {
        //            //计算当前非数量费用金额
        //            decimal sumBusQty = ClientUtil.ToDecimal(this.txtSumBusQty.Text);
        //            foreach (string costType in list_costType_costDetail)
        //            {
        //                if (matRentalOrderMaster != null)
        //                {
        //                    foreach (BasicCostSet costSet in matRentalOrderMaster.BasiCostSets)
        //                    {
        //                        if (costSet.MatCostType == costType)
        //                        {
        //                            this.dgCostDetail[costType, 0].Value = sumBusQty * costSet.CostPrice;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        void txtTransCharge_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string TransCharge = this.txtTransCharge.Text;
            validity = CommonMethod.VeryValid(TransCharge);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                return;
            }
        }

        /// <summary>
        /// 物料编码列增加事件监听，支持处理键盘回车查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.PreviewKeyDown -= new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                textBox.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals("MaterialCode"))
                {
                    textBox.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }

            }
        }

        /// <summary>
        /// 键盘回车查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (base.ViewState == MainViewState.Browser || base.ViewState == MainViewState.Initialize) return;

            if (e.KeyValue == 13)
            {
                CommonMaterial materialSelector = new CommonMaterial();

                TextBox textBox = sender as TextBox;
                if (textBox.Text != null && !textBox.Text.Equals(""))
                {
                    materialSelector.OpenSelect(textBox.Text);
                }
                else
                {
                    materialSelector.OpenSelect();
                }
                IList list = materialSelector.Result;

                if (list != null && list.Count > 0)
                {
                    Application.Resource.MaterialResource.Domain.Material selectedMaterial = list[0] as Application.Resource.MaterialResource.Domain.Material;
                    this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = selectedMaterial;
                    this.dgDetail.CurrentCell.Value = selectedMaterial.Code;
                    this.dgDetail.CurrentRow.Cells[colMaterialName.Name].Value = selectedMaterial.Name;
                    this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = selectedMaterial.Specification;

                    //动态分类复合单位                    
                    DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
                    cbo.Items.Clear();

                    StandardUnit first = null;
                    foreach (StandardUnit cu in selectedMaterial.GetMaterialAllUnit())
                    {
                        cbo.Items.Add(cu.Name);
                    }
                    first = selectedMaterial.BasicUnit;
                    this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = first;
                    cbo.Value = first.Name;
                    this.dgDetail.RefreshEdit();
                }
            }
        }

        /// <summary>
        /// 在物料编码列，敲击键盘时，取消原来已经选择的物料，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgDetail.CurrentCell.Tag = null;
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialName.Name].Value = "";
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialSpec.Name].Value = "";
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                string sError = string.Empty;
                if (this.txtSupply.Text == "" && this.txtSupply.Tag == null)
                {
                    MessageBox.Show("请选择料具出租方!");
                    return;
                }
                else
                {
                    string sMessage = GetMaterialHireOrder();
                    if (orderMaster != null)
                    {
                        // if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                        DataGridViewRow theCurrentRow = this.dgDetail.Rows[e.RowIndex];
                        CommonMaterial materialSelector = new CommonMaterial();
                        materialSelector.materialCatCode = CommonUtil.MaterialCateGGCode;
                        materialSelector.OpenSelect();
                        int i = 0;
                        if (materialSelector.Result != null && materialSelector.Result.Count > 0)
                        {
                            for (int iLen = materialSelector.Result.Count - 1; iLen > -1; iLen--)
                            {
                                Material oMaterial = materialSelector.Result[iLen] as Material;
                                IList<MatHireOrderDetail> lstOrderDetail = orderMaster.Details.OfType<MatHireOrderDetail>().Where(a => a.MaterialResource.Id == oMaterial.Id).ToList();
                                if (lstOrderDetail==null || lstOrderDetail.Count==0)
                                {
                                    sError += (string.IsNullOrEmpty(sError) ? oMaterial.Name : ("、" + oMaterial.Name));
                                    //MessageBox.Show(string.Format("[{0}未与[2]租赁商签订合同]", oMaterial.Name, this.txtSupply.Text));
                                    materialSelector.Result.Remove(oMaterial);
                                }
                            }
                            if (materialSelector.Result.Count > 1)
                            {
                                dgDetail.Rows.Insert(e.RowIndex, materialSelector.Result.Count - 1);
                            }
                            foreach (Material oMaterial in materialSelector.Result)
                            {
                                theCurrentRow = dgDetail.Rows[e.RowIndex + i];
                                i++;
                                IList<MatHireOrderDetail> lstOrderDetail = orderMaster.Details.OfType<MatHireOrderDetail>().Where(a => a.MaterialResource.Id == oMaterial.Id).ToList();
                                theCurrentRow.Cells[this.colMaterialCode.Name].Tag = oMaterial;
                                theCurrentRow.Cells[this.colMaterialCode.Name].Value = oMaterial.Code;
                                theCurrentRow.Cells[this.colMaterialName.Name].Value = oMaterial.Name;
                                theCurrentRow.Cells[this.colMaterialSpec.Name].Value = oMaterial.Specification;
                                theCurrentRow.Cells[this.colPrice.Name].Value = lstOrderDetail[0].Price;
                                if (lstOrderDetail[0].MatStandardUnit != null)
                                {
                                    theCurrentRow.Cells[this.colUnit.Name].Tag = lstOrderDetail[0].MatStandardUnit;
                                    theCurrentRow.Cells[this.colUnit.Name].Value = lstOrderDetail[0].MatStandardUnit.Name;
                                }

                            }
                            dgDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            txtCode.Focus();
                            if (!string.IsNullOrEmpty(sError))
                            {
                                MessageBox.Show(string.Format("无法添加[{0}]物资,因为[{1}]与[{2}]租赁商签订合同]", sError,projectInfo.Name, this.txtSupply.Text));
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("[{0}]与[{1}]供应商没有签订料具租赁合同", projectInfo.Name, this.txtSupply.Text));
                    }
                }
            }
            #region 
            //else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colMaterialLength.Name))//选择长度
            //{
                
            //    DataGridViewRow theCurrentRow = this.dgDetail.Rows[e.RowIndex],oTempRow=null;

            //   // decimal dPrice = ClientUtil.ToDecimal(theCurrentRow.Cells[colPrice.Name].Value);
            //    if (theCurrentRow.Cells[this.colMaterialCode.Name].Tag != null)
            //    {
            //        int iRowIndex = 0;
            //        Material oMaterial = theCurrentRow.Cells[this.colMaterialCode.Name].Tag as Material;
            //        VSelectMaterialLong oVSelectMaterialLong = new VSelectMaterialLong();
            //        oVSelectMaterialLong.StartPosition = FormStartPosition.CenterScreen;
            //        oVSelectMaterialLong.ShowDialog();
            //        if (oVSelectMaterialLong.Result != null && oVSelectMaterialLong.Result.Count > 0)
            //        {
            //            foreach (DataGridViewRow oRow in dgDetail.Rows)
            //            {
            //                //if (oRow.IsNewRow) break;
            //                if (oRow!=theCurrentRow&&  oRow.Cells[colMaterialCode.Name].Tag != null && string.Equals(oMaterial.Id,(oRow.Cells[colMaterialCode.Name].Tag as Material).Id))
            //                {
            //                    if (oRow.Cells[colMaterialLength.Name].Value != null)
            //                    {
            //                        oVSelectMaterialLong.Result.Remove(ClientUtil.ToDecimal(oRow.Cells[colMaterialLength.Name].Value));
            //                    }
            //                }
            //            }
            //            if (oVSelectMaterialLong.Result.Count > 1)
            //            {
            //                this.dgDetail.Rows.Insert(e.RowIndex, oVSelectMaterialLong.Result.Count - 1);
            //            }
                        
            //            for (int i = 0; i < oVSelectMaterialLong.Result.Count; i++)
            //            {
            //                iRowIndex = e.RowIndex +i;
            //                oTempRow = this.dgDetail.Rows[iRowIndex];
            //                oTempRow.Cells[this.colMaterialCode.Name].Tag = theCurrentRow.Cells[colMaterialCode.Name].Tag;
            //                oTempRow.Cells[this.colMaterialCode.Name].Value = theCurrentRow.Cells[colMaterialCode.Name].Value;
            //                oTempRow.Cells[this.colMaterialName.Name].Value = theCurrentRow.Cells[colMaterialName.Name].Value;
            //                oTempRow.Cells[this.colMaterialSpec.Name].Value = theCurrentRow.Cells[colMaterialSpec.Name].Value;
            //                oTempRow.Cells[this.colPrice.Name].Value = theCurrentRow.Cells[colPrice.Name].Value;
            //                oTempRow.Cells[this.colUnit.Name].Value = theCurrentRow.Cells[colUnit.Name].Value;
            //                oTempRow.Cells[this.colUnit.Name].Tag = theCurrentRow.Cells[colUnit.Name].Tag;
            //                oTempRow.Cells[this.colMaterialLength.Name].Value = oVSelectMaterialLong.Result[i];
                           
            //            }
            //            dgDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //            this.txtCode.Focus();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("请先选择物资");
            //    }

            //}
            #endregion
            else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedPart.Name))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
                frm.IsTreeSelect = true;
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    TreeNode root = frm.SelectResult[0];

                    GWBSTree task = root.Tag as GWBSTree;
                    if (task != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Value = task.Name;
                        this.dgDetail.CurrentRow.Cells[colUsedPart.Name].Tag = task;
                        dgDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        this.txtCode.Focus();
                    }
                }
            }
            else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSubject.Name))
            {
                VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                frm.ShowDialog();
                CostAccountSubject cost = frm.SelectAccountSubject;
                if (cost != null)
                {
                    if (dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colSubject.Name].Tag = cost;
                        this.dgDetail.CurrentRow.Cells[colSubject.Name].Value = cost.Name;
                    }
                    else
                    {
                        MessageBox.Show("请先选择物资信息！");
                        return;
                    }
                }
                dgDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
                this.txtCode.Focus();
            }
            else if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(this.colBorrowUnit.Name))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                VCommonSupplierRelationSelect comSelect = new VCommonSupplierRelationSelect();
                comSelect.OpenSelectView("", CommonUtil.SupplierCatCode3);
                IList list = comSelect.Result;
                if (list != null && list.Count > 0)
                {
                    SupplierRelationInfo relInfo = list[0] as SupplierRelationInfo;
                    this.dgDetail.CurrentRow.Cells[colBorrowUnit.Name].Tag = relInfo;
                    this.dgDetail.CurrentRow.Cells[colBorrowUnit.Name].Value = relInfo.SupplierInfo.Name;
                }
                dgDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
                this.txtCode.Focus();
            }
            else
            {
            }
        }

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            if (LoadTempleteFile(@"料具收料单.flx") == false) return false;
            FillFlex(matColMaster);
            flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public override bool Print()
        {
            if (LoadTempleteFile(@"料具收料单.flx") == false) return false;
            FillFlex(matColMaster);
            flexGrid1.Print();
            matColMaster.PrintTimes = matColMaster.PrintTimes + 1;
            matColMaster = model.MaterialHireMngSvr.UpdateMaterialHireCollectionMaster(matColMaster, list_delMatCollDtl);
            return true;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public override bool Export()
        {
            if (LoadTempleteFile(@"料具收料单.flx") == false) return false;
            FillFlex(matColMaster);
            flexGrid1.ExportToExcel("料具收料单【" + matColMaster.Code + "】", false, false, true);
            return true;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="matColMaster"></param>
        private void FillFlex(MatHireCollectionMaster billMaster)
        {
            int detailStartRowNumber = 7;//6为模板中的行号
            int detailCount = billMaster.Details.Count;

            //插入明细行
            flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;
            CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
            //主表数据

            flexGrid1.Cell(3, 2).Text = billMaster.Code;
            flexGrid1.Cell(3, 6).Text = billMaster.ForwardBillCode;
            flexGrid1.Cell(3, 9).Text = billMaster.SupplierName;
            flexGrid1.Cell(5, 10).Text = ClientUtil.ToString(billMaster.TransportCharge);
            flexGrid1.Cell(4, 2).Text = ClientUtil.ToString(billMaster.OldContractNum);
            flexGrid1.Cell(4, 5).Text = ClientUtil.ToString(billMaster.BalRule);
            flexGrid1.Cell(4, 8).Text = ClientUtil.ToString(billMaster.TheRankName);
            flexGrid1.Cell(5, 8).Text = billMaster.RealOperationDate.ToShortDateString();

            //填写明细数据
            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            for (int i = 0; i < detailCount; i++)
            {
                MatHireCollectionDetail detail = (MatHireCollectionDetail)billMaster.Details.ElementAt(i);
                //物资名称
                flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialName;//detail.MaterialResource.Name;
                flexGrid1.Cell(detailStartRowNumber + i, 1).WrapText = true;
                //规格型号
                flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialSpec;//detail.MaterialResource.Specification;

                //计量单位
                flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.MatStandardUnitName;

                //收料数量
                flexGrid1.Cell(detailStartRowNumber + i, 4).Text = ClientUtil.ToString(detail.Quantity);
                sumQuantity += detail.Quantity;
                //租赁价格
                flexGrid1.Cell(detailStartRowNumber + i, 5).Text = ClientUtil.ToString(detail.RentalPrice);

                //计算总费用
                if (detail.Quantity.Equals("") || detail.Price.Equals(""))
                { }
                else
                {
                    decimal money = detail.Quantity * detail.Price;
                    sumMoney += money;
                }
                //使用部位
                flexGrid1.Cell(detailStartRowNumber + i, 6).Text = ClientUtil.ToString(detail.UsedPartName);
                //借用数量
                flexGrid1.Cell(detailStartRowNumber + i, 7).Text = ClientUtil.ToString(detail.RentalPrice);
                //借用单位
                flexGrid1.Cell(detailStartRowNumber + i, 8).Text = ClientUtil.ToString(detail.BorrowUnitName);
                //力资费用
                flexGrid1.Cell(detailStartRowNumber + i, 9).Text = "";
                //核算科目
                flexGrid1.Cell(detailStartRowNumber + i, 10).Text = ClientUtil.ToString(detail.SubjectName);
                flexGrid1.Cell(detailStartRowNumber + i, 11).WrapText = true;
                //备注
                flexGrid1.Cell(detailStartRowNumber + i,11).Text = detail.Descript;
                flexGrid1.Cell(detailStartRowNumber + i, 11).WrapText = true;
                //条形码
                string a = Application.Business.Erp.SupplyChain.Client.Util.DateUtil.GetStrMoneyInchar(ClientUtil.ToString(sumMoney));
                this.flexGrid1.Cell(1, 9).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-" + a;
                this.flexGrid1.Cell(1, 9).CellType = FlexCell.CellTypeEnum.BarCode;
                this.flexGrid1.Cell(1, 9).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;
                flexGrid1.Row(detailStartRowNumber + i).AutoFit();
            }
            flexGrid1.Cell(7 + detailCount, 2).Text = billMaster.ProjectName;
            flexGrid1.Cell(5,2).Text = ClientUtil.ToString(sumQuantity);
            flexGrid1.Cell(5,5).Text =  ClientUtil.ToString(sumMoney);
            flexGrid1.Cell(7 + detailCount, 7).Text = billMaster.CreateDate.ToShortDateString();
            flexGrid1.Cell(7 + detailCount,10).Text = billMaster.CreatePersonName;


        }
        /// <summary>
        /// 装载模板
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 供应商发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSupply_TextChanged(object sender, EventArgs e)
        {

        }

        #region 解析表达式，计算费用金额
        //通过计算表达式，计算本单元格的计算值
        private decimal GetCalResult(string collQuantity, string express)
        {
            decimal result = 0;

            if (express.IndexOf("收料") != -1)
            {
                express = express.Replace("收料", collQuantity);
            }
            result = Decimal.Parse(CommonUtil.CalculateExpression(express, 2));
            return result;
        }
        //获取表达式
        private string GetCostExpression(string costType, Material material)
        {
            string expression = "";
            foreach (MatHireOrderDetail detail in orderMaster.Details)
            {
                if (detail.MaterialResource.Id == material.Id)
                {
                    foreach (OrderDetailCostSetItem theBasicDtlCostSet in detail.BasicDtlCostSets)
                    {
                        if (theBasicDtlCostSet.CostType == costType)
                        {
                            expression = theBasicDtlCostSet.ApproachExpression;
                            break;
                        }
                    }
                }
            }

            //获取全部价格类型
            IList list = StaticMethod.GetBasicDataByName(VBasicDataOptr.BASICDATENAME_PRICETYPE);
            foreach (MatHireOrderDetail detail in this.orderMaster.Details)
            {
                if (detail.MaterialResource.Id == material.Id)
                {
                    foreach (BasicDataOptr theBasicDataOptr in list)
                    {
                        string priceType = "";
                        if (expression.IndexOf(theBasicDataOptr.BasicName) != -1)
                        {
                            priceType = theBasicDataOptr.BasicName;
                        }
                        if (priceType != "")
                        {
                            int count = 0;
                            foreach (OrderDetailCostSetItem theBasicDtlCostSet in detail.BasicDtlCostSets)
                            {
                                //取基本费用明细中的价格类型
                                if (theBasicDtlCostSet.SetType == "价格定义")
                                {
                                    if (theBasicDtlCostSet.CostType == priceType)
                                    {
                                        string price = theBasicDtlCostSet.Price.ToString();
                                        expression = expression.Replace(priceType, price);
                                        count++;
                                    }
                                }
                            }
                            //count=0：用户未定义当前价格类型，此时赋值1
                            if (count == 0)
                            {
                                expression = expression.Replace(priceType, ClientUtil.ToString(1));
                            }
                        }
                    }
                }
            }
            return expression;
        }
        #endregion


    }
}
