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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Util;
using System.IO;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using FlexCell;
using Microsoft.Office.Interop.Excel;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCostMonthReportNew : TBasicDataView
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        private MCostAccountSubject subject = new MCostAccountSubject();
        public MSubContractBalance subContractOperate = new MSubContractBalance();
        private CostMonthAccountBill costBill = new CostMonthAccountBill();
        private MStockMng stockModel = new MStockMng();
        private CurrentProjectInfo ProjectInfo;
        private List<CostAccountSubject> subjectList; //科目
        private bool ifZB = true;
        private const string SUBTOTAL = "小计";

        private List<CostMonthAccDtlConsume> allConsumes;
        private List<CostMonthAccDtlConsume> allTotalConsumes;

        CostMonthReportChangeMatser CurBillMaster;

        public VCostMonthReportNew()
        {
            InitializeComponent();

            InitEvents();

            InitGrid();

            InitData();
            SetToolMenuItemLockedOrNo();
        }

        private void InitData()
        {
            ProjectInfo = StaticMethod.GetProjectInfo();
            this.cmbProject.Enabled = false;
            cmbProject.Text = ProjectInfo.Name;
            IList list = stockModel.StockInSrv.GetFiscalYear();
            this.cmbYear.Items.Clear();
            foreach (int iYear in list)
            {
                this.cmbYear.Items.Insert(this.cmbYear.Items.Count, iYear);
                if (iYear == ConstObject.TheLogin.TheComponentPeriod.NowYear)
                {
                    this.cmbYear.SelectedItem = this.cmbYear.Items[this.cmbYear.Items.Count - 1];
                }
            }

            for (int i = 1; i < 13; i++)
            {
                this.cboFiscalMonth.Items.Add(i);
            }
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
            string appName = System.Configuration.ConfigurationManager.AppSettings["AppName"];
            if (ClientUtil.ToString(appName).Contains("基础设施"))
            {
                ifZB = false;
            }

            SetDefaultPeriod();

            GetWbsRootNode();

            GetAllSubject();

            tabCost_SelectedIndexChanged(null,null);
        }

        private void SetToolMenuItemLockedOrNo()
        {
            if (ToolMenu != null)
            {
                ToolMenu.LockItem(ToolMenuItem.Modify);
                ToolMenu.LockItem(ToolMenuItem.Refresh);
                ToolMenu.LockItem(ToolMenuItem.AddNew);
                ToolMenu.LockItem(ToolMenuItem.Save);
                ToolMenu.LockItem(ToolMenuItem.Delete);
                ToolMenu.LockItem(ToolMenuItem.Cancel);
                ToolMenu.LockItem(ToolMenuItem.Refresh);
                ToolMenu.LockItem(ToolMenuItem.Submit);
                ToolMenu.LockItem(ToolMenuItem.Check);
                ToolMenu.LockItem(ToolMenuItem.Export);
            }
        }

        private void SetDefaultPeriod()
        {
            string defaultPeriod = model.CostMonthAccSrv.GetDefaultPeriod(ProjectInfo.Id);
            if (defaultPeriod != "")
            {
                this.cmbYear.Text = defaultPeriod.Split('|')[0];
                this.cboFiscalMonth.Text = defaultPeriod.Split('|')[1];

            }
        }
        
        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
            this.tabCost.SelectedIndexChanged += new EventHandler(tabCost_SelectedIndexChanged);

            this.cmsGrid.ItemClicked += new ToolStripItemClickedEventHandler(cmsGrid_ItemClicked);

            this.gdContract.CellChange += new Grid.CellChangeEventHandler(gdContract_CellChange);
            this.gdMaterial.CellChange += new Grid.CellChangeEventHandler(gdMaterial_CellChange);
            this.btn_Save.Click += new EventHandler(btn_Save_Click);
        }

        #region 控制表格行的显隐 可以通用
        //说明：只要grid.Cell(i, 1)为序号，而且层级自上而下 中文数字，英文字母，阿拉伯数字
        //如果新加表格会直接套用，所以请遵循这个规定

        void tabCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomFlexGrid grid = FinGridFormTabPage(this.tabCost.SelectedTab);
            if (grid == null)
                return;
            grid.MouseUp -= new Grid.MouseUpEventHandler(grid_MouseUp);
            grid.MouseUp += new Grid.MouseUpEventHandler(grid_MouseUp);
        }

        void cmsGrid_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            cmsGrid.Close();
            CustomFlexGrid grid = FinGridFormTabPage(this.tabCost.SelectedTab);
            if (grid == null)
                return;
            switch (e.ClickedItem.Name)
            {
                case "miHideDetail":
                    HideDetail(grid, 2);
                    break;
                case "miHideNoDataRow":
                    HideNoDataRow(grid);
                    break;
                case "miShowAllRow":
                    ShowAllRow(grid);
                    break;
                default:
                    break;
            }
        }

        private void HideNoDataRow(CustomFlexGrid grid)
        {
            int beginCol = 0 ;
            int endCol = 0;
            int beginRowIndex = 0;
            switch (grid.Name)
            {
                case "gdTotal"://汇总表
                    beginRowIndex = 5;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdContract"://合同内汇总
                    beginRowIndex = 4;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdLabour"://劳务
                    beginRowIndex = 4;
                    beginCol = 4;
                    endCol = grid.Cols - 1;
                    break;
                case "gdMaterial"://材料
                    beginRowIndex = 4;
                    beginCol = 4;
                    endCol = grid.Cols - 1;
                    break;
                case "gdAmountPrice"://材料量价
                    beginRowIndex = 4;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdMachine"://机械
                    beginRowIndex = 4;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdMeasures"://措施
                    beginRowIndex = 4;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdManager"://管理
                    beginRowIndex = 3;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdSubcontrat"://分包
                    beginRowIndex = 4;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdOther"://其他
                    beginRowIndex = 3;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdChange"://变更
                    beginRowIndex = 4;
                    beginCol = 4;
                    endCol = grid.Cols - 1;
                    break;
                case "gdTime"://记时工
                    beginRowIndex = 4;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                case "gdVisa"://签证索赔
                    beginRowIndex = 4;
                    beginCol = 3;
                    endCol = grid.Cols - 1;
                    break;
                default:
                    break;
            }
            if (beginRowIndex >= grid.Rows)
                return;
            if (endCol > 0)
            {
                for (int i = beginRowIndex; i < grid.Rows; i++)
                {
                    
                    if (grid.Name == "gdTotal" && i == grid.Rows - 1) continue;
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    bool vis = true;
                    if (strType == 1 || strType == 2)//序号为中文数字或应为字母 非末级
                    {
                        bool b_flag = FindnextTypeRow(grid, i, strType, beginCol, endCol);
                        if (b_flag)
                            vis = true;
                        else
                            vis = HaveNoZeroNumber(grid, i, beginCol, endCol);

                       
                    }
                    else if (strType == 3)//序号阿拉伯数字 末级
                    {
                        vis = HaveNoZeroNumber(grid, i, beginCol, endCol);
                    }
                    else//木有序号
                    {
                        vis = HaveNoZeroNumber(grid, i, beginCol, endCol); 
                    }
                    grid.Row(i).Visible = vis;
                }
            }
        }

        /// <summary>
        /// 是否有数据不全为0 的子集行
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="beginRowIndex"></param>
        /// <param name="strType"></param>
        /// <param name="beginCol"></param>
        /// <param name="endCol"></param>
        /// <returns></returns>
        private bool FindnextTypeRow(CustomFlexGrid grid, int beginRowIndex, int strType, int beginCol, int endCol)
        {
            bool flag = false;

            for (int i = beginRowIndex+1; i < grid.Rows; i++)
            {
                int strType_next = CheckStringType(grid.Cell(i, 1).Text);
                if (strType_next == 3)//有子集
                {
                    bool vis = HaveNoZeroNumber(grid, i, beginCol, endCol);
                    if (vis)//自己有不为0 的行
                    {
                        flag = true;
                        break;
                    }
                }
                    
                if ((strType == 1 && strType_next == strType) || (strType == 2 && (strType_next == 2 || strType_next == 1)))
                    break;
            }
            return flag;

        }

     

        private bool  HaveNoZeroNumber(CustomFlexGrid grid, int rowindex ,int beginCol, int endCol)
        {
            bool flag = false;
            for (int j = beginCol; j < endCol+1; j++)
            {
                string strText = grid.Cell(rowindex, j).Text.Trim();
                bool isNumber = CheckIsNumber(strText);
                if (isNumber && ClientUtil.ToDecimal(strText) != 0m) //本行有数据不为0 
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private void ShowAllRow(CustomFlexGrid grid)
        {
            int count = grid.Rows;
            for (int i = 0; i < count; i++)
            {
                grid.Row(i).Visible = true;
            }
        }

        private void HideDetail(CustomFlexGrid grid, int layer)
        {
            int count = grid.Rows;
            for (int i = 0; i < count; i++)
            {
                int strType = CheckStringType(grid.Cell(i, 1).Text);
                grid.Row(i).Visible = GetGridRowVisible(strType, layer);
            }
        }

        private CustomFlexGrid FinGridFormTabPage(TabPage tp)
        {
            foreach (Control item in tp.Controls)
            {
                if (item is CustomFlexGrid)
                    return item as CustomFlexGrid;
            }
            return null;
        }
        void grid_MouseUp(object Sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CustomFlexGrid grid = Sender as CustomFlexGrid;
                //MessageBox.Show(grid.Name);
                cmsGrid.Show(grid, e.X, e.Y);
            }
        }
        /// <summary>
        /// 检测文本类型  1 中文数字，2 英文字母，3 阿拉伯数字，0其他
        /// </summary>
        /// <param name="str">需要检测的字符串</param>
        /// <returns>1 中文数字，2 英文字母，3 阿拉伯数字，0其他</returns>
        private int CheckStringType(string str)
        {
            if (str.Trim() == "")
                return 0;
            char[] chinese = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九','十' };
            char[] english = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] arab = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            bool flag = true;
            foreach (char item in str)
	        {
                if (!chinese.Contains(item))//有一个不是中文数字
                {
                    flag = false;
                    break;
                }
	        }
            if (flag)
                return 1;

            flag = true;
            foreach (char item in str)
            {
                if (!english.Contains(item))
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
                return 2;

            flag = true;
            foreach (char item in str)
            {
                if (!arab.Contains(item))
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
                return 3;

            return 0;
        }
        /// <summary>
        /// 根据序号类文本的类型和要求显示的层次获得Grid行是否该显示
        /// </summary>
        /// <param name="type">序号类文本的类型 1 中文数字，2 英文字母，3 阿拉伯数字，0其他</param>
        /// <param name="layer">要求显示的层次</param>
        /// <returns></returns>
        private bool GetGridRowVisible(int type, int layer)
        {
            if (type == 0)
                return true;
            return type <= layer;
            
        }

        private bool CheckIsNumber(string str)
        {
            if (str.Trim() == "") return false;
            char[] arab = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '-',',' };
            str = str.Replace("%", "");

            bool flag = true;
            foreach (char item in str)
            {
                if (!arab.Contains(item))
                {
                    flag = false;
                    break;
                }
            }
            return flag;

        }
        #endregion

        private void InitGrid()
        {
            foreach (TabPage tpage in tabCost.TabPages)
            {
                var grid = FindFlexGrid(tpage);
                if (grid != null)
                {
                    LoadFlexFile(string.Concat(grid.Tag, ".flx"), grid);
                }
            }
        }

        private CustomFlexGrid FindFlexGrid(TabPage tp)
        {
            foreach (var ct in tp.Controls)
            {
                if (ct is CustomFlexGrid)
                {
                    return ct as CustomFlexGrid;
                }
            }

            return null;
        }

        private void GetWbsRootNode()
        {
            if (ProjectInfo == null)
            {
                return;
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheProjectGUID", ProjectInfo.Id));
            objectQuery.AddCriterion(Expression.IsNull("ParentNode"));

            IList list = subContractOperate.ObjectQuery(typeof (GWBSTree), objectQuery);
            IList taskList = new ArrayList();
            if (list == null || list.Count == 0)
            {
                txtAccountRootNode.Text = "";
                txtAccountRootNode.Tag = null;
            }
            else
            {
                GWBSTree tGwbs = (GWBSTree) list[0];
                taskList.Add(tGwbs.Id);
                txtAccountRootNode.Text = tGwbs.Name;
                txtAccountRootNode.Tag = taskList;
            }
        }

        private void GetAllSubject()
        {
            //载入科目
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddOrder(Order.Asc("Code"));

            subjectList = subject.Mm.GetCostAccountSubjects(typeof (CostAccountSubject), oq)
                .OfType<CostAccountSubject>().ToList();
        }

        private void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());

            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsCheck = true; //是否有checkbox

            frm.IsRootNode = true; //这个参数是否只选择隶属关系的根节点
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                IList taskList = new ArrayList();
                string taskStr = "";
                foreach (TreeNode node in frm.SelectResult)
                {
                    GWBSTree task = node.Tag as GWBSTree;
                    taskList.Add(task.Id);
                    taskStr += task.Name + "_";
                }
                TreeNode root = frm.SelectResult[0];

                txtAccountRootNode.Text = taskStr;
                txtAccountRootNode.Tag = taskList;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            var saveDialg = new SaveFileDialog();
            saveDialg.FileName = cmbYear.Text + "年" + cboFiscalMonth.Text + "月项目" + cmbProject.Text + "月度成本分析表";
            saveDialg.Filter = "Excel文件(*.xls)|*.xls";
            if (saveDialg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            FlashScreen.Show("数据导出中，请稍等…");

            var deskFile = saveDialg.FileName;
            var tmpFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                                                 string.Concat(Guid.NewGuid().ToString(), ".xls"));
            foreach (TabPage tp in tabCost.TabPages)
            {
                var grid = FindFlexGrid(tp);
                if (grid == null)
                {
                    continue;
                }

                if (!System.IO.File.Exists(deskFile))
                {
                    grid.ExportToExcel(deskFile, tp.Text, true, true);
                }
                else
                {
                    grid.ExportToExcel(tmpFile, tp.Text, true, true);
                    grid.MergeExcel(deskFile, tmpFile, true);
                }
            }

            FlashScreen.Close();

            if (MessageBox.Show("导出成功，是否打开文件查看？", "打开确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(saveDialg.FileName);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (ClientUtil.ToString(this.cmbYear.Text) == "")
            {
                MessageBox.Show("请输入会计年！");
                return;
            }
            if (ClientUtil.ToString(this.cboFiscalMonth.Text) == "")
            {
                MessageBox.Show("请输入会计月！");
                return;
            }

            FlashScreen.Show("正在统计月度成本核算报表...");
            try
            {

                //载入数据
                IList taskList = txtAccountRootNode.Tag as ArrayList;
                if (taskList == null)
                {
                    taskList = new ArrayList();
                }

                int year = ClientUtil.ToInt(cmbYear.Text);
                int month = ClientUtil.ToInt(cboFiscalMonth.Text);
                allTotalConsumes = 
                    model.CostMonthAccSrv.GetRealCostConsume(ProjectInfo.Id,
                    taskList.OfType<string>().ToList(), year, month);

                CurBillMaster = RetriveCostMonthReportChangeMatser(year, month);

                InitGrid();

                LoadMaterialData();
                LoadMaterialDiffData();
                LoadMachineData();
                LoadSubcontractData();
                LoadLabourvData();
                LoadMeasuresData();
                LoadManagerData();
                LoadTimeData();
                LoadOtherData();
                MaintainFixedRateRsp();
                LoadContractData();
                LoadTotalData();

             
                //设置外观
                CommonUtil.SetFlexGridFace(this.gdTotal);
                CommonUtil.SetFlexGridFace(this.gdSubcontrat);
                CommonUtil.SetFlexGridFace(this.gdMachine);
                CommonUtil.SetFlexGridFace(this.gdManager);
                CommonUtil.SetFlexGridFace(this.gdVisa);
                CommonUtil.SetFlexGridFace(this.gdLabour);
                CommonUtil.SetFlexGridFace(this.gdMeasures);
                CommonUtil.SetFlexGridFace(this.gdMaterial);
                CommonUtil.SetFlexGridFace(this.gdChange);
                CommonUtil.SetFlexGridFace(this.gdContract);
                CommonUtil.SetFlexGridFace(this.gdTime);
                CommonUtil.SetFlexGridFace(this.gdOther);
                CommonUtil.SetFlexGridFace(this.gdAmountPrice);
            }
            catch (Exception e1)
            {
                MessageBox.Show("查询月度成本分析数据失败：" + e1.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void LoadFlexFile(string flxname, CustomFlexGrid grid)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(flxname))
            {
                eFile.CreateTempleteFileFromServer(flxname);

                grid.OpenFile(path + "\\" + flxname); //载入格式
            }
            grid.EnterKeyMoveTo = MoveToEnum.NextRow;
        }

        #region  构造数据

        private void GetAllConsumes()
        {
            allConsumes = new List<CostMonthAccDtlConsume>();

            if (costBill == null)
            {
                return;
            }

            foreach (CostMonthAccountDtl dtl in costBill.Details)
            {
                allConsumes.AddRange(dtl.Details);
            }

            RefreshResourceName();
        }

        private void RefreshResourceName()
        {
            if (allConsumes == null)
            {
                return;
            }

            var res = allConsumes.GroupBy(a => a.ResourceTypeGUID);
            var objQuery = new ObjectQuery();
            var idList = new Disjunction();
            foreach (var gp in res)
            {
                idList.Add(Expression.Eq("Id", gp.Key));
            }
            objQuery.AddCriterion(idList);

            var resList = model.ObjectQuery(typeof (Material), objQuery);
            foreach (Material mat in resList)
            {
                var list = allConsumes.FindAll(a => a.ResourceTypeGUID == mat.Id);
                foreach (var consume in list)
                {
                    consume.ResourceTypeName = mat.Name;
                    consume.ResourceTypeSpec = mat.Specification;
                }
            }
        }

        /// <summary>
        /// 查询此根节点下几层的成本科目,生成月度核算物资消耗对象集合,并排序
        /// </summary>
        /// <param name="rootSubject">查询的成本科目根节点</param>
        /// <param name="queryLevel">向下查询几层</param>
        /// <param name="ifContainSelf">是否包含自己</param>
        private List<CostMonthAccDtlConsume> GetCostSubjectList(string rootSubjectCode, int queryLevel,
                                                                bool ifContainSelf)
        {
            CostAccountSubject rootSubject = subjectList.Find(s => s.Code == rootSubjectCode);

            var list = new List<CostMonthAccDtlConsume>();
            int rootLevel = rootSubject.Level;
            int maxLevel = rootLevel + queryLevel;
            string rootSyscode = rootSubject.SysCode;

            var subjectList_Head = subjectList.FindAll(a => 
                                                            (ifContainSelf && a.SysCode.Equals(rootSyscode)))
                                                           ;
            foreach (CostAccountSubject subject in subjectList_Head)
            {
                CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                dtlConsume.Data2 = "0"; //相对层级
                dtlConsume.CostingSubjectGUID = subject;
                dtlConsume.CostingSubjectName = subject.Name;
                dtlConsume.CostSubjectCode = subject.Code;
                dtlConsume.CostSubjectSyscode = subject.SysCode;
                list.Add(dtlConsume);
            }

            var subjectList_Detail = subjectList.FindAll(a => (rootSyscode != null && a.SysCode.Contains(rootSyscode) && a.Level > rootLevel &&
                    a.Level <= maxLevel)).OrderBy(a => a.Level).ThenBy(a => a.OrderNo);
            foreach (CostAccountSubject subject in subjectList_Detail)
            {
                CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                dtlConsume.Data2 = (subject.Level - rootLevel) + ""; //相对层级
                dtlConsume.CostingSubjectGUID = subject;
                dtlConsume.CostingSubjectName = subject.Name;
                dtlConsume.CostSubjectCode = subject.Code;
                dtlConsume.CostSubjectSyscode = subject.SysCode;
                list.Add(dtlConsume);
            }
            return list;
        }

        private string  GetCostSubjectSyscode(string subcode)
        {
            CostAccountSubject costSubject = subjectList.Find(s => s.Code == subcode);
            return costSubject == null ? "XXXXXXXXXXXXXX" : costSubject.SysCode;

        }
        private void SetGridStyle(CustomFlexGrid flexGrid, int frozenRows, int frozenCols)
        {
            var range = flexGrid.Range(frozenRows + 1, 1, flexGrid.Rows - 1, flexGrid.Cols - 1);
            range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);
            range.set_Borders(EdgeEnum.Left, LineStyleEnum.Thick);
            range.set_Borders(EdgeEnum.Right, LineStyleEnum.Thick);

            flexGrid.Column(1).Alignment = AlignmentEnum.CenterCenter;
            flexGrid.DefaultRowHeight = 23;
            flexGrid.FrozenRows = frozenRows;
            flexGrid.FrozenCols = frozenCols;
            if (flexGrid == gdMaterial || flexGrid == gdContract)
            {
                int i = 1;
                while (i < 5)
                {
                    flexGrid.Row(i).Locked = true;
                    i++;
                }
            }
            else
                flexGrid.Locked = true;

            range = flexGrid.Range(frozenRows + 1, frozenCols + 1, flexGrid.Rows - 1, flexGrid.Cols - 1);
            range.Alignment = AlignmentEnum.RightCenter;
        }

        #endregion

        #region 直接材料费对比表

        private void LoadMaterialData()
        {
            var subCode = "C51102";
            if (!ifZB)
            {
                subCode = "C56102";
            }
            var subCodeSysCode = GetCostSubjectSyscode(subCode);
            gdMaterial.CellChange -= new Grid.CellChangeEventHandler(gdMaterial_CellChange);
            var subList = GetCostSubjectList(subCode, 2, false);
            var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectSyscode.Contains(subCodeSysCode) && a.Data1 == "0");
            var rowIndex = 5; 

            var lvOneSubs = subList.FindAll(a => a.Data2 == "1");
            for (var i = 0; i < lvOneSubs.Count; i++)
            {
                gdMaterial.InsertRow(rowIndex, 1);
                gdMaterial.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(i + 1);
                gdMaterial.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                var subCodeSysCode_1 = GetCostSubjectSyscode(lvOneSubs[i].CostSubjectCode);

                DisplayMaterialData_Subject(ref rowIndex, totalUses, subCodeSysCode_1, false);
                rowIndex++;
                DisplayMaterialData_Resource(ref rowIndex, totalUses, subCodeSysCode_1, true);



                var lvTwoSubs =
                    subList.FindAll(a => a.Data2 == "2" && a.CostSubjectSyscode.Contains(subCodeSysCode_1));
                for (var j = 0; j < lvTwoSubs.Count; j++)
                {
                    gdMaterial.InsertRow(rowIndex, 1);
                    gdMaterial.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(65 + j) });
                    gdMaterial.Cell(rowIndex, 2).Text = lvTwoSubs[j].CostingSubjectName;
                    gdMaterial.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                    var subCodeSysCode_2 = GetCostSubjectSyscode(lvTwoSubs[j].CostSubjectCode);

                    DisplayMaterialData_Subject(ref rowIndex, totalUses, subCodeSysCode_2, false);
                    rowIndex++;

                    DisplayMaterialData_Resource(ref rowIndex, totalUses, subCodeSysCode_2, false);
                }
            }
            MaintainCostMonthReportChangeDetailSum(gdMaterial, ChangeSolutionType.ChangeDiff);
            DisplayMaterialTotal();
            gdMaterial.CellChange += new Grid.CellChangeEventHandler(gdMaterial_CellChange);

            SetGridStyle(gdMaterial, 4, 3);
        }
        private void DisplayMaterialData_Subject(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            var subDetail = details.FindAll(a => (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode));
            if (subDetail.Count() == 0)
                return;
            CustomFlexGrid grid = gdMaterial;
         
            grid.Range(rowIndex, 1, rowIndex, grid.Cols - 1).FontBold = true;

            grid.Cell(rowIndex, 2).Tag = subjectCode+"|||"; 
            //grid.Cell(rowIndex, 4).Text = 0;//累计合同收入数量
            //合同
            var qty = subDetail.Sum(a => a.SumIncomeQuantity);
            var tp = subDetail.Sum(a => a.SumIncomeTotalPrice);
            var price = (qty == 0 ? 0m : Math.Round(tp / qty, 4));
            var strprice = (price == 0 ? "0" : price.ToString("N4"));
            grid.Cell(rowIndex, 4).Text = qty.ToString("N2");
            grid.Cell(rowIndex, 5).Text = strprice;
            grid.Cell(rowIndex, 6).Text = tp.ToString("N2");

            //责任
            qty = subDetail.Sum(a => a.SumResponsiConsumeQuantity);
            tp = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
            price = (qty == 0 ? 0m : Math.Round(tp / qty, 4));
            strprice = (price == 0 ? "0" : price.ToString("N4"));
            grid.Cell(rowIndex, 7).Text = qty.ToString("N2");
            grid.Cell(rowIndex, 8).Text = strprice;

            grid.Cell(rowIndex, 9).Text = tp.ToString("N2");

            //实际
            qty = subDetail.Sum(a => a.SumRealConsumeQuantity);
            tp = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
            price = (qty == 0 ? 0m : Math.Round(tp / qty, 4));
            strprice = (price == 0 ? "0" : price.ToString("N4"));
            grid.Cell(rowIndex, 10).Text = qty.ToString("N2");
            grid.Cell(rowIndex, 14).Text = strprice;
            grid.Cell(rowIndex, 15).Text = tp.ToString("N2");  

            qty = subDetail.Sum(a => a.SumIncomeTotalPrice);
            tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
            grid.Cell(rowIndex, 16).Text = tp.ToString("N2");
            if (qty != 0)
            {
                grid.Cell(rowIndex, 17).Text = Math.Round(tp / qty, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, 17).Text = "0%";
            }

            qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
            tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
            grid.Cell(rowIndex, 18).Text = tp.ToString("N2");
            if (qty != 0)
            {
                grid.Cell(rowIndex, 19).Text = Math.Round(tp / qty, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, 19).Text = "0%";
            }
            
            grid.Row(rowIndex).Locked = true;
        }

        private bool IsHaveChildren(List<CostMonthAccDtlConsume> details, string subjectCode)
        { 
            var subDetail = details.FindAll(a =>  a.CostSubjectCode.Contains(subjectCode) && a.CostSubjectCode != subjectCode);
            return subDetail.Count() > 0;
        }
        private void DisplayMaterialData_Resource(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => new {a.ResourceTypeName, a.ResourceTypeSpec, a.RationUnitName})
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);
            if (subDetails.Count() == 0)
            {
                return;
            }

            var rowNo = 0;
     
            CustomFlexGrid grid = gdMaterial;

            foreach (var subDetail in subDetails)
            {
                grid.InsertRow(rowIndex, 1);
                rowNo++;

                grid.Cell(rowIndex, 1).Text = rowNo.ToString();
                grid.Cell(rowIndex, 2).Text =
                    string.Format("{0} {1}", subDetail.Key.ResourceTypeName, subDetail.Key.ResourceTypeSpec);
                grid.Cell(rowIndex, 2).Tag = subjectCode + "|" + subDetail.Key.ResourceTypeName + "|" + subDetail.Key.ResourceTypeSpec + "|" + subDetail.Key.RationUnitName; 

                grid.Cell(rowIndex, 3).Text = subDetail.Key.RationUnitName;
                grid.Range(rowIndex, 1, rowIndex, grid.Cols-1).FontBold = false;

                //合同
                var qty = subDetail.Sum(a => a.SumIncomeQuantity);
                var tp = subDetail.Sum(a => a.SumIncomeTotalPrice);
                var price = (qty ==0?0m: Math.Round(tp / qty, 4));
                var strprice = (price == 0 ?"0":price.ToString("N4"));
                grid.Cell(rowIndex, 4).Text = qty.ToString("N2");
                grid.Cell(rowIndex, 5).Text = strprice;
                grid.Cell(rowIndex, 6).Text = tp.ToString("N2");

                //责任
                qty = subDetail.Sum(a => a.SumResponsiConsumeQuantity);
                tp = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                price=(qty ==0?0m: Math.Round(tp / qty, 4));
                strprice=(price == 0 ?"0":price.ToString("N4"));
                grid.Cell(rowIndex, 7).Text = qty.ToString("N2");
                grid.Cell(rowIndex, 8).Text = strprice;
              
                grid.Cell(rowIndex, 9).Text = tp.ToString("N2");

                //实际
                qty = subDetail.Sum(a => a.SumRealConsumeQuantity);
                tp = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                price=(qty ==0?0m: Math.Round(tp / qty, 4));
                strprice=(price == 0 ?"0":price.ToString("N4"));
                grid.Cell(rowIndex, 10).Text = qty.ToString("N2");
                grid.Cell(rowIndex, 14).Text = strprice;
                string strTag = ClientUtil.ToString(grid.Cell(rowIndex, 2).Tag );
                if (!string.IsNullOrEmpty(strTag) && strTag.Split('|').Length == 4)
                {
                    string scode = strTag.Split('|')[0];
                    string rname = strTag.Split('|')[1];
                    string rspec = strTag.Split('|')[2];
                    string runit = strTag.Split('|')[3];

                    CostMonthReportChangeDetail cmrcd = FindOrAddChangeDiffs(scode, rname, rspec, runit);
                    grid.Cell(rowIndex, 11).Text = cmrcd.ChangeQty.ToString("N2");//调整数量
                    grid.Cell(rowIndex, 12).Text = cmrcd.ChangeRemark;//调整原因
                    grid.Cell(rowIndex, 13).Text = (qty + cmrcd.ChangeQty).ToString("N2");//调整后数量
                    grid.Cell(rowIndex, 15).Text = (tp + cmrcd.ChangeQty * price).ToString("N2");

                    qty = subDetail.Sum(a => a.SumIncomeTotalPrice) ;
                    tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice) - cmrcd.ChangeQty;
                    grid.Cell(rowIndex, 16).Text = tp.ToString("N2");
                    if (qty != 0)
                    {
                        grid.Cell(rowIndex, 17).Text = Math.Round(tp / qty, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(rowIndex, 17).Text = "0%";
                    }

                    qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                    tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice) - cmrcd.ChangeQty;
                    grid.Cell(rowIndex, 18).Text = tp.ToString("N2");
                    if (qty != 0)
                    {
                        grid.Cell(rowIndex, 19).Text = Math.Round(tp / qty, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(rowIndex, 19).Text = "0%";
                    }
                }
                else
                {

                    qty = subDetail.Sum(a => a.SumIncomeTotalPrice);
                    tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                    grid.Cell(rowIndex, 16).Text = tp.ToString("N2");
                    if (qty != 0)
                    {
                        grid.Cell(rowIndex, 17).Text = Math.Round(tp / qty, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(rowIndex, 17).Text = "0%";
                    }

                    qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                    tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                    grid.Cell(rowIndex, 18).Text = tp.ToString("N2");
                    if (qty != 0)
                    {
                        grid.Cell(rowIndex, 19).Text = Math.Round(tp / qty, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(rowIndex, 19).Text = "0%";
                    }
                }
                grid.Row(rowIndex).Locked = false;
                LockGridCellExcept(grid, rowIndex, new int[] { 11, 12 });
                rowIndex++;
            }

            #endregion
           
        }

        private void LockGridCellExcept(CustomFlexGrid grid, int rowIndex, int[] except)
        {
            for (int i = 0; i < grid.Cols; i++)
            {
                if (except.Contains(i))
                    grid.Cell(rowIndex, i).Locked = false;
                else
                    grid.Cell(rowIndex, i).Locked = true;
            }
        }

        /// <summary>
        /// 汇总调整量
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="changeType"></param>
        private void MaintainCostMonthReportChangeDetailSum(CustomFlexGrid grid, ChangeSolutionType changeType)
        {
            #region
            if (changeType == ChangeSolutionType.ChangeDiff)
            {
                for (int i = 4; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType == 1 || strType == 2)
                    {

                        string strTag = ClientUtil.ToString(grid.Cell(i, 2).Tag);
                        if (string.IsNullOrEmpty(strTag) || strTag.Split('|').Length != 4)
                        {
                            continue;
                        }
                        string subcode = strTag.Split('|')[0];
                        var EffictDetails = CurBillMaster.Details.OfType<CostMonthReportChangeDetail>().ToList().FindAll(a => a.ChangeType == changeType && a.CostSubjectCode.Contains(subcode));
                        decimal changeQty = 0;
                        decimal realQty = 0;
                        decimal realMoney = 0m;
                        decimal price = 0;
                        if (grid == gdMaterial)
                        {
                            realQty = ClientUtil.ToDecimal(grid.Cell(i, 10).Text);
                            price = ClientUtil.ToDecimal(grid.Cell(i, 14).Text);
                            realMoney = ClientUtil.ToDecimal(grid.Cell(i, 15).Text);
                            changeQty = EffictDetails.Sum(a => a.ChangeQty);

                            var qtyAfter = realQty + changeQty;
                            var realAfter = realMoney + changeQty * price;
                            var contractMoney = ClientUtil.ToDecimal(grid.Cell(i, 6).Text.Trim());
                            var responseMoney = ClientUtil.ToDecimal(grid.Cell(i, 9).Text.Trim());

                            grid.Cell(i, 11).Text = changeQty.ToString("N2");
                            grid.Cell(i, 13).Text = qtyAfter.ToString("N2");
                            grid.Cell(i, 15).Text = realAfter.ToString("N2");
                            grid.Cell(i, 16).Text = (contractMoney - realAfter).ToString("N2");
                            grid.Cell(i, 17).Text = contractMoney == 0m ? "0%" : ((contractMoney - realAfter) / contractMoney).ToString("P2");
                            grid.Cell(i, 18).Text = (responseMoney - realAfter).ToString("N2");
                            grid.Cell(i, 19).Text = responseMoney == 0m ? "0%" : ((responseMoney - realAfter) / responseMoney).ToString("P2");
                        }

                    }
                }
            }
            #endregion
            else if (changeType == ChangeSolutionType.Estimate)
            {
                for (int i = 5; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType == 1)
                    {

                        string strTag = ClientUtil.ToString(grid.Cell(i, 2).Tag);
                        if (string.IsNullOrEmpty(strTag))
                        {
                            continue;
                        }
                        string subcode = strTag;
                        var EffictDetails = CurBillMaster.Details.OfType<CostMonthReportChangeDetail>().ToList().FindAll(a => a.ChangeType == changeType && a.CostSubjectCode.Contains(subcode) && a.CostSubjectCode != subcode);
                        if (EffictDetails.Count == 0)
                            continue;
                        if (grid == gdContract)
                        {
                            
                            var realMoney  = ClientUtil.ToDecimal(grid.Cell(i, 5).Text);
                            var estimateMoney = EffictDetails.Sum(a => a.ChangeBudgetMoney);
                            CostMonthReportChangeDetail cmrcd = FindOrAddEstimates(subcode);
                            cmrcd.ChangeBudgetMoney = estimateMoney;

                            var realAfter = (realMoney + estimateMoney);
                            var contractMoney = ClientUtil.ToDecimal(grid.Cell(i, 3).Text.Trim());
                            var responseMoney = ClientUtil.ToDecimal(grid.Cell(i, 4).Text.Trim());

                            grid.Cell(i, 6).Text = estimateMoney.ToString("N2");
                            grid.Cell(i, 8).Text = realAfter.ToString("N2");
                            grid.Cell(i, 9).Text = (contractMoney - realAfter).ToString("N2");
                            grid.Cell(i, 10).Text = contractMoney == 0m ? "0%" : ((contractMoney - realAfter) / contractMoney).ToString("P2");
                            grid.Cell(i, 11).Text = (responseMoney - realAfter).ToString("N2");
                            grid.Cell(i, 12).Text = responseMoney == 0m ? "0%" : ((responseMoney - realAfter) / responseMoney).ToString("P2");
                        }

                    }
                }
            }
        }
        /// <summary>
        /// 合计
        /// </summary>
        private void DisplayMaterialTotal()
        {
            CustomFlexGrid grid = gdMaterial;
            grid.Range(grid.Rows - 1, 1, grid.Rows - 1, grid.Cols - 1).FontBold = true;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 4; j < gdMaterial.Cols; j++)
            {
                if (j == 5 || j == 8 || j == 14 || j==12) //单价 调整原因
                {
                    continue;
                }
                else if (j == 17 || j == 19)//节超比例
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 17 ? 6 : 9).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 4; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);
                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
            grid.Row(grid.Rows - 1).Locked = true;
        }

        #endregion

        #region 材料量差价差对比表

        private void LoadMaterialDiffData()
        {
            gdAmountPrice.InsertRow(4, gdMaterial.Rows - 5);
            var copyCols = new Dictionary<int, int>();
            copyCols.Add(1, 1);
            copyCols.Add(2, 2);
            copyCols.Add(3, 4);
            copyCols.Add(4, 10);
            copyCols.Add(6, 5);
            copyCols.Add(7, 14);
            copyCols.Add(9, 6);
            copyCols.Add(10, 15);
            for (int i = 5; i < gdMaterial.Rows; i++)
            {
                foreach (var col in copyCols)
                {
                    var targetCell = gdAmountPrice.Cell(i, col.Key);
                    var sourceCell = gdMaterial.Cell(i, col.Value);

                    targetCell.Text = sourceCell.Text;
                    targetCell.FontBold = sourceCell.FontBold;
                    targetCell.Alignment = sourceCell.Alignment;
                }

                gdAmountPrice.Cell(i, 5).Text =
                    (gdMaterial.Cell(i, 4).DoubleValue - gdMaterial.Cell(i, 10).DoubleValue).ToString("N2");
                gdAmountPrice.Cell(i, 8).Text = 
                    (gdMaterial.Cell(i, 5).DoubleValue - gdMaterial.Cell(i, 11).DoubleValue).ToString("N4");
                gdAmountPrice.Cell(i, 11).Text =
                    (gdMaterial.Cell(i, 6).DoubleValue - gdMaterial.Cell(i, 12).DoubleValue).ToString("N2");
            }

            SetGridStyle(gdAmountPrice, 3, 2);
        }

        #endregion

        #region 机械费对比表

        private void LoadMachineData()
        {
            var subCode = "C51103";
            if (!ifZB)
            {
                subCode = "C56103";
            }
            var subCodeSysCode = GetCostSubjectSyscode(subCode);
            var subList = GetCostSubjectList(subCode, 2, false);
            var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectSyscode.Contains(subCodeSysCode) && a.Data1 == "0");
            var rowIndex = 4;

            var lvOneSubs = subList.FindAll(a => a.Data2 == "1");
            var grid = gdMachine;
            for (var i = 0; i < lvOneSubs.Count; i++)
            {
                grid.InsertRow(rowIndex, 1);
                grid.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(i + 1);
                grid.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                var subCodeSysCode_1 = GetCostSubjectSyscode(lvOneSubs[i].CostSubjectCode);
                DisplayMachineData_Subject(ref rowIndex, totalUses, lvOneSubs[i].CostSubjectCode,subCodeSysCode_1, false);
                rowIndex++;
                DisplayMachineData_Resource(ref rowIndex, totalUses,lvOneSubs[i].CostSubjectCode, subCodeSysCode_1, true);
                var lvTwoSubs =
                    subList.FindAll(a => a.Data2 == "2" && a.CostSubjectSyscode.Contains(subCodeSysCode_1));
                for (var j = 0; j < lvTwoSubs.Count; j++)
                {
                    grid.InsertRow(rowIndex, 1);
                    grid.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(65 + j) });
                    grid.Cell(rowIndex, 2).Text = lvTwoSubs[j].CostingSubjectName;
                    grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                    var subCodeSysCode_2 = GetCostSubjectSyscode(lvTwoSubs[j].CostSubjectCode);
                    DisplayMachineData_Subject(ref rowIndex, totalUses,lvTwoSubs[j].CostSubjectCode, subCodeSysCode_2, false);
                    rowIndex++;

                    DisplayMachineData_Resource(ref rowIndex, totalUses,lvTwoSubs[j].CostSubjectCode, subCodeSysCode_2, false);
                }
            }

            DisplayMachineTotal();

            SetGridStyle(grid, 3, 2);
        }

        private void DisplayMachineData_Resource(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode,string subsyscode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subsyscode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subsyscode))
                .GroupBy(a => new { a.ResourceTypeName, a.ResourceTypeSpec })
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);
            //if (subDetails.Count() == 0 && subjectCode != "C5110306")
            if (subDetails.Count() == 0)
            {
                return;
            }

            var rowNo = 0;
            var colIndex = 1;
            var grid = gdMachine;
            foreach (var subDetail in subDetails)
            {
                grid.InsertRow(rowIndex, 1);
                colIndex = 1;
                rowNo++;

                grid.Cell(rowIndex, colIndex++).Text = rowNo.ToString();
                grid.Cell(rowIndex, colIndex++).Text =
                    string.Format("{0} {1}", subDetail.Key.ResourceTypeName, subDetail.Key.ResourceTypeSpec);
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = false;

                var qty = subDetail.Sum(a => a.SumIncomeQuantity);
                var tp = subDetail.Sum(a => a.SumIncomeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumResponsiConsumeQuantity);
                tp = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumRealConsumeQuantity);
                tp = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumIncomeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0%";
                }

                qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex).Text = "0%";
                }

                rowIndex++;
            }

            #endregion

        }
        private void DisplayMachineData_Subject(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, string subsyscode, bool isWholeMatch)
        {
            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subsyscode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subsyscode));
            var colIndex = 3;
            var grid = gdMachine;

            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.SumIncomeQuantity).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.SumIncomeTotalPrice).ToString("N2");

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.SumResponsiConsumeQuantity).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.SumResponsiConsumeTotalPrice).ToString("N2");
            if (subjectCode == "C5110306")
                grid.Cell(rowIndex, colIndex).Tag = subjectCode + "|RespMoney";

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.SumRealConsumeQuantity).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.SumRealConsumeTotalPrice).ToString("N2");

            var tmp1 = subDetails.Sum(a => a.SumIncomeTotalPrice);
            var tmp2 = tmp1 - subDetails.Sum(a => a.SumRealConsumeTotalPrice);
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }

            tmp1 = subDetails.Sum(a => a.SumResponsiConsumeTotalPrice);
            tmp2 = tmp1 - subDetails.Sum(a => a.SumRealConsumeTotalPrice);
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }
        }

        private void DisplayMachineTotal()
        {
            var grid = gdMachine;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 3; j < grid.Cols; j++)
            {
                if (j == 4 || j == 7 || j == 10)
                {
                    continue;
                }
                else if (j == 13 || j == 15)
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 13 ? 5 : 8).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 4; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);

                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
        }

        #endregion

        #region 分包工程费对比表
        private void LoadSubcontractData()
        {
            var subCode = "C51104";
            if (!ifZB)
            {
                subCode = "C56104";
            }
            var subCodeSysCode = GetCostSubjectSyscode(subCode);
            var subList = GetCostSubjectList(subCode, 2, false);
            var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectSyscode.Contains(subCodeSysCode) && a.Data1 == "0");
            var rowIndex = 4;

            var lvOneSubs = subList.FindAll(a => a.Data2 == "1");
            var grid = gdSubcontrat;
            for (var i = 0; i < lvOneSubs.Count; i++)
            {
                grid.InsertRow(rowIndex, 1);
                grid.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(i + 1);
                grid.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;

                var subCodeSysCode_1 = GetCostSubjectSyscode(lvOneSubs[i].CostSubjectCode);

                DisplaySubcontractData_Subject(ref rowIndex, totalUses, subCodeSysCode_1, false);
                rowIndex++;
                DisplaySubcontractData_Resource(ref rowIndex, totalUses, subCodeSysCode_1, true);
                

                var lvTwoSubs =
                    subList.FindAll(a => a.Data2 == "2" && a.CostSubjectSyscode.Contains(subCodeSysCode_1));
                for (var j = 0; j < lvTwoSubs.Count; j++)
                {
                    grid.InsertRow(rowIndex, 1);
                    grid.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(65 + j) });
                    grid.Cell(rowIndex, 2).Text = lvTwoSubs[j].CostingSubjectName;
                    grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                    var subCodeSysCode_2 = GetCostSubjectSyscode(lvTwoSubs[j].CostSubjectCode);
                    DisplaySubcontractData_Subject(ref rowIndex, totalUses, subCodeSysCode_2, false);
                    rowIndex++;

                    DisplaySubcontractData_Resource(ref rowIndex, totalUses, subCodeSysCode_2, false);
                }
            }

            DisplaySubcontractTotal();

            SetGridStyle(grid, 3, 2);
        }

        private void DisplaySubcontractData_Resource(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => new { a.ResourceTypeName, a.ResourceTypeSpec })
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);
            if (subDetails.Count() == 0)
            {
                return;
            }

            var rowNo = 0;
            var colIndex = 1;
            var grid = gdSubcontrat;
            foreach (var subDetail in subDetails)
            {
                grid.InsertRow(rowIndex, 1);
                colIndex = 1;
                rowNo++;

                grid.Cell(rowIndex, colIndex++).Text = rowNo.ToString();
                grid.Cell(rowIndex, colIndex++).Text =
                    string.Format("{0} {1}", subDetail.Key.ResourceTypeName, subDetail.Key.ResourceTypeSpec);
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = false;

                var qty = subDetail.Sum(a => a.SumIncomeQuantity);
                var tp = subDetail.Sum(a => a.SumIncomeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumResponsiConsumeQuantity);
                tp = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumRealConsumeQuantity);
                tp = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumIncomeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0%";
                }

                qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex).Text = "0%";
                }

                rowIndex++;
            }

            #endregion
        }
        private void DisplaySubcontractData_Subject(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => new { a.ResourceTypeName, a.ResourceTypeSpec })
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);
            if (subDetails.Count() == 0)
            {
                return;
            }

            var rowNo = 0;
            var colIndex = 3;
            var grid = gdSubcontrat;
           
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice)).ToString("N2");

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice)).ToString("N2");

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice)).ToString("N2");

            var tmp1 = subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice));
            var tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }

            tmp1 = subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice));
            tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }
   
        }

        private void DisplaySubcontractTotal()
        {
            var grid = gdSubcontrat;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 3; j < grid.Cols; j++)
            {
                if (j == 4 || j == 7 || j == 10)
                {
                    continue;
                }
                else if (j == 13 || j == 15)
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 13 ? 5 : 8).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 4; i < grid.Rows - 1; i++)
                {
                   
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);
                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
        }
        #endregion

        #region 劳务费对比表
        private void LoadLabourvData()
        {
            var subCode = "C51101";
            if (!ifZB)
            {
                subCode = "C56101";
            }
            var subCodeSysCode = GetCostSubjectSyscode(subCode);
            var subList = GetCostSubjectList(subCode, 2, false);

            var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectSyscode.Contains(subCodeSysCode) && a.Data1 == "0");
            var rowIndex = 4;

            var lvOneSubs = subList.FindAll(a => a.Data2 == "1");
            var grid = gdLabour;
            for (var i = 0; i < lvOneSubs.Count; i++)
            {
                if (lvOneSubs[i].CostSubjectCode.Contains("C5110106"))
                {
                    continue;
                }

                grid.InsertRow(rowIndex, 1);
                grid.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(i + 1);
                grid.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                var subCodeSysCode_1 = GetCostSubjectSyscode(lvOneSubs[i].CostSubjectCode);

                DisplayLabourData_Subject(ref rowIndex, totalUses, subCodeSysCode_1, false);
                rowIndex++;


                var lvTwoSubs =
                    subList.FindAll(a => a.Data2 == "2" && a.CostSubjectSyscode.Contains(subCodeSysCode_1));
                for (var j = 0; j < lvTwoSubs.Count; j++)
                {
                    grid.InsertRow(rowIndex, 1);
                    grid.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(65 + j) });
                    grid.Cell(rowIndex, 2).Text = lvTwoSubs[j].CostingSubjectName;
                    grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                    var subCodeSysCode_2 = GetCostSubjectSyscode(lvTwoSubs[j].CostSubjectCode);
                    DisplayLabourData_Subject(ref rowIndex, totalUses, subCodeSysCode_2, false);
                    rowIndex++;

                    DisplayLabourData_Resource(ref rowIndex, totalUses, subCodeSysCode_2, false);
                }
            }

            DisplayLabourTotal();

            SetGridStyle(grid, 3, 3);
        }

        private void DisplayLabourData_Resource(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => new { a.ResourceTypeName, a.ResourceTypeSpec, a.RationUnitName })
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);
            if (subDetails.Count() == 0)
            {
                return;
            }

            var rowNo = 0;
            var colIndex = 1;
            var grid = gdLabour;
            foreach (var subDetail in subDetails)
            {
                grid.InsertRow(rowIndex, 1);
                colIndex = 1;
                rowNo++;

                grid.Cell(rowIndex, colIndex++).Text = rowNo.ToString();
                grid.Cell(rowIndex, colIndex++).Text =
                    string.Format("{0} {1}", subDetail.Key.ResourceTypeName, subDetail.Key.ResourceTypeSpec);
                grid.Cell(rowIndex, colIndex++).Text = subDetail.Key.RationUnitName;
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = false;

                var qty = subDetail.Sum(a => a.SumIncomeQuantity);
                var tp = subDetail.Sum(a => a.SumIncomeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumResponsiConsumeQuantity);
                tp = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumRealConsumeQuantity);
                tp = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumIncomeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0%";
                }

                qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex).Text = "0%";
                }

                rowIndex++;
            }

            #endregion

            #region 小计

           

            #endregion
        }

        private void DisplayLabourData_Subject(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            var subDetails = details.FindAll(
                   a =>
                   (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                   (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                   .GroupBy(a => new { a.ResourceTypeName, a.ResourceTypeSpec, a.RationUnitName })
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);

            var colIndex = 3;

            var grid = gdLabour;

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice)).ToString("N2");

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice)).ToString("N2");

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice)).ToString("N2");

            var tmp1 = subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice));
            var tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }

            tmp1 = subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice));
            tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }
        }

        private void DisplayLabourTotal()
        {
            var grid = gdLabour;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 4; j < grid.Cols; j++)
            {
                if (j == 5 || j == 8 || j == 11)
                {
                    continue;
                }
                else if (j == 14 || j == 16)
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 14 ? 6 : 9).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 4; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);

                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
        }
        #endregion

        #region 措施费对比表
        private void LoadMeasuresData()
        {
            var subCode = "C512";
            if (!ifZB)
            {
                subCode = "C562";
            }
            var grid = gdMeasures;
            var subList = GetCostSubjectList(subCode, 1, true);
            var totalUsesFirst = allTotalConsumes.FindAll(a => a.CostSubjectCode==subCode && a.Data1 == "0");
            var lvSelfSubs = subList.FindAll(a => a.Data2 == "0");
            var rowIndex = 4;
            grid.InsertRow(rowIndex, 1);
            grid.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(1);
            grid.Cell(rowIndex, 2).Text = lvSelfSubs[0].CostingSubjectName;
            grid.Cell(rowIndex, 2).Tag = lvSelfSubs[0].CostSubjectCode;
            grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
            var subCodeSysCode = GetCostSubjectSyscode(subCode);

            DisplayMeasuresData_Subject(ref rowIndex, totalUsesFirst, subCodeSysCode, true);
            rowIndex++;
            DisplayMeasuresData_Resource(ref rowIndex, totalUsesFirst, subCodeSysCode, true);

            var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectCode.Contains(subCode) && a.Data1 == "0");
            

            var lvOneSubs = subList.FindAll(a => a.Data2 == "1");
            
            for (var i = 0; i < lvOneSubs.Count; i++)
            {
                grid.InsertRow(rowIndex, 1);
                grid.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(i + 2);
                grid.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                grid.Cell(rowIndex, 2).Tag = lvOneSubs[i].CostSubjectCode;
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                var subCodeSysCode_1 = GetCostSubjectSyscode(lvOneSubs[i].CostSubjectCode);
                DisplayMeasuresData_Subject(ref rowIndex, totalUses, subCodeSysCode_1, false);
                rowIndex++;
                DisplayMeasuresData_Resource(ref rowIndex, totalUses, subCodeSysCode_1, true);

                var lvTwoSubs =
                    subList.FindAll(a => a.Data2 == "1" && a.CostSubjectCode.Contains(lvOneSubs[i].CostSubjectCode));
                for (var j = 0; j < lvTwoSubs.Count; j++)
                {
                    grid.InsertRow(rowIndex, 1);
                    grid.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(65 + j) });
                    grid.Cell(rowIndex, 2).Text = lvTwoSubs[j].CostingSubjectName;
                    grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                    var subCodeSysCode_2 = GetCostSubjectSyscode(lvTwoSubs[j].CostSubjectCode);

                    DisplayMeasuresData_Subject(ref rowIndex, totalUses, subCodeSysCode_2, false);
                    rowIndex++;
                    DisplayMeasuresData_Resource(ref rowIndex, totalUses, subCodeSysCode_2, false);
                }
            }

            DisplayMeasuresTotal();

            SetGridStyle(grid, 3, 2);
        }

        private void DisplayMeasuresData_Resource(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => new { a.ResourceTypeName, a.ResourceTypeSpec })
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);
            if (subDetails.Count() == 0 && !subjectCode.Equals("C51204") && !subjectCode.Equals("C51299"))
            {
                return;
            }

            var rowNo = 0;
            var colIndex = 1;
            var grid = gdMeasures;
            foreach (var subDetail in subDetails)
            {
                grid.InsertRow(rowIndex, 1);
                colIndex = 1;
                rowNo++;

                grid.Cell(rowIndex, colIndex++).Text = rowNo.ToString();
                grid.Cell(rowIndex, colIndex++).Text =
                    string.Format("{0} {1}", subDetail.Key.ResourceTypeName, subDetail.Key.ResourceTypeSpec);
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = false;

                var qty = subDetail.Sum(a => a.SumIncomeQuantity);
                var tp = subDetail.Sum(a => a.SumIncomeTotalPrice);
                
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumResponsiConsumeQuantity);
                tp = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumRealConsumeQuantity);
                tp = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumIncomeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0%";
                }

                qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex).Text = "0%";
                }

                rowIndex++;
            }

            #endregion
        }
        private void DisplayMeasuresData_Subject(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => new { a.ResourceTypeName, a.ResourceTypeSpec })
                .OrderBy(a => a.Key.ResourceTypeName)
                .ThenBy(a => a.Key.ResourceTypeSpec);
            if (subDetails.Count() == 0 && !subjectCode.Equals("C51204") && !subjectCode.Equals("C51299"))
            {
                return;
            }
            var colIndex = 3;
            var grid = gdMeasures;

            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice)).ToString("N2");



            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice)).ToString("N2");
            //****
            if (subjectCode == "C51299" || subjectCode == "C51204")
                grid.Cell(rowIndex, colIndex).Tag = subjectCode + "|RespMoney";


            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice)).ToString("N2");

            var tmp1 = subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice));
            var tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }

            tmp1 = subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice));
            tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }

            #endregion
        }
        private void DisplayMeasuresTotal()
        {
            var grid = gdMeasures;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 3; j < grid.Cols; j++)
            {
                if (j == 4 || j == 7 || j == 10)
                {
                    continue;
                }
                else if (j == 13 || j == 15)
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 13 ? 5 : 8).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 4; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);

                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
        }
        #endregion

        #region 现场管理费对比表
        private void LoadManagerData()
        {
            var subCode = "C513";
            if (!ifZB)
            {
                subCode = "C563";
            }
            var subCodeSysCode = GetCostSubjectSyscode(subCode);
            var subList = GetCostSubjectList(subCode, 2, true);
            var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectSyscode.Contains(subCodeSysCode) && a.Data1 == "0");
            var rowIndex = 3;

            var lvOneSubs = subList.FindAll(a => a.Data2 == "0" || a.Data2 == "1");
            var grid = gdManager;
            int rownum = 0;
            //grid.InsertRow(rowIndex, lvOneSubs.Count - 1);
            for (var i = 0; i < lvOneSubs.Count; i++)
            {
                grid.InsertRow(rowIndex, 1);
                if (lvOneSubs[i].CostSubjectCode == subCode)
                {
                    grid.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(i + 1);
                    grid.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                    grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                }
                else
                {
                    rownum++;
                    //grid.Cell(rowIndex, 1).Text = rownum.ToString();
                    grid.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(64 + rownum) });
                    grid.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                }
                var subCodeSysCode_1 = GetCostSubjectSyscode(lvOneSubs[i].CostSubjectCode);

                DisplayManagerData(ref rowIndex, totalUses, subCodeSysCode_1, false);

            }

            DisplayManagerTotal();

            SetGridStyle(grid, 2, 2);
        }

        private void DisplayManagerData(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode));
            if (subDetails.Count() == 0)
            {
                rowIndex++;
                return;
            }

            var colIndex = 3;
            var grid = gdManager;

            var tp = subDetails.Sum(a => a.SumIncomeTotalPrice);
            grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

            tp = subDetails.Sum(a => a.SumResponsiConsumeTotalPrice);
            grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
            if (subjectCode == "C513")
            {
                grid.Cell(rowIndex, colIndex-1).Tag = subjectCode + "|RespMoney";
            }

            tp = subDetails.Sum(a => a.SumRealConsumeTotalPrice);
            grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

            var qty = subDetails.Sum(a => a.SumIncomeTotalPrice);
            tp = qty - subDetails.Sum(a => a.SumRealConsumeTotalPrice);
            grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
            if (qty != 0)
            {
                grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, colIndex++).Text = "0%";
            }

            qty = subDetails.Sum(a => a.SumResponsiConsumeTotalPrice);
            tp = qty - subDetails.Sum(a => a.SumRealConsumeTotalPrice);
            grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
            if (qty != 0)
            {
                grid.Cell(rowIndex, colIndex).Text = Math.Round(tp / qty, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, colIndex).Text = "0%";
            }

            rowIndex++;
        }

        private void DisplayManagerTotal()
        {
            var grid = gdManager;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 3; j < grid.Cols; j++)
            {
                if (j == 7 || j == 9)
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 7 ? 3 : 4).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 3; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);
                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
        }
        #endregion

        #region 项目合同内成本分析汇总表
        private void LoadContractData()
        {
            gdContract.CellChange -= new Grid.CellChangeEventHandler(gdContract_CellChange);
            var rowIndex = 5;
            var scode = "";
            var contract = 0m;
            var respon = 0m;
            var real = 0m;
            gdContract.InsertRow(rowIndex, 1);
            gdContract.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(rowIndex - 4);
            gdContract.Cell(rowIndex, 2).Text = "劳务费";
            gdContract.Cell(rowIndex, 2).Tag = "C51101";
            //计时工单列出了
            scode = "C51101";
            contract =ClientUtil.ToDecimal(gdLabour.Cell(gdLabour.Rows - 1, 6).Text) + ClientUtil.ToDecimal(gdTime.Cell(gdTime.Rows - 1, 5).Text);
            respon = ClientUtil.ToDecimal(gdLabour.Cell(gdLabour.Rows - 1, 9).Text) + ClientUtil.ToDecimal(gdTime.Cell(gdTime.Rows - 1, 8).Text);
            real = ClientUtil.ToDecimal(gdLabour.Cell(gdLabour.Rows - 1, 12).Text) + ClientUtil.ToDecimal(gdTime.Cell(gdTime.Rows - 1, 11).Text);
            MaintaingdContractRow(rowIndex,scode, contract, respon, real);
            gdContract.Row(rowIndex).Locked = false;
            LockGridCellExcept(gdContract, rowIndex, new int[] { 6, 7 });
            rowIndex++;

            gdContract.InsertRow(rowIndex, 1);
            gdContract.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(rowIndex - 3);
            gdContract.Cell(rowIndex, 2).Text = "材料费";
            gdContract.Cell(rowIndex, 2).Tag = "C51102";
            scode = "C51102";
            contract = ClientUtil.ToDecimal(gdMaterial.Cell(gdMaterial.Rows - 1, 6).Text);
            respon = ClientUtil.ToDecimal(gdMaterial.Cell(gdMaterial.Rows - 1, 9).Text);
            real = ClientUtil.ToDecimal(gdMaterial.Cell(gdMaterial.Rows - 1, 15).Text);
            MaintaingdContractRow(rowIndex,scode, contract, respon, real);
            gdContract.Row(rowIndex).Locked = false;
            LockGridCellExcept(gdContract, rowIndex, new int[] { 6, 7 });
            rowIndex++;

            gdContract.InsertRow(rowIndex, 1);
            gdContract.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(rowIndex - 3);
            gdContract.Cell(rowIndex, 2).Text = "施工机械费";
            gdContract.Cell(rowIndex, 2).Tag = "C51103";
            scode = "C51103";
            contract = ClientUtil.ToDecimal(gdMachine.Cell(gdMachine.Rows - 1, 5).Text);
            respon = ClientUtil.ToDecimal(gdMachine.Cell(gdMachine.Rows - 1, 8).Text);
            real = ClientUtil.ToDecimal(gdMachine.Cell(gdMachine.Rows - 1, 11).Text);

            MaintaingdContractRow(rowIndex, scode, contract, respon, real);
            gdContract.Row(rowIndex).Locked = false;
            LockGridCellExcept(gdContract, rowIndex, new int[] { 6, 7 });
            rowIndex++;

            gdContract.InsertRow(rowIndex, 1);
            gdContract.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(rowIndex - 3);
            gdContract.Cell(rowIndex, 2).Text = "措施费";
            gdContract.Cell(rowIndex, 2).Tag = "C512";
      
            scode = "C512";
            contract = ClientUtil.ToDecimal(gdMeasures.Cell(gdMeasures.Rows - 1, 5).Text);
            respon = ClientUtil.ToDecimal(gdMeasures.Cell(gdMeasures.Rows - 1, 8).Text);
            real = ClientUtil.ToDecimal(gdMeasures.Cell(gdMeasures.Rows - 1, 11).Text);
            MaintaingdContractRow(rowIndex, scode, contract, respon, real);
            gdContract.Row(rowIndex).Locked = true;

            rowIndex++;

            var rowNo = 1;
            for (var i = 4; i < gdMeasures.Rows - 1; i++)
            {
                int strType = CheckStringType(gdMeasures.Cell(i, 1).Text);
                if (strType != 1)
                    continue;
                var subjCell = gdMeasures.Cell(i, 2);
                gdContract.InsertRow(rowIndex, 1);
                //gdContract.Cell(rowIndex, 1).Text = rowNo.ToString();
                gdContract.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(64 + rowNo) });
                gdContract.Cell(rowIndex, 2).Text = subjCell.Text;
                if (!string.IsNullOrEmpty(subjCell.Tag))
                {
                    scode = subjCell.Tag;
                    gdContract.Cell(rowIndex, 2).Tag = scode;
                    contract = ClientUtil.ToDecimal(gdMeasures.Cell(i, 5).Text);
                    respon = ClientUtil.ToDecimal(gdMeasures.Cell(i, 8).Text);
                    real = ClientUtil.ToDecimal(gdMeasures.Cell(i - 1, 11).Text);
                    MaintaingdContractRow(rowIndex, scode, contract, respon, real);
                }
                gdContract.Row(rowIndex).Locked = false;
                LockGridCellExcept(gdContract, rowIndex, new int[] { 6, 7 });
                rowIndex++;
                rowNo++;               
            }

            gdContract.InsertRow(rowIndex, 1);
            gdContract.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(rowIndex - rowNo - 2);
            gdContract.Cell(rowIndex, 2).Text = "现场管理费";
            gdContract.Cell(rowIndex, 2).Tag = "C513";
            scode = "C513";
            contract = ClientUtil.ToDecimal(gdManager.Cell(gdManager.Rows - 1, 3).Text);
            respon = ClientUtil.ToDecimal( gdManager.Cell(gdManager.Rows - 1, 4).Text);
            real = ClientUtil.ToDecimal(gdManager.Cell(gdManager.Rows - 1, 5).Text);
            MaintaingdContractRow(rowIndex, scode, contract, respon, real);
            gdContract.Row(rowIndex).Locked = false;
            LockGridCellExcept(gdContract, rowIndex, new int[] { 6, 7 });
            rowIndex++;

            gdContract.InsertRow(rowIndex, 1);
            gdContract.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(rowIndex - rowNo - 2);
            gdContract.Cell(rowIndex, 2).Text = "分包工程费";
            gdContract.Cell(rowIndex, 2).Tag = "C51104";
            scode = "C51104";
            contract = ClientUtil.ToDecimal(gdSubcontrat.Cell(gdSubcontrat.Rows - 1, 5).Text);
            respon = ClientUtil.ToDecimal(gdSubcontrat.Cell(gdSubcontrat.Rows - 1, 8).Text);
            real = ClientUtil.ToDecimal(gdSubcontrat.Cell(gdSubcontrat.Rows - 1, 11).Text);
            MaintaingdContractRow(rowIndex, scode, contract, respon, real);
            gdContract.Row(rowIndex).Locked = false;
            LockGridCellExcept(gdContract, rowIndex, new int[] { 6, 7 });
            rowIndex++;

            gdContract.InsertRow(rowIndex, 1);
            gdContract.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(rowIndex - rowNo - 2);
            gdContract.Cell(rowIndex, 2).Text = "其他";
            gdContract.Cell(rowIndex, 2).Tag = "QITA";

            scode = "QITA";
            contract = ClientUtil.ToDecimal(gdOther.Cell(gdOther.Rows - 1, 3).Text);
            respon = ClientUtil.ToDecimal(gdOther.Cell(gdOther.Rows - 1, 4).Text);
            real = ClientUtil.ToDecimal(gdOther.Cell(gdOther.Rows - 1, 5).Text);
            MaintaingdContractRow(rowIndex, scode, contract, respon, real);
            gdContract.Row(rowIndex).Locked = false;
            LockGridCellExcept(gdContract, rowIndex, new int[] { 6, 7 });
            rowIndex++;

            DisplayContractTotal();

            gdContract.CellChange += new Grid.CellChangeEventHandler(gdContract_CellChange);
            SetGridStyle(gdContract, 3, 2);
            
        }

        private void MaintaingdContractRow(int rowIndex, string scode, decimal contract, decimal respon, decimal real)
        {
            gdContract.Cell(rowIndex, 3).Text = contract.ToString("N2"); //合同收入
            gdContract.Cell(rowIndex, 4).Text = respon.ToString("N2"); //责任成本
            gdContract.Cell(rowIndex, 5).Text = real.ToString("N2"); //实际成本

            CostMonthReportChangeDetail cmrcd = FindOrAddEstimates(scode);
            gdContract.Cell(rowIndex, 6).Text = cmrcd.ChangeBudgetMoney.ToString("N2");//预估金额
            gdContract.Cell(rowIndex, 7).Text = cmrcd.ChangeRemark;//预估原因
            gdContract.Cell(rowIndex, 8).Text = (real + cmrcd.ChangeBudgetMoney).ToString("N2"); ;//预估后成本

            gdContract.Cell(rowIndex, 9).Text = (contract - real - cmrcd.ChangeBudgetMoney).ToString("N2");//合同收入-实际成本
            gdContract.Cell(rowIndex, 10).Text = contract == 0m ? "0%" : ((contract - real - cmrcd.ChangeBudgetMoney) / contract).ToString("P2");//合同收入-实际成本  节超比例
            gdContract.Cell(rowIndex, 11).Text = (respon - real - cmrcd.ChangeBudgetMoney).ToString("N2");//责任成本-实际成本
            gdContract.Cell(rowIndex, 12).Text = respon == 0m ? "0%" : ((respon - real - cmrcd.ChangeBudgetMoney) / respon).ToString("P2");//节超比例

        }

        private void DisplayContractTotal()
        {
            var grid = gdContract;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 3; j < gdContract.Cols; j++)
            {
                if (j == 7)
                    continue;
                if (j == 10 || j == 12)
                {
                    var tmp1 = gdContract.Cell(gdContract.Rows - 1, j - 1).DoubleValue;
                    var tmp2 = gdContract.Cell(gdContract.Rows - 1, j == 10 ? 3 : 4).DoubleValue;

                    if (tmp2 == 0)
                    {
                        gdContract.Cell(gdContract.Rows - 1, j).Text = "0%";
                    }
                    else
                    {
                        gdContract.Cell(gdContract.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                }
                else
                {
                    var total = 0m;
                    for (var i = 4; i < gdContract.Rows - 1; i++)
                    {
                        int strType = CheckStringType(grid.Cell(i, 1).Text);
                        if (strType != 1)
                            continue; 
                        total += Convert.ToDecimal(gdContract.Cell(i, j).DoubleValue);

                    }

                    gdContract.Cell(gdContract.Rows - 1, j).Text = total.ToString("N2");
                }
            }
            gdContract.Row(gdContract.Rows - 1).Locked = true;
            
        }

        #endregion

        #region 计时工分析对比表
        private void LoadTimeData()
        {
            var subCode = "C5110106";
            if (!ifZB)
            {
                subCode = "C5160106";
            }
            var subCodeSysCode = GetCostSubjectSyscode(subCode);
            var subList = GetCostSubjectList(subCode, 2, true);
            var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectSyscode.Contains(subCodeSysCode) && a.Data1 == "0");
            var rowIndex = 4;

            var lvOneSubs = subList.FindAll(a => a.CostSubjectSyscode == subCodeSysCode);
            var grid = gdTime;
            int seq = 0;
            for (var i = 0; i < lvOneSubs.Count; i++)
            {
                grid.InsertRow(rowIndex, 1);
                grid.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(seq + 1);
                seq++;

                grid.Cell(rowIndex, 2).Text = lvOneSubs[i].CostingSubjectName;
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                var subCodeSysCode_1 = GetCostSubjectSyscode(lvOneSubs[i].CostSubjectCode);

                DisplayTimeData_Subject(ref rowIndex, totalUses, subCodeSysCode_1, false);
                rowIndex++;



                var lvTwoSubs =
                    subList.FindAll(a => a.Data2 == "1" && a.CostSubjectSyscode.Contains(subCodeSysCode_1));
                for (var j = 0; j < lvTwoSubs.Count; j++)
                {
                    grid.InsertRow(rowIndex, 1);
                    grid.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(65 + j) });
                    grid.Cell(rowIndex, 2).Text = lvTwoSubs[j].CostingSubjectName;
                    grid.Range(rowIndex, 1, rowIndex, 2).FontBold = true;
                    var subCodeSysCode_2 = GetCostSubjectSyscode(lvTwoSubs[j].CostSubjectCode);

                    DisplayTimeData_Subject(ref rowIndex, totalUses, subCodeSysCode_2, false);
                    rowIndex++;

                }
            }

            DisplayTimeTotal();

            SetGridStyle(grid, 3, 2);
        }

        private void DisplayTimeData(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => a.CostingSubjectName )
                .OrderBy(a => a.Key)
                .ThenBy(a => a.Key);
            if (subDetails.Count() == 0)
            {
                return;
            }

            var rowNo = 0;
            var colIndex = 1;
            var grid = gdTime;
            foreach (var subDetail in subDetails)
            {
                grid.InsertRow(rowIndex, 1);
                colIndex = 1;
                rowNo++;

                grid.Cell(rowIndex, colIndex++).Text = rowNo.ToString();
                grid.Cell(rowIndex, colIndex++).Text = subDetail.Key;
                grid.Range(rowIndex, 1, rowIndex, 2).FontBold = false;

                var qty = subDetail.Sum(a => a.SumIncomeQuantity);
                var tp = subDetail.Sum(a => a.SumIncomeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumResponsiConsumeQuantity);
                tp = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumRealConsumeQuantity);
                tp = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = qty.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("N4");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0";
                }
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");

                qty = subDetail.Sum(a => a.SumIncomeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = "0%";
                }

                qty = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
                tp = qty - subDetail.Sum(a => a.SumRealConsumeTotalPrice);
                grid.Cell(rowIndex, colIndex++).Text = tp.ToString("N2");
                if (qty != 0)
                {
                    grid.Cell(rowIndex, colIndex).Text = Math.Round(tp / qty, 4).ToString("P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex).Text = "0%";
                }

                rowIndex++;
            }

            #endregion
        }
        private void DisplayTimeData_Subject(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            #region 按资源汇总明细行

            var subDetails = details.FindAll(
                a =>
                (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode))
                .GroupBy(a => a.CostingSubjectName)
                .OrderBy(a => a.Key)
                .ThenBy(a => a.Key);
            if (subDetails.Count() == 0)
            {
                return;
            }


            var colIndex = 3;
            var grid = gdTime;
   

            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice)).ToString("N2");

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice)).ToString("N2");

            grid.Cell(rowIndex, ++colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeQuantity)).ToString("N2");
            colIndex += 2;
            grid.Cell(rowIndex, colIndex).Text =
                subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice)).ToString("N2");

            var tmp1 = subDetails.Sum(a => a.Sum(b => b.SumIncomeTotalPrice));
            var tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }

            tmp1 = subDetails.Sum(a => a.Sum(b => b.SumResponsiConsumeTotalPrice));
            tmp2 = tmp1 - subDetails.Sum(a => a.Sum(b => b.SumRealConsumeTotalPrice));
            grid.Cell(rowIndex, ++colIndex).Text = tmp2.ToString("N2");
            if (tmp1 != 0)
            {
                grid.Cell(rowIndex, ++colIndex).Text = Math.Round(tmp2 / tmp1, 4).ToString("P2");
            }
            else
            {
                grid.Cell(rowIndex, ++colIndex).Text = "0";
            }


            #endregion
        }


        private void DisplayTimeTotal()
        {
            var grid = gdTime;
            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 3; j < grid.Cols; j++)
            {
                if (j == 4 || j == 7 || j == 10)
                {
                    continue;
                }
                else if (j == 13 || j == 15)
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 13 ? 5 : 8).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 4; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);

                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
        }
        #endregion

        #region 其他费对比表
        private void LoadOtherData()
        {
            var subCodes = new List<string>() { "C514", "C515", "C516", "C520" };
            var rowIndex = 3;
            int ChineseNumber = 1;
            foreach (var subCode in subCodes)
            {
                var subCodeSysCode = GetCostSubjectSyscode(subCode);
                var totalUses = allTotalConsumes.FindAll(a => a.CostSubjectSyscode.Contains(subCodeSysCode) && a.Data1 == "0");   
                var sub = subjectList.Find(s => s.Code == subCode);
                if (sub == null)
                    continue;
                var subList = GetCostSubjectList(subCode, 2, false);
                gdOther.InsertRow(rowIndex, 1);
                gdOther.Cell(rowIndex, 1).Text = CommonUtil.GetChineseNumber(ChineseNumber++);
                gdOther.Cell(rowIndex, 2).Text = sub.Name;
                gdOther.Range(rowIndex, 1, rowIndex, 2).FontBold = true;

                DisplayOtherData(ref rowIndex, totalUses, subCodeSysCode, false);
                rowIndex++;

                var lvTwoSubs =
                    subList.FindAll(a => a.Data2 == "1" && a.CostSubjectSyscode.Contains(subCodeSysCode));
                for (var j = 0; j < lvTwoSubs.Count; j++)
                {
                    gdOther.InsertRow(rowIndex, 1);
                    gdOther.Cell(rowIndex, 1).Text = Encoding.ASCII.GetString(new byte[] { (byte)(65 + j) });
                    gdOther.Cell(rowIndex, 2).Text = lvTwoSubs[j].CostingSubjectName;
                    gdOther.Range(rowIndex, 1, rowIndex, 2).FontBold = false;

                    var subCodeSysCode_2 = GetCostSubjectSyscode(lvTwoSubs[j].CostSubjectCode);
                    DisplayOtherData(ref rowIndex, totalUses, subCodeSysCode_2, false);
                    rowIndex++;   
                }
            }
            DisplayOtherTotal();
            SetGridStyle(gdOther, 2, 2);
        }

        private void DisplayOtherData(ref int rowIndex, List<CostMonthAccDtlConsume> details, string subjectCode, bool isWholeMatch)
        {
            var subDetail = details.FindAll(a => (!isWholeMatch && a.CostSubjectSyscode.Contains(subjectCode)) ||
                (isWholeMatch && a.CostSubjectSyscode == subjectCode));
            if (subDetail.Count() == 0)
                return;

              CustomFlexGrid grid = gdOther;
              var colIndex = 3;
              var contactMoney = subDetail.Sum(a => a.SumIncomeTotalPrice);
              var responseMoney = subDetail.Sum(a => a.SumResponsiConsumeTotalPrice);
              var realMoney = subDetail.Sum(a => a.SumRealConsumeTotalPrice);
              grid.Cell(rowIndex, colIndex++).Text = contactMoney.ToString("N2");  //合同收入
              grid.Cell(rowIndex, colIndex++).Text = responseMoney.ToString("N2");//
              if (subjectCode == "C514")
                  gdOther.Cell(rowIndex, colIndex).Tag = subjectCode + "|RespMoney";
              grid.Cell(rowIndex, colIndex++).Text = realMoney.ToString("N2");
              grid.Cell(rowIndex, colIndex++).Text = (contactMoney - realMoney).ToString("N2");
              grid.Cell(rowIndex, colIndex++).Text = contactMoney == 0 ? "0%" : ((contactMoney - realMoney) / contactMoney).ToString("P2");
              grid.Cell(rowIndex, colIndex++).Text = (responseMoney - realMoney).ToString("N2");
              grid.Cell(rowIndex, colIndex++).Text = responseMoney == 0 ? "0%" : ((responseMoney - realMoney) / responseMoney).ToString("P2");

        }

        private void DisplayOtherTotal()
        {
            CustomFlexGrid grid = gdOther;

            var cell = grid.Cell(grid.Rows - 1, 2);
            cell.Text = "合计";
            cell.FontBold = true;

            for (int j = 3; j < grid.Cols; j++)
            {
                if (j == 7 || j == 9)
                {
                    var tmp1 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j - 1).DoubleValue);
                    var tmp2 = Convert.ToDecimal(grid.Cell(grid.Rows - 1, j == 7 ? 3 : 4).DoubleValue);
                    if (tmp2 != 0)
                    {
                        grid.Cell(grid.Rows - 1, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                    else
                    {
                        grid.Cell(grid.Rows - 1, j).Text = "0%";
                    }
                    continue;
                }

                var total = 0m;
                for (int i = 3; i < grid.Rows - 1; i++)
                {
                    int strType = CheckStringType(grid.Cell(i, 1).Text);
                    if (strType != 1)
                        continue;
                    total += Convert.ToDecimal(grid.Cell(i, j).DoubleValue);

                }

                grid.Cell(grid.Rows - 1, j).Text = total.ToString("N2");
            }
        }
        #endregion

        #region 项目合同外变更费用汇总表

        #endregion

        #region 项目签证索赔费用对比表

        #endregion

        #region 项目成本分析汇总表
        private void LoadTotalData()
        {
            gdTotal.Cell(2, 1).Text = string.Format("项目名称：{0}", ProjectInfo.Name);
            var rowIndex = 5;
            gdTotal.Cell(rowIndex, 3).Text = gdContract.Cell(gdContract.Rows - 1, 3).Text;
            gdTotal.Cell(rowIndex, 4).Text = gdContract.Cell(gdContract.Rows - 1, 4).Text;
            gdTotal.Cell(rowIndex, 5).Text = gdContract.Cell(gdContract.Rows - 1, 5).Text;
            gdTotal.Cell(rowIndex, 6).Text = gdContract.Cell(gdContract.Rows - 1, 9).Text;
            gdTotal.Cell(rowIndex, 7).Text = gdContract.Cell(gdContract.Rows - 1, 10).Text;
            gdTotal.Cell(rowIndex, 8).Text = gdContract.Cell(gdContract.Rows - 1, 11).Text;
            gdTotal.Cell(rowIndex, 9).Text = gdContract.Cell(gdContract.Rows - 1, 12).Text;
            rowIndex++;

            gdTotal.Cell(rowIndex, 3).Text = gdChange.Cell(gdChange.Rows - 1, 7).Text;
            gdTotal.Cell(rowIndex, 4).Text = gdChange.Cell(gdChange.Rows - 1, 11).Text;
            gdTotal.Cell(rowIndex, 5).Text = gdChange.Cell(gdChange.Rows - 1, 15).Text;
            gdTotal.Cell(rowIndex, 6).Text = gdChange.Cell(gdChange.Rows - 1, 16).Text;
            gdTotal.Cell(rowIndex, 7).Text = gdChange.Cell(gdChange.Rows - 1, 17).Text;
            gdTotal.Cell(rowIndex, 8).Text = gdChange.Cell(gdChange.Rows - 1, 18).Text;
            gdTotal.Cell(rowIndex, 9).Text = gdChange.Cell(gdChange.Rows - 1, 19).Text;
            rowIndex++;

            gdTotal.Cell(rowIndex, 3).Text = gdTime.Cell(gdTime.Rows - 1, 5).Text;
            gdTotal.Cell(rowIndex, 4).Text = gdTime.Cell(gdTime.Rows - 1, 8).Text;
            gdTotal.Cell(rowIndex, 5).Text = gdTime.Cell(gdTime.Rows - 1, 11).Text;
            gdTotal.Cell(rowIndex, 6).Text = gdTime.Cell(gdTime.Rows - 1, 12).Text;
            gdTotal.Cell(rowIndex, 7).Text = gdTime.Cell(gdTime.Rows - 1, 13).Text;
            gdTotal.Cell(rowIndex, 8).Text = gdTime.Cell(gdTime.Rows - 1, 14).Text;
            gdTotal.Cell(rowIndex, 9).Text = gdTime.Cell(gdTime.Rows - 1, 15).Text;
            rowIndex++;

            gdTotal.Cell(rowIndex, 3).Text = gdVisa.Cell(gdVisa.Rows - 1, 3).Text;
            gdTotal.Cell(rowIndex, 4).Text = gdVisa.Cell(gdVisa.Rows - 1, 4).Text;
            gdTotal.Cell(rowIndex, 5).Text = gdVisa.Cell(gdVisa.Rows - 1, 9).Text;
            gdTotal.Cell(rowIndex, 6).Text = gdVisa.Cell(gdVisa.Rows - 1, 10).Text;
            gdTotal.Cell(rowIndex, 7).Text = gdVisa.Cell(gdVisa.Rows - 1, 11).Text;
            gdTotal.Cell(rowIndex, 8).Text = gdVisa.Cell(gdVisa.Rows - 1, 12).Text;
            gdTotal.Cell(rowIndex, 9).Text = gdVisa.Cell(gdVisa.Rows - 1, 13).Text;

            for (int j = 3; j < gdTotal.Cols; j++)
            {
                if (j == 7 || j == 9)
                {
                    var tmp1 = gdTotal.Cell(gdTotal.Rows - 2, j - 1).DoubleValue;
                    var tmp2 = gdTotal.Cell(gdTotal.Rows - 2, j == 7 ? 3 : 4).DoubleValue;

                    if (tmp2 == 0)
                    {
                        gdTotal.Cell(gdTotal.Rows - 2, j).Text = "0%";
                    }
                    else
                    {
                        gdTotal.Cell(gdTotal.Rows - 2, j).Text = Math.Round(tmp1 / tmp2, 4).ToString("P2");
                    }
                }
                else
                {
                    var total = 0m;
                    for (var i = 5; i < gdTotal.Rows - 2; i++)
                    {
                        total += Convert.ToDecimal(gdTotal.Cell(i, j).DoubleValue);
                    }

                    gdTotal.Cell(gdTotal.Rows - 2, j).Text = total.ToString("N2");
                }
            }

            SetGridStyle(gdTotal, 5, 2);
        }
        #endregion

        #region 修正固定费率项的 责任成本
        private void MaintainFixedRateRsp()
        {
            //其他总价措施费
            MaintainFixedRateRsp(gdMeasures, "C51299", 8);
            MaintainRepSubReal(gdMeasures, "C51299", 8, 11, 14, 15);
  
           
            //临时建设费
            MaintainFixedRateRsp(gdMeasures, "C51204", 8);
            MaintainRepSubReal(gdMeasures, "C51204", 8, 11, 14, 15);
            MaintainRepSubReal(gdMeasures, gdMeasures.Rows - 1, 8, 11, 14, 15);
           

            //规费
            MaintainFixedRateRsp(gdOther, "C514", 4);
            MaintainRepSubReal(gdOther, "C514", 4,5,8,9);
            MaintainRepSubReal(gdOther, gdOther.Rows-1, 4, 5, 8, 9);

            //管理费
            MaintainFixedRateRsp(gdManager, "C513", 4);
            MaintainRepSubReal(gdManager, "C513", 4, 5, 8, 9);
            MaintainRepSubReal(gdManager, gdManager.Rows - 1, 4, 5, 8, 9);


            //电费
            MaintainFixedRateRsp(gdMachine, "C5110306", 8);
            MaintainRepSubReal(gdMachine, "C5110306", 8, 11, 14, 15);
            MaintainRepSubReal(gdMachine, gdMachine.Rows - 1, 8, 11, 14, 15);

            //LoadFlexFile(string.Concat(this.gdTotal.Tag, ".flx"), this.gdTotal);
            //LoadFlexFile(string.Concat(this.gdContract.Tag, ".flx"), this.gdContract);

            //LoadContractData();
            //LoadTotalData();
        }
        /// <summary>
        /// 维护合同收入固定费率的责任成本 并修改合计行的责任成本汇总
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="subCode"></param>
        /// <param name="colIndex"></param>
        private void MaintainFixedRateRsp(CustomFlexGrid grid, string subCode, int colIndex)
        {
            int rowindex = -1;

            //全部合同收入
            decimal sumHTIncom = ClientUtil.ToDecimal(gdTotal.Cell(gdTotal.Rows - 2, 3).Text);

            decimal dec_fixRate = 0;
            switch (subCode)
            {
                case "C51299"://其他总价措施费
                    dec_fixRate = ProjectInfo.MeasuresFeeRatio;
                    break;
                case "C51204"://临时建设费
                    dec_fixRate = ProjectInfo.TConstructionRatio;
                    break;
                case "C514"://规费
                    dec_fixRate = ProjectInfo.FeesRatio;
                    break;
                case "C513"://管理费
                    dec_fixRate = ProjectInfo.ManagementFeeRatio;
                    break;
                case "C5110306"://电费
                    dec_fixRate = ProjectInfo.ElectricRatio;
                    break;
            }

            rowindex = FindRowIndex(grid, subCode, colIndex);
            if (rowindex > -1)
            {
                string dec = (sumHTIncom * dec_fixRate).ToString("N2");
                grid.Cell(rowindex, colIndex).Text = dec; //修改当前行的责任成本
                //修改合计行的责任成本
                grid.Cell(grid.Rows - 1, colIndex).Text = (ClientUtil.ToDecimal(grid.Cell(grid.Rows - 1, colIndex).Text) + ClientUtil.ToDecimal(dec)).ToString("N2");
            }
        }

        /// <summary>
        /// 维护【责任成本-实际成本】以及【责任成本节超比率】
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="subCode"></param>
        /// <param name="colRep">责任成本列</param>
        /// <param name="colReal">实际成本列</param>
        /// <param name="colRsR">责任成本-实际成本</param>
        /// <param name="colRsRRate">责任成本节超比率</param>
        private void MaintainRepSubReal(CustomFlexGrid grid, string subCode,  int colRep, int colReal, int colRsR, int colRsRRate)
        {
            int rowindex = -1;
            rowindex = FindRowIndex(grid, subCode, colRep);
            MaintainRepSubReal(grid, rowindex, colRep, colReal, colRsR, colRsRRate);

        }
        private void MaintainRepSubReal(CustomFlexGrid grid, int rowindex, int colRep, int colReal, int colRsR, int colRsRRate)
        {

            if (rowindex > -1)
            {
                decimal dec_rep = ClientUtil.ToDecimal(grid.Cell(rowindex, colRep).Text);
                decimal dec_real = ClientUtil.ToDecimal(grid.Cell(rowindex, colReal).Text);
                grid.Cell(rowindex, colRsR).Text = (dec_rep - dec_real).ToString("N2");
                grid.Cell(rowindex, colRsRRate).Text = dec_rep == 0 ? "0%" : Math.Round((dec_rep - dec_real) / dec_rep, 4).ToString("P2");
            }

        }
        private int FindRowIndex(CustomFlexGrid grid, string subCode , int col)
        {
            string strTag = subCode + "|RespMoney";
            for (int i = 0; i < grid.Rows; i++)
            {
                string tag = ClientUtil.ToString(grid.Cell(i, col).Tag);
                if (strTag == tag)
                    return i;
            }
            return -1;
        }
        #endregion

        #region 
        void gdMaterial_CellChange(object Sender, Grid.CellChangeEventArgs e)
        {
            if (e.Col != 11 && e.Col != 12)
                return;
            CustomFlexGrid grid = gdMaterial;
            string strTag = grid.Cell(e.Row, 2).Tag as string;
            if (!string.IsNullOrEmpty(strTag) && strTag.Split('|').Length == 4)
            {
                string scode = strTag.Split('|')[0];
                string rname = strTag.Split('|')[1];
                string rspec = strTag.Split('|')[2];
                string runit = strTag.Split('|')[3];

                CostMonthReportChangeDetail cmrcd = FindOrAddChangeDiffs(scode, rname, rspec, runit);

                if (e.Col == 12)
                {
                    cmrcd.ChangeRemark = grid.Cell(e.Row, 12).Text;
                }
                else
                {
                    if(!IsNumber(grid.Cell(e.Row, 11).Text.Trim()))
                    {
                        MessageBox.Show("请输入数字！");
                        return;
                    }
                    cmrcd.ChangeQty = ClientUtil.ToDecimal(grid.Cell(e.Row, 11).Text.Trim());
                    var qty = ClientUtil.ToDecimal(grid.Cell(e.Row, 10).Text.Trim());
                    var price = ClientUtil.ToDecimal(grid.Cell(e.Row, 14).Text.Trim());
                    var realMoney = ClientUtil.ToDecimal(grid.Cell(e.Row, 15).Text.Trim());
                    var qtyAfter = qty + cmrcd.ChangeQty;
                    var realAfter = realMoney + cmrcd.ChangeQty * price;
                    var contractMoney = ClientUtil.ToDecimal(grid.Cell(e.Row, 6).Text.Trim());
                    var responseMoney = ClientUtil.ToDecimal(grid.Cell(e.Row, 9).Text.Trim());
                    grid.Cell(e.Row, 13).Text = qtyAfter.ToString("N2");//调整后数量
                    grid.Cell(e.Row, 15).Text = realAfter.ToString("N2");
                    grid.Cell(e.Row, 16).Text = (contractMoney - realAfter).ToString("N2");
                    grid.Cell(e.Row, 17).Text = contractMoney == 0m ? "0%" : ((contractMoney - realAfter) / contractMoney).ToString("P2");
                    grid.Cell(e.Row, 18).Text = (responseMoney - realAfter).ToString("N2");
                    grid.Cell(e.Row, 19).Text = responseMoney == 0m ? "0%" : ((responseMoney - realAfter) / responseMoney).ToString("P2");

                    gdMaterial.CellChange -= new Grid.CellChangeEventHandler(gdMaterial_CellChange);
                    MaintainCostMonthReportChangeDetailSum(grid, ChangeSolutionType.ChangeDiff);
                    DisplayMaterialTotal();
                    LoadFlexFile(string.Concat(this.gdContract.Tag, ".flx"), this.gdContract);
                    LoadContractData();
                    LoadFlexFile(string.Concat(this.gdTotal.Tag, ".flx"), this.gdTotal);
                    LoadTotalData();
                    gdMaterial.CellChange += new Grid.CellChangeEventHandler(gdMaterial_CellChange);

                }
                
            }       
            
        }

        private bool IsNumber(string txt)
        {
            bool b_rtn = false;
            try
            {
                Convert.ToDecimal(txt);
                b_rtn = true;
            }
            catch 
            {
                
            }
            return b_rtn;

        }


        void gdContract_CellChange(object Sender, Grid.CellChangeEventArgs e)
        {
           if (e.Col != 6 && e.Col != 7)
                return;
           CustomFlexGrid grid = gdContract;
            string strTag = grid.Cell(e.Row, 2).Tag as string;
            if (!string.IsNullOrEmpty(strTag))
            {
                string scode = strTag;
                CostMonthReportChangeDetail cmrcd = FindOrAddEstimates(scode);

                if (e.Col == 7)
                {
                    cmrcd.ChangeRemark = grid.Cell(e.Row, 12).Text;
                }
                else
                {
                    if(!IsNumber(grid.Cell(e.Row, 6).Text.Trim()))
                    {
                        MessageBox.Show("请输入数字！");
                        return;
                    }
                    cmrcd.ChangeBudgetMoney = ClientUtil.ToDecimal(grid.Cell(e.Row, 6).Text.Trim());
                    var realMoney = ClientUtil.ToDecimal(grid.Cell(e.Row, 5).Text.Trim());
                    grid.Cell(e.Row, 8).Text = (realMoney + cmrcd.ChangeBudgetMoney).ToString("N2");//调整后

                    gdContract.CellChange -= new Grid.CellChangeEventHandler(gdContract_CellChange);
                    MaintainCostMonthReportChangeDetailSum(grid, ChangeSolutionType.Estimate);
                   
                    DisplayContractTotal();
                    LoadFlexFile(string.Concat(this.gdTotal.Tag, ".flx"), this.gdTotal);
                    LoadTotalData();
                    gdContract.CellChange += new Grid.CellChangeEventHandler(gdContract_CellChange);

                }
                
            }  
        }

        private CostMonthReportChangeMatser RetriveCostMonthReportChangeMatser(int year, int month)
        {
            if (CurBillMaster != null) return CurBillMaster;
            //取本月
            CostMonthReportChangeMatser curr = subject.Mm.GetCostMonthReportChangeMatser(ProjectInfo.Id, year, month);
            if (curr != null)
            {
                curr.CreateDate = DateTime.Now;
                curr.CreatePerson = ConstObject.LoginPersonInfo;
                curr.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curr.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                curr.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                curr.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                return curr;
            }
            //本月没有取上月
            DateTime date = new DateTime(year, month, 1).AddMonths(-1);
            int l_year = date.Year;
            int l_month = date.Month;
            curr = subject.Mm.GetCostMonthReportChangeMatser(ProjectInfo.Id, l_year, l_month);
            if (curr != null)
            {
                return CopyNewCostMonthReportChangeMatser(curr, year, month);
            }
            //上月没有，新建
            else
                return CreateCostMonthReportChangeMatser(year, month);
        }

        private CostMonthReportChangeMatser CopyNewCostMonthReportChangeMatser(CostMonthReportChangeMatser curr, int year, int month)
        {
            CostMonthReportChangeMatser rtn = new CostMonthReportChangeMatser();
            rtn.ProjectId = curr.ProjectId;
            rtn.ProjectName = curr.ProjectName;
            rtn.CreateYear = year;
            rtn.CreateMonth = month;
            rtn.CreateDate = DateTime.Now;
            rtn.CreatePerson = ConstObject.LoginPersonInfo;
            rtn.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            rtn.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            rtn.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
            rtn.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            rtn.RealOperationDate = DateTime.Now;

            foreach (CostMonthReportChangeDetail item in curr.Details)
            {
                CostMonthReportChangeDetail dtl = new CostMonthReportChangeDetail();
                dtl.Master = rtn;
                dtl.CostSubjectCode = item.CostSubjectCode;
                dtl.ResourceTypeGUID = item.ResourceTypeGUID;
                dtl.ResourceTypeName = item.ResourceTypeName;
                dtl.ResourceTypeSpec = item.ResourceTypeSpec;
                dtl.RationUnitName = item.RationUnitName;
                dtl.ChangeQty = item.ChangeQty;
                dtl.ChangeBudgetMoney = item.ChangeBudgetMoney;
                dtl.ChangeRemark = item.ChangeRemark;
                dtl.ChangeType = item.ChangeType;
                rtn.Details.Add(dtl);
            }
            return rtn;

        }

        private CostMonthReportChangeMatser CreateCostMonthReportChangeMatser(int year, int month)
        {
            CostMonthReportChangeMatser rtn = new CostMonthReportChangeMatser();
            rtn.ProjectId = ProjectInfo.Id;
            rtn.ProjectName = ProjectInfo.Name;
            rtn.CreateYear = year;
            rtn.CreateMonth = month;
            rtn.CreateDate = DateTime.Now;
            rtn.CreatePerson = ConstObject.LoginPersonInfo;
            rtn.CreatePersonName = ConstObject.LoginPersonInfo.Name;
            rtn.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            rtn.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
            rtn.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            rtn.RealOperationDate = DateTime.Now;
            return rtn;
        }

        public override bool SaveView()
        {
            try
            {
                LogData log = new LogData();
                FlashScreen.Show(string.Format("正在保存调差和预估成本，请稍候..."));
                CurBillMaster = subject.Mm.SaveCostMonthReportChangeMatser(CurBillMaster);
                log.BillId = CurBillMaster.Id;
                log.BillType = "调差表";
                log.Code = "";
                log.Descript = CurBillMaster.Descript;
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = CurBillMaster.ProjectName;
                StaticMethod.InsertLogData(log);
                FlashScreen.Close();
                return true;
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("保存出错。\n" + ex.Message);
            }
            return false;
        }

       

        private CostMonthReportChangeDetail FindOrAddEstimates(string subcode)
        {

            CostMonthReportChangeDetail costDtl = CurBillMaster.Details.OfType<CostMonthReportChangeDetail>().ToList().Find(a => a.ChangeType == ChangeSolutionType.Estimate && a.CostSubjectCode == subcode);
            if (costDtl == null)
            {
                costDtl = new CostMonthReportChangeDetail();
                costDtl.Master = CurBillMaster;
                costDtl.CostSubjectCode = subcode;
                costDtl.ChangeType = ChangeSolutionType.Estimate;
                costDtl.ChangeBudgetMoney = 0;
                CurBillMaster.Details.Add(costDtl);
            }
            return costDtl;
        }
        private CostMonthReportChangeDetail FindOrAddChangeDiffs(string subcode, string ResourceTypeName, string ResourceTypeSpec, string RationUnitName)
        {

            CostMonthReportChangeDetail costDtl = CurBillMaster.Details.OfType<CostMonthReportChangeDetail>().ToList().Find(a => a.ChangeType == ChangeSolutionType.ChangeDiff 
                                                                                                                            && ((subcode == "" && a.CostSubjectCode  == null) || a.CostSubjectCode == subcode) 
                                                                                                                            && ((ResourceTypeName == "" && a.ResourceTypeName == null )||a.ResourceTypeName == ResourceTypeName )
                                                                                                                            && ((ResourceTypeSpec == "" && a.ResourceTypeSpec == null) || a.ResourceTypeSpec == ResourceTypeSpec)
                                                                                                                            && ((RationUnitName == "" && a.RationUnitName == null) || a.RationUnitName == RationUnitName)
                                                                                                                         
                                                                                                                            );
            if (costDtl == null)
            {
                costDtl = new CostMonthReportChangeDetail();
                costDtl.Master = CurBillMaster;
                costDtl.CostSubjectCode = subcode;
                costDtl.ResourceTypeName = ResourceTypeName;
                costDtl.ResourceTypeSpec = ResourceTypeSpec;
                costDtl.RationUnitName = RationUnitName;
                costDtl.ChangeType = ChangeSolutionType.ChangeDiff;
                costDtl.ChangeQty = 0;
                CurBillMaster.Details.Add(costDtl);
            }
            return costDtl;
        }

        void btn_Save_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确定要保存当前的的记录吗？", "保存记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            //{ 
            
            //}
            SaveView();
        }

        #endregion
    }
}