using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Resource.MaterialResource.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI
{
    public partial class VStockSelectMaterial : TBasicDataView
    {
        public VStockSelectMaterial()
        {
            InitializeComponent();
        }
        private IList lstMatResult = new ArrayList();
         
        private MaterialCategory oMaterialCategory;
        private bool IsMoveOut = false;
        private MStockMng model = new MStockMng();
        private EnumStockExecType execType;
        private string Profession = string.Empty;
        public VStockSelectMaterial(MaterialCategory oMaterialCategory, EnumStockExecType execType,string sProfession,bool IsMoveOut)
        {
            InitializeComponent();
            this.oMaterialCategory = oMaterialCategory;
            this.execType = execType;
            this.IsMoveOut = IsMoveOut;
            this.Profession = sProfession;
            this.Text = (this.IsMoveOut ? "调拨出库" : "领料出库") + "(" + Enum.GetName(typeof(EnumStockExecType), this.execType) + ")";
            ColRemainQuality.HeaderText = this.IsMoveOut ? "闲置物质" : "库存量";
            this.ColMaterialDiagramNum.Visible = this.IsMoveOut;
            InitEvent();
        }
        /// <summary>
        /// 获取物资SelectRelationInfo 集合
        /// </summary>
        public IList MatResult
        {
            get { return lstMatResult; }
        }
        
        public void InitEvent()
        {
            btnSearch.Click += new EventHandler(BtnSearch_Click);
            this.chkAllSelect.CheckedChanged += new EventHandler(chkAllSelect_CheckedChanged);
            this.chkUnSelect .CheckedChanged +=new EventHandler(chkUnSelect_CheckedChanged);
            this.btnSure.Click +=new EventHandler(BtnSure_Click);
            this.btnClose .Click +=new EventHandler(btnClose_Click);
        }
        public void BtnSearch_Click(object sender, EventArgs e)
        {
            this.dgDetail.Rows.Clear();
            int iRow = 0;
            string sSpec = string.Empty;
            string sCode = string.Empty;
            string sName = string.Empty;
            sSpec = txtMatSpec.Text.Trim();
            sCode = txtMaterialCode.Text.Trim();
            sName = txtMatrailName.Text.Trim();
            DataSet ds = model.StockOutSrv.GetStockMatByUnit(StaticMethod.GetProjectInfo().Id, sSpec, sCode, sName, Enum.GetName(typeof(EnumStockExecType), execType),Profession, oMaterialCategory,this.IsMoveOut );
           if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow oRow in ds.Tables[0].Rows)
               {
                   SelectRelationInfo oSelectRelationInfo = new SelectRelationInfo();
                   //t.material,t.materialname,t.materialcode,t.materialspec,t.materialstuff,
                   //     t.matstandardunit,t.matstandardunitname,sum(t.quantity)quantity
                   oSelectRelationInfo.MaterialID = ClientUtil.ToString(oRow["material"]);
                   oSelectRelationInfo.MaterialName = ClientUtil.ToString(oRow["materialname"]);
                   oSelectRelationInfo.MaterialCode = ClientUtil.ToString(oRow["materialcode"]);
                   oSelectRelationInfo.MaterialQuantity = ClientUtil.ToDecimal(oRow["quantity"]);
                   oSelectRelationInfo.MaterialSpec = ClientUtil.ToString(oRow["materialspec"]);
                   oSelectRelationInfo.MaterialStuff = ClientUtil.ToString(oRow["materialstuff"]);
                   oSelectRelationInfo.MaterialUnit = ClientUtil.ToString(oRow["matstandardunit"]);
                   oSelectRelationInfo.MaterialUnitName = ClientUtil.ToString(oRow["matstandardunitname"]);
                   oSelectRelationInfo.MaterialDiagramNum = this.execType == EnumStockExecType.安装 ? ClientUtil.ToString(oRow["diagramnumber"]) : "";
                  iRow= this.dgDetail.Rows.Add();
                  this.dgDetail.Rows[iRow].Cells[colMaterialCode.Name].Value = oSelectRelationInfo.MaterialCode;
                  this.dgDetail.Rows[iRow].Cells[colMaterialCode.Name].Tag = oSelectRelationInfo.MaterialID;
                  this.dgDetail.Rows[iRow].Cells[colMaterialName .Name  ].Value = oSelectRelationInfo.MaterialName ;
                  this.dgDetail.Rows[iRow].Cells[colMaterialSpec .Name ].Value = oSelectRelationInfo.MaterialSpec ;
                  this.dgDetail.Rows[iRow].Cells[colMaterialStuff .Name ].Value = oSelectRelationInfo.MaterialStuff ;
                  this.dgDetail.Rows[iRow].Cells[colUnit .Name ].Value = oSelectRelationInfo.MaterialUnitName;
                  this.dgDetail.Rows[iRow].Cells[colUnit.Name].Tag  = oSelectRelationInfo.MaterialUnit ;
                  this.dgDetail.Rows[iRow].Cells[ColRemainQuality.Name].Value = oSelectRelationInfo.MaterialQuantity;
                  this.dgDetail.Rows[iRow].Cells[ColMaterialDiagramNum.Name].Value = oSelectRelationInfo.MaterialDiagramNum;
                  oSelectRelationInfo.material = null;
                  this.dgDetail.Rows[iRow].Tag = oSelectRelationInfo;
               }
           }
        }
        public void BtnSure_Click(object sender, EventArgs e)
        {
            this.lstMatResult.Clear();
            foreach (DataGridViewRow oRow in this.dgDetail.Rows)
            {
                if (oRow.Tag != null)
                {
                    if (ClientUtil.ToBool(oRow.Cells[colSelect.Name].Value))
                    {
                        SelectRelationInfo oSelectRelationInfo = oRow.Tag as SelectRelationInfo;
                        IList lst = model.StockOutSrv.GetMaterial(oSelectRelationInfo.MaterialID);
                        if (lst.Count > 0)
                        {
                            oSelectRelationInfo.material = lst[0] as Material;
                            lstMatResult.Add(oSelectRelationInfo);
                        }
                    }
                    
                }
            }
            if (lstMatResult.Count == 0)
            {
                MessageBox.Show("请选择物资");
            }
            else
            {
                this.Close();
            }
        }
        public void btnClose_Click(object sender, EventArgs e)
        {
            this.lstMatResult.Clear();
            this.Close();
        }
        public void chkAllSelect_CheckedChanged(object sender,  EventArgs e)
        {
           
                foreach (DataGridViewRow oRow in this.dgDetail.Rows)
                {
                    if (oRow.IsNewRow  || oRow .Tag ==null) break;
                    oRow.Cells[colSelect.Name].Value = this.chkAllSelect.Checked;
                }
            
        }
        public void chkUnSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkUnSelect.Checked)
            {
                foreach (DataGridViewRow oRow in this.dgDetail.Rows)
                {
                    if (oRow.IsNewRow || oRow.Tag == null) break;
                    oRow.Cells[colSelect.Name].Value =! ClientUtil.ToBool(oRow.Cells[colSelect.Name].Value);
                }
            }
        }
        private void pnlFloor_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    public class SelectRelationInfo
    {
        public string MaterialID;
        public string MaterialName;
        public string MaterialCode;
        public string MaterialSpec;
        public string MaterialStuff;
        public string MaterialUnit;
        public string MaterialUnitName;
        public decimal  MaterialQuantity;
        public Material material   ;
        public string MaterialDiagramNum;
    }
}
