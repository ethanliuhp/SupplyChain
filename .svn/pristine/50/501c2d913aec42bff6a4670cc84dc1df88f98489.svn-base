using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using System.Globalization;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.WeightBill
{
    public partial class VWeightBillSelecter : Form
    {
        private MStockMng model = new MStockMng();
        private IList<WeightBillDetail> lstQueryResult = null;
        private List<WeightBillDetail> lstSelectResult = null;
        private bool _IsSingleSelect = true;
        private CurrentProjectInfo projectInfo = null;
        private string _MaterialCode = null;
        private string _SupplyCode = null;
        /// <summary>
        /// 获取选择的结果
        /// </summary>
        public List<WeightBillDetail> SelectResult
        {
            get { return lstSelectResult; }
            set { lstSelectResult = value; }
        }
        public string MaterialCode
        {
            get { return _MaterialCode; }
            set { _MaterialCode = value; }
        }
        public string SupplyCode
        {
            get { return _SupplyCode; }
            set { _SupplyCode = value; }
        }
        public bool SingleSelect
        {
            get { return this._IsSingleSelect; }
            set { }
        }
        public VWeightBillSelecter()
        {
            InitializeComponent();
            IntialEvent();
            IntialData();
        }
        public VWeightBillSelecter(bool _IsSingleSelect, string _MaterialCode, string _SupplyCode):this()
        {
            this._IsSingleSelect = _IsSingleSelect;
            this._MaterialCode = _MaterialCode;
            this._SupplyCode = _SupplyCode;
        }
        public void IntialEvent()
        {
            this.btnSure.Click+=new EventHandler(btnSure_Click);
            this.btnCancel.Click+=new EventHandler(btnCancel_Click);
            this.btnSearch.Click+=new EventHandler(btnSearch_Click);
            this.dgDetail.CellClick+=new DataGridViewCellEventHandler(dgDetail_CellClick);
        }
        public void IntialData()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.lblProjectCode.Visible=this.txtProjectCode.Visible=(projectInfo != null && projectInfo.Code == CommonUtil.CompanyProjectCode);
            this.beginTime.Value = DateTime.Now.AddDays(-7);
            this.endTime.Value = DateTime.Now;
        }
        private void dgDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgDetail.Columns[e.ColumnIndex] == colSelect)
            {
                foreach (DataGridViewRow oRow in dgDetail.Rows)
                {
                    if (oRow != dgDetail.Rows[e.RowIndex])
                    {
                        oRow.Cells[colSelect.Name].Value = false;
                    }
                    else
                    {
                        oRow.Cells[colSelect.Name].Value = oRow.Cells[colSelect.Name].Value == null ? true : !((bool)oRow.Cells[colSelect.Name].Value);
                    }
                }
            }
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.beginTime.Value < this.endTime.Value)
                {
                    FlashScreen.Show("正在查询过磅单数据,请稍好.....");
                    lstQueryResult = model.StockInSrv.QueryWeightBill(projectInfo.Code, this.beginTime.Value, this.endTime.Value,this.SupplyCode,this.MaterialCode);
                    DataBind(lstQueryResult);
                }
                else
                {
                    throw new Exception("[查询异常]:开始时间应该小于结束时间");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException==null?ex.Message:ex.InnerException.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        public void DataBind(IList<WeightBillDetail> lstQueryResult)
        {
            int iRowIndex = 0;
            this.dgDetail.Rows.Clear();
            DataGridViewRow oRow = null;
            if (lstQueryResult != null && lstQueryResult.Count > 0)
            {
                foreach (WeightBillDetail oDetail in lstQueryResult)
                {
                    iRowIndex = dgDetail.Rows.Add();
                    oRow = dgDetail.Rows[iRowIndex];
                    oRow.Cells[colCMSJ.Name].Value = DateTime.ParseExact(oDetail.Master.CMSJ, "yyyyMMddHHmmss", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss");//ClientUtil.ToDateTime( oDetail.Master.CMSJ);
                    oRow.Cells[colCLBM.Name].Value = oDetail.CLBM;
                    oRow.Cells[colCLMC.Name].Value = oDetail.CLMC;
                    oRow.Cells[colCPH.Name].Value = oDetail.Master.CPH;
                    oRow.Cells[colDJLY.Name].Value = oDetail.Master.DJLY;
                    oRow.Cells[colDWGC.Name].Value = oDetail.Master.DWGC;
                    oRow.Cells[colGGXH.Name].Value = oDetail.GGXH;
                    oRow.Cells[colGYSMC.Name].Value = oDetail.Master.GYSMC;
                    oRow.Cells[colJLDW.Name].Value = oDetail.JLDW;
                    oRow.Cells[colLX.Name].Value = oDetail.Master.LX;
                    oRow.Cells[colLX_YT.Name].Value = oDetail.Master.LX_YT;
                    oRow.Cells[colProjectCode.Name].Value = oDetail.Master.projectCode;
                    oRow.Cells[colSJSL.Name].Value = oDetail.SJSL;
                    oRow.Cells[colSYBW.Name].Value = oDetail.Master.SYBW;
                    oRow.Cells[colGBY.Name].Value = oDetail.Master.GBY;
                    oRow.Tag = oDetail;
                }
            }
        }
        public void btnCancel_Click(object sender, EventArgs e)
        {
            SelectResult = null;
            this.Close();
        }
        public void btnSure_Click(object sender, EventArgs e)
        {
            SelectResult = new List<WeightBillDetail>();
 
            foreach (DataGridViewRow oRow in dgDetail.Rows)
            {
                if (ClientUtil.ToBool(oRow.Cells[this.colSelect.Name].Value))
                {
                    SelectResult.Add(oRow.Tag as WeightBillDetail);
                    if (this.SingleSelect) { break; }
                }
            }
            if (SelectResult.Count > 0)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择过磅材料");
            }
        }

       
    }
}
