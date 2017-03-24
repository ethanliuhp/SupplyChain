using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NHibernate.Criterion;
using System.Collections;
using NHibernate;

using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;


using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewCollectQuery : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private ViewMain vm = new ViewMain();
        private ViewWriteInfo vwInfo = new ViewWriteInfo();
        string time_str = "";
        string ywzz_str = "";
        string job_name = "";//¼���λ
        string org_name = "";//¼�벿��
        string person_name = "";//¼����
        string createtime = "";//¼��ʱ��
        //���������ʽ����������
        IList save_data_list = new ArrayList();//�洢������
        IList cal_data_list = new ArrayList();//���������
        IList display_data_list = new ArrayList();//��ʾ������

        public VViewCollectQuery()
        {
            InitializeComponent();
            InitialEvents();
        }

        internal void Start()
        {
            InitialControls();
        }

        private void InitialControls()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheJob.Id", ConstObject.TheSysRole.Id));
            oq.AddCriterion(Expression.Eq("Main.ViewTypeCode", "3"));
            oq.AddFetchMode("Main", FetchMode.Eager);
            IList list = mCube.ViewService.GetViewDistributeByQuery(oq);
            IList da_list = new ArrayList();
            foreach (ViewDistribute vd in list)
            {
                da_list.Add(vd.Main);
            }
            cboView.DataSource = da_list;
            cboView.DisplayMember = "ViewName";
        }

        private void GetViewWriteInfo(ViewMain vm,string timeDimId,string ywzzDimId) {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TimeDimId", timeDimId));
            //oq.AddCriterion(Expression.Eq("YwzzDimId", ywzzDimId));
            oq.AddCriterion(Expression.Eq("Main.Id", vm.Id));
            oq.AddFetchMode("Main", FetchMode.Eager);
            oq.AddFetchMode("TheOpeOrg", FetchMode.Eager);
            oq.AddFetchMode("TheJob", FetchMode.Eager);
            oq.AddFetchMode("Author", FetchMode.Eager);
            IList list = mCube.ViewService.GetViewWriteInfoByQuery(oq);
            foreach (ViewWriteInfo vwi in list)
            {
                vwInfo = vwi;
                break;
            }
            if (list.Count > 0)
            {
                org_name = vwInfo.TheOpeOrg.Name;
                person_name = vwInfo.Author.Name;
                job_name = vwInfo.TheJob.RoleName;
                createtime = vwInfo.CreateDate.ToShortDateString();
            }
        }

        private void InitialEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnPreview.Click += new EventHandler(btnPreview_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnTime.Click += new EventHandler(btnTime_Click);
            cboView.SelectedIndexChanged += new EventHandler(cboView_SelectedIndexChanged);

        }

        /// <summary>
        ///��ʱ��ѡ��ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnTime_Click(object sender, EventArgs e)
        {
            VReportDimension vrd = new VReportDimension();
            IList attr_list = mCube.CubeManagerSrv.GetCubeAttrByCubeResgisterId(vm.CubeRegId);
            string time_id ="";
            foreach (CubeAttribute attr in attr_list)
            {
                if (attr.DimensionName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1)
                {
                    time_id = attr.DimensionId;
                }
            }
            vrd.main_cat = mCube.DimManagerSrv.GetDimensionCategoryById(time_id);

            vrd.ShowDialog();
            if (vrd.isOkClicked)
            {
                string dimCode = vrd.selectDimCode;
                bool ifRightTime = KnowledgeUtil.IfRightTime(dimCode, vm.CollectTypeCode);
                if (ifRightTime == false)
                {
                    edtTime.Text = null;
                    edtTime.Tag = null;
                    MessageBox.Show("��ѡ����ȷ��ʱ��,����ԭ���¡����ȡ��������ȷѡȡ��");
                    return;
                }
                else
                {
                    edtTime.Text = vrd.selectDimName + "";
                    edtTime.Tag = vrd.selectDimId + "";
                }
            }
        }

        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                vm = cboView.SelectedItem as ViewMain;
                if (vm == null) return;

            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("����ģ���Ӧ��ʱ�伯�ϳ���", ex);
            }
        }

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnQuery_Click(object sender, EventArgs e)
        {
            save_data_list.Clear();
            cal_data_list.Clear();
            display_data_list.Clear();
            ExploreFile eFile = new ExploreFile();

            if (edtTime.Text == null || "".Equals(edtTime.Text))
            {
                MessageBox.Show("��ѡ��ʱ�䣡");
                return;
            }

            time_str = edtTime.Tag as string;

            GetViewWriteInfo(vm, time_str, ywzz_str);//��ѯ��ͼ¼����Ϣ

            if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
            {
                LoadFlexFile();
                LoadFlexData();
                CalFlexData();
                DisplayRuleData();
            }
            else {
                MessageBox.Show("δ�����ģ��ĸ�ʽ�ļ���");
            }
        }

        //�������������ĸ�ʽ�ļ�
        private void LoadFlexFile()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            //eFile.CreateEnergyTempletFile(vm.ViewName + ".flx");
            //�����ʽ������
            dgvWrite.OpenFile(path + "\\" + vm.ViewName + ".flx");//�����ʽ
        }

        /// <summary>
        /// ��������
        /// 1���������޼�����ʽ��ֵ��2���ټ����б��ʽ��ֵ����������(������ʽ����Ƕ��)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFlexData()
        {
            string ywzz_link = vm.CollectYwzz;
            int cols = dgvWrite.Cols;
            //�����еĿ�¼���С����λ��
            for (int s = 1; s < cols; s++)
            {
                dgvWrite.Column(s).DecimalLength = 4;
            }

            IList rule_list = mCube.ViewService.GetViewRuleDefByViewMain(vm);//��ѯ��ͼ�����弯��
            IList style_list = mCube.ViewService.GetViewStyleByViewMain(vm, false);//��ѯ��ͼ��ʽ����
            int time_index = 0;//ʱ��ά������ͼ��ʽ�е�λ��
            int ywzz_index = 0;//ҵ����֯ά������ͼ��ʽ�е�λ��
            int k = 0;
            foreach (ViewStyle vs in style_list)
            {
                if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1)
                {
                    time_index = k;
                }

                if (vs.OldCatRootName.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) != -1)
                {
                    ywzz_index = k;
                }
                k++;
            }
            Hashtable ht_result = mCube.GetFreeFlexData(vm, time_str);//ά������ַ�����cubedata�Ķ�Ӧ
            char[] patten = { '_' };//�ָ���
            foreach (ViewRuleDef vrd in rule_list)
            {
                if (vrd.SaveExpress != null && !"".Equals(vrd.SaveExpress))
                {
                    if (vrd.CalExpress != null && !"".Equals(vrd.CalExpress) && vrd.CalExpress.IndexOf("_") == -1)
                    {
                        cal_data_list.Add(vrd);
                        continue;
                    }
                    
                    double add_value = 0;

                    //����ѡ�е�ҵ����ѭ��                   
                    string[] temp = ywzz_link.Split(patten);
                    for (int t = 1; t < temp.Length - 1; t++)
                    {
                        string saveExpress = vrd.SaveExpress;
                        string curr_ywzz = (string)temp[t];

                        //�Ⱥ����ʱ���ҵ����֯ά��,�Ȳ�λ��С��
                        if (time_index < ywzz_index)
                        {
                            //�Ȳ���ʱ��
                            int start_pos = mCube.GetIndexByString(saveExpress, "_", 0, time_index);//��ʼ����ʱ��ά��ֵ��λ��,�Ӻ�һ��λ����ǰ��
                            saveExpress = saveExpress.Insert(start_pos, "_" + time_str);

                            //�����ҵ����֯
                            if (ywzz_index == k)//���Ϊ���һ��λ��
                            {
                                saveExpress += "_" + ywzz_str;
                            }
                            else
                            {
                                start_pos = mCube.GetIndexByString(saveExpress, "_", 0, ywzz_index + 1);//�����ҵ��ά��ֵ��λ��,�Ӻ�һ��λ����ǰ��
                                saveExpress = saveExpress.Insert(start_pos, "_" + curr_ywzz);
                            }

                        }
                        else
                        {
                            //�Ȳ�����֯
                            int start_pos = mCube.GetIndexByString(saveExpress, "_", 0, ywzz_index);//��ʼ����ҵ��ά��ֵ��λ��,�Ӻ�һ��λ����ǰ��
                            saveExpress = saveExpress.Insert(start_pos, "_" + curr_ywzz);

                            //�����ʱ��
                            if (time_index == k)//���Ϊ���һ��λ��
                            {
                                saveExpress += "_" + time_str;
                            }
                            else
                            {
                                start_pos = mCube.GetIndexByString(saveExpress, "_", 0, time_index + 1);//�����ʱ��ά��ֵ��λ��,�Ӻ�һ��λ����ǰ��
                                saveExpress = saveExpress.Insert(start_pos, "_" + time_str);
                            }
                        }
                        CubeData cd = ht_result[saveExpress + "_"] as CubeData;
                        if (cd != null)
                        {
                            add_value = add_value + cd.Result;
                        }
                    }


                    string sign = vrd.CellSign;
                    //ȡ�����ַ�
                    int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                    int row = int.Parse(KnowledgeUtil.SplitStr(sign, 1));
                    dgvWrite.Cell(row, col).Text = add_value + "";
                    dgvWrite.Cell(row, col).Mask = FlexCell.MaskEnum.Numeric;
                    dgvWrite.Cell(row, col).BackColor = System.Drawing.Color.AliceBlue;
                    dgvWrite.Cell(row, col).Alignment = FlexCell.AlignmentEnum.RightCenter;
                    save_data_list.Add(vrd);
                }
                else
                {
                    display_data_list.Add(vrd);
                }
            }
        }


        //������ڱ��ʽ�ĵ�Ԫ���ֵ
        private void CalFlexData()
        {
            for (int k = 0; k < 3; k++)
            {
                IList temp_list = new ArrayList();
                foreach (ViewRuleDef vrd in cal_data_list)
                {
                    temp_list.Add(vrd);
                }
                DataTable dt = new DataTable();
                char[] patten = { '[', ']' };//�ָ���
                foreach (ViewRuleDef vrd in temp_list)
                {
                    string cal_value = "0";
                    string currExpress = vrd.CalExpress;
                    string cellSign = vrd.CellSign;

                    string[] temp = currExpress.Split(patten);
                    IList signList = new ArrayList();
                    for (int i = 1; i < temp.Length; i = i + 2)
                    {
                        signList.Add(temp[i]);
                    }
                    foreach (string sign in signList)
                    {
                        int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                        int row = int.Parse(KnowledgeUtil.SplitStr(sign, 1));
                        string value = dgvWrite.Cell(row, col).Text;
                        if (value == null || "".Equals(value))
                        {
                            break;
                        }
                        else
                        {
                            currExpress = currExpress.Replace("[" + sign + "]", value);
                        }
                    }

                    if (currExpress.IndexOf("[") == -1)
                    {
                        cal_value = dt.Compute(currExpress, "").ToString();
                        if (cal_value != null && !"".Equals(cal_value))
                        {
                            cal_value = Math.Round(double.Parse(cal_value), 4) + "";
                            cal_data_list.Remove(vrd);
                        }
                    }

                    int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                    int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));
                    dgvWrite.Cell(curr_row, curr_col).Text = cal_value;
                    dgvWrite.Cell(curr_row, curr_col).Mask = FlexCell.MaskEnum.Numeric;
                    dgvWrite.Cell(curr_row, curr_col).BackColor = System.Drawing.Color.AliceBlue;
                    dgvWrite.Cell(curr_row, curr_col).Alignment = FlexCell.AlignmentEnum.RightCenter;
                }
            }
        }


        //��ʾ'��ʾ��'����ĵ�Ԫ��
        private void DisplayRuleData()
        {            
            foreach (ViewRuleDef vrd in display_data_list)
            {
                string cellSign = vrd.CellSign;
                int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));
                string rule_express = vrd.DisplayRule;

                dgvWrite.Cell(curr_row, curr_col).Text = this.GetDisplayStrByRule(rule_express);
            }
        }


        private string GetDisplayStrByRule(string rule_express)
        {
            string dim_time = edtTime.Text;

            if (rule_express.IndexOf("{����ʱ��}") != -1)
            {
                rule_express = rule_express.Replace("{����ʱ��}", dim_time);
            }

            if (rule_express.IndexOf("{¼���λ}") != -1)
            {
                rule_express = rule_express.Replace("{¼���λ}", job_name);
            }

            if (rule_express.IndexOf("{¼����}") != -1)
            {
                rule_express = rule_express.Replace("{¼����}", person_name);
            }

            if (rule_express.IndexOf("{¼��ʱ��}") != -1)
            {
                rule_express = rule_express.Replace("{¼��ʱ��}", createtime);
            }

            /*if (rule_express.IndexOf("{¼�벿��}") != -1)
            {
                rule_express = rule_express.Replace("{¼�벿��}", org_name);
                
            }*/
            return rule_express;
        }

        /// <summary>
        /// Ԥ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPreview_Click(object sender, EventArgs e)
        {
            mCube.GeneralPreview(dgvWrite);
        }

        /// <summary>
        /// ����Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            dgvWrite.ExportToExcel("", true, true);
        }

        /// <summary>
        /// �ر�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}