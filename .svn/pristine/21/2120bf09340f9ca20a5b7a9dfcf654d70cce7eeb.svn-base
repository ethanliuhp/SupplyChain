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
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public partial class VSetBillProperty : TBasicDataView
    {
        private MAppPlatform Model = new MAppPlatform();
        public const  string Bill_JZJLD = "浇筑记录单";
        public const string Bill_SPTDZD = "商品砼对账单";
        public const string Bill_FJWZSQD = "废旧物资申请单";
        public string sBillPersonName = string.Empty;
        public string sBillCode = string.Empty;
        public string sQuerySql = string.Empty;
        public string sUpdateSql = string.Empty;
        public VSetBillProperty()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            //txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            ////pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            ////专业分类
            //VBasicDataOptr.InitProfessionCategory(cboProfessionalCategory, true);

            IList list= Model.Service.GetAllProject();
            this.cmbProject.Items.Clear();
            cmbProject.DataSource = list;
            cmbProject.DisplayMember = "Name";
            cmbProject.ValueMember = "Id";
            if (cmbProject.Items.Count > 0)
            {
                cmbProject.SelectedIndex = 0;
            }

            cmbBillName.Items.Clear();
            cmbBillName.Items.Add(Bill_JZJLD);
            cmbBillName.Items.Add(Bill_SPTDZD);
            cmbBillName.Items.Add(Bill_FJWZSQD);
            cmbBillName.SelectedIndex = 0;

            cstBillState.Items.Clear();
            cstBillState.Items.Add ("提交");
            cstBillState.Items.Add ("编辑");

        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }
        public void btnQuery_Click(object sender, EventArgs e)
        {
            switch (this.cmbBillName .SelectedItem + "")
            {
                case Bill_JZJLD:
                    sQuerySql = "select t.id, t.state,t.code,t.createdate,t.createpersonname from THD_POURINGNOTEMASTER t where  not exists(select 1 from thd_pouringnotedetail t1 where  t.id=t1.parentid  and t1.concretecheckid is not null ) and t.state=5   and t.projectid='{0}' and t.createdate between to_date('{1}','YYYY-MM-DD') and  to_date('{2}','YYYY-MM-DD') ";
                     sBillPersonName = " t.createpersonname ";
                     sBillCode = " t.code ";
                    sUpdateSql = "update    THD_POURINGNOTEMASTER t set t.state={0} ,t.createdate=to_date('{1}','YYYY-MM-DD') where t.id='{2}'";
                    break;
                case Bill_SPTDZD:
                    sQuerySql = "select t.id, t.state,t.code,t.createdate,t.createpersonname from THD_CONCRETECHECKINGMASTER t where  not exists(select 1 from THD_CONCRETECHECKINGDETAIL t1 where  t.id=t1.parentid  and (t1.refquantity !=0 and t1.refquantity is not null) ) and t.state=5   and t.projectid='{0}' and t.createdate between to_date('{1}','YYYY-MM-DD') and  to_date('{2}','YYYY-MM-DD') ";
                    sBillPersonName = " t.createpersonname ";
                    sBillCode = " t.code ";
                    sUpdateSql = "update    THD_CONCRETECHECKINGMASTER t set t.state={0} ,t.createdate=to_date('{1}','YYYY-MM-DD') where t.id='{2}'";
                    break;
                case Bill_FJWZSQD:
                    sQuerySql = "select t.id, t.state,t.code,t.createdate,t.createpersonname from THD_WASTEMATAPPLYMASTER t where  not exists(select 1 from THD_WASTEMATAPPLYDETAIL t1 join THD_WASTEMATPROCESSDETAIL t2 on t1.id=t2.forwarddetailid where t1.parentid=t.id ) and t.state=5   and t.projectid='{0}' and t.createdate between to_date('{1}','YYYY-MM-DD') and  to_date('{2}','YYYY-MM-DD') ";
                    sBillPersonName = " t.createpersonname ";
                    sBillCode = " t.code ";
                    sUpdateSql = "update    THD_WASTEMATAPPLYMASTER t set t.state={0} ,t.createdate=to_date('{1}','YYYY-MM-DD') where t.id='{2}'";
                    break;
                   
                default :
                    sQuerySql = "";
                    sUpdateSql = "";
                    return ;

                   
            }
            if (!string.IsNullOrEmpty(sQuerySql))
            {
                sQuerySql = string.Format(sQuerySql, cmbProject.SelectedValue + "", this.dtpDateBeginBill.Value.ToShortDateString(), this.dtpDateEndBill.Value.ToShortDateString());
                if (!string.IsNullOrEmpty(this.txtCreatePersonBill.Text.Trim()))
                {

                    sQuerySql += string.Format(" and {1} like '%{0}%'  ", this.txtCreatePersonBill.Text.Trim(), sBillPersonName);
                }
                if (!string.IsNullOrEmpty(this.txtCodeBeginBill.Text.Trim()))
                {

                    sQuerySql += string.Format(" and {1} like '%{0}%'  ", this.txtCodeBeginBill.Text.Trim(), sBillCode);
                }
                DataTable   tb = Model.Service.GetSetBillData(sQuerySql);
                int iRow = 0;
                this.dgDetail.Rows.Clear();
                if (tb != null && tb.Rows .Count >0)
                {
                    foreach (DataRow oRow in tb.Rows)
                    {
                        iRow = this.dgDetail.Rows.Add();
                        this.dgDetail.Rows[iRow].Cells[colID.Name].Value = ClientUtil.ToString(oRow["id"]);
                        this.dgDetail.Rows[iRow].Cells[colState.Name].Value = ClientUtil.ToInt(oRow["state"])==5?"提交":"";
                        this.dgDetail.Rows[iRow].Cells[colCode.Name].Value = ClientUtil.ToString(oRow["code"]);
                        this.dgDetail.Rows[iRow].Cells[colCreatePerson.Name].Value = ClientUtil.ToString(oRow["createpersonname"]);
                        this.dgDetail.Rows[iRow].Cells[colCreateTime.Name].Value = ClientUtil.ToString(oRow["createdate"]);
                    }
                }
                if (this.dgDetail.Rows.Count > 0)
                {
                    this.dgDetail.Rows[0].Selected = true;
                    this.dgDetail_SelectionChanged(this.dgDetail, null);
                }
            }
        }
        private void InitEvent()
        {
           this.dgDetail .SelectionChanged +=new EventHandler(dgDetail_SelectionChanged);
           this.btnSearch.Click += new EventHandler(btnQuery_Click);
            this.btnSetBill .Click +=new EventHandler(btnSetBill_Click);
        }
        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count == 0) return;
            if (dgDetail.SelectedRows.Count == 0) return;

            DataGridViewRow thdCurrentRow = dgDetail.SelectedRows[0];
            if (thdCurrentRow != null)
            {
                string sCreateDate =ClientUtil.ToString( thdCurrentRow.Cells[colCreateTime.Name ].Value);
                string sState =ClientUtil.ToString(  thdCurrentRow.Cells[ colState.Name ].Value);
                this.cstTime.Value =string.IsNullOrEmpty(sCreateDate)?DateTime.Now:ClientUtil.ToDateTime (sCreateDate);
                cstBillState.SelectedItem = sState;

            }
        }
        public void btnSetBill_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sUpdateSql))
            {
                   if (dgDetail.Rows.Count == 0) return;
            if (dgDetail.SelectedRows.Count == 0) return;
DataGridViewRow thdCurrentRow = dgDetail.SelectedRows[0];
                string sID=ClientUtil.ToString (thdCurrentRow.Cells [colID.Name ].Value );
                int iState=string.Equals (cstBillState.SelectedItem +"","提交")?5:0;
                string sDate=this.cstTime.Value .ToShortDateString ();

                string sUpdate = string.Format(sUpdateSql, iState, sDate, sID);
                if (Model.Service.SetBillData(sUpdate))
                {
                    MessageBox.Show("修改成功");
                    thdCurrentRow.Cells[colCreateTime.Name ].Value = sDate;
                    thdCurrentRow.Cells[colState.Name].Value = iState == 5 ? "提交" : "编辑";
                }
                else
                {
                    MessageBox.Show("修改失败");
                }
            }
        }

        
         
 

         
        
    }
}
