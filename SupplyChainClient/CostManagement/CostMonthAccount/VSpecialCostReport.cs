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
using Microsoft.Office.Interop.Excel;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostBusinessAccount
{
    public partial class VSpecialCostReport : TBasicDataView
    {
        MCostMonthAccount model = new MCostMonthAccount();
        MStockMng stockModel = new MStockMng();
        OperationOrgInfo accountOrg = new OperationOrgInfo();//����������֯
        CostMonthAccountBill costBill = new CostMonthAccountBill();
        IList list_dataCollect = new ArrayList();
        IList list_fourcal = new ArrayList();
        IList list_material = new ArrayList();
        IList list_general = new ArrayList();

        #region ��������
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        private CurrentProjectInfo projectInfo;
        string dataCollectStr = "��Ŀ�ɱ����ݻ��ܱ�(��װ)";
        string fourCalStr = "��Ŀ����Աȱ�(��װ)";
        string materialStr = "ֱ�Ӳ��϶Աȱ�(��װ)";
        string generalStr = "�¶ȳɱ�ͨ�÷�������";
        #endregion

        public VSpecialCostReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            for (int t = 1; t <= 12; t++)
            {
                this.cboFiscalMonth.Items.Add(t + "");
            }
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
            projectInfo = StaticMethod.GetProjectInfo();
            this.cmbProject.Enabled = false;
            cmbProject.Text = projectInfo.Name;
            IList list = stockModel.StockInSrv.GetFiscalYear();
            this.cmbYear.Items.Clear();
            foreach (int iYear in list)
            {
                this.cmbYear.Items.Insert(this.cmbYear.Items.Count, iYear);
                if (iYear == ConstObject.TheLogin.TheComponentPeriod.NowYear)
                {
                    this.cmbYear.SelectedItem = this.cmbYear.Items[this.cmbYear.Items.Count - 1];
                }
            }
            
            this.cmbType.SelectedIndex = 0;
            this.RemoveTab(this.cmbType.SelectedIndex);

            this.fGrid_DataCollect.Rows = 1;
            this.fGrid_FourCal.Rows = 1;
            this.fGrid_Material.Rows = 1;
            this.fGrid_General.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            btnModelConfig.Click +=new EventHandler(btnModelConfig_Click);
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
            this.btnSelectQWBS.Click += new EventHandler(btnSelectQWBS_Click);
        }

        void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsCheck = true;
            frm.IsRootNode = true;//��������Ƿ�ֻѡ��������ϵ�ĸ��ڵ�
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                IList taskList = new ArrayList();
                string taskStr = "";
                foreach (TreeNode node in frm.SelectResult)
                {
                    GWBSTree task = node.Tag as GWBSTree;
                    taskList.Add(task.Id);
                    taskStr += task.Name + "_";
                }
                TreeNode root = frm.SelectResult[0];

                txtAccountRootNode.Text = taskStr;
                txtAccountRootNode.Tag = taskList;
            }
        }

        void btnSelectQWBS_Click(object sender, EventArgs e)
        {
            VQWBSSelect frm = new VQWBSSelect(new MQWBSManagement());
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                QWBSManage qTask = root.Tag as QWBSManage;
                if (qTask != null)
                {
                    this.txtQWBS.Text = qTask.ProjectTaskName;
                    this.txtQWBS.Tag = qTask;
                }
            }
        }

        void btnModelConfig_Click(object sender, EventArgs e)
        {
            VCostReporterConfig oVCostReporterConfig = new VCostReporterConfig();
            oVCostReporterConfig.Show();
        }
        void btnExcel_Click(object sender, EventArgs e)
        {
            string defaultName = this.cmbYear.Text + "��" + this.cboFiscalMonth.Text + "�°�װ�¶ȳɱ�������";
            string fileName = this.fGrid_DataCollect.ExportToExcel(defaultName, false, false, true);
            if (fileName == "")
                return;
            FlashScreen.Show("���ڵ����ɱ���������...");
            ApplicationClass excel = new ApplicationClass();

            //���ļ�����
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            mySheet.Name = dataCollectStr;

            int startIndex = fileName.LastIndexOf("\\") + 1;
            int endIndex = fileName.IndexOf(".x");
            dataCollectStr = fileName.Substring(startIndex, endIndex - startIndex);

            string tempName = fileName.Replace(dataCollectStr, fourCalStr);
            this.fGrid_FourCal.ExportToExcel(tempName, false, false, false);
            Workbook workbook1 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet1 = workbook1.Sheets[1] as Worksheet;
            mySheet1.Name = fourCalStr;

            tempName = fileName.Replace(dataCollectStr, materialStr);
            this.fGrid_Material.ExportToExcel(tempName, false, false, false);
            Workbook workbook2 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet2 = workbook2.Sheets[1] as Worksheet;
            mySheet2.Name = materialStr;

            //tempName = fileName.Replace(dataCollectStr, generalStr);
            //this.fGrid_General.ExportToExcel(tempName, false, false, false);
            //Workbook workbook3 = excel.Workbooks.Open(tempName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            //    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //Worksheet mySheet3 = workbook3.Sheets[1] as Worksheet;
            //mySheet3.Name = generalStr;

            try
            {
                //mySheet3.Copy(Type.Missing, mySheet);
                mySheet2.Copy(Type.Missing, mySheet);
                mySheet1.Copy(Type.Missing, mySheet);

                workbook.Save();

            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                throw new Exception("�����ɱ������������");
            }
            finally
            {
                FlashScreen.Close();
                //�رչ�������˳�Excel
                workbook.Close(false, Type.Missing, Type.Missing);
                workbook1.Close(false, Type.Missing, Type.Missing);
                workbook2.Close(false, Type.Missing, Type.Missing);
                //workbook3.Close(false, Type.Missing, Type.Missing);
                //��������ļ����ڣ���ɾ��
                if (File.Exists(fileName.Replace(dataCollectStr, fourCalStr)))
                {
                    File.Delete(fileName.Replace(dataCollectStr, fourCalStr));
                }
                if (File.Exists(fileName.Replace(dataCollectStr, materialStr)))
                {
                    File.Delete(fileName.Replace(dataCollectStr, materialStr));
                }
                //if (File.Exists(fileName.Replace(dataCollectStr, generalStr)))
                //{
                //    File.Delete(fileName.Replace(dataCollectStr, generalStr));
                //}
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("�����ɱ���������ɹ���");
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RemoveTab(this.cmbType.SelectedIndex);
            this.fGrid_DataCollect.Rows = 1;
            this.fGrid_FourCal.Rows = 1;
            this.fGrid_Material.Rows = 1;
            this.fGrid_General.Rows = 1;
        }

        private void RemoveTab(int rType)
        {
            this.tabCostReport.TabPages.Clear();
            if (rType == 0)
            {
                this.tabCostReport.TabPages.Remove(this.tabGeneral);
                this.tabCostReport.TabPages.Add(this.tabFourCal);
                this.tabCostReport.TabPages.Add(this.tabMaterial);
                this.tabCostReport.TabPages.Add(this.tabDataCollect);
            }
            else if (rType == 1)
            {
                this.tabCostReport.TabPages.Remove(this.tabGeneral);
                this.tabCostReport.TabPages.Remove(this.tabFourCal);
                this.tabCostReport.TabPages.Remove(this.tabMaterial);
                this.tabCostReport.TabPages.Add(this.tabDataCollect);
            }
            else if (rType == 2)
            {
                this.tabCostReport.TabPages.Remove(this.tabGeneral);
                this.tabCostReport.TabPages.Add(this.tabFourCal);
                this.tabCostReport.TabPages.Remove(this.tabMaterial);
                this.tabCostReport.TabPages.Remove(this.tabDataCollect);
            }
            else if (rType == 3)
            {
                this.tabCostReport.TabPages.Remove(this.tabGeneral);
                this.tabCostReport.TabPages.Remove(this.tabFourCal);
                this.tabCostReport.TabPages.Add(this.tabMaterial);
                this.tabCostReport.TabPages.Remove(this.tabDataCollect);
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if (ClientUtil.ToString(this.cmbYear.Text) == "")
            {
                MessageBox.Show("���������꣡");
                return;
            }
            if (txtAccountRootNode.Tag == null)
            {
                MessageBox.Show("��ѡ�����ڵ㣡");
                return;
            }
            LoadTempleteFile("��Ŀ�ɱ����ݻ��ܱ�(��װ).flx");
            LoadTempleteFile(materialStr + ".flx");
            //LoadTempleteFile(generalStr + ".flx");
            LoadTempleteFile(fourCalStr + ".flx");
            //�嵥WBS
            QWBSManage qTask = this.txtQWBS.Tag as QWBSManage;
            string qTaskGUID = "";
            if (qTask != null)
            {
                qTaskGUID = qTask.Id;
            }
            //��������
            IList taskList = txtAccountRootNode.Tag as ArrayList;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("Kjn", Convert.ToInt32(this.cmbYear.Text)));
            oq.AddCriterion(Expression.Eq("Kjy", Convert.ToInt32(cboFiscalMonth.Text)));
            //oq.AddCriterion(Expression.Sql(" instr('" + acctNode.SysCode + "',{alias}.accountrange)>0"));
            IList list = model.CostMonthAccSrv.GetCostMonthAccountBill(oq);
            if (list != null && list.Count > 0)
            {
                costBill = model.CombCostMonthAccountBill(list);
            }
            //costBill = model.TransCostMonthAccountBill(costBill, taskList, 10000);
            costBill = model.TransCostMonthAccountBill(costBill, taskList, 1);

            list_dataCollect.Clear();
            list_fourcal.Clear();
            list_material.Clear();
            list_general.Clear();
            //OBSManage mOBS = model.CostMonthAccSrv.GetOBSManageByTaskNode(acctNode.SysCode, projectInfo.Id);
            //if (ClientUtil.ToString(mOBS.OrgJobName) == "")
            //{
            //    MessageBox.Show("����ڵ�[" + acctNode.Name + "]û������[����OBS]��");
            //    return;
            //}
            //list_dataCollect = model.CostMonthAccSrv.GetSpecialCollectReportData(ClientUtil.ToInt(this.cmbYear.Text), ClientUtil.ToInt(this.cboFiscalMonth.Text), projectInfo.Id, mOBS.OrpJob.Id);
            list_dataCollect = model.CostMonthAccSrv.GetSpecialCollectReportData(ClientUtil.ToInt(this.cmbYear.Text), ClientUtil.ToInt(this.cboFiscalMonth.Text), projectInfo.Id, "", taskList, qTaskGUID);
 
            //��������
            this.LoadCollectData();
            this.LoadFourCalData();
            this.LoadMaterialData();
            this.LoadGeneralData();

            //�������
            CommonUtil.SetFlexGridFace(this.fGrid_DataCollect);
            CommonUtil.SetFlexGridFace(this.fGrid_FourCal);
            CommonUtil.SetFlexGridFace(this.fGrid_Material);
            CommonUtil.SetFlexGridFace(this.fGrid_General);
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //�����ʽ
                if (modelName == (dataCollectStr + ".flx"))
                {
                    fGrid_DataCollect.OpenFile(path + "\\" + modelName);//�����ʽ
                }
                else if (modelName == (fourCalStr + ".flx"))
                {
                    fGrid_FourCal.OpenFile(path + "\\" + modelName);//�����ʽ
                }
                else if (modelName == (materialStr + ".flx"))
                {
                    this.fGrid_Material.OpenFile(path + "\\" + modelName);//�����ʽ
                }
                else if (modelName == (generalStr + ".flx"))
                {
                    this.fGrid_General.OpenFile(path + "\\" + modelName);//�����ʽ
                }          
            }
            else
            {
                MessageBox.Show("δ�ҵ�ģ���ʽ�ļ�" + modelName);
                return;
            }
        }

        #region ��һ ��Ŀ�ɱ����ݻ��ܱ�
        private void LoadCollectData()
        {
            this.fGrid_DataCollect.Cell(1, 1).Text = " �� Ŀ �� �� �� �� �� �� �� ";
            this.fGrid_DataCollect.Cell(2, 2).Text = projectInfo.Name;
            this.fGrid_DataCollect.Cell(2, 7).Text = this.cmbYear.Text + "��" + this.cboFiscalMonth.Text + "��";

            int dtlStartRowNum = 5;//ģ���е��к�
            int dtlCount = list_dataCollect.Count;

            //������ϸ��
            this.fGrid_DataCollect.InsertRow(dtlStartRowNum, dtlCount);
            //���õ�Ԫ��ı߿򣬶��뷽ʽ
            FlexCell.Range range = fGrid_DataCollect.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_DataCollect.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            //д����ϸ����
            for (int i = 0; i < dtlCount; i++)
            {
                DataDomain domain = (DataDomain)list_dataCollect[i];
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 1).Text = ClientUtil.ToString(domain.Name1) + "��" + ClientUtil.ToString(domain.Name2) + "��";
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name3);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(domain.Name4);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name5);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name6);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(domain.Name7);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name8);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name9);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name10);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name11);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(domain.Name12);
                this.fGrid_DataCollect.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name13);
            }
        }
        #endregion

        #region ��� ��Ŀ����Աȱ�

        private IList CreateFourCalData()
        {
            list_fourcal.Clear();

            //1: ȡ�ÿ�Ŀ����
            IList list_temp = model.CostMonthAccSrv.QueryCostReporterConfig(projectInfo.Id, "�����Ŀ");
            foreach (CostReporterConfig rConfig in list_temp)
            {
                CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                CostAccountSubject subject = new CostAccountSubject();//�����Ŀ����
                subject.Id = rConfig.MaterialCategoryID;
                subject.Version = 0;
                subject.Code = rConfig.MaterialCategoryCode;
                subject.Level = rConfig.TLevel;
                dtlConsume.CostingSubjectGUID = subject;
                dtlConsume.CostingSubjectName = rConfig.MaterialCategoryName;
                dtlConsume.CostSubjectCode = rConfig.MaterialCategoryCode;
                dtlConsume.Data1 = rConfig.DisplayName;
                dtlConsume.Data3 = subject.Code;
                list_fourcal.Add(dtlConsume);
            }

            //����ۼ�
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume four_dtlConsume in list_fourcal)
                    {
                        if (dtlConsume.CostSubjectCode != null && dtlConsume.CostSubjectCode.Contains(four_dtlConsume.CostSubjectCode))
                        {
                            this.AddCostMonthAccDtlConsume(four_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            //this.DeleteNoDataDtlConsume(list_fourcal);
            return list_fourcal;
        }

        private void LoadFourCalData()
        {
            //��ȡԤ��������Ϣ
            if (costBill.Id == null)
                return;
            IList list_whole = model.CostMonthAccSrv.GetWholePlanData(ClientUtil.ToInt(this.cmbYear.Text), ClientUtil.ToInt(this.cboFiscalMonth.Text), projectInfo.Id, costBill.AccountRange.Id, costBill.AccountOrgGUID);
            decimal buildingArea = projectInfo.BuildingArea;//�������
            list_fourcal = CreateFourCalData();
            int dtlStartRowNum = 7;//ģ���е��к�
            int dtlCount = list_fourcal.Count;

            //������ϸ��
            this.fGrid_FourCal.InsertRow(dtlStartRowNum, dtlCount + 1);
            //���õ�Ԫ��ı߿򣬶��뷽ʽ
            FlexCell.Range range = fGrid_FourCal.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_FourCal.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGrid_FourCal.Cell(3, 2).Text = costBill.ProjectName;
            this.fGrid_FourCal.Cell(3, 10).Text = costBill.Kjn + "��" + costBill.Kjy + "��";

            //д����ϸ����
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_fourcal[i];
                fGrid_FourCal.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_FourCal.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.Data1;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 2).Tag = dtlConsume.Data3;
                fGrid_FourCal.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
                //��ǰֵ
                fGrid_FourCal.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice, 2));
                fGrid_FourCal.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice, 2));
                fGrid_FourCal.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice, 2));
                fGrid_FourCal.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice, 2));

                fGrid_FourCal.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice, 2));
                if (decimal.Round(dtlConsume.CurrIncomeTotalPrice, 2) != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrIncomeTotalPrice, 2));
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 8).Text = "0";
                }
                fGrid_FourCal.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice), 2));
                if (decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice, 2) != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrResponsiConsumeTotalPrice, 2));
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                //�ۼ�ֵ
                fGrid_FourCal.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeTotalPrice, 2));
                fGrid_FourCal.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice, 2));
                fGrid_FourCal.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice, 2));
                fGrid_FourCal.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice, 2));

                fGrid_FourCal.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice, 2));
                if (decimal.Round(dtlConsume.SumIncomeTotalPrice, 2) != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) * 100 / dtlConsume.SumIncomeTotalPrice, 2));
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 16).Text = "0";
                }
                fGrid_FourCal.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice, 2));
                if (decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice, 2) != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) * 100 / dtlConsume.SumResponsiConsumeTotalPrice, 2));
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 18).Text = "0";
                }
                string subjectCode = dtlConsume.CostSubjectCode;
                decimal wholeContractMoney = 0;
                decimal wholeResponMoney = 0;
                decimal wholePlanMoney = 0;

                foreach (DataDomain wDomain in list_whole)
                {
                    if (ClientUtil.ToString(wDomain.Name2).Contains(subjectCode))
                    {
                        wholeContractMoney += ClientUtil.ToDecimal(wDomain.Name3);
                        wholeResponMoney += ClientUtil.ToDecimal(wDomain.Name4);
                        wholePlanMoney += ClientUtil.ToDecimal(wDomain.Name6);
                    }
                }

                fGrid_FourCal.Cell(dtlStartRowNum + i, 19).Text = decimal.Round(wholeContractMoney, 2) + "";//Ԥ�������ͬ����
                fGrid_FourCal.Cell(dtlStartRowNum + i, 20).Text = decimal.Round(wholeResponMoney, 2) + "";//Ԥ���������γɱ�
                fGrid_FourCal.Cell(dtlStartRowNum + i, 21).Text = decimal.Round(wholePlanMoney, 2) + "";//Ԥ������ƻ��ϼ�
                fGrid_FourCal.Cell(dtlStartRowNum + i, 22).Text = decimal.Round((wholeContractMoney - wholePlanMoney), 2) + "";//������Ԫ��
                if (decimal.Round(wholeContractMoney, 2) != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 23).Text = decimal.Round((wholeContractMoney - wholePlanMoney) * 100 / wholeContractMoney, 2) + "";
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 23).Text = "0";//������(%)
                }
                fGrid_FourCal.Cell(dtlStartRowNum + i, 24).Text = decimal.Round((wholeResponMoney - wholePlanMoney), 2) + "";//���ɱ�����(��Ԫ)
                if (decimal.Round(wholeResponMoney, 2) != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 25).Text = decimal.Round((wholeResponMoney - wholePlanMoney) * 100 / wholeResponMoney, 2) + "";
                }
                else
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 25).Text = "0";//���ɱ�������(%)
                }

                if (decimal.Round(buildingArea, 2) != 0)
                {
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 26).Text = decimal.Round(wholeContractMoney * 10000 / buildingArea, 2) + "";//Ԥ��ÿƽ�����(Ԫ/ƽ����) ��ͬ����
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 27).Text = decimal.Round(wholeResponMoney * 10000 / buildingArea, 2) + "";
                    fGrid_FourCal.Cell(dtlStartRowNum + i, 28).Text = decimal.Round(wholePlanMoney * 10000 / buildingArea, 2) + "";
                }
            }
            //д��ϼ�ֵ
            this.WriteSumFourGridData(fGrid_FourCal, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 28);
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 8).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 7).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 4).Text), 2) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 10).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 9).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 4).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 11).Text), 2) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 16).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 15).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 11).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 12).Text), 2) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 18).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 17).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 12).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 19).Text), 2) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 23).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 22).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 19).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 20).Text), 2) != 0)
            {
                fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 25).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 24).Text) * 100 / ClientUtil.ToDecimal(fGrid_FourCal.Cell(dtlStartRowNum + dtlCount, 20).Text), 2) + "";
            }
        }

        private void WriteSumFourGridData(CustomFlexGrid flexGrid, int startRow, int endRow, int startCol, int endCol)
        {
            flexGrid.Cell(endRow + 1, startCol - 1).Text = "�ϼƣ�";
            for (int i = startCol; i <= endCol; i++)
            {
                IList list_subjectCode = new ArrayList();
                decimal sumValue = 0;    
                for (int t = startRow; t <= endRow; t++)
                {
                    string subjectCode = ClientUtil.ToString(flexGrid.Cell(t, 2).Tag);
                    bool ifAdd = true;
                    foreach (string s_code in list_subjectCode)
                    {
                        if (s_code.Contains(subjectCode) || subjectCode.Contains(s_code))
                        {
                            ifAdd = false;
                        }
                    }
                    if (ifAdd == true)
                    {
                        list_subjectCode.Add(subjectCode);
                        sumValue += ClientUtil.ToDecimal(flexGrid.Cell(t, i).Text);
                    }
                }
                flexGrid.Cell(endRow + 1, i).Text = sumValue + "";
            }
            FlexCell.Range range = flexGrid.Range(endRow + 1, 1, endRow + 1, endCol);
            CommonUtil.SetFlexGridDetailFormat(range);
            flexGrid.Cell(endRow + 1, startCol - 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
        }
        #endregion

        #region ���� ֱ�Ӳ��϶Աȱ�
        private IList CreateMaterialData()
        {
            this.list_material.Clear();
            //1: ȡ����Դ���༯��
            IList list_temp = model.CostMonthAccSrv.QueryCostReporterConfig(projectInfo.Id, "��Դ����");
            foreach (CostReporterConfig rConfig in list_temp)
            {
                CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                dtlConsume.ResourceTypeGUID = rConfig.MaterialCategoryID;
                dtlConsume.ResourceTypeName = rConfig.MaterialCategoryName;
                dtlConsume.ResourceTypeCode = rConfig.MaterialCategoryCode;
                dtlConsume.Data1 = rConfig.DisplayName;
                list_material.Add(dtlConsume);
            }
            //����ۼ�
            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                {
                    foreach (CostMonthAccDtlConsume m_dtlConsume in list_material)
                    {
                        if (dtlConsume.ResourceTypeCode != null && dtlConsume.ResourceTypeCode.Contains(m_dtlConsume.ResourceTypeCode))
                        {
                            this.AddCostMonthAccDtlConsume(m_dtlConsume, dtlConsume);
                        }
                    }
                }
            }
            //this.DeleteNoDataDtlConsume(list_material);
            return list_material;
        }

        private void LoadMaterialData()
        {
            list_material = CreateMaterialData();
            int dtlStartRowNum = 7;//ģ���е��к�
            int dtlCount = this.list_material.Count;

            //������ϸ��
            this.fGrid_Material.InsertRow(dtlStartRowNum, dtlCount + 1);
            //���õ�Ԫ��ı߿򣬶��뷽ʽ
            FlexCell.Range range = fGrid_Material.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGrid_Material.Cols - 1);
            CommonUtil.SetFlexGridDetailFormat(range);

            this.fGrid_Material.Cell(3, 2).Text = costBill.ProjectName;
            this.fGrid_Material.Cell(3, 10).Text = costBill.Kjn + "��" + costBill.Kjy + "��";

            //д����ϸ����
            for (int i = 0; i < dtlCount; i++)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)list_material[i];

                fGrid_Material.Cell(dtlStartRowNum + i, 1).Text = (i + 1) + "";
                fGrid_Material.Cell(dtlStartRowNum + i, 1).Tag = dtlConsume.Data2;
                fGrid_Material.Cell(dtlStartRowNum + i, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                fGrid_Material.Cell(dtlStartRowNum + i, 2).Text = dtlConsume.Data1;
                fGrid_Material.Cell(dtlStartRowNum + i, 2).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                //��ǰ��ͬ����
                fGrid_Material.Cell(dtlStartRowNum + i, 3).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeQuantity, 2));
                if (dtlConsume.CurrIncomeQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice / dtlConsume.CurrIncomeQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 4).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice, 2));
                //��ǰ���γɱ�
                fGrid_Material.Cell(dtlStartRowNum + i, 6).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeQuantity, 2));
                if (dtlConsume.CurrResponsiConsumeQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice / dtlConsume.CurrResponsiConsumeQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 7).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrResponsiConsumeTotalPrice, 2));
                //��ǰ�ƻ��ɱ�
                fGrid_Material.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanQuantity, 2));
                if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice / dtlConsume.CurrRealConsumePlanQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 10).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumePlanTotalPrice, 2));
                //��ǰʵ�ʳɱ�
                fGrid_Material.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeQuantity, 2));
                if (dtlConsume.CurrRealConsumeQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice / dtlConsume.CurrRealConsumeQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 13).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrRealConsumeTotalPrice, 2));
                //��������
                fGrid_Material.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(decimal.Round(dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice, 2));
                if (dtlConsume.CurrIncomeTotalPrice != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrIncomeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrIncomeTotalPrice, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 16).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice), 2));
                if (dtlConsume.CurrResponsiConsumeTotalPrice != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(decimal.Round((dtlConsume.CurrResponsiConsumeTotalPrice - dtlConsume.CurrRealConsumeTotalPrice) * 100 / dtlConsume.CurrResponsiConsumeTotalPrice, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 18).Text = "0";
                }
                //�ۼƺ�ͬ����
                fGrid_Material.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeQuantity, 2));
                if (dtlConsume.SumIncomeQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeTotalPrice / dtlConsume.SumIncomeQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 20).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 21).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumIncomeTotalPrice, 2));
                //�ۼ����γɱ�
                fGrid_Material.Cell(dtlStartRowNum + i, 22).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeQuantity, 2));
                if (dtlConsume.SumResponsiConsumeQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 23).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice / dtlConsume.SumResponsiConsumeQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 23).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 24).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumResponsiConsumeTotalPrice, 2));
                //�ۼƼƻ��ɱ�
                fGrid_Material.Cell(dtlStartRowNum + i, 25).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanQuantity, 2));
                if (dtlConsume.SumRealConsumePlanQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 26).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice / dtlConsume.SumRealConsumePlanQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 26).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 27).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumePlanTotalPrice, 2));
                //�ۼ�ʵ�ʳɱ�
                fGrid_Material.Cell(dtlStartRowNum + i, 28).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeQuantity, 2));
                if (dtlConsume.SumRealConsumeQuantity != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 29).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice / dtlConsume.SumRealConsumeQuantity, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 29).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 30).Text = ClientUtil.ToString(decimal.Round(dtlConsume.SumRealConsumeTotalPrice, 2));
                //�ۼ�����
                fGrid_Material.Cell(dtlStartRowNum + i, 31).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice), 2));
                if (dtlConsume.SumIncomeTotalPrice != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 32).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumIncomeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) * 100 / dtlConsume.SumIncomeTotalPrice, 4));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 32).Text = "0";
                }
                fGrid_Material.Cell(dtlStartRowNum + i, 33).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice), 2));
                if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 34).Text = ClientUtil.ToString(decimal.Round((dtlConsume.SumResponsiConsumeTotalPrice - dtlConsume.SumRealConsumeTotalPrice) * 100 / dtlConsume.SumResponsiConsumeTotalPrice, 2));
                }
                else
                {
                    fGrid_Material.Cell(dtlStartRowNum + i, 34).Text = "0";
                }
            }
            //д��ϼ�ֵ
            if (dtlCount > 0)
            {
                this.WriteSumGridData(fGrid_Material, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 3, 34);
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 4).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 5).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 3).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 6).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 7).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 8).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 6).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 9).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 10).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 11).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 9).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 12).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 13).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 14).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 12).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 16).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 15).Text) * 100/ ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 5).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 18).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 17).Text) * 100/ ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 8).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 19).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 20).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 21).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 19).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 22).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 23).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 24).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 22).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 25).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 26).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 27).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 25).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 28).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 29).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 30).Text) / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 28).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 21).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 32).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 31).Text) * 100 / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 21).Text), 2) + "";
            }
            if (decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 24).Text), 2) != 0)
            {
                fGrid_Material.Cell(dtlStartRowNum + dtlCount, 34).Text = decimal.Round(ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 33).Text) * 100 / ClientUtil.ToDecimal(fGrid_Material.Cell(dtlStartRowNum + dtlCount, 24).Text), 2) + "";
            }
        }
        #endregion

        #region ���� �ɱ�����ͨ�ñ���
        private void LoadGeneralData()
        {
            
        }
        #endregion

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
                    sumValue += ClientUtil.ToDecimal(flexGrid.Cell(t, i).Text);
                }
                flexGrid.Cell(endRow + 1, i).Text = sumValue + "";
            }
            FlexCell.Range range = flexGrid.Range(endRow + 1, 1, endRow + 1, endCol);
            CommonUtil.SetFlexGridDetailFormat(range);
            flexGrid.Cell(endRow + 1, startCol - 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
        }

        private CostMonthAccDtlConsume AddCostMonthAccDtlConsume(CostMonthAccDtlConsume dtlConsume, CostMonthAccDtlConsume addDtlConsume)
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

        //�޳�û�����ݵļ�¼
        private IList DeleteNoDataDtlConsume(IList list)
        {
            //�޳�û�����ݵļ�¼
            IList l_temp = new ArrayList();
            foreach (CostMonthAccDtlConsume dtlConsume in list)
            {
                if (dtlConsume.CurrRealConsumeTotalPrice == 0 && dtlConsume.CurrIncomeTotalPrice == 0)
                {
                    l_temp.Add(dtlConsume);
                }
            }
            foreach (CostMonthAccDtlConsume t_dtlConsume in l_temp)
            {
                list.Remove(t_dtlConsume);
            }
            return list;
        }

        #endregion
    }
}