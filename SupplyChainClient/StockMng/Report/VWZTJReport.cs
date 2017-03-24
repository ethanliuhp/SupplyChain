using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.IO;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    public partial class VWZTJReport : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MCostAccountSubject subject = new MCostAccountSubject();
        CostMonthAccountBill costBill = new CostMonthAccountBill();
        IList list_subject = new ArrayList();
        MStockMng modelStock = new MStockMng();
        #region ��������
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        private CurrentProjectInfo ProjectInfo;
        string supplyExptr = "�ɹ��ɱ��Աȷ�����";
        string materialExptr = "�������ĶԱȱ�";
        string formatQuanttiy = "################0.###";
        string formatMoney = "################0.##";
        #endregion
        #region �������
        Hashtable htSupply = new Hashtable();//�ɹ��ɱ��Աȷ�����
        IList list_material = new ArrayList();//�������ĶԱȱ�
        
        #endregion

        public VWZTJReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            ProjectInfo = StaticMethod.GetProjectInfo();
            IList projectLst = new ArrayList();//������Բ�ѯ���е���Ŀ
            projectLst.Add(ProjectInfo);
            cmbProject.DataSource = projectLst;
            cmbProject.DisplayMember = "Name";
            cmbProject.ValueMember = "Id";
            cmbProject.SelectedItem = ProjectInfo;
            this.cmbProject.Enabled = false;

            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;

            ////�����Ŀ
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddOrder(Order.Asc("Code"));
            //list_subject = subject.Mm.GetCostAccountSubjects(typeof(CostAccountSubject), oq);

            this.fGridSupply.Rows = 1;
            this.fGridMaterial.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click+=new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridSupply.ExportToExcel(supplyExptr, false, false, true);
            fGridMaterial.ExportToExcel(materialExptr, false, false, true);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(supplyExptr + ".flx");
            LoadTempleteFile(materialExptr + ".flx");

            //��������

            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("TheProjectGUID", ProjectInfo.Id));
            //oq.AddCriterion(Expression.Eq("CreateDate", dtpDateBegin.Value.Date));
            //oq.AddCriterion(Expression.Eq("CreateDate", dtpDateEnd.Value.Date));
            //IList list = model.CostMonthAccSrv.GetCostMonthAccountBill(oq);
            //if (list != null && list.Count > 0)
            //{
            //    costBill = (CostMonthAccountBill)list[0];
            //}
            //else
            //{
            //    costBill = new CostMonthAccountBill();
            //}
            this.LoadSupplyFile();
            this.LoadMaterialFile();

            //�������
            CommonUtil.SetFlexGridFace(this.fGridSupply);
            CommonUtil.SetFlexGridFace(this.fGridMaterial);
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //�����ʽ
                if (modelName == "�ɹ��ɱ��Աȷ�����.flx")
                {                   
                    fGridSupply.OpenFile(path + "\\" + modelName);//�����ʽ
                }
                else if (modelName == "�������ĶԱȱ�.flx")
                {
                    fGridMaterial.OpenFile(path + "\\" + modelName);//�����ʽ
                }
            } else {
                MessageBox.Show("δ�ҵ�ģ���ʽ�ļ�" + modelName);
                return;
            }
        }

        #region  ��������

        /// <summary>
        /// д��ϼ��е�����
        /// </summary>
        /// <param name="flexGrid">��ά�����</param>
        /// <param name="startRow">���㷶Χ�Ŀ�ʼ��</param>
        /// <param name="endRow">���㷶Χ�Ľ�����</param>
        /// <param name="startCol">���㷶Χ�Ŀ�ʼ��</param>
        /// <param name="endCol">���㷶Χ�Ľ�����</param>
        private void WriteSumGridData(CustomFlexGrid flexGrid, int startRow, int endRow, int startCol, int endCol)
        {
            flexGrid.Cell(endRow + 1, startCol - 1).Text = "�ϼƣ�";           
            for (int i = startCol; i <= endCol; i++)
            {
                decimal sumValue = 0;
                for (int t = startRow; t <= endRow; t++)
                {
                    string ifCalSum = ClientUtil.ToString(flexGrid.Cell(t, 1).Tag);
                    if (ifCalSum == "1")
                    {
                        sumValue += ClientUtil.ToDecimal(flexGrid.Cell(t, i).Text);
                    }
                }
                flexGrid.Cell(endRow + 1, i).Text = sumValue + "";
            }
            FlexCell.Range range = flexGrid.Range(endRow + 1, 1, endRow + 1, endCol);
            CommonUtil.SetFlexGridDetailFormat(range);
            flexGrid.Cell(endRow + 1, startCol - 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
        }

        private CostMonthAccDtlConsume AddCostMonthAccDtlConsume(CostMonthAccDtlConsume dtlConsume,CostMonthAccDtlConsume addDtlConsume)
        {
            dtlConsume.CurrIncomeQuantity += addDtlConsume.CurrIncomeQuantity;
            dtlConsume.CurrRealConsumePlanQuantity += addDtlConsume.CurrRealConsumePlanQuantity;
            dtlConsume.CurrIncomeTotalPrice += addDtlConsume.CurrIncomeTotalPrice;
            dtlConsume.CurrRealConsumePlanTotalPrice += addDtlConsume.CurrRealConsumePlanTotalPrice;
            dtlConsume.CurrRealConsumeQuantity += addDtlConsume.CurrRealConsumeQuantity;
            dtlConsume.CurrRealConsumeTotalPrice += addDtlConsume.CurrRealConsumeTotalPrice;
            dtlConsume.CurrResponsiConsumeQuantity += addDtlConsume.CurrResponsiConsumeQuantity;
            dtlConsume.CurrResponsiConsumeTotalPrice += addDtlConsume.CurrResponsiConsumeTotalPrice;
            dtlConsume.SumIncomeQuantity += addDtlConsume.SumIncomeQuantity;
            dtlConsume.SumIncomeTotalPrice += addDtlConsume.SumIncomeTotalPrice;
            dtlConsume.SumRealConsumePlanQuantity += addDtlConsume.SumRealConsumePlanQuantity;
            dtlConsume.SumRealConsumePlanTotalPrice += addDtlConsume.SumRealConsumePlanTotalPrice;
            dtlConsume.SumRealConsumeQuantity += addDtlConsume.SumRealConsumeQuantity;
            dtlConsume.SumRealConsumeTotalPrice += addDtlConsume.SumRealConsumeTotalPrice;
            dtlConsume.SumResponsiConsumeQuantity += addDtlConsume.SumResponsiConsumeQuantity;
            dtlConsume.SumResponsiConsumeTotalPrice += addDtlConsume.SumResponsiConsumeTotalPrice;

            return dtlConsume;
        }

        /// <summary>
        /// ��ѯ�˸��ڵ��¼���ĳɱ���Ŀ,�����¶Ⱥ����������Ķ��󼯺�,������
        /// </summary>
        /// <param name="rootSubject">��ѯ�ĳɱ���Ŀ���ڵ�</param>
        /// <param name="queryLevel">���²�ѯ����</param>
        /// <param name="ifContainSelf">�Ƿ�����Լ�</param>
        private IList GetCostSubjectList(string rootSubjectCode, int queryLevel, bool ifContainSelf)
        {
            CostAccountSubject rootSubject = new CostAccountSubject();
            foreach (CostAccountSubject subject in list_subject)
            {
                if (subject.Code == rootSubjectCode)
                {
                    rootSubject = subject;
                    break;
                }
            }

            IList list = new ArrayList();
            int rootLevel = rootSubject.Level;
            int maxLevel = rootLevel + queryLevel;
            string rootSyscode = rootSubject.SysCode;
            foreach (CostAccountSubject subject in list_subject)
            {
                if (ifContainSelf == true && subject.SysCode.Equals(rootSyscode))
                {
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.Data2 = "1";//��Բ㼶
                    dtlConsume.CostingSubjectGUID = subject;
                    dtlConsume.CostingSubjectName = subject.Name;
                    dtlConsume.CostSubjectCode = subject.Code;
                    dtlConsume.CostSubjectSyscode = subject.SysCode;
                    list.Add(dtlConsume);
                }
                if (rootSyscode != null && subject.SysCode.Contains(rootSyscode) && subject.Level > rootLevel && subject.Level <= maxLevel)
                {
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    if (ifContainSelf == true)
                    {
                        dtlConsume.Data2 = (subject.Level - rootLevel + 1) + "";//��Բ㼶
                    }
                    else {
                        dtlConsume.Data2 = (subject.Level - rootLevel) + "";//��Բ㼶
                    }
                    dtlConsume.CostingSubjectGUID = subject;
                    dtlConsume.CostingSubjectName = subject.Name;
                    dtlConsume.CostSubjectCode = subject.Code;
                    dtlConsume.CostSubjectSyscode = subject.SysCode;
                    list.Add(dtlConsume);
                }
            }
            return list;
        }

        #endregion

        #region �ɹ��ɱ��Աȷ�����
       
        private void LoadSupplyFile()
        {            
            string matCode="";
            if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            {
                MaterialCategory strCode = txtMaterialCategory.Result[0] as MaterialCategory;
                matCode=strCode.Code;
            }
            //����ͨ����
            Hashtable htSupply = modelStock.StockInSrv.GetStockInBal(matCode, dtpDateBegin.Value.Date, dtpDateEnd.Value.Date, ProjectInfo);

            //htSupply = CreateFourCalData();
            int dtlStartRowNum = 4;//ģ���е��к�
            int dtlCount = htSupply.Count;

            //������ϸ��
            this.fGridSupply.InsertRow(dtlStartRowNum, dtlCount + 1);
            //���õ�Ԫ��ı߿򣬶��뷽ʽ
            FlexCell.Range range = fGridSupply.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridSupply.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGridSupply.Cell(2, 2).Text = ProjectInfo.Name;
            this.fGridSupply.Cell(2, 9).Text = ConstObject.LoginDate.ToShortDateString();

            //д����ϸ����
            int i = 0;
            foreach(DictionaryEntry ht in htSupply)
            {
                StockInBalDetail dtlConsume = ht.Value as StockInBalDetail;
                fGridSupply.Cell(dtlStartRowNum + i, 1).Text = ClientUtil.ToString(i + 1);
                fGridSupply.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.MaterialName;
                //fGridSupply.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGridSupply.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGridSupply.Cell(dtlStartRowNum + i, 3).Text = dtlConsume.MaterialSpec;
                fGridSupply.Cell(dtlStartRowNum + i, 3).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //��ǰֵ
                fGridSupply.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(dtlConsume.MatStandardUnitName);
                fGridSupply.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(dtlConsume.Quantity);
                decimal ysprice = 0;
                if (ClientUtil.ToDecimal(dtlConsume.TempData2) != 0)
                {
                    ysprice = decimal.Round(dtlConsume.Price / ClientUtil.ToDecimal(dtlConsume.TempData2),4);//ƽ���۸�
                }
                fGridSupply.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(ysprice);//ƽ���۸�
                decimal money = dtlConsume.Quantity * ysprice;//�ɹ����
                fGridSupply.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(money);//�ɹ����
                fGridSupply.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(dtlConsume.TempData1);//�г���
                decimal suparMoney = dtlConsume.Quantity * ClientUtil.ToDecimal(dtlConsume.TempData1);
                fGridSupply.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(suparMoney);//�г����
                decimal orderMoney = ClientUtil.ToDecimal(dtlConsume.TempData);
                fGridSupply.Cell(dtlStartRowNum + i, 10).Text = dtlConsume.TempData;//��ͬ�����
                if (suparMoney != 0)
                {
                    fGridSupply.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(decimal.Round((suparMoney - money), 4));
                    fGridSupply.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(decimal.Round(((suparMoney - money) / suparMoney * 100), 4)) + "%";
                }
                if (orderMoney != 0)
                {
                    fGridSupply.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round((orderMoney - money), 4));
                    fGridSupply.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(decimal.Round((orderMoney - money) / orderMoney, 4) * 100) + "%";
                }
                i++;
            }
            //д��ϼ�ֵ
            //this.WriteSumGridData(fGridSupply, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 18);
            fGridSupply.Cell(dtlStartRowNum + htSupply.Count + 1, 2).Text = "";
            fGridSupply.Cell(dtlStartRowNum + htSupply.Count + 1, 9).Text = ConstObject.LoginDate.ToShortDateString();
        }

        #endregion

        #region �������ĶԱȱ�

        private void LoadMaterialFile()
        {
            CurrentProjectInfo pi = cmbProject.SelectedItem as CurrentProjectInfo;
            string beginDate = dtpDateBegin.Value.ToShortDateString();
            string endDate = dtpDateEnd.Value.ToShortDateString();
            MaterialCategory materialCategory = null;
            if (!string.IsNullOrEmpty(txtMaterialCategory.Text) && txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
            {
                materialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
            }

            fGridMaterial.Cell(2, 2).Text = pi.Name;
            fGridMaterial.Cell(2, 11).Text = beginDate + " �� " + endDate;
            
            int dtlStartRowNum = 5;//ģ���е��к�
            DataSet ds = modelStock.StockInOutSrv.WZBB_Wzxhdbb(pi.Id, beginDate, endDate, materialCategory);
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            if (dt == null || dt.Rows.Count == 0) return;

            int dtlCount = dt.Rows.Count;

            //������ϸ��
            this.fGridMaterial.InsertRow(dtlStartRowNum, dtlCount);
            //���õ�Ԫ��ı߿򣬶��뷽ʽ
            FlexCell.Range range = fGridMaterial.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridMaterial.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //д����ϸ����
            for (int i = 0; i < dtlCount; i++)
            {
                DataRow dr = dt.Rows[i];
                string materialname = dr["materialname"] + "";
                string materialspec = dr["materialspec"] + "";
                string matstandardunitname = dr["matstandardunitname"] + "";
                decimal zjhsl=ClientUtil.ToDecimal(dr["zjhsl"]);//�ܼƻ�
                decimal yjhsl=ClientUtil.ToDecimal(dr["yjhsl"]);//�¼ƻ�
                decimal rjhsl=ClientUtil.ToDecimal(dr["rjhsl"]);//�ռƻ�
                decimal cgsl=ClientUtil.ToDecimal(dr["cgsl"]);//�ɹ�����
                decimal cgje=ClientUtil.ToDecimal(dr["cgje"]);//�ɹ����
                decimal cgsllj=ClientUtil.ToDecimal(dr["cgsllj"]);//�ۼƲɹ�����
                decimal cgjelj=ClientUtil.ToDecimal(dr["cgjelj"]);//�ۼƲɹ����
                decimal dbrksl=ClientUtil.ToDecimal(dr["dbrksl"]);//�����������
                decimal dbrkje=ClientUtil.ToDecimal(dr["dbrkje"]);//���������
                decimal dbrksllj=ClientUtil.ToDecimal(dr["dbrksllj"]);//�ۼƵ����������
                decimal dbrkjelj=ClientUtil.ToDecimal(dr["dbrkjelj"]);//�ۼƵ��������
                decimal xhsl=ClientUtil.ToDecimal(dr["xhsl"]);//��������
                decimal xhje=ClientUtil.ToDecimal(dr["xhje"]);//���Ľ��
                decimal xhsllj=ClientUtil.ToDecimal(dr["xhsllj"]);//�ۼ���������
                decimal xhjelj=ClientUtil.ToDecimal(dr["xhjelj"]);//�ۼ����Ľ��
                decimal htsl=ClientUtil.ToDecimal(dr["htsl"]);//��ͬ����
                decimal htje=ClientUtil.ToDecimal(dr["htje"]);//��ͬ���
                decimal zrcbsl=ClientUtil.ToDecimal(dr["zrcbsl"]);//���γɱ�����
                decimal zrcbje=ClientUtil.ToDecimal(dr["zrcbje"]);//���γɱ����
                decimal jdcbsl=ClientUtil.ToDecimal(dr["jdcbsl"]);//���ͳɱ�����
                decimal jdcbje = ClientUtil.ToDecimal(dr["jdcbje"]);//���ͳɱ����

                fGridMaterial.Cell(dtlStartRowNum + i, 1).Text = (i + 1).ToString();
                fGridMaterial.Cell(dtlStartRowNum + i, 1).Tag = "1";
                fGridMaterial.Cell(dtlStartRowNum + i, 2).Text = materialname;
                fGridMaterial.Cell(dtlStartRowNum + i, 3).Text = materialspec;
                fGridMaterial.Cell(dtlStartRowNum + i, 4).Text = matstandardunitname;
                fGridMaterial.Cell(dtlStartRowNum + i, 5).Text = zjhsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 6).Text = yjhsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 7).Text = rjhsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 8).Text = cgsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 9).Text = cgsllj.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 10).Text = cgje.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 11).Text = cgjelj.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 12).Text = dbrksl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 13).Text = dbrksllj.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 14).Text = dbrkje.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 15).Text = dbrkjelj.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 16).Text = xhsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 17).Text = xhsllj.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 18).Text = xhje.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 19).Text = xhjelj.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 20).Text = htsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 21).Text = htje.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 22).Text = zrcbsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 23).Text = zrcbje.ToString(this.formatMoney);
                fGridMaterial.Cell(dtlStartRowNum + i, 24).Text = jdcbsl.ToString(this.formatQuanttiy);
                fGridMaterial.Cell(dtlStartRowNum + i, 25).Text = jdcbje.ToString(this.formatMoney);

                //������
                if (zrcbsl != 0)
                {
                    decimal sljdl = Math.Abs(jdcbsl / zrcbsl * 100 / 100);
                    fGridMaterial.Cell(dtlStartRowNum + i, 26).Text = sljdl.ToString(this.formatMoney);
                }
                if (zrcbje != 0)
                {
                    decimal jejdl = Math.Abs(jdcbje / zrcbje * 100 / 100);
                    fGridMaterial.Cell(dtlStartRowNum + i, 27).Text = jejdl.ToString(this.formatMoney);
                }
            }
            //д��ϼ�ֵ
            this.WriteSumGridData(fGridMaterial, dtlStartRowNum, dtlStartRowNum + dtlCount-1, 5, 25);

            fGridMaterial.Cell(fGridMaterial.Rows - 1, 2).Text = "";
            fGridMaterial.Cell(fGridMaterial.Rows - 1, 24).Text = DateTime.Now.ToShortDateString();
        }
        #endregion
    }
}