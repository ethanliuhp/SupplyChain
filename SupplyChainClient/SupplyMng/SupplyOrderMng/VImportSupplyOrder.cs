using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.Util;
using Application.Resource.MaterialResource.Domain;
using NHibernate.Criterion;
 
using System.Collections;
 
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;



namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng
{
    public partial class VImportSupplyOrder : Form
    {
        
        public Hashtable  htResult = null;
      
 
        public int iMaxCount = 500;
        public IList<string> lstColumnName = new List<string>();
        public int RowCount = 0;
        public VImportSupplyOrder( int RowCount)
        {
            this.RowCount = RowCount;
            InitializeComponent();
            lstColumnName.Add("物资编码");
            lstColumnName.Add("物资名称");
            lstColumnName.Add("规格型号");
            lstColumnName.Add("图号");
            lstColumnName.Add("技术参数");
            lstColumnName.Add("品牌");
            lstColumnName.Add("计量单位");
            lstColumnName.Add("数量");
            lstColumnName.Add("采购单价");
            lstColumnName.Add("采购金额");
            lstColumnName.Add("备注");
            htResult = new Hashtable();
            InitalFlexCell();
            IntialEvent();
        }


        public void InitalFlexCell()
        {

            flexGrid.Rows = RowCount+1;
            flexGrid.Cols = lstColumnName.Count +1;
            flexGrid.DisplayRowNumber = true;
            for (int i = 0; i < lstColumnName.Count; i++)
            {
                FlexCell.Cell oCell = flexGrid.Cell(0, i + 1);
                oCell.Text = lstColumnName[i];
            }
            for (int i = 0; i < lstColumnName.Count; i++)
            {
                flexGrid.Column(i + 1).Width = 120;
            }

        }
        public void IntialEvent()
        {
           btnAdd .Click+=new EventHandler(btnAdd_Click);
            btnDelete .Click +=new EventHandler(btnDelete_Click);
            btnInsertRows .Click +=new EventHandler(btnInsertRows_Click);
            btnSure .Click +=new EventHandler(btnSure_Click);
            btnCancel1.Click += new EventHandler(btnCancel);
        }
     
        public void btnInsertRows_Click(object sender, System.EventArgs e)
        {
            int iRowCount = 0;
            try
            {
                iRowCount = int.Parse(txtRowCount.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.txtRowCount.Focus();
                return;
            }
            if (iRowCount <= 0)
            {
                MessageBox.Show("无法添加;[添加的行数大于零]");
                this.txtRowCount.Focus();
                return;
            }
            if (flexGrid.Rows == iMaxCount + 1)
            {
                MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,已经添加了{0}条。]", iMaxCount));
                this.txtRowCount.Focus();
                return;
            }
            if (flexGrid.Rows + iRowCount > iMaxCount + 1)
            {
                MessageBox.Show(string.Format("无法添加:[此列表只能批量添加{0}条记录,此次只能添加{1}条记录。]", iMaxCount, iMaxCount + 1 - flexGrid.Rows));
                this.txtRowCount.Focus();
                return;
            }
            for (int i = 0; i < iRowCount; i++)
            {
                btnAddRow();
            }
        }
       
        public void btnCancel(object sender, System.EventArgs e)
        {
            this.Close();
        }
        public void btnAddRow()
        {
            flexGrid.AutoRedraw = false;

            flexGrid.InsertRow(flexGrid.Rows, 1);
            flexGrid.Rows += 1;
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        public void btnDeleteRow(int iRow)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.RemoveItem(iRow);
            //flexGrid.Rows -= 1;

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        public void btnDelete_Click(object sender, System.EventArgs e)
        {
            btnDeleteRow(flexGrid.Selection.FirstRow);
        }
        public void btnAdd_Click(object sender, System.EventArgs e)
        {
            btnAddRow();
        }
        public bool  IsEmpty(int iRow)
        {
            bool bFlag = true ;
            if (flexGrid.Cols > 1)
            {
                for (int i = 1; i < flexGrid.Cols; i++)
                {
                    if (!string.IsNullOrEmpty(flexGrid.Cell(iRow, i).Text.Trim()))
                    {
                        bFlag = false;
                        break;
                    }
                }
            }
            
            return bFlag;
        }
        public void btnSure_Click(object sender, System.EventArgs e)
        {
            htResult.Clear();
            SupplyOrderDetail oSupplyOrderDetail = null;
            string sValue = string.Empty;
            string sError = string.Empty;
            try
            {
                for (int i = 1; i < flexGrid.Rows; i++)
                {
                    oSupplyOrderDetail = new SupplyOrderDetail();
                    lstColumnName.Add("物资编码");
                    lstColumnName.Add("物资名称");
                    lstColumnName.Add("规格型号");
                    lstColumnName.Add("图号");
                    lstColumnName.Add("技术参数");
                    lstColumnName.Add("品牌");
                    lstColumnName.Add("计量单位");
                    lstColumnName.Add("数量");
                    lstColumnName.Add("采购单价");
                    lstColumnName.Add("采购金额");
                    lstColumnName.Add("备注");
                    sValue = flexGrid.Cell(i, 1).Text.Trim();
                    if (string.IsNullOrEmpty(sValue)   )
                    {
                        if (IsEmpty(i))
                        {
                            continue;
                        }
                        else
                        {
                            sError = string.Format("物资编码不能为空:第【{0}】行的物资编号为空！", i);
                            throw new Exception(sError);
                        }
                    }
                    else
                    {
                        if (htResult.ContainsKey(sValue))
                        {
                            sError = string.Format("物资编码不能重复:第【{0}】行的物资编号重复！", i);
                            throw new Exception(sError);
                        }
                        else{
                        oSupplyOrderDetail.MaterialCode = sValue;//物资编码
                        sValue = flexGrid.Cell(i, 2).Text.Trim();
                        oSupplyOrderDetail.MaterialName = sValue;//物资名称
                        sValue = flexGrid.Cell(i, 3).Text.Trim();
                        oSupplyOrderDetail.MaterialSpec = sValue;//规格型号
                        sValue = flexGrid.Cell(i, 4).Text.Trim();
                        oSupplyOrderDetail.DiagramNumber = sValue;//图号
                        sValue = flexGrid.Cell(i, 5).Text.Trim();
                        oSupplyOrderDetail.TechnologyParameter = sValue;//技术参数

                        sValue = flexGrid.Cell(i, 6).Text.Trim();
                        oSupplyOrderDetail.Brand = sValue;//品牌
                        sValue = flexGrid.Cell(i, 7).Text.Trim();
                        oSupplyOrderDetail.MatStandardUnitName = sValue;//计量单位
                        sValue = flexGrid.Cell(i,8).Text.Trim();
                        if (!ClientUtil.IsNumberByVm(sValue))
                        {
                            oSupplyOrderDetail.Quantity = 0;//数量
                            //sError = string.Format("数量不能为空或者非数字类型:第【{0}】行的为空或者非数字类型！", i);
                            //throw new Exception(sError);
                        }
                        else
                        {
                            oSupplyOrderDetail.Quantity = ClientUtil.ToDecimal(sValue);
                        }
                        sValue = flexGrid.Cell(i, 9).Text.Trim();//采购单价
                        if (!ClientUtil.IsNumberByVm(sValue))
                        {
                            oSupplyOrderDetail.Price = 0;
                            //sError = string.Format("数量不能为空或者非数字类型:第【{0}】行的为空或者非数字类型！", i);
                            //throw new Exception(sError);
                        }
                        else
                        {
                            oSupplyOrderDetail.Price  = ClientUtil.ToDecimal(sValue);
                        }
                        oSupplyOrderDetail.Money = oSupplyOrderDetail.Quantity * oSupplyOrderDetail.Price;//采购金额
                        sValue = flexGrid.Cell(i, 10).Text.Trim();
                        oSupplyOrderDetail.Descript = sValue;//备注
                        htResult.Add(oSupplyOrderDetail.MaterialCode, oSupplyOrderDetail);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(sError);
                htResult.Clear();
                return;
            }
            this.Close();

        }


    }
}
