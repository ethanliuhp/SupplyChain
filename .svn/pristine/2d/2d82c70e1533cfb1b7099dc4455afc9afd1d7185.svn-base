using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonForm;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VProgramManage : TBasicDataView
    {
        MProgramManage matmodel = new MProgramManage();
        ProgramReduceRate currRate = null;
        MaterialInterfacePrice currPrice = null;
        Hashtable query_ht = new Hashtable();

        public VProgramManage()
        {
            InitializeComponent();
            InitialEvents();
            InitData();
        }

        private void InitData()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", "1"));
            Search(oq);
            SearchRace(oq);
            dgProgramPrice.ContextMenuStrip = cmsDg;
            dgRate.ContextMenuStrip = cmsDg;
            cmsDg.Enabled = true;
        }

        private void InitialEvents()
        {
            this.dgProgramPrice.CellDoubleClick += new DataGridViewCellEventHandler(dgProgramPrice_CellDoubleClick);
            this.dgProgramPrice.CellEndEdit += new DataGridViewCellEventHandler(dgProgramPrice_CellEndEdit);
            this.dgRate.CellDoubleClick += new DataGridViewCellEventHandler(dgRate_CellDoubleClick);            
            btnSearch.Click +=new EventHandler(btnSearch_Click);
            btnSave.Click +=new EventHandler(btnSave_Click);
            this.btnSearchMat.Click +=new EventHandler(btnSearchMat_Click);
            this.btnProject.Click +=new EventHandler(btnProject_Click);
            btnSearchRate.Click +=new EventHandler(btnSearchRate_Click);
            btnSaveRate.Click += new EventHandler(btnSaveRate_Click);
            txtRate1.LostFocus +=new EventHandler(txtRate1_LostFocus);
            txtRate2.LostFocus +=new EventHandler(txtRate2_LostFocus);
      
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
        }
        void dgProgramPrice_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int iColumnIndex = e.ColumnIndex;
            int iRowIndex = e.RowIndex;
            DateTime dateBegin;
            DateTime dateEnd;
            if (string.Equals ( dgProgramPrice.Columns[iColumnIndex].Name  ,colDateBegin.Name ))
            {
                dateBegin = ClientUtil.ToDateTime(ClientUtil.ToDateTime(dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value).ToShortDateString ());
                dateEnd =ClientUtil.ToDateTime( ClientUtil.ToDateTime(dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value).ToShortDateString ());
                if (dateBegin == DateTime.MinValue || dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value==null ||string.IsNullOrEmpty(dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value.ToString()))
                {
                    dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value = DateTime .Now ;
                    dateBegin = DateTime.Now;
                }
                if (dateEnd == DateTime.MinValue ||dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value==null || string.IsNullOrEmpty(dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value.ToString()))
                {
                    dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value = dateBegin;
                    dateEnd = dateBegin;
                }
                else  if (dateBegin > dateEnd)
                {
                    MessageBox.Show("开始时间应该小于等于结束时间!");
                    dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Selected = true;
                    dgProgramPrice.BeginEdit(false);
                }
               
            }
            else if (string.Equals(dgProgramPrice.Columns[iColumnIndex].Name,  colDateEnd .Name ))
            {
                dateBegin =ClientUtil.ToDateTime( ClientUtil.ToDateTime(dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value).ToShortDateString ());
                dateEnd = ClientUtil.ToDateTime(ClientUtil.ToDateTime(dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value).ToShortDateString ());
                if (dateEnd == DateTime.MinValue || dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value==null ||string.IsNullOrEmpty(dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value.ToString()))
                {
                    dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Value = DateTime.Now;
                    dateEnd = DateTime.Now;
                }
                if (dateBegin == DateTime.MinValue ||dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value==null || string.IsNullOrEmpty(dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value.ToString()))
                {
                    dgProgramPrice.Rows[iRowIndex].Cells[colDateBegin.Name].Value = dateEnd;
                    dateBegin=dateEnd ;
                }
                else  if (dateBegin > dateEnd)
                {
                    MessageBox.Show("开始时间应该小于等于结束时间!");
                    dgProgramPrice.Rows[iRowIndex].Cells[colDateEnd.Name].Selected = true;
                    dgProgramPrice.BeginEdit(false);
                }
                
            }
        }
        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (tabProgramPrice.SelectedTab.Name.Equals("物资信息价"))
                {
                    DataGridViewRow dr = dgProgramPrice.CurrentRow;
                    if (dr == null || dr.IsNewRow) return;
                    dgProgramPrice.Rows.Remove(dr);
                    if (dr.Tag != null)
                    {
                        currPrice = dr.Tag as MaterialInterfacePrice;
                        currPrice.State = "0";
                        currPrice = matmodel.SaveMaterialPrice(currPrice);
                    }
                }
            }
            if (tabProgramPrice.SelectedTab.Name.Equals("项目降低率"))
            {
                DataGridViewRow dr = dgRate.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgRate.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    currRate = dr.Tag as ProgramReduceRate;
                    currRate.State = "0";
                    currRate = matmodel.SaveRate(currRate);
                }
            }
        }

        private void txtRate1_LostFocus(object sender,EventArgs e)
        {
            if (txtRate1.Text != "")
            {
                if (ClientUtil.ToDecimal(txtRate1.Text) > 1)
                {
                    MessageBox.Show("降低率不能大于1");
                    txtRate1.Text = "";
                }
            }
        }

        private void txtRate2_LostFocus(object sender,EventArgs e)
        {
            if (txtRate2.Text != "")
            {
                if (ClientUtil.ToDecimal(txtRate2.Text) > 1)
                {
                    MessageBox.Show("降低率不能大于1");
                    txtRate2.Text = "";
                }
                else
                {
                    if (ClientUtil.ToDecimal(txtRate2.Text) < ClientUtil.ToDecimal(txtRate1.Text))
                    {
                        MessageBox.Show("降低率后面的数值大于前面的数值");
                        txtRate2.Text = "";
                    }
                }
            }
        }

        private void btnSearchMat_Click(object sender, EventArgs e)
        {
            CommonMaterial materialSelector = new CommonMaterial();
            materialSelector.OpenSelect();
            IList list = materialSelector.Result;
            if (list.Count > 0)
            {
                Application.Resource.MaterialResource.Domain.Material theMaterial = list[0] as Application.Resource.MaterialResource.Domain.Material;
                txtMatName.Text = theMaterial.Name;
                txtMatName.Tag = theMaterial;
                txtMatSpec.Text = theMaterial.Specification;
                txtMatStuff.Text = theMaterial.Stuff;
            }
            
        }

        private void btnSearch_Click(object sender,EventArgs e)
        {
            string strName = ClientUtil.ToString(txtMatName.Text);
            string strSpec = ClientUtil.ToString(txtMatSpec.Text);
            string strStuff = ClientUtil.ToString(txtMatStuff.Text);
            string strCode = null;
            decimal strPrice1 = 0;
            decimal strPrice2 = 0;
            ObjectQuery objectQuery = new ObjectQuery();
            if (strName != "")
            {
                strCode = ClientUtil.ToString((txtMatName.Tag as Material).Code);
                objectQuery.AddCriterion(Expression.Eq("MatName", strName));
                objectQuery.AddCriterion(Expression.Eq("MatCode", strCode));
            }
            if (strSpec != "")
            {
                objectQuery.AddCriterion(Expression.Like("MatSpec", strSpec, MatchMode.Anywhere)); 
            }
            if (strStuff != "")
            {
                objectQuery.AddCriterion(Expression.Like("MatStuff", strStuff, MatchMode.Anywhere));
            }
            if (txtPrice1.Text.Equals(""))
            { }
            else
            {
                strPrice1 = ClientUtil.ToDecimal(txtPrice1.Text);
                objectQuery.AddCriterion(Expression.Ge("Price", strPrice1));
            }
            if (txtPrice2.Text.Equals(""))
            { }
            else
            {
                strPrice2 = ClientUtil.ToDecimal(txtPrice2.Text);
                objectQuery.AddCriterion(Expression.Lt("Price", strPrice2));
            }
            
            Search(objectQuery);
        }

        private void Search(ObjectQuery objectQuery)
        {
            objectQuery.AddCriterion(Expression.Eq("State", "1"));
            IList MatList = matmodel.CurrentProjectSrv.GetMaterialPrice(objectQuery);
            dgProgramPrice.Rows.Clear();
            if (MatList.Count > 0 && MatList != null)
            {
                dgProgramPrice.Rows.Clear();
                foreach (MaterialInterfacePrice price in MatList)
                {
                    int rowIndex = this.dgProgramPrice.Rows.Add();
                    dgProgramPrice[colMatCode.Name, rowIndex].Value = price.MatCode;
                    dgProgramPrice[colMatName.Name, rowIndex].Value = price.MatName;
                    dgProgramPrice[colMatCode.Name, rowIndex].Tag = price.MaterialGUID;
                    dgProgramPrice[colMatSpec.Name, rowIndex].Value = price.MatSpec;
                    dgProgramPrice[colOldPrice.Name, rowIndex].Value = price.Price;
                    dgProgramPrice[colOldMarketPrice.Name, rowIndex].Value = price.MarketPrice;
                    dgProgramPrice[colNewPrice.Name, rowIndex].Value = price.Price;
                    dgProgramPrice[colNewMarketPrice.Name, rowIndex].Value = price.MarketPrice;
                    dgProgramPrice[colUnit.Name, rowIndex].Value = price.MatUnitName;
                    dgProgramPrice[colUnit.Name, rowIndex].Tag = price.MatUnit;
                    dgProgramPrice[colDateBegin.Name, rowIndex].Value = DateTime .Equals ( price.DateTimeBegin ,DateTime .MinValue )? "": price.DateTimeBegin.ToShortDateString ();
                    dgProgramPrice[colDateEnd.Name, rowIndex].Value = DateTime.Equals(price.DateTimeEnd, DateTime.MinValue) ? "" : price.DateTimeEnd.ToShortDateString();
                    //price.DateTimeEnd;
                    dgProgramPrice[colDescription.Name, rowIndex].Value = price.Descript;
                    dgProgramPrice.Rows[rowIndex].Tag = price;
                    rowIndex++;
                }
            }
        }

        private void SearchRace(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Supplyer", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Supplyer.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MakePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);//
            objectQuery.AddCriterion(Expression.Eq("State", "1"));
            IList MatList = matmodel.CurrentProjectSrv.GetProgramRate(objectQuery);
            dgRate.Rows.Clear();
            if (MatList.Count > 0 && MatList != null)
            {
                foreach (ProgramReduceRate price in MatList)
                {
                    int rowIndex = this.dgRate.Rows.Add();
                    dgRate[colProjectName.Name, rowIndex].Tag = price.ProjectId;
                    dgRate[colProjectName.Name, rowIndex].Value = price.ProjectName;
                    dgRate[colSupply.Name, rowIndex].Tag = price.Supplyer;
                    dgRate[colSupply.Name, rowIndex].Value = price.SupplyerName;
                    dgRate[colOldRate.Name, rowIndex].Value = price.Rate;
                    dgRate[colNewRate.Name, rowIndex].Value = price.Rate;
                    dgRate[colDescript.Name, rowIndex].Value = price.Descript;
                    dgRate[colRateMoney.Name, rowIndex].Value = price.RateMoney;
                    dgRate[colNewRateMoney.Name, rowIndex].Value = price.RateMoney;
                     
                    dgRate[colMaterCategary.Name, rowIndex].Tag = price.MaterialCategory;
                    dgRate[colMaterCategary.Name, rowIndex].Value = price.MaterialCategoryName;
                    dgRate.Rows[rowIndex].Tag = price;
                    rowIndex++;
                }
            }
        }


        private bool ValidView()
        {
            Hashtable ht = new Hashtable();
            string strPrice = string.Empty;
            string strMarketPrice = string.Empty;
            foreach (DataGridViewRow row in dgProgramPrice.Rows)
            {
                bool flag = false;
                if (row.IsNewRow ) continue;

                Material mat = row.Cells[colMatCode.Name].Tag as Material;
                if (mat == null)
                {
                    continue;
                }
                DateTime  dateCurBegin = ClientUtil.ToDateTime ( ClientUtil.ToDateTime (row.Cells[colDateBegin.Name].Value).ToShortDateString ());
                DateTime dateCurEnd =  ClientUtil.ToDateTime (ClientUtil.ToDateTime(row.Cells[colDateEnd.Name].Value).ToShortDateString ());
                flag = false;
                if (ht.ContainsValue (mat.Id))
                {
                    foreach (System.Collections.DictionaryEntry obj in ht)
                    {
                        DataGridViewRow var = obj.Key  as DataGridViewRow;
                        Material material = var.Cells[colMatCode.Name].Tag as Material;
                        DateTime dateTimeBegin = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[colDateBegin.Name].Value).ToShortDateString());
                        DateTime dateTimeEnd = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[colDateEnd.Name].Value).ToShortDateString ());
                        if (mat.Id == material.Id)
                        {
                            if (IsInclude(dateCurBegin, dateCurEnd, dateTimeBegin, dateTimeEnd))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            string sErr = string.Format("提示：第【{0}】行与第【{1}】行相同物质，时间段不存在交集\n  相同物资资源的情况下，不能出现相同的时间段！",row.Index +1,var.Index +1);
                            //MessageBox.Show("相同物资资源的情况下，不能出现相同的时间段！");
                            MessageBox.Show(sErr);
                            return false;
                        }
                    }
                }
                ht.Add(row, mat .Id );
                  strPrice = ClientUtil.ToString(row.Cells[colNewPrice.Name].Value);
                  strMarketPrice = ClientUtil.ToString(row.Cells[colNewMarketPrice.Name].Value);
                  if (string.IsNullOrEmpty(strPrice) && string.IsNullOrEmpty(strMarketPrice))
                  {
                      string sErr = string.Format("提示：第【{0}】行 维护价和市场维护价不能同时为空 ", row.Index + 1 );
                      //MessageBox.Show("相同物资资源的情况下，不能出现相同的时间段！");
                      MessageBox.Show(sErr);
                      return false;
                  }
            }
            return true;
        }
        public bool IsInclude(DateTime DateCurBegin, DateTime DateCurEnd, DateTime DateBegin, DateTime DateEnd)
        {
            bool bFlag = true ;
            if (DateCurBegin <= DateCurEnd && DateBegin <= DateEnd)
            {
                if (DateCurBegin > DateEnd || DateCurEnd < DateBegin)
                {
                    bFlag= false;
                }
            }
            return bFlag;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.label1.Focus();
            if (!ValidView()) return;
            //循环dategridview
            bool flag = false;
            IList lstMaterialInterfacePrice = new ArrayList();
            MaterialInterfacePrice currPrice = null;
            bool IsNew = false;
            DateTime dateBegin;
            DateTime dateEnd;
            string sDescript = string.Empty;
            decimal Price = 0;
            decimal MarketPrice = 0;
            string sErr = string.Empty;
            try
            {
                foreach (DataGridViewRow var in this.dgProgramPrice.Rows)
                {
                    IsNew = false;
                    if (var.IsNewRow) continue;
                    if (var.Cells[colMatCode.Name].Tag == null) continue;
                    Price = ClientUtil.ToDecimal(var.Cells[colNewPrice.Name].Value);
                    MarketPrice = ClientUtil.ToDecimal (var.Cells[colNewMarketPrice.Name].Value);

                    if (Price > 0 || MarketPrice>0)
                    {
                        if (var.Tag != null)
                        {
                            currPrice = var.Tag as MaterialInterfacePrice;
                            IsNew = false ;
                        }
                        else
                        {
                            currPrice = new MaterialInterfacePrice();
                            IsNew = true;
                        }
                          dateBegin = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[colDateBegin.Name].Value).ToShortDateString());
                          dateEnd = ClientUtil.ToDateTime(ClientUtil.ToDateTime(var.Cells[colDateEnd.Name].Value).ToShortDateString());
                          sDescript = ClientUtil.ToString(var.Cells[colDescription.Name].Value);
                        
                        if (IsNew)
                        {
                            currPrice.MakeTime = DateTime.Now;
                            currPrice.MatCode = ClientUtil.ToString(var.Cells[colMatCode.Name].Value);
                            currPrice.MaterialGUID = var.Cells[colMatCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                            currPrice.MatName = ClientUtil.ToString(var.Cells[colMatName.Name].Value);
                            currPrice.MatSpec = ClientUtil.ToString(var.Cells[colMatSpec.Name].Value);
                            currPrice.MatStuff = ClientUtil.ToString((var.Cells[colMatCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material).Stuff);
                            currPrice.MatUnit = var.Cells[colUnit.Name].Tag as StandardUnit;
                            currPrice.MatUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);
                            if (var.Cells[colNewPrice.Name].Value != null)
                            {
                                currPrice.Price = ClientUtil.ToDecimal(var.Cells[colNewPrice.Name].Value);
                            }
                            if (var.Cells[colNewMarketPrice.Name].Value != null)
                            {
                                currPrice.MarketPrice = ClientUtil.ToDecimal(var.Cells[colNewMarketPrice.Name].Value);
                            }
                            currPrice.Price = Price;
                            currPrice.MarketPrice = MarketPrice;
                            currPrice.DateTimeBegin = dateBegin;
                            currPrice.DateTimeEnd = dateEnd;
                            currPrice.State = "1";//1为可用，0为禁用
                            currPrice.Descript = sDescript;
                            currPrice.MakePerson = ConstObject.LoginPersonInfo;
                            currPrice.MakePersonName = ConstObject.LoginPersonInfo.Name;
                            var.Tag = currPrice;
                            lstMaterialInterfacePrice.Add(currPrice);
                            //currPrice = matmodel.SaveMaterialPrice(currPrice);
                            flag = true;
                        }
                        else
                        {
                            if (currPrice.Price == Price && currPrice.MarketPrice == MarketPrice && ((string.IsNullOrEmpty (currPrice.Descript) &&string.IsNullOrEmpty ( sDescript) )|| currPrice.Descript == sDescript ) && currPrice.DateTimeBegin == dateBegin && currPrice.DateTimeEnd == dateEnd)
                            {
                            }
                            else
                            {
                                currPrice.Price = Price ;
                                currPrice.MarketPrice = MarketPrice ;
                                currPrice.Descript = sDescript ;
                                currPrice.DateTimeBegin = dateBegin ;
                                currPrice.DateTimeEnd = dateEnd;
                                lstMaterialInterfacePrice.Add(currPrice);
                            }
                        }
                    }
                    else
                    {
                       sErr  =string.Format("第【{0}】行 维护价和市场维护价不能同时为0", var.Index + 1);
                        flag = false;
                        break;

                    }
                }
                if (string.IsNullOrEmpty (sErr ))
                {
                    if (lstMaterialInterfacePrice.Count > 0)
                    {
                        flag = matmodel.CurrentProjectSrv.SaveMaterialInterfacePrice(lstMaterialInterfacePrice);
                        if (flag)
                        {
                            sErr = "保存成功！";
                        }
                        else
                        {
                            sErr = "保存失败！";
                        }
                    }
                    else
                    {
                        sErr = "没有需要保存信息（你未修改信息）！";
                        flag = false; 
                    }
                }
                else
                {
                }

            }
            catch
            {
                flag = false;
            }
            MessageBox.Show(sErr );
            if (flag)
            {
                this.ViewCaption = "物资信息价";
                //MessageBox.Show("保存成功！");
                btnSearch_Click(sender, e);
            }
            else
            {
               // MessageBox.Show("保存失败！");
            }
        }

         /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgProgramPrice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgProgramPrice.Columns[e.ColumnIndex].Name.Equals(colMatCode.Name))
            {
                if (this.dgProgramPrice.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
                {
                    MessageBox.Show("不能替换，请在尾部添加");
                    return;
                }
                DataGridViewRow theCurrentRow = this.dgProgramPrice.CurrentRow;
                CommonMaterial materialSelector = new CommonMaterial();
                DataGridViewCell cell = this.dgProgramPrice[e.ColumnIndex, e.RowIndex];
                object tempValue = cell.EditedFormattedValue;
                if (tempValue != null && !tempValue.Equals(""))
                {
                    materialSelector.OpenSelect(tempValue.ToString());
                    materialSelector.OpenSelect();
                }
                else
                {
                    materialSelector.OpenSelect();
                }

                IList list = materialSelector.Result;
                foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                {
                    //ObjectQuery oq = new ObjectQuery();
                    //oq.AddCriterion(Expression.Eq("MatCode", theMaterial.Code));
                    //oq.AddCriterion(Expression.Eq("State", "1"));
                    //IList MatList = matmodel.CurrentProjectSrv.GetMaterialPrice(oq);
                    //if (MatList.Count == 0)
                    //{
                        int i = dgProgramPrice.Rows.Add();
                        this.dgProgramPrice[colMatCode.Name, i].Tag = theMaterial;
                        this.dgProgramPrice[colMatCode.Name, i].Value = theMaterial.Code;
                        this.dgProgramPrice[colMatName.Name, i].Value = theMaterial.Name;
                        this.dgProgramPrice[colMatSpec.Name, i].Value = theMaterial.Specification;
                        this.dgProgramPrice[colStuff.Name, i].Value = theMaterial.Stuff;
                        this.dgProgramPrice[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                        this.dgProgramPrice[colDateBegin.Name, i].Value = DateTime.Now;
                        this.dgProgramPrice[colDateEnd .Name ,i].Value = DateTime.Now;
                        if (theMaterial.BasicUnit != null)
                            this.dgProgramPrice[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                        i++;
                   // }
                }
            }
        }

        private void btnSearchRate_Click(object sender,EventArgs e)
        {
            string strName = ClientUtil.ToString(txtProject.Text);
            decimal strPrice1 = 0;
            decimal strPrice2 = 0;
            ObjectQuery objectQuery = new ObjectQuery();
            if (strName != "")
            {
                objectQuery.AddCriterion(Expression.Eq("ProjectName", strName));
            }
            if (txtSupplyUnit.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("Supplyer", txtSupply.Tag as SupplierRelationInfo));
            }
            if (txtRate1.Text.Equals(""))
            { }
            else
            {
                strPrice1 = ClientUtil.ToDecimal(txtRate1.Text);
                objectQuery.AddCriterion(Expression.Ge("Rate", strPrice1));
            }
            if (txtRate2.Text.Equals(""))
            { }
            else
            {
                strPrice2 = ClientUtil.ToDecimal(txtRate2.Text);
                objectQuery.AddCriterion(Expression.Lt("Rate", strPrice2));
            }
            

            SearchRace(objectQuery);
        }

        /// <summary>
        /// 项目名称列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgRate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
           string sProjectName=ClientUtil .ToString ( this.dgRate.Rows[e.RowIndex].Cells[colProjectName.Name].Value );
            string sProjectID = this.dgRate.Rows[e.RowIndex].Cells[colProjectName.Name].Tag == null ? null : ClientUtil.ToString(this.dgRate.Rows[e.RowIndex].Cells[colProjectName.Name ].Tag);
            MaterialCategory oMaterialCategory = this.dgRate.Rows[e.RowIndex].Cells[colMaterCategary.Name].Tag == null ? null : this.dgRate.Rows[e.RowIndex].Cells[colMaterCategary.Name ].Tag as MaterialCategory;
            SupplierRelationInfo oSupplierRelationInfo = this.dgRate.Rows[e.RowIndex].Cells[colSupply.Name].Tag == null ? null : this.dgRate.Rows[e.RowIndex].Cells[colSupply.Name].Tag as SupplierRelationInfo;
            if (this.dgRate.Columns[e.ColumnIndex].Name.Equals(colProjectName.Name))
            {
                if (dgRate.CurrentRow.Tag != null)
                {
                    MessageBox.Show("禁止替换，请在尾部添加行");
                    return;
                }
                VDepartSelector vmros = new VDepartSelector("1");
                vmros.ShowDialog();
                IList list = vmros.Result;
                if (list == null || list.Count == 0) return;
                CurrentProjectInfo project = list[0] as CurrentProjectInfo;
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("ProjectName", project.Name));
                //IList ProList = matmodel.CurrentProjectSrv.GetProgramRate(oq);
                if (project != null)
                {
                    if(  Check(project.Id, oMaterialCategory, oSupplierRelationInfo) )
                    {
                        dgRate.CurrentRow.Cells[colProjectName.Name].Value = project.Name;
                        dgRate.CurrentRow.Cells[colProjectName.Name].Tag = project.Id;
                        txtProject.Focus();

                    }
                    else
                    {
                        MessageBox.Show(string.Format ("项目【{0}】 物资分类【{1}】 供应商【{2}】已经存在！",project .Name ,oMaterialCategory .Name ,oSupplierRelationInfo.SupplierInfo .Name ));
                        dgRate.CurrentRow.Cells[colProjectName.Name].Selected = true;
                        dgRate.BeginEdit(false);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请选择项目！");
                    dgRate.CurrentRow.Cells[colProjectName.Name].Selected = true;
                    dgRate.BeginEdit(false);
                    return;
                }
                //if (ProList.Count == 0)
                //{
                //    dgRate.CurrentRow.Cells[colProjectName.Name].Value = project.Name;
                //    dgRate.CurrentRow.Cells[colProjectName.Name].Tag = project.Id;
                //    txtProject.Focus();
                //}
                //else
                //{
                //    MessageBox.Show("项目信息已经存在！");
                //    return;
                //}
            }
            if (this.dgRate.Columns[e.ColumnIndex].Name.Equals(colSupply.Name))
            {
                if (dgRate.CurrentRow.Tag != null)
                {
                    MessageBox.Show("禁止替换，请在尾部添加行");
                    return;
                }
                VCommonSupplierRelationSelect supply = new VCommonSupplierRelationSelect();
                supply.OpenSelectView(null, null, CommonUtil.SupplierCatCode + "-");
                IList list = supply.Result;
                if (list.Count == 0) return;
                SupplierRelationInfo supplyer = list[0] as SupplierRelationInfo;
                //foreach (SupplierRelationInfo supplyer in list)
                //{
                //    dgRate.CurrentRow.Cells[colSupply.Name].Value = supplyer.SupplierInfo.Name;
                //    dgRate.CurrentRow.Cells[colSupply.Name].Tag = supplyer;
                //}
                if (supplyer != null)
                {
                    if (Check(sProjectID, oMaterialCategory, supplyer))
                    {
                        dgRate.CurrentRow.Cells[colSupply.Name].Value = supplyer.SupplierInfo.Name;
                        dgRate.CurrentRow.Cells[colSupply.Name].Tag = supplyer;
                    }
                    else
                    {

                        MessageBox.Show(string.Format("项目【{0}】 物资分类【{1}】 供应商【{2}】已经存在！", sProjectName, oMaterialCategory.Name, supplyer.SupplierInfo.Name));
                        dgRate.CurrentRow.Cells[colSupply.Name].Selected = true;
                        dgRate.BeginEdit(false);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请选供应商！");
                    dgRate.CurrentRow.Cells[colSupply.Name].Selected = true;
                    dgRate.BeginEdit(false);
                    return;
                }
            }
            
            if (this.dgRate.Columns[e.ColumnIndex].Name.Equals(colMaterCategary.Name))
            {
                if (dgRate.CurrentRow.Tag != null)
                {
                    MessageBox.Show("禁止替换，请在尾部添加行");
                    return;
                }
                CommonMaterial materialCatSelector = new CommonMaterial();
                materialCatSelector.ObjectType = MaterialSelectType.MaterialCatView;
                materialCatSelector.OpenSelect();
                IList list = materialCatSelector.Result;
                if (list != null && list.Count > 0  )
                {
                    MaterialCategory mc = list[0] as MaterialCategory;
                    if (mc != null)
                    {
                        if (Check(sProjectID, mc, oSupplierRelationInfo))
                        {
                            dgRate.CurrentRow.Cells[colMaterCategary.Name].Tag = mc;
                            dgRate.CurrentRow.Cells[colMaterCategary.Name].Value = mc.Name;
                        }
                        else
                        {
                            MessageBox.Show(string.Format("项目【{0}】 物资分类【{1}】 供应商【{2}】已经存在！", sProjectName, mc.Name, oSupplierRelationInfo.SupplierInfo.Name));
                            dgRate.CurrentRow.Cells[colMaterCategary.Name].Selected = true;
                            dgRate.BeginEdit(false);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选物资分类！");
                        dgRate.CurrentRow.Cells[colMaterCategary.Name].Selected = true;
                        dgRate.BeginEdit(false);
                        return;
                    }
                }
                //foreach (MaterialCategory mc in list)
                //{
                //    dgRate.CurrentRow.Cells[colMaterCategary.Name].Tag = mc;
                //    dgRate.CurrentRow.Cells[colMaterCategary.Name].Value = mc.Name;
                //}
            }
        }
        
        public bool Check(string sProjectID, MaterialCategory oMaterialCategory, SupplierRelationInfo oSupplierRelationInfo)
        {
            bool bFlag = true;
            if (!string.IsNullOrEmpty(sProjectID) && oMaterialCategory != null && oSupplierRelationInfo != null)
            {
               bFlag = !matmodel.CurrentProjectSrv.GetProgramRate(sProjectID, oMaterialCategory.Id, oSupplierRelationInfo.Id);
            }
            return bFlag;

        }
        void btnProject_Click(object sender,EventArgs e)
        {
            VDepartSelector vmros = new VDepartSelector("1");
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            CurrentProjectInfo project = list[0] as CurrentProjectInfo;
            txtProject.Text = project.Name;
            txtProject.Tag = project.Id;
        }

        private bool VaView()
        {
            decimal Rate = 0;
            decimal RateMoney = 0;
            foreach (DataGridViewRow dataRow in dgRate.Rows)
            {
                if (dataRow.IsNewRow) break;
                Rate = ClientUtil.ToDecimal(dataRow.Cells[colNewRate.Name].Value);

                RateMoney = ClientUtil.ToDecimal(dataRow.Cells[colNewRateMoney.Name].Value);
                if (RateMoney >0 && Rate >0)
                {
                    MessageBox.Show("维护降低率和维护降低额只能设置其一！");
                    return false;
                }
                else if (RateMoney <= 0 && Rate <= 0)
                {
                    MessageBox.Show("维护降低率和维护降低额必须设置其一！");
                    return false;
                }
            }
            return true;
        }

        void btnSaveRate_Click(object sender,EventArgs e)
        {
            //循环dategridview
            bool flag = false;
            string sErr = string.Empty;
            bool IsNew = false;
            if (!VaView()) return;
            string sProjectID = string.Empty;
            string sDescript=string.Empty ;
            MaterialCategory oMaterialCategory = null;
            SupplierRelationInfo oSupplierRelationInfo = null;
            ProgramReduceRate currRate = null;
            decimal Rate=0;
            decimal  RateMoney=0;
            IList lstProgramReduceRates = new ArrayList();
            foreach (DataGridViewRow var in this.dgRate.Rows)
            {
               // IsNew = false;
                if (var.IsNewRow) continue ;
                sProjectID = ClientUtil.ToString(var.Cells[colProjectName.Name].Tag);
                oMaterialCategory = var.Cells[colMaterCategary.Name].Tag as MaterialCategory; ;
                oSupplierRelationInfo = var.Cells[colSupply.Name].Tag as SupplierRelationInfo;
                if (!string.IsNullOrEmpty(sProjectID) && oMaterialCategory != null && oSupplierRelationInfo != null)
                {

                    Rate = ClientUtil.ToDecimal (var.Cells[colNewRate.Name].Value);
                    RateMoney = ClientUtil.ToDecimal(var.Cells[colRateMoney.Name].Value);
                    sDescript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    if (Rate > 0 || RateMoney > 0)
                    {
                        if (var.Tag != null)
                        {
                            currRate = var.Tag as ProgramReduceRate;
                            IsNew = false;
                        }
                        else
                        {
                            currRate = new ProgramReduceRate();
                            IsNew = true;
                        }
                        if (IsNew)
                        {
                            currRate.State = "1";//1为可用，0为禁用
                            currRate.ProjectId = sProjectID;
                            currRate.ProjectName = ClientUtil.ToString(var.Cells[colProjectName.Name].Value);
                            currRate.Supplyer = oSupplierRelationInfo;
                            currRate.SupplyerName = ClientUtil.ToString(var.Cells[colSupply.Name].Value);
                            currRate.Rate = Rate;//ClientUtil.ToDecimal(var.Cells[colNewRate.Name].Value);
                            if (currRate.Rate > 100)
                            {
                                MessageBox.Show("项目[" + currRate.ProjectName + "]供应商[" + currRate.SupplyerName + "]降低率不能大于100！");
                                sErr = string.Format("项目[{0}]供应商[{1}] 物资[{2}]降低率不能大于100！", currRate.ProjectName, currRate.SupplyerName, oMaterialCategory.Name);
                                flag = false;
                                break;
                            }
                            currRate.Descript =sDescript;// ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                            currRate.MakePerson = ConstObject.LoginPersonInfo;
                            currRate.MakePersonName = ConstObject.LoginPersonInfo.Name;
                            currRate.MaterialCategory = oMaterialCategory;
                            currRate.MaterialCategoryName = ClientUtil.ToString(var.Cells[colMaterCategary.Name].Value);
                            currRate.RateMoney =RateMoney;// ClientUtil.ToDecimal(var.Cells[colNewRateMoney.Name].Value);
                            var.Tag = currRate;
                            //currRate = matmodel.CurrentProjectSrv.SaveProgramRate(currRate);
                            lstProgramReduceRates.Add(currRate);
                            flag = true;
                        }
                        else
                        {
                            if (currRate.ProjectId == sProjectID && currRate.MaterialCategory.Id == oMaterialCategory.Id && oSupplierRelationInfo.Id == currRate.Supplyer.Id && Rate == currRate.Rate && RateMoney == currRate.RateMoney && (string.Equals(currRate.Descript, sDescript) || (string.IsNullOrEmpty(currRate.Descript) && string.IsNullOrEmpty(sDescript))))
                            {

                            }
                            else
                            {
                                currRate.ProjectId = sProjectID;
                                currRate.MaterialCategory = oMaterialCategory;
                                currRate.MaterialCategoryName = oMaterialCategory.Name;
                                currRate.Supplyer = oSupplierRelationInfo;
                                currRate.SupplyerName = oSupplierRelationInfo.SupplierInfo.Name;
                                currRate.Rate = Rate;
                                currRate.RateMoney = RateMoney;
                                currRate.Descript = sDescript;
                                lstProgramReduceRates.Add(currRate);
                                flag = true;
                            }
                        }
                    }
                    else
                    {
                        sErr = string.Format("第【{0}】行降低率和降低额不能同时为零", var.Index + 1);
                        flag = false;
                        break;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(sProjectID))
                    {
                        sErr = string.Format("第【{0}】行项目为空", var.Index + 1);
                       
                    }
                    else if (oSupplierRelationInfo == null)
                    {
                        sErr = string.Format("第【{0}】行供应商为空", var.Index + 1);
                    }
                    else
                    {
                        sErr = string.Format("第【{0}】行物资分类为空", var.Index + 1);
                    }
                    flag = false; 
                    break;
                }
            }
            if (string.IsNullOrEmpty(sErr))
            {
                if (lstProgramReduceRates.Count > 0)
                {
                    flag = matmodel.CurrentProjectSrv.SaveProgramRate(lstProgramReduceRates);
                    if (flag)
                    {
                        sErr = "保存成功！";
                    }
                    else
                    {
                        sErr = "保存失败！";
                    }
                }
                else
                {
                    sErr = "没有需要保存信息";
                }
            }
            MessageBox.Show(sErr);
            if (flag)
            {
                this.ViewCaption = "项目降低率";
              
                btnSearchRate_Click(sender, e);
            }
             
        }

    }
}