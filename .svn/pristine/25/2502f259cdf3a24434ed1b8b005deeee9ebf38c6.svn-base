using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VSharingProportion : TBasicDataView
    {
        private IList resultList;

        public IList ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }

        private IList spaList = new ArrayList();
        private decimal planAllAmount = 0;
        public VSharingProportion(IList list,decimal num)
        {
            InitializeComponent();
            spaList = list;
            planAllAmount = num;
            InitEvent();
            InitData();
        }
        void InitData()
        {
            foreach (SharingProjectsAmountValueObj spa in spaList)
            {
                int rowIndex = dgSharingProjectAmount.Rows.Add();
                dgSharingProjectAmount[SharingLeafNodePath.Name, rowIndex].Value = spa.LeafNodePath;
                dgSharingProjectAmount[SharingProportion.Name, rowIndex].Value = 0;
                dgSharingProjectAmount.Rows[rowIndex].Tag = spa;
            }
        }
        void InitEvent()
        {
            rdoAverage.CheckedChanged += new EventHandler(rdoAverage_CheckedChanged);
            //rdoByProportion.CheckedChanged += new EventHandler(rdoByProportion_CheckedChanged);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
            dgSharingProjectAmount.CellEndEdit += new DataGridViewCellEventHandler(dgSharingProjectAmount_CellEndEdit);
        }

        void dgSharingProjectAmount_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgSharingProjectAmount.Rows[e.RowIndex].Cells[SharingProportion.Name].ColumnIndex)
            {
                try
                {
                    decimal proportion = Convert.ToDecimal(dgSharingProjectAmount.Rows[e.RowIndex].Cells[SharingProportion.Name].Value.ToString());
                    if (proportion < 0)
                    {
                        MessageBox.Show("请输入大于等于零的比例！");
                        dgSharingProjectAmount.CurrentCell = dgSharingProjectAmount[e.ColumnIndex, e.RowIndex];
                        dgSharingProjectAmount[e.ColumnIndex, e.RowIndex].Value = 0;
                        return;
                    }
                    if (proportion > 100)
                    {
                        MessageBox.Show("请输入小于100的比例！");
                        dgSharingProjectAmount.CurrentCell = dgSharingProjectAmount[e.ColumnIndex, e.RowIndex];
                        dgSharingProjectAmount[e.ColumnIndex, e.RowIndex].Value = 0;
                        return;
                    }
                    SharingProjectsAmountValueObj spa = dgSharingProjectAmount.Rows[e.RowIndex].Tag as SharingProjectsAmountValueObj;
                    
                     decimal   theSharingSummaryAmount = planAllAmount * proportion /(decimal)100;
                     spa.TheSharingSummaryAmount = theSharingSummaryAmount;
                    dgSharingProjectAmount.Rows[e.RowIndex].Tag = spa;
                }
                catch (Exception)
                {
                    MessageBox.Show("输入的数据有误！请重新输入！");
                    dgSharingProjectAmount.CurrentCell = dgSharingProjectAmount[e.ColumnIndex, e.RowIndex];
                    dgSharingProjectAmount[e.ColumnIndex, e.RowIndex].Value = 0;
                }
            }
        }
        //选择平摊
        void rdoAverage_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAverage.Checked)
            {
                decimal proportion = (decimal)100 / (decimal)dgSharingProjectAmount.Rows.Count;
                decimal proportionNum = 0;//留最后一个求之前的和
                for (int i = 0; i < dgSharingProjectAmount.Rows.Count; i++)
                {
                    decimal num = 0;
                    if (i != dgSharingProjectAmount.Rows.Count - 1)
                    {
                        num = decimal.Round(proportion, 2);
                        //dgSharingProjectAmount.Rows[i].Cells[SharingProportion.Name].Value = num;
                        proportionNum += num;
                    }
                    else
                    {
                        num = 100 - proportionNum;
                        //dgSharingProjectAmount.Rows[i].Cells[SharingProportion.Name].Value = num;
                    }
                    dgSharingProjectAmount.Rows[i].Cells[SharingProportion.Name].Value = num;
                    SharingProjectsAmountValueObj spa = dgSharingProjectAmount.Rows[i].Tag as SharingProjectsAmountValueObj;
                    spa.TheSharingSummaryAmount = planAllAmount * num / (decimal)100;
                    dgSharingProjectAmount.Rows[i].Tag = spa;
                }
            }
            else
            {
                
            }
        }

        
        void btnOK_Click(object sender, EventArgs e)
        {
            if (!Valide()) return;
            IList list = new ArrayList();
            foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
            {
                SharingProjectsAmountValueObj spa = row.Tag as SharingProjectsAmountValueObj;
                list.Add(spa);
            }
            resultList = list;
            this.Close();
        }

        void btnQuit_Click(object sender, EventArgs e)
        {
            resultList = null;
            this.Close();
        }

        bool Valide()
        {
            decimal proportion = 0;
            try
            {
                foreach (DataGridViewRow row in dgSharingProjectAmount.Rows)
                {
                    proportion += Convert.ToDecimal(row.Cells[SharingProportion.Name].Value.ToString());
                }
                if (proportion < 100)
                {
                    MessageBox.Show("所有分摊比例之和应为100！");
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("输入数据有误！");
                return false;
            }
            
        }
        
    }
}
