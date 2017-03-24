using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinControls.Controls;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public class TempColumn
    {
      public   string ColumnName;
      public int index;
     
    }
    public partial class VCostMonthCompareReportSetColumn : Form
    {
        private CustomFlexGrid flexGrid;
        private int iStartRowIndex; 
       private  int iStartColumnIndex;
       List<TempColumn> lstTempColumn = new List<TempColumn>();
      
        public VCostMonthCompareReportSetColumn(CustomFlexGrid flexGrid, int iStartRowIndex,int iStartColumnIndex)
        {
            InitializeComponent();
            this.flexGrid = flexGrid;
            this.iStartRowIndex = iStartRowIndex;
            this.iStartColumnIndex = iStartColumnIndex;
           
            SetDataSource();
            InitalEvent();
        }
        public void InitalEvent()
        {
            this.checkList.ItemCheck+=new ItemCheckEventHandler(checkList_ItemCheck);
        }
        public void SetDataSource()
        {
            if (flexGrid != null && flexGrid.Cols > iStartColumnIndex && flexGrid.Rows > iStartRowIndex)
            {
                int iRow = 0;
                string sValue=string.Empty;
                checkList.Items.Clear();
                for (int i = iStartColumnIndex; i < flexGrid.Cols; i++)
                {
                    sValue=flexGrid.Cell(iStartRowIndex, i).Text;
                    checkList.Items.Add(sValue, flexGrid.Column(i).Visible);
                   // ht.Add(i, sValue);
                    lstTempColumn.Insert(lstTempColumn.Count, new TempColumn() { ColumnName = sValue , index=i});

                }
            }
        }

        private void checkList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
          
                    flexGrid.Column(lstTempColumn[e.Index].index).Visible =( e.NewValue == CheckState.Checked);
                    flexGrid.Refresh();
                
        }
        
       
    }
}
