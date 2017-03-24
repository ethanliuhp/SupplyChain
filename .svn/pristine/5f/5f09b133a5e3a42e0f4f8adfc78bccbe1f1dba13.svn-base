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
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI
{
    public partial class VUpdateStock : TBasicDataView
    {
        private MStockMng model = new MStockMng();
        private string sType = string.Empty;
         
        public VUpdateStock()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.txtProjectName.Text  = StaticMethod.GetProjectInfo().Name;
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSelectProject.Click += new EventHandler(btnSelectProject_Click);
            btnSave .Click +=new EventHandler(btnSave_Click);

        }
        void btnSelectProject_Click(object sender, EventArgs e)
        {
            string strdepart = "";
            if (txtProjectName.Text != "")
            {
                strdepart = ClientUtil.ToString(txtProjectName.Text);
            }
            VDepartSelector vmros = new VDepartSelector(strdepart);
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            //string strName = ClientUtil.ToString(list[0]);
            CurrentProjectInfo cpi = list[0] as CurrentProjectInfo;
            txtProjectName.Text = cpi.Name;
            //cboProjectName.Tag = cpi.Id;
        }


        public bool Check()
        {
            if (string.IsNullOrEmpty(txtProjectName.Text.Trim()))
            {
                MessageBox.Show("请选择项目");
                this.txtProjectName.Focus();
                return false;
            }
            if (this.cboType.SelectedIndex < 0 || this.cboType.SelectedIndex >= this.cboType.Items.Count)
            {
                MessageBox.Show("请选单据类型");
                this.cboType.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtCode.Text.Trim()))
            {
                MessageBox.Show("请输入单据号");
                this.txtCode.Focus();
                return false;
            }
            return true;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {

            if (Check())
            {
                //入库单 出库单 验收单
                this.dgDetail.Rows.Clear();
                sType = this.cboType.SelectedItem.ToString();
                DataSet oDataSet = null;
                switch (this.cboType.SelectedItem.ToString())
                {

                    case "入库单":
                        {
                            MStockMng model = new MStockMng();
                            oDataSet = model.StockInSrv.QueryListStockInNotHS(txtProjectName.Text.Trim(), txtCode.Text.Trim());
                            break;
                        }

                    case "出库单":
                        {
                            MStockMng model = new MStockMng();
                            oDataSet = model.StockOutSrv.QueryListStockOutNotHS(txtProjectName.Text.Trim(), txtCode.Text.Trim());
                            break;
                        }
                    case "验收单":
                        {
                            MStockMng model = new MStockMng();
                            oDataSet = model.StockInSrv.QueryListStockInBalNotHS(txtProjectName.Text.Trim(), txtCode.Text.Trim());
                            break;
                        }
                    default:
                        break;
                }
                if (oDataSet != null && oDataSet.Tables[0] != null && oDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < oDataSet.Tables[0].Rows.Count; i++)
                    {
                        //t.id ,t.code,t.summoney,t.sumquantity,t.createpersonname,t.realoperationdate,to_char(t.createdate,'YYYY-MM-DD')createdate,t.descript ,t.projectid,t.projectname
                        this.dgDetail.Rows.Insert(this.dgDetail.Rows.Count == 0 ? 0 : this.dgDetail.Rows.Count - 1, 1);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colCode.Name].Value = ClientUtil.ToString(oDataSet.Tables[0].Rows[i]["code"]);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colCreateDate.Name].Value = ClientUtil.ToDateTime(oDataSet.Tables[0].Rows[i]["createdate"]).ToShortDateString();
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Tag = ClientUtil.ToDateTime(oDataSet.Tables[0].Rows[i]["createdate"]).ToShortDateString();
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colCreatePerson.Name].Value = ClientUtil.ToString(oDataSet.Tables[0].Rows[i]["createpersonname"]);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colDescript.Name].Value = ClientUtil.ToString(oDataSet.Tables[0].Rows[i]["descript"]);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colMoney.Name].Value = ClientUtil.ToString(oDataSet.Tables[0].Rows[i]["summoney"]);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colSumQuantity.Name].Value = ClientUtil.ToString(oDataSet.Tables[0].Rows[i]["sumquantity"]);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colRealOperation.Name].Value = ClientUtil.ToDateTime(oDataSet.Tables[0].Rows[i]["realoperationdate"]).ToShortDateString ();
                        string sMsg = IsAccount(ClientUtil.ToDateTime(oDataSet.Tables[0].Rows[i]["createdate"]), oDataSet.Tables[0].Rows[i]["projectid"].ToString());
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colCreateDate.Name].ReadOnly = !string.IsNullOrEmpty(sMsg);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colProjectName.Name].Value = ClientUtil.ToString(oDataSet.Tables[0].Rows[i]["projectname"]);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colID.Name].Value = ClientUtil.ToString (oDataSet.Tables[0].Rows[i]["id"]);
                        this.dgDetail.Rows[this.dgDetail.Rows.Count - 1].Cells[colProjectID.Name].Value = ClientUtil.ToString(oDataSet.Tables[0].Rows[i]["projectid"]);
                    }
                }
            }
        }
        
        public bool Valid(DateTime CreateDate, string sProjectID)
        {
            
            string sMsg = IsAccount(CreateDate, sProjectID);
            if (!string.IsNullOrEmpty(sMsg))
            {
                MessageBox.Show(sMsg);
                return false;
            }
            if (CreateDate > ConstObject.TheLogin.TheComponentPeriod.EndDate)
            {
                MessageBox.Show("当前业务日期[" + CreateDate.ToString() + "]已超出当前会计期的结束日期[" + ConstObject.TheLogin.TheComponentPeriod.EndDate.ToShortDateString() + "]！");
                return false;
            }
            return true;
        }
        public string IsAccount(DateTime CreateDate, string sProjectID)
        {
            MStockMng model = new MStockMng();
            string sMsg = model.StockInSrv.CheckAccounted(ConstObject.TheLogin.TheAccountOrgInfo, CreateDate, sProjectID);
            return sMsg;
        }
        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex] == this.colCreateDate)
            {
                string sProjectID = this.dgDetail.Rows[e.RowIndex].Cells[colProjectID.Name].Value.ToString();
                DateTime date = ClientUtil.ToDateTime(this.dgDetail.Rows[e.RowIndex].Cells[colCreateDate.Name].Value.ToString());
                DateTime oldDate = ClientUtil.ToDateTime(this.dgDetail.Rows[e.RowIndex].Tag.ToString());
                if (oldDate != date)
                {
                    
                    
                    if (!Valid(date, sProjectID))
                    {
                        this.dgDetail.Rows[e.RowIndex].Cells[colCreateDate.Name].Selected = true;
                    }
                }
            }
        }
        public void btnSave_Click(object sender, EventArgs e)
        {
            string sID = string.Empty;
            DateTime time=DateTime .Now ;
            string sDescript = string.Empty;
            int iYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
            int iMonth = ConstObject.TheLogin.TheComponentPeriod.NextMonth ;
            MStockMng model = new MStockMng();
            DataGridViewRow  oRow = dgDetail.CurrentRow;
            if (oRow != null)
            {
            }
             if(oRow !=null && !oRow .IsNewRow )
             {
                 sID = oRow.Cells[colID.Name].Value.ToString();
                  time = ClientUtil.ToDateTime(oRow.Cells[colCreateDate.Name].Value);

               
                sDescript=ClientUtil.ToString(oRow.Cells[colDescript.Name].Value);
                string sProjectID = ClientUtil.ToString(oRow.Cells[colProjectID.Name].Value);
                if (!string.Equals(oRow.Tag.ToString(), time.ToShortDateString()))
                {
                    if (Valid(time, sProjectID))
                    {
                    }
                    else
                    {
                        oRow.Cells[colCreateDate.Name].Selected = true;
                        return;
                    }
                }
                DataTable oTable = model.StockInSrv.GetFiscaDate(time);
                if (oTable != null && oTable.Rows.Count > 0)
                {
                   iYear=int.Parse(oTable.Rows[0]["year"].ToString());
                  iMonth=int.Parse(oTable.Rows[0]["month"].ToString());
                }
            }
             if (!string.IsNullOrEmpty(sID))
            {
                try
                {
                    switch (sType)
                    {

                        case "入库单":
                            {
                                model.StockInSrv.UpdateStockInNotHS(sID , time , sDescript , iYear , iMonth );
                               
                                break;
                            }
                        case "出库单":
                            {
                                model.StockOutSrv.UpdateStockOutNotHS(sID, time, sDescript, iYear, iMonth);
                               
                                break;
                            }
                        case "验收单":
                            {
                                model.StockInSrv.UpdateStockInBalNotHS(sID , time , sDescript , iYear , iMonth );
                               
                                break;
                            }
                        default:
                            break;
                    }
                    MessageBox.Show("保存成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存异常:" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("保存失败!没有数据");
            }
        }
    }
}
