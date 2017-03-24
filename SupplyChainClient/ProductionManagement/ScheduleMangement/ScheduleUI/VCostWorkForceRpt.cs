using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Microsoft.Office.Interop.Excel;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using System.IO;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    public partial class VCostWorkForceRpt : TBasicDataView
    {
        private CurrentProjectInfo projectInfo;
        private MProductionMng model = new MProductionMng();
        private string costStr = "劳动力预测统计报表";
        public VCostWorkForceRpt()
        {
            InitializeComponent();
            InitData();
            InitEvents();
        }

        private void InitData()
        {
            this.dtpDateBegin.Value = DateTime.Now.AddMonths(-1);
            this.dtpDateEnd.Value = DateTime.Now;
            projectInfo = StaticMethod.GetProjectInfo();
            //LoadFlexCellTemplate();
        }

        private void LoadFlexCellTemplate()
        {
            ExploreFile eFile = new ExploreFile();
            string modelName = "劳动力预测统计报表.flx";
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                throw new InvalidOperationException("未找到模板格式文件【" + modelName + "】");
            }
        }

        private void InitEvents()
        {
            this.btnQuery.Click += new EventHandler(btnQuery_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            string fileName = fGridDetail.ExportToExcel(costStr, false, false, true);
            if (fileName == "")
                return;
            FlashScreen.Show("正在导出劳动力预测统计报表...");
            ApplicationClass excel = new ApplicationClass();  // 1. 创建Excel应用程序对象的一个实例，相当于我们从开始菜单打开Excel应用程序
            //主文件对象
            Workbook workbook = excel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet;
            try
            {
                workbook.Save();
            }
            catch (Exception e1)
            {
                throw new Exception("导出劳动力预测统计报表出错！");
            }
            finally
            {
                FlashScreen.Close();
                //关闭工作表和退出Excel
                workbook.Close(false, Type.Missing, Type.Missing);
             
                excel.Quit();
                excel = null;
            }
            MessageBox.Show("导出劳动力预测统计报表成功！");
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            LoadFlexCellTemplate();
            System.Data.DataTable dt = GetRptSourceDT();

            //设置外观
            CommonUtil.SetFlexGridFace(this.fGridDetail);
            this.fGridDetail.Column(1).Width = 220;
            this.fGridDetail.Column(2).Width = 120;
            this.fGridDetail.Column(3).Width = 160;
            this.fGridDetail.Column(4).Width = 160;
            this.fGridDetail.Column(5).Width = 120;
           FillFlex(dt);
            
        }

        private void FillFlex(System.Data.DataTable dt)
        {
            fGridDetail.Rows = dt.Rows.Count + 5;
            //this.fGridDetail.Range(3, 1, 3, 5).Merge();
            this.fGridDetail.Cell(2,1).Text = GetSqlWhere(false);
            this.fGridDetail.Cell(2, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            this.fGridDetail.Cell(2, 1).FontName = "宋体";
            this.fGridDetail.Cell(2, 1).FontSize = 9;
            this.fGridDetail.Cell(2, 1).FontBold = true;

            //this.fGridDetail.Range(4, 1, 4, 5).Merge();
            this.fGridDetail.Cell(3, 1).Text = GetResourceSumInfo(dt);
            this.fGridDetail.Cell(3, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            this.fGridDetail.Cell(3, 1).FontName = "宋体";
            this.fGridDetail.Cell(3, 1).FontSize = 9;
            this.fGridDetail.Cell(3, 1).FontBold = true;

            int rowindex = 5;
            foreach (DataRow dr in dt.Rows)
            {
                fGridDetail.Cell(rowindex, 1).Text = ClientUtil.ToString(dr["Task"]);
          
                fGridDetail.Cell(rowindex, 2).Text = ClientUtil.ToString(dr["ResourceName"]);
                fGridDetail.Cell(rowindex, 3).Text = ClientUtil.ToDecimal(dr["MinNeedQutity"]).ToString("########################0.##"); //合同最大
                fGridDetail.Cell(rowindex, 4).Text = ClientUtil.ToDecimal(dr["MaxNeedQutity"]).ToString("########################0.##");
               
                fGridDetail.Cell(rowindex, 5).Text = "工日";
                rowindex++;
            }
            fGridDetail.AutoRedraw = true;
            fGridDetail.Refresh();
        }

        private string GetResourceSumInfo(System.Data.DataTable dt)
        {
            string str_rtn = "资源需求信息汇总：";
            DataView dv = new DataView(dt);
            System.Data.DataTable dt2 = dv.ToTable(true, "ResourceName");
            foreach (DataRow  item in dt2.Rows)
            {
                string rn = ClientUtil.ToString(item["ResourceName"]);
                DataRow[] drs = dt.Select("ResourceName = '" + rn + "'");
                decimal num_max = 0, num_min = 0;
                foreach (DataRow dr in drs)
                {
                    num_max += ClientUtil.ToDecimal(dr["MaxNeedQutity"]);
                    num_min += ClientUtil.ToDecimal(dr["MinNeedQutity"]);
                }
                str_rtn += (rn + "：【" + num_min.ToString("########################0.##") + "到" + num_max.ToString("########################0.##") + "】；");
            }
            return str_rtn;
        }
        private System.Data.DataTable GetRptSourceDT()
        {
            string sqlwhere = GetSqlWhere(true);
            string sql = string.Format(@"select t2.name as Task
       ,t5.resourcetypename as ResourceName
       ,sum(t5.maxworkdays *t3.planworkamount) as MaxNeedQutity
       ,sum(t5.minworkdays * t3.planworkamount) as MinNeedQutity
   from thd_weekscheduledetail t1
 inner join thd_weekschedulemaster t on t.id = t1.parentid
 inner join thd_gwbstree t2 on   t2.id = t1.gwbstree
 inner join thd_gwbsdetail t3 on t3.parentid = t2.id
 inner join thd_costitem t4 on  t4.id = t3.costitemguid 
 inner join thd_costworkforce t5 on t5.costitemid = t4.id
 where 1 = 1
   and t.execscheduletype = 40
   and t.projectid = '{0}'  {1} group by t2.name,t5.resourcetypename ", projectInfo.Id, sqlwhere);
            System.Data.DataTable dt = model.ProductionManagementSrv.GetData(sql);
            return dt;
        }

        private string GetSqlWhere(bool isSql)
        {
            string sqlwhere = "";
            string sqlText = "查询条件：";
            if (this.dtpDateBegin.Checked && !this.dtpDateEnd.Checked)
            {
                string  beginTime = dtpDateBegin.Value.Date.ToShortDateString();
                sqlwhere += string.Format(@" and (t1.PlannedBeginDate <= to_date('{0}','yyyy-MM-dd') and  t1.PlannedEndDate >= to_date('{0}','yyyy-MM-dd') )", beginTime);
                sqlText += string.Format(@"计划时间：{0}-{1}；",beginTime,"?");
            }
            if (!this.dtpDateBegin.Checked && this.dtpDateEnd.Checked)
            {
                string  endTime = dtpDateEnd.Value.Date.ToShortDateString();
                sqlwhere += string.Format(@" and (t1.PlannedBeginDate <= to_date('{0}','yyyy-MM-dd') and  t1.PlannedEndDate >= to_date('{0}','yyyy-MM-dd') )", endTime);
                sqlText += string.Format(@"计划时间：{0}-{1}；", "?", endTime);

            }

            if (this.dtpDateBegin.Checked && this.dtpDateEnd.Checked)
            {
                string beginTime = dtpDateBegin.Value.Date.ToShortDateString();
                string endTime = dtpDateEnd.Value.Date.ToShortDateString();
                sqlwhere += string.Format(@" and ((t1.PlannedBeginDate < to_date('{0}','yyyy-MM-dd') and  t1.PlannedEndDate > to_date('{0}','yyyy-MM-dd') ) 
or(t1.PlannedBeginDate < to_date('{1}','yyyy-MM-dd') and  t1.PlannedEndDate > to_date('{1}','yyyy-MM-dd') )
or (t1.PlannedBeginDate >= to_date('{0}','yyyy-MM-dd') and  t1.PlannedEndDate <= to_date('{0}','yyyy-MM-dd') )) ",beginTime, endTime);
                sqlText += string.Format(@"计划时间：{0}-{1}；", beginTime, endTime);
            }
            return isSql ? sqlwhere : sqlText;

        }
    }
}
