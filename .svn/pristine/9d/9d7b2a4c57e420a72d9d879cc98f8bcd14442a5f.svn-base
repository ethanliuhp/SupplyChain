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
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewFreeWrite : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private ViewMain vm = new ViewMain();
        private ViewWriteInfo vwInfo = new ViewWriteInfo();
        private bool ifSave = true;
        string time_str = "";
        string ywzz_str = "";
        string job_name = "";//录入岗位
        long org_id = -1;
        string org_syscode = "";//部门的系统代码
        string org_name = "";//录入部门
        string person_name = "";//录入人
        string createtime = "";//录入时间
        //定义规则表达式中三种类型
        IList save_data_list = new ArrayList();//存储的数据
        IList cal_data_list = new ArrayList();//计算的数据
        IList display_data_list = new ArrayList();//显示的数据
        Hashtable ht_exist_value = new Hashtable();//该模板涉及的表达式中已经存在的值

        public VViewFreeWrite()
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
            oq.AddOrder(Order.Asc("ViewName"));
            IList list = mCube.ViewService.GetViewDistributeByQuery(oq);
            IList da_list = new ArrayList();
            foreach (ViewDistribute vd in list)
            {
                da_list.Add(vd.Main);
            }
            
            cboView.DataSource = da_list;
            cboView.DisplayMember = "ViewName";
        }

        private void InitialEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            btnPreview.Click += new EventHandler(btnPreview_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnTime.Click += new EventHandler(btnTime_Click);

            cboView.SelectedIndexChanged += new EventHandler(cboView_SelectedIndexChanged);
        }


        #region 私有函数
        //按钮控制，控制是否可以提交，保存按钮
        private void ControlButton(bool enabled) 
        {
            if (enabled == true)
            {
                btnSave.Enabled = true;
                btnSubmit.Enabled = true;
            }
            else {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
            }
        }

        //取得报表录入信息主表
        private void GetViewWriteInfo(ViewMain vm,string timeDimId,string ywzzDimId) {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TimeDimId", timeDimId));
            oq.AddCriterion(Expression.Eq("YwzzDimId", ywzzDimId));
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
                string state = vwInfo.State;
                if (state != null && (KnowledgeUtil.VIEWWRITE_SUBMIT_CODE+"").Equals(state))
                {
                    ifSave = false;
                }
                else {
                    ifSave = true;
                }
                org_id = ClientUtil.ToLong(vwInfo.TheOpeOrg.Id);
                org_syscode = vwInfo.TheOpeOrg.SysCode;
                org_name = vwInfo.TheOpeOrg.Name;
                person_name = vwInfo.Author.Name;
                job_name = vwInfo.TheJob.RoleName;
                createtime = vwInfo.CreateDate.ToShortDateString();
            }
            else {
                org_name = ConstObject.TheOperationOrg.Name;
                person_name = ConstObject.LoginPersonInfo.Name;
                job_name = ConstObject.TheSysRole.RoleName;
                createtime = ConstObject.LoginDate.ToShortDateString();

                ifSave = true;
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

        //载入数据
        private void LoadFlexData()
        {
            bool ifSave = false;
            DataTable dt = new DataTable();
            int cols = dgvWrite.Cols;
            //设置列的可录入的小数点位数
            for (int s = 1; s < cols; s++)
            {
                dgvWrite.Column(s).DecimalLength = 4;
            }

            IList rule_list = mCube.ViewService.GetViewRuleDefByViewMain(vm);//查询视图规则定义集合
            IList style_list = mCube.ViewService.GetViewStyleByViewMain(vm, false);//查询视图格式集合

            CubeRegister cr = mCube.CubeManagerSrv.GetCubeRegisterById(vm.CubeRegId.Id);
            IList ca_list = mCube.CubeManagerSrv.GetCubeAttrByCubeResgisterId(cr);//立方属性集合
            string ifYwzz = vm.IfYwzz;

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
            //现在最后一个位置为事实，为了在mCube.TransSaveExpress里能正确处理，使K+1；
            k++;

            Hashtable ht_result = mCube.GetFreeFlexData(vm, time_str);
            //第一次查询调用存储过程生成数据
            if (string.IsNullOrEmpty(vwInfo.Id))
            {
                IList cd_list = new ArrayList();
                foreach (ViewRuleDef vrd in rule_list)
                {
                    if (vrd.SaveExpress != null && !"".Equals(vrd.SaveExpress))
                    {
                        string saveExpress = vrd.SaveExpress;
                        saveExpress = mCube.TransSaveExpress(saveExpress, ifYwzz, k, time_str, ywzz_str, time_index, ywzz_index);

                        if ( !ht_result.Contains(saveExpress+MCubeManager.SaveExpressDelimiter[0]) )
                        {
                            CubeData cd = new CubeData();
                            IList dataList = new ArrayList();
                            string[] temp = saveExpress.Split(MCubeManager.SaveExpressDelimiter,StringSplitOptions.None);
                            for (int t = 1; t < temp.Length; t++)
                            {
                                dataList.Add((string)temp[t]);
                            }
                            dataList = mCube.CubeManagerSrv.transDisplayToOrder(style_list, ca_list, dataList);
                            cd.DimDataList = dataList;
                            cd_list.Add(cd);
                        }
                    }             
                }
                mCube.CubeManagerSrv.BatchInsertCubeDatas(cr, cd_list);
                SaveViewWriteInfo();
                ht_result = mCube.GetFreeFlexData(vm, time_str);//维度组合字符串和cubedata的对应
            }
            
            ht_exist_value = mCube.GetAllValueInVm(cr, rule_list, time_str, ywzz_str);
            DimensionCategory cate = mCube.DimManagerSrv.GetDimensionCategoryById(time_str);

            dgvWrite.AutoRedraw = false;
            foreach (ViewRuleDef vrd in rule_list)
            {
                if (vrd.SaveExpress != null && !"".Equals(vrd.SaveExpress))
                {
                    string saveExpress = vrd.SaveExpress;
                    string cellSign = vrd.CellSign;
                    saveExpress = mCube.TransSaveExpress(saveExpress, ifYwzz, k, time_str, ywzz_str, time_index, ywzz_index);             
                    
                    CubeData cd = ht_result[saveExpress + MCubeManager.SaveExpressDelimiter[0]] as CubeData;

                    //如果cd为空，新增数据库
                    if (cd == null)
                    {
                        cd = new CubeData();
                        IList dataList = new ArrayList();
                        //维度代码集合
                        IList codeList = new ArrayList();
                        foreach (CubeAttribute ca in ca_list)
                        {
                            codeList.Add(ca.DimensionCode);
                        }

                        //分隔符
                        string[] temp = saveExpress.Split(MCubeManager.SaveExpressDelimiter,StringSplitOptions.None);
                        for (int t = 1; t < temp.Length; t++)
                        {
                            dataList.Add((string)temp[t]);
                        }
                        dataList = mCube.CubeManagerSrv.transDisplayToOrder(style_list, ca_list, dataList);
                        cd.DimDataList = dataList;
                        cd.DimCodeList = codeList;
                        cr.CubeAttribute = ca_list;
                        string id = mCube.CubeManagerSrv.SetCubeData(cr, cd);
                        cd.Id = id;
                    }

                    string tempaa = vrd.CellSign;
                    if (tempaa.Equals("D6"))
                    {
                        string kkkk = "";
                    }

                    //第一次查询，计算从数据库中取值的表达式
                    string cal_value = "0";
                    string calexpress = vrd.CalExpress;
                    if (ifSave == false && cd.Result != 0)
                    {
                        ifSave = true;
                    }

                    if (cd.Result == 0 && calexpress != null && calexpress.IndexOf(MCubeManager.SaveExpressDelimiter[0]) != -1)
                    {
                        
                        if (calexpress.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) != -1)
                        {
                            calexpress = calexpress.Replace(KnowledgeUtil.YWZZ_DIM_STR, ywzz_str);
                        }
                        calexpress = mCube.TransCalExpress(calexpress, cate);
                        int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                        int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));
                        bool ifLetter = KnowledgeUtil.IfHaveUpperLetter(calexpress);
                        if (ifLetter == false)
                        {
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
                                    string exception = e.ToString();
                                    if (exception.IndexOf("zero") > 0 || exception.IndexOf("零") > 0)
                                    {
                                        cal_value = "0";
                                    }
                                    else
                                    {
                                        throw new Exception("系统错误：" + exception + " 提示：第（" + curr_row + "）行第（" + curr_col + "）列的计算错误，请查看公式定义！");
                                    }
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
                        }
                        cd.Result = double.Parse(cal_value);
                    }

                    string sign = vrd.CellSign;
                    //取出列字符
                    int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                    int row = int.Parse(KnowledgeUtil.SplitStr(sign, 1));
                    if (cd.Plan != null && !"".Equals(cd.Plan))
                    {
                        dgvWrite.Cell(row, col).Text = cd.Plan;
                    }
                    else
                    {
                        dgvWrite.Cell(row, col).Text = cd.Result + "";
                    }
                    dgvWrite.Cell(row, col).Tag = cd.Id;
                    save_data_list.Add(vrd);

                    if (vrd.CalExpress != null && !"".Equals(vrd.CalExpress))
                    {
                        cal_data_list.Add(vrd);
                    }
                }
                else if (vrd.CalExpress != null && !"".Equals(vrd.CalExpress))
                {
                    cal_data_list.Add(vrd);
                }
                else
                {
                    display_data_list.Add(vrd);
                }
            }

            //计算表达式值
            if (ifSave == false || 0 == 0)
            {
                mCube.CalGridValue(rule_list, dgvWrite, ht_exist_value, time_str, ywzz_str);
            }

            //重新计算存在表间计算的公式
            this.CalCurrDataGrid();

            dgvWrite.AutoRedraw = true;
            dgvWrite.Refresh();
        }

        //计算存在表达式的单元格的值(无用)
        private void CalFlexData()
        {
            DataTable dt = new DataTable();
            IList curr_cal_list = new ArrayList();//计算规则集合的拷贝
            IList sign_list = new ArrayList();//当前单元格的代号集合
            foreach (ViewRuleDef vrd in cal_data_list)
            {
                sign_list.Add(vrd.CellSign);
                curr_cal_list.Add(vrd);
            }
            DimensionCategory cate = mCube.DimManagerSrv.GetDimensionCategoryById(time_str);
            //
             // 1：目前已经存在的单元格代号集合sign_list
             // 2：循环10次，当sign_list为空即停止循环
             // 3：每次循环已经计算的单元格从sign_list中去除，当前计算的原则为不包含在sign_list中的单元格
             //
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
                    currExpress = mCube.TransCalExpress(currExpress, cate);

                    string cellSign = vrd.CellSign;

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
                        signList.Add(temp[i]);
                    }

                    if (ifHave == false)
                    {
                        foreach (string sign in signList)
                        {
                            string value = "";
                            if (sign.IndexOf("_") == -1)
                            {
                                int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                                int row = int.Parse(KnowledgeUtil.SplitStr(sign, 1));
                                value = dgvWrite.Cell(row, col).Text;
                            }
                            else
                            {
                                object o = ht_exist_value[sign];
                                if (o != null)
                                {
                                    value = o.ToString();
                                }
                            }

                            if (value == null || "".Equals(value))
                            {
                                value = "0";
                            }
                            
                           currExpress = currExpress.Replace("[" + sign + "]", value);
                        }

                        if (currExpress.IndexOf("[") == -1)
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

                        if (haveCal == true)
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

                if (rule_express.IndexOf("备注") != -1)
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Remark;
                }
                else if (rule_express.IndexOf("备用1") != -1 && vwInfo.Standby1 != null && !"".Equals(vwInfo.Standby1))
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby1;
                }
                else if (rule_express.IndexOf("备用2") != -1 && vwInfo.Standby2 != null && !"".Equals(vwInfo.Standby2))
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby2;
                }
                else if (rule_express.IndexOf("备用3") != -1 && vwInfo.Standby3 != null && !"".Equals(vwInfo.Standby3))
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby3;
                }
                else if (rule_express.IndexOf("备用4") != -1 && vwInfo.Standby4 != null && !"".Equals(vwInfo.Standby4))
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby4;
                }
                else if (rule_express.IndexOf("备用5") != -1 && vwInfo.Standby5 != null && !"".Equals(vwInfo.Standby5))
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby5;
                }
                else
                {
                    string text = this.GetDisplayStrByRule(rule_express);
                    if (text.IndexOf("{") == -1)
                    {
                        dgvWrite.Cell(curr_row, curr_col).Text = this.GetDisplayStrByRule(rule_express);
                    }
                }
            }
        }


        private string GetDisplayStrByRule(string rule_express)
        {
            string dim_time = edtTime.Text;
            string dim_ywzz = cboYwzz.Text;

            if (rule_express.IndexOf("{定义时间}") != -1)
            {
                rule_express = rule_express.Replace("{定义时间}", dim_time);
            }

            if (rule_express.IndexOf("{定义部门}") != -1)
            {
                rule_express = rule_express.Replace("{定义部门}", dim_ywzz);
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
        #endregion

        #region 自定义事件
        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                vm = cboView.SelectedItem as ViewMain;
                if (vm == null) return;

                //查询此视图的业务组织集合
                IList ywzz_list = mCube.GetCurrYwzzList(vm);
                org_name = ConstObject.TheOperationOrg.Name;
                org_syscode = ConstObject.TheOperationOrg.SysCode;

                IList curr_ywzz_list = new ArrayList();
                foreach (ViewStyleDimension vsd in ywzz_list)
                {
                    if (!string.IsNullOrEmpty(vsd.DimCatId))
                    {
                        //20120423修改
                        //DimensionCategory cat = mCube.DimManagerSrv.GetDimensionCategoryById(vsd.DimCatId);
                        //if (org_syscode.IndexOf("." + cat.ResourceId + ".") != -1)//通过名字匹配
                        //{
                        //    curr_ywzz_list.Add(vsd);
                        //    break;
                        //}
                        if (vsd.Name == org_name)
                        {
                            curr_ywzz_list.Add(vsd);
                            break;
                        }
                    }
                }

                if (curr_ywzz_list.Count > 0 )
                {
                    cboYwzz.DataSource = curr_ywzz_list;
                    cboYwzz.DisplayMember = "Name";
                    cboYwzz.ValueMember = "DimCatId";
                }
                else {
                    MessageBox.Show("模板未定义该岗位对应的业务部门！");
                    return;
                }
                edtTime.Text = null;
                edtTime.Tag = null;
                
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找模板对应的时间集合出错。", ex);
            }
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
            string time_id="";
            foreach (CubeAttribute attr in attr_list)
            {
                if (attr.DimensionName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1)
                {
                    time_id = attr.DimensionId;
                    break;
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
                    edtTime.Text = vrd.selectDimName;
                    edtTime.Tag = vrd.selectDimId;
                }
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
            vwInfo = new ViewWriteInfo();
            ifSave = false;

            ExploreFile eFile = new ExploreFile();
            if (cboYwzz.Text == null || cboYwzz.Text.IndexOf("请选择") != -1)
            {
                MessageBox.Show("请选择业务组织！");
                return;
            }

            if (edtTime.Text == null || "".Equals(edtTime.Text))
            {
                MessageBox.Show("请选择时间！");
                return;
            }

            ywzz_str = cboYwzz.SelectedValue.ToString();
            time_str = edtTime.Tag as string;

            GetViewWriteInfo(vm, time_str, ywzz_str);//查询视图录入信息

            ControlButton(ifSave);

            if (eFile.IfExistFileInServer(vm.ViewName + ".flx"))
            {
                LoadFlexFile();
                LoadFlexData();
                DisplayRuleData();
            }
            else {
                MessageBox.Show("未定义该模板的格式文件！");
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            CubeRegister cr = mCube.CubeManagerSrv.GetCubeRegisterById(vm.CubeRegId.Id);
            IList save_list = new ArrayList();

            //重新计算存在表间计算的公式
            this.CalCurrDataGrid();

            foreach (ViewRuleDef vrd in save_data_list)
            {
                CubeData cd = new CubeData();
                string sign = vrd.CellSign;
                //取出列字符
                int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                int row = int.Parse(KnowledgeUtil.SplitStr(sign, 1));
                string cd_id = dgvWrite.Cell(row, col).Tag as string;
                cd.Id = cd_id;
                string value = dgvWrite.Cell(row, col).Text;
                if (value != null && !"".Equals(value))
                {
                    value = value.Trim();
                }
                if ( value != null && !"".Equals(value) && KnowledgeUtil.IfWidthNumber(value) == false)
                {
                    cd.Plan = value;
                }
                else
                {
                    if (value != null && !"".Equals(value))
                    {
                        cd.Result = double.Parse(value);
                    }
                    else
                    {
                        cd.Result = 0;
                    }
                }
                save_list.Add(cd);
            }
            mCube.CubeManagerSrv.BatchUpdateCubeDatasById(cr, save_list);

            SaveViewWriteInfo();
            GetViewWriteInfo(vm, time_str, ywzz_str);//查询视图录入信息

            //保存当前内容
            string fileName = "c:\\windows\\temp\\" + vm.Id + "_" + time_str + ".flx";
            //fileName = "c:\\windows\\temp\\2TLycIDvz8kvlj7e5ZKS$A_0yVv27E_X08hXyguKnx3s5.flx";
            dgvWrite.SaveFile(fileName);
            ExploreFile eFile = new ExploreFile();
            byte[] result = eFile.GetBytes(fileName);
            mCube.CubeManagerSrv.SaveFileToServerByResult(vm.Id + "_" + time_str + ".flx", result);
            
            MessageBox.Show("保存数据成功！");
        }

        /// <summary>
        ///　提交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(vwInfo.Id))
            {
                MessageBox.Show("请先保存数据！");
                return;
            }
            else
            {
                vwInfo.State = KnowledgeUtil.VIEWWRITE_SUBMIT_CODE + "";
                mCube.ViewService.SaveViewWriteInfo(vwInfo);

                MessageBox.Show("提交数据完成！");
            }
        }

        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPreview_Click(object sender, EventArgs e)
        {
            mCube.GeneralPreviewWithNoFix(dgvWrite);
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

        private void dgvWrite_LeaveCell(object Sender, FlexCell.Grid.LeaveCellEventArgs e)
        {
            //取得当前的单元格
            FlexCell.Cell cell = dgvWrite.ActiveCell;
            int row = cell.Row;
            int col = cell.Col;
            string sign = KnowledgeUtil.Init_str[col].ToString() + row;

            //得到和当前活动单元格有关的计算表达式(相关单元格)
            IList curr_express_list = this.GetCurrCalExpress(sign);

            mCube.CalGridValue(curr_express_list, dgvWrite, ht_exist_value, time_str, ywzz_str);
        }

        private void SaveViewWriteInfo() 
        {
            //查询是否定义了备注
            string bz = "_";
            string standby1 = "_";
            string standby2 = "_";
            string standby3 = "_";
            string standby4 = "_";
            string standby5 = "_";
            foreach (ViewRuleDef vrd in display_data_list)
            {
                string sign = vrd.CellSign;
                int col_bz = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                int row_bz = int.Parse(KnowledgeUtil.SplitStr(sign, 1));
                if (vrd.DisplayRule.IndexOf("备注") != -1)
                {
                    bz = dgvWrite.Cell(row_bz, col_bz).Text;
                }

                if (vrd.DisplayRule.IndexOf("备用1") != -1)
                {
                    standby1 = dgvWrite.Cell(row_bz, col_bz).Text;
                }

                if (vrd.DisplayRule.IndexOf("备用2") != -1)
                {
                    standby2 = dgvWrite.Cell(row_bz, col_bz).Text;
                }

                if (vrd.DisplayRule.IndexOf("备用3") != -1)
                {
                    standby3 = dgvWrite.Cell(row_bz, col_bz).Text;
                }

                if (vrd.DisplayRule.IndexOf("备用4") != -1)
                {
                    standby4 = dgvWrite.Cell(row_bz, col_bz).Text;
                }

                if (vrd.DisplayRule.IndexOf("备用5") != -1)
                {
                    standby5 = dgvWrite.Cell(row_bz, col_bz).Text;
                }
            }

            if (string.IsNullOrEmpty(vwInfo.Id))
            {
                vwInfo.Main = vm;
                vwInfo.TheJob = ConstObject.TheSysRole;
                vwInfo.TheOpeOrg = ConstObject.TheOperationOrg;
                vwInfo.Author = ConstObject.LoginPersonInfo;
                vwInfo.CreateDate = ConstObject.LoginDate;
                vwInfo.TimeDimId = time_str;
                vwInfo.YwzzDimId = ywzz_str;
                vwInfo.State = KnowledgeUtil.VIEWWRITE_CREATE_CODE + "";
            }
            if (bz != null && !bz.Equals("_"))
            {
                vwInfo.Remark = bz;
            }
            if (standby1 != null && !standby1.Equals("_"))
            {
                vwInfo.Standby1 = standby1;
            }
            if (standby2 != null && !standby2.Equals("_"))
            {
                vwInfo.Standby2 = standby2;
            }
            if (standby3 != null && !standby3.Equals("_"))
            {
                vwInfo.Standby3 = standby3;
            }
            if (standby4 != null && !standby4.Equals("_"))
            {
                vwInfo.Standby4 = standby4;
            }
            if (standby5 != null && !standby5.Equals("_"))
            {
                vwInfo.Standby5 = standby5;
            }
            mCube.ViewService.SaveViewWriteInfo(vwInfo);
        }

        private IList GetCurrCalExpress(string sign)
        {
            IList curr_express_list = new ArrayList();
            //循环8次
            IList rel_list = new ArrayList();//与该单元格有计算关系的单元格代号集合
            IList exist_sign_list = new ArrayList();//已经添加的计算单元格代号
            rel_list.Add(sign);

            for (int i = 0; i < 8; i++)
            {
                foreach (ViewRuleDef vrd in cal_data_list)
                {
                    string curr_express = vrd.CalExpress;
                    if (!exist_sign_list.Contains(vrd.CellSign))
                    {
                        foreach (string rel_sign in rel_list)
                        {
                            if (curr_express.IndexOf(rel_sign) != -1)
                            {
                                rel_list.Add(vrd.CellSign);
                                exist_sign_list.Add(vrd.CellSign);
                                curr_express_list.Add(vrd);
                                break;
                            }
                        }
                    }
                }
            }

            /*
            //把计算表达式拆分到最小单元
            Hashtable ht_initexpress = this.GetInitExpress(cal_data_list);           

            //和当前单元格有关的计算表达式
            foreach (ViewRuleDef vrd in cal_data_list)
            {
                string curr_sign = vrd.CellSign;
                string express = ht_initexpress[curr_sign].ToString();

                if (express.IndexOf(sign) != -1)
                {
                    curr_express_list.Add(vrd);
                }
                else
                {
                    bool ifDefine = KnowledgeUtil.IfDefineFunction(express);//判断是否存在自定义公式   
                    if (express != null && ifDefine == true)
                    {
                        curr_express_list.Add(vrd);
                    }
                }
            }*/
            return curr_express_list;
        }

        //循环
        private Hashtable GetInitExpress(IList cal_express_list)
        {
            Hashtable ht_initexpress = new Hashtable();//解除所有的表达式嵌套
            Hashtable ht_copy = new Hashtable();
            foreach (ViewRuleDef vrd in cal_express_list)
            {
                ht_copy.Add(vrd.CellSign, vrd.CalExpress);
            }

            for (int t = 0; t < 10; t++)
            {
                if (ht_copy.Count == 0)
                {
                    break;
                }

                Hashtable ht_temp = new Hashtable();
                foreach (string sign in ht_copy.Keys)
                {
                    ht_temp.Add(sign, ht_copy[sign]);
                }

                foreach (string sign in ht_temp.Keys)
                {
                    string express = ht_temp[sign].ToString();
                    if (KnowledgeUtil.IfHaveUpperLetter(express) == false)
                    {
                        ht_initexpress.Add(sign, express);
                        ht_copy.Remove(sign);
                    }
                    else {
                        bool isOk = true;
                        char[] pattern ={ '[', ']' };
                        string[] temp = express.Split(pattern);
                        for (int i = 1; i < temp.Length; i = i + 2)
                        {
                            string get_express = temp[i].ToString();
                            if (KnowledgeUtil.IfHaveUpperLetter(get_express) == true)
                            {
                                if (ht_copy[get_express] != null)
                                {
                                    isOk = false;
                                }
                                else
                                {
                                    if (ht_initexpress[get_express] != null)
                                    {
                                        express = express.Replace("[" + get_express + "]", ht_initexpress[get_express].ToString());
                                    }
                                }
                            }
                        }
                        if (isOk == true)
                        {
                            ht_initexpress.Add(sign, express);
                            ht_copy.Remove(sign);
                        }
                    }
                }           
            }        
            return ht_initexpress;
        }

        //对当前表间数据进行计算,计算公式为当前单元格
        private void CalCurrDataGrid()
        {
            DataTable dt = new DataTable();
            IList curr_express_list = new ArrayList();
            //循环5次
            IList exist_sign_list = new ArrayList();//已经完成计算的单元格代号
            IList all_sign_list = new ArrayList();//所有存在计算的单元格代号
            
            //所有存在表间计算关系的计算表达式
            foreach (ViewRuleDef vrd in cal_data_list)
            {
                string curr_express = vrd.CalExpress;
                if (curr_express.IndexOf("[") != -1 && curr_express.IndexOf("_") == -1)
                {
                    curr_express_list.Add(vrd);
                    all_sign_list.Add(vrd.CellSign);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                foreach (ViewRuleDef vrd in curr_express_list)
                {
                    string curr_express = vrd.CalExpress;
                    bool ifDefine = KnowledgeUtil.IfDefineFunction(curr_express);//判断是否存在自定义公式
                    if (ifDefine == true)
                    {
                        curr_express = mCube.GetFunctionExpress(curr_express, dgvWrite);
                    }

                    //计算未完成计算的单元格
                    if (!exist_sign_list.Contains(vrd.CellSign))
                    {
                        bool ifHaveCal = true;//是否可以计算
                        foreach (string rel_sign in all_sign_list)
                        {
                            if (curr_express.IndexOf(rel_sign) != -1)
                            {
                                if (!exist_sign_list.Contains(rel_sign))
                                {
                                    ifHaveCal = false;
                                    break;
                                }
                            }
                        }

                        //进行表间计算
                        if (ifHaveCal == true)
                        {
                            string cellSign = vrd.CellSign;
                            int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                            int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));

                            string cal_value = "0";
                            char[] pattern ={ '[', ']' };
                            string[] temp = curr_express.Split(pattern);
                            for (int t = 1; t < temp.Length; t = t + 2)
                            {
                                string get_express = temp[t].ToString();
                                int get_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(get_express, 2));
                                int get_row = int.Parse(KnowledgeUtil.SplitStr(get_express, 1));
                                string value = dgvWrite.Cell(get_row, get_col).Text;
                                //判断此数值是否为数字
                                value = value.Trim();
                                if (!KnowledgeUtil.IfWidthNumber(value) || value == null || "".Equals(value)) {
                                    value = "0";
                                }

                                curr_express = curr_express.Replace("[" + get_express + "]", value);
                            }

                            try
                            {
                                cal_value = dt.Compute(curr_express, "").ToString();
                            }
                            catch (Exception e)
                            {
                                string exception = e.ToString();
                                if (exception.IndexOf("zero") > 0 || exception.IndexOf("零") > 0)
                                {
                                    cal_value = "0";
                                }
                                else
                                {
                                    throw new Exception("系统错误：" + exception + " 提示：第（" + curr_row + "）行第（" + curr_col + "）列的计算错误，请查看公式定义！");
                                }
                            }
                            if (cal_value.IndexOf("无穷") != -1 || cal_value.IndexOf("非数字") != -1)
                            {
                                cal_value = "0";
                            }
                            if (cal_value != null && !"".Equals(cal_value))
                            {
                                cal_value = Math.Round(double.Parse(cal_value), 4) + "";
                            }

                            dgvWrite.Cell(curr_row, curr_col).Text = cal_value;
                            exist_sign_list.Add(cellSign);
                        }
                    }
                }
            }
        }
        #endregion
        
    }
}