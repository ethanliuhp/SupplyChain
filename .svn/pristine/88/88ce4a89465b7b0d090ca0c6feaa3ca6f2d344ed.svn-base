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


using Application.Business.Erp.SupplyChain.Client.BasicData;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Indicator.BasicData;
using Application.Business.Erp.SupplyChain.Client.Basic;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VViewFreeQuery : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private MBasicData baiscModel = new MBasicData();
        private ViewMain vm = new ViewMain();
        private ViewWriteInfo vwInfo = new ViewWriteInfo();
        private bool ifFs = true;
        public bool ifRefers = false;//是否资产经营系统引用技术管理报表
        string time_str = "";
        string ywzz_str = "";
        string job_name = "";//录入岗位
        string org_name = "";//录入部门
        string person_name = "";//录入人
        string createtime = "";//录入时间

        IList view_list = new ArrayList();//视图集合

        //定义规则表达式中三种类型
        IList save_data_list = new ArrayList();//存储的数据
        IList cal_data_list = new ArrayList();//计算的数据
        IList display_data_list = new ArrayList();//显示的数据
        Hashtable ht_exist_value = new Hashtable();//该模板涉及的表达式中已经存在的值

        public VViewFreeQuery()
        {
            InitializeComponent();
            InitialEvents();
        }

        internal void Start()
        {
            InitialControls();
        }

        private void InitialEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnPreview.Click += new EventHandler(btnPreview_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnFs.Click += new EventHandler(btnFs_Click);
            btnTime.Click += new EventHandler(btnTime_Click);           

            cboView.SelectedIndexChanged += new EventHandler(cboView_SelectedIndexChanged);
        }

        private void InitialControls()
        {
            cboReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            string sysCode = ConstObject.TheSystemCode;
            VBasicDataOptr.InitIndicatorReportType(cboReportType, true);            

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("SystemId", ConstObject.TheSystemCode));

            oq.AddCriterion(Expression.Eq("ViewTypeCode", "3"));
            oq.AddOrder(Order.Asc("IfDisplaySonMother"));
            oq.AddOrder(Order.Asc("ViewName"));
            view_list = mCube.ViewService.GetViewMainByQuery(oq);

            LoadReportType();
            
        }

        #region 私有函数
        //按钮控制，反审按钮
        private void ControlButton(bool enabled)
        {
            if (enabled == true)
            {
                btnFs.Enabled = true;
            }
            else
            {
                btnFs.Enabled = false;
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
                if (state != null && (KnowledgeUtil.VIEWWRITE_SUBMIT_CODE + "").Equals(state))
                {
                    ifFs = true;
                }
                else
                {
                    ifFs = false;
                }
                org_name = vwInfo.TheOpeOrg.Name;
                person_name = vwInfo.Author.Name;
                job_name = vwInfo.TheJob.RoleName;
                createtime = vwInfo.CreateDate.ToShortDateString();
            }
            else {
                vwInfo = new ViewWriteInfo();
                org_name = "";
                person_name = "";
                job_name = "";
                createtime = "";
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

        //从技术管理服务器目录下引导表格设计器的格式文件
        private void LoadFlexFileFromKn()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            eFile.CreateEnergyFileFromKnowledge(vm.ViewName + ".flx");
            //载入格式和数据
            dgvWrite.OpenFile(path + "\\" + vm.ViewName + ".flx");//载入格式
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

            IList rule_list = mCube.ViewService.GetViewRuleDefByViewMain(vm);//查询视图规则定义集合
            IList style_list = mCube.ViewService.GetViewStyleByViewMain(vm, false);//查询视图格式集合
            CubeRegister cr = mCube.CubeManagerSrv.GetCubeRegisterById(vm.CubeRegId.Id);
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
            ht_exist_value = mCube.GetAllValueInVm(cr, rule_list, time_str, ywzz_str);
            string ifYwzz = vm.IfYwzz;

            foreach (ViewRuleDef vrd in rule_list)
            {
                if (vrd.SaveExpress != null && !"".Equals(vrd.SaveExpress))
                {
                    string saveExpress = vrd.SaveExpress;
                    saveExpress = mCube.TransSaveExpress(saveExpress, ifYwzz, k, time_str, ywzz_str, time_index, ywzz_index);

                    CubeData cd = ht_result[saveExpress + "_"] as CubeData;
                    string sign = vrd.CellSign;
                    //取出列字符
                    int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(sign, 2));
                    int row = int.Parse(KnowledgeUtil.SplitStr(sign, 1));

                    if (cd == null)
                    {
                        dgvWrite.Cell(row, col).Text = "0";
                    }
                    else
                    {
                        if (cd.Plan != null && !"".Equals(cd.Plan))
                        {
                            dgvWrite.Cell(row, col).Text = cd.Plan;
                        }
                        else
                        {
                            dgvWrite.Cell(row, col).Text = cd.Result + "";
                        }
                        dgvWrite.Cell(row, col).Tag = cd.Id + "";
                        save_data_list.Add(vrd);
                    }

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
        }


        //引导已经存储的结果文件
        private void LoadResultFlexFile()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            eFile.CreateFileFromServer("ReportResult\\" + vm.Id + "_" + time_str + ".flx");
            //载入格式和数据
            dgvWrite.OpenFile(path + "\\" + "ReportResult\\" + vm.Id + "_" + time_str + ".flx");//载入格式
        }


        //计算存在表达式的单元格的值(无用)
        private void CalFlexData()
        {
            DataTable dt = new DataTable();
            foreach (ViewRuleDef vrd in cal_data_list)
            {
                string cal_value = "0";
                string currExpress = vrd.CalExpress;
                string cellSign = vrd.CellSign;
                char[] patten = { '[', ']' };//分隔符
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
                    if (cal_value.IndexOf("无穷") != -1 || cal_value.IndexOf("非数字") != -1)
                    {
                        cal_value = "0";
                    }
                    if (cal_value != null && !"".Equals(cal_value))
                    {
                        cal_value = Math.Round(double.Parse(cal_value), 4) + "";
                    }
                }

                int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));
                dgvWrite.Cell(curr_row, curr_col).Text = cal_value;
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
                else if (rule_express.IndexOf("备用1") != -1)
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby1;
                }
                else if (rule_express.IndexOf("备用2") != -1)
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby2;
                }
                else if (rule_express.IndexOf("备用3") != -1)
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby3;
                }
                else if (rule_express.IndexOf("备用4") != -1)
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby4;
                }
                else if (rule_express.IndexOf("备用5") != -1)
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = vwInfo.Standby5;
                }
                else
                {
                    dgvWrite.Cell(curr_row, curr_col).Text = this.GetDisplayStrByRule(rule_express);
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

        private void LoadReportType()
        {
            string type = cboReportType.SelectedValue as string;
            if (type == null || "".Equals(type))
            {
                type = "1";
            }

            IList curr_list = new ArrayList();
            foreach (ViewMain vm in view_list)
            {
                string v_type = vm.IfDisplaySonMother;
                if (v_type.Equals(type))
                {
                    curr_list.Add(vm);
                }
            }

            cboView.DataSource = curr_list;
            cboView.DisplayMember = "ViewName";
        }
        #endregion


        #region 自定义事件
        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                vm = cboView.SelectedItem as ViewMain;
                if (vm == null) return;

                //查询此视图的时间集合
                /*IList time_list = mCube.GetCurrTimeList(vm);
                cboTime.DataSource = time_list;
                cboTime.DisplayMember = "Name";
                cboTime.ValueMember = "DimCatId";*/

                //查询此视图的时间集合
                IList ywzz_list = mCube.GetCurrYwzzList(vm);
                cboYwzz.DataSource = ywzz_list;
                cboYwzz.DisplayMember = "Name";
                cboYwzz.ValueMember = "DimCatId";
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
            string time_id = "";
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
            ifFs = false;
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
            ControlButton(ifFs);

            /*if (ifFs == false) {
                MessageBox.Show("该模板数据还没有提交！");
                return;
            }*/

            if (eFile.IfExistFileInServer("ReportResult\\" + vm.Id + "_" + time_str + ".flx"))
            {
                LoadResultFlexFile();
                DisplayRuleData();
            }
            else
            {
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
                else
                {
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

                dgvWrite.AutoRedraw = false;

                LoadFlexData();
                DisplayRuleData();

                dgvWrite.AutoRedraw = true;
                dgvWrite.Refresh();
            }           
        }

        /// <summary>
        ///　反审数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnFs_Click(object sender, EventArgs e)
        {
            vwInfo.State = KnowledgeUtil.VIEWWRITE_CREATE_CODE + "";
            if (string.IsNullOrEmpty(vwInfo.Id))
            {
                MessageBox.Show("请选查询数据！");
                return;
            }
            mCube.ViewService.SaveViewWriteInfo(vwInfo);
            MessageBox.Show("反审完成！");
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

        private void dgvWrite_KeyUp(object Sender, KeyEventArgs e)
        {
            //mCube.CalGridValue(cal_data_list, dgvWrite, ht_exist_value, time_str, ywzz_str);
        }
        

        private void cboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportType();
        }
        #endregion

    }
}