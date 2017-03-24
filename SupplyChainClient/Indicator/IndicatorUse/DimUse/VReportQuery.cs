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
    public partial class VReportQuery : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private ViewMain vm = new ViewMain();
        public bool ifRefers = false;//是否资产经营系统引用技术管理报表
        string time_str = "";
        string time_code = "";
        string ywzz_str = "";
        string job_name = "";//录入岗位
        string org_name = "";//录入部门
        string person_name = "";//录入人
        string createtime = "";//录入时间
        //定义规则表达式中三种类型
        IList save_data_list = new ArrayList();//存储的数据
        IList cal_data_list = new ArrayList();//计算的数据
        IList display_data_list = new ArrayList();//显示的数据
        Hashtable ht_exist_value = new Hashtable();//该模板已经存在的值

        public VReportQuery()
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
            string systemId = ConstObject.TheSystemCode;
            oq.AddCriterion(Expression.Eq("SystemId", systemId));
            oq.AddCriterion(Expression.Eq("ViewTypeCode", "4"));
            IList list = mCube.ViewService.GetViewMainByQuery(oq);
            IList da_list = new ArrayList();
            foreach (ViewMain vm in list)
            {
                da_list.Add(vm);
            }
            
            cboView.DataSource = da_list;
            cboView.DisplayMember = "ViewName";
        }

        private void InitialEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnTime.Click += new EventHandler(btnTime_Click);
            btnPreview.Click += new EventHandler(btnPreview_Click);

            btnExcel.Click += new EventHandler(btnExcel_Click);

            cboView.SelectedIndexChanged += new EventHandler(cboView_SelectedIndexChanged);
        }

        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            vm = cboView.SelectedItem as ViewMain;
            if (vm == null) return;

        }

         /// <summary>
        /// 选择时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnTime_Click(object sender, EventArgs e)
        {
            VReportDimension vrd = new VReportDimension();

            if (vm == null)
            {
                MessageBox.Show("请选择模板！");
                return;
            }
            CubeRegister cubeReg = vm.CubeRegId;
            DimensionCategory cat = new DimensionCategory();
            //添加维度列表
            IList list = mCube.CubeManagerSrv.GetCubeAttrByCubeResgisterId(cubeReg);
            foreach (CubeAttribute cubeAttr in list)
            {
                if (cubeAttr.DimensionName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1)
                {
                    cat = mCube.DimManagerSrv.GetDimensionCategoryById(cubeAttr.DimensionId);
                }
            }

            vrd.main_cat = cat;

            vrd.ShowDialog();
            if (vrd.isOkClicked)
            {
                string dimId = vrd.selectDimId + "";
                string dimName = vrd.selectDimName;
                txtTime.Tag = dimId;
                txtTime.Text = dimName;
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

            if (txtTime.Text == null || "".Equals(txtTime.Text))
            {
                MessageBox.Show("请选择时间！");
                return;
            }

            time_str = txtTime.Tag as string;

            if (ifRefers == false)
            {
                if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
                {
                    LoadFlexFile();            
                }
                else
                {
                    MessageBox.Show("未定义该模板的格式文件！");
                    return;
                }
            }
            else {
                if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
                {
                    LoadFlexFileFromKn();                          
                }
                else
                {
                    MessageBox.Show("未定义该模板的格式文件！");
                    return;
                }
            }
            dgvWrite.Row(0).Visible = false;
            dgvWrite.Column(0).Visible = false;

            LoadFlexData();
            //DisplayRuleData();         
        }

        //引导表格设计器的格式文件
        private void LoadFlexFile()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;

            if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
            {
                //eFile.CreateEnergyTempletFile(vm.ViewName + ".flx");
                //载入格式和数据
                dgvWrite.OpenFile(path + "\\" + vm.ViewName + ".flx");//载入格式
            }
            else {
                MessageBox.Show("未找到模板格式文件！");
                return;
            }
        }

        //引导表格设计器的格式文件
        private void LoadFlexFileFromKn()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;

            if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
            {
                //eFile.CreateEnergyFileFromKnowledge(vm.ViewName + ".flx");
                //载入格式和数据
                dgvWrite.OpenFile(path + "\\" + vm.ViewName + ".flx");//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件！");
                return;
            }
        }

        //载入数据
        private void LoadFlexData()
        {          
            int cols = dgvWrite.Cols;
            //设置列的可录入的小数点位数
            for (int s = 1; s < cols; s++)
            {
                dgvWrite.Column(s).DecimalLength = 4;
            }

            CubeRegister cr = vm.CubeRegId;
            string reg_id = cr.Id;
            cr = mCube.CubeManagerSrv.GetCubeRegisterById(reg_id);

            DataTable dt = new DataTable();
            IList rule_list = mCube.ViewService.GetViewRuleDefByViewMain(vm);//所有的公式
            if (rule_list != null && rule_list.Count > 0)
            {
                ht_exist_value = mCube.GetAllValueInVm(cr, rule_list, time_str, ywzz_str);
                DimensionCategory cate = mCube.DimManagerSrv.GetDimensionCategoryById(time_str);
                time_code = cate.Code;

                dgvWrite.AutoRedraw = false;
                foreach (ViewRuleDef vrd in rule_list)
                {
                    string cal_value = "0";
                    string calexpress = vrd.CalExpress;
                    string cellSign = vrd.CellSign;

                    int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                    int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));
                    bool ifCal = KnowledgeUtil.IfHaveUpperLetter(calexpress);//判断是否存在表格内部计算表达式
                    //计算值
                    if (calexpress != null && ifCal == false)
                    {
                        calexpress = mCube.TransCalExpress(calexpress,cate);
                        
                        char[] pattern ={ '[', ']' };
                        string[] temp = calexpress.Split(pattern);
                        for (int i = 1; i < temp.Length; i = i + 2)
                        {
                            string get_express = temp[i].ToString();
                            string value = "";
                            if (ht_exist_value[get_express] == null)
                            {
                                value = "0";
                            }
                            else
                            {
                                value = ht_exist_value[get_express].ToString();
                            }

                            calexpress = calexpress.Replace("[" + get_express + "]", value);
                        }

                        //计算表达式的结果
                        if (calexpress.IndexOf("[") == -1 && !"".Equals(calexpress))
                        {
                            try
                            {
                                cal_value = dt.Compute(calexpress, "").ToString();
                            }
                            catch (Exception e)
                            {
                                throw new Exception("第（" + curr_row + "）行第（" + curr_col + "）列的计算错误，请查看公式定义！");
                            }
                            if (cal_value.IndexOf("无穷") != -1 || cal_value.IndexOf("非数字") != -1)
                            {
                                cal_value = "0";
                            }
                            if (cal_value != null && !"".Equals(cal_value))
                            {
                                cal_value = Math.Round(double.Parse(cal_value), 4) + "";
                            }
                        }
                        dgvWrite.Cell(curr_row, curr_col).Text = cal_value;
                        dgvWrite.Cell(curr_row, curr_col).Alignment = FlexCell.AlignmentEnum.RightCenter;
                    }
                    else
                    {//定义时间 
                        string displayRule = vrd.DisplayRule;
                        if (displayRule != null && displayRule.IndexOf("{") != -1)
                        {
                            dgvWrite.Cell(curr_row, curr_col).Text = txtTime.Text;
                        }
                    }

                }
            }

            //计算表达式值
            mCube.CalGridValue(rule_list, dgvWrite, ht_exist_value, time_str, ywzz_str);
        
            dgvWrite.AutoRedraw = true;
            dgvWrite.Refresh();      
        }        

        //无用
        private void CalExpressValue(IList rule_list) 
        {
            
            DataTable dt = new DataTable();

            IList curr_cal_list = new ArrayList();//计算规则集合的拷贝
            IList sign_list = new ArrayList();//当前单元格的代号集合
            foreach (ViewRuleDef vrd in rule_list)
            {
                curr_cal_list.Add(vrd);

                string currExpress = vrd.CalExpress;
                bool ifCal = KnowledgeUtil.IfHaveUpperLetter(currExpress);//判断是否存在表格内部计算表达式              
                if (currExpress != null && ifCal == true)
                {
                    sign_list.Add(vrd.CellSign);
                }
            }

            DimensionCategory cate = mCube.DimManagerSrv.GetDimensionCategoryById(time_str);
            for (int t = 0; t < 10; t++)
            {

                if (sign_list.Count == 0)
                {
                    break;
                }

                //计算规则集合的拷贝，变化中。。。
                IList temp_list = new ArrayList();
                foreach (ViewRuleDef vrd in curr_cal_list)
                {
                    temp_list.Add(vrd);
                }

                foreach (ViewRuleDef vrd in temp_list)
                {
                    bool haveCal = false;
                    string cal_value = "0";
                    string currExpress = vrd.CalExpress;
                    string cellSign = vrd.CellSign;
                    bool ifCal = KnowledgeUtil.IfHaveUpperLetter(currExpress);//判断是否存在表格内部计算表达式
                    if (currExpress != null && ifCal == true)
                    {
                        currExpress = mCube.TransCalExpress(currExpress, cate);
                        char[] patten = { '[', ']' };//分隔符
                        string[] temp = currExpress.Split(patten);
                        IList signList = new ArrayList();
                        bool ifHave = false;//判断计算表达式中的代号是否包含在sign_list中
                        for (int i = 1; i < temp.Length; i = i + 2)
                        {
                            if (sign_list.Contains(temp[i].ToString()))
                            {
                                ifHave = true;
                                break;
                            }

                            string get_express = temp[i].ToString();
                            string value = "";
                            if (get_express.IndexOf("_") == -1)
                            {
                                int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(get_express, 2));
                                int row = int.Parse(KnowledgeUtil.SplitStr(get_express, 1));
                                value = dgvWrite.Cell(row, col).Text;
                            }
                            else
                            {

                                object o = ht_exist_value[get_express];
                                if (o != null)
                                {
                                    value = o.ToString();
                                }
                            }

                            if (value == null || "".Equals(value) || KnowledgeUtil.IfWidthNumber(value) == false)
                            {
                                value = "0";
                            }

                            currExpress = currExpress.Replace("[" + get_express + "]", value);
                        }

                        if (currExpress.IndexOf("[") == -1 && ifHave == false)
                        {
                            try
                            {
                                cal_value = dt.Compute(currExpress, "").ToString();
                                if (cal_value.IndexOf("无穷") != -1 || cal_value.IndexOf("非数字") != -1)
                                {
                                    cal_value = "0";
                                }
                                if (cal_value != null && !"".Equals(cal_value))
                                {
                                    cal_value = Math.Round(double.Parse(cal_value), 4) + "";
                                    haveCal = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                string exception = ex.ToString();
                                if (exception.IndexOf("zero") > 0 || exception.IndexOf("零") > 0)
                                {
                                    cal_value = "0";
                                }
                            }
                        }

                        int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                        int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));
                        dgvWrite.Cell(curr_row, curr_col).Text = cal_value;
                        dgvWrite.Cell(curr_row, curr_col).Alignment = FlexCell.AlignmentEnum.RightCenter;

                        if (haveCal == true && ifHave == false)
                        {
                            curr_cal_list.Remove(vrd);
                            sign_list.Remove(vrd.CellSign);
                        }
                    }
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
            string dim_time = txtTime.Text;

            if (rule_express.IndexOf("{定义时间}") != -1)
            {
                rule_express = rule_express.Replace("{定义时间}", dim_time);
            }

            if (rule_express.IndexOf("{定义部门}") != -1)
            {
                rule_express = rule_express.Replace("{定义部门}", "");
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

            if (rule_express.IndexOf("{录入部门}") != -1)
            {
                rule_express = rule_express.Replace("{录入部门}", org_name);
                
            }
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