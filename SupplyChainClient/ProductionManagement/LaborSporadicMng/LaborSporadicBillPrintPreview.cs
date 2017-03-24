using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Component.WinControls.Controls;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    //by zzb 20161110
    //加载零星/计时用工单、派工单数据至flx，预览打印
    static class LaborSporadicBillPrintPreview
    {
        internal static void Preview(LaborSporadicMaster master)
        {
            CustomFlexGrid flexGrid1 = new CustomFlexGrid();

            flexGrid1.Tag = master;
            flexGrid1.PrintPage += new FlexCell.Grid.PrintPageEventHandler(flexGrid1_PrintPage);

            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            string modelName = "零星用工单.flx";
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                throw new InvalidOperationException("未找到模板格式文件【" + modelName + "】");
            }
            flexGrid1.PageSetup.LeftMargin = 0;
            flexGrid1.PageSetup.RightMargin = 0;
            FillFlex(flexGrid1, master);
            flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
        }

        static void flexGrid1_PrintPage(object Sender, FlexCell.Grid.PrintPageEventArgs e)
        {
            CustomFlexGrid flexGrid1 = (CustomFlexGrid)Sender;
            if (!e.Preview && e.PageNumber == flexGrid1.TotalPages)
            {
                LaborSporadicMaster master = (LaborSporadicMaster)(flexGrid1.Tag);
                master.PrintTimes = master.PrintTimes + 1;

                MLaborSporadicMng model = new MLaborSporadicMng();
                model.SaveLaborSporadicMaster(master);

                bool instead = (master.LaborState == "代工");
                flexGrid1.Cell(2, instead ? 8 : 7).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(master.PrintTimes + 1);
            }
        }

        private static void FillFlex(CustomFlexGrid flexGrid1, LaborSporadicMaster billMaster)
        {
            bool instead = false;
            //主表数据
            if (billMaster.LaborState == "计时派工")
            {
                flexGrid1.Cell(2, 1).Text = "               计 时 派 工 单";
            }
            else if (billMaster.LaborState == "代工")
            {
                instead = true;
                flexGrid1.Cell(2, 1).Text = "                  代 工 单";
                flexGrid1.Cell(4, 4).Text = "承担队伍：";
                flexGrid1.Cell(8, 6).Text = "承担队伍签字： 	";
            }

            //单据号
            flexGrid1.Cell(3, 1).Text = "单据号:" + billMaster.Code;
            flexGrid1.Cell(3, 1).WrapText = true;
            //工程名称
            flexGrid1.Cell(4, 1).Text = "工程名称:" + billMaster.ProjectName;
            //制单日期
            flexGrid1.Cell(4, 4).Text = "制单日期:" + billMaster.CreateDate.ToShortDateString();
            //分包单位
            flexGrid1.Cell(4, 7).Text = "分包单位:" + billMaster.BearTeamName;

            //条形码
            flexGrid1.Cell(1, 7).Text = billMaster.Code.Substring(billMaster.Code.Length - 11) + "-0221"; //0221 is a magic num.
            flexGrid1.Cell(1, 7).CellType = FlexCell.CellTypeEnum.BarCode;
            flexGrid1.Cell(1, 7).BarcodeType = FlexCell.BarcodeTypeEnum.CODE128B;

            flexGrid1.Cell(2, 7).Text = "打印顺序号: " + CommonUtil.GetPrintTimesStr(billMaster.PrintTimes + 1);

            if (instead) //插入 '被代工队伍' 列
            {
                short w = flexGrid1.Column(3).Width; //宽度等于第3列
                short totalWidth = 0;
                for (int i = 0; i < 5; i++) //保证前6列不变，不至于影响条码等
                {
                    totalWidth += flexGrid1.Column(i).Width;
                }
                flexGrid1.InsertCol(5, 1);
                flexGrid1.Column(7).Width = w;
                double p = (double)totalWidth / (totalWidth + w);
                for (int i = 0; i < flexGrid1.Cols - 1; i++)
                {
                    flexGrid1.Column(i).Width = (short)(flexGrid1.Column(i).Width * p);
                }
                flexGrid1.Cell(5, 6).Text = "被代工队伍";
                flexGrid1.Cell(5, 6).WrapText = true;
            }

            //明细行
            int detailStartRowNumber = 6;
            int detailCount = billMaster.Details.Count;
            flexGrid1.InsertRow(detailStartRowNumber, detailCount - 1);

            CommonUtil.SetFlexGridPrintFace(flexGrid1);

            //设置单元格的边框，对齐方式
            FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
            range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            range.Mask = FlexCell.MaskEnum.Digital;

            int cellIndex = 1;
            //填写明细数据
            for (int i = 0; i < detailCount; i++)
            {
                LaborSporadicDetail detail = (LaborSporadicDetail)billMaster.Details.ElementAt(i);
                int cur = detailStartRowNumber + i;
                cellIndex = 1;
                //序号
                flexGrid1.Cell(cur, cellIndex).Text = (i + 1).ToString();
                //任务名称
                cellIndex++;
                flexGrid1.Cell(cur, cellIndex).Text = detail.ProjectTastName;
                flexGrid1.Cell(cur, cellIndex).WrapText = true;
                //工程内容
                cellIndex++;
                flexGrid1.Cell(cur, cellIndex).Text = detail.ProjectTastDetailName;
                flexGrid1.Cell(cur, cellIndex).WrapText = true;
                ////工长名称
                //cellIndex++;
                //flexGrid1.Cell(cur, cellIndex).Text = billMaster.HandlePersonName.ToString();
                //flexGrid1.Cell(cur, cellIndex).WrapText = true;
                //工长确认工程量
                cellIndex++;
                flexGrid1.Cell(cur, cellIndex).Text = detail.RealLaborNum.ToString();
                //单位
                cellIndex++;
                flexGrid1.Cell(cur, cellIndex).Text = detail.QuantityUnitName;

                int nextCol = cellIndex + 1;
                if (instead)
                {
                    //被代工队伍
                    flexGrid1.Cell(cur, nextCol).Text = detail.InsteadTeamName;
                    flexGrid1.Cell(cur, nextCol).WrapText = true;
                    nextCol = nextCol + 1;
                }
                //开始日期--》改成“派工日期”
                flexGrid1.Cell(cur, nextCol).Text = detail.StartDate.ToShortDateString();
                ////结束日期
                //flexGrid1.Cell(cur, nextCol + 1).Text = detail.EndDate.ToShortDateString();
                //用工说明
                flexGrid1.Cell(cur, nextCol + 1).Text = detail.LaborDescript;
                flexGrid1.Cell(cur, nextCol + 1).WrapText = true;
                flexGrid1.Row(cur).AutoFit();
            }

            //工长电子签名
            CommonUtil.SetFlexSign(flexGrid1, billMaster.HandlePerson.Id, 6 + detailCount + 1, 3);
            flexGrid1.InsertRow(6 + detailCount + 1, 1);
            //审核人信息
            int maxRow = detailCount - 1 + 10;
            string signStr = CommonUtil.SetFlexAuditPrint(flexGrid1, billMaster.Id, maxRow);
#if !DEBUG
            if (signStr != "")
            {
                throw new InvalidOperationException("该单据相关审核人" + signStr + "尚未设置电子签名图片，不能打印！请联系相关人员进入系统后，在系统状态栏双击并上传！");
            }
#endif

            if (instead)
            {
                int n = 11 + detailCount - 1;
                //flexGrid1.Range(n, 7, n, 8).Merge();
                flexGrid1.Cell(n, 7).FontBold = true;
                flexGrid1.Cell(n, 7).Alignment = FlexCell.AlignmentEnum.RightCenter;
                flexGrid1.Cell(n, 7).Text = "被代工队伍签字：";
            }

            FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            flexGrid1.PageSetup.BottomMargin = ClientUtil.Tofloat("0.5");
            pageSetup.RightFooter = " 打印时间:[" + CommonMethod.GetServerDateTime() + " ],打印人:[" + ConstObject.LoginPersonInfo.Name + "]   " + "第&P页/共&N页  ";

        }
    }
}
