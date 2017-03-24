using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using NHibernate.Criterion;

using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service;
using VirtualMachine.Component.WinControls.Controls;
using System.Data;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.Util;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public class MCubeManager
    {
        private static ICubeService cubeManagerSrv;
        private static IDimensionDefineService dimManagerSrv;
        private static IViewService viewService;
        private DimensionCategory time_cate = new DimensionCategory();
        private string old_time_str = "";
        private Hashtable ht_code = new Hashtable();//时间代码和维度之间的HT
        
        /// <summary>
        /// 存储表达式分隔符
        /// </summary>
        public static string[] SaveExpressDelimiter = {","};

        public static Hashtable factDefineHash;
        /// <summary>
        /// 通过主题查询事实
        /// </summary>
        /// <param name="cubeRegisterId"></param>
        /// <returns></returns>
        public IList GetFactDefineByCubeRegisterId(string cubeRegisterId)
        {
            if (factDefineHash == null) factDefineHash = new Hashtable();
            IList factList=new ArrayList();
            if (factDefineHash.Count > 0)
            {
                factList = factDefineHash[cubeRegisterId] as IList;
            }
            if (factList == null || factList.Count == 0)
            {
                factList = CubeManagerSrv.GetFactDefineByCubeRegisterId(cubeRegisterId);
                factDefineHash.Add(cubeRegisterId, factList);
            }
            return factList;
        }

        #region FlexCell表格对象的属性封装
        //结果区域属性
        private Hashtable hashCubeData = new Hashtable();//结果单元格的Id与CubeData的HashTable
        private int startRowResult = 0;//结果单元格的开始行索引
        private int endRowResult = 0;//结果单元格的开始列索引
        private int startColResult = 0;//结果单元格的终止行索引
        private int endColResult = 0;//结果单元格的终止列索引
        private IList cell_list = new ArrayList();//单元格的重新封装
        private IList measure_list = new ArrayList();//度量维度的集合（用于数据库查询条件）
        private Hashtable ht_measure = new Hashtable();//结果表格涉及的度量对应的DimensionCategory
        private Hashtable htDimension = new Hashtable();//所有维度对应的DimensionCategory

        public void Clear()
        {
            this.hashCubeData.Clear();
            this.startRowResult = 0;
            this.endRowResult = 0;
            this.startColResult = 0;
            this.endColResult = 0;
            this.cell_list.Clear();
            this.measure_list.Clear();
            this.ht_measure.Clear();
        }

        public Hashtable HashCubeData
        {
            get { return hashCubeData; }
            set { hashCubeData = value; }
        }

        public Hashtable HtDimension
        {
            get { return htDimension; }
            set { htDimension = value; }
        }

        public int StartRowResult
        {
            get { return startRowResult; }
            set { startRowResult = value; }
        }

        public int EndRowResult
        {
            get { return endRowResult; }
            set { endRowResult = value; }
        }

        public int StartColResult
        {
            get { return startColResult; }
            set { startColResult = value; }
        }

        public int EndColResult
        {
            get { return endColResult; }
            set { endColResult = value; }
        }

        #endregion

        #region Model的属性封装
        public ICubeService CubeManagerSrv
        {
            get { return cubeManagerSrv; }
            set { cubeManagerSrv = value; }
        }

        public IDimensionDefineService DimManagerSrv
        {
            get { return dimManagerSrv; }
            set { dimManagerSrv = value; }
        }

        public IViewService ViewService
        {
            get { return viewService; }
            set { viewService = value; }
        }

        public MCubeManager()
        {
            if (cubeManagerSrv == null)
            {
                cubeManagerSrv = StaticMethod.GetService(typeof(ICubeService)) as ICubeService;
            }
            if (dimManagerSrv == null)
            {
                dimManagerSrv = StaticMethod.GetService(typeof(IDimensionDefineService)) as IDimensionDefineService;
            }
            if (viewService == null)
            {
                viewService = StaticMethod.GetService(typeof(IViewService)) as IViewService;
            }
        }

        #endregion

        #region FlexCell表格对象的方法封装
        /// <summary>
        /// 填充FlexCell的二维表的行列标题
        /// 根据“是否显示子项母项”来固定显示,度量维度一定显示在行，并且显示在行的最后一行，
        /// 在列中的值显示为3列“实际值、子项、母项”。显示为最后一列。
        /// </summary>
        /// <param name="grid">控件对象</param>
        /// <param name="type">行列类型 1：列，2：行</param>
        /// <param name="startRowIndex">起始行索引</param>
        /// <param name="startColIndex">起始列索引</param>
        /// <param name="list">维度值结构</param>
        /// <returns></returns>
        public void FillGridCell(FlexCell.Grid grid, int type, int startRowIndex, int startColIndex, IList list)
        {
            int priDimCounts = list.Count;//主维度的个数，即几行单元格
            int eachCounts = 1;//每一级的总单元格数
            int oldRowIndex = startRowIndex;//存储老行索引
            int oldColIndex = startColIndex;//存储老列索引
            foreach (ViewStyle vs in list)
            {
                IList styleDims_list = vs.Details;//格式维度明细
                eachCounts = eachCounts * styleDims_list.Count;
            }

            //填充单元格
            for (int i = 0; i < priDimCounts; i++)
            {
                //复原行列的初始值
                if (type == 1)
                {
                    startColIndex = oldColIndex;
                }
                else
                {
                    startRowIndex = oldRowIndex;
                }
                ViewStyle vs = (ViewStyle)list[i];
                //取得下一个顺序的维度的个数
                int nextCounts = 1;//后续维度值总和
                int frontCounts = 1;//前续维度值总和
                //如果不是最后一级，就取随后顺序的维度值的总和3*4*5
                if (i < priDimCounts - 1)
                {
                    for (int n = i + 1; n < priDimCounts; n++)
                    {
                        ViewStyle vs_next = (ViewStyle)list[n];
                        nextCounts = nextCounts * vs_next.Details.Count;
                    }
                }

                if (i > 0)
                {
                    for (int n = 0; n < i; n++)
                    {
                        ViewStyle vs_front = (ViewStyle)list[n];
                        frontCounts = frontCounts * vs_front.Details.Count;
                    }
                }
                
                IList styleDims_list = vs.Details;//格式维度明细
                //三级循环的次数就等于eachCounts
                //第一次循环取上级维度数
                
                for (int t1 = 0; t1 < frontCounts; t1++)
                {
                    //第二次循环取本身维度数
                    foreach (ViewStyleDimension vsd in styleDims_list)
                    {
                        //第三次循环取下级维度数
                        for (int k = 0; k < nextCounts; k++)
                        {
                            grid.Cell(startRowIndex, startColIndex).Text = vsd.Name;
                            grid.Cell(startRowIndex, startColIndex).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                            grid.Cell(startRowIndex, startColIndex).Tag = vsd.DimCatId + "";
                            grid.Cell(startRowIndex, startColIndex).BackColor = System.Drawing.Color.LightGray;
                            if (type == 1)
                            {
                                startColIndex++;
                            }
                            else
                            {
                                startRowIndex++;
                            }
                        }
                    }
                }


                if (type == 1)
                {
                    startRowIndex++;
                }
                else
                {
                    startColIndex++;
                }
            }
        }

        /// <summary>
        /// 填充FlexCell的二维表的行列标题
        /// 根据“是否显示子项母项”来固定显示,度量维度一定显示在行，并且显示在行的最后一行，
        /// 在列中的值显示为3列“实际值、子项、母项”。显示为最后一列。
        /// </summary>
        /// <param name="grid">控件对象</param>
        /// <param name="type">行列类型 1：列，2：行</param>
        /// <param name="startRowIndex">起始行索引</param>
        /// <param name="startColIndex">起始列索引</param>
        /// <param name="list">维度值结构</param>
        /// <returns></returns>
        public void FillGridCellByJJZB(FlexCell.Grid grid, int type, int startRowIndex, int startColIndex, IList list)
        {
            int priDimCounts = list.Count;//主维度的个数，即几行单元格
            int eachCounts = 1;//每一级的总单元格数
            int oldRowIndex = startRowIndex;//存储老行索引
            int oldColIndex = startColIndex;//存储老列索引
            foreach (ViewStyle vs in list)
            {
                IList styleDims_list = vs.Details;//格式维度明细
                eachCounts = eachCounts * styleDims_list.Count;
            }

            //填充单元格
            for (int i = 0; i < priDimCounts; i++)
            {
                //复原行列的初始值
                if (type == 1)
                {
                    startColIndex = oldColIndex;
                }
                else
                {
                    startRowIndex = oldRowIndex;
                }
                ViewStyle vs = (ViewStyle)list[i];
                //取得下一个顺序的维度的个数
                int nextCounts = 1;//后续维度值总和
                int frontCounts = 1;//前续维度值总和
                //如果不是最后一级，就取随后顺序的维度值的总和3*4*5
                if (i < priDimCounts - 1)
                {
                    for (int n = i + 1; n < priDimCounts; n++)
                    {
                        ViewStyle vs_next = (ViewStyle)list[n];
                        nextCounts = nextCounts * vs_next.Details.Count;
                    }
                }

                if (i > 0)
                {
                    for (int n = 0; n < i; n++)
                    {
                        ViewStyle vs_front = (ViewStyle)list[n];
                        frontCounts = frontCounts * vs_front.Details.Count;
                    }
                }

                IList styleDims_list = vs.Details;//格式维度明细
                //三级循环的次数就等于eachCounts
                //第一次循环取上级维度数

                for (int t1 = 0; t1 < frontCounts; t1++)
                {
                    //第二次循环取本身维度数
                    foreach (ViewStyleDimension vsd in styleDims_list)
                    {
                        //第三次循环取下级维度数
                        for (int k = 0; k < nextCounts; k++)
                        {
                            if (vs.OldCatRootName.IndexOf("指标") != -1)
                            {
                                grid.Cell(startRowIndex, startColIndex).Text = vsd.Name;
                                grid.Cell(startRowIndex, startColIndex).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                                grid.Cell(startRowIndex, startColIndex).Tag = vsd.DimCatId + "";
                                grid.Cell(startRowIndex, startColIndex).BackColor = System.Drawing.Color.LightGray;
                                grid.Cell(startRowIndex, startColIndex+1).Text = vsd.DimUnit;
                                grid.Cell(startRowIndex, startColIndex+1).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                                grid.Cell(startRowIndex, startColIndex+1).Tag = vsd.DimCatId + "";
                                grid.Cell(startRowIndex, startColIndex+1).BackColor = System.Drawing.Color.LightGray;
                            }
                            else {
                                grid.Cell(startRowIndex, startColIndex).Text = vsd.Name;
                                grid.Cell(startRowIndex, startColIndex).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                                grid.Cell(startRowIndex, startColIndex).Tag = vsd.DimCatId + "";
                                grid.Cell(startRowIndex, startColIndex).BackColor = System.Drawing.Color.LightGray;
                            }
                            if (type == 1)
                            {
                                startColIndex++;
                            }
                            else
                            {
                                startRowIndex++;
                            }
                        }
                    }
                }


                if (type == 1)
                {
                    startRowIndex++;
                }
                else
                {
                    startColIndex++;
                }
            }
        }

        //合并一定区域里的单元格
        public void MergeCell(FlexCell.Grid grid, int type, int startRow, int startCol, int endRow, int endCol)
        {

            if (type == 2)
            {
                //合并相同的行
                for (int l = startCol; l < endCol; l++)
                {
                    FlexCell.Range range = new FlexCell.Range();

                    for (int h = startRow; h < endRow; h++)
                    {
                        if (range.FirstRow == 0)
                        {
                            range = grid.Range(h, l, h, l);
                        }

                        string currCellValue = grid.Cell(h, l).Text;
                        string nextCellValue = "";
                        if (h + 1 <= endRow)
                        {
                            nextCellValue = grid.Cell(h + 1, l).Text;
                        }
                        if (currCellValue == nextCellValue)
                        {
                            range = grid.Range(range.FirstRow, l, h, l);
                        }
                        else
                        {
                            range = grid.Range(range.FirstRow, l, h, l);
                            range.Merge();
                            range = grid.Range(0, l, 0, l);
                        }
                    }
                }
            }
            else
            {
                //合并相同的列
                for (int h = startRow; h < endRow; h++)
                {
                    FlexCell.Range range = new FlexCell.Range();

                    for (int l = startCol; l < endCol; l++)
                    {
                        if (range.FirstCol == 0)
                        {
                            range = grid.Range(h, l, h, l);
                        }

                        string currCellValue = grid.Cell(h, l).Text;
                        string nextCellValue = "";
                        if (l + 1 <= endCol)
                        {
                            nextCellValue = grid.Cell(h, l + 1).Text;
                        }
                        if (currCellValue == nextCellValue)
                        {
                            range = grid.Range(h, range.FirstCol, h, l);
                        }
                        else
                        {
                            range = grid.Range(h, range.FirstCol, h, l);
                            range.Merge();
                            range = grid.Range(h, 0, h, 0);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 显示FlexCell表格内容,通过判断是否为“频率模板”,来选择性的显示时间
        /// </summary>
        /// <param name="grid">控件对象</param>
        /// <param name="vm">视图主表对象</param>
        /// <param name="ifScore">是否显示评估分</param>
        /// <param name="ifWrite">是否为录入界面</param>
        /// <param name="time_str">时间条件</param>
        /// <returns></returns>
        public int DisplayCustomFlexCell(FlexCell.Grid grid, ViewMain vm, bool ifScore,bool ifWrite,string time_str)
        {
            //取得当前年和当前月
            int year = ConstObject.LoginDate.Year;
            int month = ConstObject.LoginDate.Month;

            string currMonth = "";
            if (month < 10)
            {
                currMonth = year + "年0" + month + "月";
            }
            else {
                currMonth = year + "年" + month + "月";
            }

            //取得立方属性
            CubeRegister cr = this.CubeManagerSrv.GetCubeRegisterById(vm.CubeRegId.Id);
            IList ca_list = this.CubeManagerSrv.GetCubeAttrByCubeResgisterId(cr);//立方属性集合
            IList styleList = this.ViewService.GetViewStyleByViewMain(vm, true);

            IList style_l_list = new ArrayList();//列格式集合
            IList style_h_list = new ArrayList();//行格式集合

            grid.AutoRedraw = false;//修改自动重画属性
            int rowDims = 0;//行的维数
            int colDims = 0;//列的维数
            int rows = 1;//行维的总行数
            int cols = 1;//列维的总列数

            //取得行列维数
            if (styleList != null && styleList.Count > 0)
            {
                foreach (ViewStyle vs in styleList)
                {
                    IList styleMxList = this.ViewService.GetViewStyleDimByVS(vs.Id);
                    //通过此段代码来区分模板频率，目前实现为“每月”
                    if (ifWrite == true)
                    {
                        if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1 && "2".Equals(vm.CollectTypeCode))
                        {
                            styleMxList = this.ViewService.GetViewStyleDimByVS(vs.Id, currMonth);
                            if (styleMxList.Count == 0)
                            {
                                return -1;
                            }
                        }
                    }
                    else if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1 && time_str != null && !"".Equals(time_str) && time_str.IndexOf("请选择") == -1)
                    {
                        styleMxList = this.ViewService.GetViewStyleDimByVS(vs.Id, time_str);
                    }


                    if (vs.Direction.Equals("行"))
                    {
                        style_h_list.Add(vs);
                        rows = rows * styleMxList.Count;
                        rowDims++;
                    }
                    else
                    {
                        style_l_list.Add(vs);
                        cols = cols * styleMxList.Count;
                        colDims++;
                    }
                    vs.Details = styleMxList;
                }
            }

            //写入行列的数据
            //1：初始化表格的单元格
            int totalRows = colDims + rows + 1;//表格的总行数
            int totalCols = rowDims + cols + 1;//表格的总列数，1：为记录数显示
            grid.Rows = totalRows;
            grid.Cols = totalCols;

            //写入行标题
            int k = 1;
            foreach (ViewStyle vs in styleList)
            {
                if (vs.Direction.Equals("行"))
                {
                    grid.Cell(colDims, k).Text = vs.OldCatRootName;
                    grid.Cell(colDims, k).BackColor = System.Drawing.Color.LightGray;
                    grid.Cell(colDims, k).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    k++;
                }
            }

            //2：写入固定区（空白部分）
            if (rowDims > 0 && colDims > 0)
            {
                //2：写入固定区（空白部分）
                FlexCell.Range range = new FlexCell.Range();
                range = grid.Range(1, 1, colDims - 1, rowDims);//表格左上角的空白区
                range.Merge();
                range.BackColor = System.Drawing.Color.LightGray;
            }

            //当没有列时，写入列标题为：结果值
            if (colDims == 0)
            {
                grid.Cell(0, rowDims+1).Text = "结果值";
            }


            //写入行列维度值   
            startColResult = rowDims + 1;//起始列标题位置
            startRowResult = colDims + 1;//起始行标题位置

            this.FillGridCell(grid, 1, 1, startColResult, style_l_list);//填充列标题
            this.FillGridCell(grid, 2, startRowResult, 1, style_h_list);//填充行标题

            //固定行列
            grid.FixedRowColStyle = FlexCell.FixedRowColStyleEnum.Light3D;
            grid.FixedCols = rowDims + 1;
            grid.FixedRows = colDims + 1;

            //写入结果值
            IList elements = this.CubeManagerSrv.GetQueryElements(ca_list, styleList);//得到二维维度值集合
            IList result = this.CubeManagerSrv.GetCubeDataListByViewStyle(cr, elements, styleList);//得到结果
            Hashtable ht_result = this.transResult(result);
            //确定结果值的起点和终点
            endRowResult = totalRows - 1;
            endColResult = totalCols - 1;

            //设置列的可录入的小数点位数
            for (int s = startColResult; s <= endColResult; s++)
            {
                grid.Column(s).DecimalLength = 2;
            }

            int measure_index = -1;//度量维度的索引

            //按照先行后列的顺序写入二维表的结果值
            for (int row = startRowResult; row <= endRowResult; row++)
            {
                for (int col = startColResult; col <= endColResult; col++)
                {
                    //取得当前单元格的多维维度的ID，先列后行，同一行列按排列顺序取
                    string link_dim_id = "_";
                    //先列
                    for (int l = 1; l <= colDims; l++)
                    {
                        link_dim_id += grid.Cell(l, col).Tag + "_";
                    }
                    //后行
                    for (int h = 1; h <= rowDims; h++)
                    {
                        link_dim_id += grid.Cell(row, h).Tag + "_";
                    }
                    //从结果集合中取结果值
                    CubeData cd = (CubeData)ht_result[link_dim_id];
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

                        char[] patten = { '_' };//分隔符
                        string[] temp = link_dim_id.Split(patten);
                        for (int t = 1; t < temp.Length-1; t++)
                        {
                            dataList.Add((string)temp[t]);
                        }                      
                        dataList = MCubeManager.cubeManagerSrv.transDisplayToOrder(styleList, ca_list, dataList);
                        cd.DimDataList = dataList;
                        cd.DimCodeList = codeList;
                        cr.CubeAttribute = ca_list;
                        MCubeManager.cubeManagerSrv.SetCubeData(cr, cd);
                    }
                    

                    //取出其中的度量维度的索引
                    if (measure_index == -1)
                    {
                        measure_index = this.GetMensionIndex(cd);
                    }

                    if (hashCubeData[cd.Id+""] == null)
                    {
                        hashCubeData.Add(cd.Id + "", cd);
                    }
                    if (ifScore == true)
                    {
                        grid.Cell(row, col).Text = cd.Plan.ToString();
                    }
                    else {
                        double resultx = cd.Result;

                        //判断是否汇总计算，并判断汇总规则：累计相加、累计平均
                        if (resultx > 0)
                        {
                            grid.Cell(row, col).Text = resultx + "";
                        }
                        else {
                            string catId = (string)cd.DimDataList[measure_index];
                            DimensionCategory dc = (DimensionCategory)htDimension[catId];
                            
                            //判断是否为中间节点
                            bool ifMiddle = this.IfMiddleNode(cd);
                            if (ifMiddle == true)
                            {
                                dc.CategoryNodeType.GetTypeCode();
                                if (dc.CalTypeCode != null && !"".Equals(dc.CalTypeCode))
                                {
                                    grid.Cell(row, col).Text = this.CalCubeData(cr, cd, dc.CalTypeCode) + "";
                                    grid.Cell(row, col).ForeColor = System.Drawing.Color.Blue;
                                }
                                else {
                                    grid.Cell(row, col).Text = "0";
                                }
                                
                            }
                            else {
                                grid.Cell(row, col).Text = "0";
                            }
                        }
                    }
                    
                    grid.Cell(row, col).Tag = cd.Id + "";
                    grid.Cell(row, col).Mask = FlexCell.MaskEnum.Numeric;
                    grid.Cell(row, col).Alignment = FlexCell.AlignmentEnum.RightCenter;
                }
            }

            //合并相同的行列
            this.MergeCell(grid, 1, 1, startColResult, colDims, startColResult + cols);//合并列标题
            this.MergeCell(grid, 2, startRowResult, 1, startRowResult + rows, rowDims);//合并行标题

            grid.Refresh();
            grid.AutoRedraw = true;//重画属性复原

            return 0;
        }

        /// <summary>
        /// 显示FlexCell表格内容,通过判断是否为“频率模板”,来选择性的显示时间
        /// </summary>
        /// <param name="grid">控件对象</param>
        /// <param name="vm">视图主表对象</param>
        /// <param name="ifScore">是否显示评估分</param>
        /// <param name="ifWrite">是否为录入界面</param>
        /// <param name="time_str">时间条件</param>
        /// <returns></returns>
        public int DisplayCustomFlexCellByJJZB(FlexCell.Grid grid, ViewMain vm, bool ifScore, bool ifWrite,string time_str)
        {
            //取得当前年和当前月
            int year = ConstObject.LoginDate.Year;
            int month = ConstObject.LoginDate.Month;
            bool ifSonMother = false;//是否显示子母项

            if ("是".Equals(vm.IfDisplaySonMother))
            {
                ifSonMother = true;
            }


            string currMonth = "";
            if (month < 10)
            {
                currMonth = year + "年0" + month + "月";
            }
            else
            {
                currMonth = year + "年" + month + "月";
            }

            //取得立方属性
            CubeRegister cr = this.CubeManagerSrv.GetCubeRegisterById(vm.CubeRegId.Id);
            IList ca_list = this.CubeManagerSrv.GetCubeAttrByCubeResgisterId(cr);//立方属性集合
            IList styleList = this.ViewService.GetViewStyleByViewMain(vm, true);

            IList style_l_list = new ArrayList();//列格式集合
            IList style_h_list = new ArrayList();//行格式集合

            grid.AutoRedraw = false;//修改自动重画属性
            grid.MultiSelect = true;
            int rowDims = 0;//行的维数
            int colDims = 0;//列的维数
            int rows = 1;//行维的总行数
            int cols = 1;//列维的总列数

            //取得行列维数
            if (styleList != null && styleList.Count > 0)
            {
                foreach (ViewStyle vs in styleList)
                {
                    IList styleMxList = this.ViewService.GetViewStyleDimByVS(vs.Id);
                    //通过此段代码来区分模板频率，目前实现为“每月”
                    if (ifWrite == true)
                    {
                        if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1 && "2".Equals(vm.CollectTypeCode))
                        {
                            styleMxList = this.ViewService.GetViewStyleDimByVS(vs.Id, currMonth);
                            if (styleMxList.Count == 0)
                            {
                                return -1;
                            }
                        }
                    }
                    else if (vs.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1 && time_str != null && !"".Equals(time_str) && time_str.IndexOf("请选择") == -1)
                    {
                        styleMxList = this.ViewService.GetViewStyleDimByVS(vs.Id, time_str);
                    }

                    if (vs.Direction.Equals("行"))
                    {
                        style_h_list.Add(vs);
                        rows = rows * styleMxList.Count;
                        rowDims++;
                    }
                    else
                    {
                        style_l_list.Add(vs);
                        cols = cols * styleMxList.Count;
                        colDims++;
                    }
                    vs.Details = styleMxList;
                }
            }

            if (ifSonMother == true)
            {
                this.AddViewStyleBySonMother(vm, style_l_list);
                cols = cols * 3;
                rows = rows + 1;
            }

            //写入行列的数据
            //1：初始化表格的单元格
            int totalRows = colDims + rows + 1;//表格的总行数
            int totalCols = rowDims + cols + 2;//表格的总列数，1：为记录数显示，2：为计量单位

            grid.Rows = totalRows;
            grid.Cols = totalCols;

            //写入行标题
            int k = 1;
            foreach (ViewStyle vs in styleList)
            {
                if (vs.Direction.Equals("行"))
                {
                    if (ifSonMother == true)
                    {
                        grid.Cell(colDims + 1, k).Text = vs.OldCatRootName;
                        grid.Cell(colDims + 1, k).BackColor = System.Drawing.Color.LightGray;
                        grid.Cell(colDims + 1, k).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    }
                    else
                    {
                        grid.Cell(colDims, k).Text = vs.OldCatRootName;
                        grid.Cell(colDims, k).BackColor = System.Drawing.Color.LightGray;
                        grid.Cell(colDims, k).Alignment = FlexCell.AlignmentEnum.CenterCenter;
                    }
                    k++;
                }
            }

            if (ifSonMother == true)
            {
                grid.Cell(colDims + 1, k).Text = "计量单位";
                grid.Cell(colDims + 1, k).BackColor = System.Drawing.Color.LightGray;
                grid.Cell(colDims + 1, k).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            }
            else
            {
                grid.Cell(colDims, k).Text = "计量单位";
                grid.Cell(colDims, k).BackColor = System.Drawing.Color.LightGray;
                grid.Cell(colDims, k).Alignment = FlexCell.AlignmentEnum.CenterCenter;
            }

            //2：写入固定区（空白部分）
            if (rowDims > 0 && colDims > 0)
            {
                //2：写入固定区（空白部分）
                FlexCell.Range range = new FlexCell.Range();
                if (ifSonMother == true)
                {
                    range = grid.Range(1, 1, colDims, rowDims + 1);//表格左上角的空白区
                }
                else {
                    range = grid.Range(1, 1, colDims-1, rowDims + 1);//表格左上角的空白区
                }
                range.Merge();
                range.BackColor = System.Drawing.Color.LightGray;
            }

            //当没有列时，写入列标题为：结果值
            if (colDims == 0)
            {
                grid.Cell(0, rowDims + 1).Text = "结果值";
            }


            //写入行列维度值   
            startColResult = rowDims + 2;//起始列标题位置
            startRowResult = colDims + 1;//起始行标题位置
            if (ifSonMother == true)
            {
                startRowResult = startRowResult + 1;
            }
            this.FillGridCell(grid, 1, 1, startColResult, style_l_list);//填充列标题
            this.FillGridCellByJJZB(grid, 2, startRowResult, 1, style_h_list);

            //固定行列
            grid.FixedRowColStyle = FlexCell.FixedRowColStyleEnum.Light3D;
            grid.FixedCols = rowDims + 2;

            if (ifSonMother == true)
            {
                grid.FixedRows = colDims + 1 + 1;
            }
            else
            {
                grid.FixedRows = colDims + 1;
            }

            //写入结果值
            IList elements = this.CubeManagerSrv.GetQueryElements(ca_list, styleList);//得到二维维度值集合
            IList result = this.CubeManagerSrv.GetCubeDataListByViewStyle(cr, elements, styleList);//得到结果
            Hashtable ht_result = this.transResult(result);
            //确定结果值的起点和终点
            endRowResult = totalRows - 1;
            endColResult = totalCols - 1;

            //设置列的可录入的小数点位数
            for (int s = startColResult; s <= endColResult; s++)
            {
                grid.Column(s).DecimalLength = 2;
            }
            grid.Column(3).Width = 140;

            int measure_index = -1;//度量维度的索引

            //按照先行后列的顺序写入二维表的结果值
            for (int row = startRowResult; row <= endRowResult; row++)
            {
                for (int col = startColResult; col <= endColResult; col = col + 3)
                {
                    //取得当前单元格的多维维度的ID，先列后行，同一行列按排列顺序取
                    string link_dim_id = "_";
                    //先列
                    for (int l = 1; l <= colDims; l++)
                    {
                        link_dim_id += grid.Cell(l, col).Tag + "_";
                    }
                    //后行
                    for (int h = 1; h <= rowDims; h++)
                    {
                        link_dim_id += grid.Cell(row, h).Tag + "_";
                    }
                    //从结果集合中取结果值
                    CubeData cd = (CubeData)ht_result[link_dim_id];
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

                        char[] patten = { '_' };//分隔符
                        string[] temp = link_dim_id.Split(patten);
                        for (int t = 1; t < temp.Length - 1; t++)
                        {
                            dataList.Add((string)temp[t]);
                        }
                        dataList = MCubeManager.cubeManagerSrv.transDisplayToOrder(styleList, ca_list, dataList);
                        cd.DimDataList = dataList;
                        cd.DimCodeList = codeList;
                        cr.CubeAttribute = ca_list;
                        MCubeManager.cubeManagerSrv.SetCubeData(cr, cd);
                    }


                    //取出其中的度量维度的索引
                    if (measure_index == -1)
                    {
                        measure_index = this.GetMensionIndex(cd);
                    }

                    if (hashCubeData[cd.Id + ""] == null)
                    {
                        hashCubeData.Add(cd.Id + "", cd);
                    }
                    if (ifScore == true)
                    {
                        grid.Cell(row, col).Text = cd.Plan.ToString();
                    }
                    else
                    {
                        double resultx = cd.Result;
                        grid.Cell(row, col).Text = resultx + "";
                        grid.Cell(row, col + 1).Text = cd.SonValue + "";
                        grid.Cell(row, col + 2).Text = cd.MotherValue + "";
                        //判断是否汇总计算，并判断汇总规则：累计相加、累计平均
                        /*if (resultx > 0)
                        {
                            grid.Cell(row, col).Text = resultx + "";
                        }
                        else {
                            string catId = (string)cd.DimDataList[measure_index];
                            DimensionCategory dc = (DimensionCategory)htDimension[catId];
                            
                            //判断是否为中间节点
                            bool ifMiddle = this.IfMiddleNode(cd);
                            if (ifMiddle == true)
                            {
                                dc.CategoryNodeType.GetTypeCode();
                                if (dc.CalTypeCode != null && !"".Equals(dc.CalTypeCode))
                                {
                                    grid.Cell(row, col).Text = this.CalCubeData(cr, cd, dc.CalTypeCode) + "";
                                    grid.Cell(row, col).ForeColor = System.Drawing.Color.Blue;
                                }
                                else {
                                    grid.Cell(row, col).Text = "0";
                                }
                                
                            }
                            else {
                                grid.Cell(row, col).Text = "0";
                            }
                        }*/
                    }

                    grid.Cell(row, col).Tag = cd.Id + "";
                    grid.Cell(row, col).Mask = FlexCell.MaskEnum.Numeric;
                    grid.Cell(row, col).Alignment = FlexCell.AlignmentEnum.RightCenter;
                    grid.Cell(row, col + 1).Tag = "son";
                    grid.Cell(row, col + 1).Mask = FlexCell.MaskEnum.Numeric;
                    grid.Cell(row, col + 1).Alignment = FlexCell.AlignmentEnum.RightCenter;
                    grid.Cell(row, col + 2).Tag = "mother";
                    grid.Cell(row, col + 2).Mask = FlexCell.MaskEnum.Numeric;
                    grid.Cell(row, col + 2).Alignment = FlexCell.AlignmentEnum.RightCenter;
                }
            }


            //合并相同的行列
            this.MergeCell(grid, 1, 1, startColResult, colDims + 1, startColResult + cols);//合并列标题
            if (ifSonMother == true)
            {
                this.MergeCell(grid, 2, startRowResult - 1, 1, startRowResult + rows - 1, rowDims + 1);//合并行标题
            }
            else {
                this.MergeCell(grid, 2, startRowResult, 1, startRowResult + rows, rowDims);//合并行标题
            }

            grid.Row(0).Visible =false;
            grid.Column(0).Visible = false;

            grid.Refresh();
            grid.AutoRedraw = true;//重画属性复原

            return 0;
        }

        //判断结果中的单元格是否为中间节点
        private bool IfMiddleNode(CubeData cd) {
            bool ifMiddle = false;
            IList dataList = cd.DimDataList;
            foreach (string dimId in dataList)
            {
                DimensionCategory dc = (DimensionCategory)htDimension[dimId];
                string nodeType = dc.CategoryNodeType.ToString();
                if (!"LeafNode".Equals(nodeType))
                {
                    ifMiddle = true;
                    break;
                }
            }
            return ifMiddle;
        }

        //把结果集的维度联接和其CubeData对象对应起来，以便后来取数
        private Hashtable transResult(IList result)
        {
            Hashtable ht_result = new Hashtable();
            foreach (CubeData cd in result)
            {
                IList list = cd.DimDataList;
                string link = MCubeManager.SaveExpressDelimiter[0];
                foreach (string dimId in list)
                {
                    link += dimId + SaveExpressDelimiter[0];
                }
                if (!ht_result.Contains(link))
                {
                    ht_result.Add(link, cd);
                }
            }
            return ht_result;
        }

        /// <summary>
        /// 计算FlexCell表格中的计算值
        /// </summary>
        /// <param name="grid">控件对象</param>
        /// <param name="startRowResult">结果区的开始行</param>
        /// <param name="endRowResult">结果区的结束行</param>
        /// <param name="startColResult">结果区的开始列</param>
        /// <param name="endColResult">结果区的结束列</param>
        /// <param name="calCellList">计算单元格集合</param>
        /// <returns></returns>
        public void CalculateResult(FlexCell.Grid grid, int startRowResult, int endRowResult, int startColResult, int endColResult)
        {

            if (cell_list.Count == 0)
            {
                int measure_index = -1;//立方数据中度量的索引
                //取得需要的数据填充FlexCellData对象
                for (int row = startRowResult; row <= endRowResult; row++)
                {
                    for (int col = startColResult; col <= endColResult; col++)
                    {
                        FlexCellData fcd = new FlexCellData();
                        fcd.Row = row;//行索引
                        fcd.Col = col;//列索引

                        FlexCell.Cell cell = grid.Cell(row, col);
                        string id = cell.Tag;
                        CubeData cd = (CubeData)hashCubeData[id + ""];
                        //取出其中的度量维度的索引
                        if (measure_index == -1)
                        {
                            measure_index = this.GetMensionIndex(cd);
                        }
                        string link_otherdimid = "";
                        for (int i = 0; i < cd.DimDataList.Count; i++)
                        {
                            if (i == measure_index)
                            {
                                fcd.MeasureId = (string)cd.DimDataList[i];//度量值ID
                                if (measure_list.Contains((string)cd.DimDataList[i]) == false)
                                {
                                    measure_list.Add((string)cd.DimDataList[i]);
                                }
                            }
                            else
                            {
                                link_otherdimid += (string)cd.DimDataList[i] + "_";
                            }
                        }
                        fcd.LinkOtherDimId = link_otherdimid;//其他维度联接
                        if (cell.Text == null || "".Equals(cell.Text))
                        {
                            fcd.Result = 0;
                        }
                        else if (cell.Text.EndsWith("."))
                        {
                            fcd.Result = Double.Parse(cell.Text + "0");
                        }
                        else if ("-".Equals(cell.Text))
                        {
                            fcd.Result = 0;
                        }
                        else
                        {
                            fcd.Result = Double.Parse(cell.Text);//当前单元格的值
                        }
                        cell_list.Add(fcd);
                    }
                }

                //查询本结果表格涉及的度量的表达式
                ht_measure = this.QueryDimensionByCondition(measure_list);
            }
            else 
            {
                foreach (FlexCellData fcd in cell_list)
                {
                    FlexCell.Cell cell = grid.Cell(fcd.Row, fcd.Col);
                    if (cell.Text == null || "".Equals(cell.Text))
                    {
                        fcd.Result = 0;
                    }
                    else if (cell.Text.EndsWith("."))
                    {
                        return;
                    }
                    else if ("-".Equals(cell.Text))
                    {
                        return;
                    }
                    else
                    {
                        fcd.Result = Double.Parse(cell.Text);//当前单元格的值
                    }
                }
            }
            

            //重新计算结果表格中的计算值
            foreach (FlexCellData fcd in cell_list)
            {
                string measure_id = fcd.MeasureId;
                DimensionCategory dc = (DimensionCategory)ht_measure[measure_id];
                string express = dc.CalExpression;
                if (express != null && express.Length > 1)
                {
                    express = express.Substring(express.IndexOf("@")+1);//取得分隔符@后的表达式
                    string linkOtherDimId = fcd.LinkOtherDimId;
                    //获取计算表达式中度量的结果值
                    double result = this.GetCalResult(cell_list, express, linkOtherDimId);
                    FlexCell.Cell cell = grid.Cell(fcd.Row,fcd.Col);
                    cell.Text = result + "";
                }
            }
            
        }

        //通过计算表达式，计算本单元格的计算值
        private double GetCalResult(IList cell_list,string express,string linkOtherDimId)
        {
            double result = 0;
            //得到计算表达式的每一个度量ID
            char[] patten = { '{', '}' };//分隔符
            string[] temp = express.Split(patten);
            IList dimList = new ArrayList();
            for (int i = 1; i < temp.Length; i = i + 2)
            {
                dimList.Add(temp[i]);
            }

            //取得每一个度量ID对应的值
            foreach (string str in dimList)
            {
                foreach (FlexCellData fcd in cell_list)
                {
                    if (str.Equals(fcd.MeasureId) && (linkOtherDimId.Equals(fcd.LinkOtherDimId)))
                    {
                        double value = fcd.Result;
                        if (value == 0)
                        {
                            return 0;
                        }
                        else 
                        {
                            express = express.Replace("{" + str + "}", value+"");
                        }
                    }
                }
            }
            //如果表达式中的指标在此次录入中不存在，则返回0
            if (express.IndexOf("{") >= 0)
            {
                return 0;
            }

            result = Double.Parse(DateUtil.CalculateExpression(express, 2));
            return result;           
        }

        private int GetMensionIndex(CubeData cd)
        {
            int index = -1;
            IList list = cd.DimCodeList;
            foreach (string code in list)
            {
                DimensionRegister dr = dimManagerSrv.GetDimensionRegisterByCode(code);
                if (dr.IfMeasure == 1)
                {
                    index = list.IndexOf(code);
                    break;
                }
            }
            return index;
        }


        /// <summary>
        /// 计算FlexCell表格中的子母项
        /// </summary>
        /// <param name="cell">单元格对象</param>
        /// <returns></returns>
        public void CalculateResultByJJZB(FlexCell.Grid grid,FlexCell.Cell cell)
        {
            string tag = cell.Tag as string;
            string sonValue = "1";
            string motherValue = "1";
            if (!"son".Equals(tag) && !"mother".Equals(tag))
            {
                return;
            }
            int row = cell.Row;
            int col = cell.Col;
            if ("son".Equals(tag))
            {
                sonValue = cell.Text;
                motherValue = grid.Cell(row, col + 1).Text;
                if (motherValue != null && !"".Equals(motherValue) && double.Parse(motherValue) != 0)
                {
                    grid.Cell(row, col - 1).Text = double.Parse(sonValue) / double.Parse(motherValue) + "";
                }
            }
            else if ("mother".Equals(tag))
            {
                motherValue = cell.Text;
                sonValue = grid.Cell(row, col - 1).Text;
                if (motherValue != null && !"".Equals(motherValue) && double.Parse(motherValue) != 0)
                {
                    grid.Cell(row, col - 2).Text = double.Parse(sonValue) / double.Parse(motherValue) + "";
                }
            }
        }


        private Hashtable QueryDimensionByCondition(IList measure_list)
        {
            Hashtable ht_measure = new Hashtable();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.In("Id", measure_list));
            IList list = dimManagerSrv.GetDimensionCategoryByQuery(oq);
            foreach (DimensionCategory dc in list)
            {
                ht_measure.Add(dc.Id+"", dc);
            }
            return ht_measure;
        }

        //模板中存在跨时间时用，time_code为年份的编码，如：2007
        public Hashtable QueryDimensionByCodeColl(string time_code)
        {
            ht_code.Clear();
            IList code_list = new ArrayList();
            for (int i = 0; i < KnowledgeUtil.TimeVarCode.Length; i++)
            {
                code_list.Add(time_code + KnowledgeUtil.TimeVarCode[i]);
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.In("Code", code_list));
            oq.AddCriterion(Expression.Eq("DimRegId", "2"));
            IList list = dimManagerSrv.GetDimensionCategoryByQuery(oq);
            foreach (DimensionCategory dc in list)
            {
                ht_code.Add(dc.Code, dc);
            }
            return ht_code;
        }

        #endregion

        #region 浏览数据方法的封装
        // <summary>
        /// 浏览数据时的汇总算法
        /// </summary>
        /// <param name="obj">立方体对象</param>
        /// <param name="type">汇总类型(1:累计相加;2:累计平均)</param>
        /// <returns>返回的立方体的数据集合</returns>
        public double CalCubeData(CubeRegister cr,CubeData obj, string type)
        {
            double result = 0;
            //通过type取平均还是累加
            string calStr = "";
            if ("1".Equals(type))
            {
                calStr = " round(sum(RESULTVALUE),4) ";
            }
            else if ("2".Equals(type))
            {
                calStr = " round(avg(RESULTVALUE),4) ";
            }

            string sql_link = "select "+calStr+" from "+cr.CubeCode + " where 1=1 ";
            IList dataList = obj.DimDataList;//维度值集合
            IList codeList = obj.DimCodeList;//维度代码集合
            int i = 0;
            foreach (string dimId in dataList)
            {
                DimensionCategory dc = (DimensionCategory)htDimension[dimId];
                string sysCode = dc.SysCode;
                IList childList = dimManagerSrv.GetAllChildNodesBySysCodeByDBDao(sysCode);
                if (childList.Count == 0)
                {
                    childList.Add(dc);
                }
                sql_link += " and " + codeList[i] + " in ( 0";
                foreach (DimensionCategory childDc in childList)
                {
                    sql_link += "," + childDc.Id;
                }
                sql_link += ")";
                i++;
            }
            sql_link += " and RESULTVALUE > 0 ";
            string str = MCubeManager.cubeManagerSrv.GetResultBySql(sql_link);
            result = Double.Parse(str);
            return result;
        }

        // <summary>
        /// 系统自动增加子母项
        /// </summary>
        /// <param name="vStyleList">已有视图集合</param>
        /// <param name="vStyleList">视图主对象</param>
        /// <returns>返回的立方体的数据集合</returns>
        public IList  AddViewStyleBySonMother(ViewMain vm,IList vStyleList)
        {
            ViewStyle vs_add = new ViewStyle();
            vs_add.Id = "1";
            vs_add.Main = vm;
            vs_add.OldCatRootName = "子母项";
            vs_add.Direction = "列";
            vs_add.RangeOrder = 100;
            IList style_mx_list = new ArrayList();
            ViewStyleDimension vs1 = new ViewStyleDimension();
            vs1.Name = "实际值";
            vs1.Id = "1";
            ViewStyleDimension vs2 = new ViewStyleDimension();
            vs2.Name = "子项";
            vs2.Id = "2";
            ViewStyleDimension vs3 = new ViewStyleDimension();
            vs3.Name = "母项";
            vs3.Id = "3";
            style_mx_list.Add(vs1);
            style_mx_list.Add(vs2);
            style_mx_list.Add(vs3);
            vs_add.Details = style_mx_list;
            vStyleList.Add(vs_add);

            return vStyleList;
        }

        //通过视图查询此视图下的时间维度值的集合
        public IList GetCurrTimeList(ViewMain vm)
        {
            IList time_list = new ArrayList();
            ViewStyleDimension temp = new ViewStyleDimension();
            temp.Id ="";
            temp.Name = "--请选择--";
            time_list.Add(temp);

            ViewStyle vs = new ViewStyle();
            IList style_list = this.ViewService.GetViewStyleByViewMain(vm, true);
            foreach (ViewStyle obj in style_list)
            {
                if (obj.OldCatRootName.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1)
                {
                    vs = obj;
                    break;
                }
            }
            IList stylemx_list = this.ViewService.GetViewStyleDimByVS(vs.Id);
            foreach (ViewStyleDimension vsd in stylemx_list)
            {
                time_list.Add(vsd);
            }
            return time_list;
        }

        //通过视图查询此视图下的业务组织维度值的集合
        public IList GetCurrYwzzList(ViewMain vm)
        {
            IList ywzz_list = new ArrayList();

            ViewStyle vs = new ViewStyle();
            IList style_list = this.ViewService.GetViewStyleByViewMain(vm, true);
            foreach (ViewStyle obj in style_list)
            {
                if (obj.OldCatRootName.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) != -1)
                {
                    vs = obj;
                    break;
                }
            }
            IList stylemx_list = this.ViewService.GetViewStyleDimByVS(vs.Id);
            if (stylemx_list.Count > 1)
            {
                ViewStyleDimension temp = new ViewStyleDimension();
                temp.Id ="";
                temp.Name = "--请选择--";
                ywzz_list.Add(temp);
            }
            foreach (ViewStyleDimension vsd in stylemx_list)
            {
                ywzz_list.Add(vsd);
            }
            return ywzz_list;
        }

        #endregion

        #region 通用模板定义、录入
        //规则表达式的变量类型
        public IList InitRuleVarType()
        {
            IList var_list = new ArrayList();
            var_list = new ArrayList();
            var_list.Add("文本");
            var_list.Add("{定义部门}");
            var_list.Add("{定义时间}");
            var_list.Add("{录入岗位}");
            var_list.Add("{录入人}");
            var_list.Add("{录入时间}");
            var_list.Add("{录入部门}");
            var_list.Add("{备注}");
            var_list.Add("{备用1}");
            var_list.Add("{备用2}");
            var_list.Add("{备用3}");
            var_list.Add("{备用4}");
            var_list.Add("{备用5}");
            return var_list;
        }

        //得到该模板中的所有数据cubedata,返回是_1_5_6(维度ID的联接)和cubedata的对应
        //时间做特殊处理，时间维度的值从界面上取,如果是时间跨度则加上所有的时间跨度维度值
        public Hashtable GetFreeFlexData(ViewMain vm, string time_id)
        {
            CubeRegister cr = this.CubeManagerSrv.GetCubeRegisterById(vm.CubeRegId.Id);
            IList ca_list = this.CubeManagerSrv.GetCubeAttrByCubeResgisterId(cr);//立方属性集合
            IList styleList = this.ViewService.GetViewStyleByViewMain(vm, false);
            foreach (ViewStyle vs in styleList)
            {
                if (vs.OldCatRootName.Equals(KnowledgeUtil.TIME_DIM_STR))
                {
                    if ("1".Equals(vm.IfTime) && ht_code.Count > 0)
                    {
                        ICollection dim_coll = ht_code.Values;
                        IList list = vs.Details;
                        foreach (DimensionCategory dim in dim_coll)
                        {
                            ViewStyleDimension vsd = new ViewStyleDimension();
                            vsd.DimCatId = dim.Id;
                            vsd.ViewStyleId = vs;                            
                            list.Add(vsd);
                        }
                    }
                    else
                    {
                        ViewStyleDimension vsd = new ViewStyleDimension();
                        vsd.DimCatId = time_id;
                        vsd.ViewStyleId = vs;
                        IList list = vs.Details;
                        list.Add(vsd);
                    }
                }
            }
            IList factList = GetFactDefineByCubeRegisterId(cr.Id);
            IList elements = this.CubeManagerSrv.GetQueryElements(ca_list, styleList);//得到二维维度值集合
            //添加事实的ID，事实存的与其它不同，是事实对象
            elements.Add(factList);

            IList result = this.CubeManagerSrv.GetCubeDataList(cr, elements);//得到结果
            Hashtable ht_result = this.transResult(result);
            return ht_result;
        }

        //通用的预览的设置
        public void GeneralPreview(FlexCell.Grid grid)
        {
            grid.PageSetup.Landscape = true;
            grid.PageSetup.PrintFixedColumn = true;
            grid.PageSetup.PrintFixedRow = true;
            grid.PageSetup.PrintGridLines = false;
            grid.PageSetup.CenterHorizontally = true;
            grid.PageSetup.TopMargin = 1;
            grid.PageSetup.LeftMargin = 2;

            grid.PrintPreview(true,true,true,0,0,0,0,0);
        }

        //通用的预览的设置
        public void GeneralPreviewWithNoFix(FlexCell.Grid grid)
        {
            grid.PageSetup.Landscape = true;
            grid.PageSetup.PrintFixedColumn = false;
            grid.PageSetup.PrintFixedRow = false;
            grid.PageSetup.PrintGridLines = false;
            grid.PageSetup.CenterHorizontally = true;
            grid.PageSetup.TopMargin = 1;
            grid.PageSetup.LeftMargin = 1;

            grid.PrintPreview(true,true,true,0,0,0,0,0);
        }

        //计算一个字符串(str)中，从第几个开始(startIndex),第几次(count)出现sign的位置
        public int GetIndexByString(string str, string sign, int startIndex, int count)
        {
            int index = 0;
            string temp = "";
            for (int i = 0; i < count; i++)
            {
                temp += str.Substring(0, str.IndexOf(MCubeManager.SaveExpressDelimiter[0], startIndex) + 1);
                str = str.Substring(str.IndexOf(MCubeManager.SaveExpressDelimiter[0]) + 1);
            }
            if (temp.Length == 0)
            {
                index = temp.Length;
            }
            else
            {
                index = temp.Length - 1;
            }

            return index;
        }

        //把计算表达式中的时间跨度值转换成维度ID值
        public string TransCalExpress(string express, DimensionCategory cate)
        {
            string time_str = cate.Id + "";
            string registerId = ConstObject.TheSystemCode;         
            string[] times = KnowledgeUtil.TimeSpan;
            for (int i = 0; i < times.Length; i++)
            {
                string name = "_" + times[i].ToString();
                if (express.IndexOf(name) != -1)
                {
                    if (i == 0)
                    {
                        express = express.Replace("时间", time_str);
                    }
                    else
                    {
                        string code = cate.Code;
                        //判断是否为一月份的上月，如果是则归上月的数0
                        if (code.EndsWith("01") && "上月".Equals(name) && code.Length == 7)
                        {
                            express = express.Replace(times[i].ToString(), "0");
                        }
                        else
                        {
                            string get_code = KnowledgeUtil.GetCalDateCode(code, i);
                            long cat_id = this.DimManagerSrv.GetDimensionIdByCode(get_code, registerId);
                            express = express.Replace(times[i].ToString(), cat_id + "");
                        }
                    }
                }

            }
            return express;
        }

        //把视图定义规则中的计算表达式涉及到的cubedata值查询出来，把变量转换为值,返回是_1_5_6(维度ID的联接)和结果值的对应
        public Hashtable GetAllValueInVm(CubeRegister cr, IList rule_list, string time_str, string ywzz_str)
        {
            Hashtable ht_value = new Hashtable();
            DimensionCategory cate = this.DimManagerSrv.GetDimensionCategoryById(time_str);
            //先转换规则表达式为维度值的连接集合{"_2_4_8","_3_5_8"}
            IList var_link_list = new ArrayList();
            foreach (ViewRuleDef vrd in rule_list)
            {
                string calexpress = vrd.CalExpress;
                if (calexpress == null) continue;
                if (calexpress.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) != -1)
                {
                    calexpress = calexpress.Replace(KnowledgeUtil.YWZZ_DIM_STR, ywzz_str);
                }
                if (calexpress != null && !"".Equals(calexpress) && calexpress.IndexOf(MCubeManager.SaveExpressDelimiter[0]) != -1)
                {
                    calexpress = this.TransCalExpress(calexpress, cate);
                    char[] pattern ={ '[', ']' };
                    string[] temp = calexpress.Split(pattern);
                    for (int i = 1; i < temp.Length; i = i + 2)
                    {
                        string get_express = temp[i].ToString();
                        if (var_link_list.Contains(get_express) == false)
                        {
                            var_link_list.Add(get_express);
                        }
                    }
                }
            }

            if (var_link_list.Count == 0)
            {
                return ht_value;
            }

            //转换维度值联接集合为类似于{[ "a1", "a2" ],[ "b1", "b2" ],  [ "c1" ]}的形式
            IList attrList = this.CubeManagerSrv.GetCubeAttrByCubeResgisterId(cr);
            IList elements_list = new ArrayList();
            for (int k = 1; k <= attrList.Count; k++)
            {
                IList temp_list = new ArrayList();
                foreach (string str in var_link_list)
                {
                    //char[] pattern = { '_' };
                    string[] temp_str = str.Split(MCubeManager.SaveExpressDelimiter,StringSplitOptions.None);
                    for (int m = 1; m < temp_str.Length; m++)
                    {
                        if (m == k)
                        {
                            string curr_str = temp_str[m].ToString();
                            if (temp_list.Contains(curr_str) == false)
                            {
                                temp_list.Add(curr_str);
                            }
                        }
                    }
                }
                elements_list.Add(temp_list);
            }

            IList result_list = this.CubeManagerSrv.GetCubeDataList(cr, elements_list);
            foreach (CubeData cd in result_list)
            {
                IList dim_id_list = cd.DimDataList;
                string id_link = "";
                foreach (string id_str in dim_id_list)
                {
                    id_link += SaveExpressDelimiter[0] + id_str;
                }
                ht_value.Add(id_link, cd.Result);
            }
            return ht_value;

        }

        public void CalGridValue(IList rule_list, FlexCell.Grid grid, Hashtable ht_exist_value, string time_str,string ywzz_str)
        {
            DataTable dt = new DataTable();

            IList curr_cal_list = new ArrayList();//计算规则集合的拷贝
            IList sign_list = new ArrayList();//当前单元格的代号集合
            IList function_list = new ArrayList();//存在自定义公式的规则表达式集合
            if (time_cate.Id == "" || !old_time_str.Equals(time_str))
            {
                time_cate = this.DimManagerSrv.GetDimensionCategoryById(time_str);
                old_time_str = time_str;
            }

            foreach (ViewRuleDef vrd in rule_list)
            {
                curr_cal_list.Add(vrd);

                string currExpress = (vrd.CalExpress == null) ? "" : vrd.CalExpress;
                bool ifCal = KnowledgeUtil.IfHaveUpperLetter(currExpress);//判断是否存在表格内部计算表达式   
                bool ifDefine = KnowledgeUtil.IfDefineFunction(currExpress);//判断是否存在自定义公式   
                if (currExpress != null && ifCal == true && ifDefine == false)
                {
                    sign_list.Add(vrd.CellSign);
                }

                if (currExpress != null && ifDefine == true)
                {
                    function_list.Add(vrd);
                }
            }

            Hashtable ht_trans_express = new Hashtable();
            /*
             * 1：目前已经存在的单元格代号集合sign_list
             * 2：循环10次，当sign_list为空即停止循环
             * 3：每次循环已经计算的单元格从sign_list中去除，当前计算的原则为不包含在sign_list中的单元格
             */
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
                    string currExpress = (vrd.CalExpress == null) ? "" : vrd.CalExpress;
                    string cellSign = vrd.CellSign;
                    bool ifCal = KnowledgeUtil.IfHaveUpperLetter(currExpress);//判断是否存在表格内部计算表达式
                    if (currExpress != null && ifCal == true)
                    {
                        if (currExpress.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) != -1)
                        {
                            currExpress = currExpress.Replace(KnowledgeUtil.YWZZ_DIM_STR, ywzz_str);
                        }

                        if (ht_trans_express[vrd.Id + ""] == null)
                        {
                            currExpress = this.TransCalExpress(currExpress, time_cate);
                            ht_trans_express.Add(vrd.Id + "", currExpress);
                        }
                        else {
                            currExpress = ht_trans_express[vrd.Id + ""].ToString();
                        }

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
                                value = grid.Cell(row, col).Text;
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
                        grid.Cell(curr_row, curr_col).Text = cal_value;
                        grid.Cell(curr_row, curr_col).Alignment = FlexCell.AlignmentEnum.RightCenter;

                        if (haveCal == true && ifHave == false)
                        {
                            curr_cal_list.Remove(vrd);
                            sign_list.Remove(vrd.CellSign);
                        }
                    }
                }
            }

            //最后进行自定义公式计算
            foreach (ViewRuleDef vrd in function_list)
            {
                string cellSign = vrd.CellSign;
                int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(cellSign, 2));
                int curr_row = int.Parse(KnowledgeUtil.SplitStr(cellSign, 1));
                string currExpress = vrd.CalExpress;
                currExpress = GetFunctionExpress(currExpress, grid);
                string cal_value = "0";

                //计算其余的值
                char[] patten = { '[', ']' };//分隔符
                string[] temp = currExpress.Split(patten);
                for (int i = 1; i < temp.Length; i = i + 2)
                {
                    string get_express = temp[i].ToString();
                    int col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(get_express, 2));
                    int row = int.Parse(KnowledgeUtil.SplitStr(get_express, 1));
                    string value = grid.Cell(row, col).Text;

                    if (value == null || "".Equals(value) || KnowledgeUtil.IfWidthNumber(value) == false)
                    {
                        value = "0";
                    }

                    currExpress = currExpress.Replace("[" + get_express + "]", value);
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
                grid.Cell(curr_row, curr_col).Text = cal_value;
                grid.Cell(curr_row, curr_col).Alignment = FlexCell.AlignmentEnum.RightCenter;
            }
        }

        //得到自定义公式的结果值,目前包括三个公式：(SUM([B2]:[B10])、SUMIF([B2]:[B10],<,0)、COUNTIF([B2]:[B10],<,0))
        public string GetFunctionExpress(string express, FlexCell.Grid grid)
        {
            if (express.IndexOf("SUM(") == -1 && express.IndexOf("SUMIF(") == -1 && express.IndexOf("COUNTIF(") == -1)
            {
                return express;
            }

            char[] pattern ={ ',' };
            if (express.IndexOf("SUM(") != -1)
            {
                string function = "SUM(" + KnowledgeUtil.SubStr(express, "SUM(", ")") + ")";
                string b = KnowledgeUtil.SubStr(express, "SUM(", ")");
                IList al = GetAddValues(b, grid);
                double result = KnowledgeUtil.SumCellValue(al);
                express = express.Replace(function, result+"");
            }

            if (express.IndexOf("SUMIF(") != -1)
            {
                string function = "SUMIF(" + KnowledgeUtil.SubStr(express, "SUMIF(", ")") + ")";
                string b = KnowledgeUtil.SubStr(express, "SUMIF(", ",");
                //取得比较符号和值
                string sign = "";
                string compareValue = "";

                string[] temp = function.Split(pattern);
                for (int i = 1; i < temp.Length; i++)
                {
                    if (i == 1)
                    {
                        sign = temp[i].ToString();
                    }
                    else
                    {
                        compareValue = temp[i].ToString();
                        compareValue = compareValue.Substring(0, compareValue.Length - 1);
                    }
                }
                IList al = GetAddValues(b, grid);
                double result = KnowledgeUtil.SumCellValueByExpress(al, sign, double.Parse(compareValue));
                express = express.Replace(function, result + "");
            }

            if (express.IndexOf("COUNTIF(") != -1)
            {
                string function = "COUNTIF(" + KnowledgeUtil.SubStr(express, "COUNTIF(", ")") + ")";
                string b = KnowledgeUtil.SubStr(express, "COUNTIF(", ",");
                //取得比较符号和值
                string sign = "";
                string compareValue = "";
                string[] temp = function.Split(pattern);
                for (int i = 1; i < temp.Length; i++)
                {
                    if (i == 1)
                    {
                        sign = temp[i].ToString();
                    }
                    else
                    {
                        compareValue = temp[i].ToString();
                        compareValue = compareValue.Substring(0, compareValue.Length - 1);
                    }
                }
                IList al = GetAddValues(b, grid);
                double result = KnowledgeUtil.CountCellByExpress(al, sign, double.Parse(compareValue));
                express = express.Replace(function, result + "");
            }

            if (express.IndexOf("SUM(") != -1 || express.IndexOf("SUMIF(") != -1 || express.IndexOf("COUNTIF(") != -1)
            {
                express = this.GetFunctionExpress(express, grid);
            }

            return express;
        }

        private IList GetAddValues(string adds,FlexCell.Grid grid)
        {
            IList al = new ArrayList();
            char[] pattern ={ ':' };
            string begin_str = "";
            string end_str = "";
            string[] temp = adds.Split(pattern);
            for (int i = 0; i < temp.Length; i++)
            {
                if (i == 0)
                {
                    begin_str = temp[i].ToString();
                }
                else
                {
                    end_str = temp[i].ToString();
                }
            }
            //取得第一位
            string first_char = KnowledgeUtil.SplitStr(begin_str.Substring(begin_str.IndexOf("[") + 1, begin_str.IndexOf("]") - 1), 2);
            string first_num = KnowledgeUtil.SplitStr(begin_str.Substring(begin_str.IndexOf("[") + 1, begin_str.IndexOf("]") - 1), 1);
            string end_num = KnowledgeUtil.SplitStr(end_str.Substring(end_str.IndexOf("[") + 1, end_str.IndexOf("]") - 1), 1);

            for (int i = int.Parse(first_num); i <= int.Parse(end_num); i++)
            {
                int curr_col = KnowledgeUtil.GetSignIndex(KnowledgeUtil.SplitStr(first_char+i, 2));
                int curr_row = int.Parse(KnowledgeUtil.SplitStr(first_char + i, 1));
                string value = grid.Cell(curr_row, curr_col).Text;
                if (KnowledgeUtil.IfWidthNumber(value) == false || "".Equals(value))
                {
                    value = "0";
                }

                al.Add(value);
            }
            return al;
        }

        //转换存储表达式
        public string TransSaveExpress(string saveExpress, string ifYwzz, int k, string time_str, string ywzz_str, int time_index, int ywzz_index)
        {
            if ("1".Equals(ifYwzz))
            {
                //插入时间
                if (time_index == k)//如果为最后一个位置
                {
                    saveExpress += SaveExpressDelimiter[0] + time_str;
                }
                else
                {
                    int start_pos = this.GetIndexByString(saveExpress, SaveExpressDelimiter[0], 0, time_index + 1);//后插入时间维度值的位置,从后一个位置往前插
                    saveExpress = saveExpress.Insert(start_pos, SaveExpressDelimiter[0] + time_str);
                }
            }
            else
            {
                //先后插入时间和业务组织维度,先插位置小的
                if (time_index < ywzz_index)
                {
                    //先插入时间
                    int start_pos = this.GetIndexByString(saveExpress, SaveExpressDelimiter[0], 1, time_index);//开始插入时间维度值的位置,从后一个位置往前插
                    saveExpress = saveExpress.Insert(start_pos, SaveExpressDelimiter[0] + time_str);

                    //后插入业务组织
                    if (ywzz_index == k)//如果为最后一个位置
                    {
                        saveExpress += SaveExpressDelimiter[0] + ywzz_str;
                    }
                    else
                    {
                        start_pos = this.GetIndexByString(saveExpress, SaveExpressDelimiter[0], 0, ywzz_index + 1);//后插入业务维度值的位置,从后一个位置往前插
                        saveExpress = saveExpress.Insert(start_pos, SaveExpressDelimiter[0] + ywzz_str);
                    }

                }
                else
                {
                    //先插入组织
                    int start_pos = this.GetIndexByString(saveExpress, SaveExpressDelimiter[0], 1, ywzz_index);//开始插入业务维度值的位置,从后一个位置往前插
                    saveExpress = saveExpress.Insert(start_pos, SaveExpressDelimiter[0] + ywzz_str);

                    //后插入时间
                    if (time_index == k)//如果为最后一个位置
                    {
                        saveExpress += SaveExpressDelimiter[0] + time_str;
                    }
                    else
                    {
                        start_pos = this.GetIndexByString(saveExpress, SaveExpressDelimiter[0], 0, time_index + 1);//后插入时间维度值的位置,从后一个位置往前插
                        saveExpress = saveExpress.Insert(start_pos, SaveExpressDelimiter[0] + time_str);
                    }
                }
            }
            return saveExpress;
        }

        #endregion


    }
}
