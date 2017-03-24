using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng
{
    public partial class VImportExpensesSettle : Form
    {
        private MExpensesSettleMng model = new MExpensesSettleMng();
        public Hashtable htResult = null;

        public int iMaxCount = 500;
        public IList<string> lstColumnName = new List<string>();
        public int RowCount = 0;

        public VImportExpensesSettle()
        {
            InitializeComponent();
            lstColumnName.Add("成本核算科目");
            lstColumnName.Add("编码");
            lstColumnName.Add("金额");

            htResult = new Hashtable();
            InitalFlexCell();
            IntialEvent();
        }

        public void InitalFlexCell()
        {
            flexGrid.Rows = RowCount + 1;
            flexGrid.Cols = lstColumnName.Count + 1;
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
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnInsertRows.Click += new EventHandler(btnInsertRows_Click);
            btnSure.Click += new EventHandler(btnSure_Click);
            btnCancel1.Click += new EventHandler(btnCancel);
        }

        public void btnCancel(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public void btnAdd_Click(object sender, System.EventArgs e)
        {
            btnAddRow();
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

        public bool IsEmpty(int iRow)
        {
            bool bFlag = true;
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
            Hashtable hashtableSubject = new Hashtable();//成本核算科目
            CostAccountSubject Subject = null;
            ObjectQuery oqSub = new ObjectQuery();
            IList list1 = model.ExpensesSettleSrv.ObjectQuery(typeof(CostAccountSubject), oqSub);
            if (list1 != null && list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    Subject = list1[i] as CostAccountSubject;
                    hashtableSubject.Add(Subject, Subject.Code);
                }
            }
            string sValue = string.Empty;
            string sError = string.Empty;
            try
            {
                for (int i = 1; i < flexGrid.Rows; i++)
                {
                    lstColumnName.Add("成本核算科目");
                    lstColumnName.Add("编码");
                    lstColumnName.Add("金额");
                    sValue = flexGrid.Cell(i, 1).Text.Trim();
                    if (string.IsNullOrEmpty(sValue))
                    {
                        if (IsEmpty(i))
                        {
                            continue;
                        }
                        else
                        {
                            sError = string.Format("成本核算科目不能为空:第【{0}】行的成本核算科目为空！", i);
                            throw new Exception(sError);
                        }
                    }
                    else
                    {
                        string strSubjectName = flexGrid.Cell(i, 1).Text.Trim();
                        string strSubjectCode = flexGrid.Cell(i, 2).Text.Trim();
                        string strMoney = flexGrid.Cell(i, 3).Text.Trim();
                        CostAccountSubject SubjectGUID = new CostAccountSubject();
                        ExpensesSettleDetail expensesSettleDetail = new ExpensesSettleDetail();
                        if (strSubjectName != "" && strSubjectCode != "" && strMoney != "")
                        {
                            expensesSettleDetail.Money = ClientUtil.ToDecimal(strMoney);
                            if (strSubjectName != "")//科目不为空
                            {
                                foreach (System.Collections.DictionaryEntry objName in hashtableSubject)
                                {
                                    if (objName.Value.ToString().Equals(strSubjectCode))
                                    {
                                        SubjectGUID = (CostAccountSubject)objName.Key;
                                        expensesSettleDetail.AccountCostSysCode = SubjectGUID.SysCode;
                                        expensesSettleDetail.AccountCostName = SubjectGUID.Name;
                                        expensesSettleDetail.AccountCostSubject = SubjectGUID;
                                        break;
                                    }
                                }
                            }
                            if (expensesSettleDetail.AccountCostSubject != null)
                            {
                                htResult.Add(expensesSettleDetail, expensesSettleDetail.AccountCostName);
                            }
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
