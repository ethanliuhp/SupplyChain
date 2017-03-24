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
        string job_name = "";//录入岗位
        string org_name = "";//录入部门
        string person_name = "";//录入人
        string createtime = "";//录入时间
        //定义规则表达式中三种类型
        IList save_data_list = new ArrayList();//存储的数据
        IList cal_data_list = new ArrayList();//计算的数据
        IList display_data_list = new ArrayList();//显示的数据

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
        ///　时间选择按钮
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
                    MessageBox.Show("请选择正确的时间,可能原因：月、季度、年份请正确选取！");
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
                KnowledgeMessageBox.InforMessage("查找模板对应的时间集合出错。", ex);
            }
        }

        /// <summary>
        /// 查询
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
                MessageBox.Show("请选择时间！");
                return;
            }

            time_str = edtTime.Tag as string;

            GetViewWriteInfo(vm, time_str, ywzz_str);//查询视图录入信息

            if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
            {
                LoadFlexFile();
                LoadFlexData();
                CalFlexData();
                DisplayRuleData();
            }
            else {
                MessageBox.Show("未定义该模板的格式文件！");
            }
        }

        //引导表格设计器的格式文件
        private void LoadFlexFile()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            //eFile.CreateEnergyTempletFile(vm.ViewName + ".flx");
            //载入格式和数据
            dgvWrite.OpenFile(path + "\\" + vm.ViewName + ".flx");//载入格式
        }

        /// <summary>
        /// 载入数据
        /// 1：先载入无计算表达式的值，2：再计算有表达式的值，计算三遍(计算表达式中有嵌套)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFlexData()
        {
            string ywzz_link = vm.CollectYwzz;
            int cols = dgvWrite.Cols;
            //设置列的可录入的小数点位数
            for (int s = 1; s < cols; s++)
            {
                dgvWrite.Column(s).DecimalLength = 4;
            }

            IList rule_list = mCube.ViewService.GetViewRuleDefByViewMain(vm);//查询视图规则定义集合
            IList style_list = mCube.ViewService.GetViewStyleByViewMain(vm, false);//查询视图格式集合
            int time_index = 0;//时间维度在视图格式中的位置
            int ywzz_index = 0;//业务组织维度在视图格式中的位置
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
            Hashtable ht_result = mCube.GetFreeFlexData(vm, time_str);//维度组合字符串和cubedata的对应
            char[] patten = { '_' };//分隔符
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

                    //根据选中的业务部门循环                   
                    string[] temp = ywzz_link.Split(patten);
                    for (int t = 1; t < temp.Length - 1; t++)
                    {
                        string saveExpress = vrd.SaveExpress;
                        string curr_ywzz = (string)temp[t];

                        //先后插入时间和业务组织维度,先插位置小的
                        if (time_index < ywzz_index)
                        {
                            //先插入时间
                            int start_pos = mCube.GetIndexByString(saveExpress, "_", 0, time_index);//开始插入时间维度值的位置,从后一个位置往前插
                            saveExpress = saveExpress.Insert(start_pos, "_" + time_str);

                            //后插入业务组织
                            if (ywzz_index == k)//如果为最后一个位置
                            {
                                saveExpress += "_" + ywzz_str;
                            }
                            else
                            {
                                start_pos = mCube.GetIndexByString(saveExpress, "_", 0, ywzz_index + 1);//后插入业务维度值的位置,从后一个位置往前插
                                saveExpress = saveExpress.Insert(start_pos, "_" + curr_ywzz);
                            }

                        }
                        else
                        {
                            //先插入组织
                            int start_pos = mCube.GetIndexByString(saveExpress, "_", 0, ywzz_index);//开始插入业务维度值的位置,从后一个位置往前插
                            saveExpress = saveExpress.Insert(start_pos, "_" + curr_ywzz);

                            //后插入时间
                            if (time_index == k)//如果为最后一个位置
                            {
                                saveExpress += "_" + time_str;
                            }
                            else
                            {
                                start_pos = mCube.GetIndexByString(saveExpress, "_", 0, time_index + 1);//后插入时间维度值的位置,从后一个位置往前插
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
                    //取出列字符
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


        //计算存在表达式的单元格的值
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
                char[] patten = { '[', ']' };//分隔符
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


        //显示'显示区'定义的单元格
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

            if (rule_express.IndexOf("{定义时间}") != -1)
            {
                rule_express = rule_express.Replace("{定义时间}", dim_time);
            }

            if (rule_express.IndexOf("{录入岗位}") != -1)
            {
                rule_express = rule_express.Replace("{录入岗位}", job_name);
            }

            if (rule_express.IndexOf("{录入人}") != -1)
            {
                rule_express = rule_express.Replace("{录入人}", person_name);
            }

            if (rule_express.IndexOf("{录入时间}") != -1)
            {
                rule_express = rule_express.Replace("{录入时间}", createtime);
            }

            /*if (rule_express.IndexOf("{录入部门}") != -1)
            {
                rule_express = rule_express.Replace("{录入部门}", org_name);
                
            }*/
            return rule_express;
        }

        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPreview_Click(object sender, EventArgs e)
        {
            mCube.GeneralPreview(dgvWrite);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            dgvWrite.ExportToExcel("", true, true);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}