using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using System.Threading;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    public partial class VMaterialHireOrderCopy : TBasicDataView
    {
        private MMaterialHireMng model = new MMaterialHireMng();
        CurrentProjectInfo DestProjectInfo = null;
        CurrentProjectInfo SourceProjectInfo = null;
        public VMaterialHireOrderCopy()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
            DestSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
            //projectInfo = StaticMethod.GetProjectInfo();
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.SourceTenantSelector.TenantSelectorAfterEvent += (a) => { SourceProjectInfo = a.SelectedProject; };
            this.DestTenantSelector.TenantSelectorAfterEvent += (a) => { DestProjectInfo = a.SelectedProject; };
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnCopy.Click +=new EventHandler(btnCopy_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        //void btnSelect_Click(object sender, EventArgs e)
        //{
        //    VSelectProjectInfo project = new VSelectProjectInfo();
        //    CurrentProjectInfo extProject = new CurrentProjectInfo();
        //    project.ListExtendProject.Add(extProject);
        //    project.ShowDialog();

        //    if (project.Result != null && project.Result.Count > 0)
        //    {
        //        CurrentProjectInfo selectProject = project.Result[0] as CurrentProjectInfo;
        //        if (selectProject != null)
        //        {
        //            txtProject.Text = selectProject.Name;
        //            txtProject.Tag = selectProject;
        //        }
        //    }
        //}

        void btnCopy_Click(object sender,EventArgs e)
        {
            try
            {
                SupplierRelationInfo oSupplier = null; ;
                CurrentProjectInfo oProjectInfo = null; ;
                MatHireOrderMaster mat = new MatHireOrderMaster();
                MatHireOrderMaster master = dgDetail.CurrentRow.Tag as MatHireOrderMaster;
                
               


               
                if (DestProjectInfo == null && (DestSupply.Result == null || DestSupply.Result.Count == 0))
                {
                    MessageBox.Show("请选择目标[租赁方]和[出租方](可以任选一个)!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DestTenantSelector.Focus();
                    return;
                }
                else
                {
                    //查询当前项目复制的单位是否已经有租赁合同
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", master.Id));
                    master = model.MaterialHireMngSvr.GetMaterialHireOrder(oq)[0] as MatHireOrderMaster;
                    oProjectInfo = DestProjectInfo == null ? new CurrentProjectInfo() { Id = master.ProjectId, Name = master.ProjectName } : DestProjectInfo;
                    oSupplier = (DestSupply.Result == null || DestSupply.Result.Count == 0) ? master.TheSupplierRelationInfo: (DestSupply.Result[0] as SupplierRelationInfo);
                    oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("ProjectId", oProjectInfo.Id));
                    oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo.Id", oSupplier.Id));
                    IList listMaster = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireOrderMaster), oq);
                    if (listMaster != null && listMaster.Count > 0)
                    {
                        MessageBox.Show("当[" + DestProjectInfo.Name + "]的[" + dgDetail.CurrentRow.Cells[colSupplyInfo.Name].Value.ToString() + "]已经存在料具租赁合同！");
                        return;
                    }
                    mat.CreatePerson = ConstObject.LoginPersonInfo;
                mat.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                mat.CreateDate = ConstObject.LoginDate;
                mat.CreateYear = ConstObject.LoginDate.Year;
                mat.CreateMonth = ConstObject.LoginDate.Month;
                mat.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                mat.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                mat.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                mat.HandlePerson = ConstObject.LoginPersonInfo;
                mat.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                mat.DocState = DocumentState.Edit;
                    
             

                    mat.ProjectId = oProjectInfo.Id;
                    mat.ProjectName = oProjectInfo.Name;
                    mat.TheSupplierRelationInfo = oSupplier;
                    mat.SupplierID = oSupplier.Id;
                    mat.SupplierName = oSupplier.SupplierInfo.Name;
                   // mat.SupplierType = oSupplier.;
                    mat.OriginalContractNo = master.OriginalContractNo;
                    mat.BalRule = master.BalRule;
                    mat.Descript = master.Descript;
                 
                    foreach (MatHireOrderDetail detail in master.Details)
                    {
                        MatHireOrderDetail dtl = new MatHireOrderDetail();
                        ObjectQuery objectquery = new ObjectQuery();
                        objectquery.AddCriterion(Expression.Eq("Master", detail));
                        IList list = model.MaterialHireMngSvr.ObjectQuery(typeof(OrderDetailCostSetItem), objectquery);
                        dtl.Descript = detail.Descript;
                        dtl.DiagramNumber = detail.DiagramNumber;
                        dtl.MaterialCode = detail.MaterialCode;
                        dtl.MaterialName = detail.MaterialName;
                        dtl.MaterialResource = detail.MaterialResource;
                        dtl.MaterialSpec = detail.MaterialSpec;
                        dtl.MaterialStuff = detail.MaterialStuff;
                        dtl.MatStandardUnit = detail.MatStandardUnit;
                        dtl.MatStandardUnitName = detail.MatStandardUnitName;
                        dtl.Money = detail.Money;
                        dtl.Price = detail.Price;
                        dtl.Quantity = detail.Quantity;
                        dtl.RuleState = detail.RuleState;

                        if (list != null || list.Count > 0)
                        {
                            foreach (OrderDetailCostSetItem dtlCostSet in list)
                            {
                                OrderDetailCostSetItem cost = new OrderDetailCostSetItem();
                                cost.ApproachExpression = dtlCostSet.ApproachExpression;
                                cost.CostType = dtlCostSet.CostType;
                                cost.Descript = dtlCostSet.Descript;
                                cost.DiagramNumber = dtlCostSet.DiagramNumber;
                                cost.ExitExpression = dtlCostSet.ExitExpression;
                                cost.ForwardDetailId = dtlCostSet.ForwardDetailId;
                                cost.MaterialCode = dtlCostSet.MaterialCode;
                                cost.MaterialName = dtlCostSet.MaterialName;
                                cost.MaterialResource = dtlCostSet.MaterialResource;
                                cost.MaterialSpec = dtlCostSet.MaterialSpec;
                                cost.MaterialStuff = dtlCostSet.MaterialStuff;
                                cost.MatStandardUnit = dtlCostSet.MatStandardUnit;
                                cost.MatStandardUnitName = dtlCostSet.MatStandardUnitName;
                                cost.Money = dtlCostSet.Money;
                                cost.Price = dtlCostSet.Price;
                                cost.Quantity = dtlCostSet.Quantity;
                                cost.RefQuantity = dtlCostSet.RefQuantity;
                                cost.SetType = dtlCostSet.SetType;
                                cost.UsedPart = dtlCostSet.UsedPart;
                                cost.UsedPartName = dtlCostSet.UsedPartName;
                                cost.UsedPartSysCode = dtlCostSet.UsedPartSysCode;
                                cost.WorkContent = dtlCostSet.WorkContent;
                                cost.Master = dtl;
                                dtl.AddBasicDtlCostSet(cost);
                            }
                        }
                        mat.AddDetail(dtl);
                    }
                    foreach (OrderMasterCostSetItem cost in master.BasiCostSets)
                    {
                        cost.Id = null;
                        mat.AddBasicCostSet(cost);
                    }
                    mat = model.MaterialHireMngSvr.SaveMaterialHireOrderMaster(mat);
                    MessageBox.Show("复制成功！");
                    btnSearch_Click(null, null);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("复制数据出错。\n" + ExceptionUtil.ExceptionMessage( ex));
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                dgDetail.Rows.Clear();
               
                FlashScreen.Show("信息查询中......");
                ObjectQuery oq = new ObjectQuery();
                if (SourceProjectInfo != null)
                {
                    oq.AddCriterion(Expression.Eq("ProjectId", SourceProjectInfo.Id));
                }
                if (txtSupply.Result != null && txtSupply.Result.Count > 0  && txtSupply.Result[0] is SupplierRelationInfo)
                {
                    oq.AddCriterion(Expression.Eq( "TheSupplierRelationInfo", (txtSupply.Result[0] as SupplierRelationInfo)));
                }
                oq.AddOrder(new Order("Code", false));
                oq.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
                IList list = model.MaterialHireMngSvr.ObjectQuery(typeof(MatHireOrderMaster), oq); //model.MaterialHireMngSvr.GetMaterialHireOrder(oq);
                if (list == null || list.Count == 0)
                    
                {
                    FlashScreen.Close();
                    return;
                }
                foreach (MatHireOrderMaster master in list)
                {
                    int i = dgDetail.Rows.Add();
                    dgDetail[colSupplyInfo.Name, i].Value = master.TheSupplierRelationInfo.SupplierInfo.Name;
                    dgDetail[colProjectName.Name, i].Value = master.ProjectName;
                    dgDetail[colBalRule.Name, i].Value = master.BalRule;
                    dgDetail[colCreatePerson.Name, i].Value = master.CreatePersonName;
                    dgDetail[colOriContractNo.Name, i].Value = master.OriginalContractNo;
                    dgDetail[colRemark.Name, i].Value = master.Descript;
                    dgDetail[colSignDate.Name, i].Value = master.CreateDate.ToShortDateString();
                    dgDetail[colRealOperationDate.Name, i].Value = master.RealOperationDate;
                    dgDetail.Rows[i].Tag = master;
                }
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                Thread.Sleep(1);
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
               
            }
        }
      //双击事件
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = (dgDetail.Rows[e.RowIndex].Tag as MatHireOrderMaster).Id.ToString();
                if (ClientUtil.ToString(billId) != "")
                {
                    VMaterialHireOrder vOrder = new VMaterialHireOrder();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }
    }
}
