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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder
{
    public partial class VMaterialRentalOrderCopy : TBasicDataView
    {
        private MMatRentalMng model = new MMatRentalMng();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        public VMaterialRentalOrderCopy()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSelect.Click +=new EventHandler(btnSelect_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnCopy.Click +=new EventHandler(btnCopy_Click);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        void btnSelect_Click(object sender, EventArgs e)
        {
            VSelectProjectInfo project = new VSelectProjectInfo();
            CurrentProjectInfo extProject = new CurrentProjectInfo();
            project.ListExtendProject.Add(extProject);
            project.ShowDialog();

            if (project.Result != null && project.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = project.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    txtProject.Text = selectProject.Name;
                    txtProject.Tag = selectProject;
                }
            }
        }

        void btnCopy_Click(object sender,EventArgs e)
        {
            try
            {
                MaterialRentalOrderMaster mat = new MaterialRentalOrderMaster();
                MaterialRentalOrderMaster master = dgDetail.CurrentRow.Tag as MaterialRentalOrderMaster;
                //查询当前项目复制的单位是否已经有租赁合同
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("TheSupplierRelationInfo", master.TheSupplierRelationInfo));
                IList listMaster = model.MatMngSrv.ObjectQuery(typeof(MaterialRentalOrderMaster), oq);
                if (listMaster != null && listMaster.Count > 0)
                {
                    MessageBox.Show("当前项目的[" + dgDetail.CurrentRow.Cells[colSupplyInfo.Name].Value.ToString() + "]已经存在料具租赁合同！");
                    return;
                }
                mat.ProjectId = projectInfo.Id;
                mat.ProjectName = projectInfo.Name;
                mat.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                mat.CreatePerson = ConstObject.LoginPersonInfo;
                mat.HandlePerson = ConstObject.LoginPersonInfo;
                mat.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                mat.OperOrgInfo = ConstObject.TheOperationOrg;
                mat.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
                mat.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                mat.CreateDate = ConstObject.LoginDate;
                mat.TheSupplierRelationInfo = master.TheSupplierRelationInfo;
                mat.OriginalContractNo = master.OriginalContractNo;
                mat.BalRule = master.BalRule;
                mat.Descript = master.Descript;
                foreach (MaterialRentalOrderDetail detail in master.Details)
                {
                    MaterialRentalOrderDetail dtl = new MaterialRentalOrderDetail();
                    ObjectQuery objectquery = new ObjectQuery();
                    objectquery.AddCriterion(Expression.Eq("Master", detail));
                    IList list = model.MatMngSrv.ObjectQuery(typeof(BasicDtlCostSet), objectquery);
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
                        foreach (BasicDtlCostSet dtlCostSet in list)
                        {
                            BasicDtlCostSet cost = new BasicDtlCostSet();
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
                foreach (BasicCostSet cost in master.BasiCostSets)
                {
                    cost.Id = null;
                    mat.AddBasicCostSet(cost);
                }
                mat = model.SaveMaterialRentalOrder(mat);
                MessageBox.Show("复制成功！");
            }
            catch(Exception ex)
            {
                MessageBox.Show("复制数据出错。\n" + ex.Message);
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                dgDetail.Rows.Clear();
                string strName = txtProject.Text.ToString().Trim();
                CurrentProjectInfo pro = txtProject.Tag as CurrentProjectInfo;
                if (strName == "" || pro == null)
                {
                    MessageBox.Show("请先选择项目信息！");
                    return;
                }
                FlashScreen.Show("信息查询中......");
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", (txtProject.Tag as CurrentProjectInfo).Id));
                oq.AddOrder(new Order("Code", false));
                IList list = model.MatMngSrv.GetMaterialRentalOrder(oq);
                if (list == null || list.Count == 0)
                {
                    FlashScreen.Close();
                    return;
                }
                foreach (MaterialRentalOrderMaster master in list)
                {
                    int i = dgDetail.Rows.Add();
                    dgDetail[colSupplyInfo.Name, i].Value = master.TheSupplierRelationInfo.SupplierInfo.Name;
                    dgDetail[colBalRule.Name, i].Value = master.BalRule;
                    dgDetail[colCreatePerson.Name, i].Value = master.CreatePersonName;
                    dgDetail[colOriContractNo.Name, i].Value = master.OriginalContractNo;
                    dgDetail[colRemark.Name, i].Value = master.Descript;
                    dgDetail[colSignDate.Name, i].Value = master.CreateDate.ToShortDateString();
                    dgDetail[colRealOperationDate.Name, i].Value = master.RealOperationDate;
                    dgDetail.Rows[i].Tag = master;
                }
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
      //双击事件
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string billId = (dgDetail.Rows[e.RowIndex].Tag as MaterialRentalOrderMaster).Id.ToString();
                if (ClientUtil.ToString(billId) != "")
                {
                    VMaterialRentalOrder vOrder = new VMaterialRentalOrder();
                    vOrder.Start(billId);
                    vOrder.ShowDialog();
                }
            }
        }
    }
}
