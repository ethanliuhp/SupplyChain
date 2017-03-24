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
    public partial class VAllProjectStateReport : TBasicDataView
    {
        MProjectDepartment model = new MProjectDepartment();

        string orgSyscode = "";
        private CurrentProjectInfo ProjectInfo;
        string detailExptr = "��˾��Ŀ����״̬һ����";
        IList list = new ArrayList();
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();

        public VAllProjectStateReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-30);
            dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;

            ProjectInfo = StaticMethod.GetProjectInfo();
            if (ProjectInfo != null && ProjectInfo.Code != CommonUtil.CompanyProjectCode)
            {

                this.lblPSelect.Visible = false;
                this.txtOperationOrg.Visible = false;
                this.btnOperationOrg.Visible = false;
            }

            this.fGridDetail.Rows = 1;
        }

        private void InitEvents()
        {
            btnQuery.Click+=new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.fGridDetail.HyperLinkClick += new FlexCell.Grid.HyperLinkClickEventHandler(btnHyperLinkClick);
        }


        void btnHyperLinkClick(object sender, FlexCell.Grid.HyperLinkClickEventArgs e)
        {
            int currRow = e.Row;
            int currCol = e.Col;
            e.URL = "";
            e.Changed = true;
            string projectID = fGridDetail.Cell(currRow, currCol).Tag as string;
            string projectName = fGridDetail.Cell(currRow, currCol).Text;
            VProjectStateReport vReport = new VProjectStateReport(true, projectID, projectName, startDate, endDate);
            vReport.Width = 1150;
            vReport.Height = 620;
            vReport.ShowDialog();
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
            fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //�����ʽ
                if (modelName == "��˾��Ŀ����״̬һ����.flx")
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

            for (int i = startCol; i <= endCol; i++)
            {
                string oldValue = "0";
                for (int t = startRow; t <= endRow; t++)
                {
                    string currValue = flexGrid.Cell(t, i).Text;
                    if (oldValue == "0")
                    {
                        oldValue = currValue;
                    }
                    else
                    {
                        if (oldValue == currValue)
                        {
                            flexGrid.Range(t - 1, i, t, i).Merge();
                        }
                        oldValue = currValue;
                    }
                }
            }
        }

        #endregion

        #region ��Ŀʹ��״̬��ϸ��
       
        private void LoadDetailFile()
        {
            OperationOrgInfo orgInfo = this.txtOperationOrg.Tag as OperationOrgInfo;
            FlashScreen.Show("��������[��Ŀʹ��״̬]����...");

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            try
            {
                startDate = dtpDateBegin.Value.Date;
                endDate = dtpDateEnd.Value.Date;
                if (info == null)
                {
                    list = model.CurrentSrv.QueryAllProjectStateInfo("", dtpDateBegin.Value.Date, dtpDateEnd.Value.Date);
                }
                else
                {
                    list = model.CurrentSrv.QueryAllProjectStateInfo(info.Id, dtpDateBegin.Value.Date, dtpDateEnd.Value.Date);
                }
                LoadTotalFile();
            }
            catch (Exception e1)
            {
                throw new Exception("����[��Ŀʹ��״̬]�����쳣[" + e1.Message + "]");
            }
            finally
            {
                FlashScreen.Close();
            }
            
        }

        #endregion

        #region ��Ŀʹ��״̬�ۺϱ�

        private void LoadTotalFile()
        {
            fGridDetail.AutoRedraw = false;
            int dtlStartRowNum = 6;//ģ���е��к�
            int dtlCount = list.Count;

            //������ϸ��
            this.fGridDetail.InsertRow(dtlStartRowNum, dtlCount);
            //���õ�Ԫ��ı߿򣬶��뷽ʽ
            FlexCell.Range range = fGridDetail.Range(dtlStartRowNum, 1, dtlStartRowNum + dtlCount, fGridDetail.Cols - 1);
            CommonUtil.SetFlexGridDetailCenter(range);

            OperationOrgInfo info = txtOperationOrg.Tag as OperationOrgInfo;
            if (info == null || ClientUtil.ToString(info.Id) == "")
            {
                this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() + "��" + dtpDateEnd.Value.Date.ToShortDateString() + " [��˾]��Ŀʹ��״̬ͳ�Ʊ���";
            }
            else
            {
                this.fGridDetail.Cell(1, 1).Text = dtpDateBegin.Value.Date.ToShortDateString() + "��" + dtpDateEnd.Value.Date.ToShortDateString() + " [" + info.Name + "]��Ŀʹ��״̬ͳ�Ʊ���";
            }
            
            int i = 0;
            IList subList = new ArrayList();
            IList yearList = new ArrayList();
            foreach (DataDomain domain in list)
            {
                fGridDetail.Cell(dtlStartRowNum + i, 1).Text = ClientUtil.ToString(domain.Name4);//�ֹ�˾
                fGridDetail.Cell(dtlStartRowNum + i, 2).Text = ClientUtil.ToString(domain.Name3);//���
                fGridDetail.Cell(dtlStartRowNum + i, 5).Tag = ClientUtil.ToString(domain.Name3);//���
                fGridDetail.Cell(dtlStartRowNum + i, 3).Text = "";//
                fGridDetail.Cell(dtlStartRowNum + i, 4).Text = ClientUtil.ToString(domain.Name2);//��Ŀ����
                fGridDetail.Cell(dtlStartRowNum + i, 4).Tag = ClientUtil.ToString(domain.Name1);
                fGridDetail.Cell(dtlStartRowNum + i, 4).CellType = FlexCell.CellTypeEnum.HyperLink;
                fGridDetail.Cell(dtlStartRowNum + i, 5).Text = ClientUtil.ToString(domain.Name30);//�Ƿ���
                if (ClientUtil.ToString(domain.Name30) == "��")
                {
                    fGridDetail.Cell(dtlStartRowNum + i, 5).ForeColor = System.Drawing.Color.Red;
                }
                bool ifHaveSub = false;
                foreach (DataDomain data in subList)
                {
                    if (ClientUtil.ToString(domain.Name4) == ClientUtil.ToString(data.Name4))
                    {
                        if (ClientUtil.ToString(domain.Name30) == "��")
                        {
                            data.Name2 = ClientUtil.ToDecimal(data.Name2) + 1 + "";//���ϸ�����
                        }
                        else
                        {
                            data.Name1 = ClientUtil.ToDecimal(data.Name1) + 1 + "";//�ϸ�����
                        }
                        ifHaveSub = true;
                    }
                }
                if (ifHaveSub == false)
                {
                    DataDomain sub = new DataDomain();
                    sub.Name4 = ClientUtil.ToString(domain.Name4);
                    if (ClientUtil.ToString(domain.Name30) == "��")
                    {
                        sub.Name1 = "0";//�ϸ�����
                        sub.Name2 = "1";//���ϸ�����
                    }
                    else
                    {
                        sub.Name1 = "1";
                        sub.Name2 = "0";
                    }
                    subList.Add(sub);
                }

                bool ifHaveYear = false;
                foreach (DataDomain data in yearList)
                {
                    if (ClientUtil.ToString(domain.Name3) == ClientUtil.ToString(data.Name3))
                    {
                        if (ClientUtil.ToString(domain.Name30) == "��")
                        {
                            data.Name2 = ClientUtil.ToDecimal(data.Name2) + 1 + "";//���ϸ�����
                        }
                        else
                        {
                            data.Name1 = ClientUtil.ToDecimal(data.Name1) + 1 + "";//�ϸ�����
                        }
                        ifHaveYear = true;
                    }
                }
                if (ifHaveYear == false)
                {
                    DataDomain sub = new DataDomain();
                    sub.Name3 = ClientUtil.ToString(domain.Name3);
                    if (ClientUtil.ToString(domain.Name30) == "��")
                    {
                        sub.Name1 = "0";//�ϸ�����
                        sub.Name2 = "1";//���ϸ�����
                    }
                    else
                    {
                        sub.Name1 = "1";
                        sub.Name2 = "0";
                    }
                    yearList.Add(sub);
                }
                
                fGridDetail.Cell(dtlStartRowNum + i, 7).Text = ClientUtil.ToString(domain.Name5);//��Ա��λ
                fGridDetail.Cell(dtlStartRowNum + i, 8).Text = ClientUtil.ToString(domain.Name6);//������Ϣ����
                fGridDetail.Cell(dtlStartRowNum + i, 9).Text = ClientUtil.ToString(domain.Name7);//���̲�λ������
                fGridDetail.Cell(dtlStartRowNum + i, 10).Text = ClientUtil.ToString(domain.Name8);//ʩ�����������
                fGridDetail.Cell(dtlStartRowNum + i, 11).Text = ClientUtil.ToString(domain.Name9);//�ɱ�Ԥ����Ϣ����
                fGridDetail.Cell(dtlStartRowNum + i, 12).Text = ClientUtil.ToString(domain.Name10);//�����ᱨ������
                fGridDetail.Cell(dtlStartRowNum + i, 13).Text = ClientUtil.ToString(domain.Name11);//���񹤵�����
                fGridDetail.Cell(dtlStartRowNum + i, 14).Text = ClientUtil.ToString(domain.Name12);//������/��/���
                fGridDetail.Cell(dtlStartRowNum + i, 15).Text = ClientUtil.ToString(domain.Name13);//�ְ����㵥
                fGridDetail.Cell(dtlStartRowNum + i, 16).Text = ClientUtil.ToString(domain.Name14);//����/��ȫ/�������
                fGridDetail.Cell(dtlStartRowNum + i, 17).Text = ClientUtil.ToString(domain.Name15);//������ý���
                fGridDetail.Cell(dtlStartRowNum + i, 18).Text = ClientUtil.ToString(domain.Name16);//�豸���޽���
                fGridDetail.Cell(dtlStartRowNum + i, 19).Text = ClientUtil.ToString(domain.Name17);//����ʵ�ʺ��ý���
                fGridDetail.Cell(dtlStartRowNum + i, 20).Text = ClientUtil.ToString(domain.Name18);//�¶ȳɱ���������
                i++;
            }
            string str = " ͳ����Ŀһ��[" + list.Count + "]��������";
            foreach (DataDomain domain in subList)
            {
                int okProject = ClientUtil.ToInt(domain.Name1);
                int noProject = ClientUtil.ToInt(domain.Name2);
                str += " [" + ClientUtil.ToString(domain.Name4) + "��" + (okProject + noProject) + "�����ϸ���Ŀ: " + okProject + "�������ϸ���Ŀ: " + noProject + "] ";
            }
            str += "\r\n �����ͳ�ƣ�����";
            foreach (DataDomain domain in yearList)
            {
                int okProject = ClientUtil.ToInt(domain.Name1);
                int noProject = ClientUtil.ToInt(domain.Name2);
                str += " [" + ClientUtil.ToString(domain.Name3) + "��" + (okProject + noProject) + "�����ϸ���Ŀ: " + okProject + "�������ϸ���Ŀ: " + noProject + "] ";
            }
            this.fGridDetail.Cell(2, 1).Text = str;
            this.fGridDetail.Row(2).AutoFit();

            //��ʽ����
            for (int t = dtlStartRowNum; t <= dtlStartRowNum + dtlCount; t++)
            {
                fGridDetail.Row(t).AutoFit();
            }
            fGridDetail.Column(4).AutoFit();
            WriteSumGridData(fGridDetail, dtlStartRowNum, dtlStartRowNum + dtlCount - 1, 1, 2);
            //�������
            string oldValue = "0";
            int tt = 1;
            for (int t = dtlStartRowNum; t <= dtlStartRowNum + dtlCount - 1; t++)
            {
                string currValue = fGridDetail.Cell(t, 5).Tag as string;
                if (oldValue == "0")
                {
                    oldValue = currValue;
                }
                else
                {
                    if (oldValue == currValue)
                    {
                        tt++;
                    }
                    else
                    {
                        tt = 1;
                    }
                    oldValue = currValue;
                }
                fGridDetail.Cell(t, 3).Text = tt + "";
            }

            fGridDetail.AutoRedraw = true;
            fGridDetail.Refresh();

        }
        #endregion
    }
}