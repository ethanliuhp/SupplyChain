using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using NHibernate;
 

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{

    public partial class VEndAccountAudit : TBasicDataView
    {
        IRPServiceModel.Services.Common.ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        public MSubContractBalance subContractOperate = new MSubContractBalance();
        private CurrentProjectInfo projectInfo; 
        private EndAccountAuditBill curBillMaster;
        private string[] arrBalBaseReadOnly = { "计时工", "代工", "罚款", "分包签证" };
        public   string flag  ;//生成或者审核的判断标志

        public VEndAccountAudit(string sflag)
        {

            flag = sflag;
            InitializeComponent();
            InitEvents();
            InitData(); 
            if (flag =="生成")
            {
                dgvBillDetail.Columns["审减工程量"].Visible = false;
                dgvBillDetail.Columns["审减单价"].Visible = false;
                dgvBillDetail.Columns["审减金额"].Visible = false;
                dgvBillDetail.Columns["审核意见"].Visible = false;
                btnSave.Visible = false;
            }

        }

        private void InitEvents()
        {
            dgvBills.AutoGenerateColumns = false;
            btnCreate.Click += btnCreate_Click;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            dgvBills.AutoGenerateColumns = false;
            dgvBills.CellClick += new DataGridViewCellEventHandler(dgvBills_CellClick);
            dgvBills.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dgvBills_RowPostPaint);
            dgvBills.CellContentClick += new DataGridViewCellEventHandler(dgvBills_CellContentClick);
            btnSave.Click += new EventHandler(btnSave_Click);
            dgvBillDetail.CellEndEdit += new DataGridViewCellEventHandler(dgvBillDetail_CellEndEdit);
     
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvBills.SelectedRows.Count <= 0)
            {
                return;
            }
            if (SaveOrSubmit(curBillMaster))
            {
                MessageBox.Show("审核成功");
                dgvBillDetail.Refresh();
            }
        }
       
        private bool SaveOrSubmit(EndAccountAuditBill bill)
        {
           
            bill.DocState = DocumentState.InExecute;
            subContractOperate.SubBalSrv.SaveOrUpdateEndAccountAuditBill(bill);
            LoadCreatedBills();
            return true;
        }


        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            GetWbsRootNode();
            LoadCreatedBills();
        }

        void dgvBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgvBills[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colBillCode.Name)
            {

              if(flag == "审核")
              {
                EndAccountAuditBill bill = dgvBills.Rows[e.RowIndex].DataBoundItem as EndAccountAuditBill;
                if (bill == null)
                {
                    return;
                }
                if (bill.DocState != DocumentState.InExecute)
                {
                    MessageBox.Show("终结分包结算单[" + bill.Code + "]未审核完，不能打印！");
                    return;
                }
                VEndAccountAuditReport frm = new VEndAccountAuditReport(bill);   
                frm.ShowDialog();
               }

            }
        }


        private void GetWbsRootNode()
        {
            if (projectInfo == null)
            {
                return;
            }
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("ParentNode", null));
            var list = subContractOperate.ObjectQuery(typeof(GWBSTree), objectQuery).OfType<GWBSTree>().ToList();
            if (list == null || list.Count == 0)
            {
                txtRootNode.Clear();
                txtRootNode.Tag = null;
            }
            else
            {
                txtRootNode.Text = list[0].Name;
                txtRootNode.Tag = list[0];
            }
        }

        private void LoadCreatedBills()
        {
            #region  
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            var list = subContractOperate.ObjectQuery(typeof(EndAccountAuditBill), oq).OfType<EndAccountAuditBill>().ToList();
            dgvBills.DataSource = list.OrderBy(a => a.SubContractUnitName).ThenBy(b => b.Code).ToArray();
            dgvBillDetail.Rows.Clear();
            #endregion
        }

        private void dgvBillDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                object tempValue = dgvBillDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                {
                    value = tempValue.ToString().Trim();
                }

                EndAccountAuditDetail dtl = dgvBillDetail.Rows[e.RowIndex].Tag as EndAccountAuditDetail;
                if (dgvBillDetail.Columns[e.ColumnIndex].Name == "审减工程量")//审减工程量
                {
                    dtl.SJGCL = ClientUtil.ToString(value);       
                }
                else if (dgvBillDetail.Columns[e.ColumnIndex].Name == "审减单价")//审减单价
                {
                    dtl.SJDJ = ClientUtil.ToString(value);     
                }
                else if (dgvBillDetail.Columns[e.ColumnIndex].Name == "审减金额")//审减金额
                {
                    dtl.SJJE = ClientUtil.ToString(value);
                }
                else if (dgvBillDetail.Columns[e.ColumnIndex].Name == "审核意见")//审核意见
                {
                    dtl.SHYJ = ClientUtil.ToString(value);
                }
                dgvBillDetail.Rows[e.RowIndex].Tag = dtl;

            }

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
 
            #region   实现生成分包终结结算单，插入数据
            //第一步：得到符合条件的数据
            MSubContractBalance MSub = new MSubContractBalance();
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSubContractProject.TheContractGroup", NHibernate.FetchMode.Eager);
            IList IListSub = MSub.ObjectQuery(typeof(SubContractBalanceBill), objectQuery);
            #region 第二步：汇总，插入数据 注意：把相同的分包单位的不同的数据汇总成一条数据,然后再插入到数据库当中
             var query = from l in IListSub.OfType<SubContractBalanceBill>()
                         group l by  new
                        {
                            l.SubContractUnitName,
                            l.SubContractUnitGUID
                            //l.TheSubContractProject
                            
                        } into g
                         select new
                         {
                             
                             SubContractUnitName = g.Key.SubContractUnitName,
                             SubContractUnitGUID = g.Key.SubContractUnitGUID,
                             //TheSubContractProject = g.Key.TheSubContractProject,
                             SumCumulativeMoney = g.Sum(l => l.CumulativeMoney),
                             SumBalanceMoney = g.Sum(l => l.BalanceMoney)
                         };
            //取分组汇总之后的数据 
             DateTime time = subContractOperate.GetServerTime();//取服务器的时间，以便生成批次号  
            //生成之前，直接根据项目ID清除主表以及明细表中的所有信息，以免重复生成
             ObjectQuery objectQueryEnd = new ObjectQuery();
             objectQueryEnd.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
             //objectQueryEnd.AddCriterion(Expression.Eq("CreatePerson.Id", ConstObject.LoginPersonInfo.Id));//汇总当前登录人所创建的
             IList IListSubEnd = MSub.ObjectQuery(typeof(EndAccountAuditBill), objectQuery);
             if (IListSubEnd.Count > 0) //当前主表当中已经存在数据，如果存在就给提示：当前批次号已经存在，是否先删除，再生成？
             {
                 if (MessageBox.Show("当前项目的终结分包单已经存在，是否先删除，再生成？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                 {
                     return;
                 }
                 else
                 {
                     foreach (EndAccountAuditBill bl in IListSubEnd)
                     {
                         MSub.DeleteEndAccountAuditBill(bl);
                     }
                 }
             }

             FlashScreen.Show("正在生成分包终结结算单，请稍候...");
             foreach (var  SC in query)
             {
                 EndAccountAuditBill Bill = new EndAccountAuditBill();
                 //分包汇总取到的值
                 Bill.SubContractUnitName = SC.SubContractUnitName;
                 Bill.SubContractUnitGUID = SC.SubContractUnitGUID;
                 Bill.CumulativeMoney = SC.SumCumulativeMoney;
                 Bill.BalanceMoney = SC.SumBalanceMoney;
                 //其它的值 
                 Bill.Code = string.Format("{0:yyyyMMddHHmmss}", time);
                 Bill.DocState = DocumentState.Edit;
                 Bill.CreateDate = ClientUtil.ToDateTime(time.ToShortDateString());
                 Bill.CreatePerson = ConstObject.LoginPersonInfo;
                 Bill.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                 Bill.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                 Bill.CreateYear = ConstObject.LoginDate.Year;
                 Bill.CreateMonth = ConstObject.LoginDate.Month;
                 Bill.ProjectId = projectInfo.Id;
                 Bill.ProjectName = projectInfo.Name;
                 Bill.OperOrgInfo = ConstObject.TheOperationOrg;
                 Bill.OperOrgInfoName = ConstObject.TheOperationOrg.Name;
                 Bill.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                 Bill.BalanceRange = (txtRootNode.Tag as GWBSTree);
                 Bill.AuditPerson = ConstObject.LoginPersonInfo;
                 Bill.AuditPersonName = ConstObject.LoginPersonInfo.Name;
                 Bill.HandlePerson = ConstObject.LoginPersonInfo;
                 Bill.CreateBatchNo = string.Format("{0:yyyyMMddHHmmss}", time);

                 //通过查结果集赋值，取所属分包项目
                 foreach (SubContractBalanceBill sub in IListSub)//例如有1个分包单位，以前的主表中显示的三条，现在取第一个
                 {
                     if(sub.SubContractUnitGUID == SC.SubContractUnitGUID)
                     {
                         Bill.TheSubContractProject = sub.TheSubContractProject;
                         break;
                     }

                 }
                 #region  BALANCEBASE为判断标记,位于明细表当中
                 var billList = from b in IListSub.OfType<SubContractBalanceBill>()
                                where b.SubContractUnitGUID == SC.SubContractUnitGUID
                                select b;

                 var detais = new List<SubContractBalanceDetail>();
                 foreach (var bl in billList)
                 {
                     detais.AddRange(bl.Details.OfType<SubContractBalanceDetail>());//得到分包单位下的所有明细，放入集合当中
                 } 
               
                 #region 添加明细 
                 foreach (var dt in detais)
                 {
                     EndAccountAuditDetail endAccDetail = new EndAccountAuditDetail();
                     //汇总字段赋值
                     endAccDetail.BalancePrice = dt.BalancePrice;
                     endAccDetail.BalanceTotalPrice = dt.BalanceTotalPrice;
                     endAccDetail.BalacneQuantity = dt.BalacneQuantity;

                     //其它字段赋值
                     endAccDetail.BalanceBase = dt.BalanceBase;
                     endAccDetail.BalanceTask = dt.BalanceTask;
                     endAccDetail.BalanceTaskDtl = dt.BalanceTaskDtl;
                     endAccDetail.BalanceTaskDtlName = dt.BalanceTaskDtlName;
                     endAccDetail.BalanceTaskName = dt.BalanceTaskName;
                     endAccDetail.BalanceTaskSyscode = dt.BalanceTaskSyscode;
                     endAccDetail.ConfirmQuantity = dt.ConfirmQuantity;
                     endAccDetail.Descript = dt.Descript;
                     endAccDetail.DiagramNumber = dt.DiagramNumber;
                     endAccDetail.FontBillType = dt.FontBillType;
                     endAccDetail.ForwardDetailId = dt.ForwardDetailId;
                     endAccDetail.FrontBillGUID = dt.FrontBillGUID;
                     endAccDetail.HandlePerson = dt.HandlePerson;
                     endAccDetail.HandlePersonName = dt.HandlePersonName;
                     endAccDetail.Master = Bill;
                     endAccDetail.MaterialCode = dt.MaterialCode;
                     endAccDetail.MaterialName = dt.MaterialName;
                     endAccDetail.MaterialResource = dt.MaterialResource;
                     endAccDetail.MaterialSpec = dt.MaterialSpec;
                     endAccDetail.MaterialStuff = dt.MaterialStuff;
                     endAccDetail.MaterialSysCode = dt.MaterialSysCode;
                     endAccDetail.MatStandardUnit = dt.MatStandardUnit;
                     endAccDetail.MatStandardUnitName = dt.MatStandardUnitName;
                     endAccDetail.Money = dt.Money;
                     endAccDetail.Price = dt.Price;
                     endAccDetail.PriceUnit = dt.PriceUnit;
                     endAccDetail.PriceUnitName = dt.PriceUnitName;
                     endAccDetail.Quantity = dt.Quantity;
                     endAccDetail.QuantityUnit = dt.QuantityUnit;
                     endAccDetail.QuantityUnitName = dt.QuantityUnitName;
                     endAccDetail.RefQuantity = dt.RefQuantity;
                     endAccDetail.Remarks = dt.Remarks;

                     Bill.Details.Add(endAccDetail);

                 }

                 #endregion

                 #endregion
                 MSub.SaveOrUpdateEndAccountAuditBill(Bill);  
             }
             #endregion  
             FlashScreen.Close();
            LoadCreatedBills();//生成完毕，直接显示在界面上
            #endregion

        }
       
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBills.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的分包单位");
                return;
            }
     
            DataGridViewRow dgvr = dgvBills.SelectedRows[0];
            string code = dgvr.Cells["Column3"].Value.ToString();//获取分包单位 

            if (MessageBox.Show("您确认要删除分包单位【" + code + "】？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            FlashScreen.Show("数据删除中，请稍候...");
            #region  删除数据
            MSubContractBalance MSub = new MSubContractBalance();
            var bill = dgvBills.SelectedRows[0].DataBoundItem as EndAccountAuditBill;
            MSub.DeleteEndAccountAuditBill(bill);
            #endregion
            LoadCreatedBills();
            FlashScreen.Close();

        }

      
        private void dgvBills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvBillDetail.Rows.Clear();
            EndAccountAuditBill master = dgvBills.Rows[e.RowIndex].DataBoundItem as EndAccountAuditBill;
            if (master == null)
            {
                return;
            }
            curBillMaster = master;
            btnSave.Enabled = master.DocState == DocumentState.Edit;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            oq.AddFetchMode("BalanceTaskDtl", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("BalanceTaskDtl.TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList subContractBalanceDetailList = subContractOperate.ObjectQuery(typeof(EndAccountAuditDetail), oq);

            master.Details.Clear();
            var totalMoney = 0m;
            //分包终结结算明细
            foreach (EndAccountAuditDetail detail in subContractBalanceDetailList)
            {
                master.Details.Add(detail);

                int rowIndex = dgvBillDetail.Rows.Add();
                dgvBillDetail.Rows[rowIndex].Tag = detail;
                dgvBillDetail[this.colDTaskNode.Name, rowIndex].Value = detail.BalanceTaskName;
                dgvBillDetail[this.colDTaskMx.Name, rowIndex].Value = detail.BalanceTaskDtlName;
                dgvBillDetail[this.colBalMoney.Name, rowIndex].Value = detail.BalanceTotalPrice;
                dgvBillDetail[this.colBalPrice.Name, rowIndex].Value = detail.BalancePrice;
                dgvBillDetail[this.colBalQty.Name, rowIndex].Value = detail.BalacneQuantity;
                dgvBillDetail[this.colForwardType.Name, rowIndex].Value = detail.FontBillType.ToString();
                dgvBillDetail[this.DtlHandlePerson.Name, rowIndex].Value = detail.HandlePersonName;
                dgvBillDetail[this.colQtyUnit.Name, rowIndex].Value = detail.QuantityUnitName;
                dgvBillDetail[this.colPriceUnit.Name, rowIndex].Value = detail.PriceUnitName;
                dgvBillDetail[this.colUseDescript.Name, rowIndex].Value = detail.Remarks;//备注

                if (detail.BalanceTaskDtl != null && detail.BalanceTaskDtl.Id != "0")
                {
                    dgvBillDetail[this.colMainResourceName.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeName;
                    dgvBillDetail[this.colMainResourceSpec.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeSpec;
                    dgvBillDetail[this.colDigramNumber.Name, rowIndex].Value = detail.BalanceTaskDtl.DiagramNumber;
                    if (detail.BalanceTaskDtl.TheCostItem != null)
                        dgvBillDetail[this.colCostItemQuotaCode.Name, rowIndex].Value = detail.BalanceTaskDtl.TheCostItem.QuotaCode;
                }

                dgvBillDetail[colBalanceBase.Name, rowIndex].Value = detail.BalanceBase;
                totalMoney += detail.BalanceTotalPrice;

                dgvBillDetail["审减工程量", rowIndex].Value = detail.SJGCL;//审减工程量
                dgvBillDetail["审减单价", rowIndex].Value = detail.SJDJ;//审减单价
                dgvBillDetail["审减金额", rowIndex].Value = detail.SJJE;//审减金额
                dgvBillDetail["审核意见", rowIndex].Value = detail.SHYJ;//审核意见
            }
            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].Style.ForeColor = Color.Black;
            dgvBills.SelectedRows[0].Cells[colBillBalanceMoney.Name].ToolTipText = string.Empty;
        }

        private void dgvBills_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            var bill = dgvBills.Rows[e.RowIndex].DataBoundItem as EndAccountAuditBill;
            if (bill != null)
            {
                dgvBills.Rows[e.RowIndex].Cells[colBillState.Name].Value = ClientUtil.GetDocStateName(bill.DocState);

                if (bill.DocState != DocumentState.Edit)
                {
                    dgvBills.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    dgvBills.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Green;
                }
            }
        }
      
    }

}
