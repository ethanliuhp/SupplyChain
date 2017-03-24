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
    public partial class VSubcontractAmountReport : TBasicDataView
    {
        ICommonMethodSrv service = CommonMethod.CommonMethodSrv;
        string detailExptr = "分包单位工程量台账";
        string flexName = "分包单位工程量台账.flx";
        CurrentProjectInfo projectInfo;

        public VSubcontractAmountReport()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            this.fGridDetail.Rows = 1;
            for (int i = 2007; i <= DateTime.Now.Year; i++)
            {
                cbYear.Items.Add(i);
            }
            cbYear.Text = DateTime.Now.Year.ToString();
            LoadTempleteFile(flexName);
            projectInfo = StaticMethod.GetProjectInfo();

        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vmros = new VContractExcuteSelector();
            vmros.ShowDialog();
            IList list = vmros.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtPenaltyRank.Text = engineerMaster.BearerOrgName;
            txtPenaltyRank.Tag = engineerMaster;
        }


        void btnExcel_Click(object sender, EventArgs e)
        {
            fGridDetail.ExportToExcel(detailExptr, false, false, true);
        }

        private void LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式
                if (modelName == flexName)
                {
                    fGridDetail.OpenFile(path + "\\" + modelName);//载入格式
                }
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + modelName);
                return;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            LoadTempleteFile(flexName);
            LoadDetailFile();
        }

        private DataTable GetTimeSpanForYear(string year)
        {
            string sqlTimeSpan = string.Format("select fiscalyear Ayear,fiscalmonth Amonth,begindate Astart,enddate Aend from resfiscalperioddet where fiscalyear={0} order by fiscalyear asc", year);
            var timeDt = service.GetData(sqlTimeSpan).Tables[0];
            if (timeDt == null || timeDt.Rows.Count == 0)
            {
                throw new Exception("统计时间段为空！");
            }
            return timeDt;
        }

        private void LoadDetailFile()
        {
            FlashScreen.Show("正在生成[" + detailExptr + "]报告...");
            try
            {
                fGridDetail.AutoRedraw = false;

                // 首先取出统计的时间段

                TimeSlice ts = new TimeSlice(Convert.ToInt32(cbYear.Text));

                var startTime = ts.Slice[0].Value[0].ToString("yyyy-MM-dd");
                var endTime = ts.Slice[11].Value[1].ToString("yyyy-MM-dd");

                // 其次查询出所有的统计数据
                var condition = string.Format(" and t1.createdate<=to_date('{0}','yyyy-mm-dd') and t1.projectid='{1}'", endTime, projectInfo.Id);
                string deptStr = "分包单位";
                if (!string.IsNullOrEmpty(txtPenaltyRank.Text))
                {
                    condition += " and t1.subcontractprojectid='" + ((SubContractProject)txtPenaltyRank.Tag).Id + "'";
                    deptStr = txtPenaltyRank.Text;
                }
                string sql = "select t2.balancetaskdtlguid id,t2.balancetaskdtlname name,t2.balanceprice price,t2.quantityunitname unit,t1.subcontractunitguid unitid,t1.subcontractunitname unitname,t3.responsibilitilyworkamount workamount,0 changeamount,0 total,t1.createdate,t2.balacnequantity,t2.usedescript remark from thd_subcontractbalancebill t1,thd_subcontractbalancedetail t2,thd_gwbsdetail t3 where t1.state=5 and t1.id=t2.parentid and t2.balancetaskdtlguid=t3.id and t2.fontbilltype=1" + condition + " order by t1.subcontractunitname desc";
                var dt = service.GetData(sql).Tables[0];

                if (dt == null && dt.Rows.Count == 0) return;
                var result = dt.Select().Select(a => new EngineeringAmount()
                {
                    Id = a["id"].ToString(),
                    Name = a["name"].ToString(),
                    Price = Convert.ToDecimal(a["price"]),
                    Unit = a["unit"] + string.Empty,
                    UnitId = a["unitid"].ToString(),
                    UnitName = a["unitname"].ToString(),
                    WorkAmount = Convert.ToDecimal(a["workamount"]),
                    ChangeAmount = Convert.ToDecimal(a["changeamount"]),
                    Total = Convert.ToDecimal(a["total"]),
                    CreateTime = Convert.ToDateTime(a["createdate"]),
                    Num = Convert.ToDecimal(a["balacnequantity"]),
                    Remark = a["remark"] + string.Empty
                });

                var num = 6;        // 记录添加的行数的位置
                // 先按分包队伍分组，再按任务分组
                var groupDept = result.GroupBy(a => a.UnitName);
                foreach (var dept in groupDept)
                {
                    // 获取分包队伍的工程量信息
                    var tempDept = result.Where(a => a.UnitName == dept.Key);
                    // 钢筋|吨，混凝土|立方米，模板|平方米 过滤
                    tempDept = FilterSameIndex(tempDept, fGridDetail, "钢筋", "吨", ts, ref num);
                    tempDept = FilterSameIndex(tempDept, fGridDetail, "混凝土", "立方米", ts, ref num);
                    tempDept = FilterSameIndex(tempDept, fGridDetail, "模板", "平方米", ts, ref num);

                    // 其他任务处理
                    FilterSameIndex(tempDept, fGridDetail, ts, ref num);
                }


                string sTitle = string.Format("{1}项目{0}{2}工程量台账", cbYear.Text, projectInfo.Name, deptStr);
                fGridDetail.Cell(1, 1).Text = sTitle;
                fGridDetail.Cell(1, 9).Text = cbYear.Text + "年各月结算量";

                for (int tt = 0; tt < fGridDetail.Cols; tt++)
                {
                    fGridDetail.Column(tt).AutoFit();
                }
            }
            catch (Exception e1)
            {
                throw new Exception("生成[" + detailExptr + "]报告异常[" + e1.Message + "]");
            }
            finally
            {
                fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
                fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;

                fGridDetail.AutoRedraw = true;
                fGridDetail.Refresh();
                FlashScreen.Close();
            }
        }
        /// <summary>
        /// 过滤特殊的任务明细
        /// </summary>
        /// <param name="result">需要过滤的列表</param>
        /// <param name="sheet">过滤后显示的表格</param>
        /// <param name="indexName">过滤指标</param>
        /// <param name="unit">计量单位</param>
        /// <param name="ts">统计时间区间</param>
        /// <param name="num">表格中显示的行号</param>
        /// <returns>返回过滤掉指定指标后的结果集</returns>
        private IEnumerable<EngineeringAmount> FilterSameIndex(IEnumerable<EngineeringAmount> result, CustomFlexGrid sheet, string indexName, string unit, TimeSlice ts, ref int num)
        {
            // 查询出所有含有指定名称、计量单位的记录
            var records = result.Where(a => a.Name.Contains(indexName) && a.Unit == unit);
            if (records == null || records.Count() == 0)
            {
                // 不存在过滤数据则直接返回
                return result;
            }
            // 按单价分组
            var groupPrice = records.GroupBy(a => a.Price);
            foreach (var price in groupPrice)
            {
                // 查询出单价相同的记录
                var tempResult = records.Where(a => a.Price == price.Key).ToList();

                // 如果只有一条记录，名称取任务明细的名称；如果有多条，则采用指标名称
                var tempName = indexName;
                if (tempResult.Count() == 1)
                {
                    tempName = tempResult[0].Name;
                }
                // 计算小于当前年份的累计金额
                var total = tempResult.Where(a => a.CreateTime < ts.Slice[0].Value[0]).Sum(a => a.Num);                          // 总金额
                var p = price.Key;                                  // 单价
                var workAmount = tempResult.Where(a => a.CreateTime <= ts.Slice[11].Value[1]).Sum(a => a.WorkAmount);           // 总图纸量
                sheet.InsertRow(num++, 1);                    // 添加一行
                sheet.Cell(num - 2, 1).Text = (num - 6).ToString();   // 序号
                sheet.Cell(num - 2, 2).Text = tempName;               // 任务明细
                sheet.Cell(num - 2, 3).Text = tempResult[0].UnitName; // 队伍名称
                sheet.Cell(num - 2, 4).Text = tempResult[0].Unit;     // 计量单位
                sheet.Cell(num - 2, 5).Text = p.ToString();           // 单价
                sheet.Cell(num - 2, 6).Text = workAmount.ToString();  // 图纸量
                for (int i = 0; i < ts.Slice.Count; i++)              // 计算每个月的累积量
                {
                    if (ts.Slice[i].Value[0] > DateTime.Now) break;
                    var summaryResult = tempResult.Where(a => a.CreateTime >= ts.Slice[i].Value[0] && a.CreateTime <= ts.Slice[i].Value[1]);
                    var summary = summaryResult.Sum(a => a.Num);
                    sheet.Cell(num - 2, i * 2 + 9).Text = summary.ToString();
                    total += summary;
                    sheet.Cell(num - 2, i * 2 + 10).Text = total.ToString();
                }
            }
            return result.Where(a => !(a.Name.Contains(indexName) && a.Unit == unit));
        }
        /// <summary>
        /// 过滤任务明细、计量单位、单价相同的记录
        /// </summary>
        /// <param name="result">待过滤列表</param>
        /// <param name="sheet">显示表格</param>
        /// <param name="ts">时间区间</param>
        /// <param name="num">当前行号</param>
        private void FilterSameIndex(IEnumerable<EngineeringAmount> result, CustomFlexGrid sheet, TimeSlice ts, ref int num)
        {
            var groupName = result.GroupBy(a => a.Name);        // 首先根据任务明细分组
            foreach (var name in groupName)
            {
                var sameNameList = result.Where(a => a.Name == name.Key);
                var groupUnit = sameNameList.GroupBy(a => a.Unit);  // 再根据计量单位分组
                foreach (var unit in groupUnit)
                {
                    var sameUnitList = sameNameList.Where(a => a.Unit == unit.Key);
                    var priceGroup = sameUnitList.GroupBy(a => a.Price);  // 最后根据单价分组
                    foreach (var price in priceGroup)
                    {
                        var samePriceList = sameUnitList.Where(a => a.Price == price.Key).ToList();
                        var total = samePriceList.Where(a => a.CreateTime < ts.Slice[0].Value[0]).Sum(a => a.Num);                          // 往年总累积量
                        var remark = string.Empty;
                        var workAmount = samePriceList.Where(a => a.CreateTime <= ts.Slice[11].Value[1]).Sum(a => a.WorkAmount);           // 总图纸量
                        // 添加表单记录
                        sheet.InsertRow(num++, 1);                    // 添加一行
                        sheet.Cell(num - 2, 1).Text = (num - 6).ToString();   // 序号
                        sheet.Cell(num - 2, 2).Text = name.Key;               // 任务明细
                        sheet.Cell(num - 2, 3).Text = samePriceList[0].UnitName; // 队伍名称
                        sheet.Cell(num - 2, 4).Text = unit.Key;     // 计量单位
                        sheet.Cell(num - 2, 5).Text = price.Key.ToString();   // 单价
                        sheet.Cell(num - 2, 6).Text = workAmount.ToString();  // 图纸量
                        for (int i = 0; i < ts.Slice.Count; i++)              // 计算每个月的累积量
                        {
                            if (ts.Slice[i].Value[0] > DateTime.Now) break;
                            var summaryResult = samePriceList.Where(a => a.CreateTime >= ts.Slice[i].Value[0] && a.CreateTime <= ts.Slice[i].Value[1]);
                            var summary = summaryResult.Sum(a => a.Num);
                            sheet.Cell(num - 2, i * 2 + 9).Text = summary.ToString();
                            total += summary;
                            sheet.Cell(num - 2, i * 2 + 10).Text = total.ToString();
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// 工程量
    /// </summary>
    class EngineeringAmount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal WorkAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public decimal Total { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal Num { get; set; }
        public string Remark { get; set; }
    }
}