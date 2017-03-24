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
    public partial class VDataQuery : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private ViewMain vm = new ViewMain();
        private ViewWriteInfo vwInfo = new ViewWriteInfo();
        string time_str = "";
        string time_code = "";
        string ywzz_str = "";
        string job_name = "";//录入岗位
        long org_id = -1;
        string org_syscode = "";//部门的系统代码
        string org_name = "";//录入部门
        string person_name = "";//录入人
        string createtime = "";//录入时间
        IList display_data_list = new ArrayList();//显示的数据

        public VDataQuery()
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
            btnPreview.Click += new EventHandler(btnPreview_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnTime.Click += new EventHandler(btnTime_Click);

            cboView.SelectedIndexChanged += new EventHandler(cboView_SelectedIndexChanged);
        }


        #region 私有函数

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
            }
        }

        //引导表格设计器的格式文件
        private void LoadFlexFile()
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            //eFile.CreateFileFromServer("ReportResult\\" + vm.Id + "_" + time_str + ".flx");
            //载入格式和数据
            dgvWrite.OpenFile(path + "\\" + "ReportResult\\" + vm.Id + "_" + time_str + ".flx");//载入格式
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
                        DimensionCategory cat = mCube.DimManagerSrv.GetDimensionCategoryById(vsd.DimCatId);
                        if (org_syscode.IndexOf("." + cat.ResourceId + ".") != -1)//通过名字匹配
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
                time_code = dimCode;
                bool ifRightTime = KnowledgeUtil.IfRightTime(dimCode, vm.CollectTypeCode);
                if (ifRightTime == false)
                {
                    edtTime.Text = null;
                    edtTime.Tag = null;
                    MessageBox.Show("请选择正确的时间,可能原因：日、月、季度、年份请正确选取！");
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
            vwInfo = new ViewWriteInfo();

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


            if (eFile.IfExistFileInServer("ReportResult\\" + vm.Id + "_" + time_str + ".flx"))
            {
                LoadFlexFile();
                DisplayRuleData();
            }
            else
            {
                MessageBox.Show("未查询到历史数据！");
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

        #endregion
        
    }
}