using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using IRPServiceModel.Services.Common;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VMultiDimensionRpt : TBasicDataView
    {
        MPenaltyDeductionMng model =new MPenaltyDeductionMng ();

        CurrentProjectInfo projectInfo;

        int CurTabIndex = 0;
        string[] arrfileName = { "主体部分造价", "主体部分含量", "非实体部分造价" };
        CustomFlexGrid[] arrGrid;

        public VMultiDimensionRpt()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {
            arrGrid = new CustomFlexGrid[3] { flexGridMainCost, flexGridMainContent, flexGridNoMainCost };
            foreach (var item in arrGrid)
            {
                item.Rows = 1;
            }
            projectInfo = StaticMethod.GetProjectInfo();
            LoadTempleteFile();

            btnQuery.Visible = (projectInfo != null && !string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode));

        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            //btnExcel.Click += new EventHandler(btnExcel_Click);
            //btnSearch.Click += new EventHandler(btnSearch_Click);
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurTabIndex = tabControl1.SelectedIndex;
            LoadTempleteFile();
            if (CurTabIndex == 2)
            {
                btnQuery.Visible = true;
            }
        }

     

        void btnExcel_Click(object sender, EventArgs e)
        {
            //arrGrid[CurTabIndex].ExportToExcel(detailExptr, false, false, true);
        }

        private void LoadTempleteFile()
        {
            string fileName = arrfileName[CurTabIndex] + ".flx";
            var grid = arrGrid[CurTabIndex];
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(fileName))
            {
                eFile.CreateTempleteFileFromServer(fileName);
                //载入格式
                grid.OpenFile(path + "\\" + fileName);
            }
            else
            {
                MessageBox.Show("未找到模板格式文件" + fileName);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
               
                    LoadTempleteFile( );
                    LoadData();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询失败:"+ExceptionUtil.ExceptionMessage(ex));
            }
        }
        #region 数据加载部分
        public void LoadData()
        {
            switch (CurTabIndex)
            {
                case 0:
                    {
                        if (projectInfo != null && !string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))
                        {
                            MainCost();
                          
                        }
                        break;
                    }
                case 1:
                    {
                        if (projectInfo != null && !string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))
                        {
                            MainContent();
                          
                        }
                        break;
                    }
                case 2:
                    {
                        NomainCost();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// 主体部分造价
        /// </summary>
        public void MainCost()
        {
            int iCount = 0,iStart=3,iRow=0,iCol=0; 
            DataSet ds = model.PenaltyDeductionSrv.GetMainCostData(projectInfo.Id);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) throw new Exception("未查询到结果集");
            DataTable oTable= ds.Tables[0];
            CustomFlexGrid oFlexGrid = flexGridMainCost;
            oFlexGrid.AutoRedraw = false;
            FlexCell.Cell oCell = null;
            oFlexGrid.InsertRow(iStart,oTable.Rows.Count);
            decimal dArea = 0,   dMoney = 0;
            decimal dSumArea=0,dSumMoney=0;
            decimal dSumGJMoney = 0,  dSumSTMoney = 0,  dSumMBMoney=0, dSumQTMoney=0;
            foreach (DataRow oRow in oTable.Rows)
            {//id, pbsname,subjectname,tlevel,area,gjQty,gjMoney,stQty,stMoney,mbQty,mbMoney,qtQty,qtMoney,qty,money
                iRow = iStart + iCount; iCol = 1; oCell = oFlexGrid.Cell(iRow, iCol);//序号
                oCell.Text = ClientUtil.ToString(iCount+1);
                iCol = 2; oCell = oFlexGrid.Cell(iRow, iCol);//施工部位
                oCell.Text = ClientUtil.ToString(oRow["pbsname"]);oCell.WrapText=false;
                iCol = 3; oCell = oFlexGrid.Cell(iRow, iCol);//面积
                dArea = ClientUtil.ToDecimal(oRow["area"]); dSumArea += dArea; oCell.Text = ClientUtil.ToString(dArea);
                iCol = 4; oCell = oFlexGrid.Cell(iRow, iCol);//项目总成本
                dMoney = ClientUtil.ToDecimal(oRow["money"]); dSumMoney += dMoney; oCell.Text = ClientUtil.ToString(dMoney);
                iCol = 5; oCell = oFlexGrid.Cell(iRow, iCol);//单平米成本
                oCell.Text = (dArea == 0 ? "0" : (dMoney / dArea).ToString("N2"));
                iCol = 6; oCell = oFlexGrid.Cell(iRow, iCol);//钢筋成本成本
                dMoney = ClientUtil.ToDecimal(oRow["gjMoney"]); dSumGJMoney += dMoney; oCell.Text = ClientUtil.ToString(dMoney);
                iCol = 7; oCell = oFlexGrid.Cell(iRow, iCol);//钢筋单平米成本
                oCell.Text = dArea == 0 ? "0" : (dMoney / dArea).ToString("N2");
                iCol = 8; oCell = oFlexGrid.Cell(iRow, iCol);//商品砼成本
                dMoney = ClientUtil.ToDecimal(oRow["stMoney"]); dSumSTMoney += dMoney; oCell.Text = dMoney.ToString();
                iCol = 9; oCell = oFlexGrid.Cell(iRow, iCol);//商品单平米成本
                oCell.Text = dArea == 0 ? "0" : (dMoney / dArea).ToString("N2");
                iCol = 10; oCell = oFlexGrid.Cell(iRow, iCol);//模板成本
                dMoney = ClientUtil.ToDecimal(oRow["mbMoney"]); dSumMBMoney += dMoney; oCell.Text = dMoney.ToString();
                iCol = 11; oCell = oFlexGrid.Cell(iRow, iCol);//模板单平米成本
                oCell.Text = dArea == 0 ? "0" : (dMoney / dArea).ToString("N2");
                iCol = 12; oCell = oFlexGrid.Cell(iRow, iCol);//砌体成本
                dMoney = ClientUtil.ToDecimal(oRow["qtMoney"]); dSumQTMoney += dMoney; oCell.Text = dMoney.ToString();
                iCol = 13; oCell = oFlexGrid.Cell(iRow, iCol);//砌体单平米成本
                oCell.Text = dArea == 0 ? "0" : (dMoney / dArea).ToString("N2");
                oFlexGrid.Row(iRow).AutoFit();
                iCount++;
            }
            iRow = iStart + iCount;
            iCol = 1; oCell = oFlexGrid.Cell(iRow, iCol);
            oCell.Text = "合计";
            iCol = 3; oCell = oFlexGrid.Cell(iRow, iCol);//面积
            oCell.Text =dSumArea.ToString();
            iCol = 4; oCell = oFlexGrid.Cell(iRow, iCol);//项目总成本
            oCell.Text = dSumMoney.ToString();
            iCol = 6; oCell = oFlexGrid.Cell(iRow, iCol);//钢筋成本成本
            oCell.Text = dSumGJMoney.ToString();
            iCol = 8; oCell = oFlexGrid.Cell(iRow, iCol);//商品砼成本
            oCell.Text = dSumSTMoney.ToString();
            iCol = 10; oCell = oFlexGrid.Cell(iRow, iCol);//模板成本
            oCell.Text = dSumMBMoney.ToString();
            iCol = 12; oCell = oFlexGrid.Cell(iRow, iCol);//砌体成本
            oCell.Text = dSumQTMoney.ToString();
            oFlexGrid.Row(iRow).AutoFit();
            oFlexGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            oFlexGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            for (iCol = 0; iCol < oFlexGrid.Cols; iCol++)
            {
                oFlexGrid.Column(iCol).AutoFit();
            }
            oFlexGrid.AutoRedraw = true;
            oFlexGrid.Refresh();
        }
        /// <summary>
        /// 主体部分含量
        /// </summary>
        public void MainContent()
        {
            int iCount = 0, iStart = 3, iRow = 0, iCol = 0;
            DataSet ds = model.PenaltyDeductionSrv.GetMainCostData(projectInfo.Id);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) throw new Exception("未查询到结果集");
            DataTable oTable = ds.Tables[0];
            CustomFlexGrid oFlexGrid = flexGridMainContent;
            oFlexGrid.AutoRedraw = false;
            FlexCell.Cell oCell = null;
            oFlexGrid.InsertRow(iStart, oTable.Rows.Count);
            decimal dArea = 0, dQty = 0, dGJQty = 0, dSTQty = 0, dMBQty = 0, dQTQty = 0;
            decimal dSumArea = 0,  dSumQty = 0;
            decimal   dSumGJQty = 0,   dSumSTQty=0, dSumMBQty=0,  dSumQTQty=0;
            foreach (DataRow oRow in oTable.Rows)
            {//id, pbsname,subjectname,tlevel,area,gjQty,gjMoney,stQty,stMoney,mbQty,mbMoney,qtQty,qtMoney,qty,money
                iRow = iStart + iCount; iCol = 1; oCell = oFlexGrid.Cell(iRow, iCol);//序号
                oCell.Text = ClientUtil.ToString(iCount + 1);
                iCol = 2; oCell = oFlexGrid.Cell(iRow, iCol);//施工部位
                oCell.Text = ClientUtil.ToString(oRow["pbsname"]); oCell.WrapText = false;
                iCol = 3; oCell = oFlexGrid.Cell(iRow, iCol);//面积
                dArea = ClientUtil.ToDecimal(oRow["area"]); dSumArea += dArea; oCell.Text = ClientUtil.ToString(dArea);
                iCol = 4; oCell = oFlexGrid.Cell(iRow, iCol);//钢筋
                dGJQty = ClientUtil.ToDecimal(oRow["gjQty"]); dSumGJQty += dGJQty; oCell.Text = ClientUtil.ToString(dGJQty);
                iCol = 5; oCell = oFlexGrid.Cell(iRow, iCol);//商品砼
                dSTQty = ClientUtil.ToDecimal(oRow["stQty"]); dSumSTQty += dSTQty; oCell.Text = ClientUtil.ToString(dSTQty);
                iCol = 6; oCell = oFlexGrid.Cell(iRow, iCol);//模板
                dMBQty = ClientUtil.ToDecimal(oRow["mbQty"]); dSumMBQty += dMBQty; oCell.Text = ClientUtil.ToString(dMBQty);
                iCol = 7; oCell = oFlexGrid.Cell(iRow, iCol);//单平方米钢筋
                oCell.Text = dArea == 0 ? "0" : (dGJQty*1000 / dArea).ToString("N2");
                iCol = 8; oCell = oFlexGrid.Cell(iRow, iCol);//单平方米商品砼
                oCell.Text = dArea == 0 ? "0" : (dSTQty/ dArea).ToString("N2");
                iCol = 9; oCell = oFlexGrid.Cell(iRow, iCol);//单平方米模板
                oCell.Text = dArea == 0 ? "0" : (dMBQty  / dArea).ToString("N2");
                iCol = 10; oCell = oFlexGrid.Cell(iRow, iCol);//单立方米商品砼含钢筋
                oCell.Text = dSTQty == 0 ? "0" : (dGJQty * 1000 / dSTQty).ToString("N2");

                iCol = 14; oCell = oFlexGrid.Cell(iRow, iCol);//砌体
                dQTQty = ClientUtil.ToDecimal(oRow["qtQty"]); dSumQTQty += dQTQty; oCell.Text =ClientUtil.ToString( dQTQty);
                iCol = 15; oCell = oFlexGrid.Cell(iRow, iCol);//单平方米砌体
                oCell.Text = dArea == 0 ? "0" : (dQTQty / dArea).ToString("N2");
                iCol = 16; oCell = oFlexGrid.Cell(iRow, iCol);//单立方米砌体含钢筋
                oCell.Text = dQTQty == 0 ? "0" : (dGJQty * 1000 / dQTQty).ToString("N2");
                oFlexGrid.Row(iRow).AutoFit();
                iCount++;
            }
            iRow = iStart + iCount;
            iCol = 1; oCell = oFlexGrid.Cell(iRow, iCol);
            oCell.Text = "合计";
            iCol = 3; oCell = oFlexGrid.Cell(iRow, iCol);//面积
            oCell.Text = dSumArea.ToString();
            iCol = 4; oCell = oFlexGrid.Cell(iRow, iCol);//钢筋
            oCell.Text = dSumGJQty.ToString();
            iCol = 5; oCell = oFlexGrid.Cell(iRow, iCol);//商品砼
            oCell.Text = dSumSTQty.ToString();
            iCol = 6; oCell = oFlexGrid.Cell(iRow, iCol);//模板
            oCell.Text = dSumMBQty.ToString();
            iCol = 14; oCell = oFlexGrid.Cell(iRow, iCol);//砌体
            oCell.Text = dSumQTQty.ToString();
            
            oFlexGrid.Row(iRow).AutoFit();
            oFlexGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            oFlexGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            for (iCol = 0; iCol < oFlexGrid.Cols; iCol++)
            {
                oFlexGrid.Column(iCol).AutoFit();
            }
            oFlexGrid.AutoRedraw = true;
            oFlexGrid.Refresh();
        }
        /// <summary>
        /// 非主体部分造价
        /// </summary>
        public void NomainCost()
        {
            int iCount = 0, iStart = 3, iRow = 0, iCol = 0;
            string sProjectID = string.Empty, sOrgSysCode = string.Empty;
            if (projectInfo == null)
            {
                sOrgSysCode = ConstObject.TheOperationOrg.SysCode;
            }
            else if (projectInfo.Code == CommonUtil.CompanyProjectCode)
            {
                sOrgSysCode = ConstObject.TheOperationOrg.SysCode;
            }
            else
            {
                sProjectID = projectInfo.Id;
            }
            DataSet ds = model.PenaltyDeductionSrv.GetNoMainCostData(sProjectID, sOrgSysCode);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) throw new Exception("未查询到结果集");
            DataTable oTable = ds.Tables[0];
            CustomFlexGrid oFlexGrid = flexGridNoMainCost;
            oFlexGrid.AutoRedraw = false;
            FlexCell.Cell oCell = null;
            oFlexGrid.InsertRow(iStart, oTable.Rows.Count);
            decimal dArea = 0, dQty = 0, dGLMoney = 0, dLJMoney = 0, dJXMoney = 0, dAQWMMoney = 0, dGFMoney = 0, dSJMoney=0;
            decimal dSumArea = 0, dSumGLMoney = 0, dSumLJMoney = 0, dSumJXMoney = 0, dSumAQWMMoney = 0, dSumGFMoney = 0, dSumSJMoney=0;
           
            foreach (DataRow oRow in oTable.Rows)
            {   //theprojectname,sum(constructionarea) area,sum(glQty)glQty,sum(glMoney)glMoney,sum(ljQty)ljQty,
                //sum(ljMoney)ljMoney,sum(jxQty)jxQty,sum(jxMoney)jxMoney,sum(aqwmQty)aqwmQty,sum(aqwmMoney)aqwmMoney,
                //sum(gfQty)gfQty,sum(gfMoney)gfMoney,sum(sjQty)sjQty,sum(sjMoney)sjMoney,sum(qty)qty,sum(money)money
                iRow = iStart + iCount; iCol = 1; oCell = oFlexGrid.Cell(iRow, iCol);//序号
                oCell.Text = ClientUtil.ToString(iCount + 1);
                iCol = 2; oCell = oFlexGrid.Cell(iRow, iCol);//项目
                oCell.Text = ClientUtil.ToString(oRow["theprojectname"]); oCell.WrapText = false;
                iCol = 3; oCell = oFlexGrid.Cell(iRow, iCol);//面积
                dArea = ClientUtil.ToDecimal(oRow["area"]); dSumArea += dArea; oCell.Text = ClientUtil.ToString(dArea);
                iCol = 4; oCell = oFlexGrid.Cell(iRow, iCol);//管理
                dGLMoney = ClientUtil.ToDecimal(oRow["glMoney"]); dSumGLMoney += dGLMoney; oCell.Text = ClientUtil.ToString(dGLMoney);
                iCol = 5; oCell = oFlexGrid.Cell(iRow, iCol);//每平方米管理
                oCell.Text = dArea==0?"0":(dGLMoney/dArea).ToString("N2");
                iCol = 6; oCell = oFlexGrid.Cell(iRow, iCol);//临建费
                dLJMoney = ClientUtil.ToDecimal(oRow["ljMoney"]); dSumLJMoney += dLJMoney; oCell.Text = ClientUtil.ToString(dLJMoney);
                iCol = 7; oCell = oFlexGrid.Cell(iRow, iCol);//每平方米临建费
                oCell.Text = dArea == 0 ? "0" : (dLJMoney / dArea).ToString("N2");
                iCol = 8; oCell = oFlexGrid.Cell(iRow, iCol);//机械
                dJXMoney = ClientUtil.ToDecimal(oRow["jxMoney"]); dSumJXMoney += dJXMoney; oCell.Text = ClientUtil.ToString(dJXMoney);
                iCol = 9; oCell = oFlexGrid.Cell(iRow, iCol);//每平方米机械
                oCell.Text = dArea == 0 ? "0" : (dJXMoney / dArea).ToString("N2");
                iCol = 10; oCell = oFlexGrid.Cell(iRow, iCol);//安全文明
                dAQWMMoney = ClientUtil.ToDecimal(oRow["aqwmMoney"]); dSumAQWMMoney += dAQWMMoney; oCell.Text = ClientUtil.ToString(dAQWMMoney);
                iCol = 11; oCell = oFlexGrid.Cell(iRow, iCol);//每平方米安全文明
                oCell.Text = dArea == 0 ? "0" : (dAQWMMoney / dArea).ToString("N2");
                iCol = 12; oCell = oFlexGrid.Cell(iRow, iCol);//规费
                dGFMoney = ClientUtil.ToDecimal(oRow["gfMoney"]); dSumGFMoney += dGFMoney; oCell.Text = ClientUtil.ToString(dGFMoney);
                iCol = 13; oCell = oFlexGrid.Cell(iRow, iCol);//税金
                dSJMoney = ClientUtil.ToDecimal(oRow["sjMoney"]); dSumSJMoney += dSJMoney; oCell.Text = ClientUtil.ToString(dSJMoney);
                oFlexGrid.Row(iRow).AutoFit();
                iCount++;
            }
            iRow = iStart + iCount;
            iCol = 1; oCell = oFlexGrid.Cell(iRow, iCol);
            oCell.Text = "合计";
            iCol = 3; oCell = oFlexGrid.Cell(iRow, iCol);//面积
            oCell.Text = ClientUtil.ToString(dSumArea);
            iCol = 4; oCell = oFlexGrid.Cell(iRow, iCol);//管理
            oCell.Text = ClientUtil.ToString(dSumGLMoney);
            iCol = 6; oCell = oFlexGrid.Cell(iRow, iCol);//临建费
            oCell.Text = ClientUtil.ToString(dSumLJMoney);
            iCol = 8; oCell = oFlexGrid.Cell(iRow, iCol);//机械
            oCell.Text = ClientUtil.ToString(dSumJXMoney);
            iCol = 10; oCell = oFlexGrid.Cell(iRow, iCol);//安全文明
            oCell.Text = ClientUtil.ToString(dSumAQWMMoney);
            iCol = 12; oCell = oFlexGrid.Cell(iRow, iCol);//规费
            oCell.Text = ClientUtil.ToString(dSumGFMoney);
            iCol = 13; oCell = oFlexGrid.Cell(iRow, iCol);//税金
            oCell.Text = ClientUtil.ToString(dSumSJMoney);
               

            oFlexGrid.Row(iRow).AutoFit();
            oFlexGrid.BackColor1 = System.Drawing.SystemColors.ButtonFace;
            oFlexGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            oFlexGrid.Range(3, 3, oFlexGrid.Rows - 1, oFlexGrid.Cols - 1).Alignment = FlexCell.AlignmentEnum.RightCenter;
            for (iCol = 0; iCol < oFlexGrid.Cols; iCol++)
            {
                oFlexGrid.Column(iCol).AutoFit();
            }
            oFlexGrid.AutoRedraw = true;
            oFlexGrid.Refresh();
        }
        #endregion
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //VContractExcuteSelector vmros = new VContractExcuteSelector();
            //vmros.ShowDialog();
            //IList list = vmros.Result;
            //if (list == null || list.Count == 0) return;
            //SubContractProject engineerMaster = list[0] as SubContractProject;
            //txtPenaltyRank.Text = engineerMaster.BearerOrgName;
            //txtPenaltyRank.Tag = engineerMaster;
        }

        #region MyRegion
        //private DataTable GetTimeSpanForYear(string year)
        //{
        //    string sqlTimeSpan = string.Format("select fiscalyear Ayear,fiscalmonth Amonth,begindate Astart,enddate Aend from resfiscalperioddet where fiscalyear={0} order by fiscalyear asc", year);
        //    var timeDt = service.GetData(sqlTimeSpan).Tables[0];
        //    if (timeDt == null || timeDt.Rows.Count == 0)
        //    {
        //        throw new Exception("统计时间段为空！");
        //    }
        //    return timeDt;
        //}

        //private void LoadDetailFile()
        //{
        //    FlashScreen.Show("正在生成[" + detailExptr + "]报告...");
        //    try
        //    {
        //        fGridDetail.AutoRedraw = false;

        //        // 首先取出统计的时间段

        //        TimeSlice ts = new TimeSlice(Convert.ToInt32(cbYear.Text));

        //        var startTime = ts.Slice[0].Value[0].ToString("yyyy-MM-dd");
        //        var endTime = ts.Slice[11].Value[1].ToString("yyyy-MM-dd");

        //        // 其次查询出所有的统计数据
        //        var condition = string.Format(" and t1.createdate<=to_date('{0}','yyyy-mm-dd') and t1.projectid='{1}'", endTime, projectInfo.Id);
        //        string deptStr = "分包单位";
        //        if (!string.IsNullOrEmpty(txtPenaltyRank.Text))
        //        {
        //            condition += " and t1.subcontractprojectid='" + ((SubContractProject)txtPenaltyRank.Tag).Id + "'";
        //            deptStr = txtPenaltyRank.Text;
        //        }
        //        string sql = "select t2.balancetaskdtlguid id,t2.balancetaskdtlname name,t2.balanceprice price,t2.quantityunitname unit,t1.subcontractunitguid unitid,t1.subcontractunitname unitname,t3.responsibilitilyworkamount workamount,0 changeamount,0 total,t1.createdate,t2.balacnequantity,t2.usedescript remark from thd_subcontractbalancebill t1,thd_subcontractbalancedetail t2,thd_gwbsdetail t3 where t1.state=5 and t1.id=t2.parentid and t2.balancetaskdtlguid=t3.id and t2.fontbilltype=1" + condition + " order by t1.subcontractunitname desc";
        //        var dt = service.GetData(sql).Tables[0];

        //        if (dt == null && dt.Rows.Count == 0) return;
        //        var result = dt.Select().Select(a => new EngineeringAmount()
        //        {
        //            Id = a["id"].ToString(),
        //            Name = a["name"].ToString(),
        //            Price = Convert.ToDecimal(a["price"]),
        //            Unit = a["unit"] + string.Empty,
        //            UnitId = a["unitid"].ToString(),
        //            UnitName = a["unitname"].ToString(),
        //            WorkAmount = Convert.ToDecimal(a["workamount"]),
        //            ChangeAmount = Convert.ToDecimal(a["changeamount"]),
        //            Total = Convert.ToDecimal(a["total"]),
        //            CreateTime = Convert.ToDateTime(a["createdate"]),
        //            Num = Convert.ToDecimal(a["balacnequantity"]),
        //            Remark = a["remark"] + string.Empty
        //        });

        //        var num = 6;        // 记录添加的行数的位置
        //        // 先按分包队伍分组，再按任务分组
        //        var groupDept = result.GroupBy(a => a.UnitName);
        //        foreach (var dept in groupDept)
        //        {
        //            // 获取分包队伍的工程量信息
        //            var tempDept = result.Where(a => a.UnitName == dept.Key);
        //            // 钢筋|吨，混凝土|立方米，模板|平方米 过滤
        //            tempDept = FilterSameIndex(tempDept, fGridDetail, "钢筋", "吨", ts, ref num);
        //            tempDept = FilterSameIndex(tempDept, fGridDetail, "混凝土", "立方米", ts, ref num);
        //            tempDept = FilterSameIndex(tempDept, fGridDetail, "模板", "平方米", ts, ref num);

        //            // 其他任务处理
        //            FilterSameIndex(tempDept, fGridDetail, ts, ref num);
        //        }


        //        string sTitle = string.Format("{1}项目{0}{2}工程量台账", cbYear.Text, projectInfo.Name, deptStr);
        //        fGridDetail.Cell(1, 1).Text = sTitle;
        //        fGridDetail.Cell(1, 9).Text = cbYear.Text + "年各月结算量";

        //        for (int tt = 0; tt < fGridDetail.Cols; tt++)
        //        {
        //            fGridDetail.Column(tt).AutoFit();
        //        }
        //    }
        //    catch (Exception e1)
        //    {
        //        throw new Exception("生成[" + detailExptr + "]报告异常[" + e1.Message + "]");
        //    }
        //    finally
        //    {
        //        fGridDetail.BackColor1 = System.Drawing.SystemColors.ButtonFace;
        //        fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;

        //        fGridDetail.AutoRedraw = true;
        //        fGridDetail.Refresh();
        //        FlashScreen.Close();
        //    }
        //}
        ///// <summary>
        ///// 过滤特殊的任务明细
        ///// </summary>
        ///// <param name="result">需要过滤的列表</param>
        ///// <param name="sheet">过滤后显示的表格</param>
        ///// <param name="indexName">过滤指标</param>
        ///// <param name="unit">计量单位</param>
        ///// <param name="ts">统计时间区间</param>
        ///// <param name="num">表格中显示的行号</param>
        ///// <returns>返回过滤掉指定指标后的结果集</returns>
        //private IEnumerable<EngineeringAmount> FilterSameIndex(IEnumerable<EngineeringAmount> result, CustomFlexGrid sheet, string indexName, string unit, TimeSlice ts, ref int num)
        //{
        //    // 查询出所有含有指定名称、计量单位的记录
        //    var records = result.Where(a => a.Name.Contains(indexName) && a.Unit == unit);
        //    if (records == null || records.Count() == 0)
        //    {
        //        // 不存在过滤数据则直接返回
        //        return result;
        //    }
        //    // 按单价分组
        //    var groupPrice = records.GroupBy(a => a.Price);
        //    foreach (var price in groupPrice)
        //    {
        //        // 查询出单价相同的记录
        //        var tempResult = records.Where(a => a.Price == price.Key).ToList();

        //        // 如果只有一条记录，名称取任务明细的名称；如果有多条，则采用指标名称
        //        var tempName = indexName;
        //        if (tempResult.Count() == 1)
        //        {
        //            tempName = tempResult[0].Name;
        //        }
        //        // 计算小于当前年份的累计金额
        //        var total = tempResult.Where(a => a.CreateTime < ts.Slice[0].Value[0]).Sum(a => a.Num);                          // 总金额
        //        var p = price.Key;                                  // 单价
        //        var workAmount = tempResult.Where(a => a.CreateTime <= ts.Slice[11].Value[1]).Sum(a => a.WorkAmount);           // 总图纸量
        //        sheet.InsertRow(num++, 1);                    // 添加一行
        //        sheet.Cell(num - 2, 1).Text = (num - 6).ToString();   // 序号
        //        sheet.Cell(num - 2, 2).Text = tempName;               // 任务明细
        //        sheet.Cell(num - 2, 3).Text = tempResult[0].UnitName; // 队伍名称
        //        sheet.Cell(num - 2, 4).Text = tempResult[0].Unit;     // 计量单位
        //        sheet.Cell(num - 2, 5).Text = p.ToString();           // 单价
        //        sheet.Cell(num - 2, 6).Text = workAmount.ToString();  // 图纸量
        //        for (int i = 0; i < ts.Slice.Count; i++)              // 计算每个月的累积量
        //        {
        //            if (ts.Slice[i].Value[0] > DateTime.Now) break;
        //            var summaryResult = tempResult.Where(a => a.CreateTime >= ts.Slice[i].Value[0] && a.CreateTime <= ts.Slice[i].Value[1]);
        //            var summary = summaryResult.Sum(a => a.Num);
        //            sheet.Cell(num - 2, i * 2 + 9).Text = summary.ToString();
        //            total += summary;
        //            sheet.Cell(num - 2, i * 2 + 10).Text = total.ToString();
        //        }
        //    }
        //    return result.Where(a => !(a.Name.Contains(indexName) && a.Unit == unit));
        //}
        ///// <summary>
        ///// 过滤任务明细、计量单位、单价相同的记录
        ///// </summary>
        ///// <param name="result">待过滤列表</param>
        ///// <param name="sheet">显示表格</param>
        ///// <param name="ts">时间区间</param>
        ///// <param name="num">当前行号</param>
        //private void FilterSameIndex(IEnumerable<EngineeringAmount> result, CustomFlexGrid sheet, TimeSlice ts, ref int num)
        //{
        //    var groupName = result.GroupBy(a => a.Name);        // 首先根据任务明细分组
        //    foreach (var name in groupName)
        //    {
        //        var sameNameList = result.Where(a => a.Name == name.Key);
        //        var groupUnit = sameNameList.GroupBy(a => a.Unit);  // 再根据计量单位分组
        //        foreach (var unit in groupUnit)
        //        {
        //            var sameUnitList = sameNameList.Where(a => a.Unit == unit.Key);
        //            var priceGroup = sameUnitList.GroupBy(a => a.Price);  // 最后根据单价分组
        //            foreach (var price in priceGroup)
        //            {
        //                var samePriceList = sameUnitList.Where(a => a.Price == price.Key).ToList();
        //                var total = samePriceList.Where(a => a.CreateTime < ts.Slice[0].Value[0]).Sum(a => a.Num);                          // 往年总累积量
        //                var remark = string.Empty;
        //                var workAmount = samePriceList.Where(a => a.CreateTime <= ts.Slice[11].Value[1]).Sum(a => a.WorkAmount);           // 总图纸量
        //                // 添加表单记录
        //                sheet.InsertRow(num++, 1);                    // 添加一行
        //                sheet.Cell(num - 2, 1).Text = (num - 6).ToString();   // 序号
        //                sheet.Cell(num - 2, 2).Text = name.Key;               // 任务明细
        //                sheet.Cell(num - 2, 3).Text = samePriceList[0].UnitName; // 队伍名称
        //                sheet.Cell(num - 2, 4).Text = unit.Key;     // 计量单位
        //                sheet.Cell(num - 2, 5).Text = price.Key.ToString();   // 单价
        //                sheet.Cell(num - 2, 6).Text = workAmount.ToString();  // 图纸量
        //                for (int i = 0; i < ts.Slice.Count; i++)              // 计算每个月的累积量
        //                {
        //                    if (ts.Slice[i].Value[0] > DateTime.Now) break;
        //                    var summaryResult = samePriceList.Where(a => a.CreateTime >= ts.Slice[i].Value[0] && a.CreateTime <= ts.Slice[i].Value[1]);
        //                    var summary = summaryResult.Sum(a => a.Num);
        //                    sheet.Cell(num - 2, i * 2 + 9).Text = summary.ToString();
        //                    total += summary;
        //                    sheet.Cell(num - 2, i * 2 + 10).Text = total.ToString();
        //                }
        //            }
        //        }
        //    }
        //} 
        #endregion
    }
}
