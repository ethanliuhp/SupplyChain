using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using IRPServiceModel.Services.Common;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Linq;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VSceneManagementReport : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string detailExptr = "�ֳ�����ѷ����Աȱ�";
        string flexName = "�ֳ�����ѷ����Աȱ�.flx";

        string detailExptrMeasures = "��ʩ�ѳɱ������Աȱ�";
        string flexNameMeasures = "��ʩ�ѳɱ������Աȱ�.flx";


        CurrentProjectInfo projectInfo;

        public VSceneManagementReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            #region   ����ݺ��·ݵ������б��ֵ
            this.comYear.Items.Clear();
            this.comMonth.Items.Clear();
            for (int i = 0; i < 13; i++)
            {
                this.comYear.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 6));
            }
            for (int i = 1; i < 13; i++)
            {
                this.comMonth.Items.Add(i);
            }
            this.comYear.Text = DateTime.Now.Year.ToString();
            this.comMonth.Text = DateTime.Now.Month.ToString();
            #endregion

            this.fGridDetail.Rows = 1;

            this.fGridMeasures.Rows = 1;//��ʩ�ѵ��б�չʾ
           
            LoadTempleteFile(flexName);//�ֳ�����ѷ����Աȱ�
            LoadTempleteFileMeasures(flexNameMeasures);//��ʩ�ѳɱ������Աȱ�

            projectInfo = StaticMethod.GetProjectInfo();//��Ŀ�����Ϣ
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
        }


        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);

            fGridMeasures.ExportToExcel(detailExptrMeasures, false, false, true);//20160822
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //�����ʽ
                if (modelName == flexName)
                {
                    fGridDetail.OpenFile(path + "\\" + modelName);//�����ʽ
                }
            }
            else
            {
                MessageBox.Show("δ�ҵ�ģ���ʽ�ļ�" + modelName);
                return;
            }
        }


        private void LoadTempleteFileMeasures(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //�����ʽ
                if (modelName == flexNameMeasures)
                {
                    fGridMeasures.OpenFile(path + "\\" + modelName);//�����ʽ
                }
            }
            else
            {
                MessageBox.Show("δ�ҵ�ģ���ʽ�ļ�" + modelName);
                return;
            }
        }




        private void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("��������[" + detailExptr + "]����...");
            LoadTempleteFile(flexName);
            LoadDetailFile();

            LoadTempleteFileMeasures(flexNameMeasures);//��ʩ��
            LoadDetailFileMeasures();//��ʩ��
            FlashScreen.Close();
        }


        private void LoadDetailFileMeasures()
        {
           // FlashScreen.Show("��������[" + detailExptrMeasures + "]����...");
            try
            {
                fGridMeasures.AutoRedraw = false;


                #region  ��װ��ѯ���� ���ֳ�����ѷ����Աȱ�
                var condition = " and t1.theprojectguid='" + projectInfo.Id + "'  and t3.COSTSUBJECTCODE LIKE  'C512%'          ";//��ʩ��C512
                if (!string.IsNullOrEmpty(this.comYear.Text))//���
                {
                    condition += " and t1.kjn='" + this.comYear.Text + "'";
                }
                if (!string.IsNullOrEmpty(this.comMonth.Text))//�·�
                {
                    condition += " and t1.kjy='" + this.comMonth.Text + "'";
                }

                #endregion

                #region ��Ҫȡ����ֵ���ֳ�����ѷ����Աȱ�


                //��Ŀ����(��Ӧ projectInfo.Name)     ����(���������)
                //��ţ��Լ���ţ�    ������Ŀ        (��������)���γɱ�  (��������)ʵ�ʳɱ�  (��������)�ڳ���       (�ۼ�����)���γɱ�  (�ۼ�����)ʵ�ʳɱ� (�ۼ�����)�ڳ���   (�ۼ�����)�ڳ�����
                //��Ŀ����          ��ĿԤ��Ա��������   ��Ŀ�ɱ�Ա

                // string sql = "select t3.*  from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " order by t3.costsubjectcode";//������Ĳ�ѯ
                decimal dJCMoney = 0, dSumRespon = 0;
                string sql = "select t3.COSTINGSUBJECTNAME as  COSTINGSUBJECTNAME, SUM(t3.CURRRESPONSICONSUMETOTALPRICE) as  CURRRESPONSICONSUMETOTALPRICE, SUM(t3.CURRREALCONSUMETOTALPRICE) as  CURRREALCONSUMETOTALPRICE, SUM(t3.SUMRESPONSICONSUMETOTALPRICE) as  SUMRESPONSICONSUMETOTALPRICE,  SUM(t3.SUMREALCONSUMETOTALPRICE) as  SUMREALCONSUMETOTALPRICE    from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " GROUP BY  t3.COSTINGSUBJECTNAME,t3.costsubjectcode ";//����Ĳ�ѯ

                var dt = service.GetData(sql).Tables[0];
                if (dt == null && dt.Rows.Count == 0) return;
                fGridMeasures.Cell(3, 1).Text = "��Ŀ���ƣ�" + projectInfo.Name;//��Ŀ����,λ��ģ��ĵ�3�е�1��
                fGridMeasures.Cell(3, 6).Text = "���ڣ�" + DateTime.Now.Year + "��  " + DateTime.Now.Month + "��  " + DateTime.Now.Day + "��";//��  �ڣ�  ��  ��  ��,λ��ģ��ĵ�3�е�6��


                string SumAccountMoney = "";//��Ҫ�Ӳ�ѯ�����ȡֵ
                fGridMeasures.Cell(3, 9).Text = "��λ��" + SumAccountMoney + "Ԫ";//��λ��Ԫ,λ��ģ��ĵ�2�е�8��

                fGridMeasures.InsertRow(6, dt.Rows.Count - 1);          // �������ڴ洢���е�λ������   
                int num = 6;//ģ��ӵ�6�п�ʼ��ֵ
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    //int i = 45;
                    fGridMeasures.Cell(num, 1).Text = (i + 1).ToString();//���
                    fGridMeasures.Cell(num, 2).Text = dt.Rows[i]["COSTINGSUBJECTNAME"].ToString();//������Ŀ

                    fGridMeasures.Cell(num, 3).Text = dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"].ToString(); //(��������)���γɱ�  
                    fGridMeasures.Cell(num, 4).Text = dt.Rows[i]["CURRREALCONSUMETOTALPRICE"].ToString();  //(��������)ʵ�ʳɱ�
                    //�ڳ�=����-ʵ��
                    dJCMoney = ClientUtil.ToDecimal(dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"]) - ClientUtil.ToDecimal(dt.Rows[i]["CURRREALCONSUMETOTALPRICE"]);
                    fGridMeasures.Cell(num, 5).Text = dJCMoney.ToString();// (decimal.Parse(dt.Rows[i]["CURRREALCONSUMETOTALPRICE"].ToString()) - decimal.Parse(dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"].ToString())).ToString();  //(��������)�ڳ��� 
                    if (dJCMoney < 0) fGridMeasures.Cell(num, 5).BackColor =   System.Drawing.Color.Red ;

                    fGridMeasures.Cell(num, 6).Text = dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString();//(�ۼ�����)���γɱ� 
                    fGridMeasures.Cell(num, 7).Text = dt.Rows[i]["SUMREALCONSUMETOTALPRICE"].ToString();//(�ۼ�����)ʵ�ʳɱ� 
                    //�ڳ�=����-ʵ��
                    dJCMoney = ClientUtil.ToDecimal(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"]) - ClientUtil.ToDecimal(dt.Rows[i]["SUMREALCONSUMETOTALPRICE"]);
                    //string otm = (decimal.Parse(dt.Rows[i]["SUMREALCONSUMETOTALPRICE"].ToString()) - decimal.Parse(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString())).ToString();//(�ۼ�����)�ڳ���  Ҳ����  ʵ�ʳɱ� - ���γɱ�
                    fGridMeasures.Cell(num, 8).Text = dJCMoney.ToString();//(�ۼ�����)�ڳ���
                   if(dJCMoney < 0 ) fGridMeasures.Cell(num, 8).BackColor = System.Drawing.Color.Red ;

                    //string ljzrcb = dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString(); //(�ۼ�����)���γɱ� 
                    dSumRespon = ClientUtil.ToDecimal(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"]);
                    //if ((ljzrcb != "0") && (ljzrcb.Trim().Length > 0))
                    if (dSumRespon!=0)
                    {
                        //��ʵ�ʳɱ� - ���γɱ���/ ���γɱ�
                        //(����-ʵ��)/����

                       // Decimal m = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb));

                        //fGridDetail.Cell(num, 9).Text = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb)).ToString();//(�ۼ�����)�ڳ����� ,��(�ۼ�����)�ڳ��� ���� (�ۼ�����)���γɱ�
                        fGridMeasures.Cell(num, 9).Text =Math.Round((dJCMoney * 100) / dSumRespon,2).ToString("N2") + "%";  //��ȷ��С�����2λ
                    }
                    else
                    {
                        fGridMeasures.Cell(num, 9).Text = "0";
                    }

                    num++;

                }

                #region  ��3����ʱ����Ҫ�� ,�����ģ��������һ�м���
                fGridMeasures.Cell(num, 1).Text = "��Ŀ����:";
                fGridMeasures.Cell(num, 4).Text = "��ĿԤ��Ա��������):";
                fGridMeasures.Cell(num, 7).Text = "��Ŀ�ɱ�Ա:";
                //��Ŀ����          
                //��ĿԤ��Ա��������   
                //��Ŀ�ɱ�Ա
                #endregion

                #endregion

                
            }
            catch (Exception e1)
            {
                throw new Exception("����[" + detailExptrMeasures + "]�����쳣[" + e1.Message + "]");
            }
            finally
            {
                fGridMeasures.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridMeasures.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                FlexCell.Range oRange = fGridMeasures.Range(1, 1, fGridMeasures.Rows - 1, fGridMeasures.Cols - 1);
                oRange.Locked = true;
                fGridMeasures.SelectionMode = FlexCell.SelectionModeEnum.Free;
                fGridMeasures.AutoRedraw = true;
                fGridMeasures.Refresh();
               
            }

        }

        private void LoadDetailFile()
        {
           
            try
            {
                fGridDetail.AutoRedraw = false;
               

                #region  ��װ��ѯ���� ���ֳ�����ѷ����Աȱ�
                var condition = " and t1.theprojectguid='" + projectInfo.Id + "'  and t3.COSTSUBJECTCODE LIKE  'C513%'          ";//�ֳ������C513
                if (!string.IsNullOrEmpty(this.comYear.Text))//���
                {
                    condition += " and t1.kjn='" + this.comYear.Text + "'";
                }
                if (!string.IsNullOrEmpty(this.comMonth.Text))//�·�
                {
                    condition += " and t1.kjy='" + this.comMonth.Text + "'";
                }

                #endregion

                #region ��Ҫȡ����ֵ���ֳ�����ѷ����Աȱ�


                //��Ŀ����(��Ӧ projectInfo.Name)     ����(���������)
                //��ţ��Լ���ţ�    ������Ŀ        (��������)���γɱ�  (��������)ʵ�ʳɱ�  (��������)�ڳ���       (�ۼ�����)���γɱ�  (�ۼ�����)ʵ�ʳɱ� (�ۼ�����)�ڳ���   (�ۼ�����)�ڳ�����
                //��Ŀ����          ��ĿԤ��Ա��������   ��Ŀ�ɱ�Ա

               // string sql = "select t3.*  from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " order by t3.costsubjectcode";//������Ĳ�ѯ

                string sql = "select t3.COSTINGSUBJECTNAME as  COSTINGSUBJECTNAME, SUM(t3.CURRRESPONSICONSUMETOTALPRICE) as  CURRRESPONSICONSUMETOTALPRICE, SUM(t3.CURRREALCONSUMETOTALPRICE) as  CURRREALCONSUMETOTALPRICE, SUM(t3.SUMRESPONSICONSUMETOTALPRICE) as  SUMRESPONSICONSUMETOTALPRICE,  SUM(t3.SUMREALCONSUMETOTALPRICE) as  SUMREALCONSUMETOTALPRICE    from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 where t1.id=t2.parentid and t2.id=t3.parentid  " + condition + " GROUP BY  t3.COSTINGSUBJECTNAME,t3.costsubjectcode ";//����Ĳ�ѯ
                decimal dJCMoney = 0,dSumRepso=0;
                var dt = service.GetData(sql).Tables[0];
                if (dt == null && dt.Rows.Count == 0) return;
                fGridDetail.Cell(3, 1).Text = "��Ŀ���ƣ�" +  projectInfo.Name;//��Ŀ����,λ��ģ��ĵ�3�е�1��
                fGridDetail.Cell(3, 6).Text = "���ڣ�" + DateTime.Now.Year + "��  " + DateTime.Now.Month + "��  " + DateTime.Now.Day + "��";//��  �ڣ�  ��  ��  ��,λ��ģ��ĵ�3�е�6��


                string SumAccountMoney = "";//��Ҫ�Ӳ�ѯ�����ȡֵ
                fGridDetail.Cell(3, 9).Text = "��λ��" + SumAccountMoney + "Ԫ";//��λ��Ԫ,λ��ģ��ĵ�2�е�8��

                fGridDetail.InsertRow(6, dt.Rows.Count - 1);          // �������ڴ洢���е�λ������   
                int num = 6;//ģ��ӵ�6�п�ʼ��ֵ
                for (int i = 0; i < dt.Rows.Count;i++ )
                {

                    //int i = 45;
                    fGridDetail.Cell(num, 1).Text = (i + 1).ToString();//���
                    fGridDetail.Cell(num, 2).Text = dt.Rows[i]["COSTINGSUBJECTNAME"].ToString();//������Ŀ

                    fGridDetail.Cell(num, 3).Text = dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"].ToString(); //(��������)���γɱ�  
                    fGridDetail.Cell(num, 4).Text = dt.Rows[i]["CURRREALCONSUMETOTALPRICE"].ToString();  //(��������)ʵ�ʳɱ�
                    //����-ʵ�ʳɱ�
                    dJCMoney =ClientUtil.ToDecimal(dt.Rows[i]["CURRRESPONSICONSUMETOTALPRICE"])- ClientUtil.ToDecimal(dt.Rows[i]["CURRREALCONSUMETOTALPRICE"] ) ;
                    fGridDetail.Cell(num, 5).Text = dJCMoney.ToString();  //(��������)�ڳ��� 
                    if (dJCMoney < 0) fGridDetail.Cell(num, 5).BackColor =   System.Drawing.Color.Red  ;
                 


                    fGridDetail.Cell(num, 6).Text = dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"].ToString();//(�ۼ�����)���γɱ� 
                    fGridDetail.Cell(num, 7).Text = dt.Rows[i]["SUMREALCONSUMETOTALPRICE"].ToString();//(�ۼ�����)ʵ�ʳɱ� 
                    //����-ʵ�ʳɱ�
                    dJCMoney = ClientUtil.ToDecimal(dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"]) - ClientUtil.ToDecimal(dt.Rows[i]["SUMREALCONSUMETOTALPRICE"]);//(�ۼ�����)�ڳ���  Ҳ����   
                    fGridDetail.Cell(num, 8).Text = dJCMoney.ToString();//(�ۼ�����)�ڳ���
                    if (dJCMoney < 0) fGridDetail.Cell(num, 8).BackColor =   System.Drawing.Color.Red  ;
                    dSumRepso =ClientUtil.ToDecimal( dt.Rows[i]["SUMRESPONSICONSUMETOTALPRICE"] ); //(�ۼ�����)���γɱ� 

                    if (dSumRepso!=0)
                    {
                        //��ʵ�ʳɱ� - ���γɱ���/ ���γɱ�


                       // Decimal m = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb));

                        //fGridDetail.Cell(num, 9).Text = Convert.ToDecimal(decimal.Parse(otm) / decimal.Parse(ljzrcb)).ToString();//(�ۼ�����)�ڳ����� ,��(�ۼ�����)�ڳ��� ���� (�ۼ�����)���γɱ�
                        fGridDetail.Cell(num, 9).Text = Math.Round( (dJCMoney / dSumRepso * 100),2).ToString("N2") + "%";  //��ȷ��С�����2λ
                    }
                    else
                    {
                        fGridDetail.Cell(num, 9).Text = "0";
                    }
                  
                    num++;

                }

                #region  ��3����ʱ����Ҫ�� ,�����ģ��������һ�м���
                fGridDetail.Cell(num, 1).Text = "��Ŀ����:";
                fGridDetail.Cell(num, 4).Text = "��ĿԤ��Ա��������):";
                fGridDetail.Cell(num, 7).Text = "��Ŀ�ɱ�Ա:";
                //��Ŀ����          
                    //��ĿԤ��Ա��������   
                //��Ŀ�ɱ�Ա
                #endregion

                #endregion

                #region   DEMO

                ////// ����ȡ��ͳ�Ƶ�ʱ���
                //TimeSlice ts = new TimeSlice(Convert.ToInt32(cbYear.Text));

                //var startTime = ts.Slice[0].Value[0];
                //var endTime = ts.Slice[11].Value[1];

                //// ��β�ѯ�����е�ͳ������
                //var condition = string.Format(" and submitdate<=to_date('{0}','yyyy-mm-dd')", endTime.ToString("yyyy-MM-dd"));
                //if (!string.IsNullOrEmpty(txtSupply.Text))
                //{
                //    condition += " and supplierrelation='" + (this.txtSupply.Result[0] as SupplierRelationInfo).Id + "'";
                //}
                //string sql = "select suppliername name, supplierrelation id,summoney money,submitdate time from thd_materialrentelsetmaster where state=5 and projectid='" + projectInfo.Id + "'" + condition + " order by createdate desc";
                //var dt = service.GetData(sql).Tables[0];

          
                //if (dt == null && dt.Rows.Count == 0) return;
                //var result = dt.Select().Select(a => new { CreateTime = Convert.ToDateTime(a["time"]), Name = a["name"].ToString(), Money = Convert.ToDecimal(a["money"]), Id = a["id"].ToString() });
                //var resultGroup = result.GroupBy(a => a.Id);                // ���ݵ�λid����
                //fGridDetail.InsertRow(6, resultGroup.Count() - 1);          // �������ڴ洢���е�λ������

                //int num = 0;
                //foreach (var item in resultGroup)
                //{
                //    var departmentResult = result.Where(a => a.Id == item.Key).ToList();
                //    var tempResult = departmentResult.Where(a => a.CreateTime >= startTime && a.CreateTime <= endTime).ToList();
                //    decimal total = departmentResult.Where(a => a.CreateTime < startTime).Sum(a => a.Money);
                //    fGridDetail.Cell(5 + num, 1).Text = (num + 1).ToString();
                //    fGridDetail.Cell(5 + num, 2).Text = departmentResult[0].Name;
                //    foreach (var time in ts.Slice)
                //    {
                //        if (time.Value[0] > DateTime.Now) break;
                //        var summary = tempResult.Where(a => a.CreateTime >= time.Value[0] && a.CreateTime <= time.Value[1]).Sum(a => a.Money);
                //        total += summary;
                //        fGridDetail.Cell(5 + num, time.Key * 2 + 1).Text = summary.ToString();
                //        fGridDetail.Cell(5 + num, time.Key * 2 + 2).Text = total.ToString();
                //    }
                //    num++;
                //}

                //string sTitle = string.Format("{0}�� {1}��Ŀ��е���޽���̨��", cbYear.Text, projectInfo.Name);
                //fGridDetail.Cell(1, 1).Text = sTitle;
                //fGridDetail.Cell(2, 3).Text = cbYear.Text + "��ÿ�½����Ԫ��";

                //for (int tt = 0; tt < fGridDetail.Cols; tt++)
                //{
                //    fGridDetail.Column(tt).AutoFit();
                //}
                #endregion
            }
            catch (Exception e1)
            {
                throw new Exception("����[" + detailExptr + "]�����쳣[" + e1.Message + "]");
            }
            finally
            {
                fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
                FlexCell.Range oRange = fGridDetail.Range(1, 1, fGridDetail.Rows - 1, fGridDetail.Cols - 1);
                oRange.Locked = true;
                fGridDetail.SelectionMode = FlexCell.SelectionModeEnum.Free;
                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                //FlashScreen.Close();
            }

        }

    }
}