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
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    public partial class VProjectDataInfo : TBasicDataView
    {
        MProjectDepartment model = new MProjectDepartment();

        string orgSyscode = "";
        private CurrentProjectInfo ProjectInfo;
        MStockMng stockModel = new MStockMng();
        #region ��������
        private string loginPersonName = ConstObject.LoginPersonInfo.Name;
        private string loginDate = ConstObject.LoginDate.ToShortDateString();
        string detailExptr = "��Ŀ�ۺ�����ͳ�Ʊ�";
        string formatQuanttiy = "################0.###";
        string formatMoney = "################0.##";
        #endregion
        #region �������
        Hashtable htData = new Hashtable();
        
        #endregion

        public VProjectDataInfo()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            ProjectInfo = StaticMethod.GetProjectInfo();
            if (ProjectInfo != null && ProjectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                this.txtProject.Text = ProjectInfo.Name;
                this.txtProject.Tag = ProjectInfo.Id;

                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
            }

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

            for (int i = 1; i < 13; i++)
            {
                this.cboFiscalMonth.Items.Add(i);
            }
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();

            this.fGridDetail.Rows = 1;
           
        }

        private void InitEvents()
        {
            btnQuery.Click+=new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.btnPreview.Click += new EventHandler(btnPreview_Click);
        }

        void btnPreview_Click(object sender, EventArgs e)
        {

            FlexCell.PageSetup pageSetup = fGridDetail.PageSetup;
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            if (pd.PrinterSettings.IsValid)
            {
                for (int t = pd.PrinterSettings.PaperSizes.Count - 1; t > 0; t--)
                    if (pd.PrinterSettings.PaperSizes[t].Kind == System.Drawing.Printing.PaperKind.A4)
                    {
                        fGridDetail.PageSetup.PaperSize = pd.PrinterSettings.PaperSizes[t];
                        pd.Dispose();
                    }
            }
            pageSetup.Landscape = true;
            pageSetup.RightMargin = ClientUtil.Tofloat("0.1");
            pageSetup.LeftMargin = ClientUtil.Tofloat("0.1");
            pageSetup.TopMargin = ClientUtil.Tofloat("0.1");
            pageSetup.BottomMargin = ClientUtil.Tofloat("0.1");
            //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("�Զ���ֽ��", 1000, 800);
            //pageSetup.PaperSize = paperSize;
            this.fGridDetail.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            string opgId = "";
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = info;
                txtOperationOrg.Text = info.Name;
                opgId = info.Id;
            }
            DataDomain domain = model.CurrentSrv.GetProjectInfoByOpgId(opgId);
            if (TransUtil.ToString(domain.Name1) != "")
            {
                this.txtProject.Text = TransUtil.ToString(domain.Name2);
                this.txtProject.Tag = TransUtil.ToString(domain.Name1);
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(detailExptr + ".flx");

            //��������
            this.LoadDetailFile();

            //�������
            //fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //�����ʽ
                if (modelName == "��Ŀ�ۺ�����ͳ�Ʊ�.flx")
                {                   
                    fGridDetail.OpenFile(path + "\\" + modelName);//�����ʽ
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

        #endregion

        #region ��Ŀʹ��״̬��ϸ��
       
        private void LoadDetailFile()
        {
            FlexCell.Chart chart;
            FlexCell.Chart addChart;
            fGridDetail.AutoRedraw = false;
            fGridDetail.Row(0).Visible = false;
            for (int i = 1; i <= 9; i++)
            {
                if (i == 6)
                {
                    fGridDetail.Column(i).Width = 50;
                }
                else
                {
                    fGridDetail.Column(i).Width = 100;
                }
            }

            string projectID = this.txtProject.Tag as string;
            if (projectID == null)
            {
                MessageBox.Show("��ѡ����Ŀ��");
                return;
            }
            FlashScreen.Show("��������[��Ŀ�ۺ�����]ͳ��...");

            DataDomain domain = new DataDomain();
            try
            {
                int fiscalYear = ClientUtil.ToInt(cmbYear.Text);
                int fiscalMonth = ClientUtil.ToInt(this.cboFiscalMonth.Text);

                htData = model.CurrentSrv.QueryProjectDataInfo(projectID, fiscalYear, fiscalMonth);
                domain = (DataDomain)htData["businessinfo"];
                if (domain == null)
                    return;
                fGridDetail.Cell(1, 1).Text = fiscalYear + "��" + fiscalMonth +  "��" + this.txtProject.Text + "��Ŀ�ۺ�����";

                //�������
                decimal currRealMoney = ClientUtil.ToDecimal(domain.Name1) + ClientUtil.ToDecimal(domain.Name2)
                   + ClientUtil.ToDecimal(domain.Name3) + ClientUtil.ToDecimal(domain.Name4);
                decimal addRealMoney = ClientUtil.ToDecimal(domain.Name5) + ClientUtil.ToDecimal(domain.Name6)
                    + ClientUtil.ToDecimal(domain.Name7) + ClientUtil.ToDecimal(domain.Name8);
                fGridDetail.Cell(4, 1).Text = ClientUtil.ToString(domain.Name1);//���ڷְ�������
                fGridDetail.Cell(4, 2).Text = ClientUtil.ToString(domain.Name2);//���ڲ������Ľ��
                fGridDetail.Cell(4, 3).Text = ClientUtil.ToString(domain.Name3);//�����豸���޽��
                fGridDetail.Cell(4, 4).Text = ClientUtil.ToString(domain.Name4);//���ڲ�����ý��
                fGridDetail.Cell(4, 5).Text = currRealMoney + "";
                fGridDetail.Cell(16, 1).Text = ClientUtil.ToString(domain.Name5);//�ۼƷְ�������
                fGridDetail.Cell(16, 2).Text = ClientUtil.ToString(domain.Name6);//�ۼƲ������Ľ��
                fGridDetail.Cell(16, 3).Text = ClientUtil.ToString(domain.Name7);//�ۼ��豸���޽��
                fGridDetail.Cell(16, 4).Text = ClientUtil.ToString(domain.Name8);//�ۼƲ�����ý��
                fGridDetail.Cell(16, 5).Text = addRealMoney + "";

                fGridDetail.Cell(27, 1).Text = "ҵ��ȷ�Ͻ�" + ClientUtil.ToDecimal(domain.Name12).ToString("f2");//�ۼ�ҵ��ȷ�Ͻ��
                fGridDetail.Cell(27, 3).Text = "�տ��" + ClientUtil.ToDecimal(domain.Name13).ToString("f2");//�ۼ��տ���
                fGridDetail.Cell(28, 1).Text = "�ۼ�ʵ�ʳɱ���" + addRealMoney.ToString("f2");//�ۼ�ҵ��ȷ�Ͻ��
                if (addRealMoney != 0)
                {
                    fGridDetail.Cell(29, 1).Text = "ȷȨ��[(ҵ��ȷ�Ͻ��/�ۼ�ʵ�ʳɱ�)*100%]��" + Decimal.Round(ClientUtil.ToDecimal(domain.Name12) * 100 / addRealMoney, 2) + "%";//ȷȨ��
                }
                else
                {
                    fGridDetail.Cell(29, 1).Text = "ȷȨ��[(ҵ��ȷ�Ͻ��/�ۼ�ʵ�ʳɱ�)*100%]��0"; 
                }
                DataDomain matoutDomain = (DataDomain)htData["materialoutinfo"];
                DataDomain matSupplyDomain = (DataDomain)htData["matsupplyinfo"];
                fGridDetail.Cell(4, 8).Text = ClientUtil.ToString(matSupplyDomain.Name1);//���ڸֽ������
                fGridDetail.Cell(4, 9).Text = ClientUtil.ToString(matoutDomain.Name2);//���ڻ�����������
                fGridDetail.Cell(4, 10).Text = ClientUtil.ToString(matoutDomain.Name3);//�����Ͼ����޽��
                fGridDetail.Cell(4, 11).Text = ClientUtil.ToString(matSupplyDomain.Name3);//��������������
                fGridDetail.Cell(17, 8).Text = ClientUtil.ToString(matSupplyDomain.Name4);//�ۼƸֽ������
                fGridDetail.Cell(17, 9).Text = ClientUtil.ToString(matoutDomain.Name6);//�ۼƻ�����������
                fGridDetail.Cell(17, 10).Text = ClientUtil.ToString(matoutDomain.Name7);//�ۼ��Ͼ����޽��
                fGridDetail.Cell(17, 11).Text = ClientUtil.ToString(matSupplyDomain.Name6);//�ۼ�����������
              
                fGridDetail.Cell(5, 8).Text = ClientUtil.ToString(matoutDomain.Name1);//���ڸֽ����Ľ��
                fGridDetail.Cell(5, 9).Text = ClientUtil.ToString(matoutDomain.Name2);//���ڻ��������Ľ��
                fGridDetail.Cell(5, 10).Text = ClientUtil.ToString(matoutDomain.Name3);//�����Ͼ����޽��
                fGridDetail.Cell(5, 11).Text = ClientUtil.ToString(matoutDomain.Name4);//�����������Ľ��
                fGridDetail.Cell(18, 8).Text = ClientUtil.ToString(matoutDomain.Name5);//�ۼƸֽ����Ľ��
                fGridDetail.Cell(18, 9).Text = ClientUtil.ToString(matoutDomain.Name6);//�ۼƻ��������Ľ��
                fGridDetail.Cell(18, 10).Text = ClientUtil.ToString(matoutDomain.Name7);//�ۼ��Ͼ����޽��
                fGridDetail.Cell(18, 11).Text = ClientUtil.ToString(matoutDomain.Name8);//�ۼ��������Ľ��
                
                // ���һ��3D��ͼ(����)
                fGridDetail.AddChart(5, 1);
                chart = fGridDetail.Chart(5, 1);
                chart.SetDataSource(3, 1, 4, 4);
                chart.ChartType = FlexCell.Chart.ChartTypeEnum.Pie3D;
                chart.DisplayDataLabels = true;
                chart.ScaleFont = false;

                // ���һ��3D��ͼ(�ۼ�)
                fGridDetail.AddChart(17, 1);
                addChart = fGridDetail.Chart(17, 1);
                addChart.SetDataSource(15, 1, 16, 4);
                addChart.ChartType = FlexCell.Chart.ChartTypeEnum.Pie3D;
                addChart.DisplayDataLabels = true;
                addChart.ScaleFont = false;

                // ���һ��3D����ͼ��(����)
                fGridDetail.AddChart(6, 7);
                chart = fGridDetail.Chart(6, 7);
                chart.SetDataSource(3, 7, 5, 11);
                chart.PlotBy = FlexCell.Chart.PlotTypeEnum.Rows;
                chart.ChartType = FlexCell.Chart.ChartTypeEnum.ColumnClustered3D;
                chart.ScaleFont = false;

                // ���һ��3D����ͼ��(�ۼ�)
                fGridDetail.AddChart(19, 7);
                chart = fGridDetail.Chart(19, 7);
                chart.SetDataSource(16, 7, 18, 11);
                chart.PlotBy = FlexCell.Chart.PlotTypeEnum.Rows;
                chart.ChartType = FlexCell.Chart.ChartTypeEnum.ColumnClustered3D;
                chart.ScaleFont = false;
            }
            catch (Exception e1)
            {
                throw new Exception("����[��Ŀ�ۺ�����]ͳ���쳣[" + e1.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }

            fGridDetail.PageSetup.CenterVertically = true;
            fGridDetail.PageSetup.CenterHorizontally = true;
            fGridDetail.AutoRedraw = true;
            fGridDetail.Refresh();
        }

        #endregion
    }
}