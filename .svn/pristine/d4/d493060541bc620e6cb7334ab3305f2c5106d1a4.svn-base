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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng
{
    public partial class VMaterialSelector : TBasicDataView
    {
        MMatRentalMng model = new MMatRentalMng();
        private MaterialRentalSettlementDetail curBillDetail;
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public MaterialRentalSettlementDetail CurBillDetail
        {
            get { return curBillDetail; }
            set { curBillDetail = value; }
        }

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VMaterialSelector()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            //添加下拉列表框的信息
            //VBasicDataOptr.InitWokerType(colWokerType, false);
            VBasicDataOptr.InitMaterialRental(colCostName, false);

            string[] locks = new string[] { colSettleSubject.Name, colMaterialCode.Name, colQuantity.Name, colQuantityUnit.Name, colRentalDate.Name, colDateUnit.Name, colSettleMoney.Name };
            dgDetail.SetColumnsReadOnly(locks);
        }
        private void dgDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //给指定列的下拉框添加SelectedIndexChanged事件
            if (dgDetail.CurrentCell.RowIndex != -1 && dgDetail.CurrentCell.ColumnIndex == 1)
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(VMaterialSelector_SelectedIndexChanged);
        }

        private void InitEvent()
        {
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            Load += new EventHandler(VMaterialSelector_Load);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);

        }
        //选择信息的时候添加主表信息
        void VMaterialSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgDetail.CurrentRow.Cells[colDateUnit.Name].Value = curBillDetail.DateUnitName;
            dgDetail.CurrentRow.Cells[colDateUnit.Name].Tag = curBillDetail.DateUnit as StandardUnit;
            dgDetail.CurrentRow.Cells[colQuantity.Name].Value = curBillDetail.Quantity;
            dgDetail.CurrentRow.Cells[colRentalDate.Name].Value = curBillDetail.SettleDate;
            dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = curBillDetail.QuantityUnitName;
            dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = curBillDetail.QuantityUnit as StandardUnit;
            dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value = curBillDetail.MaterialName;//物资名称
            dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = curBillDetail.MaterialResource as Material;
        }

        void VMaterialSelector_Load(object sender, EventArgs e)
        {
            if (Result != null && Result.Count > 0)
            {
                foreach (MaterialSubjectDetail obj in Result)
                {
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail.Rows[rowIndex].Tag = obj;
                    dgDetail[colCostName.Name, rowIndex].Value = obj.CostName;
                    dgDetail[colDateUnit.Name, rowIndex].Value = obj.DateUnitName;
                    dgDetail[colDateUnit.Name, rowIndex].Tag = obj.DateUnit;
                    dgDetail[colDescription.Name, rowIndex].Value = obj.Descript;
                    dgDetail[colQuantity.Name, rowIndex].Value = obj.SettleQuantity;
                    dgDetail[colQuantityUnit.Name, rowIndex].Tag = obj.QuantityUnit;
                    dgDetail[colQuantityUnit.Name, rowIndex].Value = obj.QuantityUnitName;
                    dgDetail[colRentalDate.Name, rowIndex].Value = obj.SettleDate;
                    dgDetail[colRentalPrice.Name, rowIndex].Value = obj.SettlePrice;
                    dgDetail[colSettleMoney.Name, rowIndex].Value = obj.SettleMoney;
                    dgDetail[colMaterialCode.Name, rowIndex].Value = obj.MaterialTypeName;
                    dgDetail[colMaterialCode.Name, rowIndex].Tag = obj.MaterialType;
                    dgDetail[colSettleSubject.Name, rowIndex].Tag = obj.SettleSubject;
                    dgDetail[colSettleSubject.Name, rowIndex].Value = obj.SettleSubjectName;
                }
            }
            else
            {
                if (CurBillDetail != null && curBillDetail.Id != null)
                {
                    foreach (MaterialSubjectDetail obj in CurBillDetail.MaterialSubjectDetails)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        dgDetail.Rows[rowIndex].Tag = obj;
                        //dgDetail[col.Name, rowIndex].Value = obj.PeopleNum;
                        //dgDetail[colWokerType.Name, rowIndex].Value = obj.WorkerType;
                        dgDetail[colCostName.Name, rowIndex].Value = obj.CostName;
                        dgDetail[colDateUnit.Name, rowIndex].Value = obj.DateUnitName;
                        dgDetail[colDateUnit.Name, rowIndex].Tag = obj.DateUnit;
                        dgDetail[colDescription.Name, rowIndex].Value = obj.Descript;
                        dgDetail[colQuantity.Name, rowIndex].Value = obj.SettleQuantity;
                        dgDetail[colQuantityUnit.Name, rowIndex].Tag = obj.QuantityUnit;
                        dgDetail[colQuantityUnit.Name, rowIndex].Value = obj.QuantityUnitName;
                        dgDetail[colRentalDate.Name, rowIndex].Value = obj.SettleDate;
                        dgDetail[colRentalPrice.Name, rowIndex].Value = obj.SettlePrice;
                        dgDetail[colSettleMoney.Name, rowIndex].Value = obj.SettleMoney;
                        dgDetail[colMaterialCode.Name, rowIndex].Value = obj.MaterialTypeName;
                        dgDetail[colMaterialCode.Name, rowIndex].Tag = obj.MaterialType;
                        dgDetail[colSettleSubject.Name, rowIndex].Tag = obj.SettleSubject;
                        dgDetail[colSettleSubject.Name, rowIndex].Value = obj.SettleSubjectName;
                    }
                }
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnOK.Focus();
        }

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 1)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow drt in dgDetail.Rows)
                    {
                        if (drt.IsNewRow) break;

                        DataGridViewRow dr = dgDetail.CurrentRow;
                        if (this.result == null)
                        {
                            dgDetail.Rows.Clear();
                        }
                        else
                        {
                            this.result.Clear();
                            dgDetail.Rows.Clear();
                        }

                    }
                }
            }
            //this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 1)
            {
                if (MessageBox.Show("关闭前要保存信息吗？", "保存信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    this.result.Clear();
                    foreach (DataGridViewRow var in dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        MaterialSubjectDetail dtl = var.Tag as MaterialSubjectDetail;
                        if (dtl == null)
                        {
                            dtl = new MaterialSubjectDetail();
                        }

                        dtl.CostName = ClientUtil.ToString(var.Cells[colCostName.Name].Value);
                        dtl.DateUnit = var.Cells[colDateUnit.Name].Tag as StandardUnit;
                        dtl.DateUnitName = ClientUtil.ToString(var.Cells[colDateUnit.Name].Value);
                        dtl.Descript = ClientUtil.ToString(var.Cells[colDescription.Name].Value);
                        dtl.MaterialType = var.Cells[colMaterialCode.Name].Tag as Material;
                        dtl.MaterialTypeName = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                        dtl.SettleMoney = ClientUtil.ToDecimal(var.Cells[colSettleMoney.Name].Value);
                        dtl.SettlePrice = ClientUtil.ToDecimal(var.Cells[colRentalPrice.Name].Value);
                        dtl.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);
                        dtl.QuantityUnit = var.Cells[colQuantity.Name].Tag as StandardUnit;
                        dtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);
                        dtl.SettleDate = ClientUtil.ToDecimal(var.Cells[colRentalDate.Name].Value);
                        dtl.SettleSubjectName = ClientUtil.ToString(var.Cells[colSettleSubject.Name].Value);
                        if (dtl.MaterialResource != null)
                        {
                            dtl.MaterialSpec = dtl.MaterialType.Specification;
                            dtl.MaterialStuff = dtl.MaterialType.Stuff;
                        }

                        result.Add(dtl);

                    }
                }
            }
            this.btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            if (this.dgDetail.SelectedRows.Count != 0)
            {
                //校验
                foreach (DataGridViewRow dr in this.dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    if (dr.Cells[colCostName.Name].Value == null)
                    {
                        MessageBox.Show("费用名称不允许为空！");
                        dgDetail.CurrentCell = dr.Cells[colCostName.Name];
                        return;
                    }

                    if (dr.Cells[colSettleSubject.Name].Tag == null)
                    {
                        MessageBox.Show("结算科目不允许为空！");
                        dgDetail.CurrentCell = dr.Cells[colSettleSubject.Name];
                        return;
                    }
                }
                //费用不能重复
                for (int i = 0; i < dgDetail.Rows.Count - 2; i++)
                {
                    //int j = dgDetail.Rows.Count - 2;
                    for (int j = dgDetail.Rows.Count - 3; j > i; j--)
                    {
                        if (dgDetail[1, i].Value.Equals(dgDetail[1, j].Value))
                        {
                            MessageBox.Show("费用信息不可重复！");
                            return;
                        }
                    }
                }
                string strUnit = "元";
                Application.Resource.MaterialResource.Domain.StandardUnit Unit = null;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Name", strUnit));
                IList lists = model.MatMngSrv.GetDomainByCondition(typeof(Application.Resource.MaterialResource.Domain.StandardUnit), oq);
                if (lists != null && lists.Count > 0)
                {
                    Unit = lists[0] as Application.Resource.MaterialResource.Domain.StandardUnit;
                }
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    MaterialSubjectDetail dtl = var.Tag as MaterialSubjectDetail;
                    if (dtl == null)
                    {
                        dtl = new MaterialSubjectDetail();
                    }
                    dtl.CostName = ClientUtil.ToString(var.Cells[colCostName.Name].Value);
                    dtl.DateUnit = var.Cells[colDateUnit.Name].Tag as StandardUnit;
                    dtl.DateUnitName = ClientUtil.ToString(var.Cells[colDateUnit.Name].Value);
                    dtl.Descript = ClientUtil.ToString(var.Cells[colDescription.Name].Value);
                    dtl.MaterialType = var.Cells[colMaterialCode.Name].Tag as Material;
                    dtl.MaterialTypeName = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);
                    dtl.SettleMoney = ClientUtil.ToDecimal(var.Cells[colSettleMoney.Name].Value);
                    dtl.SettlePrice = ClientUtil.ToDecimal(var.Cells[colRentalPrice.Name].Value);
                    //价格单位默认为元
                    dtl.PriceUnit = Unit;
                    dtl.PriceUnitName = Unit.Name;
                    dtl.Quantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);
                    dtl.SettleQuantity = ClientUtil.ToDecimal(var.Cells[colQuantity.Name].Value);
                    dtl.QuantityUnit = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;
                    dtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);
                    dtl.SettleDate = ClientUtil.ToDecimal(var.Cells[colRentalDate.Name].Value);
                    dtl.SettleSubject = var.Cells[colSettleSubject.Name].Tag as CostAccountSubject;
                    dtl.SettleSubjectName = ClientUtil.ToString(var.Cells[colSettleSubject.Name].Value);
                    dtl.SettleSubjectSyscode = dtl.SettleSubject.SysCode;
                    result.Add(dtl);
                }
            }
            this.btnOK.FindForm().Close();
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            bool flag = true;
            if (colName == colQuantity.Name)//数量
            {
                string quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value);
                if (quantity == null)
                {
                    quantity = "0";
                    return;
                }
                validity = CommonMethod.VeryValid(quantity);
                if (validity == false)
                {
                    MessageBox.Show("数量为数字！");
                    dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value = "";
                    flag = false;
                }
            }
            if (colName == colCostName.Name)
            {
                btnCancel.Focus();
                Hashtable hashtableSubject = new Hashtable();//成本核算科目
                CostAccountSubject Subject = null;
                ObjectQuery oqSub = new ObjectQuery();
                IList list1 = model.MatMngSrv.GetDomainByCondition(typeof(CostAccountSubject), oqSub);
                if (list1 != null && list1.Count > 0)
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        Subject = list1[i] as CostAccountSubject;
                        hashtableSubject.Add(Subject, Subject.Code);
                    }
                }
                string strType = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value);
                string strCost = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colCostName.Name].Value);
                if (strType != "" && strCost != "")
                {
                    string strSubjectCode = null;
                    if (strType.Equals("固定塔式起重机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030101";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030102";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030103";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030104";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030105";
                        }
                    }
                    if (strType.Equals("门式起重机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030101";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030102";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030103";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030104";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030105";
                        }
                    }
                    if (strType.Equals("施工升降机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030201";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030202";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030203";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030204";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030205";
                        }
                    }
                    if (strType.Equals("汽车式起重机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030301";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030302";
                        }
                    }
                    if (strType.Equals("井架提升机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030401";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030402";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030403";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030404";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030405";
                        }
                    }
                    if (strSubjectCode != null)
                    {
                        CostAccountSubject SubjectGUID = new CostAccountSubject();
                        foreach (System.Collections.DictionaryEntry objName in hashtableSubject)
                        {
                            if (objName.Value.ToString().Equals(strSubjectCode))
                            {
                                SubjectGUID = (CostAccountSubject)objName.Key;
                                dgDetail.CurrentRow.Cells[colSettleSubject.Name].Tag = SubjectGUID;
                                dgDetail.CurrentRow.Cells[colSettleSubject.Name].Value = SubjectGUID.Name;
                                break;
                            }
                        }
                    }
                }
            }
            if (colName == colRentalDate.Name)//租赁时间
            {
                string quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colRentalDate.Name].Value);
                if (quantity == null)
                {
                    quantity = "0";
                    return;
                }
                validity = CommonMethod.VeryValid(quantity);
                if (validity == false)
                {
                    MessageBox.Show("租赁时间为数字！");
                    dgDetail.Rows[e.RowIndex].Cells[colRentalDate.Name].Value = "";
                    flag = false;
                }
            }
            if (colName == colRentalPrice.Name)//租赁单价
            {
                string quantity = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colRentalPrice.Name].Value);
                if (quantity == null)
                {
                    quantity = "0";
                    return;
                }
                validity = CommonMethod.VeryValid(quantity);
                if (validity == false)
                {
                    MessageBox.Show("租赁单价为数字！");
                    dgDetail.Rows[e.RowIndex].Cells[colRentalPrice.Name].Value = "";
                    flag = false;
                }
            }
            if (flag)
            {
                decimal summoney = 0;
                for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                {
                    decimal quantity = 0;
                    decimal money = 0;
                    decimal time = 0;
                    decimal price = 0;
                    quantity = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colQuantity.Name].Value);
                    time = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colRentalDate.Name].Value);
                    price = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colRentalPrice.Name].Value);
                    if (quantity != 0 && time != 0 && price != 0)
                    {
                        if (dgDetail.Rows[i].Cells[colDateUnit.Name].Value == null || dgDetail.Rows[i].Cells[colDateUnit.Name].Value == "")
                        {
                            return;
                        }
                        //当前单价即为当前单位对应的单价，无需做转换处理【如：除以30】
                        //if(dgDetail.Rows[i].Cells[colDateUnit.Name].Value.Equals("月"))
                        {
                            money = Math.Round(quantity * time * price, 2);
                            summoney += money;
                        }
                        //if(dgDetail.Rows[i].Cells[colDateUnit.Name].Value.Equals("天"))
                        //{
                        //    money = Math.Round(quantity * time / 30 * price, 2);
                        //    summoney += money;
                        //}
                    }
                    dgDetail.Rows[i].Cells[colSettleMoney.Name].Value = ClientUtil.ToString(money);
                }
                curBillDetail.SettleMoney = summoney;
            }
        }

        private void dgDetail_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    DataGridViewRow dr = dgDetail.CurrentRow;
                    if (dr == null || dr.IsNewRow) return;
                    dgDetail.Rows.Remove(dr);
                    if (dr.Tag != null)
                    {
                        curBillDetail.MaterialSubjectDetails.Remove(dr.Tag as MaterialSubjectDetail);
                    }
                }
            }
        }
        void dgDetail_CellContentClick(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow.Cells[colSettleSubject.Name].Value == null && dgDetail.CurrentRow.Cells[colCostName.Name].Value != "")
            {
                Hashtable hashtableSubject = new Hashtable();//成本核算科目
                CostAccountSubject Subject = null;
                ObjectQuery oqSub = new ObjectQuery();
                IList list1 = model.MatMngSrv.GetDomainByCondition(typeof(CostAccountSubject), oqSub);
                if (list1 != null && list1.Count > 0)
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        Subject = list1[i] as CostAccountSubject;
                        hashtableSubject.Add(Subject, Subject.Code);
                    }
                }
                string strType = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colMaterialCode.Name].Value);
                string strCost = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colCostName.Name].Value);
                if (strType != "" && strCost != "")
                {
                    string strSubjectCode = null;
                    if (strType.Equals("固定塔式起重机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030101";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030102";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030103";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030104";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030105";
                        }
                    }
                    if (strType.Equals("门式起重机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030101";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030102";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030103";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030104";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030105";
                        }
                    }
                    if (strType.Equals("施工升降机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030201";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030202";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030203";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030204";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030205";
                        }
                    }
                    if (strType.Equals("汽车式起重机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030301";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030302";
                        }
                    }
                    if (strType.Equals("井架提升机"))
                    {
                        if (strCost.Equals("机械租赁费"))
                        {
                            strSubjectCode = "C511030401";
                        }
                        if (strCost.Equals("机械进出场及安拆费"))
                        {
                            strSubjectCode = "C511030402";
                        }
                        if (strCost.Equals("机械人工费"))
                        {
                            strSubjectCode = "C511030403";
                        }
                        if (strCost.Equals("基础预埋费"))
                        {
                            strSubjectCode = "C511030404";
                        }
                        if (strCost.Equals("设备基础费"))
                        {
                            strSubjectCode = "C511030405";
                        }
                    }
                    if (strSubjectCode != "")
                    {
                        CostAccountSubject SubjectGUID = new CostAccountSubject();
                        foreach (System.Collections.DictionaryEntry objName in hashtableSubject)
                        {
                            if (objName.Value.ToString().Equals(strSubjectCode))
                            {
                                SubjectGUID = (CostAccountSubject)objName.Key;
                                dgDetail.CurrentRow.Cells[colSettleSubject.Name].Tag = SubjectGUID;
                                dgDetail.CurrentRow.Cells[colSettleSubject.Name].Value = SubjectGUID.Name;
                                break;
                            }
                        }
                    }
                }
            }

        }
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSettleSubject.Name))
                {
                    if (dgDetail.CurrentRow.Cells[colSettleSubject.Name].Value == "" || dgDetail.CurrentRow.Cells[colSettleSubject.Name].Value == null)
                    {
                        VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                        frm.ShowDialog();
                        CostAccountSubject cost = frm.SelectAccountSubject;
                        if (cost != null)
                        {
                            this.dgDetail.CurrentRow.Cells[colSettleSubject.Name].Tag = cost;
                            this.dgDetail.CurrentRow.Cells[colSettleSubject.Name].Value = cost.Name;
                            this.btnCancel.Focus();
                        }
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colQuantityUnit.Name))
                {
                    //双击数量单位
                    StandardUnit su1 = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su1 != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = su1;
                        this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = su1.Name;
                    }
                }
                if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colDateUnit.Name))
                {
                    //双击数量单位
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {
                        this.dgDetail.CurrentRow.Cells[colDateUnit.Name].Tag = su;
                        this.dgDetail.CurrentRow.Cells[colDateUnit.Name].Value = su.Name;
                        decimal quantity = 0;
                        decimal money = 0;
                        decimal time = 0;
                        decimal price = 0;
                        quantity = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colQuantity.Name].Value);
                        time = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colRentalDate.Name].Value);
                        price = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colRentalPrice.Name].Value);
                        if (quantity != 0 && time != 0 && price != 0)
                        {
                            //当前单价即为当前单位对应的单价，无需做转换处理【如：除以30】
                            //if(dgDetail.CurrentRow.Cells[colDateUnit.Name].Value.Equals("月"))
                            {
                                money = Math.Round(quantity * time * price, 2);
                            }
                            //if(dgDetail.CurrentRow.Cells[colDateUnit.Name].Value.Equals("天"))
                            //{
                            //    money = Math.Round(quantity * time / 30 * price, 2);
                            //}
                        }
                        dgDetail.CurrentRow.Cells[colSettleMoney.Name].Value = ClientUtil.ToString(money);

                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}
